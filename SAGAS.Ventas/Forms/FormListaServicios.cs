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

namespace SAGAS.Ventas.Forms
{
    public partial class FormListaServicios : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Forms.Dialogs.DialogPrecioServicio nf;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormListaServicios()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Caption = "Eliminar";
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

                var lista = from ap in db.ServicioPrecio
                            join p in db.Productos on ap.ServicioID equals p.ID
                            join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                            join a in db.Areas on c.AreaID equals a.ID     
                                               where ap.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) & ap.SubEstacionID.Equals(Parametros.General.SubEstacionID) & p.Activo
                                               select new
                                               {
                                                  ID = ap.ID,
                                                  Servicio = p.Codigo + " " + p.Nombre,
                                                  Area = a.Nombre,
                                                  PrecioTotal = ap.PrecioTotal,
                                                  PrecioVenta = ap.PrecioVenta
                                                 };

                bdsManejadorDatos.DataSource = Parametros.General.LINQToDataTable(lista);
                
                this.grid.DataSource = bdsManejadorDatos;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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

            FillControl();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogPrecioServicio();
                    nf.Text = "Ingresar Precio";
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
                        nf = new Forms.Dialogs.DialogPrecioServicio();
                        nf.Text = "Modificar Precio";
                        nf.EntidadAnterior = db.ServicioPrecio.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        string texto = Convert.ToString(gvData.GetFocusedRowCellValue("Servicio"));
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            var SP = db.ServicioPrecio.Single(c => c.ID == id);

                            db.ServicioPrecio.DeleteOnSubmit(SP);
                            
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas, "Se Inactivó el Precio del Servicio: " + texto, this.Name);

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();

                            FillControl();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        #endregion
    }
}
