using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.Design;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Native;
using System.Data.SqlClient;
using DevExpress.DataAccess.Sql;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Net;
using System.Security.Principal;

namespace SAGAS.Reportes
{
    

    public partial class FormRptReport : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int c = Parametros.General.UserID;
        internal DateTime _FechaServer;
        private BackgroundWorker bgw;
        private DevExpress.XtraReports.UI.XtraReport Mrep;
        private DevExpress.XtraReports.UI.XtraReport ResrvaDatos = new DevExpress.XtraReports.UI.XtraReport();
        private string VistaTabla;
        private int ReporteID;
        //private List<Entidad.ParametrosReporte> DatosReporte;
        private List<string> fecha = new List<string> { "Desde", "Hasta" };
        private List<TextEdit> datos1 = new List<TextEdit>();
        private List<CheckEdit> datos2 = new List<CheckEdit>();
        private List<ComboBoxEdit> datos3 = new List<ComboBoxEdit>();
        private List<CalcEdit> datos4 = new List<CalcEdit>();
        private List<SpinEdit> datos5 = new List<SpinEdit>();
        private List<TimeEdit> datos6 = new List<TimeEdit>();
        private List<ToggleSwitch> datos7 = new List<ToggleSwitch>();
        private List<DateEdit> datos8 = new List<DateEdit>();
        private List<CheckedComboBoxEdit> datos9 = new List<CheckedComboBoxEdit>();
        private CheckedComboBoxEdit datoEsdaciones = new CheckedComboBoxEdit();
        private Entidad.Reporte archivo;
        private String consulta;
        DevExpress.DataAccess.Sql.SqlDataSource datosDataSouser;
        private DataSet m_ClonedDataSet;
        private struct ejecucion 
        {
            string ProductoCodigo {get; set;}	
            string ProductoNombre {get; set;}
            int Cantidad { get; set; }
        }

        #endregion

        #region *** INICIO ***

