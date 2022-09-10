namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    partial class DialogTipoActivo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTipoActivo));
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblNombre = new DevExpress.XtraEditors.LabelControl();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.TextEdit();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.mmoDescripcion = new DevExpress.XtraEditors.MemoEdit();
            this.lblNota = new DevExpress.XtraEditors.LabelControl();
            this.chkTangible = new DevExpress.XtraEditors.CheckEdit();
            this.btnGuardar = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.lblCuentaActivo = new DevExpress.XtraEditors.LabelControl();
            this.lblCuentaGasto = new DevExpress.XtraEditors.LabelControl();
            this.lblCuentaDepreciacionAcumulada = new DevExpress.XtraEditors.LabelControl();
            this.lblDistribucionContable = new DevExpress.XtraEditors.LabelControl();
            this.glkvCuentaActivo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CuentaActivoID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaActivoCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaActivoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaActivoDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.glkCuentaActivo = new DevExpress.XtraEditors.GridLookUpEdit();
            this.glkvCuentaGasto = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.CuentaGastosID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaGastosCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaGastosNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CuentaGastosDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.glkCuentaGasto = new DevExpress.XtraEditors.GridLookUpEdit();
            this.glkvDepreciacionAcumulada = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DepreciacionAcumuladaID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DepreciacionAcumuladaCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DepreciacionAcumuladaNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DepreciacionAcumuladaDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.glkCuentaDepreciacionAcumulada = new DevExpress.XtraEditors.GridLookUpEdit();
            this.lblTipoEstado = new DevExpress.XtraEditors.LabelControl();
            this.chkDepreciable = new DevExpress.XtraEditors.CheckEdit();
            this.lblVidaUtil = new DevExpress.XtraEditors.LabelControl();
            this.seVidaUtil = new DevExpress.XtraEditors.SpinEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTangible.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvCuentaActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaActivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvCuentaGasto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaGasto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvDepreciacionAcumulada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaDepreciacionAcumulada.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepreciable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seVidaUtil.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(5, 24);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(38, 13);
            this.lblCodigo.TabIndex = 1;
            this.lblCodigo.Text = "Código";
            // 
            // lblNombre
            // 
            this.lblNombre.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(5, 69);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Location = new System.Drawing.Point(5, 113);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(54, 13);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripción";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(5, 43);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.DisplayFormat.FormatString = "{0:00}";
            this.txtCodigo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCodigo.Properties.EditFormat.FormatString = "{0:00}";
            this.txtCodigo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCodigo.Properties.Mask.EditMask = "99";
            this.txtCodigo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtCodigo.Size = new System.Drawing.Size(146, 20);
            this.txtCodigo.TabIndex = 24;
            this.txtCodigo.ToolTip = "Codigo del bien";
            this.txtCodigo.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(5, 87);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(308, 20);
            this.txtNombre.TabIndex = 25;
            this.txtNombre.ToolTip = "Nombre del bien";
            this.txtNombre.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // mmoDescripcion
            // 
            this.mmoDescripcion.Location = new System.Drawing.Point(5, 132);
            this.mmoDescripcion.Name = "mmoDescripcion";
            this.mmoDescripcion.Size = new System.Drawing.Size(308, 64);
            this.mmoDescripcion.TabIndex = 26;
            this.mmoDescripcion.UseOptimizedRendering = true;
            // 
            // lblNota
            // 
            this.lblNota.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNota.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNota.Location = new System.Drawing.Point(0, 265);
            this.lblNota.Name = "lblNota";
            this.lblNota.Size = new System.Drawing.Size(252, 13);
            this.lblNota.TabIndex = 35;
            this.lblNota.Text = "Nota: Los campos en negritas son requeridos";
            // 
            // chkTangible
            // 
            this.chkTangible.Location = new System.Drawing.Point(17, 232);
            this.chkTangible.Name = "chkTangible";
            this.chkTangible.Properties.Caption = "Tangible";
            this.chkTangible.Size = new System.Drawing.Size(96, 19);
            this.chkTangible.TabIndex = 57;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGuardar.Image = global::SAGAS.ActivoFijo.Properties.Resources.Ok20;
            this.btnGuardar.Location = new System.Drawing.Point(2, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 30);
            this.btnGuardar.TabIndex = 61;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancelar.Image = global::SAGAS.ActivoFijo.Properties.Resources.cancel20;
            this.btnCancelar.Location = new System.Drawing.Point(77, 2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(79, 30);
            this.btnCancelar.TabIndex = 62;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblCuentaActivo
            // 
            this.lblCuentaActivo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaActivo.Location = new System.Drawing.Point(346, 51);
            this.lblCuentaActivo.Name = "lblCuentaActivo";
            this.lblCuentaActivo.Size = new System.Drawing.Size(79, 13);
            this.lblCuentaActivo.TabIndex = 15;
            this.lblCuentaActivo.Text = "Cuenta Activo";
            // 
            // lblCuentaGasto
            // 
            this.lblCuentaGasto.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaGasto.Location = new System.Drawing.Point(346, 96);
            this.lblCuentaGasto.Name = "lblCuentaGasto";
            this.lblCuentaGasto.Size = new System.Drawing.Size(82, 13);
            this.lblCuentaGasto.TabIndex = 16;
            this.lblCuentaGasto.Text = "Cuenta Gastos";
            // 
            // lblCuentaDepreciacionAcumulada
            // 
            this.lblCuentaDepreciacionAcumulada.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCuentaDepreciacionAcumulada.Location = new System.Drawing.Point(346, 141);
            this.lblCuentaDepreciacionAcumulada.Name = "lblCuentaDepreciacionAcumulada";
            this.lblCuentaDepreciacionAcumulada.Size = new System.Drawing.Size(182, 13);
            this.lblCuentaDepreciacionAcumulada.TabIndex = 23;
            this.lblCuentaDepreciacionAcumulada.Text = "Cuenta Depreciación Acumulada";
            // 
            // lblDistribucionContable
            // 
            this.lblDistribucionContable.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistribucionContable.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblDistribucionContable.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDistribucionContable.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal;
            this.lblDistribucionContable.LineVisible = true;
            this.lblDistribucionContable.Location = new System.Drawing.Point(346, 24);
            this.lblDistribucionContable.Name = "lblDistribucionContable";
            this.lblDistribucionContable.Size = new System.Drawing.Size(282, 24);
            this.lblDistribucionContable.TabIndex = 49;
            this.lblDistribucionContable.Text = "Distribución Contable";
            // 
            // glkvCuentaActivo
            // 
            this.glkvCuentaActivo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CuentaActivoID,
            this.CuentaActivoCodigo,
            this.CuentaActivoNombre,
            this.CuentaActivoDisplay});
            this.glkvCuentaActivo.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.glkvCuentaActivo.Name = "glkvCuentaActivo";
            this.glkvCuentaActivo.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.glkvCuentaActivo.OptionsView.ShowGroupPanel = false;
            // 
            // CuentaActivoID
            // 
            this.CuentaActivoID.Caption = "ID";
            this.CuentaActivoID.FieldName = "ID";
            this.CuentaActivoID.Name = "CuentaActivoID";
            this.CuentaActivoID.Visible = true;
            this.CuentaActivoID.VisibleIndex = 2;
            // 
            // CuentaActivoCodigo
            // 
            this.CuentaActivoCodigo.Caption = "Codigo";
            this.CuentaActivoCodigo.FieldName = "Codigo";
            this.CuentaActivoCodigo.Name = "CuentaActivoCodigo";
            this.CuentaActivoCodigo.Visible = true;
            this.CuentaActivoCodigo.VisibleIndex = 0;
            // 
            // CuentaActivoNombre
            // 
            this.CuentaActivoNombre.Caption = "Nombre";
            this.CuentaActivoNombre.FieldName = "Nombre";
            this.CuentaActivoNombre.Name = "CuentaActivoNombre";
            this.CuentaActivoNombre.Visible = true;
            this.CuentaActivoNombre.VisibleIndex = 1;
            // 
            // CuentaActivoDisplay
            // 
            this.CuentaActivoDisplay.Caption = "Display";
            this.CuentaActivoDisplay.FieldName = "Display";
            this.CuentaActivoDisplay.Name = "CuentaActivoDisplay";
            this.CuentaActivoDisplay.Visible = true;
            this.CuentaActivoDisplay.VisibleIndex = 3;
            // 
            // glkCuentaActivo
            // 
            this.glkCuentaActivo.Location = new System.Drawing.Point(346, 70);
            this.glkCuentaActivo.Name = "glkCuentaActivo";
            this.glkCuentaActivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkCuentaActivo.Properties.DisplayMember = "Display";
            this.glkCuentaActivo.Properties.ValueMember = "ID";
            this.glkCuentaActivo.Properties.View = this.glkvCuentaActivo;
            this.glkCuentaActivo.Size = new System.Drawing.Size(274, 20);
            this.glkCuentaActivo.TabIndex = 50;
            // 
            // glkvCuentaGasto
            // 
            this.glkvCuentaGasto.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.CuentaGastosID,
            this.CuentaGastosCodigo,
            this.CuentaGastosNombre,
            this.CuentaGastosDisplay});
            this.glkvCuentaGasto.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.glkvCuentaGasto.Name = "glkvCuentaGasto";
            this.glkvCuentaGasto.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.glkvCuentaGasto.OptionsView.ShowGroupPanel = false;
            // 
            // CuentaGastosID
            // 
            this.CuentaGastosID.Caption = "ID";
            this.CuentaGastosID.FieldName = "ID";
            this.CuentaGastosID.Name = "CuentaGastosID";
            this.CuentaGastosID.Visible = true;
            this.CuentaGastosID.VisibleIndex = 0;
            // 
            // CuentaGastosCodigo
            // 
            this.CuentaGastosCodigo.Caption = "Codigo";
            this.CuentaGastosCodigo.FieldName = "Codigo";
            this.CuentaGastosCodigo.Name = "CuentaGastosCodigo";
            this.CuentaGastosCodigo.Visible = true;
            this.CuentaGastosCodigo.VisibleIndex = 1;
            // 
            // CuentaGastosNombre
            // 
            this.CuentaGastosNombre.Caption = "Nombre";
            this.CuentaGastosNombre.FieldName = "Nombre";
            this.CuentaGastosNombre.Name = "CuentaGastosNombre";
            this.CuentaGastosNombre.Visible = true;
            this.CuentaGastosNombre.VisibleIndex = 2;
            // 
            // CuentaGastosDisplay
            // 
            this.CuentaGastosDisplay.Caption = "Display";
            this.CuentaGastosDisplay.FieldName = "Display";
            this.CuentaGastosDisplay.Name = "CuentaGastosDisplay";
            this.CuentaGastosDisplay.Visible = true;
            this.CuentaGastosDisplay.VisibleIndex = 3;
            // 
            // glkCuentaGasto
            // 
            this.glkCuentaGasto.Location = new System.Drawing.Point(346, 115);
            this.glkCuentaGasto.Name = "glkCuentaGasto";
            this.glkCuentaGasto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkCuentaGasto.Properties.DisplayMember = "Display";
            this.glkCuentaGasto.Properties.ValueMember = "ID";
            this.glkCuentaGasto.Properties.View = this.glkvCuentaGasto;
            this.glkCuentaGasto.Size = new System.Drawing.Size(274, 20);
            this.glkCuentaGasto.TabIndex = 51;
            // 
            // glkvDepreciacionAcumulada
            // 
            this.glkvDepreciacionAcumulada.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DepreciacionAcumuladaID,
            this.DepreciacionAcumuladaCodigo,
            this.DepreciacionAcumuladaNombre,
            this.DepreciacionAcumuladaDisplay});
            this.glkvDepreciacionAcumulada.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.glkvDepreciacionAcumulada.Name = "glkvDepreciacionAcumulada";
            this.glkvDepreciacionAcumulada.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.glkvDepreciacionAcumulada.OptionsView.ShowGroupPanel = false;
            // 
            // DepreciacionAcumuladaID
            // 
            this.DepreciacionAcumuladaID.Caption = "ID";
            this.DepreciacionAcumuladaID.FieldName = "ID";
            this.DepreciacionAcumuladaID.Name = "DepreciacionAcumuladaID";
            this.DepreciacionAcumuladaID.Visible = true;
            this.DepreciacionAcumuladaID.VisibleIndex = 0;
            // 
            // DepreciacionAcumuladaCodigo
            // 
            this.DepreciacionAcumuladaCodigo.Caption = "Codigo";
            this.DepreciacionAcumuladaCodigo.FieldName = "Codigo";
            this.DepreciacionAcumuladaCodigo.Name = "DepreciacionAcumuladaCodigo";
            this.DepreciacionAcumuladaCodigo.Visible = true;
            this.DepreciacionAcumuladaCodigo.VisibleIndex = 1;
            // 
            // DepreciacionAcumuladaNombre
            // 
            this.DepreciacionAcumuladaNombre.Caption = "DepreciacionAcumuladaNombre";
            this.DepreciacionAcumuladaNombre.FieldName = "Nombre";
            this.DepreciacionAcumuladaNombre.Name = "DepreciacionAcumuladaNombre";
            this.DepreciacionAcumuladaNombre.Visible = true;
            this.DepreciacionAcumuladaNombre.VisibleIndex = 2;
            // 
            // DepreciacionAcumuladaDisplay
            // 
            this.DepreciacionAcumuladaDisplay.Caption = "Display";
            this.DepreciacionAcumuladaDisplay.FieldNameSortGroup = "Display";
            this.DepreciacionAcumuladaDisplay.Name = "DepreciacionAcumuladaDisplay";
            this.DepreciacionAcumuladaDisplay.Visible = true;
            this.DepreciacionAcumuladaDisplay.VisibleIndex = 3;
            // 
            // glkCuentaDepreciacionAcumulada
            // 
            this.glkCuentaDepreciacionAcumulada.Location = new System.Drawing.Point(346, 160);
            this.glkCuentaDepreciacionAcumulada.Name = "glkCuentaDepreciacionAcumulada";
            this.glkCuentaDepreciacionAcumulada.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkCuentaDepreciacionAcumulada.Properties.DisplayMember = "Display";
            this.glkCuentaDepreciacionAcumulada.Properties.ValueMember = "ID";
            this.glkCuentaDepreciacionAcumulada.Properties.View = this.glkvDepreciacionAcumulada;
            this.glkCuentaDepreciacionAcumulada.Size = new System.Drawing.Size(274, 20);
            this.glkCuentaDepreciacionAcumulada.TabIndex = 52;
            // 
            // lblTipoEstado
            // 
            this.lblTipoEstado.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoEstado.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblTipoEstado.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblTipoEstado.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal;
            this.lblTipoEstado.LineVisible = true;
            this.lblTipoEstado.Location = new System.Drawing.Point(5, 202);
            this.lblTipoEstado.Name = "lblTipoEstado";
            this.lblTipoEstado.Size = new System.Drawing.Size(308, 24);
            this.lblTipoEstado.TabIndex = 67;
            this.lblTipoEstado.Text = "Tipo y Estado";
            // 
            // chkDepreciable
            // 
            this.chkDepreciable.Location = new System.Drawing.Point(119, 232);
            this.chkDepreciable.Name = "chkDepreciable";
            this.chkDepreciable.Properties.Caption = "Depreciable";
            this.chkDepreciable.Size = new System.Drawing.Size(82, 19);
            this.chkDepreciable.TabIndex = 68;
            // 
            // lblVidaUtil
            // 
            this.lblVidaUtil.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVidaUtil.Location = new System.Drawing.Point(346, 186);
            this.lblVidaUtil.Name = "lblVidaUtil";
            this.lblVidaUtil.Size = new System.Drawing.Size(95, 13);
            this.lblVidaUtil.TabIndex = 69;
            this.lblVidaUtil.Text = "Vida Util (Meses)";
            // 
            // seVidaUtil
            // 
            this.seVidaUtil.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seVidaUtil.Location = new System.Drawing.Point(346, 206);
            this.seVidaUtil.Name = "seVidaUtil";
            this.seVidaUtil.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seVidaUtil.Properties.IsFloatValue = false;
            this.seVidaUtil.Properties.Mask.EditMask = "N00";
            this.seVidaUtil.Properties.MaxValue = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.seVidaUtil.Size = new System.Drawing.Size(100, 20);
            this.seVidaUtil.TabIndex = 70;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancelar);
            this.panelControl2.Controls.Add(this.btnGuardar);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 278);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(623, 34);
            this.panelControl2.TabIndex = 71;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblCodigo);
            this.groupControl1.Controls.Add(this.seVidaUtil);
            this.groupControl1.Controls.Add(this.mmoDescripcion);
            this.groupControl1.Controls.Add(this.lblVidaUtil);
            this.groupControl1.Controls.Add(this.lblNombre);
            this.groupControl1.Controls.Add(this.chkDepreciable);
            this.groupControl1.Controls.Add(this.lblDescripcion);
            this.groupControl1.Controls.Add(this.chkTangible);
            this.groupControl1.Controls.Add(this.lblCuentaActivo);
            this.groupControl1.Controls.Add(this.lblTipoEstado);
            this.groupControl1.Controls.Add(this.lblCuentaGasto);
            this.groupControl1.Controls.Add(this.lblCuentaDepreciacionAcumulada);
            this.groupControl1.Controls.Add(this.glkCuentaDepreciacionAcumulada);
            this.groupControl1.Controls.Add(this.txtCodigo);
            this.groupControl1.Controls.Add(this.glkCuentaGasto);
            this.groupControl1.Controls.Add(this.txtNombre);
            this.groupControl1.Controls.Add(this.glkCuentaActivo);
            this.groupControl1.Controls.Add(this.lblDistribucionContable);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(623, 265);
            this.groupControl1.TabIndex = 72;
            this.groupControl1.Text = "Datos Generales";
            // 
            // DialogTipoActivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(623, 312);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.lblNota);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogTipoActivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.MyFormTipoActivo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTangible.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvCuentaActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaActivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvCuentaGasto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaGasto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvDepreciacionAcumulada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkCuentaDepreciacionAcumulada.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepreciable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seVidaUtil.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblNombre;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.TextEdit txtCodigo;
        private DevExpress.XtraEditors.TextEdit txtNombre;
        private DevExpress.XtraEditors.MemoEdit mmoDescripcion;
        private DevExpress.XtraEditors.LabelControl lblNota;
        private DevExpress.XtraEditors.CheckEdit chkTangible;
        private DevExpress.XtraEditors.SimpleButton btnGuardar;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.LabelControl lblCuentaActivo;
        private DevExpress.XtraEditors.LabelControl lblCuentaGasto;
        private DevExpress.XtraEditors.LabelControl lblCuentaDepreciacionAcumulada;
        private DevExpress.XtraEditors.LabelControl lblDistribucionContable;
        private DevExpress.XtraGrid.Views.Grid.GridView glkvCuentaActivo;
        private DevExpress.XtraEditors.GridLookUpEdit glkCuentaActivo;
        private DevExpress.XtraGrid.Views.Grid.GridView glkvCuentaGasto;
        private DevExpress.XtraEditors.GridLookUpEdit glkCuentaGasto;
        private DevExpress.XtraGrid.Views.Grid.GridView glkvDepreciacionAcumulada;
        private DevExpress.XtraEditors.GridLookUpEdit glkCuentaDepreciacionAcumulada;
        private DevExpress.XtraEditors.LabelControl lblTipoEstado;
        private DevExpress.XtraEditors.CheckEdit chkDepreciable;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaActivoID;       
        private DevExpress.XtraGrid.Columns.GridColumn CuentaActivoDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaActivoCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaActivoNombre;
        private DevExpress.XtraEditors.LabelControl lblVidaUtil;
        private DevExpress.XtraEditors.SpinEdit seVidaUtil;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaGastosID;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaGastosCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaGastosNombre;
        private DevExpress.XtraGrid.Columns.GridColumn CuentaGastosDisplay;
        private DevExpress.XtraGrid.Columns.GridColumn DepreciacionAcumuladaID;
        private DevExpress.XtraGrid.Columns.GridColumn DepreciacionAcumuladaCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn DepreciacionAcumuladaNombre;
        private DevExpress.XtraGrid.Columns.GridColumn DepreciacionAcumuladaDisplay;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}
