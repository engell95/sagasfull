namespace SAGAS.ActivoFijo.Forms
{
    partial class FormTipoActivo
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
            this.Codigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Nombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Descripcion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EsTangible = new DevExpress.XtraGrid.Columns.GridColumn();
            this.EsDepreciable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaGasto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaDepreciacionAcumulada = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
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
            this.Codigo,
            this.Nombre,
            this.Descripcion,
            this.EsTangible,
            this.EsDepreciable,
            this.CuentaActivo,
            this.CuentaGasto,
            this.CuentaDepreciacionAcumulada,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.Caption = "colID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // Codigo
            // 
            this.Codigo.AppearanceCell.Options.UseTextOptions = true;
            this.Codigo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.Codigo.Caption = "Codigo";
            this.Codigo.DisplayFormat.FormatString = "{0:00}";
            this.Codigo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.Codigo.FieldName = "Codigo";
            this.Codigo.Name = "Codigo";
            this.Codigo.Visible = true;
            this.Codigo.VisibleIndex = 0;
            // 
            // Nombre
            // 
            this.Nombre.Caption = "Nombre";
            this.Nombre.FieldName = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.Visible = true;
            this.Nombre.VisibleIndex = 1;
            // 
            // Descripcion
            // 
            this.Descripcion.Caption = "Descripcion";
            this.Descripcion.FieldName = "Descripcion";
            this.Descripcion.Name = "Descripcion";
            this.Descripcion.Visible = true;
            this.Descripcion.VisibleIndex = 2;
            // 
            // EsTangible
            // 
            this.EsTangible.Caption = "Tangible";
            this.EsTangible.FieldName = "EsTangible";
            this.EsTangible.Name = "EsTangible";
            this.EsTangible.Visible = true;
            this.EsTangible.VisibleIndex = 3;
            // 
            // EsDepreciable
            // 
            this.EsDepreciable.Caption = "Depreciable";
            this.EsDepreciable.FieldName = "EsDepreciable";
            this.EsDepreciable.Name = "EsDepreciable";
            this.EsDepreciable.Visible = true;
            this.EsDepreciable.VisibleIndex = 4;
            // 
            // CuentaActivo
            // 
            this.CuentaActivo.Caption = "Cuenta Activo";
            this.CuentaActivo.FieldName = "CuentaActivo";
            this.CuentaActivo.Name = "CuentaActivo";
            this.CuentaActivo.Visible = true;
            this.CuentaActivo.VisibleIndex = 5;
            // 
            // CuentaGasto
            // 
            this.CuentaGasto.Caption = "Cuenta Gasto";
            this.CuentaGasto.FieldName = "CuentaGasto";
            this.CuentaGasto.Name = "CuentaGasto";
            this.CuentaGasto.Visible = true;
            this.CuentaGasto.VisibleIndex = 6;
            // 
            // CuentaDepreciacionAcumulada
            // 
            this.CuentaDepreciacionAcumulada.Caption = "Cuenta Depreciacion Acumulado";
            this.CuentaDepreciacionAcumulada.FieldName = "CuentaDepreciacionAcumulada";
            this.CuentaDepreciacionAcumulada.Name = "CuentaDepreciacionAcumulada";
            this.CuentaDepreciacionAcumulada.Visible = true;
            this.CuentaDepreciacionAcumulada.VisibleIndex = 7;
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
            this.colActivo.VisibleIndex = 8;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // FormTipoActivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 361);
            this.Controls.Add(this.grid);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTipoActivo";
            this.Text = "Tipos de Activos";
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
        private DevExpress.XtraGrid.Columns.GridColumn Codigo;
        private DevExpress.XtraGrid.Columns.GridColumn Nombre;
        private DevExpress.XtraGrid.Columns.GridColumn Descripcion;
        private DevExpress.XtraGrid.Columns.GridColumn EsTangible;
        private DevExpress.XtraGrid.Columns.GridColumn EsDepreciable;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaActivo;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaGasto;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaDepreciacionAcumulada;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}
