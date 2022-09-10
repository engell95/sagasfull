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
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogEO : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEstruckOrgani MDI;
        internal Entidad.EstructuraOrganizativa EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int PadreID = 0;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string Descripcion
        {
            get { return mmoDescripcion.Text; }
            set { mmoDescripcion.Text = value; }
        }

        private int IdEmpresa
        {
            get { return Convert.ToInt32(empresaLookUpEdit.EditValue); }
            set { empresaLookUpEdit.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogEO()
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

                var query = from em in db.Empresas
                            select new { em.Nombre, em.ID };

                empresaLookUpEdit.Properties.DataSource = query;
                
                if (query.Count() > 0)
                    IdEmpresa = query.First().ID;

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Descripcion = EntidadAnterior.Descripcion;
                    IdEmpresa = EntidadAnterior.EmpresaID;
                    PadreID = EntidadAnterior.PadreID;

                    if (PadreID <= 0)
                        txtPadre.Text = "<< Ninguna >>";
                    else
                        txtPadre.Text = db.EstructuraOrganizativa.Single(eo => eo.ID.Equals(PadreID)).Nombre;
                }
                else
                {
                    if (PadreID <= 0)
                        txtPadre.Text = "<< Ninguna >>";

                    else
                        txtPadre.Text = db.EstructuraOrganizativa.Single(eo => eo.ID.Equals(PadreID)).Nombre;
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
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || empresaLookUpEdit.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (empresaLookUpEdit.EditValue == null)
            {
                //Classes.Global.DialogMsg("Asignar la estructura organizativa a una empresa", AdmonP.Classes.MsgType.warning);
                //return false;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.EstructuraOrganizativa EO;

                    if (Editable)
                    { EO = db.EstructuraOrganizativa.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        EO = new Entidad.EstructuraOrganizativa();
                        EO.Activo = true;
                    }

                    EO.Nombre = Nombre;
                    EO.Descripcion = Descripcion;
                    EO.EmpresaID = IdEmpresa;
                    EO.PadreID = PadreID;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EO, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó la Estructura Organizativa: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.EstructuraOrganizativa.InsertOnSubmit(EO);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó la Estructura Organizativa: " + EO.Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }



        
    }


}