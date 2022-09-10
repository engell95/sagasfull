namespace SAGAS.Ventas.Forms.Dialogs
{
    partial class DialogVentas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogVentas));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.lbCliente = new DevExpress.XtraEditors.ListBoxControl();
            this.lkArea = new DevExpress.XtraEditors.LookUpEdit();
            this.glkClient = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dateFechaVencimiento = new DevExpress.XtraEditors.DateEdit();
            this.txtPlazo = new DevExpress.XtraEditors.TextEdit();
            this.dateFechaFactura = new DevExpress.XtraEditors.DateEdit();
            this.mmoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.txtReferencia = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ColAlmacen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacen = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colCantidadInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpCalcCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.colPrecio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spPrecio = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colSubTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colDescuento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spDescuento = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colVentaNeta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTaxValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spTax = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCosto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColSiempreSalidaVentas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColEsProducto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lblGrandTotal = new DevExpress.XtraEditors.LabelControl();
            this.txtGrandTotal = new DevExpress.XtraEditors.TextEdit();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnShowComprobante = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infBloqueo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlazo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaFactura.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReferencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCalcCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrecio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDescuento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrandTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infBloqueo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.lbCliente);
            this.layoutControlTop.Controls.Add(this.lkArea);
            this.layoutControlTop.Controls.Add(this.glkClient);
            this.layoutControlTop.Controls.Add(this.dateFechaVencimiento);
            this.layoutControlTop.Controls.Add(this.txtPlazo);
            this.layoutControlTop.Controls.Add(this.dateFechaFactura);
            this.layoutControlTop.Controls.Add(this.mmoComentario);
            this.layoutControlTop.Controls.Add(this.txtReferencia);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 164);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // lbCliente
            // 
            this.lbCliente.Appearance.BackColor = System.Drawing.Color.White;
            this.lbCliente.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lbCliente.Appearance.Options.UseBackColor = true;
            this.lbCliente.Appearance.Options.UseForeColor = true;
            this.lbCliente.Enabled = false;
            this.lbCliente.Location = new System.Drawing.Point(15, 82);
            this.lbCliente.Name = "lbCliente";
            this.lbCliente.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbCliente.Size = new System.Drawing.Size(307, 67);
            this.lbCliente.StyleController = this.layoutControlTop;
            this.lbCliente.TabIndex = 7;
            this.lbCliente.TabStop = false;
            // 
            // lkArea
            // 
            this.lkArea.Location = new System.Drawing.Point(107, 31);
            this.lkArea.Name = "lkArea";
            this.lkArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkArea.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Áreas")});
            this.lkArea.Properties.DisplayMember = "Display";
            this.lkArea.Properties.NullText = "<Seleccione el Área>";
            this.lkArea.Properties.ValueMember = "ID";
            this.lkArea.Size = new System.Drawing.Size(215, 20);
            this.lkArea.StyleController = this.layoutControlTop;
            this.lkArea.TabIndex = 6;
            this.lkArea.EditValueChanged += new System.EventHandler(this.lkArea_EditValueChanged);
            this.lkArea.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.lkArea_EditValueChanging);
            // 
            // glkClient
            // 
            this.glkClient.Location = new System.Drawing.Point(110, 58);
            this.glkClient.Name = "glkClient";
            this.glkClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkClient.Properties.DisplayMember = "Display";
            this.glkClient.Properties.NullText = "<Seleccione el Cliente>";
            this.glkClient.Properties.ValueMember = "ID";
            this.glkClient.Properties.View = this.gridLookUpEdit1View;
            this.glkClient.Size = new System.Drawing.Size(212, 20);
            this.glkClient.StyleController = this.layoutControlTop;
            this.glkClient.TabIndex = 2;
            this.glkClient.EditValueChanged += new System.EventHandler(this.glkProvee_EditValueChanged);
            this.glkClient.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.glkClient_EditValueChanging);
            this.glkClient.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsBehavior.Editable = false;
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Código";
            this.gridColumn2.FieldName = "Codigo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 179;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Cliente";
            this.gridColumn3.FieldName = "Nombre";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 640;
            // 
            // dateFechaVencimiento
            // 
            this.dateFechaVencimiento.EditValue = null;
            this.dateFechaVencimiento.Enabled = false;
            this.dateFechaVencimiento.Location = new System.Drawing.Point(819, 82);
            this.dateFechaVencimiento.Name = "dateFechaVencimiento";
            this.dateFechaVencimiento.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.dateFechaVencimiento.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.dateFechaVencimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaVencimiento.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaVencimiento.Size = new System.Drawing.Size(98, 20);
            this.dateFechaVencimiento.StyleController = this.layoutControlTop;
            this.dateFechaVencimiento.TabIndex = 8;
            this.dateFechaVencimiento.EditValueChanged += new System.EventHandler(this.dateFechaVencimiento_EditValueChanged);
            this.dateFechaVencimiento.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // txtPlazo
            // 
            this.txtPlazo.Enabled = false;
            this.txtPlazo.Location = new System.Drawing.Point(819, 58);
            this.txtPlazo.Name = "txtPlazo";
            this.txtPlazo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtPlazo.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtPlazo.Size = new System.Drawing.Size(98, 20);
            this.txtPlazo.StyleController = this.layoutControlTop;
            this.txtPlazo.TabIndex = 5;
            this.txtPlazo.Enter += new System.EventHandler(this.txtRuc_Enter);
            // 
            // dateFechaFactura
            // 
            this.dateFechaFactura.EditValue = null;
            this.dateFechaFactura.Location = new System.Drawing.Point(819, 31);
            this.dateFechaFactura.Name = "dateFechaFactura";
            this.dateFechaFactura.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dateFechaFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaFactura.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaFactura.Size = new System.Drawing.Size(101, 20);
            this.dateFechaFactura.StyleController = this.layoutControlTop;
            this.dateFechaFactura.TabIndex = 1;
            this.dateFechaFactura.EditValueChanged += new System.EventHandler(this.dateFechaFactura_EditValueChanged);
            this.dateFechaFactura.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.dateFechaFactura_EditValueChanging);
            this.dateFechaFactura.Validated += new System.EventHandler(this.dateFechaFactura_Validated);
            // 
            // mmoComentario
            // 
            this.mmoComentario.Location = new System.Drawing.Point(421, 58);
            this.mmoComentario.Name = "mmoComentario";
            this.mmoComentario.Size = new System.Drawing.Size(252, 91);
            this.mmoComentario.StyleController = this.layoutControlTop;
            this.mmoComentario.TabIndex = 8;
            this.mmoComentario.UseOptimizedRendering = true;
            // 
            // txtReferencia
            // 
            this.txtReferencia.EditValue = "";
            this.txtReferencia.Location = new System.Drawing.Point(421, 31);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Properties.DisplayFormat.FormatString = "f0";
            this.txtReferencia.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtReferencia.Properties.EditFormat.FormatString = "f0";
            this.txtReferencia.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtReferencia.Properties.Mask.EditMask = "f0";
            this.txtReferencia.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtReferencia.Size = new System.Drawing.Size(252, 20);
            this.txtReferencia.StyleController = this.layoutControlTop;
            this.txtReferencia.TabIndex = 6;
            this.txtReferencia.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlGroupDatos,
            this.layoutControlGroup2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(932, 164);
            this.layoutControlGroup1.Text = "Encabezado de la Venta";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem5.Control = this.dateFechaFactura;
            this.layoutControlItem5.CustomizationFormText = "Fecha de Compra";
            this.layoutControlItem5.Location = new System.Drawing.Point(665, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(247, 24);
            this.layoutControlItem5.Text = "Fecha Factura";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(137, 13);
            this.layoutControlItem5.TextToControlDistance = 5;
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6,
            this.layoutControlItem9,
            this.layoutControlItem2,
            this.layoutControlItem12,
            this.layoutControlItem3});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 24);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(912, 101);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem6.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem6.Control = this.glkClient;
            this.layoutControlItem6.CustomizationFormText = "Proveedor";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(311, 24);
            this.layoutControlItem6.Text = "Cliente";
            this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem6.TextToControlDistance = 5;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem9.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem9.Control = this.txtPlazo;
            this.layoutControlItem9.CustomizationFormText = "Plazo";
            this.layoutControlItem9.Location = new System.Drawing.Point(662, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(244, 24);
            this.layoutControlItem9.Text = "Plazo";
            this.layoutControlItem9.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem9.TextSize = new System.Drawing.Size(137, 13);
            this.layoutControlItem9.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.layoutControlItem2.Control = this.mmoComentario;
            this.layoutControlItem2.CustomizationFormText = "Comentario";
            this.layoutControlItem2.Location = new System.Drawing.Point(311, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(351, 95);
            this.layoutControlItem2.Text = "Comentario";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextLocation = DevExpress.Utils.Locations.Default;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem12.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem12.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem12.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem12.Control = this.dateFechaVencimiento;
            this.layoutControlItem12.CustomizationFormText = "Fecha de Vencimiento";
            this.layoutControlItem12.Location = new System.Drawing.Point(662, 24);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(244, 71);
            this.layoutControlItem12.Text = "Fecha de Vencimiento";
            this.layoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(137, 13);
            this.layoutControlItem12.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lbCliente;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(311, 71);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AllowDrawBackground = false;
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem7});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(665, 24);
            this.layoutControlGroup2.Text = "layoutControlGroup2";
            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtReferencia;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(314, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(351, 24);
            this.layoutControlItem1.Text = "Nro de Factura";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem7.Control = this.lkArea;
            this.layoutControlItem7.CustomizationFormText = "Área de Venta";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(314, 24);
            this.layoutControlItem7.Text = "Área de Venta";
            this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem7.TextSize = new System.Drawing.Size(90, 20);
            this.layoutControlItem7.TextToControlDistance = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridDetalle);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(936, 526);
            this.panelControl1.TabIndex = 0;
            // 
            // gridDetalle
            // 
            this.gridDetalle.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridDetalle.DataSource = this.bdsDetalle;
            this.gridDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridDetalle.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridDetalle_EmbeddedNavigator_ButtonClick);
            this.gridDetalle.Location = new System.Drawing.Point(2, 166);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speCantidad,
            this.speCost,
            this.cboUnidadMedida,
            this.gridProductos,
            this.cboAlmacen,
            this.spSubTotal,
            this.spDescuento,
            this.spTax,
            this.spPrecio,
            this.rpCalcCantidad});
            this.gridDetalle.Size = new System.Drawing.Size(932, 345);
            this.gridDetalle.TabIndex = 8;
            this.gridDetalle.UseEmbeddedNavigator = true;
            this.gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            this.gridDetalle.Leave += new System.EventHandler(this.gridDetalle_Leave);
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // gvDetalle
            // 
            this.gvDetalle.ActiveFilterEnabled = false;
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProduct,
            this.colUnit,
            this.ColAlmacen,
            this.colCantidadInicial,
            this.colQuantity,
            this.colPrecio,
            this.colSubTotal,
            this.colDescuento,
            this.colVentaNeta,
            this.colTaxValue,
            this.colTotal,
            this.colAplicaIVA,
            this.colCosto,
            this.ColSiempreSalidaVentas,
            this.ColEsProducto});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowGroup = false;
            this.gvDetalle.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsFilter.AllowFilterEditor = false;
            this.gvDetalle.OptionsFilter.AllowMRUFilterList = false;
            this.gvDetalle.OptionsLayout.Columns.AddNewColumns = false;
            this.gvDetalle.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvDetalle.OptionsNavigation.AutoFocusNewRow = true;
            this.gvDetalle.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvDetalle.OptionsView.ShowFooter = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvDetalle_CustomRowCellEditForEditing);
            this.gvDetalle.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvDetalle_InvalidRowException);
            this.gvDetalle.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvDetalle_ValidateRow);
            this.gvDetalle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvDetalle_KeyDown);
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.ColumnEdit = this.gridProductos;
            this.colProduct.FieldName = "ProductoID";
            this.colProduct.Name = "colProduct";
            this.colProduct.Visible = true;
            this.colProduct.VisibleIndex = 0;
            this.colProduct.Width = 199;
            // 
            // gridProductos
            // 
            this.gridProductos.AutoHeight = false;
            this.gridProductos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.View = this.gridViewProductos;
            // 
            // gridViewProductos
            // 
            this.gridViewProductos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn7,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12});
            this.gridViewProductos.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewProductos.Name = "gridViewProductos";
            this.gridViewProductos.OptionsBehavior.Editable = false;
            this.gridViewProductos.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewProductos.OptionsView.ShowAutoFilterRow = true;
            this.gridViewProductos.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn7
            // 
            this.gridColumn7.FieldName = "ID";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Código";
            this.gridColumn10.FieldName = "Codigo";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 241;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Producto";
            this.gridColumn11.FieldName = "Nombre";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 458;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Uni.  Medida";
            this.gridColumn12.FieldName = "UmidadName";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 2;
            this.gridColumn12.Width = 159;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Unid. Med.";
            this.colUnit.ColumnEdit = this.cboUnidadMedida;
            this.colUnit.FieldName = "UnidadMedidaID";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.AllowEdit = false;
            this.colUnit.OptionsColumn.AllowFocus = false;
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 1;
            this.colUnit.Width = 90;
            // 
            // cboUnidadMedida
            // 
            this.cboUnidadMedida.AutoHeight = false;
            this.cboUnidadMedida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnidadMedida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.cboUnidadMedida.Name = "cboUnidadMedida";
            this.cboUnidadMedida.NullText = "";
            // 
            // ColAlmacen
            // 
            this.ColAlmacen.Caption = "Alm. / Tan.";
            this.ColAlmacen.ColumnEdit = this.cboAlmacen;
            this.ColAlmacen.FieldName = "AlmacenSalidaID";
            this.ColAlmacen.Name = "ColAlmacen";
            this.ColAlmacen.Visible = true;
            this.ColAlmacen.VisibleIndex = 2;
            this.ColAlmacen.Width = 86;
            // 
            // cboAlmacen
            // 
            this.cboAlmacen.AutoHeight = false;
            this.cboAlmacen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacen.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Almacen / Tanque")});
            this.cboAlmacen.DisplayMember = "Nombre";
            this.cboAlmacen.Name = "cboAlmacen";
            this.cboAlmacen.NullText = "";
            this.cboAlmacen.ValueMember = "ID";
            this.cboAlmacen.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboAlmacen_Closed);
            this.cboAlmacen.Popup += new System.EventHandler(this.cboAlmacen_Popup);
            this.cboAlmacen.Leave += new System.EventHandler(this.cboAlmacen_Leave);
            // 
            // colCantidadInicial
            // 
            this.colCantidadInicial.Caption = "Existencia";
            this.colCantidadInicial.FieldName = "CantidadInicial";
            this.colCantidadInicial.Name = "colCantidadInicial";
            this.colCantidadInicial.OptionsColumn.AllowEdit = false;
            this.colCantidadInicial.OptionsColumn.AllowFocus = false;
            this.colCantidadInicial.Visible = true;
            this.colCantidadInicial.VisibleIndex = 3;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "Cantidad";
            this.colQuantity.ColumnEdit = this.rpCalcCantidad;
            this.colQuantity.DisplayFormat.FormatString = "n3";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "CantidadSalida";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 4;
            this.colQuantity.Width = 55;
            // 
            // rpCalcCantidad
            // 
            this.rpCalcCantidad.AutoHeight = false;
            this.rpCalcCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpCalcCantidad.DisplayFormat.FormatString = "n3";
            this.rpCalcCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpCalcCantidad.EditFormat.FormatString = "n3";
            this.rpCalcCantidad.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpCalcCantidad.Name = "rpCalcCantidad";
            this.rpCalcCantidad.Precision = 3;
            // 
            // colPrecio
            // 
            this.colPrecio.Caption = "Precio";
            this.colPrecio.ColumnEdit = this.spPrecio;
            this.colPrecio.DisplayFormat.FormatString = "n6";
            this.colPrecio.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrecio.FieldName = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.OptionsColumn.AllowEdit = false;
            this.colPrecio.OptionsColumn.AllowFocus = false;
            this.colPrecio.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colPrecio.Visible = true;
            this.colPrecio.VisibleIndex = 5;
            this.colPrecio.Width = 71;
            // 
            // spPrecio
            // 
            this.spPrecio.AutoHeight = false;
            this.spPrecio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spPrecio.Name = "spPrecio";
            // 
            // colSubTotal
            // 
            this.colSubTotal.Caption = "Sub Total";
            this.colSubTotal.ColumnEdit = this.speCost;
            this.colSubTotal.DisplayFormat.FormatString = "n2";
            this.colSubTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSubTotal.FieldName = "PrecioTotal";
            this.colSubTotal.Name = "colSubTotal";
            this.colSubTotal.OptionsColumn.AllowEdit = false;
            this.colSubTotal.OptionsColumn.AllowFocus = false;
            this.colSubTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PrecioTotal", "{0:#,0.00;(#,0.00)}")});
            this.colSubTotal.UnboundExpression = "[CantidadSalida] * [Precio]";
            this.colSubTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colSubTotal.Visible = true;
            this.colSubTotal.VisibleIndex = 6;
            this.colSubTotal.Width = 81;
            // 
            // speCost
            // 
            this.speCost.AutoHeight = false;
            this.speCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCost.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCost.Name = "speCost";
            // 
            // colDescuento
            // 
            this.colDescuento.Caption = "Ajuste";
            this.colDescuento.ColumnEdit = this.spDescuento;
            this.colDescuento.DisplayFormat.FormatString = "n2";
            this.colDescuento.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDescuento.FieldName = "Descuento";
            this.colDescuento.Name = "colDescuento";
            this.colDescuento.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Descuento", "{0:#,0.00;(#,0.00)}")});
            this.colDescuento.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colDescuento.Visible = true;
            this.colDescuento.VisibleIndex = 7;
            this.colDescuento.Width = 45;
            // 
            // spDescuento
            // 
            this.spDescuento.AutoHeight = false;
            this.spDescuento.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spDescuento.MaxValue = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.spDescuento.Name = "spDescuento";
            // 
            // colVentaNeta
            // 
            this.colVentaNeta.Caption = "VentaNeta";
            this.colVentaNeta.DisplayFormat.FormatString = "n2";
            this.colVentaNeta.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colVentaNeta.FieldName = "VentaNeta";
            this.colVentaNeta.Name = "colVentaNeta";
            this.colVentaNeta.OptionsColumn.AllowEdit = false;
            this.colVentaNeta.OptionsColumn.AllowFocus = false;
            this.colVentaNeta.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "VentaNeta", "{0:#,0.00;(#,0.00)}")});
            this.colVentaNeta.UnboundExpression = "[PrecioTotal] + [Descuento]";
            this.colVentaNeta.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colVentaNeta.Visible = true;
            this.colVentaNeta.VisibleIndex = 8;
            // 
            // colTaxValue
            // 
            this.colTaxValue.Caption = "IVA";
            this.colTaxValue.ColumnEdit = this.spTax;
            this.colTaxValue.DisplayFormat.FormatString = "n2";
            this.colTaxValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTaxValue.FieldName = "ImpuestoTotal";
            this.colTaxValue.Name = "colTaxValue";
            this.colTaxValue.OptionsColumn.AllowEdit = false;
            this.colTaxValue.OptionsColumn.AllowFocus = false;
            this.colTaxValue.OptionsColumn.ReadOnly = true;
            this.colTaxValue.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ImpuestoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colTaxValue.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTaxValue.Visible = true;
            this.colTaxValue.VisibleIndex = 9;
            this.colTaxValue.Width = 41;
            // 
            // spTax
            // 
            this.spTax.AutoHeight = false;
            this.spTax.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spTax.DisplayFormat.FormatString = "n2";
            this.spTax.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.EditFormat.FormatString = "n2";
            this.spTax.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.Name = "spTax";
            // 
            // colTotal
            // 
            this.colTotal.Caption = "TOTAL";
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.AllowEdit = false;
            this.colTotal.OptionsColumn.AllowFocus = false;
            this.colTotal.OptionsColumn.ReadOnly = true;
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:#,0.00;(#,0.00)}")});
            this.colTotal.UnboundExpression = "([PrecioTotal] + [Descuento]) + [ImpuestoTotal]";
            this.colTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 10;
            this.colTotal.Width = 64;
            // 
            // colAplicaIVA
            // 
            this.colAplicaIVA.FieldName = "EsManejo";
            this.colAplicaIVA.Name = "colAplicaIVA";
            // 
            // colCosto
            // 
            this.colCosto.FieldName = "CostoSalida";
            this.colCosto.Name = "colCosto";
            // 
            // ColSiempreSalidaVentas
            // 
            this.ColSiempreSalidaVentas.FieldName = "DetalleCombustible";
            this.ColSiempreSalidaVentas.Name = "ColSiempreSalidaVentas";
            // 
            // ColEsProducto
            // 
            this.ColEsProducto.FieldName = "EsProducto";
            this.ColEsProducto.Name = "ColEsProducto";
            // 
            // cboImpuestos
            // 
            this.cboImpuestos.AutoHeight = false;
            this.cboImpuestos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboImpuestos.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.cboImpuestos.Name = "cboImpuestos";
            this.cboImpuestos.NullText = "";
            this.cboImpuestos.ShowHeader = false;
            // 
            // speCantidad
            // 
            this.speCantidad.AutoHeight = false;
            this.speCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCantidad.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // spSubTotal
            // 
            this.spSubTotal.AutoHeight = false;
            this.spSubTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spSubTotal.Name = "spSubTotal";
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
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.lblGrandTotal);
            this.panelControl2.Controls.Add(this.txtGrandTotal);
            this.panelControl2.Controls.Add(this.bntNew);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Controls.Add(this.btnShowComprobante);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 526);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(936, 35);
            this.panelControl2.TabIndex = 1;
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
            // lblGrandTotal
            // 
            this.lblGrandTotal.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Horizontal;
            this.lblGrandTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGrandTotal.Location = new System.Drawing.Point(590, 2);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Size = new System.Drawing.Size(85, 29);
            this.lblGrandTotal.TabIndex = 3;
            this.lblGrandTotal.Text = "TOTAL ";
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.txtGrandTotal.EditValue = "0.00";
            this.txtGrandTotal.Location = new System.Drawing.Point(675, 2);
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.txtGrandTotal.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtGrandTotal.Properties.ReadOnly = true;
            this.txtGrandTotal.Size = new System.Drawing.Size(146, 30);
            this.txtGrandTotal.TabIndex = 2;
            this.txtGrandTotal.EditValueChanged += new System.EventHandler(this.txtGrandTotal_EditValueChanged);
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
            // btnShowComprobante
            // 
            this.btnShowComprobante.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowComprobante.Image = ((System.Drawing.Image)(resources.GetObject("btnShowComprobante.Image")));
            this.btnShowComprobante.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnShowComprobante.Location = new System.Drawing.Point(821, 2);
            this.btnShowComprobante.LookAndFeel.SkinName = "McSkin";
            this.btnShowComprobante.Name = "btnShowComprobante";
            this.btnShowComprobante.Size = new System.Drawing.Size(113, 31);
            this.btnShowComprobante.TabIndex = 4;
            this.btnShowComprobante.Text = "Mostrar \r\nComprobante";
            this.btnShowComprobante.Click += new System.EventHandler(this.btnShowComprobante_Click);
            // 
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // infBloqueo
            // 
            this.infBloqueo.ContainerControl = this;
            // 
            // errTC_Periodo
            // 
            this.errTC_Periodo.ContainerControl = this;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Código";
            this.gridColumn5.FieldName = "Codigo";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Producto";
            this.gridColumn6.FieldName = "Nombre";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Código";
            this.gridColumn8.FieldName = "Codigo";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 0;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Producto";
            this.gridColumn9.FieldName = "Nombre";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            // 
            // DialogVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 561);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lbCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlazo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaFactura.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReferencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpCalcCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrecio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spDescuento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGrandTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infBloqueo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.TextEdit txtReferencia;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.MemoEdit mmoComentario;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infBloqueo;
        private DevExpress.XtraEditors.DateEdit dateFechaFactura;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacen;
        private DevExpress.XtraEditors.LabelControl lblGrandTotal;
        private DevExpress.XtraEditors.TextEdit txtGrandTotal;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraEditors.TextEdit txtPlazo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.DateEdit dateFechaVencimiento;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.GridLookUpEdit glkClient;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spDescuento;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colProduct;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacen;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadInicial;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colPrecio;
        private DevExpress.XtraGrid.Columns.GridColumn colSubTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colTaxValue;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colDescuento;
        private DevExpress.XtraGrid.Columns.GridColumn colVentaNeta;
        private DevExpress.XtraEditors.LookUpEdit lkArea;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaIVA;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spPrecio;
        private DevExpress.XtraGrid.Columns.GridColumn colCosto;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit rpCalcCantidad;
        private DevExpress.XtraGrid.Columns.GridColumn ColSiempreSalidaVentas;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn ColEsProducto;
        private DevExpress.XtraEditors.ListBoxControl lbCliente;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
    }
}