using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraReports.UI;

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormComprasPedido : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogComprasPedido nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private int _ClaseCombustible;
        private List<int> lista;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();

        #endregion

        #region <<< INICIO >>>

        public FormComprasPedido()
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
                    _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();

                    lkMes.DataSource = listadoMes.GetListMeses();

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaRegistro") >= fecha).ToString();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                var query = from v in dv.VistaMovimientos
                                               where lista.Contains(v.EstacionServicioID) && (v.MovimientoTipoID.Equals(3) || v.MovimientoTipoID.Equals(4))
                                               select new
                                               {
                                                   v.ID,
                                                   Proveedor = v.ProveedorNombre,
                                                   FechaRegistro = v.FechaContabilizacion,
                                                   Year = v.FechaContabilizacion.Year,
                                                   Mes = v.FechaContabilizacion.Month,
                                                   Monto = (v.Credito ? (v.MonedaID.Equals(MonedaID) ? v.Monto : v.MontoMonedaSecundaria) : v.SubTotal),
                                                   v.Referencia,
                                                   v.Anulado,
                                                   v.EstacionServicioID,
                                                   Moneda = v.MonedaSimbolo + " | " + v.MonedaNombre,
                                                   ES = v.EstacionNombre,
                                                   SUS = v.SubEstacionNombre,
                                                   v.MovimientoTipoID,
                                                   NombreMovimiento = v.MovimientoTipoNombre,
                                                   v.Comentario,
                                                   v.Credito,
                                                   v.Pagado

                                               };
                e.QueryableSource = query;
                e.Tag = dv;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
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
                rv.Text = "Comprobante de compra " + VM.First().Referencia;

                Reportes.Contabilidad.Hojas.RptComprobanteCompra rep = new Reportes.Contabilidad.Hojas.RptComprobanteCompra();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.CeTitulo.Text = (VM.First().MovimientoTipoID.Equals(3) ? "Comprobante de Compra" : "Anulación de Compra");

                rep.DataSource = VM;

                if (VM.First().VistaKardexes.Sum(s => s.MontoISC).Equals(0))
                {
                    rep.xrCellTituloISC.Dispose();
                    rep.xrCellTituloSubtotal.Dispose();
                    rep.xrCellValorISC.Dispose();
                    rep.xrCellValorSubtotal.Dispose();
                    rep.xrCellSumISC.Dispose();
                    rep.xrCellSumSubtotal.Dispose();
                }

                if (VM.First().VistaKardexes.Count(o => !String.IsNullOrEmpty(o.DetalleCombustible)).Equals(0))
                {
                    rep.xrTableRowDetalle.Dispose();
                    rep.xrTableRow7.HeightF = 25f;
                    rep.Detail1.HeightF = 25f;
                }

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                
                if (ToPrint)
                {
                    rep.CreateDocument();
                    rep.Print();
                    rv.Close();

                    if (!VM.First().Credito)
                        ImprimirCajaChica(VM.First().ID, true);
                }
                else
                {
                    rep.CreateDocument(true);
                    rv.Show();
                }
            
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void ImprimirCajaChica(int ID, bool ToPrint)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Provisión " + VM.First().Referencia;

                Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica rep = new Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeRuc.Text = Ruc;

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, bool Refresh, int ID, bool ToPrint)
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

            if (ToPrint)
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
                    nf = new Forms.Dialogs.DialogComprasPedido(Usuario, false);
                    nf.Text = "Crear Nueva Compra";
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

        //Anular la compra
        private void AnularCompra(int ID)
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

                    if (Manterior.Pagado)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + Manterior.Referencia + ", ya esta pagado.", Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Manterior.ID)) > 0)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + (Manterior.Numero.Equals(0) ? Manterior.Referencia : Manterior.Numero.ToString()) + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 4;
                    M.ProveedorID = Manterior.ProveedorID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaFisico = Manterior.FechaFisico;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Credito = false;
                    M.FechaVencimiento = null;
                    M.Referencia = "ANULADO   " + Manterior.Referencia;
                    M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                "Se anuló la Compra: " + M.Referencia, this.Name);

                    foreach (var dk in Manterior.Kardexes)
                    {
                        //decimal CostoMov = LineaDetalle.Cost;
                        Entidad.Kardex KX = new Entidad.Kardex();
                        KX.ProductoID = dk.ProductoID;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;
                        KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                        KX.CantidadSalida = dk.CantidadEntrada;
                        KX.CantidadInicial = dk.CantidadFinal;
                        KX.CantidadFinal = dk.CantidadInicial;
                        KX.CostoInicial = dk.CostoFinal;
                        KX.CostoFinal = dk.CostoInicial;
                        KX.CostoSalida = dk.CostoEntrada;
                        KX.EstacionServicioID = dk.EstacionServicioID;
                        KX.SubEstacionID = dk.SubEstacionID;
                        KX.MovimientoID = M.ID;
                        KX.CostoTotal = dk.CostoTotal;
                        KX.ImpuestoTotal = dk.ImpuestoTotal;
                        KX.MontoISC = dk.MontoISC;
                        KX.SubTotal = dk.SubTotal;
                        KX.DetalleCombustible = dk.DetalleCombustible;
                        KX.EsProducto = dk.EsProducto;

                        decimal Valor = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha, false) + Parametros.General.SaldoKardexPost(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha) - KX.CantidadSalida;
                        if (Valor < 0)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("El producto : " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " no puede quedar en negativo.", Parametros.MsgType.warning);
                            return;
                        }

                        db.Kardexes.InsertOnSubmit(KX);

                        #region <<< ACTUALIZAR COSTOS >>>
                        /*
                        var ActualizarCosto = (from k in db.Kardexes
                                               join m in db.Movimientos on k.Movimiento equals m
                                               join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                               where k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && k.ProductoID.Equals(KX.ProductoID) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                                               && k.Fecha.Date >= KX.Fecha.Date
                                               select new { k.ID, k.Fecha, mt.Entrada }).OrderBy(o => o.Fecha).ThenBy(t => !t.Entrada).ToList();

                        var KCIList = (from k in db.Kardexes 
                                       join m in db.Movimientos on k.MovimientoID equals m.ID
                                       where k.ProductoID.Equals(KX.ProductoID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                     && k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && !k.CostoEntrada.Equals(0)
                                     && k.Fecha.Date <= KX.Fecha.Date 
                                     && !k.ID.Equals(dk.ID)
                                       select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                        decimal vCosto = 0m;
                        if (KCIList.Count() > 0)
                            vCosto = Decimal.Round(KCIList.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                        
                        foreach (var k in ActualizarCosto)
                        {
                            if (!k.ID.Equals(dk.ID))
                            {
                                Entidad.Kardex NK = db.Kardexes.Single(o => o.ID.Equals(k.ID));

                                if (NK.Movimiento.MovimientoTipoID.Equals(1) || NK.Movimiento.MovimientoTipoID.Equals(3))
                                {
                                    //var NKCI = (from kd in db.Kardexes
                                    //            join mt in db.Movimientos on kd.MovimientoID equals mt.ID
                                    //            where kd.ProductoID.Equals(KX.ProductoID) && mt.Anulado.Equals(false) && (mt.MovimientoTipoID.Equals(3) || mt.MovimientoTipoID.Equals(1))
                                    //              && kd.EstacionServicioID.Equals(KX.EstacionServicioID) && kd.SubEstacionID.Equals(KX.SubEstacionID) && !kd.CostoEntrada.Equals(0)
                                    //              && kd.Fecha.Date <= KX.Fecha && (!kd.ID.Equals(dk.ID) && !kd.ID.Equals(NK.ID))
                                    //            select new { kd.ID, kd.Fecha, kd.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                                    //decimal nvCosto = 0m;
                                    //if (NKCI.Count() > 0)
                                    //    nvCosto = Decimal.Round(KCI.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                                    ////Costo Inicial es vCosto
                                    //decimal vCantidadInicial = 0m;
                                    ////Cantidad Inicial para el calculo
                                    //vCantidadInicial = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, 0, KX.Fecha, true) - KX.CantidadSalida;

                                    //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * nvCosto), 4, MidpointRounding.AwayFromZero);
                                    //vCosto = Decimal.Round(Convert.ToDecimal((vCostosInicialesRecalculo + NK.CostoTotal) / vCantidadInicial), 4, MidpointRounding.AwayFromZero);

                                    //if (NK.Fecha.Date >= KX.Fecha.Date)
                                    //    NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                    ////////////////////////////////////////////////////////////////////////////////////

                                    //Costo Inicial es vCosto
                                    decimal vCantidades = 0m;
                                    //Cantidad Inicial para el calculo
                                    vCantidades = Parametros.General.SaldoKardexAnulacion(db, dk.ID, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, 0, NK.Fecha, true);
                                    //Costo Total Inicial para el calculo
                                    //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);

                                    decimal vSaldoAct = 0m;
                                    Parametros.General.SaldoCostoTotalKardex(db, dk.ID, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, NK.Fecha, out vSaldoAct);

                                    vCosto = Decimal.Round(Convert.ToDecimal(vSaldoAct / vCantidades), 4, MidpointRounding.AwayFromZero);

                                    if (NK.Fecha.Date >= KX.Fecha.Date)
                                        NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);


                                }
                                else
                                {

                                    NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                    if (!NK.CostoEntrada.Equals(0))
                                    {
                                        NK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                        NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                    }

                                    if (!NK.CostoSalida.Equals(0))
                                    {
                                        NK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                        NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                    }

                                    db.SubmitChanges();

                                    var Anterior = db.Kardexes.Where(o => o.MovimientoID.Equals(NK.MovimientoID) && !o.ID.Equals(NK.ID)).Select(s => new { s.ID, s.CostoFinal });

                                    decimal vSuma = 0m;

                                    if (Anterior.Count() > 0)
                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoFinal), 2, MidpointRounding.AwayFromZero);

                                    //Entidad.Movimiento MK = db.Movimientos.Single(o => o.ID.Equals(NK.MovimientoID));

                                    var area = from a in db.Areas
                                               join pc in db.ProductoClases on a.ID equals pc.AreaID
                                               join p in db.Productos on pc.ID equals p.ProductoClaseID
                                               where p.ID.Equals(NK.ProductoID)
                                               select a;

                                    if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                    {
                                        Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                        if (NCC != null)
                                        {
                                            if (NCC.Monto < 0)
                                                NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                            else
                                                NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                            db.SubmitChanges();
                                        }
                                    }
                                    else
                                    {
                                        var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaCostoID;

                                        Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                        if (NCC != null)
                                        {
                                            if (NCC.Monto < 0)
                                                NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                            else
                                                NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                            db.SubmitChanges();
                                        }
                                    }

                                    if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                    {
                                        Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                        if (NCC != null)
                                        {
                                            if (NCC.Monto < 0)
                                                NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                            else
                                                NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                            db.SubmitChanges();
                                        }
                                    }
                                    else
                                    {
                                        var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaInventarioID;

                                        Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                        if (NCC != null)
                                        {
                                            if (NCC.Monto < 0)
                                                NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                            else
                                                NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                            db.SubmitChanges();
                                        }
                                    }

                                }
                            }
                        }
                         * */
                        #endregion

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                        if (!M.ProveedorID.Equals(Parametros.Config.ProveedorCombustibleID()))
                        {
                            var AL = (from al in db.AlmacenProductos
                                      where al.ProductoID.Equals(KX.ProductoID)
                                        && al.AlmacenID.Equals(KX.AlmacenSalidaID)
                                      select al).ToList();

                            if (AL.Count() > 0)
                            {
                                Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(KX.AlmacenSalidaID));
                                AP.Cantidad = KX.CantidadFinal;
                                decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                if (KX.ImpuestoTotal > 0)
                                    precio += Decimal.Round((KX.CostoFinal * 0.15m), 6, MidpointRounding.AwayFromZero);
                                //-- INSERTAR REGISTRO 
                                ///////////////////**********************************************************
                                AP.Costo = KX.CostoFinal;
                                AP.PrecioSugerido = precio;
                            }
                        }
                        else if (M.ProveedorID.Equals(Parametros.Config.ProveedorCombustibleID()))
                        {
                            var TP = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(KX.ProductoID)
                                        && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                                      select tp).ToList();

                            if (TP.Count() > 0)
                            {
                                Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
                                T.Cantidad = TP.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - dk.CantidadSalida;
                                T.Costo = KX.CostoFinal;
                            }
                        }
                        //------------------------------------------------------------------------//

                        #endregion

                        //para que actualice los datos del registro
                        db.SubmitChanges();

                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
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

                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.ProveedorID.Equals(M.ProveedorID) && d.Valor.Equals(M.Monto) && d.MovimientoID.Equals(ID)).FirstOrDefault();

                    if (D != null)
                    {
                        db.Deudors.DeleteOnSubmit(D);
                        db.SubmitChanges();
                    }
                    #endregion

                    //glkOrdenes.Properties.DataSource = (from o in dv.VistaOrdenesCompras where o.Aprobado && o.Estado.Equals(4) && o.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && o.SubEstacionID.Equals(Parametros.General.SubEstacionID) select new { o.ID, o.ProveedorNombre, o.Numero, o.Fecha, Display = o.Numero + " | " + o.ProveedorNombre }).OrderBy(o => o.Fecha);
                    var Orden = db.OrdenCompras.FirstOrDefault(oc => oc.MovimientoID.Equals(Manterior.ID));
                    if (Orden != null)
                    {
                        Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Orden.ID));
                        OC.MovimientoID = 0;
                        OC.Estado = 4;
                        db.SubmitChanges();
                    }

                    //var Pedido = db.PedidoCompras.Where(p => p.MovimientoID.Equals(Manterior.ID));
                    //if (Pedido != null)
                    //{
                    //    Entidad.Pedido P = db.Pedidos.Single(o => o.ID.Equals(Pedido.First().PedidoID));
                    //    P.Finalizado = false;
                    //    db.PedidoCompras.DeleteAllOnSubmit(Pedido);
                    //    db.SubmitChanges();
                    //}

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
                            AnularCompra(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(3))
                        {
                            nf = new Forms.Dialogs.DialogComprasPedido(Usuario, true);
                            nf.Text = "Vista Previa de Compra";
                            nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " + 
                                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
                            nf.EntidadAnterior = db.Movimientos.Single(m => m.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nf.bntNew.Enabled = this.btnAgregar.Enabled;
                            nf.Owner = this.Owner;
                            nf.MdiParent = this.MdiParent;
                            nf.MDI = this;
                            nf.Show();
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
              
        #endregion   

        
    }
}
