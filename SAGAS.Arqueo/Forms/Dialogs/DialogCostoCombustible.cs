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
using DevExpress.XtraBars;
using System.Text.RegularExpressions;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogCostoCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCostoCombustible MDI;
        internal Entidad.CostoCombustible EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private DataTable dtPrecioCombustible;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecio;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandPrecioDif;
        private decimal MargenVariacion = Parametros.Config.VariacionPorcentualPrecio();
        private DateTime _LastDate;

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

        public DialogCostoCombustible()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
            FillControl();
            ValidarerrRequiredField();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var UltimaFecha = db.CostoCombustibles.OrderByDescending(o => o.FechaFinal);

                _LastDate = (UltimaFecha.Count() > 0 ? Convert.ToDateTime(UltimaFecha.First().FechaFinal) : Convert.ToDateTime(db.GetDateServer()));

                if (Editable)
                {
                    FechaInicio = EntidadAnterior.FechaInicial;
                    FechaFinal = EntidadAnterior.FechaFinal;
                }
                else
                {
                    if (UltimaFecha.Count() > 0)
                    {
                        dateInicio.EditValue = UltimaFecha.First().FechaFinal.AddDays(1);
                        dateFinal.EditValue = Convert.ToDateTime(dateInicio.EditValue).Date.AddDays(Parametros.Config.RangoDiasPrecioCombustible() - 1);
                    }
                    else
                    {
                        dateInicio.EditValue = db.GetDateServer();
                        dateFinal.EditValue = Convert.ToDateTime(dateInicio.EditValue).Date.AddDays(Parametros.Config.RangoDiasPrecioCombustible() - 1);
                    }                     
                }

                FillPricesBand();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void FillPricesBand()
        {
            
            try
            {
            
            dtPrecioCombustible = new DataTable(); 
            bandPrecio = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();


            this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandPrecio});

            dtPrecioCombustible.Columns.Add("IDP", typeof(Int32));
            dtPrecioCombustible.Columns.Add("ES", typeof(String));
            dtPrecioCombustible.Columns.Add("IDSUS", typeof(Int32));

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
                rpPrecio.AllowMouseWheel = false;
                rpPrecio.Buttons.Clear();
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

                this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            rpPrecio});

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

                this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            rpPrecioDif});

                this.bandPrecioDif.Columns.Add(colProdDIF);
                colProdDIF.ColumnEdit = rpPrecioDif;
                cont++;
                ///***********************************///////////
                ///
                #endregion


            }


            //var obj = (from es in db.EstacionServicios
            //           where es.Activo
            //           select es).OrderBy(z => z.ZonaID).ThenBy(n => n.Nombre);

            var obj = (from es in db.EstacionServicios
                       join ses in db.SubEstacions on es.ID equals ses.EstacionServicioID into sus
                       from su in sus.DefaultIfEmpty()
                       where es.Activo 
                       select new
                       {
                           IDEstacion = es.ID,
                           CodigoEstacion = es.Codigo,
                           IDSubEstacion = su.ID == null ? 0 : su.ID,
                           CodigoSubEstacion = su.Codigo,
                           Nombre = su.ID == null ? es.Nombre : su.Nombre,
                           es.ZonaID
                       }).Distinct().OrderBy(z => z.ZonaID).ThenBy(n => n.Nombre);

            var ListValor = db.CostoCombustibleDetalles.Where(pcd => pcd.PrecioCombustibleID.Equals(EntidadAnterior.ID));

            foreach (var l in obj)
            {

                DataRow dr = dtPrecioCombustible.NewRow();
                dr["IDP"] = l.IDEstacion;
                dr["IDSUS"] = l.IDSubEstacion;
                dr["ES"] = l.Nombre;

                if (Editable)
                {
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
                }
                else
                {
                    for (int i = 3; i < cont; i++)
                    {

                        dr[i] = 0.0;
                    }
                }
                dtPrecioCombustible.Rows.Add(dr);

            }




            this.grid.DataSource = PrecioCombustibleDT;
                                                      

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

            if (Convert.ToDateTime(dateInicio.EditValue).Date > _LastDate.Date.AddDays(1))
            {
                Parametros.General.DialogMsg("El periodo de la lista de precios debe ser continuo." + Environment.NewLine, Parametros.MsgType.warning);
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
                    Entidad.CostoCombustible CC;

                    if (Editable)
                    { CC = db.CostoCombustibles.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        CC = new Entidad.CostoCombustible();
                        CC.UsuarioID = Parametros.General.UserID;
                    }

                        CC.FechaInicial = FechaInicio;
                        CC.FechaFinal = FechaFinal;


                        if (Editable)
                        {
                            DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(CC, 1));
                            DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                             "Se modificó la Lista de costo para la semana: " + EntidadAnterior.FechaInicial, this.Name, dtPosterior, dtAnterior);

                        }

                    else
                    {
                        db.CostoCombustibles.InsertOnSubmit(CC);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        "Se creó Lista de Precio para la semana: " + CC.FechaInicial, this.Name);

                    }

                    db.SubmitChanges();

                    int val = 0;
                    foreach (DataRow det in PrecioCombustibleDT.Rows)
                    {
                        

                        for (int i = 0; i < bandPrecio.Columns.Count; i++)
                        {
                            if (Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i])) >= 0)
                            {

                                Entidad.CostoCombustibleDetalle PCD;

                                var objPre = from pd in db.CostoCombustibleDetalles
                                                where pd.EstacionServicioID == Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]))
                                                && pd.SubEstacionID == Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]))
                                                && pd.ProductoID == Convert.ToInt32(bandPrecio.Columns[i].FieldName)
                                                && !pd.PrecioDiferenciado && pd.PrecioCombustibleID == CC.ID
                                                select pd;

                                if (objPre.Count() > 0)
                                {
                                    PCD = objPre.First();
                                    PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                    PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                    PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i]));
                                    PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                    PCD.PrecioDiferenciado = false;
                                    PCD.PrecioCombustibleID = CC.ID;

                                    if (Editable && objPre.Count() > 0)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PCD, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(objPre.First(), 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se modificó costo Full para la semana: " + EntidadAnterior.FechaInicial, this.Name, dtPosterior, dtAnterior);
                                    }

                                    else
                                    {
                                        db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                        "Se creó costo Full para la semana: " + CC.FechaInicial, this.Name);
                                    }
                                    db.SubmitChanges();
                                }
                                else if (objPre.Count().Equals(0) && Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i])) > 0)
                                {
                                    PCD = new Entidad.CostoCombustibleDetalle();
                                    PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                    PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                    PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecio.Columns[i]));
                                    PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                    PCD.PrecioDiferenciado = false;
                                    PCD.PrecioCombustibleID = CC.ID;

                                    if (Editable && objPre.Count() > 0)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PCD, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(objPre.First(), 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se modificó costo Full para la semana: " + EntidadAnterior.FechaInicial, this.Name, dtPosterior, dtAnterior);
                                    }

                                    else
                                    {
                                        db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                        "Se creó costo Full para la semana: " + CC.FechaInicial, this.Name);
                                    }
                                    db.SubmitChanges();
                                }

                                
                            }
                        }

                        for (int i = 0; i < bandPrecioDif.Columns.Count; i++)
                        {
                            if (Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i])) >= 0)
                            {
                                Entidad.CostoCombustibleDetalle PCD;

                                //from pd in db.PrecioCombustibleDetalles
                                //             where pd.EstacionServicioID == Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]))
                                //             && pd.ProductoID == Convert.ToInt32(bandPrecio.Columns[i].FieldName)
                                //             && pd.PrecioDiferenciado && pd.PrecioCombustibleID == PC.ID
                                //             select pd;

                                var objPreDif = from pd in db.CostoCombustibleDetalles
                                                where pd.EstacionServicioID == Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]))
                                                && pd.SubEstacionID == Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]))
                                                && pd.ProductoID == Convert.ToInt32(bandPrecio.Columns[i].FieldName)
                                                && pd.PrecioDiferenciado && pd.PrecioCombustibleID == CC.ID
                                                select pd;

                                if (objPreDif.Count() > 0)
                                {
                                    PCD = objPreDif.First();
                                    PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                    PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                    PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i]));
                                    PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                    PCD.PrecioDiferenciado = true;
                                    PCD.PrecioCombustibleID = CC.ID;

                                    if (Editable && objPreDif.Count() > 0)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PCD, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(objPreDif.First(), 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se modificó Precio Diferencial para la semana: " + EntidadAnterior.FechaInicial, this.Name, dtPosterior, dtAnterior);

                                    }

                                    else
                                    {
                                        db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                        "Se creó Precio Diferencial para la semana: " + CC.FechaInicial, this.Name);

                                    }


                                    db.SubmitChanges();

                                }
                                else if (objPreDif.Count().Equals(0) && Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i])) > 0)
                                {
                                    PCD = new Entidad.CostoCombustibleDetalle();
                                    PCD.EstacionServicioID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[0]));
                                    PCD.SubEstacionID = Convert.ToInt32(bgvEs.GetRowCellValue(val, bandEs.Columns[1]));
                                    PCD.Precio = Convert.ToDecimal(bgvEs.GetRowCellValue(val, bandPrecioDif.Columns[i]));
                                    PCD.ProductoID = Convert.ToInt32(bandPrecio.Columns[i].FieldName);
                                    PCD.PrecioDiferenciado = true;
                                    PCD.PrecioCombustibleID = CC.ID;

                                    if (Editable && objPreDif.Count() > 0)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(PCD, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(objPreDif.First(), 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se modificó Precio Diferencial para la semana: " + EntidadAnterior.FechaInicial, this.Name, dtPosterior, dtAnterior);

                                    }

                                    else
                                    {
                                        db.CostoCombustibleDetalles.InsertOnSubmit(PCD);
                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                        "Se creó Precio Diferencial para la semana: " + CC.FechaInicial, this.Name);

                                    }


                                    db.SubmitChanges();
                                }

                               
                            }

                        }
                        

                        val ++;

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

        private void ShowPupUpMenu()
        {
            try
            {

                DevExpress.XtraBars.BarManager barManager1;
                PopupMenu popupMenuCells;
                DevExpress.XtraBars.BarButtonItem menuButtonCopiar = new DevExpress.XtraBars.BarButtonItem();
                DevExpress.XtraBars.BarButtonItem menuButtonPegar = new DevExpress.XtraBars.BarButtonItem();

                barManager1 = new BarManager();
                barManager1.Form = this;
                popupMenuCells = new DevExpress.XtraBars.PopupMenu(barManager1);

                menuButtonCopiar.Caption = "C&opiar";
                menuButtonCopiar.Glyph = Properties.Resources.page_white_stack;
                menuButtonCopiar.Id = 1;
                menuButtonCopiar.ItemClick += new ItemClickEventHandler(menuButtonCopiar_ItemClick);

                menuButtonPegar.Caption = "P&egar";
                menuButtonPegar.Glyph = Properties.Resources.paste_plain;
                menuButtonPegar.Id = 2;
                menuButtonPegar.ItemClick += new ItemClickEventHandler(menuButtonPegar_ItemClick);
                barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { menuButtonCopiar, menuButtonPegar });

                popupMenuCells.ItemLinks.Add(barManager1.Items[0]);
                popupMenuCells.ItemLinks.Add(barManager1.Items[1]);
                barManager1.SetPopupContextMenu(this.grid, popupMenuCells);

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
           
            if (!Guardar()) return;
            
            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
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

                layoutControlGroupFecha.Text = "Lista de costo Semana del " + Convert.ToDateTime(dateInicio.EditValue).ToShortDateString() + " al " + Convert.ToDateTime(dateFinal.EditValue).ToShortDateString();
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


                        var obj = (from pcd in db.CostoCombustibleDetalles
                                   join pc in db.CostoCombustibles on pcd.PrecioCombustibleID equals pc.ID
                                   where pcd.EstacionServicioID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns[0])))
                                   && pcd.SubEstacionID.Equals(Convert.ToInt32(bgvEs.GetFocusedRowCellValue(bgvEs.Columns["IDSUS"])))
                                   && pcd.ProductoID == IDProd
                                   && pcd.PrecioDiferenciado == e.Column.FieldName.Length > 1 ? true : false
                                   select new { pcd.Precio, pc.FechaInicial, pc.FechaFinal }).OrderByDescending(o => o.FechaFinal);

                        //if (obj.Count() > 0)
                        //{
                        //    decimal OldPrice = obj.First().Precio;
                        //    decimal valor = Decimal.Round(((NewPrice / OldPrice) * 100), 2, MidpointRounding.AwayFromZero);

                        //    if (NewPrice > OldPrice)
                        //    {
                        //        if ((valor - 100) > MargenVariacion)
                        //        {
                        //            bgvEs.SetColumnError(e.Column, "El monto del costo supera la variación porcentual definida: " + MargenVariacion.ToString() + "%", ErrorType.Warning);

                        //        }
                        //        else
                        //            bgvEs.SetColumnError(e.Column, "", ErrorType.None);

                        //    }
                        //    else if (NewPrice < OldPrice)
                        //    {
                        //        if ((100 - valor) > MargenVariacion)
                        //        {
                        //            bgvEs.SetColumnError(e.Column, "El monto del costo supera la variación porcentual definida: " + MargenVariacion.ToString() + "%", ErrorType.Warning);

                        //        }
                        //        else
                        //            bgvEs.SetColumnError(e.Column, "", ErrorType.None);
                        //    }

                        //}
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void bgvEs_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            ShowPupUpMenu();
        }
                 
        private void menuButtonCopiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.bgvEs.CopyToClipboard();
        }

        private void menuButtonPegar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var filas = bgvEs.GetSelectedRows();
                string lista = Clipboard.GetText();

                List<String> datos = new List<String>();
                string[] lineas = Regex.Split(lista, "\r\n");


                int i = 0;
                foreach (int obj in filas)
                {
                    int j = 0;
                    foreach (string item in lineas.ElementAt(i).Split('\t'))
                    {

                        bgvEs.SetRowCellValue(obj, bgvEs.GetSelectedCells(obj).ElementAt(j), item.ToString());
                        j++;
                    }
                    i++;
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