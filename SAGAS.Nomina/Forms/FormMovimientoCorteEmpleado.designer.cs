namespace SAGAS.Nomina.Forms
{
    partial class FormMovimientoCorteEmpleado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMovimientoCorteEmpleado));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlanilla = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaFinal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAprobado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkAprovado = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkEsIngreso = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.chkAplicaRetencion = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rchkEsIngreso = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.splitContainerControlMain = new DevExpress.XtraEditors.SplitContainerControl();
            this.xtraTabControlMain = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAprovado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEsIngreso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaRetencion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkEsIngreso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).BeginInit();
            this.splitContainerControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).BeginInit();
            this.SuspendLayout();
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Excel";
            this.barButtonItem1.Id = 6;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "PDF";
            this.barButtonItem2.Id = 7;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.DataSource = this.bdsManejadorDatos;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEsIngreso,
            this.chkAplicaRetencion,
            this.rchkEsIngreso,
            this.repositoryItemCheckedComboBoxEdit1,
            this.chkAprovado});
            this.grid.Size = new System.Drawing.Size(621, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colPlanilla,
            this.colFechaInicial,
            this.colFechaFinal,
            this.colAprobado,
            this.colComentario});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.OptionsColumn.AllowEdit = false;
            this.colID.OptionsColumn.AllowFocus = false;
            // 
            // colPlanilla
            // 
            this.colPlanilla.Caption = "Planilla";
            this.colPlanilla.FieldName = "Planilla";
            this.colPlanilla.Name = "colPlanilla";
            this.colPlanilla.OptionsColumn.AllowEdit = false;
            this.colPlanilla.Visible = true;
            this.colPlanilla.VisibleIndex = 0;
            this.colPlanilla.Width = 182;
            // 
            // colFechaInicial
            // 
            this.colFechaInicial.Caption = "Fecha Inicial";
            this.colFechaInicial.FieldName = "FechaInicial";
            this.colFechaInicial.Name = "colFechaInicial";
            this.colFechaInicial.OptionsColumn.AllowEdit = false;
            this.colFechaInicial.Visible = true;
            this.colFechaInicial.VisibleIndex = 1;
            this.colFechaInicial.Width = 144;
            // 
            // colFechaFinal
            // 
            this.colFechaFinal.Caption = "Fecha Final";
            this.colFechaFinal.FieldName = "FechaFinal";
            this.colFechaFinal.Name = "colFechaFinal";
            this.colFechaFinal.OptionsColumn.AllowEdit = false;
            this.colFechaFinal.Visible = true;
            this.colFechaFinal.VisibleIndex = 2;
            this.colFechaFinal.Width = 127;
            // 
            // colAprobado
            // 
            this.colAprobado.Caption = "Aprobado";
            this.colAprobado.ColumnEdit = this.chkAprovado;
            this.colAprobado.FieldName = "Approved";
            this.colAprobado.Name = "colAprobado";
            this.colAprobado.Visible = true;
            this.colAprobado.VisibleIndex = 3;
            this.colAprobado.Width = 77;
            // 
            // chkAprovado
            // 
            this.chkAprovado.AutoHeight = false;
            this.chkAprovado.Caption = "Check";
            this.chkAprovado.Name = "chkAprovado";
            this.chkAprovado.Click += new System.EventHandler(this.chkAprovado_Click);
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comment";
            this.colComentario.Name = "colComentario";
            this.colComentario.OptionsColumn.AllowEdit = false;
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 4;
            this.colComentario.Width = 359;
            // 
            // chkEsIngreso
            // 
            this.chkEsIngreso.AutoHeight = false;
            this.chkEsIngreso.Caption = "Check";
            this.chkEsIngreso.Name = "chkEsIngreso";
            // 
            // chkAplicaRetencion
            // 
            this.chkAplicaRetencion.AutoHeight = false;
            this.chkAplicaRetencion.Caption = "Check";
            this.chkAplicaRetencion.Name = "chkAplicaRetencion";
            // 
            // rchkEsIngreso
            // 
            this.rchkEsIngreso.AllowFocused = false;
            this.rchkEsIngreso.AutoHeight = false;
            this.rchkEsIngreso.Caption = "Check";
            this.rchkEsIngreso.Name = "rchkEsIngreso";
            this.rchkEsIngreso.ReadOnly = true;
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            // 
            // splitContainerControlMain
            // 
            this.splitContainerControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlMain.Location = new System.Drawing.Point(0, 33);
            this.splitContainerControlMain.Name = "splitContainerControlMain";
            this.splitContainerControlMain.Panel1.Controls.Add(this.grid);
            this.splitContainerControlMain.Panel1.Text = "Panel1";
            this.splitContainerControlMain.Panel2.Controls.Add(this.xtraTabControlMain);
            this.splitContainerControlMain.Panel2.Text = "Panel2";
            this.splitContainerControlMain.Size = new System.Drawing.Size(1051, 521);
            this.splitContainerControlMain.SplitterPosition = 621;
            this.splitContainerControlMain.TabIndex = 6;
            this.splitContainerControlMain.Text = "splitContainerControl1";
            // 
            // xtraTabControlMain
            // 
            this.xtraTabControlMain.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            this.xtraTabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlMain.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControlMain.Name = "xtraTabControlMain";
            this.xtraTabControlMain.Size = new System.Drawing.Size(426, 521);
            this.xtraTabControlMain.TabIndex = 1;
            this.xtraTabControlMain.CloseButtonClick += new System.EventHandler(this.xtraTabControlMain_CloseButtonClick);
            // 
            // FormMovimientoCorteEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 554);
            this.Controls.Add(this.splitContainerControlMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormMovimientoCorteEmpleado";
            this.Text = "Lista Horas Extras";
            this.Load += new System.EventHandler(this.FormSucursal_Load);
            this.Controls.SetChildIndex(this.splitContainerControlMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAprovado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEsIngreso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaRetencion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkEsIngreso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlMain)).EndInit();
            this.splitContainerControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEsIngreso;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkAplicaRetencion;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkEsIngreso;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colPlanilla;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaInicial;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaFinal;
        private DevExpress.XtraGrid.Columns.GridColumn colAprobado;
        private DevExpress.XtraGrid.Columns.GridColumn colComentario;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlMain;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlMain;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkAprovado;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
    }
}

