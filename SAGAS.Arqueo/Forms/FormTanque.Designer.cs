namespace SAGAS.Arqueo.Forms
{
    partial class FormTanque
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTanque));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicioID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colSubEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductoID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrcolProducto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rcolColor = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.colCapacidad = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCantidad = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDiametro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLongitud = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAltura = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModelo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProveedor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerie = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoTanqueID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrTipoTanque = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemColorEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrcolProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcolColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrTipoTanque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).BeginInit();
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
            this.lkrEstacionServicio,
            this.lkrcolProducto,
            this.lkrTipoTanque,
            this.rcolColor,
            this.repositoryItemColorEdit1,
            this.lkSES,
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
            this.colEstacionServicioID,
            this.colSubEstacion,
            this.colNombre,
            this.colProductoID,
            this.colColor,
            this.colCapacidad,
            this.colCantidad,
            this.colDiametro,
            this.colLongitud,
            this.colAltura,
            this.colModelo,
            this.colProveedor,
            this.colSerie,
            this.colTipoTanqueID,
            this.colComentario,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colEstacionServicioID, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colEstacionServicioID
            // 
            this.colEstacionServicioID.Caption = "Estación de Servicio";
            this.colEstacionServicioID.ColumnEdit = this.lkrEstacionServicio;
            this.colEstacionServicioID.FieldName = "EstacionServicioID";
            this.colEstacionServicioID.Name = "colEstacionServicioID";
            this.colEstacionServicioID.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colEstacionServicioID.Visible = true;
            this.colEstacionServicioID.VisibleIndex = 0;
            this.colEstacionServicioID.Width = 161;
            // 
            // lkrEstacionServicio
            // 
            this.lkrEstacionServicio.AutoHeight = false;
            this.lkrEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estación de Servicio", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkrEstacionServicio.Name = "lkrEstacionServicio";
            this.lkrEstacionServicio.NullText = "<No tiene Estación de Servicio Asignada>";
            // 
            // colSubEstacion
            // 
            this.colSubEstacion.Caption = "Sub Estación";
            this.colSubEstacion.ColumnEdit = this.lkSES;
            this.colSubEstacion.FieldName = "SubEstacionID";
            this.colSubEstacion.Name = "colSubEstacion";
            this.colSubEstacion.Visible = true;
            this.colSubEstacion.VisibleIndex = 1;
            this.colSubEstacion.Width = 181;
            // 
            // lkSES
            // 
            this.lkSES.AutoHeight = false;
            this.lkSES.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSES.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Sub Estación", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkSES.Name = "lkSES";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Tanque";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 2;
            this.colNombre.Width = 167;
            // 
            // colProductoID
            // 
            this.colProductoID.Caption = "Combustible";
            this.colProductoID.ColumnEdit = this.lkrcolProducto;
            this.colProductoID.FieldName = "ProductoID";
            this.colProductoID.Name = "colProductoID";
            this.colProductoID.Visible = true;
            this.colProductoID.VisibleIndex = 3;
            this.colProductoID.Width = 101;
            // 
            // lkrcolProducto
            // 
            this.lkrcolProducto.AutoHeight = false;
            this.lkrcolProducto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrcolProducto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Combustible")});
            this.lkrcolProducto.Name = "lkrcolProducto";
            this.lkrcolProducto.NullText = "<No tiene Combustible Asignado>";
            // 
            // colColor
            // 
            this.colColor.Caption = "Color";
            this.colColor.ColumnEdit = this.rcolColor;
            this.colColor.FieldName = "Color";
            this.colColor.Name = "colColor";
            this.colColor.Visible = true;
            this.colColor.VisibleIndex = 4;
            // 
            // rcolColor
            // 
            this.rcolColor.AutoHeight = false;
            this.rcolColor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rcolColor.Name = "rcolColor";
            // 
            // colCapacidad
            // 
            this.colCapacidad.Caption = "Capacidad Lts.";
            this.colCapacidad.FieldName = "Capacidad";
            this.colCapacidad.Name = "colCapacidad";
            this.colCapacidad.Visible = true;
            this.colCapacidad.VisibleIndex = 5;
            // 
            // colCantidad
            // 
            this.colCantidad.Caption = "Equivalente Ltrs";
            this.colCantidad.FieldName = "Litros";
            this.colCantidad.Name = "colCantidad";
            this.colCantidad.Visible = true;
            this.colCantidad.VisibleIndex = 6;
            // 
            // colDiametro
            // 
            this.colDiametro.Caption = "Diametro";
            this.colDiametro.FieldName = "Diametro";
            this.colDiametro.Name = "colDiametro";
            this.colDiametro.Visible = true;
            this.colDiametro.VisibleIndex = 7;
            // 
            // colLongitud
            // 
            this.colLongitud.Caption = "Longitud";
            this.colLongitud.FieldName = "Longitud";
            this.colLongitud.Name = "colLongitud";
            this.colLongitud.Visible = true;
            this.colLongitud.VisibleIndex = 8;
            // 
            // colAltura
            // 
            this.colAltura.Caption = "Altura Pulgadas \"";
            this.colAltura.FieldName = "Altura";
            this.colAltura.Name = "colAltura";
            this.colAltura.Visible = true;
            this.colAltura.VisibleIndex = 9;
            // 
            // colModelo
            // 
            this.colModelo.Caption = "Marca";
            this.colModelo.FieldName = "Marca";
            this.colModelo.Name = "colModelo";
            this.colModelo.Visible = true;
            this.colModelo.VisibleIndex = 10;
            // 
            // colProveedor
            // 
            this.colProveedor.Caption = "Proveedor";
            this.colProveedor.FieldName = "Proveedor";
            this.colProveedor.Name = "colProveedor";
            this.colProveedor.Visible = true;
            this.colProveedor.VisibleIndex = 11;
            // 
            // colSerie
            // 
            this.colSerie.Caption = "Serie";
            this.colSerie.FieldName = "Serie";
            this.colSerie.Name = "colSerie";
            this.colSerie.Visible = true;
            this.colSerie.VisibleIndex = 12;
            // 
            // colTipoTanqueID
            // 
            this.colTipoTanqueID.Caption = "Tipo de Tanque";
            this.colTipoTanqueID.ColumnEdit = this.lkrTipoTanque;
            this.colTipoTanqueID.FieldName = "TipoTanqueID";
            this.colTipoTanqueID.Name = "colTipoTanqueID";
            this.colTipoTanqueID.Visible = true;
            this.colTipoTanqueID.VisibleIndex = 13;
            // 
            // lkrTipoTanque
            // 
            this.lkrTipoTanque.AutoHeight = false;
            this.lkrTipoTanque.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrTipoTanque.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipo de Tanque")});
            this.lkrTipoTanque.Name = "lkrTipoTanque";
            this.lkrTipoTanque.NullText = "<No tiene Tipo de Tanque Asignado>";
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 14;
            // 
            // repositoryItemColorEdit1
            // 
            this.repositoryItemColorEdit1.AutoHeight = false;
            this.repositoryItemColorEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
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
            // FormTanque
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTanque";
            this.Text = "Lista de Tanques";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrcolProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcolColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrTipoTanque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicioID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colProductoID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrcolProducto;
        private DevExpress.XtraGrid.Columns.GridColumn colCapacidad;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidad;
        private DevExpress.XtraGrid.Columns.GridColumn colDiametro;
        private DevExpress.XtraGrid.Columns.GridColumn colLongitud;
        private DevExpress.XtraGrid.Columns.GridColumn colModelo;
        private DevExpress.XtraGrid.Columns.GridColumn colProveedor;
        private DevExpress.XtraGrid.Columns.GridColumn colSerie;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoTanqueID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrTipoTanque;
        private DevExpress.XtraGrid.Columns.GridColumn colComentario;
        private DevExpress.XtraGrid.Columns.GridColumn colAltura;
        private DevExpress.XtraGrid.Columns.GridColumn colColor;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit rcolColor;
        private DevExpress.XtraGrid.Columns.GridColumn colSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit repositoryItemColorEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}

