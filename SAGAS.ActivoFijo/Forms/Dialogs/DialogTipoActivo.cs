using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogTipoActivo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTipoActivo MDI;
        internal Entidad.TipoActivo EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        DXErrorProvider errRequiredField = new DXErrorProvider();
        

        public int Codigo
        {
            get { return Convert.ToInt32(txtCodigo.Text); }
            set { txtCodigo.EditValue = value; }
        }

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }
        
        public string Descripcion
        {
            get { return mmoDescripcion.Text; }
            set { mmoDescripcion.Text = value; }
        }

        public bool EsTangible 
        {
            get { return chkTangible.Checked; }
            set { chkTangible.Checked = value; }        
        }

        public bool EsDepreciable
        {
            get { return chkDepreciable.Checked; }
            set { chkDepreciable.Checked = value; }
        }

        public int CuentaActivo 
        {
            get { return Convert.ToInt32(this.glkCuentaActivo.EditValue); }
            set { this.glkCuentaActivo.EditValue = value; }        
        }

        public int CuentaGasto
        {
            get { return Convert.ToInt32(this.glkCuentaGasto.EditValue); }
            set { this.glkCuentaGasto.EditValue = value; }
        }
        
        public int CuentaDepreciacionAcumulada
        {
            get { return Convert.ToInt32(this.glkCuentaDepreciacionAcumulada.EditValue); }
            set { this.glkCuentaDepreciacionAcumulada.EditValue = value; }
        }

        public int VidaUtil
        {
            get { return Convert.ToInt32(this.seVidaUtil.Value); }
            set { this.seVidaUtil.Value = value; }
        }
        
        #endregion

        public DialogTipoActivo()
        {
            InitializeComponent();
        }

        private void MyFormTipoActivo_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField(); 
        }

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //**Cuentas
                var lista = (from cc in db.CuentaContables where cc.Activo && cc.Detalle 
                             select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();

                lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "---", Display = "--- | N / A" });

                glkCuentaActivo.Properties.DataSource = lista;                
                glkCuentaGasto.Properties.DataSource = lista;  
                glkCuentaDepreciacionAcumulada.Properties.DataSource =  lista;
                
                if (Editable)
                {
                    Codigo = EntidadAnterior.Codigo;
                    Nombre = EntidadAnterior.Nombre;                   
                    Descripcion = EntidadAnterior.Descripcion;
                    EsTangible = EntidadAnterior.EsTangible;
                    EsDepreciable = EntidadAnterior.EsDepreciable;
                    CuentaActivo = EntidadAnterior.CuentaActivo;
                    CuentaGasto = EntidadAnterior.CuentaGasto;
                    CuentaDepreciacionAcumulada = EntidadAnterior.CuentaDepreciacionAcumulada;
                    VidaUtil = EntidadAnterior.VidaUtil;
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

            if (!ValidarCodigo(Convert.ToInt32(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Tipo Activo '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }


        private bool ValidarCodigo(int code, int? ID)
        {
            var result = (from i in db.TipoActivo
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
                    Entidad.TipoActivo TipoActivo;

                    if (Editable)
                    { 
                        TipoActivo = db.TipoActivo.Single(e => e.ID == EntidadAnterior.ID); 
                    }
                    
                    else
                    {
                        TipoActivo = new Entidad.TipoActivo();
                        TipoActivo.Activo = true;
                    }

                    TipoActivo.Codigo = Codigo;
                    TipoActivo.Nombre = Nombre;
                    TipoActivo.Descripcion = Descripcion;
                    TipoActivo.EsTangible = EsTangible;
                    TipoActivo.EsDepreciable = EsDepreciable;
                    TipoActivo.CuentaActivo = CuentaActivo;
                    TipoActivo.CuentaGasto = CuentaGasto;
                    TipoActivo.CuentaDepreciacionAcumulada = CuentaDepreciacionAcumulada;
                    TipoActivo.VidaUtil = VidaUtil;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(TipoActivo, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,"Se modificó el Tipo de Activo: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.TipoActivo.InsertOnSubmit(TipoActivo);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                        "Se creó el Tipo de Activo: " + TipoActivo.Nombre, this.Name);

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

        private void btnGuardar_Click(object sender, EventArgs e)
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
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.TextEdit)sender, errRequiredField);
        }

        #endregion 
    }
}