        public FormRptReport(int ID)
        {
            ReporteID = ID;
            //VistaTabla = Tabla;
            //this.Mrep = archivo;
            InitializeComponent();            
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                // Set the processing mode for the ReportViewer to Remote
                //reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;

                //ServerReport serverReport = reportViewer1.ServerReport;

                // Get a reference to the default credentials
                //System.Net.ICredentials credentials =
                //    System.Net.CredentialCache.DefaultCredentials;

                //// Get a reference to the report server credentials
                //ReportServerCredentials rsCredentials =
                //    serverReport.ReportServerCredentials;

                //// Set the credentials for the server report
                //rsCredentials.NetworkCredentials = credentials;

                //NetworkCredential myCred = new NetworkCredential("Administrador","Zanzi2016");
                //reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;


                // Set the report server URL and report path
                //serverReport.ReportServerUrl =
                //    new Uri("http:// <Server Name>/reportserver");
                //serverReport.ReportPath =
                //    "/AdventureWorks Sample Reports/Sales Order Detail";

            //reportViewer1.ServerReport.ReportServerUrl = new Uri(@"http://161.0.36.74/Zanzibar/Pages/Folder.aspx");
            //   reportViewer1.ServerReport.ReportPath = @"test";

                // Create the sales order number report parameter
                //ReportParameter salesOrderNumber = new ReportParameter();
                //salesOrderNumber.Name = "SalesOrderNumber";
                //salesOrderNumber.Values.Add("SO43661");

                // Set the report parameters for the report
                //reportViewer1.ServerReport.SetParameters(
                //    new ReportParameter[] { salesOrderNumber });

                // Refresh the report
                //reportViewer1.RefreshReport();



                reportViewer1.Reset();
                reportViewer1.LocalReport.DataSources.Clear();
                //reportViewer1.Visible = true;
                reportViewer1.LocalReport.ReportPath = @"C:\TEST.rdl";
                //ReportDataSource rds = new ReportDataSource("ds_users", dt);
                //ReportViewer1.LocalReport.DataSources.Add(rds);
                //reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
                reportViewer1.RefreshReport();
                reportViewer1.LocalReport.Refresh();

                //reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
                //reportViewer1.ServerReport.ReportServerUrl = new Uri("http://161.0.36.74/Zanzibar");
                //reportViewer1.ServerReport.ReportPath = @"/Pages/Report.aspx?ItemPath=%2fContabilidad%2fAnalisis%2f01-Resultados+por+Estación+y+Cuentas";

            //NetworkCredential myCred = new NetworkCredential("Administrador","Zanzi2016");
            //reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;

            //reportViewer1.RefreshReport();



                //reportViewer1.ServerReport.ReportServerCredentials = new CustomReportCredentials("Administrador", "Zanzi2016", "MyDomain");

                //Reporte en DevExpress
                //Reportes.FormReportViewer rv = new Reportes.FormReportViewer();

                //System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                

                //DevExpress.XtraReports.UI.XtraReport rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                //Reportes.FormRptReport fr = new Reportes.FormRptReport(obj.ID);

                //archivo = db.Reportes.Single(d => d.ID.Equals(ReporteID));

                //this.Text = archivo.Nombre;

                //System.IO.MemoryStream ms = new System.IO.MemoryStream(archivo.Archivo);

                //Mrep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);

                //int cantidadP = Mrep.Parameters.Count;

                //for (int i = 0; i < cantidadP; i++)
                //    Mrep.Parameters.RemoveAt(0);

                //consulta = archivo.ParametroWhere;

                /*DevExpress.DataAccess.Sql.SqlDataSource dDataSouser = (DevExpress.DataAccess.Sql.SqlDataSource)Mrep.DataSource;

                CustomSqlQuery query = new CustomSqlQuery();
                query.Name = "valor";
                query.Sql = "select * from dbo.[<Rpt_Existencias_Tienda_Lubs_Etc] where EstacionServicioID = 11 and IDArea = 5 and Fecha = '2016-04-04' and AlmacenID = 13";
                
                dDataSouser.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString();
                dDataSouser.Connection.Open();
                dDataSouser.Queries.Add(query);
                dDataSouser.Connection.Close();*/
                /*
                 DevExpress.DataAccess.Sql.SqlDataSource' to type 'DevExpress.XtraReports.Native.DataSource'.
                 */
                
                //MessageBox.Show(dDataSouser.Connection.ConnectionString);

                //valorParemetros = Mrep.Parameters.ToList();

                //datosDataSouser = (DevExpress.DataAccess.Sql.SqlDataSource)Mrep.DataSource;
                //datosDataSouser2 = Mrep.DataSource;

                //datosDataSouser.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString();

                //MessageBox.Show(Parametros.Config.GetCadenaConexionString());
                
                //String.Copy(datosDataSouser.Connection.ConnectionString);
                
                //Mrep.RequestParameters = false;
                ////Mrep.Parameters.
                //Mrep.DataSource = null;
                //Mrep.DataAdapter = null;
                //Mrep.DataMember = null;

                //Mrep.CreateDocument();
                //this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                //Mrep.CreateDocument(true);
                
                //DatosReporte = db.ParametrosReportes.Where(d => d.ReportesID == ReporteID).OrderBy(o => o.lugar).ToList();

                //foreach (Entidad.ParametrosReporte d in DatosReporte)
                //    insertar(d);

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
            finally
            {
                if (db.Connection.State == ConnectionState.Open) 
                    db.Connection.Close();
            }
        }

        //void insertar(Entidad.ParametrosReporte PR)
        //{
        //    DevExpress.XtraLayout.LayoutControlItem LCI = new DevExpress.XtraLayout.LayoutControlItem();

        //    try
        //    {
        //        LCI.Name = "LayoutControl" + PR.Parametro;
        //        LCI.Text = PR.Nombre;
        //        layoutControlParametros.Add(LCI);
        //        string nombreComponenete = PR.Componente + PR.Parametro;

        //        if (PR.Componente.Equals("DateEdit"))
        //        {
        //            DateEdit DateE1 = new DateEdit();
        //            DateE1.Name = nombreComponenete;
        //            DateE1.DateTime = _FechaServer;
        //            LCI.Control = DateE1;
        //            datos8.Add(DateE1);
        //        }
        //        if (PR.Componente.Equals("TextEdit"))
        //        {
        //            TextEdit TextE = new TextEdit();
        //            TextE.Name = nombreComponenete;
        //            LCI.Control = TextE;
        //            datos1.Add(TextE);
        //        }
        //        if (PR.Componente.Equals("CheckEdit"))
        //        {
        //            CheckEdit CheckE = new CheckEdit();
        //            CheckE.Name = nombreComponenete;
        //            LCI.Control = CheckE;
        //            CheckE.Text = "";
        //            datos2.Add(CheckE);
        //        }
        //        if (PR.Componente.Equals("CalcEdit"))
        //        {
        //            CalcEdit CalcE = new CalcEdit();
        //            CalcE.Name = nombreComponenete;
        //            LCI.Control = CalcE;
        //            datos4.Add(CalcE);
        //        }
        //        if (PR.Componente.Equals("SpinEdit"))
        //        {
        //            SpinEdit SpinE = new SpinEdit();
        //            SpinE.Name = nombreComponenete;
        //            LCI.Control = SpinE;
        //            datos5.Add(SpinE);
        //        }
        //        if (PR.Componente.Equals("ToggleSwitch"))
        //        {
        //            ToggleSwitch ToggleS = new ToggleSwitch();
        //            ToggleS.Name = nombreComponenete;
        //            LCI.Control = ToggleS;
        //            datos7.Add(ToggleS);
        //        }
        //        if (PR.Componente.Equals("TimeEdit"))
        //        {
        //            TimeEdit TimeE = new TimeEdit();
        //            TimeE.Name = nombreComponenete;
        //            LCI.Control = TimeE;
        //            datos6.Add(TimeE);
        //        }
        //        if (PR.Componente.Equals("ComboBoxEdit"))
        //        {
        //            ComboBoxEdit ComboBE = new ComboBoxEdit();
        //            ComboBE.Name = nombreComponenete;

