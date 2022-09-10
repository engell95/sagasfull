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
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Tesoreria.Forms.Dialogs
{
    public partial class DialogPagoImpresion : Form
    {
        #region *** DECLARACIONES ***

        #endregion

        #region *** INICIO ***

        public DialogPagoImpresion()
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
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #endregion        


    }


}