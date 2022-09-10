using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Tesoreria.Hojas
{
    public partial class SubRptECProveedorMov : XtraReport
    {
        private decimal _Saldo = 0;

        public SubRptECProveedorMov()
        {
            InitializeComponent();
        }

        public SubRptECProveedorMov(object Procedure, string fecha, decimal Saldo, Boolean Anticipo)
        {
            InitializeComponent();
            this.xrSetDate.Text = fecha;

            if (Anticipo)
                _Saldo = Saldo;
            else
                _Saldo = -Math.Abs(Saldo);

            this.xfSaldoAnterior.Text = _Saldo.ToString("#,0.00");
            this.DataSource = Procedure;
        }

        private void xrCeSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Saldo += (String.IsNullOrEmpty(xrceMonto.Text) ? 0m : (Convert.ToDecimal(xrceMonto.Text) * -1));
            xrCeSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }

        private void xrCeSaldoFinal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeSaldoFinal.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }



      

    }
}
