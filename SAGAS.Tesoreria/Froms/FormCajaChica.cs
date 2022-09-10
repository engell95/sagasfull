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
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Tesoreria.Forms
{                                
    public partial class FormCajaChica : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogCajaChica nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _ConeptoCajaChica = Parametros.Config.ConceptoCajaChicaID();

        #endregion

        #region <<< INICIO >>>

        public FormCajaChica()
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
                var query = from iv in dv.VistaMovimientos.Where(v => (v.MovimientoTipoID.Equals(41) || v.MovimientoTipoID.Equals(42) || (v.MovimientoTipoID.Equals(3) && !v.Credito) || (v.MovimientoTipoID.Equals(82) && v.ConceptoContableID.Equals(_ConeptoCajaChica))))
                            where lista.Contains(iv.EstacionServicioID)// && !iv.Finalizado
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.Entregado,
                                iv.Referencia,
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                Factura = (iv.MovimientoTipoID.Equals(3) ? iv.Referencia : "N/A"),
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Responsable = iv.ProveedorNombre,
                                Beneficiario = ((iv.PreveedorReferencia.Equals("") || iv.PreveedorReferencia == null) ? "Empleado" : iv.PreveedorReferencia.ToString()),//iv.PreveedorReferencia : "Empleado"),
                                RucProveedor = ((iv.PreveedorReferencia.Equals("") || iv.PreveedorReferencia == null) ? "N/A" : iv.PreveedorReferenciaRUC.ToString()),
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = iv.Monto,
                                iv.Pagado
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

                Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica rep = new Reportes.Tesoreria.Hojas.RptCoprobanteCajaChica();
                
                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = rep.CeEmpresa2.Text = Nombre;
                rep.CeRuc.Text = Ruc;
                rep.xrTcProveedor.Text = VM.First().PreveedorReferencia;

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
            {
                if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(42))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "Este documento no se puede visualizar.", Parametros.MsgType.warning);
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
                    nf = new Forms.Dialogs.DialogCajaChica(Usuario);
                    nf.Text = "Nueva Doc Caja Chica";
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
        private void AnularProvision(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    if (Manterior.Pagado)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", ya esta pagado.", Parametros.MsgType.warning);
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

                    var Cargo = db.Movimientos.FirstOrDefault(m => m.MovimientoReferenciaID.Equals(Manterior.ID));
                    
                    if (Cargo != null)
                    {
                        if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Cargo.ID)) > 0)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento Cargo a Empleado : " + (Cargo.Numero.Equals(0) ? Cargo.Referencia : Cargo.Numero.ToString()) + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        } 
                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 42;
                    M.ProveedorID = Manterior.ProveedorID;
                    M.EmpleadoID = Manterior.EmpleadoID;
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
                    M.Referencia = "ANULADO   " + Manterior.Numero;
                    M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                                "Se anuló la Caja Chica : " + M.Referencia, this.Name);

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
                                        
                    Entidad.Deudor DE = db.Deudors.Where(d => d.EmpleadoID.Equals(M.EmpleadoID) && d.Valor.Equals(M.Monto) && d.MovimientoID.Equals(ID)).FirstOrDefault();

                    if (DE != null)
                    {
                        db.Deudors.DeleteOnSubmit(DE);
                        db.SubmitChanges();
                    }

                    Entidad.Movimiento MCargo = db.Movimientos.FirstOrDefault(m => m.MovimientoReferenciaID.Equals(Manterior.ID));

                    if (MCargo != null)
                    {
                        MCargo.Anulado = true;
                        MCargo.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        MCargo.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();
                    }

                    #endregion
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (!Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(3))
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(42))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionServicioID))))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                            {
                                AnularProvision(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                                FillControl(true);
                            }
                        }
                    }
                }
                else
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + "Es un documento de venta, debe ser anulado en la lista del modulo de Compras", Parametros.MsgType.warning);
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
                    page.Text = M.Abreviatura + " " + M.Numero.ToString();
                    page.Tag = M.ID;

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {

                        Parametros.MyXtraGridCajaChica xtra = new Parametros.MyXtraGridCajaChica();

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
              
        #endregion   

    }
}
