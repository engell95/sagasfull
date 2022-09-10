namespace SAGAS.Contabilidad.Forms
{
    partial class FormCuentaContable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCuentaContable));
            DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition();
            this.colPorcentaje = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeCC = new DevExpress.XtraTreeList.TreeList();
            this.colID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colIDPadre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCodigo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNombre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNivel = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colIDTipoCuenta = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lkIDTipoCuenta = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colDetalle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colModular = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEsDolar = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEsImpuesto = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colComentario = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colLimite = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colActivo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridES = new DevExpress.XtraGrid.GridControl();
            this.gvDataES = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkIDTipoCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // barExportar
            // 
            this.barExportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barExportar.ImageOptions.Image")));
            // 
            // colPorcentaje
            // 
            this.colPorcentaje.Caption = "Porcentaje";
            this.colPorcentaje.FieldName = "Porcentaje";
            this.colPorcentaje.Name = "colPorcentaje";
            this.colPorcentaje.Visible = true;
            this.colPorcentaje.VisibleIndex = 8;
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
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 33);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.treeCC);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridES);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(804, 521);
            this.splitContainerControl1.SplitterPosition = 556;
            this.splitContainerControl1.TabIndex = 5;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // treeCC
            // 
            this.treeCC.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colID,
            this.colIDPadre,
            this.colCodigo,
            this.colNombre,
            this.colNivel,
            this.colIDTipoCuenta,
            this.colDetalle,
            this.colModular,
            this.colEsDolar,
            this.colEsImpuesto,
            this.colPorcentaje,
            this.colComentario,
            this.colLimite,
            this.treeListColumn1,
            this.colActivo,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4});
            this.treeCC.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeCC.Dock = System.Windows.Forms.DockStyle.Fill;
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.White;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.Column = this.colPorcentaje;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.LessOrEqual;
            styleFormatCondition1.Value1 = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.treeCC.FormatConditions.AddRange(new DevExpress.XtraTreeList.StyleFormatConditions.StyleFormatCondition[] {
            styleFormatCondition1});
            this.treeCC.Location = new System.Drawing.Point(0, 0);
            this.treeCC.Name = "treeCC";
            this.treeCC.OptionsBehavior.Editable = false;
            this.treeCC.OptionsBehavior.PopulateServiceColumns = true;
            this.treeCC.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Smart;
            this.treeCC.OptionsFilter.ShowAllValuesInFilterPopup = true;
            this.treeCC.OptionsNavigation.AutoMoveRowFocus = true;
            this.treeCC.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeCC.OptionsView.AutoWidth = false;
            this.treeCC.OptionsView.EnableAppearanceEvenRow = true;
            this.treeCC.OptionsView.EnableAppearanceOddRow = true;
            this.treeCC.OptionsView.ShowAutoFilterRow = true;
            this.treeCC.OptionsView.ShowHorzLines = false;
            this.treeCC.ParentFieldName = "IDPadre";
            this.treeCC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkIDTipoCuenta,
            this.rchkActivo});
            this.treeCC.Size = new System.Drawing.Size(556, 521);
            this.treeCC.TabIndex = 4;
            this.treeCC.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeCC_FocusedNodeChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colIDPadre
            // 
            this.colIDPadre.FieldName = "IDPadre";
            this.colIDPadre.Name = "colIDPadre";
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Codigo";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            this.colCodigo.Width = 164;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 1;
            this.colNombre.Width = 141;
            // 
            // colNivel
            // 
            this.colNivel.AppearanceCell.Options.UseTextOptions = true;
            this.colNivel.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colNivel.Caption = "Nivel";
            this.colNivel.FieldName = "Nivel";
            this.colNivel.Name = "colNivel";
            this.colNivel.Visible = true;
            this.colNivel.VisibleIndex = 2;
            this.colNivel.Width = 45;
            // 
            // colIDTipoCuenta
            // 
            this.colIDTipoCuenta.Caption = "Tipo de Cuenta";
            this.colIDTipoCuenta.ColumnEdit = this.lkIDTipoCuenta;
            this.colIDTipoCuenta.FieldName = "IDTipoCuenta";
            this.colIDTipoCuenta.Name = "colIDTipoCuenta";
            this.colIDTipoCuenta.Visible = true;
            this.colIDTipoCuenta.VisibleIndex = 3;
            this.colIDTipoCuenta.Width = 97;
            // 
            // lkIDTipoCuenta
            // 
            this.lkIDTipoCuenta.AutoHeight = false;
            this.lkIDTipoCuenta.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkIDTipoCuenta.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipos de Cuentas")});
            this.lkIDTipoCuenta.Name = "lkIDTipoCuenta";
            // 
            // colDetalle
            // 
            this.colDetalle.Caption = "Es Detalle";
            this.colDetalle.FieldName = "Detalle";
            this.colDetalle.Name = "colDetalle";
            this.colDetalle.Visible = true;
            this.colDetalle.VisibleIndex = 4;
            this.colDetalle.Width = 71;
            // 
            // colModular
            // 
            this.colModular.Caption = "Es Modular";
            this.colModular.FieldName = "Modular";
            this.colModular.Name = "colModular";
            this.colModular.Visible = true;
            this.colModular.VisibleIndex = 5;
            this.colModular.Width = 72;
            // 
            // colEsDolar
            // 
            this.colEsDolar.Caption = "Es Dolar";
            this.colEsDolar.FieldName = "EsDolar";
            this.colEsDolar.Name = "colEsDolar";
            this.colEsDolar.SortOrder = System.Windows.Forms.SortOrder.Descending;
            this.colEsDolar.Visible = true;
            this.colEsDolar.VisibleIndex = 6;
            this.colEsDolar.Width = 60;
            // 
            // colEsImpuesto
            // 
            this.colEsImpuesto.Caption = "EsImpuesto";
            this.colEsImpuesto.FieldName = "EsImpuesto";
            this.colEsImpuesto.Name = "colEsImpuesto";
            this.colEsImpuesto.Visible = true;
            this.colEsImpuesto.VisibleIndex = 7;
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 10;
            this.colComentario.Width = 238;
            // 
            // colLimite
            // 
            this.colLimite.Caption = "Límite";
            this.colLimite.FieldName = "Limite";
            this.colLimite.Name = "colLimite";
            this.colLimite.Visible = true;
            this.colLimite.VisibleIndex = 9;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "Libros";
            this.treeListColumn1.FieldName = "AgrupacionLibros";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 11;
            this.treeListColumn1.Width = 153;
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
            this.colActivo.VisibleIndex = 12;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "Tipo";
            this.treeListColumn2.FieldName = "Tipo";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 13;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "Tipo 2";
            this.treeListColumn3.FieldName = "Tipo2";
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 14;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "Tipo 3";
            this.treeListColumn4.FieldName = "Tipo3";
            this.treeListColumn4.Name = "treeListColumn4";
            this.treeListColumn4.Visible = true;
            this.treeListColumn4.VisibleIndex = 15;
            // 
            // gridES
            // 
            this.gridES.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridES.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridES.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridES.Location = new System.Drawing.Point(0, 0);
            this.gridES.MainView = this.gvDataES;
            this.gridES.MenuManager = this.barManager;
            this.gridES.Name = "gridES";
            this.gridES.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.gridES.Size = new System.Drawing.Size(244, 521);
            this.gridES.TabIndex = 7;
            this.gridES.UseEmbeddedNavigator = true;
            this.gridES.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataES});
            // 
            // gvDataES
            // 
            this.gvDataES.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDES,
            this.colNombreES});
            this.gvDataES.GridControl = this.gridES;
            this.gvDataES.Name = "gvDataES";
            this.gvDataES.OptionsBehavior.Editable = false;
            this.gvDataES.OptionsMenu.EnableColumnMenu = false;
            this.gvDataES.OptionsMenu.EnableFooterMenu = false;
            this.gvDataES.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataES.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataES.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataES.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDataES.OptionsView.ColumnAutoWidth = false;
            this.gvDataES.OptionsView.ShowGroupPanel = false;
            // 
            // colIDES
            // 
            this.colIDES.FieldName = "EstacionServicioID";
            this.colIDES.Name = "colIDES";
            // 
            // colNombreES
            // 
            this.colNombreES.Caption = "Estaciones de Servicio";
            this.colNombreES.FieldName = "Nombre";
            this.colNombreES.Name = "colNombreES";
            this.colNombreES.Visible = true;
            this.colNombreES.VisibleIndex = 0;
            this.colNombreES.Width = 157;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Empresa")});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "<No tiene Empresa Asignada>";
            // 
            // FormCuentaContable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormCuentaContable";
            this.Text = "Catálogo de Cuentas";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkIDTipoCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridES;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataES;
        private DevExpress.XtraGrid.Columns.GridColumn colIDES;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraTreeList.TreeList treeCC;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colIDPadre;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCodigo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNombre;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNivel;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colIDTipoCuenta;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkIDTipoCuenta;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDetalle;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colModular;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colEsDolar;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colEsImpuesto;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPorcentaje;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colComentario;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colLimite;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
    }
}

