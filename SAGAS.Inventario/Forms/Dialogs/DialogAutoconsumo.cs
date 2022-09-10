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
    public partial class DialogAutoconsumo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormAutoConsumo MDI;
        internal Entidad.ResumenDia EntidadRD;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        private List<Parametros.ListIdTidDisplayPriceValue> listaProductoRD;
        private int _IDAutoconsumo;
        private int _IDDonacion;
        private int IDPrint = 0;
        internal bool _EsObviar = false;
        private decimal _Disponible = 0;
        internal bool Next = false;
        private decimal _TipoCambio = 0;
        private int _Cuenta = 0;
        private int _OficinaCentralID = 0;
        private int _ClaseCombustible;

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Referencia
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
        }

        private string Placa
        {
            get { return txtPlaca.Text; }
            set { txtPlaca.Text = value; }
        }

        private string Conductor
        {
            get { return txtConductor.Text; }
            set { txtConductor.Text = value; }
        }

        private string Comentario
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFechaVenta.EditValue); }
            set { dateFechaVenta.EditValue = value; }
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

        public DialogAutoconsumo(int UserID, bool _editando)
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

                _IDAutoconsumo = Parametros.Config.ExtracionAutoconsumoID();
                _IDDonacion = Parametros.Config.ExtracionDonacionesID();
                _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();
               
                _OficinaCentralID = db.EstacionServicios.Single(es => es.ID.Equals(12)).ID;
                lkCeco.Properties.DataSource = from c in db.CentroCostos.Where(o => o.Activo && o.CentroCostoPorEstacions.Any(es => es.EstacionID.Equals(_OficinaCentralID))) 
                                               select new {c.ID, Display = c.Codigo + " | " + c.Nombre}; 

                //---LLenar Almacenes ---//
                listaTanque = from t in db.Tanques
                              join p in db.Productos on t.ProductoID equals p.ID
                              where t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                              select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre + " | " + p.Nombre };

                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                this.lbResumen.Width = 350;

                txtNumero.Text = number.ToString("000000000");
                //Almacen GRID
                cboAlmacenSalida.DataSource = listaTanque.ToList();

                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//              

                dateFechaVenta.EditValue = null;// Convert.ToDateTime(db.GetDateServer());

                rgTipos.SelectedIndex = 0;

                radioGroup1_SelectedIndexChanged(null, null);

                rgTipos.Properties.Items[2].Enabled = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "rgPermitirSalidaCombustible");

                this.bdsDetalle.DataSource = this.DetalleEM;
                
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
            Parametros.General.ValidateEmptyStringRule(txtReferencia, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(dateFechaVenta, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && i.MovimientoTipoID.Equals(25) && !i.Anulado && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion)
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
                if (txtNumero.Text == "" || txtReferencia.Text == "" || memoComentario.Text == "" || dateFechaVenta.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                    return false;
                }

                if (_TipoCambio.Equals(0))
                {
                    Parametros.General.DialogMsg("El Tipo de Cambio es 0 (cero), favor revisar la Fecha de la Salida del Combustible.", Parametros.MsgType.warning);
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

                if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
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

                    if (_Cuenta.Equals(0))
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Distribución de Extracción : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                        return false;
                    }

                    if (chkOficinaCentral.Checked)
                    {
                        if (lkCeco.EditValue == null)
                        {
                            Parametros.General.DialogMsg("Debe seleccionar un Centro de Costo para Oficina Central.", Parametros.MsgType.warning);
                            return false;
                        }
                    }

                    if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                    {
                        Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                        return false;
                    }

                    //BLOQUEO PARA COMBUSTIBLE
                    List<int> PAreas = (from p in db.Productos
                                        join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                        where EM.Select(s => s.ProductoID).Contains(p.ID) && pc.ID.Equals(_ClaseCombustible)
                                        group pc by pc.AreaID into gr
                                        select gr
                                       ).Select(s => s.Key).ToList();

                    if (PAreas.Count > 0)
                    {
                        if (!Parametros.General.ValidateKardexMovemente(Fecha, db, IDEstacionServicio, IDSubEstacion, 24, 0))
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Fecha.ToShortDateString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }

                }

                return true;
            }
            catch { return false; }
        }

        private bool GuardarTemporal()
        {
            if (!ValidarCampos(false))
                return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M = new Entidad.Movimiento();
                    M.MovimientoTipoID = 25;
                    M.UsuarioID = UsuarioID;
                    M.NumeroPlaca = Placa;
                    M.Entregado = Conductor;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Fecha;
                    M.MonedaID = Parametros.Config.MonedaPrincipal();
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal));
                    M.MontoMonedaSecundaria = Decimal.Round((DetalleEM.Sum(s => s.CostoTotal) / M.TipoCambio), 2, MidpointRounding.AwayFromZero);

                    int number = 1;
                    if (db.TemporalMovimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.TemporalMovimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    M.Numero = number;

                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = memoComentario.Text;
                    M.Referencia = Referencia;

                    if (!_EsObviar)
                    {
                        M.ResumenDiaID = EntidadRD.ID;
                        M.ExtraxionID = (rgTipos.SelectedIndex.Equals(0) ? _IDAutoconsumo : _IDDonacion);

                        if (lkConcepto.EditValue != null)
                            M.ExtraxionConceptoID = Convert.ToInt32(lkConcepto.EditValue);
                    }

                    db.Movimientos.InsertOnSubmit(M);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                    "Se registró la Salida de Combustible Temporal: " + M.Numero, this.Name);

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    foreach (var dk in DetalleEM)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                        {
                            trans.Rollback();
                            Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
                            return false;
                        }

                        Entidad.TemporalKardex KX = new Entidad.TemporalKardex();

                        KX.MovimientoTemporalID = M.ID;
                        KX.ProductoID = Producto.ID;
                        KX.UnidadMedidaID = dk.UnidadMedidaID;
                        KX.Fecha = M.FechaRegistro;
                        KX.EstacionServicioID = IDEstacionServicio;
                        KX.SubEstacionID = IDSubEstacion;
                        KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                        KX.CantidadSalida = dk.CantidadSalida;
                        KX.CostoSalida = dk.CostoSalida;
                        KX.CostoTotal = dk.CostoTotal;

                        KX.CantidadInicial = dk.CantidadEntrada;

                        KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                        if (KX.CantidadSalida > dk.CantidadInicial)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                            return false;
                        }

                        db.TemporalKardexes.InsertOnSubmit(KX);
                                                
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

                        db.TemporalComprobanteContables.InsertOnSubmit(new Entidad.TemporalComprobanteContable { 
                            MovimientoTemporalID = M.ID, CentroCostoID = linea.CentroCostoID, CuentaContableID = linea.CuentaContableID, 
                            Descripcion = linea.Descripcion, EstacionServicioID = linea.EstacionServicioID,
                         Fecha = linea.Fecha, Linea = linea.Linea, Monto = linea.Monto, TipoCambio = linea.TipoCambio, MontoMonedaSecundaria = linea.MontoMonedaSecundaria,
                          SubEstacionID = linea.SubEstacionID});
                        db.SubmitChanges();
                    });

                    db.SubmitChanges();

                    #endregion

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

        private bool Guardar()
        {
            if (!ValidarCampos(false))
                return false;
            
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M = new Entidad.Movimiento();
                    M.MovimientoTipoID = 25;
                    M.UsuarioID = UsuarioID;
                    M.NumeroPlaca = Placa;
                    M.Entregado = Conductor;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Fecha;
                    M.MonedaID = Parametros.Config.MonedaPrincipal();
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Math.Abs(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)));
                    M.MontoMonedaSecundaria = Math.Abs(Decimal.Round((DetalleEM.Sum(s => s.CostoTotal) / M.TipoCambio), 2, MidpointRounding.AwayFromZero));

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(25)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    M.Numero = number;

                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = memoComentario.Text;
                    M.Referencia = Referencia;
                    M.Finalizado = true;

                    if (!_EsObviar)
                    {
                        M.ResumenDiaID = EntidadRD.ID;
                        M.ExtraxionID = (rgTipos.SelectedIndex.Equals(0) ? _IDAutoconsumo : _IDDonacion);
                    }

                    if (lkConcepto.EditValue != null)
                        M.ExtraxionConceptoID = Convert.ToInt32(lkConcepto.EditValue);

                    db.Movimientos.InsertOnSubmit(M);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                    "Se registró la Salida de Combustible: " + M.Numero, this.Name);

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    foreach (var dk in DetalleEM)
                    {
                        var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                        if (DetalleEM.Count(x => x.ProductoID.Equals(dk)) > 1)
                        {
                            trans.Rollback();
                            Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " ya existe en la lista.", Parametros.MsgType.warning);
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
                        KX.CostoTotal = dk.CostoTotal;

                        //var TP = (from tp in db.TanqueProductos
                        //          where tp.ProductoID.Equals(Producto.ID)
                        //          && tp.TanqueID.Equals(KX.AlmacenSalidaID)
                        //          select tp).ToList();

                        ////-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                        ////-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                        //if (TP.Count() == 0)
                        //{
                        //    KX.CantidadInicial = 0;
                        //}
                        //else
                        //    KX.CantidadInicial = TP.Single(q => q.ProductoID.Equals(dk.ProductoID)).Cantidad;

                        KX.CantidadInicial = dk.CantidadEntrada;

                        KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                        if (!_EsObviar)
                        {
                            if (KX.CantidadSalida > dk.CantidadInicial)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                return false;
                            }
                        }

                        if (KX.CantidadSalida > dk.CantidadEntrada)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                            return false;
                        }

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

        private decimal Costo(int IDProd)
        {
            try
            {
                var obj = (from k in db.Kardexes
                           join m in db.Movimientos on k.MovimientoID equals m.ID
                           where k.ProductoID.Equals(IDProd) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
                             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                           select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                if (obj.Count > 0)
                    return obj.First().CostoEntrada;
                else
                    return 0;
            }
            catch { return 0; }
        }

        private List<Entidad.ComprobanteContable> PartidasContable
        {
            get
            {
                try
                {
                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    int i = 1;

                    if (!chkOficinaCentral.Checked)
                    {
                        #region <<< SALIDA DIRECTA >>>
                        
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = _Cuenta,
                            Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = Fecha,
                            Descripcion = "Salida de Combustible Nro.: " + Numero + " " + rgTipos.Properties.Items[rgTipos.SelectedIndex].Description + " | Ref. " + Referencia,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion,
                            CentroCostoID = db.CuentaContables.Single(cc => cc.ID.Equals(_Cuenta)).CecoID

                        });
                        i++;

                        List<Int32> IDInventario = new List<Int32>();

                        DetalleEM.ToList().ForEach(K =>
                            {
                                var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));
                                if (!IDInventario.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Salida de Combustible Nro.: " + Numero + " " + rgTipos.Properties.Items[rgTipos.SelectedIndex].Description + " | Ref. " + Referencia,
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
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += -Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            });

                        #endregion
                    }
                    else if (chkOficinaCentral.Checked)
                    {
                        #region <<< SALIDA TEMPORAL >>>

                         int loop = 0;

                         while (loop < 2)
                         {
                             CD.Add(new Entidad.ComprobanteContable
                             {
                                 CuentaContableID = _Cuenta,
                                 Monto = Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero),
                                 TipoCambio = _TipoCambio,
                                 MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                 Fecha = Fecha,
                                 Descripcion = "Salida de Combustible Nro.: " + Numero + " " + rgTipos.Properties.Items[rgTipos.SelectedIndex].Description + " | Ref. " + Referencia,
                                 Linea = i,
                                 EstacionServicioID = (loop.Equals(0) ? IDEstacionServicio : _OficinaCentralID),
                                 SubEstacionID = (loop.Equals(0) ? IDSubEstacion : 0),
                                 CentroCostoID = (loop.Equals(0) ? db.CuentaContables.Single(cc => cc.ID.Equals(_Cuenta)).CecoID : Convert.ToInt32(lkCeco.EditValue))

                             });
                             i++;

                             if (loop.Equals(0))
                             {
                                 List<Int32> IDInventario = new List<Int32>();

                                 DetalleEM.ToList().ForEach(K =>
                                 {
                                     var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));
                                     if (!IDInventario.Contains(producto.CuentaInventarioID))
                                     {
                                         CD.Add(new Entidad.ComprobanteContable
                                         {
                                             CuentaContableID = producto.CuentaInventarioID,
                                             Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                             TipoCambio = _TipoCambio,
                                             MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                             Fecha = Fecha,
                                             Descripcion = "Salida de Combustible Nro.: " + Numero + " " + rgTipos.Properties.Items[rgTipos.SelectedIndex].Description + " | Ref. " + Referencia,
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
                                         comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                         comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                     }
                                 });
                             }
                             else
                             {
                                 CD.Add(new Entidad.ComprobanteContable
                                 {
                                     CuentaContableID = db.EstacionServicios.Single(es => es.ID.Equals(IDEstacionServicio)).CuentaInternaActivo,
                                     Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                                     TipoCambio = _TipoCambio,
                                     MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(DetalleEM.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                     Fecha = Fecha,
                                     Descripcion = "Salida de Combustible Nro.: " + Numero + " " + rgTipos.Properties.Items[rgTipos.SelectedIndex].Description + " | Ref. " + Referencia,
                                     Linea = i,
                                     EstacionServicioID = _OficinaCentralID,
                                     SubEstacionID = 0,
                                     CentroCostoID = 0

                                 });
                                 i++;
                             }

                             loop++;

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
            if (!chkOficinaCentral.Checked)
            {
                if (!Guardar()) return;
            }
            else if (chkOficinaCentral.Checked)
            {
                if (!GuardarTemporal()) return;
            }

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
                if (DetalleEM.Count > 0 || txtReferencia.Text != "")
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
                rgTipos.Enabled = false;
            else
                rgTipos.Enabled = true;

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
                if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadSalida")) <= 0.000)
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
            if (!_EsObviar)
            {
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
                }
            }

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
                        view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
                        e.ErrorText = "La cantidad a despachar sobrepasa la existencia";
                        e.Valid = false;
                        Validate = false;
                    }
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
                try
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
                                        Parametros.General.DialogMsg("La cantidad de salida sobrepasa la existencia del arqueo", Parametros.MsgType.warning);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                    }
                                }
                            }
                        }
                    }

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                    {
                        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")).Equals(0))
                        {
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada") != null)
                            {
                                if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")).Equals(0))
                                {
                                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) > Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")))
                                    {
                                        Parametros.General.DialogMsg("La cantidad de salida sobrepasa la existencia en el tanque", Parametros.MsgType.warning);
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 0);
                                    }
                                }
                            }
                        }
                    }

                    decimal vCosto = 0, vCantidad = 0, vCostoTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida") != null)
                        vCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoSalida"));

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida") != null)
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));

                    vCostoTotal = Decimal.Round(vCosto * vCantidad, 2, MidpointRounding.AwayFromZero);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);

                    //if (vCantidad > 0)
                    //    gvDetalle.RefreshData();

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            //--  Calcular las montos de la Venta
            //if (e.Column == colQuantity)
            //{               
                    

                    //if (gvDetalle.GetRowCellValue(e.RowHandle, "Precio") != null)
                    //    vPrecio = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Precio"));

                    //if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida")) != Convert.ToDecimal("0.00"))
                    //    vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadSalida"));
                    //else
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);

                    //if (chkOficinaCentral.Checked)
                    //{
                    //    vDescuento = Decimal.Round(vCantidad * 0.26m, 2, MidpointRounding.AwayFromZero);
                    //    gvDetalle.SetRowCellValue(e.RowHandle, "Descuento", vDescuento);
                    //}

                    //vSubTotal = Decimal.Round((vPrecio * vCantidad), 2, MidpointRounding.AwayFromZero);
                    //gvDetalle.SetRowCellValue(e.RowHandle, "SubTotal", vSubTotal);

                    //vTotal = vSubTotal - vDescuento;
                    //gvDetalle.SetRowCellValue(e.RowHandle, "PrecioTotal", vTotal);


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
                
            //}
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

                    List<Parametros.ListIdDisplay> TCombustible;

                    if (!_EsObviar)
                    {
                        TCombustible = (from t in db.Tanques
                                        where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                        && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                        select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre }).ToList();
                    }
                    else
                    {
                        TCombustible = (from t in db.Tanques
                                        where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                        //&& listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                        select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre }).ToList();
                    
                    }
                    ColAlmacenSalida.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                    cboAlmacenSalida.DataSource = TCombustible;//lkAlmacen.Properties.DataSource;
                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", TCombustible.First().ID);

                    //var menos = (from k in db.Kardexes
                    //             join m in db.Movimientos on k.MovimientoID equals m.ID
                    //             where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(25) && k.ProductoID.Equals(Producto.ID)
                    //             && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) &&
                    //             (radioGroup1.SelectedIndex.Equals(0) && (m.ExtraxionID.Equals(_IDAutoconsumo) && m.ExtraxionConceptoID.Equals(Convert.ToInt32(lkConcepto.EditValue))) || (radioGroup1.SelectedIndex.Equals(1) && m.ExtraxionID.Equals(_IDDonacion)))
                    //             group k by new { k.ProductoID } into gr
                    //             select new
                    //             {
                    //                 ID = gr.Key.ProductoID,
                    //                 Value = gr.Sum(s => s.CantidadSalida)
                    //             }).ToList();

                    
                    decimal Valor = 0;
                    if (!_EsObviar)
                    Valor = Decimal.Round(listaProductoRD.Where(l => l.ID.Equals(Producto.ID) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")))).Sum(s => s.Value), 3, MidpointRounding.AwayFromZero);
                    //Valor = Decimal.Round(listaProductoRD.Where(l => l.ID.Equals(Producto.ID) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")))).Sum(s => s.Value) - menos.Sum(s => s.Value), 3, MidpointRounding.AwayFromZero);

                    decimal VCtoEntrada, vCtoFinal;
                    Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, Fecha, out vCtoFinal, out VCtoEntrada);

                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCtoFinal);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false));


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
                                var Producto = db.Productos.Select(o => new { o.ID } ).Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));
                                List<Parametros.ListIdDisplay> TCombustible;

                                if (!_EsObviar)
                                {
                                    TCombustible = (from t in db.Tanques
                                                    where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                                    && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                                    select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre }).ToList();
                                }
                                else
                                {
                                    TCombustible = (from t in db.Tanques
                                                    where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                                    //&& listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                                    select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre }).ToList();

                                }


                                cboAlmacenSalida.DataSource = TCombustible;

                                if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).Equals(0))
                                {
                                    Parametros.General.DialogMsg("El Tanque seleccionado no contiene este producto", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                }
                                else
                                {
                                    decimal Valor = 0;
                                     
                                    if (!_EsObviar)
                                    {
                                        var menos = (from k in db.Kardexes
                                                     join m in db.Movimientos on k.MovimientoID equals m.ID
                                                     where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(25) && k.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))
                                                     && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                                     group k by new { k.ProductoID } into gr
                                                     select new
                                                     {
                                                         ID = gr.Key.ProductoID,
                                                         Value = gr.Sum(s => s.CantidadSalida)
                                                     }).ToList();


                                        Valor = Decimal.Round(listaProductoRD.Where(l => l.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")))).Sum(s => s.Value) - menos.Sum(s => s.Value), 3, MidpointRounding.AwayFromZero);
                                    }

                                    decimal VCtoEntrada, vCtoFinal;
                                    Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")), 0, Fecha, out vCtoFinal, out VCtoEntrada);

                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Valor);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoSalida", vCtoFinal);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")), Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), Fecha, false));

                                    if (listaProductoRD != null)
                                    {
                                        var query = listaProductoRD.Where(l => l.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))));

                                        if (query != null)
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", query.Sum(s => s.Value));
                                        else
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                    }
                                    else
                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
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
                if (ValidarCampos(false))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
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
                
        private void chkDescuento_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgTipos.SelectedIndex.Equals(0))
            {
                _EsObviar = false;
                chkOficinaCentral.Enabled = true;
                var Extraxion = db.ExtracionConceptos.Where(ex => ex.IDExtracionPago.Equals(_IDAutoconsumo));

                if (Extraxion.Count() > 0)
                {
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.lkConcepto.Properties.DataSource = null;
                    this.lkConcepto.Properties.DataSource = Extraxion;
                    this.lkConcepto.EditValue = null;
                    _Cuenta = 0;
                }
                else
                {
                    this.lkConcepto.EditValue = null;
                    this.lkConcepto.Properties.DataSource = null;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _Cuenta = db.ExtracionPagos.Single(ex => ex.ID.Equals(_IDAutoconsumo)).CuentaContableID;
                }
            }
            else if (rgTipos.SelectedIndex.Equals(1))
            {
                chkOficinaCentral.Checked = false;
                chkOficinaCentral.Enabled = false;
                _EsObviar = false;
                var Extraxion = db.ExtracionConceptos.Where(ex => ex.IDExtracionPago.Equals(_IDDonacion));
                
                if (Extraxion.Count() > 0)
                {
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.lkConcepto.Properties.DataSource = null;
                    this.lkConcepto.Properties.DataSource = Extraxion;
                    this.lkConcepto.EditValue = null;
                    _Cuenta = 0;
                }
                else
                {
                    this.lkConcepto.EditValue = null;
                    this.lkConcepto.Properties.DataSource = null;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _Cuenta = db.ExtracionPagos.Single(ex => ex.ID.Equals(_IDDonacion)).CuentaContableID;
                }
            }
            else
            {
                chkOficinaCentral.Checked = false;
                chkOficinaCentral.Enabled = false;
                _EsObviar = true;

                var Extraxion = db.ExtracionConceptos.Where(ex => ex.IDExtracionPago.Equals(_IDAutoconsumo));

                if (Extraxion.Count() > 0)
                {
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.lkConcepto.Properties.DataSource = null;
                    this.lkConcepto.Properties.DataSource = Extraxion;
                    this.lkConcepto.EditValue = null;
                    _Cuenta = 0;
                }
                else
                {
                    this.lkConcepto.EditValue = null;
                    this.lkConcepto.Properties.DataSource = null;
                    this.layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _Cuenta = db.ExtracionPagos.Single(ex => ex.ID.Equals(_IDAutoconsumo)).CuentaContableID;
                }
            }

            dateFechaVenta_Validated(dateFechaVenta, null);
        }

        //Carga Data del Resumen
        private void dateFechaVenta_Validated(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);

                if (dateFechaVenta.EditValue != null)
                {
                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
                    EntidadRD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDEstacionServicio) && r.SubEstacionID.Equals(IDSubEstacion) && r.FechaInicial.Date.Equals(Fecha.Date)).FirstOrDefault();

                    if (EntidadRD != null && !_EsObviar)
                    {
                        layoutControlGroupRD.Text = "Resumen Arqueo Nro: " + EntidadRD.Numero.ToString() + " | Litros Restantes";

                        if (lkConcepto.Properties.DataSource == null && lkConcepto.EditValue == null)
                        {
                            listaProductoRD = null;

                            listaProductoRD = (from ape in db.ArqueoProductoExtracions
                                               join ap in db.ArqueoProductos on ape.ArqueoProductoID equals ap.ID
                                               join p in db.Productos.OrderBy(o => o.Codigo) on ap.ProductoID equals p.ID
                                               join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                               join tn in db.Tanques on ap.TanqueID equals tn.ID
                                               join t in db.Turnos on ai.TurnoID equals t.ID
                                               join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
                                               where rd.ID.Equals(EntidadRD.ID) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial)) && 
                                               (rgTipos.SelectedIndex.Equals(0) && (ape.ExtracionID.Equals(_IDAutoconsumo)) || (rgTipos.SelectedIndex.Equals(1) && ape.ExtracionID.Equals(_IDDonacion)))
                                               group ape by new { ap.ProductoID, ap.TanqueID, Nombre = p.Nombre + " => " + tn.Nombre, ap.Precio } into gr
                                               select new Parametros.ListIdTidDisplayPriceValue
                                               {
                                                   ID = gr.Key.ProductoID,
                                                   TID = gr.Key.TanqueID,
                                                   Display = gr.Key.Nombre,
                                                   Price = gr.Key.Precio,
                                                   Value = gr.Sum(s => s.Valor)
                                               }).ToList();

                            var menos = (from k in db.Kardexes
                                         join m in db.Movimientos on k.MovimientoID equals m.ID
                                         where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(25)
                                         && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                         && m.ExtraxionID.Equals((rgTipos.SelectedIndex.Equals(0) ? _IDAutoconsumo : _IDDonacion))
                                         && m.ExtraxionConceptoID.Equals(lkConcepto.EditValue != null ? Convert.ToInt32(lkConcepto.EditValue) : 0)
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

                                lbResumen.Items.Add(item.Display + " | " + item.Value.ToString("#,0.000") + " Litros");
                            });

                            //--- Fill Combos Detalles --//
                            gridProductos.DataSource = null;
                            gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                            gridProductos.DataSource = from P in db.Productos
                                                       join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                       && listaProductoRD.Select(s => s.ID).Contains(P.ID)
                                                       select new
                                                       {
                                                           P.ID,
                                                           P.Codigo,
                                                           P.Nombre,
                                                           UmidadName = U.Nombre,
                                                           Display = P.Codigo + " | " + P.Nombre
                                                       };
                        }
                        else
                        {
                            lbResumen.Items.Clear();
                            gridProductos.DataSource = null;
                            lkConcepto_EditValueChanged(null, null);
                        }
                    }
                    else
                    {
                        if (!_EsObviar)
                        {
                            Parametros.General.DialogMsg("La fecha seleccionada no tiene resumen del día.", Parametros.MsgType.warning);
                            dateFechaVenta.EditValue = null;
                            layoutControlGroupRD.Text = "Resumen Arqueo";
                            lbResumen.Items.Clear();
                            gridProductos.DataSource = null;
                        }
                        else
                        {
                            lbResumen.Items.Clear();
                            //--- Fill Combos Detalles --//
                            gridProductos.DataSource = null;
                            gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                            gridProductos.DataSource = from P in db.Productos
                                                       join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       join T in db.Tanques on P.ID equals T.ProductoID
                                                       where P.Activo && T.Activo && T.EstacionServicioID.Equals(IDEstacionServicio) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                       group P by new { P.ID, P.Codigo, P.Nombre, Unidad = U.Nombre } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombre,
                                                           UmidadName = gr.Key.Unidad,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                       };

                        }
                    }
                }
                else
                {
                    _TipoCambio = 0;
                    if (lkConcepto.EditValue != null)
                        lkConcepto_EditValueChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Carga los datos del Concepto Contable por Autoconsumo
        private void lkConcepto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkConcepto.EditValue != null && dateFechaVenta.EditValue != null)
                {
                    if (!_EsObviar)
                    {
                        listaProductoRD = null;
                        _Cuenta = db.ExtracionConceptos.Single(ex => ex.ID.Equals(Convert.ToInt32(lkConcepto.EditValue))).CuentaContableID;
                        listaProductoRD = (from ape in db.ArqueoProductoExtracions
                                           //join apec in db.ArqueoProductoExtracionConceptos on ape equals apec.ArqueoProductoExtracion
                                           join ap in db.ArqueoProductos on ape.ArqueoProductoID equals ap.ID
                                           join p in db.Productos.OrderBy(o => o.Codigo) on ap.ProductoID equals p.ID
                                           join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
                                           join tn in db.Tanques on ap.TanqueID equals tn.ID
                                           join t in db.Turnos on ai.TurnoID equals t.ID
                                           join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
                                           where rd.ID.Equals(EntidadRD.ID) &&
                                           (rgTipos.SelectedIndex.Equals(0) && (ape.ExtracionID.Equals(_IDAutoconsumo)) || (rgTipos.SelectedIndex.Equals(1) && ape.ExtracionID.Equals(_IDDonacion)))
                                           && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
                                           // && apec.ExtracionConceptoID.Equals(Convert.ToInt32(lkConcepto.EditValue))
                                           group ape by new { ape.ID, ap.ProductoID, ap.TanqueID, Nombre = p.Nombre + " => " + tn.Nombre, ap.Precio, LitrosXConcepto = (ape.ArqueoProductoExtracionConceptos.Count(apec => apec.ExtracionConceptoID.Equals(Convert.ToInt32(lkConcepto.EditValue))) > 0 ? ape.ArqueoProductoExtracionConceptos.Where(apec => apec.ExtracionConceptoID.Equals(Convert.ToInt32(lkConcepto.EditValue))).Sum(s => s.Litros) : 0m) } into gr
                                           select new Parametros.ListIdTidDisplayPriceValue
                                           {
                                               ID = gr.Key.ProductoID,
                                               TID = gr.Key.TanqueID,
                                               Display = gr.Key.Nombre,
                                               Price = gr.Key.Precio,
                                               Value = gr.Key.LitrosXConcepto
                                           }).ToList();

                        var menos = (from k in db.Kardexes
                                     join m in db.Movimientos on k.MovimientoID equals m.ID
                                     where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(25)
                                     && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                     && m.ExtraxionID.Equals((rgTipos.SelectedIndex.Equals(0) ? _IDAutoconsumo : _IDDonacion))
                                     && m.ExtraxionConceptoID.Equals(lkConcepto.EditValue != null ? Convert.ToInt32(lkConcepto.EditValue) : 0)
                                     group k by new { k.ProductoID, k.AlmacenSalidaID } into gr
                                     select new
                                     {
                                         ID = gr.Key.ProductoID,
                                         TID = gr.Key.AlmacenSalidaID,
                                         Value = gr.Sum(s => s.CantidadSalida)
                                     }).ToList();

                        lbResumen.Items.Clear();
                        listaProductoRD.Where(l => l.Value > 0).GroupBy(g => new { g.ID, g.Display, g.TID, g.Price }).OrderBy(o => o.Key.ID).ToList().ForEach(item =>
                        {
                            decimal valor = item.Sum(s => s.Value);

                            if (menos.Count(m => m.ID.Equals(item.Key.ID) && m.TID.Equals(item.Key.TID)) > 0)
                                valor -= menos.First(m => m.ID.Equals(item.Key.ID) && m.TID.Equals(item.Key.TID)).Value;

                            lbResumen.Items.Add(item.Key.Display + " | " + valor.ToString("#,0.000") + " Litros");
                        });

                        //--- Fill Combos Detalles --//
                        gridProductos.DataSource = null;
                        gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                        gridProductos.DataSource = from P in db.Productos
                                                   join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                   where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                   && listaProductoRD.Where(l => l.Value > 0).GroupBy(g => g.ID).Select(s => s.Key).Contains(P.ID)
                                                   select new
                                                   {
                                                       P.ID,
                                                       P.Codigo,
                                                       P.Nombre,
                                                       UmidadName = U.Nombre,
                                                       Display = P.Codigo + " | " + P.Nombre
                                                   };
                    }
                    else
                    {
                        lbResumen.Items.Clear();
                        _Cuenta = db.ExtracionConceptos.Single(ex => ex.ID.Equals(Convert.ToInt32(lkConcepto.EditValue))).CuentaContableID;
                    }
                }
                else
                {
                    if (lkConcepto.EditValue != null)
                    {
                        Parametros.General.DialogMsg("Debe de seleccionar primero la fecha de la salida.", Parametros.MsgType.warning);
                        lbResumen.Items.Clear();
                        gridProductos.DataSource = null;
                        lkConcepto.EditValue = null;
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void lkConcepto_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void chkOficinaCentral_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOficinaCentral.Checked)
            {
                layoutControlItemCeco.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (chkOficinaCentral.Checked)
            {
                layoutControlItemCeco.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lkCeco.EditValue = null;
            }
        }

        #endregion

   

    }
}