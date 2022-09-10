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
using DevExpress.Data.Linq;
using DevExpress.Data.Filtering;
using System.Threading;
using System.Threading.Tasks;

namespace SAGAS.Contabilidad.Forms
{
    public partial class FormRptBalanceGeneral : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private BackgroundWorker bgw;
        Reportes.Contabilidad.Hojas.RptBalanzaComprobacion rep;
        internal string _Nombre, _Direccion, _Telefono;
        System.Drawing.Image _picture_LogoEmpresa;
        private List<ListadoGrupo> ListaGrupo = new List<ListadoGrupo>();
        private List<ListadoTipoCuenta> ListaTipoCuenta = new List<ListadoTipoCuenta>();
        private List<ListadoCuentas> ListaCuenta = new List<ListadoCuentas>();
        internal int _CuentaUtilidadEjerciocioID = 0;

        private int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        struct ListadoGrupo
        {
            public string NombreGrupo;
            public decimal MontoGrupo;
            public List<ListadoTipoCuenta> ChildList;
        };

        struct ListadoTipoCuenta
        {
            public string NombreGrupo;
            public int Orden;
            public string NombreTipoCuenta;
            public decimal MontoTipoCuenta;
            public List<ListadoCuentas> ChildList;
        };

        struct ListadoCuentas
        {
            public string NombreGrupo;
            public int Orden;
            public string NombreTipoCuenta;
            public string Display;
            public decimal MontoCuenta;            
        };

        #endregion

        #region *** INICIO ***

        public FormRptBalanceGeneral()
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

