using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors;

namespace SAGAS.Nomina.Forms.Wizards
{
    public partial class wizPagoVacaciones : Form
    {
        #region Declaracion
        private SAGAS.Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dview;
        private bool editando, descansada;
        private SAGAS.Entidad.MovimientoEmpleado movimientoempleado;
        private SAGAS.Entidad.VacacionesPagadas pagoVacaciones;
        private SAGAS.Entidad.Empleado empleado;
        private int _MovimientoVacacionID;
        private int idMovimiento;
        private int idEmpleado;
        private bool loaded;
        DataTable dataT = new DataTable();
        internal Forms.FormVacaciones MDI;
        private bool ShowMsg = false;
        private bool NextProvision = false;
        private bool RefreshMDI = false;
        private int IDPrint = 0;
        internal decimal _ValorINSSLaboral = 0;
        private DateTime _FechaActual;
        internal List<int> lista;
        /*
        private SAGAS.Entidad.SAGASDataClassesDataContext db
        private bool editando;
        private SAGAS.Entidad.MovimientoEmpleado moviempleado;
        private SAGAS.Entidad.MovimientoEmpleado pagoVacaciones;
        private SAGAS.Entidad.VacacionesPagadas vacacionesPagadas;
        private SAGAS.Entidad.Empleado empleado;
        private int idMovimiento;
        private int idEmpleado;
        private bool loaded;
        DataTable dataT = new DataTable();
        */
        #endregion

