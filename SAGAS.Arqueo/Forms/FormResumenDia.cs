using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraBars;
using System.Text.RegularExpressions;	

namespace SAGAS.Arqueo.Forms
{                                
    public partial class FormResumenDia : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogArqueoIsla nf;
        private Forms.Dialogs.DialogCambioLecturas ncl;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private Entidad.ResumenDia RD;
        private Entidad.Turno T;
        private Entidad.ArqueoIsla AI;
        private Entidad.ArqueoEfectivo AE;
        private Parametros.TiposArqueo TipoArqueo;
        private DevExpress.XtraBars.Bar barItems;
        private bool btnDesaprobar = false;
        private bool btnsGeneralRD = false;
        private bool btnsReimpresionRD = false;
        private bool btnCambiarLecturasArqueo = false;

        #endregion

        #region <<< INICIO >>>

        public FormResumenDia()
        {
            InitializeComponent();
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barTop.DockCol = 1; 
            this.barTop.OptionsBar.UseWholeRow = false;
            barItems = new DevExpress.XtraBars.Bar(barManager);
            barItems.OptionsBar.AllowQuickCustomization = false;
            barItems.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            //Accesos Especificos
            this.btnDesaprobar = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnDesaprobar");
            this.btnsGeneralRD = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnsGeneralRD");
            this.btnsReimpresionRD = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnsReimpresionRD");
            this.btnCambiarLecturasArqueo = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnCambiarLecturasArqueo");
            
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl(false);
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl(bool Refresh)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());


