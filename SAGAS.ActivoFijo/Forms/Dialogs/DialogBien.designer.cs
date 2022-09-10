namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    partial class DialogBien
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogBien));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.lkSus = new DevExpress.XtraEditors.LookUpEdit();
            this.lkEs = new DevExpress.XtraEditors.LookUpEdit();
            this.radioTipo = new DevExpress.XtraEditors.RadioGroup();
            this.chkAplicaIva = new DevExpress.XtraEditors.CheckEdit();
            this.spIva = new DevExpress.XtraEditors.SpinEdit();
            this.spSub = new DevExpress.XtraEditors.SpinEdit();
            this.spTotal = new DevExpress.XtraEditors.SpinEdit();
            this.lkDocumento = new DevExpress.XtraEditors.LookUpEdit();
            this.glkProvClientEstacion = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtTelefonos = new DevExpress.XtraEditors.TextEdit();
            this.txtRuc = new DevExpress.XtraEditors.TextEdit();
            this.dateFechaAdquisicion = new DevExpress.XtraEditors.DateEdit();
            this.mmoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.txtNoFactura = new DevExpress.XtraEditors.TextEdit();
            this.glkTipoMovimiento = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblProvClientEstacion = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblIVA = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblTotalFactura = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblComentario = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblAplicaIVA = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblTelefono = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblSubTotal = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblNoFactura = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblTipo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblTipoMovimiento = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblFechaAdquisicion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemEstacion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemSubEstacion = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblDocumento = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRuc = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridBien = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.gvBien = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpSpCodigo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colNombre = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colActivo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpLkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTipoActivo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpLkTipoActivo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDepreciacion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemCheckEditDepreciacion = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colUsuarioAsignado = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colArea = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpLkAreaID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colValorAdquisicion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpSpValor = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colValorDepreAcumul = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colVidaUtilMeses = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMesesDepreciados = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colNoSerie = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMarca = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colModelo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMatricula = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colChasis = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMotor = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDescripcion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speValor = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridCuenta = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDCC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigoCC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreCC = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkCentroCosto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spTax = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemGridLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.rpLkEstacion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEditSubEstacion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.lkSus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaIva.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIva.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSub.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDocumento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkProvClientEstacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefonos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRuc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaAdquisicion.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaAdquisicion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkTipoMovimiento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProvClientEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalFactura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAplicaIVA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTelefono)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoMovimiento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDocumento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRuc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCodigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkTipoActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDepreciacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkAreaID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCentroCosto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditSubEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.lkSus);
            this.layoutControlTop.Controls.Add(this.lkEs);
            this.layoutControlTop.Controls.Add(this.radioTipo);
            this.layoutControlTop.Controls.Add(this.chkAplicaIva);
            this.layoutControlTop.Controls.Add(this.spIva);
            this.layoutControlTop.Controls.Add(this.spSub);
            this.layoutControlTop.Controls.Add(this.spTotal);
            this.layoutControlTop.Controls.Add(this.lkDocumento);
            this.layoutControlTop.Controls.Add(this.glkProvClientEstacion);
            this.layoutControlTop.Controls.Add(this.txtTelefonos);
            this.layoutControlTop.Controls.Add(this.txtRuc);
            this.layoutControlTop.Controls.Add(this.dateFechaAdquisicion);
            this.layoutControlTop.Controls.Add(this.mmoComentario);
            this.layoutControlTop.Controls.Add(this.txtNoFactura);
            this.layoutControlTop.Controls.Add(this.glkTipoMovimiento);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(1081, 154);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // lkSus
            // 
            this.lkSus.Location = new System.Drawing.Point(885, 52);
            this.lkSus.Name = "lkSus";
            this.lkSus.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.lkSus.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lkSus.Properties.Appearance.Options.UseBackColor = true;
            this.lkSus.Properties.Appearance.Options.UseForeColor = true;
            this.lkSus.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.AliceBlue;
            this.lkSus.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Black;
            this.lkSus.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.lkSus.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.lkSus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSus.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Sub Estaciones")});
            this.lkSus.Properties.DisplayMember = "Nombre";
            this.lkSus.Properties.NullText = "<Seleccione la Sub Estación>";
            this.lkSus.Properties.ValueMember = "ID";
            this.lkSus.Size = new System.Drawing.Size(187, 20);
            this.lkSus.StyleController = this.layoutControlTop;
            this.lkSus.TabIndex = 7;
            // 
            // lkEs
            // 
            this.lkEs.Location = new System.Drawing.Point(885, 28);
            this.lkEs.Name = "lkEs";
            this.lkEs.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.lkEs.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lkEs.Properties.Appearance.Options.UseBackColor = true;
            this.lkEs.Properties.Appearance.Options.UseForeColor = true;
            this.lkEs.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.AliceBlue;
            this.lkEs.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Black;
            this.lkEs.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.lkEs.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.lkEs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkEs.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones")});
            this.lkEs.Properties.DisplayMember = "Nombre";
            this.lkEs.Properties.NullText = "<Seleccione la Estación>";
            this.lkEs.Properties.ValueMember = "ID";
            this.lkEs.Size = new System.Drawing.Size(187, 20);
            this.lkEs.StyleController = this.layoutControlTop;
            this.lkEs.TabIndex = 6;
            this.lkEs.EditValueChanged += new System.EventHandler(this.lkEs_EditValueChanged);
            // 
            // radioTipo
            // 
            this.radioTipo.Location = new System.Drawing.Point(9, 28);
            this.radioTipo.Name = "radioTipo";
            this.radioTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Alta")});
            this.radioTipo.Size = new System.Drawing.Size(75, 117);
            this.radioTipo.StyleController = this.layoutControlTop;
            this.radioTipo.TabIndex = 9;
            this.radioTipo.SelectedIndexChanged += new System.EventHandler(this.radioTipo_SelectedIndexChanged);
            // 
            // chkAplicaIva
            // 
            this.chkAplicaIva.Location = new System.Drawing.Point(418, 100);
            this.chkAplicaIva.Name = "chkAplicaIva";
            this.chkAplicaIva.Properties.Caption = "Aplica IVA?";
            this.chkAplicaIva.Size = new System.Drawing.Size(168, 19);
            this.chkAplicaIva.StyleController = this.layoutControlTop;
            this.chkAplicaIva.TabIndex = 6;
            this.chkAplicaIva.CheckedChanged += new System.EventHandler(this.chkAplicaIva_CheckedChanged);
            this.chkAplicaIva.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.chkAplicaIva_EditValueChanging);
            // 
            // spIva
            // 
            this.spIva.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spIva.Location = new System.Drawing.Point(685, 100);
            this.spIva.Name = "spIva";
            this.spIva.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spIva.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spIva.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spIva.Properties.DisplayFormat.FormatString = "n2";
            this.spIva.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spIva.Properties.EditFormat.FormatString = "n2";
            this.spIva.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spIva.Properties.Mask.EditMask = "n2";
            this.spIva.Properties.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.spIva.Size = new System.Drawing.Size(81, 20);
            this.spIva.StyleController = this.layoutControlTop;
            this.spIva.TabIndex = 9;
            this.spIva.EditValueChanged += new System.EventHandler(this.spIva_EditValueChanged);
            // 
            // spSub
            // 
            this.spSub.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spSub.Location = new System.Drawing.Point(533, 76);
            this.spSub.Name = "spSub";
            this.spSub.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spSub.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spSub.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.spSub.Properties.DisplayFormat.FormatString = "n2";
            this.spSub.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSub.Properties.EditFormat.FormatString = "n2";
            this.spSub.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spSub.Properties.Mask.EditMask = "n2";
            this.spSub.Properties.MaxValue = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.spSub.Size = new System.Drawing.Size(233, 20);
            this.spSub.StyleController = this.layoutControlTop;
            this.spSub.TabIndex = 8;
            this.spSub.EditValueChanged += new System.EventHandler(this.spSub_EditValueChanged);
            // 
            // spTotal
            // 
            this.spTotal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spTotal.Location = new System.Drawing.Point(533, 124);
            this.spTotal.Name = "spTotal";
            this.spTotal.Properties.AllowFocused = false;
            this.spTotal.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.spTotal.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.spTotal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.spTotal.Properties.DisplayFormat.FormatString = "n2";
            this.spTotal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTotal.Properties.EditFormat.FormatString = "n2";
            this.spTotal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTotal.Properties.Mask.EditMask = "n2";
            this.spTotal.Properties.MaxValue = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.spTotal.Properties.ReadOnly = true;
            this.spTotal.Size = new System.Drawing.Size(233, 20);
            this.spTotal.StyleController = this.layoutControlTop;
            this.spTotal.TabIndex = 7;
            this.spTotal.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // lkDocumento
            // 
            this.lkDocumento.Location = new System.Drawing.Point(188, 52);
            this.lkDocumento.Name = "lkDocumento";
            this.lkDocumento.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.lkDocumento.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lkDocumento.Properties.Appearance.Options.UseBackColor = true;
            this.lkDocumento.Properties.Appearance.Options.UseForeColor = true;
            this.lkDocumento.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.AliceBlue;
            this.lkDocumento.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Black;
            this.lkDocumento.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.lkDocumento.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.lkDocumento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkDocumento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Monedas")});
            this.lkDocumento.Properties.DisplayMember = "Display";
            this.lkDocumento.Properties.NullText = "<Seleccione el Documento>";
            this.lkDocumento.Properties.ValueMember = "ID";
            this.lkDocumento.Size = new System.Drawing.Size(216, 20);
            this.lkDocumento.StyleController = this.layoutControlTop;
            this.lkDocumento.TabIndex = 3;
            this.lkDocumento.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // glkProvClientEstacion
            // 
            this.glkProvClientEstacion.EditValue = "\"\"";
            this.glkProvClientEstacion.Location = new System.Drawing.Point(188, 76);
            this.glkProvClientEstacion.Name = "glkProvClientEstacion";
            this.glkProvClientEstacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkProvClientEstacion.Properties.DisplayMember = "Display";
            this.glkProvClientEstacion.Properties.NullText = "<Seleccione el Proveedor>";
            this.glkProvClientEstacion.Properties.PopupFormSize = new System.Drawing.Size(600, 0);
            this.glkProvClientEstacion.Properties.ValueMember = "ID";
            this.glkProvClientEstacion.Properties.View = this.gridLookUpEdit1View;
            this.glkProvClientEstacion.Size = new System.Drawing.Size(216, 20);
            this.glkProvClientEstacion.StyleController = this.layoutControlTop;
            this.glkProvClientEstacion.TabIndex = 2;
            this.glkProvClientEstacion.EditValueChanged += new System.EventHandler(this.glkProvClientEstacion_EditValueChanged);
            this.glkProvClientEstacion.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn4});
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
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Datos";
            this.gridColumn4.FieldName = "Display";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 289;
            // 
            // txtTelefonos
            // 
            this.txtTelefonos.Enabled = false;
            this.txtTelefonos.Location = new System.Drawing.Point(188, 125);
            this.txtTelefonos.Name = "txtTelefonos";
            this.txtTelefonos.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtTelefonos.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtTelefonos.Size = new System.Drawing.Size(216, 20);
            this.txtTelefonos.StyleController = this.layoutControlTop;
            this.txtTelefonos.TabIndex = 6;
            // 
            // txtRuc
            // 
            this.txtRuc.Enabled = false;
            this.txtRuc.Location = new System.Drawing.Point(188, 101);
            this.txtRuc.Name = "txtRuc";
            this.txtRuc.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.txtRuc.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.txtRuc.Size = new System.Drawing.Size(216, 20);
            this.txtRuc.StyleController = this.layoutControlTop;
            this.txtRuc.TabIndex = 5;
            // 
            // dateFechaAdquisicion
            // 
            this.dateFechaAdquisicion.EditValue = null;
            this.dateFechaAdquisicion.Location = new System.Drawing.Point(533, 28);
            this.dateFechaAdquisicion.Name = "dateFechaAdquisicion";
            this.dateFechaAdquisicion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaAdquisicion.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaAdquisicion.Size = new System.Drawing.Size(233, 20);
            this.dateFechaAdquisicion.StyleController = this.layoutControlTop;
            this.dateFechaAdquisicion.TabIndex = 4;
            this.dateFechaAdquisicion.EditValueChanged += new System.EventHandler(this.dateFechaCompra_EditValueChanged);
            this.dateFechaAdquisicion.Validated += new System.EventHandler(this.dateFechaCompra_Validated);
            // 
            // mmoComentario
            // 
            this.mmoComentario.Location = new System.Drawing.Point(770, 94);
            this.mmoComentario.Name = "mmoComentario";
            this.mmoComentario.Size = new System.Drawing.Size(302, 51);
            this.mmoComentario.StyleController = this.layoutControlTop;
            this.mmoComentario.TabIndex = 8;
            this.mmoComentario.UseOptimizedRendering = true;
            this.mmoComentario.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // txtNoFactura
            // 
            this.txtNoFactura.EditValue = "";
            this.txtNoFactura.Location = new System.Drawing.Point(533, 52);
            this.txtNoFactura.Name = "txtNoFactura";
            this.txtNoFactura.Size = new System.Drawing.Size(233, 20);
            this.txtNoFactura.StyleController = this.layoutControlTop;
            this.txtNoFactura.TabIndex = 6;
            this.txtNoFactura.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // glkTipoMovimiento
            // 
            this.glkTipoMovimiento.EditValue = "";
            this.glkTipoMovimiento.Location = new System.Drawing.Point(188, 28);
            this.glkTipoMovimiento.Name = "glkTipoMovimiento";
            this.glkTipoMovimiento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkTipoMovimiento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Movimientos", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.glkTipoMovimiento.Properties.DisplayMember = "Nombre";
            this.glkTipoMovimiento.Properties.ValueMember = "ID";
            this.glkTipoMovimiento.Size = new System.Drawing.Size(216, 20);
            this.glkTipoMovimiento.StyleController = this.layoutControlTop;
            this.glkTipoMovimiento.TabIndex = 10;
            this.glkTipoMovimiento.EditValueChanged += new System.EventHandler(this.glkTipoMovimiento_EditValueChanged);
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(1081, 154);
            this.layoutControlGroup1.Text = " ";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblProvClientEstacion,
            this.lblIVA,
            this.lblTotalFactura,
            this.lblComentario,
            this.lblAplicaIVA,
            this.lblTelefono,
            this.lblSubTotal,
            this.lblNoFactura,
            this.lblTipo,
            this.lblTipoMovimiento,
            this.lblFechaAdquisicion,
            this.layoutControlItemEstacion,
            this.layoutControlItemSubEstacion,
            this.lblDocumento,
            this.lblRuc,
            this.emptySpaceItem4});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(1073, 127);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // lblProvClientEstacion
            // 
            this.lblProvClientEstacion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblProvClientEstacion.AppearanceItemCaption.Options.UseFont = true;
            this.lblProvClientEstacion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblProvClientEstacion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblProvClientEstacion.Control = this.glkProvClientEstacion;
            this.lblProvClientEstacion.CustomizationFormText = "Proveedor";
            this.lblProvClientEstacion.Location = new System.Drawing.Point(79, 48);
            this.lblProvClientEstacion.MinSize = new System.Drawing.Size(154, 24);
            this.lblProvClientEstacion.Name = "lblProvClientEstacion";
            this.lblProvClientEstacion.Size = new System.Drawing.Size(320, 25);
            this.lblProvClientEstacion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblProvClientEstacion.Text = "Proveedor";
            this.lblProvClientEstacion.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblProvClientEstacion.TextSize = new System.Drawing.Size(95, 13);
            this.lblProvClientEstacion.TextToControlDistance = 5;
            this.lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblIVA
            // 
            this.lblIVA.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblIVA.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblIVA.Control = this.spIva;
            this.lblIVA.CustomizationFormText = "IVA";
            this.lblIVA.Location = new System.Drawing.Point(581, 72);
            this.lblIVA.MinSize = new System.Drawing.Size(149, 24);
            this.lblIVA.Name = "lblIVA";
            this.lblIVA.Size = new System.Drawing.Size(180, 24);
            this.lblIVA.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblIVA.Text = "IVA";
            this.lblIVA.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblIVA.TextSize = new System.Drawing.Size(90, 20);
            this.lblIVA.TextToControlDistance = 5;
            this.lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblTotalFactura
            // 
            this.lblTotalFactura.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTotalFactura.AppearanceItemCaption.Options.UseFont = true;
            this.lblTotalFactura.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblTotalFactura.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblTotalFactura.Control = this.spTotal;
            this.lblTotalFactura.CustomizationFormText = "Total Factura";
            this.lblTotalFactura.Location = new System.Drawing.Point(409, 96);
            this.lblTotalFactura.MinSize = new System.Drawing.Size(169, 24);
            this.lblTotalFactura.Name = "lblTotalFactura";
            this.lblTotalFactura.Size = new System.Drawing.Size(352, 25);
            this.lblTotalFactura.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblTotalFactura.Text = "Total Factura";
            this.lblTotalFactura.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblTotalFactura.TextSize = new System.Drawing.Size(110, 13);
            this.lblTotalFactura.TextToControlDistance = 5;
            this.lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblComentario
            // 
            this.lblComentario.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblComentario.AppearanceItemCaption.Options.UseFont = true;
            this.lblComentario.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblComentario.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblComentario.Control = this.mmoComentario;
            this.lblComentario.CustomizationFormText = "Comentario";
            this.lblComentario.Location = new System.Drawing.Point(761, 48);
            this.lblComentario.MinSize = new System.Drawing.Size(94, 38);
            this.lblComentario.Name = "lblComentario";
            this.lblComentario.Size = new System.Drawing.Size(306, 73);
            this.lblComentario.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblComentario.Text = "Comentario";
            this.lblComentario.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblComentario.TextLocation = DevExpress.Utils.Locations.Top;
            this.lblComentario.TextSize = new System.Drawing.Size(90, 13);
            this.lblComentario.TextToControlDistance = 5;
            this.lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblAplicaIVA
            // 
            this.lblAplicaIVA.Control = this.chkAplicaIva;
            this.lblAplicaIVA.CustomizationFormText = "layoutControlItem15";
            this.lblAplicaIVA.Location = new System.Drawing.Point(409, 72);
            this.lblAplicaIVA.MinSize = new System.Drawing.Size(78, 23);
            this.lblAplicaIVA.Name = "lblAplicaIVA";
            this.lblAplicaIVA.Size = new System.Drawing.Size(172, 24);
            this.lblAplicaIVA.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblAplicaIVA.Text = "lblAplicaIVA";
            this.lblAplicaIVA.TextSize = new System.Drawing.Size(0, 0);
            this.lblAplicaIVA.TextToControlDistance = 0;
            this.lblAplicaIVA.TextVisible = false;
            this.lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblTelefono
            // 
            this.lblTelefono.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblTelefono.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblTelefono.Control = this.txtTelefonos;
            this.lblTelefono.CustomizationFormText = "Teléfonos";
            this.lblTelefono.Location = new System.Drawing.Point(79, 97);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(320, 24);
            this.lblTelefono.Text = "Teléfonos";
            this.lblTelefono.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblTelefono.TextSize = new System.Drawing.Size(95, 13);
            this.lblTelefono.TextToControlDistance = 5;
            this.lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSubTotal.AppearanceItemCaption.Options.UseFont = true;
            this.lblSubTotal.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblSubTotal.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblSubTotal.Control = this.spSub;
            this.lblSubTotal.CustomizationFormText = "Sub Total";
            this.lblSubTotal.Location = new System.Drawing.Point(409, 48);
            this.lblSubTotal.MinSize = new System.Drawing.Size(169, 24);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(352, 24);
            this.lblSubTotal.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblSubTotal.Text = "Sub Total";
            this.lblSubTotal.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblSubTotal.TextSize = new System.Drawing.Size(110, 20);
            this.lblSubTotal.TextToControlDistance = 5;
            this.lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblNoFactura
            // 
            this.lblNoFactura.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNoFactura.AppearanceItemCaption.Options.UseFont = true;
            this.lblNoFactura.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblNoFactura.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblNoFactura.Control = this.txtNoFactura;
            this.lblNoFactura.CustomizationFormText = "No. de Factura";
            this.lblNoFactura.Location = new System.Drawing.Point(409, 24);
            this.lblNoFactura.MinSize = new System.Drawing.Size(169, 24);
            this.lblNoFactura.Name = "lblNoFactura";
            this.lblNoFactura.Size = new System.Drawing.Size(352, 24);
            this.lblNoFactura.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblNoFactura.Text = "No. de Factura";
            this.lblNoFactura.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblNoFactura.TextSize = new System.Drawing.Size(110, 13);
            this.lblNoFactura.TextToControlDistance = 5;
            this.lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblTipo
            // 
            this.lblTipo.Control = this.radioTipo;
            this.lblTipo.CustomizationFormText = "lblTipo";
            this.lblTipo.Location = new System.Drawing.Point(0, 0);
            this.lblTipo.MaxSize = new System.Drawing.Size(79, 0);
            this.lblTipo.MinSize = new System.Drawing.Size(79, 29);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(79, 121);
            this.lblTipo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblTipo.Text = "lblTipo";
            this.lblTipo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblTipo.TextSize = new System.Drawing.Size(0, 0);
            this.lblTipo.TextToControlDistance = 0;
            this.lblTipo.TextVisible = false;
            // 
            // lblTipoMovimiento
            // 
            this.lblTipoMovimiento.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblTipoMovimiento.AppearanceItemCaption.Options.UseFont = true;
            this.lblTipoMovimiento.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblTipoMovimiento.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblTipoMovimiento.Control = this.glkTipoMovimiento;
            this.lblTipoMovimiento.CustomizationFormText = "Tipo Movimiento";
            this.lblTipoMovimiento.Location = new System.Drawing.Point(79, 0);
            this.lblTipoMovimiento.MaxSize = new System.Drawing.Size(320, 24);
            this.lblTipoMovimiento.MinSize = new System.Drawing.Size(320, 24);
            this.lblTipoMovimiento.Name = "lblTipoMovimiento";
            this.lblTipoMovimiento.Size = new System.Drawing.Size(320, 24);
            this.lblTipoMovimiento.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblTipoMovimiento.Text = "Tipo Movimiento";
            this.lblTipoMovimiento.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblTipoMovimiento.TextSize = new System.Drawing.Size(95, 13);
            this.lblTipoMovimiento.TextToControlDistance = 5;
            // 
            // lblFechaAdquisicion
            // 
            this.lblFechaAdquisicion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseFont = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblFechaAdquisicion.Control = this.dateFechaAdquisicion;
            this.lblFechaAdquisicion.CustomizationFormText = "Fecha Adquisición";
            this.lblFechaAdquisicion.Location = new System.Drawing.Point(409, 0);
            this.lblFechaAdquisicion.MaxSize = new System.Drawing.Size(352, 0);
            this.lblFechaAdquisicion.MinSize = new System.Drawing.Size(352, 24);
            this.lblFechaAdquisicion.Name = "lblFechaAdquisicion";
            this.lblFechaAdquisicion.Size = new System.Drawing.Size(352, 24);
            this.lblFechaAdquisicion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblFechaAdquisicion.Text = "Fecha Adquisición";
            this.lblFechaAdquisicion.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblFechaAdquisicion.TextSize = new System.Drawing.Size(110, 13);
            this.lblFechaAdquisicion.TextToControlDistance = 5;
            this.lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItemEstacion
            // 
            this.layoutControlItemEstacion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemEstacion.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemEstacion.Control = this.lkEs;
            this.layoutControlItemEstacion.CustomizationFormText = "Estación de Servicio";
            this.layoutControlItemEstacion.Location = new System.Drawing.Point(761, 0);
            this.layoutControlItemEstacion.MinSize = new System.Drawing.Size(169, 24);
            this.layoutControlItemEstacion.Name = "layoutControlItemEstacion";
            this.layoutControlItemEstacion.Size = new System.Drawing.Size(306, 24);
            this.layoutControlItemEstacion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemEstacion.Text = "Estación de Servicio";
            this.layoutControlItemEstacion.TextSize = new System.Drawing.Size(112, 13);
            this.layoutControlItemEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItemSubEstacion
            // 
            this.layoutControlItemSubEstacion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemSubEstacion.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemSubEstacion.Control = this.lkSus;
            this.layoutControlItemSubEstacion.CustomizationFormText = "Sub Estación";
            this.layoutControlItemSubEstacion.Location = new System.Drawing.Point(761, 24);
            this.layoutControlItemSubEstacion.MinSize = new System.Drawing.Size(169, 24);
            this.layoutControlItemSubEstacion.Name = "layoutControlItemSubEstacion";
            this.layoutControlItemSubEstacion.Size = new System.Drawing.Size(306, 24);
            this.layoutControlItemSubEstacion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemSubEstacion.Text = "Sub Estación";
            this.layoutControlItemSubEstacion.TextSize = new System.Drawing.Size(112, 13);
            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblDocumento
            // 
            this.lblDocumento.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblDocumento.AppearanceItemCaption.Options.UseFont = true;
            this.lblDocumento.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblDocumento.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblDocumento.Control = this.lkDocumento;
            this.lblDocumento.CustomizationFormText = "Tipo de Moneda";
            this.lblDocumento.Location = new System.Drawing.Point(79, 24);
            this.lblDocumento.MinSize = new System.Drawing.Size(169, 24);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(320, 24);
            this.lblDocumento.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblDocumento.Text = "Documento";
            this.lblDocumento.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblDocumento.TextSize = new System.Drawing.Size(95, 13);
            this.lblDocumento.TextToControlDistance = 5;
            this.lblDocumento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblRuc
            // 
            this.lblRuc.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
            this.lblRuc.AppearanceItemCaption.Options.UseForeColor = true;
            this.lblRuc.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblRuc.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lblRuc.Control = this.txtRuc;
            this.lblRuc.CustomizationFormText = "Ruc";
            this.lblRuc.Location = new System.Drawing.Point(79, 73);
            this.lblRuc.Name = "lblRuc";
            this.lblRuc.Size = new System.Drawing.Size(320, 24);
            this.lblRuc.Text = "Ruc / N. Comercial";
            this.lblRuc.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblRuc.TextSize = new System.Drawing.Size(95, 13);
            this.lblRuc.TextToControlDistance = 5;
            this.lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.CustomizationFormText = "emptySpaceItem4";
            this.emptySpaceItem4.Location = new System.Drawing.Point(399, 0);
            this.emptySpaceItem4.MaxSize = new System.Drawing.Size(10, 0);
            this.emptySpaceItem4.MinSize = new System.Drawing.Size(10, 10);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(10, 121);
            this.emptySpaceItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem4.Text = "emptySpaceItem4";
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridBien);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1085, 414);
            this.panelControl1.TabIndex = 0;
            // 
            // gridBien
            // 
            this.gridBien.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridBien.DataSource = this.bdsDetalle;
            this.gridBien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBien.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridBien.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridDetalle_EmbeddedNavigator_ButtonClick);
            this.gridBien.Location = new System.Drawing.Point(2, 156);
            this.gridBien.MainView = this.gvBien;
            this.gridBien.Name = "gridBien";
            this.gridBien.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speValor,
            this.speCost,
            this.cboUnidadMedida,
            this.gridCuenta,
            this.lkCentroCosto,
            this.spSubTotal,
            this.spMontoISC,
            this.spTax,
            this.repositoryItemGridLookUpEdit1,
            this.rpLkEstacion,
            this.repositoryItemLookUpEditSubEstacion,
            this.repositoryItemCheckEditDepreciacion,
            this.repositoryItemLookUpEdit1,
            this.rpLkActivo,
            this.rpLkAreaID,
            this.rpLkTipoActivo,
            this.rpSpValor,
            this.rpSpCodigo});
            this.gridBien.Size = new System.Drawing.Size(1081, 243);
            this.gridBien.TabIndex = 8;
            this.gridBien.UseEmbeddedNavigator = true;
            this.gridBien.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvBien});
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // gvBien
            // 
            this.gvBien.ActiveFilterEnabled = false;
            this.gvBien.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvBien.Appearance.FooterPanel.Options.UseFont = true;
            this.gvBien.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvBien.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombre,
            this.colActivo,
            this.colValorAdquisicion,
            this.colValorDepreAcumul,
            this.colVidaUtilMeses,
            this.colMesesDepreciados,
            this.colArea,
            this.colUsuarioAsignado,
            this.colNoSerie,
            this.colMarca,
            this.colModelo,
            this.colMatricula,
            this.colChasis,
            this.colMotor,
            this.colDescripcion,
            this.colDepreciacion,
            this.colTipoActivo});
            this.gvBien.GridControl = this.gridBien;
            this.gvBien.Name = "gvBien";
            this.gvBien.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvBien.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvBien.OptionsCustomization.AllowBandMoving = false;
            this.gvBien.OptionsCustomization.AllowBandResizing = false;
            this.gvBien.OptionsCustomization.AllowFilter = false;
            this.gvBien.OptionsCustomization.AllowGroup = false;
            this.gvBien.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvBien.OptionsCustomization.AllowSort = false;
            this.gvBien.OptionsFilter.AllowFilterEditor = false;
            this.gvBien.OptionsFilter.AllowMRUFilterList = false;
            this.gvBien.OptionsLayout.Columns.AddNewColumns = false;
            this.gvBien.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvBien.OptionsNavigation.AutoFocusNewRow = true;
            this.gvBien.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            this.gvBien.OptionsView.ShowBands = false;
            this.gvBien.OptionsView.ShowFooter = true;
            this.gvBien.OptionsView.ShowGroupPanel = false;
            this.gvBien.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.gvDetalle_ShowingEditor);
            this.gvBien.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvDetalle_FocusedRowChanged);
            this.gvBien.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDetalle_CellValueChanged);
            this.gvBien.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvDetalle_InvalidRowException);
            this.gvBien.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvDetalle_ValidateRow);
            this.gvBien.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gvDetalle_KeyDown);
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.colID);
            this.gridBand1.Columns.Add(this.colCodigo);
            this.gridBand1.Columns.Add(this.colNombre);
            this.gridBand1.Columns.Add(this.colActivo);
            this.gridBand1.Columns.Add(this.colTipoActivo);
            this.gridBand1.Columns.Add(this.colDepreciacion);
            this.gridBand1.Columns.Add(this.colUsuarioAsignado);
            this.gridBand1.Columns.Add(this.colArea);
            this.gridBand1.Columns.Add(this.colValorAdquisicion);
            this.gridBand1.Columns.Add(this.colValorDepreAcumul);
            this.gridBand1.Columns.Add(this.colVidaUtilMeses);
            this.gridBand1.Columns.Add(this.colMesesDepreciados);
            this.gridBand1.Columns.Add(this.colNoSerie);
            this.gridBand1.Columns.Add(this.colMarca);
            this.gridBand1.Columns.Add(this.colModelo);
            this.gridBand1.Columns.Add(this.colMatricula);
            this.gridBand1.Columns.Add(this.colChasis);
            this.gridBand1.Columns.Add(this.colMotor);
            this.gridBand1.Columns.Add(this.colDescripcion);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 1773;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Width = 64;
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Codigo";
            this.colCodigo.ColumnEdit = this.rpSpCodigo;
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.Width = 93;
            // 
            // rpSpCodigo
            // 
            this.rpSpCodigo.AutoHeight = false;
            this.rpSpCodigo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
            this.rpSpCodigo.DisplayFormat.FormatString = "f0";
            this.rpSpCodigo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpCodigo.EditFormat.FormatString = "f0";
            this.rpSpCodigo.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpCodigo.Mask.EditMask = "f0";
            this.rpSpCodigo.Name = "rpSpCodigo";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.Width = 141;
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rpLkActivo;
            this.colActivo.FieldName = "ActivoID";
            this.colActivo.Name = "colActivo";
            this.colActivo.Visible = true;
            this.colActivo.Width = 140;
            // 
            // rpLkActivo
            // 
            this.rpLkActivo.AutoHeight = false;
            this.rpLkActivo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpLkActivo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Activos", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.rpLkActivo.DisplayMember = "Nombre";
            this.rpLkActivo.Name = "rpLkActivo";
            this.rpLkActivo.ValueMember = "ID";
            // 
            // colTipoActivo
            // 
            this.colTipoActivo.Caption = "Tipo Activo";
            this.colTipoActivo.ColumnEdit = this.rpLkTipoActivo;
            this.colTipoActivo.FieldName = "TipoActivoID";
            this.colTipoActivo.Name = "colTipoActivo";
            this.colTipoActivo.OptionsColumn.AllowFocus = false;
            this.colTipoActivo.Visible = true;
            this.colTipoActivo.Width = 121;
            // 
            // rpLkTipoActivo
            // 
            this.rpLkTipoActivo.AutoHeight = false;
            this.rpLkTipoActivo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpLkTipoActivo.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipo Activo")});
            this.rpLkTipoActivo.DisplayMember = "Nombre";
            this.rpLkTipoActivo.Name = "rpLkTipoActivo";
            this.rpLkTipoActivo.ValueMember = "ID";
            // 
            // colDepreciacion
            // 
            this.colDepreciacion.Caption = "Aplica Depreciación";
            this.colDepreciacion.ColumnEdit = this.repositoryItemCheckEditDepreciacion;
            this.colDepreciacion.FieldName = "EsDepreciado";
            this.colDepreciacion.Name = "colDepreciacion";
            this.colDepreciacion.Visible = true;
            this.colDepreciacion.Width = 113;
            // 
            // repositoryItemCheckEditDepreciacion
            // 
            this.repositoryItemCheckEditDepreciacion.AutoHeight = false;
            this.repositoryItemCheckEditDepreciacion.Caption = "Check";
            this.repositoryItemCheckEditDepreciacion.Name = "repositoryItemCheckEditDepreciacion";
            // 
            // colUsuarioAsignado
            // 
            this.colUsuarioAsignado.Caption = "Usuario Asignado";
            this.colUsuarioAsignado.FieldName = "UsuarioAsignado";
            this.colUsuarioAsignado.Name = "colUsuarioAsignado";
            this.colUsuarioAsignado.Visible = true;
            this.colUsuarioAsignado.Width = 118;
            // 
            // colArea
            // 
            this.colArea.Caption = "Área";
            this.colArea.ColumnEdit = this.rpLkAreaID;
            this.colArea.FieldName = "AreaID";
            this.colArea.Name = "colArea";
            this.colArea.Visible = true;
            this.colArea.Width = 131;
            // 
            // rpLkAreaID
            // 
            this.rpLkAreaID.AutoHeight = false;
            this.rpLkAreaID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpLkAreaID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Áreas")});
            this.rpLkAreaID.DisplayMember = "Nombre";
            this.rpLkAreaID.Name = "rpLkAreaID";
            this.rpLkAreaID.ValueMember = "ID";
            // 
            // colValorAdquisicion
            // 
            this.colValorAdquisicion.Caption = "Valor Adquisición";
            this.colValorAdquisicion.ColumnEdit = this.rpSpValor;
            this.colValorAdquisicion.FieldName = "ValorAdquisicion";
            this.colValorAdquisicion.Name = "colValorAdquisicion";
            this.colValorAdquisicion.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ValorAdquisicion", "{0:N2}")});
            this.colValorAdquisicion.Visible = true;
            this.colValorAdquisicion.Width = 116;
            // 
            // rpSpValor
            // 
            this.rpSpValor.AutoHeight = false;
            this.rpSpValor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpSpValor.DisplayFormat.FormatString = "n2";
            this.rpSpValor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpValor.EditFormat.FormatString = "n2";
            this.rpSpValor.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpValor.Mask.EditMask = "n2";
            this.rpSpValor.Name = "rpSpValor";
            // 
            // colValorDepreAcumul
            // 
            this.colValorDepreAcumul.Caption = "Valor Depre. Acum.";
            this.colValorDepreAcumul.Name = "colValorDepreAcumul";
            this.colValorDepreAcumul.Width = 113;
            // 
            // colVidaUtilMeses
            // 
            this.colVidaUtilMeses.Caption = "Vida Util (Meses)";
            this.colVidaUtilMeses.Name = "colVidaUtilMeses";
            this.colVidaUtilMeses.Width = 93;
            // 
            // colMesesDepreciados
            // 
            this.colMesesDepreciados.Caption = "Meses Depreciados";
            this.colMesesDepreciados.Name = "colMesesDepreciados";
            this.colMesesDepreciados.Width = 110;
            // 
            // colNoSerie
            // 
            this.colNoSerie.Caption = "No. Serie";
            this.colNoSerie.FieldName = "NoSerie";
            this.colNoSerie.Name = "colNoSerie";
            this.colNoSerie.Visible = true;
            this.colNoSerie.Width = 88;
            // 
            // colMarca
            // 
            this.colMarca.Caption = "Marca";
            this.colMarca.FieldName = "Marca";
            this.colMarca.Name = "colMarca";
            this.colMarca.Visible = true;
            this.colMarca.Width = 109;
            // 
            // colModelo
            // 
            this.colModelo.Caption = "Modelo";
            this.colModelo.FieldName = "Modelo";
            this.colModelo.Name = "colModelo";
            this.colModelo.Visible = true;
            this.colModelo.Width = 96;
            // 
            // colMatricula
            // 
            this.colMatricula.Caption = "Matricula";
            this.colMatricula.FieldName = "Matricula";
            this.colMatricula.Name = "colMatricula";
            this.colMatricula.Visible = true;
            this.colMatricula.Width = 86;
            // 
            // colChasis
            // 
            this.colChasis.Caption = "Chasis";
            this.colChasis.FieldName = "Chasis";
            this.colChasis.Name = "colChasis";
            this.colChasis.Visible = true;
            this.colChasis.Width = 72;
            // 
            // colMotor
            // 
            this.colMotor.Caption = "Motor";
            this.colMotor.FieldName = "Motor";
            this.colMotor.Name = "colMotor";
            this.colMotor.Visible = true;
            this.colMotor.Width = 55;
            // 
            // colDescripcion
            // 
            this.colDescripcion.Caption = "Descripcion";
            this.colDescripcion.FieldName = "Descripcion";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.Visible = true;
            this.colDescripcion.Width = 294;
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
            // speValor
            // 
            this.speValor.AutoHeight = false;
            this.speValor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speValor.DisplayFormat.FormatString = "n2";
            this.speValor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speValor.EditFormat.FormatString = "n2";
            this.speValor.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speValor.Mask.EditMask = "n2";
            this.speValor.MaxValue = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.speValor.MinValue = new decimal(new int[] {
            1000000000,
            0,
            0,
            -2147483648});
            this.speValor.Name = "speValor";
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
            // gridCuenta
            // 
            this.gridCuenta.AutoHeight = false;
            this.gridCuenta.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridCuenta.Name = "gridCuenta";
            this.gridCuenta.NullText = "";
            this.gridCuenta.View = this.gridViewProductos;
            // 
            // gridViewProductos
            // 
            this.gridViewProductos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDCC,
            this.colCodigoCC,
            this.colNombreCC});
            this.gridViewProductos.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewProductos.Name = "gridViewProductos";
            this.gridViewProductos.OptionsBehavior.Editable = false;
            this.gridViewProductos.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewProductos.OptionsView.ShowAutoFilterRow = true;
            this.gridViewProductos.OptionsView.ShowGroupPanel = false;
            // 
            // colIDCC
            // 
            this.colIDCC.FieldName = "ID";
            this.colIDCC.Name = "colIDCC";
            // 
            // colCodigoCC
            // 
            this.colCodigoCC.Caption = "Código";
            this.colCodigoCC.FieldName = "Codigo";
            this.colCodigoCC.Name = "colCodigoCC";
            this.colCodigoCC.Visible = true;
            this.colCodigoCC.VisibleIndex = 0;
            this.colCodigoCC.Width = 293;
            // 
            // colNombreCC
            // 
            this.colNombreCC.Caption = "Cuenta Contable";
            this.colNombreCC.FieldName = "Display";
            this.colNombreCC.Name = "colNombreCC";
            this.colNombreCC.Visible = true;
            this.colNombreCC.VisibleIndex = 1;
            this.colNombreCC.Width = 523;
            // 
            // lkCentroCosto
            // 
            this.lkCentroCosto.AutoHeight = false;
            this.lkCentroCosto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCentroCosto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Centro de Costo")});
            this.lkCentroCosto.Name = "lkCentroCosto";
            this.lkCentroCosto.NullText = "";
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
            // repositoryItemGridLookUpEdit1
            // 
            this.repositoryItemGridLookUpEdit1.AutoHeight = false;
            this.repositoryItemGridLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEdit1.Name = "repositoryItemGridLookUpEdit1";
            this.repositoryItemGridLookUpEdit1.View = this.repositoryItemGridLookUpEdit1View;
            // 
            // repositoryItemGridLookUpEdit1View
            // 
            this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
            this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
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
            // repositoryItemLookUpEditSubEstacion
            // 
            this.repositoryItemLookUpEditSubEstacion.AutoHeight = false;
            this.repositoryItemLookUpEditSubEstacion.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditSubEstacion.Name = "repositoryItemLookUpEditSubEstacion";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 399);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 414);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1085, 34);
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
            this.btnShowComprobante.Location = new System.Drawing.Point(970, 2);
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
            // infDifES
            // 
            this.infDifES.ContainerControl = this;
            // 
            // errTC_Periodo
            // 
            this.errTC_Periodo.ContainerControl = this;
            // 
            // DialogBien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 448);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogBien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crear alta de Activo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkSus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaIva.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIva.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSub.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDocumento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkProvClientEstacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTelefonos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRuc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaAdquisicion.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaAdquisicion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkTipoMovimiento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblProvClientEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblIVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTotalFactura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAplicaIVA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTelefono)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblTipoMovimiento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDocumento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblRuc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCodigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkTipoActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDepreciacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkAreaID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpValor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speValor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCentroCosto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditSubEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
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
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.DateEdit dateFechaAdquisicion;
        private DevExpress.XtraLayout.LayoutControlItem lblFechaAdquisicion;
        public DevExpress.XtraGrid.GridControl gridBien;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridCuenta;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speValor;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkCentroCosto;
        private DevExpress.XtraEditors.SimpleButton btnShowComprobante;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        private DevExpress.XtraEditors.TextEdit txtRuc;
        private DevExpress.XtraLayout.LayoutControlItem lblRuc;
        private DevExpress.XtraEditors.TextEdit txtTelefonos;
        private DevExpress.XtraLayout.LayoutControlItem lblTelefono;
        private DevExpress.XtraEditors.GridLookUpEdit glkProvClientEstacion;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraLayout.LayoutControlItem lblProvClientEstacion;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.LookUpEdit lkDocumento;
        private DevExpress.XtraLayout.LayoutControlItem lblDocumento;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView gvBien;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        private DevExpress.XtraEditors.SpinEdit spTotal;
        private DevExpress.XtraLayout.LayoutControlItem lblTotalFactura;
        private DevExpress.XtraGrid.Columns.GridColumn colIDCC;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoCC;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreCC;
        private DevExpress.XtraEditors.SpinEdit spIva;
        private DevExpress.XtraEditors.SpinEdit spSub;
        private DevExpress.XtraLayout.LayoutControlItem lblSubTotal;
        private DevExpress.XtraLayout.LayoutControlItem lblIVA;
        private DevExpress.XtraEditors.CheckEdit chkAplicaIva;
        private DevExpress.XtraLayout.LayoutControlItem lblAplicaIVA;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        public DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.RadioGroup radioTipo;
        private DevExpress.XtraLayout.LayoutControlItem lblTipo;
        private DevExpress.XtraLayout.LayoutControlItem lblTipoMovimiento;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colActivo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNombre;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colValorAdquisicion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colValorDepreAcumul;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colArea;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colUsuarioAsignado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNoSerie;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMarca;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colModelo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMatricula;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colChasis;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMotor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDescripcion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colVidaUtilMeses;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMesesDepreciados;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.LookUpEdit glkTipoMovimiento;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDepreciacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditDepreciacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkAreaID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTipoActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkTipoActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.LookUpEdit lkSus;
        private DevExpress.XtraEditors.LookUpEdit lkEs;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemEstacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpValor;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
    }
}