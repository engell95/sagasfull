using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Administracion.Forms
{                                
    public partial class FormProductoClase : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogProductoClase nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int IDClaseCombustible = Parametros.Config.ProductoClaseCombustible();

        #endregion

        #region <<< INICIO >>>

        public FormProductoClase()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Caption = "Inactivar";
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

                bdsManejadorDatos.DataSource = from P in db.ProductoClases
                                               select P;

                //**AREAS
                lkrArea.DataSource = from a in db.Areas
                                                  where a.Activo
                                                  select new { a.ID, a.Nombre };

                lkrArea.DisplayMember = "Nombre";
                lkrArea.ValueMember = "ID";

                
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
                    nf = new Forms.Dialogs.DialogProductoClase();
                    nf.Text = "Crear Clase de Productos";
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
                        nf = new Forms.Dialogs.DialogProductoClase();
                        nf.Text = "Editar Clase de Productos";
                        nf.EntidadAnterior = db.ProductoClases.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var P = db.ProductoClases.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                if (P.ID == IDClaseCombustible)
                                {
                                    Parametros.General.DialogMsg("La clase no se puede borrar debido a que esta asignada al combustible" + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                //if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == Z.ID && o.Activo).Count()) > 0)
                                //{
                                //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
                                //    return;
                                //}

                                //Desactivar Registro 
                                P.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó la Clase de Productos: " + P.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                //Activar Registro 
                                P.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó la Clase de Productos: " + P.Nombre, this.Name);
                                this.btnAnular.Caption = "Inactivar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
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

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, "Activo");
        }

        
    }
}
