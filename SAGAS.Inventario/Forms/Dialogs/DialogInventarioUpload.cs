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
using System.Text.RegularExpressions;
using DevExpress.XtraBars;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogInventarioUpload : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormInventarioUpload MDI;
        internal Entidad.InventarioUpload EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool NextDialog = false;
        private DataTable dtInventario = new DataTable();
        internal Entidad.ConceptoContable Concepto;
        private decimal _TipoCambio;
       
        private string Referencia
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Observacion
        {
            get { return memoObservacion.Text; }
            set { memoObservacion.Text = value; }
        }

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private int IDConcepto
        {
            get { return Convert.ToInt32(lkConcepto.EditValue); }
            set { lkConcepto.EditValue = value; }
        }

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogInventarioUpload(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
                txtNumero.Properties.ReadOnly = true;
                lkArea.Properties.ReadOnly = true;
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
                Concepto = db.ConceptoContables.Single(cc => cc.ID.Equals(10));
                lkArea.Properties.DataSource = db.Areas.Where(a => a.Activo && (!a.ID.Equals(1) && !a.ID.Equals(6))).Select(s => new { s.ID, s.Nombre });
                lkConcepto.Properties.DataSource = db.ConceptoContables.Where(a => a.Activo).Select(s => new { s.ID, s.Nombre });
              
                //txtNumero.Text = "000000000";

                int number = 1;
                if (db.InventarioUpload.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.InventarioUpload.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                txtNumero.Text = number.ToString("000000000");

                //nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " +
                //                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
                            
                
                
                //-------------------------------//


                if (Editable)
                {
                    Referencia = EntidadAnterior.Numero.ToString("000000000");
                    Observacion = EntidadAnterior.Observacion;
                    IDArea = EntidadAnterior.AreaID;
                    Fecha = EntidadAnterior.FechaInventario;
                    IDConcepto = EntidadAnterior.ConceptoID;

                    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);


                    dtInventario.Columns.Add("IDP", typeof(Int32));
                    dtInventario.Columns.Add("Codigo", typeof(String));
                    dtInventario.Columns.Add("Producto", typeof(String));
                    dtInventario.Columns.Add("Unidad", typeof(String));
                    dtInventario.Columns.Add("Total", typeof(Decimal));
                    dtInventario.Columns.Add("Costo", typeof(Decimal));
                    dtInventario.Columns.Add("Precio", typeof(Decimal));

                    db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion) && a.Activo).ToList().ForEach(alma =>
                    {
                        dtInventario.Columns.Add(new DataColumn { ColumnName = alma.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });

                        var colAlma = new Parametros.MyBandColumn(alma.Nombre, alma.ID.ToString(), alma);
                        colAlma.OptionsFilter.AllowAutoFilter = false;
                        colAlma.OptionsFilter.AllowFilter = false;
                        colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();

                        //**Repositorio para digitar la lectura
                        rpLectura.AutoHeight = false;
                        rpLectura.AllowMouseWheel = false;
                        rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                        rpLectura.Name = "rpItem" + alma.Nombre;
                        rpLectura.EditFormat.FormatString = "N3";
                        rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpLectura.DisplayFormat.FormatString = "N3";
                        rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpLectura.EditMask = "N3";
                        rpLectura.Buttons.Clear();
                        this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                        colAlma.ColumnEdit = rpLectura;

                        bandAlmacenes.Columns.Add(colAlma);

                        //COLUMNA COSTOALMACEN
                        dtInventario.Columns.Add(new DataColumn { ColumnName = "T" + alma.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });

                        var colTAlma = new Parametros.MyBandColumn("Cto " + alma.Nombre, "T" + alma.ID.ToString(), alma);
                        colTAlma.OptionsFilter.AllowAutoFilter = false;
                        colTAlma.OptionsFilter.AllowFilter = false;
                        colTAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpTLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();

                        //**Repositorio para digitar la lectura
                        rpTLectura.AutoHeight = false;
                        rpTLectura.AllowMouseWheel = false;
                        rpTLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                        rpTLectura.Name = "rpItem" + alma.Nombre;
                        rpTLectura.EditFormat.FormatString = "N2";
                        rpTLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpTLectura.DisplayFormat.FormatString = "N2";
                        rpTLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpTLectura.EditMask = "N2";
                        rpTLectura.Buttons.Clear();
                        this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpTLectura });

                        colTAlma.ColumnEdit = rpTLectura;

                        bandAlmacenes.Columns.Add(colTAlma);



                        //string plus = "";
                        //if (!String.IsNullOrEmpty(colTotal.UnboundExpression))
                        //    plus = " + ";

                        //colTotal.UnboundExpression += plus + "[" + alma.ID.ToString() + "]";
                    });

                    dtInventario.Rows.Clear();

                    var query = from p in db.Productos
                                join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                                join ar in db.Areas on c.AreaID equals ar.ID
                                where ar.ID.Equals(IDArea)
                                select new
                                {
                                    IDP = p.ID,
                                    Producto = p.Codigo + " | " + p.Nombre
                                };

                    query.ToList().ForEach(obj => { dtInventario.Rows.Add(0, "", "", "", 0m, 0m, 0m); });

                    bdsDetalle.DataSource = dtInventario;

                    int i = 0;
                    ////Despues hacerlo por el DataTable para que lo devuelva ordenado
                    EntidadAnterior.InventarioUploadDetalle.GroupBy(g => g.ProductoID).ToList().ForEach(obj =>
                        {
                            DataRow row = dtInventario.Rows[i];

                            row["IDP"] = obj.Key;
                            row["Codigo"] = db.Productos.FirstOrDefault(p => p.ID.Equals(obj.Key)).Codigo;
                            row["Producto"] = db.Productos.FirstOrDefault(p => p.ID.Equals(obj.Key)).Nombre;
                            row["Unidad"] = db.UnidadMedidas.FirstOrDefault(u => u.ID.Equals(Convert.ToInt32(db.Productos.FirstOrDefault(p => p.ID.Equals(obj.Key)).UnidadMedidaID))).Nombre;
                            row["Costo"] = EntidadAnterior.InventarioUploadDetalle.FirstOrDefault(p => p.ProductoID.Equals(obj.Key)).Costo;
                            row["Precio"] = EntidadAnterior.InventarioUploadDetalle.FirstOrDefault(p => p.ProductoID.Equals(obj.Key)).Precio;
                            row["Total"] = EntidadAnterior.InventarioUploadDetalle.FirstOrDefault(p => p.ProductoID.Equals(obj.Key)).CostoTotal;

                            decimal vTotal = 0;
                            EntidadAnterior.InventarioUploadDetalle.Where(p => p.ProductoID.Equals(obj.Key)).ToList().ForEach(alma =>
                                {
                                    row[alma.AlmacenID.ToString()] = alma.Cantidad;
                                    row["T" + alma.AlmacenID.ToString()] = alma.CostoTotalAlmacen;
                                    vTotal += alma.Cantidad;
                                });

                            if (Convert.ToDecimal(row["Total"]).Equals(0))
                                row["Total"] = Decimal.Round(vTotal * Convert.ToDecimal(row["Costo"]), 2, MidpointRounding.AwayFromZero);
                            i++;
                            //bgvProductos.SetRowCellValue(bgvProductos.LocateByValue("IDP", obj.ProductoID, null), obj.AlmacenID.ToString(), obj.Cantidad);
                        });

                    bgvProductos.RefreshData();

                }
                else
                {
                    this.layoutControlGroup1.Text += " " + Parametros.General.EstacionServicioName + " | " +
                                    (IDSubEstacion.Equals(0) ? "" : db.SubEstacions.Single(s =>s.ID.Equals(IDSubEstacion)).Nombre);
                    DateTime DateServer = Convert.ToDateTime(db.GetDateServer());
                    Fecha = Convert.ToDateTime(new DateTime( DateServer.Year, DateServer.Month, 1)).AddDays(-1);

                    IDConcepto = Concepto.ID;

                    dtInventario.Columns.Add("IDP", typeof(Int32));
                    dtInventario.Columns.Add("Codigo", typeof(String));
                    dtInventario.Columns.Add("Producto", typeof(String));
                    dtInventario.Columns.Add("Unidad", typeof(String));
                    dtInventario.Columns.Add("Total", typeof(Decimal));
                    dtInventario.Columns.Add("Costo", typeof(Decimal));
                    dtInventario.Columns.Add("Precio", typeof(Decimal));

                    db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion) && a.Activo).ToList().ForEach(alma =>
                        {
                            dtInventario.Columns.Add(new DataColumn { ColumnName = alma.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });
                                                        
                            var colAlma = new Parametros.MyBandColumn(alma.Nombre, alma.ID.ToString(), alma);
                            colAlma.OptionsFilter.AllowAutoFilter = false;
                            colAlma.OptionsFilter.AllowFilter = false;
                            colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();

                            //**Repositorio para digitar la lectura
                            rpLectura.AutoHeight = false;
                            rpLectura.AllowMouseWheel = false;
                            rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                            rpLectura.Name = "rpItem" + alma.Nombre;
                            rpLectura.EditFormat.FormatString = "N3";
                            rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            rpLectura.DisplayFormat.FormatString = "N3";
                            rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            rpLectura.EditMask = "N3";
                            rpLectura.Buttons.Clear();
                            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                            colAlma.ColumnEdit = rpLectura;

                            bandAlmacenes.Columns.Add(colAlma);

                            //COLUMNA COSTOALMACEN
                            dtInventario.Columns.Add(new DataColumn { ColumnName = "T" + alma.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });

                            var colTAlma = new Parametros.MyBandColumn("Cto " + alma.Nombre, "T" + alma.ID.ToString(), alma);
                            colTAlma.OptionsFilter.AllowAutoFilter = false;
                            colTAlma.OptionsFilter.AllowFilter = false;
                            colTAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpTLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();

                            //**Repositorio para digitar la lectura
                            rpTLectura.AutoHeight = false;
                            rpTLectura.AllowMouseWheel = false;
                            rpTLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                            rpTLectura.Name = "rpItem" + alma.Nombre;
                            rpTLectura.EditFormat.FormatString = "N2";
                            rpTLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            rpTLectura.DisplayFormat.FormatString = "N2";
                            rpTLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            rpTLectura.EditMask = "N2";
                            rpTLectura.Buttons.Clear();
                            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpTLectura });

                            colTAlma.ColumnEdit = rpTLectura;

                            bandAlmacenes.Columns.Add(colTAlma);


                            //string plus = "";
                            //if (!String.IsNullOrEmpty(colTotal.UnboundExpression))
                            //    plus = " + ";

                            //colTotal.UnboundExpression += plus + "[" + alma.ID.ToString() + "]"; 
                        });

                    //dtInventario.DefaultView.Sort = "Total Desc";

                    bdsDetalle.DataSource = dtInventario;
                    
                }

                if (bgvProductos.RowCount > 0)
                    bgvProductos.FocusedRowHandle = 0;

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
            Parametros.General.ValidateEmptyStringRule(lkArea, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNumero.EditValue == null || lkArea.EditValue == null)
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

            DataRow[] allRows = dtInventario.Select("Codigo <> ''");

            foreach (DataRow det in allRows)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(det["Codigo"])))
                {
                    if (det["IDP"] != null)
                    {
                        if (Convert.ToInt32(det["IDP"]).Equals(0))
                        {
                            Parametros.General.DialogMsg("El Producto: " + Convert.ToString(det["Codigo"]) + " no esta registrado en el sistema.", Parametros.MsgType.warning);
                            return false;
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El Producto: " + Convert.ToString(det["Codigo"]) + " no esta registrado en el sistema.", Parametros.MsgType.warning);
                        return false;
                    }
                }
            }

            if (!Parametros.General.ValidateKardexMovemente(Fecha, db, IDEstacionServicio, IDSubEstacion, 9, 0))
            {
                Parametros.General.DialogMsg("El Inventario para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
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

                    Entidad.InventarioUpload INV;

                    if (Editable)
                    {
                        INV = db.InventarioUpload.Single(e => e.ID == EntidadAnterior.ID);

                        if (INV.Finalizado.Equals(true))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Este Levantamiento de Inventario ya fue Ingresado." + Environment.NewLine, Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        INV.InventarioUploadDetalle.Clear();
                    }
                    else
                    {
                        if (db.InventarioFisicos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.AreaID.Equals(IDArea) && m.FechaInventario.Date >= Fecha.Date) > 0)
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Ya existe un levantamiento de inventario físico del área seleccionada posterior a esta fecha " + Fecha.ToShortDateString() + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }

                        INV = new Entidad.InventarioUpload();
                        INV.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        int number = 1;
                        if (db.InventarioUpload.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.InventarioUpload.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        INV.Numero = number;
                    }

                    INV.EstacionServicioID = IDEstacionServicio;
                    INV.SubEstacionID = IDSubEstacion;                    
                    INV.FechaInventario = Fecha;
                    INV.Observacion = Observacion;
                    INV.ConceptoID = IDConcepto;
                    INV.AreaID = IDArea;
                    INV.UserID = UsuarioID;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(INV, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó la plantilla inicial de Inventario: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.InventarioUpload.InsertOnSubmit(INV);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró la plantilla inicial de Inventario: " + INV.Numero, this.Name);

                    }

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EXISTENCIAS EN DETALLE :::

                    DataRow[] allRows = dtInventario.Select("IDP > 0");

                   
                    foreach (DataRow row in allRows)
                    {
                        int IDProd = 0;
                        foreach (DataColumn col in row.Table.Columns)
                        {                            
                            if (col.ColumnName.Equals("IDP"))
                                IDProd = Convert.ToInt32(row["IDP"]);

                            if (col.DataType == typeof(Decimal) && (!Convert.ToString(col.ColumnName).Contains("T") && !Convert.ToString(col.ColumnName).Contains("Total") && !Convert.ToString(col.ColumnName).Contains("Costo") && !Convert.ToString(col.ColumnName).Contains("Precio")))
                            {
                                    if (!Convert.ToDecimal(row[col]).Equals(0))
                                    {
                                        INV.InventarioUploadDetalle.Add(new Entidad.InventarioUploadDetalle { ProductoID = IDProd, AlmacenID = Convert.ToInt32(col.ColumnName), Cantidad = Convert.ToDecimal(row[col]), Costo = Convert.ToDecimal(row["Costo"]), Precio = Convert.ToDecimal(row["Precio"]), CostoTotal = Convert.ToDecimal(row["Total"]), CostoTotalAlmacen = Decimal.Round(Convert.ToDecimal(row["T" + col.ColumnName ]), 2, MidpointRounding.AwayFromZero) });
                                        db.SubmitChanges();
                                    }
                              
                            }
                        }

                    }
                    #endregion

                    if (Finish)
                    {
                        INV.Finalizado = true;

                        Entidad.Movimiento M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 43;
                        M.UsuarioID = UsuarioID;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.FechaRegistro = INV.FechaInventario;
                        M.MonedaID = Parametros.Config.MonedaPrincipal();                        
                        M.TipoCambio = _TipoCambio;
                        M.Monto = INV.InventarioUploadDetalle.Sum(s => s.CostoTotal);
                        M.MontoMonedaSecundaria = Decimal.Round(Math.Abs(Convert.ToDecimal(Convert.ToDecimal(INV.InventarioUploadDetalle.Sum(s => s.CostoTotal)) / _TipoCambio)), 2, MidpointRounding.AwayFromZero);
                        M.Numero = INV.Numero;
                        M.EstacionServicioID = INV.EstacionServicioID;
                        M.SubEstacionID = INV.SubEstacionID;
                        M.Comentario = INV.Observacion;
                        M.ConceptoContableID = INV.ConceptoID;

                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Inventario Inicial: " + M.Numero, this.Name);

                        db.SubmitChanges();

                        INV.MovimientoID = M.ID;
                        
                        foreach (var dk in INV.InventarioUploadDetalle)
                        {
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.MovimientoID = M.ID;
                            KX.EstacionServicioID = M.EstacionServicioID;
                            KX.SubEstacionID = M.SubEstacionID;
                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = Producto.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.CantidadInicial = 0;
                            KX.AlmacenEntradaID = dk.AlmacenID;
                            KX.CantidadEntrada = Decimal.Round(dk.Cantidad, 3, MidpointRounding.AwayFromZero);
                            KX.CantidadFinal = Decimal.Round(KX.CantidadInicial + KX.CantidadEntrada, 3, MidpointRounding.AwayFromZero);
                            KX.CostoTotal = Decimal.Round(dk.CostoTotalAlmacen, 2, MidpointRounding.AwayFromZero);

                            decimal vCant = Decimal.Round(INV.InventarioUploadDetalle.Where(p => p.ProductoID.Equals(Producto.ID)).Sum(s => s.Cantidad), 2, MidpointRounding.AwayFromZero);
                            decimal vCosto = Decimal.Round(dk.CostoTotal / vCant, 4, MidpointRounding.AwayFromZero);
                            KX.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);                            
                            KX.CostoFinal = Decimal.Round(KX.CostoEntrada, 4, MidpointRounding.AwayFromZero);
                            
                            //ToList().ForEach(alma =>
                            //{
                            //    row[alma.AlmacenID.ToString()] = alma.Cantidad;
                            //    vTotal += alma.Cantidad;
                           
                            

                            db.Kardexes.InsertOnSubmit(KX);
                            db.SubmitChanges();

                            #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                            //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                            
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

                                    AP.PrecioVenta = Decimal.Round(dk.Precio, 6, MidpointRounding.AwayFromZero);
                                    if (!Producto.ExentoIVA)
                                        AP.PrecioTotal = Decimal.Round(dk.Precio + (dk.Precio * 0.15m), 6, MidpointRounding.AwayFromZero);
                                    else
                                        AP.PrecioTotal = Decimal.Round(dk.Precio, 6, MidpointRounding.AwayFromZero);

                                    decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                    if (!Producto.ExentoIVA)
                                        precio += Decimal.Round((KX.CostoFinal * 0.15m), 6, MidpointRounding.AwayFromZero);
                                    //-- INSERTAR REGISTRO 
                                    AP.PrecioSugerido = precio;
                                    AP.Costo = KX.CostoFinal;
                                    db.AlmacenProductos.InsertOnSubmit(AP);
                                }
                                else //-- SI HAY REGISTRO DE EXISTENCIA ACTUALIZAR CANTIDAD CON COMPRA
                                {
                                    Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(Producto.ID) && p.AlmacenID.Equals(KX.AlmacenEntradaID));
                                    AP.Cantidad = KX.CantidadFinal;// + AL.Single(q => q.ProductoID.Equals(Producto.ID)).Cantidad;
                                    AP.PrecioVenta = Decimal.Round(dk.Precio, 6, MidpointRounding.AwayFromZero);
                                    if (!Producto.ExentoIVA)
                                        AP.PrecioTotal = Decimal.Round(dk.Precio + (dk.Precio * 0.15m), 6, MidpointRounding.AwayFromZero);
                                    else
                                        AP.PrecioTotal = Decimal.Round(dk.Precio, 6, MidpointRounding.AwayFromZero);

                                    decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                    if (!Producto.ExentoIVA)
                                        precio += Decimal.Round((KX.CostoFinal * 0.15m), 6, MidpointRounding.AwayFromZero);
                                   
                                    AP.PrecioSugerido = precio;
                                    AP.Costo = KX.CostoFinal;
                                    db.SubmitChanges();
                                }
                           

                            #endregion
                            

                        }

                        decimal _Monto = INV.InventarioUploadDetalle.Select(t => new { t.ProductoID, t.CostoTotal }).Distinct().Sum(s => s.CostoTotal);

                        M.Monto = Decimal.Round(_Monto, 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(_Monto / _TipoCambio, 2, MidpointRounding.AwayFromZero);

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

                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    int i = 1;
                    decimal vTotal = 0m;
                    


                        List<Int32> IDInventario = new List<Int32>();

                    DataRow[] allRows = dtInventario.Select("IDP > 0");

                    foreach (DataRow row in allRows)
                    {
                        int IDProd = 0;
                        IDProd = Convert.ToInt32(row["IDP"]);
                        //decimal vLinea = 0m;
                        //foreach (DataColumn col in row.Table.Columns)
                        //{
                        //    if (col.ColumnName.Equals("IDP"))
                        //        IDProd = Convert.ToInt32(row["IDP"]);

                        //    if (col.DataType == typeof(Decimal) && (!Convert.ToString(col.ColumnName).Contains("Total") && !Convert.ToString(col.ColumnName).Contains("Costo") && !Convert.ToString(col.ColumnName).Contains("Precio")))
                        //    {
                        //        if (!Convert.ToDecimal(row[col]).Equals(0))
                        //        {
                        //            vLinea += Convert.ToDecimal(row[col]);
                        //        }

                        //    }
                        //}
                                    var area = from a in db.Areas
                                               join pc in db.ProductoClases on a.ID equals pc.AreaID
                                               join p in db.Productos on pc.ID equals p.ProductoClaseID
                                               where p.ID.Equals(IDProd)
                                               select a;

                                    if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                    {
                                        if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                        {
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = area.First().CuentaInventarioID,
                                                Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(row["Total"]) / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                                Fecha = Fecha,
                                                Descripcion = "Inventario Inicial",
                                                Linea = i,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                            IDInventario.Add(area.First().CuentaInventarioID);
                                            vTotal += Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero);
                                            i++;
                                        }
                                        else
                                        {
                                            var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                            comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero));
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(row["Total"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                            vTotal += Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero);
                                        }
                                    }
                                    else
                                    {
                                        var producto = db.Productos.Single(p => p.ID.Equals(IDProd));
                                        if (!IDInventario.Contains(producto.CuentaInventarioID))
                                        {
                                            CD.Add(new Entidad.ComprobanteContable
                                            {
                                                CuentaContableID = producto.CuentaInventarioID,
                                                Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero)),
                                                TipoCambio = _TipoCambio,
                                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(row["Total"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                                Fecha = Fecha,
                                                Descripcion = "Inventario Inicial",
                                                Linea = i,
                                                EstacionServicioID = IDEstacionServicio,
                                                SubEstacionID = IDSubEstacion
                                            });
                                            IDInventario.Add(producto.CuentaInventarioID);
                                            vTotal += Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero);
                                            i++;
                                        }
                                        else
                                        {
                                            var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                            comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero));
                                            comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(row["Total"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                            vTotal += Decimal.Round(Convert.ToDecimal(row["Total"]), 2, MidpointRounding.AwayFromZero);
                                        }
                                    }

                            //    }
                            //}
                        //}
                    }

                    //Concepto Contable
                    var cuenta = db.ConceptoContables.Single(c => c.ID.Equals(IDConcepto)).CuentaContableID;
                    CD.Add(new Entidad.ComprobanteContable
                    {
                        CuentaContableID = cuenta,
                        Monto = -Math.Abs(Decimal.Round(vTotal, 2, MidpointRounding.AwayFromZero)),
                        TipoCambio = _TipoCambio,
                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vTotal / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                        Fecha = Fecha,
                        Descripcion = "Inventario Inicial",
                        Linea = i,
                        EstacionServicioID = IDEstacionServicio,
                        SubEstacionID = IDSubEstacion
                    });
                    i++;  

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
            if (Parametros.General.DialogMsg("¿Desea finalizar este carga de inventario?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
            {
                if (!Guardar(true)) return;
                this.Close();
            }
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, NextDialog);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (lkArea.EditValue != null)
                {
                    if (Parametros.General.DialogMsg("El levantamiento de inventario actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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
                        + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                                               
                    }
                }
            }
        } 

        private void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkArea.EditValue != null)
                {
                    if (!IDArea.Equals(0) && !Editable)
                    {
                        dtInventario.Rows.Clear();

                        var query = from p in db.Productos
                                                join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                                                join ar in db.Areas on c.AreaID equals ar.ID
                                                where ar.ID.Equals(IDArea)
                                                select new
                                                {
                                                    IDP = p.ID,
                                                    Producto = p.Codigo + " | " + p.Nombre
                                                };

                        query.ToList().ForEach( obj => {dtInventario.Rows.Add(0, "","", "", 0m, 0m, 0m);});

                        bgvProductos.RefreshData();

                        if (bgvProductos.RowCount > 0)
                            bgvProductos.FocusedRowHandle = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void lkArea_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //if (Convert.ToDecimal(bgvProductos.Columns["Total"]. .SummaryItem.SummaryValue) > 0)
            //{
            //    Parametros.General.DialogMsg("El Inventario físico tiene detalle de productos digitados.", Parametros.MsgType.warning);
            //    e.Cancel = true;
            //}
        }

        private void bgvProductos_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            ShowPupUpMenu();
        }

        private void menuButtonCopiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.bgvProductos.CopyToClipboard();
        }

        private void menuButtonPegar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var filas = bgvProductos.GetSelectedRows();
                string lista = Clipboard.GetText();

                List<String> datos = new List<String>();
                string[] lineas = Regex.Split(lista, "\r\n");


                int i = 0;
                foreach (int obj in filas)
                {
                    int j = 0;
                    foreach (string item in lineas.ElementAt(i).Split('\t'))
                    {

                        bgvProductos.SetRowCellValue(obj, bgvProductos.GetSelectedCells(obj).ElementAt(j), item.ToString());
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
                var Prod = db.Productos.Where(p => p.Activo && !p.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())).Select(s => s.Codigo);

                foreach (DataRow det in dtInventario.Rows)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(det["Codigo"])))
                    {
                        if (Prod.Contains(Convert.ToString(det["Codigo"])))
                        {
                            var P = db.Productos.FirstOrDefault(p => p.Codigo.Equals(Convert.ToString(det["Codigo"]))).ID;
                            det["IDP"] = P;
                        }
                        else
                            Parametros.General.DialogMsg("No se encuentra registrado el codigo " + Convert.ToString(det["Codigo"]), Parametros.MsgType.warning);

                        decimal vCosto = Convert.ToDecimal(det["Total"]);
                        List<Parametros.ListDisplayValue> Columnas = new List<Parametros.ListDisplayValue>();

                        foreach (DataColumn col in det.Table.Columns)
                        {
                            if (col.DataType == typeof(Decimal) && (!Convert.ToString(col.ColumnName).Contains("T") && !Convert.ToString(col.ColumnName).Contains("Total") && !Convert.ToString(col.ColumnName).Contains("Costo") && !Convert.ToString(col.ColumnName).Contains("Precio")))
                            {
                                if (!Convert.ToDecimal(det[col]).Equals(0))
                                {
                                    Columnas.Add(new Parametros.ListDisplayValue( "T" + col.ColumnName, Convert.ToDecimal(det[col])));
                                }
                            }
                        }

                        if (Columnas.Count > 0)
                        {
                            if (Columnas.Count.Equals(1))
                            {
                                det[Columnas.ElementAt(0).Display] = vCosto;
                            }
                            else
                            {
                                decimal vCtoUnit = 0m;
                                decimal vTotalCant = 0m;

                                vTotalCant = Decimal.Round(Convert.ToDecimal(Columnas.Sum(s => s.Value)), 2, MidpointRounding.AwayFromZero);
                                vCtoUnit = Decimal.Round(Convert.ToDecimal(vCosto / vTotalCant), 4, MidpointRounding.AwayFromZero);

                                for (int i = 0; i < Columnas.Count; i++)
                                {
                                    int ult = Columnas.Count - 1;

                                    if (i.Equals(ult))
                                    {
                                        det[Columnas.ElementAt(i).Display] = vCosto;
                                    }
                                    else
                                    {
                                        decimal CtoAlmacen = Decimal.Round(Columnas.ElementAt(i).Value * vCtoUnit, 2, MidpointRounding.AwayFromZero);
                                        det[Columnas.ElementAt(i).Display] = CtoAlmacen;
                                        vCosto -= CtoAlmacen;
                                    }
                                }
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
                            + view.GetRowCellDisplayText(RowHandle, "ProductoID").ToString(), Parametros.MsgType.question) == DialogResult.OK)
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
                if (Parametros.General.ValidateKardexMovemente(Fecha, db, IDEstacionServicio, IDSubEstacion, 9, 0))
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
                else
                {
                    DateTime fecha = Convert.ToDateTime(dateFecha.EditValue);
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGINVENTARIOBLOQUEADO + fecha.ToShortDateString(), Parametros.MsgType.warning);
                    dateFecha.EditValue = null;
                }
            }
        }

        #endregion

       

    }
}