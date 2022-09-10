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
    public partial class FormMovimientoCorteEmpleado : SAGAS.Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogMovimientoCorteEmpleado nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        
        #endregion

        #region <<< INICIO >>>

        public FormMovimientoCorteEmpleado()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
        }

        private void FormSucursal_Load(object sender, EventArgs e)
        {
            this.FillControl();
            this.btnAnular.Caption = "Eliminar";
            this.barImprimir.Caption = "Vista Previa";
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            { 
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                bdsManejadorDatos.DataSource = from me in db.MovimientoHorasExtras
                                               join pl in db.Planillas on me.PlanillaID equals pl.ID
                                               join es in db.EstacionServicios on pl.EstacionServicioID equals es.ID
                                               where (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                               select new
                                               {
                                                   me.ID,
                                                   me.Approved,
                                                   me.Comment,
                                                   me.FechaFinal,
                                                   me.FechaInicial,
                                                   me.PlanillaID,
                                                   Planilla = pl.Nombre
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
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var VM = dv.VistaMovimientoEmpleadoHorasExtras.Where(m => m.MovimientoHorasExtrasID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).ToList();
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    rv.Text = "HE " + VM.First().FechaFinalMaestroHE.ToShortDateString();

                    Reportes.Nomina.Hojas.ReporteFormatoHE rep = new Reportes.Nomina.Hojas.ReporteFormatoHE();

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.pbLogo.Image = picture_LogoEmpresa;
                    rep.xrCellEmpresa.Text = Nombre;
                    rep.xrCellEstacion.Text = VM.First().NombrePlanilla;

                    rep.DataSource = VM;

                    var admon = from em in db.Empleados
                                join es in db.EstacionServicios on em.ID equals es.AdministradorID
                                where es.ID.Equals(VM.First().EstacionID)
                                select new { Display = em.Nombres + " " + em.Apellidos, Preparado = "Administrador " + es.Nombre };

                    if (admon.Count() > 0)
                    {
                        rep.xrCellPreparadoPor.Text = admon.First().Display;
                        rep.xrCellPreparadoEstacion.Text = admon.First().Preparado;
                    }

                    rep.lblFecha.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                    rv.Owner = this.Owner;
                    rv.MdiParent = this.MdiParent;
                    rep.RequestParameters = false;
                    rep.CreateDocument();
                    rv.Show();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
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

        internal void CleanDialog(bool ShowMSG, bool RefreshMDI)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if(RefreshMDI)
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
                    nf = new Forms.Dialogs.DialogMovimientoCorteEmpleado(Parametros.General.UserID, false);
                    nf.Text = "Crear Registro de Horas Extras";
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
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
            if (gvData.FocusedRowHandle >= 0)
                try
                {
                    if (nf == null)
                    {
                        if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAprobado)))
                        {
                            nf = new Forms.Dialogs.DialogMovimientoCorteEmpleado(Parametros.General.UserID, true);
                            nf.Text = "Editar Registro de Horas Extras";
                            nf._IDHE = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                            nf.Owner = this.Owner;
                            nf.MdiParent = this.MdiParent;
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

                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + Environment.NewLine + "Horas Extras al corte " + Convert.ToDateTime(gvData.GetRowCellValue(gvData.FocusedRowHandle, colFechaFinal)).ToShortDateString(), Parametros.MsgType.question) == DialogResult.OK)
                        {

                            Entidad.MovimientoHorasExtras MHE = db.MovimientoHorasExtras.Single(me => me.ID == id);

                            if (MHE.Approved)
                            {
                                Parametros.General.DialogMsg("No se puede Eliminar el Registro" + Environment.NewLine + "Estan Aprobadas las Horas Extras al corte " + Convert.ToDateTime(gvData.GetRowCellValue(gvData.FocusedRowHandle, colFechaFinal)).ToShortDateString(), Parametros.MsgType.warning);
                                return;
                            }

                            //Verificar en Nomina
                            //if (db.ProductoClases.Count(o => o.AreaID.Equals(ME.ID) && o.Activo) > 0)
                            //{
                            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                            //    return;
                            //}

                            //if (A.ID.Equals(Parametros.Config.ProductoAreaServicioID()))
                            //{
                            //    Parametros.General.DialogMsg("El Área esta asignada como Área de servicios en las configuración general del sistema", Parametros.MsgType.warning);
                            //    return;
                            //}
                            //Desactivar Registro 

                            string vPlanilla, vFecha;

                            vPlanilla = Convert.ToString(gvData.GetFocusedRowCellDisplayText(colPlanilla));
                            vFecha = Convert.ToDateTime(gvData.GetFocusedRowCellDisplayText(colFechaFinal)).ToShortDateString();

                            db.MovimientoEmpleado.DeleteAllOnSubmit(db.MovimientoEmpleado.Where(o => o.MovimientoHorasExtrasID.Equals(MHE.ID)));
                            db.SubmitChanges();

                            db.MovimientoHorasExtras.DeleteOnSubmit(MHE);
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina, "Se eliminaron las Horas Extras de la planilla : " + vPlanilla + " con fecha de corte al " + vFecha, this.Name);

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
            }
        }

        private void xtraTabControlMain_CloseButtonClick(object sender, EventArgs e)
        {
            this.paginas.Remove(Convert.ToInt32(xtraTabControlMain.SelectedTabPage.Tag));
            this.xtraTabControlMain.TabPages.Remove(xtraTabControlMain.SelectedTabPage);

            if (this.xtraTabControlMain.TabPages.Count > 0)
            {
                this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages[(this.xtraTabControlMain.TabPages.Count - 1)];
            }
        }

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var M = dv.VistaMovimientoEmpleadoHorasExtras.Where(o => o.MovimientoHorasExtrasID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = "HE " + Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaFinal)).ToShortDateString();
                    page.Tag = M.First().MovimientoHorasExtrasID;

                    Parametros.MyXtraGridHorasExtras xtra = new Parametros.MyXtraGridHorasExtras();
                    xtra.grid.DataSource = M;
                    xtra.gvData.RefreshData();

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {
                        xtra.Dock = DockStyle.Fill;
                        page.Controls.Add(xtra);
                        this.xtraTabControlMain.TabPages.Add(page);
                        this.xtraTabControlMain.SelectedTabPage = page;
                        paginas.Add(Convert.ToInt32(page.Tag));
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
  
        private void chkAprovado_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkAprovado"))
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        bool Finished = !Convert.ToBoolean(gvData.GetRowCellValue(gvData.FocusedRowHandle, colAprobado));

                        if (Parametros.General.DialogMsg("¿Desea " + (Finished ? "Aprobar" : "Desaprobar") + " las Horas Extras al corte " + Convert.ToDateTime(gvData.GetRowCellValue(gvData.FocusedRowHandle, colFechaFinal)).ToShortDateString() + "?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            Entidad.MovimientoHorasExtras M = db.MovimientoHorasExtras.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(gvData.FocusedRowHandle, colID))));

                            //Verificar si las Horas Extras ya estan generadas en Nomina
                            //var Mov = db.Movimientos.FirstOrDefault(f => f.ID.Equals(I.MovimientoID));

                            //if (Mov != null)
                            //{
                            //    if (Mov.Finalizado)
                            //    {
                            //        Parametros.General.DialogMsg("El Ajuste para el Inventario Físico " + Convert.ToString(gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2)) + " ya esta finalizado y no se puede reaperturar.", Parametros.MsgType.warning);
                            //        return;
                            //    }
                            //    else
                            //    {
                            //        I.Finalizado = Finished;

                            //        db.SubmitChanges();

                            //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                            //            "Se " + (Finished ? "Finalizó" : "Aperturó") + "el Invetario Físico: " + gvData.GetRowCellValue(gvData.FocusedRowHandle, gridColumn2).ToString(), this.Name);
                            //        this.timerMSG.Start();
                            //        FillControl(true);
                            //        gvData.FocusedColumn = gridColumn6;
                            //    }
                            //}
                            //else
                            //{
                                M.Approved = Finished;

                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                                    "Se " + (Finished ? "Aprobó" : "Desaprobó") + " las Horas Extras al corte : " + Convert.ToDateTime(gvData.GetRowCellValue(gvData.FocusedRowHandle, colFechaFinal)).ToShortDateString(), this.Name);
                                this.timerMSG.Start();
                                FillControl();
                                gvData.FocusedColumn = colComentario;
                            //}
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

    }
}
