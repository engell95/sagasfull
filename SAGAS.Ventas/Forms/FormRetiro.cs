using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.XtraReports.UI;

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormRetiro : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogRetiro nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        
        #endregion

        #region <<< INICIO >>>

        public FormRetiro()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnModificar.Caption = "Vista Arqueo";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            //this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Glyph = Ventas.Properties.Resources.EnviarOrden;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaMovimiento);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl(false);            
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
                    rpMes.DataSource = listadoMes.GetListMeses();

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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(85))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,                                
                                iv.Numero,
                                iv.Anulado,
                                iv.MonedaID,
                                iv.Comentario,
                                //iv.Referencia,                                
                                iv.MonedaSimbolo,
                                iv.EstacionNombre,                                
                                //iv.AreaVentaNombre,
                                iv.MovimientoTipoID,
                                iv.EstacionServicioID,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,                                
                                iv.MovimientoTipoNombre,
                                iv.ConceptoContableNombre,                                
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = (iv.MonedaID.Equals(1) ? iv.Monto : iv.MontoMonedaSecundaria),
                                //iv.NombreDeudor
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

        private void ImprimirComprobante(int ID, bool ToPrint)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");

                Reportes.Ventas.Hojas.RptLiquidacionDeposito rep = new Reportes.Ventas.Hojas.RptLiquidacionDeposito();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");
                rep.xrCellDoc.Text = "Origen retiro de efectivo";
                rep.DetailReport4.Visible = false;

                rep.DataSource = VM;

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                //if (ToPrint)
                //{
                //    rep.Print();
                //    rv.Close();
                //}
                //else
                    rv.Show();

                ////*********************************************************************//////

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, bool Refresh, int ID)
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

            if (NextRegistro)
                Add();

            //if (!ID.Equals(0))
            //{
            //    ImprimirComprobante(ID, true);
            //}
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
                    nf = new Forms.Dialogs.DialogRetiro(Usuario);
                    nf.Text = "Nuevo Retiro";
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void Edit()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                        rv.previewBarTop.Visible = false;
                        rv.Text = "Vista Previa Efectivo Retiro";
                        Reportes.Util.PrintArqueoEfectivoRetiro(this, dbView, db, db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))), Parametros.TiposImpresion.Re_Impresion, false, Parametros.Properties.Resources.TXTVISTAPREVIA, rv.printControlAreaReport);
                        rv.MdiParent = this.MdiParent;
                        rv.Owner = this.Owner;
                        rv.Show();
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

            //Anular la compra
            private void AnularProvision(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    #region ::: AGREGANDO ANULACION DE MOVIMIENTO :::
                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;                    

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 86;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    //M.AreaID = Manterior.AreaID;
                    db.Movimientos.InsertOnSubmit(M);
                    db.SubmitChanges();
                    #endregion

                    #region ::: QUITANDO COMPROBANTE :::
                    int l = Manterior.ComprobanteContables.Count;
                    Manterior.ComprobanteContables.OrderBy(o => o.Linea).ToList().ForEach(linea =>
                    {
                        Entidad.ComprobanteContable CD = new Entidad.ComprobanteContable();

                        CD.CuentaContableID = linea.CuentaContableID;
                        CD.Monto = linea.Monto * (-1);
                        CD.TipoCambio = linea.TipoCambio;
                        CD.MontoMonedaSecundaria = linea.MontoMonedaSecundaria * (-1);
                        CD.Fecha = linea.Fecha;
                        CD.Descripcion = linea.Descripcion;
                        CD.EstacionServicioID = linea.EstacionServicioID;
                        CD.SubEstacionID = linea.SubEstacionID;
                        CD.CentroCostoID = linea.CentroCostoID;
                        CD.Linea = l;
                        l--;

                        M.ComprobanteContables.Add(CD);
                        db.SubmitChanges();
                    });
                    #endregion

                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.MovimientoID.Equals(ID)).FirstOrDefault();

                        if (D != null)
                        {

                            D.Pagos.ToList().ForEach(det =>
                            {
                                Entidad.Movimiento MD = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                                MD.Abono -= det.Monto;
                                MD.Pagado = false;
                                db.SubmitChanges();
                            });

                            D.Pagos.Clear();
                            db.Deudors.DeleteOnSubmit(D);
                            db.SubmitChanges();
                        }

                    #endregion

                    #region ::: QUITANDO ARQUEO :::

                    Entidad.ArqueoEfectivoRetiro AE = db.ArqueoEfectivoRetiros.SingleOrDefault(s => s.MovimientoID.Equals(ID));
                    if (AE != null)
                    {
                        AE.ArqueoEfectivoRetiroDetalles.Clear();
                        db.ArqueoEfectivoRetiros.DeleteOnSubmit(AE);
                        db.SubmitChanges();
                    }
                    #endregion

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se anuló el retiro : " + Manterior.Numero, this.Name);
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

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(50))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularProvision(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            FillControl(true);
                        }
                    }
                }
            }
        }

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(28) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(30)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridLiquidacionDeposito xtra = new Parametros.MyXtraGridLiquidacionDeposito();

                        xtra.splitContainerControlBottom.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;

                        xtra.gridDetalle.DataSource = dv.VistaPagos.Where(v => M.VistaDeudors.Select(s => s.ID).Contains(v.DeudorID));
                        xtra.gridNotas.DataSource = M.VistaMinutasDeposito;
                        xtra.gridComprobante.DataSource = M.VistaComprobantes;

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
              
        #endregion   
        
    }
}
