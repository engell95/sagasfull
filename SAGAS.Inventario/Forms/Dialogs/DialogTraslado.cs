using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.Inventario.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogTraslado : Form
    { 
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTrasladoExterno MDI;
        //private List<ObjectListTipoMovimiento> ListTipoMovimiento = new List<ObjectListTipoMovimiento>();
        //private List<ObjectListProveedor> ListProveedor = new List<ObjectListProveedor>();
        //private List<ObjectListCliente> ListCliente = new List<ObjectListCliente>();
        //private List<ObjectListEstacion> ListEstacion = new List<ObjectListEstacion>();
        //private List<ObjectListSubEstacion> ListSubEstacion = new List<ObjectListSubEstacion>();
        internal Entidad.Movimiento EntidadAnterior;
        internal Entidad.Movimiento EntidadEntrada;
        internal bool OnlyView = false;
        private bool _Entrada = false;
        internal bool ShowMsg;
        internal int _ClaseCombiustible;
        private int UsuarioID;        
        private bool NextBien = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private Parametros.TiposMovimientoInventario _Tipo;
        private bool _Guardado = false;
        private bool _ToPrint = false;
        private int IDPrint = 0;
        public bool EsCombustible = false;
        private decimal _TipoCambio;
        private List<int> Lkardex;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private IQueryable<Parametros.ListIdCodeDisplay> listaAlmacen;
        private IQueryable<Parametros.ListIdCodeDisplay> listaAlmacenEntrada;
        internal decimal vTotalPagar = 0m;
        internal int _MonedaPrimaria;
        internal int _MonedaSecundaria;

        internal List<int> AreasCerradas = new List<int>();
        internal List<ListaAreas> EtAreas = new List<ListaAreas>();

        public struct ListaAreas
        {
            public int ID;
            public string Display;
            public int AreaID;
            public string Nombre;
        }

        private List<ListaEstacionesServicio> les;

        struct ListadosAcitvos
        {
            public int ID;
            public string Nombre;
            public int IDTipo;
        }


        private List<Entidad.Kardex> OC = new List<Entidad.Kardex>();
        public List<Entidad.Kardex> DetalleOC
        {
            get { return OC; }
            set
            {
                OC = value;
                this.bdsDetalle.DataSource = this.OC;
            }
        }

        private int AreaID
        {
            get { return Convert.ToInt32(glkArea.EditValue); }
            set { glkArea.EditValue = value; }
        }

        private DateTime FechaTraslado
        {
            get { return Convert.ToDateTime(dateFechaTraslado.EditValue); }
            set { dateFechaTraslado.EditValue = value; }
        }

        private string NoReferencia
        {
            get { return txtNoFactura.Text; }
            set { txtNoFactura.Text = value; }
        
        }
        
        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }

        struct ListaEstacionesServicio
        {
            public int ID;
            public string Nombre;
        }

        struct ListaSubEstacionesServicio
        {
            public int ID;
            public string Nombre;
            public int EstacionID;
        }


        #endregion

 
        public DialogTraslado(int UserID, bool Entrada)
        {
            InitializeComponent();
            UsuarioID = UserID;
            _Entrada = Entrada;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
            
            //glkTipoMovimiento.Properties.DisplayMember = "Nombre";
            //glkTipoMovimiento.Properties.ValueMember = "ID";

            //glkProvClientEstacion.Properties.DisplayMember = "Display";
            //glkProvClientEstacion.Properties.ValueMember = "ID";

            //lkMoneda.Properties.DisplayMember = "Display";
            //lkMoneda.Properties.ValueMember = "ID";

            //gridCuenta.DisplayMember = "Codigo";
            //gridCuenta.ValueMember = "ID";            
        }

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //TIPO DE MOVIMIENTO

                _ClaseCombiustible = Parametros.Config.ProductoClaseCombustible();
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _MonedaSecundaria = Parametros.Config.MonedaSecundaria();

                //COMBOS
                //Tipo de Area
                EtAreas = (from a in db.AreaVentas
                           join ar in db.Areas on a.ID equals ar.AreaVentaID
                           join c in db.ProductoClases on ar.ID equals c.AreaID
                           where a.Activo && ar.Activo
                           group ar by new { a.ID, Display = a.Nombre, AreaID = ar.ID, ar.Nombre } into gr
                           select new ListaAreas { ID = gr.Key.ID, Display = gr.Key.Display, AreaID = gr.Key.AreaID, Nombre = gr.Key.Nombre }).ToList();

                glkArea.EditValue = null;
                glkArea.Properties.DataSource = EtAreas.GroupBy(g => new { g.ID, g.Display }).Select(s => new { ID = s.Key.ID, Display = s.Key.Display }).ToList();
               
                les = (from es in db.EstacionServicios
                      where es.Activo && (es.ID == IDEstacionServicio)
                      select new ListaEstacionesServicio { ID = es.ID, Nombre = es.Nombre}).ToList();
                
                glkESOut.Properties.DataSource = db.EstacionServicios.Where(o => o.Activo && o.ID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                glkESOut.EditValue = IDEstacionServicio;
                //glkSESOut.Properties.DataSource = db.SubEstacions.Where(o => o.Activo).Select(s => new { s.ID, s.Nombre }).ToList();

                glkESIn.Properties.DataSource = db.EstacionServicios.Where(o => o.Activo).Select(s => new { s.ID, s.Nombre }).ToList();

                Lkardex = db.Kardexes.Where(k => k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion)).Select(s => s.ProductoID).GroupBy(g => g).Select(k => k.Key).ToList();

                this.bdsDetalle.DataSource = DetalleOC;

                //NUMERO
                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(68)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(68)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                NoReferencia = number.ToString("000000000");
                
                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";

                if (_Entrada)
                {
                    EntidadEntrada = db.Movimientos.FirstOrDefault(o => o.MovimientoReferenciaID.Equals(EntidadAnterior.ID));
                    NoReferencia = EntidadAnterior.Numero.ToString();
                    txtNroRecibido.Text = EntidadEntrada.Numero.ToString();
                    FechaTraslado = Convert.ToDateTime(EntidadAnterior.FechaRegistro);
                    Comentario = EntidadAnterior.Comentario;
                    glkESOut.Properties.DataSource = db.EstacionServicios.Where(o => o.Activo && o.ID.Equals(EntidadAnterior.EstacionServicioID)).Select(s => new { s.ID, s.Nombre }).ToList();
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    glkESOut.EditValue = IDEstacionServicio;
                    //IDSubEstacion = EntidadAnterior.SubEstacionID;
                    //glkSESOut.EditValue = IDSubEstacion;

                    glkESIn.EditValue = EntidadEntrada.EstacionServicioID;

                    DetalleOC.AddRange(EntidadAnterior.Kardexes.ToList());
                  
                    this.bdsDetalle.DataSource = this.DetalleOC;

                    //Opciones del Grid
                    gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                    gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                    gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;

                    //Columnas del Grid
                    colProductID.OptionsColumn.AllowFocus = false;
                    colProductID.OptionsColumn.AllowEdit = false;
                    colUnit.OptionsColumn.AllowFocus = false;
                    colUnit.OptionsColumn.AllowEdit = false;

                    ColAlmacenSalida.OptionsColumn.AllowFocus = false;
                    ColAlmacenSalida.OptionsColumn.AllowEdit = false;
                    ColAlmacenEntrada.Visible = true;
                    ColAlmacenEntrada.VisibleIndex = 3;
                    ColAlmacenEntrada.OptionsColumn.AllowFocus = true;
                    ColAlmacenEntrada.OptionsColumn.AllowEdit = true;

                    EsCombustible = db.AreaVentas.Single(o => o.ID.Equals(EntidadEntrada.AreaID)).EsCombustible;
                    

                    if (EsCombustible)
                    {
                        ColAlmacenSalida.Caption = "Tanque de Entrada";
                        listaAlmacenEntrada = from al in db.Tanques
                                       where al.Activo && al.EstacionServicioID.Equals(EntidadEntrada.EstacionServicioID) && al.SubEstacionID.Equals(EntidadEntrada.SubEstacionID)
                                       select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = al.ProductoID, Display = al.Nombre };
                    }
                    else
                    {
                        ColAlmacenSalida.Caption = "Almacen de Entrada";
                        listaAlmacenEntrada = from al in db.Almacens
                                       where al.Activo && al.EstacionServicioID.Equals(EntidadEntrada.EstacionServicioID) && al.SubEstacionID.Equals(EntidadEntrada.SubEstacionID)
                                       select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = 0, Display = al.Nombre };
                    }
                    cboAlmacenEntrada.DataSource = listaAlmacenEntrada;

                    colCantidadExistencia.Visible = false;
                    colCantidadSalida.OptionsColumn.AllowFocus = false;
                    colCantidadSalida.OptionsColumn.AllowEdit = false;
                    colCantidadSalida.Caption = "Cantidad de Entrada";

                    colCostSalida.OptionsColumn.AllowFocus = false;
                    colCostSalida.OptionsColumn.AllowEdit = false;
                    colCostoTotal.OptionsColumn.AllowFocus = false;
                    colCostoTotal.OptionsColumn.AllowEdit = false;
                    
                    gvDetalle.RefreshData();
                    
                    lcFechaSalida.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemNroEntrada.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    glkArea.Enabled = glkESIn.Enabled = glkESOut.Enabled = glkSESIn.Enabled = glkSESOut.Enabled = dateFechaTraslado.Enabled = txtNoFactura.Enabled = mmoComentario.Enabled = false;

                }
                else
                {

                    dateFechaTraslado.EditValue = Convert.ToDateTime(db.GetDateServer());
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaTraslado.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaTraslado.Date)).First().Valor : 0m);
                    
                    this.bdsDetalle.DataSource = this.DetalleOC;
                }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (_Entrada)
                {
                    AreaID = EntidadEntrada.AreaID;
                    //glkArea.EditValue = AreaID;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Validando DevErrorComponente
        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNoFactura, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkESIn, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkArea, errRequiredField);
        }

        public bool ValidarCampos(bool detalle)
        {
            if (!_Entrada)
            {
                if (dateFechaTraslado.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtNoFactura.Text == "" || glkArea.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del documento.", Parametros.MsgType.warning);
                    return false;
                }

                if (glkESIn.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar una Estación de Servicio para trasladar los productos.", Parametros.MsgType.warning);
                    return false;
                }

                if (lciSESin.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    if (glkSESIn.EditValue == null)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación de Servicio para trasladar los productos.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (!Parametros.General.ValidateTipoCambio(dateFechaTraslado, errRequiredField, db))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                    return false;
                }

                if (_TipoCambio <= 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidatePeriodoContable(FechaTraslado, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }


                if (FechaTraslado.Date > Convert.ToDateTime(db.GetDateServer()).Date)
                {
                    Parametros.General.DialogMsg("La fecha del documento no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
                    return false;
                }

                if (Parametros.General.ListSES.Count > 0)
                {
                    if (IDSubEstacion <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (IDEstacionServicio.Equals(Convert.ToInt32(glkESIn.EditValue)))
                {
                    if (lciSESin.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                    {
                        if (IDSubEstacion.Equals(Convert.ToInt32(glkSESIn.EditValue)))
                        {
                            Parametros.General.DialogMsg("La Sub Estación de Destino no puede ser igual a la Sub Estación de Origen.", Parametros.MsgType.warning);
                            return false;
                        }

                    }
                    else
                    {
                        Parametros.General.DialogMsg("La Estación de Destino no puede ser igual a la Estación de Origen.", Parametros.MsgType.warning);
                        return false;
                    }
                }
            }
            else
            {
                if (dateFechaRecibido.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la fecha de recibido.", Parametros.MsgType.warning);
                    return false;
                }
                else
                {
                    if (Convert.ToDateTime(dateFechaRecibido.EditValue) <= Convert.ToDateTime("01/01/2000"))
                    {
                        Parametros.General.DialogMsg("La fecha de recibido no es valida.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (DetalleOC.Count(o => o.AlmacenEntradaID.Equals(0)) > 0)
                {
                    Parametros.General.DialogMsg("Favor revisar los Almacenes de entradas.", Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidateTipoCambio(dateFechaRecibido, errRequiredField, db))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                    return false;
                }

                if (_TipoCambio <= 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(dateFechaRecibido.EditValue).Date, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }


                if (FechaTraslado.Date > Convert.ToDateTime(dateFechaRecibido.EditValue).Date)
                {
                    Parametros.General.DialogMsg("La fecha de Recibido no puede ser menor a la fecha original del Traslado.", Parametros.MsgType.warning);
                    return false;
                }

            }

            if (detalle)
            {
                if (DetalleOC.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a trasladar." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!_Entrada)
                {
                    //VALIDAS BLOQUEO
                    var obj = (from p in db.Productos
                               join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                               join ar in db.Areas on pc.AreaID equals ar.ID
                               where OC.Select(s => s.ProductoID).Contains(p.ID)
                               group ar by new { ar.ID, ar.Nombre } into gr
                               select new { ID = gr.Key.ID, Nombre = gr.Key.Nombre }).ToList();

                    string texto = "";
                    obj.ForEach(det =>
                    {
                        if (!Parametros.General.ValidateKardexMovemente(FechaTraslado, db, IDEstacionServicio, IDSubEstacion, (EsCombustible ? 24 : 9), (EsCombustible ? 0 : det.ID)))
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaTraslado.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                            texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                        }
                    });

                    if (!String.IsNullOrEmpty(texto))
                    {
                        infBloqueo.SetError(glkArea, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        return false;
                    }
                    else
                        infBloqueo.SetError(glkArea, "", ErrorType.None);
                }
                else
                {
                    //VALIDAS BLOQUEO
                    var obj = (from p in db.Productos
                               join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                               join ar in db.Areas on pc.AreaID equals ar.ID
                               where OC.Select(s => s.ProductoID).Contains(p.ID)
                               group ar by new { ar.ID, ar.Nombre } into gr
                               select new { ID = gr.Key.ID, Nombre = gr.Key.Nombre }).ToList();

                    string texto = "";
                    obj.ForEach(det =>
                    {
                        if (!Parametros.General.ValidateKardexMovemente(Convert.ToDateTime(dateFechaRecibido.EditValue), db, EntidadEntrada.EstacionServicioID, EntidadEntrada.SubEstacionID, (EsCombustible ? 24 : 9), (EsCombustible ? 0 : det.ID)))
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Convert.ToDateTime(dateFechaRecibido.EditValue).Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                            texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                        }
                    });

                    if (!String.IsNullOrEmpty(texto))
                    {
                        infBloqueo.SetError(dateFechaRecibido, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        return false;
                    }
                    else
                        infBloqueo.SetError(dateFechaRecibido, "", ErrorType.None);
                }
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(true)) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    if (!_Entrada)
                    {
                        #region <<< Traslado  >>>

                        Entidad.Movimiento M;

                        M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 68;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaTraslado;
                        M.MonedaID = _MonedaPrimaria;
                        M.TipoCambio = _TipoCambio;
                        M.Monto = Decimal.Round(DetalleOC.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(M.Monto / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                        M.AreaID = AreaID;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(68)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(68)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        M.Numero = number;

                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = Comentario;

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Traslado Externo de Invetario: " + M.Numero.ToString(), this.Name);

                        db.SubmitChanges();

                        Entidad.Movimiento RM = new Entidad.Movimiento();
                        RM.MovimientoTipoID = 69;
                        RM.UsuarioID = UsuarioID;
                        RM.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        RM.FechaRegistro = FechaTraslado;
                        RM.Monto = M.Monto;
                        RM.MonedaID = M.MonedaID;
                        RM.MontoMonedaSecundaria = M.MontoMonedaSecundaria;
                        RM.TipoCambio = M.TipoCambio;
                        RM.AreaID = M.AreaID;
                        RM.EstacionServicioID = Convert.ToInt32(glkESIn.EditValue);
                        RM.SubEstacionID = (lciSESin.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always ? Convert.ToInt32(glkSESIn.EditValue) : 0);

                        int Rnumber = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(RM.EstacionServicioID) && m.SubEstacionID.Equals(RM.SubEstacionID) && m.MovimientoTipoID.Equals(69)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(RM.EstacionServicioID) && m.SubEstacionID.Equals(RM.SubEstacionID) && m.MovimientoTipoID.Equals(69)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        RM.Numero = Rnumber;

                        RM.Referencia = "Recibo de Traslado " + M.Numero.ToString();
                        RM.Comentario = Comentario;
                        RM.MovimientoReferenciaID = M.ID;

                        db.Movimientos.InsertOnSubmit(RM);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró Recibo de Inventario: " + RM.Numero.ToString(), this.Name);

                        db.SubmitChanges();

                        #region <<< REGISTRANDO KARDEX >>>

                        foreach (var dk in DetalleOC)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;

                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.CantidadInicial = Decimal.Round(dk.CantidadInicial, 3, MidpointRounding.AwayFromZero);
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            if (dk.CantidadSalida <= 0)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("La cantidad del producto: " + Producto.Codigo + " | " + Producto.Nombre + " no puede ser igual a 0 (cero).", Parametros.MsgType.warning);
                                return false;
                            }

                            KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                            KX.CantidadSalida = Decimal.Round(dk.CantidadSalida, 3, MidpointRounding.AwayFromZero);
                            KX.CostoSalida = Decimal.Round(dk.CostoSalida, 4, MidpointRounding.AwayFromZero);
                            KX.CostoFinal = Decimal.Round(KX.CostoSalida, 4, MidpointRounding.AwayFromZero);
                            KX.CostoTotal = Decimal.Round(KX.CostoSalida * KX.CantidadSalida, 2, MidpointRounding.AwayFromZero);

                            if (KX.CantidadSalida > KX.CantidadInicial)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                return false;
                            }

                            KX.CantidadFinal = Decimal.Round(KX.CantidadInicial - KX.CantidadSalida, 3, MidpointRounding.AwayFromZero);
                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();

                       
                        }

                        #endregion

                        #region <<< REGISTRANDO COMPROBANTE >>>
                        if (!M.EstacionServicioID.Equals(RM.EstacionServicioID))
                        {
                            List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                            var obj = from cds in Compronbante
                                      join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                                      join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                                      select new
                                      {
                                          Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                          Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                      };

                            if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }
                            Compronbante.ForEach(linea =>
                                {
                                    M.ComprobanteContables.Add(linea);
                                    db.SubmitChanges();
                                });

                            db.SubmitChanges();
                        }
                        #endregion

                        #endregion
                    }
                    else
                    {
                        #region <<< Recibido  >>>

                        Entidad.Movimiento M = db.Movimientos.Single(o => o.ID.Equals(EntidadEntrada.ID));

                        M.FechaRegistro = Convert.ToDateTime(dateFechaRecibido.EditValue).Date;
                        M.Finalizado = true;

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se finalizó el Recibo Externo de Invetario: " + M.Numero.ToString(), this.Name);

                        db.SubmitChanges();

                        #region <<< REGISTRANDO KARDEX >>>

                        foreach (var dk in DetalleOC)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;

                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                           
                            KX.EstacionServicioID = EntidadEntrada.EstacionServicioID;
                            KX.SubEstacionID = EntidadEntrada.SubEstacionID;
                            KX.AlmacenEntradaID = dk.AlmacenEntradaID;

                            if (dk.CantidadSalida <= 0)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("Debe seleccionar el Almacen / Tanque de Entrada para el producto: " + Producto.Codigo + " | " + Producto.Nombre, Parametros.MsgType.warning);
                                return false;
                            }

                            KX.CantidadEntrada = Decimal.Round(dk.CantidadSalida, 3, MidpointRounding.AwayFromZero);
                            
                            KX.CantidadInicial = Parametros.General.SaldoKardexCompra(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.Fecha, true);
                            KX.CantidadFinal = Decimal.Round(KX.CantidadInicial + KX.CantidadEntrada, 3, MidpointRounding.AwayFromZero);
                           

                            //Entrada
                            decimal VCtoEntrada, vCtoFinal;
                            Parametros.General.LastCost(db, M.EstacionServicioID, M.SubEstacionID, Producto.ID, 0, Convert.ToDateTime(dateFechaRecibido.EditValue).Date, out vCtoFinal, out VCtoEntrada);

                            KX.CostoInicial = Decimal.Round(vCtoFinal, 4, MidpointRounding.AwayFromZero);

                            KX.CostoEntrada = Decimal.Round(dk.CostoSalida, 4, MidpointRounding.AwayFromZero);

                            KX.CostoTotal = Decimal.Round(KX.CostoEntrada * KX.CantidadEntrada, 2, MidpointRounding.AwayFromZero);

                            decimal vSaldo = 0m;
                            Parametros.General.SaldoCostoTotalKardex(db, KX.ID, 0, M.EstacionServicioID, M.SubEstacionID, Producto.ID, Convert.ToDateTime(dateFechaRecibido.EditValue).Date, out vSaldo);

                            if (KX.CantidadFinal > 0)
                                KX.CostoFinal = Math.Abs(Decimal.Round(Convert.ToDecimal((vSaldo + KX.CostoTotal) / KX.CantidadFinal), 4, MidpointRounding.AwayFromZero));
                            else
                                KX.CostoFinal = Math.Abs(Decimal.Round(KX.CostoEntrada, 4, MidpointRounding.AwayFromZero));

                            ////////////////////////////////////////////////////////////////////////////
                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();
                        }

                        #endregion

                        #region <<< REGISTRANDO COMPROBANTE >>>
                        if (!EntidadAnterior.EstacionServicioID.Equals(EntidadEntrada.EstacionServicioID))
                        {
                            List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                            var obj = from cds in Compronbante
                                      join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                                      join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                                      select new
                                      {
                                          Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                          Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                      };

                            if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }
                            Compronbante.ForEach(linea =>
                            {
                                M.ComprobanteContables.Add(linea);
                                db.SubmitChanges();
                            });

                            db.SubmitChanges();
                        }
                        #endregion

                        #endregion
                    }

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    return true;
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        private List<Entidad.ComprobanteContable> PartidasContable
        {

            get
            {
                try
                {
                    
                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                    string texto = "Traslado de Inventario " + NoReferencia;
                    int i = 1;

                    if (!_Entrada)
                    {
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = db.EstacionServicios.Single(s => s.ID.Equals(Convert.ToInt32(glkESIn.EditValue))).CuentaInternaActivo,
                            Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = FechaTraslado,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion,
                            CentroCostoID = 0

                        });
                        i++;

                        List<Int32> IDInventario = new List<Int32>();

                        #region <<< DETALLE_COMPROBANTE >>>
                        DetalleOC.ToList().ForEach(K =>
                        {
                            var area = from a in db.Areas
                                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                       where p.ID.Equals(K.ProductoID)
                                       select a;

                            #region <<< CUENTA_INVENTARIO >>>
                            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                            {
                                if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = area.First().CuentaInventarioID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaTraslado,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion,
                                        CentroCostoID = 0
                                    });
                                    IDInventario.Add(area.First().CuentaInventarioID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                    comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            }
                            else
                            {
                                var productoCuenta = db.Productos.Single(p => p.ID.Equals(K.ProductoID)).CuentaInventarioID;
                                if (!IDInventario.Contains(productoCuenta))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = productoCuenta,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaTraslado,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    IDInventario.Add(productoCuenta);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(productoCuenta)).First();
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion

                        });

                        #endregion

                       
                    }
                    else
                    {
                        List<Int32> IDInventario = new List<Int32>();

                        #region <<< DETALLE_COMPROBANTE >>>
                        DetalleOC.ToList().ForEach(K =>
                        {
                            var area = from a in db.Areas
                                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                       where p.ID.Equals(K.ProductoID)
                                       select a;

                            #region <<< CUENTA_INVENTARIO >>>
                            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                            {
                                if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = area.First().CuentaInventarioID,
                                        Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaTraslado,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion,
                                        CentroCostoID = 0
                                    });
                                    IDInventario.Add(area.First().CuentaInventarioID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                    comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            }
                            else
                            {
                                var productoCuenta = db.Productos.Single(p => p.ID.Equals(K.ProductoID)).CuentaInventarioID;
                                if (!IDInventario.Contains(productoCuenta))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = productoCuenta,
                                        Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaTraslado,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    IDInventario.Add(productoCuenta);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(productoCuenta)).First();
                                    comprobante.Monto += Decimal.Round(Math.Abs(Convert.ToDecimal(K.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion

                        });

                        #endregion

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = db.EstacionServicios.Single(s => s.ID.Equals(Convert.ToInt32(EntidadAnterior.EstacionServicioID))).CuentaInternaPasivo,
                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = FechaTraslado,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion,
                            CentroCostoID = 0

                        });
                        i++;

                    }

                    return CD;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {           
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, NextBien);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !OnlyView)
            {
                if (DetalleOC.Count > 0 || txtNoFactura.Text != "" || string.IsNullOrEmpty(mmoComentario.Text))
                {
                    if (Parametros.General.DialogMsg("El Movimiento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            EntidadAnterior = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #region <<< GRID_DETALLES >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "ProductoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
                    e.ErrorText = "Debe Seleccionar un Producto";
                    e.Valid = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
                e.ErrorText = "Debe Seleccionar un Producto";
                e.Valid = false;
            }

            //-- Validar Columna de Almacen / Tanque             
            if (view.GetRowCellValue(RowHandle, "AlmacenSalidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen / Tanque");
                    e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                    e.Valid = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen / Tanque");
                e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                e.Valid = false;
            }

            if (_Entrada)
            {
                //-- Validar Columna de Almacen / Tanque             
                if (view.GetRowCellValue(RowHandle, "AlmacenEntradaID") != null)
                {
                    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID")) == 0)
                    {
                        view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen / Tanque para ingresar el Producto");
                        e.ErrorText = "Debe Seleccionar un Almacen / Tanque para ingresar el Producto";
                        e.Valid = false;
                    }
                }
                else
                {
                    view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen / Tanque para ingresar el Producto");
                    e.ErrorText = "Debe Seleccionar un Almacen / Tanque para ingresar el Producto";
                    e.Valid = false;
                }
            }

            //-- Validar Columna Cantidad de Salida             
            if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) == 0)
                {
                    view.SetColumnError(view.Columns["CantidadSalida"], "Debe ingresar una cantidad a salir");
                    e.ErrorText = "Debe ingresar una cantidad a salir";
                    e.Valid = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CantidadSalida"], "Debe ingresar una cantidad a salir");
                e.ErrorText = "Debe ingresar una cantidad a salir";
                e.Valid = false;
            }

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (!_Entrada)
            {
                #region <<< CALCULOS_MONTOS >>>
                //--  Calcular las montos de las columnas de Impuestos y Subtotal
                //&& IDConepto.Equals(IDInvntarioInicial)
                if (e.Column == colCantidadSalida)//)
                {
                    try
                    {
                        decimal vCantidadInicial = 0, vCosto = 0, vCantidad = 0, vCostoTotal = 0;

                        if (_Entrada)
                        {
                            #region <<< ENTRADAS  >>>

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                                vCosto = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida")), 4, MidpointRounding.AwayFromZero);
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                                vCantidad = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")), 3, MidpointRounding.AwayFromZero);
                            //else
                            //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                            vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);

                            gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);


                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial") != null)
                                vCantidadInicial = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial"));

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                            {
                                if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                                {
                                    if (vCantidad > vCantidadInicial)
                                    {
                                        Parametros.General.DialogMsg("La cantidad a salir sobrepasa la existencia", Parametros.MsgType.warning);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (!_Entrada)
                        {
                            #region <<< SALIDAS >>>

                            //OBTENIENDO VALORES
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial") != null)
                                vCantidadInicial = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")), 3, MidpointRounding.AwayFromZero);

                            if (vCantidadInicial <= 0)
                            {
                                Parametros.General.DialogMsg("La cantidad de salida no puede exceder a la cantidad en existencia.", Parametros.MsgType.warning);
                                return;
                            }

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                                vCantidad = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")), 3, MidpointRounding.AwayFromZero);

                            if (vCantidad > vCantidadInicial)
                            {
                                Parametros.General.DialogMsg("La cantidad de salida no puede exceder a la cantidad en existencia.", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                return;
                            }

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                                vCosto = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida")), 4, MidpointRounding.AwayFromZero);

                            vCostoTotal = Decimal.Round(vCosto * vCantidad, 2, MidpointRounding.AwayFromZero);
                            gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }

                }
                #endregion

                #region <<< COLUMNA_PRODUCTO >>>
                //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
                if (e.Column == colProductID)
                {

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
                        {
                            return;
                        }
                        else if (DetalleOC.Count(d => d.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                        {
                            Parametros.General.DialogMsg("El producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                            gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                            gvDetalle.FocusedColumn = colProductID;
                            return;
                        }
                    }

                    try
                    {
                        var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                        //-- Unidad Principal     
                        gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                        //OBTENER COSTO
                        decimal VCtoEntrada, vCtoFinal;
                        Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaTraslado, out vCtoFinal, out VCtoEntrada);
                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCtoFinal);
                        //OBTENER CANTIDAD
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaTraslado, false));

                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) < 0)
                        {
                            Parametros.General.DialogMsg("El producto seleccionado tiene existencia invalida.", Parametros.MsgType.warning);
                            gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                            gvDetalle.FocusedColumn = colProductID;
                            return;
                        }


                        if (!EsCombustible)
                        {
                            var lista = listaAlmacen.Where(l => !l.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))));
                            cboAlmacenEntrada.DataSource = lista;//lkAlmacen.Properties.DataSource;
                            //gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", (_Entrada.Equals(Parametros.TiposMovimientoInventario.Entrada) ? Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")) : (lista.Count() > 0 ? lista.First().ID : 0)));

                            //-- Cantidad Inicial de 0
                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                        }
                        else if (EsCombustible)
                        {
                            var lista = listaAlmacen.Where(t => t.Code.Equals(Producto.ID));
                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", (lista.Count() > 0 ? lista.First().ID : 0));

                            //-- Cantidad Inicial de 0
                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
                //--
                #endregion

                #region <<< COLUMNA_ALMACEN  >>>

                if (e.Column == ColAlmacenSalida)
                {
                    try
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") != null)
                                {
                                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")) > 0)
                                    {
                                        int Producto = Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"));

                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaTraslado, false));

                                        if (!EsCombustible)
                                        {
                                            var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) & a.ProductoID.Equals(Producto));

                                            if (query.ToList().Count > 0 && Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) > 0)
                                            {
                                                //decimal vExistencia = query.First().Cantidad;
                                                //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Decimal.Round(vExistencia, 3, MidpointRounding.AwayFromZero));
                                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);
                                            }
                                            else
                                            {
                                                Parametros.General.DialogMsg("El producto no se encuentra en el almacen seleccionado.", Parametros.MsgType.warning);
                                                gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                            }
                                        }
                                    }
                                }

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
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                    }
                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                view.AddNewRow();
            }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                    }
                }
            }
        }
        
        #endregion

        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos(false))
                    return;

                if (IDEstacionServicio.Equals(Convert.ToInt32(glkESIn.EditValue)))
                {
                    Parametros.General.DialogMsg("El Traslado entre Sub Estaciones no genera Comprobante Contable." + Environment.NewLine, Parametros.MsgType.warning);
                    return;
                }

                if (DetalleOC.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a trasladar." + Environment.NewLine, Parametros.MsgType.warning);
                    return;
                }

                using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                {
                    nf.DetalleCD = PartidasContable;
                    nf.Text = "Comprobante Contable Traslado de Inventario";
                    nf.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaVencimiento_EditValueChanged(object sender, EventArgs e)
        {
            txtNombre_Validated_1(sender, null);
        }         

        //Botones para accesos directos
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.btnOK.Visible.Equals(true))
            {
                if (keyData == (Keys.F7))
                {
                    btnOK_Click_1(null, null);
                    return true;
                }
            }

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
                return true;
            }

            if (this.bntNew.Enabled.Equals(true))
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    bntNew_Click(null, null);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        //EVENTO REGISTRAR BIENES
        private void bntNew_Click(object sender, EventArgs e)
        {            
                NextBien = true;
                RefreshMDI = false;
                ShowMsg = false;
                this.Close();
        } 
       
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                dateFechaTraslado.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
            {
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaTraslado.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaTraslado.Date)).First().Valor : 0m);
            }
        }
        #endregion

        //EVENTO CONTROL Area de Venta Seleccionada
        private void glkTipoMovimiento_EditValueChanged(object sender, EventArgs e)
        {
            if (glkArea.EditValue != null)
            {
                try
                {
                    
                    if (AreaID > 0)
                    {
                        ValidateBloqueo((BaseEdit)sender, infBloqueo);
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        //LLENADO GRID PRODUCTO
                        gridProductos.DataSource = null;
                        var ListaProductos = from P in db.Productos
                                             join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                             join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                             join A in db.Areas on C.AreaID equals A.ID
                                             join AR in db.AreaVentas on A.AreaVentaID equals AR.ID
                                             where P.Activo.Equals(true) && A.AreaVentaID.Equals(AreaID)
                                             && Lkardex.Contains(P.ID)
                                             group P by new { P.ID, P.Codigo, P.Nombre, UnidadNombre = U.Nombre, UnidadID = U.ID } into gr
                                             select new strucprod.Productos
                                             {
                                                 ProductoID = gr.Key.ID,
                                                 ProductoCodigo = gr.Key.Codigo,
                                                 ProductoNombre = gr.Key.Nombre,
                                                 UnidadID = gr.Key.UnidadID,
                                                 UnidadNombre = gr.Key.UnidadNombre,
                                                 Display = gr.Key.Codigo + " | " + gr.Key.Nombre,
                                                 Cantidad = 0,
                                                 Costo = 0
                                             };
                        if (ListaProductos == null)
                            return;
                        gridProductos.DataSource = ListaProductos.Select(s => new
                        {
                            s.ProductoID,
                            s.ProductoCodigo,
                            s.ProductoNombre,
                            s.UnidadID,
                            s.UnidadNombre,
                            s.Display,
                            s.Cantidad,
                            s.Costo
                        }).ToList();

                        //DEFICINICION GRID PRODUCTOS
                        if ((int)glkArea.EditValue == 3) EsCombustible = true;
                        else EsCombustible = false;

                        //---LLenar Almacenes ---//
                        if (EsCombustible)
                        {
                            if (!_Entrada)
                            {
                                ColAlmacenSalida.Caption = "Tanque de Salida";
                                listaAlmacen = from al in db.Tanques
                                               where al.Activo && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                               select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = al.ProductoID, Display = al.Nombre };
                            }
                            else
                            {
                                ColAlmacenSalida.Caption = "Tanque de Salida";
                                listaAlmacen = from al in db.Tanques
                                               where al.Activo && al.EstacionServicioID.Equals(EntidadAnterior.EstacionServicioID) && al.SubEstacionID.Equals(EntidadAnterior.SubEstacionID)
                                               select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = al.ProductoID, Display = al.Nombre };                           
                            }
                        }
                        else
                        {
                            if (!_Entrada)
                            {
                                ColAlmacenSalida.Caption = "Almacen de Salida";
                                listaAlmacen = from al in db.Almacens
                                               where al.Activo && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                               select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = 0, Display = al.Nombre };
                            }
                            else
                            {
                                ColAlmacenSalida.Caption = "Almacen de Salida";
                                listaAlmacen = from al in db.Almacens
                                               where al.Activo && al.EstacionServicioID.Equals(EntidadAnterior.EstacionServicioID) && al.SubEstacionID.Equals(EntidadAnterior.SubEstacionID)
                                               select new Parametros.ListIdCodeDisplay { ID = al.ID, Code = 0, Display = al.Nombre };                           
                            }
                        }

                        //LLENAR LISTA COLUMNA ALMACEN
                        cboAlmacenSalida.DataSource = listaAlmacen.ToList();
                        cboAlmacenSalida.DisplayMember = "Display";
                        cboAlmacenSalida.ValueMember = "ID";

                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }        
        }

        //VALIDAR BLOQUEO COMPONENTE LKAREA
        private void ValidateBloqueo(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider)
        {
            try
            {
                if (dateFechaTraslado.EditValue != null)
                {
                    if (FechaTraslado.Date > Convert.ToDateTime("01/01/2000").Date)
                    {
                        string texto = "";
                        EtAreas.Where(o => o.ID.Equals(AreaID)).ToList().ForEach(det =>
                        {
                            if (!Parametros.General.ValidateKardexMovemente(FechaTraslado, db, IDEstacionServicio, IDSubEstacion, (EsCombustible ? 24 : 9), (EsCombustible ? 0 : det.AreaID)))
                            {
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaTraslado.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                                texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                            }
                        });

                        if (!String.IsNullOrEmpty(texto))
                            dxErrorProvider.SetError(control, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        else
                            dxErrorProvider.SetError(control, "", ErrorType.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //VALIDACION CAMBIO DE FECHA
        private void dateFechaTraslado_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dateFechaTraslado.EditValue != null)
                {
                    if (FechaTraslado.Date > Convert.ToDateTime("01/01/2000").Date)
                    {
                        string texto = "";
                        EtAreas.Where(o => o.ID.Equals(AreaID)).ToList().ForEach(det =>
                        {
                            if (!Parametros.General.ValidateKardexMovemente(FechaTraslado, db, IDEstacionServicio, IDSubEstacion, (EsCombustible ? 24 : 9), (EsCombustible ? 0 : det.AreaID)))
                            {
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaTraslado.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                                texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                            }
                        });

                        if (!String.IsNullOrEmpty(texto))
                        {
                            infBloqueo.SetError(glkArea, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        }
                        else
                        {
                            infBloqueo.SetError(glkArea, "", ErrorType.None);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void glkESOut_EditValueChanged(object sender, EventArgs e)
        {
            if (!_Entrada)
            {
                List<ListaSubEstacionesServicio> lses = (from ses in db.SubEstacions
                                                         where ses.Activo && (ses.ID == IDSubEstacion)
                                                         select new ListaSubEstacionesServicio { ID = ses.ID, Nombre = ses.Nombre, EstacionID = ses.EstacionServicioID }).ToList();
                if (lses.Count == 0) return;
                if (les.First().ID == lses.First().EstacionID)
                {
                    glkSESOut.Properties.DataSource = db.SubEstacions.Where(o => o.Activo && o.ID.Equals(IDSubEstacion)).Select(s => new { s.ID, s.Nombre }).ToList();
                    lciSESout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    glkSESOut.EditValue = IDSubEstacion;

                }
            }
            else
            {
                List<ListaSubEstacionesServicio> lses = (from ses in db.SubEstacions
                                                         where ses.Activo && (ses.ID == EntidadAnterior.SubEstacionID)
                                                         select new ListaSubEstacionesServicio { ID = ses.ID, Nombre = ses.Nombre, EstacionID = ses.EstacionServicioID }).ToList();
                if (lses.Count == 0) return;
                if (les.First().ID == lses.First().EstacionID)
                {
                    glkSESOut.Properties.DataSource = db.SubEstacions.Where(o => o.Activo && o.ID.Equals(EntidadAnterior.SubEstacionID)).Select(s => new { s.ID, s.Nombre }).ToList();
                    lciSESout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    glkSESOut.EditValue = EntidadAnterior.SubEstacionID;

                }
            }
        }

        private void glkESIn_EditValueChanged(object sender, EventArgs e)
        {
            if (!_Entrada)
            {
                List<ListaSubEstacionesServicio> lses = (from ses in db.SubEstacions
                                                         where ses.Activo && (ses.EstacionServicioID == (int)glkESIn.EditValue)
                                                         select new ListaSubEstacionesServicio { ID = ses.ID, Nombre = ses.Nombre, EstacionID = ses.EstacionServicioID }).ToList();

                glkSESIn.EditValue = null;
                glkSESIn.Properties.DataSource = null;
                glkSESIn.Properties.NullText = "[Seleccione una subestación]";
                if (lses.Count == 0)
                {
                    lciSESin.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    return;
                }
                glkSESIn.Properties.DataSource = db.SubEstacions.Where(o => o.Activo && o.EstacionServicioID.Equals(glkESIn.EditValue)).Select(s => new { s.ID, s.Nombre }).ToList();
                lciSESin.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                List<ListaSubEstacionesServicio> lses = (from ses in db.SubEstacions
                                                         where ses.Activo && (ses.EstacionServicioID == (int)glkESIn.EditValue)
                                                         select new ListaSubEstacionesServicio { ID = ses.ID, Nombre = ses.Nombre, EstacionID = ses.EstacionServicioID }).ToList();

                glkSESIn.EditValue = null;
                glkSESIn.Properties.DataSource = null;
                glkSESIn.Properties.NullText = "[Seleccione una subestación]";
                if (lses.Count == 0)
                {
                    lciSESin.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    return;
                }
                glkSESIn.Properties.DataSource = db.SubEstacions.Where(o => o.Activo && o.EstacionServicioID.Equals(glkESIn.EditValue)).Select(s => new { s.ID, s.Nombre }).ToList();
                lciSESin.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                glkSESIn.EditValue = EntidadEntrada.SubEstacionID;
            }
        }

        //LLenado del datasource Almacen
        private void cboAlmacenSalida_QueryPopUp(object sender, CancelEventArgs e)
        {
            try
            {
                if (EsCombustible)
                {
                    DevExpress.XtraEditors.LookUpEdit popupForm = (sender as DevExpress.XtraEditors.LookUpEdit);

                    //ID del PRODUCTO
                    int p = 0;

                    if (gvDetalle.GetFocusedRowCellValue(colProductID) != null)
                        p = Convert.ToInt32(gvDetalle.GetFocusedRowCellValue(colProductID));

                    popupForm.Properties.DataSource = listaAlmacen.Where(o => o.Code.Equals(p));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void cboAlmacenSalida_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            if (EsCombustible)
            {
                DevExpress.XtraEditors.LookUpEdit popupForm = (sender as DevExpress.XtraEditors.LookUpEdit);
                popupForm.Properties.DataSource = listaAlmacen;
            }
        }

        private void dateFechaRecibido_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dateFechaRecibido.EditValue != null)
                {
                    if (Convert.ToDateTime(dateFechaRecibido.EditValue).Date > Convert.ToDateTime("01/01/2000").Date)
                    {
                        string texto = "";
                        EtAreas.Where(o => o.ID.Equals(AreaID)).ToList().ForEach(det =>
                        {
                            if (!Parametros.General.ValidateKardexMovemente(Convert.ToDateTime(dateFechaRecibido.EditValue).Date, db, EntidadEntrada.EstacionServicioID, EntidadEntrada.SubEstacionID, (EsCombustible ? 24 : 9), (EsCombustible ? 0 : det.AreaID)))
                            {
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Convert.ToDateTime(dateFechaRecibido.EditValue).Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                                texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                            }
                        });

                        if (!String.IsNullOrEmpty(texto))
                        {
                            infBloqueo.SetError(dateFechaRecibido, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        }
                        else
                        {
                            infBloqueo.SetError(dateFechaRecibido, "", ErrorType.None);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaRecibido_Validated(object sender, EventArgs e)
        {

            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                dateFechaRecibido.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
            {
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Convert.ToDateTime(dateFechaRecibido.EditValue))) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Convert.ToDateTime(dateFechaRecibido.EditValue))).First().Valor : 0m);
            }
        }

        private void cboAlmacenEntrada_QueryPopUp(object sender, CancelEventArgs e)
        {
            try
            {
                if (EsCombustible)
                {
                    DevExpress.XtraEditors.LookUpEdit popupForm = (sender as DevExpress.XtraEditors.LookUpEdit);

                    //ID del PRODUCTO
                    int p = 0;

                    if (gvDetalle.GetFocusedRowCellValue(colProductID) != null)
                        p = Convert.ToInt32(gvDetalle.GetFocusedRowCellValue(colProductID));

                    popupForm.Properties.DataSource = listaAlmacenEntrada.Where(o => o.Code.Equals(p));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void cboAlmacenEntrada_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            if (EsCombustible)
            {
                DevExpress.XtraEditors.LookUpEdit popupForm = (sender as DevExpress.XtraEditors.LookUpEdit);
                popupForm.Properties.DataSource = listaAlmacenEntrada;
            }
        }

       
    }

    public class strucprod
    {
        public struct Productos // 
        {
            public int ProductoID;
            public string ProductoCodigo;
            public string ProductoNombre;
            public string Display;
            public int UnidadID;
            public string UnidadNombre;
            public int AreaVentaID;
            public string AreaVentaNombte;
            //bool Activo;
            public decimal Cantidad;
            public decimal Costo;
        }
    }
   
}