using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogGetManejo : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        private int UsuarioID = Parametros.General.UserID;
        public Form _MDIParent = new Form();
        public bool Comprobante = false;

        public int RD
        {
            get { return Convert.ToInt32(lkCombustible.EditValue); }
            set { lkCombustible.EditValue = value; }
        }


        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }
         
        #endregion

        #region *** INICIO ***

        public DialogGetManejo()
        {
            InitializeComponent();

        }
                
        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                lkES.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                
                lkES.EditValue = Parametros.General.EstacionServicioID;

                dateDesde.EditValue = Convert.ToDateTime(db.GetDateServer());
                dateHasta.EditValue = Convert.ToDateTime(db.GetDateServer());
                              
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** METODOS ***

      
        private decimal OtroEquivalente(string texto)
        {
            try
            {
                string input = Regex.Replace(texto, "[^0-9]+", string.Empty);
                return Convert.ToDecimal(input);
            }
            catch
            {return 1m;}
        }

        public bool ValidarCampos()
        {
            try
            {
                if (lkES.EditValue == null || Convert.ToInt32(glkCliente.EditValue) <= 0 || Convert.ToInt32(lkCombustible.EditValue) < 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToDateTime(dateHasta.EditValue) < Convert.ToDateTime(dateDesde.EditValue))
                {
                    Parametros.General.DialogMsg("Favor revizar las fechas." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                return true;
            }
            catch
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            try
            {
                    if (ValidarCampos())
                    {
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                        rv.Text = "Reporte Acta de Manejo";

                        Reportes.Inventario.Hojas.RptActaManejo rep = new Reportes.Inventario.Hojas.RptActaManejo();
                        
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                        db.CommandTimeout = dbView.CommandTimeout = 900;

                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                        
                        rep.CeEstacion.Text = Convert.ToString(lkES.Text);
                        rep.xrCeCliente.Text = Convert.ToString(glkCliente.Text);

                        var Es = db.EstacionServicios.Single(es => es.ID.Equals(Convert.ToInt32(lkES.EditValue)));

                        if (!Es.AdministradorID.Equals(0))
                        {
                            var empl = db.Empleados.Single(em => em.ID.Equals(Es.AdministradorID));
                            rep.xrCeAdmonNombre.Text = empl.Nombres + " " + empl.Apellidos;
                        }

                        DateTime FechaDesde = Convert.ToDateTime(dateDesde.EditValue);
                        DateTime FechaHasta = Convert.ToDateTime(dateHasta.EditValue);

                        rep.xrCeFechaInicial.Text = FechaDesde.ToShortDateString();
                        rep.xrCeFechaFinal.Text = FechaHasta.ToShortDateString();

                        var saldo = from vd in dbView.VistaDeudors
                                    join vm in dbView.VistaMovimientos on vd.VistaMovimiento equals vm
                                    where vm.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) && !vm.Anulado && vd.ClienteID.Equals(Convert.ToInt32(glkCliente.EditValue)) && 
                                    (vd.ProductoID.Equals(Convert.ToInt32(lkCombustible.EditValue)) || Convert.ToInt32(lkCombustible.EditValue).Equals(0))
                                    group vd by new {vd.ProductoID, vd.ProductoCodigo, vd.ProductoNombre, vd.ClienteID} into gr
                                    select new
                                    {
                                        ProductoID = gr.Key.ProductoID,
                                        Codigo = gr.Key.ProductoCodigo,
                                        Nombre = gr.Key.ProductoNombre,
                                        Total = (
                                        gr.Count(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date) > 0
                                        ? (gr.Where(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date).Sum(s => s.Valor) != 0m
                                        ? gr.Where(o => o.ClienteID.Equals(gr.Key.ClienteID) && o.ProductoID.Equals(gr.Key.ProductoID) && o.FechaFisico.Value.Date < FechaDesde.Date).Sum(s => s.Valor)
                                        :0m)
                                        : 0m)
                                        
                                    };

                        saldo.Where(o => !o.ProductoID.Equals(0)).ToList().ForEach(obj =>
                            {
                                DevExpress.XtraReports.UI.GroupFooterBand GroupFooter = new DevExpress.XtraReports.UI.GroupFooterBand();
                                var Proc = dbView.GetActaManejo(Convert.ToInt32(lkES.EditValue), Convert.ToInt32(glkCliente.EditValue), obj.ProductoID, FechaDesde.Date, FechaHasta.Date).ToList();
                                
                                DevExpress.XtraReports.UI.XRSubreport xrSubVerificacion = new DevExpress.XtraReports.UI.XRSubreport();

                                xrSubVerificacion.ReportSource = new Reportes.Inventario.Hojas.SubRptLitrosManejo(obj.Nombre, obj.Total, Decimal.Round(obj.Total / 3.785m, 3, MidpointRounding.AwayFromZero), Proc);
                                GroupFooter.Controls.Add(xrSubVerificacion);
                                rep.Bands.Add(GroupFooter);
                            });
                                               
                        rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                        rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                        rv.MdiParent = _MDIParent;

                        rep.RequestParameters = false;
                        rep.CreateDocument();
                        rv.Show();
                        this.Dispose();
                    }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkES.EditValue != null)
                {

                    glkCliente.Properties.DataSource = null;
                    glkCliente.Properties.DataSource = (from c in db.Clientes
                                                       join ces in db.ClienteEstacions on c.ID equals ces.ClienteID
                                                        where ces.EstacionServicioID.Equals(IDEstacionServicio) && c.Activo && c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID())
                                                       group c by new { ces.ClienteID, c.Codigo, c.Nombre } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ClienteID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombre,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                       }).ToList();

                    lkCombustible.Properties.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void glkCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkCliente.EditValue) > 0)
                {
                    lkCombustible.Properties.DataSource = null;

                    var obj = (from d in db.Deudors
                               join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio)) on d.MovimientoID equals m.ID
                               join p in db.Productos on d.ProductoID equals p.ID
                               join c in db.Clientes.Where(c => c.ID.Equals(Convert.ToInt32(glkCliente.EditValue))) on d.ClienteID equals c.ID
                               group d by new { d.ProductoID, p.Nombre } into gr
                               select new
                               {
                                   ID = gr.Key.ProductoID,
                                   Producto = gr.Key.Nombre
                               }).ToList();
                    
                    if (obj.Count > 0)
                    {
                        List<Parametros.ListIdDisplay> lista = new List<Parametros.ListIdDisplay>();
                        lista.Add(new Parametros.ListIdDisplay { ID = 0, Display = "TODOS" });
                        obj.ForEach(item => { lista.Add(new Parametros.ListIdDisplay { ID = item.ID, Display = item.Producto }); });
                                                
                        lkCombustible.Properties.DataSource = lista;
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