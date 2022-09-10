namespace SAGAS.Inventario.Forms.Dialogs
{
    partial class DialogProductoVariacion
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogProductoVariacion));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.spDiferencia = new DevExpress.XtraEditors.SpinEdit();
            this.spVariacion = new DevExpress.XtraEditors.SpinEdit();
            this.lkProducto = new DevExpress.XtraEditors.LookUpEdit();
            this.lkeSubEstacion = new DevExpress.XtraEditors.LookUpEdit();
            this.lkeEstacionServicio = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.Producto = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblRequerido = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.ofdReport = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spDiferencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spVariacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkProducto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeSubEstacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Producto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
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
            this.layoutControl1.Controls.Add(this.spDiferencia);
            this.layoutControl1.Controls.Add(this.spVariacion);
            this.layoutControl1.Controls.Add(this.lkProducto);
            this.layoutControl1.Controls.Add(this.lkeSubEstacion);
            this.layoutControl1.Controls.Add(this.lkeEstacionServicio);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 2);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(390, 170);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // spDiferencia
            // 
            this.spDiferencia.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spDiferencia.Location = new System.Drawing.Point(138, 127);
            this.spDiferencia.Name = "spDiferencia";
            this.spDiferencia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, false, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spDiferencia.Properties.DisplayFormat.FormatString = "N3";
            this.spDiferencia.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spDiferencia.Properties.EditFormat.FormatString = "N3";
            this.spDiferencia.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spDiferencia.Properties.Mask.EditMask = "N3";
            this.spDiferencia.Size = new System.Drawing.Size(240, 20);
            this.spDiferencia.StyleController = this.layoutControl1;
            this.spDiferencia.TabIndex = 3;
            // 
            // spVariacion
            // 
            this.spVariacion.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spVariacion.Location = new System.Drawing.Point(138, 103);
            this.spVariacion.Name = "spVariacion";
            this.spVariacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, false, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.spVariacion.Properties.DisplayFormat.FormatString = "N3";
            this.spVariacion.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spVariacion.Properties.EditFormat.FormatString = "N3";
            this.spVariacion.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spVariacion.Properties.Mask.EditMask = "N3";
            this.spVariacion.Size = new System.Drawing.Size(240, 20);
            this.spVariacion.StyleController = this.layoutControl1;
            this.spVariacion.TabIndex = 2;
            // 
            // lkProducto
            // 
            this.lkProducto.Location = new System.Drawing.Point(138, 79);
            this.lkProducto.Name = "lkProducto";
            this.lkProducto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkProducto.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Productos", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Descending)});
            this.lkProducto.Properties.DisplayMember = "Nombre";
            this.lkProducto.Properties.NullText = "<Seleccione el Producto>";
            this.lkProducto.Properties.ValueMember = "ID";
            this.lkProducto.Size = new System.Drawing.Size(240, 20);
            this.lkProducto.StyleController = this.layoutControl1;
            this.lkProducto.TabIndex = 6;
            // 
            // lkeSubEstacion
            // 
            this.lkeSubEstacion.Location = new System.Drawing.Point(138, 55);
            this.lkeSubEstacion.Name = "lkeSubEstacion";
            this.lkeSubEstacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeSubEstacion.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkeSubEstacion.Properties.DisplayMember = "Nombre";
            this.lkeSubEstacion.Properties.NullText = "<Seleccione la Sub Estación>";
            this.lkeSubEstacion.Properties.ValueMember = "ID";
            this.lkeSubEstacion.Size = new System.Drawing.Size(240, 20);
            this.lkeSubEstacion.StyleController = this.layoutControl1;
            this.lkeSubEstacion.TabIndex = 5;
            // 
            // lkeEstacionServicio
            // 
            this.lkeEstacionServicio.Location = new System.Drawing.Point(138, 31);
            this.lkeEstacionServicio.Name = "lkeEstacionServicio";
            this.lkeEstacionServicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkeEstacionServicio.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkeEstacionServicio.Properties.DisplayMember = "Nombre";
            this.lkeEstacionServicio.Properties.NullText = "<Seleccione la Estación de Servicio>";
            this.lkeEstacionServicio.Properties.ValueMember = "ID";
            this.lkeEstacionServicio.Size = new System.Drawing.Size(240, 20);
            this.lkeEstacionServicio.StyleController = this.layoutControl1;
            this.lkeEstacionServicio.TabIndex = 4;
            this.lkeEstacionServicio.EditValueChanged += new System.EventHandler(this.lkeEstacionServicio_EditValueChanged);
            this.lkeEstacionServicio.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItem9,
            this.layoutControlItem12,
            this.Producto,
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(390, 170);
            this.layoutControlGroup1.Text = "Datos de la Serie por Retenciones";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 120);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(370, 11);
            this.emptySpaceItem1.Text = "emptySpaceItem1";
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlItem9.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem9.Control = this.lkeEstacionServicio;
            this.layoutControlItem9.CustomizationFormText = "Estación de Servicios";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem9.Text = "Estación de Servicios";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.Control = this.lkeSubEstacion;
            this.layoutControlItem12.CustomizationFormText = "Sub Estación";
            this.layoutControlItem12.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem12.Text = "Sub Estación";
            this.layoutControlItem12.TextSize = new System.Drawing.Size(123, 13);
            // 
            // Producto
            // 
            this.Producto.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.Producto.AppearanceItemCaption.Options.UseFont = true;
            this.Producto.Control = this.lkProducto;
            this.Producto.CustomizationFormText = "Producto";
            this.Producto.Location = new System.Drawing.Point(0, 48);
            this.Producto.Name = "Producto";
            this.Producto.Size = new System.Drawing.Size(370, 24);
            this.Producto.Text = "Producto";
            this.Producto.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.spVariacion;
            this.layoutControlItem2.CustomizationFormText = "Permisible Acta Variación";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem2.Text = "Permisible Acta Variación";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(123, 13);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.spDiferencia;
            this.layoutControlItem1.CustomizationFormText = "Permisible Acta Diferencia";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(370, 24);
            this.layoutControlItem1.Text = "Permisible Acta Diferencia";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(123, 13);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.layoutControl1);
            this.panelControl1.Controls.Add(this.lblRequerido);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(394, 187);
            this.panelControl1.TabIndex = 0;
            // 
            // lblRequerido
            // 
            this.lblRequerido.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblRequerido.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRequerido.Location = new System.Drawing.Point(2, 172);
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
            this.panelControl2.Location = new System.Drawing.Point(0, 187);
            this.panelControl2.LookAndFeel.SkinName = "McSkin";
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(394, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.Image = global::SAGAS.Inventario.Properties.Resources.cancel20;
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
            this.btnOK.Image = global::SAGAS.Inventario.Properties.Resources.Ok20;
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
            // ofdReport
            // 
            this.ofdReport.FileName = "openFileDialog1";
            // 
            // DialogProductoVariacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(394, 221);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogProductoVariacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spDiferencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spVariacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkProducto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeSubEstacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkeEstacionServicio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Producto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
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
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblRequerido;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.LookUpEdit lkeEstacionServicio;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private System.Windows.Forms.OpenFileDialog ofdReport;
        private DevExpress.XtraEditors.LookUpEdit lkeSubEstacion;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem12;
        private DevExpress.XtraEditors.SpinEdit spDiferencia;
        private DevExpress.XtraEditors.SpinEdit spVariacion;
        private DevExpress.XtraEditors.LookUpEdit lkProducto;
        private DevExpress.XtraLayout.LayoutControlItem Producto;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}