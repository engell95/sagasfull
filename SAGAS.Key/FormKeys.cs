using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using System.IO;

namespace SAGAS.Key
{
    public partial class FormKeys : Form
    {
        public FormKeys()
        {
            InitializeComponent();
        }

        private void btnConexion_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
                {
                    conn.ConnectionString = Properties.Resources.CnnConexion;
                    conn.Open();
                    conn.Close();
                    MessageBox.Show("Conexión Exitosa", "CONEXION", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (System.Data.SqlClient.SqlException except)
            {
                switch (except.Number)
                {
                    case 26:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 50:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 53:
                        MessageBox.Show("NO HAY CONEXION EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "Error de conexión a la base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Properties.Resources.CnnConexion);
                db.CommandTimeout = 9999;
                string route = "";
                openFileDialogUp.Filter = "ActualizadorSagas|*.exe";

                if (openFileDialogUp.ShowDialog() == DialogResult.OK)
                    {
                        route = Path.GetDirectoryName(@openFileDialogUp.FileName);
                        Entidad.Upload Up = new Entidad.Upload();
                        Up.Archivo = File.ReadAllBytes(route + @"\ActualizadorSagas.exe");
                    Up.Fecha = Convert.ToDateTime(db.GetDateServer());

                    db.Uploads.InsertOnSubmit(Up);
                    db.SubmitChanges();
                    MessageBox.Show("ARCHIVO SUBIDO EXITOSAMENTE");
 
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR AL CARGAR EL ARCHIVO" + ex.Message);
            }
        }
    }
}
