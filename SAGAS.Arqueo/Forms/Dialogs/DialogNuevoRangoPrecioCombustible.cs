using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;
using SAGAS.Arqueo.Forms; 
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogNuevoRangoPrecioCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Entidad.PrecioCombustible EntidadAnterior;
        private bool ShowMsg = false;
        private bool Editable;
        private bool Begin = false;
        private DataTable dtPrecioCombustible;
        private DataTable dtEstacionesServicios;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecio;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecioDif;
        private decimal MargenVariacion = Parametros.Config.VariacionPorcentualPrecio();


        private DateTime FechaInicio
        {
            get { return Convert.ToDateTime(dateInicio.EditValue); }
            set { dateInicio.EditValue = value; }
        }

        private DateTime FechaFinal
        {
            get { return Convert.ToDateTime(dateFinal.EditValue); }
            set { dateFinal.EditValue = value; }
        }

        public DataTable PrecioCombustibleDT
        {
            get
            {
                return dtPrecioCombustible;
            }
            set
            {
                dtPrecioCombustible = value;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogNuevoRangoPrecioCombustible()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());


                dateInicio.EditValue = db.GetDateServer();
                //dateFinal.EditValue = Convert.ToDateTime(dateInicio.EditValue).Date.AddDays(Parametros.Config.RangoDiasPrecioCombustible());
                dateFinal.EditValue = EntidadAnterior.FechaFinal;

                var listSucursales = (from ES in db.EstacionServicios
                                      join ses in db.SubEstacions on ES.ID equals ses.EstacionServicioID into sus
                                      from su in sus.DefaultIfEmpty()
                                      join pcd in db.PrecioCombustibleDetalles on ES.ID equals pcd.EstacionServicioID
                                      where ES.Activo
                                      select new
                                       { 
                                           IDES = ES.ID,
                                           //CodigoEstacion = ES.Codigo,
                                           IDSubEstacion = su.ID == null ? 0 : su.ID,
                                           //CodigoSubEstacion = String.IsNullOrEmpty(su.Codigo) ? "0" : su.Codigo,
                                           NombreES = su.ID == null ? ES.Nombre : su.Nombre,
                                           SelectedES = ES.Activo,
                                           ES.ZonaID
                                       }).Distinct().OrderBy(z => z.ZonaID);//.ThenBy(n => n.Nombre);


                this.dtEstacionesServicios = Parametros.General.LINQToDataTable(listSucursales);

                this.gridES.DataSource = dtEstacionesServicios;

                for (int i = 0; i < gvES.RowCount; i++)
                {
                    this.gvES.SetRowCellValue(i, "SelectedES", false);
                }

                FillBands();
                Begin = true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void FillBands()
        {
            try
            {
                dtPrecioCombustible = new DataTable();
                bandPrecio = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
                this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] { this.bandPrecio });

                dtPrecioCombustible.Columns.Add("IDP", typeof(Int32));
                dtPrecioCombustible.Columns.Add("IDSUS", typeof(Int32));
                dtPrecioCombustible.Columns.Add("ES", typeof(String));

                this.bandPrecio.Caption = "PRECIO FULL";
                this.bandPrecio.MinWidth = 20;
                this.bandPrecio.OptionsBand.AllowMove = false;
                this.bandPrecio.OptionsBand.AllowPress = false;
                this.bandPrecio.OptionsBand.ShowCaption = true;
                this.bandPrecio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.bandPrecio.Width = 300;

                bandPrecioDif = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();

                this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandPrecioDif});

                this.bandPrecioDif.Caption = "PRECIO DIFERENCIADO";
                this.bandPrecioDif.MinWidth = 20;
                this.bandPrecioDif.OptionsBand.AllowMove = false;
                this.bandPrecioDif.OptionsBand.AllowPress = false;
                this.bandPrecioDif.OptionsBand.ShowCaption = true;
                this.bandPrecioDif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.bandPrecioDif.Width = 300;

                int cont = 3;

                foreach (var item in db.Productos.Where(p => p.Activo && p.ProductoClaseID == Parametros.Config.ProductoClaseCombustible()))
                {
                    dtPrecioCombustible.Columns.Add(item.ID.ToString(), typeof(Decimal));
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProd = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                    //**Columna
                    colProd.Caption = item.Nombre;
                    colProd.Visible = true;
                    colProd.Width = 100;
                    colProd.FieldName = item.ID.ToString();
                    colProd.Name = "col" + item.ID.ToString();
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

                    this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpPrecio });

                    bandPrecio.Columns.Add(colProd);
                    colProd.ColumnEdit = rpPrecio;
                    cont++;

                    //***********************//
                    ////PRECIO DIFERENCIAL////
                    //***********************//

                    #region <<< PRECIODIFERENCIAL >>>

                    dtPrecioCombustible.Columns.Add(item.ID.ToString() + "Dif", typeof(Decimal));
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProdDIF = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                    //**Columna
                    colProdDIF.Caption = item.Nombre;
                    colProdDIF.Visible = true;
                    colProdDIF.Width = 100;
                    colProdDIF.FieldName = item.ID.ToString() + "Dif";
                    colProdDIF.Name = "col" + item.ID.ToString() + "Dif";

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

                    this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpPrecioDif });

                    this.bandPrecioDif.Columns.Add(colProdDIF);
                    colProdDIF.ColumnEdit = rpPrecioDif;
                    cont++;
                    ///***********************************///////////
                    ///
                    #endregion

                }

                this.grid.DataSource = dtPrecioCombustible;
                this.gvData.RefreshData();

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
       
        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(dateInicio, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(dateFinal, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (dateInicio.EditValue == null || dateFinal.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if ((Convert.ToDateTime(dateInicio.EditValue).Date == Convert.ToDateTime(dateFinal.EditValue).Date) || (Convert.ToDateTime(dateInicio.EditValue).Date > Convert.ToDateTime(dateFinal.EditValue).Date))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGVALIDARFECHA + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.PrecioCombustible PCAnterior;
                    Entidad.PrecioCombustible PCPosterior;


                    PCAnterior = new Entidad.PrecioCombustible();
                    PCPosterior = new Entidad.PrecioCombustible();

                    PCAnterior.UsuarioID = Parametros.General.UserID;
                    PCPosterior.UsuarioID = Parametros.General.UserID;

                    PCAnterior.FechaInicial = EntidadAnterior.FechaInicial;
                    PCAnterior.FechaFinal = FechaInicio.AddDays(-1);

                    PCPosterior.FechaInicial = FechaInicio;
                    PCPosterior.FechaFinal = FechaFinal;


                    db.PrecioCombustibles.InsertOnSubmit(PCAnterior);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se creó Lista de Precio para la semana: " + PCAnterior.FechaInicial, this.Name);

                    db.PrecioCombustibles.InsertOnSubmit(PCPosterior);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se creó Lista de Precio para la semana: " + PCPosterior.FechaInicial, this.Name);
                                  
                    db.SubmitChanges();

                    foreach (DataRow row in dtPrecioCombustible.Rows)
                    {
                        //var pcdet =
                        db.PrecioCombustibleDetalles.Where(p => p.PrecioCombustibleID == EntidadAnterior.ID && p.EstacionServicioID == Convert.ToInt32(row["IDP"]) && p.SubEstacionID == Convert.ToInt32(row["IDSUS"])).ToList().ForEach(det =>
                            {
                                Entidad.PrecioCombustibleDetalle NewPCD = db.PrecioCombustibleDetalles.Single(p => p.ID.Equals(det.ID));

                                NewPCD.PrecioCombustible = PCAnterior;
                                db.SubmitChanges();
                            });
                    }

                    int val = 0;
                    foreach (DataRow det in PrecioCombustibleDT.Rows)
                    {
                        
                        for (int i = 0; i < bandPrecio.Columns.Count; i++)
                        {
                            if (Convert.ToInt32(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i])) > 0)
                            {

                                Entidad.PrecioCombustibleDetalle PCD = new Entidad.PrecioCombustibleDetalle();

                                PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i]));
                                PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                PCD.PrecioDiferenciado = false;
                                PCD.PrecioCombustibleID = PCPosterior.ID;

                                db.PrecioCombustibleDetalles.InsertOnSubmit(PCD);
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se creó Precio Full para la semana: " + PCPosterior.FechaInicial, this.Name);

                                db.SubmitChanges();
                            }
                        }

                        for (int i = 0; i < bandPrecioDif.Columns.Count; i++)
                        {
                            if (Convert.ToInt32(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i])) > 0)
                            {
                                Entidad.PrecioCombustibleDetalle PCD = new Entidad.PrecioCombustibleDetalle();

                                PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i]));
                                PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                PCD.PrecioDiferenciado = true;
                                PCD.PrecioCombustibleID = PCPosterior.ID;

                                db.PrecioCombustibleDetalles.InsertOnSubmit(PCD);
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se creó Precio Diferencial para la semana: " + PCPosterior.FechaInicial, this.Name);

                                db.SubmitChanges();
                            }

                        }


                        val++;

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {

            if (!Guardar()) return;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dateInicio_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void dateInicio_EditValueChanged(object sender, EventArgs e)
        {
            if (dateInicio.EditValue != null || dateFinal.EditValue != null)
            {
                if ((Convert.ToDateTime(dateInicio.EditValue).Date == Convert.ToDateTime(dateFinal.EditValue).Date) || (Convert.ToDateTime(dateInicio.EditValue).Date > Convert.ToDateTime(dateFinal.EditValue).Date))
                {
                    Parametros.General.ValidateError(dateInicio, errDateField, Parametros.Properties.Resources.MSGVALIDARFECHA, true);
                    Parametros.General.ValidateError(dateFinal, errDateField, Parametros.Properties.Resources.MSGVALIDARFECHA, true);
                }
                else
                {
                    Parametros.General.ValidateError(dateInicio, errDateField, "", false);
                    Parametros.General.ValidateError(dateFinal, errDateField, "", false);
                }

                layoutControlGroupFecha.Text = "Lista de Precio Semana del " + Convert.ToDateTime(dateInicio.EditValue).ToShortDateString() + " al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
            }

        }

        private void bgvEs_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(bgvEs.GetFocusedRowCellValue(e.Column))))
                    bgvEs.SetFocusedRowCellValue(e.Column, 0m);
                else
                {
                    if (Convert.ToDecimal(bgvEs.GetFocusedRowCellValue(e.Column)) > 0)
                    {
                        decimal NewPrice = Convert.ToDecimal(bgvEs.GetFocusedRowCellValue(e.Column));

                        int IDProd = 0;
                        IDProd = Convert.ToInt32(Convert.ToString(e.Column.FieldName).Substring(0, 1));


                        var obj = (from pcd in db.PrecioCombustibleDetalles
                                   join pc in db.PrecioCombustibles on pcd.PrecioCombustibleID equals pc.ID
                                   where pcd.EstacionServicioID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns[0])))
                                   && pcd.SubEstacionID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns["IDSUS"])))
                                   && pcd.ProductoID == IDProd
                                   && pcd.PrecioDiferenciado == e.Column.FieldName.Length > 1 ? true : false
                                   select new { pcd.Precio, pc.FechaInicial, pc.FechaFinal }).OrderByDescending(o => o.FechaFinal);

                        if (obj.Count() > 0)
                        {
                            decimal OldPrice = obj.First().Precio;
                            decimal valor = Decimal.Round(((NewPrice / OldPrice) * 100), 2, MidpointRounding.AwayFromZero);

                            if (NewPrice > OldPrice)
                            {
                                if ((valor - 100) > MargenVariacion)
                                {
                                    bgvEs.SetColumnError(e.Column, "El monto del precio supera la variación porcentual definida: " + MargenVariacion.ToString() + "%", ErrorType.Warning);

                                }
                                else
                                    bgvEs.SetColumnError(e.Column, "", ErrorType.None);

                            }
                            else if (NewPrice < OldPrice)
                            {
                                if ((100 - valor) > MargenVariacion)
                                {
                                    bgvEs.SetColumnError(e.Column, "El monto del precio supera la variación porcentual definida: " + MargenVariacion.ToString() + "%", ErrorType.Warning);

                                }
                                else
                                    bgvEs.SetColumnError(e.Column, "", ErrorType.None);
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void btnSelectAllES_Click(object sender, EventArgs e)
        {
            gvES.ActiveFilter.Clear();
            for (int i = 0; i <= gvES.RowCount; i++)
            {
                gvES.SetRowCellValue(i, "SelectedES", true);
                gvES.FocusedRowHandle = i;
            }
        }

        private void btnUnselectAllES_Click(object sender, EventArgs e)
        {
            gvES.ActiveFilter.Clear();
            for (int i = 0; i <= gvES.RowCount; i++)
            {
                gvES.SetRowCellValue(i, "SelectedES", false);
                gvES.FocusedRowHandle = i;
            }
        }

        private void gvES_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (Begin)
                {
                    if (e.Column == colSelectedES)
                    {
                        if (Convert.ToBoolean(gvES.GetFocusedRowCellValue(colSelectedES)))
                        {
                            dtPrecioCombustible.DefaultView.RowFilter = "IDP = '" + Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDES)) + "' AND IDSUS = '" + Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDSUS1)) + "'";
                            if (dtPrecioCombustible.DefaultView.Count <= 0)
                            {
                                DataRow dr = dtPrecioCombustible.NewRow();
                                dr["IDP"] = Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDES));
                                dr["IDSUS"] = Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDSUS1));
                                dr["ES"] = Convert.ToString(gvES.GetFocusedRowCellValue(colNombre));

                                for (int i = 3; i < (bandPrecio.Columns.Count + bandPrecioDif.Columns.Count + 3) ; i++)
                                {
                                    dr[i] = 0.0;
                                }

                                dtPrecioCombustible.Rows.Add(dr);

                                this.gvES.RefreshData();
                            }

                            dtPrecioCombustible.DefaultView.RowFilter = "";
                        }
                        else if (!Convert.ToBoolean(gvES.GetFocusedRowCellValue(colSelectedES)))
                        {
                            dtPrecioCombustible.DefaultView.RowFilter = "IDP = '" + Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDES)) + "' AND IDSUS = '" + Convert.ToInt32(gvES.GetFocusedRowCellValue(colIDSUS1)) + "'";
                            if (dtPrecioCombustible.DefaultView.Count > 0)
                            {
                                dtPrecioCombustible.DefaultView.Delete(0);
                            }
                            dtPrecioCombustible.DefaultView.RowFilter = "";

                            this.gvES.RefreshData();
                        }
                    }

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