namespace SAGAS.Inventario.Forms.Dialogs
{
    partial class DialogDevolucionConversion
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogDevolucionConversion));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.txtReferencia = new DevExpress.XtraEditors.TextEdit();
            this.GlkFactura = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spMonto = new DevExpress.XtraEditors.SpinEdit();
            this.rgOptions = new DevExpress.XtraEditors.RadioGroup();
            this.glkProvee = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spTC = new DevExpress.XtraEditors.SpinEdit();
            this.memoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.dateFechaOrden = new DevExpress.XtraEditors.DateEdit();
            this.txtNumero = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlProv = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemFactura = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemConversion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlRef = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControlTop = new DevExpress.XtraEditors.GroupControl();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.colExistencia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostoInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCostoTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colIva = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spIva = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.cboAlmacenEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spTax = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.speSubtotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.groupControlBottom = new DevExpress.XtraEditors.GroupControl();
            this.gridConversion = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalleC = new System.Windows.Forms.BindingSource();
            this.gvConversion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductoC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridProductosC = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnidadC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboUnidadMedidaC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colAlmacenC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkAlmacenC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colCantidadC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostoUnitarioC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCostoTotalC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpCTotalConvercion = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemLookUpEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemSpinEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemSpinEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemSpinEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemLookUpEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnShowComprobante = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtReferencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GlkFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMonto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgOptions.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkProvee.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaOrden.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaOrden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFactura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemConversion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlRef)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).BeginInit();
            this.groupControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubtotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlBottom)).BeginInit();
            this.groupControlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConversion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalleC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConversion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductosC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedidaC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkAlmacenC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCTotalConvercion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.txtReferencia);
            this.layoutControlTop.Controls.Add(this.GlkFactura);
            this.layoutControlTop.Controls.Add(this.spMonto);
            this.layoutControlTop.Controls.Add(this.rgOptions);
            this.layoutControlTop.Controls.Add(this.glkProvee);
            this.layoutControlTop.Controls.Add(this.spTC);
            this.layoutControlTop.Controls.Add(this.memoComentario);
            this.layoutControlTop.Controls.Add(this.dateFechaOrden);
            this.layoutControlTop.Controls.Add(this.txtNumero);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 107);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // txtReferencia
            // 
            this.txtReferencia.EditValue = "";
            this.txtReferencia.Location = new System.Drawing.Point(237, 52);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Size = new System.Drawing.Size(203, 20);
            this.txtReferencia.StyleController = this.layoutControlTop;
            this.txtReferencia.TabIndex = 7;
            // 
            // GlkFactura
            // 
            this.GlkFactura.Location = new System.Drawing.Point(692, 52);
            this.GlkFactura.Name = "GlkFactura";
            this.GlkFactura.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.GlkFactura.Properties.DisplayMember = "Display";
            this.GlkFactura.Properties.NullText = "<Seleccione la Factura>";
            this.GlkFactura.Properties.ValueMember = "ID";
            this.GlkFactura.Properties.View = this.gridView1;
            this.GlkFactura.Size = new System.Drawing.Size(231, 20);
            this.GlkFactura.StyleController = this.layoutControlTop;
            this.GlkFactura.TabIndex = 8;
            this.GlkFactura.EditValueChanged += new System.EventHandler(this.GlkFactura_EditValueChanged);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn21,
            this.gridColumn22});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn21, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn15
            // 
            this.gridColumn15.FieldName = "ID";
            this.gridColumn15.Name = "gridColumn15";
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "Referencia";
            this.gridColumn16.FieldName = "Referencia";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 0;
            this.gridColumn16.Width = 171;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "Fecha";
            this.gridColumn21.FieldName = "FechaRegistro";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 1;
            this.gridColumn21.Width = 389;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "Monto";
            this.gridColumn22.DisplayFormat.FormatString = "n2";
            this.gridColumn22.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn22.FieldName = "Monto";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 2;
            this.gridColumn22.Width = 298;
            // 
            // spMonto
            // 
            this.spMonto.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spMonto.Location = new System.Drawing.Point(692, 76);
            this.spMonto.Name = "spMonto";
            this.spMonto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spMonto.Properties.DisplayFormat.FormatString = "n2";
            this.spMonto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spMonto.Properties.ReadOnly = true;
            this.spMonto.Size = new System.Drawing.Size(231, 20);
            this.spMonto.StyleController = this.layoutControlTop;
            this.spMonto.TabIndex = 7;
            this.spMonto.EditValueChanged += new System.EventHandler(this.spMonto_EditValueChanged);
            // 
            // rgOptions
            // 
            this.rgOptions.Location = new System.Drawing.Point(9, 44);
            this.rgOptions.Name = "rgOptions";
            this.rgOptions.Properties.Columns = 2;
            this.rgOptions.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Devolución"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Conversión")});
            this.rgOptions.Size = new System.Drawing.Size(199, 28);
            this.rgOptions.StyleController = this.layoutControlTop;
            this.rgOptions.TabIndex = 7;
            this.rgOptions.SelectedIndexChanged += new System.EventHandler(this.rgOptions_SelectedIndexChanged);
            // 
            // glkProvee
            // 
            this.glkProvee.Location = new System.Drawing.Point(692, 28);
            this.glkProvee.Name = "glkProvee";
            this.glkProvee.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkProvee.Properties.DisplayMember = "Display";
            this.glkProvee.Properties.NullText = "<Seleccione el Proveedor>";
            this.glkProvee.Properties.ValueMember = "ID";
            this.glkProvee.Properties.View = this.gridLookUpEdit1View;
            this.glkProvee.Size = new System.Drawing.Size(231, 20);
            this.glkProvee.StyleController = this.layoutControlTop;
            this.glkProvee.TabIndex = 7;
            this.glkProvee.EditValueChanged += new System.EventHandler(this.glkProvee_EditValueChanged);
            this.glkProvee.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.glkProvee_EditValueChanging);
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
            this.gridColumn3.Caption = "Proveedor";
            this.gridColumn3.FieldName = "Nombre";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 640;
            // 
            // spTC
            // 
            this.spTC.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spTC.Location = new System.Drawing.Point(529, 28);
            this.spTC.Name = "spTC";
            this.spTC.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spTC.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spTC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spTC.Properties.DisplayFormat.FormatString = "n4";
            this.spTC.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTC.Properties.EditFormat.FormatString = "n4";
            this.spTC.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTC.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spTC.Properties.ReadOnly = true;
            this.spTC.Size = new System.Drawing.Size(64, 20);
            this.spTC.StyleController = this.layoutControlTop;
            this.spTC.TabIndex = 6;
            // 
            // memoComentario
            // 
            this.memoComentario.Location = new System.Drawing.Point(124, 76);
            this.memoComentario.Name = "memoComentario";
            this.memoComentario.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.memoComentario.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.memoComentario.Size = new System.Drawing.Size(469, 22);
            this.memoComentario.StyleController = this.layoutControlTop;
            this.memoComentario.TabIndex = 7;
            this.memoComentario.UseOptimizedRendering = true;
            // 
            // dateFechaOrden
            // 
            this.dateFechaOrden.EditValue = null;
            this.dateFechaOrden.Location = new System.Drawing.Point(327, 28);
            this.dateFechaOrden.Name = "dateFechaOrden";
            this.dateFechaOrden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaOrden.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaOrden.Size = new System.Drawing.Size(113, 20);
            this.dateFechaOrden.StyleController = this.layoutControlTop;
            this.dateFechaOrden.TabIndex = 4;
            this.dateFechaOrden.EditValueChanged += new System.EventHandler(this.dateFechaIngreso_Validated);
            this.dateFechaOrden.Validated += new System.EventHandler(this.dateFechaIngreso_Validated);
            // 
            // txtNumero
            // 
            this.txtNumero.EditValue = "";
            this.txtNumero.Enabled = false;
            this.txtNumero.Location = new System.Drawing.Point(529, 52);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Properties.Mask.EditMask = "999999999";
            this.txtNumero.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtNumero.Size = new System.Drawing.Size(64, 20);
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(932, 107);
            this.layoutControlGroup1.Text = "Encabezado del Movimiento";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem8,
            this.layoutControlProv,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItemFactura,
            this.layoutControlItem1,
            this.layoutControlItemConversion,
            this.layoutControlItem11,
            this.layoutControlRef});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.ShowTabPageCloseButton = true;
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(924, 80);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem8.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.dateFechaOrden;
            this.layoutControlItem8.CustomizationFormText = "Fecha";
            this.layoutControlItem8.Location = new System.Drawing.Point(203, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(232, 24);
            this.layoutControlItem8.Text = "Fecha";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(110, 13);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // layoutControlProv
            // 
            this.layoutControlProv.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlProv.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlProv.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlProv.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlProv.Control = this.glkProvee;
            this.layoutControlProv.CustomizationFormText = "Proveedor";
            this.layoutControlProv.Location = new System.Drawing.Point(588, 0);
            this.layoutControlProv.Name = "layoutControlProv";
            this.layoutControlProv.Size = new System.Drawing.Size(330, 24);
            this.layoutControlProv.Text = "Proveedor";
            this.layoutControlProv.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.spTC;
            this.layoutControlItem3.CustomizationFormText = "T/C";
            this.layoutControlItem3.Location = new System.Drawing.Point(435, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(153, 24);
            this.layoutControlItem3.Text = "T/C";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(80, 20);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.rgOptions;
            this.layoutControlItem4.CustomizationFormText = "Tipo de Movimiento";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(203, 48);
            this.layoutControlItem4.Text = "Tipo de Movimiento";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItemFactura
            // 
            this.layoutControlItemFactura.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemFactura.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemFactura.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemFactura.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItemFactura.Control = this.GlkFactura;
            this.layoutControlItemFactura.CustomizationFormText = "Factura";
            this.layoutControlItemFactura.Location = new System.Drawing.Point(588, 24);
            this.layoutControlItemFactura.Name = "layoutControlItemFactura";
            this.layoutControlItemFactura.Size = new System.Drawing.Size(330, 24);
            this.layoutControlItemFactura.Text = "Factura";
            this.layoutControlItemFactura.TextSize = new System.Drawing.Size(92, 13);
            this.layoutControlItemFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.txtNumero;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(435, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(153, 24);
            this.layoutControlItem1.Text = "Número";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(80, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItemConversion
            // 
            this.layoutControlItemConversion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemConversion.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemConversion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemConversion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItemConversion.Control = this.spMonto;
            this.layoutControlItemConversion.CustomizationFormText = "Monto";
            this.layoutControlItemConversion.Location = new System.Drawing.Point(588, 48);
            this.layoutControlItemConversion.Name = "layoutControlItemConversion";
            this.layoutControlItemConversion.Size = new System.Drawing.Size(330, 26);
            this.layoutControlItemConversion.Text = "Monto";
            this.layoutControlItemConversion.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem11.Control = this.memoComentario;
            this.layoutControlItem11.CustomizationFormText = "Dirección";
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.Size = new System.Drawing.Size(588, 26);
            this.layoutControlItem11.Text = "Comentario";
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(110, 20);
            this.layoutControlItem11.TextToControlDistance = 5;
            // 
            // layoutControlRef
            // 
            this.layoutControlRef.Control = this.txtReferencia;
            this.layoutControlRef.CustomizationFormText = "Ref.";
            this.layoutControlRef.Location = new System.Drawing.Point(203, 24);
            this.layoutControlRef.Name = "layoutControlRef";
            this.layoutControlRef.Size = new System.Drawing.Size(232, 24);
            this.layoutControlRef.Text = "Ref.";
            this.layoutControlRef.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlRef.TextSize = new System.Drawing.Size(20, 20);
            this.layoutControlRef.TextToControlDistance = 5;
            this.layoutControlRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(936, 526);
            this.panelControl1.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 109);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.CaptionLocation = DevExpress.Utils.Locations.Top;
            this.splitContainerControl1.Panel1.Controls.Add(this.groupControlTop);
            this.splitContainerControl1.Panel1.ShowCaption = true;
            this.splitContainerControl1.Panel1.Text = "Salida";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControlBottom);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.ShowCaption = true;
            this.splitContainerControl1.Size = new System.Drawing.Size(932, 402);
            this.splitContainerControl1.SplitterPosition = 221;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // groupControlTop
            // 
            this.groupControlTop.Controls.Add(this.gridDetalle);
            this.groupControlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlTop.Location = new System.Drawing.Point(0, 0);
            this.groupControlTop.Name = "groupControlTop";
            this.groupControlTop.Size = new System.Drawing.Size(932, 221);
            this.groupControlTop.TabIndex = 9;
            this.groupControlTop.Text = "Devolución";
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
            this.gridDetalle.Location = new System.Drawing.Point(2, 21);
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
            this.spTax,
            this.cboAlmacenSalida,
            this.spIva,
            this.speSubtotal,
            this.speTotal});
            this.gridDetalle.Size = new System.Drawing.Size(928, 198);
            this.gridDetalle.TabIndex = 8;
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
            this.colID,
            this.colProduct,
            this.colUnit,
            this.ColAlmacenSalida,
            this.colExistencia,
            this.colQuantity,
            this.colCostoInicial,
            this.colCost,
            this.colCostoTotal,
            this.colIva,
            this.colTotal});
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
            this.gvDetalle.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gvDetalle_FocusedColumnChanged);
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanging);
            this.gvDetalle.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvDetalle_InvalidRowException);
            this.gvDetalle.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvDetalle_ValidateRow);
            this.gvDetalle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvDetalle_KeyDown);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.ColumnEdit = this.gridProductos;
            this.colProduct.FieldName = "ProductoID";
            this.colProduct.Name = "colProduct";
            this.colProduct.Visible = true;
            this.colProduct.VisibleIndex = 0;
            this.colProduct.Width = 285;
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
            this.colUnit.Width = 118;
            // 
            // cboUnidadMedida
            // 
            this.cboUnidadMedida.AutoHeight = false;
            this.cboUnidadMedida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnidadMedida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.cboUnidadMedida.DisplayMember = "Nombre";
            this.cboUnidadMedida.Name = "cboUnidadMedida";
            this.cboUnidadMedida.NullText = "";
            this.cboUnidadMedida.ValueMember = "ID";
            // 
            // ColAlmacenSalida
            // 
            this.ColAlmacenSalida.Caption = "Almacen";
            this.ColAlmacenSalida.ColumnEdit = this.cboAlmacenSalida;
            this.ColAlmacenSalida.FieldName = "AlmacenSalidaID";
            this.ColAlmacenSalida.Name = "ColAlmacenSalida";
            this.ColAlmacenSalida.Visible = true;
            this.ColAlmacenSalida.VisibleIndex = 2;
            this.ColAlmacenSalida.Width = 87;
            // 
            // cboAlmacenSalida
            // 
            this.cboAlmacenSalida.AutoHeight = false;
            this.cboAlmacenSalida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacenSalida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen")});
            this.cboAlmacenSalida.DisplayMember = "Display";
            this.cboAlmacenSalida.Name = "cboAlmacenSalida";
            this.cboAlmacenSalida.NullText = "";
            this.cboAlmacenSalida.ValueMember = "ID";
            // 
            // colExistencia
            // 
            this.colExistencia.Caption = "Existencia";
            this.colExistencia.DisplayFormat.FormatString = "N3";
            this.colExistencia.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colExistencia.FieldName = "CantidadInicial";
            this.colExistencia.Name = "colExistencia";
            this.colExistencia.OptionsColumn.AllowEdit = false;
            this.colExistencia.OptionsColumn.AllowFocus = false;
            this.colExistencia.Visible = true;
            this.colExistencia.VisibleIndex = 3;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "Cantidad";
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
            this.colQuantity.Width = 96;
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
            999999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // colCostoInicial
            // 
            this.colCostoInicial.Caption = "Ult. Cto.";
            this.colCostoInicial.DisplayFormat.FormatString = "N4";
            this.colCostoInicial.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoInicial.FieldName = "CostoInicial";
            this.colCostoInicial.Name = "colCostoInicial";
            this.colCostoInicial.OptionsColumn.AllowEdit = false;
            this.colCostoInicial.OptionsColumn.AllowFocus = false;
            this.colCostoInicial.OptionsColumn.ReadOnly = true;
            this.colCostoInicial.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoInicial.Visible = true;
            this.colCostoInicial.VisibleIndex = 5;
            // 
            // colCost
            // 
            this.colCost.Caption = "Costo Unitario";
            this.colCost.DisplayFormat.FormatString = "n4";
            this.colCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCost.FieldName = "CostoSalida";
            this.colCost.Name = "colCost";
            this.colCost.OptionsColumn.AllowEdit = false;
            this.colCost.OptionsColumn.AllowFocus = false;
            this.colCost.OptionsColumn.ReadOnly = true;
            this.colCost.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCost.Visible = true;
            this.colCost.VisibleIndex = 6;
            this.colCost.Width = 107;
            // 
            // colCostoTotal
            // 
            this.colCostoTotal.Caption = "Costo Total";
            this.colCostoTotal.ColumnEdit = this.speCost;
            this.colCostoTotal.DisplayFormat.FormatString = "n2";
            this.colCostoTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoTotal.FieldName = "CostoTotal";
            this.colCostoTotal.Name = "colCostoTotal";
            this.colCostoTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CostoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colCostoTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoTotal.Visible = true;
            this.colCostoTotal.VisibleIndex = 7;
            this.colCostoTotal.Width = 143;
            // 
            // speCost
            // 
            this.speCost.AutoHeight = false;
            this.speCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCost.DisplayFormat.FormatString = "n2";
            this.speCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.EditFormat.FormatString = "n2";
            this.speCost.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCost.Name = "speCost";
            // 
            // colIva
            // 
            this.colIva.Caption = "IVA";
            this.colIva.ColumnEdit = this.spIva;
            this.colIva.DisplayFormat.FormatString = "n2";
            this.colIva.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIva.FieldName = "ImpuestoTotal";
            this.colIva.Name = "colIva";
            this.colIva.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ImpuestoTotal", "{0:N2}")});
            this.colIva.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colIva.Visible = true;
            this.colIva.VisibleIndex = 8;
            // 
            // spIva
            // 
            this.spIva.AutoHeight = false;
            this.spIva.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spIva.DisplayFormat.FormatString = "n2";
            this.spIva.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spIva.EditFormat.FormatString = "n2";
            this.spIva.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spIva.Mask.EditMask = "n2";
            this.spIva.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spIva.Name = "spIva";
            // 
            // colTotal
            // 
            this.colTotal.Caption = "Total";
            this.colTotal.ColumnEdit = this.speTotal;
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.AllowEdit = false;
            this.colTotal.OptionsColumn.AllowFocus = false;
            this.colTotal.OptionsColumn.ReadOnly = true;
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:#,0.00;(#,0.00)}")});
            this.colTotal.UnboundExpression = "CostoTotal + ImpuestoTotal";
            this.colTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 9;
            // 
            // speTotal
            // 
            this.speTotal.AutoHeight = false;
            this.speTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speTotal.DisplayFormat.FormatString = "n2";
            this.speTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speTotal.EditFormat.FormatString = "n2";
            this.speTotal.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speTotal.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speTotal.Name = "speTotal";
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
            // speSubtotal
            // 
            this.speSubtotal.AutoHeight = false;
            this.speSubtotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.speSubtotal.DisplayFormat.FormatString = "n4";
            this.speSubtotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speSubtotal.EditFormat.FormatString = "n4";
            this.speSubtotal.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speSubtotal.Name = "speSubtotal";
            // 
            // groupControlBottom
            // 
            this.groupControlBottom.Controls.Add(this.gridConversion);
            this.groupControlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlBottom.Location = new System.Drawing.Point(0, 0);
            this.groupControlBottom.Name = "groupControlBottom";
            this.groupControlBottom.Size = new System.Drawing.Size(932, 177);
            this.groupControlBottom.TabIndex = 10;
            this.groupControlBottom.Text = "Conversión";
            // 
            // gridConversion
            // 
            this.gridConversion.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridConversion.DataSource = this.bdsDetalleC;
            this.gridConversion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridConversion.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridConversion.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridConversion.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridConversion.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridConversion.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridConversion_EmbeddedNavigator_ButtonClick);
            this.gridConversion.Location = new System.Drawing.Point(2, 21);
            this.gridConversion.MainView = this.gvConversion;
            this.gridConversion.Name = "gridConversion";
            this.gridConversion.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit4,
            this.repositoryItemSpinEdit1,
            this.rpSpCTotalConvercion,
            this.cboUnidadMedidaC,
            this.gridProductosC,
            this.lkAlmacenC,
            this.repositoryItemSpinEdit3,
            this.repositoryItemSpinEdit4,
            this.repositoryItemSpinEdit5,
            this.repositoryItemLookUpEdit3});
            this.gridConversion.Size = new System.Drawing.Size(928, 154);
            this.gridConversion.TabIndex = 9;
            this.gridConversion.UseEmbeddedNavigator = true;
            this.gridConversion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvConversion});
            // 
            // bdsDetalleC
            // 
            this.bdsDetalleC.AllowNew = true;
            // 
            // gvConversion
            // 
            this.gvConversion.ActiveFilterEnabled = false;
            this.gvConversion.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvConversion.Appearance.FooterPanel.Options.UseFont = true;
            this.gvConversion.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn20,
            this.colProductoC,
            this.colUnidadC,
            this.colAlmacenC,
            this.colCantidadC,
            this.colCostoUnitarioC,
            this.colCostoTotalC});
            this.gvConversion.GridControl = this.gridConversion;
            this.gvConversion.Name = "gvConversion";
            this.gvConversion.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvConversion.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvConversion.OptionsCustomization.AllowColumnMoving = false;
            this.gvConversion.OptionsCustomization.AllowFilter = false;
            this.gvConversion.OptionsCustomization.AllowGroup = false;
            this.gvConversion.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvConversion.OptionsCustomization.AllowSort = false;
            this.gvConversion.OptionsFilter.AllowFilterEditor = false;
            this.gvConversion.OptionsFilter.AllowMRUFilterList = false;
            this.gvConversion.OptionsLayout.Columns.AddNewColumns = false;
            this.gvConversion.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvConversion.OptionsNavigation.AutoFocusNewRow = true;
            this.gvConversion.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvConversion.OptionsView.ShowFooter = true;
            this.gvConversion.OptionsView.ShowGroupPanel = false;
            this.gvConversion.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvConversion_CellValueChanged);
            this.gvConversion.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvConversion_InvalidRowException);
            this.gvConversion.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvConversion_ValidateRow);
            this.gvConversion.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvConversion_KeyDown);
            // 
            // gridColumn20
            // 
            this.gridColumn20.FieldName = "ID";
            this.gridColumn20.Name = "gridColumn20";
            // 
            // colProductoC
            // 
            this.colProductoC.Caption = "Producto";
            this.colProductoC.ColumnEdit = this.gridProductosC;
            this.colProductoC.FieldName = "ProductoID";
            this.colProductoC.Name = "colProductoC";
            this.colProductoC.Visible = true;
            this.colProductoC.VisibleIndex = 0;
            this.colProductoC.Width = 156;
            // 
            // gridProductosC
            // 
            this.gridProductosC.AutoHeight = false;
            this.gridProductosC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridProductosC.DisplayMember = "Display";
            this.gridProductosC.Name = "gridProductosC";
            this.gridProductosC.NullText = "";
            this.gridProductosC.ValueMember = "ID";
            this.gridProductosC.View = this.gridView2;
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12});
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.FieldName = "ID";
            this.gridColumn9.Name = "gridColumn9";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Código";
            this.gridColumn10.FieldName = "Codigo";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 206;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Producto";
            this.gridColumn11.FieldName = "Nombre";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 1;
            this.gridColumn11.Width = 434;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Und.  Medida";
            this.gridColumn12.FieldName = "UmidadName";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 2;
            this.gridColumn12.Width = 176;
            // 
            // colUnidadC
            // 
            this.colUnidadC.Caption = "Unidad de Medida";
            this.colUnidadC.ColumnEdit = this.cboUnidadMedidaC;
            this.colUnidadC.FieldName = "UnidadMedidaID";
            this.colUnidadC.Name = "colUnidadC";
            this.colUnidadC.OptionsColumn.AllowEdit = false;
            this.colUnidadC.OptionsColumn.AllowFocus = false;
            this.colUnidadC.OptionsColumn.ReadOnly = true;
            this.colUnidadC.Visible = true;
            this.colUnidadC.VisibleIndex = 1;
            this.colUnidadC.Width = 65;
            // 
            // cboUnidadMedidaC
            // 
            this.cboUnidadMedidaC.AutoHeight = false;
            this.cboUnidadMedidaC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnidadMedidaC.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.cboUnidadMedidaC.DisplayMember = "Nombre";
            this.cboUnidadMedidaC.Name = "cboUnidadMedidaC";
            this.cboUnidadMedidaC.NullText = "";
            this.cboUnidadMedidaC.ValueMember = "ID";
            // 
            // colAlmacenC
            // 
            this.colAlmacenC.Caption = "Almacen";
            this.colAlmacenC.ColumnEdit = this.lkAlmacenC;
            this.colAlmacenC.FieldName = "AlmacenEntradaID";
            this.colAlmacenC.Name = "colAlmacenC";
            this.colAlmacenC.Visible = true;
            this.colAlmacenC.VisibleIndex = 2;
            this.colAlmacenC.Width = 68;
            // 
            // lkAlmacenC
            // 
            this.lkAlmacenC.AutoHeight = false;
            this.lkAlmacenC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkAlmacenC.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen / Tanque")});
            this.lkAlmacenC.DisplayMember = "Display";
            this.lkAlmacenC.Name = "lkAlmacenC";
            this.lkAlmacenC.NullText = "";
            this.lkAlmacenC.ValueMember = "ID";
            // 
            // colCantidadC
            // 
            this.colCantidadC.Caption = "Cantidad";
            this.colCantidadC.ColumnEdit = this.repositoryItemSpinEdit1;
            this.colCantidadC.DisplayFormat.FormatString = "n3";
            this.colCantidadC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCantidadC.FieldName = "CantidadEntrada";
            this.colCantidadC.Name = "colCantidadC";
            this.colCantidadC.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CantidadEntrada", "{0:N3}")});
            this.colCantidadC.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCantidadC.Visible = true;
            this.colCantidadC.VisibleIndex = 3;
            this.colCantidadC.Width = 59;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.DisplayFormat.FormatString = "n3";
            this.repositoryItemSpinEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemSpinEdit1.EditFormat.FormatString = "n3";
            this.repositoryItemSpinEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // colCostoUnitarioC
            // 
            this.colCostoUnitarioC.Caption = "Costo Unitario";
            this.colCostoUnitarioC.DisplayFormat.FormatString = "n4";
            this.colCostoUnitarioC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoUnitarioC.FieldName = "CostoEntrada";
            this.colCostoUnitarioC.Name = "colCostoUnitarioC";
            this.colCostoUnitarioC.OptionsColumn.AllowEdit = false;
            this.colCostoUnitarioC.OptionsColumn.AllowFocus = false;
            this.colCostoUnitarioC.OptionsColumn.ReadOnly = true;
            this.colCostoUnitarioC.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoUnitarioC.Visible = true;
            this.colCostoUnitarioC.VisibleIndex = 4;
            this.colCostoUnitarioC.Width = 63;
            // 
            // colCostoTotalC
            // 
            this.colCostoTotalC.Caption = "Costo Total";
            this.colCostoTotalC.ColumnEdit = this.rpSpCTotalConvercion;
            this.colCostoTotalC.DisplayFormat.FormatString = "n2";
            this.colCostoTotalC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoTotalC.FieldName = "CostoTotal";
            this.colCostoTotalC.Name = "colCostoTotalC";
            this.colCostoTotalC.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CostoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colCostoTotalC.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoTotalC.Visible = true;
            this.colCostoTotalC.VisibleIndex = 5;
            this.colCostoTotalC.Width = 79;
            // 
            // rpSpCTotalConvercion
            // 
            this.rpSpCTotalConvercion.AutoHeight = false;
            this.rpSpCTotalConvercion.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.rpSpCTotalConvercion.DisplayFormat.FormatString = "2";
            this.rpSpCTotalConvercion.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpCTotalConvercion.EditFormat.FormatString = "2";
            this.rpSpCTotalConvercion.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpCTotalConvercion.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.rpSpCTotalConvercion.Name = "rpSpCTotalConvercion";
            // 
            // repositoryItemLookUpEdit4
            // 
            this.repositoryItemLookUpEdit4.AutoHeight = false;
            this.repositoryItemLookUpEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit4.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.repositoryItemLookUpEdit4.Name = "repositoryItemLookUpEdit4";
            this.repositoryItemLookUpEdit4.NullText = "";
            this.repositoryItemLookUpEdit4.ShowHeader = false;
            // 
            // repositoryItemSpinEdit3
            // 
            this.repositoryItemSpinEdit3.AutoHeight = false;
            this.repositoryItemSpinEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit3.Name = "repositoryItemSpinEdit3";
            // 
            // repositoryItemSpinEdit4
            // 
            this.repositoryItemSpinEdit4.AutoHeight = false;
            this.repositoryItemSpinEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit4.Name = "repositoryItemSpinEdit4";
            // 
            // repositoryItemSpinEdit5
            // 
            this.repositoryItemSpinEdit5.AutoHeight = false;
            this.repositoryItemSpinEdit5.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit5.DisplayFormat.FormatString = "n2";
            this.repositoryItemSpinEdit5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemSpinEdit5.EditFormat.FormatString = "n2";
            this.repositoryItemSpinEdit5.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repositoryItemSpinEdit5.Name = "repositoryItemSpinEdit5";
            // 
            // repositoryItemLookUpEdit3
            // 
            this.repositoryItemLookUpEdit3.AutoHeight = false;
            this.repositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit3.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen")});
            this.repositoryItemLookUpEdit3.DisplayMember = "Display";
            this.repositoryItemLookUpEdit3.Name = "repositoryItemLookUpEdit3";
            this.repositoryItemLookUpEdit3.NullText = "";
            this.repositoryItemLookUpEdit3.ValueMember = "ID";
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
            this.btnShowComprobante.TabIndex = 6;
            this.btnShowComprobante.Text = "Mostrar \r\nComprobante";
            this.btnShowComprobante.Click += new System.EventHandler(this.btnShowComprobante_Click);
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
            // DialogDevolucionConversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 561);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogDevolucionConversion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtReferencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GlkFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMonto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgOptions.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkProvee.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaOrden.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaOrden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlProv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFactura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemConversion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlRef)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlTop)).EndInit();
            this.groupControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speSubtotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlBottom)).EndInit();
            this.groupControlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridConversion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalleC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConversion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductosC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedidaC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkAlmacenC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCTotalConvercion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).EndInit();
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
        private DevExpress.XtraEditors.DateEdit dateFechaOrden;
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
        private DevExpress.XtraEditors.SpinEdit spTC;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colProduct;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colCost;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacenSalida;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenSalida;
        private DevExpress.XtraGrid.Columns.GridColumn colExistencia;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.GridLookUpEdit glkProvee;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlProv;
        private DevExpress.XtraEditors.RadioGroup rgOptions;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        public DevExpress.XtraGrid.GridControl gridConversion;
        private DevExpress.XtraGrid.Views.Grid.GridView gvConversion;
        private DevExpress.XtraGrid.Columns.GridColumn colProductoC;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductosC;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn colUnidadC;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedidaC;
        private DevExpress.XtraGrid.Columns.GridColumn colAlmacenC;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkAlmacenC;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadC;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoTotalC;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpCTotalConvercion;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoUnitarioC;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit5;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit3;
        private DevExpress.XtraEditors.GridLookUpEdit GlkFactura;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
        private DevExpress.XtraEditors.SpinEdit spMonto;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFactura;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemConversion;
        private System.Windows.Forms.BindingSource bdsDetalleC;
        private DevExpress.XtraEditors.GroupControl groupControlTop;
        private DevExpress.XtraEditors.GroupControl groupControlBottom;
        private DevExpress.XtraGrid.Columns.GridColumn colIva;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spIva;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speSubtotal;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoInicial;
        private DevExpress.XtraEditors.TextEdit txtReferencia;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlRef;
    }
}