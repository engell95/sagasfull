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
    public partial class DialogAprobacionPago : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int _ID = 0;

        public DateTime Fecha
        {
            get { return datePago.DateTime; }
            set { datePago.DateTime = value; }
        }

        public string Comentario
        {
            get { return memo.Text; }
            set { memo.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogAprobacionPago()
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
            try
            {
                if (String.IsNullOrEmpty(Comentario))
                {
                    Parametros.General.DialogMsg("Debe digitar un comentario", Parametros.MsgType.warning);
                    return false;

                }

                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return false;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion        


    }


}