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

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormVentas : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogVentas nf;
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
        private int _ClienteAnticipo;

        #endregion

        #region <<< INICIO >>>

        public FormVentas()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaOrdenesCompra);
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
                    _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
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
                            where lista.Contains(v.EstacionServicioID) && (v.MovimientoTipoID.Equals(7) || v.MovimientoTipoID.Equals(8)) && v.ResumenDiaID.Equals(0)
                            select new
                            {
                                v.ID,
                                Cliente = v.ClienteNombre,
                                v.EstacionServicioID,
                                FechaRegistro = v.FechaContabilizacion,
                                Year = v.FechaContabilizacion.Year,
                                Mes = v.FechaContabilizacion.Month,
                                Monto = (v.MonedaID.Equals(MonedaID) ? v.Monto : v.MontoMonedaSecundaria),
                                Numero = v.Numero.ToString(),
                                v.Anulado,
                                Moneda = v.MonedaSimbolo + " | " + v.MonedaNombre,
                                ES = v.EstacionNombre,
                                SUS = v.SubEstacionNombre,
                                v.MovimientoTipoID,
                                v.MovimientoTipoNombre,
                                v.Credito,
                                v.AreaVentaNombre,
                                v.Pagado,
                                v.Comentario

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
                rv.Text = "Comprobante de Venta " + VM.First().Referencia;

                Reportes.Contabilidad.Hojas.RptComprobanteVenta rep = new Reportes.Contabilidad.Hojas.RptComprobanteVenta();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.CeTitulo.Text = (VM.First().MovimientoTipoID.Equals(7) ? "Comprobante de Venta" : "Anulación de Venta");

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
                    nf = new Forms.Dialogs.DialogVentas(Usuario);
                    nf.Text = "Crear Nueva Venta";
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
        private void AnularVenta(int ID)
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
                        if (db.Clientes.Single(s => s.ID.Equals(Manterior.ClienteID)).CuentaContableID.Equals(_ClienteAnticipo))
                        {
                            if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "PermiteAnularAnticipo"))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                                if (Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "¿Desea proceder con la anulación?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                                {
                                    trans.Rollback();
                                    return;
                                }
                                else
                                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);
                            }
                            else
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "No tiene permiso para anular esta factura.", Parametros.MsgType.warning);
                                trans.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "La cuenta contable del cliente no es anticipo.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }
                    else
                    {
                        if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Manterior.ID)) > 0)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + Manterior.Referencia + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }


                    var obj = (from p in db.Productos
                               join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                               join ar in db.Areas on pc.AreaID equals ar.ID
                               where Manterior.Kardexes.Select(s => s.ProductoID).Contains(p.ID)
                               group ar by new { ar.ID, ar.Nombre } into gr
                               select new { ID = gr.Key.ID, Nombre = gr.Key.Nombre }).ToList();

                    string texto = "";
                    obj.ForEach(det =>
                    {
                        if (!Parametros.General.ValidateKardexMovemente(Manterior.FechaRegistro.Date, db, Manterior.EstacionServicioID, Manterior.SubEstacionID, 9, det.ID))
                        {
                            texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                        }
                    });

                    if (!String.IsNullOrEmpty(texto))
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Manterior.FechaRegistro.Date.ToShortDateString() + " y Área: " + texto, Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 8;
                    M.ClienteID = Manterior.ClienteID;
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
                    M.Numero = Manterior.Numero;
                    M.AreaID = Manterior.AreaID;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                                "Se anuló la Venta: " + M.Referencia, this.Name);

                    foreach (var dk in Manterior.Kardexes)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        //decimal CostoMov = LineaDetalle.Cost;
                        Entidad.Kardex KX = new Entidad.Kardex();
                        KX.ProductoID = dk.ProductoID;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;
                        KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                        KX.CantidadEntrada = dk.CantidadSalida;
                        KX.CantidadInicial = dk.CantidadFinal;
                        KX.CantidadFinal = dk.CantidadInicial;
                        KX.CostoEntrada = dk.CostoSalida;
                        KX.EstacionServicioID = dk.EstacionServicioID;
                        KX.SubEstacionID = dk.SubEstacionID;
                        KX.MovimientoID = M.ID;
                        KX.CostoTotal = dk.CostoTotal;
                        KX.ImpuestoTotal = dk.ImpuestoTotal;
                        KX.Precio = dk.Precio;
                        KX.PrecioTotal = dk.PrecioTotal;
                        KX.Descuento = dk.Descuento;
                        KX.EsProducto = dk.EsProducto;

                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                        if (!Producto.ProductoClaseID.Equals(_IDClaseCombustible))
                        {
                            var AL = (from al in db.AlmacenProductos
                                      where al.ProductoID.Equals(KX.ProductoID)
                                        && al.AlmacenID.Equals(KX.AlmacenEntradaID)
                                      select al).ToList();

                            if (AL.Count() > 0)
                            {
                                Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(KX.AlmacenEntradaID));
                                AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;                                
                            }
                        }
                        else if (Producto.ProductoClaseID.Equals(_IDClaseCombustible))
                        {
                            var TP = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(KX.ProductoID)
                                        && tp.TanqueID.Equals(KX.AlmacenEntradaID)
                                      select tp).ToList();

                            if (TP.Count() > 0)
                            {
                                Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenEntradaID));
                                T.Cantidad = TP.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - dk.CantidadSalida;
                                T.Costo = KX.CostoFinal;
                            }
                        }
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

                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.ClienteID.Equals(M.ClienteID) && d.Valor.Equals(M.Monto) && d.MovimientoID.Equals(ID)).FirstOrDefault();

                    if (D != null)
                    {
                        db.Deudors.DeleteOnSubmit(D);
                        db.SubmitChanges();
                    }
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

        
        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(8))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularVenta(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                    page.Text = "Venta " + M.Numero.ToString("000000000");
                    page.Tag = M.ID;

                    Parametros.MyXtraGridVentas xtra = new Parametros.MyXtraGridVentas();
                    xtra.gridDetalle.DataSource = M.VistaKardexes;
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