                bdsManejadorDatos.DataSource = from RD in db.ResumenDias
                                               where (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == RD.EstacionServicioID))
                                               orderby RD.FechaInicial descending
                                                select RD;                                                              
                //**Estaciones de Servicio
                rpEstacionServicio.DataSource = from es in db.EstacionServicios
                                                where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                                select new { es.ID, es.Nombre };

                rpEstacionServicio.DisplayMember = "Nombre";
                rpEstacionServicio.ValueMember = "ID";

                //if (Parametros.General.ListSES.Count <= 0)
                //    this.colSUS.Visible = false;

                //**SubEstacionServicio
                lkSES.DataSource = db.SubEstacions.Where(sus => sus.Activo).Select(s => new { s.ID, s.Nombre });
                lkSES.DisplayMember = "Nombre";
                lkSES.ValueMember = "ID";


                //**Islas
                rpIslas.DataSource = from I in db.Islas
                                     where I.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == I.EstacionServicioID))
                                     select new { I.ID, I.Nombre };

                rpIslas.DisplayMember = "Nombre";
                rpIslas.ValueMember = "ID";

                //**Tecnicos
                IQueryable<Parametros.ListIdDisplay> listEm = ((from em in db.Empleados
                                                                join pl in db.Planillas on em.PlanillaID equals pl.ID
                                                                where (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == pl.EstacionServicioID))
                                                                select new Parametros.ListIdDisplay { ID = em.ID, Display = em.Nombres + " " + em.Apellidos }).OrderBy(o => o.Display));

                List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>(listEm);

                listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Isla Cerrada"));

                rpTecnico.DataSource = listadoDisplay;
                rpTecnico.DisplayMember = "Display";
                rpTecnico.ValueMember = "ID";

                //INVENTARIO FISICO
                rpTanqueIR.DataSource = db.Tanques.Select(s => new { s.ID, s.Nombre });
                rpProductoIR.DataSource = db.Productos.Where( o => o.ProductoClaseID.Equals(1)).Select(s => new { s.ID, s.Nombre });

                //permiso para editar los litros en el inventario físico
              gvDataIR.OptionsBehavior.Editable = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "rpLitrosInventarioResumen"); 

                //**Estados
                Parametros.ListEstados listado = new Parametros.ListEstados();
                rpEstado.DataSource = listado.GetListEstadosArqueo();
                rpEstado.DisplayMember = "Name";
                rpEstado.ValueMember = "ID";
                rpEstado.Columns.Clear();
                this.rpEstado.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
                new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Estado")});

                if (!Refresh)
                {
                    gridRD.ForceInitialize();
                    gvDataRD.FocusedColumn = this.gvDataRD.VisibleColumns[0];
                    gvDataRD.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
                    DateTime now = DateTime.Now;
                    DateTime firstDay = new DateTime(now.Year, now.Month, 1);
                    if (gvDataRD.RowCount > 0)
                    {
                        DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                        gvDataRD.ActiveFilterString = (new OperandProperty("FechaInicial") > fecha.AddDays(-1)).ToString();
                        //"[FechaInicial] >= #28/02/2014#";
                        //"[FechaInicial] > #" + fecha.AddDays(-1).ToShortDateString() + "#";
                    }
                    //Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID == IDES).OrderByDescending(o => o.ID).First().FechaInicial).ToString("dd/MM/yyyy") + "'";
                    gvDataRD.ShowEditor();
                }
                
                //MessageBox.Show(gvDataRD.GetFocusedRowCellValue("Numero").ToString() + "  " + gvDataRD.GetFocusedRowCellValue("FechaInicial").ToString());

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(gridRD);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(gridRD);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(gridRD);
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;
            ncl = null;
            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl(true);

        }

        protected override void CleanFilter()
        {
            this.gvDataRD.ActiveFilter.Clear();
        }  
                      
        #endregion    
        
        //RESUMEN DEL DIA
        private void gvDataRD_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;

                if (view.FocusedRowHandle >= 0)
                {
                    RD = db.ResumenDias.Single(rd => rd.ID.Equals(Convert.ToInt32(gvDataRD.GetFocusedRowCellValue(colIDRD))));

                    barItems.ClearLinks();

                    //Boton Vista Previa
                    DevExpress.XtraBars.BarButtonItem btnpreview = new DevExpress.XtraBars.BarButtonItem();
                    btnpreview.Caption = "Vista Previa Resumen del Día Nro. " + RD.Numero.ToString();
                    btnpreview.Glyph = Properties.Resources.zoom;
                    btnpreview.Id = 1;
                    btnpreview.Name = "btnpreview";
                    btnpreview.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    btnpreview.Tag = RD;
                    btnpreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnpreview_ItemClick);
                    barItems.AddItem(btnpreview);
                    //--

                    if (!RD.Aprobado && btnsGeneralRD)
                    {
                        //Boton Aprobar Resumen
                        DevExpress.XtraBars.BarButtonItem btnAprobar = new DevExpress.XtraBars.BarButtonItem();
                        btnAprobar.Caption = "Aprobar Resumen del Día Nro. " + RD.Numero.ToString();
                        btnAprobar.Glyph = Properties.Resources.AprobarRD;
                        btnAprobar.Id = 0;
                        btnAprobar.Name = "btnAprobar";
                        btnAprobar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnAprobar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAprobar_ItemClick);
                        btnAprobar.Tag = RD;
                        barItems.AddItem(btnAprobar);
                        //--
                    }
                    else if (RD.Aprobado)
                    {
                        //Boton Vista Previa Arqueo de Efectivo
                        DevExpress.XtraBars.BarButtonItem btnpreviewAE = new DevExpress.XtraBars.BarButtonItem();
                        btnpreviewAE.Caption = "Vista Previa Resumen Efectivo del Día Nro. " + RD.Numero.ToString();
                        btnpreviewAE.Glyph = Properties.Resources.zoom_fit;
                        btnpreviewAE.Id = 1;
                        btnpreviewAE.Name = "btnpreviewAE";
                        btnpreviewAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnpreviewAE.Tag = RD;
                        btnpreviewAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnpreviewRE_ItemClick);
                        barItems.AddItem(btnpreviewAE);
                        //--

                        if (btnsReimpresionRD || btnsGeneralRD)
                        {
                            //Boton Reimpresion
                            DevExpress.XtraBars.BarButtonItem btnReprint = new DevExpress.XtraBars.BarButtonItem();
                            btnReprint.Caption = "Reimpresion Resumen del Día Nro. " + RD.Numero.ToString();
                            btnReprint.Glyph = Properties.Resources.printer;
                            btnReprint.Id = 2;
                            btnReprint.Name = "btnReprint";
                            btnReprint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnReprint.Tag = RD;
                            btnReprint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReprint_ItemClick);
                            barItems.AddItem(btnReprint);
                            //--

                            //Boton Re Impresion Arqueo Efectivo
                            DevExpress.XtraBars.BarButtonItem btnprePrintAE = new DevExpress.XtraBars.BarButtonItem();
                            btnprePrintAE.Caption = "Re-Impresión Resumen Efectivo del Día Nro. " + RD.Numero.ToString();
                            btnprePrintAE.Glyph = Properties.Resources.printer;
                            btnprePrintAE.Id = 1;
                            btnprePrintAE.Name = "btnprePrintAE";
                            btnprePrintAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnprePrintAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnprePrintAE_ItemClick);
                            barItems.AddItem(btnprePrintAE);
                            //--
                        }

                        if (btnDesaprobar)
                        {
                            //Boton Desaprobar Resumen
                            DevExpress.XtraBars.BarButtonItem btnItemDesaprobar = new DevExpress.XtraBars.BarButtonItem();
                            btnItemDesaprobar.Caption = "Desaprobar Resumen del Día Nro. " + RD.Numero.ToString();
                            btnItemDesaprobar.Glyph = Properties.Resources.DesaprobarRD;
                            btnItemDesaprobar.Id = 0;
                            btnItemDesaprobar.Name = "btnItemDesaprobar";
                            btnItemDesaprobar.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnItemDesaprobar.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemDesaprobar_ItemClick);
                            btnItemDesaprobar.Tag = RD;
                            barItems.AddItem(btnItemDesaprobar);
                            //--
                        }
                    }

                    TipoArqueo = Parametros.TiposArqueo.ResumenDia;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //RESUMEN DE TURNO
        private void gvDataTurno_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;

                if (view.FocusedRowHandle >= 0)
                {
                    T = db.Turnos.Single(t => t.ID == Convert.ToInt32(view.GetFocusedRowCellValue(ColIDT)));
                    RD = T.ResumenDia;
                    barItems.ClearLinks();

                    //Boton Vista Previa Resumen Turno
                    DevExpress.XtraBars.BarButtonItem btnPreview = new DevExpress.XtraBars.BarButtonItem();
                    btnPreview.Caption = "Vista Previa Resumen de Turno Nro. " + T.Numero.ToString();
                    btnPreview.Glyph = Properties.Resources.zoom;
                    btnPreview.Id = 0;
                    btnPreview.Name = "btnPreview";
                    btnPreview.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    btnPreview.Tag = T;
                    btnPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnpreview_ItemClick);
                    barItems.AddItem(btnPreview);
                    //--

                    if (T.Cerrada && (btnsGeneralRD || btnsReimpresionRD))
                    {
                        //Boton Reimpresion
                        DevExpress.XtraBars.BarButtonItem btnReprint = new DevExpress.XtraBars.BarButtonItem();
                        btnReprint.Caption = "Reimpresion Resumen de Turno Nro. " + T.Numero.ToString();
                        btnReprint.Glyph = Properties.Resources.printer;
                        btnReprint.Id = 2;
                        btnReprint.Name = "btnReprint";
                        btnReprint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnReprint.Tag = RD;
                        btnReprint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReprint_ItemClick);
                        barItems.AddItem(btnReprint);
                        //--
                    }

                    var objAE = db.ArqueoEfectivos.Where(ae => ae.TurnoID.Equals(T.ID));

                    if (objAE.Count() > 0)
                    {
                        if (objAE.First().Cerrado)
                        {
                            //Boton Vista Previa
                            AE = objAE.First();
                            DevExpress.XtraBars.BarButtonItem btnpreviewRE = new DevExpress.XtraBars.BarButtonItem();
                            btnpreviewRE.Caption = "Vista Previa Resumen de Efectivo";
                            btnpreviewRE.Glyph = Properties.Resources.zoom_fit;
                            btnpreviewRE.Id = 1;
                            btnpreviewRE.Name = "btnpreviewRE";
                            btnpreviewRE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnpreviewRE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnpreviewRE_ItemClick);
                            barItems.AddItem(btnpreviewRE);
                            //--

                            if (btnsGeneralRD || btnsReimpresionRD)
                            {
                                //Boton Re Impresrion Arqueo Efectivo
                                AE = objAE.First();
                                DevExpress.XtraBars.BarButtonItem btnprePrintAE = new DevExpress.XtraBars.BarButtonItem();
                                btnprePrintAE.Caption = "Re-Impresión Resumen de Efectivo";
                                btnprePrintAE.Glyph = Properties.Resources.printer;
                                btnprePrintAE.Id = 1;
                                btnprePrintAE.Name = "btnprePrintAE";
                                btnprePrintAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                                btnprePrintAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnprePrintAE_ItemClick);
                                barItems.AddItem(btnprePrintAE);
                                //--
                            }
                        }
                    }

                    if (objAE.Count() <= 0 && !T.Cerrada && T.Especial)
                    {
                        if (btnsGeneralRD)
                        {
                            //Boton Abrir Arqueo Efectivo
                            DevExpress.XtraBars.BarButtonItem btnCrearAE = new DevExpress.XtraBars.BarButtonItem();
                            btnCrearAE.Caption = "Abrir Arqueo de Efectivo";
                            btnCrearAE.Glyph = Properties.Resources.OpenTurno;
                            btnCrearAE.Id = 1;
                            btnCrearAE.Name = "btnCrearAE";
                            btnCrearAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnCrearAE.Tag = T;
                            btnCrearAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCrearAE_ItemClick);
                            barItems.AddItem(btnCrearAE);
                            //--
                        }
                    }

                    if (T.Cerrada && !RD.Aprobado && btnsGeneralRD)
                    {
                        //Boton Abrir Turno
                        DevExpress.XtraBars.BarButtonItem btnOpenT = new DevExpress.XtraBars.BarButtonItem();
                        btnOpenT.Caption = "Abrir Resumen de Turno Nro. " + T.Numero.ToString();
                        btnOpenT.Glyph = Properties.Resources.OpenTurno;
                        btnOpenT.Id = 1;
                        btnOpenT.Name = "btnpreviewRE";
                        btnOpenT.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnOpenT.Tag = T;
                        btnOpenT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenT_ItemClick);
                        barItems.AddItem(btnOpenT);
                        //--
                    }
                    else if (!T.Cerrada && !RD.Aprobado && btnsGeneralRD)
                    {

                        //Boton Cerrar Turno
                        DevExpress.XtraBars.BarButtonItem btnCloseT = new DevExpress.XtraBars.BarButtonItem();
                        btnCloseT.Caption = "Cerrar Resumen de Turno Nro. " + T.Numero.ToString();
                        btnCloseT.Glyph = Properties.Resources.inbox_document;
                        btnCloseT.Id = 1;
                        btnCloseT.Name = "btnCloseT";
                        btnCloseT.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnCloseT.Tag = T;
                        btnCloseT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCloseT_ItemClick);
                        barItems.AddItem(btnCloseT);
                        //--

                        if (objAE.Count() > 0)
                        {
                            if (objAE.First().Cerrado && btnsGeneralRD)
                            {
                                //Boton Abrir Arqueo Efectivo
                                DevExpress.XtraBars.BarButtonItem btnOpenAE = new DevExpress.XtraBars.BarButtonItem();
                                btnOpenAE.Caption = "Abrir Arqueo de Efectivo";
                                btnOpenAE.Glyph = Properties.Resources.money_bag_label;
                                btnOpenAE.Id = 1;
                                btnOpenAE.Name = "btnOpenAE";
                                btnOpenAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                                btnOpenAE.Tag = T;
                                btnOpenAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenAE_ItemClick);
                                barItems.AddItem(btnOpenAE);
                                //--
                            }
                            else if (!objAE.First().Cerrado && btnsGeneralRD)
                            {
                                //Boton Editar Arqueo Efectivo
                                DevExpress.XtraBars.BarButtonItem btnEditAE = new DevExpress.XtraBars.BarButtonItem();
                                btnEditAE.Caption = "Editar Arqueo de Efectivo";
                                btnEditAE.Glyph = Properties.Resources.money__arrow;
                                btnEditAE.Id = 1;
                                btnEditAE.Name = "btnEditAE";
                                btnEditAE.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                                btnEditAE.Tag = T;
                                btnEditAE.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
                                barItems.AddItem(btnEditAE);
                                //--
                            }
                        }
                    }

                    TipoArqueo = Parametros.TiposArqueo.Turno;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            } 
        }

        //ARQUEO DE ISLA
        private void gvDataAI_Click(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;

                if (view.FocusedRowHandle >= 0)
                {
                    AI = db.ArqueoIslas.Single(ai => ai.ID == Convert.ToInt32(view.GetFocusedRowCellValue(ColIDAI)));
                    T = AI.Turno;
                    RD = AI.Turno.ResumenDia;

                    barItems.ClearLinks();

                    //Boton Vista Previa Arqueo
                    DevExpress.XtraBars.BarButtonItem btnPreview = new DevExpress.XtraBars.BarButtonItem();
                    btnPreview.Caption = "Vista Previa Arqueo Nro " + AI.Numero.ToString();
                    btnPreview.Glyph = Properties.Resources.zoom;
                    btnPreview.Id = 0;
                    btnPreview.Name = "btnPreview";
                    btnPreview.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                    btnPreview.Tag = AI;
                    btnPreview.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnpreview_ItemClick);
                    barItems.AddItem(btnPreview);
                    //--

                    if (AI.Cerrado && AI.Estado.Equals((int)Parametros.Estados.Cerrado))
                    {
                        if (btnsReimpresionRD || btnsGeneralRD)
                        {
                            //Boton Reimpresion
                            DevExpress.XtraBars.BarButtonItem btnReprint = new DevExpress.XtraBars.BarButtonItem();
                            btnReprint.Caption = "Reimpresion de Arqueo Nro. " + AI.Numero.ToString();
                            btnReprint.Glyph = Properties.Resources.printer;
                            btnReprint.Id = 2;
                            btnReprint.Name = "btnReprint";
                            btnReprint.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnReprint.Tag = RD;
                            btnReprint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReprint_ItemClick);
                            barItems.AddItem(btnReprint);
                            //--
                        }

                        if (btnCambiarLecturasArqueo)
                        {
                            //Boton Cambiar Lecturas en Arqueos
                            DevExpress.XtraBars.BarButtonItem btnCambiarLectura = new DevExpress.XtraBars.BarButtonItem();
                            btnCambiarLectura.Caption = "Cambiar Lecturas en Arqueo Nro. " + AI.Numero.ToString();
                            btnCambiarLectura.Glyph = Properties.Resources.ip;
                            btnCambiarLectura.Id = 2;
                            btnCambiarLectura.Name = "btnCambiarLectura";
                            btnCambiarLectura.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnCambiarLectura.Tag = RD;
                            btnCambiarLectura.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCambiarLectura_ItemClick);
                            barItems.AddItem(btnCambiarLectura);
                            //--
                        }

                        if (!T.Cerrada && btnsGeneralRD)
                        {
                            //Boton Abrir Arqueo de Isla
                            DevExpress.XtraBars.BarButtonItem btnOpenAI = new DevExpress.XtraBars.BarButtonItem();
                            btnOpenAI.Caption = "Abrir Arqueo Nro. " + AI.Numero.ToString();
                            btnOpenAI.Glyph = Properties.Resources.openid;
                            btnOpenAI.Id = 2;
                            btnOpenAI.Name = "btnOpenAI";
                            btnOpenAI.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                            btnOpenAI.Tag = RD;
                            btnOpenAI.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenAI_ItemClick);
                            barItems.AddItem(btnOpenAI);
                            //--
                        }
                    }

                    if (AI.Estado.Equals((int)Parametros.Estados.Modificado) && btnsGeneralRD)
                    {
                        //Boton Modificar Arqueo de Isla
                        DevExpress.XtraBars.BarButtonItem btnEdit = new DevExpress.XtraBars.BarButtonItem();
                        btnEdit.Caption = "Editar Arqueo Nro. " + AI.Numero.ToString();
                        btnEdit.Glyph = Properties.Resources.edit_diff;
                        btnEdit.Id = 2;
                        btnEdit.Name = "btnEdit";
                        btnEdit.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnEdit.Tag = RD;
                        btnEdit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEdit_ItemClick);
                        barItems.AddItem(btnEdit);
                        //--
                    }

                    if (AI.ArqueoEspecial && !AI.Oficial && btnsGeneralRD)
                    {
                        //Boton convertir Arqueo especial a oficial
                        DevExpress.XtraBars.BarButtonItem btnOficial = new DevExpress.XtraBars.BarButtonItem();
                        btnOficial.Caption = "Convertir Arqueo Oficial Nro. " + AI.Numero.ToString();
                        btnOficial.Glyph = Properties.Resources.edit_diff;
                        btnOficial.Id = 2;
                        btnOficial.Name = "btnOficial";
                        btnOficial.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnOficial.Tag = RD;
                        btnOficial.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOficial_ItemClick);
                        barItems.AddItem(btnOficial);
                        //--
                    }

                    if (RD.Aprobado && btnsGeneralRD)
                    {
                        //Boton Agregar Observacion
                        DevExpress.XtraBars.BarButtonItem btnObservacion = new DevExpress.XtraBars.BarButtonItem();
                        btnObservacion.Caption = "Agregar Observación al Arqueo Nro. " + AI.Numero.ToString();
                        btnObservacion.Glyph = Properties.Resources.counter;
                        btnObservacion.Id = 2;
                        btnObservacion.Name = "btnObservacion";
                        btnObservacion.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
                        btnObservacion.Tag = RD;
                        btnObservacion.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnObservacion_ItemClick);
                        barItems.AddItem(btnObservacion);
                        //--
                    }

                    TipoArqueo = Parametros.TiposArqueo.Isla;

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnpreview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                this.printControlAreaReport.Refresh();

                if (TipoArqueo.Equals(Parametros.TiposArqueo.ResumenDia))
                {
                    Reportes.Util.PrintArqueoFast(this, dbView, db, 0, 0, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.ResumenDia, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.Turno))
                {
                    Reportes.Util.PrintArqueoFast(this, dbView, db, 0, T.ID, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.Isla))
                {
                    Reportes.Util.PrintArqueoFast(this, dbView, db, AI.ID, 0, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.Isla, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnpreviewRE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                if (TipoArqueo.Equals(Parametros.TiposArqueo.Turno))
                    Reportes.Util.PrintArqueoEfectivo(this, dbView, db, AE, T, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.ResumenDia))
                    Reportes.Util.PrintArqueoEfectivo(this, dbView, db, null, null, RD, Parametros.TiposImpresion.Vista_Previa, false, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnprePrintAE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                if (TipoArqueo.Equals(Parametros.TiposArqueo.Turno))
                {
                    Entidad.ArqueoEfectivo NewAE = db.ArqueoEfectivos.Single(a => a.ID.Equals(AE.ID));
                    NewAE.ReImpresionCount = NewAE.ReImpresionCount + 1;
                    db.SubmitChanges();
                    Reportes.Util.PrintArqueoEfectivo(this, dbView, db, NewAE, T, RD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.ResumenDia))
                    Reportes.Util.PrintArqueoEfectivo(this, dbView, db, null, null, RD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.Properties.Resources.TXTVISTAPREVIA, this.printControlAreaReport);
              
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnReprint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                //rep = new Reportes.Arqueos.Hojas.RptArqueo();
                //this.printControlAreaReport.Refresh();

                if (TipoArqueo.Equals(Parametros.TiposArqueo.ResumenDia))
                {                    
                    Entidad.ResumenDia NewRD = db.ResumenDias.Single(a => a.ID.Equals(RD.ID));
                    NewRD.ReImpresionCount = NewRD.ReImpresionCount + 1;
                    db.SubmitChanges();
                    Reportes.Util.PrintArqueoFast(this, dbView, db, 0, 0, NewRD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.TiposArqueo.ResumenDia, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.Turno))
                {                    
                    Entidad.Turno NewT = db.Turnos.Single(a => a.ID.Equals(T.ID));
                    NewT.ReImpresionCount = NewT.ReImpresionCount + 1;
                    db.SubmitChanges();
                    Reportes.Util.PrintArqueoFast(this, dbView, db, 0, NewT.ID, RD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.Isla))
                {
                    Entidad.ArqueoIsla NewAI = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));
                    NewAI.ReImpresionCount = AI.ReImpresionCount + 1;
                    db.SubmitChanges();
                    Reportes.Util.PrintArqueoFast(this, dbView, db, NewAI.ID, 0, RD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.TiposArqueo.Isla, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //APROBACIÓN DE RESUMÉN DEL DÍA
        private void btnAprobar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (db.ResumenDias.Where(r => !r.Aprobado && r.EstacionServicioID.Equals(RD.EstacionServicioID) && r.SubEstacionID.Equals(RD.SubEstacionID)).OrderBy(o => o.ID).First().ID.Equals(RD.ID))
                {
                    if (db.Turnos.Count(t => t.ResumenDiaID.Equals(RD.ID) && !t.Cerrada) <= 0)
                    {
                        if (Parametros.General.DialogMsg("Esta seguro de Aprobar el Resumen del Día Nro. " + RD.Numero, Parametros.MsgType.question) == DialogResult.OK)
                        {
                            using (Arqueo.Forms.Dialogs.DialogInventarioResumen dg = new Arqueo.Forms.Dialogs.DialogInventarioResumen(RD.ID))
                            {
                                dg.Text = "Inventario Físico";

                                if (dg.ShowDialog().Equals(DialogResult.OK))
                                {

                                    //Entidad.InventarioResumenDia InventarioRD;
                                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                                    Entidad.ResumenDia NewRD = db.ResumenDias.Single(r => r.ID.Equals(RD.ID));
                                    NewRD.Aprobado = true;
                                    NewRD.FechaFinal = db.GetDateServer();
                                    NewRD.UsuarioCerradoID = Parametros.General.UserID;
                                    db.SubmitChanges();

                                    if (db.Turnos.Count(t => t.ResumenDia.Equals(NewRD)) > 1)
                                    {
                                        Reportes.Util.PrintArqueoFast(this, dbView, db, 0, 0, NewRD, (NewRD.Desaprobado ? Parametros.TiposImpresion.Modificado : Parametros.TiposImpresion.Original), true, Parametros.TiposArqueo.ResumenDia, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);
                                        Reportes.Util.PrintArqueoEfectivo(this, dbView, db, null, null, RD, Parametros.TiposImpresion.Original, true, "Cargando Datos Arqueo de Efectivo", this.printControlAreaReport);
                                    }
                                    else
                                        Parametros.General.DialogMsg("La estación cuenta con un solo Turno, el cual sirve como soporte para el Resumen del Día", Parametros.MsgType.message);

                                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                    "Se aprobó el Resumen de Día Nro. " + NewRD.Numero.ToString(), this.Name);
                                    Parametros.General.DialogMsg("Se aprobó el Resumen de Día Nro. " + NewRD.Numero.ToString(), Parametros.MsgType.message);
                                    FillControl(true);

                                }
                            }
                        }
                    }
                    else
                        Parametros.General.DialogMsg("No se puede aprobar el Resumen del Día Nro. " + RD.Numero.ToString() + Environment.NewLine
                            + "Debe cerrar los turnos de este Resumen del Día.", Parametros.MsgType.message);
                }
                else
                    Parametros.General.DialogMsg("No se puede aprobar el Resumen del Día Nro. " + RD.Numero.ToString() + Environment.NewLine
                        + "Existen Resumenes de Días anteriores sin ser aprobados.", Parametros.MsgType.warning);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnItemDesaprobar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("Esta seguro de Desaprobar el Resumen del Día Nro. " + RD.Numero, Parametros.MsgType.question) == DialogResult.OK)
                {
                    Entidad.ResumenDia NewRD = db.ResumenDias.Single(r => r.ID.Equals(RD.ID));
                    if (!NewRD.Contabilizado)
                    {
                        ////******POR EL MOMENTO NO SE VALIDARA LA DESAPROBACION******//////
                        //if (!RD.Desaprobado)
                        //{
                            NewRD.Aprobado = false;
                            NewRD.Desaprobado = true;
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                            "Se desaprobó el Resumen de Día Nro. " + NewRD.Numero.ToString(), this.Name);
                            Parametros.General.DialogMsg("Se desaprobó el Resumen de Día Nro. " + NewRD.Numero.ToString(), Parametros.MsgType.message);
                            FillControl(true);
                        //}
                        //else
                        //{
                        //    Parametros.General.DialogMsg("El Resumen del Día ya fue desaprobado anteriormente, no se puede volver a desaprobar.", Parametros.MsgType.warning);
                        //}
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El Resumen del Día ya esta contabilizado, no se puede desaprobar.", Parametros.MsgType.warning);
                    }
                }                  
             
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnOpenAI_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Entidad.ArqueoIsla NewAI = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));
                NewAI.Estado = (int)Parametros.Estados.Modificado;
                db.SubmitChanges();
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                "Se reaperturo el Arqueo de Isla Día Nro. " + NewAI.Numero.ToString(), this.Name);
                Parametros.General.DialogMsg("Se reaperturo el Arqueo de Isla Nro. " + NewAI.Numero.ToString(), Parametros.MsgType.message);
                FillControl(true);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnObservacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("Esta seguro de agregar observación al Arqueo Nro. " + AI.Numero.ToString(), Parametros.MsgType.question) == DialogResult.OK)
                {
                    using (Arqueo.Forms.Dialogs.DialogObservacion dg = new Arqueo.Forms.Dialogs.DialogObservacion())
                    {
                        dg.Text = "Agregar un comentario al Arqueo Nro. " + AI.Numero.ToString();
                        dg.Observacion = AI.Observacion;

                        if (dg.ShowDialog().Equals(DialogResult.OK))
                        {
                            Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                            Entidad.ArqueoIsla NewAI = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));
                            NewAI.Observacion = dg.Observacion;
                            NewAI.ReImpresionCount = AI.ReImpresionCount + 1;
                            db.SubmitChanges();

                            Entidad.ResumenDia NewRD = db.ResumenDias.Single(a => a.ID.Equals(RD.ID));
                            NewRD.ReImpresionCount = NewRD.ReImpresionCount + 1;
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                            "Se agrego una observación al Arqueo de Isla Día Nro. " + NewAI.Numero.ToString(), this.Name);

                            Reportes.Util.PrintArqueoFast(this, dbView, db, NewAI.ID, 0, NewRD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.TiposArqueo.Isla, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);
                            Reportes.Util.PrintArqueoFast(this, dbView, db, 0, 0, NewRD, Parametros.TiposImpresion.Re_Impresion, true, Parametros.TiposArqueo.ResumenDia, Parametros.Properties.Resources.TXTCARGANDO, this.printControlAreaReport);


                            FillControl(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        private void btnCrearAE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool abierto = false;
                foreach (System.Windows.Forms.Form form in Application.OpenForms)
                {
                    if (form.Name == "DialogArqueoEfectivo")
                    {
                        form.Activate();
                        abierto = true;
                    }
                }

                if (!abierto)
                {
                    Entidad.ArqueoEfectivo NewAE = db.ArqueoEfectivos.SingleOrDefault(ae => ae.TurnoID.Equals(T.ID));

                    if (NewAE != null)
                    {
                        if (!NewAE.Cerrado)
                        {
                            Arqueo.Forms.Dialogs.DialogArqueoEfectivo nfAE = new Arqueo.Forms.Dialogs.DialogArqueoEfectivo();
                            nfAE.Text = "Arqueo de Efectivo";
                            nfAE.EsEspecial = true;
                            nfAE.IDturno = T.ID;
                            nfAE.Owner = this;
                            nfAE.Show();
                        }
                    }
                    else
                    {
                        if (T.Especial)
                        {
                            if (Parametros.General.DialogMsg("No existe un Arqueo de Efectivo Abierto para este Turno, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                Entidad.ArqueoEfectivo AEfectivo = new Entidad.ArqueoEfectivo();

                                AEfectivo.TurnoID = T.ID;
                                AEfectivo.UsuarioCreado = T.UsuarioAbiertoID;
                                AEfectivo.FechaCreado = T.FechaInicial;
                                AEfectivo.Estado = 0;
                                db.ArqueoEfectivos.InsertOnSubmit(AEfectivo);
                                db.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (TipoArqueo.Equals(Parametros.TiposArqueo.Isla))
                {
                    if (nf == null)
                    {
                        if (AI != null)
                        {
                            Entidad.ArqueoIsla NewAI = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));
                            if (AI.Estado == (int)Parametros.Estados.Modificado)
                            {
                                nf = new Forms.Dialogs.DialogArqueoIsla(Usuario);
                                nf.Text = "Editar Arqueo de Isla";
                                nf.EntidadAnterior = NewAI;
                                nf.Owner = this;
                                nf.Editable = true;
                                if (AI.Estado.Equals(Convert.ToInt32(Parametros.Estados.Modificado)))
                                    nf.Modificando = true;
                                nf.RDI = this;
                                nf.Show();
                            }
                            else
                                Parametros.General.DialogMsg("El Arqueo " + AI.Numero.ToString() + " ya esta cerrado, no se puede editar.", Parametros.MsgType.warning);
                        }
                    }
                }
                else if (TipoArqueo.Equals(Parametros.TiposArqueo.Turno))
                {
                    bool abierto = false;
                    foreach (System.Windows.Forms.Form form in Application.OpenForms)
                    {
                        if (form.Name == "DialogArqueoEfectivo")
                        {
                            form.Activate();
                            abierto = true;
                        }
                    }

                    if (!abierto)
                    {
                        Entidad.ArqueoEfectivo NewAE = db.ArqueoEfectivos.Where(ae => ae.TurnoID.Equals(T.ID)).First();
                        if (!NewAE.Cerrado)
                        {
                            Arqueo.Forms.Dialogs.DialogArqueoEfectivo nfAE = new Arqueo.Forms.Dialogs.DialogArqueoEfectivo();
                            nfAE.Text = "Arqueo de Efectivo";
                            nfAE.EntidadAnterior = NewAE;
                            nfAE.Owner = this;
                            nfAE.Show();
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnCambiarLectura_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (TipoArqueo.Equals(Parametros.TiposArqueo.Isla))
                {
                    if (ncl == null)
                    {
                        if (AI != null)
                        {
                            Entidad.ArqueoIsla NewAI = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));

                            ncl = new Forms.Dialogs.DialogCambioLecturas(Parametros.General.UserID, AI.Turno.ResumenDia, AI.Turno, AI);
                            ncl.Text = "Cambiar Lecturas en Arqueo de Isla";
                            ncl.Owner = this;
                            ncl.RDI = this;
                            ncl.Show();
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnOpenT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("Esta seguro de reaperturar el Turno " + T.Numero, Parametros.MsgType.question) == DialogResult.OK)
                {
                    Entidad.Turno NewT = db.Turnos.Single(a => a.ID.Equals(T.ID));
                    NewT.Cerrada = false;
                    NewT.Estado = 1;
                    db.SubmitChanges();
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se reaperturo el Turno Día Nro. " + NewT.Numero.ToString(), this.Name);
                    Parametros.General.DialogMsg("Se reaperturo el Turno Nro. " + NewT.Numero.ToString(), Parametros.MsgType.message);
                    FillControl(true);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnCloseT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("¿Esta seguro de cerrar el Turno Nro. " + T.Numero.ToString() + " ?", Parametros.MsgType.question) == DialogResult.OK)
                {
                    if (db.GetArqueosIslasPendientes(IDES, T.ID, RD.SubEstacionID) <= 0 || T.Especial)
                    {
                        if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(T.ID) && ae.Cerrado) > 0 || T.Especial)
                        {
                            if (!T.Especial)
                            {
                                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                                Reportes.Util.PrintArqueoFast(this, dbView, db, 0, T.ID, RD, (((Parametros.Estados)(T.Estado)).Equals(Parametros.Estados.Abierto) ? Parametros.TiposImpresion.Original : Parametros.TiposImpresion.Modificado), true, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTCIERREREGISTRO, this.printControlAreaReport);

                                Entidad.Turno Tur = db.Turnos.Single(t => t.ID.Equals(T.ID));
                                Tur.Cerrada = true;
                                Tur.Estado = 2;
                                Tur.FechaFinal = db.GetDateServer();
                                Tur.UsuarioCerradoID = Parametros.General.UserID;
                                db.SubmitChanges();

                                if (!Tur.Especial)
                                {
                                    Entidad.ResumenDia EtResumen = db.ResumenDias.SingleOrDefault(s => s.ID.Equals(Tur.ResumenDiaID));

                                    if (EtResumen != null)
                                    {
                                        if (EtResumen.SubEstacionID > 0)
                                        {
                                            if (Tur.Numero >= Convert.ToInt32(db.SubEstacions.Single(es => es.ID.Equals(EtResumen.SubEstacionID)).NumeroTurnos))
                                            {
                                                db.ResumenDias.Single(r => r.ID.Equals(EtResumen.ID)).Cerrado = true;
                                                db.SubmitChanges();
                                            }
                                        }
                                        else
                                        {
                                            if (Tur.Numero >= Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(EtResumen.EstacionServicioID)).NumeroTurnos))
                                            {
                                                db.ResumenDias.Single(r => r.ID.Equals(EtResumen.ID)).Cerrado = true;
                                                db.SubmitChanges();
                                            }
                                        }
                                    }

                                    //if (Tur.Numero.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(IDES)).NumeroTurnos)))
                                    //{
                                    //    Entidad.ResumenDia Res = db.ResumenDias.Single(r => r.ID.Equals(RD.ID));
                                    //    Res.Cerrado = true;
                                    //    db.SubmitChanges();
                                    //}
                                }



                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Finalizó el Turno: " + Tur.Numero.ToString(), this.Name);
                                FillControl(true);
                            }
                            else
                            {
                                if (db.ArqueoIslas.Count(ai => ai.Turno.Equals(T) && (!ai.Cerrado || ai.Estado.Equals(Parametros.Estados.Modificado))) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGARQUEOSISLASABIERTOS, Parametros.MsgType.warning);
                                    return;
                                }
                                else
                                {
                                    if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(T.ID)) > 0)
                                    {
                                        if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(T.ID) && ae.Cerrado) > 0)
                                        {
                                            Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                                            Reportes.Util.PrintArqueoFast(this, dbView, db, 0, T.ID, RD, Parametros.TiposImpresion.Original, true, Parametros.TiposArqueo.Turno, Parametros.Properties.Resources.TXTCIERREREGISTRO, this.printControlAreaReport);

                                            Entidad.Turno Tur = db.Turnos.Single(t => t.ID.Equals(T.ID));
                                            Tur.Cerrada = true;
                                            Tur.FechaFinal = db.GetDateServer();
                                            Tur.UsuarioCerradoID = Parametros.General.UserID;
                                            db.SubmitChanges();
                                            
                                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Finalizó el Turno: " + Tur.Numero.ToString(), this.Name);
                                            FillControl(true);
                                        }
                                        else
                                        {
                                            Parametros.General.DialogMsg("Debe de cerrar el arqueo de efectivo de este turno!", Parametros.MsgType.warning);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Parametros.General.DialogMsg("No existe un Arqueo de Efectivo asignado a este turno!", Parametros.MsgType.warning);
                                        return;
                                    }

                                }
                            }
                        }
                        else
                        {
                            Parametros.General.DialogMsg("Debe de cerrar el arqueo de efectivo de este turno!", Parametros.MsgType.warning);
                            return;
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGARQUEOSISLASABIERTOS, Parametros.MsgType.warning);
                        return;
                    } 
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void btnOficial_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (AI.Cerrado && AI.Estado.Equals(2))
                {
                    if (Parametros.General.DialogMsg("Esta seguro de convertir el Arqueo Especial Nro. " + AI.Numero.ToString() + " a Arqueo Oficial ", Parametros.MsgType.question) == DialogResult.OK)
                    {
                        if (db.Turnos.Count(t => !t.Cerrada && t.ResumenDiaID.Equals(RD.ID) && t.Especial) > 0)
                        {
                            Entidad.Turno Tur = db.Turnos.Where(t => !t.Cerrada && t.ResumenDiaID.Equals(RD.ID) && t.Especial).OrderByDescending(o => o.ID).First();

                            Entidad.ArqueoIsla Arqueo = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));

                            Arqueo.Oficial = true;
                            Arqueo.Turno = Tur;
                            Arqueo.FechaCerrado = Convert.ToDateTime(db.GetDateServer());
                            Arqueo.UsuarioCerrado = Parametros.General.UserID;
                            db.SubmitChanges();

                            foreach (var AM in db.ArqueoMangueras.Where(am => am.ArqueoProducto.ArqueoIslaID.Equals(AI.ID)))
                            {
                                Entidad.Manguera EtManguera = db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID));

                                EtManguera.LecturaMecanica = AM.LecturaMecanicaFinal;
                                EtManguera.LecturaEfectivo = AM.LecturaEfectivoFinal;
                                EtManguera.LecturaElectronica = AM.LecturaElectronicaFinal;
                                db.SubmitChanges();
                            }
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Convirtió el Arqueo de Isla Nro.: " + Arqueo.Numero.ToString() + " a un Arqueo Oficial.", this.Name);
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            FillControl(true);
                        }
                        else
                        {
                            if (Parametros.General.DialogMsg("No existe un Turno Especial Abierto, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                Entidad.Turno Tur = new Entidad.Turno();

                                Tur.FechaInicial = RD.FechaInicial;
                                Tur.UsuarioAbiertoID = Parametros.General.UserID;
                                Tur.ResumenDiaID = RD.ID;
                                Tur.Especial = true;
                                Tur.Numero = Convert.ToInt32(db.Turnos.Count(r => r.Cerrada && r.ResumenDiaID.Equals(RD.ID) && r.Especial)) > 0 ? Convert.ToInt32(db.Turnos.Where(r => r.Cerrada && r.ResumenDiaID.Equals(RD.ID) && r.Especial).OrderByDescending(o => o.ID).First().Numero) + 1 : 1;

                                db.Turnos.InsertOnSubmit(Tur);
                                db.SubmitChanges();

                                Entidad.ArqueoEfectivo AEfectivo = new Entidad.ArqueoEfectivo();

                                AEfectivo.TurnoID = Tur.ID;
                                AEfectivo.UsuarioCreado = Tur.UsuarioAbiertoID;
                                AEfectivo.FechaCreado = Tur.FechaInicial;
                                AEfectivo.Estado = 0;
                                db.ArqueoEfectivos.InsertOnSubmit(AEfectivo);
                                db.SubmitChanges();

                                Entidad.ArqueoIsla Arqueo = db.ArqueoIslas.Single(a => a.ID.Equals(AI.ID));

                                Arqueo.Oficial = true;
                                Arqueo.Turno = Tur;
                                Arqueo.FechaCerrado = Convert.ToDateTime(db.GetDateServer());
                                Arqueo.UsuarioCerrado = Parametros.General.UserID;
                                db.SubmitChanges();

                                foreach (var AM in db.ArqueoMangueras.Where(am => am.ArqueoProducto.ArqueoIslaID.Equals(AI.ID)))
                                {
                                    Entidad.Manguera EtManguera = db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID));

                                    EtManguera.LecturaMecanica = AM.LecturaMecanicaFinal;
                                    EtManguera.LecturaEfectivo = AM.LecturaEfectivoFinal;
                                    EtManguera.LecturaElectronica = AM.LecturaElectronicaFinal;
                                    db.SubmitChanges();
                                }
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Convirtió el Arqueo de Isla Nro.: " + Arqueo.Numero.ToString() + " a un Arqueo Oficial.", this.Name);
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                FillControl(true);
                            }

                        }
                    }
                }
                else
                {
                    Parametros.General.DialogMsg("El arqueo de isla no esta cerrado, debe proceder a cerrarlo para optar a convertirlo a Oficial", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private void btnOpenAE_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("Esta seguro de reaperturar el Arqueo de Efectivo", Parametros.MsgType.question) == DialogResult.OK)
                {
                    Entidad.ArqueoEfectivo NewAE = db.ArqueoEfectivos.Where(ae => ae.TurnoID.Equals(T.ID)).First();
                    NewAE.Cerrado = false;
                    db.SubmitChanges();
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se reaperturo el Arqueo de Efectivo del Turno " + T.Numero.ToString(), this.Name);
                    Parametros.General.DialogMsg("Se reaperturo el Arqueo de Efectivo del Turno " + T.Numero.ToString(), Parametros.MsgType.message);
                    FillControl(true);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //Cambio de litros en Inventario Resumen
        private void gvDataIR_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "Litros")
                {
                    DevExpress.XtraGrid.Views.Grid.GridView gvLoad = (DevExpress.XtraGrid.Views.Grid.GridView)(sender);
                    decimal vLitros = 0;
                    if (gvLoad.GetRowCellValue(e.RowHandle, "ID") != null)
                    {

                        vLitros = Decimal.Round(Convert.ToDecimal(gvLoad.GetFocusedRowCellValue("Litros")), 3, MidpointRounding.AwayFromZero);

                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            db.InventarioResumenDia.Single(s => s.ID.Equals(Convert.ToInt32(gvLoad.GetRowCellValue(e.RowHandle, colIDIR)))).Litros = vLitros;
                                db.SubmitChanges();
                        
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se modificó el litro del producto : " + gvLoad.GetRowCellValue(e.RowHandle, colProductoIR).ToString() + ", en tanque : " + gvLoad.GetRowCellValue(e.RowHandle, colTanqueIR).ToString() + ", del resumen del día ID : " + gvLoad.GetRowCellValue(e.RowHandle, colIDIR).ToString(), this.Name);
                
                    }
                    else
                        Parametros.General.DialogMsg("Los litros del resumen no fueron cargados correctamente.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

    }
}
