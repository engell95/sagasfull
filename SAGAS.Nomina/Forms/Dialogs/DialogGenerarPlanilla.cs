using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogGenerarPlanilla : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal FormPlanillasGeneradas MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;
        internal bool _Editable;
        private bool RefreshMDI = false;
        private int IDEstacionServicio;
        private int IDSubEstacion;
        private int IDCuentaPlanilla;
        private bool _Guardado = false;
        private int IDPrint = 0;
        private List<Parametros.ListComprobanteNomina> EtComprobante = new List<Parametros.ListComprobanteNomina>();
        private List<Parametros.ListToGeneratePayroll> EtEmpleado;
        private List<Entidad.IRRetenido> EtIRRetenidos;
        private List<Entidad.TablaIR> EtTablaIR;
        private List<Entidad.MovimientoEmpleado> EtIngresos;
        private List<Entidad.MovimientoEmpleado> EtEgresos;
        private List<Entidad.MovimientoEmpleado> EtHorasExtras;
        private List<Entidad.TipoMovimientoEmpleado> EtTiposMovimientos;
        internal decimal vTotalPagar = 0m;
        internal int IDMonedaPrincipal;
        internal decimal _TipoCambio;
        internal decimal _ValorINSSLaboral = 0;
        internal decimal _ValorINSSPatronal = 0;
        internal decimal _ValorINATEC = 0;
        internal decimal _MaximoSalarioINSS = 0;
        internal int _MovimientoHoraExtra;
        internal int _MovimientoPension;
        internal int _MovimientoSubsidio;
        internal int _MonedaPrimaria;
        internal int _MovimientoAusenciaID;
        
        private DateTime FechaDesde
        {
            get { return Convert.ToDateTime(dateFechaDesde.EditValue); }
            set { dateFechaDesde.EditValue = value; }
        }

        private DateTime FechaHasta
        {
            get { return Convert.ToDateTime(dateFechaHasta.EditValue); }
            set { dateFechaHasta.EditValue = value; }
        }

        private int IDPlanilla
        {
            get { return Convert.ToInt32(lkPlanilla.EditValue); }
            set { lkPlanilla.EditValue = value; }
        }

        private int Numero
        {
            get { return Convert.ToInt32(txtNumero.Text); }
            set { txtNumero.Text = value.ToString(); }
        }

        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private List<Entidad.VistaComprobante> CD;

        
        #endregion

        #region *** INICIO ***

        public DialogGenerarPlanilla(int UserID)
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
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                //Obtener los valores configurables de la nomina
                _ValorINSSLaboral = Parametros.Config.ValorINSSLaboral();
                _ValorINSSPatronal = Parametros.Config.ValorINSSPatronal();
                _ValorINATEC = Parametros.Config.ValorINATEC();
                _MaximoSalarioINSS = Parametros.Config.MaximoSalarioINSS();
                _MovimientoHoraExtra = Parametros.Config.MovimientoHoraExtraID();
                _MovimientoSubsidio = Parametros.Config.MovimientoSubsidioID();
                _MovimientoPension = Parametros.Config.MovimientoPensionID();
                _MovimientoAusenciaID = Parametros.Config.MovimientoAusenciaID();
                //Cargar Tipos Movimientos Empleados
                EtTiposMovimientos = db.TipoMovimientoEmpleado.Where(o => o.Activo).ToList();
                //---

                //Cargar Limites IR
                EtTablaIR = db.TablaIR.ToList();
                //---

                //Cargar Planillas
                lkPlanilla.Properties.DataSource = db.Planillas.Where(o => o.Activo).Select(s => new { s.ID, s.Nombre }).ToList();
                //---

                //Valores configurables
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();

                //Obtener Fechas
                DateTime FechaV;
                FechaV = Convert.ToDateTime(db.GetDateServer());
                dateFechaDesde.EditValue = Convert.ToDateTime(db.GetDateServer());
                if (FechaV.Day <= 15)
                {
                    dateFechaDesde.EditValue = new DateTime(FechaV.Year, FechaV.Month, 1);
                }
                else
                {
                    dateFechaDesde.EditValue = new DateTime(FechaV.Year, FechaV.Month, 16);
                }
                
                //IDPlanilla = 21;
                //botMostrarEmpleado_Click(null, null);

                
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
        }

        private bool ValidarReferencia(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? i.MovimientoTipoID.Equals(51) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.ID != Convert.ToInt32(ID) :
                          i.MovimientoTipoID.Equals(51) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio))
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }
        
        public bool ValidarCampos(bool detalle)
        {
            if (lkPlanilla.EditValue == null || dateFechaDesde.EditValue == null || dateFechaHasta.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la planilla.", Parametros.MsgType.warning);
                return false;
            }

            if (EtEmpleado.All(c => !c.Selected))
            {
                Parametros.General.DialogMsg("No existe ningun empleado seleccionado", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaHasta, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(dateFechaHasta.DateTime, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            //if (Parametros.General.ListSES.Count > 0)
            //{
            //    if (IDSubEstacion <= 0)
            //    {
            //        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
            //        return false;
            //    }
            //}


            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(true)) return false;

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 900;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.PlanillaGenerada PG;

                    if (_Editable)
                    {
                        PG = db.PlanillaGenerada.Single(p => p.ID.Equals(0));
                        //M.Modificado = true;
                    }
                    else
                    {
                        PG = new Entidad.PlanillaGenerada();

                        PG.PlanillaID = IDPlanilla;
                        PG.FechaDesde = FechaDesde;
                        PG.FechaHasta = FechaHasta;

                    }

                    db.PlanillaGenerada.InsertOnSubmit(PG);
                    db.SubmitChanges();

                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaHasta.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaHasta.Date)).First().Valor : 0m);

                    var EtPension = db.TipoMovimientoEmpleado.Single(s => s.ID.Equals(_MovimientoPension));
                    //var TiposMovimientos = db.TipoMovimientoEmpleado.Where(o => o.Activo);
                    string texto = "Registro de Nomina " + lkPlanilla.Text + " Correspondiente " + (PG.FechaDesde.Day > 15 ? "II" : "I") + " Quincena " + Parametros.General.GetMonthInLetters(PG.FechaDesde.Month) + " del " + PG.FechaDesde.Year.ToString();                        
                    var obj = EtEmpleado.Where(o => o.Selected);
                    progressBarCont.Properties.Maximum = obj.Count();

                    #region <<< Registros DetallePlanillaGenerada >>>
                    foreach (var reg in obj)
                    {
                        //Campos de verificación
                        bool quincenaCompleta = true;
                        decimal _Provision = 0m;
                        decimal _IngresosGrabables = 0m;
                        decimal _IngresosNoGrabables = 0m;
                        decimal _Egresos = 0m;
                        decimal _Pension = 0m;
                        decimal _ValorSubsidio = 0m;

                        

                        //Verificar fecha de ingreso para detectar si el empleado tiene la quincena completa
                        DateTime fechaDesde = PG.FechaDesde;
                        if (reg.FechaIngreso.Date > fechaDesde.Date)
                        {
                            fechaDesde = reg.FechaIngreso.AddDays(-1);
                            quincenaCompleta = false;
                        }

                        //Si el empleado Ingresó despues de la Fecha Final de la planilla, mandar mensaje de correccion de fecha.
                        if (reg.FechaIngreso.Date > PG.FechaHasta.Date)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Empleado '" + reg.NombreCompleto + "' Tiene una fecha de ingreso invalida (" + reg.FechaIngreso.ToShortDateString() + ")"
                                + "\nPor favor corrija esto en Empleado -> Editar. Para generar correctamente la planilla", Parametros.MsgType.warning);
                            trans.Rollback();
                            progressBarCont.EditValue = 0;
                            return false;
                        }

                        Entidad.DetallePlanillaGenerada DPG = new Entidad.DetallePlanillaGenerada();

                        //Campos Tabla DetallePlanillaGenerada
                        DPG.EmpleadoID = reg.EmpleadoID;
                        DPG.NombreCompleto = reg.NombreCompleto;
                        DPG.AreaID = reg.AreaID;
                        DPG.CargoID = reg.CargoID;
                        DPG.CodigoEmpleado = reg.Codigo;
                        DPG.NumeroINSS = reg.INSS;
                        DPG.EstructuraOrganizativaID = reg.EstructuraOrganizativaID;
                        DPG.SalarioBasico = reg.SalarioBasico;
                        
                        //Centro Costo ID
                        int ceco = reg.CentroCostoID;
                        string Orden = reg.OrdenCD;
                        bool DifTarjeta = reg.DifTaejeta;
                        //Calculo de provisiones (Indemnizaciones, Aguinaldo, Vacaciones)
                        _Provision = Decimal.Round(Decimal.Round(DPG.SalarioBasico / 2, 2, MidpointRounding.AwayFromZero) / 12, 2, MidpointRounding.AwayFromZero);

                        if (quincenaCompleta)
                        {
                            DPG.DiasLaborados = 15;
                            DPG.SalarioPlanilla = Decimal.Round(DPG.SalarioBasico / 2, 2, MidpointRounding.AwayFromZero);//Quincena Completa
                        }
                        else
                        {
                            DPG.DiasLaborados = Convert.ToInt32((FechaHasta.Date - reg.FechaIngreso).TotalDays) + 1;
                            DPG.DiasLaborados += Convert.ToInt32(15 - ((PG.FechaHasta - PG.FechaDesde).TotalDays + 1));
                            DPG.SalarioPlanilla = Decimal.Round((DPG.SalarioBasico * DPG.DiasLaborados) / 30, 2, MidpointRounding.AwayFromZero);
                        }

                        if (DPG.DiasLaborados > 15) DPG.DiasLaborados = 15;//Validar que los dias laborados no sean mayores a 15

                        #region <<< Registros TipoMovimientoEmpleado >>>

                        #region <<< TipoMovimientoEmpleado en Planillas por Registros >>>

                        var TiposMovimientosRegistro = EtTiposMovimientos.Where(o => !o.MostrarEnPlanilla).OrderByDescending(o => o.EsIngreso);

                        foreach (var item in TiposMovimientosRegistro)
                        {
                            if (item.ID.Equals(_MovimientoHoraExtra))
                            {
                                foreach (var ing in EtHorasExtras.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID)))
                                {
                                    if (ing.MontoTotal > 0)
                                    {
                                        _IngresosGrabables += Decimal.Round(ing.MontoTotal, 2, MidpointRounding.AwayFromZero);
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(ing.MontoTotal, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = ing.TipoMovimientoEmpleadoID, MovimientoEmpleadoID = ing.ID, Cantidad = ing.CantidadHE, Cuotas = ing.NumeroCuota + 1, Monto = Decimal.Round(ing.MontoTotal, 2, MidpointRounding.AwayFromZero) });
                                    }
                                }
                            }
                            else if (item.ID.Equals(_MovimientoSubsidio))
                            {
                                foreach (var ing in EtIngresos.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.TipoMovimientoEmpleadoID.Equals(item.ID)))
                                {
                                    _ValorSubsidio += Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero);
                                    _IngresosNoGrabables += Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero);
                                    EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                    DPG.DiasLaborados -= Decimal.Round(ing.CantidadDias, 2, MidpointRounding.AwayFromZero);
                                    if (DPG.DiasLaborados < 0) DPG.DiasLaborados = 0;

                                    if (ing.CantidadDias.Equals(15))
                                        DPG.SalarioPlanilla = 0m;
                                    else
                                    {
                                        DPG.SalarioPlanilla -= Decimal.Round(ing.MontoTotal, 2, MidpointRounding.AwayFromZero);
                                        if (DPG.SalarioPlanilla < 0) DPG.SalarioPlanilla = 0m;
                                    }

                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = ing.TipoMovimientoEmpleadoID, MovimientoEmpleadoID = ing.ID, Cantidad = ing.CantidadDias, Cuotas = ing.NumeroCuota + 1, Monto = Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero) });
                                }
                            }
                            else if (item.ID.Equals(_MovimientoAusenciaID))
                            {
                                foreach (var ing in EtEgresos.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.TipoMovimientoEmpleadoID.Equals(item.ID)))
                                {
                                    DPG.DiasLaborados -= Decimal.Round(ing.CantidadDias, 2, MidpointRounding.AwayFromZero);
                                    DPG.SalarioPlanilla -= Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero);
                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = ing.TipoMovimientoEmpleadoID, MovimientoEmpleadoID = ing.ID, Cantidad = ing.CantidadDias, Cuotas = ing.NumeroCuota + 1, Monto = Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero) });
                                }
                            }
                            else
                            {
                                if (item.EsIngreso)
                                {
                                    foreach (var ing in EtIngresos.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.TipoMovimientoEmpleadoID.Equals(item.ID)))
                                    {
                                        if (EtTiposMovimientos.Single(s => s.ID.Equals(ing.TipoMovimientoEmpleadoID)).AplicaRetencion)
                                            _IngresosGrabables += Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero);
                                        else
                                            _IngresosNoGrabables += Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero);

                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = ing.TipoMovimientoEmpleadoID, MovimientoEmpleadoID = ing.ID, Cantidad = (ing.TipoMovimientoEmpleadoID.Equals(_MovimientoHoraExtra) ? ing.CantidadHE : ing.CantidadDias), Cuotas = ing.NumeroCuota + 1, Monto = Decimal.Round(ing.MontoCuota, 2, MidpointRounding.AwayFromZero) });

                                    }
                                }
                                else if (!item.EsIngreso)
                                {
                                    foreach (var ing in EtEgresos.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.TipoMovimientoEmpleadoID.Equals(item.ID)))
                                    {
                                        decimal mCuota = ing.MontoTotal - ing.Abono;
                                        
                                        _Egresos += Decimal.Round((mCuota > ing.MontoCuota ? ing.MontoCuota : mCuota), 2, MidpointRounding.AwayFromZero);
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round((mCuota > ing.MontoCuota ? ing.MontoCuota : mCuota), 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = ing.TipoMovimientoEmpleadoID, MovimientoEmpleadoID = ing.ID, Cantidad = (ing.TipoMovimientoEmpleadoID.Equals(_MovimientoHoraExtra) ? ing.CantidadHE : ing.CantidadDias), Cuotas = ing.NumeroCuota + 1, Monto = Decimal.Round((mCuota > ing.MontoCuota ? ing.MontoCuota : mCuota), 2, MidpointRounding.AwayFromZero) });

                                    }
                                }
                            }                            
                        }

                        #endregion

                        #region <<< TipoMovimientoEmpleado en Planillas por Defecto >>>

                        var TiposMovimientosDefecto = EtTiposMovimientos.Where(o => o.MostrarEnPlanilla).OrderByDescending(o => o.EsIngreso);

                        foreach (var item in TiposMovimientosDefecto)
                        {
                            switch (item.ID)
                            {
                                case (int)Parametros.TiposMovimientoEmpleadoID.Salario:
                                    {
                                        #region <<< SALARIO >>>
                                        //Obtener el Salario para planilla
                                        _IngresosGrabables += Decimal.Round(DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero);
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.INSSLaboral:
                                    {
                                        #region <<< INSS LABORAL >>>
                                        /*================================================================
                                        INSS LABORAL
                                        ================================================================*/
                                        //Valor para calcular el tope del salario maximo cotizable dividido en INSS Laboral
                                        decimal vtopeQuincenal = 0m, vtopeMensual;
                                        vtopeQuincenal = Decimal.Round((_MaximoSalarioINSS * _ValorINSSLaboral) / 2m, 2, MidpointRounding.AwayFromZero);
                                        vtopeMensual = Decimal.Round(_MaximoSalarioINSS * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);
                                        //Variable del total del INSS en la quincena pasada (si es en calculo de la II Quincena es diferente a 0)
                                        decimal inssTotalMes = 0;

                                        //Si la planilla es para la segunda quincena se toma el valor del INSSTotalMes de la tabla
                                        if ((int)Convert.ToDateTime(PG.FechaHasta).Day > 15)
                                            inssTotalMes = InssPagado(DPG.EmpleadoID, PG.FechaDesde, PG.PlanillaID, db);

                                        //se calcula el valor del INSS de la quincenca actual
                                        decimal inssActual = 0;
                                        inssActual = Decimal.Round((_IngresosGrabables * _ValorINSSLaboral), 2, MidpointRounding.AwayFromZero);

                                        //Se suman los INSS de la quincena pasada y de esta quincena (en caso de que sea la primera quincena no hay un INSS acumulado, solo el de esta quincena)
                                        decimal inssTotal = inssTotalMes + inssActual;

                                        //Entidades.TablaIRAcumulativo TIRA = db.TablaIRAcumulativos.Single(o => o.EmpleadoID == )

                                        //Si es la primera quincena
                                        if ((int)Convert.ToDateTime(PG.FechaHasta).Day < 16)
                                        {
                                            //Si el INSS calculado excede el limite de deducciones se le deduce la mitad del tope del INSS 
                                            if (inssTotal > vtopeQuincenal)
                                            {
                                                EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero) });
                                                _Egresos += vtopeQuincenal;
                                            }
                                            else
                                            {
                                                EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(inssActual, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(inssActual, 2, MidpointRounding.AwayFromZero) });
                                                _Egresos += inssActual;
                                            }
                                            //de lo contrario se graba el INSS normal del salario del empleado
                                            DPG.INSSTotalMes = inssTotal;
                                        }
                                        else
                                        {
                                            //Si para la segunda quincena el INSS es mayor al tope maximo del INSS / 2                                           
                                            if (inssTotal > vtopeMensual)
                                             {
                                                 decimal x = 0;

                                                 //decimal InssPlanillaIQ = InssPlanilla(empleado.IdEmpleado, planilla.FechaDesde, empleado.IdPlanilla);

                                                 x = inssTotal - inssTotalMes;

                                                 if (x > vtopeQuincenal)
                                                 {
                                                     EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                     DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero) });
                                                     DPG.INSSTotalMes = vtopeQuincenal;
                                                     _Egresos += vtopeQuincenal;
                                                 }
                                                 else
                                                 {
                                                     EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(x, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                     DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(x, 2, MidpointRounding.AwayFromZero) });
                                                     DPG.INSSTotalMes = x;
                                                     _Egresos += x;
                                                 }                                                
                                             }
                                             else
                                             {
                                                 decimal a = vtopeMensual - inssTotalMes;
                                                 decimal b = inssTotal - inssTotalMes;

                                                 if (b > a)
                                                 {
                                                     EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(a, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                     DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(a, 2, MidpointRounding.AwayFromZero) });
                                                     DPG.INSSTotalMes = a;
                                                     _Egresos += a;
                                                 }
                                                 else
                                                 {
                                                     EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(b, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                     DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(b, 2, MidpointRounding.AwayFromZero) });
                                                     DPG.INSSTotalMes = b;
                                                     _Egresos += b;
                                                 }
                                             }
                                        }
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.IREmpleado:
                                    {
                                        #region <<< IR >>>
                                        decimal vTotalIR = 0;
                                        //Valor para calcular el tope del salario maximo cotizable dividido en INSS Laboral
                                        decimal vtopeMensual = 0m;
                                        vtopeMensual = Decimal.Round(_MaximoSalarioINSS * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);

                                        //Mes que se genera la planilla
                                        int mesplanilla = Convert.ToInt32(PG.FechaDesde.Month);

                                        //Entidad de la tabla del IR acumulativo segun el trabajador asignado
                                        Entidad.IRRetenido tira;
                                        bool exist = false;

                                        //Obtener el IRRetenido acumulativo del empleado que se esta calculando la nomina
                                        var tiraGuest = EtIRRetenidos.FirstOrDefault(t => t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.Year && t.FechaIR.Month == PG.FechaDesde.Month));

                                        if (tiraGuest != null)
                                        {
                                            tira = db.IRRetenido.Single(s => s.ID.Equals(tiraGuest.ID));
                                            //Si existe se obtienen los valores de la tabla del IR acumulativo
                                            exist = true;
                                            //else
                                            if (reg.EsSalarioPromedio)
                                            tira.SueldoAcumulado =
                                                EtIRRetenidos.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && !PG.FechaDesde.Month.Equals(1) && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0
                                                ? EtIRRetenidos.Where(t => t.EmpleadoID == DPG.EmpleadoID && !PG.FechaDesde.Month.Equals(1) && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().SueldoAcumulado
                                                : 0m;
                                        }
                                        else
                                        {
                                            //Si no existe se crea una entidad nueva de TablaIRAcumulativo
                                            exist = false;
                                            tira = new Entidad.IRRetenido();
                                            tira.EmpleadoID = DPG.EmpleadoID;
                                            tira.FechaIR = PG.FechaDesde.Date;
                                            tira.SalarioBasico = Decimal.Round(DPG.SalarioBasico, 2, MidpointRounding.AwayFromZero);
                                            tira.MesesTranscurridos = mesplanilla;
                                            if (reg.EsSalarioPromedio)
                                            tira.SueldoAcumulado = EtIRRetenidos.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && !PG.FechaDesde.Month.Equals(1) && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0
                                                ? EtIRRetenidos.Where(t => t.EmpleadoID == DPG.EmpleadoID && !PG.FechaDesde.Month.Equals(1) && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().SueldoAcumulado
                                                : 0m;
                                        }

                                        //Valor para calcular el INSS total del empleado
                                        decimal vINSSTotalIR = 0m;

                                        //Si es el primer mes del calculo anual del IR y primera quincena ---- o el empleado no existe en la tabla de IR Retenidos
                                        if ((mesplanilla == 1 && (int)Convert.ToDateTime(PG.FechaDesde).Day < 15) || !exist)
                                        {
                                            //Obteniendo el INSS para la resta del total a recibie del calculo del IR
                                            decimal inssBasico = Decimal.Round(DPG.SalarioPlanilla * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);//INSS proyectado Segunda Quincena
                                            vINSSTotalIR = DPG.INSSTotalMes + inssBasico;

                                            if (vINSSTotalIR > vtopeMensual)//si es mayor al Tope deduccion INSS, el valor sera igual al tope valor del INSS
                                                vINSSTotalIR = vtopeMensual;

                                            tira.OtrosIngresosVirtual = Decimal.Round(_IngresosGrabables - DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero);
                                            tira.INSSLaboralVirtual = Decimal.Round(vINSSTotalIR, 2, MidpointRounding.AwayFromZero);

                                            if (reg.EsSalarioPromedio)
                                                tira.SueldoAcumulado += Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/  - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
                                            else
                                                tira.SueldoAcumulado = Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/  - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else //Del segundo Mes del calculo del IR anual en adelante
                                        {
                                            if ((int)Convert.ToDateTime(PG.FechaDesde).Day < 15)//Primera quincena del IR
                                            {
                                                decimal inssBasico = Decimal.Round(DPG.SalarioPlanilla * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);//INSS proyectado Segunda Quincena
                                                vINSSTotalIR = DPG.INSSTotalMes + inssBasico;

                                                if (vINSSTotalIR > vtopeMensual)
                                                    vINSSTotalIR = vtopeMensual;

                                                tira.OtrosIngresosVirtual = Decimal.Round(_IngresosGrabables - DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero);
                                                tira.INSSLaboralVirtual = Decimal.Round(vINSSTotalIR, 2, MidpointRounding.AwayFromZero);

                                                if (reg.EsSalarioPromedio)
                                                    tira.SueldoAcumulado += Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
                                                else
                                                    tira.SueldoAcumulado = Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
                                            }
                                            else//Segunda quincena del IR
                                            {
                                                //Se calcula el ingreso extra de la quincena
                                                tira.OtrosIngresosVirtual = Decimal.Round(tira.OtrosIngresos + _IngresosGrabables /*Ingresos Primera Quincena*/- DPG.SalarioPlanilla /*Se resta el salario recibido de la segunda quincena porque ya fue proyectado en la primera Quincena*/, 2, MidpointRounding.AwayFromZero);

                                                decimal DeduccionInssIngresosExtras = 0;
                                                decimal SalarioAnterior = SalarioPlanillaAnterior(DPG.EmpleadoID, PG.FechaDesde, PG.PlanillaID, db);

                                                decimal VariacionSalario = Decimal.Round(Convert.ToDecimal(DPG.SalarioPlanilla - SalarioAnterior), 2, MidpointRounding.AwayFromZero);

                                                if (tira.INSSLaboral > vtopeMensual)
                                                {
                                                    if (reg.EsSalarioPromedio)
                                                        tira.SueldoAcumulado += Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + VariacionSalario + tira.OtrosIngresosVirtual /*+ (detallePlanilla.ValorSubsidio)*/, 2, MidpointRounding.AwayFromZero);
                                                    else
                                                        tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + VariacionSalario + tira.OtrosIngresosVirtual /*+ (detallePlanilla.ValorSubsidio)*/, 2, MidpointRounding.AwayFromZero);
                                                }
                                                else
                                                {
                                                    DeduccionInssIngresosExtras = Decimal.Round((tira.OtrosIngresosVirtual - tira.OtrosIngresos + (tira.INSSLaboral >= vtopeMensual ? 0m : VariacionSalario)) * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);

                                                    decimal INSSIR = 0;

                                                    if ((vtopeMensual - tira.INSSLaboral) < DeduccionInssIngresosExtras)
                                                        INSSIR = vtopeMensual - tira.INSSLaboral;
                                                    else
                                                        INSSIR = DeduccionInssIngresosExtras + tira.INSSLaboral;

                                                    tira.INSSLaboralVirtual = INSSIR;

                                                    if (reg.EsSalarioPromedio)
                                                        tira.SueldoAcumulado += Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ tira.OtrosIngresosVirtual - INSSIR, 2, MidpointRounding.AwayFromZero);
                                                    else
                                                    {
                                                        tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ tira.OtrosIngresosVirtual - INSSIR, 2, MidpointRounding.AwayFromZero);
                                                        //if (tira.OtrosIngresosVirtual > 0)
                                                        //{
                                                        //    DateTime vFecha = new DateTime(PG.FechaHasta.Year, FechaHasta.Month, 01);
                                                        //    decimal vAcumulado = db.IRRetenido.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.FechaIR < vFecha).Sum(s => s.SueldoAcumulado);
                                                        //    tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + vAcumulado + tira.OtrosIngresosVirtual -INSSIR, 2, MidpointRounding.AwayFromZero);
                                                        //}
                                                        //else
                                                            //tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ -INSSIR, 2, MidpointRounding.AwayFromZero);
                                                    
                                                    }
                                                }

                                            }
                                        }


                                        tira.MesesTranscurridos = (Convert.ToInt32(Convert.ToDateTime(PG.FechaDesde).Month).Equals(1) ? 1 :
                                                                      (db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().MesesTranscurridos + 1 : 1)
                                                                        );
                                        //en case que sea promedio se divide entre los meses transcurridos de lo contrario se divide entre 1
                                        tira.SueldoPromedioMensual = Decimal.Round(tira.SueldoAcumulado / (reg.EsSalarioPromedio ? tira.MesesTranscurridos : 1), 2, MidpointRounding.AwayFromZero);

                                        tira.EspectativaAnual = Decimal.Round(tira.SueldoPromedioMensual * 12, 2, MidpointRounding.AwayFromZero);
                                        
                                        var rangoIR = EtTablaIR.SingleOrDefault(ir => tira.EspectativaAnual > ir.Desde && tira.EspectativaAnual <= ir.Hasta);

                                        if (rangoIR != null)
                                        {
                                            tira.ImpuestoBase = Decimal.Round(rangoIR.Base, 2, MidpointRounding.AwayFromZero);
                                            tira.PorcentajeAplicable = rangoIR.Tasa;
                                            tira.SobreExceso = rangoIR.Techo;
                                        }

                                        tira.Total = Decimal.Round(tira.ImpuestoBase + (tira.PorcentajeAplicable * (tira.EspectativaAnual - tira.SobreExceso)), 2, MidpointRounding.AwayFromZero);

                                        tira.IRAnualSobre12 = Decimal.Round(tira.Total / 12m, 2, MidpointRounding.AwayFromZero);

                                        if (reg.EsSalarioPromedio)
                                        {
                                            tira.IRMesesTranscurridos = Decimal.Round(tira.IRAnualSobre12 * tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

                                            if (tira.MesesPorRetener.Equals(1))
                                                tira.RetencionAcumulada = 0m;
                                            else
                                            {
                                                decimal RetencionAcumuladaAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionAcumulada : 0m;
                                                decimal RetencionMesAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionMes : 0m;

                                                tira.RetencionAcumulada = RetencionAcumuladaAnterior + RetencionMesAnterior;
                                            }
                                            tira.RetencionMes = Decimal.Round(tira.IRMesesTranscurridos - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);
                                        }
                                        else
                                        {
                                            tira.RetencionMes = tira.IRAnualSobre12;
                                            //if (tira.OtrosIngresosVirtual > 0)
                                            //{
                                            //    tira.IRMesesTranscurridos = Decimal.Round(tira.IRAnualSobre12 * tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);
                                            //    tira.RetencionMes = Decimal.Round(tira.IRMesesTranscurridos - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);
                                            //}
                                            //else
                                            //    tira.RetencionMes = tira.IRAnualSobre12;
                                        }

                                        if ((int)Convert.ToDateTime(PG.FechaDesde).Day < 15)
                                        {
                                            tira.IRRetenidoI = Decimal.Round(tira.RetencionMes / 2, 2, MidpointRounding.AwayFromZero);

                                            vTotalIR = (tira.IRRetenidoI < 0 ? 0m : tira.IRRetenidoI);
                                            _Egresos += vTotalIR;

                                        }
                                        else
                                        {
                                            tira.IRRetenidoII = Decimal.Round(tira.RetencionMes - tira.IRRetenidoI, 2, MidpointRounding.AwayFromZero);
                                            vTotalIR = (tira.IRRetenidoII < 0 ? 0m : tira.IRRetenidoII);
                                            _Egresos += vTotalIR;
                                        }

                                        #region <<< ANTERIOR 2016 >>>
                                        /*
                                        tira.MesesTranscurridos = (Convert.ToInt32(Convert.ToDateTime(PG.FechaDesde).Month).Equals(1) ? 1 :
                                                                      (db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().MesesTranscurridos + 1 : 1)
                                                                        );
                 
                                        tira.SueldoPromedioMensual = Decimal.Round(tira.SueldoAcumulado / tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

                                        tira.EspectativaAnual = Decimal.Round(tira.SueldoPromedioMensual * 12, 2, MidpointRounding.AwayFromZero);

                                        var rangoIR = EtTablaIR.SingleOrDefault(ir => tira.EspectativaAnual > ir.Desde && tira.EspectativaAnual <= ir.Hasta);

                                        if (rangoIR != null)
                                        {
                                            tira.ImpuestoBase = Decimal.Round(rangoIR.Base, 2, MidpointRounding.AwayFromZero);
                                            tira.PorcentajeAplicable = rangoIR.Tasa;
                                            tira.SobreExceso = rangoIR.Techo;
                                        }

                                        tira.Total = Decimal.Round(tira.ImpuestoBase + (tira.PorcentajeAplicable * (tira.EspectativaAnual - tira.SobreExceso)), 2, MidpointRounding.AwayFromZero);

                                        tira.IRAnualSobre12 = Decimal.Round(tira.Total / 12m, 2, MidpointRounding.AwayFromZero);
                                        tira.IRMesesTranscurridos = Decimal.Round(tira.IRAnualSobre12 * tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

                                        if (tira.MesesPorRetener.Equals(1))
                                            tira.RetencionAcumulada = 0m;
                                        else
                                        {
                                            decimal RetencionAcumuladaAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionAcumulada : 0m;
                                            decimal RetencionMesAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionMes : 0m;

                                            tira.RetencionAcumulada = RetencionAcumuladaAnterior + RetencionMesAnterior;
                                        }

                                        tira.RetencionMes = Decimal.Round(tira.IRMesesTranscurridos - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);

                                        if ((int)Convert.ToDateTime(PG.FechaDesde).Day < 15)
                                        {
                                            tira.IRRetenidoI = Decimal.Round(tira.RetencionMes / 2, 2, MidpointRounding.AwayFromZero);

                                            vTotalIR = (tira.IRRetenidoI < 0 ? 0m : tira.IRRetenidoI);
                                            _Egresos += vTotalIR;

                                        }
                                        else
                                        {
                                            tira.IRRetenidoII = Decimal.Round(tira.RetencionMes - tira.IRRetenidoI, 2, MidpointRounding.AwayFromZero);
                                            vTotalIR = (tira.IRRetenidoII < 0 ? 0m : tira.IRRetenidoII);
                                            _Egresos += vTotalIR;
                                        }
                                        */
                                        #endregion

                                        #region <<< ANTERIOR 2015  >>>
                                        /*ANTERIOR ******************************************
                                            decimal RetencionAcumuladaAnterior = EtIRRetenidos.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0
                                                ? EtIRRetenidos.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionAcumulada
                                                : 0m;
                                            decimal RetencionMesAnterior = EtIRRetenidos.Count(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)) > 0
                                                ? EtIRRetenidos.Where(t => t.ID != tira.ID && t.EmpleadoID == DPG.EmpleadoID && (t.FechaIR.Year == PG.FechaDesde.AddMonths(-1).Year && t.FechaIR.Month == PG.FechaDesde.AddMonths(-1).Month)).First().RetencionMes
                                                : 0m;

                                            tira.RetencionAcumulada = Decimal.Round(RetencionAcumuladaAnterior + RetencionMesAnterior, 2, MidpointRounding.AwayFromZero);



                                            tira.SaldoPorRetener = Decimal.Round(tira.Total - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);

                                            tira.MesesPorRetener = Convert.ToInt32(13 - mesplanilla);
                                            tira.RetencionMes = Decimal.Round(tira.SaldoPorRetener / tira.MesesPorRetener, 2, MidpointRounding.AwayFromZero);

                                           

                                            if ((int)Convert.ToDateTime(PG.FechaDesde).Day < 15)
                                            {
                                                //if (empleado.FullIRPrimera)
                                                //{
                                                //    tira.IRRetenidoI = Decimal.Round(tira.RetencionMes, 2, MidpointRounding.AwayFromZero);
                                                //    totalIR = tira.IRRetenidoI;
                                                //}
                                                //else
                                                //{
                                                tira.IRRetenidoI = Decimal.Round(tira.RetencionMes / 2, 2, MidpointRounding.AwayFromZero);

                                                if (tira.IRRetenidoI < 0)
                                                    tira.IRRetenidoI = 0m;

                                                vTotalIR = tira.IRRetenidoI;
                                                _Egresos += vTotalIR;
                                                //}

                                            }
                                            else
                                            {
                                                //if (!empleado.FullIRPrimera)
                                                //{
                                                tira.IRRetenidoII = Decimal.Round(tira.RetencionMes - tira.IRRetenidoI, 2, MidpointRounding.AwayFromZero);

                                                if (tira.IRRetenidoII < 0)
                                                    tira.IRRetenidoII = 0m;

                                                vTotalIR = tira.IRRetenidoII;
                                                _Egresos += vTotalIR;
                                                //}
                                                //else
                                                //{
                                                //     tira.IRRetenidoII = 0m;
                                                //     totalIR = 0m;
                                                //// }

                                            }
                                        }
                                        */


                                        #endregion

                                        if (!exist)
                                            db.IRRetenido.InsertOnSubmit(tira);

                                        db.SubmitChanges();

                                        //Guardar el IR del  Empleado
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(vTotalIR, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(vTotalIR, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.ProvIndemnizacion:
                                    {
                                        #region <<< INSS PROVISION INDEMNIZACION >>>
                                        //Obtener el saldo de la Provision Indemnizacion
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.ProvVacaciones:
                                    {
                                        #region <<< INSS PROVISION VACACIONES >>>
                                        //Obtener el saldo de la Provision vacaciones
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.ProvAguinaldo:
                                    {
                                        #region <<< INSS PROVISION AGUINALDO >>>
                                        //Obtener el saldo de la Provision Aguinaldo
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(_Provision, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.INSSPatronal:
                                    {
                                        #region <<< INSS PATRONAL >>>
                                        decimal inssTotalPatronalMes = 0;
                                        //Valor para calcular el tope del salario maximo cotizable dividido en INSS patronal
                                        decimal vtopeQuincenal = 0m, vtopeMensual = 0m;
                                        vtopeQuincenal = Decimal.Round((_MaximoSalarioINSS * _ValorINSSPatronal) / 2m, 2, MidpointRounding.AwayFromZero);
                                        vtopeMensual = Decimal.Round(_MaximoSalarioINSS * _ValorINSSPatronal, 2, MidpointRounding.AwayFromZero);

                                        //Obtener el INSS Patronal Segunda Quincena
                                        if ((int)Convert.ToDateTime(PG.FechaHasta).Day > 15)
                                            inssTotalPatronalMes = InssPatronalPagado(DPG.EmpleadoID, PG.FechaDesde, PG.PlanillaID, db);

                                        decimal inssPatronalActual = 0;
                                        inssPatronalActual = Decimal.Round((_IngresosGrabables * _ValorINSSPatronal), 2, MidpointRounding.AwayFromZero);

                                        decimal inssPatronalTotal = inssTotalPatronalMes + inssPatronalActual;

                                        //Si es la primera quincena
                                        if ((int)Convert.ToDateTime(PG.FechaHasta).Day < 16)
                                        {

                                            //Si el INSS calculado excede el limite de deducciones se le deduce la mitad del tope del INSS 
                                            if (inssPatronalTotal > vtopeQuincenal)
                                            {
                                                EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero) });
                                            }
                                            else
                                            {
                                                EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(inssPatronalActual, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(inssPatronalActual, 2, MidpointRounding.AwayFromZero) });
                                            }
                                            //de lo contrario se graba el INSS normal del salario del empleado

                                            DPG.INSSPatronalTotalMes = inssPatronalTotal;

                                        }
                                        else
                                        {
                                            //Si para la segunda quincena el INSS es mayor al tope maximo del INSS / 2                                           
                                            if (inssPatronalActual > vtopeQuincenal)
                                            {
                                                decimal x = 0;

                                                //decimal InssPlanillaIQ = InssPlanilla(empleado.IdEmpleado, planilla.FechaDesde, empleado.IdPlanilla);

                                                x = inssPatronalTotal - inssTotalPatronalMes;

                                                if (x > vtopeQuincenal)
                                                {
                                                    EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(vtopeQuincenal, 2, MidpointRounding.AwayFromZero) });
                                                    DPG.INSSPatronalTotalMes = vtopeQuincenal;
                                                }
                                                else
                                                {
                                                    EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(x, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(x, 2, MidpointRounding.AwayFromZero) });
                                                    DPG.INSSPatronalTotalMes = inssPatronalTotal;
                                                }
                                            }
                                            else
                                            {
                                                decimal a = vtopeMensual - inssTotalPatronalMes;
                                                decimal b = inssPatronalTotal - inssTotalPatronalMes;

                                                if (b > a)
                                                {
                                                    EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(a, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(a, 2, MidpointRounding.AwayFromZero) });
                                                    DPG.INSSPatronalTotalMes = inssPatronalTotal;
                                                }
                                                else
                                                {
                                                    EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(b, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                                    DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(b, 2, MidpointRounding.AwayFromZero) });
                                                    DPG.INSSPatronalTotalMes = inssPatronalTotal;
                                                }
                                            }
                                            //Si para la segunda quincena el INSS es mayor al tope maximo del INSS / 2
                                            //if (inssTotalPatronalMes > (Classes.Global.TopePagoINSSPatronal() / 2))
                                            //{
                                            //    decimal x = 0;

                                            //    decimal InssPatronalPlanillaIQ = InssPatronalPlanilla(empleado.IdEmpleado, planilla.FechaDesde, empleado.IdPlanilla);

                                            //    x = inssPatronalTotal - InssPatronalPlanillaIQ;

                                            //    if (x > (Classes.Global.TopePagoINSSPatronal() / 2m))
                                            //        detallePlanilla.INSSPatronal = Decimal.Round(Convert.ToDecimal(Classes.Global.TopePagoINSSPatronal() / 2), 2);

                                            //    else
                                            //    {
                                            //        detallePlanilla.INSSPatronal = x;
                                            //    }

                                            //    detallePlanilla.INSSPatronalTotalMes = inssPatronalTotal;

                                            //}
                                            //else
                                            //{
                                            //    decimal a = Classes.Global.TopePagoINSSPatronal() - inssTotalPatronalMes;
                                            //    decimal b = inssPatronalTotal - inssTotalPatronalMes;

                                            //    if (b > a)
                                            //        detallePlanilla.INSSPatronal = a;
                                            //    else
                                            //        detallePlanilla.INSSPatronal = b;

                                            //    detallePlanilla.INSSPatronalTotalMes = 0;

                                            //}
                                        }

                                        break;
                                        #endregion
                                    }
                                case (int)Parametros.TiposMovimientoEmpleadoID.INATEC:
                                    {
                                        #region <<< INSS INATEC >>>
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(Decimal.Round(_IngresosGrabables, 2, MidpointRounding.AwayFromZero) * _ValorINATEC, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = item.CuentaContableCreditoID, CuentaContableDebitoID = item.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = item.ID, Monto = Decimal.Round(Decimal.Round(_IngresosGrabables, 2, MidpointRounding.AwayFromZero) * _ValorINATEC, 2, MidpointRounding.AwayFromZero) });
                                        break;
                                        #endregion
                                    }
                                default:
                                    break;
                            }

                        }

                        #endregion

                        #endregion


                        #region <<< PENSION ALIMENTACION >>>
                        if (reg.HasPension != null)
                        {
                            if (Convert.ToBoolean(reg.HasPension))
                            {
                                if (reg.EsPorcentaje != null)
                                {
                                    if (Convert.ToBoolean(reg.EsPorcentaje))
                                    {
                                        _Pension = Decimal.Round(Convert.ToDecimal(_IngresosGrabables * (reg.MontoPension / 100m)), 2, MidpointRounding.AwayFromZero);
                                        _Egresos += Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero);
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = EtPension.CuentaContableCreditoID, CuentaContableDebitoID = EtPension.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = EtPension.ID, MovimientoEmpleadoID = reg.EmpleadoID, Cantidad = 0, Cuotas = 1, Monto = Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero) });

                                    }
                                    else
                                    {
                                        _Pension = Decimal.Round(Convert.ToDecimal(reg.MontoPension), 2);
                                        _Egresos += Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero);
                                        EtComprobante.Add(new Parametros.ListComprobanteNomina { AreaCentoCostoID = ceco, Monto = Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero), CuentaContableCreditoID = EtPension.CuentaContableCreditoID, CuentaContableDebitoID = EtPension.CuentaContableDebitoID, Descripcion = texto, Orden = Orden, DifTarjeta = DifTarjeta });
                                        DPG.DetalleMovimientoPlanilla.Add(new Entidad.DetalleMovimientoPlanilla { TipoMovimientoEmpleadoID = EtPension.ID, MovimientoEmpleadoID = reg.EmpleadoID, Cantidad = 0, Cuotas = 1, Monto = Decimal.Round(_Pension, 2, MidpointRounding.AwayFromZero) });

                                    }
                                }

                            }
                        }
                        #endregion


                        DPG.TotalIngresos = Decimal.Round(_IngresosGrabables, 2, MidpointRounding.AwayFromZero) + Decimal.Round(_IngresosNoGrabables, 2, MidpointRounding.AwayFromZero);
                        DPG.TotalEgresos = Decimal.Round(_Egresos, 2, MidpointRounding.AwayFromZero);
                        DPG.Total = Decimal.Round(DPG.TotalIngresos, 2, MidpointRounding.AwayFromZero) - Decimal.Round(DPG.TotalEgresos, 2, MidpointRounding.AwayFromZero);
                        PG.DetallePlanillaGenerada.Add(DPG);

                        progressBarCont.PerformStep();
                        progressBarCont.Update();
                    }
                    #endregion


                    #region <<< GENERANDO COMPROBANTE >>>

                    List<Entidad.ComprobanteContable> Compronbante = new List<Entidad.ComprobanteContable>();
                    int i = 0;
                    
                    var Debitos = EtComprobante.Where(o => !o.CuentaContableDebitoID.Equals(0)).OrderBy(o => o.Orden);

                    foreach (var gr in Debitos.GroupBy(g => g.Orden))
                    {
                        foreach (var line in gr.GroupBy(g => g.AreaCentoCostoID).OrderBy(o => o.Key))
                        {
                            foreach (var det in Debitos.Where(o => (gr.Key == null || o.Orden.Equals(gr.Key)) && (line.Key.Equals(0) || o.AreaCentoCostoID.Equals(line.Key))))
                        {
                            if (Compronbante.Count(s => s.CuentaContableID.Equals(det.CuentaContableDebitoID) && s.CentroCostoID.Equals(det.AreaCentoCostoID)) <= 0)
                            {
                                Compronbante.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = det.CuentaContableDebitoID,
                                    Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(det.Monto), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(det.Monto) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaHasta.AddDays(-2),
                                    Descripcion = det.Descripcion,
                                    Linea = i,
                                    CentroCostoID = det.AreaCentoCostoID,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                i++;
                            }
                            else
                            {
                                var comprobante = Compronbante.Where(c => c.CuentaContableID.Equals(det.CuentaContableDebitoID) && c.CentroCostoID.Equals(det.AreaCentoCostoID)).First();
                                comprobante.Monto += Decimal.Round(Math.Abs(Convert.ToDecimal(det.Monto)), 2, MidpointRounding.AwayFromZero);
                                comprobante.MontoMonedaSecundaria += Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(det.Monto)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            }

                            
                            }

                        }
                    }

                    var Creditos = EtComprobante.Where(o => !o.CuentaContableCreditoID.Equals(0) && !o.Monto.Equals(0));

                    foreach (var gr in Creditos.GroupBy(g => g.CuentaContableCreditoID))
                    {
                        Compronbante.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = gr.Key,
                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(Creditos.Where(c => c.CuentaContableCreditoID.Equals(gr.Key)).Sum(s => s.Monto)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(Creditos.Where(c => c.CuentaContableCreditoID.Equals(gr.Key)).Sum(s => s.Monto)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaHasta.AddDays(-2),
                            Descripcion = texto,
                            Linea = i,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                    }

                    var vTotalNeto = from p in PG.DetallePlanillaGenerada
                                      join o in obj on p.EmpleadoID equals o.EmpleadoID
                                      select new 
                                      {
                                          o.DifTaejeta,
                                          p.Total
                                      };

                    if (vTotalNeto.Count(o => !o.DifTaejeta) > 0)
                    {
                        Compronbante.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = IDCuentaPlanilla,
                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(vTotalNeto.Where(o => !o.DifTaejeta).Sum(s => s.Total)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(vTotalNeto.Where(o => !o.DifTaejeta).Sum(s => s.Total)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaHasta.AddDays(-2),
                            Descripcion = texto,
                            Linea = i,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                    }

                    if (vTotalNeto.Count(o => o.DifTaejeta) > 0)
                    {
                        Compronbante.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = 244,
                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(vTotalNeto.Where(o => o.DifTaejeta).Sum(s => s.Total)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(vTotalNeto.Where(o => o.DifTaejeta).Sum(s => s.Total)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaHasta.AddDays(-2),
                            Descripcion = texto,
                            Linea = i,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                    }


                    //List<Entidad.ComprobanteContable> Compronbante = (from x in CD
                    //                                                  select new Entidad.ComprobanteContable
                    //                                                  {
                    //                                                      CuentaContableID = x.CuentaContableID,
                    //                                                      Monto = Decimal.Round((x.Debito.HasValue ? Convert.ToDecimal(x.Debito) : 0m) - (x.Credito.HasValue ? Convert.ToDecimal(x.Credito) : 0m), 2, MidpointRounding.AwayFromZero),
                    //                                                      TipoCambio = _TipoCambio,
                    //                                                      MontoMonedaSecundaria = Decimal.Round(((x.Debito.HasValue ? Convert.ToDecimal(x.Debito) : 0m) - (x.Credito.HasValue ? Convert.ToDecimal(x.Credito) : 0m)) / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                    //                                                      Fecha = Fecha,
                    //                                                      Descripcion = x.Descripcion,
                    //                                                      CentroCostoID = x.CentroCostoID,
                    //                                                      EstacionServicioID = IDEstacionServicio,
                    //                                                      SubEstacionID = IDSubEstacion
                    //                                                  }).ToList();

                    //var obj = from cds in Compronbante
                    //          join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                    //          join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                    //          select new
                    //          {
                    //              Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                    //              Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                    //          };

                    //if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    //{
                    //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                    //    trans.Rollback();
                    //    return false;
                    //}

                    //M.ComprobanteContables.Clear();

                    //int l = 1;
                    //Compronbante.ForEach(linea =>
                    //    {
                    //        linea.Linea = l;
                    //        M.ComprobanteContables.Add(linea);
                    //        l++;
                    //    });

                    #endregion

                    Entidad.Movimiento M;

                    if (_Editable)
                        M = null;
                    else
                    {
                        M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 72;
                        M.Comentario = texto;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer()).Date;
                        M.FechaRegistro = FechaHasta.AddDays(-2).Date;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                    }

                    M.UsuarioID = UsuarioID;
                    M.Monto = Decimal.Round(PG.DetallePlanillaGenerada.Sum(s => s.Total), 2, MidpointRounding.AwayFromZero);
                    M.MonedaID = _MonedaPrimaria;

                    M.MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(PG.DetallePlanillaGenerada.Sum(s => s.Total)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                    M.TipoCambio = _TipoCambio;

                    int number = 1;
                    var Mov = db.Movimientos.Select(s => new { s.EstacionServicioID, s.SubEstacionID, s.MovimientoTipoID, s.Numero }).Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(72)).OrderByDescending(o => o.Numero).FirstOrDefault();
                    if (Mov != null)
                    {
                        number = Parametros.General.ValorConsecutivo(Mov.Numero.ToString());
                    }

                    M.Numero = number;

                    db.Movimientos.InsertOnSubmit(M);
                    db.SubmitChanges();

                    if (_Editable)
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó la Nómina: " + lkPlanilla.Text + " Desde " + FechaDesde.Date.ToShortDateString() + " Hasta " + FechaDesde.Date.ToShortDateString(), this.Name);

                    }
                    else
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se modificó la Nómina: " + lkPlanilla.Text + " Desde " + FechaDesde.Date.ToShortDateString() + " Hasta " + FechaDesde.Date.ToShortDateString(), this.Name);
                    }

                    PG.MovimientoID = M.ID;

                    #region <<< REGISTRANDO COMPROBANTE >>>

                    //var objCD = from cds in Compronbante
                    //          join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                    //          join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                    //          select new
                    //          {
                    //              Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                    //              Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                    //          };

                    //if (!(Decimal.Round(objCD.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((objCD.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    //{
                    //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                    //    trans.Rollback();
                    //    return false;
                    //}

                    Compronbante.ForEach(linea =>
                    {
                        M.ComprobanteContables.Add(linea);
                        db.SubmitChanges();
                    });

                    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    IDPrint = PG.ID;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    
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
                    progressBarCont.EditValue = 0;
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        private decimal InssPagado(int empleado, DateTime fecha, int IdPlanilla, Entidad.SAGASDataClassesDataContext con)
        {
            if (fecha.Day > 15)
            {
                var planilla = con.PlanillaGenerada.Where(p => p.PlanillaID.Equals(IdPlanilla) && p.Aprobado && (p.FechaDesde.Day) < 15).OrderByDescending(o => o.FechaDesde).FirstOrDefault();


                if (planilla != null)
                {
                    var lst = planilla.DetallePlanillaGenerada.SingleOrDefault(s => s.EmpleadoID.Equals(empleado));

                    if (lst != null)
                    {
                        var inss = lst.DetalleMovimientoPlanilla.FirstOrDefault(s => s.TipoMovimientoEmpleadoID.Equals((int)Parametros.TiposMovimientoEmpleadoID.INSSLaboral));

                        if (inss != null)
                            return Decimal.Round(inss.Monto, 2, MidpointRounding.AwayFromZero);
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        private decimal SalarioPlanillaAnterior(int empleado, DateTime fecha, int IdPlanilla, Entidad.SAGASDataClassesDataContext con)
        {
            if (fecha.Day > 15)
            {
                var planilla = con.PlanillaGenerada.Where(p => p.PlanillaID.Equals(IdPlanilla) && p.Aprobado && (p.FechaDesde.Day) < 15).OrderByDescending(o => o.FechaDesde).FirstOrDefault();


                if (planilla != null)
                {
                    var lst = planilla.DetallePlanillaGenerada.SingleOrDefault(s => s.EmpleadoID.Equals(empleado));

                    if (lst != null)
                    {
                        var inss = lst.DetalleMovimientoPlanilla.FirstOrDefault(s => s.TipoMovimientoEmpleadoID.Equals((int)Parametros.TiposMovimientoEmpleadoID.Salario));

                        if (inss != null)
                            return Decimal.Round(inss.Monto, 2, MidpointRounding.AwayFromZero);
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        private decimal InssPatronalPagado(int empleado, DateTime fecha, int IdPlanilla, Entidad.SAGASDataClassesDataContext con)
        {
            if (fecha.Day > 15)
            {
                var planilla = con.PlanillaGenerada.Where(p => p.PlanillaID.Equals(IdPlanilla) && p.Aprobado && (p.FechaDesde.Day) < 15).OrderByDescending(o => o.FechaDesde).FirstOrDefault();


                if (planilla != null)
                {
                    var lst = planilla.DetallePlanillaGenerada.SingleOrDefault(s => s.EmpleadoID.Equals(empleado));

                    if (lst != null)
                    {
                        var inss = lst.DetalleMovimientoPlanilla.FirstOrDefault(s => s.TipoMovimientoEmpleadoID.Equals((int)Parametros.TiposMovimientoEmpleadoID.INSSPatronal));

                        if (inss != null)
                            return Decimal.Round(inss.Monto, 2, MidpointRounding.AwayFromZero);
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
                return 0;
        }
        //Para determinar el salario de la planilla anterior del empleado
        private decimal SalarioPlanillaAnterior(int empleado, DateTime fecha, int IdPlanilla)
        {
            //DateTime date = fecha.AddDays(-1);
            //var planilla = db.PlanillaGenerada.Where(p => p.Planilla == IdPlanilla && p.FechaHasta == date);

            //if (planilla.Count() > 0)
            //{
            //    var lst = db.DetallePlanilla.Where(l => l.Empleado == empleado && l.PlanillaGenerada == planilla.First().ID);
            //    if (lst.Count() <= 0)
            //        return 0;
            //    return Decimal.Round(Convert.ToDecimal(lst.First().SalarioBasico / 2m), 2);
            //}
            //else
                return 0;
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
            MDI.CleanDialog(ShowMsg, RefreshMDI, IDPrint);
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
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
            
            return base.ProcessCmdKey(ref msg, keyData);
        }        
        
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            //if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            //{
            //    _TipoCambio = 0;
            //    dateFechaDesde.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            //}
            //else
            //    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
            //            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);            

        }

        public static DateTime GetNextQuincenaHere(DateTime fecha)
        {
            DateTime date = new DateTime();
            try
            {
                int dia = fecha.Day;
                if (dia <= 15)
                    date = new DateTime(fecha.Year, fecha.Month, 15);
                else
                {
                    date = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
                    date = date.AddDays(-1); //Ultimo dia del mes
                }
                return date;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return date;
            }
        }

        private void dateFechaDesde_EditValueChanged(object sender, EventArgs e)
        { 
            try{ 
                dateFechaHasta.EditValue = GetNextQuincenaHere(dateFechaDesde.DateTime);
              }catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaDesde_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                dateFechaHasta.EditValue =  GetNextQuincenaHere(dateFechaDesde.DateTime.Date);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
           }
        }

        private void botMostrarEmpleado_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);

                if (IDPlanilla > 0)
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                    EtEmpleado = (from E in db.Empleados
                                  join P in db.Planillas on E.PlanillaID equals P.ID
                                  join an in db.AreaNomina on E.AreaNominaID equals an.ID into DefaultCosto
                                  from centro in DefaultCosto.DefaultIfEmpty()
                                  join et in db.EstructuraOrganizativa on E.EstructuraOrganizativaID equals et.ID into DefaultOrden
                                  from orden in DefaultOrden.DefaultIfEmpty()
                                  where (E.Activo && !E.Liquidado) && P.ID.Equals(IDPlanilla) && (E.FechaIngreso.HasValue ? Convert.ToDateTime(E.FechaIngreso) : Convert.ToDateTime("01/01/2000")).Date <= FechaHasta.Date
                                  select new Parametros.ListToGeneratePayroll
                             {
                                 EmpleadoID = E.ID,
                                 Selected = E.Activo,
                                 Codigo = E.Codigo,
                                 Planilla = P.Nombre,
                                 NombreCompleto = E.Nombres + " " + E.Apellidos,
                                 AreaID = E.AreaNominaID,
                                 CentroCostoID = (centro != null ? centro.CentroCostoID : 0),
                                 CargoID = E.CargoID,
                                 INSS = E.NumeroSeguroSocial,
                                 SalarioBasico = E.SalarioActual,
                                 EstructuraOrganizativaID = E.EstructuraOrganizativaID,
                                 OrdenCD = orden.Nombre,
                                 DifTaejeta = E.DiferenteDeTarjeta,
                                 FechaIngreso = (E.FechaIngreso.HasValue ? Convert.ToDateTime(E.FechaIngreso) : Convert.ToDateTime("01/01/2000")),
                                 HasPension = E.PagaPension,
                                 EsPorcentaje = E.EsPorcentaje,
                                 MontoPension = E.MontoPension,
                                 EsSalarioPromedio = E.CalculoSalarioPromedioAguinaldo

                             }).ToList();

                    bdsDetalle.DataSource = EtEmpleado;

                    this.gvData.RefreshData();

                    //Cargar la entidad de IRRetenidos de acuerdo a los empleados seleccionados
                    EtIRRetenidos = (from ir in db.IRRetenido
                                     where EtEmpleado.Select(s => s.EmpleadoID).Contains(ir.EmpleadoID)
                                     select ir).ToList();

                    //Cargar los Ingresos
                    EtIngresos = (from ig in db.MovimientoEmpleado
                                  join t in db.TipoMovimientoEmpleado on ig.TipoMovimientoEmpleadoID equals t.ID 
                                  where EtEmpleado.Select(s => s.EmpleadoID).Contains(ig.EmpleadoID) && (ig.FechaInicial.Date >= FechaDesde.Date && ig.FechaFinal.Date <= FechaHasta.Date) && !ig.Pagado && ig.MovimientoHorasExtrasID.Equals(0) && t.EsIngreso
                                  select ig).ToList();

                    //Cargar los Egresos
                    EtEgresos = (from ig in db.MovimientoEmpleado
                                 join t in db.TipoMovimientoEmpleado on ig.TipoMovimientoEmpleadoID equals t.ID
                                 where EtEmpleado.Select(s => s.EmpleadoID).Contains(ig.EmpleadoID) 
                                 && ((FechaDesde.Date >= ig.FechaInicial.Date && FechaHasta.Date <= ig.FechaFinal.Date) 
                                 || (ig.FechaInicial.Date >= FechaDesde.Date && (ig.FechaFinal.Date <= FechaHasta.Date) || (FechaHasta.Date <= ig.FechaFinal)) 
                                 || (FechaDesde.Date >= ig.FechaInicial.Date && (ig.FechaFinal.Date <= FechaHasta.Date && ig.FechaFinal.Date >= FechaDesde.Date)) 
                                 || (ig.FechaInicial.Date <= FechaDesde.Date && FechaHasta.Date <= ig.FechaFinal.Date)) 
                                 && !ig.Pagado && ig.MovimientoHorasExtrasID.Equals(0) && !t.EsIngreso
                                  select ig).ToList();

                    //Cargar las Horas Extras
                    EtHorasExtras = (from ig in db.MovimientoEmpleado
                                     join he in db.MovimientoHorasExtras on ig.MovimientoHorasExtrasID equals he.ID
                                     where he.PlanillaID.Equals(IDPlanilla) && EtEmpleado.Select(s => s.EmpleadoID).Contains(ig.EmpleadoID)
                                     && (ig.FechaFinal.Date >= FechaDesde.Date && ig.FechaFinal.Date <= FechaHasta.Date) && !ig.Pagado 
                                     && !ig.MovimientoHorasExtrasID.Equals(0) && he.Approved
                                  select ig).ToList();

                    var Plan = db.Planillas.Select(o => new { o.EstacionServicioID, o.SubEstacionID, o.CuentaContableID, o.ID }).Single(s => s.ID.Equals(IDPlanilla));

                    IDEstacionServicio = Plan.EstacionServicioID;
                    IDSubEstacion = (Plan.SubEstacionID > 0 ? Plan.SubEstacionID : db.EstacionServicios.Single(s => s.ID.Equals(IDEstacionServicio)).SubEstacionPrincipalID);
                    IDCuentaPlanilla = Plan.CuentaContableID;

                    int number = 1;
                    var Mov = db.Movimientos.Select(s => new { s.EstacionServicioID, s.SubEstacionID, s.MovimientoTipoID, s.Numero}).Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(72)).OrderByDescending(o => o.Numero).FirstOrDefault();
                    if (Mov != null)
                    {
                        number = Parametros.General.ValorConsecutivo(Mov.Numero.ToString());
                    }
                    Numero = number;

                    this.botMostrarEmpleado.Enabled = false;
                    this.lkPlanilla.Enabled = false;
                    this.dateFechaDesde.Enabled = false;
                    this.dateFechaHasta.Enabled = false;
                }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        private void tglEmpleado_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (EtEmpleado != null)
                {
                    if (tglEmpleado.IsOn)
                    {

                        if (EtEmpleado.Count > 0)
                        {
                            EtEmpleado.ForEach(fe => fe.Selected = false);
                            this.gvData.RefreshData();
                        }
                    }
                    else if (!tglEmpleado.IsOn)
                    {
                        if (EtEmpleado.Count > 0)
                        {
                            EtEmpleado.ForEach(fe => fe.Selected = true);
                            this.gvData.RefreshData();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, err.Message);
            }
        }

        #endregion
    }
}
