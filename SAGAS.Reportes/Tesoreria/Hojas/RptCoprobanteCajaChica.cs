﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Tesoreria.Hojas
{
    public partial class RptCoprobanteCajaChica : DevExpress.XtraReports.UI.XtraReport
    {
        public RptCoprobanteCajaChica()
        {
            InitializeComponent();
        }

        private void RptEntradaSalidaManejo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrTableCell9_AfterPrint(object sender, EventArgs e)
        {
            
        }
              
    }
}
