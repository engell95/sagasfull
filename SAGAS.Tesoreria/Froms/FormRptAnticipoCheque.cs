using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormRptAnticipoCheque : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        internal DateTime _FechaServer;
        private BackgroundWorker bgw;
        private List<Parametros.ListIdDisplayCodeBool> EtProveedor = new List<Parametros.ListIdDisplayCodeBool>();

        Reportes.Tesoreria.Hojas.RptSaldoAnticipo Mrep;

        #endregion

        #region *** INICIO ***

        public FormRptAnticipoCheque()
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
        
                _FechaServer = Convert.ToDateTime(db.GetDateServer()).Date;
                dateFin.DateTime = _FechaServer;

                //--PROVEEDORES
                EtProveedor = (from p in db.Proveedors
                            select new Parametros.ListIdDisplayCodeBool
                            {
                                ID = p.ID,
                                Codigo = p.Codigo,
                                Display = p.Nombre,
                                valor = false
                            }).ToList();

                this.bdsDetalleG.DataSource = this.EtProveedor;
                gridG.DataSource = bdsDetalleG;

                //Estaciones de Servicio
                var lista = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                             select new { es.ID, es.Nombre }).ToList();
                lista.Insert(0, new { ID = 0, Nombre = "TODAS" });

                lkES.Properties.DataSource = lista;
                lkES.EditValue = Parametros.General.EstacionServicioID;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
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
                if (EtProveedor.Count(c => c.valor) > 0)
                {
                    bgw = new BackgroundWorker();
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.RunWorkerAsync();
                }
                else
                    Parametros.General.DialogMsg("Favor seleccionar un Proveedor.", Parametros.MsgType.warning);
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
                Reportes.Tesoreria.Hojas.RptSaldoAnticipo rep = new Reportes.Tesoreria.Hojas.RptSaldoAnticipo();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                //DateTime FechaInicial, FechaFinal;
                //FechaInicial = new DateTime(Convert.ToInt32(dateFecha.DateTime.Year), Convert.ToInt32(dateFecha.DateTime.Month), 01);
                //FechaFinal = dateFecha.DateTime;
                rep.xrSetDate.Text = _FechaServer.ToShortDateString();
                rep.xrCeRango.Text = " Hasta " + dateFin.DateTime.Date.ToShortDateString();

                rep.RequestParameters = false;

                bool SUS = (lkSES.EditValue == null ? false : true);

                List<Entidad.GetChequesAnticiposResult> Anticipo = dv.GetChequesAnticipos(Convert.ToDateTime(dateFin.DateTime).Date).ToList();

                rep.DataSource = from o in Anticipo
                                 where EtProveedor.Where(m => m.valor).Select(s => s.ID).Contains(o.ProveedorID) && (chkPagado.Checked.Equals(false) ? o.Pagado.Equals(false) : chkPagado.Checked.Equals(true))
                                 && (o.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                        && (!SUS || (SUS && ((o.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                          // && (o.FechaRegistro.Date >= Convert.ToDateTime(dateInicio.DateTime).Date && o.FechaRegistro.Date <= Convert.ToDateTime(dateFin.DateTime).Date)
                                 select o;

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
                    Mrep = ((Reportes.Tesoreria.Hojas.RptSaldoAnticipo)(e.Result));
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



        private void tgSwGestor_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (tgSwGestor.IsOn)
                    EtProveedor.ForEach(p => p.valor = true);
                else
                    EtProveedor.ForEach(p => p.valor = false);

                gvDataG.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            if (lkES.EditValue != null)
            {
                if (Convert.ToInt32(lkES.EditValue).Equals(0))
                {
                    this.lkSES.EditValue = null;
                    this.lkSES.Properties.DataSource = null;
                    this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else if (!Convert.ToInt32(lkES.EditValue).Equals(0))
                {
                    var ES = db.EstacionServicios.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkES.EditValue)));

                    if (ES != null)
                    {
                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).Select(s => new { s.ID, s.Nombre })).ToList();

                        if (Sus.Count > 0)
                        {
                            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            Sus.Insert(0, new { ID = 0, Nombre = "TODOS" });
                            lkSES.Properties.DataSource = Sus;
                        }
                        else
                        {
                            this.lkSES.EditValue = null;
                            this.lkSES.Properties.DataSource = null;
                            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        }

                    }
                    else
                    {
                        this.lkSES.EditValue = null;
                        this.lkSES.Properties.DataSource = null;
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                }
            }
            else
            {
                this.lkSES.EditValue = null;
                this.lkSES.Properties.DataSource = null;
                this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

    }
}