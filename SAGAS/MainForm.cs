using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using DevExpress.XtraEditors;
using System.DirectoryServices;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using DevExpress.XtraReports.UI;
using System.Data.OleDb;
using System.Data.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace SAGAS
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm //DevExpress.XtraBars.Ribbon.RibbonForm  // Form
    {
        private string route = @System.IO.Directory.GetCurrentDirectory() + @"\Update";
        private string routeRpt = @System.IO.Directory.GetCurrentDirectory() + @"\Reportes";

        #region <<< INICIO >>>

        public MainForm()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly);
            defaultLookAndFeelMain.LookAndFeel.SkinName = "Office 2007 Green";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ni");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ni");
            Parametros.General.Conexion = Parametros.Config.GetCadenaConexionString();           
        }
     
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            DateTime fecha = Convert.ToDateTime("2000-01-01");
                //db.GetLastUpdate() != null ? Convert.ToDateTime(db.GetLastUpdate()) : Convert.ToDateTime("2000-01-01");
            SetPermisos(Parametros.General.UserID, Parametros.General.EstacionServicioID, db);
            Parametros.General.Empresa = db.Empresas.Where(o => o.Activo).First();

            Parametros.General.ReportServerUrl = Parametros.Config.GetReportReady("ReportServerUrl");
            Parametros.General.ReportPath = Parametros.Config.GetReportReady("ReportPath");
            Parametros.General.userNameCredential = Parametros.Config.GetReportReady("userNameCredential");
            Parametros.General.passwordCredential = Parametros.Config.GetReportReady("passwordCredential");
            Parametros.General.domainCredential = Parametros.Config.GetReportReady("domainCredential");
            
            btnKey.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            barListaLicencia.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            e.Result = fecha;
        }
        
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ribbonControlParent.Enabled = true;
            this.Text = Parametros.General.Empresa.Nombre + " " + Parametros.General.EstacionServicioName;
            DateTime FechaLastUpdate = (DateTime)e.Result;
            DateTime fecha = String.IsNullOrEmpty(Parametros.Config.GetValueByKeyAppSettings(Parametros.Config.strUpdateDate)) ? Convert.ToDateTime("24/02/2014") : Convert.ToDateTime(Parametros.Config.GetValueByKeyAppSettings(Parametros.Config.strUpdateDate));

            if (FechaLastUpdate > fecha)
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.Upload Up = db.Uploads.OrderByDescending(o => o.ID).FirstOrDefault();
                Parametros.General.DialogMsg("El sistema encontró una nueva actualización, se procedera a instalarse.", Parametros.MsgType.message);
                if (!System.IO.Directory.Exists(route))
                    System.IO.Directory.CreateDirectory(route);

                var di = new DirectoryInfo(route).GetFiles();

                di.ToList().ForEach(archivo => { archivo.Delete(); });

                File.WriteAllBytes(route + @"\ActualizadorSagas.exe", Up.Archivo);

                if (System.IO.File.Exists(@System.IO.Directory.GetCurrentDirectory() + @"\UpdateSagas.bat"))
                {
                    Parametros.Config.SetValueByKeyAppSettings(Parametros.Config.strUpdateDate, Convert.ToDateTime(db.GetDateServer()).ToString());
                    System.Diagnostics.Process.Start(@System.IO.Directory.GetCurrentDirectory() + @"\UpdateSagas.bat");
                    Application.Exit();
                }
                else
                    Parametros.General.DialogMsg("El archivo UpdateSagas.bat no existe", Parametros.MsgType.warning);

                this.Close();
            }
            else
            {
                //new Inventario.Forms.FormComprasPedido().Show();
                //fr.Owner = this;
                //fr.MdiParent = this;
                //fr.Show();
                //new Ventas.Forms.FormRetiro().Show();
                //new Tesoreria.Forms.FormPagos().Show();
                //ShowDevolucionConversion();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                bw.WorkerReportsProgress = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.RunWorkerAsync();

                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //if (Convert.ToDateTime(db.GetDateServer()) < Convert.ToDateTime("2016/06/06"))
                //{
                //File.WriteAllBytes(@"D:\" + @"Anexo.repx", db.Reportes.Single(s => s.ID.Equals(203)).Archivo);

                if (1>0)//(Parametros.LicensesManager.VerificarLicencia("SAGAS", db))
                {
                    Parametros.General.GetSubEstaciones(Parametros.General.UserID, Parametros.General.EstacionServicioID);
                    Parametros.General.GetListaColoresTanques();
                    if (!FillDataStatus(db)) return;
                    this.timerCnn.Start();
                }
                else
                    this.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "INTERRUPCIÓN DEL PROCESO", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
                 
        #endregion

        #region <<< MODULOS >>>

        //CONFIGURACION
        private void barConfiguracion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryConfiguracion.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryConfiguracion.Pages[0];
        }

        //ACTIVO
        private async void barActivoMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryActivoF.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryActivoF.Pages[0];

            if (barReportesActivoFijo.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {                
                await RptAF().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSActivoFijo, Parametros.Config.GetModuloID("Activo Fijo")).ConfigureAwait(false);
            }
        }

        //REPORTES ACTIVO FIJO
        async Task RptAF()
        {
            try
            {
                await Task.Delay(10);
                barReportesActivoFijo.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;                
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(8) && re.ModuloID.Equals(8)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesActivoFijo.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //TESORERIA
        private async void barTesoreria_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryTesoreria.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryTesoreria.Pages[0];

            if (barReportesTesoreria.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptTesoreria().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSTesoreria, Parametros.Config.GetModuloID("Tesorería")).ConfigureAwait(false);                
            }
        
        }

        //REPORTES TESORERIA
        async Task RptTesoreria()
        {
            try
            {
                await Task.Delay(10);
                barReportesTesoreria.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(5) && re.ModuloID.Equals(5)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesTesoreria.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }


                //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //db.CommandTimeout = 200;

                //var Rpts = db.Reportes.Where(r => !r.EsSubReporte && r.ModuloID.Equals(5)).OrderBy(o => o.Orden).ToList();

                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph; 
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesTesoreria.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesTesoreria.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //        barRptWeb.AddItem(btn);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //ADMINISTRACION
        private void barAdministracion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryAdministracion.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryAdministracion.Pages[0];           
        }

        //NOMINA
        private async void barNomina_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryNomina.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryNomina.Pages[0];

            if (barReportesNomina.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptNomina().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSNomina, Parametros.Config.GetModuloID("Recursos Humanos")).ConfigureAwait(false);
            }
        }

        //REPORTES NOMINA
        async Task RptNomina()
        {
            try
            {
                await Task.Delay(10);
                barReportesNomina.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(4) && re.ModuloID.Equals(4)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesNomina.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

                //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //db.CommandTimeout = 200;

                //var Rpts = db.Reportes.Where(r => !r.EsSubReporte && r.ModuloID.Equals(4)).OrderBy(o => o.Orden).ToList();

                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesNomina.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesNomina.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptWeb.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //ARQUEO
        private async void barArqueo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribbonPageCategoryArqueo.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryArqueo.Pages[0];

            if (barReportesArqueo.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptArqueo().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSArqueo, Parametros.Config.GetModuloID("Arqueo")).ConfigureAwait(false);
            }

        }

        //REPORTES ARQUEO
        async Task RptArqueo()
        {
            try
            {
                await Task.Delay(10);
                barReportesArqueo.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(2) && re.ModuloID.Equals(2)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesArqueo.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

                //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //db.CommandTimeout = 200;

                //var Rpts = db.Reportes.Where(r => !r.EsSubReporte && r.ModuloID.Equals(2)).OrderBy(o => o.Orden).ToList();

                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesArqueo.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesArqueo.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptWeb.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //CONTABILIDAD
        private async void barContabilidad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribbonPageCategoryContabilidad.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryContabilidad.Pages[0];
                        
            if (barReportesContabilidad.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptContabilidad().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSContabilidad, Parametros.Config.GetModuloID("Contabilidad")).ConfigureAwait(false);
            }
        }
        
        //REPORTES CONTABILIDAD
        async Task RptContabilidad()
        {
            try
            {
                await Task.Delay(10);
                barReportesContabilidad.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(3) && re.ModuloID.Equals(3)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesContabilidad.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                        {
                            var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                            btn.Tag = rep.EtReporte;                            
                            btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                            barRptTunel.AddItem(btn);
                        });
                    
                }

                //barReportesContabilidad.AddItem(new DevExpress.XtraBars.BarItem { Caption = "xxx", Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon)),  });

                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesContabilidad.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesContabilidad.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptWeb.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //INVENTARIO
        private async void barInventario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryInventario.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryInventario.Pages[0];

            if (barReportesInventario.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptInventario().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSInventario, Parametros.Config.GetModuloID("Inventario")).ConfigureAwait(false);
            }
        }

        //REPORTES INVENTARIO
        async Task RptInventario()
        {
            try
            {
                await Task.Delay(10);
                barReportesInventario.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(6) && re.ModuloID.Equals(6)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesInventario.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

                //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //db.CommandTimeout = 200;

                //var Rpts = db.Reportes.Where(r => !r.EsSubReporte && r.ModuloID.Equals(6)).OrderBy(o => o.Orden).ToList();
                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesInventario.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesInventario.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptWeb.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //VENTAS
        private async void barVentas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ribbonPageCategoryVentas.Visible = true;
            ribbonControlParent.SelectedPage = ribbonPageCategoryVentas.Pages[0];

            if (barReportesVentas.Visibility == DevExpress.XtraBars.BarItemVisibility.Always)
            {
                await RptVentas().ConfigureAwait(false);
                await RptSSRS(barReportesSSRSVentas, Parametros.Config.GetModuloID("Ventas")).ConfigureAwait(false);
            }
        }

        //REPORTES VENTAS
        async Task RptVentas()
        {
            try
            {
                await Task.Delay(10);
                barReportesVentas.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from r in db.Reportes
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(7) && re.ModuloID.Equals(7)
                           select new
                           {
                               ReporteID = r.ID,
                               ReporteNombre = r.Nombre,
                               ReporteOrden = r.Orden,
                               EtReporte = r,
                               EstructuraID = re.ID,
                               EstructuraNombre = re.Nombre,
                               EstructuraOrden = re.Orden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barReportesVentas.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

                //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //db.CommandTimeout = 200;
                //var Rpts = db.Reportes.Where(r => !r.EsSubReporte && r.ModuloID.Equals(7)).OrderBy(o => o.Orden).ToList();

                //if (Rpts.Count(c => c.EstructuraID.Equals(0)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                //    barRptTunel.Caption = "Vía Tunel";
                //    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesVentas.AddItem(barRptTunel);

                //    Rpts.Where(r => r.EstructuraID.Equals(0)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptTunel.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}

                //if (Rpts.Count(c => c.EstructuraID.Equals(1)) > 0)
                //{
                //    DevExpress.XtraBars.BarSubItem barRptWeb = new DevExpress.XtraBars.BarSubItem();
                //    barRptWeb.Caption = "Vía Internet";
                //    barRptWeb.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                //    barRptWeb.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                //    barRptWeb.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //    barReportesVentas.AddItem(barRptWeb);

                //    Rpts.Where(r => r.EstructuraID.Equals(1)).ToList().ForEach(rep =>
                //    {
                //        var btn = new Parametros.MyBarButtonItem(rep.Nombre);
                //        btn.Tag = rep;
                //        barRptWeb.AddItem(btn);
                //        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ItemClick);
                //    });
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #region <<< REPORTES SSRS >>>
        async Task RptSSRS(DevExpress.XtraBars.BarSubItem bsi, int IDModulo)
        {
            try
            {
                await Task.Delay(10);
                bsi.ClearLinks();
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 200;
                var Rpts = from AR in db.GetViewReportesSSRSByUsers.Where(w => w.UsuarioID == Parametros.General.UserID)// && w.EstacionServicioID == Parametros.General.EstacionServicioID)
                           join r in db.ReportesSSRs on AR.ReportesSSRSID equals r.ID
                           join re in db.ReportesEstructuras on r.EstructuraID equals re.ID
                           where !r.EsSubReporte && r.ModuloID.Equals(IDModulo) && re.ModuloID.Equals(IDModulo)
                           group r by new {r.ID, r.Nombre, r.Orden, EstructuraID = re.ID, r, EstructuraNombre = re.Nombre, EstructuraOrden = re.Orden} into gr
                           select new
                           {
                               ReporteID = gr.Key.ID,
                               ReporteNombre = gr.Key.Nombre,
                               ReporteOrden = gr.Key.Orden,
                               EtReporte = gr.Key.r,
                               EstructuraID = gr.Key.EstructuraID,
                               EstructuraNombre = gr.Key.EstructuraNombre,
                               EstructuraOrden = gr.Key.EstructuraOrden
                           };

                foreach (var det in Rpts.GroupBy(g => new { g.EstructuraID, g.EstructuraNombre, g.EstructuraOrden }).OrderBy(o => o.Key.EstructuraOrden))
                {
                    DevExpress.XtraBars.BarSubItem barRptTunel = new DevExpress.XtraBars.BarSubItem();
                    barRptTunel.Caption = det.Key.EstructuraNombre;
                    barRptTunel.Glyph = ((System.Drawing.Image)(Parametros.Properties.Resources.Reports_icon));
                    barRptTunel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    barRptTunel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    bsi.AddItem(barRptTunel);

                    Rpts.Where(r => r.EstructuraID.Equals(det.Key.EstructuraID)).OrderBy(o => o.ReporteOrden).ToList().ForEach(rep =>
                    {
                        var btn = new Parametros.MyBarButtonItem(rep.ReporteNombre);
                        btn.Tag = rep.EtReporte;
                        btn.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_ReportSSRS_ItemClick);
                        barRptTunel.AddItem(btn);
                    });

                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        #endregion

        #endregion

        #region <<< METODOS >>>

        private void SetPermisos(int UserID, int IDES, Entidad.SAGASDataClassesDataContext db)
        {
            try
            {

                var query = (from u in db.Usuarios
                             join acs in db.AccesoSistemas on u.ID equals acs.UsuarioID
                             join acc in db.Accesos on acs.AccesoID equals acc.ID
                             where u.ID.Equals(UserID)
                             select new
                             {
                                 acs.ID,
                                 acs.UsuarioID,
                                 acs.AccesoID,
                                 u.Nombre,
                                 NombreAcceso = acc.Nombre,
                                 acc.ModuloID,
                                 acc.Pantalla,
                                 acc.Componente,
                                 acc.PermiteMenu,
                             }).GroupBy(g => new {g.AccesoID, g.ModuloID, g.Componente}).ToList();

                if (query.Count(o => o.Key.ModuloID.Equals(1)) > 0)
                {
                    this.barAdministracion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    this.barConfiguracion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                if (query.Count(o => o.Key.ModuloID.Equals(2)) > 0)
                    this.barArqueo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(3)) > 0)
                    this.barContabilidad.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(4)) > 0)
                    this.barNomina.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(5)) > 0)
                    this.barTesoreria.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(6)) > 0)
                    this.barInventario.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(7)) > 0)
                    this.barVentas.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                if (query.Count(o => o.Key.ModuloID.Equals(8)) > 0)
                    this.barActivoMenu.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                
                if (db.AccesoSistemas.Where(acs => acs.UsuarioID.Equals(UserID)).GroupBy(g => g.EstacionServicioID).Count() > 1)
                    this.barCambiarEstacion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                
                
                foreach (object item in ribbonControlParent.Items)
                {
                    
                    if (item.GetType() == typeof(DevExpress.XtraBars.BarButtonItem))
                    {
                        DevExpress.XtraBars.BarButtonItem btn = item as DevExpress.XtraBars.BarButtonItem;

                        if (btn.Tag == null)
                        {
                            btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                            if (query.Exists(q => q.Key.Componente.Equals(btn.Name)))
                                btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                        }
                    }

                    if (item.GetType() == typeof(DevExpress.XtraBars.BarSubItem))
                    {
                        DevExpress.XtraBars.BarSubItem btn = item as DevExpress.XtraBars.BarSubItem;
                        btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        if (query.Exists(q => q.Key.Componente.Equals(btn.Name)))
                        btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }

                }
                
                /*
                MessageBox.Show("Todos Always");
                foreach (object item in ribbonControlParent.Items)
                {

                    if (item.GetType() == typeof(DevExpress.XtraBars.BarButtonItem))
                    {
                        DevExpress.XtraBars.BarButtonItem btn = item as DevExpress.XtraBars.BarButtonItem;

                        if (btn.Tag == null)
                        {
                            btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            
                        }
                    }

                    if (item.GetType() == typeof(DevExpress.XtraBars.BarSubItem))
                    {
                        DevExpress.XtraBars.BarSubItem btn = item as DevExpress.XtraBars.BarSubItem;
                            btn.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }

                }
                */


                //barRecosteo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //barTrasladoActivoFijo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private bool FillDataStatus(Entidad.SAGASDataClassesDataContext db)
        {               
            barStaticItemUser.Caption = "Usuario: " + Parametros.General.UserName;

            if (db.Usuarios.Single(u => u.ID == Parametros.General.UserID).IsReset)
            {
                Administracion.Forms.Dialogs.DialogPass dg = new Administracion.Forms.Dialogs.DialogPass(Parametros.General.UserID);

                dg.Text = "Validar contraseña";
                dg.ShowDialog();

                if (dg.DialogResult != DialogResult.OK)
                {
                    this.Close();
                    return false;
                }
            }

            Parametros.General.EstacionServicioName = db.EstacionServicios.Single(es => es.ID == Parametros.General.EstacionServicioID).Nombre;
            barStaticItemES.Caption = "Estación de Servicio: " + Parametros.General.EstacionServicioName;
           
            if (Parametros.General.ListSES.Count.Equals(0))
            {
                this._BotSubEstscion.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                _lkSubEstacion.DataSource = null;
            }
            else if (!Parametros.General.ListSES.Count.Equals(0))
            {
                this._BotSubEstscion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                _lkSubEstacion.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                _lkSubEstacion.DisplayMember = "Nombre";
                _lkSubEstacion.ValueMember = "ID";
                _BotSubEstscion.EditValue = Parametros.General.SubEstacionID;
            }

            Parametros.General.AddLogBook(db, Parametros.TipoAccion.InicioSesion, "Inicio sesión usuario: " + Parametros.General.UserName.ToString(),this.Name);
            return true;
        }

        private bool FormCargado(string formName)
        {
            //foreach (System.Windows.Forms.Form form in this.MdiChildren)
            //{
            //    if (form.Name == formName)
            //    {
            //        form.Activate();
            //        return true;
            //    }
            //}
            //return false;
            return Parametros.Config.FormCargado(formName, this);
        }

        #region <<< FORM CHILD >>>

        private void ShowEstacionServicio()
        {

            if (!FormCargado("FormEstaciones"))
            {
                Administracion.Forms.FormEstaciones fr = new Administracion.Forms.FormEstaciones();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowEmpresa()
        {

            if (!FormCargado("FormEmpresa"))
            {
               
                Administracion.Forms.FormEmpresa fr = new Administracion.Forms.FormEmpresa();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowZona()
        {

            if (!FormCargado("FormZona"))
            {
            Administracion.Forms.FormZona fr = new Administracion.Forms.FormZona();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowAudit()
        {

            if (!FormCargado("FormAudit"))
            {
                Administracion.Forms.FormAudit fr = new Administracion.Forms.FormAudit();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowLicencias()
        {

            if (!FormCargado("FormLicencia"))
            {
                Parametros.Forms.FormLicencia fr = new Parametros.Forms.FormLicencia();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowSubEstacion()
        {

            if (!FormCargado("FormSubEstacion"))
            {
                Administracion.Forms.FormSubEstacion fr = new Administracion.Forms.FormSubEstacion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptFaltanteSobrante()
        {

            if (!FormCargado("FormRptFaltanteSobrante"))
            {
                Reportes.Arqueos.Forms.FormRptFaltanteSobrante fr = new Reportes.Arqueos.Forms.FormRptFaltanteSobrante();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptEfectivoRecibido()
        {

            if (!FormCargado("FormRptEfectivoRecibido"))
            {
                Reportes.Arqueos.Forms.FormRptEfectivoRecibido fr = new Reportes.Arqueos.Forms.FormRptEfectivoRecibido();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptVentasContado()
        {

            if (!FormCargado("FormRptVentasContado"))
            {
                Reportes.Arqueos.Forms.FormRptVentasContado fr = new Reportes.Arqueos.Forms.FormRptVentasContado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptExtraxionVerificacion()
        {

            if (!FormCargado("FormRptExtraxionVerificacion"))
            {
                Reportes.Arqueos.Forms.FormRptExtraxionVerificacion fr = new Reportes.Arqueos.Forms.FormRptExtraxionVerificacion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowColores()
        {

            if (!FormCargado("FormColor"))
            {
                Arqueo.Forms.FormColor fr = new Arqueo.Forms.FormColor();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoCuenta()
        {

            if (!FormCargado("FormTipoCuenta"))
            {
                Contabilidad.Forms.FormTipoCuenta fr = new Contabilidad.Forms.FormTipoCuenta();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCatalogoCuenta()
        {

            if (!FormCargado("FormCuentaContable"))
            {
                Contabilidad.Forms.FormCuentaContable fr = new Contabilidad.Forms.FormCuentaContable();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowDepartamento()
        {

            if (!FormCargado("FormDepartamento"))
            {
                Administracion.Forms.FormDepartamento fr = new Administracion.Forms.FormDepartamento();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowProveedor()
        {

            if (!FormCargado("FormProveedores"))
            {
                Inventario.Forms.FormProveedores fr = new Inventario.Forms.FormProveedores();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoCambio()
        {

            if (!FormCargado("FormTipoCambio"))
            {
                Administracion.Forms.FormTipoCambio fr = new Administracion.Forms.FormTipoCambio();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCompras()
        {

            if (!FormCargado("FormCompras"))
            {
                Inventario.Forms.FormCompras fr = new Inventario.Forms.FormCompras();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCentroCosto()
        {

            if (!FormCargado("FormCentroCosto"))
            {
                Contabilidad.Forms.FormCentroCosto fr = new Contabilidad.Forms.FormCentroCosto();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowProvisiones()
        {

            if (!FormCargado("FormProvisiones"))
            {
                Contabilidad.Forms.FormProvisiones fr = new Contabilidad.Forms.FormProvisiones();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowAlmacenes()
        {

            if (!FormCargado("FormAlmacen"))
            {
                Inventario.Forms.FormAlmacen fr = new Inventario.Forms.FormAlmacen();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowKardex()
        {

            if (!FormCargado("FormRptKardex"))
            {
                Inventario.Forms.FormRptKardex fr = new Inventario.Forms.FormRptKardex();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowOrdenCompra()
        {

            if (!FormCargado("FormOrdenCompras"))
            {
                Inventario.Forms.FormOrdenCompras fr = new Inventario.Forms.FormOrdenCompras();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowListaPrecio()
        {

            if (!FormCargado("FormListaPrecio"))
            {

                Ventas.Forms.FormListaPrecio fr = new Ventas.Forms.FormListaPrecio();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowVentas()
        {

            if (!FormCargado("FormVentas"))
            {
                Ventas.Forms.FormVentas fr = new Ventas.Forms.FormVentas();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCliente()
        {
            if (!FormCargado("FormClientes"))
            {
                Ventas.Forms.FormClientes fr = new Ventas.Forms.FormClientes();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowInicio()
        {

            if (!FormCargado("FormInicioArqueo"))
            {
                Arqueo.Forms.FormInicioArqueo fr = new Arqueo.Forms.FormInicioArqueo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        

        private void ShowExistencia()
        {

            if (!FormCargado("FormExistencia"))
            {
                Ventas.Forms.FormExistencia fr = new Ventas.Forms.FormExistencia();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowInventarioFisico()
        {

            if (!FormCargado("FormInventarioFisico"))
            {
                Inventario.Forms.FormInventarioFisico fr = new Inventario.Forms.FormInventarioFisico();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowAjusteInventario()
        {

            if (!FormCargado("FormAjusteInventario"))
            {
                Inventario.Forms.FormAjusteInventario fr = new Inventario.Forms.FormAjusteInventario();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowInventarioCombustible()
        {

            if (!FormCargado("FormInventarioCombustible"))
            {
                Inventario.Forms.FormInventarioCombustible fr = new Inventario.Forms.FormInventarioCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowActaCombustible()
        {

            if (!FormCargado("FormActaCombustible"))
            {
                Inventario.Forms.FormActaCombustible fr = new Inventario.Forms.FormActaCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowSalidaCombustible()
        {

            if (!FormCargado("FormAutoConsumo"))
            {
                Inventario.Forms.FormAutoConsumo fr = new Inventario.Forms.FormAutoConsumo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        

        private void ShowEntradaSalidaInventario()
        {

            if (!FormCargado("FormEntradaSalidaInventario"))
            {
                Inventario.Forms.FormEntradaSalidaInventario fr = new Inventario.Forms.FormEntradaSalidaInventario();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowConceptoContable()
        {

            if (!FormCargado("FormConceptosContables"))
            {
                Contabilidad.Forms.FormConceptosContables fr = new Contabilidad.Forms.FormConceptosContables();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowDevolucionConversion()
        {

            if (!FormCargado("FormDevolucionConversion"))
            {
                Inventario.Forms.FormDevolucionConversion fr = new Inventario.Forms.FormDevolucionConversion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowEntradaManejo()
        {

            if (!FormCargado("FormEntradaManejo"))
            {
                Inventario.Forms.FormEntradaManejo fr = new Inventario.Forms.FormEntradaManejo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowSalidaManejo()
        {

            if (!FormCargado("FormSalidaManejo"))
            {
                Inventario.Forms.FormSalidaManejo fr = new Inventario.Forms.FormSalidaManejo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowComprobanteArqueo()
        {

            if (!FormCargado("FormComprobanteArqueo"))
            {
                Ventas.Forms.FormComprobanteArqueo fr = new Ventas.Forms.FormComprobanteArqueo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowVentaCreditoCombustible()
        {

            if (!FormCargado("FormVentaCreditoCombustible"))
            {
                Ventas.Forms.FormVentaCreditoCombustible fr = new Ventas.Forms.FormVentaCreditoCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowROC()
        {

            if (!FormCargado("FormROC"))
            {
                Ventas.Forms.FormROC fr = new Ventas.Forms.FormROC();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowNotas()
        {

            if (!FormCargado("FormNotasDC"))
            {
                Ventas.Forms.FormNotasDC fr = new Ventas.Forms.FormNotasDC();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPrestamoManejo()
        {

            if (!FormCargado("FormPrestamoManejo"))
            {
                Inventario.Forms.FormPrestamoManejo fr = new Inventario.Forms.FormPrestamoManejo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowECCliente()
        {
            Ventas.Forms.FormRptEstadoClientes fr = new Ventas.Forms.FormRptEstadoClientes();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
        }

        private void ShowIntegracionSaldoCliente()
        {
            try
            {
                if (!FormCargado("FormRptIntegracionCuentaCliente"))
                {
                    Ventas.Forms.FormRptIntegracionCuentaCliente fr = new Ventas.Forms.FormRptIntegracionCuentaCliente();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void ShowCheques()
        {

            if (!FormCargado("FormCheque"))
            {
                Tesoreria.Forms.FormCheque fr = new Tesoreria.Forms.FormCheque();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCajaChica()
        {

            if (!FormCargado("FormCajaChica"))
            {
                Tesoreria.Forms.FormCajaChica fr = new Tesoreria.Forms.FormCajaChica();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowInventarioUpload()
        {

            if (!FormCargado("FormInventarioUpload"))
            {
                Inventario.Forms.FormInventarioUpload fr = new Inventario.Forms.FormInventarioUpload();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPlantillaCliente()
        {

            if (!FormCargado("FormClienteUpload"))
            {
                Ventas.Forms.FormClienteUpload fr = new Ventas.Forms.FormClienteUpload();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPlantillaProveedor()
        {

            if (!FormCargado("FormProveedorUpload"))
            {
                Inventario.Forms.FormProveedorUpload fr = new Inventario.Forms.FormProveedorUpload();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowComprobantes()
        {

            if (!FormCargado("FormComprobanteContable"))
            {
                Contabilidad.Forms.FormComprobanteContable fr = new Contabilidad.Forms.FormComprobanteContable();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowLiquidaciones()
        {

            if (!FormCargado("FormLiquidaciones"))
            {
                Ventas.Forms.FormLiquidaciones fr = new Ventas.Forms.FormLiquidaciones();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPagosManuales()
        {

            if (!FormCargado("FormPagoManual"))
            {
                Tesoreria.Forms.FormPagoManual fr = new Tesoreria.Forms.FormPagoManual();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }
        
        private void ShowTipoActivo()
        {

            if (!FormCargado("FormTipoActivo"))
            {
                ActivoFijo.Forms.FormTipoActivo fr = new ActivoFijo.Forms.FormTipoActivo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowActivoFijo()
        {

            if (!FormCargado("FormActivo"))
            {
                ActivoFijo.Forms.FormActivo fr = new ActivoFijo.Forms.FormActivo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoMovActivo()
        {

            if (!FormCargado("FormTipoMovimientoActivo"))
            {
                ActivoFijo.Forms.FormTipoMovimientoActivo fr = new ActivoFijo.Forms.FormTipoMovimientoActivo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowBien()
        {

            if (!FormCargado("FormBien"))
            {
                ActivoFijo.Forms.FormBien fr = new ActivoFijo.Forms.FormBien();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRecosteo()
        {

            if (!FormCargado("FormRecosteo"))
            {
                Inventario.Forms.FormRecosteo fr = new Inventario.Forms.FormRecosteo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowMovimientoDetalle()
        {

            if (!FormCargado("FormRptDetalleCuenta"))
            {
                Contabilidad.Forms.FormRptDetalleCuenta fr = new Contabilidad.Forms.FormRptDetalleCuenta();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptBalanzaComprobacion()
        {

            if (!FormCargado("FormRptBalanzaComprobacion"))
            {
                Contabilidad.Forms.FormRptBalanzaComprobacion fr = new Contabilidad.Forms.FormRptBalanzaComprobacion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowAreaNomina()
        {

            if (!FormCargado("FormAreaNomina"))
            {
                Nomina.Forms.FormAreaNomina fr = new Nomina.Forms.FormAreaNomina();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCargos()
        {

            if (!FormCargado("FormCargo"))
            {
                Nomina.Forms.FormCargo fr = new Nomina.Forms.FormCargo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowProfesion()
        {

            if (!FormCargado("FormProfesion"))
            {
                Nomina.Forms.FormProfesion fr = new Nomina.Forms.FormProfesion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowEstructuraOrganizativa()
        {

            if (!FormCargado("FormEstruckOrgani"))
            {
                Nomina.Forms.FormEstruckOrgani fr = new Nomina.Forms.FormEstruckOrgani();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoAusencia()
        {

            if (!FormCargado("FormTipoAusencia"))
            {
                Nomina.Forms.FormTipoAusencia fr = new Nomina.Forms.FormTipoAusencia();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCarterasVencidas()
        {

            if (!FormCargado("FormCarteraVencidas"))
            {
                Ventas.Forms.FormCarteraVencidas fr = new Ventas.Forms.FormCarteraVencidas();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowEstadoCuentaProveedor()
        {

            if (!FormCargado("FormRptEstadoProveedor"))
            {
                Tesoreria.Forms.FormRptEstadoProveedor fr = new Tesoreria.Forms.FormRptEstadoProveedor();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCierreContable()
        {

            if (!FormCargado("FormCierre"))
            {
                Contabilidad.Forms.FormCierre fr = new Contabilidad.Forms.FormCierre();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        

        private void ShowBalanceGeneral()
        {

            if (!FormCargado("FormRptBalanceGeneral"))
            {
                Contabilidad.Forms.FormRptBalanceGeneral fr = new Contabilidad.Forms.FormRptBalanceGeneral();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowEstadoResultado()
        {
            if (!FormCargado("FormRptEstadoResultado"))
            {
                Contabilidad.Forms.FormRptEstadoResultado fr = new Contabilidad.Forms.FormRptEstadoResultado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowCompensacionAnticipos()
        {
            if (!FormCargado("DialogCompensacionAnticipos"))
            {
                Ventas.Forms.Dialogs.DialogCompensacionAnticipos fr = new Ventas.Forms.Dialogs.DialogCompensacionAnticipos(Parametros.General.UserID);
                fr.Text = "Nueva Compensación";
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowMovimientoEmpleado()
        {

            if (!FormCargado("FormMovimientoEmpleado"))
            {
                Nomina.Forms.FormMovimientoEmpleado fr = new Nomina.Forms.FormMovimientoEmpleado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }
        
        private void ShowTrasladoExterno()
        {
            if (!FormCargado("FormTrasladoExterno"))
            {
                Inventario.Forms.FormTrasladoExterno fr = new Inventario.Forms.FormTrasladoExterno();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowMovimientoCorteHE()
        {
            if (!FormCargado("FormMovimientoCorteEmpleado"))
            {
                Nomina.Forms.FormMovimientoCorteEmpleado fr = new Nomina.Forms.FormMovimientoCorteEmpleado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowGenerarPlanilla()
        {
            if (!FormCargado("FormPlanillasGeneradas"))
            {
                Nomina.Forms.FormPlanillasGeneradas fr = new Nomina.Forms.FormPlanillasGeneradas();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }
        
        private void ShowTrasladoEmpleado()
        {
            if (!FormCargado("FormTrasladoEmpleado"))
            {
                Nomina.Forms.FormTrasladoEmpleado fr = new Nomina.Forms.FormTrasladoEmpleado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowLiquidacionesEmpleados()
        {
            if (!FormCargado("FormLiquidacionesEmpleados"))
            {
                Nomina.Forms.FormLiquidacionesEmpleados fr = new Nomina.Forms.FormLiquidacionesEmpleados();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }
        
        private void ShowVacaciones()
        {
            if (!FormCargado("FormVacaciones"))
            {
                Nomina.Forms.FormVacaciones fr = new Nomina.Forms.FormVacaciones();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowAguinaldo()
        {
            if (!FormCargado("FormPagoAguinaldo"))
            {
                Nomina.Forms.FormPagoAguinaldo fr = new Nomina.Forms.FormPagoAguinaldo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowDepreciacion()
        {
            if (!FormCargado("FormDepreciacion"))
            {
                ActivoFijo.Forms.FormDepreciacion fr = new ActivoFijo.Forms.FormDepreciacion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowInventarioCierre()
        {
            if (!FormCargado("FormCierreInventario"))
            {
                Inventario.Forms.FormCierreInventario fr = new Inventario.Forms.FormCierreInventario();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        } 

        private void ShowListaServicios()
        {

            if (!FormCargado("FormListaServicios"))
            {
                Ventas.Forms.FormListaServicios fr = new Ventas.Forms.FormListaServicios();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        

        private void ShowCuentasBancaria()
        {

            if (!FormCargado("FormCuentaBancaria"))
            {
                Tesoreria.Forms.FormCuentaBancaria fr = new Tesoreria.Forms.FormCuentaBancaria();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoCliente()
        {

            if (!FormCargado("FormTipoCliente"))
            {
                Ventas.Forms.FormTipoCliente fr = new Ventas.Forms.FormTipoCliente();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        
        
        private void ShowListaReportsSSRS()
        {

            if (!FormCargado("FormReportesSSRS"))
            {
                Administracion.Forms.FormReportesSSRS fr = new Administracion.Forms.FormReportesSSRS(this);
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowListaReports()
        {

            if (!FormCargado("FormReportes"))
            {
                Administracion.Forms.FormReportes fr = new Administracion.Forms.FormReportes();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }        

        private void ShowMDIReports()
        {

            if (!FormCargado("MDIReports"))
            {
                Reportes.MDIReports fr = new Reportes.MDIReports();
                fr.Owner = this;
                //fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTablasImpuestos()
        {

            if (!FormCargado("FormTablaImpuesto"))
            {
                Contabilidad.Forms.FormTablaImpuesto fr = new Contabilidad.Forms.FormTablaImpuesto();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptFormasPago()
        {

            if (!FormCargado("FormRptFormasPago"))
            {
                Reportes.Arqueos.Forms.FormRptFormasPago fr = new Reportes.Arqueos.Forms.FormRptFormasPago();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowRptDistribucionExtraxion()
        {

            if (!FormCargado("FormRptDistribucionExtraxion"))
            {
                Reportes.Arqueos.Forms.FormRptDistribucionExtraxion fr = new Reportes.Arqueos.Forms.FormRptDistribucionExtraxion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTanque()
        {

            if (!FormCargado("FormTanque"))
            {
                Arqueo.Forms.FormTanque fr = new Arqueo.Forms.FormTanque();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowIsla()
        {

            if (!FormCargado("FormIsla"))
            {
                Arqueo.Forms.FormIsla fr = new Arqueo.Forms.FormIsla();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowDispensador()
        {

            if (!FormCargado("FormDispensador"))
            {
                Arqueo.Forms.FormDispensador fr = new Arqueo.Forms.FormDispensador();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowManguera()
        {

            if (!FormCargado("FormManguera"))
            {
                Arqueo.Forms.FormManguera fr = new Arqueo.Forms.FormManguera();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPrecioCombustible()
        {

            if (!FormCargado("FormPrecioCombustible"))
            {
                Arqueo.Forms.FormPrecioCombustible fr = new Arqueo.Forms.FormPrecioCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowFormExtracionPago()
        {

            if (!FormCargado("FormExtracionPago"))
            {
                Arqueo.Forms.FormExtracionPago fr = new Arqueo.Forms.FormExtracionPago();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowArqueoIsla()
        {

            if (!FormCargado("FormArqueoIsla"))
            {
                Arqueo.Forms.FormArqueoIsla fr = new Arqueo.Forms.FormArqueoIsla();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowResumenDia()
        {

            if (!FormCargado("FormResumenDia"))
            {
                Arqueo.Forms.FormResumenDia fr = new Arqueo.Forms.FormResumenDia();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowTipoTanque()
        {

            if (!FormCargado("FormTipoTanque"))
            {
                Arqueo.Forms.FormTipoTanque fr = new Arqueo.Forms.FormTipoTanque();
                fr.Owner = this;
                fr.MdiParent = this;                
                fr.Show();
            }
        }

        private void ShowProductoClase()
        {

            if (!FormCargado("FormProductoClase"))
            {
                Administracion.Forms.FormProductoClase fr = new Administracion.Forms.FormProductoClase();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowUnidadMedida()
        {

            if (!FormCargado("FormUnidadMedida"))
            {
                Administracion.Forms.FormUnidadMedida fr = new Administracion.Forms.FormUnidadMedida();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowArea()
        {

            if (!FormCargado("FormArea"))
            {
                Administracion.Forms.FormArea fr = new Administracion.Forms.FormArea();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowProducto()
        {

            if (!FormCargado("FormProducto"))
            {
                Administracion.Forms.FormProducto fr = new Administracion.Forms.FormProducto();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowUsuario()
        {

            if (!FormCargado("FormUsuario"))
            {
            Administracion.Forms.FormUsuario fr = new Administracion.Forms.FormUsuario();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowPlanilla()
        {

            if (!FormCargado("FormPlanilla"))
            {
            Nomina.Forms.FormPlanilla fr = new Nomina.Forms.FormPlanilla();
            fr.Owner = this;
            fr.MdiParent = this;
            fr.Show();
            }
        }

        private void ShowEmpleado()
        {

            if (!FormCargado("FormEmpleado"))
            {
                Nomina.Forms.FormEmpleado fr = new Nomina.Forms.FormEmpleado();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowPlanVacaciones()
        {

            if (!FormCargado("FormPlanVacaciones"))
            {
                Nomina.Forms.FormPlanVacaciones fr = new Nomina.Forms.FormPlanVacaciones();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void ShowArqueoEfectivo()
        {
            bool abierto = false;
            foreach (System.Windows.Forms.Form form in Application.OpenForms)
            {
                if (form.Name == "DialogArqueoEfectivo")
                {
                    form.Activate();
                    abierto = true;
                }
            }

            if (!abierto)
            {
                Arqueo.Forms.Dialogs.DialogArqueoEfectivo fr = new Arqueo.Forms.Dialogs.DialogArqueoEfectivo();
                fr.Text = "Arqueo de Efectivo";
                fr.Owner = this;
                fr.Show();
            }
        }

        private void ShowArqueoTurno()
        {
            bool abierto = false;
            foreach (System.Windows.Forms.Form form in Application.OpenForms)
            {
                if (form.Name == "DialogArqueoTurno")
                {
                    form.Activate();
                    abierto = true;
                }
            }

            if (!abierto)
            {
                Arqueo.Forms.Dialogs.DialogArqueoTurno fr = new Arqueo.Forms.Dialogs.DialogArqueoTurno();
                fr.Text = "Arqueo de Turno";
                fr.Owner = this;
                fr.Show();
            }
        }

        #endregion

        #endregion

        #region <<< EVENTOS >>>

        #region <<< CONFIGURACION >>>

        private void barConexion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Parametros.Forms.Dialogs.DialogConexion dg = new Parametros.Forms.Dialogs.DialogConexion())
                {
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barOpcionesGenerales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Administracion.Forms.Dialogs.DialogOpcionesGenerales dg = new Administracion.Forms.Dialogs.DialogOpcionesGenerales())
                {
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< ADMINISTRACION >>>

        private void barSucursal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEstacionServicio();
        }

        private void barEmpresa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEmpresa();
        }

        private void barZona_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowZona();
        }

        private void barTanque_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTanque();
        }

        private void barTipoTanque_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoTanque();
        }
  
        private void barUsuario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowUsuario();
        }

        #endregion

        #region <<< NOMINA >>>
    
        private void barPlanilla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPlanilla();
        }        

        private void barEmpleado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEmpleado();
        }

        private void barPlanVacaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPlanVacaciones();
        }

        #endregion

        private void barCategoriaProductos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowProductoClase();
        }

        private void barUnidadMedida_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowUnidadMedida();
        }

        private void barArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             ShowArea();
        }

        private void barProducto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowProducto();
        }

        private void barIsla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowIsla();
        }

        private void barDispensador_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowDispensador();
        }

        private void barManguera_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowManguera();
        }

        private void barPrecioCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPrecioCombustible();
        }

        private void barTipoExtraccionPago_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowFormExtracionPago();
        }

        private void barArqueoIsla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
              ShowArqueoIsla();
        }

        private void barCambiarEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int x = 0;
                if (FormCargado("FormInicioArqueo"))
                    x++;

                if (this.MdiChildren.Count() <= x)
                {
                    using (Administracion.Forms.Dialogs.DialogPass dg = new Administracion.Forms.Dialogs.DialogPass(Parametros.General.UserID))
                    {
                        dg.Text = "Seleccionar Estación de Servicio";
                        dg.EsCambioEstacion = true;

                        if (dg.ShowDialog() == DialogResult.OK)
                        {

                            if (Parametros.General.ListSES.Count.Equals(0))
                            {
                                this._BotSubEstscion.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                                _lkSubEstacion.DataSource = null;
                            }
                            else if (!Parametros.General.ListSES.Count.Equals(0))
                            {
                                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                                this._BotSubEstscion.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                                _lkSubEstacion.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                                _lkSubEstacion.DisplayMember = "Nombre";
                                _lkSubEstacion.ValueMember = "ID";

                                if (Parametros.General.ListSES.Count.Equals(1))
                                    Parametros.General.SubEstacionID = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                                
                                _BotSubEstscion.EditValue = Parametros.General.SubEstacionID;
                            }

                            barStaticItemES.Caption = "Estación de Servicio: " + Parametros.General.EstacionServicioName;

                            if (!Parametros.General.ListSES.Count.Equals(1))
                                this.Text = Parametros.General.Empresa.Nombre + " " + Parametros.General.EstacionServicioName;

                        }
                    }
                }
                else
                    if (Parametros.General.DialogMsg("Para cambiar de Estación de Servicios debe de cerrar todas las ventanas del sistema a exepción de la ventana de INICIO." + Environment.NewLine + "¿Desea cerrar todas las ventanas?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (System.Windows.Forms.Form form in this.MdiChildren)
                        {
                            form.Close();
                        }

                        Parametros.General.DialogMsg("Intente cambiar de estación nuevamente", Parametros.MsgType.message);

                    } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barCambiarContrasena_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Administracion.Forms.Dialogs.DialogPass dg = new Administracion.Forms.Dialogs.DialogPass(Parametros.General.UserID))
                {
                    dg.Text = "Cambiar Contraseña";
                    dg.EsCambioContrasena = true;
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barAuditoria_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAudit();
        }

        private void barResumenEfectivo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowArqueoEfectivo();
        }

        private void barResumenTurno_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowArqueoTurno();
        }

        private void barResumenDia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowResumenDia();
        }

        private void btnKey_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Parametros.Forms.Dialogs.DialogKeyGen dg = new Parametros.Forms.Dialogs.DialogKeyGen())
                {
                    dg.Text = "Generar Licencia";
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barListaLicencia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowLicencias();
        }

        private void barReportesFaltantesSobrantes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptFaltanteSobrante();
        }

        private void barSubEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowSubEstacion();
        }

        private void barImpresoraLocal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Parametros.Forms.Dialogs.DialogPrinter dg = new Parametros.Forms.Dialogs.DialogPrinter())
                {
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barReporteDistribucionExtraxion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptDistribucionExtraxion();
        }

        private void barRptFormaPago_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptFormasPago();
        }

        private void barRptEfectivoRecibido_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptEfectivoRecibido();
        }

        private void barRptVentasContado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptVentasContado();
        }

        private void barRptExtraxionVerificacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptExtraxionVerificacion();
        }

        private void barRptResumenCupones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (Reportes.Arqueos.Dialogs.DialogGetDataArqueo dg = new Reportes.Arqueos.Dialogs.DialogGetDataArqueo())
                {
                    //dg.Owner = this;
                    dg._MDIParent = this;
                    dg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void timerCnn_Tick(object sender, EventArgs e)
        {
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.ConnectionString = Parametros.Config.GetCadenaConexionString() + ";Connection Timeout=1";                
                try
                {
                    conn.Open();
                    this.barStaticIConn.Glyph = Properties.Resources.database_green_icon;
                    conn.Close();
                }
                catch { this.barStaticIConn.Glyph = Properties.Resources.database_red_icon; }
            }
        }

        private void barColoresTanques_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowColores();
        }

        private void barTipoCuenta_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoCuenta();
        }

        private void barCuentaContable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCatalogoCuenta();
        }

        private void barDepartamento_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowDepartamento();
        }

        private void barProveedor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowProveedor();
        }

        private void barTipoCambio_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoCambio();
        }
        
        private void barReportDesign_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMDIReports();
        }

        private void barReportUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowListaReports();

        }

        private void barReportSSRS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowListaReportsSSRS();
        }

        //*****************************//
        //*********REPORTES************//
        //*********REPORTES************//
        //*********REPORTES************//
        //*****************************//
        #region <<< REPORTES >>>

        private void btn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {                                
                var obj = ((Entidad.Reporte)((Parametros.MyBarButtonItem)(e.Item)).Tag);

                if (!String.IsNullOrEmpty(obj.Extension))
                {

                    if (obj.Extension.Equals(".repx"))
                    {
                        //Reporte en DevExpress
                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();

                        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                        DevExpress.XtraReports.UI.XtraReport rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                        /*Reportes.FormRptReport fr = new Reportes.FormRptReport(obj.ID);
                        fr.Owner = this;
                        fr.MdiParent = this;
                        fr.Show();*/
                      
                        using (DialogGetES dg = new DialogGetES())
                        {
                            dg.IDEstacionServicio = Parametros.General.EstacionServicioID;

                            if (dg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (!dg.IDEstacionServicio.Equals(0))
                                {
                                    rep.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter
                                    {
                                        Type = typeof(System.Int32),
                                        Value = dg.IDEstacionServicio,
                                        Visible = false,
                                        Name = "ESID"
                                    });

                                    rep.FilterString += " AND [EstacionServicioID] == ?ESID";

                                    if (dg.IDSubEstacion > 0)
                                    {

                                        rep.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter
                                        {
                                            Type = typeof(System.Int32),
                                            Value = dg.IDSubEstacion,
                                            Visible = false,
                                            Name = "SESID"
                                        });

                                        rep.FilterString += " AND [SubEstacionID] == ?SESID";
                                    }

                                    rv.Text = obj.Nombre;

                                    //Obtener el Data Source del Reporte
                                    //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                                    
                                    var ds = rep.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
                                    //Asignar la nueva cadena de conexion
                                    ds.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString() + @"; Network Library=DBMSSOCN; connection timeout=900";//   "Data Source=192.168.206.2; Network Library=DBMSSOCN; Initial Catalog=SAGAS;User ID=sa;Password=pa$$w0rd; connection timeout=900;"; // Parametros.Config.GetCadenaConexionString() + @"; Network Library=DBMSSOCN; connection timeout=900";
                                    //ds.Connection.Open();
                                    rep.DataSource = ds;

                                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                                    rv.Owner = this;
                                    rv.MdiParent = this;
                                    rep.CreateDocument(true);
                                    rv.Show();

                                    //DevExpress.XtraReports.Parameters.Parameter par = new DevExpress.XtraReports.Parameters.Parameter();

                                    //rep.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter{
                                    //>>> busqueda de subreports en las basndas del xtrareport <<<//
                                    foreach (Band b in rep.Bands)
                                    {
                                        if (b.Controls.Count > 0)
                                        {
                                            foreach (XRControl c in b.Controls)
                                            {
                                                if (c.GetType() == typeof(DevExpress.XtraReports.UI.XRSubreport))
                                                {
                                                    DevExpress.XtraReports.UI.XRSubreport sub = c as DevExpress.XtraReports.UI.XRSubreport;
                                                    //MessageBox.Show(sub.ReportSourceUrl.ToString());
                                                    sub.ReportSourceUrl = null;
                                                    Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                                                    System.IO.MemoryStream ms2 = new System.IO.MemoryStream(db.Reportes.Single(r => r.ID.Equals(1)).Archivo);
                                                    DevExpress.XtraReports.UI.XtraReport rep2 = DevExpress.XtraReports.UI.XtraReport.FromStream(ms2, true);

                                                    sub.ReportSource = rep2;
                                                }

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    rv.Text = obj.Nombre;
                                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                                    rv.Owner = this;
                                    rv.MdiParent = this;
                                    rep.CreateDocument(true);
                                    rv.Show();
                                }
                            }
                        }
                        
                    }
                    else if (obj.Extension.Equals(".rpt"))
                    {

                        //Reportes en Crystal

                        if (!System.IO.Directory.Exists(routeRpt))
                            System.IO.Directory.CreateDirectory(routeRpt);
                        routeRpt += @"\" + @obj.ArchivoNombre + obj.Extension;
                        File.WriteAllBytes(routeRpt, obj.Archivo);

                        if (System.IO.File.Exists(@routeRpt))
                        {
                            Reportes.FormCrystalViewer CR = new Reportes.FormCrystalViewer();
                            CR.Text = obj.Nombre;
                            CR.Tag = @routeRpt;
                            CR.crystalReportViewer1.ReportSource = @routeRpt;
                            CR.Refresh();
                            CR.Owner = this;
                            CR.MdiParent = this;
                            CR.Show();
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);                
                    }
                }
                else
                    Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());                
            }

        }
        
        private void btn_ReportSSRS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                string reportname = e.Item.Caption.ToString();
                //string textfile = gvData.GetFocusedRowCellValue(colNombreArchivo).ToString();
                if (!Parametros.Config.FormCargado(reportname, this))
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    SAGAS.Reportes.FormReportSSRS frs = new SAGAS.Reportes.FormReportSSRS(reportname/*, textfile*/);
                    frs.Owner = this;
                    frs.MdiParent = this;
                    frs.Name = reportname;
                    frs.Show();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        #endregion

        private void barCompras_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCompras();
        }

        //Seleccion de Sub Estación
        private void _BotSubEstscion_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_BotSubEstscion.EditValue != null)
                {
                    if (Convert.ToInt32(_BotSubEstscion.EditValue) > 0)
                    {
                         Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                         db.Usuarios.Single(u => u.ID.Equals(Parametros.General.UserID)).SubEstacionID = Convert.ToInt32(_BotSubEstscion.EditValue);
                         db.SubmitChanges();
                         Parametros.General.SubEstacionID = Convert.ToInt32(_BotSubEstscion.EditValue);
                         _lkSubEstacion.Appearance.BackColor = Color.Transparent;
                         this.Text = Parametros.General.Empresa.Nombre + " " + Parametros.General.EstacionServicioName + " | Sub Estación: " + db.SubEstacions.Single(s => s.ID.Equals(Parametros.General.SubEstacionID)).Nombre;
                    }
                    else
                    {
                        _lkSubEstacion.Appearance.BackColor = Color.Red;
                        this.Text = Parametros.General.Empresa.Nombre + " " + Parametros.General.EstacionServicioName;
                    }
                }
            }
            catch { this.barStaticIConn.Glyph = Properties.Resources.database_red_icon; }
        }

        private void barAlmacen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAlmacenes();
        }

        private void barProvisiones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowProvisiones();
        }

        private void barCentroCosto_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCentroCosto();
        }

        private void _lkSubEstacion_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                int x = 0;
                if (FormCargado("FormInicioArqueo"))
                    x++;

                if (this.MdiChildren.Count() > x)
                {
                    Parametros.General.DialogMsg("Para cambiar de la Sub Estación debe de cerrar todas las ventanas del sistema a exepción de la ventana de INICIO.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barKardex_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowKardex();
        }

        private void barOrdenCompra_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowOrdenCompra();
        }

        private void barListaPrecio_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowListaPrecio();
        }

        private void barVentasVarias_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowVentas();
        }

        private void barExistencia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowExistencia();
        }
        
        private void ribbonControlParent_ApplicationButtonDoubleClick(object sender, EventArgs e)
        {
            ShowInicio();
        }

        private void barInventarioFisico_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowInventarioFisico();
        }

        private void barCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCliente();
        }

        private void barAjusteInventario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAjusteInventario();
        }

        private void barEntradaSalidaInventario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEntradaSalidaInventario();
        }

        private void barConceptosContable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowConceptoContable();
        }

        private void barDevolucionConversion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowDevolucionConversion();
        }

        private void barEntradaManejo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEntradaManejo();
        }

        private void barTipoCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoCliente();
        }

        private void barSalidaManejo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowSalidaManejo();
        }

        private void barComprobanteArqueo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowComprobanteArqueo();
            //try
            //{
            //    int ID = 0;
            //    using (Reportes.Arqueos.Dialogs.DialogGetDataArqueo dg = new Reportes.Arqueos.Dialogs.DialogGetDataArqueo())
            //    {
            //        dg.Comprobante = true;
            //        dg._MDIParent = this;
            //        dg.ShowDialog();
            //        ID = dg.IDRD;
            //    }

            //    if (!ID.Equals(0))
            //    {

            //    //BORRAR
            //    //ID = 5507;

            //        Contabilidad.Forms.Dialogs.DialogComprobanteArqueo nf = new Contabilidad.Forms.Dialogs.DialogComprobanteArqueo();
            //        Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            //        nf.Owner = this;
            //        nf.MdiParent = this;
            //        nf.RD = db.ResumenDias.Single(r => r.ID.Equals(ID));
            //        nf.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //}
             
        }

        private void barVentaCreditoCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowVentaCreditoCombustible();
        }

        private void barRoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowROC();
        }

        private void barActaManejo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Inventario.Forms.FormRptActaConcManejo fr = new Inventario.Forms.FormRptActaConcManejo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            //using (Inventario.Forms.Dialogs.DialogGetManejo dg = new Inventario.Forms.Dialogs.DialogGetManejo())
            //{
            //    dg._MDIParent = this;
            //    dg.ShowDialog();
            //}
        }

        private void barCuentaBancaria_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCuentasBancaria();
        }

        private void barInventarioCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowInventarioCombustible();
        }

        private void barActaCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowActaCombustible();
        }

        private void barSalidaCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowSalidaCombustible();
        }

        private void barNotaDebitoCredito_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowNotas();
        }

        private void barPrestamoManejo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPrestamoManejo();
        }

        private void barECCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowECCliente();
        }

        private void barCheques_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCheques();
        }

        private void barCajaChica_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCajaChica();
        }

        private void barInventarioUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowInventarioUpload();
        }

        private void barClienteUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPlantillaCliente();
        }

        private void btnServicioPrecio_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowListaServicios();
        }

        private void barProveedoresUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPlantillaProveedor();
        }

        private void barComprobantesDiarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowComprobantes();
        }

        private void barLiquidacionesDeposito_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowLiquidaciones();
        }

        private void barPagosManuales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowPagosManuales();
        }

        private void barTipoActivo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoActivo();
        }

        private void barActivoFijo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowActivoFijo();
        }

        private void barTipoMovActivo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoMovActivo();
        }

        private void barBien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowBien();
        }

        private void barRecosteo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRecosteo();
        }

        private void barMovimientoDetalle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMovimientoDetalle();
        }

        private void barBalanzaComprobacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowRptBalanzaComprobacion();
        }

        private void barAreaNomina_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAreaNomina();
        }

        private void barCargosEmpleados_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCargos();
        }

        private void barProfesion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowProfesion();
        }

        private void barEstructuraOrganizativa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEstructuraOrganizativa();
        }

        private void barTipoAusencias_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTipoAusencia();
        }

        private void barCarterasVencidas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCarterasVencidas();
        }

        private void barEstadoCuentaProveedor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEstadoCuentaProveedor();
        }

        private void barBalanceGeneral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowBalanceGeneral();
        }

        private void barCierreContable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCierreContable();
        }

        private void barEstadoResultado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowEstadoResultado();
        }

        private void barCompensacionAnticipos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowCompensacionAnticipos();
        }

        private void barMovimientoEmpleado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMovimientoEmpleado();
        }

        private void barTrasladoExterno_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTrasladoExterno();
        }

        private void barMovimientoCorteHoras_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMovimientoCorteHE();
        }

        private void barGenerarPlanilla_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowGenerarPlanilla();
        }

        private void barTrasladoEmpleado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowTrasladoEmpleado();
        }

        private void barLiquidacionEmpleado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowLiquidacionesEmpleados();
        }

        private void barInventarioCierre_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowInventarioCierre();
        }

        private void barVacaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowVacaciones();
        }

        private void barAguinaldo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowAguinaldo();
        }

        private void barDepreciacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowDepreciacion();
        }

        private void barDetalleDepreciaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormDetalleDepreciacion"))
                {
                    ActivoFijo.Forms.FormDetalleDepreciacion fr = new ActivoFijo.Forms.FormDetalleDepreciacion();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barChequesFinalizados_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormChequesFinalizados"))
                {
                    Tesoreria.Forms.FormChequesFinalizados fr = new Tesoreria.Forms.FormChequesFinalizados();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barProveedorUso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormProveedorUso"))
                {
                    Tesoreria.Forms.FormProveedorUso fr = new Tesoreria.Forms.FormProveedorUso();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barTrasladoActivoFijo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormTrasladoBien"))
                {
                    ActivoFijo.Forms.FormTrasladoBien fr = new ActivoFijo.Forms.FormTrasladoBien();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barBajaActivoFijo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormBajaBien"))
                {
                    ActivoFijo.Forms.FormBajaBien fr = new ActivoFijo.Forms.FormBajaBien();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barTipoMov_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormTipoCuentaMovimiento"))
                {
                    Administracion.Forms.FormTipoCuentaMovimiento fr = new Administracion.Forms.FormTipoCuentaMovimiento();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barIntegracionSaldoCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowIntegracionSaldoCliente();
        }

        private void barCierrePeriodoFiscal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                if (!FormCargado("FormCierrePeriodoFiscal"))
                {
                    Contabilidad.Forms.FormCierrePeriodoFiscal fr = new Contabilidad.Forms.FormCierrePeriodoFiscal();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barIntegracionSaldoProveedor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Tesoreria.Forms.FormRptIntegracionCuentaProveedor fr = new Tesoreria.Forms.FormRptIntegracionCuentaProveedor();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barChequeAnticipo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Tesoreria.Forms.FormRptAnticipoCheque fr = new Tesoreria.Forms.FormRptAnticipoCheque();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormEmpleadoIR"))
                {
                    Nomina.Forms.FormEmpleadoIR fr = new Nomina.Forms.FormEmpleadoIR();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void baFormatoCheque_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormFormato"))
                {
                    Tesoreria.Forms.FormFormato fr = new Tesoreria.Forms.FormFormato();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barRetenciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormRetenciones"))
                {
                    Tesoreria.Forms.FormRetenciones fr = new Tesoreria.Forms.FormRetenciones();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barSerieRetenciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormSerieRetenciones"))
                {
                    Tesoreria.Forms.FormSerieRetenciones fr = new Tesoreria.Forms.FormSerieRetenciones();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barPrestacionesLaborales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                    Nomina.Forms.FormPrestaciones fr = new Nomina.Forms.FormPrestaciones();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barCambioLaboral_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Nomina.Forms.FormRptCambios fr = new Nomina.Forms.FormRptCambios();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barTipoMovEmpleado_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!FormCargado("FormTipoMovEmpleado"))
                {
                    Nomina.Forms.FormTipoMovEmpleado fr = new Nomina.Forms.FormTipoMovEmpleado();
                    fr.Owner = this;
                    fr.MdiParent = this;
                    fr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void barProductoVariacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormProductoVariacion"))
            {
                Inventario.Forms.FormProductoVariacion fr = new Inventario.Forms.FormProductoVariacion();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        //private void ShowIntegracionCuentaProveedor()
        //{
        //    if (!FormCargado("FormRptIntegracionCuentaProveedor"))
        //    {
        //        Tesoreria.Forms.FormRptIntegracionCuentaProveedor fr = new Tesoreria.Forms.FormRptIntegracionCuentaProveedor();
        //        fr.Owner = this;
        //        fr.MdiParent = this;
        //        fr.Show();
        //    }
        //}

        #endregion

        private void BarConceptosOperativos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormConceptoOperativo"))
            {
                Contabilidad.Forms.FormConceptoOperativo fr = new Contabilidad.Forms.FormConceptoOperativo();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void BarPagosOperativos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormPagos"))
            {
                Tesoreria.Forms.FormPagos fr = new Tesoreria.Forms.FormPagos();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void barRetiro_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormRetiro"))
            {
                Ventas.Forms.FormRetiro fr = new Ventas.Forms.FormRetiro();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void barCostoCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormCostoCombustible"))
            {
                Arqueo.Forms.FormCostoCombustible fr = new Arqueo.Forms.FormCostoCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void barOrdenCombPedido_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormOrdenCombPedido"))
            {
                Arqueo.Forms.FormOrdenCombPedido fr = new Arqueo.Forms.FormOrdenCombPedido();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void barPedidoCombustible_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormPedidoCombustible"))
            {
                Inventario.Forms.FormPedidoCombustible fr = new Inventario.Forms.FormPedidoCombustible();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }

        private void barTerminal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!FormCargado("FormTerminal"))
            {
                Administracion.Forms.FormTerminal fr = new Administracion.Forms.FormTerminal();
                fr.Owner = this;
                fr.MdiParent = this;
                fr.Show();
            }
        }
    }

}
