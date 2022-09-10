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
    public partial class FormInventarioCombustible : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogInventarioCombustible nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();  
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormInventarioCombustible()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.btnAnular.Caption = "Eliminar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaInventarioCombustible);
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
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaInventario") > fecha.AddDays(-1)).ToString();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from iv in dv.VistaInventarioCombustibles
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaInventario,
                                Year = iv.FechaInventario.Year,
                                Mes = iv.FechaInventario.Month,
                                iv.Finalizado,
                                iv.MovimientoNumero,
                                iv.EstacionServicioID,
                                iv.SubEstacionID,
                                iv.MovimientoID
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
                    var VM = dbView.VistaInventarioCombustibles.Where(m => m.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();

                    //if (!VM.First().MovimientoID.Equals(0))
                    //{
                    //    if (dbView.VistaMovimientos.First(v => v.ID.Equals(VM.First().MovimientoID)).Finalizado)
                    //    {

                            Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                            rv.Text = "Inventario Combustible " + VM.First().Numero;

                            Reportes.Inventario.Hojas.RptInventarioCombustible rep = new Reportes.Inventario.Hojas.RptInventarioCombustible();

                            string Nombre, Direccion, Telefono;
                            System.Drawing.Image picture_LogoEmpresa;
                            Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                            rep.PicLogo.Image = picture_LogoEmpresa;
                            rep.CeEmpresa.Text = Nombre;
                            rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                            rep.ceCorte.Text = "Cortado al " + VM.First().FechaInventario.ToShortDateString();

                            Entidad.ResumenDia RD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))) && r.SubEstacionID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID"))) && r.FechaInicial.Date.Equals(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaInventario")).Date)).FirstOrDefault();

                            if (RD != null)
                            {
                                Entidad.Turno T = RD.Turnos.OrderByDescending(t => t.Numero).FirstOrDefault();

                                if (T != null)
                                {

                                    var query = (from am in db.ArqueoMangueras
                                                 join m in db.Mangueras on am.MangueraID equals m.ID
                                                 join d in db.Dispensadors on m.DispensadorID equals d.ID
                                                 join i in db.Islas on d.IslaID equals i.ID
                                                 join ap in db.ArqueoProductos on am.ArqueoProductoID equals ap.ID
                                                 join p in db.Productos on ap.ProductoID equals p.ID
                                                 join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                                 join t in db.Turnos on ai.TurnoID equals t.ID
                                                 join r in db.ResumenDias on t.ResumenDiaID equals r.ID
                                                 where r.ID.Equals(RD.ID) && t.ID.Equals(T.ID) && d.Activo
                                                 select new
                                                 {
                                                     ResumenDiaID = r.ID,
                                                     r.FechaInicial,
                                                     Turno = t.ID,
                                                     Combustible = p.Codigo + " | " + p.Nombre,
                                                     Isla = i.Nombre,
                                                     Manguera = m.Numero,
                                                     Sello = d.Sello,
                                                     Lado = "Lado " + m.Lado,
                                                     Dispensador = i.Nombre + " | " + d.Nombre,
                                                     am.LecturaElectronicaFinal,
                                                     am.LecturaMecanicaFinal,
                                                     am.LecturaEfectivoFinal
                                                 }).ToList();

                                    rep.xrPivotGridDet.DataSource = query;
                                }
                            }

                            rep.xrPivotGridMediciones.DataSource = VM.First().VistaInventarioCombustibleDetalles;
                            rep.xrPivotGridValores.DataSource = VM.First().VistaInventarioCombustibleValores;
                            rep.DataSource = VM;

                            rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                            rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                            rv.Owner = this.Owner;
                            rv.MdiParent = this.MdiParent;
                            rep.RequestParameters = false;
                            rep.CreateDocument();

                            rv.Show();

                        //}
                        //else
                        //    Parametros.General.DialogMsg("El Acta de Combustible no ha sido finalizado.", Parametros.MsgType.warning);

                    //}
                    //else
                    //    Parametros.General.DialogMsg("El inventario de Combustible no esta asignado a ningún ajuste.", Parametros.MsgType.warning);

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

        internal void CleanDialog(bool ShowMSG, bool Refresh, bool Next)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if (Refresh)
                FillControl(true);

            if (Next)
                Add();
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
                    nf = new Forms.Dialogs.DialogInventarioCombustible(Usuario, false);
                    nf.Text = "Crear Nuevo Inventario de Combustible";
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
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventario de combustible ya fue ingresado a un acta, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            //if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID) && Convert.ToBoolean(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                            //{
                            if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaInventario")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                                {
                            nf = new Forms.Dialogs.DialogInventarioCombustible(Usuario, true);
                            nf.Text = "Editar Inventario de Combustible";
                            nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                                (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                            nf.EntidadAnterior = db.InventarioCombustibles.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nf.Owner = this.Owner;
                            nf.Editable = true;
                            nf.MdiParent = this.MdiParent;
                            nf.MDI = this;
                            nf.Show();
                                }
                            else
                                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                            //}
                            //else
                            //    Parametros.General.DialogMsg("El Registro esta en otra Estación", Parametros.MsgType.warning);

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

        protected override void Del()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventario de combustible ya fue ingresado a un acta, no se puede borrar.", Parametros.MsgType.warning);
                    else
                    {
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID) && Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                        {
                            if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaInventario")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                            {

                                Entidad.InventarioCombustible IC = db.InventarioCombustibles.Single( i => i.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue("ID"))));
                                string txt = "";

                                IC.InventarioCombustibleDetalles.Clear();
                                IC.InventarioCombustibleValores.Clear();

                                txt = IC.Numero.ToString();

                                db.InventarioCombustibles.DeleteOnSubmit(IC);
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                "Se borró el inventario de combustible: " + txt, this.Name);

                               CleanDialog(true, true, false);
                            }
                            else
                                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        }
                        else
                            Parametros.General.DialogMsg("El Registro esta en otra Estación", Parametros.MsgType.warning);

                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
