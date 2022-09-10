using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Linq;

namespace SAGAS.Arqueo.Forms
{                                
    public partial class FormPrecioCombustible : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogPrecioCombustible nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog(); 
        private DataTable dtPrecioCombustible;
        //private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecio;
        //private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecioDif;

        #endregion

        #region <<< INICIO >>>

        public FormPrecioCombustible()
        {
            InitializeComponent();
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            try
            {
                this.FillControl();
                this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
                this.btnCambiarRangoPrecio.Enabled = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "btnCambiarRangoPrecio");
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                bdsManejadorDatos.DataSource = from pc in db.PrecioCombustibles
                                               select pc;

                this.grid.DataSource = bdsManejadorDatos;

                gvData_FocusedRowChanged(null, null);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogPrecioCombustible();
                    nf.Text = "Crear Lista de Precio de Combustible";
                    nf.Owner = this;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        nf = new Forms.Dialogs.DialogPrecioCombustible();
                        nf.Text = "Editar Lista de Precio de Combustible";
                        nf.EntidadAnterior = db.PrecioCombustibles.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        nf.Owner = this;
                        nf.Editable = true;
                        nf.MDI = this;
                        nf.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
              
        #endregion

        #region <<< EVENTOS >>>

        
        private void pGridPCD_CustomDrawFieldValue(object sender, DevExpress.XtraPivotGrid.PivotCustomDrawFieldValueEventArgs e)
        {
            try
            {
                if (e.Area == DevExpress.XtraPivotGrid.PivotArea.ColumnArea)
                {
                    if (e.Field.FieldName.Equals("Nombre"))
                    {
                        e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Parametros.General.ListColTanques.Find(f => f.Nombre.Equals(e.Value.ToString())).Color));
                        e.Appearance.ForeColor = Color.White;
                        
                    }
                }
            }
            catch { e.Appearance.BackColor = Color.White; }
        }

        
        private void pGridPCD_FieldValueDisplayText(object sender, DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventArgs e)
        {
            try
            {
                if (e.Field != null)
                {
                    if (e.Field.FieldName.Equals("PrecioDiferenciado"))
                    {
                        e.Field.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        if (e.Value.Equals(true))
                            e.DisplayText = "PRECIO DIFERENCIADO";
                        else
                            e.DisplayText = "PRECIO FULL";
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void pGridPCD_CustomDrawFieldHeader(object sender, DevExpress.XtraPivotGrid.PivotCustomDrawFieldHeaderEventArgs e)
        {
//if (e.IsColumn)
            //if (e.Field.FieldName.Equals("PrecioDiferenciado"))
            //    if (e. Value.Equals(true))
            //        e.DisplayText = "Si";
            //    else
            //        e.DisplayText = "No";
        }

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    //int IDPC = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                    var obj = db.spListaPrecioCombustible(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        
                        //(from es in db.EstacionServicios
                        //       join z in db.Zonas on es.ZonaID equals z.ID
                        //       join ses in db.SubEstacions on es.ID equals ses.EstacionServicioID into sus
                        //       from su in sus.DefaultIfEmpty()
                        //       join pcd in db.PrecioCombustibleDetalles.GroupBy(g => new { g.PrecioCombustibleID, g.PrecioDiferenciado, g.ProductoID, g.Precio, g.EstacionServicioID, g.SubEstacionID }) on es.ID equals pcd.Key.EstacionServicioID
                        //       join p in db.Productos on pcd.Key.ProductoID equals p.ID
                        //       where es.Activo && pcd.Key.PrecioCombustibleID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))
                        //       select new
                        //       {
                        //           pcd.Key.PrecioDiferenciado,
                        //           Producto = p.Nombre,
                        //           p.Codigo,
                        //           pcd.Key.Precio,
                        //           //IDEstacion = es.ID,
                        //           //CodigoEstacion = es.Codigo,
                        //           //IDSubEstacion = su.ID == null ? 0 : su.ID,
                        //           //CodigoSubEstacion = su.Codigo,
                        //           Nombre = (su.ID == null ? es.Nombre : su.Nombre).Distinct(),
                        //           es.ZonaID,
                        //           ZonaNombre = z.Nombre
                        //       }).OrderBy(z => z.ZonaID).ThenBy(n => n.Nombre);

                    this.pGridPCD.DataSource = obj;

                    this.pGridPCD.RefreshData(); 


                    //pGridPCD.DataSource = obj;
                    //pGridPCD.RefreshData();

                    /*
                    grid.Refresh();
                    bgvEs.RefreshData();
                    bgvEs.BeginUpdate();

                    int bandCount = bgvEs.Bands.Count;

                    for (int i = 1; i < bandCount; i++)
                    {
                        bgvEs.Bands.RemoveAt(1);

                    }

                    int IDPC = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                    dtPrecioCombustible = new DataTable();
                    dtPrecioCombustible.Columns.Clear();

                    dtPrecioCombustible.Columns.Add("IDP", typeof(Int32));
                    dtPrecioCombustible.Columns.Add("IDSUS", typeof(Int32));
                    dtPrecioCombustible.Columns.Add("ES", typeof(String));

                    var bandPrecio = new Parametros.MyBand("bandPrecio", "Precio Full", 300, bgvEs.Bands.Count + 1);                    
                    this.bgvEs.Bands.Add(bandPrecio);

                       var bandPrecioDif = new Parametros.MyBand("bandPrecioDif", "Precio Diferenciado", 300, bgvEs.Bands.Count + 1);
                        this.bgvEs.Bands.Add(bandPrecioDif);               

                    int cont = 3;

                    foreach (var item in db.Productos.Where(p => p.Activo && p.ProductoClaseID == Parametros.Config.ProductoClaseCombustible()))
                    {
                        dtPrecioCombustible.Columns.Add(item.ID.ToString(), typeof(Decimal));
                        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProd = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                        //**Columna
                        colProd.Caption = item.Nombre;
                        colProd.FieldName = item.ID.ToString();
                        colProd.Name = "col" + item.ID.ToString();
                        colProd.Visible = true;
                        colProd.Width = 100;
                        colProd.AppearanceHeader.Options.UseForeColor = true;
                        colProd.AppearanceHeader.Options.UseBackColor = true;
                        colProd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                        colProd.AppearanceHeader.ForeColor = Color.White;
                        colProd.DisplayFormat.FormatString = "N2";
                        colProd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                        if (db.Tanques.Where(t => t.ProductoID == item.ID).Count() > 0)
                        {
                            colProd.AppearanceHeader.BackColor = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == item.ID).First().Color));
                            colProd.AppearanceHeader.BackColor2 = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == item.ID).First().Color));

                        }

                        //**Repositorio
                        DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpPrecio = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                        rpPrecio.AutoHeight = false;
                        //    rpPrecio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                        //new DevExpress.XtraEditors.Controls.EditorButton()});
                        rpPrecio.MaxValue = new decimal(new int[] {
                        10000000,
                        0,
                        0,
                        0});
                        rpPrecio.Name = "rpItem" + item.ID.ToString();

                        //rpPrecio.EditValueChanged += new System.EventHandler(spinEdits_EditValueChanged);

                                    this.bgvEs.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
                        colProd});

                                    this.gridPCD.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
                        rpPrecio});

                        bandPrecio.Columns.Add(colProd);
                        colProd.ColumnEdit = rpPrecio;
                        cont++;

                        //***********************/
                        ////PRECIO DIFERENCIAL////
                        //***********************//
                        /*
                        #region <<< PRECIODIFERENCIAL >>>


                        dtPrecioCombustible.Columns.Add(item.ID.ToString() + "Dif", typeof(Decimal));
                        DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProdDIF = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                        //**Columna
                        colProdDIF.Caption = item.Nombre;
                        colProdDIF.FieldName = item.ID.ToString() + "Dif";
                        colProdDIF.Name = "col" + item.ID.ToString() + "Dif";
                        colProdDIF.Visible = true;
                        colProdDIF.Width = 100;
                        colProdDIF.AppearanceHeader.Options.UseForeColor = true;
                        colProdDIF.AppearanceHeader.Options.UseBackColor = true;
                        colProdDIF.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                        colProdDIF.AppearanceHeader.ForeColor = Color.White;
                        colProdDIF.DisplayFormat.FormatString = "N2";
                        colProdDIF.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                        if (db.Tanques.Where(t => t.ProductoID == item.ID).Count() > 0)
                        {
                            colProdDIF.AppearanceHeader.BackColor = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == item.ID).First().Color));
                            colProdDIF.AppearanceHeader.BackColor2 = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == item.ID).First().Color));

                        }

                        //**Repositorio
                        DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpPrecioDif = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                        rpPrecioDif.AutoHeight = false;
                        //    rpPrecio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                        //new DevExpress.XtraEditors.Controls.EditorButton()});
                        rpPrecioDif.MaxValue = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
                        rpPrecioDif.Name = "rpItem" + item.ID.ToString() + "Dif";



                        this.bgvEs.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            colProdDIF});

                        this.gridPCD.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            rpPrecioDif});

                        bandPrecioDif.Columns.Add(colProdDIF);
                        colProdDIF.ColumnEdit = rpPrecioDif;
                        cont++;
                        ///***********************************///////////
                        ///
                    /**
                        #endregion
                    

                    }


                    bgvEs.EndUpdate();

                    var obj = (from es in db.EstacionServicios
                               join ses in db.SubEstacions on es.ID equals ses.EstacionServicioID into sus
                               from su in sus.DefaultIfEmpty()
                               join pcd in db.PrecioCombustibleDetalles on es.ID equals pcd.EstacionServicioID
                               where es.Activo && pcd.PrecioCombustibleID.Equals(IDPC)
                               select new
                               {
                                   IDEstacion = es.ID,
                                   CodigoEstacion = es.Codigo,
                                   IDSubEstacion = su.ID == null ? 0 : su.ID,
                                   CodigoSubEstacion = su.Codigo,
                                   Nombre = su.ID == null ? es.Nombre : su.Nombre,
                                   es.ZonaID
                               }).Distinct().OrderBy(z => z.ZonaID).ThenBy(n => n.Nombre);

                      var ListValor = db.PrecioCombustibleDetalles.Where(pcd => pcd.PrecioCombustibleID.Equals(IDPC));
                    
                      obj.ToList().ForEach(l =>
                    {

                        DataRow dr = dtPrecioCombustible.NewRow();
                        dr["IDP"] = l.IDEstacion;
                        dr["IDSUS"] = l.IDSubEstacion;
                        dr["ES"] = l.Nombre;
                            for (int i = 0; i < bandPrecio.Columns.Count; i++)
                            {

                                int x = 3 + (bandPrecio.Columns[i].ColIndex * 2);
                                int IDProd = Convert.ToInt32(bandPrecio.Columns[i].FieldName);

                                dr[x] = ListValor.Count(pcd => pcd.EstacionServicioID.Equals(l.IDEstacion) && pcd.SubEstacionID.Equals(l.IDSubEstacion) && pcd.ProductoID.Equals(IDProd) && !pcd.PrecioDiferenciado) > 0 ?
                                    ListValor.First(pcd => pcd.EstacionServicioID.Equals(l.IDEstacion) && pcd.SubEstacionID.Equals(l.IDSubEstacion) && pcd.ProductoID.Equals(IDProd) && !pcd.PrecioDiferenciado).Precio : 0;
                         
                                x++;

                                dr[x] = ListValor.Count(pcd => pcd.EstacionServicioID.Equals(l.IDEstacion) && pcd.SubEstacionID.Equals(l.IDSubEstacion) && pcd.ProductoID.Equals(IDProd) && pcd.PrecioDiferenciado) > 0 ?
                                     ListValor.First(pcd => pcd.EstacionServicioID.Equals(l.IDEstacion) && pcd.SubEstacionID.Equals(l.IDSubEstacion) && pcd.ProductoID.Equals(IDProd) && pcd.PrecioDiferenciado).Precio : 0;

                            }        
                    
                        dtPrecioCombustible.Rows.Add(dr);

                    });


                    this.gridPCD.DataSource = dtPrecioCombustible;

                    
                **/
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            }
        }

        private void btnCambiarRangoPrecio_Click(object sender, EventArgs e)
        {
            try
            {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        Forms.Dialogs.DialogNuevoRangoPrecioCombustible dg = new Forms.Dialogs.DialogNuevoRangoPrecioCombustible();
                        dg.Text = "Editar Nuevo Rango para la Lista de Precio de Combustible";
                        dg.EntidadAnterior = db.PrecioCombustibles.Single(pc => pc.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        dg.Owner = this;
                        dg.ShowDialog();
                        FillControl();
                    }
               
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void btnPrecioPorTurno_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    Forms.Dialogs.DialogPrecioPorTurno dg = new Forms.Dialogs.DialogPrecioPorTurno();
                    dg.Text = "Crear Precio para un Turno específico";
                    dg.Owner = this;
                    dg.ShowDialog();
                    FillControl();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

    }
}
