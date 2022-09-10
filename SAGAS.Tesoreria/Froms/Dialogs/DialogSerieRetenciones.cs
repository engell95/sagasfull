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
    public partial class DialogSerieRetenciones : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormSerieRetenciones MDI;
        internal Entidad.SeriesRetencione EntidadAnterior;
        internal bool Editable = false;
        public int IdPadre = 0;
        private int UsuarioID = 0;
        private string chequeName = "";
        private string ExtensionName = "";
        private bool ShowMsg = false;

        private string Serie
        {
            get { return txtSerie.Text; }
            set { txtSerie.Text = value; }
        }

        private string Separador
        {
            get { return txtSeparador.Text; }
            set { txtSeparador.Text = value; }
        }


        private int Numero
        {
            get { return Convert.ToInt32(this.txtNumero.EditValue); }
            set { this.txtNumero.EditValue = value; }
        }

       
        private int Estacion
        {
            get { return Convert.ToInt32(this.lkeEstacionServicio.EditValue); }
            set { this.lkeEstacionServicio.EditValue = value; }
        }

        private int SubEstacion
        {
            get { return Convert.ToInt32(this.lkeSubEstacion.EditValue); }
            set { this.lkeSubEstacion.EditValue = value; }
        }

        private bool Alcaldia
        {
            get { return chkAlcaldia.Checked; }
            set { this.chkAlcaldia.Checked = value; }
        }

        private bool Automatico
        {
            get { return chkAutomatico.Checked; }
            set { this.chkAutomatico.Checked = value; }
        }
                
        #endregion

        #region *** INICIO ***

        public DialogSerieRetenciones(int UserID)
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
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;
            
                if (Editable)
                {
                    Serie = EntidadAnterior.Prefijo;
                    Separador = EntidadAnterior.Separador;
                    Numero = EntidadAnterior.Numero;
                    Estacion = EntidadAnterior.EstacionServicioID;
                    SubEstacion = EntidadAnterior.SubEstacionServicioID;
                    Alcaldia = EntidadAnterior.EsAlcaldia;
                    Automatico = EntidadAnterior.Automatico;

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtSerie, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtSeparador, errRequiredField);
            //Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txtSerie.Text) || String.IsNullOrEmpty(txtSeparador.Text) || String.IsNullOrEmpty(txtNumero.Text) || lkeEstacionServicio.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarSERIE((EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("La Estación de Servicio " + lkeEstacionServicio.Text + (lkeSubEstacion.EditValue == null ? "" : "Sub E/S " + lkeSubEstacion.Text) + ", ya tiene asignada una serie de retenciones." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// VALIDAR SERIE REPETIDA
        /// </summary>
        /// <param name="ID">ID de la Entidad Anterior</param>
        /// <returns></returns>
        private bool ValidarSERIE(int? ID)
        {
            var result = (from s in db.SeriesRetenciones
                          where (ID.HasValue ? s.EstacionServicioID.Equals(Estacion) && s.SubEstacionServicioID.Equals(SubEstacion) && s.EsAlcaldia.Equals(Alcaldia) && s.ID != Convert.ToInt32(ID) : s.EstacionServicioID.Equals(Estacion) && s.SubEstacionServicioID.Equals(SubEstacion) && s.EsAlcaldia.Equals(Alcaldia))
                          select s);

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
                    Entidad.SeriesRetencione S;

                    if (Editable)
                    {
                        S = db.SeriesRetenciones.Single(e => e.ID.Equals(EntidadAnterior.ID));
                        
                    }
                    else 
                    {
                        S = new Entidad.SeriesRetencione();
                    }

                    S.Prefijo = Serie;
                    S.Separador = Separador;
                    S.Numero = Numero;
                    S.EstacionServicioID = Estacion;
                    S.SubEstacionServicioID = SubEstacion;
                    S.EsAlcaldia = Alcaldia;
                    S.Automatico = Automatico;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(S, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó la Serie : " + lkeSubEstacion.Text, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.SeriesRetenciones.InsertOnSubmit(S);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó la Cuenta Bancaria: " + lkeSubEstacion.Text, this.Name);
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
            MDI.CleanDialog(ShowMsg, false);
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
                if (lkeEstacionServicio.EditValue != null)
                {
                    if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))) > 0)
                    {
                        //**SubEstación
                        lkeSubEstacion.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))).ToList();
                        lkeSubEstacion.Properties.DisplayMember = "Nombre";
                        lkeSubEstacion.Properties.ValueMember = "ID";
                    }
                    else
                    {
                        lkeSubEstacion.Properties.DataSource = null;
                        lkeSubEstacion.EditValue = null;
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        #endregion

    }
    
}