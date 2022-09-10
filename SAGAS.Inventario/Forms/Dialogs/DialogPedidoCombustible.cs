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
using DevExpress.XtraPivotGrid.ViewInfo;
using DevExpress.XtraPrinting.Native;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogPedidoCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormPedidoCombustible MDI;
        internal Entidad.Pedido EntidadAnterior;
        private DataTable dtPrecioCombustible;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;
        private bool NextOrden = false;
        private bool RefreshMDI = false;
        private bool _Guardado = false;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandProducto;
        //private decimal _MargenCosto = Parametros.Config.MargenToleranciaCosto();

        public DataTable PrecioCombustibleDT
        {
            get
            {
                return dtPrecioCombustible;
            }
            set
            {
                dtPrecioCombustible = value;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogPedidoCombustible(int UserID, bool _editando)
        {
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
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

                //Proveedor
                lkProveedor.Properties.DataSource = (from p in db.Proveedors where p.Activo select new { p.ID, p.Codigo, p.Nombre, Display = p.Codigo + " | " + p.Nombre }).OrderBy(o => o.Codigo);

                //Estación Servicio
                lkEstacion.Properties.DataSource = (from p in db.EstacionServicios where p.Activo && p.ID.Equals(Parametros.General.EstacionServicioID) select new { ID = p.ID, Display = p.Nombre });
                lkEstacion.ItemIndex = 1;

                //Terminales
                List<Parametros.ListIdDisplay> Term = (from p in db.Terminals select new Parametros.ListIdDisplay { ID = p.ID, Display = p.Nombre }).ToList();
                Term.Add(new Parametros.ListIdDisplay { ID = 0, Display = "<N/A>" });
                rpLkTerminal.DataSource = Term;

                //ENTREGAR----
                rpCboEntrega.Items.AddRange(Parametros.Config.ListaEntregar());

                //Turnos----
                rpCboTurno.Items.AddRange(Parametros.Config.ListaTurno());

                dtPrecioCombustible = new DataTable();
                bandProducto = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();

                this.bgView.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandProducto});

                dtPrecioCombustible.Columns.Add("ID", typeof(Int32));
                dtPrecioCombustible.Columns.Add("TerminalID", typeof(Int32));
                dtPrecioCombustible.Columns.Add("Entrega", typeof(String));
                dtPrecioCombustible.Columns.Add("Observacion", typeof(String));
                dtPrecioCombustible.Columns.Add("FechaSolicitud", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("TurnoSolicitud", typeof(String));
                dtPrecioCombustible.Columns.Add("FechaSalida", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("FechaEntrega", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("HoraEntrega", typeof(DateTime));

                this.bandProducto.Caption = "PRODUCTOS GALONES";
                this.bandProducto.MinWidth = 20;
                this.bandProducto.OptionsBand.AllowMove = false;
                this.bandProducto.OptionsBand.AllowPress = false;
                this.bandProducto.OptionsBand.ShowCaption = true;
                this.bandProducto.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.bandProducto.Width = 300;

                foreach (var item in db.OrdenCombustiblePedidos.OrderBy(o => o.Orden))
                {

                    var orden = db.Productos.Single(o => o.ID.Equals(item.ProductoID));

                    dtPrecioCombustible.Columns.Add(orden.ID.ToString(), typeof(Decimal));
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProd = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                    //**Columna
                    colProd.Caption = orden.Nombre;
                    colProd.FieldName = orden.ID.ToString();
                    colProd.Name = "col" + orden.ID.ToString();
                    colProd.Visible = true;
                    colProd.Width = 100;
                    colProd.AppearanceHeader.Options.UseForeColor = true;
                    colProd.AppearanceHeader.Options.UseBackColor = true;
                    colProd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                    colProd.AppearanceHeader.ForeColor = Color.White;
                    colProd.DisplayFormat.FormatString = "N0";
                    colProd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                    if (db.Tanques.Where(t => t.ProductoID == orden.ID).Count() > 0)
                    {
                        colProd.AppearanceHeader.BackColor = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == orden.ID).First().Color));
                        colProd.AppearanceHeader.BackColor2 = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == orden.ID).First().Color));

                    }

                    //**Repositorio
                    DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpPrecio = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                    rpPrecio.AutoHeight = false;
                    rpPrecio.AllowMouseWheel = false;
                    rpPrecio.EditFormat.FormatString = "N0";
                    rpPrecio.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    rpPrecio.Mask.EditMask = "N0";

                    rpPrecio.Buttons.Clear();
                    //    rpPrecio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                    //new DevExpress.XtraEditors.Controls.EditorButton()});
                    rpPrecio.MaxValue = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
                    rpPrecio.Name = "rpItem" + orden.ID.ToString();

                    //rpPrecio.EditValueChanged += new System.EventHandler(spinEdits_EditValueChanged);

                    this.bgView.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            colProd});

                    this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            rpPrecio});


                    bandProducto.Columns.Add(colProd);
                    colProd.ColumnEdit = rpPrecio;

                }


                dtPrecioCombustible.Columns.Add("TOTAL", typeof(Decimal));

                //**Columna
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                colTotal.Caption = "TOTAL";
                colTotal.FieldName = "TOTAL";
                colTotal.Name = "colTOTAL";
                colTotal.Visible = true;
                colTotal.Width = 100;
                colTotal.AppearanceHeader.Options.UseForeColor = true;
                colTotal.AppearanceHeader.Options.UseBackColor = true;
                colTotal.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                colTotal.AppearanceHeader.ForeColor = Color.Black;
                colTotal.DisplayFormat.FormatString = "N0";
                colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colTotal.OptionsColumn.AllowEdit = colTotal.OptionsColumn.AllowFocus = false;
                bandProducto.Columns.Add(colTotal);

                if (Editable)
                {
                    txtReferencia.Text = EntidadAnterior.Numero.ToString();

                    memoComentario.Text = EntidadAnterior.Comentario;
                    dateFechaOrden.DateTime = EntidadAnterior.FechaCreado;
                    lkProveedor.EditValue = EntidadAnterior.PedidoCombustibles.First().ProveedorID;
                    chkISC.Checked = EntidadAnterior.EsIECC;

                    EntidadAnterior.PedidoCombustibles.ForEach(x =>
                   {
                       DataRow dr = dtPrecioCombustible.NewRow();

                       dr["ID"] = x.ID;
                       dr["TerminalID"] = x.TerminalID;
                       dr["Observacion"] = x.Observacion;

                       if (x.FechaSolicitud.HasValue)
                           dr["FechaSolicitud"] = x.FechaSolicitud;

                       dr["Entrega"] = x.Entrega;
                       dr["TurnoSolicitud"] = x.TurnoSolicitud;

                       if (x.FechaSalida.HasValue)
                           dr["FechaSalida"] = x.FechaSalida;

                       if (x.FechaEntrega.HasValue)
                           dr["FechaEntrega"] = x.FechaEntrega;

                       if (x.HoraEntrega.HasValue)
                           dr["HoraEntrega"] = x.HoraEntrega;

                       decimal v = 0m;

                       x.PedidoCombustibleDetalles.ForEach(p =>
                       {
                           dr[p.ProductoID.ToString()] = p.Galones;
                           v += p.Galones;
                       });

                       dr["TOTAL"] = v;

                       dtPrecioCombustible.Rows.Add(dr);
                   });

                }
                else
                {
                    int number = 1;
                    if (db.Pedidos.Count(c => c.EstacionID.Equals(Parametros.General.EstacionServicioID)) > 0)
                    {
                        number = db.Pedidos.Where( p => p.EstacionID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending( o => o.Numero).First().Numero + 1;
                    }

                    txtReferencia.Text = number.ToString();
                    dateFechaOrden.DateTime = DateTime.Now.Date;
                    lkProveedor.EditValue = Parametros.Config.ProveedorCombustibleID();
                }

                this.bdsDetalle.DataSource = PrecioCombustibleDT;

                //    IDProveedor = EntidadAnterior.ProveedorID;
                //    Referencia = EntidadAnterior.Numero;
                //    FechaOrden = Convert.ToDateTime(EntidadAnterior.Fecha);
                //    IDMonedaPrincipal = EntidadAnterior.MonedaID;
                //    _TipoCambio = EntidadAnterior.TipoCambio;
                //    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                //    IDSubEstacion = EntidadAnterior.SubEstacionID;

                //    OC = EntidadAnterior.OrdenCompraDetalles.ToList();

                //    if (!EntidadAnterior.MonedaID.Equals(Parametros.Config.MonedaPrincipal()))
                //    {
                //        DetalleOC.ForEach(lista =>
                //            {
                //                lista.Costo = Decimal.Round((lista.Costo / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                //                lista.CostoTotal = Decimal.Round((lista.CostoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                //                lista.ImpuestoTotal = Decimal.Round((lista.ImpuestoTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                //            });
                //    }

                //    this.bdsDetalle.DataSource = this.DetalleOC;
                //    gvDetalle.RefreshData();

                //    txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

                //}
                //else
                //{

                //    dateFechaOrden.EditValue = Convert.ToDateTime(db.GetDateServer());
                //    IDMonedaPrincipal = Parametros.Config.MonedaPrincipal();

                //    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaOrden.Date)) > 0 ?
                //            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaOrden.Date)).First().Valor : 0m);

                //    this.bdsDetalle.DataSource = this.DetalleOC;

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
        }

        private bool ValidarReferencia(string code)
        {
            //var result = (from i in db.Movimientos
            //              where  i.Referencia.Equals(code) && i.ProveedorID.Equals(IDProveedor) && !i.Anulado
            //              select i);

            //if (result.Count() > 0)
            //{
            //    return false;
            //}
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (txtReferencia.Text == "" || lkEstacion.EditValue == null || lkProveedor.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
                return false;
            }

            var acta = db.Movimientos.Where(o => o.MovimientoTipoID.Equals(24) && !o.Anulado && o.Finalizado).Select(s => s.FechaRegistro);

            if (acta.Count() > 0)
            {
                if (dateFechaOrden.DateTime.Date <= acta.OrderByDescending(o => o.Date).First())
                {
                    Parametros.General.DialogMsg("La fecha no puede ser menor al acta finalizada." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }
            }

            if (PrecioCombustibleDT.Rows.Count <= 0)
            {
                Parametros.General.DialogMsg("Debe de ingresar detalle." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }



            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Pedido P;

                    if (Editable)
                    {
                        P = db.Pedidos.Single(e => e.ID == EntidadAnterior.ID);
                        P.FechaModificado = Convert.ToDateTime(db.GetDateServer());
                        P.UsuarioModificado = Parametros.General.UserID;

                        P.PedidoCombustibles.ForEach(d => d.PedidoCombustibleDetalles.Clear());
                        P.PedidoCombustibles.Clear();
                    }
                    else
                    {
                        P = new Entidad.Pedido();
                        P.FechaModificado = Convert.ToDateTime(db.GetDateServer());
                        P.Usuario = Parametros.General.UserID;
                    }

                    P.EstacionID = Convert.ToInt32(lkEstacion.EditValue);
                    P.FechaCreado = dateFechaOrden.DateTime.Date;
                    P.Numero = Convert.ToInt32(txtReferencia.Text);
                    P.EsIECC = chkISC.Checked; 
                    P.Comentario = memoComentario.Text;

                    if (Editable)
                    {
                        //DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(NewOC, 1));
                        //DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        // "Se modificó la Orden de Compra: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Pedidos.InsertOnSubmit(P);
                        //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        //"Se registró la Orden de Compra: " + NewOC.Numero, this.Name);

                    }

                    db.SubmitChanges();

                    int n = 0;
                    foreach (DataRow det in PrecioCombustibleDT.Rows)
                    {
                        Entidad.PedidoCombustible PC = new Entidad.PedidoCombustible();

                        PC.ProveedorID = Convert.ToInt32(lkProveedor.EditValue);

                        PC.EstacionServicioID = Convert.ToInt32(lkEstacion.EditValue);

                        if (det["TerminalID"] != DBNull.Value)
                            PC.TerminalID = Convert.ToInt32(det["TerminalID"]);

                        PC.Observacion = Convert.ToString(det["Observacion"]);
                        PC.TurnoSolicitud = Convert.ToString(det["TurnoSolicitud"]);
                        PC.Entrega = Convert.ToString(det["Entrega"]);

                        if (det["FechaSolicitud"] != DBNull.Value)
                            PC.FechaSolicitud = Convert.ToDateTime(det["FechaSolicitud"]);

                        if (det["FechaSalida"] != DBNull.Value)
                            PC.FechaSalida = Convert.ToDateTime(det["FechaSalida"]);

                        if (det["FechaEntrega"] != DBNull.Value)
                            PC.FechaEntrega = Convert.ToDateTime(det["FechaEntrega"]);

                        if (det["HoraEntrega"] != DBNull.Value)
                            PC.HoraEntrega = Convert.ToDateTime(det["HoraEntrega"]);

                        for (int i = 0; i < bandProducto.Columns.Count; i++)
                        {
                            if (!bandProducto.Columns[i].FieldName.Equals("TOTAL"))
                            {
                                if (bgView.GetRowCellValue(n, bandProducto.Columns[i]) != DBNull.Value)
                                {
                                    if (Convert.ToDecimal(bgView.GetRowCellValue(n, bandProducto.Columns[i])) > 0)
                                    {
                                        Entidad.PedidoCombustibleDetalle PCD = new Entidad.PedidoCombustibleDetalle();
                                        PCD.ProductoID = Convert.ToInt32(bandProducto.Columns[i].FieldName);
                                        PCD.Galones = Convert.ToDecimal(bgView.GetRowCellValue(n, bandProducto.Columns[i]));
                                        PCD.Litros = Decimal.Round(PCD.Galones * 3.785m, 2, MidpointRounding.AwayFromZero);
                                        PC.PedidoCombustibleDetalles.Add(PCD);
                                    }
                                }
                            }
                        }

                            P.PedidoCombustibles.Add(PC);
                            n++;
                        
                    }

                    db.SubmitChanges();
                    //NewOC.ProveedorID = IDProveedor;
                    //NewOC.Numero = Referencia;
                    //NewOC.Fecha = FechaOrden;
                    //NewOC.UsuarioID = UsuarioID;
                    //NewOC.TipoCambio = _TipoCambio;
                    //NewOC.MonedaID = IDMonedaPrincipal;
                    //NewOC.Monto = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(txtGrandTotal.Text) :
                    //            Decimal.Round((Convert.ToDecimal(txtGrandTotal.Text) * NewOC.TipoCambio), 2, MidpointRounding.AwayFromZero));
                    //NewOC.MontoMonedaSecundaria = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round((NewOC.Monto / NewOC.TipoCambio), 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(txtGrandTotal.Text));
                    //NewOC.EstacionServicioID = IDEstacionServicio;
                    //NewOC.SubEstacionID = IDSubEstacion;

             

                    //db.SubmitChanges();

                    //#region ::: REGISTRANDO EXISTENCIAS EN DETALLE :::

                    //DetalleOC.ForEach(linea =>
                    //   {
                    //       NewOC.OrdenCompraDetalles.Add(linea);
                    //       db.SubmitChanges();
                    //   });


                    //    //------------------------------------------------------------------------//

                    //    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    NextOrden = false;
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
            //if (!_Guardado && !NextOrden)
            //{
            //    if (DetalleOC.Count > 0 || txtReferencia.Text != "" || Provee != null)
            //    {
            //        if (Parametros.General.DialogMsg("La orden de compra actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
            //        {
            //            e.Cancel = true;
            //        }
            //    }
            //}

            //Provee = null;
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
            //bool Validate = true;
            //DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            //int RowHandle = view.FocusedRowHandle;

            ////-- Validar Columna de Productos             
            //if (view.GetRowCellValue(RowHandle, "ProductoID") != null)
            //{
            //    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")) == 0)
            //    {
            //        view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
            //        e.ErrorText = "Debe Seleccionar un Producto";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
            //    e.ErrorText = "Debe Seleccionar un Producto";
            //    e.Valid = false;
            //    Validate = false;
            //}


            ////-- Validar Columna de Cantidad
            ////--
            //if (view.GetRowCellValue(RowHandle, "CantidadEntrada") != null)
            //{
            //    if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadEntrada")) <= 0.00)
            //    {

            //        view.SetColumnError(view.Columns["CantidadEntrada"], "La Cantidad debe ser mayor a cero");
            //        e.ErrorText = "La Cantidad debe ser mayor a cero";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["CantidadEntrada"], "La Cantidad debe ser mayor a cero");
            //    e.ErrorText = "La Cantidad debe ser mayor a cero";
            //    e.Valid = false;
            //    Validate = false;
            //}

            ////-- Validar Columna de Productos             
            //if (view.GetRowCellValue(RowHandle, "AlmacenEntradaID") != null)
            //{
            //    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenEntradaID")) == 0)
            //    {
            //        view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen");
            //        e.ErrorText = "Debe Seleccionar un Almacen";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["AlmacenEntradaID"], "Debe Seleccionar un Almacen");
            //    e.ErrorText = "Debe Seleccionar un Almacen";
            //    e.Valid = false;
            //    Validate = false;
            //}

            //decimal NewPrice = 0;
            //decimal OldPrice = 0;
            //bool _OutMargen = false;

            ////if (view.GetRowCellValue(RowHandle, "Costo") != null)
            ////{
            ////    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")).Equals(0))
            ////    {
            ////        NewPrice = (IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")) :
            ////            Decimal.Round((Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Costo")) * _TipoCambio), 2, MidpointRounding.AwayFromZero));
            ////    }
            ////}

            //if (view.GetRowCellValue(RowHandle, "UltimoCosto") != null)
            //{
            //    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "UltimoCosto")).Equals(0))
            //    {
            //        OldPrice = Convert.ToDecimal(view.GetRowCellValue(RowHandle, "UltimoCosto"));
            //    }
            //}

            //decimal valor = 0;
            //decimal _MargenCosto = 0;
            //if (!OldPrice.Equals(0))
            //    valor = Decimal.Round(((NewPrice / OldPrice) * 100), 4, MidpointRounding.AwayFromZero);

            //_MargenCosto = db.Productos.SingleOrDefault(o => o.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).MargenToleranciaCosto;


            //if (NewPrice > OldPrice)
            //{
            //    if ((valor - 100) > _MargenCosto)
            //        _OutMargen = true;

            //}
            //else if (NewPrice < OldPrice)
            //{
            //    if ((100 - valor) > _MargenCosto)
            //        _OutMargen = true;
            //}

            //if (_OutMargen)
            //{
            //    view.SetColumnError(view.Columns["Costo"], "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%");
            //    e.ErrorText = "El costo de compra supera el margen de tolerancia definida: " + _MargenCosto.ToString() + "%";
            //    e.Valid = false;
            //    Validate = false;
            //}

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void bgView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "1" || e.Column.FieldName == "2" || e.Column.FieldName == "3")
                {
                    decimal v1 = 0, v2 = 0, v3 = 0;

                    if (bgView.GetRowCellValue(e.RowHandle, "1") != DBNull.Value)
                       v1 = Convert.ToDecimal(bgView.GetRowCellValue(e.RowHandle, "1"));

                    if (bgView.GetRowCellValue(e.RowHandle, "2") != DBNull.Value)
                        v2 = Convert.ToDecimal(bgView.GetRowCellValue(e.RowHandle, "2"));

                    if (bgView.GetRowCellValue(e.RowHandle, "3") != DBNull.Value)
                        v3 = Convert.ToDecimal(bgView.GetRowCellValue(e.RowHandle, "3"));

                    bgView.SetRowCellValue(e.RowHandle, "TOTAL", v1 + v2 + v3);

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
            //if (ValidarCampos(true))
            //{

            //    #region <<< CALCULOS_MONTOS >>>
            //    //--  Calcular las montos de las columnas de Impuestos y Subtotal
            //    if ((e.Column == colCostoTotal) || e.Column == colQuantity)
            //    {
            //        decimal vCosto = 0, vCantidad = 1, vValorImpuesto = 0, vCostoTotal = 0, vISC = 0, vSubTotal = 0;

            //        if (gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal") != null)
            //            vCostoTotal = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CostoTotal"));
            //        if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) != Convert.ToDecimal("0.00"))
            //            vCantidad = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada"));
            //        else
            //            gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

            //        vCosto = Decimal.Round((vCostoTotal / vCantidad), 4, MidpointRounding.AwayFromZero);

            //        vValorImpuesto = Decimal.Round((vCostoTotal * 0.15m), 2, MidpointRounding.AwayFromZero);

            //        gvDetalle.SetRowCellValue(e.RowHandle, "ImpuestoTotal", vValorImpuesto);
            //        gvDetalle.SetRowCellValue(e.RowHandle, "Costo", vCosto);

            //        txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
            //    }
            //    #endregion

            //    #region <<< CAMBIO_IVA >>>

            //    if (e.Column == colTaxValue && chkIVA.Checked)
            //    {
            //        try
            //        {
            //            if (!_OldTax.Equals(0))
            //            {
            //                if (gvDetalle.GetFocusedRowCellValue(colTaxValue) != null)
            //                {
            //                    if (Math.Abs(Convert.ToDecimal(gvDetalle.GetFocusedRowCellValue(colTaxValue)) - _OldTax) > _MargenIVA)
            //                    {
            //                        Parametros.General.DialogMsg("El cambio en el IVA no puede ser mayor al margen permitido: " + _MargenIVA.ToString("#,0.00") + Environment.NewLine, Parametros.MsgType.warning);
            //                        gvDetalle.SetFocusedRowCellValue(colTaxValue, _OldTax);
            //                    }
            //                }
            //                else
            //                {

            //                    gvDetalle.SetFocusedRowCellValue(colTaxValue, _OldTax);
            //                }

            //                txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
            //            }

            //            _ChangeTax = false;
            //        }
            //        catch
            //        {
            //            _OldTax = 0;
            //            _ChangeTax = false;
            //        }
            //    }

            //    #endregion

            //    #region <<< COLUMNA_PRODUCTO >>>
            //    //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            //    if (e.Column == colProduct)
            //    {
            //        if (gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID") != null)
            //        {
            //            if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")) == 0)
            //            {
            //                return;
            //            }
            //        }

            //        try
            //        {
            //            var Producto = db.Productos.Single(p => p.ID == Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ProductoID")));

            //            //-- Unidad Principal     
            //            var Um = Producto.UnidadMedidaID;
            //            gvDetalle.SetRowCellValue(e.RowHandle, "UnidadMedidaID", Um);

            //            cboAlmacen.DataSource = lkAlmacen.Properties.DataSource;
            //            if (!Editable)
            //                gvDetalle.SetRowCellValue(e.RowHandle, "AlmacenEntradaID", IDAlmacen);

            //            ColAlmacen.OptionsColumn.ReadOnly = UnAlmacen;

            //            //////////////
            //            var KCI = (from k in db.Kardexes
            //                       join m in db.Movimientos on k.MovimientoID equals m.ID
            //                       where k.ProductoID.Equals(Producto.ID) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
            //                         && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
            //                       select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();


            //            //-- SI NO HAY REGISTRO DE EXISTENCIA / INICIO EN CERO
            //            //-- SI HAY REGISTRO DE EXISTENCIA / INDICAR COMO CANTIDAD INICIAL
            //            if (KCI.Count().Equals(0))
            //                gvDetalle.SetRowCellValue(e.RowHandle, "UltimoCosto", 0.0000m);
            //            else
            //                gvDetalle.SetRowCellValue(e.RowHandle, "UltimoCosto", KCI.First().CostoEntrada);
            //            //////////


            //            //-- Cantidad Inicial de 1
            //            if (Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadEntrada")) == Convert.ToDecimal("0.00"))
            //                gvDetalle.SetRowCellValue(e.RowHandle, "CantidadEntrada", 1);

            //        }
            //        catch (Exception ex)
            //        {
            //            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //        }
            //    }
            //    //--
            //    #endregion

            //}
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
                //DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                //int RowHandle = view.FocusedRowHandle;
                //if (RowHandle >= 0)
                //{
                //    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                //        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                //    {
                //        view.DeleteRow(view.FocusedRowHandle);

                //        txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                //        //Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["SubTotal"].SummaryText) + Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                //    }

                //}
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                view.AddNewRow();
            }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            //if (e.Button.ButtonType == NavigatorButtonType.Remove)
            //{
            //    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
            //    int RowHandle = view.FocusedRowHandle;
            //    if (RowHandle >= 0)
            //    {
            //        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
            //            + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
            //        {
            //            view.DeleteRow(view.FocusedRowHandle);

            //            txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");

            //        }
            //    }
            //}
        }

        //Validar fecha de ingreso y actualizar el tipo de cambio
        private void dateFechaIngreso_Validated(object sender, EventArgs e)
        {
            //if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            //{
            //    _TipoCambio = 0;
            //    dateFechaOrden.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            //}
            //else
            //    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaOrden.Date)) > 0 ?
            //            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaOrden.Date)).First().Valor : 0m);
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
            //if (DetalleOC.Count > 0 || dateFechaOrden.EditValue != null || lkMoneda.EditValue != null || txtReferencia.Text != "")
            //{
            //    if (Parametros.General.DialogMsg("La orden de compra actual tiene datos registrados. ¿Desea cancelar esta orden y realizar una nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
            //    {
            //        NextOrden = true;
            //        RefreshMDI = false;
            //        ShowMsg = false;
            //        this.Close();
            //    }
            //}                
        }

        private void lkAlmacen_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //if (DetalleOC.Count > 0)
            //{
            //    Parametros.General.DialogMsg("La compra tiene detalle de productos ingresados.", Parametros.MsgType.warning);
            //    e.Cancel = true;
            //}
        }

        private void chkIVA_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //if (DetalleOC.Count <= 0)
            //{
            //    Parametros.General.DialogMsg("La compra no tiene detalle de productos ingresados.", Parametros.MsgType.warning);
            //    e.Cancel = true;
            //}
        }




        private void spTax_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //try
            //{
            //    if (!_ChangeTax)
            //    {
            //        _OldTax = Convert.ToDecimal(e.OldValue);
            //        _ChangeTax = true;
            //    }
            //}
            //catch
            //{
            //    _OldTax = 0;
            //}
        }

        //Sumando los totales al cambiar de filas
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
        }

        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bgView.AddNewRow();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    //if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                    //    + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    //{
                        view.DeleteRow(view.FocusedRowHandle);

                        //txtGrandTotal.Text = Convert.ToDecimal(DetalleOC.Sum(s => s.CostoTotal) + DetalleOC.Sum(s => s.ImpuestoTotal)).ToString("#,0.00");
                        //Decimal.Round(Convert.ToDecimal(gvDetalle.Columns["SubTotal"].SummaryText) + Convert.ToDecimal(gvDetalle.Columns["ImpuestoTotal"].SummaryText), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    //}

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        
    }
}