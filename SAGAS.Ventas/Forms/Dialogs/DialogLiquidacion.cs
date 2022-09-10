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
    public partial class DialogLiquidacion : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormLiquidaciones MDI;
        //internal Entidad.OrdenCompra EntidadAnterior;
        private Parametros.ListTipoDepositos listadoDeposito = new Parametros.ListTipoDepositos();
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        internal int _ClienteCordoba;
        internal int _ClienteDolar;
        internal int _CuentaPorCobrarEmpleado;
        private static Entidad.Cliente client;
        private static Entidad.Empleado empl;
        private static Entidad.TipoPago TipoPago;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private List<Entidad.MinutasDeposito> DetalleMinutas = new List<Entidad.MinutasDeposito>();
        private decimal _MargenDiferenciaCambiaria = 0;

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

        private int TipoMovimiento
        {
            get { return Convert.ToInt32(lkTipoMov.EditValue); }
            set { lkTipoMov.EditValue = value; }
        }

        private int AreaVenta
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private DateTime FechaDeposito
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        private decimal _Diferencial
        {
            get { return Convert.ToDecimal(spDifCambiario.Value); }
            set { spDifCambiario.Value = value; }
        }

        private decimal _MontoTotal
        {
            get { return Convert.ToDecimal(spTotal.Value); }
            set { spTotal.Value = value; }
        }        
       
        private string Numero
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

        private int IDDeudor
        {
            get { return Convert.ToInt32(glkDeudor.EditValue); }
            set { glkDeudor.EditValue = value; }
        }
        #endregion

        #region *** INICIO ***

        public DialogLiquidacion(int UserID)
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
                lkTipoMov.Properties.DataSource = listadoDeposito.GetListTipoDepositos();
                lkArea.Properties.DataSource = db.AreaVentas.Where(a => a.Activo).Select(s => new { s.ID, s.Nombre });
                rgDeudor.SelectedIndex = -1;
                this.bdsMinutas.DataSource = this.DetalleMinutas;                
                _ClienteCordoba = 399;
                _ClienteDolar = 457;
                _CuentaPorCobrarEmpleado = Parametros.Config.CuentaPorCobrarEmpleado();
                _MargenDiferenciaCambiaria = Parametros.Config.MargenDiferenciaCambiaria();

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

                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(49)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(49)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                txtNumero.Text = number.ToString("000000000");
                
                lkMoneda.Properties.DataSource = from m in db.Monedas select new { m.ID, Display = m.Simbolo + " | " + m.Nombre };
                IDMonedaPrincipal = MonedaPrimaria;
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Abreviatura });
                rpLkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre});
                dateFecha.EditValue = Convert.ToDateTime(db.GetDateServer());
                
                if (dateFecha.EditValue != null)
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaDeposito.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaDeposito.Date)).First().Valor : 0m);


        


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
            Parametros.General.ValidateEmptyStringRule(dateFecha, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
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
           

            if (txtNumero.Text == "" || lkMoneda.EditValue == null || dateFecha.EditValue == null || lkTipoMov.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del Deposito.", Parametros.MsgType.warning);
                return false;
            }
            
            if (!Parametros.General.ValidatePeriodoContable(FechaDeposito, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (detalle)
            {                               

                if (DetalleM.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (DetalleMinutas.Count <= 0)
                {
                    if (Parametros.General.DialogMsg("No existe detalle de minutas, desea continuar." + Environment.NewLine, Parametros.MsgType.question) != DialogResult.OK)
                        return false;
                }
                else
                {
                    if (DetalleMinutas.Count(c => c.Monto.Equals(0)) > 0)
                    {
                        Parametros.General.DialogMsg("El monto de una minuta es igual a 0 (cero), debe ingresar un monto mayor." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }                

                if (DetalleM.Sum(s => s.Litros) <= 0)
                {
                    Parametros.General.DialogMsg("El total del monto abonado es 0 (cero), debe abonar un monto mayor." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (DetalleM.Count(d => d.Litros > 0 && d.FechaRegistro.Date > FechaDeposito.Date) > 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
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
                                
            }

            return true;
        }

        private void GetDiferencias()
        {
            try
            {
                decimal vCuentas = DetalleComprobante.AsEnumerable().Where(o => o["Monto"] != DBNull.Value).Sum(x => ((decimal)x["Monto"]));
                decimal vDiferencia = _MontoTotal - (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) : Decimal.Round(Convert.ToDecimal(DetalleM.Sum(s => s.Litros * s.TipoCambio)), 2, MidpointRounding.AwayFromZero)) - vCuentas;

                spDifCambiario.Value = 0m;
                
                if (vDiferencia > 0)
                {
                    if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                    {
                        spDifCambiario.Value = vDiferencia;
                        layoutControlItemDif.Text = "<>";
                        spDiferencia.Value = 0;
                        layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else
                    {
                        layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItemDif.Text = "DIFERENCIA";
                        spDiferencia.Value = vDiferencia;
                        spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Green;
                    }
                }
                else if (vDiferencia < 0)
                {
                    layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDif.Text = "DIFERENCIA";
                    spDiferencia.Value = vDiferencia;
                    spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Red;
                }
                else if (vDiferencia.Equals(0))
                {
                    layoutControlItemDif.Text = "<>";
                    spDiferencia.Value = vDiferencia;
                    layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private bool Guardar()
        {
           

            if (!ValidarCampos(true))
                return false;


            if (IDMonedaPrincipal.Equals(MonedaSecundaria))
            {
                    decimal montoConcepto = ValidacionPagado_GridConcepto();
                decimal montoDetalle = ValidacionPagado_GridDetalle();

                decimal diferencia = _MontoTotal - _Diferencial;
                if (montoConcepto != montoDetalle)
                {
                   Parametros.General.DialogMsg("El total del Monto de las Cuentas Bancarias no puede ser distinto al total del Monto Pagado." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
                if (_Diferencial > _MargenDiferenciaCambiaria)     //(diferencia > _MargenDiferenciaCambiaria)
                {
                    Parametros.General.DialogMsg("El monto del diferencial cambiario sobrepasa el margen de: " + _MargenDiferenciaCambiaria + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
                


            }
            
            if (!spDiferencia.Value.Equals(0))
            {
                Parametros.General.DialogMsg("La diferencia debe ser igual a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
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

                    M.MovimientoTipoID = 49;
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = FechaDeposito;
                    M.Monto = _MontoTotal;
                    M.MonedaID = IDMonedaPrincipal;
                    M.TipoCambio = _TipoCambio;
                    M.MontoMonedaSecundaria = Decimal.Round(_MontoTotal / _TipoCambio, 2, MidpointRounding.AwayFromZero);
                    M.Numero = Convert.ToInt32(Numero);
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.TipoLiquidacionDeposito = lkTipoMov.Text;

                    if (TipoMovimiento.Equals(0))//Movimiento (Liquidación) tipo VentaContado (no es el cliente 399 o 457) este busca autom. el cliente dependiendo de la moneda escogida($|C$)
                    {
                        M.AreaID = AreaVenta;
                    }
                    else if (TipoMovimiento.Equals(1))//Movimiento (Liquidación) tipo NotaDébito
                    {
                        //Aqui se puede apuntar al cliente 399 o 457 ... o no.
                        if (rgDeudor.SelectedIndex.Equals(0))
                            M.ClienteID = IDDeudor;
                        else if (rgDeudor.SelectedIndex.Equals(1))
                            M.EmpleadoID = IDDeudor;
                    }
                    
                    db.Movimientos.InsertOnSubmit(M);
                    db.SubmitChanges();

                    #region <<< REGISTRANDO MINUTAS >>>

                    M.MinutasDeposito.AddRange(DetalleMinutas);
                                        
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
                    
                    decimal Saldo = 0;

                    if (IDMonedaPrincipal.Equals(MonedaPrimaria))
                        Saldo = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                    else
                        Saldo = Decimal.Round(DetalleM.Sum(s => s.Litros * s.TipoCambio), 2, MidpointRounding.AwayFromZero);
                    
                    D.Valor = -Math.Abs(Saldo);
                    D.MovimientoID = M.ID;
                    db.Deudors.InsertOnSubmit(D);
                    db.SubmitChanges();

                    #endregion

                    #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>

                    if (DetalleM.Count > 0)
                    {
                        if (DetalleM.Sum(s => s.Litros) > 0)
                        {
                            DetalleM.Where(d => d.Litros > 0).ToList().ForEach(linea =>
                                {
                                    Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(linea.ID));
                                    MLine.Abono += (IDMonedaPrincipal.Equals(MonedaPrimaria) ? linea.Litros : Decimal.Round(linea.Litros * linea.TipoCambio, 2, MidpointRounding.AwayFromZero));
                                    MLine.Pagado = linea.Pagado;
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = linea.ID, Monto = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? linea.Litros : Decimal.Round(linea.Litros * linea.TipoCambio, 2, MidpointRounding.AwayFromZero)) });
                                    linea.Litros = 0;
                                    db.SubmitChanges();
                                });
                        }
                    }

                    #endregion

                    //para que actualice los datos del registro
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                    "Se registró la Liquidación del Deposito: " + M.Numero, this.Name);
                    db.SubmitChanges();
                    trans.Commit();

                    IDPrint = 0;
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
                   
                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    int i = 1;
                    var Bancos = db.CuentaBancarias.Where(b => b.Activo && (b.EstacionServicioID.Equals(IDEstacionServicio) || b.MonedaID.Equals(2))).ToList();

                    DetalleMinutas.ForEach(det =>
                    {

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = Bancos.Single(b => b.ID.Equals(det.CuentaBancariaID)).CuentaContableID,
                            Monto = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Convert.ToDecimal(det.Monto) :
                        Decimal.Round(Convert.ToDecimal(det.Monto * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(det.Monto / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                            det.Monto),
                            Fecha = FechaDeposito,
                            Descripcion = "Minuta Deposito " + det.Numero.ToString(),
                            Linea = i,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                    });

                    int cuenta = 0;

                    if (TipoMovimiento.Equals(2))
                    {
                        if (IDMonedaPrincipal.Equals(MonedaPrimaria))
                        cuenta = 4;
                        else
                        cuenta = 5;
                    }
                    else if (TipoMovimiento.Equals(0))
                        cuenta = db.Clientes.Single(c => c.ID.Equals(DetalleM.First().ClienteID)).CuentaContableID;
                    else if ((TipoMovimiento.Equals(1)))
                    {
                        if (rgDeudor.SelectedIndex.Equals(0))
                            cuenta = db.Clientes.Single(s => s.ID.Equals(Convert.ToInt32(glkDeudor.EditValue))).CuentaContableID;//cuenta = db.Clientes.Single(c => c.ID.Equals(DetalleM.First().ClienteID)).CuentaContableID;
                        else if (rgDeudor.SelectedIndex.Equals(1))
                            cuenta = _CuentaPorCobrarEmpleado;
                    }

                            if (Convert.ToDecimal(DetalleM.Sum(s => s.Litros)) > 0)
                            {
                                //MONTO ACREDITADO PARA EL CLIENTE
                                //MONTO ORIGEN
                                var vOrigen = DetalleM.Where(s => !s.CuentaID.Equals(0) && s.Litros > 0);
                                if (vOrigen.Count() > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = vOrigen.First().CuentaID,
                                        Monto = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(Convert.ToDecimal(vOrigen.Sum(s => s.Litros)), 2, MidpointRounding.AwayFromZero))
                                        : -Math.Abs(Decimal.Round(Convert.ToDecimal(vOrigen.Sum(s => s.Litros * s.TipoCambio)), 2, MidpointRounding.AwayFromZero))),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vOrigen.Sum(s => s.Litros)) / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaDeposito,
                                        Descripcion = "Minuta Deposito " + Numero.ToString(),
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }

                                //MONTO SIN ORIGEN
                                var vSinOrigen = DetalleM.Where(s => s.CuentaID.Equals(0) && s.Litros > 0);
                                if (vSinOrigen.Count() > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = cuenta,
                                        Monto =(IDMonedaPrincipal.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(Convert.ToDecimal(vSinOrigen.Sum(s => s.Litros)), 2, MidpointRounding.AwayFromZero)) :
                         -Math.Abs(Decimal.Round(Convert.ToDecimal(vSinOrigen.Sum(s => s.Litros * s.TipoCambio)), 2, MidpointRounding.AwayFromZero))),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vSinOrigen.Sum(s => s.Litros)) / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaDeposito,
                                        Descripcion = "Minuta Deposito " + Numero.ToString(),
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });

                                }
                            }

                    //CD.Add(new Entidad.ComprobanteContable
                    //{
                    //    CuentaContableID = cuenta,
                    //    Monto = -Math.Abs(Convert.ToDecimal(DetalleM.Sum(s => s.Litros))),
                    //    TipoCambio = _TipoCambio,
                    //    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleM.Sum(s => s.Litros) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                    //    Fecha = FechaDeposito,
                    //    Descripcion = "Minuta Deposito " + Numero.ToString(),
                    //    Linea = i,
                    //    CentroCostoID = 0,
                    //    EstacionServicioID = IDEstacionServicio,
                    //    SubEstacionID = IDSubEstacion
                    //});
                    //i++;

                    if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                    {
                        if (spDifCambiario.Value > 0)
                        {
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 479,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(spDifCambiario.Value), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(spDifCambiario.Value / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = FechaDeposito,
                                Descripcion = "Ganancia Diferencial Tipo de Cambio, Minuta Deposito " + Numero.ToString(),
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;
                        }
                    }

                    foreach (DataRow linea in DetalleComprobante.Rows)
                    {

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                            Monto = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) * -1), 2,MidpointRounding.AwayFromZero),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio) * -1, 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaDeposito,
                            Descripcion = " Minuta Deposito " + Numero.ToString(),
                            Linea = i,
                            CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
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

        private decimal ValidacionPagado_GridConcepto()
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridConcepto.DefaultView;
            int RowHandle = view.FocusedRowHandle;

            decimal monto = 0;

            monto = Convert.ToDecimal(view.Columns["Monto"].SummaryItem.SummaryValue.ToString());
            return monto;
        }
        private decimal ValidacionPagado_GridDetalle()
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
            int RowHandle = view.FocusedRowHandle;

            decimal monto = 0;


            monto = Convert.ToDecimal(view.Columns["Litros"].SummaryItem.SummaryValue.ToString());

            return monto;
        }
        private decimal GetMontoSecundario()
        {
            try
            {
                decimal monto = 0m;
                DetalleM.Where(m => m.Litros > 0).ToList().ForEach(det =>
                    {
                       monto += Decimal.Round(Decimal.Round(det.Litros, 2, MidpointRounding.AwayFromZero) / Decimal.Round(det.TipoCambio, 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero); 
                    });

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
            MDI.CleanDialog(ShowMsg, Next, RefreshMDI, IDPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (DetalleM.Count > 0)
                {
                    if (Parametros.General.DialogMsg("El Deposito actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        Next = false;
                        e.Cancel = true;
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

        //Selección del Deudor
        

        //Cambiar las opciones del ROC
        

        private void dateFechaRecibo_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaDeposito.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaDeposito.Date)).First().Valor : 0m);
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
                    layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //txtGrandTotal.BackColor = Color.FromArgb(232, 249, 239);
                    //txtGrandTotal.ForeColor = Color.Black;
                }
                else if (Convert.ToInt32(lkMoneda.EditValue).Equals(MonedaSecundaria))
                {
                    lkMoneda.BackColor = Color.ForestGreen;
                    lkMoneda.ForeColor = Color.White;
                    layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //txtGrandTotal.BackColor = Color.ForestGreen;
                    //txtGrandTotal.ForeColor = Color.White;
                }


            }
        }

        #region <<< GRID MINUTAS >>>

        private void gvDataConcepto_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "CuentaBancariaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "CuentaBancariaID")) == 0)
                {
                    view.SetColumnError(view.Columns["CuentaBancariaID"], "Debe Seleccionar una Cuenta Bancaria");
                    e.ErrorText = "Debe Seleccionar una Cuenta Bancaria";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaBancariaID"], "Debe Seleccionar una Cuenta Bancaria");
                e.ErrorText = "Debe Seleccionar una Cuenta Bancaria";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Número Minuta             
            if (view.GetRowCellValue(RowHandle, "Numero") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "Numero")) == 0)
                {
                    view.SetColumnError(view.Columns["Numero"], "Debe Digitar el Número de Minuta");
                    e.ErrorText = "Debe Digitar el Número de Minuta";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Numero"], "Debe Digitar el Número de Minuta");
                e.ErrorText = "Debe Digitar el Número de Minuta";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Número Minuta             
            if (view.GetRowCellValue(RowHandle, "Monto") != null)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Monto")) <= 0.00m)
                {
                    view.SetColumnError(view.Columns["Monto"], "Debe Digitar el Monto de la Minuta");
                    e.ErrorText = "Debe Digitar el Monto de la Minuta";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Monto"], "Debe Digitar el Monto de la Minuta");
                e.ErrorText = "Debe Digitar el Monto de la Minuta";
                e.Valid = false;
                Validate = false;
            }
            
            _MontoTotal = 0m;
            DetalleMinutas.ForEach( det => _MontoTotal += (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(det.Monto, 2, MidpointRounding.AwayFromZero) : Decimal.Round(det.Monto * _TipoCambio, 2, MidpointRounding.AwayFromZero)));

            //_MontoTotal = (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero) : Decimal.Round(DetalleMinutas.Sum(s => s.Monto) * _TipoCambio, 2, MidpointRounding.AwayFromZero));
        }

        private void gvDataConcepto_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        private void gvDataConcepto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridConcepto.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "Numero").ToString() + " Monto: " + view.GetRowCellDisplayText(RowHandle, "Monto").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        gvDataConcepto.RefreshData();
                        _MontoTotal = 0m;
                        DetalleMinutas.ForEach(det => _MontoTotal += (IDMonedaPrincipal.Equals(MonedaPrimaria) ? Decimal.Round(det.Monto, 2, MidpointRounding.AwayFromZero) : Decimal.Round(det.Monto * _TipoCambio, 2, MidpointRounding.AwayFromZero)));

                    }

                }
            }
        }

        #endregion

        #region <<< GRID DOCUMENTOS >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

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
                //if (e.Column == ColValor)
                //{
                //    decimal monto, abono;
                //    monto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Monto"));
                //    abono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Abono"));
                //    gvDetalle.SetRowCellValue(e.RowHandle, "Saldo", (monto - abono));
                //    decimal _tc = Math.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "TipoCambio")), 2);
                //    gvDetalle.SetRowCellValue(e.RowHandle, "SaldoUS", (monto - abono)/_tc);
                //}

                if (e.Column == colMonto)
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                    {
                        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                        {
                            decimal vSaldo = 0, vAbono = 0;

                            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                            if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "SaldoUS") != null)
                                    vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "SaldoUS"));

                                if (vAbono > vSaldo)
                                {
                                    Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", 0.00);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                }
                                else
                                {
                                    if (vAbono.Equals(vSaldo))
                                    {
                                        gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", 0.00);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldoDolar", 0.00);
                                    }
                                    else
                                    {
                                        gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                                        decimal _tc = Math.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "TipoCambio")),2, MidpointRounding.AwayFromZero);
                                        decimal _saldo = Math.Round((vSaldo - vAbono) * _tc, 2, MidpointRounding.AwayFromZero);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", _saldo);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldoDolar", (vSaldo-vAbono));
                                    }
                                }
                            }
                            else
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                    vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                                if (vAbono > vSaldo)
                                {
                                    Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", 0.00);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                }
                                else
                                {
                                    if (vAbono.Equals(vSaldo))
                                    {
                                        gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", 0.00);
                                    }
                                    else
                                    {
                                        gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);                                        
                                        decimal _saldo = (vSaldo - vAbono);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "NuevoSaldo", _saldo);
                                    }
                                }
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
                    GetDiferencias();
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
            GetDiferencias();
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

            GetDiferencias();
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

            GetDiferencias();

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
                        GetDiferencias();
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                view.AddNewRow();
                bgvCuentas.RefreshData();
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
                if (ValidarCampos(true) && !btnLoad.Enabled)
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Liquidación de Deposito";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Seleccionar el Tipo de Movimiento para la liquidación
        private void lkTipoMov_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (TipoMovimiento != null)
                {
                    if (TipoMovimiento.Equals(0))
                    {
                        layoutControlItemRadioG.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        rgDeudor.SelectedIndex = -1;
                        layoutControlItemArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        
                        
                    }
                    else if (TipoMovimiento.Equals(1))
                    {
                        layoutControlItemArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lkArea.EditValue = null;

                        layoutControlItemRadioG.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    }
                    else if (TipoMovimiento.Equals(2))
                    {
                        layoutControlItemArea.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lkArea.EditValue = null;
                        layoutControlItemRadioG.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        rgDeudor.SelectedIndex = -1;
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
             
        private void rgDeudor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgDeudor.SelectedIndex.Equals(0))
                {
                    layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDeudor.Text = "Cliente";
                    glkDeudor.Properties.NullText = "<Seleccione el Cliente>";

                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.DataSource = null;
                    glkDeudor.Properties.DataSource = (from c in db.Clientes
                                                       where c.Activo && c.AplicaLiquidacion
                                                       group c by new { c.ID, c.Codigo, c.Nombre } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombre,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                       }).ToList();
                }
                else if (rgDeudor.SelectedIndex.Equals(1))
                {
                    layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDeudor.Text = "Empleado";
                    glkDeudor.Properties.NullText = "<Seleccione el Empleado>";

                    glkDeudor.EditValue = null;
                    glkDeudor.Properties.DataSource = null;
                    glkDeudor.Properties.DataSource = (from em in db.Empleados
                                                       join m in db.Movimientos on em.ID equals m.EmpleadoID
                                                       where em.Activo && m.MovimientoTipoID.Equals(27)
                                                        && !m.Anulado && !m.Pagado && (m.EstacionServicioID.Equals(IDEstacionServicio) || em.EsMultiEstacion)
                                                       group em by new { em.ID, em.Codigo, em.Nombres, em.Apellidos } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombres + " " + gr.Key.Apellidos,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombres + " " + gr.Key.Apellidos
                                                       }).ToList();
                }
                else
                {
                    layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    glkDeudor.EditValue = null;
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
                if (ValidarCampos(false))
                {
                    if (TipoMovimiento.Equals(0))
                    {
                      

                        if (lkArea.EditValue != null)
                        {

                            this.bdsDetalle.DataSource = null;

                            if (AreaVenta.Equals(3))
                            {
                                DetalleM = (from m in db.Movimientos
                                            join c in db.Clientes on m.ClienteID equals c.ID
                                            where m.MovimientoTipoID.Equals(7) && m.AreaID.Equals(AreaVenta) && c.AplicaLiquidacion && !m.Credito
                                            && m.ClienteID.Equals((IDMonedaPrincipal.Equals(MonedaPrimaria) ? _ClienteCordoba : _ClienteDolar)) && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }
                            else
                            {
                                DetalleM = (from m in db.Movimientos
                                            where m.MovimientoTipoID.Equals(7) && m.AreaID.Equals(AreaVenta) && !m.Credito
                                            && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }

                            this.bdsDetalle.DataSource = DetalleM;

                            if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                            {
                                this.colTC.Visible = true;
                                this.colTC.VisibleIndex = 5;
                                this.colMontoUS.Visible = true;
                                this.colMontoUS.VisibleIndex = 6;
                                this.colSaldoDolar.Visible = true;
                                this.colSaldoDolar.VisibleIndex = 7;
                                
                                this.colNuevoSaldo.Visible = false;
                                this.colNuevoSaldo.VisibleIndex = -1;
                                this.colNuevoSaldoAux.Visible = true;
                                this.colNuevoSaldoAux.VisibleIndex = 10;

                                this.colNuevoSaldoUS.Visible = true;
                                this.colNuevoSaldoUS.VisibleIndex = 11;
                                this.layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.MonedaID.Equals(2)).Select(s => new { s.ID, s.Nombre }).ToList();
                                //this.colNuevoSaldo.UnboundExpression = "[Saldo Doc C$]-([Monto Pag]*[T/C])";
                            }
                            else
                            {
                                lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                                colNuevoSaldoAux.Visible = false;
                                colNuevoSaldoAux.VisibleIndex = -5;
                            }
                            gridConcepto.Enabled = true;
                            gvDetalle.RefreshData();
                            lkTipoMov.Enabled = false;
                            lkArea.Enabled = false;
                            dateFecha.Enabled = false;
                            lkMoneda.Enabled = false;
                            btnLoad.Enabled = false;

                        }
                        else
                            Parametros.General.DialogMsg("Favor seleccionar un Área", Parametros.MsgType.warning);
                    }
                    else if (TipoMovimiento.Equals(1))
                    {
                        if (IDDeudor > 0)
                        {

                            this.bdsDetalle.DataSource = null;

                            if (rgDeudor.SelectedIndex.Equals(0))
                            {
                                DetalleM = (from m in db.Movimientos
                                            where m.MovimientoTipoID.Equals(27) && ((rgDeudor.SelectedIndex.Equals(0) && m.ClienteID.Equals(IDDeudor)) || (rgDeudor.SelectedIndex.Equals(1) && m.EmpleadoID.Equals(IDDeudor)))
                                            && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }
                            else if (rgDeudor.SelectedIndex.Equals(1))
                            {
                                DetalleM = (from m in db.Movimientos
                                            join c in db.ConceptoContables on m.ConceptoContableID equals c.ID into DefaultConcepto
                                            from cto in DefaultConcepto.DefaultIfEmpty()
                                            where (m.MovimientoTipoID.Equals(27) || (m.MovimientoTipoID.Equals(39) && cto.GeneraDocumento)) && ((rgDeudor.SelectedIndex.Equals(0) && m.ClienteID.Equals(IDDeudor)) || (rgDeudor.SelectedIndex.Equals(1) && m.EmpleadoID.Equals(IDDeudor)))
                                            && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }

                            if (rgDeudor.SelectedIndex.Equals(0))
                            {
                                DetalleM.AddRange((from m in db.Movimientos
                                                   join mp in db.MovimientoPagos on m.ID equals mp.MovimientoID
                                                   join p in db.TipoPagos on mp.TipoPagoID equals p.ID
                                                   where m.MovimientoTipoID.Equals(22) && m.MonedaID.Equals(IDMonedaPrincipal)
                                                   && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio) && p.DeudorID.Equals(IDDeudor)
                                                   select m).OrderBy(o => o.FechaRegistro).ToList());
                            }

                            this.bdsDetalle.DataSource = DetalleM;

                            if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                            {
                                this.colTC.Visible = true;
                                this.colTC.VisibleIndex = 5;
                                this.colMontoUS.Visible = true;
                                this.colMontoUS.VisibleIndex = 6;
                                this.colSaldoDolar.Visible = true;
                                this.colSaldoDolar.VisibleIndex = 7;
                                this.colNuevoSaldo.Visible = false;
                                this.colNuevoSaldo.VisibleIndex = -1;
                                this.colNuevoSaldoAux.Visible = true;
                                this.colNuevoSaldoAux.VisibleIndex = 10;

                                this.colNuevoSaldoUS.Visible = true;
                                this.colNuevoSaldoUS.VisibleIndex = 11;

                                this.layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.MonedaID.Equals(2)).Select(s => new { s.ID, s.Nombre }).ToList();
                                //this.colSaldo.UnboundExpression = "Round([Saldo Doc C$]- ([Monto Pag]*[T/C]),2)";
                            }
                            else
                                lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                                
                            gridConcepto.Enabled = true;
                            gvDetalle.RefreshData();
                            lkTipoMov.Enabled = false;
                            glkDeudor.Enabled = false;
                            rgDeudor.Enabled = false;
                            lkArea.Enabled = false;
                            dateFecha.Enabled = false;
                            lkMoneda.Enabled = false;
                            btnLoad.Enabled = false;

                        }                        
                        else
                            Parametros.General.DialogMsg("Favor seleccionar un Deudor", Parametros.MsgType.warning);
                    }
                    else if (TipoMovimiento.Equals(2))
                    {
                        this.bdsDetalle.DataSource = null;

                        DetalleM = (from m in db.Movimientos
                                    join mp in db.MovimientoPagos on m.ID equals mp.MovimientoID
                                    join p in db.TipoPagos on mp.TipoPagoID equals p.ID
                                    where m.MovimientoTipoID.Equals(22) && m.MonedaID.Equals(IDMonedaPrincipal)
                                    && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio) && p.DeudorID.Equals(0)
                                    select m).OrderBy(o => o.FechaRegistro).ToList();

                            this.bdsDetalle.DataSource = DetalleM;

                            if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                            {
                                this.colTC.Visible = true;
                                this.colTC.VisibleIndex = 5;
                                this.colMontoUS.Visible = true;
                                this.colMontoUS.VisibleIndex = 6;
                                this.colSaldoDolar.Visible = true;
                                this.colSaldoDolar.VisibleIndex = 7;
                                this.colNuevoSaldo.Visible = false;
                                this.colNuevoSaldo.VisibleIndex = -1;
                                this.colNuevoSaldoAux.Visible = true;
                                this.colNuevoSaldoAux.VisibleIndex = 9;
                                this.colNuevoSaldoUS.Visible = true;
                                this.colNuevoSaldoUS.VisibleIndex = 10;
                                this.layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.MonedaID.Equals(2)).Select(s => new { s.ID, s.Nombre }).ToList();
                                //this.colSaldo.UnboundExpression = "Round([Saldo Doc C$]- ([Monto Pag]*[T/C]),2)";
                            }
                            else
                               lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                               
                        
                            
                            gridConcepto.Enabled = true;
                            gvDetalle.RefreshData();
                            lkTipoMov.Enabled = false;
                            dateFecha.Enabled = false;
                            lkMoneda.Enabled = false;
                            btnLoad.Enabled = false;
                    }
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    decimal monto, abono, saldo, saldous;
                    monto = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Monto"));
                    abono = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Abono"));
                    saldo = Math.Round((monto - abono), 2, MidpointRounding.AwayFromZero);
                    view.SetRowCellValue(RowHandle, "NuevoSaldo", saldo);
                    decimal _tc = Math.Round(Convert.ToDecimal(view.GetRowCellValue(RowHandle, "TipoCambio")), 2, MidpointRounding.AwayFromZero);
                    saldous = (_tc.Equals(0) ? 0m : Math.Round((saldo / _tc), 2, MidpointRounding.AwayFromZero));
                    view.SetRowCellValue(RowHandle, "NuevoSaldoDolar", saldous);


                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void spTotal_EditValueChanged(object sender, EventArgs e)
        {
            GetDiferencias();
        }
        private void rpChkSelected_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
            int RowHandle = view.FocusedRowHandle;


            bool checkEstado = Convert.ToBoolean(view.GetRowCellValue(RowHandle,"Pagado"));
            //-- Validar Columna de Pago
            
            if ( checkEstado ==false)
            {
                decimal saldo = 0;
                if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                {
                    saldo = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "SaldoUS"));
                }
                else
                {
                    saldo = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Saldo"));
                }

                view.SetRowCellValue(RowHandle, "Litros", saldo);
            }
            else view.SetRowCellValue(RowHandle, "Litros", 0.00);   

        }
       #endregion

        








    }
}