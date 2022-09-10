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
    public partial class FormBajaBien : Parametros.Forms.FormBase
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


        public FormBajaBien()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Reversión de Baja";
            this.barImprimir.Caption = "Vista Previa";
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaBienes);
            this.btnAgregar.Visibility = this.btnModificar.Visibility = BarItemVisibility.Never;
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
                            //join vb in dv.VistaMovimientos on v.MovimientoAltaID equals vb.ID
                            where lista.Contains(v.EstacionID) && !v.Activo
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
                                MovimientoID = v.MovimientoBajaID
                                ,v.EsDepreciado
                                ,Fecha = v.FechaLiquidacion
                                ,Mes = (v.FechaLiquidacion.HasValue ? v.FechaLiquidacion.Value.Month : 0)
       ,v.Activo                         
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
                if (Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID)).Equals(0))
                    Parametros.General.DialogMsg("La Baja no generó Comprobante Contable.", Parametros.MsgType.warning);
                else
                    ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID)));
            }
        }

        private void ImprimirComprobante(int ID, int MovID)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaMovimientos.Where(m => m.ID.Equals(MovID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString();

                Reportes.ActivoFijo.Hojas.RptComprobanteBaja rep = new Reportes.ActivoFijo.Hojas.RptComprobanteBaja();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.CeTitulo.Text = VM.First().MovimientoTipoNombre + "  " + VM.First().Numero.ToString();

                var T = dbView.VistaBienes.SingleOrDefault(s => s.ID.Equals(ID));

                rep.xrCeCodigo.Text = T.Codigo.ToString();
                rep.xrCeNombre.Text = T.Nombre;
                rep.xrCeUsuarioAnterior.Text = T.UsuarioAsignado;
                rep.xrCeAreaAnterior.Text = T.FechaAdquisicion.ToShortDateString();
                rep.xrCeEstacionAnterior.Text = (String.IsNullOrEmpty(T.SubEstacionNombre) ? T.EstacionNombre : T.SubEstacionNombre);

                rep.xrCeFecha.Text = VM.First().FechaContabilizacion.ToShortDateString();

                rep.DataSource = VM;

                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument(true);

                rv.Show();


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

        protected override void Del()
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    int _MovID = 0;

                    if (gvData.GetFocusedRowCellValue(colMovimientoID) != null)
                        _MovID = Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID));

                    if (_MovID > 0)
                    {
                        
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        var vM = db.Movimientos.Single(b => b.ID.Equals(_MovID));

                        DateTime vFechaV = new DateTime(vM.FechaRegistro.Date.Year, vM.FechaRegistro.Date.Month, 1).AddMonths(1).AddDays(-1);

                        DateTime FechaAnterior = new DateTime(vFechaV.Year, vFechaV.Month, 01);

                        if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(vM.EstacionServicioID) && (m.FechaRegistro.Date > FechaAnterior.Date && m.FechaRegistro.Date <= vFechaV.Date)) >= 1)
                        {
                            Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo", Parametros.MsgType.warning);
                            return;
                        }

                        if (!Parametros.General.ValidatePeriodoContable(vM.FechaRegistro.Date, db, vM.EstacionServicioID))
                            Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                            {
                                AnularBaja(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), _MovID);
                                FillControl(true);
                            }
                        }
                    }
                    else
                        Parametros.General.DialogMsg("El Bien fue Anulado, no se puede revertir.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Anular la compra
        private void AnularBaja(int ID, int MovID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                    Entidad.Bien EB = db.Bien.Single(b => b.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));

                    EB.Activo = true;
                    EB.FechaLiquidacion = null;

                    db.SubmitChanges();

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(MovID));

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 79;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    db.Movimientos.InsertOnSubmit(M);

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

                    EB.MovimientoBajaID = 0;


                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                        "Se ha revertido la Baja del Bien : " + EB.Codigo + " | " + EB.Nombre, this.Name);

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

        private void gvData_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    int _id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colMovimientoID));

                    if (_id > 0)
                    {
                        var M = dv.VistaMovimientos.Single(o => o.ID.Equals(_id));
                        DevExpress.XtraTab.XtraTabPage page = new DevExpress.XtraTab.XtraTabPage();
                        page.Text = M.Abreviatura + " " + M.Numero.ToString("");
                        page.Tag = M.ID;

                        Parametros.MyXtraGridCD xtra = new Parametros.MyXtraGridCD();

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
