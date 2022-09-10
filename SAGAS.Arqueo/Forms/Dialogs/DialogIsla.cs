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
using SAGAS.Arqueo.Forms;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogIsla : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormIsla MDI;
        internal Entidad.Isla EntidadAnterior;
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
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogIsla(int UserID)
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

                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.Properties.DisplayMember = "Nombre";
                lkeEstacionServicio.Properties.ValueMember = "ID";  
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                //**SubEstación
                lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                lkSES.Properties.DisplayMember = "Nombre";
                lkSES.Properties.ValueMember = "ID";

                if (db.Islas.Count(i => i.Activo && i.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)) > 0)
                {
                    int numero = Parametros.General.ValorConsecutivo(db.Islas.Where(i => i.Activo && i.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending(o => o.Nombre).First().Nombre);
                    Nombre = "I " + numero.ToString();
                }

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Comentario = EntidadAnterior.Comentario;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;
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
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeEstacionServicio.EditValue == null)
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
                    Entidad.Isla I;

                    if (Editable)
                    { I = db.Islas.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        I = new Entidad.Isla();
                        I.Activo = true;
                    }

                    I.Nombre = Nombre;
                    I.Comentario = Comentario;
                    I.EstacionServicioID = IDEstacionServicio;
                    I.SubEstacionID = IDSubEstacion;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(I, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                         "Se modificó la Isla: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Islas.InsertOnSubmit(I);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó la Isla: " + I.Nombre, this.Name);

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

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkeEstacionServicio_EditValueChanged(object sender, EventArgs e)
        {
           
            Parametros.General.ValidateDifES((BaseEdit)sender, infDifES);
        }

        #endregion
    }
}