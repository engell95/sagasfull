namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    partial class DialogActivo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogActivo));
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblNombre = new DevExpress.XtraEditors.LabelControl();
            this.lblDescripcion = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.TextEdit();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.mmoDescripcion = new DevExpress.XtraEditors.MemoEdit();
            this.lblNota = new DevExpress.XtraEditors.LabelControl();
            this.btnGuardar = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.lblTipoActivo = new DevExpress.XtraEditors.LabelControl();
            this.glkTipoActivo = new DevExpress.XtraEditors.GridLookUpEdit();
            this.glkvTipoActivo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TipoActivoNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TipoActivoDisplay = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkTipoActivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvTipoActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCodigo
            // 
            this.lblCodigo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigo.Location = new System.Drawing.Point(5, 26);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(38, 13);
            this.lblCodigo.TabIndex = 1;
            this.lblCodigo.Text = "Código";
            // 
            // lblNombre
            // 
            this.lblNombre.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(5, 71);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(44, 13);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre";
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.Location = new System.Drawing.Point(5, 157);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(54, 13);
            this.lblDescripcion.TabIndex = 3;
            this.lblDescripcion.Text = "Descripción";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(5, 45);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.DisplayFormat.FormatString = "{0:000}";
            this.txtCodigo.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCodigo.Properties.EditFormat.FormatString = "{0:000}";
            this.txtCodigo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtCodigo.Properties.Mask.EditMask = "999";
            this.txtCodigo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;
            this.txtCodigo.Size = new System.Drawing.Size(146, 20);
            this.txtCodigo.TabIndex = 0;
            this.txtCodigo.ToolTip = "Codigo del bien";
            this.txtCodigo.Validated += new System.EventHandler(this.txtCodigo_Validated);
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(5, 89);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(308, 20);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.ToolTip = "Nombre del bien";
            this.txtNombre.Validated += new System.EventHandler(this.txtCodigo_Validated);
            // 
            // mmoDescripcion
            // 
            this.mmoDescripcion.Location = new System.Drawing.Point(5, 176);
            this.mmoDescripcion.Name = "mmoDescripcion";
            this.mmoDescripcion.Size = new System.Drawing.Size(308, 64);
            this.mmoDescripcion.TabIndex = 3;
            this.mmoDescripcion.UseOptimizedRendering = true;
            // 
            // lblNota
            // 
            this.lblNota.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNota.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNota.Location = new System.Drawing.Point(0, 256);
            this.lblNota.Name = "lblNota";
            this.lblNota.Size = new System.Drawing.Size(252, 13);
            this.lblNota.TabIndex = 1;
            this.lblNota.Text = "Nota: Los campos en negritas son requeridos";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnGuardar.Image = global::SAGAS.ActivoFijo.Properties.Resources.Ok20;
            this.btnGuardar.Location = new System.Drawing.Point(2, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 30);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancelar.Image = global::SAGAS.ActivoFijo.Properties.Resources.cancel20;
            this.btnCancelar.Location = new System.Drawing.Point(77, 2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(82, 30);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblTipoActivo
            // 
            this.lblTipoActivo.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoActivo.Location = new System.Drawing.Point(5, 115);
            this.lblTipoActivo.Name = "lblTipoActivo";
            this.lblTipoActivo.Size = new System.Drawing.Size(63, 13);
            this.lblTipoActivo.TabIndex = 63;
            this.lblTipoActivo.Text = "Tipo Activo";
            // 
            // glkTipoActivo
            // 
            this.glkTipoActivo.Location = new System.Drawing.Point(5, 131);
            this.glkTipoActivo.Name = "glkTipoActivo";
            this.glkTipoActivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.glkTipoActivo.Properties.DisplayMember = "Display";
            this.glkTipoActivo.Properties.ValueMember = "ID";
            this.glkTipoActivo.Properties.View = this.glkvTipoActivo;
            this.glkTipoActivo.Size = new System.Drawing.Size(308, 20);
            this.glkTipoActivo.TabIndex = 2;
            this.glkTipoActivo.Validated += new System.EventHandler(this.txtCodigo_Validated);
            // 
            // glkvTipoActivo
            // 
            this.glkvTipoActivo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.TipoActivoNombre,
            this.TipoActivoDisplay});
            this.glkvTipoActivo.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.glkvTipoActivo.Name = "glkvTipoActivo";
            this.glkvTipoActivo.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.glkvTipoActivo.OptionsView.ShowGroupPanel = false;
            // 
            // TipoActivoNombre
            // 
            this.TipoActivoNombre.AppearanceCell.Options.UseTextOptions = true;
            this.TipoActivoNombre.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.TipoActivoNombre.Caption = "Codigo";
            this.TipoActivoNombre.DisplayFormat.FormatString = "{0:00}";
            this.TipoActivoNombre.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.TipoActivoNombre.FieldName = "Codigo";
            this.TipoActivoNombre.Name = "TipoActivoNombre";
            this.TipoActivoNombre.Visible = true;
            this.TipoActivoNombre.VisibleIndex = 0;
            // 
            // TipoActivoDisplay
            // 
            this.TipoActivoDisplay.Caption = "Nombre";
            this.TipoActivoDisplay.FieldName = "Nombre";
            this.TipoActivoDisplay.Name = "TipoActivoDisplay";
            this.TipoActivoDisplay.Visible = true;
            this.TipoActivoDisplay.VisibleIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblCodigo);
            this.groupControl1.Controls.Add(this.glkTipoActivo);
            this.groupControl1.Controls.Add(this.mmoDescripcion);
            this.groupControl1.Controls.Add(this.lblTipoActivo);
            this.groupControl1.Controls.Add(this.lblNombre);
            this.groupControl1.Controls.Add(this.lblDescripcion);
            this.groupControl1.Controls.Add(this.txtCodigo);
            this.groupControl1.Controls.Add(this.txtNombre);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(334, 256);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Datos Generales";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancelar);
            this.panelControl1.Controls.Add(this.btnGuardar);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 269);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(334, 34);
            this.panelControl1.TabIndex = 1;
            // 
            // DialogActivo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 303);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.lblNota);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogActivo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogActivo_FormClosed);
            this.Load += new System.EventHandler(this.MyFormActivo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mmoDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkTipoActivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.glkvTipoActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblNombre;
        private DevExpress.XtraEditors.LabelControl lblDescripcion;
        private DevExpress.XtraEditors.TextEdit txtCodigo;
        private DevExpress.XtraEditors.TextEdit txtNombre;
        private DevExpress.XtraEditors.MemoEdit mmoDescripcion;
        private DevExpress.XtraEditors.LabelControl lblNota;
        private DevExpress.XtraEditors.SimpleButton btnGuardar;
        private DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.LabelControl lblTipoActivo;
        private DevExpress.XtraEditors.GridLookUpEdit glkTipoActivo;
        private DevExpress.XtraGrid.Views.Grid.GridView glkvTipoActivo;
        private DevExpress.XtraGrid.Columns.GridColumn TipoActivoNombre;
        private DevExpress.XtraGrid.Columns.GridColumn TipoActivoDisplay;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}
