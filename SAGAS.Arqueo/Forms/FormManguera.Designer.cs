namespace SAGAS.Arqueo.Forms
{
    partial class FormManguera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManguera));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTanqueID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rglTanque = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.rgvTanque = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDTanque = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTanqueNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProductoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colColor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemColorEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemColorEdit();
            this.colDispensadorID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrDispensador = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colEstacionServicioID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colSubEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colMecanica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEfectivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colElectronica = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSerie = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFlujoRapido = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrecioDiferenciado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDescuento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rglTanque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTanque)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrDispensador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
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
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrDispensador,
            this.rglTanque,
            this.repositoryItemColorEdit1,
            this.lkEstacionServicio,
            this.lkSES,
            this.rchkActivo});
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
            this.colNumero,
            this.colTanqueID,
            this.colColor,
            this.colDispensadorID,
            this.colEstacionServicioID,
            this.colSubEstacion,
            this.colMecanica,
            this.colEfectivo,
            this.colElectronica,
            this.colSerie,
            this.colLado,
            this.colFlujoRapido,
            this.colPrecioDiferenciado,
            this.colDescuento,
            this.colComentario,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colEstacionServicioID, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Número";
            this.colNumero.FieldName = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 0;
            // 
            // colTanqueID
            // 
            this.colTanqueID.Caption = "Tanque";
            this.colTanqueID.ColumnEdit = this.rglTanque;
            this.colTanqueID.FieldName = "TanqueID";
            this.colTanqueID.Name = "colTanqueID";
            this.colTanqueID.Visible = true;
            this.colTanqueID.VisibleIndex = 1;
            this.colTanqueID.Width = 161;
            // 
            // rglTanque
            // 
            this.rglTanque.AutoHeight = false;
            this.rglTanque.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rglTanque.Name = "rglTanque";
            this.rglTanque.NullText = "<No tiene Tanque Asignado>";
            this.rglTanque.View = this.rgvTanque;
            // 
            // rgvTanque
            // 
            this.rgvTanque.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDTanque,
            this.colTanqueNombre,
            this.colProductoNombre});
            this.rgvTanque.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.rgvTanque.Name = "rgvTanque";
            this.rgvTanque.OptionsBehavior.Editable = false;
            this.rgvTanque.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.rgvTanque.OptionsView.ShowGroupPanel = false;
            // 
            // colIDTanque
            // 
            this.colIDTanque.Caption = "Tanque";
            this.colIDTanque.FieldName = "IDTanque";
            this.colIDTanque.Name = "colIDTanque";
            // 
            // colTanqueNombre
            // 
            this.colTanqueNombre.Caption = "Tanques";
            this.colTanqueNombre.FieldName = "TanqueNombre";
            this.colTanqueNombre.Name = "colTanqueNombre";
            this.colTanqueNombre.Visible = true;
            this.colTanqueNombre.VisibleIndex = 0;
            // 
            // colProductoNombre
            // 
            this.colProductoNombre.Caption = "Combustibles";
            this.colProductoNombre.FieldName = "ProductoNombre";
            this.colProductoNombre.Name = "colProductoNombre";
            this.colProductoNombre.Visible = true;
            this.colProductoNombre.VisibleIndex = 1;
            // 
            // colColor
            // 
            this.colColor.Caption = "Color";
            this.colColor.ColumnEdit = this.repositoryItemColorEdit1;
            this.colColor.FieldName = "Color";
            this.colColor.Name = "colColor";
            this.colColor.Visible = true;
            this.colColor.VisibleIndex = 2;
            // 
            // repositoryItemColorEdit1
            // 
            this.repositoryItemColorEdit1.AutoHeight = false;
            this.repositoryItemColorEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemColorEdit1.Name = "repositoryItemColorEdit1";
            this.repositoryItemColorEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            // 
            // colDispensadorID
            // 
            this.colDispensadorID.Caption = "Dispensador";
            this.colDispensadorID.ColumnEdit = this.lkrDispensador;
            this.colDispensadorID.FieldName = "DispensadorID";
            this.colDispensadorID.Name = "colDispensadorID";
            this.colDispensadorID.Visible = true;
            this.colDispensadorID.VisibleIndex = 3;
            // 
            // lkrDispensador
            // 
            this.lkrDispensador.AutoHeight = false;
            this.lkrDispensador.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrDispensador.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Dispensadores")});
            this.lkrDispensador.Name = "lkrDispensador";
            this.lkrDispensador.NullText = "<No tiene Dispensador Asignado>";
            // 
            // colEstacionServicioID
            // 
            this.colEstacionServicioID.Caption = "Estación de Servicio";
            this.colEstacionServicioID.ColumnEdit = this.lkEstacionServicio;
            this.colEstacionServicioID.FieldName = "EstacionServicioID";
            this.colEstacionServicioID.Name = "colEstacionServicioID";
            this.colEstacionServicioID.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
            this.colEstacionServicioID.Visible = true;
            this.colEstacionServicioID.VisibleIndex = 4;
            // 
            // lkEstacionServicio
            // 
            this.lkEstacionServicio.AutoHeight = false;
            this.lkEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkEstacionServicio.Name = "lkEstacionServicio";
            this.lkEstacionServicio.NullText = "<No tiene Estación de Servicio Asignada>";
            // 
            // colSubEstacion
            // 
            this.colSubEstacion.Caption = "Sub Estación";
            this.colSubEstacion.ColumnEdit = this.lkSES;
            this.colSubEstacion.FieldName = "SubEstacionID";
            this.colSubEstacion.Name = "colSubEstacion";
            this.colSubEstacion.Visible = true;
            this.colSubEstacion.VisibleIndex = 5;
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
            // colMecanica
            // 
            this.colMecanica.Caption = "Lectura Mecánica";
            this.colMecanica.DisplayFormat.FormatString = "N3";
            this.colMecanica.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMecanica.FieldName = "LecturaMecanica";
            this.colMecanica.Name = "colMecanica";
            this.colMecanica.Visible = true;
            this.colMecanica.VisibleIndex = 6;
            this.colMecanica.Width = 100;
            // 
            // colEfectivo
            // 
            this.colEfectivo.Caption = "Lectura Efectivo";
            this.colEfectivo.DisplayFormat.FormatString = "N3";
            this.colEfectivo.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colEfectivo.FieldName = "LecturaEfectivo";
            this.colEfectivo.Name = "colEfectivo";
            this.colEfectivo.Visible = true;
            this.colEfectivo.VisibleIndex = 7;
            this.colEfectivo.Width = 95;
            // 
            // colElectronica
            // 
            this.colElectronica.Caption = "Lectura Eléctronica";
            this.colElectronica.DisplayFormat.FormatString = "N3";
            this.colElectronica.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colElectronica.FieldName = "LecturaElectronica";
            this.colElectronica.Name = "colElectronica";
            this.colElectronica.Visible = true;
            this.colElectronica.VisibleIndex = 8;
            this.colElectronica.Width = 99;
            // 
            // colSerie
            // 
            this.colSerie.Caption = "Serie";
            this.colSerie.FieldName = "Serie";
            this.colSerie.Name = "colSerie";
            this.colSerie.Visible = true;
            this.colSerie.VisibleIndex = 9;
            // 
            // colLado
            // 
            this.colLado.Caption = "Lado";
            this.colLado.FieldName = "Lado";
            this.colLado.Name = "colLado";
            this.colLado.Visible = true;
            this.colLado.VisibleIndex = 10;
            // 
            // colFlujoRapido
            // 
            this.colFlujoRapido.Caption = "FlujoRapido";
            this.colFlujoRapido.FieldName = "FlujoRapido";
            this.colFlujoRapido.Name = "colFlujoRapido";
            this.colFlujoRapido.Visible = true;
            this.colFlujoRapido.VisibleIndex = 11;
            this.colFlujoRapido.Width = 82;
            // 
            // colPrecioDiferenciado
            // 
            this.colPrecioDiferenciado.Caption = "PrecioDiferenciado";
            this.colPrecioDiferenciado.FieldName = "PrecioDiferenciado";
            this.colPrecioDiferenciado.Name = "colPrecioDiferenciado";
            this.colPrecioDiferenciado.Visible = true;
            this.colPrecioDiferenciado.VisibleIndex = 12;
            this.colPrecioDiferenciado.Width = 110;
            // 
            // colDescuento
            // 
            this.colDescuento.Caption = "Descuento";
            this.colDescuento.FieldName = "Descuento";
            this.colDescuento.Name = "colDescuento";
            this.colDescuento.Visible = true;
            this.colDescuento.VisibleIndex = 13;
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 14;
            this.colComentario.Width = 592;
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rchkActivo;
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.OptionsColumn.AllowEdit = false;
            this.colActivo.OptionsColumn.ReadOnly = true;
            this.colActivo.Visible = true;
            this.colActivo.VisibleIndex = 15;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // FormManguera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormManguera";
            this.Text = "Lista de Mangueras";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rglTanque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgvTanque)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemColorEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrDispensador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colComentario;
        private DevExpress.XtraGrid.Columns.GridColumn colTanqueID;
        private DevExpress.XtraGrid.Columns.GridColumn colDispensadorID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrDispensador;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit rglTanque;
        private DevExpress.XtraGrid.Views.Grid.GridView rgvTanque;
        private DevExpress.XtraGrid.Columns.GridColumn colSerie;
        private DevExpress.XtraGrid.Columns.GridColumn colLado;
        private DevExpress.XtraGrid.Columns.GridColumn colFlujoRapido;
        private DevExpress.XtraGrid.Columns.GridColumn colPrecioDiferenciado;
        private DevExpress.XtraGrid.Columns.GridColumn colIDTanque;
        private DevExpress.XtraGrid.Columns.GridColumn colTanqueNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colProductoNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colDescuento;
        private DevExpress.XtraGrid.Columns.GridColumn colColor;
        private DevExpress.XtraEditors.Repository.RepositoryItemColorEdit repositoryItemColorEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colMecanica;
        private DevExpress.XtraGrid.Columns.GridColumn colEfectivo;
        private DevExpress.XtraGrid.Columns.GridColumn colElectronica;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionServicioID;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colSubEstacion;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}

