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
    public partial class DialogSubEstacion : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormSubEstacion MDI;
        internal Entidad.SubEstacion EntidadAnterior;
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

        public int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
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

        private int IDProveedor
        {
            get { return Convert.ToInt32(lkCCh.EditValue); }
            set { lkCCh.EditValue = value; }
        }
        
        #endregion

        #region *** INICIO ***

        public DialogSubEstacion()
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

                lkES.Properties.DataSource = from es in db.EstacionServicios where es.Activo select new {es.ID, es.Nombre };
                lkES.Properties.DisplayMember = "Nombre";
                lkES.Properties.ValueMember = "ID";

                glkCIA.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };

                glkCIP.Properties.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                
                lkArqueador.Properties.DataSource = from e in db.Empleados where e.Activo select new { e.ID, Display = e.Nombres + " " + e.Apellidos };
                lkArqueador.Properties.DisplayMember = "Display";
                lkArqueador.Properties.ValueMember = "ID";

                var listacch = (from p in db.Proveedors
                               where p.Activo
                               select new { p.ID, p.Codigo, p.Nombre, Display = p.Codigo + " | " + p.Nombre }).ToList();

                listacch.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                lkCCh.Properties.DataSource = listacch;

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Direccion = EntidadAnterior.Direccion;
                    Codigo = EntidadAnterior.Codigo;
                    Telefono = EntidadAnterior.Telefono;
                    IDArqueador = EntidadAnterior.ArqueadorID;
                    NroTurnos = EntidadAnterior.NumeroTurnos;
                    IDEstacion = EntidadAnterior.EstacionServicioID;
                    AplicarFechaArqueo = EntidadAnterior.AplicarFechaArqueo;
                    dateArqueo.EditValue = EntidadAnterior.FechaArqueo;
                    CIA = EntidadAnterior.CuentaInternaActivo;
                    CIP = EntidadAnterior.CuentaInternaPasivo;
                    IDProveedor = EntidadAnterior.ProveedorCajaChicaID;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkES, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkES.EditValue == null || txtCodigo.Text == "" || lkArqueador.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.SubEstacion SES;

                    if (Editable)
                    { SES = db.SubEstacions.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        SES = new Entidad.SubEstacion();
                        SES.Activo = true;
                    }

                    SES.Nombre = Nombre;
                    SES.Direccion = Direccion;
                    SES.Codigo = Codigo;
                    SES.Telefono = Telefono;
                    SES.EstacionServicioID = IDEstacion;
                    SES.ArqueadorID = IDArqueador;
                    SES.NumeroTurnos = NroTurnos;
                    SES.CuentaInternaActivo = CIA;
                    SES.CuentaInternaPasivo = CIP;
                    SES.AplicarFechaArqueo = AplicarFechaArqueo;
                    SES.ProveedorCajaChicaID = IDProveedor;
                    if (dateArqueo.EditValue != null)
                    SES.FechaArqueo = Convert.ToDateTime(dateArqueo.EditValue);
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(SES, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó la Sub Estación de Servicio: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.SubEstacions.InsertOnSubmit(SES);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó la Sub Estación: " + SES.Nombre, this.Name);
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

        #endregion        

        private void txtCodigo_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void chkAplicarFechaArqueo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicarFechaArqueo.Checked)
                this.layoutControlItemFecha.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
            {
                this.layoutControlItemFecha.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.dateArqueo.EditValue = null;
            }
        }
        
    }
}