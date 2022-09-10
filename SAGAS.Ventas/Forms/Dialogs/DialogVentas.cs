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
    public partial class DialogVentas : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormVentas MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        //private bool UnAlmacen = true;
        private int _CuentaIVA = Parametros.Config.IVAPorPagar();
        private decimal _ValorIVA = 0;
        private bool NextVenta = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private int ProductoAreaServicioID = Parametros.Config.ProductoAreaServicioID();
        private int _IDClaseCombustible = Parametros.Config.ProductoClaseCombustible();
        private bool _EsContado = false;
        private bool _ChangeTax = false; 
        private bool _Guardado = false;
        private decimal _MargenIVA= Parametros.Config.MargenValorCambioIVA();
        private decimal _OldTax = 0;
        private bool _ToPrint = false;
        private int IDPrint = 0;
        private List<Entidad.Almacen> listaAlmacen = new List<Entidad.Almacen>();
        private List<Entidad.AlmacenProducto> listaAlmacenProducto;
        private decimal _TipoCambio = 0;
        private decimal _MargenAjusteVentasCreditoComb;
        internal int _MonedaPrimaria;
        internal int _ClienteVentaContadoID;
        internal int _ProductoClaseCombustible;
        private decimal _Disponible = 0;
        internal DateTime _FechaActual;
        internal int _ClienteAnticipo;
        internal List<int> AreasCerradas = new List<int>();
        internal List<ListaAreas> EtAreas = new List<ListaAreas>();
        
        public struct ListaAreas
        {
            public int ID;
            public string Display;
            public int AreaID;
            public string Nombre;
        }

        private DateTime FechaVenta
        {
            get { return Convert.ToDateTime(dateFechaFactura.EditValue); }
            set { dateFechaFactura.EditValue = value; }
        }

        private int IDCliente
        {
            get { return Convert.ToInt32(glkClient.EditValue); }
            set { glkClient.EditValue = value; }
        }

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        

        private string Numero
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
        }

        private DateTime FechaVencimiento
        {
            get { return Convert.ToDateTime(dateFechaVencimiento.EditValue); }
            set { dateFechaVencimiento.EditValue = value; }
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }
        
        private static Entidad.EstacionServicio ES;
        private static Entidad.Cliente client;

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

        #endregion

        #region *** INICIO ***

        public DialogVentas(int UserID)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            //OnlyView = _OnlyView;

            //if (OnlyView)
            //{ //-- Bloquear Controles --//               
            //    glkProvee.Properties.ReadOnly = true;
            //    lkMoneda.Properties.ReadOnly = true;
            //    rgCreditoContado.Properties.ReadOnly = true;
            //    txtReferencia.Properties.ReadOnly = true;
            //    dateFechaFactura.Properties.ReadOnly = true;
            //    lkAlmacen.Properties.ReadOnly = true;
            //    chkEsCombustible.Properties.ReadOnly = true;
            //    chkAplicaISC.Properties.ReadOnly = true;
            //    glkProvee.Properties.ReadOnly = true;
            //    dateFechaVencimiento.Properties.ReadOnly = true;
            //    mmoComentario.Properties.ReadOnly = true;
            //    dateFechaIngreso.Properties.ReadOnly = true;
            //    gvDetalle.OptionsBehavior.Editable = false;
            //    gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
            //    gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            //    colCostoInical.Visible = false;
            //    btnOK.Enabled = false;
            //    btnOK.Visible = false;
            //    spTC.Properties.ReadOnly = true;
            //    chkIVA.Properties.ReadOnly = true;
            //    spISC.Properties.ReadOnly = true;
            //}
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
                
                
                this._ValorIVA = 0.15m;// Decimal.Round((db.CuentaContables.Single(c => c.ID.Equals(15)).Porcentaje / 100m), 2, MidpointRounding.AwayFromZero);
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                _MargenAjusteVentasCreditoComb = Parametros.Config.MargenAjusteVentasCreditoComb();
                spDescuento.MaxValue = Math.Abs(_MargenAjusteVentasCreditoComb);
                spDescuento.MinValue = -Math.Abs(_MargenAjusteVentasCreditoComb);
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _ClienteVentaContadoID = Parametros.Config.ClienteVentaConttadoID();
                _ProductoClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                _FechaActual = Convert.ToDateTime(db.GetDateServer());
                _ClienteAnticipo = Parametros.Config.CuentaAnticipoClienteID();

                //---LLenar Almacenes ---//
                listaAlmacen = db.Almacens.Where(al => al.Activo & al.EstacionServicioID.Equals(IDEstacionServicio) & al.SubEstacionID.Equals(IDSubEstacion)).ToList();
                //---LLenar Almacenes Productos ---//
                listaAlmacenProducto = (from ap in db.AlmacenProductos
                                       join a in db.Almacens.Where(o => listaAlmacen.Contains(o)) on ap.AlmacenID equals a.ID
                                       //where p.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && (l.AplicaVentas || Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "DetalleCombustible")).Equals(true))
                                       select ap).ToList();
                
                //Repository Item Lista de Almacenes
                //Almacen GRID
                cboAlmacen.DataSource = listaAlmacen.ToList();

                //Tipo de Area
                EtAreas = (from a in db.AreaVentas 
                join ar in db.Areas on a.ID equals ar.AreaVentaID
                join c in db.ProductoClases on ar.ID equals c.AreaID
                                               where a.Activo && ar.Activo && !c.ID.Equals(_ProductoClaseCombustible)
                                               group ar by new { a.ID, Display = a.Nombre, AreaID = ar.ID, ar.Nombre } into gr
                           select new ListaAreas { ID = gr.Key.ID, Display = gr.Key.Display, AreaID = gr.Key.AreaID, Nombre = gr.Key.Nombre }).ToList();

                lkArea.Properties.DataSource = EtAreas.GroupBy(g => new { g.ID, g.Display }).Select(s => new { ID = s.Key.ID, Display = s.Key.Display }).ToList();


                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";

                this.bdsDetalle.DataSource = this.DetalleK;
                
                if (!Parametros.General.ClienteID.Equals(0))
                    IDCliente = Parametros.General.ClienteID;

                if (!Parametros.General.AreaVentaID.Equals(0))
                    IDArea = Parametros.General.AreaVentaID;

                FechaVenta = (Parametros.General.FechaVenta.Date > Convert.ToDateTime("01/01/2000") ? Parametros.General.FechaVenta : Convert.ToDateTime(db.GetDateServer()));

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
            Parametros.General.ValidateEmptyStringRule(txtReferencia, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkClient, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(dateFechaVencimiento, errRequiredField);
        }

        private bool ValidarReferencia(int code)
        {
            var result = (from i in db.Movimientos
                          where i.Numero.Equals(code) && i.AreaID.Equals(IDArea) && i.MovimientoTipoID.Equals(7) && !i.Anulado && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (dateFechaFactura.EditValue == null || dateFechaVencimiento.EditValue == null || txtReferencia.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la venta.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkClient.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Cliente.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaFactura, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (FechaVencimiento.Date < FechaVenta.Date)
            {
                Parametros.General.DialogMsg("La fecha de vencimiento debe ser mayor a la fecha de venta.", Parametros.MsgType.warning);
                return false;
            }

            if (FechaVenta.Date > Convert.ToDateTime(db.GetDateServer()).Date)
            {
                Parametros.General.DialogMsg("La fecha de venta no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaVenta, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
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
            
            if (!detalle)
            {
                if (K.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a la venta." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                
            //VALIDAS BLOQUEO
                var obj = (from p in db.Productos
                           join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                           join ar in db.Areas on pc.AreaID equals ar.ID
                           where K.Select(s => s.ProductoID).Contains(p.ID)
                           group ar by new { ar.ID, ar.Nombre } into gr
                           select new { ID = gr.Key.ID, Nombre = gr.Key.Nombre}).ToList();

                string texto = "";
                obj.ForEach(det =>
                    {
                        if (!Parametros.General.ValidateKardexMovemente(FechaVenta, db, IDEstacionServicio, IDSubEstacion, 9, det.ID))
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaVenta.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                            texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                        }
                    });

                if (!String.IsNullOrEmpty(texto))
                {
                    infBloqueo.SetError(lkArea, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                    return false;
                }
                else
                    infBloqueo.SetError(lkArea, "", ErrorType.None);

                if (!ValidarReferencia(Convert.ToInt32(Numero)))
                {
                    Parametros.General.DialogMsg("El número para esta venta ya existe : " + Convert.ToString(txtReferencia.Text), Parametros.MsgType.warning);
                    return false;
                }

                if (K.Count(o => o.Precio.Equals(0)) > 0)
                {
                    Parametros.General.DialogMsg("El precio de venta de los productos no puede ser igual a 0 (cero).", Parametros.MsgType.warning);
                    return false;
                }
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (Convert.ToDecimal(txtGrandTotal.Text) <= 0)
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

            if (!client.ID.Equals(_ClienteVentaContadoID))
            {
                if (_Disponible <= 0)
                {
                    Parametros.General.DialogMsg("El cliente no tiene saldo disponible.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToDecimal(Convert.ToDecimal(txtGrandTotal.Text)) > _Disponible)
                {
                    Parametros.General.DialogMsg("El monto total es mayor al saldo disponible por el cliente.", Parametros.MsgType.warning);
                    return false;
                }
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

                    Entidad.Movimiento M;

                    M = new Entidad.Movimiento();
                    M.MovimientoTipoID = 7;
                    M.ClienteID = IDCliente;
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = FechaVenta;
                    M.MonedaID = _MonedaPrimaria;
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Convert.ToDecimal(txtGrandTotal.Text);
                    M.MontoMonedaSecundaria = Decimal.Round((M.Monto / M.TipoCambio), 2, MidpointRounding.AwayFromZero);
                    M.Credito = (M.ClienteID.Equals(_ClienteVentaContadoID) ? false : true);
                    M.Numero = Convert.ToInt32(Numero);
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.CuentaID = db.Clientes.Single(s => s.ID.Equals(IDCliente)).CuentaContableID;
                    M.FechaVencimiento = FechaVencimiento;
                    M.AreaID = IDArea;

                    db.Movimientos.InsertOnSubmit(M);                    
                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    foreach (var dk in DetalleK)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        
                        if (DetalleK.Count(x => x.ProductoID.Equals(dk)) > 1)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                            return false;
                        }

                        //decimal CostoMov = LineaDetalle.Cost;
                        Entidad.Kardex KX = new Entidad.Kardex();
                        KX.MovimientoID = M.ID;
                        KX.ProductoID = Producto.ID;
                        KX.EsProducto = !Producto.EsServicio;
                        KX.EstacionServicioID = IDEstacionServicio;
                        KX.SubEstacionID = IDSubEstacion;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;

                        if (KX.EsProducto)
                        {
                            KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                            KX.CantidadInicial = dk.CantidadInicial;
                            KX.CantidadSalida = dk.CantidadSalida;
                            KX.CostoSalida = dk.CostoSalida;
                            KX.CostoInicial = dk.CostoSalida;
                            KX.CostoFinal = dk.CostoSalida;
                            KX.CostoTotal = Decimal.Round(dk.CostoSalida * dk.CantidadSalida, 2, MidpointRounding.AwayFromZero);


                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;
                            KX.Precio = dk.Precio;
                            KX.PrecioTotal = dk.PrecioTotal;
                            KX.Descuento = dk.Descuento;

                            if (KX.CantidadSalida > KX.CantidadInicial)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("La cantidad a vender del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                return false;
                            }
                        }
                        else if (!KX.EsProducto)
                        {
                            KX.CantidadSalida = dk.CantidadSalida;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;
                            KX.Precio = dk.Precio;
                            KX.PrecioTotal = dk.PrecioTotal;
                            KX.Descuento = dk.Descuento;

                        }

                        db.Kardexes.InsertOnSubmit(KX);

                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                        
                            if (!Producto.ProductoClaseID.Equals(_IDClaseCombustible))
                            {
                                var AL = (from al in db.AlmacenProductos
                                          where al.ProductoID.Equals(Producto.ID)
                                            && al.AlmacenID.Equals(KX.AlmacenSalidaID)
                                          select al).ToList();

                                if (!AL.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(dk.AlmacenSalidaID));
                                    AP.Cantidad = KX.CantidadFinal;
                                }
                            }
                            else if (Producto.ProductoClaseID.Equals(_IDClaseCombustible))
                            {
                                var TP = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(dk.AlmacenSalidaID)
                                          select tp).ToList();

                                if (TP.Count() == 0) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                                {
                                    //-- CREAR NUEVO REGISTRO 
                                    Entidad.TanqueProducto T = new Entidad.TanqueProducto();
                                    T.ProductoID = Producto.ID;
                                    T.TanqueID = dk.AlmacenEntradaID;
                                    T.Cantidad = dk.CantidadEntrada;
                                    T.Costo = KX.CostoFinal;
                                    db.TanqueProductos.InsertOnSubmit(T);
                                }
                                else //-- SI HAY REGISTRO DE EXISTENCIA ACTUALIZAR CANTIDAD CON COMPRA
                                {
                                    Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(dk.AlmacenEntradaID));
                                    T.Cantidad = dk.CantidadEntrada + TP.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad;
                                    T.Costo = KX.CostoFinal;
                                }
                            }
                        //------------------------------------------------------------------------//

                        #endregion

                        //para que actualice los datos del registro
                        db.SubmitChanges();
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
                    if (M.Credito)
                    {

                        Entidad.Deudor D = new Entidad.Deudor();
                        D.ClienteID = M.ClienteID;
                        D.Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero);
                        D.MovimientoID = M.ID;
                        db.Deudors.InsertOnSubmit(D);
                        db.SubmitChanges();

                        ///PAGO DEL ANTICIPO
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
                    }


                    #endregion

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                    "Se registró la Venta: " + M.Numero, this.Name);

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.ClienteID = client.ID;
                    Parametros.General.AreaVentaID = IDArea;
                    Parametros.General.FechaVenta = FechaVenta.Date;

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    NextVenta = true;
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    //_ToPrint = true;
                    //IDPrint = M.ID;

                    

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
                    if (client != null & IDArea > 0)
                    {
                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                        string texto = "Factura de Venta " + client.Nombre + " Nro. " + Numero.ToString();
                        int i = 1;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = client.CuentaContableID,
                            Monto = Decimal.Round(Convert.ToDecimal(txtGrandTotal.Text), 2, MidpointRounding.AwayFromZero),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(txtGrandTotal.Text) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaVenta,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                                                
                        if (DetalleK.Sum(s => s.ImpuestoTotal) > 0)
                        {
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaIVA,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleK.Sum(s => s.ImpuestoTotal)), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(DetalleK.Sum(s => s.ImpuestoTotal)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                Fecha = FechaVenta,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;
                        }

                        List<Int32> IDVenta = new List<Int32>();
                        List<Int32> IDCosto = new List<Int32>();
                        List<Int32> IDInventario = new List<Int32>();

                        #region <<< DETALLE_COMPROBANTE >>>
                        DetalleK.ToList().ForEach(K =>
                            {
                                var area = from a in db.Areas
                                           join pc in db.ProductoClases on a.ID equals pc.AreaID
                                           join p in db.Productos on pc.ID equals p.ProductoClaseID
                                           where p.ID.Equals(K.ProductoID)
                                           select a;

                                #region <<< CUENTA_VENTA >>>
                                if (area.Count(a => !a.CuentaVentaID.Equals(0)) > 0)
                                {
                                    if (!IDVenta.Contains(area.First().CuentaVentaID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaVentaID,
                                            Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(K.PrecioTotal + K.Descuento)), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.PrecioTotal + K.Descuento)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                            Fecha = FechaVenta,
                                            Descripcion = texto,
                                            Linea = i,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDVenta.Add(area.First().CuentaVentaID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaVentaID)).First();
                                        comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.PrecioTotal + K.Descuento)), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.PrecioTotal + K.Descuento)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                    }
                                }
                                else
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));
                                    if (!IDVenta.Contains(producto.CuentaVentaID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaVentaID,
                                            Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(K.PrecioTotal + K.Descuento)), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.PrecioTotal + K.Descuento) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
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
                                        comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.PrecioTotal + K.Descuento)), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.PrecioTotal + K.Descuento) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                                    }
                                }

                                #endregion

                                #region <<< CUENTA_INVENTARIO >>>
                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                {
                                    if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaInventarioID,
                                            Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                            Fecha = FechaVenta,
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
                                        comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                    }
                                }
                                else
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));
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
                                if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                {
                                    if (!IDCosto.Contains(area.First().CuentaCostoID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaCostoID,
                                            Monto = Decimal.Round(Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                            Fecha = FechaVenta,
                                            Descripcion = texto,
                                            Linea = i,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDCosto.Add(area.First().CuentaCostoID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaCostoID)).First();
                                        comprobante.Monto += Decimal.Round(Math.Abs(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida)), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                                    }
                                }
                                else
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));
                                    if (!IDCosto.Contains(producto.CuentaCostoID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaCostoID,
                                            Monto = Decimal.Round(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
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
                                        comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoSalida * K.CantidadSalida) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
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
            MDI.CleanDialog(ShowMsg, NextVenta, RefreshMDI);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !NextVenta)
            {
                if (DetalleK.Count > 0 || txtReferencia.Text != "" || client != null)
                {
                    if (Parametros.General.DialogMsg("La Venta actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            client = null;
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

        //Selecciona el Cliente
        private void glkProvee_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkClient.EditValue) > 0)
                {
                    client = db.Clientes.SingleOrDefault(p => p.ID.Equals(IDCliente));

                    if (client != null)
                    {
                        client = db.Clientes.Single(p => p.ID.Equals(IDCliente));

                        if (!client.CuentaContableID.Equals(_ClienteAnticipo))
                        {

                            txtPlazo.Text = client.Plazo.ToString();
                            lbCliente.Items.Clear();
                            if (!client.ID.Equals(_ClienteVentaContadoID))
                                lbCliente.Items.Add("Límite de Crédito   |  C$  " + client.LimiteCredito.ToString("#,0.00"));

                            decimal Saldo = Convert.ToDecimal(db.GetSaldoCliente(client.ID, _FechaActual));
                            lbCliente.Items.Add("Saldo Actual          |  C$  " + Saldo.ToString("#,0.00"));

                            if (!client.ID.Equals(_ClienteVentaContadoID))
                            {
                                _Disponible = client.LimiteCredito - Saldo;
                                lbCliente.Items.Add("Disponible              |  C$  " + _Disponible.ToString("#,0.00"));
                            }

                            if (dateFechaFactura.EditValue != null && client != null)
                                FechaVencimiento = Convert.ToDateTime(dateFechaFactura.EditValue).AddDays(client.Plazo);
                            else
                                dateFechaVencimiento.EditValue = null;
                        }
                        else
                        {

                            txtPlazo.Text = client.Plazo.ToString();
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

                            if (dateFechaFactura.EditValue != null && client != null)
                                FechaVencimiento = Convert.ToDateTime(dateFechaFactura.EditValue).AddDays(client.Plazo);
                            else
                                dateFechaVencimiento.EditValue = null;

                          

                            //spLimite.Value = client.LimiteCredito;

                            //decimal Saldo = (obj.Count() > 0 ? Convert.ToDecimal(obj.Sum(s => s) * -1) : 0m);
                            //spSaldo.Value = Saldo;

                            //decimal _Disponible = client.LimiteCredito - Saldo;
                            //spDisponible.Value = _Disponible;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
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


            //-- Validar Columna de Existencia
            //--
            if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
            {
                if (view.GetRowCellValue(RowHandle, "CantidadInicial") != null)
                {
                    if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadInicial")) <= 0.00)
                    {
                        view.SetColumnError(view.Columns["CantidadInicial"], "No hay Existencia para Vender");
                        e.ErrorText = "No hay Existencia para Vender";
                        e.Valid = false;
                        Validate = false;
                    }
                }
                else
                {
                    view.SetColumnError(view.Columns["CantidadInicial"], "No Existe Existencia para Vender");
                    e.ErrorText = "No hay Existencia para Vender";
                    e.Valid = false;
                    Validate = false;
                }

                //-- Validar Columna de Almacen             
                if (view.GetRowCellValue(RowHandle, "AlmacenSalidaID") != null)
                {
                    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID")) == 0)
                    {
                        view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen / Tanque");
                        e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                        e.Valid = false;
                        Validate = false;
                    }
                }
                else
                {
                    view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Almacen / Tanque");
                    e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                    e.Valid = false;
                    Validate = false;
                }

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
                                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a vender sobrepasa la existencia");
                                    e.ErrorText = "La cantidad a vender sobrepasa la existencia";
                                    e.Valid = false;
                                    Validate = false;
                                }
                            }
                            else
                            {
                                view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a vender sobrepasa la existencia");
                                e.ErrorText = "La cantidad a vender sobrepasa la existencia";
                                e.Valid = false;
                                Validate = false;
                            }
                        }
                        else
                        {
                            view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a vender sobrepasa la existencia");
                            e.ErrorText = "La cantidad a vender sobrepasa la existencia";
                            e.Valid = false;
                            Validate = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a vender sobrepasa la existencia");
                        e.ErrorText = "La cantidad a vender sobrepasa la existencia";
                        e.Valid = false;
                        Validate = false;
                    }
                }
                else
                {
                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a vender sobrepasa la existencia");
                    e.ErrorText = "La cantidad a vender sobrepasa la existencia";
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
            if (e.Column == colQuantity)
            {
                if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
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
            }

                //--  Calcular las montos de las columnas de Impuestos y Subtotal
            if (e.Column == colDescuento || e.Column == colQuantity || e.Column == colPrecio)
                {
                    decimal vPrecio = 0, vCantidad = 1, vSubTotal = 0, vDescuento = 0, vVentaNeta = 0, vValorImpuesto = 0, vTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Precio") != null)
                        vPrecio = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Precio"));

                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) != Convert.ToDecimal("0.00"))
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Descuento") != null)
                        vDescuento = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Descuento"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "Descuento", 0);

                    //if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "PrecioTotal")) != Convert.ToDecimal("0.00"))
                    //    vSubTotal = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "PrecioTotal"));

                    vSubTotal = Decimal.Round((vPrecio * vCantidad), 2, MidpointRounding.AwayFromZero);

                    gvDetalle.SetRowCellValue(e.RowHandle, "PrecioTotal", vSubTotal);

                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "VentaNeta")) != Convert.ToDecimal("0.00"))
                        vVentaNeta = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "VentaNeta"));

                    if (client != null)
                    {
                        if (!client.ExentoIVA)
                        {
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo") != null)
                            {
                                if (!Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo")))
                                {
                                    vValorImpuesto = Decimal.Round((vVentaNeta * 0.15m), 2, MidpointRounding.AwayFromZero);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo") != null)
                        {
                            if (!Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo")))
                            {
                                vValorImpuesto = Decimal.Round((vVentaNeta * 0.15m), 2, MidpointRounding.AwayFromZero);
                                gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
                            }
                        }
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
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                    gvDetalle.SetRowCellValue(e.RowHandle, "EsProducto", !Producto.EsServicio);

                    gvDetalle.SetRowCellValue(e.RowHandle, "EsManejo", Producto.ExentoIVA);
                    //-- Unidad Principal     
                    gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);
                    //-- Aplica todos los almacenes
                    gvDetalle.SetRowCellValue(e.RowHandle, "DetalleCombustible", Producto.SiempreSalidaVentas.ToString());

                    if (Producto.ProductoClaseID.Equals(_ProductoClaseCombustible))
                    {
                        ////Tanques por combustible
                        //var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                        //ColAlmacen.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                        List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>();

                        listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Tanque"));

                        cboAlmacen.DataSource = listadoDisplay;
                        cboAlmacen.DisplayMember = "Display";
                        cboAlmacen.ValueMember = "ID";
                        gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);                        

                    }
                    else
                    {
                        if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
                        {
                            int IDAlma = 0;

                            listaAlmacenProducto = (from ap in db.AlmacenProductos
                                                    join a in db.Almacens.Where(o => listaAlmacen.Contains(o)) on ap.AlmacenID equals a.ID
                                                    //where p.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && (l.AplicaVentas || Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "DetalleCombustible")).Equals(true))
                                                    select ap).ToList();

                            var Almas = (from a in listaAlmacen
                                         join ap in listaAlmacenProducto on a.ID equals ap.AlmacenID
                                         //join p in db.Productos on ap.ProductoID equals p.ID
                                         where ap.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && (a.AplicaVentas || Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "DetalleCombustible")).Equals(true))
                                         select a).ToList();

                            if (Almas.Count() > 0)
                                IDAlma = Almas.First().ID;

                            ColAlmacen.OptionsColumn.AllowFocus = (Almas.Count() > 1 ? true : false);

                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", IDAlma);

                            decimal VCtoEntrada, vCtoFinal;
                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaVenta, out vCtoFinal, out VCtoEntrada);

                            var query = listaAlmacenProducto.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) & a.ProductoID.Equals(Producto.ID));

                            if (query.ToList().Count > 0)
                            {
                                decimal vPrecio = Decimal.Round(query.First().PrecioVenta, 6, MidpointRounding.AwayFromZero);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Precio", vPrecio);

                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCtoFinal);

                            }
                            else
                            {
                                Parametros.General.DialogMsg("El producto no tiene existencia en el almacen seleccionado.", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Precio", 0);
                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", 0);
                            }

                        }
                        else
                        {
                            decimal vPrecio = 0;
                            var objServicio = db.ServicioPrecio.FirstOrDefault(s => s.EstacionServicioID.Equals(IDEstacionServicio) && s.SubEstacionID.Equals(IDSubEstacion) && s.ServicioID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))));
                           if (objServicio != null)
                               vPrecio = Decimal.Round(objServicio.PrecioVenta, 6, MidpointRounding.AwayFromZero);
                            
                            gvDetalle.SetRowCellValue(e.RowHandle, "Precio", vPrecio);
                            gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", 0);
                        }
                    }


                    //-- Cantidad Inicial de 1
                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) == Convert.ToDecimal("0.00"))
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            //--
            #endregion

            #region <<< COSTO_ALMACEN>>>

            if (e.Column == ColAlmacen)
            {
                try
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                        {
                            if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")))
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") != null)
                                {
                                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")) > 0)
                                    {
                                        int Producto = Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"));
                                        
                                        var query = listaAlmacenProducto.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) & a.ProductoID.Equals(Producto));

                                        decimal vCantidadInicial = Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaVenta, false) + Parametros.General.SaldoKardexPost(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaVenta);

                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", vCantidadInicial);

                                        if (query.ToList().Count > 0 && Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) > 0)
                                        {
                                            gvDetalle.SetRowCellValue(e.RowHandle, "Precio", Decimal.Round(query.First().PrecioVenta, 6, MidpointRounding.AwayFromZero));
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);
                                        }
                                        else
                                        {
                                            Parametros.General.DialogMsg("El producto no tiene existencia en el almacen seleccionado.", Parametros.MsgType.warning);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                            gvDetalle.SetRowCellValue(e.RowHandle, "Precio", 0);
                                        }
                                    }
                                }
                            }
                        }                                
                    }

                    cboAlmacen.DataSource = listaAlmacen.ToList();

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            #endregion

            txtGrandTotal.Text = Convert.ToDecimal(DetalleK.Sum(s => s.PrecioTotal) + DetalleK.Sum(s => s.Descuento) + DetalleK.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
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

                        txtGrandTotal.Text = Convert.ToDecimal(DetalleK.Sum(s => s.PrecioTotal) + DetalleK.Sum(s => s.Descuento) + DetalleK.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
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

                        txtGrandTotal.Text = Convert.ToDecimal(DetalleK.Sum(s => s.PrecioTotal) + DetalleK.Sum(s => s.Descuento) + DetalleK.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");                       
                    }
                }
            }
        }  

        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!OnlyView || ValidarCampos(false))
                //{
                    if (Convert.ToDecimal(txtGrandTotal.Text) <= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            return;
                    }

                    if (client != null & IDArea > 0)
                    {
                        using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                        {
                            nf.DetalleCD = PartidasContable;
                            nf.Text = "Comprobante Contable de ventas";
                            nf.ShowDialog();
                        }
                    }
                //}
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

        private void txtRuc_Enter(object sender, EventArgs e)
        {
            txtReferencia.Focus();
            txtReferencia.Select();
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
            if (DetalleK.Count > 0 || dateFechaFactura.EditValue != null || dateFechaVencimiento.EditValue != null || txtReferencia.Text != "")
            {
                if (Parametros.General.DialogMsg("La venta actual tiene datos registrados. ¿Desea cancelar esta venta y realizar una nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    NextVenta = true;
                    RefreshMDI = false;
                    ShowMsg = false;
                    this.Close();
                }
            }                
        }

        private void dateFechaFactura_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (dateFechaFactura.EditValue != null)
                {
                    if (FechaVenta.Date > Convert.ToDateTime("01/01/2000").Date)
                    {
                        string texto = "";
                        EtAreas.Where(o => o.ID.Equals(IDArea)).ToList().ForEach(det =>
                        {
                            if (!Parametros.General.ValidateKardexMovemente(FechaVenta, db, IDEstacionServicio, IDSubEstacion, 9, det.AreaID))
                            {
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaVenta.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
                                texto += (String.IsNullOrEmpty(texto) ? det.Nombre : ", " + det.Nombre);
                            }
                        });

                        if (!String.IsNullOrEmpty(texto))
                        {
                            infBloqueo.SetError(lkArea, "Existen Áreas cerradas : " + texto, ErrorType.Warning);
                        }
                        else
                        {
                            infBloqueo.SetError(lkArea, "", ErrorType.None);

                            if (client != null)
                                FechaVencimiento = FechaVenta.AddDays(client.Plazo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
     
        private void txtGrandTotal_EditValueChanged(object sender, EventArgs e)
        {
            //if (_EsContado && Provee != null)
            //{
            //    decimal subtotal = Convert.ToDecimal(txtGrandTotal.Text);
            //    decimal costoTotal = DetalleK.Sum(s => s.CostoTotal);
            //    decimal resta = 0;

            //    if (Provee.AplicaRetencion)
            //    {
            //        decimal IR = (db.CuentaContables.Single(cc => cc.ID.Equals(Provee.ImpuestoRetencionID)).Porcentaje / 100m);
            //        spIR.Value = Decimal.Round((costoTotal * IR), 2, MidpointRounding.AwayFromZero);
            //        resta += Decimal.Round((costoTotal * IR), 2, MidpointRounding.AwayFromZero);
            //    }

            //    if (Provee.AplicaAlcaldia)
            //    {
            //        decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(231)).Porcentaje / 100m);
            //        spAlcaldia.Value = Decimal.Round((costoTotal * Alcaldia), 2, MidpointRounding.AwayFromZero);
            //        resta += Decimal.Round((costoTotal * Alcaldia), 2, MidpointRounding.AwayFromZero);
            //    }

            //    spTotal.Value = (subtotal - resta);
            //}
        }
        
        //OBTENER EL AREAID Y LOS PRODUCTOS
        private void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkArea.EditValue != null)
                {
                    if (!IDArea.Equals(0))
                    {
                        ValidateBloqueo((BaseEdit)sender, infBloqueo);
                        
                        if (listaAlmacen.Count(l =>l.AplicaVentas) > 0)
                        {
                            //--- Fill Combos Detalles --//
                            gridProductos.DataSource = null;
                            var ListaProductos =  from P in db.Productos
                                                  join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                       join A in db.Areas on C.AreaID equals A.ID
                                                       join AL in db.AlmacenProductos on P.ID equals AL.ProductoID
                                                       join ALM in db.Almacens.Where(o => listaAlmacen.Contains(o)) on AL.AlmacenID equals ALM.ID
                                                  where P.Activo.Equals(true) & A.Activo & C.Activo & A.AreaVentaID.Equals(IDArea) 
                                                  //& AL.Cantidad > 0 & (ALM.AplicaVentas || P.SiempreSalidaVentas)
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

                            var ListaServicos = from P in db.Productos
                                                 join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                 join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                 join A in db.Areas on C.AreaID equals A.ID
                                                 join SP in db.ServicioPrecio on P.ID equals SP.ServicioID
                                                 where P.Activo.Equals(true) & A.Activo & C.Activo & A.AreaVentaID.Equals(IDArea) & P.EsServicio & SP.EstacionServicioID.Equals(IDEstacionServicio) & SP.SubEstacionID.Equals(IDSubEstacion)
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

                            var ListaFinal = ListaProductos.Union(ListaServicos).ToList();

                            //if (ListaProductos.Count(l => l.SiempreSalidaVentas.Equals(true)) > 0 && ListaProductos.GroupBy(g => g.AlmacenID).Count() > 1)
                            //{
                            //    listaAlmacen = from al in db.Almacens
                            //                   join lp in ListaProductos.GroupBy(g => g.AlmacenID) on al.ID equals lp.Key
                            //                   where al.Activo & al.EstacionServicioID.Equals(IDEstacionServicio) & al.SubEstacionID.Equals(IDSubEstacion)
                            //                   select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };

                            //    UnAlmacen = (listaAlmacen.ToList().Count > 1 ? false : true);
                            //    ColAlmacen.OptionsColumn.ReadOnly = UnAlmacen;
                            //    ColAlmacen.OptionsColumn.AllowFocus = !UnAlmacen;

                            //    cboAlmacen.DataSource = listaAlmacen.ToList();
                            //    cboAlmacen.DisplayMember = "Display";
                            //    cboAlmacen.ValueMember = "ID";
                            //}

                            //if (ListaProductos.Count(l => l.AplicaDescuento) > 0)
                            //{
                            //    colDescuento.OptionsColumn.ReadOnly = false;
                            //    colDescuento.OptionsColumn.AllowFocus = true;
                            //}
                            //else
                            //{
                            //    colDescuento.OptionsColumn.ReadOnly = true;
                            //    colDescuento.OptionsColumn.AllowFocus = false;
                            //}

                            gridProductos.DataSource = ListaFinal;
                            gridProductos.DisplayMember = "Display";
                            gridProductos.ValueMember = "ID";

                            var AreaVenta = db.AreaVentas.Single(a => a.ID.Equals(IDArea));

                            ////Cliente
                            glkClient.Properties.DataSource = null;
                            glkClient.Properties.DataSource = (from c in db.Clientes
                                                               where c.Activo && ((AreaVenta.AplicaCredito && c.AplicaCreditoLubricentro) || c.ID.Equals(_ClienteVentaContadoID)) &&
                                                               (db.ClienteEstacions.Any(es => es.EstacionServicioID.Equals(IDEstacionServicio) && es.ClienteID.Equals(c.ID)))
                                                               select new { c.ID, c.Codigo, c.Nombre, Display = c.Codigo + " | " + c.Nombre }).OrderBy(o => o.Codigo);
                      

                        }
                        else
                            Parametros.General.DialogMsg("No hay almacen asignado para la venta", Parametros.MsgType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //VALIDAR BLOQUEO COMPONENTE LKAREA
        private void ValidateBloqueo(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider)
        {
            try
            {
                if (dateFechaFactura.EditValue != null)
                {
                    if (FechaVenta.Date > Convert.ToDateTime("01/01/2000").Date)
                    {
                        string texto = "";
                        EtAreas.Where(o => o.ID.Equals(IDArea)).ToList().ForEach(det =>
                            {
                                if (!Parametros.General.ValidateKardexMovemente(FechaVenta, db, IDEstacionServicio, IDSubEstacion, 9, det.AreaID))
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaVenta.Date.ToShortDateString() + " y Área " + det.Nombre, Parametros.MsgType.warning);
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

        private void gvDetalle_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column == ColAlmacen)
            {
                try
                {
                    if (gvDetalle.GetRowCellValue(gvDetalle.FocusedRowHandle, "DetalleCombustible") != null)
                    {
                        if (Convert.ToBoolean(gvDetalle.GetRowCellValue(gvDetalle.FocusedRowHandle, "DetalleCombustible")).Equals(true))
                        {
                            ////Almacen GRID
                            cboAlmacen.DataSource = from l in db.Almacens.Where(o => listaAlmacen.Contains(o))
                                                    join a in db.AlmacenProductos on l.ID equals a.AlmacenID
                                                    join p in db.Productos on a.ProductoID equals p.ID
                                                    where p.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(gvDetalle.FocusedRowHandle, "ProductoID"))) & a.Cantidad > 0
                                                    select l;

                        }
                        else
                        {
                            //Almacen GRID
                            cboAlmacen.DataSource = listaAlmacen.Where(l => l.AplicaVentas).ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        private void cboAlmacen_Popup(object sender, EventArgs e)
        {
            
        }

        private void cboAlmacen_Leave(object sender, EventArgs e)
        {
            try
            {
                cboAlmacen.DataSource = listaAlmacen.ToList();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void cboAlmacen_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
           
        }
      
        private void lkArea_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0)
            {
                Parametros.General.DialogMsg("La venta tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void glkClient_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0)
            {
                Parametros.General.DialogMsg("La venta tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            txtGrandTotal.Text = Convert.ToDecimal(DetalleK.Sum(s => s.PrecioTotal) + DetalleK.Sum(s => s.Descuento) + DetalleK.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
        }

        private void gridDetalle_Leave(object sender, EventArgs e)
        {
            txtGrandTotal.Text = Convert.ToDecimal(DetalleK.Sum(s => s.PrecioTotal) + DetalleK.Sum(s => s.Descuento) + DetalleK.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
        }

        private void dateFechaFactura_Validated(object sender, EventArgs e)
        {
            if (dateFechaFactura.EditValue != null)
            {
                if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
                {
                    _TipoCambio = 0;
                    dateFechaFactura.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
                }
                else
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaVenta.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaVenta.Date)).First().Valor : 0m);
            }
        }
               
        private void dateFechaFactura_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (dateFechaFactura.EditValue != null)
            {
                if (DetalleK.Count > 0)
                {
                    Parametros.General.DialogMsg("La venta tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }
            }
        }

        #endregion

       

    }
}