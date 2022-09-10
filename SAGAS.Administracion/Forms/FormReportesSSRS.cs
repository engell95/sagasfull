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
using SAGAS.Reportes;

namespace SAGAS.Administracion.Forms
{                                
public partial class FormReportesSSRS : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogReportesSSRS nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        //private List<Entidad.AccesoSistema> DatosReporte;
        private Form sagas;

        #endregion

        #region <<< INICIO >>>

        public FormReportesSSRS()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Eliminar";
            this.btnAgregar.Caption = "Subir";
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.Reporte);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        public FormReportesSSRS(Form sagaspr)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.sagas = sagaspr;
            this.btnAnular.Caption = "Eliminar";
            this.btnAgregar.Caption = "Subir";
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.ReportesSSR);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl(true);
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
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
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

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl(true);
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
                    nf = new Forms.Dialogs.DialogReportesSSRS();
                    nf.Text = "Subir Reporte";
                    nf.Owner = this;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
                        nf = new Forms.Dialogs.DialogReportesSSRS();
                        nf.Text = "Editar Reporte";
                        nf.EntidadAnterior = db.ReportesSSRs.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        nf.Owner = this;
                        nf.Editable = true;
                        nf.MDI = this;
                        nf.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            var R = db.ReportesSSRs.Single(c => c.ID == id);
                            var AR = db.AccesosReportes.Where(w => w.ReportesSSRSID == id);
                            string nombre = R.Nombre;
                            db.ReportesSSRs.DeleteOnSubmit(R);
                            db.AccesosReportes.DeleteAllOnSubmit(AR);
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Eliminó el Reporte: " + nombre + ", y sus accesos.", this.Name);

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();

                            FillControl(true);
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

        #region <<< EVENTOS >>>
        private void linqInstantFeedbackSource1_GetQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var query = from AR in db.GetViewReportesSSRSByUsers.Where(w => w.UsuarioID == Parametros.General.UserID)
                            join R in db.ReportesSSRs on AR.ReportesSSRSID equals R.ID
                            join RE in db.ReportesEstructuras on R.EstructuraID equals RE.ID
                            join M in db.Modulos on R.ModuloID equals M.ID
                            select new
                            {
                                AR.EstacionServicioID,
                                AR.UsuarioID,
                                AR.ReportesSSRSID,
                                R.Comentario,
                                R.EsSubReporte,
                                EstructuraNombre = RE.Nombre,
                                R.Fecha,
                                R.ID,
                                R.ModuloID,
                                R.Nombre,
                                R.Orden,
                                ModuloNombre = M.Nombre
                            };

                e.QueryableSource = query;
                e.Tag = db;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataClassesDataContext)e.Tag).Dispose();
        }

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                if (gvData.GetFocusedRowCellValue(colNombre) != null)
                {
                    string reportname = gvData.GetFocusedRowCellValue(colNombre).ToString();
                    //string textfile = gvData.GetFocusedRowCellValue(colNombreArchivo).ToString();
                    if (!Parametros.Config.FormCargado(reportname, sagas))
                    {
                        FormReportSSRS frs = new FormReportSSRS(reportname/*, textfile*/);
                        frs.Owner = this.Owner;
                        frs.MdiParent = this.MdiParent;
                        frs.Name = reportname;
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        frs.Show();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        #endregion
    }
}
