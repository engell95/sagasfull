namespace SAGAS.Tesoreria.Forms
{
    partial class FormRetenciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRetenciones));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMonto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSub = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCuentaContable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlkCuentaContable = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colEsAlcaldia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chkAlcaldia = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrArea = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkCuentaContable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlcaldia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
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
            this.grid.EmbeddedNavigator.TextStringFormat = " {0:N0} de {1:N0}";
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrArea,
            this.rchkActivo,
            this.chkAlcaldia,
            this.rlkCuentaContable,
            this.repositoryItemCheckEdit1});
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colFecha,
            this.colNumero,
            this.colConcepto,
            this.colMonto,
            this.colSub,
            this.colCuentaContable,
            this.colEsAlcaldia,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colFecha
            // 
            this.colFecha.Caption = "Fecha";
            this.colFecha.FieldName = "FechaRegistro";
            this.colFecha.Name = "colFecha";
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 4;
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Numero";
            this.colNumero.FieldName = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.OptionsColumn.AllowEdit = false;
            this.colNumero.OptionsColumn.AllowFocus = false;
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 3;
            this.colNumero.Width = 113;
            // 
            // colConcepto
            // 
            this.colConcepto.Caption = "Concepto";
            this.colConcepto.FieldName = "Concepto";
            this.colConcepto.Name = "colConcepto";
            this.colConcepto.OptionsColumn.AllowEdit = false;
            this.colConcepto.OptionsColumn.AllowFocus = false;
            this.colConcepto.Visible = true;
            this.colConcepto.VisibleIndex = 5;
            this.colConcepto.Width = 506;
            // 
            // colMonto
            // 
            this.colMonto.Caption = "Monto";
            this.colMonto.DisplayFormat.FormatString = "n2";
            this.colMonto.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMonto.FieldName = "Monto";
            this.colMonto.Name = "colMonto";
            this.colMonto.OptionsColumn.AllowEdit = false;
            this.colMonto.OptionsColumn.AllowFocus = false;
            this.colMonto.Visible = true;
            this.colMonto.VisibleIndex = 6;
            this.colMonto.Width = 84;
            // 
            // colSub
            // 
            this.colSub.Caption = "Sub Total";
            this.colSub.DisplayFormat.FormatString = "n2";
            this.colSub.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSub.FieldName = "SubTotal";
            this.colSub.Name = "colSub";
            this.colSub.OptionsColumn.AllowEdit = false;
            this.colSub.OptionsColumn.AllowFocus = false;
            this.colSub.Visible = true;
            this.colSub.VisibleIndex = 7;
            this.colSub.Width = 82;
            // 
            // colCuentaContable
            // 
            this.colCuentaContable.Caption = "Cuenta Contable";
            this.colCuentaContable.ColumnEdit = this.rlkCuentaContable;
            this.colCuentaContable.FieldName = "CuentaContableID";
            this.colCuentaContable.Name = "colCuentaContable";
            this.colCuentaContable.OptionsColumn.AllowEdit = false;
            this.colCuentaContable.OptionsColumn.AllowFocus = false;
            this.colCuentaContable.Visible = true;
            this.colCuentaContable.VisibleIndex = 8;
            this.colCuentaContable.Width = 212;
            // 
            // rlkCuentaContable
            // 
            this.rlkCuentaContable.AutoHeight = false;
            this.rlkCuentaContable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlkCuentaContable.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuenta Contable")});
            this.rlkCuentaContable.DisplayMember = "Display";
            this.rlkCuentaContable.Name = "rlkCuentaContable";
            this.rlkCuentaContable.ValueMember = "ID";
            // 
            // colEsAlcaldia
            // 
            this.colEsAlcaldia.Caption = "Es Alcaldia";
            this.colEsAlcaldia.ColumnEdit = this.chkAlcaldia;
            this.colEsAlcaldia.FieldName = "EsAlcaldia";
            this.colEsAlcaldia.Name = "colEsAlcaldia";
            this.colEsAlcaldia.OptionsColumn.AllowEdit = false;
            this.colEsAlcaldia.OptionsColumn.AllowFocus = false;
            this.colEsAlcaldia.Visible = true;
            this.colEsAlcaldia.VisibleIndex = 9;
            // 
            // chkAlcaldia
            // 
            this.chkAlcaldia.AutoHeight = false;
            this.chkAlcaldia.Caption = "Check";
            this.chkAlcaldia.Name = "chkAlcaldia";
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "MovimientoID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Proveedor";
            this.gridColumn2.FieldName = "Proveedor";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 195;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Estación Servicio";
            this.gridColumn3.FieldName = "Estacion";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 173;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Tipo Movimiento";
            this.gridColumn4.FieldName = "MovimientoTipo";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 116;
            // 
            // lkrArea
            // 
            this.lkrArea.AutoHeight = false;
            this.lkrArea.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrArea.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Áreas")});
            this.lkrArea.Name = "lkrArea";
            this.lkrArea.NullText = "<Seleccione el Área>";
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Anulado";
            this.gridColumn5.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn5.FieldName = "Anulado";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 10;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // FormRetenciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormRetenciones";
            this.Text = "Lista de Retenciones";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkCuentaContable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAlcaldia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrArea;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colConcepto;
        private DevExpress.XtraGrid.Columns.GridColumn colMonto;
        private DevExpress.XtraGrid.Columns.GridColumn colSub;
        private DevExpress.XtraGrid.Columns.GridColumn colCuentaContable;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlkCuentaContable;
        private DevExpress.XtraGrid.Columns.GridColumn colEsAlcaldia;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkAlcaldia;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    }
}

