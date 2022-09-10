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
using SAGAS.Inventario.Forms;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogProveedores : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormProveedores MDI;
        internal Entidad.Proveedor EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private DataTable dtEstacionesServicios;
        private DataTable dtEstacionesServiciosAlcaldia;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string NombreComercial
        {
            get { return txtNombreComercial.Text; }
            set { txtNombreComercial.Text = value; }
        }

        private string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        private string RUC
        {
            get { return txtRUC.Text; }
            set { txtRUC.Text = value; }
        }

        private int IDTipoProveedor
        {
            get { return Convert.ToInt32(lkTipoProveedor.EditValue); }
            set { lkTipoProveedor.EditValue = value; }
        }

        private int IDClaseProveedor
        {
            get { return Convert.ToInt32(lkClaseID.EditValue); }
            set { lkClaseID.EditValue = value; }
        }

        private int IDDepartamento
        {
            get { return Convert.ToInt32(lkDepartamento.EditValue); }
            set { lkDepartamento.EditValue = value; }
        }

        private string Contacto
        {
            get { return memoContacto.Text; }
            set { memoContacto.Text = value; }
        }

        private string Direccion
        {
            get { return memoDir.Text; }
            set { memoDir.Text = value; }
        }

        private string Telefono1
        {
            get { return txtTelefono1.Text; }
            set { txtTelefono1.Text = value; }
        }

        private string Telefono2
        {
            get { return txtTelefono2.Text; }
            set { txtTelefono2.Text = value; }
        }

        private string Telefono3
        {
            get { return txtTelefono3.Text; }
            set { txtTelefono3.Text = value; }
        }

        private string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }

        private string SitioWeb
        {
            get { return txtSitioWeb.Text; }
            set { txtSitioWeb.Text = value; }
        }
        
        public decimal Limite
        {
            get { return Convert.ToInt32(spLimiteCredito.Value); }
            set { spLimiteCredito.Value = value; }
        }

        public int Plazo
        {
            get { return Convert.ToInt32(spPlazo.Value); }
            set { spPlazo.Value = value; }
        }

        public int CCID
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        public int CCImpuesto
        {
            get { return Convert.ToInt32(this.glkCCI.EditValue); }
            set { this.glkCCI.EditValue = value; }
        }

        private bool AplicaRetencion
        {
            get { return Convert.ToBoolean(chkAplicaRetencion.Checked); }
            set { chkAplicaRetencion.Checked = value; }
        }

        private bool AplicaIVA
        {
            get { return Convert.ToBoolean(chkAplicaIVA.Checked); }
            set { chkAplicaIVA.Checked = value; }
        }

        private bool CajaChPagosMan
        {
            get { return Convert.ToBoolean(chkCajaChPagosMan.Checked); }
            set { chkCajaChPagosMan.Checked = value; }
        }
        private bool AplicaAlcaldia
        {
            get { return Convert.ToBoolean(chkAplicaAlcaldia.Checked); }
            set { chkAplicaAlcaldia.Checked = value; }
        }

        private bool EsMasivo
        {
            get { return Convert.ToBoolean(chkProveedorMasivo.Checked); }
            set { chkProveedorMasivo.Checked = value; }
        }

        private bool EsMasivoManual
        {
            get { return Convert.ToBoolean(chkMasivoManual.Checked); }
            set { chkMasivoManual.Checked = value; }
        }

        private bool AplicaAbono
        {
            get { return Convert.ToBoolean(chkAplicaAbono.Checked); }
            set { chkAplicaAbono.Checked = value; }
        }

        private int ESPagoID
        {
            get { return Convert.ToInt32(lkEstacion.EditValue); }
            set { lkEstacion.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogProveedores()
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

                lkTipoProveedor.Properties.DataSource = from tp in db.TipoProveedors where tp.Activo select new { tp.ID, tp.Nombre };
                lkTipoProveedor.Properties.DisplayMember = "Nombre";
                lkTipoProveedor.Properties.ValueMember = "ID";

                lkClaseID.Properties.DataSource = from pc in db.ProveedorClases select new { pc.ID, pc.Nombre };
                lkClaseID.Properties.DisplayMember = "Nombre";
                lkClaseID.Properties.ValueMember = "ID";

                lkDepartamento.Properties.DataSource = from d in db.Departamentos where d.Activo select new { d.ID, d.Nombre};
                lkDepartamento.Properties.DisplayMember = "Nombre";
                lkDepartamento.Properties.ValueMember = "ID";

                glkCC.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre};
                glkCC.Properties.DisplayMember = "Display";
                glkCC.Properties.ValueMember = "ID";

                glkCCI.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle && cc.EsImpuesto select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                glkCCI.Properties.DisplayMember = "Display";
                glkCCI.Properties.ValueMember = "ID";

                var listSucursales = (from ES in db.EstacionServicios
                                      where ES.Activo
                                      select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

                lkEstacion.Properties.DataSource = listSucursales;

                var listSucursalesAlcaldia = (from ES in db.EstacionServicios
                                      where ES.Activo && ES.AplicaRetencionAlcaldia
                                      select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

                this.dtEstacionesServiciosAlcaldia = ToDataTable(listSucursalesAlcaldia);
                this.dtEstacionesServicios = ToDataTable(listSucursales);

                this.gridAlcaldia.DataSource = dtEstacionesServiciosAlcaldia;
                this.grid.DataSource = dtEstacionesServicios;
                
                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    NombreComercial = EntidadAnterior.NombreComercial;
                    RUC = EntidadAnterior.RUC;
                    IDTipoProveedor = EntidadAnterior.TipoProveedorID;
                    IDClaseProveedor = EntidadAnterior.ProveedorClaseID;
                    IDDepartamento = EntidadAnterior.DepartamentoID;
                    Contacto = EntidadAnterior.Contacto;
                    Direccion = EntidadAnterior.Direccion;
                    Telefono1 = EntidadAnterior.Telefono1;
                    Telefono2 = EntidadAnterior.Telefono2;
                    Telefono3 = EntidadAnterior.Telefono3;
                    Email = EntidadAnterior.Email;
                    SitioWeb = EntidadAnterior.WebSite;
                    Limite = EntidadAnterior.LimiteCredito;
                    Plazo = EntidadAnterior.Plazo;
                    CCID = EntidadAnterior.CuentaContableID;
                    AplicaRetencion = EntidadAnterior.AplicaRetencion;
                    CCImpuesto = EntidadAnterior.ImpuestoRetencionID;
                    AplicaIVA = EntidadAnterior.AplicaIVA;
                    ESPagoID = EntidadAnterior.EstacionPagoID;
                    EsMasivo = EntidadAnterior.PagoMasivo;
                    EsMasivoManual = EntidadAnterior.PagoMasivoManual;
                    AplicaAbono = EntidadAnterior.AplicaAbono;
                    chkBloqueo.Checked = EntidadAnterior.Bloqueado;
                    CajaChPagosMan = EntidadAnterior.MostrarCajaChicaEnPagoManual;
                    if (EntidadAnterior.AplicaBloqueo)
                    {
                        chkBloqueo.Visible = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkBloqueado");
                    }

                    if (this.chkProveedorMasivo.Checked)
                    {
                        db.EstacionServicios.Where(es => es.Activo).ToList().ForEach(obj =>
                        {
                            if (!EntidadAnterior.ProveedorMasivoEstacions.Any(o => o.EstacionServicioID.Equals(obj.ID))) // (obj.id  ToList().Exists .Contains(q => q.EstacionID.Equals(obj.ID)))
                            {
                                DataRow[] ESRow = dtEstacionesServicios.Select("ID = " + obj.ID);
                                DataRow row = ESRow.First();
                                row["SelectedES"] = false;
                            }
                        });

                        gvData.RefreshData();
                    }

                        db.EstacionServicios.Where(es => es.Activo && es.AplicaRetencionAlcaldia).ToList().ForEach(obj =>
                        {
                            if (!EntidadAnterior.ProveedorAlcaldiaEstacions.Any(o => o.EstacionServicioID.Equals(obj.ID)))
                            {
                                DataRow[] ESRow = dtEstacionesServiciosAlcaldia.Select("ID = " + obj.ID);
                                DataRow row = ESRow.First();
                                row["SelectedES"] = false;
                            }
                        });

                        gvDataAlcaldia.RefreshData();

                        if (dtEstacionesServiciosAlcaldia.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
                            AplicaAlcaldia = false;
                        else
                            AplicaAlcaldia = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkTipoProveedor, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkDepartamento, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtRUC, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkClaseID, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "" || String.IsNullOrEmpty(txtRUC.Text) || lkTipoProveedor.EditValue == null || IDClaseProveedor .Equals(0) || lkClaseID.EditValue == null || lkDepartamento.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

             if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Proveedor '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

             if (Convert.ToInt32(glkCC.EditValue) <= 0)
             {
                 Parametros.General.DialogMsg("Debe seleccionar la Cuenta Contable para este Proveedor.", Parametros.MsgType.warning);
                 return false;
             }

             if (chkAplicaRetencion.Checked)
             {
                 if (Convert.ToInt32(glkCCI.EditValue) <= 0)
                 {
                     Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGSELECCIONARCC + Environment.NewLine + " Para la Retención.", Parametros.MsgType.warning);
                     return false;
                 }

             }

             if (chkProveedorMasivo.Checked)
             {
                 if (dtEstacionesServicios.AsEnumerable().Count(c => ((bool) c["SelectedES"]).Equals(true)) <= 0)
                 {
                     Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio", Parametros.MsgType.warning);
                     dtEstacionesServicios.DefaultView.RowFilter = "";
                     return false;
                 }

                 if (ESPagoID <= 0)
                 {
                     Parametros.General.DialogMsg("Debe seleccionar la Estación / Oficina de pago masivo para este Proveedor.", Parametros.MsgType.warning);
                     return false;
                 }

             }

             //if (!chkAplicaAlcaldia.Checked)
             //{
             //    if (dtEstacionesServiciosAlcaldia.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
             //    {
             //        Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio que no aplique retención de alcaldía", Parametros.MsgType.warning);
             //        dtEstacionesServiciosAlcaldia.DefaultView.RowFilter = "";
             //        return false;
             //    }

             //}

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Proveedors
                          where (ID.HasValue ? i.Codigo == code && i.ID != Convert.ToInt32(ID) : i.Codigo == code)
                          select i);

            if (result.Count() > 0)
            {
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
                    Entidad.Proveedor P;

                    if (Editable)
                    { P = db.Proveedors.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        P = new Entidad.Proveedor();
                        P.Activo = true;
                    }

                    P.Codigo = Codigo;
                    P.Nombre = Nombre;
                    P.NombreComercial = NombreComercial;
                    P.RUC = RUC;
                    P.TipoProveedorID = IDTipoProveedor;
                    P.ProveedorClaseID = IDClaseProveedor;
                    P.DepartamentoID = IDDepartamento;
                    P.Contacto = Contacto;
                    P.Direccion = Direccion;
                    P.Telefono1 = Telefono1;
                    P.Telefono2 = Telefono2;
                    P.Telefono3 = Telefono3;
                    P.Email = Email;
                    P.WebSite = SitioWeb;
                    P.LimiteCredito = Limite;
                    P.Plazo = Plazo;
                    P.CuentaContableID = CCID;
                    P.AplicaRetencion = AplicaRetencion;
                    P.ImpuestoRetencionID = CCImpuesto;
                    P.AplicaIVA = AplicaIVA;
                    P.AplicaAlcaldia = AplicaAlcaldia;
                    P.PagoMasivo = EsMasivo;
                    P.EstacionPagoID = ESPagoID;
                    P.AplicaAbono = AplicaAbono;
                    P.PagoMasivoManual = EsMasivoManual;
                    P.Bloqueado = chkBloqueo.Checked;
                    P.MostrarCajaChicaEnPagoManual = CajaChPagosMan;
                                 
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(P, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó el Proveedor: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Proveedors.InsertOnSubmit(P);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Proveedor: " + P.Nombre, this.Name);

                    }

                    db.SubmitChanges();

                    foreach (DataRow rb in dtEstacionesServicios.DefaultView.Table.Rows)
                    {
                        if (Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PMAnterior = P.ProveedorMasivoEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PMAnterior.Count() <= 0)
                            {

                                Entidad.ProveedorMasivoEstacion PM = new Entidad.ProveedorMasivoEstacion();
                                PM.EstacionServicioID = Convert.ToInt32(rb["ID"]);
                                P.ProveedorMasivoEstacions.Add(PM);
                                db.SubmitChanges();
                            }
                        }
                        else if (!Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PM = P.ProveedorMasivoEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PM.Count() > 0)
                            {
                                P.ProveedorMasivoEstacions.Remove(PM.First());
                                db.SubmitChanges();
                            }
                        }

                    }

                    foreach (DataRow rb in dtEstacionesServiciosAlcaldia.DefaultView.Table.Rows)
                    {
                        if (Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PMAnterior = P.ProveedorAlcaldiaEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PMAnterior.Count() <= 0)
                            {

                                Entidad.ProveedorAlcaldiaEstacion PAM = new Entidad.ProveedorAlcaldiaEstacion();
                                PAM.EstacionServicioID = Convert.ToInt32(rb["ID"]);
                                P.ProveedorAlcaldiaEstacions.Add(PAM);
                                db.SubmitChanges();
                            }
                        }
                        else if (!Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PAM = P.ProveedorAlcaldiaEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PAM.Count() > 0)
                            {
                                P.ProveedorAlcaldiaEstacions.Remove(PAM.First());
                                db.SubmitChanges();
                            }
                        }

                    }

                    
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
            Parametros.General.ValidateEmptyStringRule((TextEdit)sender, errRequiredField);
        }

        private void btnSelectAllES_Click(object sender, EventArgs e)
        {
            gvData.ActiveFilter.Clear();
            for (int i = 0; i < gvData.RowCount; i++)
            {
                gvData.SetRowCellValue(i, "SelectedES", true);
            }
        }

        private void btnUnselectAllES_Click(object sender, EventArgs e)
        {
            gvData.ActiveFilter.Clear();
            for (int i = 0; i < gvData.RowCount; i++)
            {
                gvData.SetRowCellValue(i, "SelectedES", false);
            }
        }

        private void chkAplicaRetencion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaRetencion.Checked)
                this.layoutControlGroupCCRetencion .Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else if (!chkAplicaRetencion.Checked)
            {
                this.layoutControlGroupCCRetencion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.glkCCI.EditValue = null;
            }
        }

        private void chkProveedorMasivo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProveedorMasivo.Checked)            
            {
                this.layoutControlItemES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                btnSelectAllES_Click(null, null);
                lkEstacion.Enabled = true;
            }
            else if (!chkProveedorMasivo.Checked)
            {
                this.layoutControlItemES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                btnUnselectAllES_Click(null, null);
                ESPagoID = 0;
                lkEstacion.Enabled = false;
            }
        }

        private void chkAplicaAlcaldia_CheckedChanged(object sender, EventArgs e)
        {
            //if (!chkAplicaAlcaldia.Checked)
            //{
            //    this.layoutControlGroupAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //}
            //else if (chkAplicaAlcaldia.Checked)
            //{
            //    this.layoutControlGroupAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    gvDataAlcaldia.ActiveFilter.Clear();
            //    for (int i = 0; i < gvDataAlcaldia.RowCount; i++)
            //    {
            //        gvDataAlcaldia.SetRowCellValue(i, "SelectedES", false);
            //    }
            //}
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                btnCancel_Click(null, null);
                return true;
            }

            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        private void txtRUC_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsLetterOrDigit(e.KeyChar);
        }

        private void gvDataAlcaldia_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (dtEstacionesServiciosAlcaldia.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
                    AplicaAlcaldia = false;
                else
                    AplicaAlcaldia = true;

                gvDataAlcaldia.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());               
            }

        }

        private void btnSelectAllESAlma_Click(object sender, EventArgs e)
        {
            gvDataAlcaldia.ActiveFilter.Clear();
            for (int i = 0; i < gvDataAlcaldia.RowCount; i++)
            {
                gvDataAlcaldia.SetRowCellValue(i, "SelectedES", true);
            }

            gvDataAlcaldia.RefreshData();

            if (dtEstacionesServiciosAlcaldia.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
                AplicaAlcaldia = false;
            else
                AplicaAlcaldia = true;
        }

        private void btnUnselectAllESAlma_Click(object sender, EventArgs e)
        {
            gvDataAlcaldia.ActiveFilter.Clear();
            for (int i = 0; i < gvDataAlcaldia.RowCount; i++)
            {
                gvDataAlcaldia.SetRowCellValue(i, "SelectedES", false);
            }

            gvDataAlcaldia.RefreshData();

            if (dtEstacionesServiciosAlcaldia.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
                AplicaAlcaldia = false;
            else
                AplicaAlcaldia = true;
        }
    
    }
}