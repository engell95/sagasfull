using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class RptKardexCierre : DevExpress.XtraReports.UI.XtraReport
    {
        public RptKardexCierre()
        {
            InitializeComponent();
        }

        private void RptKardex_DataSourceDemanded(object sender, EventArgs e)
        {
            
        }

        private void RptKardex_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {

        }
              
    }
}
