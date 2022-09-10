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
    public partial class DialogKeyGen : Form
    {
        #region >>> INICIO <<<
        private Entidad.SAGASDataClassesDataContext db;
        public string KeyNamePC = "";
        public string KeyBaseBoard = "";
        public string KeySerialDisk = "";
        public string KeyCPUActual = "";
        public string KeyLogicalDiskSerial = "";
        //public int KeyESID = 0;
        //public int KeyTipoConexionID = 0;
        public string KeyRegistro = "";
        public string KeyBD = "";
        private static readonly Random Random = new Random();
        private Entidad.Licencia Key;

        public int ESID
        {
            get { return Convert.ToInt32(lkeEstacionServicio.EditValue); }
            set { lkeEstacionServicio.EditValue = value; }
        }

        public int TipoConexionValue
        {
            get { return Convert.ToInt32(rgConexion.EditValue); }
            set { rgConexion.EditValue = value; }
        }

        public DialogKeyGen()
        {
            InitializeComponent();

        }

        #endregion     
        
        #region >>> METODOS <<<

        /// <summary>
        /// Generate Serial
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <param name="strongPassword"> </param>
        /// <returns></returns>
        private static string SerialGenerator(int SerialLength, bool strongSerial)
        {
            int seed = Random.Next(1, int.MaxValue);
            //const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string specialCharacters = @"!#$%&'()*+,-./:;<=>?@[\]_";

            var chars = new char[SerialLength];
            var rd = new Random(seed);

            for (var i = 0; i < SerialLength; i++)
            {
                // If we are to use special characters
                if (strongSerial && i % Random.Next(3, SerialLength) == 0)
                {
                    chars[i] = specialCharacters[rd.Next(0, specialCharacters.Length)];
                }
                else
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
            }

            return new string(chars);
        }
       
        #endregion

        #region >>> EVENTOS <<<
              
        private void DialogConexion_Load(object sender, EventArgs e)
        {
            
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                //**EstacionServicio
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.Properties.DisplayMember = "Nombre";
                lkeEstacionServicio.Properties.ValueMember = "ID";  
 
            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR",  MessageBoxButtons.OK,  MessageBoxIcon.Error); }
        
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Key.Equals(null))
            {
                try
                {
                    if (btnSerial.Text.Equals(txtSerialRegistro.Text) && Parametros.Security.Encrypt(txtSerialRegistro.Text).Equals(Key.SerialRegistro))
                    {
                        if (String.IsNullOrEmpty(Key.SerialBD))
                        {
                            Entidad.Licencia L = Key;
                            L.SerialBD = Parametros.Security.Encrypt(SerialGenerator(50, true));
                            db.SubmitChanges();
                            Parametros.General.DialogMsg("Se genero la licencia correctamente", MsgType.message);
                            this.Close();
                        }
                        else
                            Parametros.General.DialogMsg("El serial generado ya tiene una licencia asignada", MsgType.warning);
                    }
                    else
                        Parametros.General.DialogMsg("Favor cargue de nuevo el serial del registro!", MsgType.warning);
                }
                catch (Exception ex)
                { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
                Parametros.General.DialogMsg("Debe de cargar un registro!", MsgType.warning);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void btnSerial_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(btnSerial.Text))
            this.btnSerial.Text = "Digitar Serial del Registro";
        }

        private void btnSerial_Properties_Enter(object sender, EventArgs e)
        {
            this.btnSerial.SelectAll();
            this.btnSerial.Focus();
        }

        private void btnSerial_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!String.IsNullOrEmpty(btnSerial.Text))
            {
                try
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                    var Licencia = db.Licencias.Where(l => l.SerialRegistro.Equals(Parametros.Security.Encrypt(btnSerial.Text)));
                    if (Licencia.Count() > 0)
                    {
                        if (Licencia.Count().Equals(1))
                        {
                            //string RKeyRegistro = "", RNamePC = "", RBaseBoard = "", RSerialDisk = "", RCPUActual = "", RLogicalDiskSerial = "", RESID = "", RTipoConexionID = "";

                            //RNamePC = Config.GetValueByKeyAppSettings(Config.strVarNamePC);
                            //RBaseBoard = Config.GetValueByKeyAppSettings(Config.strVarBaseBoard);
                            //RSerialDisk = Config.GetValueByKeyAppSettings(Config.strVarSerialDisk);
                            //RCPUActual = Config.GetValueByKeyAppSettings(Config.strVarCPUActual);
                            //RLogicalDiskSerial = Config.GetValueByKeyAppSettings(Config.strLogicalDiskSerial);
                            //RESID = Config.GetValueByKeyAppSettings(Config.strVarESID);
                            //RTipoConexionID = Config.GetValueByKeyAppSettings(Config.strTipoConexionID);
                            //RKeyRegistro = Config.GetValueByKeyAppSettings(Config.strVarKeyRegistro);

                            Key = Licencia.First();

                            //if (Parametros.Security.Encrypt(RKeyRegistro).Equals(Key.SerialRegistro) && Parametros.Security.Encrypt(RNamePC).Equals(Key.NamePC) &&
                            //    Parametros.Security.Encrypt(RBaseBoard).Equals(Key.BaseBoard) && Parametros.Security.Encrypt(RSerialDisk).Equals(Key.SerialDisk) &&
                            //    Parametros.Security.Encrypt(RCPUActual).Equals(Key.CPUId) && Parametros.Security.Encrypt(RLogicalDiskSerial).Equals(Key.LogicalDiskSerial) &&
                            //    Parametros.Security.Encrypt(RESID).Equals(Key.EstacionServicio) && Parametros.Security.Encrypt(RTipoConexionID).Equals(Key.TipoConexion))
                            //{
                            txtNamePC.Text = Parametros.Security.Decrypt(Key.NamePC);
                            txtBaseBoard.Text = Parametros.Security.Decrypt(Key.BaseBoard);
                            txtSerialDisk.Text = Parametros.Security.Decrypt(Key.SerialDisk);
                            txtCPUId.Text = Parametros.Security.Decrypt(Key.CPUId);
                            txtSerialLogicDisk.Text = Parametros.Security.Decrypt(Key.LogicalDiskSerial);
                            lkeEstacionServicio.EditValue = Convert.ToInt32(Parametros.Security.Decrypt(Key.EstacionServicio));
                            rgConexion.EditValue = Convert.ToInt32(((TipoConexion)(Enum.Parse(typeof(TipoConexion), Parametros.Security.Decrypt(Key.TipoConexion)))));
                            txtSerialRegistro.Text = Parametros.Security.Decrypt(Key.SerialRegistro);
                            //}
                        }
                        else
                            Parametros.General.DialogMsg("Existen más de un registro con el Serial ingresado", MsgType.warning);
                    }
                    else
                        Parametros.General.DialogMsg("No existe ningun Registro con el Serial ingresado", MsgType.warning);
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, MsgType.error, ex.ToString());
                }
            }
            else
                Parametros.General.DialogMsg("Debe de digitar el serial del registro", MsgType.warning); 
        }
                  
    }
}