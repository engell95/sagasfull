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
    public partial class DialogCentroCosto : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCentroCosto MDI;
        internal Entidad.CentroCosto EntidadAnterior;
        private DataTable dtEstacionesServicios;
        internal bool Editable = false;
        private bool ShowMsg = false;
        public int IdPadre = 0;

        private string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        private string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        private bool Deducible
        {
            get { return chkDeducible.Checked; }
            set { chkDeducible.Checked = value; }
        }
                
        #endregion

        #region *** INICIO ***

        public DialogCentroCosto()
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

                var listSucursales = (from ES in db.EstacionServicios
                                      where ES.Activo
                                      select new { ES.ID, ES.Nombre, SelectedES = ES.Activo }).OrderBy(o => o.Nombre);

                this.dtEstacionesServicios = Parametros.General.LINQToDataTable(listSucursales);

                this.grid.DataSource = dtEstacionesServicios;

                if (Editable)
                {
                    Nombre = EntidadAnterior.Nombre;
                    Codigo = EntidadAnterior.Codigo;
                    Deducible = EntidadAnterior.NoDeducible;
                    //var query = db.CuentaContableEstacions.Where(cc => cc.CuentaContable.Equals(EntidadAnterior)).ToList();

                    db.EstacionServicios.Where(es => es.Activo).ToList().ForEach(obj =>
                    {
                        if (!EntidadAnterior.CentroCostoPorEstacions.ToList().Exists(q => q.EstacionID.Equals(obj.ID)))
                        {
                            DataRow[] ESRow = dtEstacionesServicios.Select("ID = " + obj.ID);
                            DataRow row = ESRow.First();
                            row["SelectedES"] = false;
                        }
                    });
                    gvData.RefreshData();
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
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txtNombre.Text) && String.IsNullOrEmpty(txtNombre.Text))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarCodigo(Convert.ToString(txtNombre.Text), (EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("El Código del Centro de Costo '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro nombre.", Parametros.MsgType.warning);
                return false;
            }

            if (dtEstacionesServicios.AsEnumerable().Count(c => ((bool)c["SelectedES"]).Equals(true)) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos una Estación de Servicio", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.CentroCostos
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
                    Entidad.CentroCosto CTO;

                    if (Editable)
                    { CTO = db.CentroCostos.Single(e => e.ID.Equals(EntidadAnterior.ID)); }
                    else 
                    {
                        CTO = new Entidad.CentroCosto();
                        CTO.Activo = true;
                    }

                    CTO.Nombre = Nombre;
                    CTO.Codigo = Codigo;
                    CTO.NoDeducible = Deducible;
                                                      
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(CTO, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó el Centro de Costo: " + EntidadAnterior.Nombre, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.CentroCostos.InsertOnSubmit(CTO);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó el Centro de Costo: " + CTO.Nombre, this.Name);

                    }

                    db.SubmitChanges();

                    foreach (DataRow rb in dtEstacionesServicios.DefaultView.Table.Rows)
                    {
                        if (Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var CTOESAnterior = CTO.CentroCostoPorEstacions.Where(x => x.EstacionID.Equals(Convert.ToInt32(rb["ID"])));
                            //db.CentroCostoPorEstacions.Where(x => x.CentroCosto.Equals(CTO) && x.EstacionID.Equals(Convert.ToInt32(rb["ID"])));

                            if (CTOESAnterior.Count() > 0)
                            {
                                Entidad.CentroCostoPorEstacion CTOES = CTOESAnterior.First();

                                CTOES.EstacionID = Convert.ToInt32(rb["ID"]);
                                db.SubmitChanges();
                            }
                            else if (CTOESAnterior.Count() <= 0)
                            {
                               
                                CTO.CentroCostoPorEstacions.Add(new Entidad.CentroCostoPorEstacion { EstacionID = Convert.ToInt32(rb["ID"]) });
                                db.SubmitChanges();
                            }
                        }
                        else if (!Convert.ToBoolean(rb["SelectedES"]))
                        {
                            var CTOESAnterior = CTO.CentroCostoPorEstacions.Where(x => x.EstacionID.Equals(Convert.ToInt32(rb["ID"])));

                            if (CTOESAnterior.Count() > 0)
                            {
                                db.CentroCostoPorEstacions.DeleteOnSubmit(CTOESAnterior.First());
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
    }
    
}