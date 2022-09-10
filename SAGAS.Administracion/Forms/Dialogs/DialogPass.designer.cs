namespace SAGAS.Administracion.Forms.Dialogs
{
    partial class DialogPass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPass));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.lkES = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtAnterior = new DevExpress.XtraEditors.TextEdit();
            this.txtConfirmar = new DevExpress.XtraEditors.TextEdit();
            this.txtNueva = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemNueva = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemConfirmar = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAnterior = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkES.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnterior.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNueva.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNueva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemConfirmar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAnterior)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.lkES);
            this.layoutControl1.Controls.Add(this.txtAnterior);
            this.layoutControl1.Controls.Add(this.txtConfirmar);
            this.layoutControl1.Controls.Add(this.txtNueva);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup;
            this.layoutControl1.Size = new System.Drawing.Size(406, 145);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // lkES
            // 
            this.lkES.EditValue = "<Seleccione la Estación de Servicio>";
            this.lkES.Location = new System.Drawing.Point(127, 103);
            this.lkES.Name = "lkES";
            this.lkES.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkES.Properties.NullText = "<Seleccione la Estación de Servicio>";
            this.lkES.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.lkES.Properties.View = this.gridLookUpEdit1View;
            this.lkES.Size = new System.Drawing.Size(267, 20);
            this.lkES.StyleController = this.layoutControl1;
            this.lkES.TabIndex = 2;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombre});
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsBehavior.Editable = false;
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowAutoFilterRow = true;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            this.gridLookUpEdit1View.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCodigo, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Codigo";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            this.colCodigo.Width = 247;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Estación de Servicio";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 1;
            this.colNombre.Width = 541;
            // 
            // txtAnterior
            // 
            this.txtAnterior.EditValue = "";
            this.txtAnterior.Location = new System.Drawing.Point(127, 31);
            this.txtAnterior.Name = "txtAnterior";
            this.txtAnterior.Properties.PasswordChar = '•';
            this.txtAnterior.Size = new System.Drawing.Size(267, 20);
            this.txtAnterior.StyleController = this.layoutControl1;
            this.txtAnterior.TabIndex = 3;
            this.txtAnterior.Validated += new System.EventHandler(this.txtAnterior_Validated);
            // 
            // txtConfirmar
            // 
            this.txtConfirmar.EditValue = "";
            this.txtConfirmar.Location = new System.Drawing.Point(127, 79);
            this.txtConfirmar.Name = "txtConfirmar";
            this.txtConfirmar.Properties.PasswordChar = '•';
            this.txtConfirmar.Size = new System.Drawing.Size(267, 20);
            this.txtConfirmar.StyleController = this.layoutControl1;
            this.txtConfirmar.TabIndex = 3;
            this.txtConfirmar.Validated += new System.EventHandler(this.txtAnterior_Validated);
            // 
            // txtNueva
            // 
            this.txtNueva.EditValue = "";
            this.txtNueva.Location = new System.Drawing.Point(127, 55);
            this.txtNueva.Name = "txtNueva";
            this.txtNueva.Properties.PasswordChar = '•';
            this.txtNueva.Size = new System.Drawing.Size(267, 20);
            this.txtNueva.StyleController = this.layoutControl1;
            this.txtNueva.TabIndex = 2;
            this.txtNueva.Validated += new System.EventHandler(this.txtAnterior_Validated);
            // 
            // layoutControlGroup
            // 
            this.layoutControlGroup.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemNueva,
            this.layoutControlItemConfirmar,
            this.layoutControlItemAnterior,
            this.layoutControlItem1});
            this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup.Name = "layoutControlGroup";
            this.layoutControlGroup.Size = new System.Drawing.Size(406, 145);
            this.layoutControlGroup.Text = "Contraseña";
            // 
            // layoutControlItemNueva
            // 
            this.layoutControlItemNueva.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemNueva.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemNueva.Control = this.txtNueva;
            this.layoutControlItemNueva.CustomizationFormText = "Nueva";
            this.layoutControlItemNueva.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemNueva.Name = "layoutControlItemNueva";
            this.layoutControlItemNueva.Size = new System.Drawing.Size(386, 24);
            this.layoutControlItemNueva.Text = "Nueva";
            this.layoutControlItemNueva.TextSize = new System.Drawing.Size(112, 13);
            // 
            // layoutControlItemConfirmar
            // 
            this.layoutControlItemConfirmar.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemConfirmar.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemConfirmar.Control = this.txtConfirmar;
            this.layoutControlItemConfirmar.CustomizationFormText = "Confirmar";
            this.layoutControlItemConfirmar.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItemConfirmar.Name = "layoutControlItemConfirmar";
            this.layoutControlItemConfirmar.Size = new System.Drawing.Size(386, 24);
            this.layoutControlItemConfirmar.Text = "Confirmar";
            this.layoutControlItemConfirmar.TextSize = new System.Drawing.Size(112, 13);
            // 
            // layoutControlItemAnterior
            // 
            this.layoutControlItemAnterior.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemAnterior.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemAnterior.Control = this.txtAnterior;
            this.layoutControlItemAnterior.CustomizationFormText = "Anterior";
            this.layoutControlItemAnterior.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAnterior.Name = "layoutControlItemAnterior";
            this.layoutControlItemAnterior.Size = new System.Drawing.Size(386, 24);
            this.layoutControlItemAnterior.Text = "Anterior";
            this.layoutControlItemAnterior.TextSize = new System.Drawing.Size(112, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.lkES;
            this.layoutControlItem1.CustomizationFormText = "Estación de Servicio";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(386, 34);
            this.layoutControlItem1.Text = "Estación de Servicio";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(112, 13);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(410, 162);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 147);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 162);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(410, 34);
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
            // DialogPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(410, 196);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkES.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnterior.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConfirmar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNueva.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNueva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemConfirmar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAnterior)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.TextEdit txtConfirmar;
        private DevExpress.XtraEditors.TextEdit txtNueva;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemNueva;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemConfirmar;
        private DevExpress.XtraEditors.TextEdit txtAnterior;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAnterior;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.GridLookUpEdit lkES;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}