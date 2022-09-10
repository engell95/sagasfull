using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class SubRptComprobante : DevExpress.XtraReports.UI.XtraReport
    {
        public SubRptComprobante(object Lista)
        {
            InitializeComponent();
            this.DataSource = Lista;
        }

        //private void xrPivotGridDet_FieldValueDisplayText(object sender, DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventArgs e)
        //{
        //    if (e.IsColumn == true)// ValueType == DevExpress.XtraPivotGrid.PivotGridValueType.GrandTotal)
        //        e.;
        //}
              
    }
}
