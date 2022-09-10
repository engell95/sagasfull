using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using DevExpress.XtraCharts;

namespace SAGAS.Ventas.Forms
{
    public partial class FormCarteraVencidas : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        private int Fila = -1;
        internal int UsuarioID = Parametros.General.UserID;
        List<Entidad.VistaPagadosResultados> ListaRep;
        List<Parametros.ListNombreDisplayValueLimite> Lista;
        private BackgroundWorker bgwOn;
        SAGAS.Reportes.Ventas.Hojas.RptClientesCreditos Mrep;
        //private int Usuario = Parametros.General.UserID;
        //private int IDES = Parametros.General.EstacionServicioID;
        //private int IDEfectivo = Parametros.Config.IDFormaPagoEfectivo();

        #endregion

        #region <<< INICIO >>>

        public FormCarteraVencidas()
        {
            InitializeComponent();
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dateFecha.EditValue = db.GetDateServer();
                lkCuenta.Properties.DataSource = db.CuentaContables.Where(c => c.ID.Equals(58)).Select(s => new { s.ID, Display = s.Codigo + " | " + s.Nombre });
                lkCuenta.EditValue = 58;
                btnLoad_Click(null, null);
            }            

            catch (Exception ex)
            {                
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

       
              
        #endregion    

        #region <<< PIVOT >>>

        private async void btnLoad_Click(object sender, EventArgs e)
        {
            if (dateFecha.EditValue != null)
            {
                await Task.Delay(10);

                await ListaTotales().ConfigureAwait(false);
            }
            else
                Parametros.General.DialogMsg("Favor digitar una Fecha.", Parametros.MsgType.warning);
        }

        async Task ListaTotales()
        {
            try
            {

                await Task.Delay(10);
                dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                dbView.CommandTimeout = db.CommandTimeout = 600;

                List<int> listaES = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == UsuarioID).Select(s => s.EstacionServicioID));
                 

                //LISTA
                ListaRep = (from sp in dbView.MovimientosPagados(Convert.ToDateTime(dateFecha.EditValue).Date, 58)
                            where listaES.Contains(sp.EstacionServicioID)
                            select new Entidad.VistaPagadosResultados
                            {
                                MovimientoID = sp.MovimientoID,
                                ClienteID = sp.ClienteID,
                                ClienteCodigo = sp.ClienteCodigo,
                                LimiteCredito = sp.LimiteCredito,
                                Plazo = sp.Plazo,
                                ClienteNombre = sp.ClienteNombre,
                                FechaContabilizacion = sp.FechaContabilizacion,
                                Monto = sp.Monto - sp.Abono,
                                EstacionServicioID = sp.EstacionServicioID,
                                EstacionNombre = sp.EstacionNombre,
                                Abono = sp.Abono,
                                FechaVencimiento = sp.FechaVencimiento,
                                Numero = sp.Numero,
                                Abreviatura = sp.Abreviatura,
                                AreaVentaNombre = sp.AreaVentaNombre,
                                Dias = sp.Dias,
                                Grupos = sp.Grupos
                            }).ToList();

                Lista = (from sp in ListaRep
                         group sp by new { sp.EstacionNombre, sp.Grupos } into gr
                         select new Parametros.ListNombreDisplayValueLimite
                         {
                             Nombre = gr.Key.EstacionNombre,
                             Display = gr.Key.Grupos,
                             Limite = 0,//Convert.ToDecimal(gr.Sum(s => s.LimiteCredito)),
                             Value = Convert.ToDecimal(gr.Sum(s => s.Monto))
                         }).ToList();

                Lista.ToList().ForEach(obj => { obj.Limite = Convert.ToDecimal(ListaRep.Where(o => o.EstacionNombre.Equals(obj.Nombre)).GroupBy(g => new {g.ClienteID, g.LimiteCredito}).Sum(s => s.Key.LimiteCredito)); });

                Lista.ToList().ForEach(det =>
                {
                    switch (det.Display)
                    {
                        case "0":
                            det.Display = "Corriente";
                            break;
                        case "1":
                            det.Display = "De 01 - 8 Días";
                            break;
                        case "2":
                            det.Display = "De 09 - 15 Días";
                            break;
                        case "3":
                            det.Display = "De 16 - 30 Días";
                            break;
                        case "4":
                            det.Display = "Más de 31 Días";
                            break;
                    }
                });


                pivotGridControlLista.DataSource = Lista;
                pivotGridControlLista.RefreshData();
                btnExport.Enabled = true;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< GRAFICA >>>

        private void pivotGridControlLista_CellClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            if (e.RowField != null)
            {
                if (e.RowIndex >= 0)
                {
                    if (!e.RowIndex.Equals(Fila))
                    {
                        try
                        {
                            Fila = e.RowIndex;
                            string texto = pivotGridControlLista.GetFieldValue(fieldEstacion,e.RowIndex).ToString();
                            var Ventas = Lista.Where(v => v.Nombre.Equals(texto)).ToList();

                            chartControlLista.Titles[0].Text = "Antigüedad " + texto;
                            chartControlLista.DataSource = Ventas;

                        }

                        catch (Exception ex)
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                        }
                    }

                }
            }
        }

        #endregion
        
        #region <<< REPORTE >>>

        private void pivotGridControlLista_CellDoubleClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            if (e.RowField != null)
            {
                if (e.RowIndex >= 0)
                {
                    try
                    {
                        //Estado de Cuenta
                        bgwOn = new BackgroundWorker();
                        bgwOn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOn_RunWorkerCompleted);
                        bgwOn.DoWork += new DoWorkEventHandler(bgwOn_DoWork);
                        string texto = pivotGridControlLista.GetFieldValue(fieldEstacion,e.RowIndex).ToString();
                        bgwOn.RunWorkerAsync(texto);
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
            }
        }

        private void bgwOn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                SAGAS.Reportes.Ventas.Hojas.RptClientesCreditos rep = new SAGAS.Reportes.Ventas.Hojas.RptClientesCreditos(Convert.ToDecimal(ListaRep.Where(v => v.EstacionNombre.Equals(e.Argument.ToString())).Sum(s => s.Monto)));
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = e.Argument.ToString();
                rep.xrCeTitulo.Text = "Resumen Antigüedad de Saldos de Clientes al " + Convert.ToDateTime(dateFecha.EditValue).ToShortDateString();
                rep.DataSource = ListaRep.Where(v => v.EstacionNombre.Equals(e.Argument.ToString()));
                rep.xrChartTotal.DataSource = ListaRep.Where(v => v.EstacionNombre.Equals(e.Argument.ToString())).ToList();
                e.Result = rep;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void bgwOn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    Mrep = ((Reportes.Ventas.Hojas.RptClientesCreditos)(e.Result));
                    Mrep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (vistaLista.DataSource != null)
                {
                    this.pivotGridControlLista.ShowPrintPreview();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

             #endregion   
    }
}
