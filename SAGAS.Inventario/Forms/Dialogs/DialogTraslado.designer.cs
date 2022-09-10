namespace SAGAS.Inventario.Forms.Dialogs
{
    partial class DialogTraslado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTraslado));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.txtNroRecibido = new DevExpress.XtraEditors.TextEdit();
            this.dateFechaRecibido = new DevExpress.XtraEditors.DateEdit();
            this.glkESOut = new DevExpress.XtraEditors.LookUpEdit();
            this.glkSESIn = new DevExpress.XtraEditors.LookUpEdit();
            this.glkESIn = new DevExpress.XtraEditors.LookUpEdit();
            this.glkSESOut = new DevExpress.XtraEditors.LookUpEdit();
            this.dateFechaTraslado = new DevExpress.XtraEditors.DateEdit();
            this.mmoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.txtNoFactura = new DevExpress.XtraEditors.TextEdit();
            this.glkArea = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblComentario = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciSESout = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemES = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciSESin = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblFechaAdquisicion = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblNoFactura = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblTipoMovimiento = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcFechaSalida = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemNroEntrada = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gvProducto = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ColAlmacenEntrada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacenEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.ColAlmacenSalida = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacenSalida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colCantidadExistencia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spCantidadInicial = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCantidadEntrada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spCantidadEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCantidadSalida = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostoInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCostEntrada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spCtoUniEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostSalida = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spCostUnit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostoTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rspCostoTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemLookUpEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rlkUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemSpinEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboUnidad = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cboQExist = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bntNew = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnShowComprobante = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.infBloqueo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNroRecibido.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaRecibido.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaRecibido.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkESOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkSESIn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkESIn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkSESOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaTraslado.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaTraslado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkArea.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSESout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSESin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoMovimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcFechaSalida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNroEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCantidadInicial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCantidadEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCtoUniEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rspCostoTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboQExist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infBloqueo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.txtNroRecibido);
            this.layoutControlTop.Controls.Add(this.dateFechaRecibido);
            this.layoutControlTop.Controls.Add(this.glkESOut);
            this.layoutControlTop.Controls.Add(this.glkSESIn);
            this.layoutControlTop.Controls.Add(this.glkESIn);
            this.layoutControlTop.Controls.Add(this.glkSESOut);
            this.layoutControlTop.Controls.Add(this.dateFechaTraslado);
            this.layoutControlTop.Controls.Add(this.mmoComentario);
            this.layoutControlTop.Controls.Add(this.txtNoFactura);
            this.layoutControlTop.Controls.Add(this.glkArea);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(844, 206);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // txtNroRecibido
            // 
            this.txtNroRecibido.EditValue = "";
            this.txtNroRecibido.Location = new System.Drawing.Point(713, 52);
            this.txtNroRecibido.Name = "txtNroRecibido";
            this.txtNroRecibido.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtNroRecibido.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtNroRecibido.Properties.DisplayFormat.FormatString = "f0";
            this.txtNroRecibido.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNroRecibido.Properties.EditFormat.FormatString = "f0";
            this.txtNroRecibido.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNroRecibido.Properties.Mask.EditMask = "f0";
            this.txtNroRecibido.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNroRecibido.Properties.ReadOnly = true;
            this.txtNroRecibido.Size = new System.Drawing.Size(122, 20);
            this.txtNroRecibido.StyleController = this.layoutControlTop;
            this.txtNroRecibido.TabIndex = 7;
            // 
            // dateFechaRecibido
            // 
            this.dateFechaRecibido.EditValue = null;
            this.dateFechaRecibido.Location = new System.Drawing.Point(713, 76);
            this.dateFechaRecibido.Name = "dateFechaRecibido";
            this.dateFechaRecibido.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaRecibido.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaRecibido.Size = new System.Drawing.Size(122, 20);
            this.dateFechaRecibido.StyleController = this.layoutControlTop;
            this.dateFechaRecibido.TabIndex = 16;
            this.dateFechaRecibido.EditValueChanged += new System.EventHandler(this.dateFechaRecibido_EditValueChanged);
            this.dateFechaRecibido.Validated += new System.EventHandler(this.dateFechaRecibido_Validated);
            // 
            // glkESOut
            // 
            this.glkESOut.Location = new System.Drawing.Point(131, 72);
            this.glkESOut.Name = "glkESOut";
            this.glkESOut.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.glkESOut.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.glkESOut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkESOut.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkESOut.Properties.DisplayMember = "Nombre";
            this.glkESOut.Properties.NullText = "[Seleccione E/S]";
            this.glkESOut.Properties.ReadOnly = true;
            this.glkESOut.Properties.ValueMember = "ID";
            this.glkESOut.Size = new System.Drawing.Size(157, 20);
            this.glkESOut.StyleController = this.layoutControlTop;
            this.glkESOut.TabIndex = 15;
            this.glkESOut.EditValueChanged += new System.EventHandler(this.glkESOut_EditValueChanged);
            // 
            // glkSESIn
            // 
            this.glkSESIn.Location = new System.Drawing.Point(131, 169);
            this.glkSESIn.Name = "glkSESIn";
            this.glkSESIn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkSESIn.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Subestaciones", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkSESIn.Properties.DisplayMember = "Nombre";
            this.glkSESIn.Properties.NullText = "[Seleccione subestación]";
            this.glkSESIn.Properties.ValueMember = "ID";
            this.glkSESIn.Size = new System.Drawing.Size(157, 20);
            this.glkSESIn.StyleController = this.layoutControlTop;
            this.glkSESIn.TabIndex = 14;
            // 
            // glkESIn
            // 
            this.glkESIn.Location = new System.Drawing.Point(131, 145);
            this.glkESIn.Name = "glkESIn";
            this.glkESIn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkESIn.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkESIn.Properties.DisplayMember = "Nombre";
            this.glkESIn.Properties.NullText = "[Seleccione E/S]";
            this.glkESIn.Properties.ValueMember = "ID";
            this.glkESIn.Size = new System.Drawing.Size(157, 20);
            this.glkESIn.StyleController = this.layoutControlTop;
            this.glkESIn.TabIndex = 13;
            this.glkESIn.EditValueChanged += new System.EventHandler(this.glkESIn_EditValueChanged);
            this.glkESIn.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // glkSESOut
            // 
            this.glkSESOut.EditValue = "";
            this.glkSESOut.Location = new System.Drawing.Point(131, 96);
            this.glkSESOut.Name = "glkSESOut";
            this.glkSESOut.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.glkSESOut.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.glkSESOut.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkSESOut.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Subestaciones", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkSESOut.Properties.DisplayMember = "Nombre";
            this.glkSESOut.Properties.NullText = "[Seleccione subestación]";
            this.glkSESOut.Properties.ReadOnly = true;
            this.glkSESOut.Properties.ValueMember = "ID";
            this.glkSESOut.Size = new System.Drawing.Size(157, 20);
            this.glkSESOut.StyleController = this.layoutControlTop;
            this.glkSESOut.TabIndex = 12;
            // 
            // dateFechaTraslado
            // 
            this.dateFechaTraslado.EditValue = null;
            this.dateFechaTraslado.Location = new System.Drawing.Point(414, 76);
            this.dateFechaTraslado.Name = "dateFechaTraslado";
            this.dateFechaTraslado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaTraslado.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaTraslado.Size = new System.Drawing.Size(179, 20);
            this.dateFechaTraslado.StyleController = this.layoutControlTop;
            this.dateFechaTraslado.TabIndex = 4;
            this.dateFechaTraslado.EditValueChanged += new System.EventHandler(this.dateFechaTraslado_EditValueChanged);
            this.dateFechaTraslado.Validated += new System.EventHandler(this.dateFechaCompra_Validated);
            // 
            // mmoComentario
            // 
            this.mmoComentario.Location = new System.Drawing.Point(298, 118);
            this.mmoComentario.Name = "mmoComentario";
            this.mmoComentario.Size = new System.Drawing.Size(537, 79);
            this.mmoComentario.StyleController = this.layoutControlTop;
            this.mmoComentario.TabIndex = 8;
            this.mmoComentario.UseOptimizedRendering = true;
            this.mmoComentario.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // txtNoFactura
            // 
            this.txtNoFactura.EditValue = "";
            this.txtNoFactura.Location = new System.Drawing.Point(414, 52);
            this.txtNoFactura.Name = "txtNoFactura";
            this.txtNoFactura.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtNoFactura.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtNoFactura.Properties.DisplayFormat.FormatString = "f0";
            this.txtNoFactura.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNoFactura.Properties.EditFormat.FormatString = "f0";
            this.txtNoFactura.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNoFactura.Properties.Mask.EditMask = "f0";
            this.txtNoFactura.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtNoFactura.Properties.ReadOnly = true;
            this.txtNoFactura.Size = new System.Drawing.Size(179, 20);
            this.txtNoFactura.StyleController = this.layoutControlTop;
            this.txtNoFactura.TabIndex = 6;
            this.txtNoFactura.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // glkArea
            // 
            this.glkArea.EditValue = "";
            this.glkArea.Location = new System.Drawing.Point(414, 28);
            this.glkArea.Name = "glkArea";
            this.glkArea.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkArea.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Areas", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkArea.Properties.DisplayMember = "Display";
            this.glkArea.Properties.NullText = "<Seleccione el Área>";
            this.glkArea.Properties.ValueMember = "ID";
            this.glkArea.Size = new System.Drawing.Size(421, 20);
            this.glkArea.StyleController = this.layoutControlTop;
            this.glkArea.TabIndex = 10;
            this.glkArea.EditValueChanged += new System.EventHandler(this.glkTipoMovimiento_EditValueChanged);
            this.glkArea.Validated += new System.EventHandler(this.txtNombre_Validated_1);
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
            this.layoutControlGroup1.ShowInCustomizationForm = false;
            this.layoutControlGroup1.Size = new System.Drawing.Size(844, 206);
            this.layoutControlGroup1.Text = " ";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblComentario,
            this.layoutControlGroup2,
            this.lblFechaAdquisicion,
            this.lblNoFactura,
            this.lblTipoMovimiento,
            this.lcFechaSalida,
            this.layoutControlItemNroEntrada});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(836, 179);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // lblComentario
            // 
            this.lblComentario.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblComentario.AppearanceItemCaption.Options.UseFont = true;
            this.lblComentario.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblComentario.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblComentario.Control = this.mmoComentario;
            this.lblComentario.CustomizationFormText = "Comentario";
            this.lblComentario.Location = new System.Drawing.Point(289, 72);
            this.lblComentario.Name = "lblComentario";
            this.lblComentario.Size = new System.Drawing.Size(541, 101);
            this.lblComentario.Text = "Comentario";
            this.lblComentario.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblComentario.TextLocation = DevExpress.Utils.Locations.Top;
            this.lblComentario.TextSize = new System.Drawing.Size(90, 13);
            this.lblComentario.TextToControlDistance = 5;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.Options.UseTextOptions = true;
            this.layoutControlGroup2.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlGroup2.CustomizationFormText = "Estaciones";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup3,
            this.layoutControlGroup5});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(289, 173);
            this.layoutControlGroup2.Text = "Estaciones";
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.AppearanceGroup.Options.UseTextOptions = true;
            this.layoutControlGroup3.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutControlGroup3.CustomizationFormText = "Estación de salida";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciSESout,
            this.layoutControlItemES});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup3.Size = new System.Drawing.Size(283, 73);
            this.layoutControlGroup3.Text = "Estación de salida";
            // 
            // lciSESout
            // 
            this.lciSESout.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lciSESout.AppearanceItemCaption.Options.UseFont = true;
            this.lciSESout.Control = this.glkSESOut;
            this.lciSESout.CustomizationFormText = "Sub Estación";
            this.lciSESout.Location = new System.Drawing.Point(0, 24);
            this.lciSESout.Name = "lciSESout";
            this.lciSESout.Size = new System.Drawing.Size(277, 24);
            this.lciSESout.Text = "Sub Estación";
            this.lciSESout.TextSize = new System.Drawing.Size(113, 13);
            this.lciSESout.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItemES
            // 
            this.layoutControlItemES.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemES.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemES.Control = this.glkESOut;
            this.layoutControlItemES.CustomizationFormText = "Estación Servicio";
            this.layoutControlItemES.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemES.Name = "layoutControlItemES";
            this.layoutControlItemES.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItemES.Text = "Estación Servicio";
            this.layoutControlItemES.TextSize = new System.Drawing.Size(113, 13);
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.AppearanceGroup.Options.UseTextOptions = true;
            this.layoutControlGroup5.AppearanceGroup.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutControlGroup5.CustomizationFormText = "Estación de Entrada";
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.lciSESin});
            this.layoutControlGroup5.Location = new System.Drawing.Point(0, 73);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup5.Size = new System.Drawing.Size(283, 75);
            this.layoutControlGroup5.Text = "Estación de Entrada";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.glkESIn;
            this.layoutControlItem1.CustomizationFormText = "Estación Servicio";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(277, 24);
            this.layoutControlItem1.Text = "Estación Servicio";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(113, 13);
            // 
            // lciSESin
            // 
            this.lciSESin.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lciSESin.AppearanceItemCaption.Options.UseFont = true;
            this.lciSESin.Control = this.glkSESIn;
            this.lciSESin.CustomizationFormText = "Sub Estación";
            this.lciSESin.Location = new System.Drawing.Point(0, 24);
            this.lciSESin.Name = "lciSESin";
            this.lciSESin.Size = new System.Drawing.Size(277, 26);
            this.lciSESin.Text = "Sub Estación";
            this.lciSESin.TextSize = new System.Drawing.Size(113, 13);
            this.lciSESin.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblFechaAdquisicion
            // 
            this.lblFechaAdquisicion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseFont = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblFechaAdquisicion.Control = this.dateFechaTraslado;
            this.lblFechaAdquisicion.CustomizationFormText = "Fecha Adquisición";
            this.lblFechaAdquisicion.Location = new System.Drawing.Point(289, 48);
            this.lblFechaAdquisicion.MinSize = new System.Drawing.Size(169, 24);
            this.lblFechaAdquisicion.Name = "lblFechaAdquisicion";
            this.lblFechaAdquisicion.Size = new System.Drawing.Size(299, 24);
            this.lblFechaAdquisicion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblFechaAdquisicion.Text = "Fecha de traslado";
            this.lblFechaAdquisicion.TextSize = new System.Drawing.Size(113, 13);
            // 
            // lblNoFactura
            // 
            this.lblNoFactura.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNoFactura.AppearanceItemCaption.Options.UseFont = true;
            this.lblNoFactura.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblNoFactura.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblNoFactura.Control = this.txtNoFactura;
            this.lblNoFactura.CustomizationFormText = "No. de Factura";
            this.lblNoFactura.Location = new System.Drawing.Point(289, 24);
            this.lblNoFactura.Name = "lblNoFactura";
            this.lblNoFactura.Size = new System.Drawing.Size(299, 24);
            this.lblNoFactura.Text = "Número de Traslado";
            this.lblNoFactura.TextSize = new System.Drawing.Size(113, 13);
            // 
            // lblTipoMovimiento
            // 
            this.lblTipoMovimiento.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTipoMovimiento.AppearanceItemCaption.Options.UseFont = true;
            this.lblTipoMovimiento.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblTipoMovimiento.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblTipoMovimiento.Control = this.glkArea;
            this.lblTipoMovimiento.CustomizationFormText = "Tipo Movimiento";
            this.lblTipoMovimiento.Location = new System.Drawing.Point(289, 0);
            this.lblTipoMovimiento.Name = "lblTipoMovimiento";
            this.lblTipoMovimiento.Size = new System.Drawing.Size(541, 24);
            this.lblTipoMovimiento.Text = "Área de venta";
            this.lblTipoMovimiento.TextSize = new System.Drawing.Size(113, 13);
            // 
            // lcFechaSalida
            // 
            this.lcFechaSalida.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcFechaSalida.AppearanceItemCaption.Options.UseFont = true;
            this.lcFechaSalida.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lcFechaSalida.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lcFechaSalida.Control = this.dateFechaRecibido;
            this.lcFechaSalida.CustomizationFormText = "Fecha de salida";
            this.lcFechaSalida.Location = new System.Drawing.Point(588, 48);
            this.lcFechaSalida.Name = "lcFechaSalida";
            this.lcFechaSalida.Size = new System.Drawing.Size(242, 24);
            this.lcFechaSalida.Text = "Fecha de Recibido";
            this.lcFechaSalida.TextSize = new System.Drawing.Size(113, 13);
            this.lcFechaSalida.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItemNroEntrada
            // 
            this.layoutControlItemNroEntrada.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemNroEntrada.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemNroEntrada.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemNroEntrada.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItemNroEntrada.Control = this.txtNroRecibido;
            this.layoutControlItemNroEntrada.CustomizationFormText = "Número de Recibido";
            this.layoutControlItemNroEntrada.Location = new System.Drawing.Point(588, 24);
            this.layoutControlItemNroEntrada.Name = "layoutControlItemNroEntrada";
            this.layoutControlItemNroEntrada.Size = new System.Drawing.Size(242, 24);
            this.layoutControlItemNroEntrada.Text = "Número de Recibido";
            this.layoutControlItemNroEntrada.TextSize = new System.Drawing.Size(113, 13);
            this.layoutControlItemNroEntrada.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
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
            this.panelControl1.Size = new System.Drawing.Size(848, 361);
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
            this.gridDetalle.Location = new System.Drawing.Point(2, 208);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit3,
            this.speCantidad,
            this.rspCostoTotal,
            this.rlkUnidadMedida,
            this.gridProductos,
            this.cboAlmacenEntrada,
            this.repositoryItemSpinEdit2,
            this.repositoryItemSpinEdit3,
            this.spCostUnit,
            this.cboAlmacenSalida,
            this.cboUnidad,
            this.cboUnidadMedida,
            this.cboQExist,
            this.spCantidadInicial,
            this.spCtoUniEntrada,
            this.spCantidadEntrada});
            this.gridDetalle.Size = new System.Drawing.Size(844, 138);
            this.gridDetalle.TabIndex = 9;
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
            this.colProductID,
            this.colUnit,
            this.ColAlmacenEntrada,
            this.ColAlmacenSalida,
            this.colCantidadExistencia,
            this.colCantidadEntrada,
            this.colCantidadSalida,
            this.colCostoInicial,
            this.colCostEntrada,
            this.colCostSalida,
            this.colCostoTotal});
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
            this.gvDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvDetalle.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvDetalle_InvalidRowException);
            this.gvDetalle.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvDetalle_ValidateRow);
            this.gvDetalle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvDetalle_KeyDown);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colProductID
            // 
            this.colProductID.Caption = "Producto";
            this.colProductID.ColumnEdit = this.gridProductos;
            this.colProductID.FieldName = "ProductoID";
            this.colProductID.Name = "colProductID";
            this.colProductID.Visible = true;
            this.colProductID.VisibleIndex = 0;
            this.colProductID.Width = 162;
            // 
            // gridProductos
            // 
            this.gridProductos.AutoHeight = false;
            this.gridProductos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridProductos.DisplayMember = "Display";
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.ValueMember = "ProductoID";
            this.gridProductos.View = this.gvProducto;
            // 
            // gvProducto
            // 
            this.gvProducto.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gvProducto.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvProducto.Name = "gvProducto";
            this.gvProducto.OptionsBehavior.Editable = false;
            this.gvProducto.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvProducto.OptionsView.ShowAutoFilterRow = true;
            this.gvProducto.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "ProductoID";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Código";
            this.gridColumn5.FieldName = "ProductoCodigo";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 206;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Producto";
            this.gridColumn6.FieldName = "ProductoNombre";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 434;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Und.  Medida";
            this.gridColumn7.FieldName = "UnidadNombre";
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
            this.colUnit.Width = 97;
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
            // ColAlmacenEntrada
            // 
            this.ColAlmacenEntrada.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ColAlmacenEntrada.AppearanceCell.Options.UseBackColor = true;
            this.ColAlmacenEntrada.Caption = "Almacen de Entrada";
            this.ColAlmacenEntrada.ColumnEdit = this.cboAlmacenEntrada;
            this.ColAlmacenEntrada.FieldName = "AlmacenEntradaID";
            this.ColAlmacenEntrada.Name = "ColAlmacenEntrada";
            this.ColAlmacenEntrada.Width = 113;
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
            this.cboAlmacenEntrada.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.cboAlmacenEntrada_QueryPopUp);
            this.cboAlmacenEntrada.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboAlmacenEntrada_Closed);
            // 
            // ColAlmacenSalida
            // 
            this.ColAlmacenSalida.Caption = "Almacen de Salida";
            this.ColAlmacenSalida.ColumnEdit = this.cboAlmacenSalida;
            this.ColAlmacenSalida.FieldName = "AlmacenSalidaID";
            this.ColAlmacenSalida.Name = "ColAlmacenSalida";
            this.ColAlmacenSalida.Visible = true;
            this.ColAlmacenSalida.VisibleIndex = 2;
            this.ColAlmacenSalida.Width = 114;
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
            this.cboAlmacenSalida.QueryPopUp += new System.ComponentModel.CancelEventHandler(this.cboAlmacenSalida_QueryPopUp);
            this.cboAlmacenSalida.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboAlmacenSalida_Closed);
            // 
            // colCantidadExistencia
            // 
            this.colCantidadExistencia.Caption = "Cantidad Existencia";
            this.colCantidadExistencia.ColumnEdit = this.spCantidadInicial;
            this.colCantidadExistencia.DisplayFormat.FormatString = "N3";
            this.colCantidadExistencia.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCantidadExistencia.FieldName = "CantidadInicial";
            this.colCantidadExistencia.Name = "colCantidadExistencia";
            this.colCantidadExistencia.OptionsColumn.AllowEdit = false;
            this.colCantidadExistencia.OptionsColumn.AllowFocus = false;
            this.colCantidadExistencia.Visible = true;
            this.colCantidadExistencia.VisibleIndex = 3;
            this.colCantidadExistencia.Width = 114;
            // 
            // spCantidadInicial
            // 
            this.spCantidadInicial.AutoHeight = false;
            this.spCantidadInicial.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spCantidadInicial.Name = "spCantidadInicial";
            this.spCantidadInicial.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // colCantidadEntrada
            // 
            this.colCantidadEntrada.Caption = "Cantidad de entrada";
            this.colCantidadEntrada.ColumnEdit = this.spCantidadEntrada;
            this.colCantidadEntrada.FieldName = "CantidadEntrada";
            this.colCantidadEntrada.Name = "colCantidadEntrada";
            // 
            // spCantidadEntrada
            // 
            this.spCantidadEntrada.AllowMouseWheel = false;
            this.spCantidadEntrada.AutoHeight = false;
            this.spCantidadEntrada.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spCantidadEntrada.DisplayFormat.FormatString = "n4";
            this.spCantidadEntrada.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCantidadEntrada.EditFormat.FormatString = "n4";
            this.spCantidadEntrada.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCantidadEntrada.Name = "spCantidadEntrada";
            // 
            // colCantidadSalida
            // 
            this.colCantidadSalida.Caption = "Cantidad Salida";
            this.colCantidadSalida.ColumnEdit = this.speCantidad;
            this.colCantidadSalida.DisplayFormat.FormatString = "n3";
            this.colCantidadSalida.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCantidadSalida.FieldName = "CantidadSalida";
            this.colCantidadSalida.Name = "colCantidadSalida";
            this.colCantidadSalida.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CantidadSalida", "{0:N3}")});
            this.colCantidadSalida.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCantidadSalida.Visible = true;
            this.colCantidadSalida.VisibleIndex = 4;
            this.colCantidadSalida.Width = 98;
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
            // colCostoInicial
            // 
            this.colCostoInicial.Caption = "Ult. Cto.";
            this.colCostoInicial.FieldName = "CostoInicial";
            this.colCostoInicial.Name = "colCostoInicial";
            this.colCostoInicial.OptionsColumn.AllowEdit = false;
            this.colCostoInicial.OptionsColumn.AllowFocus = false;
            // 
            // colCostEntrada
            // 
            this.colCostEntrada.Caption = "Costo Unitario";
            this.colCostEntrada.ColumnEdit = this.spCtoUniEntrada;
            this.colCostEntrada.DisplayFormat.FormatString = "n4";
            this.colCostEntrada.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostEntrada.FieldName = "CostoEntrada";
            this.colCostEntrada.Name = "colCostEntrada";
            // 
            // spCtoUniEntrada
            // 
            this.spCtoUniEntrada.AllowMouseWheel = false;
            this.spCtoUniEntrada.AutoHeight = false;
            this.spCtoUniEntrada.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.SpinRight)});
            this.spCtoUniEntrada.DisplayFormat.FormatString = "n4";
            this.spCtoUniEntrada.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCtoUniEntrada.EditFormat.FormatString = "n4";
            this.spCtoUniEntrada.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCtoUniEntrada.Name = "spCtoUniEntrada";
            // 
            // colCostSalida
            // 
            this.colCostSalida.Caption = "Costo Unitario";
            this.colCostSalida.ColumnEdit = this.spCostUnit;
            this.colCostSalida.DisplayFormat.FormatString = "n4";
            this.colCostSalida.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostSalida.FieldName = "CostoSalida";
            this.colCostSalida.Name = "colCostSalida";
            this.colCostSalida.OptionsColumn.AllowEdit = false;
            this.colCostSalida.OptionsColumn.AllowFocus = false;
            this.colCostSalida.OptionsColumn.ReadOnly = true;
            this.colCostSalida.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostSalida.Visible = true;
            this.colCostSalida.VisibleIndex = 5;
            this.colCostSalida.Width = 119;
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
            // colCostoTotal
            // 
            this.colCostoTotal.Caption = "Costo Total";
            this.colCostoTotal.ColumnEdit = this.rspCostoTotal;
            this.colCostoTotal.DisplayFormat.FormatString = "n2";
            this.colCostoTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoTotal.FieldName = "CostoTotal";
            this.colCostoTotal.Name = "colCostoTotal";
            this.colCostoTotal.OptionsColumn.AllowEdit = false;
            this.colCostoTotal.OptionsColumn.AllowFocus = false;
            this.colCostoTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CostoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colCostoTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoTotal.Visible = true;
            this.colCostoTotal.VisibleIndex = 6;
            this.colCostoTotal.Width = 122;
            // 
            // rspCostoTotal
            // 
            this.rspCostoTotal.AutoHeight = false;
            this.rspCostoTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.rspCostoTotal.DisplayFormat.FormatString = "2";
            this.rspCostoTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rspCostoTotal.EditFormat.FormatString = "2";
            this.rspCostoTotal.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rspCostoTotal.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.rspCostoTotal.Name = "rspCostoTotal";
            // 
            // repositoryItemLookUpEdit3
            // 
            this.repositoryItemLookUpEdit3.AutoHeight = false;
            this.repositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit3.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.repositoryItemLookUpEdit3.Name = "repositoryItemLookUpEdit3";
            this.repositoryItemLookUpEdit3.NullText = "";
            this.repositoryItemLookUpEdit3.ShowHeader = false;
            // 
            // rlkUnidadMedida
            // 
            this.rlkUnidadMedida.AutoHeight = false;
            this.rlkUnidadMedida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlkUnidadMedida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.rlkUnidadMedida.Name = "rlkUnidadMedida";
            this.rlkUnidadMedida.NullText = "";
            this.rlkUnidadMedida.ValueMember = "ID";
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
            // 
            // repositoryItemSpinEdit3
            // 
            this.repositoryItemSpinEdit3.AutoHeight = false;
            this.repositoryItemSpinEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit3.Name = "repositoryItemSpinEdit3";
            // 
            // cboUnidad
            // 
            this.cboUnidad.AutoHeight = false;
            this.cboUnidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnidad.Name = "cboUnidad";
            // 
            // cboQExist
            // 
            this.cboQExist.AutoHeight = false;
            this.cboQExist.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear)});
            this.cboQExist.Name = "cboQExist";
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 346);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 361);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(848, 34);
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
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
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
            this.bntNew.Size = new System.Drawing.Size(65, 30);
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
            this.btnOK.Size = new System.Drawing.Size(84, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Guardar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // btnShowComprobante
            // 
            this.btnShowComprobante.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnShowComprobante.Image = ((System.Drawing.Image)(resources.GetObject("btnShowComprobante.Image")));
            this.btnShowComprobante.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnShowComprobante.Location = new System.Drawing.Point(733, 2);
            this.btnShowComprobante.LookAndFeel.SkinName = "McSkin";
            this.btnShowComprobante.Name = "btnShowComprobante";
            this.btnShowComprobante.Size = new System.Drawing.Size(113, 30);
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
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroup4.ExpandButtonVisible = true;
            this.layoutControlGroup4.Expanded = false;
            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup4.Name = "layoutControlGroupDatos";
            this.layoutControlGroup4.Size = new System.Drawing.Size(529, 150);
            this.layoutControlGroup4.Text = "layoutControlGroupDatos";
            this.layoutControlGroup4.TextVisible = false;
            // 
            // DialogTraslado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 395);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogTraslado";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Traslado";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtNroRecibido.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaRecibido.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaRecibido.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkESOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkSESIn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkESIn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkSESOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaTraslado.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaTraslado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkArea.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSESout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciSESin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoMovimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcFechaSalida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNroEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCantidadInicial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCantidadEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCtoUniEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rspCostoTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboQExist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infBloqueo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.TextEdit txtNoFactura;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem lblNoFactura;
        private DevExpress.XtraEditors.MemoEdit mmoComentario;
        private DevExpress.XtraLayout.LayoutControlItem lblComentario;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infBloqueo;
        private DevExpress.XtraEditors.DateEdit dateFechaTraslado;
        private DevExpress.XtraLayout.LayoutControlItem lblFechaAdquisicion;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        public DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraLayout.LayoutControlItem lblTipoMovimiento;
        private DevExpress.XtraEditors.LookUpEdit glkArea;
        private DevExpress.XtraEditors.LookUpEdit glkSESOut;
        private DevExpress.XtraLayout.LayoutControlItem lciSESout;
        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colProductID;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProducto;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlkUnidadMedida;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacenEntrada;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenEntrada;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacenSalida;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenSalida;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadExistencia;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadSalida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rspCostoTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colCostSalida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCostUnit;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoInicial;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboUnidad;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
        private DevExpress.XtraEditors.LookUpEdit glkSESIn;
        private DevExpress.XtraEditors.LookUpEdit glkESIn;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem lciSESin;
        private DevExpress.XtraEditors.LookUpEdit glkESOut;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboQExist;
        private DevExpress.XtraEditors.DateEdit dateFechaRecibido;
        private DevExpress.XtraLayout.LayoutControlItem lcFechaSalida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCantidadInicial;
        private DevExpress.XtraGrid.Columns.GridColumn colCostEntrada;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCtoUniEntrada;
        private DevExpress.XtraGrid.Columns.GridColumn colCantidadEntrada;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCantidadEntrada;
        private DevExpress.XtraEditors.TextEdit txtNroRecibido;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemNroEntrada;
    }
}