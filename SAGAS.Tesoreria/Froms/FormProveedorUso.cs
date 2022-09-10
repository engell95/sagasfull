using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormProveedorUso : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormProveedorUso()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Caption = "Eliminar";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            timerRefresh.Start();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var lista = from uso in db.ProveedorUsadoPorEstacions
                            join u in db.Usuarios on uso.UserID equals u.ID
                            join es in db.EstacionServicios on uso.EstacionID equals es.ID
                            join p in db.Proveedors on uso.ProveedorID equals p.ID
                            join sb in db.SubEstacions on uso.SubEstacionID equals sb.ID into ses
                            from sub in ses.DefaultIfEmpty()
                            select new
                            {
                                uso.ID,
                                Estacion = es.Nombre,
                                SubEstacion = (sub == null ? String.Empty : sub.Nombre),
                                Usuario = u.Nombre,
                                Proveedor = p.Codigo + " | " + p.Nombre,
                                uso.Fecha
                            };


                bdsManejadorDatos.DataSource = lista;
                
                this.grid.DataSource = bdsManejadorDatos;

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

            FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Del()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    timerRefresh.Stop();
                    if (Parametros.General.DialogMsg("¿Esta seguro eliminar el registro seleccionado?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.ProveedorUsadoPorEstacions.DeleteOnSubmit(db.ProveedorUsadoPorEstacions.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))));
                        db.SubmitChanges();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                    }
                    FillControl();
                    timerRefresh.Start();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                timerRefresh.Start();
            } 
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            this.FillControl();
            //count++;

            //if (count <= 20)
            //{
            //    this.barMessage.Caption = "Los cambios se han guardado correctamente.";
            //    this.barMessage.Glyph = SAGAS.Parametros.Properties.Resources.informationIcon;
            //}
            //else
            //{
            //    count = 0;
            //    this.barMessage.Caption = "";
            //    this.barMessage.Glyph = null;
            //    timerMSG.Dispose();
            //}
        }

        #endregion
    }
}
