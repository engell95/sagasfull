namespace SAGAS.Reportes.Arqueos.Forms
{
    partial class FormRptFormasPago
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRptFormasPago));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResumenDiaNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResumenDiaFechaInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTurno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colArqueoNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIslaNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTecnicoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPagoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpIslas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpTecnico = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpTipo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.linqInstantFeedbackSource1 = new DevExpress.Data.Linq.LinqInstantFeedbackSource();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTipo)).BeginInit();
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
            this.grid.EmbeddedNavigator.TextStringFormat = "{0:N0} de {1:N0}";
            this.grid.Location = new System.Drawing.Point(0, 33);
            this.grid.MainView = this.gvData;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpEstacionServicio,
            this.rpIslas,
            this.rpTecnico,
            this.rpTipo});
            this.grid.Size = new System.Drawing.Size(804, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colColor,
            this.colEstacionServicio,
            this.colResumenDiaNumero,
            this.colResumenDiaFechaInicial,
            this.colTurno,
            this.colArqueoNumero,
            this.colIslaNombre,
            this.colTecnicoNombre,
            this.colPagoNombre,
            this.colValor});
            this.gvData.GridControl = this.grid;
            this.gvData.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Valor", null, "Total C$ = {0:#,0.00;(#,0.00)}")});
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowFooter = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colArqueoNumero, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // colColor
            // 
            this.colColor.Caption = "Color";
            this.colColor.FieldName = "Color";
            this.colColor.Name = "colColor";
            this.colColor.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // colEstacionServicio
            // 
            this.colEstacionServicio.Caption = "Estación de Servicio";
            this.colEstacionServicio.FieldName = "EstacionNombre";
            this.colEstacionServicio.Name = "colEstacionServicio";
            this.colEstacionServicio.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colEstacionServicio.Visible = true;
            this.colEstacionServicio.VisibleIndex = 0;
            this.colEstacionServicio.Width = 152;
            // 
            // colResumenDiaNumero
            // 
            this.colResumenDiaNumero.Caption = "Nro. Resumen";
            this.colResumenDiaNumero.FieldName = "ResumenDiaNumero";
            this.colResumenDiaNumero.Name = "colResumenDiaNumero";
            this.colResumenDiaNumero.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colResumenDiaNumero.Visible = true;
            this.colResumenDiaNumero.VisibleIndex = 1;
            this.colResumenDiaNumero.Width = 129;
            // 
            // colResumenDiaFechaInicial
            // 
            this.colResumenDiaFechaInicial.Caption = "Fecha";
            this.colResumenDiaFechaInicial.FieldName = "ResumenDiaFechaInicial";
            this.colResumenDiaFechaInicial.Name = "colResumenDiaFechaInicial";
            this.colResumenDiaFechaInicial.Visible = true;
            this.colResumenDiaFechaInicial.VisibleIndex = 2;
            this.colResumenDiaFechaInicial.Width = 126;
            // 
            // colTurno
            // 
            this.colTurno.Caption = "Turno";
            this.colTurno.FieldName = "TurnoNumero";
            this.colTurno.Name = "colTurno";
            this.colTurno.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colTurno.Visible = true;
            this.colTurno.VisibleIndex = 3;
            this.colTurno.Width = 102;
            // 
            // colArqueoNumero
            // 
            this.colArqueoNumero.Caption = "Nro. Arqueo";
            this.colArqueoNumero.FieldName = "ArqueoNumero";
            this.colArqueoNumero.Name = "colArqueoNumero";
            this.colArqueoNumero.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colArqueoNumero.Visible = true;
            this.colArqueoNumero.VisibleIndex = 4;
            this.colArqueoNumero.Width = 116;
            // 
            // colIslaNombre
            // 
            this.colIslaNombre.Caption = "Isla";
            this.colIslaNombre.FieldName = "IslaNombre";
            this.colIslaNombre.Name = "colIslaNombre";
            this.colIslaNombre.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colIslaNombre.Visible = true;
            this.colIslaNombre.VisibleIndex = 5;
            this.colIslaNombre.Width = 63;
            // 
            // colTecnicoNombre
            // 
            this.colTecnicoNombre.Caption = "Técnico";
            this.colTecnicoNombre.FieldName = "TecnicoNombre";
            this.colTecnicoNombre.Name = "colTecnicoNombre";
            this.colTecnicoNombre.Visible = true;
            this.colTecnicoNombre.VisibleIndex = 6;
            this.colTecnicoNombre.Width = 122;
            // 
            // colPagoNombre
            // 
            this.colPagoNombre.Caption = "Tipo de Pago";
            this.colPagoNombre.FieldName = "PagoNombre";
            this.colPagoNombre.Name = "colPagoNombre";
            this.colPagoNombre.Visible = true;
            this.colPagoNombre.VisibleIndex = 7;
            this.colPagoNombre.Width = 96;
            // 
            // colValor
            // 
            this.colValor.Caption = "Valor";
            this.colValor.DisplayFormat.FormatString = "n2";
            this.colValor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colValor.FieldName = "Valor";
            this.colValor.GroupFormat.FormatString = "n2";
            this.colValor.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colValor.Name = "colValor";
            this.colValor.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Valor", "C$ = {0:#,0.00;(#,0.00)}")});
            this.colValor.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colValor.Visible = true;
            this.colValor.VisibleIndex = 8;
            this.colValor.Width = 109;
            // 
            // rpEstacionServicio
            // 
            this.rpEstacionServicio.AutoHeight = false;
            this.rpEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio")});
            this.rpEstacionServicio.Name = "rpEstacionServicio";
            // 
            // rpIslas
            // 
            this.rpIslas.AutoHeight = false;
            this.rpIslas.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpIslas.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Islas")});
            this.rpIslas.Name = "rpIslas";
            // 
            // rpTecnico
            // 
            this.rpTecnico.AutoHeight = false;
            this.rpTecnico.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpTecnico.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Técnicos")});
            this.rpTecnico.Name = "rpTecnico";
            // 
            // rpTipo
            // 
            this.rpTipo.AutoHeight = false;
            this.rpTipo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpTipo.Name = "rpTipo";
            // 
            // linqInstantFeedbackSource1
            // 
            this.linqInstantFeedbackSource1.AreSourceRowsThreadSafe = true;
            // 
            // FormRptFormasPago
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormRptFormasPago";
            this.Text = "Reporte Formas de Pago";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTipo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colIslaNombre;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpIslas;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTecnico;
        private DevExpress.XtraGrid.Columns.GridColumn colArqueoNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colTurno;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTipo;
        private DevExpress.XtraGrid.Columns.GridColumn colResumenDiaNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colResumenDiaFechaInicial;
        private DevExpress.Data.Linq.LinqInstantFeedbackSource linqInstantFeedbackSource1;
        private DevExpress.XtraGrid.Columns.GridColumn colColor;
        private DevExpress.XtraGrid.Columns.GridColumn colValor;
        private DevExpress.XtraGrid.Columns.GridColumn colTecnicoNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colPagoNombre;
    }
}

