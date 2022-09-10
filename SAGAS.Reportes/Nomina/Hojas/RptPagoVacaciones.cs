using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Nomina.Hojas
{
    public partial class RptPagoVacaciones : DevExpress.XtraReports.UI.XtraReport
    {

        public Image Logo
        {
            set { pbLogo.Image = value; }
        }


        public RptPagoVacaciones()
        {
            InitializeComponent();
        }

    }
}
