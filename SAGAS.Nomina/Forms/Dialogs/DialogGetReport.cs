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
    public partial class DialogGetReport : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        public DateTime FechaMinima;
        public DateTime FechaMaxima;
        public bool IsReadOnly = false;
        private bool IsSusOnly = false;
                 
        #endregion

        #region *** INICIO ***

        public DialogGetReport()
        {
            InitializeComponent();

        }
        
        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
               

                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
           }
        }

        #endregion

        #region *** METODOS ***

        
        private bool Guardar()
        {
            try
            {
               
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
            {
                if (!Guardar()) return;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
         #endregion 

    }
}