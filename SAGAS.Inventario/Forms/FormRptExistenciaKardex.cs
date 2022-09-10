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

namespace SAGAS.Inventario.Forms
{
    public partial class FormRptExistenciaKardex : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal List<Entidad.Area> EtArea = new List<Entidad.Area>();
        private List<Parametros.ListIdDisplay> EtClase = new List<Parametros.ListIdDisplay>();
        private int Usuario = Parametros.General.UserID;
        internal List<Entidad.Almacen> EtAlmacenes;
        internal int _Combustible;
        private List<Estaciones> EtEstaciones;
        private List<Parametros.ListKardexProducto> EtProducto;
        Reportes.Inventario.Hojas.RptKardexCierre Mrep;
        private BackgroundWorker bgw;

        struct Estaciones
        {
            public int ID;
            public string Nombre;
        }


        #endregion

        #region *** INICIO ***

        public FormRptExistenciaKardex()
        {
            InitializeComponent();  
        }

        private void FormRptExistenciaKardex_Shown(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                EtArea = db.Areas.Where(o => !o.EsServicio).ToList();

                //Clase Combustible
                _Combustible = Parametros.Config.ProductoClaseCombustible();

                EtProducto = (from p in db.Productos
                             join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                             join a in db.Areas on pc.AreaID equals a.ID
                             select new Parametros.ListKardexProducto
                             {
                                 ID = p.ID,
                                 Codigo = p.Codigo,
                                 Nombre = p.Nombre,
                                 AreaID = a.ID,
                                 AreaNombre = a.Nombre,
                                 ClaseID = pc.ID,
                                 ClaseNombre = pc.Nombre                                 
                             }).ToList();

                EtAlmacenes = db.Almacens.Where(a => a.Activo).ToList();

                tgCombustible.IsOn = false;
                tgCombustible_Toggled(null, null);

                //Areas
                var listaArea = EtProducto.Where(o => !o.ClaseID.Equals(_Combustible)).GroupBy(g => new { g.AreaID, g.AreaNombre }).Select(s => new { ID = s.Key.AreaID, Display = s.Key.AreaNombre }).ToList();
                listaArea.Insert(0, new { ID = 0, Display = "TODAS" });
                lkArea.Properties.DataSource = listaArea;

                //Estaciones de Servicio
                EtEstaciones = (from es in db.EstacionServicios
                             where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                             select new Estaciones {ID = es.ID, Nombre = es.Nombre }).ToList();
                EtEstaciones.Insert(0, new Estaciones { ID = 0, Nombre = "TODAS" });

                lkES.Properties.DataSource = EtEstaciones.Select(s => new { s.ID, s.Nombre }).ToList();
                lkES.EditValue = Parametros.General.EstacionServicioID;

                dateFecha.EditValue = Convert.ToDateTime(db.GetDateServer()).Date;

                //lkClase.Properties.DataSource = from c in db.ProductoClases where c.Activo select new { c.ID, c.Nombre };
                
                ////Tipos de Movimiento
                // var tipos = (from mt in db.MovimientoTipos where mt.AplicaKardex select new { mt.ID, mt.Nombre }).OrderBy(o => o.Nombre).ToList();
                //tipos.Insert(0, new { ID = 0, Nombre = "TODOS"});

                //lkTipoMov.Properties.DataSource = tipos;
                //lkTipoMov.EditValue = 0;

                ////Estaciones de Servicio
                //var lista = (from es in db.EstacionServicios
                //            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                //            select new { es.ID, es.Nombre }).ToList();
                //lista.Insert(0, new { ID = 0, Nombre = "TODAS" });

                //lkES.Properties.DataSource = lista;
                //lkES.EditValue = Parametros.General.EstacionServicioID;

                //lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();
                //lkMesFinal.Properties.DataSource = listadoMes.GetListMeses();

                //lkMesInicial.EditValue = DateTime.Now.Month;
                //lkMesInicial_Validated(null, null);
                //lkMesFinal.EditValue = DateTime.Now.Month;
                //lkMesFinal_Validated(null, null);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        public bool ValidarCampos()
        {
            if (dateFecha.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Fecha del Cierre.", Parametros.MsgType.warning);
                return false;
            }

            //if (lkClase.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar la clase del Producto", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (lkProdDesde.EditValue != null || lkProdHasta.EditValue != null)
            //{
            //    if (lkProdDesde.EditValue == null)
            //    {
            //        Parametros.General.DialogMsg("Debe seleccionar el código inicial del rango.", Parametros.MsgType.warning);
            //        return false;
            //    }

            //    if (lkProdHasta.EditValue == null)
            //    {
            //        Parametros.General.DialogMsg("Debe seleccionar el código final del rango.", Parametros.MsgType.warning);
            //        return false;
            //    }

            //    if (Convert.ToInt32(lkProdDesde.Text) > Convert.ToInt32(lkProdHasta.Text))
            //    {
            //        Parametros.General.DialogMsg("El código inicial debe ser menor al código final.", Parametros.MsgType.warning);
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (dtProductos.AsEnumerable().Count(c => ((bool)c["Selected"]).Equals(true)) <= 0)
            //    {
            //        Parametros.General.DialogMsg("Debe seleccionar al menos un Producto de la lista.", Parametros.MsgType.warning);
            //        return false;
            //    }
            //}

            //if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a mostrar.", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            //{
            //    Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (lkTipoMov.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar el tipo de movimiento", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (lkES.EditValue == null)
            //{
            //    Parametros.General.DialogMsg("Debe seleccionar la Estación de Servicio", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (layoutControlItemSes.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
            //{
            //    if (lkSES.EditValue == null)
            //    {
            //        Parametros.General.DialogMsg("Debe seleccionar la Sub Estación de Servicio", Parametros.MsgType.warning);
            //        return false;
            //    }
            //}

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
        
            

        //Estacion
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
                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).Select(s => new { s.ID, s.Nombre })).ToList();

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

