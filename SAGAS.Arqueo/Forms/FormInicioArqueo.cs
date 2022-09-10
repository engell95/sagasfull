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

namespace SAGAS.Arqueo.Forms
{
    public partial class FormInicioArqueo : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        private int Fila = 0;
        //private int Usuario = Parametros.General.UserID;
        //private int IDES = Parametros.General.EstacionServicioID;
        //private int IDEfectivo = Parametros.Config.IDFormaPagoEfectivo();

        #endregion

        #region <<< INICIO >>>

        public FormInicioArqueo()
        {
            InitializeComponent();
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            FillControl().ConfigureAwait(false);
            //dashViewer.Dashboard.DataSources = 
        }

        #endregion

        #region <<< METODOS >>>

        async Task FillControl()
        {
            try
            {
                await Task.Delay(10);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                this.gridESRD.DataSource = db.GetListaArqueosAprobados(Parametros.General.UserID);

                this.bgvDataESRD.RefreshData();    
            }            

            catch (Exception ex)
            {                
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

       
              
        #endregion    

        //PRECIOS
        private async void gvDataESRD_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (bgvDataESRD.FocusedRowHandle >= 0)
            {
                try
                {
                    await FilaArqueoCambiada(Convert.ToInt32(bgvDataESRD.GetFocusedRowCellValue(colID)), Convert.ToInt32(bgvDataESRD.GetFocusedRowCellValue(colIDSES)));
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            }
        }

        async Task FilaArqueoCambiada(int IDPC, int IDPCSub)
        {
            try
            {

                await Task.Delay(10);

                //PRECIOS
                var Precios = (from pcd in db.PrecioCombustibleDetalles
                               join pc in db.PrecioCombustibles on pcd.PrecioCombustibleID equals pc.ID
                               join p in db.Productos on pcd.ProductoID equals p.ID
                               join es in db.EstacionServicios on pcd.EstacionServicioID equals es.ID
                               where pcd.EstacionServicioID.Equals(IDPC) && pcd.SubEstacionID.Equals(IDPCSub)
                               select new
                               {
                                   Producto = p.Nombre,
                                   p.Codigo,
                                   pcd.Precio,
                                   pc.FechaInicial,
                                   es.Nombre
                               }).OrderBy(z => z.FechaInicial).ToList();

                chartControlPrice.DataSource = Precios;
                chartControlPrice.SeriesDataMember = "Producto";
                chartControlPrice.SeriesTemplate.ArgumentDataMember = "FechaInicial";
                chartControlPrice.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Precio" });
                chartControlPrice.SeriesTemplate.View = new LineSeriesView();
                XYDiagram diagram = (XYDiagram)chartControlPrice.Diagram;
                diagram.AxisY.VisualRange.MinValue = 20;
                diagram.EnableAxisXScrolling = true; //EnableAxisXZooming 
                diagram.EnableAxisXZooming = true;

                //Tanques
                var Tanques = db.GetVistaTanqueLitro(IDPC, IDPCSub).ToList();
                chartControlTank.DataSource = Tanques;
                //chartControlTank.DataSource = Tanques;
                //chartControlTank.SeriesDataMember = "Producto";
                //chartControlTank.SeriesTemplate.ArgumentDataMember = "FechaInicial";
                //chartControlTank.SeriesTemplate.ValueDataMembers.AddRange(new string[] { "Precio" });
                //chartControlTank.SeriesTemplate.View = new BarSeriesView();
                //XYDiagram diagram2 = (XYDiagram)chartControlTank.Diagram;
                //diagram2.AxisY.VisualRange.MinValue = 20;
                //diagram2.EnableAxisXScrolling = true; //EnableAxisXZooming 
                //diagram2.EnableAxisXZooming = true;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnExportGrid_Click(object sender, EventArgs e)
        {
            Parametros.General.PrintExportComponet(false, this.Text, this.MdiParent, null, null, this.gridESRD, false, 25, 25, 140, 50, 1);
        }

        //VENTAS
       
        private void btnExportPivot_Click(object sender, EventArgs e)
        {
           
        }

        private void dashboardViewer1_Load(object sender, EventArgs e)
        {

        }

        #region <<< EVENTOS >>>

        

        #endregion
               
    }
}
