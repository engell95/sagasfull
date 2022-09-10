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

namespace SAGAS.Inventario.Forms
{
    public partial class FormAutoConsumo : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogAutoconsumo nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();   
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private bool btnAprobar = false; 
        
        #endregion

        #region <<< INICIO >>>

        public FormAutoConsumo()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Anular";
            this.btnModificar.Caption = "Aprobar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Glyph = Properties.Resources.AprobarOrden;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.MovimientoTipo);
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
                var query = from iv in dv.VistaMovimientos.Where(v => (v.MovimientoTipoID.Equals(25) || v.MovimientoTipoID.Equals(26)))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Observacion = iv.Comentario,
                                iv.MovimientoTipoNombre,
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                iv.Referencia,
                                iv.Monto,
                                iv.ExtracionConceptoNombre,
                                iv.ExtracionPagoNombre,
                                iv.Finalizado,
                                iv.Entregado
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
            try
            {
                ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
            }
            catch { }
        }

        private void ImprimirComprobante(int ID, bool ToPrint)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Salida Combustible " + VM.First().Numero;

                Reportes.Inventario.Hojas.RptComprobanteSalidaCombustible rep = new Reportes.Inventario.Hojas.RptComprobanteSalidaCombustible();
                
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.DataSource = VM;


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

        private void AnularMovimiento(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    if (Manterior.ResumenDiaID > 0)
                    {
                        if (db.ResumenDias.Single(rd => rd.ID.Equals(Manterior.ResumenDiaID)).Contabilizado)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("La salida de combustible no se puede anular, el Resumen del Día para esta fecha ya esta contabilizado.", Parametros.MsgType.warning);
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
                    M.MovimientoTipoID = 26;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaVencimiento = null;
                    M.MonedaID = Manterior.MonedaID;
                    M.Monto = Manterior.Monto;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Credito = false;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    M.AlmacenID = Manterior.AlmacenID;
                    M.ResumenDiaID = Manterior.ResumenDiaID;
                    M.Comentario = Manterior.Comentario;

                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                "Se anuló la Salida de Inventario: " + Manterior.Referencia, this.Name);

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

        private void AnularMovimientoTemporal(int ID, int Tipo)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento M = db.Movimientos.Single(m => m.ID.Equals(ID));

                    #region <<< ELIMINAR_KARDEX >>>

                    db.TemporalKardexes.Where(k => k.MovimientoTemporalID.Equals(M.ID)).ToList().ForEach(db.TemporalKardexes.DeleteOnSubmit);
                    db.SubmitChanges();

                    #endregion

                    #region <<< ELIMINAR_COMPROBANTE >>>

                    db.TemporalComprobanteContables.Where(c => c.MovimientoTemporalID.Equals(M.ID)).ToList().ForEach(db.TemporalComprobanteContables.DeleteOnSubmit);
                    db.SubmitChanges();

                    #endregion

                    db.Movimientos.DeleteOnSubmit(M);

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

        private bool AprobarMovimiento(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento M = db.Movimientos.Single(m => m.ID.Equals(ID));

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(M.EstacionServicioID) && m.SubEstacionID.Equals(M.SubEstacionID) && m.MovimientoTipoID.Equals(25)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(M.EstacionServicioID) && m.SubEstacionID.Equals(M.SubEstacionID) && m.MovimientoTipoID.Equals(25)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    M.Numero = number;
                    M.Finalizado = true;
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                    "Se Aprobó la Salida de Combustible: " + M.Numero, this.Name);
                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    var DetalleEM = db.TemporalKardexes.Where(k => k.MovimientoTemporalID.Equals(M.ID));
                    foreach (var dk in DetalleEM)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                        {
                            trans.Rollback();
                            Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                            return false;
                        }

                        Entidad.Kardex KX = new Entidad.Kardex();

                        KX.MovimientoID = dk.MovimientoTemporalID;
                        KX.ProductoID = Producto.ID;
                        KX.EsProducto = !Producto.EsServicio;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;
                        KX.EstacionServicioID = dk.EstacionServicioID;
                        KX.SubEstacionID = dk.SubEstacionID;
                        KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                        KX.CantidadSalida = dk.CantidadSalida;
                        KX.CostoSalida = dk.CostoSalida;
                        KX.CostoTotal = dk.CostoTotal;

                        //var TP = (from tp in db.TanqueProductos
                        //          where tp.ProductoID.Equals(Producto.ID)
                        //          && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                        //          select tp).ToList();

                        ////-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                        ////-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                        //if (TP.Count() == 0)
                        //{
                        //    KX.CantidadInicial = 0;
                        //}
                        //else
                        //    KX.CantidadInicial = TP.Single(q => q.ProductoID.Equals(dk.ProductoID)).Cantidad;

                        KX.CantidadInicial = dk.CantidadEntrada;

                        KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                        if (KX.CantidadSalida > dk.CantidadInicial)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                            return false;
                        }

                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                        var Tanque = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(Producto.ID)
                                        && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                                      select tp).ToList();

                        if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                        {
                            Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
                            TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - KX.CantidadSalida;
                        }

                        db.SubmitChanges();

                        #endregion
                    }

                    #endregion

                    #region <<< ELIMINAR_KARDEX >>>

                    db.TemporalKardexes.Where(k => k.MovimientoTemporalID.Equals(M.ID)).ToList().ForEach(db.TemporalKardexes.DeleteOnSubmit);
                    db.SubmitChanges();

                    #endregion

                    #region <<< REGISTRANDO COMPROBANTE >>>

                    var Compronbante = db.TemporalComprobanteContables.Where(c => c.MovimientoTemporalID.Equals(M.ID));

                    Compronbante.ToList().ForEach(linea =>
                    {
                        M.ComprobanteContables.Add(new Entidad.ComprobanteContable
                        { 
                            CentroCostoID = linea.CentroCostoID,
                            CuentaContableID = linea.CuentaContableID,
                            Descripcion = linea.Descripcion,
                            EstacionServicioID = linea.EstacionServicioID,
                            Fecha = linea.Fecha,
                            Linea = linea.Linea,
                            Monto = linea.Monto,
                            TipoCambio = linea.TipoCambio,
                            MontoMonedaSecundaria = linea.MontoMonedaSecundaria,
                            SubEstacionID = linea.SubEstacionID
                        });
                        db.SubmitChanges();
                    });

                    db.SubmitChanges();

                    #endregion

                    #region <<< ELIMINAR_COMPROBANTE >>>

                    db.TemporalComprobanteContables.Where(c => c.MovimientoTemporalID.Equals(M.ID)).ToList().ForEach(db.TemporalComprobanteContables.DeleteOnSubmit);
                    db.SubmitChanges();

                    #endregion

                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
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
                if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "No esta aprobado / finalizado.", Parametros.MsgType.warning);
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
                    nf = new Forms.Dialogs.DialogAutoconsumo(Usuario, false);
                    nf.Text = "Crear Salida de Combustible";
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
                //if (nf == null)
                //{
                //    if (gvData.FocusedRowHandle >= 0)
                //    {
                //        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(25))
                //        {
                //            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")).Equals(true))
                //                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + "La sailda de combustible ya fue aprobada.", Parametros.MsgType.warning);
                //            else
                //            {
                //                if (Parametros.General.DialogMsg("¿Esta seguro de aprobar el registro?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                //                {
                //                    AprobarMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                //                    FillControl(true);
                //                    gvData_FocusedRowChanged(null, null);
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //    nf.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        //Anular Registro
        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(26))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")))
                                AnularMovimiento(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            else
                                AnularMovimientoTemporal(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));

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
                    if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) && Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(25))
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = "Sal Comb " + M.Numero.ToString("000000000");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridSalidaCombustible xtra = new Parametros.MyXtraGridSalidaCombustible();

                        if (M.Finalizado)
                        {
                            xtra.gridDetalle.DataSource = (from k in M.VistaKardexes
                                                           select new
                                                           {
                                                               k.ProductoCodigo,
                                                               k.ProductoNombre,
                                                               k.UnidadMedidaNombre,
                                                               Almacen = k.AlmacenSalidaNombre,
                                                               Cantidad = k.CantidadSalida,
                                                               Costo = k.CostoSalida,
                                                               Total = k.CostoTotal
                                                           }).ToList();

                            xtra.gridComprobante.DataSource = M.VistaComprobantes;
                        }
                        else if (!M.Finalizado)
                        {
                            xtra.gridDetalle.DataSource = (from k in db.TemporalKardexes
                                                           join m in db.Movimientos on k.MovimientoTemporalID equals m.ID
                                                           join p in db.Productos on k.ProductoID equals p.ID
                                                           join um in db.UnidadMedidas on k.UnidadMedidaID equals um.ID
                                                           join t in db.Tanques on k.AlmacenSalidaID equals t.ID
                                                           select new
                                                           {
                                                               ProductoCodigo = p.Codigo,
                                                               ProductoNombre = p.Nombre,
                                                               UnidadMedidaNombre = um.Nombre,
                                                               Almacen = t.Nombre,
                                                               Cantidad = k.CantidadSalida,
                                                               Costo = k.CostoSalida,
                                                               Total = k.CostoTotal
                                                           }).ToList();

                            xtra.gridComprobante.DataSource = dv.VistaComprobantesTemporales.Where(vc => vc.MovimientoTemporalID.Equals(M.ID));
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
        
        private void gvData_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                //if (gvData.FocusedRowHandle >= 0)
                //{
                //    if (btnAprobar)
                //    {
                //        if (gvData.GetFocusedRowCellValue(colTipoMovimiento).GetType() != typeof(DevExpress.Data.NotLoadedObject))
                //        {
                //            if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(25))
                //            {
                //                if (gvData.GetFocusedRowCellValue("Finalizado").GetType() != typeof(DevExpress.Data.NotLoadedObject))
                //                {
                //                    if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")))
                //                    {
                //                        this.btnModificar.Enabled = true;
                //                        this.btnModificar.Visibility = BarItemVisibility.Always;
                //                    }
                //                    else if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Finalizado")))
                //                    {
                //                        this.btnModificar.Enabled = false;
                //                        this.btnModificar.Visibility = BarItemVisibility.Never;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                this.btnModificar.Enabled = false;
                //                this.btnModificar.Visibility = BarItemVisibility.Never;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

    }
}
