using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Data.Filtering;

namespace SAGAS.Tesoreria.Forms
{                                
    public partial class FormRetenciones : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        //private Forms.Dialogs.DialogProductoClase nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int IDClaseCombustible = Parametros.Config.ProductoClaseCombustible();
        internal List<Entidad.FormatosCheque> EtFormatos = new List<Entidad.FormatosCheque>(); 
        DevExpress.XtraReports.UI.XtraReport rep;

        #endregion

        #region <<< INICIO >>>

        public FormRetenciones()
        {
            InitializeComponent();
            this.btnAgregar.Enabled = this.btnAnular.Enabled = this.btnModificar.Enabled = false;
            this.btnAgregar.Visibility = this.btnModificar.Visibility = btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                SAGAS.Entidad.SAGASDataViewsDataContext dv =new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                bdsManejadorDatos.DataSource = (from r in dv.VistaImpresionRetenciones
                                                where r.Monto > 0
                                               select r).OrderByDescending(o=>o.FechaRegistro);

                rlkCuentaContable.DataSource = (from cc in db.CuentaContables
                                                select new
                                                {
                                                    Display = cc.Nombre,
                                                    ID = cc.ID
                                                }).ToList();
          
                
                this.grid.DataSource = bdsManejadorDatos;
                EtFormatos = db.FormatosCheques.ToList();

                if (gvData.RowCount > 0)
                {
                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    gvData.ActiveFilterString = (new OperandProperty("FechaRegistro") >= fecha).ToString();
                 
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();


                    //  Mov = (from o in db.Movimientos
                    //         where o.Numero.Equals(Convert.ToInt32(spNumero.Value)) && !o.Anulado && !o.Finalizado && o.MovimientoTipoID.Equals(39)
                    //         && (o.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                    //&& (!SUS || (SUS && ((o.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)))))) //|| Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                    //         select o).FirstOrDefault();
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("EsAlcaldia")).Equals(true))
                    {

                        DataTable dt = new DataTable();

                        using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                        {
                            conn.Open();
                            // Creates a SQL command
                            using (var command = new SqlCommand("SELECT * FROM [Rpt_FormatoAlcaldia] where ID = @ID", conn))
                            {
                                //parametro del ID del movimiento Cheque
                                command.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(gvData.GetFocusedRowCellValue("MovimientoID"));
                                // Loads the query results into the table
                                dt.Load(command.ExecuteReader());
                            }

                        }

                        int CB = Parametros.Config.FormatoRetencionAlcaldia();
                        var obj = EtFormatos.FirstOrDefault(o => o.ID.Equals(CB));

                        if (obj != null)
                        {
                            if (obj.Archivo != null)
                            {
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                                rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                                rep.DataSource = dt;
                                rep.RequestParameters = false;
                                rep.ShowPrintMarginsWarning = false;
                                //rep.CreateDocument();

                                rep.PrintingSystem.SetCommandVisibility(DevExpress.XtraPrinting.PrintingSystemCommand.Open, DevExpress.XtraPrinting.CommandVisibility.None);
                                rv.Text = obj.ArchivoNombre;

                                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                                rv.Owner = this.Owner;
                                rv.MdiParent = this.MdiParent;
                                rep.CreateDocument();
                                rv.Show();

                                //Entidad.Movimiento MP = db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                                //MP.Finalizado = true;
                                //db.SubmitChanges();
                            }
                            else
                            { Parametros.General.DialogMsg("La cuenta bancaria no tiene formato de impresión", Parametros.MsgType.warning); }
                        }
                    }
                    else if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("EsAlcaldia")).Equals(false))
                    {
                        DataTable dt = new DataTable();

                        using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                        {
                            conn.Open();
                            // Creates a SQL command
                            using (var command = new SqlCommand("SELECT * FROM [Rpt_FormatoRetencion] where ID = @ID", conn))
                            {
                                //parametro del ID del movimiento Cheque
                                command.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(gvData.GetFocusedRowCellValue("MovimientoID"));
                                // Loads the query results into the table
                                dt.Load(command.ExecuteReader());
                            }

                        }

                        int CB = Parametros.Config.FormatoRetencion();
                        var obj = EtFormatos.FirstOrDefault(o => o.ID.Equals(CB));

                        if (obj != null)
                        {
                            if (obj.Archivo != null)
                            {
                                System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                                rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                                rep.DataSource = dt;
                                rep.RequestParameters = false;
                                rep.ShowPrintMarginsWarning = false;
                                //rep.CreateDocument();

                                rep.PrintingSystem.SetCommandVisibility(DevExpress.XtraPrinting.PrintingSystemCommand.Open, DevExpress.XtraPrinting.CommandVisibility.None);
                                rv.Text = obj.ArchivoNombre;

                                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                                rv.Owner = this.Owner;
                                rv.MdiParent = this.MdiParent;
                                rep.CreateDocument();
                                rv.Show();

                                //Entidad.Movimiento MP = db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                                //MP.Finalizado = true;
                                //db.SubmitChanges();
                            }
                            else
                            { Parametros.General.DialogMsg("La cuenta bancaria no tiene formato de impresión", Parametros.MsgType.warning); }
                        }
                    }

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();

                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

        internal void CleanDialog(bool ShowMSG)
        {
            //nf = null;

            //if (ShowMSG)
            //{
            //    if (ShowMsgDialog)
            //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
            //    else
            //        this.timerMSG.Start();
            //}

            //FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            //try
            //{
            //    if (nf == null)
            //    {
            //        nf = new Forms.Dialogs.DialogProductoClase();
            //        nf.Text = "Crear Clase de Productos";
            //        nf.Owner = this;
            //        nf.MDI = this;
            //        nf.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            //}
        }

        protected override void Edit()
        {
            try
            {
            //    if (nf == null)
            //    {
            //        if (gvData.FocusedRowHandle >= 0)
            //        {
            //            nf = new Forms.Dialogs.DialogProductoClase();
            //            nf.Text = "Editar Clase de Productos";
            //            nf.EntidadAnterior = db.ProductoClases.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
            //            nf.Owner = this;
            //            nf.Editable = true;
            //            nf.MDI = this;
            //            nf.Show();
            //        }
            //    }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void Del()
        {
            try{
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
              
        #endregion

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, "Activo");
        }

        
    }
}
