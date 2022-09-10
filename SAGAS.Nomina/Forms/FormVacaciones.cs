using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormVacaciones : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Wizards.wizPagoVacaciones nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        internal List<int> lista;

        #endregion

        #region <<< INICIO >>>

        public FormVacaciones()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Eliminar";
            this.barImprimir.Caption = "Vista Previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Enabled = false;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                 lista = (db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID)).ToList();

                bdsManejadorDatos.DataSource = (from v in dv.VistaDiasCuentaVacaciones
                                                join em in dv.VistaEmleados on v.EmpleadoID equals em.ID
                                                join gvpbe in dv.GetViewPlanillaByEmpleados on em.ID equals gvpbe.EmpleadoID
                                                where lista.Contains(gvpbe.EstacionServicioID)
                                                select new
                                                                    {
                                                                        v.ID,
                                                                        em.Codigo,
                                                                        em.Nombres,
                                                                        em.Apellidos,
                                                                        v.Descansada,
                                                                        v.FechaDesde,
                                                                        v.FechaHasta,
                                                                        v.Dias,
                                                                        v.Monto,
                                                                        v.EmpleadoID,
                                                                        PlanillaID = gvpbe.ID,
                                                                        gvpbe.Nombre,
                                                                        v.Tipo,
                                                                        v.Observacion
                                                                    }).OrderByDescending(o => o.ID);

                this.grid.DataSource = bdsManejadorDatos;

                DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                this.gvData.ActiveFilterString = (new OperandProperty("FechaHasta") >= fecha).ToString();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue(colDescansada)))
                    ImprimirLicencia(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colEmpleadoID)));
                else
                    ImprimirPago(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)), Convert.ToInt32(gvData.GetFocusedRowCellValue(colEmpleadoID)));
            }
        }

        private void ImprimirLicencia(int ID, int EmpleadoID)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaDiasCuentaVacaciones.Where(m => m.ID.Equals(ID)).ToList();
                var EM = db.Empleados.Single(s => s.ID.Equals(EmpleadoID));
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Vacaciones de " + EM.Nombres + " " + EM.Apellidos;

                Reportes.Nomina.Hojas.LicenciaVacaciones rep = new Reportes.Nomina.Hojas.LicenciaVacaciones();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.pbLogo.Image = picture_LogoEmpresa;
                rep.lblEmpresa.Text = Nombre;
                rep.lblEstacion.Text = db.Planillas.Single(s => s.ID.Equals(EM.PlanillaID)).Nombre;

                rep.xrCellCodigo.Text = EM.Codigo;
                rep.xrCellNombreApellido.Text = EM.Nombres + " " + EM.Apellidos;
                rep.xrCellIngreso.Text = EM.FechaIngreso.Value.ToShortDateString();

                int years = 0, months = 0, daysL = 0;

                Parametros.General.RangoFechasYMD(VM.First().FechaHasta.Date, EM.FechaIngreso.Value.Date, out years, out months, out daysL);

                rep.xrCellTiempoLaborado.Text = (years > 0 ? years.ToString() + " Años,  " : "" ) + months.ToString() + " Meses,  " + daysL.ToString() + " Dias.";
                
                rep.xrCellUbicacion.Text = db.EstructuraOrganizativa.Single(s => s.ID.Equals(EM.EstructuraOrganizativaID)).Nombre;

                rep.DataSource = VM;

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

        private void ImprimirPago(int ID, int EmpleadoID)
        {

            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dbView.VistaPagoVacaciones.Where(m => m.ID.Equals(ID)).ToList();
                var EM = db.Empleados.Single(s => s.ID.Equals(EmpleadoID));
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = "Vacaciones de " + VM.First().Trabajador;

                Reportes.Nomina.Hojas.RptPagoVacaciones rep = new Reportes.Nomina.Hojas.RptPagoVacaciones();
                //db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);
                rep.pbLogo.Image = picture_LogoEmpresa;
                rep.lblEmpresa.Text = Nombre;
                //rep.lblEstacion.Text = db.Planillas.Single(s => s.ID.Equals(EM.PlanillaID)).Nombre;

                rep.lblRuc.Text = Ruc;
                rep.lblDireccion.Text = Direccion;

                //int years = 0, months = 0, daysL = 0;

                //Parametros.General.RangoFechasYMD(VM.First().FechaHasta.Date, EM.FechaIngreso.Value.Date, out years, out months, out daysL, 1);

                //rep.xrCellTiempoLaborado.Text = (years > 0 ? years.ToString() + " Años,  " : "") + months.ToString() + " Meses,  " + daysL.ToString() + " Dias.";

                //rep.xrCellUbicacion.Text = db.EstructuraOrganizativa.Single(s => s.ID.Equals(EM.EstructuraOrganizativaID)).Nombre;

                rep.DataSource = VM;

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


        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl();
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
                    nf = new Forms.Wizards.wizPagoVacaciones(lista);
                    nf.Text = "Crear Vacaciones";
                    nf.lista = lista;
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Edit()
        {
            //try
            //{
            //    if (nf == null)
            //    {
            //        if (gvData.FocusedRowHandle >= 0)
            //        {
            //            nf = new Forms.Wizards.wizPagoVacaciones();
            //            nf.Text = "Editar Vacaciones";
            //            //nf.EtAnterior = db.Cargo.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
            //            nf.Owner = this.Owner;
            //            nf.MdiParent = this.MdiParent;
            //            //nf.Editable = true;
            //            nf.MDI = this;
            //            nf.Show();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //}
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        bool Descanso = Convert.ToBoolean(gvData.GetFocusedRowCellValue(colDescansada));
                        
                        Entidad.DiasVacaciones DV;
                         Entidad.VacacionesPagadas PV;

                         if (Descanso)
                         {
                             DV = db.DiasVacaciones.Single(c => c.ID == id);
                             PV = null;
                         }
                         else
                         {
                             PV = db.VacacionesPagadas.Single(c => c.ID == id);
                             DV = null;
                         }

                         Entidad.MovimientoEmpleado ME;

                         if (DV != null)
                             ME = db.MovimientoEmpleado.Single(s => s.ID.Equals(DV.MovimientoEmpleadoID));
                         else
                             ME = null;

                        Entidad.Empleado EM = db.Empleados.Single(o => o.ID.Equals((DV == null ? PV.EmpleadoID : DV.EmpleadoID)));
                        Entidad.UltimaFechaVacacion UFV;
                            
                        if (DV == null)
                            UFV = db.UltimaFechaVacacions.SingleOrDefault(u => u.VacacionesPagadasID.Equals(PV.ID));
                        else
                            UFV = db.UltimaFechaVacacions.SingleOrDefault(u => u.DiasVacacionesID.Equals(DV.ID));
                            
                        //if(db.UltimaFechaVacacions.Last(l => l.EmpleadoID.Equals(EM.ID)).FechaUltimaVacacion.Date < )


                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            //string Fecha = " desde " + DV.FechaDesde.ToShortDateString() + " hasta " + DV.FechaHasta.ToShortDateString();

                            //#region Actualizar Fecha Ultima de pago de vacaciones

                            //if (UFV != null)
                            //{
                            //    if (EM.FechaUltimaVacacion == null)
                            //        EM.FechaUltimaVacacion = EM.FechaIngreso;

                            //    int year, month, day;
                            //    //AÑOS
                            //    year = Convert.ToInt32(Math.Floor((DV == null ? PV.Dias : DV.Dias) / 30m));

                            //    decimal vmonth = Decimal.Round(((DV == null ? PV.Dias : DV.Dias) / 30m - Math.Floor((DV == null ? PV.Dias : DV.Dias) / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);

                            //    month = Convert.ToInt32(Math.Floor(vmonth));

                            //    decimal vday = vmonth - Math.Floor(vmonth);

                            //    day = Convert.ToInt32(Decimal.Round((vday * 30m), 0, MidpointRounding.AwayFromZero));

                            //    DateTime NuevaVacacion = UFV.AntiguaFechaUltimaVacacion;//EM.FechaUltimaVacacion.Value.Date.AddDays(-Convert.ToDouble(DV.DiasCalendario));//AddYears(-year).AddMonths(-month).AddDays(-day);
                            //    DateTime ViejaFecha = UFV.FechaUltimaVacacion;//EM.FechaUltimaVacacion.Value.Date;

                            //    EM.FechaUltimaVacacion = NuevaVacacion.Date;

                            //    db.UltimaFechaVacacions.DeleteOnSubmit(UFV);
                            //    db.SubmitChanges();
                            //}
                            //#endregion

                            #region Actualizar Fecha Ultima de pago de vacaciones

                            if (EM.FechaUltimaVacacion == null)
                                EM.FechaUltimaVacacion = EM.FechaIngreso;

                            int year, month, day;
                            //AÑOS
                            year = Convert.ToInt32(Math.Floor(DV.Dias / 30m));

                            decimal vmonth = Decimal.Round((DV.Dias / 30m - Math.Floor(DV.Dias / 30m)) * 12m, 2, MidpointRounding.AwayFromZero);

                            month = Convert.ToInt32(Math.Floor(vmonth));

                            decimal vday = vmonth - Math.Floor(vmonth);

                            day = Convert.ToInt32(Decimal.Round((vday * 30m), 0, MidpointRounding.AwayFromZero));

                            DateTime ViejaFecha = EM.FechaUltimaVacacion.Value.Date;
                            DateTime NuevaVacacion = EM.FechaUltimaVacacion.Value.Date;

                            NuevaVacacion = NuevaVacacion.Date.AddYears(-year);
                            NuevaVacacion = NuevaVacacion.Date.AddMonths(-month);

                            int d = (NuevaVacacion.Day > 31 ? 30 : NuevaVacacion.Day) - day;

                            if (!day.Equals(0))
                            {
                                if (d < 0)
                                {
                                    day = Math.Abs(d);
                                    NuevaVacacion = NuevaVacacion.Date.AddMonths(-1);
                                    NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, (30 - day));
                                }
                                else if (d > 0)
                                {
                                    NuevaVacacion = NuevaVacacion.Date.AddDays(-day);
                                }
                                else
                                {
                                    NuevaVacacion = NuevaVacacion.Date.AddMonths(-1);
                                    //NuevaVacacion = new DateTime(NuevaVacacion.Year, NuevaVacacion.Month, 1);
                                }
                            }
                            //else
                            //    NuevaVacacion = NuevaVacacion.Date.AddDays(-1);


                            EM.FechaUltimaVacacion = NuevaVacacion.Date;

                            #endregion
                          

                            if (DV != null)
                                db.DiasVacaciones.DeleteOnSubmit(DV);

                            if (PV != null)
                                db.VacacionesPagadas.DeleteOnSubmit(PV);

                            if (ME != null)
                                db.MovimientoEmpleado.DeleteOnSubmit(ME);

                            db.SubmitChanges();

                            //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                            //"Se Eliminaron las vacaciones de : " + EM.Nombres + " " + EM.Apellidos + Fecha, this.Name);
                            

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();

                            FillControl();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
              
        #endregion

        
    }
}
