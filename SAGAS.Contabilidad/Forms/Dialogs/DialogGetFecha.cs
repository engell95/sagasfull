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

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogGetFecha : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        public DateTime FechaMinima;
        public DateTime FechaMaxima;
        public bool IsReadOnly = false;
        private bool IsSusOnly = false;
        private bool FreeDate = false;

        public DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

         
        #endregion

        #region *** INICIO ***

        public DialogGetFecha(DateTime Fmin, DateTime Fmax)
        {
            InitializeComponent();
            FechaMaxima = Fmax;
            FechaMinima = Fmin;

        }
        
        public DialogGetFecha(bool Free)
        {
            InitializeComponent();
            FreeDate = Free;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                if (!FreeDate)
                {
                    this.dateFecha.Properties.MinValue = FechaMinima;
                    this.dateFecha.Properties.MaxValue = FechaMaxima;
                }
                else
                    Fecha = DateTime.Now;
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
                //if (!IsSusOnly)
                //{
                //    if (Convert.ToDateTime(dateFecha.EditValue) < FechaMinima || Convert.ToDateTime(dateFecha.EditValue) > FechaMaxima)
                //    {
                //        Parametros.General.DialogMsg("La fecha debe de estar dentro del rango", Parametros.MsgType.warning);
                //        return false;
                //    }
                //}

                //if (Parametros.General.ListSES.Count > 0 && lkSES.EditValue == null)
                //{
                //    Parametros.General.DialogMsg("Debe de seleccionar una Sub Estación", Parametros.MsgType.warning);
                //    return false;

                //}

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