        //            List<String> sentences = new List<String>();
        //            int position = 0;
        //            int start = 0;
        //            string value = PR.ConsultaInterna;

        //            do
        //            {
        //                position = value.IndexOf(' ', start);
        //                if (position >= 0)
        //                {
        //                    sentences.Add(value.Substring(start, position - start + 1).Trim());
        //                    start = position + 1;
        //                }
        //            } while (position > 0);
        //            LCI.Control = ComboBE;

        //            foreach (DataRow Row in ObtenerInformacion(PR.ConsultaInterna).Rows)
        //            {
        //                ComboBE.Properties.Items.Add(Row[sentences[1]]);
        //            }
        //            datos3.Add(ComboBE);
        //        }
        //        if (PR.Componente.Equals("CheckedComboBoxEdit"))
        //        {
        //            CheckedComboBoxEdit ComboBE = new CheckedComboBoxEdit();
        //            ComboBE.Name = nombreComponenete;

        //            List<String> sentences = new List<String>();
        //            int position = 0;
        //            int start = 0;
        //            string value = PR.ConsultaInterna;

        //            do
        //            {
        //                position = value.IndexOf(' ', start);
        //                if (position >= 0)
        //                {
        //                    sentences.Add(value.Substring(start, position - start + 1).Trim());
        //                    start = position + 1;
        //                }
        //            } while (position > 0);
        //            LCI.Control = ComboBE;

        //            foreach (DataRow Row in ObtenerInformacion(PR.ConsultaInterna).Rows)
        //            {
        //                ComboBE.Properties.Items.Add(Row[sentences[1]]);
        //            }
        //            datos9.Add(ComboBE);
        //        }
        //        if (PR.Componente.Equals("Estaciones"))
        //        {
        //            CheckedComboBoxEdit CheckedCombo = new CheckedComboBoxEdit();
        //            CheckedCombo.Name = PR.Componente + PR.Parametro;

        //            List<String> valores = db.GetMostrarEstacionesySubestacionesDeEmpl(c).Select(n => n.Nombre).ToList();

        //            foreach (string d in valores)
        //                CheckedCombo.Properties.Items.Add(d);

        //            //CheckedCombo.Properties.Items.Add("Managua");
        //            //CheckedCombo.Properties.Items.Add("Leon");
        //            //CheckedCombo.Properties.Items.Add("Chinandega");

        //            CheckedCombo.Properties.SeparatorChar = ',';

        //            LCI.Control = CheckedCombo;

        //            datoEsdaciones = CheckedCombo;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
        //    }
        //}
        
        //void Reconocer(Entidad.ParametrosReporte PR)
        //{
        //    string where = "", nombreComponenete = PR.Componente + PR.Parametro;

        //    try
        //    {
        //        if (PR.Componente.Equals("DateEdit"))
        //        {
        //            DateEdit v1 = datos8.Single(d => d.Name.Equals(nombreComponenete));
        //            where = "'" + v1.DateTime.Month + "-" + v1.DateTime.Day + "-" + v1.DateTime.Year + "'";
        //        }
        //        if (PR.Componente.Equals("TextEdit"))
        //        {
        //            TextEdit TE = datos1.Single(d => d.Name.Equals(nombreComponenete));
        //            if (TE.Text != "")
        //                where = "'" + TE.Text + "'";
        //        }
        //        if (PR.Componente.Equals("CheckEdit"))
        //        {
        //            CheckEdit CE = datos2.Single(d => d.Name.Equals(nombreComponenete));

