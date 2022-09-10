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

namespace SAGAS.Contabilidad.Forms
{                                
    public partial class FormProvisiones : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogProvisiones nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private List<int> lista;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();

        #endregion

        #region <<< INICIO >>>

        public FormProvisiones()
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

                    lkMes.DataSource = listadoMes.GetListMeses();

                    DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaRegistro") >= fecha).ToString();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, DevExpress.Data.Linq.GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                var query = from v in dv.VistaMovimientos
                                           where lista.Contains(v.EstacionServicioID) && (v.MovimientoTipoID.Equals(5) || v.MovimientoTipoID.Equals(6))
                                           select new
                                           {
                                               v.ID,
                                               Proveedor = v.ProveedorNombre,
                                               FechaRegistro = v.FechaContabilizacion,
                                               Year = v.FechaContabilizacion.Year,
                                               Mes = v.FechaContabilizacion.Month,
                                               Monto = (v.MonedaID.Equals(MonedaID) ? v.Monto : v.MontoMonedaSecundaria),
                                               v.Referencia,
                                               v.Anulado,
                                               Moneda = v.MonedaSimbolo + " | " + v.MonedaNombre,
                                               v.EstacionServicioID,
                                               ES = v.EstacionNombre,
                                               SUS = v.SubEstacionNombre,
                                               v.MovimientoTipoID,
                                               NombreMovimiento = v.MovimientoTipoNombre,
                                               v.Comentario,
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
                rv.Text = "Provisión " + VM.First().Referencia;

                Reportes.Contabilidad.Hojas.RptComprobanteProvision rep = new Reportes.Contabilidad.Hojas.RptComprobanteProvision();
                
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);

                rep.CeTitulo.Text = (VM.First().MovimientoTipoID.Equals(5) ? "Comprobante Provision de Proveedores" : "Anulación Provision de Proveedores");

                rep.DataSource = VM;

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
                FillControl();

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
                    nf = new Forms.Dialogs.DialogProvisiones(Usuario, false);
                    nf.Text = "Crear Nueva Provisión de Proveedores";
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

                    if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Manterior.ID)) > 0)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + Manterior.Referencia + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
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
                    M.MovimientoTipoID = 6;
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

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                                "Se anuló la Provisión: " + M.Referencia, this.Name);

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
                    FillControl();
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
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colAnulado)) || Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(6))
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
                        if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(5))
                        {
                            nf = new Forms.Dialogs.DialogProvisiones(Usuario, true);
                            nf.Text = "Vista Previa Provisión de Gastos";
                            nf.EntidadAnterior = db.Movimientos.Single(m => m.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " +
                                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
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
