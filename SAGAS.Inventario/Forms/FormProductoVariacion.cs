using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;
//using DevExpress.DataAccess;

namespace SAGAS.Inventario.Forms
{
    public partial class FormProductoVariacion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogProductoVariacion nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private int _Cuenta = 0;
        
        #endregion

        #region <<< INICIO >>>

        public FormProductoVariacion()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Eliminar";
            this.barImprimir.Caption = "Vista previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaSeriesRetencione);
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
                var query = from v in dv.ViewProductoVariacions
                            select new
                            {
                                v.EstacionServicio,
                                v.ID,
                                v.SubEstacion,
                                v.Producto,
                                v.PermisibleActaVariacion,
                                v.PermisibleActaDiferencia
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

       

        //protected override void Imprimir()
        //{
            
        //    try
        //    {
        //        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
        //        var obj = db.CuentaBancarias.Where(cb => cb.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).First();//((Entidad.Reporte)((Parametros.MyBarButtonItem)(e.Item)).Tag);

        //        if (!String.IsNullOrEmpty(obj.Extension))
        //        {

        //            if (obj.Extension.Equals(".repx"))
        //            {
        //                //Reporte en DevExpress
        //                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();

        //                System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

        //                DevExpress.XtraReports.UI.XtraReport rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);


        //                            rv.Text = obj.Nombre;


        //                            var ds = rep.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
        //                            //Asignar la nueva cadena de conexion
        //                            ds.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString() + @"; Network Library=DBMSSOCN; connection timeout=900";
        //                            //ds.Connection.Open();

        //                            //var Vista = dv.VistaMovimientos.FirstOrDefault(o => o.CuentaBancariaID.Equals(obj.ID) && !o.Anulado);

        //                            //if (Vista != null)
        //                            //{

        //                                rep.DataSource = ds;
        //                                rep.Parameters["ParametroID"].Value = 39783;
        //                                //rep.Param.Visible = false;
        //                            //}
        //                            //else
        //                            //{
        //                            //    rep.DataSource = db.Movimientos.Where(m => m.ID.Equals(0));
        //                            //    rep.CreateDocument(true);
        //                            //}


        //                            rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
        //                            rv.Owner = this;
        //                            rv.MdiParent = this.MdiParent;
        //                            rep.CreateDocument(true);
        //                            rv.Show();

        //            }
        //            /*else if (obj.Extension.Equals(".rpt"))
        //            {

        //                //Reportes en Crystal

        //                if (!System.IO.Directory.Exists(routeRpt))
        //                    System.IO.Directory.CreateDirectory(routeRpt);
        //                routeRpt += @"\" + @obj.ArchivoNombre + obj.Extension;
        //                File.WriteAllBytes(routeRpt, obj.Archivo);

        //                if (System.IO.File.Exists(@routeRpt))
        //                {
        //                    Reportes.FormCrystalViewer CR = new Reportes.FormCrystalViewer();
        //                    CR.Text = obj.Nombre;
        //                    CR.Tag = @routeRpt;
        //                    CR.crystalReportViewer1.ReportSource = @routeRpt;
        //                    CR.Refresh();
        //                    CR.Owner = this;
        //                    CR.MdiParent = this;
        //                    CR.Show();
        //                }
        //            }
        //            else
        //            {
        //                Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);
        //            }*/
        //        }
        //        else
        //            Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);
        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //    }
        //}

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool EsRemesa)
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
                    nf = new Forms.Dialogs.DialogProductoVariacion(Usuario);
                    nf.Text = "Nuevo Producto Variación";
                    nf.Owner = this;
                    nf.MDI = this;
                    nf.Show();
                }
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
                        nf = new Forms.Dialogs.DialogProductoVariacion(Usuario);
                        nf.Text = "Editar Producto Variación";
                        nf.EntidadAnterior = db.ProductoVariacions.Single(e => e.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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

                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {
                            var V = db.ProductoVariacions.Single(c => c.ID == id);

                            //Eliminar Registro 
                            string texto = Convert.ToString(gvData.GetFocusedRowCellValue(gridColumn1));
                            db.ProductoVariacions.DeleteOnSubmit(V);
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria, "Se Eliminó Producto Variación: " + texto, this.Name);

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
    }
}
