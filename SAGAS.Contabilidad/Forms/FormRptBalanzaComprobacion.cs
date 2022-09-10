using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Linq;
using DevExpress.Data.Filtering;
using DevExpress.XtraPrinting;

namespace SAGAS.Contabilidad.Forms
{
    public partial class FormRptBalanzaComprobacion : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<StEstaciones> EtEstaciones = new List<StEstaciones>();
        private List<StSubEstaciones> EtSubEstaciones = new List<StSubEstaciones>();
        private List<Entidad.GetBalanzaComprobacionResult> EtVista;
        private List<Entidad.GetBalanzaComprobacionFusionadaResult> EtVistaFusion;
        private BackgroundWorker bgw;
        private BackgroundWorker bgwGrid;
        Reportes.Contabilidad.Hojas.RptBalanzaComprobacion rep;
        internal bool _Fusion = false;
        int AnioInicial = 0;
        int AnioFinal = 0;

        struct StEstaciones
        {
            public int ID;
            public string  Nombre;
            public bool Activo;
        };

        struct StSubEstaciones
        {
            public int ID;
            public string Nombre;
            public int EstacionServicioID;
            public bool Activo;
        };

        private int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        private DateTime _fechaInicial
        {
            get { return dateInicial.DateTime.Date; }
            set { dateInicial.EditValue = value; }
        }

