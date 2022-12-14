namespace SAGAS.Ventas.Forms.Dialogs
{
    partial class DialogClienteUpload
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogClienteUpload));
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.chkAnticipos = new DevExpress.XtraEditors.CheckEdit();
            this.memoObservacion = new DevExpress.XtraEditors.MemoEdit();
            this.dateFecha = new DevExpress.XtraEditors.DateEdit();
            this.txtNumero = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource(this.components);
            this.gvCarga = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.datePlantillaDetalle = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colClienteID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreCliente = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaldo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spSaldo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnShowComprobante = new DevExpress.XtraEditors.SimpleButton();
            this.btnValidate = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.bntFinalizar = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAnticipos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoObservacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCarga)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datePlantillaDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datePlantillaDetalle.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSaldo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "C?digo";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 2;
            this.colCodigo.Width = 132;
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.chkAnticipos);
            this.layoutControlTop.Controls.Add(this.memoObservacion);
            this.layoutControlTop.Controls.Add(this.dateFecha);
            this.layoutControlTop.Controls.Add(this.txtNumero);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 81);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // chkAnticipos
            // 
            this.chkAnticipos.Location = new System.Drawing.Point(189, 28);
            this.chkAnticipos.Name = "chkAnticipos";
            this.chkAnticipos.Properties.Caption = "Aplican Anticipos ?";
            this.chkAnticipos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.chkAnticipos.Size = new System.Drawing.Size(110, 19);
            this.chkAnticipos.StyleController = this.layoutControlTop;
            this.chkAnticipos.TabIndex = 9;
            // 
            // memoObservacion
            // 
            this.memoObservacion.Location = new System.Drawing.Point(408, 28);
            this.memoObservacion.Name = "memoObservacion";
            this.memoObservacion.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.memoObservacion.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.memoObservacion.Size = new System.Drawing.Size(515, 44);
            this.memoObservacion.StyleController = this.layoutControlTop;
            this.memoObservacion.TabIndex = 7;
            this.memoObservacion.UseOptimizedRendering = true;
            // 
            // dateFecha
            // 
            this.dateFecha.EditValue = new System.DateTime(2014, 7, 15, 16, 19, 4, 0);
            this.dateFecha.Location = new System.Drawing.Point(104, 28);
            this.dateFecha.Name = "dateFecha";
            this.dateFecha.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dateFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFecha.Size = new System.Drawing.Size(81, 20);
            this.dateFecha.StyleController = this.layoutControlTop;
            this.dateFecha.TabIndex = 4;
            this.dateFecha.Validated += new System.EventHandler(this.dateFecha_Validated);
            // 
            // txtNumero
            // 
            this.txtNumero.EditValue = "";
            this.txtNumero.Location = new System.Drawing.Point(104, 52);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Properties.Mask.EditMask = "999999999";
            this.txtNumero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtNumero.Properties.ReadOnly = true;
            this.txtNumero.Size = new System.Drawing.Size(81, 20);
            this.txtNumero.StyleController = this.layoutControlTop;
            this.txtNumero.TabIndex = 6;
            this.txtNumero.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroupDatos});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup1.ShowTabPageCloseButton = true;
            this.layoutControlGroup1.Size = new System.Drawing.Size(932, 81);
            this.layoutControlGroup1.Text = "Encabezado Carga de Clientes";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.layoutControlItem11,
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.ShowTabPageCloseButton = true;
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(924, 54);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem8.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.dateFecha;
            this.layoutControlItem8.CustomizationFormText = "Fecha";
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(180, 24);
            this.layoutControlItem8.Text = "Fecha";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.memoObservacion;
            this.layoutControlItem11.CustomizationFormText = "Direcci?n";
            this.layoutControlItem11.Location = new System.Drawing.Point(294, 0);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(624, 48);
            this.layoutControlItem11.Text = "Observaci?n";
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(100, 20);
            this.layoutControlItem11.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtNumero;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(180, 24);
            this.layoutControlItem1.Text = "N?mero";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.chkAnticipos;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(180, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(114, 48);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.grid);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(936, 526);
            this.panelControl1.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.DataSource = this.bdsDetalle;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridDetalle_EmbeddedNavigator_ButtonClick);
            gridLevelNode1.RelationName = "Level1";
            this.grid.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grid.Location = new System.Drawing.Point(2, 83);
            this.grid.MainView = this.gvCarga;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.datePlantillaDetalle,
            this.spSaldo});
            this.grid.Size = new System.Drawing.Size(932, 428);
            this.grid.TabIndex = 7;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCarga,
            this.gvData});
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // gvCarga
            // 
            this.gvCarga.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.gvCarga.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.Transparent;
            this.gvCarga.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.gvCarga.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvCarga.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDP,
            this.colFecha,
            this.colNumero,
            this.colCodigo,
            this.colClienteID,
            this.colNombreCliente,
            this.colSaldo});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Column = this.colCodigo;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition1.Expression = "[ClienteID] == 0";
            this.gvCarga.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gvCarga.GridControl = this.grid;
            this.gvCarga.Name = "gvCarga";
            this.gvCarga.OptionsCustomization.AllowColumnMoving = false;
            this.gvCarga.OptionsCustomization.AllowFilter = false;
            this.gvCarga.OptionsMenu.EnableColumnMenu = false;
            this.gvCarga.OptionsMenu.EnableFooterMenu = false;
            this.gvCarga.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvCarga.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvCarga.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvCarga.OptionsSelection.MultiSelect = true;
            this.gvCarga.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvCarga.OptionsView.ColumnAutoWidth = false;
            this.gvCarga.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvCarga.OptionsView.ShowAutoFilterRow = true;
            this.gvCarga.OptionsView.ShowFooter = true;
            this.gvCarga.OptionsView.ShowGroupPanel = false;
            this.gvCarga.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gvCarga_PopupMenuShowing);
            // 
            // colIDP
            // 
            this.colIDP.FieldName = "ID";
            this.colIDP.Name = "colIDP";
            this.colIDP.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // colFecha
            // 
            this.colFecha.AppearanceHeader.BackColor = System.Drawing.Color.LightCyan;
            this.colFecha.AppearanceHeader.BackColor2 = System.Drawing.Color.LightCyan;
            this.colFecha.AppearanceHeader.BorderColor = System.Drawing.Color.Transparent;
            this.colFecha.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colFecha.AppearanceHeader.Options.UseBackColor = true;
            this.colFecha.AppearanceHeader.Options.UseForeColor = true;
            this.colFecha.Caption = "Fecha";
            this.colFecha.ColumnEdit = this.datePlantillaDetalle;
            this.colFecha.FieldName = "Fecha";
            this.colFecha.Name = "colFecha";
            this.colFecha.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 0;
            this.colFecha.Width = 138;
            // 
            // datePlantillaDetalle
            // 
            this.datePlantillaDetalle.AutoHeight = false;
            this.datePlantillaDetalle.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datePlantillaDetalle.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.datePlantillaDetalle.Name = "datePlantillaDetalle";
            // 
            // colNumero
            // 
            this.colNumero.AppearanceHeader.Options.UseTextOptions = true;
            this.colNumero.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNumero.Caption = "N?mero";
            this.colNumero.FieldName = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colNumero.OptionsFilter.AllowAutoFilter = false;
            this.colNumero.OptionsFilter.AllowFilter = false;
            this.colNumero.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 1;
            this.colNumero.Width = 84;
            // 
            // colClienteID
            // 
            this.colClienteID.FieldName = "ClienteID";
            this.colClienteID.Name = "colClienteID";
            this.colClienteID.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colClienteID.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // colNombreCliente
            // 
            this.colNombreCliente.Caption = "Cliente";
            this.colNombreCliente.FieldName = "NombreCliente";
            this.colNombreCliente.Name = "colNombreCliente";
            this.colNombreCliente.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colNombreCliente.Visible = true;
            this.colNombreCliente.VisibleIndex = 3;
            this.colNombreCliente.Width = 217;
            // 
            // colSaldo
            // 
            this.colSaldo.Caption = "Saldo";
            this.colSaldo.ColumnEdit = this.spSaldo;
            this.colSaldo.DisplayFormat.FormatString = "N2";
            this.colSaldo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSaldo.FieldName = "Saldo";
            this.colSaldo.Name = "colSaldo";
            this.colSaldo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colSaldo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Saldo", "{0:N2}")});
            this.colSaldo.Visible = true;
            this.colSaldo.VisibleIndex = 4;
            // 
            // spSaldo
            // 
            this.spSaldo.AutoHeight = false;
            this.spSaldo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spSaldo.DisplayFormat.FormatString = "N2";
            this.spSaldo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSaldo.EditFormat.FormatString = "N2";
            this.spSaldo.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSaldo.Mask.EditMask = "N2";
            this.spSaldo.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spSaldo.Name = "spSaldo";
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombreEstacion});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombreEstacion
            // 
            this.colNombreEstacion.Caption = "Estaci?n de Servicio";
            this.colNombreEstacion.FieldName = "Nombre";
            this.colNombreEstacion.Name = "colNombreEstacion";
            this.colNombreEstacion.Visible = true;
            this.colNombreEstacion.VisibleIndex = 0;
            this.colNombreEstacion.Width = 265;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 511);
            this.lblRequerido.Name = "lblRequerido";
            this.lblRequerido.Size = new System.Drawing.Size(255, 13);
            this.lblRequerido.TabIndex = 1;
            this.lblRequerido.Text = "Nota:  Los campos en negritas son requeridos";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnShowComprobante);
            this.panelControl2.Controls.Add(this.btnValidate);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.bntNew);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Controls.Add(this.bntFinalizar);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 526);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(936, 35);
            this.panelControl2.TabIndex = 1;
            // 
            // btnShowComprobante
            // 
            this.btnShowComprobante.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowComprobante.Image = ((System.Drawing.Image)(resources.GetObject("btnShowComprobante.Image")));
            this.btnShowComprobante.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnShowComprobante.Location = new System.Drawing.Point(631, 2);
            this.btnShowComprobante.LookAndFeel.SkinName = "McSkin";
            this.btnShowComprobante.Name = "btnShowComprobante";
            this.btnShowComprobante.Size = new System.Drawing.Size(107, 31);
            this.btnShowComprobante.TabIndex = 7;
            this.btnShowComprobante.Text = "Mostrar \r\nComprobante";
            this.btnShowComprobante.Click += new System.EventHandler(this.btnShowComprobante_Click);
            // 
            // btnValidate
            // 
            this.btnValidate.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.btnValidate.Appearance.Options.UseFont = true;
            this.btnValidate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnValidate.Image = global::SAGAS.Ventas.Properties.Resources.scale_image;
            this.btnValidate.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnValidate.Location = new System.Drawing.Point(738, 2);
            this.btnValidate.LookAndFeel.SkinName = "McSkin";
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(84, 31);
            this.btnValidate.TabIndex = 6;
            this.btnValidate.Text = "Validar \r\nC?digos";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(151, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bntNew
            // 
            this.bntNew.Dock = System.Windows.Forms.DockStyle.Left;
            this.bntNew.Image = ((System.Drawing.Image)(resources.GetObject("bntNew.Image")));
            this.bntNew.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.bntNew.Location = new System.Drawing.Point(86, 2);
            this.bntNew.LookAndFeel.SkinName = "McSkin";
            this.bntNew.Name = "bntNew";
            this.bntNew.Size = new System.Drawing.Size(65, 31);
            this.bntNew.TabIndex = 5;
            this.bntNew.Text = "Nuevo";
            this.bntNew.Click += new System.EventHandler(this.bntNew_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 31);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Guardar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // bntFinalizar
            // 
            this.bntFinalizar.Dock = System.Windows.Forms.DockStyle.Right;
            this.bntFinalizar.Image = global::SAGAS.Ventas.Properties.Resources.EnviarOrden;
            this.bntFinalizar.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.bntFinalizar.Location = new System.Drawing.Point(822, 2);
            this.bntFinalizar.LookAndFeel.SkinName = "McSkin";
            this.bntFinalizar.Name = "bntFinalizar";
            this.bntFinalizar.Size = new System.Drawing.Size(112, 31);
            this.bntFinalizar.TabIndex = 8;
            this.bntFinalizar.Text = "Finalizar Carga";
            this.bntFinalizar.Click += new System.EventHandler(this.bntFinalizar_Click);
            // 
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // infDifES
            // 
            this.infDifES.ContainerControl = this;
            // 
            // errTC_Periodo
            // 
            this.errTC_Periodo.ContainerControl = this;
            // 
            // DialogClienteUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 561);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogClienteUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkAnticipos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoObservacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCarga)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datePlantillaDetalle.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datePlantillaDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSaldo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.TextEdit txtNumero;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.DateEdit dateFecha;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraEditors.MemoEdit memoObservacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreEstacion;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btnValidate;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.SimpleButton bntFinalizar;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCarga;
        private DevExpress.XtraGrid.Columns.GridColumn colIDP;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colClienteID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreCliente;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit datePlantillaDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colSaldo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSaldo;
        private DevExpress.XtraEditors.CheckEdit chkAnticipos;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}