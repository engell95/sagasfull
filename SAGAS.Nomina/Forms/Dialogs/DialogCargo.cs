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
    public partial class DialogCargo : Form
    {
        #region *** DECLARACIONES ***

        public Entidad.Cargo EtAnterior;
        internal FormCargo MDI;
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

        public int Orden
        {
            get { return Convert.ToInt32(spOrden.Value); }
            set { spOrden.Value = value; }
        }

        public bool EsBombero
        {
            get { return Convert.ToBoolean(EsBomberoCheckEdit.Checked); }
            set { EsBomberoCheckEdit.Checked = value; }
        } 

        #endregion

        #region *** INICIO ***

        public DialogCargo(int UserID)
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
            //_EtAnterior = new Mapeo.Cargo();
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            
            if(Editable)
                cargoBindingSource.DataSource = EtAnterior;
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
                    Entidad.Cargo C;

                    if (Editable)
                    { C = db.Cargo.Single(e => e.ID == EtAnterior.ID); }
                    else 
                    {
                        C = new Entidad.Cargo();
                        C.Activo = true;
                    }

                    C.Nombre = Nombre;
                    C.Descripcion = Descripcion;
                    C.Orden = Orden;
                    C.EsBombero = EsBombero;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(C, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EtAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó el Cargo: " + EtAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Cargo.InsertOnSubmit(C);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó el Cargo: " + C.Nombre, this.Name);

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

        #endregion        

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }
    }
}