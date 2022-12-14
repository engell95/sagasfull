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
    public partial class DialogDepartamento : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormDepartamento MDI;
        internal Entidad.Departamento EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogDepartamento()
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

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    Comentario = EntidadAnterior.Comentario;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

             if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El c?digo del Departamento '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro c?digo.", Parametros.MsgType.warning);
                return false;
            }             

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Departamentos
                          where (ID.HasValue ? i.Codigo == code && i.ID != Convert.ToInt32(ID) : i.Codigo == code)
                          select i);

            if (result.Count() > 0)
            {
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
                    Entidad.Departamento D;

                    if (Editable)
                    { D = db.Departamentos.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        D = new Entidad.Departamento();
                        D.Activo = true;
                    }

                    D.Codigo = Codigo;
                    D.Nombre = Nombre;
                    D.Comentario = Comentario;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(D, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modific? el Departamento: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Departamentos.InsertOnSubmit(D);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se cre? el Departamento: " + D.Nombre, this.Name);

                    }

                    db.SubmitChanges();
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

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
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