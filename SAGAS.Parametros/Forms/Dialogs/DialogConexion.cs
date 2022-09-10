using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;

namespace SAGAS.Parametros.Forms.Dialogs
{
    public partial class DialogConexion : Form
    {
        #region >>> INICIO <<<
        
        public DialogConexion()
        {
            InitializeComponent();

        }

        #endregion     
        
        #region >>> METODOS <<<
        
         
        // Metodo para realizar prueba de conexion 
        //(bool): True indica que la conexion es correcta, y False si la conexion es erronea</returns>
        public bool TestConnection(string _stringconexion)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
                {
                    conn.ConnectionString = _stringconexion;
                    conn.Open();
                    conn.Close();

                    SAGAS.Parametros.General.DialogMsg("CONECCION REALIZADA EXITOSAMENTE.", SAGAS.Parametros.MsgType.message);  
                   
                    return true;
                }
            }
            catch (System.Data.SqlClient.SqlException except)
            {
                switch (except.Number)
                {
                    case 50:
                        MessageBox.Show("EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        break;
                    case 53:
                        MessageBox.Show("EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        break;
                    case 233:
                        MessageBox.Show("CONEXIÓN CON EL SERVIDOR ESTABLECIDA, PERO EL LOGIN ES INCORRECTO." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        break;
                    case 4060:
                        MessageBox.Show("NO SE PUEDE CONECTARSE A LA BASE DE DATOS INDICADA." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        break;
                    default:
                        MessageBox.Show("ERROR GENERAL DE LA CONEXION DE SQL!!!" + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       
                        break;
                }

                return false;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);       

                return false;
            }

        } 
        

        #endregion

        #region >>> EVENTOS <<<

        private void btnOk_Click(object sender, EventArgs e)
        {
            
                    string conexion_string = "Data Source=";

                    conexion_string += txtServerName.Text;
                    conexion_string += ";Initial Catalog=";
                    conexion_string += txtDataBaseName.Text;
                    conexion_string += "; User ID=";
                    conexion_string += txtDBUser.Text;
                    conexion_string += "; Password =";
                    conexion_string += txtDBPass.Text;

                    TestConnection(conexion_string);
               
        }

        private void DialogConexion_Load(object sender, EventArgs e)
        {
            
            try
            {
                string str_conn = SAGAS.Parametros.Config.GetCadenaConexionString();

                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
                csb.ConnectionString = str_conn;
                
                txtServerName.Text = csb.DataSource;
                txtDataBaseName.Text = csb.InitialCatalog;
                txtDBUser.Text = csb.UserID;
                txtDBPass.Text = csb.Password;
                 
            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR",  MessageBoxButtons.OK,  MessageBoxIcon.Error); }
        
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string conexion_string = "Data Source=";

                conexion_string += txtServerName.Text;
                conexion_string += ";Initial Catalog=";
                conexion_string += txtDataBaseName.Text;
                conexion_string += "; User ID=";
                conexion_string += txtDBUser.Text;
                conexion_string += "; Password =";
                conexion_string += txtDBPass.Text;

                if (!TestConnection(conexion_string)) return;

                Parametros.General.Conexion = conexion_string;
                SAGAS.Parametros.Config.SetCadenaConexionString(conexion_string);
                SAGAS.Parametros.General.DialogMsg(Properties.Resources.MSGCAMBIOSGUARDADOS, SAGAS.Parametros.MsgType.message); 

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

         
 
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

          
    }
}