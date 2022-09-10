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
    public partial class DialogCostoPorTurno : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsg = false;
        private bool Editable;
        private DataTable dtPrecioCombustible;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecio;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecioDif;
        private decimal MargenVariacion = Parametros.Config.VariacionPorcentualPrecio();


        private DateTime FechaInicio
        {
            get { return Convert.ToDateTime(dateInicio.EditValue); }
            set { dateInicio.EditValue = value; }
        }

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
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

        public DialogCostoPorTurno()
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
                
                //var listSucursales = (from ES in db.EstacionServicios
                //                      join ses in db.SubEstacions on ES.ID equals ses.EstacionServicioID into sus
                //                      from su in sus.DefaultIfEmpty()
                //                      join pcd in db.PrecioCombustibleDetalles on ES.ID equals pcd.EstacionServicioID
                //                      where ES.Activo
                //                      select new
                //                       { 
                //                           IDES = ES.ID,
                //                           //CodigoEstacion = ES.Codigo,
                //                           IDSubEstacion = su.ID == null ? 0 : su.ID,
                //                           //CodigoSubEstacion = String.IsNullOrEmpty(su.Codigo) ? "0" : su.Codigo,
                //                           NombreES = su.ID == null ? ES.Nombre : su.Nombre,
                //                           SelectedES = ES.Activo,
                //                           ES.ZonaID
                //                       }).Distinct().OrderBy(z => z.ZonaID);//.ThenBy(n => n.Nombre);

                lkES.Properties.DataSource = from es in db.EstacionServicios
                                             where es.Activo
                                             select new { es.ID, es.Codigo, es.Nombre, Display = es.Codigo + " - " + es.Nombre };
                lkES.Properties.DisplayMember = "Display";
                lkES.Properties.ValueMember = "ID";


                //this.dtEstacionesServicios = Parametros.General.LINQToDataTable(listSucursales);

                FillBands();
                IDEstacionServicio = Parametros.General.EstacionServicioID;
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

                this.bandPrecio.Caption = "COSTO FULL (C$ por Galones)";
                this.bandPrecio.MinWidth = 20;
                this.bandPrecio.OptionsBand.AllowMove = false;
                this.bandPrecio.OptionsBand.AllowPress = false;
                this.bandPrecio.OptionsBand.ShowCaption = true;
                this.bandPrecio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.bandPrecio.Width = 300;

                bandPrecioDif = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();

                this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandPrecioDif});

                this.bandPrecioDif.Caption = "COSTO DIFERENCIADO (C$ por Galones)";
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
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
       
        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(dateInicio, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(spNroTurno, errRequiredField);
        }

        public bool ValidarCampos()
        {
            try
            {
                if (dateInicio.EditValue == null || Convert.ToInt32(lkES.EditValue) <= 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (this.layoutControlItemSub.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
                {
                    if (lkSub.EditValue == null)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }
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
                    Entidad.CostoCombustible PC = new Entidad.CostoCombustible();

                    PC.UsuarioID = Parametros.General.UserID;
                    PC.FechaInicial = FechaInicio;
                    PC.FechaFinal = FechaInicio;
                    PC.Turno = Convert.ToInt32(this.spNroTurno.EditValue);
                    PC.EsTurnoEspecial = this.chkEspecial.Checked;

                    db.CostoCombustibles.InsertOnSubmit(PC);
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                    "Se creó Lista de costo para un Turno específico: " + PC.FechaInicial , this.Name);

                    db.SubmitChanges();


                    int val = 0;
                    foreach (DataRow det in PrecioCombustibleDT.Rows)
                    {
                        
                        for (int i = 0; i < bandPrecio.Columns.Count; i++)
                        {
                            if (Convert.ToInt32(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i])) > 0)
                            {

                                Entidad.CostoCombustibleDetalle PCD = new Entidad.CostoCombustibleDetalle();

                                PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i]));
                                PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                PCD.PrecioDiferenciado = false;
                                PCD.CostoCombustible = PC;

                                db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se creó costo Full para la semana: " + PC.FechaInicial, this.Name);

                                db.SubmitChanges();
                            }
                        }

                        for (int i = 0; i < bandPrecioDif.Columns.Count; i++)
                        {
                            if (Convert.ToInt32(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i])) > 0)
                            {
                                Entidad.CostoCombustibleDetalle PCD = new Entidad.CostoCombustibleDetalle();

                                PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i]));
                                PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                PCD.PrecioDiferenciado = true;
                                PCD.CostoCombustible = PC;

                                db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se creó costo Diferencial para la semana: " + PC.FechaInicial, this.Name);

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
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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


                        var obj = (from pcd in db.CostoCombustibleDetalles
                                   join pc in db.CostoCombustibles on pcd.PrecioCombustibleID equals pc.ID
                                   where pcd.EstacionServicioID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns[0])))
                                   && pcd.SubEstacionID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns["IDSUS"])))
                                   && pcd.ProductoID == IDProd
                                   && pcd.PrecioDiferenciado == e.Column.FieldName.Length > 1 ? true : false
                                   select new { pcd.Precio, pc.FechaInicial, pc.FechaFinal }).OrderByDescending(o => o.FechaFinal);

                        /*
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
                        */
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
        
        #endregion

        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lkES.EditValue) > 0)
            {
                try
                {
                    if (db.SubEstacions.Count(o => o.EstacionServicioID.Equals(IDEstacionServicio)) > 0)
                    {
                        this.layoutControlItemSub.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //**SubEstación
                        lkSub.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(IDEstacionServicio))).ToList();
                        lkSub.Properties.DisplayMember = "Nombre";
                        lkSub.Properties.ValueMember = "ID";
                        dtPrecioCombustible.Rows.Clear();
                        return;
                    }
                    else
                    {
                        this.layoutControlItemSub.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        this.lkSub.EditValue = null;
                        this.spNroTurno.Properties.MaxValue = db.EstacionServicios.Single(es => es.ID.Equals(IDEstacionServicio)).NumeroTurnos;

                        //Agregar la fila para los precios
                        dtPrecioCombustible.Rows.Clear();
                        DataRow dr = dtPrecioCombustible.NewRow();
                        dr["IDP"] = Convert.ToInt32(IDEstacionServicio);
                        dr["IDSUS"] = 0;
                        dr["ES"] = db.EstacionServicios.Single(es => es.ID.Equals(IDEstacionServicio)).Nombre;

                        for (int i = 3; i < (bandPrecio.Columns.Count + bandPrecioDif.Columns.Count + 3); i++)
                        {
                            dr[i] = 0.0;
                        }

                        dtPrecioCombustible.Rows.Add(dr);

                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                }
                
            }
        }

        private void lkSub_EditValueChanged(object sender, EventArgs e)
        {
            if (this.lkSub.EditValue != null)
            {
                try
                {
                    this.spNroTurno.Properties.MaxValue = db.SubEstacions.Single(ses => ses.ID.Equals(Convert.ToInt32(lkSub.EditValue))).NumeroTurnos;

                    //Agregar la fila para los precios
                    dtPrecioCombustible.Rows.Clear();
                    DataRow dr = dtPrecioCombustible.NewRow();
                    dr["IDP"] = Convert.ToInt32(IDEstacionServicio);
                    dr["IDSUS"] = Convert.ToInt32(lkSub.EditValue);
                    dr["ES"] = db.EstacionServicios.Single(es => es.ID.Equals(IDEstacionServicio)).Nombre;

                    for (int i = 3; i < (bandPrecio.Columns.Count + bandPrecioDif.Columns.Count + 3); i++)
                    {
                        dr[i] = 0.0;
                    }

                    dtPrecioCombustible.Rows.Add(dr);
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                }
            }
        }
        
    }
}