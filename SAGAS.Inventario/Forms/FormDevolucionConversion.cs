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
    public partial class FormDevolucionConversion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogDevolucionConversion nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        
        #endregion

        #region <<< INICIO >>>

        public FormDevolucionConversion()
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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(12) || v.MovimientoTipoID.Equals(13))// || v.MovimientoTipoID.Equals(80))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Monto,
                                iv.Numero,
                                iv.Referencia,
                                iv.Anulado,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.PreveedorReferencia,
                                iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Observacion = iv.Comentario,
                                iv.MovimientoTipoNombre,
                                iv.ConceptoContableNombre,
                                iv.EstacionServicioID,
                                iv.NumeroReferencia,
                                iv.MovimientoTipoID,
                                iv.MovimientoReferenciaID
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

        private void ImprimirComprobante(int ID, bool ToPrint)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();

                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Movimiento de Inventario " + VM.First().Numero;

                Reportes.Inventario.Hojas.RptDevolucionConversion rep = new Reportes.Inventario.Hojas.RptDevolucionConversion();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.DataSource = VM;

                if (VM.First().VistaComprobantes.Count() <= 0)
                    rep.DetailReportCD.Visible = false;

                if (VM.First().MovimientoTipoID.Equals(12))
                {
                    rep.DetailReportConv.Visible = false;
                    rep.DetailReportCD.Visible = true;
                }
                else if (VM.First().MovimientoTipoID.Equals(13) || VM.First().MovimientoTipoID.Equals(80))
                {
                    if (VM.First().MovimientoTipoID.Equals(13))
                    {
                        var vista = dbView.VistaMovimientos.Where(v => v.ID.Equals(VM.First().MovimientoReferenciaID));

                        if (vista.Count() > 0)
                        {
                            rep.DetailReportConv.Visible = true;
                            rep.DetailReportCD.Visible = false;
                            rep.DetailReportDev.DataSource = vista;
                            rep.xrTableCell17.DataBindings.Add("Text", null, "VistaKardexes.AlmacenSalidaNombre");
                            rep.xrTableCell18.DataBindings.Add("Text", null, "VistaKardexes.CantidadSalida");
                            rep.xrTableCell19.DataBindings.Add("Text", null, "VistaKardexes.CostoSalida");
                            rep.xrCellValorSubtotal.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal");
                            rep.xrTableCell4.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal");
                            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
                            xrSummary1.FormatString = "{0:n2}";
                            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
                            rep.xrTableCell4.Summary = xrSummary1;
                        }
                    }
                    else if (VM.First().MovimientoTipoID.Equals(80))
                    {
                        var vista = dbView.VistaMovimientos.Where(v => v.ID.Equals(VM.First().MovimientoReferenciaID));

                        if (vista.Count() > 0)
                        {
                            rep.DetailReportConv.Visible = true;
                            rep.DetailReportCD.Visible = false;
                            rep.DetailReportConv.DataSource = vista;
                        }
                    }
                }

                //rep.CeTitulo.Text = (VM.First().MovimientoTipoNombre);

                

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
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
        }

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, int ID, bool Refresh, bool Next)
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
                this.Add();
            else
                this.Activate();

            //if (!ID.Equals(0))
            //ImprimirComprobante(ID, true);
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
                    nf = new Forms.Dialogs.DialogDevolucionConversion(Usuario);
                    nf.Text = "Crear Dev / Conv";
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(4))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoReferenciaID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
                            FillControl(true);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Anula los movimientos de Devolución y Conversion (In & Out)
        /// </summary>
        /// <param name="ID">Id del movimiento a anular (Entrada si es conversion)</param>
        /// <param name="IDC">Id del movimiento a anular(Si el ID del movimiento corresponde a una entrada, este es el el movimiento generado en la salida)</param>
        /// <param name="Tipo">Define el tipo de movimiento del movimiento (Devolución = 12 & Conversión = 13)</param>
        private void AnularMovimiento(int ID, int IDC, int Tipo)
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

                    if (Tipo.Equals(12))
                    {
                        if (db.Movimientos.Single(m => m.ID.Equals(Manterior.MovimientoReferenciaID)).Pagado)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("La Factura relacionada a esta devolución " + Manterior.Referencia + ", ya esta pagada.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }

                    List<int> Areas = (from p in db.Productos
                                    join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                    where Manterior.Kardexes.Select(s => s.ProductoID).Contains(p.ID)
                                    group pc by pc.AreaID into gr
                                    select gr
                                                ).Select(s => s.Key).ToList();

                    bool Bloqueo = false;

                    foreach (int obj in Areas)
                    {
                        if (!Parametros.General.ValidateKardexMovemente(Manterior.FechaRegistro, db, Manterior.EstacionServicioID, Manterior.SubEstacionID, 9, obj))
                        {
                            Bloqueo = true;
                            break;
                        }
                    }

                    if (Bloqueo)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();                         
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Manterior.FechaRegistro.ToShortDateString(), Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    if (Tipo.Equals(12))
                    {
                        #region <<< ANULACION_DEVOLUCION >>>

                        #region <<< MOVIMIENTO >>>

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = (63);
                        M.ProveedorID = Manterior.ProveedorID;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.ProveedorID = Manterior.ProveedorID;
                        M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.Comentario = Manterior.Comentario;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló la Devolución: " + Manterior.Numero.ToString("000000000"), this.Name);
                        #endregion
                        
                        #region
                        foreach (var dk in Manterior.Kardexes)
                        {
                            //------- ESTABLECER CANTIDAD FINAL ---------//  
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.ProductoID = dk.ProductoID;
                            KX.EsProducto = dk.EsProducto;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.MovimientoID = M.ID;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;
                            KX.CantidadInicial = dk.CantidadFinal;
                            KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                            KX.CantidadEntrada = dk.CantidadSalida;
                            KX.CostoEntrada = dk.CostoSalida;
                            KX.CostoFinal = dk.CostoFinal;
                            KX.CostoTotal = dk.CostoTotal;
                            KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;

                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();
                        }
                        #endregion

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        #region <<< COMPROBANTE >>>

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


                        db.SubmitChanges();
                        trans.Commit();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        #endregion
                    }
                    else if (Tipo.Equals(13))
                    {
                        #region <<< ANULACION_CONVERSION >>>

                        #region <<< MOVIMIENTO (ENTRADA)>>>

                        Entidad.Movimiento M = new Entidad.Movimiento();
                        
                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = (81);
                        M.ProveedorID = Manterior.ProveedorID;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.ProveedorID = Manterior.ProveedorID;
                        M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.Comentario = Manterior.Comentario;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló la Conversion (Entrada): " + Manterior.Numero.ToString("000000000"), this.Name);

                        #endregion

                        #region <<< KARDEX (ENTRADA) >>>

                        foreach (var dk in Manterior.Kardexes.Where(o => o.AlmacenSalidaID > 0))
                        {
                            //------- ESTABLECER CANTIDAD FINAL ---------//  
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.ProductoID = dk.ProductoID;
                            KX.EsProducto = dk.EsProducto;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.MovimientoID = M.ID;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;
                            KX.CantidadInicial = dk.CantidadFinal;
                            KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                            KX.CantidadEntrada = dk.CantidadSalida;
                            KX.CostoEntrada = dk.CostoSalida;
                            KX.CostoFinal = dk.CostoSalida;
                            KX.CostoTotal = dk.CostoTotal;
                            KX.CantidadFinal = KX.CantidadInicial - KX.CantidadEntrada;

                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();
                        }

                        #endregion

                        #region <<< KARDEX (SALIDA) >>>

                        foreach (var dk in Manterior.Kardexes.Where(o => o.AlmacenEntradaID > 0))
                        {
                            //------- ESTABLECER CANTIDAD FINAL ---------//  
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.ProductoID = dk.ProductoID;
                            KX.EsProducto = dk.EsProducto;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.MovimientoID = M.ID;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;
                            KX.CantidadInicial = dk.CantidadFinal;
                            KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                            KX.CantidadSalida = dk.CantidadEntrada;
                            KX.CostoSalida = dk.CostoEntrada;
                            KX.CostoFinal = dk.CostoFinal;
                            KX.CostoTotal = dk.CostoTotal;
                            KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;

                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();
                        }

                        #endregion

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;

                        db.SubmitChanges();

                        #region <<< COMPROBANTE >>>

                        //int l = Manterior.ComprobanteContables.Count;
                        //Manterior.ComprobanteContables.OrderBy(o => o.Linea).ToList().ForEach(linea =>
                        //{
                        //    Entidad.ComprobanteContable CD = new Entidad.ComprobanteContable();

                        //    CD.CuentaContableID = linea.CuentaContableID;
                        //    CD.Monto = linea.Monto * (-1);
                        //    CD.TipoCambio = linea.TipoCambio;
                        //    CD.MontoMonedaSecundaria = linea.MontoMonedaSecundaria * (-1);
                        //    CD.Fecha = linea.Fecha;
                        //    CD.Descripcion = linea.Descripcion;
                        //    CD.EstacionServicioID = linea.EstacionServicioID;
                        //    CD.SubEstacionID = linea.SubEstacionID;
                        //    CD.CentroCostoID = linea.CentroCostoID;
                        //    CD.Linea = l;
                        //    l--;

                        //    M.ComprobanteContables.Add(CD);
                        //    db.SubmitChanges();
                        //});

                        #endregion

                        db.SubmitChanges();
                        trans.Commit();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        
                        #endregion
                    }
                     
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

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    //var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    //DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    //page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                    //page.Tag = M.ID;

                    //Parametros.MyXtraGridMovimientoInventario xtra = new Parametros.MyXtraGridMovimientoInventario();
                    //if (M.MovimientoTipoID.Equals(1) || M.MovimientoTipoID.Equals(11))
                    //{
                    //    xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenEntradaNombre, Cantidad = s.CantidadEntrada, Costo = s.CostoEntrada, Total = Decimal.Round(s.CostoEntrada * s.CantidadEntrada, 2, MidpointRounding.AwayFromZero) }).ToList();
                    //    xtra.gridComprobante.DataSource = M.VistaComprobantes;
                    //}
                    //else if (M.MovimientoTipoID.Equals(2) || M.MovimientoTipoID.Equals(10))
                    //{
                    //    xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenSalidaNombre, Cantidad = s.CantidadSalida, Costo = s.CostoSalida, Total = Decimal.Round(s.CostoSalida * s.CantidadSalida, 2, MidpointRounding.AwayFromZero) }).ToList();
                    //    xtra.gridComprobante.DataSource = M.VistaComprobantes;
                    //}


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
