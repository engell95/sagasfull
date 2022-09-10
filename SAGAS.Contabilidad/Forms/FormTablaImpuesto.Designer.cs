﻿namespace SAGAS.Contabilidad.Forms
{
    partial class FormTablaImpuesto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTablaImpuesto));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigoCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPorcentaje = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComentario = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
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
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colCodigoCuenta,
            this.colNombreCuenta,
            this.colPorcentaje,
            this.colComentario});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigoCuenta
            // 
            this.colCodigoCuenta.Caption = "Codigo de Cuenta";
            this.colCodigoCuenta.FieldName = "CuentaCodigo";
            this.colCodigoCuenta.Name = "colCodigoCuenta";
            this.colCodigoCuenta.Visible = true;
            this.colCodigoCuenta.VisibleIndex = 0;
            this.colCodigoCuenta.Width = 103;
            // 
            // colNombreCuenta
            // 
            this.colNombreCuenta.Caption = "Nombre de Cuenta";
            this.colNombreCuenta.FieldName = "CuentaNombre";
            this.colNombreCuenta.Name = "colNombreCuenta";
            this.colNombreCuenta.Visible = true;
            this.colNombreCuenta.VisibleIndex = 1;
            this.colNombreCuenta.Width = 246;
            // 
            // colPorcentaje
            // 
            this.colPorcentaje.Caption = "Porcentaje";
            this.colPorcentaje.FieldName = "Porcentaje";
            this.colPorcentaje.Name = "colPorcentaje";
            this.colPorcentaje.Visible = true;
            this.colPorcentaje.VisibleIndex = 2;
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 3;
            this.colComentario.Width = 427;
            // 
            // FormTablaImpuesto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTablaImpuesto";
            this.Text = "Lista de Impuestos";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colComentario;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn colPorcentaje;
    }
}

