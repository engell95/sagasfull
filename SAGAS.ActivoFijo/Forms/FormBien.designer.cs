﻿namespace SAGAS.ActivoFijo.Forms
{
    partial class FormBien
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
            this.splitCentral = new DevExpress.XtraEditors.SplitContainerControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.linqInstantFeedbackSource1 = new DevExpress.Data.Linq.LinqInstantFeedbackSource();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoMovimientoActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIDMovimiento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEsDepreciado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoMovimiento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpIslas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpTecnico = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpEstado = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkMes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkMoneda = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCentral)).BeginInit();
            this.splitCentral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).BeginInit();
            this.SuspendLayout();
            // 
            // splitCentral
            // 
            this.splitCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCentral.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitCentral.Location = new System.Drawing.Point(0, 39);
            this.splitCentral.Name = "splitCentral";
            this.splitCentral.Panel1.Controls.Add(this.grid);
            this.splitCentral.Panel1.Text = "Panel1";
            this.splitCentral.Panel2.Text = "Panel2";
            this.splitCentral.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitCentral.Size = new System.Drawing.Size(660, 322);
            this.splitCentral.SplitterPosition = 428;
            this.splitCentral.TabIndex = 7;
            this.splitCentral.Text = "splitContainerControl1";
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
            this.lkMes,
            this.lkMoneda});
            this.grid.Size = new System.Drawing.Size(660, 322);
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
            this.colID,
            this.gridColumn1,
            this.colSUS,
            this.gridColumn2,
            this.colNumero,
            this.gridColumn4,
            this.gridColumn5,
            this.colFecha,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn20,
            this.colTipoMovimientoActivo,
            this.gridColumn22,
            this.colIDMovimiento,
            this.colEsDepreciado,
            this.colTipoMovimiento,
            this.gridColumn6,
            this.colEstacionID});
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
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Estación Servicio";
            this.gridColumn1.FieldName = "EstacionNombre";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 115;
            // 
            // colSUS
            // 
            this.colSUS.Caption = "Sub Estación";
            this.colSUS.FieldName = "SubEstacionNombre";
            this.colSUS.Name = "colSUS";
            this.colSUS.Visible = true;
            this.colSUS.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Código";
            this.gridColumn2.FieldName = "Codigo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 13;
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Nro. Factura";
            this.colNumero.FieldName = "NoFactura";
            this.colNumero.Name = "colNumero";
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 10;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Nombre";
            this.gridColumn4.FieldName = "Nombre";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 14;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Descripción";
            this.gridColumn5.FieldName = "Descripcion";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 17;
            // 
            // colFecha
            // 
            this.colFecha.Caption = "Fecha Adquisición";
            this.colFecha.FieldName = "FechaAdquisicion";
            this.colFecha.Name = "colFecha";
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 11;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Usuario Asignado";
            this.gridColumn7.FieldName = "UsuarioAsignado";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 18;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Nro. Serie";
            this.gridColumn8.FieldName = "NoSerie";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 19;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Marca";
            this.gridColumn9.FieldName = "Marca";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 20;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Modelo";
            this.gridColumn10.FieldName = "Modelo";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 21;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Matricula";
            this.gridColumn11.FieldName = "Matricula";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 22;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Chasis";
            this.gridColumn12.FieldName = "Chasis";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 23;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Motor";
            this.gridColumn13.FieldName = "Motor";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 24;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "Área";
            this.gridColumn14.FieldName = "CentroCosto";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 12;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "Código Tipo Activo";
            this.gridColumn15.FieldName = "TipoActivoCodigo";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 3;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "Nombre Tipo Activo";
            this.gridColumn16.FieldName = "TipoActivoNombre";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 4;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "Código Cuenta";
            this.gridColumn17.FieldName = "CuentaActivoCodigo";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 8;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Nombre Cuenta";
            this.gridColumn18.FieldName = "CuentaActivoNombre";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 9;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "Código Activo";
            this.gridColumn19.FieldName = "ActivoCodigo";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 6;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "Nombre Activo";
            this.gridColumn20.FieldName = "ActivoNombre";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 7;
            // 
            // colTipoMovimientoActivo
            // 
            this.colTipoMovimientoActivo.Caption = "Tipo de Movimiento";
            this.colTipoMovimientoActivo.FieldName = "TipoMovimientoActivoNombre";
            this.colTipoMovimientoActivo.Name = "colTipoMovimientoActivo";
            this.colTipoMovimientoActivo.Visible = true;
            this.colTipoMovimientoActivo.VisibleIndex = 2;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "Valor Adquisición";
            this.gridColumn22.DisplayFormat.FormatString = "N2";
            this.gridColumn22.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn22.FieldName = "ValorAdquisicion";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 15;
            this.gridColumn22.Width = 103;
            // 
            // colIDMovimiento
            // 
            this.colIDMovimiento.FieldName = "MovimientoID";
            this.colIDMovimiento.Name = "colIDMovimiento";
            // 
            // colEsDepreciado
            // 
            this.colEsDepreciado.Caption = "En Depreciación";
            this.colEsDepreciado.FieldName = "EsDepreciado";
            this.colEsDepreciado.Name = "colEsDepreciado";
            this.colEsDepreciado.Visible = true;
            this.colEsDepreciado.VisibleIndex = 16;
            this.colEsDepreciado.Width = 109;
            // 
            // colTipoMovimiento
            // 
            this.colTipoMovimiento.FieldName = "MovimientoTipoID";
            this.colTipoMovimiento.Name = "colTipoMovimiento";
            this.colTipoMovimiento.Visible = true;
            this.colTipoMovimiento.VisibleIndex = 25;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Vida Util";
            this.gridColumn6.DisplayFormat.FormatString = "f0";
            this.gridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn6.FieldName = "VidaUtilMeses";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 57;
            // 
            // colEstacionID
            // 
            this.colEstacionID.FieldName = "EstacionID";
            this.colEstacionID.Name = "colEstacionID";
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
            // lkMoneda
            // 
            this.lkMoneda.AutoHeight = false;
            this.lkMoneda.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMoneda.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Monedas")});
            this.lkMoneda.DisplayMember = "Display";
            this.lkMoneda.Name = "lkMoneda";
            this.lkMoneda.ValueMember = "ID";
            // 
            // FormBien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 361);
            this.Controls.Add(this.splitCentral);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormBien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista de Bienes";
            this.Load += new System.EventHandler(this.FormBien_Load);
            this.Controls.SetChildIndex(this.splitCentral, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCentral)).EndInit();
            this.splitCentral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion       

        private DevExpress.XtraEditors.SplitContainerControl splitCentral;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colSUS;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMes;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpIslas;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTecnico;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMoneda;
        private DevExpress.Data.Linq.LinqInstantFeedbackSource linqInstantFeedbackSource1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoMovimientoActivo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraGrid.Columns.GridColumn colIDMovimiento;
        private DevExpress.XtraGrid.Columns.GridColumn colEsDepreciado;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoMovimiento;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionID;

    }
}
