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

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormPagoManual : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPagoManual nf;
        private Forms.Dialogs.DialogChequeMasivo nfm;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();  
        private List<int> lista;
        private List<int> paginas = new List<int>();
        private bool _FinalizarCheque;
        private DevExpress.XtraBars.Bar barItems;
        DevExpress.XtraBars.BarButtonItem btnMasivo = new DevExpress.XtraBars.BarButtonItem();
        
        #endregion

        #region <<< INICIO >>>

        public FormPagoManual()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear Pago";
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaMovimiento);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
            this.barTop.DockCol = 1;
            this.barTop.OptionsBar.UseWholeRow = false;
            barItems = new DevExpress.XtraBars.Bar(barManager);
            barItems.OptionsBar.AllowQuickCustomization = false;
            barItems.OptionsBar.DrawDragBorder = false;
            barItems.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
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

                    _FinalizarCheque = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnImprimirCheque");

                    if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "barPagosMasivos"))
                    {
                        DevExpress.XtraBars.BarButtonItem btnMasivo = new DevExpress.XtraBars.BarButtonItem();
                        btnMasivo.Caption = "Liq. Masivo";
                        btnMasivo.Glyph = Properties.Resources.addFolder24;
                        btnMasivo.Id = 1;
                        btnMasivo.Name = "btnMasivo";
                        btnMasivo.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnMasivo.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMasivo_ItemClick);
                        barItems.AddItem(btnMasivo);
                    }

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
                var query = from iv in dv.VistaMovimientos.Where(v => (v.MovimientoTipoID.Equals(55) || v.MovimientoTipoID.Equals(56) || v.MovimientoTipoID.Equals(61) || v.MovimientoTipoID.Equals(62) || v.MovimientoTipoID.Equals(73) || v.MovimientoTipoID.Equals(74)))
                            where lista.Contains(iv.EstacionServicioID) && !iv.Finalizado
                            select new
                            {
                                iv.ID,
                                iv.Referencia,
                                iv.EstacionNombre,
                                iv.EstacionServicioID,
                                iv.SubEstacionNombre,
                                iv.FechaContabilizacion,
                                iv.Comentario,
                                iv.MovimientoTipoNombre,
                                Beneficiario = (!iv.ProveedorID.Equals(0) ? iv.ProveedorCodigo + " | " + iv.ProveedorNombre : iv.EmpleadoCodigo + " | " + iv.EmpleadoNombres + " " + iv.EmpleadoApellidos),
                                iv.Anulado,
                                iv.MovimientoTipoID,
                                Year = iv.FechaContabilizacion.Year,
                                Mes = iv.FechaContabilizacion.Month,
                                Monto = iv.Monto,
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
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero;

                Reportes.Tesoreria.Hojas.RptComprobanteCheque rep = new Reportes.Tesoreria.Hojas.RptComprobanteCheque();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.xrTableCellTitulo.Text = "Comprobante " + VM.First().MovimientoTipoNombre + " " + VM.First().Referencia;
                rep.ceCantidad.Text = Parametros.General.enletras(VM.First().Monto.ToString());
                rep.xrTableBanco.Visible = false; 
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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro, int ID, bool Refresh, bool Masivo)
        {
            if (Masivo)
                nfm = null;
            else
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
            {
                if (Masivo)
                    btnMasivo_ItemClick(null, null);
                else
                    Add();
            }

            if (!ID.Equals(0))
                ImprimirComprobante(ID, true);
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        private void btnMasivo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (nfm == null)
                {
                    nfm = new Forms.Dialogs.DialogChequeMasivo(Usuario, false, true);
                    nfm.Text = "Crear Cheque Masivo";
                    nfm.Owner = this.Owner;
                    nfm.MdiParent = this.MdiParent;
                    nfm.MDIManual = this;
                    nfm.Show();
                }
                else
                    nfm.Activate();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogPagoManual(Usuario);
                    nf.Text = "Crear Pago Manual";
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

                //        if (!Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID")).Equals(Parametros.General.EstacionServicioID))
                //            Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + " El Cheque no pertenece a la Estación de Servicio seleccionada, no se puede editar.", Parametros.MsgType.warning);
                //        else
                //        {
                //            if (!paginas.Contains(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))))
                //            {
                //                if (Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                //                {
                //                    nf = new Forms.Dialogs.DialogPagoManual(Usuario);
                //                    nf.Text = "Editar Pago Manual";
                //                    nf.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                //                    nf.Owner = this.Owner;
                //                    nf.MdiParent = this.MdiParent;
                //                    nf.MDI = this;
                //                    nf.Show();
                //                }
                //                else
                //                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                //            }
                //            else
                //            {
                //                Parametros.General.DialogMsg("Debe de cerrar el detallte del Cheque Número " + Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + ", para poder ser editado.", Parametros.MsgType.warning);
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

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(56) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(62) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(74))
                        Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                    else
                    {
                        if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFechaCreado)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(ColEstacionID))))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                            {
                                if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(73))
                                {
                                    AnularMasivo(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
                                    FillControl(true);
                                }
                                else
                                {
                                    AnularProvision(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)));
                                    FillControl(true);
                                }
                            }
                        }
                    }
             }
        }

        //Anular el movimiento Pago
        private void AnularProvision(int ID, int Tipo)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    if (Tipo.Equals(55))
                    {
                        #region <<< ANULACIÓN PAGO MANUAL >>>

                        Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = 56;
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
                        M.Referencia = "ANULADO   " + Manterior.Referencia;
                        M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                                    "Se anuló el Pago Manual: " + M.Referencia, this.Name);

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

                        Entidad.Deudor D = db.Deudors.Where(d => d.ProveedorID.Equals(M.ProveedorID) && d.MovimientoID.Equals(ID)).FirstOrDefault();

                        if (D != null)
                        {

                            D.Pagos.ToList().ForEach(det =>
                            {
                                Entidad.Movimiento MD = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                                MD.Abono -= det.Monto;
                                MD.Pagado = false;
                                if (det.PagoIVA)
                                    MD.IVAPagado = false;

                                db.SubmitChanges();
                            });

                            D.Pagos.Clear();
                            db.Deudors.DeleteOnSubmit(D);
                            db.SubmitChanges();
                        }

                        #endregion

                        db.Retenciones.DeleteAllOnSubmit(db.Retenciones.Where(o => o.MovimientoID.Equals(ID)));
                        db.SubmitChanges();
                        #endregion
                    }
                    else if (Tipo.Equals(61))
                    {
                        #region <<< ANULACIÓN LIQUIDACION ANTICIPO >>>

                        Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));


                        Entidad.Movimiento ND = db.Movimientos.SingleOrDefault(s => s.MovimientoReferenciaID.Equals(ID) && s.MovimientoTipoID.Equals(27));

                        if (ND != null)
                        {
                            if (ND.Abono > 0)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("No se puede anular la Liquidación de Anticipos " + Manterior.Referencia + ", la Nota de Débito: " + ND.Numero + ", ya esta pagado / abonada.", Parametros.MsgType.warning);
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
                        M.MovimientoTipoID = 62;
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
                        M.Referencia = "ANULADO   " + Manterior.Referencia;
                        M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                                    "Se anuló la liquidación de Anticipo: " + M.Referencia, this.Name);

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

                        Entidad.Deudor D = db.Deudors.SingleOrDefault(d => d.MovimientoID.Equals(ID));

                        if (D != null)
                        {
                            D.Pagos.ToList().ForEach(det =>
                            {
                                Entidad.Movimiento MD = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                                MD.Abono -= det.Monto;
                                MD.Pagado = false;
                                if (det.PagoIVA)
                                    MD.IVAPagado = false;

                                db.SubmitChanges();
                            });

                            D.Pagos.Clear();
                            db.Deudors.DeleteOnSubmit(D);
                            db.SubmitChanges();
                        }

                        //RETENCIONES
                        db.Retenciones.DeleteAllOnSubmit(db.Retenciones.Where(o => o.MovimientoID.Equals(ID)));
                        db.SubmitChanges();
                        #endregion

                        #region <<< ANULACIÓN NOTA DEBITO
                        if (ND != null)
                        {
                            ND.Anulado = true;
                            ND.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                            ND.UsuarioAnuladoID = Parametros.General.UserID;
                            db.SubmitChanges();

                            Entidad.Movimiento NDAnulada = new Entidad.Movimiento();

                            NDAnulada.EstacionServicioID = ND.EstacionServicioID;
                            NDAnulada.SubEstacionID = ND.SubEstacionID;
                            NDAnulada.MovimientoTipoID = 28;
                            NDAnulada.ClienteID = ND.ClienteID;
                            NDAnulada.EmpleadoID = ND.EmpleadoID;
                            NDAnulada.UsuarioID = Parametros.General.UserID;
                            NDAnulada.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                            NDAnulada.FechaRegistro = ND.FechaRegistro;
                            NDAnulada.Monto = ND.Monto;
                            NDAnulada.MonedaID = ND.MonedaID;
                            NDAnulada.MontoMonedaSecundaria = ND.MontoMonedaSecundaria;
                            NDAnulada.TipoCambio = ND.TipoCambio;
                            NDAnulada.Referencia = "ANULADO   " + ND.Referencia.ToString();
                            db.Movimientos.InsertOnSubmit(NDAnulada);

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                                "Se anuló la Nota de : Débito" + ND.Referencia, this.Name);

                        }
                        #endregion

                        #endregion
                    }

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

        //Anular el movimiento Masivo
        private void AnularMasivo(int ID, int Tipo)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                        #region <<< ANULACIÓN PAGO MASIVO MANUAL >>>

                        Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                        Manterior.Anulado = true;
                        Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                        Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                        db.SubmitChanges();

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.EstacionServicioID = Manterior.EstacionServicioID;
                        M.SubEstacionID = Manterior.SubEstacionID;
                        M.MovimientoTipoID = 74;
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
                        M.Referencia = "ANULADO   " + Manterior.Referencia;
                        M.ProveedorReferenciaID = Manterior.ProveedorReferenciaID;
                        db.Movimientos.InsertOnSubmit(M);

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                                    "Se anuló el Pago Masivo Manual: " + M.Referencia, this.Name);

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

                        Entidad.Deudor D = db.Deudors.Where(d => d.ProveedorID.Equals(M.ProveedorID) && d.MovimientoID.Equals(ID)).FirstOrDefault();

                        if (D != null)
                        {

                            D.Pagos.ToList().ForEach(det =>
                            {
                                Entidad.Movimiento MD = db.Movimientos.Single(m => m.ID.Equals(det.MovimientoPagoID));
                                MD.Abono -= det.Monto;
                                MD.Pagado = false;
                                if (det.PagoIVA)
                                    MD.IVAPagado = false;

                                db.SubmitChanges();
                            });

                            D.Pagos.Clear();
                            db.Deudors.DeleteOnSubmit(D);
                            db.SubmitChanges();
                        }

                        db.Retenciones.DeleteAllOnSubmit(db.Retenciones.Where(o => o.MovimientoID.Equals(ID)));
                        db.SubmitChanges();
                        #endregion


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
        //Formulario Vista Previa
        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    var M = dv.VistaMovimientos.Single(o => o.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                    DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                    page.Text = M.Abreviatura + " " + (String.IsNullOrEmpty(M.Referencia) ? M.Numero.ToString() : M.Referencia);
                    page.Tag = M.ID;

                    if (paginas.Contains(Convert.ToInt32(page.Tag)))
                    {
                        this.xtraTabControlMain.SelectedTabPage = this.xtraTabControlMain.TabPages.Where(p => Convert.ToInt32(p.Tag).Equals(Convert.ToInt32(page.Tag))).First();
                    }
                    else
                    {

                    Parametros.MyXtraGridCheque xtra = new Parametros.MyXtraGridCheque(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));

                    xtra.gridDetalle.DataSource = from p in db.Pagos
                                                  join d in db.Deudors on p.DeudorID equals d.ID
                                                  join m in db.Movimientos on p.MovimientoPagoID equals m.ID
                                                  join mp in db.Movimientos on p.MovimientoPagoID equals mp.ID
                                                  join es in db.EstacionServicios on mp.EstacionServicioID equals es.ID
                                                  join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                  where d.MovimientoID.Equals(M.ID)
                                                  select new
                                                  {
                                                      d.ID,
                                                      mt.Abreviatura,
                                                      Referencia = (m.Numero.Equals(0) ? m.Referencia : m.Numero.ToString()),
                                                      m.FechaRegistro,
                                                      Estacion = es.Nombre,
                                                      Valor = m.Monto,
                                                      p.Monto
                                                  };

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
        
        //Evitar doble click para expandir o contraer grupo
        private void gvData_GroupRowCollapsing(object sender, RowAllowEventArgs e)
        {
            e.Allow = false;
        }
        
        #endregion
    }
}
