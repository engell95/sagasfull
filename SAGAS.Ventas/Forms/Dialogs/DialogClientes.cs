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
using SAGAS.Ventas.Forms;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogClientes : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormClientes MDI;
        internal Entidad.Cliente EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private DataTable dtEstacionesServicios;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string RazonSocial
        {
            get { return txtRazonSocial.Text; }
            set { txtRazonSocial.Text = value; }
        }

        private string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        private string CodigoInterno
        {
            get { return txtCodigoInterno.Text; }
            set { txtCodigoInterno.Text = value; }
        }

        private string RUC
        {
            get { return txtRUC.Text; }
            set { txtRUC.Text = value; }
        }
        
        private int IDDepartamento
        {
            get { return Convert.ToInt32(lkDepartamento.EditValue); }
            set { lkDepartamento.EditValue = value; }
        }

        private string Contactos
        {
            get { return memoContacto.Text; }
            set { memoContacto.Text = value; }
        }

        private string Municipio
        {
            get { return txtMunicipio.Text; }
            set { txtMunicipio.Text = value; }
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
            get { return Convert.ToDecimal(spLimiteCredito.Value); }
            set { spLimiteCredito.Value = value; }
        }

        public int Plazo
        {
            get { return Convert.ToInt32(spPlazo.Value); }
            set { spPlazo.Value = value; }
        }

        public decimal DescuentoXLitro
        {
            get { return Convert.ToInt32(spDescuentoXLitro.Value); }
            set { spDescuentoXLitro.Value = value; }
        }

        public decimal Interes
        {
            get { return Convert.ToInt32(spInteres.Value); }
            set { spInteres.Value = value; }
        }

        public int CCID
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        private bool ExentoIVA
        {
            get { return Convert.ToBoolean(chkExentoIVA.Checked); }
            set { chkExentoIVA.Checked = value; }
        }

        private bool PagoGrupal
        {
            get { return Convert.ToBoolean(chkPagoGrupal.Checked); }
            set { chkPagoGrupal.Checked = value; }
        }

        private int IDTipo
        {
            get { return Convert.ToInt32(lkTipo.EditValue); }
            set { lkTipo.EditValue = value; }
        }

        private int ESPagoID
        {
            get { return Convert.ToInt32(lkEstacion.EditValue); }
            set { lkEstacion.EditValue = value; }
        }

        private bool AplicaCredito
        {
            get { return Convert.ToBoolean(chkAplicaCredito.Checked); }
            set { chkAplicaCredito.Checked = value; }
        }

        public decimal DAsumido
        {
            get { return Convert.ToDecimal(spAsumido.Value); }
            set { spAsumido.Value = value; }
        }

        public decimal DCompartido
        {
            get { return Convert.ToDecimal(spCompartido.Value); }
            set { spCompartido.Value = value; }
        }

        public decimal DTotal
        {
            get { return Convert.ToDecimal(spDescTotal.Value); }
            set { spDescTotal.Value = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogClientes()
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

                lkDepartamento.Properties.DataSource = from d in db.Departamentos where d.Activo select new { d.ID, d.Nombre};
                lkDepartamento.Properties.DisplayMember = "Nombre";
                lkDepartamento.Properties.ValueMember = "ID";

                //lkEstacion.Properties.DataSource = from es in db.EstacionServicios
                //                                            where es.Activo 
                //                                            select new { es.ID, es.Nombre };

                lkTipo.Properties.DataSource = from tc in db.TipoClientes where tc.Activo select new { tc.ID, tc.Nombre };

                var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });


                glkCC.Properties.DataSource = lista;
                //glkCC.Properties.DisplayMember = "Display";
                //glkCC.Properties.ValueMember = "ID";

                var listSucursales = (from ES in db.EstacionServicios
                                      where ES.Activo
                                      select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

                var listaMasiva = listSucursales.ToList();

                listaMasiva.Insert(0, new { ID = 0, Nombre = "N / A", SelectedES = false });

                lkEstacion.Properties.DataSource = listaMasiva;
                this.dtEstacionesServicios = ToDataTable(listSucursales);

                this.grid.DataSource = dtEstacionesServicios;

                gvData.ActiveFilter.Clear();
                for (int i = 0; i < gvData.RowCount; i++)
                {
                    gvData.SetRowCellValue(i, "SelectedES", false);
                }

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    CodigoInterno = EntidadAnterior.CodigoInterno;
                    RazonSocial = EntidadAnterior.RazonSocial;
                    RUC = EntidadAnterior.RUC;
                    IDDepartamento = EntidadAnterior.DepartamentoID;
                    Contactos = EntidadAnterior.Contactos;
                    Direccion = EntidadAnterior.Direccion;
                    Telefono1 = EntidadAnterior.Telefono1;
                    Telefono2 = EntidadAnterior.Telefono2;
                    Telefono3 = EntidadAnterior.Telefono3;
                    Email = EntidadAnterior.Email;
                    SitioWeb = EntidadAnterior.WebSite;
                    Limite = EntidadAnterior.LimiteCredito;
                    Plazo = EntidadAnterior.Plazo;
                    CCID = EntidadAnterior.CuentaContableID;
                    Municipio = EntidadAnterior.Municipio;
                    DescuentoXLitro = EntidadAnterior.DescuentoXLitro;
                    Interes = EntidadAnterior.Interes;
                    ExentoIVA = EntidadAnterior.ExentoIVA;
                    IDTipo = EntidadAnterior.TipoClienteID;
                    ESPagoID = EntidadAnterior.EstacionPagoID;
                    PagoGrupal = EntidadAnterior.RealizaPagoGrupoES;
                    AplicaCredito = EntidadAnterior.AplicaCreditoLubricentro;
                    DAsumido = EntidadAnterior.DescuentoAsumido;
                    DCompartido = EntidadAnterior.DescuentoCompartido;
                    DTotal = EntidadAnterior.DescuentoTotal;

                    db.EstacionServicios.Where(es => es.Activo).ToList().ForEach(obj =>
                        {
                            if (EntidadAnterior.ClienteEstacions.Any(o => o.EstacionServicioID.Equals(obj.ID))) // (obj.id  ToList().Exists .Contains(q => q.EstacionID.Equals(obj.ID)))
                            {
                                DataRow[] ESRow = dtEstacionesServicios.Select("ID = " + obj.ID);
                                DataRow row = ESRow.First();
                                row["SelectedES"] = true;
                            }
                        });

                    gvData.RefreshData();

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
            Parametros.General.ValidateEmptyStringRule(lkDepartamento, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkTipo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtRUC, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "" || lkDepartamento.EditValue == null || lkTipo.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Cliente '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            if (!IDTipo.Equals(Parametros.Config.TipoClienteManejoID()))
            {
                if (glkCC.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Cuenta Contable para este Cliente.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToInt32(glkCC.EditValue) < 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Cuenta Contable para este Cliente.", Parametros.MsgType.warning);
                    return false;
                }

                if (String.IsNullOrEmpty(txtRUC.Text))
                {
                    Parametros.General.DialogMsg("Debe Digitar el Número RUC del Cliente.", Parametros.MsgType.warning);
                    return false;
                }
            }
            if (dtEstacionesServicios.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio", Parametros.MsgType.warning);
                dtEstacionesServicios.DefaultView.RowFilter = "";
                return false;
            }

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Clientes
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
                    Entidad.Cliente C;

                    if (Editable)
                    { C = db.Clientes.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        C = new Entidad.Cliente();
                        C.Activo = true;
                    }

                    C.Codigo = Codigo;
                    C.CodigoInterno = CodigoInterno;
                    C.Nombre = Nombre;
                    C.RazonSocial = RazonSocial;
                    C.RUC = RUC;
                    C.DepartamentoID = IDDepartamento;
                    C.Contactos = Contactos;
                    C.Direccion = Direccion;
                    C.Telefono1 = Telefono1;
                    C.Telefono2 = Telefono2;
                    C.Telefono3 = Telefono3;
                    C.Email = Email;
                    C.WebSite = SitioWeb;
                    C.LimiteCredito = Limite;
                    C.Plazo = Plazo;
                    C.CuentaContableID = CCID;
                    C.Municipio = Municipio;
                    C.DescuentoXLitro = DescuentoXLitro;
                    C.Interes = Interes;
                    C.ExentoIVA = ExentoIVA;
                    C.TipoClienteID = IDTipo;
                    C.RealizaPagoGrupoES = PagoGrupal;
                    C.EstacionPagoID = ESPagoID;
                    C.AplicaCreditoLubricentro = AplicaCredito;
                    C.DescuentoAsumido = DAsumido;
                    C.DescuentoCompartido = DCompartido;
                    C.DescuentoTotal = DTotal;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(C, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó el Cliente: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Clientes.InsertOnSubmit(C);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Cliente: " + C.Nombre, this.Name);

                    }

                    db.SubmitChanges();

                    foreach (DataRow rb in dtEstacionesServicios.DefaultView.Table.Rows)
                    {
                        if (Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PMAnterior = C.ClienteEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PMAnterior.Count() <= 0)
                            {

                                Entidad.ClienteEstacion CE = new Entidad.ClienteEstacion();
                                CE.EstacionServicioID = Convert.ToInt32(rb["ID"]);
                                C.ClienteEstacions.Add(CE);
                                db.SubmitChanges();
                            }
                        }
                        else if (!Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var PM = C.ClienteEstacions.Where(x => x.EstacionServicioID.Equals(Convert.ToInt32(rb["ID"])));

                            if (PM.Count() > 0)
                            {
                                C.ClienteEstacions.Remove(PM.First());
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

        private void txtRUC_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Char.IsLetterOrDigit(e.KeyChar);
        }
         
        private void chkPagoGrupal_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (ESPagoID.Equals(0))
            {
                Parametros.General.DialogMsg("No ha seleccionado una Estación de Servicio donde se realizaran los pagos grupales.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        #endregion

        private void spAsumido_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DTotal = DAsumido + DCompartido;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

    }
}