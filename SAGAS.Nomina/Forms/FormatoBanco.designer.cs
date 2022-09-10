namespace SAGAS.Nomina.Forms
{
    partial class FormatoBanco
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormatoBanco));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEmpleado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNoCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.progressBarCont = new DevExpress.XtraEditors.ProgressBarControl();
            this.chkCaracter = new DevExpress.XtraEditors.CheckEdit();
            this.btnToTxt = new DevExpress.XtraEditors.SimpleButton();
            this.txtSimbolo = new DevExpress.XtraEditors.TextEdit();
            this.btnToExecl = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptyLabel = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarCont.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCaracter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSimbolo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptyLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
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
            this.grid.Location = new System.Drawing.Point(0, 59);
            this.grid.MainView = this.gvData;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrEstacionServicio});
            this.grid.Size = new System.Drawing.Size(804, 265);
            this.grid.TabIndex = 6;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEmpleado,
            this.colNoCuenta,
            this.colConcepto,
            this.colTotal});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ShowFooter = true;
            // 
            // colEmpleado
            // 
            this.colEmpleado.FieldName = "NombreCompleto";
            this.colEmpleado.Name = "colEmpleado";
            this.colEmpleado.Visible = true;
            this.colEmpleado.VisibleIndex = 0;
            this.colEmpleado.Width = 226;
            // 
            // colNoCuenta
            // 
            this.colNoCuenta.Caption = "Nro. Cuenta";
            this.colNoCuenta.FieldName = "NoCuenta";
            this.colNoCuenta.Name = "colNoCuenta";
            this.colNoCuenta.Visible = true;
            this.colNoCuenta.VisibleIndex = 1;
            this.colNoCuenta.Width = 202;
            // 
            // colConcepto
            // 
            this.colConcepto.Caption = "Concepto";
            this.colConcepto.FieldName = "Concepto";
            this.colConcepto.Name = "colConcepto";
            this.colConcepto.Visible = true;
            this.colConcepto.VisibleIndex = 2;
            this.colConcepto.Width = 223;
            // 
            // colTotal
            // 
            this.colTotal.AppearanceCell.Options.UseTextOptions = true;
            this.colTotal.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.colTotal.Caption = "Total";
            this.colTotal.DisplayFormat.FormatString = "{0:0.##}";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:N2}")});
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 3;
            this.colTotal.Width = 134;
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
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.progressBarCont);
            this.layoutControl1.Controls.Add(this.chkCaracter);
            this.layoutControl1.Controls.Add(this.btnToTxt);
            this.layoutControl1.Controls.Add(this.txtSimbolo);
            this.layoutControl1.Controls.Add(this.btnToExecl);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(804, 59);
            this.layoutControl1.TabIndex = 7;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // progressBarCont
            // 
            this.progressBarCont.Location = new System.Drawing.Point(467, 30);
            this.progressBarCont.Name = "progressBarCont";
            this.progressBarCont.Properties.ShowTitle = true;
            this.progressBarCont.Properties.Step = 1;
            this.progressBarCont.Size = new System.Drawing.Size(332, 18);
            this.progressBarCont.StyleController = this.layoutControl1;
            this.progressBarCont.TabIndex = 8;
            // 
            // chkCaracter
            // 
            this.chkCaracter.Location = new System.Drawing.Point(218, 30);
            this.chkCaracter.Name = "chkCaracter";
            this.chkCaracter.Properties.Caption = "Separador con símbolos";
            this.chkCaracter.Size = new System.Drawing.Size(140, 19);
            this.chkCaracter.StyleController = this.layoutControl1;
            this.chkCaracter.TabIndex = 8;
            this.chkCaracter.CheckedChanged += new System.EventHandler(this.chkCaracter_CheckedChanged);
            // 
            // btnToTxt
            // 
            this.btnToTxt.Image = ((System.Drawing.Image)(resources.GetObject("btnToTxt.Image")));
            this.btnToTxt.Location = new System.Drawing.Point(112, 27);
            this.btnToTxt.Name = "btnToTxt";
            this.btnToTxt.Size = new System.Drawing.Size(99, 30);
            this.btnToTxt.StyleController = this.layoutControl1;
            this.btnToTxt.TabIndex = 8;
            this.btnToTxt.Text = "Formato TXT";
            this.btnToTxt.Click += new System.EventHandler(this.btnToTxt_Click);
            // 
            // txtSimbolo
            // 
            this.txtSimbolo.EditValue = "|";
            this.txtSimbolo.Enabled = false;
            this.txtSimbolo.Location = new System.Drawing.Point(407, 30);
            this.txtSimbolo.Name = "txtSimbolo";
            this.txtSimbolo.Size = new System.Drawing.Size(50, 20);
            this.txtSimbolo.StyleController = this.layoutControl1;
            this.txtSimbolo.TabIndex = 5;
            // 
            // btnToExecl
            // 
            this.btnToExecl.Image = ((System.Drawing.Image)(resources.GetObject("btnToExecl.Image")));
            this.btnToExecl.Location = new System.Drawing.Point(2, 27);
            this.btnToExecl.Name = "btnToExecl";
            this.btnToExecl.Size = new System.Drawing.Size(106, 30);
            this.btnToExecl.StyleController = this.layoutControl1;
            this.btnToExecl.TabIndex = 4;
            this.btnToExecl.Text = "Formato Excel";
            this.btnToExecl.Click += new System.EventHandler(this.btnToExecl_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptyLabel,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(804, 59);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnToExecl;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 25);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(110, 34);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtSimbolo;
            this.layoutControlItem2.CustomizationFormText = "Símbolo";
            this.layoutControlItem2.Location = new System.Drawing.Point(363, 25);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem2.Size = new System.Drawing.Size(99, 34);
            this.layoutControlItem2.Text = "Símbolo";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(36, 13);
            // 
            // emptyLabel
            // 
            this.emptyLabel.AllowHotTrack = false;
            this.emptyLabel.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.emptyLabel.AppearanceItemCaption.Options.UseFont = true;
            this.emptyLabel.AppearanceItemCaption.Options.UseTextOptions = true;
            this.emptyLabel.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.emptyLabel.CustomizationFormText = "<>";
            this.emptyLabel.Location = new System.Drawing.Point(0, 0);
            this.emptyLabel.Name = "emptyLabel";
            this.emptyLabel.Size = new System.Drawing.Size(804, 25);
            this.emptyLabel.Text = "<>";
            this.emptyLabel.TextSize = new System.Drawing.Size(36, 0);
            this.emptyLabel.TextVisible = true;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.btnToTxt;
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(110, 25);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(103, 34);
            this.layoutControlItem3.Text = "layoutControlItem3";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.chkCaracter;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
            this.layoutControlItem4.Location = new System.Drawing.Point(213, 25);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem4.Size = new System.Drawing.Size(150, 34);
            this.layoutControlItem4.Text = "layoutControlItem4";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.progressBarCont;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(462, 25);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItem5.Size = new System.Drawing.Size(342, 34);
            this.layoutControlItem5.Text = "layoutControlItem5";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextToControlDistance = 0;
            this.layoutControlItem5.TextVisible = false;
            // 
            // FormatoBanco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 324);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.layoutControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormatoBanco";
            this.Text = "Formato Exportable Banco";
            this.Load += new System.EventHandler(this.FormZona_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarCont.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCaracter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSimbolo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptyLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colEmpleado;
        private DevExpress.XtraGrid.Columns.GridColumn colNoCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn colConcepto;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrEstacionServicio;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit txtSimbolo;
        private DevExpress.XtraEditors.SimpleButton btnToExecl;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        public DevExpress.XtraLayout.EmptySpaceItem emptyLabel;
        private DevExpress.XtraEditors.SimpleButton btnToTxt;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.CheckEdit chkCaracter;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.ProgressBarControl progressBarCont;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}

