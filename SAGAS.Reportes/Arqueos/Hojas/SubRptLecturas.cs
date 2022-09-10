using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptLecturas : XtraReport
    {
        public SubRptLecturas(string PrimerValor, string SegundoValor, string TercerValor, bool OcultarTerceraFila, bool EsVerificacionEfectivo, bool EsVerificacionMecVsElectronica)
        {
            InitializeComponent();
            this.xrCellInicial.DataBindings.Add("Text", null, PrimerValor, "{0:n3}");
            this.xrCellFinal.DataBindings.Add("Text", null, SegundoValor, "{0:n3}");
            this.xrCellExtraxion.DataBindings.Add("Text", null, TercerValor, "{0:n3}");

            if (!EsVerificacionEfectivo)
                this.xrCellInicial.Dispose();

            if (!EsVerificacionMecVsElectronica)
                this.xrCellFinal.Dispose();

            if (OcultarTerceraFila)
                this.xrCellExtraxion.Dispose();
            
        }
    }
}
