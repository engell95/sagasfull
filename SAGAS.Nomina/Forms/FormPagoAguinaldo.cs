using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms
{
    public partial class FormPagoAguinaldo : Parametros.Forms.FormBase
    {
        #region Declaraciones & Inicializacion
        private Forms.Dialogs.dialogPagoAguinaldo nf;
        private SAGAS.Entidad.SAGASDataClassesDataContext db;
        private SAGAS.Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        public FormPagoAguinaldo()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Descartar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }
        
        #endregion

        #region Metodos

        protected override void FillControl(bool refresh)
        {
            try
            {
                if(db == null)
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                if (dv == null)
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                var pagos = from p in db.PagoAguinaldos
                            join pl in db.Planillas
                            on p.PlanillaID equals pl.ID
                            select new { p.ID, p.FechaInicio, p.FechaFinal, p.PlanillaID, p.Total, p.Aplicado, NombrePlanilla = pl.Nombre };
                grid.DataSource = pagos;
                gvForm.ExpandAllGroups();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void Imprimir()
        {
            if (gvForm.FocusedRowHandle >= 0)
            {
                ShowReportePlanilla(Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
            }
            //this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        protected override void CleanFilter()
        {
            this.gvForm.ActiveFilter.Clear();
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl(true);
        }

        private void RefreshData()
        {
            FillControl(true);
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

                DateTime fi = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaInicio)), ff = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaFinal));
            string titulo = "NÓMINA DÉCIMO TERCER MES DEL PERÍODO: " + fi.ToShortDateString() + "  Al  " + ff.ToShortDateString();
            rep.lbltitulo.Text = titulo;
               rep.picLogo.Image = picture_LogoEmpresa;
               rep.lblEmpresa.Text = Nombre;
               rep.lblRuc.Text = Ruc;
               rep.lblDireccion.Text = Direccion;
            //    rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

            //    rep.CeTitulo.Text = (VM.First().MovimientoTipoID.Equals(7) ? "Comprobante de Venta" : "Anulación de Venta");

                rep.DataSource = VM;

                //rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
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
            ////SAGAS.Entidad.DataManager dManager = new SAGAS.Entidad.DataManager(Parametros.Config.GetCadenaConexionString());
            //SAGAS.Reportes.Nomina.Hojas.PlanillaAguinaldo rep = new SAGAS.Reportes.Nomina.Hojas.PlanillaAguinaldo();
            //DateTime fi = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaInicio)), ff = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaFinal));
            //string titulo = "Periodo desde " + fi.ToShortDateString() + " al " + ff.ToShortDateString();
            //rep.lbltitulo.Text = titulo;
            //var lpa = dv.GetListPagoAguinaldo(idPagoAguinaldo);
            //DataTable dt = Parametros.General.LINQToDataTable(lpa);
            //if (dt.Rows.Count > 0)
            //{
            //    rep.DataSource = dt;
            //    rep.Logo = Parametros.General.LogoEmpresa(Convert.ToInt32(dt.Rows[0]["IdEmpresa"]), db);
            //    Parametros.General.ShowReport(rep, "Pago Aguinaldo, " + titulo);
            //}
            //else
            //{
            //    Parametros.General.DialogMsg("Ha ocurrido un problema al intetar cargar el reporte. Por favor comuniquese con su proveedor", SAGAS.General.MsgType.warning);
            //}
        //}

        private void ShowReporteComprobantes(int idPagoAguinaldo)
        {
            //SAGAS.Entidad.DataManager dManager = new SAGAS.Entidad.DataManager(Parametros.General.GetConnectionString());
            //DataTable dt = dManager.GetListPagoAguinaldo(idPagoAguinaldo);
            //SAGAS.Reportes.Aguinaldo.comprobanteAguinaldo rep = new SAGAS.Reportes.Aguinaldo.comprobanteAguinaldo(dt, idPagoAguinaldo);
            //DateTime fi = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaInicio)), ff = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaFinal));
            //string titulo = "Periodo desde " + fi.ToShortDateString() + " al " + fi.ToShortDateString();
            //rep.lbltitulo.Text = titulo;
            //if (dt.Rows.Count > 0)
            //{
            //    rep.DataSource = dt;
            //    rep.Logo = Parametros.General.LogoEmpresa(Convert.ToInt32(dt.Rows[0]["IdEmpresa"]), db);
            //    Parametros.General.ShowReport(rep, "Comprobantes de pagos del " + fi.ToShortDateString() + " al " + fi.ToShortDateString());
            //}
            //else
            //{
            //    Parametros.General.DialogMsg("Ha ocurrido un problema al intetar cargar el reporte. Por favor comuniquese con su proveedor", SAGAS.General.MsgType.warning);
            //}
       

        }

        private void ShowFormatoBanco()
        {
            
                try
                {
                    //SAGAS.Entidad.DataManager dManager = new SAGAS.Entidad.DataManager(Parametros.General.GetConnectionString());
                    //var plan = db.PagoAguinaldo.Single(p => p.ID == Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));

                    //string titulo = "INVERSIONES ZANZIBAR S.A." + Environment.NewLine + Convert.ToString(db.Planillas.Single(p => p.ID == Convert.ToInt32(plan.PlanillaID)).Nombre) +
                    //            Environment.NewLine + "Archivo plano para el banco aguinaldo del " + Parametros.General.CurrentCulture.DateTimeFormat.GetMonthName(plan.FechaInicio.Month) + " del año " +
                    //            plan.FechaInicio.Year.ToString() + " al "
                    //             + Parametros.General.CurrentCulture.DateTimeFormat.GetMonthName(plan.FechaFinal.Month) + " del año " + plan.FechaFinal.Year.ToString();


                    //SAGAS.Reportes.ReporteCuentaBDFAguinaldo rep = new SAGAS.Reportes.ReporteCuentaBDFAguinaldo();
                    //rep.lblConcepto.Text = titulo;

                    //DataTable result;

                    //result = Parametros.General.LINQToDataTable(dv.VistaCuentaTarjetaBDFAguinaldo.Where(o => o.ID == Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID))));

                    //rep.DataSource = result;
                    //Parametros.General.ShowReport(rep, "Archivo plano para el banco ");


                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.ToString());
                }
            
        }

        private bool PermiteAprobar(int idPlanilla, DateTime fi, DateTime ff)
        {
            var pagoAguinaldo = db.PagoAguinaldos.Where(pa => pa.Aplicado && pa.FechaInicio.Date == fi.Date && pa.FechaFinal.Date == ff.Date && pa.PlanillaID == idPlanilla);
            if (pagoAguinaldo.Count() > 0)
                return false;
            return true;
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.dialogPagoAguinaldo();
                    nf.Text = "Crear Pago de Aguinaldo";
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Del()
        {
            try
            {
                if (gvForm.FocusedRowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR, SAGAS.Parametros.MsgType.question)
                                == DialogResult.OK)
                    {

                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        var pagoAguinaldo = db.PagoAguinaldos.Single(pa => pa.ID == Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
                        db.PagoAguinaldos.DeleteOnSubmit(pagoAguinaldo);
                        db.SubmitChanges();
                        FillControl(true);
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region Eventos

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            if (gvForm.FocusedRowHandle >= 0)
            {
                ShowReportePlanilla(Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
            }
        }

        private void rpChkAprobado_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvForm.FocusedRowHandle >= 0)
                {
                    if (!Convert.ToBoolean(gvForm.GetFocusedRowCellValue(colAprobado)))
                    {
                        int idPagoAguinaldo = Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)), idPlanilla
                            = Convert.ToInt32(gvForm.GetFocusedRowCellValue(colIdPlanilla));
                        DateTime fi = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaInicio)), ff
                            = Convert.ToDateTime(gvForm.GetFocusedRowCellValue(colFechaFinal));
                        if (PermiteAprobar(idPlanilla, fi, ff))
                        {
                            if (Parametros.General.DialogMsg("¿Está seguro de aprobar la nómina de Aguinaldo seleccionada?", SAGAS.Parametros.MsgType.question)
                                == DialogResult.OK)
                            {
                                try
                                {
                                    Parametros.General.ShowWaitSplash(this, "APROBANDO", Parametros.Properties.Resources.TXTGUARDANDO);

                                    var pagoAguinaldo = db.PagoAguinaldos.Single(pa => pa.ID == idPagoAguinaldo);
                                    pagoAguinaldo.Aplicado = true;
                                    db.SubmitChanges();

                                    var detalle = db.DetallePagoAguinaldos.Where(o => o.PagoAguinaldoID.Equals(pagoAguinaldo.ID));

                                    if (detalle.Count() > 0)
                                    {

                                        foreach (var obj in detalle)
                                        {
                                            var em = db.Empleados.SingleOrDefault(x => x.ID.Equals(obj.EmpleadoID));
                                            if (em != null)
                                            {
                                                em.FechaUltimoAguinaldo = ff.AddDays(1);
                                                db.SubmitChanges();
                                            }
                                        }
                                    }
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    RefreshData();
                                }
                                catch (Exception ex)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                                }
                            }
                        }
                        else
                        {
                            Parametros.General.DialogMsg("Ya se encuentra aprobada un pago de aguinaldo para este periodo", SAGAS.Parametros.MsgType.warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
        }


        
        private void btnComprobantesPago_Click(object sender, EventArgs e)
        {
            if (gvForm.FocusedRowHandle >= 0)
            {
                ShowReporteComprobantes(Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
            }
        }

        private void btnFormatoBanco_Click(object sender, EventArgs e)
        {
            if (gvForm.FocusedRowHandle >= 0)
            {
                try
                {
                    int ID = Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID));

                    if (ID > 0)
                    {
                        Forms.FormatoBanco fb = new FormatoBanco(true);
                        fb._ID = ID;
                        fb.Owner = this.Owner;
                        fb.MdiParent = this.MdiParent;
                        fb.Show();
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        private void FormPagoAguinaldo_Shown(object sender, EventArgs e)
        {
            FillControl(true);
        }

        #endregion

        private void btnComprobantePagoNomina_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvForm.FocusedRowHandle >= 0)
                {
                    int ID = Convert.ToInt32(gvForm.GetFocusedRowCellValue("ID"));

                    if (ID > 0)
                    {
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        var VM = dbView.VistaPagoAguinaldos.Where(m => m.ID.Equals(ID)).ToList();

                        Reportes.Nomina.Hojas.ComprobantePagoAguinaldo rep = new Reportes.Nomina.Hojas.ComprobantePagoAguinaldo();

                        string Nombre, Direccion, Telefono, Ruc;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                        rep.pbLogo.Image = picture_LogoEmpresa;
                        rep.lblRuc.Text = rep.CelRuc1.Text = rep.xrTableCell2.Text = Ruc;
                        rep.lblDireccion.Text = Direccion;
                        rep.CelEmpresa1.Text = rep.lblEmpresa.Text = rep.xrTableCell1.Text = Nombre;
                        rep.xrLabel4.Text += " AGUINALDO " + VM.First().NombrePlanilla;
                        rep.lbltitulo.Text = rep.CelRango1.Text = rep.xrTableCell5.Text = "Periodo desde " + VM.First().FechaInicio.ToShortDateString() + " al " + VM.First().FechaFin.ToShortDateString();
                        rep.xrTableCell50.Text = rep.xrTableCell55.Text = "Expresado en: Córdobas";
                        rep.Fecha.Value = VM.First().FechaInicio;
                        //rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");
                        //rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                        rep.DataSource = VM;

                        //rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                        rep.RequestParameters = false;
                        rep.CreateDocument(true);
                        printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


       
    }
}
