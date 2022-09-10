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

namespace SAGAS.Ventas.Forms
{
    public partial class FormRptEstadoCuentaCliente : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private int Usuario = Parametros.General.UserID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<StEstaciones> EtEstaciones = new List<StEstaciones>();
        private List<StSubEstaciones> EtSubEstaciones = new List<StSubEstaciones>();
        private List<Entidad.GetBalanzaComprobacionResult> EtVista;
        private BackgroundWorker bgw;
        Reportes.Contabilidad.Hojas.RptBalanzaComprobacion rep;

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


        #endregion

        #region *** INICIO ***

        public FormRptEstadoCuentaCliente()
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


               var Est = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                 select new StEstaciones 
                                 { ID = es.ID, Nombre = es.Nombre, Activo = es.Activo }).ToList();

                //var SubEst = db.SubEstacions.Where(ses => ses.Activo && Est.Select(es => es.ID).Contains(ses.EstacionServicioID)).Select(s => new StSubEstaciones { ID = s.ID, Nombre = s.Nombre, EstacionServicioID = s.EstacionServicioID, Activo = s.Activo }).ToList();
                
                Est.Insert(0, new StEstaciones { ID = 0, Nombre = "TODOS", Activo = true });

                //if (SubEst.Count > 0)
                //    SubEst.Insert(0, new StSubEstaciones { ID = 0, Nombre = "TODOS", EstacionServicioID = 0, Activo = true });

                EtEstaciones = Est.ToList();
                //EtSubEstaciones = SubEst.ToList();
                
                //LOOKUPS
                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre});
                IDEstacion = Parametros.General.EstacionServicioID;

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;
                lkMesInicial_Validated(null, null);
                lkMesFinal.EditValue = DateTime.Now.Month;
                lkMesFinal_Validated(null, null);
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
        
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            //if (lkES.EditValue != null)
            //{
            //    if (IDEstacion > 0)
            //    {
            //        var Sus = EtSubEstaciones.Where(sus => sus.EstacionServicioID.Equals(IDEstacion) || sus.EstacionServicioID.Equals(0)).ToList();

            //        if (Sus.Count(c => !c.EstacionServicioID.Equals(0))  > 0)
            //        {
            //            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            //            lkSUS.Properties.DataSource = Sus.Select(s => new { s.ID, s.Nombre });
            //            lkSUS.EditValue = null;
            //        }
            //        else
            //        {
            //            this.lkSUS.EditValue = null;
            //            this.lkSUS.Properties.DataSource = null;
            //            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

            //        }
            //    }
            //}
            //else
            //{
            //    this.lkSUS.EditValue = null;
            //    this.lkSUS.Properties.DataSource = null;
            //    this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}
        }
           
        private void lkMesInicial_Validated(object sender, EventArgs e)
        {
            if (lkMesInicial.EditValue != null)
            {
                dateInicial.EditValue = Convert.ToDateTime(new DateTime(DateTime.Now.Year, Convert.ToInt32(lkMesInicial.EditValue), 1));
            }
        }

        private void lkMesFinal_Validated(object sender, EventArgs e)
        {
            if (lkMesFinal.EditValue != null)
            {
                int mes = (Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? 1 : Convert.ToInt32(lkMesFinal.EditValue) + 1);
                DateTime fecha = new DateTime((Convert.ToInt32(lkMesFinal.EditValue).Equals(12) ? DateTime.Now.AddYears(1).Year : DateTime.Now.Year), mes, 1);
                dateFinal.EditValue = fecha.AddDays(-1);
            }
        }

        private void dateInicial_EditValueChanged(object sender, EventArgs e)
        {
            if (dateInicial.EditValue != null)
            {
                lkMesInicial.EditValue = Convert.ToDateTime(dateInicial.EditValue).Month;
            }
        }

        private void dateFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (dateFinal.EditValue != null)
            {
                lkMesFinal.EditValue = Convert.ToDateTime(dateFinal.EditValue).Month;
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

                Reportes.Contabilidad.Hojas.RptBalanzaComprobacion MRep = new Reportes.Contabilidad.Hojas.RptBalanzaComprobacion();

                string Nombre, Direccion, Telefono, Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);

                MRep.xrSetDate.Text = _Fecha.ToShortDateString() + "  " + _Fecha.ToShortTimeString();
                MRep.PicLogo.Image = picture_LogoEmpresa;
                MRep.CeEmpresa.Text = Nombre;
                MRep.CeRuc.Text = Ruc;
                MRep.xrCeRango.Text = "Del " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + "   Al   " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                
                int ES = (int)lkES.EditValue;
                if (ES.Equals(0))
                {
                    EtVista = new List<Entidad.GetBalanzaComprobacionResult>();

                    foreach (var Sede in EtEstaciones)
                    {
                        EtVista.AddRange(dbView.GetBalanzaComprobacion(Sede.ID, 0,true, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList());
                    }
                }
                else
                {
                    EtVista = dbView.GetBalanzaComprobacion(ES, 0,true, Convert.ToDateTime(dateInicial.EditValue), Convert.ToDateTime(dateFinal.EditValue)).OrderBy(o => o.CuentaCodigo).ToList();
                }

                MRep.DataSource = EtVista;
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
                    rep = ((Reportes.Contabilidad.Hojas.RptBalanzaComprobacion)(e.Result));
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


        #endregion
    }
}