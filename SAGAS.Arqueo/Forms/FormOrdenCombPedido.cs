using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Arqueo.Forms
{                                
    public partial class FormOrdenCombPedido : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogOrdenCombPedido nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDCombustible = Parametros.Config.ProductoClaseCombustible();

        #endregion

        #region <<< INICIO >>>

        public FormOrdenCombPedido()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Eliminar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
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

                bdsManejadorDatos.DataSource = from T in db.OrdenCombustiblePedidos
                                                   //join p in db.Productos
                                               select T;
                                               //{
                                               //    T.ID,
                                               //    T.EstacionServicioID,
                                               //    T.Nombre,
                                               //    T.ProductoID,
                                               //    T.Color,
                                               //    T.Capacidad,
                                               //    T.Litros,
                                               //    T.Diametro,
                                               //    T.Longitud,
                                               //    T.Altura,
                                               //    T.Marca,
                                               //    T.Serie,
                                               //    T.Proveedor,
                                               //    T.TipoTanqueID,
                                               //    T.Comentario,
                                               //    T.SubEstacionID,
                                               //    T.Activo
                                               //};

               
                this.grid.DataSource = bdsManejadorDatos;

                //**Produto
                lkrcolProducto.DataSource = from p in db.Productos
                                            where p.Activo && p.ProductoClaseID == IDCombustible 
                                            select new { p.ID, p.Nombre };

                lkrcolProducto.DisplayMember = "Nombre";
                lkrcolProducto.ValueMember = "ID";

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
                    nf = new Forms.Dialogs.DialogOrdenCombPedido(Usuario, IDCombustible);
                    nf.Text = "Crear Orden";
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
                        nf = new Forms.Dialogs.DialogOrdenCombPedido(Usuario, IDCombustible);
                        nf.Text = "Editar Orden";
                        nf.EntidadAnterior = db.OrdenCombustiblePedidos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var T = db.OrdenCombustiblePedidos.Single(c => c.ID == id);

                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                //if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == P.ID && o.Activo).Count()) > 0)
                                //{
                                //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
                                //    return;
                                //}

                                //Desactivar Registro 
                                db.OrdenCombustiblePedidos.DeleteOnSubmit(T);
                                db.SubmitChanges();

                                //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se eliminó el Orden: " + T.Nombre, this.Name);
                                //this.btnAnular.Caption = "Activar";

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
              
        #endregion

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (gvData.FocusedRowHandle >= 0)
            //    Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
        }


        
    }
}
