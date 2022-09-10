namespace SAGAS.Administracion.Forms.Dialogs
{
    partial class DialogTipoCuentaModulo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTipoCuentaModulo));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.gridTipoCuentaModulo = new DevExpress.XtraGrid.GridControl();
            this.gVTipoCuenta = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colselect = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.bdsManejador = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTipoCuentaModulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gVTipoCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejador)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.gridTipoCuentaModulo);
            this.layoutControl1.Controls.Add(this.txtNombre);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(365, 353);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // gridTipoCuentaModulo
            // 
            this.gridTipoCuentaModulo.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridTipoCuentaModulo.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridTipoCuentaModulo.Location = new System.Drawing.Point(12, 55);
            this.gridTipoCuentaModulo.MainView = this.gVTipoCuenta;
            this.gridTipoCuentaModulo.Name = "gridTipoCuentaModulo";
            this.gridTipoCuentaModulo.Size = new System.Drawing.Size(341, 286);
            this.gridTipoCuentaModulo.TabIndex = 2;
            this.gridTipoCuentaModulo.UseEmbeddedNavigator = true;
            this.gridTipoCuentaModulo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gVTipoCuenta});
            // 
            // gVTipoCuenta
            // 
            this.gVTipoCuenta.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colselect,
            this.colNombre});
            this.gVTipoCuenta.GridControl = this.gridTipoCuentaModulo;
            this.gVTipoCuenta.Name = "gVTipoCuenta";
            this.gVTipoCuenta.OptionsView.ShowGroupPanel = false;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colselect
            // 
            this.colselect.Caption = "Seleccionar";
            this.colselect.FieldName = "Booleano";
            this.colselect.Name = "colselect";
            this.colselect.Visible = true;
            this.colselect.VisibleIndex = 0;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Display";
            this.colNombre.Name = "colNombre";
            this.colNombre.OptionsColumn.AllowEdit = false;
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 1;
            // 
            // txtNombre
            // 
            this.txtNombre.EditValue = "";
            this.txtNombre.Location = new System.Drawing.Point(62, 31);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Properties.AllowFocused = false;
            this.txtNombre.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.Khaki;
            this.txtNombre.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.txtNombre.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Black;
            this.txtNombre.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtNombre.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtNombre.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.txtNombre.Properties.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(291, 20);
            this.txtNombre.StyleController = this.layoutControl1;
            this.txtNombre.TabIndex = 0;
            this.txtNombre.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(365, 353);
            this.layoutControlGroup1.Text = "Datos";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.txtNombre;
            this.layoutControlItem1.CustomizationFormText = "Servidor";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(345, 24);
            this.layoutControlItem1.Text = "Nombre ";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(47, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gridTipoCuentaModulo;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(345, 290);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(369, 370);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 355);
            this.lblRequerido.Name = "lblRequerido";
            this.lblRequerido.Size = new System.Drawing.Size(255, 13);
            this.lblRequerido.TabIndex = 1;
            this.lblRequerido.Text = "Nota:  Los campos en negritas son requeridos";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 370);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(369, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Administracion.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(86, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.Image = global::SAGAS.Administracion.Properties.Resources.Ok20;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Guardar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // DialogTipoCuentaModulo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(369, 404);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogTipoCuentaModulo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTipoCuentaModulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gVTipoCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejador)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraGrid.GridControl gridTipoCuentaModulo;
        private DevExpress.XtraGrid.Views.Grid.GridView gVTipoCuenta;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        public DevExpress.XtraEditors.TextEdit txtNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colselect;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private System.Windows.Forms.BindingSource bdsManejador;
    }
}