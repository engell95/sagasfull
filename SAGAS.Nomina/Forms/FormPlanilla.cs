using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormPlanilla : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPlanilla nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormPlanilla()
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

                bdsManejadorDatos.DataSource = from p in db.Planillas
                                               join eo in db.EstructuraOrganizativa on p.EstructuraOrganizativaID equals eo.ID into DefaultEstruct
                                               from Estruct in DefaultEstruct.DefaultIfEmpty()
                                               join es in db.EstacionServicios on p.EstacionServicioID equals es.ID into DefaultEstacion
                                               from Estacion in DefaultEstacion.DefaultIfEmpty()
                                               join ses in db.SubEstacions on p.SubEstacionID equals ses.ID into DefaultSub
                                               from Sub in DefaultSub.DefaultIfEmpty()
                                               where p.Activo
                                               select new
                                               {
                                                   p.ID,
                                                   p.Nombre,
                                                   p.Comentario,
                                                   p.NumeroPlanilla,
                                                   EO = Estruct.Nombre,
                                                   ES = Estacion.Nombre,
                                                   SES = Sub.Nombre
                                               };

                
                this.grid.DataSource = bdsManejadorDatos;

                
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
                    nf = new Forms.Dialogs.DialogPlanilla(Usuario);
                    nf.Text = "Crear Planilla";
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
                        nf = new Forms.Dialogs.DialogPlanilla(Usuario);
                        nf.Text = "Editar Planilla";
                        nf.EntidadAnterior = db.Planillas.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                //if (nf == null)
                //{
                //    if (gvData.FocusedRowHandle >= 0)
                //    {
                //        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                //        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                //        {

                //            var Z = db.Zonas.Single(c => c.ID == id);

                //            if (Convert.ToInt32(db.EstacionServicios.Where(o => o.ZonaID == Z.ID && o.Activo).Count()) > 0)
                //            {
                //                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEBORRAR + Environment.NewLine, Parametros.MsgType.warning);
                //                return;
                //            }

                //            //Desactivar Registro 
                //            Z.Activo = false;
                //            db.SubmitChanges();

                //            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa, "Se Inactivó la Planila: " + Z.FechaFin, this.Name);

                //            if (ShowMsgDialog)
                //                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                //            else
                //                this.timerMSG.Start(); 

                //            FillControl();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
              
        #endregion

        
    }
}
