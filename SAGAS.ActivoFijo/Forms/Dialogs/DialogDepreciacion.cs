using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.ActivoFijo.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogDepreciacion : Form
    { 
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormDepreciacion MDI;
        private List<ObjectListSubEstacion> ListSubEstacion = new List<ObjectListSubEstacion>();
        internal Entidad.Movimiento EntidadAnterior;
        internal bool ShowMsg;
        internal decimal _TipoCambio;
        private int UsuarioID;        
        private bool NextDepre = false;
        private bool RefreshMDI = false;
        private bool _Guardado = false;
        private bool _ToPrint = false;
        private int IDMonedaPrincipal;
        private int IDPrint = 0;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        internal decimal vTotalPagar = 0m;
        internal int _MonedaPrimaria;
        internal int _MonedaSecundaria;
        internal List<Parametros.ListIdDisplay> EtEstacion;
        internal List<Parametros.ListIdCodeDisplay> EtSubEstacion;


        private int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkEs.EditValue); }
            set { lkEs.EditValue = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSus.EditValue); }
            set { lkSus.EditValue = value; }
        }

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFechaDepre.EditValue); }
            set { dateFechaDepre.EditValue = value; }
        }

        private string Numero 
        {
            get { return txtNoFactura.Text; }
            set { txtNoFactura.Text = value; }
        
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }
        
        
        //{
        //    get { return Convert.ToInt32(lkMoneda.EditValue); }
        //    set { lkMoneda.EditValue = value; }
        //}

        private List<Entidad.VistaDepreciacion> D = new List<Entidad.VistaDepreciacion>();
        public List<Entidad.VistaDepreciacion> DetalleD
        {
            get { return D; }
            set
            {
                D = value;
                this.bdsDetalle.DataSource = this.D;
            }
        }

        #endregion


        public DialogDepreciacion(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;

        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            
        }

        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();           
        }

        #region *** METODOS ***
        
        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _MonedaSecundaria = Parametros.Config.MonedaSecundaria();
                IDMonedaPrincipal = _MonedaPrimaria;
                Fecha = Convert.ToDateTime(db.GetDateServer()).Date;

                //Estaciones
                EtEstacion = db.EstacionServicios.Where(o => o.Activo).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre }).ToList();
                lkEs.Properties.DataSource = EtEstacion;               
                //Sub Estaciones
                EtSubEstacion = db.SubEstacions.Where(o => o.Activo).Select(s => new Parametros.ListIdCodeDisplay { ID = s.ID, Code = s.EstacionServicioID, Display = s.Nombre }).ToList();
                lkSus.Properties.DataSource = EtSubEstacion;
                IDEstacionServicio = Parametros.General.EstacionServicioID;

                //Areas <> CECOS
                rpLkAreaID.DataSource = db.CentroCostos.Where(c => c.Activo).Select(s => new { s.ID, s.Nombre });
                
                Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
                                          
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNoFactura, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
        }

        private bool ValidarReferencia(string code, int? ID)
        {
            //var result = (from i in db.Movimientos
            //              where  (ID.HasValue ? i.Referencia.Equals(code) && !i.ID.Equals(ID) && i.ProveedorID.Equals(IDSujeto) && !i.Anulado : i.Referencia.Equals(code) && i.ProveedorID.Equals(IDSujeto) && !i.Anulado)
            //              select i);

            //if (result.Count() > 0)
            //{
                return false;
            //}
            //return true;
        }

        public bool ValidarCampos(bool detalle)
        {
            if (dateFechaDepre.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtNoFactura.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del documento.", Parametros.MsgType.warning);
                return false;
            }
            
            if (!Parametros.General.ValidateTipoCambio(dateFechaDepre, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }
                      
            if (Parametros.General.ListSES.Count > 0)
            {
                if (IDSubEstacion <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (detalle)
            {
                if (DetalleD.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
                
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false))
                return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M;
                    M = new Entidad.Movimiento();
                    M.MovimientoTipoID = 75;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.MonedaID = IDMonedaPrincipal;
                    M.TipoCambio = _TipoCambio;
                    M.UsuarioID = UsuarioID;
                    M.FechaRegistro = Fecha;

                    int number = 1;
                    var Mov = db.Movimientos.Select(s => new { s.EstacionServicioID, s.SubEstacionID, s.MovimientoTipoID, s.Numero }).Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(75)).OrderByDescending(o => o.Numero).FirstOrDefault();
                    if (Mov != null)
                    {
                        number = Parametros.General.ValorConsecutivo(Mov.Numero.ToString());
                    }

                    M.Numero = number;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Decimal.Round(DetalleD.Sum(s => s.ValorDepreciacion), 2, MidpointRounding.AwayFromZero);
                    M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(DetalleD.Sum(s => s.ValorDepreciacion)) / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                    M.MonedaID = _MonedaPrimaria;

                    db.Movimientos.InsertOnSubmit(M);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                    "Se registró la Depreciación : " + M.Numero.ToString("000000000"), this.Name);

                    db.SubmitChanges();

                    #region <<< Registrando Detalle >>>
                    foreach (var obj in DetalleD)
                    {
                        if (obj.ValorDepreciacion > 0 || obj.NumeroDepreciacion > 0)
                        {
                            Entidad.Depreciacion D;

                            D = new Entidad.Depreciacion();

                            D.BienID = obj.BienID;
                            D.ValorAdquisicion = obj.ValorAdquisicion;
                            D.VidaUtilMeses = obj.VidaUtilMeses;
                            D.ValorDepreciacion = obj.ValorDepreciacion;
                            D.NumeroDepreciacion = obj.NumeroDepreciacion;
                            D.MovimientoID = M.ID;
                            D.ValorDepreciacionAcumulada = obj.ValorDepreActual + obj.ValorDepreciacion;
                            db.Depreciacions.InsertOnSubmit(D);

                            Entidad.Bien B = db.Bien.SingleOrDefault(s => s.ID.Equals(obj.BienID));

                            if (B != null)
                            {
                                B.ValorDepreActual = obj.ValorDepreActual + obj.ValorDepreciacion;
                                B.MesesDepreciados = obj.NumeroDepreciacion;//obj.MesesDepreciados + obj.NumeroDepreciacion;

                                if (B.ValorDepreActual >= B.ValorAdquisicion)
                                    B.EsDepreciado = false;

                            }
                            db.SubmitChanges();
                        }
                        else
                        {
                            Entidad.Bien B = db.Bien.SingleOrDefault(s => s.ID.Equals(obj.BienID));

                            if (B != null)
                            {
                                B.EsDepreciado = false;
                            }

                            db.SubmitChanges();
                        }
                    }

                    #endregion
                    
                    #region <<< REGISTRANDO COMPROBANTE >>>

                    List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                    var objCC = from cds in Compronbante
                                join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                                join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                                select new
                                {
                                    Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                    Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                };

                    if (!(Decimal.Round(objCC.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((objCC.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                        trans.Rollback();
                        return false;
                    }

                    M.ComprobanteContables.Clear();

                    Compronbante.ForEach(linea =>
                        {
                            M.ComprobanteContables.Add(linea);
                            db.SubmitChanges();
                        });

                    db.SubmitChanges();
                    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    return true;
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        private List<Entidad.ComprobanteContable> PartidasContable
        {
            get
            {
                try
                {
                   List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    
                    #region <<< DETALLE_COMPROBANTE >>>
                    int i = 1;

                    DetalleD.GroupBy(g => g.TipoActivoID).ToList().ForEach(det =>
                        {
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = det.First().CuentaGasto,
                                Monto = Math.Abs(det.Sum(s => s.ValorDepreciacion)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Sum(s => s.ValorDepreciacion)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = "Depreciación " +  det.First().TipoActivoNombre + " para el " + Fecha.ToShortDateString(),
                                Linea = i,
                                CentroCostoID = det.First().AreaID,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;

                            det.Where(o => !o.ValorDepreciacion.Equals(0)).ToList().ForEach(obj =>
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = obj.CuentaActivo,
                                        Monto = -Math.Abs(obj.ValorDepreciacion),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(obj.ValorDepreciacion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Depreciación " + obj.Nombre + " para el " + Fecha.ToShortDateString(),
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;
                                });
                        });
                    
                    #endregion
                    return CD;
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }
        }

        #endregion



        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, NextDepre);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !NextDepre)
            {
                if (DetalleD.Count > 0 || string.IsNullOrEmpty(mmoComentario.Text))
                {
                    if (Parametros.General.DialogMsg("El Documento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ValidarCampos(true))
                //{
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable.OrderBy(o => o.Linea).ToList();
                        nf.Text = "Comprobante Depreciación";
                        nf.ShowDialog();
                    }
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaVencimiento_EditValueChanged(object sender, EventArgs e)
        {
            txtNombre_Validated_1(sender, null);
        }         

        //Botones para accesos directos
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.btnOK.Visible.Equals(true))
            {
                if (keyData == (Keys.F7))
                {
                    btnOK_Click_1(null, null);
                    return true;
                }
            }

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
                return true;
            }

            if (this.bntNew.Enabled.Equals(true))
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    bntNew_Click(null, null);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        //EVENTO REGISTRAR BIENES
        private void bntNew_Click(object sender, EventArgs e)
        {
            NextDepre = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();
        }
          
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            try
            {                

            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaDepre.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
            
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void lkEs_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkEs.EditValue != null)
                {
                    if (Convert.ToInt32(lkEs.EditValue).Equals(0))
                    {
                        this.lkSus.EditValue = null;
                        this.lkSus.Properties.DataSource = null;
                        this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else if (!Convert.ToInt32(lkEs.EditValue).Equals(0))
                    {

                        var Sus = (EtSubEstacion.Where(sus => sus.Code.Equals(Convert.ToInt32(lkEs.EditValue))).Select(s => new { s.ID, s.Display})).ToList();

                        if (Sus.Count > 0)
                        {
                            this.lkSus.EditValue = null;
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            lkSus.Properties.DataSource = Sus;
                        }
                        else
                        {
                            this.lkSus.EditValue = null;
                            this.lkSus.Properties.DataSource = null;
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        }

                        int number = 1;
                        var Mov = db.Movimientos.Select(s => new { s.EstacionServicioID, s.SubEstacionID, s.MovimientoTipoID, s.Numero }).Where(m => m.EstacionServicioID.Equals(Convert.ToInt32(lkEs.EditValue)) && m.SubEstacionID.Equals(0) && m.MovimientoTipoID.Equals(75)).OrderByDescending(o => o.Numero).FirstOrDefault();
                        if (Mov != null)
                        {
                            number = Parametros.General.ValorConsecutivo(Mov.Numero.ToString());
                        }

                        Numero = number.ToString();
                    }
                }
                else
                {
                    this.lkSus.EditValue = null;
                    this.lkSus.Properties.DataSource = null;
                    this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCargaBien()) return;

                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                
                DateTime vFecha = new DateTime(dateFechaDepre.DateTime.Year, dateFechaDepre.DateTime.Month, 1).AddDays(-1);
                var lista = dv.VistaBienes.Where(o => o.EstacionID.Equals(IDEstacionServicio) && o.SubEstacionID.Equals(IDSubEstacion) && o.Activo && o.EsDepreciado && o.FechaAdquisicion.Date <= vFecha.Date).ToList();

                    lista.ForEach(det =>
                        {

                            Entidad.VistaDepreciacion VD = new Entidad.VistaDepreciacion();

                            VD.BienID = det.ID;
                            VD.TipoActivoID = det.TipoActivoID;
                            VD.TipoActivoNombre = det.TipoActivoNombre;
                            VD.ActivoNombre = det.ActivoNombre;
                            VD.AreaID = det.AreaID;
                            VD.Nombre = det.Nombre;
                            VD.Codigo = det.Codigo;
                            VD.NoSerie = det.NoSerie;
                            VD.Modelo = det.Modelo;
                            VD.FechaAdquisicion = det.FechaAdquisicion;
                            VD.EstacionID = IDEstacionServicio;
                            VD.EstacionNombre = (IDEstacionServicio > 0 ? EtEstacion.Single(s => s.ID.Equals(IDEstacionServicio)).Display : "");
                            VD.SubEstacionID = IDSubEstacion;
                            VD.SubEstacionNombre = (IDSubEstacion > 0 ? EtSubEstacion.Single(s => s.ID.Equals(IDSubEstacion)).Display : "");
                            VD.ValorAdquisicion = det.ValorAdquisicion;
                            VD.VidaUtilMeses = det.VidaUtilMeses;

                            //Valor Depreciacion                        
                            decimal Valor = Decimal.Round(det.ValorAdquisicion / det.VidaUtilMeses, 2, MidpointRounding.AwayFromZero);

                            decimal vDepreciacionActual = 0m;
                            if (det.ValorDepreActual.Equals(0))
                            {
                                //Valor de la Depreciacion Actual
                                vDepreciacionActual = Decimal.Round((((vFecha.Year - det.FechaAdquisicion.Year) * 12) + (vFecha.Month - det.FechaAdquisicion.Month)) * Valor, 2, MidpointRounding.AwayFromZero);

                                if (vDepreciacionActual > det.ValorAdquisicion)
                                    vDepreciacionActual = det.ValorAdquisicion;
                            }
                            else
                            {
                                vDepreciacionActual = det.ValorDepreActual;
                            }

                            VD.ValorDepreActual = vDepreciacionActual;

                            int vMeses = 0;
                            if (det.MesesDepreciados.Equals(0))
                            {
                                //Valor del mes de depreciación 
                                vMeses = (((vFecha.Year - det.FechaAdquisicion.Year) * 12) + (vFecha.Month - det.FechaAdquisicion.Month));

                                if (vMeses > det.VidaUtilMeses)
                                    vMeses = det.VidaUtilMeses;

                            }
                            else
                            {
                                vMeses = det.MesesDepreciados;
                            }

                            VD.MesesDepreciados = vMeses;

                            VD.NumeroDepreciacion = (VD.MesesDepreciados >= VD.VidaUtilMeses ? 0 : VD.MesesDepreciados + 1);

                            if (VD.ValorDepreActual >= VD.ValorAdquisicion)
                                VD.ValorDepreciacion = 0m;
                            else
                            {
                                //Si el mes de depreciacion es 0
                                if (VD.MesesDepreciados >= VD.VidaUtilMeses)
                                {
                                    VD.ValorDepreciacion = VD.ValorAdquisicion - VD.ValorDepreActual;
                                }
                                else if (VD.NumeroDepreciacion.Equals(VD.VidaUtilMeses))
                                {
                                    VD.ValorDepreciacion = VD.ValorAdquisicion - VD.ValorDepreActual;
                                }
                                else
                                    VD.ValorDepreciacion = Valor;
                            }

                            VD.CuentaActivo = det.CuentaActivo;
                            VD.CuentaGasto = det.CuentaGasto;

                            DetalleD.Add(VD);
                        }
                    );
                    
            
            this.bdsDetalle.DataSource = DetalleD;
            gvBien.RefreshData();

            if (lista.Count > 1)
            {

            this.dateFechaDepre.Properties.ReadOnly = true;
            this.lkEs.Properties.ReadOnly = true;
            this.lkSus.Properties.ReadOnly = true;

            btnLoad.Enabled = false;
            }
                
                Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private bool ValidarCargaBien()
        {try
            {  
            DateTime vFecha = new DateTime(dateFechaDepre.DateTime.Year, dateFechaDepre.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            DateTime FechaAnterior = vFecha.AddMonths(-1);

            var query = db.PeriodoContables.FirstOrDefault(p => p.EstacionID.Equals(IDEstacionServicio) && (p.FechaInicio.Date <= vFecha.Date && p.FechaFin.Date >= vFecha.Date));

            if (query == null)
            {
                Parametros.General.DialogMsg("El periodo para esta fecha no ha sido creado", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(vFecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(IDEstacionServicio)) > 0)
            {
                if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(IDEstacionServicio) && (m.FechaRegistro.Date > new DateTime(FechaAnterior.Year, FechaAnterior.Month, 1).AddDays(-1).Date && m.FechaRegistro.Date <= new DateTime(FechaAnterior.Year, FechaAnterior.Month, 1).AddMonths(1).AddDays(-1).Date)) <= 0)
                {
                    Parametros.General.DialogMsg("La depreciación del periodo pasado no ha sido aplicada", Parametros.MsgType.warning);
                    return false;
                }

                if (Parametros.General.ValidatePeriodoContable(FechaAnterior, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo contable para la fecha anterior no esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && (m.FechaRegistro.Date > new DateTime(vFecha.Year, vFecha.Month, 01).Date && m.FechaRegistro.Date <= vFecha.Date)) >= 1) { 
                Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo",Parametros.MsgType.warning);
                return false;
            }

           

            return true;                                
        }
            catch(Exception ex){
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

                return false;
            }
        }

        #endregion

        private void gvBien_GroupRowCollapsing(object sender, RowAllowEventArgs e)
        {
            e.Allow = false;
        }

        private void dateFechaDepre_EditValueChanged(object sender, EventArgs e)
        {
            try
            { 
            DateTime vFecha = new DateTime(dateFechaDepre.DateTime.Year, dateFechaDepre.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            if (!dateFechaDepre.DateTime.Date.Equals(vFecha.Date))
                Fecha = vFecha;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


    }

}