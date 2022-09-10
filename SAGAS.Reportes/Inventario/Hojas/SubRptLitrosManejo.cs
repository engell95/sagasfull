using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class SubRptLitrosManejo : XtraReport
    {
        private decimal _Saldo = 0;
        private decimal _Entrada = 0;
        private decimal _Salida = 0;

        public SubRptLitrosManejo()
        {
            InitializeComponent();
            this.xrCellList.DataBindings.Add("Text", null, "Nombre");
        }

        public SubRptLitrosManejo(string Producto, decimal Litros, decimal Galones, object Procedure)
        {
            InitializeComponent();
            this.xrCeProducto.Text = Producto;
            _Saldo = Litros;
            this.xrCeSaldoLitros.Text = Litros.ToString("#,0.000");
            this.xrCeSaldoGlns.Text = Galones.ToString("#,0.00");
            this.DataSource = Procedure;
        }

        private void xrTableCell28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Saldo += (String.IsNullOrEmpty(xrCeEntrada.Text) ? 0 : Convert.ToDecimal(xrCeEntrada.Text)) - (String.IsNullOrEmpty(xrCeSalida.Text) ? 0 : Convert.ToDecimal(xrCeSalida.Text));

            if (!String.IsNullOrEmpty(xrCeEntrada.Text))
            {
                if (Convert.ToDecimal(xrCeEntrada.Text) > 0)
                    _Entrada += Convert.ToDecimal(xrCeEntrada.Text);
            }

            if (!String.IsNullOrEmpty(xrCeSalida.Text))
            {
                if (Convert.ToDecimal(xrCeSalida.Text) > 0)
                    _Salida += Convert.ToDecimal(xrCeSalida.Text);
            }

            xrTableCell28.Text = Convert.ToDecimal(_Saldo).ToString("#,0.000");
            xrTableCell25.Text = Decimal.Round(_Saldo / 3.785m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell26.Text = Convert.ToDecimal(_Saldo).ToString("#,0.000");
            xrTableCell27.Text = Decimal.Round(_Saldo / 3.785m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        private void xrCeEntradaLtrs_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeEntradaLtrs.Text = Convert.ToDecimal(_Entrada).ToString("#,0.000");
            xrCeEntradaGlns.Text = Decimal.Round(_Entrada / 3.785m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        private void xrCeSalidaLtrs_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeSalidaLtrs.Text = Convert.ToDecimal(_Salida).ToString("#,0.000");
            xrCeSalidaGlns.Text = Decimal.Round(_Salida / 3.785m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }


      

    }
}
