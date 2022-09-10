using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Parametros.Forms
{
    public partial class FormBase : Form
    {
        #region <<< DECLARACIONES >>>

        public int count = 0;

        #endregion

        #region <<< INICIO >>>

        public FormBase()
        {
            InitializeComponent();
        }

        #endregion

        #region <<< EVENTOS DE LLAMADOS A METODOS GENERICOS >>>

        private void barImprimir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Imprimir();
        }

        private void barExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportarExcel();
        }

        private void barPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportarPDF();
        }

        private void barSalir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void timerMSG_Tick(object sender, EventArgs e)
        {
            count++;

            if (count <= 20)
            {
                this.barMessage.Caption = "Los cambios se han guardado correctamente.";
                this.barMessage.Glyph = SAGAS.Parametros.Properties.Resources.informationIcon;
            }
            else
            {
                count = 0;
                this.barMessage.Caption = "";
                this.barMessage.Glyph = null;
                timerMSG.Dispose();
            }
        }
    
        private void btnAgregar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Add();
        }

        private void btnModificar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Edit();
        }

        private void btnAnular_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Del();
        }

        private void barLimpiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CleanFilter();
        }

        private void barRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.FillControl();
            this.FillControl(true);
        }

        #endregion

        #region <<< METODOS GENERICOS DEL FORMBASE >>>

        protected virtual void FillControl()
        {

        } //Llenar Controles // Refresh

        protected virtual void FillControl(bool Refresh)
        {

        }

        // ------METODOS DE ACCESO A DATOS ---

        protected virtual void PrintList(DevExpress.XtraGrid.GridControl grid)
        {
            Parametros.General.PrintExportComponet(true, this.Text, this.MdiParent, null, null, grid, false, 25, 25, 140, 50, 0);
        }

        protected virtual void PrintList(DevExpress.XtraTreeList.TreeList tree)
        {
            Parametros.General.PrintExportComponet(true, this.Text, this.MdiParent, null, null, tree, false, 25, 25, 140, 50, 0);
        }

        protected virtual void ExportListToExcel(DevExpress.XtraGrid.GridControl grid)
        {
            Parametros.General.PrintExportComponet(false, this.Text, this.MdiParent, null, null, grid, false, 25, 25, 140, 50, 1);
        }

        protected virtual void ExportListToPDF(DevExpress.XtraGrid.GridControl grid)
        {
            Parametros.General.PrintExportComponet(false, this.Text, this.MdiParent, null, null, grid, false, 25, 25, 140, 50, 2);
        }

        protected virtual void ExportListToExcel(DevExpress.XtraTreeList.TreeList tree)
        {
            Parametros.General.PrintExportComponet(false, this.Text, this.MdiParent, null, null, tree, false, 25, 25, 140, 50, 1);
        }

        protected virtual void ExportListToPDF(DevExpress.XtraTreeList.TreeList tree)
        {
            Parametros.General.PrintExportComponet(false, this.Text, this.MdiParent, null, null, tree, false, 25, 25, 140, 50, 2);
        }


        public void Export(DevExpress.XtraGrid.GridControl grid, int exportTo)
        {
            switch (exportTo)
            {
                case 1: this.dglExportToFile.Filter = "Hoja de Microsoft Excel (*.xls)|*.xls";
                    break;
                case 2: this.dglExportToFile.Filter = "Archivo Formato pdf (*.pdf)|*.pdf";
                    break;
                //case 3: this.dlgExportToFile.Filter = "Documento HTML (*.htm)|*.htm";
                //    break;
            }
            if (this.dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.dglExportToFile.FileName != "")
                {
                    switch (exportTo)
                    {
                        case 1:
                            grid.MainView.ExportToXls(this.dglExportToFile.FileName);
                            break;
                        case 2:
                            grid.MainView.ExportToPdf(this.dglExportToFile.FileName);
                            break;
                        //case 3:
                        //    this.dgData.MainView.ExportToHtml(this.dlgExportToFile.FileName);
                        //    break;
                    }
                }
            }

        }

        protected virtual void Imprimir()
        {

        }

        protected virtual void ExportarExcel()
        {

        }

        protected virtual void ExportarPDF()
        {

        }

        protected virtual void CleanFilter()
        {

        }

        protected virtual void Add()
        {

        } //Add

        protected virtual void Edit()
        {

        } //Edit

        protected virtual void Del()
        {

        }            
        
        #endregion

        
        
    }
}
