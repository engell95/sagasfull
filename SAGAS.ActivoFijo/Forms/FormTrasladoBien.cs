using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAGAS.Parametros;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;

namespace SAGAS.ActivoFijo.Forms
{
    public partial class FormTrasladoBien : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogBien nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private Parametros.ListEstadosOrdenes listado = new Parametros.ListEstadosOrdenes();
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _IDClaseCombustible = Parametros.Config.ProductoClaseCombustible();

        #endregion


        public FormTrasladoBien()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = this.btnModificar.Visibility = this.btnAnular.Visibility = BarItemVisibility.Never;
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaTrasladoBien);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }


        #region <<< METODOS >>>

        private void FormBien_Load(object sender, EventArgs e)
        {
            FillControl(false); 
        }

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
                    lkMes.DataSource = listadoMes.GetListMeses();
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
                var query = from v in dv.VistaTrasladoBiens
                            where (lista.Contains(v.EstacionAnteriorID) || lista.Contains(v.EstacionNuevaID))
                            select new
                            {
                                v.ID
      ,
                                v.MovimientoID
      ,
                                v.BienID
      ,
                                v.BienCodigo
      ,
                                v.BienNombre
      ,
                                v.UsuarioAsignadoAnterior
      ,
                                v.AreaAnteriorID
      ,
                                v.AreaAnteriorNombre
      ,
                                v.EstacionAnteriorID
      ,
                                v.EstacionAnteriorNombre
      ,
                                v.SubEstacionAnteriorID
      ,
                                v.SubEstacionAnteriorNombre
      ,
                                v.UsuarioAsignadoNuevo
      ,
                                v.AreaNuevaID
      ,
                                v.AreaNuevaNombre
      ,
                                v.EstacionNuevaID
      ,
                                v.EstacionNuevaNombre
      ,
                                v.SubEstacionNueva
      ,
                                v.SubEstacionNuevaID
                                ,
                                v.FechaRegistro
                                ,
                                Mes = (v.FechaRegistro.HasValue ? v.FechaRegistro.Value.Month : 0)
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
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID)).Equals(0))
                    Parametros.General.DialogMsg("El traslado no generó Comprobante Contable.", Parametros.MsgType.warning);
                else
                    ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID)));
            }
        }

        private void ImprimirComprobante(int ID, int MovID)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(MovID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString();

                Reportes.ActivoFijo.Hojas.RptComprobanteTraslado rep = new Reportes.ActivoFijo.Hojas.RptComprobanteTraslado();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString();

                rep.xrCeFecha.Text = VM.First().FechaContabilizacion.ToShortDateString();

                var T = dbView.VistaTrasladoBiens.SingleOrDefault(s => s.ID.Equals(ID));

                rep.xrCeCodigo.Text = T.BienCodigo.ToString();
                rep.xrCeNombre.Text = T.BienNombre;
                rep.xrCeUsuarioAnterior.Text = T.UsuarioAsignadoAnterior;
                rep.xrCeUsuarioNuevo.Text = T.UsuarioAsignadoNuevo;
                rep.xrCeAreaAnterior.Text = T.AreaAnteriorNombre;
                rep.xrCeAreaNuevo.Text = T.AreaNuevaNombre;
                rep.xrCeEstacionAnterior.Text = (String.IsNullOrEmpty(T.SubEstacionAnteriorNombre) ? T.EstacionAnteriorNombre : T.SubEstacionAnteriorNombre);
                rep.xrCeEstacionNuevo.Text = (String.IsNullOrEmpty(T.SubEstacionNueva) ? T.EstacionNuevaNombre : T.SubEstacionNueva);
     
                rep.DataSource = VM;
                
                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
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
            this.gvData.ActiveFilter.Clear();
        }

        #region <<< TABLA COMPROBANTE >>>
        
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    int _id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID));
                    if (_id > 0)
                    {
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(_id));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridCD xtra = new Parametros.MyXtraGridCD();

                        xtra.gridComprobante.DataSource = M.VistaComprobantes;

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
                    else
                        Parametros.General.DialogMsg("El traslado no generó Comprobante Contable.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void xtraTabControlMain_CloseButtonClick(object sender, EventArgs e)
        {
            this.paginas.Remove(Convert.ToInt32(xtraTabControlMain.SelectedTabPage.Tag));
            this.xtraTabControlMain.TabPages.Remove(xtraTabControlMain.SelectedTabPage);

            if (this.xtraTabControlMain.TabPages.Count > 0)
            {
                this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages[(this.xtraTabControlMain.TabPages.Count - 1)];
            }
        }

        #endregion

        #endregion

    }
}
