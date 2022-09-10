namespace SAGAS.Arqueo.Forms.Dialogs
{
    partial class DialogInventarioResumen
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraGrid.StyleFormatCondition();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogInventarioResumen));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridEquivalentes = new DevExpress.XtraGrid.GridControl();
            this.bdsLista = new System.Windows.Forms.BindingSource();
            this.gvDataEquivalente = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLitros = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpSpLitros = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colIDT = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkTanque = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colIDP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkProducto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpMemoC = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.rpSpGalones = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEquivalentes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLista)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataEquivalente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpLitros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTanque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMemoC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpGalones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridEquivalentes);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(441, 183);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridEquivalentes
            // 
            this.gridEquivalentes.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridEquivalentes.DataSource = this.bdsLista;
            this.gridEquivalentes.Location = new System.Drawing.Point(12, 12);
            this.gridEquivalentes.MainView = this.gvDataEquivalente;
            this.gridEquivalentes.Name = "gridEquivalentes";
            this.gridEquivalentes.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpMemoC,
            this.rpSpLitros,
            this.rpSpGalones,
            this.lkTanque,
            this.lkProducto});
            this.gridEquivalentes.Size = new System.Drawing.Size(417, 159);
            this.gridEquivalentes.TabIndex = 8;
            this.gridEquivalentes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataEquivalente});
            // 
            // gvDataEquivalente
            // 
            this.gvDataEquivalente.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataEquivalente.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDataEquivalente.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colLitros,
            this.colIDT,
            this.colIDP});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.Blue;
            styleFormatCondition1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.White;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.Appearance.Options.UseFont = true;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = this.colIDP;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = 1;
            styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.Red;
            styleFormatCondition2.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            styleFormatCondition2.Appearance.ForeColor = System.Drawing.Color.White;
            styleFormatCondition2.Appearance.Options.UseBackColor = true;
            styleFormatCondition2.Appearance.Options.UseFont = true;
            styleFormatCondition2.Appearance.Options.UseForeColor = true;
            styleFormatCondition2.ApplyToRow = true;
            styleFormatCondition2.Column = this.colIDP;
            styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition2.Value1 = 2;
            styleFormatCondition3.Appearance.BackColor = System.Drawing.Color.Green;
            styleFormatCondition3.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            styleFormatCondition3.Appearance.ForeColor = System.Drawing.Color.White;
            styleFormatCondition3.Appearance.Options.UseBackColor = true;
            styleFormatCondition3.Appearance.Options.UseFont = true;
            styleFormatCondition3.Appearance.Options.UseForeColor = true;
            styleFormatCondition3.ApplyToRow = true;
            styleFormatCondition3.Column = this.colIDP;
            styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition3.Value1 = 3;
            styleFormatCondition4.Appearance.BackColor = System.Drawing.Color.Silver;
            styleFormatCondition4.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            styleFormatCondition4.Appearance.Options.UseBackColor = true;
            styleFormatCondition4.Appearance.Options.UseFont = true;
            styleFormatCondition4.ApplyToRow = true;
            styleFormatCondition4.Column = this.colIDP;
            styleFormatCondition4.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition4.Value1 = 4;
            this.gvDataEquivalente.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2,
            styleFormatCondition3,
            styleFormatCondition4});
            this.gvDataEquivalente.GridControl = this.gridEquivalentes;
            this.gvDataEquivalente.Name = "gvDataEquivalente";
            this.gvDataEquivalente.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataEquivalente.OptionsCustomization.AllowFilter = false;
            this.gvDataEquivalente.OptionsCustomization.AllowGroup = false;
            this.gvDataEquivalente.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDataEquivalente.OptionsCustomization.AllowSort = false;
            this.gvDataEquivalente.OptionsMenu.EnableColumnMenu = false;
            this.gvDataEquivalente.OptionsMenu.EnableFooterMenu = false;
            this.gvDataEquivalente.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataEquivalente.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDataEquivalente.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataEquivalente.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvDataEquivalente.OptionsView.ShowDetailButtons = false;
            this.gvDataEquivalente.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colLitros
            // 
            this.colLitros.AppearanceCell.Options.UseTextOptions = true;
            this.colLitros.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colLitros.AppearanceHeader.Options.UseTextOptions = true;
            this.colLitros.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colLitros.Caption = "Litros";
            this.colLitros.ColumnEdit = this.rpSpLitros;
            this.colLitros.DisplayFormat.FormatString = "N3";
            this.colLitros.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLitros.FieldName = "Litros";
            this.colLitros.Name = "colLitros";
            this.colLitros.Visible = true;
            this.colLitros.VisibleIndex = 2;
            this.colLitros.Width = 145;
            // 
            // rpSpLitros
            // 
            this.rpSpLitros.AutoHeight = false;
            this.rpSpLitros.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.rpSpLitros.DisplayFormat.FormatString = "N3";
            this.rpSpLitros.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpLitros.EditFormat.FormatString = "N3";
            this.rpSpLitros.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpLitros.Mask.EditMask = "N3";
            this.rpSpLitros.Name = "rpSpLitros";
            // 
            // colIDT
            // 
            this.colIDT.Caption = "Tanque";
            this.colIDT.ColumnEdit = this.lkTanque;
            this.colIDT.FieldName = "TanqueID";
            this.colIDT.Name = "colIDT";
            this.colIDT.OptionsColumn.AllowEdit = false;
            this.colIDT.OptionsColumn.AllowFocus = false;
            this.colIDT.Visible = true;
            this.colIDT.VisibleIndex = 0;
            this.colIDT.Width = 117;
            // 
            // lkTanque
            // 
            this.lkTanque.AutoHeight = false;
            this.lkTanque.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTanque.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tanque")});
            this.lkTanque.DisplayMember = "Nombre";
            this.lkTanque.Name = "lkTanque";
            this.lkTanque.NullText = "<N/A>";
            this.lkTanque.ValueMember = "ID";
            // 
            // colIDP
            // 
            this.colIDP.Caption = "Producto";
            this.colIDP.ColumnEdit = this.lkProducto;
            this.colIDP.FieldName = "ProductoID";
            this.colIDP.Name = "colIDP";
            this.colIDP.OptionsColumn.AllowEdit = false;
            this.colIDP.OptionsColumn.AllowFocus = false;
            this.colIDP.Visible = true;
            this.colIDP.VisibleIndex = 1;
            this.colIDP.Width = 137;
            // 
            // lkProducto
            // 
            this.lkProducto.AutoHeight = false;
            this.lkProducto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkProducto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Producto")});
            this.lkProducto.DisplayMember = "Nombre";
            this.lkProducto.Name = "lkProducto";
            this.lkProducto.NullText = "<N/A>";
            this.lkProducto.ValueMember = "ID";
            // 
            // rpMemoC
            // 
            this.rpMemoC.AcceptsReturn = false;
            this.rpMemoC.AcceptsTab = false;
            this.rpMemoC.AutoHeight = false;
            this.rpMemoC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpMemoC.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Enter);
            this.rpMemoC.DisplayFormat.FormatString = "N2";
            this.rpMemoC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpMemoC.Name = "rpMemoC";
            this.rpMemoC.ShowIcon = false;
            this.rpMemoC.Tag = "C";
            // 
            // rpSpGalones
            // 
            this.rpSpGalones.AutoHeight = false;
            this.rpSpGalones.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.rpSpGalones.DisplayFormat.FormatString = "N3";
            this.rpSpGalones.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpGalones.EditFormat.FormatString = "N3";
            this.rpSpGalones.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.rpSpGalones.Mask.EditMask = "N3";
            this.rpSpGalones.Name = "rpSpGalones";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(441, 183);
            this.layoutControlGroup1.Text = "Datos del Color";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridEquivalentes;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(421, 163);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(445, 200);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 185);
            this.lblRequerido.Name = "lblRequerido";
            this.lblRequerido.Size = new System.Drawing.Size(255, 13);
            this.lblRequerido.TabIndex = 1;
            this.lblRequerido.Text = "Nota:  Los campos en negritas son requeridos";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 200);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(445, 34);
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
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // DialogInventarioResumen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(445, 234);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogInventarioResumen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEquivalentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsLista)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataEquivalente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpLitros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTanque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpMemoC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpSpGalones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraGrid.GridControl gridEquivalentes;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataEquivalente;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colLitros;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpLitros;
        private DevExpress.XtraGrid.Columns.GridColumn colIDT;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTanque;
        private DevExpress.XtraGrid.Columns.GridColumn colIDP;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkProducto;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpMemoC;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpSpGalones;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.BindingSource bdsLista;
    }
}