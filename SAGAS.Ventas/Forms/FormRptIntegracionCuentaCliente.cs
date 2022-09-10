using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Threading.Tasks;

namespace SAGAS.Ventas.Forms
{
    public partial class FormRptIntegracionCuentaCliente : Form
    {
        #region *** DECLARACIONES ***

        private int Usuario = Parametros.General.UserID;
        private string NombreUsuario = Parametros.General.UserName;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        internal int _ContadoDolares;
        internal int _ContadoCordobas;
        internal int _ClienteAnticipo;
        internal int _ClientePrestamoManejoID;
        internal int _CuentaEmpleadoID;
        internal int _ConceptoSalarioID;
        internal bool _EsEmpleado = false;
        private BackgroundWorker bgwOff;
        private BackgroundWorker bgwOn;
        private static Entidad.Cliente client;
        private Entidad.SAGASDataClassesDataContext db;
        private List<Entidad.EstacionServicio> EtEstaciones;
        private int _Central;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private SAGAS.Reportes.Ventas.Hojas.RptECClientesMain MrepMain;/*---------*/
        private SAGAS.Reportes.Ventas.Hojas.SubRptECClientes Srep;/*---------*/
        private SAGAS.Reportes.Nomina.Hojas.SubRptECEmpleado Erep;/*---------*/
        private List<Entidad.VistaEstadoCuentaCliente> EtVista;
        private List<ListaClientesCuentas> _ListClientesToShow = new List<ListaClientesCuentas>();
        private List<ListaClientesCuentas> _ListClientes;

        private DevExpress.XtraReports.UI.XRTable xrtbDatos;
        private DevExpress.XtraReports.UI.XRTableRow trEstacion;
        private DevExpress.XtraReports.UI.XRTableCell tcEstacion;
        private DevExpress.XtraReports.UI.XRTableCell tcbEstacion;
        private DevExpress.XtraReports.UI.XRTableRow trCliente;
        private DevExpress.XtraReports.UI.XRTableCell tcCliente;
        private DevExpress.XtraReports.UI.XRTableCell tcbCliente;
        private DevExpress.XtraReports.UI.XRTableRow trRUC;
        private DevExpress.XtraReports.UI.XRTableCell tcRUC;
        private DevExpress.XtraReports.UI.XRTableCell tcbRUC;
        private DevExpress.XtraReports.UI.XRTableRow trDirec;
        private DevExpress.XtraReports.UI.XRTableCell tcDirec;
        private DevExpress.XtraReports.UI.XRTableCell tcbDirec;
        private DevExpress.XtraReports.UI.XRTableRow trTelf;
        private DevExpress.XtraReports.UI.XRTableCell tcTelf;
        private DevExpress.XtraReports.UI.XRTableCell tcbTelf;
        private DevExpress.XtraReports.UI.XRTableRow trPeriodo;
        private DevExpress.XtraReports.UI.XRTableCell tcPeriodo;
        private DevExpress.XtraReports.UI.XRTableCell tcbPeriodo;

        private int IDCliente
        {
            get { return Convert.ToInt32(glkCliente.EditValue); }
            set { glkCliente.EditValue = value; }
        }

        struct ListaClientesCuentas
        {
            public int ID;
            public string Codigo;
            public string Nombre;
            public string Display;
            public int CuentaID;
        }

        #endregion

        #region *** INICIO ***

