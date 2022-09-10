namespace SAGAS.Administracion.Forms
{
    partial class FormAudit
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
            DevExpress.XtraBars.Bar TopBar;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAudit));
            this.btnLoad = new DevExpress.XtraBars.BarButtonItem();
            this.barImprimir = new DevExpress.XtraBars.BarButtonItem();
            this.barExportar = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.DataGridTabla = new DevExpress.XtraGrid.GridControl();
            this.GridViewTabla = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColEstacionServicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColTipoAccion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAccion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComputadora = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCampo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValorAntesCampo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValorDespuesCampo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.riCheckLocked = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rpUsuario = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.linqInstantFeedbackSource1 = new DevExpress.Data.Linq.LinqInstantFeedbackSource();
            this.dglExportToFile = new System.Windows.Forms.SaveFileDialog();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.dateFinal = new DevExpress.XtraEditors.DateEdit();
            this.dateInicial = new DevExpress.XtraEditors.DateEdit();
            this.lkTipo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            TopBar = new DevExpress.XtraBars.Bar();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridTabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTabla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.riCheckLocked)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpUsuario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // TopBar
            // 
            TopBar.BarName = "Main menu";
            TopBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            TopBar.DockCol = 0;
            TopBar.DockRow = 0;
            TopBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            TopBar.FloatLocation = new System.Drawing.Point(92, 155);
            TopBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLoad, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barImprimir),
            new DevExpress.XtraBars.LinkPersistInfo(this.barExportar)});
            TopBar.OptionsBar.AllowQuickCustomization = false;
            TopBar.OptionsBar.DrawDragBorder = false;
            TopBar.OptionsBar.MultiLine = true;
            TopBar.OptionsBar.UseWholeRow = true;
            TopBar.Text = "Main menu";
            // 
            // btnLoad
            // 
            this.btnLoad.Caption = "Cargar";
            this.btnLoad.Glyph = global::SAGAS.Administracion.Properties.Resources.application_put;
            this.btnLoad.Id = 11;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnLoad.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoad_ItemClick);
            // 
            // barImprimir
            // 
            this.barImprimir.Caption = "Imprimir";
            this.barImprimir.Glyph = global::SAGAS.Administracion.Properties.Resources.print24;
            this.barImprimir.Id = 9;
            this.barImprimir.Name = "barImprimir";
            this.barImprimir.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barImprimir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPrint_ItemClick);
            // 
            // barExportar
            // 
            this.barExportar.Caption = "Excel";
            this.barExportar.Glyph = global::SAGAS.Administracion.Properties.Resources.Excel24;
            this.barExportar.Id = 10;
            this.barExportar.Name = "barExportar";
            this.barExportar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barExportar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExcel_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            TopBar});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barImprimir,
            this.barExportar,
            this.btnLoad});
            this.barManager1.MainMenu = TopBar;
            this.barManager1.MaxItemId = 12;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(984, 32);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 516);
            this.barDockControlBottom.Size = new System.Drawing.Size(984, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 32);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 484);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(984, 32);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 484);
            // 
            // DataGridTabla
            // 
            this.DataGridTabla.Cursor = System.Windows.Forms.Cursors.Default;
            this.DataGridTabla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.DataGridTabla.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.DataGridTabla.EmbeddedNavigator.TextStringFormat = " {0:N0} de {1:N0}";
            this.DataGridTabla.Location = new System.Drawing.Point(0, 0);
            this.DataGridTabla.LookAndFeel.SkinName = "McSkin";
            this.DataGridTabla.MainView = this.GridViewTabla;
            this.DataGridTabla.MenuManager = this.barManager1;
            this.DataGridTabla.Name = "DataGridTabla";
            this.DataGridTabla.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.riCheckLocked,
            this.rpUsuario,
            this.rpEstacionServicio});
            this.DataGridTabla.Size = new System.Drawing.Size(805, 484);
            this.DataGridTabla.TabIndex = 4;
            this.DataGridTabla.UseEmbeddedNavigator = true;
            this.DataGridTabla.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridViewTabla});
            // 
            // GridViewTabla
            // 
            this.GridViewTabla.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColID,
            this.ColUser,
            this.ColEstacionServicio,
            this.ColTipoAccion,
            this.ColAccion,
            this.ColDate,
            this.colComputadora,
            this.colCampo,
            this.colValorAntesCampo,
            this.colValorDespuesCampo});
            this.GridViewTabla.GridControl = this.DataGridTabla;
            this.GridViewTabla.Name = "GridViewTabla";
            this.GridViewTabla.OptionsBehavior.AutoPopulateColumns = false;
            this.GridViewTabla.OptionsBehavior.Editable = false;
            this.GridViewTabla.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GridViewTabla.OptionsView.ColumnAutoWidth = false;
            this.GridViewTabla.OptionsView.ShowAutoFilterRow = true;
            // 
            // ColID
            // 
            this.ColID.FieldName = "ID";
            this.ColID.Name = "ColID";
            // 
            // ColUser
            // 
            this.ColUser.Caption = "Usuario";
            this.ColUser.FieldName = "Usuario";
            this.ColUser.Name = "ColUser";
            this.ColUser.Visible = true;
            this.ColUser.VisibleIndex = 0;
            this.ColUser.Width = 93;
            // 
            // ColEstacionServicio
            // 
            this.ColEstacionServicio.Caption = "Estación de Servicio";
            this.ColEstacionServicio.FieldName = "EstacionServicio";
            this.ColEstacionServicio.Name = "ColEstacionServicio";
            this.ColEstacionServicio.Visible = true;
            this.ColEstacionServicio.VisibleIndex = 1;
            this.ColEstacionServicio.Width = 124;
            // 
            // ColTipoAccion
            // 
            this.ColTipoAccion.Caption = "Tipo de Acción";
            this.ColTipoAccion.FieldName = "TipoAccion";
            this.ColTipoAccion.Name = "ColTipoAccion";
            this.ColTipoAccion.Visible = true;
            this.ColTipoAccion.VisibleIndex = 2;
            this.ColTipoAccion.Width = 101;
            // 
            // ColAccion
            // 
            this.ColAccion.Caption = "Acción";
            this.ColAccion.FieldName = "Accion";
            this.ColAccion.Name = "ColAccion";
            this.ColAccion.Visible = true;
            this.ColAccion.VisibleIndex = 3;
            this.ColAccion.Width = 218;
            // 
            // ColDate
            // 
            this.ColDate.Caption = "Fecha";
            this.ColDate.DisplayFormat.FormatString = "dd/MM/yyyy   hh:mm tt";
            this.ColDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.ColDate.FieldName = "Date";
            this.ColDate.Name = "ColDate";
            this.ColDate.Visible = true;
            this.ColDate.VisibleIndex = 4;
            this.ColDate.Width = 132;
            // 
            // colComputadora
            // 
            this.colComputadora.Caption = "Computadora";
            this.colComputadora.FieldName = "Computadora";
            this.colComputadora.Name = "colComputadora";
            this.colComputadora.Visible = true;
            this.colComputadora.VisibleIndex = 5;
            this.colComputadora.Width = 111;
            // 
            // colCampo
            // 
            this.colCampo.Caption = "Campo Modificado";
            this.colCampo.FieldName = "Campo";
            this.colCampo.Name = "colCampo";
            this.colCampo.Visible = true;
            this.colCampo.VisibleIndex = 6;
            this.colCampo.Width = 120;
            // 
            // colValorAntesCampo
            // 
            this.colValorAntesCampo.Caption = "Valor Anterior del Campo";
            this.colValorAntesCampo.FieldName = "ValorAntesCampo";
            this.colValorAntesCampo.Name = "colValorAntesCampo";
            this.colValorAntesCampo.Visible = true;
            this.colValorAntesCampo.VisibleIndex = 7;
            this.colValorAntesCampo.Width = 131;
            // 
            // colValorDespuesCampo
            // 
            this.colValorDespuesCampo.Caption = "Valor Posterior del Campo";
            this.colValorDespuesCampo.FieldName = "ValorDespuesCampo";
            this.colValorDespuesCampo.Name = "colValorDespuesCampo";
            this.colValorDespuesCampo.Visible = true;
            this.colValorDespuesCampo.VisibleIndex = 8;
            this.colValorDespuesCampo.Width = 135;
            // 
            // riCheckLocked
            // 
            this.riCheckLocked.AutoHeight = false;
            this.riCheckLocked.Caption = "Check";
            this.riCheckLocked.Name = "riCheckLocked";
            // 
            // rpUsuario
            // 
            this.rpUsuario.AutoHeight = false;
            this.rpUsuario.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpUsuario.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Usuarios")});
            this.rpUsuario.Name = "rpUsuario";
            // 
            // rpEstacionServicio
            // 
            this.rpEstacionServicio.AutoHeight = false;
            this.rpEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicios")});
            this.rpEstacionServicio.Name = "rpEstacionServicio";
            // 
            // linqInstantFeedbackSource1
            // 
            this.linqInstantFeedbackSource1.AreSourceRowsThreadSafe = true;
            this.linqInstantFeedbackSource1.GetQueryable += new System.EventHandler<DevExpress.Data.Linq.GetQueryableEventArgs>(this.linqInstantFeedbackSource1_GetQueryable);
            this.linqInstantFeedbackSource1.DismissQueryable += new System.EventHandler<DevExpress.Data.Linq.GetQueryableEventArgs>(this.linqInstantFeedbackSource1_DismissQueryable);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 32);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.DataGridTabla);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(984, 484);
            this.splitContainerControl1.SplitterPosition = 174;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.dateFinal);
            this.layoutControl1.Controls.Add(this.dateInicial);
            this.layoutControl1.Controls.Add(this.lkTipo);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(174, 484);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // dateFinal
            // 
            this.dateFinal.EditValue = null;
            this.dateFinal.Location = new System.Drawing.Point(64, 50);
            this.dateFinal.MenuManager = this.barManager1;
            this.dateFinal.Name = "dateFinal";
            this.dateFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFinal.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFinal.Size = new System.Drawing.Size(108, 20);
            this.dateFinal.StyleController = this.layoutControl1;
            this.dateFinal.TabIndex = 10;
            // 
            // dateInicial
            // 
            this.dateInicial.EditValue = null;
            this.dateInicial.Location = new System.Drawing.Point(64, 26);
            this.dateInicial.MenuManager = this.barManager1;
            this.dateInicial.Name = "dateInicial";
            this.dateInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateInicial.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateInicial.Size = new System.Drawing.Size(108, 20);
            this.dateInicial.StyleController = this.layoutControl1;
            this.dateInicial.TabIndex = 0;
            // 
            // lkTipo
            // 
            this.lkTipo.Location = new System.Drawing.Point(64, 2);
            this.lkTipo.MenuManager = this.barManager1;
            this.lkTipo.Name = "lkTipo";
            this.lkTipo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTipo.Properties.NullText = "<Tipo ? >";
            this.lkTipo.Properties.PopupSizeable = true;
            this.lkTipo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.lkTipo.Size = new System.Drawing.Size(108, 20);
            this.lkTipo.StyleController = this.layoutControl1;
            this.lkTipo.TabIndex = 11;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Fecha Inical";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(174, 484);
            this.layoutControlGroup1.Text = "Fecha Inical";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.dateInicial;
            this.layoutControlItem1.CustomizationFormText = "Fecha Inicial";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(174, 24);
            this.layoutControlItem1.Text = "Fecha Inicial";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(59, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.dateFinal;
            this.layoutControlItem2.CustomizationFormText = "Fecha Final";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(174, 436);
            this.layoutControlItem2.Text = "Fecha Final";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(59, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lkTipo;
            this.layoutControlItem3.CustomizationFormText = "Tipo";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(174, 24);
            this.layoutControlItem3.Text = "Tipo";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(59, 13);
            // 
            // FormAudit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 516);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAudit";
            this.Text = "Auditoría del Sistema";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridTabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridViewTabla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.riCheckLocked)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpUsuario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraGrid.GridControl DataGridTabla;
        private DevExpress.XtraGrid.Views.Grid.GridView GridViewTabla;
        private DevExpress.XtraGrid.Columns.GridColumn ColID;
        private DevExpress.XtraGrid.Columns.GridColumn ColUser;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit riCheckLocked;
        private DevExpress.XtraBars.BarButtonItem barImprimir;
        private DevExpress.XtraBars.BarButtonItem barExportar;
        private System.Windows.Forms.SaveFileDialog dglExportToFile;
        private DevExpress.XtraGrid.Columns.GridColumn ColEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn ColTipoAccion;
        private DevExpress.XtraGrid.Columns.GridColumn ColAccion;
        private DevExpress.XtraGrid.Columns.GridColumn ColDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpUsuario;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colComputadora;
        private DevExpress.XtraGrid.Columns.GridColumn colCampo;
        private DevExpress.XtraGrid.Columns.GridColumn colValorAntesCampo;
        private DevExpress.XtraGrid.Columns.GridColumn colValorDespuesCampo;
        private DevExpress.Data.Linq.LinqInstantFeedbackSource linqInstantFeedbackSource1;
        private DevExpress.XtraBars.BarButtonItem btnLoad;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.DateEdit dateFinal;
        private DevExpress.XtraEditors.DateEdit dateInicial;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.ComboBoxEdit lkTipo;
    }
}