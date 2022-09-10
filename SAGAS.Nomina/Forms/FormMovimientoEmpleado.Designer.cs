namespace SAGAS.Nomina.Forms
{
    partial class FormMovimientoEmpleado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMovimientoEmpleado));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.colID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPlanilla = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEmpleadoCodigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEmpleado = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colNombre = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDescripcion = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colFechaInicial = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colFechaFinal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEsIngreso = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.rchkEsIngreso = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colCantidadCuotas = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCantidad = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCantidadDias = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colMontoTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.chkEsIngreso = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.chkAplicaRetencion = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBandDatosGenerales = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBandDeducciones = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBandHorasExtras = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBandAusenciaSubsidio = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBandTotal = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkEsIngreso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEsIngreso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaRetencion)).BeginInit();
            this.SuspendLayout();
            // 
            // barTop
            // 
            this.barTop.OptionsBar.AllowQuickCustomization = false;
            this.barTop.OptionsBar.DrawDragBorder = false;
            this.barTop.OptionsBar.UseWholeRow = true;
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
            this.grid.Location = new System.Drawing.Point(0, 33);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkEsIngreso,
            this.chkAplicaRetencion,
            this.rchkEsIngreso});
            this.grid.Size = new System.Drawing.Size(1051, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBandDatosGenerales,
            this.gridBandDeducciones,
            this.gridBandHorasExtras,
            this.gridBandAusenciaSubsidio,
            this.gridBandTotal});
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colID,
            this.colPlanilla,
            this.colEmpleadoCodigo,
            this.colEmpleado,
            this.colNombre,
            this.colEsIngreso,
            this.colDescripcion,
            this.colFechaInicial,
            this.colFechaFinal,
            this.colCantidadDias,
            this.colCantidad,
            this.colMontoTotal,
            this.colCantidadCuotas,
            this.bandedGridColumn1});
            this.gvData.GridControl = this.grid;
            this.gvData.GroupCount = 1;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colPlanilla, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFechaFinal, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.OptionsColumn.AllowEdit = false;
            this.colID.Visible = true;
            this.colID.Width = 53;
            // 
            // colPlanilla
            // 
            this.colPlanilla.Caption = "Planilla";
            this.colPlanilla.FieldName = "PlanillaNombre";
            this.colPlanilla.Name = "colPlanilla";
            this.colPlanilla.OptionsColumn.AllowEdit = false;
            this.colPlanilla.Visible = true;
            this.colPlanilla.Width = 127;
            // 
            // colEmpleadoCodigo
            // 
            this.colEmpleadoCodigo.Caption = "Código Empleado";
            this.colEmpleadoCodigo.FieldName = "EmpleadoCodigo";
            this.colEmpleadoCodigo.Name = "colEmpleadoCodigo";
            this.colEmpleadoCodigo.OptionsColumn.AllowEdit = false;
            this.colEmpleadoCodigo.Visible = true;
            this.colEmpleadoCodigo.Width = 91;
            // 
            // colEmpleado
            // 
            this.colEmpleado.Caption = "Empleado";
            this.colEmpleado.FieldName = "EmpleadoNombreCompleto";
            this.colEmpleado.Name = "colEmpleado";
            this.colEmpleado.OptionsColumn.AllowEdit = false;
            this.colEmpleado.Visible = true;
            this.colEmpleado.Width = 86;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Tipo Movimiento";
            this.colNombre.FieldName = "TipoMovimientoEmpleadoNombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.OptionsColumn.AllowEdit = false;
            this.colNombre.Visible = true;
            this.colNombre.Width = 93;
            // 
            // colDescripcion
            // 
            this.colDescripcion.Caption = "Descripción";
            this.colDescripcion.FieldName = "Descripcion";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.OptionsColumn.AllowEdit = false;
            this.colDescripcion.Visible = true;
            this.colDescripcion.Width = 86;
            // 
            // colFechaInicial
            // 
            this.colFechaInicial.Caption = "Fecha Inicial";
            this.colFechaInicial.FieldName = "FechaInicial";
            this.colFechaInicial.Name = "colFechaInicial";
            this.colFechaInicial.OptionsColumn.AllowEdit = false;
            this.colFechaInicial.Visible = true;
            this.colFechaInicial.Width = 69;
            // 
            // colFechaFinal
            // 
            this.colFechaFinal.Caption = "Fecha Final";
            this.colFechaFinal.FieldName = "FechaFinal";
            this.colFechaFinal.Name = "colFechaFinal";
            this.colFechaFinal.OptionsColumn.AllowEdit = false;
            this.colFechaFinal.Visible = true;
            this.colFechaFinal.Width = 78;
            // 
            // colEsIngreso
            // 
            this.colEsIngreso.Caption = "Es Ingreso";
            this.colEsIngreso.ColumnEdit = this.rchkEsIngreso;
            this.colEsIngreso.FieldName = "EsIngreso";
            this.colEsIngreso.Name = "colEsIngreso";
            this.colEsIngreso.OptionsColumn.AllowEdit = false;
            this.colEsIngreso.Visible = true;
            // 
            // rchkEsIngreso
            // 
            this.rchkEsIngreso.AllowFocused = false;
            this.rchkEsIngreso.AutoHeight = false;
            this.rchkEsIngreso.Caption = "Check";
            this.rchkEsIngreso.Name = "rchkEsIngreso";
            this.rchkEsIngreso.ReadOnly = true;
            // 
            // colCantidadCuotas
            // 
            this.colCantidadCuotas.Caption = "Cantidad Cuotas";
            this.colCantidadCuotas.FieldName = "CantidadCuotas";
            this.colCantidadCuotas.Name = "colCantidadCuotas";
            this.colCantidadCuotas.OptionsColumn.AllowEdit = false;
            this.colCantidadCuotas.Visible = true;
            this.colCantidadCuotas.Width = 91;
            // 
            // colCantidad
            // 
            this.colCantidad.Caption = "Cantidad H.E.";
            this.colCantidad.FieldName = "CantidadHorasExtras";
            this.colCantidad.Name = "colCantidad";
            this.colCantidad.OptionsColumn.AllowEdit = false;
            this.colCantidad.Visible = true;
            this.colCantidad.Width = 82;
            // 
            // colCantidadDias
            // 
            this.colCantidadDias.Caption = "Cantidad de días";
            this.colCantidadDias.FieldName = "CantidadDiasAusencia";
            this.colCantidadDias.Name = "colCantidadDias";
            this.colCantidadDias.OptionsColumn.AllowEdit = false;
            this.colCantidadDias.Visible = true;
            this.colCantidadDias.Width = 101;
            // 
            // colMontoTotal
            // 
            this.colMontoTotal.Caption = "Monto Total";
            this.colMontoTotal.FieldName = "MontoTotal";
            this.colMontoTotal.Name = "colMontoTotal";
            this.colMontoTotal.OptionsColumn.AllowEdit = false;
            this.colMontoTotal.Visible = true;
            this.colMontoTotal.Width = 66;
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
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.Caption = "Monto Cuota";
            this.bandedGridColumn1.DisplayFormat.FormatString = "n2";
            this.bandedGridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.bandedGridColumn1.FieldName = "MontoCuota";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            // 
            // gridBandDatosGenerales
            // 
            this.gridBandDatosGenerales.Caption = "Datos Generales";
            this.gridBandDatosGenerales.Columns.Add(this.colID);
            this.gridBandDatosGenerales.Columns.Add(this.colPlanilla);
            this.gridBandDatosGenerales.Columns.Add(this.colEmpleadoCodigo);
            this.gridBandDatosGenerales.Columns.Add(this.colEmpleado);
            this.gridBandDatosGenerales.Columns.Add(this.colNombre);
            this.gridBandDatosGenerales.Columns.Add(this.colDescripcion);
            this.gridBandDatosGenerales.Columns.Add(this.colFechaInicial);
            this.gridBandDatosGenerales.Columns.Add(this.colFechaFinal);
            this.gridBandDatosGenerales.Columns.Add(this.colEsIngreso);
            this.gridBandDatosGenerales.Name = "gridBandDatosGenerales";
            this.gridBandDatosGenerales.VisibleIndex = 0;
            this.gridBandDatosGenerales.Width = 758;
            // 
            // gridBandDeducciones
            // 
            this.gridBandDeducciones.Caption = "Deducciones / Ingresos";
            this.gridBandDeducciones.Columns.Add(this.colCantidadCuotas);
            this.gridBandDeducciones.Columns.Add(this.bandedGridColumn1);
            this.gridBandDeducciones.Name = "gridBandDeducciones";
            this.gridBandDeducciones.VisibleIndex = 1;
            this.gridBandDeducciones.Width = 166;
            // 
            // gridBandHorasExtras
            // 
            this.gridBandHorasExtras.Caption = "Horas Extras";
            this.gridBandHorasExtras.Columns.Add(this.colCantidad);
            this.gridBandHorasExtras.Name = "gridBandHorasExtras";
            this.gridBandHorasExtras.VisibleIndex = 2;
            this.gridBandHorasExtras.Width = 82;
            // 
            // gridBandAusenciaSubsidio
            // 
            this.gridBandAusenciaSubsidio.Caption = "Ausencia / Subsidio";
            this.gridBandAusenciaSubsidio.Columns.Add(this.colCantidadDias);
            this.gridBandAusenciaSubsidio.Name = "gridBandAusenciaSubsidio";
            this.gridBandAusenciaSubsidio.VisibleIndex = 3;
            this.gridBandAusenciaSubsidio.Width = 101;
            // 
            // gridBandTotal
            // 
            this.gridBandTotal.Caption = "Total";
            this.gridBandTotal.Columns.Add(this.colMontoTotal);
            this.gridBandTotal.Name = "gridBandTotal";
            this.gridBandTotal.VisibleIndex = 4;
            this.gridBandTotal.Width = 66;
            // 
            // FormMovimientoEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormMovimientoEmpleado";
            this.Text = "Lista de Movimientos de Empleados";
            this.Load += new System.EventHandler(this.FormSucursal_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkEsIngreso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEsIngreso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAplicaRetencion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkEsIngreso;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkAplicaRetencion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvData;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colID;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPlanilla;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEmpleadoCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEmpleado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNombre;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDescripcion;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colFechaInicial;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colFechaFinal;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCantidadCuotas;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCantidad;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMontoTotal;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEsIngreso;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCantidadDias;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkEsIngreso;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandDatosGenerales;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandDeducciones;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandHorasExtras;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandAusenciaSubsidio;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandTotal;
    }
}