        //SubEstación
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

        //Obtener Almacenes
        private void GetAlmacenes(bool Mostrar, bool SubEstacion, int ID)
        {
            if (Mostrar)
            {
                if (!SubEstacion)
                {
                    if (!tgCombustible.IsOn)
                    {
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
                        this.layoutControlItemAlma.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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

        private void btnLoad_Click(object sender, EventArgs e)
        {            
            try
            {
                if (ValidarCampos())
                {
                    bgw = new BackgroundWorker();
                    bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
                    bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
                    bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
                    bgw.WorkerReportsProgress = true;
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
                Reportes.Inventario.Hojas.RptKardexCierre rep = new Reportes.Inventario.Hojas.RptKardexCierre();

                //string Nombre, Direccion, Telefono;
                //System.Drawing.Image picture_LogoEmpresa;
                //Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                
                //rep.PicLogo.Image = picture_LogoEmpresa;
                //rep.CeEmpresa.Text = Nombre;
                rep.xrCorte.Text = "Cortado al " + dateFecha.DateTime.ToShortDateString(); 

                if (lkES.EditValue.Equals(0))
                    MessageBox.Show("xxx");
                else
                {
                    if (lkSES.EditValue == null)
                    {
                        DevExpress.XtraReports.UI.GroupHeaderBand GH = new DevExpress.XtraReports.UI.GroupHeaderBand();
                        DevExpress.XtraReports.UI.XRSubreport xrSubKardex = new DevExpress.XtraReports.UI.XRSubreport();
                     
                        var local = EtEstaciones.Single(s => s.ID.Equals(Convert.ToInt32(lkES.EditValue)));
                        var obj = db.GetInventarioCorte(local.ID, 0, Convert.ToInt32(lkArea.EditValue), Convert.ToInt32(lkClase.EditValue), Convert.ToInt32(lkProducto.EditValue), dateFecha.DateTime.Date);
                        xrSubKardex.ReportSource = new Reportes.Inventario.Hojas.SubRptKardexCierre(obj.Where(o => !o.CostoTotal.Equals(0) && !o.CantidadFinal.Equals(0)).ToList(), local.Nombre);
                        

                        GH.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
                        GH.Controls.Add(xrSubKardex);
                        rep.Bands.Add(GH);
                        bgw.ReportProgress(100);
                    }
                    else
                    {
                        if (lkSES.EditValue.Equals(0))
                        {
                            //Reportes.Inventario.Hojas.RptKardexCierre rep = new Reportes.Inventario.Hojas.RptKardexCierre();
                        }
                        else
                        {
                            rep.CeEstacion.Text = Convert.ToString(lkSES.Text);
                        }
                    }
                }

                rep.RequestParameters = false;
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
                    Mrep = ((Reportes.Inventario.Hojas.RptKardexCierre)(e.Result));
                    Mrep.CreateDocument(true);
                    this.printControlAreaReport.PrintingSystem = Mrep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProgreso.EditValue = e.ProgressPercentage;
        }

        private void tgCombustible_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (tgCombustible.IsOn)
                {
                    lkArea.EditValue = 0;
                    layoutControlGroupFilter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                }
                else
                {
                    lkArea.EditValue = null;
                    lkClase.EditValue = null;
                    layoutControlGroupFilter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;


                    //lkClase.EditValue = null;
                    //layoutControlGroupFilter.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    //lkArea.EditValue = 0;

                    //if (lkSES.EditValue != null)
                    //    lkSES_EditValueChanged(null, null);
                    //else
                    //{
                    //    if (lkES.EditValue != null)
                    //        lkES_EditValueChanged(null, null);
                    //}
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        //Cargar la lista de las clases
        private async void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkArea.EditValue != null)
                {
                    await Task.Delay(10);
                    await GetClases().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        //Clases         
        async Task GetClases()
        {
            try
            {
                EtClase.Clear();
                EtClase = EtProducto.Where(o => !o.ClaseID.Equals(_Combustible) && (Convert.ToInt32(lkArea.EditValue).Equals(0) || o.AreaID.Equals(Convert.ToInt32(lkArea.EditValue)))).GroupBy(g => new { g.ClaseID, g.ClaseNombre }).Select(s => new Parametros.ListIdDisplay { ID = s.Key.ClaseID, Display = s.Key.ClaseNombre }).ToList();
                EtClase.Insert(0, new Parametros.ListIdDisplay { ID = 0, Display = "TODAS" });
                lkClase.Properties.DataSource = EtClase;
                lkClase.EditValue = null;
                lkClase.EditValue = 0;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private async void lkClase_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkClase.EditValue != null)
                {
                    await Task.Delay(10);
                    await GetProductos().ConfigureAwait(false);
                }               
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        //Clases         
        async Task GetProductos()
        {
             try
             {
                 int cl = Convert.ToInt32(lkClase.EditValue);
                 //Cargar Productos
                 lkProducto.Properties.DataSource = null;
                 List<Parametros.ListIdString1String3String3> P = EtProducto.Where(o => ((cl.Equals(0) && EtClase.Select(s => s.ID).Contains(o.ClaseID)) || o.ClaseID.Equals(cl))).Select(s => new Parametros.ListIdString1String3String3 { ID = s.ID, Codigo = s.Codigo, Nombre = s.Nombre, Display = s.Codigo + " | " + s.Nombre}).ToList();
                 P.Insert(0, new Parametros.ListIdString1String3String3 { ID = 0, Codigo = "0", Nombre = "TODOS", Display = "0 | TODOS"});
                 lkProducto.Properties.DataSource = P.ToList();
                 lkProducto.EditValue = 0;


                 if (lkSES.EditValue != null)
                     lkSES_EditValueChanged(null, null);
                 else
                 {
                     if (lkES.EditValue != null)
                         lkES_EditValueChanged(null, null);
                 }
                
            }
             catch (Exception ex)
             {
                 Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
             }
        }

    }
}