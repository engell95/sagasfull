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
    public partial class FormRptEstadoProveedor : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private int Usuario = Parametros.General.UserID;
        private string NombreUsuario = Parametros.General.UserName;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private static Entidad.Proveedor prov;
        private List<Entidad.VistaEstadoCuentaProveedor> EtVista;
        private BackgroundWorker bgwOff;
        private BackgroundWorker bgwOn;
        private SAGAS.Reportes.Ventas.Hojas.RptECClientesMain MrepMain;/*---------*/
        private SAGAS.Reportes.Tesoreria.Hojas.SubRptECProveedor Srep;
        private List<ListaProveedorCuentas> _ListProveedorToShow = new List<ListaProveedorCuentas>();

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

        private int IDProveedor
        {
            get { return Convert.ToInt32(glkProveedor.EditValue); }
            set { glkProveedor.EditValue = value; }
        }

        struct ListaProveedorCuentas
        {
            public int ID;
            public string Codigo;
            public string Nombre;
            public string Display;
            public int CuentaID;
        }

        private List<ListaProveedorCuentas> _ListProveedor;
        #endregion

        #region *** INICIO ***

        public FormRptEstadoProveedor()
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
            this.tcCliente.Text = "Proveedor   :";
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
                //Limpiando los Proveedores
                glkProveedor.Properties.DataSource = null;
                //Cargando los proveedores
                _ListProveedor = (from p in db.Proveedors
                        where p.Activo
                        group p by new {  p.ID, p.Codigo, p.Nombre, p.CuentaContableID } into gr
                                 select new ListaProveedorCuentas
                        {
                            ID = gr.Key.ID,
                            Codigo = gr.Key.Codigo,
                            Nombre = gr.Key.Nombre,
                            Display = gr.Key.Codigo + " | " + gr.Key.Nombre,
                            CuentaID = gr.Key.CuentaContableID
                        }).ToList();

                //Estaciones de Servicio
                var lista = (from es in db.EstacionServicios
                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                            select new { es.ID, es.Nombre }).ToList();

                if (Convert.ToInt32(db.GetViewEstacionesServicioByUsers.Count(ges => ges.UsuarioID == Usuario)) >= Convert.ToInt32(db.EstacionServicios.Count(o => o.Activo)))
                    lista.Insert(0, new { ID = 0, Nombre = "TODAS" });

                lkES.Properties.DataSource = lista;
                lkES.EditValue = Parametros.General.EstacionServicioID;

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;
                lkMesInicial_Validated(null, null);
                lkMesFinal.EditValue = DateTime.Now.Month;
                lkMesFinal_Validated(null, null);

                var obj = (from pr in db.Proveedors
                          join cc in db.CuentaContables on pr.CuentaContableID equals cc.ID
                          group pr by new { pr.CuentaContableID, cc.Codigo, cc.Nombre } into gr
                          select new
                          {
                              CuentaID = gr.Key.CuentaContableID,
                              CuentaCodigo = gr.Key.Codigo,
                              CuentaNombre = gr.Key.Nombre
                          }).ToList();

                obj.Insert(0, new { CuentaID = 0, CuentaCodigo = "0000000000", CuentaNombre = "TODAS" });


                lkTipoCta.Properties.DataSource = obj.Select(s => new { ID = s.CuentaID, Display = s.CuentaCodigo + " | " + s.CuentaNombre }).ToList();
                lkTipoCta.EditValue = 0;

                switchReport.IsOn = false;
                switchReport_Toggled(null, null);
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
            if (lkTipoCta.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar el tipo de cuenta.", Parametros.MsgType.warning);
                return false;
            }

            if (IDProveedor < 0)
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

                if (IDProveedor.Equals(0) && Convert.ToInt32(lkTipoCta.EditValue).Equals(0))
                {
                    Parametros.General.DialogMsg("Debe seleccinar un Proveedor.", Parametros.MsgType.warning);
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
                        bgwOn.WorkerReportsProgress = true;
                        bgwOn.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOn_RunWorkerCompleted);
                        bgwOn.DoWork += new DoWorkEventHandler(bgwOn_DoWork);
                        bgwOn.ProgressChanged += new ProgressChangedEventHandler(bgwOn_ProgressChanged);
                        bgwOn.RunWorkerAsync();
                    }
                    else if (!switchReport.IsOn)
                    {
                        //Facturas Pendientes
                        bgwOff = new BackgroundWorker();
                        bgwOff.WorkerReportsProgress = true;
                        bgwOff.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwOff_RunWorkerCompleted);
                        bgwOff.DoWork += new DoWorkEventHandler(bgwOff_DoWork);
                        bgwOff.ProgressChanged += new ProgressChangedEventHandler(bgwOff_ProgressChanged);
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
                decimal _SaldoTotal = 0;

                if (IDProveedor.Equals(0))
                    ListaCL.AddRange(_ListProveedorToShow.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                else
                    ListaCL.Add(IDProveedor);

                int z = ListaCL.Count;
                decimal x = 0;
                ListaCL.ForEach(det =>
                {

                    
                    bool SUS = (lkSES.EditValue == null ? false : true);

                    EtVista = (from vcp in dv.VistaEstadoCuentaProveedor
                               where vcp.ID.Equals(det)
                                && vcp.FechaRegistro.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                                && (vcp.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                && (!SUS || (SUS && ((vcp.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                               select vcp).ToList();

                    List<Entidad.VistaEstadoCuentaProveedor> vistaMasivo = (from vcp in dv.VistaEstadoCuentaProveedorPagoMasivos
                                                                            where vcp.ID.Equals(det)
                            && Convert.ToDateTime(vcp.FechaRegistro).Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                            && (vcp.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                            && (!SUS || (SUS && ((vcp.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                                                            select new Entidad.VistaEstadoCuentaProveedor
                                                                            {
                                                                                ID = vcp.ID,
                                                                                Proveedores = vcp.Proveedores,
                                                                                RUC = vcp.RUC,
                                                                                Direccion = vcp.Direccion,
                                                                                Telefonos = vcp.Telefonos,
                                                                                FechaRegistro = Convert.ToDateTime(vcp.FechaRegistro),
                                                                                TipoMovimiento = vcp.TipoMovimiento,
                                                                                Comentario = vcp.Comentario,
                                                                                EstacionServicioID = vcp.EstacionServicioID,
                                                                                SubEstacionID = vcp.SubEstacionID,
                                                                                EstacionNombre = vcp.EstacionNombre,
                                                                                NumeroDocumento = vcp.NumeroDocumento,
                                                                                Monto = vcp.Monto,
                                                                                Pagado = Convert.ToBoolean(vcp.Pagado)
                                                                            }).ToList();

                    EtVista.AddRange(vistaMasivo);
                    EtVista.OrderBy(o => o.FechaRegistro);
                    var lSaldo = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date < Convert.ToDateTime(dateInicial.EditValue).Date);

                    decimal vSaldo = 0;
                    Boolean bAnticipo = false;
                    if (lSaldo.Count() > 0)
                    {
                        vSaldo = Convert.ToDecimal(lSaldo.Sum(s => s.Monto));

                        if (EtVista.Count(c => c.ID.Equals(det) && c.MovimientoTipoID.Equals(61)) > 0)//(lSaldo.Count(c => c.MovimientoTipoID.Equals(61)) > 0)
                        {
                            vSaldo = (vSaldo * -1);
                            bAnticipo = true;
                        }
                    }

                    var Lista = EtVista.Where(o => o.ID.Equals(det) && o.FechaRegistro.Date >= Convert.ToDateTime(dateInicial.EditValue).Date).OrderBy(o => o.FechaRegistro).ToList();
                    /*******************************************************/
                    x++;
                    bgwOn.ReportProgress((int)Math.Ceiling((x / z) * 100));

                    if (Lista.Count == 0 && vSaldo.Equals(0)) return;
                    ///////////////////////////////////////////////////////////
                    
                    SAGAS.Reportes.Tesoreria.Hojas.RptECProveedorMov Mrep = new SAGAS.Reportes.Tesoreria.Hojas.RptECProveedorMov();
                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                    Mrep.PicLogo.Image = picture_LogoEmpresa;
                    Mrep.CeEmpresa.Text = Nombre;
                    Mrep.xrCeRango.Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();

                    DevExpress.XtraReports.UI.GroupHeaderBand ghEstacion = new DevExpress.XtraReports.UI.GroupHeaderBand();

                    DevExpress.XtraReports.UI.XRSubreport xrSubDetalle = new DevExpress.XtraReports.UI.XRSubreport();

                    XRTablaDatos();
                    /////////////////////////////////////////////////////////////

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
                    xrtbDatos.Rows[1].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Proveedores : "");
                    xrtbDatos.Rows[2].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().RUC : "");
                    xrtbDatos.Rows[3].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Direccion : "");
                    xrtbDatos.Rows[4].Cells[1].Text = (EtVista.Count() > 0 ? EtVista.First().Telefonos : "");
                    xrtbDatos.Rows[5].Cells[1].Text = "Desde " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + " Al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
                    ghEstacion.Controls.Add(xrtbDatos);

                    xrSubDetalle.ReportSource = new SAGAS.Reportes.Tesoreria.Hojas.SubRptECProveedorMov(Lista, vFecha.ToString(), vSaldo, bAnticipo);
                    xrSubDetalle.LocationF = new PointF(0f, 150f);
                    ghEstacion.Controls.Add(xrSubDetalle);
                    ghEstacion.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                    Mrep.Bands.Add(ghEstacion);

                    DevExpress.XtraReports.UI.GroupHeaderBand GroupHSub = new DevExpress.XtraReports.UI.GroupHeaderBand();

                    DevExpress.XtraReports.UI.XRSubreport SubRep = new DevExpress.XtraReports.UI.XRSubreport();
                    SubRep.ReportSource = Mrep;
                    //_SaldoTotal += Decimal.Round(vSaldo + Convert.ToDecimal(Lista.Sum(s => s.Monto)), 2, MidpointRounding.AwayFromZero);
                    GroupHSub.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
                    GroupHSub.Controls.Add(SubRep);
                    MainMrep.Bands.Add(GroupHSub);

                    
                });

                //MainMrep.xrCeSaldoFInal.Text = _SaldoTotal.ToString("#,0.00");
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

        private void bgwOn_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgreso.EditValue = e.ProgressPercentage;
        }

        ///SWITCH OFF

        private void bgwOff_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTVISTAPREVIA);
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dbView.CommandTimeout = 600;
                //Fecha del Servidor
                DateTime vFecha = Convert.ToDateTime(db.GetDateServer());
                SAGAS.Reportes.Tesoreria.Hojas.SubRptECProveedor rep = new Reportes.Tesoreria.Hojas.SubRptECProveedor(Convert.ToDateTime(dateFinal.EditValue));
                rep.RequestParameters = false;                
                //Lista de los proveedores
                List<Int32> ListaPR = new List<int>();
                //Llenando la lista de los proveedores
                if (IDProveedor.Equals(0))
                    ListaPR.AddRange(_ListProveedorToShow.Where(o => !o.ID.Equals(0)).Select(s => s.ID));
                else
                    ListaPR.Add(IDProveedor);

                //Parametros de la empresa
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
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

                //Cuenta Contable
                int vCuenta = 0;

                if (lkTipoCta.EditValue != null)
                {
                    if (Convert.ToInt32(lkTipoCta.EditValue) > 0)
                    {
                    vCuenta = Convert.ToInt32(lkTipoCta.EditValue);
                    }
                    else
                    {
                        vCuenta = _ListProveedorToShow.Single(p => p.ID.Equals(IDProveedor)).CuentaID;
                    }
                }
                else
                {
                    vCuenta = (_ListProveedorToShow.Count(p => p.ID.Equals(IDProveedor)) > 0 ? _ListProveedorToShow.First(p => p.ID.Equals(IDProveedor)).CuentaID : 0);
                }

                        rep.XrCeFechaImpresion.Text = vFecha.ToShortTimeString();
                        rep.XrCeMoneda.Text = db.Monedas.Single(m => m.ID.Equals(1)).Simbolo;

                        bool SUS = (lkSES.EditValue == null ? false : true);
                        
                        
                        #region <<< SALDO CORTADO >>>
                        //if (!chkCorte.Checked)
                        //{
                        //    ////////////////////////////////if
                        //    
                        //    //ULTIMAS PENDIENTES
                        //    var ListaD = (from vm in dbView.VistaMovimientos
                        //                  where (vm.MovimientoTipoID.Equals(3) || vm.MovimientoTipoID.Equals(5) || (vm.MovimientoTipoID.Equals(41)))
                        //                  && !vm.Anulado && !vm.Pagado
                        //                  && vm.ProveedorID.Equals(det)
                        //                  && vm.FechaContabilizacion.Date <= Convert.ToDateTime(dateFinal.EditValue).Date
                        //                  && (vm.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                        //                  && (!SUS || (SUS && ((vm.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                        //                  select vm).OrderBy(o => o.FechaContabilizacion).ToList();

                        //    decimal mt = 0, a = 0;
                        //    if (ListaD.Count > 0)
                        //    {
                        //        mt = Decimal.Round(ListaD.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                        //        a = Decimal.Round(ListaD.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                        //        HayFilas = true;
                        //    }

                        //    sSaldo = mt - a;

                        //    DevExpress.XtraReports.UI.XRSubreport xrSubDespues = new DevExpress.XtraReports.UI.XRSubreport();
                        //    xrSubDespues.ReportSource = new SAGAS.Reportes.Tesoreria.Hojas.SubRptECProveedor(vFecha, Convert.ToDateTime(dateFinal.EditValue).Date, 0, " CORTADAS HASTA", ListaD);
                        //    rep.GroupHeader1.Controls.Add(xrSubDespues);
                        //    _SaldoTotal += Decimal.Round(ListaD.Sum(s => s.Monto - s.Abono), 2, MidpointRounding.AwayFromZero);
                        //    #endregion


                        //}
                        //else
                        //{
#endregion

                            //PENDIENTES AL CORTE
                        var ListaD = (from v in dbView.DocumentosPagadosProveedor(Convert.ToDateTime(dateFinal.EditValue).Date, Convert.ToInt32(vCuenta))
                                        where ListaPR.Contains(v.ProveedorID)
                                        && (v.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                        && (!SUS || (SUS && ((v.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                        select v).OrderBy(o => o.FechaContabilizacion).ToList();

                //Llenado de Datos
                rep.DataSource = ListaD;

                //Resultado Tarea Asyncrona
                e.Result = rep;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
                    Srep = ((SAGAS.Reportes.Tesoreria.Hojas.SubRptECProveedor)(e.Result));
                    Srep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Srep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
    
        private void bgwOff_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgreso.EditValue = e.ProgressPercentage;
        }

        /// 
        #endregion

        private void lkTipoCta_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkTipoCta.EditValue != null)
                {
                    glkProveedor.EditValue = null;
                    glkProveedor.Properties.DataSource = null;
                    if (Convert.ToInt32(lkTipoCta.EditValue) > 0)
                    {
                        _ListProveedorToShow.Clear();
                        _ListProveedorToShow = _ListProveedor.Where(o => o.CuentaID.Equals((int)lkTipoCta.EditValue)).Select(s => new ListaProveedorCuentas { ID = s.ID, Codigo = s.Codigo, Nombre = s.Nombre, Display = s.Display, CuentaID = s.CuentaID }).ToList();
                        _ListProveedorToShow.Insert(0, new ListaProveedorCuentas { ID = 0, Codigo = "", Nombre = "TODOS", Display = "TODOS", CuentaID = 0 });
                        glkProveedor.Properties.DataSource = _ListProveedorToShow.Select(s => new { s.ID, s.Codigo, s.Nombre, s.Display, s.CuentaID }).ToList();
                        glkProveedor.EditValue = 0;
                    }
                    else if (Convert.ToInt32(lkTipoCta.EditValue) == 0)
                    {
                        _ListProveedorToShow.Clear();
                        _ListProveedorToShow = _ListProveedor.Select(s => new ListaProveedorCuentas { ID = s.ID, Codigo = s.Codigo, Nombre = s.Nombre, Display = s.Display, CuentaID = s.CuentaID }).ToList();
                        glkProveedor.Properties.DataSource = _ListProveedorToShow.Select(s => new { s.ID, s.Codigo, s.Nombre, s.Display, s.CuentaID }).ToList();
                        glkProveedor.EditValue = null;
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
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

        private void glkProveedor_EditValueChanged(object sender, EventArgs e)
        {
            if (IDProveedor > 0)
            {
                prov = db.Proveedors.Single(p => p.ID.Equals(IDProveedor));

                var obj = (from d in db.Deudors
                           join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)) on d.MovimientoID equals m.ID
                           join c in db.Proveedors.Where(c => c.ID.Equals(prov.ID)) on d.ProveedorID equals c.ID
                           group d by new { d.ProveedorID, c.Nombre } into gr
                           select new
                           {
                               ID = gr.Key.ProveedorID,
                               Proveedor = gr.Key.Nombre,
                               suma = gr.Sum(s => s.Valor)
                           }).ToList();

                spLimite.Value = prov.LimiteCredito;

                decimal Saldo = (db.Deudors.Count(d => d.ProveedorID.Equals(IDProveedor)) > 0 ? db.Deudors.Where(d => d.ProveedorID.Equals(IDProveedor)).Sum(s => s.Valor) : 0m);
                spSaldo.Value = Saldo;

                decimal _Disponible = prov.LimiteCredito - Saldo;
                spDisponible.Value = _Disponible;
            }
            else
            {
                spLimite.Value = 0m;
                spSaldo.Value = 0m;
                spDisponible.Value = 0m;
            }
        }

        private void chkCorte_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormRptEstadoProveedor_Load(object sender, EventArgs e)
        {
            layoutControlItem5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem13.Text = "Lista Proveedor";
        }
       
    }
}
