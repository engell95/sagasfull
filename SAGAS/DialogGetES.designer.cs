namespace SAGAS
{
    partial class DialogGetES
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogGetES));
            this.panelControlCenter = new DevExpress.XtraEditors.PanelControl();
            this.layoutControlCenter = new DevExpress.XtraLayout.LayoutControl();
            this.lkES = new DevExpress.XtraEditors.LookUpEdit();
            this.lkSES = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemSES = new DevExpress.XtraLayout.LayoutControlItem();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).BeginInit();
            this.panelControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCenter)).BeginInit();
            this.layoutControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkES.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlCenter
            // 
            this.panelControlCenter.Controls.Add(this.layoutControlCenter);
            this.panelControlCenter.Controls.Add(this.lblRequerido);
            this.panelControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlCenter.Location = new System.Drawing.Point(0, 0);
            this.panelControlCenter.LookAndFeel.SkinName = "McSkin";
            this.panelControlCenter.Name = "panelControlCenter";
            this.panelControlCenter.Size = new System.Drawing.Size(393, 89);
            this.panelControlCenter.TabIndex = 1;
            // 
            // layoutControlCenter
            // 
            this.layoutControlCenter.Controls.Add(this.lkES);
            this.layoutControlCenter.Controls.Add(this.lkSES);
            this.layoutControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlCenter.Location = new System.Drawing.Point(2, 2);
            this.layoutControlCenter.Name = "layoutControlCenter";
            this.layoutControlCenter.Root = this.layoutControlGroup1;
            this.layoutControlCenter.Size = new System.Drawing.Size(389, 72);
            this.layoutControlCenter.TabIndex = 0;
            this.layoutControlCenter.Text = "layoutControl1";
            // 
            // lkES
            // 
            this.lkES.Location = new System.Drawing.Point(122, 7);
            this.lkES.Name = "lkES";
            this.lkES.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkES.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Estación", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkES.Properties.NullText = "<Seleccione la Estación>";
            this.lkES.Size = new System.Drawing.Size(260, 20);
            this.lkES.StyleController = this.layoutControlCenter;
            this.lkES.TabIndex = 4;
            this.lkES.EditValueChanged += new System.EventHandler(this.lkES_EditValueChanged);
            // 
            // lkSES
            // 
            this.lkSES.Location = new System.Drawing.Point(122, 31);
            this.lkSES.Name = "lkSES";
            this.lkSES.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSES.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Sub Estación")});
            this.lkSES.Properties.NullText = "<Seleccione la Sub Estación>";
            this.lkSES.Size = new System.Drawing.Size(260, 20);
            this.lkSES.StyleController = this.layoutControlCenter;
            this.lkSES.TabIndex = 3;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItemSES});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(389, 72);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.lkES;
            this.layoutControlItem2.CustomizationFormText = "Estación de Servicio";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(379, 24);
            this.layoutControlItem2.Text = "Estación de Servicio";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(112, 13);
            // 
            // layoutControlItemSES
            // 
            this.layoutControlItemSES.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItemSES.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItemSES.Control = this.lkSES;
            this.layoutControlItemSES.CustomizationFormText = "Sub Estación";
            this.layoutControlItemSES.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemSES.Name = "layoutControlItemSES";
            this.layoutControlItemSES.Size = new System.Drawing.Size(379, 38);
            this.layoutControlItemSES.Text = "Sub Estación";
            this.layoutControlItemSES.TextSize = new System.Drawing.Size(112, 13);
            this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 74);
            this.lblRequerido.Name = "lblRequerido";
            this.lblRequerido.Size = new System.Drawing.Size(255, 13);
            this.lblRequerido.TabIndex = 3;
            this.lblRequerido.Text = "Nota:  Los campos en negritas son requeridos";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 89);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(393, 34);
            this.panelControl2.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(90, 2);
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
            this.btnOK.Image = global::SAGAS.Properties.Resources.Ok20;
            this.btnOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Continuar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lkSES;
            this.layoutControlItem1.CustomizationFormText = "Sub Estación";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItemSES";
            this.layoutControlItem1.Size = new System.Drawing.Size(379, 71);
            this.layoutControlItem1.Text = "Sub Estación";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(61, 13);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // DialogGetES
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(393, 123);
            this.Controls.Add(this.panelControlCenter);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogGetES";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Seleccionar los Parametros";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).EndInit();
            this.panelControlCenter.ResumeLayout(false);
            this.panelControlCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlCenter)).EndInit();
            this.layoutControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkES.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlCenter;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControl layoutControlCenter;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.LookUpEdit lkSES;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSES;
        private DevExpress.XtraEditors.LookUpEdit lkES;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
    }
}