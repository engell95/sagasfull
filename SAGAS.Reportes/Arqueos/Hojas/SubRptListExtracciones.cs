using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptListExtracciones : XtraReport
    {
        public SubRptListExtracciones()
        {
            InitializeComponent();
            this.xrCellList.DataBindings.Add("Text", null, "Nombre");
        }

        public SubRptListExtracciones(float Ancho)
        {
            InitializeComponent();
            this.xrTable3.WidthF = Ancho;
            this.xrTableFooter.WidthF = Ancho;
            this.xrCellList.DataBindings.Add("Text", null, "Nombre");
        }

      

    }
}
