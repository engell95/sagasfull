using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormAjusteInventario : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogAjusteInventario nf;
        private Forms.Dialogs.DialogAjusteInventario nfPrint;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormAjusteInventario()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;            
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaMovimiento);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            FillControl(false);            
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl(bool Refresh)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    
                if (Refresh)
                    linqInstantFeedbackSource1.Refresh();
                else
                {
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                    lkMes.DataSource = listadoMes.GetListMeses();

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaContabilizacion") > fecha.AddDays(-1)).ToString();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(9))
                            join inv in dv.VistaInventarioFisicos on iv.ID equals inv.MovimientoID
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                ID = iv.ID,
                                Numero = iv.Numero,
                                EstacionNombre = iv.EstacionNombre,
                                SubEstacionNombre = iv.SubEstacionNombre,
                                FechaContabilizacion = iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Finalizado = iv.Finalizado,
                                Comentario = iv.Comentario,
                                Area = inv.AreaNombre,
                                iv.InventarioFisicoNumero,
                                iv.EstacionServicioID,
                                iv.InventarioBloqueado,
                                iv.SubEstacionID
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

        protected override void Imprimir()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();

                    if (!VM.First().Finalizado)
                        Parametros.General.DialogMsg("El Ajuste no se ha Finalizado.", Parametros.MsgType.warning);
                    else
                    {
                        if (nfPrint == null)
                        {
                            nfPrint = new Forms.Dialogs.DialogAjusteInventario(Usuario, true, true);
                            nfPrint.InventarioFisicoMDI = db.InventarioFisicos.Where(iv => iv.MovimientoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).First().ID;
                            nfPrint.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nfPrint.Owner = this.Owner;
                            nfPrint.MdiParent = this.MdiParent;
                            nfPrint.MDI = this;
                            nfPrint.Show();
                            nfPrint = null;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void ImprimirFinalizado(int _ID)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(_ID)).ToList();

                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogAjusteInventario(Usuario, true, true);
                    nf.IDInventarioFisico = db.InventarioFisicos.Where(iv => iv.MovimientoID.Equals(_ID)).First().ID;
                    nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(_ID));
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();

                    nf = null;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool Refresh, bool Send, int Id, int RID)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

                FillControl(true);
                this.Activate();

                if (Send)
                    ImprimirFinalizado(Id);

                if (!RID.Equals(0))
                    ReEdit(RID);
                                
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
                    nf = new Forms.Dialogs.DialogAjusteInventario(Usuario, false, false);
                    nf.Text = "Crear Nuevo Ajuste de Inventario Físico";
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();
                }
                else
                    nf.Activate();
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

                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El ajuste seleccionado ya fue finalizado, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID) && Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                            {
                                if (db.InventarioFisicos.Count(iv => iv.MovimientoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))) > 0)
                                {
                                    if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                                    {

                                        nf = new Forms.Dialogs.DialogAjusteInventario(Usuario, true, false);
                                        nf.IDInventarioFisico = db.InventarioFisicos.Where(iv => iv.MovimientoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).First().ID;
                                        nf.Text = "Editar Ajuste de Inventario Físico";
                                        nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                                            (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                                        nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                        nf.Owner = this.Owner;
                                        nf.MdiParent = this.MdiParent;
                                        nf.MDI = this;
                                        nf.Show();
                                    }
                                    else
                                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                }
                                else
                                {
                                    Parametros.General.DialogMsg("No hay un levantamiento de inventario asignado al ajuste seleccionado.", Parametros.MsgType.warning);
                                }
                            }
                            else
                                Parametros.General.DialogMsg("El Registro esta en otra Estación", Parametros.MsgType.warning);

                        }
                    }
                }
                else
                    nf.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        private void ReEdit(int ID)
        {
            try
            {
                if (nf == null)
                {
                    if (ID > 0)
                    {

                        Entidad.Movimiento EA = db.Movimientos.Single(e => e.ID.Equals(ID));

                        if (EA.Finalizado.Equals(true))
                            Parametros.General.DialogMsg(EA.Numero.ToString() + Environment.NewLine + " El ajuste seleccionado ya fue finalizado, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            if (EA.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && EA.SubEstacionID.Equals(Parametros.General.SubEstacionID))
                            {
                                if (db.InventarioFisicos.Count(iv => iv.MovimientoID.Equals(EA.ID)) > 0)
                                {
                                    if (Parametros.General.ValidatePeriodoContable(EA.FechaRegistro.Date, db, EA.EstacionServicioID))
                                    {
                                        var sus = db.SubEstacions.SingleOrDefault(s => s.ID.Equals(EA.SubEstacionID));
                                        nf = new Forms.Dialogs.DialogAjusteInventario(Usuario, true, false);
                                        nf.IDInventarioFisico = db.InventarioFisicos.Where(iv => iv.MovimientoID.Equals(EA.ID)).First().ID;
                                        nf.Text = "Editar Ajuste de Inventario Físico";
                                        nf.layoutControlGroup1.Text += " " + db.EstacionServicios.Single(s => s.ID.Equals(EA.EstacionServicioID)).Nombre + " | " +
                                            (sus == null ? "" : sus.Nombre);
                                        nf.EntidadAnterior = EA;
                                        nf.ShowFinish = true;
                                        nf.Owner = this.Owner;
                                        nf.MdiParent = this.MdiParent;
                                        nf.MDI = this;
                                        nf.Show();
                                    }
                                    else
                                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                }
                                else
                                {
                                    Parametros.General.DialogMsg("No hay un levantamiento de inventario asignado al ajuste seleccionado.", Parametros.MsgType.warning);
                                }
                            }
                            else
                                Parametros.General.DialogMsg("El Registro esta en otra Estación", Parametros.MsgType.warning);

                        }
                    }
                }
                else
                    nf.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    //dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    //var OC = dv.VistaOrdenesCompras.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    //DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    //page.Text = "Orden " + OC.Numero;
                    //page.Tag = OC.ID;

                    //Parametros.MyXtraGridOCD xtra = new Parametros.MyXtraGridOCD();
                    //xtra.gridDetalle.DataSource = OC.VistaOrdenesComprasDetalles;

                    //if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    //{
                    //    this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    //}
                    //else
                    //{
                    //    xtra.Dock = DockStyle.Fill;
                    //    page.Controls.Add(xtra);
                    //    this.xtraTabControlMain.TabPages.Add(page);
                    //    this.xtraTabControlMain.SelectedTabPage = page;
                    //    paginas.Add(Convert.ToInt32(page.Tag));
                    //}
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion  
       
    }
}
