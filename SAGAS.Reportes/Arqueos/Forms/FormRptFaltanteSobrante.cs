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
using System.Text.RegularExpressions;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;

namespace SAGAS.Reportes.Arqueos.Forms
{
    public partial class FormRptFaltanteSobrante : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;

        #endregion

        #region <<< INICIO >>>

        public FormRptFaltanteSobrante()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaFaltantesSobrante);
            this.linqInstantFeedbackSource1.KeyExpression = "[ArqueoIslaID]";
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
            
                //**TIPOS
                Parametros.TiposFaltantesSobrantes listado = new Parametros.TiposFaltantesSobrantes();
                rpTipo.DataSource = listado.GetListEstadosArqueo();
                rpTipo.DisplayMember = "Name";
                rpTipo.ValueMember = "ID";
                rpTipo.Columns.Clear();
                this.rpTipo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Tipo")});
                                
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

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                List<int> lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));

                var query = from vfs in dbView.VistaArqueoIslas
                            where lista.Contains(vfs.EstacionServicioID)
                            && (!vfs.ArqueoEspecial ||  (vfs.ArqueoEspecial && vfs.Oficial))
                            select new
                            {
                                vfs.ArqueoIslaID,
                                vfs.EstacionNombre,
                                vfs.ResumenDiaNumero,
                                vfs.ResumenDiaFechaInicial,
                                vfs.TurnoNumero,
                                vfs.ArqueoNumero,
                                vfs.IslaNombre,
                                vfs.TecnicoNombre,
                                vfs.DiferenciaRecibida,
                                vfs.SobranteFaltante,
                                Tipo =
                                (
                                    vfs.SobranteFaltante > 0.00m ? 1 :
                                    vfs.SobranteFaltante < 0.00m ? 2 : 0
                                )
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
        
    }
}
