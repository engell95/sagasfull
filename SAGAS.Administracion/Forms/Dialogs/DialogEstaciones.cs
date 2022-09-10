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

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogEstaciones : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEstaciones MDI;
        internal Entidad.EstacionServicio EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Direccion
        {
            get { return mmoDireccion.Text; }
            set { mmoDireccion.Text = value; }
        }
        public string Firma
        {
            get { return memoFirmaEECC.Text; }
            set { memoFirmaEECC.Text = value; }
        }

        public int IDZona
        {
            get { return Convert.ToInt32(lkeZona.EditValue); }
            set { lkeZona.EditValue = value; }
        }

        public int IDAdmin
        {
            get { return Convert.ToInt32(lkeAdministrador.EditValue); }
            set { lkeAdministrador.EditValue = value; }
        }

        public int IDResCont
        {
            get { return Convert.ToInt32(lkeRespContable.EditValue); }
            set { lkeRespContable.EditValue = value; }
        }

        public int IDArqueador
        {
            get { return Convert.ToInt32(lkArqueador.EditValue); }
            set { lkArqueador.EditValue = value; }
        }

        public int NroTurnos
        {
            get { return Convert.ToInt32(spNumeroTurnos.Value); }
            set { spNumeroTurnos.Value = value; }
        }

        public string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        public string Telefono
        {
            get { return txtTelefono.Text; }
            set { txtTelefono.Text = value; }
        }

        public bool AplicarFechaArqueo
        {
            get { return Convert.ToBoolean(chkAplicarFechaArqueo.Checked); }
            set { chkAplicarFechaArqueo.Checked = value; }
        }

        public bool FormatoCosta
        {
            get { return Convert.ToBoolean(chkFormato.Checked); }
            set { chkFormato.Checked = value; }
        }
        
        public bool AplicaRetencionAlcaldia
        {
            get { return Convert.ToBoolean(chkAplicaRetencionAlcaldia.Checked); }
            set { chkAplicaRetencionAlcaldia.Checked = value; }
        }

        public bool EsInventarioCombustibleCero
        {
            get { return Convert.ToBoolean(chkInventarioCombustibleCero.Checked); }
            set { chkInventarioCombustibleCero.Checked = value; }
        }

        private int IDProveedor
        {
            get { return Convert.ToInt32(lkCCh.EditValue); }
            set { lkCCh.EditValue = value; }
        }

        public int CIA
        {
            get { return Convert.ToInt32(this.glkCIA.EditValue); }
            set { this.glkCIA.EditValue = value; }
        }

        public int CIP
        {
            get { return Convert.ToInt32(this.glkCIP.EditValue); }
            set { this.glkCIP.EditValue = value; }
        }
        public int Coperativa
        {
            get { return Convert.ToInt32(this.glkOperativo.EditValue); }
            set { this.glkOperativo.EditValue = value; }
        }        
        public decimal Variacion
        {
            get { return Convert.ToDecimal(spVariacion.Value); }
            set { spVariacion.Value = value; }
        }

        public decimal Diferencias
        {
            get { return Convert.ToDecimal(spDiferencia.Value); }
            set { spDiferencia.Value = value; }
        }

        internal List<Entidad.EstacionAreaPermisible> P = new List<Entidad.EstacionAreaPermisible>();
        internal List<Entidad.EstacionAreaPermisible> EtPermisible
        {
            get { return P; }
            set
            {
                P = value;
                this.bdsPermisible.DataSource = this.P;
            }
        }


        #endregion

        #region *** INICIO ***

        public DialogEstaciones()
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

                lkeZona.Properties.DataSource = from z in db.Zonas where z.Activo select new {z.ID, z.Nombre };
                lkeZona.Properties.DisplayMember = "Nombre";
                lkeZona.Properties.ValueMember = "ID";

                lkeAdministrador.Properties.DataSource = from e in db.Empleados where e.Activo select new { e.ID, Display = e.Nombres + " " + e.Apellidos };
                lkeAdministrador.Properties.DisplayMember = "Display";
                lkeAdministrador.Properties.ValueMember = "ID";

                lkeRespContable.Properties.DataSource = from e in db.Empleados where e.Activo select new { e.ID, Display = e.Nombres + " " + e.Apellidos };
                lkeRespContable.Properties.DisplayMember = "Display";
                lkeRespContable.Properties.ValueMember = "ID";

                lkArqueador.Properties.DataSource = from e in db.Empleados where e.Activo select new { e.ID, Display = e.Nombres + " " + e.Apellidos };
                lkArqueador.Properties.DisplayMember = "Display";
                lkArqueador.Properties.ValueMember = "ID";

                rpLkArea.DataSource = db.Areas.Where(o => o.Activo && !o.EsServicio).Select(s => new { s.ID, s.Nombre }).ToList();
                EtPermisible = db.EstacionAreaPermisible.Where(o => o.EstacionID.Equals(0)).ToList();

                glkCIA.Properties.DataSource = glkCIP.Properties.DataSource = glkOperativo.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                
                //glkCIP.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                
                 var listacch = (from p in db.Proveedors
                              where p.Activo
                              select new { p.ID, p.Codigo, p.Nombre, Display = p.Codigo + " | " + p.Nombre}).ToList();
                 listacch.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                 lkCCh.Properties.DataSource = listacch;
                lkCCh.Properties.DisplayMember = "Display";
                lkCCh.Properties.ValueMember = "ID";
                IDProveedor = 0;

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Direccion = EntidadAnterior.Direccion;
                    IDZona = EntidadAnterior.ZonaID;
                    Codigo = EntidadAnterior.Codigo;
                    IDAdmin = EntidadAnterior.AdministradorID;
                    IDResCont = EntidadAnterior.ResponsableContableID;
                    Telefono = EntidadAnterior.Telefono;
                    IDArqueador = EntidadAnterior.ArqueadorID;
                    NroTurnos = EntidadAnterior.NumeroTurnos;
                    AplicarFechaArqueo = EntidadAnterior.AplicarFechaArqueo;
                    dateArqueo.EditValue = EntidadAnterior.FechaArqueo;
                    AplicaRetencionAlcaldia = EntidadAnterior.AplicaRetencionAlcaldia;
                    IDProveedor = EntidadAnterior.ProveedorCajaChicaID;
                    CIA = EntidadAnterior.CuentaInternaActivo;
                    CIP = EntidadAnterior.CuentaInternaPasivo;
                    Variacion = EntidadAnterior.PermisibleActaVariacion;
                    Diferencias = EntidadAnterior.PermisibleActaDiferencia;
                    EsInventarioCombustibleCero = EntidadAnterior.InventarioCombustibleCero;
                    Firma = EntidadAnterior.FirmaEstadoCuenta;
                    Coperativa = EntidadAnterior.CuentaPagoOperativoID;
                    FormatoCosta = EntidadAnterior.FormatoCosta;
                    EtPermisible = db.EstacionAreaPermisible.Where(o => o.EstacionID.Equals(EntidadAnterior.ID)).ToList();
                }

                this.bdsPermisible.DataSource = this.EtPermisible;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeZona, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeAdministrador, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeZona.EditValue == null || txtCodigo.Text == "" || lkeAdministrador.EditValue == null || Convert.ToInt32(lkeAdministrador.EditValue) == 0 || Convert.ToInt32(lkCCh.EditValue) <= 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarLogin(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código de la Estación de Servicio '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            if (this.chkAplicarFechaArqueo.Checked)
            {
                if (dateArqueo.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccinar la nueva fecha de inicio para los arqueos.", Parametros.MsgType.warning);
                    return false;
                }
            }
            

            return true;
        }

        private bool ValidarLogin(string code, int? ID)
        {
            var result = (from i in db.EstacionServicios
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
                    Entidad.EstacionServicio ES;

                    if (Editable)
                    { ES = db.EstacionServicios.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        ES = new Entidad.EstacionServicio();
                        ES.Activo = true;

                    }

                    ES.Nombre = Nombre;
                    ES.Direccion = Direccion;
                    ES.ZonaID = IDZona;
                    ES.Codigo = Codigo;
                    ES.Telefono = Telefono;
                    ES.AdministradorID = IDAdmin;
                    ES.ResponsableContableID = IDResCont;
                    ES.ArqueadorID = IDArqueador;
                    ES.NumeroTurnos = NroTurnos;
                    ES.ProveedorCajaChicaID = IDProveedor;
                    ES.CuentaInternaActivo = CIA;
                    ES.CuentaInternaPasivo = CIP;
                    ES.AplicaRetencionAlcaldia = AplicaRetencionAlcaldia;
                    ES.AplicarFechaArqueo = AplicarFechaArqueo;
                    ES.PermisibleActaVariacion = Variacion;
                    ES.PermisibleActaDiferencia = Diferencias;
                    ES.FirmaEstadoCuenta = Firma;
                    ES.InventarioCombustibleCero = EsInventarioCombustibleCero;
                    ES.CuentaPagoOperativoID = Coperativa;
                    ES.FormatoCosta = FormatoCosta;

                    if (dateArqueo.EditValue != null)
                        ES.FechaArqueo = Convert.ToDateTime(dateArqueo.EditValue);

                    db.EstacionAreaPermisible.DeleteAllOnSubmit(db.EstacionAreaPermisible.Where(o => o.EstacionID.Equals(ES.ID)));

                    EtPermisible.ForEach(det =>
                        {
                            if (det.AreaID != null)
                            {
                                if (!det.AreaID.Equals(0))
                                {
                                    db.EstacionAreaPermisible.InsertOnSubmit(new Entidad.EstacionAreaPermisible { AreaID = det.AreaID, EstacionID = ES.ID, Permisible = det.Permisible });
                                    db.SubmitChanges();
                                }
                            }

                        });
              
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(ES, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó la Estación de Servicio: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);
                    }
                    else
                    {
                        db.EstacionServicios.InsertOnSubmit(ES);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó la Estación de Servicios: " + ES.Nombre, this.Name);
                    }

                    db.SubmitChanges();

                    if (!Editable)//Creación de la Serie                    
                        db.Series.InsertOnSubmit(new Entidad.Series { NumeroInicial = 1, NumeroActual = 0, EstacionServicioID = ES.ID, NumeroFinal = 0, MovimientoTipoID = 41 });

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
        
        private void txtCodigo_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void chkAplicarFechaArqueo_CheckedChanged(object sender, EventArgs e)
        {
            if (Editable)
            {
                if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(EntidadAnterior.ID)) > 0)
                {
                    if (chkAplicarFechaArqueo.Checked)
                    {
                        Parametros.General.DialogMsg("La Estación de Servicio seleccionada tiene sub estaciones asignadas, debe de asignar la fecha de arqueo a la Sub Estación.", Parametros.MsgType.warning);
                        this.chkAplicarFechaArqueo.Checked = false;
                    }
                    
                    return;
                }
            }

            if (chkAplicarFechaArqueo.Checked)
                this.layoutControlItemFecha.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
            {
                this.layoutControlItemFecha.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.dateArqueo.EditValue = null;
            }
        }

        private void gvDataTC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }
                if (e.KeyCode == Keys.Delete)
                {

                    if (gvDataTC.FocusedRowHandle >= 0)
                    {
                        int _id = Convert.ToInt32(gvDataTC.GetFocusedRowCellValue("AreaID"));

                        if (_id > 0)
                        {
                            var obj = EtPermisible.SingleOrDefault(o => o.AreaID.Equals(_id));

                            if (obj != null)
                                EtPermisible.Remove(obj);
                        }

                        gvDataTC.RefreshData();

                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void gvDataTC_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column == colArea)
            {
                if (gvDataTC.GetRowCellValue(e.RowHandle, "AreaID") != null)
                {
                    if (Convert.ToInt32(gvDataTC.GetRowCellValue(e.RowHandle, "AreaID")) == 0)
                    {
                        return;
                    }
                    else if (EtPermisible.Count(d => d.AreaID.Equals(Convert.ToInt32(gvDataTC.GetRowCellValue(e.RowHandle, "AreaID")))) > 1)
                    {
                        Parametros.General.DialogMsg("El área seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        gvDataTC.SetRowCellValue(e.RowHandle, "AreaID", 0);
                        gvDataTC.FocusedColumn = colArea;
                        return;
                    }
                }
            }
        }

        #endregion
    }
}