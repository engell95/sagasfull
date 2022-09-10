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
    public partial class DialogTipoAusencia : Form
    {
        #region *** DECLARACIONES ***

        public Entidad.TipoAusencia EtAnterior;
        internal FormTipoAusencia MDI;
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

        public DialogTipoAusencia(int UserID)
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
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            if (Editable)
                profesionBindingSource.DataSource = EtAnterior;
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
                db.CommandTimeout = 120;
                try
                {
                    Entidad.TipoAusencia TA;

                    if (Editable)
                    { TA = db.TipoAusencia.Single(e => e.ID == EtAnterior.ID); }
                    else 
                    {
                        TA = new Entidad.TipoAusencia();
                        TA.Activo = true;
                    }

                    TA.Nombre = Nombre;
                    TA.Descripcion = Descripcion;
                                  
                    if (Editable)
                    {
                        DataTable _dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(TA, 1));
                        DataTable _dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EtAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó el Tipo de Ausencia: " + EtAnterior.Nombre, this.Name, _dtPosterior, _dtAnterior);

                    }
                    else
                    {
                        db.TipoAusencia.InsertOnSubmit(TA);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó el Tipo de Ausencia: " + TA.Nombre, this.Name);

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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #endregion

    
    }
}