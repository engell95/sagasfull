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
    public partial class DialogEntradaSalidaInventario : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEntradaSalidaInventario MDI;
        //internal Entidad.OrdenCompra EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool _Next = false;
        private Parametros.TiposMovimientoInventario _Tipo;
        private IQueryable<Parametros.ListIdDisplay> listaAlmacen;
        private int IDPrint = 0;
        private bool _PermitirCombustibleEntradaInv;
        private int IDInvntarioInicial = Parametros.Config.ConceptoInventarioInicial();
        private int ProductoAreaServicioID = Parametros.Config.ProductoAreaServicioID();
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        
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
        
        private int IDAlmacen
        {
            get { return Convert.ToInt32(lkAlmacen.EditValue); }
            set { lkAlmacen.EditValue = value; }
        }

        private int IDConepto
        {
            get { return Convert.ToInt32(lkConcepto.EditValue); }
            set { lkConcepto.EditValue = value; }
        }

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private DateTime FechaOrden
        {
            get { return Convert.ToDateTime(dateFechaOrden.EditValue); }
            set { dateFechaOrden.EditValue = value; }
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

        #endregion

        #region *** INICIO ***

        public DialogEntradaSalidaInventario(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;



            if (Editable)
            { //-- Bloquear Controles --//    
                layoutControlItemAlmacen.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                ColAlmacenEntrada.OptionsColumn.ReadOnly = false;
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
                _PermitirCombustibleEntradaInv = Parametros.Config.PermitirCombustibleEntradaInv();
                

                var result = (from acs in db.AccesoSistemas
                              join acc in db.Accesos
                              on acs.AccesoID equals acc.ID
                              where acs.UsuarioID.Equals(UsuarioID)
                              && acc.Componente.Equals("rgOptions")
                              group acc by acc.Nombre into gr
                              select new {gr.Key}).ToList();

                result.ForEach(acces =>
                    {
                        if (acces.Key.Contains("Entrada"))
                            rgOptions.Properties.Items[0].Enabled = true;
                        else if (acces.Key.Contains("Salida"))
                            rgOptions.Properties.Items[1].Enabled = true;
                        else if (acces.Key.Contains("Traslado"))
                            rgOptions.Properties.Items[2].Enabled = true;
                    });


                //--- Fill Combos Detalles --//
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
                listaTanque = db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });               

                txtNumero.Text = "000000000";

                //---LLenar Almacenes ---//
                listaAlmacen = from al in db.Almacens
                                where al.Activo && al.EstacionServicioID.Equals(IDEstacionServicio) && al.SubEstacionID.Equals(IDSubEstacion)
                                select new Parametros.ListIdDisplay { ID = al.ID, Display = al.Nombre };

                //Almacen GRID
                cboAlmacenEntrada.DataSource = listaAlmacen.ToList();
                cboAlmacenEntrada.DisplayMember = "Display";
                cboAlmacenEntrada.ValueMember = "ID";
                //Almacen FORM
                lkAlmacen.Properties.DataSource = listaAlmacen.ToList();
                lkAlmacen.Properties.DisplayMember = "Display";
                lkAlmacen.Properties.ValueMember = "ID";

                lkArea.Properties.DataSource = db.Areas.Where(ar => ar.Activo && (!ar.ID.Equals(1) && !ar.ID.Equals(6)));
                lkConcepto.Properties.DataSource = db.ConceptoContables.Where(cc => cc.Activo && cc.EsMovimientoInventario).Select(s => new { s.ID, Display = s.Nombre}).ToList();

                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//


                if (Editable)
                {
                    //Numero = EntidadAnterior.Numero;
                    //FechaOrden = Convert.ToDateTime(EntidadAnterior.Fecha);
                    //_TipoCambio = EntidadAnterior.TipoCambio;
                    //IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    //IDSubEstacion = EntidadAnterior.SubEstacionID;

                    //OC = EntidadAnterior.OrdenCompraDetalles.ToList();

                    //if (!EntidadAnterior.MonedaID.Equals(Parametros.Config.MonedaPrincipal()))
                    //{
                    //    DetalleOC.ForEach(lista =>
                    //        {
                    //            lista.Costo = Decimal.Round((lista.Costo / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                    //            lista.CostoTotal = Decimal.Round((lista.CostoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                    //            lista.ImpuestoTotal = Decimal.Round((lista.ImpuestoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                    //        });
                    //}

                    //this.bdsDetalle.DataSource = this.DetalleOC;
                    //gvDetalle.RefreshData();

                    //txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

                }
                else
                {

                    dateFechaOrden.EditValue = Convert.ToDateTime(db.GetDateServer());

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
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
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

        public bool ValidarCampos(bool detalle)
        {
            if (txtNumero.Text == "" || String.IsNullOrEmpty(memoComentario.Text))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                return false;
            }

            if (!_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
            {
                if (lkConcepto.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar un concepto contable.", Parametros.MsgType.warning);
                    return false;
                }
            }
            
            if (!Parametros.General.ValidateTipoCambio(dateFechaOrden, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateKardexMovemente(Convert.ToDateTime(dateFechaOrden.EditValue), db, IDEstacionServicio, IDSubEstacion, (chkCombustible.Checked ? 24 : 9), IDArea))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + Convert.ToDateTime(dateFechaOrden.EditValue).ToShortDateString(), Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaOrden, db, IDEstacionServicio))
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
                if (DetalleOC.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                {
                    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                    return false;
                }
                                
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            //if (Convert.ToDecimal(txtGrandTotal.Text) <= 0)
            //{
            //    Parametros.General.DialogMsg("El detalle de la lista no puede estar en 0 'CERO' ", Parametros.MsgType.warning);
            //        return false;
            //}

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    Entidad.Movimiento RM = new Entidad.Movimiento();

                    M.MovimientoTipoID = (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada) ? 1 : (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida) ? 2 : 10));
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = FechaOrden;
                    M.Monto = Decimal.Round(Math.Abs(Convert.ToDecimal(OC.Sum(s => s.CostoTotal))), 2, MidpointRounding.AwayFromZero);
                    M.MonedaID = Parametros.Config.MonedaPrincipal();
                    M.MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                    M.TipoCambio = _TipoCambio;
                    int number = 1;
                    if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                    {
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(1)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(1)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                    }
                    else if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida))
                    {
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(2)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(2)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                    }
                    else if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                    {
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(10)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(10)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                    }

                    M.Numero = number;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = memoComentario.Text;
                    M.AlmacenID = IDAlmacen;
                    M.ConceptoContableID = (!_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado) ? IDConepto : 0);
                    
                    

                    db.Movimientos.InsertOnSubmit(M);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                    "Se registró la salida externa: " + M.Referencia, this.Name);

                    db.SubmitChanges();

                    if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                    {
                        RM.MovimientoTipoID = 11;
                        RM.UsuarioID = UsuarioID;
                        RM.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        RM.FechaRegistro = FechaOrden;
                        RM.Monto = Decimal.Round(Math.Abs(Convert.ToDecimal(OC.Sum(s => s.CostoTotal))), 2, MidpointRounding.AwayFromZero);
                        RM.MonedaID = Parametros.Config.MonedaPrincipal();
                        RM.MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                        RM.TipoCambio = _TipoCambio;
                        RM.Numero = Convert.ToInt32(Numero);
                        RM.EstacionServicioID = IDEstacionServicio;
                        RM.SubEstacionID = IDSubEstacion;
                        RM.Comentario = memoComentario.Text;
                        RM.AlmacenID = IDAlmacen;
                        RM.MovimientoReferenciaID = M.ID;

                        db.Movimientos.InsertOnSubmit(RM);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la entrada externa: " + RM.Referencia, this.Name);
                        
                        db.SubmitChanges();
                    }

                    #region ::: REGISTRANDO EN KARDEX DE BD :::

                    //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                    foreach (var dk in DetalleOC)
                    {
                        int loop = 1;
                        Parametros.TiposMovimientoInventario _TipoKardex = _Tipo;

                            loop = (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Traslado) ? 0 : 1);
                    
                        while (loop < 2)
                        {
                            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                            {
                                if (loop.Equals(0))
                                {
                                    dk.AlmacenSalidaID = IDAlmacen;
                                    _TipoKardex = Parametros.TiposMovimientoInventario.Salida;
                                }
                                else if (loop.Equals(1))
                                    _TipoKardex = Parametros.TiposMovimientoInventario.Entrada;
                                
                            }

                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            //decimal CostoMov = LineaDetalle.Cost;
                            Entidad.Kardex KX = new Entidad.Kardex();

                            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                            {
                                if (loop.Equals(0))
                                    KX.MovimientoID = M.ID;
                                else
                                    KX.MovimientoID = RM.ID;
                            }
                            else
                                KX.MovimientoID = M.ID;
                                                        
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.CantidadInicial = Decimal.Round(dk.CantidadInicial, 3, MidpointRounding.AwayFromZero);
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;
                            KX.Precio = dk.Precio;
                            KX.PrecioTotal = dk.PrecioTotal;
                            KX.Descuento = dk.Descuento;

                            if (dk.CantidadEntrada <= 0)
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                trans.Rollback();
                                Parametros.General.DialogMsg("La cantidad del producto: " + Producto.Codigo + " | " + Producto.Nombre + " no puede ser igual a 0 (cero).", Parametros.MsgType.warning);
                                return false;
                            }

                            //if (dk.CostoEntrada <= 0)
                            //{
                            //    if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            //    {
                            //        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            //        trans.Rollback();
                            //        Parametros.General.DialogMsg("El costo ingresado del producto: " + Producto.Codigo + " | " + Producto.Nombre + " no puede ser igual a 0 (cero).", Parametros.MsgType.warning);
                            //        return false;
                            //    }
                            //    else
                            //    {
                            //        Parametros.General.splashScreenManagerMain.CloseWaitForm();

                            //        if (Parametros.General.DialogMsg("El costo ingresado del producto: " + Producto.Codigo + " | " + Producto.Nombre + " es 0 (cero)." + Environment.NewLine + "¿Desea seguir con el proceso?.", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            //        {
                            //            trans.Rollback();
                            //            return false;
                            //        }
                            //        else
                            //            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                            //    }
                            //}

                            if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Salida))
                            {
                                KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KX.CantidadSalida = Decimal.Round(dk.CantidadEntrada, 3, MidpointRounding.AwayFromZero);
                                KX.CostoSalida = Decimal.Round(dk.CostoEntrada, 4, MidpointRounding.AwayFromZero);
                                //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                                KX.CantidadFinal = Decimal.Round(KX.CantidadInicial - KX.CantidadSalida, 3, MidpointRounding.AwayFromZero);
                                KX.CostoFinal = Decimal.Round(KX.CostoSalida, 4, MidpointRounding.AwayFromZero);
                                KX.CostoTotal = Decimal.Round(KX.CostoSalida * KX.CantidadSalida, 2, MidpointRounding.AwayFromZero);

                                if (KX.CantidadSalida > KX.CantidadInicial)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    trans.Rollback();
                                    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                    return false;
                                }

                                db.Kardexes.InsertOnSubmit(KX);
                            }
                            else if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            {
                                KX.AlmacenEntradaID = dk.AlmacenEntradaID;
                                KX.CantidadEntrada = Decimal.Round(dk.CantidadEntrada, 3, MidpointRounding.AwayFromZero);
                                KX.CostoEntrada = Decimal.Round(dk.CostoEntrada, 4, MidpointRounding.AwayFromZero);
                                //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                                KX.CantidadFinal = Decimal.Round(KX.CantidadInicial + KX.CantidadEntrada, 3, MidpointRounding.AwayFromZero);
                                KX.CostoTotal = Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero);

                                
                                if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                                {

                                    decimal VCtoEntrada, vCtoFinal;
                                    Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);

                                    KX.CostoInicial = Decimal.Round(vCtoFinal, 4, MidpointRounding.AwayFromZero);

                                    KX.CostoEntrada = Decimal.Round(dk.CostoEntrada, 4, MidpointRounding.AwayFromZero);

                                    decimal vSaldo = 0m;
                                    Parametros.General.SaldoCostoTotalKardex(db, KX.ID, 0, IDEstacionServicio, IDSubEstacion, Producto.ID, FechaOrden, out vSaldo);

                                    if (KX.CantidadFinal > 0)
                                        KX.CostoFinal = Math.Abs(Decimal.Round(Convert.ToDecimal((vSaldo + KX.CostoTotal) / KX.CantidadFinal), 4, MidpointRounding.AwayFromZero));
                                    else
                                        KX.CostoFinal = Math.Abs(Decimal.Round(KX.CostoEntrada, 4, MidpointRounding.AwayFromZero));

                                    ////////////////////////////////////////////////////////////////////////////
                                
                                }
                                else
                                    KX.CostoFinal = Decimal.Round(KX.CostoEntrada, 4, MidpointRounding.AwayFromZero);

                                db.Kardexes.InsertOnSubmit(KX);
                                db.SubmitChanges();
             //Validar Recalculos de Costo
                                #region <<< ACTUALIZAR COSTOS >>>

                                //var ActualizarCosto = (from k in db.Kardexes
                                //                       join m in db.Movimientos on k.Movimiento equals m
                                //                       join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                //                       where k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && k.ProductoID.Equals(KX.ProductoID) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                                //                       && k.Fecha.Date >= KX.Fecha.Date
                                //                       select new { k.ID, k.Fecha, mt.Entrada }).OrderBy(o => o.Fecha).ThenBy(t => !t.Entrada).ToList();

                                //var KCIList = (from k in db.Kardexes
                                //               join m in db.Movimientos on k.MovimientoID equals m.ID
                                //               where k.ProductoID.Equals(KX.ProductoID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                //             && k.EstacionServicioID.Equals(KX.EstacionServicioID) && k.SubEstacionID.Equals(KX.SubEstacionID) && !k.CostoEntrada.Equals(0)
                                //             && k.Fecha.Date <= KX.Fecha.Date
                                //               select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();


                                //decimal vCosto = 0m;
                                //if (KCIList.Count() > 0)
                                //    vCosto = Decimal.Round(KCIList.First().CostoFinal, 4, MidpointRounding.AwayFromZero);

                                //foreach (var k in ActualizarCosto)
                                //{
                                //    if (!k.ID.Equals(KX.ID))
                                //    {
                                //        Entidad.Kardex NK = db.Kardexes.Single(o => o.ID.Equals(k.ID));

                                //        if (NK.Movimiento.MovimientoTipoID.Equals(1) || NK.Movimiento.MovimientoTipoID.Equals(3))
                                //        {

                                //            ////Costo Inicial es vCosto
                                //            //decimal vCantidadInicial = 0m;
                                //            ////Cantidad Inicial para el calculo
                                //            //vCantidadInicial = Parametros.General.SaldoKardex(db, KX.EstacionServicioID, KX.SubEstacionID, KX.ProductoID, 0, KX.Fecha, true);
                                //            ////Costo Total Inicial para el calculo
                                //            //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);
                                //            //vCosto = Decimal.Round(Convert.ToDecimal((vCostosInicialesRecalculo + KX.CostoTotal) / KX.CantidadFinal), 4, MidpointRounding.AwayFromZero);

                                //            //if (NK.Fecha.Date >= KX.Fecha.Date)
                                //            //    NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                //            //Costo Inicial es vCosto
                                //            decimal vCantidades = 0m;
                                //            //Cantidad Inicial para el calculo
                                //            vCantidades = Parametros.General.SaldoKardex(db, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, 0, NK.Fecha, true);
                                //            //Costo Total Inicial para el calculo
                                //            //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);

                                //            decimal vSaldoAct = 0m;
                                //            Parametros.General.SaldoCostoTotalKardex(db, 0, NK.EstacionServicioID, NK.SubEstacionID, NK.ProductoID, NK.Fecha, out vSaldoAct);

                                //            vCosto = Decimal.Round(Convert.ToDecimal(vSaldoAct / vCantidades), 4, MidpointRounding.AwayFromZero);

                                //            if (NK.Fecha.Date >= KX.Fecha.Date)
                                //                NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                //        }
                                //        else
                                //        {

                                //            NK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //            if (!NK.CostoEntrada.Equals(0))
                                //            {
                                //                NK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                //            }

                                //            if (!NK.CostoSalida.Equals(0))
                                //            {
                                //                NK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                //                NK.CostoTotal = Decimal.Round(NK.CostoFinal * NK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                //            }

                                //            db.SubmitChanges();

                                //            var Anterior = db.Kardexes.Where(o => o.MovimientoID.Equals(NK.MovimientoID) && !o.ID.Equals(NK.ID)).Select(s => new { s.ID, s.CostoFinal });

                                //            decimal vSuma = 0m;

                                //            if (Anterior.Count() > 0)
                                //                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoFinal), 2, MidpointRounding.AwayFromZero);

                                //            //Entidad.Movimiento MK = db.Movimientos.Single(o => o.ID.Equals(NK.MovimientoID));

                                //            var area = from a in db.Areas
                                //                       join pc in db.ProductoClases on a.ID equals pc.AreaID
                                //                       join p in db.Productos on pc.ID equals p.ProductoClaseID
                                //                       where p.ID.Equals(NK.ProductoID)
                                //                       select a;

                                //            if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                //            {
                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                //                if (NCC != null)
                                //                {
                                //                    if (NCC.Monto < 0)
                                //                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                    else
                                //                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                    db.SubmitChanges();
                                //                }
                                //            }
                                //            else
                                //            {
                                //                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaCostoID;

                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                //                if (NCC != null)
                                //                {
                                //                    if (NCC.Monto < 0)
                                //                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                    else
                                //                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                    db.SubmitChanges();
                                //                }
                                //            }

                                //            if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                //            {
                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                //                if (NCC != null)
                                //                {
                                //                    if (NCC.Monto < 0)
                                //                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                    else
                                //                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                    db.SubmitChanges();
                                //                }
                                //            }
                                //            else
                                //            {
                                //                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(NK.ProductoID)).CuentaInventarioID;

                                //                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(NK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                //                if (NCC != null)
                                //                {
                                //                    if (NCC.Monto < 0)
                                //                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                //                    else
                                //                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + NK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                //                    db.SubmitChanges();
                                //                }
                                //            }

                                //        }
                                //    }
                                //}
                                #endregion

                            }
                            
                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                            if (!chkCombustible.Checked)
                            {
                                var AL = (from al in db.AlmacenProductos
                                          where al.ProductoID.Equals(Producto.ID)
                                            && al.AlmacenID.Equals((_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada) ? KX.AlmacenEntradaID : KX.AlmacenSalidaID))
                                          select al).ToList();

                                if (!AL.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals((_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada) ? KX.AlmacenEntradaID : KX.AlmacenSalidaID)));
                                    if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada))
                                        AP.Cantidad = AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad + KX.CantidadEntrada;
                                    if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Salida))
                                        AP.Cantidad = AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad - KX.CantidadSalida;
                                }
                                else
                                {
                                    if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada))
                                    {
                                        Entidad.AlmacenProducto AP = new Entidad.AlmacenProducto();
                                        AP.ProductoID = Producto.ID;
                                        AP.AlmacenID = KX.AlmacenEntradaID;
                                        AP.Cantidad = KX.CantidadFinal;
                                        //decimal precio = Decimal.Round((KX.CostoFinal * 1.50m), 6, MidpointRounding.AwayFromZero);
                                                                               
                                        //-- INSERTAR REGISTRO 
                                        //AP.PrecioSugerido = precio;
                                        AP.Costo = dk.CostoFinal;
                                        db.AlmacenProductos.InsertOnSubmit(AP);

                                    }
                                }
                            }
                            else
                            {
                                if (_TipoKardex.Equals(Parametros.TiposMovimientoInventario.Entrada))
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
                            }

                            //------------------------------------------------------------------------//

                            #endregion
                            
                            loop++;
                            db.SubmitChanges();
                        }
                                                
                        //para que actualice los datos del registro
                        db.SubmitChanges();
                    }

                    #region <<< REGISTRANDO COMPROBANTE >>>
                    if (!_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
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

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
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
                    Entidad.Movimiento M;
                    if (!chkCombustible.Checked)
                    {
                        if (!IDConepto.Equals(0) & !IDArea.Equals(0) & !IDAlmacen.Equals(0))
                        {
                            int Costo = db.Areas.Single(c => c.ID.Equals(IDArea)).CentroCostoID;
                            List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            {

                                string texto = "Entrada de Inventario Nro. " + Numero.ToString();
                                int i = 1;

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
                                                Monto = Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
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
                                            comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
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
                                                Monto = Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
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
                                            comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                        }
                                    }

                                    #endregion

                                });

                                #endregion

                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID,
                                    Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaOrden,
                                    Descripcion = texto,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion,
                                    CentroCostoID = (db.TipoCuentas.Single(t => t.ID.Equals(Convert.ToInt32(db.CuentaContables.Single(c => c.ID.Equals(Convert.ToInt32(db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID))).IDTipoCuenta))).UsaCentroCosto ? Costo : 0)

                                });
                                i++;
                            }
                            else if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida))
                            {

                                string texto = "Salida de Inventario Nro. " + Numero.ToString();
                                int i = 1;

                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID,
                                    Monto = Decimal.Round(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaOrden,
                                    Descripcion = texto,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion,
                                    CentroCostoID = (db.TipoCuentas.Single(t => t.ID.Equals(Convert.ToInt32(db.CuentaContables.Single(c => c.ID.Equals(Convert.ToInt32(db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID))).IDTipoCuenta))).UsaCentroCosto ? Costo : 0)

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
                                        if (!IDInventario.Contains(producto.CuentaInventarioID))
                                        {
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = producto.CuentaInventarioID,
                                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
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
                                            var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                            comprobante.Monto += Decimal.Round(-Math.Abs(Convert.ToDecimal(K.CostoTotal)), 2, MidpointRounding.AwayFromZero);
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(-Math.Abs(Convert.ToDecimal(K.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                        }
                                    }

                                    #endregion

                                });

                                #endregion

                            }


                            return CD;
                        }
                        else
                        {
                            Parametros.General.DialogMsg("Debe llenar los campos requeridos.", Parametros.MsgType.warning);
                            return null;
                        }
                    }
                    else if (chkCombustible.Checked)
                    {
                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();

                        string texto = "Entrada de Inventario Nro. " + Numero.ToString();
                        int i = 1;

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
                                        Monto = Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
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
                                    comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
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
                                        Monto = Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
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
                                    var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                    comprobante.Monto += Decimal.Round(Convert.ToDecimal(K.CostoTotal), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(K.CostoTotal) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion

                        });

                        #endregion

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID,
                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)), 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(OC.Sum(s => s.CostoTotal)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                            Fecha = FechaOrden,
                            Descripcion = texto,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion,
                            CentroCostoID = 0 //(db.TipoCuentas.Single(t => t.ID.Equals(Convert.ToInt32(db.CuentaContables.Single(c => c.ID.Equals(Convert.ToInt32(db.ConceptoContables.Single(s => s.ID.Equals(IDConepto)).CuentaContableID))).IDTipoCuenta))).UsaCentroCosto ?  Costo : 0)

                        });
                        i++;

                        return CD;
                    }
                    else
                        return null;
                    
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
            MDI.CleanDialog(ShowMsg,  IDPrint, RefreshMDI, _Next);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (!_Next)
                {
                    if (DetalleOC.Count > 0 || rgOptions.SelectedIndex >= 0)
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

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            decimal _MargenCosto = 0;
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

            //-- Validar Columna de Productos             
            if (view.GetRowCellValue(RowHandle, "UnidadMedidaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "UnidadMedidaID")) == 0)
                {
                    view.SetColumnError(view.Columns["UnidadMedidaID"], "No existe Unidad de Medida para el producto seleccionado");
                    e.ErrorText = "No existe Unidad de Medida para el producto seleccionado";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["UnidadMedidaID"], "No existe Unidad de Medida para el producto seleccionado");
                e.ErrorText = "No existe Unidad de Medida para el producto seleccionado";
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

            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada) || _Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
            {
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

                if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                {
                decimal NewPrice = 0;
                decimal OldPrice = 0;
                bool _OutMargen = false;

                if (view.GetRowCellValue(RowHandle, "CostoEntrada") != null)
                {
                    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoEntrada")).Equals(0))
                    {
                        NewPrice = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CostoEntrada"));
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
                }
            }

            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida))
            {
                //-- Validar Columna de Productos             
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
            }
        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        private void gvDetalle_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (chkCombustible.Checked)
                {
                    if (e.Column == ColAlmacenEntrada)
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                            {
                                var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                                ColAlmacenEntrada.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);
                                cboAlmacenEntrada.DataSource = TCombustible;
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

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (ValidarCampos(true))
            {
                #region <<< CALCULOS_MONTOS >>>
                //--  Calcular las montos de las columnas de Impuestos y Subtotal
                //&& IDConepto.Equals(IDInvntarioInicial)
                if (e.Column == colQuantity || (rgOptions.SelectedIndex.Equals(0) && e.Column == colCostoTotal))//(e.Column == colCostoTotal) || )
                {
                    decimal vCosto = 0, vCantidad = 0, vCostoTotal = 0, vExistencia = 0;
                    
                    if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida) || _Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada") != null)
                            vCosto = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada")), 4, MidpointRounding.AwayFromZero);
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada") != null)
                            vCantidad = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")), 3, MidpointRounding.AwayFromZero);
                        //else
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);

                        vCostoTotal = Decimal.Round((vCosto * vCantidad), 2, MidpointRounding.AwayFromZero);

                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoTotal", vCostoTotal);


                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial") != null)
                            vExistencia = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial"));
                        
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                            {
                                if (vCantidad > vExistencia)
                                {
                                    Parametros.General.DialogMsg("La cantidad a salir sobrepasa la existencia", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);
                                }
                            }
                        }
                    }

                    if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                    {

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
                            vCostoTotal = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal")), 2, MidpointRounding.AwayFromZero);

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada") != null)
                        {
                            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) <= 0m)
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);
                            else
                                vCantidad = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")), 3, MidpointRounding.AwayFromZero);
                        }
                        else
                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

                        vCosto = (vCantidad > 0 ? Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero) : 0);

                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", vCosto);

                        decimal _MargenCosto = 0;

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")).Equals(0))
                            {
                                if (gvDetalle.GetRowCellValue(e.RowHandle, "Precio") != null)
                                    _MargenCosto = db.Productos.SingleOrDefault(o => o.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).MargenToleranciaCosto;
                                decimal NewPrice = 0;
                                decimal OldPrice = 0;
                                bool _OutMargen = false;

                                if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada") != null)
                                {
                                    if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada")).Equals(0))
                                    {
                                        NewPrice = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoEntrada"));
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

                    txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                }
                #endregion
                
                #region <<< COLUMNA_PRODUCTO >>>
                //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
                if (e.Column == colProduct)
                {
                    if (!chkCombustible.Checked)
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
                                gvDetalle.FocusedColumn = colProduct;
                                return;
                            }
                        }
                    }

                    try
                    {
                        var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                        //-- Unidad Principal     
                        //var Um = Producto.UnidadMedidaID;
                        gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                        //var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(IDAlmacen) & a.ProductoID.Equals(Producto.ID));

                        decimal VCtoEntrada, vCtoFinal;
                        Parametros.General.LastCost(db, IDEstacionServicio, IDSubEstacion, Producto.ID, 0, FechaOrden, out vCtoFinal, out VCtoEntrada);

                        gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", vCtoFinal);
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto.ID, IDAlmacen, FechaOrden, (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada) ? true : false)));

                        if (!_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                        {
                            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadInicial")) < 0)
                            {
                                Parametros.General.DialogMsg("El producto seleccionado tiene existencia invalida.", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                                gvDetalle.FocusedColumn = colProduct;
                                return;
                            }
                        }

                        if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            gvDetalle.SetRowCellValue(e.RowHandle, "CostoInicial", VCtoEntrada);

                        //if (query.ToList().Count > 0)
                        //{
                        //    decimal vExistencia = query.First().Cantidad;
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Decimal.Round(query.First().Cantidad, 3, MidpointRounding.AwayFromZero));
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", Decimal.Round(query.First().Costo, 4, MidpointRounding.AwayFromZero));
                        //}
                        //else
                        //{
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0.000m);
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", 0.0000m);
                        //}

                        //////////////
                        //var KCI = (from k in db.Kardexes
                        //           join m in db.Movimientos on k.MovimientoID equals m.ID
                        //           where k.ProductoID.Equals(Producto.ID) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
                        //             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                        //           select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();


                        ////-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                        ////-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                        //if (KCI.Count().Equals(0))
                        //{
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0.0000m);
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", 0.0000m);
                        //}
                        //else
                        //{
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", KCI.First().CantidadFinal);
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "CostoEntrada", KCI.First().CostoFinal);
                        //}
                        //////////

                        if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada) || _Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                        {
                            if (!chkCombustible.Checked)
                            {
                                var lista = (_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado) ? listaAlmacen.Where(l => !l.ID.Equals(IDAlmacen)) : listaAlmacen);
                                cboAlmacenEntrada.DataSource = lista;//lkAlmacen.Properties.DataSource;
                                gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada) ? IDAlmacen : (lista.Count() > 0 ? lista.First().ID : 0)));

                                //-- Cantidad Inicial de 1
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);
                            }
                            else if (chkCombustible.Checked)
                            {
                                var lista = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                                cboAlmacenEntrada.DataSource = lista;//lkAlmacen.Properties.DataSource;
                                gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", (lista.Count() > 0 ? lista.First().ID : 0));

                                //-- Cantidad Inicial de 1
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0);
                            }
                        }
                        else if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida))
                        {
                            cboAlmacenSalida.DataSource = lkAlmacen.Properties.DataSource;
                            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", IDAlmacen);

                            //-- Cantidad Inicial de 1
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 0); 
                        
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

                                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, Producto, Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")), FechaOrden, false));

                                        if (!chkCombustible.Checked)
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
                                        else if (chkCombustible.Checked)
                                        {
                                            //var TCombustible = db.Tanques.Where(t => t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                                            //cboAlmacenEntrada.DataSource = TCombustible;

                                        }
                                    }
                                }

                                //if (gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID") != null)
                                //{
                                //    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")) > 0)
                                //    {
                                //        int Producto = Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"));

                                //        var query = db.AlmacenProductos.Where(a => a.AlmacenID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) & a.ProductoID.Equals(Producto));

                                //        if (query.ToList().Count > 0)
                                //        {
                                //            decimal vExistencia = query.First().Cantidad;
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", vExistencia);
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "Precio", Decimal.Round(query.First().PrecioVenta, 2, MidpointRounding.AwayFromZero));
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);
                                //        }
                                //        else
                                //        {
                                //            Parametros.General.DialogMsg("El producto no se encuentra en el almacen seleccionado.", Parametros.MsgType.warning);
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                                //            gvDetalle.SetRowCellValue(e.RowHandle, "Precio", 0);
                                //        }
                                //    }
                                //}
                            }
                        }

                        //cboAlmacen.DataSource = listaAlmacen.ToList();

                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }

                #endregion

                }
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
            if (dateFechaOrden.EditValue != null)
            {
                if (IDArea > 0 || chkCombustible.Checked)
                {
                    if (Parametros.General.ValidateKardexMovemente(FechaOrden, db, IDEstacionServicio, IDSubEstacion, 9, IDArea))
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
                    else
                    {
                        DateTime fecha = Convert.ToDateTime(dateFechaOrden.EditValue);
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString() + " y Área seleccionada.", Parametros.MsgType.warning);
                        dateFechaOrden.EditValue = null;
                    }
                }
                else
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
            }
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
            if (DetalleOC.Count > 0 || rgOptions.SelectedIndex >= 0)
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
               
        private void lkAlmacen_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (IDArea.Equals(0))
            {
                Parametros.General.DialogMsg("No ha seleccionado el Área.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
            if (DetalleOC.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }
        
        private void lkArea_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (IDConepto.Equals(0) && !_Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
            {
                Parametros.General.DialogMsg("No ha seleccionado el Concepto Contable.", Parametros.MsgType.warning);
                e.Cancel = true;
                return;
            }
            if (rgOptions.SelectedIndex < 0)
            {
                Parametros.General.DialogMsg("No ha seleccionado el tipo de Movimiento.", Parametros.MsgType.warning);
                e.Cancel = true;
                return;
            }
            if (DetalleOC.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
                return;
            }
            if (IDAlmacen > 0)
            {
                Parametros.General.DialogMsg("No se puede modificar el Área una vez seleccionado el almacén.", Parametros.MsgType.warning);
                e.Cancel = true;
                return;
            }

            if ((int)e.NewValue  > 0)
            {
                if (dateFechaOrden.EditValue != null)
                {
                    if (!Parametros.General.ValidateKardexMovemente(FechaOrden, db, IDEstacionServicio, IDSubEstacion, 9, (int)e.NewValue))
                    {
                        DateTime fecha = Convert.ToDateTime(dateFechaOrden.EditValue);
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString() + " y Área seleccionada.", Parametros.MsgType.warning);
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    Parametros.General.DialogMsg("La fecha no es correcta, favor verificar la fecha del Movimiento de Inventario", Parametros.MsgType.warning);
                    e.Cancel = true;
                    return;
                }
            }
        }

        //Sumando los totales al cambiar de filas
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            try
            {
                txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

                if (chkCombustible.Checked)
                {
                    cboAlmacenEntrada.DataSource = listaTanque;

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void rgOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgOptions.SelectedIndex.Equals(0))
                {
                    _Tipo = Parametros.TiposMovimientoInventario.Entrada;
                    layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemAlmacen.Text = "Almacén";
                    this.btnShowComprobante.Enabled = true;
                    ColAlmacenEntrada.Caption = "Almacén";
                    ColAlmacenEntrada.Visible = true;
                    ColAlmacenEntrada.VisibleIndex = 2;
                    colCostoInicial.Visible = true;
                    colCostoInicial.VisibleIndex = 4;
                    ColAlmacenSalida.Visible = false;
                    colExistencia.Visible = false;
                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(1)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(1)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
                else if (rgOptions.SelectedIndex.Equals(1))
                {
                    _Tipo = Parametros.TiposMovimientoInventario.Salida;
                    layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemAlmacen.Text = "Almacén";
                    this.btnShowComprobante.Enabled = true;
                    ColAlmacenEntrada.Visible = false;
                    colCostoInicial.Visible = false;
                    ColAlmacenSalida.Visible = true;
                    ColAlmacenSalida.VisibleIndex = 2;
                    colExistencia.Visible = true;
                    colExistencia.VisibleIndex = 3;

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(2)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(2)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
                else if (rgOptions.SelectedIndex.Equals(2))
                {
                    _Tipo = Parametros.TiposMovimientoInventario.Traslado;
                    layoutControlItemConcepto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemAlmacen.Text = "Almacén Traslado";
                    this.btnShowComprobante.Enabled = false;
                    colCostoInicial.Visible = false;
                    ColAlmacenEntrada.Caption = "Almacén Destino";
                    ColAlmacenEntrada.Visible = true;
                    ColAlmacenEntrada.VisibleIndex = 2;
                    colExistencia.Visible = true;
                    colExistencia.VisibleIndex = 3;

                    int number = 1;
                    if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(10)) > 0)
                    {
                        number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(10)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                    }

                    txtNumero.Text = number.ToString("000000000");
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        private void lkAlmacen_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkAlmacen.EditValue != null)
                {
                    if (!IDArea.Equals(0))
                    {
                        if (!IDAlmacen.Equals(0) && !Editable)
                        {
                            if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Entrada))
                            {
                                gridProductos.DataSource = null;
                                if (rgOptions.SelectedIndex.Equals(0) && IDConepto.Equals(IDInvntarioInicial))
                                {

                                    var ListaProductos = from P in db.Productos
                                                         join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                         join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                         join A in db.Areas on C.AreaID equals A.ID
                                                         where P.Activo.Equals(true) && !P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible()) && !C.AreaID.Equals(ProductoAreaServicioID) && A.Activo && C.Activo && A.ID.Equals(IDArea)
                                                         select new
                                                         {
                                                             P.ID,
                                                             P.Codigo,
                                                             P.Nombre,
                                                             UmidadName = U.Nombre,
                                                             Display = P.Codigo + " | " + P.Nombre
                                                         };

                                    gridProductos.DataSource = ListaProductos;

                                }
                                else
                                {
                                    var ListaProductos = from P in db.Productos
                                                         join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                         join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                         join A in db.Areas on C.AreaID equals A.ID
                                                         join AL in db.AlmacenProductos on P.ID equals AL.ProductoID
                                                         join ALM in db.Almacens on AL.AlmacenID equals ALM.ID
                                                         where P.Activo.Equals(true) & A.Activo & C.Activo && A.ID.Equals(IDArea) && !C.ID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                         && ALM.Activo && ALM.EstacionServicioID.Equals(IDEstacionServicio) && ALM.SubEstacionID.Equals(IDSubEstacion)
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
                                }            

                            }
                            else if (_Tipo.Equals(Parametros.TiposMovimientoInventario.Salida) || _Tipo.Equals(Parametros.TiposMovimientoInventario.Traslado))
                            {
                                gridProductos.DataSource = null;
                                var ListaProductos = from P in db.Productos
                                                     join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                     join C in db.ProductoClases on P.ProductoClaseID equals C.ID
                                                     join A in db.Areas on C.AreaID equals A.ID
                                                     join AL in db.AlmacenProductos on P.ID equals AL.ProductoID
                                                     join ALM in db.Almacens.Where(o => o.ID.Equals(IDAlmacen)) on AL.AlmacenID equals ALM.ID
                                                     where P.Activo.Equals(true) & A.Activo & C.Activo && A.ID.Equals(IDArea) && !C.ID.Equals(Parametros.Config.ProductoClaseCombustible())
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

                            }

                            rgOptions.Properties.ReadOnly = true;
                        }
                    }
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

                if (!IDConepto.Equals(0) & ((!IDArea.Equals(0) & !IDAlmacen.Equals(0)) || chkCombustible.Checked))
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

        private void lkConcepto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgOptions.SelectedIndex.Equals(0))
                {
                    if (IDConepto.Equals(IDInvntarioInicial))
                    {

                        if (_PermitirCombustibleEntradaInv)
                            layoutControlItemComb.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                        colCostoTotal.OptionsColumn.AllowFocus = true;
                        colCostoTotal.OptionsColumn.AllowEdit = true;
                        colCostoTotal.OptionsColumn.ReadOnly = false;

                    }
                    else
                    {
                        chkCombustible.Checked = false;
                        layoutControlItemComb.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        colCostoTotal.OptionsColumn.AllowFocus = true;
                        colCostoTotal.OptionsColumn.AllowEdit = true;
                        colCostoTotal.OptionsColumn.ReadOnly = false;

                        //colCostoTotal.OptionsColumn.AllowFocus = false;
                        //colCostoTotal.OptionsColumn.AllowEdit = false;
                        //colCostoTotal.OptionsColumn.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Verifica si es carga de combustible
        private void chkCombustible_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                rgOptions.Properties.ReadOnly = false;

                if (chkCombustible.Checked)
                {
                    lkArea.EditValue = null;
                    lkArea.Enabled = false;
                    lkAlmacen.EditValue = null;
                    lkAlmacen.Enabled = false;

                    gridProductos.DataSource = null;

                    var ListaProductos = from P in db.Productos
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

                    cboAlmacenEntrada.DataSource = null;
                    gridProductos.DataSource = ListaProductos;
                }
                else
                {

                    lkArea.Enabled = true;
                    lkAlmacen.Enabled = true;

                    gridProductos.DataSource = null;

                    cboAlmacenEntrada.DataSource = listaAlmacen.ToList();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void gvDetalle_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (chkCombustible.Checked)
                {
                    cboAlmacenEntrada.DataSource = listaTanque;
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