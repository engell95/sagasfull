using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAGAS.Reportes
{
    public partial class FormReportSSRS : Form
    {
        private string ReportName { get; set; }
        //private string TextTitle { get; set; }
        private string ReportServerUrl { get; set; }
        private string ReportPath { get; set; }
        private string userNameCredential { get; set; }
        private string passwordCredential { get; set; }
        private string domainCredential { get; set; }

        public FormReportSSRS(string ReportName/*, string texttitle*/)
        {
            InitializeComponent();
            this.ReportName = ReportName;
            //this.TextTitle = texttitle;
            this.Text = this.ReportName/*TextTitle*/;
            this.ReportServerUrl = Parametros.General.ReportServerUrl;
            this.ReportPath = Parametros.General.ReportPath;
            this.userNameCredential = Parametros.General.userNameCredential;
            this.passwordCredential = Parametros.General.passwordCredential;
            this.domainCredential = Parametros.General.domainCredential;
        }

        private void FormReportSSRS_Load(object sender, EventArgs e)
        {
            if (this.ReportServerUrl == null)
            {
                Parametros.General.DialogMsg("No se ha configurado el URL del servidor de reportes.\nPóngase en contacto con el Administrador del sistema SAGAS.", Parametros.MsgType.warning);
                this.Close();
            }
            if (this.ReportPath == null)
            {
                Parametros.General.DialogMsg("No se ha configurado la ubicación del reporte.\nPóngase en contacto con el Administrador del sistema SAGAS.", Parametros.MsgType.warning);
                this.Close();
            }
            if (this.userNameCredential == null)
            {
                Parametros.General.DialogMsg("No se ha configurado la credencial: usuario de la base de datos del reporte.\nPóngase en contacto con el Administrador del sistema SAGAS.", Parametros.MsgType.warning);
                this.Close();
            }
            if (this.passwordCredential == null)
            {
                Parametros.General.DialogMsg("No se ha configurado la credencial: contraseña de la base de datos del reporte.\nPóngase en contacto con el Administrador del sistema SAGAS.", Parametros.MsgType.warning);
                this.Close();
            }
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            try
            {
                //string v = "";
                //foreach (var listUSE in db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Parametros.General.UserID).OrderBy(o => o.EstacionServicioID))
                //    v += listUSE.EstacionServicioID + "-";
                //MessageBox.Show("este user tiene: " + db.GetViewEstacionesServicioByUsers.Count(ges => ges.UsuarioID == Parametros.General.UserID) + " estaciones: " + v);// && ges.EstacionServicioID == Parametros.General.EstacionServicioID;
                reportViewer1.Width = 800;
                reportViewer1.Height = 600;
                reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
                reportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerUrl);//new Uri("http://desarrollobl-pc/ReportServer"); //new Uri("http://sagas/ReportServer");
                reportViewer1.ServerReport.ReportPath = ReportPath + ReportName;//"/Arqueo Pista/Faltantes-Sobrantes/02-Faltantes y Sobrantes (Resumen)";

                //MessageBox.Show("RSC is: " + reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials);
                System.Net.NetworkCredential myCred = new NetworkCredential(userNameCredential, passwordCredential/*"Administrador", "Sagas2016"||"DesarrolloBL", "2303"*/);
                reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
                //MessageBox.Show("Upgrade RSC is: " + myCred.Password + " " + myCred.UserName + reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials.GetCredential(new Uri("http://desarrollobl-pc/ReportServer"),""));

                //MessageBox.Show("DSC is: " + reportViewer1.ServerReport.GetDataSources().First().Name);//+ myCred.GetCredential(new Uri("http://desarrollobl-pc/ReportServer"),"Windows Authentication").ToString());
                Microsoft.Reporting.WinForms.DataSourceCredentials[] se = new Microsoft.Reporting.WinForms.DataSourceCredentials[1];//new List<Microsoft.Reporting.WinForms.DataSourceCredentials>();
                se[0] = new Microsoft.Reporting.WinForms.DataSourceCredentials() { Name = "SAGAS", Password = "Sagas2016" };//, UserId = Parametros.General.UserID.ToString() };
                //reportViewer1.ServerReport.SetDataSourceCredentials(se);

                /*IReportServerCredentials irsc = new CustomReportCredentials("Administrator", "MYpassworw", "");
                reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = (System.Net.ICredentials) irsc;*/

                //reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = ;

                //MessageBox.Show("DSC is: " + reportViewer1.ServerReport.GetDataSources().Count);
                reportViewer1.ServerReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("ParamUserID", Parametros.General.UserID.ToString(), false));
                reportViewer1.ServerReport.Refresh();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                return;
            }
        }
    }
}
