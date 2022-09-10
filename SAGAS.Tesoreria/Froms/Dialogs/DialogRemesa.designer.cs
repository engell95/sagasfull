namespace SAGAS.Tesoreria.Forms.Dialogs
{
    partial class DialogRemesa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogRemesa));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.spNumero = new DevExpress.XtraEditors.SpinEdit();
            this.chkEnUso = new DevExpress.XtraEditors.CheckEdit();
            this.spFinal = new DevExpress.XtraEditors.SpinEdit();
            this.chkCambioInicial = new DevExpress.XtraEditors.CheckEdit();
            this.spInicial = new DevExpress.XtraEditors.SpinEdit();
            this.lkCuenta = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemPermitir = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemUso = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemNumero = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.txtConsecutivo = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spNumero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnUso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCambioInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPermitir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNumero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConsecutivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.txtConsecutivo);
            this.layoutControl1.Controls.Add(this.spNumero);
            this.layoutControl1.Controls.Add(this.chkEnUso);
            this.layoutControl1.Controls.Add(this.spFinal);
            this.layoutControl1.Controls.Add(this.chkCambioInicial);
            this.layoutControl1.Controls.Add(this.spInicial);
            this.layoutControl1.Controls.Add(this.lkCuenta);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(347, 206);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // spNumero
            // 
            this.spNumero.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spNumero.Location = new System.Drawing.Point(107, 173);
            this.spNumero.Name = "spNumero";
            this.spNumero.Properties.AllowFocused = false;
            this.spNumero.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.spNumero.Properties.Appearance.Options.UseBackColor = true;
            this.spNumero.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spNumero.Properties.DisplayFormat.FormatString = "N0";
            this.spNumero.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spNumero.Properties.EditFormat.FormatString = "N0";
            this.spNumero.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spNumero.Properties.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spNumero.Properties.ReadOnly = true;
            this.spNumero.Size = new System.Drawing.Size(228, 20);
            this.spNumero.StyleController = this.layoutControl1;
            this.spNumero.TabIndex = 3;
            // 
            // chkEnUso
            // 
            this.chkEnUso.Enabled = false;
            this.chkEnUso.Location = new System.Drawing.Point(12, 150);
            this.chkEnUso.Name = "chkEnUso";
            this.chkEnUso.Properties.AllowFocused = false;
            this.chkEnUso.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chkEnUso.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Black;
            this.chkEnUso.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.chkEnUso.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.chkEnUso.Properties.Caption = "Remesa en Uso";
            this.chkEnUso.Properties.ReadOnly = true;
            this.chkEnUso.Size = new System.Drawing.Size(323, 19);
            this.chkEnUso.StyleController = this.layoutControl1;
            this.chkEnUso.TabIndex = 3;
            // 
            // spFinal
            // 
            this.spFinal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spFinal.Location = new System.Drawing.Point(107, 126);
            this.spFinal.Name = "spFinal";
            this.spFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spFinal.Properties.DisplayFormat.FormatString = "N0";
            this.spFinal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spFinal.Properties.EditFormat.FormatString = "N0";
            this.spFinal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spFinal.Properties.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spFinal.Size = new System.Drawing.Size(228, 20);
            this.spFinal.StyleController = this.layoutControl1;
            this.spFinal.TabIndex = 3;
            // 
            // chkCambioInicial
            // 
            this.chkCambioInicial.Location = new System.Drawing.Point(12, 79);
            this.chkCambioInicial.Name = "chkCambioInicial";
            this.chkCambioInicial.Properties.Caption = "Permitir cambiar el número inicial?";
            this.chkCambioInicial.Size = new System.Drawing.Size(323, 19);
            this.chkCambioInicial.StyleController = this.layoutControl1;
            this.chkCambioInicial.TabIndex = 2;
            this.chkCambioInicial.CheckedChanged += new System.EventHandler(this.chkCambioInicial_CheckedChanged);
            // 
            // spInicial
            // 
            this.spInicial.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spInicial.Location = new System.Drawing.Point(107, 102);
            this.spInicial.Name = "spInicial";
            this.spInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spInicial.Properties.DisplayFormat.FormatString = "N0";
            this.spInicial.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spInicial.Properties.EditFormat.FormatString = "N0";
            this.spInicial.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spInicial.Properties.MaxValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.spInicial.Properties.ReadOnly = true;
            this.spInicial.Size = new System.Drawing.Size(228, 20);
            this.spInicial.StyleController = this.layoutControl1;
            this.spInicial.TabIndex = 2;
            // 
            // lkCuenta
            // 
            this.lkCuenta.Location = new System.Drawing.Point(107, 31);
            this.lkCuenta.Name = "lkCuenta";
            this.lkCuenta.Properties.AllowFocused = false;
            this.lkCuenta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCuenta.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuentas")});
            this.lkCuenta.Properties.DisplayMember = "Display";
            this.lkCuenta.Properties.NullText = "<Seleccione la Cuenta>";
            this.lkCuenta.Properties.ReadOnly = true;
            this.lkCuenta.Properties.ValueMember = "ID";
            this.lkCuenta.Size = new System.Drawing.Size(228, 20);
            this.lkCuenta.StyleController = this.layoutControl1;
            this.lkCuenta.TabIndex = 2;
            this.lkCuenta.EditValueChanged += new System.EventHandler(this.lkCuenta_EditValueChanged);
            this.lkCuenta.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5,
            this.layoutControlItemPermitir,
            this.layoutControlItem4,
            this.layoutControlItemUso,
            this.layoutControlItem7,
            this.layoutControlItem1,
            this.layoutControlItemNumero});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(347, 206);
            this.layoutControlGroup1.Text = "Datos de la Remesa";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem5.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem5.Control = this.lkCuenta;
            this.layoutControlItem5.CustomizationFormText = "Banco";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(327, 24);
            this.layoutControlItem5.Text = "Cuenta Bancaria";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItemPermitir
            // 
            this.layoutControlItemPermitir.Control = this.chkCambioInicial;
            this.layoutControlItemPermitir.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItemPermitir.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItemPermitir.Name = "layoutControlItemPermitir";
            this.layoutControlItemPermitir.Size = new System.Drawing.Size(327, 23);
            this.layoutControlItemPermitir.Text = "layoutControlItemPermitir";
            this.layoutControlItemPermitir.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemPermitir.TextToControlDistance = 0;
            this.layoutControlItemPermitir.TextVisible = false;
            this.layoutControlItemPermitir.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem4.Control = this.spInicial;
            this.layoutControlItem4.CustomizationFormText = "Cantidad Minima";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 71);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(327, 24);
            this.layoutControlItem4.Text = "Número Inicial";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItemUso
            // 
            this.layoutControlItemUso.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.layoutControlItemUso.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlItemUso.Control = this.chkEnUso;
            this.layoutControlItemUso.CustomizationFormText = "layoutControlItemUso";
            this.layoutControlItemUso.Location = new System.Drawing.Point(0, 119);
            this.layoutControlItemUso.Name = "layoutControlItemUso";
            this.layoutControlItemUso.Size = new System.Drawing.Size(327, 23);
            this.layoutControlItemUso.Text = "layoutControlItemUso";
            this.layoutControlItemUso.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemUso.TextToControlDistance = 0;
            this.layoutControlItemUso.TextVisible = false;
            this.layoutControlItemUso.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem7.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem7.Control = this.spFinal;
            this.layoutControlItem7.CustomizationFormText = "Número Final";
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 95);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(327, 24);
            this.layoutControlItem7.Text = "Número Final";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(92, 13);
            // 
            // layoutControlItemNumero
            // 
            this.layoutControlItemNumero.AppearanceItemCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.layoutControlItemNumero.AppearanceItemCaption.Options.UseBackColor = true;
            this.layoutControlItemNumero.Control = this.spNumero;
            this.layoutControlItemNumero.CustomizationFormText = "Numero Actual";
            this.layoutControlItemNumero.Location = new System.Drawing.Point(0, 142);
            this.layoutControlItemNumero.Name = "layoutControlItemNumero";
            this.layoutControlItemNumero.Size = new System.Drawing.Size(327, 25);
            this.layoutControlItemNumero.Text = "Numero Actual";
            this.layoutControlItemNumero.TextSize = new System.Drawing.Size(92, 13);
            this.layoutControlItemNumero.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(351, 223);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 208);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 223);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(351, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Tesoreria.Properties.Resources.cancel20;
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
            this.btnOK.Image = global::SAGAS.Tesoreria.Properties.Resources.Ok20;
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
            // txtConsecutivo
            // 
            this.txtConsecutivo.Location = new System.Drawing.Point(107, 55);
            this.txtConsecutivo.Name = "txtConsecutivo";
            this.txtConsecutivo.Properties.AllowFocused = false;
            this.txtConsecutivo.Properties.ReadOnly = true;
            this.txtConsecutivo.Size = new System.Drawing.Size(228, 20);
            this.txtConsecutivo.StyleController = this.layoutControl1;
            this.txtConsecutivo.TabIndex = 2;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem1.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem1.Control = this.txtConsecutivo;
            this.layoutControlItem1.CustomizationFormText = "Consecutivo";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(327, 24);
            this.layoutControlItem1.Text = "Consecutivo";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(92, 13);
            // 
            // DialogRemesa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(351, 257);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogRemesa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spNumero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnUso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkCambioInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPermitir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNumero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtConsecutivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.LookUpEdit lkCuenta;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.SpinEdit spInicial;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.CheckEdit chkCambioInicial;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPermitir;
        private DevExpress.XtraEditors.SpinEdit spFinal;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraEditors.CheckEdit chkEnUso;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUso;
        private DevExpress.XtraEditors.SpinEdit spNumero;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemNumero;
        private DevExpress.XtraEditors.TextEdit txtConsecutivo;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}