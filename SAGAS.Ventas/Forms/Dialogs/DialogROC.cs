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
    public partial class DialogROC : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormROC MDI;
        //internal Entidad.OrdenCompra EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private int _TipoPagoCheque;
        private decimal _PorcentajeComisionBancariaROC;
        private decimal _PorcentajeManejoCuentaROC;
        private int _CajaMonedaNacional;
        private int _CajaMonedaExtranjera;
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        internal bool EsEmpleado = false;
        internal bool _OtrosIngresos = false;
        internal bool _PagoGrupo = false;
        internal int _ConceptoAnticipo;
        internal bool _EsAnticipo = false;
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        internal int _CECO;
        internal int _PorcentajeComisionBancariaROCCuentaContable;
        internal int _PorcentajeManejoCuentaROCCuentaContable;
        internal int _CuentaGananciaDiferencialCambiarioID;

        private decimal _Comision
        {
            get { return Convert.ToDecimal(spComision.Value); }
            set { spComision.Value = value; }
        }
        
        private decimal _Manejo
        {
            get { return Convert.ToDecimal(spManejo.Value); }
            set { spManejo.Value = value; }
        }

        private decimal _Diferencial
        {
            get { return Convert.ToDecimal(spDifCambiario.Value); }
            set { spDifCambiario.Value = value; }
        }
       
        private string Referencia
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Comentario
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
        }

        private int IDMonedaPrincipal
        {
            get { return Convert.ToInt32(lkMoneda.EditValue); }
            set { lkMoneda.EditValue = value; }
        }
                
        private DateTime FechaRecibo
        {
            get { return Convert.ToDateTime(dateFechaRecibo.EditValue); }
            set { dateFechaRecibo.EditValue = value; }
        }

        private int IDDeudor
        {
            get { return Convert.ToInt32(glkDeudor.EditValue); }
            set { glkDeudor.EditValue = value; }
        }

        private int IDTipoPago
        {
            get { return Convert.ToInt32(lkTipoPago.EditValue); }
            set { lkTipoPago.EditValue = value; }
        }

        private int IDTransferencia
        {
            get { return Convert.ToInt32(lkTransferencia.EditValue); }
            set { lkTransferencia.EditValue = value; }
        }

        private decimal _Total
        {
            get { return Convert.ToDecimal(spTotal.Value); }
            set { spTotal.Value = value; }
        }

        private static Entidad.Cliente client;
        private static Entidad.Empleado empl;
        private static Entidad.TipoPago TipoPago;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;

        private List<Entidad.Movimiento> M = new List<Entidad.Movimiento>();
        public List<Entidad.Movimiento> DetalleM
        {
            get { return M; }
            set
            {
                M = value;
                this.bdsDetalle.DataSource = this.M;
            }
        }

        

        #endregion

        #region *** INICIO ***

        public DialogROC(int UserID, bool _editando)
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

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                rgOption.SelectedIndex = -1;
                _TipoPagoCheque = Parametros.Config.TipoPagoChequeID();
                _PorcentajeComisionBancariaROC = Parametros.Config.PorcentajeComisionBancariaROC();
                _PorcentajeManejoCuentaROC = Parametros.Config.PorcentajeManejoCuentaROC();
                _CajaMonedaNacional = Parametros.Config.CajaMonedaNacional();
                _CajaMonedaExtranjera = Parametros.Config.CajaMonedaExtranjera();
                _ConceptoAnticipo = Parametros.Config.ConceptoAnticipo();
                _PorcentajeComisionBancariaROCCuentaContable = Parametros.Config.PorcentajeComisionBancariaROCCuentaContable();
                _PorcentajeManejoCuentaROCCuentaContable = Parametros.Config.PorcentajeManejoCuentaROCCuentaContable();
                _CuentaGananciaDiferencialCambiarioID = Parametros.Config.CuentaGananciaDiferencialCambiarioID();
                
                this.layoutControlItemrgOptions.Width = 60;
                this.layoutControlItemFecha.Width = 150;
                this.emptySpaceItem1.Width = 300;

                var cecos = (from cto in db.CentroCostos
                        join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                        where ctoEs.EstacionID.Equals(IDEstacionServicio) && cto.Nombre.Contains("Pista")
                        select new { cto.ID }).FirstOrDefault();

                if (cecos != null)
                    _CECO = cecos.ID;

                //int number = 1;
                //if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(22)) > 0)
                //{
                //    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(22)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                //}

                //txtNumero.Text = number.ToString("000000000");
                
                lkMoneda.Properties.DataSource = from m in db.Monedas select new { m.ID, Display = m.Simbolo + " | " + m.Nombre };
                IDMonedaPrincipal = MonedaPrimaria;
                lkConcepto.Properties.DataSource = db.ConceptoContables.Where(cc => cc.Activo && cc.EsRoc).Select(s => new { s.ID, Display = s.Nombre }).ToList();
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Abreviatura });
                rpLkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre});
                dateFechaRecibo.EditValue = Convert.ToDateTime(db.GetDateServer());
                
                if (dateFechaRecibo.EditValue != null)
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaRecibo.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaRecibo.Date)).First().Valor : 0m);

                lkTipoPago.Properties.DataSource = db.TipoPagos.Where(t => t.Activo).Select(s => new { ID = s.ID, Display = s.Nombre });

                this.dateHasta.EditValue = FechaRecibo;
                this.dateDesde.EditValue = new DateTime(FechaRecibo.Year, FechaRecibo.Month, 1);
               
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
            Parametros.General.ValidateEmptyStringRule(dateFechaRecibo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkTipoPago, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
        }

        private bool ValidarReferencia(int code)
        {
            var result = (from i in db.Movimientos
                          where i.MovimientoTipoID.Equals(22) && i.Numero.Equals(code) && !i.Anulado && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (txtNumero.Text == "" || lkMoneda.EditValue == null || dateFechaRecibo.EditValue == null || lkTipoPago.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del Recibo Oficial de Caja.", Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarReferencia(Convert.ToInt32(Referencia)))
            {
                Parametros.General.DialogMsg("El número del Recibo Oficial de Caja '" + Referencia + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkDeudor.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Deudor para continuar con el proceso.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaRecibo, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (detalle)
            {
                if (_Total <= 0)
                {
                    Parametros.General.DialogMsg("El valor total del Recibo Oficial de Caja es 0 (cero), el valor debe ser mayor a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (IDTipoPago.Equals(_TipoPagoCheque))
                {
                    if (spNroCheque.Value.Equals(0) || String.IsNullOrEmpty(txtBanco.Text))
                    {
                        Parametros.General.DialogMsg("Completar el Nro de Cheque y el Banco." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }
                
                if (!spDiferencia.Value.Equals(0) && lkConcepto.EditValue == null && !rgOption.SelectedIndex.Equals(2))
                {
                    Parametros.General.DialogMsg("El Recibo Oficial de Caja tiene diferencia, debe seleccionar un concepto para referenciar la diferencia." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (lkConcepto.EditValue != null)
                {
                    var C = db.ConceptoContables.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkConcepto.EditValue)));

                    if (C != null)
                    {
                        if (spDiferencia.Value < 0)
                        {
                            if (spDiferencia.Value < C.Permisible)
                            {
                                Parametros.General.DialogMsg("El valor de la diferencia es menor al permisible del concepto:   " + C.Permisible.ToString() + Environment.NewLine, Parametros.MsgType.warning);
                                return false;
                            }
                        }
                        else if (spDiferencia.Value > 0)
                        {
                            if (!C.ID.Equals(_ConceptoAnticipo) && !_EsAnticipo)
                            {
                                if (spDiferencia.Value > C.Permisible)
                                {
                                    Parametros.General.DialogMsg("El valor de la diferencia es mayor al permisible del concepto:   " + C.Permisible.ToString() + Environment.NewLine, Parametros.MsgType.warning);
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (!_OtrosIngresos && !_EsAnticipo)
                {
                    if (DetalleM.Count <= 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (_OtrosIngresos)
                {
                    if (bdsDetalleOtros.Count < 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    else if (Decimal.Round(Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero) != (Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)))
                    {
                        Parametros.General.DialogMsg("No se recomienda guardas los datos con los totales descuadrados favor revisar: " + Environment.NewLine + Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero) + Environment.NewLine + Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText).ToString("#,0.00"), Parametros.MsgType.warning);
                        return false;
                    }

                }

                if (String.IsNullOrEmpty(Comentario))
                {
                    Parametros.General.DialogMsg("Debe Ingresar un Comentario.", Parametros.MsgType.warning);
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
                //if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                //{
                //    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                //    return false;
                //}
                                
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(true))
                return false;

            if (DetalleM.Count > 0)
            {
                if (DetalleM.Count(d => d.Litros > 0 && d.FechaRegistro.Date > FechaRecibo) > 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
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

                    Entidad.Movimiento M = new Entidad.Movimiento();
                    if (EsEmpleado)
                        M.EmpleadoID = IDDeudor;
                    else
                        M.ClienteID = IDDeudor;

                    M.MovimientoTipoID = 22;
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = FechaRecibo;
                    M.Monto = _Total;
                    M.MonedaID = IDMonedaPrincipal;
                    M.TipoCambio = _TipoCambio;
                    M.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : spMonto.Value);
                    M.Numero = Convert.ToInt32(Referencia);

                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.ConceptoContableID = (lkConcepto.EditValue != null ? Convert.ToInt32(lkConcepto.EditValue) : 0);
                    //if (_OtrosIngresos)
                    //{
                    //    M.CuentaID = Convert.ToInt32(gridCuenta.value);
                    //}
                    db.Movimientos.InsertOnSubmit(M);                    
                    db.SubmitChanges();
                
                    #region <<< REGISTRANDO MOVIMIENTO PAGO >>>

                    if (!IDTipoPago.Equals(0))
                    {
                        Entidad.MovimientoPago MP = new Entidad.MovimientoPago();

                        MP.TipoPagoID = TipoPago.ID;
                        MP.Monto = spMonto.Value;
                        MP.MonedaID = IDMonedaPrincipal;
                        MP.NroCheque = Convert.ToInt32(spNroCheque.Value);
                        MP.Banco = txtBanco.Text;
                        MP.ComisionBancaria = _Comision;
                        MP.ManejoCuenta = _Manejo;

                        M.MovimientoPagos.Add(MP);
                    }
                    #endregion

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

                    #region ::: REGISTRANDO DEUDOR :::

                    Entidad.Deudor D = new Entidad.Deudor();
                    if (EsEmpleado)
                        D.EmpleadoID = IDDeudor;
                    else
                        D.ClienteID = IDDeudor;

                    decimal Saldo = 0;
                    if (!_OtrosIngresos)
                    {
                        Saldo = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);

                        if (Convert.ToInt32(lkConcepto.EditValue).Equals(_ConceptoAnticipo) && _EsAnticipo && client.CuentaContableID.Equals(532))
                            Saldo += Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero);

                        D.Valor = -Math.Abs(Saldo);
                        D.MovimientoID = M.ID;
                        db.Deudors.InsertOnSubmit(D);
                        db.SubmitChanges();
                    }
                    else if (_OtrosIngresos)
                    {
                        Saldo = Decimal.Round(Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText), 2, MidpointRounding.AwayFromZero);

                        //if (Convert.ToInt32(lkConcepto.EditValue).Equals(_ConceptoAnticipo) && _EsAnticipo && client.CuentaContableID.Equals(532))
                            //Saldo += Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero);

                    }
                    

                    #endregion

                    #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>
                    if (!_OtrosIngresos)
                    {
                        string texto = "";

                        if (DetalleM.Count > 0)
                        {
                            if (DetalleM.Sum(s => s.Litros) > 0)
                            {
                                foreach (Entidad.Movimiento linea in DetalleM.Where(d => d.Litros > 0))
                                {
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
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(texto))
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + texto + ", esta Anulado.", Parametros.MsgType.warning);
                            return false;
                        }
                    }

                    #endregion

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                    "Se registró el ROC: " + M.Numero.ToString("000000000"), this.Name);
                    ////para que actualice los datos del registro
                    db.SubmitChanges();
                    trans.Commit();

                    IDPrint = M.ID;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    Next = true;                    
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
                    if (!_OtrosIngresos && (client == null && empl == null))
                    {
                        Parametros.General.DialogMsg("Debe seleccionar un Deudor.", Parametros.MsgType.warning);
                        return null;
                    }

                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                    if (!_OtrosIngresos)
                    {
                        if (client != null)
                        {
                            if (client.RealizaPagoGrupoES && client.EstacionPagoID.Equals(IDEstacionServicio)) // ES PAGO MASIVO
                            {
                                int gr = 1, i = 100;
                                int CuentaPrimera = 0;

                                if (!TipoPago.DeudorID.Equals(0))
                                {
                                    var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));
                                    CuentaPrimera = duedor.CuentaContableID;
                                }

                                if (CuentaPrimera.Equals(0))
                                    CuentaPrimera = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? _CajaMonedaNacional : _CajaMonedaExtranjera);

                                int IDCuentaActivoMasiva = db.EstacionServicios.Single(s => s.ID.Equals(IDEstacionServicio)).CuentaInternaActivo;

                                gr++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = CuentaPrimera,
                                    Monto = Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaRecibo,
                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                    Linea = gr,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });

                                var grupo = DetalleM.Where(d => d.Litros > 0).GroupBy(dm => dm.EstacionServicioID);

                                foreach (var obj in grupo)
                                {
                                    var ES = db.EstacionServicios.Select(s => new { s.ID, s.Nombre, s.GrupoAlcaldiaID, s.SubEstacionPrincipalID, s.CuentaInternaActivo, s.CuentaInternaPasivo }).Single(o => o.ID.Equals(obj.Key));

                                    //**************CUENTAS POR PAGAR CASA MATRIZ***************
                                    //Si la estacion de Pago es diferente
                                    if (!ES.ID.Equals(IDEstacionServicio))
                                    {
                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = IDCuentaActivoMasiva,
                                            Monto = Math.Abs(Decimal.Round(obj.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(obj.Sum(s => s.Litros / s.TipoCambio), 2, MidpointRounding.AwayFromZero) : obj.Sum(s => s.Litros))),
                                            Fecha = FechaRecibo,
                                            Descripcion = client.Nombre + "Cuenta por Cobrar. " + ES.Nombre + " con Recibo " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = ES.ID,
                                            SubEstacionID = ES.SubEstacionPrincipalID
                                        });
                                    }

                                    //Si la estacion de Pago es diferente
                                    if (!ES.ID.Equals(IDEstacionServicio))
                                    {
                                        gr++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = ES.CuentaInternaPasivo,
                                            Monto = -Math.Abs(obj.Sum(s => s.Litros)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(obj.Sum(s => s.Litros / s.TipoCambio), 2, MidpointRounding.AwayFromZero) : obj.Sum(s => s.Litros))),
                                            Fecha = FechaRecibo,
                                            Descripcion = "Cuenta por Pagar. " + ES.Nombre + " con ROC cliente compartido " + Referencia,
                                            Linea = gr,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                    }

                                    DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).ToList().ForEach(item =>
                                    {
                                        //**********PAGOS DE DOCUMENTOS**********
                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = client.CuentaContableID,
                                            Monto = -Math.Abs(Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.Litros)),
                                            Fecha = FechaRecibo,
                                            Descripcion = client.Nombre + " Pago Doc. " + item.Referencia + " con Recibo " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = item.EstacionServicioID,
                                            SubEstacionID = item.SubEstacionID
                                        });

                                    });
                                }

                                if (!spDiferencia.Value.Equals(0))
                                {
                                    if (lkConcepto.EditValue != null && !_EsAnticipo)
                                    {
                                        var C = db.ConceptoContables.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkConcepto.EditValue)));
                                        if (C != null)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = C.CuentaContableID,
                                                Monto = Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero) * -1,
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero) * -1,
                                                Fecha = FechaRecibo,
                                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Concepto " + lkConcepto.Text + "  – ROC Nro. " + Referencia,
                                                Linea = i,
                                                CentroCostoID = (spDiferencia.Value >= 0 ? 0 : _CECO),
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }
                                }


                                //MONTO ACREDITADO AL ANTICIPO
                                if (_EsAnticipo)
                                {
                                    //ID 526 Cuenta trnasitoria 9000000001
                                    int vCuenta = 0;

                                    if (EsEmpleado)
                                        vCuenta = 100;
                                    else
                                        vCuenta = (client.CuentaContableID.Equals(532) ? client.CuentaContableID : 526);

                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = vCuenta,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(spDiferencia.Value), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaRecibo,
                                        Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }


                                if (IDMonedaPrincipal.Equals(MonedaSecundaria) && !_EsAnticipo)
                                {
                                    if (_Diferencial > 0)
                                    {
                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = _CuentaGananciaDiferencialCambiarioID,
                                            Monto = -Math.Abs(Decimal.Round(_Diferencial, 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Diferencial / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaRecibo,
                                            Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Diferencial Cambiario – ROC Nro. " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                    }
                                }
                            }
                            else
                            {
                                int i = 0;

                                if (IDDeudor > 0)
                                {
                                    int CuentaPrimera = 0;

                                    if (!IDTipoPago.Equals(0))
                                    {
                                        //    if (IDTipoPago.Equals(_TipoPagoTransferencia))
                                        //    {
                                        //        if (IDTransferencia.Equals(0))
                                        //            Parametros.General.DialogMsg("Debe seleccionar una cuenta de transferencia o la Estación de Servicio de procedencia.", Parametros.MsgType.warning);

                                        //        if (IDTransferencia > 0)
                                        //        {
                                        //            if (chkPorEstacion.Checked)
                                        //            {
                                        //                CuentaPrimera = db.EstacionServicios.Single(es => es.ID.Equals(IDTransferencia)).CuentaInternaActivo;
                                        //            }
                                        //            else if (!chkPorEstacion.Checked)
                                        //            {
                                        //                CuentaPrimera = IDTransferencia;
                                        //            }
                                        //        }

                                        //    }

                                        if (!TipoPago.DeudorID.Equals(0))
                                        {
                                            var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));
                                            CuentaPrimera = duedor.CuentaContableID;
                                        }

                                        if (CuentaPrimera.Equals(0))
                                            CuentaPrimera = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? _CajaMonedaNacional : _CajaMonedaExtranjera);
                                    }

                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = CuentaPrimera,
                                        Monto = Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                        Fecha = FechaRecibo,
                                        Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }

                                #region <<< DETALLE_COMPROBANTE >>>

                                if (IDDeudor > 0)
                                {
                                    //if (bgvCuentas.RowCount > 0 && Convert.ToDecimal(bgvCuentas.GetRowCellValue(0, ColValor)) != 0.00m)
                                    //{//Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText)
                                    if (Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) > 0)
                                    {
                                        //MONTO ACREDITADO PARA EL CLIENTE
                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = (EsEmpleado ? 100 : client.CuentaContableID),
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleM.Sum(s => s.Litros)), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(GetMontoSecundario(), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaRecibo,
                                            Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                    }

                                    //MONTO ACREDITADO AL ANTICIPO
                                    if (_EsAnticipo)
                                    {
                                        //ID 526 Cuenta trnasitoria 9000000001
                                        int vCuenta = 0;

                                        if (EsEmpleado)
                                            vCuenta = 100;
                                        else
                                            vCuenta = (client.CuentaContableID.Equals(532) ? client.CuentaContableID : 526);

                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = vCuenta,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(spDiferencia.Value), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaRecibo,
                                            Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                    }

                                    if (TipoPago != null)
                                    {
                                        if (!TipoPago.DeudorID.Equals(0))
                                        {
                                            var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));

                                            if (_Comision > 0)
                                            {
                                                i++;
                                                CD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = _PorcentajeComisionBancariaROCCuentaContable,
                                                    Monto = -Math.Abs(Decimal.Round(_Comision, 2, MidpointRounding.AwayFromZero)),
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Comision / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                    Fecha = FechaRecibo,
                                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Comisión Bancaria – ROC Nro. " + Referencia,
                                                    Linea = i,
                                                    CentroCostoID = _CECO,
                                                    EstacionServicioID = IDEstacionServicio,
                                                    SubEstacionID = IDSubEstacion
                                                });
                                            }

                                            if (_Manejo > 0)
                                            {
                                                i++;
                                                CD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = _PorcentajeManejoCuentaROCCuentaContable,
                                                    Monto = -Math.Abs(Decimal.Round(_Manejo, 2, MidpointRounding.AwayFromZero)),
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Manejo / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                    Fecha = FechaRecibo,
                                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Manejo de Cuentas – ROC Nro. " + Referencia,
                                                    Linea = i,
                                                    CentroCostoID = 0,
                                                    EstacionServicioID = IDEstacionServicio,
                                                    SubEstacionID = IDSubEstacion
                                                });
                                            }
                                        }
                                    }

                                    if (IDMonedaPrincipal.Equals(MonedaSecundaria) && !_EsAnticipo)
                                    {
                                        if (_Diferencial > 0)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = _CuentaGananciaDiferencialCambiarioID,
                                                Monto = -Math.Abs(Decimal.Round(_Diferencial, 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Diferencial / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                Fecha = FechaRecibo,
                                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Diferencial Cambiario – ROC Nro. " + Referencia,
                                                Linea = i,
                                                CentroCostoID = 0,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }

                                    if (!spDiferencia.Value.Equals(0))
                                    {
                                        if (lkConcepto.EditValue != null && !_EsAnticipo)
                                        {
                                            var C = db.ConceptoContables.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkConcepto.EditValue)));
                                            if (C != null)
                                            {
                                                i++;
                                                CD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = C.CuentaContableID,
                                                    Monto = Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero) * -1,
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero) * -1,
                                                    Fecha = FechaRecibo,
                                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Concepto " + lkConcepto.Text + "  – ROC Nro. " + Referencia,
                                                    Linea = i,
                                                    CentroCostoID = (spDiferencia.Value >= 0 ? 0 : _CECO),
                                                    EstacionServicioID = IDEstacionServicio,
                                                    SubEstacionID = IDSubEstacion
                                                });
                                            }
                                        }
                                    }

                                    //}
                                }
                                #endregion
                            }
                        }
                        else //DIFERENTE A PAGOS MASIVOS
                        {
                            int i = 0;

                            if (IDDeudor > 0)
                            {
                                int CuentaPrimera = 0;

                                if (!IDTipoPago.Equals(0))
                                {
                                    //    if (IDTipoPago.Equals(_TipoPagoTransferencia))
                                    //    {
                                    //        if (IDTransferencia.Equals(0))
                                    //            Parametros.General.DialogMsg("Debe seleccionar una cuenta de transferencia o la Estación de Servicio de procedencia.", Parametros.MsgType.warning);

                                    //        if (IDTransferencia > 0)
                                    //        {
                                    //            if (chkPorEstacion.Checked)
                                    //            {
                                    //                CuentaPrimera = db.EstacionServicios.Single(es => es.ID.Equals(IDTransferencia)).CuentaInternaActivo;
                                    //            }
                                    //            else if (!chkPorEstacion.Checked)
                                    //            {
                                    //                CuentaPrimera = IDTransferencia;
                                    //            }
                                    //        }

                                    //    }

                                    if (!TipoPago.DeudorID.Equals(0))
                                    {
                                        var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));
                                        CuentaPrimera = duedor.CuentaContableID;
                                    }

                                    if (CuentaPrimera.Equals(0))
                                        CuentaPrimera = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? _CajaMonedaNacional : _CajaMonedaExtranjera);
                                }

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = CuentaPrimera,
                                    Monto = Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaRecibo,
                                    Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            #region <<< DETALLE_COMPROBANTE >>>

                            if (IDDeudor > 0)
                            {
                                //if (bgvCuentas.RowCount > 0 && Convert.ToDecimal(bgvCuentas.GetRowCellValue(0, ColValor)) != 0.00m)
                                //{//Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText)
                                if (Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) > 0)
                                {
                                    //MONTO ACREDITADO PARA EL CLIENTE
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = (EsEmpleado ? 100 : client.CuentaContableID),
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleM.Sum(s => s.Litros)), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(GetMontoSecundario(), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaRecibo,
                                        Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }

                                //MONTO ACREDITADO AL ANTICIPO
                                if (_EsAnticipo)
                                {
                                    //ID 526 Cuenta trnasitoria 9000000001
                                    int vCuenta = 0;

                                    if (EsEmpleado)
                                        vCuenta = 100;
                                    else
                                        vCuenta = (client.CuentaContableID.Equals(532) ? client.CuentaContableID : 526);

                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = vCuenta,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(spDiferencia.Value), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaRecibo,
                                        Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }

                                if (TipoPago != null)
                                {
                                    if (!TipoPago.DeudorID.Equals(0))
                                    {
                                        var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));

                                        if (_Comision > 0)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = _PorcentajeComisionBancariaROCCuentaContable,
                                                Monto = -Math.Abs(Decimal.Round(_Comision, 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Comision / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                Fecha = FechaRecibo,
                                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Comisión Bancaria – ROC Nro. " + Referencia,
                                                Linea = i,
                                                CentroCostoID = _CECO,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }

                                        if (_Manejo > 0)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = _PorcentajeManejoCuentaROCCuentaContable,
                                                Monto = -Math.Abs(Decimal.Round(_Manejo, 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Manejo / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                Fecha = FechaRecibo,
                                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Manejo de Cuentas – ROC Nro. " + Referencia,
                                                Linea = i,
                                                CentroCostoID = 0,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }
                                }

                                if (IDMonedaPrincipal.Equals(MonedaSecundaria) && !_EsAnticipo)
                                {
                                    if (_Diferencial > 0)
                                    {
                                        i++;
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = _CuentaGananciaDiferencialCambiarioID,
                                            Monto = -Math.Abs(Decimal.Round(_Diferencial, 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Diferencial / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaRecibo,
                                            Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Diferencial Cambiario – ROC Nro. " + Referencia,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                    }
                                }

                                if (!spDiferencia.Value.Equals(0))
                                {
                                    if (lkConcepto.EditValue != null && !_EsAnticipo)
                                    {
                                        var C = db.ConceptoContables.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkConcepto.EditValue)));
                                        if (C != null)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = C.CuentaContableID,
                                                Monto = Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero) * -1,
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(spDiferencia.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero) * -1,
                                                Fecha = FechaRecibo,
                                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " Concepto " + lkConcepto.Text + "  – ROC Nro. " + Referencia,
                                                Linea = i,
                                                CentroCostoID = (spDiferencia.Value >= 0 ? 0 : _CECO),
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }
                                }

                                //}
                            }
                            #endregion

                        }

                        return CD.OrderBy(o => o.Linea).ToList();
                        
                    }
                    else if (_OtrosIngresos)
                    {
                        int i = 0;

                        if (IDDeudor > 0)
                        {
                            int CuentaPrimera = 0;


                            if (!IDTipoPago.Equals(0))
                            {
                                //    if (IDTipoPago.Equals(_TipoPagoTransferencia))
                                //    {
                                //        if (IDTransferencia.Equals(0))
                                //            Parametros.General.DialogMsg("Debe seleccionar una cuenta de transferencia o la Estación de Servicio de procedencia.", Parametros.MsgType.warning);

                                //        if (IDTransferencia > 0)
                                //        {
                                //            if (chkPorEstacion.Checked)
                                //            {
                                //                CuentaPrimera = db.EstacionServicios.Single(es => es.ID.Equals(IDTransferencia)).CuentaInternaActivo;
                                //            }
                                //            else if (!chkPorEstacion.Checked)
                                //            {
                                //                CuentaPrimera = IDTransferencia;
                                //            }
                                //        }

                                //    }

                                if (!TipoPago.DeudorID.Equals(0))
                                {
                                    var duedor = db.Clientes.Single(cl => cl.ID.Equals(TipoPago.DeudorID));
                                    CuentaPrimera = duedor.CuentaContableID;
                                }

                                if (CuentaPrimera.Equals(0))
                                    CuentaPrimera = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? _CajaMonedaNacional : _CajaMonedaExtranjera);
                            }

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = CuentaPrimera,
                                Monto = Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Total),
                                Fecha = FechaRecibo,
                                Descripcion = (EsEmpleado ? empl.Nombres + " " + empl.Apellidos : client.Nombre) + " – ROC Nro. " + Referencia,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        #region <<< DETALLE_COMPROBANTE >>>

                        if (IDDeudor > 0)
                        {
                            if (bgvCuentas.RowCount > 0 && Convert.ToDecimal(bgvCuentas.GetRowCellValue(0, ColValor)) != 0.00m)
                            {
                                #region <<< DETALLE_COMPROBANTE >>>
                                //int i = 1;
                                decimal _vMP = 0, _vMS = 0;

                                //DetalleComprobante.Rows. .ToList().ForEach(K =>
                                foreach (DataRow linea in DetalleComprobante.Rows)
                                {

                                    _vMP += (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])) :
                                    Decimal.Round((Convert.ToDecimal(linea["Monto"]) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                                    _vMS += (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                    Convert.ToDecimal(linea["Monto"]));

                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                        Monto = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? -Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])) :
                                        -Decimal.Round((Convert.ToDecimal(linea["Monto"]) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? -Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                        -Convert.ToDecimal(linea["Monto"])),
                                        Fecha = FechaRecibo,
                                        Descripcion = Convert.ToString(linea["Descripcion"]),
                                        Linea = i,
                                        CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;
                                }

                                #endregion
                            }
                        }
                        #endregion

                        return CD.OrderBy(o => o.Linea).ToList();
                    }
                    else
                        return null;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }

        }

        private decimal GetMontoSecundario()
        {
            try
            {
                decimal monto = 0m;
                if (!_OtrosIngresos)
                {
                    DetalleM.Where(m => m.Litros > 0).ToList().ForEach(det =>
                        {
                            monto += Decimal.Round(Decimal.Round(det.Litros, 2, MidpointRounding.AwayFromZero) / Decimal.Round(_TipoCambio, 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                        });
                }
                else if (_OtrosIngresos)
                {
                    for(int dt = 0; dt < bgvCuentas.RowCount; dt++)
                        monto += Decimal.Round(Decimal.Round(Convert.ToDecimal(bgvCuentas.GetRowCellValue(dt, ColValor)), 2, MidpointRounding.AwayFromZero)/Decimal.Round(_TipoCambio, 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero);
                            //Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText);
                }
                return monto;
            }
            catch { return 0m; }
        }

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

        #endregion
               
        #region *** EVENTOS ***

        //--GUARDAR
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, Next, IDPrint, RefreshMDI);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (!_OtrosIngresos)
                {
                    if (DetalleM.Count > 0 || spMonto.Value > 0)
                    {
                        if (Parametros.General.DialogMsg("El Recibo actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        {
                            Next = false;
                            e.Cancel = true;
                        }
                    }
                }
                else if (_OtrosIngresos)
                {
                    if (DetalleComprobante != null)
                    {
                        if (DetalleComprobante.Rows.Count > 0 || spMonto.Value > 0)
                        {
                            if (Parametros.General.DialogMsg("El Recibo actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            {
                                Next = false;
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }

        }

        //--CANCELAR
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--VALIDACION CAMPOS
        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Accesos Directos por Botones
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                bntNew_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Nuevo ROC
        private void bntNew_Click(object sender, EventArgs e)
        {
            Next = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();
        }

        //Selección del Deudor(glkDeudor_EditValuChanged)
        private void glkClient_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkDeudor.EditValue) > 0)
                {
                    if (!EsEmpleado)
                    {
                        empl = null;
                        client = db.Clientes.Single(p => p.ID.Equals(IDDeudor));

                        //var obj = (from d in db.Deudors
                        //           join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)) on d.MovimientoID equals m.ID
                        //           join c in db.Clientes.Where(c => c.ID.Equals(client.ID)) on d.ClienteID equals c.ID
                        //           group d by new { d.ClienteID, c.Nombre } into gr
                        //           select new
                        //           {
                        //               ID = gr.Key.ClienteID,
                        //               Cliente = gr.Key.Nombre,
                        //               suma = gr.Sum(s => s.Valor)
                        //           }).ToList();

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
                    else if (EsEmpleado)
                    {
                        client = null;
                        empl = db.Empleados.Single(p => p.ID.Equals(IDDeudor));

                        var obj = (from d in db.Deudors
                                   join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)) on d.MovimientoID equals m.ID
                                   join em in db.Empleados.Where(c => c.ID.Equals(empl.ID)) on d.EmpleadoID equals em.ID
                                   group d by new { d.EmpleadoID, em.Nombres, em.Apellidos } into gr
                                   select new
                                   {
                                       ID = gr.Key.EmpleadoID,
                                       Empleado = gr.Key.Nombres + " " + gr.Key.Apellidos,
                                       suma = gr.Sum(s => s.Valor)
                                   }).ToList();

                        spLimite.Value = 0;// + emp.LimiteCredito.ToString("#,0.00"));

                        decimal Saldo = (db.Deudors.Count(d => d.EmpleadoID.Equals(IDDeudor)) > 0 ? db.Deudors.Where(d => d.EmpleadoID.Equals(IDDeudor)).Sum(s => s.Valor) : 0m);
                        spSaldo.Value = Saldo;

                        decimal _Disponible = 0 - Saldo;
                        spDisponible.Value = _Disponible;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //Cambiar las opciones del ROC
        private void rgOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgOption.SelectedIndex.Equals(0))
                {
                    this.gridCuentas.Visible = false;
                    this.gridCuentas.Dock = DockStyle.None;
                    this.gridDetalle.Visible = true;
                    this.gridDetalle.Dock = DockStyle.Fill;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    btnLoad.Text = "Cargar Lista";
                    layoutControlItemTipo.Text = "Cliente";
                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.NullText = "<Seleccione al Cliente>";
                    EsEmpleado = false;
                    _OtrosIngresos = false;
                    spLimite.Value = 0;
                    spSaldo.Value = 0;
                    spDisponible.Value = 0;
                    this.dateDesde.Enabled = true;
                    this.dateHasta.Enabled = true;

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
                }
                else if (rgOption.SelectedIndex.Equals(1))
                {
                    this.gridCuentas.Visible = false;
                    this.gridCuentas.Dock = DockStyle.None;
                    this.gridDetalle.Visible = true;
                    this.gridDetalle.Dock = DockStyle.Fill;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    _OtrosIngresos = false;
                    btnLoad.Text = "Cargar Lista";
                    layoutControlItemTipo.Text = "Empleado";
                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.NullText = "<Seleccione al Empleado>";
                    EsEmpleado = true;
                    spLimite.Value = 0;
                    spSaldo.Value = 0;
                    spDisponible.Value = 0;
                    this.dateDesde.Enabled = true;
                    this.dateHasta.Enabled = true;

                    glkDeudor.Properties.DataSource = null;
                    glkDeudor.Properties.DataSource = (from em in db.Empleados
                                                       join p in db.Planillas on em.PlanillaID equals p.ID
                                                       where em.Activo && (p.EstacionServicioID == IDEstacionServicio || em.EsMultiEstacion)
                                                       select new { em.ID, em.Codigo, Nombre = em.Nombres + " " + em.Apellidos, Display = em.Codigo + " | " + em.Nombres + " " + em.Apellidos }).ToList();
                }
                else if (rgOption.SelectedIndex.Equals(2))
                {
                    this.gridDetalle.Visible = false;
                    this.gridDetalle.Dock = DockStyle.None;
                    this.gridCuentas.Visible = true;
                    this.gridCuentas.Dock = DockStyle.Fill;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _OtrosIngresos = true;
                    EsEmpleado = false;
                    btnLoad.Text = "Cargar Cuentas";
                    layoutControlItemTipo.Text = "Cliente";
                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.NullText = "<Seleccione al Cliente>";
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
                    this.dateDesde.Enabled = false;
                    this.dateHasta.Enabled = false;
                    spLimite.Value = 0;
                    spSaldo.Value = 0;
                    spDisponible.Value = 0;


                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void dateFechaRecibo_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaRecibo.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaRecibo.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaRecibo.Date)).First().Valor : 0m);
        }

        private void lkMoneda_EditValueChanged(object sender, EventArgs e)
        {
            if (lkMoneda.EditValue != null)
            {
                //lblGrandTotal.Text = (_EsContado ? "TOTAL FACTURA: " : "TOTAL: ") + db.Monedas.Single(m => m.ID.Equals(Convert.ToInt32(lkMoneda.EditValue))).Simbolo;

                if (Convert.ToInt32(lkMoneda.EditValue).Equals(MonedaPrimaria))
                {
                    lkMoneda.BackColor = Color.White;
                    lkMoneda.ForeColor = Color.Black;
                    spMonto.BackColor = Color.White;
                    spMonto.ForeColor = Color.Black;
                    //txtGrandTotal.BackColor = Color.FromArgb(232, 249, 239);
                    //txtGrandTotal.ForeColor = Color.Black;
                }
                else if (Convert.ToInt32(lkMoneda.EditValue).Equals(MonedaSecundaria))
                {
                    lkMoneda.BackColor = Color.ForestGreen;
                    lkMoneda.ForeColor = Color.White;
                    spMonto.BackColor = Color.ForestGreen;
                    spMonto.ForeColor = Color.White;
                    //txtGrandTotal.BackColor = Color.ForestGreen;
                    //txtGrandTotal.ForeColor = Color.White;
                }

                spMonto_EditValueChanged(null, null);

            }
        }

        //Seleccionar el tipo de pago
        private void lkTipoPago_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!IDTipoPago.Equals(0))
                {
                    TipoPago = db.TipoPagos.Single(t => t.ID.Equals(IDTipoPago));
                    this.spNroCheque.Value = 0;
                    this.txtBanco.Text = "";
                    this.chkPorEstacion.Checked = false;
                    this.chkComisionBancaria.Checked = false;
                    this.chkManejoCuenta.Checked = false;
                    this.lkMoneda.Enabled = true;
                    this.lkTransferencia.Properties.DataSource = null;
                    lkTransferencia.EditValue = null;
                    this.layoutControlItemComision.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.layoutControlItemNro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlItemBanco.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlItemPorEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlItemTransferencia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlItemComisionBancaria.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlItemManejoCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    if (IDTipoPago.Equals(_TipoPagoCheque))
                    {
                        this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        this.layoutControlItemNro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.layoutControlItemBanco.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }

                    //if (IDTipoPago.Equals(_TipoPagoTransferencia))
                    //{
                    //this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //this.layoutControlItemPorEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //this.layoutControlItemTransferencia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //this.layoutControlItemTransferencia.Width = 300;
                    //var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle && cc.EsBanco select new { cc.ID, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                    //this.lkTransferencia.Properties.DataSource = lista;
                    //}

                    if (!TipoPago.DeudorID.Equals(0))
                    {
                        this.emptySpaceItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        this.layoutControlItemComisionBancaria.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                        IDMonedaPrincipal = 1;
                        this.lkMoneda.Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cargar la lista de los pagos del Deudor
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidarCampos(false))
                {
                    this.rgOption.Enabled = false;
                    this.glkDeudor.Enabled = false;
                    this.btnLoad.Enabled = false;
                    this.dateDesde.Enabled = false;
                    this.dateHasta.Enabled = false;
                    this.dateFechaRecibo.Enabled = false;
                    this.lkMoneda.Enabled = false;
                    this.lkTipoPago.Enabled = false;
                    this.bdsDetalle.DataSource = null;

                    if (!_OtrosIngresos)
                    {
                        if (!EsEmpleado)
                        {
                            client = db.Clientes.Single(s => s.ID.Equals(IDDeudor));
                            empl = null;

                            if (client.RealizaPagoGrupoES && client.EstacionPagoID.Equals(IDEstacionServicio))
                            {
                                _PagoGrupo = true;
                                DetalleM = (from m in db.Movimientos
                                            where (m.MovimientoTipoID.Equals(7) || m.MovimientoTipoID.Equals(27))
                                            && !m.Anulado && !m.Pagado && m.ClienteID.Equals(IDDeudor)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }
                            else
                            {
                                DetalleM = (from m in db.Movimientos
                                            where (m.MovimientoTipoID.Equals(7) || m.MovimientoTipoID.Equals(27)) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                            && !m.Anulado && !m.Pagado && m.ClienteID.Equals(IDDeudor)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }

                            DetalleM.Where(m => (m.FechaRegistro.Date >= Convert.ToDateTime(dateDesde.EditValue).Date && m.FechaRegistro.Date <= Convert.ToDateTime(dateHasta.EditValue).Date)).OrderBy(o => o.FechaRegistro).ToList().ForEach(item =>
                            {
                                decimal saldo = item.Monto - item.Abono;
                                item.Litros += saldo;
                                item.Pagado = true;
                            });

                            this.bdsDetalle.DataSource = DetalleM;
                            gvDetalle.RefreshData();
                        }
                        else if (EsEmpleado)
                        {
                            empl = db.Empleados.Single(s => s.ID.Equals(IDDeudor));
                            client = null;

                            DetalleM = (from m in db.Movimientos
                                        where (m.MovimientoTipoID.Equals(53) || m.MovimientoTipoID.Equals(27)) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                        && !m.Anulado && !m.Pagado && m.EmpleadoID.Equals(IDDeudor)
                                        select m).OrderBy(o => o.FechaRegistro).ToList();

                            DetalleM.Where(m => (m.FechaRegistro.Date >= Convert.ToDateTime(dateDesde.EditValue).Date && m.FechaRegistro.Date <= Convert.ToDateTime(dateHasta.EditValue).Date)).OrderBy(o => o.FechaRegistro).ToList().ForEach(item =>
                            {
                                decimal saldo = item.Monto - item.Abono;
                                item.Litros += saldo;
                                item.Pagado = true;
                            });

                            this.bdsDetalle.DataSource = DetalleM;
                            gvDetalle.RefreshData();
                        }
                    }
                    else
                    {
                        bgvCuentas.OptionsBehavior.Editable = true;

                        client = db.Clientes.Single(s => s.ID.Equals(IDDeudor));
                        empl = null;


                        this.CentrosCostos = from cto in db.CentroCostos
                                             join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                             where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                             select new Parametros.ListIdDisplay { ID = cto.ID, Display = cto.Nombre };

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
                        gridCuenta.DisplayMember = "Codigo";
                        gridCuenta.ValueMember = "ID";

                        //Centro Costo GRID
                        lkCentroCosto.DataSource = CentrosCostos;
                        lkCentroCosto.DisplayMember = "Display";
                        lkCentroCosto.ValueMember = "ID";

                        var listComprobante = (from cd in db.ComprobanteContables
                                               join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                               join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                               where cd.MovimientoID.Equals(0)
                                               select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                        this.DetalleComprobante = ToDataTable(listComprobante);

                        this.bdsDetalleOtros.DataSource = this.DetalleComprobante;
                    }


                    if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                    {
                        this.colTC.Visible = true;
                        this.colTC.VisibleIndex = 5;
                        this.colMontoUS.Visible = true;
                        this.colMontoUS.VisibleIndex = 6;
                        this.layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }

                    spMonto_EditValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cargar la lista de transferencia por E/S o por cuentas bancarias
        private void chkPorEstacion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPorEstacion.Checked)
                {
                    this.lkTransferencia.Properties.DataSource = null;
                    lkTransferencia.EditValue = null;
                    var lista = (from es in db.EstacionServicios where es.Activo && !es.ID.Equals(IDEstacionServicio) select new { es.ID, Display = es.Nombre }).OrderBy(o => o.Display).ToList();
                    this.lkTransferencia.Properties.DataSource = lista;
                }
                else if (!chkPorEstacion.Checked)
                {
                    this.lkTransferencia.Properties.DataSource = null;
                    lkTransferencia.EditValue = null;
                    var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle && cc.EsBanco select new { cc.ID, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                    this.lkTransferencia.Properties.DataSource = lista;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void spMonto_EditValueChanged(object sender, EventArgs e)
        {
            GetComision();
            _Total = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Convert.ToDecimal(spMonto.Value) + _Comision + _Manejo :
                            Decimal.Round((Convert.ToDecimal(spMonto.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

            if (IDMonedaPrincipal.Equals(MonedaSecundaria) && !_EsAnticipo)
            {
                decimal cambiario = _Total - Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);

                _Diferencial = (cambiario > 0 ? cambiario : 0m);
            }
            else
                _Diferencial = 0;

            decimal vCuentas = 0;

            if (DetalleComprobante != null)
            {
                if (DetalleComprobante.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(bgvCuentas.Columns["Monto"].SummaryText))
                    {
                        vCuentas = Convert.ToDecimal(bgvCuentas.Columns["Monto"].SummaryText);
                    }
                }
            }

            decimal Dif = (_Total - _Comision - _Manejo - _Diferencial) - Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero) - vCuentas;
            layoutControlDif.Visible = false;
            spDiferencia.Value = 0;

            if (Dif > 0)
            {
                layoutControlDif.Visible = true;
                spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.ForestGreen;
                layoutControlItemDif.Text = "Sobrante";
                spDiferencia.Value = Dif;
            }
            else if (Dif < 0)
            {
                layoutControlDif.Visible = true;
                spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Red;
                layoutControlItemDif.Text = "Faltante";
                spDiferencia.Value = Dif;
            }

        }

        private void chkComisionBancaria_CheckedChanged(object sender, EventArgs e)
        {
            spMonto_EditValueChanged(null, null);
        }

        private void chkManejoCuenta_CheckedChanged(object sender, EventArgs e)
        {
            spMonto_EditValueChanged(null, null);
        }

        private void GetComision()
        {
            decimal Subtotal = spMonto.Value;
            if (chkComisionBancaria.Checked)
            {
                this.layoutControlItemComision.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                _Comision = Decimal.Round(Convert.ToDecimal(spMonto.Value) * Convert.ToDecimal(_PorcentajeComisionBancariaROC / 100m), 2, MidpointRounding.AwayFromZero);
                Subtotal = spMonto.Value - _Comision;

                this.layoutControlItemManejoCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

            }
            else if (!chkComisionBancaria.Checked)
            {
                _Comision = 0;
                Subtotal = spMonto.Value;
                chkManejoCuenta.Checked = false;
                this.layoutControlItemComision.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.layoutControlItemManejoCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            if (chkManejoCuenta.Checked)
            {
                this.layoutControlItemManejo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                _Manejo = Decimal.Round(Convert.ToDecimal(Subtotal) * Convert.ToDecimal(_PorcentajeComisionBancariaROC / 100m), 2, MidpointRounding.AwayFromZero);
                Subtotal = spMonto.Value - _Manejo;
            }
            else if (!chkManejoCuenta.Checked)
            {
                _Manejo = 0;
                this.layoutControlItemManejo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            //decimal comision = (chkComisionBancaria.Checked ? Decimal.Round(Convert.ToDecimal(spMonto.Value) * Convert.ToDecimal(_PorcentajeComisionBancariaROC / 100m), 2, MidpointRounding.AwayFromZero) : 0);
            //decimal Manejo = (chkManejoCuenta.Checked ? Decimal.Round(Convert.ToDecimal(spMonto.Value) * Convert.ToDecimal(_PorcentajeManejoCuentaROC / 100m), 2, MidpointRounding.AwayFromZero) : 0);
            //spComision.Value = comision + Manejo;
        }
        
        //Tipos de Conceptos para Sobrantes / Faltantes
        private void lkConcepto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkConcepto.EditValue != null)
                {
                    if (Convert.ToInt32(lkConcepto.EditValue).Equals(_ConceptoAnticipo))
                        _EsAnticipo = true;
                    else
                        _EsAnticipo = false;

                    spMonto_EditValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }
        
        #region <<< GRID DOCUMENTOS >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            
            //-- Validar Columna de Tanque          
          

        }

        //Mensaje Validación del detalle
        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMonto)
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                    {
                        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                        {
                            decimal vSaldo = 0, vAbono = 0;

                            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                            if (vAbono > vSaldo)
                            {
                                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                            }
                            else
                            {
                                if (vAbono.Equals(vSaldo))
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                else
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                            }
                        }
                        else
                        {
                            gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                        }
                    }
                    else
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0);
                    }

                    gvDetalle.RefreshData();
                    spMonto_EditValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< GRID CUENTAS CONTABLES >>>

        private void bgvCuentas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            spMonto_EditValueChanged(null, null);
        }

        private void bgvCuentas_LostFocus(object sender, EventArgs e)
        {
            spMonto_EditValueChanged(null, null);
        }

        //Validar Filas de las Cuentas
        private void bgvCuentas_ValidateRow(object sender, ValidateRowEventArgs e)
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

        //Mensaje Validación del detalle
        private void bgvCuentas_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void bgvCuentas_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Linea", 0);

                    if (bgvCuentas.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(bgvCuentas.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        bgvCuentas.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(bgvCuentas.GetRowCellValue(bgvCuentas.RowCount - 2, "Descripcion")));
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            spMonto_EditValueChanged(null, null);
            //spDiferencia.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");

            #endregion
        }

        //metodos para borrar y agregar nueva fila
        private void bgvCuentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        bgvCuentas.RefreshData();
                        spMonto_EditValueChanged(null, null);

                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                view.AddNewRow();
                bgvCuentas.RefreshData();
                spMonto_EditValueChanged(null, null);
            }
        }

        private void bgvCuentas_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (bgvCuentas.FocusedColumn == colCentroCosto)
            {
                if (bgvCuentas.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(bgvCuentas.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }

            }
        }

        #endregion

        //Botón para mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(true))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Recibo Oficial de Caja";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

    }
}