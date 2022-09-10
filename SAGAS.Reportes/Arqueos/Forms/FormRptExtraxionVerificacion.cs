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
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;

namespace SAGAS.Reportes.Arqueos.Forms
{
    public partial class FormRptExtraxionVerificacion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int IDEfectivo = Parametros.Config.IDFormaPagoEfectivo();
        //private Reportes.Arqueos.Charts.FormChartExtraxionVerificacion nf;

        #endregion

        #region <<< INICIO >>>

        public FormRptExtraxionVerificacion()
        {
            InitializeComponent();
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaArqueoManguera);
            this.linqInstantFeedbackSource1.KeyExpression = "[ArqueoMangueraID]";
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
                linqInstantFeedbackSource1.GetQueryable += linqInstantFeedbackSource1_GetQueryable;

                this.grid.DataSource = linqInstantFeedbackSource1;

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("ResumenDiaFechaInicial") > fecha.AddDays(-1)).ToString();
              }
            finally { this.gvData.RefreshData(); }
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

       
        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }      
        

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            this.Edit();
        }
      
        #endregion    

        #region <<< EVENTOS >>>

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                List<int> lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                 
                var query = from vai in dbView.VistaArqueoIslas
                            join vam in dbView.VistaArqueoMangueras on vai equals vam.VistaArqueoIsla
                            where lista.Contains(vai.EstacionServicioID)
                            && (!vai.ArqueoEspecial || (vai.ArqueoEspecial && vai.Oficial))
                            select new
                            {
                                 vai.EstacionNombre
                                ,vai.ResumenDiaNumero
                                ,vai.ResumenDiaFechaInicial
                                ,vai.TurnoNumero
                                ,vai.IslaNombre
                                ,vai.ArqueoNumero
                                ,vam.ArqueoMangueraID
                                ,vam.TanqueNombre
                                ,vam.DispensadorNombre
                                ,vam.ProductoNombre
                                ,vam.MangueraNumero
                                ,vam.Lado
                                ,vam.VerificacionEfectivo
                                ,vam.VerificacionMecElectronica
                                ,Extraxion = (vam.EsLecturaElectronica ? vam.ExtraxionElectronica : vam.ExtraxionMecanica)
                                
                            };

                e.QueryableSource = query;
                e.Tag = dbView;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }
        

        #endregion
               
    }
}
