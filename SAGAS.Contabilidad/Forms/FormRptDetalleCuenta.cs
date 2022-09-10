using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;

namespace SAGAS.Contabilidad.Forms
{
    public partial class FormRptDetalleCuenta : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private int Usuario = Parametros.General.UserID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<Parametros.ListIdDisplayCodeBool> EtCuentas;
        private List<StEstaciones> EtEstaciones = new List<StEstaciones>();
        private List<StSubEstaciones> EtSubEstaciones = new List<StSubEstaciones>();
        private List<Entidad.VistaRptMovimientoDetalle> EtVista;
        private BackgroundWorker bgw;
        Reportes.Contabilidad.Hojas.RptDetalleCuentas rep;
        int AnioInicial = 0;
        int AnioFinal = 0;

        struct StEstaciones
        {
            public int ID;
            public string  Nombre;
            public bool Activo;
        };

        struct StSubEstaciones
        {
            public int ID;
            public string Nombre;
            public int EstacionServicioID;
            public bool Activo;
        };

        private int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSUS.EditValue); }
            set { lkSUS.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public FormRptDetalleCuenta()
        {
            InitializeComponent();    
        }

        private void FormRptDetalleCuenta_Shown(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                
                //Valores

                //Listas

                AnioInicial = DateTime.Now.Year;
                AnioFinal = DateTime.Now.Year;

               var Est = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                 select new StEstaciones 
                                 { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).ToList();

                var SubEst = db.SubEstacions.Where(ses => ses.Activo && Est.Select(es => es.ID).Contains(ses.EstacionServicioID)).Select(s => new StSubEstaciones { ID = s.ID, Nombre = s.Nombre, EstacionServicioID = s.EstacionServicioID, Activo = s.Activo }).ToList();
                
                Est.Insert(0, new StEstaciones { ID = 0, Nombre = "TODOS", Activo = true });

                if (SubEst.Count > 0)
                    SubEst.Insert(0, new StSubEstaciones { ID = 0, Nombre = "TODOS", EstacionServicioID = 0, Activo = true });

                EtEstaciones = Est.ToList();
                EtSubEstaciones = SubEst.ToList();
                EtCuentas = db.CuentaContables.Where(c => c.Activo && c.Detalle).Select(s => new Parametros.ListIdDisplayCodeBool { ID = s.ID, Codigo = s.Codigo, Display = s.Nombre, valor = s.Activo }).OrderBy(o => o.Codigo).ToList();
                bdsCuentas.DataSource = EtCuentas;
                gvDataCuentas.RefreshData();
                
                //LOOKUPS
                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre});
                IDEstacion = Parametros.General.EstacionServicioID;

                this.lkProdDesde.Properties.DataSource = EtCuentas.Select(s => new { s.Codigo, Display = s.Codigo + " | " + s.Display}).OrderBy(o => o.Codigo).ToList();
                this.lkProdHasta.Properties.DataSource = EtCuentas.Select(s => new { s.Codigo, Display = s.Codigo + " | " + s.Display }).OrderBy(o => o.Codigo).ToList();

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;
                lkMesInicial_Validated(null, null);
                lkMesFinal.EditValue = DateTime.Now.Month;
                lkMesFinal_Validated(null, null);
                btnUnselect_Click(null, null);
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        public bool ValidarCampos()
        {
            if (lkES.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Estación de Servicio", Parametros.MsgType.warning);
                return false;
            }

            if (layoutControlItemSes.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
            {
                if (lkSUS.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Sub Estación de Servicio", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (lkProdDesde.EditValue != null || lkProdHasta.EditValue != null)
            {
                if (lkProdDesde.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar el código inicial del rango.", Parametros.MsgType.warning);
                    return false;
                }

                if (lkProdHasta.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar el código final del rango.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToInt64(lkProdDesde.EditValue) > Convert.ToInt64(lkProdHasta.EditValue))
                {
                    Parametros.General.DialogMsg("El código inicial debe ser menor al código final.", Parametros.MsgType.warning);
                    return false;
                }
            }
            else
            {
                if (EtCuentas.Count(c => c.valor) <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar al menos una cuenta de la lista.", Parametros.MsgType.warning);
                    return false;
                }
            }


            if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a recalcular.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            {
                Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                return false;
            }

            
            return true;
        }
               

        #endregion

        #region <<< EVENTOS >>>
        private void btnCargarLista_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                gvDataCuentas.ActiveFilter.Clear();
                
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            if (lkES.EditValue != null)
            {
                if (IDEstacion > 0)
                {
                    var Sus = EtSubEstaciones.Where(sus => sus.EstacionServicioID.Equals(IDEstacion) || sus.EstacionServicioID.Equals(0)).ToList();

                    if (Sus.Count(c => !c.EstacionServicioID.Equals(0))  > 0)
                    {
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lkSUS.Properties.DataSource = Sus.Select(s => new { s.ID, s.Nombre });
                        lkSUS.EditValue = null;
                    }
                    else
                    {
                        this.lkSUS.EditValue = null;
                        this.lkSUS.Properties.DataSource = null;
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    }
                }
            }
            else
            {
                this.lkSUS.EditValue = null;
                this.lkSUS.Properties.DataSource = null;
                this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (EtCuentas.Count() > 0)
                    EtCuentas.ToList().ForEach(p => p.valor = true);

                gvDataCuentas.RefreshData();
                lkProdDesde.EditValue = null;
                lkProdHasta.EditValue = null;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            try
            {
                if (EtCuentas.Count() > 0)
                    EtCuentas.ToList().ForEach(p => p.valor = false);

                gvDataCuentas.RefreshData();
                lkProdDesde.EditValue = null;
                lkProdHasta.EditValue = null;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void lkMesInicial_Validated(object sender, EventArgs e)
        {
            if (lkMesInicial.EditValue != null)
            {
                dateInicial.EditValue = Convert.ToDateTime(new DateTime(AnioInicial, Convert.ToInt32(lkMesInicial.EditValue), 1));
            }
        }

        private void lkMesFinal_Validated(object sender, EventArgs e)
        {
            if (lkMesFinal.EditValue != null)
            {
                int mes = (Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? 1 : Convert.ToInt32(lkMesFinal.EditValue) + 1);
                DateTime fecha = new DateTime((Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? AnioFinal + 1 : AnioFinal), mes, 1);
                dateFinal.EditValue = fecha.AddDays(-1);
            }
        }

        private void dateInicial_EditValueChanged(object sender, EventArgs e)
        {
            if (dateInicial.EditValue != null)
            {
                lkMesInicial.EditValue = Convert.ToDateTime(dateInicial.EditValue).Month;
                AnioInicial = Convert.ToDateTime(dateInicial.EditValue).Year;
            }
        }

        private void dateFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (dateFinal.EditValue != null)
            {
                lkMesFinal.EditValue = Convert.ToDateTime(dateFinal.EditValue).Month;
                AnioFinal = Convert.ToDateTime(dateFinal.EditValue).Year;
            } 
        }

        private void lkProdDesde_EditValueChanged(object sender, EventArgs e)
        {
            if (lkProdDesde.EditValue != null || lkProdHasta.EditValue != null)
            {
                gvDataCuentas.ActiveFilter.Clear();

                EtCuentas.ToList().ForEach(p => p.valor = false);
                gvDataCuentas.RefreshData();
            }
        }

        private void lkProdHasta_EditValueChanged(object sender, EventArgs e)
        {
            if (lkProdDesde.EditValue != null || lkProdHasta.EditValue != null)
            {
                gvDataCuentas.ActiveFilter.Clear();

                EtCuentas.ToList().ForEach(p => p.valor = false);
                gvDataCuentas.RefreshData();
            }
        }
    
        private void gvDataCuentas_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                if (EtCuentas.Count(c => c.valor) > 0)
                {
                    lkProdDesde.EditValue = null;
                    lkProdHasta.EditValue = null;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    bgw = new BackgroundWorker();
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dbView.CommandTimeout = 600;
                DateTime _Fecha = Convert.ToDateTime(db.GetDateServer());

                int ES = (int)lkES.EditValue;               

                Reportes.Contabilidad.Hojas.RptDetalleCuentas Mrep = new Reportes.Contabilidad.Hojas.RptDetalleCuentas();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                Mrep.PicLogo.Image = picture_LogoEmpresa;
                Mrep.CeEmpresa.Text = Nombre;
                Mrep.xrCeRango.Text = "Del " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + "   Al   " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();

                if (ES.Equals(0))
                {
                    #region <<< VARIAS ESATCIONES  >>>
                    
                    if (lkProdDesde.EditValue != null && lkProdHasta.EditValue != null)
                    {
                        #region <<< POR RANGO  >>>
                        foreach (var Sede in EtEstaciones)
                        {
                            if (EtSubEstaciones.Count(c => c.EstacionServicioID.Equals(Sede.ID)) > 0)
                            {
                                foreach (var Sub in EtSubEstaciones.Where(o => o.EstacionServicioID.Equals(Sede.ID)))
                                {
                                    int r = lkProdHasta.ItemIndex - lkProdDesde.ItemIndex + 1;
                                    var obj = EtCuentas.ToList().GetRange(lkProdDesde.ItemIndex, r).OrderByDescending(o => o.Codigo);

                                    EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                               where (vmd.EstacionServicioID.Equals(Sede.ID) && vmd.SubEstacionID.Equals(Sub.ID))
                                               && obj.Select(s => s.Codigo).Contains(vmd.CuentaCodigo) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                               && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                               select vmd).ToList();

                                    foreach (var det in obj)
                                    {

                                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                        DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                        var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                        decimal vSaldo = 0;
                                        if (lSaldo.Count() > 0)
                                            vSaldo = lSaldo.Sum(s => s.Monto);

                                        var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                        xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                        xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                        xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                        xrEstacion.StylePriority.UseTextAlignment = false;
                                        xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(Sede.ID)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(Sub.ID)).Nombre;
                                        xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                        ghEstacion.Controls.Add(xrEstacion);
                                        xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                        xrSubDetalle.LocationF = new PointF(0f, 20f);
                                        ghEstacion.Controls.Add(xrSubDetalle);
                                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                        Mrep.Bands.Add(ghEstacion);
                                    }
                                }
                            }
                            else
                            {
                                int r = lkProdHasta.ItemIndex - lkProdDesde.ItemIndex + 1;
                                var obj = EtCuentas.ToList().GetRange(lkProdDesde.ItemIndex, r).OrderByDescending(o => o.Codigo);

                                EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                           where (vmd.EstacionServicioID.Equals(Sede.ID))
                                           && obj.Select(s => s.Codigo).Contains(vmd.CuentaCodigo) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                           && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                           select vmd).ToList();

                                foreach (var det in obj)
                                {

                                    DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                    DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                    DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                    var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                    decimal vSaldo = 0;
                                    if (lSaldo.Count() > 0)
                                        vSaldo = lSaldo.Sum(s => s.Monto);

                                    var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                    xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                    xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                    xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                    xrEstacion.StylePriority.UseTextAlignment = false;
                                    xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(Sede.ID)).Nombre;
                                    xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                    ghEstacion.Controls.Add(xrEstacion);
                                    xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                    xrSubDetalle.LocationF = new PointF(0f, 20f);
                                    ghEstacion.Controls.Add(xrSubDetalle);
                                    ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                    Mrep.Bands.Add(ghEstacion);
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        #region <<< POR DETALLE  >>>
                        foreach (var Sede in EtEstaciones)
                        {
                            if (EtSubEstaciones.Count(c => c.EstacionServicioID.Equals(Sede.ID)) > 0)
                            {
                                foreach (var Sub in EtSubEstaciones.Where(o => o.EstacionServicioID.Equals(Sede.ID)))
                                {
                                    EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                               where (vmd.EstacionServicioID.Equals(Sede.ID) && vmd.SubEstacionID.Equals(Sub.ID)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                               && EtCuentas.Where(o => o.valor).Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                               && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                               select vmd).ToList();

                                    var obj = EtCuentas.Where(o => o.valor).OrderByDescending(o => o.Codigo);
                                    foreach (var det in obj)
                                    {

                                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                        DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                        var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                        decimal vSaldo = 0;
                                        if (lSaldo.Count() > 0)
                                            vSaldo = lSaldo.Sum(s => s.Monto);

                                        var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                        xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                        xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                        xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                        xrEstacion.StylePriority.UseTextAlignment = false;
                                        xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(Sede.ID)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(Sub.ID)).Nombre;
                                        xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                        ghEstacion.Controls.Add(xrEstacion);
                                        xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                        xrSubDetalle.LocationF = new PointF(0f, 20f);
                                        ghEstacion.Controls.Add(xrSubDetalle);
                                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                        Mrep.Bands.Add(ghEstacion);
                                    }
                                }
                            }
                            else
                            {
                                EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                           where (vmd.EstacionServicioID.Equals(Sede.ID)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                           && EtCuentas.Where(o => o.valor).Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                           && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                           select vmd).ToList();

                                var obj = EtCuentas.Where(o => o.valor).OrderByDescending(o => o.Codigo);
                                foreach (var det in obj)
                                {

                                    DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                    DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                    DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                    var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                    decimal vSaldo = 0;
                                    if (lSaldo.Count() > 0)
                                        vSaldo = lSaldo.Sum(s => s.Monto);

                                    var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                    xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                    xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                    xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                    xrEstacion.StylePriority.UseTextAlignment = false;
                                    xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(Sede.ID)).Nombre;
                                    xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                    ghEstacion.Controls.Add(xrEstacion);
                                    xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                    xrSubDetalle.LocationF = new PointF(0f, 20f);
                                    ghEstacion.Controls.Add(xrSubDetalle);
                                    ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                    Mrep.Bands.Add(ghEstacion);
                                }
                            }
                        } 
   
                        #endregion
                    }

                    #endregion
                }
                else
                {
                    if (layoutControlItemSes.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
                    {
                        int SUS = (int)lkSUS.EditValue;

                        #region <<< POR SUB ESATCION  >>>

                        if (SUS.Equals(0))
                        {
                            #region <<< VARIAS SUB ESATCION  >>>


                            if (lkProdDesde.EditValue != null && lkProdHasta.EditValue != null)
                            {
                                foreach (var Sub in EtSubEstaciones.Where(o => o.EstacionServicioID.Equals(ES)))
                                {
                                    int r = lkProdHasta.ItemIndex - lkProdDesde.ItemIndex + 1;
                                    var obj = EtCuentas.ToList().GetRange(lkProdDesde.ItemIndex, r).OrderByDescending(o => o.Codigo);

                                    EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                               where (vmd.EstacionServicioID.Equals(ES) && vmd.SubEstacionID.Equals(Sub.ID))
                                               && obj.Select(s => s.Codigo).Contains(vmd.CuentaCodigo) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                               && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                               select vmd).ToList();

                                    foreach (var det in obj)
                                    {

                                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                        DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                        var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                        decimal vSaldo = 0;
                                        if (lSaldo.Count() > 0)
                                            vSaldo = lSaldo.Sum(s => s.Monto);

                                        var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                        xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                        xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                        xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                        xrEstacion.StylePriority.UseTextAlignment = false;
                                        xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(Sub.ID)).Nombre;
                                        xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                        ghEstacion.Controls.Add(xrEstacion);
                                        xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                        xrSubDetalle.LocationF = new PointF(0f, 20f);
                                        ghEstacion.Controls.Add(xrSubDetalle);
                                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                        Mrep.Bands.Add(ghEstacion);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var Sub in EtSubEstaciones.Where(o => o.EstacionServicioID.Equals(ES)))
                                {
                                    EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                               where (vmd.EstacionServicioID.Equals(ES) && vmd.SubEstacionID.Equals(Sub.ID)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                               && EtCuentas.Where(o => o.valor).Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                               && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                               select vmd).ToList();

                                    var obj = EtCuentas.Where(o => o.valor).OrderByDescending(o => o.Codigo);
                                    foreach (var det in obj)
                                    {

                                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                        DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                        var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                        decimal vSaldo = 0;
                                        if (lSaldo.Count() > 0)
                                            vSaldo = lSaldo.Sum(s => s.Monto);

                                        var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                        xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                        xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                        xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                        xrEstacion.StylePriority.UseTextAlignment = false;
                                        xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(Sub.ID)).Nombre;
                                        xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                        ghEstacion.Controls.Add(xrEstacion);
                                        xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                        xrSubDetalle.LocationF = new PointF(0f, 20f);
                                        ghEstacion.Controls.Add(xrSubDetalle);
                                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                        Mrep.Bands.Add(ghEstacion);
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region <<< UNA SUB ESATCION  >>>
                            if (lkProdDesde.EditValue != null && lkProdHasta.EditValue != null)
                            {
                                int r = lkProdHasta.ItemIndex - lkProdDesde.ItemIndex + 1;
                                var obj = EtCuentas.ToList().GetRange(lkProdDesde.ItemIndex, r).OrderByDescending(o => o.Codigo);

                                EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                           where (vmd.EstacionServicioID.Equals(ES) && vmd.SubEstacionID.Equals(SUS))
                                           && obj.Select(s => s.Codigo).Contains(vmd.CuentaCodigo) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                           && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                           select vmd).ToList();

                                foreach (var det in obj)
                                {

                                    DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                    DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                    DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                    var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                    decimal vSaldo = 0;
                                    if (lSaldo.Count() > 0)
                                        vSaldo = lSaldo.Sum(s => s.Monto);

                                    var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                    xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                    xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                    xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                    xrEstacion.StylePriority.UseTextAlignment = false;
                                    xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(SUS)).Nombre;
                                    xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                    ghEstacion.Controls.Add(xrEstacion);
                                    xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                    xrSubDetalle.LocationF = new PointF(0f, 20f);
                                    ghEstacion.Controls.Add(xrSubDetalle);
                                    ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                    Mrep.Bands.Add(ghEstacion);
                                }
                            }
                            else
                            {
                                EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                           where (vmd.EstacionServicioID.Equals(ES) && vmd.SubEstacionID.Equals(SUS)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                           && EtCuentas.Where(o => o.valor).Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                           && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                           select vmd).ToList();

                                var obj = EtCuentas.Where(o => o.valor).OrderByDescending(o => o.Codigo);
                                foreach (var det in obj)
                                {

                                    DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                    DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                    DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                    var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                    decimal vSaldo = 0;
                                    if (lSaldo.Count() > 0)
                                        vSaldo = lSaldo.Sum(s => s.Monto);

                                    var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                    xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                    xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                    xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                    xrEstacion.StylePriority.UseTextAlignment = false;
                                    xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre + " | " + EtSubEstaciones.Single(s => s.ID.Equals(SUS)).Nombre;
                                    xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                    ghEstacion.Controls.Add(xrEstacion);
                                    xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                    xrSubDetalle.LocationF = new PointF(0f, 20f);
                                    ghEstacion.Controls.Add(xrSubDetalle);
                                    ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                    Mrep.Bands.Add(ghEstacion);
                                }
                            }

                            #endregion
                        }

                        #endregion
                    }
                    else
                    {
                        #region <<< POR ESATCION  >>>
                        if (lkProdDesde.EditValue != null && lkProdHasta.EditValue != null)
                        {
                            int r = lkProdHasta.ItemIndex - lkProdDesde.ItemIndex + 1;
                            var obj = EtCuentas.ToList().GetRange(lkProdDesde.ItemIndex, r).OrderByDescending(o => o.Codigo);

                            EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                       where (vmd.EstacionServicioID.Equals(ES)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                       && obj.Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                       && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                       select vmd).ToList();

                            foreach (var det in obj)
                            {

                                DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                decimal vSaldo = 0;
                                if (lSaldo.Count() > 0)
                                    vSaldo = lSaldo.Sum(s => s.Monto);

                                var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                xrEstacion.StylePriority.UseTextAlignment = false;
                                xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre;
                                xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                ghEstacion.Controls.Add(xrEstacion);
                                xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                xrSubDetalle.LocationF = new PointF(0f, 20f);
                                ghEstacion.Controls.Add(xrSubDetalle);
                                ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                Mrep.Bands.Add(ghEstacion);
                            }
                        }
                        else
                        {
                            EtVista = (from vmd in dbView.VistaRptMovimientoDetalle
                                       where (vmd.EstacionServicioID.Equals(ES)) && (chkCierre.Checked || (!chkCierre.Checked && vmd.MovimientoTipoID != 64))
                                       && EtCuentas.Where(o => o.valor).Select(s => s.Codigo).Contains(vmd.CuentaCodigo)
                                       && vmd.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                       select vmd).ToList();

                            var obj = EtCuentas.Where(o => o.valor).OrderByDescending(o => o.Codigo);
                            foreach (var det in obj)
                            {

                                DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                                DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();
                                DevExpress.XtraReports.UI.XRLabel xrEstacion = new DevExpress.XtraReports.UI.XRLabel();

                                var lSaldo = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                                decimal vSaldo = 0;
                                if (lSaldo.Count() > 0)
                                    vSaldo = lSaldo.Sum(s => s.Monto);

                                var Lista = EtVista.Where(o => o.CuentaCodigo.Equals(det.Codigo) && o.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date);

                                xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                                xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                                xrEstacion.SizeF = new System.Drawing.SizeF(200f, 20F);
                                xrEstacion.StylePriority.UseTextAlignment = false;
                                xrEstacion.Text = EtEstaciones.Single(s => s.ID.Equals(ES)).Nombre;
                                xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                                ghEstacion.Controls.Add(xrEstacion);
                                xrSubDetalle.ReportSource = new Reportes.Contabilidad.Hojas.SubRptDetalleCuentas(det.Codigo, det.Display, Lista, _Fecha.ToShortDateString(), vSaldo);
                                xrSubDetalle.LocationF = new PointF(0f, 20f);
                                ghEstacion.Controls.Add(xrSubDetalle);
                                ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                                Mrep.Bands.Add(ghEstacion);
                            }
                        }

                        #endregion
                    }
                }

                e.Result = Mrep;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
            
        }


        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    rep = ((Reportes.Contabilidad.Hojas.RptDetalleCuentas)(e.Result));
                    rep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}