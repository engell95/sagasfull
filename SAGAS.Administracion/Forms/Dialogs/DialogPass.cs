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
using SAGAS.Administracion.Forms;

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogPass : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormUsuario MDI;
        private int UsuarioID;
        public bool EsCambioEstacion = false;
        public bool EsCambioContrasena = false;

        public string Anterior
        {
            get { return txtAnterior.Text; }
            set { txtAnterior.Text = value; }
        }

        public string Nueva
        {
            get { return txtNueva.Text; }
            set { txtNueva.Text = value; }
        }

        public string Confirmar
        {
            get { return txtConfirmar.Text; }
            set { txtConfirmar.Text = value; }
        }

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogPass(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               
                lkES.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Codigo, es.Nombre, Display = es.Codigo + " - " + es.Nombre };
                lkES.Properties.DisplayMember = "Display";
                lkES.Properties.ValueMember = "ID";


                if (Convert.ToInt32(db.Usuarios.Single(u => u.ID == UsuarioID).EstacionServicioID) > 0)
                    lkES.EditValue = Convert.ToInt32(db.Usuarios.Single(u => u.ID == UsuarioID).EstacionServicioID);

                if (EsCambioEstacion)
                {
                    layoutControlGroup.TextVisible = false;
                    layoutControlItemAnterior.Visibility  = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemConfirmar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemNueva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

                if (EsCambioContrasena)
                    layoutControlItem1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
               
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private void ValidarerrRequiredField()
        {
            if (!EsCambioEstacion)
            {
                Parametros.General.ValidateEmptyStringRule(txtAnterior, errRequiredField);
                Parametros.General.ValidateEmptyStringRule(txtNueva, errRequiredField);
                Parametros.General.ValidateEmptyStringRule(txtConfirmar, errRequiredField);
            }

            if (!EsCambioContrasena)
                Parametros.General.ValidateEmptyStringRule(lkES, errRequiredField);
        }

        public bool ValidarCampos()
        {
            try
            {
                if (((!EsCambioEstacion) && (txtAnterior.Text == "" || txtNueva.Text == "" || txtConfirmar.Text == "")) || (!EsCambioContrasena && Convert.ToInt32(lkES.EditValue) <= 0))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!EsCambioEstacion)
                {
                    if (txtNueva.Text.Length < 6)
                    {
                        Parametros.General.DialogMsg("La contraseña debe contener minimo 6 caracteres " + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }

                    if (txtAnterior.Text == Parametros.Security.Decrypt(Convert.ToString(db.Usuarios.Single(u => u.ID == UsuarioID).Contrasena), Parametros.Config.MagicWord))
                    {
                        if (txtNueva.Text != txtConfirmar.Text)
                        {
                            Parametros.General.DialogMsg("Confirme su nueva contraseña " + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Confirme contraseña anterior" + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.Usuario U;

                     U = db.Usuarios.Single(e => e.ID == UsuarioID);

                     if (!EsCambioEstacion)
                     {
                         U.Contrasena = Parametros.Security.Encrypt(txtNueva.Text, Parametros.Config.MagicWord);
                         Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                             "Se cambió la contraseña del usuario: " + U.Nombre, this.Name);
                     }

                     if (!EsCambioContrasena)
                     {
                         U.EstacionServicioID = IDEstacionServicio;
                         U.SubEstacionID = 0;
                         Parametros.General.EstacionServicioID = IDEstacionServicio;
                         Parametros.General.SubEstacionID = 0;
                         Parametros.General.EstacionServicioName = db.EstacionServicios.Single(es => es.ID == Parametros.General.EstacionServicioID).Nombre;
                         Parametros.General.GetSubEstaciones(Parametros.General.UserID, Parametros.General.EstacionServicioID);
                         Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                             "Se cambió la Estación de Servicio: " + U.Nombre, this.Name);
                     }
                     
                    U.IsReset = false;   
                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
         
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion        

        private void txtAnterior_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.BaseEdit)sender, errRequiredField);
        }       
        
    }
}