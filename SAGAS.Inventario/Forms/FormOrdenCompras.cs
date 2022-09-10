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
    public partial class FormOrdenCompras : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogOrdenCompras nf;
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

        public FormOrdenCompras()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.btnAnular.Caption = "Anular";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
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
                    
                if (Refresh)
                    linqInstantFeedbackSource1.Refresh();
                else
                {
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));

                    lkEstado.DataSource = listado.GetListEstadosOrdenes();
                    lkMes.DataSource = listadoMes.GetListMeses();

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("Fecha") > fecha.AddDays(-1)).ToString();
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
                var query = from oc in dv.VistaOrdenesCompras
                            where lista.Contains(oc.EstacionServicioID)
                            select new
                            {
                                oc.ID,
                                Proveedor = oc.ProveedorNombre,
                                oc.Numero,
                                oc.Fecha,
                                Year = oc.Fecha.Year,
                                Mes =  oc.Fecha.Month,
                                Monto = (oc.MonedaID.Equals(MonedaID) ? oc.Monto : oc.MontoMonedaSecundaria),
                                Moneda = oc.MonedaSimbolo + " | " + oc.MonedaNombre,
                                EstacionServicio = oc.EstacionNombre,
                                SubEstacion = oc.SubEstacionNombre,
                                oc.Comentario,
                                oc.Estado
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
            this.PrintList(grid);
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
                    nf = new Forms.Dialogs.DialogOrdenCompras(Usuario, false);
                    nf.Text = "Crear Nueva Orden de Compra";
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

                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)) > 1)
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + " Esta orden no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            nf = new Forms.Dialogs.DialogOrdenCompras(Usuario, true);
                            nf.Text = "Editar Orden de Compra";
                            nf.EntidadAnterior = db.OrdenCompras.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nf.Owner = this.Owner;
                            nf.MdiParent = this.MdiParent;
                            nf.MDI = this;
                            nf.Show();
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
                if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)) >= 3 && !Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)).Equals(5))
                  Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + " Esta orden no se puede anular.", Parametros.MsgType.warning);
                else
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        using (Inventario.Forms.Dialogs.DialogComentario dg = new Inventario.Forms.Dialogs.DialogComentario())
                        {
                            Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                            dg.Text = "Agregar un comentario a la orden de compra. " + OC.Numero;
                            dg.Comentario = OC.Comentario;

                            if (dg.ShowDialog().Equals(DialogResult.OK))
                            {
                                OC.Comentario = dg.Comentario;
                                OC.Anulado = true;
                                OC.Enviado = false;
                                OC.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                                OC.UsuarioAnuladoID = Parametros.General.UserID;
                                OC.Estado = 3;
                                db.SubmitChanges();
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario, "Se Anuló la Orden de Compra : " + OC.Numero, this.Name);

                                FillControl(true);
                            }
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
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var OC = dv.VistaOrdenesCompras.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = "Orden " + OC.Numero;
                    page.Tag = OC.ID;

                    Parametros.MyXtraGridOCD xtra = new Parametros.MyXtraGridOCD();
                    xtra.gridDetalle.DataSource = OC.VistaOrdenesComprasDetalles;

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
                }
            }
            catch (Exception ex)
            {
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
                    if (gvData.GetFocusedRowCellValue(colEstado).GetType() != typeof(DevExpress.Data.NotLoadedObject))
                    {
                        string texto = Convert.ToString(gvData.GetFocusedRowCellValue(colNumero));
                        int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                        barItems.ClearLinks();
                        
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)) < 2)
                        {
                            //Boton Enviar Orden
                            DevExpress.XtraBars.BarButtonItem btnEnviar = new DevExpress.XtraBars.BarButtonItem();
                            btnEnviar.Caption = "Enviar Orden " + texto;
                            btnEnviar.Glyph = Properties.Resources.EnviarOrden;
                            btnEnviar.Id = 1;
                            btnEnviar.Name = "btnEnviar";
                            btnEnviar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnEnviar.Tag = ID;
                            btnEnviar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEnviar_ItemClick);
                            barItems.AddItem(btnEnviar);
                        }

                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)).Equals(2) & btnsRestricciones)
                        {

                            //Boton Rechazar Orden
                            DevExpress.XtraBars.BarButtonItem btnRechazar = new DevExpress.XtraBars.BarButtonItem();
                            btnRechazar.Caption = "Rechazar Orden " + texto;
                            btnRechazar.Glyph = Properties.Resources.RechazarOrden;
                            btnRechazar.Id = 1;
                            btnRechazar.Name = "btnRechazar";
                            btnRechazar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnRechazar.Tag = ID;
                            btnRechazar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRechazar_ItemClick);
                            barItems.AddItem(btnRechazar);

                            //Boton Aprobar Orden
                            DevExpress.XtraBars.BarButtonItem btnAprobar = new DevExpress.XtraBars.BarButtonItem();
                            btnAprobar.Caption = "Aprobar Orden " + texto;
                            btnAprobar.Glyph = Properties.Resources.AprobarOrden;
                            btnAprobar.Id = 2;
                            btnAprobar.Name = "btnAprobar";
                            btnAprobar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnAprobar.Tag = ID;
                            btnAprobar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAprobar_ItemClick);
                            barItems.AddItem(btnAprobar);
                        }

                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstado)).Equals(4))
                        {
                            //Boton Enviar Orden
                            DevExpress.XtraBars.BarButtonItem btnPreview = new DevExpress.XtraBars.BarButtonItem();
                            btnPreview.Caption = "Imprimir Orden " + texto;
                            btnPreview.Glyph = Properties.Resources.print24;
                            btnPreview.Id = 1;
                            btnPreview.Name = "btnPreview";
                            btnPreview.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnPreview.Tag = ID;
                            btnPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPreview_ItemClick);
                            barItems.AddItem(btnPreview);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void btnPreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barItems.ItemLinks.Count > 0)
                {

                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dbView.VistaOrdenesCompras.Where(m => m.ID.Equals(Convert.ToInt32(barItems.ItemLinks[0].Item.Tag))).ToList();
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    rv.Text = "Orden de compra " + VM.First().Numero;

                    Reportes.Inventario.Hojas.RptOrdenCompra rep = new Reportes.Inventario.Hojas.RptOrdenCompra();

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                    rep.CeTitulo.Text = "Orden de Compra";

                    rep.DataSource = VM;

                    rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                    rv.Owner = this.Owner;
                    rv.MdiParent = this.MdiParent;
                    rep.RequestParameters = false;
                    rep.CreateDocument();

                    rv.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        private void btnEnviar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barItems.ItemLinks.Count > 0)
                {
                    if (Parametros.General.DialogMsg("¿Desea enviar la orden de compra?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Convert.ToInt32(barItems.ItemLinks[0].Item.Tag)));
                        OC.Enviado = true;
                        OC.Estado = 2;
                        db.SubmitChanges();
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario, "Se Envió la Orden de Compra : " + OC.Numero, this.Name);
                        FillControl(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);         
            }
        }

        private void btnRechazar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barItems.ItemLinks.Count > 0)
                {
                    if (Parametros.General.DialogMsg("¿Desea rechazar la orden de compra?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        using (Inventario.Forms.Dialogs.DialogComentario dg = new Inventario.Forms.Dialogs.DialogComentario())
                        {
                            Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Convert.ToInt32(barItems.ItemLinks[0].Item.Tag)));
                            dg.Text = "Agregar un comentario a la orden de compra. " + OC.Numero;
                            dg.Comentario = OC.Comentario;

                            if (dg.ShowDialog().Equals(DialogResult.OK))
                            {
                                OC.Comentario = dg.Comentario;
                                OC.Enviado = false;
                                OC.Estado = 1;
                                db.SubmitChanges();
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario, "Se Rechazó la Orden de Compra : " + OC.Numero, this.Name);
                                FillControl(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnAprobar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barItems.ItemLinks.Count > 0)
                {
                    if (Parametros.General.DialogMsg("¿Desea aprobar la orden de compra?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Convert.ToInt32(barItems.ItemLinks[0].Item.Tag)));
                        OC.Enviado = false;
                        OC.Aprobado = true;
                        OC.FechaAprobado = Convert.ToDateTime(db.GetDateServer());
                        OC.Estado = 4;
                        db.SubmitChanges();
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario, "Se Aprobó la Orden de Compra : " + OC.Numero, this.Name);
                        FillControl(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion  
       
    }
}
