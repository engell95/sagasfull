namespace SAGAS.Nomina.Forms.Dialogs
{
    partial class DialogCargo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCargo));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.mmoDescripcion = new DevExpress.XtraEditors.MemoEdit();
            this.cargoBindingSource = new System.Windows.Forms.BindingSource();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.spOrden = new DevExpress.XtraEditors.SpinEdit();
            this.EsBomberoCheckEdit = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForDescripcion = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForOrden = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEsBombero = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cargoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spOrden.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EsBomberoCheckEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescripcion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForOrden)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEsBombero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(393, 151);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.dataLayoutControl1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(389, 134);
            this.panelControl3.TabIndex = 2;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.mmoDescripcion);
            this.dataLayoutControl1.Controls.Add(this.txtNombre);
            this.dataLayoutControl1.Controls.Add(this.spOrden);
            this.dataLayoutControl1.Controls.Add(this.EsBomberoCheckEdit);
            this.dataLayoutControl1.DataSource = this.cargoBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(2, 2);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.Root = this.layoutControlGroup1;
            this.dataLayoutControl1.Size = new System.Drawing.Size(385, 130);
            this.dataLayoutControl1.TabIndex = 0;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // mmoDescripcion
            // 
            this.mmoDescripcion.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.cargoBindingSource, "Descripcion", true));
            this.mmoDescripcion.Location = new System.Drawing.Point(69, 60);
            this.mmoDescripcion.Name = "mmoDescripcion";
            this.mmoDescripcion.Size = new System.Drawing.Size(304, 58);
            this.mmoDescripcion.StyleController = this.dataLayoutControl1;
            this.mmoDescripcion.TabIndex = 5;
            this.mmoDescripcion.UseOptimizedRendering = true;
            this.mmoDescripcion.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // cargoBindingSource
            // 
            this.cargoBindingSource.DataSource = typeof(SAGAS.Entidad.Cargo);
            // 
            // txtNombre
            // 
            this.txtNombre.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.cargoBindingSource, "Nombre", true));
            this.txtNombre.Location = new System.Drawing.Point(69, 12);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(304, 20);
            this.txtNombre.StyleController = this.dataLayoutControl1;
            this.txtNombre.TabIndex = 6;
            this.txtNombre.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // spOrden
            // 
            this.spOrden.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.cargoBindingSource, "Orden", true));
            this.spOrden.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spOrden.Location = new System.Drawing.Point(69, 36);
            this.spOrden.Name = "spOrden";
            this.spOrden.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spOrden.Properties.DisplayFormat.FormatString = "f0";
            this.spOrden.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spOrden.Properties.EditFormat.FormatString = "f0";
            this.spOrden.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spOrden.Properties.Mask.EditMask = "f0";
            this.spOrden.Properties.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.spOrden.Size = new System.Drawing.Size(104, 20);
            this.spOrden.StyleController = this.dataLayoutControl1;
            this.spOrden.TabIndex = 7;
            // 
            // EsBomberoCheckEdit
            // 
            this.EsBomberoCheckEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.cargoBindingSource, "EsBombero", true));
            this.EsBomberoCheckEdit.Location = new System.Drawing.Point(177, 36);
            this.EsBomberoCheckEdit.Name = "EsBomberoCheckEdit";
            this.EsBomberoCheckEdit.Properties.Caption = "Despacha en Pista ?";
            this.EsBomberoCheckEdit.Size = new System.Drawing.Size(196, 19);
            this.EsBomberoCheckEdit.StyleController = this.dataLayoutControl1;
            this.EsBomberoCheckEdit.TabIndex = 9;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForDescripcion,
            this.layoutControlItem1,
            this.ItemForOrden,
            this.ItemForEsBombero});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(385, 130);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // ItemForDescripcion
            // 
            this.ItemForDescripcion.Control = this.mmoDescripcion;
            this.ItemForDescripcion.CustomizationFormText = "Descripcion";
            this.ItemForDescripcion.Location = new System.Drawing.Point(0, 48);
            this.ItemForDescripcion.Name = "ItemForDescripcion";
            this.ItemForDescripcion.Size = new System.Drawing.Size(365, 62);
            this.ItemForDescripcion.Text = "Descripcion";
            this.ItemForDescripcion.TextSize = new System.Drawing.Size(54, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.txtNombre;
            this.layoutControlItem1.CustomizationFormText = "Nombre";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "ItemForNombre";
            this.layoutControlItem1.Size = new System.Drawing.Size(365, 24);
            this.layoutControlItem1.Text = "Nombre";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(54, 13);
            // 
            // ItemForOrden
            // 
            this.ItemForOrden.Control = this.spOrden;
            this.ItemForOrden.CustomizationFormText = "Orden";
            this.ItemForOrden.Location = new System.Drawing.Point(0, 24);
            this.ItemForOrden.Name = "ItemForOrden";
            this.ItemForOrden.Size = new System.Drawing.Size(165, 24);
            this.ItemForOrden.Text = "Orden";
            this.ItemForOrden.TextSize = new System.Drawing.Size(54, 13);
            // 
            // ItemForEsBombero
            // 
            this.ItemForEsBombero.Control = this.EsBomberoCheckEdit;
            this.ItemForEsBombero.CustomizationFormText = "ItemForEsBombero";
            this.ItemForEsBombero.Location = new System.Drawing.Point(165, 24);
            this.ItemForEsBombero.Name = "ItemForEsBombero";
            this.ItemForEsBombero.Size = new System.Drawing.Size(200, 24);
            this.ItemForEsBombero.Text = "ItemForEsBombero";
            this.ItemForEsBombero.TextSize = new System.Drawing.Size(0, 0);
            this.ItemForEsBombero.TextToControlDistance = 0;
            this.ItemForEsBombero.TextVisible = false;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 136);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 151);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(393, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Nomina.Properties.Resources.cancel20;
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
            this.btnOK.Image = global::SAGAS.Nomina.Properties.Resources.Ok20;
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
            // infDifES
            // 
            this.infDifES.ContainerControl = this;
            // 
            // DialogCargo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(393, 185);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCargo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cargoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spOrden.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EsBomberoCheckEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForDescripcion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForOrden)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEsBombero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private System.Windows.Forms.BindingSource cargoBindingSource;
        private DevExpress.XtraEditors.MemoEdit mmoDescripcion;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem ItemForDescripcion;
        private DevExpress.XtraEditors.TextEdit txtNombre;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.SpinEdit spOrden;
        private DevExpress.XtraLayout.LayoutControlItem ItemForOrden;
        private DevExpress.XtraEditors.CheckEdit EsBomberoCheckEdit;
        private DevExpress.XtraLayout.LayoutControlItem ItemForEsBombero;
    }
}