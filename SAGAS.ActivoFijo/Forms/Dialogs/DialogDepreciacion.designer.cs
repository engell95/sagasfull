namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    partial class DialogDepreciacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogDepreciacion));
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.lkSus = new DevExpress.XtraEditors.LookUpEdit();
            this.lkEs = new DevExpress.XtraEditors.LookUpEdit();
            this.dateFechaDepre = new DevExpress.XtraEditors.DateEdit();
            this.mmoComentario = new DevExpress.XtraEditors.MemoEdit();
            this.txtNoFactura = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroupDatos = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lblNoFactura = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblFechaAdquisicion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemEstacion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemSubEstacion = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem4 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lblComentario = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridBien = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.gvBien = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpCodigo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNoSerie = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModelo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.repositoryItemCheckEditDepreciacion = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpLkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpLkAreaID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpLkTipoActivo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpSpValor = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
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
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDepre.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDepre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubEstacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBien)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCodigo)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDepreciacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkAreaID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkTipoActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.btnLoad);
            this.layoutControlTop.Controls.Add(this.lkSus);
            this.layoutControlTop.Controls.Add(this.lkEs);
            this.layoutControlTop.Controls.Add(this.dateFechaDepre);
            this.layoutControlTop.Controls.Add(this.mmoComentario);
            this.layoutControlTop.Controls.Add(this.txtNoFactura);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(1081, 148);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::SAGAS.ActivoFijo.Properties.Resources.application_put1;
            this.btnLoad.Location = new System.Drawing.Point(270, 52);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(97, 22);
            this.btnLoad.StyleController = this.layoutControlTop;
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Cargar Bienes";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lkSus
            // 
            this.lkSus.Location = new System.Drawing.Point(486, 52);
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
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Sub Estaciones")});
            this.lkSus.Properties.DisplayMember = "Display";
            this.lkSus.Properties.NullText = "<Seleccione la Sub Estación>";
            this.lkSus.Properties.ValueMember = "ID";
            this.lkSus.Size = new System.Drawing.Size(586, 20);
            this.lkSus.StyleController = this.layoutControlTop;
            this.lkSus.TabIndex = 7;
            // 
            // lkEs
            // 
            this.lkEs.Location = new System.Drawing.Point(486, 28);
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
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Estaciones", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkEs.Properties.DisplayMember = "Display";
            this.lkEs.Properties.NullText = "<Seleccione la Estación>";
            this.lkEs.Properties.ValueMember = "ID";
            this.lkEs.Size = new System.Drawing.Size(586, 20);
            this.lkEs.StyleController = this.layoutControlTop;
            this.lkEs.TabIndex = 6;
            this.lkEs.EditValueChanged += new System.EventHandler(this.lkEs_EditValueChanged);
            // 
            // dateFechaDepre
            // 
            this.dateFechaDepre.EditValue = null;
            this.dateFechaDepre.Location = new System.Drawing.Point(134, 28);
            this.dateFechaDepre.Name = "dateFechaDepre";
            this.dateFechaDepre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaDepre.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaDepre.Size = new System.Drawing.Size(233, 20);
            this.dateFechaDepre.StyleController = this.layoutControlTop;
            this.dateFechaDepre.TabIndex = 4;
            this.dateFechaDepre.EditValueChanged += new System.EventHandler(this.dateFechaDepre_EditValueChanged);
            this.dateFechaDepre.Validated += new System.EventHandler(this.dateFechaCompra_Validated);
            // 
            // mmoComentario
            // 
            this.mmoComentario.Location = new System.Drawing.Point(19, 96);
            this.mmoComentario.Name = "mmoComentario";
            this.mmoComentario.Size = new System.Drawing.Size(1053, 43);
            this.mmoComentario.StyleController = this.layoutControlTop;
            this.mmoComentario.TabIndex = 8;
            this.mmoComentario.UseOptimizedRendering = true;
            this.mmoComentario.Validated += new System.EventHandler(this.txtNombre_Validated_1);
            // 
            // txtNoFactura
            // 
            this.txtNoFactura.EditValue = "";
            this.txtNoFactura.Location = new System.Drawing.Point(134, 52);
            this.txtNoFactura.Name = "txtNoFactura";
            this.txtNoFactura.Properties.ReadOnly = true;
            this.txtNoFactura.Size = new System.Drawing.Size(132, 20);
            this.txtNoFactura.StyleController = this.layoutControlTop;
            this.txtNoFactura.TabIndex = 6;
            this.txtNoFactura.Validated += new System.EventHandler(this.txtNombre_Validated_1);
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
            this.layoutControlGroup1.Size = new System.Drawing.Size(1081, 148);
            this.layoutControlGroup1.Text = " ";
            // 
            // layoutControlGroupDatos
            // 
            this.layoutControlGroupDatos.CustomizationFormText = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lblNoFactura,
            this.lblFechaAdquisicion,
            this.layoutControlItemEstacion,
            this.layoutControlItemSubEstacion,
            this.emptySpaceItem4,
            this.lblComentario,
            this.layoutControlItem1});
            this.layoutControlGroupDatos.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupDatos.Name = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupDatos.Size = new System.Drawing.Size(1073, 121);
            this.layoutControlGroupDatos.Text = "layoutControlGroupDatos";
            this.layoutControlGroupDatos.TextVisible = false;
            // 
            // lblNoFactura
            // 
            this.lblNoFactura.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNoFactura.AppearanceItemCaption.Options.UseFont = true;
            this.lblNoFactura.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblNoFactura.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblNoFactura.Control = this.txtNoFactura;
            this.lblNoFactura.CustomizationFormText = "No. de Factura";
            this.lblNoFactura.Location = new System.Drawing.Point(10, 24);
            this.lblNoFactura.MinSize = new System.Drawing.Size(169, 24);
            this.lblNoFactura.Name = "lblNoFactura";
            this.lblNoFactura.Size = new System.Drawing.Size(251, 26);
            this.lblNoFactura.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblNoFactura.Text = "Número";
            this.lblNoFactura.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblNoFactura.TextSize = new System.Drawing.Size(110, 13);
            this.lblNoFactura.TextToControlDistance = 5;
            // 
            // lblFechaAdquisicion
            // 
            this.lblFechaAdquisicion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseFont = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblFechaAdquisicion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.lblFechaAdquisicion.Control = this.dateFechaDepre;
            this.lblFechaAdquisicion.CustomizationFormText = "Fecha Adquisición";
            this.lblFechaAdquisicion.Location = new System.Drawing.Point(10, 0);
            this.lblFechaAdquisicion.MaxSize = new System.Drawing.Size(352, 0);
            this.lblFechaAdquisicion.MinSize = new System.Drawing.Size(352, 24);
            this.lblFechaAdquisicion.Name = "lblFechaAdquisicion";
            this.lblFechaAdquisicion.Size = new System.Drawing.Size(352, 24);
            this.lblFechaAdquisicion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblFechaAdquisicion.Text = "Fecha Depreciación";
            this.lblFechaAdquisicion.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblFechaAdquisicion.TextSize = new System.Drawing.Size(110, 13);
            this.lblFechaAdquisicion.TextToControlDistance = 5;
            // 
            // layoutControlItemEstacion
            // 
            this.layoutControlItemEstacion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemEstacion.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemEstacion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemEstacion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutControlItemEstacion.Control = this.lkEs;
            this.layoutControlItemEstacion.CustomizationFormText = "Estación de Servicio";
            this.layoutControlItemEstacion.Location = new System.Drawing.Point(362, 0);
            this.layoutControlItemEstacion.MinSize = new System.Drawing.Size(169, 24);
            this.layoutControlItemEstacion.Name = "layoutControlItemEstacion";
            this.layoutControlItemEstacion.Size = new System.Drawing.Size(705, 24);
            this.layoutControlItemEstacion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemEstacion.Text = "Estación de Servicio";
            this.layoutControlItemEstacion.TextSize = new System.Drawing.Size(112, 13);
            // 
            // layoutControlItemSubEstacion
            // 
            this.layoutControlItemSubEstacion.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemSubEstacion.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemSubEstacion.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItemSubEstacion.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.layoutControlItemSubEstacion.Control = this.lkSus;
            this.layoutControlItemSubEstacion.CustomizationFormText = "Sub Estación";
            this.layoutControlItemSubEstacion.Location = new System.Drawing.Point(362, 24);
            this.layoutControlItemSubEstacion.MinSize = new System.Drawing.Size(169, 24);
            this.layoutControlItemSubEstacion.Name = "layoutControlItemSubEstacion";
            this.layoutControlItemSubEstacion.Size = new System.Drawing.Size(705, 26);
            this.layoutControlItemSubEstacion.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemSubEstacion.Text = "Sub Estación";
            this.layoutControlItemSubEstacion.TextSize = new System.Drawing.Size(112, 13);
            // 
            // emptySpaceItem4
            // 
            this.emptySpaceItem4.AllowHotTrack = false;
            this.emptySpaceItem4.CustomizationFormText = "emptySpaceItem4";
            this.emptySpaceItem4.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem4.MaxSize = new System.Drawing.Size(10, 0);
            this.emptySpaceItem4.MinSize = new System.Drawing.Size(10, 10);
            this.emptySpaceItem4.Name = "emptySpaceItem4";
            this.emptySpaceItem4.Size = new System.Drawing.Size(10, 115);
            this.emptySpaceItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem4.Text = "emptySpaceItem4";
            this.emptySpaceItem4.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lblComentario
            // 
            this.lblComentario.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblComentario.AppearanceItemCaption.Options.UseFont = true;
            this.lblComentario.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lblComentario.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lblComentario.Control = this.mmoComentario;
            this.lblComentario.CustomizationFormText = "Comentario";
            this.lblComentario.Location = new System.Drawing.Point(10, 50);
            this.lblComentario.MinSize = new System.Drawing.Size(94, 38);
            this.lblComentario.Name = "lblComentario";
            this.lblComentario.Size = new System.Drawing.Size(1057, 65);
            this.lblComentario.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.lblComentario.Text = "Comentario";
            this.lblComentario.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lblComentario.TextLocation = DevExpress.Utils.Locations.Top;
            this.lblComentario.TextSize = new System.Drawing.Size(90, 13);
            this.lblComentario.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnLoad;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(261, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(101, 26);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
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
            this.gridBien.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridBien.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridBien.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridBien.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridBien.Location = new System.Drawing.Point(2, 150);
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
            this.gridBien.Size = new System.Drawing.Size(1081, 249);
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
            this.gvBien.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombre,
            this.colNoSerie,
            this.colModelo,
            this.colES,
            this.colSubEstacion,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.gvBien.GridControl = this.gridBien;
            this.gvBien.GroupCount = 1;
            this.gvBien.Name = "gvBien";
            this.gvBien.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvBien.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvBien.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvBien.OptionsBehavior.Editable = false;
            this.gvBien.OptionsCustomization.AllowFilter = false;
            this.gvBien.OptionsCustomization.AllowGroup = false;
            this.gvBien.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvBien.OptionsCustomization.AllowSort = false;
            this.gvBien.OptionsFilter.AllowFilterEditor = false;
            this.gvBien.OptionsFilter.AllowMRUFilterList = false;
            this.gvBien.OptionsLayout.Columns.AddNewColumns = false;
            this.gvBien.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvBien.OptionsNavigation.AutoFocusNewRow = true;
            this.gvBien.OptionsSelection.UseIndicatorForSelection = false;
            this.gvBien.OptionsView.ColumnAutoWidth = false;
            this.gvBien.OptionsView.ShowAutoFilterRow = true;
            this.gvBien.OptionsView.ShowFooter = true;
            this.gvBien.OptionsView.ShowGroupPanel = false;
            this.gvBien.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colES, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvBien.GroupRowCollapsing += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvBien_GroupRowCollapsing);
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
            this.colCodigo.VisibleIndex = 2;
            this.colCodigo.Width = 84;
            // 
            // rpSpCodigo
            // 
            this.rpSpCodigo.AutoHeight = false;
            this.rpSpCodigo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
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
            this.colNombre.VisibleIndex = 3;
            this.colNombre.Width = 70;
            // 
            // colNoSerie
            // 
            this.colNoSerie.Caption = "No. Serie";
            this.colNoSerie.FieldName = "NoSerie";
            this.colNoSerie.Name = "colNoSerie";
            this.colNoSerie.Visible = true;
            this.colNoSerie.VisibleIndex = 4;
            this.colNoSerie.Width = 90;
            // 
            // colModelo
            // 
            this.colModelo.Caption = "Modelo";
            this.colModelo.FieldName = "Modelo";
            this.colModelo.Name = "colModelo";
            this.colModelo.Visible = true;
            this.colModelo.VisibleIndex = 5;
            this.colModelo.Width = 64;
            // 
            // colES
            // 
            this.colES.Caption = "Estación";
            this.colES.FieldName = "EstacionNombre";
            this.colES.Name = "colES";
            this.colES.Visible = true;
            this.colES.VisibleIndex = 5;
            // 
            // colSubEstacion
            // 
            this.colSubEstacion.Caption = "Sub Estación";
            this.colSubEstacion.FieldName = "SubEstacionNombre";
            this.colSubEstacion.Name = "colSubEstacion";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Tipo Activo";
            this.gridColumn1.FieldName = "TipoActivoNombre";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 69;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Nombre Activo";
            this.gridColumn2.FieldName = "ActivoNombre";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 82;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Fecha Adquisición";
            this.gridColumn3.DisplayFormat.FormatString = "d";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn3.FieldName = "FechaAdquisicion";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 6;
            this.gridColumn3.Width = 93;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gridColumn4.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn4.Caption = "Valor Adquisición";
            this.gridColumn4.DisplayFormat.FormatString = "N2";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.FieldName = "ValorAdquisicion";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ValorAdquisicion", "{0:N2}")});
            this.gridColumn4.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 7;
            this.gridColumn4.Width = 97;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gridColumn5.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn5.Caption = "Vida Util Meses";
            this.gridColumn5.DisplayFormat.FormatString = "N2";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.FieldName = "VidaUtilMeses";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "VidaUtilMeses", "{0:N2}")});
            this.gridColumn5.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 8;
            this.gridColumn5.Width = 86;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridColumn6.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn6.Caption = "Meses Dep. Acum";
            this.gridColumn6.DisplayFormat.FormatString = "N2";
            this.gridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn6.FieldName = "MesesDepreciados";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MesesDepreciados", "{0:N2}")});
            this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 10;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridColumn7.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn7.Caption = "Valor Deprec. Acum";
            this.gridColumn7.DisplayFormat.FormatString = "N2";
            this.gridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn7.FieldName = "ValorDepreActual";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ValorDepreActual", "{0:N2}")});
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 9;
            this.gridColumn7.Width = 98;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridColumn8.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn8.Caption = "Monto Depr.";
            this.gridColumn8.DisplayFormat.FormatString = "N2";
            this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn8.FieldName = "ValorDepreciacion";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ValorDepreciacion", "{0:N2}")});
            this.gridColumn8.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 11;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridColumn9.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn9.Caption = "Mes Depr.";
            this.gridColumn9.DisplayFormat.FormatString = "N2";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn9.FieldName = "NumeroDepreciacion";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "NumeroDepreciacion", "{0:N2}")});
            this.gridColumn9.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 12;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn10.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.Caption = "Valor en Libros";
            this.gridColumn10.DisplayFormat.FormatString = "n2";
            this.gridColumn10.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn10.FieldName = "TValor";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TValor", "{0:N2}")});
            this.gridColumn10.UnboundExpression = "[ValorAdquisicion]-[ValorDepreActual]-[ValorDepreciacion]";
            this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 13;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridColumn11.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn11.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn11.AppearanceCell.Options.UseFont = true;
            this.gridColumn11.Caption = "Meses Por Depreciar";
            this.gridColumn11.FieldName = "TMeses";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "TMeses", "{0:N2}")});
            this.gridColumn11.UnboundExpression = "Iif([VidaUtilMeses]-[NumeroDepreciacion] >= [VidaUtilMeses], 0, [VidaUtilMeses]-[" +
    "NumeroDepreciacion])";
            this.gridColumn11.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 14;
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
            // repositoryItemCheckEditDepreciacion
            // 
            this.repositoryItemCheckEditDepreciacion.AutoHeight = false;
            this.repositoryItemCheckEditDepreciacion.Caption = "Check";
            this.repositoryItemCheckEditDepreciacion.Name = "repositoryItemCheckEditDepreciacion";
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
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
            // DialogDepreciacion
            // 
            this.AcceptButton = this.btnLoad;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 448);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogDepreciacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movimiento de Bienes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCompras_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkSus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDepre.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDepre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoComentario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNoFactura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblNoFactura)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFechaAdquisicion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSubEstacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblComentario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvBien)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpCodigo)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditDepreciacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkAreaID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkTipoActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpValor)).EndInit();
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
        private DevExpress.XtraEditors.DateEdit dateFechaDepre;
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
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupDatos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        private DevExpress.XtraGrid.Columns.GridColumn colIDCC;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoCC;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreCC;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        public DevExpress.XtraEditors.SimpleButton bntNew;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView repositoryItemGridLookUpEdit1View;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditDepreciacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkAreaID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkTipoActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.LookUpEdit lkSus;
        private DevExpress.XtraEditors.LookUpEdit lkEs;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemEstacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpValor;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpCodigo;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem4;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Views.Grid.GridView gvBien;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colNoSerie;
        private DevExpress.XtraGrid.Columns.GridColumn colModelo;
        private DevExpress.XtraGrid.Columns.GridColumn colES;
        private DevExpress.XtraGrid.Columns.GridColumn colSubEstacion;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
    }
}