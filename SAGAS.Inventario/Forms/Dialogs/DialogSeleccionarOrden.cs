using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.Inventario.Forms;
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogSeleccionarOrden : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataViewsDataContext dv;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IDOrden = 0;
        public List<Entidad.VistaPedidosAbierto> EtPedidos;

        #endregion

        #region *** INICIO ***

        public DialogSeleccionarOrden()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                EtPedidos = dv.VistaPedidosAbiertos.Where(o => o.Litros > 0m).ToList();
                grid.DataSource = (from v in EtPedidos
                                   where v.EstacionID.Equals(Parametros.General.EstacionServicioID)
                                   group v by new { v.ID, v.Estacion, v.Comentario, v.Numero, v.FechaCreado, v.EsIECC} into g
                                   select new
                                   {
                                       g.Key.ID,
                                       g.Key.Estacion,
                                       g.Key.Comentario,
                                       g.Key.Numero,
                                       g.Key.FechaCreado,
                                       g.Key.EsIECC
                                   });

                gvData.RefreshData();
                //glkOrdenes.Properties.DataSource = (from o in dv.VistaPedidoCombustibles 
                //                                    where o.Aprobado && o.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) 
                //                                    group o by new { o.PedidoID, o.Proveedor, o.Numero, o.FechaSolicitud } into g
                //                                    select new { ID = g.Key.PedidoID, ProveedorNombre = g.Key.Proveedor, g.Key.Numero, Fecha = g.Key.FechaSolicitud, Display = g.Key.Numero + " | " + g.Key.Proveedor }).OrderBy(o => o.Fecha);
                //glkOrdenes.Properties.DisplayMember = "Display";
                //glkOrdenes.Properties.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {

            if (gvData.FocusedRowHandle >= 0)
            {
                IDOrden = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                EtPedidos = EtPedidos.Where(o => o.ID.Equals(IDOrden)).ToList();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                Parametros.General.DialogMsg("No ha seleccionado un pedido.", Parametros.MsgType.warning);
                return;
            }
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #endregion

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    gridDetalle.DataSource = null;
                    gridDetalle.DataSource = EtPedidos.Where(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    gvDetalle.RefreshData();
                }
                else
                    gridDetalle.DataSource = null;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

    }


}