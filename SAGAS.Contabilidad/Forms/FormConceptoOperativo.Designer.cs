namespace SAGAS.Contabilidad.Forms
{
    partial class FormConceptoOperativo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConceptoOperativo));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpESID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkCuenta = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkMoneda = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkTipoProveedor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkDepartamento = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkRetencion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpClaseID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpESID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).BeginInit();
            this.SuspendLayout();
            // 
            // barExportar
            // 
            this.barExportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barExportar.ImageOptions.Image")));
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEstacion});
            this.gridView1.GridControl = this.grid;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colEstacion
            // 
            this.colEstacion.Caption = "Estación de Servicio";
            this.colEstacion.ColumnEdit = this.rpESID;
            this.colEstacion.FieldName = "EstacionID";
            this.colEstacion.Name = "colEstacion";
            this.colEstacion.Visible = true;
            this.colEstacion.VisibleIndex = 0;
            // 
            // rpESID
            // 
            this.rpESID.AutoHeight = false;
            this.rpESID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpESID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estación de Servicio", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.rpESID.Name = "rpESID";
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
            gridLevelNode1.LevelTemplate = this.gridView1;
            gridLevelNode1.RelationName = "CentroCostoPorEstacions";
            this.grid.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grid.Location = new System.Drawing.Point(0, 33);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkTipoProveedor,
            this.lkDepartamento,
            this.lkRetencion,
            this.rpClaseID,
            this.rpESID,
            this.lkCuenta,
            this.rchkActivo,
            this.lkMoneda});
            this.grid.Size = new System.Drawing.Size(804, 521);
            this.grid.TabIndex = 6;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData,
            this.gridView1});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombre,
            this.colCuenta,
            this.gridColumn3,
            this.colActivo,
            this.gridColumn1,
            this.gridColumn2});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Concepto";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 178;
            // 
            // colCuenta
            // 
            this.colCuenta.Caption = "Cuenta Contable";
            this.colCuenta.ColumnEdit = this.lkCuenta;
            this.colCuenta.FieldName = "CuentaContableID";
            this.colCuenta.Name = "colCuenta";
            this.colCuenta.Visible = true;
            this.colCuenta.VisibleIndex = 1;
            this.colCuenta.Width = 227;
            // 
            // lkCuenta
            // 
            this.lkCuenta.AutoHeight = false;
            this.lkCuenta.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCuenta.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuentas")});
            this.lkCuenta.DisplayMember = "Display";
            this.lkCuenta.Name = "lkCuenta";
            this.lkCuenta.ValueMember = "ID";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Aplica Pago ?";
            this.gridColumn3.FieldName = "EsPago";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
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
            this.colActivo.VisibleIndex = 5;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Moneda";
            this.gridColumn1.ColumnEdit = this.lkMoneda;
            this.gridColumn1.FieldName = "MonedaID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // lkMoneda
            // 
            this.lkMoneda.AutoHeight = false;
            this.lkMoneda.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMoneda.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Monedas")});
            this.lkMoneda.DisplayMember = "Display";
            this.lkMoneda.Name = "lkMoneda";
            this.lkMoneda.NullText = "<N/A>";
            this.lkMoneda.ValueMember = "ID";
            // 
            // lkTipoProveedor
            // 
            this.lkTipoProveedor.AutoHeight = false;
            this.lkTipoProveedor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTipoProveedor.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipo de Proveedor")});
            this.lkTipoProveedor.Name = "lkTipoProveedor";
            // 
            // lkDepartamento
            // 
            this.lkDepartamento.AutoHeight = false;
            this.lkDepartamento.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkDepartamento.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Departamentos")});
            this.lkDepartamento.Name = "lkDepartamento";
            // 
            // lkRetencion
            // 
            this.lkRetencion.AutoHeight = false;
            this.lkRetencion.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkRetencion.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Retencion")});
            this.lkRetencion.Name = "lkRetencion";
            // 
            // rpClaseID
            // 
            this.rpClaseID.AutoHeight = false;
            this.rpClaseID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpClaseID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Clase de Proveedor")});
            this.rpClaseID.Name = "rpClaseID";
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
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Aplica Diferencia?";
            this.gridColumn2.FieldName = "EsDiferencia";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 101;
            // 
            // FormConceptoOperativo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormConceptoOperativo";
            this.Text = "Conceptos Operativos";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpESID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colCuenta;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTipoProveedor;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkDepartamento;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkRetencion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpClaseID;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpESID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMoneda;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}

