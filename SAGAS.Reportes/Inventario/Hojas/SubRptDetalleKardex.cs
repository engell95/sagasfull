using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class SubRptDetalleKardex : XtraReport
    {
        private decimal _Litros = 0;
        private decimal _Saldo = 0;

        public SubRptDetalleKardex()
        {
            InitializeComponent();
            //this.xrCellList.DataBindings.Add("Text", null, "Nombre");
        }

        public SubRptDetalleKardex(string Codigo, string Nombre, object Procedure, string fecha, decimal Litros, decimal costo, decimal Saldo)
        {
            InitializeComponent();
            this.xrSetDate.Text = fecha;
            _Litros = Litros;
            this.xfCevLitros.Text = _Litros.ToString("#,0.000");
            this.xfCevCosto.Text = costo.ToString("#,0.0000");
            _Saldo = Saldo;//Decimal.Round(Litros * costo, 2, MidpointRounding.AwayFromZero);
            this.xfCevCostoTotal.Text = _Saldo.ToString("#,0.00");            
            this.xrProdCodigo.Text = Codigo;
            this.xrCeProdNombre.Text = Nombre;
            this.DataSource = Procedure;
        }

        private void xrTableCell28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {            
            //xrTableCell25.Text = Decimal.Round(_Saldo / 3.7854m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrTableCell26.Text = Convert.ToDecimal(_Saldo).ToString("#,0.000");
            //xrTableCell27.Text = Decimal.Round(_Saldo / 3.7854m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Litros += (String.IsNullOrEmpty(xrCeEntrada.Text) ? 0m : Convert.ToDecimal(xrCeEntrada.Text)) - (String.IsNullOrEmpty(xrCeSalida.Text) ? 0m : Convert.ToDecimal(xrCeSalida.Text));
            xrCeFinal.Text = Convert.ToDecimal(_Litros).ToString("#,0.000");
        }

        private void xrCevEntradas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //decimal valor = (String.IsNullOrEmpty(xrCeEntrada.Text) ? 0m : Convert.ToDecimal(xrCeEntrada.Text)) * Convert.ToDecimal(xrTableCell28.Text);

            //if (valor > 0)
            //    xrCevEntradas.Text = Convert.ToDecimal(valor).ToString("#,0.00");
            //else
            //    xrCevEntradas.Text = "";
        }

        private void xrCevSalidas_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //decimal valor = (String.IsNullOrEmpty(xrCeSalida.Text) ? 0m : Convert.ToDecimal(xrCeSalida.Text)) * Convert.ToDecimal(xrTableCell28.Text);

            //if (valor > 0)
            //    xrCevSalidas.Text = Convert.ToDecimal(valor).ToString("#,0.00");
            //else
            //    xrCevSalidas.Text = "";
        }

        private void xrCeSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _Saldo += (String.IsNullOrEmpty(xrCevEntradas.Text) ? 0m : Convert.ToDecimal(xrCevEntradas.Text)) - (String.IsNullOrEmpty(xrCevSalidas.Text) ? 0m : Convert.ToDecimal(xrCevSalidas.Text));
            xrCeSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }

        private void xrTableRow20_PreviewDoubleClick(object sender, PreviewMouseEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("2da Etapa");
        }

        private void xrCeTotalFinal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrCeTotalFinal.Text = Convert.ToDecimal(_Litros).ToString("#,0.000");
        }

        private void xrCeTotalSaldo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           xrCeTotalSaldo.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }


      

    }
}
