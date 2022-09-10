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
using System.Windows.Input;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogConceptoContable : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormConceptosContables MDI;
        internal Entidad.ConceptoContable EntidadAnterior;
        private DataTable dtEstacionesServicios;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public int CC
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        private decimal Permisible
        {
            get { return Convert.ToDecimal(spMargen.Value); }
            set { spMargen.Value = value; }
        }

        public bool EsRoc
        {
            get { return Convert.ToBoolean(chkAplicaRoc.Checked); }
            set { chkAplicaRoc.Checked = value; }
        }

        public bool EsAutoconsumo
        {
            get { return Convert.ToBoolean(chkAutoconsumo.Checked); }
            set { chkAutoconsumo.Checked = value; }
        }

        public bool EsAjuste
        {
            get { return Convert.ToBoolean(chkEsAjuste.Checked); }
            set { chkEsAjuste.Checked = value; }
        }

        public bool EsCajaChica
        {
            get { return Convert.ToBoolean(chkCajaChica.Checked); }
            set { chkCajaChica.Checked = value; }
        }
                
        public bool EsMovInventario
        {
            get { return Convert.ToBoolean(chkMovInventario.Checked); }
            set { chkMovInventario.Checked = value; }
        }

        public bool EsCheque
        {
            get { return Convert.ToBoolean(chkCheque.Checked); }
            set { chkCheque.Checked = value; }
        }

        public bool EsDocumento
        {
            get { return Convert.ToBoolean(chkGeneraDocumento.Checked); }
            set { chkGeneraDocumento.Checked = value; }
        } 

        #endregion

        #region *** INICIO ***

        public DialogConceptoContable()
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

                var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                glkCC.Properties.DataSource = lista;
            
                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    CC = EntidadAnterior.CuentaContableID;
                    EsRoc = EntidadAnterior.EsRoc;
                    Permisible = EntidadAnterior.Permisible;
                    EsAutoconsumo = EntidadAnterior.EsAutoconsumo;
                    EsAjuste = EntidadAnterior.EsAjuste;
                    EsCajaChica = EntidadAnterior.EsCajaChica;
                    EsCheque = EntidadAnterior.EsCheque;
                    EsMovInventario = EntidadAnterior.EsMovimientoInventario;
                    EsDocumento = EntidadAnterior.GeneraDocumento;
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
            if (String.IsNullOrEmpty(txtNombre.Text))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkCC.EditValue) < 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Cuenta Contable.", Parametros.MsgType.warning);
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
                    Entidad.ConceptoContable Concepto;

                    if (Editable)
                    { Concepto = db.ConceptoContables.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        Concepto = new Entidad.ConceptoContable();
                        Concepto.Activo = true;
                    }

                    Concepto.Nombre = Nombre;
                    Concepto.CuentaContableID = CC;
                    Concepto.EsRoc = EsRoc;
                    Concepto.Permisible = Permisible;
                    Concepto.EsAutoconsumo = EsAutoconsumo;
                    Concepto.EsAjuste = EsAjuste;
                    Concepto.EsCajaChica = EsCajaChica;
                    Concepto.EsCheque = EsCheque;
                    Concepto.EsMovimientoInventario = EsMovInventario;
                    Concepto.GeneraDocumento = EsDocumento;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(Concepto, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó el concepto contable: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ConceptoContables.InsertOnSubmit(Concepto);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó el concepto contable: " + Concepto.Nombre, this.Name);

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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #endregion
    }
    
}