using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Ventas.Hojas
{
    public partial class RptClientesCreditos : DevExpress.XtraReports.UI.XtraReport
    {
        internal decimal vConcentracion = 0m;
        internal decimal tSobregiro = 0m;
        internal decimal tLimite = 0m;
        
        public RptClientesCreditos(decimal Concentracion)
        {
            InitializeComponent();
            vConcentracion = Concentracion;
        }
                
        private void xrCeSobregiro_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            decimal valor = Convert.ToDecimal(e.Value);
            decimal vSobregiro;
            vSobregiro = Convert.ToDecimal((String.IsNullOrEmpty(xrTableCell14.Text) ? 0m : Convert.ToDecimal(xrTableCell14.Text)) - Convert.ToDecimal(e.Value));

            if (vSobregiro < 0)
            {
                e.Text = vSobregiro.ToString("#,0.00");
                tSobregiro += vSobregiro;
            }
            else
            {
                e.Text = "-";
            }
                   
        }

        private void xrTableCell14_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            
        }

        private void xrTableCell14_AfterPrint(object sender, EventArgs e)
        {
            tLimite += (String.IsNullOrEmpty(xrTableCell14.Text) ? 0m : Convert.ToDecimal(xrTableCell14.Text));
        }
        
        private void xrTableCell11_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            e.Text = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(e.Value) / vConcentracion) * 100m), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");        
       }

        private void xrTableCell36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell36.Text = tSobregiro.ToString("#,0.00");
        }

        private void xrTableCell17_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell17.Text = tLimite.ToString("#,0.00");
        }

        private void xrChartTotal_CustomDrawAxisLabel(object sender, DevExpress.XtraCharts.CustomDrawAxisLabelEventArgs e)
        {
            if (e.Item.GetType() == typeof(DevExpress.XtraCharts.AxisLabelItem))
            {
                if (e.Item.Axis.GetType() == typeof(DevExpress.XtraCharts.AxisX))
                {
                    switch (e.Item.Text)
                    {
                        case "0":
                            e.Item.Text = "Corriente";
                            break;
                        case "1":
                            e.Item.Text = "De 1 - 8 Días";
                            break;
                        case "2":
                            e.Item.Text = "De 9 - 15 Días";
                            break;
                        case "3":
                            e.Item.Text = "De 16 - 30 Días";
                            break;
                        case "4":
                            e.Item.Text = "Más de 31 Días";
                            break;
                    }
                }
            }
        }

       

       

        
                      
    }
}
