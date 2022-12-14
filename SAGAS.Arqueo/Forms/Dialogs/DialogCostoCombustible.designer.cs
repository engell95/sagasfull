namespace SAGAS.Arqueo.Forms.Dialogs
{
    partial class DialogCostoCombustible
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCostoCombustible));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.dateInicio = new DevExpress.XtraEditors.DateEdit();
            this.dateFinal = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroupFecha = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.bgvEs = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.bandEs = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colIDP = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colIDSUS = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colES = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.errDateField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFecha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvEs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errDateField)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.dateInicio);
            this.layoutControl1.Controls.Add(this.dateFinal);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroupFecha;
            this.layoutControl1.Size = new System.Drawing.Size(1004, 68);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // dateInicio
            // 
            this.dateInicio.EditValue = null;
            this.dateInicio.Location = new System.Drawing.Point(85, 32);
            this.dateInicio.Name = "dateInicio";
            this.dateInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateInicio.Size = new System.Drawing.Size(405, 20);
            this.dateInicio.StyleController = this.layoutControl1;
            this.dateInicio.TabIndex = 3;
            this.dateInicio.EditValueChanged += new System.EventHandler(this.dateInicio_EditValueChanged);
            this.dateInicio.Validated += new System.EventHandler(this.dateInicio_Validated);
            // 
            // dateFinal
            // 
            this.dateFinal.EditValue = null;
            this.dateFinal.Location = new System.Drawing.Point(567, 32);
            this.dateFinal.Name = "dateFinal";
            this.dateFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFinal.Size = new System.Drawing.Size(425, 20);
            this.dateFinal.StyleController = this.layoutControl1;
            this.dateFinal.TabIndex = 2;
            this.dateFinal.EditValueChanged += new System.EventHandler(this.dateInicio_EditValueChanged);
            this.dateFinal.Validated += new System.EventHandler(this.dateInicio_Validated);
            // 
            // layoutControlGroupFecha
            // 
            this.layoutControlGroupFecha.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroupFecha.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupFecha.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroupFecha.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroupFecha.Name = "layoutControlGroupFecha";
            this.layoutControlGroupFecha.Size = new System.Drawing.Size(1004, 68);
            this.layoutControlGroupFecha.Text = "Lista de Precio";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.dateFinal;
            this.layoutControlItem2.CustomizationFormText = "Fecha Final";
            this.layoutControlItem2.Location = new System.Drawing.Point(482, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(502, 28);
            this.layoutControlItem2.Text = "Fecha Final";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(70, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.dateInicio;
            this.layoutControlItem3.CustomizationFormText = "Fecha Inicial";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(482, 28);
            this.layoutControlItem3.Text = "Fecha Inicial";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(70, 13);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.grid);
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1008, 495);
            this.panelControl1.TabIndex = 0;
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(2, 70);
            this.grid.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.grid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grid.MainView = this.bgvEs;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(1004, 410);
            this.grid.TabIndex = 6;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvEs,
            this.gvData});
            // 
            // bgvEs
            // 
            this.bgvEs.Appearance.BandPanel.BackColor = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanel.Options.UseBackColor = true;
            this.bgvEs.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanelBackground.Options.UseBackColor = true;
            this.bgvEs.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.bgvEs.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.Transparent;
            this.bgvEs.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.bgvEs.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.bgvEs.Appearance.HeaderPanelBackground.ForeColor = System.Drawing.Color.Red;
            this.bgvEs.Appearance.HeaderPanelBackground.Options.UseForeColor = true;
            this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandEs});
            this.bgvEs.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colIDP,
            this.colIDSUS,
            this.colES});
            this.bgvEs.GridControl = this.grid;
            this.bgvEs.Name = "bgvEs";
            this.bgvEs.OptionsCustomization.AllowBandMoving = false;
            this.bgvEs.OptionsCustomization.AllowColumnMoving = false;
            this.bgvEs.OptionsCustomization.AllowFilter = false;
            this.bgvEs.OptionsCustomization.AllowSort = false;
            this.bgvEs.OptionsMenu.EnableColumnMenu = false;
            this.bgvEs.OptionsMenu.EnableFooterMenu = false;
            this.bgvEs.OptionsMenu.EnableGroupPanelMenu = false;
            this.bgvEs.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.bgvEs.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.bgvEs.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.bgvEs.OptionsSelection.MultiSelect = true;
            this.bgvEs.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.bgvEs.OptionsView.ColumnAutoWidth = false;
            this.bgvEs.OptionsView.ShowGroupPanel = false;
            this.bgvEs.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.bgvEs_PopupMenuShowing);
            this.bgvEs.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.bgvEs_CellValueChanged);
            // 
            // bandEs
            // 
            this.bandEs.Caption = "<>";
            this.bandEs.Columns.Add(this.colIDP);
            this.bandEs.Columns.Add(this.colIDSUS);
            this.bandEs.Columns.Add(this.colES);
            this.bandEs.MinWidth = 200;
            this.bandEs.Name = "bandEs";
            this.bandEs.OptionsBand.AllowMove = false;
            this.bandEs.OptionsBand.AllowPress = false;
            this.bandEs.OptionsBand.ShowCaption = false;
            this.bandEs.VisibleIndex = 0;
            this.bandEs.Width = 200;
            // 
            // colIDP
            // 
            this.colIDP.FieldName = "IDP";
            this.colIDP.Name = "colIDP";
            // 
            // colIDSUS
            // 
            this.colIDSUS.FieldName = "IDSUS";
            this.colIDSUS.Name = "colIDSUS";
            // 
            // colES
            // 
            this.colES.AppearanceHeader.BackColor = System.Drawing.Color.LightCyan;
            this.colES.AppearanceHeader.BackColor2 = System.Drawing.Color.LightCyan;
            this.colES.AppearanceHeader.BorderColor = System.Drawing.Color.Transparent;
            this.colES.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colES.AppearanceHeader.Options.UseBackColor = true;
            this.colES.AppearanceHeader.Options.UseForeColor = true;
            this.colES.Caption = "Estaci?n de Servicio";
            this.colES.FieldName = "ES";
            this.colES.MinWidth = 200;
            this.colES.Name = "colES";
            this.colES.OptionsColumn.AllowEdit = false;
            this.colES.Visible = true;
            this.colES.Width = 200;
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombreEstacion});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombreEstacion
            // 
            this.colNombreEstacion.Caption = "Estaci?n de Servicio";
            this.colNombreEstacion.FieldName = "Nombre";
            this.colNombreEstacion.Name = "colNombreEstacion";
            this.colNombreEstacion.Visible = true;
            this.colNombreEstacion.VisibleIndex = 0;
            this.colNombreEstacion.Width = 265;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Appearance.Options.UseFont = true;
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 480);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 495);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1008, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
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
            this.btnOK.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.ImageOptions.Image")));
            this.btnOK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
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
            // errDateField
            // 
            this.errDateField.ContainerControl = this;
            // 
            // DialogCostoCombustible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1008, 529);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCostoCombustible";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFecha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvEs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errDateField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupFecha;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DateEdit dateInicio;
        private DevExpress.XtraEditors.DateEdit dateFinal;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errDateField;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bgvEs;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDP;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colES;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreEstacion;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDSUS;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandEs;
    }
}