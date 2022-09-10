namespace SAGAS.Contabilidad.Forms.Dialogs
{
    partial class DialogCierre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogCierre));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnCrear = new DevExpress.XtraEditors.SimpleButton();
            this.ckCerrado = new DevExpress.XtraEditors.CheckEdit();
            this.ckConsolidado = new DevExpress.XtraEditors.CheckEdit();
            this.lkeEstacionServicio = new DevExpress.XtraEditors.LookUpEdit();
            this.bdsManejadorDatos = new System.Windows.Forms.BindingSource();
            this.txtPeriodoAnterior = new DevExpress.XtraEditors.TextEdit();
            this.txtPeriodoApertura = new DevExpress.XtraEditors.TextEdit();
            this.dtPeriodoCerrar = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcPeriodo = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcConsolidado = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcCerrado = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcPeriodoCerrar = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcCrear = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcPeriodoAperturar = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckCerrado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckConsolidado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoAnterior.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoApertura.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoCerrar.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoCerrar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConsolidado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCerrado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodoCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCrear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodoAperturar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnCrear);
            this.layoutControl1.Controls.Add(this.ckCerrado);
            this.layoutControl1.Controls.Add(this.ckConsolidado);
            this.layoutControl1.Controls.Add(this.lkeEstacionServicio);
            this.layoutControl1.Controls.Add(this.txtPeriodoAnterior);
            this.layoutControl1.Controls.Add(this.txtPeriodoApertura);
            this.layoutControl1.Controls.Add(this.dtPeriodoCerrar);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(455, 45, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(403, 183);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnCrear
            // 
            this.btnCrear.Image = global::SAGAS.Contabilidad.Properties.Resources.resetPass24;
            this.btnCrear.Location = new System.Drawing.Point(256, 96);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(135, 30);
            this.btnCrear.StyleController = this.layoutControl1;
            this.btnCrear.TabIndex = 13;
            this.btnCrear.Text = "Generar Cierre";
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            // 
            // ckCerrado
            // 
            this.ckCerrado.Location = new System.Drawing.Point(256, 73);
            this.ckCerrado.Name = "ckCerrado";
            this.ckCerrado.Properties.Caption = "";
            this.ckCerrado.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ckCerrado.Properties.ReadOnly = true;
            this.ckCerrado.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ckCerrado.Size = new System.Drawing.Size(68, 19);
            this.ckCerrado.StyleController = this.layoutControl1;
            this.ckCerrado.TabIndex = 12;
            // 
            // ckConsolidado
            // 
            this.ckConsolidado.Location = new System.Drawing.Point(328, 73);
            this.ckConsolidado.Name = "ckConsolidado";
            this.ckConsolidado.Properties.Caption = "";
            this.ckConsolidado.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ckConsolidado.Properties.ReadOnly = true;
            this.ckConsolidado.Size = new System.Drawing.Size(63, 19);
            this.ckConsolidado.StyleController = this.layoutControl1;
            this.ckConsolidado.TabIndex = 11;
            // 
            // lkeEstacionServicio
            // 
            this.lkeEstacionServicio.Location = new System.Drawing.Point(127, 31);
            this.lkeEstacionServicio.Name = "lkeEstacionServicio";
            this.lkeEstacionServicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeEstacionServicio.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estación de Servicio")});
            this.lkeEstacionServicio.Properties.DataSource = this.bdsManejadorDatos;
            this.lkeEstacionServicio.Properties.DisplayMember = "Nombre";
            this.lkeEstacionServicio.Properties.NullText = "<Seleccione la Estación de Servicio>";
            this.lkeEstacionServicio.Properties.ReadOnly = true;
            this.lkeEstacionServicio.Properties.ValueMember = "ID";
            this.lkeEstacionServicio.Size = new System.Drawing.Size(264, 20);
            this.lkeEstacionServicio.StyleController = this.layoutControl1;
            this.lkeEstacionServicio.TabIndex = 2;
            this.lkeEstacionServicio.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // txtPeriodoAnterior
            // 
            this.txtPeriodoAnterior.EditValue = "";
            this.txtPeriodoAnterior.Location = new System.Drawing.Point(12, 71);
            this.txtPeriodoAnterior.Name = "txtPeriodoAnterior";
            this.txtPeriodoAnterior.Properties.AllowFocused = false;
            this.txtPeriodoAnterior.Properties.ReadOnly = true;
            this.txtPeriodoAnterior.Size = new System.Drawing.Size(240, 20);
            this.txtPeriodoAnterior.StyleController = this.layoutControl1;
            this.txtPeriodoAnterior.TabIndex = 0;
            this.txtPeriodoAnterior.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // txtPeriodoApertura
            // 
            this.txtPeriodoApertura.Location = new System.Drawing.Point(12, 151);
            this.txtPeriodoApertura.Name = "txtPeriodoApertura";
            this.txtPeriodoApertura.Properties.ReadOnly = true;
            this.txtPeriodoApertura.Size = new System.Drawing.Size(240, 20);
            this.txtPeriodoApertura.StyleController = this.layoutControl1;
            this.txtPeriodoApertura.TabIndex = 15;
            // 
            // dtPeriodoCerrar
            // 
            this.dtPeriodoCerrar.EditValue = null;
            this.dtPeriodoCerrar.Location = new System.Drawing.Point(12, 111);
            this.dtPeriodoCerrar.Name = "dtPeriodoCerrar";
            this.dtPeriodoCerrar.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.False;
            this.dtPeriodoCerrar.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriodoCerrar.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPeriodoCerrar.Properties.Mask.EditMask = "";
            this.dtPeriodoCerrar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.dtPeriodoCerrar.Properties.ReadOnly = true;
            this.dtPeriodoCerrar.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.dtPeriodoCerrar.Size = new System.Drawing.Size(240, 20);
            this.dtPeriodoCerrar.StyleController = this.layoutControl1;
            this.dtPeriodoCerrar.TabIndex = 14;
            this.dtPeriodoCerrar.EditValueChanged += new System.EventHandler(this.dtPeriodoCerrar_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcPeriodo,
            this.lcConsolidado,
            this.lcCerrado,
            this.layoutControlItem3,
            this.lcPeriodoCerrar,
            this.lcCrear,
            this.lcPeriodoAperturar});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(403, 183);
            this.layoutControlGroup1.Text = "Parámetros del Cierre";
            // 
            // lcPeriodo
            // 
            this.lcPeriodo.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcPeriodo.AppearanceItemCaption.Options.UseFont = true;
            this.lcPeriodo.Control = this.txtPeriodoAnterior;
            this.lcPeriodo.CustomizationFormText = "Servidor";
            this.lcPeriodo.Location = new System.Drawing.Point(0, 24);
            this.lcPeriodo.Name = "lcPeriodo";
            this.lcPeriodo.Size = new System.Drawing.Size(244, 40);
            this.lcPeriodo.Text = "Periodo anterior:";
            this.lcPeriodo.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcPeriodo.TextSize = new System.Drawing.Size(112, 13);
            // 
            // lcConsolidado
            // 
            this.lcConsolidado.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lcConsolidado.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lcConsolidado.Control = this.ckConsolidado;
            this.lcConsolidado.CustomizationFormText = "Consolidado";
            this.lcConsolidado.Location = new System.Drawing.Point(316, 24);
            this.lcConsolidado.Name = "lcConsolidado";
            this.lcConsolidado.Size = new System.Drawing.Size(67, 41);
            this.lcConsolidado.Text = "Consolidado";
            this.lcConsolidado.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lcConsolidado.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcConsolidado.TextSize = new System.Drawing.Size(58, 13);
            this.lcConsolidado.TextToControlDistance = 5;
            // 
            // lcCerrado
            // 
            this.lcCerrado.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lcCerrado.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lcCerrado.Control = this.ckCerrado;
            this.lcCerrado.CustomizationFormText = "Cerrado";
            this.lcCerrado.Location = new System.Drawing.Point(244, 24);
            this.lcCerrado.Name = "lcCerrado";
            this.lcCerrado.Size = new System.Drawing.Size(72, 41);
            this.lcCerrado.Text = "Cerrado";
            this.lcCerrado.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.lcCerrado.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcCerrado.TextSize = new System.Drawing.Size(39, 13);
            this.lcCerrado.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.lkeEstacionServicio;
            this.layoutControlItem3.CustomizationFormText = "Empresa";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(383, 24);
            this.layoutControlItem3.Text = "Estación de Servicio:";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(112, 13);
            // 
            // lcPeriodoCerrar
            // 
            this.lcPeriodoCerrar.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcPeriodoCerrar.AppearanceItemCaption.Options.UseFont = true;
            this.lcPeriodoCerrar.Control = this.dtPeriodoCerrar;
            this.lcPeriodoCerrar.CustomizationFormText = "Periodo a Cerrar";
            this.lcPeriodoCerrar.Location = new System.Drawing.Point(0, 64);
            this.lcPeriodoCerrar.Name = "lcPeriodoCerrar";
            this.lcPeriodoCerrar.Size = new System.Drawing.Size(244, 40);
            this.lcPeriodoCerrar.Text = "Periodo a Cerrar";
            this.lcPeriodoCerrar.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcPeriodoCerrar.TextSize = new System.Drawing.Size(112, 13);
            // 
            // lcCrear
            // 
            this.lcCrear.Control = this.btnCrear;
            this.lcCrear.CustomizationFormText = "lcCrear";
            this.lcCrear.Location = new System.Drawing.Point(244, 65);
            this.lcCrear.Name = "lcCrear";
            this.lcCrear.Size = new System.Drawing.Size(139, 79);
            this.lcCrear.Text = "lcCrear";
            this.lcCrear.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcCrear.TextSize = new System.Drawing.Size(0, 0);
            this.lcCrear.TextToControlDistance = 0;
            this.lcCrear.TextVisible = false;
            // 
            // lcPeriodoAperturar
            // 
            this.lcPeriodoAperturar.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lcPeriodoAperturar.AppearanceItemCaption.Options.UseFont = true;
            this.lcPeriodoAperturar.Control = this.txtPeriodoApertura;
            this.lcPeriodoAperturar.CustomizationFormText = "Periodo a Aperturar";
            this.lcPeriodoAperturar.Location = new System.Drawing.Point(0, 104);
            this.lcPeriodoAperturar.Name = "lcPeriodoAperturar";
            this.lcPeriodoAperturar.Size = new System.Drawing.Size(244, 40);
            this.lcPeriodoAperturar.Text = "Periodo a Aperturar";
            this.lcPeriodoAperturar.TextLocation = DevExpress.Utils.Locations.Top;
            this.lcPeriodoAperturar.TextSize = new System.Drawing.Size(112, 13);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(407, 201);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 186);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 201);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(407, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Contabilidad.Properties.Resources.cancel20;
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
            this.btnOK.Image = global::SAGAS.Contabilidad.Properties.Resources.Ok20;
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
            // DialogCierre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(407, 235);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogCierre";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckCerrado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckConsolidado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoAnterior.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoApertura.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoCerrar.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPeriodoCerrar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcConsolidado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCerrado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodoCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcCrear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcPeriodoAperturar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit txtPeriodoAnterior;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraLayout.LayoutControlItem lcPeriodo;
        private DevExpress.XtraEditors.LookUpEdit lkeEstacionServicio;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private System.Windows.Forms.BindingSource dataClasses1DataContextBindingSource;
        private System.Windows.Forms.BindingSource bdsManejadorDatos;
        private DevExpress.XtraEditors.CheckEdit ckCerrado;
        private DevExpress.XtraEditors.CheckEdit ckConsolidado;
        private DevExpress.XtraLayout.LayoutControlItem lcConsolidado;
        private DevExpress.XtraLayout.LayoutControlItem lcCerrado;
        private DevExpress.XtraEditors.SimpleButton btnCrear;
        private DevExpress.XtraLayout.LayoutControlItem lcCrear;
        private DevExpress.XtraLayout.LayoutControlItem lcPeriodoCerrar;
        private DevExpress.XtraLayout.LayoutControlItem lcPeriodoAperturar;
        private DevExpress.XtraEditors.TextEdit txtPeriodoApertura;
        private DevExpress.XtraEditors.DateEdit dtPeriodoCerrar;
    }
}