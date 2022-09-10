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
    public partial class DialogRemesa : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
        internal Forms.FormCuentaBancaria MDI;
        internal Entidad.Remesa EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;
        private int UsuarioID = 0;
        
        public int Cuenta
        {
            get { return Convert.ToInt32(this.lkCuenta.EditValue); }
            set { this.lkCuenta.EditValue = value; }
        }       

        private int Inicial
        {
            get { return Convert.ToInt32(this.spInicial.Value); }
            set { this.spInicial.Value = value; }
        }

        private int Final
        {
            get { return Convert.ToInt32(this.spFinal.Value); }
            set { this.spFinal.Value = value; }
        }

        private bool EnUso
        {
            set { this.chkEnUso.Checked = value; }
        }

        private string Consecutivo
        {
            get { return txtConsecutivo.Text; }
            set { txtConsecutivo.Text = value; }
        }
                
        #endregion

        #region *** INICIO ***

        public DialogRemesa(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
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
                lkCuenta.Properties.DataSource = db.CuentaBancarias.Where(b => b.Activo).Select(s => new { s.ID, Display = s.Nombre});

                if (Parametros.General.SystemOptionAcces(UsuarioID, "chkCambioInicial"))
                    layoutControlItemPermitir.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                if (Editable)
                {
                    Cuenta = EntidadAnterior.CuentaBancariaID;
                    Inicial = EntidadAnterior.NumeroInicial;
                    Final = EntidadAnterior.NumeroFinal;
                    EnUso = EntidadAnterior.EnUso;
                    spNumero.Value = EntidadAnterior.Numero;

                    layoutControlItemUso.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemNumero.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
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
            Parametros.General.ValidateEmptyStringRule(lkCuenta, errRequiredField);;
        }

        public bool ValidarCampos()
        {
            if (lkCuenta.EditValue == null || Cuenta.Equals(0))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Final <= Inicial)
            {
                Parametros.General.DialogMsg("El número inicial de la remesa no puede ser igual o menor al número final." + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.Remesa R;

                    if (Editable)
                    { R = db.Remesas.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        R = new Entidad.Remesa();
                        int number = 1;
                        if (db.Remesas.Count(m => m.CuentaBancariaID.Equals(Cuenta)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Remesas.Where(m => m.CuentaBancariaID.Equals(Cuenta)).OrderByDescending(o => o.Consecutivo).First().Consecutivo.ToString());
                        }

                        R.Consecutivo = number;
                    }

                    R.CuentaBancariaID = Cuenta;
                    R.NumeroInicial = Inicial;
                    R.NumeroFinal = Final;                    
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(R, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                         "Se modificó la Remesa: " + EntidadAnterior.Consecutivo.ToString("000000"), this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Remesas.InsertOnSubmit(R);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se creó la Remesa: " + R.Consecutivo.ToString("000000"), this.Name);
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
            MDI.CleanDialog(ShowMsg, true, true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkCuenta_EditValueChanged(object sender, EventArgs e)
        {
            if (!Cuenta.Equals(0))
            {
                int consec = 1;
                if (db.Remesas.Count(m => m.CuentaBancariaID.Equals(Cuenta)) > 0)
                {
                    consec = Parametros.General.ValorConsecutivo(db.Remesas.Where(m => m.CuentaBancariaID.Equals(Cuenta)).OrderByDescending(o => o.Consecutivo).First().Consecutivo.ToString());
                }

                Consecutivo = consec.ToString("000000");

                int number = 1;
                if (db.Remesas.Count(m => m.CuentaBancariaID.Equals(Cuenta)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Remesas.Where(m => m.CuentaBancariaID.Equals(Cuenta)).OrderByDescending(o => o.Consecutivo).First().NumeroFinal.ToString());
                }

                Inicial = number;
                Final = number;
            }
        }
        
        private void chkCambioInicial_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCambioInicial.Checked)
                spInicial.Properties.ReadOnly = false;
            else
                spInicial.Properties.ReadOnly = true;
        }

        #endregion

    }
    
}