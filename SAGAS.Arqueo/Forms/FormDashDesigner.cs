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
    public partial class FormDashDesigner : Form
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

        public FormDashDesigner()
        {
            InitializeComponent();
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
            //dashboardDesigner1.Dashboard.DataSources = 
        }

        #endregion

        #region <<< METODOS >>>

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                //dashboardDesigner1.Dashboard.AddDataSource("Arqueos", db.GetListaArqueosAprobados(Parametros.General.UserID));
                //dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                //var ListESRD = from rd in db.ResumenDias
                //               join es in db.EstacionServicios on rd.EstacionServicioID equals es.ID
                //               join sus in db.SubEstacions on rd.SubEstacionID equals sus.ID into ses from sub in ses.DefaultIfEmpty()
                //               join z in db.Zonas on es.ZonaID equals z.ID
                //               where (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == rd.EstacionServicioID))
                //               group rd by new { rd.EstacionServicioID, rd.SubEstacionID, Estacion = es.Nombre, SubEstacion = sub.Nombre, ZonaNombre = z.Nombre} into gr
                //               select new
                //               {
                //                   IDES = gr.Key.EstacionServicioID,
                //                   IDSES = gr.Key.SubEstacionID,
                //                   Zona = gr.Key.ZonaNombre,
                //                   Estacion = (String.IsNullOrEmpty(gr.Key.SubEstacion) ? gr.Key.Estacion : gr.Key.SubEstacion),
                //                   FechaArqueo = gr.Max(m => m.FechaInicial).ToShortDateString(),
                //                   FechaAprobado = (gr.Count(o => o.Aprobado) > 0 ? gr.Where(o => o.Aprobado).Max(m => m.FechaInicial).ToShortDateString() : gr.Max(m => m.FechaInicial).ToShortDateString()),
                //                   DiaNoArqueo = SqlMethods.DateDiffDay(Convert.ToDateTime(gr.Max(m => m.FechaInicial)),Convert.ToDateTime(db.GetDateServer())),
                //                   DiaNoAprobado = SqlMethods.DateDiffDay(Convert.ToDateTime((gr.Count(o => o.Aprobado) > 0 ? gr.Where(r => r.Aprobado).Max(m => m.FechaInicial) : gr.Max(m => m.FechaInicial))), Convert.ToDateTime(db.GetDateServer()))                                   
                //               };

                //this.gridESRD.DataSource = ListESRD;

                //this.bgvDataESRD.RefreshData();

                //var Move = (from vm in dbView.VistaMovimientos.ToList() 
                //where !vm.Anulado && !vm.EsAnulado && !vm.MovimientoTipoID.Equals(11) && !vm.MovimientoTipoID.Equals(43) && !vm.MovimientoTipoID.Equals(44)
                //group vm by new {vm.EstacionServicioID, vm.SubEstacionID, vm.EstacionNombre, vm.SubEstacionNombre, vm.MovimientoTipoNombre} into gr
                //select new 
                //{ 
                //Estacion = (String.IsNullOrEmpty(gr.Key.SubEstacionNombre) ? gr.Key.EstacionNombre : gr.Key.SubEstacionNombre), 
                //gr.Key.MovimientoTipoNombre,
                //Fecha = gr.Max(m => m.FechaContabilizacion).ToShortDateString(),
                //Cantidad = gr.Count(c => c.MovimientoTipoID > 0)
                //}).OrderBy(o => o.Estacion).ToList();

            }            
            catch (Exception ex)
            {                
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

       
              
        #endregion    

       


        #region <<< EVENTOS >>>

        

        #endregion
               
    }
}
