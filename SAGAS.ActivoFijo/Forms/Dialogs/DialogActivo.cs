using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogActivo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormActivo MDI;
        internal Entidad.Activo EntidadAnterior;
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

        public int TipoActivoID
        {
            get { return Convert.ToInt32(this.glkTipoActivo.EditValue); }
            set { this.glkTipoActivo.EditValue = value; }      
        }

        public string Descripcion
        {
            get { return mmoDescripcion.Text; }
            set { mmoDescripcion.Text = value; }
        }

        #endregion


        public DialogActivo()
        {
            InitializeComponent();
        }

        private void MyFormActivo_Load(object sender, EventArgs e)
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

                //**TipoActivo
                var lista = (from cc in db.TipoActivo.ToList()
                             where cc.Activo 
                             select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo.ToString("00") + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                
                //lista.Insert(0, new { ID = 0, Codigo = "---", Nombre = "N / A", Display = "--- | N / A" });

                glkTipoActivo.Properties.DataSource = lista;                
               
                if (Editable)
                {
                    Codigo = EntidadAnterior.Codigo;
                    Nombre = EntidadAnterior.Nombre;
                    Descripcion = EntidadAnterior.Descripcion;
                    TipoActivoID = EntidadAnterior.TipoActivoID;
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
            Parametros.General.ValidateEmptyStringRule(glkTipoActivo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || txtCodigo.Text == "" || TipoActivoID <= 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarCodigo(Convert.ToInt32(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Activo '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarCodigo(int code, int? ID)
        {
            var result = (from i in db.Activo
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
                    Entidad.Activo Activo;

                    if (Editable)
                    {
                        Activo = db.Activo.Single(e => e.ID == EntidadAnterior.ID);
                    }

                    else
                    {
                        Activo = new Entidad.Activo();
                        Activo.Activado = true;
                    }

                    Activo.Codigo = Codigo;
                    Activo.Nombre = Nombre;
                    Activo.Descripcion = Descripcion;
                    Activo.TipoActivoID = TipoActivoID;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(Activo, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se modificó el Tipo de Activo: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Activo.InsertOnSubmit(Activo);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó el Tipo de Activo: " + Activo.Nombre, this.Name);

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

        private void DialogActivo_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
        }

        private void txtCodigo_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.TextEdit)sender, errRequiredField);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
