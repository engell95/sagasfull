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
using SAGAS.Arqueo.Forms;
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogSello : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int _ID = 0;

        public string Anterior
        {
            get { return txtAnterior.Text; }
            set { txtAnterior.Text = value; }
        }

        private string Nuevo
        {
            get { return txtNuevo.Text; }
            set { txtNuevo.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogSello()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //FillControl();
        }

        #endregion

        #region *** METODOS ***
                     

        private bool Guardar()
        {           
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.Dispensador D = db.Dispensadors.Single(d => d.ID.Equals(_ID));
                    D.Sello = Nuevo;

                    db.SubmitChanges();
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se cambió el sello: " + D.Nombre, this.Name);
                    
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
            if (!String.IsNullOrEmpty(Nuevo))
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                this.Guardar();
                this.Close();
            }
            else
                return;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion        


    }


}