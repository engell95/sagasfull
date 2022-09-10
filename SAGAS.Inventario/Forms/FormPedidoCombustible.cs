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

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormPedidoCombustible : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPedidoCombustible nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListEstadosOrdenes listado = new Parametros.ListEstadosOrdenes();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private bool btnsRestricciones = false;
        private DevExpress.XtraBars.Bar barItems = new DevExpress.XtraBars.Bar();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        
        #endregion

        #region <<< INICIO >>> 

        public FormPedidoCombustible()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnsRestricciones = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnsRestriccionOrdenCompra");
            this.barTop.DockCol = 1;
            this.barTop.OptionsBar.UseWholeRow = false;
            this.barItems.Manager = barManager;
            this.barItems.OptionsBar.AllowQuickCustomization = false;
            this.barItems.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaOrdenesCompra);
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
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                if (Refresh)
                    linqInstantFeedbackSource1.Refresh();
                else
                {
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaCreado") > fecha.AddDays(-1)).ToString();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from oc in dv.VistaPedidoCombustibles
                            group oc by new { oc.PedidoID, oc.Anulado, oc.FechaCreado, oc.Numero, oc.Aprobado, oc.EsIECC, oc.Estacion, oc.Comentario, oc.EstacionServicioID, oc.Finalizado } into g
                            select new
                            {
                                ID = g.Key.PedidoID,
                                g.Key.Anulado,
                                g.Key.FechaCreado,
                                g.Key.Comentario,
                                g.Key.Numero,
                                g.Key.Aprobado,
                                g.Key.Finalizado,
                                g.Key.Estacion,
                                g.Key.EstacionServicioID,
                                g.Key.EsIECC,
                                FechaCarga = Convert.ToDateTime(g.Min(f => f.FechaSolicitud))
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

        protected override void Imprimir()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobado)))
                    {

                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);

                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        var VM = dbView.VistaPedidoCombustibles.Where(m => m.PedidoID.Equals(id)).ToList();
                        Reportes.FormReportViewerPedido rv = new Reportes.FormReportViewerPedido();
                        rv.Text = "Pedido :  " + VM.First().Numero;

                        if (db.EstacionServicios.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionID)))).FormatoCosta)
                        {
                            //FORMATO COSTA--------
                            Reportes.Inventario.Hojas.RptPedidoCombustible rep = new Reportes.Inventario.Hojas.RptPedidoCombustible();

                            string Nombre, Direccion, Telefono;
                            System.Drawing.Image picture_LogoEmpresa;
                            Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                            rep.DataSource = (from oc in VM
                                              group oc by new
                                              {
                                                  oc.PedidoID,
                                                  oc.Anulado,
                                                  oc.FechaCreado,
                                                  oc.FechaSolicitud,
                                                  oc.Entrega,
                                                  oc.FechaEntrega,
                                                  oc.FechaSalida,
                                                  oc.TurnoSolicitud,
                                                  oc.Proveedor,
                                                  oc.HoraEntrega,
                                                  oc.Comentario,
                                                  oc.Numero,
                                                  oc.Aprobado,
                                                  oc.Estacion,
                                                  oc.EstacionServicioID
                                              } into g
                                              select new
                                              {
                                                  ID = g.Key.PedidoID,
                                                  g.Key.Anulado,
                                                  g.Key.FechaCreado,
                                                  g.Key.Numero,
                                                  g.Key.Aprobado,
                                                  g.Key.Entrega,
                                                  g.Key.Estacion,
                                                  g.Key.EstacionServicioID,
                                                  g.Key.Comentario,
                                                  g.Key.FechaSolicitud,
                                                  g.Key.FechaEntrega,
                                                  g.Key.FechaSalida,
                                                  TurnoSolicitud = g.Key.FechaSolicitud.Value.ToString("dd/MM/yyyy") + "  " + g.Key.TurnoSolicitud,
                                                  g.Key.Proveedor,
                                                  g.Key.HoraEntrega,
                                                  PS = g.Where(o => o.ProductoID.Equals(1)).Sum(s => s.Galones),
                                                  PR = g.Where(o => o.ProductoID.Equals(2)).Sum(s => s.Galones),
                                                  DS = g.Where(o => o.ProductoID.Equals(3)).Sum(s => s.Galones)
                                              });

                            var objP = from o in db.OrdenCombustiblePedidos
                                       join p in db.Productos on o.ProductoID equals p.ID
                                       join t in db.Tanques on p.ID equals t.ProductoID
                                       group o by new { o.ProductoID, p.Nombre, t.Color, o.Orden } into g
                                       select new
                                       {
                                           g.Key.ProductoID,
                                           g.Key.Color,
                                           g.Key.Nombre,
                                           g.Key.Orden
                                       };

                            float w = 746.833252f;

                            foreach (var P in objP.OrderBy(o => o.Orden))
                            {
                                //Filas Tipo de Productos
                                rep.xrTableHead.WidthF = w + 100f;
                                rep.xrTableDetail.WidthF = w + 100f;
                                rep.xrTableRowHead.Cells.Add(new Parametros.MyXRTableCell(P.Nombre, 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.FromArgb(P.Color), Color.White));

                                DevExpress.XtraReports.UI.XRTableCell celda = new Parametros.MyXRTableCell("0", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black);
                                celda.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                            new DevExpress.XtraReports.UI.XRBinding("Text", null, (P.ProductoID.Equals(1) ? "PS" : (P.ProductoID.Equals(2) ? "PR" : "DS")),"{0:N2}")});
                                rep.xrTableRowDetail.Cells.Add(celda);
                                w += 100f;
                            }

                            if (objP.Count() > 1)
                            {
                                //Filas Tipo de Productos
                                rep.xrTableHead.WidthF = w + 100f;
                                rep.xrTableDetail.WidthF = w + 100f;
                                rep.xrTableRowHead.Cells.Add(new Parametros.MyXRTableCell("TOTAL GALONES", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));

                                DevExpress.XtraReports.UI.XRTableCell celdaT = new Parametros.MyXRTableCell("0", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black);
                                celdaT.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfTotal","{0:N2}")});
                                rep.xrTableRowDetail.Cells.Add(celdaT);

                                w += 100f;
                            }
                            rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                            rv.Owner = this.Owner;
                            rv.MdiParent = this.MdiParent;
                            rep.RequestParameters = false;
                            rep.CreateDocument();

                            rv.Show();
                        }
                        else
                        {
                            //FORMATO NORMAL--------
                            Reportes.Inventario.Hojas.RptPedidoCombustibleFormat rep = new Reportes.Inventario.Hojas.RptPedidoCombustibleFormat();

                            string Nombre, Direccion, Telefono;
                            System.Drawing.Image picture_LogoEmpresa;
                            Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                            rep.DataSource = (from oc in VM
                                              group oc by new
                                              {
                                                  oc.PedidoID,
                                                  oc.Anulado,
                                                  oc.FechaCreado,
                                                  oc.FechaSolicitud,
                                                  oc.FechaEntrega,
                                                  oc.FechaSalida,
                                                  oc.TurnoSolicitud,
                                                  oc.Terminal,
                                                  oc.Proveedor,
                                                  oc.HoraEntrega,
                                                  oc.Comentario,
                                                  oc.Numero,
                                                  oc.Aprobado,
                                                  oc.Estacion,
                                                  oc.EstacionServicioID
                                              } into g
                                              select new
                                              {
                                                  ID = g.Key.PedidoID,
                                                  g.Key.Anulado,
                                                  g.Key.FechaCreado,
                                                  g.Key.Numero,
                                                  Comentario = "Hora de entrega " + g.Key.HoraEntrega.Value.ToString("hh:mm tt") + " del día " +
                                                  g.Key.FechaEntrega.Value.ToString("dd/MM/yyyy") + "  " + g.Key.Comentario,
                                                  g.Key.Aprobado,
                                                  g.Key.Estacion,
                                                  g.Key.EstacionServicioID,
                                                  g.Key.Terminal,
                                                  g.Key.FechaSolicitud,
                                                  g.Key.FechaEntrega,
                                                  g.Key.FechaSalida,
                                                  g.Key.TurnoSolicitud,
                                                  g.Key.Proveedor,
                                                  PS = g.Where(o => o.ProductoID.Equals(1)).Sum(s => s.Galones),
                                                  PR = g.Where(o => o.ProductoID.Equals(2)).Sum(s => s.Galones),
                                                  DS = g.Where(o => o.ProductoID.Equals(3)).Sum(s => s.Galones)
                                              });

                            var objP = from o in db.OrdenCombustiblePedidos
                                       join p in db.Productos on o.ProductoID equals p.ID
                                       join t in db.Tanques on p.ID equals t.ProductoID
                                       group o by new { o.ProductoID, p.Nombre, t.Color, o.Orden } into g
                                       select new
                                       {
                                           g.Key.ProductoID,
                                           g.Key.Color,
                                           g.Key.Nombre,
                                           g.Key.Orden
                                       };

                            float w = 746.833252f;

                            foreach (var P in objP.OrderBy(o => o.Orden))
                            {
                                //Filas Tipo de Productos
                                rep.xrTableHead.WidthF = w + 100f;
                                rep.xrTableDetail.WidthF = w + 100f;
                                rep.xrTableRowHead.Cells.Add(new Parametros.MyXRTableCell(P.Nombre, 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.FromArgb(P.Color), Color.White));

                                DevExpress.XtraReports.UI.XRTableCell celda = new Parametros.MyXRTableCell("0", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black);
                                celda.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                            new DevExpress.XtraReports.UI.XRBinding("Text", null, (P.ProductoID.Equals(1) ? "PS" : (P.ProductoID.Equals(2) ? "PR" : "DS")),"{0:N2}")});
                                rep.xrTableRowDetail.Cells.Add(celda);
                                w += 100f;
                            }

                            if (objP.Count() > 1)
                            {
                                //Filas Tipo de Productos
                                rep.xrTableHead.WidthF = w + 100f;
                                rep.xrTableDetail.WidthF = w + 100f;
                                rep.xrTableRowHead.Cells.Add(new Parametros.MyXRTableCell("TOTAL", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));

                                DevExpress.XtraReports.UI.XRTableCell celdaT = new Parametros.MyXRTableCell("0", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black);
                                celdaT.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
                            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfTotal","{0:N2}")});
                                rep.xrTableRowDetail.Cells.Add(celdaT);

                                w += 100f;
                            }
                            rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                            rv.Owner = this.Owner;
                            rv.MdiParent = this.MdiParent;
                            rep.RequestParameters = false;
                            rep.CreateDocument();

                            rv.Show();
                        }

                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El pedido no esta aprobado.", Parametros.MsgType.warning);
                    }
                }


            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, bool Refresh)
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

            if (NextRegistro)
                Add();

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
                    nf = new Forms.Dialogs.DialogPedidoCombustible(Usuario, false);
                    nf.Text = "Crear Pedido";
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobado)))
                            Parametros.General.DialogMsg("El pedido esta aprobado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                                Parametros.General.DialogMsg("El pedido esta anulado.", Parametros.MsgType.warning);
                            else
                            {
                                nf = new Forms.Dialogs.DialogPedidoCombustible(Usuario, true);
                                nf.Text = "Editar Pedido";
                                nf.EntidadAnterior = db.Pedidos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                nf.Owner = this.Owner;
                                nf.MdiParent = this.MdiParent;
                                nf.MDI = this;
                                nf.Show();
                            }
                        }
                    }
                }
                else
                    nf.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + " Este pedido no se puede anular, ya esta anulado.", Parametros.MsgType.warning);
                else
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobado)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + " Este pedido no se puede anular, ya esta aprobado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                            Entidad.Pedido P = db.Pedidos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                            P.Anulado = true;
                            db.SubmitChanges();

                            FillControl(true);
                        }
                    }
                }
            }
        }

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {

                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);

                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    //var P = dv.VistaPedidoCombustibles.First();// o => o.PedidoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = "Pedido " + Convert.ToInt32(gvData.GetFocusedRowCellValue(colNumero)).ToString("N0");
                    page.Tag = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                    Parametros.MyXtraGridPedido xtra = new Parametros.MyXtraGridPedido(db, db.Pedidos.Single(p => p.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    //xtra.gridDetalle.DataSource = OC.VistaOrdenesComprasDetalles;

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {
                        xtra.Dock = DockStyle.Fill;
                        page.Controls.Add(xtra);
                        this.xtraTabControlMain.TabPages.Add(page);
                        this.xtraTabControlMain.SelectedTabPage = page;
                        paginas.Add(Convert.ToInt32(page.Tag));
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

        //Cerrando las pestañas detalles de ordenes
        private void xtraTabControlMain_CloseButtonClick(object sender, EventArgs e)
        {
            this.paginas.Remove(Convert.ToInt32(xtraTabControlMain.SelectedTabPage.Tag));
            this.xtraTabControlMain.TabPages.Remove(xtraTabControlMain.SelectedTabPage);

            if (this.xtraTabControlMain.TabPages.Count > 0)
            {
                this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages[(this.xtraTabControlMain.TabPages.Count - 1)];
            }
        }

        private void gvData_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void rpChkAprobado_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkAprobarNomina"))
                //{
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "No se puede aprobar, el pedido esta está anulado.", Parametros.MsgType.warning);
                    else
                    {
                        if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFecha)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionID))))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobado)))
                            {
                                //if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFecha)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionID))))
                                //    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                //else
                                //{
                                    //Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "La Nomina esta ya está aprobada.", Parametros.MsgType.warning);
                                    if (Parametros.General.DialogMsg("¿Está seguro de desaprobar el pedido?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                                        Entidad.Pedido P = db.Pedidos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                                        P.Aprobado = false;
                                        db.SubmitChanges();

                                        FillControl(true);
                                        gvData.FocusedColumn = colNumero;
                                    }
                                //}
                            }
                            else
                            {
                                    if (Parametros.General.DialogMsg("¿Está seguro de aprobar el pedido?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                                        Entidad.Pedido P = db.Pedidos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                                        P.Aprobado = true;
                                        db.SubmitChanges();

                                        FillControl(true);
                                        gvData.FocusedColumn = colNumero;
                                    }
                                
                            }
                        }
                    }
                }

                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion
    }
}
