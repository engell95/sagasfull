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
using System.Data.SqlClient;

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormPagos : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPagos nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private DevExpress.XtraBars.Bar barItems;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        //private bool _FinalizarCheque;
        private DateTime vFecha;
        DevExpress.XtraBars.BarButtonItem btnMasivo = new DevExpress.XtraBars.BarButtonItem();
        DevExpress.XtraBars.BarButtonItem btnPrint = new DevExpress.XtraBars.BarButtonItem();
        
        #endregion

        #region <<< INICIO >>>

        public FormPagos()
        {
            try
            {
                InitializeComponent();
                this.btnAgregar.Caption = "Crear";
                this.btnAnular.Caption = "Anular";
                this.barImprimir.Caption = "Vista Previa";
                Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
                this.btnModificar.Visibility = BarItemVisibility.Never;
                this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaMovimiento);
                this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
                this.barTop.DockCol = 1;
                this.barTop.OptionsBar.UseWholeRow = false;
                barItems = new DevExpress.XtraBars.Bar(barManager);
                barItems.OptionsBar.AllowQuickCustomization = false;
                barItems.OptionsBar.DrawDragBorder = false;
                barItems.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
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

                    vFecha = Convert.ToDateTime(db.GetDateServer());

                    //_FinalizarCheque = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnImprimirCheque");

                    //if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnImprimirCheque"))
                    //{
                    //    DevExpress.XtraBars.BarButtonItem btnPrint = new DevExpress.XtraBars.BarButtonItem();
                    //    btnPrint.Caption = "Imprimir";
                    //    btnPrint.Glyph = Properties.Resources.Misc_Printer_Default_icon;
                    //    btnPrint.Id = 2;
                    //    btnPrint.Name = "btnPrint";
                    //    btnPrint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    //    btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
                    //    barItems.AddItem(btnPrint);
                    //}

                    DateTime fecha = new DateTime(vFecha.Year, vFecha.Month, 01);
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
                var query = from iv in dv.VistaMovimientos
                            where lista.Contains(iv.EstacionServicioID) && iv.MovimientoTipoID.Equals(82)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.EstacionNombre,
                                iv.EstacionServicioID,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                iv.Comentario,
                                iv.MonedaSimbolo,
                                iv.ProveedorNombre,
                                iv.Revisado,
                                //Beneficiario = (!iv.ProveedorID.Equals(0) ? iv.ProveedorCodigo + " | " + iv.ProveedorNombre : iv.EmpleadoCodigo + " | " + iv.EmpleadoNombres + " " + iv.EmpleadoApellidos),
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                iv.Monto,
                                iv.ConceptoContableNombre,
                                iv.EsMasivo,
                                iv.EsAnticipo,
                                iv.MovimientoTipoNombre,
                                iv.Finalizado,
                                iv.FechaFisico, 
                                iv.ComentarioActa
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
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero.ToString();

                Reportes.Tesoreria.Hojas.RptCoprobantePagoOperativo rep = new Reportes.Tesoreria.Hojas.RptCoprobantePagoOperativo();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = rep.CeEmpresa2.Text = Nombre;
                rep.CeRuc.Text = Ruc;
                rep.xrTcProveedor.Text = VM.First().ProveedorNombre;

                rep.xrLabelTitulo.Text = "Solicitud de Pago";
                rep.xrTableFoot.Visible = false;

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

                rep.CeLetras.Text = Parametros.General.enletras(VM.First().Monto.ToString());
                rep.DataSource = VM;

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

            /*
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero;

                Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica rep = new Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

               // rep.ceCantidad.Text = Parametros.General.enletras(VM.First().Monto.ToString());

                rep.DataSource = VM;

                //rep.xrSetDate.Text = vFecha.ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                rv.Show();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            */
        }

        private void ImprimirComprobanteAprobado(int ID, bool ToPrint)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(dbView.VistaMovimientos.First( f => f.ID.Equals(ID)).MovimientoReferenciaID)).ToList();
               //List<Entidad.VistaComprobante> obj = dbView.VistaComprobantes.Where(m => m.MovimientoID.Equals(VM.First().MovimientoReferenciaID)).ToList();
               // VM.First().VistaComprobantes.Clear();
               // VM.First().VistaComprobantes.AddRange(obj);
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                //var VM = obj.Where(o => o.ID.Equals(ID));
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero.ToString();

                Reportes.Tesoreria.Hojas.RptCoprobantePagoOperativo rep = new Reportes.Tesoreria.Hojas.RptCoprobantePagoOperativo();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = rep.CeEmpresa2.Text = Nombre;
                rep.CeRuc.Text = Ruc;
                rep.xrTcProveedor.Text = VM.First().PreveedorReferencia;

                //rep.xrLabelTitulo.Text = "Solicitud de Pago";
                //rep.xrTableFoot.Visible = false;

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

                rep.CeLetras.Text = Parametros.General.enletras(VM.First().Monto.ToString());
                rep.DataSource = VM;
                //rep.DetailReport.DataSource = dv.VistaMovimientos.Where(m => m.ID.Equals(VM.First().MovimientoReferenciaID));

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

                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                //Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                //var MA = db.Movimientos.Single(s => s.ID.Equals(ID));
                //var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(MA.MovimientoReferenciaID)).ToList();
                //Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                //rv.Text = VM.First().Abreviatura + " Aprobado " + VM.First().Numero.ToString();

                //Reportes.Contabilidad.Hojas.RptComprobanteCD rep = new Reportes.Contabilidad.Hojas.RptComprobanteCD();

                //string Nombre, Direccion, Telefono;
                //System.Drawing.Image picture_LogoEmpresa;
                //Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                //rep.PicLogo.Image = picture_LogoEmpresa;
                //rep.CeEmpresa.Text = Nombre;
                //rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + " Aprobado " + VM.First().Numero.ToString();
                //rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);


                ////else
                ////rep.Watermark.Text = (ToPrint ? "" : "RE-IMPRESION");
                ////Datos del Encabezado      
                //rep.DataSource = VM;


                //rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                //rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                //rv.Owner = this.Owner;
                //rv.MdiParent = this.MdiParent;
                //rep.RequestParameters = false;
                //rep.CreateDocument(true);

                //if (ToPrint)
                //{
                //    rep.Print();
                //    rv.Close();
                //}
                //else
                //    rv.Show();


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        protected override void Imprimir()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colFin)))
                    {
                        using (Forms.Dialogs.DialogPagoImpresion df = new Forms.Dialogs.DialogPagoImpresion())
                        {
                            df.Text = "¿Impresión?";

                            if (df.ShowDialog().Equals(DialogResult.OK))
                                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
                            else
                                ImprimirComprobanteAprobado(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
                        }
                    }
                    else
                        ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
                }
            }
           catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Anular el movimiento Masivo
        private void AnularCheque(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    #region <<< ANULACIÓN CHEQUE >>>

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = vFecha;
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 40;
                    M.ProveedorID = Manterior.ProveedorID;
                    M.EmpleadoID = Manterior.EmpleadoID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = vFecha;
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaFisico = Manterior.FechaFisico;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Credito = false;
                    M.FechaVencimiento = null;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                                "Se anuló el Cheque: " + Manterior.Numero.ToString(), this.Name);

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

                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.MovimientoID.Equals(ID)).FirstOrDefault();

                    if (D != null)
                    {

                        D.Pagos.ToList().ForEach(det =>
                        {
                            Entidad.Movimiento MD = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                            MD.Abono -= det.Monto;
                            MD.Pagado = false;
                            MD.MovimientoReferenciaID = 0;
                            if (det.PagoIVA)
                                MD.IVAPagado = false;

                            db.SubmitChanges();
                        });

                        D.Pagos.Clear();
                        db.Deudors.DeleteOnSubmit(D);
                        db.SubmitChanges();
                    }

                    db.Retenciones.DeleteAllOnSubmit(db.Retenciones.Where(o => o.MovimientoID.Equals(ID)));
                    db.SubmitChanges();
                    #endregion


                    #endregion

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
                    nf = new Forms.Dialogs.DialogPagos(Usuario, false,false);
                    nf.Text = "Crear Pago";
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

                //if (gvData.FocusedRowHandle >= 0)
                //{
                //    if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue("EsMasivo")))
                //    {
                //        if (nf == null)
                //        {
                //            if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                //                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Cheque no pertenece a la Estación de Servicio seleccionada, no se puede editar.", Parametros.MsgType.warning);
                //            else
                //            {
                //                if (!paginas.Contains(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))))
                //                {
                //                    if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                //                    {
                //                        nf = new Forms.Dialogs.DialogCheque(Usuario, true);
                //                        nf.Text = "Editar Cheque";
                //                        nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                //                        nf.Owner = this.Owner;
                //                        nf.MdiParent = this.MdiParent;
                //                        nf.MDI = this;
                //                        nf.Show();
                //                    }
                //                    else
                //                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                //                }
                //                else
                //                {
                //                    Parametros.General.DialogMsg("Debe de cerrar el detallte del Cheque Número " + Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + ", para poder ser editado.", Parametros.MsgType.warning);
                //                }
                //            }
                //        }
                //        else
                //            nf.Activate();
                //    }
                //    else
                //    {
                //        if (nfm == null)
                //        {
                //            if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                //                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Cheque no pertenece a la Sede seleccionada, no se puede editar.", Parametros.MsgType.warning);
                //            else
                //            {
                //                if (!paginas.Contains(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))))
                //                {
                //                    if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                //                    {
                //                        nfm = new Forms.Dialogs.DialogChequeMasivo(Usuario, true, false);
                //                        nfm.Text = "Editar Cheque Masivo";
                //                        nfm.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                //                        nfm.Owner = this.Owner;
                //                        nfm.MdiParent = this.MdiParent;
                //                        nfm.MDI = this;
                //                        nfm.Show();
                //                    }
                //                    else
                //                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                //                }
                //                else
                //                {
                //                    Parametros.General.DialogMsg("Debe de cerrar el detallte del Cheque Número " + Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + ", para poder ser editado.", Parametros.MsgType.warning);
                //                }
                //            }
                //        }
                //        else
                //            nfm.Activate();
                //    }
                //}
            }            
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(40))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colRevisado)))
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "La orden esta revisada y no puede anularse.", Parametros.MsgType.warning);
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                            {
                                AnularCheque(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                FillControl(true);
                            }
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
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                 
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = M.SiglasBanco + " " + M.Abreviatura + " " + M.Numero.ToString();
                    page.Tag = M.ID;

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {

                        Parametros.MyXtraGridCheque xtra = new Parametros.MyXtraGridCheque(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));

                        xtra.gridDetalle.DataSource = from p in db.Pagos
                                                      join d in db.Deudors on p.DeudorID equals d.ID
                                                      join m in db.Movimientos on p.MovimientoPagoID equals m.ID
                                                      join mp in db.Movimientos on p.MovimientoPagoID equals mp.ID
                                                      join es in db.EstacionServicios on mp.EstacionServicioID equals es.ID
                                                      join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                      where d.MovimientoID.Equals(M.ID)
                                                      select new
                                                      {
                                                          d.ID,
                                                          mt.Abreviatura,
                                                          Referencia = (m.Numero.Equals(0) ? m.Referencia : m.Numero.ToString()),
                                                          m.FechaRegistro,
                                                          Estacion = es.Nombre,
                                                          Valor = m.Monto,
                                                          p.Monto
                                                      };

                        xtra.gridComprobante.DataSource = M.VistaComprobantes;

                        var R = dv.VistaRetenciones.Where(r => r.MovimientoID.Equals(M.ID));

                        if (R.Count() > 0)
                        {
                            xtra.gridImp.DataSource = M.VistaRetenciones;
                            xtra.chkMostrar.Visible = true;
                        }
                  
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
        
        private void xtraTabControlMain_CloseButtonClick(object sender, EventArgs e)
        {
            this.paginas.Remove(Convert.ToInt32(xtraTabControlMain.SelectedTabPage.Tag));
            this.xtraTabControlMain.TabPages.Remove(xtraTabControlMain.SelectedTabPage);

            if (this.xtraTabControlMain.TabPages.Count > 0)
            {
                this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages[(this.xtraTabControlMain.TabPages.Count - 1)];
            }
        }
        
        //Evitar doble click para expandir o contraer grupo
        private void gvData_GroupRowCollapsing(object sender, RowAllowEventArgs e)
        {
            //e.Allow = false;
        }

        private void Aprobar(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    DateTime _fecha;
                    string _memo;

                    using (Forms.Dialogs.DialogAprobacionPago df = new Forms.Dialogs.DialogAprobacionPago())
                    {
                        df.Text = "Aplicación";
                        df.Fecha = DateTime.Now.Date;

                        if (df.ShowDialog().Equals(DialogResult.OK))
                        {
                            _fecha = df.Fecha;
                            _memo = df.Comentario;
                        }
                        else
                            return;
                    }

                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    #region <<< APROBACIÓN >>>

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.MovimientoTipoID = 84;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = _fecha;

                    M.UsuarioID = Parametros.General.UserID;
                    M.Numero = Manterior.Numero;
                    M.Referencia = Manterior.Referencia;
                    M.ProveedorID = Manterior.ProveedorReferenciaID;
                    M.Entregado = Manterior.Entregado;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.ConceptoContableID = Manterior.ConceptoContableID;
                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.Comentario = _memo;
                    M.ProveedorReferenciaID = Manterior.ProveedorID;

                    db.Movimientos.InsertOnSubmit(M);

                    //Cuenta Caja Operativa
                    int _ccop = db.EstacionServicios.Single(s => s.ID.Equals(M.EstacionServicioID)).CuentaPagoOperativoID;
                    int _concepto = db.ConceptoCajaOperativas.Single(s => s.ID.Equals(Manterior.ConceptoContableID)).CuentaContableID;

                    Entidad.ComprobanteContable CDanterior = Manterior.ComprobanteContables.FirstOrDefault(f => f.CuentaContableID.Equals(_ccop));

                    int l = 1;
                    Entidad.ComprobanteContable CD = new Entidad.ComprobanteContable();

                    CD.CuentaContableID = CDanterior.CuentaContableID;
                        CD.Monto = Math.Abs(CDanterior.Monto);
                        CD.TipoCambio = CDanterior.TipoCambio;
                        CD.MontoMonedaSecundaria = CDanterior.MontoMonedaSecundaria;
                        CD.Fecha = M.FechaRegistro;
                    CD.Descripcion = _memo;
                        CD.EstacionServicioID = CDanterior.EstacionServicioID;
                        CD.SubEstacionID = CDanterior.SubEstacionID;
                        CD.CentroCostoID = CDanterior.CentroCostoID;
                        CD.Linea = l;
                        l++;


                        M.ComprobanteContables.Add(CD);
                        db.SubmitChanges();

                    Entidad.ComprobanteContable CDn = new Entidad.ComprobanteContable();

                    CDn.CuentaContableID = _concepto;
                    CDn.Monto = -Math.Abs(CDanterior.Monto);
                    CDn.TipoCambio = CDanterior.TipoCambio;
                    CDn.MontoMonedaSecundaria = -Math.Abs(CDanterior.MontoMonedaSecundaria);
                    CDn.Fecha = M.FechaRegistro;
                    CDn.Descripcion = _memo;
                    CDn.EstacionServicioID = CDanterior.EstacionServicioID;
                    CDn.SubEstacionID = CDanterior.SubEstacionID;
                    CDn.CentroCostoID = CDanterior.CentroCostoID;
                    CDn.Linea = l;
                    l++;

                    M.ComprobanteContables.Add(CDn);
                    db.SubmitChanges();



                    //});

                    #endregion
                    Manterior.Finalizado = true;
                    Manterior.MovimientoReferenciaID = M.ID;
                    Manterior.FechaFisico = _fecha;
                    Manterior.ComentarioActa = _memo;

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                               "Se Aprobó la Orden: " + Manterior.Numero.ToString(), this.Name);

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

        private void AnularPago(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    Entidad.Movimiento M = db.Movimientos.Single(m => m.ID.Equals(Manterior.MovimientoReferenciaID));

                    M.ComprobanteContables.Clear();
                    db.Movimientos.DeleteOnSubmit(M);

                    Manterior.Finalizado = false;
                    Manterior.MovimientoReferenciaID = 0;
                    Manterior.FechaFisico = null;
                    Manterior.ComentarioActa = null;

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                               "Se desaprobó la Orden: " + Manterior.Numero.ToString(), this.Name);

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

        private void RevisarPago(int ID, bool valor)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));


                    Manterior.Revisado = valor;

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                               "Se " + (valor ? " revisó " : "quitó la revisión de") + " la Orden: " + Manterior.Numero.ToString(), this.Name);

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

        #endregion

        #region <<< IMPRESION DE CHEUQES >>>

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Tesoreria.Forms.FormRptImpresionCheques fr = new Tesoreria.Forms.FormRptImpresionCheques();
                fr.Owner = this;
                fr.MdiParent = this.MdiParent;
                fr.Show();

               /*
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                {
                    conn.Open();

                    // Creates a SQL command
                    using (var command = new SqlCommand("SELECT * FROM [<Rpt_ImpresionCheques] where MovimientoID = @ID", conn))
                    {
                        //parametro del ID del movimiento Cheque
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        // Loads the query results into the table
                        dt.Load(command.ExecuteReader());
                    }

                }

                var obj = db.CuentaBancarias.FirstOrDefault(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colCuenta))));

                if (obj != null)
                {
                    if (obj.Archivo != null)
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                        DevExpress.XtraReports.UI.XtraReport rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                        rep.DataSource = dt;
                        rep.RequestParameters = false;
                        rep.CreateDocument();
                        rep.Print();

                        Entidad.Movimiento MP = db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        MP.Finalizado = true;
                        db.SubmitChanges();
                    }
                    else
                    { Parametros.General.DialogMsg("La cuenta bancaria no tiene formato de impresión", Parametros.MsgType.warning); }
                }
                else
                { Parametros.General.DialogMsg("La cuenta bancaria no existe", Parametros.MsgType.warning); }
      */
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        #endregion

        private void ChkFin_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkAplicarPagoOperativo"))
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colRevisado)))
                    {
                        if (gvData.FocusedRowHandle >= 0)
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "El pago está anulado.", Parametros.MsgType.warning);
                            else
                            {
                                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colFin)))
                                {
                                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                    else
                                    {
                                        if (Parametros.General.DialogMsg("¿Está seguro de desaprobar el Pago?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                        {
                                            AnularPago(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                            FillControl(true);
                                            gvData.FocusedColumn = colNumero;
                                        }
                                    }
                                }
                                else
                                {
                                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                    else
                                    {
                                        if (Parametros.General.DialogMsg("¿Está seguro de aprobar el Pago?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                        {
                                            Aprobar(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                            FillControl(true);
                                            gvData.FocusedColumn = colNumero;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                        Parametros.General.DialogMsg("La orden de pago no esta revisada.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void chkRev_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkRevisarPagoOperativo"))
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)))
                            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "El pago está anulado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colRevisado)))
                            {
                                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colFin)))
                                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "La orden ya esta APLICADA!.", Parametros.MsgType.warning);
                                else
                                {
                                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                    else
                                    {
                                        if (Parametros.General.DialogMsg("¿Está seguro de quitar el revisado del Pago?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                        {
                                            RevisarPago(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
                                            FillControl(true);
                                            gvData.FocusedColumn = colNumero;
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                else
                                {
                                    if (Parametros.General.DialogMsg("¿Está seguro de dar por revisado del Pago?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                    {
                                        RevisarPago(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), true);
                                        FillControl(true);
                                        gvData.FocusedColumn = colNumero;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
    }
}