        //            if (CE.Checked) { where = " 1 "; }
        //            else { where = " 0 "; }
        //        }
        //        if (PR.Componente.Equals("CalcEdit"))
        //        {
        //            CalcEdit CE = datos4.Single(d => d.Name.Equals(nombreComponenete));
        //            where = CE.Text;
        //        }
        //        if (PR.Componente.Equals("SpinEdit"))
        //        {
        //            SpinEdit SE = datos5.Single(d => d.Name.Equals(nombreComponenete));
        //            where = "" + SE.Value;
        //        }
        //        if (PR.Componente.Equals("ToggleSwitch"))
        //        {
        //            ToggleSwitch TS = datos7.Single(d => d.Name.Equals(nombreComponenete));

        //            if (TS.IsOn) { where = " 1 "; }
        //            else { where = " 0 "; }
        //        }
        //        if (PR.Componente.Equals("TimeEdit"))
        //        {
        //            TimeEdit TE = datos6.Single(d => d.Name.Equals(nombreComponenete));
        //            where = where + "'" + TE.Time.Hour + ":" + TE.Time.Minute + ":" + TE.Time.Second + "'";
        //        }
        //        if (PR.Componente.Equals("ComboBoxEdit"))
        //        {
        //            ComboBoxEdit CE = datos3.Single(d => d.Name.Equals(nombreComponenete));
        //            where = "'"+CE.Text+"'";
        //        }

        //        if (PR.Componente.Equals("CheckedComboBoxEdit"))
        //        {
        //            string cantidad = ""; int v = 0;

        //            CheckedComboBoxEdit CheckedCombo = datos9.Single(d => d.Name.Equals(nombreComponenete));

        //            foreach (var d in CheckedCombo.Properties.GetItems().GetCheckedValues())
        //            {
        //                if (v == 0)
        //                {
        //                    cantidad = "'" + d + "'";
        //                    v++;
        //                }
        //                else
        //                    cantidad = cantidad + ", '" + d + "'";
        //            }

        //            where = cantidad;
        //        }

        //        if (PR.Componente.Equals("Estaciones"))
        //        {
        //            string cantidad = ""; int v = 0;

        //            foreach (var d in datoEsdaciones.Properties.GetItems().GetCheckedValues())
        //            {
        //                if (v == 0)
        //                {
        //                    cantidad = "'" + d + "'";
        //                    v++;
        //                }
        //                else
        //                    cantidad = cantidad + ", '" + d + "'";
        //            }

        //            where = cantidad;
        //        }

        //        consulta = consulta.Replace("'" + PR.lugar + "'", where);

        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
        //    }
        //}

