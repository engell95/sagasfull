namespace SAGAS.Arqueo.Forms.Dialogs
{
    partial class DialogActualizarLectura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogActualizarLectura));
            this.rpSpElectronica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridMangueras = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colIDManguera = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLado = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colManguera = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colColor = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpColor = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colLecturaMecanicaAnterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpSpinMecanica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colLecturaEfectivoAnterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rpSpEfectivo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colLecturaElectronicaAnterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colLecturaMecanicaPosterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLecturaEfectivoPosterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colLecturaElectronicaPosterior = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btrRemoveAcces = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMangueras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpinMecanica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpEfectivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btrRemoveAcces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rpSpElectronica
            // 
            this.rpSpElectronica.AllowMouseWheel = false;
            this.rpSpElectronica.AutoHeight = false;
            this.rpSpElectronica.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.rpSpElectronica.Mask.EditMask = "N3";
            this.rpSpElectronica.MaxValue = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.rpSpElectronica.Name = "rpSpElectronica";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridMangueras);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(876, 377);
            this.panelControl1.TabIndex = 0;
            // 
            // gridMangueras
            // 
            this.gridMangueras.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridMangueras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMangueras.Location = new System.Drawing.Point(2, 2);
            this.gridMangueras.MainView = this.bandedGridView1;
            this.gridMangueras.Name = "gridMangueras";
            this.gridMangueras.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2,
            this.btrRemoveAcces,
            this.rpSpinMecanica,
            this.rpSpEfectivo,
            this.rpSpElectronica,
            this.rpColor});
            this.gridMangueras.Size = new System.Drawing.Size(872, 373);
            this.gridMangueras.TabIndex = 36;
            this.gridMangueras.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridView1.Appearance.FooterPanel.Options.UseFont = true;
            this.bandedGridView1.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridView1.Appearance.GroupRow.Options.UseFont = true;
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colIDManguera,
            this.colLado,
            this.colManguera,
            this.colColor,
            this.colLecturaMecanicaAnterior,
            this.colLecturaEfectivoAnterior,
            this.colLecturaElectronicaAnterior,
            this.colLecturaMecanicaPosterior,
            this.colLecturaEfectivoPosterior,
            this.colLecturaElectronicaPosterior});
            this.bandedGridView1.GridControl = this.gridMangueras;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.bandedGridView1.OptionsCustomization.AllowColumnMoving = false;
            this.bandedGridView1.OptionsCustomization.AllowFilter = false;
            this.bandedGridView1.OptionsCustomization.AllowGroup = false;
            this.bandedGridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.bandedGridView1.OptionsCustomization.AllowSort = false;
            this.bandedGridView1.OptionsMenu.EnableColumnMenu = false;
            this.bandedGridView1.OptionsMenu.EnableFooterMenu = false;
            this.bandedGridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.bandedGridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.bandedGridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.bandedGridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.bandedGridView1.OptionsMenu.ShowSplitItem = false;
            this.bandedGridView1.OptionsPrint.ExpandAllDetails = true;
            this.bandedGridView1.OptionsPrint.PrintDetails = true;
            this.bandedGridView1.OptionsPrint.PrintFilterInfo = true;
            this.bandedGridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.bandedGridView1.OptionsSelection.MultiSelect = true;
            this.bandedGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office2003;
            this.bandedGridView1.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.bandedGridView1.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "Datos";
            this.gridBand1.Columns.Add(this.colIDManguera);
            this.gridBand1.Columns.Add(this.colLado);
            this.gridBand1.Columns.Add(this.colManguera);
            this.gridBand1.Columns.Add(this.colColor);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 157;
            // 
            // colIDManguera
            // 
            this.colIDManguera.FieldName = "IDManguera";
            this.colIDManguera.Name = "colIDManguera";
            this.colIDManguera.OptionsColumn.AllowEdit = false;
            // 
            // colLado
            // 
            this.colLado.Caption = "Lado";
            this.colLado.FieldName = "Lado";
            this.colLado.Name = "colLado";
            this.colLado.OptionsColumn.AllowEdit = false;
            this.colLado.Visible = true;
            this.colLado.Width = 39;
            // 
            // colManguera
            // 
            this.colManguera.Caption = "Manguera";
            this.colManguera.FieldName = "MangueraNombre";
            this.colManguera.Name = "colManguera";
            this.colManguera.OptionsColumn.AllowEdit = false;
            this.colManguera.Visible = true;
            this.colManguera.Width = 63;
            // 
            // colColor
            // 
            this.colColor.Caption = "Color";
            this.colColor.ColumnEdit = this.rpColor;
            this.colColor.FieldName = "Color";
            this.colColor.Name = "colColor";
            this.colColor.OptionsColumn.AllowEdit = false;
            this.colColor.Visible = true;
            this.colColor.Width = 55;
            // 
            // rpColor
            // 
            this.rpColor.AutoHeight = false;
            this.rpColor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpColor.Name = "rpColor";
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "Lecturas Anteriores";
            this.gridBand2.Columns.Add(this.colLecturaMecanicaAnterior);
            this.gridBand2.Columns.Add(this.colLecturaEfectivoAnterior);
            this.gridBand2.Columns.Add(this.colLecturaElectronicaAnterior);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 378;
            // 
            // colLecturaMecanicaAnterior
            // 
            this.colLecturaMecanicaAnterior.Caption = "Lectura Mecánica Ant.";
            this.colLecturaMecanicaAnterior.ColumnEdit = this.rpSpinMecanica;
            this.colLecturaMecanicaAnterior.DisplayFormat.FormatString = "N3";
            this.colLecturaMecanicaAnterior.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaMecanicaAnterior.FieldName = "LecturaMecanicaAnterior";
            this.colLecturaMecanicaAnterior.Name = "colLecturaMecanicaAnterior";
            this.colLecturaMecanicaAnterior.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colLecturaMecanicaAnterior.Visible = true;
            this.colLecturaMecanicaAnterior.Width = 132;
            // 
            // rpSpinMecanica
            // 
            this.rpSpinMecanica.AllowMouseWheel = false;
            this.rpSpinMecanica.AutoHeight = false;
            this.rpSpinMecanica.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.rpSpinMecanica.Mask.EditMask = "N3";
            this.rpSpinMecanica.MaxValue = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.rpSpinMecanica.Name = "rpSpinMecanica";
            // 
            // colLecturaEfectivoAnterior
            // 
            this.colLecturaEfectivoAnterior.Caption = "Lectura Efectivo Ant.";
            this.colLecturaEfectivoAnterior.ColumnEdit = this.rpSpEfectivo;
            this.colLecturaEfectivoAnterior.DisplayFormat.FormatString = "N3";
            this.colLecturaEfectivoAnterior.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaEfectivoAnterior.FieldName = "LecturaEfectivoAnterior";
            this.colLecturaEfectivoAnterior.Name = "colLecturaEfectivoAnterior";
            this.colLecturaEfectivoAnterior.Visible = true;
            this.colLecturaEfectivoAnterior.Width = 116;
            // 
            // rpSpEfectivo
            // 
            this.rpSpEfectivo.AllowMouseWheel = false;
            this.rpSpEfectivo.AutoHeight = false;
            this.rpSpEfectivo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.rpSpEfectivo.Mask.EditMask = "N3";
            this.rpSpEfectivo.MaxValue = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.rpSpEfectivo.Name = "rpSpEfectivo";
            // 
            // colLecturaElectronicaAnterior
            // 
            this.colLecturaElectronicaAnterior.Caption = "Lectura Electrónica Ant.";
            this.colLecturaElectronicaAnterior.ColumnEdit = this.rpSpElectronica;
            this.colLecturaElectronicaAnterior.DisplayFormat.FormatString = "N3";
            this.colLecturaElectronicaAnterior.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaElectronicaAnterior.FieldName = "LecturaElectronicaAnterior";
            this.colLecturaElectronicaAnterior.Name = "colLecturaElectronicaAnterior";
            this.colLecturaElectronicaAnterior.Visible = true;
            this.colLecturaElectronicaAnterior.Width = 130;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "Lecturas Nuevas";
            this.gridBand3.Columns.Add(this.colLecturaMecanicaPosterior);
            this.gridBand3.Columns.Add(this.colLecturaEfectivoPosterior);
            this.gridBand3.Columns.Add(this.colLecturaElectronicaPosterior);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 391;
            // 
            // colLecturaMecanicaPosterior
            // 
            this.colLecturaMecanicaPosterior.Caption = "Lectura Mecánica Post.";
            this.colLecturaMecanicaPosterior.FieldName = "LecturaMecanicaPosterior";
            this.colLecturaMecanicaPosterior.Name = "colLecturaMecanicaPosterior";
            this.colLecturaMecanicaPosterior.Visible = true;
            this.colLecturaMecanicaPosterior.Width = 127;
            // 
            // colLecturaEfectivoPosterior
            // 
            this.colLecturaEfectivoPosterior.Caption = "Lectura Efectivo Post.";
            this.colLecturaEfectivoPosterior.FieldName = "LecturaEfectivoPosterior";
            this.colLecturaEfectivoPosterior.Name = "colLecturaEfectivoPosterior";
            this.colLecturaEfectivoPosterior.Visible = true;
            this.colLecturaEfectivoPosterior.Width = 127;
            // 
            // colLecturaElectronicaPosterior
            // 
            this.colLecturaElectronicaPosterior.Caption = "Lectura Electrónica Post.";
            this.colLecturaElectronicaPosterior.FieldName = "LecturaElectronicaPosterior";
            this.colLecturaElectronicaPosterior.Name = "colLecturaElectronicaPosterior";
            this.colLecturaElectronicaPosterior.Visible = true;
            this.colLecturaElectronicaPosterior.Width = 137;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Caption = "Check";
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // btrRemoveAcces
            // 
            this.btrRemoveAcces.AutoHeight = false;
            this.btrRemoveAcces.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Minus)});
            this.btrRemoveAcces.Name = "btrRemoveAcces";
            this.btrRemoveAcces.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 377);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(876, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Arqueo.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(86, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.Image = global::SAGAS.Arqueo.Properties.Resources.Ok20;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Guardar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // DialogActualizarLectura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(876, 411);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogActualizarLectura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMangueras)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpinMecanica)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpEfectivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btrRemoveAcces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.GridControl gridMangueras;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btrRemoveAcces;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpinMecanica;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpEfectivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpElectronica;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit rpColor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDManguera;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colManguera;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colColor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaMecanicaAnterior;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaEfectivoAnterior;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaElectronicaAnterior;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaMecanicaPosterior;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaEfectivoPosterior;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colLecturaElectronicaPosterior;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
    }
}