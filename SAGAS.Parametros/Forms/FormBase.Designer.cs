namespace SAGAS.Parametros.Forms
{
    partial class FormBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBase));
            this.barManager = new DevExpress.XtraBars.BarManager();
            this.barTop = new DevExpress.XtraBars.Bar();
            this.btnAgregar = new DevExpress.XtraBars.BarButtonItem();
            this.btnModificar = new DevExpress.XtraBars.BarButtonItem();
            this.btnAnular = new DevExpress.XtraBars.BarButtonItem();
            this.barRefresh = new DevExpress.XtraBars.BarButtonItem();
            this.barLimpiar = new DevExpress.XtraBars.BarButtonItem();
            this.barImprimir = new DevExpress.XtraBars.BarButtonItem();
            this.barExportar = new DevExpress.XtraBars.BarSubItem();
            this.barExcel = new DevExpress.XtraBars.BarButtonItem();
            this.barPDF = new DevExpress.XtraBars.BarButtonItem();
            this.barSalir = new DevExpress.XtraBars.BarButtonItem();
            this.barMessage = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.bdsManejadorDatos = new System.Windows.Forms.BindingSource();
            this.dglExportToFile = new System.Windows.Forms.SaveFileDialog();
            this.timerMSG = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barTop});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAgregar,
            this.btnModificar,
            this.btnAnular,
            this.barImprimir,
            this.barExportar,
            this.barExcel,
            this.barPDF,
            this.barSalir,
            this.barMessage,
            this.barLimpiar,
            this.barRefresh});
            this.barManager.MaxItemId = 13;
            // 
            // barTop
            // 
            this.barTop.BarName = "Tools";
            this.barTop.DockCol = 0;
            this.barTop.DockRow = 0;
            this.barTop.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barTop.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAgregar),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnModificar),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAnular),
            new DevExpress.XtraBars.LinkPersistInfo(this.barRefresh, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barLimpiar),
            new DevExpress.XtraBars.LinkPersistInfo(this.barImprimir, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barExportar),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSalir, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barMessage)});
            this.barTop.OptionsBar.AllowQuickCustomization = false;
            this.barTop.OptionsBar.DrawDragBorder = false;
            this.barTop.OptionsBar.UseWholeRow = true;
            this.barTop.Text = "Tools";
            // 
            // btnAgregar
            // 
            this.btnAgregar.Caption = "Agregar";
            this.btnAgregar.Glyph = global::SAGAS.Parametros.Properties.Resources.add24;
            this.btnAgregar.Id = 0;
            this.btnAgregar.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F2);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnAgregar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAgregar_ItemClick);
            // 
            // btnModificar
            // 
            this.btnModificar.Caption = "Modificar";
            this.btnModificar.Glyph = global::SAGAS.Parametros.Properties.Resources.modificar24;
            this.btnModificar.Id = 1;
            this.btnModificar.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F3);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnModificar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnModificar_ItemClick);
            // 
            // btnAnular
            // 
            this.btnAnular.Caption = "<>";
            this.btnAnular.Glyph = global::SAGAS.Parametros.Properties.Resources.Anular24;
            this.btnAnular.Id = 2;
            this.btnAnular.Name = "btnAnular";
            this.btnAnular.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.btnAnular.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAnular_ItemClick);
            // 
            // barRefresh
            // 
            this.barRefresh.Caption = "Actualizar";
            this.barRefresh.Glyph = global::SAGAS.Parametros.Properties.Resources.refresh24;
            this.barRefresh.Id = 12;
            this.barRefresh.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F5);
            this.barRefresh.Name = "barRefresh";
            this.barRefresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barRefresh_ItemClick);
            // 
            // barLimpiar
            // 
            this.barLimpiar.Caption = "Borrar Busqueda";
            this.barLimpiar.Glyph = global::SAGAS.Parametros.Properties.Resources.filterDelete24;
            this.barLimpiar.Id = 11;
            this.barLimpiar.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F12);
            this.barLimpiar.Name = "barLimpiar";
            this.barLimpiar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barLimpiar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLimpiar_ItemClick);
            // 
            // barImprimir
            // 
            this.barImprimir.Caption = "Imprimir";
            this.barImprimir.Glyph = global::SAGAS.Parametros.Properties.Resources.print24;
            this.barImprimir.Id = 3;
            this.barImprimir.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F9);
            this.barImprimir.Name = "barImprimir";
            this.barImprimir.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barImprimir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barImprimir_ItemClick);
            // 
            // barExportar
            // 
            this.barExportar.Caption = "Exportar";
            this.barExportar.Glyph = global::SAGAS.Parametros.Properties.Resources.exportar24;
            this.barExportar.Id = 6;
            this.barExportar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barExcel),
            new DevExpress.XtraBars.LinkPersistInfo(this.barPDF)});
            this.barExportar.Name = "barExportar";
            this.barExportar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // barExcel
            // 
            this.barExcel.Caption = "Excel";
            this.barExcel.Glyph = ((System.Drawing.Image)(resources.GetObject("barExcel.Glyph")));
            this.barExcel.Id = 7;
            this.barExcel.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F10);
            this.barExcel.Name = "barExcel";
            this.barExcel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barExcel_ItemClick);
            // 
            // barPDF
            // 
            this.barPDF.Caption = "PDF";
            this.barPDF.Glyph = global::SAGAS.Parametros.Properties.Resources.PDF24;
            this.barPDF.Id = 8;
            this.barPDF.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F11);
            this.barPDF.Name = "barPDF";
            this.barPDF.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barPDF.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barPDF_ItemClick);
            // 
            // barSalir
            // 
            this.barSalir.Caption = "Salir";
            this.barSalir.Glyph = global::SAGAS.Parametros.Properties.Resources.Salir24;
            this.barSalir.Id = 9;
            this.barSalir.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.barSalir.Name = "barSalir";
            this.barSalir.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barSalir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barSalir_ItemClick);
            // 
            // barMessage
            // 
            this.barMessage.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barMessage.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.barMessage.Id = 10;
            this.barMessage.Name = "barMessage";
            this.barMessage.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barMessage.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(804, 33);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 554);
            this.barDockControlBottom.Size = new System.Drawing.Size(804, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 33);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 521);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(804, 33);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 521);
            // 
            // timerMSG
            // 
            this.timerMSG.Tick += new System.EventHandler(this.timerMSG_Tick);
            // 
            // FormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBase";
            this.Text = "FormBase";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        public DevExpress.XtraBars.BarButtonItem btnAgregar;
        public DevExpress.XtraBars.BarButtonItem btnModificar;
        public DevExpress.XtraBars.BarButtonItem btnAnular;
        public DevExpress.XtraBars.BarManager barManager;
        public System.Windows.Forms.BindingSource bdsManejadorDatos;
        public DevExpress.XtraBars.BarButtonItem barImprimir;
        public DevExpress.XtraBars.BarSubItem barExportar;
        public DevExpress.XtraBars.BarButtonItem barExcel;
        public DevExpress.XtraBars.BarButtonItem barPDF;
        private DevExpress.XtraBars.BarButtonItem barSalir;
        public DevExpress.XtraBars.BarStaticItem barMessage;
        public DevExpress.XtraBars.Bar barTop;
        public System.Windows.Forms.SaveFileDialog dglExportToFile;
        public System.Windows.Forms.Timer timerMSG;
        public DevExpress.XtraBars.BarButtonItem barLimpiar;
        public DevExpress.XtraBars.BarButtonItem barRefresh;
    }
}