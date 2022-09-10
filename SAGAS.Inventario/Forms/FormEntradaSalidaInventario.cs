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
    public partial class FormEntradaSalidaInventario : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogEntradaSalidaInventario nf;
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

        public FormEntradaSalidaInventario()
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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(1) || v.MovimientoTipoID.Equals(2) || v.MovimientoTipoID.Equals(10) || v.MovimientoTipoID.Equals(14) || v.MovimientoTipoID.Equals(15) || v.MovimientoTipoID.Equals(16))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Referencia,
                                iv.Numero,
                                iv.EstacionServicioID,
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
                                iv.Monto
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
                rv.Text = "Movimiento de Inventario " + VM.First().Numero;

                Reportes.Inventario.Hojas.RptComprobanteMovimiento rep = new Reportes.Inventario.Hojas.RptComprobanteMovimiento();
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

                if (VM.First().MovimientoTipoID.Equals(1))
                {
                    rep.xrCellExisTitle.Dispose();
                    rep.xrCellExisValue.Dispose();
                }
                else if (VM.First().MovimientoTipoID.Equals(10))
                {
                    var vista = dbView.VistaMovimientos.Where(v => v.MovimientoReferenciaID.Equals(VM.First().ID));

                    if (vista.Count() > 0)
                    {//vista.First().VistaKardexes.First().CostoTotal
                        rep.xrCellAlmacenTitle.Text = "Almacén Entrada";
                        rep.DetailReport.DataSource = vista;
                        rep.xrTableCell17.DataBindings.Add("Text", null, "VistaKardexes.AlmacenEntradaNombre");
                        rep.xrTableCell18.DataBindings.Add("Text", null, "VistaKardexes.CantidadEntrada");
                        rep.xrTableCell19.DataBindings.Add("Text", null, "VistaKardexes.CostoEntrada");    
                        rep.xrCellValorSubtotal.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal"); 
                        rep.xrTableCell4.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal"); 
 
                    }
                }

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

                    if (Tipo.Equals(1) || Tipo.Equals(2))
                    {
                        #region <<< ANULACION_ENTRADA/SALIDA >>>

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = (Tipo.Equals(1) ? 14 : 15);
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
                        M.AlmacenID = Manterior.AlmacenID;
                        M.Comentario = Manterior.Comentario;
                        

                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el movimiento de inventario : " + Manterior.Numero.ToString("000000000"), this.Name);

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

                            int Alma = 0;

                            if (Tipo.Equals(1))
                            {
                                //Anulacion de entrada
                                //*****REFLEJA SALIDA******//
                                Alma = dk.AlmacenEntradaID;
                                KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                KX.CantidadSalida = dk.CantidadEntrada;
                                KX.CantidadInicial = dk.CantidadFinal;
                                KX.CantidadFinal = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoFinal;
                                KX.CostoFinal = dk.CostoInicial;
                                KX.CostoSalida = dk.CostoEntrada;                                
                                KX.CostoTotal = dk.CostoTotal;
                                KX.EsProducto = dk.EsProducto;

                                decimal Valor = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha, false) + Parametros.General.SaldoKardexPost(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha) - KX.CantidadSalida;
                                if (Valor < 0)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    trans.Rollback();
                                    Parametros.General.DialogMsg("El producto : " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " no puede quedar en negativo " + Valor.ToString("#,0.000"), Parametros.MsgType.warning);
                                    return;
                                }

                                db.Kardexes.InsertOnSubmit(KX);
                                db.SubmitChanges();

                                #region <<< ACTUALIZAR COSTOS >>>
                                                               
                                //var ActualizarCosto = (from k in db.Kardexes
                                //                       join m in db.Movimientos on k.Movimiento equals m
                                //                       join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                //                       where k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && k.ProductoID.Equals(KX.ProductoID) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                                //                       && k.Fecha.Date >= KX.Fecha.Date
                                //                       select new { k.ID, k.Fecha, mt.Entrada }).OrderBy(o => o.Fecha).ThenBy(t => !t.Entrada).ToList();

                                //var KCI = (from k in db.Kardexes
                                //               join m in db.Movimientos on k.MovimientoID equals m.ID
                                //               where k.ProductoID.Equals(KX.ProductoID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                //             && k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && !k.CostoEntrada.Equals(0)
                                //             && k.Fecha.Date <= KX.Fecha.Date
                                //             && !k.ID.Equals(dk.ID)
                                //               select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();


                                //decimal vCosto = 0m;
                                //if (KCI.Count() > 0)
                                //    vCosto = Decimal.Round(KCI.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                                //foreach (var k in ActualizarCosto)
                                //{
                                //    if (!k.ID.Equals(dk.ID))
                                //    {
                                //        Entidad.Kardex NK = db.Kardexes.Single(o => o.ID.Equals(k.ID));

                                //        if (NK.Movimiento.MovimientoTipoID.Equals(1) || NK.Movimiento.MovimientoTipoID.Equals(3))
                                //        {
                                //            //var NKCI = (from kd in db.Kardexes
                                //            //            join mt in db.Movimientos on kd.MovimientoID equals mt.ID
                                //            //            where kd.ProductoID.Equals(KX.ProductoID) && mt.Anulado.Equals(false) && (mt.MovimientoTipoID.Equals(3) || mt.MovimientoTipoID.Equals(1))
                                //            //              && kd.EstacionServicioID.Equals(KX.EstacionServicioID) && kd.SubEstacionID.Equals(KX.SubEstacionID) && !kd.CostoEntrada.Equals(0)
                                //            //              && kd.Fecha.Date <= KX.Fecha && (!kd.ID.Equals(dk.ID) && !kd.ID.Equals(NK.ID))
                                //            //            select new { kd.ID, kd.Fecha, kd.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                                //            //decimal nvCosto = 0m;
                                //            //if (NKCI.Count() > 0)
                                //            //    nvCosto = Decimal.Round(KCI.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                                //            ////Costo Inicial es vCosto
                                //            //decimal vCantidadInicial = 0m;
                                //            ////Cantidad Inicial para el calculo
                                //            //vCantidadInicial = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, 0, KX.Fecha, true) - KX.CantidadSalida;

                                //            //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * nvCosto), 4, MidpointRounding.AwayFromZero);
                                //            //vCosto = Decimal.Round(Convert.ToDecimal((vCostosInicialesRecalculo + NK.CostoTotal) / vCantidadInicial), 4, MidpointRounding.AwayFromZero);

                                //            //if (NK.Fecha.Date >= KX.Fecha.Date)
                                //            //    NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                //            //Costo Inicial es vCosto
                                //            decimal vCantidades = 0m;
                                //            //Cantidad Inicial para el calculo
                                //            vCantidades = Parametros.General.SaldoKardexAnulacion(db, dk.ID, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, 0, NK.Fecha, true);
                                //            //Costo Total Inicial para el calculo
                                //            //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);

                                //            decimal vSaldoAct = 0m;
                                //            Parametros.General.SaldoCostoTotalKardex(db, dk.ID, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, NK.Fecha, out vSaldoAct);

                                //            vCosto = Decimal.Round(Convert.ToDecimal(vSaldoAct / vCantidades), 4, MidpointRounding.AwayFromZero);

                                //            if (NK.Fecha.Date >= KX.Fecha.Date)
                                //                NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);


                                //        }
                                //        else
                                //        {

                                //            NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //            if (!NK.CostoEntrada.Equals(0))
                                //            {
                                //                NK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                //            }

                                //            if (!NK.CostoSalida.Equals(0))
                                //            {
                                //                NK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                //            }

                                //            db.SubmitChanges();

                                //            var Anterior = db.Kardexes.Where(o => o.MovimientoID.Equals(NK.MovimientoID) && !o.ID.Equals(NK.ID)).Select(s => new { s.ID, s.CostoFinal });

                                //            decimal vSuma = 0m;

                                //            if (Anterior.Count() > 0)
                                //                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoFinal), 2, MidpointRounding.AwayFromZero);

                                //            //Entidad.Movimiento MK = db.Movimientos.Single(o => o.ID.Equals(NK.MovimientoID));

                                //            var area = from a in db.Areas
                                //                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                //                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                //                       where p.ID.Equals(NK.ProductoID)
                                //                       select a;

                                //            if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                //            {
                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                //                if (NCC.Monto < 0)
                                //                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                else
                                //                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                db.SubmitChanges();
                                //            }
                                //            else
                                //            {
                                //                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaCostoID;

                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                //                if (NCC.Monto < 0)
                                //                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                else
                                //                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                db.SubmitChanges();

                                //            }

                                //            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                //            {
                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                //                if (NCC.Monto < 0)
                                //                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                else
                                //                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                db.SubmitChanges();
                                //            }
                                //            else
                                //            {
                                //                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaInventarioID;

                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                //                if (NCC.Monto < 0)
                                //                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                else
                                //                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                db.SubmitChanges();

                                //            }

                                //        }
                                //    }
                                //}

                                #endregion

                            }
                            else
                            {
                                //Anulacion de salida
                                //*****REFLEJA ENTRADA******//
                                Alma = dk.AlmacenSalidaID; 
                                KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                KX.CantidadEntrada = dk.CantidadSalida;
                                KX.CantidadInicial = dk.CantidadFinal;
                                KX.CantidadFinal = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoFinal;
                                KX.CostoFinal = dk.CostoInicial;
                                KX.CostoEntrada = dk.CostoSalida;
                                KX.CostoTotal = dk.CostoTotal;
                                KX.EsProducto = dk.EsProducto;

                                db.Kardexes.InsertOnSubmit(KX);
                                db.SubmitChanges();
                            }
                            

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                            
                                var AL = (from al in db.AlmacenProductos
                                          where al.ProductoID.Equals(KX.ProductoID)
                                            && al.AlmacenID.Equals(Alma)
                                          select al).ToList();

                                if (AL.Count() > 0)
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(Alma));
                                    if (Tipo.Equals(2))
                                        AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;
                                    if (Tipo.Equals(1))
                                        AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;
                            
                                        decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                        //-- INSERTAR REGISTRO 
                                        ///////////////////**********************************************************
                                        AP.Costo = KX.CostoFinal;
                                        AP.PrecioSugerido = precio;
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
                            #endregion                   
                    }
                    else if (Tipo.Equals(10))
                    {                        
                               
                        #region <<< ANULACION_RECIBIDO >>>

                        Entidad.Movimiento MRanterior = db.Movimientos.SingleOrDefault(m => m.MovimientoReferenciaID.Equals(ID));

                        if (MRanterior != null)
                        {                            

                            Entidad.Movimiento MR = new Entidad.Movimiento();

                            MR.EstacionServicioID = MRanterior.EstacionServicioID;
                            MR.SubEstacionID = MRanterior.SubEstacionID;
                            MR.MovimientoTipoID = 17;
                            MR.UsuarioID = Parametros.General.UserID;
                            MR.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                            MR.FechaRegistro = MRanterior.FechaRegistro;
                            MR.Monto = MRanterior.Monto;
                            MR.MonedaID = MRanterior.MonedaID;
                            MR.MontoMonedaSecundaria = MRanterior.MontoMonedaSecundaria;
                            MR.TipoCambio = MRanterior.TipoCambio;
                            MR.Referencia = "ANULADO   " + MRanterior.Numero.ToString();
                            MR.AlmacenID = MRanterior.AlmacenID;
                            MR.Comentario = MRanterior.Comentario;
                            MR.MovimientoReferenciaID = MRanterior.ID;

                            db.Movimientos.InsertOnSubmit(MR);

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el movimiento de inventario : " + MRanterior.Numero.ToString("000000000"), this.Name);

                            foreach (var dk in MRanterior.Kardexes)
                            {
                                //decimal CostoMov = LineaDetalle.Cost;
                                Entidad.Kardex KX = new Entidad.Kardex();
                                KX.ProductoID = dk.ProductoID;
                                KX.UnidadMedidaID = dk.UnidadMedidaID;
                                KX.Fecha = Manterior.FechaRegistro;
                                KX.MovimientoID = MR.ID;
                                KX.EstacionServicioID = dk.EstacionServicioID;
                                KX.SubEstacionID = dk.SubEstacionID;
                                int Alma = 0;
                                //Anulacion de entrada
                                //    //*****REFLEJA SALIDA******//
                                Alma = dk.AlmacenEntradaID;
                                KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                KX.CantidadSalida = dk.CantidadEntrada;
                                KX.CantidadInicial = dk.CantidadFinal;
                                KX.CantidadFinal = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoFinal;
                                KX.CostoFinal = dk.CostoInicial;
                                KX.CostoSalida = dk.CostoEntrada;
                                KX.CostoTotal = dk.CostoTotal;
                                KX.EsProducto = dk.EsProducto;

                                decimal Valor = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha, false) + Parametros.General.SaldoKardexPost(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha) - KX.CantidadSalida;
                                if (Valor < 0)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    trans.Rollback();
                                    Parametros.General.DialogMsg("El producto : " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " no puede quedar en negativo " + Valor.ToString("#,0.000"), Parametros.MsgType.warning);
                                    return;
                                }

                                db.Kardexes.InsertOnSubmit(KX);

                                #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                                //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                                var AL = (from al in db.AlmacenProductos
                                          where al.ProductoID.Equals(KX.ProductoID)
                                            && al.AlmacenID.Equals(Alma)
                                          select al).ToList();

                                if (AL.Count() > 0)
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(Alma));
                                    AP.Cantidad = Valor;
                                }
                                //------------------------------------------------------------------------//

                                #endregion

                                //para que actualice los datos del registro
                                db.SubmitChanges();
                            }

                            MRanterior.Anulado = true;
                            MRanterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                            MRanterior.UsuarioAnuladoID = Parametros.General.UserID;

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                        }
                        #endregion

                        #region <<< ANULACION_TRASLADO >>>

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = 16;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.AlmacenID = Manterior.AlmacenID;
                        M.Comentario = Manterior.Comentario;

                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el movimiento de inventario : " + Manterior.Numero.ToString("000000000"), this.Name);

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

                            int Alma = 0;

                            //if (Tipo.Equals(1))
                            //{
                            //    //Anulacion de entrada
                            //    //*****REFLEJA SALIDA******//
                            //    Alma = dk.AlmacenEntradaID;
                            //    KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                            //    KX.CantidadSalida = dk.CantidadEntrada;
                            //    KX.CantidadInicial = dk.CantidadFinal;
                            //    KX.CantidadFinal = dk.CantidadInicial;
                            //    KX.CostoInicial = dk.CostoFinal;
                            //    KX.CostoFinal = dk.CostoInicial;
                            //    KX.CostoSalida = dk.CostoEntrada;
                            //    KX.CostoTotal = dk.CostoTotal;
                            //    KX.EsProducto = dk.EsProducto;
                            //}
                            //else
                            //{
                            //Anulacion de salida
                            //*****REFLEJA ENTRADA******//
                            Alma = dk.AlmacenSalidaID;
                            KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                            KX.CantidadEntrada = dk.CantidadSalida;
                            KX.CantidadInicial = dk.CantidadFinal;
                            KX.CantidadFinal = dk.CantidadInicial;
                            KX.CostoInicial = dk.CostoFinal;
                            KX.CostoFinal = dk.CostoInicial;
                            KX.CostoEntrada = dk.CostoSalida;
                            KX.CostoTotal = dk.CostoTotal;
                            KX.EsProducto = dk.EsProducto;



                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var AL = (from al in db.AlmacenProductos
                                      where al.ProductoID.Equals(KX.ProductoID)
                                        && al.AlmacenID.Equals(Alma)
                                      select al).ToList();

                            if (AL.Count() > 0)
                            {
                                Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(Alma));
                                AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;
                                //if (Tipo.Equals(1))
                                //    AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;

                            }
                            //------------------------------------------------------------------------//

                            #endregion

                            //para que actualice los datos del registro
                            db.SubmitChanges();

                        }

                        #endregion

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();
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
                    nf = new Forms.Dialogs.DialogEntradaSalidaInventario(Usuario, false);
                    nf.Text = "Crear Entr / Sal / Tras";
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
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(14) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(15) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(16))
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
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();

                    if (M.MovimientoTipoID.Equals(1) || M.MovimientoTipoID.Equals(2) || M.MovimientoTipoID.Equals(10) || M.MovimientoTipoID.Equals(11))
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                    else
                        page.Text = M.Abreviatura + " " + M.Referencia;

                    page.Tag = M.ID;

                    Parametros.MyXtraGridMovimientoInventario xtra = new Parametros.MyXtraGridMovimientoInventario();
                    if (M.MovimientoTipoID.Equals(1) || M.MovimientoTipoID.Equals(15) || M.MovimientoTipoID.Equals(11) || M.MovimientoTipoID.Equals(16))
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenEntradaNombre, Cantidad = s.CantidadEntrada, Costo = s.CostoEntrada, Total = Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero) }).ToList();
                        xtra.gridComprobante.DataSource = M.VistaComprobantes;
                    }
                    else if (M.MovimientoTipoID.Equals(2) || M.MovimientoTipoID.Equals(14) || M.MovimientoTipoID.Equals(10) || M.MovimientoTipoID.Equals(17))
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
