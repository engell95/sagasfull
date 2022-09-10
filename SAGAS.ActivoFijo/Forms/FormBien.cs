using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAGAS.Parametros;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;

namespace SAGAS.ActivoFijo.Forms
{
    public partial class FormBien : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogBien nfNew;
        private Forms.Dialogs.DialogBienEdit nfEdit;
        private Forms.Dialogs.DialogBienBaja nfBaja;
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


        public FormBien()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.btnAnular.Caption = "Baja";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaBienes);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }


        #region <<< METODOS >>>

        private void FormBien_Load(object sender, EventArgs e)
        {
            FillControl(false); 
        }

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
                    this.gvData.ActiveFilterString = (new OperandProperty("FechaAdquisicion") > fecha.AddDays(-1)).ToString();
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
                var query = from v in dv.VistaBienes
                            join vb in dv.VistaMovimientos on v.MovimientoAltaID equals vb.ID
                            where lista.Contains(v.EstacionID) && v.Activo
                            select new
                            {
                                v.ID
      ,
                                v.Codigo
      ,
                                v.NoFactura
      ,
                                v.Nombre
      ,
                                v.Descripcion
      ,
                                v.FechaAdquisicion
      ,
                                v.FechaLiquidacion
      ,
                                v.ValorAdquisicion
      ,
                                v.ValorDepreActual
      ,
                                v.VidaUtilMeses
      ,
                                v.MesesDepreciados
      ,
                                v.UsuarioAsignado
      ,
                                v.NoSerie
      ,
                                v.Marca
      ,
                                v.Modelo
      ,
                                v.Matricula
      ,
                                v.Chasis
      ,
                                v.Motor
      ,
                                v.CentroCosto
      ,
                                v.TipoActivoCodigo
      ,
                                v.TipoActivoNombre
      ,
                                v.TipoActivoActive
      ,
                                v.CuentaActivoCodigo
      ,
                                v.CuentaActivoNombre
      ,
                                v.ActivoCodigo
      ,
                                v.ActivoNombre
      ,
                                v.EstacionNombre
      ,
                                v.SubEstacionNombre
                          ,
                                vb.Referencia
                                ,vb.TipoMovimientoActivoNombre
                                ,vb.MovimientoTipoID
                                ,
                                MovimientoID = v.MovimientoAltaID
                                ,v.EsDepreciado
                                ,v.EstacionID
                                
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

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                var vM = db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colIDMovimiento))));
                if (vM.Anulado)
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue(vM.Referencia)) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                    ImprimirComprobante(vM.ID, false);
            }
        }

        private void ImprimirComprobante(int ID, bool ToPrint)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = (VM.First().VistaComprobantes.Count > 0 ? "Comprobante " : "") + VM.First().TipoMovimientoActivoNombre;

                Reportes.ActivoFijo.Hojas.RptComprobanteActivoFijo rep = new Reportes.ActivoFijo.Hojas.RptComprobanteActivoFijo();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.CeTitulo.Text = (VM.First().VistaComprobantes.Count > 0 ? "Comprobante " : "") + "Alta Activo Fijo";

                rep.DataSource = VM;

                if (VM.First().VistaComprobantes.Count <= 0)
                    rep.drComprobante.Visible = false;

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;

                if (ToPrint)
                {
                    rep.CreateDocument();
                    rep.Print();
                    rv.Close();
                }
                else
                {
                    rep.CreateDocument(true);
                    rv.Show();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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

        internal void CleanDialog(bool ShowMSG, bool Next, bool Refresh, bool _New)
        {
            if (_New)
                nfNew = null;
            else if (!_New)
                nfEdit = null;

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
                Add();

            this.Activate();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nfNew == null)
                {
                    nfNew = new Forms.Dialogs.DialogBien(Parametros.General.UserID);
                    nfNew.Text = "Crear Movimiento de Activo";
                    nfNew.Owner = this.Owner;
                    nfNew.MdiParent = this.MdiParent;
                    nfNew.MDI = this;
                    nfNew.Show();
                }
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
                DateTime vFecha = Convert.ToDateTime(db.GetDateServer());
                if (Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFecha)).Date < new DateTime(vFecha.Year, vFecha.Month, 1).Date)
                {
                    if (nfEdit == null)
                    {
                        if (gvData.FocusedRowHandle >= 0)
                        {
                            nfEdit = new Forms.Dialogs.DialogBienEdit(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nfEdit.Text = "Editar Bien";
                            nfEdit.Owner = this.Owner;
                            nfEdit.MdiParent = this.MdiParent;
                            nfEdit.Editable = true;
                            nfEdit.MDI = this;
                            nfEdit.Show();
                        }
                    }
                }
                else
                {
                    if (nfNew == null)
                    {
                        if (gvData.FocusedRowHandle >= 0)
                        {
                            if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue(colFecha)).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue(colEstacionID))))
                                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                            else
                            {
                                if (db.Pagos.Count(c => c.MovimientoPagoID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colIDMovimiento)))) > 0)
                                {
                                    Parametros.General.DialogMsg("El Documento: " + Convert.ToString(gvData.GetFocusedRowCellValue(colNumero)) + ", esta Pagado / Abonado." + Environment.NewLine + "Favor consultar los reportes.", Parametros.MsgType.warning);
                                }
                                else
                                {

                                    nfNew = new Forms.Dialogs.DialogBien(Parametros.General.UserID);
                                    nfNew.Text = "Editar Movimiento de Activo";
                                    nfNew.EntidadAnterior = db.Movimientos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colIDMovimiento)));
                                    nfNew.Owner = this.Owner;
                                    nfNew.MdiParent = this.MdiParent;
                                    nfNew.Editable = true;
                                    nfNew.MDI = this;
                                    nfNew.Show();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
             }
        }

        protected override void Del()
        {
            try
            {

                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg("¿Esta Seguro dar de Baja al Bien seleccionado?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {

                        if (nfEdit == null)
                        {
                            nfBaja = new Forms.Dialogs.DialogBienBaja(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                            nfBaja.Text = "Crear Baja";
                            nfBaja.Owner = this.Owner;
                            nfBaja.MdiParent = this.MdiParent;
                            nfBaja.Editable = true;
                            nfBaja.MDI = this;
                            nfBaja.Show();
                        }
                    }
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            var BD = db.Bien.Where(b => b.MovimientoAltaID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colIDMovimiento)))).Select(s => s.ID);

                            if (db.Depreciacions.Count(d => BD.Any(o => o.Equals(d.BienID))) > 0)
                            {
                                Parametros.General.DialogMsg("Los bienes que se anularán ya estan depreciados, no puede eliminar Bienes depreciados.", Parametros.MsgType.warning);
                            }                                
                            else
                            {
                                if (!Convert.ToBoolean(gvData.GetFocusedRowCellValue(colEsDepreciado)))
                                    Parametros.General.DialogMsg("Los bienes que se anularán ya estan depreciados, no puede eliminar Bienes depreciados.", Parametros.MsgType.warning);
                                else
                                {
                                    if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colTipoMovimiento)).Equals(66))
                                    {
                                        AnularAlta(Convert.ToInt32(gvData.GetFocusedRowCellValue(colIDMovimiento)));
                                        FillControl(true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Anular la compra
        private void AnularAlta(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {                    
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    DateTime vFecha = new DateTime(Manterior.FechaRegistro.Year, Manterior.FechaRegistro.Month, 1).AddMonths(1).AddDays(-1);

                    DateTime FechaAnterior = new DateTime(vFecha.Year, vFecha.Month, 01);

                    var query = db.PeriodoContables.FirstOrDefault(p => p.EstacionID.Equals(Manterior.EstacionServicioID) && (p.FechaInicio.Date <= vFecha.Date && p.FechaFin.Date >= vFecha.Date));


                    if (!Parametros.General.ValidatePeriodoContable(vFecha, db, Manterior.EstacionServicioID))
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Periodo contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        return;
                    }
                                        
                    if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(Manterior.EstacionServicioID) && (m.FechaRegistro.Date > FechaAnterior.Date && m.FechaRegistro.Date <= vFecha.Date)) >= 1)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo", Parametros.MsgType.warning);
                        return;
                    }

                    if (Manterior.Pagado)
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El Documento: " + Manterior.Referencia + ", ya esta pagado.", Parametros.MsgType.warning);
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


                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 67;
                    M.ProveedorID = Manterior.ProveedorID;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaFisico = Manterior.FechaFisico;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Referencia = "ANULADO   " + Manterior.Referencia;
                    M.TipoMovimientoActivoID = Manterior.TipoMovimientoActivoID;
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                                "Se anuló la Alta: " + M.Referencia, this.Name);

                    var BL = db.Bien.Where(b => b.MovimientoAltaID.Equals(Manterior.ID));
                    db.Bien.DeleteAllOnSubmit(BL);
                    db.SubmitChanges();

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

                    #region ::: QUITANDO DEUDOR :::

                    Entidad.Deudor D = db.Deudors.Where(d => d.MovimientoID.Equals(ID)).FirstOrDefault();

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

        #endregion
    
    }
}
