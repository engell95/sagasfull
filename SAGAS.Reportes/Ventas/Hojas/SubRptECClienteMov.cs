using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Ventas.Hojas
{
    public partial class SubRptECClienteMov : XtraReport
    {
        public decimal _Saldo = 0;

        public SubRptECClienteMov()
        {
            InitializeComponent();
        }

        public SubRptECClienteMov(object Procedure, string fecha, decimal Saldo)
        {
            InitializeComponent();
            this.xrSetDate.Text = fecha;
            _Saldo = Saldo;
            this.xfSaldoAnterior.Text = _Saldo.ToString("#,0.00");
            this.DataSource = Procedure;
        }

        private void xrCeSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Saldo += (String.IsNullOrEmpty(xrceMonto.Text) ? 0m : Convert.ToDecimal(xrceMonto.Text));
            xrCeSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }

        private void xrCeSaldoFinal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeSaldoFinal.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }



      

    }
}
