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
using System.Windows.Input;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogTipoCuenta : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTipoCuenta MDI;
        internal Entidad.TipoCuenta EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        private bool EsNaturalezaDeudora
        {
            get { return Convert.ToBoolean(chkNaturalezaDeudora.Checked); }
            set { chkNaturalezaDeudora.Checked = value; }
        }

        private bool EsEfectoCero
        {
            get { return Convert.ToBoolean(chkEfectoCero.Checked); }
            set { chkEfectoCero.Checked = value; }
        }

        private bool UsaCentroCosto
        {
            get { return Convert.ToBoolean(chkUsaCentroCosto.Checked); }
            set { chkUsaCentroCosto.Checked = value; }
        }

        private bool Balance
        {
            get { return Convert.ToBoolean(chkCuentaBalance.Checked); }
            set { chkCuentaBalance.Checked = value; }
        }

        private bool Resultado
        {
            get { return Convert.ToBoolean(chkCuentaResultado.Checked); }
            set { chkCuentaResultado.Checked = value; }
        }

        private int Orden
        {
            get { return Convert.ToInt32(spOrden.Value); }
            set { spOrden.Value = value; }
        }

        private int Grupo
        {
            get { return Convert.ToInt32(spGrupo.Value); }
            set { spGrupo.Value = value; }
        }
        #endregion

        #region *** INICIO ***

        public DialogTipoCuenta()
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

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Comentario = EntidadAnterior.Comentario;
                    EsNaturalezaDeudora = EntidadAnterior.NaturalezaDeudora;
                    EsEfectoCero = EntidadAnterior.EfectoCero;
                    UsaCentroCosto = EntidadAnterior.UsaCentroCosto;
                    IdPadre = EntidadAnterior.IdPadre;
                    Balance = EntidadAnterior.EsCuentaBalanza;
                    Resultado = EntidadAnterior.EsCuenteResultado;
                    Orden = EntidadAnterior.Orden;
                    Grupo = EntidadAnterior.GrupoID;

                    if (IdPadre <= 0)
                        txtPadre.Text = "<< Ninguna >>";
                    else
                        txtPadre.Text = db.TipoCuentas.Single(tc => tc.ID.Equals(IdPadre)).Nombre;
                }
                else
                {
                    if (IdPadre <= 0)
                        txtPadre.Text = "<< Ninguna >>";

                    else
                        txtPadre.Text = db.TipoCuentas.Single(tc => tc.ID.Equals(IdPadre)).Nombre;
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
            if (txtNombre.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarNombre(Convert.ToString(txtNombre.Text), (EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("El Nombre del Tipo de Cuenta '" + Convert.ToString(txtNombre.Text) + "' ya esta registrado en el sistema, por favor seleccione otro nombre.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarNombre(string code, int? ID)
        {
            var result = (from i in db.TipoCuentas
                          where (ID.HasValue ? i.Nombre.Equals(code) && !i.ID.Equals(Convert.ToInt32(ID)) : i.Nombre.Equals(code))
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
                    Entidad.TipoCuenta TC;

                    if (Editable)
                    { TC = db.TipoCuentas.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        TC = new Entidad.TipoCuenta();
                        TC.Activo = true;
                    }

                    TC.Nombre = Nombre;
                    TC.Comentario = Comentario;
                    TC.NaturalezaDeudora = EsNaturalezaDeudora;
                    TC.IdEmpresa = 1;
                    TC.IdPadre = IdPadre;
                    TC.EfectoCero = EsEfectoCero;
                    TC.UsaCentroCosto = UsaCentroCosto;
                    TC.EsCuentaBalanza = Balance;
                    TC.EsCuenteResultado = Resultado;
                    TC.Orden = Orden;
                    TC.GrupoID = Grupo;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(TC, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó el Tipo de Cuenta: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.TipoCuentas.InsertOnSubmit(TC);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó Tipo el de Cuenta: " + TC.Nombre, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
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