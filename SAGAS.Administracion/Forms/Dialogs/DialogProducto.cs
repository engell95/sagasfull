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
    public partial class DialogProducto : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormProducto MDI;
        internal Entidad.Producto EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int IDClaseProducto
        {
            get { return Convert.ToInt32(lkeClase.EditValue); }
            set { lkeClase.EditValue = value; }
        }

        public decimal MargenCosto
        {
            get { return Convert.ToDecimal(spMargen.Value); }
            set { spMargen.Value = value; }
        }

        public int IDUnidadMedida
        {
            get { return Convert.ToInt32(lkeUnidadMedida.EditValue); }
            set { lkeUnidadMedida.EditValue = value; }
        }

        private Byte[] Imagen
        {
            get
            {
                if (picImagen.Image == null) return null;
                else return Parametros.General.ImageToBytes(picImagen.Image);
            }
            set
            {
                if (value == null) picImagen.Image = null;
                else picImagen.Image = Parametros.General.BytesToImage(value);
            }
        }

        public bool ExentoIVA
        {
            get { return Convert.ToBoolean(chkExentoIVA.Checked); }
            set { chkExentoIVA.Checked = value; }
        }

        public bool EsSiempreSalidaVentas
        {
            get { return Convert.ToBoolean(chkSiempreSalidaVentas.Checked); }
            set { chkSiempreSalidaVentas.Checked = value; }
        }

        public bool AplicaDescuento
        {
            get { return Convert.ToBoolean(chkAplicaDescuento.Checked); }
            set { chkAplicaDescuento.Checked = value; }
        }
        
        private bool ISC
        {
            get { return Convert.ToBoolean(chkISC.Checked); }
            set { chkISC.Checked = value; }
        }

        public bool EsServicio
        {
            get { return Convert.ToBoolean(chkServicio.Checked); }
            set { chkServicio.Checked = value; }
        }

        public bool EsConversion
        {
            get { return Convert.ToBoolean(chkConversion.Checked); }
            set { chkConversion.Checked = value; }
        }

        public int CCV
        {
            get { return Convert.ToInt32(this.glkCCV.EditValue); }
            set { this.glkCCV.EditValue = value; }
        }

        public int CCI
        {
            get { return Convert.ToInt32(this.glkCCI.EditValue); }
            set { this.glkCCI.EditValue = value; }
        }

        public int CCC
        {
            get { return Convert.ToInt32(this.glkCCC.EditValue); }
            set { this.glkCCC.EditValue = value; }
        }

        public int CCD
        {
            get { return Convert.ToInt32(this.glkCCD.EditValue); }
            set { this.glkCCD.EditValue = value; }
        }

        public int CSob
        {
            get { return Convert.ToInt32(this.glkCSob.EditValue); }
            set { this.glkCSob.EditValue = value; }
        }
        public int CFal
        {
            get { return Convert.ToInt32(this.glkCFal.EditValue); }
            set { this.glkCFal.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogProducto()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl(); 
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                lkeClase.Properties.DataSource = from e in db.ProductoClases where e.Activo select new { e.ID, e.Nombre };
                lkeClase.Properties.DisplayMember = "Nombre";
                lkeClase.Properties.ValueMember = "ID";

                lkeUnidadMedida.Properties.DataSource = from u in db.UnidadMedidas where u.Activo select new { u.ID, u.Nombre };
                lkeUnidadMedida.Properties.DisplayMember = "Nombre";
                lkeUnidadMedida.Properties.ValueMember = "ID";

                var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                glkCCV.Properties.DataSource = lista;
                //    from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCV.Properties.DisplayMember = "Display";
                //glkCCV.Properties.ValueMember = "ID";

                glkCCI.Properties.DataSource = lista;
                //    from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCI.Properties.DisplayMember = "Display";
                //glkCCI.Properties.ValueMember = "ID";

                glkCCC.Properties.DataSource = lista;
                //    from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCC.Properties.DisplayMember = "Display";
                //glkCCC.Properties.ValueMember = "ID";

                glkCCD.Properties.DataSource = lista;
                //    from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCD.Properties.DisplayMember = "Display";
                //glkCCD.Properties.ValueMember = "ID";

                glkCSob.Properties.DataSource = lista;
                glkCFal.Properties.DataSource = lista;

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    Comentario = EntidadAnterior.Comentario;                    
                    IDClaseProducto = EntidadAnterior.ProductoClaseID;
                    IDUnidadMedida = EntidadAnterior.UnidadMedidaID;
                    Imagen = EntidadAnterior.Imagen;
                    ExentoIVA = EntidadAnterior.ExentoIVA;
                    AplicaDescuento = EntidadAnterior.AplicaDescuento;
                    EsServicio = EntidadAnterior.EsServicio;
                    EsConversion = EntidadAnterior.Conversion;
                    CCV = EntidadAnterior.CuentaVentaID;
                    CCI = EntidadAnterior.CuentaInventarioID;
                    CCC = EntidadAnterior.CuentaCostoID;
                    CCD = EntidadAnterior.CuentaDescuentoID;
                    ISC = EntidadAnterior.AplicaISC;
                    MargenCosto = EntidadAnterior.MargenToleranciaCosto;
                    EsSiempreSalidaVentas = EntidadAnterior.SiempreSalidaVentas;
                    CSob = EntidadAnterior.CuentaSobranteID;
                    CFal = EntidadAnterior.CuentaFaltanteID;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "" || lkeClase.EditValue == null || lkeUnidadMedida.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Producto '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Productos
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
                    Entidad.Producto P;

                    if (Editable)
                    { P = db.Productos.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        P = new Entidad.Producto();
                        P.Activo = true;
                    }

                    P.Nombre = Nombre;
                    P.Codigo = Codigo; 
                    P.Comentario = Comentario;
                    P.ProductoClaseID = IDClaseProducto;
                    P.UnidadMedidaID = IDUnidadMedida;
                    P.Imagen = Imagen;
                    P.ExentoIVA = ExentoIVA;
                    P.AplicaDescuento = AplicaDescuento;
                    P.EsServicio = EsServicio;
                    P.AplicaISC = ISC;
                    P.MargenToleranciaCosto = MargenCosto;
                    P.SiempreSalidaVentas = EsSiempreSalidaVentas;
                    P.Conversion = EsConversion;
                    
                    if (glkCCV.Properties.ReadOnly.Equals(false))
                        P.CuentaVentaID = CCV;

                    if (glkCCI.Properties.ReadOnly.Equals(false))
                        P.CuentaInventarioID = CCI;

                    if (glkCCC.Properties.ReadOnly.Equals(false))
                        P.CuentaCostoID = CCC;

                    if (glkCCD.Properties.ReadOnly.Equals(false))
                        P.CuentaDescuentoID = CCD;

                    if (glkCSob.Properties.ReadOnly.Equals(false))
                        P.CuentaSobranteID = CSob;

                    if (glkCFal.Properties.ReadOnly.Equals(false))
                        P.CuentaFaltanteID = CFal;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(P, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó el Producto: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Productos.InsertOnSubmit(P);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Producto: " + P.Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    EntidadAnterior = null;
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

        private void lkeClase_EditValueChanged(object sender, EventArgs e)
        {
            if (lkeClase.EditValue != null)
            {
                if (Convert.ToInt32(lkeClase.EditValue) > 0)
                {
                    var obj = db.Areas.Where(a => a.ID.Equals(Convert.ToInt32(db.ProductoClases.Single(c => c.ID.Equals(Convert.ToInt32(lkeClase.EditValue))).AreaID)));
                    txtArea.Text = obj.First().Nombre;

                    if (obj.First().CuentaVentaID > 0)
                    {
                        glkCCV.Properties.ReadOnly = true;
                        CCV = obj.First().CuentaVentaID;
                    }
                    else
                    {
                        glkCCV.Properties.ReadOnly = false;
                        CCV = 0;
                    }

                    if (obj.First().CuentaInventarioID > 0)
                    {
                        glkCCI.Properties.ReadOnly = true;
                        CCI = obj.First().CuentaInventarioID;
                    }
                    else
                    {
                        glkCCI.Properties.ReadOnly = false;
                        CCI = 0;
                    }

                    if (obj.First().CuentaCostoID > 0)
                    {
                        glkCCC.Properties.ReadOnly = true;
                        CCC = obj.First().CuentaCostoID;
                    }
                    else
                    {
                        glkCCC.Properties.ReadOnly = false;
                        CCC = 0;
                    }

                    if (obj.First().CuentaDescuentoID > 0)
                    {
                        glkCCD.Properties.ReadOnly = true;
                        CCD = obj.First().CuentaDescuentoID;
                    }
                    else
                    {
                        glkCCD.Properties.ReadOnly = false;
                        CCD = 0;
                    }

                    if (obj.First().CuentaSobranteID > 0)
                    {
                        glkCSob.Properties.ReadOnly = true;
                        CSob = obj.First().CuentaSobranteID;
                    }
                    else
                    {
                        glkCSob.Properties.ReadOnly = false;
                        CSob = 0;
                    }

                    if (obj.First().CuentaFaltanteID > 0)
                    {
                        glkCFal.Properties.ReadOnly = true;
                        CFal = obj.First().CuentaFaltanteID;
                    }
                    else
                    {
                        glkCFal.Properties.ReadOnly = false;
                        CFal = 0;
                    }
                }
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
    }
}