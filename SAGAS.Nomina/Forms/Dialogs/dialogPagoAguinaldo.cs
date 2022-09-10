using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class dialogPagoAguinaldo : Form
    {
         #region Declaraciones

        private SAGAS.Entidad.SAGASDataClassesDataContext db;
        internal FormPagoAguinaldo MDI;
        private DataTable dtEmpleados;
        private bool RefreshMDI = false;

        #endregion

        #region Inicializacion

        public dialogPagoAguinaldo()
        {
            InitializeComponent();
            db = new SAGAS.Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            ValidarerrRequiredField();
        }

        private void dialogPagoAguinaldo_Load(object sender, EventArgs e)
        {
            var query = db.Empresas;
            if (query.Count() > 0)
            {
                empresaLookUpEdit.Properties.DataSource = query;
                empresaLookUpEdit.Properties.DisplayMember = "Nombre";
                empresaLookUpEdit.Properties.ValueMember = "ID";
                empresaLookUpEdit.ItemIndex = 0;
            }

            fechaInicialDateEdit.EditValue = Parametros.Config.InicioAguinaldo();
            fechaFinalDateEdit.EditValue = Parametros.Config.FinAguinaldo();

        }

        #endregion

        #region Metodos

        #region Pagar Aguinaldo

        private int PagarAguinaldo()
        {
            try
            {
                int idGenerado = 0;

                if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();
                db.CommandTimeout = 600;

                #region Using
                using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
                {
                    try
                    {
                        if (gvForm.RowCount > 0)
                        {

                            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                            SAGAS.Entidad.PagoAguinaldo pagoAguinaldo = new SAGAS.Entidad.PagoAguinaldo();
                            pagoAguinaldo.FechaInicio = fechaInicialDateEdit.DateTime.Date;
                            pagoAguinaldo.FechaFinal = fechaFinalDateEdit.DateTime.Date;
                            pagoAguinaldo.PlanillaID = Convert.ToInt32(planillaLookUpEdit.EditValue);
                            pagoAguinaldo.Aplicado = false;

                            db.Transaction = trans;
                            db.PagoAguinaldos.InsertOnSubmit(pagoAguinaldo);
                            db.SubmitChanges(); // Retornar el Id de Pago de Aguinaldo

                            gvForm.ClearColumnsFilter();

                            DateTime pFecha = Convert.ToDateTime(fechaInicialDateEdit.DateTime).Date;
                            DateTime uFecha = Convert.ToDateTime(fechaFinalDateEdit.DateTime).Date;

                            for (int i = 0; i < gvForm.RowCount; i++)
                            {
                                if (Convert.ToBoolean(gvForm.GetRowCellValue(i, colSelected)).Equals(true))
                                {
                                    #region NUEVO DETALLE DE PAGO DE AGUINALDO

                                    var empleado = db.Empleados.SingleOrDefault(em => em.ID == Convert.ToInt32(gvForm.GetRowCellValue(i, colIdEmpleado)));
                                    if (empleado != null)
                                    {
                                        DateTime fechaDesde = empleado.FechaUltimoAguinaldo.Equals(null) ? pagoAguinaldo.FechaInicio : Convert.ToDateTime(empleado.FechaUltimoAguinaldo);

                                        if (empleado.FechaIngreso > fechaDesde)
                                        {
                                            fechaDesde = empleado.FechaIngreso.Value.Date;
                                        }

                                        SAGAS.Entidad.DetallePagoAguinaldo detallePago = new SAGAS.Entidad.DetallePagoAguinaldo();

                                        detallePago.PagoAguinaldoID = pagoAguinaldo.ID;
                                        detallePago.EmpleadoID = Convert.ToInt32(gvForm.GetRowCellValue(i, colIdEmpleado));
                                        detallePago.SalarioMensual = Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo));

                                        detallePago.DiasPagados = Convert.ToInt32((pagoAguinaldo.FechaFinal - fechaDesde).TotalDays);

                                        int days = 0, month = 0, year = 0;

                                        //Parametros.General.RangoFechasYMD(pagoAguinaldo.FechaFinal.Date, fechaDesde.Date, out year, out month, out days, 1);

                                        //decimal diasAguinaldo = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2);
                                        //diasAguinaldo += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2);
                                        //diasAguinaldo += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2);

                                        //detallePago.Monto = Decimal.Round((detallePago.SalarioMensual / 30) * diasAguinaldo, 2);


                                        DateTime aFecha = (empleado.FechaUltimoAguinaldo.HasValue ? Convert.ToDateTime(empleado.FechaUltimoAguinaldo.Value.Date) : empleado.FechaIngreso.Value.Date);

                                        if (aFecha <= pFecha)
                                            year = 1;
                                        else
                                        {
                                            if (aFecha.Date.Month.Equals(12))
                                                month = uFecha.Date.Month;
                                            else
                                                month = uFecha.Date.Month - aFecha.Date.Month;

                                            days = uFecha.Date.Day - aFecha.Date.Day + 1;
                                        }

                                        decimal Monto = 0;

                                        Monto += Decimal.Round(Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) * year, 2, MidpointRounding.AwayFromZero);
                                        Monto += Decimal.Round(Decimal.Round((Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) / 12m), 2, MidpointRounding.AwayFromZero) * month, 2, MidpointRounding.AwayFromZero);
                                        Monto += Decimal.Round(Decimal.Round(Decimal.Round((Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) / 12m), 2, MidpointRounding.AwayFromZero) / 30m, 2, MidpointRounding.AwayFromZero) * days, 2, MidpointRounding.AwayFromZero);

                                        detallePago.Monto = Monto;
                                        
                                        detallePago.FechaFin = pagoAguinaldo.FechaFinal;
                                        detallePago.FechaInicio = fechaDesde;

                                    #endregion

                                        #region Adelantos

                                        /*================================================================
                                     ADELANTO
                                     ================================================================*/
                                        var adelanto = from d in db.AdelantoAguinaldos
                                                       where d.EmpleadoID == detallePago.EmpleadoID
                                                       && (d.FechaRegistro >= pagoAguinaldo.FechaInicio && d.FechaRegistro <= pagoAguinaldo.FechaFinal)
                                                       select d;

                                        if (adelanto.Count() > 0)
                                        {
                                            detallePago.Adelanto = adelanto.Sum(vi => vi.Monto);
                                        }

                                        detallePago.Adelanto += Convert.ToDecimal(gvForm.GetRowCellValue(i, colDeducciones));

                                        #endregion

                                        detallePago.Total = detallePago.Monto - detallePago.Adelanto;

                                        db.DetallePagoAguinaldos.InsertOnSubmit(detallePago);

                                        pagoAguinaldo.Total += detallePago.Total;
                                    }
                                }
                            }//FIN DEL FOR

                            db.SubmitChanges();
                            trans.Commit();
                            idGenerado = pagoAguinaldo.ID;
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        }
                        else
                        {
                            Parametros.General.DialogMsg("No existe ningun empleado seleccionado", SAGAS.Parametros.MsgType.warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        trans.Rollback();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                    }
                    finally
                    {
                        db.Connection.Close();
                    }
                }
                #endregion

                return idGenerado;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());
                return 0;
            }
        }

        #endregion

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(empresaLookUpEdit, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(fechaInicialDateEdit, ErrorProvider);
            Parametros.General.ValidateEmptyStringRule(fechaFinalDateEdit, ErrorProvider);
        }

        private bool ValidarDatos()
        {
            if (empresaLookUpEdit.EditValue == null)
            {
                Parametros.General.ValidateEmptyStringRule(empresaLookUpEdit, ErrorProvider);
                return false;
            }
            if (fechaInicialDateEdit.EditValue == null)
            {
                Parametros.General.ValidateEmptyStringRule(fechaInicialDateEdit, ErrorProvider);
                return false;
            }
            if (fechaFinalDateEdit.EditValue == null)
            {
                Parametros.General.ValidateEmptyStringRule(fechaFinalDateEdit, ErrorProvider);
                return false;
            }
            dtEmpleados.DefaultView.RowFilter = "Selected = 'true'";
            if (dtEmpleados.DefaultView.Count <= 0)
            {
                Parametros.General.DialogMsg("Favor seleccionar al empleado.", SAGAS.Parametros.MsgType.warning);
                return false;
            }

            dtEmpleados.DefaultView.RowFilter = "";


            return true;
        }

        public System.Data.DataTable ToDataTable(object query)
        {
            //try
            //{
            //    if (query == null)
            //        throw new ArgumentNullException("Consulta no especificada!");

            //    System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            //    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            //    adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            //    System.Data.DataTable dt = new System.Data.DataTable("sd");



            //    cmd.Connection.Open();
            //    adapter.FillSchema(dt, System.Data.SchemaType.Source);
            //    adapter.Fill(dt);

            //    return dt;
            //}
            //catch (Exception ex)
            //{ Parametros.General.DialogMsg(Properties.Resources.MSG_ERROR, SAGAS.Classes.MsgType.error, ex.ToString());
            //return null;
            //}
            ////finally
            ////{
            ////    cmd.Connection.Close();
            ////}


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

        private bool PlanillaAprobada()
        {
            DateTime fechaHasta = fechaFinalDateEdit.DateTime.Date, fechaDesde = fechaInicialDateEdit.DateTime.Date;
            var plGn = db.PagoAguinaldos.Where(pl => pl.FechaInicio == fechaDesde && pl.FechaFinal == fechaHasta && pl.Aplicado == true);
            if (plGn.Count() > 0)
            {
                Parametros.General.DialogMsg("La Planilla no se puede generar debido a que ya se aprobo el pago de aguinaldo para este periodo.", SAGAS.Parametros.MsgType.warning);
                return true;
            }
            return false;

        }

        private void ShowReportePlanilla(int idPagoAguinaldo)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaPagoAguinaldos.Where(m => m.ID.Equals(idPagoAguinaldo)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Aguinaldo " + VM.First().FechaFin.ToShortDateString();

                SAGAS.Reportes.Nomina.Hojas.PlanillaAguinaldo rep = new SAGAS.Reportes.Nomina.Hojas.PlanillaAguinaldo();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);

                DateTime fi = Convert.ToDateTime(fechaInicialDateEdit.DateTime.Date), ff = Convert.ToDateTime(fechaFinalDateEdit.DateTime.Date);
                string titulo = "Periodo desde  " + fi.ToShortDateString() + "  al  " + ff.ToShortDateString();
                rep.lbltitulo.Text = titulo;
                rep.picLogo.Image = picture_LogoEmpresa;
                rep.lblEmpresa.Text = Nombre;
                rep.lblRuc.Text = Ruc;
                rep.lblDireccion.Text = Direccion;

                rep.DataSource = VM;

                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                rv.Show();


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion 

        #region Eventos
    
        private void ckTodasLasPlanillas_CheckedChanged(object sender, EventArgs e)
        {
            
            grid.DataSource = null;
            if (ckTodasLasPlanillas.Checked)
            {
                if (empresaLookUpEdit.EditValue != null)
                {
                    //var q = from empl in db.Empleado
                    //            join eo in db.EstructuraOrganizativa
                    //            on empl.IdEO equals eo.IdEO
                    //            where empl.Activo && eo.IdEmpresa == Convert.ToInt32(empresaLookUpEdit.EditValue)
                    //            select new
                    //            {
                    //                empl.IdEmpleado,
                    //                empl.Nombres,
                    //                empl.Apellidos,
                    //                Selected = empl.Activo,
                    //                empl.FechaIngreso,
                    //                empl.SalarioActual,
                    //                Cargo = empl.IdCargo,
                    //                empl.Codigo,

                    //            };

                    var query = from ga in db.GetAguinaldo(Convert.ToInt32(empresaLookUpEdit.EditValue), Convert.ToInt32(planillaLookUpEdit.EditValue), ckTodasLasPlanillas.Checked, Convert.ToDateTime(fechaFinalDateEdit.EditValue)).AsQueryable()
                                select new
                                {
                                    ga.ID,
                                    ga.Nombres,
                                    ga.Apellidos,
                                    ga.Selected,
                                    ga.FechaIngreso,
                                    ga.SalarioActual,
                                    ga.Cargo,
                                    ga.Codigo,
                                    ga.SalarioMaximo,
                                    ga.Deducciones,
                                    ga.TotalRecibir
                                };

                    dtEmpleados = SAGAS.Parametros.General.LINQToDataTable(query);

                    grid.DataSource = dtEmpleados;
                    planillaLookUpEdit.Enabled = false;
                }
            }
            else
            {
                planillaLookUpEdit.Enabled = true;
                if (planillaLookUpEdit.EditValue != null)
                {
                    //var query = from empl in db.Empleado
                    //            where empl.Activo && empl.IdPlanilla == Convert.ToInt32(planillaLookUpEdit.EditValue)
                    //            select new
                    //            {
                    //                empl.IdEmpleado,
                    //                empl.Nombres,
                    //                empl.Apellidos,
                    //                Selected = empl.Activo,
                    //                empl.FechaIngreso,
                    //                empl.SalarioActual,
                    //                Cargo = empl.IdCargo,
                    //                empl.Codigo
                    //            };

                    var query = from ga in db.GetAguinaldo(Convert.ToInt32(empresaLookUpEdit.EditValue), Convert.ToInt32(planillaLookUpEdit.EditValue), ckTodasLasPlanillas.Checked, Convert.ToDateTime(fechaFinalDateEdit.EditValue)).AsQueryable()
                                select new
                                {
                                    ga.ID,
                                    ga.Nombres,
                                    ga.Apellidos,
                                    ga.Selected,
                                    ga.FechaIngreso,
                                    ga.SalarioActual,
                                    ga.Cargo,
                                    ga.Codigo,
                                    ga.SalarioMaximo,
                                    ga.Deducciones,
                                    ga.TotalRecibir
                                };

                    dtEmpleados = SAGAS.Parametros.General.LINQToDataTable(query);
                    grid.DataSource = dtEmpleados;
                }
            }
        }

        private void planillaLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (planillaLookUpEdit.EditValue != null)
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                    grid.DataSource = null;
                    int pl = Convert.ToInt32(planillaLookUpEdit.EditValue);

                    DateTime vFecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Date.Year, 06, 01);

                    var query = from dp in db.DetallePlanillaGenerada
                                join pg in db.PlanillaGenerada on dp.PlanillaGeneradaID equals pg.ID
                                join em in db.Empleados on dp.EmpleadoID equals em.ID
                                where em.Activo && em.PlanillaID.Equals((int)planillaLookUpEdit.EditValue) && pg.FechaDesde.Date >= vFecha
                                group dp by new { pg.FechaHasta.Month, em.ID, em.CalculoSalarioPromedioAguinaldo, em.Nombres, em.Apellidos, Selected = em.Activo, em.FechaIngreso, em.SalarioActual, em.Codigo, SalarioMaximo = em.SalarioActual, Deducciones = 0.00m, TotalRecibir = em.SalarioActual } into gr
                                select new
                                {
                                    IdEmpleado = gr.Key.ID,
                                    Nombres = gr.Key.Nombres,
                                    Apellidos = gr.Key.Apellidos,
                                    Selected = gr.Key.Selected,
                                    FehaIngreso = gr.Key.FechaIngreso,
                                    SalarioActual = gr.Key.SalarioActual,
                                    Codigo = gr.Key.Codigo,
                                    SalarioMaximo = (gr.Key.CalculoSalarioPromedioAguinaldo ? gr.Sum(s => s.TotalIngresos) /* db.PlanillaGenerada.Where(p => gr.Select(s => s.PlanillaGeneradaID).Contains(p.ID)).GroupBy(g => new {g.FechaHasta.Month, Suma = g.DetallePlanillaGenerada.Sum(s => s.TotalIngresos)}).Max(m => m.Key.Suma)*/ : gr.Key.SalarioActual),
                                    Deducciones = gr.Key.Deducciones,
                                    TotalRecibir = (gr.Key.CalculoSalarioPromedioAguinaldo ? gr.Sum(s => s.TotalIngresos) : gr.Key.SalarioActual)
                                };

                    var grupo = query.GroupBy(g => new { g.IdEmpleado, g.Nombres, g.Apellidos, g.Selected, g.FehaIngreso, g.SalarioActual, g.Codigo, g.Deducciones }).Select(s => new
                        {
                            s.Key.IdEmpleado,
                            s.Key.Nombres,
                            s.Key.Apellidos,
                            s.Key.Selected,
                            s.Key.FehaIngreso,
                            s.Key.SalarioActual,
                            s.Key.Codigo,
                            SalarioMaximo = s.Max(m => m.SalarioMaximo),
                            s.Key.Deducciones,
                            TotalRecibir = s.Max(m => m.TotalRecibir)

                        });

                    dtEmpleados = SAGAS.Parametros.General.LINQToDataTable(grupo);
                    grid.DataSource = dtEmpleados;
                    DateTime pFecha = Convert.ToDateTime(fechaInicialDateEdit.DateTime).Date;
                    DateTime uFecha = Convert.ToDateTime(fechaFinalDateEdit.DateTime).Date;

                    for (int i = 0; i < gvForm.RowCount; i++)
                    {
                        DateTime pagoAguinaldoFechaInicio = fechaInicialDateEdit.DateTime.Date;

                        var empleado = db.Empleados.SingleOrDefault(em => em.ID == Convert.ToInt32(gvForm.GetRowCellValue(i, colIdEmpleado)));

                        if (empleado != null)
                        {
                            DateTime fechaDesde = empleado.FechaUltimoAguinaldo.Equals(null) ? pagoAguinaldoFechaInicio : Convert.ToDateTime(empleado.FechaUltimoAguinaldo);

                            int days = 0, month = 0, year = 0;

                            //Parametros.General.RangoFechasYMD(fechaFinalDateEdit.DateTime.Date, fechaDesde.Date, out year, out month, out days, 1);

                            //decimal diasAguinaldo = Decimal.Round(year * Parametros.General.valorAnualVacaciones, 2);
                            //diasAguinaldo += Decimal.Round(month * Parametros.General.valorMensualVacaciones, 2);
                            //diasAguinaldo += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2);

                            //decimal Monto = Decimal.Round((Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) / 30) * diasAguinaldo, 2);
                            
                            DateTime aFecha = (empleado.FechaUltimoAguinaldo.HasValue ? Convert.ToDateTime(empleado.FechaUltimoAguinaldo.Value.Date) : empleado.FechaIngreso.Value.Date);

                            if (aFecha <= pFecha)
                                year = 1;
                            else
                            {
                                if (aFecha.Date.Month.Equals(12))
                                    month = uFecha.Date.Month;
                                else
                                    month = uFecha.Date.Month - aFecha.Date.Month;

                                days = uFecha.Date.Day - aFecha.Date.Day + 1;
                            }

                            decimal Monto = 0;

                            Monto += Decimal.Round(Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) * year, 2, MidpointRounding.AwayFromZero);
                            Monto += Decimal.Round(Decimal.Round((Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) / 12m), 2, MidpointRounding.AwayFromZero) * month, 2, MidpointRounding.AwayFromZero);
                            Monto += Decimal.Round(Decimal.Round(Decimal.Round((Convert.ToDecimal(gvForm.GetRowCellValue(i, colSalarioMaximo)) / 12m), 2, MidpointRounding.AwayFromZero) / 30m, 2, MidpointRounding.AwayFromZero) * days, 2, MidpointRounding.AwayFromZero);

                            this.gvForm.SetRowCellValue(i, "TotalRecibir", Monto);
                        }
                    }


                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //private void btnSelectAll_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < gvForm.RowCount; i++)
        //    {
        //        gvForm.SetRowCellValue(i, "Selected", true);
        //    }
        //}

        //private void btnUnSelectAll_Click(object sender, EventArgs e)
        //{
        //    for (int i = 0; i < gvForm.RowCount; i++)
        //    {
        //        gvForm.SetRowCellValue(i, "Selected", false);
        //    }
        //}

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (ValidarDatos())
                {
                    if (!PlanillaAprobada())
                    {
                        if (Parametros.General.DialogMsg("¿Seguro continuar con el proceso?", SAGAS.Parametros.MsgType.question)
                            == DialogResult.OK)
                        {
                            this.Activate();
                            this.Cursor = Cursors.WaitCursor;
                            int numeroPlanilla = PagarAguinaldo();
                            this.Cursor = Cursors.Default;

                            if (numeroPlanilla > 0)
                            {
                                if (Parametros.General.DialogMsg("Proceso completado exitosamente.\nDesea ver la vista previa de impresión ahora?", SAGAS.Parametros.MsgType.question)
                                    == DialogResult.OK)
                                {
                                    #region Agregar Bitacora
                                    Parametros.General.AddLogBook(db, SAGAS.Parametros.TipoAccion.Nomina,
                                        "Generacion de Nueva Planilla de aguinaldo #" + numeroPlanilla.ToString()
                                        + ", Correspondiente a: " + planillaLookUpEdit.Text
                                        + " del " + fechaInicialDateEdit.DateTime.ToShortDateString() + " al " + fechaFinalDateEdit.DateTime.ToShortDateString(),this.Name);
                                    #endregion

                                    ShowReportePlanilla(numeroPlanilla);
                                }


                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        } 
        

        private void empresaLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            grid.DataSource = null;
            if (empresaLookUpEdit.EditValue != null)
            {

                var query = from pl in db.Planillas
                            join eo in db.EstructuraOrganizativa
                            on pl.EstructuraOrganizativaID equals eo.ID
                            where eo.EmpresaID == Convert.ToInt32(empresaLookUpEdit.EditValue)
                            select pl;
                planillaLookUpEdit.Properties.DataSource = query;
                planillaLookUpEdit.Properties.DisplayMember = "Nombre";
                planillaLookUpEdit.Properties.ValueMember = "ID";
            }
        }


        private void gvForm_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colSalarioMaximo)
                {
                    decimal Total = 0, TotalRecibir = 0, Deducciones = 0;

                    if (gvForm.GetFocusedRowCellValue("TotalRecibir") != null)
                        TotalRecibir = Convert.ToDecimal(gvForm.GetFocusedRowCellValue("TotalRecibir"));

                    if (gvForm.GetFocusedRowCellValue("Deducciones") != null)
                        Deducciones = Convert.ToDecimal(gvForm.GetFocusedRowCellValue("Deducciones"));

                    Total = Decimal.Round((TotalRecibir - Deducciones), 2);

                    gvForm.SetFocusedRowCellValue("TotalRecibir", Total);

                }
                if (e.Column == colDeducciones)
                {
                    decimal Total = 0, TotalRecibir = 0, Deducciones = 0;

                    if (gvForm.GetFocusedRowCellValue("TotalRecibir") != null)
                        TotalRecibir = Convert.ToDecimal(gvForm.GetFocusedRowCellValue("TotalRecibir"));

                    if (gvForm.GetFocusedRowCellValue("Deducciones") != null)
                        Deducciones = Convert.ToDecimal(gvForm.GetFocusedRowCellValue("Deducciones"));

                    Total = Decimal.Round((TotalRecibir - Deducciones), 2);

                    gvForm.SetFocusedRowCellValue("TotalRecibir", Total);
                }

            }
            catch (Exception ex)
            { Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString()); }
        }

        private void tglSelect_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (tglSelect.IsOn)
                {
                    for (int i = 0; i < gvForm.RowCount; i++)
                    {
                        gvForm.SetRowCellValue(i, "Selected", true);
                    }
                }
                else if (!tglSelect.IsOn)
                {
                    for (int i = 0; i < gvForm.RowCount; i++)
                    {
                        gvForm.SetRowCellValue(i, "Selected", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dialogPagoAguinaldo_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(RefreshMDI);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
        #endregion