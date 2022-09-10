using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormComprobanteArqueo : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogComprobanteArqueo nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<int> lista;
        private List<int> paginas = new List<int>();


        #endregion

        #region <<< INICIO >>>

        public FormComprobanteArqueo()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaMovimiento);
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
                    lkMes.DataSource = listadoMes.GetListMeses();

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaContabilizacion") > fecha.AddDays(-1)).ToString();
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
                var query = from iv in dv.VistaMovimientos.Where(v => (v.MovimientoTipoID.Equals(46) || v.MovimientoTipoID.Equals(47) || v.MovimientoTipoID.Equals(48)))
                            where lista.Contains(iv.EstacionServicioID)// && !iv.Finalizado
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.ResumenDiaNumero,
                                iv.Referencia,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = iv.Monto
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


        private void ImprimirComprobante(int ID, bool ToPrint, bool Preview)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Comprobante Arqueo " + VM.First().Numero.ToString("000000000");

                Reportes.Contabilidad.Hojas.RptComprobanteArqueo rep = new Reportes.Contabilidad.Hojas.RptComprobanteArqueo();
                
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeTitulo.Text = VM.First().MovimientoTipoNombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToShortDateString();
                rep.CeResumen.Text = db.ResumenDias.Single(r => r.ID.Equals(VM.First().ResumenDiaID)).Numero.ToString();
                rep.DataSource = VM;

                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                if (ToPrint)
                {
                    rep.Print();
                    rv.Close();
                }
                else
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
            {
                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false, true);
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

        internal void CleanDialog(bool ShowMSG, bool Refresh, int ID)
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


            //if (!ID.Equals(0))
            //{
            //    ImprimirComprobante(ID, true);
            //}
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
                    int ID = 0;
                    using (Reportes.Arqueos.Dialogs.DialogGetDataArqueo dg = new Reportes.Arqueos.Dialogs.DialogGetDataArqueo())
                    {
                        dg.Comprobante = true;
                        dg._MDIParent = this;
                        dg.ShowDialog();
                        ID = dg.IDRD;
                    }

                    if (!ID.Equals(0))
                    {

                        //BORRAR
                        //ID = 5507;

                        Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        nf = new Ventas.Forms.Dialogs.DialogComprobanteArqueo();
                        nf.Owner = this.Owner;
                        nf.MdiParent = this.MdiParent;
                        nf.MDI = this;
                        nf.RD = db.ResumenDias.Single(r => r.ID.Equals(ID));
                        nf.Show();

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

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = M.Abreviatura + " " + M.Numero.ToString();
                    page.Tag = M.ID;

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {

                        Parametros.MyXtraGridComprobante xtra = new Parametros.MyXtraGridComprobante();

                        xtra.gridDetalle.DataSource = M.VistaKardexes;
                        xtra.gridComprobante.DataSource = M.VistaComprobantes;
                        xtra.gridNotas.DataSource = dv.VistaMovimientos.Where(o => o.MovimientoReferenciaID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList().Select(s => new {Deudor = (String.IsNullOrEmpty(s.ClienteNombre) ? s.EmpleadoNombres + " " + s.EmpleadoApellidos : s.ClienteNombre), s.Referencia, TipoMovimiento = s.MovimientoTipoNombre, s.Monto });
                        
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

    }
}
