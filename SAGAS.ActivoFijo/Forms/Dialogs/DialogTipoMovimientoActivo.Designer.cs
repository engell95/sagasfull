namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    partial class DialogTipoMovimientoActivo
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogTipoMovimientoActivo));
            this.lblNombre = new DevExpress.XtraEditors.LabelControl();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.lblDatosGenerales = new DevExpress.XtraEditors.LabelControl();
            this.lblNota = new DevExpress.XtraEditors.LabelControl();
            this.lblSeparadorPie = new DevExpress.XtraEditors.LabelControl();
            this.btnGuardar = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.ckAplicaDepreciacion = new DevExpress.XtraEditors.CheckEdit();
            this.radioTipo = new DevExpress.XtraEditors.RadioGroup();
            this.radioEstado = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAplicaDepreciacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioEstado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblNombre
            // 
            this.lblNombre.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(3, 33);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(3, 51);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(308, 20);
            this.txtNombre.TabIndex = 25;
            this.txtNombre.ToolTip = "Nombre del bien";
            this.txtNombre.Validated += new System.EventHandler(this.txtNombre_Validated);
            // 
            // lblDatosGenerales
            // 
            this.lblDatosGenerales.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatosGenerales.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblDatosGenerales.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDatosGenerales.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal;
            this.lblDatosGenerales.LineVisible = true;
            this.lblDatosGenerales.Location = new System.Drawing.Point(3, 3);
            this.lblDatosGenerales.Name = "lblDatosGenerales";
            this.lblDatosGenerales.Size = new System.Drawing.Size(308, 24);
            this.lblDatosGenerales.TabIndex = 29;
            this.lblDatosGenerales.Text = "Datos Generales";
            // 
            // lblNota
            // 
            this.lblNota.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNota.Location = new System.Drawing.Point(5, 231);
            this.lblNota.Name = "lblNota";
            this.lblNota.Size = new System.Drawing.Size(252, 13);
            this.lblNota.TabIndex = 35;
            this.lblNota.Text = "Nota: Los campos en negritas son requeridos";
            // 
            // lblSeparadorPie
            // 
            this.lblSeparadorPie.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeparadorPie.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblSeparadorPie.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal;
            this.lblSeparadorPie.LineVisible = true;
            this.lblSeparadorPie.Location = new System.Drawing.Point(2, 216);
            this.lblSeparadorPie.Name = "lblSeparadorPie";
            this.lblSeparadorPie.Size = new System.Drawing.Size(311, 17);
            this.lblSeparadorPie.TabIndex = 54;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Image = global::SAGAS.ActivoFijo.Properties.Resources.Ok20;
            this.btnGuardar.Location = new System.Drawing.Point(5, 250);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(80, 28);
            this.btnGuardar.TabIndex = 61;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Image = global::SAGAS.ActivoFijo.Properties.Resources.cancel20;
            this.btnCancelar.Location = new System.Drawing.Point(85, 250);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(80, 28);
            this.btnCancelar.TabIndex = 62;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ckAplicaDepreciacion
            // 
            this.ckAplicaDepreciacion.Location = new System.Drawing.Point(12, 77);
            this.ckAplicaDepreciacion.Name = "ckAplicaDepreciacion";
            this.ckAplicaDepreciacion.Properties.Caption = "Aplica Depreciación";
            this.ckAplicaDepreciacion.Size = new System.Drawing.Size(136, 19);
            this.ckAplicaDepreciacion.TabIndex = 63;
            // 
            // radioTipo
            // 
            this.radioTipo.Location = new System.Drawing.Point(5, 102);
            this.radioTipo.Name = "radioTipo";
            this.radioTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Aplica a Proveedor"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Ninguno")});
            this.radioTipo.Size = new System.Drawing.Size(308, 51);
            this.radioTipo.TabIndex = 64;
            // 
            // radioEstado
            // 
            this.radioEstado.Location = new System.Drawing.Point(5, 159);
            this.radioEstado.Name = "radioEstado";
            this.radioEstado.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Es Alta")});
            this.radioEstado.Size = new System.Drawing.Size(308, 51);
            this.radioEstado.TabIndex = 65;
            // 
            // DialogTipoMovimientoActivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(317, 280);
            this.Controls.Add(this.radioEstado);
            this.Controls.Add(this.radioTipo);
            this.Controls.Add(this.ckAplicaDepreciacion);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblSeparadorPie);
            this.Controls.Add(this.lblNota);
            this.Controls.Add(this.lblDatosGenerales);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogTipoMovimientoActivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogZona_FormClosed);
            this.Load += new System.EventHandler(this.MyFormActivo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAplicaDepreciacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioEstado.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblNombre;
        private DevExpress.XtraEditors.TextEdit txtNombre;
        private DevExpress.XtraEditors.LabelControl lblDatosGenerales;
        private DevExpress.XtraEditors.LabelControl lblNota;
        private DevExpress.XtraEditors.LabelControl lblSeparadorPie;
        private DevExpress.XtraEditors.SimpleButton btnGuardar;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.CheckEdit ckAplicaDepreciacion;
        private DevExpress.XtraEditors.RadioGroup radioTipo;
        private DevExpress.XtraEditors.RadioGroup radioEstado;                     
    }
}
