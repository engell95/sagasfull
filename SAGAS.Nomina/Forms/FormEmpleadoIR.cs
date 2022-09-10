using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormEmpleadoIR : SAGAS.Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        
        #endregion

        #region <<< INICIO >>>

        public FormEmpleadoIR()
        {
            InitializeComponent();
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void FormSucursal_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                bdsManejadorDatos.DataSource = from v in dv.VistaEmleados
                                               select new
                                               {
                                                   v.ID,
                                                   v.Codigo,
                                                   v.Nombres,
                                                   v.Apellidos,
                                                   v.SalarioActual,
                                                   v.FechaIngreso
                                               };

                this.gvData.RefreshData();
                
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

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        #endregion        

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
        
                    bdsIR.DataSource = null;
                    bdsIR.DataSource = db.IRRetenido.Where(o => o.EmpleadoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();
               }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void gvDataIR_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                bdsIR.EndEdit();
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                bdsIR.CancelEdit();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        

    }
}
