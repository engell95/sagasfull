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
using SAGAS.Arqueo.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogCambioLecturas : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        //internal Forms.FormArqueoIsla MDI;
        internal Forms.FormResumenDia RDI;
        internal Entidad.ArqueoIsla EntidadAnterior;
        internal Entidad.ArqueoIsla ArqueoSelected;
        public int IDSUS = 0;
        internal Entidad.ArqueoProducto EtArqueoProducto;
        internal Entidad.ArqueoManguera EtArqueoManguera;
        public Entidad.ResumenDia ResumenSelected;
        private Entidad.VistaArqueoIsla VAI;
        public Entidad.Turno TurnoSelected;
        internal bool Editable = false;
        internal bool Modificando = false;
        private bool ShowMsg = false;
        private bool NextArqueo = false;
        private int UsuarioID;
        private int IDES = Parametros.General.EstacionServicioID;
        private DataTable dtDispensador;
        private DataTable dtMecanica;
        private DataTable dtEfectivo;
        private DataTable dtElectronica;
        //private DataTable dtArqueoProducto;
        //private DataTable dtArqueoManguera;
        private DataTable dtVerificacion;
        private DataTable dtVentas;
        private DataTable dtExtracciones;
        private DataTable dtTotalExtraxion;
        private DataTable dtPago;
        private DataTable dtConcepto;
        private DataTable dtDescuento;
        private DataTable dtPagoEfectivo;
        private List<Entidad.Tanque> Tanques;
        private decimal DiferenciaVerificacionLectura = Parametros.Config.DiferenciaVerificacionLectura();
        private decimal RangoEfectivo = Parametros.Config.RangoEfectivo();
        private decimal RangoMecElectronico = Parametros.Config.RangoMecElectronico();
        private int IDAI = 0;
        private bool MecanicaAmbasCara = false;
        private bool EsDispensadorProblemaLecturas = false;
        private decimal _GrandSobranteDiferencia = 0;

                

        //Tabla que mantiene los campos de los dispensadores a validar
        public DataTable Dispensador
        {
            get
            {
                return dtDispensador;
            }
            set
            {
                dtDispensador = value;
            }
        }

        public DataTable Mecanica
        {
            get
            {
                return dtMecanica;
            }
            set
            {
                dtMecanica = value;
            }
        }

        public DataTable Efectivo
        {
            get
            {
                return dtEfectivo;
            }
            set
            {
                dtEfectivo = value;
            }
        }

        public DataTable Electronica
        {
            get
            {
                return dtElectronica;
            }
            set
            {
                dtElectronica = value;
            }
        }

        #endregion

        #region *** INICIO ***

        /// <summary>
        /// Inicializa la ventana de arqueo de isla
        /// </summary>
        /// <param name="UserID">ID del usuario que esta ingresado en el sistema</param>
        public DialogCambioLecturas(int UserID, Entidad.ResumenDia RD, Entidad.Turno T, Entidad.ArqueoIsla AI)
        {
            InitializeComponent();
            UsuarioID = UserID;
            ArqueoSelected = AI;
            TurnoSelected = ArqueoSelected.Turno;
            ResumenSelected = ArqueoSelected.Turno.ResumenDia;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            if (FillControl())
            {
                this.panelControlPrincipal.Location = new Point(2, 10);
            }
            else
                this.BeginInvoke(new MethodInvoker(this.Close));
        }

        #endregion

        #region *** METODOS ***


        /// <summary>
        /// Metodo para cargar los datos iniciales de esta ventana (Arqueo de Isla)
        /// </summary>
        private bool FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                VAI = dbView.VistaArqueoIslas.Single(v => v.ArqueoIslaID.Equals(ArqueoSelected.ID));

                if (!ResumenSelected.SubEstacionID.Equals(0))
                    lkSES.Text = VAI.SubEstacionNombre;

                txtTecnico.Text = VAI.TecnicoNombre;
                txtEstacionServicio.Text = VAI.EstacionNombre;
                txtTurno.Text = VAI.TurnoNumero.ToString();
                dateFecha.EditValue = ResumenSelected.FechaInicial;
                txtNumero.Text = VAI.ArqueoNumero.ToString();
                txtIsla.Text = VAI.IslaNombre;
                txtArqueador.Text = VAI.ArquedaorNombre;
                chkArqueoEspecial.Checked = VAI.ArqueoEspecial;
                bdsArqueoIsla.DataSource = ArqueoSelected;
     
                layoutControlGridLecturas.Visible = true;
                if (FillLecturas())
                {
                    if (Efectivo.Rows.Count > 0 || Electronica.Rows.Count > 0)
                    {
                        if (Efectivo.Rows.Count <= 0)
                        {
                            this.layoutControlGridLecturas.Height -= 125;
                            this.layoutControlGroupElectronica.Height = 100;
                            this.panelControlPrincipal.Height -= 170;
                        }

                        if (Electronica.Rows.Count <= 0)
                        {
                            this.layoutControlGridLecturas.Height -= 125;
                            this.layoutControlGroupEfectivo.Height = 100;
                            this.panelControlPrincipal.Height -= 170;
                        }

                        this.layoutControlGroupMecanica.Height = 128;

                        if (!EsDispensadorProblemaLecturas)
                        {
                            this.layoutControlVerificacion.Visible = true;
                            this.layoutControlVerificacion.BringToFront();
                        }
                    }
                    else
                    {
                        this.layoutControlGridLecturas.Height -= 250;
                        this.panelControlPrincipal.Height -= 360;
                    }
                }
                else
                    return false;

                this.btnOK.Enabled = false;
                this.btnOKNext.Enabled = false;

                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }
          
        #region <<< Validaciones >>>
   
        public bool ValidarArqueo()
        {
            try
            {
                for (int i = 2; i < dgvVerif.Columns.Count; i++)
                {
                    if (Efectivo.Rows.Count > 0 && Mecanica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                    {
                        decimal valor = Convert.ToDecimal(dgvVerif.GetRowCellValue(0, dgvVerif.Columns[i]));
                        if (valor > RangoEfectivo || valor < -(RangoEfectivo))
                        {
                            Parametros.General.DialogMsg("La verificación en Efectivo excede el rango permitido: " + RangoEfectivo.ToString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }

                    if (Mecanica.Rows.Count > 0 && Electronica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                    {
                        decimal valor = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i]));
                        if (valor > RangoMecElectronico || valor < -(RangoMecElectronico))
                        {
                            Parametros.General.DialogMsg("La verificación Mecánica vs Electrónica excede el rango permitido: " + RangoMecElectronico.ToString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }

                    return true;
            }
            catch { return false; }
        }
        
        #endregion


        /// <summary>
        /// Estilo de color de las celdas para verificación
        /// </summary>
        /// <param name="BCol">BandColumn aplicable para el style</param>
        /// <returns></returns>
        private DevExpress.XtraGrid.StyleFormatCondition styleFormatConditioner(Parametros.MyBandColumn BCol)
        {
             try
             {
                 DevExpress.XtraGrid.StyleFormatCondition style = new DevExpress.XtraGrid.StyleFormatCondition();

                 style.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
                 style.Appearance.Options.UseBackColor = true;
                 style.ApplyToRow = false;
                 style.Column = BCol;
                 style.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                 style.Expression = "[" + BCol.FieldName + "] < " + Convert.ToString(Decimal.Negate(DiferenciaVerificacionLectura)) + " or [" + BCol.FieldName + "] > " + Convert.ToString(DiferenciaVerificacionLectura);

                 return style;
             }
            catch
             {return null;}
        }
         
        /// <summary>
        /// Cargar las lecturas de los Grid
        /// </summary>
        /// 
        
        private bool FillLecturas()
        {
            try
            {
            
            //Carga los dispensadores de la isla
            var objDis = db.Dispensadors.Where(d => d.IslaID.Equals(ArqueoSelected.IslaID) && d.Activo);

            if (objDis.Count() > 0)
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);

                #region <<<DatosInicialesArqueo >>>

                //Verificar si el dispensador tiene la suma de las lecturas mecanicas en una sola cara
                MecanicaAmbasCara = objDis.Count(d => d.MecanicaAmbasCara) > 0 ? true : false;
                //Verificar si el dispensador tiene problemas de lecturas
                EsDispensadorProblemaLecturas = objDis.Count(d => d.EsDispensadorProblemaLecturas) > 0 ? true : false;

                //Cargo los productos para la tabla ArqueoProducto
                var product = (from p in db.Productos
                               join t in db.Tanques on p.ID equals t.ProductoID
                               join m in db.Mangueras on t.ID equals m.TanqueID
                               join d in db.Dispensadors on m.DispensadorID equals d.ID
                               where d.Activo && m.Activo && (db.Dispensadors.Any(di => di.IslaID.Equals(ArqueoSelected.IslaID) && di.ID.Equals(d.ID)))
                               select new { IDProducto = p.ID, Producto = p.Nombre, m.PrecioDiferenciado }).Distinct();


                //Creación de las tablas del Arqueo
                dtDispensador = Parametros.General.LINQToDataTable(objDis);
                dtMecanica = new DataTable();
                dtEfectivo = new DataTable();
                dtElectronica = new DataTable();
                //dtConcepto = new DataTable();

                //dtMecanica.Columns.Add("", typeof(Int32));
                dtMecanica.Columns.Add("Lectura", typeof(String));
                //dtEfectivo.Columns.Add("IDP", typeof(Int32));
                dtEfectivo.Columns.Add("Lectura", typeof(String));
                //dtElectronica.Columns.Add("IDP", typeof(Int32));
                dtElectronica.Columns.Add("Lectura", typeof(String));

                
                //Lista de mangueras en la isla seleccionado
                var manguras = from m in db.Mangueras
                               join d in db.Dispensadors on m.DispensadorID equals d.ID
                               where m.Activo && (objDis.Any(o => o.ID == d.ID))
                               select m;

                Tanques = db.Tanques.Where(t => t.Activo && (manguras.Any(m => m.TanqueID == t.ID))).ToList();

                  int bandasDelete = dgvVerif.Bands.Count;
                    for (int i = 1; i < bandasDelete; i++)
                    {
                        dgvVerif.Bands.RemoveAt(1);
                    }
                //if (!NextArqueo)
                //{
                    //Agregar bandas al grid verificacion de acuerdo a los lados del dispensador
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {
                        var bandVerif = new Parametros.MyBand("bandVerif" + lado.Key.ToString(), "Verificación Lado " + lado.Key.ToString(), Convert.ToInt32(lado.Count() * 120), dgvVerif.Bands.Count + 1);

                        dgvVerif.Bands.Add(bandVerif);
                    }
                //}
                //Numero de bandas en el grid de verificacion
                int bandas = dgvVerif.Bands.Count;

                dtVerificacion = new DataTable();
                dtVerificacion.Columns.Add("TipoVerificacion", typeof(String));

                #endregion

             
                //Recorremos cada manguera de la isla
                foreach (var rmang in manguras)
                {                    
                    //Se agrega una columna por manguera a la tabla dtVerificacion
                    dtVerificacion.Columns.Add(rmang.ID.ToString(), typeof(Decimal));
                    //if (!NextArqueo)
                    //{
                    //    //Producto de la manguera
                        var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(rmang.TanqueID))).ProductoID));


                        //Validar donde agregar la manguera si el lado A o lado B
                        if (rmang.Lado.ToString() == "A")
                        {
                            //var col = new Parametros.MyBandColumn(rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, false);
                            var col = new Parametros.MyBandColumn(prod.Nombre + " (" + rmang.Numero + ")", rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == rmang.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                            dgvVerif.FormatConditions.Add(styleFormatConditioner(col));
                            this.dgvVerif.Bands[1].Columns.Add(col);

                        }
                        else if (rmang.Lado.ToString() == "B")
                        {
                            if (bandas > 1)
                            {
                                //var col = new Parametros.MyBandColumn(rmang.ID.ToString(), dgvVerif.Bands[2].Columns.Count + 1, 120, false);
                                var col = new Parametros.MyBandColumn(prod.Nombre + " (" + rmang.Numero + ")", rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == rmang.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                dgvVerif.FormatConditions.Add(styleFormatConditioner(col));

                                if (this.dgvVerif.Bands.Count > 2)
                                this.dgvVerif.Bands[2].Columns.Add(col);
                                else
                                    this.dgvVerif.Bands[1].Columns.Add(col);
                            }
                        }
                    //}
                }

                //Se carga el enum de los datos de la Verificacion
                foreach (Parametros.TiposVerificacion tipo in Enum.GetValues(typeof(Parametros.TiposVerificacion)))
                {
                    //Creamos una nueva fila en el dataTable dtVerificacion
                    DataRow dr = dtVerificacion.NewRow();
                    dr["TipoVerificacion"] = Parametros.General.GetEnumTiposVerificacion(tipo);

                    //foreach (var list in manguras)
                    //{
                    //    dr[list.ID.ToString()] = 0;
                    //}

                    foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                    {
                        foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                        { dr[am.MangueraID.ToString()] = 0; }
                    }

                    dtVerificacion.Rows.Add(dr);

                }

                gridVerif.DataSource = dtVerificacion;
                dgvVerif.RefreshData();
            
                
                VerificacionLecturaEfectivo();
                VerificacionMecVsElectronica();

                ///**************************************************************************************************************************///  
        

        
                #region <<< LecturaMecanica >>>

                //verificar si en la lista objDis esta activada la lectura mecanica
                if (objDis.Count(o => o.LecturaMecanica) > 0)
                {
                    //Mostrar el grid Mecanica
                    this.layoutControlGroupMecanica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    int bandasDeleteMec = bgvDataMecanica.Bands.Count;
                    for (int i = 1; i < bandasDeleteMec; i++)
                    {
                        bgvDataMecanica.Bands.RemoveAt(1);
                    }
                    
                    //Variable para contar los puntos de llenado
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {

                        //crear banda para los lados del grid de lectura mecanica
                        var bandLado = new Parametros.MyBand("gBandMec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataMecanica.Bands.Count + 1);

                        //Diseño de las bandas del gridMecanica
                        this.bgvDataMecanica.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);

                            //Se rocorren las mangueras de cada Dispensador
                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura mecanica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtMecanica.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));

                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataMecanica.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();

                                //agrego la columna al BandeGridView bgvDataMecanica
                                //this.bgvDataMecanica.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridMecanica.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);
                                //bandLado.Columns.Add(colProd);                           

                            }
                        }
                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtMecanica.NewRow();
                        dr["Lectura"] = val.ToString();
                        
                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura

                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID);
                                
                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaMecanicaInicial;
                                    //obj.LecturaMecanica;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaMecanicaFinal;
                                    //0.00m;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtMecanica.Rows[1][nombre]) - Convert.ToDecimal(dtMecanica.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtMecanica.Rows.Add(dr);

                    }

                    gridMecanica.DataSource = Mecanica;
                }

                #endregion

                ///**************************************************************************************************************************///

        
                #region <<< LecturaEfectivo >>>

                //verificar si en la lista objDis esta activada la lectura Efectivo
                if (objDis.Count(o => o.LecturaEfectivo) > 0)
                {
                    //Mostrar el grid Efectivo
                    this.layoutControlGroupEfectivo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    int bandasDeleteEfec = bgvDataEfectivo.Bands.Count;
                    for (int i = 1; i < bandasDeleteEfec; i++)
                    {
                        bgvDataEfectivo.Bands.RemoveAt(1);
                    }

                    //Variable para contar los puntos de llenado
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {

                        //crear banda para los lados del grid de lectura efectivo
                        var bandLado = new Parametros.MyBand("gBandEfec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataEfectivo.Bands.Count + 1);

                        //Diseño de las bandas del gridEfectivo
                        this.bgvDataEfectivo.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);


                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura mecanica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtEfectivo.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));


                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataEfectivo.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();
                                //agrego la columna al BandeGridView bgvDataMecanica
                                //this.bgvDataEfectivo.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridEfectivo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);

                            }
                        }

                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtEfectivo.NewRow();
                        dr["Lectura"] = val.ToString();


                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura
                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID);

                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaEfectivoInicial;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaEfectivoFinal;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtEfectivo.Rows[1][nombre]) - Convert.ToDecimal(dtEfectivo.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtEfectivo.Rows.Add(dr);
                    }

                    gridEfectivo.DataSource = Efectivo;
                }

                #endregion

                ///*************************************************************************************************************************///

       
                #region <<< LecturaElectronica >>>

                //Cargar Gird de Lecuras en ELECTRONICA
                if (objDis.Count(o => o.LecturaElectronica) > 0)
                {
                    //Mostrar el grid Efectivo
                    this.layoutControlGroupElectronica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    int bandasDeleteElec = bgvDataElectronico.Bands.Count;
                    for (int i = 1; i < bandasDeleteElec; i++)
                    {
                        bgvDataElectronico.Bands.RemoveAt(1);
                    }

                    //lado.key devuelve el valor del lado de la manguera
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {
                        //crear banda para los lados del grid de lectura Electronica
                        var bandLado = new Parametros.MyBand("gBandElec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataElectronico.Bands.Count + 1);

                        //Diseño de las bandas del gridElectronica
                        this.bgvDataElectronico.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);

                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura Electronica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtElectronica.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));

                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataElectronico.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();
                                //agrego la columna al BandeGridView bgvDataMecanica
                                this.bgvDataElectronico.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridElectronica.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);

                            }
                        }

                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtElectronica.NewRow();
                        dr["Lectura"] = val.ToString();


                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura
                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID); ;

                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaElectronicaInicial;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaElectronicaFinal;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtElectronica.Rows[1][nombre]) - Convert.ToDecimal(dtElectronica.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtElectronica.Rows.Add(dr);

                    }

                    gridElectronica.DataSource = Electronica;

                }

                #endregion

                ///************************************************************************************************************************///

                VerificacionLecturaEfectivo();
                VerificacionMecVsElectronica();               
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                return true;
            }
            else
            {                     
                Parametros.General.DialogMsg("La isla no tiene asignado ningún dispensador.", Parametros.MsgType.warning);
                return false;
            }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }
        
        #region <<< VerificacionLecturas/TotalExtraxion/Formula >>>

        private void VerificacionLecturaEfectivo()
        {
            try
            {
                if (Efectivo.Rows.Count > 0 && Mecanica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                {
                    for (int i = 2; i < dgvVerif.Columns.Count; i++)
                    {
                        decimal PrimerValor = Decimal.Round(Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, bgvDataEfectivo.Columns[i])), 3, MidpointRounding.AwayFromZero);
                        decimal SegundoValor = 0;

                        if (Electronica.Rows.Count > 0)
                            SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])), 3, MidpointRounding.AwayFromZero);
                        else
                            SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        decimal total = 0m;

                        var Manguera = Electronica.Rows.Count > 0 ? ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag)) : ((Entidad.Manguera)((Entidad.Manguera)bgvDataMecanica.Columns[i].Tag));

                        string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString();
                        //string Producto = bgvDataElectronico.Columns[i].FieldName.Substring(0, 1);

                        //EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(Producto) && );
                        if (Manguera.PrecioDiferenciado)
                            Producto += "Dif";

                        //var Prod = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag));
                        decimal Precio = Convert.ToDecimal(ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(Convert.ToInt32(Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID)) && ap.EsDiferenciado.Equals(Manguera.PrecioDiferenciado)).First().Precio);
                            //Convert.ToDecimal(dgvVentas.GetRowCellValue(1, dgvVentas.Columns[Producto]));

                        decimal Descuento = 0;

                        try { Descuento = Convert.ToDecimal(ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(Convert.ToInt32(Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID)) && ap.EsDiferenciado.Equals(Manguera.PrecioDiferenciado)).First().ArqueoMangueras.Where(am => am.MangueraID.Equals(Manguera.ID)).First().Descuento); }
                        catch { Descuento = 0; }

                        // = Convert.ToDecimal(dgvVentas.GetRowCellValue(2, dgvVentas.Columns[Producto]));

                        if (!Descuento.Equals(0m))
                            total = Decimal.Round(PrimerValor - (SegundoValor * (Precio - Descuento)), 3, MidpointRounding.AwayFromZero);
                        else
                            total = Decimal.Round(PrimerValor - (SegundoValor * (Precio)), 3, MidpointRounding.AwayFromZero);

                        dgvVerif.SetRowCellValue(0, dgvVerif.Columns[Manguera.ID.ToString()], total);
                    }

                    dgvVerif.RefreshData();

                    if (btnOK.Enabled.Equals(false))
                        btnOK.Enabled = true;

                    if (btnOKNext.Enabled.Equals(false))
                        btnOKNext.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void VerificacionMecVsElectronica()
        {
            try
            {
                if (Mecanica.Rows.Count > 0 && Electronica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                {
                    for (int i = 2; i < dgvVerif.Columns.Count; i++)
                    {
                        decimal PrimerValor = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        decimal SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag));

                        decimal total = Decimal.Round(PrimerValor - SegundoValor, 3, MidpointRounding.AwayFromZero);

                        dgvVerif.SetRowCellValue(1, dgvVerif.Columns[Manguera.ID.ToString()], total);
                    }
                    dgvVerif.RefreshData();

                    if (MecanicaAmbasCara)
                    {
                        int col = Convert.ToInt32((dgvVerif.Columns.Count - 2) / 2m);

                        for (int i = 2; i < dgvVerif.Columns.Count; i++)
                        {
                            if (i < (col + 2))
                            {
                                decimal x = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i]));
                                decimal y = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i + col]));

                                dgvVerif.SetRowCellValue(1, dgvVerif.Columns[i], x + y);
                            }
                            else
                                dgvVerif.SetRowCellValue(1, dgvVerif.Columns[i], 0);
                        }

                        dgvVerif.RefreshData();
                    }

                    //List<Int32> ID = new List<Int32>();

                    //for (int i = 2; i < dgvDataTotalExtraxion.Columns.Count; i++)
                    //{
                    //    string prod = dgvDataTotalExtraxion.Columns[i].FieldName;
                    //    decimal total = 0;

                    //    for (int j = 2; j < bgvDataElectronico.Columns.Count; j++)
                    //    {
                    //        var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[j].Tag));
                    //        string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString();

                    //        if (Manguera.PrecioDiferenciado)
                    //            Producto += "Dif";

                    //        if (prod.Equals(Producto))
                    //        {
                    //            decimal venta = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[j])), 3, MidpointRounding.AwayFromZero);
                    //            if (Manguera.EsLecturaGalones)
                    //                total += (venta * Parametros.General.LitrosGalones);
                    //            else
                    //                total += venta;
                    //        }
                    //    }

                    //    //Obtengo el ID del producto de la tabla Total Extraxion
                    //    int IDProd = ((Int32)(dgvDataTotalExtraxion.Columns[i].Tag));

                    //    //Verifico si el ID esta en la lista de ID's, si esta se resetea a 0 el valor del descuento automatico
                    //    if (!ID.Contains(IDProd))
                    //    {
                    //        bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[IDProd.ToString()], 0m);

                    //        var AP = ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(IDProd));

                    //        foreach (var objAP in AP)
                    //        {
                    //            foreach (var objAM in objAP.ArqueoMangueras.Where(a => a.ArqueoProductoID.Equals(objAP.ID)))
                    //            {
                    //                decimal Desc = 0;
                    //                Desc += Convert.ToDecimal(bgvDataDescuento.GetRowCellValue(0, bgvDataDescuento.Columns[IDProd.ToString()]));
                    //                Desc += Decimal.Round((objAM.LecturaElectronicaFinal - objAM.LecturaElectronicaInicial) * Convert.ToDecimal(objAM.Descuento), 2, MidpointRounding.AwayFromZero);
                    //                bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[IDProd.ToString()], Desc);
                    //                bgvDataDescuento.RefreshData();

                    //            }
                    //        }
                    //        ID.Add(IDProd);
                    //    }

                    //    dgvDataTotalExtraxion.SetRowCellValue(0, dgvDataTotalExtraxion.Columns[i], total.ToString("#,0.000"));


                    //}
                    //decimal suma = 0.00m;

                    //for (int i = 2; i < bgvDataDescuento.Columns.Count; i++)
                    //{
                    //    decimal calc = 0m;
                    //    suma += Decimal.TryParse(bgvDataDescuento.Columns[i].SummaryText, out calc) ? calc : 0;
                    //    txtDescuentoTotal.Text = Decimal.Round(suma, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    //}

                    //dgvDataTotalExtraxion.RefreshData();
                    //TotalExtraxionVenta();
                    
                }

                if (btnOK.Enabled.Equals(false))
                    btnOK.Enabled = true;

                if (btnOKNext.Enabled.Equals(false))
                    btnOKNext.Enabled = true;
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
             
        #endregion
        
        //****************//
        // GUARDAR ARQUEO //
        //****************//
        // GUARDAR ARQUEO //
        //****************// 

        
        private bool Guardar()
        {
            
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                string info = "";
                
                try
                {                       
                    if (ArqueoSelected != null)
                    {
                        #region <<< Resumen, Turno, Efectivo >>>
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                        Entidad.ArqueoIsla AI = db.ArqueoIslas.Single(ai => ai.ID.Equals(ArqueoSelected.ID)); ;
                        Entidad.Turno T = AI.Turno;
                        Entidad.ResumenDia RD = T.ResumenDia;//db.ResumenDias.Single(r => r.Equals(ResumenSelected.ID));

                        if (RD.Aprobado)
                        {
                            if (!RD.Desaprobado)
                            {
                                RD.Aprobado = false;
                                RD.Desaprobado = true;
                                db.SubmitChanges();
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se desaprobó el Resumen del día Nro: "
                                + RD.Numero, this.Name); 
                            }
                            else
                            {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("Las lecturas no se pueden cambiar, el Resumen del Día ya fue desaprobado anteriormente y no esta permitido que se desapruebe de nuevo.", Parametros.MsgType.warning);
                            return false;
                            }
                                
                        }

                        T.Cerrada = false;
                        T.Estado = 1;

                        if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(T.ID)) > 0)
                        {
                            Entidad.ArqueoEfectivo AE = db.ArqueoEfectivos.Where(ae => ae.TurnoID.Equals(T.ID)).First();
                            AE.Cerrado = false;
                            AE.Estado = 1;
                        }

                        AI.Cerrado = false;
                        AI.Estado = 1;

                        #endregion
                        
                        #region <<< ArqueosProductos/Lecturas >>>
                        foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                            {

                                info = "Favor revisar los datos (Lecturas de las Mangueras)";
                                if (arm.ID > 0)
                                {
                                    Entidad.ArqueoManguera AM = db.ArqueoMangueras.Single(a => a.ID == arm.ID);

                                    if (Editable)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(arm, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(AM, 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se editó las lecturas del Arqueo de la Isla Número: " + ArqueoSelected.Numero + ", Manguera: " + db.Mangueras.Single(m => m.ID == arm.MangueraID).Numero, this.Name, dtPosterior, dtAnterior);
                                    }
                                    AM.LecturaMecanicaInicial = arm.LecturaMecanicaInicial;
                                    AM.LecturaMecanicaFinal = arm.LecturaMecanicaFinal;
                                    AM.LecturaEfectivoInicial = arm.LecturaEfectivoInicial;
                                    AM.LecturaEfectivoFinal = arm.LecturaEfectivoFinal;
                                    AM.LecturaElectronicaInicial = arm.LecturaElectronicaInicial;
                                    AM.LecturaElectronicaFinal = arm.LecturaElectronicaFinal;
                                    AM.Descuento = arm.Descuento;

                                    db.SubmitChanges();
                                }
                            }
                        }
                           #endregion

                        

                        db.SubmitChanges();
                        trans.Commit();
                        EntidadAnterior = AI;
                        ShowMsg = true;
                        btnOK.Enabled = false;
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();                        
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                   
                        return true;
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe de cargar los datos del encabezado para el Arqueo", Parametros.MsgType.warning);
                        return false;
                    }
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR + Environment.NewLine + info, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                    return false;
                }

                finally
                {
                     if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
            
        }
        #endregion

        #region *** EVENTOS ***

        #region <<< EventosGenerales >>>

        private void btnOK_Click_1(object sender, EventArgs e)
        {

            foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
            {
                foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                {
                    if ((arm.LecturaMecanicaFinal - arm.LecturaMecanicaInicial) < 0)
                    {
                        Parametros.General.DialogMsg("La extracción en las lecturas Mecánicas no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                        return;
                    }

                    if ((arm.LecturaEfectivoFinal - arm.LecturaEfectivoInicial) < 0)
                    {
                        Parametros.General.DialogMsg("La extracción en las lecturas Efectivo no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                        return;
                    }

                    if ((arm.LecturaElectronicaFinal - arm.LecturaElectronicaInicial) < 0)
                    {
                        Parametros.General.DialogMsg("La extracción en las lecturas Electrónica no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                        return;
                    }
                    
                }
            }

            if (Parametros.General.DialogMsg("¿Desea Cambiar las lecturas de este arqueo?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                if (!Guardar())
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                this.Close();
            }
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (RDI != null)
                RDI.CleanDialog(ShowMsg);
        }
         
        private void DialogArqueoIsla_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnOK.Enabled.Equals(true))
            {
                DialogResult resultado;

                resultado = Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGMODIFICADOCLOSING + Environment.NewLine, Parametros.MsgType.question);

                if (resultado == DialogResult.OK)
                    e.Cancel = true;
            }
        }

        private void btnOKNext_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
                {
                    foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                    {
                        if ((arm.LecturaMecanicaFinal - arm.LecturaMecanicaInicial) < 0)
                        {
                            Parametros.General.DialogMsg("La extracción en las lecturas Mecánicas no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                            return;
                        }

                        if ((arm.LecturaEfectivoFinal - arm.LecturaEfectivoInicial) < 0)
                        {
                            Parametros.General.DialogMsg("La extracción en las lecturas Efectivo no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                            return;
                        }

                        if ((arm.LecturaElectronicaFinal - arm.LecturaElectronicaInicial) < 0)
                        {
                            Parametros.General.DialogMsg("La extracción en las lecturas Electrónica no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                            return;
                        }

                    }
                }

                if (Parametros.General.DialogMsg("¿Desea Cambiar las lecturas de este arqueo?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    if (!Guardar())
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    this.Cursor = Cursors.Default;
                    ArqueoSelected = db.ArqueoIslas.Where(a => a.IslaID.Equals(EntidadAnterior.IslaID) && a.ID > EntidadAnterior.ID).OrderBy(o => o.ID).First();
                    TurnoSelected = ArqueoSelected.Turno;
                    ResumenSelected = ArqueoSelected.Turno.ResumenDia;
                    NextArqueo = true;

                    FillControl();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
    

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        #endregion

        #region <<< EventosLecturaMecanica >>>

        private void bgvDataMecanica_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la ultima fila

            if (bgvDataMecanica.FocusedRowHandle == 2)
                e.Cancel = true;
        }           

        private void bgvDataMecanica_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataMecanica.GetFocusedRowCellValue(e.Column))))
                    bgvDataMecanica.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    /////////////////////////////////////////////////////////////////////MessageBox.Show(e.Column.FieldName);

                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(0, e.Column)), 3);
                    extracion = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(1, e.Column)), 3);

                    final = extracion - inicial;

                    if (final < 0 && !extracion.Equals(0))
                    {

                        Parametros.General.DialogMsg("Extracción no puede ser mayor al inicial", Parametros.MsgType.warning);

                        bgvDataMecanica.SetRowCellValue(1, e.Column, 0);

                        return;
                    }

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado);
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaMecanicaInicial = inicial;
                    EtArqueoManguera.LecturaMecanicaFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataMecanica.SetRowCellValue(2, e.Column, final);
                    VerificacionMecVsElectronica();
                                                                       

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
          
        private void bgvDataMecanica_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataMecanica.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataMecanica.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataMecanica.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataMecanica_LostFocus(object sender, EventArgs e)
        {
            //Recorrer las columnas de la lectura Mecanica
            for (int i = 2; i < bgvDataMecanica.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])) < 0)
                {   
                    bgvDataMecanica.SetColumnError(bgvDataMecanica.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);
                    break;
                }
            }
        }

        #endregion 

        #region  <<< EventosLecturaEfectivo >>>

        private void bgvDataEfectivo_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la ultima fila
            

            if (bgvDataEfectivo.FocusedRowHandle == 2)
                e.Cancel = true;
                   
        }

        private void bgvDataEfectivo_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataEfectivo.GetFocusedRowCellValue(e.Column))))
                    bgvDataEfectivo.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(0, e.Column));
                    extracion = Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(1, e.Column));

                    final = extracion - inicial;

                    if (final < 0 && !extracion.Equals(0))
                    {

                        Parametros.General.DialogMsg("Extracción no puede ser mayor al inicial", Parametros.MsgType.warning);

                        bgvDataMecanica.SetRowCellValue(1, e.Column, 0);

                        return;
                    }

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado);
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaEfectivoInicial = inicial;
                    EtArqueoManguera.LecturaEfectivoFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataEfectivo.SetRowCellValue(2, e.Column, final);

                    //Recorrer las columnas del Grid de Verificación y validar el total
                    VerificacionLecturaEfectivo(); 

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void bgvDataEfectivo_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataEfectivo.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataEfectivo.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataEfectivo.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataEfectivo_LostFocus(object sender, EventArgs e)
        {

            //Recorrer las columnas de la lectura Efectivo
            for (int i = 2; i < bgvDataEfectivo.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, bgvDataEfectivo.Columns[i])) < 0)
                {
                    bgvDataEfectivo.SetColumnError(bgvDataEfectivo.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);

                    break;
                }

            }
        }

        #endregion

        #region <<< EventosLecturaElectronica >>>

        private void bgvDataElectrico_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la primera y ultima fila
                       if (bgvDataElectronico.FocusedRowHandle == 2)
                e.Cancel = true;
        }

        private void bgvDataElectrico_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataElectronico.GetFocusedRowCellValue(e.Column))))
                    bgvDataElectronico.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(0, e.Column)), 3, MidpointRounding.AwayFromZero);
                    extracion = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(1, e.Column)), 3, MidpointRounding.AwayFromZero);

                    final = extracion - inicial;

                    if (final < 0 && !extracion.Equals(0))
                    {

                        Parametros.General.DialogMsg("Extracción no puede ser mayor al inicial", Parametros.MsgType.warning);

                        bgvDataMecanica.SetRowCellValue(1, e.Column, 0);

                        return;
                    }

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado); 
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaElectronicaInicial = inicial;
                    EtArqueoManguera.LecturaElectronicaFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataElectronico.SetRowCellValue(2, e.Column, final);
                    VerificacionLecturaEfectivo();
                    VerificacionMecVsElectronica();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void bgvDataElectrico_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataElectronico.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataElectronico.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataElectronico.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataElectrico_LostFocus(object sender, EventArgs e)
        {
            //Recorrer las columnas de la lectura Efectivo
            for (int i = 2; i < bgvDataEfectivo.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])) < 0)
                {
                    bgvDataElectronico.SetColumnError(bgvDataElectronico.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);
                    break;
                }
            }
        }

        #endregion

       
        #endregion        

    }
}