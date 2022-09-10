namespace SAGAS.Administracion.Forms
{
    partial class FormProducto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProducto));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCategoriaProductoID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrClaseProducto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colArea = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnidadMedidaID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaISC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrClaseProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            this.SuspendLayout();
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Excel";
            this.barButtonItem1.Id = 6;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "PDF";
            this.barButtonItem2.Id = 7;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrClaseProducto,
            this.lkrUnidadMedida,
            this.rchkActivo});
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombre,
            this.colCategoriaProductoID,
            this.colArea,
            this.colUnidadMedidaID,
            this.colComentario,
            this.gridColumn1,
            this.gridColumn2,
            this.colAplicaISC,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCodigo, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Código";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            this.colCodigo.Width = 96;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Producto";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 1;
            this.colNombre.Width = 167;
            // 
            // colCategoriaProductoID
            // 
            this.colCategoriaProductoID.Caption = "Clase";
            this.colCategoriaProductoID.ColumnEdit = this.lkrClaseProducto;
            this.colCategoriaProductoID.FieldName = "ProductoClaseID";
            this.colCategoriaProductoID.Name = "colCategoriaProductoID";
            this.colCategoriaProductoID.Visible = true;
            this.colCategoriaProductoID.VisibleIndex = 2;
            this.colCategoriaProductoID.Width = 119;
            // 
            // lkrClaseProducto
            // 
            this.lkrClaseProducto.AutoHeight = false;
            this.lkrClaseProducto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrClaseProducto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Clases")});
            this.lkrClaseProducto.Name = "lkrClaseProducto";
            this.lkrClaseProducto.NullText = "<No tiene Clase de Productos Asignado>";
            // 
            // colArea
            // 
            this.colArea.Caption = "Area";
            this.colArea.FieldName = "Area";
            this.colArea.Name = "colArea";
            this.colArea.Visible = true;
            this.colArea.VisibleIndex = 3;
            this.colArea.Width = 106;
            // 
            // colUnidadMedidaID
            // 
            this.colUnidadMedidaID.Caption = "Unidad de Medida";
            this.colUnidadMedidaID.ColumnEdit = this.lkrUnidadMedida;
            this.colUnidadMedidaID.FieldName = "UnidadMedidaID";
            this.colUnidadMedidaID.Name = "colUnidadMedidaID";
            this.colUnidadMedidaID.Visible = true;
            this.colUnidadMedidaID.VisibleIndex = 4;
            this.colUnidadMedidaID.Width = 101;
            // 
            // lkrUnidadMedida
            // 
            this.lkrUnidadMedida.AutoHeight = false;
            this.lkrUnidadMedida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrUnidadMedida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Unidades de Medida")});
            this.lkrUnidadMedida.Name = "lkrUnidadMedida";
            this.lkrUnidadMedida.NullText = "<No tiene Unidad de Medida Asignado>";
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 14;
            this.colComentario.Width = 401;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Es Servicio?";
            this.gridColumn1.FieldName = "EsServicio";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Es Excento IVA?";
            this.gridColumn2.FieldName = "ExentoIVA";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 6;
            this.gridColumn2.Width = 95;
            // 
            // colAplicaISC
            // 
            this.colAplicaISC.Caption = "Aplica ISC?";
            this.colAplicaISC.FieldName = "AplicaISC";
            this.colAplicaISC.Name = "colAplicaISC";
            this.colAplicaISC.Visible = true;
            this.colAplicaISC.VisibleIndex = 7;
            this.colAplicaISC.Width = 66;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Cuenta Venta";
            this.gridColumn3.FieldName = "CuentaVenta";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 8;
            this.gridColumn3.Width = 133;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Cuenta Inventario";
            this.gridColumn4.FieldName = "CuentaInventario";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 9;
            this.gridColumn4.Width = 133;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Cuenta Costo";
            this.gridColumn5.FieldName = "CuentaCosto";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 10;
            this.gridColumn5.Width = 126;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Cuenta Descuento";
            this.gridColumn6.FieldName = "CuentaDescuento";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 11;
            this.gridColumn6.Width = 128;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Cuenta Sobrante";
            this.gridColumn7.FieldName = "CuentaSobrante";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 12;
            this.gridColumn7.Width = 147;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "CuentaFaltante";
            this.gridColumn8.FieldName = "CuentaFaltante";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 13;
            this.gridColumn8.Width = 141;
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rchkActivo;
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.OptionsColumn.AllowEdit = false;
            this.colActivo.OptionsColumn.ReadOnly = true;
            this.colActivo.Visible = true;
            this.colActivo.VisibleIndex = 15;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // FormProducto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormProducto";
            this.Text = "Lista de Productos";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrClaseProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colComentario;
        private DevExpress.XtraGrid.Columns.GridColumn colCategoriaProductoID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrClaseProducto;
        private DevExpress.XtraGrid.Columns.GridColumn colUnidadMedidaID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrUnidadMedida;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaISC;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colArea;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}

