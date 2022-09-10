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
    public partial class DialogDevolucionConversion : Form
    {
        #region *** DECLARACIONES ***

        internal Forms.FormDevolucionConversion MDI;
        private Entidad.SAGASDataClassesDataContext db;
        private static Entidad.Proveedor Provee;
        private static Entidad.Proveedor ProveeReferencia;
        private int UsuarioID;
        private int IDPrint = 0;
        internal bool CalculandoDevolucion = false;
        internal bool CalculandoConversion = false;
        private bool RefreshMDI = false;
        private bool ShowMsg = false;
        private bool _Guardado = false;
        private bool _Next = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private IQueryable<Parametros.ListIdDisplay> listaAlmacen;
        private Parametros.TiposDevConvProveedor _Tipo;
        private int _CuentaIVACredito = Parametros.Config.IVAPorAcreditar();
        private int _CuentaIVAContado = Parametros.Config.IVAAcreditado();
        private decimal _margenIVA, oldIVA;

        private int IDProveedor
        {
            get { return Convert.ToInt32(glkProvee.EditValue); }
            set { glkProvee.EditValue = value; }
        }

        private int IDFactura
        {
            get { return Convert.ToInt32(GlkFactura.EditValue); }
            set { GlkFactura.EditValue = value; }
        }

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
        }
        
        private DateTime FechaOrden
        {
            get { return Convert.ToDateTime(dateFechaOrden.EditValue); }
            set { dateFechaOrden.EditValue = value; }
        }

        private List<Entidad.Kardex> K = new List<Entidad.Kardex>();
        
        public List<Entidad.Kardex> DetalleK
        {
            get { return K; }
            set
            {
                K = value;
                this.bdsDetalle.DataSource = this.K;
            }
        }

        private List<Entidad.Kardex> KC = new List<Entidad.Kardex>();
        
        public List<Entidad.Kardex> DetalleKC
        {
            get { return KC; }
            set
            {
                KC = value;
                this.bdsDetalleC.DataSource = this.KC;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogDevolucionConversion(int UserID)
        {            
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {

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
                _margenIVA = Parametros.Config.MargenValorCambioIVA();


                txtNumero.Text = "000000000";

                //---LLenar Almacenes ---//
                listaAlmacen = from al in db.Almacens
                               where al.Activo && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                               select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };

                //Almacen GRID
                cboAlmacenEntrada.DataSource = listaAlmacen.ToList();
                lkAlmacenC.DataSource = listaAlmacen.ToList();

                //---UNIDADES DE MEDIDA
                var unidades = from U in db.UnidadMedidas
                               select new { U.ID, U.Nombre };
                cboUnidadMedida.DataSource = unidades;
                cboUnidadMedidaC.DataSource = unidades;
                //-------------------------------//

                     glkProvee.Properties.DataSource = from p in db.Proveedors
                                                      join m in db.Movimientos on p.ID equals m.ProveedorID
                                                      where p.Activo && !m.Anulado && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                                      group p by new { p.ID, p.Codigo, p.Nombre } into gr
                                                      select new
                                                      {
                                                          ID = gr.Key.ID,
                                                          Codigo = gr.Key.Codigo,
                                                          Nombre = gr.Key.Nombre,
                                                          Display = gr.Key.Codigo + " " + gr.Key.Nombre
                                                      };

               
                
                dateFechaOrden.EditValue = Convert.ToDateTime(db.GetDateServer());

                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaOrden.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaOrden.Date)).First().Valor : 0m);

                this.bdsDetalle.DataSource = this.DetalleK;
                this.bdsDetalleC.DataSource = this.DetalleKC;

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
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && !i.Anulado && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos()
        {
            if (txtNumero.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaOrden, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaOrden, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El periodo contable ya esta cerrado", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkProvee.EditValue) <= 0 && _Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
            {
                Parametros.General.DialogMsg("Debe seleccionar un Proveedor.", Parametros.MsgType.warning);
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

            if (DetalleK.Count <= 0)
            {
                Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }
            else
            {
                if (DetalleK.Count(k => k.ProductoID <= 0 || k.UnidadMedidaID <= 0 || k.CantidadSalida <= 0 || k.AlmacenSalidaID <= 0) > 0)
                {
                    Parametros.General.DialogMsg("Favor Revisar el detalle de la Conversión." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                List<int> Areas = (from p in db.Productos
                                   join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                   where DetalleK.Select(s => s.ProductoID).Contains(p.ID)
                                   group pc by pc.AreaID into gr
                                   select gr
                                       ).Select(s => s.Key).ToList();

                bool Bloqueo = false;
                
                foreach (int obj in Areas)
                {
                    if (!Parametros.General.ValidateKardexMovemente(FechaOrden, db, IDEstacionServicio, IDSubEstacion, 9, obj))
                    {
                        Bloqueo = true;
                        break;
                    }
                }

                if (Bloqueo)
                {
                    DateTime fecha = Convert.ToDateTime(dateFechaOrden.EditValue);
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString() + " para los productos listados.", Parametros.MsgType.warning);
                    return false;
                }

            }

            if (rgOptions.SelectedIndex.Equals(1))
            {
                if (DetalleK.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento en la lista de conversiones." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!DetalleK.Sum(s => s.CostoTotal).Equals(DetalleKC.Sum(s => s.CostoTotal)))
                {
                    Parametros.General.DialogMsg("El Costo Total de la conversión no coincide." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
            }

            if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
            {
                Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                return false;
            }

            if (_Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
            {
                var vFact = db.Movimientos.Where(m => m.ID.Equals(IDFactura)).FirstOrDefault();

                if (vFact != null)
                {
                    if (spMonto.Value > (vFact.Monto - vFact.Abono))
                    {
                        Parametros.General.DialogMsg("El Monto de la DEVOLUCIÓN no puede ser mayor al monto de la factura.", Parametros.MsgType.warning);
                        return false;
                    }
                }
            }


            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos())
                return false;

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    bool IsConv = (rgOptions.SelectedIndex.Equals(0) ? false : true);

                    //Entidad.Movimiento RM = new Entidad.Movimiento();
                    Entidad.Movimiento CM = new Entidad.Movimiento();
                    Entidad.Movimiento M = new Entidad.Movimiento();

                    int number = 1;
                    if (IsConv) //CONVERSION
                    {
                        #region <<< REGISTRANDO MOVIMIENTO ENTRADA >>>

                        CM.MovimientoTipoID = 13;
                        CM.UsuarioID = UsuarioID;
                        CM.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        CM.FechaRegistro = FechaOrden;
                        CM.Monto = Convert.ToDecimal(spMonto.Value);
                        CM.MonedaID = Parametros.Config.MonedaPrincipal();
                        CM.TipoCambio = _TipoCambio;
                        CM.MontoMonedaSecundaria = Decimal.Round(spMonto.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero);

                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(13)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(13)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                        CM.Numero = number;
                        CM.EstacionServicioID = IDEstacionServicio;
                        CM.SubEstacionID = IDSubEstacion;
                        CM.Comentario = memoComentario.Text;

                        if (Provee != null)
                        {
                            CM.ProveedorID = Provee.ID;
                            CM.ProveedorReferenciaID = Provee.ID;
                            CM.IVA = DetalleK.Sum(s => s.ImpuestoTotal);
                            CM.SubTotal = DetalleK.Sum(s => s.CostoTotal - s.ImpuestoTotal);
                        }

                        db.Movimientos.InsertOnSubmit(CM);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el movimiento: " + CM.Numero, this.Name);

                        db.SubmitChanges();
                        
                        #endregion

                        #region <<< REGISTRANDO MOVIMIENTO SALIDA >>>
                    
                        //RM.MovimientoTipoID = 80;
                        //RM.UsuarioID = UsuarioID;
                        //RM.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        //RM.FechaRegistro = FechaOrden;
                        //RM.Monto = Convert.ToDecimal(spMonto.Value);
                        //RM.MonedaID = Parametros.Config.MonedaPrincipal();
                        //RM.TipoCambio = _TipoCambio;
                        //RM.MontoMonedaSecundaria = Decimal.Round(spMonto.Value / _TipoCambio, 2, MidpointRounding.AwayFromZero); 

                        //if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(80)) > 0)
                        //{
                        //    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(80)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        //}
                        //RM.Numero = Convert.ToInt32(Numero);
                        //RM.EstacionServicioID = IDEstacionServicio;
                        //RM.SubEstacionID = IDSubEstacion;
                        //RM.Comentario = memoComentario.Text;
                        //RM.MovimientoReferenciaID = CM.ID;
                        //RM.ProveedorID = (Provee == null ? 0 : Provee.ID);
                        //RM.ProveedorReferenciaID = (Provee == null ? 0 : Provee.ID);

                        //db.Movimientos.InsertOnSubmit(RM);
                        //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        //"Se registró el movimiento: " + RM.Numero, this.Name);

                        //CM.MovimientoReferenciaID = RM.ID;

                        //db.SubmitChanges();
                        
                        #endregion

                        #region ::: REGISTRANDO EN KARDEX DEV (ENTRADA):::
                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        decimal vIva = 0;
                        foreach (var dk in DetalleK)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = CM.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = CM.FechaRegistro;
                            KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                            KX.CantidadSalida = dk.CantidadSalida;
                            KX.CostoSalida = dk.CostoSalida;
                            KX.CostoFinal = dk.CostoSalida;
                            KX.CostoTotal = dk.CostoTotal - dk.ImpuestoTotal;

                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;
                            
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;
                            
                            #region //
                            //if (ProveeReferencia == null)
                            //{
                            //    if (Provee.AplicaIVA)
                            //    {
                            //        if (!Producto.ExentoIVA)
                            //            KX.ImpuestoTotal = vIva = Decimal.Round((KX.CostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                            //    }
                            //}
                            //else
                            //{
                            //    if (!Producto.ExentoIVA)
                            //        KX.ImpuestoTotal = vIva = Decimal.Round((KX.CostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                            //}
                            #endregion

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            //var AL = (from al in db.AlmacenProductos
                            //          where al.ProductoID.Equals(Producto.ID)
                            //            && al.AlmacenID.Equals(KX.AlmacenSalidaID)
                            //          select al).ToList();

                            //if (!AL.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            //{
                            //    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(KX.AlmacenSalidaID));
                            //    AP.Cantidad = AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - KX.CantidadSalida;
                            //    db.SubmitChanges();
                            //}
                            //------------------------------------------------------------------------//

                            #endregion

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                        }

                        #endregion

                        #region ::: REGISTRANDO EN KARDEX CONV :::

                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        foreach (var dk in DetalleKC)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = CM.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = CM.FechaRegistro;
                            KX.CantidadInicial = dk.CantidadInicial;
                            KX.CostoInicial = dk.CostoEntrada;
                            KX.CostoEntrada = dk.CostoEntrada;
                            KX.AlmacenEntradaID = dk.AlmacenEntradaID;
                            KX.CantidadEntrada = dk.CantidadEntrada;
                            KX.CostoTotal = Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero);

                            //var ALP = (from al in db.AlmacenProductos
                            //           where al.ProductoID.Equals(Producto.ID)
                            //             && al.AlmacenID.Equals(dk.AlmacenEntradaID)
                            //           select al).ToList();

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                            //if (ALP.Count() == 0)
                            //{
                            //    KX.CantidadInicial = 0;
                            //}
                            //else
                            //    KX.CantidadInicial = ALP.Single(q => q.ProductoID.Equals(dk.ProductoID)).Cantidad;

                            //var KCI = (from k in db.Kardexes
                            //           join m in db.Movimientos on k.MovimientoID equals m.ID
                            //           where k.ProductoID.Equals(Producto.ID) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
                            //             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                            //           select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO COSTO INICIAL
                            //if (KCI.Count() == 0)
                            //{
                            //    KX.CostoInicial = 0;
                            //}
                            //else
                            //    KX.CostoInicial = KCI.First(q => q.ProductoID.Equals(dk.ProductoID)).CostoFinal;

                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                            KX.CostoEntrada = Decimal.Round(dk.CostoEntrada, 4, MidpointRounding.AwayFromZero);

                            //decimal vCostosIniciales = Decimal.Round(Convert.ToDecimal(KX.CantidadInicial * KX.CostoInicial), 4, MidpointRounding.AwayFromZero);
                            KX.CostoFinal = KX.CostoEntrada;//Decimal.Round(Convert.ToDecimal((vCostosIniciales + KX.CostoTotal) / KX.CantidadFinal), 4, MidpointRounding.AwayFromZero);


                            #region //
                            //}
                            //else if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            //{
                            //    KX.AlmacenEntradaID = dk.AlmacenEntradaID;
                            //    KX.CantidadEntrada = dk.CantidadEntrada;
                            //    KX.CostoEntrada = dk.CostoEntrada;
                            //    //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            //    KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;
                            //    KX.CostoTotal = KX.CostoEntrada * KX.CantidadSalida;
                            //}
                            #endregion


                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var AL = (from al in db.AlmacenProductos
                                      where al.ProductoID.Equals(Producto.ID)
                                        && al.AlmacenID.Equals(KX.AlmacenSalidaID)
                                      select al).ToList();

                            if (!AL.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            {
                                Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(KX.AlmacenSalidaID));
                                AP.Cantidad = AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - dk.CantidadEntrada;
                                decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                AP.Costo = KX.CostoFinal;
                                AP.PrecioSugerido = precio;
                            }


                            #endregion


                            db.SubmitChanges();

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                        }

                        #endregion
}
                    else //DEVOLUCION
                    {
                        #region <<< REGISTRANDO MOVIMIENTO >>>
                    
                        M.MovimientoTipoID = 12;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaOrden;
                        M.Monto = Convert.ToDecimal(spMonto.Value);
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.MontoMonedaSecundaria = Decimal.Round(spMonto.Value / M.TipoCambio, 2, MidpointRounding.AwayFromZero);

                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(12)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(12)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                        M.Numero = number;
                        M.Referencia = txtReferencia.Text;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = memoComentario.Text;

                        if (Provee != null)
                        {
                            M.ProveedorID = Provee.ID;
                            M.ProveedorReferenciaID = Provee.ID;
                            M.IVA = DetalleK.Sum(s => s.ImpuestoTotal);
                            M.SubTotal = DetalleK.Sum(s => s.CostoTotal - s.ImpuestoTotal);
                            //Actualizando referencias si se ha seleccionado al menos una factura.
                            if (IDFactura > 0)
                            {
                                var MAnt = db.Movimientos.Single(mv => mv.ID.Equals(IDFactura));
                                M.Referencia = MAnt.Referencia;
                                M.ProveedorReferenciaID = MAnt.ProveedorID;
                                M.MovimientoReferenciaID = MAnt.ID;
                            }
                        }

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el movimiento: " + M.Numero, this.Name);

                        db.SubmitChanges();

                        #endregion

                        #region ::: REGISTRANDO EN KARDEX DEV :::
                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        decimal vIva = 0;
                        foreach (var dk in DetalleK)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.CantidadInicial = dk.CantidadInicial;

                            KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                            KX.CantidadSalida = dk.CantidadSalida;
                            KX.CostoSalida = dk.CostoSalida;
                            KX.CostoFinal = dk.CostoSalida;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;
                            KX.CostoTotal = dk.CostoTotal - dk.ImpuestoTotal;

                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                            if (KX.CantidadSalida > KX.CantidadInicial)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                return false;
                            }


                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            //if (ProveeReferencia == null)
                            //{
                            //    if (Provee.AplicaIVA)
                            //    {
                            //        if (!Producto.ExentoIVA)
                            //            KX.ImpuestoTotal = vIva = Decimal.Round((KX.CostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                            //    }
                            //}
                            //else
                            //{
                            //    if (!Producto.ExentoIVA)
                            //        KX.ImpuestoTotal = vIva = Decimal.Round((KX.CostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                            //}

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            //var AL = (from al in db.AlmacenProductos
                            //          where al.ProductoID.Equals(Producto.ID)
                            //            && al.AlmacenID.Equals(KX.AlmacenSalidaID)
                            //          select al).ToList();

                            //if (!AL.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            //{
                            //    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(KX.AlmacenSalidaID));
                            //    AP.Cantidad = AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - KX.CantidadSalida;
                            //    db.SubmitChanges();
                            //}
                            //------------------------------------------------------------------------//

                            #endregion

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                        }

                        #endregion

                        #region ::: REGISTRANDO DEUDOR :::

                        if (Provee != null)
                        {


                            Entidad.Deudor D = new Entidad.Deudor();
                            D.ProveedorID = Provee.ID;
                            D.MovimientoID = M.ID;
                            D.Valor = M.Monto;
                            db.Deudors.InsertOnSubmit(D);
                            db.SubmitChanges();
                            if (_Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
                            {
                                if (IDFactura > 0)
                                {
                                    Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(IDFactura));
                                    MLine.Abono += M.Monto;
                                    MLine.Pagado = (MLine.Abono >= MLine.Monto ? true : false);

                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = MLine.ID, Monto = M.Monto, PagoIVA = false });
                                }
                                else
                                {
                                    Parametros.General.DialogMsg("Debe seleccionar la factura.", Parametros.MsgType.warning);
                                    return false;
                                }
                                db.SubmitChanges();
                            }

                        }

                        #endregion

                        #region <<< REGISTRANDO COMPROBANTE >>>
                        if (!IsConv)
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
                    
                    }
                    
                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    IDPrint = IsConv ? CM.ID : M.ID;
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
                    Entidad.Movimiento M;
                    if (rgOptions.SelectedIndex.Equals(0) && Provee != null )
                    {
                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                            string texto = "Devolución Proveedor " + Provee.Nombre;
                            int i = 1;

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Provee.CuentaContableID,
                                Monto = Decimal.Round(Convert.ToDecimal(spMonto.Value), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spMonto.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = FechaOrden,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;

                            List<Int32> IDInventario = new List<Int32>();

                            #region <<< DETALLE_COMPROBANTE >>>
                            DetalleK.ToList().ForEach(K =>
                            {
                                var area = from a in db.Areas
                                           join pc in db.ProductoClases on a.ID equals pc.AreaID
                                           join p in db.Productos on pc.ID equals p.ProductoClaseID
                                           where p.ID.Equals(K.ProductoID)
                                           select a;

                                #region <<< CUENTA_INVENTARIO >>>
                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));

                                    if (ProveeReferencia == null)
                                    {
                                        if (Provee.AplicaIVA)
                                        {
                                            if (!producto.ExentoIVA)
                                            {                                                
                                                i++;
                                                CD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = _CuentaIVACredito,
                                                    Monto = -Math.Abs(Decimal.Round((decimal)K.ImpuestoTotal, 2, MidpointRounding.AwayFromZero)),
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round((decimal)K.ImpuestoTotal / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                    Fecha = FechaOrden,
                                                    Descripcion = Provee.Nombre + " Acreditación IVA " + texto,
                                                    Linea = i,
                                                    CentroCostoID = 0,
                                                    EstacionServicioID = IDEstacionServicio,
                                                    SubEstacionID = IDSubEstacion
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!producto.ExentoIVA)
                                        {                                            
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = _CuentaIVAContado,
                                                Monto = -Math.Abs(Decimal.Round((decimal)K.ImpuestoTotal, 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round((decimal)K.ImpuestoTotal / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                Fecha = FechaOrden,
                                                Descripcion = Provee.Nombre + " Acreditación IVA " + texto,
                                                Linea = i,
                                                CentroCostoID = 0,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }

                                    if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaInventarioID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaOrden,
                                            Descripcion = texto,
                                            Linea = i,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
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
                                    var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));

                                    if (ProveeReferencia == null)
                                    {
                                        if (Provee.AplicaIVA)
                                        {
                                            if (!producto.ExentoIVA)
                                            {
                                                i++;
                                                CD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = _CuentaIVACredito,
                                                    Monto = -Math.Abs(Decimal.Round(K.ImpuestoTotal, 2, MidpointRounding.AwayFromZero)),
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(K.ImpuestoTotal / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                    Fecha = FechaOrden,
                                                    Descripcion = Provee.Nombre + " Acreditación IVA " + texto,
                                                    Linea = i,
                                                    CentroCostoID = 0,
                                                    EstacionServicioID = IDEstacionServicio,
                                                    SubEstacionID = IDSubEstacion
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!producto.ExentoIVA)
                                        {
                                            i++;
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = _CuentaIVAContado,
                                                Monto = -Math.Abs(Decimal.Round(K.ImpuestoTotal, 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(K.ImpuestoTotal / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                                Fecha = FechaOrden,
                                                Descripcion = Provee.Nombre + " Acreditación IVA " + texto,
                                                Linea = i,
                                                CentroCostoID = 0,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                        }
                                    }

                                    if (!IDInventario.Contains(producto.CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaInventarioID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaOrden,
                                            Descripcion = texto,
                                            Linea = i,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDInventario.Add(producto.CuentaInventarioID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                        comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }
                                }

                                #endregion

                            });

                            #endregion

                        return CD;
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe llenar los campos requeridos.", Parametros.MsgType.warning);
                        return null;
                    }
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
            MDI.CleanDialog(ShowMsg, IDPrint, RefreshMDI, _Next);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (!_Next)
                {
                    if (DetalleK.Count > 0 || Convert.ToInt32(glkProvee.EditValue) <= 0)
                    {
                        if (Parametros.General.DialogMsg("El movimiento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }

            //EntidadAnterior = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        #region <<< GRID_TOP >>>
        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

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

            //-- Validar Columna de UnidadMedida             
            if (view.GetRowCellValue(RowHandle, "UnidadMedidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "UnidadMedidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["UnidadMedidaID"], "Debe Seleccionar una Unidad Medida");
                    e.ErrorText = "Debe Seleccionar una Unidad Medida";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["UnidadMedidaID"], "Debe Seleccionar una Unidad Medida");
                e.ErrorText = "Debe Seleccionar una Unidad Medida";
                e.Valid = false;
                Validate = false;
            }
            
            //-- Validar Columna de Cantidad
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

            //-- Validar Columna de AlmacenSalida             
            if (view.GetRowCellValue(RowHandle, "AlmacenSalidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen");
                    e.ErrorText = "Debe Seleccionar un Almacen";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen");
                e.ErrorText = "Debe Seleccionar un Almacen";
                e.Valid = false;
                Validate = false;
            }

            spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal + s.ImpuestoTotal));

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

            #region <<< CALCULOS_MONTOS ::: Cantidad-CostoTotal-IVA>>>
            //--  Calcular las montos de las columnas de Impuestos y Subtotal
            if (e.Column == colQuantity || e.Column == colCostoTotal || e.Column == colIva)
            {
                try
                {
                    if (CalculandoDevolucion)
                        return;

                    if (Provee == null && _Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
                    {
                        Parametros.General.DialogMsg("Debe selccionar un Proveedor.", Parametros.MsgType.warning);
                        return;
                    }

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") == null)
                    {
                        Parametros.General.DialogMsg("Debe selccionar un Producto.", Parametros.MsgType.warning);
                        return;
                    }
                    else
                    {
                        if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                        {
                            Parametros.General.DialogMsg("Debe selccionar un Producto.", Parametros.MsgType.warning);
                            return;
                        }
                    }
                    
                    var P = db.Productos.Select(s => new { s.ID, s.ExentoIVA }).Single(o => o.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))));

                    string col = e.Column.Name;
                    decimal vCosto = 0, vCantidad = 0, vCostoTotal = 0, vIVA = 0, vExistencia = 0, vnewIVA, vTotal = 0;
                    CalculandoDevolucion = true;

                    //Calculo de la cantidad
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", vCantidad);

                    //Calculo del IVA
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal") != null)
                        vIVA = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vIVA);
                    
                    //Calcular valores de acuerdo a la columna editada
                    switch (col)
                    {
                        case "colQuantity":
                            {
                                #region ::: Actualizando columnas al editar la cantidad :::
                                //Calculo del costo Unitario
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                                    vCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida"));

                                if (Provee != null)
                                {
                                    if (Provee.AplicaIVA)
                                    {
                                        if (!P.ExentoIVA && _Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
                                        {
                                            //Calculo IVA
                                            vIVA = Decimal.Round((vCosto * vCantidad) * 0.15m, 2, MidpointRounding.AwayFromZero);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vIVA);
                                        }
                                        else
                                            gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", 0m);
                                    }
                                }

                                //Calculo Costo Total y Total
                                vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);
                                vTotal = Decimal.Round((vCostoTotal + vIVA), 2, MidpointRounding.AwayFromZero);
                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Total", vTotal);
                                break;
                                #endregion
                            }
                        case "colIva": 
                            {
                                #region ::: Actualizando columnas al editar el IVA :::
                                //Calculo del costo Unitario
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                                    vCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida"));

                                if (Provee.AplicaIVA)
                                {
                                    if (!P.ExentoIVA)
                                    {
                                        //Calculo IVA
                                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal") != null)
                                        {
                                            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) > 0m)
                                            {
                                                vnewIVA = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal"));
                                                if (Math.Abs(vnewIVA - oldIVA) > _margenIVA)
                                                {
                                                    Parametros.General.DialogMsg("El cambio en el IVA no puede ser mayor al margen permitido: " + _margenIVA.ToString("#,0.00") + Environment.NewLine, Parametros.MsgType.warning);
                                                    gvDetalle.SetRowCellValue(e.RowHandle, colIva, oldIVA);
                                                }
                                                //else
                                                //{
                                                //    vIVA = vnewIVA;
                                                //}
                                            }
                                            else
                                            {
                                                Parametros.General.DialogMsg("No insista en ingresar IVA sin haber digitado primero la cantidad.\nEl sistema remitirá un informe especial sobre su desempeño a la alta gerencia.\nMuchas gracias", Parametros.MsgType.warning);
                                                gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", 0m);
                                                CalculandoDevolucion = false;
                                                return;
                                            }
                                        }
                                    }
                                    else
                                        gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", 0m);
                                }

                                //Calculo Costo Total
                                vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);
                                //Cálculo Total
                                vTotal = Decimal.Round((vCostoTotal + vIVA), 2, MidpointRounding.AwayFromZero);
                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Total", vTotal);
                                break;
                                #endregion
                            }
                        case "colCostoTotal":
                            {
                                #region ::: Actualizando columnas al editar el costo total :::
                                //Calculo Costo Total
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                                    vCostoTotal = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal"));

                                //Validación del IVA
                                if (Provee.AplicaIVA)
                                {
                                    if (!P.ExentoIVA && _Tipo.Equals(Parametros.TiposDevConvProveedor.Devolucion))
                                    {
                                        //Calculo IVA
                                        vIVA = Decimal.Round(vCostoTotal * 0.15m, 2, MidpointRounding.AwayFromZero);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vIVA);
                                    }
                                    else
                                        gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", 0m);
                                }

                                //Calculo del costo Unitario
                                if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) > 0m)
                                {
                                    vCosto = Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero);
                                    vTotal = Decimal.Round((vCostoTotal + vIVA), 2, MidpointRounding.AwayFromZero);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCosto);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Total", vTotal);
                                }
                                else
                                {
                                    Parametros.General.DialogMsg("No insista en ingresar Costo total sin haber digitado primero la cantidad.\nEl sistema remitirá un informe especial sobre su desempeño a la alta gerencia.\nMuchas gracias", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", 0m);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", 0m);
                                    CalculandoDevolucion = false;
                                    return;
                                }
                                break;
                                #endregion
                            }
                        default:
                            break;
                    }

                    //if (e.Column != colCostoTotal)
                    //{
                    //    //Calculo IVA
                    //    if (gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal") != null)
                    //        vIVA = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal"));

                    //    //Calculo del costo Unitario
                    //    if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                    //        vCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida"));
                    //}
                    //else
                    //{
                    //    //Calculo IVA
                    //    if (gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal") != null)
                    //        vIVA = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "ImpuestoTotal"));

                    //    //Calculo Costo Total
                    //    vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);
                    //}



                    //Obtener la existencia
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial") != null)
                        vExistencia = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial"));

                    //Validar que la cantidad a salir no sea mayor que la existencia
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                        {
                            if (vCantidad > vExistencia)
                            {
                                Parametros.General.DialogMsg("La cantidad a salir sobrepasa la existencia", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                            }
                        }
                    }
                    CalculandoDevolucion = false;
                    spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));

                }
                catch (Exception ex)
                {
                    CalculandoDevolucion = false;
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
                    else if (DetalleK.Count(d => d.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                    {
                        Parametros.General.DialogMsg("El producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                        gvDetalle.FocusedColumn = colProduct;
                        return;
                    }
                }

                try
                {
                    var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                    //-- Unidad Principal     
                    //var Um = Producto.UnidadMedidaID;
                    gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                    cboAlmacenSalida.DataSource = listaAlmacen.ToList();
                    //if (gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") == null)
                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);//listaAlmacen.First().ID);

                    //decimal VCtoEntrada, vCtoFinal;
                    //Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);

                    //gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", Decimal.Round(VCtoEntrada, 4, MidpointRounding.AwayFromZero));
                    //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaOrden, false));

                    //if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) < 0)
                    //{
                    //    Parametros.General.DialogMsg("El producto seleccionado tiene existencia invalida.", Parametros.MsgType.warning);
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                    //    gvDetalle.FocusedColumn = colProduct;
                    //    return;
                    //}
                                        
                    //-- Cantidad Inicial de 1
                    //if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) == Convert.ToDecimal("0.00"))

                    //-- Cantidad Inicial de 0
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);

                    spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            //--
            #endregion

            #region <<< COLUMNA_ALMACEN >>>
            if (e.Column == ColAlmacenSalida)
            {
                try
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                        {
                            if (gvDetalle.FocusedColumn.Name.Equals(ColAlmacenSalida.Name))
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") != null)//
                                {
                                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")) > 0)
                                    {
                                        int Producto = Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"));

                                        decimal VCtoEntrada = 0, vCtoFinal = 0;

                                        if (IDFactura > 0)
                                        {
                                            var kProd = db.Kardexes.Where(k => k.MovimientoID.Equals(IDFactura) && k.ProductoID.Equals(Producto)).FirstOrDefault();

                                            if (kProd != null)
                                                VCtoEntrada = Decimal.Round(kProd.CostoEntrada, 4, MidpointRounding.AwayFromZero);
                                            else
                                                Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);
                                        }
                                        else
                                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);


                                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", Decimal.Round(VCtoEntrada, 4, MidpointRounding.AwayFromZero));
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaOrden, false));
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoInicial", Decimal.Round(VCtoEntrada, 4, MidpointRounding.AwayFromZero));

                                        var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) & a.ProductoID.Equals(Producto));

                                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) <= 0)
                                        {
                                            Parametros.General.DialogMsg("El producto no se encuentra en el almacen seleccionado.", Parametros.MsgType.warning);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            #endregion

            //if (rgOptions.SelectedIndex.Equals(1))
            //    SetCostoConversion();
        }

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

                        spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));
                        //Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["SubTotal"].SummaryText) + Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                view.AddNewRow();
                spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));
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

                        spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));
                                               
                    }
                }
            }
        }
        
        #endregion

        #region <<< GRID_BOTTOM >>>

        private void gvConversion_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

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
            if (view.GetRowCellValue(RowHandle, "CantidadEntrada") != null)
            {
                if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadEntrada")) <= 0.00)
                {

                    view.SetColumnError(view.Columns["CantidadEntrada"], "La Cantidad debe ser mayor a cero");
                    e.ErrorText = "La Cantidad debe ser mayor a cero";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CantidadEntrada"], "La Cantidad debe ser mayor a cero");
                e.ErrorText = "La Cantidad debe ser mayor a cero";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Productos             
            if (view.GetRowCellValue(RowHandle, "AlmacenEntradaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen");
                    e.ErrorText = "Debe Seleccionar un Almacen";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen");
                e.ErrorText = "Debe Seleccionar un Almacen";
                e.Valid = false;
                Validate = false;
            }

            //SetCostoConversion();

        }
        
        private void gvConversion_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        
        }

        private void gvConversion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridConversion.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                        //SetCostoConversion();
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridConversion.DefaultView;
                view.AddNewRow();
            }
        }

        private void gvConversion_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

            #region <<< CALCULOS_MONTOS >>>
            //--  Calcular las montos de las columnas de Impuestos y Subtotal
            if (e.Column == colCantidadC || e.Column == colCostoTotalC)
            {
                try
                {
                    //if (CalculandoConversion)
                    //    return;
                    decimal vCosto = 0, vCantidad = 1, vCostoTotal = 0;


                    if (gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada") != null)
                        vCantidad = Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada"));

                    if (vCantidad > 0m)
                    {
                            if (gvConversion.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                                vCostoTotal = Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CostoTotal"));

                            gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", (vCostoTotal / vCantidad));

                            //if (gvConversion.GetRowCellValue(e.RowHandle, "CostoEntrada") != null)
                            //    vCosto = Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CostoEntrada"));


                            //vCantidad = Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
                            //gvConversion.SetRowCellValue(e.RowHandle, "CostoTotal", (vCosto * vCantidad));
                    }
                    else
                    {
                        gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", 0);

                        if (Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CostoTotal")) > 0)
                            gvConversion.SetRowCellValue(e.RowHandle, "CostoTotal", 0);
                    }
                        //    
                        //else

                    if (gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada") == null)
                        gvConversion.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                    //if (Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada")).Equals(0))
                    //{
                    //    gvConversion.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);
                    //    CalculandoConversion = true;
                    //}

                    //SetCostoConversion();
                    //vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);

                    //gvConversion.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);
                    //CalculandoConversion = false;
                }
                catch (Exception ex)
                {
                    //CalculandoConversion = false;
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            }
            #endregion

            #region <<< COLUMNA_PRODUCTO >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colProductoC)
            {
                if (gvConversion.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                {
                    if (Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
                    {
                        return;
                    }
                    else if (DetalleKC.Count(d => d.ProductoID.Equals(Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                    {
                        Parametros.General.DialogMsg("El producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        gvConversion.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                        gvConversion.FocusedColumn = colProductoC;
                        return;
                    }
                }

                try
                {
                    var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "ProductoID")));

                    //-- Unidad Principal     
                    //var Um = Producto.UnidadMedidaID;
                    gvConversion.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                    cboAlmacenSalida.DataSource = listaAlmacen.ToList();
                    //if (gvConversion.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") == null)
                    gvConversion.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", listaAlmacen.First().ID);
                    gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", 0.0000m);
                    gvConversion.SetRowCellValue(e.RowHandle, "", 4);
                    //var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "AlmacenEntradaID"))) & a.ProductoID.Equals(Producto.ID));

                    //if (query.ToList().Count > 0)
                    //{
                    //    decimal vExistencia = query.First().Cantidad;
                    //    gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", query.First().Costo);
                    //}
                    //else
                    //{
                    //    gvConversion.SetRowCellValue(e.RowHandle, "CantidadInicial", 0.0000m);
                    //    gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", 0.0000m);
                    //}

                    //-- Cantidad Inicial de 1
                    if (Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CantidadEntrada")) == Convert.ToDecimal("0.00"))
                        gvConversion.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                    //SetCostoConversion();
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            //--
            #endregion

            #region <<< COLUMNA_ALMACEN >>>
            if (e.Column == colAlmacenC)
            {
                try
                {
                    if (gvConversion.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                        {
                            if (gvConversion.FocusedColumn.Name.Equals(colAlmacenC.Name))
                            {
                                if (gvConversion.GetRowCellValue(e.RowHandle, "AlmacenEntradaID") != null)//
                                {
                                    if (Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "AlmacenEntradaID")) > 0)
                                    {
                                        int Producto = Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "ProductoID"));

                                        decimal VCtoEntrada = 0, vCtoFinal = 0;

                                        if (IDFactura > 0)
                                        {
                                            var kProd = db.Kardexes.Where(k => k.ProductoID.Equals(Producto)).FirstOrDefault();

                                            if (kProd != null)
                                                VCtoEntrada = Decimal.Round(kProd.CostoEntrada, 4, MidpointRounding.AwayFromZero);
                                            else
                                                Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);
                                        }
                                        else
                                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);


                                        gvConversion.SetRowCellValue(e.RowHandle, "CostoEntrada", Decimal.Round(VCtoEntrada, 4, MidpointRounding.AwayFromZero));
                                        //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaOrden, false));


                                        var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvConversion.GetRowCellValue(e.RowHandle, "AlmacenEntradaID"))) & a.ProductoID.Equals(Producto));

                                        if (query.Count() <= 0)//Convert.ToDecimal(gvConversion.GetRowCellValue(e.RowHandle, "CantidadInicial")) <= 0)
                                        {
                                            Parametros.General.DialogMsg("El producto no se encuentra en el almacen seleccionado.", Parametros.MsgType.warning);
                                            //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                            gvConversion.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", 0);
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            #endregion
        }

        private void gridConversion_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridConversion.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                        spMonto.Value = Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal));
                        //SetCostoConversion();
                    }
                }
            }
        }

        private void SetCostoConversion()
        {
            try
            {
                if (_Tipo.Equals(Parametros.TiposDevConvProveedor.Conversion))
                {
                    decimal cont = KC.Sum(s => s.CantidadEntrada);
                    if (KC.Count > 0 && cont > 0)
                    {
                        decimal vCosto = Decimal.Round((spMonto.Value / cont), 4, MidpointRounding.AwayFromZero);

                        KC.ForEach(line =>
                            {
                                line.CostoEntrada = vCosto;
                                line.CostoTotal = Decimal.Round(vCosto * line.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                            });
                        
                        gvConversion.RefreshData();
                    }
                }
            }            
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        //Validar fecha de ingreso y actualizar el tipo de cambio
        private void dateFechaIngreso_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaOrden.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaOrden.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaOrden.Date)).First().Valor : 0m);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
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
            if (DetalleK.Count > 0 || Convert.ToInt32(glkProvee.EditValue) <= 0)
            {
                if (Parametros.General.DialogMsg("El movimiento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }

            _Next = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();

        }
       
        private void rgOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgOptions.SelectedIndex.Equals(0))
                {
                    _Tipo = Parametros.TiposDevConvProveedor.Devolucion;
                    groupControlTop.Text = "Devolución";
                    layoutControlItemFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlProv.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.btnShowComprobante.Enabled = true;
                    colIva.Visible = colTotal.Visible = true;
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(12)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(12)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");

                    layoutControlRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                else if (rgOptions.SelectedIndex.Equals(1))
                {
                    _Tipo = Parametros.TiposDevConvProveedor.Conversion;
                    layoutControlProv.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    glkProvee.EditValue = null;
                    groupControlTop.Text = "Salida"; groupControlBottom.Text = "Entrada";
                    layoutControlItemFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    ProveeReferencia = null;
                    this.btnShowComprobante.Enabled = false;
                    splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
                    colIva.Visible = colTotal.Visible = false;
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(13)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(13)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");

                    //--- Fill Combos Detalles --//
                    gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                    gridProductosC.View.OptionsBehavior.AutoPopulateColumns = false;

                    //-------------------PRODCUTOS---------------------------//
                    gridProductos.DataSource = null;
                    gridProductosC.DataSource = null;
                    var ListaProductos = from P in db.Productos
                                         join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                         join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                         join A in db.Areas on C.AreaID equals A.ID
                                         join AL in db.AlmacenProductos on P.ID equals AL.ProductoID
                                         join ALM in db.Almacens on AL.AlmacenID equals ALM.ID
                                         where P.Activo.Equals(true) & P.Conversion & A.Activo & C.Activo && !C.ID.Equals(Parametros.Config.ProductoClaseCombustible()) && ALM.EstacionServicioID.Equals(IDEstacionServicio) && ALM.SubEstacionID.Equals(IDSubEstacion)
                                         group P by new { P.ID, P.Codigo, P.Nombre, Unidad = U.Nombre, P.AplicaDescuento, P.SiempreSalidaVentas } into gr
                                         select new
                                         {
                                             ID = gr.Key.ID,
                                             Codigo = gr.Key.Codigo,
                                             Nombre = gr.Key.Nombre,
                                             UmidadName = gr.Key.Unidad,
                                             AplicaDescuento = gr.Key.AplicaDescuento,
                                             SiempreSalidaVentas = gr.Key.SiempreSalidaVentas,
                                             Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                         };

                    gridProductos.DataSource = ListaProductos;
                    gridProductosC.DataSource = ListaProductos;

                    layoutControlRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {

                if (rgOptions.SelectedIndex.Equals(0) && Provee != null)
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable";
                        nf.ShowDialog();
                    }
                }                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void glkProvee_EditValueChanged(object sender, EventArgs e)
        {
            if (!IDProveedor.Equals(0))
            {
                Provee = db.Proveedors.Single(p => p.ID.Equals(IDProveedor));
                rgOptions.Properties.ReadOnly = true;

                if (Provee.AplicaIVA)
                {
                    colIva.OptionsColumn.AllowEdit = true;
                    colIva.OptionsColumn.AllowFocus = true;
                }
                else
                {
                    colIva.OptionsColumn.AllowEdit = false;
                    colIva.OptionsColumn.AllowFocus = false;
                }
                
                 var query = db.Movimientos.Where(m => m.ProveedorID.Equals(IDProveedor) && m.MovimientoTipoID.Equals(3) && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)).Select(s => new { s.ID, s.Referencia, s.FechaRegistro, Monto = s.Monto - s.Abono}).ToList();
                 dateFechaOrden.Properties.ReadOnly = true;
                     //, Display = "Nro. " + s.Referencia + " | Fecha: " + s.FechaRegistro.ToString() + " | Valor: " + s.Monto.ToString("{0:N2}") }).ToList();
                 GlkFactura.Properties.DataSource = query.Select(s => new { s.ID, s.Referencia, s.FechaRegistro, s.Monto, Display = "Nro. " + s.Referencia + " | Fecha: " + s.FechaRegistro.ToShortDateString() + " | Valor: " + s.Monto.ToString("N2") }).ToList();

                int id = (query.Count > 0 ? query.First().ID : 0);

                 if (rgOptions.SelectedIndex.Equals(0))
            {
                
                    //--- Fill Combos Detalles --//
                    gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                    gridProductosC.View.OptionsBehavior.AutoPopulateColumns = false;
               
                    //-------------------PRODCUTOS---------------------------//
                    gridProductos.DataSource = null;
                    gridProductosC.DataSource = null;
                    var ListaProductos = from P in db.Productos
                                         join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                         join K in db.Kardexes.Where(o => o.MovimientoID.Equals(id)) on P.ID equals K.ProductoID
                                         where P.Activo //& A.Activo & C.Activo && !C.ID.Equals(Parametros.Config.ProductoClaseCombustible()) //&& ALM.EstacionServicioID.Equals(IDEstacionServicio) && ALM.SubEstacionID.Equals(IDSubEstacion)
                                         group P by new { P.ID, P.Codigo, P.Nombre, Unidad = U.Nombre, P.AplicaDescuento, P.SiempreSalidaVentas } into gr
                                         select new
                                         {
                                             ID = gr.Key.ID,
                                             Codigo = gr.Key.Codigo,
                                             Nombre = gr.Key.Nombre,
                                             UmidadName = gr.Key.Unidad,
                                             AplicaDescuento = gr.Key.AplicaDescuento,
                                             SiempreSalidaVentas = gr.Key.SiempreSalidaVentas,
                                             Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                         };

                    gridProductos.DataSource = ListaProductos;
                    gridProductosC.DataSource = ListaProductos;
            }

            }

        }

        private void glkProvee_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (rgOptions.SelectedIndex < 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar el tipo de movimiento.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void spMonto_EditValueChanged(object sender, EventArgs e)
        {
            //if (rgOptions.SelectedIndex.Equals(1))
                //SetCostoConversion();
        }

        private void GlkFactura_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IDFactura > 0)
                {
                    //Proveedor Referencia
                    ProveeReferencia = null;
                    var PR = db.Movimientos.SingleOrDefault(m => m.ID.Equals(IDFactura));
                    if (PR != null)
                    {
                        if ((PR.FechaRegistro.Date > dateFechaOrden.DateTime.Date))
                        {
                            Parametros.General.DialogMsg("No se puede cargar la factura con fecha: " + PR.FechaRegistro.Date.ToShortDateString() + ", ya que no corresponde a un periodo valido.\nVerificar la fecha de este movimiento: " + dateFechaOrden.DateTime.Date.ToShortDateString(), Parametros.MsgType.warning);
                            IDFactura = 0;
                        }
                        if (PR.ProveedorReferenciaID > 0)
                        {
                            ProveeReferencia = db.Proveedors.SingleOrDefault(p => p.ID.Equals(PR.ProveedorReferenciaID));
                            dateFechaOrden.Properties.ReadOnly = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void gvDetalle_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn.Name.Equals(colIva.Name))
                if (gvDetalle.GetFocusedValue() != null)
                    oldIVA = Convert.ToDecimal(gvDetalle.GetFocusedValue());
        }

        private void gvDetalle_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

        }

        #endregion
    }
}