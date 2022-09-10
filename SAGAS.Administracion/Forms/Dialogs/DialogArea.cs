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
    public partial class DialogArea : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormArea MDI;
        internal Entidad.Area EntidadAnterior;
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

        //public bool OrdenCompra
        //{
        //    get { return Convert.ToBoolean(chkOrdenCompra.Checked); }
        //    set { chkOrdenCompra.Checked = value; }
        //}

        public bool EsServicio
        {
            get { return Convert.ToBoolean(chkEsServicio.Checked); }
            set { chkEsServicio.Checked = value; }
        }

        public int Costo
        {
            get { return Convert.ToInt32(this.lkCentroCosto.EditValue); }
            set { this.lkCentroCosto.EditValue = value; }
        }

        public int CCSob
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

        public DialogArea()
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

                //**Cuentas
                var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                glkCCV.Properties.DataSource = lista;
                    //from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCV.Properties.DisplayMember = "Display";
                //glkCCV.Properties.ValueMember = "ID";

                glkCCI.Properties.DataSource = lista;
                    //from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCI.Properties.DisplayMember = "Display";
                //glkCCI.Properties.ValueMember = "ID";

                glkCCC.Properties.DataSource = lista;
                    //from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCC.Properties.DisplayMember = "Display";
                //glkCCC.Properties.ValueMember = "ID";

                glkCCD.Properties.DataSource = lista;
                    //from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre };
                //glkCCD.Properties.DisplayMember = "Display";
                //glkCCD.Properties.ValueMember = "ID";

                glkCSob.Properties.DataSource = lista;
                glkCFal.Properties.DataSource = lista;

                lkCentroCosto.Properties.DataSource = from ct in db.CentroCostos where ct.Activo select new { ct.ID, Display = ct.Codigo + " | " + ct.Nombre };
                

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    Comentario = EntidadAnterior.Comentario;
                    CCV = EntidadAnterior.CuentaVentaID;
                    CCI = EntidadAnterior.CuentaInventarioID;
                    CCC = EntidadAnterior.CuentaCostoID;
                    CCD = EntidadAnterior.CuentaDescuentoID;
                    Costo = EntidadAnterior.CentroCostoID;
                    //OrdenCompra = EntidadAnterior.AplicaOrdenCompra;
                    CCSob = EntidadAnterior.CuentaSobranteID;
                    CFal = EntidadAnterior.CuentaFaltanteID;
                    EsServicio = EntidadAnterior.EsServicio;
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
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

             if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Área '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }             

            return true;
        }


        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Areas
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
                    Entidad.Area A;

                    if (Editable)
                    { A = db.Areas.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        A = new Entidad.Area();
                        A.Activo = true;
                    }

                    A.Codigo = Codigo;
                    A.Nombre = Nombre;
                    A.Comentario = Comentario;
                    A.CuentaVentaID = CCV;
                    A.CuentaInventarioID = CCI;
                    A.CuentaCostoID = CCC;
                    A.CuentaDescuentoID = CCD;
                    A.CentroCostoID = Costo;
                    //A.AplicaOrdenCompra = OrdenCompra;
                    A.CuentaSobranteID = CCSob;
                    A.CuentaFaltanteID = CFal;
                    A.EsServicio = EsServicio;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(A, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó el Área: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Areas.InsertOnSubmit(A);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Área: " + A.Nombre, this.Name);

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
            Parametros.General.ValidateEmptyStringRule((TextEdit)sender, errRequiredField);
        }

        #endregion 

    }
}