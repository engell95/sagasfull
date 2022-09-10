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
    public partial class FormTipoCuentaMovimiento : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogTipoCuentaModulo nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormTipoCuentaMovimiento()
        {
            InitializeComponent();
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAgregar.Visibility = this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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

                bdsManejadorDatos.DataSource = from mv in db.MovimientoTipos
                                               join tic in db.TipoCuentaModulo on mv.ID equals tic.MovimientoTipoID
                                               where mv.ID == 29
                                               group mv by new { mv.Nombre } into gr
                                               select new { gr.Key.Nombre };

                this.gridTipo .DataSource= bdsManejadorDatos;
/*
                bdsManejadorDatos.DataSource = from Z in db.Zonas
                                               where Z.Activo
                                               select Z;

                
                this.grid.DataSource = bdsManejadorDatos;

                //**Empresas
                lkrEmpresa.DataSource = from e in db.Empresas
                                        where e.Activo
                                                 select new { e.ID, e.Nombre };

                lkrEmpresa.DisplayMember = "Nombre";
                lkrEmpresa.ValueMember = "ID";
                */
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(gridTipo);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(gridTipo);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(gridTipo);
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
            this.gridCuenta.DataSource = null;
            this.gVdata2.RefreshData();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            /*  try
              {
                  if (nf == null)
                  {
                      nf = new Forms.Dialogs.DialogZona();
                      nf.Text = "Crear Zona";
                      nf.Owner = this;
                      nf.MDI = this;
                      nf.Show();
                  } 
              }
              catch (Exception ex)
              {
                  Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                  Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
              } */
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null) {

                    if (gvData.FocusedRowHandle >= 0){
                     
                    nf = new Forms.Dialogs.DialogTipoCuentaModulo();
                    nf.Text = "Editar movimiento";
                    nf._ID = 29;
                    nf.txtNombre.Text = Convert.ToString(gvData.GetFocusedRowCellValue(colTipoMov));
                    nf.MDI = this;
                    nf.Owner = this;
                    nf.Show();
                    }
                }

            }
            catch (Exception ex) {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            #region <<Comentarios>>
            /* try
             {
                 if (nf == null)
                 {
                     if (gvData.FocusedRowHandle >= 0)
                     {
                         nf = new Forms.Dialogs.DialogZona();
                         nf.Text = "Editar Zona";
                         nf.EntidadAnterior = db.Zonas.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                 Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
             } */
            # endregion 
        }

        protected override void Del()
        {
           /* try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            var Z = db.Zonas.Single(c => c.ID == id);

                            if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == Z.ID && o.Activo).Count()) > 0)
                            {
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                return;
                            }

                            //Desactivar Registro 
                            Z.Activo = false;
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó la Zona: " + Z.Nombre, this.Name);

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start(); 

                            FillControl();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            } */
        }
              
        #endregion

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                gridCuenta.DataSource = from tic in db.TipoCuentaModulo
                                        join mv in db.MovimientoTipos on tic.MovimientoTipoID equals mv.ID
                                        join tc in db.TipoCuentas on tic.TipoCuentaID equals tc.ID
                                        where tic.MovimientoTipoID == 29
                                        select new { tic.TipoCuentaID, tic.MovimientoTipoID, tc.Nombre };

                this.gVdata2.RefreshData();
            }
            catch(Exception ex) {
            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            
            }
        }

        

        
    }
}