        private DateTime _fechaFinal
        {
            get { return dateFinal.DateTime.Date; }
            set { dateFinal.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public FormRptBalanzaComprobacion()
        {
            InitializeComponent();
        }

        private void FormRptDetalleCuenta_Shown(object sender, EventArgs e)
        {
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
                chkCierre.Checked = true;
                _Fusion = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "swFusion");

                if (_Fusion)
                {
                    layoutControlItemSW.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    swFusion.IsOn = false;
                }

                splitReport.PanelVisibility = SplitPanelVisibility.Panel2;

                AnioInicial = DateTime.Now.Year;
                AnioFinal = DateTime.Now.Year;

               var Est = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                 select new StEstaciones 
                                 { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).ToList();

                //var SubEst = db.SubEstacions.Where(ses => ses.Activo && Est.Select(es => es.ID).Contains(ses.EstacionServicioID)).Select(s => new StSubEstaciones { ID = s.ID, Nombre = s.Nombre, EstacionServicioID = s.EstacionServicioID, Activo = s.Activo }).ToList();
                
                Est.Insert(0, new StEstaciones { ID = 0, Nombre = "TODOS", Activo = true });

                //if (SubEst.Count > 0)
                //    SubEst.Insert(0, new StSubEstaciones { ID = 0, Nombre = "TODOS", EstacionServicioID = 0, Activo = true });

                EtEstaciones = Est.ToList();
                //EtSubEstaciones = SubEst.ToList();
                
                //LOOKUPS
                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre});
                IDEstacion = Parametros.General.EstacionServicioID;

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;
                lkMesInicial_Validated(null, null);
                lkMesFinal.EditValue = DateTime.Now.Month;
                lkMesFinal_Validated(null, null);
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        public bool ValidarCampos()
        {
            if (!swFusion.IsOn)
            {
                if (lkES.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Estación de Servicio", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a recalcular.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            {
                Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                return false;
            }

            
            return true;
        }

        private void ImprimirActa(string TitleForm
           , System.Windows.Forms.Form mdi
           , DevExpress.XtraPivotGrid.PivotGridControl pivotGridMargin
           , DevExpress.XtraLayout.LayoutControl layOutTop
            , DevExpress.XtraLayout.LayoutControl layOutBottom
           , DevExpress.XtraGrid.GridControl gridcontrol
           , DevExpress.XtraGrid.GridControl gridCD
            , string _Fecha)
        {
            try
            {
                #region ::: Declaraciones :::
                DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
                DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink(ps);
                //DevExpress.XtraPrinting.PrintableComponentLink pclgrid = new DevExpress.XtraPrinting.PrintableComponentLink();
                //DevExpress.XtraPrinting.PrintableComponentLink pclgridCD = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridpiv = new DevExpress.XtraPrinting.PrintableComponentLink();
                //DevExpress.XtraPrinting.PrintableComponentLink pclLaoyoutTop = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclLaoyoutBottom = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PageHeaderFooter phf = (DevExpress.XtraPrinting.PageHeaderFooter)compositeLink.PageHeaderFooter;
                string SaltoLinea = System.Environment.NewLine;
                string param_Empresa = "", param_direccion = "", param_tel = "";
                System.Drawing.Image picture_LogoEmpresa = null;
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                #endregion

                #region ::: Datos de la Empresa :::
                Parametros.General.GetCompanyData(out param_Empresa,
                out param_direccion, out  param_tel, out picture_LogoEmpresa);

                string direccionResult = "", direccionTmp = "";
                if (!String.IsNullOrEmpty(param_direccion))
                {
                    string[] direccionSplit = param_direccion.Split(' ');

                    foreach (var i in direccionSplit)
                    {
                        direccionTmp += " " + i;
                        if (direccionTmp.Length >= 75)
                        {
                            direccionResult += SaltoLinea + i;
                            direccionTmp = "";
                        }
                        else direccionResult += " " + i;

                    }
                }

                #endregion

                #region ::: Creando Imagenes de Header y Fooder :::
                System.Windows.Forms.ImageList pict = new System.Windows.Forms.ImageList();
                if (picture_LogoEmpresa != null)
                {
                    pict.Images.Add(picture_LogoEmpresa);
                    pict.ImageSize = new Size(142, 67);

                    compositeLink.Images.Add(pict.Images[0]);
                }

                #endregion

                #region ::: Configuracion del reporte :::
                phf.Header.Content.Clear();
                phf.Header.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                phf.Header.Content.AddRange(new string[] { "[Image 0]", param_Empresa + SaltoLinea /*+ Estacion + (String.IsNullOrEmpty(SubEstacion) ? "" : " | " + SubEstacion) + SaltoLinea + " " + SaltoLinea */+ TitleForm });
                phf.Footer.Content.AddRange(new string[] { "[Image 1]", "Software de Administración de Gasolinera" });

                //pclLaoyoutTop.Component = layOutTop;
                pclLaoyoutBottom.Component = layOutBottom;
                //pclgrid.Component = gridcontrol;
                //pclgridCD.Component = gridCD;
                pclgridpiv.Component = pivotGridMargin;

                Link l2 = new Link();
                l2.CreateDetailArea += new CreateAreaEventHandler(l2_CreateDetailArea);

                Link l = new Link();
                l.CreateDetailArea += new CreateAreaEventHandler(l_CreateDetailArea);

                //compositeLink.Links.Add(pclLaoyoutTop);
                //compositeLink.Links.Add(pclgrid);
                compositeLink.Links.Add(pclgridpiv);
                compositeLink.Links.Add(l);
                compositeLink.Links.Add(pclLaoyoutBottom);
                //compositeLink.Links.Add(l2);
                //compositeLink.Links.Add(pclgridCD);
                //compositeLink.Links.Add(l);
                //compositeLink.Links.Add(pclLaoyoutBottom);
                compositeLink.CreateDocument();



                ps.PageSettings.Landscape = true;
                ps.PageSettings.LeftMargin = 20;
                ps.PageSettings.RightMargin = 10;
                ps.PageSettings.TopMargin = 100;
                ps.PageSettings.BottomMargin = 10;


                #endregion

                #region ::: Ejecucion del Reporte :::
                Reportes.FormReportViewer rForm = new Reportes.FormReportViewer();
                rForm.Text = TitleForm;
                rForm.MdiParent = mdi;

                ps.Document.ScaleFactor = 0.63f;
                rForm.printControlAreaReport.PrintingSystem = ps;

                rForm.Show();
                rForm.Activate();
                rForm.BringToFront();
                rForm.TopMost = true;

                #endregion
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        #endregion

        #region <<< EVENTOS >>>
        
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            //if (lkES.EditValue != null)
            //{
            //    if (IDEstacion > 0)
            //    {
            //        var Sus = EtSubEstaciones.Where(sus => sus.EstacionServicioID.Equals(IDEstacion) || sus.EstacionServicioID.Equals(0)).ToList();

            //        if (Sus.Count(c => !c.EstacionServicioID.Equals(0))  > 0)
            //        {
            //            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //            lkSUS.Properties.DataSource = Sus.Select(s => new { s.ID, s.Nombre });
            //            lkSUS.EditValue = null;
            //        }
            //        else
            //        {
            //            this.lkSUS.EditValue = null;
            //            this.lkSUS.Properties.DataSource = null;
            //            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //        }
            //    }
            //}
            //else
            //{
            //    this.lkSUS.EditValue = null;
            //    this.lkSUS.Properties.DataSource = null;
            //    this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}
        }
           
        private void lkMesInicial_Validated(object sender, EventArgs e)
        {
            if (lkMesInicial.EditValue != null)
            {
                dateInicial.EditValue = Convert.ToDateTime(new DateTime(AnioInicial, Convert.ToInt32(lkMesInicial.EditValue), 1));
                
                if (Convert.ToDateTime(dateInicial.EditValue).Month.Equals(12))
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                else
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            }
        }

        private void lkMesFinal_Validated(object sender, EventArgs e)
        {
            if (lkMesFinal.EditValue != null)
            {
                int mes = (Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? 1 : Convert.ToInt32(lkMesFinal.EditValue) + 1);
                DateTime fecha = new DateTime((Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? AnioFinal + 1 : AnioFinal), mes, 1);
                dateFinal.EditValue = fecha.AddDays(-1);

                if (Convert.ToDateTime(dateFinal.EditValue).Month.Equals(12))
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                else
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void dateInicial_EditValueChanged(object sender, EventArgs e)
        {
            if (dateInicial.EditValue != null)
            {
                lkMesInicial.EditValue = Convert.ToDateTime(dateInicial.EditValue).Month;
                AnioInicial = Convert.ToDateTime(dateInicial.EditValue).Year;

                if (Convert.ToDateTime(dateInicial.EditValue).Month.Equals(12))
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                else
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void dateFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (dateFinal.EditValue != null)
            {
                lkMesFinal.EditValue = Convert.ToDateTime(dateFinal.EditValue).Month;
                AnioFinal = Convert.ToDateTime(dateFinal.EditValue).Year;

                if (Convert.ToDateTime(dateFinal.EditValue).Month.Equals(12))
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                else
                    layoutControlItemCierre.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            } 
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    bgw = new BackgroundWorker();
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.RunWorkerAsync();
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
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dbView.CommandTimeout = 600;
                DateTime _Fecha = Convert.ToDateTime(db.GetDateServer());

                Reportes.Contabilidad.Hojas.RptBalanzaComprobacion MRep = new Reportes.Contabilidad.Hojas.RptBalanzaComprobacion();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);

                MRep.xrSetDate.Text = _Fecha.ToShortDateString() + "  " + _Fecha.ToShortTimeString();
                MRep.PicLogo.Image = picture_LogoEmpresa;
                MRep.CeEmpresa.Text = Nombre;
                MRep.CeRuc.Text = Ruc;
                MRep.xrCeRango.Text = "Del " + Convert.ToDateTime(_fechaInicial).ToShortDateString() + "   Al   " + Convert.ToDateTime(_fechaFinal).ToShortDateString();
                
                int ES = (int)lkES.EditValue;
                if (ES.Equals(0))
                {
                    EtVista = new List<Entidad.GetBalanzaComprobacionResult>();

                    foreach (var Sede in EtEstaciones.Where(o => !o.ID.Equals(0)))
                    {
                        EtVista.AddRange(dbView.GetBalanzaComprobacion(Sede.ID, 0,chkCierre.Checked, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList());
                    }
                }
                else
                {
                    EtVista = dbView.GetBalanzaComprobacion(ES, 0, chkCierre.Checked, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList();
                }

                MRep.DataSource = EtVista;
                e.Result = MRep;
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
                    rep = ((Reportes.Contabilidad.Hojas.RptBalanzaComprobacion)(e.Result));
                    rep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void swFusion_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (swFusion.IsOn)
                {
                    splitReport.PanelVisibility = SplitPanelVisibility.Panel1;
                    lkES.EditValue = null;
                    lkES.Enabled = false;
                    btnLoad.Enabled = false;
                    layoutControlItemSwBtn1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemSwBtn2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    pivotGridControl1.Visible = true;

                    if (printControlAreaReport.PrintingSystem != null)
                        printControlAreaReport.PrintingSystem.ClearContent();
                }
                else
                {
                    splitReport.PanelVisibility = SplitPanelVisibility.Panel2;
                    lkES.Enabled = true;
                    btnLoad.Enabled = true;
                    bdsCuentas.DataSource = null;
                    layoutControlItemSwBtn1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemSwBtn2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void btnLoadGrid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    bgwGrid = new BackgroundWorker();
                    bgwGrid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwGrid_RunWorkerCompleted);
                    bgwGrid.DoWork += new DoWorkEventHandler(bgwGrid_DoWork);
                    bgwGrid.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgwGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dbView.CommandTimeout = 600;
                EtVistaFusion = dbView.GetBalanzaComprobacionFusionada(chkCierre.Checked, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList();

                e.Result = EtVistaFusion;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        void bgwGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    bdsCuentas.DataSource = ((List<Entidad.GetBalanzaComprobacionFusionadaResult>)(e.Result));
                    pivotGridControl1.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnPrintFusion_Click(object sender, EventArgs e)
        {
            try
            {
                if (bdsCuentas.DataSource != null)
                {
                    //this.pivotGridControl1.ShowPrintPreview();

                    lblContadorGral.Text = "Lic." + Parametros.Config.ChargeSignature(3);
                    lblViceGteFinan.Text = "Lic." + Parametros.Config.ChargeSignature(49);
                    lblViceGteGral.Text = "Lic." + Parametros.Config.ChargeSignature(47);
                    lblGteGral.Text = "Lic." + Parametros.Config.ChargeSignature(1);

                    /*var Elaborado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(EntidadAnterior.EstacionServicioID)).ResponsableContableID)));
                    var Revisado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(EntidadAnterior.EstacionServicioID)).AdministradorID)));


                    if (Elaborado != null)
                        lblContadorGral.Text = Elaborado.Nombres + " " + Elaborado.Apellidos;
                    else
                        lblContadorGral.Text = "<>..........................";

                    if (Revisado != null)
                        lblViceGteFinan.Text = Revisado.Nombres + " " + Revisado.Apellidos;
                    else
                        lblViceGteFinan.Text = "<>..........................";
                    */
                    layoutControlBottom.Visible = false;

                    

                    //var Es = db.EstacionServicios.FirstOrDefault(f => f.ID.Equals(EntidadAnterior.EstacionServicioID));
                    //var Ses = db.SubEstacions.FirstOrDefault(f => f.ID.Equals(EntidadAnterior.SubEstacionID));
                    string _Fecha = Convert.ToDateTime(db.GetDateServer()).ToString();

                    emptyFecha.Text = (String.IsNullOrEmpty(_Fecha) ? "" : _Fecha);

                    ImprimirActa("Balanza de Comprobacion" + Environment.NewLine + "Del: " + _fechaInicial.Date.ToShortDateString() + " al " + _fechaFinal.Date.ToShortDateString(), this.MdiParent, pivotGridControl1, null, layoutControlBottom, null, null, _Fecha);
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void l2_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {
            e.Graph.PrintingSystem.InsertPageBreak(0);
        }

        void l_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {

            TextBrick tb = new TextBrick();
            tb.BackColor = Color.Transparent;
            tb.BorderColor = Color.Transparent;
            tb.Rect = new RectangleF(0, 0, 100, 200);
            e.Graph.DrawBrick(tb);
        }

        #endregion

        
    }
}