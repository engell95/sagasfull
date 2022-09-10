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

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogPrestamoManejo : Form
    {
        #region *** DECLARACIONES ***
        #region<<Acta>>
        //private List<Entidad.ActaCombustible> Actas = new List<Entidad.ActaCombustible>();
        //public List<Entidad.ActaCombustible> A
        //{
        //    get { return Actas; }
        //    set
        //    {
        //        Actas = value;
        //        this.bdsDetalle.DataSource = this.Actas;
        //    }
        //}
        #endregion

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormPrestamoManejo MDI;
        internal Entidad.Movimiento EntidadMov;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        private int IDPrint = 0;
        private decimal _Disponible = 0;
        internal bool Next = false;
        private decimal _TipoCambio = 0;
        //Prestamo Manejo a E/S
        internal int _CuentaProveedorPrestamoManejoID = 0;
        internal decimal _SaldoProvedorPrestamoManejoMovID = 0m;
        //Prestamo E/S a Manejo
        internal int _CuentaClientePrestamoManejoID = 0;
        internal decimal _SaldoClientePrestamoManejoMovID = 0m;
        internal int _ClaseCombustible;

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Comentario
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }
        
        private List<Entidad.Kardex> EM = new List<Entidad.Kardex>();
        public List<Entidad.Kardex> DetalleEM
        {
            get { return EM; }
            set
            {
                EM = value;
                this.bdsDetalle.DataSource = this.EM;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogPrestamoManejo(int UserID)
        {            
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //rgCreditoContado.SelectedIndex = 0;
            //IDProveedor = 186;
            //Referencia = "123";
            //IDAlmacen = 1;
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
              //  _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();

                var prov = db.Clientes.FirstOrDefault(p => p.ID.Equals(Parametros.Config.ProveedorPrestamoManejoID()));

                if (prov != null)
                    _CuentaProveedorPrestamoManejoID = prov.CuentaContableID;

                var cliente = db.Clientes.FirstOrDefault(p => p.ID.Equals(Parametros.Config.ClientePrestamoManejoID()));

                if (cliente != null)
                    _CuentaClientePrestamoManejoID = cliente.CuentaContableID;
                   

                //---LLenar Almacenes ---//
                listaTanque = from t in db.Tanques
                              join p in db.Productos on t.ProductoID equals p.ID
                              where t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                              select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre + " | " + p.Nombre };

                //Almacen GRID
                cboAlmacenSalida.DataSource = listaTanque.ToList();
                //cboAlmacenEntrada.DisplayMember = "Display";
                //cboAlmacenEntrada.ValueMember = "ID";
                
                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//


                //--- Fill Combos Detalles --//
                //splashGrid = new DevExpress.XtraSplashScreen.SplashScreenManager( Formulario, typeof(global::SAGAS.Parametros.Forms.Dialogs.WaitSplashForm), true, true);
                //splashGrid.ShowWaitForm()
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                gridProductos.DataSource = from P in db.Productos
                                     join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                     join t in db.Tanques.Where(t => t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion))  on P.ID equals t.ProductoID
                                     where P.Activo & P.ProductoClaseID.Equals(_ClaseCombustible)
                                        group P by new { P.ID, P.Codigo, P.Nombre, Unidad = U.Nombre } into gr
                                    select new
                                     {
                                         ID = gr.Key.ID,
                                         Codigo = gr.Key.Codigo,
                                         Nombre = gr.Key.Nombre,
                                         UmidadName = gr.Key.Unidad,
                                         Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                     };

                dateFecha.EditValue = Convert.ToDateTime(db.GetDateServer());

                this.bdsDetalle.DataSource = this.DetalleEM;

                if (dateFecha.EditValue != null)
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);

                rgTipo.SelectedIndex = -1;
                rgManejo.SelectedIndex = -1;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            int MTID = 0;

            if (rgManejo.SelectedIndex.Equals(0))
            {
                if (rgTipo.SelectedIndex.Equals(0))
                    MTID = 31;
            }
            else
            {
                if (rgTipo.SelectedIndex.Equals(0))
                    MTID = 35;
            }

            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && !i.Anulado && i.MovimientoTipoID.Equals(MTID)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            try
            { 
                if (txtNumero.Text == "" || memoComentario.Text == "" || dateFecha.EditValue == null || _TipoCambio.Equals(0))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                    return false;
                }

                if (rgManejo.SelectedIndex.Equals(0))
                {
                    if (_CuentaProveedorPrestamoManejoID.Equals(0))
                    {
                        Parametros.General.DialogMsg("No existe cuenta contable realcionada con el proveedor Manejo.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }

                if (rgManejo.SelectedIndex.Equals(1))
                {
                    if (_CuentaClientePrestamoManejoID.Equals(0))
                    {
                        Parametros.General.DialogMsg("No existe cuenta contable realcionada con el cliente Manejo.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (Parametros.General.ListSES.Count > 0)
                {
                    if (IDSubEstacion <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (!detalle)
                {
                    if (DetalleEM.Count <= 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    else {
                        
                              //BLOQUEO PARA COMBUSTIBLE
                            List<int> PAreas = (from p in db.Productos
                                                join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                                where DetalleEM.Select(s => s.ProductoID).Contains(p.ID) && pc.ID.Equals(_ClaseCombustible)
                                                group pc by pc.AreaID into gr
                                                select gr
                                               ).Select(s => s.Key).ToList();

                            if (PAreas.Count > 0)
                            {
                                if (!Parametros.General.ValidateKardexMovemente(Fecha.Date, db, IDEstacionServicio, IDSubEstacion, 24, 0))
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Fecha.Date.ToShortDateString(), Parametros.MsgType.warning);
                                    return false;
                                }
                            }
                        
                    }


                    if (!ValidarReferencia(Convert.ToString(Numero)))
                    {
                        Parametros.General.DialogMsg("La referencia para esta registro ya existe : " + Convert.ToString(Numero), Parametros.MsgType.warning);
                        return false;
                    }
                }

               
                return true;
            }
            catch { return false; }
        }

        private bool Guardar(int manejo)
        {
            if (!ValidarCampos(false))
                return false;

            bool Bloqueo = false;
            
            if (!(manejo == 0 && rgTipo.SelectedIndex == 0))
            {
                foreach (Entidad.Kardex K in DetalleEM) {
                    if (K.CantidadInicial < K.CantidadSalida) {
                        Bloqueo = true;
                        break;
                    }
                }
            }

            if (Bloqueo)
            {
                Parametros.General.DialogMsg("La cantidad de reposicion no puede ser mayor a la existencia", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateKardexMovemente(Fecha, db, IDEstacionServicio, IDSubEstacion, 24, 0))
            {
                Parametros.General.DialogMsg("El el acta ya esta cerrada", Parametros.MsgType.warning);
                return false;
            }

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();
            
            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    if (manejo.Equals(0))
                    {

                        #region <<< TIPO Prestamo Manejo de Cliente Manejo a E/S >>>

                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                        int IDTipo = rgTipo.SelectedIndex;

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        M.MovimientoTipoID = (IDTipo.Equals(0) ? 31 : 33);
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Fecha;
                        M.ClienteID = db.Clientes.FirstOrDefault(p => p.ID.Equals(Parametros.Config.ProveedorPrestamoManejoID())).ID;
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.Monto = Math.Abs(IDTipo.Equals(0) ? Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) : spSaldo.Value);
                        M.MontoMonedaSecundaria = Math.Abs(IDTipo.Equals(0) ? Decimal.Round((DetalleEM.Sum(s => s.CostoTotal) / M.TipoCambio), 2, MidpointRounding.AwayFromZero) : Decimal.Round((spSaldo.Value / M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                    
                        int number = 1;
                        if (IDTipo.Equals(0))
                        {
                            if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(31)) > 0)
                            {
                                number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(31)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                            }
                        }
                        else if (IDTipo.Equals(1))
                        {
                            if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(33)) > 0)
                            {
                                number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(33)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                            }
                        }
                         
                                               
                        M.Numero = number;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = memoComentario.Text;
                        M.Litros = Convert.ToDecimal(DetalleEM.Sum(s => s.CantidadSalida));
                        M.MovimientoReferenciaID = (IDTipo.Equals(0) ? 0 : EntidadMov.ID);

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró Prestamo Manejo de Cliente Manejo a E/S: " + M.Numero, this.Name);

                        db.SubmitChanges();

                        var EtProductos = db.Productos.Select(s => new {s.ID, s.Codigo, s.Nombre, s.EsServicio}).Where(o => DetalleEM.Select(s => s.ProductoID).Contains(o.ID));
                    
                        #region ::: REGISTRANDO EN KARDEX DE BD :::

                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        foreach (var dk in DetalleEM)
                        {
                            var Producto = EtProductos.Single(s => s.ID.Equals(dk.ProductoID));
                            if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                            {
                                trans.Rollback();
                                Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                                return false;
                            }

                            #region <<< Primera Linea >>>
                        
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            if (IDTipo.Equals(0))
                            {
                                #region Entrada
                                KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                KX.CantidadEntrada = dk.CantidadSalida;
                                KX.CantidadInicial = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoSalida;
                                KX.CostoEntrada = dk.CostoSalida;
                                KX.CostoFinal = dk.CostoSalida;
                                KX.CostoTotal = Decimal.Round(KX.CantidadEntrada * KX.CostoEntrada, 2, MidpointRounding.AwayFromZero);

                                KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;

                                //if (KX.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                                #endregion
                            }
                            else
                            {
                                #region Reposicion
                                KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KX.CantidadSalida = dk.CantidadSalida;
                                KX.CantidadInicial = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoSalida;
                                KX.CostoSalida = dk.CostoSalida;
                                KX.CostoFinal = dk.CostoSalida;
                                KX.CostoTotal = Decimal.Round(KX.CantidadSalida * KX.CostoSalida, 2, MidpointRounding.AwayFromZero);

                                KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                                //if (KX.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                                #endregion
                            }

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var Tanque = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(dk.AlmacenSalidaID)
                                          select tp).ToList();

                            if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            {
                                Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(dk.AlmacenSalidaID));
                                TPto.Cantidad = KX.CantidadFinal;
                            }

                            db.SubmitChanges();

                            #endregion                        
                        
                            #endregion

                            #region <<< Segunda Linea >>>

                            Entidad.Kardex KXs = new Entidad.Kardex();

                            KXs.MovimientoID = M.ID;
                            KXs.ProductoID = Producto.ID;
                            KXs.EsProducto = !Producto.EsServicio;
                            KXs.UnidadMedidaID = dk.UnidadMedidaID;
                            KXs.Fecha = M.FechaRegistro;
                            KXs.EstacionServicioID = IDEstacionServicio;
                            KXs.SubEstacionID = IDSubEstacion;
                            KXs.EsManejo = true;

                            if (IDTipo.Equals(0))
                            {
                                #region Entrada
                                /*
                                KXs.AlmacenEntradaID = dk.AlmacenSalidaID;
                                KXs.CantidadEntrada = dk.CantidadSalida;
                                KXs.CantidadInicial = dk.CantidadInicial;
                                KXs.CostoInicial = dk.CostoSalida;
                                KXs.CostoEntrada = dk.CostoSalida;
                                KXs.CostoFinal = dk.CostoSalida;
                                KXs.CostoTotal = Decimal.Round(KXs.CantidadEntrada * KXs.CostoEntrada, 2, MidpointRounding.AwayFromZero);

                                KXs.CantidadFinal = KXs.CantidadInicial + KXs.CantidadEntrada;
                                */
                                KXs.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KXs.CantidadSalida = dk.CantidadSalida;
                                KXs.CostoInicial = dk.CostoSalida;
                                KXs.CostoSalida = dk.CostoSalida;
                                KXs.CostoFinal = dk.CostoSalida;
                                KXs.CostoTotal = Decimal.Round(KXs.CantidadSalida * KXs.CostoSalida, 2, MidpointRounding.AwayFromZero);

                              
                                //if (KX.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                                #endregion
                            }
                            else
                            {
                                #region Reposicion
                                if (rgManejo.SelectedIndex.Equals(0)) //Reposicion Cliente Manejo a E/S ::: Entrada a Manejo
                                {
                                    KXs.AlmacenEntradaID = dk.AlmacenSalidaID;
                                    KXs.CantidadEntrada = dk.CantidadSalida;
                                    KXs.CostoInicial = dk.CostoSalida;
                                    KXs.CostoEntrada = dk.CostoSalida;
                                    KXs.CostoFinal = dk.CostoSalida;
                                    KXs.CostoTotal = Decimal.Round(KXs.CantidadEntrada * KXs.CostoEntrada, 2, MidpointRounding.AwayFromZero);

                                }
                                else
                                {
                                    KXs.AlmacenSalidaID = dk.AlmacenSalidaID;
                                    KXs.CantidadSalida = dk.CantidadSalida;
                                    KXs.CantidadInicial = dk.CantidadInicial;
                                    KXs.CostoInicial = dk.CostoSalida;
                                    KXs.CostoSalida = dk.CostoSalida;
                                    KXs.CostoFinal = dk.CostoSalida;
                                    KXs.CostoTotal = Decimal.Round(KXs.CantidadSalida * KXs.CostoSalida, 2, MidpointRounding.AwayFromZero);

                                    KXs.CantidadFinal = KXs.CantidadInicial - KXs.CantidadSalida;
                                }
                                

                                //if (KXs.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                                #endregion
                            }

                            db.Kardexes.InsertOnSubmit(KXs);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var Tanqueseg = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(dk.AlmacenSalidaID)
                                          select tp).ToList();

                            if (!Tanqueseg.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            {
                                Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(dk.AlmacenSalidaID));
                                TPto.Cantidad = KXs.CantidadFinal;
                            }

                            db.SubmitChanges();

                            #endregion                        

                            #endregion
                        }

                    #endregion

                    #region <<< REGISTRANDO COMPROBANTE >>>
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

                    #endregion

                    #region ::: REGISTRANDO TANQUE PRESTAMO :::
                    M.AlmacenPrestamos.Add(new Entidad.AlmacenPrestamo { EstacionServicioID = M.EstacionServicioID, SubEstacionID = M.SubEstacionID, Saldo = (IDTipo.Equals(0) ? M.Monto : -Math.Abs(M.Monto)), Litros = (IDTipo.Equals(0) ? M.Litros : -Math.Abs(M.Litros))});
                    db.SubmitChanges();
                    #endregion

                    if (IDTipo.Equals(1))
                    {
                        Entidad.Movimiento MAnterior = db.Movimientos.Single(m => m.ID.Equals(EntidadMov.ID));
                        MAnterior.Finalizado = true;
                        db.SubmitChanges();
                    }

                    //para que actualice los datos del registro
                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    Next = true;
                    IDPrint = M.ID;
                    return true;

                        #endregion

                    }
                    else if (manejo.Equals(1))
                    {
                        #region <<< TIPO Prestamo Manejo de E/S a Cliente Manejo >>>

                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                        int IDTipo = rgTipo.SelectedIndex;

                        Entidad.Movimiento M = new Entidad.Movimiento();

                        //35 = Prestamo Manejo de E/S a Cliente Manejo
                        //37 = Reversión Prestamo Manejo de E/S a Cliente Manejo

                        M.MovimientoTipoID = (IDTipo.Equals(0) ? 35 : 37);
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = Fecha;
                        M.ClienteID = db.Clientes.FirstOrDefault(p => p.ID.Equals(Parametros.Config.ClientePrestamoManejoID())).ID;
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.Monto = Math.Abs(IDTipo.Equals(0) ? Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) : spSaldo.Value);
                        M.MontoMonedaSecundaria = Math.Abs(IDTipo.Equals(0) ? Decimal.Round((DetalleEM.Sum(s => s.CostoTotal) / M.TipoCambio), 2, MidpointRounding.AwayFromZero) : Decimal.Round((spSaldo.Value / M.TipoCambio), 2, MidpointRounding.AwayFromZero));

                        int number = 1;
                        if (IDTipo.Equals(0))
                        {
                            if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)) > 0)
                            {
                                number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                            }
                        }
                        else if (IDTipo.Equals(1))
                        {
                            if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)) > 0)
                            {
                                number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                            }
                        }

                        M.Numero = number;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = memoComentario.Text;
                        M.Litros = Convert.ToDecimal(DetalleEM.Sum(s => s.CantidadSalida));
                        M.MovimientoReferenciaID = (IDTipo.Equals(0) ? 0 : EntidadMov.ID);

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró Prestamo Manejo de E/S a Cliente Manejo: " + M.Numero, this.Name);

                        db.SubmitChanges();

                        var EtProductos = db.Productos.Select(s => new {s.ID, s.Codigo, s.Nombre, s.EsServicio}).Where(o => DetalleEM.Select(s => s.ProductoID).Contains(o.ID));

                        #region ::: REGISTRANDO EN KARDEX DE BD :::

                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        foreach (var dk in DetalleEM)
                        {
                            var Producto = EtProductos.Single(s => s.ID.Equals(dk.ProductoID));
                            if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                            {
                                trans.Rollback();
                                Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                                return false;
                            }

                            #region <<< Primero Linea>>>
                            
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            if (IDTipo.Equals(0))
                            {
                                KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KX.CantidadSalida = dk.CantidadSalida;
                                KX.CantidadInicial = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoSalida;
                                KX.CostoSalida = dk.CostoSalida;
                                KX.CostoFinal = dk.CostoSalida;
                                KX.CostoTotal = Decimal.Round(KX.CantidadSalida * KX.CostoSalida, 2, MidpointRounding.AwayFromZero);

                                if (KX.CantidadSalida > dk.CantidadInicial)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    trans.Rollback();
                                    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                    return false;
                                }

                                KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                            }
                            else
                            {
                                KX.AlmacenEntradaID = dk.AlmacenSalidaID;
                                KX.CantidadEntrada = dk.CantidadSalida;
                                KX.CantidadInicial = dk.CantidadInicial;
                                KX.CostoInicial = dk.CostoSalida;
                                KX.CostoEntrada = dk.CostoSalida;
                                KX.CostoFinal = dk.CostoSalida;
                                KX.CostoTotal = Decimal.Round(KX.CantidadEntrada * KX.CostoEntrada, 2, MidpointRounding.AwayFromZero);

                                KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;

                                //if (KX.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                            }

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var Tanque = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(dk.AlmacenSalidaID)
                                          select tp).ToList();

                            if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            {
                                Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(dk.AlmacenSalidaID));
                                TPto.Cantidad = KX.CantidadFinal;
                            }

                            db.SubmitChanges();

                            #endregion

                            #endregion

                            #region <<< Segundo >>>
                            
                            Entidad.Kardex KXs = new Entidad.Kardex();

                            KXs.MovimientoID = M.ID;
                            KXs.ProductoID = Producto.ID;
                            KXs.EsProducto = !Producto.EsServicio;
                            KXs.UnidadMedidaID = dk.UnidadMedidaID;
                            KXs.Fecha = M.FechaRegistro;
                            KXs.EstacionServicioID = IDEstacionServicio;
                            KXs.SubEstacionID = IDSubEstacion;
                            KXs.EsManejo = true;

                            if (!IDTipo.Equals(0))
                            {
                                KXs.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KXs.CantidadSalida = dk.CantidadSalida;
                                KXs.CantidadInicial = dk.CantidadInicial;
                                KXs.CostoInicial = dk.CostoSalida;
                                KXs.CostoSalida = dk.CostoSalida;
                                KXs.CostoFinal = dk.CostoSalida;
                                KXs.CostoTotal = Decimal.Round(KXs.CantidadSalida * KXs.CostoSalida, 2, MidpointRounding.AwayFromZero);

                                KXs.CantidadFinal = KXs.CantidadInicial - KXs.CantidadSalida;

                                //if (KX.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}

                            }
                            else
                            {
                                KXs.AlmacenEntradaID = dk.AlmacenSalidaID;
                                KXs.CantidadEntrada = dk.CantidadSalida;
                                KXs.CantidadInicial = dk.CantidadInicial;
                                KXs.CostoInicial = dk.CostoSalida;
                                KXs.CostoEntrada = dk.CostoSalida;
                                KXs.CostoFinal = dk.CostoSalida;
                                KXs.CostoTotal = Decimal.Round(KXs.CantidadEntrada * KXs.CostoEntrada, 2, MidpointRounding.AwayFromZero);

                                KXs.CantidadFinal = KXs.CantidadInicial + KXs.CantidadEntrada;

                                //if (KXs.CantidadSalida > dk.CantidadInicial)
                                //{
                                //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                //    trans.Rollback();
                                //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                //    return false;
                                //}
                            }

                            db.Kardexes.InsertOnSubmit(KXs);

                            #endregion
                        }

                        #endregion

                        #region <<< REGISTRANDO COMPROBANTE >>>
                        List<Entidad.ComprobanteContable> Compronbante = PartidasContable.OrderBy(o => o.Linea).ToList();

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

                        #endregion

                        #region ::: REGISTRANDO TANQUE PRESTAMO :::
                        M.AlmacenPrestamos.Add(new Entidad.AlmacenPrestamo { EstacionServicioID = M.EstacionServicioID, SubEstacionID = M.SubEstacionID, Saldo = (IDTipo.Equals(0) ? M.Monto : -Math.Abs(M.Monto)), Litros = (IDTipo.Equals(0) ? M.Litros : -Math.Abs(M.Litros)) });
                        db.SubmitChanges();
                        #endregion

                        if (IDTipo.Equals(1))
                        {
                            Entidad.Movimiento MAnterior = db.Movimientos.Single(m => m.ID.Equals(EntidadMov.ID));
                            MAnterior.Finalizado = true;
                            db.SubmitChanges();
                        }

                        //para que actualice los datos del registro
                        db.SubmitChanges();
                        trans.Commit();

                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        RefreshMDI = true;
                        ShowMsg = true;
                        _Guardado = true;
                        Next = true;
                        IDPrint = M.ID;
                        return true;

                        #endregion

                    }

                    return false;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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
                    int i = 1;

                    if (rgManejo.SelectedIndex.Equals(0))
                    {
                        #region <<< Prestamo de Cliente Manejo a E/S

                        if (rgTipo.SelectedIndex.Equals(0))
                        {
                            #region <<< Salida de Cliente Manejo a E/S
                            List<Int32> ID = new List<Int32>();
                            string texto = "Prestamo Manejo de Cliente Manejo a E/S " + Numero;
                            //Detalle por Combustible
                            DetalleEM.ToList().ForEach(K =>
                        {
                            var producto = db.Productos.Select(s => new { s.CuentaInventarioID, s.ID }).Single(p => p.ID.Equals(K.ProductoID));
                            if (!ID.Contains(producto.CuentaInventarioID))
                            {
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = producto.CuentaInventarioID,
                                    Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                    Fecha = Fecha,
                                    Descripcion = texto,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                ID.Add(producto.CuentaInventarioID);
                                i++;
                            }
                            else
                            {
                                var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                comprobante.MontoMonedaSecundaria += Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                            }
                        });

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaProveedorPrestamoManejoID,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion

                            });
                            i++;
                            #endregion
                        }
                        else
                        {
                            #region /////// DEVOLUCION MANEJO ///////////


                            List<Int32> ID = new List<Int32>();
                            string texto = "Reversión Prestamo Manejo de Cliente Manejo a E/S " + Numero;

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaProveedorPrestamoManejoID,
                                Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(_SaldoProvedorPrestamoManejoMovID), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(_SaldoProvedorPrestamoManejoMovID) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion

                            });
                            i++;

                            //Detalle por Combustible
                            DetalleEM.ToList().ForEach(K =>
                            {
                                var producto = db.Productos.Select(s => new { s.CuentaInventarioID, s.ID }).Single(p => p.ID.Equals(K.ProductoID));
                                if (!ID.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    ID.Add(producto.CuentaInventarioID);
                                    i += 2;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                    comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            });

                            //Diferencia entre Prestamo Manejo (_SaldoClientePrestamoManejoMovID) y reposición (CD.Monto)
                            List<Entidad.ComprobanteContable> NewCD = new List<Entidad.ComprobanteContable>();
                            foreach (Entidad.ComprobanteContable det in CD.Where(o => !o.CuentaContableID.Equals(_CuentaProveedorPrestamoManejoID)))
                            {
                                //obtenemos el saldo anterior del combustible del CD
                                var ent = EntidadMov.ComprobanteContables.Select(s => new { s.CuentaContableID, s.Monto }).FirstOrDefault(o => o.CuentaContableID.Equals(det.CuentaContableID));

                                if (ent != null)
                                {
                                    decimal dif = det.Monto + ent.Monto;
                                    if (!dif.Equals(0))
                                    {
                                        //Positivo Debito, Negativo Credito
                                        dif = dif * -1;
                                        var cuenta = db.Productos.Select(s => new { s.CuentaInventarioID, s.CuentaCostoID }).FirstOrDefault(s => s.CuentaInventarioID.Equals(det.CuentaContableID));

                                        NewCD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = cuenta.CuentaCostoID,
                                            Monto = Decimal.Round(Convert.ToDecimal(dif), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(dif) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Costo en " + texto,
                                            Linea = det.Linea + 1,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });

                                    }
                                }
                            }

                            CD.AddRange(NewCD);

                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region <<< Prestamo de E/S a Cliente Manejo

                        if (rgTipo.SelectedIndex.Equals(0))
                        {
                            #region <<< Salida de E/S a Cliente Manejo

                            List<Int32> ID = new List<Int32>();
                            string texto = "Prestamo Manejo de E/S a Cliente Manejo " + Numero;

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaClientePrestamoManejoID,
                                Monto = Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion

                            });
                            i++;

                            //Detalle por Combustible
                            DetalleEM.ToList().ForEach(K =>
                            {
                                var producto = db.Productos.Select(s => new { s.CuentaInventarioID, s.ID }).Single(p => p.ID.Equals(K.ProductoID));
                                if (!ID.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    ID.Add(producto.CuentaInventarioID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                    comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            #endregion
                        }
                        else
                        {
                            #region /////// DEVOLUCION MANEJO ///////////

                            List<Int32> ID = new List<Int32>();
                            string texto = "Reversión Prestamo Manejo de E/S a Cliente Manejo " + Numero;

                            //Detalle por Combustible
                            DetalleEM.ToList().ForEach(K =>
                            {
                                var producto = db.Productos.Select(s => new { s.CuentaInventarioID, s.ID }).Single(p => p.ID.Equals(K.ProductoID));
                                if (!ID.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    ID.Add(producto.CuentaInventarioID);
                                    i += 2;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                    comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            });

                            //Diferencia entre Prestamo Manejo (_SaldoClientePrestamoManejoMovID) y reposición (CD.Monto)
                            List<Entidad.ComprobanteContable> NewCD = new List<Entidad.ComprobanteContable>();
                            foreach (Entidad.ComprobanteContable det in CD)
                            {
                                //obtenemos el saldo anterior del combustible del CD
                                var ent = EntidadMov.ComprobanteContables.Select(s => new { s.CuentaContableID, s.Monto }).FirstOrDefault(o => o.CuentaContableID.Equals(det.CuentaContableID));

                                if (ent != null)
                                {
                                    decimal dif = det.Monto + ent.Monto;
                                    if (!dif.Equals(0))
                                    {
                                        //Positivo Debito, Negativo Credito
                                        dif = dif * -1;
                                        var cuenta = db.Productos.Select(s => new { s.CuentaInventarioID, s.CuentaCostoID }).FirstOrDefault(s => s.CuentaInventarioID.Equals(det.CuentaContableID));

                                        NewCD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = cuenta.CuentaCostoID,
                                            Monto = Decimal.Round(Convert.ToDecimal(dif), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(dif) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Costo en " + texto,
                                            Linea = det.Linea + 1,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });

                                    }
                                }
                            }

                            CD.AddRange(NewCD);

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaClientePrestamoManejoID,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(_SaldoClientePrestamoManejoMovID), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = texto,
                                Linea = CD.Count + 1,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion

                            });
                            i++;
                            #endregion
                        }
                        #endregion
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
            if (!Guardar(rgManejo.SelectedIndex)) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, Next, RefreshMDI);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (DetalleEM.Count > 0 || txtNumero.Text != "")
                {
                    if (Parametros.General.DialogMsg("El movimiento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        Next = false;
                        e.Cancel = true;
                    }
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            cboAlmacenSalida.DataSource = listaTanque;

            if (DetalleEM.Count > 0)
            {
                rgManejo.Properties.ReadOnly = true;
                rgTipo.Properties.ReadOnly = true;
            }
            else
            {
                rgManejo.Properties.ReadOnly = false;
                rgTipo.Properties.ReadOnly = false;
            }

            //-- Validar Columna de Productos             
            if (view.GetRowCellValue(RowHandle, "ProductoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
                    e.ErrorText = "Debe Seleccionar un Producto";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
                e.ErrorText = "Debe Seleccionar un Producto";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Cantidad
            //--
            if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
            {
                if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadSalida")) <= 0.00)
                {

                    view.SetColumnError(view.Columns["CantidadSalida"], "La Cantidad debe ser mayor a cero");
                    e.ErrorText = "La Cantidad debe ser mayor a cero";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CantidadSalida"], "La Cantidad debe ser mayor a cero");
                e.ErrorText = "La Cantidad debe ser mayor a cero";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Tanque             
            if (view.GetRowCellValue(RowHandle, "AlmacenSalidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Tanque");
                    e.ErrorText = "Debe Seleccionar un Tanque";
                    e.Valid = false;
                    Validate = false;
                }
                else
                {
                    if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).Equals(0))
                    {
                        view.SetColumnError(view.Columns["AlmacenSalidaID"], "El Tanque seleccionado no contiene este producto");
                        e.ErrorText = "El Tanque seleccionado no contiene este producto";
                        e.Valid = false;
                        Validate = false;
                    }
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Tanque");
                e.ErrorText = "Debe Seleccionar un Tanque";
                e.Valid = false;
                Validate = false;
            }

            if (rgTipo.SelectedIndex.Equals(1))
            {
                //-- Validar Existencia             
                if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
                {
                    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")).Equals(0))
                    {
                        if (view.GetRowCellValue(RowHandle, "CantidadInicial") != null)
                        {
                            if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")).Equals(0))
                            {
                                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) > Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")))
                                {
                                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
                                    e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
                                    e.Valid = false;
                                    Validate = false;
                                }
                            }
                            else
                            {
                                view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
                                e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
                                e.Valid = false;
                                Validate = false;
                            }
                        }
                        else
                        {
                            view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
                            e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
                            e.Valid = false;
                            Validate = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
                        e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
                        e.Valid = false;
                        Validate = false;
                    }
                }
                else
                {
                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
                    e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
                    e.Valid = false;
                    Validate = false;
                }
            }

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }
        
        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            

            #region <<< CALCULOS_MONTOS >>>

            //CANTIDADES
           // if (e.Column == colQuantity && rgTipo.SelectedIndex.Equals(1))
            if(e.Column == colQuantity)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                {
                    if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")).Equals(0))
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial") != null)
                        {
                            if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")).Equals(0))
                            {
                                if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) > Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")))
                                {
                                    if (!(rgManejo.SelectedIndex == 0 && rgTipo.SelectedIndex == 0))
                                    {
                                        Parametros.General.DialogMsg("La cantidad a vender sobrepasa la existencia:  " + gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial").ToString(), Parametros.MsgType.warning);
                                        //Parametros.General.DialogMsg("", Parametros.MsgType.error);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //--  Calcular las montos del movimiento
            if (e.Column == colQuantity)
            {
                try
                {
                    decimal vCosto = 0, vCantidad = 0, vTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                        vCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida"));

                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) != null)
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));
                    //else
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);

                    vTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vTotal);

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            #endregion
            
            #region <<< COLUMNA_PRODUCTO >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colProduct)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
                    {
                        return;
                    }
                    else if (DetalleEM.Count(d => d.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                    {
                        Parametros.General.DialogMsg("El producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                        gvDetalle.FocusedColumn = colProduct;
                        return;
                    }
                }

                try
                {            
                    
                    var Producto = db.Productos.Select(s => new { s.ID, s.UnidadMedidaID}).Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                    //-- Unidad Principal     
                    //var Um = Producto.UnidadMedidaID;
                    gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                    //var TCombustible = from t in db.Tanques
                    //                   //join l in listaProductoRD on t.ProductoID equals l.ID
                    //                   where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                    //                   && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                    //                   select new { ID = t.ID, Display = t.Nombre };

                    //Tanques por combustible
                    var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                    ColAlmacenSalida.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                    cboAlmacenSalida.DataSource = TCombustible;//lkAlmacen.Properties.DataSource;
                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", TCombustible.First().ID);

                    if (rgManejo.SelectedIndex.Equals(0))
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Decimal.Round(Convert.ToDecimal(db.SaldoKardexManejo(IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false)), 3, MidpointRounding.AwayFromZero));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Decimal.Round(Convert.ToDecimal(db.SaldoKardex(IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false)), 3, MidpointRounding.AwayFromZero));
                    
                    //if
                    //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardexManejo(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false));
                    //else
                        //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false));
                    
                    //CALCULO COSTO ANTERIOR
                    //decimal VCtoEntrada, vCtoFinal;
                    //Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, Fecha, out vCtoFinal, out VCtoEntrada);

                    decimal vCtoFinal = Decimal.Round(Convert.ToDecimal(db.GetLastCostoPromedio(IDEstacionServicio, IDSubEstacion, Producto.ID, Fecha, 0)), 4, MidpointRounding.AwayFromZero);
                    
                    //-- Cantidad Inicial de 1
                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCtoFinal);
                    
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
                            var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                            cboAlmacenSalida.DataSource = TCombustible;

                            if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).Equals(0))
                            {
                                Parametros.General.DialogMsg("El Tanque seleccionado no contiene este producto", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            #endregion
            }
        }

        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus) && rgTipo.SelectedIndex.Equals(0))
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

            if ((e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus) && rgTipo.SelectedIndex.Equals(0))
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                view.AddNewRow();
            }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove && rgTipo.SelectedIndex.Equals(0))
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

        private void gvDetalle_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!ValidarCampos(true))
            {
                return;
            }

            try
            {
                if (e.Column == ColAlmacenSalida)
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                        {
                            var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                            ColAlmacenSalida.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);
                            cboAlmacenSalida.DataSource = TCombustible;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                bntNew_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        private void bntNew_Click(object sender, EventArgs e)
        {
            Next = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();               
        }
        
        //Sumando los totales al cambiar de filas
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            cboAlmacenSalida.DataSource = listaTanque;
        }

        private void gvDetalle_LostFocus(object sender, EventArgs e)
        {
            cboAlmacenSalida.DataSource = listaTanque;
        }
      
        //Carga Data del Resumen
        private void dateFechaEntrada_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (date)
               // if (!Parametros.General.ValidateKardexMovemente(Fecha, db, IDEstacionServicio, IDSubEstacion, 24, 0))               
                //{
                //    DateTime fecha = Convert.ToDateTime(dateFecha.EditValue);
                //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString(), Parametros.MsgType.warning);
                //    dateFechaCompra.EditValue = null;
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void dateFechaEntrada_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }        

        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (DetalleEM.Count > 0 )
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable Prestamo Manejo";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaVenta_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
        }
        
        private void chkDescuento_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        //Tipo de Prestamo Manejo Entrada / Salida
        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgTipo.SelectedIndex >= 0)
            {
                rgManejo.Properties.ReadOnly = true;
            }

            if (rgTipo.SelectedIndex.Equals(0))
            {            
                lkEntradas.Properties.DataSource = null;
                lkEntradas.EditValue = null;
                EntidadMov = null;
                spSaldo.Value = 0;
                DetalleEM.Clear();
                gvDetalle.OptionsBehavior.Editable = true;
                gvDetalle.RefreshData();
                layoutControlGroupDev.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                if (rgManejo.SelectedIndex.Equals(0))
                {
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(31)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(31)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
                else if (rgManejo.SelectedIndex.Equals(1))
                {
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(35)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
            }
            else if (rgTipo.SelectedIndex.Equals(1))
            {
                
                layoutControlGroupDev.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                gvDetalle.OptionsBehavior.Editable = false;
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                                
                if (rgManejo.SelectedIndex.Equals(0))
                {
                    lkEntradas.Properties.DataSource = dv.VistaMovimientos.Where(m => m.MovimientoTipoID.Equals(31) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && !m.Finalizado && !m.Anulado).Select(s => new { ID = s.ID, Display = String.Format("{0:000000000} | {1}", s.Numero, s.ClienteNombre) });
                    
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(33)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(33)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
                else if (rgManejo.SelectedIndex.Equals(1))
                {
                    lkEntradas.Properties.DataSource = dv.VistaMovimientos.Where(m => m.MovimientoTipoID.Equals(35) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && !m.Finalizado && !m.Anulado).Select(s => new { ID = s.ID, Display = String.Format("{0:000000000} | {1}", s.Numero, s.ClienteNombre) });

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(37)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(37)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
            }
        }

        private void lkEntradas_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                #region <Pruebas>
              //  System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
              //  System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
              //  adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
              //  System.Data.DataTable dt = new System.Data.DataTable("sd");
           //---db.SetRecosteoByProducto(vP.ID, IDEstacionServicio, IDSubEstacion, vLastAjuste.FechaRegistro.AddDays(1).Date, Fecha, UsuarioID);
              //  string sqlsaldo = "SaldoKardex";
              //  //  ojo el SqlData Adapter modifica directamente el data source
              ////  SqlDataAdapter leer;
              //  //El sql DataReader solo lee
              //  SqlCommand saldokardex;
              //  string con = Parametros.Config.GetCadenaConexionString();
              //  saldokardex = new SqlCommand(sqlsaldo, con);
              //  SqlDataReader leersaldokardex;
              // // db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               // saldokardex = new SqlCommand("{}", db); 
# endregion
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                if (lkEntradas.EditValue != null)
                {
                    EntidadMov = null;
                    EntidadMov = db.Movimientos.Single(m => m.ID.Equals(Convert.ToInt32(lkEntradas.EditValue)));
                    spSaldo.Value = EntidadMov.Monto;

                    DetalleEM.Clear();
                    db.Kardexes.Where(k => k.MovimientoID.Equals(EntidadMov.ID) && !k.EsManejo).ToList().ForEach(obj =>
                        {
                            decimal costo = Decimal.Round(Convert.ToDecimal(db.GetLastCostoPromedio(IDEstacionServicio, IDSubEstacion, obj.ProductoID, Fecha, 0)), 4, MidpointRounding.AwayFromZero);
                            DetalleEM.Add(new Entidad.Kardex
                            {
                                ProductoID = obj.ProductoID,
                                UnidadMedidaID = obj.UnidadMedidaID,
                                AlmacenSalidaID = (rgManejo.SelectedIndex.Equals(0) ? obj.AlmacenEntradaID : obj.AlmacenSalidaID),
                                CantidadSalida = (rgManejo.SelectedIndex.Equals(0) ? obj.CantidadEntrada : obj.CantidadSalida),
                                //CostoSalida = (rgManejo.SelectedIndex.Equals(0) ? obj.CostoEntrada : obj.CostoSalida),
                                CostoSalida = costo,
                                CantidadInicial = Decimal.Round(Convert.ToDecimal(db.SaldoKardex(IDEstacionServicio, IDSubEstacion, obj.ProductoID, (rgManejo.SelectedIndex.Equals(0) ? obj.AlmacenEntradaID : obj.AlmacenSalidaID), Fecha, false)), 2, MidpointRounding.AwayFromZero),
                                //CantidadInicial = Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, obj.ProductoID, obj.AlmacenEntradaID, Fecha, false),
                                CostoTotal = Decimal.Round(costo *  (rgManejo.SelectedIndex.Equals(0) ? obj.CantidadEntrada : obj.CantidadSalida), 2, MidpointRounding.AwayFromZero)
                            });
                        });
                    gvDetalle.RefreshData();

                    rgManejo.Properties.ReadOnly = true;
                    rgTipo.Properties.ReadOnly = true;

                    if (rgManejo.SelectedIndex.Equals(0))
                    {
                        var vSaldo = EntidadMov.ComprobanteContables.Select(s => new { s.CuentaContableID, s.Monto }).FirstOrDefault(o => o.CuentaContableID.Equals(_CuentaProveedorPrestamoManejoID));
                        _SaldoProvedorPrestamoManejoMovID = (vSaldo != null ? vSaldo.Monto : 0m);                    
                    }
                    else
                    {
                        var vSaldo = EntidadMov.ComprobanteContables.Select(s => new { s.CuentaContableID, s.Monto }).FirstOrDefault(o => o.CuentaContableID.Equals(_CuentaClientePrestamoManejoID));
                        _SaldoClientePrestamoManejoMovID = (vSaldo != null ? vSaldo.Monto : 0m);
                    }
                }
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }        

        private void rgManejo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgManejo.SelectedIndex >= 0)
            {
                if (rgManejo.SelectedIndex.Equals(0))
                    rgTipo.Properties.Items[0].Description = "Entrada Kardex";
                else
                    rgTipo.Properties.Items[0].Description = "Salida Kardex";

                rgTipo.Properties.ReadOnly = false;
                
            }
        }

        #endregion

    }
}