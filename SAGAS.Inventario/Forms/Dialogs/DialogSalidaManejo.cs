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
    public partial class DialogSalidaManejo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormSalidaManejo MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal Entidad.ResumenDia EntidadRD;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private IQueryable<Parametros.ListIdDisplay> listaTanque;
        private List<Parametros.ListIdTidDisplayValue> listaProductoRD;
        private int IDPrint = 0;
        internal bool Next = false;
        internal int _ClaseCombustible;
        internal bool _Editable = false;

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

        private string Placa
        {
            get { return txtNumeroPlaca.Text; }
            set { txtNumeroPlaca.Text = value; }
        }
        
        private DateTime FechaEntrada
        {
            get { return Convert.ToDateTime(dateFechaEntrada.EditValue); }
            set { dateFechaEntrada.EditValue = value; }
        }

        private int IDCliente
        {
            get { return Convert.ToInt32(glkClient.EditValue); }
            set { glkClient.EditValue = value; }
        }

        public bool _SoloSalida
        {
            get { return Convert.ToBoolean(chkSoloSalida.Checked); }
            set { chkSoloSalida.Checked = value; }
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

        public DialogSalidaManejo(int UserID, bool _editando)
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

                if (_Editable)
                {
                    Referencia = EntidadAnterior.Referencia;
                    Comentario = EntidadAnterior.Comentario;
                    FechaEntrada = EntidadAnterior.FechaRegistro;
                    Placa = EntidadAnterior.NumeroPlaca;
                    dateFechaEntrada_Validated(null, null);
                    //if (EntidadAnterior.ResumenDiaID > 0)
                    //    EntidadRD = db.ResumenDias.Single(s => s.ID.Equals(EntidadAnterior.ResumenDiaID));
                    //else
                    //    chkSoloSalida.Checked = true;

                    IDCliente = EntidadAnterior.ClienteID;
                    this.DetalleEM = EntidadAnterior.Kardexes.ToList();
                    this.bdsDetalle.DataSource = this.DetalleEM;

                    this.dateFechaEntrada.Enabled = false;

                    this.gvDetalle.OptionsBehavior.Editable = false;
                    this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                }
                else
                {
                    dateFechaEntrada.EditValue = null;// Convert.ToDateTime(db.GetDateServer());
                    this.bdsDetalle.DataSource = this.DetalleEM;
                }

                if (Parametros.General.SystemOptionAcces(UsuarioID, "chkSoloSalida"))
                    layoutControlItemSoloSalida.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

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
                          where (ID > 0 ? i.Referencia.Equals(code) && i.MovimientoTipoID.Equals(19) && !i.Anulado && i.ClienteID.Equals(IDCliente) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion) && i.ID != Convert.ToInt32(ID) :
                          i.Referencia.Equals(code) && i.MovimientoTipoID.Equals(19) && !i.Anulado && i.ClienteID.Equals(IDCliente) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion))
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (!Parametros.General.ValidateKardexMovemente(FechaEntrada, db, IDEstacionServicio, IDSubEstacion, 24, 0))
            {
                Parametros.General.DialogMsg("El acta de combustible ya esta cerrado", Parametros.MsgType.warning);
            }


            if (txtReferencia.Text == "" || Convert.ToInt32(glkClient.EditValue) <= 0 || dateFechaEntrada.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                return false;
            }

            if (!ValidarReferencia(txtReferencia.Text,(EntidadAnterior == null ? 0 : EntidadAnterior.ID)))
            {
                Parametros.General.DialogMsg("La referencia para esta salida de manejo ya existe.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaEntrada, db, IDEstacionServicio))
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
                        M.MovimientoTipoID = 19;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = FechaEntrada;
                        M.FechaFisico = FechaEntrada;
                        M.Litros = Decimal.Round(DetalleEM.Sum(s => s.CantidadSalida), 3, MidpointRounding.AwayFromZero);
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.ResumenDiaID = (_SoloSalida ? 0 : EntidadRD.ID);
                       
                    }

                    M.NumeroPlaca = Placa;
                    M.ClienteID = IDCliente;
                    M.Referencia = Referencia;
                    M.Comentario = memoComentario.Text;

                    db.SubmitChanges();

                    if (_Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó la Salida de Manejo: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la Salida de Manejo: " + M.Numero, this.Name);
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
                                KX.Fecha = M.FechaRegistro;
                                KX.EstacionServicioID = IDEstacionServicio;
                                KX.SubEstacionID = IDSubEstacion;

                                //KX.CantidadInicial = dk.CantidadInicial;

                                KX.AlmacenSalidaID = dk.AlmacenSalidaID;
                                KX.CantidadSalida = dk.CantidadSalida;

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

                                if (!_SoloSalida)
                                {
                                    if (KX.CantidadSalida > dk.CantidadInicial)
                                    {
                                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                        trans.Rollback();
                                        Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                        return false;
                                    }
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

                            #region ::: REGISTRANDO DEUDOR :::

                            if (_Editable)
                            {
                                db.Deudors.DeleteOnSubmit(db.Deudors.Single(d => d.MovimientoID.Equals(M.ID) && d.ProductoID.Equals(dk.ProductoID)));
                                db.SubmitChanges();
                            }

                            db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = IDCliente, Valor = -Math.Abs(Decimal.Round(dk.CantidadSalida, 3, MidpointRounding.AwayFromZero)), ProductoID = dk.ProductoID, MovimientoID = M.ID });
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
            if (!_SoloSalida)
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
                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
                    e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
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

                        List<Parametros.ListIdDisplayNombre> TCombustible = new List<Parametros.ListIdDisplayNombre>();

                        if (!_SoloSalida)
                        {
                            TCombustible = (from t in db.Tanques
                                           //join l in listaProductoRD on t.ProductoID equals l.ID
                                           where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                           && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                           select new Parametros.ListIdDisplayNombre { ID = t.ID, Display = t.Nombre }).ToList();
                        }
                        else
                        {
                            TCombustible = (from t in db.Tanques
                                            where t.Activo && t.ProductoID.Equals(Producto.ID) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                            select new Parametros.ListIdDisplayNombre { ID = t.ID, Display = t.Nombre }).ToList();
                        }

                        ColAlmacenSalida.OptionsColumn.ReadOnly = (TCombustible.ToList().Count > 1 ? false : true);

                        cboAlmacenSalida.DataSource = TCombustible;//lkAlmacen.Properties.DataSource;
                        gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", (TCombustible.ToList().Count > 0 ? TCombustible.First().ID : 0));

                        if (!_SoloSalida)
                        {
                            var query = listaProductoRD.Where(l => l.ID.Equals(Producto.ID)).FirstOrDefault();

                            if (query != null)
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", query.Value);
                            else
                                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
                        }
                        
                            //-- Cantidad Inicial de 1
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadSalida", 1);


                      
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
                                var TCombustible = from t in db.Tanques
                                                   //join l in listaProductoRD on t.ProductoID equals l.ID
                                                   where t.Activo && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion)
                                                   && listaProductoRD.Select(s => s.ID).Contains(t.ProductoID)
                                                   select new { ID = t.ID, Display = t.Nombre };
                                cboAlmacenSalida.DataSource = TCombustible;

                                if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")))).Equals(0))
                                {
                                    Parametros.General.DialogMsg("El Tanque seleccionado no contiene este producto", Parametros.MsgType.warning);
                                    if (!Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")).Equals(0))
                                        gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenSalidaID", 0);
                                }
                                else
                                {
                                    if (!_SoloSalida)
                                    {
                                        var query = listaProductoRD.Where(l => l.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID"))) && l.TID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "AlmacenSalidaID")))).FirstOrDefault();

                                        if (query != null)
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", query.Value);
                                        else
                                            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadInicial", 0);
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
                    chkSoloSalida.Properties.ReadOnly = true;
                    if (client != null)
                    {
                        if (!_SoloSalida)
                        {
                            var obj = (from d in db.Deudors
                                       join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)) on d.MovimientoID equals m.ID
                                       join p in db.Productos on d.ProductoID equals p.ID
                                       join c in db.Clientes.Where(c => c.ID.Equals(client.ID)) on d.ClienteID equals c.ID
                                       group d by new { d.ProductoID, p.Nombre } into gr
                                       select new
                                       {
                                           ID = gr.Key.ProductoID,
                                           Producto = gr.Key.Nombre,
                                           suma = gr.Sum(s => s.Valor)
                                       }).ToList();

                            lbCliente.Items.Clear();
                            obj.ForEach(item =>
                            {
                                lbCliente.Items.Add(item.Producto + " | " + item.suma.ToString("#,0.000") + " Litros");
                            });

                            gridProductos.DataSource = null;
                            gridProductos.DataSource = from P in db.Productos
                                                       join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                       && obj.Select(s => s.ID).Contains(P.ID)
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
                            gridProductos.DataSource = null;
                            gridProductos.DataSource = from P in db.Productos
                                                       join U in db.UnidadMedidas on P.UnidadMedidaID equals U.ID
                                                       join T in db.Tanques on P.ID equals T.ProductoID
                                                       where P.Activo.Equals(true) && P.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                                       && T.EstacionServicioID.Equals(IDEstacionServicio) && T.SubEstacionID.Equals(IDSubEstacion)
                                                       select new
                                                       {
                                                           P.ID,
                                                           P.Codigo,
                                                           P.Nombre,
                                                           UmidadName = U.Nombre,
                                                           Display = P.Codigo + " | " + P.Nombre
                                                       };
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //Carga Data del Resumen
        private void dateFechaEntrada_EditValueChanged(object sender, EventArgs e)
        {
            
        }
        
        private void dateFechaEntrada_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (!_Editable)
            {
                if (DetalleEM.Count > 0)
                {
                    Parametros.General.DialogMsg("La lista tiene detalle de productos ingresados.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }
            }
        }
        
        private void dateFechaEntrada_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dateFechaEntrada.EditValue != null)
                {
                    if (!_SoloSalida)
                    {
                        EntidadRD = db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDEstacionServicio) && r.SubEstacionID.Equals(IDSubEstacion) && r.FechaInicial.Date.Equals(FechaEntrada.Date)).FirstOrDefault();

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
                                               where rd.ID.Equals(EntidadRD.ID) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial)) && ape.ExtracionID.Equals(Parametros.Config.TipoExtraccionManejoID())
                                               group ape by new { ap.ProductoID, ap.TanqueID, Nombre = p.Nombre + " => " + tn.Nombre } into gr
                                               select new Parametros.ListIdTidDisplayValue
                                               {
                                                   ID = gr.Key.ProductoID,
                                                   TID = gr.Key.TanqueID,
                                                   Display = gr.Key.Nombre,
                                                   Value = gr.Sum(s => s.Valor)
                                               }).ToList();

                            var menos = (from k in db.Kardexes
                                         join m in db.Movimientos on k.MovimientoID equals m.ID
                                         where m.ResumenDiaID.Equals(EntidadRD.ID) && !m.Anulado && m.MovimientoTipoID.Equals(19)
                                         && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion)
                                         group k by new { k.ProductoID, k.AlmacenSalidaID } into gr
                                         select new
                                         {
                                             ID = gr.Key.ProductoID,
                                             TID = gr.Key.AlmacenSalidaID,
                                             Value = gr.Sum(s => s.CantidadSalida)
                                         }).ToList();

                            lbResumen.Items.Clear();
                            listaProductoRD.ToList().ForEach(item =>
                            {
                                if (menos.Count(m => m.ID.Equals(item.ID) && m.TID.Equals(item.TID)) > 0)
                                    item.Value -= menos.First(m => m.ID.Equals(item.ID) && m.TID.Equals(item.TID)).Value;

                                lbResumen.Items.Add(item.Display + " | " + item.Value.ToString("#,0.000") + " Litros");
                                //listaProductoRD.Add( new Parametros.ListIdDisplayValue (item.ID, item.Display, item.Value));
                            });

                        }
                        else
                        {
                            Parametros.General.DialogMsg("La fecha seleccionada no tiene resumen del día.", Parametros.MsgType.warning);
                            dateFechaEntrada.EditValue = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        

        private void glkClient_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
        }

        private void chkSoloSalida_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSoloSalida.Checked)
                this.gridColumn9.Visible = false;
            else if (!chkSoloSalida.Checked)
                this.gridColumn9.Visible = true;
        }
        #endregion
    }
}