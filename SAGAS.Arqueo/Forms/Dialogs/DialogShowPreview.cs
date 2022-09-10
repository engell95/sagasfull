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
    public partial class DialogShowPreview : Form
    {
        #region *** DECLARACIONES ***
        

        #endregion

        #region *** INICIO ***

        public DialogShowPreview()
        {
            InitializeComponent();

        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
            Entidad.ResumenDia RD = db.ResumenDias.Where(r => !r.Aprobado && r.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending(o => o.ID).First();
            int TurnoID = db.Turnos.Where(r => !r.Cerrada && r.ResumenDiaID.Equals(RD.ID)).OrderByDescending(o => o.ID).First().ID;
            Reportes.Util.PrintArqueoFast(this, dbView, db, 0, TurnoID, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTVISTAPREVIA, null);
            
        }

        #endregion

        #region *** METODOS ***

        
        private bool Guardar()
        {
            try
            {
                //if (Convert.ToDateTime(dateFecha.EditValue) < FechaMinima || Convert.ToDateTime(dateFecha.EditValue) > FechaMaxima)
                //{
                //    Parametros.General.DialogMsg("La fecha debe de estar dentro del rango", Parametros.MsgType.warning);
                //    return false;
                //}
                //else
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