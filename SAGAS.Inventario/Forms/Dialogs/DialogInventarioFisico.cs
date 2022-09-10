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
    public partial class DialogInventarioFisico : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormInventarioFisico MDI;
        internal Entidad.InventarioFisico EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool NextDialog = false;
        private DataTable dtInventario = new DataTable();
       
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

        private string Personal
        {
            get { return txtPersonal.Text; }
            set { txtPersonal.Text = value; }
        }

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogInventarioFisico(int UserID, bool _editando)
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

                lkArea.Properties.DataSource = db.Areas.Where(a => a.Activo && (!a.ID.Equals(1) && !a.ID.Equals(6))).Select(s => new { s.ID, s.Nombre });
                
                //txtNumero.Text = "000000000";

                int number = 1;
                if (db.InventarioFisicos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.InventarioFisicos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                txtNumero.Text = number.ToString("000000000");

                //nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " +
                //                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
                            
                
                
                //-------------------------------//


                if (Editable)
                {
                    Referencia = EntidadAnterior.Numero.ToString("000000000");
                    Observacion = EntidadAnterior.Observacion;
                    Personal = EntidadAnterior.PersonalApoyo;
                    IDArea = EntidadAnterior.AreaID;
                    Fecha = EntidadAnterior.FechaInventario;

                    dtInventario.Columns.Add("IDP", typeof(Int32));
                    dtInventario.Columns.Add("Producto", typeof(String));

                    db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion)).ToList().ForEach(alma =>
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

                        string plus = "";
                        if (!String.IsNullOrEmpty(colTotal.UnboundExpression))
                            plus = " + ";

                        colTotal.UnboundExpression += plus + "[" + alma.ID.ToString() + "]";
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

                    query.ToList().ForEach(obj => { dtInventario.Rows.Add(obj.IDP, obj.Producto); });

                    bdsDetalle.DataSource = dtInventario;

                    //Despues hacerlo por el DataTable para que lo devuelva ordenado
                    EntidadAnterior.InventarioFisicoDetalles.ToList().ForEach(obj =>
                        {
                            bgvProductos.SetRowCellValue(bgvProductos.LocateByValue("IDP", obj.ProductoID, null), obj.AlmacenID.ToString(), obj.Cantidad);
                        });

                    bgvProductos.RefreshData();

                }
                else
                {
                    this.layoutControlGroup1.Text += " " + Parametros.General.EstacionServicioName + " | " +
                                    (IDSubEstacion.Equals(0) ? "" : db.SubEstacions.Single(s =>s.ID.Equals(IDSubEstacion)).Nombre);
                    DateTime DateServer = Convert.ToDateTime(db.GetDateServer());
                    Fecha = Convert.ToDateTime(new DateTime( DateServer.Year, DateServer.Month, 1)).AddDays(-1);

                    dtInventario.Columns.Add("IDP", typeof(Int32));
                    dtInventario.Columns.Add("Producto", typeof(String));

                    db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion)).ToList().ForEach(alma =>
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

                            string plus = "";
                            if (!String.IsNullOrEmpty(colTotal.UnboundExpression))
                                plus = " + ";

                            colTotal.UnboundExpression += plus + "[" + alma.ID.ToString() + "]"; 
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

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
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



            if (Convert.ToDecimal(bgvProductos.Columns["Total"].SummaryItem.SummaryValue) <= 0)
            {
                Parametros.General.DialogMsg("El Inventario físico no tiene detalle de productos digitados.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;
            
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.InventarioFisico INV;

                    if (Editable)
                    {
                        INV = db.InventarioFisicos.Single(e => e.ID == EntidadAnterior.ID);

                        if (INV.Finalizado.Equals(true))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Este Levantamiento de Inventario ya fue finalizado." + Environment.NewLine, Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        INV.InventarioFisicoDetalles.Clear();
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

                        INV = new Entidad.InventarioFisico();
                        INV.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        int number = 1;
                        if (db.InventarioFisicos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.InventarioFisicos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        INV.Numero = number;
                    }

                    INV.EstacionServicioID = IDEstacionServicio;
                    INV.SubEstacionID = IDSubEstacion;                    
                    INV.FechaInventario = Fecha;
                    INV.Observacion = Observacion;
                    
                    INV.PersonalApoyo = Personal;
                    INV.AreaID = IDArea;
                    INV.UserID = UsuarioID;

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(INV, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó el Levantamiento de Inventario: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.InventarioFisicos.InsertOnSubmit(INV);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Levantamiento de Inventario: " + INV.Numero, this.Name);

                    }

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EXISTENCIAS EN DETALLE :::

                    foreach (DataRow row in dtInventario.Rows)
                    {
                        int IDProd = 0;
                        foreach (DataColumn col in row.Table.Columns)
                        {                            
                            if (col.ColumnName.Equals("IDP"))
                                IDProd = Convert.ToInt32(row["IDP"]);

                            if (col.DataType == typeof(Decimal))
                            {
                                if (!Convert.ToDecimal(row[col]).Equals(0))
                                {
                                    INV.InventarioFisicoDetalles.Add(new Entidad.InventarioFisicoDetalle { ProductoID = IDProd, AlmacenID = Convert.ToInt32(col.ColumnName), Cantidad = Convert.ToDecimal(row[col]) });
                                    db.SubmitChanges();
                                }
                            }
                        }

                    }
                    #endregion

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

                        query.ToList().ForEach( obj => {dtInventario.Rows.Add(obj.IDP, obj.Producto);});

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
            if (Convert.ToDecimal(bgvProductos.Columns["Total"].SummaryItem.SummaryValue) > 0)
            {
                Parametros.General.DialogMsg("El Inventario físico tiene detalle de productos digitados.", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        #endregion

        private void bgvProductos_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Tag != null)
            {
                if (e.Column.Tag.GetType() == typeof(Entidad.Almacen))
                {
                    if (e.Value == DBNull.Value)
                        bgvProductos.SetFocusedRowCellValue(e.Column, 0m);
                }
            }
        }

    }
}