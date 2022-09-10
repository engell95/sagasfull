using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace SAGAS
{
    public partial class DialogLogIn : Form
    {
        #region *** INICIO ***

        private int cont;
        private string UserRegistered = Parametros.Config.GetValueByKeyAppSettings(Parametros.Config.strUserRegistered);
        public static SAGAS.Parametros.FuncAnimationForm animation_form = new SAGAS.Parametros.FuncAnimationForm();

        public DialogLogIn()
        {
            InitializeComponent();
            animation_form.OpacityTimer(this, true);
        }

        #endregion

        #region *** EVENTOS ***         

        private void WarningOut(int c)
        {
            if (c == 2)
            {
                this.lblWarning.Visible = true;
                this.lblWarning.Text = "Último intento para ingresar al sistema!";
            }
            if (c == 3)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Parametros.Config.TestConnection())
            {          
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                try
                {
                    //var uss = db.Usuarios.Single(s => s.Login.Equals("hjerez"))
                    cont++;
                    WarningOut(cont);
                    var user = db.Usuarios.Where(u => u.Login.ToLower() == txtUser.Text.ToLower());
                    if (user.Count() > 0)
                    {
                        Entidad.Usuario usuario = user.First();
                        if (usuario.Contrasena == Parametros.Security.Encrypt(txtPass.Text, Parametros.Config.MagicWord))
                        {
                            Parametros.General.UserID = usuario.ID;
                            Parametros.General.UserName = usuario.Nombre;
                            Parametros.General.EstacionServicioID = usuario.EstacionServicioID;
                            Parametros.General.SubEstacionID = usuario.SubEstacionID;
                            Parametros.General.EmpresaID = usuario.EmpresaID;

                            Parametros.Config.SetValueByKeyAppSettings(Parametros.Config.strUserRegistered, usuario.Login);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;

                        }
                        else
                        {
                            Parametros.General.DialogMsg("Usuario o Contraseña invalida" + Environment.NewLine, Parametros.MsgType.warning);
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Usuario o Contraseña invalida" + Environment.NewLine, Parametros.MsgType.warning);
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

                }
            }
            else
                btnConexion.Visible = true;
                
        }

        private void btnConexion_Click(object sender, EventArgs e)
        {


            Parametros.Forms.Dialogs.DialogConexion dg = new Parametros.Forms.Dialogs.DialogConexion();

            dg.ShowDialog();

        }

        #endregion

        private void DialogLogIn_Load(object sender, EventArgs e)
        {
           //btnConexion.Visible = true;        

            txtUser.Text = UserRegistered;
            if (!String.IsNullOrEmpty(txtUser.Text))
                txtPass.Select();

            //txtPass.Text = "21240";
            //btnOk_Click(sender, e);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            //try
            //{
            //    Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            //    var watch = Stopwatch.StartNew();

            //    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(LoadDB));
            //    thread.Start();

            //    watch.Stop();
            //    var elapsedMs = watch.Elapsed;
            //    MessageBox.Show(elapsedMs.ToString());


            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //}
        }

        private void LoadDB()
        {
            try
            {
                FileInfo file = new FileInfo(@System.IO.Directory.GetCurrentDirectory() + @"/SQLCosteo.sql");
                string script = file.OpenText().ReadToEnd();
                SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString());
                Server server = new Server(new ServerConnection(conn));
                server.ConnectionContext.ExecuteNonQuery(script);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk.PerformClick();
            }
        }

        private void PicLogin_MouseDown(object sender, MouseEventArgs e)
        {
            SAGAS.Parametros.FuncAnimationForm.ReleaseCapture();
            SAGAS.Parametros.FuncAnimationForm.Position(this.Handle, 0x112, 0xf012, 0);
        }
        
    }
}