        #region Inicializacion
        /// <summary>
        /// Nuevo Movimiento
        /// </summary>
        /// <param name="tm"></param>
        public wizPagoVacaciones(List<int> l)
        {
            Iniciar();

            lista = l;

            gridEmpleados.DataSource = from s in db.Empleados
                                       join p in db.Planillas on s.PlanillaID equals p.ID
                where  s.Activo == true && lista.Contains(p.EstacionServicioID)
                select new { s.ID, s.Codigo, s.Nombres, s.Apellidos, s.Cedula, s.NumeroSeguroSocial, s.SalarioActual};

            NewMovimiento();
        }
        /// <summary>
        /// Nuevo Movimiento con cliente asignado
        /// </summary>
        /// <param name="empl">Id De Empleado</param>
        public wizPagoVacaciones(int empl)
        {
            Iniciar();

            gridEmpleados.DataSource = from s in db.Empleados
                                       join p in db.Planillas on s.PlanillaID equals p.ID
                                       where s.Activo == true && lista.Contains(p.EstacionServicioID)
                                       select new { s.ID, s.Codigo, s.Nombres, s.Apellidos, s.Cedula, s.NumeroSeguroSocial, s.SalarioActual };
            idEmpleado = empl;

            NewMovimiento();

            wizard.SelectedPage = wpFinish;
            wpFinish.AllowBack = false;
     

        }
        /// <summary>
        /// Editar el movimiento
        /// </summary>
        /// <param name="detalleMovimiento"></param>
        public wizPagoVacaciones(SAGAS.Entidad.MovimientoEmpleado MovimientoEmpleado)
        {
            Iniciar();
            movimientoempleado = MovimientoEmpleado;
            idEmpleado = movimientoempleado.EmpleadoID;
            SAGAS.Entidad.TipoMovimientoEmpleado mov = (from u in db.TipoMovimientoEmpleado
                                                        join s in db.MovimientoEmpleado on u.ID equals s.TipoMovimientoEmpleadoID
                                                        where s.ID == movimientoempleado.ID
                                                        select u).FirstOrDefault();

            gridEmpleados.DataSource = db.Empleados.Where(em => em.ID == idEmpleado).Select(s => new { s.ID, s.Codigo, s.Nombres, s.Apellidos, s.Cedula, s.NumeroSeguroSocial, s.SalarioActual });

            wizard.SelectedPage = wpFinish;
            wpFinish.AllowBack = false;

            editando = true;
            diasVacacionesSpinEdit.EditValue = movimientoempleado.CantidadDias;
            desdeDateEditDesc.EditValue = desdeDateEdit.EditValue = movimientoempleado.FechaInicial;

            pagoVacaciones = db.VacacionesPagadas.First(pv => pv.MovimientoEmpleadoID == movimientoempleado.ID);
        }
        /// <summary>
        /// Editar pago de vacaciones
        /// </summary>
        /// <param name="detalleMovimiento"></param>
        public wizPagoVacaciones(int empl, int pagoVac)
        {
            Iniciar();
            idEmpleado = empl;

            pagoVacaciones = db.VacacionesPagadas.Single(pv => pv.ID == pagoVac);
            
            empleado = db.Empleados.Single(e => e.ID == empl);
            gridEmpleados.DataSource = db.Empleados.Where(em => em.ID == idEmpleado).Select(s => new { s.ID, s.Codigo, s.Nombres, s.Apellidos, s.Cedula, s.NumeroSeguroSocial, s.SalarioActual });

            wizard.SelectedPage = wpFinish;
            wpFinish.AllowBack = false;

            editando = true;

            desdeDateEditDesc.EditValue = desdeDateEdit.EditValue = pagoVacaciones.FechaDesde;
            hastaDateEditDesc.EditValue = hastaDateEdit.EditValue = pagoVacaciones.FechaHasta;
            diasVacacionesSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Dias, 2);
            diasPendientesSpinEdit.EditValue = Decimal.Round(pagoVacaciones.DiasPago, 2);
            fechaPagoDateEdit.EditValue = pagoVacaciones.FechaPago;
            montoSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Monto, 2);
            observasionMemoEdit.EditValue = pagoVacaciones.Obsevacion;
            inssSpinEdit.EditValue = Decimal.Round(pagoVacaciones.SeguroSocial, 2);
            patronalSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Patronal, 2);
            inatecSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Inatec, 2);
            irSpinEdit.EditValue = Decimal.Round(pagoVacaciones.IR, 2);
            totalPagarSpinEdit.EditValue = Decimal.Round(pagoVacaciones.TotalPagar, 2);
            tswVacaDesPag.IsOn = pagoVacaciones.Reposo;

            var query = from p in db.VacacionesPagadas
                        where p.FechaPago >= desdeDateEdit.DateTime.Date && p.FechaPago <= hastaDateEdit.DateTime.Date
                        && p.EmpleadoID == empl
                        select p;
            DataTable dta = new DataTable();

            if (query.Count() > 0) diasPendientesSpinEdit.EditValue = diasVacacionesSpinEdit.Value - query.Sum(d => d.DiasPago);
            else diasPendientesSpinEdit.EditValue = diasVacacionesSpinEdit.EditValue;

            dta = LINQToDataTable(query.AsEnumerable());
            gridVacaciones.DataSource = dta;
            gvVacaciones.ExpandAllGroups();

        }
        
        private void wizPagoVacaciones_Load(object sender, EventArgs e)
        {
            comboPeriodo.EditValue = 15;
            irSpinEdit.Properties.MinValue = 0;
            inatecSpinEdit.Properties.MinValue = 0;
            inssSpinEdit.Properties.MinValue = 0;
            patronalSpinEdit.Properties.MinValue = 0;
            _ValorINSSLaboral =  Parametros.Config.ValorINSSLaboral();
            //--

            if (!Parametros.Config.PagoVacacionesPlanilla())
            {
                desdeDateEditDesc.EditValue = desdeDateEdit.EditValue = Parametros.General.GetNextQuincena(DateTime.Now.Date);
                hastaDateEditDesc.EditValue = hastaDateEdit.EditValue = Parametros.General.GetNextQuincena(DateTime.Now.Date);
            }
            else
            {
                desdeDateEditDesc.EditValue = desdeDateEdit.EditValue = DateTime.Now.Date;
                hastaDateEditDesc.EditValue = hastaDateEdit.EditValue = DateTime.Now.Date;
            }
            loaded = true;
            //--
        }
        #endregion

        #region Metodos

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others* will follow |*other ones
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static bool ValidateEmptyStringRule(BaseEdit control, DXErrorProvider dxErrorProvider)
        {
            if (control.EditValue == null || control.EditValue.ToString().Trim().Length == 0)
            {
                //ShowDialog(Properties.Resources.STR_CAMPO_REQUERIDO, MsgType.caution);

                dxErrorProvider.SetError(control, "Campo requerido", ErrorType.Critical);

                control.Focus();

                return false;
            }
            else
            {
                dxErrorProvider.SetError(control, "", ErrorType.None);
                return true;
            }
        }

        private void Iniciar()
        {
            InitializeComponent();
            db = new SAGAS.Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            dview = new SAGAS.Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
            _FechaActual = Convert.ToDateTime(db.GetDateServer());
            fechaPagoDateEdit.EditValue = Convert.ToDateTime(db.GetDateServer());
            dateFechaCorte.EditValue = _FechaActual;
            _MovimientoVacacionID = Parametros.Config.MovimientoVacacionID();
            var movimiento = db.TipoMovimientoEmpleado.Where(mov => mov.ID == _MovimientoVacacionID);
            if (movimiento.Count() > 0)
                idMovimiento = movimiento.First().ID;
            else
            {
                Parametros.General.DialogMsg("No se encuentra configurado las Vacaciones.\nPor favor comuniquese con Oficina Central para arreglar esto.", Parametros.MsgType.warning);
                this.Close();
            }
        }

        private void NewMovimiento()
        {
            movimientoempleado = new SAGAS.Entidad.MovimientoEmpleado();
            movimientoempleado.TipoCambio = 1;
            //moviempleado.PagarEnCuota = true;
            movimientoempleado.QuitarDePlanilla = true;
            movimientoempleado.CantidadCuotas = 1;
            movimientoempleado.FechaInicial = Parametros.General.GetNextQuincena(DateTime.Now);
            movimientoempleado.FechaFinal = movimientoempleado.FechaInicial;
            movimientoempleado.Descansadas = false;

            pagoVacaciones = new SAGAS.Entidad.VacacionesPagadas();
            pagoVacaciones.Reposo = false;
            
        }

        private bool ValidarCampos()
        {
            foreach (object control in groupBox1.Controls)
            {
                if (control.GetType() == typeof(DevExpress.XtraEditors.SpinEdit))
                {
                    DevExpress.XtraEditors.SpinEdit spe = control as DevExpress.XtraEditors.SpinEdit;
                    if (spe.Value == null) return false;
                } else if (control.GetType() == typeof(DevExpress.XtraEditors.LookUpEdit))
                {
                    DevExpress.XtraEditors.DateEdit de = control as DevExpress.XtraEditors.DateEdit;
                    if (de.EditValue == null) return false;
                }
            }

            if (descansada)
            {
                if ((!chDay.Checked && !chHour.Checked) || (!chProgramada.Checked && !chPersonal.Checked && !chOtros.Checked))
                {
                    Parametros.General.DialogMsg("Favor revisar los campos requeridos", Parametros.MsgType.warning, null);
                    return false;
                }

                if (chHour.Checked)
                {
                    TimeSpan d = Convert.ToDateTime(timeFin.EditValue) - Convert.ToDateTime(timeInicio.EditValue);
                    if (((d.Hours + (d.Minutes / 60m)) / 8m) < 0)
                    {
                        Parametros.General.DialogMsg("Por Favor revise las Horas de las Vacaciones", Parametros.MsgType.warning, null);
                        return false;
                    }
                }
            }
            return true;
        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(montoSpinEdit, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(desdeDateEdit, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(desdeDateEdit, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(comboPeriodo, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(hastaDateEdit, ErrorProvider);
        }

        private void ShowReportVacaciones()
        {
            try
            {
                //SAGAS.Reportes.Nomina.Hojas.PagoVacaciones rep = new SAGAS.Reportes.Nomina.Hojas.PagoVacaciones();
                ////SAGAS.Entidad.DataManager dm = new SAGAS.Entidad.DataManager(Classes.Global.GetConnectionString());
                //var row = db.GetPagoVacaciones(pagoVacaciones.ID, pagoVacaciones.EmpleadoID, pagoVacaciones.FechaDesde, pagoVacaciones.FechaHasta, pagoVacaciones.Reposo).ToList();
                ////SAGAS.Reportes.SAGASDataSet ds = db.GetPagoVacaciones(ID, pagoVacaciones.EmpleadoID, pagoVacaciones.FechaDesde, pagoVacaciones.FechaHasta, pagoVacaciones.Reposo);

                //if (row.ToList().Count > 0)
                //{
                //    List<SAGAS.Entidad.GetPagoVacacionesResult> res = row;
                //    rep.DataSource = res;
                //    //rep.Logo = Parametros.General.LogoEmpresa(Convert.ToInt32(dview.ListPagoVacaciones.Single().EmpresaID), db);
                //    //Parametros.General.ShowReport(rep, "Pago vacaciones de " + gvEmpleados.GetFocusedDisplayText());
                //}
                //else
                //{
                //    Parametros.General.DialogMsg("No hay datos que mostrar", SAGAS.Parametros.MsgType.warning);
                //}
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }
        }


        #endregion

        #region Eventos

        private void CalcularCampos(object sender, EventArgs e)
        {
            if (desdeDateEdit.EditValue != null && hastaDateEdit.EditValue != null && loaded)
            {
                int days = 0, month = 0, year = 0;

                Parametros.General.RangoFechasYMD(hastaDateEdit.DateTime.Date, desdeDateEdit.DateTime.Date, out year, out month, out days);
                
                decimal diasVacaciones = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2);
                diasVacaciones += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2);
                diasVacaciones += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2);

                diasVacacionesSpinEdit.EditValue = Decimal.Round(diasVacaciones, 2);

                diasPendientesSpinEdit.Properties.MaxValue = diasVacacionesSpinEdit.Value;

 
               diasPendientesSpinEdit.EditValue = Decimal.Round(Decimal.Round(Convert.ToDecimal(diasVacacionesSpinEdit.EditValue),2) - Decimal.Round(Convert.ToDecimal(diasDescansadosSpinEdit.EditValue),2), 2);

                

            }
        }

        private void CalcularCamposDesc(object sender, EventArgs e)
        {
            if (diasVacacionesSpinEditDesc.Enabled)
            {
                int dias = Convert.ToInt32(Decimal.Ceiling(diasVacacionesSpinEditDesc.Value));

                //if (diasVacacionesSpinEditDesc.Value > speRemainingDays.Value)
                //{
                //    Parametros.General.DialogMsg("Imposible otorgar " + diasVacacionesSpinEditDesc.Value + " dias de vacaciones, ya que el empleado sólo dispone de " + speRemainingDays.Value + " dias para vacaciones.", Parametros.MsgType.warning);
                //    diasVacacionesSpinEditDesc.Properties.MaxValue = speRemainingDays.Value;
                //}
                
                hastaDateEditDesc.EditValue = desdeDateEditDesc.DateTime.AddDays(dias - 1);

                if (!chkVariosMeses.Checked)
                {
                    if (!hastaDateEditDesc.DateTime.Month.Equals(desdeDateEditDesc.DateTime.Month))
                    {
                        Parametros.General.DialogMsg("El rango de vacaciones debe estar en el mismo mes " + Parametros.General.GetMonthInLetters(desdeDateEditDesc.DateTime.Month), Parametros.MsgType.warning);
                        diasVacacionesSpinEditDesc.Value = 0;

                    }
                }

                //GeTLastHolidayDate();
            }
            else if (!diasVacacionesSpinEditDesc.Enabled)
                hastaDateEditDesc.EditValue = desdeDateEditDesc.EditValue;
        }

        //private void GeTLastHolidayDate()
        //{
        //    newlastholiday = empsel.FechaUltimaVacaciones.Value;
        //    int meses = 0;
        //    meses = Convert.ToInt32(Math.Truncate(Convert.ToDouble(diasVacacionesSpinEdit.Value) / 2.5));
        //    double decimals = 0;
        //    decimals = Math.Abs(Convert.ToDouble(diasVacacionesSpinEdit.Value) / 2.5 - meses);
        //    int dia = 0;
        //    dia = Convert.ToInt32((decimals * 30));
        //    //MessageBox.Show("numero completo: " + (Convert.ToDouble(diasVacacionesSpinEdit.Value) / 2.5) + " entero: " + meses + " decimal: " + decimals);

        //    //MessageBox.Show("ultima vak: " + empsel.FechaUltimaVacaciones.Value.ToShortDateString());
        //    if (decimals == 0)
        //    {
        //        newlastholiday = empsel.FechaUltimaVacaciones.Value.AddMonths(meses).AddDays(-1);

        //    }
        //    else if (!(decimals == 0))
        //    {
        //        if (meses > 0)
        //            newlastholiday = empsel.FechaUltimaVacaciones.Value.AddMonths(meses).AddDays(dia).AddDays(-1);
        //    }

        //    //MessageBox.Show("new last vak: " + newlastholiday.Date.ToShortDateString());
        //}

        private decimal GetTotalHoliday(DateTime dateTime, DateTime FechaCorte)
        {
            try
            { 
            int days = 0, month = 0, year = 0;

            Parametros.General.RangoFechasYMD(FechaCorte.Date, dateTime, out year, out month, out days);
            
            decimal diasVacaciones = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2);
            diasVacaciones += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2);
            diasVacaciones += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2);

            //TimeSpan ts = Convert.ToDateTime(db.GetSystemDate().Value.ToShortDateString()) - Convert.ToDateTime(dateTime.ToShortDateString());
            return diasVacaciones;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());
                return 0m;
            }
        }


        private void montoSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                //subTotalSpinEdit.EditValue = montoSpinEdit.Value - montoVacacionesSpinEdit.Value;

                pagoVacaciones.SeguroSocial = Decimal.Round(montoSpinEdit.Value * _ValorINSSLaboral, 2, MidpointRounding.AwayFromZero);
                if (pagoVacaciones.SeguroSocial < 0) pagoVacaciones.SeguroSocial = 0;

                //decimal ingresosConRetenciones = Decimal.Round((montoSpinEdit.Value - pagoVacaciones.SeguroSocial) * Parametros.General.factorIR, 2);

                #region <<< IR >>>
                decimal vTotalIR = 0;

                decimal vSaldo = 0;
                

                vSaldo = Convert.ToDecimal(gvEmpleados.GetFocusedRowCellValue(colSalarioActual)) - (Decimal.Round(Convert.ToDecimal(gvEmpleados.GetFocusedRowCellValue(colSalarioActual)) * _ValorINSSLaboral,2, MidpointRounding.AwayFromZero));
                if (vSaldo > 8333.34m)
                {
                    //CALCULO IR NORMAL
                    decimal vExpectativaAnual = 0, vRentaSujeta = 0, vIRExceso, vImpuestoRenta = 0;

                    vExpectativaAnual = Decimal.Round(vSaldo * 12, 2, MidpointRounding.AwayFromZero);

                    var rangoIR = db.TablaIR.SingleOrDefault(ir => vExpectativaAnual > ir.Desde && vExpectativaAnual <= ir.Hasta);

                    if (rangoIR != null)
                    {
                        vRentaSujeta = vExpectativaAnual - rangoIR.Techo;
                        vIRExceso = Decimal.Round(vRentaSujeta * rangoIR.Tasa, 2, MidpointRounding.AwayFromZero);
                        vImpuestoRenta = Decimal.Round(vIRExceso + rangoIR.Base, 2, MidpointRounding.AwayFromZero);
                        vTotalIR = Decimal.Round(vImpuestoRenta / 12, 2, MidpointRounding.AwayFromZero);
                    }

                    //CALCULO IR OCASIONAL
                    vExpectativaAnual += Decimal.Round(montoSpinEdit.Value - pagoVacaciones.SeguroSocial, 2, MidpointRounding.AwayFromZero);

                    var OrangoIR = db.TablaIR.SingleOrDefault(ir => vExpectativaAnual > ir.Desde && vExpectativaAnual <= ir.Hasta);

                    if (OrangoIR != null)
                    {
                        vRentaSujeta = vExpectativaAnual - OrangoIR.Techo;
                        vIRExceso = Decimal.Round(vRentaSujeta * OrangoIR.Tasa, 2, MidpointRounding.AwayFromZero);
                        vTotalIR = Decimal.Round(vIRExceso + OrangoIR.Base - vImpuestoRenta, 2, MidpointRounding.AwayFromZero);
                    }
                }
                else
                {
                    vSaldo += (montoSpinEdit.Value - pagoVacaciones.SeguroSocial);
                    //CALCULO IR NORMAL + IR OCASIONAL
                    decimal vExpectativaAnual = 0, vRentaSujeta = 0, vIRExceso, vImpuestoRenta;

                    vExpectativaAnual = Decimal.Round(vSaldo * 12, 2, MidpointRounding.AwayFromZero);

                    var rangoIR = db.TablaIR.SingleOrDefault(ir => vExpectativaAnual > ir.Desde && vExpectativaAnual <= ir.Hasta);

                    if (rangoIR != null)
                    {
                        vRentaSujeta = vExpectativaAnual - rangoIR.Techo;
                        vIRExceso = Decimal.Round(vRentaSujeta * rangoIR.Tasa, 2, MidpointRounding.AwayFromZero);
                        vImpuestoRenta = Decimal.Round(vIRExceso + rangoIR.Base, 2, MidpointRounding.AwayFromZero);
                        vTotalIR = Decimal.Round(vImpuestoRenta / 12, 2, MidpointRounding.AwayFromZero);
                    }

                }

                pagoVacaciones.IR = vTotalIR;

                #endregion

                //if (ingresosConRetenciones < 0) ingresosConRetenciones = 0;
                //if (ingresosConRetenciones > 0)
                //{
                //    //Obtiene el registro del rango salarial
                //    var rangoIR = db.TablaIR
                //        .Where(ir => ingresosConRetenciones > ir.Desde && ingresosConRetenciones <= ir.Hasta).Single();

                //    decimal excedente = Decimal.Round((ingresosConRetenciones) - rangoIR.Techo, 2);

                //    pagoVacaciones.IR = 0;// Decimal.Round(((((excedente * rangoIR.Tasa) + rangoIR.Base) / Parametros.General.factorIR)), 2);
                //}
                //else pagoVacaciones.IR = 0;



                pagoVacaciones.Inatec = Decimal.Round(montoSpinEdit.Value * Parametros.Config.ValorINATEC(), 2, MidpointRounding.AwayFromZero);
                pagoVacaciones.Patronal = Decimal.Round(montoSpinEdit.Value * Parametros.Config.ValorINSSPatronal(), 2, MidpointRounding.AwayFromZero);

                inssSpinEdit.EditValue = Decimal.Round(pagoVacaciones.SeguroSocial, 2, MidpointRounding.AwayFromZero);
                patronalSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Patronal, 2, MidpointRounding.AwayFromZero);
                irSpinEdit.EditValue = Decimal.Round(pagoVacaciones.IR, 2, MidpointRounding.AwayFromZero);
                inatecSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Inatec, 2, MidpointRounding.AwayFromZero);

                pagoVacaciones.TotalPagar = Decimal.Round(montoSpinEdit.Value + movimientoempleado.Abono - pagoVacaciones.SeguroSocial - pagoVacaciones.IR, 2, MidpointRounding.AwayFromZero);

                totalPagarSpinEdit.EditValue = Decimal.Round(pagoVacaciones.TotalPagar, 2, MidpointRounding.AwayFromZero);
            }
        }

        private void diasPagoSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (loaded)
            {
                decimal salario = db.Empleados.Single(empl => empl.ID == idEmpleado).SalarioActual;
                decimal dia = Decimal.Round((salario / 30),2); // Se trabaja 8 horas, esto debe estar sujeto a su horario

                pagoVacaciones.Monto = Decimal.Round(diasPendientesSpinEdit.Value * dia,2);

                switch (Convert.ToInt32(diasVacacionesSpinEdit.Value))
                {
                    case 15:
                        pagoVacaciones.Monto = Decimal.Round(empleado.SalarioActual / 2, 2, MidpointRounding.AwayFromZero);
                        break;
                    case 30:
                        pagoVacaciones.Monto = Decimal.Round(empleado.SalarioActual, 2, MidpointRounding.AwayFromZero);
                        break;
                    case 45:
                        pagoVacaciones.Monto = Decimal.Round(empleado.SalarioActual * 1.5m, 2, MidpointRounding.AwayFromZero);
                        break;
                    case 60:
                        pagoVacaciones.Monto = Decimal.Round(empleado.SalarioActual * 2, 2, MidpointRounding.AwayFromZero);
                        break;
                    default:
                        pagoVacaciones.Monto = Decimal.Round(dia * diasVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);

                        break;

                }

                montoSpinEdit.EditValue = Decimal.Round(pagoVacaciones.Monto,2, MidpointRounding.AwayFromZero);
            }
        }

        private void wizard_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            if (e.Page == wpEmpleado)
            {
                if (gvEmpleados.FocusedRowHandle >= 0)
                {
                    idEmpleado = Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue("ID"));

                    var empsel = db.Empleados.Select(s => new { s.ID, s.FechaUltimaVacacion, s.FechaIngreso }).SingleOrDefault(o => o.ID.Equals(idEmpleado));

                    if (empsel != null)
                    {
                        decimal total = 0;
                        if (empsel.FechaUltimaVacacion != null)
                        {
                            total = GetTotalHoliday(Convert.ToDateTime(empsel.FechaUltimaVacacion), dateFechaCorte.DateTime.Date);
                            desdeDateEdit.DateTime = Convert.ToDateTime(empsel.FechaUltimaVacacion.Value).Date;
                        }
                        else if (empsel.FechaUltimaVacacion == null)
                        {
                            //empsel.FechaUltimaVacacion = empsel.FechaIngreso;
                            total = GetTotalHoliday(Convert.ToDateTime(empsel.FechaIngreso), dateFechaCorte.DateTime.Date);
                        }
                        speRemainingDays.Value = Math.Round(total, 2, MidpointRounding.AwayFromZero);//midpoint referencia de redondeo[awayfromzero] a partir del 0.5 y no del 0.6
                   
                    }

                    tswVacaDesPag_Toggled(null, null);
                }
                else
                {
                    Parametros.General.DialogMsg("Favor seleccionar un empleado.", SAGAS.Parametros.MsgType.warning);
                    e.Handled = true;
                }
            }
        }

        private void wizard_SelectedPageChanged(object sender, DevExpress.XtraWizard.WizardPageChangedEventArgs e)
        {
            if (e.Direction == DevExpress.XtraWizard.Direction.Forward)
            {
                if (e.Page == wpFinish)
                {
                    lblEmpleado.Text = "Empleado: " + gvEmpleados.GetFocusedRowCellValue("Nombres") + " " + gvEmpleados.GetFocusedRowCellValue("Apellidos");
                    lblEmpleado.Text += " | Salario: " + Convert.ToDecimal(gvEmpleados.GetFocusedRowCellValue(colSalarioActual)).ToString("#,###,##0.00");
                    if (!editando)
                    {
                        empleado = db.Empleados.Single(em => em.ID == Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue(colIdEmpleado)));
                        desdeDateEditDesc.EditValue = db.GetDateServer();
                            //desdeDateEdit.EditValue = /*empleado.FechaUltimaVacacion == null ?*/ db.GetDateServer();//DateTime.Now.Date
                            /*: db.Empleados.Single(em => em.ID == Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue(colIdEmpleado))).FechaUltimaVacacion;*/

                        var query = from vdcv in dview.VistaDiasCuentaVacaciones
                                    where vdcv.EmpleadoID == idEmpleado
                                    select new
                                    {
                                        vdcv.ID,
                                        vdcv.TipoAusenciaID,
                                        vdcv.MovimientoEmpleadoID,
                                        vdcv.EmpleadoID,
                                        vdcv.Monto,
                                        vdcv.Salario,
                                        vdcv.SalarioDiario,
                                        vdcv.Dias,
                                        vdcv.DiasPago,
                                        vdcv.FechaDesde,
                                        vdcv.FechaHasta,
                                        vdcv.FechaPago,
                                        vdcv.Selected
                                    };

                        
                        dataT = LINQToDataTable(query.AsEnumerable());//Classes.Util.LINQToDataTable(query.AsEnumerable());
                        gridVacaciones.DataSource = dataT;
                        gvVacaciones.ExpandAllGroups();

                        CalcularCampos(sender, null);
                        CalcularCamposDesc(sender, null);
                    }
                }
            }
        }

        private void gvVacaciones_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        
            {
            if (e.RowHandle >= 0)
            {
                if (e.Column == colSelected)
                {
                    if (Convert.ToBoolean(gvVacaciones.GetFocusedRowCellValue(colSelected)))
                    {
                        decimal dias = 0;
                        foreach (DataRow row in dataT.Rows)
                            {
                            if (Convert.ToBoolean(row["Selected"]))
                                {
                                dias += Convert.ToDecimal(row["DiasPago"]);
                                }
                            }
                        diasDescansadosSpinEdit.EditValue = dias;
                    }
                    else
                        {
                        decimal dias = 0;
                        foreach (DataRow row in dataT.Rows)
                            {
                            if (Convert.ToBoolean(row["Selected"]))
                                {
                                dias += Convert.ToDecimal(row["DiasPago"]);
                                }
                            }
                        diasDescansadosSpinEdit.EditValue = dias;
                        }
                }
            }
        }

        private void wizard_FinishClick(object sender, CancelEventArgs e)
        {
            if (!ValidarCampos())
            {
                e.Cancel = true;
                return;
            }
            try
            {
                if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();
                using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
                {
                    try
                    {
                        db.CommandTimeout = 600;
                        db.Transaction = trans;

                        if (descansada)
                        {
                            #region <<<  DESCANSADAS  >>>

                            if (idEmpleado > 0)
                            {
                                Entidad.MovimientoEmpleado ME = new Entidad.MovimientoEmpleado();
                                ME.TipoMovimientoEmpleadoID = Parametros.Config.MovimientoVacacionID();
                                ME.EmpleadoID = idEmpleado;
                                ME.FechaInicial = desdeDateEditDesc.DateTime.Date;
                                ME.FechaFinal = hastaDateEditDesc.DateTime.Date;
                                ME.Descripcion = observasionMemoEdit.Text;
                                ME.SalarioDiario = (empleado != null ? Decimal.Round(empleado.SalarioActual / 30m, 2, MidpointRounding.AwayFromZero) : 0m);
                                ME.CantidadDias = diasVacacionesSpinEditDesc.Value;
                                ME.MontoTotal = Decimal.Round(ME.CantidadDias * ME.SalarioDiario, 2, MidpointRounding.AwayFromZero);
                                ME.Descansadas = true;

                                db.MovimientoEmpleado.InsertOnSubmit(ME);
                                db.SubmitChanges();

                                Entidad.DiasVacaciones DV = new Entidad.DiasVacaciones();
                                DV.EmpleadoID = ME.EmpleadoID;
                                DV.MovimientoEmpleadoID = ME.ID;
                                DV.Salario = (empleado != null ? Decimal.Round(empleado.SalarioActual / 30m, 2, MidpointRounding.AwayFromZero) : 0m);
                                DV.SalarioDiario = ME.SalarioDiario;
                                DV.FechaDesde = new DateTime(Convert.ToDateTime(desdeDateEditDesc.EditValue).Year, Convert.ToDateTime(desdeDateEditDesc.EditValue).Month, Convert.ToDateTime(desdeDateEditDesc.EditValue).Day, Convert.ToDateTime(timeInicio.EditValue).Hour, Convert.ToDateTime(timeInicio.EditValue).Minute, 00);
                                DV.FechaHasta = new DateTime(Convert.ToDateTime(hastaDateEditDesc.EditValue).Year, Convert.ToDateTime(hastaDateEditDesc.EditValue).Month, Convert.ToDateTime(hastaDateEditDesc.EditValue).Day, Convert.ToDateTime(timeFin.EditValue).Hour, Convert.ToDateTime(timeFin.EditValue).Minute, 00);
                                DV.Dias = DV.DiasPago = ME.CantidadDias;
                                DV.FechaPago = new DateTime(Convert.ToDateTime(desdeDateEditDesc.EditValue).Year, Convert.ToDateTime(desdeDateEditDesc.EditValue).Month, Convert.ToDateTime(desdeDateEditDesc.EditValue).Day, Convert.ToDateTime(timeInicio.EditValue).Hour, Convert.ToDateTime(timeInicio.EditValue).Minute, 00);
                                DV.Monto = Decimal.Round(DV.Dias * DV.SalarioDiario, 2, MidpointRounding.AwayFromZero);
                                DV.Observacion = observasionMemoEdit.Text;
                                DV.Personal = chPersonal.Checked;
                                DV.VacacionesProgramadas = chProgramada.Checked;
                                DV.Otras = memoOtros.Text;
                                DV.DiasAcumulados = speRemainingDays.Value;
                                DV.DiasPendientes = DV.DiasAcumulados - DV.Dias;
                                DV.DiasCalendario = (DV.Dias * 12);

                                db.DiasVacaciones.InsertOnSubmit(DV);
                                db.SubmitChanges();

                                // #region Actualizar VIEJA

                                //SAGAS.Entidad.Empleado EM = db.Empleados.Single(empl => empl.ID == idEmpleado);

                                //if (EM.FechaUltimaVacacion == null)
                                //    EM.FechaUltimaVacacion = EM.FechaIngreso;

                                //int year, month, day;
                                ////AÑOS
                                //year = Convert.ToInt32(Math.Floor(diasVacacionesSpinEditDesc.Value / 30m));
                                
                                //decimal vmonth = Decimal.Round((diasVacacionesSpinEditDesc.Value / 30m - Math.Floor(diasVacacionesSpinEditDesc.Value / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);
                                
                                //month = Convert.ToInt32(Math.Floor(vmonth));

                                //decimal vday = vmonth - Math.Floor(vmonth);

                                //day = Convert.ToInt32(Decimal.Round(vday * 30m, 0, MidpointRounding.AwayFromZero));

                                //DateTime ViejaFecha = EM.FechaUltimaVacacion.Value.Date;
                                //DateTime NuevaVacacion = EM.FechaUltimaVacacion.Value.Date.AddYears(year).AddMonths(month).AddDays(day);

                                //Entidad.UltimaFechaVacacion UFV = new Entidad.UltimaFechaVacacion();
                                //UFV.DiasVacacionesID = DV.ID;
                                //UFV.MovimientoEmpleadoID = ME.ID;
                                //UFV.AntiguaFechaUltimaVacacion = ViejaFecha;
                                //UFV.FechaUltimaVacacion = NuevaVacacion;
                                //UFV.EmpleadoID = EM.ID;

                                //db.UltimaFechaVacacions.InsertOnSubmit(UFV);
                                //db.SubmitChanges();

                                //EM.FechaUltimaVacacion = NuevaVacacion.Date;

                                //db.SubmitChanges();
                                //#endregion

                                #region Actualizar Fecha Ultima de pago de vacaciones

                                Entidad.Empleado EM = db.Empleados.Single(empl => empl.ID == idEmpleado);

                                if (EM.FechaUltimaVacacion == null)
                                    EM.FechaUltimaVacacion = EM.FechaIngreso;

                                int year, month, day;
                                //AÑOS
                                year = Convert.ToInt32(Math.Floor(diasVacacionesSpinEditDesc.Value / 30m));

                                decimal vmonth = Decimal.Round((diasVacacionesSpinEditDesc.Value / 30m - Math.Floor(diasVacacionesSpinEditDesc.Value / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);

                                month = Convert.ToInt32(Math.Floor(vmonth));

                                decimal vday = vmonth - Math.Floor(vmonth);

                                day = Convert.ToInt32(Decimal.Round(vday * 30m, 0, MidpointRounding.AwayFromZero));

                                DateTime ViejaFecha = EM.FechaUltimaVacacion.Value.Date;
                                DateTime NuevaVacacion = EM.FechaUltimaVacacion.Value.Date;

                                NuevaVacacion = NuevaVacacion.Date.AddYears(year);
                                NuevaVacacion = NuevaVacacion.Date.AddMonths(month);

                                int d = 30 - NuevaVacacion.Day;

                                if (!day.Equals(0))
                                {
                                    if (d < day)
                                    {
                                        day -= d;
                                        NuevaVacacion = NuevaVacacion.Date.AddMonths(1);
                                        NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, day);
                                    }
                                    else if (d > day)
                                    {
                                        NuevaVacacion = NuevaVacacion.Date.AddDays(day);
                                    }
                                    else
                                    {
                                        NuevaVacacion = NuevaVacacion.Date.AddMonths(1);
                                        NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, 1);
                                    }
                                }
                                //else
                                //    NuevaVacacion = NuevaVacacion.Date.AddDays(-1);




                                EM.FechaUltimaVacacion = NuevaVacacion.Date;

                                db.SubmitChanges();
                                #endregion

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se registró las vacaciones de : " + EM.Nombres + " " + EM.Apellidos + " desde " + DV.FechaDesde.ToShortDateString() + " hasta " + DV.FechaHasta.ToShortDateString(), this.Name);
                            
                            trans.Commit();
                            Parametros.General.DialogMsg(SAGAS.Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, SAGAS.Parametros.MsgType.message);
                            ShowMsg = true;
                            this.Close();
                            }
                            else
                            {
                                Parametros.General.DialogMsg("No ha sido seleccionado ningún empleado", SAGAS.Parametros.MsgType.warning);
                                e.Cancel = true;
                                return;
                            }

                            #endregion
                        }
                        else
                        {
                            #region <<<  PAGADAS  >>>
                            
                            if (Parametros.Config.PagoVacacionesPlanilla())
                            {
                                #region ::: PagoVacacionesPlanilla :::
                                //if (!editando)
                                //{
                                //    movimiento = new SAGAS.Entidad.DetalleMovimiento();
                                //    movimiento.TipoCambio = 1;
                                //    movimiento.Periodo = 15;
                                //    movimiento.PagarEnCuota = true;
                                //    movimiento.AplicarEnPlanilla = true;
                                //}
                                movimientoempleado.ID = idMovimiento;
                                movimientoempleado.EmpleadoID = idEmpleado;
                                movimientoempleado.CantidadDias = Decimal.Round((!descansada ? diasVacacionesSpinEdit.Value : seDiasVak.Value), 2);
                                movimientoempleado.Descripcion = "Pago de Vacaciones";
                                movimientoempleado.MontoTotal = Decimal.Round(montoSpinEdit.Value, 2);
                                movimientoempleado.CantidadCuotas = Convert.ToInt32(cuotasSpinEdit.Value);
                                movimientoempleado.FechaInicial = !descansada ? desdeDateEdit.DateTime.Date : deFechaDesdeVD.DateTime.Date;
                                movimientoempleado.FechaFinal = !descansada ? hastaDateEdit.DateTime.Date : deFechaHastaVD.DateTime.Date;
                                movimientoempleado.Descripcion = observasionMemoEdit.Text;
                                movimientoempleado.Abono = Decimal.Round(montoSpinEdit.Value, 2);
                                movimientoempleado.MontoCuota = Decimal.Round(montoSpinEdit.Value, 2);
                                movimientoempleado.Pagado = true;
                                movimientoempleado.Descansadas = descansada;

                                if (!editando)
                                {
                                    db.MovimientoEmpleado.InsertOnSubmit(movimientoempleado);
                                }
                                db.SubmitChanges();

                                pagoVacaciones.Salario = Decimal.Round(empleado.SalarioActual, 2);
                                pagoVacaciones.SalarioDiario = Decimal.Round(pagoVacaciones.Salario / Parametros.General.diasMes, 2);
                                pagoVacaciones.FechaDesde = !descansada ? desdeDateEdit.DateTime.Date : deFechaDesdeVD.DateTime.Date;
                                pagoVacaciones.FechaHasta = !descansada ? hastaDateEdit.DateTime.Date : deFechaHastaVD.DateTime.Date;
                                pagoVacaciones.Dias = Decimal.Round((!descansada ? diasVacacionesSpinEdit.Value : seDiasVak.Value), 2);
                                pagoVacaciones.DiasCuentaVacaciones = Decimal.Round((!descansada ? diasDescansadosSpinEdit.Value : seDiasDesk.Value), 2);
                                pagoVacaciones.DiasPago = Decimal.Round((!descansada ? diasPendientesSpinEdit.Value : seDiasPend.Value), 2);
                                pagoVacaciones.EmpleadoID = idEmpleado;
                                pagoVacaciones.FechaPago = fechaPagoDateEdit.DateTime.Date;
                                pagoVacaciones.MontoVacAcumuladas = Decimal.Round(montoAcumuladoSpinEdit.Value, 2);
                                pagoVacaciones.Deduccion = Decimal.Round(montoVacacionesSpinEdit.Value, 2);
                                pagoVacaciones.Monto = Decimal.Round(montoSpinEdit.Value, 2);
                                pagoVacaciones.Obsevacion = observasionMemoEdit.Text;
                                pagoVacaciones.SeguroSocial = Decimal.Round(inssSpinEdit.Value, 2);
                                pagoVacaciones.Patronal = Decimal.Round(patronalSpinEdit.Value, 2);
                                pagoVacaciones.Inatec = Decimal.Round(inatecSpinEdit.Value, 2);
                                pagoVacaciones.IR = Decimal.Round(irSpinEdit.Value, 2);
                                pagoVacaciones.TotalPagar = Decimal.Round(totalPagarSpinEdit.Value, 2);
                                pagoVacaciones.Reposo = descansada;

                                if (!editando)
                                {
                                    pagoVacaciones.MovimientoEmpleadoID = movimientoempleado.ID;
                                    db.VacacionesPagadas.InsertOnSubmit(pagoVacaciones);
                                }
                                db.SubmitChanges();
                                #endregion
                            }
                            else
                            {
                                pagoVacaciones.Salario = empleado.SalarioActual;
                                pagoVacaciones.SalarioDiario = Decimal.Round(pagoVacaciones.Salario / Parametros.General.diasMes, 2);
                                pagoVacaciones.FechaDesde = !descansada ? desdeDateEdit.DateTime.Date : deFechaDesdeVD.DateTime.Date;
                                pagoVacaciones.FechaHasta = !descansada ? hastaDateEdit.DateTime.Date : deFechaHastaVD.DateTime.Date;
                                pagoVacaciones.Dias = Decimal.Round((!descansada ? diasVacacionesSpinEdit.Value : seDiasVak.Value), 2);
                                pagoVacaciones.DiasCuentaVacaciones = Decimal.Round((!descansada ? diasDescansadosSpinEdit.Value : seDiasDesk.Value), 2);
                                pagoVacaciones.DiasPago = Decimal.Round((!descansada ? diasPendientesSpinEdit.Value : seDiasPend.Value), 2);
                                pagoVacaciones.EmpleadoID = idEmpleado;
                                pagoVacaciones.FechaPago = fechaPagoDateEdit.DateTime.Date;
                                pagoVacaciones.MontoVacAcumuladas = Decimal.Round(montoAcumuladoSpinEdit.Value, 2);
                                pagoVacaciones.Deduccion = Decimal.Round(montoVacacionesSpinEdit.Value, 2);
                                pagoVacaciones.Monto = Decimal.Round(montoSpinEdit.Value, 2);
                                pagoVacaciones.Obsevacion = observasionMemoEdit.Text;
                                pagoVacaciones.SeguroSocial = Decimal.Round(inssSpinEdit.Value, 2);
                                pagoVacaciones.Patronal = Decimal.Round(patronalSpinEdit.Value, 2);
                                pagoVacaciones.Inatec = Decimal.Round(inatecSpinEdit.Value, 2);
                                pagoVacaciones.IR = Decimal.Round(irSpinEdit.Value, 2);
                                pagoVacaciones.TotalPagar = Decimal.Round(totalPagarSpinEdit.Value, 2);

                            #if (ManejarLiquidacionVacaciones)
                            foreach (var pg in pagoVacaciones.DetallePagoVacaciones)
                                db.DetallePagoVacaciones.DeleteOnSubmit(pg);

                            var query = from p in db.PagoVacaciones
                                        where p.FechaPago >= desdeDateEdit.DateTime.Date && p.FechaPago <= hastaDateEdit.DateTime.Date
                                         && p.IdEmpleado == idEmpleado
                                        select p;
                            //Agegar detalle para liquidacion de vacaciones
                            foreach (var item in query)
                            {
                                SAGAS.Entidad.DetallePagoVacaciones dpv = new SAGAS.Entidad.DetallePagoVacaciones();
                                dpv.Dias = item.Dias;
                                dpv.DiasPago = item.DiasPago;
                                dpv.FechaDesde = item.FechaDesde;
                                dpv.FechaHasta = item.FechaHasta;
                                dpv.FechaPago = item.FechaPago;
                                dpv.IdAusencia = item.IdAusencia;
                                dpv.IdDetalleMovimiento = item.IdDetalleMovimiento;
                                dpv.Inatec = item.Inatec;
                                dpv.IR = item.IR;
                                dpv.Monto = item.Monto;
                                dpv.Obsevacion = item.Obsevacion;
                                dpv.Patronal = item.Patronal;
                                dpv.Reposo = item.Reposo;
                                dpv.Salario = item.Salario;
                                dpv.SalarioDiario = item.SalarioDiario;
                                dpv.SeguroSocial = item.SeguroSocial;
                                dpv.TipoCambio = item.TipoCambio;
                                dpv.TotalPagar = item.TotalPagar;
                                dpv.PagoVacaciones = pagoVacaciones.ID;
                                pagoVacaciones.DetallePagoVacaciones.Add(dpv);
                            } 
                            #endif

                                if (!editando)
                                {
                                    db.VacacionesPagadas.InsertOnSubmit(pagoVacaciones);
                                    //Eliminar los detalles anteriores
                                    //foreach (var item in pagoVacaciones.DetallePagoVacaciones)
                                    //{
                                    //    db.DetallePagoVacaciones.DeleteOnSubmit(item);
                                    //}
                                }
                                pagoVacaciones.Deduccion = 0;
                                if (gvVacaciones.RowCount > 0)
                                {
                                    for (int i = 0; i < gvVacaciones.RowCount; i++)
                                    {
                                        if (Convert.ToBoolean(gvVacaciones.GetRowCellValue(i, colSelected)))
                                        {
                                            SAGAS.Entidad.DetallePagoVacaciones dpv = new SAGAS.Entidad.DetallePagoVacaciones();
                                            var diasACuenta = db.DiasVacaciones.Single(dv => dv.ID == Convert.ToInt32(gvVacaciones.GetRowCellValue(i, colID)));
                                            dpv.Dias = Decimal.Round(diasACuenta.Dias, 2);
                                            dpv.DiasPago = Decimal.Round(diasACuenta.DiasPago, 2);
                                            dpv.FechaDesde = diasACuenta.FechaDesde;
                                            dpv.FechaHasta = diasACuenta.FechaHasta;
                                            dpv.FechaPago = diasACuenta.FechaPago;
                                            dpv.EmpleadoID = diasACuenta.EmpleadoID;
                                            dpv.DiasVacacionesID = diasACuenta.ID;
                                            dpv.VacacionesPagadasID = pagoVacaciones.ID;
                                            dpv.Salario = Decimal.Round(diasACuenta.Salario, 2);
                                            dpv.SalarioDiario = Decimal.Round(pagoVacaciones.SalarioDiario, 2);
                                            dpv.Monto = Decimal.Round(dpv.DiasPago * dpv.SalarioDiario, 2);
                                            //pagoVacaciones.DetallePagoVacacion.Add(dpv);
                                            pagoVacaciones.Deduccion += Decimal.Round(dpv.Monto, 2);
                                        }

                                    }
                                }
                                //pagoVacaciones.SubTotal = pagoVacaciones.Monto - pagoVacaciones.Deduccion;

                                db.SubmitChanges();

                                //#region Actualizar Fecha Ultima de pago de vacaciones

                                //SAGAS.Entidad.Empleado EM = db.Empleados.Single(empl => empl.ID == idEmpleado);

                                //if (EM.FechaUltimaVacacion == null)
                                //    EM.FechaUltimaVacacion = EM.FechaIngreso;

                                //int year, month, day;
                                ////AÑOS
                                //year = Convert.ToInt32(Math.Floor(diasPendientesSpinEdit.Value / 30m));

                                //decimal vmonth = Decimal.Round((diasPendientesSpinEdit.Value / 30m - Math.Floor(diasPendientesSpinEdit.Value / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);

                                //month = Convert.ToInt32(Math.Floor(vmonth));

                                //decimal vday = vmonth - Math.Floor(vmonth);

                                //day = Convert.ToInt32(Decimal.Round(vday * 30m, 0, MidpointRounding.AwayFromZero));

                                //DateTime ViejaFecha = EM.FechaUltimaVacacion.Value.Date;
                                //DateTime NuevaVacacion = EM.FechaUltimaVacacion.Value.Date.AddYears(year).AddMonths(month).AddDays(day);

                                //Entidad.UltimaFechaVacacion UFV = new Entidad.UltimaFechaVacacion();
                                //UFV.VacacionesPagadasID = pagoVacaciones.ID;
                                //UFV.MovimientoEmpleadoID = movimientoempleado.ID;
                                //UFV.AntiguaFechaUltimaVacacion = ViejaFecha;
                                //UFV.FechaUltimaVacacion = NuevaVacacion;
                                //UFV.EmpleadoID = EM.ID;

                                //db.UltimaFechaVacacions.InsertOnSubmit(UFV);
                                //db.SubmitChanges();

                                //EM.FechaUltimaVacacion = NuevaVacacion.Date;

                                //db.SubmitChanges();

                                //#endregion

                                #region Actualizar Fecha Ultima de pago de vacaciones

                                Entidad.Empleado EM = db.Empleados.Single(empl => empl.ID == idEmpleado);

                                if (EM.FechaUltimaVacacion == null)
                                    EM.FechaUltimaVacacion = EM.FechaIngreso;

                                int year, month, day;
                                //AÑOS
                                year = Convert.ToInt32(Math.Floor(diasVacacionesSpinEditDesc.Value / 30m));

                                decimal vmonth = Decimal.Round((diasVacacionesSpinEditDesc.Value / 30m - Math.Floor(diasVacacionesSpinEditDesc.Value / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);

                                month = Convert.ToInt32(Math.Floor(vmonth));

                                decimal vday = vmonth - Math.Floor(vmonth);

                                day = Convert.ToInt32(Decimal.Round(vday * 30m, 0, MidpointRounding.AwayFromZero));

                                DateTime ViejaFecha = EM.FechaUltimaVacacion.Value.Date;
                                DateTime NuevaVacacion = EM.FechaUltimaVacacion.Value.Date;

                                NuevaVacacion = NuevaVacacion.Date.AddYears(year);
                                NuevaVacacion = NuevaVacacion.Date.AddMonths(month);

                                int d = 30 - NuevaVacacion.Day;

                                if (!day.Equals(0))
                                {
                                    if (d < day)
                                    {
                                        day -= d;
                                        NuevaVacacion = NuevaVacacion.Date.AddMonths(1);
                                        NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, day);
                                    }
                                    else if (d > day)
                                    {
                                        NuevaVacacion = NuevaVacacion.Date.AddDays(day);
                                    }
                                    else
                                    {
                                        NuevaVacacion = NuevaVacacion.Date.AddMonths(1);
                                        NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, 1);
                                    }
                                }
                                //else
                                //    NuevaVacacion = NuevaVacacion.Date.AddDays(-1);




                                EM.FechaUltimaVacacion = NuevaVacacion.Date;

                                db.SubmitChanges();
                                #endregion
                            }
                            trans.Commit();
                            Parametros.General.DialogMsg(SAGAS.Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, SAGAS.Parametros.MsgType.message);
                            NextProvision = true;
                            RefreshMDI = true;
                            ShowMsg = true;
                            IDPrint = pagoVacaciones.ID;


                            if (!editando)
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                                    "Grabación de Pago de Vacaciones # " + pagoVacaciones.ID.ToString() + " para " + gvEmpleados.GetFocusedRowCellValue(colNombres).ToString()
                                    + " " + gvEmpleados.GetFocusedRowCellValue(colApellidos).ToString()
                                    + ", Desde: " + pagoVacaciones.FechaDesde.ToShortDateString() + " hasta: " + pagoVacaciones.FechaHasta.ToShortDateString()
                                    + ", Monto: +" + pagoVacaciones.Monto.ToString(), this.Name);
                            else
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                               "Modificacion de Pago de Vacaciones # " + pagoVacaciones.ID.ToString() + " para " + gvEmpleados.GetFocusedRowCellValue(colNombres).ToString()
                               + " " + gvEmpleados.GetFocusedRowCellValue(colApellidos).ToString()
                               + ", Desde: " + pagoVacaciones.FechaDesde.ToShortDateString() + " hasta: " + pagoVacaciones.FechaHasta.ToShortDateString()
                               + ", Monto: +" + pagoVacaciones.Monto.ToString(), this.Name);

                            /*if (!Parametros.General.PagoVacacionesPlanilla())
                                if (!editando) ShowReportVacaciones();*/
                            #endregion
                        }
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());

            }
            finally
            {
                db.Connection.Close();
            }
        }

        private void diasCuentaVacacionesSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (loaded && empleado != null)
            {
                decimal salarioDiario = Decimal.Round(empleado.SalarioActual / Parametros.General.diasMes,2);
                montoVacacionesSpinEdit.EditValue = Decimal.Round(salarioDiario * diasDescansadosSpinEdit.Value,2);
                diasPendientesSpinEdit.EditValue = Decimal.Round(Decimal.Round(Convert.ToDecimal(diasVacacionesSpinEdit.EditValue), 2) - Decimal.Round(Convert.ToDecimal(diasDescansadosSpinEdit.EditValue), 2), 2);
            }
        }

        private void diasVacacionesSpinEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (loaded && empleado != null)
                {
                    decimal SalarioDia = Decimal.Round((empleado.SalarioActual / Parametros.General.diasMes), 2);

                    switch (Convert.ToInt32(diasVacacionesSpinEdit.Value))
                    {
                        case 15:
                            montoAcumuladoSpinEdit.EditValue = Decimal.Round(empleado.SalarioActual / 2, 2, MidpointRounding.AwayFromZero);
                            break;
                        case 30:
                            montoAcumuladoSpinEdit.EditValue = Decimal.Round(empleado.SalarioActual, 2, MidpointRounding.AwayFromZero);
                            break;
                        case 45:
                            montoAcumuladoSpinEdit.EditValue = Decimal.Round(empleado.SalarioActual * 1.5m, 2, MidpointRounding.AwayFromZero);
                            break;
                        case 60:
                            montoAcumuladoSpinEdit.EditValue = Decimal.Round(empleado.SalarioActual * 2, 2, MidpointRounding.AwayFromZero);
                            break;
                        default:
                            montoAcumuladoSpinEdit.EditValue = Decimal.Round(SalarioDia * diasVacacionesSpinEdit.Value, 2, MidpointRounding.AwayFromZero);

                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }  

        private void tswVacaDesPag_Toggled(object sender, EventArgs e)
        {
            if (tswVacaDesPag.IsOn)
            {
                descansada = false;
                scDatos.PanelVisibility = SplitPanelVisibility.Panel2;
            }
            else
            {
                descansada = true;
                scDatos.PanelVisibility = SplitPanelVisibility.Panel1;
            }
        }

        private void wizard_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }
        
        private void wizPagoVacaciones_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
        }

        //Dias en Reposo
        private void chDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chDay.Checked)
            {
                diasVacacionesSpinEditDesc.Enabled = true;
                diasVacacionesSpinEditDesc.Value = 1;
            }

            else if (!chDay.Checked)
            {
                diasVacacionesSpinEditDesc.Enabled = false;
                diasVacacionesSpinEditDesc.EditValue = null;
            }
        }

        //Horas en Reposo
        private void chHour_CheckedChanged(object sender, EventArgs e)
        {
            if (chHour.Checked)
            {
                panelchHoras.Visible = true;
                timeInicio.Enabled = true;
                timeFin.Enabled = true;
                timeInicio.EditValue = DateTime.Parse("08:00:00");
                timeFin.EditValue = DateTime.Parse("08:00:00");
            }

            else if (!chHour.Checked)
            {
                panelchHoras.Visible = false;
                timeInicio.Enabled = false;
                timeFin.Enabled = false;
                timeInicio.EditValue = null;
                timeFin.EditValue = null;

            }
        }

        //Por Horas
        private void chHoraSumar_CheckedChanged(object sender, EventArgs e)
        {
            if (chHoraSumar.Checked) { chHoraReferencia.Checked = false; }

            else if (!chHoraSumar.Checked) { chHoraReferencia.Checked = true; }
        }

        //Por Días
        private void chHoraReferencia_CheckedChanged(object sender, EventArgs e)
        {
            if (chHoraReferencia.Checked) { chHoraSumar.Checked = false; }

            else if (!chHoraReferencia.Checked) { chHoraSumar.Checked = true; }
        }

        //Motivos Programadas
        private void chProgramada_CheckedChanged(object sender, EventArgs e)
        {
            if (chProgramada.Checked)
            {
                chPersonal.Checked = chOtros.Checked = false;
            }
        }

        //Motivos Personal
        private void chPersonal_CheckedChanged(object sender, EventArgs e)
        {
            if (chPersonal.Checked)
            {
                chProgramada.Checked = chOtros.Checked = false;
            }
        }

        //Motivos Otros
        private void chOtros_CheckedChanged(object sender, EventArgs e)
        {
            if (chOtros.Checked)
            {
                chProgramada.Checked = chPersonal.Checked = false;
                memoOtros.Enabled = true;
            }

            else if (!chOtros.Checked)
            {
                memoOtros.Enabled = false;
                memoOtros.EditValue = null;
            }
        }

        private void dateFechaCorte_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                var empsel = db.Empleados.Select(s => new { s.ID, s.FechaUltimaVacacion, s.FechaIngreso }).SingleOrDefault(o => o.ID.Equals(idEmpleado));

                if (empsel != null)
                {
                    decimal total = 0;
                    if (empsel.FechaUltimaVacacion != null)
                    {
                        total = GetTotalHoliday(Convert.ToDateTime(empsel.FechaUltimaVacacion), dateFechaCorte.DateTime.Date);
                    }
                    else if (empsel.FechaUltimaVacacion == null)
                    {
                        //empsel.FechaUltimaVacacion = empsel.FechaIngreso;
                        total = GetTotalHoliday(Convert.ToDateTime(empsel.FechaIngreso), dateFechaCorte.DateTime.Date);
                    }
                    speRemainingDays.Value = Math.Round(total, 2, MidpointRounding.AwayFromZero);//midpoint referencia de redondeo[awayfromzero] a partir del 0.5 y no del 0.6
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());

            }
        }

        #endregion

    }
}
