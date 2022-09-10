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

namespace SAGAS.Inventario.Forms
{
    public partial class FormRptActaConcManejo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<StEstaciones> EtEstaciones = new List<StEstaciones>();
        private List<StSubEstaciones> EtSubEstaciones = new List<StSubEstaciones>();
        private BackgroundWorker bgw;
        private BackgroundWorker bgwGrid;
        Reportes.Inventario.Hojas.RptActaManejo MRep;

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

        public FormRptActaConcManejo()
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
         

               var Est = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                 select new StEstaciones 
                                 { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).ToList();

                //var SubEst = db.SubEstacions.Where(ses => ses.Activo && Est.Select(es => es.ID).Contains(ses.EstacionServicioID)).Select(s => new StSubEstaciones { ID = s.ID, Nombre = s.Nombre, EstacionServicioID = s.EstacionServicioID, Activo = s.Activo }).ToList();
                
                //Est.Insert(0, new StEstaciones { ID = 0, Nombre = "TODOS", Activo = true });

                //if (SubEst.Count > 0)
                //    SubEst.Insert(0, new StSubEstaciones { ID = 0, Nombre = "TODOS", EstacionServicioID = 0, Activo = true });

                EtEstaciones = Est.ToList();
                //EtSubEstaciones = SubEst.ToList();
                
                //LOOKUPS
                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre});
                IDEstacion = Parametros.General.EstacionServicioID;

                dateInicial.EditValue = dateFinal.EditValue = DateTime.Now.Date;

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
                /*
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

                */
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
            try
            {
                if (lkES.EditValue != null)
                {

                    glkCliente.Properties.DataSource = null;
                    glkCliente.Properties.DataSource = (from c in db.Clientes
                                                        join ces in db.ClienteEstacions on c.ID equals ces.ClienteID
                                                        where ces.EstacionServicioID.Equals(IDEstacion) && c.Activo && c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID())
                                                        group c by new { ces.ClienteID, c.Codigo, c.Nombre } into gr
                                                        select new
                                                        {
                                                            ID = gr.Key.ClienteID,
                                                            Codigo = gr.Key.Codigo,
                                                            Nombre = gr.Key.Nombre,
                                                            Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                        }).ToList();

                    lkCombustible.Properties.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
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
                
                Reportes.Inventario.Hojas.RptActaManejo rep = new Reportes.Inventario.Hojas.RptActaManejo();

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                db.CommandTimeout = dbView.CommandTimeout = 900;

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                rep.CeEstacion.Text = Convert.ToString(lkES.Text);
                rep.xrCeCliente.Text = Convert.ToString(glkCliente.Text);

                var Es = db.EstacionServicios.Single(es => es.ID.Equals(Convert.ToInt32(lkES.EditValue)));

                if (!Es.AdministradorID.Equals(0))
                {
                    var empl = db.Empleados.Single(em => em.ID.Equals(Es.AdministradorID));
                    rep.xrCeAdmonNombre.Text = empl.Nombres + " " + empl.Apellidos;
                }

                DateTime FechaDesde = Convert.ToDateTime(dateInicial.EditValue);
                DateTime FechaHasta = Convert.ToDateTime(dateFinal.EditValue);

                rep.xrCeFechaInicial.Text = FechaDesde.ToShortDateString();
                rep.xrCeFechaFinal.Text = FechaHasta.ToShortDateString();

                var saldo = from vd in dbView.VistaDeudors
                            join vm in dbView.VistaMovimientos on vd.VistaMovimiento equals vm
                            where vm.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) && !vm.Anulado && vd.ClienteID.Equals(Convert.ToInt32(glkCliente.EditValue)) &&
                            (vd.ProductoID.Equals(Convert.ToInt32(lkCombustible.EditValue)) || Convert.ToInt32(lkCombustible.EditValue).Equals(0))
                            group vd by new { vd.ProductoID, vd.ProductoCodigo, vd.ProductoNombre, vd.ClienteID } into gr
                            select new
                            {
                                ProductoID = gr.Key.ProductoID,
                                Codigo = gr.Key.ProductoCodigo,
                                Nombre = gr.Key.ProductoNombre,
                                Total = (
                                gr.Count(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date) > 0
                                ? (gr.Where(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date).Sum(s => s.Valor) != 0m
                                ? gr.Where(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date).Sum(s => s.Valor)
                                : 0m)
                                : 0m)

                            };

                saldo.Where(o => !o.ProductoID.Equals(0)).ToList().ForEach(obj =>
                {
                    DevExpress.XtraReports.UI.GroupFooterBand GroupFooter = new DevExpress.XtraReports.UI.GroupFooterBand();
                    var Proc = dbView.GetActaManejo(Convert.ToInt32(lkES.EditValue), Convert.ToInt32(glkCliente.EditValue), obj.ProductoID, FechaDesde.Date, FechaHasta.Date).ToList();

                    DevExpress.XtraReports.UI.XRSubreport xrSubVerificacion = new DevExpress.XtraReports.UI.XRSubreport();

                    xrSubVerificacion.ReportSource = new Reportes.Inventario.Hojas.SubRptLitrosManejo(obj.Nombre, obj.Total, Decimal.Round(obj.Total / 3.785m, 3, MidpointRounding.AwayFromZero), Proc);
                    GroupFooter.Controls.Add(xrSubVerificacion);
                    rep.Bands.Add(GroupFooter);
                });

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rep.RequestParameters = false;
                //rep.CreateDocument();

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
                    MRep = ((Reportes.Inventario.Hojas.RptActaManejo)(e.Result));
                    MRep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = MRep.PrintingSystem;
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

        private void btnLoadGrid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    bgwGrid = new BackgroundWorker();
                    //bgwGrid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwGrid_RunWorkerCompleted);
                    //bgwGrid.DoWork += new DoWorkEventHandler(bgwGrid_DoWork);
                    bgwGrid.RunWorkerAsync();
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

        #endregion

        private void glkCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkCliente.EditValue) > 0)
                {
                    lkCombustible.Properties.DataSource = null;

                    var obj = (from d in db.Deudors
                               join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacion)) on d.MovimientoID equals m.ID
                               join p in db.Productos on d.ProductoID equals p.ID
                               join c in db.Clientes.Where(c => c.ID.Equals(Convert.ToInt32(glkCliente.EditValue))) on d.ClienteID equals c.ID
                               group d by new { d.ProductoID, p.Nombre } into gr
                               select new
                               {
                                   ID = gr.Key.ProductoID,
                                   Producto = gr.Key.Nombre
                               }).ToList();

                    if (obj.Count > 0)
                    {
                        List<Parametros.ListIdDisplay> lista = new List<Parametros.ListIdDisplay>();
                        lista.Add(new Parametros.ListIdDisplay { ID = 0, Display = "TODOS" });
                        obj.ForEach(item => { lista.Add(new Parametros.ListIdDisplay { ID = item.ID, Display = item.Producto }); });

                        lkCombustible.Properties.DataSource = lista;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


        
    }
}