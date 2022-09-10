namespace SAGAS.Nomina.Forms
{
    partial class FormTrasladoEmpleado
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTrasladoEmpleado));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.lkrEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gvData = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.Codigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Nombre = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Apellidos = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Area = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Cargo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.EstructuraOrganizativa = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.DatosEmpleado = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
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
            gridLevelNode1.RelationName = "Level1";
            this.grid.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrEstacionServicio});
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 6;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // lkrEstacionServicio
            // 
            this.lkrEstacionServicio.AutoHeight = false;
            this.lkrEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FechaFin", "Estación de Servicio")});
            this.lkrEstacionServicio.Name = "lkrEstacionServicio";
            this.lkrEstacionServicio.NullText = "<No tiene Estación de Servicio Asignada>";
            // 
            // gvData
            // 
            this.gvData.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.DatosEmpleado,
            this.gridBand2,
            this.gridBand3});
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.Codigo,
            this.Nombre,
            this.Apellidos,
            this.colID,
            this.Area,
            this.bandedGridColumn1,
            this.Cargo,
            this.EstructuraOrganizativa,
            this.bandedGridColumn2,
            this.bandedGridColumn3,
            this.bandedGridColumn4,
            this.bandedGridColumn5,
            this.bandedGridColumn6,
            this.bandedGridColumn7});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            // 
            // Codigo
            // 
            this.Codigo.Caption = "Codigo";
            this.Codigo.FieldName = "Codigo";
            this.Codigo.Name = "Codigo";
            this.Codigo.Visible = true;
            this.Codigo.Width = 70;
            // 
            // Nombre
            // 
            this.Nombre.Caption = "Nombre";
            this.Nombre.FieldName = "Nombres";
            this.Nombre.Name = "Nombre";
            this.Nombre.Visible = true;
            this.Nombre.Width = 98;
            // 
            // Apellidos
            // 
            this.Apellidos.Caption = "Apellidos";
            this.Apellidos.FieldName = "Apellidos";
            this.Apellidos.Name = "Apellidos";
            this.Apellidos.Visible = true;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // Area
            // 
            this.Area.Caption = "Area";
            this.Area.FieldName = "AreaAnteriorNominaNombre";
            this.Area.Name = "Area";
            this.Area.Visible = true;
            this.Area.Width = 86;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.Caption = "Area";
            this.bandedGridColumn1.FieldName = "AreaNominaNuevaNombre";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            // 
            // Cargo
            // 
            this.Cargo.Caption = "Cargo";
            this.Cargo.FieldName = "CargoAnteriorNombre";
            this.Cargo.Name = "Cargo";
            this.Cargo.Visible = true;
            this.Cargo.Width = 92;
            // 
            // EstructuraOrganizativa
            // 
            this.EstructuraOrganizativa.Caption = "Estructura Organizativa";
            this.EstructuraOrganizativa.FieldName = "EstructuraOrganizativaNombre";
            this.EstructuraOrganizativa.Name = "EstructuraOrganizativa";
            this.EstructuraOrganizativa.Visible = true;
            this.EstructuraOrganizativa.Width = 148;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.Caption = "Planilla";
            this.bandedGridColumn2.FieldName = "PlanillaAnteriorNombre";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.Width = 83;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.Caption = "Salario";
            this.bandedGridColumn3.DisplayFormat.FormatString = "N2";
            this.bandedGridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.bandedGridColumn3.FieldName = "SalarioAnterior";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.Visible = true;
            // 
            // bandedGridColumn4
            // 
            this.bandedGridColumn4.Caption = "Cargo";
            this.bandedGridColumn4.FieldName = "CargoNuevoNombre";
            this.bandedGridColumn4.Name = "bandedGridColumn4";
            this.bandedGridColumn4.Visible = true;
            this.bandedGridColumn4.Width = 77;
            // 
            // bandedGridColumn5
            // 
            this.bandedGridColumn5.Caption = "Estructura Organizativa";
            this.bandedGridColumn5.FieldName = "EstructuraOrganizativaNuevaNombre";
            this.bandedGridColumn5.Name = "bandedGridColumn5";
            this.bandedGridColumn5.Visible = true;
            this.bandedGridColumn5.Width = 159;
            // 
            // bandedGridColumn6
            // 
            this.bandedGridColumn6.Caption = "Planilla";
            this.bandedGridColumn6.FieldName = "PlanillaNuevaNombre";
            this.bandedGridColumn6.Name = "bandedGridColumn6";
            this.bandedGridColumn6.Visible = true;
            // 
            // bandedGridColumn7
            // 
            this.bandedGridColumn7.Caption = "Salario";
            this.bandedGridColumn7.DisplayFormat.FormatString = "N2";
            this.bandedGridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.bandedGridColumn7.FieldName = "SalarioNuevo";
            this.bandedGridColumn7.Name = "bandedGridColumn7";
            this.bandedGridColumn7.Visible = true;
            // 
            // DatosEmpleado
            // 
            this.DatosEmpleado.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DatosEmpleado.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.DatosEmpleado.AppearanceHeader.Options.UseBackColor = true;
            this.DatosEmpleado.AppearanceHeader.Options.UseFont = true;
            this.DatosEmpleado.AppearanceHeader.Options.UseTextOptions = true;
            this.DatosEmpleado.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.DatosEmpleado.Caption = "Datos del Empleado";
            this.DatosEmpleado.Columns.Add(this.Codigo);
            this.DatosEmpleado.Columns.Add(this.Nombre);
            this.DatosEmpleado.Columns.Add(this.Apellidos);
            this.DatosEmpleado.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.DatosEmpleado.Name = "DatosEmpleado";
            this.DatosEmpleado.VisibleIndex = 0;
            this.DatosEmpleado.Width = 243;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "Datos Anteriores";
            this.gridBand2.Columns.Add(this.Area);
            this.gridBand2.Columns.Add(this.Cargo);
            this.gridBand2.Columns.Add(this.EstructuraOrganizativa);
            this.gridBand2.Columns.Add(this.bandedGridColumn2);
            this.gridBand2.Columns.Add(this.bandedGridColumn3);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 484;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridBand3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridBand3.AppearanceHeader.Options.UseBackColor = true;
            this.gridBand3.AppearanceHeader.Options.UseFont = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "Datos Nuevos";
            this.gridBand3.Columns.Add(this.bandedGridColumn1);
            this.gridBand3.Columns.Add(this.bandedGridColumn4);
            this.gridBand3.Columns.Add(this.bandedGridColumn5);
            this.gridBand3.Columns.Add(this.bandedGridColumn6);
            this.gridBand3.Columns.Add(this.bandedGridColumn7);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 461;
            // 
            // FormTrasladoEmpleado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTrasladoEmpleado";
            this.Text = "Traslado Empleado";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrEstacionServicio;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvData;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand DatosEmpleado;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Codigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Nombre;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Apellidos;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Area;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn Cargo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn EstructuraOrganizativa;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn5;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn6;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn7;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colID;
    }
}

