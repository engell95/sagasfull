using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class SubRptCierreKardex : XtraReport
    {
        internal decimal x = 0;

        public SubRptCierreKardex(List<Entidad.GetInventarioCorteResult> lista, string area)
        {
            InitializeComponent();
            this.xrArea.Text = area;
            this.xrPivot.DataSource = lista;
        }

        private void xrPivot_CustomSummary(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotGridCustomSummaryEventArgs e)
        {

            if (e.RowField == null)//TOTAL
                e.CustomValue = x.ToString("#,#.00");
            else
            {
                if (e.RowField.FieldName == "ProductoCodigo")
                    x += Convert.ToDecimal(e.SummaryValue.Max); 

                e.CustomValue = e.SummaryValue.Max;
            }
        }
    }
}
