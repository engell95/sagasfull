namespace SAGAS.Contabilidad.Forms
{
    partial class FormTipoCuenta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTipoCuenta));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.treeTipoCuenta = new DevExpress.XtraTreeList.TreeList();
            this.colID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colIDPadre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNombre = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colNaturalezaDeudora = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colEfectoCero = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colUsaCentroCosto = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colComentario = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colActivo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colBalance = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colResultado = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrden = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colGrupo = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeTipoCuenta)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            this.SuspendLayout();
            // 
            // barExportar
            // 
            this.barExportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barExportar.ImageOptions.Image")));
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
            // treeTipoCuenta
            // 
            this.treeTipoCuenta.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colID,
            this.colIDPadre,
            this.colNombre,
            this.colNaturalezaDeudora,
            this.colEfectoCero,
            this.colUsaCentroCosto,
            this.colComentario,
            this.colActivo,
            this.colBalance,
            this.colResultado,
            this.colOrden,
            this.colGrupo});
            this.treeTipoCuenta.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeTipoCuenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTipoCuenta.Location = new System.Drawing.Point(0, 39);
            this.treeTipoCuenta.Name = "treeTipoCuenta";
            this.treeTipoCuenta.OptionsBehavior.Editable = false;
            this.treeTipoCuenta.OptionsBehavior.PopulateServiceColumns = true;
            this.treeTipoCuenta.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
            this.treeTipoCuenta.OptionsNavigation.AutoMoveRowFocus = true;
            this.treeTipoCuenta.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeTipoCuenta.OptionsView.EnableAppearanceEvenRow = true;
            this.treeTipoCuenta.OptionsView.EnableAppearanceOddRow = true;
            this.treeTipoCuenta.OptionsView.ShowAutoFilterRow = true;
            this.treeTipoCuenta.OptionsView.ShowHorzLines = false;
            this.treeTipoCuenta.ParentFieldName = "IdPadre";
            this.treeTipoCuenta.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rchkActivo});
            this.treeTipoCuenta.Size = new System.Drawing.Size(804, 515);
            this.treeTipoCuenta.TabIndex = 4;
            this.treeTipoCuenta.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeTipoCuenta_FocusedNodeChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colIDPadre
            // 
            this.colIDPadre.FieldName = "IdPadre";
            this.colIDPadre.Name = "colIDPadre";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 184;
            // 
            // colNaturalezaDeudora
            // 
            this.colNaturalezaDeudora.Caption = "Naturaleza Deudora";
            this.colNaturalezaDeudora.FieldName = "NaturalezaDeudora";
            this.colNaturalezaDeudora.Name = "colNaturalezaDeudora";
            this.colNaturalezaDeudora.Visible = true;
            this.colNaturalezaDeudora.VisibleIndex = 1;
            this.colNaturalezaDeudora.Width = 113;
            // 
            // colEfectoCero
            // 
            this.colEfectoCero.Caption = "Efecto Cero";
            this.colEfectoCero.FieldName = "EfectoCero";
            this.colEfectoCero.Name = "colEfectoCero";
            this.colEfectoCero.Visible = true;
            this.colEfectoCero.VisibleIndex = 2;
            this.colEfectoCero.Width = 70;
            // 
            // colUsaCentroCosto
            // 
            this.colUsaCentroCosto.Caption = "Usa Centro Costo";
            this.colUsaCentroCosto.FieldName = "UsaCentroCosto";
            this.colUsaCentroCosto.Name = "colUsaCentroCosto";
            this.colUsaCentroCosto.Visible = true;
            this.colUsaCentroCosto.VisibleIndex = 3;
            this.colUsaCentroCosto.Width = 99;
            // 
            // colComentario
            // 
            this.colComentario.Caption = "Comentario";
            this.colComentario.FieldName = "Comentario";
            this.colComentario.Name = "colComentario";
            this.colComentario.Visible = true;
            this.colComentario.VisibleIndex = 4;
            this.colComentario.Width = 262;
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rchkActivo;
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.OptionsColumn.AllowEdit = false;
            this.colActivo.OptionsColumn.ReadOnly = true;
            this.colActivo.Visible = true;
            this.colActivo.VisibleIndex = 5;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // colBalance
            // 
            this.colBalance.Caption = "Cuenta Balance";
            this.colBalance.FieldName = "Cuenta Balance";
            this.colBalance.Name = "colBalance";
            this.colBalance.Visible = true;
            this.colBalance.VisibleIndex = 6;
            // 
            // colResultado
            // 
            this.colResultado.Caption = "Cuente Resultado";
            this.colResultado.FieldName = "Cuente Resultado";
            this.colResultado.Name = "colResultado";
            this.colResultado.Visible = true;
            this.colResultado.VisibleIndex = 7;
            // 
            // colOrden
            // 
            this.colOrden.Caption = "Orden";
            this.colOrden.FieldName = "Orden";
            this.colOrden.Name = "colOrden";
            this.colOrden.Visible = true;
            this.colOrden.VisibleIndex = 8;
            // 
            // colGrupo
            // 
            this.colGrupo.Caption = "GrupoID";
            this.colGrupo.FieldName = "GrupoID";
            this.colGrupo.Name = "colGrupo";
            this.colGrupo.Visible = true;
            this.colGrupo.VisibleIndex = 9;
            // 
            // FormTipoCuenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.treeTipoCuenta);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormTipoCuenta";
            this.Text = "Lista de Tipos de Cuentas";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.treeTipoCuenta, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeTipoCuenta)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraTreeList.TreeList treeTipoCuenta;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colID;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colIDPadre;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNombre;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colNaturalezaDeudora;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colEfectoCero;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colComentario;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colUsaCentroCosto;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBalance;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colResultado;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrden;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colGrupo;
    }
}

