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
using SAGAS.Nomina.Forms;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogPlanilla : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormPlanilla MDI;
        internal Entidad.Planilla EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;

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

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkeEstacionServicio.EditValue); }
            set { lkeEstacionServicio.EditValue = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSubEstacion.EditValue); }
            set { lkSubEstacion.EditValue = value; }
        }

        public int NumeroPlanilla
        {
            get { return Convert.ToInt32(speNumeroPlanilla.EditValue); }
            set { speNumeroPlanilla.EditValue = value; }
        }

        private int EO
        {

            get { return Convert.ToInt32(trEO.EditValue); }
            set { trEO.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogPlanilla(int UserID)
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
                
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios where es.Activo select new {es.ID, es.Nombre };
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                trEO.Properties.DataSource = db.EstructuraOrganizativa.Where(o => o.Activo);

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;
                    NumeroPlanilla = EntidadAnterior.NumeroPlanilla;
                    Comentario = EntidadAnterior.Comentario;
                    EO = EntidadAnterior.EstructuraOrganizativaID;
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
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeEstacionServicio.EditValue == null || trEO.EditValue == null)
            {
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
                    Entidad.Planilla P;

                    if (Editable)
                    { P = db.Planillas.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        P = new Entidad.Planilla();
                        P.Activo = true;
                        P.TipoPlanilla = 1;
                    }

                    P.Nombre = Nombre;
                    P.Comentario = Comentario;
                    P.EstacionServicioID = IDEstacionServicio;
                    P.SubEstacionID = IDSubEstacion;
                    P.NumeroPlanilla = NumeroPlanilla;
                    P.EstructuraOrganizativaID = EO;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(P, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó la Planilla: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Planillas.InsertOnSubmit(P);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó la Planilla: " + P.Nombre, this.Name);

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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkeEstacionServicio_EditValueChanged(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
            Parametros.General.ValidateDifES((BaseEdit)sender, infDifES);

            if (lkeEstacionServicio.EditValue != null)
            {
                var sus = db.SubEstacions.Where(s => s.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue)));

                if (sus.Count() > 0)
                {
                    layoutControlItemSUS.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lkSubEstacion.EditValue = null;
                    lkSubEstacion.Properties.DataSource = sus.Select(s => new {s.ID, s.Nombre});
                    lkSubEstacion.Properties.DisplayMember = "Nombre";
                    lkSubEstacion.Properties.ValueMember = "ID";
                }
                else
                {
                    lkSubEstacion.EditValue = null;
                    layoutControlItemSUS.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lkSubEstacion.Properties.DataSource = null;
                }
            }
        }

        #endregion

    }
}