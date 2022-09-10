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
    public partial class DialogCuentaBancaria : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCuentaBancaria MDI;
        internal Entidad.CuentaBancaria EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;
        private int UsuarioID = 0;
        private string route;
        private string chequeName = "";
        private string ExtensionName = "";

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private int Banco
        {
            get { return Convert.ToInt32(this.lkBanco.EditValue); }
            set { this.lkBanco.EditValue = value; }
        }

        private string Comentario
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }

        private string Numero
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        private string Clave
        {
            get { return txtClave.Text; }
            set { txtClave.Text = value; }
        }

        public int CC
        {
            get { return Convert.ToInt32(this.glkCC.EditValue); }
            set { this.glkCC.EditValue = value; }
        }

        private int Estacion
        {
            get { return Convert.ToInt32(this.lkeEstacionServicio.EditValue); }
            set { this.lkeEstacionServicio.EditValue = value; }
        }

        private string Archivo
        {
            get { return (this.cboFormatos.SelectedItem == null ? "" : this.cboFormatos.SelectedItem.ToString()); }
            set { this.cboFormatos.SelectedItem = value; }
        }


        private int Moneda
        {
            get { return Convert.ToInt32(this.lkMoneda.EditValue); }
            set { this.lkMoneda.EditValue = value; }
        }

        private int Cantidad
        {
            get { return Convert.ToInt32(this.spMinima.Value); }
            set { this.spMinima.Value = value; }
        }

        private string Siglas
        {
            get { return txtSiglas.Text; }
            set { txtSiglas.Text = value; }
        }
                
        #endregion

        #region *** INICIO ***

        public DialogCuentaBancaria(int UserID)
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
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                
                lkBanco.Properties.DataSource = db.Bancos.Where(b => b.Activo);
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;
                lkMoneda.Properties.DataSource = db.Monedas.Select(s => new { s.ID, Display = s.Simbolo + " | " + s.Nombre});
                
                var formas = db.FormatosCheques.Select(s => s.ArchivoNombre).ToList();
                formas.ForEach( det =>
                    {
                     cboFormatos.Properties.Items.Add(det);
                    });
                //System.Object[] ItemObject = new System.Object[10];
                //for (int i = 0; i <= 9; i++)
                //{
                //    ItemObject[i] = "Item" + i;
                //}
                //listBox1.Items.AddRange(ItemObject);
                //cboFormatos.Properties.Items.AddRange(.ToList());

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Numero = EntidadAnterior.Codigo;
                    Clave = EntidadAnterior.ClaveBanco;
                    Comentario = EntidadAnterior.Comentario;
                    Banco = EntidadAnterior.BancoID;
                    Estacion = EntidadAnterior.EstacionServicioID;
                    CC = EntidadAnterior.CuentaContableID;                    
                    Moneda = EntidadAnterior.MonedaID;
                    Cantidad = EntidadAnterior.Minimo;
                    Siglas = EntidadAnterior.Siglas;
                    Archivo = EntidadAnterior.ArchivoNombre;

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
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkBanco, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtClave, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtCodigo.Text) || String.IsNullOrEmpty(txtClave.Text) || lkBanco.EditValue == null || lkeEstacionServicio.EditValue == null || lkMoneda.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkCC.EditValue) < 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Cuenta Contable.", Parametros.MsgType.warning);
                return false;
            }

            //******** YA NO HAY REPORTE ********************//

            //if (!Editable && String.IsNullOrEmpty(route))
            //{
            //    Parametros.General.DialogMsg("Debe de seleccionar el archivo de REPORTE" + Environment.NewLine, Parametros.MsgType.warning);
            //    return false;
            //}

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
                    Entidad.CuentaBancaria cuenta;

                    if (Editable)
                    {
                        cuenta = db.CuentaBancarias.Single(e => e.ID.Equals(EntidadAnterior.ID));
                        
                        //if (!String.IsNullOrEmpty(route))
                        //{
                        //    cuenta.Archivo = File.ReadAllBytes(route);
                        //    cuenta.Fecha = Convert.ToDateTime(db.GetDateServer());
                        //    cuenta.ArchivoNombre = chequeName;
                        //    cuenta.Extension = ExtensionName;
                        //}
                    }
                    else 
                    {
                        cuenta = new Entidad.CuentaBancaria();
                        cuenta.Activo = true;
                       // cuenta.Archivo = File.ReadAllBytes(route);
                        cuenta.Fecha = Convert.ToDateTime(db.GetDateServer());
                        //cuenta.ArchivoNombre = chequeName;
                        //cuenta.Extension = ExtensionName;
                    }

                    cuenta.ArchivoNombre = (String.IsNullOrEmpty(Archivo) ? "" : Archivo);
                    cuenta.Nombre = Nombre;
                    cuenta.Codigo = Numero;
                    cuenta.ClaveBanco = Clave;
                    cuenta.Comentario = Comentario;
                    cuenta.BancoID = Banco;
                    cuenta.CuentaContableID = CC;
                    cuenta.EstacionServicioID = Estacion;
                    cuenta.MonedaID = Moneda;
                    cuenta.Minimo = Cantidad;
                    cuenta.Siglas = Siglas;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(cuenta, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                         "Se modificó la Cuenta Bancaria: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.CuentaBancarias.InsertOnSubmit(cuenta);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se creó la Cuenta Bancaria: " + cuenta.Nombre, this.Name);
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
            MDI.CleanDialog(ShowMsg, false, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkeEstacionServicio_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
                Parametros.General.ValidateDifES((BaseEdit)sender, infDifES);

                CC = 0;
                glkCC.Properties.DataSource = null;

                var lista = (from cc in db.CuentaContables
                             join ces in db.CuentaContableEstacions on cc equals ces.CuentaContable
                             where cc.Activo && cc.Detalle && ces.EstacionID.Equals(Estacion) select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                glkCC.Properties.DataSource = lista;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //ofdReport.Filter = "Reportes |*.repx";

                if (ofdReport.ShowDialog() == DialogResult.OK)
                {
                    this.btnUpload.Text = Path.GetFileName(@ofdReport.FileName);
                    this.chequeName = Path.GetFileNameWithoutExtension(@ofdReport.FileName);
                    this.ExtensionName = Path.GetExtension(@ofdReport.FileName);
                    route = @ofdReport.FileName;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "ERROR AL CARGAR EL ARCHIVO" + ex.Message);
            }
        }
        
        #endregion

    }
    
}