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
    public partial class FormProducto : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogProducto nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormProducto()
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

                var cuentas = db.CuentaContables.Where(c => c.Activo && c.Detalle).Select(s => new { s.ID, Display = s.Codigo + " | " + s.Nombre });
                var clases = from c in db.ProductoClases
                             join a in db.Areas on c.AreaID equals a.ID
                             select new
                             {
                                 c.ID,
                                 Area = a.Nombre,
                                 a.CuentaVentaID,
                                 a.CuentaInventarioID,
                                 a.CuentaCostoID,
                                 a.CuentaDescuentoID,
                                 a.CuentaSobranteID,
                                 a.CuentaFaltanteID
                             };

                bdsManejadorDatos.DataSource = from P in db.Productos
                                               select new
                                               {                                                   
                                                   P.ID,
                                                   P.Codigo,
                                                   P.Nombre,
                                                   P.ProductoClaseID,
                                                   Area = clases.Single(c => c.ID.Equals(P.ProductoClaseID)).Area,
                                                   P.UnidadMedidaID,
                                                   P.Comentario,
                                                   P.EsServicio,
                                                   P.ExentoIVA,
                                                   P.AplicaISC,
                                                   P.Activo,
                                                   CuentaVenta = (P.CuentaVentaID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaVentaID)).First().Display :
                                                   (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaVentaID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaVentaID))).First().Display : "< N/A >")),
                                                   CuentaInventario = (P.CuentaInventarioID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaInventarioID)).First().Display :
                                                   (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaInventarioID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaInventarioID))).First().Display : "< N/A >")),
                                                   CuentaCosto = (P.CuentaCostoID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaCostoID)).First().Display :
                                                   (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaCostoID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaCostoID))).First().Display : "< N/A >")),
                                                   CuentaDescuento = (P.CuentaDescuentoID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaDescuentoID)).First().Display :
                                                   (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaDescuentoID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaDescuentoID))).First().Display : "< N/A >")),
                                                   CuentaSobrante = (P.CuentaSobranteID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaSobranteID)).First().Display :
                                                       (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaSobranteID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaSobranteID))).First().Display : "< N/A >")),
                                                   CuentaFaltante = (P.CuentaFaltanteID > 0 ? cuentas.Where(c => c.ID.Equals(P.CuentaFaltanteID)).First().Display :
                                                       (clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaFaltanteID > 0 ? cuentas.Where(cc => cc.ID.Equals(Convert.ToInt32(clases.Single(c => c.ID.Equals(P.ProductoClaseID)).CuentaFaltanteID))).First().Display : "< N/A >"))
                                               
                                               };

                
                this.grid.DataSource = bdsManejadorDatos;

                //**ClaseProducto
                lkrClaseProducto.DataSource = from pc in db.ProductoClases
                                        where pc.Activo
                                                 select new { pc.ID, pc.Nombre };

                lkrClaseProducto.DisplayMember = "Nombre";
                lkrClaseProducto.ValueMember = "ID";

                //**UnidadMedida
                lkrUnidadMedida.DataSource = from um in db.UnidadMedidas
                                                  where um.Activo
                                                  select new { um.ID, um.Nombre };

                lkrUnidadMedida.DisplayMember = "Nombre";
                lkrUnidadMedida.ValueMember = "ID";

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
                    nf = new Forms.Dialogs.DialogProducto();
                    nf.Text = "Crear Producto";
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
                        nf = new Forms.Dialogs.DialogProducto();
                        nf.Text = "Editar Producto";
                        nf.EntidadAnterior = db.Productos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var P = db.Productos.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                if (db.Kardexes.Count(o => o.ProductoID.Equals(P.ID)) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                //Desactivar Registro 
                                P.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó el Producto: " + P.Nombre, this.Name);
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

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó el Producto: " + P.Nombre, this.Name);
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
