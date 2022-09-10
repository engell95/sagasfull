namespace SAGAS.Nomina.Forms
{
    partial class FormEstruckOrgani
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEstruckOrgani));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.treeEstrukOrgani = new DevExpress.XtraTreeList.TreeList();
            this.colID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colIDPadre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEmpresaID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.rpLkEmpresa = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNombre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDescripcion = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colActivo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeEstrukOrgani)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEmpresa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
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
            // treeEstrukOrgani
            // 
            this.treeEstrukOrgani.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colID,
            this.colIDPadre,
            this.colEmpresaID,
            this.colNombre,
            this.colDescripcion,
            this.colActivo});
            this.treeEstrukOrgani.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeEstrukOrgani.Location = new System.Drawing.Point(0, 33);
            this.treeEstrukOrgani.Name = "treeEstrukOrgani";
            this.treeEstrukOrgani.OptionsBehavior.AutoMoveRowFocus = true;
            this.treeEstrukOrgani.OptionsBehavior.Editable = false;
            this.treeEstrukOrgani.OptionsBehavior.EnableFiltering = true;
            this.treeEstrukOrgani.OptionsBehavior.PopulateServiceColumns = true;
            this.treeEstrukOrgani.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
            this.treeEstrukOrgani.OptionsFilter.ShowAllValuesInFilterPopup = true;
            this.treeEstrukOrgani.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeEstrukOrgani.OptionsView.AutoWidth = false;
            this.treeEstrukOrgani.OptionsView.EnableAppearanceEvenRow = true;
            this.treeEstrukOrgani.OptionsView.ShowAutoFilterRow = true;
            this.treeEstrukOrgani.ParentFieldName = "PadreID";
            this.treeEstrukOrgani.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpLkEmpresa,
            this.rchkActivo});
            this.treeEstrukOrgani.Size = new System.Drawing.Size(804, 521);
            this.treeEstrukOrgani.TabIndex = 4;
            this.treeEstrukOrgani.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeEstrukOrgani_FocusedNodeChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colIDPadre
            // 
            this.colIDPadre.FieldName = "PadreID";
            this.colIDPadre.Name = "colIDPadre";
            // 
            // colEmpresaID
            // 
            this.colEmpresaID.Caption = "Empresa";
            this.colEmpresaID.ColumnEdit = this.rpLkEmpresa;
            this.colEmpresaID.FieldName = "EmpresaID";
            this.colEmpresaID.Name = "colEmpresaID";
            this.colEmpresaID.Visible = true;
            this.colEmpresaID.VisibleIndex = 2;
            this.colEmpresaID.Width = 197;
            // 
            // rpLkEmpresa
            // 
            this.rpLkEmpresa.AutoHeight = false;
            this.rpLkEmpresa.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpLkEmpresa.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Empresas")});
            this.rpLkEmpresa.DisplayMember = "Display";
            this.rpLkEmpresa.Name = "rpLkEmpresa";
            this.rpLkEmpresa.ValueMember = "ID";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 205;
            // 
            // colDescripcion
            // 
            this.colDescripcion.Caption = "Descripción";
            this.colDescripcion.FieldName = "Descripcion";
            this.colDescripcion.Name = "colDescripcion";
            this.colDescripcion.Visible = true;
            this.colDescripcion.VisibleIndex = 1;
            this.colDescripcion.Width = 303;
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
            this.colActivo.VisibleIndex = 3;
            this.colActivo.Width = 58;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // FormEstruckOrgani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.treeEstrukOrgani);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormEstruckOrgani";
            this.Text = "Lista de Estructura Organizativa";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.treeEstrukOrgani, 0);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeEstrukOrgani)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpLkEmpresa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraTreeList.TreeList treeEstrukOrgani;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colIDPadre;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colActivo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDescripcion;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colEmpresaID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNombre;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpLkEmpresa;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}

