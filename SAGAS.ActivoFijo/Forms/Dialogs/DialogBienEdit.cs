using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogBienEdit : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormBien MDI;
        internal Entidad.VistaBienes EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        internal bool RefreshMDI = false;
        internal DateTime vFecha;
        internal int _ID = 0;
        internal decimal _TipoCambio = 0;
        DXErrorProvider errRequiredField = new DXErrorProvider();
        

        //public int Codigo
        //{
        //    get { return Convert.ToInt32(txtCodigo.Text); }
        //    set { txtCodigo.EditValue = value; }
        //}

        //public string Nombre
        //{
        //    get { return txtNombre.Text; }
        //    set { txtNombre.Text = value; }
        //}
        
        //public string Descripcion
        //{
        //    get { return mmoDescripcion.Text; }
        //    set { mmoDescripcion.Text = value; }
        //}

        //public bool EsTangible 
        //{
        //    get { return chkTangible.Checked; }
        //    set { chkTangible.Checked = value; }        
        //}

        //public bool EsDepreciable
        //{
        //    get { return chkDepreciable.Checked; }
        //    set { chkDepreciable.Checked = value; }
        //}

        //public int CuentaActivo 
        //{
        //    get { return Convert.ToInt32(this.glkCuentaActivo.EditValue); }
        //    set { this.glkCuentaActivo.EditValue = value; }        
        //}

        //public int CuentaGasto
        //{
        //    get { return Convert.ToInt32(this.glkCuentaGasto.EditValue); }
        //    set { this.glkCuentaGasto.EditValue = value; }
        //}
        
        //public int CuentaDepreciacionAcumulada
        //{
        //    get { return Convert.ToInt32(this.glkCuentaDepreciacionAcumulada.EditValue); }
        //    set { this.glkCuentaDepreciacionAcumulada.EditValue = value; }
        //}

        //public int VidaUtil
        //{
        //    get { return Convert.ToInt32(this.seVidaUtil.Value); }
        //    set { this.seVidaUtil.Value = value; }
        //}
        
        #endregion

        public DialogBienEdit(int id)
        {
            InitializeComponent();
            _ID = id;
        }
        
        private void DialogBienEdit_Shown(object sender, EventArgs e)
        {
            FillControl();
        }

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                vFecha = Convert.ToDateTime(db.GetDateServer());
                dateFecha.DateTime = vFecha.Date;
                dateFecha_Validated(dateFecha, null);
                lkArea.Properties.DataSource = db.CentroCostos.Where(c => c.Activo).Select(s => new { s.ID, s.Nombre });

                EntidadAnterior = dv.VistaBienes.Single(s => s.ID.Equals(_ID));

                txtComentario.Text = EntidadAnterior.Descripcion;
                txtCodigo.Text = EntidadAnterior.Codigo.ToString();
                txtNombre.Text = EntidadAnterior.Nombre;
                txtActivo.Text = EntidadAnterior.ActivoNombre;
                txtTipoActivo.Text = EntidadAnterior.TipoActivoNombre;
                txtNroSerie.Text = EntidadAnterior.NoSerie;
                txtMarca.Text = EntidadAnterior.Marca;
                txtModelo.Text = EntidadAnterior.Modelo;
                txtMatricula.Text = EntidadAnterior.Matricula;
                txtChasis.Text = EntidadAnterior.Chasis;
                txtMotor.Text = EntidadAnterior.Motor;
                txtFecha.Text = EntidadAnterior.FechaAdquisicion.ToShortDateString();
                txtFactura.Text = EntidadAnterior.NoFactura;
                txtValorAdquisicion.Text = EntidadAnterior.ValorAdquisicion.ToString("#,#.00");
                txtDepreciacionAcumulada.Text = EntidadAnterior.ValorDepreActual.ToString("#,#.00");

                txtAsignado.Text = EntidadAnterior.UsuarioAsignado;
                lkArea.EditValue = EntidadAnterior.AreaID;
                txtEstacionAnterior.Text = (EntidadAnterior.SubEstacionID > 0 ? db.SubEstacions.Single(s => s.ID.Equals(EntidadAnterior.SubEstacionID)).Nombre : db.EstacionServicios.Single(s => s.ID.Equals(EntidadAnterior.EstacionID)).Nombre);

                lkEs.Properties.DataSource = db.EstacionServicios.Where(o => !o.ID.Equals(EntidadAnterior.EstacionID)).Select(s => new { s.ID, s.Nombre }).ToList();
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        public bool ValidarCampos()
        {
            if (chkCambiar.Checked)
            {
                DateTime vFechaV = new DateTime(dateFecha.DateTime.Year, dateFecha.DateTime.Month, 1).AddMonths(1).AddDays(-1);

                DateTime FechaAnterior = new DateTime(vFechaV.Year, vFechaV.Month, 01);


                if (_TipoCambio <= 0)
                {
                    Parametros.General.DialogMsg("No existe tipo de cambio para la fecha:  " + dateFecha.DateTime.ToShortDateString() + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (lkEs.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Estacíon de Traslado" + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
                else
                {
                    if (Convert.ToInt32(lkEs.EditValue) <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar la Estacíon de Traslado" + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }


                if (!Parametros.General.ValidatePeriodoContable(dateFecha.DateTime, db, EntidadAnterior.EstacionID))
                {
                    Parametros.General.DialogMsg("El Periodo contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }

                if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && (m.FechaRegistro.Date > FechaAnterior.Date && m.FechaRegistro.Date <= vFechaV.Date)) >= 1)
                {
                    Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo", Parametros.MsgType.warning);
                    return false;
                }

                if (layoutControlItemSubEstacion.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    if (lkSus.EditValue == null)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación" + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    else
                    {
                        if (Convert.ToInt32(lkSus.EditValue) <= 0)
                        {
                            Parametros.General.DialogMsg("Debe seleccionar una Sub Estación" + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }

            }

            return true;
        }
        
        private bool ValidarCodigo(int code, int? ID)
        {
            var result = (from i in db.TipoActivo
                          where (ID.HasValue ? i.Codigo == code && i.ID != Convert.ToInt32(ID) : i.Codigo == code)                          
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Entidad.Bien EB = db.Bien.Single(s => s.ID.Equals(EntidadAnterior.ID));
                    Entidad.TrasladoBien TB = new Entidad.TrasladoBien();

                    EB.Codigo = Convert.ToInt32(txtCodigo.Text);
                    EB.Nombre = txtNombre.Text;
                    EB.NoSerie = txtNroSerie.Text;
                    EB.Marca = txtMarca.Text;
                    EB.Modelo = txtModelo.Text;
                    EB.Matricula = txtMatricula.Text;
                    EB.Chasis = txtChasis.Text;
                    EB.Motor = txtMotor.Text;
                    EB.Descripcion = txtComentario.Text;
                    EB.NoFactura = txtFactura.Text;
                    EB.UsuarioAsignado = txtAsignado.Text;
                    EB.AreaID = Convert.ToInt32(lkArea.EditValue);

                    //Datos Anteriores
                    TB.BienID = EB.ID;
                    TB.UsuarioAsignadoAnterior = EntidadAnterior.UsuarioAsignado;
                    TB.AreaAnteriorID = EntidadAnterior.AreaID;
                    TB.EstacionAnteriorID = EntidadAnterior.EstacionID;
                    TB.SubEstacionAnteriorID = EntidadAnterior.SubEstacionID;

                    //Datos Nuevos
                    TB.UsuarioAsignadoNuevo = EB.UsuarioAsignado;
                    TB.AreaNuevaID = EB.AreaID;
                    TB.EstacionNuevaID = EntidadAnterior.EstacionID;
                    TB.SubEstacionNuevaID = EntidadAnterior.SubEstacionID;

                    db.SubmitChanges();

                    if (chkCambiar.Checked)
                    {
                        EB.EstacionID = Convert.ToInt32(lkEs.EditValue);
                        EB.SubEstacionID = (lkSus.EditValue == null ? 0 : Convert.ToInt32(lkSus.EditValue));

                        //Datos Nuevos
                        TB.EstacionNuevaID = EB.EstacionID;
                        TB.SubEstacionNuevaID = EB.SubEstacionID;

                        Entidad.Movimiento M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 77;

                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.UsuarioID = Parametros.General.UserID;

                        M.FechaRegistro = dateFecha.DateTime.Date;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && m.SubEstacionID.Equals(EntidadAnterior.SubEstacionID) && m.MovimientoTipoID.Equals(77)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && m.SubEstacionID.Equals(EntidadAnterior.SubEstacionID) && m.MovimientoTipoID.Equals(77)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                        M.Numero = number;

                        M.EstacionServicioID = EntidadAnterior.EstacionID;
                        M.SubEstacionID = EntidadAnterior.SubEstacionID;
                        M.Monto = Decimal.Round(EntidadAnterior.ValorDepreActual, 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(EntidadAnterior.ValorDepreActual) / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                        db.Movimientos.InsertOnSubmit(M);
                        db.SubmitChanges();

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
                        TB.MovimientoID = M.ID;
                    }

                    if (TB.UsuarioAsignadoAnterior != TB.UsuarioAsignadoNuevo || !TB.AreaNuevaID.Equals(TB.AreaAnteriorID) || !TB.EstacionNuevaID.Equals(TB.EstacionAnteriorID) || !TB.SubEstacionNuevaID.Equals(TB.SubEstacionAnteriorID))
                        db.TrasladoBiens.InsertOnSubmit(TB);

                    //DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EB, 1));
                    //DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se modificó el Tipo de Activo: " + EntidadAnterior.Nombre, this.Name);//, dtPosterior, dtAnterior);

                    db.SubmitChanges();
                    trans.Commit();

                    RefreshMDI = true;
                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
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

                    int i = 1;

                    var EsOrigen = db.EstacionServicios.Select(o => new { o.ID, o.CuentaInternaActivo, o.CuentaInternaPasivo }).Single(s => s.ID.Equals(EntidadAnterior.EstacionID));
                    var EsDestino = db.EstacionServicios.Select(o => new { o.ID, o.CuentaInternaActivo, o.CuentaInternaPasivo }).Single(s => s.ID.Equals(Convert.ToInt32(lkEs.EditValue)));
                    int cuenta = db.TipoActivo.Single(s => s.ID.Equals(EntidadAnterior.TipoActivoID)).CuentaActivo;
                   
                    #region <<< ORIGEN_BIEN >>>                   

                    //Depreciación
                    
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = EntidadAnterior.CuentaActivo,
                            Monto = Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                            Linea = i,
                            EstacionServicioID = EntidadAnterior.EstacionID,
                            SubEstacionID = EntidadAnterior.SubEstacionID
                        });
                        i++;

                    //Adquisición
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = cuenta,
                            Monto = -Math.Abs(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                            Linea = i,
                            EstacionServicioID = EntidadAnterior.EstacionID,
                            SubEstacionID = EntidadAnterior.SubEstacionID
                        });
                        i++;

                    //Adquisición Cuenta por Cobrar
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = EsDestino.CuentaInternaActivo,
                            Monto = Convert.ToDecimal(EntidadAnterior.ValorAdquisicion),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                            Linea = i,
                            EstacionServicioID = EntidadAnterior.EstacionID,
                            SubEstacionID = EntidadAnterior.SubEstacionID
                        });
                        i++;

                            //Depresiación Cuenta por Cobrar
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = EsDestino.CuentaInternaPasivo,
                                Monto = -Math.Abs(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = dateFecha.DateTime.Date,
                                Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                                Linea = i,
                                EstacionServicioID = EntidadAnterior.EstacionID,
                                SubEstacionID = EntidadAnterior.SubEstacionID
                            });
                            i++;
                    #endregion

                        #region <<< DESTINO_BIEN >>>

                        //Adquisición
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = cuenta,
                            Monto = Convert.ToDecimal(EntidadAnterior.ValorAdquisicion),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                            Linea = i,
                            EstacionServicioID = Convert.ToInt32(lkEs.EditValue),
                            SubEstacionID = (lkSus.EditValue == null ? 0 : Convert.ToInt32(lkSus.EditValue))
                        });
                        i++;

                            //Depreciación
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = EntidadAnterior.CuentaActivo,
                                Monto = -Math.Abs(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = dateFecha.DateTime.Date,
                                Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                                Linea = i,
                                EstacionServicioID = Convert.ToInt32(lkEs.EditValue),
                                SubEstacionID = (lkSus.EditValue == null ? 0 : Convert.ToInt32(lkSus.EditValue))
                            });
                            i++;

                        //Adquisición Cuenta por Cobrar
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = EsOrigen.CuentaInternaPasivo,
                            Monto = -Math.Abs(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                            Linea = i,
                            EstacionServicioID = Convert.ToInt32(lkEs.EditValue),
                            SubEstacionID = (lkSus.EditValue == null ? 0 : Convert.ToInt32(lkSus.EditValue))
                        });
                        i++;

                            //Depreciacioón Cuenta por Cobrar
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = EsOrigen.CuentaInternaActivo,
                                Monto = Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = dateFecha.DateTime.Date,
                                Descripcion = "Traslado del Bien " + EntidadAnterior.Codigo + " | " + EntidadAnterior.Nombre,
                                Linea = i,
                                EstacionServicioID = Convert.ToInt32(lkEs.EditValue),
                                SubEstacionID = (lkSus.EditValue == null ? 0 : Convert.ToInt32(lkSus.EditValue))
                            });
                            i++;
                        #endregion



                    return CD;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, false, RefreshMDI, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.TextEdit)sender, errRequiredField);
        }

        private void chkCambiar_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlGroupEstaciones.Visibility = (chkCambiar.Checked ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never);
            btnShowComprobante.Visible = chkCambiar.Checked;
            lkEs.EditValue = null;
            lkSus.EditValue = null;
        }

        private void lkEstacion_EditValueChanged(object sender, EventArgs e)
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

                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkEs.EditValue))).Select(s => new { s.ID, s.Nombre })).ToList();

                        if (Sus.Count > 0)
                        {
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            lkSus.Properties.DataSource = Sus;
                        }
                        else
                        {
                            this.lkSus.EditValue = null;
                            this.lkSus.Properties.DataSource = null;
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        }
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


        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                {
                    nf.DetalleCD = PartidasContable;
                    nf.Text = "Comprobante Contable Traslado de Activo";
                    nf.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFecha_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(dateFecha.DateTime.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(dateFecha.DateTime.Date)).First().Valor : 0m);

        }


        #endregion

    }
}
