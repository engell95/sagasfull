namespace SAGAS.Ventas.Forms.Dialogs
{
    partial class DialogCompensacionAnticipos
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCompensacionAnticipos));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.btnAnticipar = new DevExpress.XtraEditors.SimpleButton();
            this.spMonto = new DevExpress.XtraEditors.SpinEdit();
            this.spSaldo = new DevExpress.XtraEditors.SpinEdit();
            this.lkMesInicial = new DevExpress.XtraEditors.LookUpEdit();
            this.glkDeudor = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupClEm = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemDeudor = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bdsMinutas = new System.Windows.Forms.BindingSource(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControlMain = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rplkTipo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColValor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaldo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAbono = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speMontoDetalle = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colPagado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpChkSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colNuevoSaldo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMontoUS = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRef = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacenEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spCostUnit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboAlmacenSalida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpLkEstacion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.bdsDetalleOtros = new System.Windows.Forms.BindingSource(this.components);
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlDif = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spMonto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMesInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkDeudor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupClEm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDeudor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMinutas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).BeginInit();
            this.splitContainerControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rplkTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speMontoDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChkSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalleOtros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.btnAnticipar);
            this.layoutControlTop.Controls.Add(this.spMonto);
            this.layoutControlTop.Controls.Add(this.spSaldo);
            this.layoutControlTop.Controls.Add(this.lkMesInicial);
            this.layoutControlTop.Controls.Add(this.glkDeudor);
            this.layoutControlTop.Controls.Add(this.btnLoad);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(932, 108);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // btnAnticipar
            // 
            this.btnAnticipar.Image = global::SAGAS.Ventas.Properties.Resources.down;
            this.btnAnticipar.Location = new System.Drawing.Point(525, 76);
            this.btnAnticipar.Name = "btnAnticipar";
            this.btnAnticipar.Size = new System.Drawing.Size(398, 22);
            this.btnAnticipar.StyleController = this.layoutControlTop;
            this.btnAnticipar.TabIndex = 8;
            this.btnAnticipar.Text = "Compensar";
            this.btnAnticipar.ToolTip = "Compensar";
            // 
            // spMonto
            // 
            this.spMonto.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spMonto.Enabled = false;
            this.spMonto.Location = new System.Drawing.Point(671, 52);
            this.spMonto.Name = "spMonto";
            this.spMonto.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.spMonto.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spMonto.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.spMonto.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spMonto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spMonto.Properties.DisplayFormat.FormatString = "n2";
            this.spMonto.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spMonto.Properties.EditFormat.FormatString = "n2";
            this.spMonto.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spMonto.Properties.Mask.EditMask = "n2";
            this.spMonto.Properties.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spMonto.Size = new System.Drawing.Size(252, 20);
            this.spMonto.StyleController = this.layoutControlTop;
            this.spMonto.TabIndex = 11;
            // 
            // spSaldo
            // 
            this.spSaldo.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spSaldo.Enabled = false;
            this.spSaldo.Location = new System.Drawing.Point(671, 28);
            this.spSaldo.Name = "spSaldo";
            this.spSaldo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.spSaldo.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spSaldo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.spSaldo.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spSaldo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.spSaldo.Properties.DisplayFormat.FormatString = "n2";
            this.spSaldo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSaldo.Properties.EditFormat.FormatString = "n2";
            this.spSaldo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSaldo.Properties.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spSaldo.Size = new System.Drawing.Size(252, 20);
            this.spSaldo.StyleController = this.layoutControlTop;
            this.spSaldo.TabIndex = 10;
            // 
            // lkMesInicial
            // 
            this.lkMesInicial.Location = new System.Drawing.Point(155, 52);
            this.lkMesInicial.Name = "lkMesInicial";
            this.lkMesInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMesInicial.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Mes")});
            this.lkMesInicial.Properties.DisplayMember = "Name";
            this.lkMesInicial.Properties.NullText = "<N/A>";
            this.lkMesInicial.Properties.ValueMember = "ID";
            this.lkMesInicial.Size = new System.Drawing.Size(366, 20);
            this.lkMesInicial.StyleController = this.layoutControlTop;
            this.lkMesInicial.TabIndex = 17;
            // 
            // glkDeudor
            // 
            this.glkDeudor.Location = new System.Drawing.Point(155, 28);
            this.glkDeudor.Name = "glkDeudor";
            this.glkDeudor.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
            this.glkDeudor.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.glkDeudor.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.glkDeudor.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.glkDeudor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkDeudor.Properties.DisplayMember = "Display";
            this.glkDeudor.Properties.NullText = "<Seleccione el Cliente>";
            this.glkDeudor.Properties.ValueMember = "ID";
            this.glkDeudor.Properties.View = this.gridLookUpEdit1View;
            this.glkDeudor.Size = new System.Drawing.Size(366, 20);
            this.glkDeudor.StyleController = this.layoutControlTop;
            this.glkDeudor.TabIndex = 8;
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
            this.gridLookUpEdit1View.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
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
            this.gridColumn3.Caption = "Deudor";
            this.gridColumn3.FieldName = "Nombre";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 640;
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::SAGAS.Ventas.Properties.Resources.application_put;
            this.btnLoad.Location = new System.Drawing.Point(9, 76);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(512, 22);
            this.btnLoad.StyleController = this.layoutControlTop;
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Cargar Lista";
            this.btnLoad.ToolTip = "Cargar Datos";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(932, 108);
            this.layoutControlGroup1.Text = "Datos de la Compensación";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroupClEm});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.ShowTabPageCloseButton = true;
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(924, 81);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // layoutControlGroupClEm
            // 
            this.layoutControlGroupClEm.CustomizationFormText = "layoutControlGroupClEm";
            this.layoutControlGroupClEm.GroupBordersVisible = false;
            this.layoutControlGroupClEm.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemDeudor,
            this.layoutControlItem5,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem1,
            this.layoutControlItem4});
            this.layoutControlGroupClEm.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupClEm.Name = "layoutControlGroupClEm";
            this.layoutControlGroupClEm.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupClEm.Size = new System.Drawing.Size(918, 75);
            this.layoutControlGroupClEm.Text = "layoutControlGroupClEm";
            this.layoutControlGroupClEm.TextVisible = false;
            // 
            // layoutControlItemDeudor
            // 
            this.layoutControlItemDeudor.Control = this.glkDeudor;
            this.layoutControlItemDeudor.CustomizationFormText = "<>";
            this.layoutControlItemDeudor.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemDeudor.Name = "layoutControlItemDeudor";
            this.layoutControlItemDeudor.Size = new System.Drawing.Size(516, 24);
            this.layoutControlItemDeudor.Text = "Clientes";
            this.layoutControlItemDeudor.TextSize = new System.Drawing.Size(143, 13);
            this.layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.btnLoad;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(516, 27);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.spSaldo;
            this.layoutControlItem2.CustomizationFormText = "Saldo Disponible para Anticipo";
            this.layoutControlItem2.Location = new System.Drawing.Point(516, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(402, 24);
            this.layoutControlItem2.Text = "Saldo Disponible para Anticipo";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.spMonto;
            this.layoutControlItem3.CustomizationFormText = "Monto a Anticipar";
            this.layoutControlItem3.Location = new System.Drawing.Point(516, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(402, 24);
            this.layoutControlItem3.Text = "Monto a Anticipar";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lkMesInicial;
            this.layoutControlItem1.CustomizationFormText = "Mes a Compensar";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(516, 24);
            this.layoutControlItem1.Text = "Mes a Compensar";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.btnAnticipar;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(516, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(402, 27);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // bdsMinutas
            // 
            this.bdsMinutas.AllowNew = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControlMain);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(936, 367);
            this.panelControl1.TabIndex = 0;
            // 
            // splitContainerControlMain
            // 
            this.splitContainerControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlMain.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControlMain.Location = new System.Drawing.Point(2, 110);
            this.splitContainerControlMain.Name = "splitContainerControlMain";
            this.splitContainerControlMain.Panel1.Controls.Add(this.gridDetalle);
            this.splitContainerControlMain.Panel1.Text = "Panel1";
            this.splitContainerControlMain.Panel2.Text = "Panel2";
            this.splitContainerControlMain.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitContainerControlMain.Size = new System.Drawing.Size(932, 242);
            this.splitContainerControlMain.SplitterPosition = 357;
            this.splitContainerControlMain.TabIndex = 10;
            this.splitContainerControlMain.Text = "splitContainerControl1";
            // 
            // gridDetalle
            // 
            this.gridDetalle.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridDetalle.DataSource = this.bdsDetalle;
            this.gridDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridDetalle.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridDetalle.Location = new System.Drawing.Point(0, 0);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speCantidad,
            this.speMontoDetalle,
            this.rplkTipo,
            this.gridProductos,
            this.cboAlmacenEntrada,
            this.spSubTotal,
            this.spMontoISC,
            this.spCostUnit,
            this.cboAlmacenSalida,
            this.rpChkSelected,
            this.rpLkEstacion});
            this.gridDetalle.Size = new System.Drawing.Size(932, 242);
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
            this.colTipo,
            this.colNro,
            this.colFecha,
            this.ColValor,
            this.colSaldo,
            this.colAbono,
            this.colMonto,
            this.colPagado,
            this.colNuevoSaldo,
            this.colTC,
            this.colMontoUS,
            this.colRef});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowGroup = false;
            this.gvDetalle.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsDetail.AllowZoomDetail = false;
            this.gvDetalle.OptionsDetail.EnableMasterViewMode = false;
            this.gvDetalle.OptionsDetail.ShowDetailTabs = false;
            this.gvDetalle.OptionsDetail.SmartDetailExpand = false;
            this.gvDetalle.OptionsFilter.AllowFilterEditor = false;
            this.gvDetalle.OptionsFilter.AllowMRUFilterList = false;
            this.gvDetalle.OptionsLayout.Columns.AddNewColumns = false;
            this.gvDetalle.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvDetalle.OptionsNavigation.AutoFocusNewRow = true;
            this.gvDetalle.OptionsView.ShowFooter = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvDetalle_InvalidRowException);
            this.gvDetalle.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvDetalle_ValidateRow);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colTipo
            // 
            this.colTipo.Caption = "Tipo Mov";
            this.colTipo.ColumnEdit = this.rplkTipo;
            this.colTipo.FieldName = "MovimientoTipoID";
            this.colTipo.Name = "colTipo";
            this.colTipo.OptionsColumn.AllowEdit = false;
            this.colTipo.OptionsColumn.AllowFocus = false;
            this.colTipo.Visible = true;
            this.colTipo.VisibleIndex = 0;
            this.colTipo.Width = 110;
            // 
            // rplkTipo
            // 
            this.rplkTipo.AutoHeight = false;
            this.rplkTipo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rplkTipo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.rplkTipo.DisplayMember = "Abreviatura";
            this.rplkTipo.Name = "rplkTipo";
            this.rplkTipo.NullText = "";
            this.rplkTipo.ValueMember = "ID";
            // 
            // colNro
            // 
            this.colNro.AppearanceCell.Options.UseTextOptions = true;
            this.colNro.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colNro.Caption = "Número";
            this.colNro.DisplayFormat.FormatString = "{0:000000000}";
            this.colNro.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colNro.FieldName = "Numero";
            this.colNro.Name = "colNro";
            this.colNro.OptionsColumn.AllowEdit = false;
            this.colNro.OptionsColumn.AllowFocus = false;
            this.colNro.Visible = true;
            this.colNro.VisibleIndex = 1;
            this.colNro.Width = 98;
            // 
            // colFecha
            // 
            this.colFecha.Caption = "Fecha";
            this.colFecha.FieldName = "FechaRegistro";
            this.colFecha.Name = "colFecha";
            this.colFecha.OptionsColumn.AllowEdit = false;
            this.colFecha.OptionsColumn.AllowFocus = false;
            this.colFecha.OptionsColumn.ReadOnly = true;
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 3;
            this.colFecha.Width = 91;
            // 
            // ColValor
            // 
            this.ColValor.AppearanceHeader.Options.UseTextOptions = true;
            this.ColValor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ColValor.Caption = "Valor Doc C$";
            this.ColValor.DisplayFormat.FormatString = "N2";
            this.ColValor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColValor.FieldName = "Monto";
            this.ColValor.Name = "ColValor";
            this.ColValor.OptionsColumn.AllowEdit = false;
            this.ColValor.OptionsColumn.AllowFocus = false;
            this.ColValor.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.ColValor.Visible = true;
            this.ColValor.VisibleIndex = 4;
            this.ColValor.Width = 100;
            // 
            // colSaldo
            // 
            this.colSaldo.AppearanceHeader.Options.UseTextOptions = true;
            this.colSaldo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colSaldo.Caption = "Saldo Doc C$";
            this.colSaldo.DisplayFormat.FormatString = "n2";
            this.colSaldo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSaldo.FieldName = "Saldo";
            this.colSaldo.Name = "colSaldo";
            this.colSaldo.OptionsColumn.AllowEdit = false;
            this.colSaldo.OptionsColumn.AllowFocus = false;
            this.colSaldo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Saldo", "{0:N2}")});
            this.colSaldo.UnboundExpression = "[Monto] - [Abono]";
            this.colSaldo.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colSaldo.Visible = true;
            this.colSaldo.VisibleIndex = 5;
            this.colSaldo.Width = 110;
            // 
            // colAbono
            // 
            this.colAbono.Caption = "Abono";
            this.colAbono.DisplayFormat.FormatString = "n2";
            this.colAbono.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAbono.FieldName = "Abono";
            this.colAbono.Name = "colAbono";
            this.colAbono.OptionsColumn.AllowEdit = false;
            this.colAbono.OptionsColumn.AllowFocus = false;
            this.colAbono.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            // 
            // colMonto
            // 
            this.colMonto.AppearanceHeader.Options.UseTextOptions = true;
            this.colMonto.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colMonto.Caption = "Monto Pag";
            this.colMonto.ColumnEdit = this.speMontoDetalle;
            this.colMonto.DisplayFormat.FormatString = "N2";
            this.colMonto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMonto.FieldName = "Litros";
            this.colMonto.Name = "colMonto";
            this.colMonto.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Litros", "{0:N2}")});
            this.colMonto.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colMonto.Visible = true;
            this.colMonto.VisibleIndex = 6;
            this.colMonto.Width = 134;
            // 
            // speMontoDetalle
            // 
            this.speMontoDetalle.AutoHeight = false;
            this.speMontoDetalle.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speMontoDetalle.DisplayFormat.FormatString = "2";
            this.speMontoDetalle.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speMontoDetalle.EditFormat.FormatString = "2";
            this.speMontoDetalle.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speMontoDetalle.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.speMontoDetalle.Name = "speMontoDetalle";
            // 
            // colPagado
            // 
            this.colPagado.AppearanceHeader.Options.UseTextOptions = true;
            this.colPagado.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPagado.Caption = "Pagado";
            this.colPagado.ColumnEdit = this.rpChkSelected;
            this.colPagado.FieldName = "Pagado";
            this.colPagado.Name = "colPagado";
            this.colPagado.OptionsColumn.AllowEdit = false;
            this.colPagado.OptionsColumn.AllowFocus = false;
            this.colPagado.Visible = true;
            this.colPagado.VisibleIndex = 8;
            this.colPagado.Width = 130;
            // 
            // rpChkSelected
            // 
            this.rpChkSelected.AutoHeight = false;
            this.rpChkSelected.Caption = "Check";
            this.rpChkSelected.Name = "rpChkSelected";
            // 
            // colNuevoSaldo
            // 
            this.colNuevoSaldo.AppearanceHeader.Options.UseTextOptions = true;
            this.colNuevoSaldo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colNuevoSaldo.Caption = "Nuevo Saldo";
            this.colNuevoSaldo.DisplayFormat.FormatString = "N2";
            this.colNuevoSaldo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colNuevoSaldo.FieldName = "NuevoSaldo";
            this.colNuevoSaldo.Name = "colNuevoSaldo";
            this.colNuevoSaldo.OptionsColumn.AllowEdit = false;
            this.colNuevoSaldo.OptionsColumn.AllowFocus = false;
            this.colNuevoSaldo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "NuevoSaldo", "{0:N2}")});
            this.colNuevoSaldo.UnboundExpression = "[Monto] - [Abono] - [Litros]";
            this.colNuevoSaldo.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colNuevoSaldo.Visible = true;
            this.colNuevoSaldo.VisibleIndex = 7;
            this.colNuevoSaldo.Width = 129;
            // 
            // colTC
            // 
            this.colTC.AppearanceCell.Options.UseTextOptions = true;
            this.colTC.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTC.AppearanceHeader.Options.UseTextOptions = true;
            this.colTC.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colTC.Caption = "T/C";
            this.colTC.DisplayFormat.FormatString = "N4";
            this.colTC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTC.FieldName = "TipoCambio";
            this.colTC.Name = "colTC";
            this.colTC.OptionsColumn.AllowEdit = false;
            this.colTC.OptionsColumn.AllowFocus = false;
            this.colTC.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            // 
            // colMontoUS
            // 
            this.colMontoUS.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.colMontoUS.AppearanceCell.Options.UseBackColor = true;
            this.colMontoUS.AppearanceCell.Options.UseTextOptions = true;
            this.colMontoUS.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colMontoUS.AppearanceHeader.Options.UseTextOptions = true;
            this.colMontoUS.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colMontoUS.Caption = "Valor Doc U$";
            this.colMontoUS.DisplayFormat.FormatString = "N2";
            this.colMontoUS.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMontoUS.FieldName = "MontoUS";
            this.colMontoUS.Name = "colMontoUS";
            this.colMontoUS.OptionsColumn.AllowEdit = false;
            this.colMontoUS.OptionsColumn.AllowFocus = false;
            this.colMontoUS.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontoUS", "U$ = {0:N2}")});
            this.colMontoUS.UnboundExpression = "Round(([Monto] / [TipoCambio]), 2)";
            this.colMontoUS.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            // 
            // colRef
            // 
            this.colRef.Caption = "Referencia";
            this.colRef.FieldName = "Referencia";
            this.colRef.Name = "colRef";
            this.colRef.OptionsColumn.AllowEdit = false;
            this.colRef.OptionsColumn.AllowFocus = false;
            this.colRef.Visible = true;
            this.colRef.VisibleIndex = 2;
            this.colRef.Width = 82;
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
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.spCostUnit.DisplayFormat.FormatString = "n4";
            this.spCostUnit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.EditFormat.FormatString = "n4";
            this.spCostUnit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.Name = "spCostUnit";
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
            // rpLkEstacion
            // 
            this.rpLkEstacion.AutoHeight = false;
            this.rpLkEstacion.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpLkEstacion.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estación")});
            this.rpLkEstacion.DisplayMember = "Nombre";
            this.rpLkEstacion.Name = "rpLkEstacion";
            this.rpLkEstacion.ValueMember = "ID";
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 352);
            this.lblRequerido.Name = "lblRequerido";
            this.lblRequerido.Size = new System.Drawing.Size(255, 13);
            this.lblRequerido.TabIndex = 1;
            this.lblRequerido.Text = "Nota:  Los campos en negritas son requeridos";
            // 
            // bdsDetalleOtros
            // 
            this.bdsDetalleOtros.AllowNew = true;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.layoutControlDif);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.bntNew);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 367);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(936, 35);
            this.panelControl2.TabIndex = 1;
            // 
            // layoutControlDif
            // 
            this.layoutControlDif.Dock = System.Windows.Forms.DockStyle.Right;
            this.layoutControlDif.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.layoutControlDif.Location = new System.Drawing.Point(644, 2);
            this.layoutControlDif.Name = "layoutControlDif";
            this.layoutControlDif.Root = this.layoutControlGroup2;
            this.layoutControlDif.Size = new System.Drawing.Size(290, 31);
            this.layoutControlDif.TabIndex = 6;
            this.layoutControlDif.Text = "layoutControl1";
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
            this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup2.GroupBordersVisible = false;
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(290, 31);
            this.layoutControlGroup2.Text = "layoutControlGroup2";
            this.layoutControlGroup2.TextVisible = false;
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
            this.btnOK.Text = "Aplicar";
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
            // DialogCompensacionAnticipos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 402);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogCompensacionAnticipos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spMonto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMesInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkDeudor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupClEm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDeudor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsMinutas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).EndInit();
            this.splitContainerControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rplkTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speMontoDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChkSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalleOtros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlDif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rplkTipo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speMontoDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenEntrada;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
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
        private DevExpress.XtraGrid.Columns.GridColumn colNro;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraGrid.Columns.GridColumn ColValor;
        private DevExpress.XtraGrid.Columns.GridColumn colSaldo;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenSalida;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraGrid.Columns.GridColumn colAbono;
        private DevExpress.XtraGrid.Columns.GridColumn colMonto;
        private DevExpress.XtraGrid.Columns.GridColumn colPagado;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpChkSelected;
        private DevExpress.XtraGrid.Columns.GridColumn colTipo;
        private DevExpress.XtraGrid.Columns.GridColumn colNuevoSaldo;
        private DevExpress.XtraLayout.LayoutControl layoutControlDif;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraGrid.Columns.GridColumn colTC;
        private DevExpress.XtraGrid.Columns.GridColumn colMontoUS;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkEstacion;
        private System.Windows.Forms.BindingSource bdsDetalleOtros;
        private DevExpress.XtraGrid.Columns.GridColumn colRef;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupClEm;
        private DevExpress.XtraEditors.GridLookUpEdit glkDeudor;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDeudor;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlMain;
        private System.Windows.Forms.BindingSource bdsMinutas;
        private DevExpress.XtraEditors.LookUpEdit lkMesInicial;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SimpleButton btnAnticipar;
        private DevExpress.XtraEditors.SpinEdit spMonto;
        private DevExpress.XtraEditors.SpinEdit spSaldo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
    }
}