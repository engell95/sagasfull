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

namespace SAGAS.Contabilidad.Forms
{
    public partial class FormRptEstadoResultado : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int Usuario = Parametros.General.UserID;
        private BackgroundWorker bgw;
        Reportes.Contabilidad.Hojas.RptEstadoResultado rep;
        internal string _Nombre, _Direccion, _Telefono;
        System.Drawing.Image _picture_LogoEmpresa;
        private List<ListadoGrupo> ListaGrupo = new List<ListadoGrupo>();
        private List<ListadoTipoCuenta> ListaTipoCuenta = new List<ListadoTipoCuenta>();
        private List<ListadoCuentas> ListaCuenta = new List<ListadoCuentas>();
        internal int _CuentaUtilidadEjerciocioID = 0;
        private List<StEstaciones> EtEstaciones = new List<StEstaciones>();
        private List<StSubEstaciones> EtSubEstaciones = new List<StSubEstaciones>();

        private int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        struct StEstaciones
        {
            public int ID;
            public string Nombre;
            public bool Activo;
        };

        struct StSubEstaciones
        {
            public int ID;
            public string Nombre;
            public int EstacionServicioID;
            public bool Activo;
        };

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

        public FormRptEstadoResultado()
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

                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkFusionEERR"))
                    layoutControlItemChkFusion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                if (Parametros.General.Empresa != null)
                {
                    Parametros.General.GetCompanyData(out _Nombre, out _Direccion, out _Telefono, out _picture_LogoEmpresa);
                    picEmpresa.Image = _picture_LogoEmpresa;
                }

