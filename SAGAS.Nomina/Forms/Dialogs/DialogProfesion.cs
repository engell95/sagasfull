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

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogProfesion : Form
    {
        #region *** DECLARACIONES ***

        public Entidad.ProfesionOficio EtAnterior;
        internal FormProfesion MDI;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsg = false;
        internal bool Editable = false;
        private int UsuarioID;
        
        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Descripcion
        {
            get { return mmoDescripcion.Text; }
            set { mmoDescripcion.Text = value; }
        }
        
        #endregion
        
        #region *** INICIO ***

        public DialogProfesion(int UserID)
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
            //_EtAnterior = new Mapeo.ProfesionOficio();
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            
            if (Editable)
                profesionOficioBindingSource.DataSource = EtAnterior;
        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoDescripcion, errRequiredField);
        }

        public bool ValidarCampos()
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
            if ( !ValidarCampos() ) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.ProfesionOficio PO;

                    if (Editable)
                    { PO = db.ProfesionOficio.Single(e => e.ID == EtAnterior.ID); }
                    else 
                    {
                        PO = new Entidad.ProfesionOficio();
                        PO.Activo = true;
                    }

                    PO.Nombre = Nombre;
                    PO.Descripcion = Descripcion;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PO, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EtAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó la Profesión: " + EtAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ProfesionOficio.InsertOnSubmit(PO);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó la Profesión: " + PO.Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
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
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }
    }
}