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
using SAGAS.Ventas.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogVentaCreditoCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormVentaCreditoCombustible MDI;
        internal Entidad.ResumenDia EntidadRD;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        private List<Parametros.ListIdTidDisplayPriceValue> listaProductoRD;
        private int IDPrint = 0;
        private decimal _Disponible = 0;
        internal bool Next = false;
        private decimal _TipoCambio = 0;
        private decimal _MargenAjusteVentasCreditoComb;
        internal int _MonedaPrimaria;
        internal DateTime _FechaActual;
        internal int _ClienteAnticipo;
        
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
        
        private DateTime FechaVenta
        {
            get { return Convert.ToDateTime(dateFechaVenta.EditValue); }
            set { dateFechaVenta.EditValue = value; }
        }

        private DateTime FechaVencimiento
        {
            get { return Convert.ToDateTime(dateFechaVencimiento.EditValue); }
            set { dateFechaVencimiento.EditValue = value; }
        }

        private int IDCliente
        {
            get { return Convert.ToInt32(glkClient.EditValue); }
            set { glkClient.EditValue = value; }
        }

        private static Entidad.Cliente client;

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

        public DialogVentaCreditoCombustible(int UserID, bool _editando)
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
                _MargenAjusteVentasCreditoComb = Parametros.Config.MargenAjusteVentasCreditoComb();
                spAjuste.MaxValue = Math.Abs(_MargenAjusteVentasCreditoComb);
                spAjuste.MinValue = -Math.Abs(_MargenAjusteVentasCreditoComb);


                  var Clientes =  (from c in db.Clientes
                     join ces in db.ClienteEstacions on c.ID equals ces.ClienteID
                     where ces.EstacionServicioID.Equals(IDEstacionServicio) && c.Activo && !c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID())
                     group c by new { ces.ClienteID, c.Codigo, c.Nombre } into gr
                     select new
                     {
                         ID = gr.Key.ClienteID,
                         Codigo = gr.Key.Codigo,
                         Nombre = gr.Key.Nombre,
                         Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                     }).ToList();

                  glkClient.Properties.DataSource = Clientes;
                //db.Clientes.Where(c => c.Activo && !c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID())).Select(s => new { s.ID, s.Codigo, s.Nombre, Display = s.Codigo + " | " + s.Nombre }).ToList();
                chkDescuento.Checked = true;
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _FechaActual = Convert.ToDateTime(db.GetDateServer());
                _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();
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
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                gridProductos.DataSource = from P in db.Productos
                                     join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                     join t in db.Tanques.Where(t => t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion))  on P.ID equals t.ProductoID
                                     where P.Activo & P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                        group P by new { P.ID, P.Codigo, P.Nombre, Unidad = U.Nombre } into gr
                                    select new
                                     {
                                         ID = gr.Key.ID,
                                         Codigo = gr.Key.Codigo,
                                         Nombre = gr.Key.Nombre,
                                         UmidadName = gr.Key.Unidad,
                                         Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                     };

                dateFechaVenta.EditValue = null;// Convert.ToDateTime(db.GetDateServer());

                this.bdsDetalle.DataSource = this.DetalleEM;

                if (!Parametros.General.ClienteID.Equals(0))
                    IDCliente = Parametros.General.ClienteID;

                if (Parametros.General.FechaVenta.Date > Convert.ToDateTime("01/01/2014"))
                    FechaVenta = Parametros.General.FechaVenta;

                if (dateFechaVenta.EditValue != null)
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaVenta.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaVenta.Date)).First().Valor : 0m);
                
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

        private bool ValidarReferencia(int code)
        {
            var result = (from i in db.Movimientos
                          where i.Numero.Equals(code) && i.AreaID.Equals(db.AreaVentas.First(a => a.EsCombustible).ID) && i.MovimientoTipoID.Equals(7) && !i.Anulado && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion)
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
                if (txtNumero.Text == "" || Convert.ToInt32(glkClient.EditValue) <= 0 || dateFechaVenta.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
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

                if (!Parametros.General.ValidatePeriodoContable(FechaVenta, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }

                if (!detalle)
                {
                    if (DetalleEM.Count <= 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }

                    if (!ValidarReferencia(Convert.ToInt32(Numero)))
                    {
                        Parametros.General.DialogMsg("El numero de Doc para esta venta ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                        return false;
                    }

                }

                return true;
            }
            catch { return false; }
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false))
                return false;

            if (_Disponible <= 0)
            {
                Parametros.General.DialogMsg("El cliente no tiene saldo disponible.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToDecimal(DetalleEM.Sum(s => s.PrecioTotal)) > _Disponible)
            {
                Parametros.General.DialogMsg("El monto total es mayor al saldo disponible por el cliente.", Parametros.MsgType.warning);
                return false;
            }

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M = new Entidad.Movimiento();
                    M.ClienteID = IDCliente;
                    M.MovimientoTipoID = 7;
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = FechaVenta;
                    M.FechaVencimiento = FechaVencimiento;
                    M.MonedaID = _MonedaPrimaria;
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Convert.ToDecimal(DetalleEM.Sum(s => s.PrecioTotal));
                    M.MontoMonedaSecundaria = Decimal.Round((DetalleEM.Sum(s => s.PrecioTotal) / M.TipoCambio), 2, MidpointRounding.AwayFromZero);
                    M.Numero = Convert.ToInt32(Numero);
                    M.Credito = true;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = memoComentario.Text;
                    M.ResumenDiaID = EntidadRD.ID;
                    M.CuentaID = db.Clientes.Single(s => s.ID.Equals(IDCliente)).CuentaContableID;
                    M.AreaID = db.AreaVentas.First(a => a.EsCombustible).ID;

                    db.Movimientos.InsertOnSubmit(M);                    
                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    foreach (var dk in DetalleEM)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        
                        if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                            return false;
                        }

                        if (dk.CantidadSalida <= 0)
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("La Cantidad del producto " + Producto.Codigo + " | " + Producto.Nombre + " debe ser mayor a 0 (cero).", Parametros.MsgType.warning);
                            return false;
                        }
                        
                        Entidad.Kardex KX = new Entidad.Kardex();

                        KX.MovimientoID = M.ID;
                        KX.ProductoID = Producto.ID;
                        KX.EsProducto = !Producto.EsServicio;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;
                        KX.EstacionServicioID = IDEstacionServicio;
                        KX.SubEstacionID = IDSubEstacion;
                        KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                        KX.CantidadSalida = dk.CantidadSalida;
                        KX.CostoSalida = dk.CostoSalida;
                        KX.CostoTotal = Decimal.Round(dk.CostoSalida * dk.CantidadSalida, 2, MidpointRounding.AwayFromZero);

                        var TP = (from tp in db.TanqueProductos
                                  where tp.ProductoID.Equals(Producto.ID)
                                  && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                                  select tp).ToList();

                        //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                        //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                        if (TP.Count() == 0)
                        {
                            KX.CantidadInicial = 0;
                        }
                        else
                            KX.CantidadInicial = TP.Single(q => q.ProductoID.Equals(dk.ProductoID)).Cantidad;

                        KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                        if (KX.CantidadSalida > dk.CantidadInicial)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                            return false;
                        }

                        if (KX.CantidadSalida > dk.CantidadEntrada)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa el arqueo.", Parametros.MsgType.warning);
                            return false;
                        }

                        KX.Precio = dk.Precio;
                        KX.PrecioTotal = dk.PrecioTotal;
                        KX.Descuento = dk.Descuento;

                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                        var Tanque = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(Producto.ID)
                                        && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                                      select tp).ToList();

                        if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                        {
                            Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
                            TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - KX.CantidadSalida;
                        }

                        db.SubmitChanges();

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

                    #region ::: REGISTRANDO DEUDOR :::
                    Entidad.Deudor D = new Entidad.Deudor();
                    D.ClienteID = IDCliente;
                    D.Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero);
                    D.MovimientoID = M.ID;
                    db.Deudors.InsertOnSubmit(D);
                    db.SubmitChanges();

                    //db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = IDCliente, Valor = Decimal.Round(M.Monto, 3, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                    //db.SubmitChanges();
                    if (client != null)
                    {
                        if (client.CuentaContableID.Equals(_ClienteAnticipo))
                        {
                            decimal Saldo = 0m;
                            
                            var qSaldo = from dr in db.Deudors
                                             join mv in db.Movimientos on dr.MovimientoID equals mv.ID
                                             where !dr.ID.Equals(D.ID) && dr.ClienteID.Equals(IDCliente) && !mv.Anulado
                                             select dr.Valor;
                            //).Sum(s => s.Valor) : 0m);

                            if (qSaldo.Count() > 0)
                                Saldo = Decimal.Round(qSaldo.Sum(), 2, MidpointRounding.AwayFromZero);

                            if (Saldo < 0)
                            {
                                if (Math.Abs(Saldo) >= M.Monto)
                                {
                                    M.Abono = M.Monto;
                                    M.Pagado = true;
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = M.ID, Monto = M.Abono });
                                }
                                else
                                {
                                    M.Abono = Math.Abs(Saldo);
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = M.ID, Monto = M.Abono });
                                }

                                db.SubmitChanges();
                            }
                        }
                    }
                    
                    #endregion

                    //para que actualice los datos del registro
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                    "Se registró la Venta de Crédito: " + M.Numero.ToString("000000000"), this.Name);
                    Parametros.General.ClienteID = client.ID;
                    Parametros.General.FechaVenta = FechaVenta.Date;

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    Next = true;
                    IDPrint = M.ID;                                        
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
                    if (client != null && !String.IsNullOrEmpty(Numero) && DetalleEM.Count > 0 && dateFechaVenta.EditValue != null)
                    {
                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                        string texto = "Factura de Venta " + client.Nombre + " Nro. " + Numero.ToString();
                        int i = 1;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = client.CuentaContableID,
                            Monto = Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.PrecioTotal)), 2, MidpointRounding.AwayFromZero),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.PrecioTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaVenta,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;

                        List<Int32> IDDescuento = new List<Int32>();
                        List<Int32> IDVenta = new List<Int32>();
                        List<Int32> IDCosto = new List<Int32>();
                        List<Int32> IDInventario = new List<Int32>();

                        #region <<< DETALLE_COMPROBANTE >>>
                        DetalleEM.ToList().ForEach(K =>
                        {
                            var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));

                            #region <<< CUENTA_DESCUENTO >>>

                            //if (!producto.CuentaDescuentoID.Equals(0) && K.Descuento > 0)
                            //{
                            //    if (!IDDescuento.Contains(producto.CuentaDescuentoID))
                            //    {
                            //        CD.Add(new Entidad.ComprobanteContable
                            //        {
                            //            CuentaContableID = producto.CuentaDescuentoID,
                            //            Monto = Decimal.Round(Convert.ToDecimal(K.Descuento), 2, MidpointRounding.AwayFromZero),
                            //            TipoCambio = _TipoCambio,
                            //            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.Descuento) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            //            Fecha = FechaVenta,
                            //            Descripcion = texto,
                            //            Linea = i,
                            //            EstacionServicioID = IDEstacionServicio,
                            //            SubEstacionID = IDSubEstacion
                            //        });
                            //        IDDescuento.Add(producto.CuentaDescuentoID);
                            //        i++;
                            //    }
                            //    else
                            //    {
                            //        var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaDescuentoID)).First();
                            //        comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.Descuento), 2, MidpointRounding.AwayFromZero);
                            //        comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.Descuento) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            //    }
                            //}

                            #endregion

                            #region <<< CUENTA_VENTA >>>

                            if (!producto.CuentaVentaID.Equals(0))
                            {
                                if (!IDVenta.Contains(producto.CuentaVentaID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaVentaID,
                                        Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(K.PrecioTotal)), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.PrecioTotal) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                        Fecha = FechaVenta,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    IDVenta.Add(producto.CuentaVentaID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaVentaID)).First();
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.SubTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.SubTotal) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion

                            #region <<< CUENTA_INVENTARIO >>>
                            if (!producto.CuentaInventarioID.Equals(0))
                            {
                                if (!IDInventario.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                        Fecha = FechaVenta,
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
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion

                            #region <<< CUENTA_COSTO >>>
                            if (!producto.CuentaCostoID.Equals(0))
                            {
                                if (!IDCosto.Contains(producto.CuentaCostoID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaCostoID,
                                        Monto = Decimal.Round(Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                        Fecha = FechaVenta,
                                        Descripcion = texto,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    IDCosto.Add(producto.CuentaCostoID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaCostoID)).First();
                                    comprobante.Monto += Decimal.Round(Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion



                        });

                        #endregion

                        return CD;
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe seleccionar un Cliente.", Parametros.MsgType.warning);
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
            MDI.CleanDialog(ShowMsg, Next, IDPrint, RefreshMDI);
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

            //-- Validar Existencia             
            if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
            {
                if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")).Equals(0))
                {
                    if (view.GetRowCellValue(RowHandle, "CantidadEntrada") != null)
                    {
                        if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadEntrada")).Equals(0))
                        {
                            if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) > Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadEntrada")))
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

                    if (view.GetRowCellValue(RowHandle, "CantidadInicial") != null)
                    {
                    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")).Equals(0))
                        {
                            if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) > Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")))
                            {
                                view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
                                e.ErrorText = "La cantidad a despachar sobrepasa la existencia";
                                e.Valid = false;
                                Validate = false;
                            }
                        }
                        else
                        {
                            view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
                            e.ErrorText = "La cantidad a despachar sobrepasa la existencia";
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
                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar debe ser mayor a 0 (cero)");
                    e.ErrorText = "La cantidad a despachar debe ser mayor a 0 (cero)";
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
            if (e.Column == colQuantity)
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
                                    Parametros.General.DialogMsg("La cantidad a vender sobrepasa la existencia", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                }
                            }
                        }
                    }
                }
            }

            //--  Calcular las montos de la Venta
            if (e.Column == colQuantity || e.Column == colDescuento)
            {
                try
                {
                    decimal vPrecio = 0, vCantidad = 0, vSubTotal = 0, vDescuento = 0, vTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Precio") != null)
                        vPrecio = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Precio"));

                    if ((gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) != null)
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));

                    //if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) != Convert.ToDecimal("0.00"))
                    //    vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));
                    //else 
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);

                    //if (chkDescuento.Checked)
                    //{
                    //    vDescuento = Decimal.Round(vCantidad * 0.26m, 2, MidpointRounding.AwayFromZero);
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "Descuento", vDescuento);
                    //}

                    if ((gvDetalle.GetRowCellValue(e.RowHandle, "Descuento")) != null)
                        vDescuento = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Descuento"));

                    vSubTotal = Decimal.Round((vPrecio * vCantidad), 2, MidpointRounding.AwayFromZero);
                    gvDetalle.SetRowCellValue(e.RowHandle, "SubTotal", vSubTotal);

                    vTotal = vSubTotal + vDescuento;
                    gvDetalle.SetRowCellValue(e.RowHandle, "PrecioTotal", vTotal);


                    //    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "VentaNeta")) != Convert.ToDecimal("0.00"))
                    //        vVentaNeta = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "VentaNeta"));

                    //    if (client != null)
                    //    {
                    //        if (!client.ExentoIVA)
                    //        {
                    //            if (gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto") != null)
                    //            {
                    //                if (!Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
                    //                {
                    //                    vValorImpuesto = Decimal.Round((vVentaNeta * 0.15m), 2, MidpointRounding.AwayFromZero);
                    //                    gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto") != null)
                    //        {
                    //            if (!Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
                    //            {
                    //                vValorImpuesto = Decimal.Round((vVentaNeta * 0.15m), 2, MidpointRounding.AwayFromZero);
                    //                gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
                    //            }
                    //        }
                    //    }
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
                    var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                    //-- Unidad Principal     
                    //var Um = Producto.UnidadMedidaID;
                    gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                    var TCombustible = from t in db.Tanques
                                       //join l in listaProductoRD on t.ProductoID equals l.ID
                                       where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                       && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                       select new { ID = t.ID, Display = t.Nombre };

                    ColAlmacenSalida.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                    cboAlmacenSalida.DataSource = TCombustible;//lkAlmacen.Properties.DataSource;
                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", (TCombustible.Count() > 0 ? TCombustible.First().ID : 0));

                    var query = listaProductoRD.Where(l => l.ID.Equals(Producto.ID) && l.TID.Equals(TCombustible.Count() > 0 ? TCombustible.First().ID : 0)).FirstOrDefault();

                    //gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaVenta, false));

                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Decimal.Round(Convert.ToDecimal(db.SaldoKardex(IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaVenta, false)), 3, MidpointRounding.AwayFromZero));

                    if (query != null)
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", query.Value);
                        gvDetalle.SetRowCellValue(e.RowHandle, "Precio", query.Price);
                    }
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                    //decimal VCtoEntrada, vCtoFinal;
                    //Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaVenta, out vCtoFinal, out VCtoEntrada);

                    decimal vCtoFinal = Decimal.Round(Convert.ToDecimal(db.GetLastCostoPromedio(IDEstacionServicio, IDSubEstacion, Producto.ID, FechaVenta, 0)), 4, MidpointRounding.AwayFromZero);

                    //-- Cantidad Inicial de 1
                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", Decimal.Round(vCtoFinal, 4, MidpointRounding.AwayFromZero));



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
                            if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")).Equals(0))
                            {
                                var TCombustible = from t in db.Tanques
                                                   //join l in listaProductoRD on t.ProductoID equals l.ID
                                                   where t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                                   && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                                   select new { ID = t.ID, Display = t.Nombre };
                                cboAlmacenSalida.DataSource = TCombustible;

                                if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).Equals(0))
                                {
                                    Parametros.General.DialogMsg("El Tanque seleccionado no contiene este producto", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                }
                                else
                                {
                                    var query = listaProductoRD.Where(l => l.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")))).FirstOrDefault();

                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")), Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaVenta, false));

                                    if (query != null)
                                    {
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", query.Value);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "Precio", query.Price);
                                    }
                                    else
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                                    decimal VCtoEntrada, vCtoFinal;
                                    Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")), 0, FechaVenta, out vCtoFinal, out VCtoEntrada);

                                    //-- Cantidad Inicial de 1
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", Decimal.Round(vCtoFinal, 4, MidpointRounding.AwayFromZero));

                                }

                            }
                        }
                    }

                    //cboAlmacenEntrada.DataSource = listaTanque.ToList();

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            }
            #endregion
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
               
        //Carga Data del Cliente
        private void glkClient_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkClient.EditValue) > 0)
                {
                    client = db.Clientes.SingleOrDefault(p => p.ID.Equals(IDCliente));

                    if (client != null)
                    {
                        if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                        {

                            lbCliente.Items.Clear();
                            lbCliente.Items.Add("Límite de Crédito   |  C$  " + client.LimiteCredito.ToString("#,0.00"));

                            decimal Saldo = Convert.ToDecimal(db.GetSaldoCliente(client.ID, _FechaActual));
                            lbCliente.Items.Add("Saldo Actual          |  C$  " + Saldo.ToString("#,0.00"));

                            _Disponible = client.LimiteCredito - Saldo;
                            lbCliente.Items.Add("Disponible              |  C$  " + _Disponible.ToString("#,0.00"));

                            if (dateFechaVenta.EditValue != null && client != null)
                                FechaVencimiento = Convert.ToDateTime(dateFechaVenta.EditValue).AddDays(client.Plazo);
                            else
                                dateFechaVencimiento.EditValue = null;
                        }
                        else
                        {
                            lbCliente.Items.Clear();

                            var obj = from d in db.Deudors
                                      join m in db.Movimientos on d.MovimientoID equals m.ID
                                      join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                      where !m.Anulado && !mt.EsAnulado && (m.ClienteID.Equals(client.ID) || d.ClienteID.Equals(client.ID)) 
                                      select d.Valor;

                            if (obj != null)
                            {
                                decimal limite = client.LimiteCredito;
                                decimal Saldo = (obj.Count() > 0 ? Convert.ToDecimal(obj.Sum(s => s)) : 0m);
                                _Disponible = Convert.ToDecimal(obj.Sum(s => s) * -1);

                                lbCliente.Items.Add("Disponible              |  C$  " + _Disponible.ToString("#,0.00"));
                                lbCliente.Items.Add("Saldo Actual          |  C$  " + Saldo.ToString("#,0.00"));

                            }
                            else
                            {
                                _Disponible = 0;
                                lbCliente.Items.Add("Disponible              |  C$  " + _Disponible.ToString("#,0.00"));
                            }

                            //if (obj != null)
                            //{
                            //    if (obj.Count() > 0)
                            //        _Disponible = Convert.ToDecimal(obj.Sum(s => s) * -1);
                            //    else
                            //        _Disponible = 0m;
                            //}
                            //else
                            //    _Disponible = 0m;

                            //lbCliente.Items.Add("Disponible              |  C$  " + _Disponible.ToString("#,0.00"));

                            if (dateFechaVenta.EditValue != null && client != null)
                                FechaVencimiento = Convert.ToDateTime(dateFechaVenta.EditValue).AddDays(client.Plazo);
                            else
                                dateFechaVencimiento.EditValue = null;
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("No ha seleccionado un Cliente valido.", Parametros.MsgType.warning);
                        glkClient.EditValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Carga Data del Resumen
        private void dateFechaEntrada_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dateFechaVenta.EditValue != null)
                {
                    EntidadRD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDEstacionServicio) && r.SubEstacionID.Equals(IDSubEstacion) && r.FechaInicial.Date.Equals(FechaVenta.Date)).FirstOrDefault();

                    if (EntidadRD != null)
                    {
                        layoutControlGroupRD.Text = "Resumen Arqueo Nro: " + EntidadRD.Numero.ToString() + " | Litros Restantes";

                        listaProductoRD = null;

                        listaProductoRD = (from ape in db.ArqueoProductoExtracions
                                  join ap in db.ArqueoProductos on ape.ArqueoProductoID equals ap.ID
                                  join p in db.Productos.OrderBy(o => o.Codigo) on ap.ProductoID equals p.ID
                                  join tn in db.Tanques on ap.TanqueID equals tn.ID
                                  join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                  join t in db.Turnos on ai.TurnoID equals t.ID
                                  join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
                                           where rd.ID.Equals(EntidadRD.ID) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial)) && (ape.ExtracionID.Equals(1) || ape.ExtracionID.Equals(2))
                                           group ape by new { ap.ProductoID, ap.TanqueID, Nombre = p.Nombre + " => " + tn.Nombre } into gr
                                           select new Parametros.ListIdTidDisplayPriceValue
                                  {
                                      ID = gr.Key.ProductoID,
                                      TID = gr.Key.TanqueID,
                                      Display = gr.Key.Nombre,
                                      Price = gr.Min(pr => pr.ArqueoProducto.Precio),
                                      Value = gr.Sum(s => s.Valor)
                                  }).ToList();
                        
                        var menos = (from k in db.Kardexes
                                     join m in db.Movimientos on k.MovimientoID equals m.ID
                                     where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(7)
                                     && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                     group k by new { k.ProductoID, k.AlmacenSalidaID } into gr
                                     select new
                                     {
                                         ID = gr.Key.ProductoID,
                                         TID = gr.Key.AlmacenSalidaID,
                                         Value = gr.Sum(s => s.CantidadSalida)
                                     }).ToList();

                        lbResumen.Items.Clear();
                        listaProductoRD.OrderBy(o => o.ID).ToList().ForEach(item =>
                            {
                                if (menos.Count(m => m.ID.Equals(item.ID) && m.TID.Equals(item.TID)) > 0)
                                    item.Value -= menos.First(m => m.ID.Equals(item.ID) && m.TID.Equals(item.TID)).Value;

                                lbResumen.Items.Add(item.Display + " | " + item.Value.ToString("#,0.000") + " Litros  |  Precio  C$ " + item.Price);
                                //listaProductoRD.Add( new Parametros.ListIdDisplayValue (item.ID, item.Display, item.Value));
                            });

                        if (dateFechaVenta.EditValue != null && client != null)
                            FechaVencimiento = Convert.ToDateTime(dateFechaVenta.EditValue).AddDays(client.Plazo);
                        else
                            dateFechaVencimiento.EditValue = null;
                    }
                    else
                    {
                        Parametros.General.DialogMsg("La fecha seleccionada no tiene resumen del día.", Parametros.MsgType.warning);
                        dateFechaVenta.EditValue = null;
                    }
                }
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
                if (client != null && !String.IsNullOrEmpty(Numero) && DetalleEM.Count > 0 && dateFechaVenta.EditValue != null)
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable de ventas";
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
                dateFechaVenta.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaVenta.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaVenta.Date)).First().Valor : 0m);
        }
        
        private void chkDescuento_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        #endregion

        private void chkDescuento_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDescuento.Checked)
            {
                this.gvDetalle.Columns["Descuento"].OptionsColumn.AllowEdit = true;
                this.gvDetalle.Columns["Descuento"].OptionsColumn.ReadOnly = false;
                this.gvDetalle.Columns["Descuento"].OptionsColumn.AllowFocus = true;
            }
            else if (!chkDescuento.Checked)
            {
                this.gvDetalle.Columns["Descuento"].OptionsColumn.AllowEdit = false;
                this.gvDetalle.Columns["Descuento"].OptionsColumn.ReadOnly = true;
                this.gvDetalle.Columns["Descuento"].OptionsColumn.AllowFocus = false;
            }
            
        }

    }
}