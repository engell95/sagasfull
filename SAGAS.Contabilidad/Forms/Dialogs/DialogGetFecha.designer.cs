namespace SAGAS.Contabilidad.Forms.Dialogs
{
    partial class DialogGetFecha
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogGetFecha));
            this.panelControlCenter = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlCenter = new DevExpress.XtraLayout.LayoutControl();
            this.dateFecha = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemCaption = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).BeginInit();
            this.panelControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCenter)).BeginInit();
            this.layoutControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCaption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlCenter
            // 
            this.panelControlCenter.Controls.Add(this.layoutControlCenter);
            this.panelControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlCenter.Location = new System.Drawing.Point(0, 0);
            this.panelControlCenter.LookAndFeel.SkinName = "McSkin";
            this.panelControlCenter.Name = "panelControlCenter";
            this.panelControlCenter.Size = new System.Drawing.Size(393, 46);
            this.panelControlCenter.TabIndex = 1;
            // 
            // layoutControlCenter
            // 
            this.layoutControlCenter.Controls.Add(this.dateFecha);
            this.layoutControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlCenter.Location = new System.Drawing.Point(2, 2);
            this.layoutControlCenter.Name = "layoutControlCenter";
            this.layoutControlCenter.Root = this.layoutControlGroup1;
            this.layoutControlCenter.Size = new System.Drawing.Size(389, 42);
            this.layoutControlCenter.TabIndex = 0;
            this.layoutControlCenter.Text = "layoutControl1";
            // 
            // dateFecha
            // 
            this.dateFecha.EditValue = null;
            this.dateFecha.Location = new System.Drawing.Point(119, 7);
            this.dateFecha.Name = "dateFecha";
            this.dateFecha.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFecha.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFecha.Size = new System.Drawing.Size(263, 20);
            this.dateFecha.StyleController = this.layoutControlCenter;
            this.dateFecha.TabIndex = 3;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemCaption});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(389, 42);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItemCaption
            // 
            this.layoutControlItemCaption.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemCaption.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemCaption.Control = this.dateFecha;
            this.layoutControlItemCaption.CustomizationFormText = "<>";
            this.layoutControlItemCaption.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemCaption.Name = "layoutControlItemCaption";
            this.layoutControlItemCaption.Size = new System.Drawing.Size(379, 32);
            this.layoutControlItemCaption.Text = "Seleccione la Fecha";
            this.layoutControlItemCaption.TextSize = new System.Drawing.Size(109, 13);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 46);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(393, 34);
            this.panelControl2.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Contabilidad.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(98, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.Image = global::SAGAS.Contabilidad.Properties.Resources.Ok20;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(96, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Seleccionar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // DialogGetFecha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(393, 80);
            this.Controls.Add(this.panelControlCenter);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogGetFecha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccionar la Fecha";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).EndInit();
            this.panelControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCenter)).EndInit();
            this.layoutControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFecha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCaption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlCenter;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControl layoutControlCenter;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        public DevExpress.XtraEditors.DateEdit dateFecha;
        public DevExpress.XtraLayout.LayoutControlItem layoutControlItemCaption;
    }
}