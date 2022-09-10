using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.Data.Linq;

namespace SAGAS.Administracion.Forms
{
    public partial class FormAudit : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataViewsDataContext dv;

        #endregion

        #region *** INICIO ***

        public FormAudit()
        {
            InitializeComponent(); 
            CheckForIllegalCrossThreadCalls = false;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaAuditoria);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                

                var collection = Enum.GetValues(typeof(Parametros.TipoAccion));

                lkTipo.Properties.Items.AddRange(collection);
                lkTipo.Properties.Items.Add("<TODOS>");
                lkTipo.SelectedItem = "<TODOS>";

                dateInicial.DateTime = dateFinal.DateTime = DateTime.Now.Date;

    //.Cast<string>()
    //.Select(v => v.ToString())
    //.ToList();
                   // Enum.GetValues(typeof(Parametros.TipoAccion)).Cast<string>(); 
                //var obj = (from a in db.Audits
                //          select a).OrderByDescending(o => o.Date);

                          //{
                          //    ID = lb.LogBookID,
                          //    User = u.UserName,
                          //    Rol = r.RolName,
                          //    ActionType = lb.ActionType,
                          //    Description = lb.Action,
                          //    Computadora = lb.Host,
                          //    Date = lb.Date
                          //}).OrderByDescending(o => o.Date);
                
                //this.DataGrid.DataSource = obj;

                ////**Estaciones Servicio
                //rpEstacionServicio.DataSource = from es in db.EstacionServicios
                //                        where es.Activo
                //                        select new { es.ID, es.Nombre };

                //rpEstacionServicio.DisplayMember = "Nombre";
                //rpEstacionServicio.ValueMember = "ID";

                ////**Estaciones Servicio
                //rpUsuario.DataSource = from u in db.Usuarios
                //                                where u.Activo
                //                                select new { u.ID, u.Nombre};

                //rpUsuario.DisplayMember = "Nombre";
                //rpUsuario.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        #region *** EVENTOS ***
        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dv.CommandTimeout = 6000;
                var query = from a in dv.VistaAuditoria 
                                where a.Date.Date >= dateInicial.DateTime.Date && a.Date.Date <= dateFinal.DateTime.Date
                                && (a.TipoAccion.Equals(lkTipo.SelectedItem) || lkTipo.SelectedItem.Equals("<TODOS>"))// && !iv.Finalizado
                            select new
                            {
                                a.ID
      ,
                                a.UsuarioID
      ,
                                a.Usuario
      ,
                                a.EstacionServicioID
      ,
                                a.EstacionServicio
      ,
                                a.TipoAccion
      ,
                                a.Accion
      ,
                                a.Acceso
      ,
                                a.Date
      ,
                                a.Computadora
      ,
                                a.Campo
      ,
                                a.ValorAntesCampo
      ,
                                a.ValorDespuesCampo
                            };

                e.QueryableSource = query;
                e.Tag = dv;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Parametros.General.PrintExportComponet(true, this.Text, this.MdiParent, null, null, DataGridTabla, false, 25, 25, 140, 50, 0);
        }

        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.dglExportToFile.Filter = "Hoja de Microsoft Excel (*.xlsx)|*.xlsx";

            if (this.dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.dglExportToFile.FileName != "")
                {
                    this.DataGridTabla.MainView.ExportToXlsx(this.dglExportToFile.FileName);

                }
            }
        }

        #endregion

        private void btnLoad_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataGridTabla.DataSource = null;
            DataGridTabla.DataSource = linqInstantFeedbackSource1;
            GridViewTabla.RefreshData();
        }
        
    }
}