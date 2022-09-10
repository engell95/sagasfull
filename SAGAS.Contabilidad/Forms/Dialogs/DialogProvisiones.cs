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
using SAGAS.Contabilidad.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogProvisiones : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormProvisiones MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal bool OnlyView = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool NextProvision = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID; 
        private bool _Guardado = false;
        private bool _ToPrint = false;
        private int IDPrint = 0;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private int _CuentaIVACredito = Parametros.Config.IVAPorAcreditar();
        internal decimal vTotalPagar = 0m;
        internal int _MonedaPrimaria;
        internal int _MonedaSecundaria;
        internal DateTime _FechaCarga;

        /// PEDIDOS COMBUSTIBLE
        internal bool _calculado = false;
        private Entidad.Pedido EtPedido;
        private IQueryable<Parametros.ListIdDisplay> Tanques;
        private List<Parametros.ListIdDisplayValue> _CombCantidad;

        private List<Entidad.Kardex> K = new List<Entidad.Kardex>();
        public List<Entidad.Kardex> DetalleK
        {
            get { return K; }
            set
            {
                K = value;
                this.bdsComb.DataSource = this.K;
            }
        }

        /// <summary>
        /// -------------------
        /// </summary>


        private DateTime FechaCompra
        {
            get { return Convert.ToDateTime(dateFechaCompra.EditValue); }
            set { dateFechaCompra.EditValue = value; }
        }

        private int IDProveedor
        {
            get { return Convert.ToInt32(glkProvee.EditValue); }
            set { glkProvee.EditValue = value; }
        }

        private string Referencia
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
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
        
        private int IDMonedaPrincipal
        {
            get { return Convert.ToInt32(lkMoneda.EditValue); }
            set { lkMoneda.EditValue = value; }
        }

        private static Entidad.Proveedor Provee;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;

        #endregion

        #region *** INICIO ***

        public DialogProvisiones(int UserID, bool _OnlyView)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            OnlyView = _OnlyView;

            if (OnlyView)
            { //-- Bloquear Controles --//               
                glkProvee.Properties.ReadOnly = true;
                lkMoneda.Properties.ReadOnly = true;
                txtReferencia.Properties.ReadOnly = true;
                dateFechaCompra.Properties.ReadOnly = true;
                dateFechaVencimiento.Properties.ReadOnly = true;
                mmoComentario.Properties.ReadOnly = true;
                gvDetalle.OptionsBehavior.Editable = false;
                gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
                btnOK.Enabled = false;
                btnOK.Visible = false;
                spTC.Properties.ReadOnly = true;
                spTotal.Properties.ReadOnly = true;
                layoutControlItemSub.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemEsIva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

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

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (OnlyView ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                

                this.CentrosCostos = from cto in db.CentroCostos
                                      join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                      where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                      select new Parametros.ListIdDisplay {ID = cto.ID, Display = cto.Nombre};

                Cuentas = (from cc in db.CuentaContables
                                         join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                         join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                                         where cc.Detalle && !cc.Modular && cc.Activo && cces.EstacionID.Equals(IDEstacionServicio)
                                         select new Parametros.ListIdDisplayCodeBool
                                         {
                                             ID = cc.ID,
                                             Codigo = cc.Codigo,
                                             Display = cc.Nombre,
                                             valor = tc.UsaCentroCosto
                                         }).OrderBy(o => o.Codigo);

                //Proveedor
                glkProvee.Properties.DataSource = (from p in db.Proveedors where (p.Activo || OnlyView) select new { p.ID, p.Codigo, p.Nombre, p.NombreComercial, Display = p.Codigo + " | " + p.Nombre }).OrderBy(o => o.Codigo);
                glkProvee.Properties.DisplayMember = "Display";
                glkProvee.Properties.ValueMember = "ID";

                //Tipo de Moneda
                lkMoneda.Properties.DataSource = from m in db.Monedas select new { m.ID, Display = m.Simbolo + " | " + m.Nombre};
                lkMoneda.Properties.DisplayMember = "Display";
                lkMoneda.Properties.ValueMember = "ID";

                //--- Fill Combos Detalles --//
                gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                gridCuenta.DataSource = null;
                gridCuenta.DataSource = Cuentas;

                //Centro Costo GRID
                lkCentroCosto.DataSource = CentrosCostos;
                lkCentroCosto.DisplayMember = "Display";
                lkCentroCosto.ValueMember = "ID";

                dateFechaCompra.EditValue = Convert.ToDateTime(db.GetDateServer());
                IDMonedaPrincipal = Parametros.Config.MonedaPrincipal();
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _MonedaSecundaria = Parametros.Config.MonedaSecundaria();

                Tanques = db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });


                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaCompra.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaCompra.Date)).First().Valor : 0m);

                #region <<< VISTA_PREVIA >>>
                if (OnlyView)
                {
                    FechaCompra = EntidadAnterior.FechaRegistro;
                    IDProveedor = EntidadAnterior.ProveedorID;
                    Referencia = EntidadAnterior.Referencia;
                    Comentario = EntidadAnterior.Comentario;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;
                    IDMonedaPrincipal = EntidadAnterior.MonedaID;
                    FechaVencimiento = Convert.ToDateTime(EntidadAnterior.FechaVencimiento);
                    _TipoCambio = EntidadAnterior.TipoCambio;
                    spTotal.Value = EntidadAnterior.Monto;

                    var listComprobante = (from cd in db.ComprobanteContables
                                           join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                           join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                           where cd.MovimientoID.Equals(EntidadAnterior.ID) && !cd.CuentaContableID.Equals(Provee.CuentaContableID)
                                           select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                    this.DetalleComprobante = ToDataTable(listComprobante);

                    if (!EntidadAnterior.MonedaID.Equals(_MonedaPrimaria))
                    {
                        DetalleComprobante.AsEnumerable().ToList().ForEach(lista =>
                            {
                                lista["Monto"] = Decimal.Round((Convert.ToDecimal(lista["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            });
                    }

                    gvDetalle.RefreshData();                   

                }
                else
                {
                    var listComprobante = (from cd in db.ComprobanteContables
                                           join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                           join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                           where cd.MovimientoID.Equals(0)
                                           select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                    this.DetalleComprobante = ToDataTable(listComprobante);
                }
                #endregion

                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

                this.bdsDetalle.DataSource = this.DetalleComprobante;

                //--- Fill Combos Detalles --//
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                gridProductos.DataSource = null;
                gridProductos.DataSource = from P in db.Productos
                                           join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                           join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                           join A in db.Areas on C.AreaID equals A.ID
                                           where P.Activo.Equals(true) && !P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && A.Activo && C.Activo && !A.AplicaOrdenCompra
                                           select new
                                           {
                                               P.ID,
                                               P.Codigo,
                                               P.Nombre,
                                               UmidadName = U.Nombre,
                                               Display = P.Codigo + " | " + P.Nombre
                                           };

                //---LLenar Almacenes ---//
                IQueryable<Parametros.ListIdDisplay> listaAlmacen;

                    listaAlmacen = from al in db.Tanques
                                   where (al.Activo || OnlyView) && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                   select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };

                //UnAlmacen = (listaAlmacen.ToList().Count > 1 ? false : true);
                //Almacen GRID
                cboAlmacen.DataSource = listaAlmacen.ToList();
                cboAlmacen.DisplayMember = "Display";
                cboAlmacen.ValueMember = "ID";

                gridProductos.DisplayMember = "Display";
                gridProductos.ValueMember = "ID";
                gridProductos.View.Columns.Clear();
                GridColumn colCode = gridProductos.View.Columns.AddField("Codigo");
                colCode.Caption = "Codigo";
                colCode.VisibleIndex = 1;

                GridColumn colName = gridProductos.View.Columns.AddField("Nombre");
                colName.Caption = "Nombre";
                colName.VisibleIndex = 2;

                //--
                GridColumn colUnidad = gridProductos.View.Columns.AddField("UmidadName");
                colUnidad.Caption = "Unidad de Medida";
                colUnidad.VisibleIndex = 3;

                gridProductos.View.BestFitColumns();
                gridProductos.PopupFormWidth = 550;

                //---------------------------------//

                cboRpUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboRpUnidadMedida.DisplayMember = "Nombre";
                cboRpUnidadMedida.ValueMember = "ID";

                this.bdsComb.DataSource = this.DetalleK;

                txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");
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
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkProvee, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(spTotal, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && i.ProveedorID.Equals(IDProveedor) && !i.Anulado
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (dateFechaCompra.EditValue == null || dateFechaVencimiento.EditValue == null || lkMoneda.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtReferencia.Text == "" || spTotal.Value.Equals(0))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la provisión.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkProvee.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Proveedor.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaCompra, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaCompra, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (FechaVencimiento.Date < FechaCompra.Date)
            {
                Parametros.General.DialogMsg("La fecha de vencimiento debe ser mayor a la fecha de factura.", Parametros.MsgType.warning);
                return false;
            }

            if (FechaCompra.Date > Convert.ToDateTime(db.GetDateServer()).Date)
            {
                Parametros.General.DialogMsg("La fecha de recibido o de factura no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
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
                if (DetalleComprobante.Rows.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a la provisión." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!ValidarReferencia(Convert.ToString(txtReferencia.Text)))
                {
                    Parametros.General.DialogMsg("La referencia para esta provision ya existe : " + Convert.ToString(txtReferencia.Text), Parametros.MsgType.warning);
                    return false;
                }

            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (Convert.ToDecimal(txtGrandTotal.Text).Equals(0))
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

            if (!Convert.ToDecimal(txtGrandTotal.Text).Equals(Convert.ToDecimal(spSub.Value)))
            {
                Parametros.General.DialogMsg("El subtotal de la factura no es igual al Total contabilizado", Parametros.MsgType.warning);
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

                        Entidad.Movimiento M;

                        M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 5;
                        M.ProveedorID = IDProveedor;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaCompra;
                        M.MonedaID = IDMonedaPrincipal;
                        M.TipoCambio = _TipoCambio;
                        M.Credito = true;
                        M.Referencia = Referencia;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = Comentario;
                        M.FechaVencimiento = FechaVencimiento;
                        M.FechaCarga = _FechaCarga;
                        M.SubTotal = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spSub.Value) :
                                    Decimal.Round((Convert.ToDecimal(spSub.Value) * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                        M.IVA = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spIva.Value) :
                                    Decimal.Round((Convert.ToDecimal(spIva.Value) * M.TipoCambio), 2, MidpointRounding.AwayFromZero));

                        M.Monto = Decimal.Round(M.SubTotal + M.IVA, 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(M.SubTotal + M.IVA) / M.TipoCambio, 2, MidpointRounding.AwayFromZero) : Decimal.Round(Convert.ToDecimal(spSub.Value + spIva.Value), 2, MidpointRounding.AwayFromZero));
                        M.CuentaID = db.Proveedors.Single(s => s.ID.Equals(IDProveedor)).CuentaContableID;
                        db.Movimientos.InsertOnSubmit(M);                        
                        db.SubmitChanges();
                                            
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

                    if (!M.Monto.Equals(vTotalPagar))
                        M.Monto = Decimal.Round(vTotalPagar, 2, MidpointRounding.AwayFromZero);

                    #region ::: REGISTRANDO DEUDOR :::
                    
                    db.Deudors.InsertOnSubmit(new Entidad.Deudor { ProveedorID = IDProveedor, Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                    db.SubmitChanges();
                    #endregion

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se registró la Provisión de Gastos: " + M.Referencia, this.Name);


                    #region <<< Registrando Solicitud >>>

                    foreach (var dk in DetalleK)
                    {
                        if (EtPedido != null)
                        {
                            var Producto = db.Productos.Select(s => new { s.ID, s.Codigo, s.Nombre}).Single(p => p.ID.Equals(dk.ProductoID));

                            if (DetalleK.Where(d => d.ProductoID.Equals(dk.ProductoID)).Sum(s => s.CantidadEntrada) > _CombCantidad.First(f => f.ID.Equals(dk.ProductoID)).Value)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " excede la cantidad del pedido, favor revisar el inventario.", Parametros.MsgType.warning);
                                return false;
                            }
                            else
                            {
                                if (DetalleK.Where(d => d.ProductoID.Equals(dk.ProductoID)).Sum(s => s.CantidadEntrada) == _CombCantidad.First(f => f.ID.Equals(dk.ProductoID)).Value)
                                    _CombCantidad.Single(s => s.ID.Equals(dk.ProductoID)).Display = "1";
                                else
                                    _CombCantidad.Single(s => s.ID.Equals(dk.ProductoID)).Display = "0";
                            }
                        }
                    }


                        if (EtPedido != null)
                    {
                        //DetalleK.ForEach(k =>
                        //{
                        //    Entidad.PedidoCompra EP = new Entidad.PedidoCompra();
                        //    EP.MovimientoID = M.ID;
                        //    EP.PedidoID = EtPedido.ID;
                        //    EP.ProductoID = k.ProductoID;
                        //    EP.Cantidad = k.CantidadEntrada;
                        //    db.PedidoCompras.InsertOnSubmit(EP);
                        //    db.SubmitChanges();
                        //});

                        //if (_CombCantidad.All(c => c.Display.Equals("1")))
                        //{
                        //    var Epe = db.Pedidos.Single(s => s.ID.Equals(EtPedido.ID));
                        //    Epe.Finalizado = true;
                        //    db.SubmitChanges();
                        //}
                    }

                    #endregion

                    db.SubmitChanges();
                        trans.Commit();

                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        NextProvision = true;
                        RefreshMDI = true;
                        ShowMsg = true;
                        _Guardado = true;
                        _ToPrint = true;
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

                    if (!OnlyView)
                    {
                        if (Provee == null)
                        {
                            Parametros.General.DialogMsg("Debe seleccionar un Proveedor.", Parametros.MsgType.warning);
                            return null;
                        }

                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();


                        #region <<< DETALLE_COMPROBANTE >>>
                        int i = 1;
                        decimal _vMP = 0, _vMS = 0;
                        
                        //DetalleComprobante.Rows. .ToList().ForEach(K =>
                        foreach (DataRow linea in DetalleComprobante.Rows)
                        {

                            _vMP += (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])) :
                            Decimal.Round((Convert.ToDecimal(linea["Monto"]) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                            _vMS += (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                            Convert.ToDecimal(linea["Monto"]));
                            
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                Monto = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])) :
                            Decimal.Round((Convert.ToDecimal(linea["Monto"]) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                Convert.ToDecimal(linea["Monto"])),
                                Fecha = FechaCompra,
                                Descripcion = Convert.ToString(linea["Descripcion"]),
                                Linea = i,
                                CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;
                        }

                        #endregion

                        decimal _ivaMP = 0, _ivaMS = 0;

                        if (chkAplicaIva.Checked)
                        {
                            _ivaMP = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spIva.Value) :
                                Decimal.Round((Convert.ToDecimal(spIva.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                            _ivaMS = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spIva.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                Convert.ToDecimal(spIva.Value));

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaIVACredito,
                                Monto = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spIva.Value) :
                                Decimal.Round((Convert.ToDecimal(spIva.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spIva.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                Convert.ToDecimal(spIva.Value)),
                                Fecha = FechaCompra,
                                Descripcion = Provee.Nombre + " – Factura Nro. " + txtReferencia.Text,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        if (Provee != null)
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Provee.CuentaContableID,
                                Monto = -Math.Abs(Decimal.Round(_vMP + _ivaMP, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_vMS + _ivaMS, 2, MidpointRounding.AwayFromZero)),
                                Fecha = FechaCompra,
                                Descripcion = Provee.Nombre + " – Factura Nro. " + txtReferencia.Text,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        vTotalPagar = _vMP + _ivaMP;
                        return CD;
                    }
                    else
                        return EntidadAnterior.ComprobanteContables.ToList();

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
            MDI.CleanDialog(ShowMsg, NextProvision, RefreshMDI, IDPrint, _ToPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !OnlyView && !NextProvision)
            {
                if (DetalleComprobante.Rows.Count > 0 || txtReferencia.Text != "" || string.IsNullOrEmpty(mmoComentario.Text) || Provee != null)
                {
                    if (Parametros.General.DialogMsg("La provisión actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            Provee = null;
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

        //Selecciona el Proveedor
        private void glkProvee_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkProvee.EditValue) > 0)
                {
                    Provee = db.Proveedors.Single(p => p.ID.Equals(IDProveedor));
                    txtRuc.Text = Provee.RUC + " / " + Provee.NombreComercial;
                    txtPlazo.Text = Provee.Plazo.ToString();
                    txtTelefonos.Text = Provee.Telefono1 + " | " + Provee.Telefono2 + " | " + Provee.Telefono3;
                    memoDir.Text = Provee.Direccion;
                    FechaVencimiento = FechaCompra.AddDays(Provee.Plazo);

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #region <<< GRID_DETALLES >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            
            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "CuentaContableID") != DBNull.Value)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "CuentaContableID")) == 0)
                {
                    view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                    e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Descripcion
            //--
            if (view.GetRowCellValue(RowHandle, "Descripcion") == DBNull.Value)
            {
                view.SetColumnError(view.Columns["Descripcion"], "Debe de escribir una descripción.");
                e.ErrorText = "Debe de escribir una descripción.";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Valor             
            if (view.GetRowCellValue(RowHandle, "Monto") != DBNull.Value)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Monto")).Equals(0))
                {
                    view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                    e.ErrorText = "Debe Ingresar un valor en la linea.";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                e.ErrorText = "Debe Ingresar un valor en la linea.";
                e.Valid = false;
                Validate = false;
            }

            txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");

            //-- Validar Columna de Centro de Costo             
            if (view.GetRowCellValue(RowHandle, "UsaCentroCosto") != DBNull.Value)
            {
                if (Convert.ToBoolean(view.GetRowCellValue(RowHandle, "UsaCentroCosto")).Equals(true))
                {
                    if (view.GetRowCellValue(RowHandle, "CentroCostoID") != DBNull.Value)
                    {
                        if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CentroCostoID")).Equals(0))
                        {

                            view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo.");
                            e.ErrorText = "Debe seleccionar un centro de costo.";
                            e.Valid = false;
                            Validate = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo");
                        e.ErrorText = "Debe seleccionar un centro de costo";
                        e.Valid = false;
                        Validate = false;
                    }
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
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
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    gvDetalle.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    gvDetalle.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Linea", 0);

                    
                    if (gvDetalle.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(gvDetalle.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(gvDetalle.GetRowCellValue(gvDetalle.RowCount - 2, "Descripcion")));
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            if (e.Column == colValor)
            {
                //if (gvDetalle.GetRowCellValue(e.RowHandle, "Monto") == DBNull.Value)
                //    gvDetalle.SetRowCellValue(e.RowHandle, "Monto", 0.00);
                //else
                //{
                //    gvDetalle.RefreshData();
                    
                //}
            }

            txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");

            #endregion
        }
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {
            if (!OnlyView)
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
                            + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);

                            txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");
                            
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    view.AddNewRow();
                }
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
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");
                                               
                    }
                }
            }
        }

        //Validando la asignacion de Centro de Costo
        private void gvDetalle_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvDetalle.FocusedColumn == colCentroCosto)
            {
                if (gvDetalle.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(gvDetalle.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }

            }
        }
        
        #endregion

        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Convert.ToDecimal(txtGrandTotal.Text).Equals(Convert.ToDecimal(spSub.Value)))
                {
                    Parametros.General.DialogMsg("El subtotal de la factura no es igual al Total contabilizado", Parametros.MsgType.warning);
                    return;
                }

                if (Convert.ToDecimal(txtGrandTotal.Text).Equals(0))
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        return;
                }

                if (Provee != null)
                {
                    var obj = from cds in PartidasContable.ToList()
                              join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                              join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                              select new
                              {
                                  Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                  Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                              };

                    if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                        return;
                    }

                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;                      
                        nf.Text = "Comprobante Contable Provisión de Gastos";
                        nf.ShowDialog();
                    }
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

        private void lkMoneda_EditValueChanged(object sender, EventArgs e)
        {
            if (lkMoneda.EditValue != null)
            {
                if (Convert.ToInt32(lkMoneda.EditValue).Equals(_MonedaPrimaria))
                {
                    lkMoneda.BackColor = Color.White;
                    lkMoneda.ForeColor = Color.Black;

                    txtGrandTotal.BackColor = Color.FromArgb(232, 249, 239);
                    txtGrandTotal.ForeColor = Color.Black;
                }
                else if (Convert.ToInt32(lkMoneda.EditValue).Equals(_MonedaSecundaria))
                {
                    lkMoneda.BackColor = Color.ForestGreen;
                    lkMoneda.ForeColor = Color.White;

                    txtGrandTotal.BackColor = Color.ForestGreen;
                    txtGrandTotal.ForeColor = Color.White;
                }

            }
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

        //Boton nueva provision
        private void bntNew_Click(object sender, EventArgs e)
        {
            if (!OnlyView)
            {
                if (DetalleComprobante.Rows.Count > 0 || dateFechaCompra.EditValue != null || dateFechaVencimiento.EditValue != null || lkMoneda.EditValue != null || txtReferencia.Text != "")
                {
                    if (Parametros.General.DialogMsg("La provisión actual tiene datos registrados. ¿Desea cancelar esta provisión y realizar una nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        NextProvision = true;
                        RefreshMDI = false;
                        ShowMsg = false;
                        this.Close();
                    }
                }
            }
            else
            {
                NextProvision = true;
                RefreshMDI = false;
                ShowMsg = false;
                this.Close();
            }
        } 
          
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            txtGrandTotal.Text = Convert.ToDecimal(gvDetalle.Columns["Monto"].SummaryText).ToString("#,0.00");
        }

        private void dateFechaCompra_EditValueChanged(object sender, EventArgs e)
        {
            if (Provee != null)
                FechaVencimiento = FechaCompra.AddDays(Provee.Plazo);
        }  

        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaCompra.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaCompra.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaCompra.Date)).First().Valor : 0m);
        }
        
        private void spSub_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spSub.Value) > 0)
            {
                decimal iva = 0m;
                if (chkAplicaIva.Checked)
                {
                    iva = Decimal.Round(Convert.ToDecimal(spSub.Value * 0.15m), 2, MidpointRounding.AwayFromZero);
                    spIva.Value = iva;
                }

                spTotal.Value = Convert.ToDecimal(spSub.Value + iva);
            }
        }

        private void spIva_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spIva.Value) > 0)
            {
                spTotal.Value = Convert.ToDecimal(spSub.Value + spIva.Value);
            }
        }

        private void chkAplicaIva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaIva.Checked)
            {
                layoutControlItemIva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                spSub_EditValueChanged(null, null);
            }
            else if (!chkAplicaIva.Checked)
            {
                layoutControlItemIva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                spIva.Value = 0;
                spSub_EditValueChanged(null, null);
            }
        }
        
        private void chkAplicaIva_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Provee != null)
            {
                if (!Provee.AplicaIVA && Convert.ToBoolean(e.NewValue).Equals(true))
                {
                    Parametros.General.DialogMsg("El proveedor no aplica IVA.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }

            }
        }

        private void lkMoneda_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleComprobante != null)
            {
                if (DetalleComprobante.Rows.Count > 0)
                {
                    Parametros.General.DialogMsg("La provisión tiene detalle ingresados.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region <<< PEDIDO COMBUSTIBLE >>>

        private void gridComb_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridComb.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        //txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal) + K.Sum(s => s.MontoISC)).ToString("#,0.00");

                    }
                }
            }
        }

        private void gvComb_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            //if (ValidarCampos(true))
            //{
                #region <<< CALCULOS_MONTOS >>>
                //--  Calcular las montos de las columnas de Impuestos y Subtotal
                if ((e.Column == colCostoTotal && !colCostoTotal.ReadOnly) || e.Column == colQuantity || e.Column == colCostoSubTotal)
                {
                    if (_calculado)
                        return;

                    decimal vCosto = 0, vCantidad = 1, vValorImpuesto = 0, vCostoTotal = 0, vISC = 0, vSubTotal = 0;

                    if (EtPedido != null && e.Column == colQuantity)
                    {
                        if (gvComb.GetRowCellValue(e.RowHandle, "CostoEntrada") != null)
                            vCosto = Decimal.Round(Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CostoEntrada")), 4, MidpointRounding.AwayFromZero);
                        if (Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                            vCantidad = Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
                        else
                            gvComb.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                        vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);

                    }
                    else
                    {

                        if (gvComb.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                            vCostoTotal = Decimal.Round(Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CostoTotal")), 2, MidpointRounding.AwayFromZero);
                        if (Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                            vCantidad = Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
                        else
                            gvComb.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                        vCosto = Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero);

                    }

                    if (Provee.AplicaIVA)
                    {
                        if (gvComb.GetRowCellValue(e.RowHandle, "EsManejo") != null)
                        {
                            if (!(Convert.ToBoolean(gvComb.GetRowCellValue(e.RowHandle, "EsManejo"))))
                                vValorImpuesto = Decimal.Round((vCostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                        }
                    }


                    SetValue(e.RowHandle, vISC, vValorImpuesto, vCosto, vCostoTotal);

                    
                        if (gvComb.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                        string texto = "";// InfoCombustible(Convert.ToInt32(gvComb.GetRowCellValue(e.RowHandle, "ProductoID")), vCosto);
                            gvComb.SetRowCellValue(e.RowHandle, "DetalleCombustible", texto);
                        }
                }

                #endregion


                #region <<< COLUMNA_PRODUCTO >>>
                //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
                if (e.Column == colProduct)
                {
                    if (gvComb.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (Convert.ToInt32(gvComb.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
                        {
                            return;
                        }
                    }

                    try
                    {
                        var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvComb.GetRowCellValue(e.RowHandle, "ProductoID")));

                        //-- Unidad Principal     
                        int Um = Producto.UnidadMedidaID;
                        gvComb.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Um);

                        

                            //Tanques por combustible
                            var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                            ColAlmacen.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                            //cboAlmacen.DataSource = TCombustible;
                            //cboAlmacen.DisplayMember = "Display";
                            //cboAlmacen.ValueMember = "ID";
                            gvComb.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", TCombustible.First().ID);

                            decimal VCtoEntrada, vCtoFinal;
                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaCompra, out vCtoFinal, out VCtoEntrada);

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                            gvComb.SetRowCellValue(e.RowHandle, "CostoInicial", VCtoEntrada);

                            //////////

                            decimal costo = 0;
                            if (Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                                costo = Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada"));

                        string texto = "";// InfoCombustible(Producto.ID, costo);
                        gvComb.SetRowCellValue(e.RowHandle, "DetalleCombustible", texto);

                        gvComb.SetRowCellValue(e.RowHandle, "Precio", Producto.MargenToleranciaCosto);
                        gvComb.SetRowCellValue(e.RowHandle, "EsProducto", Producto.AplicaISC);
                        gvComb.SetRowCellValue(e.RowHandle, "EsManejo", Producto.ExentoIVA);

                        //-- Cantidad Inicial de 1
                        if (Convert.ToDecimal(gvComb.GetRowCellValue(e.RowHandle, "CantidadEntrada")) == Convert.ToDecimal("0.00"))
                            gvComb.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
                //--
                #endregion

                #region <<< MARGEN_COSTO >>>

                
                #endregion
            //}
        }

        private void gvComb_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //if (!ValidarCampos(true))
            //{
            //    return;
            //}

            try
            {
                if (e.Column == ColAlmacen)
                {
                    if (gvComb.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (!Convert.ToInt32(gvComb.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                        {
                            var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvComb.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                            ColAlmacen.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);
                            cboAlmacen.DataSource = TCombustible;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


        private void SetValue(int i, decimal ISC, decimal Impuesto, decimal Costo, decimal Total)
        {
            _calculado = true;
            gvComb.SetRowCellValue(i, "MontoISC", ISC);
            gvComb.SetRowCellValue(i, "ImpuestoTotal", Impuesto);
            gvComb.SetRowCellValue(i, "CostoEntrada", Costo);

            if (EtPedido != null)
                gvComb.SetRowCellValue(i, "CostoTotal", Total);

                int prod = 0;
                if (Convert.ToDecimal(gvComb.GetRowCellValue(i, "ProductoID")) != Convert.ToDecimal("0.00"))
                    prod = Convert.ToInt32(gvComb.GetRowCellValue(i, "ProductoID"));

            string texto = "";// InfoCombustible(prod, Costo);
                gvComb.SetRowCellValue(i, "DetalleCombustible", texto);
           
            _calculado = false;
            gvComb.UpdateTotalSummary();
            //txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

        }

        private void gvComb_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {

            decimal suma = 0;
            if (DetalleK.Count > 0)
                suma = Decimal.Round(DetalleK.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

            e.TotalValue = suma;

        }

        private void gvComb_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            try
            {
                if (e.FocusedColumn == colUnit)
                {
                    gvComb.FocusedColumn = gvComb.VisibleColumns[2];
                    gvComb.ShowEditor();
                }

                if (e.FocusedColumn == ColAlmacen)
                {
                    if (ColAlmacen.ReadOnly)
                    {
                        gvComb.FocusedColumn = gvComb.VisibleColumns[3];
                        gvComb.ShowEditor();
                    }
                }

                if (e.FocusedColumn == colCostoInical)
                {
                    gvComb.FocusedColumn = colCostoTotal;
                    gvComb.ShowEditor();
                }

                if (e.FocusedColumn == colCost)
                {
                    gvComb.FocusedColumn = colCostoTotal;
                    gvComb.ShowEditor();
                }

                if (e.FocusedColumn == colTaxValue)
                {
                    gvComb.FocusedColumn = gvComb.VisibleColumns[Convert.ToInt32(gvComb.VisibleColumns.Count - 1)];
                    gvComb.ShowEditor();
                }

                if (e.FocusedColumn == colTotal)
                {
                    gvComb.FocusedColumn = gvComb.VisibleColumns[Convert.ToInt32(gvComb.VisibleColumns.Count - 1)];
                    gvComb.ShowEditor();
                }

            }
            catch
            {
                gvComb.FocusedColumn = gvComb.VisibleColumns[0];
                gvComb.ShowEditor();
            }
        }

        private void gvComb_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
                cboAlmacen.DataSource = Tanques;
        }

        private void gvComb_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        private void gvComb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridComb.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        //txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal) + K.Sum(s => s.MontoISC)).ToString("#,0.00");
                        //Decimal.Round(Convert.ToDecimal(gvComb.Columns["SubTotal"].SummaryText) + Convert.ToDecimal(gvComb.Columns["ImpuestoTotal"].SummaryText), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridComb.DefaultView;
                view.AddNewRow();
            }
        }

        private void gvComb_LostFocus(object sender, EventArgs e)
        {
                cboAlmacen.DataSource = Tanques;
        }

        private void gvComb_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            decimal _MargenCosto = 0;

            cboAlmacen.DataSource = Tanques;

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

            //-- Validar Columna de UnidadMedidaID             
            if (view.GetRowCellValue(RowHandle, "UnidadMedidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "UnidadMedidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["UnidadMedidaID"], "Debe Seleccionar la Unidad de Medida");
                    e.ErrorText = "Debe Seleccionar la Unidad de Medida";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["UnidadMedidaID"], "Debe Seleccionar la Unidad de Medida");
                e.ErrorText = "Debe Seleccionar la Unidad de Medida";
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

            //-- Validar Columna de Almacenes             
            if (view.GetRowCellValue(RowHandle, "AlmacenEntradaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen / Tanque");
                    e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen / Tanque");
                e.ErrorText = "Debe Seleccionar un Almacen / Tanque";
                e.Valid = false;
                Validate = false;
            }

            if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID"))) && t.ProductoID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).Equals(0))
            {
                view.SetColumnError(view.Columns["AlmacenEntradaID"], "El Tanque seleccionado no contiene este producto");
                e.ErrorText = "El Tanque seleccionado no contiene este producto";
                e.Valid = false;
                Validate = false;
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using (SAGAS.Contabilidad.Forms.Dialogs.DialogSeleccionarOrden dg = new SAGAS.Contabilidad.Forms.Dialogs.DialogSeleccionarOrden())
                {
                    dg.Text = "Pedidos Aprobadas";
                    //dg.layoutControlItemTexto.Text = "Pedidos : ";

                    if (dg.ShowDialog().Equals(DialogResult.OK))
                    {
                        EtPedido = db.Pedidos.Single(o => o.ID.Equals(dg.IDOrden));
                        List<Entidad.VistaPedidosAbierto> EtVistaPedidos = dg.EtPedidos;
                        IDProveedor = EtPedido.PedidoCombustibles.First().ProveedorID;
                        FechaCompra = EtPedido.FechaCreado;
                        _FechaCarga = dg.fCarga;
                        _CombCantidad = new List<Parametros.ListIdDisplayValue>();
                        var T = db.Tanques.Where(t => t.EstacionServicioID.Equals(EtPedido.EstacionID) && t.Activo);
                        bool t2 = false;
                        int tp = 0;
                        int tt = 0;
                        decimal tct = 0m;
                        K.Clear();

                        EtVistaPedidos.ForEach(p =>
                        {
                            //    p.PedidoCombustibleDetalles.ForEach(l =>
                            //{

                            if (T.Count(t => t.ProductoID.Equals(p.ProductoID)) > 1)
                            {
                                t2 = true;
                                tp = p.ProductoID;
                                tt = T.First(s => s.ProductoID.Equals(p.ProductoID)).ID;
                                tct = Decimal.Round((p.Costo * p.Galones) / Convert.ToDecimal(p.Litros), 4, MidpointRounding.AwayFromZero);
                            }

                            _CombCantidad.Add(new Parametros.ListIdDisplayValue { ID = p.ProductoID, Value = Convert.ToDecimal(p.Litros) });

                            K.Add(new Entidad.Kardex { ProductoID = p.ProductoID, UnidadMedidaID = 1, AlmacenEntradaID = T.First(s => s.ProductoID.Equals(p.ProductoID)).ID, CantidadEntrada = Convert.ToDecimal(p.Litros), CostoTotal = Decimal.Round(p.Costo * p.Galones, 2, MidpointRounding.AwayFromZero), CostoEntrada = Decimal.Round((p.Costo * p.Galones) / Convert.ToDecimal(p.Litros), 4, MidpointRounding.AwayFromZero) }); // AlmacenEntradaID = l.AlmacenEntradaID, , CostoInicial = l.UltimoCosto, CostoEntrada = l.Costo, ImpuestoTotal = l.ImpuestoTotal });
                                                                                                                                                                                                                                                                                                                                                                                                                                       //});
                        });

                        if (t2)
                        {
                            ColAlmacen.OptionsColumn.AllowFocus = true;
                            ColAlmacen.OptionsColumn.ReadOnly = false;
                            K.Add(new Entidad.Kardex { ProductoID = tp, UnidadMedidaID = 1, AlmacenEntradaID = T.First(s => s.ProductoID.Equals(tp) && !s.ID.Equals(tt)).ID, CantidadEntrada = 0m, CostoTotal = 0m, CostoEntrada = tct });
                        }

                        //Orden.OrdenCompraDetalles.ToList().ForEach(l =>
                        //{
                        //    K.Add(new Entidad.Kardex { ProductoID = l.ProductoID, UnidadMedidaID = l.UnidadMedidaID, AlmacenEntradaID = l.AlmacenEntradaID, CantidadEntrada = l.CantidadEntrada, CostoInicial = l.UltimoCosto, CostoEntrada = l.Costo, CostoTotal = l.CostoTotal, ImpuestoTotal = l.ImpuestoTotal });
                        //});

                        gridProductos.DataSource = from P in db.Productos
                                                   join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                   join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                   join A in db.Areas on C.AreaID equals A.ID
                                                   where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && A.Activo && C.Activo
                                                   select new
                                                   {
                                                       P.ID,
                                                       P.Codigo,
                                                       P.Nombre,
                                                       UmidadName = U.Nombre,
                                                       Display = P.Codigo + " | " + P.Nombre
                                                   };

                        //this.emptySpaceItemOrden.TextVisible = true;
                        //this.emptySpaceItemOrden.Text = "Pedido : " + EtPedido.Numero;
                        this.slLabel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.slLabel.TextVisible = true;
                        this.slLabel.Text = "Pedido : " + EtPedido.Numero + " Fecha Carga: " + _FechaCarga.ToShortDateString();

                        colProduct.OptionsColumn.AllowFocus = colUnit.OptionsColumn.AllowFocus = colCostoInical.OptionsColumn.AllowFocus = colCost.OptionsColumn.AllowFocus = colCostoTotal.OptionsColumn.AllowFocus = colTaxValue.OptionsColumn.AllowFocus = colTotal.OptionsColumn.AllowFocus = false;
                        colProduct.OptionsColumn.ReadOnly = colUnit.OptionsColumn.ReadOnly = colCostoInical.OptionsColumn.ReadOnly = colCost.OptionsColumn.ReadOnly = colCostoTotal.OptionsColumn.ReadOnly = colTaxValue.OptionsColumn.ReadOnly = colTotal.OptionsColumn.ReadOnly = true;

                        if (EtPedido.EsIECC)
                        {
                            colCostoTotal.OptionsColumn.AllowFocus = true;
                            colCostoTotal.OptionsColumn.ReadOnly = false;
                        }

                        this.gvComb.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                        this.gvComb.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                        this.gvComb.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                        this.gvComb.RefreshData();

                        this.layoutControlItemMostrar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        this.chkBtnComb.Checked = true;
                        //txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void chkBtnComb_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBtnComb.Checked)
            {
                this.splitCentral.PanelVisibility = SplitPanelVisibility.Both;
                //this.chkBtnImpuesto.Image = Tesoreria.Properties.Resources.Ocultar;
                this.chkBtnComb.Text = "Ocultar Combustibles";
            }
            else if (!chkBtnComb.Checked)
            {
                this.splitCentral.PanelVisibility = SplitPanelVisibility.Panel1;
                //this.chkBtnComb.Image = Tesoreria.Properties.Resources.Mostrar;
                this.chkBtnComb.Text = "Mostrar Combustibles";
            }
        }

        #endregion


    }
}