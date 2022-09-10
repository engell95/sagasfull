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
    public partial class DialogAbonoCuota : Form
    {
        #region *** DECLARACIONES ***

        public Entidad.ProfesionOficio EtAnterior;
        internal FormProfesion MDI;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsg = false;
        internal bool Editable = false;
        private int UsuarioID;
        
        public DateTime Fecha
        {
            get { return dateFecha.DateTime.Date; }
            set { dateFecha.DateTime = value; }
        }

        public decimal Abono
        {
            get { return spAbono.Value; }
            set { spAbono.Value = value; }
        }
        
        #endregion
        
        #region *** INICIO ***

        public DialogAbonoCuota()
        {
            InitializeComponent();
            Fecha = DateTime.Now.Date;
        }


        #endregion

        #region *** METODOS ***


        public bool ValidarCampos()
        {
            if (spAbono.Value <= 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion        

    }
}