using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;

namespace SAGAS.Nomina.Forms
{
    public partial class FormLiquidacionesEmpleados : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>
        private Entidad.SAGASDataClassesDataContext db;
        private Forms.Wizards.wizLiquidacion nf;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private int MonedaID = Parametros.Config.MonedaPrincipal();
        private List<int> lista;
        #endregion

        public FormLiquidacionesEmpleados()
        {
            InitializeComponent();
            this.btnAgregar.Caption = "Crear";
            this.barImprimir.Caption = "Vista Previa";
            this.btnAnular.Caption = "Anular Liquidación";
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            //this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaPlanillaGenerada);
            //this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                bdsManejadorDatos.DataSource = from me in dv.VistaLiquidacions
                                               select me;

                this.grid.DataSource = bdsManejadorDatos;

                //**Zonas

                /*chkAplicaRetencion.f = from p in db.Planillas
                                         where p.Activo
                                        select new { p.ID, p.FechaFin };

                chkAplicaRetencion.DisplayMember = "FechaFin";
                chkAplicaRetencion.ValueMember = "ID";*/

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Wizards.wizLiquidacion();
                    nf.Text = "Crear Nueva Liquidación";
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

        protected override void Del()
        {
            if ( gvForm.FocusedRowHandle>= 0)
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    eliminar_registro(Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
                    FillControl();
                }
            }
        }

