namespace SAGAS.Administracion.Forms
{
    partial class FormTipoCuentaMovimiento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTipoCuentaMovimiento));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridTipo = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoMov = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCuenta = new DevExpress.XtraGrid.GridControl();
            this.gVdata2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Cuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTipo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gVdata2)).BeginInit();
            this.SuspendLayout();
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Excel";
            this.barButtonItem1.Id = 6;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "PDF";
            this.barButtonItem2.Id = 7;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 33);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridTipo);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridCuenta);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(804, 521);
            this.splitContainerControl1.SplitterPosition = 360;
            this.splitContainerControl1.TabIndex = 4;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridTipo
            // 
            this.gridTipo.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTipo.Location = new System.Drawing.Point(0, 0);
            this.gridTipo.MainView = this.gvData;
            this.gridTipo.MenuManager = this.barManager;
            this.gridTipo.Name = "gridTipo";
            this.gridTipo.Size = new System.Drawing.Size(360, 521);
            this.gridTipo.TabIndex = 0;
            this.gridTipo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colTipoMov});
            this.gvData.GridControl = this.gridTipo;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "MovimientoTipoID";
            this.colID.Name = "colID";
            // 
            // colTipoMov
            // 
            this.colTipoMov.Caption = "Tipo Movimiento";
            this.colTipoMov.FieldName = "Nombre";
            this.colTipoMov.Name = "colTipoMov";
            this.colTipoMov.Visible = true;
            this.colTipoMov.VisibleIndex = 0;
            // 
            // gridCuenta
            // 
            this.gridCuenta.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridCuenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCuenta.Location = new System.Drawing.Point(0, 0);
            this.gridCuenta.MainView = this.gVdata2;
            this.gridCuenta.MenuManager = this.barManager;
            this.gridCuenta.Name = "gridCuenta";
            this.gridCuenta.Size = new System.Drawing.Size(440, 521);
            this.gridCuenta.TabIndex = 0;
            this.gridCuenta.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gVdata2});
            // 
            // gVdata2
            // 
            this.gVdata2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ID,
            this.Cuenta});
            this.gVdata2.GridControl = this.gridCuenta;
            this.gVdata2.Name = "gVdata2";
            this.gVdata2.OptionsBehavior.Editable = false;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.FieldName = "MovimientoTipoID";
            this.ID.Name = "ID";
            // 
            // Cuenta
            // 
            this.Cuenta.Caption = "Nombre";
            this.Cuenta.FieldName = "Nombre";
            this.Cuenta.Name = "Cuenta";
            this.Cuenta.Visible = true;
            this.Cuenta.VisibleIndex = 0;
            // 
            // FormTipoCuentaMovimiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTipoCuentaMovimiento";
            this.Text = "Tipo de cuenta";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTipo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gVdata2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridTipo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.GridControl gridCuenta;
        private DevExpress.XtraGrid.Views.Grid.GridView gVdata2;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoMov;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraGrid.Columns.GridColumn Cuenta;
    }
}

