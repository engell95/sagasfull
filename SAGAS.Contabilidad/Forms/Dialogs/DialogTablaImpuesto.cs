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

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogTablaImpuesto : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTablaImpuesto MDI;
        internal Entidad.TablaImpuesto EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
                
        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int CCID
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        public decimal Porcentaje
        {
            get { return Convert.ToDecimal(this.spPorcentaje.Value); }
            set { this.spPorcentaje.Value = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogTablaImpuesto()
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

                glkCC.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                glkCC.Properties.DisplayMember = "Display";
                glkCC.Properties.ValueMember = "ID";

                if (Editable)
                {
                    CCID = EntidadAnterior.CuentaContableID;
                    Comentario = EntidadAnterior.Comentario;
                    Porcentaje = EntidadAnterior.Porcentaje;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkCC.EditValue) <= 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGSELECCIONARCC, Parametros.MsgType.warning);
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
                    Entidad.TablaImpuesto TI;

                    if (Editable)
                    { TI = db.TablaImpuestos.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        TI = new Entidad.TablaImpuesto();
                        TI.Activo = true;
                    }

                    TI.CuentaContableID = CCID;
                    TI.Comentario = Comentario;
                    TI.Porcentaje = Porcentaje;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(TI, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó el Impuesto: " + db.CuentaContables.Single(t => t.ID.Equals(EntidadAnterior.CuentaContableID)).Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.TablaImpuestos.InsertOnSubmit(TI);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Impuesto: " + db.CuentaContables.Single(t => t.ID.Equals(EntidadAnterior.CuentaContableID)).Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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

        #endregion        

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((TextEdit)sender, errRequiredField);
        }
        
    }
}