        void eliminar_registro(int index){
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    var direccion = (from l in db.Liquidacion
                                     where l.ID.Equals(index)
                                     select new { id_empleado = l.EmpleadoID, id_liquidacion = l.ID }).First();

                    Entidad.Empleado E = db.Empleados.Single(m => m.ID.Equals(direccion.id_empleado));
                    E.FechaSalida = null;
                    E.Activo = true;
                    
                    db.SubmitChanges();

                    Entidad.Liquidacion L = db.Liquidacion.Single(m => m.ID.Equals(direccion.id_liquidacion));
                    db.Liquidacion.DeleteOnSubmit(L);
                    
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
        protected override void Imprimir()
        {
            if (gvForm.FocusedRowHandle >= 0)
            ShowReportLiquidacion(Convert.ToInt32(gvForm.GetFocusedRowCellValue(colID)));
        }

        private void ShowReportLiquidacion(int ID)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaLiquidacions.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().EmpleadoNombres + " " + VM.First().EmpleadoApellidos;

                Reportes.Nomina.Hojas.LiquidacionZ rep = new Reportes.Nomina.Hojas.LiquidacionZ();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.pbLogo.Image = picture_LogoEmpresa;

                if (VM.FirstOrDefault().CalculoSalarioPromedioAguinaldo)
                {
                    rep.xrCellSalario.Text = "Salario Promedio:";
                }
                else
                {
                    rep.xrCellSalario.Text = "Salario Básico:";
                }

                int ElaboradoEmpleadoID = Convert.ToInt32(db.Configuracions.Where(c => c.Campo.Equals("ElaboradoLiquidacionEmpleadoID")).FirstOrDefault().Valor);

                var nombre = (from e in db.Empleados
                              join c in db.Cargo on e.CargoID equals c.ID
                              where e.ID.Equals(ElaboradoEmpleadoID)
                              select new { texto = e.Nombres + " " + e.Apellidos, cargo = c.Nombre }).ToList();

                if (nombre.Count > 0)
                    rep.xrElaboradoNombre.Text = nombre.FirstOrDefault().texto + Environment.NewLine + nombre.FirstOrDefault().cargo;
                

                rep.DataSource = VM;


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

        protected override void CleanFilter()
        {
            this.gvForm.ActiveFilter.Clear();
        }

        internal void CleanDialog(bool RefreshMDI)
        {
            nf = null;

            if (RefreshMDI)
                FillControl();
        }

        #endregion

        private void CargarLiquidacionesByTipo(bool pmostrarinss, bool pdolarizada)
        {
            //MostrarLiquidacionInss = pmostrarinss;
            
            //var result = (from i in db.ListLiquidacion
            //              select new
            //              {
            //                  i.ID,
            //                  i.Codigo,i.NombreEmpleado,i.IdCargo,i.NombreCargo,i.NumSeguroSocial,
            //                  i.Cedula,i.Motivo,i.FechaDesde,i.FechaFinal
            //                  ,
            //                  SalarioBase = decimal.Round(((pmostrarinss ? i.SalarioBaseGrabable : i.SalarioBase) * (pdolarizada ? (i.Dolarizado ? 1 : (1 / i.TipoCambio)) : (i.Dolarizado ? i.TipoCambio : 1 ))),2)
            //                  ,
            //                  PagoNeto = decimal.Round(((pmostrarinss ? i.PagoNetoGrabable : i.PagoNeto) * (pdolarizada ? (i.Dolarizado ? 1 : (1 / i.TipoCambio)) : (i.Dolarizado ? i.TipoCambio : 1))),2)
            //              }
            //              );

            //grid.DataSource = result.OrderByDescending(c=> c.FechaFinal);
            //gvForm.RefreshData();
            
        }

        private void FormLiquidaciones_Load(object sender, EventArgs e)
        {
            FillControl();
            //CargarLiquidacionesByTipo(true, false);
            //ckMostrarLiquidacionExtra.Visible = Classes.Global.PermitePlanillaConfidencial();
            //ckMostrarDolares.Visible = Classes.Global.PermiteSalarioDolares();
            //if (Classes.Global.ManejarCodigoEmpleado())
            //    ColCodigo.Visible = true;
            //else ColCodigo.Visible = false;
        }

        //private List<decimal> CalcularProvision13vo(decimal salario, DateTime fechaInicio, DateTime fechaFinal)
        //{
            //AdmonP.Entidades.ListLiquidacion liq = gvForm.GetFocusedRow() as AdmonP.Entidades.ListLiquidacion;

            //List<decimal> valores = new List<decimal>();
            //List<int> meses = new List<int>();
            //meses.Add(12);
            //meses.Add(1);
            //meses.Add(2);
            //meses.Add(3);
            //meses.Add(4);
            //meses.Add(5);
            //meses.Add(6);
            //meses.Add(7);
            //meses.Add(8);
            //meses.Add(9);
            //meses.Add(10);
            //meses.Add(11);

            //int days = 0, month = 0, year = 0;

            //Classes.Util.RangoFechasYMD(fechaFinal, fechaInicio, out year, out month, out days);

            //decimal diasAguinaldo = year * Classes.Global.valorAnualVacaciones;
            //diasAguinaldo += month * Classes.Global.valorMensualVacaciones;
            //diasAguinaldo += days * Classes.Global.valorDiarioVacaciones;

            //DateTime fecha = fechaInicio;
            //decimal acum = 0;
            //foreach (decimal mes in meses)
            //{
            //    decimal valor = 0;

            //    if (mes == fecha.Month)
            //    {
            //        if ((fecha < fechaFinal) && (diasAguinaldo - acum) > 2.5m)
            //        {
            //            int day = fecha.Day;
            //            if (day > 1) //mes no completo
            //            {
            //                valor = (day * 2.5m) / 30m;
            //                if (valor > 2.5m) valor = 2.5m;
            //            }
            //            else valor = 2.5m;
            //        }
            //        else
            //        {
            //            valor = diasAguinaldo - acum;
            //        }

            //        fecha = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
            //    }

            //    valores.Add(valor);
            //    acum += valor;
            //}
            //valores.Add(diasAguinaldo);
            //valores.Add(liq.Aguinaldo);
            //return valores;
        //}


        //private AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable DatosVacaciones(int liquidacion)
        //{
        //    AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable data = new AdmonP.Reportes.DataSets.DSReports.CalculoVacacionesDataTable();
        //    var row = data.NewCalculoVacacionesRow();

        //    var liq = db.Liquidacion.Single(l => l.ID == liquidacion);
        //    DateTime fechaInicio = liq.FechaDesde, fechaFin = liq.FechaFinVaciones;

        //    int intervaloMeses =(int) Classes.DateAndTime.DateDiff(AdmonP.Classes.DateInterval.Month, fechaInicio, fechaFin);
        //    decimal acum = 0, diasPag = liq.DiasVacaciones, acumPag = 0;
        //    for (int i = -6; i < 0; i++)
        //    {
        //        decimal valor = 0, valorPag =0;
        //        string nomMes = "";
        //        DateTime fecha = new DateTime(fechaFin.AddMonths(i).Year, fechaFin.AddMonths(i).Month, 1);
        //        var vacacionesPagadas = db.Get_PagoVacacionesByEmpleado(liq.IdEmpleado, fecha, fecha.AddMonths(1).AddDays(-1), true);
        //        if (vacacionesPagadas.Count() > 0)
        //        {
        //            var dt = Classes.Util.LINQToDataTable(vacacionesPagadas.AsEnumerable());
        //            valorPag = Convert.ToDecimal(dt.Rows[0]["DiasDescansados"]);
        //        }
        //        nomMes = fecha.ToString("MMM-yy");
        //        if (fecha > fechaInicio)
        //        {
        //            if ((diasPag - acum) > 2.5m)
        //            {
        //                valor = 2.5m;
        //            }
        //            else
        //                valor = diasPag - acum;
        //        }
        //        // Esto no me Gusta, se debe poder mejorar, El tiempo no me dio para mejorarlo | AL 27/05/2011
        //        if (i == -6)
        //        {
        //            row.Mes1AN = nomMes;
        //            row.Mes1DN = nomMes;
        //            row.Mes1AV = valor;
        //            row.Mes1DV = valorPag;
        //        }
        //        if (i == -5)
        //        {
        //            row.Mes2AN = nomMes;
        //            row.Mes2DN = nomMes;
        //            row.Mes2AV = valor;
        //            row.Mes2DV = valorPag;
        //        }
        //        if (i == -4)
        //        {
        //            row.Mes3AN = nomMes;
        //            row.Mes3DN = nomMes;
        //            row.Mes3AV = valor;
        //            row.Mes3DV = valorPag;
        //        }
        //        if (i == -3)
        //        {
        //            row.Mes4AN = nomMes;
        //            row.Mes4DN = nomMes;
        //            row.Mes4AV = valor;
        //            row.Mes4DV = valorPag;
        //        }
        //        if (i == -2)
        //        {
        //            row.Mes5AN = nomMes;
        //            row.Mes5DN = nomMes;
        //            row.Mes5AV = valor;
        //            row.Mes5DV = valorPag;
        //        }
        //        if (i == -1)
        //        {
        //            row.Mes6AN = nomMes;
        //            row.Mes6DN = nomMes;
        //            row.Mes6AV = valor;
        //            row.Mes6DV = valorPag;
        //        }
        //        acum += valor;
        //        acumPag += valorPag;
        //    }
        //    row.SaldoAcum = diasPag - (acum - acumPag);
        //    row.Saldo = diasPag;
        //    data.AddCalculoVacacionesRow(row);
        //    return data;
        //}
    

        private void ShowReportLiquidacion()
        {
            //try
            //{
            //    int idLiquidacion = Convert.ToInt32(gvForm.GetFocusedRowCellValue("ID"));
            //    AdmonP.Entidades.DataManager dm = new AdmonP.Entidades.DataManager(Classes.Global.GetConnectionString());
            //    AdmonP.Reportes.DataSets.DSReports ds = dm.GetLiquidacion(idLiquidacion,ckMostrarDolares.Checked);
            //    var liq = db.ListLiquidacion.Where(o => o.ID.Equals((int)gvForm.GetFocusedRowCellValue("ID")));

            //    if (liq.Count() > 0)
            //    {
            //        if (Classes.Global.FormatoEmpresa().Equals(0))
            //        {
            //            AdmonP.WUI.Reports.LiquidacionAdmicon rep = new AdmonP.WUI.Reports.LiquidacionAdmicon();
            //            rep.DataSource = liq;
            //            rep.Name = "Liquidacion " + gvForm.GetFocusedRowCellValue("NombreEmpleado").ToString();
            //            Classes.Global.ShowReport(rep, "Liquidación Final " + gvForm.GetFocusedRowCellValue("NombreEmpleado").ToString());
            //        }
            //        else
            //        {
            //            AdmonP.Reportes.Liquidaciones.LiquidacionZ rep = new AdmonP.Reportes.Liquidaciones.LiquidacionZ();
            //            rep.DataSource = liq;
            //            rep.Name = "Liquidacion " + gvForm.GetFocusedRowCellValue("NombreEmpleado").ToString();
            //            Classes.Global.ShowReport(rep, "Liquidación Final " + gvForm.GetFocusedRowCellValue("NombreEmpleado").ToString());                   
            //        }

            //    }
            //    else
            //    {
            //        Classes.Global.DialogMsg("No hay datos que mostrar", AdmonP.Classes.MsgType.warning);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            
                ShowReportLiquidacion();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //Classes.Global.ExportToXLS(gvForm);
        }

        private void FormLiquidaciones_Shown(object sender, EventArgs e)
        {
            FillControl(false);   
        }

        //private void ckMostrarDolares_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckMostrarDolares.Checked)
        //    {
        //        ckMostrarDolares.Image = AdmonP.Properties.Resources.ok_32;
        //    }
        //    else
        //    {
        //        ckMostrarDolares.Image = AdmonP.Properties.Resources.square2_32;
        //    }
        //    CargarLiquidacionesByTipo(!ckMostrarLiquidacionExtra.Checked, ckMostrarDolares.Checked);
        //}

        //private void ckMostrarLiquidacionExtra_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ckMostrarLiquidacionExtra.Checked)
        //    {
        //        ckMostrarLiquidacionExtra.Image = AdmonP.Properties.Resources.ok_32;               
        //    }
        //    else
        //    {
        //        ckMostrarLiquidacionExtra.Image = AdmonP.Properties.Resources.square2_32;               
        //    }
        //    CargarLiquidacionesByTipo(!ckMostrarLiquidacionExtra.Checked, ckMostrarDolares.Checked);
        //}

        //private void btnEliminar_Click(object sender, EventArgs e)
        //{
        //    if (gvForm.FocusedRowHandle >= 0)
        //    {

        //        if (Classes.Global.DialogMsg(Properties.Resources.MSG_CONFIRMAR_ANULAR, AdmonP.Classes.MsgType.question)
        //            == DialogResult.OK)
        //        {
        //            try
        //            {
        //                AdmonP.Entidades.Liquidacion liq = db.Liquidacion.Single(o => o.ID == Convert.ToInt32(gvForm.GetFocusedRowCellValue("ID")));
        //                AdmonP.Entidades.Empleado empleado = db.Empleado.Single(o => o.IdEmpleado.Equals(liq.IdEmpleado));
        //                empleado.Activo = true;
        //                empleado.FechaSalida = null;

        //                db.LiquidacionDetalleIngreso.DeleteAllOnSubmit(db.LiquidacionDetalleIngreso.Where(o => o.IdLiquidacion.Equals(liq.ID)));
        //                db.LiquidacionDetalleDeduccion.DeleteAllOnSubmit(db.LiquidacionDetalleDeduccion.Where(o => o.IdLiquidacion.Equals(liq.ID)));
        //                db.Liquidacion.DeleteOnSubmit(liq);
        //                db.SubmitChanges();
        //                Classes.Global.DialogMsg("Proceso completado exitosamente.", AdmonP.Classes.MsgType.message);
        //                CargarLiquidacionesByTipo(true, false);

        //            }
        //            catch (Exception ex)
        //            {
        //                Classes.Global.DialogMsg(Properties.Resources.MSG_ERROR, AdmonP.Classes.MsgType.error, ex.ToString());
        //            }
        //        }
                
        //    }
        //}
    }
}
