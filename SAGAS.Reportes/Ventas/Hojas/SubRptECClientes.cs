using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Ventas.Hojas
{
    public partial class SubRptECClientes : XtraReport
    {
        private decimal _Saldo = 0;
        private decimal vTotal = 0;

        public SubRptECClientes(DateTime vFecha)
        {
            InitializeComponent();
            this.Parameters["pFecha"].Value = vFecha;
        }

        public SubRptECClientes(DateTime vFecha, DateTime uFecha, decimal Saldo, string texto, object Procedure, bool EsEmpleado)
        {
            InitializeComponent();
            _Saldo = Saldo;
            XrCeTitulo.Text = (EsEmpleado ? "DOCUMENTOS" : "FACTURAS") + " / NOTAS " + texto + " EL  : " + uFecha.ToShortDateString();
            this.Parameters["pFecha"].Value = vFecha;
            this.DataSource = Procedure;
            this.XrCeTotal.Text = "TOTAL ANTIGÜEDAD   :";
        }
        
        private void xrTableCell22_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            vTotal = Convert.ToDecimal((String.IsNullOrEmpty(xrTableCell22.Text) ? "0" : xrTableCell22.Text));            
        }

        private void XrCeComentario_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            this.XrCeComentario.Text = "Hacemos constar, que de parte de " + this.CeEstacion.Text + ", hemos recibido a entera satisfacción Facturas de combustible " +
                             "con el volumen arriba detallado y por lo tanto estamos de acuerdo con los datos referidos y nos comprometemos a cancelar la cantidad de " + this.xrCeMoneda.Text +
                             "  " + vTotal.ToString("#,#.00") + " (" + Parametros.General.enletras(vTotal.ToString()) + "), una vez recibido el  presente documento, de lo contrario estará sujeto a recargo por mora.";
        }



      

    }
}
