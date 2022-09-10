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
using SAGAS.Inventario.Forms;
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogComentario : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Comentario
        {
            get { return memoObs.Text; }
            set { memoObs.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogComentario()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //FillControl();
        }

        #endregion

        #region *** METODOS ***

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