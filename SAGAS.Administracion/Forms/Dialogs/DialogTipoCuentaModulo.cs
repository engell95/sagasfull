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
using SAGAS.Administracion.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogTipoCuentaModulo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTipoCuentaMovimiento MDI;
        internal List<Parametros.ListIdDisplayValueBooleano> EtCuentas;
        internal int _ID;
        internal bool Editable = true;
        private bool ShowMsg = false;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        
         
        #endregion

        #region *** INICIO ***

        public DialogTipoCuentaModulo()
        {
            InitializeComponent();
            
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                EtCuentas = (from tcm in db.TipoCuentas
                            where tcm.Activo
                            select new Parametros.ListIdDisplayValueBooleano
                            {
                                ID = tcm.ID,
                                Display = tcm.Nombre,
                                Booleano = !tcm.Activo
                            }).ToList();

                List<int> Cuentas = new List<int>();
                Cuentas = (from tcm in db.TipoCuentaModulo
                           where tcm.MovimientoTipoID.Equals(_ID)
                           select tcm.TipoCuentaID
                ).ToList();

                EtCuentas.ForEach(det => det.Booleano = (Cuentas.Contains(det.ID) ? true : false));

                bdsManejador.DataSource = EtCuentas;
                //this.gridTipoCuentaModulo.DataSource = EtCuentas;
                this.gridTipoCuentaModulo.DataSource = bdsManejador.DataSource;
              
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

            return true;
        }
        
        private bool Guardar() {
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    db.TipoCuentaModulo.DeleteAllOnSubmit(db.TipoCuentaModulo.Where(o => o.MovimientoTipoID.Equals(_ID )));
                    db.SubmitChanges();

                    EtCuentas.Where(o => o.Booleano).ToList().ForEach(det => db.TipoCuentaModulo.InsertOnSubmit(new Entidad.TipoCuentaModulo{ TipoCuentaID = det.ID, MovimientoTipoID = _ID }));

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
            
        }

       

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #endregion        


        

        
    }
}