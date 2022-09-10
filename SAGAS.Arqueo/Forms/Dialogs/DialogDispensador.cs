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
    public partial class DialogDispensador : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormDispensador MDI;
        internal Entidad.Dispensador EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;

        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Modelo
        {
            get { return txtModelo.Text; }
            set { txtModelo.Text = value; }
        } 

        public string Serie
        {
            get { return txtSerie.Text; }
            set { txtSerie.Text = value; }
        }

        public string Marca
        {
            get { return txtMarca.Text; }
            set { txtMarca.Text = value; }
        }

        public string CodigoDNP
        {
            get { return txtCodigoDNP.Text; }
            set { txtCodigoDNP.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public int IDIsla
        {
            get { return Convert.ToInt32(lkeIsla.EditValue); }
            set { lkeIsla.EditValue = value; }
        }

        public bool Mecanica
        {
            get { return Convert.ToBoolean(chkMecanica.Checked); }
            set { chkMecanica.Checked = value; }
        }

        public bool Efectivo
        {
            get { return Convert.ToBoolean(chkEfectivo.Checked); }
            set { chkEfectivo.Checked = value; }
        }

        public bool Electronica
        {
            get { return Convert.ToBoolean(chkElectronica.Checked); }
            set { chkElectronica.Checked = value; }
        }

        public bool MecanicaAmbasCara
        {
            get { return Convert.ToBoolean(chkMecanicaAmbasCara.Checked); }
            set { chkMecanicaAmbasCara.Checked = value; }
        }

        public bool EsDispensadorProblemaLecturas
        {
            get { return Convert.ToBoolean(chkEsDispensadorProblemaLecturas.Checked); }
            set { chkEsDispensadorProblemaLecturas.Checked = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogDispensador(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
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

                //**EstacionServicio
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.Properties.DisplayMember = "Nombre";
                lkeEstacionServicio.Properties.ValueMember = "ID";
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                ////**SubEstación
                //lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();

                //lkSES.Properties.DisplayMember = "Nombre";
                //lkSES.Properties.ValueMember = "ID";

                var datos = from D in db.Dispensadors
                            join I in db.Islas on D.IslaID equals I.ID
                            where D.Activo && I.Activo && I.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)
                            select D;

                if (datos.ToList().Count > 0)
                {
                    int numero = Parametros.General.ValorConsecutivo(datos.OrderByDescending(o => o.Nombre).First().Nombre);
                    Nombre = "D " + numero.ToString();
                }
                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Modelo = EntidadAnterior.Modelo;
                    Serie = EntidadAnterior.Serie;
                    Marca = EntidadAnterior.Marca;
                    CodigoDNP = EntidadAnterior.CodigoDNP;
                    Comentario = EntidadAnterior.Comentario;
                    IDIsla = EntidadAnterior.IslaID;
                    lkeEstacionServicio.EditValue = db.Islas.Single(t => t.ID == IDIsla).EstacionServicioID;
                    lkSES.EditValue = db.Islas.Single(t => t.ID.Equals(IDIsla)).SubEstacionID;
                    Efectivo = EntidadAnterior.LecturaEfectivo;
                    Electronica = EntidadAnterior.LecturaElectronica;
                    Mecanica = EntidadAnterior.LecturaMecanica;
                    MecanicaAmbasCara = EntidadAnterior.MecanicaAmbasCara;
                    EsDispensadorProblemaLecturas = EntidadAnterior.EsDispensadorProblemaLecturas;
                    IDIsla = EntidadAnterior.IslaID;
                    
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
            Parametros.General.ValidateEmptyStringRule(lkeIsla, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNombre.Text == "" || lkeIsla.EditValue == null || lkeEstacionServicio.EditValue == null)
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
                    Entidad.Dispensador D;

                    if (Editable)
                    { D = db.Dispensadors.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        D = new Entidad.Dispensador();
                        D.Activo = true;
                    }

                    D.Nombre = Nombre;

                    D.Modelo = Modelo;
                    D.Serie = Serie;
                    D.Marca = Marca;
                    D.CodigoDNP = CodigoDNP;
                    D.Comentario = Comentario;
                    D.IslaID = IDIsla;
                    D.LecturaEfectivo = Efectivo;
                    D.LecturaElectronica = Electronica;
                    D.LecturaMecanica = Mecanica;
                    D.MecanicaAmbasCara = MecanicaAmbasCara;
                    D.EsDispensadorProblemaLecturas = EsDispensadorProblemaLecturas;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(D, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                         "Se modificó el Dispensador: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Dispensadors.InsertOnSubmit(D);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó el Dispensador: " + D.Nombre, this.Name);

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
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void lkeEstacionServicio_EditValueChanged(object sender, EventArgs e)
        {
            Parametros.General.ValidateDifES((BaseEdit)sender, infDifES);

            if (lkeEstacionServicio.EditValue != null)
            {
                if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))) > 0)
                {
                    //**SubEstación
                    lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue))).ToList();
                    lkSES.Properties.DisplayMember = "Nombre";
                    lkSES.Properties.ValueMember = "ID";
                    lkeIsla.Properties.DataSource = null;

                }
                else
                {
                    //Islas
                    lkSES.Properties.DataSource = null;
                    lkeIsla.Properties.DataSource = from I in db.Islas
                                                    where I.Activo && I.EstacionServicioID == Convert.ToInt32(lkeEstacionServicio.EditValue)
                                                    select new { I.ID, I.Nombre };
                    lkeIsla.Properties.DisplayMember = "Nombre";
                    lkeIsla.Properties.ValueMember = "ID";
                }
            }
        }
    
        #endregion

        private void lkSES_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkSES.EditValue != null)
                {
                    //Islas

                    lkeIsla.Properties.DataSource = from I in db.Islas
                                                    where I.Activo && I.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue)) && I.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue))
                                                    select new { I.ID, I.Nombre };
                    lkeIsla.Properties.DisplayMember = "Nombre";
                    lkeIsla.Properties.ValueMember = "ID";
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

    }
}