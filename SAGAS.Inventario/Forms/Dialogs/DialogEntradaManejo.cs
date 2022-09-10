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
    public partial class DialogEntradaManejo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEntradaManejo MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        private int IDPrint = 0;
        internal bool Next = false;
        internal bool _Editable = false;
        internal int _ClaseCombustible;

        private string Referencia
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
        }

        private string Comentario
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }

        private DateTime FechaEntrada
        {
            get { return Convert.ToDateTime(dateFechaEntrada.EditValue); }
            set { dateFechaEntrada.EditValue = value; }
        }

        private DateTime FechaFactura
        {
            get { return Convert.ToDateTime(dateFechaFactura.EditValue); }
            set { dateFechaFactura.EditValue = value; }
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

        public DialogEntradaManejo(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            _Editable = _editando;
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

                glkClient.Properties.DataSource = (from c in db.Clientes
                                                  join ces in db.ClienteEstacions on c equals ces.Cliente
                                                  where c.Activo && c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID()) && ces.EstacionServicioID.Equals(IDEstacionServicio)
                    select new { c.ID, c.Codigo, c.Nombre, Display = c.Codigo + " | " + c.Nombre }).ToList();

                _ClaseCombustible = Parametros.Config.ProductoClaseCombustible();

                //db.Clientes.Where(c => c.Activo && c.TipoClienteID.Equals(Parametros.Config.TipoClienteManejoID())).Select(s => new { s.ID, s.Codigo, s.Nombre, Display = s.Codigo + " | " + s.Nombre }).ToList();

                //---LLenar Almacenes ---//
                listaTanque = from t in db.Tanques
                              join p in db.Productos on t.ProductoID equals p.ID
                              where t.Activo && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                              select new Parametros.ListIdDisplay { ID = t.ID, Display = t.Nombre + " | " + p.Nombre };

                //Almacen GRID
                cboAlmacenEntrada.DataSource = listaTanque.ToList();
                //cboAlmacenEntrada.DisplayMember = "Display";
                //cboAlmacenEntrada.ValueMember = "ID";
                //Almacen FORM
                //lkBodega.Properties.DisplayMember = "Display";
                //lkBodega.Properties.ValueMember = "ID";


                cboUnidadMedida.DataSource = from U in db.UnidadMedidas
                                             select new { U.ID, U.Nombre };
                cboUnidadMedida.DisplayMember = "Nombre";
                cboUnidadMedida.ValueMember = "ID";
                //-------------------------------//

                //--- Fill Combos Detalles --//
                gridProductos.View.OptionsBehavior.AutoPopulateColumns = false;
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

                if (_Editable)
                {
                    Referencia = EntidadAnterior.Referencia;
                        Comentario = EntidadAnterior.Comentario;
                        FechaEntrada = EntidadAnterior.FechaRegistro;
                        FechaFactura = Convert.ToDateTime(EntidadAnterior.FechaFisico);
                    IDCliente = EntidadAnterior.ClienteID;
                    this.DetalleEM = EntidadAnterior.Kardexes.ToList();
                    this.bdsDetalle.DataSource = this.DetalleEM;

                    this.dateFechaEntrada.Enabled = this.dateFechaFactura.Enabled = false;

                    this.gvDetalle.OptionsBehavior.Editable = false;
                    this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                }
                else
                {
                    dateFechaEntrada.EditValue = Convert.ToDateTime(db.GetDateServer());
                    FechaFactura = FechaEntrada;
                    this.bdsDetalle.DataSource = this.DetalleEM;
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
        }

        private bool ValidarReferencia(string code, int ID)
        {
            var result = (from i in db.Movimientos
                          where (ID > 0 ? i.Referencia.Equals(code) && i.MovimientoTipoID.Equals(18) && !i.Anulado && i.ClienteID.Equals(IDCliente) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion) && i.ID != Convert.ToInt32(ID) :
                          i.Referencia.Equals(code) && i.MovimientoTipoID.Equals(18) && !i.Anulado && i.ClienteID.Equals(IDCliente) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion))
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }


        public bool ValidarCampos(bool detalle)
        {
            if (!Parametros.General.ValidateKardexMovemente(FechaEntrada, db, IDEstacionServicio, IDSubEstacion, 24, 0)) {
                Parametros.General.DialogMsg("El acta de combustible ya esta cerrado", Parametros.MsgType.warning);
            }

            if (txtReferencia.Text == "" || Convert.ToInt32(glkClient.EditValue) <= 0)
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

            if (dateFechaEntrada.EditValue == null || dateFechaFactura.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe ingresar las fechas correctamente.", Parametros.MsgType.warning);
                return false;
            }

            if (FechaEntrada.Date > Convert.ToDateTime(db.GetDateServer()).Date || FechaFactura.Date > Convert.ToDateTime(db.GetDateServer()).Date)
            {
                Parametros.General.DialogMsg("La fecha de recibido o de factura no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaFactura, db, IDEstacionServicio))
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
                else
                {
                    //BLOQUEO PARA COMBUSTIBLE
                    List<int> PAreas = (from p in db.Productos
                                        join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                        where DetalleEM.Select(s => s.ProductoID).Contains(p.ID) && pc.ID.Equals(_ClaseCombustible)
                                        group pc by pc.AreaID into gr
                                        select gr
                                       ).Select(s => s.Key).ToList();

                    if (PAreas.Count > 0)
                    {
                        if (!Parametros.General.ValidateKardexMovemente(FechaEntrada.Date, db, IDEstacionServicio, IDSubEstacion, 24, 0))
                        {
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + FechaEntrada.Date.ToShortDateString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }

                if (!ValidarReferencia(txtReferencia.Text, (EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
                {
                    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(Referencia), Parametros.MsgType.warning);
                    return false;
                }
                                
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false))
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

                    
                    Entidad.Movimiento M;

                    if (_Editable)
                    {
                        M = db.Movimientos.Single(e => e.ID == EntidadAnterior.ID);
                        M.Modificado = true;
                    }
                    else
                    {
                        M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 18;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaEntrada;
                        M.FechaFisico = FechaFactura;
                        M.Litros = Decimal.Round(DetalleEM.Sum(s => s.CantidadEntrada), 3, MidpointRounding.AwayFromZero);
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                    }

                    M.ClienteID = IDCliente;
                    M.Referencia = Referencia;
                    M.Comentario = memoComentario.Text;

                    db.SubmitChanges();

                    if (_Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó la Entrada de Manejo: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la Entrada de Manejo: " + M.Numero, this.Name);
                    }

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN KARDEX DE BD :::
                    
                        //------------------------------ INSERTAR DATOS KARDEX ------------------------------//
                        foreach (var dk in DetalleEM)
                        {
                            if (!_Editable)
                            {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();

                            KX.MovimientoID = M.ID;
                            KX.ProductoID = Producto.ID;
                            KX.EsManejo = true;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = dk.UnidadMedidaID;
                            KX.Fecha = Convert.ToDateTime(M.FechaRegistro);
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;
                            KX.ImpuestoTotal = dk.ImpuestoTotal;

                            //KX.CantidadInicial = dk.CantidadInicial;

                            KX.AlmacenEntradaID = dk.AlmacenEntradaID;
                            KX.CantidadEntrada = dk.CantidadEntrada;

                            var TP = (from tp in db.TanqueProductos
                                      where tp.ProductoID.Equals(Producto.ID)
                                      && tp.TanqueID.Equals(dk.AlmacenEntradaID)
                                      select tp).ToList();

                            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
                            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
                            if (TP.Count() == 0)
                            {
                                KX.CantidadInicial = 0;
                            }
                            else
                                KX.CantidadInicial = TP.Single(q => q.ProductoID.Equals(dk.ProductoID)).Cantidad;

                            KX.CantidadFinal = KX.CantidadInicial + KX.CantidadEntrada;
                            //KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                            //KX.CantidadSalida = dk.CantidadEntrada;
                            //KX.CostoSalida = dk.CostoEntrada;
                            //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                            //KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;
                            //KX.CostoTotal = KX.CostoSalida * KX.CantidadSalida;

                            //if (KX.CantidadSalida > KX.CantidadInicial)
                            //{
                            //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            //    trans.Rollback();
                            //    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                            //    return false;
                            //}

                            db.Kardexes.InsertOnSubmit(KX);

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

                            var Tanque = (from tp in db.TanqueProductos
                                          where tp.ProductoID.Equals(Producto.ID)
                                            && tp.TanqueID.Equals(KX.AlmacenEntradaID)
                                          select tp).ToList();

                            if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
                            {
                                Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.TanqueID.Equals(KX.AlmacenEntradaID));
                                TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad + KX.CantidadEntrada;
                            }
                            else
                            {
                                Entidad.TanqueProducto TPto = new Entidad.TanqueProducto();
                                TPto.ProductoID = Producto.ID;
                                TPto.TanqueID = KX.AlmacenEntradaID;
                                TPto.Cantidad = KX.CantidadEntrada;
                                db.TanqueProductos.InsertOnSubmit(TPto);

                            }

                            db.SubmitChanges();

                            #endregion

                            }

                            #region ::: REGISTRANDO DEUDOR :::
                            if (_Editable)
                            {
                                db.Deudors.DeleteOnSubmit(db.Deudors.Single(d => d.MovimientoID.Equals(M.ID) && d.ProductoID.Equals(dk.ProductoID)));
                                db.SubmitChanges();
                            }

                            db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = IDCliente, Valor = Decimal.Round(dk.CantidadEntrada, 3, MidpointRounding.AwayFromZero), ProductoID = dk.ProductoID, MovimientoID = M.ID });
                            db.SubmitChanges();
                            #endregion
                        }
                                  
                    #endregion

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
            cboAlmacenEntrada.DataSource = listaTanque;

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

            //-- Validar Columna de Tanque             
            if (view.GetRowCellValue(RowHandle, "AlmacenEntradaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Tanque");
                    e.ErrorText = "Debe Seleccionar un Tanque";
                    e.Valid = false;
                    Validate = false;
                }
                else
                {
                    if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID"))) && t.ProductoID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).Equals(0))
                    {
                        view.SetColumnError(view.Columns["AlmacenEntradaID"], "El Tanque seleccionado no contiene este producto");
                        e.ErrorText = "El Tanque seleccionado no contiene este producto";
                        e.Valid = false;
                        Validate = false;
                    }
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Tanque");
                e.ErrorText = "Debe Seleccionar un Tanque";
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
                        //else if (DetalleEM.Count(d => d.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))) > 1)
                        //{
                        //    Parametros.General.DialogMsg("El producto seleccionado ya existe en la lista.", Parametros.MsgType.warning);
                        //    gvDetalle.SetRowCellValue(e.RowHandle, "ProductoID", 0);
                        //    gvDetalle.FocusedColumn = colProduct;
                        //    return;
                        //}
                    }

                    try
                    {
                        var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

                        //-- Unidad Principal     
                        //var Um = Producto.UnidadMedidaID;
                        gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Producto.UnidadMedidaID);

                        var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                        ColAlmacenEntrada.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                        cboAlmacenEntrada.DataSource = TCombustible;//lkAlmacen.Properties.DataSource;
                        gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", TCombustible.First().ID);

                            //-- Cantidad Inicial de 1
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);
                      
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
                //--
                #endregion

                #region <<< COLUMNA_ALMACEN  >>>

                if (e.Column == ColAlmacenEntrada)
                {
                    try
                    {
                        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
                        {
                            if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) > 0)
                            {
                                var TCombustible = db.Tanques.Where(t => t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre });
                                cboAlmacenEntrada.DataSource = TCombustible;

                                if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenEntradaID"))) && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).Equals(0))
                                {
                                    Parametros.General.DialogMsg("El Tanque seleccionado no contiene este producto", Parametros.MsgType.warning);
                                    gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", 0);
                                }
                            }
                        }

                        //cboAlmacenEntrada.DataSource = listaTanque.ToList();

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
               
        private void lkAlmacen_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (DetalleEM.Count > 0)
            {
                Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }
        
        //Sumando los totales al cambiar de filas
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            cboAlmacenEntrada.DataSource = listaTanque;
        }

        private void gvDetalle_LostFocus(object sender, EventArgs e)
        {
            cboAlmacenEntrada.DataSource = listaTanque;
        }
                
        private void glkClient_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(glkClient.EditValue) > 0)
                {
                    client = db.Clientes.Single(p => p.ID.Equals(IDCliente));
                    //txtRuc.Text = client.RUC;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void dateFechaEntrada_EditValueChanged(object sender, EventArgs e)
        {
            FechaFactura = FechaEntrada;
        }

        #endregion

    }
}