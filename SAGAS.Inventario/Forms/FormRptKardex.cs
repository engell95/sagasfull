using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SAGAS.Inventario.Forms
{
    public partial class FormRptKardex : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private DataTable dtProductos;
        private int Usuario = Parametros.General.UserID;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        private List<Parametros.ListIdDisplay> EtClase = new List<Parametros.ListIdDisplay>();
        private IQueryable<Parametros.ListIdString1String3String3> listaRango;// = new Parametros.ListIdString1String3String3();
        private List<Parametros.ListKardexProducto> EtKardex = new List<Parametros.ListKardexProducto>();
        internal List<Entidad.Tanque> EtTanques;
        internal List<Entidad.Almacen> EtAlmacenes;
        internal int _Combustible;
        private BackgroundWorker bgw;
        Reportes.Inventario.Hojas.RptKardex Mrep;

        private int IDClase
        {
            get { return Convert.ToInt32(lkClase.EditValue); }
            set { lkClase.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public FormRptKardex()
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
    
                //Clase Combustible
                _Combustible = Parametros.Config.ProductoClaseCombustible();

                //Cargar los productos
                EtKardex = (from p in db.Productos
                            join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                            join ar in db.Areas on pc.AreaID equals ar.ID
                            where p.Activo && pc.Activo && ar.Activo && !ar.EsServicio && !p.EsServicio
                            select new Parametros.ListKardexProducto
                            {
                                ID = p.ID,
                                Codigo = p.Codigo,
                                Nombre = p.Nombre,
                                ClaseID = pc.ID,
                                ClaseNombre = pc.Nombre,
                                AreaID = ar.ID,
                                AreaNombre = ar.Nombre
                            }).ToList();

                tgCombustible.IsOn = false;
                tgCombustible_Toggled(null, null);                

                //Areas
                var listaArea = EtKardex.Where(o => !o.ClaseID.Equals(_Combustible)).GroupBy(g => new { g.AreaID, g.AreaNombre }).Select(s => new { ID = s.Key.AreaID, Display = s.Key.AreaNombre }).ToList();
                listaArea.Insert(0, new { ID = 0, Display = "TODAS" });
                lkArea.Properties.DataSource = listaArea;
                

                EtTanques = db.Tanques.Where(t => t.Activo).ToList();
                EtAlmacenes = db.Almacens.Where(a => a.Activo).ToList();



                //Tipos de Movimiento
                 var tipos = (from mt in db.MovimientoTipos where mt.AplicaKardex select new { mt.ID, mt.Nombre }).ToList();
                tipos.Insert(0, new { ID = 0, Nombre = "TODOS"});

                lkTipoMov.Properties.DataSource = tipos;
                lkTipoMov.EditValue = 0;

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
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

            if (EtKardex.Count(c => c.Selected) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos un Producto de la lista.", Parametros.MsgType.warning);
                return false;
            }

            if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            {
                Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                return false;
            }

            if (lkTipoMov.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar el tipo de movimiento", Parametros.MsgType.warning);
                return false;
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

        private void GetAlmacenes(bool Mostrar, bool SubEstacion, int ID)
        {
            if (Mostrar)
            {
                if (!SubEstacion)
                {
                    if (!tgCombustible.IsOn)
                    {
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var Alma = EtTanques.Where(a => a.EstacionServicioID.Equals(ID) && a.Activo).Select(s => new { s.ID, s.Nombre }).ToList();
                        Alma.Insert(0, new { ID = 0, Nombre = "TODOS" });
                        lkAlmacen.Properties.DataSource = Alma;
                        this.layoutControlItemAlma.Text = "Tanques";
                        this.lkAlmacen.Properties.NullText = "<Seleccione el Tanque>";
                        this.lkAlmacen.EditValue = null;
                    }
                    else
                    {
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var Alma = EtAlmacenes.Where(a => a.EstacionServicioID.Equals(ID) && a.Activo).Select(s => new { s.ID, s.Nombre }).ToList();
                        Alma.Insert(0, new { ID = 0, Nombre = "TODOS" });
                        lkAlmacen.Properties.DataSource = Alma;
                        this.layoutControlItemAlma.Text = "Almacen";
                        this.lkAlmacen.Properties.NullText = "<Seleccione el Almacén>";
                        this.lkAlmacen.EditValue = null;
                    }
                }
                else if (SubEstacion)
                {
                    if (!tgCombustible.IsOn)
                    {
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var Alma = EtTanques.Where(a => a.SubEstacionID.Equals(ID) && a.Activo).Select(s => new { s.ID, s.Nombre }).ToList();
                        Alma.Insert(0, new { ID = 0, Nombre = "TODOS" });
                        lkAlmacen.Properties.DataSource = Alma;
                        this.layoutControlItemAlma.Text = "Tanques";
                        this.lkAlmacen.EditValue = null;
                    }
                    else
                    {
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        var Alma = EtAlmacenes.Where(a => a.SubEstacionID.Equals(ID) && a.Activo).Select(s => new { s.ID, s.Nombre }).ToList();
                        Alma.Insert(0, new { ID = 0, Nombre = "TODOS" });
                        lkAlmacen.Properties.DataSource = Alma;
                        this.layoutControlItemAlma.Text = "Almacen";
                        this.lkAlmacen.EditValue = null;
                    }
                }
            }
            else
            {
                this.lkAlmacen.EditValue = null;
                this.lkAlmacen.Properties.DataSource = null;
                this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
        
      

        private void lkSES_EditValueChanged(object sender, EventArgs e)
        {
            if (lkSES.EditValue != null)
            {
                if (Convert.ToInt32(lkSES.EditValue).Equals(0))
                {
                    GetAlmacenes(false, false, 0);
                }
                else if (!Convert.ToInt32(lkSES.EditValue).Equals(0))
                {
                    var SES = db.SubEstacions.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkSES.EditValue)));

                    if (SES != null)
                    {
                        GetAlmacenes(true, true, SES.ID);
                    }
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

                    GetAlmacenes(false, false, 0);
                }
                else if (!Convert.ToInt32(lkES.EditValue).Equals(0))
                {
                    var ES = db.EstacionServicios.SingleOrDefault(s => s.ID.Equals(Convert.ToInt32(lkES.EditValue)));

                    if (ES != null)
                    {       
                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).Select(s => new {s.ID, s.Nombre})).ToList();

                        if (Sus.Count > 0)
                        {
                            this.lkSES.EditValue = null;
                            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            Sus.Insert(0, new { ID = 0, Nombre = "TODOS" });
                            lkSES.Properties.DataSource = Sus;

                            GetAlmacenes(false, false, 0);
                        }
                        else
                        {
                            this.lkSES.EditValue = null;
                            this.lkSES.Properties.DataSource = null;
                            this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                            GetAlmacenes(true, false, ES.ID);
                        }
                            
                    }
                    else 
                    {
                        this.lkSES.EditValue = null;
                        this.lkSES.Properties.DataSource = null;
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        GetAlmacenes(false, false, 0);
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
                dbView.CommandTimeout = 900;
                Reportes.Inventario.Hojas.RptKardex rep = new Reportes.Inventario.Hojas.RptKardex();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;

                if (lkES.EditValue.Equals(0))
                    rep.CeEstacion.Text = "Todas las Estaciones";
                else
                {
                    if (lkSES.EditValue == null)
                        rep.CeEstacion.Text = Convert.ToString(lkES.Text);
                    else
                    {
                        if (lkSES.EditValue.Equals(0))
                            rep.CeEstacion.Text = "Todas las Sub Estaciones";
                        else
                            rep.CeEstacion.Text = Convert.ToString(lkSES.Text);
                    }
                }

                rep.CeTitulo.Text = "Reporte Kardex     Del:  " + Convert.ToDateTime(dateInicial.EditValue).ToShortDateString() + "   Al:  " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();

                var Selected = (gridPre.DataSource as List<Parametros.ListKardexProducto>).Where(o => o.Selected);
                foreach (var row in Selected)
                {
                    #region <<< SUB_REPORTS >>>
                    bool SUS = (lkSES.EditValue == null ? false : true);
                    bool Alma = (lkAlmacen.EditValue == null ? false : true);


                    var saldo = (from k in dbView.VistaKardexes
                                 join m in dbView.VistaMovimientos on k.VistaMovimiento equals m
                                 where k.ProductoID.Equals(Convert.ToInt32(row.ID)) && m.AplicaKardex && !k.EsManejo && !m.Anulado && !m.EsAnulado
                                && (m.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                && (!SUS || (SUS && ((m.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                && (!Alma || (Alma && ((k.AlmacenEntradaID.Equals(Convert.ToInt32(lkAlmacen.EditValue)) || k.AlmacenSalidaID.Equals(Convert.ToInt32(lkAlmacen.EditValue)) || Convert.ToInt32(lkAlmacen.EditValue).Equals(0)))))
                                && k.Fecha.Date < Convert.ToDateTime(dateInicial.EditValue).Date
                                 select new { k.CantidadEntrada, k.CantidadSalida, k.CostoFinal, k.CostoTotal, k.Fecha }).OrderByDescending(o => o.Fecha);

                    var query = (from k in dbView.VistaKardexes
                                 join m in dbView.VistaMovimientos on k.VistaMovimiento equals m
                                 where k.ProductoID.Equals(Convert.ToInt32(row.ID)) && m.AplicaKardex && !k.EsManejo && !m.Anulado && !m.EsAnulado
                                 && (m.MovimientoTipoID.Equals(Convert.ToInt32(lkTipoMov.EditValue)) || Convert.ToInt32(lkTipoMov.EditValue).Equals(0))
                                 && (m.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) || Convert.ToInt32(lkES.EditValue).Equals(0))
                                 && (!SUS || (SUS && ((m.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) || Convert.ToInt32(lkSES.EditValue).Equals(0)))))
                                 && (!Alma || (Alma && ((k.AlmacenEntradaID.Equals(Convert.ToInt32(lkAlmacen.EditValue)) || k.AlmacenSalidaID.Equals(Convert.ToInt32(lkAlmacen.EditValue)) || Convert.ToInt32(lkAlmacen.EditValue).Equals(0)))))
                                 && (k.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date && k.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date)
                                 select new
                                 {
                                     k.ID,
                                     k.EsProducto,
                                     m.Abreviatura,
                                     m.Referencia,
                                     m.Numero,
                                     m.Entrada,
                                     Descripcion = (m.MovimientoTipoID.Equals(3) ? m.ProveedorNombre : " "),
                                     k.ProductoCodigo,
                                     k.ProductoNombre,
                                     k.UnidadMedidaNombre,
                                     k.Fecha,
                                     k.EstacionNombre,
                                     k.SubEstacionNombre,
                                     k.AlmacenEntradaNombre,
                                     k.AlmacenSalidaNombre,
                                     k.CostoEntrada,
                                     k.CantidadEntrada,
                                     k.CantidadSalida,
                                     k.CostoSalida,
                                     k.Observacion,
                                     k.CostoFinal,
                                     k.CostoTotal
                                 }).OrderBy(o => o.Fecha).ThenBy(t => !t.Entrada).ThenBy(th => th.ID);

                    //if (query.Count() > 0)
                    //{
                    DevExpress.XtraReports.UI.GroupFooterBand GroupFooter = new DevExpress.XtraReports.UI.GroupFooterBand();

                    decimal vLitros = Decimal.Round((saldo.Count() > 0 ? saldo.Sum(s => s.CantidadEntrada) - saldo.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);
                    decimal vSaldo = 0m;
                    saldo.ToList().ForEach(s => vSaldo += ((s.CantidadEntrada >= 0 && s.CantidadSalida <= 0) ? Math.Abs(Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero))));

                    DevExpress.XtraReports.UI.XRSubreport xrSubKardex = new DevExpress.XtraReports.UI.XRSubreport();
                    xrSubKardex.ReportSource = new Reportes.Inventario.Hojas.SubRptDetalleKardex(row.Codigo, row.Nombre, query, Convert.ToDateTime(db.GetDateServer()).ToString(), vLitros, (saldo.Count() > 0 ? saldo.First().CostoFinal : 0m), vSaldo);
                    //xrSubKardex.ReportSource = new Reportes.Inventario.Hojas.SubRptDetalleKardex(query.First().ProductoCodigo, query.First().ProductoNombre, query, Convert.ToDateTime(db.GetDateServer()).ToString(), vLitros, (saldo.Count() > 0 ? saldo.First().CostoFinal : 0m), vSaldo);
                    GroupFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                    GroupFooter.Controls.Add(xrSubKardex);
                    rep.Bands.Add(GroupFooter);

                    //}
                    #endregion
                }

                rep.RequestParameters = false;
                //rep.CreateDocument(true);
                //printControlAreaReport.PrintingSystem = rep.PrintingSystem;
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
                    Mrep = ((Reportes.Inventario.Hojas.RptKardex)(e.Result));
                    Mrep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void tgCombustible_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (tgCombustible.IsOn)
                {
                    lkArea.EditValue = 0;
                    layoutControlGroupAC.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                }
                else
                {
                    lkArea.EditValue = null;
                    lkClase.EditValue = null;
                    layoutControlGroupAC.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    gvDataPre.ActiveFilter.Clear();
                    gridPre.DataSource = null;
                    gridPre.DataSource = EtKardex.Where(o => o.ClaseID.Equals(_Combustible)).ToList();
                    btnUnselect_Click(null, null);
                    gvDataPre.RefreshData();

                    if (lkSES.EditValue != null)
                        lkSES_EditValueChanged(null, null);
                    else
                    {
                        if (lkES.EditValue != null)
                            lkES_EditValueChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
               
        //Cargar la lista de las clases
        private void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkArea.EditValue != null)
                {
                    //Clases
                    EtClase.Clear();
                    EtClase = EtKardex.Where(o => !o.ClaseID.Equals(_Combustible) && (Convert.ToInt32(lkArea.EditValue).Equals(0) || o.AreaID.Equals(Convert.ToInt32(lkArea.EditValue)))).GroupBy(g => new { g.ClaseID, g.ClaseNombre }).Select(s => new Parametros.ListIdDisplay { ID = s.Key.ClaseID, Display = s.Key.ClaseNombre }).ToList();
                    EtClase.Insert(0, new Parametros.ListIdDisplay { ID = 0, Display = "TODAS" });
                    lkClase.Properties.DataSource = EtClase;
                    lkClase.EditValue = null;
                    lkClase.EditValue = 0;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        //Cargar Grid
        private void lkClase_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkClase.EditValue != null)
                {
                    int cl = Convert.ToInt32(lkClase.EditValue);
                    //Cargar Productos
                    gvDataPre.ActiveFilter.Clear();
                    gridPre.DataSource = null;
                    gridPre.DataSource = EtKardex.Where(o => ((cl.Equals(0) && EtClase.Select(s => s.ID).Contains(o.ClaseID)) || o.ClaseID.Equals(cl))).ToList();
                    btnUnselect_Click(null, null);
                    gvDataPre.RefreshData();


                    if (lkSES.EditValue != null)
                        lkSES_EditValueChanged(null, null);
                    else
                    {
                        if (lkES.EditValue != null)
                            lkES_EditValueChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }


        #endregion

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (EtKardex.Count > 0)
                    EtKardex.ForEach(p => p.Selected = true);

                gvDataPre.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            try
            {
                if (EtKardex.Count > 0)
                    EtKardex.ForEach(p => p.Selected = false);

                gvDataPre.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

    }
}