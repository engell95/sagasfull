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

namespace SAGAS.Tesoreria.Forms.Dialogs
{
    public partial class DialogFormato : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Entidad.FormatosCheque EntidadAnterior;
        internal bool Editable = false;
        private string route;
        private string reporteName = "";
        private string ExtensionName = "";
        private bool ShowMsg = false;
        internal Forms.FormFormato MDI;
       
        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }



        #endregion

        #region *** INICIO ***

        public DialogFormato()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl(); 
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               
                if (Editable)
                {
                    Nombre = EntidadAnterior.ArchivoNombre;
                    this.btnUpload.Text = EntidadAnterior.ArchivoNombre;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
             }

        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

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
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    Entidad.FormatosCheque FC;

                    if (Editable)
                    {
                        FC = db.FormatosCheques.Single(e => e.ID == EntidadAnterior.ID);
                        if (!String.IsNullOrEmpty(route))
                        {
                            FC.Archivo = File.ReadAllBytes(route);
                            FC.Fecha = Convert.ToDateTime(db.GetDateServer());
                            FC.ArchivoNombre = reporteName;
                            FC.Extension = ExtensionName;
                        }
                    }
                    else
                    {
                        FC = new Entidad.FormatosCheque();
                        FC.Archivo = File.ReadAllBytes(route);
                        FC.Fecha = Convert.ToDateTime(db.GetDateServer());
                        FC.ArchivoNombre = reporteName;
                        FC.Extension = ExtensionName;

                        db.FormatosCheques.InsertOnSubmit(FC);
                    }
                    
                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
            MDI.CleanDialog(ShowMsg);
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
                    this.reporteName = Nombre = Path.GetFileNameWithoutExtension(@ofdReport.FileName);
                    this.ExtensionName = Path.GetExtension(@ofdReport.FileName);
                    route = @ofdReport.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("INTERRUPCION AL CARGAR EL ARCHIVO" + ex.Message);
            }
        }

        #endregion


    }
}