        public FormRptIntegracionCuentaCliente()
        {
            InitializeComponent();
            XRTablaDatos();
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void XRTablaDatos()
        {

            this.xrtbDatos = new DevExpress.XtraReports.UI.XRTable();
            this.trEstacion = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcEstacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbEstacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.trCliente = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcCliente = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbCliente = new DevExpress.XtraReports.UI.XRTableCell();
            this.trRUC = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcRUC = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbRUC = new DevExpress.XtraReports.UI.XRTableCell();
            this.trDirec = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcDirec = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbDirec = new DevExpress.XtraReports.UI.XRTableCell();
            this.trTelf = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcTelf = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbTelf = new DevExpress.XtraReports.UI.XRTableCell();
            this.trPeriodo = new DevExpress.XtraReports.UI.XRTableRow();
            this.tcPeriodo = new DevExpress.XtraReports.UI.XRTableCell();
            this.tcbPeriodo = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrtbDatos)).BeginInit();
            // 
            // xrtbDatos
            // 
            this.xrtbDatos.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrtbDatos.Name = "xrtbDatos";
            this.xrtbDatos.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.trEstacion,
            this.trCliente,
            this.trRUC,
            this.trDirec,
            this.trTelf,
            this.trPeriodo});
            this.xrtbDatos.SizeF = new System.Drawing.SizeF(800F, 150F);
            // 
            // trEstacion
            // 
            this.trEstacion.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcEstacion,
            this.tcbEstacion});
            this.trEstacion.Name = "trEstacion";
            this.trEstacion.WidthF = 800f;
            // 
            // tcEstacion
            // 
            this.tcEstacion.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcEstacion.Name = "tcEstacion";
            this.tcEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcEstacion.StylePriority.UseFont = false;
            this.tcEstacion.StylePriority.UsePadding = false;
            this.tcEstacion.StylePriority.UseTextAlignment = false;
            this.tcEstacion.Text = "Estación   :";
            this.tcEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcEstacion.WidthF = 100f;
            // 
            // tcbEstacion
            // 
            this.tcbEstacion.Name = "tcbEstacion";
            this.tcbEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbEstacion.StylePriority.UsePadding = false;
            this.tcbEstacion.StylePriority.UseTextAlignment = false;
            this.tcbEstacion.Text = "tcbEstacion";
            this.tcbEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbEstacion.WidthF = 600f;
            // 
            // trCliente
            // 
            this.trCliente.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcCliente,
            this.tcbCliente});
            this.trCliente.Name = "trCliente";
            this.trCliente.WidthF = 800f;
            // 
            // tcCliente
            // 
            this.tcCliente.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcCliente.Name = "tcCliente";
            this.tcCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcCliente.StylePriority.UseFont = false;
            this.tcCliente.StylePriority.UsePadding = false;
            this.tcCliente.StylePriority.UseTextAlignment = false;
            this.tcCliente.Text = "Cliente   :";
            this.tcCliente.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcCliente.WidthF = 100f;
            // 
            // tcbCliente
            // 
            this.tcbCliente.Name = "tcbCliente";
            this.tcbCliente.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbCliente.StylePriority.UsePadding = false;
            this.tcbCliente.StylePriority.UseTextAlignment = false;
            this.tcbCliente.Text = "tcbCliente";
            this.tcbCliente.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbCliente.WidthF = 600f;
            // 
            // trRUC
            // 
            this.trRUC.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcRUC,
            this.tcbRUC});
            this.trRUC.Name = "trRUC";
            this.trRUC.WidthF = 800f;
            // 
            // tcRUC
            // 
            this.tcRUC.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcRUC.Name = "tcRUC";
            this.tcRUC.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcRUC.StylePriority.UseFont = false;
            this.tcRUC.StylePriority.UsePadding = false;
            this.tcRUC.StylePriority.UseTextAlignment = false;
            this.tcRUC.Text = "RUC   :";
            this.tcRUC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcRUC.WidthF = 100f;
            // 
            // tcbRUC
            // 
            this.tcbRUC.Name = "tcbRUC";
            this.tcbRUC.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbRUC.StylePriority.UsePadding = false;
            this.tcbRUC.StylePriority.UseTextAlignment = false;
            this.tcbRUC.Text = "tcbRUC";
            this.tcbRUC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbRUC.WidthF = 600f;
            // 
            // trDirec
            // 
            this.trDirec.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcDirec,
            this.tcbDirec});
            this.trDirec.Name = "trDirec";
            this.trDirec.WidthF = 800f;
            // 
            // tcDirec
            // 
            this.tcDirec.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcDirec.Name = "tcDirec";
            this.tcDirec.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcDirec.StylePriority.UseFont = false;
            this.tcDirec.StylePriority.UsePadding = false;
            this.tcDirec.StylePriority.UseTextAlignment = false;
            this.tcDirec.Text = "Dirección   :";
            this.tcDirec.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcDirec.WidthF = 100f;
            // 
            // tcbDirec
            // 
            this.tcbDirec.Name = "tcbDirec";
            this.tcbDirec.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbDirec.StylePriority.UsePadding = false;
            this.tcbDirec.StylePriority.UseTextAlignment = false;
            this.tcbDirec.Text = "tcbDirec";
            this.tcbDirec.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbDirec.WidthF = 600f;
            // 
            // trTelf
            // 
            this.trTelf.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcTelf,
            this.tcbTelf});
            this.trTelf.Name = "trTelf";
            this.trTelf.WidthF = 800f;
            // 
            // tcTelf
            // 
            this.tcTelf.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcTelf.Name = "tcTelf";
            this.tcTelf.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcTelf.StylePriority.UseFont = false;
            this.tcTelf.StylePriority.UsePadding = false;
            this.tcTelf.StylePriority.UseTextAlignment = false;
            this.tcTelf.Text = "Teléfonos   :";
            this.tcTelf.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcTelf.WidthF = 100f;
            // 
            // tcbTelf
            // 
            this.tcbTelf.Name = "tcbTelf";
            this.tcbTelf.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbTelf.StylePriority.UsePadding = false;
            this.tcbTelf.StylePriority.UseTextAlignment = false;
            this.tcbTelf.Text = "tcbTelf";
            this.tcbTelf.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbTelf.WidthF = 600f;
            // 
            // trPeriodo
            // 
            this.trPeriodo.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tcPeriodo,
            this.tcbPeriodo});
            this.trPeriodo.Name = "trPeriodo";
            this.trPeriodo.WidthF = 800f;
            // 
            // tcPeriodo
            // 
            this.tcPeriodo.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tcPeriodo.Name = "tcPeriodo";
            this.tcPeriodo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.tcPeriodo.StylePriority.UseFont = false;
            this.tcPeriodo.StylePriority.UsePadding = false;
            this.tcPeriodo.StylePriority.UseTextAlignment = false;
            this.tcPeriodo.Text = "Período del   :";
            this.tcPeriodo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.tcPeriodo.WidthF = 100f;
            // 
            // tcbPeriodo
            // 
            this.tcbPeriodo.Name = "tcbPeriodo";
            this.tcbPeriodo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.tcbPeriodo.StylePriority.UsePadding = false;
            this.tcbPeriodo.StylePriority.UseTextAlignment = false;
            this.tcbPeriodo.Text = "tcbPeriodo";
            this.tcbPeriodo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tcbPeriodo.WidthF = 600f;
            ((System.ComponentModel.ISupportInitialize)(this.xrtbDatos)).EndInit();
        }

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                _ContadoDolares = Parametros.Config.ClienteVentaContadoMonedaExtrangeraID();
                _ContadoCordobas = Parametros.Config.ClienteVentaConttadoID();
                _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
                _ClientePrestamoManejoID = Parametros.Config.ClientePrestamoManejoID();
                _CuentaEmpleadoID = Parametros.Config.CuentaPorCobrarEmpleado();
                _ConceptoSalarioID = Parametros.Config.ConceptoSalarioEmpleadoID();
                _Central = Parametros.Config.OficinaCentralID();
                EtEstaciones = db.EstacionServicios.ToList();

                //Revisar glkCliente.Properties.DataSource = null;
                //_ListClientes = (from c in db.Clientes
                //        join ces in db.ClienteEstacions on c.ID equals ces.ClienteID
                //        where ces.EstacionServicioID.Equals(IDEstacionServicio) && c.Activo
                //        group c by new { ces.ClienteID, c.Codigo, c.Nombre, c.CuentaContableID } into gr
                //                 select new ListaClientesCuentas
                //        {
                //            ID = gr.Key.ClienteID,
                //            Codigo = gr.Key.Codigo,
                //            Nombre = gr.Key.Nombre,
                //            Display = gr.Key.Codigo + " | " + gr.Key.Nombre,
                //            CuentaID = gr.Key.CuentaContableID
                //}).ToList();
                _ListClientes = (from m in db.Movimientos
                                 join c in db.Clientes on m.ClienteID equals c.ID
                                 where m.ClienteID > 0 && m.EstacionServicioID.Equals(IDEstacionServicio)
                                 group c by new { c.ID, c.Codigo, c.Nombre, c.CuentaContableID } into g
                                 select new ListaClientesCuentas { ID = g.Key.ID, Codigo = g.Key.Codigo, Nombre = g.Key.Nombre, Display = g.Key.Codigo  + " | " + g.Key.Nombre, CuentaID = g.Key.CuentaContableID }).ToList();
                //lkTipoMov.EditValue = 0;

                //Estaciones de Servicio
                var lista = (from es in db.EstacionServicios
                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                            select new { es.ID, es.Nombre }).ToList();
                lista.Insert(0, new { ID = 0, Nombre = "TODAS" });

                lkES.Properties.DataSource = lista;
                lkES.EditValue = Parametros.General.EstacionServicioID;

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;
                lkMesInicial_Validated(null, null);
                lkMesFinal.EditValue = DateTime.Now.Month;
                lkMesFinal_Validated(null, null);

                var obj = (from cl in db.Clientes
                          join cc in db.CuentaContables on cl.CuentaContableID equals cc.ID
                          group cl by new { cl.CuentaContableID, cc.Codigo, cc.Nombre } into gr
                          select new
                          {
                              CuentaID = gr.Key.CuentaContableID,
                              CuentaCodigo = gr.Key.Codigo,
                              CuentaNombre = gr.Key.Nombre
                          }).ToList();

                obj.Insert(0, new { CuentaID = 0, CuentaCodigo = "0000000000", CuentaNombre = "TODAS" });

                //lkTipoCta.Properties.DataSource = obj.Select(s => new { ID = s.CuentaID, Display = s.CuentaCodigo + " | " + s.CuentaNombre }).ToList();
                //lkTipoCta.EditValue = 0;
                switchReport.IsOn = false;
                switchReport_Toggled(null, null);
                glkCliente.Properties.DataSource = _ListClientes.Select(s => new {s.ID, s.Codigo, s.Nombre, s.Display}).ToList();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        public bool ValidarCampos()
        {
            //if (lkTipoCta.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar el tipo de cuenta.", Parametros.MsgType.warning);
            //    return false;
            //}
            
            if (IDCliente < 0 || glkCliente.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar un cliente.", Parametros.MsgType.warning);
                return false;
            }

            if (switchReport.IsOn)
            {
                if (dateInicial.EditValue == null || dateFinal.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
                    return false;
                }
            }
            else
            {
                if (dateFinal.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (switchReport.IsOn)
            {
                if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
                {
                    Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (lkES.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Estación de Servicio", Parametros.MsgType.warning);
                return false;
            }

            if (layoutControlItemSes.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
            {
                if (lkSES.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Sub Estación de Servicio", Parametros.MsgType.warning);
                    return false;
                }
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
                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).Select(s => new {s.ID, s.Nombre})).ToList();

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

        private void glkCliente_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IDCliente > 0)
                {
                    if (!_EsEmpleado)
                    {
                        client = db.Clientes.Single(p => p.ID.Equals(IDCliente
                            ));

                        if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                        {
                            var obj = (from d in db.Deudors
                                       join m in db.Movimientos on d.MovimientoID equals m.ID
                                       join c in db.Clientes.Where(c => c.ID.Equals(client.ID)) on d.ClienteID equals c.ID
                                       group d by new { d.ClienteID, c.Nombre } into gr
                                       select new
                                       {
                                           ID = gr.Key.ClienteID,
                                           Cliente = gr.Key.Nombre,
                                           suma = gr.Sum(s => s.Valor)
                                       }).ToList();

                            spLimite.Value = client.LimiteCredito;

                            //decimal Saldo = (db.Deudors.Count(d => d.ClienteID.Equals(IDCliente)) > 0 ? db.Deudors.Where(d => d.ClienteID.Equals(IDCliente)).Sum(s => s.Valor) : 0m);
                            decimal Saldo = Convert.ToDecimal(db.GetSaldoCliente(client.ID, dateFinal.DateTime.Date));
                            spSaldo.Value = Saldo;

                            decimal _Disponible = client.LimiteCredito - Saldo;
                            spDisponible.Value = _Disponible;
                        }
                        else
                        {
                            var obj = from d in db.Deudors
                                      join m in db.Movimientos on d.MovimientoID equals m.ID
                                      join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                      where !m.Anulado && !mt.EsAnulado && (m.ClienteID.Equals(client.ID) || d.ClienteID.Equals(client.ID))
                                      select d.Valor;

                            spLimite.Value = client.LimiteCredito;

                            //decimal Saldo = (db.Deudors.Count(d => d.ClienteID.Equals(IDDeudor)) > 0 ? db.Deudors.Where(d => d.ClienteID.Equals(IDDeudor)).Sum(s => s.Valor) : 0m);
                            decimal Saldo = (obj.Count() > 0 ? Convert.ToDecimal(obj.Sum(s => s)) : 0m);
                            spSaldo.Value = Saldo;

                            decimal _Disponible = client.LimiteCredito - Saldo;
                            spDisponible.Value = _Disponible;
                        }


                    }
                    else
                    {
                        spLimite.Value = 0m;
                        spSaldo.Value = 0m;
                        spDisponible.Value = 0m;
                    }
                }
                else
                {
                    spLimite.Value = 0m;
                    spSaldo.Value = 0m;
                    spDisponible.Value = 0m;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    if (switchReport.IsOn)
                    {
                        //Movimientos
                        bgwOn = new BackgroundWorker();
                        bgwOn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOn_RunWorkerCompleted);
                        bgwOn.DoWork += new DoWorkEventHandler(bgwOn_DoWork);
                        bgwOn.RunWorkerAsync();
                    }
                    else if (!switchReport.IsOn)
                    {
                        //Facturas Pendientes
                        bgwOff = new BackgroundWorker();
                        bgwOff.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOff_RunWorkerCompleted);
                        bgwOff.DoWork += new DoWorkEventHandler(bgwOff_DoWork);
                        bgwOff.RunWorkerAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
            
        }

        private void bgwOn_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv.CommandTimeout = 600;
                
                DateTime vFecha = Convert.ToDateTime(db.GetDateServer());
                SAGAS.Reportes.Ventas.Hojas.RptECClientesMain MainMrep = new SAGAS.Reportes.Ventas.Hojas.RptECClientesMain();


                List<Int32> ListaCL = new List<int>();
                decimal SaldoTotal = 0;

                if (_EsEmpleado)
                {
                    #region <<< EMPLEADOS >>>

                    if (IDCliente.Equals(0))
                        ListaCL.AddRange(_ListClientesToShow.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                    else
                        ListaCL.Add(IDCliente);

                    ListaCL.ForEach(det =>
                    {
                        //SAGAS.Reportes.Ventas.Hojas.RptECClienteMov Mrep = new SAGAS.Reportes.Ventas.Hojas.RptECClienteMov();


                        /************************************/
                        bool SUS = (lkSES.EditValue == null ? false : true);

                        EtVista = (from vcc in dv.VistaEstadoCuentaEmpleadoContabilidad
                                   where vcc.ID.Equals(det)
                                    && vcc.FechaRegistro.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                    && (vcc.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                    && (!SUS || (SUS && ((vcc.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                   select new Entidad.VistaEstadoCuentaCliente
                                   {
                                       MovimientoID = vcc.MovimientoID,
                                       ID = vcc.ID,
                                       CuentaContableID = _CuentaEmpleadoID,
                                       Clientes = vcc.Empleados,
                                       FechaRegistro = vcc.FechaRegistro,
                                       TipoMovimiento = vcc.TipoMovimiento,
                                       Comentario = vcc.Comentario,
                                       Area = vcc.Area,
                                       EstacionNombre = vcc.EstacionNombre,
                                       EstacionServicioID = vcc.EstacionServicioID,
                                       SubEstacionID = vcc.SubEstacionID,
                                       NumeroDocumento = vcc.NumeroDocumento,
                                       DeudorID = vcc.DeudorID,
                                       Monto = vcc.Monto,
                                       Pagado = vcc.Pagado,
                                   }).ToList();


                        EtVista.OrderBy(o => o.FechaRegistro);

                        var lSaldo = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                        decimal vSaldo = 0;
                        if (lSaldo.Count() > 0)
                            vSaldo = Convert.ToDecimal(lSaldo.Sum(s => s.Monto));

                        var Lista = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date >= Convert.ToDateTime(dateInicial.EditValue).Date).OrderBy(o => o.FechaRegistro).ToList();
                        /**********************************/
                        if (Lista.Count == 0 && vSaldo.Equals(0)) return;
                        ////////////////////////////////////

                        SAGAS.Reportes.Ventas.Hojas.RptECClienteMov Mrep = new SAGAS.Reportes.Ventas.Hojas.RptECClienteMov();
                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                        Mrep.PicLogo.Image = picture_LogoEmpresa;
                        Mrep.CeEmpresa.Text = Nombre;
                        Mrep.xrCeRango.Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();

                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();

                        XRTablaDatos();
                        ///////////////////////////////////

                        string Estaciones = "";

                        if (lkES.EditValue.Equals(0))
                            Estaciones = "Todas las Estaciones";
                        else
                        {
                            if (lkSES.EditValue == null)
                                Estaciones = Convert.ToString(lkES.Text);
                            else
                            {
                                if (lkSES.EditValue.Equals(0))
                                    Estaciones = Convert.ToString(lkES.Text) + " Todas las Sub Estaciones";
                                else
                                    Estaciones = Convert.ToString(lkSES.Text);
                            }
                        }

                        xrtbDatos.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                        xrtbDatos.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                        xrtbDatos.SizeF = new System.Drawing.SizeF(800f, 150F);
                        xrtbDatos.StylePriority.UseTextAlignment = false;
                        xrtbDatos.Rows[0].Cells[1].Text = Estaciones;
                        xrtbDatos.Rows[1].Cells[0].Text = "Empleado  :";
                        xrtbDatos.Rows[1].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Clientes : "");
                        xrtbDatos.Rows[2].Visible = false;// .Text = (EtVista.Count() > 0 ? EtVista.First().RUC : "");
                        xrtbDatos.Rows[3].Visible = false;//Text = (EtVista.Count() > 0 ? EtVista.First().Direccion : "");
                        xrtbDatos.Rows[4].Visible = false;// = false;//Text = (EtVista.Count() > 0 ? EtVista.First().Telefonos : "");
                        xrtbDatos.Rows[5].Visible = false;//.Cells[1].Visible = false;//Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                        ghEstacion.Controls.Add(xrtbDatos);

                        xrSubDetalle.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClienteMov(Lista, vFecha.ToString(), vSaldo);
                        xrSubDetalle.LocationF = new PointF(0f, 150f);
                        ghEstacion.Controls.Add(xrSubDetalle);
                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                        Mrep.Bands.Add(ghEstacion);

                        DevExpress.XtraReports.UI.GroupHeaderBand GroupHSub = new DevExpress.XtraReports.UI.GroupHeaderBand();

                        DevExpress.XtraReports.UI.XRSubreport SubRep = new DevExpress.XtraReports.UI.XRSubreport();
                        SubRep.ReportSource = Mrep;
                        Mrep.CreateDocument(true);
                        SaldoTotal += Decimal.Round(vSaldo + Convert.ToDecimal(Lista.Sum(s => s.Monto)), 2, MidpointRounding.AwayFromZero);
                        GroupHSub.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
                        GroupHSub.Controls.Add(SubRep);
                        MainMrep.Bands.Add(GroupHSub);
                    });
                    
                    #endregion
                }
                else
                {
                    #region <<< CLIENTES >>>
                    
                    if (IDCliente.Equals(0))
                        ListaCL.AddRange(_ListClientesToShow.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                    else
                        ListaCL.Add(IDCliente);

                    ListaCL.ForEach(det =>
                    {
                        //SAGAS.Reportes.Ventas.Hojas.RptECClienteMov Mrep = new SAGAS.Reportes.Ventas.Hojas.RptECClienteMov();


                        /************************************/
                        bool SUS = (lkSES.EditValue == null ? false : true);

                        EtVista = (from vcc in dv.VistaEstadoCuentaCliente
                                   where vcc.ID.Equals(det)
                                    && vcc.FechaRegistro.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                    && (vcc.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                    && (!SUS || (SUS && ((vcc.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                   select vcc).ToList();

                        if (det.Equals(_ContadoCordobas) || det.Equals(_ContadoDolares))
                        {
                            var CL = db.Clientes.Single(s => s.ID.Equals(det));

                            List<Entidad.VistaEstadoCuentaCliente> obj = (from m in db.Movimientos
                                                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                                          join av in db.AreaVentas on m.AreaID equals av.ID
                                                                          join d in db.Deudors on m.ID equals d.MovimientoID
                                                                          where m.MonedaID.Equals(det.Equals(_ContadoCordobas) ? 1 : 2) && (m.AreaID > 0 || m.ClienteID.Equals(det))
                                                                          && m.FechaRegistro.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                                                          && (m.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                                                          && (!SUS || (SUS && ((m.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                                                          && !m.Anulado && m.MovimientoTipoID.Equals(49)
                                                                          select new Entidad.VistaEstadoCuentaCliente
                                                                          {
                                                                              MovimientoID = m.ID,
                                                                              ID = CL.ID,
                                                                              CuentaContableID = CL.CuentaContableID,
                                                                              Clientes = CL.Codigo + " " + CL.Nombre,
                                                                              RUC = CL.RUC,
                                                                              Direccion = CL.Direccion,
                                                                              Telefonos = CL.Telefono1,
                                                                              FechaRegistro = m.FechaRegistro,
                                                                              TipoMovimiento = mt.Abreviatura,
                                                                              Area = av.Nombre,
                                                                              Comentario = m.Comentario,
                                                                              EstacionServicioID = m.EstacionServicioID,
                                                                              SubEstacionID = m.SubEstacionID,
                                                                              NumeroDocumento = (m.Numero > 0 ? m.Numero.ToString() : m.Referencia),
                                                                              DeudorID = 0,
                                                                              Monto = -Math.Abs(d.Pagos.Sum(s => s.Monto)),
                                                                              Pagado = m.Pagado
                                                                          }).ToList();

                            EtVista.AddRange(obj);

                        }

                        //if (det.Equals(_ClientePrestamoManejoID))
                        //{
                        //    var CL = db.Clientes.Single(s => s.ID.Equals(det));

                        //    List<Entidad.VistaEstadoCuentaCliente> obj = (from m in db.Movimientos
                        //                                                  join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                        //                                                  where m.FechaRegistro.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                        //                                                  && (m.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                        //                                                  && (!SUS || (SUS && ((m.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                        //                                                  && !m.Anulado && mt.EsAnulado
                        //                                                  select new Entidad.VistaEstadoCuentaCliente
                        //                                                  {
                        //                                                      MovimientoID = m.ID,
                        //                                                      ID = CL.ID,
                        //                                                      CuentaContableID = CL.CuentaContableID,
                        //                                                      Clientes = CL.Codigo + " " + CL.Nombre,
                        //                                                      RUC = CL.RUC,
                        //                                                      Direccion = CL.Direccion,
                        //                                                      Telefonos = CL.Telefono1,
                        //                                                      FechaRegistro = m.FechaRegistro,
                        //                                                      TipoMovimiento = mt.Abreviatura,
                        //                                                      Area = av.Nombre,
                        //                                                      Comentario = m.Comentario,
                        //                                                      EstacionServicioID = m.EstacionServicioID,
                        //                                                      SubEstacionID = m.SubEstacionID,
                        //                                                      NumeroDocumento = (m.Numero > 0 ? m.Numero.ToString() : m.Referencia),
                        //                                                      DeudorID = 0,
                        //                                                      Monto = -Math.Abs(d.Pagos.Sum(s => s.Monto)),
                        //                                                      Pagado = m.Pagado
                        //                                                  }).ToList();

                        //    EtVista.AddRange(obj);

                        //}


                        EtVista.OrderBy(o => o.FechaRegistro);

                        var lSaldo = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                        decimal vSaldo = 0;
                        if (lSaldo.Count() > 0)
                            vSaldo = Convert.ToDecimal(lSaldo.Sum(s => s.Monto));

                        var Lista = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date >= Convert.ToDateTime(dateInicial.EditValue).Date).OrderBy(o => o.FechaRegistro).ToList();
                        /**********************************/
                        if (Lista.Count == 0 && vSaldo.Equals(0)) return;
                        ////////////////////////////////////

                        SAGAS.Reportes.Ventas.Hojas.RptECClienteMov Mrep = new SAGAS.Reportes.Ventas.Hojas.RptECClienteMov();
                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                        Mrep.PicLogo.Image = picture_LogoEmpresa;
                        Mrep.CeEmpresa.Text = Nombre;
                        Mrep.xrCeRango.Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();

                        DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                        DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();

                        XRTablaDatos();
                        ///////////////////////////////////

                        string Estaciones = "";

                        if (lkES.EditValue.Equals(0))
                            Estaciones = "Todas las Estaciones";
                        else
                        {
                            if (lkSES.EditValue == null)
                                Estaciones = Convert.ToString(lkES.Text);
                            else
                            {
                                if (lkSES.EditValue.Equals(0))
                                    Estaciones = Convert.ToString(lkES.Text) + " Todas las Sub Estaciones";
                                else
                                    Estaciones = Convert.ToString(lkSES.Text);
                            }
                        }

                        xrtbDatos.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
                        xrtbDatos.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
                        xrtbDatos.SizeF = new System.Drawing.SizeF(800f, 150F);
                        xrtbDatos.StylePriority.UseTextAlignment = false;
                        xrtbDatos.Rows[0].Cells[1].Text = Estaciones;
                        xrtbDatos.Rows[1].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Clientes : "");
                        xrtbDatos.Rows[2].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().RUC : "");
                        xrtbDatos.Rows[3].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Direccion : "");
                        xrtbDatos.Rows[4].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Telefonos : "");
                        xrtbDatos.Rows[5].Cells[1].Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                        ghEstacion.Controls.Add(xrtbDatos);

                        xrSubDetalle.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClienteMov(Lista, vFecha.ToString(), vSaldo);
                        xrSubDetalle.LocationF = new PointF(0f, 150f);
                        ghEstacion.Controls.Add(xrSubDetalle);
                        ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                        Mrep.Bands.Add(ghEstacion);

                        DevExpress.XtraReports.UI.GroupHeaderBand GroupHSub = new DevExpress.XtraReports.UI.GroupHeaderBand();

                        DevExpress.XtraReports.UI.XRSubreport SubRep = new DevExpress.XtraReports.UI.XRSubreport();
                        SubRep.ReportSource = Mrep;
                        Mrep.CreateDocument(true);
                        SaldoTotal += Decimal.Round(vSaldo + Convert.ToDecimal(Lista.Sum(s => s.Monto)), 2, MidpointRounding.AwayFromZero);
                        GroupHSub.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
                        GroupHSub.Controls.Add(SubRep);
                        MainMrep.Bands.Add(GroupHSub);
                    });

                    #endregion
                }

                MainMrep.xrCeSaldoFInal.Text = SaldoTotal.ToString("#,0.00");
                e.Result = MainMrep;
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
                    MrepMain = ((SAGAS.Reportes.Ventas.Hojas.RptECClientesMain)(e.Result));
                    MrepMain.Landscape = true;
                    MrepMain.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = MrepMain.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void bgwOff_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_EsEmpleado)
                {
                    #region <<< EMPLEADOS >>>
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    dbView.CommandTimeout = 600;
                    //Fecha del Servidor
                    DateTime vFecha = Convert.ToDateTime(db.GetDateServer());
                    SAGAS.Reportes.Nomina.Hojas.SubRptECEmpleado rep = new SAGAS.Reportes.Nomina.Hojas.SubRptECEmpleado(vFecha);
                    rep.RequestParameters = false;
                    //Lista de los clientes
                    List<Int32> ListaCL = new List<int>();
                    //Llenando la lista de los clientes
                    if (IDCliente.Equals(0))
                        ListaCL.AddRange(_ListClientesToShow.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                    else
                        ListaCL.Add(IDCliente);

                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    //rep.XrCeCliente.Text = "Empleado   :";

                    rep.xrImpresion.Text = vFecha.ToShortDateString() + " " + vFecha.ToShortTimeString();

                    if (lkES.EditValue.Equals(0))
                        rep.CeEstacion.Text = "Todas las Estaciones";
                    else
                    {
                        if (lkSES.EditValue == null)
                            rep.CeEstacion.Text = Convert.ToString(lkES.Text);
                        else
                        {
                            if (lkSES.EditValue.Equals(0))
                                rep.CeEstacion.Text = Convert.ToString(lkES.Text) + " Todas las Sub Estaciones";
                            else
                                rep.CeEstacion.Text = Convert.ToString(lkSES.Text);
                        }
                    }

                    rep.xrCeMoneda.Text = db.Monedas.Single(m => m.ID.Equals(1)).Simbolo;

                    bool SUS = (lkSES.EditValue == null ? false : true);
                    rep.xrCeUsuario.Text = NombreUsuario;

                    //PENDIENTES AL CORTE                                
                    var ListaD = (from v in dbView.DocumentosPagadosEmpleado(Convert.ToDateTime(dateFinal.EditValue).Date)
                                  where ListaCL.Contains(v.EmpleadoID)
                                  && (v.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                  select v).OrderBy(o => o.FechaContabilizacion).ToList();

                    rep.DataSource = ListaD;
                    rep.XrCeTitulo.Text += " " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                    e.Result = rep;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();

                    #endregion

                }
                else
                {
                    #region <<< CLIENTES >>>
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    dbView.CommandTimeout = 600;
                    //Fecha del Servidor
                    DateTime vFecha = Convert.ToDateTime(db.GetDateServer());
                    SAGAS.Reportes.Ventas.Hojas.SubRptECClientes rep = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha);
                    rep.RequestParameters = false;
                    //Lista de los clientes
                    List<Int32> ListaCL = new List<int>();
                    //Llenando la lista de los clientes
                    if (IDCliente.Equals(0))
                        ListaCL.AddRange(_ListClientes.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                    else
                        ListaCL.Add(IDCliente);

                    //ListaCL.ForEach(det =>
                    //{
                    //    if (!det.Equals(0))
                    //    {
                    //        SAGAS.Reportes.Ventas.Hojas.RptECClientes rep = new SAGAS.Reportes.Ventas.Hojas.RptECClientes();

                    //Parametros de la empresa
                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.xrImpresion.Text = vFecha.ToShortDateString() + " " + vFecha.ToShortTimeString();

                    string texto = "N/A";

                    if (lkES.EditValue.Equals(0))
                    {
                        rep.CeEstacion.Text = "Todas las Estaciones";
                        var obj = EtEstaciones.SingleOrDefault(s => s.ID.Equals(_Central));

                        if (obj != null)
                            texto = obj.FirmaEstadoCuenta;

                    }
                    else
                    {
                        if (lkSES.EditValue == null)
                            rep.CeEstacion.Text = Convert.ToString(lkES.Text);
                        else
                        {
                            if (lkSES.EditValue.Equals(0))
                                rep.CeEstacion.Text = Convert.ToString(lkES.Text) + " Todas las Sub Estaciones";
                            else
                                rep.CeEstacion.Text = Convert.ToString(lkSES.Text);
                        }

                        var obj = EtEstaciones.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkES.EditValue)));

                        if (obj != null)
                            texto = obj.FirmaEstadoCuenta;

                    }

                    //Cuenta Contable
                    int vCuenta = 0;

                    //if (lkTipoCta.EditValue != null)
                    //{
                    //    if (Convert.ToInt32(lkTipoCta.EditValue) > 0)
                    //    {
                    //        vCuenta = Convert.ToInt32(lkTipoCta.EditValue);
                    //    }
                    //    else
                    //    {
                    vCuenta = _ListClientes.Single(p => p.ID.Equals(IDCliente)).CuentaID;
                    //    }
                    //}
                    //else
                    //{
                    //    vCuenta = (_ListClientesToShow.Count(p => p.ID.Equals(IDCliente)) > 0 ? _ListClientesToShow.First(p => p.ID.Equals(IDCliente)).CuentaID : 0);
                    //}

                    rep.xrCeMoneda.Text = db.Monedas.Single(m => m.ID.Equals(1)).Simbolo;

                    bool SUS = (lkSES.EditValue == null ? false : true);

                    //PENDIENTES AL CORTE                                
                    List<Entidad.MovimientosPagadosResult> ListaD = (from v in dbView.MovimientosPagados(Convert.ToDateTime(dateFinal.EditValue).Date, vCuenta)
                                                                     where ListaCL.Contains(v.ClienteID)
                                                                     && (v.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                                                     select v).OrderBy(o => o.FechaContabilizacion).ToList();

                    List<Entidad.MovimientosPagadosResult> ListaRoc = (from v in dbView.MovimientosPagadosOrigenROC(Convert.ToDateTime(dateFinal.EditValue).Date, vCuenta)
                                                                       where ListaCL.Contains(Convert.ToInt32(v.ClienteID))
                                                                       && (v.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                                                       select new Entidad.MovimientosPagadosResult
                                                                       {
                                                                           Abono = v.Abono,
                                                                           Abreviatura = v.Abreviatura,
                                                                           AreaVentaNombre = v.AreaVentaNombre,
                                                                           ClienteCodigo = v.ClienteCodigo,
                                                                           ClienteDireccion = v.ClienteDireccion,
                                                                           ClienteID = Convert.ToInt32(v.ClienteID),
                                                                           ClienteNombre = v.ClienteNombre,
                                                                           Dias = v.Dias,
                                                                           EstacionNombre = v.EstacionNombre,
                                                                           EstacionServicioID = v.EstacionServicioID,
                                                                           FechaContabilizacion = v.FechaContabilizacion,
                                                                           FechaVencimiento = v.FechaVencimiento,
                                                                           Grupos = v.Grupos,
                                                                           LimiteCredito = Convert.ToDecimal(v.LimiteCredito),
                                                                           Monto = v.Monto,
                                                                           MovimientoID = v.MovimientoID,
                                                                           Numero = v.Numero,
                                                                           Plazo = Convert.ToInt32(v.Plazo),
                                                                           Telefonos = v.Telefonos
                                                                       }
                                                                       ).ToList();

                    ListaD.AddRange(ListaRoc);

                    rep.DataSource = ListaD;

                    rep.GroupFooter1.Visible = true;
                    //rep.XrCeTotalRep.Text = Decimal.Round(Convert.ToDecimal(ListaD.Sum(s => s.Monto - s.Abono)), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    //rep.XrCeComentario.Text = "Hacemos constar, que de parte de " + rep.CeEstacion.Text + ", hemos recibido a entera satisfacción Facturas de combustible " +
                    //"con el volumen arriba detallado y por lo tanto estamos de acuerdo con los datos referidos y nos comprometemos a cancelar la cantidad de " + rep.xrCeMoneda.Text +
                    //"  " + rep.xrTableCell22.Text + " (" + Parametros.General.enletras(rep.xrTableCell22.Text) + "), una vez recibido el  presente documento, de lo contrario estará sujeto a recargo por mora.";

                    DateTime FechaCorte = Convert.ToDateTime(dateFinal.EditValue);
                    rep.xrCeTitle.Text += " " + FechaCorte.ToShortDateString();
                    rep.XrCeTitulo.Text += " " + FechaCorte.ToShortDateString();
                    rep.XrCeFechaBottom.Text = "Fecha " + FechaCorte.Day + " " + ((Parametros.Meses)(FechaCorte.Month)).ToString() + " " + FechaCorte.Year;
                    rep.XrCeCheque.Text = texto; //"EFECTUAR CK A NOMBRE DE: " + Parametros.General.Empresa.Nombre;

                    /*
                    string textoCS = "";
                    //Listar las transferencias de las ES
                    if (!lkES.EditValue.Equals(0))
                    {
                        var tes = db.CuentaBancarias.Where(cb => cb.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).Select(s => s.Nombre);

                        tes.ToList().ForEach(o => textoCS += o + ", ");

                        rep.XrCeTransferenciaCS.Text = "PAGO TRANSFERENCIA C$: EN " + textoCS;
                    }

                    string textoUS = "";
                    var tesUS = db.CuentaBancarias.Where(cb => cb.EstacionServicioID.Equals(12) && cb.MonedaID.Equals(2)).Select(s => s.Nombre);

                    tesUS.ToList().ForEach(o => textoUS += o + ", ");

                    rep.XrCeTransferenciaUS.Text = "PAGO TRANSFERENCIA U$: EN " + textoUS;
                    */

                    //Usuario Ingresado al Sistema (Elabora Reporte)
                    rep.xrCeUsuario.Text = NombreUsuario;

                    e.Result = rep;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();

                    // var Cliente = db.Clientes.Select( s => new {s.Codigo, s.Nombre, s.Direccion, s.Telefono1, s.Telefono2, s.Telefono3, s.CuentaContableID, s.ID}).Single(c => c.ID.Equals(det));

                    // //Datos Cliente
                    // rep.XrCeCodigo.Text = Cliente.Codigo;
                    // rep.XrCeCliente.Text = Cliente.Nombre;
                    // rep.XrCeDireccion.Text = Cliente.Direccion;
                    // rep.XrCeTel1.Text = Cliente.Telefono1;
                    // rep.XrCeTel2.Text = Cliente.Telefono2;
                    // rep.XrCeTel3.Text = Cliente.Telefono3;

                    // //Datos Fecha
                    // rep.XrCeFechaInicial.Text = Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                    // rep.XrCeFechaFinal.Text = "";                       
                    // rep.XrCeFechaImpresion.Text = vFecha.ToShortDateString();
                    // rep.XrCeMoneda.Text = db.Monedas.Single(m => m.ID.Equals(1)).Simbolo;

                    // //SALDOS
                    //// decimal pSaldo = 0;
                    // decimal sSaldo = 0;
                    // bool SUS = (lkSES.EditValue == null ? false : true);

                    //#region <<< SALDO ANTERIOR >>>
                    //Saldo Antes del Corte

                    //var Lista = (from vm in dbView.VistaMovimientos
                    //             where (vm.MovimientoTipoID.Equals(7) || vm.MovimientoTipoID.Equals(27))
                    //             && !vm.Anulado && !vm.Pagado
                    //             && vm.ClienteID.Equals(det)
                    //             && vm.FechaContabilizacion.Date < Convert.ToDateTime(dateInicial.EditValue).Date
                    //             && (vm.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                    //             && (!SUS || (SUS && ((vm.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                    //             select vm).OrderBy(o => o.FechaContabilizacion).ToList();

                    //decimal x = 0, y = 0;
                    //if (Lista.Count > 0)
                    //{
                    //    x = Decimal.Round(Lista.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                    //    y = Decimal.Round(Lista.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                    //}

                    //pSaldo = x - y;

                    //DevExpress.XtraReports.UI.XRSubreport xrSubAnterior = new DevExpress.XtraReports.UI.XRSubreport();
                    //xrSubAnterior.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha, Convert.ToDateTime(dateInicial.EditValue).Date, 0, "ANTES", Lista);
                    //xrSubAnterior.ReportSource = new ReportesAF.SubRptECClientes(vFecha, Convert.ToDateTime(dateInicial.EditValue).Date, 0, "ANTES", Lista);
                    //rep.GroupHeader2.Controls.Add(xrSubAnterior);

                    #endregion


                    ////////////////////////////////if
                    #region <<< SALDO CORTADO >>>
                    //Saldo Antes del Corte
                    //bool HayFilas = false;
                    //if (!chkCorte.Checked)
                    //{



                    //if (ListaD.Count > 0) HayFilas = true;
                    //DevExpress.XtraReports.UI.XRSubreport xrSubDespues = new DevExpress.XtraReports.UI.XRSubreport();
                    ////xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha, Convert.ToDateTime(dateInicial.EditValue).Date, pSaldo, "DESPUES", ListaD);
                    //xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(Convert.ToDateTime(dateFinal.EditValue).Date, Convert.ToDateTime(dateFinal.EditValue).Date, 0, " CORTADAS HASTA", ListaD.OrderBy(o => o.FechaContabilizacion), _EsEmpleado);
                    //rep.GroupHeader1.Controls.Add(xrSubDespues);
                    //SaldoTotal += Decimal.Round(Convert.ToDecimal(ListaD.Sum(s => s.Monto - s.Abono)), 2, MidpointRounding.AwayFromZero);

                    //    decimal mt = 0, a = 0;
                    //    if (ListaD.Count > 0)
                    //    {
                    //        mt = Decimal.Round(ListaD.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                    //        a = Decimal.Round(ListaD.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                    //        HayFilas = true;
                    //    }

                    //    sSaldo = mt - a;
                    //    DevExpress.XtraReports.UI.XRSubreport xrSubDespues = new DevExpress.XtraReports.UI.XRSubreport();
                    //    //xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha, Convert.ToDateTime(dateInicial.EditValue).Date, pSaldo, "DESPUES", ListaD);
                    //    xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha, Convert.ToDateTime(dateFinal.EditValue).Date, 0, " CORTADAS HASTA", ListaD, _EsEmpleado);
                    //    rep.GroupHeader1.Controls.Add(xrSubDespues);
                    //    SaldoTotal += Decimal.Round(ListaD.Sum(s => s.Monto - s.Abono), 2, MidpointRounding.AwayFromZero);
                    //#endregion


                    //}
                    //else
                    //{
                    //    //PENDIENTES AL CORTE                                
                    //    var ListaD = (from v in dbView.MovimientosPagados(Convert.ToDateTime(dateFinal.EditValue).Date, (Convert.ToInt32(lkTipoCta.EditValue).Equals(0) ? Cliente.CuentaContableID : Convert.ToInt32(lkTipoCta.EditValue)))
                    //                  where v.ClienteID.Equals(det)
                    //                  && (v.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                    //                  select v).OrderBy(o => o.FechaContabilizacion).ToList();
                    //    if (ListaD.Count > 0) HayFilas = true;
                    //    DevExpress.XtraReports.UI.XRSubreport xrSubDespues = new DevExpress.XtraReports.UI.XRSubreport();
                    //    //xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(vFecha, Convert.ToDateTime(dateInicial.EditValue).Date, pSaldo, "DESPUES", ListaD);
                    //    xrSubDespues.ReportSource = new SAGAS.Reportes.Ventas.Hojas.SubRptECClientes(Convert.ToDateTime(dateFinal.EditValue).Date, Convert.ToDateTime(dateFinal.EditValue).Date, 0, " CORTADAS HASTA", ListaD, _EsEmpleado);
                    //    rep.GroupHeader1.Controls.Add(xrSubDespues);
                    //    SaldoTotal += Decimal.Round(Convert.ToDecimal(ListaD.Sum(s =>  s.Monto - s.Abono)), 2, MidpointRounding.AwayFromZero);
                    //}

                    ////////////////

                    //        if (HayFilas)
                    //        {
                    //            DevExpress.XtraReports.UI.GroupHeaderBand GroupHSub = new DevExpress.XtraReports.UI.GroupHeaderBand();

                    //            DevExpress.XtraReports.UI.XRSubreport SubRep = new DevExpress.XtraReports.UI.XRSubreport();
                    //            SubRep.ReportSource = rep;
                    //            GroupHSub.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
                    //            GroupHSub.Controls.Add(SubRep);
                    //            Mrep.Bands.Add(GroupHSub);
                    //        }      
                    //        //Mrep.Detail.Controls.Add(SubRep);
                    //    }
                    //});

                    #endregion

                    //Mrep.xrCeSaldoFInal.Text = SaldoTotal.ToString("#,0.00");


                }                
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void bgwOff_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    if (_EsEmpleado)
                    {
                        Erep = ((SAGAS.Reportes.Nomina.Hojas.SubRptECEmpleado)(e.Result));
                        Erep.CreateDocument(true);
                        this.printControlAreaReport.PrintingSystem = Erep.PrintingSystem;
                    }
                    else
                    {
                        Srep = ((SAGAS.Reportes.Ventas.Hojas.SubRptECClientes)(e.Result));
                        Srep.CreateDocument(true);
                        this.printControlAreaReport.PrintingSystem = Srep.PrintingSystem;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
    

        #endregion

        //private async void lkTipoCta_EditValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (lkTipoCta.EditValue != null)
        //        {   
        //            await ListaTotales(Convert.ToInt32(lkTipoCta.EditValue)).ConfigureAwait(false);                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
        //    }
        //}

        //Metodo Asyncrono para cargar la lista
        async Task ListaTotales(int LkValue)
        {
            try
            {
                await Task.Delay(10);
                //if (Convert.ToInt32(LkValue).Equals(_CuentaEmpleadoID))
                //{
                //    _EsEmpleado = true;

                //    glkCliente.EditValue = null;
                //    glkCliente.Properties.DataSource = null;
                //    glkCliente.Properties.NullText = "<Seleccione el Empleado>";
                //    switchReport.Properties.ReadOnly = false;
                //    if (Convert.ToInt32(LkValue) > 0)
                //    {
                //        _ListClientesToShow.Clear();

                //        _ListClientesToShow = (from em in db.Empleados
                //                               join m in db.Movimientos on em.ID equals m.EmpleadoID
                //                               where m.EstacionServicioID.Equals(IDEstacionServicio)
                //                               group em by new { em.ID, em.Codigo, em.Nombres, em.Apellidos } into gr
                //                               select new ListaClientesCuentas
                //                               {
                //                                   ID = gr.Key.ID,
                //                                   Codigo = gr.Key.Codigo,
                //                                   Nombre = gr.Key.Nombres + " " + gr.Key.Apellidos,
                //                                   Display = gr.Key.Codigo + " | " + gr.Key.Nombres + " " + gr.Key.Apellidos,
                //                                   CuentaID = 1
                //                               }).ToList();

                //        _ListClientesToShow.Insert(0, new ListaClientesCuentas { ID = 0, Codigo = "", Nombre = "TODOS", Display = "TODOS", CuentaID = 0 });
                //        glkCliente.Properties.DataSource = _ListClientesToShow.Select(s => new { s.ID, s.Codigo, s.Nombre, s.Display, s.CuentaID }).ToList();

                //    }
                //}
                //else
                //{ 
                //if(Convert.ToInt32(LkValue).Equals()){    
                _EsEmpleado = false;

                    glkCliente.EditValue = null;
                    glkCliente.Properties.DataSource = null;
                    glkCliente.Properties.NullText = "<Seleccione el Deudor>";
                    switchReport.Properties.ReadOnly = false;
                    if (Convert.ToInt32(LkValue) > 0)
                    {
                        _ListClientesToShow.Clear();
                        _ListClientesToShow = _ListClientes.Where(o => o.CuentaID.Equals((int)LkValue)).Select(s => new ListaClientesCuentas { ID = s.ID, Codigo = s.Codigo, Nombre = s.Nombre, Display = s.Display, CuentaID = s.CuentaID }).ToList();
                        _ListClientesToShow.Insert(0, new ListaClientesCuentas { ID = 0, Codigo = "", Nombre = "TODOS", Display = "TODOS", CuentaID = 0 });
                        glkCliente.Properties.DataSource = _ListClientesToShow.Select(s => new { s.ID, s.Codigo, s.Nombre, s.Display, s.CuentaID }).ToList();
                        if (Convert.ToInt32(LkValue) == _ClienteAnticipo)
                        {
                            switchReport.IsOn = true;
                            switchReport.Properties.ReadOnly = true;
                        }
                    }
                    else if (Convert.ToInt32(LkValue) == 0)
                    {
                        _ListClientesToShow.Clear();
                        _ListClientesToShow = _ListClientes./*Where(o => o.CuentaID.Equals((int)lkTipoCta.EditValue)).*/Select(s => new ListaClientesCuentas { ID = s.ID, Codigo = s.Codigo, Nombre = s.Nombre, Display = s.Display, CuentaID = s.CuentaID }).ToList();
                        _ListClientesToShow.Insert(0, new ListaClientesCuentas { ID = 0, Codigo = "", Nombre = "TODOS", Display = "TODOS", CuentaID = 0 });
                        glkCliente.Properties.DataSource = _ListClientesToShow.Select(s => new { s.ID, s.Codigo, s.Nombre, s.Display, s.CuentaID }).ToList();

                    }
                }
            //}
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void switchReport_Toggled(object sender, EventArgs e)
        {
            if (switchReport.IsOn)
            {
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlGroupFechas.Text = "Fecha Rango Período";
            }
            else if (!switchReport.IsOn)
            {
                layoutControlItem8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlGroupFechas.Text = "Fecha Corte";                
            }
        }

       
    }
}