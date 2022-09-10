using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraReports.UI;

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormRptImpresionCheques : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        internal DateTime _FechaServer;
        internal int _ID;
        internal Entidad.Movimiento Mov;
        internal List<Entidad.FormatosCheque> EtFormatos = new List<Entidad.FormatosCheque>();
        DevExpress.XtraReports.UI.XtraReport rep;

        #endregion

        #region *** INICIO ***

        public FormRptImpresionCheques()
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


                //Estaciones de Servicio
                var lista = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                             select new { es.ID, es.Nombre }).ToList();
                lkES.Properties.DataSource = lista;
                lkES.EditValue = Parametros.General.EstacionServicioID;

                EtFormatos = db.FormatosCheques.ToList();

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


        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                bool SUS = (lkSES.EditValue == null ? false : true);

                Mov = (from o in db.Movimientos
                       where o.Numero.Equals(Convert.ToInt32(spNumero.Value)) && !o.Anulado && !o.Finalizado && o.MovimientoTipoID.Equals(39)
                       && (o.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
              && (!SUS || (SUS && ((o.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)))))) //|| Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                       select o).FirstOrDefault();

                if (Mov != null)
                {

                    DataTable dt = new DataTable();

                    using (SqlConnection conn = new SqlConnection(Parametros.Config.GetCadenaConexionString()))
                    {
                        conn.Open();
                        // Creates a SQL command
                        using (var command = new SqlCommand("SELECT * FROM [<Rpt_ImpresionCheques] where MovimientoID = @ID", conn))
                        {
                            //parametro del ID del movimiento Cheque
                            command.Parameters.Add("@ID", SqlDbType.Int).Value = Mov.ID;
                            // Loads the query results into the table
                            dt.Load(command.ExecuteReader());
                        }

                    }

                    string CB = db.CuentaBancarias.FirstOrDefault(s => s.ID.Equals(Mov.CuentaBancariaID)).ArchivoNombre;
                    var obj = EtFormatos.FirstOrDefault(o => o.ArchivoNombre.Equals(CB));

                    if (obj != null)
                    {
                        if (obj.Archivo != null)
                        {
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                            rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);
                            rep.DataSource = dt;
                            rep.RequestParameters = false;
                            rep.ShowPrintMarginsWarning = false;
                            rep.CreateDocument();

                            rep.PrintingSystem.SetCommandVisibility(DevExpress.XtraPrinting.PrintingSystemCommand.Open, DevExpress.XtraPrinting.CommandVisibility.None);

                            printControlAreaReport.PrintingSystem = rep.PrintingSystem;

                            //Entidad.Movimiento MP = db.Movimientos.Single(s => s.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                            //MP.Finalizado = true;
                            //db.SubmitChanges();
                        }
                        else
                        { Parametros.General.DialogMsg("La cuenta bancaria no tiene formato de impresión", Parametros.MsgType.warning); }
                    }
                    //{ Parametros.General.DialogMsg("La cuenta bancaria no existe", Parametros.MsgType.warning); }

                    //Reportes.Tesoreria.Hojas.RptSaldoAnticipo rep = new Reportes.Tesoreria.Hojas.RptSaldoAnticipo();

                    //string Nombre, Direccion, Telefono;
                    //System.Drawing.Image picture_LogoEmpresa;
                    //Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                    //rep.PicLogo.Image = picture_LogoEmpresa;
                    //rep.CeEmpresa.Text = Nombre;
                    ////DateTime FechaInicial, FechaFinal;
                    ////FechaInicial = new DateTime(Convert.ToInt32(dateFecha.DateTime.Year), Convert.ToInt32(dateFecha.DateTime.Month), 01);
                    ////FechaFinal = dateFecha.DateTime;
                    //rep.xrCeRango.Text = "Desde " + dateInicio.DateTime.ToShortDateString() + " Al " + dateFin.DateTime.Date.ToShortDateString();

                    //rep.RequestParameters = false;

                    //bool SUS = (lkSES.EditValue == null ? false : true);

                    //rep.DataSource = from o in dv.VistaChequesAnticipos
                    //                 where EtProveedor.Where(m => m.valor).Select(s => s.ID).Contains(o.ProveedorID) && (chkPagado.Checked.Equals(false) ? o.Pagado.Equals(false) : chkPagado.Checked.Equals(true))
                    //                 && (o.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                    //        && (!SUS || (SUS && ((o.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                    //           && (o.FechaRegistro.Date >= Convert.ToDateTime(dateInicio.DateTime).Date && o.FechaRegistro.Date <= Convert.ToDateTime(dateFin.DateTime).Date)
                    //                 select o;

                    //e.Result = rep;
                }
                else
                    Parametros.General.DialogMsg("El Cheque Nro " + spNumero.Value.ToString() + Environment.NewLine + "No es valido para ser impreso", Parametros.MsgType.warning);

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }


        #endregion


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

        private void printControlAreaReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.P) //detect key ctrl+p
                {
                    e.SuppressKeyPress = true; //<= Set it to true.
                    //MessageBox.Show("This Document is Protected !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void FormRptImpresionCheques_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.P) //detect key ctrl+p
                {
                    e.SuppressKeyPress = true; //<= Set it to true.
                    //MessageBox.Show("This Document is Protected !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if ((Keys.Control | Keys.P) == keyData)
            {
                btnPrint_Click(null, null);
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }

        

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               
                Entidad.Movimiento MP = db.Movimientos.Single(s => s.ID.Equals(Mov.ID));
                rep.Print();
                MP.Finalizado = true;
                db.SubmitChanges();

                Mov = null;
                spNumero.Value = 0;
                printControlAreaReport.PrintingSystem = null;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

    }
}