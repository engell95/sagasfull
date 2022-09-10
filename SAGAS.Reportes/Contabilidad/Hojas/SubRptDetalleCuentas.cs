using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Contabilidad.Hojas
{
    public partial class SubRptDetalleCuentas : XtraReport
    {
        private decimal _Saldo = 0;

        public SubRptDetalleCuentas()
        {
            InitializeComponent();
        }

        public SubRptDetalleCuentas(string Codigo, string Nombre, object Procedure, string fecha, decimal Saldo)
        {
            InitializeComponent();
            this.xrSetDate.Text = fecha;
            _Saldo = Saldo;
            this.xfSaldoAnterior.Text = _Saldo.ToString("#,0.00");            
            this.xrCuentaCodigo.Text = Codigo;
            this.xrCeCuentaNombre.Text = Nombre;
            this.DataSource = Procedure;
        }
                
        private void xrCeSaldo_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Saldo += (String.IsNullOrEmpty(xrCeMonto.Text) ? 0m : Convert.ToDecimal(xrCeMonto.Text));
            xrCeSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }

        private void xrCeTotalSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeTotalSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }



      

    }
}
