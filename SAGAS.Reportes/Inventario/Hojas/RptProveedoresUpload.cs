using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class RptProveedoresUpload : DevExpress.XtraReports.UI.XtraReport
    {
        public RptProveedoresUpload()
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
