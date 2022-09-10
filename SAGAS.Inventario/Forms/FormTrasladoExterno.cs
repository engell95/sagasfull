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
    public partial class FormTrasladoExterno : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogTraslado nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _ClaseCombustible;
        
        #endregion

        #region <<< INICIO >>>

        public FormTrasladoExterno()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Trasladar";
            this.btnModificar.Caption = "Recibir";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
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
                _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                    
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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(68) || v.MovimientoTipoID.Equals(69) || v.MovimientoTipoID.Equals(70))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Referencia,
                                iv.Numero,
                                iv.EstacionServicioID,
                                iv.SubEstacionID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Observacion = iv.Comentario,
                                iv.MovimientoTipoNombre,
                                iv.ConceptoContableNombre,
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                iv.Monto,
                                iv.Finalizado,
                                Enlace = (iv.MovimientoReferenciaID > 0 ? iv.MovimientoReferenciaID : iv.ID)
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

                if (VM.First().MovimientoTipoID.Equals(69))
                {
                    if (VM.First().VistaKardexes.Count <= 0)
                    {
                        Parametros.General.DialogMsg("El " + VM.First().MovimientoTipoNombre + " no ha sido aceptado por la Estación de Destino.", Parametros.MsgType.warning);
                        return;
                    }
                }

                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();

                if (VM.First().MovimientoTipoID.Equals(68))
                    rv.Text = "Traslado Externo " + VM.First().Numero.ToString();
                else if (VM.First().MovimientoTipoID.Equals(69))
                    rv.Text = "Recepción Externa " + VM.First().Numero.ToString();

                Reportes.Inventario.Hojas.RptComprobanteTraslado rep = new Reportes.Inventario.Hojas.RptComprobanteTraslado();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                if (VM.First().MovimientoTipoID.Equals(68))
                {
                    var Rec = dbView.VistaMovimientos.Where(o => o.MovimientoReferenciaID.Equals(VM.First().ID)).Select(s => new { s.EstacionNombre, s.SubEstacionNombre }).FirstOrDefault();

                    if (Rec != null)
                        rep.xrCeEstacionMov.Text = Rec.EstacionNombre + (String.IsNullOrEmpty(Rec.SubEstacionNombre) ? "" : Rec.SubEstacionNombre);
                }
                else if (VM.First().MovimientoTipoID.Equals(69))
                {
                    var Tras = dbView.VistaMovimientos.Where(o => o.ID.Equals(VM.First().MovimientoReferenciaID)).Select(s => new { s.EstacionNombre, s.SubEstacionNombre }).FirstOrDefault();

                    if (Tras != null)
                        rep.xrCeEstacionMov.Text = Tras.EstacionNombre + (String.IsNullOrEmpty(Tras.SubEstacionNombre) ? "" : Tras.SubEstacionNombre);
                }

                rep.DataSource = VM;

                if (VM.First().VistaComprobantes.Count <= 0)
                    rep.DetailReportCD.Visible = false;

                if (VM.First().UsuarioID.Equals(VM.First().AdmonID))
                    rep.xrCellRevisado.Text = VM.First().ContaNombre;
                else
                    rep.xrCellRevisado.Text = VM.First().AdmonNombre;

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

                    //INVENTARIO BLOQUEADO DIFERENTECOMBUSTIBLE
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

                    //BLOQUEO PARA COMBUSTIBLE
                    List<int> PAreas = (from p in db.Productos
                                        join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                        where Manterior.Kardexes.Select(s => s.ProductoID).Contains(p.ID) && pc.ID.Equals(_ClaseCombustible)
                                        group pc by pc.AreaID into gr
                                        select gr
                                       ).Select(s => s.Key).ToList();

                    if (PAreas.Count > 0)
                    {
                        if (!Parametros.General.ValidateKardexMovemente(Manterior.FechaRegistro, db, Manterior.EstacionServicioID, Manterior.SubEstacionID, 24, 0))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Manterior.FechaRegistro.ToShortDateString(), Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }

                    if (Tipo.Equals(68))
                    {
                        #region <<< ANULACION_TRASLADO >>>

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = 70;
                        M.ProveedorID = Manterior.ProveedorID;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.ConceptoContableID = Manterior.ConceptoContableID;
                        M.Comentario = Manterior.Comentario;                        

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el traslado de Inventario: " + Manterior.Numero.ToString("000000000"), this.Name);

                        foreach (var dk in Manterior.Kardexes)
                        {
                            //decimal CostoMov = LineaDetalle.Cost;
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.ProductoID = dk.ProductoID;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.MovimientoID = M.ID;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;

                            //Anulacion de salida
                            //*****REFLEJA ENTRADA******//
                            KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                            KX.CantidadEntrada = dk.CantidadSalida;
                            KX.CantidadInicial = dk.CantidadFinal;
                            KX.CantidadFinal = dk.CantidadInicial;
                            KX.CostoInicial = dk.CostoFinal;
                            KX.CostoFinal = dk.CostoFinal;
                            KX.CostoEntrada = dk.CostoSalida;
                            KX.CostoTotal = dk.CostoTotal;
                            KX.EsProducto = dk.EsProducto;

                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();
                        }
                        
                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        db.Movimientos.DeleteOnSubmit(db.Movimientos.Single(o => o.MovimientoReferenciaID.Equals(Manterior.ID)));
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
                            db.SubmitChanges();
                        });
                            #endregion                   
                    }
                    else if (Tipo.Equals(69))
                    {
                        #region <<< ANULACION_RECIBIDO >>>

                        Manterior.Finalizado = false;

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el recibido de Inventario: " + Manterior.Numero.ToString("000000000"), this.Name);

                        Manterior.Kardexes.Clear();
                        Manterior.ComprobanteContables.Clear();

                        #endregion   
                    }

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
                this.Add();
            else
                this.Activate();

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
                    nf = new Forms.Dialogs.DialogTraslado(Usuario, false);
                    nf.Text = "Crear Traslado Externo";
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
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(69))
                        {
                            if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                            { Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Movimiento no pertenece a la Sub Estación de Servicio seleccionada, no se puede Recibir.", Parametros.MsgType.warning); }
                            else
                            {
                                if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                                { Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Movimiento no pertenece a la Sub Estación de Servicio seleccionada, no se puede Recibir.", Parametros.MsgType.warning); }
                                else
                                {
                                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                                        Parametros.General.DialogMsg("El Recibo Externo de Mercaderia " + Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + " ya esta Finalizado.", Parametros.MsgType.warning);
                                    else
                                    {
                                        nf = new Forms.Dialogs.DialogTraslado(Usuario, true);
                                        nf.Text = "Editar Levantamiento de Inventario Físico";
                                        nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colEnlace)));
                                        nf.Owner = this.Owner;
                                        nf.MdiParent = this.MdiParent;
                                        nf.MDI = this;
                                        nf.Show();
                                    }
                                }
                            }
                        }
                        else
                            Parametros.General.DialogMsg("El Movimiento seleccionado no corresponde a Recibo Externo de Mercaderia.", Parametros.MsgType.warning);
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
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(70) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(71)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                        { Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Movimiento no pertenece a la Sub Estación de Servicio seleccionada, no se puede Anular.", Parametros.MsgType.warning); }
                        else
                        {
                            if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("SubEstacionID")).Equals(Parametros.General.SubEstacionID))
                            { Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Movimiento no pertenece a la Sub Estación de Servicio seleccionada, no se puede Anular.", Parametros.MsgType.warning); }
                            else
                            {
                                if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                else
                                {
                                    if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(68))
                                    {
                                        if (db.Movimientos.Single(o => o.MovimientoReferenciaID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).Finalizado)
                                            Parametros.General.DialogMsg("El Movimiento de Recibido para este traslado ya esta finalizado, no puede anular el traslado.", Parametros.MsgType.warning);
                                        else
                                        {
                                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                                            {
                                                AnularMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
                                                FillControl(true);
                                            }
                                        }
                                    }
                                    else if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(69))
                                    {
                                        if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colFinalizado)))
                                            Parametros.General.DialogMsg("El Movimiento de Recibido no esta finalizado, no puede anular el recibido solo el traslado.", Parametros.MsgType.warning);
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = M.Abreviatura + " " + M.Numero.ToString();

                    page.Tag = M.ID;

                    Parametros.MyXtraGridMovimientoInventario xtra = new Parametros.MyXtraGridMovimientoInventario();
                    if (M.MovimientoTipoID.Equals(69))
                    {
                        if (M.Finalizado)
                        {
                            xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenEntradaNombre, Cantidad = s.CantidadEntrada, Costo = s.CostoEntrada, Total = Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero) }).ToList();
                            xtra.gridComprobante.DataSource = M.VistaComprobantes;
                        }
                        else
                        {
                            Parametros.General.DialogMsg("El Movimiento seleccionado no ha esta finalizado.", Parametros.MsgType.warning);
                            return;
                        }
                    }
                    else if (M.MovimientoTipoID.Equals(68))
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenSalidaNombre, Cantidad = s.CantidadSalida, Costo = s.CostoSalida, Total = Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero) }).ToList();
                        xtra.gridComprobante.DataSource = M.VistaComprobantes;
                    }


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
