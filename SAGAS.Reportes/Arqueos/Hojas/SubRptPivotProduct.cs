using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Forms;
using DevExpress.XtraReports.UI.PivotGrid;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptPivotProduct : XtraReport
    {
        public SubRptPivotProduct(float AnchoTabla, bool MostrarFull, float AnchoFull,  bool MostrarDiferenciado, float AnchoDiferenciado
            , object dataTotal, object dataDist, object dataValorVenta)
        {
            try
            {
                InitializeComponent();
                this.xrTableTitulo.WidthF = AnchoTabla;

                this.xrPivotGridProducto.DataSource = dataTotal;

                //Distribucion por extraxion
                this.xrTableDist.WidthF = this.xrTableTitulo.WidthF;
                this.xrDistExtraxion.WidthF = this.xrTableTitulo.WidthF;
                this.xrPivotGridDist.DataSource = dataDist;
                this.xrPivotGridDist.OptionsView.ShowColumnTotals = false;


                //Datos de la Venta
                this.xrTableDatosTitulo.WidthF = this.xrTableTitulo.WidthF;
                this.xrSubreportValorVenta.ReportSource = new Reportes.Arqueos.Hojas.SubRptValoresVenta("ExtraVenta", "PrecioVenta", "Descuento", "ValorVenta");
                this.xrSubreportValorVenta.ReportSource.DataSource = dataValorVenta;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void xrCellValores_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            if (cell.Text.Equals("0.00") || cell.Text.Equals("0.000"))
                cell.Text = "-";
        }

        private void xrPivotGridProducto_PrintFieldValue(object sender, DevExpress.XtraPivotGrid.CustomExportFieldValueEventArgs e)
        {
            
        }

        private void xrPivotGridDist_CustomCellValue(object sender, DevExpress.XtraPivotGrid.PivotCellValueEventArgs e)
        {
            
        }

        private void xrPivotGridProducto_FieldValueDisplayText(object sender, DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventArgs e)
        {
            
        }

        private void xrPivotGridProducto_FieldValueDisplayText(object sender, PivotFieldDisplayTextEventArgs e)
        {
            if (e.Field.FieldName.Equals("EsDiferenciado"))
            {
                if (e.Value.Equals(true))
                    e.DisplayText = "PRECIO DIFERENCIADO";
                else
                    e.DisplayText = "PRECIO FULL";
            }
        }

        private void xrPivotGridProducto_PrintFieldValue(object sender, CustomExportFieldValueEventArgs e)
        {
            try
            {
                if (e.Field.FieldName.Equals("ProductoNombre"))
                {
                    int charLocation = e.Value.ToString().IndexOf(" =>", StringComparison.Ordinal);

                    string txt = e.Value.ToString().Substring(0, charLocation);
                
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Parametros.General.ListColTanques.Find(f => f.Nombre.Contains(txt)).Color));
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch { e.Appearance.BackColor = Color.White; }
        }

        private void xrPivotGridDist_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            if (e.Value.Equals(0m))
                e.Value = "-";
        }

    }
}
