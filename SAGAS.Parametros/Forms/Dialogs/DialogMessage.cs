using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SAGAS.Parametros;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Linq;

namespace SAGAS.Parametros.Forms.Dialogs
{
    /// <summary>
    /// AUTOR: Victor Bonilla
    /// </summary>
    /// 
    public partial class DialogMessage : Form
    {
        private MsgType messageType;
        private string message;
        private string moreInfo;

        public DialogMessage(MsgType tipoMensaje, string mensaje, string masInformacion)
        {
            InitializeComponent();
            messageType = tipoMensaje;
            message = mensaje;
            moreInfo = masInformacion;
            this.BringToFront();
            this.TopMost = true;
        }

        public DialogMessage(MsgType tipoMensaje, string mensaje)
        {
            InitializeComponent();
            messageType = tipoMensaje;
            message = mensaje;
            moreInfo = string.Empty;
            this.BringToFront();
            this.TopMost = true;
        }

          /*
        private void SendMail(String Error)
        {
            try
            {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                //Classes.GetVol gv = new Classes.GetVol();
            msg.To.Add("susistemaideal@gmail.com");
            msg.From = new MailAddress("soportesusistemaideal@gmail.com", "Soporte Susistemaideal", System.Text.Encoding.UTF8);
            msg.Subject = "Error Sistema AdmonAc";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body =  //Convert.ToString(gv.GetVolumeSerial("C")) + Environment.NewLine +
                        //Convert.ToString(Classes.LicensesManager.GetCPUId()) + Environment.NewLine +
                        //"Nombre de la Computadora: " + Convert.ToString(Classes.LicensesManager.GetNamePC()) + Environment.NewLine +
                        "Local: " + EmpresaName() + Environment.NewLine +
                        Convert.ToString(DateTime.Now) +
                        "Error: " + Error;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = false;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("soportesusistemaideal@gmail.com", "c0ntr@s3n@");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            
                client.Send(msg);
            }
            catch
            {
                
            }
        }
           * 
           */


        /*
        private static string EmpresaName()
        {
            try
            {
                REGISTRO.Entity.DataClassesDataContext db = new REGISTRO.Entity.DataClassesDataContext(Clases.Config.GetCadenaConexionString());
                if (db.MyUnis.Count() > 0)
                    return Convert.ToString(db.MyUnis.First().UName);
                else
                    return "N/A";
            }
            catch { return "N/A"; }
            
        }
         * 
         */

         
        private void DialogMessage_Load(object sender, EventArgs e)
        {
            try
            {                      

                lblMensaje.Text = message;
                this.Height = 233;
                switch (messageType)
                {
                    case MsgType.message:
                        {
                            messagePicture.EditValue = Properties.Resources.infoTitle;
                            btnCancel.Visible = false;
                            btnOK.Select();
                            break;
                        }
                    case MsgType.warning:
                        {
                            messagePicture.EditValue = Properties.Resources.warningTitle;
                            btnOK.Visible = false;
                            btnCancel.Text = "&Cerrar";
                            btnCancel.Select();
                            break;
                        }
                    case MsgType.question:
                        {
                            messagePicture.EditValue = Properties.Resources.questionTitle;
                            //btnNo.Visible = true;
                            btnOK.Size = new Size(55, 32);
                            btnOK.Text = "&Si";
                            //btnNo.Text = "&No";
                            btnCancel.Text = "&Cancelar";
                            btnCancel.Select();
                            break;
                        }
                    case MsgType.questionNO:
                        {
                            messagePicture.EditValue = Properties.Resources.questionTitle;
                            btnNo.Visible = true;
                            btnOK.Size = new Size(55, 32);
                            btnOK.Text = "&Si";
                            btnNo.Text = "&No";
                            btnCancel.Text = "&Cancelar";
                            btnCancel.Select();
                            break;
                        }
                    case MsgType.error:
                        {
                            messagePicture.EditValue = Properties.Resources.errorTitle;
                            btnCancel.Visible = false;
                            btnInfo.Visible = true;
                            btnOK.Select();
                            //REGISTRO.Clases.General.AddLogBook(new REGISTRO.Entity.DataClassesDataContext(REGISTRO.Clases.Config.GetCadenaConexionString())
                            //    , REGISTRO.Clases.General.TipoActividad.Error, "Interrupción del proceso en la Aplicación. " + moreInfo);
                            //SendMail(moreInfo);
                            break;
                        }
                }
                if (!String.IsNullOrEmpty(moreInfo))
                {
                    //this.Height = 381;
                    meMasInformacion.Text = moreInfo;
                }

                this.BringToFront();
                this.TopMost = true;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message + Environment.NewLine + "ERROR GRAVE DEL SISTEMA POR FAVOR COMUNIQUESE CON SOPORTE TÉCNICO", "ERROR GRAVE DEL SISTEMA", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (this.Height <= 234)
            {
                this.Height = 381;
                this.btnInfo.Text = "Menos Información";

            }
            else if (this.Height >= 234)
            {
                this.Height = 233;
                this.btnInfo.Text = "Más Información";
            }
        }

       
    }
}
