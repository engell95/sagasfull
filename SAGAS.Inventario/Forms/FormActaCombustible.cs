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
using DevExpress.XtraReports.UI;

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormActaCombustible : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogActaCombustible nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses(); 
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        
        #endregion

        #region <<< INICIO >>>

        public FormActaCombustible()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Editar";
            this.btnAnular.Caption = "Eliminar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(24))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                ID = iv.ID,
                                Numero = iv.Numero,
                                iv.EstacionServicioID,
                                iv.SubEstacionID,
                                EstacionNombre = iv.EstacionNombre,
                                SubEstacionNombre = iv.SubEstacionNombre,
                                FechaContabilizacion = iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Finalizado = iv.Finalizado,
                                Comentario = iv.Comentario,
                                iv.InventarioCombustibleNumero
                            };

                e.QueryableSource = query;
                e.Tag = db;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataClassesDataContext)e.Tag).Dispose();
        }

        private void ImprimirFinalizado(int ID)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();

                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Acta de Combustible " + VM.First().Numero;

                Reportes.Inventario.Hojas.RptActaCombustible rep = new Reportes.Inventario.Hojas.RptActaCombustible();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.ceCorte.Text = "Cierre al " + VM.First().FechaContabilizacion.ToShortDateString();

                var IV = dbView.VistaInventarioCombustibles.FirstOrDefault(v => v.MovimientoID.Equals(VM.First().ID));

                if (IV != null)
                {
                    rep.xrPivotGridMediciones.DataMember = "VistaInventarioCombustibleDetalles";
                    rep.xrPivotGridMediciones.DataSource = IV.VistaInventarioCombustibleDetalles;
                }

                //rep.xrPivotGridValores.DataSource = VM.First().VistaInventarioCombustibleValores;
                rep.DataSource = VM;

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                rep.Print();

                //////////////////////////// IMPRESIÓN INVENTARIO DE COMBUSTIBLE //////////////////////////////////

                var VIC = dbView.VistaInventarioCombustibles.Where(m => m.MovimientoID.Equals(VM.First().ID)).ToList();

                Reportes.FormReportViewer rv2 = new Reportes.FormReportViewer();
                rv2.Text = "Inventario Combustible " + VIC.First().Numero;

                Reportes.Inventario.Hojas.RptInventarioCombustible rep2 = new Reportes.Inventario.Hojas.RptInventarioCombustible();

                string Nombre2, Direccion2, Telefono2;
                System.Drawing.Image picture_LogoEmpresa2;
                Parametros.General.GetCompanyData(out Nombre2, out Direccion2, out Telefono2, out picture_LogoEmpresa2);
                rep2.PicLogo.Image = picture_LogoEmpresa2;
                rep2.CeEmpresa.Text = Nombre2;
                rep2.CeEstacion.Text = (VIC.First().SubEstacionID > 0 ? VIC.First().SubEstacionNombre : VIC.First().EstacionNombre);
                rep2.ceCorte.Text = "Cortado al " + VIC.First().FechaInventario.ToShortDateString();

                Entidad.ResumenDia RD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(VIC.First().EstacionServicioID) && r.SubEstacionID.Equals(VIC.First().SubEstacionID) && r.FechaInicial.Date.Equals(VIC.First().FechaInventario.Date)).FirstOrDefault();

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

                        rep2.xrPivotGridDet.DataSource = query;
                    }
                }

                rep2.xrPivotGridMediciones.DataSource = VIC.First().VistaInventarioCombustibleDetalles;
                rep2.xrPivotGridValores.DataSource = VIC.First().VistaInventarioCombustibleValores;
                rep2.DataSource = VIC;

                rep2.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv2.printControlAreaReport.PrintingSystem = rep2.PrintingSystem;
                rv2.Owner = this.Owner;
                rv2.MdiParent = this.MdiParent;
                rep2.RequestParameters = false;
                rep2.CreateDocument();
                rep2.Print();

                rv2.Close();
                rv.Close();


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override void Imprimir()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {

                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();

                    if (VM.First().Finalizado)
                    {

                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                        rv.Text = "Acta de Combustible " + VM.First().Numero;

                        Reportes.Inventario.Hojas.RptActaCombustible rep = new Reportes.Inventario.Hojas.RptActaCombustible();

                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                        rep.PicLogo.Image = picture_LogoEmpresa;
                        rep.CeEmpresa.Text = Nombre;
                        rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                        rep.ceCorte.Text = "Cierre al " + VM.First().FechaContabilizacion.ToShortDateString();

                        var IV = dbView.VistaInventarioCombustibles.FirstOrDefault(v => v.MovimientoID.Equals(VM.First().ID));

                        if (IV != null)
                        {
                            rep.xrPivotGridMediciones.DataMember = "VistaInventarioCombustibleDetalles";
                            rep.xrPivotGridMediciones.DataSource = IV.VistaInventarioCombustibleDetalles;
                        }

                        //rep.xrPivotGridValores.DataSource = VM.First().VistaInventarioCombustibleValores;
                        rep.DataSource = VM;

                        rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                        rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                        rv.Owner = this.Owner;
                        rv.MdiParent = this.MdiParent;
                        rep.RequestParameters = false;
                        rep.CreateDocument();

                        rv.Show();
                    }
                    else
                        Parametros.General.DialogMsg("El Acta de Combustible no ha sido finalizado.", Parametros.MsgType.warning);

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, int ID, int RID)
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

            if (NextRegistro)
                Add();
            else
                this.Activate();

            if (!ID.Equals(0))
                ImprimirFinalizado(ID);

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
               DateTime dia = Convert.ToDateTime(db.GetDateServer());

            if (db.ActaCombustibles.Count(ac => ac.FechaActa.Equals(dia.Date) && ac.EstacionServicioID.Equals(IDES) && ac.SubEstacionID.Equals(Parametros.General.SubEstacionID)) > 0) 
            {
                Parametros.General.DialogMsg("Ya existe una Acta de Combustible creada el  " + dia.ToShortDateString(), Parametros.MsgType.warning);
                return;
            }
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogActaCombustible(Usuario, false);
                    nf.Text = "Crear Acta de Combustible";
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
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El acta seleccionado ya fue finalizado, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID) && Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                            {
                                if (db.InventarioCombustibles.Count(iv => iv.MovimientoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))) > 0)
                                {
                                    if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                                    {
                                        nf = new Forms.Dialogs.DialogActaCombustible(Usuario, true);
                                        nf.IDInventarioCombustible = db.InventarioCombustibles.Where(iv => iv.MovimientoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).First().ID;
                                        nf.Text = "Editar Acta de Combustible";
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
                                    Parametros.General.DialogMsg("No hay un inventario de combustible asignado al acta seleccionado.", Parametros.MsgType.warning);
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
            }
            
        }

        protected override void Del()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {

                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El acta seleccionado ya fue finalizado, no se puede borrar.", Parametros.MsgType.warning);
                    else
                    {
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID) && Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                        {
                            if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                            {
                                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                {
                                    AnularMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                    FillControl(true);
                                }
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

        private void AnularMovimiento(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento M = db.Movimientos.Single(m => m.ID.Equals(ID));
                    string txt = M.Numero.ToString();
                    Entidad.InventarioCombustible IV = db.InventarioCombustibles.SingleOrDefault(ic => ic.MovimientoID.Equals(M.ID));

                    if (IV != null)
                    {
                        IV.MovimientoID = 0;
                        IV.Finalizado = false;
                    }

                    db.SubmitChanges();

                    db.ActaCombustibles.DeleteAllOnSubmit(db.ActaCombustibles.Where(ac => ac.MovimientoID.Equals(M.ID)));
                    db.Movimientos.DeleteOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                "Se borró el acta de combustible: " + txt, this.Name);

                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
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
                            Parametros.General.DialogMsg(EA.Numero.ToString() + Environment.NewLine + " El acta seleccionado ya fue finalizado, no se puede editar.", Parametros.MsgType.warning);
                        else
                        {
                            if (EA.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && EA.SubEstacionID.Equals(Parametros.General.SubEstacionID))
                            {
                                if (db.InventarioCombustibles.Count(iv => iv.MovimientoID.Equals(EA.ID)) > 0)
                                {
                                    if (Parametros.General.ValidatePeriodoContable(EA.FechaRegistro.Date, db, EA.EstacionServicioID))
                                    {
                                        var sus = db.SubEstacions.SingleOrDefault(s => s.ID.Equals(EA.SubEstacionID));
                                        nf = new Forms.Dialogs.DialogActaCombustible(Usuario, true);
                                        nf.IDInventarioCombustible = db.InventarioCombustibles.Where(iv => iv.MovimientoID.Equals(EA.ID)).First().ID;
                                        nf.Text = "Editar Acta de Combustible";
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
                                    Parametros.General.DialogMsg("No hay un inventario de combustible asignado al acta seleccionado.", Parametros.MsgType.warning);
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
