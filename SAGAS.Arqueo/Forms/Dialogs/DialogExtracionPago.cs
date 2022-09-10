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
using SAGAS.Arqueo.Forms;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogExtracionPago : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormExtracionPago MDI;
        internal Entidad.ExtracionPago EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        //private List<Parametros.TipoComprobanteArqueo> listadoTipo;
        private Parametros.ListTipoComprobanteArqueo listadoTipo = new Parametros.ListTipoComprobanteArqueo(); 

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public bool EsPago
        {
            get { return Convert.ToBoolean(chkEsPago.Checked); }
            set { chkEsPago.Checked = value; }
        }

        public bool TieneDetalle
        {
            get { return Convert.ToBoolean(chkTieneDetalle.Checked); }
            set { chkTieneDetalle.Checked = value; }
        }

        public bool TieneConcepto
        {
            get { return Convert.ToBoolean(chkTieneConcepto.Checked); }
            set { chkTieneConcepto.Checked = value; }
        }

        public bool EsEfectivo
        {
            get { return Convert.ToBoolean(chkEsEfectivo.Checked); }
            set { chkEsEfectivo.Checked = value; }
        }

        public int Orden
        {
            get { return Convert.ToInt32(speOrden.Value); }
            set { speOrden.EditValue = value; }
        }

        public decimal EquivalenteLitros
        {
            get { return Convert.ToDecimal(speLitros.Value); }
            set { speLitros.EditValue = value; }
        }

        public decimal EquivalenteMoneda
        {
            get { return Convert.ToDecimal(spMoneda.Value); }
            set { spMoneda.EditValue = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int DeudorID
        {
            get { return Convert.ToInt32(this.glkDeudor.EditValue); }
            set { this.glkDeudor.EditValue = value; }
        }

        public int CCID
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        public bool EsOtro
        {
            get { return Convert.ToBoolean(chkOtro.Checked); }
            set { chkOtro.Checked = value; }
        }

        public bool EsCupon
        {
            get { return Convert.ToBoolean(chkCupon.Checked); }
            set { chkCupon.Checked = value; }
        }

        public bool EsPetrocard
        {
            get { return Convert.ToBoolean(chkPetrocard.Checked); }
            set { chkPetrocard.Checked = value; }
        }

        public bool EsCuponCS
        {
            get { return Convert.ToBoolean(chkCuponCS.Checked); }
            set { chkCuponCS.Checked = value; }
        }

        public bool EsContabilizadoTurnos
        {
            get { return Convert.ToBoolean(chkContabiliadoTurnos.Checked); }
            set { chkContabiliadoTurnos.Checked = value; }
        }

        public bool EsCuponEspecial
        {
            get { return Convert.ToBoolean(chkEspecial.Checked); }
            set { chkEspecial.Checked = value; }
        }

        public int Tipo
        {
            get { return Convert.ToInt32(this.lkTipo.EditValue); }
            set { this.lkTipo.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogExtracionPago()
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

                var ListaCC = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();

                glkCC.Properties.DataSource = ListaCC;
                glkDeudor.Properties.DataSource = (from cc in db.Clientes where cc.Activo select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lkCuenta.DataSource = ListaCC;
                lkTipo.Properties.DataSource = listadoTipo.GetListTipoComprobanteArqueo();
                Tipo = 0;
                             
                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Orden = EntidadAnterior.Orden;
                    TieneDetalle = EntidadAnterior.TieneDetalle;
                    EsPago = EntidadAnterior.EsPago;
                    EquivalenteLitros = EntidadAnterior.EquivalenteLitros;
                    Comentario = EntidadAnterior.Comentario;
                    TieneConcepto = EntidadAnterior.TieneConcepto;
                    EsEfectivo = EntidadAnterior.EsEfectivo;
                    EsOtro = EntidadAnterior.EsOtro;
                    DeudorID = EntidadAnterior.DeudorID;
                    CCID = EntidadAnterior.CuentaContableID;
                    EsCupon = EntidadAnterior.EsCupon;
                    EsPetrocard = EntidadAnterior.EsPetrocard;
                    Tipo = EntidadAnterior.TipoComprobanteArqueo;
                    EsContabilizadoTurnos = EntidadAnterior.ContabilizarPorTurnos;
                    EsCuponCS = EntidadAnterior.EsCuponMoneda;
                    EquivalenteMoneda = EntidadAnterior.EquivalenteMoneda;
                    EsCuponEspecial = EntidadAnterior.EsCuponEspecial;
                }
                else
                { EntidadAnterior = new Entidad.ExtracionPago(); }
                 
                bdsConceptos.DataSource = EntidadAnterior;
                bdsConceptos.DataMember = "ExtracionConceptos";
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || speOrden.Value < 1)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (EsPetrocard.Equals(true) && EsCupon.Equals(true))
            {
                Parametros.General.DialogMsg("El registro no puede ser Cupón ni Petrocard, debe seleccionar una sola opción." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (TieneConcepto)
            {
                if (EntidadAnterior.ExtracionConceptos.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de asignar al menos un concepto a la lista." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
            }

            if (!EsOtro)
            {
                if (DeudorID.Equals(0) && CCID.Equals(0))
                {
                    Parametros.General.DialogMsg("Debe de asignar un deudor o una cuenta contable." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
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
                    Entidad.ExtracionPago EP;

                    if (Editable)
                    { EP = db.ExtracionPagos.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        EP = new Entidad.ExtracionPago();
                        EP.Activo = true;
                    }

                    EP.Nombre = Nombre;
                    EP.Orden = Orden;
                    EP.TieneDetalle = TieneDetalle;
                    EP.EsPago = EsPago;
                    EP.EquivalenteLitros = EquivalenteLitros;
                    EP.Comentario = Comentario;
                    EP.TieneConcepto = TieneConcepto;
                    EP.EsEfectivo = EsEfectivo;
                    EP.EsOtro = EsOtro;
                    EP.DeudorID = DeudorID;
                    EP.CuentaContableID = CCID;
                    EP.EsCupon = EsCupon;
                    EP.EsPetrocard = EsPetrocard;
                    EP.TipoComprobanteArqueo = Tipo;
                    EP.ContabilizarPorTurnos = EsContabilizadoTurnos;
                    EP.EquivalenteMoneda = EquivalenteMoneda;
                    EP.EsCuponMoneda = EsCuponCS;
                    EP.EsCuponEspecial = EsCuponEspecial;

                    if (TieneConcepto)
                        EP.ExtracionConceptos = EntidadAnterior.ExtracionConceptos;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EP, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                         "Se modificó Tipo de Extración / Pago: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ExtracionPagos.InsertOnSubmit(EP);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó Tipo de Extración / Pago: " + EP.Nombre, this.Name);

                    }

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

        private void chkTieneConcepto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTieneConcepto.Checked)
                layoutControlGrid.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else if (!chkTieneConcepto.Checked)
                layoutControlGrid.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        #endregion

        private void gvDataConcepto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }
            if (e.KeyCode == Keys.Delete)
            {

                if (gvDataConcepto.FocusedRowHandle >= 0)
                {
                    int _id = Convert.ToInt32(gvDataConcepto.GetFocusedRowCellValue("ID"));

                    foreach (Entidad.ExtracionConcepto i in EntidadAnterior.ExtracionConceptos)
                    {
                        if (i.ID == _id)
                        {

                            //Entity.PrerequisiteClass p = db.PrerequisiteClasses.Single(o => o.PrerequisiteClassID == _id);
                            //db.PrerequisiteClasses.DeleteOnSubmit(p);
                            EntidadAnterior.ExtracionConceptos.Remove(i);

                            //db.SubmitChanges();
                            break;
                        }
                    }
                    gvDataConcepto.RefreshData(); 
                }

            }
        }

        private void chkOtro_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOtro.Checked)
            {
                DeudorID = 0;
                CCID = 0;
                glkDeudor.Properties.ReadOnly = true;
                glkCC.Properties.ReadOnly = true;
            }
            else
            {
                glkDeudor.Properties.ReadOnly = false;
                glkCC.Properties.ReadOnly = false;
            }
        }

        private void glkDeudor_EditValueChanged(object sender, EventArgs e)
        {
            if (DeudorID > 0)
                CCID = 0;
        }

        private void glkCC_EditValueChanged(object sender, EventArgs e)
        {
            if (CCID > 0)
                DeudorID = 0;
        }

        private void chkCupon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCupon.Checked)
                chkPetrocard.Checked = false;
        }

        private void chkPetrocard_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPetrocard.Checked)
                chkCupon.Checked = false;
        }

    }
}