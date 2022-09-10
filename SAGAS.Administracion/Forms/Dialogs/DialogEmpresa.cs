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
    public partial class DialogEmpresa : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEmpresa MDI;
        internal Entidad.Empresa EntidadAnterior;
        private bool ShowMsg = false;
               
        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string NumeroRuc
        {
            get { return txtNumeroRuc.Text; }
            set { txtNumeroRuc.Text = value; }
        }

        private string Telefono
        {
            get { return txtTelefono.Text; }
            set { txtTelefono.Text = value; }
        }

        private string Direccion
        {
            get { return mmoDireccion.Text; }
            set { mmoDireccion.Text = value; }
        }

        private Byte[] Logo
        {
            get
            {
                if (picLogo.Image == null) return null;
                else return Parametros.General.ImageToBytes(picLogo.Image);
            }
            set
            {
                if (value == null) picLogo.Image = null;
                else picLogo.Image = Parametros.General.BytesToImage(value);
            }
        }
        #endregion

        #region *** INICIO ***

        public DialogEmpresa()
        {
            InitializeComponent();
                        
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
                Nombre = EntidadAnterior.Nombre;
                NumeroRuc = EntidadAnterior.NumeroRuc;
                Telefono = EntidadAnterior.Telefono;
                Direccion = EntidadAnterior.Direccion;
                Logo = EntidadAnterior.imagen;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
        }

        private bool ValidarCampos()
        {
            if (txtNombre.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
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
                        Entidad.Empresa Emp = db.Empresas.Single(e => e.ID == EntidadAnterior.ID);

                        Emp.Nombre = Nombre;
                        Emp.NumeroRuc = NumeroRuc;
                        Emp.Telefono = Telefono;
                        Emp.Direccion = Direccion;
                        Emp.imagen = Logo;
                        db.SubmitChanges();

                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(Emp, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó la empresa: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);
                                   
                        trans.Commit();

                        ShowMsg = true;
                        return true;
                    }

                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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

            this.Close();
         
        }
        
        private void DialogEmpresa_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {            
           this.Close();
        }

        #endregion        

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((TextEdit)sender, errRequiredField);
        }

      
    }
}