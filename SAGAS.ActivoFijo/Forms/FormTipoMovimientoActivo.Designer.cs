namespace SAGAS.ActivoFijo.Forms
{
    partial class FormTipoMovimientoActivo
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaDepreciacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaProveedor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaCliente = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEsAlta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEsTraslado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rchkActivo});
            this.grid.Size = new System.Drawing.Size(660, 322);
            this.grid.TabIndex = 4;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombre,
            this.colAplicaDepreciacion,
            this.colAplicaProveedor,
            this.colAplicaCliente,
            this.colEsAlta,
            this.colEsTraslado,
            this.gridColumn1,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.Caption = "colID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            // 
            // colAplicaDepreciacion
            // 
            this.colAplicaDepreciacion.Caption = "Aplica Depreciación";
            this.colAplicaDepreciacion.FieldName = "AplicaDepreciacion";
            this.colAplicaDepreciacion.Name = "colAplicaDepreciacion";
            this.colAplicaDepreciacion.Visible = true;
            this.colAplicaDepreciacion.VisibleIndex = 1;
            // 
            // colAplicaProveedor
            // 
            this.colAplicaProveedor.Caption = "Aplica Proveedor";
            this.colAplicaProveedor.FieldName = "AplicaProveedor";
            this.colAplicaProveedor.Name = "colAplicaProveedor";
            this.colAplicaProveedor.Visible = true;
            this.colAplicaProveedor.VisibleIndex = 2;
            // 
            // colAplicaCliente
            // 
            this.colAplicaCliente.Caption = "Aplica Cliente";
            this.colAplicaCliente.Name = "colAplicaCliente";
            // 
            // colEsAlta
            // 
            this.colEsAlta.Caption = "Es Alta";
            this.colEsAlta.FieldName = "EsAlta";
            this.colEsAlta.Name = "colEsAlta";
            this.colEsAlta.Visible = true;
            this.colEsAlta.VisibleIndex = 3;
            // 
            // colEsTraslado
            // 
            this.colEsTraslado.Caption = "Es Traslado";
            this.colEsTraslado.FieldName = "EsTraslado";
            this.colEsTraslado.Name = "colEsTraslado";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Es Baja";
            this.gridColumn1.FieldName = "EsBaja";
            this.gridColumn1.Name = "gridColumn1";
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
            this.colActivo.VisibleIndex = 4;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // FormTipoMovimientoActivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 361);
            this.Controls.Add(this.grid);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTipoMovimientoActivo";
            this.Text = "Lista Movimientos Activos Fijos";
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion       

        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaCliente;
        private DevExpress.XtraGrid.Columns.GridColumn colEsAlta;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaDepreciacion;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaProveedor;
        private DevExpress.XtraGrid.Columns.GridColumn colEsTraslado;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}
