using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Nomina.Hojas
{
    public partial class SubRptECEmpleado : XtraReport
    {
        private decimal _Saldo = 0;

        public SubRptECEmpleado(DateTime vFecha)
        {
            InitializeComponent();
            this.Parameters["pFecha"].Value = vFecha;
        }
        
    }
}
