namespace SAGAS.Contabilidad.Forms.Dialogs
{
    partial class DialogShowComprobante
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogShowComprobante));
            this.rpSpElectronica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridComprobante = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource();
            this.bgvComprobante = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCodigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDescripcion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDebito = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCredito = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colNombre = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btrRemoveAcces = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.rpSpinMecanica = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.rpSpEfectivo = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.rpColor = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lblDiferencia = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvComprobante)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btrRemoveAcces)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpinMecanica)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpEfectivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
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
            100000000,
            0,
            0,
            0});
            this.rpSpElectronica.Name = "rpSpElectronica";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridComprobante);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(685, 306);
            this.panelControl1.TabIndex = 0;
            // 
            // gridComprobante
            // 
            this.gridComprobante.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridComprobante.DataSource = this.bdsDetalle;
            this.gridComprobante.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridComprobante.Location = new System.Drawing.Point(2, 2);
            this.gridComprobante.MainView = this.bgvComprobante;
            this.gridComprobante.Name = "gridComprobante";
            this.gridComprobante.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit2,
            this.btrRemoveAcces,
            this.rpSpinMecanica,
            this.rpSpEfectivo,
            this.rpSpElectronica,
            this.rpColor,
            this.repositoryItemMemoEdit1});
            this.gridComprobante.Size = new System.Drawing.Size(681, 302);
            this.gridComprobante.TabIndex = 36;
            this.gridComprobante.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvComprobante});
            // 
            // bgvComprobante
            // 
            this.bgvComprobante.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bgvComprobante.Appearance.FooterPanel.Options.UseFont = true;
            this.bgvComprobante.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bgvComprobante.Appearance.GroupRow.Options.UseFont = true;
            this.bgvComprobante.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2});
            this.bgvComprobante.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombre,
            this.colDebito,
            this.colCredito,
            this.colDescripcion});
            this.bgvComprobante.GridControl = this.gridComprobante;
            this.bgvComprobante.Name = "bgvComprobante";
            this.bgvComprobante.OptionsBehavior.AutoExpandAllGroups = true;
            this.bgvComprobante.OptionsBehavior.Editable = false;
            this.bgvComprobante.OptionsCustomization.AllowColumnMoving = false;
            this.bgvComprobante.OptionsCustomization.AllowFilter = false;
            this.bgvComprobante.OptionsCustomization.AllowGroup = false;
            this.bgvComprobante.OptionsCustomization.AllowQuickHideColumns = false;
            this.bgvComprobante.OptionsCustomization.AllowSort = false;
            this.bgvComprobante.OptionsMenu.EnableColumnMenu = false;
            this.bgvComprobante.OptionsMenu.EnableFooterMenu = false;
            this.bgvComprobante.OptionsMenu.EnableGroupPanelMenu = false;
            this.bgvComprobante.OptionsMenu.ShowAutoFilterRowItem = false;
            this.bgvComprobante.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.bgvComprobante.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.bgvComprobante.OptionsMenu.ShowSplitItem = false;
            this.bgvComprobante.OptionsPrint.ExpandAllDetails = true;
            this.bgvComprobante.OptionsPrint.PrintDetails = true;
            this.bgvComprobante.OptionsPrint.PrintFilterInfo = true;
            this.bgvComprobante.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.bgvComprobante.OptionsSelection.MultiSelect = true;
            this.bgvComprobante.OptionsView.ColumnAutoWidth = true;
            this.bgvComprobante.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office2003;
            this.bgvComprobante.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.bgvComprobante.OptionsView.ShowBands = false;
            this.bgvComprobante.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.bgvComprobante.OptionsView.ShowFooter = true;
            this.bgvComprobante.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = "gridBand2";
            this.gridBand2.Columns.Add(this.colCodigo);
            this.gridBand2.Columns.Add(this.colDescripcion);
            this.gridBand2.Columns.Add(this.colDebito);
            this.gridBand2.Columns.Add(this.colCredito);
            this.gridBand2.Columns.Add(this.colNombre);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.OptionsBand.ShowCaption = false;
            this.gridBand2.VisibleIndex = 0;
            this.gridBand2.Width = 598;
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Código Cuenta";
            this.colCodigo.FieldName = "CodigoCuenta";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.Width = 101;
            // 
            // colDescripcion
            // 
            this.colDescripcion.Caption = "Descripción";
            this.colDescripcion.FieldName = "Descripcion";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.Visible = true;
            this.colDescripcion.Width = 285;
            // 
            // colDebito
            // 
            this.colDebito.Caption = "Debito";
            this.colDebito.DisplayFormat.FormatString = "n2";
            this.colDebito.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colDebito.FieldName = "Debito";
            this.colDebito.Name = "colDebito";
            this.colDebito.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Debito", "{0:#,0.00;(#,0.00)}")});
            this.colDebito.Visible = true;
            this.colDebito.Width = 110;
            // 
            // colCredito
            // 
            this.colCredito.Caption = "Credito";
            this.colCredito.DisplayFormat.FormatString = "n2";
            this.colCredito.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCredito.FieldName = "Credito";
            this.colCredito.Name = "colCredito";
            this.colCredito.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Credito", "{0:#,0.00;(#,0.00)}")});
            this.colCredito.Visible = true;
            this.colCredito.Width = 102;
            // 
            // colNombre
            // 
            this.colNombre.AppearanceCell.BackColor = System.Drawing.Color.Cornsilk;
            this.colNombre.AppearanceCell.Options.UseBackColor = true;
            this.colNombre.Caption = "Nombre Cuenta";
            this.colNombre.FieldName = "NombreCuenta";
            this.colNombre.Name = "colNombre";
            this.colNombre.OptionsColumn.ShowCaption = false;
            this.colNombre.RowIndex = 1;
            this.colNombre.Visible = true;
            this.colNombre.Width = 598;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
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
            // rpSpinMecanica
            // 
            this.rpSpinMecanica.AllowMouseWheel = false;
            this.rpSpinMecanica.AutoHeight = false;
            this.rpSpinMecanica.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.rpSpinMecanica.Mask.EditMask = "N3";
            this.rpSpinMecanica.MaxValue = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.rpSpinMecanica.Name = "rpSpinMecanica";
            // 
            // rpSpEfectivo
            // 
            this.rpSpEfectivo.AllowMouseWheel = false;
            this.rpSpEfectivo.AutoHeight = false;
            this.rpSpEfectivo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
            this.rpSpEfectivo.Mask.EditMask = "N3";
            this.rpSpEfectivo.MaxValue = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.rpSpEfectivo.Name = "rpSpEfectivo";
            // 
            // rpColor
            // 
            this.rpColor.AutoHeight = false;
            this.rpColor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpColor.Name = "rpColor";
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lblDiferencia);
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 306);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(685, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // lblDiferencia
            // 
            this.lblDiferencia.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblDiferencia.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDiferencia.Location = new System.Drawing.Point(651, 2);
            this.lblDiferencia.Name = "lblDiferencia";
            this.lblDiferencia.Size = new System.Drawing.Size(32, 24);
            this.lblDiferencia.TabIndex = 2;
            this.lblDiferencia.Text = "<>";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Contabilidad.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(2, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "CERRAR";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DialogShowComprobante
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(685, 340);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "DialogShowComprobante";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rpSpElectronica)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvComprobante)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btrRemoveAcces)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpinMecanica)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpEfectivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit btrRemoveAcces;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpinMecanica;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpEfectivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpElectronica;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit rpColor;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView bgvComprobante;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDescripcion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDebito;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCredito;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNombre;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colID;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.LabelControl lblDiferencia;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.GridControl gridComprobante;
    }
}