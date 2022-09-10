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
    public partial class DialogLicencia : Form
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

        public DialogLicencia()
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

        private void btnOk_Click(object sender, EventArgs e)
        {

            if (!lkeEstacionServicio.EditValue.Equals(null) && Convert.ToInt32(rgConexion.EditValue) >= 0)
            {
                try
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    string route = "";
                    saveFileDialogKey.Filter = "Archivo Licencia|*.txt";

                    if (saveFileDialogKey.ShowDialog() == DialogResult.OK)
                    {
                        Config.SetValueByKeyAppSettings(Config.strVarNamePC, KeyNamePC);
                        Config.SetValueByKeyAppSettings(Config.strVarBaseBoard, KeyBaseBoard);
                        Config.SetValueByKeyAppSettings(Config.strVarSerialDisk, KeySerialDisk);
                        Config.SetValueByKeyAppSettings(Config.strVarCPUActual, KeyCPUActual);
                        Config.SetValueByKeyAppSettings(Config.strLogicalDiskSerial, KeyLogicalDiskSerial);
                        Config.SetValueByKeyAppSettings(Config.strVarESID, this.ESID.ToString());
                        Config.SetValueByKeyAppSettings(Config.strTipoConexionID, Enum.GetName(typeof(TipoConexion), rgConexion.EditValue));
                        Config.SetValueByKeyAppSettings(Config.strVarKeyRegistro, KeyRegistro);

                        var Licencia = db.Licencias.Where(l => l.NamePC.Equals(Parametros.Security.Encrypt(KeyNamePC)) && l.BaseBoard.Equals(Parametros.Security.Encrypt(KeyBaseBoard)) && l.SerialDisk.Equals(Parametros.Security.Encrypt(KeySerialDisk)) && l.CPUId.Equals(Parametros.Security.Encrypt(KeyCPUActual)) && l.LogicalDiskSerial.Equals(Parametros.Security.Encrypt(KeyLogicalDiskSerial)));
                        Entidad.Licencia Key;
                        if (Licencia.Count() > 0)
                        {   
                            Key = Licencia.First();

                            if (!String.IsNullOrEmpty(Key.SerialBD))
                            {
                                Parametros.General.DialogMsg("Esta computadora ya tiene un serial registrado", MsgType.warning);
                                return;
                            }

                            Key.NamePC = Parametros.Security.Encrypt(KeyNamePC);
                            Key.BaseBoard = Parametros.Security.Encrypt(KeyBaseBoard);
                            Key.SerialDisk = Parametros.Security.Encrypt(KeySerialDisk);
                            Key.CPUId = Parametros.Security.Encrypt(KeyCPUActual);
                            Key.LogicalDiskSerial = Parametros.Security.Encrypt(KeyLogicalDiskSerial);
                            Key.EstacionServicio = Parametros.Security.Encrypt(this.ESID.ToString());
                            Key.TipoConexion = Parametros.Security.Encrypt(Enum.GetName(typeof(TipoConexion), rgConexion.EditValue));
                            Key.SerialRegistro = Parametros.Security.Encrypt(KeyRegistro);
                        }
                        else
                        {
                            Key = new Entidad.Licencia();
                            Key.NamePC = Parametros.Security.Encrypt(KeyNamePC);
                            Key.BaseBoard = Parametros.Security.Encrypt(KeyBaseBoard);
                            Key.SerialDisk = Parametros.Security.Encrypt(KeySerialDisk);
                            Key.CPUId = Parametros.Security.Encrypt(KeyCPUActual);
                            Key.LogicalDiskSerial = Parametros.Security.Encrypt(KeyLogicalDiskSerial);
                            Key.EstacionServicio = Parametros.Security.Encrypt(this.ESID.ToString());
                            Key.TipoConexion = Parametros.Security.Encrypt(Enum.GetName(typeof(TipoConexion), rgConexion.EditValue));
                            Key.SerialRegistro = Parametros.Security.Encrypt(KeyRegistro);
                            db.Licencias.InsertOnSubmit(Key);
                        }
                        db.SubmitChanges();
                        route = saveFileDialogKey.FileName.Replace("\\", "/");

                        using (StreamWriter sw = File.CreateText(route))
                        {
                            sw.WriteLine("Computadora");
                            sw.WriteLine(this.KeyNamePC);
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("PlacaBase");
                            sw.WriteLine(this.KeyBaseBoard);
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("SerialDisco");
                            sw.WriteLine(this.KeySerialDisk);
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("CPU");
                            sw.WriteLine(this.KeyCPUActual);
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("LogicalDiskSerial");
                            sw.WriteLine(this.KeyLogicalDiskSerial);
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("EstacionServicio");
                            sw.WriteLine(this.ESID.ToString());
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("TipoConexion");
                            sw.WriteLine(Enum.GetName(typeof(TipoConexion), rgConexion.EditValue));
                            sw.WriteLine("---------******************-------");
                            sw.WriteLine("SerialRegistro");
                            sw.WriteLine(this.KeyRegistro);
                        }

                        Parametros.General.DialogMsg("El registro se guardo correctamente en su computadora", MsgType.message);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se creó el archivo de Registro para la Licencia", this.Name);
                        this.Close();
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, MsgType.error, ex.ToString());
                }

            }
            else
                Parametros.General.DialogMsg("Debe de escoger la Estación de Servicio y el tipo de conexión.", MsgType.warning);
               
        }

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

                if (String.IsNullOrEmpty(this.KeyRegistro))
                    KeyRegistro = SerialGenerator(30, false);

                this.txtSerialRegistro.Text = this.KeyRegistro;   
                 
            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR",  MessageBoxButtons.OK,  MessageBoxIcon.Error); }
        
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                        
                var Licencia = db.Licencias.Where(l => l.NamePC.Equals(Parametros.Security.Encrypt(KeyNamePC)) && l.BaseBoard.Equals(Parametros.Security.Encrypt(KeyBaseBoard))
                    && l.SerialDisk.Equals(Parametros.Security.Encrypt(KeySerialDisk)) && l.CPUId.Equals(Parametros.Security.Encrypt(KeyCPUActual)) && l.SerialRegistro.Equals(Parametros.Security.Encrypt(KeyRegistro))
                    && l.LogicalDiskSerial.Equals(Parametros.Security.Encrypt(KeyLogicalDiskSerial)) && l.EstacionServicio.Equals(Parametros.Security.Encrypt(ESID.ToString())));

                if (Licencia.Count() > 0)
                {
                    if (Licencia.Count().Equals(1))
                    {
                        Entidad.Licencia Key = Licencia.First();

                        if (!String.IsNullOrEmpty(Key.SerialBD))
                        {
                            if (Key.SerialBD.Equals(Parametros.Security.Encrypt(txtLicencia.Text)))
                            {
                                Config.SetValueByKeyAppSettings(Config.strKeyBD, txtLicencia.Text);
                                Parametros.General.DialogMsg("El sistema fue licenciado correctamente.", MsgType.message);
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "El serial " + KeyRegistro + " fue licenciado.", this.Name);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                                Parametros.General.DialogMsg("La licencia no coincide con el serial registrado.", MsgType.warning);
                        }
                        else
                            Parametros.General.DialogMsg("No hay licencia registrada para este serial.", MsgType.warning);
                    }
                    else
                        Parametros.General.DialogMsg("Existen mas de 1 un serial registrado.", MsgType.warning);
                }
                else
                    Parametros.General.DialogMsg("El serial no esta registrada!, debe de generar el serial", MsgType.warning);    
            }
            catch (Exception ex)
            { MessageBox.Show(SAGAS.Parametros.Properties.Resources.MSGERROR + Environment.NewLine + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
           
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

          
    }
}