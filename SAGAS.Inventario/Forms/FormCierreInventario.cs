using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using DevExpress.XtraCharts;

namespace SAGAS.Inventario.Forms
{
    public partial class FormCierreInventario : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        internal int UsuarioID = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormCierreInventario()
        {
            InitializeComponent();
        }


        #endregion

        #region <<< PIVOT >>>
        
        async Task ListaTotales(bool EsCombustible)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                await Task.Delay(10);
                dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                pivotGridControlListaComb.DataSource = pivotGridControlListaProd.DataSource = null;

                List<int> listaES = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == UsuarioID).Select(s => s.EstacionServicioID));

                if (EsCombustible)
                {
                    //LISTA
                    var ListaProd = from sp in dbView._Rpt_CierreInventariosCombs
                                    where listaES.Contains(sp.EstacionServicioID)
                                    select sp;

                    splitContainerControlMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                    pivotGridControlListaComb.DataSource = ListaProd;
                    pivotGridControlListaComb.RefreshData();
                }
                else
                {
                    //LISTA
                    var ListaProd = from sp in dbView._Rpt_CierreInventarios
                                    where listaES.Contains(sp.EstacionServicioID)
                                    select sp;

                    splitContainerControlMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                    pivotGridControlListaProd.DataSource = ListaProd;
                    pivotGridControlListaProd.RefreshData();
                }
                Parametros.General.splashScreenManagerMain.CloseWaitForm();

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        private void btnExport_Click(object sender, EventArgs e)
        {
             try
             {
                 if (tgSwProducto.IsOn)//Otros Productos
                 {
                     if (vistaListaProd.DataSource != null)
                     {
                         this.pivotGridControlListaProd.ShowPrintPreview();
                     }
                 }
                 else//Combustible
                 {
                     if (vistaListaComb.DataSource != null)
                     {
                         this.pivotGridControlListaComb.ShowPrintPreview();
                     }
                 }
             }
             catch (Exception ex)
             {
                 Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
             }
        }

        private async void btnLoad_Click_1(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(10);
                await ListaTotales(!tgSwProducto.IsOn).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
 
    }
}
