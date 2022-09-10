using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Administracion.Forms
{                                
    public partial class FormEstaciones : SAGAS.Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogEstaciones nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        
        #endregion

        #region <<< INICIO >>>

        public FormEstaciones()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
        }

        private void FormSucursal_Load(object sender, EventArgs e)
        {
            this.FillControl();
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never; 
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               
                bdsManejadorDatos.DataSource = (from ES in db.EstacionServicios
                                                where ES.Activo
                                               select ES).OrderBy(o => o.Nombre);
                this.grid.DataSource = bdsManejadorDatos;

                //**Zonas
                lkrZona.DataSource = from z in db.Zonas
                                     where z.Activo
                                        select new { z.ID, z.Nombre };

                lkrZona.DisplayMember = "Nombre";
                lkrZona.ValueMember = "ID";

                //**Administrador
                rleAdminID.DataSource = from e in db.Empleados
                                     where e.Activo
                                     select new { e.ID, Display = e.Nombres + " " +e.Apellidos };

                rleAdminID.DisplayMember = "Display";
                rleAdminID.ValueMember = "ID";

                //**Proveedor Caja Chica
                rpProveedor.DataSource = from e in db.Proveedors
                                     where e.Activo
                                     select new { e.ID, Display = e.Codigo + " | " + e.Nombre };

                rpProveedor.DisplayMember = "Display";
                rpProveedor.ValueMember = "ID";

                //**Responsable Contable
                rlkRespContID.DataSource = from e in db.Empleados
                                     where e.Activo
                                     select new { e.ID, Display = e.Nombres + " " + e.Apellidos };

                rlkRespContID.DisplayMember = "Display";
                rlkRespContID.ValueMember = "ID";

                //**Arqueador
                rpArqueadorID.DataSource = from e in db.Empleados
                                           where e.Activo
                                           select new { e.ID, Display = e.Nombres + " " + e.Apellidos };

                rpArqueadorID.DisplayMember = "Display";
                rpArqueadorID.ValueMember = "ID";

                lkCIA.DataSource = lkCIP.DataSource = from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Codigo + " | " + cc.Nombre };

                 //from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Codigo + " | " + cc.Nombre };
                
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
                    nf = new Forms.Dialogs.DialogEstaciones();
                    nf.Text = "Crear Estación de Servicio";
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
                        nf = new Forms.Dialogs.DialogEstaciones();
                        nf.Text = "Editar Estación de Servicio";
                        nf.EntidadAnterior = db.EstacionServicios.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
            //try
            //{
            //    if (nf == null)
            //    {
            //        if (gvData.FocusedRowHandle >= 0)
            //        {
            //            int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

            //            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROELIMINAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
            //            {

            //                var Z = db.Zonas.Single(c => c.ID == id);

            //                if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == Z.ID && o.Activo).Count()) > 0)
            //                {
            //                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
            //                    return;
            //                }

            //                //Desactivar Registro 
            //                Z.Activo = false;
            //                db.SubmitChanges();

            //                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó la Estación de Servicio: " + Z.Nombre, this.Name);

            //                if (ShowMsgDialog)
            //                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
            //                else
            //                    this.timerMSG.Start();

            //                FillControl();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            //}
        }
  
        #endregion        

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, "Activo");
        }
        

    }
}
