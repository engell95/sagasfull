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
    public partial class DialogManguera : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormManguera MDI;
        internal Entidad.Manguera EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;
        private int CombustibleID;
      
        public string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        public string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        public string Serie
        {
            get { return txtSerie.Text; }
            set { txtSerie.Text = value; }
        }

        public int IDTanque
        {
            get { return Convert.ToInt32(glkTanque.EditValue); }
            set { glkTanque.EditValue = value; }
        }

        public int IDDispensador
        {
            get { return Convert.ToInt32(glkDispensador.EditValue); }
            set { glkDispensador.EditValue = value; }
        }

        public string Lado
        {
            get { return Convert.ToString(cboLado.SelectedItem); }
            set { cboLado.SelectedItem = value; }
        }
      
        public bool FlujoRapido
        {
            get { return Convert.ToBoolean(chkFlujoRapido.Checked); }
            set { chkFlujoRapido.Checked = value; }
        }

        public bool PrecioDiferenciado
        {
            get { return Convert.ToBoolean(chkPrecioDiferenciado.Checked); }
            set { chkPrecioDiferenciado.Checked = value; }
        }

        public bool EsLecturaGalones
        {
            get { return Convert.ToBoolean(chkEsLecturaGalones.Checked); }
            set { chkEsLecturaGalones.Checked = value; }
        }

        public decimal Descuento
        {
            get { return Convert.ToDecimal(speDescuento.EditValue); }
            set { speDescuento.EditValue = value; }
        }

        public decimal Mecanica
        {
            get { return Convert.ToDecimal(spMecanica.Value); }
            set { speDescuento.Value = value; }
        }

        public decimal Electronica
        {
            get { return Convert.ToDecimal(spElectronica.Value); }
            set { spElectronica.Value = value; }
        }

        public decimal Efectivo
        {
            get { return Convert.ToDecimal(spEfectivo.Value); }
            set { spEfectivo.Value = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogManguera(int UserID, int ComID)
        {
            InitializeComponent();
            UsuarioID = UserID;
            CombustibleID = ComID;
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

                //**EstacionServicio
                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkeEstacionServicio.Properties.DisplayMember = "Nombre";
                lkeEstacionServicio.Properties.ValueMember = "ID";
                lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                var datos = from M in db.Mangueras
                            join D in db.Dispensadors on M.DispensadorID equals D.ID
                            join I in db.Islas on D.IslaID equals I.ID
                            where D.Activo && I.Activo && M.Activo && I.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)
                            select M;

                if (datos.ToList().Count > 0)
                {
                    int Number = Parametros.General.ValorConsecutivo(datos.OrderByDescending(o => o.Numero).First().Numero);
                    Numero = "M " + Number.ToString("00");
                }

                if (Editable)
                {
                    Numero = EntidadAnterior.Numero;
                    Comentario = EntidadAnterior.Comentario;
                    Descuento = EntidadAnterior.Descuento;
                    IDDispensador = EntidadAnterior.DispensadorID;
                    FlujoRapido = EntidadAnterior.FlujoRapido;
                    Lado = EntidadAnterior.Lado;
                    PrecioDiferenciado = EntidadAnterior.PrecioDiferenciado;
                    EsLecturaGalones = EntidadAnterior.EsLecturaGalones;
                    Serie = EntidadAnterior.Serie;
                    IDTanque = EntidadAnterior.TanqueID;
                    lkeEstacionServicio.EditValue = db.Tanques.Single(t => t.ID == IDTanque).EstacionServicioID;
                    lkSES.EditValue = db.Tanques.Single(t => t.ID.Equals(IDTanque)).SubEstacionID;
                    spMecanica.Value = EntidadAnterior.LecturaMecanica;
                    spElectronica.Value = EntidadAnterior.LecturaElectronica;
                    spEfectivo.Value = EntidadAnterior.LecturaEfectivo;
                    IDTanque = EntidadAnterior.TanqueID;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }


        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkTanque, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkDispensador, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNumero.Text == "" || lkeEstacionServicio.EditValue == null || glkTanque.EditValue == null || glkDispensador.EditValue == null || cboLado.SelectedIndex < 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarES())
            {
                Parametros.General.DialogMsg("la Isla seleccionada no esta registrada en la misma Estacion de Servicio del Tanque seleccionado." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarES()
        {
            try
            {
                var obj = from es in db.EstacionServicios
                          join t in db.Tanques on es.ID equals t.EstacionServicioID
                          join i in db.Islas on es.ID equals i.EstacionServicioID
                          join d in db.Dispensadors on i.ID equals d.IslaID
                          where t.ID == Convert.ToInt32(IDTanque) && d.ID == Convert.ToInt32(IDDispensador)
                          select new { es.ID };

                if (obj.Count() > 0)
                    return true;
                else
                    return false;
            }
            catch { return false; }

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
                    Entidad.Manguera M;

                    if (Editable)
                    { M = db.Mangueras.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        M = new Entidad.Manguera();
                        M.Activo = true;
                    }

                    M.Numero = Numero;
                    M.Comentario = Comentario;
                    M.Descuento = Descuento;
                    M.DispensadorID = IDDispensador;
                    M.FlujoRapido = FlujoRapido;
                    M.Lado = Lado;
                    M.PrecioDiferenciado = PrecioDiferenciado;
                    M.EsLecturaGalones = EsLecturaGalones;
                    M.Serie = Serie;
                    M.TanqueID = IDTanque;
                    M.LecturaMecanica = Mecanica;
                    M.LecturaEfectivo = Efectivo;
                    M.LecturaElectronica = Electronica;
                                  
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                         "Se modificó la Manguera: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Mangueras.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó la Manguera: " + M.Numero, this.Name);

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

        private void txtNumero_Validated(object sender, EventArgs e)
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
                    glkTanque.Properties.DataSource = null;
                    glkDispensador.Properties.DataSource = null;
                }
                else
                {
                    lkSES.Properties.DataSource = null;
                    //**Tanques
                    glkTanque.Properties.DataSource = from T in db.Tanques
                                                      join P in db.Productos on T.ProductoID equals P.ID
                                                      where T.Activo && P.Activo && P.ProductoClaseID == CombustibleID && T.EstacionServicioID == Convert.ToInt32(lkeEstacionServicio.EditValue)
                                                           && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == T.EstacionServicioID))
                                                      select new
                                                      {
                                                          IDTanque = T.ID,
                                                          TanqueNombre = T.Nombre,
                                                          ProductoNombre = P.Nombre,
                                                          Display = T.Nombre + " => " + P.Nombre
                                                      };
                    glkTanque.Properties.DisplayMember = "Display";
                    glkTanque.Properties.ValueMember = "IDTanque";

                    //**Dispensadores
                    glkDispensador.Properties.DataSource = from D in db.Dispensadors
                                                           join I in db.Islas on D.IslaID equals I.ID
                                                           where D.Activo && I.Activo && I.EstacionServicioID == Convert.ToInt32(lkeEstacionServicio.EditValue)
                                                           && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == I.EstacionServicioID))
                                                           select new
                                                           {
                                                               IDDispensador = D.ID,
                                                               IslaNombre = I.Nombre,
                                                               DispensadorNombre = D.Nombre,
                                                               Display = I.Nombre + " => " + D.Nombre
                                                           };
                    glkDispensador.Properties.DisplayMember = "Display";
                    glkDispensador.Properties.ValueMember = "IDDispensador";
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
                    //**Tanques
                    glkTanque.Properties.DataSource = from T in db.Tanques
                                                      join P in db.Productos on T.ProductoID equals P.ID
                                                      where T.Activo && P.Activo && P.ProductoClaseID.Equals(CombustibleID) && T.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue)) && T.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue))
                                                           && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == T.EstacionServicioID))
                                                      select new
                                                      {
                                                          IDTanque = T.ID,
                                                          TanqueNombre = T.Nombre,
                                                          ProductoNombre = P.Nombre,
                                                          Display = T.Nombre + " => " + P.Nombre
                                                      };
                    glkTanque.Properties.DisplayMember = "Display";
                    glkTanque.Properties.ValueMember = "IDTanque";

                    //**Dispensadores
                    glkDispensador.Properties.DataSource = from D in db.Dispensadors
                                                           join I in db.Islas on D.IslaID equals I.ID
                                                           where D.Activo && I.Activo && I.EstacionServicioID.Equals(Convert.ToInt32(lkeEstacionServicio.EditValue)) && I.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue))
                                                           && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == I.EstacionServicioID))
                                                           select new
                                                           {
                                                               IDDispensador = D.ID,
                                                               IslaNombre = I.Nombre,
                                                               DispensadorNombre = D.Nombre,
                                                               Display = I.Nombre + " => " + D.Nombre
                                                           };
                    glkDispensador.Properties.DisplayMember = "Display";
                    glkDispensador.Properties.ValueMember = "IDDispensador";
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

    }
}