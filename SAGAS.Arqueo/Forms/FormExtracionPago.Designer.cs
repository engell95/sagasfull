namespace SAGAS.Arqueo.Forms
{
    partial class FormExtracionPago
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExtracionPago));
            this.gvDataDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCuentaDet = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkCuentaDet = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOrden = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEsPago = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTieneDetalle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEquivalenteLitros = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpDeudor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpCC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkTipo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuentaDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDeudor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            this.SuspendLayout();
            // 
            // gvDataDetail
            // 
            this.gvDataDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDConcepto,
            this.colConcepto,
            this.colCuentaDet});
            this.gvDataDetail.GridControl = this.grid;
            this.gvDataDetail.Name = "gvDataDetail";
            this.gvDataDetail.OptionsBehavior.Editable = false;
            this.gvDataDetail.OptionsMenu.EnableColumnMenu = false;
            this.gvDataDetail.OptionsMenu.EnableFooterMenu = false;
            this.gvDataDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataDetail.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataDetail.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDataDetail.OptionsView.ColumnAutoWidth = false;
            this.gvDataDetail.ViewCaption = "Conceptos";
            // 
            // colIDConcepto
            // 
            this.colIDConcepto.FieldName = "IDConcepto";
            this.colIDConcepto.Name = "colIDConcepto";
            // 
            // colConcepto
            // 
            this.colConcepto.Caption = "Concepto";
            this.colConcepto.FieldName = "Concepto";
            this.colConcepto.Name = "colConcepto";
            this.colConcepto.Visible = true;
            this.colConcepto.VisibleIndex = 0;
            // 
            // colCuentaDet
            // 
            this.colCuentaDet.Caption = "Cuenta Contable";
            this.colCuentaDet.ColumnEdit = this.lkCuentaDet;
            this.colCuentaDet.FieldName = "CuentaContableID";
            this.colCuentaDet.Name = "colCuentaDet";
            this.colCuentaDet.Visible = true;
            this.colCuentaDet.VisibleIndex = 1;
            this.colCuentaDet.Width = 300;
            // 
            // lkCuentaDet
            // 
            this.lkCuentaDet.AutoHeight = false;
            this.lkCuentaDet.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCuentaDet.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Cuenta")});
            this.lkCuentaDet.DisplayMember = "Display";
            this.lkCuentaDet.Name = "lkCuentaDet";
            this.lkCuentaDet.NullText = "<N/A>";
            this.lkCuentaDet.ValueMember = "ID";
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.DataSource = this.bdsManejadorDatos;
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
            gridLevelNode1.LevelTemplate = this.gvDataDetail;
            gridLevelNode1.RelationName = "ExtracionConceptos";
            this.grid.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpDeudor,
            this.rpCC,
            this.lkCuentaDet,
            this.lkTipo,
            this.rchkActivo});
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData,
            this.gvDataDetail});
            // 
            // gvData
            // 
            this.gvData.ChildGridLevelName = "ExtracionConceptos";
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombre,
            this.colOrden,
            this.colEsPago,
            this.colTieneDetalle,
            this.colEquivalenteLitros,
            this.colComentario,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.colActivo,
            this.gridColumn5,
            this.gridColumn6});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colEsPago, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colOrden, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvData.FocusedRowLoaded += new DevExpress.XtraGrid.Views.Base.RowEventHandler(this.gvData_FocusedRowLoaded);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Tipo de Extración / Pago";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 220;
            // 
            // colOrden
            // 
            this.colOrden.Caption = "Orden Consecutivo";
            this.colOrden.FieldName = "Orden";
            this.colOrden.Name = "colOrden";
            this.colOrden.Visible = true;
            this.colOrden.VisibleIndex = 1;
            this.colOrden.Width = 124;
            // 
            // colEsPago
            // 
            this.colEsPago.Caption = "Es Forma de Pago?";
            this.colEsPago.FieldName = "EsPago";
            this.colEsPago.Name = "colEsPago";
            this.colEsPago.Visible = true;
            this.colEsPago.VisibleIndex = 2;
            this.colEsPago.Width = 178;
            // 
            // colTieneDetalle
            // 
            this.colTieneDetalle.Caption = "Tiene Detalle?";
            this.colTieneDetalle.FieldName = "TieneDetalle";
            this.colTieneDetalle.Name = "colTieneDetalle";
            this.colTieneDetalle.Visible = true;
            this.colTieneDetalle.VisibleIndex = 3;
            // 
            // colEquivalenteLitros
            // 
            this.colEquivalenteLitros.Caption = "Equi. Litros";
            this.colEquivalenteLitros.DisplayFormat.FormatString = "N3";
            this.colEquivalenteLitros.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colEquivalenteLitros.FieldName = "EquivalenteLitros";
            this.colEquivalenteLitros.Name = "colEquivalenteLitros";
            this.colEquivalenteLitros.Visible = true;
            this.colEquivalenteLitros.VisibleIndex = 4;
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 11;
            this.colComentario.Width = 592;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Es Otro?";
            this.gridColumn1.FieldName = "EsOtro";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 8;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Deudor";
            this.gridColumn2.ColumnEdit = this.rpDeudor;
            this.gridColumn2.FieldName = "DeudorID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 5;
            this.gridColumn2.Width = 151;
            // 
            // rpDeudor
            // 
            this.rpDeudor.AutoHeight = false;
            this.rpDeudor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpDeudor.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Deudor")});
            this.rpDeudor.DisplayMember = "Display";
            this.rpDeudor.Name = "rpDeudor";
            this.rpDeudor.ValueMember = "ID";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Cuenta Contable";
            this.gridColumn3.ColumnEdit = this.rpCC;
            this.gridColumn3.FieldName = "CuentaContableID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 6;
            this.gridColumn3.Width = 148;
            // 
            // rpCC
            // 
            this.rpCC.AutoHeight = false;
            this.rpCC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpCC.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuenta")});
            this.rpCC.DisplayMember = "Display";
            this.rpCC.Name = "rpCC";
            this.rpCC.ValueMember = "ID";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Tipo Comprobación";
            this.gridColumn4.ColumnEdit = this.lkTipo;
            this.gridColumn4.FieldName = "TipoComprobanteArqueo";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 7;
            // 
            // lkTipo
            // 
            this.lkTipo.AutoHeight = false;
            this.lkTipo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTipo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Tipo")});
            this.lkTipo.DisplayMember = "Name";
            this.lkTipo.Name = "lkTipo";
            this.lkTipo.NullText = "<N/A>";
            this.lkTipo.ValueMember = "ID";
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
            this.colActivo.VisibleIndex = 12;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
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
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Cupón";
            this.gridColumn5.FieldName = "EsCupon";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 9;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Cup. Especial";
            this.gridColumn6.FieldName = "EsCuponEspecial";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 10;
            // 
            // FormExtracionPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormExtracionPago";
            this.Text = "Lista Tipos de Extraciones / Pagos";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuentaDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpDeudor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipo)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn colOrden;
        private DevExpress.XtraGrid.Columns.GridColumn colEsPago;
        private DevExpress.XtraGrid.Columns.GridColumn colEquivalenteLitros;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colTieneDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colIDConcepto;
        private DevExpress.XtraGrid.Columns.GridColumn colConcepto;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpDeudor;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpCC;
        private DevExpress.XtraGrid.Columns.GridColumn colCuentaDet;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkCuentaDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTipo;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    }
}

