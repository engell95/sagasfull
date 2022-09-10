using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.XtraReports.UI;

namespace SAGAS.Ventas.Forms
{                                
    public partial class FormNotasDC : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogNotasDC nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private int _ClienteAnticipo;
        
        #endregion

        #region <<< INICIO >>>

        public FormNotasDC()
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
                    _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                    rpMes.DataSource = listadoMes.GetListMeses();

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
                var query = from iv in dv.VistaMovimientos.Where(v => (v.MovimientoTipoID.Equals(27) || v.MovimientoTipoID.Equals(29) || v.MovimientoTipoID.Equals(28) || v.MovimientoTipoID.Equals(30)) && v.ResumenDiaID.Equals(0))
                            where lista.Contains(iv.EstacionServicioID)
                            select new
                            {
                                iv.ID,
                                iv.Numero,
                                iv.Referencia,
                                iv.EstacionServicioID,
                                iv.EstacionNombre,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Cliente = (iv.ClienteID.Equals(0) ? iv.EmpleadoNombres + " " + iv.EmpleadoApellidos : iv.ClienteCodigo + " | " + iv.ClienteNombre),
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = iv.Monto,
                                iv.Pagado,
                                Cupones = (iv.VistaNotaCreditoCupones.Count > 0 ? true : false)
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

        private void ImprimirComprobante(int ID, bool ToPrint, bool HasCD)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");

                Reportes.Ventas.Hojas.RptComprobanteNotaDC rep = new Reportes.Ventas.Hojas.RptComprobanteNotaDC();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeRuc.Text = Ruc;
                rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString("000000000");
                rep.ceCantidad.Text = Parametros.General.enletras(VM.First().Monto.ToString());
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

                rep.DataSource = VM;
                if (VM.First().VistaNotaCreditoCupones.Count <= 0)
                    rep.DetailNCC.Visible = false;

                if (!HasCD)
                    rep.DetailCD.Visible = false;

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

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {

                DialogResult resultado;

                resultado = Parametros.General.DialogMsg("¿Desea mostrar el comprobante?", Parametros.MsgType.questionNO);

                if (resultado == DialogResult.OK)
                    ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false, true);
                else if (resultado == DialogResult.No)
                    ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), false, false);

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, bool Refresh, int ID)
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
            {
                if (Parametros.General.DialogMsg("¿Desea Imprimir el documento?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    ImprimirComprobante(ID, true, true);
                    ImprimirComprobante(ID, true, false);
                }
            }
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
                    nf = new Forms.Dialogs.DialogNotasDC(Usuario);
                    nf.Text = "Nueva Nota";
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

                    if (Manterior.MovimientoTipoID.Equals(27))
                    {
                        if (Manterior.MovimientoReferenciaID > 0)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("La Nota de Debito no se puede anular, esta referenciada a un Comprobante de Arqueo.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return;
                        }

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

                        //if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Manterior.ID)) > 0)
                        //{
                        //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        //    Parametros.General.DialogMsg("El Documento: " + Manterior.Numero + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                        //    trans.Rollback();
                        //    return;
                        //}

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

                    }

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    Manterior.NotaCreditoCupones.Clear();
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = (Manterior.MovimientoTipoID.Equals(27) ? 28 : 30);
                    M.ClienteID = Manterior.ClienteID;
                    M.EmpleadoID = Manterior.EmpleadoID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se anuló la Nota de : " + (Manterior.MovimientoTipoID.Equals(27) ? "Débito" : "Crédito") + Manterior.Numero.ToString("000000000"), this.Name);

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

                    Entidad.Deudor D;

                    if (M.MovimientoTipoID.Equals(28))
                        D = db.Deudors.Where(d => d.ClienteID.Equals(M.ClienteID) && d.EmpleadoID.Equals(M.EmpleadoID) && d.Valor.Equals(M.Monto) && d.MovimientoID.Equals(ID)).FirstOrDefault();
                    else if (M.MovimientoTipoID.Equals(30))
                        D = db.Deudors.Where(d => d.ClienteID.Equals(M.ClienteID) && d.EmpleadoID.Equals(M.EmpleadoID) && d.Valor.Equals(-Math.Abs(M.Monto)) && d.MovimientoID.Equals(ID)).FirstOrDefault();
                    else
                        D = null;

                    if (D != null)
                    {
                        D.Pagos.ToList().ForEach(det =>
                        {
                            Entidad.Movimiento MDRemove = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                            MDRemove.Abono -= det.Monto;
                            MDRemove.Pagado = false;
                            db.SubmitChanges();
                        });

                        D.Pagos.Clear();
                        db.Deudors.DeleteOnSubmit(D);
                        db.SubmitChanges();
                    }
                    #endregion

                    //NOTA POR CUPONES
                    if (db.Movimientos.Count(m => m.MovimientoReferenciaID.Equals(Manterior.ID)) > 0)
                    {
                        Entidad.Movimiento MDebito = db.Movimientos.Single(m => m.MovimientoReferenciaID.Equals(Manterior.ID));

                        Entidad.Deudor Debito;

                        Debito = db.Deudors.Where(d => d.ClienteID.Equals(MDebito.ClienteID) && d.EmpleadoID.Equals(MDebito.EmpleadoID) && d.Valor.Equals(MDebito.Monto) && d.MovimientoID.Equals(MDebito.ID)).FirstOrDefault();

                        if (Debito != null)
                        {
                            Debito.Pagos.ToList().ForEach(det =>
                            {
                                Entidad.Movimiento MDRemove = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                                MDRemove.Abono -= det.Monto;
                                MDRemove.Pagado = false;
                                db.SubmitChanges();
                            });

                            Debito.Pagos.Clear();
                            db.Deudors.DeleteOnSubmit(Debito);
                            db.SubmitChanges();
                        }

                        db.Movimientos.DeleteOnSubmit(MDebito);
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

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(28) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(30)))
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
        }

        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(28) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(30)))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("000000000");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridNotasDC xtra = new Parametros.MyXtraGridNotasDC();

                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(29))
                        {
                            xtra.gridDetalle.DataSource = from p in db.Pagos
                                                          join d in db.Deudors on p.DeudorID equals d.ID
                                                          join m in db.Movimientos on p.MovimientoPagoID equals m.ID
                                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                          where d.MovimientoID.Equals(M.ID)
                                                          select new
                                                          {
                                                              d.ID,
                                                              mt.Abreviatura,
                                                              m.FechaRegistro,
                                                              Numero = (m.Numero.Equals(0) ? m.Referencia : m.Numero.ToString()),
                                                              Valor = m.Monto,
                                                              p.Monto
                                                          };

                            xtra.splitContainerControlMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Both;
                        }

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
