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

namespace SAGAS.Reportes.Arqueos.Charts
{
    public partial class FormChartExtraxionVerificacion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        public object Dts;
        //private int Usuario = Parametros.General.UserID;
        //private int IDES = Parametros.General.EstacionServicioID;
        //private int IDEfectivo = Parametros.Config.IDFormaPagoEfectivo();

        #endregion

        #region <<< INICIO >>>

        public FormChartExtraxionVerificacion()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; 
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
                dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                //linqInstantFeedbackSource1.GetQueryable += linqInstantFeedbackSource1_GetQueryable;

                this.pivotGridControlMain.DataSource = Dts;

                //this.gvData.RefreshData();                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override void Imprimir()
        {
            //this.PrintList(grid);

        }

        protected override void ExportarExcel()
        {
            //this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            //this.ExportListToPDF(grid);
        }

       
        protected override void CleanFilter()
        {
            //this.gvData.ActiveFilter.Clear();
        }      
        

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            //this.Edit();
        }
              
        #endregion    

        #region <<< EVENTOS >>>

        

        #endregion
               
    }
}
