namespace SAGAS.Parametros.Forms.Dialogs
{
    partial class DialogLicencia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogLicencia));
            this.layoutControlMain = new DevExpress.XtraLayout.LayoutControl();
            this.rgConexion = new DevExpress.XtraEditors.RadioGroup();
            this.lkeEstacionServicio = new DevExpress.XtraEditors.LookUpEdit();
            this.txtLicencia = new DevExpress.XtraEditors.TextEdit();
            this.txtSerialRegistro = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControlCenter = new DevExpress.XtraEditors.PanelControl();
            this.panelControlBottom = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.saveFileDialogKey = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogKey = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).BeginInit();
            this.layoutControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgConexion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerialRegistro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCenter)).BeginInit();
            this.panelControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlBottom)).BeginInit();
            this.panelControlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutControlMain
            // 
            this.layoutControlMain.Controls.Add(this.rgConexion);
            this.layoutControlMain.Controls.Add(this.lkeEstacionServicio);
            this.layoutControlMain.Controls.Add(this.txtLicencia);
            this.layoutControlMain.Controls.Add(this.txtSerialRegistro);
            this.layoutControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlMain.Location = new System.Drawing.Point(2, 2);
            this.layoutControlMain.Name = "layoutControlMain";
            this.layoutControlMain.Root = this.layoutControlGroup1;
            this.layoutControlMain.Size = new System.Drawing.Size(431, 144);
            this.layoutControlMain.TabIndex = 0;
            this.layoutControlMain.Text = "layoutControl1";
            // 
            // rgConexion
            // 
            this.rgConexion.Location = new System.Drawing.Point(129, 55);
            this.rgConexion.Name = "rgConexion";
            this.rgConexion.Properties.Columns = 2;
            this.rgConexion.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Remota"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Local")});
            this.rgConexion.Size = new System.Drawing.Size(290, 29);
            this.rgConexion.StyleController = this.layoutControlMain;
            this.rgConexion.TabIndex = 3;
            // 
            // lkeEstacionServicio
            // 
            this.lkeEstacionServicio.Location = new System.Drawing.Point(129, 31);
            this.lkeEstacionServicio.Name = "lkeEstacionServicio";
            this.lkeEstacionServicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeEstacionServicio.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio")});
            this.lkeEstacionServicio.Properties.NullText = "<Seleccione la Estación de Servicio>";
            this.lkeEstacionServicio.Size = new System.Drawing.Size(290, 20);
            this.lkeEstacionServicio.StyleController = this.layoutControlMain;
            this.lkeEstacionServicio.TabIndex = 6;
            // 
            // txtLicencia
            // 
            this.txtLicencia.EditValue = "";
            this.txtLicencia.Location = new System.Drawing.Point(129, 112);
            this.txtLicencia.Name = "txtLicencia";
            this.txtLicencia.Size = new System.Drawing.Size(290, 20);
            this.txtLicencia.StyleController = this.layoutControlMain;
            this.txtLicencia.TabIndex = 2;
            // 
            // txtSerialRegistro
            // 
            this.txtSerialRegistro.EditValue = "";
            this.txtSerialRegistro.Location = new System.Drawing.Point(129, 88);
            this.txtSerialRegistro.Name = "txtSerialRegistro";
            this.txtSerialRegistro.Properties.ReadOnly = true;
            this.txtSerialRegistro.Size = new System.Drawing.Size(290, 20);
            this.txtSerialRegistro.StyleController = this.layoutControlMain;
            this.txtSerialRegistro.TabIndex = 1;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de Conección";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem1,
            this.layoutControlItem5});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(431, 144);
            this.layoutControlGroup1.Text = "Datos Generales";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem2.Control = this.txtSerialRegistro;
            this.layoutControlItem2.CustomizationFormText = "Base de Datos";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 57);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(411, 24);
            this.layoutControlItem2.Text = "Serial de Registro";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(114, 13);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem3.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem3.Control = this.txtLicencia;
            this.layoutControlItem3.CustomizationFormText = "Usuario";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 81);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(411, 24);
            this.layoutControlItem3.Text = "Licencia del Sistema";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(114, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.lkeEstacionServicio;
            this.layoutControlItem1.CustomizationFormText = "Estación de Servicio";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(411, 24);
            this.layoutControlItem1.Text = "Estación de Servicio";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(114, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.Control = this.rgConexion;
            this.layoutControlItem5.CustomizationFormText = "Tipo de Conexión";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(411, 33);
            this.layoutControlItem5.Text = "Tipo de Conexión";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(114, 13);
            // 
            // panelControlCenter
            // 
            this.panelControlCenter.Controls.Add(this.layoutControlMain);
            this.panelControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlCenter.Location = new System.Drawing.Point(0, 0);
            this.panelControlCenter.Name = "panelControlCenter";
            this.panelControlCenter.Size = new System.Drawing.Size(435, 148);
            this.panelControlCenter.TabIndex = 0;
            // 
            // panelControlBottom
            // 
            this.panelControlBottom.Controls.Add(this.btnCancel);
            this.panelControlBottom.Controls.Add(this.btnSave);
            this.panelControlBottom.Controls.Add(this.btnOk);
            this.panelControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControlBottom.Location = new System.Drawing.Point(0, 148);
            this.panelControlBottom.Name = "panelControlBottom";
            this.panelControlBottom.Size = new System.Drawing.Size(435, 47);
            this.panelControlBottom.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Parametros.Properties.Resources.cancel20;
            this.btnCancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(306, 2);
            this.btnCancel.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(127, 43);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSave.Image = global::SAGAS.Parametros.Properties.Resources.save_key_icon;
            this.btnSave.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(155, 2);
            this.btnSave.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(151, 43);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Validar Licencia";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOk.Image = global::SAGAS.Parametros.Properties.Resources.open_key_icon;
            this.btnOk.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(2, 2);
            this.btnOk.LookAndFeel.SkinName = "Office 2007 Green";
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(153, 43);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Exportar Registro";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // openFileDialogKey
            // 
            this.openFileDialogKey.FileName = "openFileDialog1";
            // 
            // DialogLicencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(435, 195);
            this.Controls.Add(this.panelControlCenter);
            this.Controls.Add(this.panelControlBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogLicencia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Licencia SAGAS";
            this.Load += new System.EventHandler(this.DialogConexion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlMain)).EndInit();
            this.layoutControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgConexion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLicencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSerialRegistro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
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
        private DevExpress.XtraEditors.TextEdit txtLicencia;
        private DevExpress.XtraEditors.TextEdit txtSerialRegistro;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        public DevExpress.XtraEditors.LookUpEdit lkeEstacionServicio;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.RadioGroup rgConexion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private System.Windows.Forms.SaveFileDialog saveFileDialogKey;
        private System.Windows.Forms.OpenFileDialog openFileDialogKey;
        public DevExpress.XtraEditors.SimpleButton btnOk;
    }
}