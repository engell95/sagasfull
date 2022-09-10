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
    public partial class DialogCompras : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCompras MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal bool OnlyView = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool UnAlmacen = true;
        private int _CuentaIVACredito = Parametros.Config.IVAPorAcreditar();
        private int _CuentaIVAContado = Parametros.Config.IVAAcreditado();
        private decimal _ValorIVA = 0;
        private bool NextCompra = false;
        private bool RefreshMDI = false;
        private bool _ISCCalculating = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private int ProductoAreaServicioID = Parametros.Config.ProductoAreaServicioID();
        private int ProveedorNoRegistrado = Parametros.Config.ProveedorNoRegistradoID();
        private int _CajaChicaID = 0;
        private int _CuentaContableCajaChicaID = 0;
        private bool _EsContado = false;
        private bool _ChangeTax = false;
        private bool _ChangeTC = false;
        private bool _ChangeAlma = false;
        private bool _AplicaAlcaldia = false;
        private bool _Guardado = false;
        private decimal _MargenIVA = Parametros.Config.MargenValorCambioIVA();
        private decimal _MargenTC = Parametros.Config.MargenValorCambioTC();
        private decimal _MargenValorCambioAlcaldia = Parametros.Config.MargenValorCambioAlcaldia();
        private decimal _OldTax = 0;
        private decimal _OldTC = 0;
        private decimal _OldAlma = 0;
        private decimal _costoTotal = 0;
        private bool _ToPrint = false;
        private int IDPrint = 0;
        private int _IDSubSerie = 0;
        private IQueryable<Parametros.ListIdDisplay> Tanques;
        private Entidad.OrdenCompra Orden;
        internal decimal vTotalPagar = 0m;
        private int _ClaseCombustible;
        
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
        
        private int IDAlmacen
        {
            get { return Convert.ToInt32(lkAlmacen.EditValue); }
            set { lkAlmacen.EditValue = value; }
        }

        private DateTime FechaFisico
        {
            get { return Convert.ToDateTime(dateFechaIngreso.EditValue); }
            set { dateFechaIngreso.EditValue = value; }
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

        private static Entidad.EstacionServicio ES;
        private static Entidad.Proveedor Provee;

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

        public DialogCompras(int UserID, bool _OnlyView)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            OnlyView = _OnlyView;

            if (OnlyView)
            { //-- Bloquear Controles --//               
                glkProvee.Properties.ReadOnly = true;
                lkMoneda.Properties.ReadOnly = true;
                rgCreditoContado.Properties.ReadOnly = true;
                txtReferencia.Properties.ReadOnly = true;
                dateFechaCompra.Properties.ReadOnly = true;
                lkAlmacen.Properties.ReadOnly = true;
                chkEsCombustible.Properties.ReadOnly = true;
                chkAplicaISC.Properties.ReadOnly = true;
                glkProvee.Properties.ReadOnly = true;
                dateFechaVencimiento.Properties.ReadOnly = true;
                mmoComentario.Properties.ReadOnly = true;
                dateFechaIngreso.Properties.ReadOnly = true;
                gvDetalle.OptionsBehavior.Editable = false;
                gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
                colCostoInical.Visible = false;
                btnOK.Enabled = false;
                btnOK.Visible = false;
                spTC.Properties.ReadOnly = true;
                chkIVA.Properties.ReadOnly = true;
                spISC.Properties.ReadOnly = true;
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
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (OnlyView ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                rgCreditoContado.Properties.Items[2].Enabled = false;
                _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                this._ValorIVA = 0.15m;// Decimal.Round((db.CuentaContables.Single(c => c.ID.Equals(15)).Porcentaje / 100m), 2, MidpointRounding.AwayFromZero);
                
                //Proveedor
                glkProvee.Properties.DataSource = (from p in db.Proveedors
                                                   where (p.Activo || OnlyView) && !(db.EstacionServicios.Any(es => es.ProveedorCajaChicaID.Equals(p.ID)))
                                                   select new { p.ID, p.Codigo, p.Nombre, p.NombreComercial, Display = p.Codigo + " | " + p.Nombre }).OrderBy(o => o.Codigo);
                glkProvee.Properties.DisplayMember = "Display";
                glkProvee.Properties.ValueMember = "ID";

                //Tipo de Moneda
                lkMoneda.Properties.DataSource = from m in db.Monedas select new { m.ID, Display = m.Simbolo + " | " + m.Nombre};
                lkMoneda.Properties.DisplayMember = "Display";
                lkMoneda.Properties.ValueMember = "ID";

                //--- Fill Combos Detalles --//
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                gridProductos.DataSource = null;
                gridProductos.DataSource = from P in db.Productos
                                           join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                           join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                           join A in db.Areas on C.AreaID equals A.ID
                                           where P.Activo.Equals(true) && !P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && !C.AreaID.Equals(ProductoAreaServicioID) && A.Activo && C.Activo && !A.AplicaOrdenCompra
                                           select new
                                           {
                                               P.ID,
                                               P.Codigo,
                                               P.Nombre,
                                               UmidadName = U.Nombre,
                                               Display = P.Codigo + " | " + P.Nombre
                                           };

                

                dateFechaCompra.EditValue = Convert.ToDateTime(db.GetDateServer());
                dateFechaIngreso.EditValue = Convert.ToDateTime(db.GetDateServer());
                IDMonedaPrincipal = Parametros.Config.MonedaPrincipal();

                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaFisico.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaFisico.Date)).First().Valor : 0m);


                if (OnlyView)
                {
                    rgCreditoContado.SelectedIndex = (EntidadAnterior.Credito ? 0 : 1);
                    FechaCompra = EntidadAnterior.FechaRegistro;
                    IDProveedor = (EntidadAnterior.Credito ? EntidadAnterior.ProveedorID : EntidadAnterior.ProveedorReferenciaID);
                    if (!EntidadAnterior.Credito)
                    {
                        _CajaChicaID = EntidadAnterior.ProveedorID;
                        txtCajaChica.Text = db.Proveedors.Single(p => p.ID.Equals(_CajaChicaID)).Nombre;
                    }

                    Referencia = EntidadAnterior.Referencia;
                    IDAlmacen = EntidadAnterior.AlmacenID;
                    FechaFisico = Convert.ToDateTime(EntidadAnterior.FechaFisico);
                    Comentario = EntidadAnterior.Comentario;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;
                    IDMonedaPrincipal = EntidadAnterior.MonedaID;
                    FechaVencimiento = Convert.ToDateTime(EntidadAnterior.FechaVencimiento);
                    _TipoCambio = EntidadAnterior.TipoCambio;
                    spNumero.Value = EntidadAnterior.Numero;

                    //--- Fill Combos Detalles --//
                    gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                    gridProductos.DataSource = null;
                    gridProductos.DataSource = from P in db.Productos
                                               join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                               select new
                                               {
                                                   P.ID,
                                                   P.Codigo,
                                                   P.Nombre,
                                                   UmidadName = U.Nombre,
                                                   Display = P.Codigo + " | " + P.Nombre
                                               };



                    K = EntidadAnterior.Kardexes.ToList();
                    if (!K.Sum(s => s.MontoISC).Equals(0))
                    {
                        chkAplicaISC.Checked = true;
                        spISC.Value = K.Sum(s => s.MontoISC);
                    }

                    var ProdCombustibles = db.Productos.Where(p => p.Activo && p.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()));

                    // if (K.Any(kr => kr.ProductoID.Equals(ProdCombustibles.)))

                    if (!EntidadAnterior.MonedaID.Equals(Parametros.Config.MonedaPrincipal()))
                    {
                        DetalleK.ForEach(lista =>
                            {
                                lista.CostoEntrada = Decimal.Round((lista.CostoEntrada / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                lista.CostoTotal = Decimal.Round((lista.CostoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                lista.ImpuestoTotal = Decimal.Round((lista.ImpuestoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                lista.MontoISC = Decimal.Round((lista.MontoISC / _TipoCambio), 4, MidpointRounding.AwayFromZero);
                                lista.SubTotal = Decimal.Round((lista.SubTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            });
                    }

                    if (K.Count(kr => (ProdCombustibles.Any(p => p.ID.Equals(kr.ProductoID)))) > 0)
                    {
                        chkEsCombustible.Checked = true;
                        cboAlmacen.DataSource = Tanques;
                    }
                    gvDetalle.RefreshData();

                    txtGrandTotal.Text = Decimal.Round(Convert.ToDecimal(DetalleK.Sum(s => s.CostoTotal + s.ImpuestoTotal)), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");



                }

                

                //---LLenar Almacenes ---//
                IQueryable<Parametros.ListIdDisplay> listaAlmacen;

                if (chkEsCombustible.Checked)
                    listaAlmacen = from al in db.Tanques
                                   where (al.Activo || OnlyView) && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                   select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };
                else
                    listaAlmacen = from al in db.Almacens
                                   where (al.Activo || OnlyView) && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                   select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };

                UnAlmacen = (listaAlmacen.ToList().Count > 1 ? false : true);
                //Almacen GRID
                cboAlmacen.DataSource = listaAlmacen.ToList();
                cboAlmacen.DisplayMember = "Display";
                cboAlmacen.ValueMember = "ID";
                //Almacen FORM
                lkAlmacen.Properties.DataSource = listaAlmacen.ToList();
                lkAlmacen.Properties.DisplayMember = "Display";
                lkAlmacen.Properties.ValueMember = "ID";

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

                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//

                this.bdsDetalle.DataSource = this.DetalleK;
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
            Parametros.General.ValidateEmptyStringRule(glkProvee, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(dateFechaVencimiento, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
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

        private bool ValidarNumeroCajaChica(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? (i.MovimientoTipoID.Equals(41) || i.MovimientoTipoID.Equals(3)) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(_IDSubSerie) && i.ID != Convert.ToInt32(ID) :
                          (i.MovimientoTipoID.Equals(41) || i.MovimientoTipoID.Equals(3)) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio)) && i.SubEstacionID.Equals(_IDSubSerie)
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

                if (dateFechaCompra.EditValue == null || dateFechaVencimiento.EditValue == null || lkMoneda.EditValue == null || txtReferencia.Text == "" || mmoComentario.Text == "")
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidateKardexMovemente(Convert.ToDateTime(dateFechaCompra.EditValue), db, IDEstacionServicio, IDSubEstacion, (chkEsCombustible.Checked ? 24 : 9), 0))
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Convert.ToDateTime(dateFechaCompra.EditValue).ToShortDateString(), Parametros.MsgType.warning);
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

                if (FechaVencimiento.Date < FechaFisico.Date)
                {
                    Parametros.General.DialogMsg("La fecha de vencimiento debe ser mayor a la fecha de compra.", Parametros.MsgType.warning);
                    return false;
                }

                if (FechaCompra.Date > Convert.ToDateTime(db.GetDateServer()).Date || FechaFisico.Date > Convert.ToDateTime(db.GetDateServer()).Date)
                {
                    Parametros.General.DialogMsg("La fecha de recibido o de factura no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidatePeriodoContable(FechaCompra, db, IDEstacionServicio))
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
                        Parametros.General.DialogMsg("Debe de ingresar detalle a la compra." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    else
                    {
                        //INVENTARIO BLOQUEADO DIFERENTECOMBUSTIBLE
                        List<int> Areas = (from p in db.Productos
                                           join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                           where K.Select(s => s.ProductoID).Contains(p.ID)
                                           group pc by pc.AreaID into gr
                                           select gr
                                           ).Select(s => s.Key).ToList();

                        bool Bloqueo = false;

                        foreach (int obj in Areas)
                        {
                            if (!Parametros.General.ValidateKardexMovemente(FechaCompra, db, IDEstacionServicio, IDSubEstacion, 9, obj))
                            {
                                Bloqueo = true;
                                break;
                            }
                        }

                        if (Bloqueo)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaCompra.ToShortDateString(), Parametros.MsgType.warning);
                            return false;
                        }

                        //BLOQUEO PARA COMBUSTIBLE
                        List<int> PAreas = (from p in db.Productos
                                            join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                            where K.Select(s => s.ProductoID).Contains(p.ID) && pc.ID.Equals(_ClaseCombustible)
                                            group pc by pc.AreaID into gr
                                            select gr
                                           ).Select(s => s.Key).ToList();

                        if (PAreas.Count > 0)
                        {
                            if (!Parametros.General.ValidateKardexMovemente(FechaCompra, db, IDEstacionServicio, IDSubEstacion, 24, 0))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaCompra.ToShortDateString(), Parametros.MsgType.warning);
                                return false;
                            }
                        }
                    }

                    if (!ValidarReferencia(Convert.ToString(txtReferencia.Text)))
                    {
                        Parametros.General.DialogMsg("La referencia para esta compra ya existe : " + Convert.ToString(txtReferencia.Text), Parametros.MsgType.warning);
                        return false;
                    }

                    if (!chkAplicaISC.Checked)
                    {
                        var ProdISC = db.Productos.Where(p => p.AplicaISC).Select(s => s.ID).ToList();

                        var lista = from k in DetalleK
                                    where ProdISC.Contains(k.ProductoID)
                                    select new { ProdISC };

                        //(from k in DetalleK
                        //         join p in db.Productos on k.ProductoID equals p.ID
                        //         where p.AplicaISC
                        //         select k).ToList();

                        if (lista.Count() > 0)
                        {
                            if (Parametros.General.DialogMsg("Existen productos en el detalle de la lista que aplican ISC.¿Desea guardar sin aplicar ISC?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (spISC.Value <= 0)
                        {
                            Parametros.General.DialogMsg("El valor ISC no puede ser igual a 0.", Parametros.MsgType.warning);
                            return false;
                        }
                    }

                    if (_EsContado)
                    {
                        if (Provee.AplicaRetencion)
                        {
                            if (_costoTotal >= db.CuentaContables.Single(s => s.ID.Equals(Provee.ImpuestoRetencionID)).Limite)
                            {
                                if (spIRReferencia.Value.Equals(0))
                                {
                                    Parametros.General.DialogMsg("Debe ingresar el Nro. de Retención IR en la fuente.", Parametros.MsgType.warning);
                                    return false;
                                }
                            }
                        }

                        if (_AplicaAlcaldia)
                        {
                            if (_costoTotal >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || Provee.TipoProveedorID.Equals(ProveedorNoRegistrado))
                            {
                                if (spAlcaldiaReferencia.Value.Equals(0))
                                {
                                    Parametros.General.DialogMsg("Debe ingresar el Nro. de Retención Alcaldía.", Parametros.MsgType.warning);
                                    return false;
                                }
                            }
                        }

                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return false;
            }
        }

        private bool RetieneAlcadia()
        {
            try
            {
                if (Provee != null)
                {
                    if (Provee.AplicaAlcaldia)
                    {
                        var ES = db.EstacionServicios.SingleOrDefault(es => es.ID.Equals(IDEstacionServicio));

                        if (ES != null)
                        {
                            if (ES.AplicaRetencionAlcaldia)
                            {
                                if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(Provee.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    else 
                        return false;
                }
                else
                    return false;

            }
            catch { return false; }
        }
        
        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (Convert.ToDecimal(txtGrandTotal.Text) <= 0)
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 2000;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                        Entidad.Movimiento M;

                        M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 3;
                        M.ProveedorID = (_EsContado ? _CajaChicaID : IDProveedor);
                        M.ProveedorReferenciaID = (_EsContado ? IDProveedor : 0);
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaCompra;
                        M.FechaFisico = FechaFisico;
                        M.MonedaID = IDMonedaPrincipal;
                        M.TipoCambio = _TipoCambio;
                        M.Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(txtGrandTotal.Text) :
                                    Decimal.Round((Convert.ToDecimal(txtGrandTotal.Text) * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                        M.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round((M.Monto / M.TipoCambio), 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(txtGrandTotal.Text));
                        M.Credito = (_EsContado ? false : true);
                        M.CuentaID = (_EsContado ? 0 : db.Proveedors.Single(s => s.ID.Equals(IDProveedor)).CuentaContableID);
                        M.Referencia = Referencia;
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = Comentario;
                        M.AlmacenID = IDAlmacen;
                        M.FechaVencimiento = FechaVencimiento;
                        M.Numero = (_EsContado ? Parametros.General.GetNroSerie(IDEstacionServicio, _IDSubSerie, 41, db) : 0);


                        if (_EsContado && M.Numero > 0)
                        {
                            var SerieUso = db.Series.Where(s => s.EstacionServicioID.Equals(M.EstacionServicioID) && s.SubEstacionID.Equals(_IDSubSerie) && s.MovimientoTipoID.Equals(41)).ToList();

                            if (!ValidarNumeroCajaChica(Convert.ToInt32(M.Numero), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("El número consecutivo de la caja chica '" + M.Numero + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }

                            if (String.IsNullOrEmpty(txtEntregado.Text))
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("Favor digite a quien será entregado el desembolso.", Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }

                            M.Entregado = txtEntregado.Text;

                            if (SerieUso.Count > 0)
                            {
                                Entidad.Series SR = db.Series.First(s => s.EstacionServicioID.Equals(M.EstacionServicioID) && s.SubEstacionID.Equals(_IDSubSerie) && s.MovimientoTipoID.Equals(41));
                                SR.NumeroActual = M.Numero;
                                db.SubmitChanges();

                            }                        
                        }
                    
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la Compra: " + M.Referencia, this.Name);
                        decimal _IVA = 0, _SubTotal = 0;

                        db.SubmitChanges();

                        #region <<< CONTADO >>>

                        if (!M.Credito)
                        {
                            if (Provee.AplicaRetencion)
                            {
                                var P = db.Proveedors.Single(pr => pr.ID.Equals(IDProveedor)).ImpuestoRetencionID;
                                M.Retenciones.Add(new Entidad.Retencione { Monto = Convert.ToDecimal(spIR.Value), Numero = Convert.ToInt32(spIRReferencia.Value), SubTotal = _costoTotal, Concepto = M.Comentario, CuentaContableID = P });
                                db.SubmitChanges();

                                    int n = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                    if (n > 0)
                                        db.SeriesRetenciones.Single(s => s.EstacionServicioID.Equals(IDEstacionServicio) && s.SubEstacionServicioID.Equals(IDSubEstacion) && s.EsAlcaldia.Equals(false)).Numero = n + 1;
                         
                                db.SubmitChanges();
                            }

                            if (_AplicaAlcaldia)
                            {
                                M.Retenciones.Add(new Entidad.Retencione { Monto = Convert.ToDecimal(spAlcaldia.Value), Numero = Convert.ToInt32(spAlcaldiaReferencia.Value), SubTotal = _costoTotal, EsAlcaldia = true, Concepto = M.Comentario, CuentaContableID = 231 });
                                db.SubmitChanges();

                                    int n = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                    if (n > 0)
                                        db.SeriesRetenciones.Single(s => s.EstacionServicioID.Equals(IDEstacionServicio) && s.SubEstacionServicioID.Equals(IDSubEstacion) && s.EsAlcaldia.Equals(true)).Numero = n + 1;
                              
                            }
                        }

                        #endregion

                        #region ::: REGISTRANDO EN KARDEX DE BD :::

                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        foreach (var dk in DetalleK)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            //decimal CostoMov = LineaDetalle.Cost;
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.MovimientoID = M.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;                            
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = Convert.ToDateTime(M.FechaRegistro).Date;                            
                            KX.AlmacenEntradaID = dk.AlmacenEntradaID;
                            KX.CantidadEntrada = dk.CantidadEntrada;
                            KX.CostoTotal = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero) :
                                    Decimal.Round((dk.CostoTotal * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                            _SubTotal += Decimal.Round(KX.CostoTotal, 2, MidpointRounding.AwayFromZero);
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            if (!chkEsCombustible.Checked)
                            {
                                //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                                //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                                
                                KX.CantidadInicial = Parametros.General.SaldoKardexCompra(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.Fecha, true); 
                                     
                            }
                            else if (chkEsCombustible.Checked)
                            {
                                KX.DetalleCombustible = dk.DetalleCombustible;
                                KX.CantidadInicial = Parametros.General.SaldoKardexCompra(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, KX.Fecha, true);
                            }

                            //if (KX.CantidadInicial <= 0)
                            //{
                            //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            //    trans.Rollback();
                            //    Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " no tiene existencia, favor revisar.", Parametros.MsgType.warning);
                            //    return false;
                            //}

                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;

                            //if (KX.CantidadFinal <= 0)
                            //{
                            //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            //    trans.Rollback();
                            //    Parametros.General.DialogMsg("El producto " + Producto.Codigo + " | " + Producto.Nombre + " no tiene existencia, favor revisar el inventario.", Parametros.MsgType.warning);
                            //    return false;
                            //}

                            //Cambiar tipo de costeo            
                            decimal VCtoEntrada, vCtoFinal;
                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaCompra, out vCtoFinal, out VCtoEntrada);

                            KX.CostoInicial = Math.Abs(Decimal.Round(vCtoFinal, 4, MidpointRounding.AwayFromZero));

                            KX.CostoEntrada = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(dk.CostoEntrada, 4, MidpointRounding.AwayFromZero) :
                                    Decimal.Round((dk.CostoEntrada * M.TipoCambio), 4, MidpointRounding.AwayFromZero));

                            decimal vSaldo = 0m;
                            Parametros.General.SaldoCostoTotalKardex(db, KX.ID, 0, IDEstacionServicio, IDSubEstacion, Producto.ID, FechaCompra, out vSaldo);

                            KX.CostoFinal = Math.Abs(Decimal.Round(Convert.ToDecimal((vSaldo + KX.CostoTotal) / (KX.CantidadFinal.Equals(0m) ? 1 : Math.Abs(KX.CantidadFinal))), 4, MidpointRounding.AwayFromZero));
                            
                            ////////////////////////////////////////////////////////////////////////////////

                            //KX.CostoFinal = Decimal.Round(Convert.ToDecimal(KX.CostoEntrada), 4, MidpointRounding.AwayFromZero);

                            KX.ImpuestoTotal = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? dk.ImpuestoTotal :
                                    Decimal.Round((dk.ImpuestoTotal * M.TipoCambio), 2, MidpointRounding.AwayFromZero));

                            _IVA += Decimal.Round(KX.ImpuestoTotal, 2, MidpointRounding.AwayFromZero);

                            if (chkAplicaISC.Checked)
                            {
                                KX.MontoISC = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(dk.MontoISC, 4, MidpointRounding.AwayFromZero) :
                                    Decimal.Round((dk.MontoISC * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                                KX.SubTotal = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(dk.SubTotal, 2, MidpointRounding.AwayFromZero) :
                                    Decimal.Round((dk.SubTotal * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                            }

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                            if (!chkEsCombustible.Checked)
                            {
                                var AL = (from al in db.AlmacenProductos
                                          where al.ProductoID.Equals(Producto.ID)
                                            && al.AlmacenID.Equals(KX.AlmacenEntradaID)
                                          select al).ToList();

                                if (AL.Count() == 0) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                                {
                                    //-- CREAR NUEVO REGISTRO 
                                    Entidad.AlmacenProducto AP = new Entidad.AlmacenProducto();
                                    AP.ProductoID = Producto.ID;
                                    AP.AlmacenID = KX.AlmacenEntradaID;
                                    AP.Cantidad = KX.CantidadFinal;
                                    decimal precio = Decimal.Round((KX.CostoEntrada * 1.35m), 6, MidpointRounding.AwayFromZero);
                                    if (KX.ImpuestoTotal > 0)
                                        precio += Decimal.Round((precio * 0.15m), 6, MidpointRounding.AwayFromZero);
                                    //-- INSERTAR REGISTRO 
                                    AP.PrecioSugerido = precio;
                                    AP.Costo = KX.CostoFinal;
                                    db.AlmacenProductos.InsertOnSubmit(AP);
                                }
                                else //-- SI HAY REGISTRO DE EXISTENCIA ACTUALIZAR CANTIDAD CON COMPRA
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(KX.AlmacenEntradaID));
                                    AP.Cantidad = KX.CantidadFinal;// + AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad;

                                    decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                    if (KX.ImpuestoTotal > 0)
                                        precio += Decimal.Round((KX.CostoFinal * 0.15m), 6, MidpointRounding.AwayFromZero);
                                    //-- INSERTAR REGISTRO 
                                    AP.Costo = KX.CostoFinal;
                                    AP.PrecioSugerido = precio;
                                }
                            }
                            else if (chkEsCombustible.Checked)
                            {
                                var TP = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(KX.AlmacenEntradaID)
                                          select tp).ToList();

                                if (TP.Count() == 0) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                                {
                                    //-- CREAR NUEVO REGISTRO 
                                    Entidad.TanqueProducto T = new Entidad.TanqueProducto();
                                    T.ProductoID = Producto.ID;
                                    T.TanqueID = KX.AlmacenEntradaID;
                                    T.Cantidad = KX.CantidadFinal;
                                    T.Costo = KX.CostoFinal;
                                    db.TanqueProductos.InsertOnSubmit(T);
                                }
                                else //-- SI HAY REGISTRO DE EXISTENCIA ACTUALIZAR CANTIDAD CON COMPRA
                                {
                                    Entidad.TanqueProducto T = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(dk.AlmacenEntradaID));
                                    T.Cantidad = KX.CantidadFinal;
                                    T.Costo = KX.CostoFinal;
                                    db.SubmitChanges();
                                }
                            }
                            
                            #endregion

                                #region <<< ACTUALIZAR COSTOS >>>
                            /*
                                var ActualizarCosto = (from k in db.Kardexes
                                                       join m in db.Movimientos on k.Movimiento equals m
                                                       join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                       where k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && k.ProductoID.Equals(KX.ProductoID) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                                                       && k.Fecha.Date >= KX.Fecha.Date
                                                       select new { k.ID, k.Fecha, mt.Entrada }).OrderBy(o => o.Fecha).ThenBy(t => !t.Entrada).ToList();

                                var KCIList = (from k in db.Kardexes
                                           join m in db.Movimientos on k.MovimientoID equals m.ID
                                               where k.ProductoID.Equals(KX.ProductoID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                             && k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && !k.CostoEntrada.Equals(0)
                                             && k.Fecha.Date <= KX.Fecha.Date
                                           select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                                decimal vCosto = 0m;
                                if (KCIList.Count() > 0)
                                    vCosto = Decimal.Round(KCIList.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                                foreach (var k in ActualizarCosto)
                                {
                                    if (!k.ID.Equals(KX.ID))
                                    {
                                        Entidad.Kardex NK = db.Kardexes.Single(o => o.ID.Equals(k.ID));

                                        if (NK.Movimiento.MovimientoTipoID.Equals(1) || NK.Movimiento.MovimientoTipoID.Equals(3))
                                        {

                                            //Costo Inicial es vCosto
                                            decimal vCantidades = 0m;
                                            //Cantidad Inicial para el calculo
                                            vCantidades = Parametros.General.SaldoKardex(db, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, 0, NK.Fecha, true);
                                            //Costo Total Inicial para el calculo
                                            //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);

                                            decimal vSaldoAct = 0m;
                                            Parametros.General.SaldoCostoTotalKardex(db, 0, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, NK.Fecha, out vSaldoAct);

                                            vCosto = Decimal.Round(Convert.ToDecimal(vSaldoAct / vCantidades), 4, MidpointRounding.AwayFromZero);

                                            if (NK.Fecha.Date >= KX.Fecha.Date)
                                                NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                        }
                                        else
                                        {

                                            NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                            if (!NK.CostoEntrada.Equals(0))
                                            {
                                                NK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                            }

                                            if (!NK.CostoSalida.Equals(0))
                                            {
                                                NK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                            }

                                            db.SubmitChanges();

                                            var Anterior = db.Kardexes.Where(o => o.MovimientoID.Equals(NK.MovimientoID) && !o.ID.Equals(NK.ID)).Select(s => new { s.ID, s.CostoFinal });

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoFinal), 2, MidpointRounding.AwayFromZero);

                                            //Entidad.Movimiento MK = db.Movimientos.Single(o => o.ID.Equals(NK.MovimientoID));

                                            var area = from a in db.Areas
                                                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                                       where p.ID.Equals(NK.ProductoID)
                                                       select a;

                                            if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                            {
                                                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                                if (NCC != null)
                                                {
                                                    if (NCC.Monto < 0)
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                    db.SubmitChanges();
                                                }
                                            }
                                            else
                                            {
                                                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaCostoID;

                                                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                                if (NCC != null)
                                                {
                                                    if (NCC.Monto < 0)
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                    db.SubmitChanges();
                                                }
                                            }

                                            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                            {
                                                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                                if (NCC != null)
                                                {
                                                    if (NCC.Monto < 0)
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                    db.SubmitChanges();
                                                }
                                            }
                                            else
                                            {
                                                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaInventarioID;

                                                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                                if (NCC != null)
                                                {
                                                    if (NCC.Monto < 0)
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                    db.SubmitChanges();
                                                }
                                            }

                                        }
                                    }
                                }
                             * */
                                #endregion

                            
                            //------------------------------------------------------------------------//

                            

                            //para que actualice los datos del registro
                            db.SubmitChanges();
                        }
                        
                        #endregion

                        if (M.Credito)
                        {
                            M.IVA = _IVA;
                            M.SubTotal = _SubTotal;
                            db.SubmitChanges();
                        }
                        else
                        {
                            M.IVA = _IVA;
                            M.SubTotal = _SubTotal;
                            M.Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(spTotal.Value) :
                                Decimal.Round((Convert.ToDecimal(spTotal.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                            M.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spTotal.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                               Convert.ToDecimal(spTotal.Value));
                            db.SubmitChanges();
                        }

                      
                        

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

                        #region <<< ORDEN_COMPRA >>>

                    if (Orden != null)
                    {
                        Entidad.OrdenCompra OC = db.OrdenCompras.Single(o => o.ID.Equals(Orden.ID));
                        OC.Estado = 5;
                        OC.MovimientoID = M.ID;
                        db.SubmitChanges();

                    }

                    #endregion

                    if (!M.Monto.Equals(vTotalPagar))
                        M.Monto = Decimal.Round(vTotalPagar, 2, MidpointRounding.AwayFromZero);

                    #region ::: REGISTRANDO DEUDOR :::

                    db.Deudors.InsertOnSubmit(new Entidad.Deudor { ProveedorID = (_EsContado ? _CajaChicaID : IDProveedor), Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                    db.SubmitChanges();
                    #endregion
                                                         
                    db.SubmitChanges();
                        trans.Commit();

                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        NextCompra = true;
                        RefreshMDI = true;
                        ShowMsg = true;
                        _Guardado = true;
                        _ToPrint = true;
                        IDPrint = M.ID;
                        return true;
                    
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

                //Entidad.Movimiento M;
                if (!OnlyView)
                {
                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                    List<Int32> ID = new List<Int32>();

                    #region <<< DETALLE_COMPROBANTE >>>
                    decimal _vMP = 0, _vMS = 0;
                    int i = 1;
                    DetalleK.ToList().ForEach(K =>
                        {
                            var area = from a in db.Areas
                                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                       where p.ID.Equals(K.ProductoID)
                                       select a;

                            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                            {
                                _vMP += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                                _vMS += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                    Convert.ToDecimal(K.CostoTotal));

                                if (!ID.Contains(area.First().CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable { CuentaContableID = area.First().CuentaInventarioID, 
                                        Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),                                    
                                    TipoCambio = _TipoCambio, 
                                    MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                    Convert.ToDecimal(K.CostoTotal)),
                                    Fecha = FechaCompra, Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text, Linea = i , EstacionServicioID = IDEstacionServicio, SubEstacionID = IDSubEstacion});
                                    ID.Add(area.First().CuentaInventarioID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                    comprobante.Monto += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                    Convert.ToDecimal(K.CostoTotal));
                                }
                            }
                            else
                            {
                                var producto = db.Productos.Single(p => p.ID.Equals(K.ProductoID));

                                _vMP += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                                _vMS += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                        Convert.ToDecimal(K.CostoTotal));

                                if (!ID.Contains(producto.CuentaInventarioID))
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = producto.CuentaInventarioID,
                                        Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                        Convert.ToDecimal(K.CostoTotal)),
                                        Fecha = FechaCompra, Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
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
                                    comprobante.Monto += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(K.CostoTotal) :
                                    Decimal.Round((Convert.ToDecimal(K.CostoTotal) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                                    Convert.ToDecimal(K.CostoTotal));
                                }
                            }
                            
                        });

                    #endregion
                    decimal _ivaMP = 0, _ivaMS = 0;
                    if (Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) > 0)
                    {
                        _ivaMP = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) :
                            Decimal.Round((Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                        _ivaMS = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                            Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText));

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = (_EsContado ? _CuentaIVAContado : _CuentaIVACredito),
                            Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) :
                            Decimal.Round((Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) * _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText) / _TipoCambio), 2, MidpointRounding.AwayFromZero) :
                            Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText)),
                            Fecha = FechaCompra, Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                    }                    

                    if (_EsContado)
                    {
                        if (!_CuentaContableCajaChicaID.Equals(0))
                        {
                            vTotalPagar = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(spTotal.Value) :
                                Decimal.Round((Convert.ToDecimal(spTotal.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaContableCajaChicaID,
                                Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? -Math.Abs(Convert.ToDecimal(spTotal.Value)) :
                                Decimal.Round(-Math.Abs((Convert.ToDecimal(spTotal.Value) * _TipoCambio)), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(spTotal.Value) / _TipoCambio)), 2, MidpointRounding.AwayFromZero) :
                                -Math.Abs(Convert.ToDecimal(spTotal.Value))),
                                Fecha = FechaCompra, Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        if (Provee != null)
                        {
                            if (Provee.AplicaRetencion)
                            {
                                //decimal valor = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(spIR.Value) :
                                //    Decimal.Round((Convert.ToDecimal(spIR.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                                //if (valor >= db.CuentaContables.Single(s => s.ID.Equals(Provee.ImpuestoRetencionID)).Limite)
                                //{
                                if (spIR.Value > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = Provee.ImpuestoRetencionID,
                                        Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? -Math.Abs(Convert.ToDecimal(spIR.Value)) :
                                        Decimal.Round(-Math.Abs((Convert.ToDecimal(spIR.Value) * _TipoCambio)), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(spIR.Value) / _TipoCambio)), 2, MidpointRounding.AwayFromZero) :
                                        -Math.Abs(Convert.ToDecimal(spIR.Value))),
                                        Fecha = FechaCompra,
                                        Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }
                                //}
                            }

                            if (_AplicaAlcaldia)
                            {
                                //decimal valor = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(spAlcaldia.Value) :
                                //    Decimal.Round((Convert.ToDecimal(spAlcaldia.Value) * _TipoCambio), 2, MidpointRounding.AwayFromZero));

                                //if (valor >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite)
                                //{
                                if (spAlcaldia.Value > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = 231,
                                        Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? -Math.Abs(Convert.ToDecimal(spAlcaldia.Value)) :
                                        Decimal.Round(-Math.Abs((Convert.ToDecimal(spAlcaldia.Value) * _TipoCambio)), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(-Math.Abs(Convert.ToDecimal(Convert.ToDecimal(spAlcaldia.Value) / _TipoCambio)), 2, MidpointRounding.AwayFromZero) :
                                        -Math.Abs(Convert.ToDecimal(spAlcaldia.Value))),
                                        Fecha = FechaCompra,
                                        Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }
                            }
                        }

                    }
                    else if (!_EsContado)
                    {
                        if (Provee != null)
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Provee.CuentaContableID,
                                Monto = -Math.Abs(Decimal.Round(_vMP + _ivaMP, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_vMS + _ivaMS, 2, MidpointRounding.AwayFromZero)),
                                Fecha = FechaCompra, Descripcion = (_EsContado ? txtCajaChica.Text : Provee.Nombre) + " – Factura Nro. " + txtReferencia.Text,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            vTotalPagar = _vMP + _ivaMP;
                        }
                    }

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
            MDI.CleanDialog(ShowMsg, NextCompra, RefreshMDI, IDPrint, _ToPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !OnlyView && !NextCompra)
            {
                if (DetalleK.Count > 0 || txtReferencia.Text != "" || Provee != null)
                {
                    if (Parametros.General.DialogMsg("La compra actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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
                    FechaVencimiento = FechaFisico.AddDays(Provee.Plazo);
                    _AplicaAlcaldia = false;
 
                    if (_EsContado)
                    {
                        _AplicaAlcaldia = RetieneAlcadia();
                        if (Provee.AplicaRetencion || _AplicaAlcaldia)
                        {
                            layoutControlContado.Visible = true;

                            if (Provee.AplicaRetencion)
                            {
                                emptySpaceItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                layoutControlItemNroIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                layoutControlItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                                int xr = 0;

                                xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                if (xr > 0)
                                {
                                    spIRReferencia.Value = xr;
                                    spIRReferencia.Properties.AllowFocused = false;
                                    spIRReferencia.Properties.ReadOnly = true;
                                }
                                else
                                {
                                    spIRReferencia.Value = xr;
                                    spIRReferencia.Properties.AllowFocused = true;
                                    spIRReferencia.Properties.ReadOnly = false;
                                }


                            }
                            else if (!Provee.AplicaRetencion)
                            {
                                emptySpaceItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                layoutControlItemNroIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                layoutControlItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                spIRReferencia.Value = 0;
                            }

                            if (_AplicaAlcaldia)
                            {
                                layoutControlItemNroAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                emptySpaceItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                layoutControlItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                                int xa = 0;

                                xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                if (xa > 0)
                                {
                                    spAlcaldiaReferencia.Value = xa;
                                    spAlcaldiaReferencia.Properties.AllowFocused = false;
                                    spAlcaldiaReferencia.Properties.ReadOnly = true;
                                }
                                else
                                {
                                    spAlcaldiaReferencia.Value = xa;
                                    spAlcaldiaReferencia.Properties.AllowFocused = true;
                                }
                                    
                            }
                            else if (!_AplicaAlcaldia)
                            {
                                layoutControlItemNroAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                emptySpaceItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                layoutControlItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            }
                        }
                        else
                        {
                            emptySpaceItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            layoutControlItemNroIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            layoutControlItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                                        
                            layoutControlItemNroAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            emptySpaceItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            layoutControlItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        }
                    }
                    else
                    {
                        layoutControlItemNroAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        emptySpaceItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItemAlcaldia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        emptySpaceItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItemNroIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlItemIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        
                        layoutControlContado.Visible = false;                            
                    }

                    rgCreditoContado.Properties.ReadOnly = true;
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
            DevExpress.XtraGrid.Views.Grid.GridView  view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
           
            decimal _MargenCosto = 0;
            if (chkEsCombustible.Checked)
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

            if (chkEsCombustible.Checked)
            {
                if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID"))) && t.ProductoID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).Equals(0))
                {
                    view.SetColumnError(view.Columns["AlmacenEntradaID"], "El Tanque seleccionado no contiene este producto");
                    e.ErrorText = "El Tanque seleccionado no contiene este producto";
                    e.Valid = false;
                    Validate = false;
                }
            }


            decimal NewPrice = 0;
            decimal OldPrice = 0;
            bool _OutMargen = false;

            if (view.GetRowCellValue(RowHandle, "CostoEntrada") != null)
            {
                if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoEntrada")).Equals(0))
                {
                    NewPrice = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoEntrada")) :
                        Decimal.Round((Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoEntrada")) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                }
            }

            if (view.GetRowCellValue(RowHandle, "CostoInicial") != null)
            {
                if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoInicial")).Equals(0))
                {
                    OldPrice = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoInicial"));
                }
            }

            if (!NewPrice.Equals(0) & !OldPrice.Equals(0))
            {
                _MargenCosto = db.Productos.SingleOrDefault(o => o.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).MargenToleranciaCosto;
               
                decimal valor = 0;

                if (!OldPrice.Equals(0))
                    valor = Decimal.Round(((NewPrice / OldPrice) * 100), 4, MidpointRounding.AwayFromZero);

                if (NewPrice > OldPrice)
                {
                    if ((valor - 100) > _MargenCosto)
                        _OutMargen = true;
                    //view.SetColumnError(colCost, "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%", ErrorType.Critical);

                }
                else if (NewPrice < OldPrice)
                {
                    if ((100 - valor) > _MargenCosto)
                        _OutMargen = true;
                }

                if (_OutMargen)
                {
                    view.SetColumnError(view.Columns["CostoEntrada"], "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%");
                    e.ErrorText = "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%";
                    e.Valid = false;
                    Validate = false;
                }
            }
            //if (chkAplicaISC.Checked)
            //    CalculoISC();

        }
               
        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if(ValidarCampos(true))
            {
                #region <<< CALCULOS_MONTOS >>>
                //--  Calcular las montos de las columnas de Impuestos y Subtotal
                if ((e.Column == colCostoTotal && !colCostoTotal.ReadOnly) || e.Column == colQuantity || e.Column == colCostoSubTotal)
                {
                    decimal vCosto = 0, vCantidad = 1, vValorImpuesto = 0, vCostoTotal = 0, vISC = 0, vSubTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                        vCostoTotal = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal")), 2, MidpointRounding.AwayFromZero);
                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                    if (chkAplicaISC.Checked)
                    {
                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "SubTotal")) != Convert.ToDecimal("0.00"))
                            vSubTotal = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "SubTotal"));

                        var obj = from l in K where l.EsProducto && !l.CostoTotal.Equals(0) select new { l.ProductoID, l.CantidadEntrada};
                            
                            //from l in K
                            //      join p in db.Productos on l.ProductoID equals p.ID
                            //      where p.AplicaISC && !l.CostoTotal.Equals(0)
                            //      select l;

                        if (obj.ToList().Count > 0)
                        {
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto") != null)
                            {
                                if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsProducto")) && !vCostoTotal.Equals(0))
                                {
                                    vISC = Decimal.Round(((spISC.Value / obj.Sum(s => s.CantidadEntrada)) * vCantidad), 4, MidpointRounding.AwayFromZero);
                                }
                            }
                        }

                        vCostoTotal = Decimal.Round((vISC + vSubTotal), 2, MidpointRounding.AwayFromZero);

                    }

                    vCosto = Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero);

                    if (Provee.AplicaIVA)
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo") != null)
                        {
                            if (!(Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "EsManejo"))))
                                vValorImpuesto = Decimal.Round((vCostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);
                        }
                    }

                    if (_ISCCalculating)
                        return;

                    SetValue(e.RowHandle, vISC, vValorImpuesto, vCosto, vCostoTotal);

                    if (chkEsCombustible.Checked)
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            string texto = InfoCombustible(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")), vCosto);
                            gvDetalle.SetRowCellValue(e.RowHandle, "DetalleCombustible", texto);
                        }
                    }
                }

                #endregion

                #region <<< CAMBIO_IVA >>>

                if (e.Column == colTaxValue && chkIVA.Checked)
                {
                    try
                    {
                        if (!_OldTax.Equals(0))
                        {
                            if (gvDetalle.GetFocusedRowCellValue(colTaxValue) != null)
                            {
                                if (Math.Abs(Convert.ToDecimal(gvDetalle.GetFocusedRowCellValue(colTaxValue)) - _OldTax) > _MargenIVA)
                                {
                                    Parametros.General.DialogMsg("El cambio en el IVA no puede ser mayor al margen permitido: " + _MargenIVA.ToString("#,0.00") + Environment.NewLine, Parametros.MsgType.warning);
                                    gvDetalle.SetFocusedRowCellValue(colTaxValue, _OldTax);
                                }
                            }
                            else
                            {
                                
                                gvDetalle.SetFocusedRowCellValue(colTaxValue, _OldTax);                                
                            }

                            txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                        }

                        _ChangeTax = false;
                    }
                    catch
                    {
                        _OldTax = 0;
                        _ChangeTax = false;
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
                    }

                    try
                    {
                        var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));
                       
                        //-- Unidad Principal     
                        int Um = Producto.UnidadMedidaID;
                        gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Um);

                        if (Producto.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()))
                        {                           
                            
                            //Tanques por combustible
                            var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                            ColAlmacen.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                            //cboAlmacen.DataSource = TCombustible;
                            //cboAlmacen.DisplayMember = "Display";
                            //cboAlmacen.ValueMember = "ID";
                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", TCombustible.First().ID);

                            decimal VCtoEntrada, vCtoFinal;
                            Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaCompra, out vCtoFinal, out VCtoEntrada);

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                            gvDetalle.SetRowCellValue(e.RowHandle, "CostoInicial", VCtoEntrada);
                            
                                //////////

                            decimal costo = 0;
                            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                                costo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada"));

                            string texto = InfoCombustible(Producto.ID, costo);
                            gvDetalle.SetRowCellValue(e.RowHandle, "DetalleCombustible", texto);
                        }
                        else
                        {
                            cboAlmacen.DataSource = lkAlmacen.Properties.DataSource;
                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", IDAlmacen);
                            ColAlmacen.OptionsColumn.ReadOnly = UnAlmacen;

                            //////////////
                            var KCI = (from k in db.Kardexes
                                       join m in db.Movimientos on k.MovimientoID equals m.ID
                                       where k.ProductoID.Equals(Producto.ID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1))
                                         && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                                       select new { k.ID, k.Fecha, k.CostoEntrada}).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();
 

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                            if (KCI.Count().Equals(0))
                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoInicial", 0.0000m);
                            else
                                gvDetalle.SetRowCellValue(e.RowHandle, "CostoInicial", KCI.First().CostoEntrada);
                            //////////
                        }

                        gvDetalle.SetRowCellValue(e.RowHandle, "Precio", Producto.MargenToleranciaCosto);
                        gvDetalle.SetRowCellValue(e.RowHandle, "EsProducto", Producto.AplicaISC);
                        gvDetalle.SetRowCellValue(e.RowHandle, "EsManejo", Producto.ExentoIVA); 

                        //-- Cantidad Inicial de 1
                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) == Convert.ToDecimal("0.00"))
                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);
                                                                                               
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
                //--
                #endregion

                #region <<< MARGEN_COSTO >>>
                
                if (e.Column == colCost)
                {
                    decimal _MargenCosto = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                    {
                        if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                        {
                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Precio") != null)
                                _MargenCosto = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Precio"));
                            
                            decimal NewPrice = 0;
                            decimal OldPrice = 0;
                            bool _OutMargen = false;

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada") != null)
                            {
                                if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada")).Equals(0))
                                {
                                    NewPrice = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada")) :
                                        Decimal.Round((Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada")) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }
                            }

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoInicial") != null)
                            {
                                if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoInicial")).Equals(0))
                                {
                                    OldPrice = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoInicial"));
                                }
                            }

                            if (!NewPrice.Equals(0) & !OldPrice.Equals(0))
                            {
                                decimal valor = 0;

                                if (!OldPrice.Equals(0))
                                    valor = Decimal.Round(((NewPrice / OldPrice) * 100), 4, MidpointRounding.AwayFromZero);

                                if (NewPrice > OldPrice)
                                {
                                    if ((valor - 100) > _MargenCosto)
                                        _OutMargen = true;
                                }
                                else if (NewPrice < OldPrice)
                                {
                                    if ((100 - valor) > _MargenCosto)
                                        _OutMargen = true;
                                }

                                if (_OutMargen)
                                {
                                    Parametros.General.DialogMsg("El costo unitario de compra '" + NewPrice.ToString() + "' supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", 0.00m);
                                }
                            }
                        }

                    }
                }

                //if (e.Column == ColAlmacen)
                //{
                //    if (chkEsCombustible.Checked)
                //    {
                //        cboAlmacen.DataSource = Tanques;
                //    }
                //}
                    //    try
                //    {
                //        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                //        {
                //            if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                //            {
                //                int Producto = Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"));

                //                    //////////////
                //                    var KCI = (from k in db.Kardexes
                //                               where k.ProductoID.Equals(Producto)
                //                                 && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(Parametros.General.SubEstacionID) && !k.CostoEntrada.Equals(0)
                //                               select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

                //                    //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                //                    //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                //                    if (KCI.Count().Equals(0))
                //                        gvDetalle.SetFocusedRowCellValue("CostoInicial", 0.0000m);
                //                    else
                //                        gvDetalle.SetFocusedRowCellValue("CostoInicial", KCI.First().CostoEntrada);
                //                    //////////
                                
                                
                //            }

                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                //    }
                //}
                #endregion
        }
        }

        private void SetValue(int i,decimal ISC, decimal Impuesto, decimal  Costo, decimal Total)
        {
            gvDetalle.SetRowCellValue(i, "MontoISC", ISC);
            gvDetalle.SetRowCellValue(i, "ImpuestoTotal", Impuesto);
            gvDetalle.SetRowCellValue(i, "CostoEntrada", Costo);

            if (chkAplicaISC.Checked)
            {
                _ISCCalculating = true;
                gvDetalle.SetRowCellValue(i, "CostoTotal", Total);

                for (int j = 0; j < DetalleK.Count; j++)
                {
                    decimal valor = 0;
                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(j, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                        valor = Convert.ToDecimal(gvDetalle.GetRowCellValue(j, "CantidadEntrada"));

                    gvDetalle.SetRowCellValue(j, "CantidadEntrada", valor);
                }
                _ISCCalculating = false;

            }

            if (chkEsCombustible.Checked)
            {
                int prod = 0;
                if (Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "ProductoID")) != Convert.ToDecimal("0.00"))
                    prod = Convert.ToInt32(gvDetalle.GetRowCellValue(i, "ProductoID"));

                string texto = InfoCombustible(prod, Costo);
                gvDetalle.SetRowCellValue(i, "DetalleCombustible", texto);
            }

            txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                 
        }

        //Linea Informtavia margen del producto
        private String InfoCombustible(int IDProd, decimal costo )
        {
            try
            {                
                decimal precio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(IDProd, IDEstacionServicio, IDSubEstacion, false, 0, false, FechaFisico.Date)), 2, MidpointRounding.AwayFromZero);
                decimal resta = precio - costo;
                decimal galones = resta * 3.7854m;

                return "Precio Pizarra: " + precio.ToString("#,0.00") + " | Margen de Comercialización: " + galones.ToString("#,0.00");

            }
            catch { return "N/A"; }
        }

        private void gvDetalle_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (!ValidarCampos(true))
            {
                return;
            }

            try
            {
                if (e.Column == ColAlmacen)
                {
                    if (chkEsCombustible.Checked)
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                            {
                                var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                                ColAlmacen.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);
                                cboAlmacen.DataSource = TCombustible;
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

        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {
            if (!OnlyView)
            {
                if (!chkAplicaISC.Checked)
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

                                txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal) + K.Sum(s => s.MontoISC)).ToString("#,0.00");
                                //Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["SubTotal"].SummaryText) + Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
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

                        txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal) + K.Sum(s => s.MontoISC)).ToString("#,0.00");
                                               
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

                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable de Compras";
                        nf.ShowDialog();
                    }

                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        //Validar fecha de ingreso y actualizar el tipo de cambio
        private void dateFechaIngreso_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaIngreso.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaFisico.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaFisico.Date)).First().Valor : 0m);
        }

        private void dateFechaVencimiento_EditValueChanged(object sender, EventArgs e)
        {
            txtNombre_Validated_1(sender, null);
        }         

        private void lkMoneda_EditValueChanged(object sender, EventArgs e)
        {
            if (lkMoneda.EditValue != null)
            {
                lblGrandTotal.Text = (_EsContado ? "TOTAL FACTURA: " : "TOTAL: ") + db.Monedas.Single(m => m.ID.Equals(Convert.ToInt32(lkMoneda.EditValue))).Simbolo;

                if (Convert.ToInt32(lkMoneda.EditValue).Equals(Parametros.Config.MonedaPrincipal()))
                {
                    lkMoneda.BackColor = Color.White;
                    lkMoneda.ForeColor = Color.Black;

                    txtGrandTotal.BackColor = Color.FromArgb(232, 249, 239);
                    txtGrandTotal.ForeColor = Color.Black;
                }
                else if (Convert.ToInt32(lkMoneda.EditValue).Equals(Parametros.Config.MonedaSecundaria()))
                {
                    lkMoneda.BackColor = Color.ForestGreen;
                    lkMoneda.ForeColor = Color.White;

                    txtGrandTotal.BackColor = Color.ForestGreen;
                    txtGrandTotal.ForeColor = Color.White;
                }

            }
        }
        
        //Validación al cambio de columna
        private void gvDetalle_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            try
            {                
                if (e.FocusedColumn == colUnit)
                {
                    gvDetalle.FocusedColumn = gvDetalle.VisibleColumns[2];
                    gvDetalle.ShowEditor();
                }

                if (e.FocusedColumn == ColAlmacen)
                {
                    if (ColAlmacen.ReadOnly)
                    {
                        gvDetalle.FocusedColumn = gvDetalle.VisibleColumns[3];
                        gvDetalle.ShowEditor();
                    }
                }

                if (e.FocusedColumn == colCostoInical)
                {
                    gvDetalle.FocusedColumn = colCostoTotal;
                    gvDetalle.ShowEditor();
                }

                if (e.FocusedColumn == colCost)
                {
                    gvDetalle.FocusedColumn = colCostoTotal;
                    gvDetalle.ShowEditor();
                }

                if (e.FocusedColumn == colTaxValue && !chkIVA.Checked)
                {
                    gvDetalle.FocusedColumn = gvDetalle.VisibleColumns[Convert.ToInt32(gvDetalle.VisibleColumns.Count - 1)];
                    gvDetalle.ShowEditor();
                }

                if (e.FocusedColumn == colTotal)
                {
                    gvDetalle.FocusedColumn = gvDetalle.VisibleColumns[Convert.ToInt32(gvDetalle.VisibleColumns.Count - 1)];
                    gvDetalle.ShowEditor();
                }

            }
            catch
            {
                gvDetalle.FocusedColumn = gvDetalle.VisibleColumns[0];
                gvDetalle.ShowEditor();
            }
        }

        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (chkEsCombustible.Checked)
                cboAlmacen.DataSource = Tanques;
        }

        private void gvDetalle_LostFocus(object sender, EventArgs e)
        {
            if (chkEsCombustible.Checked)
                cboAlmacen.DataSource = Tanques;
        }

        private void txtRuc_Enter(object sender, EventArgs e)
        {
            txtReferencia.Focus();
            txtReferencia.Select();
        }

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

        private void bntNew_Click(object sender, EventArgs e)
        {
            if (DetalleK.Count > 0 || dateFechaCompra.EditValue != null || dateFechaVencimiento.EditValue != null || lkMoneda.EditValue != null || txtReferencia.Text != "")
            {
                if (Parametros.General.DialogMsg("La compra actual tiene datos registrados. ¿Desea cancelar esta compra y realizar una nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    NextCompra = true;
                    RefreshMDI = false;
                    ShowMsg = false;
                    this.Close();
                }
            }                
        }

        //Validar chequeo para ISC
        private void chkAplicaISC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkAplicaISC.Checked)
                {
                    layoutControlItemISC.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;  
                    colCostoSubTotal.Visible = true;
                    colCostoSubTotal.VisibleIndex = 5;
                    colISC.Visible = true;
                    colISC.VisibleIndex = 6;

                    if (!OnlyView)
                    {
                        colCostoTotal.OptionsColumn.ReadOnly = true;
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        for (int i = 0; i < DetalleK.Count; i++)
                        {
                            decimal valor = 0;
                            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "CostoTotal")) != Convert.ToDecimal("0.00"))
                                valor = Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "CostoTotal"));

                            gvDetalle.SetRowCellValue(i, "SubTotal", valor);
                        }

                        if (spISC.Value > 0)
                            this.spISC.BackColor = Color.White;
                        else if (spISC.Value <= 0)
                            this.spISC.BackColor = Color.Red;

                        this.gvDetalle.OptionsBehavior.Editable = false;
                        this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                        this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                        this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                        this.gvDetalle.RefreshData();
                        txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                    }
                }
                else if (!chkAplicaISC.Checked)
                {
                    layoutControlItemISC.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    colISC.Visible = false;
                    colCostoSubTotal.Visible = false;
                    colCostoTotal.OptionsColumn.ReadOnly = false;
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    for (int i = 0; i < DetalleK.Count; i++)
                    {
                        decimal valor = 0;
                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "SubTotal")) != Convert.ToDecimal("0.00"))
                            valor = Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "SubTotal"));

                        gvDetalle.SetRowCellValue(i, "CostoTotal", valor);
                    }
                    this.gvDetalle.OptionsBehavior.Editable = true;
                    this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
                    this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
                    this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
                    this.gvDetalle.RefreshData();
                    txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                }

                    //CalculoISC();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        private void spISC_EditValueChanged(object sender, EventArgs e)
        {
            if (!OnlyView)
            {
                if (!spISC.Value.Equals(0))
                {
                    this.spISC.BackColor = Color.White;
                    for (int i = 0; i < DetalleK.Count; i++)
                    {
                        decimal valor = 0;
                        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                            valor = Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "CantidadEntrada"));

                        gvDetalle.SetRowCellValue(i, "CantidadEntrada", valor);
                    }
                    this.gvDetalle.RefreshData();
                    
                }
                else if (spISC.Value <= 0)
                    this.spISC.BackColor = Color.Red;
                //    CalculoISC();

                txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
            }          
        }

        //Tipo de Factura (Credito / Contado / Orde de Compra)
        private void rgCreditoContado_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                if (rgCreditoContado.SelectedIndex.Equals(0))
                {
                    _EsContado = false;
                    
                    _IDSubSerie = 0;
                    txtEntregado.Text = "";
                    layoutControlItemDir.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemEntregado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    if (Orden == null)
                    this.layoutControlItemEsCombustible.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    
                    this.layoutControlGroupDatos.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    
                    if (!OnlyView)
                    {
                        lkMoneda.Enabled = true;
                        _CajaChicaID = 0;
                        _CuentaContableCajaChicaID = 0;
                    }

                    layoutControlItemCajaChica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemCaja.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
                else if (rgCreditoContado.SelectedIndex.Equals(1))
                {
                    _EsContado = true;
                    lkMoneda.ItemIndex = 0;
                    lkMoneda.Enabled = false;
                    layoutControlItemDir.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemEntregado.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    this.layoutControlItemEsCombustible.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    this.layoutControlGroupDatos.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    if (!OnlyView)
                    {
                        int Pchica = 0;

                        if (IDSubEstacion > 0 && db.SubEstacions.Count(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0) > 0)
                        {
                            Pchica = db.SubEstacions.First(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0).ProveedorCajaChicaID;
                            _IDSubSerie = db.SubEstacions.First(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0).ID;
                        }
                        else
                            Pchica = db.EstacionServicios.First(s => s.ID.Equals(IDEstacionServicio) && s.ProveedorCajaChicaID > 0).ProveedorCajaChicaID;


                        _CajaChicaID = Pchica;  //db.EstacionServicios.Single(es => es.ID.Equals(IDEstacionServicio)).ProveedorCajaChicaID;
                        var CCH = db.Proveedors.Where(p => p.ID.Equals(_CajaChicaID)).ToList();
                        txtCajaChica.Text = (!CCH.Count.Equals(0) ? CCH.First().Nombre : "N/A");
                        _CuentaContableCajaChicaID = CCH.First().CuentaContableID;
                        spNumero.Value = Parametros.General.GetNroSerie(IDEstacionServicio, _IDSubSerie, 41, db);
                    }
                     
                    layoutControlItemCajaChica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemCaja.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                }
                else if (rgCreditoContado.SelectedIndex.Equals(2))
                {
                    using (Inventario.Forms.Dialogs.DialogSeleccionarOrden dg = new Inventario.Forms.Dialogs.DialogSeleccionarOrden())
                    {
                        dg.Text = "Ordenes de compra Aprobadas";
                        
                        if (dg.ShowDialog().Equals(DialogResult.OK))
                        {
                            Orden = db.OrdenCompras.Single(o => o.ID.Equals(dg.IDOrden));
                            IDProveedor = Orden.ProveedorID;

                            K.Clear();

                            Orden.OrdenCompraDetalles.ToList().ForEach(l =>
                                {
                                    K.Add(new Entidad.Kardex { ProductoID = l.ProductoID, UnidadMedidaID = l.UnidadMedidaID, AlmacenEntradaID = l.AlmacenEntradaID, CantidadEntrada = l.CantidadEntrada, CostoInicial = l.UltimoCosto, CostoEntrada = l.Costo, CostoTotal = l.CostoTotal, ImpuestoTotal = l.ImpuestoTotal });
                                });

                            gridProductos.DataSource = from P in db.Productos
                                                       join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                       join A in db.Areas on C.AreaID equals A.ID
                                                       where P.Activo.Equals(true) && !P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && !C.AreaID.Equals(ProductoAreaServicioID) && A.Activo && C.Activo
                                                       select new
                                                       {
                                                           P.ID,
                                                           P.Codigo,
                                                           P.Nombre,
                                                           UmidadName = U.Nombre,
                                                           Display = P.Codigo + " | " + P.Nombre
                                                       };

                            this.chkAplicaISC.Properties.ReadOnly = true;
                            lkMoneda.Enabled = false;
                            this.layoutControlItemEsCombustible.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            this.emptySpaceItemOrden.TextVisible = true;
                            this.emptySpaceItemOrden.Text = "Orden de Compra : " + Orden.Numero;

                            glkProvee.Properties.ReadOnly = true;
                            this.gvDetalle.OptionsBehavior.Editable = false;
                            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                            this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                            this.gvDetalle.RefreshData();

                            txtGrandTotal.Text = Convert.ToDecimal(K.Sum(s => s.CostoTotal) + K.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                        }
                    }

                    rgCreditoContado.SelectedIndex = 0;


                }

                lkMoneda_EditValueChanged(null, null);
            }        
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }       

        private void dateFechaIngreso_EditValueChanged(object sender, EventArgs e)
        {
            if (Provee != null)
                FechaVencimiento = FechaFisico.AddDays(Provee.Plazo);
        }

        //Cambiar la fecha de contabilizacion
        private void dateFechaCompra_EditValueChanged(object sender, EventArgs e)
        {
            if (dateFechaCompra.EditValue != null)
            {
                if (Parametros.General.ValidateKardexMovemente(FechaCompra, db, IDEstacionServicio, IDSubEstacion, (chkEsCombustible.Checked ? 24 : 9), 0))
                {
                    if (dateFechaCompra.EditValue != null)
                    {
                        FechaFisico = FechaCompra;
                        dateFechaIngreso_Validated(dateFechaIngreso, null);
                    }
                }
                else
                {
                    DateTime fecha = Convert.ToDateTime(dateFechaCompra.EditValue);
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString(), Parametros.MsgType.warning);
                    dateFechaCompra.EditValue = null;
                }
            }
        }
        
        private void lkAlmacen_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkAlmacen.EditValue != null)
                {
                    for (int i = 0; i < DetalleK.Count; i++)
                    {
                        gvDetalle.SetRowCellValue(i, "AlmacenEntradaID", IDAlmacen);
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Valida si la compra es Combustible
        private void chkEsCombustible_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEsCombustible.Checked)
            {
                colDetalle.Visible = true;
                Tanques = db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                if (!OnlyView)
                {
                    gridProductos.DataSource = null;
                    gridProductos.DataSource = from P in db.Productos
                                               join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                               join T in db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).GroupBy(g => g.ProductoID)
                                               on P.ID equals T.Key
                                               where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                select new
                                               {
                                                   P.ID,
                                                   P.Codigo,
                                                   P.Nombre,
                                                   UmidadName = U.Nombre,
                                                   Display = P.Codigo + " | " + P.Nombre
                                               };
                    IDProveedor = Parametros.Config.ProveedorCombustibleID();
                    //glkProvee.Properties.ReadOnly = true;
                    rgCreditoContado.Properties.ReadOnly = true;
                    lkAlmacen.Properties.ReadOnly = true;
                
                }                
            }
            else
            {
                colDetalle.Visible = false;

                if (!OnlyView)
                {
                    gridProductos.DataSource = null;
                    gridProductos.DataSource = from P in db.Productos
                                               join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                               join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                               join A in db.Areas on C.AreaID equals A.ID
                                               where P.Activo.Equals(true) && !P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && !C.AreaID.Equals(ProductoAreaServicioID) && A.Activo && C.Activo && !A.AplicaOrdenCompra
                                               select new
                                               {
                                                   P.ID,
                                                   P.Codigo,
                                                   P.Nombre,
                                                   UmidadName = U.Nombre,
                                                   Display = P.Codigo + " | " + P.Nombre
                                               };
                }

                glkProvee.Properties.ReadOnly = false;
                lkAlmacen.Properties.ReadOnly = false;
                rgCreditoContado.Properties.ReadOnly = false;
            }
        }

        private void chkEsCombustible_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0 && !OnlyView)
            {
                Parametros.General.DialogMsg("La compra tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void lkAlmacen_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0)
            {
                Parametros.General.DialogMsg("La compra tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }
        
        //Validando el cambio de valor en ISC
        private void chkAplicaISC_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count <= 0)
            {
                Parametros.General.DialogMsg("La compra no tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void chkIVA_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count <= 0)
            {
                Parametros.General.DialogMsg("La compra no tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }

            if (Provee != null)
            {
                if (!Provee.AplicaIVA && Convert.ToBoolean(e.NewValue).Equals(true))
                {
                    Parametros.General.DialogMsg("El proveedor no aplica IVA.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }

            }
        }

        private void glkProvee_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0)
            {
                Parametros.General.DialogMsg("La compra tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }       

        //Calculando Valor Total
        private void txtGrandTotal_EditValueChanged(object sender, EventArgs e)
        {
            _costoTotal = 0;

            if (_EsContado && Provee != null)
            {
                decimal subtotal = Convert.ToDecimal(txtGrandTotal.Text);
                _costoTotal = (chkAplicaISC.Checked ? DetalleK.Sum(s => s.SubTotal) : DetalleK.Sum(s => s.CostoTotal));
                decimal resta = 0;

                if (Provee.AplicaRetencion)
                {
                    if (_costoTotal >= db.CuentaContables.Single(s => s.ID.Equals(Provee.ImpuestoRetencionID)).Limite)
                    {
                        decimal IR = (db.CuentaContables.Single(cc => cc.ID.Equals(Provee.ImpuestoRetencionID)).Porcentaje / 100m);
                        spIR.Value = Decimal.Round((_costoTotal * IR), 2, MidpointRounding.AwayFromZero);
                        resta += Decimal.Round((_costoTotal * IR), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                        spIR.Value = 0m;
                }
                else
                    spIR.Value = 0m;

                if (_AplicaAlcaldia)
                {
                    if (_costoTotal >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || Provee.TipoProveedorID.Equals(ProveedorNoRegistrado))
                    {
                        decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(231)).Porcentaje / 100m);
                        spAlcaldia.Value = Decimal.Round((_costoTotal * Alcaldia), 2, MidpointRounding.AwayFromZero);
                        resta += Decimal.Round((_costoTotal * Alcaldia), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                        spAlcaldia.Value = 0m;
                }
                else
                    spAlcaldia.Value = 0m;

                spTotal.Value = (subtotal - resta);
            }
        }
        
        //Porceso de cambio en la Aplicacion del IVA
        private void chkIVA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIVA.Checked)
            {
                colTaxValue.OptionsColumn.ReadOnly = false;

                if (chkAplicaISC.Checked || Orden != null)
                {
                    this.gvDetalle.OptionsBehavior.Editable = true;
                    colQuantity.OptionsColumn.ReadOnly = true;
                    colQuantity.OptionsColumn.AllowFocus = false;
                    colCostoTotal.OptionsColumn.ReadOnly = true;
                    colCostoTotal.OptionsColumn.AllowFocus = false;
                    colProduct.OptionsColumn.ReadOnly = true;
                    colProduct.OptionsColumn.AllowFocus = false;
                    gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
                    gridDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
                    gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                    gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
                }
            }
            else if (!chkIVA.Checked)
            {
                colTaxValue.OptionsColumn.ReadOnly = true;
            }
        }

        private void spTax_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!_ChangeTax)
                {
                    _OldTax = Convert.ToDecimal(e.OldValue);
                    _ChangeTax = true;
                }
            }
            catch
            {
                _OldTax = 0;
            }
        }
        
        private void spTC_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!_ChangeTC)
                {
                    _OldTC= Convert.ToDecimal(e.OldValue);
                    _ChangeTC = true;
                }
            }
            catch
            {
                _OldTC = 0;
            }
            
        }
        
        private void spTC_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_OldTC.Equals(0))
                {
                    if (Math.Abs(Convert.ToDecimal(spTC.Value) - Convert.ToDecimal(_OldTC)) > _MargenTC)
                    {
                        Parametros.General.DialogMsg("El cambio en la T/C no puede ser mayor al margen permitido: " + _MargenTC.ToString("#,0.00") + Environment.NewLine, Parametros.MsgType.warning);
                        spTC.Value = _OldTC;
                    }

                }

                _ChangeTC = false;
            }
            catch
            {
                _OldTC = 0;
                _ChangeTC = false;
            }

        }

        private void spAlcaldia_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (!_ChangeAlma)
                {
                    _OldAlma = Convert.ToDecimal(e.OldValue);
                    _ChangeAlma = true;
                }
            }
            catch
            {
                _OldAlma = 0;
            }
        }
        
        private void spAlcaldia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_OldAlma.Equals(0) && spAlcaldia.IsEditorActive)
                {
                    if (Math.Abs(Convert.ToDecimal(spAlcaldia.Value) - Convert.ToDecimal(_OldAlma)) > _MargenValorCambioAlcaldia)
                    {
                        Parametros.General.DialogMsg("El cambio en la Retención de la Alcaldia no puede ser mayor al margen permitido: " + _MargenValorCambioAlcaldia.ToString("#,0.00") + Environment.NewLine, Parametros.MsgType.warning);
                        spAlcaldia.Value = _OldAlma;
                    }

                }
                
                _ChangeAlma = false;
                spTotal.Value = Convert.ToDecimal(txtGrandTotal.Text) - spIR.Value - spAlcaldia.Value;
            }
            catch
            {
                _OldAlma = 0;
                _ChangeAlma = false;
            }
        }

        //Validar Lista 
        private void dateFechaCompra_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleK.Count > 0)
            {
                Parametros.General.DialogMsg("No se puede cambiar la fecha porque la compra tiene detalles registrado.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }
        //Cambiar detalle en el combustible
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (chkEsCombustible.Checked)
            {
                for (int i = 0; i < gvDetalle.RowCount; i++)
                {
                if (gvDetalle.GetRowCellValue(i, "ProductoID") != null)
                    {
                        string texto = InfoCombustible(Convert.ToInt32(gvDetalle.GetRowCellValue(i, "ProductoID")), Convert.ToDecimal(gvDetalle.GetRowCellValue(i, "CostoEntrada")));
                            gvDetalle.SetRowCellValue(i, "DetalleCombustible", texto);                        
                    }
                }
            }        
        }
        
        private void gvDetalle_CustomSummaryExists(object sender, DevExpress.Data.CustomSummaryExistEventArgs e)
        {
           
        }

        private void gvDetalle_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {

            decimal suma = 0;
            if (DetalleK.Count > 0)
                suma = Decimal.Round(DetalleK.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

            if (chkAplicaISC.Checked)
            {
                decimal ISC = 0, Sub = 0;
                if (DetalleK.Count > 0)
                {
                    ISC = Decimal.Round(DetalleK.Sum(s => s.MontoISC), 2, MidpointRounding.AwayFromZero);
                    Sub = Decimal.Round(DetalleK.Sum(s => s.SubTotal), 2, MidpointRounding.AwayFromZero);

                    if (!suma.Equals(ISC + Sub))
                    {

                        decimal dif = (ISC + Sub) - suma;

                        DetalleK.First().CostoTotal += dif;

                    }
                }
            }

            e.TotalValue = suma;

            //if (DetalleK.Count > 0)
            //{
            //    decimal iva = DetalleK.Sum(s => s.ImpuestoTotal);
            //    MessageBox.Show("total = " + Convert.ToString(suma + iva));
            //}
            //if (e.Item == gvDetalle.TotalSummary["CostoTotal"])
            //{
            //    MessageBox.Show("si...");
            //}

        }

        #endregion


    }
}