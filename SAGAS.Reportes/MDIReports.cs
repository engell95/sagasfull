using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Exports;
using DevExpress.XtraReports.UserDesigner;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes
{
    public partial class MDIReports : Form
    {
        public MDIReports()
        {
            InitializeComponent();
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ni");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ni");
        }
        
        private void xrTabbedMdiManagerMain_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            MessageBox.Show(e.Page.Text);
            //DevExpress.XtraReports.UI.XtraReport rep = (e.Page as DevExpress.XtraReports.UI.XtraReport)//.PopupWindow as MemoExPopupForm;
        }

        private void MDIReports_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'sAGASDataSet.Acceso' table. You can move, or remove it, as needed.

        }

        private void barConexion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DevExpress.XtraReports.UI.XtraReport report1 = new DevExpress.XtraReports.UI.XtraReport();
            //report1.CreateDocument();

            //report1.Pages = xrTabbedMdiManager1.Pages;
            //report1.DataSource = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            //this.xrTabbedMdiManager1.SelectedPage as DevExpress.XtraReports.UI.XtraReport
            //foreach (DevExpress.XtraReports.UI.XtraReport form in this.xrTabbedMdiManager1.Pages.)
            //{
            //    form.DataSource = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            //      //Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            //}

        }

           
    }
}
