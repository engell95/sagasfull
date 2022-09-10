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
    public partial class DialogProductoClase : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormProductoClase MDI;
        internal Entidad.ProductoClase EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int IDArea
        {
            get { return Convert.ToInt32(lkeArea.EditValue); }
            set { lkeArea.EditValue = value; }
        }

        public string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogProductoClase()
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

                //AREAS
                lkeArea.Properties.DataSource = from a in db.Areas where a.Activo select new { a.ID, a.Nombre };
                lkeArea.Properties.DisplayMember = "Nombre";
                lkeArea.Properties.ValueMember = "ID";

                if (Editable)
                {
                    Codigo = EntidadAnterior.Codigo;
                    Nombre = EntidadAnterior.Nombre;
                    Comentario = EntidadAnterior.Comentario;
                    IDArea = EntidadAnterior.AreaID;
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
            Parametros.General.ValidateEmptyStringRule(lkeArea, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeArea.EditValue == null || txtCodigo.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarLogin(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código de la clase '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarLogin(string code, int? ID)
        {
            var result = (from i in db.ProductoClases
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
                    Entidad.ProductoClase P;

                    if (Editable)
                    { P = db.ProductoClases.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        P = new Entidad.ProductoClase();
                        P.Activo = true;
                    }

                    P.Codigo = Codigo;
                    P.Nombre = Nombre;
                    P.Comentario = Comentario;
                    P.AreaID = IDArea;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(P, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                         "Se modificó la Clase de Productos: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.ProductoClases.InsertOnSubmit(P);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                        "Se creó la Clase de Productos: " + P.Nombre, this.Name);

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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((TextEdit)sender, errRequiredField);
        }
        
    }
}