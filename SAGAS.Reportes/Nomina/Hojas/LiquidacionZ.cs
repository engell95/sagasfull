using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Nomina.Hojas
{
    public partial class LiquidacionZ : DevExpress.XtraReports.UI.XtraReport
    {
        public Image Logo
        {
            set { pbLogo.Image = value; }
        }

        public LiquidacionZ()
        {
            InitializeComponent();
        }

    }
}
