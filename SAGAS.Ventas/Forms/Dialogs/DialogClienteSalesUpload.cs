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
using System.Text.RegularExpressions;
using DevExpress.XtraBars;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogClienteSalesUpload : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormClienteUpload MDI;
        internal Entidad.PlantillaCarga EntidadAnterior;
        private List<Entidad.VistaCargaVentaClientes> Vista;
        private List<Entidad.Movimiento> MaestroM = new List<Entidad.Movimiento>();
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool NextDialog = false;
        private int ClaseID;
        private decimal _TipoCambio;
       
        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Observacion
        {
            get { return memoObservacion.Text; }
            set { memoObservacion.Text = value; }
        }
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogClienteSalesUpload(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
                txtNumero.Properties.ReadOnly = true;
                dateFecha.Properties.ReadOnly = true;
            }
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
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                int number = 1;
                if (db.PlantillaCarga.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.EsCliente) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.PlantillaCarga.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.EsCliente).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                txtNumero.Text = number.ToString("000000000");

                ClaseID = Parametros.Config.ProductoClaseCombustible();
                
                    //nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " +
                //                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
                            
                
                
                //-------------------------------//


                if (Editable)
                {
                    Numero = EntidadAnterior.Numero.ToString("000000000");
                    Observacion = EntidadAnterior.Observacion;
                    Fecha = EntidadAnterior.FechaCarga;

                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);


                    //var lines = Enumerable.Range(0, Convert.ToInt32(5000 - EntidadAnterior.PlantillaCargaCliente.Count)).ToList();

                    //lines.ForEach(obj => EntidadAnterior.PlantillaCargaCliente.Add(new Entidad.PlantillaCargaCliente { Fecha = this.Fecha, ClienteID = 0, Codigo = "", NombreCliente = "", Numero = "", Saldo = 0m }));

                    //bdsDetalle.DataSource = EntidadAnterior.PlantillaCargaCliente.ToList();
                    //gvCarga.RefreshData();

                    Vista = (from v in dv.VistaCargaVentaClientes
                             where EntidadAnterior.PlantillaCargaCliente.Select(s => s.ID).Contains(v.PlantillaCargaClienteID)
                             select v).OrderBy(o => o.ClienteID).ThenBy(t => t.Numero).ToList();

                    for (int i = 1; i <= 15000 - Vista.Count; i++)
                    { Vista.Add(new Entidad.VistaCargaVentaClientes { InventarioUploadDetalleID = i }); }

                    //lines.ForEach(obj => EntidadAnterior.PlantillaCargaCliente.Add( new Entidad.PlantillaCargaCliente { Fecha = this.Fecha, ClienteID = 0, Codigo = "", NombreCliente = "", Numero = "", Saldo = 0m }));
                    
                    bdsDetalle.DataSource = Vista;
                    gvCarga.RefreshData();
                }
                else
                {
                    EntidadAnterior = new Entidad.PlantillaCarga();
                    this.layoutControlGroup1.Text += " " + Parametros.General.EstacionServicioName + " | " +
                                    (IDSubEstacion.Equals(0) ? "" : db.SubEstacions.Single(s =>s.ID.Equals(IDSubEstacion)).Nombre);
                    Fecha = Convert.ToDateTime(db.GetDateServer());

                    Vista = (from v in dv.VistaCargaVentaClientes
                             where v.PlantillaCargaClienteID.Equals(0)
                             select v).ToList();

                    for (int i = 1; i <= 15000; i++)
                    { Vista.Add(new Entidad.VistaCargaVentaClientes { InventarioUploadDetalleID = i }); }

                        //lines.ForEach(obj => EntidadAnterior.PlantillaCargaCliente.Add( new Entidad.PlantillaCargaCliente { Fecha = this.Fecha, ClienteID = 0, Codigo = "", NombreCliente = "", Numero = "", Saldo = 0m }));

                        bdsDetalle.DataSource = Vista;
                        gvCarga.RefreshData();
                    //EntidadAnterior.PlantillaCargaCliente.ToList();
                    
                }

                if (gvCarga.RowCount > 0)
                    gvCarga.FocusedRowHandle = 0;

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

        public bool ValidarCampos()
        {
            if (txtNumero.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
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
                 
            //if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            //{
            //    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
            //    return false;
            //}

            return true;
        }

        private bool Guardar(bool Finish)
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                    db.CommandTimeout = 3000;

                    Entidad.PlantillaCarga PC;

                    if (Editable)
                    {
                        PC = db.PlantillaCarga.Single(e => e.ID == EntidadAnterior.ID);

                        if (PC.Finalizado.Equals(true))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Esta plantilla ya fue Ingresada." + Environment.NewLine, Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        PC.PlantillaCargaCliente.ToList().ForEach(det => det.InventarioUploadDetalle.Clear());
                        PC.PlantillaCargaCliente.Clear();
                    }
                    else
                    {
                        PC = new Entidad.PlantillaCarga();
                        PC.FechaCreado = Convert.ToDateTime(db.GetDateServer());

                        int number = 1;
                        if (db.PlantillaCarga.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.EsCliente) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.PlantillaCarga.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.EsCliente).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        PC.Numero = number;
                        PC.EsCliente = true;
                        PC.TieneVentas = true;
                    }

                    PC.EstacionServicioID = IDEstacionServicio;
                    PC.SubEstacionID = IDSubEstacion;                    
                    PC.FechaCarga = Fecha;
                    PC.Observacion = Observacion;
                    PC.UserID = UsuarioID;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PC, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                         "Se modificó la plantilla inicial de Clientes: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.PlantillaCarga.InsertOnSubmit(PC);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se registró la plantilla inicial de CLientes: " + PC.Numero, this.Name);

                    }

                    db.SubmitChanges();

                    var Busqueda = Vista.Where(v => v.ClienteID > 0 && v.ProductoID > 0 && v.TanqueID > 0 && v.VentaTotal > 0 && v.CostoTotal > 0).ToList();

                    List<Entidad.PlantillaCargaCliente> PCC = (from v in Busqueda
                                                               group v by new { v.Numero, v.ClienteID, v.CodigoCliente, v.NombreCliente, v.Fecha } into gr
                                                               select new Entidad.PlantillaCargaCliente
                                                                {
                                                                     Fecha = gr.Key.Fecha,
                                                                     Numero = gr.Key.Numero,
                                                                     Codigo = gr.Key.CodigoCliente,
                                                                     ClienteID = gr.Key.ClienteID,
                                                                     NombreCliente = gr.Key.NombreCliente,
                                                                     Saldo = gr.Sum(s => s.VentaTotal)
                                                                }).ToList();

                    PCC.ForEach( det => 
                        {
                            List<Entidad.InventarioUploadDetalle> IUD = (from v in Busqueda
                                                               where det.Numero.Equals(v.Numero) && det.ClienteID.Equals(v.ClienteID) && det.Fecha.Date.Equals(v.Fecha.Date)
                                                               select new Entidad.InventarioUploadDetalle
                                                                {
                                                                    AlmacenID = v.TanqueID,
                                                                    ProductoID = v.ProductoID,
                                                                    Cantidad = v.Cantidad,
                                                                    Costo = v.Costo,
                                                                    CostoTotal = v.CostoTotal,
                                                                    CostoTotalAlmacen = v.CostoTotal,
                                                                    Precio = v.Precio,
                                                                    PrecioTotal = v.VentaTotal
                                                                }).ToList();
                            det.InventarioUploadDetalle.AddRange(IUD);
                        });

                    PC.PlantillaCargaCliente.AddRange(PCC);
                    db.SubmitChanges();
                    
                    if (Finish)
                    {
                        PC.Finalizado = true;

                        Entidad.Movimiento M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 44;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = PC.FechaCarga;
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.Numero = PC.Numero;
                        M.EstacionServicioID = PC.EstacionServicioID;
                        M.SubEstacionID = PC.SubEstacionID;
                        M.Comentario = PC.Observacion;
                        M.UsuarioID = PC.UserID;
                         M.Monto = Decimal.Round(PC.PlantillaCargaCliente.Sum(s => s.Saldo), 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(PC.PlantillaCargaCliente.Sum(s => s.Saldo) / _TipoCambio, 2, MidpointRounding.AwayFromZero);


                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se registró el Movimiento para la Plantilla Inicial de Clientes: " + M.Numero, this.Name);

                        db.SubmitChanges();

                        PC.MovimientoID = M.ID;

                        #region Guardando de Comprobante
                        
                        //Compronbante.ForEach(linea =>
                        //{
                        //    M.ComprobanteContables.Add(linea);
                        //    db.SubmitChanges();
                        //});

                        //db.SubmitChanges();

                        #endregion
                        int Area = db.AreaVentas.First(a => a.EsCombustible).ID;
                        progressBarCont.EditValue = 0;

                        progressBarCont.Properties.Maximum = PC.PlantillaCargaCliente.Count;
                    
                        foreach (Entidad.PlantillaCargaCliente det in PC.PlantillaCargaCliente)
                        {
                            progressBarCont.PerformStep();
                            progressBarCont.Update();

                            Entidad.Movimiento MD = new Entidad.Movimiento();
                            MD.ClienteID = det.ClienteID;
                            MD.MovimientoTipoID = 7;
                            MD.UsuarioID = UsuarioID;
                            MD.FechaCreado = M.FechaCreado;
                            MD.FechaRegistro = det.Fecha;
                            MD.MonedaID = M.MonedaID;
                            MD.TipoCambio = M.TipoCambio;
                            MD.FechaVencimiento = MD.FechaRegistro.AddDays(db.Clientes.Single(c => c.ID.Equals(det.ClienteID)).Plazo);
                            MD.Monto = det.Saldo;
                            MD.MontoMonedaSecundaria = Decimal.Round((det.Saldo / M.TipoCambio), 2, MidpointRounding.AwayFromZero);
                            MD.Numero = Convert.ToInt32(det.Numero);
                            MD.EstacionServicioID = M.EstacionServicioID;
                            MD.SubEstacionID = M.SubEstacionID;
                            MD.Comentario = PC.Observacion;
                            MD.UsuarioID = PC.UserID;
                            MD.Credito = true;
                            MD.MovimientoReferenciaID = M.ID;

                            MD.ResumenDiaID = db.ResumenDias.Where(rd => rd.EstacionServicioID.Equals(IDEstacionServicio) && rd.FechaInicial.Date.Equals(det.Fecha.Date)).First().ID;
                            MD.AreaID = Area;
                            db.Movimientos.InsertOnSubmit(MD);
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                            "Se registró la Venta de Crédito: " + MD.Numero.ToString("000000000"), this.Name);

                            db.SubmitChanges();

                            List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                            int i = 1;
                            var Cuentas = db.Clientes.Where(c => c.ID.Equals(det.ClienteID)).Select(s => new { s.ID, s.CuentaContableID });

                            string texto = "Factura de Venta Cliente " + det.NombreCliente + " Nro. " + det.Numero;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID,
                                Monto = Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(det.Saldo / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = det.Fecha,
                                Descripcion = texto,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;

                            List<Int32> IDVenta = new List<Int32>();
                            List<Int32> IDCosto = new List<Int32>();
                            List<Int32> IDInventario = new List<Int32>();

                            foreach (var dk in det.InventarioUploadDetalle)
                            {
                                //var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                                
                                Entidad.Kardex KX = new Entidad.Kardex();

                                KX.MovimientoID = MD.ID;
                                KX.ProductoID = dk.ProductoID;
                                KX.EsProducto = true;
                                KX.UnidadMedidaID = 1;
                                KX.Fecha = MD.FechaRegistro;
                                KX.EstacionServicioID = IDEstacionServicio;
                                KX.SubEstacionID = IDSubEstacion;
                                KX.AlmacenSalidaID = dk.AlmacenID;
                                KX.CantidadSalida = dk.Cantidad;
                                KX.CostoTotal = Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero);
                                KX.CostoSalida = Decimal.Round(dk.Costo, 4, MidpointRounding.AwayFromZero);

                                KX.CantidadInicial = Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, dk.ProductoID, dk.AlmacenID, KX.Fecha, false);
                               
                                KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

                                KX.Precio = dk.Precio;
                                KX.PrecioTotal = dk.PrecioTotal;

                                db.Kardexes.InsertOnSubmit(KX);

                                #region <<< COMPROBANTE PRODUCTO >>>
                                
                                var producto = db.Productos.Where(p => p.ID.Equals(KX.ProductoID)).Select(s => new { s.CuentaVentaID, s.CuentaInventarioID, s.CuentaCostoID }).FirstOrDefault();

                                if (producto != null)
                                {
                                    #region <<< CUENTA_VENTA >>>

                                    if (!producto.CuentaVentaID.Equals(0))
                                    {
                                        if (!IDVenta.Contains(producto.CuentaVentaID))
                                        {
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = producto.CuentaVentaID,
                                                Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(KX.PrecioTotal)), 2, MidpointRounding.AwayFromZero),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(KX.PrecioTotal / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                                Fecha = det.Fecha,
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
                                            comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(KX.PrecioTotal)), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(KX.PrecioTotal) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
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
                                                Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(KX.CostoTotal)), 2, MidpointRounding.AwayFromZero),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(KX.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                                Fecha = det.Fecha,
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
                                            comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(KX.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(KX.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
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
                                                Monto = Decimal.Round(Convert.ToDecimal(KX.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(KX.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                                Fecha = det.Fecha,
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
                                            comprobante.Monto += Decimal.Round(Convert.ToDecimal(KX.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(KX.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                        }
                                    }


                                    #endregion

                                }

                                #endregion
                            }

                            #region <<< REGISTRANDO COMPROBANTE >>>
                            
                            var obj = from cds in CD
                                      join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                                      join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                                      select new
                                      {
                                          Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                          Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                      };

                            if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                            {
                                progressBarCont.EditValue = 0;
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine + det.NombreCliente + " Nro. " + det.Numero , Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }
                            CD.ForEach(linea =>
                            {
                                MD.ComprobanteContables.Add(linea);
                            });

                            db.SubmitChanges();

                            #endregion

                            #region ::: REGISTRANDO DEUDOR :::
                            Entidad.Deudor D = new Entidad.Deudor();
                            D.ClienteID = det.ClienteID;
                            D.Valor = Decimal.Round(det.Saldo, 2, MidpointRounding.AwayFromZero);
                            D.MovimientoID = MD.ID;
                            db.Deudors.InsertOnSubmit(D);
                            db.SubmitChanges();

                            decimal Saldo = (db.Deudors.Count(d => !d.ID.Equals(D.ID) && d.ClienteID.Equals(det.ClienteID)) > 0 ? db.Deudors.Where(d => !d.ID.Equals(D.ID) && d.ClienteID.Equals(det.ClienteID)).Sum(s => s.Valor) : 0m);

                            if (Saldo < 0)
                            {
                                if (Math.Abs(Saldo) > MD.Monto)
                                {
                                    MD.Abono = MD.Monto;
                                    MD.Pagado = true;
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = MD.ID, Monto = MD.Abono });
                                }
                                else
                                {
                                    MD.Abono = Math.Abs(Saldo);
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = MD.ID, Monto = MD.Abono });
                                }

                                db.SubmitChanges();
                            }


                            #endregion
                        }

                    // if (!chkAnticipos.Checked)
                    //{
                    //    PC.PlantillaCargaCliente.ToList().ForEach( det =>
                    //    {
                    //        Entidad.Movimiento MD = new Entidad.Movimiento();
                    //        MD.ClienteID = det.ClienteID;
                    //        MD.MovimientoTipoID = 7;
                    //        MD.UsuarioID = UsuarioID;
                    //        MD.FechaCreado = M.FechaCreado;
                    //        MD.FechaRegistro = det.Fecha;
                    //        MD.MonedaID = M.MonedaID;
                    //        MD.TipoCambio = M.TipoCambio;
                    //        MD.FechaVencimiento = MD.FechaRegistro.AddDays(db.Clientes.Single(c => c.ID.Equals(det.ClienteID)).Plazo);
                    //        MD.Monto = det.Saldo;
                    //        MD.MontoMonedaSecundaria = Decimal.Round((det.Saldo / M.TipoCambio), 2, MidpointRounding.AwayFromZero);
                    //        MD.Referencia = det.Numero;
                    //        MD.EstacionServicioID = M.EstacionServicioID;
                    //        MD.SubEstacionID = M.SubEstacionID;
                    //        MD.Comentario = PC.Observacion;
                    //        MD.UsuarioID = PC.UserID;
                    //        MD.Credito = true;
                    //        MD.MovimientoReferenciaID = M.ID;

                    //        db.Movimientos.InsertOnSubmit(MD);

                    //        #region ::: REGISTRANDO DEUDOR :::

                    //        db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = det.ClienteID, Valor = Decimal.Round(det.Saldo, 3, MidpointRounding.AwayFromZero), MovimientoID = MD.ID });
                    //        db.SubmitChanges();
                    //        #endregion
                    //    });
                    // }
                    // else if (chkAnticipos.Checked)
                    // {
                    //     PC.PlantillaCargaCliente.ToList().ForEach( det =>
                    //    {
                    //     #region ::: REGISTRANDO DEUDOR :::

                    //     db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = det.ClienteID, Valor = -Math.Abs(Decimal.Round(det.Saldo, 3, MidpointRounding.AwayFromZero)), MovimientoID = M.ID });
                    //     db.SubmitChanges();
                    //     #endregion
                    //    });
                    // }


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
                    trans.Rollback();
                    progressBarCont.EditValue = 0;
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
                    progressBarCont.EditValue = 0;
                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    List<Int32> IDCuenta = new List<Int32>();
                    int i = 1;

                    
                     var Busqueda = Vista.Where(v => v.ClienteID > 0 && v.ProductoID > 0 && v.TanqueID > 0 && v.VentaTotal > 0 && v.CostoTotal > 0).ToList();
                    var Cuentas = db.Clientes.Where(c => (Busqueda.Where(p => !p.ClienteID.Equals(0)).Select(s => s.ClienteID).Contains(c.ID))).Select(s => new { s.ID, s.CuentaContableID });

                progressBarCont.Properties.Maximum = Busqueda.Count();
                int Number = 0;
                foreach (Entidad.VistaCargaVentaClientes det in Busqueda)
                {
                    progressBarCont.PerformStep();
                    progressBarCont.Update();
                    string texto = "";
                    if (!Convert.ToInt32(det.Numero).Equals(Number))
                    {
                         texto = "Factura de Venta Cliente " + det.NombreCliente + " Nro. " + det.Numero;
                        Number = Convert.ToInt32(det.Numero);
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID,
                            Monto = Decimal.Round(Convert.ToDecimal(det.VentaTotal), 2, MidpointRounding.AwayFromZero),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(det.VentaTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = det.Fecha,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion,
                            MovimientoID = Convert.ToInt32(det.Numero)
                        });
                        i++;
                    }
                    else
                    {
                        var comprobante = CD.Where(c => c.MovimientoID.Equals(Number)).OrderByDescending(o => o.Linea).First();
                        comprobante.Monto += Decimal.Round(Convert.ToDecimal(det.VentaTotal), 2, MidpointRounding.AwayFromZero);
                        comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.VentaTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                        texto = comprobante.Descripcion;    
                    }

                        List<Int32> IDVenta = new List<Int32>();
                        List<Int32> IDCosto = new List<Int32>();
                        List<Int32> IDInventario = new List<Int32>();

                        #region <<< DETALLE_PRODUCTO >>>

                        var producto = db.Productos.Where(p => p.ID.Equals(det.ProductoID)).Select(s => new { s.CuentaVentaID, s.CuentaInventarioID, s.CuentaCostoID }).FirstOrDefault();

                        if (producto != null)
                        {
                            #region <<< CUENTA_VENTA >>>

                            if (!producto.CuentaVentaID.Equals(0))
                            {
                                if (!IDVenta.Contains(producto.CuentaVentaID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaVentaID,
                                        Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(det.VentaTotal)), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(det.VentaTotal / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                        Fecha = det.Fecha,
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
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(det.VentaTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(det.VentaTotal) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
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
                                        Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(det.CostoTotal)), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(det.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                        Fecha = det.Fecha,
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
                                    comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(det.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(det.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
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
                                        Monto = Decimal.Round(Convert.ToDecimal(det.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                        Fecha = det.Fecha,
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
                                    comprobante.Monto += Decimal.Round(Convert.ToDecimal(det.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            }


                            #endregion

                        }
                        #endregion

                   
                    
                    }

                    //if (!IDCuenta.Contains(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID))
                    //{
                    //    CD.Add(new Entidad.ComprobanteContable
                    //                {
                    //                    CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID,
                    //                    Monto = Decimal.Round(Convert.ToDecimal(det.VentaTotal), 2, MidpointRounding.AwayFromZero),
                    //                    TipoCambio = _TipoCambio,
                    //                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.VentaTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                    //                    Fecha = Fecha,
                    //                    Descripcion = "Carga Ventas Iniciales del Cliente " + det.CodigoCliente + " | " + det.NombreCliente,
                    //                    Linea = i,
                    //                    EstacionServicioID = IDEstacionServicio,
                    //                    SubEstacionID = IDSubEstacion
                    //                });
                    //    IDCuenta.Add(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID);
                    //    i++;



                    //}
                    //else
                    //{
                    //    var comprobante = CD.Where(c => c.CuentaContableID.Equals(Cuentas.First(ct => ct.ID.Equals(det.ClienteID)).CuentaContableID)).First();
                    //    comprobante.Monto += Decimal.Round(Convert.ToDecimal(det.VentaTotal), 2, MidpointRounding.AwayFromZero);
                    //    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.VentaTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                    //}
                

                //progressBarCont.PerformStep();
                //progressBarCont.Update();
                //        CD.Add(new Entidad.ComprobanteContable
                //        {
                //            CuentaContableID = 526,
                //            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(Busqueda.Sum(s => s.VentaTotal)), 2, MidpointRounding.AwayFromZero)),
                //            TipoCambio = _TipoCambio,
                //            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Busqueda.Where(p => !p.ClienteID.Equals(0)).Sum(s => s.VentaTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                //            Fecha = Fecha,
                //            Descripcion = "Carga Ventas Iniciales del Cliente ",// + det.CodigoCliente + " | " + det.NombreCliente,
                //            Linea = i,
                //            EstacionServicioID = IDEstacionServicio,
                //            SubEstacionID = IDSubEstacion
                //        });
                    
                    return CD;
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }
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

        private void ShowPupUpMenu()
        {
            try
            {

                DevExpress.XtraBars.BarManager barManager1;
                PopupMenu popupMenuCells;
                DevExpress.XtraBars.BarButtonItem menuButtonCopiar = new DevExpress.XtraBars.BarButtonItem();
                DevExpress.XtraBars.BarButtonItem menuButtonPegar = new DevExpress.XtraBars.BarButtonItem();

                barManager1 = new BarManager();
                barManager1.Form = this;
                popupMenuCells = new DevExpress.XtraBars.PopupMenu(barManager1);

                menuButtonCopiar.Caption = "C&opiar";
                menuButtonCopiar.Glyph = Properties.Resources.page_white_stack;
                menuButtonCopiar.Id = 1;
                menuButtonCopiar.ItemClick += new ItemClickEventHandler(menuButtonCopiar_ItemClick);

                menuButtonPegar.Caption = "P&egar";
                menuButtonPegar.Glyph = Properties.Resources.paste_plain;
                menuButtonPegar.Id = 2;
                menuButtonPegar.ItemClick += new ItemClickEventHandler(menuButtonPegar_ItemClick);
                barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { menuButtonCopiar, menuButtonPegar });

                popupMenuCells.ItemLinks.Add(barManager1.Items[0]);
                popupMenuCells.ItemLinks.Add(barManager1.Items[1]);
                barManager1.SetPopupContextMenu(this.grid, popupMenuCells);

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar(false)) return;

            this.Close();
        }

        private void bntFinalizar_Click(object sender, EventArgs e)
        {
            if (Parametros.General.DialogMsg("¿Desea finalizar este carga de la plantilla?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
            {
                if (!Guardar(true)) return;
                this.Close();
            }
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, NextDialog, true);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (EntidadAnterior.PlantillaCargaCliente.Count > 0)
                {
                    if (Parametros.General.DialogMsg("La plantilla actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        NextDialog = false;
                        e.Cancel = true;
                    }
                }
            }

            EntidadAnterior = null;
        }

        private void bntNew_Click(object sender, EventArgs e)
        {
             NextDialog = true;
             this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }
        
        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.grid.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "Numero").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                                               
                    }
                }
            }
        } 
        
        private void gvCarga_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            ShowPupUpMenu();
        }

        private void menuButtonCopiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gvCarga.CopyToClipboard();
        }

        private void menuButtonPegar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var filas = gvCarga.GetSelectedRows();
                string lista = Clipboard.GetText();

                List<String> datos = new List<String>();
                string[] lineas = Regex.Split(lista, "\r\n");


                int i = 0;
                foreach (int obj in filas)
                {
                    int j = 0;
                    foreach (string item in lineas.ElementAt(i).Split('\t'))
                    {

                        gvCarga.SetRowCellValue(obj, gvCarga.GetSelectedCells(obj).ElementAt(j), item.ToString());
                        j++;
                    }
                    i++;
                }

            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains("Index was out of range"))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                }
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                progressBarCont.EditValue = 0;
                var Client = db.Clientes.Where(c => c.Activo).Select(s => new { s.ID, s.Codigo }).ToList();
                var Prod = db.Productos.Where(p => p.Activo && p.ProductoClaseID.Equals(ClaseID)).Select(s => new { s.ID, s.Codigo}).ToList();
                var Tanque = db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();

                var costo = (from k in db.Kardexes
                             join m in db.Movimientos on k.MovimientoID equals m.ID
                             where m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion)
                             select new { k.ID, k.Fecha, k.ProductoID, k.CostoFinal, k.CostoEntrada }).ToList();

                var Busqueda = Vista.Where(v => !String.IsNullOrEmpty(v.CodigoCliente) && !String.IsNullOrEmpty(v.CodigoProducto) && !String.IsNullOrEmpty(v.Tanque));
                progressBarCont.Properties.Maximum = Busqueda.Count();
                foreach (Entidad.VistaCargaVentaClientes det in Busqueda)
                {
                    progressBarCont.PerformStep();
                    progressBarCont.Update();
                    if (!String.IsNullOrEmpty(det.CodigoCliente) || !String.IsNullOrEmpty(det.CodigoProducto) || !String.IsNullOrEmpty(det.Fecha.ToString()) || !String.IsNullOrEmpty(det.Tanque))
                    {
                        bool Valido = true;
                        if (!Client.Select(s => s.Codigo).Contains(Convert.ToString(det.CodigoCliente)))
                        {
                            Valido = false;
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("No se encuentra registrado el codigo " + det.CodigoCliente, Parametros.MsgType.warning);
                            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        }

                        if (!Prod.Select(s => s.Codigo).Contains(Convert.ToString(det.CodigoProducto)))
                        {
                            Valido = false;
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("No se encuentra registrado el codigo " + det.CodigoProducto, Parametros.MsgType.warning);
                            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        }

                        if (!Tanque.Select(s => s.Nombre).Contains(Convert.ToString(det.Tanque)))
                        {
                            Valido = false;
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("No se encuentra registrado el codigo " + det.Tanque, Parametros.MsgType.warning);
                            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        }

                        if (Valido)
                        {
                            det.ClienteID = Client.First(c => c.Codigo.Equals(det.CodigoCliente)).ID;
                            det.ProductoID = Prod.First(c => c.Codigo.Equals(det.CodigoProducto)).ID;
                            det.TanqueID = Tanque.First(t => t.Nombre.Equals(det.Tanque)).ID;

                            var VCtoEntrada = costo.Where(c => c.ProductoID.Equals(det.ProductoID) && c.Fecha.Date <= det.Fecha.Date && !c.CostoEntrada.Equals(0)).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).FirstOrDefault();

                            det.Costo = (VCtoEntrada != null ? VCtoEntrada.CostoFinal : 0m);
                            det.CostoTotal = Decimal.Round(det.Costo * det.Cantidad, 2, MidpointRounding.AwayFromZero);


                        }
                    }
                }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                gvCarga.RefreshData();
            }
            catch (Exception ex)
            {
                progressBarCont.EditValue = 0;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void bgvProductos_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.grid.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                            + view.GetRowCellDisplayText(RowHandle, "Numero").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.grid.DefaultView;
                    view.AddNewRow();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                {
                    nf.DetalleCD = PartidasContable;
                    nf.Text = "Comprobante Contable";
                    nf.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Verfificar Tipo de Cambio
        private void dateFecha_Validated(object sender, EventArgs e)
        {
            if (dateFecha.EditValue != null)
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
        }

        #endregion

     

       

    }
}