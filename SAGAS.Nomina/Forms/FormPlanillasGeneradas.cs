using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Data.Linq;
using System.Data.SqlClient;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormPlanillasGeneradas : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogGenerarPlanilla nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private Parametros.ListEstadosOrdenes listado = new Parametros.ListEstadosOrdenes();
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormPlanillasGeneradas()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Anular";
            this.btnModificar.Caption = "Aprobar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaPlanillaGenerada);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            FillControl(false);            
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl(bool Refresh)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    
                if (Refresh)
                    linqInstantFeedbackSource1.Refresh();
                else
                {
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                    lkMonth.DataSource = listadoMes.GetListMeses();

                    btnMostrarCDNomina.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnMostrarCDNomina");
                    btnFormatoBanco.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnFormatoBanco");
                    btnComprobantePagoNomina.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnComprobantePagoNomina");
                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaDesde") > fecha.AddDays(-1)).ToString();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from v in dv.VistaPlanillaGenerada
                            where lista.Contains(v.EstacionServicioID)
                            select new
                            {
                                v.ID,
                                v.PlanillaID,
                                v.PlanillaNombre,
                                v.NumeroPlanilla,
                                v.EstructuraOrganizativaNombre,
                                v.FechaDesde,
                                v.FechaHasta,
                                Mes = v.FechaHasta.Month,
                                Year = v.FechaHasta.Year,
                                v.Aprobado,
                                v.TipoCambio,
                                v.MovimientoID,
                                v.UsuarioID,
                                v.Numero,
                                v.FechaCreado,
                                v.FechaRegistro,
                                v.MovimientoTipoID,
                                v.EstacionServicioID,
                                v.EstacionNombre,
                                v.SubEstacionID,
                                v.SubEstacionNombre
                            };

                e.QueryableSource = query;
                e.Tag = dv;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }

        private void ImprimirComprobante(int ID)
        {

            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                {
                    conn.Open();

                    // 1.  create a command object identifying the stored procedure
                    SqlCommand cmd = new SqlCommand("sp_PivotViewDetalleMovimientoPlanilla", conn);

                    // 2. set the command object so it knows to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 3. add parameter to command, which will be passed to the stored procedure
                    cmd.Parameters.Add(new SqlParameter("@DetallePlanillaGeneradaID", Convert.ToInt32(ID)));

                    // execute the command
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        // iterate through results, printing each to console
                        dt.Load(rdr);
                    }

                }

                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Planilla";

                string exprex = "";

                foreach (DataColumn col in dt.Columns)
                {
                    string nombre = col.ColumnName;

                    switch (nombre)
                    {
                        case "Monto11":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }

                        case "Monto17":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto18":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto20":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto21":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto22":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto23":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto24":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto25":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        case "Monto27":
                            {
                                if (String.IsNullOrEmpty(exprex))
                                    exprex = "isnull(" + col.ColumnName + ",0)";
                                else
                                    exprex += "+ isnull(" + col.ColumnName + ",0)";

                                break;
                            }
                        default:

                            break;
                    }
                }

                dt.Columns.Add("xxx", typeof(decimal), exprex);
                    //"isnull(Monto11,0) + isnull(Monto18,0)+ isnull(Monto20,0)+ isnull(Monto21,0)+ isnull(Monto22,0)+ isnull(Monto23,0)+ isnull(Monto24,0)+ isnull(Monto25,0) + isnull(Monto26,0)");

                Reportes.Nomina.Hojas.Planillaz rep = new Reportes.Nomina.Hojas.Planillaz(dt);
                string vPlanilla = dt.Rows[0]["PlanillaNombre"].ToString();
                DateTime FechaInicial = Convert.ToDateTime(dt.Rows[0]["FechaDesde"]).Date;
                DateTime FechaFinal = Convert.ToDateTime(dt.Rows[0]["FechaHasta"]).Date;
                string Nombre, Direccion, Telefono, vRuc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out vRuc, out picture_LogoEmpresa);
                rep.pbLogo.Image = picture_LogoEmpresa;
                rep.lblEmpresa.Text = Nombre;
                rep.lblRuc.Text = vRuc;
                rep.lblDireccion.Text = Direccion;

                rv.Text = vPlanilla + " " + FechaFinal.ToShortDateString();
                rep.lbltitulo.Text = "NOMINA PERIODO CORESPONDIENTE DEL " + FechaInicial.Day.ToString() + " AL " + FechaFinal.Day.ToString() + " DE " + Parametros.General.GetMonthInLetters(FechaInicial.Month) + " " + FechaInicial.Year.ToString();

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

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool Refresh, int IDPrint)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if (Refresh)
                FillControl(true);
            
            if (IDPrint > 0)
                ImprimirComprobante(IDPrint);
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogGenerarPlanilla(Usuario);
                    nf.Text = "Crear Nueva Planilla";
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();
                }
                else
                    nf.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        
        //Anular la Nomina
        private void AnularNomina(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    Entidad.PlanillaGenerada PG = db.PlanillaGenerada.Single(s => s.ID.Equals(Convert.ToInt32(ID)));
                    PG.DetallePlanillaGenerada.ToList().ForEach(det => det.DetalleMovimientoPlanilla.Clear());
                    PG.DetallePlanillaGenerada.Clear();
                    db.PlanillaGenerada.DeleteOnSubmit(PG);

                    if (PG.MovimientoID > 0)
                    {
                        db.ComprobanteContables.DeleteAllOnSubmit(db.ComprobanteContables.Where(o => o.MovimientoID.Equals(PG.MovimientoID)));
                        db.Movimientos.DeleteOnSubmit(db.Movimientos.Single(s => s.ID.Equals(PG.MovimientoID)));
                    }

                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        protected override void Edit()
        {
            
        }

        private void Aprobar(int ID, string Nombre, int Number)
        {
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                    Entidad.PlanillaGenerada PG = db.PlanillaGenerada.Single(s => s.ID.Equals(Convert.ToInt32(ID)));
                    int _Subsidio = Parametros.Config.MovimientoSubsidioID();

                    if (!PG.Aprobado)
                    {
                        foreach (Entidad.DetallePlanillaGenerada item in PG.DetallePlanillaGenerada)
                        {
                            foreach (Entidad.DetalleMovimientoPlanilla obj in item.DetalleMovimientoPlanilla.Where(o => o.MovimientoEmpleadoID > 0))
                            {
                                Entidad.MovimientoEmpleado ME = db.MovimientoEmpleado.SingleOrDefault(s => s.ID.Equals(obj.MovimientoEmpleadoID));

                                if (ME != null)
                                {
                                    ME.Abono += obj.Monto;

                                    if (obj.TipoMovimientoEmpleadoID.Equals(_Subsidio))
                                    {
                                        if (ME.Abono >= ME.MontoCuota)
                                            ME.Pagado = true;
                                    }
                                    else
                                    {
                                        if (ME.Abono >= ME.MontoTotal)
                                            ME.Pagado = true;
                                    }

                                    ME.NumeroCuota += 1;
                                    db.SubmitChanges();
                                }
                            }

                            Entidad.IRRetenido tira = db.IRRetenido.FirstOrDefault(ti => ti.EmpleadoID.Equals(item.EmpleadoID) && (ti.FechaIR.Year == Convert.ToDateTime(PG.FechaDesde).Year && ti.FechaIR.Month == Convert.ToDateTime(PG.FechaDesde).Month));

                            if (tira != null)
                            {
                                tira.OtrosIngresos = tira.OtrosIngresosVirtual;
                                tira.INSSLaboral = tira.INSSLaboralVirtual;
                                db.SubmitChanges();
                            }
                        }

                        PG.Aprobado = true;
                        db.SubmitChanges();
                        trans.Commit();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("La nómina " + Nombre + " Nro. " + Number + " fue aprobada exitosamente.", Parametros.MsgType.message);
                    }
                    else
                    {
                        trans.Rollback();
                        Parametros.General.DialogMsg("La Planilla actual ya se encuentra aprobada", Parametros.MsgType.warning);
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();                   
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
                
        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobada)))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "La Nomina esta ya está aprobada.", Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaDesde)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularNomina(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            FillControl(true);
                        }
                    }
                }
            }
        }

        #endregion  

        #region <<< EVENTOS  >>>        

        private void btnMostrarCDNomina_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue("MovimientoID"));

                    if (ID > 0)
                    {
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();

                        Reportes.Contabilidad.Hojas.RptComprobanteCD rep = new Reportes.Contabilidad.Hojas.RptComprobanteCD();

                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                        rep.PicLogo.Image = picture_LogoEmpresa;
                        rep.CeEmpresa.Text = Nombre;
                        rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");
                        rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                        rep.DataSource = VM;

                        rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
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

        private void btnFormatoBanco_Click(object sender, EventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                    if (ID > 0)
                    {
                        Forms.FormatoBanco fb = new FormatoBanco(false);
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

        private void btnComprobantePagoNomina_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                    if (ID > 0)
                    {
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                        
                        DataTable dt = new DataTable();

                        using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                        {
                            conn.Open();

                            // 1.  create a command object identifying the stored procedure
                            SqlCommand cmd = new SqlCommand("sp_PivotViewDetalleMovimientoPlanilla", conn);

                            // 2. set the command object so it knows to execute a stored procedure
                            cmd.CommandType = CommandType.StoredProcedure;

                            // 3. add parameter to command, which will be passed to the stored procedure
                            cmd.Parameters.Add(new SqlParameter("@DetallePlanillaGeneradaID", Convert.ToInt32(ID)));

                            // execute the command
                            using (SqlDataReader rdr = cmd.ExecuteReader())
                            {
                                // iterate through results, printing each to console
                                dt.Load(rdr);
                            }

                        }

                        string exprex = "";

                        foreach (DataColumn col in dt.Columns)
                        {
                            string nombre = col.ColumnName;

                            switch (nombre)
                            {
                                case "Monto11":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }

                                case "Monto17":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto18":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto20":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto21":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto22":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto23":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto24":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }
                                case "Monto25":
                                    {
                                        if (String.IsNullOrEmpty(exprex))
                                            exprex = "isnull(" + col.ColumnName + ",0)";
                                        else
                                            exprex += "+ isnull(" + col.ColumnName + ",0)";

                                        break;
                                    }                                
                                default:

                                    break;
                            }
                        }

                        dt.Columns.Add("xxx", typeof(decimal), exprex);
                        //"isnull(Monto11,0) + isnull(Monto18,0)+ isnull(Monto20,0)+ isnull(Monto21,0)+ isnull(Monto22,0)+ isnull(Monto23,0)+ isnull(Monto24,0)+ isnull(Monto25,0) + isnull(Monto26,0)");

                        Reportes.Nomina.Hojas.comprobantePago rep = new Reportes.Nomina.Hojas.comprobantePago(dt);
                        string vPlanilla = dt.Rows[0]["PlanillaNombre"].ToString();
                        DateTime FechaInicial = Convert.ToDateTime(dt.Rows[0]["FechaDesde"]).Date;
                        DateTime FechaFinal = Convert.ToDateTime(dt.Rows[0]["FechaHasta"]).Date;
                        string rango = "Del " + FechaInicial.Date.ToShortDateString() + " al " + FechaFinal.Date.ToShortDateString();
                        string Nombre, Direccion, Telefono, vRuc;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out vRuc, out picture_LogoEmpresa);
                        rep.pbLogo.Image = picture_LogoEmpresa;
                        rep.lblEmpresa.Text = rep.CelEmpresa1.Text = rep.CelEmpresa2.Text = Nombre;
                        rep.lblRuc.Text = rep.CelRuc1.Text = rep.CelRuc2.Text = vRuc;
                        rep.lblDireccion.Text = Direccion;
                        rep.lbltitulo.Text = rep.CelRango1.Text = rep.CelRango2.Text = rango;
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

        /// Aprobacion de la Nomina
        private void chkAprobado_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkAprobarNomina"))
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobada)))
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "La Nomina esta ya está aprobada.", Parametros.MsgType.warning);
                        else
                        {
                            if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaDesde)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                            else
                            {
                                if (Parametros.General.DialogMsg("¿Está seguro de aprobar la nómina seleccionada?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                {
                                    Aprobar(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), gvData.GetFocusedRowCellValue(colPlanillaNombre).ToString(), Convert.ToInt32(gvData.GetFocusedRowCellValue(colNumero)));
                                    FillControl(true);
                                    gvData.FocusedColumn = colNumero;
                                }
                            }
                        }
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
