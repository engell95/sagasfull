namespace SAGAS.Parametros.Forms.Dialogs
{
    partial class DialogPrinter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogPrinter));
            this.layoutControlMain = new DevExpress.XtraLayout.LayoutControl();
            this.cboPrinter = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControlCenter = new DevExpress.XtraEditors.PanelControl();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnTest = new DevExpress.XtraEditors.SimpleButton();
            this.saveFileDialogKey = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogKey = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).BeginInit();
            this.layoutControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboPrinter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).BeginInit();
            this.panelControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControlMain
            // 
            this.layoutControlMain.Controls.Add(this.cboPrinter);
            this.layoutControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlMain.Location = new System.Drawing.Point(2, 2);
            this.layoutControlMain.Name = "layoutControlMain";
            this.layoutControlMain.Root = this.layoutControlGroup1;
            this.layoutControlMain.Size = new System.Drawing.Size(431, 76);
            this.layoutControlMain.TabIndex = 0;
            this.layoutControlMain.Text = "layoutControl1";
            // 
            // cboPrinter
            // 
            this.cboPrinter.Location = new System.Drawing.Point(108, 31);
            this.cboPrinter.Name = "cboPrinter";
            this.cboPrinter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboPrinter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboPrinter.Size = new System.Drawing.Size(311, 20);
            this.cboPrinter.StyleController = this.layoutControlMain;
            this.cboPrinter.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de Conección";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(431, 76);
            this.layoutControlGroup1.Text = "Seleccione la Impresora";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.cboPrinter;
            this.layoutControlItem2.CustomizationFormText = "Impresora Local";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(411, 37);
            this.layoutControlItem2.Text = "Impresora Local";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(92, 13);
            // 
            // panelControlCenter
            // 
            this.panelControlCenter.Controls.Add(this.layoutControlMain);
            this.panelControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlCenter.Location = new System.Drawing.Point(0, 0);
            this.panelControlCenter.Name = "panelControlCenter";
            this.panelControlCenter.Size = new System.Drawing.Size(435, 80);
            this.panelControlCenter.TabIndex = 0;
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnSave);
            this.panelControlBottom.Controls.Add(this.btnTest);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControlBottom.Location = new System.Drawing.Point(0, 80);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(435, 35);
            this.panelControlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Parametros.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(246, 2);
            this.btnCancel.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(99, 31);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Image = global::SAGAS.Parametros.Properties.Resources.Ok20;
            this.btnSave.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(115, 2);
            this.btnSave.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(131, 31);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Guardar Impresora";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTest.Image = global::SAGAS.Parametros.Properties.Resources.printerLocal;
            this.btnTest.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(2, 2);
            this.btnTest.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(113, 31);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Probar Impresión";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // openFileDialogKey
            // 
            this.openFileDialogKey.FileName = "openFileDialog1";
            // 
            // DialogPrinter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(435, 115);
            this.Controls.Add(this.panelControlCenter);
            this.Controls.Add(this.panelControlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogPrinter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuración Local";
            this.Load += new System.EventHandler(this.DialogConexion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).EndInit();
            this.layoutControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboPrinter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).EndInit();
            this.panelControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).EndInit();
            this.panelControlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlMain;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.PanelControl panelControlCenter;
        private DevExpress.XtraEditors.PanelControl panelControlBottom;
        private DevExpress.XtraEditors.SimpleButton btnTest;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private System.Windows.Forms.SaveFileDialog saveFileDialogKey;
        private System.Windows.Forms.OpenFileDialog openFileDialogKey;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.ComboBoxEdit cboPrinter;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}