                //Estaciones de Servicio
                var Est = (from es in db.EstacionServicios
                           where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                           select new StEstaciones { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).ToList();

                var SubEst = db.SubEstacions.Where(ses => ses.Activo && Est.Select(es => es.ID).Contains(ses.EstacionServicioID)).Select(s => new StSubEstaciones { ID = s.ID, Nombre = s.Nombre, EstacionServicioID = s.EstacionServicioID, Activo = s.Activo }).ToList();

                if (SubEst.Count > 0)
                    SubEst.Insert(0, new StSubEstaciones { ID = 0, Nombre = "TODOS", EstacionServicioID = 0, Activo = true });

                EtEstaciones = Est.ToList();
                EtSubEstaciones = SubEst.ToList();
                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre }); ;
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
            if (lkES.EditValue != null)
            {
                if (IDEstacion > 0)
                {
                    var Sus = EtSubEstaciones.Where(sus => sus.EstacionServicioID.Equals(IDEstacion) || sus.EstacionServicioID.Equals(0)).ToList();

                    if (Sus.Count(c => !c.EstacionServicioID.Equals(0)) > 0)
                    {
                        this.layoutControlItemSus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lkSus.Properties.DataSource = Sus.Select(s => new { s.ID, s.Nombre });
                        lkSus.EditValue = null;
                    }
                    else
                    {
                        this.lkSus.EditValue = null;
                        this.lkSus.Properties.DataSource = null;
                        this.layoutControlItemSus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    }
                }
            }
            else
            {
                this.lkSus.EditValue = null;
                this.lkSus.Properties.DataSource = null;
                this.layoutControlItemSus.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);

                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv.CommandTimeout = db.CommandTimeout = 600;
                
                DateTime _Fecha = new DateTime(Convert.ToInt32(spAnio.Value), Convert.ToInt32(lkMesFinal.EditValue), 1).AddMonths(1).AddDays(-1);
                var Gr = db.TipoCuentaGrupo.Select(s => new { s.ID, s.Nombre }).OrderBy(o => o.ID).ToList();

                Reportes.Contabilidad.Hojas.RptEstadoResultado MRep = new Reportes.Contabilidad.Hojas.RptEstadoResultado();
                
                MRep.xrCeEstacion.Text = (chkFusionEERR.Checked ? " " : lkES.Text);               

                DateTime _FechaInicioAnterior;
                DateTime _FechaIFinAnterior;
                DateTime _FechaInicioActual;
                DateTime _FechaInicioPeriodo;

                _FechaInicioAnterior = _Fecha.AddDays(1).AddMonths(-2);
                _FechaIFinAnterior = _FechaInicioAnterior.AddMonths(1).AddDays(-1);
                _FechaInicioActual = _Fecha.AddDays(1).AddMonths(-1);
                _FechaInicioPeriodo = new DateTime(_Fecha.Year, 1, 1);

                MRep.CeRango.Text = "Del:   " + _FechaInicioPeriodo.ToShortDateString() + "  Al:   " + _Fecha.ToShortDateString();
                                                                                                                                      //    01/11/2015              30/11/2015                  01/12/2015         31/12/2015      01/01/2015
                //var obj = dv.GetEstadoResultado(IDEstacion, (lkSus.EditValue != null ? (int)lkSus.EditValue : 0), chkCierre.Checked, _FechaInicioAnterior.Date, _FechaIFinAnterior.Date, _FechaInicioActual.Date, _Fecha.Date, _FechaInicioPeriodo.Date).Where(o => !String.IsNullOrEmpty(o.Grupo)).ToList();

                var obj = (from v in dv.GetEstadoResultado(IDEstacion, (lkSus.EditValue != null ? (int)lkSus.EditValue : 0), chkCierre.Checked, _FechaInicioAnterior.Date, _FechaIFinAnterior.Date, _FechaInicioActual.Date, _Fecha.Date, _FechaInicioPeriodo.Date)
                           where !String.IsNullOrEmpty(v.Grupo)
                           select new
                           {
                               v.CuentaCodigo,
                               v.CuentaContableID,
                               v.CuentaNombre,
                               v.EstacionNombre,
                               v.EstacionServicioID,
                               v.Grupo,
                               v.IDPadreTipoCuenta,
                               v.IDTipoCuenta,
                               v.Orden,
                               v.SaldoActual,
                               v.SaldoAcumulado,
                               v.SaldoAnterior,
                               v.SubEstacionID,
                               v.SubEstacionNombre,
                               v.TipoCuenta
                           }).ToList();


                //(from v in dv.GetBalanceGeneral(IDEstacion, chkCierre.Checked, _Fecha)
                // where !String.IsNullOrEmpty(v.Grupo) && v.IDPadreTipoCuenta.Equals(1)
                // select new ListadoCuentas
                // {
                //     Orden = v.Orden,
                //     NombreGrupo = v.Grupo,
                //     NombreTipoCuenta = v.TipoCuena,
                //     Display = v.Codigo + " " + v.Nombre,
                //     MontoCuenta = v.Saldo
                // }).OrderBy(o => o.Orden).ToList();

                float TamanoA = 20f;
                float TamanoB = 20f;
                float TamanoC = 20f;
                float TamanoD = 40f;
                float TamanoE = 40f;

                decimal _S2A = 0;
                decimal _S2B = 0;
                decimal _S2C = 0;

                decimal _S3A = 0;
                decimal _S3B = 0;
                decimal _S3C = 0;

                decimal _S4A = 0;
                decimal _S4B = 0;
                decimal _S4C = 0;

                decimal _S5A = 0;
                decimal _S5B = 0;
                decimal _S5C = 0;

                decimal _S6A = 0;
                decimal _S6B = 0;
                decimal _S6C = 0;

                foreach (var item in Gr)
                {
                    if (obj.Select(s => s.Grupo).Contains(item.Nombre))
                    {
                        if (item.ID.Equals(7) || item.ID.Equals(8))
                        {
                            var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.CuentaNombre }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                            _S2A += Convert.ToDecimal(query.Sum(s => s.SaldoAnterior));
                            _S2B += Convert.ToDecimal(query.Sum(s => s.SaldoActual));
                            _S2C += Convert.ToDecimal(query.Sum(s => s.SaldoAcumulado));

                            DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, query, true, true);
                            xrSubA.LocationF = new PointF(0f, TamanoA);
                            TamanoA += xrSubA.HeightF;
                            MRep.gh1.Controls.Add(xrSubA);
                        }
                        else if (item.ID.Equals(9))
                        {
                            var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.CuentaNombre }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                            _S3A += Convert.ToDecimal(query.Sum(s => s.SaldoAnterior));
                            _S3B += Convert.ToDecimal(query.Sum(s => s.SaldoActual));
                            _S3C += Convert.ToDecimal(query.Sum(s => s.SaldoAcumulado));

                            DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, query, true, true);
                            xrSubA.LocationF = new PointF(0f, TamanoB);
                            TamanoB += xrSubA.HeightF;
                            MRep.gh2.Controls.Add(xrSubA);
                        }
                        else if (item.ID.Equals(15))
                        {
                            var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.TipoCuenta }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                            var tipos = (from o in query
                                         group o by new { o.Display } into gr
                                         select new
                                         {
                                             Display = gr.Key.Display,
                                             SaldoActual = gr.Sum(s => s.SaldoActual),
                                             SaldoAnterior = gr.Sum(s => s.SaldoAnterior),
                                             SaldoAcumulado = gr.Sum(s => s.SaldoAcumulado)
                                         }).ToList();

                            _S5A += Convert.ToDecimal(tipos.Sum(s => s.SaldoAnterior));
                            _S5B += Convert.ToDecimal(tipos.Sum(s => s.SaldoActual));
                            _S5C += Convert.ToDecimal(tipos.Sum(s => s.SaldoAcumulado));

                            DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, tipos, true, true);
                            xrSubA.LocationF = new PointF(0f, TamanoD);
                            TamanoD += xrSubA.HeightF;
                            MRep.gh4.Controls.Add(xrSubA);
                        }
                        else if (item.ID.Equals(16))
                        {
                            var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.CuentaNombre }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                            _S6A += Convert.ToDecimal(query.Sum(s => s.SaldoAnterior));
                            _S6B += Convert.ToDecimal(query.Sum(s => s.SaldoActual));
                            _S6C += Convert.ToDecimal(query.Sum(s => s.SaldoAcumulado));

                            DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, query, true, true);
                            xrSubA.LocationF = new PointF(0f, TamanoE);
                            TamanoE += xrSubA.HeightF;
                            MRep.gh5.Controls.Add(xrSubA);
                        }
                        else
                        {
                            if (item.ID.Equals(10))
                            {
                                var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.CuentaNombre }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                                _S4A += Convert.ToDecimal(query.Sum(s => s.SaldoAnterior));
                                _S4B += Convert.ToDecimal(query.Sum(s => s.SaldoActual));
                                _S4C += Convert.ToDecimal(query.Sum(s => s.SaldoAcumulado));

                                DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, query, true, true);
                                xrSubA.LocationF = new PointF(0f, TamanoC);
                                TamanoC += xrSubA.HeightF;
                                MRep.gh3.Controls.Add(xrSubA);
                            }
                            else
                            {
                                var query = obj.Where(o => o.Grupo.Equals(item.Nombre)).GroupBy(g => new { Display = g.TipoCuenta }).Select(s => new { s.Key.Display, SaldoAnterior = s.Sum(m => m.SaldoAnterior), SaldoActual = s.Sum(m => m.SaldoActual), SaldoAcumulado = s.Sum(m => m.SaldoAcumulado) }).ToList();

                                var tipos = (from o in query
                                             group o by new { o.Display } into gr
                                             select new
                                             {
                                                 Display = gr.Key.Display,
                                                 SaldoActual = gr.Sum(s => s.SaldoActual),
                                                 SaldoAnterior = gr.Sum(s => s.SaldoAnterior),
                                                 SaldoAcumulado = gr.Sum(s => s.SaldoAcumulado)
                                             }).ToList();

                                _S4A += Convert.ToDecimal(tipos.Sum(s => s.SaldoAnterior));
                                _S4B += Convert.ToDecimal(tipos.Sum(s => s.SaldoActual));
                                _S4C += Convert.ToDecimal(tipos.Sum(s => s.SaldoAcumulado));

                                DevExpress.XtraReports.UI.XRSubreport xrSubA = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubA.ReportSource = new Reportes.Contabilidad.Hojas.SubRptEstadoResultado("Display", "SaldoAnterior", "SaldoActual", "SaldoAcumulado", item.Nombre, tipos, false, false);
                                xrSubA.LocationF = new PointF(0f, TamanoC);
                                TamanoC += xrSubA.HeightF;
                                MRep.gh3.Controls.Add(xrSubA);
                            }
                        }
                    }
                }

                MRep.xrS2A.Text = String.Format( "{0:#,0.00;(#,0.00)}",  _S2A);
                MRep.xrS2B.Text = String.Format( "{0:#,0.00;(#,0.00)}", _S2B);
                MRep.xrS2C.Text = String.Format( "{0:#,0.00;(#,0.00)}", _S2C);

                MRep.xrS3A.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2A + _S3A));
                MRep.xrS3B.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2B + _S3B));
                MRep.xrS3C.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2C + _S3C));

                MRep.xrS4A.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2A + _S3A + _S4A));
                MRep.xrS4B.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2B + _S3B + _S4B));
                MRep.xrS4C.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2C + _S3C + _S4C));

                MRep.xrS5A.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2A + _S3A + _S4A + _S5A));
                MRep.xrS5B.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2B + _S3B + _S4B + _S5B));
                MRep.xrS5C.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2C + _S3C + _S4C + _S5C));

                MRep.xrS6A.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2A + _S3A + _S4A + _S5A + _S6A));
                MRep.xrS6B.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2B + _S3B + _S4B + _S5B + _S6B));
                MRep.xrS6C.Text = String.Format( "{0:#,0.00;(#,0.00)}",Convert.ToDecimal(_S2C + _S3C + _S4C + _S5C + _S6C));

                if (chkFusionEERR.Checked)
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

                e.Result = MRep;
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
                    rep = ((Reportes.Contabilidad.Hojas.RptEstadoResultado)(e.Result));

                    if (Parametros.General.Empresa != null)
                    {
                        Parametros.General.GetCompanyData(out _Nombre, out _Direccion, out _Telefono, out _picture_LogoEmpresa);
                        picEmpresa.Image = _picture_LogoEmpresa;
                    }
                    rep.xrCeEmpresa.Text = _Nombre;
                    rep.PicLogo.Image = _picture_LogoEmpresa;
                    rep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                }
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
                if (chkFusionEERR.Checked)
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