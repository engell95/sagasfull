namespace SAGAS.Parametros.Forms
{
    partial class FormLicencia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLicencia));
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.gridLicencia = new DevExpress.XtraGrid.GridControl();
            this.gvDataLicencia = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNamepc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBaseBoard = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerialDisk = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCPUId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLogicalDiskSerial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkeEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTipoConexion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerialRegistro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerialBD = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLicencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataLicencia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlTop
            // 
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(982, 56);
            this.panelControlTop.TabIndex = 0;
            // 
            // gridLicencia
            // 
            this.gridLicencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLicencia.Location = new System.Drawing.Point(0, 56);
            this.gridLicencia.MainView = this.gvDataLicencia;
            this.gridLicencia.Name = "gridLicencia";
            this.gridLicencia.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkeEstacionServicio});
            this.gridLicencia.Size = new System.Drawing.Size(982, 519);
            this.gridLicencia.TabIndex = 1;
            this.gridLicencia.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataLicencia});
            // 
            // gvDataLicencia
            // 
            this.gvDataLicencia.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNamepc,
            this.colBaseBoard,
            this.colSerialDisk,
            this.colCPUId,
            this.colLogicalDiskSerial,
            this.colEstacionServicio,
            this.colTipoConexion,
            this.colSerialRegistro,
            this.colSerialBD});
            this.gvDataLicencia.GridControl = this.gridLicencia;
            this.gvDataLicencia.Name = "gvDataLicencia";
            this.gvDataLicencia.OptionsBehavior.Editable = false;
            this.gvDataLicencia.OptionsMenu.EnableFooterMenu = false;
            this.gvDataLicencia.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataLicencia.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvDataLicencia.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataLicencia.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataLicencia.OptionsMenu.ShowSplitItem = false;
            this.gvDataLicencia.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvDataLicencia.OptionsSelection.MultiSelect = true;
            this.gvDataLicencia.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gvDataLicencia.OptionsView.ColumnAutoWidth = false;
            this.gvDataLicencia.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colID.Width = 48;
            // 
            // colNamepc
            // 
            this.colNamepc.Caption = "Nombre Computadora";
            this.colNamepc.FieldName = "NamePC";
            this.colNamepc.Name = "colNamepc";
            this.colNamepc.Visible = true;
            this.colNamepc.VisibleIndex = 1;
            this.colNamepc.Width = 127;
            // 
            // colBaseBoard
            // 
            this.colBaseBoard.Caption = "Placa Base";
            this.colBaseBoard.FieldName = "BaseBoard";
            this.colBaseBoard.Name = "colBaseBoard";
            this.colBaseBoard.Visible = true;
            this.colBaseBoard.VisibleIndex = 2;
            this.colBaseBoard.Width = 74;
            // 
            // colSerialDisk
            // 
            this.colSerialDisk.Caption = "Serial Disco";
            this.colSerialDisk.FieldName = "SerialDisk";
            this.colSerialDisk.Name = "colSerialDisk";
            this.colSerialDisk.Visible = true;
            this.colSerialDisk.VisibleIndex = 3;
            this.colSerialDisk.Width = 74;
            // 
            // colCPUId
            // 
            this.colCPUId.Caption = "ID CPU";
            this.colCPUId.FieldName = "CPUId";
            this.colCPUId.Name = "colCPUId";
            this.colCPUId.Visible = true;
            this.colCPUId.VisibleIndex = 4;
            this.colCPUId.Width = 74;
            // 
            // colLogicalDiskSerial
            // 
            this.colLogicalDiskSerial.Caption = "Serial Disco Lógico";
            this.colLogicalDiskSerial.FieldName = "LogicalDiskSerial";
            this.colLogicalDiskSerial.Name = "colLogicalDiskSerial";
            this.colLogicalDiskSerial.Visible = true;
            this.colLogicalDiskSerial.VisibleIndex = 5;
            this.colLogicalDiskSerial.Width = 106;
            // 
            // colEstacionServicio
            // 
            this.colEstacionServicio.Caption = "Estación de Servicio";
            this.colEstacionServicio.ColumnEdit = this.lkeEstacionServicio;
            this.colEstacionServicio.FieldName = "EstacionServicio";
            this.colEstacionServicio.Name = "colEstacionServicio";
            this.colEstacionServicio.Visible = true;
            this.colEstacionServicio.VisibleIndex = 6;
            this.colEstacionServicio.Width = 121;
            // 
            // lkeEstacionServicio
            // 
            this.lkeEstacionServicio.AutoHeight = false;
            this.lkeEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombres", "Estaciones de Servicio")});
            this.lkeEstacionServicio.Name = "lkeEstacionServicio";
            // 
            // colTipoConexion
            // 
            this.colTipoConexion.Caption = "Tipo de Conexión";
            this.colTipoConexion.FieldName = "TipoConexionStr";
            this.colTipoConexion.Name = "colTipoConexion";
            this.colTipoConexion.Visible = true;
            this.colTipoConexion.VisibleIndex = 7;
            this.colTipoConexion.Width = 103;
            // 
            // colSerialRegistro
            // 
            this.colSerialRegistro.Caption = "SerialRegistro";
            this.colSerialRegistro.FieldName = "SerialRegistro";
            this.colSerialRegistro.Name = "colSerialRegistro";
            this.colSerialRegistro.Visible = true;
            this.colSerialRegistro.VisibleIndex = 8;
            this.colSerialRegistro.Width = 263;
            // 
            // colSerialBD
            // 
            this.colSerialBD.Caption = "Serial BD";
            this.colSerialBD.FieldName = "SerialBD";
            this.colSerialBD.Name = "colSerialBD";
            this.colSerialBD.Visible = true;
            this.colSerialBD.VisibleIndex = 9;
            this.colSerialBD.Width = 281;
            // 
            // FormLicencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 575);
            this.Controls.Add(this.gridLicencia);
            this.Controls.Add(this.panelControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLicencia";
            this.Text = "Lista de Licencias";
            this.Load += new System.EventHandler(this.FormLicencia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLicencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataLicencia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraGrid.GridControl gridLicencia;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataLicencia;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNamepc;
        private DevExpress.XtraGrid.Columns.GridColumn colBaseBoard;
        private DevExpress.XtraGrid.Columns.GridColumn colSerialDisk;
        private DevExpress.XtraGrid.Columns.GridColumn colCPUId;
        private DevExpress.XtraGrid.Columns.GridColumn colLogicalDiskSerial;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoConexion;
        private DevExpress.XtraGrid.Columns.GridColumn colSerialRegistro;
        private DevExpress.XtraGrid.Columns.GridColumn colSerialBD;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkeEstacionServicio;
    }
}