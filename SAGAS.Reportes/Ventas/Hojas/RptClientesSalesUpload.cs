using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Ventas.Hojas
{
    public partial class RptClientesSalesUpload : DevExpress.XtraReports.UI.XtraReport
    {
        public RptClientesSalesUpload()
        {
            InitializeComponent();
        }

        private void xrPivotGridDet_CustomSummary(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotGridCustomSummaryEventArgs e)
        {
           
        }

        //private void xrPivotGridDet_FieldValueDisplayText(object sender, DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventArgs e)
        //{
        //    if (e.IsColumn == true)// ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
        //        e.;
        //}
              
    }
}
