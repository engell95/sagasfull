namespace SAGAS.Arqueo.Forms
{
    partial class FormArqueoIsla
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormArqueoIsla));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionServicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colSubEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTurno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIslaID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpIslas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTecnico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpTecnico = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colFechaCreado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstado = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colArqueoEspecial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).BeginInit();
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
            this.grid.EmbeddedNavigator.TextStringFormat = "{0:N0} de {1:N0}";
            this.grid.Location = new System.Drawing.Point(0, 33);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpEstacionServicio,
            this.rpIslas,
            this.rpTecnico,
            this.rpEstado,
            this.lkSES});
            this.grid.Size = new System.Drawing.Size(804, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colEstacionServicio,
            this.colSubEstacion,
            this.colTurno,
            this.colIslaID,
            this.colNumero,
            this.colTecnico,
            this.colFechaCreado,
            this.colEstado,
            this.colArqueoEspecial,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colNumero, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colEstacionServicio
            // 
            this.colEstacionServicio.Caption = "Estación de Servicio";
            this.colEstacionServicio.ColumnEdit = this.rpEstacionServicio;
            this.colEstacionServicio.FieldName = "EstacionServicioID";
            this.colEstacionServicio.Name = "colEstacionServicio";
            this.colEstacionServicio.Visible = true;
            this.colEstacionServicio.VisibleIndex = 0;
            this.colEstacionServicio.Width = 166;
            // 
            // rpEstacionServicio
            // 
            this.rpEstacionServicio.AutoHeight = false;
            this.rpEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio")});
            this.rpEstacionServicio.Name = "rpEstacionServicio";
            // 
            // colSubEstacion
            // 
            this.colSubEstacion.Caption = "Sub Estación";
            this.colSubEstacion.ColumnEdit = this.lkSES;
            this.colSubEstacion.FieldName = "SubEstacionID";
            this.colSubEstacion.Name = "colSubEstacion";
            this.colSubEstacion.Visible = true;
            this.colSubEstacion.VisibleIndex = 1;
            this.colSubEstacion.Width = 91;
            // 
            // lkSES
            // 
            this.lkSES.AutoHeight = false;
            this.lkSES.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSES.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Sub Estación")});
            this.lkSES.Name = "lkSES";
            // 
            // colTurno
            // 
            this.colTurno.Caption = "Turno";
            this.colTurno.FieldName = "Turno";
            this.colTurno.Name = "colTurno";
            this.colTurno.Visible = true;
            this.colTurno.VisibleIndex = 2;
            // 
            // colIslaID
            // 
            this.colIslaID.Caption = "Isla";
            this.colIslaID.ColumnEdit = this.rpIslas;
            this.colIslaID.FieldName = "IslaID";
            this.colIslaID.Name = "colIslaID";
            this.colIslaID.Visible = true;
            this.colIslaID.VisibleIndex = 3;
            this.colIslaID.Width = 89;
            // 
            // rpIslas
            // 
            this.rpIslas.AutoHeight = false;
            this.rpIslas.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpIslas.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Islas")});
            this.rpIslas.Name = "rpIslas";
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Número";
            this.colNumero.FieldName = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 4;
            // 
            // colTecnico
            // 
            this.colTecnico.Caption = "Técnico";
            this.colTecnico.ColumnEdit = this.rpTecnico;
            this.colTecnico.FieldName = "TecnicoID";
            this.colTecnico.Name = "colTecnico";
            this.colTecnico.Visible = true;
            this.colTecnico.VisibleIndex = 5;
            this.colTecnico.Width = 162;
            // 
            // rpTecnico
            // 
            this.rpTecnico.AutoHeight = false;
            this.rpTecnico.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpTecnico.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Técnicos")});
            this.rpTecnico.Name = "rpTecnico";
            // 
            // colFechaCreado
            // 
            this.colFechaCreado.Caption = "Fecha Creado";
            this.colFechaCreado.FieldName = "FechaCreado";
            this.colFechaCreado.Name = "colFechaCreado";
            this.colFechaCreado.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.colFechaCreado.Visible = true;
            this.colFechaCreado.VisibleIndex = 8;
            this.colFechaCreado.Width = 83;
            // 
            // colEstado
            // 
            this.colEstado.Caption = "Estado";
            this.colEstado.ColumnEdit = this.rpEstado;
            this.colEstado.FieldName = "Estado";
            this.colEstado.Name = "colEstado";
            this.colEstado.Visible = true;
            this.colEstado.VisibleIndex = 9;
            this.colEstado.Width = 86;
            // 
            // rpEstado
            // 
            this.rpEstado.AutoHeight = false;
            this.rpEstado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstado.Name = "rpEstado";
            // 
            // colArqueoEspecial
            // 
            this.colArqueoEspecial.Caption = "Es Arqueo Especial?";
            this.colArqueoEspecial.FieldName = "ArqueoEspecial";
            this.colArqueoEspecial.Name = "colArqueoEspecial";
            this.colArqueoEspecial.Visible = true;
            this.colArqueoEspecial.VisibleIndex = 10;
            this.colArqueoEspecial.Width = 107;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Año";
            this.gridColumn1.FieldName = "Year";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 6;
            this.gridColumn1.Width = 40;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Mes";
            this.gridColumn2.FieldName = "Mes";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 7;
            this.gridColumn2.Width = 66;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Oficial ?";
            this.gridColumn3.FieldName = "Oficial";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 11;
            // 
            // FormArqueoIsla
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormArqueoIsla";
            this.Text = "Lista de Arqueo de Islas";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colIslaID;
        private DevExpress.XtraGrid.Columns.GridColumn colTecnico;
        private DevExpress.XtraGrid.Columns.GridColumn colArqueoEspecial;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpIslas;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTecnico;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colTurno;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaCreado;
        private DevExpress.XtraGrid.Columns.GridColumn colEstado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstado;
        private DevExpress.XtraGrid.Columns.GridColumn colSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}

