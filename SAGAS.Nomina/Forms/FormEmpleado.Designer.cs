namespace SAGAS.Nomina.Forms
{
    partial class FormEmpleado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmpleado));
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombres = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApellidos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlanillaNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDireccion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTelefono = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCelular = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGenero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkGenero = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colFechaNacimiento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCedula = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProfesionOficioNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumeroNominaInss = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumeroSeguroSocial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSalarioActual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaContrato = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaIngreso = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaSalida = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCargoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAreaNominaNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoSangre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPariente = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPagaPension = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMontoPension = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEsPorcentaje = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDiferenteDeTarjeta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumeroCuentaTarjeta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCalculoSalarioPromedioAguinaldo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstadoCivilNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstructuraOrganizativaNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrPlanilla = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.cboGenero = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkGenero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrPlanilla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGenero)).BeginInit();
            this.SuspendLayout();
            // 
            // bdsManejadorDatos
            // 
            this.bdsManejadorDatos.DataSource = typeof(SAGAS.Entidad.VistaEmleados);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(804, 33);
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
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrPlanilla,
            this.cboGenero,
            this.lkGenero});
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
            this.colCodigo,
            this.colNombres,
            this.colApellidos,
            this.colPlanillaNombre,
            this.colActivo,
            this.colDireccion,
            this.colTelefono,
            this.colCelular,
            this.colEmail,
            this.colGenero,
            this.colFechaNacimiento,
            this.colCedula,
            this.colProfesionOficioNombre,
            this.colNumeroNominaInss,
            this.colNumeroSeguroSocial,
            this.colSalarioActual,
            this.colFechaContrato,
            this.colFechaIngreso,
            this.colFechaSalida,
            this.colCargoNombre,
            this.colAreaNominaNombre,
            this.colTipoSangre,
            this.colPariente,
            this.colPagaPension,
            this.colMontoPension,
            this.colEsPorcentaje,
            this.colDiferenteDeTarjeta,
            this.colNumeroCuentaTarjeta,
            this.colCalculoSalarioPromedioAguinaldo,
            this.colEstadoCivilNombre,
            this.colEstructuraOrganizativaNombre,
            this.gridColumn1});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCargoNombre, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigo
            // 
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            // 
            // colNombres
            // 
            this.colNombres.FieldName = "Nombres";
            this.colNombres.Name = "colNombres";
            this.colNombres.Visible = true;
            this.colNombres.VisibleIndex = 1;
            // 
            // colApellidos
            // 
            this.colApellidos.FieldName = "Apellidos";
            this.colApellidos.Name = "colApellidos";
            this.colApellidos.Visible = true;
            this.colApellidos.VisibleIndex = 2;
            // 
            // colPlanillaNombre
            // 
            this.colPlanillaNombre.FieldName = "PlanillaNombre";
            this.colPlanillaNombre.Name = "colPlanillaNombre";
            this.colPlanillaNombre.Visible = true;
            this.colPlanillaNombre.VisibleIndex = 3;
            this.colPlanillaNombre.Width = 172;
            // 
            // colActivo
            // 
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.Visible = true;
            this.colActivo.VisibleIndex = 4;
            this.colActivo.Width = 53;
            // 
            // colDireccion
            // 
            this.colDireccion.FieldName = "Direccion";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.Visible = true;
            this.colDireccion.VisibleIndex = 20;
            // 
            // colTelefono
            // 
            this.colTelefono.FieldName = "Telefono";
            this.colTelefono.Name = "colTelefono";
            this.colTelefono.Visible = true;
            this.colTelefono.VisibleIndex = 21;
            // 
            // colCelular
            // 
            this.colCelular.FieldName = "Celular";
            this.colCelular.Name = "colCelular";
            this.colCelular.Visible = true;
            this.colCelular.VisibleIndex = 22;
            // 
            // colEmail
            // 
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 23;
            // 
            // colGenero
            // 
            this.colGenero.ColumnEdit = this.lkGenero;
            this.colGenero.FieldName = "Genero";
            this.colGenero.Name = "colGenero";
            this.colGenero.Visible = true;
            this.colGenero.VisibleIndex = 16;
            // 
            // lkGenero
            // 
            this.lkGenero.AutoHeight = false;
            this.lkGenero.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkGenero.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Genero", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkGenero.DisplayMember = "Nombre";
            this.lkGenero.Name = "lkGenero";
            this.lkGenero.ValueMember = "ID";
            // 
            // colFechaNacimiento
            // 
            this.colFechaNacimiento.FieldName = "FechaNacimiento";
            this.colFechaNacimiento.Name = "colFechaNacimiento";
            this.colFechaNacimiento.Visible = true;
            this.colFechaNacimiento.VisibleIndex = 17;
            this.colFechaNacimiento.Width = 100;
            // 
            // colCedula
            // 
            this.colCedula.FieldName = "Cedula";
            this.colCedula.Name = "colCedula";
            this.colCedula.Visible = true;
            this.colCedula.VisibleIndex = 18;
            // 
            // colProfesionOficioNombre
            // 
            this.colProfesionOficioNombre.FieldName = "ProfesionOficioNombre";
            this.colProfesionOficioNombre.Name = "colProfesionOficioNombre";
            this.colProfesionOficioNombre.Visible = true;
            this.colProfesionOficioNombre.VisibleIndex = 19;
            this.colProfesionOficioNombre.Width = 96;
            // 
            // colNumeroNominaInss
            // 
            this.colNumeroNominaInss.FieldName = "NumeroNominaInss";
            this.colNumeroNominaInss.Name = "colNumeroNominaInss";
            this.colNumeroNominaInss.Visible = true;
            this.colNumeroNominaInss.VisibleIndex = 8;
            this.colNumeroNominaInss.Width = 126;
            // 
            // colNumeroSeguroSocial
            // 
            this.colNumeroSeguroSocial.FieldName = "NumeroSeguroSocial";
            this.colNumeroSeguroSocial.Name = "colNumeroSeguroSocial";
            this.colNumeroSeguroSocial.Visible = true;
            this.colNumeroSeguroSocial.VisibleIndex = 9;
            this.colNumeroSeguroSocial.Width = 146;
            // 
            // colSalarioActual
            // 
            this.colSalarioActual.DisplayFormat.FormatString = "n2";
            this.colSalarioActual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSalarioActual.FieldName = "SalarioActual";
            this.colSalarioActual.Name = "colSalarioActual";
            this.colSalarioActual.Visible = true;
            this.colSalarioActual.VisibleIndex = 10;
            this.colSalarioActual.Width = 118;
            // 
            // colFechaContrato
            // 
            this.colFechaContrato.FieldName = "FechaContrato";
            this.colFechaContrato.Name = "colFechaContrato";
            this.colFechaContrato.Visible = true;
            this.colFechaContrato.VisibleIndex = 13;
            // 
            // colFechaIngreso
            // 
            this.colFechaIngreso.FieldName = "FechaIngreso";
            this.colFechaIngreso.Name = "colFechaIngreso";
            this.colFechaIngreso.Visible = true;
            this.colFechaIngreso.VisibleIndex = 12;
            // 
            // colFechaSalida
            // 
            this.colFechaSalida.FieldName = "FechaSalida";
            this.colFechaSalida.Name = "colFechaSalida";
            this.colFechaSalida.Visible = true;
            this.colFechaSalida.VisibleIndex = 5;
            // 
            // colCargoNombre
            // 
            this.colCargoNombre.FieldName = "CargoNombre";
            this.colCargoNombre.Name = "colCargoNombre";
            this.colCargoNombre.Visible = true;
            this.colCargoNombre.VisibleIndex = 14;
            // 
            // colAreaNominaNombre
            // 
            this.colAreaNominaNombre.FieldName = "AreaNominaNombre";
            this.colAreaNominaNombre.Name = "colAreaNominaNombre";
            this.colAreaNominaNombre.Visible = true;
            this.colAreaNominaNombre.VisibleIndex = 15;
            // 
            // colTipoSangre
            // 
            this.colTipoSangre.FieldName = "TipoSangre";
            this.colTipoSangre.Name = "colTipoSangre";
            this.colTipoSangre.Visible = true;
            this.colTipoSangre.VisibleIndex = 25;
            // 
            // colPariente
            // 
            this.colPariente.FieldName = "Pariente";
            this.colPariente.Name = "colPariente";
            this.colPariente.Visible = true;
            this.colPariente.VisibleIndex = 26;
            // 
            // colPagaPension
            // 
            this.colPagaPension.FieldName = "PagaPension";
            this.colPagaPension.Name = "colPagaPension";
            this.colPagaPension.Visible = true;
            this.colPagaPension.VisibleIndex = 27;
            // 
            // colMontoPension
            // 
            this.colMontoPension.FieldName = "MontoPension";
            this.colMontoPension.Name = "colMontoPension";
            this.colMontoPension.Visible = true;
            this.colMontoPension.VisibleIndex = 29;
            // 
            // colEsPorcentaje
            // 
            this.colEsPorcentaje.FieldName = "EsPorcentaje";
            this.colEsPorcentaje.Name = "colEsPorcentaje";
            this.colEsPorcentaje.Visible = true;
            this.colEsPorcentaje.VisibleIndex = 28;
            // 
            // colDiferenteDeTarjeta
            // 
            this.colDiferenteDeTarjeta.FieldName = "DiferenteDeTarjeta";
            this.colDiferenteDeTarjeta.Name = "colDiferenteDeTarjeta";
            this.colDiferenteDeTarjeta.Visible = true;
            this.colDiferenteDeTarjeta.VisibleIndex = 30;
            // 
            // colNumeroCuentaTarjeta
            // 
            this.colNumeroCuentaTarjeta.FieldName = "NumeroCuentaTarjeta";
            this.colNumeroCuentaTarjeta.Name = "colNumeroCuentaTarjeta";
            this.colNumeroCuentaTarjeta.Visible = true;
            this.colNumeroCuentaTarjeta.VisibleIndex = 6;
            this.colNumeroCuentaTarjeta.Width = 127;
            // 
            // colCalculoSalarioPromedioAguinaldo
            // 
            this.colCalculoSalarioPromedioAguinaldo.FieldName = "CalculoSalarioPromedioAguinaldo";
            this.colCalculoSalarioPromedioAguinaldo.Name = "colCalculoSalarioPromedioAguinaldo";
            this.colCalculoSalarioPromedioAguinaldo.Visible = true;
            this.colCalculoSalarioPromedioAguinaldo.VisibleIndex = 31;
            // 
            // colEstadoCivilNombre
            // 
            this.colEstadoCivilNombre.FieldName = "EstadoCivilNombre";
            this.colEstadoCivilNombre.Name = "colEstadoCivilNombre";
            this.colEstadoCivilNombre.Visible = true;
            this.colEstadoCivilNombre.VisibleIndex = 24;
            // 
            // colEstructuraOrganizativaNombre
            // 
            this.colEstructuraOrganizativaNombre.FieldName = "EstructuraOrganizativaNombre";
            this.colEstructuraOrganizativaNombre.Name = "colEstructuraOrganizativaNombre";
            this.colEstructuraOrganizativaNombre.Visible = true;
            this.colEstructuraOrganizativaNombre.VisibleIndex = 7;
            this.colEstructuraOrganizativaNombre.Width = 150;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Depreciación";
            this.gridColumn1.DisplayFormat.FormatString = "N2";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn1.FieldName = "Bono";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 11;
            // 
            // lkrPlanilla
            // 
            this.lkrPlanilla.AutoHeight = false;
            this.lkrPlanilla.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrPlanilla.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Planilla")});
            this.lkrPlanilla.Name = "lkrPlanilla";
            // 
            // cboGenero
            // 
            this.cboGenero.AutoHeight = false;
            this.cboGenero.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboGenero.Items.AddRange(new object[] {
            "M",
            "F"});
            this.cboGenero.Name = "cboGenero";
            this.cboGenero.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // FormEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormEmpleado";
            this.Text = "Lista de Empleados";
            this.Load += new System.EventHandler(this.FormSucursal_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkGenero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrPlanilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGenero)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrPlanilla;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colNombres;
        private DevExpress.XtraGrid.Columns.GridColumn colApellidos;
        private DevExpress.XtraGrid.Columns.GridColumn colPlanillaNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraGrid.Columns.GridColumn colDireccion;
        private DevExpress.XtraGrid.Columns.GridColumn colTelefono;
        private DevExpress.XtraGrid.Columns.GridColumn colCelular;
        private DevExpress.XtraGrid.Columns.GridColumn colEmail;
        private DevExpress.XtraGrid.Columns.GridColumn colGenero;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaNacimiento;
        private DevExpress.XtraGrid.Columns.GridColumn colCedula;
        private DevExpress.XtraGrid.Columns.GridColumn colProfesionOficioNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroNominaInss;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroSeguroSocial;
        private DevExpress.XtraGrid.Columns.GridColumn colSalarioActual;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaContrato;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaIngreso;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaSalida;
        private DevExpress.XtraGrid.Columns.GridColumn colCargoNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colAreaNominaNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoSangre;
        private DevExpress.XtraGrid.Columns.GridColumn colPariente;
        private DevExpress.XtraGrid.Columns.GridColumn colPagaPension;
        private DevExpress.XtraGrid.Columns.GridColumn colMontoPension;
        private DevExpress.XtraGrid.Columns.GridColumn colEsPorcentaje;
        private DevExpress.XtraGrid.Columns.GridColumn colDiferenteDeTarjeta;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroCuentaTarjeta;
        private DevExpress.XtraGrid.Columns.GridColumn colCalculoSalarioPromedioAguinaldo;
        private DevExpress.XtraGrid.Columns.GridColumn colEstadoCivilNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colEstructuraOrganizativaNombre;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboGenero;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkGenero;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}

