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
using SAGAS.Ventas.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogNotasDC : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormNotasDC MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool NextProvision = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID; 
        private bool _Guardado = false;
        private int IDPrint = 0;
        private bool EsEmpleado = false;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        internal bool _Cupones = false;
        internal bool _HasDiferenciado = false;
        internal int _MonedaPrimaria;
        internal int _ProductoClaseCombustible;
        internal int _ClienteAnticipo;
        internal int _CuponesNotaID;
        internal DateTime _FechaActual;
        private decimal _Disponible = 0;
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFechaNota.EditValue); }
            set { dateFechaNota.EditValue = value; }
        }

        private int IDProveedor
        {
            get { return Convert.ToInt32(glkDeudor.EditValue); }
            set { glkDeudor.EditValue = value; }
        }

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        struct ListCupones
        {
            public int ID;
            public string Display;
            public bool EsCuponCS;
            public decimal Equivalente;
            public int Orden;
        };


        List<ListCupones> _ListCupones = new List<ListCupones>();
        private static Entidad.Cliente client;
        private static Entidad.Empleado empl;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private List<Parametros.ListIdDisplay> listaTipo = new List<Parametros.ListIdDisplay>();
        private DataTable DetalleComprobante;
        private List<Entidad.Movimiento> DetalleM = new List<Entidad.Movimiento>();

        private List<Entidad.NotaCreditoCupones> NCC = new List<Entidad.NotaCreditoCupones>();
        public List<Entidad.NotaCreditoCupones> DetalleNCC
        {
            get { return NCC; }
            set
            {
                NCC = value;
                this.bdsCupones.DataSource = this.NCC;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogNotasDC(int UserID)
        {            
            InitializeComponent();
            UsuarioID = UserID;            
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //rgCreditoContado.SelectedIndex = 0;
            //IDProveedor = 186;
            //Referencia = "123";
            //IDAlmacen = 1;
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
            
        }

        #endregion

        #region *** METODOS ***

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Panel1;
                listaTipo.Add(new Parametros.ListIdDisplay(1, "Cliente")); 
                listaTipo.Add(new Parametros.ListIdDisplay(2, "Empleado"));
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Nombre, s.Abreviatura });
                lkTipoDeudor.Properties.DataSource = listaTipo;
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _ProductoClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
                _CuponesNotaID = Parametros.Config.CuponesNotaID();
                _FechaActual = Convert.ToDateTime(db.GetDateServer());

                this.CentrosCostos = from cto in db.CentroCostos
                                      join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                      where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                      select new Parametros.ListIdDisplay {ID = cto.ID, Display = cto.Nombre};

                Cuentas = (from cc in db.CuentaContables
                                         join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                         join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                                         where cc.Detalle && !cc.Modular && cc.Activo && cces.EstacionID.Equals(IDEstacionServicio)
                                         select new Parametros.ListIdDisplayCodeBool
                                         {
                                             ID = cc.ID,
                                             Codigo = cc.Codigo,
                                             Display = cc.Nombre,
                                             valor = tc.UsaCentroCosto
                                         }).OrderBy(o => o.Codigo);

                ////Proveedor
                //glkDeudor.Properties.DataSource = (from p in db.Proveedors where (p.Activo || OnlyView) select new { p.ID, p.Codigo, p.Nombre, p.NombreComercial, Display = p.Codigo + " | " + p.Nombre }).OrderBy(o => o.Codigo);
                //glkDeudor.Properties.DisplayMember = "Display";
                //glkDeudor.Properties.ValueMember = "ID";

                //--- Fill Combos Detalles --//
                gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                gridCuenta.DataSource = null;
                gridCuenta.DataSource = Cuentas;

                //Centro Costo GRID
                lkCentroCosto.DataSource = CentrosCostos;
                lkCentroCosto.DisplayMember = "Display";
                lkCentroCosto.ValueMember = "ID";

                dateFechaNota.EditValue = Convert.ToDateTime(db.GetDateServer());

                dateDesde.EditValue = dateFechaNota.EditValue;
                dateHasta.EditValue = dateFechaNota.EditValue;

                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);

                rgOption.SelectedIndex = -1;
                rgOption.SelectedIndex = 0;

                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

                var listComprobante = (from cd in db.ComprobanteContables
                                       join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                       join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                       where cd.MovimientoID.Equals(0)
                                       select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                this.DetalleComprobante = ToDataTable(listComprobante);

                this.bdsDetalle.DataSource = this.DetalleComprobante;

                var HasDiferenciado = from m in db.Mangueras
                                      join t in db.Tanques on m.TanqueID equals t.ID
                                      where t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion) && m.PrecioDiferenciado
                                      select new { m.ID };

                if (HasDiferenciado.Count() > 0)
                {
                    _HasDiferenciado = true;
                }

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
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkDeudor, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && i.ProveedorID.Equals(IDProveedor) && !i.Anulado
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (chkAnticipo.Checked)
            {
                if (client != null)
                {
                    if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                    {
                        Parametros.General.DialogMsg("El cliente " + glkDeudor.Text + " no aplica para saldo anticipo.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                else
                {
                    Parametros.General.DialogMsg("El cliente " + glkDeudor.Text + " no aplica para saldo anticipo.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (dateFechaNota.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtNumero.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la provisión.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkDeudor.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Deudor.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaNota, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (Fecha.Date > Convert.ToDateTime(db.GetDateServer()).Date)
            {
                Parametros.General.DialogMsg("La fecha de recibido o de factura no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
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

            if (!detalle)
            {
                if (rgOption.SelectedIndex.Equals(0))
                {
                    if (DetalleComprobante.Rows.Count <= 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle a la Nota." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText) < 0)
                        {
                            Parametros.General.DialogMsg("La nota de débito no puede ser menor a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }
                else
                {
                    if (!_Cupones)
                    {
                        if (!chkAnticipo.Checked)
                        {
                            if (Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) <= 0)
                            {
                                Parametros.General.DialogMsg("Debe de ingresar detalle de los Documentos." + Environment.NewLine, Parametros.MsgType.warning);
                                return false;
                            }

                            if (DetalleComprobante.Rows.Count <= 0)
                            {
                                Parametros.General.DialogMsg("Debe de ingresar detalle a la Nota." + Environment.NewLine, Parametros.MsgType.warning);
                                return false;
                            }

                            if (!Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).Equals(Convert.ToDecimal(DetalleM.Sum(s => s.Litros))))
                            {
                                Parametros.General.DialogMsg("el total del MONTO PAGADO de los DOCUMENTOS es diferente al VALOR TOTAL de la NOTA" + Environment.NewLine, Parametros.MsgType.warning);
                                return false;
                            }
                        }
                    }
                    else if (_Cupones)
                    {
                        if (Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)) <= 0)
                        {
                            Parametros.General.DialogMsg("Debe de ingresar detalle de los Cupones." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }

                if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                {
                    Parametros.General.DialogMsg("La referencia para esta Nota ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                    return false;
                }

            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (DetalleM.Count > 0)
            {
                if (DetalleM.Count(d => d.Litros > 0 && d.FechaRegistro.Date > Fecha) > 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
            }

            if (rgOption.SelectedIndex.Equals(0) && !EsEmpleado)
            {
                if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                {
                    if (_Disponible <= 0)
                    {
                        Parametros.General.DialogMsg("El cliente no tiene saldo disponible.", Parametros.MsgType.warning);
                        return false;
                    }

                    if (!String.IsNullOrEmpty(gvDetalle.Columns["Monto"].SummaryText))
                    {
                        if (Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero) > _Disponible)
                        {
                            Parametros.General.DialogMsg("El monto total es mayor al saldo disponible por el cliente.", Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }
                /*else
                {
                    Parametros.General.DialogMsg("Revisar la suma del detalle antes de guardar.", Parametros.MsgType.warning);
                    return false;
                }*/
            }

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
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
                    M.MovimientoTipoID = Convert.ToInt32(rgOption.Properties.Items[rgOption.SelectedIndex].Value);
                    if (EsEmpleado)
                        M.EmpleadoID = empl.ID;
                    else
                        M.ClienteID = client.ID;

                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Fecha;
                    M.MonedaID = _MonedaPrimaria;
                    M.TipoCambio = _TipoCambio;
                    if (rgOption.SelectedIndex.Equals(0))
                    {
                        M.Monto = Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        if (!_Cupones)
                        {
                            if (chkAnticipo.Checked)
                            {
                                M.Monto = Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero);
                                M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                M.Monto = Decimal.Round(Convert.ToDecimal(DetalleM.Sum(s => s.Litros)), 2, MidpointRounding.AwayFromZero);
                                M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            }
                        }
                        else if (_Cupones)
                        {
                            M.Monto = Decimal.Round(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)), 2, MidpointRounding.AwayFromZero);
                            M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                        }
                   }

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(Convert.ToInt32(rgOption.Properties.Items[rgOption.SelectedIndex].Value))) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(Convert.ToInt32(rgOption.Properties.Items[rgOption.SelectedIndex].Value))).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    M.Numero = number;
                    
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;

                    db.Movimientos.InsertOnSubmit(M);                    
                    db.SubmitChanges();

                    #region <<< REGISTRANDO COMPROBANTE >>>
                    List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                    var obj = from cds in Compronbante
                              join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                              join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                              select new
                              {
                                  Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                  Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                              };

                    if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                        trans.Rollback();
                        return false;
                    }
                    Compronbante.ForEach(linea =>
                        {
                            M.ComprobanteContables.Add(linea);
                            db.SubmitChanges();
                        });

                    db.SubmitChanges();

                    #endregion

                    if (!_Cupones)
                    {
                        
                            #region ::: REGISTRANDO DEUDOR :::
                            Entidad.Deudor D = new Entidad.Deudor();
                            if (EsEmpleado)
                                D.EmpleadoID = M.EmpleadoID;
                            else
                                D.ClienteID = M.ClienteID;

                            decimal Saldo = 0;

                            if (rgOption.SelectedIndex.Equals(0))
                                Saldo = M.Monto;
                            else
                                Saldo = -Math.Abs(M.Monto);

                            D.Valor = Saldo;
                            D.MovimientoID = M.ID;
                            db.Deudors.InsertOnSubmit(D);
                            db.SubmitChanges();

                            if (client != null)
                            {
                                if (rgOption.SelectedIndex.Equals(0))
                                {
                                    if (client.CuentaContableID.Equals(_ClienteAnticipo))
                                    {
                                        decimal vSaldo = 0m;

                                        var qSaldo = from dr in db.Deudors
                                                     join mv in db.Movimientos on dr.MovimientoID equals mv.ID
                                                     where !dr.ID.Equals(D.ID) && dr.ClienteID.Equals(client.ID) && !mv.Anulado
                                                     select dr.Valor;
                                        //).Sum(s => s.Valor) : 0m);

                                        if (qSaldo.Count() > 0)
                                            vSaldo = Decimal.Round(qSaldo.Sum(), 2, MidpointRounding.AwayFromZero);

                                        if (vSaldo < 0)
                                        {
                                            if (Math.Abs(vSaldo) > M.Monto)
                                            {
                                                M.Abono = M.Monto;
                                                M.Pagado = true;
                                                D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = M.ID, Monto = M.Abono });
                                            }
                                            else
                                            {
                                                M.Abono = Math.Abs(vSaldo);
                                                D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = M.ID, Monto = M.Abono });
                                            }

                                            db.SubmitChanges();
                                        }
                                    }
                                }
                            }

                            #endregion
                            if (!chkAnticipo.Checked)
                            {
                                #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>
                                string texto = "";

                                if (DetalleM.Count > 0)
                                {
                                    if (DetalleM.Sum(s => s.Litros) > 0)
                                    {
                                        foreach (Entidad.Movimiento linea in DetalleM.Where(d => d.Litros > 0))
                                        {
                                            Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(linea.ID));

                                            if (MLine.Anulado)
                                            {
                                                texto = (MLine.Numero.Equals(0) ? MLine.Referencia : MLine.Numero.ToString());
                                                break;
                                            }
                                            
                                            MLine.Abono += linea.Litros;
                                            MLine.Pagado = linea.Pagado;
                                            D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = linea.ID, Monto = linea.Litros });
                                            linea.Litros = 0;
                                            MLine.Litros = 0;
                                            db.SubmitChanges();
                                        }

                                        if (!String.IsNullOrEmpty(texto))
                                        {
                                            trans.Rollback();
                                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                            Parametros.General.DialogMsg("El Documento: " + texto + ", esta Anulado.", Parametros.MsgType.warning);
                                            return false;
                                        }
                                    }
                                }
                                #endregion
                            }
                    }
                    else if (_Cupones)
                    {
                        #region ::: REGISTRANDO DEUDOR :::
                        Entidad.Deudor D = new Entidad.Deudor();
                        D.ClienteID = M.ClienteID;
                        D.Valor = -Math.Abs(Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero));
                        D.MovimientoID = M.ID;
                        db.Deudors.InsertOnSubmit(D);
                        db.SubmitChanges();
                        #endregion

                        DetalleNCC.ForEach(det => det.MovimientoID = 0);
                        M.NotaCreditoCupones.AddRange(DetalleNCC);
                        db.SubmitChanges();

                        Entidad.Movimiento MDebito = new Entidad.Movimiento();

                        MDebito.MovimientoTipoID = 27;
                        MDebito.ClienteID = _CuponesNotaID;
                        MDebito.UsuarioID = M.UsuarioID;
                        MDebito.FechaCreado = M.FechaCreado;
                        MDebito.FechaRegistro = M.FechaRegistro;
                        MDebito.MonedaID = M.MonedaID;
                        MDebito.TipoCambio = M.TipoCambio;
                        MDebito.Monto = M.Monto;
                        MDebito.MontoMonedaSecundaria = M.MontoMonedaSecundaria;
                        MDebito.Referencia = "ND Cupones Anticipo " + M.Numero.ToString("000000000");
                        MDebito.EstacionServicioID = M.EstacionServicioID;
                        MDebito.SubEstacionID = M.SubEstacionID;
                        MDebito.Comentario = M.Comentario;
                        MDebito.MovimientoReferenciaID = M.ID;

                        db.Movimientos.InsertOnSubmit(MDebito);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se registró la Nota de Débito : " + MDebito.Referencia, this.Name);
                        db.SubmitChanges();

                        //DEUDOR DNP
                        Entidad.Deudor DNP = new Entidad.Deudor();
                        DNP.ClienteID = MDebito.ClienteID;
                        DNP.Valor = Decimal.Round(MDebito.Monto, 2, MidpointRounding.AwayFromZero);
                        DNP.MovimientoID = MDebito.ID;
                        db.Deudors.InsertOnSubmit(DNP);
                        db.SubmitChanges();

                    }

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                    "Se registró la Nota de " + rgOption.Properties.Items[rgOption.SelectedIndex].Description + " : " + M.Numero.ToString("000000000"), this.Name);
                    db.SubmitChanges();
                    trans.Commit();

                    NextProvision = false;
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    IDPrint = M.ID;
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
                    int i = 1;

                    if (rgOption.SelectedIndex.Equals(0))
                    {                        
                        if (client != null || empl != null)
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = (EsEmpleado ? 100 : client.CuentaContableID),
                                Monto = Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – Nota de Débito " + Numero,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        //DetalleComprobante.Rows. .ToList().ForEach(K =>
                        foreach (DataRow linea in DetalleComprobante.Rows)
                        {
                            //select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])) * -1,
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio) * -1, 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = Convert.ToString(linea["Descripcion"]),
                                Linea = i,
                                CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;
                        }
                    }
                    else if (rgOption.SelectedIndex.Equals(1))
                    {

                        if (!_Cupones)
                        {
                            decimal vMonto = 0;

                            foreach (DataRow linea in DetalleComprobante.Rows)
                            {
                                //select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);
                                vMonto += Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]));
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                    Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                    Fecha = Fecha,
                                    Descripcion = Convert.ToString(linea["Descripcion"]),
                                    Linea = i,
                                    CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                i++;
                            }

                            if (client != null || empl != null)
                            {
                                decimal vSaldo = 0;

                                if (chkAnticipo.Checked)
                                    vSaldo = vMonto;
                                else 
                                    vSaldo = Convert.ToDecimal(DetalleM.Sum(s => s.Litros));

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = (EsEmpleado ? 100 : client.CuentaContableID),
                                    Monto = -Math.Abs(Decimal.Round(vSaldo, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vSaldo / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                    Fecha = Fecha,
                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – Nota de Crédito " + Numero,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }
                        }
                        else if (_Cupones)
                        {
                            var CL = db.Clientes.Single(s => s.ID.Equals(_CuponesNotaID));
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = CL.CuentaContableID,
                                Monto = Decimal.Round(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = "Anticipo " + CL.Nombre,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = client.CuentaContableID,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleNCC.Sum(s => s.PrecioTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion =  client.Nombre + " – Nota de Crédito " + Numero,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }
                    }

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

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, NextProvision, RefreshMDI, IDPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {   
            if (!_Guardado && !NextProvision)
            {
                if (DetalleComprobante.Rows.Count > 0)
                {
                    if (Parametros.General.DialogMsg("La Nota actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            empl = null;
            client = null;
            EntidadAnterior = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Selecciona el Deudor
        private void glkProvee_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (glkDeudor.EditValue != null)
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);

                    if (Convert.ToInt32(glkDeudor.EditValue) > 0)
                    {
                        if (EsEmpleado)
                        {
                            empl = db.Empleados.Single(em => em.ID.Equals(Convert.ToInt32(glkDeudor.EditValue)));
                            client = null;

                            txtRuc.Text = empl.Codigo;
                            txtTelefonos.Text = "";
                            memoDir.Text = "";

                            spLimite.Value = 0;

                            decimal Saldo = (db.Deudors.Count(d => d.EmpleadoID.Equals(Convert.ToInt32(glkDeudor.EditValue))) > 0 ? db.Deudors.Where(d => d.EmpleadoID.Equals(Convert.ToInt32(glkDeudor.EditValue))).Sum(s => s.Valor) : 0m);
                            spSaldo.Value = Saldo;

                            decimal _Disponible = 0 - Saldo;
                            spDisponible.Value = _Disponible;

                        }
                        else
                        {
                            client = db.Clientes.Single(em => em.ID.Equals(Convert.ToInt32(glkDeudor.EditValue)));
                            empl = null;

                            txtRuc.Text = client.RUC;
                            txtTelefonos.Text = client.Telefono1 + " | " + client.Telefono2 + " | " + client.Telefono3;
                            memoDir.Text = client.Direccion;

                            if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                            {
                                spLimite.Value = client.LimiteCredito;

                                decimal Saldo = Convert.ToDecimal(db.GetSaldoCliente(client.ID, _FechaActual));
                                spSaldo.Value = Saldo;

                                _Disponible = client.LimiteCredito - Saldo;
                                spDisponible.Value = _Disponible;
;

                            }
                            else
                            {
                                //var obj = from d in db.Deudors
                                //          join m in db.Movimientos on d.MovimientoID equals m.ID
                                //          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                //          where !m.Anulado && !mt.EsAnulado && (m.ClienteID.Equals(client.ID) || d.ClienteID.Equals(client.ID)) 
                                //          select d.Valor;

                                //if (obj != null)
                                //{
                                //    if (obj.Count() > 0)
                                //        _Disponible = Convert.ToDecimal(obj.Sum(s => s) * -1);
                                //}

                                //spDisponible.Value = _Disponible;

                                
                                var obj = from d in db.Deudors
                                          join m in db.Movimientos on d.MovimientoID equals m.ID
                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                          where !m.Anulado && !mt.EsAnulado && (m.ClienteID.Equals(client.ID) || d.ClienteID.Equals(client.ID))
                                          select d.Valor;

                                spLimite.Value = client.LimiteCredito;

                                //decimal Saldo = (db.Deudors.Count(d => d.ClienteID.Equals(IDDeudor)) > 0 ? db.Deudors.Where(d => d.ClienteID.Equals(IDDeudor)).Sum(s => s.Valor) : 0m);
                                decimal Saldo = (obj.Count() > 0 ? Convert.ToDecimal(obj.Sum(s => s)) : 0m);
                                spSaldo.Value = Saldo;

                                decimal _Disponible = client.LimiteCredito - Saldo;
                                spDisponible.Value = _Disponible;
                            }
                        }

                        rgOption.Enabled = false;
                    }

                    if (rgOption.SelectedIndex.Equals(0))
                        this.gvDetalle.OptionsBehavior.Editable = true;

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());          
            }
        }

        #region <<< GRID DOCUMENTOS >>>

        //Validar las Filas del detalle
        private void gvCuentas_ValidateRow(object sender, ValidateRowEventArgs e)
        {

        }

        //Mensaje Validación del detalle
        private void gvCuentas_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvCuentas_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMonto)
                {
                    if (gvCuentas.GetRowCellValue(e.RowHandle, "Litros") != null)
                    {
                        if (!Convert.ToDecimal(gvCuentas.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                        {
                            decimal vSaldo = 0, vAbono = 0;

                            vAbono = Convert.ToDecimal(gvCuentas.GetRowCellValue(e.RowHandle, "Litros"));

                            if (gvCuentas.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvCuentas.GetRowCellValue(e.RowHandle, "Saldo"));

                            if (vAbono > vSaldo)
                            {
                                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                gvCuentas.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                            }
                            else
                            {
                                if (vAbono.Equals(vSaldo))
                                    gvCuentas.SetRowCellValue(e.RowHandle, "Pagado", true);
                                else
                                    gvCuentas.SetRowCellValue(e.RowHandle, "Pagado", false);
                            }
                        }
                        else
                        {
                            gvCuentas.SetRowCellValue(e.RowHandle, "Pagado", false);
                        }
                    }
                    else
                    {
                        gvCuentas.SetRowCellValue(e.RowHandle, "Litros", 0);
                    }

                    gvCuentas.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< GRID_DETALLES >>>

        //Cambio de Fila
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //gvDetalle.RefreshData();
        }

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            
            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "CuentaContableID") != DBNull.Value)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "CuentaContableID")) == 0)
                {
                    view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                    e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Descripcion
            //--
            if (view.GetRowCellValue(RowHandle, "Descripcion") == DBNull.Value)
            {
                view.SetColumnError(view.Columns["Descripcion"], "Debe de escribir una descripción.");
                e.ErrorText = "Debe de escribir una descripción.";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Valor             
            if (view.GetRowCellValue(RowHandle, "Monto") != DBNull.Value)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Monto")).Equals(0))
                {
                    view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                    e.ErrorText = "Debe Ingresar un valor en la linea.";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                e.ErrorText = "Debe Ingresar un valor en la linea.";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Centro de Costo             
            if (view.GetRowCellValue(RowHandle, "UsaCentroCosto") != DBNull.Value)
            {
                if (Convert.ToBoolean(view.GetRowCellValue(RowHandle, "UsaCentroCosto")).Equals(true))
                {
                    if (view.GetRowCellValue(RowHandle, "CentroCostoID") != DBNull.Value)
                    {
                        if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CentroCostoID")).Equals(0))
                        {

                            view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo.");
                            e.ErrorText = "Debe seleccionar un centro de costo.";
                            e.Valid = false;
                            Validate = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo");
                        e.ErrorText = "Debe seleccionar un centro de costo";
                        e.Valid = false;
                        Validate = false;
                    }
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                e.Valid = false;
                Validate = false;
            }
        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    gvDetalle.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    gvDetalle.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Linea", 0);

                    
                    if (gvDetalle.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(gvDetalle.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(gvDetalle.GetRowCellValue(gvDetalle.RowCount - 2, "Descripcion")));
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            

            #endregion
        }
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {            
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }

                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                            + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);

                            gvDetalle.RefreshData();
                            
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    view.AddNewRow();
                    gvDetalle.RefreshData();
                }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        gvDetalle.RefreshData();
                                               
                    }
                }
            }
        }

        //Validando la asignacion de Centro de Costo
        private void gvDetalle_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvDetalle.FocusedColumn == colCentroCosto)
            {
                if (gvDetalle.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(gvDetalle.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }

            }
        }
        
        #endregion

        #region <<<  GRID_CUPONES >>>


        #endregion

        private void gvCupon_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column == colQuantity)
            {
                try
                {
                    decimal vCantidad = 0, vEquivalencia = 0, vCantidadLitros = 0, vPrecio = 0, vTotal = 0;

                    if (gvCupon.GetRowCellValue(e.RowHandle, "CantidadCupon") != null)
                        vCantidad = Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "CantidadCupon"));

                    if (gvCupon.GetRowCellValue(e.RowHandle, "Equivalencia") != null)
                        vEquivalencia = Convert.ToDecimal(gvCupon.GetRowCellValue(e.RowHandle, "Equivalencia"));

                    //Verificar si es cupon Litros
                    if (!Convert.ToBoolean(gvCupon.GetRowCellValue(e.RowHandle, "MovimientoID")))
                    {
                        vCantidadLitros = Decimal.Round(vCantidad * vEquivalencia, 3, MidpointRounding.AwayFromZero);

                        if (gvCupon.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            //Precio del combustible
                            if (!Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                                if (_HasDiferenciado)
                                    vPrecio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "ProductoID")), IDEstacionServicio, IDSubEstacion, true, 0, false, Fecha.Date)), 2, MidpointRounding.AwayFromZero);
                                else
                                    vPrecio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "ProductoID")), IDEstacionServicio, IDSubEstacion, false, 0, false, Fecha.Date)), 2, MidpointRounding.AwayFromZero);
                        }
                    }
                    
                    gvCupon.SetRowCellValue(e.RowHandle, "CantidadLitro", vCantidadLitros);
                    gvCupon.SetRowCellValue(e.RowHandle, "Precio", vPrecio);
                    
                    if (!Convert.ToBoolean(gvCupon.GetRowCellValue(e.RowHandle, "MovimientoID")))
                        vTotal = Decimal.Round(vCantidadLitros * vPrecio, 3, MidpointRounding.AwayFromZero);
                    else
                        vTotal = Decimal.Round(vCantidad * vEquivalencia, 3, MidpointRounding.AwayFromZero);

                    gvCupon.SetRowCellValue(e.RowHandle, "PrecioTotal", vTotal);
                    
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            if (e.Column == colProduct)
            {
                if (gvCupon.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                {
                    if (Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
                    {
                        return;
                    }
                        
                    else if (DetalleNCC.Count(d => d.CuponID.Equals(Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "CuponID"))) && d.ProductoID.Equals(Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                    {
                        Parametros.General.DialogMsg("El cupón y producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        gvCupon.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                        gvCupon.SetRowCellValue(e.RowHandle, "CantidadCupon", 0);
                        gvCupon.FocusedColumn = colProduct;
                        return;
                    }
                }
                else
                    return;



                gvCupon.SetRowCellValue(e.RowHandle, "CantidadCupon", 0);
            }
            //COLUMNA CUPON
            if (e.Column == colCupon)
            {
                if (gvCupon.GetRowCellValue(e.RowHandle, "CuponID") != null)
                {
                    if (Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "CuponID")) == 0)
                    {
                        return;
                    }

                }
                else
                    return;


                try
                {
                    var cup = _ListCupones.Single(s => s.ID.Equals(Convert.ToInt32(gvCupon.GetRowCellValue(e.RowHandle, "CuponID"))));
                    gvCupon.SetRowCellValue(e.RowHandle, colEsCuponCS, cup.EsCuponCS);
                    gvCupon.SetRowCellValue(e.RowHandle, colEquivalencia, cup.Equivalente);
                    gvCupon.SetRowCellValue(e.RowHandle, "CantidadCupon", 0);
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        private void gvCupon_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Columna de Cupon             
            if (view.GetRowCellValue(RowHandle, "CuponID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "CuponID")) == 0)
                {
                    view.SetColumnError(view.Columns["CuponID"], "Debe Seleccionar un Cupón");
                    e.ErrorText = "Debe Seleccionar un Cupón";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuponID"], "Debe Seleccionar un Cupón");
                e.ErrorText = "Debe Seleccionar un Cupón";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Productos             
            if (view.GetRowCellValue(RowHandle, "ProductoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Combustible");
                    e.ErrorText = "Debe Seleccionar un Combustible";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Combustible");
                e.ErrorText = "Debe Seleccionar un Combustible";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Cantidad
            //--
            if (view.GetRowCellValue(RowHandle, "CantidadCupon") != null)
            {
                if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadCupon")) <= 0.00)
                {

                    view.SetColumnError(view.Columns["CantidadCupon"], "La Cantidad debe ser mayor a cero");
                    e.ErrorText = "La Cantidad debe ser mayor a cero";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CantidadCupon"], "La Cantidad debe ser mayor a cero");
                e.ErrorText = "La Cantidad debe ser mayor a cero";
                e.Valid = false;
                Validate = false;
            }
                      

        }

        private void gvCupon_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Boton para cargar datos
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(true))
            {
                this.glkDeudor.Enabled = false;
                this.btnLoad.Enabled = false;
                this.dateDesde.Enabled = false;
                this.dateHasta.Enabled = false;
                this.bdsDetalleCuentas.DataSource = null;
                this.gvDetalle.OptionsBehavior.Editable = true;

                if (rgOption.SelectedIndex.Equals(1))
                {
                    if (!EsEmpleado)
                    {
                        client = db.Clientes.Single(s => s.ID.Equals(Convert.ToInt32(glkDeudor.EditValue)));
                        empl = null;

                            DetalleM = (from m in db.Movimientos
                                        where (m.MovimientoTipoID.Equals(7) || m.MovimientoTipoID.Equals(27)) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                        && !m.Anulado && !m.Pagado && m.ClienteID.Equals(Convert.ToInt32(glkDeudor.EditValue))
                                        select m).OrderBy(o => o.FechaRegistro).ToList();

                        DetalleM.Where(m => (m.FechaRegistro.Date >= Convert.ToDateTime(dateDesde.EditValue).Date && m.FechaRegistro.Date <= Convert.ToDateTime(dateHasta.EditValue).Date)).OrderBy(o => o.FechaRegistro).ToList().ForEach(item =>
                        {
                            decimal saldo = item.Monto - item.Abono;
                            item.Litros += saldo;
                            item.Pagado = true;
                        });

                        this.layoutControlItemEsAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.bdsDetalleCuentas.DataSource = DetalleM;
                        gvDetalle.RefreshData();
                    }
                    else if (EsEmpleado)
                    {
                        empl = db.Empleados.Single(s => s.ID.Equals(Convert.ToInt32(glkDeudor.EditValue)));
                        client = null;

                        DetalleM = (from m in db.Movimientos
                                    join c in db.ConceptoContables on m.ConceptoContableID equals c.ID into DefaultConcepto
                                    from cto in DefaultConcepto.DefaultIfEmpty()
                                    where (m.MovimientoTipoID.Equals(27) || (m.MovimientoTipoID.Equals(39) && cto.GeneraDocumento) || m.MovimientoTipoID.Equals(53)) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                    && !m.Anulado && !m.Pagado && m.EmpleadoID.Equals(Convert.ToInt32(glkDeudor.EditValue))
                                    select m).OrderBy(o => o.FechaRegistro).ToList();

                        DetalleM.Where(m => (m.FechaRegistro.Date >= Convert.ToDateTime(dateDesde.EditValue).Date && m.FechaRegistro.Date <= Convert.ToDateTime(dateHasta.EditValue).Date)).OrderBy(o => o.FechaRegistro).ToList().ForEach(item =>
                        {
                            decimal saldo = item.Monto - item.Abono;
                            item.Litros += saldo;
                            item.Pagado = true;
                        });

                        this.bdsDetalleCuentas.DataSource = DetalleM;
                        gvDetalle.RefreshData();
                    }
                }
                
            }
        }

        //Boton para cargar cupones anticipos
        private void btnCupones_Click(object sender, EventArgs e)
        {
            if (ValidarCampos(true) && !EsEmpleado)
            {
                try
                {
                    _ListCupones = (from cup in db.ExtracionPagos
                                          where cup.EsCupon && cup.Activo
                                          select new ListCupones { 
                                              ID = cup.ID, 
                                              Display = cup.Nombre, 
                                              EsCuponCS = cup.EsCuponMoneda, 
                                              Equivalente = (cup.EquivalenteLitros.Equals(0) ? cup.EquivalenteMoneda : cup.EquivalenteLitros),
                                              Orden = cup.Orden }).OrderBy(o => o.Orden).ToList();


                    
                    rpCupon.DataSource = _ListCupones.Select(s => new { s.ID, s.Display, s.EsCuponCS, s.Equivalente}).ToList();
                   
                    rpCombustible.DataSource = (from P in db.Productos
                                    join t in db.Tanques.Where(t => t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)) on P.ID equals t.ProductoID
                                                where P.Activo & P.ProductoClaseID.Equals(_ProductoClaseCombustible)
                                    group P by new { P.ID, P.Codigo, P.Nombre} into gr
                                    select new
                                    {
                                        ID = gr.Key.ID,
                                        Codigo = gr.Key.Codigo,
                                        Nombre = gr.Key.Nombre,
                                        Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                    }).OrderBy(o => o.Codigo).ToList();

                    this.bdsCupones.DataSource = this.DetalleNCC;
                    _Cupones = true;
                    splitContainerControlMain.SendToBack();
                    splitContainerControlMain.Visible = false;
                    splitContainerControlMain.Dock = DockStyle.None;

                    panelControlCupones.Visible = true;
                    panelControlCupones.BringToFront();
                    panelControlCupones.Dock = DockStyle.Fill;

                    dateDesde.Enabled = false;
                    dateHasta.Enabled = false;
                    lkTipoDeudor.Enabled = false;
                    dateFechaNota.Enabled = false;
                    btnCupones.Enabled = false;
                    btnLoad.Enabled = false;
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(true))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante de Nota Crédito / Débito";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
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

        //Boton nueva provision
        private void bntNew_Click(object sender, EventArgs e)
        {

            if (DetalleComprobante.Rows.Count > 0)
            {
                if (Parametros.General.DialogMsg("La Nota actual tiene datos registrados. ¿Desea cancelar esta nota y realizar una nueva?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            NextProvision = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();
        } 
                 
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaNota.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
        }
        
        //Seleccionar el tipo de deudor
        private void lkTipoDeudor_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkTipoDeudor.EditValue.Equals(1))
                {
                    layoutControlItem6.Text = "Cliente";
                    layoutControlItem4.Text = "Ruc / N. Comercial";
                    
                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.NullText = "<Seleccione al Cliente>";
                    EsEmpleado = false;

                    glkDeudor.Properties.DataSource = null;
                    glkDeudor.Properties.DataSource = (from c in db.Clientes
                                                       join ces in db.ClienteEstacions on c.ID equals ces.ClienteID
                                                       where ces.EstacionServicioID.Equals(IDEstacionServicio) && c.Activo
                                                       group c by new { ces.ClienteID, c.Codigo, c.Nombre } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ClienteID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombre,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                       }).ToList();
                    gridColumn3.Caption = "Clientes";

                    layoutControlItemEsAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    chkAnticipo.Checked = false;

                }
                else if (lkTipoDeudor.EditValue.Equals(2))
                {
                    layoutControlItem6.Text = "Empleado";
                    layoutControlItem4.Text = "Nro. INSS";
                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.NullText = "<Seleccione al Empleado>";
                    EsEmpleado = true;

                    glkDeudor.Properties.DataSource = null;
                    glkDeudor.Properties.DataSource = (from em in db.Empleados
                                                       join p in db.Planillas on em.PlanillaID equals p.ID
                                                       where em.Activo && (p.EstacionServicioID == IDEstacionServicio || em.EsMultiEstacion)
                                                       select new { em.ID, em.Codigo, Nombre = em.Nombres + " " + em.Apellidos, Display = em.Codigo + " | " + em.Nombres + " " + em.Apellidos }).ToList();
                    
                    layoutControlItemEsAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    gridColumn3.Caption = "Empleados";
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        //Seleccionar el tipo de Nota
        private void rgOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                if (rgOption.SelectedIndex.Equals(0))
                {
                    layoutControlItem19.Visibility = layoutControlItem20.Visibility = layoutControlItem21.Visibility = layoutControlItem22.Visibility = layoutControlItemEsAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Panel2;
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(27)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(27)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");

                    Cuentas = (from cc in db.CuentaContables
                               join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                               join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                               where cc.Detalle && !cc.Modular && cc.Activo && cces.EstacionID.Equals(IDEstacionServicio)
                               select new Parametros.ListIdDisplayCodeBool
                               {
                                   ID = cc.ID,
                                   Codigo = cc.Codigo,
                                   Display = cc.Nombre,
                                   valor = tc.UsaCentroCosto
                               }).OrderBy(o => o.Codigo);

                    //--- Fill Combos Detalles --//
                    gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                    gridCuenta.DataSource = null;
                    gridCuenta.DataSource = Cuentas;
                }
                else if (rgOption.SelectedIndex.Equals(1))
                {
                    layoutControlItem19.Visibility = layoutControlItem20.Visibility = layoutControlItem21.Visibility = layoutControlItem22.Visibility = layoutControlItemEsAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Both;
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(29)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(29)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");


                    //List<int> TiposCuenta = new List<int>();

                    //TiposCuenta.AddRange(new int[] { 26, 27, 32, 23, 29 });

                    Cuentas = (from cc in db.CuentaContables
                               join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                               join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                               where cc.Detalle && db.TipoCuentaModulo.Where(t => t.MovimientoTipoID.Equals(29)).Select(s => s.TipoCuentaID).Contains(tc.ID) && cc.Activo && cces.EstacionID.Equals(IDEstacionServicio)
                               select new Parametros.ListIdDisplayCodeBool
                               {
                                   ID = cc.ID,
                                   Codigo = cc.Codigo,
                                   Display = cc.Nombre,
                                   valor = tc.UsaCentroCosto
                               }).OrderBy(o => o.Codigo);


                    //--- Fill Combos Detalles --//
                    gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                    gridCuenta.DataSource = null;
                    gridCuenta.DataSource = Cuentas;

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Validar el cambio de seleccion del tipo de deudor
        private void lkTipoDeudor_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleComprobante.Rows.Count > 0)
            {
                Parametros.General.DialogMsg("La Nota tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        #endregion


        public DevExpress.XtraGrid.Views.Grid.GridView calor { get; set; }
    }
}