                _CuentaUtilidadEjerciocioID = Parametros.Config.CuentaUtilidadEjerciocioID();

                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkFusion"))
                    layoutControlItemChkFusion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                if (Parametros.General.Empresa != null)
                {
                    Parametros.General.GetCompanyData(out _Nombre, out _Direccion, out _Telefono, out _picture_LogoEmpresa);
                    picEmpresa.Image = _picture_LogoEmpresa;
                }
                //Estaciones de Servicio
                var lista = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                             select new { es.ID, es.Nombre }).ToList();

                lkES.Properties.DataSource = lista;
                lkES.EditValue = Parametros.General.EstacionServicioID;

                DateTime Fecha = Convert.ToDateTime(db.GetDateServer());

                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.EditValue = Fecha.Month;
                spAnio.Value = Fecha.Year;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                //btnLoad_Click(null, null);                
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        public bool ValidarCampos()
        {

            //if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a recalcular.", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            //{
            //    Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
            //    return false;
            //}

            
            return true;
        }
               

        #endregion

        #region <<< EVENTOS >>>
        
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
        }           
        
        private async void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {

                    await Generar().ConfigureAwait(false);
                    //bgw = new BackgroundWorker();
                    //bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    //bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    //bgw.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        async Task Generar()
        {
            try
            {
            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
            
                await Task.Delay(10);

            if (Parametros.General.Empresa != null)
            {
                Parametros.General.GetCompanyData(out _Nombre, out _Direccion, out _Telefono, out _picture_LogoEmpresa);
                picEmpresa.Image = _picture_LogoEmpresa;
            }

            dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            dv.CommandTimeout = db.CommandTimeout = 600;

            //Parametros.General.GerenteGeneralID = ;
            //Parametros.General.ViceGerenteFinancieroID = ;
            //Parametros.General.ContadorGeneralID = ;
            //Parametros.General.ViceContadorID = ;
            //Parametros.General.ViceGerenteGeneralID = ;

            DateTime _Fecha = new DateTime(Convert.ToInt32(spAnio.Value), Convert.ToInt32(lkMesFinal.EditValue), 1).AddMonths(1).AddDays(-1);
            var Gr = db.TipoCuentaGrupo.Select(s => new { s.ID, s.Nombre }).OrderBy(o => o.ID).ToList();

            Reportes.Contabilidad.Hojas.RptBalance MRep = new Reportes.Contabilidad.Hojas.RptBalance();
            MRep.xrCeEmpresa.Text = _Nombre;
            MRep.PicLogo.Image = _picture_LogoEmpresa;
            MRep.xrCeEstacion.Text = (chkFusion.Checked ? " " : lkES.Text);
            MRep.CeRango.Text = "Al:   " + _Fecha.ToShortDateString();
            if (chkFusion.Checked)
            {
                MRep.xrLblContadorGral.Text = "Lic." + Parametros.Config.ChargeSignature(3);
                MRep.xrLblViceGteFinan.Text = "Lic." + Parametros.Config.ChargeSignature(49);
                MRep.xrLblViceGteGral.Text = "Lic." + Parametros.Config.ChargeSignature(47);
                MRep.xrLblGteGral.Text = "Lic." + Parametros.Config.ChargeSignature(1);
            }
            else
            {
                MRep.xrLblRevVGG.Visible = false;
                MRep.xrLblViceGteGral.Visible = false;
                MRep.xrLblVGG.Visible = false;
                MRep.xrLblAprobGG.Visible = false;
                MRep.xrLblGteGral.Visible = false;
                MRep.xrLblGG.Visible = false;
                var Elaborado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(IDEstacion)).ResponsableContableID)));
                var Revisado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(IDEstacion)).AdministradorID)));
                if (Elaborado != null)
                {
                    MRep.xrLblContadorGral.Text = Elaborado.Nombres + " " + Elaborado.Apellidos;
                    MRep.xrLblCG.Text = "Contador";
                }
                else
                {
                    MRep.xrLblContadorGral.Text = "<>..........................";
                    MRep.xrLblCG.Text = "Contador";
                }

                if (Revisado != null)
                {
                    MRep.xrLblViceGteFinan.Text = Revisado.Nombres + " " + Revisado.Apellidos;
                    MRep.xrLblVGF.Text = "Administrador";
                }
                else
                {
                    MRep.xrLblViceGteFinan.Text = "<>..........................";
                    MRep.xrLblVGF.Text = "Administrador";
                }
            }
            ListaCuenta = (from v in dv.GetBalanceGeneral(IDEstacion, chkCierre.Checked, _Fecha)
                           where !String.IsNullOrEmpty(v.Grupo) && v.IDPadreTipoCuenta.Equals(1)
                           select new ListadoCuentas
                           {
                               Orden = v.Orden,
                               NombreGrupo = v.Grupo,
                               NombreTipoCuenta = v.TipoCuena,
                               Display = v.Codigo + " " + v.Nombre,
                               MontoCuenta = v.Saldo
                           }).OrderBy(o => o.Orden).ToList();

            ListaTipoCuenta = (from v in ListaCuenta
                               group v by new { v.NombreGrupo, v.NombreTipoCuenta, v.Orden } into gr
                               select new ListadoTipoCuenta
                               {
                                   Orden = gr.Key.Orden,
                                   NombreGrupo = gr.Key.NombreGrupo,
                                   NombreTipoCuenta = gr.Key.NombreTipoCuenta,
                                   MontoTipoCuenta = gr.Sum(s => s.MontoCuenta),
                                   ChildList = gr.Where(o => o.NombreTipoCuenta.Equals(gr.Key.NombreTipoCuenta)).Select(s => new ListadoCuentas { Display = s.Display, MontoCuenta = s.MontoCuenta, NombreTipoCuenta = s.NombreTipoCuenta }).ToList()
                               }).OrderBy(o => o.Orden).ToList();

            ListaGrupo = (from v in ListaTipoCuenta
                          group v by new { v.NombreGrupo } into gr
                          select new ListadoGrupo
                          {
                              NombreGrupo = gr.Key.NombreGrupo,
                              MontoGrupo = gr.Sum(s => s.MontoTipoCuenta),
                              ChildList = gr.Where(o => o.NombreGrupo.Equals(gr.Key.NombreGrupo)).Select(s => new ListadoTipoCuenta { NombreTipoCuenta = s.NombreTipoCuenta, MontoTipoCuenta = s.MontoTipoCuenta }).ToList()
                          }).ToList();

            var ListaA = ListaGrupo.Select(s => new { s.NombreGrupo, s.MontoGrupo, ListadoTipoCuenta = s.ChildList.Select(o => new { o.NombreTipoCuenta, o.MontoTipoCuenta, o.Orden, ListadoCuentas = ListaCuenta.Where(c => c.NombreTipoCuenta.Equals(o.NombreTipoCuenta)).Select(c => new { Display = c.Display, MontoCuenta = c.MontoCuenta }).ToList() }).OrderBy(o => o.Orden).ToList() }).ToList();

            bdsActivo.DataSource = ListaA;
            gvDataA.RefreshData();
            float TamanoA = 0f;
            foreach (var item in Gr)
            {
                if (ListaA.Select(s => s.NombreGrupo).Contains(item.Nombre))
                {
                    DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                    xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptBalance("NombreTipoCuenta", "MontoTipoCuenta", item.Nombre, ListaTipoCuenta.Where(o => o.NombreGrupo.Equals(item.Nombre)).Select(s => new { s.NombreTipoCuenta, s.MontoTipoCuenta }).ToList(), false, 0m);
                    xrSubA.LocationF = new PointF(100f, TamanoA);
                    TamanoA += xrSubA.HeightF;
                    MRep.gh1.Controls.Add(xrSubA);
                }
            }

            decimal TotalA = ListaA.Sum(s => s.MontoGrupo);
            MRep.lblTotalActivo.Text = TotalA.ToString("#,0.00;(#,0.00)");
            ListaCuenta.Clear();
            ListaTipoCuenta.Clear();
            ListaGrupo.Clear();

            ListaCuenta = (from v in dv.GetBalanceGeneral(IDEstacion, chkCierre.Checked, _Fecha)
                           where !String.IsNullOrEmpty(v.Grupo) && !v.IDPadreTipoCuenta.Equals(1)
                           select new ListadoCuentas
                           {
                               Orden = v.Orden,
                               NombreGrupo = v.Grupo,
                               NombreTipoCuenta = v.TipoCuena,
                               Display = v.Codigo + " " + v.Nombre,
                               MontoCuenta = v.Saldo
                           }).OrderBy(o => o.Orden).ToList();

                    decimal Saldo = Convert.ToDecimal(db.GetUtilidadEjercicio(IDEstacion, 0, chkCierre.Checked, _Fecha));

                    Saldo += Convert.ToDecimal(db.GetUtilidadEjercicio(IDEstacion, 0, true, new DateTime(_Fecha.Year - 1, 12, 31)));

                    if (!Saldo.Equals(0))
                    {
                        var cuenta = (from c in db.CuentaContables
                                      join tc in db.TipoCuentas on c.IDTipoCuenta equals tc.ID
                                      join tcr in db.TipoCuentaGrupo on tc.GrupoID equals tcr.ID
                                      where c.ID.Equals(_CuentaUtilidadEjerciocioID)
                                      select new
                                      {
                                          Display = c.Codigo + " " + c.Nombre,
                                          NombreTipoCuenta = tc.Nombre,
                                          NombreGrupo = tcr.Nombre,
                                          Orden = tc.Orden
                                      }).Single();

                        ListaCuenta.Add(new ListadoCuentas { Display = cuenta.Display, Orden = cuenta.Orden, NombreGrupo = cuenta.NombreGrupo, NombreTipoCuenta = cuenta.NombreTipoCuenta, MontoCuenta = Saldo });

                    }

            ListaTipoCuenta = (from v in ListaCuenta
                               group v by new { v.NombreGrupo, v.NombreTipoCuenta, v.Orden } into gr
                               select new ListadoTipoCuenta
                               {
                                   Orden = gr.Key.Orden,
                                   NombreGrupo = gr.Key.NombreGrupo,
                                   NombreTipoCuenta = gr.Key.NombreTipoCuenta,
                                   MontoTipoCuenta = gr.Sum(s => s.MontoCuenta),
                                   ChildList = gr.Where(o => o.NombreTipoCuenta.Equals(gr.Key.NombreTipoCuenta)).Select(s => new ListadoCuentas { Display = s.Display, MontoCuenta = s.MontoCuenta, NombreTipoCuenta = s.NombreTipoCuenta }).ToList()
                               }).OrderBy(o => o.Orden).ToList();

            ListaGrupo = (from v in ListaTipoCuenta
                          group v by new { v.NombreGrupo } into gr
                          select new ListadoGrupo
                          {
                              NombreGrupo = gr.Key.NombreGrupo,
                              MontoGrupo = gr.Sum(s => s.MontoTipoCuenta),
                              ChildList = gr.Where(o => o.NombreGrupo.Equals(gr.Key.NombreGrupo)).Select(s => new ListadoTipoCuenta { NombreTipoCuenta = s.NombreTipoCuenta, MontoTipoCuenta = s.MontoTipoCuenta }).ToList()
                          }).ToList();

            var ListaB = ListaGrupo.Select(s => new { s.NombreGrupo, s.MontoGrupo, ListadoTipoCuenta = s.ChildList.Select(o => new { o.NombreTipoCuenta, o.Orden, o.MontoTipoCuenta, ListadoCuentas = ListaCuenta.Where(c => c.NombreTipoCuenta.Equals(o.NombreTipoCuenta)).Select(c => new { Display = c.Display, MontoCuenta = c.MontoCuenta }).ToList() }).OrderBy(o => o.Orden).ToList() }).ToList();

            bdsPasCap.DataSource = ListaB;
            gvDataB.RefreshData();
            float TamanoB = 0f;

            foreach (var item in Gr)
            {
                if (ListaB.Select(s => s.NombreGrupo).Contains(item.Nombre))
                {
                    bool Mostrar = (item.Nombre.Contains("CAPITAL") ? true : false);
                    decimal mPasivo = 0m;

                    if (Mostrar)
                    {
                        var vPasivo = ListaB.Where(o => !o.NombreGrupo.Contains(item.Nombre));

                        

                        if (vPasivo.Count() > 0)
                            mPasivo = vPasivo.Sum(s => s.MontoGrupo);
                    }

                    DevExpress.XtraReports.UI.XRSubreport xrSubB = new DevExpress.XtraReports.UI.XRSubreport();
                    xrSubB.ReportSource = new Reportes.Contabilidad.Hojas.SubRptBalance("NombreTipoCuenta", "MontoTipoCuenta", item.Nombre, ListaTipoCuenta.Where(o => o.NombreGrupo.Equals(item.Nombre)).Select(s => new { s.NombreTipoCuenta, s.MontoTipoCuenta }).ToList(), Mostrar, mPasivo);
                    xrSubB.LocationF = new PointF(100f, TamanoB);
                    TamanoB += xrSubB.HeightF;
                    MRep.ghPas.Controls.Add(xrSubB);
                }
            }
            decimal TotalB = ListaB.Sum(s => s.MontoGrupo);
            MRep.lblTotalPas.Text = TotalB.ToString("#,0.00;(#,0.00)");

            if (Math.Abs(TotalA) != Math.Abs(TotalB))
                MRep.Watermark.Text = "DESCUADRADO";

            MRep.CreateDocument(true);

            this.printControlAreaReport.PrintingSystem = MRep.PrintingSystem;
            Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
            
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //if (e.Result != null)
                //{
                //    rep = ((Reportes.Contabilidad.Hojas.RptBalanzaComprobacion)(e.Result));
                //    rep.CreateDocument(true);
                //    this.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void swFusion_Toggled(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void btnLoadGrid_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    //bgwGrid = new BackgroundWorker();
                    //bgwGrid.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwGrid_RunWorkerCompleted);
                    //bgwGrid.DoWork += new DoWorkEventHandler(bgwGrid_DoWork);
                    //bgwGrid.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgwGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                //Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                //dbView.CommandTimeout = 600;

                //EtVista = dbView.GetBalanzaComprobacion(0, 0, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList();

                //e.Result = EtVista;
                //Parametros.General.splashScreenManagerMain.CloseWaitForm();

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        void bgwGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    //bdsCuentas.DataSource = ((List<Entidad.GetBalanzaComprobacionResult>)(e.Result));
                    //pivotGridControl1.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void chkFusion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFusion.Checked)
                {
                    lkES.EditValue = null;
                    lkES.Enabled = false;
                }
                else
                {
                    lkES.EditValue = null;
                    lkES.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        private void lkMesFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (lkMesFinal.Text == "Diciembre")
            {
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                chkCierre.EditValue = false;
            }
            else
            {
                chkCierre.EditValue = true;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        #endregion

    }
}