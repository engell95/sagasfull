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
    public partial class FormRptDistribucionExtraxion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        int x = 0;

        #endregion

        #region <<< INICIO >>>

        public FormRptDistribucionExtraxion()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaArqueoProductoExtraxion);
            this.linqInstantFeedbackSource1.KeyExpression = "[ArqueoProductoExtraxionID]";
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
            

                var query = from vev in dbView.VistaExtraxionesVarias
                            join vape in dbView.VistaArqueoProductoExtraxions
                            on vev equals vape.VistaExtraxionesVaria
                            where lista.Contains(vev.EstacionServicioID)
                            && (!vev.ArqueoEspecial || (vev.ArqueoEspecial && vev.Oficial))
                            select new
                            {   
                                vev.EstacionNombre
                                ,vev.ResumenDiaNumero
                                ,vev.ResumenDiaFechaInicial
                                ,vev.TurnoNumero
                                ,vev.IslaNombre
                                ,vev.ArqueoNumero
                                ,vev.ProductoNombre
                                ,vev.Color
                                ,vape.ArqueoProductoExtraxionID
                                ,vape.ExtracionNombre
                                ,Cantidad = (vape.EquivalenteLitros > 0 ? vape.Valor / vape.EquivalenteLitros : 0)
                                ,Litros = vape.Valor
                                ,Galones = vape.Valor / 3.7854m
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
