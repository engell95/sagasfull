﻿namespace SAGAS.Tesoreria.Forms
{
    partial class FormCajaChica
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCajaChica));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.linqInstantFeedbackSource1 = new DevExpress.Data.Linq.LinqInstantFeedbackSource();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProveedor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaCreado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAnulado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoMovimiento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkMes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicioID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpIslas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpTecnico = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpEstado = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitContainerControlMain = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControlMain = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).BeginInit();
            this.splitContainerControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // barTop
            // 
            this.barTop.OptionsBar.AllowQuickCustomization = false;
            this.barTop.OptionsBar.DrawDragBorder = false;
            this.barTop.OptionsBar.UseWholeRow = true;
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
            this.grid.DataSource = this.linqInstantFeedbackSource1;
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
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpEstacionServicio,
            this.rpIslas,
            this.rpTecnico,
            this.rpEstado,
            this.lkSES,
            this.lkMes});
            this.grid.Size = new System.Drawing.Size(532, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // linqInstantFeedbackSource1
            // 
            this.linqInstantFeedbackSource1.AreSourceRowsThreadSafe = true;
            this.linqInstantFeedbackSource1.GetQueryable += new System.EventHandler<DevExpress.Data.Linq.GetQueryableEventArgs>(this.linqInstantFeedbackSource1_GetQueryable);
            this.linqInstantFeedbackSource1.DismissQueryable += new System.EventHandler<DevExpress.Data.Linq.GetQueryableEventArgs>(this.linqInstantFeedbackSource1_DismissQueryable);
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.colID,
            this.colEstacionServicio,
            this.colSubEstacion,
            this.colProveedor,
            this.colFechaCreado,
            this.colMonto,
            this.colNumero,
            this.gridColumn1,
            this.colAnulado,
            this.colTipoMovimiento,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.colEstacionServicioID});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsDetail.AllowZoomDetail = false;
            this.gvData.OptionsDetail.EnableMasterViewMode = false;
            this.gvData.OptionsDetail.ShowDetailTabs = false;
            this.gvData.OptionsDetail.SmartDetailExpand = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowFooter = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFechaCreado, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Movimiento";
            this.gridColumn2.FieldName = "MovimientoTipoNombre";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 83;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colEstacionServicio
            // 
            this.colEstacionServicio.Caption = "Estación de Servicio";
            this.colEstacionServicio.FieldName = "EstacionNombre";
            this.colEstacionServicio.Name = "colEstacionServicio";
            this.colEstacionServicio.Visible = true;
            this.colEstacionServicio.VisibleIndex = 0;
            this.colEstacionServicio.Width = 166;
            // 
            // colSubEstacion
            // 
            this.colSubEstacion.Caption = "Sub Estación";
            this.colSubEstacion.FieldName = "SubEstacionNombre";
            this.colSubEstacion.Name = "colSubEstacion";
            this.colSubEstacion.Visible = true;
            this.colSubEstacion.VisibleIndex = 1;
            this.colSubEstacion.Width = 91;
            // 
            // colProveedor
            // 
            this.colProveedor.Caption = "Proveedor / Empleado";
            this.colProveedor.FieldName = "Beneficiario";
            this.colProveedor.Name = "colProveedor";
            this.colProveedor.Visible = true;
            this.colProveedor.VisibleIndex = 11;
            this.colProveedor.Width = 217;
            // 
            // colFechaCreado
            // 
            this.colFechaCreado.Caption = "Fecha";
            this.colFechaCreado.FieldName = "FechaContabilizacion";
            this.colFechaCreado.Name = "colFechaCreado";
            this.colFechaCreado.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.colFechaCreado.Visible = true;
            this.colFechaCreado.VisibleIndex = 9;
            this.colFechaCreado.Width = 96;
            // 
            // colMonto
            // 
            this.colMonto.Caption = "Total Doc.";
            this.colMonto.DisplayFormat.FormatString = "n2";
            this.colMonto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMonto.FieldName = "Monto";
            this.colMonto.Name = "colMonto";
            this.colMonto.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Monto", "{0:N2}")});
            this.colMonto.Visible = true;
            this.colMonto.VisibleIndex = 13;
            this.colMonto.Width = 89;
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Fact. Compra";
            this.colNumero.FieldName = "Factura";
            this.colNumero.Name = "colNumero";
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 5;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Comentario";
            this.gridColumn1.FieldName = "Comentario";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 14;
            this.gridColumn1.Width = 95;
            // 
            // colAnulado
            // 
            this.colAnulado.Caption = "Anulado";
            this.colAnulado.FieldName = "Anulado";
            this.colAnulado.Name = "colAnulado";
            this.colAnulado.Visible = true;
            this.colAnulado.VisibleIndex = 16;
            this.colAnulado.Width = 162;
            // 
            // colTipoMovimiento
            // 
            this.colTipoMovimiento.FieldName = "MovimientoTipoID";
            this.colTipoMovimiento.Name = "colTipoMovimiento";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Año";
            this.gridColumn3.FieldName = "Year";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 7;
            this.gridColumn3.Width = 46;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Mes";
            this.gridColumn4.ColumnEdit = this.lkMes;
            this.gridColumn4.FieldName = "Mes";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 8;
            // 
            // lkMes
            // 
            this.lkMes.AutoHeight = false;
            this.lkMes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMes.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Mes")});
            this.lkMes.DisplayMember = "Name";
            this.lkMes.Name = "lkMes";
            this.lkMes.NullText = "<N/A>";
            this.lkMes.ValueMember = "ID";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Responsable";
            this.gridColumn5.FieldName = "Responsable";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 106;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Entregado";
            this.gridColumn6.FieldName = "Entregado";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 10;
            this.gridColumn6.Width = 91;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Número";
            this.gridColumn7.DisplayFormat.FormatString = "000000000";
            this.gridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumn7.FieldName = "Numero";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Reembolsado";
            this.gridColumn8.FieldName = "Pagado";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 15;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Referencia";
            this.gridColumn9.FieldName = "Referencia";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 6;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Ruc Proveedor";
            this.gridColumn10.FieldName = "RucProveedor";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 12;
            this.gridColumn10.Width = 100;
            // 
            // colEstacionServicioID
            // 
            this.colEstacionServicioID.FieldName = "EstacionServicioID";
            this.colEstacionServicioID.Name = "colEstacionServicioID";
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
            // rpEstado
            // 
            this.rpEstado.AutoHeight = false;
            this.rpEstado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstado.Name = "rpEstado";
            // 
            // lkSES
            // 
            this.lkSES.AutoHeight = false;
            this.lkSES.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSES.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Sub Estación")});
            this.lkSES.Name = "lkSES";
            // 
            // splitContainerControlMain
            // 
            this.splitContainerControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlMain.Location = new System.Drawing.Point(0, 33);
            this.splitContainerControlMain.Name = "splitContainerControlMain";
            this.splitContainerControlMain.Panel1.Controls.Add(this.grid);
            this.splitContainerControlMain.Panel1.Text = "Panel1";
            this.splitContainerControlMain.Panel2.Controls.Add(this.xtraTabControlMain);
            this.splitContainerControlMain.Panel2.Text = "Panel2";
            this.splitContainerControlMain.Size = new System.Drawing.Size(804, 521);
            this.splitContainerControlMain.SplitterPosition = 532;
            this.splitContainerControlMain.TabIndex = 6;
            this.splitContainerControlMain.Text = "splitContainerControl1";
            // 
            // xtraTabControlMain
            // 
            this.xtraTabControlMain.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlMain.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlMain.Name = "xtraTabControlMain";
            this.xtraTabControlMain.Size = new System.Drawing.Size(268, 521);
            this.xtraTabControlMain.TabIndex = 2;
            this.xtraTabControlMain.CloseButtonClick += new System.EventHandler(this.xtraTabControlMain_CloseButtonClick);
            // 
            // FormCajaChica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormCajaChica";
            this.Text = "Lista de Caja Chica";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControlMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).EndInit();
            this.splitContainerControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colMonto;
        private DevExpress.XtraGrid.Columns.GridColumn colAnulado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpIslas;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTecnico;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colProveedor;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaCreado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstado;
        private DevExpress.XtraGrid.Columns.GridColumn colSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoMovimiento;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.Data.Linq.LinqInstantFeedbackSource linqInstantFeedbackSource1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMes;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlMain;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlMain;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicioID;
    }
}
