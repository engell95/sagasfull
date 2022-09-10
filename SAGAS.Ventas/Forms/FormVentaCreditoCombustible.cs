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
    public partial class FormVentaCreditoCombustible : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogVentaCreditoCombustible nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _ClienteAnticipo;
        
        #endregion

        #region <<< INICIO >>>

        public FormVentaCreditoCombustible()
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
                    _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
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
                var query = from iv in dv.VistaMovimientos.Where(v => (!v.ResumenDiaID.Equals(0) && (v.MovimientoTipoID.Equals(7) || v.MovimientoTipoID.Equals(8))) 
                    || (v.MovimientoTipoID.Equals(7) && !v.MovimientoReferenciaID.Equals(0)))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.Referencia,
                                //Numero = (String.IsNullOrEmpty(iv.Referencia) ? String.Format("{0:000000000}", iv.Numero) : iv.Referencia),
                                //   String.Format("{0:000000000}", iv.Numero) : iv.Referencia),
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Observacion = iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Cliente = iv.ClienteCodigo + " | " + iv.ClienteNombre,
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                iv.Pagado,
                                iv.Monto,
                                iv.NombreCuentaContable
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
                if (VM.First().MovimientoReferenciaID.Equals(0))
                {
                    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    rv.Text = VM.First().MovimientoTipoNombre + " " + VM.First().Referencia;

                    Reportes.Contabilidad.Hojas.RptComprobanteVentaCredito rep = new Reportes.Contabilidad.Hojas.RptComprobanteVentaCredito();
                    //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                    rep.DataSource = VM;

                    var RD = db.ResumenDias.SingleOrDefault(r => r.ID.Equals(VM.First().ResumenDiaID));

                    if (RD != null)
                    {
                        rep.xrTableCellRDNro.Text = RD.Numero.ToString();
                    }

                    rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                    rv.Owner = this.Owner;
                    rv.MdiParent = this.MdiParent;
                    rep.RequestParameters = false;
                    rep.CreateDocument();

                    rv.Show();
                }
                else
                    Parametros.General.DialogMsg("No se puede generar la vista previa, la venta esta relacionado a un Resumen del Día.", Parametros.MsgType.warning);

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

                    if (Manterior.MovimientoReferenciaID >  0)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("La venta no se puede anular, esta referenciada a un Comprobante de Arqueo.", Parametros.MsgType.warning);
                        trans.Rollback();
                        return;
                    }

                    if (Manterior.ResumenDiaID > 0)
                    {
                        if (db.ResumenDias.Single(rd => rd.ID.Equals(Manterior.ResumenDiaID)).Contabilizado)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("La Venta no se puede anular, el Resumen del Día para esta fecha ya esta contabilizado.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }


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
                                Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                                trans.Rollback();
                                return;
                            }
                        }
                        else
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }
                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    #region <<< ANULACION_ENTRADA/SALIDA >>>

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 8;
                    M.ClienteID = Manterior.ClienteID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaVencimiento = null;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Numero = Manterior.Numero;
                    M.Credito = false;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    M.AlmacenID = Manterior.AlmacenID;
                    M.ResumenDiaID = Manterior.ResumenDiaID;
                    M.Comentario = Manterior.Comentario;
                    M.AreaID = Manterior.AreaID;

                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                                "Se anuló la Venta de Crédito : " + Manterior.Numero, this.Name);

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

                        KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                        KX.CantidadEntrada = dk.CantidadSalida;
                        KX.CantidadInicial = dk.CantidadFinal;
                        KX.CantidadFinal = dk.CantidadInicial;
                        KX.CostoEntrada = dk.CostoSalida;
                        KX.EsProducto = dk.EsProducto;
                        KX.CostoTotal = dk.CostoTotal;
                        KX.Precio = dk.Precio;
                        KX.PrecioTotal = dk.PrecioTotal;
                        KX.Descuento = dk.Descuento;
                        KX.EsProducto = dk.EsProducto;


                        //Anulacion de salida
                        //*****REFLEJA ENTRADA******//
                        //Alma = dk.AlmacenSalidaID; 
                        //KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                        //KX.CantidadEntrada = dk.CantidadSalida;
                        //KX.CantidadInicial = dk.CantidadFinal;
                        //KX.CantidadFinal = dk.CantidadInicial;
                        //KX.CostoInicial = dk.CostoFinal;
                        //KX.CostoFinal = dk.CostoInicial;
                        //KX.CostoEntrada = dk.CostoSalida;
                        //KX.CostoTotal = dk.CostoTotal;
                        //KX.EsProducto = dk.EsProducto;
                        
                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                        var AL = (from al in db.TanqueProductos
                                  where al.ProductoID.Equals(KX.ProductoID)
                                    && al.TanqueID.Equals(KX.AlmacenEntradaID)
                                  select al).ToList();

                        if (AL.Count() > 0)
                        {
                            Entidad.TanqueProducto TP = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenEntradaID));
                            TP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;

                            //if (Tipo.Equals(2))
                            //    AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;
                            //if (Tipo.Equals(1))

                        }


                        //------------------------------------------------------------------------//

                        #endregion

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

                        //para que actualice los datos del registro
                        db.SubmitChanges();
                    }

                    #endregion

                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }
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

            //if (!ID.Equals(0))
            //    ImprimirComprobante(ID, true);
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
                    nf = new Forms.Dialogs.DialogVentaCreditoCombustible(Usuario, false);
                    nf.Text = "Crear Nueva Venta al Crédito";
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
                    if (M.MovimientoReferenciaID.Equals(0))
                    {
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridVentaCredito xtra = new Parametros.MyXtraGridVentaCredito();

                        if (M.EsAnulado)
                        {
                            xtra.ColAlmacenSal.Visible = false;
                            xtra.colQuantity.Visible = false;
                        }
                        else
                        {
                            xtra.ColAlmacenEnt.Visible = false;
                            xtra.colQuantityIn.Visible = false;
                        }
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
                    else
                        Parametros.General.DialogMsg("No se puede generar la vista previa, la venta esta relacionado a un Resumen del Día.", Parametros.MsgType.warning);

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
