namespace SAGAS.Inventario.Forms.Dialogs
{
    partial class DialogInventarioUpload
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogInventarioUpload));
            this.colCodigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.lkConcepto = new DevExpress.XtraEditors.LookUpEdit();
            this.memoObservacion = new DevExpress.XtraEditors.MemoEdit();
            this.dateFecha = new DevExpress.XtraEditors.DateEdit();
            this.lkArea = new DevExpress.XtraEditors.LookUpEdit();
            this.txtNumero = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemAlmacen = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.bgvProductos = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.bandEs = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colIDP = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colProducto = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colUnidad = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandAlmacenes = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandTotal = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colcosto = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPrecio = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
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
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkConcepto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoObservacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvProductos)).BeginInit();
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
            this.colCodigo.Caption = "Código";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCodigo.Visible = true;
            this.colCodigo.Width = 81;
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.lkConcepto);
            this.layoutControlTop.Controls.Add(this.memoObservacion);
            this.layoutControlTop.Controls.Add(this.dateFecha);
            this.layoutControlTop.Controls.Add(this.lkArea);
            this.layoutControlTop.Controls.Add(this.txtNumero);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 81);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // lkConcepto
            // 
            this.lkConcepto.Location = new System.Drawing.Point(371, 52);
            this.lkConcepto.Name = "lkConcepto";
            this.lkConcepto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkConcepto.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Conceptos")});
            this.lkConcepto.Properties.DisplayMember = "Nombre";
            this.lkConcepto.Properties.NullText = "<Seleccione el Concepto>";
            this.lkConcepto.Properties.ReadOnly = true;
            this.lkConcepto.Properties.ValueMember = "ID";
            this.lkConcepto.Size = new System.Drawing.Size(110, 20);
            this.lkConcepto.StyleController = this.layoutControlTop;
            this.lkConcepto.TabIndex = 10;
            // 
            // memoObservacion
            // 
            this.memoObservacion.Location = new System.Drawing.Point(590, 28);
            this.memoObservacion.Name = "memoObservacion";
            this.memoObservacion.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.memoObservacion.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.memoObservacion.Size = new System.Drawing.Size(333, 44);
            this.memoObservacion.StyleController = this.layoutControlTop;
            this.memoObservacion.TabIndex = 7;
            this.memoObservacion.UseOptimizedRendering = true;
            // 
            // dateFecha
            // 
            this.dateFecha.EditValue = new System.DateTime(2014, 7, 15, 16, 19, 4, 0);
            this.dateFecha.Location = new System.Drawing.Point(114, 28);
            this.dateFecha.Name = "dateFecha";
            this.dateFecha.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dateFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFecha.Size = new System.Drawing.Size(158, 20);
            this.dateFecha.StyleController = this.layoutControlTop;
            this.dateFecha.TabIndex = 4;
            this.dateFecha.Validated += new System.EventHandler(this.dateFecha_Validated);
            // 
            // lkArea
            // 
            this.lkArea.Location = new System.Drawing.Point(114, 52);
            this.lkArea.Name = "lkArea";
            this.lkArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkArea.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Areas")});
            this.lkArea.Properties.DisplayMember = "Nombre";
            this.lkArea.Properties.NullText = "<Seleccione el Área>";
            this.lkArea.Properties.ValueMember = "ID";
            this.lkArea.Size = new System.Drawing.Size(158, 20);
            this.lkArea.StyleController = this.layoutControlTop;
            this.lkArea.TabIndex = 9;
            this.lkArea.EditValueChanged += new System.EventHandler(this.lkArea_EditValueChanged);
            this.lkArea.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.lkArea_EditValueChanging);
            this.lkArea.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // txtNumero
            // 
            this.txtNumero.EditValue = "";
            this.txtNumero.Location = new System.Drawing.Point(371, 28);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Properties.Mask.EditMask = "999999999";
            this.txtNumero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtNumero.Properties.ReadOnly = true;
            this.txtNumero.Size = new System.Drawing.Size(110, 20);
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
            this.layoutControlGroup1.Text = "Encabezado de Carga de Inventario";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAlmacen,
            this.layoutControlItem8,
            this.layoutControlItem1,
            this.layoutControlItem11,
            this.layoutControlItem2});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.ShowTabPageCloseButton = true;
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(924, 54);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlItemAlmacen
            // 
            this.layoutControlItemAlmacen.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemAlmacen.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemAlmacen.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemAlmacen.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItemAlmacen.Control = this.lkArea;
            this.layoutControlItemAlmacen.CustomizationFormText = "Almacen";
            this.layoutControlItemAlmacen.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemAlmacen.Name = "layoutControlItemAlmacen";
            this.layoutControlItemAlmacen.Size = new System.Drawing.Size(267, 24);
            this.layoutControlItemAlmacen.Text = "Área";
            this.layoutControlItemAlmacen.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItemAlmacen.TextLocation = DevExpress.Utils.Locations.Default;
            this.layoutControlItemAlmacen.TextSize = new System.Drawing.Size(100, 13);
            this.layoutControlItemAlmacen.TextToControlDistance = 5;
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
            this.layoutControlItem8.Size = new System.Drawing.Size(267, 24);
            this.layoutControlItem8.Text = "Fecha";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(100, 13);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtNumero;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(267, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(209, 24);
            this.layoutControlItem1.Text = "Número";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.memoObservacion;
            this.layoutControlItem11.CustomizationFormText = "Dirección";
            this.layoutControlItem11.Location = new System.Drawing.Point(476, 0);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(442, 48);
            this.layoutControlItem11.Text = "Observación";
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(100, 20);
            this.layoutControlItem11.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.Control = this.lkConcepto;
            this.layoutControlItem2.CustomizationFormText = "Concepto";
            this.layoutControlItem2.Location = new System.Drawing.Point(267, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(209, 24);
            this.layoutControlItem2.Text = "Concepto";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(90, 20);
            this.layoutControlItem2.TextToControlDistance = 5;
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
            this.grid.MainView = this.bgvProductos;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(932, 428);
            this.grid.TabIndex = 7;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvProductos,
            this.gvData});
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // bgvProductos
            // 
            this.bgvProductos.Appearance.BandPanel.BackColor = System.Drawing.Color.LightCyan;
            this.bgvProductos.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvProductos.Appearance.BandPanel.Options.UseBackColor = true;
            this.bgvProductos.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.LightCyan;
            this.bgvProductos.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvProductos.Appearance.BandPanelBackground.Options.UseBackColor = true;
            this.bgvProductos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.bgvProductos.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.Transparent;
            this.bgvProductos.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.bgvProductos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.bgvProductos.Appearance.HeaderPanelBackground.ForeColor = System.Drawing.Color.Red;
            this.bgvProductos.Appearance.HeaderPanelBackground.Options.UseForeColor = true;
            this.bgvProductos.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandEs,
            this.bandAlmacenes,
            this.bandTotal});
            this.bgvProductos.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colIDP,
            this.colProducto,
            this.colTotal,
            this.colCodigo,
            this.colPrecio,
            this.colcosto,
            this.colUnidad});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Column = this.colCodigo;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
            styleFormatCondition1.Expression = "[IDP] == 0";
            this.bgvProductos.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.bgvProductos.GridControl = this.grid;
            this.bgvProductos.Name = "bgvProductos";
            this.bgvProductos.OptionsCustomization.AllowBandMoving = false;
            this.bgvProductos.OptionsCustomization.AllowColumnMoving = false;
            this.bgvProductos.OptionsCustomization.AllowColumnResizing = false;
            this.bgvProductos.OptionsCustomization.AllowFilter = false;
            this.bgvProductos.OptionsMenu.EnableColumnMenu = false;
            this.bgvProductos.OptionsMenu.EnableFooterMenu = false;
            this.bgvProductos.OptionsMenu.EnableGroupPanelMenu = false;
            this.bgvProductos.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.bgvProductos.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.bgvProductos.OptionsSelection.MultiSelect = true;
            this.bgvProductos.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.bgvProductos.OptionsView.ColumnAutoWidth = false;
            this.bgvProductos.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.bgvProductos.OptionsView.ShowAutoFilterRow = true;
            this.bgvProductos.OptionsView.ShowFooter = true;
            this.bgvProductos.OptionsView.ShowGroupPanel = false;
            this.bgvProductos.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.bgvProductos_PopupMenuShowing);
            this.bgvProductos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bgvProductos_KeyDown);
            // 
            // bandEs
            // 
            this.bandEs.Caption = "<>";
            this.bandEs.Columns.Add(this.colIDP);
            this.bandEs.Columns.Add(this.colCodigo);
            this.bandEs.Columns.Add(this.colProducto);
            this.bandEs.Columns.Add(this.colUnidad);
            this.bandEs.Name = "bandEs";
            this.bandEs.OptionsBand.AllowMove = false;
            this.bandEs.OptionsBand.AllowPress = false;
            this.bandEs.OptionsBand.ShowCaption = false;
            this.bandEs.VisibleIndex = 0;
            this.bandEs.Width = 469;
            // 
            // colIDP
            // 
            this.colIDP.FieldName = "IDP";
            this.colIDP.Name = "colIDP";
            this.colIDP.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // colProducto
            // 
            this.colProducto.AppearanceHeader.BackColor = System.Drawing.Color.LightCyan;
            this.colProducto.AppearanceHeader.BackColor2 = System.Drawing.Color.LightCyan;
            this.colProducto.AppearanceHeader.BorderColor = System.Drawing.Color.Transparent;
            this.colProducto.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colProducto.AppearanceHeader.Options.UseBackColor = true;
            this.colProducto.AppearanceHeader.Options.UseForeColor = true;
            this.colProducto.Caption = "Producto";
            this.colProducto.FieldName = "Producto";
            this.colProducto.Name = "colProducto";
            this.colProducto.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colProducto.Visible = true;
            this.colProducto.Width = 306;
            // 
            // colUnidad
            // 
            this.colUnidad.Caption = "Unidad Med.";
            this.colUnidad.FieldName = "Unidad";
            this.colUnidad.Name = "colUnidad";
            this.colUnidad.Visible = true;
            this.colUnidad.Width = 82;
            // 
            // bandAlmacenes
            // 
            this.bandAlmacenes.AppearanceHeader.Options.UseTextOptions = true;
            this.bandAlmacenes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandAlmacenes.Caption = "Alamacenes";
            this.bandAlmacenes.Name = "bandAlmacenes";
            this.bandAlmacenes.VisibleIndex = 1;
            this.bandAlmacenes.Width = 185;
            // 
            // bandTotal
            // 
            this.bandTotal.AppearanceHeader.Options.UseTextOptions = true;
            this.bandTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandTotal.Columns.Add(this.colcosto);
            this.bandTotal.Columns.Add(this.colTotal);
            this.bandTotal.Columns.Add(this.colPrecio);
            this.bandTotal.Name = "bandTotal";
            this.bandTotal.VisibleIndex = 2;
            this.bandTotal.Width = 225;
            // 
            // colcosto
            // 
            this.colcosto.Caption = "Costo Prom.";
            this.colcosto.DisplayFormat.FormatString = "N6";
            this.colcosto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colcosto.FieldName = "Costo";
            this.colcosto.Name = "colcosto";
            this.colcosto.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colcosto.Visible = true;
            // 
            // colTotal
            // 
            this.colTotal.AppearanceHeader.Options.UseTextOptions = true;
            this.colTotal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTotal.Caption = "Total";
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTotal.OptionsFilter.AllowAutoFilter = false;
            this.colTotal.OptionsFilter.AllowFilter = false;
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:N2}")});
            this.colTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTotal.Visible = true;
            // 
            // colPrecio
            // 
            this.colPrecio.Caption = "Precio";
            this.colPrecio.DisplayFormat.FormatString = "N6";
            this.colPrecio.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrecio.FieldName = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPrecio.Visible = true;
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
            this.colNombreEstacion.Caption = "Estación de Servicio";
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
            this.btnValidate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnValidate.Image = global::SAGAS.Inventario.Properties.Resources.scale_image;
            this.btnValidate.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnValidate.Location = new System.Drawing.Point(738, 2);
            this.btnValidate.LookAndFeel.SkinName = "McSkin";
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(84, 31);
            this.btnValidate.TabIndex = 6;
            this.btnValidate.Text = "Validar \r\nProductos";
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
            this.bntFinalizar.Image = global::SAGAS.Inventario.Properties.Resources.EnviarOrden;
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
            // DialogInventarioUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 561);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogInventarioUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkConcepto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoObservacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvProductos)).EndInit();
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
        private DevExpress.XtraEditors.LookUpEdit lkArea;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAlmacen;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraEditors.MemoEdit memoObservacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bgvProductos;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDP;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProducto;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreEstacion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTotal;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colcosto;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPrecio;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandEs;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colUnidad;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandAlmacenes;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandTotal;
        private DevExpress.XtraEditors.SimpleButton btnValidate;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.LookUpEdit lkConcepto;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.SimpleButton bntFinalizar;
    }
}