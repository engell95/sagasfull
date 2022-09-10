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
    public partial class FormZona : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogZona nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormZona()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Inactivar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this); 
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

                bdsManejadorDatos.DataSource = from Z in db.Zonas
                                               select Z;

                
                this.grid.DataSource = bdsManejadorDatos;

                //**Empresas
                lkrEmpresa.DataSource = from e in db.Empresas
                                        where e.Activo
                                                 select new { e.ID, e.Nombre };

                lkrEmpresa.DisplayMember = "Nombre";
                lkrEmpresa.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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
                        var Z = db.Zonas.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == Z.ID && o.Activo).Count()) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                //Desactivar Registro 
                                Z.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó la Zona: " + Z.Nombre, this.Name);
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
                                //Desactivar Registro 
                                Z.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se activó la Zona: " + Z.Nombre, this.Name);
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
              
        #endregion
        
        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, "Activo");
        }

        
    }
}
