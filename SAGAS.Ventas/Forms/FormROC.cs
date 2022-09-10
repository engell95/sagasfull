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
using DevExpress.XtraReports.UI;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Ventas.Forms
{
    public partial class FormROC : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogROC nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();  
        private List<int> lista;
        private List<int> paginas = new List<int>();
        
        #endregion

        #region <<< INICIO >>>

        public FormROC()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(22) || v.MovimientoTipoID.Equals(23))
                            where lista.Contains(iv.EstacionServicioID) //&& !iv.ConceptoContableID.Equals(12)
                            select new
                            {
                                iv.ID,
                                iv.Referencia,
                                iv.Numero,
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Deudor = (iv.ClienteID.Equals(0) ? iv.EmpleadoNombres + " " + iv.EmpleadoApellidos : iv.ClienteCodigo + " | " + iv.ClienteNombre),
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = iv.Monto,
                                iv.Pagado
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
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero;

                Reportes.Ventas.Hojas.RptROC rep = new Reportes.Ventas.Hojas.RptROC();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeRuc.Text = Ruc;
                rep.CeMovimiento.Text = VM.First().MovimientoTipoNombre;
                rep.CeNumero.Text = VM.First().Numero.ToString("000000000");
                
                //Datos del Deudor
                if (!String.IsNullOrEmpty(VM.First().ClienteNombre))
                {                   
                rep.CeDeudor.Text = VM.First().ClienteNombre;
                rep.CeRucInss.Text = "Nro. RUC   : ";
                rep.CeRucInssValor.Text = db.Clientes.Single(c => c.ID.Equals(VM.First().ClienteID)).RUC;
                }
                else
                {
                    rep.CeDeudor.Text = VM.First().EmpleadoNombres + " " + VM.First().EmpleadoApellidos;
                }

                //Datos del Encabezado
                if (VM.First().SubEstacionID > 0)
                {
                    var Sub = db.SubEstacions.Single(s => s.ID.Equals(VM.First().SubEstacionID));
                    rep.CeEstacion.Text = Sub.Nombre;
                    rep.CeDireccion.Text = "Dirección : " + Sub.Direccion;
                    rep.CeTelefono.Text = "Teléfono : " + Sub.Telefono;
                }
                else
                {
                    var ES = db.EstacionServicios.Single(s => s.ID.Equals(VM.First().EstacionServicioID));
                    rep.CeEstacion.Text = ES.Nombre;
                    rep.CeDireccion.Text = "Dirección : " + ES.Direccion;
                    rep.CeTelefono.Text = "Teléfono : " + ES.Telefono;
                }

                rep.CeFecha.Text = VM.First().FechaContabilizacion.Day + " " + Parametros.General.GetMonthInLetters(VM.First().FechaContabilizacion.Month) + " " + VM.First().FechaContabilizacion.Year;

                rep.CeMoneda.Text = VM.First().MonedaSimbolo;                
                rep.CeTipoCambio.Text = VM.First().TipoCambio.ToString("#,0.0000");
                rep.CeComentario.Text = VM.First().Comentario;
                
                //Tipo de Moneda
                if (VM.First().MonedaID.Equals(1))
                {
                    rep.CeMonto.Text = VM.First().Monto.ToString("#,0.00");
                    rep.CeLetras.Text = Parametros.General.enletras(VM.First().Monto.ToString()) + " " + db.Monedas.Single(s => s.ID.Equals(VM.First().MonedaID)).Nombre;
                    rep.CeValorCS.Text = "";
                    rep.CeMonedaCS.Text = "";
                    rep.CeMontoCS.Text = "";
                }
                else
                {
                    rep.CeMonto.Text = VM.First().MontoMonedaSecundaria.ToString("#,0.00");
                    rep.CeLetras.Text = Parametros.General.enletras(VM.First().MontoMonedaSecundaria.ToString()) + " " + db.Monedas.Single(s => s.ID.Equals(VM.First().MonedaID)).Nombre;
                    rep.CeValorCS.Text = "";
                    rep.CeMonedaCS.Text = "";
                    rep.CeMontoCS.Text = "";
                }

                rep.DataSource = VM;
           
                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                if (ToPrint)
                {
                    rep.Print();
                    rv.Close();
                }
                else
                    rv.Show();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void AnularMovimiento(int ID, int Tipo)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Manterior.ID)) > 0)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + (Manterior.Numero.Equals(0) ? Manterior.Referencia : Manterior.Numero.ToString()) + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;

                    #region <<< ANULACION_ROC >>>

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EmpleadoID = Manterior.EmpleadoID;
                    M.ClienteID = Manterior.ClienteID;
                    M.MovimientoTipoID = 23;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.Comentario = Manterior.Comentario;

                    db.Movimientos.InsertOnSubmit(M);
                    db.SubmitChanges();
                    
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
                        
                    });
                    db.SubmitChanges();
                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.ClienteID.Equals(M.ClienteID) && d.EmpleadoID.Equals(M.EmpleadoID) && d.MovimientoID.Equals(ID)).FirstOrDefault();

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


                    #endregion
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                                "Se anuló el ROC: " + Manterior.Numero, this.Name);
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

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(23))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, int ID, bool Refresh)
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

            if (!ID.Equals(0))
                ImprimirComprobante(ID, true);
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
                    nf = new Forms.Dialogs.DialogROC(Usuario, false);
                    nf.Text = "Crear Nuevo ROC";
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
                    //if (gvData.FocusedRowHandle >= 0)
                    //{

                    //    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                    //        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " Este inventrio físico ya fue ingresado a un ajuste, no se puede editar.", Parametros.MsgType.warning);
                    //    else
                    //    {
                    //        nf = new Forms.Dialogs.DialogInventarioFisico(Usuario, true);
                    //        nf.Text = "Editar Levantamiento de Inventario Físico";
                    //        nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue("EstacionNombre").ToString() + " | " +
                    //            (gvData.GetFocusedRowCellValue("SubEstacionNombre") == null ? "" : gvData.GetFocusedRowCellValue("SubEstacionNombre").ToString());
                    //        nf.EntidadAnterior = db.InventarioFisicos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                    //        nf.Owner = this.Owner;
                    //        nf.MdiParent = this.MdiParent;
                    //        nf.MDI = this;
                    //        nf.Show();
                    //    }
                    //}
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
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(23))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
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
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridROC xtra = new Parametros.MyXtraGridROC();
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(23))
                            xtra.splitContainerControlMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                        else
                            xtra.gridDetalle.DataSource = from p in db.Pagos
                                                          join d in db.Deudors on p.DeudorID equals d.ID
                                                          join m in db.Movimientos on p.MovimientoPagoID equals m.ID
                                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                          join s in db.EstacionServicios on m.EstacionServicioID equals s.ID
                                                          where d.MovimientoID.Equals(M.ID)
                                                          select new
                                                          {
                                                              d.ID,
                                                              Estacion = s.Nombre,
                                                              mt.Abreviatura,
                                                              m.FechaRegistro,
                                                              Numero = (m.Numero.Equals(0) ? m.Referencia : m.Numero.ToString()),
                                                              Valor = m.Monto,
                                                              p.Monto
                                                          };


                       xtra.gridComprobante.DataSource = M.VistaComprobantes;
                       xtra.rpCeco.DataSource = db.CentroCostos.Select(s => new { s.ID, Display = s.Nombre });

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
