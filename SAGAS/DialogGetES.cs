using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;

namespace SAGAS
{
    public partial class DialogGetES : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        private int UsuarioID = Parametros.General.UserID;
        public Form _MDIParent = new Form();

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogGetES()
        {
            InitializeComponent();
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
        }
                
        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                IQueryable<Parametros.ListIdDisplay> listaES = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                               select new Parametros.ListIdDisplay { ID = es.ID, Display = es.Nombre };

                List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>(listaES);
                if (listaES.Count() >= db.EstacionServicios.Count(c => c.Activo))
                    listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Todas E/S"));

                lkES.Properties.DataSource = listadoDisplay;
                lkES.Properties.DisplayMember = "Display";
                lkES.Properties.ValueMember = "ID";
                lkES.EditValue = Parametros.General.EstacionServicioID;
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** METODOS ***

        public bool ValidarCampos()
        {
            try
            {
                if (lkES.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (this.layoutControlItemSES.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
                {
                    if (lkSES.EditValue == null)
                    {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                            return false;                        
                    }
                }

                return true;
            }
            catch
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
     
         #endregion 

        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkES.EditValue != null)
                {
                    
                    if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))) > 0)
                    {
                        this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                     
                        //**SubEstación

                        IQueryable<Parametros.ListIdDisplay> listaSES = from ses in db.SubEstacions
                                                                        where ses.Activo && ses.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))
                                                                       select new Parametros.ListIdDisplay { ID = ses.ID, Display = ses.Nombre };

                        List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>(listaSES);
                        listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Todas Sub Estaciones"));

                        lkSES.Properties.DataSource = listadoDisplay;
                        lkSES.Properties.DisplayMember = "Display";
                        lkSES.Properties.ValueMember = "ID";

                    }
                    else
                    {
                        this.IDSubEstacion = 0;
                        this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }       

    }
}