using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.Contabilidad.Forms;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogCierre : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCierre MDI; 
        internal Entidad.PeriodoContable EtPeriodoAnterior;
        internal Entidad.PeriodoContable EtPeriodo;
        internal Entidad.PeriodoContable EtPeriodoPosterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;
        private DateTime fechaAnterior, fechaCierre, fechaApertura;


        public DateTime FechaFin
        {
            get ;
            set ;
        }

        public DateTime FechaInicio
        {
            get ;
            set ;
        }
        
        public int EstacionID
        {
            get { return Convert.ToInt32(lkeEstacionServicio.EditValue); }
            set { lkeEstacionServicio.EditValue = value; }
        }

        public bool Cerrado {
            get { return ckCerrado.Checked; }
            set { ckCerrado.Checked = value; }
        }

        public bool Consolidado
        {
            get { return ckConsolidado.Checked; }
            set { ckConsolidado.Checked = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogCierre(int UserID, int IDES)
        {
            InitializeComponent();
            UsuarioID = UserID;
            EstacionID = IDES;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        struct ListaPeriodoContableEstacionesServicio
        {
            public int ID;
            public string Codigo;
            public string Nombre;
            public string Display;
            public bool Activo;
            public bool Cerrado;
            public bool Consolidado;
            public DateTime FechaInicio;
            public DateTime FechaFin;
        }
        
        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                List<ListaPeriodoContableEstacionesServicio> estaciones = (from es in db.EstacionServicios
                                                join pc in db.PeriodoContables on es.ID equals pc.EstacionID
                                                where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                                                           select new ListaPeriodoContableEstacionesServicio { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo, FechaInicio = pc.FechaInicio, FechaFin = pc.FechaFin, Cerrado = pc.Cerrado, Consolidado = pc.Consolidado }).ToList();
                

                lkeEstacionServicio.Properties.DataSource = from es in db.EstacionServicios where es.Activo select new {es.ID, es.Nombre };
               //lkeEstacionServicio.EditValue = Parametros.General.EstacionServicioID;

                EtPeriodoAnterior = db.PeriodoContables.Where(c => c.EstacionID.Equals(EstacionID) && c.Cerrado).OrderByDescending(o => o.FechaFin).FirstOrDefault();

                if (EtPeriodoAnterior != null)
                {
                    FechaInicio = EtPeriodoAnterior.FechaFin.Date;
                    ckCerrado.Checked = EtPeriodoAnterior.Cerrado;
                    ckConsolidado.Checked = EtPeriodoAnterior.Consolidado;
                }
                string Mes = (FechaInicio.Date > Convert.ToDateTime("01/01/2000").Date ? Parametros.General.GetMonthInLetters(FechaInicio.Month) : "N/A");
                txtPeriodoAnterior.Text = Mes + " - " + (FechaInicio > Convert.ToDateTime("01/01/2000").Date ? FechaInicio.Year.ToString() : "N/A");
                fechaAnterior = FechaInicio;

                EtPeriodo = db.PeriodoContables.Where(c => c.EstacionID.Equals(EstacionID) && !c.Cerrado).OrderBy(o => o.FechaFin).FirstOrDefault();

                if (EtPeriodo != null)
                    fechaCierre = EtPeriodo.FechaFin.Date;

                string MesActual = Parametros.General.GetMonthInLetters(fechaCierre.Month);
                dtPeriodoCerrar.Text = MesActual + " - " + fechaCierre.Year; //GetEndOfMonth(FechaInicio)

                
                //fechaCierre = dtPeriodoCerrar.DateTime;
                //fechaApertura = GetNextMonth(dtPeriodoCerrar.DateTime);

                //txtPeriodoApertura.Text = "";
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtPeriodoAnterior, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkeEstacionServicio, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtPeriodoAnterior.Text == "" || lkeEstacionServicio.EditValue == null || txtPeriodoApertura.Text == "" || btnCrear.Enabled == true || EtPeriodoPosterior == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.CommandTimeout = 600;
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    if (EtPeriodo != null)
                    {
                        Entidad.PeriodoContable PC;

                        PC = db.PeriodoContables.Single(es => es.ID.Equals(EtPeriodo.ID));

                        PC.Cerrado = true;


                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se cerró el Periódo Contable " + PC.NombreEstacion + " del: " + PC.FechaInicio + " al " + PC.FechaFin, this.Name);
                        db.SubmitChanges();
                    }

                    Entidad.PeriodoContable PCNuevo;


                    var Et = db.PeriodoContables.FirstOrDefault(c => c.EstacionID.Equals(EtPeriodoPosterior.EstacionID) && c.FechaInicio.Date.Equals(EtPeriodoPosterior.FechaInicio.Date) && c.FechaFin.Date.Equals(EtPeriodoPosterior.FechaFin.Date));

                    if (Et == null)
                    {
                        PCNuevo = new Entidad.PeriodoContable();

                        PCNuevo.EstacionID = EstacionID;
                        PCNuevo.FechaInicio = EtPeriodoPosterior.FechaInicio;
                        PCNuevo.FechaFin = EtPeriodoPosterior.FechaFin;
                        PCNuevo.NombreEstacion = EtPeriodoPosterior.NombreEstacion;
                        db.PeriodoContables.InsertOnSubmit(PCNuevo);
                    }
                    else
                    {

                        PCNuevo = db.PeriodoContables.Single(es => es.EstacionID.Equals(EtPeriodoPosterior.EstacionID) && es.FechaInicio.Date.Equals(EtPeriodoPosterior.FechaInicio.Date) && es.FechaFin.Date.Equals(EtPeriodoPosterior.FechaFin.Date));
                        PCNuevo.Cerrado = false;
                        db.SubmitChanges();

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se creó el Periódo Contable " + PCNuevo.NombreEstacion + " del: " + PCNuevo.FechaInicio + " al " + PCNuevo.FechaFin, this.Name);
                    }


                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        public static DateTime GetNextMonth(DateTime fecha)
        {
            DateTime date = new DateTime();
            try
            {
                //int dia = fecha.Day;
                //if (dia <= 15)
                //    date = new DateTime(fecha.Year, fecha.Month, 15);
                //else
                //{
                    ////date = new DateTime(fecha.Year, fecha.Month + 1, 1);                
                    date = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
                    
                    if (date.Month == 1)
                        date.AddYears(1);
                    //date = date.AddDays(-1); //Ultimo dia del mes
                //}
                return date;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return date;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                if (EtPeriodoPosterior == null)
                {
                    if (EtPeriodo != null)
                    {
                        EtPeriodoPosterior = null;
                        EtPeriodoPosterior = new Entidad.PeriodoContable();
                        EtPeriodoPosterior.EstacionID = EstacionID;
                        EtPeriodoPosterior.FechaInicio = EtPeriodo.FechaFin.AddDays(1);
                        EtPeriodoPosterior.FechaFin = EtPeriodoPosterior.FechaInicio.AddMonths(1).AddDays(-1);
                        EtPeriodoPosterior.NombreEstacion = lkeEstacionServicio.Text;
                    }
                    else
                    {
                        using (Contabilidad.Forms.Dialogs.DialogGetFecha dg = new Contabilidad.Forms.Dialogs.DialogGetFecha(true))
                        {
                            if (dg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                string MesActual = Parametros.General.GetMonthInLetters(new DateTime(dg.Fecha.Year, dg.Fecha.Month, 1).Date.AddDays(-1).Month);
                                dtPeriodoCerrar.Text = MesActual + " - " + new DateTime(dg.Fecha.Year, dg.Fecha.Month, 1).Date.AddDays(-1).Year;

                                EtPeriodoPosterior = null;
                                EtPeriodoPosterior = new Entidad.PeriodoContable();
                                EtPeriodoPosterior.EstacionID = EstacionID;
                                EtPeriodoPosterior.FechaInicio = new DateTime(dg.Fecha.Year, dg.Fecha.Month, 1).Date;
                                EtPeriodoPosterior.FechaFin = new DateTime(dg.Fecha.AddMonths(1).Year, dg.Fecha.AddMonths(1).Month, 1).AddDays(-1).Date;
                                EtPeriodoPosterior.NombreEstacion = lkeEstacionServicio.Text;
                            }
                            else
                            {
                                EtPeriodoPosterior = null;
                                Parametros.General.DialogMsg("El Periodo Contable no fue creado.", Parametros.MsgType.warning);
                                return;
                            }
                        }
                    }
                    string Mes = Parametros.General.GetMonthInLetters(EtPeriodoPosterior.FechaFin.Month);
                    txtPeriodoApertura.Text = Mes + " - " + EtPeriodoPosterior.FechaFin.Year;
                    btnCrear.Enabled = false;
                }
            }
            catch (Exception ex)
            { Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString()); }
        }

        #endregion

        private void dtPeriodoCerrar_EditValueChanged(object sender, EventArgs e)
        {
            //string Mes;
            //fechaCierre = dtPeriodoCerrar.DateTime;

            //Mes = Parametros.General.GetMonthInLetters(fechaCierre.Month);
            //dtPeriodoCerrar.Text = Mes + " - " + fechaCierre.Year;
            //Mes = Parametros.General.GetMonthInLetters(GetNextMonth(Convert.ToDateTime(dtPeriodoCerrar.EditValue)).Month);
            //txtPeriodoApertura.Text = Mes + " - " + GetNextMonth(Convert.ToDateTime(dtPeriodoCerrar.EditValue)).Year;
        }

    }
}