using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms.Wizards
{
    public partial class wizLiquidacion : Form
    {
        #region Declaraciones


        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.Liquidacion EtLiquidacion;
        private Entidad.Empleado ETEmpleado;
        private System.Data.DataTable ingresos;
        private System.Data.DataTable deducciones;
        private System.Data.DataTable salarios;
        private System.Data.DataTable vacaciones;
        internal FormLiquidacionesEmpleados MDI;
        private BindingSource bdsIngresos = new BindingSource();
        private BindingSource bdsDeduccion = new BindingSource();
        internal DateTime _FechaServidor;
        private string datos;
        private decimal _SalarioActualEmpleado = 0;
        private decimal SalarioBaseGrabable = 0;
        private decimal SalarioDiarioGrabable = 0;
        private decimal _TotalIngresosAntesRetencion = 0;
        private decimal CantDiasVacacionesGrabable = 0;
        private decimal MontoDiasVacionesGrabable = 0;
        private decimal CantDiasTrabajoGrabable = 0;
        private decimal MontoDiasTrabajoGrabable = 0;
        internal decimal _MaximoSalarioINSS = 0;
        internal decimal _ValorINSSLaboral = 0;
        private bool RefreshMDI = false;
        private decimal diasTrans = 0;


        #endregion

        #region Propiedades

        //public int ID
        //{
        //    get { return liquidacion.ID; }
        //}

        #endregion

        #region Inicializacion
        public wizLiquidacion()
        {
            InitializeComponent();
        }

        private void wizLiquidacion_Shown(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                EtLiquidacion = new Entidad.Liquidacion();
                gridEmpleados.DataSource = db.Empleados.Select(s => new { s.ID, s.Codigo, s.Nombres, s.Apellidos, s.Cedula, s.NumeroSeguroSocial, s.Activo }).Where(em => em.Activo);
                _FechaServidor = Convert.ToDateTime(db.GetDateServer()).Date;
                dateFechaLiquidacion.EditValue = _FechaServidor;
                comboTipoBaja.Properties.DataSource = db.TipoBaja;

                //ingresos
                BuildTablaIngreso();
                bdsIngresos.DataSource = ingresos;
                gridIngresos.DataSource = bdsIngresos;

                comboTipoIngreso.Properties.DataSource = db.TipoMovimientoEmpleado.Where(m => m.EsIngreso && m.Activo && !m.MostrarEnPlanilla);

                //deducciones
                BuildTablaDeduccion();
                bdsDeduccion.DataSource = deducciones;
                gridDeducciones.DataSource = bdsDeduccion;

                comboTipoDeduccion.Properties.DataSource = db.TipoMovimientoEmpleado.Where(m => !m.EsIngreso && m.Activo && !m.MostrarEnPlanilla);

                _MaximoSalarioINSS = Parametros.Config.MaximoSalarioINSS();
                _ValorINSSLaboral = Parametros.Config.ValorINSSLaboral();


                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region Metodos

        private void BuildTablaIngreso()
        {
            ingresos = new DataTable();
            ingresos.Columns.Add("ID", typeof(int));
            ingresos.Columns.Add("Descripcion", typeof(string));
            ingresos.Columns.Add("Monto", typeof(decimal));
            ingresos.Columns.Add("Grabable", typeof(bool));
            ingresos.Columns.Add("CantidadHoras", typeof(decimal));
        }
        private void BuildTablaDeduccion()
        {
            deducciones = new DataTable();
            deducciones.Columns.Add("ID", typeof(int));
            deducciones.Columns.Add("Descripcion", typeof(string));
            deducciones.Columns.Add("Monto", typeof(decimal));
        }

        private void CalcularIngresos()
        {
            try { IngresosGrabablesSpinEdit.EditValue = Decimal.Round(Convert.ToDecimal(ingresos.Compute("Sum(Monto)", "Grabable = 1")), 2); }  catch { IngresosGrabablesSpinEdit.EditValue = 0; }
            try { IngresosNoGrabablesSpinEdit.EditValue = Decimal.Round(Convert.ToDecimal(ingresos.Compute("Sum(Monto)", "Grabable = 0")), 2);} catch{IngresosNoGrabablesSpinEdit.EditValue =0;}

        }

        private void CalcularTotalIngresosAntesRetencion()
        {
            //DataTable dtsalarios = new DataTable();
            //dtsalarios = dataManager.GetListUltimos6Pagos(empleado.IdEmpleado, false, false);
            //if (ckSalarioVariable.Checked)
            //{
            //    try { SalarioBaseGrabable = Decimal.Round(Convert.ToDecimal(dtsalarios.Compute("AVG(TotalSalarioComisiones)", "")), 2); }
            //    catch { SalarioBaseGrabable = 0; }

            //    if (ckTodoMontoEnDolares.Checked)
            //    {
            //         SalarioBaseGrabable = Decimal.Round(SalarioBaseGrabable / liquidacion.TipoCambio);
            //    }
            //}
            //else
            //{
            //    if (empleado.Dolarizado) SalarioBaseGrabable = Decimal.Round(empleado.SalarioActual * liquidacion.TipoCambio);
            //    else SalarioBaseGrabable = Decimal.Round(empleado.SalarioActual);
            //}

            //// Calcular Indemnizacion
            //if (!ckNoPagarIndenmizacion.Checked)
            //{
            //    decimal diasIndem = 0, mesesIndem = 0, anyosIndem = 0;
            //    liquidacion.IndemnizacionGrabable = Decimal.Round(Classes.Util.CalcularIndemnizacion(SalarioBaseGrabable, liquidacion.FechaDesde, liquidacion.FechaFinal, out diasIndem, out mesesIndem, out anyosIndem), 2);
            //}
            //else  liquidacion.IndemnizacionGrabable= 0;
            
            ////Calcular 13voMes
            //liquidacion.AguinaldoGrabable = Classes.Util.Calcular13voMes(SalarioBaseGrabable, fechaUltimaAguinaldo.DateTime.Date.AddDays(1), hastaDateEdit.DateTime.Date);

            //SalarioDiarioGrabable = Decimal.Round(SalarioBaseGrabable / Classes.Global.diasMes, 2);
            //CantDiasTrabajoGrabable = Decimal.Round(diasSalarioSpinEdit.Value, 2);
            //CantDiasVacacionesGrabable = Decimal.Round(diasVacacionesSpinEdit.Value, 2);
            //MontoDiasTrabajoGrabable = Decimal.Round(diasSalarioSpinEdit.Value * SalarioDiarioGrabable, 2);
            //MontoDiasVacionesGrabable = Decimal.Round(diasVacacionesSpinEdit.Value * SalarioDiarioGrabable, 2);

            //TotalIngresosAntesRetencion = Decimal.Round(MontoDiasVacionesGrabable + MontoDiasTrabajoGrabable + IngresosGrabablesSpinEdit.Value, 2);

            //liquidacion.TotalIngresoGrabable = Decimal.Round(TotalIngresosAntesRetencion, 2);

            //if (ckTodoMontoEnDolares.Checked)
            //{
            //    MontoDiasTrabajoGrabable = Decimal.Round(MontoDiasTrabajoGrabable  / liquidacion.TipoCambio, 2);
            //    MontoDiasVacionesGrabable = Decimal.Round(MontoDiasVacionesGrabable / liquidacion.TipoCambio, 2);
            //    TotalIngresosAntesRetencion = Decimal.Round(TotalIngresosAntesRetencion / liquidacion.TipoCambio, 2);
            //    SalarioBaseGrabable = Decimal.Round(SalarioBaseGrabable / liquidacion.TipoCambio, 2);
            //    SalarioDiarioGrabable = Decimal.Round(SalarioDiarioGrabable / liquidacion.TipoCambio, 2);
            //    liquidacion.AguinaldoGrabable = Decimal.Round(liquidacion.AguinaldoGrabable / liquidacion.TipoCambio, 2);
            //    liquidacion.IndemnizacionGrabable = Decimal.Round(liquidacion.IndemnizacionGrabable / liquidacion.TipoCambio, 2);
            //    liquidacion.TotalIngresoGrabable = Decimal.Round(liquidacion.TotalIngresoGrabable / liquidacion.TipoCambio, 2);
            //}


        }

        private decimal SalarioPagado(int empleado, DateTime fecha, int IdPlanilla)
        {
            //DateTime date = fecha.AddDays(-1);
            //var planilla = db.PlanillaGenerada.Where(p => p.Planilla == IdPlanilla && (p.FechaDesde <= fecha && p.FechaHasta >= fecha));

            //if (planilla.Count() > 0)
            //{
            //    var lst = db.DetallePlanilla.Where(l => l.Empleado == empleado && l.PlanillaGenerada == planilla.First().ID);
            //    if (lst.Count() <= 0)
            //        return 0;
            //    return Convert.ToDecimal(lst.First().TotalIngresos);
            //}
            //else
                return 0;
        }

        private void SetLiquidacionValues()
        {
            EtLiquidacion.EmpleadoID = ETEmpleado.ID;
            EtLiquidacion.PlanillaID = ETEmpleado.PlanillaID;
            EtLiquidacion.Motivo = motivoTextEdit.Text;
            EtLiquidacion.SegundoMotivo = motivoTexEdit2.Text;
            EtLiquidacion.TipoBajaID = Convert.ToInt32(comboTipoBaja.EditValue);
            EtLiquidacion.FechaInicio = desdeDateEdit.DateTime.Date;
            EtLiquidacion.FechaFinal = hastaDateEdit.DateTime.Date;
            EtLiquidacion.SalarioBase = Decimal.Round(ingresoMensualSpinEdit.Value, 2);
            EtLiquidacion.SalarioDiario = Decimal.Round(EtLiquidacion.SalarioBase / Parametros.General.diasMes, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.FechaAguinaldo = fechaUltimaAguinaldo.DateTime.Date;
            EtLiquidacion.AguinaldoMonto = Decimal.Round(treceavoSpinEdit.Value, 2);
            EtLiquidacion.IndemnizacionMonto = Decimal.Round(indemnizacionSpinEdit.Value, 2);
            //EtLiquidacion.DiasIndemnizacion = Convert.ToInt32((liquidacion.FechaFinal - liquidacion.FechaDesde).TotalDays);


            //int days = 0, month = 0, year = 0;

            //Classes.Util.RangoFechasYMD(hastaDateEdit.DateTime.Date, fechaUltimaAguinaldo.DateTime.Date, out year, out month, out days);

            //decimal diasAguinaldo = Decimal.Round(year * Classes.Global.valorAnualVacaciones, 2);
            //diasAguinaldo += Decimal.Round(month * Classes.Global.valorMensualVacaciones, 2);
            //diasAguinaldo += Decimal.Round(days * Classes.Global.valorDiarioVacaciones, 2);

            //diasPendienteSpinEdit.EditValue = diasAguinaldo;
            //EtLiquidacion.AguinaldoDias
            //liquidacion.DiasAguinaldo = Decimal.Round(diasAguinaldo, 2);

            #region Vacaciones

            EtLiquidacion.FechaInicioVacaciones = fechaUltimaVacacionesDateEdit.DateTime.Date;
            if (EtLiquidacion.FechaInicioVacaciones < ETEmpleado.FechaContrato) EtLiquidacion.FechaInicioVacaciones = Convert.ToDateTime(ETEmpleado.FechaContrato);
            EtLiquidacion.FechaFinVaciones = hastaDateEdit.DateTime.Date;
            //Acumuladas
            EtLiquidacion.VacacionesAcumuladasDias = Decimal.Round(diasVacAcumuladoSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.VacacionesAcumuladasMonto = Decimal.Round(valorDiasAcumuladosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            //Descansadas
            EtLiquidacion.VacacionesDescansadosDias = Decimal.Round(diasDescansadosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.VacacionesDescansadasMonto = Decimal.Round(valorDiasDescansadosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            //Pendientes
            EtLiquidacion.VacacionesPendientesDias = Decimal.Round(diasPendienteSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.VacacionesPendientesMonto = Decimal.Round(valorDiasPendientesVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            //Vacaciones Pagadas
            EtLiquidacion.VacacionesDias = Decimal.Round(diasVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.VacacionesMonto = Decimal.Round(totalVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);


            #endregion

            EtLiquidacion.OtrosIngresos = Decimal.Round(Convert.ToDecimal(colMonto.SummaryItem.SummaryValue == null ? 0 : colMonto.SummaryItem.SummaryValue), 2);
            EtLiquidacion.OtrasDeducciones = Decimal.Round(Convert.ToDecimal(colMontoDed.SummaryItem.SummaryValue == null ? 0 : colMontoDed.SummaryItem.SummaryValue), 2);

            EtLiquidacion.DiasSalarioCantidad = Decimal.Round(diasSalarioSpinEdit.Value, 2);
            EtLiquidacion.DiasSalarioMonto = Decimal.Round(totalSalarioSpinEdit.Value, 2);

            //CalcularTotalIngresosAntesRetencion();

            //if (TotalIngresosAntesRetencion < 0) liquidacion.SeguroSocial = 0;
            //else 

            _TotalIngresosAntesRetencion = Decimal.Round(EtLiquidacion.VacacionesMonto + EtLiquidacion.DiasSalarioMonto + EtLiquidacion.OtrosIngresos, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.TotalIngresos = _TotalIngresosAntesRetencion;
            EtLiquidacion.TotalNeto = _TotalIngresosAntesRetencion + EtLiquidacion.IndemnizacionMonto + EtLiquidacion.AguinaldoMonto;
            EtLiquidacion.SeguroSocial = Decimal.Round(_TotalIngresosAntesRetencion * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.TotalDeducciones = Decimal.Round(EtLiquidacion.SeguroSocial + EtLiquidacion.IR + EtLiquidacion.OtrasDeducciones, 2, MidpointRounding.AwayFromZero);
            
            decimal ingresosConRetenciones = Decimal.Round((_TotalIngresosAntesRetencion - EtLiquidacion.SeguroSocial), 2, MidpointRounding.AwayFromZero);
            decimal totalIR = 0;

            #region <<< IR >>>
            //if (ingresosConRetenciones > 0)
            //{
                
            //    decimal vTotalIR = 0;
            //    //Valor para calcular el tope del salario maximo cotizable dividido en INSS Laboral
            //    decimal vtopeMensual = 0m;
            //    vtopeMensual = Decimal.Round(_MaximoSalarioINSS * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);

            //    //Mes que se genera la planilla
            //    int mesplanilla = Convert.ToInt32(EtLiquidacion.FechaFinal.Month);

            //    //Entidad de la tabla del IR acumulativo segun el trabajador asignado
            //    Entidad.IRRetenido tira;
            //    bool exist = false;

            //    //Obtener el IRRetenido acumulativo del empleado que se esta calculando la nomina
            //    var tiraGuest = db.IRRetenido.FirstOrDefault(t => t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.Month));

            //    if (tiraGuest != null)
            //    {
            //        tira = db.IRRetenido.Single(s => s.ID.Equals(tiraGuest.ID));
            //        //Si existe se obtienen los valores de la tabla del IR acumulativo
            //        exist = true;
            //        //else
            //        if (ETEmpleado.CalculoSalarioPromedioAguinaldo)
            //            tira.SueldoAcumulado =
            //                db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && !EtLiquidacion.FechaFinal.Month.Equals(1) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //                ? db.IRRetenido.Where(t => t.EmpleadoID == ETEmpleado.ID && !EtLiquidacion.FechaFinal.Month.Equals(1) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().SueldoAcumulado
            //                : 0m;
            //    }
            //    else
            //    {
            //        //Si no existe se crea una entidad nueva de TablaIRAcumulativo
            //        exist = false;
            //        tira = new Entidad.IRRetenido();
            //        tira.EmpleadoID = ETEmpleado.ID;
            //        tira.FechaIR = EtLiquidacion.FechaFinal.Date;
            //        tira.SalarioBasico = Decimal.Round(ETEmpleado.SalarioActual, 2, MidpointRounding.AwayFromZero);
            //        tira.MesesTranscurridos = mesplanilla;
            //        if (ETEmpleado.CalculoSalarioPromedioAguinaldo)
            //            tira.SueldoAcumulado = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && !EtLiquidacion.FechaFinal.Month.Equals(1) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //                ? db.IRRetenido.Where(t => t.EmpleadoID == ETEmpleado.ID && !EtLiquidacion.FechaFinal.Month.Equals(1) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().SueldoAcumulado
            //                : 0m;
            //    }

            //    //Valor para calcular el INSS total del empleado
            //    decimal vINSSTotalIR = 0m;

            //    //Si es el primer mes del calculo anual del IR y primera quincena ---- o el empleado no existe en la tabla de IR Retenidos
            //    if ((mesplanilla == 1 && (int)Convert.ToDateTime(EtLiquidacion.FechaFinal).Day < 15) || !exist)
            //    {

            //        tira.INSSLaboralVirtual = Decimal.Round(EtLiquidacion.SeguroSocial, 2, MidpointRounding.AwayFromZero);
            //        tira.SueldoAcumulado = Decimal.Round(ingresosConRetenciones, 2, MidpointRounding.AwayFromZero);

            //        /*
            //        //Obteniendo el INSS para la resta del total a recibie del calculo del IR
            //        decimal inssBasico = Decimal.Round(ETEmpleado.SalarioActual * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);//INSS proyectado Segunda Quincena
            //        vINSSTotalIR = ETEmpleado.SalarioActual + inssBasico;

            //        if (vINSSTotalIR > vtopeMensual)//si es mayor al Tope deduccion INSS, el valor sera igual al tope valor del INSS
            //            vINSSTotalIR = vtopeMensual;

            //        tira.OtrosIngresosVirtual = Decimal.Round(_IngresosGrabables - DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero);
            //        tira.INSSLaboralVirtual = Decimal.Round(vINSSTotalIR, 2, MidpointRounding.AwayFromZero);
            //        */
            //        //if (reg.EsSalarioPromedio)
            //        //    tira.SueldoAcumulado += Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/  - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
            //        //else
            //        //    tira.SueldoAcumulado = Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/  - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
             
            //    }
            //    else //Del segundo Mes del calculo del IR anual en adelante
            //    {
            //            var QuincenaPagada = (from dp in db.DetallePlanillaGenerada
            //                                  join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
            //                                  where dp.EmpleadoID.Equals(ETEmpleado.ID) && (pg.FechaDesde.Date <= EtLiquidacion.FechaFinal.Date && pg.FechaHasta.Date >= EtLiquidacion.FechaFinal.Date)
            //                                  select new { pg.FechaDesde, dp.TotalIngresos }).OrderByDescending(o => o.FechaDesde).FirstOrDefault();

            //            tira.INSSLaboralVirtual = Decimal.Round((((ETEmpleado.SalarioActual / 2m) + (QuincenaPagada != null ? QuincenaPagada.TotalIngresos : 0m) + tira.OtrosIngresos) * _ValorINSSLaboral) + EtLiquidacion.SeguroSocial, 2, MidpointRounding.AwayFromZero);
            //            //tira.INSSLaboralVirtual = Decimal.Round((((ETEmpleado.SalarioActual / 2m) + (QuincenaPagada != null ? QuincenaPagada.TotalIngresos : 0m) + tira.OtrosIngresos) * _ValorINSSLaboral) + EtLiquidacion.SeguroSocial, 2, MidpointRounding.AwayFromZero);
            //            //tira.SueldoAcumulado += Decimal.Round(Decimal.Round(ETEmpleado.SalarioActual / 2m, 2, MidpointRounding.AwayFromZero) + (QuincenaPagada != null ? QuincenaPagada.TotalIngresos : 0m) + tira.OtrosIngresos + ingresosConRetenciones - tira.INSSLaboralVirtual, 2, MidpointRounding.AwayFromZero);

            //            if (ETEmpleado.CalculoSalarioPromedioAguinaldo)
            //                tira.SueldoAcumulado += Decimal.Round(ingresosConRetenciones /*Ingresos Primera Quincena + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero) Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
            //            else
            //                tira.SueldoAcumulado = Decimal.Round(ingresosConRetenciones /*Ingresos Primera Quincena + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero) Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
                    

            //        //if ((int)Convert.ToDateTime(PG.FechaDesde).Day < 15)//Primera quincena del IR
            //        //{
            //        //    decimal inssBasico = Decimal.Round(DPG.SalarioPlanilla * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);//INSS proyectado Segunda Quincena
            //        //    vINSSTotalIR = DPG.INSSTotalMes + inssBasico;

            //        //    if (vINSSTotalIR > vtopeMensual)
            //        //        vINSSTotalIR = vtopeMensual;

            //        //    tira.OtrosIngresosVirtual = Decimal.Round(_IngresosGrabables - DPG.SalarioPlanilla, 2, MidpointRounding.AwayFromZero);
            //        //    tira.INSSLaboralVirtual = Decimal.Round(vINSSTotalIR, 2, MidpointRounding.AwayFromZero);

            //        //    if (reg.EsSalarioPromedio)
            //        //        tira.SueldoAcumulado += Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
            //        //    else
            //        //        tira.SueldoAcumulado = Decimal.Round(_IngresosGrabables /*Ingresos Primera Quincena*/ + Decimal.Round(DPG.SalarioBasico / 2m, 2, MidpointRounding.AwayFromZero)/*Proyeccion Salario Segunda Quincena*/ - vINSSTotalIR /*INSSProyectado*/ /* + (detallePlanilla.ValorSubsidio) Preguntar por el Subsidio*/, 2, MidpointRounding.AwayFromZero);
            //        }
            //        //else//Segunda quincena del IR
            //        //{
            //        //    //Se calcula el ingreso extra de la quincena
            //        //    tira.OtrosIngresosVirtual = Decimal.Round(tira.OtrosIngresos + _IngresosGrabables /*Ingresos Primera Quincena*/- DPG.SalarioPlanilla /*Se resta el salario recibido de la segunda quincena porque ya fue proyectado en la primera Quincena*/, 2, MidpointRounding.AwayFromZero);

            //        //    decimal DeduccionInssIngresosExtras = 0;
            //        //    decimal SalarioAnterior = SalarioPlanillaAnterior(DPG.EmpleadoID, PG.FechaDesde, PG.PlanillaID, db);

            //        //    decimal VariacionSalario = Decimal.Round(Convert.ToDecimal(DPG.SalarioPlanilla - SalarioAnterior), 2, MidpointRounding.AwayFromZero);

            //        //    if (tira.INSSLaboral > vtopeMensual)
            //        //    {
            //        //        if (reg.EsSalarioPromedio)
            //        //            tira.SueldoAcumulado += Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + VariacionSalario + tira.OtrosIngresosVirtual /*+ (detallePlanilla.ValorSubsidio)*/, 2, MidpointRounding.AwayFromZero);
            //        //        else
            //        //            tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + VariacionSalario + tira.OtrosIngresosVirtual /*+ (detallePlanilla.ValorSubsidio)*/, 2, MidpointRounding.AwayFromZero);
            //        //    }
            //        //    else
            //        //    {
            //        //        DeduccionInssIngresosExtras = Decimal.Round((tira.OtrosIngresosVirtual - tira.OtrosIngresos + (tira.INSSLaboral >= vtopeMensual ? 0m : VariacionSalario)) * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);

            //        //        decimal INSSIR = 0;

            //        //        if ((vtopeMensual - tira.INSSLaboral) < DeduccionInssIngresosExtras)
            //        //            INSSIR = vtopeMensual - tira.INSSLaboral;
            //        //        else
            //        //            INSSIR = DeduccionInssIngresosExtras + tira.INSSLaboral;

            //        //        tira.INSSLaboralVirtual = INSSIR;

            //        //        if (reg.EsSalarioPromedio)
            //        //            tira.SueldoAcumulado += Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ tira.OtrosIngresosVirtual - INSSIR, 2, MidpointRounding.AwayFromZero);
            //        //        else
            //        //        {
            //        //            tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ tira.OtrosIngresosVirtual - INSSIR, 2, MidpointRounding.AwayFromZero);
            //        //            //if (tira.OtrosIngresosVirtual > 0)
            //        //            //{
            //        //            //    DateTime vFecha = new DateTime(PG.FechaHasta.Year, FechaHasta.Month, 01);
            //        //            //    decimal vAcumulado = db.IRRetenido.Where(o => o.EmpleadoID.Equals(DPG.EmpleadoID) && o.FechaIR < vFecha).Sum(s => s.SueldoAcumulado);
            //        //            //    tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + vAcumulado + tira.OtrosIngresosVirtual -INSSIR, 2, MidpointRounding.AwayFromZero);
            //        //            //}
            //        //            //else
            //        //            //tira.SueldoAcumulado = Decimal.Round(SalarioAnterior + DPG.SalarioPlanilla + /*VariacionSalario + */ -INSSIR, 2, MidpointRounding.AwayFromZero);

            //        //        }
            //        //    }

            //        //}



            //    tira.MesesTranscurridos = (Convert.ToInt32(Convert.ToDateTime(EtLiquidacion.FechaFinal).Month).Equals(1) ? 1 :
            //                                  (db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().MesesTranscurridos + 1 : 1)
            //                                    );
            //    //en case que sea promedio se divide entre los meses transcurridos de lo contrario se divide entre 1
            //    tira.SueldoPromedioMensual = Decimal.Round(tira.SueldoAcumulado / (ETEmpleado.CalculoSalarioPromedioAguinaldo ? tira.MesesTranscurridos : 1), 2, MidpointRounding.AwayFromZero);

            //    tira.EspectativaAnual = Decimal.Round(tira.SueldoPromedioMensual * 12, 2, MidpointRounding.AwayFromZero);

            //    var rangoIR = db.TablaIR.SingleOrDefault(ir => tira.EspectativaAnual > ir.Desde && tira.EspectativaAnual <= ir.Hasta);

            //    if (rangoIR != null)
            //    {
            //        tira.ImpuestoBase = Decimal.Round(rangoIR.Base, 2, MidpointRounding.AwayFromZero);
            //        tira.PorcentajeAplicable = rangoIR.Tasa;
            //        tira.SobreExceso = rangoIR.Techo;
            //    }

            //    tira.Total = Decimal.Round(tira.ImpuestoBase + (tira.PorcentajeAplicable * (tira.EspectativaAnual - tira.SobreExceso)), 2, MidpointRounding.AwayFromZero);

            //    tira.IRAnualSobre12 = Decimal.Round(tira.Total / 12m, 2, MidpointRounding.AwayFromZero);

            //    if (ETEmpleado.CalculoSalarioPromedioAguinaldo)
            //    {
            //        tira.IRMesesTranscurridos = Decimal.Round(tira.IRAnualSobre12 * tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

            //        if (tira.MesesPorRetener.Equals(1))
            //            tira.RetencionAcumulada = 0m;
            //        else
            //        {
            //            decimal RetencionAcumuladaAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().RetencionAcumulada : 0m;
            //            decimal RetencionMesAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0 ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID == ETEmpleado.ID && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().RetencionMes : 0m;

            //            tira.RetencionAcumulada = RetencionAcumuladaAnterior + RetencionMesAnterior;
            //        }
            //        tira.RetencionMes = Decimal.Round(tira.IRMesesTranscurridos - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);
            //    }
            //    else
            //    {
            //        tira.RetencionMes = tira.IRAnualSobre12;
            //        //if (tira.OtrosIngresosVirtual > 0)
            //        //{
            //        //    tira.IRMesesTranscurridos = Decimal.Round(tira.IRAnualSobre12 * tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);
            //        //    tira.RetencionMes = Decimal.Round(tira.IRMesesTranscurridos - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);
            //        //}
            //        //else
            //        //    tira.RetencionMes = tira.IRAnualSobre12;
            //    }

            //    if ((int)Convert.ToDateTime(EtLiquidacion.FechaFinal).Day < 15)
            //    {
            //        tira.IRRetenidoI = Decimal.Round(tira.RetencionMes, 2, MidpointRounding.AwayFromZero);
            //        vTotalIR = (tira.IRRetenidoI < 0 ? 0m : tira.IRRetenidoI);

            //    }
            //    else
            //    {
            //        tira.IRRetenidoII = Decimal.Round(tira.RetencionMes - tira.IRRetenidoI, 2, MidpointRounding.AwayFromZero);
            //        vTotalIR = (tira.IRRetenidoII < 0 ? 0m : tira.IRRetenidoII);
            //    }


            //    if (!exist)
            //        db.IRRetenido.InsertOnSubmit(tira);

            //    db.SubmitChanges();

            //    //Guardar el IR del  Empleado
            //    EtLiquidacion.IR = vTotalIR;
                

            //    //#region <<< IR >>>
            //    //decimal vTotalIR = 0;
            //    ////Valor para calcular el tope del salario maximo cotizable dividido en INSS Laboral
            //    //decimal vtopeMensual = 0m;
            //    //vtopeMensual = Decimal.Round(_MaximoSalarioINSS * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);

            //    ////Mes que se genera la planilla
            //    //int mesplanilla = Convert.ToInt32(EtLiquidacion.FechaFinal.Month);

            //    ////Entidad de la tabla del IR acumulativo segun el trabajador asignado
            //    //Entidad.IRRetenido tira;
            //    //bool exist = false;

            //    ////Obtener el IRRetenido acumulativo del empleado que se esta calculando la nomina
            //    //var tiraGuest = db.IRRetenido.FirstOrDefault(t => t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.Month));

            //    //if (tiraGuest != null)
            //    //{
            //    //    tira = db.IRRetenido.Single(s => s.ID.Equals(tiraGuest.ID));
            //    //    //Si existe se obtienen los valores de la tabla del IR acumulativo
            //    //    exist = true;
            //    //    //else
            //    //    tira.SueldoAcumulado =
            //    //        db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //    //        ? db.IRRetenido.Where(t => t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().SueldoAcumulado
            //    //        : 0m;
            //    //}
            //    //else
            //    //{
            //    //    //Si no existe se crea una entidad nueva de TablaIRAcumulativo
            //    //    exist = false;
            //    //    tira = new Entidad.IRRetenido();
            //    //    tira.EmpleadoID = ETEmpleado.ID;
            //    //    tira.FechaIR = EtLiquidacion.FechaFinal.Date;
            //    //    tira.SalarioBasico = Decimal.Round(EtLiquidacion.SalarioBase, 2, MidpointRounding.AwayFromZero);
            //    //    tira.MesesTranscurridos = mesplanilla;
            //    //    tira.SueldoAcumulado = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //    //        ? db.IRRetenido.Where(t => t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().SueldoAcumulado
            //    //        : 0m;
            //    //}

            //    ////Valor para calcular el INSS total del empleado
            //    //decimal vINSSTotalIR = 0m;

            //    //if ((int)Convert.ToDateTime(Convert.ToDateTime(hastaDateEdit.EditValue)).Day < 15)
            //    //{
            //    //    tira.INSSLaboralVirtual = Decimal.Round(EtLiquidacion.SeguroSocial, 2, MidpointRounding.AwayFromZero);
            //    //    tira.SueldoAcumulado += Decimal.Round(ingresosConRetenciones, 2, MidpointRounding.AwayFromZero);
            //    //}
            //    //else
            //    //{
            //    //    var QuincenaPagada = (from dp in db.DetallePlanillaGenerada
            //    //                          join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
            //    //                          where dp.EmpleadoID.Equals(ETEmpleado.ID) && (pg.FechaDesde.Date <= EtLiquidacion.FechaFinal.Date && pg.FechaHasta.Date >= EtLiquidacion.FechaFinal.Date)
            //    //                          select new { pg.FechaDesde, dp.TotalIngresos }).OrderByDescending(o => o.FechaDesde).FirstOrDefault();

            //    //    tira.INSSLaboralVirtual = Decimal.Round((((ETEmpleado.SalarioActual / 2m) + (QuincenaPagada != null ? QuincenaPagada.TotalIngresos : 0m) + tira.OtrosIngresos) * _ValorINSSLaboral) + EtLiquidacion.SeguroSocial, 2, MidpointRounding.AwayFromZero);
            //    //    tira.SueldoAcumulado += Decimal.Round(Decimal.Round(ETEmpleado.SalarioActual / 2m, 2, MidpointRounding.AwayFromZero) + (QuincenaPagada != null ? QuincenaPagada.TotalIngresos : 0m) + tira.OtrosIngresos + ingresosConRetenciones - tira.INSSLaboralVirtual, 2, MidpointRounding.AwayFromZero);
            //    //}

            //    ////igual al mes de la planilla
            //    //tira.MesesTranscurridos = (Convert.ToInt32(Convert.ToDateTime(hastaDateEdit.EditValue).Month));

            //    //tira.SueldoPromedioMensual = Decimal.Round(tira.SueldoAcumulado / tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

            //    //tira.EspectativaAnual = Decimal.Round(tira.SueldoPromedioMensual * 12, 2, MidpointRounding.AwayFromZero);

            //    //tira.SueldoPromedioMensual = Decimal.Round(tira.SueldoAcumulado / tira.MesesTranscurridos, 2, MidpointRounding.AwayFromZero);

            //    //tira.EspectativaAnual = Decimal.Round(tira.SueldoPromedioMensual * 12, 2, MidpointRounding.AwayFromZero);

            //    //if (tira.EspectativaAnual > 100000)
            //    //{
            //    //    var rangoIR = db.TablaIR.Single(ir => tira.EspectativaAnual > ir.Desde && tira.EspectativaAnual <= ir.Hasta);

            //    //    tira.ImpuestoBase = Decimal.Round(rangoIR.Base, 2, MidpointRounding.AwayFromZero);
            //    //    tira.PorcentajeAplicable = rangoIR.Tasa;
            //    //    tira.SobreExceso = rangoIR.Techo;
            //    //    tira.Total = Decimal.Round(tira.ImpuestoBase + (tira.PorcentajeAplicable * (tira.EspectativaAnual - tira.SobreExceso)), 2, MidpointRounding.AwayFromZero);

            //    //    decimal RetencionAcumuladaAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //    //        ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().RetencionAcumulada
            //    //        : 0m;
            //    //    decimal RetencionMesAnterior = db.IRRetenido.Count(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)) > 0
            //    //        ? db.IRRetenido.Where(t => t.ID != tira.ID && t.EmpleadoID.Equals(ETEmpleado.ID) && (t.FechaIR.Year == EtLiquidacion.FechaFinal.AddMonths(-1).Year && t.FechaIR.Month == EtLiquidacion.FechaFinal.AddMonths(-1).Month)).First().RetencionMes
            //    //        : 0m;

            //    //    tira.RetencionAcumulada = Decimal.Round(RetencionAcumuladaAnterior + RetencionMesAnterior, 2, MidpointRounding.AwayFromZero);
                    
            //    //    tira.SaldoPorRetener = Decimal.Round(tira.Total - tira.RetencionAcumulada, 2, MidpointRounding.AwayFromZero);

            //    //    tira.MesesPorRetener = Convert.ToInt32(13 - mesplanilla);
            //    //    tira.RetencionMes = Decimal.Round(tira.SaldoPorRetener / tira.MesesPorRetener, 2, MidpointRounding.AwayFromZero);
                    
            //    //    if ((int)Convert.ToDateTime(EtLiquidacion.FechaFinal).Day < 15)
            //    //    {
            //    //        tira.IRRetenidoI = Decimal.Round(tira.RetencionMes / 2, 2, MidpointRounding.AwayFromZero);
            //    //        vTotalIR = tira.IRRetenidoI;                        
            //    //    }
            //    //    else
            //    //    {
            //    //        tira.IRRetenidoII = Decimal.Round(tira.RetencionMes - tira.IRRetenidoI, 2, MidpointRounding.AwayFromZero);
            //    //        vTotalIR = tira.IRRetenidoII;                     
            //    //    }
            //    //}

            //    //if (!exist)
            //    //    db.IRRetenido.InsertOnSubmit(tira);

            //    //db.SubmitChanges();

            //    ////Guardar el IR del  Empleado
            //    //EtLiquidacion.IR = vTotalIR;
            //    //#endregion
                
            //}
            //else
            //    EtLiquidacion.IR = 0m;

            #endregion

                EtLiquidacion.IR = spIR.Value;

            int years = 0, months = 0, daysL = 0;

            Parametros.General.RangoFechasYMD(EtLiquidacion.FechaFinal, EtLiquidacion.FechaInicio, out years, out months, out daysL);
            EtLiquidacion.AnyosLaborados = years;
            EtLiquidacion.MesesLaborados = months;
            EtLiquidacion.DiasLaborados = daysL;

            EtLiquidacion.PagoNeto = Decimal.Round((EtLiquidacion.DiasSalarioMonto + EtLiquidacion.VacacionesMonto + EtLiquidacion.OtrosIngresos - EtLiquidacion.SeguroSocial - EtLiquidacion.IR - EtLiquidacion.OtrasDeducciones + EtLiquidacion.IndemnizacionMonto + EtLiquidacion.AguinaldoMonto), 2, MidpointRounding.AwayFromZero);

            EtLiquidacion.INATEC = Decimal.Round(_TotalIngresosAntesRetencion * Parametros.Config.ValorINATEC(), 2, MidpointRounding.AwayFromZero);
            EtLiquidacion.Patronal = Decimal.Round(_TotalIngresosAntesRetencion * Parametros.Config.ValorINSSPatronal(), 2, MidpointRounding.AwayFromZero);

            EtLiquidacion.CargoID = Convert.ToInt32(ETEmpleado.CargoID);
            EtLiquidacion.AreaID = Convert.ToInt32(ETEmpleado.AreaNominaID);

            //EtLiquidacion.SalarioBaseGrabable = Decimal.Round(SalarioBaseGrabable, 2);
            //liquidacion.Dolarizado = ckTodoMontoEnDolares.Checked;
            //liquidacion.FechaINSS = empleado.FechaContrato;
            


        }

        private void ShowResumen()
        {
            lblNombreEmpleado.Text = "Empleado: " + ETEmpleado.Nombres + " " + ETEmpleado.Apellidos + " | Código: " + ETEmpleado.Codigo;
            lblRangoFecha.Text = "Fecha Ingreso: " + EtLiquidacion.FechaInicio.ToShortDateString() + "  Fecha Egreso: " + EtLiquidacion.FechaFinal.ToShortDateString();
            lblSalarioEmpleado.Text = "Salario Promedio: " + EtLiquidacion.SalarioBase.ToString("#,###,##0.00") + " | Salario Diario: " + Decimal.Round((EtLiquidacion.SalarioDiario), 2, MidpointRounding.AwayFromZero).ToString("#,###,##0.00");
            lblBaja.Text = "Motivo Baja: " + motivoTextEdit.Text + " | Tipo: " + comboTipoBaja.Text;


            lblTiempoLaborado.Text = "Tiempo laborado: | Años  " + EtLiquidacion.AnyosLaborados.ToString() + " | Meses  " + EtLiquidacion.MesesLaborados.ToString() + "  | Dias  " + EtLiquidacion.DiasLaborados.ToString();

            lvDescripcion.Clear();
            lvMontos.Clear();

            lvDescripcion.Items.Add("Dias Trabajados: " + EtLiquidacion.DiasSalarioCantidad.ToString());
            lvMontos.Items.Add(EtLiquidacion.DiasSalarioMonto.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("Vacaciones: " + EtLiquidacion.VacacionesDias.ToString());
            lvMontos.Items.Add(EtLiquidacion.VacacionesMonto.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("Otros Ingresos: ");
            lvMontos.Items.Add(EtLiquidacion.OtrosIngresos.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("");
            lvMontos.Items.Add("______________________________________________");

            lvDescripcion.Items.Add("Total Ingresos: ");
            lvMontos.Items.Add((EtLiquidacion.DiasSalarioMonto + EtLiquidacion.VacacionesMonto + EtLiquidacion.OtrosIngresos).ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("INSS (-): ");
            lvMontos.Items.Add(EtLiquidacion.SeguroSocial.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("IR (-): ");
            lvMontos.Items.Add(EtLiquidacion.IR.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("Otras Deducciones: ");
            lvMontos.Items.Add(EtLiquidacion.OtrasDeducciones.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("");
            lvMontos.Items.Add("______________________________________________");

            lvDescripcion.Items.Add("Aguinaldo: ");
            lvMontos.Items.Add(EtLiquidacion.AguinaldoMonto.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("Indemnización: ");
            lvMontos.Items.Add(EtLiquidacion.IndemnizacionMonto.ToString("#,###,##0.00"));

            lvDescripcion.Items.Add("");
            lvMontos.Items.Add("______________________________________________");

            lvDescripcion.Items.Add("Total a Pagar: ");
            lvMontos.Items.Add(EtLiquidacion.PagoNeto.ToString("#,###,##0.00"));

        }

        private void ShowReportLiquidacion(int ID)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaLiquidacions.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().EmpleadoNombres + " " + VM.First().EmpleadoApellidos;

                Reportes.Nomina.Hojas.LiquidacionZ rep = new Reportes.Nomina.Hojas.LiquidacionZ();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.pbLogo.Image = picture_LogoEmpresa;

                if (VM.FirstOrDefault().CalculoSalarioPromedioAguinaldo)
                {
                    rep.xrCellSalario.Text = "Salario Promedio:";
                }
                else
                {
                    rep.xrCellSalario.Text = "Salario Básico:";
                }

                int ElaboradoEmpleadoID = Convert.ToInt32(db.Configuracions.Where(c => c.Campo.Equals("ElaboradoLiquidacionEmpleadoID")).FirstOrDefault().Valor);

                var nombre = (from e in db.Empleados
                              join c in db.Cargo on e.CargoID equals c.ID
                              where e.ID.Equals(ElaboradoEmpleadoID)
                              select new { texto = e.Nombres + " " + e.Apellidos, cargo = c.Nombre }).ToList();

                if (nombre.Count > 0)
                    rep.xrElaboradoNombre.Text = nombre.FirstOrDefault().texto + Environment.NewLine + nombre.FirstOrDefault().cargo;
                

                rep.DataSource = VM;


                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument(true);
                rv.Show();


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        

        //private AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable DatosVacaciones(int liquidacion)
        //{
        //    AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable data = new AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable();
        //    var row = data.NewCalculoVacacionesRow();

        //    var liq = db.Liquidacion.Single(l => l.ID == liquidacion);
        //    DateTime fechaInicio = liq.FechaDesde, fechaFin = liq.FechaFinVaciones;

        //    int intervaloMeses = (int)Classes.DateAndTime.DateDiff(AdmonP.Classes.DateInterval.Month, fechaInicio, fechaFin);
        //    decimal acum = 0, diasPag = liq.DiasVacaciones, acumPag = 0;
        //    for (int i = -6; i < 0; i++)
        //    {
        //        decimal valor = 0, valorPag = 0;
        //        string nomMes = "";
        //        DateTime fecha = new DateTime(fechaFin.AddMonths(i).Year, fechaFin.AddMonths(i).Month, 1);
        //        var vacacionesPagadas = db.Get_PagoVacacionesByEmpleado(liq.IdEmpleado, fecha, fecha.AddMonths(1).AddDays(-1), true);
        //        if (vacacionesPagadas.Count() > 0)
        //        {
        //            var dt = Classes.Util.LINQToDataTable(vacacionesPagadas.AsEnumerable());
        //            valorPag = Decimal.Round(Convert.ToDecimal(dt.Rows[0]["DiasDescansados"]), 2);
        //        }
        //        nomMes = fecha.ToString("MMM-yy");
        //        if (fecha > fechaInicio)
        //        {
        //            if ((diasPag - acum) > 2.5m)
        //            {
        //                valor = 2.5m;
        //            }
        //            else
        //                valor = Decimal.Round(diasPag - acum);
        //        }
        //        // Esto no me Gusta, se debe poder mejorar, El tiempo no me dio para mejorarlo | AL 27/05/2011
        //        if (i == -6)
        //        {
        //            row.Mes1AN = nomMes;
        //            row.Mes1DN = nomMes;
        //            row.Mes1AV = Decimal.Round(valor, 2);
        //            row.Mes1DV = Decimal.Round(valorPag);
        //        }
        //        if (i == -5)
        //        {
        //            row.Mes2AN = nomMes;
        //            row.Mes2DN = nomMes;
        //            row.Mes2AV = Decimal.Round(valor, 2);
        //            row.Mes2DV = Decimal.Round(valorPag, 2);
        //        }
        //        if (i == -4)
        //        {
        //            row.Mes3AN = nomMes;
        //            row.Mes3DN = nomMes;
        //            row.Mes3AV = Decimal.Round(valor, 2);
        //            row.Mes3DV = Decimal.Round(valorPag, 2);
        //        }
        //        if (i == -3)
        //        {
        //            row.Mes4AN = nomMes;
        //            row.Mes4DN = nomMes;
        //            row.Mes4AV = Decimal.Round(valor, 2);
        //            row.Mes4DV = Decimal.Round(valorPag, 2);
        //        }
        //        if (i == -2)
        //        {
        //            row.Mes5AN = nomMes;
        //            row.Mes5DN = nomMes;
        //            row.Mes5AV = Decimal.Round(valor, 2);
        //            row.Mes5DV = Decimal.Round(valorPag, 2);
        //        }
        //        if (i == -1)
        //        {
        //            row.Mes6AN = nomMes;
        //            row.Mes6DN = nomMes;
        //            row.Mes6AV = Decimal.Round(valor, 2);
        //            row.Mes6DV = Decimal.Round(valorPag, 2);
        //        }
        //        acum += Decimal.Round(valor, 2);
        //        acumPag += Decimal.Round(valorPag, 2);
        //    }
        //    row.SaldoAcum = Decimal.Round(diasPag - (acum - acumPag), 2);
        //    row.Saldo = Decimal.Round(diasPag, 2);
        //    data.AddCalculoVacacionesRow(row);
        //    return data;
        //}

        #endregion

        #region Eventos


        #region Aquinaldo e Indemnizacion

        private void Calcular13voIndemnizacion(object sender, EventArgs e)
        {
            try
            {
                //if (diasTrans > 30) //SI EL TRABAJADOR SUPERA LOS 30 DIAS ENTONCES SI PAGAR PRESTACIONES
                //{
                    if ((salarios != null ? salarios.Rows.Count : 0) > 0 && ckSalarioVariable.Checked) // Si existen pagos en los ultimos 6 meses y si es salario variable
                    {
                        //asignar el salario de base de calculo
                        //cuando se manda a llenar al grid de salarios ya viene en dolares o en cordobas segun lo solicitado
                        //liquidacion.SalarioBase = Math.Round(Convert.ToDecimal(salarios.Compute("AVG(TotalSalarioComisiones)", "")), 2);
                    }
                    else
                    {
                        ckSalarioVariable.Checked = false; // Control del estado de la caja de salario variable
                        //inicializando el salario
                        EtLiquidacion.SalarioBase = Decimal.Round(_SalarioActualEmpleado, 2, MidpointRounding.AwayFromZero);

                    }

                    ingresoMensualSpinEdit.Value = Decimal.Round(EtLiquidacion.SalarioBase, 2, MidpointRounding.AwayFromZero);  // Mostrar la base de calculo

                    //Asignar Fechas a la Liquidacion
                    EtLiquidacion.FechaInicio = Convert.ToDateTime(desdeDateEdit.EditValue).Date;
                    EtLiquidacion.FechaFinal = Convert.ToDateTime(hastaDateEdit.EditValue).Date;
                    EtLiquidacion.FechaAguinaldo = Convert.ToDateTime(fechaUltimaAguinaldo.EditValue).Date;

                    // Calcular Indemnizacion

                    if (diasTrans > 30)
                    {
                        if (!ckNoPagarIndenmizacion.Checked)
                        {
                            decimal diasIndem = 0, mesesIndem = 0, anyosIndem = 0, diasTotales;
                            EtLiquidacion.IndemnizacionMonto = Decimal.Round(Parametros.General.CalcularIndemnizacion(ingresoMensualSpinEdit.Value, EtLiquidacion.FechaInicio, EtLiquidacion.FechaFinal, out diasIndem, out mesesIndem, out anyosIndem), 2, MidpointRounding.AwayFromZero);
                            EtLiquidacion.IndemnizacionDiasCantidad = Decimal.Round(diasIndem, 2);
                            EtLiquidacion.IndemnizacionMesesCantidad = Decimal.Round(mesesIndem, 2);
                            EtLiquidacion.IndemnizacionAnyosCantidad = Decimal.Round(anyosIndem, 2);
                            EtLiquidacion.IndemnizacionDias = Decimal.Round(anyosIndem + mesesIndem + diasIndem, 2, MidpointRounding.AwayFromZero);

                        }
                        else
                            EtLiquidacion.IndemnizacionMonto = EtLiquidacion.IndemnizacionDiasCantidad = EtLiquidacion.IndemnizacionMesesCantidad = EtLiquidacion.IndemnizacionAnyosCantidad = EtLiquidacion.IndemnizacionDias = 0m;
                    }
                    else
                        EtLiquidacion.IndemnizacionMonto = EtLiquidacion.IndemnizacionDiasCantidad = EtLiquidacion.IndemnizacionMesesCantidad = EtLiquidacion.IndemnizacionAnyosCantidad = EtLiquidacion.IndemnizacionDias = 0m;

                    //Calcular 13voMes
                    int days = 0, month = 0, year = 0;

                    Parametros.General.RangoFechasYMD(hastaDateEdit.DateTime.Date, fechaUltimaAguinaldo.DateTime.Date, out year, out month, out days);

                    decimal diasAguinaldo = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2);
                    diasAguinaldo += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2);
                    diasAguinaldo += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2);

                    EtLiquidacion.AguinaldoDias = (diasTrans > 30 ? diasAguinaldo : 0m);

                    if (ETEmpleado.CalculoSalarioPromedioAguinaldo)
                    {

                        List<Parametros.ListDateSalarioValue> nominas = (from dp in db.DetallePlanillaGenerada
                                                                         join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
                                                                         where dp.EmpleadoID.Equals(ETEmpleado.ID) && pg.Aprobado
                                                                         select new Parametros.ListDateSalarioValue
                                                                         {
                                                                             Fecha = pg.FechaDesde,
                                                                             Salario = dp.SalarioPlanilla,
                                                                             Total = dp.TotalIngresos
                                                                         }).ToList();

                        nominas.AddRange(
                            (
                            from a in db.SalarioAcumuladoAdmonPs
                            where a.EmpleadoID.Equals(ETEmpleado.ID)
                            select new Parametros.ListDateSalarioValue
                            {
                                Fecha = a.FechaDesde,
                                Salario = 0,
                                Total = a.TotalIngresos
                            }
                            ).ToList()
                            );

                        var sm = from o in nominas
                                 where (o.Fecha.Date >= Convert.ToDateTime(ETEmpleado.FechaUltimoAguinaldo).Date
                                 && (o.Fecha.Date >= _FechaServidor.AddMonths(-6).Date && o.Fecha.Date <= _FechaServidor.Date))
                                 group o by new { o.Fecha.Month } into gr
                                 select new { Ingresos = gr.Sum(s => s.Total) };

                        //var sm = from dp in db.DetallePlanillaGenerada
                        //         join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
                        //         where dp.EmpleadoID.Equals(ETEmpleado.ID) && (pg.FechaDesde.Date >= Convert.ToDateTime(ETEmpleado.FechaUltimoAguinaldo).Date
                        //         && (pg.FechaDesde.Date >= _FechaServidor.AddMonths(-6).Date && pg.FechaDesde <= _FechaServidor.Date))
                        //         group dp by new {pg.FechaDesde.Month} into gr
                        //         select new {Ingresos = gr.Sum(s => s.TotalIngresos)};

                        lblSM.Visible = true;
                        speSM.Visible = true;
                        speSM.Value = (sm.Count() > 0 ? Decimal.Round(sm.Max(m => m.Ingresos), 2, MidpointRounding.AwayFromZero) : 0m);

                        if (speSM.Value < ingresoMensualSpinEdit.Value)
                            speSM.Value = ingresoMensualSpinEdit.Value;
                    }

                if (diasTrans > 30)
                    EtLiquidacion.AguinaldoMonto = speSM.Value > 0 ? Decimal.Round((speSM.Value / 30) * diasAguinaldo, 2, MidpointRounding.AwayFromZero) : Decimal.Round((ingresoMensualSpinEdit.Value / 30) * diasAguinaldo, 2, MidpointRounding.AwayFromZero);
                else
                    EtLiquidacion.AguinaldoMonto = 0m;

                    //// Mostrar los Calculos
                if (diasTrans > 30)
                {
                    indemnizacionSpinEdit.EditValue = Math.Round(EtLiquidacion.IndemnizacionMonto, 2, MidpointRounding.AwayFromZero);
                    treceavoSpinEdit.EditValue = Math.Round(EtLiquidacion.AguinaldoMonto, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    indemnizacionSpinEdit.EditValue = 0m;
                    treceavoSpinEdit.EditValue = 0m;
                }
                    ////totalTreceavoIndemSpinEdit.EditValue = Math.Round(liquidacion.Indemnizacion + liquidacion.Aguinaldo, 2);

                    ////Mostrar los Dias de Calculo
                    lblCantidadDiasTreceavo.Text = "Cantidad de dias = " + EtLiquidacion.AguinaldoDias.ToString("#,#.00");
                    lblCantidadDiasIndemnizacion.Text = "Cantidad de dias = " + EtLiquidacion.IndemnizacionDias.ToString("#,#.00");
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void CalcularSalario(object sender, EventArgs e)
        {
            try
            {
                //salarioDiarioSpinEdit.EditValue = Decimal.Round(salarioBasicoSpinEdit.Value / Classes.Global.diasMes, 2);
                //totalSalarioSpinEdit.EditValue = Math.Round(salarioDiarioSpinEdit.Value * diasSalarioSpinEdit.Value, 2);
            }
            catch
            { }
        }

        private void ckSalarioVariable_CheckedChanged(object sender, EventArgs e)
        {          
            //if (salarios.Rows.Count > 0)
            //{
            //    if (ckSalarioVariable.Checked)
            //    {
            //        liquidacion.SalarioBase = Math.Round(Convert.ToDecimal(salarios.Compute("AVG(TotalSalarioComisiones)", "")), 2);
            //        ingresoMensualSpinEdit.EditValue = Math.Round(liquidacion.SalarioBase, 2);
            //    }
            //}
            //else
            //{
            //    if (!ckConsolidarSalarios.Checked)
            //    Classes.Global.DialogMsg("El Colaborador no ha recibido ningun pago por planilla, por favor deshabilite esta opción.", AdmonP.Classes.MsgType.warning, "");
            //}
        }

        private void ckNoPagarIndenmizacion_CheckedChanged(object sender, EventArgs e)
        {
            if (ckNoPagarIndenmizacion.Checked)
            {
                indemnizacionSpinEdit.EditValue = 0;
            }
        }

        private void diasSalarioSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                totalSalarioSpinEdit.EditValue = Math.Round((salarioBasicoSpinEdit.Value / Parametros.General.diasMes) * diasSalarioSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            }
            catch { }
        }

        #endregion

        #region Calculo de Pago de Vacaciones

        private void fechaUltimaVacacionesDateEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (diasTrans > 30)
            {
                int days = 0, month = 0, year = 0;

                Parametros.General.RangoFechasYMD(hastaDateEdit.DateTime.Date, fechaUltimaVacacionesDateEdit.DateTime.Date, out year, out month, out days);

                decimal diasVacaciones = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2, MidpointRounding.AwayFromZero);
                diasVacaciones += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2, MidpointRounding.AwayFromZero);
                diasVacaciones += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2, MidpointRounding.AwayFromZero);

                diasPendienteSpinEdit.EditValue = Decimal.Round(diasVacaciones, 2);
            }
         }

        private void diasVacAcumuladoSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (diasTrans > 30)
            {
                decimal days = Decimal.Round(diasVacAcumuladoSpinEdit.Value, 2);
                valorDiasAcumuladosSpinEdit.EditValue = Decimal.Round(Decimal.Round((EtLiquidacion.SalarioBase / 30), 2, MidpointRounding.AwayFromZero) * days, 2, MidpointRounding.AwayFromZero);
                diasVacacionesSpinEdit.EditValue = Math.Round(diasVacAcumuladoSpinEdit.Value + diasPendienteSpinEdit.Value - diasDescansadosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);

            }
        }

        private void diasDescansadosSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (diasTrans > 30)
            {
                decimal days = diasDescansadosSpinEdit.Value;
                valorDiasDescansadosSpinEdit.EditValue = Decimal.Round(Decimal.Round((EtLiquidacion.SalarioBase / 30), 2, MidpointRounding.AwayFromZero) * days, 2, MidpointRounding.AwayFromZero);
                diasVacacionesSpinEdit.EditValue = Decimal.Round(diasVacAcumuladoSpinEdit.Value + diasPendienteSpinEdit.Value - diasDescansadosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            }
        }

        private void diasPendienteSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (diasTrans > 30)
            {

                decimal days = diasPendienteSpinEdit.Value;
                valorDiasPendientesVacacionesSpinEdit.EditValue = Decimal.Round(Decimal.Round((EtLiquidacion.SalarioBase / 30), 2, MidpointRounding.AwayFromZero) * days, 2, MidpointRounding.AwayFromZero);
                diasVacacionesSpinEdit.EditValue = Math.Round(diasVacAcumuladoSpinEdit.Value + diasPendienteSpinEdit.Value - diasDescansadosSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            }
        }

        private void diasVacacionesSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (diasTrans > 30)
            {
                totalVacacionesSpinEdit.EditValue = Decimal.Round((EtLiquidacion.SalarioBase / Parametros.General.diasMes) * diasVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);
            }
        }

        #endregion

        #region Seleccionar | Agregar | Quitar - Ingresos, Egresos


        private void gvFormIngresos_DoubleClick(object sender, EventArgs e)
        {
            if (gvFormIngresos.FocusedRowHandle >= 0)
            {
                bdsIngresos.RemoveCurrent();
            }
        }
               
        private void btnAgregarIngreso_Click(object sender, EventArgs e)
        {
            if (comboTipoIngreso.EditValue == null)
            {
                Parametros.General.DialogMsg("Por favor Seleccione el tipo de ingreso", Parametros.MsgType.warning);
                return;
            }
            if (tipoIngresoSpinEdit.Value <= 0)
            {
                Parametros.General.DialogMsg("Por favor ingrese el monto del ingreso", Parametros.MsgType.warning);
                return;
            }
            DataRow dr = ingresos.NewRow();
            dr["ID"] = comboTipoIngreso.EditValue;
            dr["Descripcion"] = comboTipoIngreso.Text;
            dr["Monto"] = tipoIngresoSpinEdit.Value;
            dr["Grabable"] = db.TipoMovimientoEmpleado.Single(m => m.ID == Convert.ToInt32(comboTipoIngreso.EditValue)).AplicaRetencion;
            dr["CantidadHoras"] = spCantidadHoras.EditValue;
            ingresos.Rows.Add(dr);
            gridIngresos.Refresh();
            gvFormIngresos.RefreshData();
            CalcularIngresos();


        }
        
        private void gridIngresos_EmbeddedNavigator_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (gvFormIngresos.FocusedRowHandle >= 0)
            {
                bdsIngresos.RemoveCurrent();
                CalcularIngresos();
            }
        }

        private void gridDeducciones_EmbeddedNavigator_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (gvFormDeducciones.FocusedRowHandle >= 0)
            {
                bdsDeduccion.RemoveCurrent();
            }
        }

        private void btnAgregarDeduccion_Click(object sender, EventArgs e)
        {
            if (comboTipoDeduccion.EditValue == null)
            {
                Parametros.General.DialogMsg("Por favor Seleccione el tipo de Deducción", Parametros.MsgType.warning);
                return;
            }
            if (tipoDeduccionSpinEdit.Value <= 0)
            {
                Parametros.General.DialogMsg("Por favor ingrese el monto de la Deducción", Parametros.MsgType.warning);
                return;
            }
            DataRow dr = deducciones.NewRow();
            dr["ID"] = comboTipoDeduccion.EditValue;
            dr["Descripcion"] = comboTipoDeduccion.Text;
            dr["Monto"] = tipoDeduccionSpinEdit.Value;
            deducciones.Rows.Add(dr);
            gridDeducciones.Refresh();
            gvFormDeducciones.RefreshData();
        }

        #endregion

        #region Wizard

        #region Navigation

        private void wizard_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {

            if (e.Page == wpEmpleado)
            {
                if (gvEmpleados.FocusedRowHandle >= 0)
                {
                    try
                    {
                        ETEmpleado = db.Empleados.Single(em => em.ID.Equals(Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue("ID"))));
                        _SalarioActualEmpleado = Decimal.Round(ETEmpleado.SalarioActual, 2, MidpointRounding.AwayFromZero);
                        datos = "Empleado: " + ETEmpleado.Nombres + " " + ETEmpleado.Apellidos + " | Salario: " + _SalarioActualEmpleado.ToString("#,###,##0.00");

                        EtLiquidacion.OtrosIngresos = 0;
                        EtLiquidacion.OtrasDeducciones = 0;

                        #region Inicializar la informacion para el calculo de la liquidacion

                        desdeDateEdit.EditValue = ETEmpleado.FechaContrato; // Fecha de Ingreso del empleado
                        hastaDateEdit.EditValue = _FechaServidor; // Fecha Actual
                        diasTrans = Decimal.Round(Convert.ToDecimal((_FechaServidor.Date - Convert.ToDateTime(desdeDateEdit.EditValue)).TotalDays), 2, MidpointRounding.AwayFromZero);
                        fechaUltimaAguinaldo.EditValue = ETEmpleado.FechaUltimoAguinaldo;
                        List<Parametros.ListDateSalarioValue> nominas = (from dp in db.DetallePlanillaGenerada
                                      join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
                                      where dp.EmpleadoID.Equals(ETEmpleado.ID) && pg.Aprobado
                                      select new Parametros.ListDateSalarioValue
                                      {
                                         Fecha = pg.FechaDesde,
                                          Salario = dp.SalarioPlanilla,
                                          Total = dp.TotalIngresos
                                      }).ToList();

                        nominas.AddRange(
                            (
                            from a in db.SalarioAcumuladoAdmonPs
                            where a.EmpleadoID.Equals(ETEmpleado.ID)
                            select new Parametros.ListDateSalarioValue
                            {
                            Fecha = a.FechaDesde,
                            Salario = 0,
                            Total = a.TotalIngresos
                            }
                            ).ToList()
                            );
                        //salarios = dataManager.GetListUltimos6Pagos(empleado.IdEmpleado, ckTodoMontoEnDolares.Checked, ckConsolidarSalarios.Checked); // Mostrar el salario devengado de los ultimos 6 meses
                        gridSalarios.DataSource = nominas.OrderByDescending(o => o.Fecha).Take(12); //Asignar
                        gvSalarios.RefreshData();
                        Calcular13voIndemnizacion(sender, null);

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                        e.Handled = true;
                        return;
                    }
                }
                else
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.Page == wpAntiguedad)
            {
                lblVacaciones.Text = lblOtrosIngresos.Text = lblOtrasDeducciones.Text = lblDiasPendienteSalario.Text = "Empleado: " + ETEmpleado.Nombres + " " + ETEmpleado.Apellidos + " | Salario: " + EtLiquidacion.SalarioBase.ToString("#,#.00") + " | Salario Diario: " + Decimal.Round((EtLiquidacion.SalarioBase / 30), 2, MidpointRounding.AwayFromZero).ToString("#,#.00");
               
                if (String.IsNullOrEmpty(motivoTextEdit.Text))
                {
                    Parametros.General.DialogMsg("Por favor ingrese el motivo de baja del empleado", Parametros.MsgType.warning);
                    e.Handled = true;
                    return;
                }
                if (comboTipoBaja.EditValue == null)
                {
                    Parametros.General.DialogMsg("Por favor seleccione el tipo de baja del empleado", Parametros.MsgType.warning);
                    e.Handled = true;
                    return;
                }
                
                //vacaciones = dataManager.GetHistorialVacaciones(empleado.IdEmpleado);
                fechaFinalVacaciones.EditValue = hastaDateEdit.EditValue;
                fechaUltimaVacacionesDateEdit.EditValue = (ETEmpleado.FechaUltimaVacacion.HasValue ? Convert.ToDateTime(ETEmpleado.FechaUltimaVacacion).Date : Convert.ToDateTime(ETEmpleado.FechaContrato).Date);
                fechaUltimaVacacionesDateEdit_EditValueChanged(null, null);
                //gridVacaciones.DataSource = vacaciones;
                //if (vacaciones.Rows.Count > 0)
                //{
                //    diasDescansadosSpinEdit.EditValue = Decimal.Round(Convert.ToDecimal(vacaciones.Compute("Sum(CantidadDias)", "")), 2);

                //    gvVacaciones.ExpandAllGroups();
                //}
                _SalarioActualEmpleado = Decimal.Round(EtLiquidacion.SalarioBase, 2);
            }
        }

        private void wizard_SelectedPageChanged(object sender, DevExpress.XtraWizard.WizardPageChangedEventArgs e)
        {
            if (e.Direction == DevExpress.XtraWizard.Direction.Forward)
            {

                if (e.PrevPage == wpVacaciones)
                {

                }
                if (e.Page == wpSalario)
                {
                    salarioBasicoSpinEdit.EditValue = Decimal.Round(_SalarioActualEmpleado, 2);
                    var fch = (from p in db.PlanillaGenerada
                               join dp in db.DetallePlanillaGenerada on p.ID equals dp.PlanillaGeneradaID
                               join em in db.Empleados on dp.EmpleadoID equals em.ID
                               where em.ID.Equals(ETEmpleado.ID)
                               group new { p, em } by new {  p.FechaHasta} into f
                               select new
                               {
                                   Fecha = f.Max(r => r.p.FechaHasta)
                               }
                                                );
                 
                        DateTime ultimaFechaPago = (fch.Count() > 0 ? fch.Max(f => f.Fecha).Date : _FechaServidor.Date);
                        fechaUltimaPago.EditValue = ultimaFechaPago;
                        decimal diasTrans = Decimal.Round(Convert.ToDecimal((dateFechaLiquidacion.DateTime.Date - ultimaFechaPago).TotalDays), 2, MidpointRounding.AwayFromZero);
                        salarioDiarioSpinEdit.EditValue = Decimal.Round((Convert.ToDecimal(salarioBasicoSpinEdit.EditValue) / Parametros.General.diasMes), 2, MidpointRounding.AwayFromZero);
                        EtLiquidacion.DiasSalarioMonto = Decimal.Round(diasTrans * (Convert.ToDecimal(salarioBasicoSpinEdit.EditValue) / Parametros.General.diasMes), 2, MidpointRounding.AwayFromZero);
                        //salarioBasicoSpinEdit.EditValue = empleado.SalarioActual;
                        diasSalarioSpinEdit.EditValue = Decimal.Round(diasTrans, 2, MidpointRounding.AwayFromZero);
                        totalSalarioSpinEdit.EditValue = Decimal.Round(EtLiquidacion.DiasSalarioMonto, 2, MidpointRounding.AwayFromZero);
              
                }
                if (e.PrevPage == wpIngresos)
                {
                    EtLiquidacion.OtrosIngresos = Decimal.Round(Convert.ToDecimal(colMonto.SummaryItem.SummaryValue == null ? 0 : colMonto.SummaryItem.SummaryValue), 2, MidpointRounding.AwayFromZero);

                     try { EtLiquidacion.CantidadHE = Decimal.Round(Convert.ToDecimal(ingresos.Compute("Sum(CantidadHoras)", "ID = 8")), 2); }  catch { EtLiquidacion.CantidadHE = 0m; }
                     try { EtLiquidacion.MontoHE = Decimal.Round(Convert.ToDecimal(ingresos.Compute("Sum(Monto)", "ID = 8")), 2); }  catch { EtLiquidacion.MontoHE = 0m; }
           
                }
                if (e.PrevPage == wpDeducciones)
                {
                    EtLiquidacion.OtrasDeducciones = Decimal.Round(Convert.ToDecimal(colMontoDed.SummaryItem.SummaryValue == null ? 0 : colMontoDed.SummaryItem.SummaryValue), 2, MidpointRounding.AwayFromZero);
                }

                if (e.Page == wpFinish)
                {
                    SetLiquidacionValues();
                    ShowResumen();
                }

            }
        }

        private void wizard_SelectedPageChanging(object sender, DevExpress.XtraWizard.WizardPageChangingEventArgs e)
        {
            if (e.Direction == DevExpress.XtraWizard.Direction.Forward)
            {
                if (e.PrevPage == wpEmpleado)
                {
                    lblIndemnizacion.Text = datos;
                }
            }
        }

        #endregion

        private void wizard_FinishClick(object sender, CancelEventArgs e)
        {
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed)
                db.Connection.Open();
            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                try
                {
                    db.Transaction = trans;
                    db.CommandTimeout = 600;

                    Entidad.Empleado EM = db.Empleados.Single(s => s.ID.Equals(ETEmpleado.ID));
                    EM.Activo = false;
                    EM.FechaSalida = EtLiquidacion.FechaFinal;
                    EtLiquidacion.FechaRegistro = _FechaServidor;
                    db.Liquidacion.InsertOnSubmit(EtLiquidacion);

                    db.SubmitChanges();

                    ////Detalle Ingresos
                    //foreach (DataRow dr in ingresos.Rows)
                    //{
                    //    AdmonP.Entidades.LiquidacionDetalleIngreso ldi = new AdmonP.Entidades.LiquidacionDetalleIngreso();
                    //    ldi.IdLiquidacion = liquidacion.ID;
                    //    ldi.Descripcion = dr["Descripcion"].ToString();
                    //    ldi.IdMovimiento = dr["ID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["ID"]);
                    //    ldi.Monto = Decimal.Round(Convert.ToDecimal(dr["Monto"]), 2);
                    //    db.LiquidacionDetalleIngreso.InsertOnSubmit(ldi);

                    //}
                    ////Detalle Deducciones
                    //foreach (DataRow dr in deducciones.Rows)
                    //{
                    //    AdmonP.Entidades.LiquidacionDetalleDeduccion ldi = new AdmonP.Entidades.LiquidacionDetalleDeduccion();
                    //    ldi.IdLiquidacion = liquidacion.ID;
                    //    ldi.Descripcion = dr["Descripcion"].ToString();
                    //    ldi.IdMovimiento = dr["ID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dr["ID"]);
                    //    ldi.Monto = Decimal.Round(Convert.ToDecimal(dr["Monto"]), 2);
                    //    db.LiquidacionDetalleDeduccion.InsertOnSubmit(ldi);
                    //}

                    #region Liquidar Movimientos

                    //foreach (var item in db.AdelantoAguinaldo.Where(l => l.IdEmpleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}

                    //foreach (var item in db.Ausencia.Where(l => l.Empleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}

                    //foreach (var item in db.DiasVacaciones.Where(l => l.IdEmpleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}
                    //foreach (var item in db.DetalleMovimiento.Where(l => l.IdEmpleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}

                    //foreach (var item in db.PagoVacaciones.Where(l => l.IdEmpleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}
                    //foreach (var item in db.Subsidios.Where(l => l.Empleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}
                    //foreach (var item in db.DetallePlanilla.Where(l => l.Empleado == liquidacion.IdEmpleado))
                    //{
                    //    item.Liquidado = true;
                    //}
                    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                    "Grabación de Liquidación de Empleado " + gvEmpleados.GetFocusedRowCellValue(colNombres).ToString() + " " + gvEmpleados.GetFocusedRowCellValue(colApellidos).ToString(), this.Name);
                    RefreshMDI = true;
                    ShowReportLiquidacion(EtLiquidacion.ID);
                    this.Close();
                }
                catch (Exception ex)
                {
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        #endregion

        private void ckTodoMontoEnDolares_CheckedChanged(object sender, EventArgs e)
        {
            //salarios = dataManager.GetListUltimos6Pagos(empleado.IdEmpleado, ckTodoMontoEnDolares.Checked, ckConsolidarSalarios.Checked); // Mostrar el salario devengado de los ultimos 6 meses
            //gridSalarios.DataSource = salarios; //Asignar
            //Calcular13voIndemnizacion(null, null);
            //valorDiasAcumuladosSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasVacAcumuladoSpinEdit.Value, 2);
            //valorDiasDescansadosSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasDescansadosSpinEdit.Value, 2);
            //valorDiasPendientesVacacionesSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasPendienteSpinEdit.Value, 2);
            //totalVacacionesSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / Classes.Global.diasMes), 2) * diasVacacionesSpinEdit.Value, 2);
        }

        private void ckConsolidarSalarios_CheckedChanged(object sender, EventArgs e)
        {
                       
            //salarios = dataManager.GetListUltimos6Pagos(empleado.IdEmpleado, ckTodoMontoEnDolares.Checked, ckConsolidarSalarios.Checked); // Mostrar el salario devengado de los ultimos 6 meses
            //gridSalarios.DataSource = salarios; //Asignar

            //Calcular13voIndemnizacion(sender, null);
            //valorDiasAcumuladosSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasVacAcumuladoSpinEdit.Value, 2);
            //valorDiasDescansadosSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasDescansadosSpinEdit.Value, 2);
            //valorDiasPendientesVacacionesSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / 30), 2) * diasPendienteSpinEdit.Value, 2);
            //totalVacacionesSpinEdit.EditValue = Decimal.Round(Decimal.Round((liquidacion.SalarioBase / Classes.Global.diasMes), 2) * diasVacacionesSpinEdit.Value, 2);
         
        }

       
        #endregion

        private void wizard_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }

        private void wizLiquidacion_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(RefreshMDI);
        }

        private void comboTipoIngreso_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboTipoIngreso.EditValue) == 8)
            {
                this.colCantidadHoras.Visible = true;
                this.colCantidadHoras.VisibleIndex = 1;
                this.spCantidadHoras.Visible = true;
                this.labelCantidadH.Visible = true;
                this.spSalarioDiario.Visible = true;
                this.labelSalarioDiario.Visible = true;
                this.spSalarioDiario.EditValue = Decimal.Round((EtLiquidacion.SalarioBase / 30), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                    this.spCantidadHoras.Visible = false;
                    this.labelCantidadH.Visible = false;
                    this.spSalarioDiario.Visible = false;
                    this.labelSalarioDiario.Visible = false;
                    this.spCantidadHoras.EditValue = 0;
                    this.spSalarioDiario.EditValue = 0;
            }

        }

        private void spSalarioDiario_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spCantidadHoras.EditValue) > 0 && Convert.ToDecimal(spSalarioDiario.EditValue) > 0)
            {
                tipoIngresoSpinEdit.EditValue = Convert.ToDecimal(spCantidadHoras.EditValue) * (Convert.ToDecimal(spSalarioDiario.EditValue)/8);
            }
        }

        private void spCantidadHoras_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spCantidadHoras.EditValue) > 0 && Convert.ToDecimal(spSalarioDiario.EditValue) > 0)
            {
                tipoIngresoSpinEdit.EditValue = Decimal.Round((Convert.ToDecimal(spCantidadHoras.EditValue) * (Convert.ToDecimal(spSalarioDiario.EditValue) / 8)) * 2, 2, MidpointRounding.AwayFromZero);
            }
        }

        private void dateFechaLiquidacion_EditValueChanged(object sender, EventArgs e)
        {
            if (dateFechaLiquidacion.EditValue != null)
            {
                hastaDateEdit.DateTime = dateFechaLiquidacion.DateTime;
                diasTrans = Decimal.Round(Convert.ToDecimal((dateFechaLiquidacion.DateTime.Date - Convert.ToDateTime(desdeDateEdit.EditValue)).TotalDays), 2, MidpointRounding.AwayFromZero);
                        
            }
        }

        

 
    }
}
