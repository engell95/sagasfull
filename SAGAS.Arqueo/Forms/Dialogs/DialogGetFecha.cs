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

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogGetFecha : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        public DateTime FechaMinima;
        public DateTime FechaMaxima;
        public bool IsReadOnly = false;
        private bool IsSusOnly = false;

        public DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }
         
        #endregion

        #region *** INICIO ***

        public DialogGetFecha(DateTime Fmin, DateTime Fmax)
        {
            InitializeComponent();
            FechaMaxima = Fmax;
            FechaMinima = Fmin;

        }

        public DialogGetFecha(bool SUSOnly)
        {
            InitializeComponent();
            IsSusOnly = SUSOnly;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                this.dateFecha.Properties.MinValue = FechaMinima;
                this.dateFecha.Properties.MaxValue = FechaMaxima;

                //**SubEstación
                lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                lkSES.Properties.DisplayMember = "Nombre";
                lkSES.Properties.ValueMember = "ID";

                if (Convert.ToInt32(db.ResumenDias.Count(r => r.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && r.SubEstacionID.Equals(IDSubEstacion))) > 0)
                {
                    Fecha = Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1);
                    this.dateFecha.Properties.ReadOnly = true;
                }

                if (Parametros.General.ListSES.Count > 0)
                {
                    this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    if (Parametros.General.ListSES.Count.Equals(1))
                    {
                        IDSubEstacion = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                        lkSES.Properties.ReadOnly = true;
                    }
                }

                if (IsReadOnly)
                {
                    lkSES.Properties.ReadOnly = true;
                }

                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** METODOS ***

        
        private bool Guardar()
        {
            try
            {
                if (!IsSusOnly)
                {
                    if (Convert.ToDateTime(dateFecha.EditValue) < FechaMinima || Convert.ToDateTime(dateFecha.EditValue) > FechaMaxima)
                    {
                        Parametros.General.DialogMsg("La fecha debe de estar dentro del rango", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (Parametros.General.ListSES.Count > 0 && lkSES.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe de seleccionar una Sub Estación", Parametros.MsgType.warning);
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