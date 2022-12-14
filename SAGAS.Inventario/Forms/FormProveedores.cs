using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormProveedores : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogProveedores nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormProveedores()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Caption = "Inactivar";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //**Tipo de Proveedor
                lkTipoProveedor.DataSource = from tp in db.TipoProveedors
                                              where tp.Activo
                                              select new { tp.ID, tp.Nombre };

                lkTipoProveedor.DisplayMember = "Nombre";
                lkTipoProveedor.ValueMember = "ID";

                //**Clase de Proveedor
                rpClaseID.DataSource = from cp in db.ProveedorClases
                                       select new { cp.ID, cp.Nombre };

                rpClaseID.DisplayMember = "Nombre";
                rpClaseID.ValueMember = "ID";

                //**Departamento
                lkDepartamento.DataSource = from d in db.Departamentos
                                             where d.Activo
                                             select new { d.ID, d.Nombre };

                lkDepartamento.DisplayMember = "Nombre";
                lkDepartamento.ValueMember = "ID";


                bdsManejadorDatos.DataSource = (from P in db.Proveedors
                                               join cc in db.CuentaContables on P.CuentaContableID equals cc.ID
                                               select new
                                               {
                                                   ID = P.ID,
                                                   Nombre = P.Nombre,
                                                   Codigo = P.Codigo,
                                                   NombreComercial = P.NombreComercial,
                                                   RUC = P.RUC,
                                                   CuentaCodigo = cc.Codigo,
                                                   CuentaNombre = cc.Nombre,
                                                   P.TipoProveedorID,
                                                   P.ProveedorClaseID,
                                                   P.DepartamentoID,
                                                   P.Contacto,
                                                   P.Direccion,
                                                   P.Telefono1,
                                                   P.Telefono2,
                                                   P.Telefono3,
                                                   P.Email,
                                                   P.WebSite,
                                                   P.LimiteCredito,
                                                   P.Plazo,
                                                   P.CuentaContableID,
                                                   P.AplicaRetencion,
                                                   ImpuestoRetencion = (P.AplicaRetencion ? db.CuentaContables.Single(c => c.ID.Equals(P.ImpuestoRetencionID)).Nombre : "< N/A >"),
                                                   P.AplicaIVA,
                                                   P.AplicaAlcaldia,
                                                   P.PagoMasivo,
                                                   P.Activo

                                               }).OrderBy(O => O.Codigo);

                this.grid.DataSource = bdsManejadorDatos;


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogProveedores();
                    nf.Text = "Crear Proveedor";
                    nf.Owner = this;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        nf = new Forms.Dialogs.DialogProveedores();
                        nf.Text = "Editar Proveedor";
                        nf.EntidadAnterior = db.Proveedors.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        nf.Owner = this;
                        nf.Editable = true;
                        nf.MDI = this;
                        nf.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        var P = db.Proveedors.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {

                                if (Convert.ToInt32(db.Movimientos.Where(o => o.ProveedorID.Equals(P.ID) && !o.Anulado).Count()) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                P.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó el Proveedor: " + P.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                P.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó el Proveedor: " + P.Nombre, this.Name);
                                this.btnAnular.Caption = "Inactivar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        #endregion

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
            {             
                try
                {
                    Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colMasivo)) || Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAplicaAlcaldia)))
                    {
                        splitContainerControlCenter.Panel2.Show();
                        int ID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colMasivo)))
                        {
                            gridES.Visible = true;
                            var obj = (from pm in db.ProveedorMasivoEstacions
                                       join p in db.Proveedors on pm.Proveedor equals p
                                       join es in db.EstacionServicios on pm.EstacionServicioID equals es.ID
                                       where p.ID.Equals(ID)
                                       select new { EstacionServicioID = es.ID, es.Nombre }).OrderBy(o => o.Nombre);

                            gridES.DataSource = obj;
                        }
                        else
                            gridES.Visible = false;

                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAplicaAlcaldia)))
                        {
                            gridAlcaldia.Visible = true;
                            var obj = (from pm in db.ProveedorAlcaldiaEstacions
                                       join p in db.Proveedors on pm.Proveedor equals p
                                       join es in db.EstacionServicios on pm.EstacionServicioID equals es.ID
                                       where p.ID.Equals(ID)
                                       select new { EstacionServicioID = es.ID, es.Nombre }).OrderBy(o => o.Nombre);

                            gridAlcaldia.DataSource = obj;
                        }
                        else
                            gridAlcaldia.Visible = false;

                        //if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colMasivo)) && !Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAplicaAlcaldia)))
                        //    gridAlcaldia.Dock = DockStyle.Fill;

                    }
                    else
                        splitContainerControlCenter.Panel2.Hide();// .Visible = false;
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

    }
}
