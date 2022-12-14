using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Nomina.Forms
{
    public partial class FormPrestaciones : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private List<StPlanillas> EtPlanillas = new List<StPlanillas>();
        private List<Entidad.DiasVacaciones> EtDias;
        private int Usuario = Parametros.General.UserID;

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(new DateTime (dateFechaDesde.DateTime.Year, dateFechaDesde.DateTime.Month, DateTime.DaysInMonth(dateFechaDesde.DateTime.Year, dateFechaDesde.DateTime.Month))); }
            set { dateFechaDesde.EditValue = value; }
        }

        struct StPlanillas
        {
            public int ID;
            public string Nombre;
            public bool Activo;
        };

        #endregion

        #region *** INICIO ***

        public FormPrestaciones()
        {            
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //rgCreditoContado.SelectedIndex = 0;
            //IDProveedor = 186;
            //Referencia = "123";
            //IDAlmacen = 1;
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
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

                Fecha = DateTime.Now.Date;
                #region Habilitar/Deshabilitar bot?n exportar a excel
                //int cargoid = (from u in db.Usuarios
                //               join e in db.Empleados on u.EmpleadoID equals e.ID
                //               where u.ID.Equals(Parametros.General.UserID)
                //               select new { e.CargoID }).Single().CargoID;
                //if (cargoid > 0)
                //{
                //    /*if (cargoid == 41)
                //    { lciExcel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; }
                //    else
                //    { lciExcel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never; }*/
                //    //btnExcel.Enabled = (cargoid == 41 || cargoid == 1 || cargoid == 3) ? true : false;
                //}
                #endregion

                #region Cargar las Planillas
                var PL = (from es in db.Planillas
                          where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.EstacionServicioID))
                          select new StPlanillas { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).OrderBy(o => o.Nombre).ToList();

                if (Convert.ToInt32(db.GetViewEstacionesServicioByUsers.Count(ges => ges.UsuarioID == Usuario)) >= Convert.ToInt32(db.EstacionServicios.Count(o => o.Activo)))
                    PL.Insert(0, new StPlanillas { ID = 0, Nombre = "Todas las Planillas.", Activo = true });

                EtPlanillas = PL.ToList();
                lePlanilla.Properties.DataSource = EtPlanillas.Select(s => new { s.ID, s.Nombre });
                #endregion

                tgSwTipo_Toggled(null, null);

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private decimal diasVacaciones(int id, DateTime? _fecha)
        {
            try
            {
                if (_fecha != null)
                {
                    int years = 0, months = 0, days = 0;
                    decimal diasIndem = 0, mesesIndem = 0, anyosIndem = 0;
                    Parametros.General.RangoFechasYMD(Fecha.Date, _fecha.Value.Date, out years, out months, out days);

                    decimal diasAcumulados = Decimal.Round(years * Parametros.General.valorAnualVacaciones, 2, MidpointRounding.AwayFromZero);
                    diasAcumulados += Decimal.Round(months * Parametros.General.valorMensualVacaciones, 2, MidpointRounding.AwayFromZero);
                    diasAcumulados += Decimal.Round(days * Parametros.General.valorDiarioVacaciones, 2, MidpointRounding.AwayFromZero);

                    decimal D = 0;

                    //if (diasAcumulados < 0)
                    //{
                        // if (EtDias.Where(o => o.EmpleadoID.Equals(id)).Count() > 0)
                        //{

                        //    D = Decimal.Round(diasAcumulados + Convert.ToDecimal(EtDias.Where(o => o.FechaPago.Date >= Fecha.Date && o.EmpleadoID.Equals(id)).Sum(s => s.Dias)), 2, MidpointRounding.AwayFromZero);
                       
                        // }
                        //else

                            D = Decimal.Round(diasAcumulados, 2, MidpointRounding.AwayFromZero);
                    //}
                    //else
                    //    return 0m;
                         if (D < 0m)
                             D = 0;

                    return D;
               
                }
                else
                    return 0m;
            }
            catch { return 0m; }
        }

        private decimal GetIndemnizacion(decimal Salario, DateTime? FechaIngreso)
        {
            try
            {
                if (FechaIngreso != null)
                {
                    decimal diasIndem = 0, mesesIndem = 0, anyosIndem = 0, diasTotales;
                    return Decimal.Round(Parametros.General.CalcularIndemnizacion(Salario, FechaIngreso.Value.Date, Fecha.Date, out diasIndem, out mesesIndem, out anyosIndem), 2, MidpointRounding.AwayFromZero);

                }
                else
                    return 0m;
            }
            catch (Exception ex)
            {
                return 0m;
            }
        }


        #endregion

        #region *** EVENTOS ***

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                int ESID = lePlanilla.EditValue != null ? Convert.ToInt32(lePlanilla.EditValue) : 0;

                if (tgSwTipo.IsOn)
                {
                    var obj = from v in dv.VistaLiquidacions
                              where (Convert.ToDateTime(v.FechaRegistro) >= dateFechaInicial.DateTime.Date) && Convert.ToDateTime(v.FechaRegistro).Date <= Fecha
                              select v;

                    gridLiq.DataSource = obj;
                    gvLiqidacion.RefreshData();
                }
                else
                {
                    var obj = dv.GetConsolidadoVacaciones(ESID, Fecha);

                    //EtDias = db.DiasVacaciones.Where(o => o.FechaDesde.Date > Fecha.Date).ToList();


                    var vac = from o in obj
                              where !o.PlanillaID.Equals(25) && o.FechaIngreso <= dateFechaDesde.DateTime.Date
                              select new
                              {
                                  ID = o.ID,
                                  Codigo = o.Codigo,
                                  Cargo = o.CargoNombre,
                                  Empleado = o.Empleado,
                                  Planilla = o.Planilla,
                                  Fecha = this.Fecha.Date,
                                  o.FechaIngreso,
                                  o.Salario,
                                  DiasAguinaldo = diasVacaciones(o.ID, (o.FechaUltimoAguinaldo == null ? o.FechaIngreso : o.FechaUltimoAguinaldo)),
                                  DiasIndemnizacion = diasVacaciones(o.ID, o.FechaIngreso),
                                  SalarioDiario = Decimal.Round(Convert.ToDecimal(o.Salario) / 30m, 2, MidpointRounding.AwayFromZero),
                                  MontoIndemnizacion = GetIndemnizacion(Convert.ToDecimal(o.Salario), o.FechaIngreso.Value)

                              };
                    grid.DataSource = vac;
                    bandedGridView1.RefreshData();

                    //var vac = from o in obj
                    //          select new
                    //          {
                    //              ID = o.ID,
                    //              Codigo = o.Codigo,
                    //              Cargo = o.CargoNombre,
                    //              Empleado = o.Empleado,
                    //              Planilla = o.Planilla,
                    //              Fecha = this.Fecha.Date,
                    //              o.FechaIngreso,
                    //              Dias = diasVacaciones(o.ID, o.FechaUltimaVacacion),
                    //              SalarioDiario = Decimal.Round(Convert.ToDecimal(o.Salario) / 30m, 2, MidpointRounding.AwayFromZero),

                    //          };
                    //grid.DataSource = vac.Where(o => o.Dias > 0m);

                    //var obj = from em in db.Empleados
                    //          join pl in db.Planillas on em.PlanillaID equals pl.ID
                    //          where (!em.FechaSalida.HasValue || Convert.ToDateTime(em.FechaSalida) >= Fecha) && Convert.ToDateTime(em.FechaIngreso).Date <= Fecha
                    //          select new
                    //          {
                    //              em.ID,
                    //              em.Nombres,
                    //              em.Apellidos,
                    //              Planilla = pl.Nombre,
                    //              em.FechaIngreso,
                    //              em.FechaUltimoAguinaldo,
                    //              em.FechaUltimaVacacion,
                    //              SalarioActual = em.SalarioActual + em.Bono
                    //          };

                }
                

                //var obj = from em in db.Empleados
                //          join pl in db.Planillas on em.PlanillaID equals pl.ID
                //          join es in db.EstacionServicios on pl.EstacionServicioID equals es.ID
                //          where em.Activo && (!em.FechaSalida.HasValue || Convert.ToDateTime(em.FechaSalida) >= Fecha) && Convert.ToDateTime(em.FechaIngreso).Date <= Fecha && (es.ID == ESID || ESID == 0)
                //          select new
                //              {
                //                  em.ID,
                //                  em.Nombres,
                //                  em.Apellidos,
                //                  Planilla = pl.Nombre,
                //                  em.FechaIngreso,
                //                  em.FechaUltimaVacacion,
                //                  SalarioActual = em.SalarioActual// + em.Bono
                //              };

                
                //gvData.BestFitColumns(true);
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {

            Parametros.General.FechaVenta = Fecha.Date == null ? db.GetDateServer().Value : Fecha.Date;
            Parametros.General.PreviewHoliday(false, "Reporte Prestaciones Acumuladas", this.lePlanilla.Text, this.MdiParent, null, null, grid, false, 25, 25, 140, 50, 1);
        }

        private void btnPDF_Click(object sender, EventArgs e)
        {
            Parametros.General.FechaVenta = Fecha.Date == null ? db.GetDateServer().Value : Fecha.Date;
            Parametros.General.PreviewHoliday(false, "Reporte Prestaciones Acumuladas", this.lePlanilla.Text, this.MdiParent, null, null, grid, false, 25, 25, 140, 50, 2);
      
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            Parametros.General.FechaVenta = Fecha.Date == null ? db.GetDateServer().Value : Fecha.Date;
            Parametros.General.PreviewHoliday(true, "Reporte Prestaciones Acumuladas", this.lePlanilla.Text, this.MdiParent, null, null, grid, true, 25, 25, 140, 50, 0);
        }

       
        private void tgSwTipo_Toggled(object sender, EventArgs e)
        {
             if (tgSwTipo.IsOn)
            {
                splitMain.PanelVisibility = SplitPanelVisibility.Panel2;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lciFechaCorte.Text = "Fecha Final";
                this.Text = "Liquidaciones";
            }
            else
            {
                splitMain.PanelVisibility = SplitPanelVisibility.Panel1;
                layoutControlItem6.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lciFechaCorte.Text = "Fecha Corte";
                this.Text = "Prestaciones";
            }
        }

         #endregion

    }
}
