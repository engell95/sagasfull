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
    public partial class DialogOrdenCompras : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormOrdenCompras MDI;
        internal Entidad.OrdenCompra EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool UnAlmacen = true;
        private bool NextOrden = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _ChangeTax = false; 
        private bool _Guardado = false;
        //private decimal _MargenCosto = Parametros.Config.MargenToleranciaCosto();
        private decimal _MargenIVA= Parametros.Config.MargenValorCambioIVA();
        private decimal _OldTax = 0;
        private decimal _ValorIVA = 0;
       
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

        private DateTime FechaOrden
        {
            get { return Convert.ToDateTime(dateFechaOrden.EditValue); }
            set { dateFechaOrden.EditValue = value; }
        }

        private int IDMonedaPrincipal
        {
            get { return Convert.ToInt32(lkMoneda.EditValue); }
            set { lkMoneda.EditValue = value; }
        }

        private static Entidad.Proveedor Provee;

        private List<Entidad.OrdenCompraDetalle> OC = new List<Entidad.OrdenCompraDetalle>();
        public List<Entidad.OrdenCompraDetalle> DetalleOC
        {
            get { return OC; }
            set
            {
                OC = value;
                this.bdsDetalle.DataSource = this.OC;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogOrdenCompras(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
                layoutControlItemAlmacen.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                ColAlmacen.OptionsColumn.ReadOnly = false;
                //glkProvee.Properties.ReadOnly = true;
                //lkMoneda.Properties.ReadOnly = true;
                //txtReferencia.Properties.ReadOnly = true;
                //lkAlmacen.Properties.ReadOnly = true;
                //dateFechaOrden.Properties.ReadOnly = true;
                //gvDetalle.OptionsBehavior.Editable = false;
                //gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                //gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
                //btnOK.Enabled = false;
                //btnOK.Visible = false;
                //spTC.Properties.ReadOnly = true;
                //chkIVA.Properties.ReadOnly = true;
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
                
                this._ValorIVA = 0.15m;
                
                //Proveedor
                glkProvee.Properties.DataSource = (from p in db.Proveedors where p.Activo select new { p.ID, p.Codigo, p.Nombre, Display = p.Codigo + " | " + p.Nombre }).OrderBy(o => o.Codigo);
                glkProvee.Properties.DisplayMember = "Display";
                glkProvee.Properties.ValueMember = "ID";

                //Tipo de Moneda
                lkMoneda.Properties.DataSource = from m in db.Monedas select new { m.ID, Display = m.Simbolo + " | " + m.Nombre};
                lkMoneda.Properties.DisplayMember = "Display";
                lkMoneda.Properties.ValueMember = "ID";

                //--- Fill Combos Detalles --//
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                gridProductos.DataSource = null;
                gridProductos.DataSource = (from P in db.Productos
                                            join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                            join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                            join A in db.Areas on C.AreaID equals A.ID
                                            where P.Activo && A.Activo && C.Activo
                                            select new
                                           {
                                               P.ID,
                                               P.Codigo,
                                               P.Nombre,
                                               UmidadName = U.Nombre,
                                               Display = P.Codigo + " | " + P.Nombre
                                           }).OrderBy(o => o.Codigo);

                gridProductos.DisplayMember = "Display";
                gridProductos.ValueMember = "ID";

                //---LLenar Almacenes ---//
                IQueryable<Parametros.ListIdDisplay> listaAlmacen = from al in db.Almacens
                                                                    where al.Activo && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
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


                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//


                if (Editable)
                {
                    IDProveedor = EntidadAnterior.ProveedorID;
                    Referencia = EntidadAnterior.Numero;
                    FechaOrden = Convert.ToDateTime(EntidadAnterior.Fecha);
                    IDMonedaPrincipal = EntidadAnterior.MonedaID;
                    _TipoCambio = EntidadAnterior.TipoCambio;
                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;

                    OC = EntidadAnterior.OrdenCompraDetalles.ToList();

                    if (!EntidadAnterior.MonedaID.Equals(Parametros.Config.MonedaPrincipal()))
                    {
                        DetalleOC.ForEach(lista =>
                            {
                                lista.Costo = Decimal.Round((lista.Costo / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                lista.CostoTotal = Decimal.Round((lista.CostoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                lista.ImpuestoTotal = Decimal.Round((lista.ImpuestoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                            });
                    }

                    this.bdsDetalle.DataSource = this.DetalleOC;
                    gvDetalle.RefreshData();

                    txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

                }
                else
                {

                    dateFechaOrden.EditValue = Convert.ToDateTime(db.GetDateServer());
                    IDMonedaPrincipal = Parametros.Config.MonedaPrincipal();

                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaOrden.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaOrden.Date)).First().Valor : 0m);

                    this.bdsDetalle.DataSource = this.DetalleOC;
                }


                
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
            Parametros.General.ValidateEmptyStringRule(lkMoneda, errRequiredField);
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
            if (lkMoneda.EditValue == null || txtReferencia.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkProvee.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Proveedor.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaOrden, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
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
                if (DetalleOC.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a la compra." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!ValidarReferencia(Convert.ToString(txtReferencia.Text)))
                {
                    Parametros.General.DialogMsg("La referencia para esta compra ya existe : " + Convert.ToString(txtReferencia.Text), Parametros.MsgType.warning);
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
                Parametros.General.DialogMsg("El Total da la orden no puede estar en 0 'CERO' ", Parametros.MsgType.warning);
                    return false;
            }

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.OrdenCompra NewOC;

                    if (Editable)
                    { 
                        NewOC = db.OrdenCompras.Single(e => e.ID == EntidadAnterior.ID); 
                        NewOC.OrdenCompraDetalles.Clear();
                    }
                    else 
                    {
                       NewOC = new Entidad.OrdenCompra();
                    }
                                        
                    NewOC.ProveedorID = IDProveedor;
                    NewOC.Numero = Referencia;
                    NewOC.Fecha = FechaOrden;
                    NewOC.UsuarioID = UsuarioID;
                    NewOC.TipoCambio = _TipoCambio;
                    NewOC.MonedaID = IDMonedaPrincipal;
                    NewOC.Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(txtGrandTotal.Text) :
                                Decimal.Round((Convert.ToDecimal(txtGrandTotal.Text) * NewOC.TipoCambio), 2, MidpointRounding.AwayFromZero));
                    NewOC.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round((NewOC.Monto / NewOC.TipoCambio), 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(txtGrandTotal.Text));
                    NewOC.EstacionServicioID = IDEstacionServicio;
                    NewOC.SubEstacionID = IDSubEstacion;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(NewOC, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó la Orden de Compra: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.OrdenCompras.InsertOnSubmit(NewOC);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la Orden de Compra: " + NewOC.Numero, this.Name);

                    }
                    
                    db.SubmitChanges();

                    #region ::: REGISTRANDO EXISTENCIAS EN DETALLE :::

                    DetalleOC.ForEach(linea =>
                       {
                           NewOC.OrdenCompraDetalles.Add(linea);
                           db.SubmitChanges();
                       });

                       
                        //------------------------------------------------------------------------//

                        #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    NextOrden = true;
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
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
            MDI.CleanDialog(ShowMsg, NextOrden, RefreshMDI);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !NextOrden)
            {
                if (DetalleOC.Count > 0 || txtReferencia.Text != "" || Provee != null)
                {
                    if (Parametros.General.DialogMsg("La orden de compra actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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
                    txtRuc.Text = Provee.RUC;
                    txtTelefonos.Text = Provee.Telefono1 + " | " + Provee.Telefono2 + " | " + Provee.Telefono3;
                    memoDir.Text = Provee.Direccion;

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

            decimal NewPrice = 0;
            decimal OldPrice = 0;
            bool _OutMargen = false;

            if (view.GetRowCellValue(RowHandle, "Costo") != null)
            {
                if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")).Equals(0))
                {
                    NewPrice = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")) :
                        Decimal.Round((Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
                }
            }

            if (view.GetRowCellValue(RowHandle, "UltimoCosto") != null)
            {
                if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "UltimoCosto")).Equals(0))
                {
                    OldPrice = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "UltimoCosto"));
                }
            }

            decimal valor = 0;
            decimal _MargenCosto = 0;
            if (!OldPrice.Equals(0))
                valor = Decimal.Round(((NewPrice / OldPrice) * 100), 4, MidpointRounding.AwayFromZero);

            _MargenCosto = db.Productos.SingleOrDefault(o => o.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).MargenToleranciaCosto;
               

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
                view.SetColumnError(view.Columns["Costo"], "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%");
                e.ErrorText = "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%";
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
            if (ValidarCampos(true))
            {

                #region <<< CALCULOS_MONTOS >>>
                //--  Calcular las montos de las columnas de Impuestos y Subtotal
                if ((e.Column == colCostoTotal) || e.Column == colQuantity)
                {
                    decimal vCosto = 0, vCantidad = 1, vValorImpuesto = 0, vCostoTotal = 0, vISC = 0, vSubTotal = 0;

                    if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                        vCostoTotal = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal"));
                    if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
                        vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
                    else
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                    vCosto = Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero);

                    vValorImpuesto = Decimal.Round((vCostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);

                    gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Costo", vCosto);

                    txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
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

                            txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
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
                        var Um = Producto.UnidadMedidaID;
                        gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Um);

                        cboAlmacen.DataSource = lkAlmacen.Properties.DataSource;
                        if (!Editable)
                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", IDAlmacen);

                        ColAlmacen.OptionsColumn.ReadOnly = UnAlmacen;

                        //////////////
                        var KCI = (from k in db.Kardexes
                                   join m in db.Movimientos on k.MovimientoID equals m.ID
                                   where k.ProductoID.Equals(Producto.ID) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
                                     && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                                   select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();


                        //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                        //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                        if (KCI.Count().Equals(0))
                            gvDetalle.SetRowCellValue(e.RowHandle, "UltimoCosto", 0.0000m);
                        else
                            gvDetalle.SetRowCellValue(e.RowHandle, "UltimoCosto", KCI.First().CostoEntrada);
                        //////////


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

            }
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

                        txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
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

                        txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                                               
                    }
                }
            }
        }  
              
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

        private void lkMoneda_EditValueChanged(object sender, EventArgs e)
        {
            if (lkMoneda.EditValue != null)
            {
                //lblGrandTotal.Text = "TOTAL: " + db.Monedas.Single(m => m.ID.Equals(Convert.ToInt32(lkMoneda.EditValue))).Simbolo;

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

            if (keyData == (Keys.Control | Keys.N))
            {
                bntNew_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        private void bntNew_Click(object sender, EventArgs e)
        {
            if (DetalleOC.Count > 0 || dateFechaOrden.EditValue != null || lkMoneda.EditValue != null || txtReferencia.Text != "")
            {
                if (Parametros.General.DialogMsg("La orden de compra actual tiene datos registrados. ¿Desea cancelar esta orden y realizar una nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    NextOrden = true;
                    RefreshMDI = false;
                    ShowMsg = false;
                    this.Close();
                }
            }                
        }
               
        private void lkAlmacen_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleOC.Count > 0)
            {
                Parametros.General.DialogMsg("La compra tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }
        
        private void chkIVA_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleOC.Count <= 0)
            {
                Parametros.General.DialogMsg("La compra no tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        
        //Porceso de cambio en la Aplicacion del IVA
        private void chkIVA_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIVA.Checked)
            {
                colTaxValue.OptionsColumn.ReadOnly = false;
                colTaxValue.OptionsColumn.AllowFocus = true;
                
            }
            else if (!chkIVA.Checked)
            {
                colTaxValue.OptionsColumn.ReadOnly = true;
                colTaxValue.OptionsColumn.AllowFocus = false;
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

        //Sumando los totales al cambiar de filas
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
        }
        
        #endregion 

    }
}