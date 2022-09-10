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
using SAGAS.Contabilidad.Forms;
using DevExpress.XtraEditors.Popup;

namespace SAGAS.Tesoreria.Forms.Dialogs
{
    public partial class DialogFormatoBanco : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormFormatoBanco MDI;
        internal Entidad.FormatosCheque EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;
        private int UsuarioID = 0;
        private string route;
        private string chequeName = "";
        private string ExtensionName = "";
     
        #endregion

        #region *** INICIO ***

        public DialogFormatoBanco(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            //}

        }


        public bool ValidarCampos()
        {
            
            if (!Editable && String.IsNullOrEmpty(route))
            {
                Parametros.General.DialogMsg("Debe de seleccionar el archivo de REPORTE" + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Editable && !String.IsNullOrEmpty(route))
            {
                if (Parametros.General.DialogMsg("Un archivo de REPORTE fue seleccionado, ¿Desea reemplazarlo?" + Environment.NewLine, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.Cancel)
                    return false;
            }
            return true;
        }
        
        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.FormatosCheque cuenta;

                    if (Editable)
                    {
                        cuenta = db.FormatosCheques.Single(e => e.ID.Equals(EntidadAnterior.ID));

                        if (!String.IsNullOrEmpty(route))
                        {
                            cuenta.Archivo = File.ReadAllBytes(route);
                            cuenta.Fecha = Convert.ToDateTime(db.GetDateServer());
                            cuenta.ArchivoNombre = chequeName;
                            cuenta.Extension = ExtensionName;
                        }
                    }
                    else 
                    {
                        cuenta = new Entidad.FormatosCheque();
                        cuenta.Archivo = File.ReadAllBytes(route);
                        cuenta.Fecha = Convert.ToDateTime(db.GetDateServer());
                        cuenta.ArchivoNombre = chequeName;
                        cuenta.Extension = ExtensionName;
                    }

                    if (!Editable)
                        db.FormatosCheques.InsertOnSubmit(cuenta);

                    db.SubmitChanges();
                    
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
            if (!Guardar()) return;

            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, false, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //ofdReport.Filter = "Reportes |*.repx";

                if (ofdReport.ShowDialog() == DialogResult.OK)
                {
                    this.btnUpload.Text = Path.GetFileName(@ofdReport.FileName);
                    this.chequeName = this.txtNombre.Text = Path.GetFileNameWithoutExtension(@ofdReport.FileName);
                    this.ExtensionName = this.txtSiglas.Text = Path.GetExtension(@ofdReport.FileName);
                    route = @ofdReport.FileName;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "ERROR AL CARGAR EL ARCHIVO" + ex.Message);
            }
        }
        
        #endregion

    }
    
}