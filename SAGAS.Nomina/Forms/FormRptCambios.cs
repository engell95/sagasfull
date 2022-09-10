using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SAGAS.Reportes.Nomina.Hojas;

namespace SAGAS.Nomina.Forms
{
    public partial class FormRptCambios : Form
    {
        #region *** DECLARACIONES ***
        //-********************************************************************-
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        internal DateTime _FechaServer;
        private BackgroundWorker bgw;
        //private List<Parametros.ListGrupoCuadreProducto> EtGrupoCuadreProducto;
        //private List<Parametros.ListGrupoCuadreGasto> EtGrupoCuadreGasto;
        //private List<Parametros.ListGrupoCuadre> EtGrupoCuadre;        
        Reportes.Nomina.Hojas.RptCambiosEmpleados Mrep;

        private int IDPlanilla
        {
            get { return Convert.ToInt32(lkSede.EditValue); }
            set { lkSede.EditValue = value; }
        }

        private DateTime FechaInicial
        {
            get { return Convert.ToDateTime(dateDesde.EditValue); }
            set { dateHasta.EditValue = value; }
        }
        //-*******************************************************************-
        private DateTime FechaFinal
        {
            get { return Convert.ToDateTime(dateHasta.EditValue); }
            set { dateHasta.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public FormRptCambios()
        {
            InitializeComponent();            
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

                lkSede.Properties.DataSource = db.Planillas.Where(o => o.Activo).Select(s => new { ID = s.ID, Display = s.Nombre }).ToList();
                _FechaServer = Convert.ToDateTime(db.GetDateServer()).Date;
                dateDesde.DateTime = dateHasta.DateTime = _FechaServer;
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
            //if (lkSede.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar la planilla.", Parametros.MsgType.warning);
            //    return false;
            //}

            if (dateDesde.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
                return false;
            }
           
            if (Convert.ToDateTime(dateDesde.EditValue) > Convert.ToDateTime(dateHasta.EditValue))
            {
                Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                return false;
            }
            
            return true;
        }

        #endregion

        #region *** EVENTOS ***

        private void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Parametros.General.PrintExportComponet(true, this.Text, this.MdiParent, null, null, DataGrid, false, 25, 25, 140, 50, 0);
        }

        private void btnExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.dglExportToFile.Filter = "Hoja de Microsoft Excel (*.xlsx)|*.xlsx";

            if (this.dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.dglExportToFile.FileName != "")
                {
                    this.DataGrid.MainView.ExportToXlsx(this.dglExportToFile.FileName);

                }
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
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dv.CommandTimeout = 900;
                Reportes.Nomina.Hojas.RptCambiosEmpleados rep = new Reportes.Nomina.Hojas.RptCambiosEmpleados();

                string Nombre, Direccion, Telefono,Ruc;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyDataRuc(out Nombre, out Direccion, out Telefono, out Ruc, out picture_LogoEmpresa);

                rep.xrPictureBox1.Image = picture_LogoEmpresa;
                rep.labelCompania.Text = Nombre;
                rep.labelDireccion.Text = Direccion;
                rep.labelRuc.Text = Ruc;
                rep.labelReporte.Text = "Cambios Laborales del   " + FechaInicial.ToShortDateString() + " al "+ FechaFinal.ToShortDateString();
                rep.RequestParameters = false;
                var repds = (from dt in dv.VistaTransladoEmpleados
                             where (dt.Fecha.Date >= FechaInicial.Date && dt.Fecha.Date <= FechaFinal.Date) //&& dt.Planilla.Equals(IDPlanilla)
                             select dt
                            );
                rep.DataSource = repds;
                
                e.Result = rep;
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

                    Mrep = ((Reportes.Nomina.Hojas.RptCambiosEmpleados)(e.Result));
                    Mrep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printControlAreaReport_Load(object sender, EventArgs e)
        {

        }

        

    }
}