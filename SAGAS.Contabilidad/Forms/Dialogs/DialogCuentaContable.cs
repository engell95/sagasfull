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
    public partial class DialogCuentaContable : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCuentaContable MDI;
        internal Entidad.CuentaContable EntidadAnterior;
        private DataTable dtEstacionesServicios;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;

        private string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

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

        private string AgrupacionLibros {
            get { return textAgrupacion.Text; }
            set { textAgrupacion.Text = value; }
                      
                }

        private string Tipo
        {
            get { return txtTipo1.Text; }
            set { txtTipo1.Text = value; }

        }

        private string Tipo2
        {
            get { return txtTipo2.Text; }
            set { txtTipo2.Text = value; }

        }

        private string Tipo3
        {
            get { return txtTipo3.Text; }
            set { txtTipo3.Text = value; }

        }

        private bool EsDetalle
        {
            get { return Convert.ToBoolean(chkDetalle.Checked); }
            set { chkDetalle.Checked = value; }
        }

        private bool EsDolar
        {
            get { return Convert.ToBoolean(chkEsDolar.Checked); }
            set { chkEsDolar.Checked = value; }
        }

        private bool EsModular
        {
            get { return Convert.ToBoolean(chkModular.Checked); }
            set { chkModular.Checked = value; }
        }

        public int IDTipoCuenta
        {
            get { return Convert.ToInt32(lkTipoCuenta.EditValue); }
            set { lkTipoCuenta.EditValue = value; }
        }

        public int IDCeco
        {
            get { return Convert.ToInt32(lkCeco.EditValue); }
            set { lkCeco.EditValue = value; }
        }

        public int Nivel
        {
            get { return Convert.ToInt32(spNivel.Value); }
            set { spNivel.Value = value; }
        }

        private bool EsImpuesto
        {
            get { return Convert.ToBoolean(chkEsImpuesto.Checked); }
            set { chkEsImpuesto.Checked = value; }
        }

        private bool EsBanco
        {
            get { return Convert.ToBoolean(chkBanco.Checked); }
            set { chkBanco.Checked = value; }
        }

        private decimal Porcentaje
        {
            get { return Convert.ToDecimal(spPorcentaje.Value); }
            set { spPorcentaje.Value = value; }
        }

        private decimal Limite
        {
            get { return Convert.ToDecimal(spLimite.Value); }
            set { spLimite.Value = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogCuentaContable()
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

                //Tipo de Cuenta
                lkTipoCuenta.Properties.DataSource = from z in db.TipoCuentas where z.Activo select new { z.ID, z.Nombre };
                lkTipoCuenta.Properties.DisplayMember = "Nombre";
                lkTipoCuenta.Properties.ValueMember = "ID";

                //Centro de Costo
                lkCeco.Properties.DataSource = from ce in db.CentroCostos where ce.Activo select new { ce.ID, Display = ce.Codigo + " | " + ce.Nombre };
                
                var listSucursales = (from ES in db.EstacionServicios
                                      where ES.Activo
                                      select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

                this.dtEstacionesServicios = ToDataTable(listSucursales);

                this.grid.DataSource = dtEstacionesServicios;

                if (Editable)
                {
                    Codigo = EntidadAnterior.Codigo;
                    Nombre = EntidadAnterior.Nombre;
                    AgrupacionLibros = EntidadAnterior.AgrupacionLibros;            
                    Comentario = EntidadAnterior.Comentario;
                    EsDetalle = EntidadAnterior.Detalle;
                    EsDolar = EntidadAnterior.EsDolar;
                    EsModular = EntidadAnterior.Modular;
                    IdPadre = EntidadAnterior.IDPadre;
                    IDTipoCuenta = EntidadAnterior.IDTipoCuenta;
                    Nivel = EntidadAnterior.Nivel;
                    EsImpuesto = EntidadAnterior.EsImpuesto;
                    Porcentaje = EntidadAnterior.Porcentaje;
                    Limite = EntidadAnterior.Limite;
                    EsBanco = EntidadAnterior.EsBanco;
                    IDCeco = EntidadAnterior.CecoID;
                    Tipo = EntidadAnterior.Tipo;
                    Tipo2 = EntidadAnterior.Tipo2;
                    Tipo3 = EntidadAnterior.Tipo3;

                    if (IdPadre <= 0)
                        txtPadre.Text = "<< Ninguna >>";
                    else
                        txtPadre.Text = db.CuentaContables.Single(tc => tc.ID.Equals(IdPadre)).Nombre;

                    var query = db.CuentaContableEstacions.Where(cc => cc.CuentaContable.Equals(EntidadAnterior)).ToList();

                        db.EstacionServicios.Where(es => es.Activo).ToList().ForEach(obj =>
                        {
                            if (!query.Exists(q => q.EstacionID.Equals(obj.ID)))
                            {
                                DataRow[] ESRow = dtEstacionesServicios.Select("ID = " + obj.ID);
                                DataRow row = ESRow.First();
                                row["SelectedES"] = false;
                            }
                        });
                        gvData.RefreshData();

                }
                else
                {
                    if (IdPadre <= 0)
                    {
                        txtCodigo.Text = "0000000000";
                        txtPadre.Text = "<< Ninguna >>";
                        spNivel.Value = 1;
                    }
                    else
                    {
                        var CC = db.CuentaContables.Single(tc => tc.ID.Equals(IdPadre));
                        txtPadre.Text = CC.Nombre;
                        spNivel.Value = CC.Nivel + 1;
                        txtCodigo.Text = CC.Codigo;
                    }
                                        
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkTipoCuenta, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || String.IsNullOrEmpty(txtCodigo.Text) || lkTipoCuenta.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), (EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("El Código de la Cuenta Contablea '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            //if (!ValidarNombre(Convert.ToString(txtNombre.Text), (EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            //{
            //    Parametros.General.DialogMsg("El Nombre de la Cuenta Contable '" + Convert.ToString(txtNombre.Text) + "' ya esta registrado en el sistema, por favor seleccione otro nombre.", Parametros.MsgType.warning);
            //    return false;
            //}

            if (dtEstacionesServicios.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.CuentaContables
                          where (ID.HasValue ? i.Codigo.Equals(code) && !i.ID.Equals(Convert.ToInt32(ID)) : i.Codigo.Equals(code))
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        private bool ValidarNombre(string code, int? ID)
        {
            var result = (from i in db.CuentaContables
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
                    Entidad.CuentaContable CC;

                    if (Editable)
                    { CC = db.CuentaContables.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        CC = new Entidad.CuentaContable();
                        CC.Activo = true;
                    }

                    CC.Codigo = Codigo;
                    CC.Nombre = Nombre;
                    CC.AgrupacionLibros = AgrupacionLibros;
                    CC.Comentario = Comentario;
                    CC.Detalle = EsDetalle;
                    CC.EsDolar = EsDolar;
                    CC.Modular = EsModular;
                    CC.IDEmpresa = 1;
                    CC.IDPadre = IdPadre;
                    CC.IDTipoCuenta = IDTipoCuenta;
                    CC.Nivel = Nivel;
                    CC.EsImpuesto = EsImpuesto;
                    CC.Porcentaje = Porcentaje;
                    CC.Limite = Limite;
                    CC.EsBanco = EsBanco;
                    CC.CecoID = IDCeco;
                    CC.Tipo = Tipo;
                    CC.Tipo2 = Tipo2;
                    CC.Tipo3 = Tipo3;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(CC, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó la Cuenta Contable: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.CuentaContables.InsertOnSubmit(CC);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó la Cuenta Contable: " + CC.Nombre, this.Name);

                    }

                    db.SubmitChanges();


                    foreach (DataRow rb in dtEstacionesServicios.DefaultView.Table.Rows)
                    {
                        if (Convert.ToBoolean(rb["SelectedES"]))
                            {
                                var CCESAnterior = db.CuentaContableEstacions.Where(x => x.CuentaContable.Equals(CC) && x.EstacionID.Equals(Convert.ToInt32(rb["ID"])));

                                if (CCESAnterior.Count() > 0)
                                {
                                    Entidad.CuentaContableEstacion CCES = CCESAnterior.First();

                                    CCES.CuentaContable = CC;
                                    CCES.EstacionID = Convert.ToInt32(rb["ID"]);
                                    db.SubmitChanges();
                                }
                                else if (CCESAnterior.Count() <= 0)
                                {
                                    Entidad.CuentaContableEstacion CCES = new Entidad.CuentaContableEstacion();
                                    CCES.CuentaContable = CC;
                                    CCES.EstacionID = Convert.ToInt32(rb["ID"]);
                                    db.CuentaContableEstacions.InsertOnSubmit(CCES);
                                    db.SubmitChanges();
                                }
                            }
                        else if (!Convert.ToBoolean(rb["SelectedES"]))
                            {
                                var CCESAnterior = db.CuentaContableEstacions.Where(x => x.CuentaContable.Equals(CC) && x.EstacionID.Equals(Convert.ToInt32(rb["ID"])));

                                if (CCESAnterior.Count() > 0)
                                {
                                    Entidad.CuentaContableEstacion CCES = CCESAnterior.First();
                                    db.CuentaContableEstacions.DeleteOnSubmit(CCES);
                                    db.SubmitChanges();
                                }

                            }
                        
                    }



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

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void chkDetalle_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDetalle.Checked)
                layoutControlGroupModular.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
            {
                layoutControlGroupModular.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                chkModular.Checked = false;
            }
        }
        
        private void btnSelectAllES_Click(object sender, EventArgs e)
        {
            gvData.ActiveFilter.Clear();
            for (int i = 0; i < gvData.RowCount; i++)
            {
                gvData.SetRowCellValue(i, "SelectedES", true);
            }
        }

        private void btnUnselectAllES_Click(object sender, EventArgs e)
        {
            gvData.ActiveFilter.Clear();
            for (int i = 0; i < gvData.RowCount; i++)
            {
                gvData.SetRowCellValue(i, "SelectedES", false);
            }
        }





        #endregion

        private void chkEsImpuesto_CheckedChanged(object sender, EventArgs e)
        {

        }

    }


}