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
    public partial class FormPrestamoManejo : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPrestamoManejo nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private Parametros.ListEstadosOrdenes listado = new Parametros.ListEstadosOrdenes();
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _IDClaseCombustible = Parametros.Config.ProductoClaseCombustible();
        
        #endregion

        #region <<< INICIO >>>

        public FormPrestamoManejo()
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
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaRegistro") > fecha.AddDays(-1)).ToString();
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
                var query = from v in dv.VistaMovimientos
                            where lista.Contains(v.EstacionServicioID) && 
                            (v.MovimientoTipoID.Equals(31) || v.MovimientoTipoID.Equals(32) || v.MovimientoTipoID.Equals(33) || v.MovimientoTipoID.Equals(34)
                            || v.MovimientoTipoID.Equals(35) || v.MovimientoTipoID.Equals(36) || v.MovimientoTipoID.Equals(37) || v.MovimientoTipoID.Equals(38))
                            select new
                            {
                                v.ID,
                                Finalizado = v.Finalizado,
                                FechaRegistro = v.FechaContabilizacion,
                                Year = v.FechaContabilizacion.Year,
                                Mes = v.FechaContabilizacion.Month,
                                Monto = (v.MonedaID.Equals(MonedaID) ? v.Monto : v.MontoMonedaSecundaria),
                                v.Numero,
                                v.Referencia,
                                v.Anulado,
                                ES = v.EstacionNombre,
                                SUS = v.SubEstacionNombre,
                                v.MovimientoTipoID,
                                v.MovimientoTipoNombre,
                                v.EstacionServicioID,
                                Enlace = (v.MovimientoReferenciaID <= 0 ? v.ID : v.MovimientoReferenciaID)
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

                Reportes.Inventario.Hojas.RptPrestamoManejo rep = new Reportes.Inventario.Hojas.RptPrestamoManejo();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                //rep.CeTitulo.Text = (VM.First().MovimientoTipoID.Equals(7) ? "Comprobante de Venta" : "Anulación de Venta");

                rep.DataSource = VM;

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, bool Refresh)
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
                    nf = new Forms.Dialogs.DialogPrestamoManejo(Usuario);
                    nf.Text = "Crear Prestamo Manejo";
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
        private void AnularPrestamo(int ID, int Tipo)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    if (Tipo.Equals(31) || Tipo.Equals(33))
                    {
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                        Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = (Tipo.Equals(31) ? 32 : 34);
                        M.ProveedorID = Manterior.ProveedorID;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.Litros = Manterior.Litros;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló el Prestamo Manejo de Cliente Manejo a E/S: " + M.Referencia, this.Name);

                        if (Tipo.Equals(33))
                        {
                            Entidad.Movimiento MEntrada = db.Movimientos.Single(m => m.ID.Equals(Manterior.MovimientoReferenciaID));
                            MEntrada.Finalizado = false;
                            db.SubmitChanges();
                        }

                        foreach (var dk in Manterior.Kardexes)
                        {
                            
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            int almacen = 0;
                            //decimal CostoMov = LineaDetalle.Cost;

                            #region <<< Primera >>>

                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.ProductoID = dk.ProductoID;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;
                            KX.MovimientoID = M.ID;
                            KX.EsProducto = dk.EsProducto;
                            KX.EsManejo = dk.EsManejo;

                            if (!KX.EsManejo)
                            {
                                if (Tipo.Equals(31))
                                {
                                    almacen = dk.AlmacenEntradaID;
                                    KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                    KX.CantidadSalida = dk.CantidadEntrada;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoSalida = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;

                                }
                                else if (Tipo.Equals(33))
                                {
                                    almacen = dk.AlmacenSalidaID;
                                    KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                    KX.CantidadEntrada = dk.CantidadSalida;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoEntrada = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;
                                }
                            }
                            else
                            {
                                if (Tipo.Equals(33))
                                {
                                    almacen = dk.AlmacenEntradaID;
                                    KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                    KX.CantidadSalida = dk.CantidadEntrada;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoSalida = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;

                                }
                                else if (Tipo.Equals(31))
                                {
                                    almacen = dk.AlmacenSalidaID;
                                    KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                    KX.CantidadEntrada = dk.CantidadSalida;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoEntrada = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;
                                }
                            }

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var TP = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(KX.ProductoID)
                                        && tp.TanqueID.Equals(almacen)
                                      select tp).ToList();

                            if (TP.Count() > 0)
                            {
                                Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(almacen));
                                T.Cantidad = KX.CantidadFinal;
                            }
                            //------------------------------------------------------------------------//

                            #endregion

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                            #endregion

                            //#region <<< Segunda >>>

                            //Entidad.Kardex KXs = new Entidad.Kardex();

                            //KXs.ProductoID = dk.ProductoID;
                            //KXs.UnidadMedidaID = dk.UnidadMedidaID;
                            //KXs.Fecha = M.FechaRegistro;
                            //KXs.EstacionServicioID = dk.EstacionServicioID;
                            //KXs.SubEstacionID = dk.SubEstacionID;
                            //KXs.MovimientoID = M.ID;
                            //KXs.EsProducto = dk.EsProducto;

                            //if (Tipo.Equals(33))
                            //{
                            //    almacen = dk.AlmacenEntradaID;
                            //    KXs.AlmacenSalidaID = dk.AlmacenEntradaID;
                            //    KXs.CantidadSalida = dk.CantidadEntrada;
                            //    KXs.CantidadInicial = dk.CantidadFinal;
                            //    KXs.CantidadFinal = dk.CantidadInicial;
                            //    KXs.CostoInicial = dk.CostoFinal;
                            //    KXs.CostoSalida = dk.CostoFinal;
                            //    KXs.CostoFinal = dk.CostoFinal;
                            //    KXs.CostoTotal = dk.CostoTotal;

                            //}
                            //else if (Tipo.Equals(31))
                            //{
                            //    almacen = dk.AlmacenSalidaID;
                            //    KXs.AlmacenEntradaID = dk.AlmacenSalidaID;
                            //    KXs.CantidadEntrada = dk.CantidadSalida;
                            //    KXs.CantidadFinal = dk.CantidadInicial;
                            //    KXs.CantidadInicial = dk.CantidadFinal;
                            //    KXs.CostoInicial = dk.CostoFinal;
                            //    KXs.CostoEntrada = dk.CostoFinal;
                            //    KXs.CostoFinal = dk.CostoFinal;
                            //    KXs.CostoTotal = dk.CostoTotal;
                            //}

                            //db.Kardexes.InsertOnSubmit(KXs);

                            //#region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            ////------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            //var TPSeg = (from tp in db.TanqueProductos
                            //             where tp.ProductoID.Equals(KXs.ProductoID)
                            //            && tp.TanqueID.Equals(almacen)
                            //          select tp).ToList();

                            //if (TPSeg.Count() > 0)
                            //{
                            //    Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(KXs.ProductoID) && p.TanqueID.Equals(almacen));
                            //    T.Cantidad = KX.CantidadFinal;
                            //}
                            ////------------------------------------------------------------------------//

                            //#endregion

                            ////para que actualice los datos del registro
                            //db.SubmitChanges();
                            //#endregion

                        }

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

                        #region ::: QUITANDO ALMACEN PRESTAMO :::

                        Entidad.AlmacenPrestamo AP = db.AlmacenPrestamos.Where(d => d.MovimientoID.Equals(Manterior.ID) && d.EstacionServicioID.Equals(Manterior.EstacionServicioID) && d.SubEstacionID.Equals(Manterior.SubEstacionID)).FirstOrDefault();

                        if (AP != null)
                        {
                            db.AlmacenPrestamos.DeleteOnSubmit(AP);
                            db.SubmitChanges();
                        }
                        #endregion
                    }
                    else if (Tipo.Equals(35) || Tipo.Equals(37))
                    {
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                        Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = (Tipo.Equals(35) ? 36 : 38);
                        M.ClienteID = Manterior.ClienteID;
                        M.UsuarioID = Parametros.General.UserID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Manterior.FechaRegistro;
                        M.Monto = Manterior.Monto;
                        M.MonedaID = Manterior.MonedaID;
                        M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                        M.TipoCambio = Manterior.TipoCambio;
                        M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                        M.Litros = Manterior.Litros;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                    "Se anuló Prestamo Manejo de E/S a Cliente Manejo: " + M.Referencia, this.Name);

                        if (Tipo.Equals(37))
                        {
                            Entidad.Movimiento MEntrada = db.Movimientos.Single(m => m.ID.Equals(Manterior.MovimientoReferenciaID));
                            MEntrada.Finalizado = false;
                            db.SubmitChanges();
                        }

                        foreach (var dk in Manterior.Kardexes)
                        {

                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            int almacen = 0;
                            //decimal CostoMov = LineaDetalle.Cost;
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.ProductoID = dk.ProductoID;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.EstacionServicioID = dk.EstacionServicioID;
                            KX.SubEstacionID = dk.SubEstacionID;
                            KX.MovimientoID = M.ID;
                            KX.EsProducto = dk.EsProducto;
                            KX.EsManejo = dk.EsManejo;

                            if (!KX.EsManejo)
                            {
                                if (Tipo.Equals(37))
                                {
                                    almacen = dk.AlmacenEntradaID;
                                    KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                    KX.CantidadSalida = dk.CantidadEntrada;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoSalida = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;

                                }
                                else if (Tipo.Equals(35))
                                {
                                    almacen = dk.AlmacenSalidaID;
                                    KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                    KX.CantidadEntrada = dk.CantidadSalida;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoEntrada = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;
                                }
                            }
                            else
                            {
                                if (Tipo.Equals(35))
                                {
                                    almacen = dk.AlmacenEntradaID;
                                    KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                                    KX.CantidadSalida = dk.CantidadEntrada;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoSalida = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;

                                }
                                else if (Tipo.Equals(37))
                                {
                                    almacen = dk.AlmacenSalidaID;
                                    KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                    KX.CantidadEntrada = dk.CantidadSalida;
                                    KX.CantidadFinal = dk.CantidadInicial;
                                    KX.CantidadInicial = dk.CantidadFinal;
                                    KX.CostoInicial = dk.CostoFinal;
                                    KX.CostoEntrada = dk.CostoFinal;
                                    KX.CostoFinal = dk.CostoFinal;
                                    KX.CostoTotal = dk.CostoTotal;
                                }
                            }

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            //var TP = (from tp in db.TanqueProductos
                            //          where tp.ProductoID.Equals(KX.ProductoID)
                            //            && tp.TanqueID.Equals(almacen)
                            //          select tp).ToList();

                            //if (TP.Count() > 0)
                            //{
                            //    Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(almacen));
                            //    T.Cantidad = KX.CantidadFinal;
                            //}
                            //------------------------------------------------------------------------//

                            #endregion

                            //para que actualice los datos del registro
                            db.SubmitChanges();

                        }

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

                        #region ::: QUITANDO ALMACEN PRESTAMO :::

                        Entidad.AlmacenPrestamo AP = db.AlmacenPrestamos.Where(d => d.MovimientoID.Equals(Manterior.ID) && d.EstacionServicioID.Equals(Manterior.EstacionServicioID) && d.SubEstacionID.Equals(Manterior.SubEstacionID)).FirstOrDefault();

                        if (AP != null)
                        {
                            db.AlmacenPrestamos.DeleteOnSubmit(AP);
                            db.SubmitChanges();
                        }
                        #endregion
                    }

                    db.SubmitChanges();
                    trans.Commit();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
                finally
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }
                
        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colFinalizado)))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "El registro ya fue finalizado", Parametros.MsgType.warning);
                else
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(32) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(34))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaRegistro")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                            {
                                AnularPrestamo(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
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
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = M.Abreviatura + " " + (M.Numero.Equals(0) ? M.Referencia : M.Numero.ToString("000000000"));
                    page.Tag = M.ID;

                    Parametros.MyXtraGridPrestamoManejo xtra = new Parametros.MyXtraGridPrestamoManejo();                    
                    xtra.gridComprobante.DataSource = M.VistaComprobantes;

                    if (M.MovimientoTipoID.Equals(31) || M.MovimientoTipoID.Equals(34) || M.MovimientoTipoID.Equals(35) || M.MovimientoTipoID.Equals(38))
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Where(o => o.CantidadEntrada > 0);
                        xtra.ColAlmacenSalida.Visible = false;
                        xtra.colQuantitySalida.Visible = false;
                    }
                    else
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Where(o => o.CantidadSalida > 0);
                        xtra.ColAlmacenEntrada.Visible = false;
                        xtra.colQuantityEntrada.Visible = false;
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

        //Cerrando las pestañas detalles de ordenes
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
