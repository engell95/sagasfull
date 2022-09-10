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

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormClienteUpload : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogClienteUpload nf;
        private Forms.Dialogs.DialogClienteSalesUpload nfSales;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormClienteUpload()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            //this.btnModificar.Caption = "Editar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            this.btnAnular.Glyph = Properties.Resources.AddSales;
            this.btnAnular.Caption = "Cargar Clientes con Ventas";
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaPlantilasInicales);
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

                    //DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    //this.gvData.ActiveFilterString = (new OperandProperty("FechaInventario") > fecha.AddDays(-1)).ToString();
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
                var query = from iv in dv.VistaPlantilasInicales
                            where lista.Contains(iv.EstacionServicioID) && iv.EsCliente
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaCarga,
                                iv.Finalizado,
                                iv.Observacion,
                                iv.MovimientoNumero,
                                iv.MontoMovimiento,
                                iv.EsAnticipo,
                                iv.TieneVentas
                            };

                e.QueryableSource = query;
                e.Tag = db;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataClassesDataContext)e.Tag).Dispose();
        }

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("TieneVentas")).Equals(false))
                {
                    ImprimirPlantilla(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                }
                else if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("TieneVentas")).Equals(true))
                {
                    ImprimirPlantillaVentas(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                }
            }            
        }

        private void ImprimirPlantilla(int ID)
        {
            try
            {

                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dbView.VistaPlantilasInicales.Where(m => m.ID.Equals(ID)).ToList();
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    rv.Text = "Carga Saldos Clientes" + VM.First().Numero;

                    Reportes.Ventas.Hojas.RptClientesUpload rep = new Reportes.Ventas.Hojas.RptClientesUpload();

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                    rep.DataSource = VM;

                    var VC = dbView.VistaMovimientos.Where(v => v.ID.Equals(VM.First().MovimientoID)).ToList();
                    if (VC.Count > 0)
                    {
                        DevExpress.XtraReports.UI.XRSubreport xrSubCC = new DevExpress.XtraReports.UI.XRSubreport();
                        xrSubCC.ReportSource = new Reportes.Inventario.Hojas.SubRptComprobante(VC);
                        rep.GroupFooterCC.Controls.Add(xrSubCC);
                    }

                    rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                    rv.Owner = this.Owner;
                    rv.MdiParent = this.MdiParent;
                    rep.RequestParameters = false;
                    rep.CreateDocument();

                    rv.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void ImprimirPlantillaVentas(int ID)
        {
            try
            {

                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaPlantilasInicales.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Carga Saldos Clientes " + VM.First().Numero;

                Reportes.Ventas.Hojas.RptClientesSalesUpload rep = new Reportes.Ventas.Hojas.RptClientesSalesUpload();
                
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.DataSource = dbView.VistaCargaVentaClientes.Where(v => VM.First().VistaPlantillaCargaCliente.Select(s => s.ID).Contains(v.PlantillaCargaClienteID));

                var VC = dbView.VistaMovimientos.Where(v => v.ID.Equals(VM.First().MovimientoID)).ToList();
                if (VC.Count > 0)
                {
                    DevExpress.XtraReports.UI.XRSubreport xrSubCC = new DevExpress.XtraReports.UI.XRSubreport();
                    xrSubCC.ReportSource = new Reportes.Inventario.Hojas.SubRptComprobante(VC);
                    rep.GroupFooterCC.Controls.Add(xrSubCC);
                }

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rep.CreateDocument(true);
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                
                rv.Show();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
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

        internal void CleanDialog(bool ShowMSG, bool Refresh, bool Next, bool Sales)
        {
            if (Sales)
                nfSales = null;
            else
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

            if (Next)
                Add();
            else
                this.Activate();
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
                    nf = new Forms.Dialogs.DialogClienteUpload(Usuario, false);
                    nf.Text = "Crear Plantilla Cliente";
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

                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("TieneVentas")).Equals(false))
                    {
                        if (nf == null)
                        {

                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventrio físico ya fue ingresado, no se puede editar.", Parametros.MsgType.warning);
                            else
                            {
                                nf = new Forms.Dialogs.DialogClienteUpload(Usuario, true);
                                nf.Text = "Editar Plantilla Cliente";
                                nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                                    (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                                nf.EntidadAnterior = db.PlantillaCarga.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                nf.Owner = this.Owner;
                                nf.MdiParent = this.MdiParent;
                                nf.MDI = this;
                                nf.Show();
                            }
                        }
                        else
                            nf.Activate();
                    }
                    else if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("TieneVentas")).Equals(true))
                    {
                        if (nfSales == null)
                        {

                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventrio físico ya fue ingresado, no se puede editar.", Parametros.MsgType.warning);
                            else
                            {
                                nfSales = new Forms.Dialogs.DialogClienteSalesUpload(Usuario, true);
                                nfSales.Text = "Editar Plantilla Cliente Ventas";
                                nfSales.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                                    (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                                nfSales.EntidadAnterior = db.PlantillaCarga.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                nfSales.Owner = this.Owner;
                                nfSales.MdiParent = this.MdiParent;
                                nfSales.MDI = this;
                                nfSales.Show();
                            }
                        }
                        else
                            nfSales.Activate();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        protected override void Del()
        {
            try
            {
                if (nfSales == null)
                {
                    nfSales = new Forms.Dialogs.DialogClienteSalesUpload(Usuario, false);
                    nfSales.Text = "Plantilla Cliente Ventas";
                    nfSales.Owner = this.Owner;
                    nfSales.MdiParent = this.MdiParent;
                    nfSales.MDI = this;
                    nfSales.Show();
                }
                else
                    nfSales.Activate();
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
                    Edit();
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
