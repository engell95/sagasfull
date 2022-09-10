namespace SAGAS.Arqueo.Forms.Dialogs
{
    partial class DialogCambiarLectura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCambiarLectura));
            this.colLecturaElectronica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpElectronica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridMangueras = new DevExpress.XtraGrid.GridControl();
            this.gvDataMangueras = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDManguera = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colManguera = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpColor = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.colLecturaMecanica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpinMecanica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colLecturaEfectivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpEfectivo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btrRemoveAcces = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMangueras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataMangueras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpinMecanica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpEfectivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btrRemoveAcces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // colLecturaElectronica
            // 
            this.colLecturaElectronica.Caption = "Lectura Electrónica";
            this.colLecturaElectronica.ColumnEdit = this.rpSpElectronica;
            this.colLecturaElectronica.DisplayFormat.FormatString = "N3";
            this.colLecturaElectronica.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaElectronica.FieldName = "LecturaElectronica";
            this.colLecturaElectronica.Name = "colLecturaElectronica";
            this.colLecturaElectronica.Visible = true;
            this.colLecturaElectronica.VisibleIndex = 5;
            this.colLecturaElectronica.Width = 106;
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
            this.panelControl1.Size = new System.Drawing.Size(542, 377);
            this.panelControl1.TabIndex = 0;
            // 
            // gridMangueras
            // 
            this.gridMangueras.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridMangueras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMangueras.Location = new System.Drawing.Point(2, 2);
            this.gridMangueras.MainView = this.gvDataMangueras;
            this.gridMangueras.Name = "gridMangueras";
            this.gridMangueras.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2,
            this.btrRemoveAcces,
            this.rpSpinMecanica,
            this.rpSpEfectivo,
            this.rpSpElectronica,
            this.rpColor});
            this.gridMangueras.Size = new System.Drawing.Size(538, 373);
            this.gridMangueras.TabIndex = 36;
            this.gridMangueras.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataMangueras});
            // 
            // gvDataMangueras
            // 
            this.gvDataMangueras.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataMangueras.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDataMangueras.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataMangueras.Appearance.GroupRow.Options.UseFont = true;
            this.gvDataMangueras.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDManguera,
            this.colLado,
            this.colManguera,
            this.colColor,
            this.colLecturaMecanica,
            this.colLecturaEfectivo,
            this.colLecturaElectronica});
            this.gvDataMangueras.GridControl = this.gridMangueras;
            this.gvDataMangueras.Name = "gvDataMangueras";
            this.gvDataMangueras.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDataMangueras.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataMangueras.OptionsCustomization.AllowFilter = false;
            this.gvDataMangueras.OptionsCustomization.AllowGroup = false;
            this.gvDataMangueras.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDataMangueras.OptionsCustomization.AllowSort = false;
            this.gvDataMangueras.OptionsMenu.EnableColumnMenu = false;
            this.gvDataMangueras.OptionsMenu.EnableFooterMenu = false;
            this.gvDataMangueras.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataMangueras.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDataMangueras.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataMangueras.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataMangueras.OptionsMenu.ShowSplitItem = false;
            this.gvDataMangueras.OptionsPrint.ExpandAllDetails = true;
            this.gvDataMangueras.OptionsPrint.PrintDetails = true;
            this.gvDataMangueras.OptionsPrint.PrintFilterInfo = true;
            this.gvDataMangueras.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvDataMangueras.OptionsSelection.MultiSelect = true;
            this.gvDataMangueras.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDataMangueras.OptionsView.ColumnAutoWidth = false;
            this.gvDataMangueras.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office2003;
            this.gvDataMangueras.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gvDataMangueras.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gvDataMangueras.OptionsView.ShowGroupedColumns = true;
            this.gvDataMangueras.OptionsView.ShowGroupPanel = false;
            this.gvDataMangueras.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gvDataMangueras_PopupMenuShowing);
            this.gvDataMangueras.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDataMangueras_CellValueChanged);
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
            this.colLado.VisibleIndex = 0;
            this.colLado.Width = 43;
            // 
            // colManguera
            // 
            this.colManguera.Caption = "Manguera";
            this.colManguera.FieldName = "MangueraNombre";
            this.colManguera.Name = "colManguera";
            this.colManguera.OptionsColumn.AllowEdit = false;
            this.colManguera.Visible = true;
            this.colManguera.VisibleIndex = 1;
            this.colManguera.Width = 77;
            // 
            // colColor
            // 
            this.colColor.Caption = "Color";
            this.colColor.ColumnEdit = this.rpColor;
            this.colColor.FieldName = "Color";
            this.colColor.Name = "colColor";
            this.colColor.OptionsColumn.AllowEdit = false;
            this.colColor.Visible = true;
            this.colColor.VisibleIndex = 2;
            // 
            // rpColor
            // 
            this.rpColor.AutoHeight = false;
            this.rpColor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpColor.Name = "rpColor";
            // 
            // colLecturaMecanica
            // 
            this.colLecturaMecanica.Caption = "Lectura Mecánica";
            this.colLecturaMecanica.ColumnEdit = this.rpSpinMecanica;
            this.colLecturaMecanica.DisplayFormat.FormatString = "N3";
            this.colLecturaMecanica.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaMecanica.FieldName = "LecturaMecanica";
            this.colLecturaMecanica.Name = "colLecturaMecanica";
            this.colLecturaMecanica.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colLecturaMecanica.Visible = true;
            this.colLecturaMecanica.VisibleIndex = 3;
            this.colLecturaMecanica.Width = 99;
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
            // colLecturaEfectivo
            // 
            this.colLecturaEfectivo.Caption = "Lectura Efectivo";
            this.colLecturaEfectivo.ColumnEdit = this.rpSpEfectivo;
            this.colLecturaEfectivo.DisplayFormat.FormatString = "N3";
            this.colLecturaEfectivo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLecturaEfectivo.FieldName = "LecturaEfectivo";
            this.colLecturaEfectivo.Name = "colLecturaEfectivo";
            this.colLecturaEfectivo.Visible = true;
            this.colLecturaEfectivo.VisibleIndex = 4;
            this.colLecturaEfectivo.Width = 93;
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
            this.panelControl2.Size = new System.Drawing.Size(542, 34);
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
            // DialogCambiarLectura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(542, 411);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCambiarLectura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMangueras)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataMangueras)).EndInit();
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
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataMangueras;
        private DevExpress.XtraGrid.Columns.GridColumn colIDManguera;
        private DevExpress.XtraGrid.Columns.GridColumn colManguera;
        private DevExpress.XtraGrid.Columns.GridColumn colLecturaMecanica;
        private DevExpress.XtraGrid.Columns.GridColumn colLecturaEfectivo;
        private DevExpress.XtraGrid.Columns.GridColumn colLecturaElectronica;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btrRemoveAcces;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpinMecanica;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpEfectivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpElectronica;
        private DevExpress.XtraGrid.Columns.GridColumn colColor;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit rpColor;
        private DevExpress.XtraGrid.Columns.GridColumn colLado;
    }
}