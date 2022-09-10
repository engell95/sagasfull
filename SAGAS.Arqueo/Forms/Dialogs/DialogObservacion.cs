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
    public partial class DialogObservacion : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Observacion
        {
            get { return memoObs.Text; }
            set { memoObs.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogObservacion()
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
                    Entidad.Color C;

                    C = new Entidad.Color();
                        C.Activo = true;

                    //C.Colored = Colored;
                   
                        db.Colors.InsertOnSubmit(C);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó el Color: " + C.Colored, this.Name);


                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
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
            if (!String.IsNullOrEmpty(memoObs.Text))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

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