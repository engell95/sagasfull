namespace SAGAS.Ventas.Forms.Dialogs
{
    partial class DialogVentaCreditoCombustible
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogVentaCreditoCombustible));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.chkDescuento = new DevExpress.XtraEditors.CheckEdit();
            this.dateFechaVencimiento = new DevExpress.XtraEditors.DateEdit();
            this.lbResumen = new DevExpress.XtraEditors.ListBoxControl();
            this.lbCliente = new DevExpress.XtraEditors.ListBoxControl();
            this.glkClient = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.memoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.dateFechaVenta = new DevExpress.XtraEditors.DateEdit();
            this.txtNumero = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupRD = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ColAlmacenSalida = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacenSalida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExistencia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrecio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescuento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spAjuste = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCosto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCantidadTanques = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboAlmacenEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spCostUnit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnShowComprobante = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkDescuento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbResumen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVenta.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spAjuste)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.chkDescuento);
            this.layoutControlTop.Controls.Add(this.dateFechaVencimiento);
            this.layoutControlTop.Controls.Add(this.lbResumen);
            this.layoutControlTop.Controls.Add(this.lbCliente);
            this.layoutControlTop.Controls.Add(this.glkClient);
            this.layoutControlTop.Controls.Add(this.memoComentario);
            this.layoutControlTop.Controls.Add(this.dateFechaVenta);
            this.layoutControlTop.Controls.Add(this.txtNumero);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 145);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // chkDescuento
            // 
            this.chkDescuento.Location = new System.Drawing.Point(666, 114);
            this.chkDescuento.Name = "chkDescuento";
            this.chkDescuento.Properties.Caption = "Ajuste Centavo";
            this.chkDescuento.Size = new System.Drawing.Size(254, 19);
            this.chkDescuento.StyleController = this.layoutControlTop;
            this.chkDescuento.TabIndex = 7;
            this.chkDescuento.CheckedChanged += new System.EventHandler(this.chkDescuento_CheckedChanged);
            this.chkDescuento.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.chkDescuento_EditValueChanging);
            // 
            // dateFechaVencimiento
            // 
            this.dateFechaVencimiento.EditValue = null;
            this.dateFechaVencimiento.Enabled = false;
            this.dateFechaVencimiento.Location = new System.Drawing.Point(426, 76);
            this.dateFechaVencimiento.Name = "dateFechaVencimiento";
            this.dateFechaVencimiento.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.dateFechaVencimiento.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.dateFechaVencimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaVencimiento.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaVencimiento.Size = new System.Drawing.Size(233, 20);
            this.dateFechaVencimiento.StyleController = this.layoutControlTop;
            this.dateFechaVencimiento.TabIndex = 3;
            // 
            // lbResumen
            // 
            this.lbResumen.Appearance.BackColor = System.Drawing.Color.White;
            this.lbResumen.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lbResumen.Appearance.Options.UseBackColor = true;
            this.lbResumen.Appearance.Options.UseForeColor = true;
            this.lbResumen.Enabled = false;
            this.lbResumen.Location = new System.Drawing.Point(666, 50);
            this.lbResumen.Name = "lbResumen";
            this.lbResumen.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbResumen.Size = new System.Drawing.Size(254, 60);
            this.lbResumen.StyleController = this.layoutControlTop;
            this.lbResumen.TabIndex = 7;
            this.lbResumen.TabStop = false;
            // 
            // lbCliente
            // 
            this.lbCliente.Appearance.BackColor = System.Drawing.Color.White;
            this.lbCliente.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lbCliente.Appearance.Options.UseBackColor = true;
            this.lbCliente.Appearance.Options.UseForeColor = true;
            this.lbCliente.Enabled = false;
            this.lbCliente.Location = new System.Drawing.Point(9, 52);
            this.lbCliente.Name = "lbCliente";
            this.lbCliente.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.lbCliente.Size = new System.Drawing.Size(298, 84);
            this.lbCliente.StyleController = this.layoutControlTop;
            this.lbCliente.TabIndex = 6;
            this.lbCliente.TabStop = false;
            // 
            // glkClient
            // 
            this.glkClient.Location = new System.Drawing.Point(124, 28);
            this.glkClient.Name = "glkClient";
            this.glkClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkClient.Properties.DisplayMember = "Display";
            this.glkClient.Properties.NullText = "<Seleccione el Cliente>";
            this.glkClient.Properties.ValueMember = "ID";
            this.glkClient.Properties.View = this.gridLookUpEdit1View;
            this.glkClient.Size = new System.Drawing.Size(183, 20);
            this.glkClient.StyleController = this.layoutControlTop;
            this.glkClient.TabIndex = 0;
            this.glkClient.EditValueChanged += new System.EventHandler(this.glkClient_EditValueChanged);
            this.glkClient.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.dateFechaEntrada_EditValueChanging);
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
            // memoComentario
            // 
            this.memoComentario.Location = new System.Drawing.Point(426, 100);
            this.memoComentario.Name = "memoComentario";
            this.memoComentario.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.memoComentario.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.memoComentario.Size = new System.Drawing.Size(233, 36);
            this.memoComentario.StyleController = this.layoutControlTop;
            this.memoComentario.TabIndex = 4;
            this.memoComentario.UseOptimizedRendering = true;
            // 
            // dateFechaVenta
            // 
            this.dateFechaVenta.EditValue = null;
            this.dateFechaVenta.Location = new System.Drawing.Point(426, 28);
            this.dateFechaVenta.Name = "dateFechaVenta";
            this.dateFechaVenta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaVenta.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaVenta.Size = new System.Drawing.Size(233, 20);
            this.dateFechaVenta.StyleController = this.layoutControlTop;
            this.dateFechaVenta.TabIndex = 1;
            this.dateFechaVenta.EditValueChanged += new System.EventHandler(this.dateFechaEntrada_EditValueChanged);
            this.dateFechaVenta.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.dateFechaEntrada_EditValueChanging);
            this.dateFechaVenta.Validated += new System.EventHandler(this.dateFechaVenta_Validated);
            // 
            // txtNumero
            // 
            this.txtNumero.EditValue = "";
            this.txtNumero.Location = new System.Drawing.Point(426, 52);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Properties.DisplayFormat.FormatString = "f0";
            this.txtNumero.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNumero.Properties.EditFormat.FormatString = "f0";
            this.txtNumero.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNumero.Properties.Mask.EditMask = "f0";
            this.txtNumero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNumero.Size = new System.Drawing.Size(233, 20);
            this.txtNumero.StyleController = this.layoutControlTop;
            this.txtNumero.TabIndex = 2;
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(932, 145);
            this.layoutControlGroup1.Text = "Datos de la Venta";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem8,
            this.layoutControlItem11,
            this.layoutControlGroupRD,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.ShowTabPageCloseButton = true;
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(924, 118);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtNumero;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(302, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(352, 24);
            this.layoutControlItem1.Text = "Número";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(110, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem8.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.dateFechaVenta;
            this.layoutControlItem8.CustomizationFormText = "Fecha Venta";
            this.layoutControlItem8.Location = new System.Drawing.Point(302, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(352, 24);
            this.layoutControlItem8.Text = "Fecha Venta";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(110, 13);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.memoComentario;
            this.layoutControlItem11.CustomizationFormText = "Dirección";
            this.layoutControlItem11.Location = new System.Drawing.Point(302, 72);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(352, 40);
            this.layoutControlItem11.Text = "Comentario";
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(110, 20);
            this.layoutControlItem11.TextToControlDistance = 5;
            // 
            // layoutControlGroupRD
            // 
            this.layoutControlGroupRD.CustomizationFormText = "Resumen de Arqueo";
            this.layoutControlGroupRD.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroupRD.Location = new System.Drawing.Point(654, 0);
            this.layoutControlGroupRD.Name = "layoutControlGroupRD";
            this.layoutControlGroupRD.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupRD.Size = new System.Drawing.Size(264, 112);
            this.layoutControlGroupRD.Text = "Resumen de Arqueo";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.lbResumen;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(258, 64);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.chkDescuento;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 64);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(258, 23);
            this.layoutControlItem6.Text = "layoutControlItem6";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextToControlDistance = 0;
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.lbCliente;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(302, 88);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem2.Control = this.glkClient;
            this.layoutControlItem2.CustomizationFormText = "Cliente";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(302, 24);
            this.layoutControlItem2.Text = "Cliente";
            this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(110, 20);
            this.layoutControlItem2.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.dateFechaVencimiento;
            this.layoutControlItem3.CustomizationFormText = "Fecha Vencimiento";
            this.layoutControlItem3.Location = new System.Drawing.Point(302, 48);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(352, 24);
            this.layoutControlItem3.Text = "Fecha Vencimiento";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(110, 20);
            this.layoutControlItem3.TextToControlDistance = 5;
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
            this.gridDetalle.Location = new System.Drawing.Point(2, 147);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speCantidad,
            this.speCost,
            this.cboUnidadMedida,
            this.gridProductos,
            this.cboAlmacenEntrada,
            this.spSubTotal,
            this.spMontoISC,
            this.spCostUnit,
            this.cboAlmacenSalida,
            this.repositoryItemButtonEdit1,
            this.spAjuste});
            this.gridDetalle.Size = new System.Drawing.Size(932, 364);
            this.gridDetalle.TabIndex = 1;
            this.gridDetalle.UseEmbeddedNavigator = true;
            this.gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // gvDetalle
            // 
            this.gvDetalle.ActiveFilterEnabled = false;
            this.gvDetalle.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDetalle.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProduct,
            this.colUnit,
            this.ColAlmacenSalida,
            this.colQuantity,
            this.colID,
            this.gridColumn8,
            this.colExistencia,
            this.colPrecio,
            this.gridColumn11,
            this.colDescuento,
            this.colCosto,
            this.colCantidadTanques});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
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
            this.gvDetalle.LostFocus += new System.EventHandler(this.gvDetalle_LostFocus);
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.ColumnEdit = this.gridProductos;
            this.colProduct.FieldName = "ProductoID";
            this.colProduct.Name = "colProduct";
            this.colProduct.Visible = true;
            this.colProduct.VisibleIndex = 0;
            this.colProduct.Width = 165;
            // 
            // gridProductos
            // 
            this.gridProductos.AutoHeight = false;
            this.gridProductos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridProductos.DisplayMember = "Display";
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.ValueMember = "ID";
            this.gridProductos.View = this.gridViewProductos;
            // 
            // gridViewProductos
            // 
            this.gridViewProductos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gridViewProductos.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewProductos.Name = "gridViewProductos";
            this.gridViewProductos.OptionsBehavior.Editable = false;
            this.gridViewProductos.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewProductos.OptionsView.ShowAutoFilterRow = true;
            this.gridViewProductos.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "ID";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Código";
            this.gridColumn5.FieldName = "Codigo";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 206;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Producto";
            this.gridColumn6.FieldName = "Nombre";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 434;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Und.  Medida";
            this.gridColumn7.FieldName = "UmidadName";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 176;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Unidad de Medida";
            this.colUnit.ColumnEdit = this.cboUnidadMedida;
            this.colUnit.FieldName = "UnidadMedidaID";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.AllowEdit = false;
            this.colUnit.OptionsColumn.AllowFocus = false;
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 1;
            this.colUnit.Width = 102;
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
            // ColAlmacenSalida
            // 
            this.ColAlmacenSalida.Caption = "Tanque";
            this.ColAlmacenSalida.ColumnEdit = this.cboAlmacenSalida;
            this.ColAlmacenSalida.FieldName = "AlmacenSalidaID";
            this.ColAlmacenSalida.Name = "ColAlmacenSalida";
            this.ColAlmacenSalida.Visible = true;
            this.ColAlmacenSalida.VisibleIndex = 2;
            this.ColAlmacenSalida.Width = 97;
            // 
            // cboAlmacenSalida
            // 
            this.cboAlmacenSalida.AutoHeight = false;
            this.cboAlmacenSalida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacenSalida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Tanque")});
            this.cboAlmacenSalida.DisplayMember = "Display";
            this.cboAlmacenSalida.Name = "cboAlmacenSalida";
            this.cboAlmacenSalida.NullText = "";
            this.cboAlmacenSalida.ValueMember = "ID";
            // 
            // colQuantity
            // 
            this.colQuantity.AppearanceHeader.Options.UseTextOptions = true;
            this.colQuantity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colQuantity.Caption = "Litros";
            this.colQuantity.ColumnEdit = this.speCantidad;
            this.colQuantity.DisplayFormat.FormatString = "n3";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "CantidadSalida";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CantidadSalida", "{0:N3}")});
            this.colQuantity.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 4;
            this.colQuantity.Width = 71;
            // 
            // speCantidad
            // 
            this.speCantidad.AutoHeight = false;
            this.speCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCantidad.DisplayFormat.FormatString = "n3";
            this.speCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.EditFormat.FormatString = "n3";
            this.speCantidad.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn8.Caption = "TOTAL";
            this.gridColumn8.DisplayFormat.FormatString = "N2";
            this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn8.FieldName = "PrecioTotal";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "PrecioTotal", "{0:N2}")});
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 8;
            this.gridColumn8.Width = 89;
            // 
            // colExistencia
            // 
            this.colExistencia.AppearanceCell.Options.UseTextOptions = true;
            this.colExistencia.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colExistencia.Caption = "Existencia";
            this.colExistencia.DisplayFormat.FormatString = "N3";
            this.colExistencia.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colExistencia.FieldName = "CantidadInicial";
            this.colExistencia.Name = "colExistencia";
            this.colExistencia.OptionsColumn.AllowEdit = false;
            this.colExistencia.OptionsColumn.AllowFocus = false;
            this.colExistencia.Visible = true;
            this.colExistencia.VisibleIndex = 3;
            this.colExistencia.Width = 92;
            // 
            // colPrecio
            // 
            this.colPrecio.AppearanceHeader.Options.UseTextOptions = true;
            this.colPrecio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPrecio.Caption = "Precio";
            this.colPrecio.DisplayFormat.FormatString = "N2";
            this.colPrecio.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPrecio.FieldName = "Precio";
            this.colPrecio.Name = "colPrecio";
            this.colPrecio.OptionsColumn.AllowEdit = false;
            this.colPrecio.OptionsColumn.AllowFocus = false;
            this.colPrecio.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colPrecio.Visible = true;
            this.colPrecio.VisibleIndex = 5;
            this.colPrecio.Width = 63;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn11.Caption = "SubTotal";
            this.gridColumn11.DisplayFormat.FormatString = "N2";
            this.gridColumn11.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn11.FieldName = "SubTotal";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            this.gridColumn11.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "SubTotal", "{0:N2}")});
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 6;
            this.gridColumn11.Width = 72;
            // 
            // colDescuento
            // 
            this.colDescuento.AppearanceHeader.Options.UseTextOptions = true;
            this.colDescuento.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colDescuento.Caption = "Ajuste";
            this.colDescuento.ColumnEdit = this.spAjuste;
            this.colDescuento.DisplayFormat.FormatString = "N2";
            this.colDescuento.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDescuento.FieldName = "Descuento";
            this.colDescuento.Name = "colDescuento";
            this.colDescuento.OptionsColumn.AllowEdit = false;
            this.colDescuento.OptionsColumn.AllowFocus = false;
            this.colDescuento.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Descuento", "{0:N2}")});
            this.colDescuento.Visible = true;
            this.colDescuento.VisibleIndex = 7;
            this.colDescuento.Width = 68;
            // 
            // spAjuste
            // 
            this.spAjuste.AutoHeight = false;
            this.spAjuste.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spAjuste.DisplayFormat.FormatString = "N2";
            this.spAjuste.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spAjuste.EditFormat.FormatString = "N2";
            this.spAjuste.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spAjuste.Mask.EditMask = "N2";
            this.spAjuste.MaxValue = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.spAjuste.MinValue = new decimal(new int[] {
            5,
            0,
            0,
            -2147352576});
            this.spAjuste.Name = "spAjuste";
            // 
            // colCosto
            // 
            this.colCosto.FieldName = "CostoSalida";
            this.colCosto.Name = "colCosto";
            // 
            // colCantidadTanques
            // 
            this.colCantidadTanques.FieldName = "CantidadEntrada";
            this.colCantidadTanques.Name = "colCantidadTanques";
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
            // speCost
            // 
            this.speCost.AutoHeight = false;
            this.speCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCost.DisplayFormat.FormatString = "2";
            this.speCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.EditFormat.FormatString = "2";
            this.speCost.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCost.Name = "speCost";
            // 
            // cboAlmacenEntrada
            // 
            this.cboAlmacenEntrada.AutoHeight = false;
            this.cboAlmacenEntrada.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacenEntrada.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen / Tanque")});
            this.cboAlmacenEntrada.DisplayMember = "Display";
            this.cboAlmacenEntrada.Name = "cboAlmacenEntrada";
            this.cboAlmacenEntrada.NullText = "";
            this.cboAlmacenEntrada.ValueMember = "ID";
            // 
            // spSubTotal
            // 
            this.spSubTotal.AutoHeight = false;
            this.spSubTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spSubTotal.Name = "spSubTotal";
            // 
            // spMontoISC
            // 
            this.spMontoISC.AutoHeight = false;
            this.spMontoISC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spMontoISC.Name = "spMontoISC";
            // 
            // spCostUnit
            // 
            this.spCostUnit.AllowMouseWheel = false;
            this.spCostUnit.AutoHeight = false;
            this.spCostUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spCostUnit.DisplayFormat.FormatString = "n4";
            this.spCostUnit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.EditFormat.FormatString = "n4";
            this.spCostUnit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.Name = "spCostUnit";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
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
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.bntNew);
            this.panelControl2.Controls.Add(this.btnOK);
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
            this.btnShowComprobante.Location = new System.Drawing.Point(821, 2);
            this.btnShowComprobante.LookAndFeel.SkinName = "McSkin";
            this.btnShowComprobante.Name = "btnShowComprobante";
            this.btnShowComprobante.Size = new System.Drawing.Size(113, 31);
            this.btnShowComprobante.TabIndex = 6;
            this.btnShowComprobante.Text = "Mostrar \r\nComprobante";
            this.btnShowComprobante.Click += new System.EventHandler(this.btnShowComprobante_Click);
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
            // DialogVentaCreditoCombustible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 561);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogVentaCreditoCombustible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chkDescuento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVencimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbResumen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVenta.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaVenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupRD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spAjuste)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit txtNumero;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.DateEdit dateFechaVenta;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenEntrada;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraEditors.MemoEdit memoComentario;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCostUnit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colProduct;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacenSalida;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenSalida;
        private DevExpress.XtraEditors.GridLookUpEdit glkClient;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.ListBoxControl lbResumen;
        private DevExpress.XtraEditors.ListBoxControl lbCliente;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupRD;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.Columns.GridColumn colExistencia;
        private DevExpress.XtraGrid.Columns.GridColumn colPrecio;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn colDescuento;
        private DevExpress.XtraEditors.DateEdit dateFechaVencimiento;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit chkDescuento;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraGrid.Columns.GridColumn colCosto;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spAjuste;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadTanques;
    }
}