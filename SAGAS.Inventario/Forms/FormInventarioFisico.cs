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
    public partial class FormInventarioFisico : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogInventarioFisico nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormInventarioFisico()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaInventarioFisico);
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
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaInventario") > fecha.AddDays(-1)).ToString();
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
                var query = from iv in dv.VistaInventarioFisicos
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaInventario,
                                Year = iv.FechaInventario.Year,
                                Mes = iv.FechaInventario.Month,
                                iv.Finalizado,
                                iv.Observacion,
                                iv.AreaNombre,
                                iv.MovimientoNumero
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
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {

                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dbView.VistaInventarioFisicos.Where(m => m.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    rv.Text = "Inventario Físico " + VM.First().Numero;

                    Reportes.Inventario.Hojas.RptInventarioFisico rep = new Reportes.Inventario.Hojas.RptInventarioFisico();

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                    
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

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool Refresh, bool Next)
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
                    nf = new Forms.Dialogs.DialogInventarioFisico(Usuario, false);
                    nf.Text = "Crear Nuevo Levantamiento de Inventario Físico";
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

                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventrio físico ya fue ingresado a un ajuste, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            nf = new Forms.Dialogs.DialogInventarioFisico(Usuario, true);
                            nf.Text = "Editar Levantamiento de Inventario Físico";
                            nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                                (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                            nf.EntidadAnterior = db.InventarioFisicos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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

        #region <<< FINALIZACIÓN DE INVENTARIO
        
        private void chkFinalizado_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkFinalizadoInventarioFisico"))
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        bool Finished = !Convert.ToBoolean(gvData.GetRowCellValue(gvData.FocusedRowHandle, colFinalizado));

                        if (Parametros.General.DialogMsg("¿Desea " + (Finished ? "Finalizar" : "Aperturar") + " el Inventario Físico " + Convert.ToString(gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2)) + "?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            Entidad.InventarioFisico I = db.InventarioFisicos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(gvData.FocusedRowHandle, "ID"))));

                            var Mov = db.Movimientos.FirstOrDefault(f => f.ID.Equals(I.MovimientoID));

                            if (Mov != null)
                            {
                                if (Mov.Finalizado)
                                {
                                    Parametros.General.DialogMsg("El Ajuste para el Inventario Físico " + Convert.ToString(gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2)) + " ya esta finalizado y no se puede reaperturar.", Parametros.MsgType.warning);
                                    return;
                                }
                                else
                                {
                                    I.Finalizado = Finished;

                                    db.SubmitChanges();

                                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                        "Se " + (Finished ? "Finalizó" : "Aperturó") + " el Invetario Físico: " + gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2).ToString(), this.Name);
                                    this.timerMSG.Start();
                                    FillControl(true);
                                    gvData.FocusedColumn = gridColumn6;
                                }
                            }
                            else
                            {
                                I.Finalizado = Finished;

                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se " + (Finished ? "Finalizó" : "Aperturó") + " el Invetario Físico: " + gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2).ToString(), this.Name);
                                this.timerMSG.Start();
                                FillControl(true);
                                gvData.FocusedColumn = gridColumn6;
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
