using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptValoresVenta : XtraReport
    {
        public SubRptValoresVenta(string PrimerValor, string SegundoValor, string TercerValor, string CuartoValor)
        {
            InitializeComponent();
            this.xrCellExVenta.DataBindings.Add("Text", null, PrimerValor, "{0:n3}");
            this.xrCellPrecVenta.DataBindings.Add("Text", null, SegundoValor, "{0:n2}");
            this.xrCellDescuento.DataBindings.Add("Text", null, TercerValor, "{0:n2}");
            this.xrCellValorVenta.DataBindings.Add("Text", null, CuartoValor, "{0:n2}");            
        }
    }
}