        public static DataTable ObtenerInformacion(string a)
        {
            DataSet dsA = null;
            using (SqlConnection _conexion = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
            {
                using (SqlCommand _comando = _conexion.CreateCommand())
                {
                    _comando.CommandTimeout = 900;
                    _comando.CommandText = a;
                    _comando.CommandType = CommandType.Text;
                    SqlDataAdapter _adactador = new SqlDataAdapter(_comando);
                    dsA = new DataSet();
                    _adactador.Fill(dsA);
                    _comando.Connection.Close();
                }
            }

            return dsA.Tables[0];
        }

        public bool ValidarCampos()
        {
            for (int i = 1; i < datos8.Count(); i = i + 2) 
            {
                if (datos8.Single(d => d.TabIndex == 0).EditValue == null || datos8.Single(d => d.TabIndex == 1).EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToDateTime(datos8.Single(d => d.TabIndex == 0).EditValue) > Convert.ToDateTime(datos8.Single(d => d.TabIndex == 0).EditValue))
                {
                    Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                    return false;
                }
            }
            if (datoEsdaciones.Properties.GetItems().GetCheckedValues().Count()==0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos una Estación.", Parametros.MsgType.warning);
                return false;
            }
            foreach (CalcEdit d in datos4) 
            {
                if (d.Value <= 0) 
                {
                    Parametros.General.DialogMsg("Debe escribir un dato numerico en el "+d.Name, Parametros.MsgType.warning);
                    return false;
                }
            }

            foreach (ComboBoxEdit d in datos3) 
            {
                if (d.SelectedIndex <= 0) 
                {
                    Parametros.General.DialogMsg("Selecionar un valor del comboBox " + d.Name, Parametros.MsgType.warning);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region *** EVENTOS ***

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Parametros.General.PrintExportComponet(true, this.Text, this.MdiParent, null, null, DataGrid, false, 25, 25, 140, 50, 0);
        }

        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.dglExportToFile.Filter = "Hoja de Microsoft Excel (*.xlsx)|*.xlsx";

            if (this.dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.dglExportToFile.FileName != "")
                {
                    this.DataGrid.MainView.ExportToXlsx(this.dglExportToFile.FileName);

                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (true)
                {
                    bgw = new BackgroundWorker();
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.RunWorkerAsync();
                    ReportPrintTool printTool = new ReportPrintTool(Mrep);
                    printTool.ShowPreview();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);

                System.IO.MemoryStream ms = new System.IO.MemoryStream(archivo.Archivo);

                XtraReport rep = new XtraReport();

                DataTable dtA = ObtenerInformacion("select * from dbo.[<Rpt_Existencias_Tienda_Lubs_Etc] where EstacionServicioID = 11 and IDArea = 5 and Fecha = '2016-04-04' and AlmacenID = 13");
                dtA.TableName = "<Rpt_Existencias_Tienda_Lubs_Etc";
                rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);

                //rep.ClearContent();

                /*m_ClonedDataSet = new DataSet();

                m_ClonedDataSet.Tables.Add("<Rpt_Existencias_Tienda_Lubs_Etc");

                foreach (DataColumn d in dtA.Columns)
                    m_ClonedDataSet.Tables["<Rpt_Existencias_Tienda_Lubs_Etc"].Columns.Add(d.ColumnName, d.DataType);

                m_ClonedDataSet = dtA.DataSet;

                MessageBox.Show("Avanza");*/
                //valor.ConnectionString = Parametros.Config.GetCadenaConexionString();
                
                //SqlDataSource dDataSouser = (DevExpress.DataAccess.Sql.SqlDataSource)Mrep.DataSource;

                //dDataSouser.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString();
                
                /*CustomSqlQuery query = new CustomSqlQuery();
                query.Name = "customQuery1";
                query.Sql = "select * from dbo.[<Rpt_Existencias_Tienda_Lubs_Etc] where EstacionServicioID = 11 and IDArea = 5 and Fecha = '2016-04-04' and AlmacenID = 13";
                */
                //dDataSouser.Queries.Add(query);
                //df.InsertCommand.All(query);
                //bindingSource1.DataSource = dtA;
                //bindingSource1.;
                //dv = new Entidad.SAFIERODataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                //ep rep = new ep();

                //DataSet fdg = dtA.DataSet;
                //fdg.DataSetName = "<Rpt_Existencias_Tienda_Lubs_Etc";
                //fdg.Tables = ;

                rep.RequestParameters = false;
                //240002		-24.000
                //240014	COCA SABORES BOTELLA PLASTICA 500ML	-24.000
                //rep.DataMember = "InvalidDataMember";
                
            //     DevExpress.XtraReports.  XtraReportSell xr1 = new Library_1.XtraReportSell();
            //xr1.datasource=sell;
            //xr1.DataMember= Library_1.libraryDataSet1.TFactorSellDataTable.TableName 
            //xr1.ShowPreviewDialog();
                //rep.bin .DataSource = dtA.AsDataView();
                //rep.DataMember = rep.DataSource //dtA.TableName;
                //select * from dbo.[<Rpt_Existencias_Tienda_Lubs_Etc] where EstacionServicioID = 11 and IDArea = 5 and Fecha = '2016-04-04' and AlmacenID = 13"
                //rep.DataMember = "dbo.[<Rpt_Existencias_Tienda_Lubs_Etc]";
                //rep.DataMember = "ValidDataMember";
                
                //DataSource d = dTA.DataSet.Tables.ToString;

                //rep.Report.DataMember = "InvalidDataMember";
                //rep.Report.DataSource = dtA;
                //rep.Report.DataMember = "ValidDataMember";

                //rep.FillDataSource();
                //rep.DataMember = dtA.DataSet.Tables.; 
                //rep.DataMember.All("");

                e.Result = rep;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    Mrep = ((DevExpress.XtraReports.UI.XtraReport)(e.Result));
                    Mrep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        private void FormRptReport_Load(object sender, EventArgs e)
        {

            //this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    ReportViewer1.Width = 800;
            //    ReportViewer1.Height = 600;
            //    ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            //    IReportServerCredentials irsc = new CustomReportCredentials("Administrator", "MYpassworw", "domena");
            //    ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            //    ReportViewer1.ServerReport.ReportServerUrl = new Uri("http://192.168.0.1/ReportServer/");
            //    ReportViewer1.ServerReport.ReportPath = "/autonarudzba/listanarudzbi";
            //    ReportViewer1.ServerReport.Refresh();
            //}
        }


    }

    public class CustomReportCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {

        // local variable for network credential.
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;  // not use ImpersonationUser
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {

                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}