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
    public partial class FormEntradaManejo : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogEntradaManejo nf;
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

        public FormEntradaManejo()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
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
                var query = from iv in dv.VistaMovimientos.Where(v => v.MovimientoTipoID.Equals(18) || v.MovimientoTipoID.Equals(20))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Referencia,
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                Year = (iv.FechaFisico.HasValue ? Convert.ToDateTime(iv.FechaFisico).Year : Convert.ToDateTime(iv.FechaContabilizacion).Year),
                                Mes = (iv.FechaFisico.HasValue ? Convert.ToDateTime(iv.FechaFisico).Month : Convert.ToDateTime(iv.FechaContabilizacion).Month),
                                FechaRecibido = (iv.FechaFisico.HasValue ? String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(iv.FechaFisico).Date) : "N/A"),
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Cliente = iv.ClienteCodigo + " | " + iv.ClienteNombre,
                                iv.Anulado,
                                iv.MovimientoTipoID
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
                rv.Text = VM.First().MovimientoTipoNombre + " " + VM.First().Referencia;

                Reportes.Contabilidad.Hojas.RptEntradaSalidaManejo rep = new Reportes.Contabilidad.Hojas.RptEntradaSalidaManejo();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.DataSource = VM;
                
                //else if (VM.First().MovimientoTipoID.Equals(10))
                //{
                //    var vista = dbView.VistaMovimientos.Where(v => v.MovimientoReferenciaID.Equals(VM.First().ID));

                //    if (vista.Count() > 0)
                //    {//vista.First().VistaKardexes.First().CostoTotal
                //        rep.xrCellAlmacenTitle.Text = "Almacén Entrada";
                //        rep.DetailReport.DataSource = vista;
                //        rep.xrTableCell17.DataBindings.Add("Text", null, "VistaKardexes.AlmacenEntradaNombre");
                //        rep.xrTableCell18.DataBindings.Add("Text", null, "VistaKardexes.CantidadEntrada");
                //        rep.xrTableCell19.DataBindings.Add("Text", null, "VistaKardexes.CostoEntrada");    
                //        rep.xrCellValorSubtotal.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal"); 
                //        rep.xrTableCell4.DataBindings.Add("Text", null, "VistaKardexes.CostoTotal"); 
 
                //    }
                //}

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

                    #region <<< ANULACION_ENTRADA/SALIDA >>>

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 20;
                    M.ClienteID = Manterior.ClienteID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.Litros = Decimal.Round(Manterior.Litros, 3, MidpointRounding.AwayFromZero);
                    M.Referencia = "ANULADO   " + Manterior.Referencia.ToString();
                    M.AlmacenID = Manterior.AlmacenID;
                    M.Comentario = Manterior.Comentario;

                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                                "Se anuló la Entrada Manejo: " + Manterior.Referencia, this.Name);

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

                        int Alma = 0; //Anulacion de entrada
                        //*****REFLEJA SALIDA******//
                        Alma = dk.AlmacenEntradaID;
                        KX.AlmacenSalidaID = dk.AlmacenEntradaID;
                        KX.CantidadSalida = dk.CantidadEntrada;
                        KX.CantidadInicial = dk.CantidadFinal;
                        KX.CantidadFinal = dk.CantidadInicial;
                        KX.EsProducto = dk.EsProducto;

                        //*****VALIDA ANULACION CANTIDAD ENTRADA******//
                        
                        //decimal Valor = Parametros.General.SaldoKardexManejo(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, M.FechaRegistro, false) + Parametros.General.SaldoKardexManejoPost(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, KX.Fecha) - KX.CantidadSalida;
                        //if (Valor < 0)
                        //{
                        //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        //    trans.Rollback();
                        //    Parametros.General.DialogMsg("El producto : " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " no puede quedar en negativo " + Valor.ToString("#,0.000"), Parametros.MsgType.warning);
                        //    return;
                        //}



                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                        var AL = (from al in db.TanqueProductos
                                  where al.ProductoID.Equals(KX.ProductoID)
                                    && al.TanqueID.Equals(Alma)
                                  select al).ToList();

                        if (AL.Count() > 0)
                        {
                            Entidad.TanqueProducto TP = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(Alma));
                            TP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;

                            //if (Tipo.Equals(2))
                            //    AP.Cantidad = AL.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad + KX.CantidadEntrada;
                            //if (Tipo.Equals(1))

                        }


                        //------------------------------------------------------------------------//

                        #endregion


                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        #region ::: QUITANDO DEUDOR :::

                        Entidad.Deudor D = db.Deudors.Where(d => d.ClienteID.Equals(M.ClienteID) && d.Valor.Equals(KX.CantidadSalida) && d.ProductoID.Equals(KX.ProductoID) && d.MovimientoID.Equals(ID)).FirstOrDefault();
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
                    nf = new Forms.Dialogs.DialogEntradaManejo(Usuario, false);
                    nf.Text = "Nueva Entrada Manejo";
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
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                        {
                            if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Anulado")).Equals(true))
                                Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Referencia")) + Environment.NewLine + " El registro esta anulado, no se puede editar.", Parametros.MsgType.warning);
                            else
                            {
                                if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                                else
                                {
                                    nf = new Forms.Dialogs.DialogEntradaManejo(Usuario, true);
                                    nf.Text = "Editar Entrada Manejo";
                                    nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                    nf.Owner = this.Owner;
                                    nf.MdiParent = this.MdiParent;
                                    nf.MDI = this;
                                    nf.Show();
                                }
                            }
                        }
                        else
                            Parametros.General.DialogMsg("La Estación de Servicio Seleccionada no es la misma que la Estación de Servicio del registro.", Parametros.MsgType.warning);

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

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(20))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
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
                    page.Text = M.Abreviatura + " " + M.Referencia;
                    page.Tag = M.ID;

                    Parametros.MyXtraGridEntradaManejo xtra = new Parametros.MyXtraGridEntradaManejo();

                    if (M.MovimientoTipoID.Equals(18))
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " | " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenEntradaNombre, Cantidad = s.CantidadEntrada }).ToList();
                    }
                    else if (M.MovimientoTipoID.Equals(20))
                    {
                        xtra.gridDetalle.DataSource = M.VistaKardexes.Select(s => new { Producto = s.ProductoCodigo + " | " + s.ProductoNombre, s.UnidadMedidaNombre, Almacen = s.AlmacenSalidaNombre, Cantidad = s.CantidadSalida }).ToList();
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
