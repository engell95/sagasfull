using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Threading;
using System.Threading.Tasks;

namespace SAGAS.Ventas.Forms
{
    public partial class FormListaPrecio : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormListaPrecio()
        {
            InitializeComponent();
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void FormListaPrecio_Shown(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected async override void FillControl()
        {
            try
            {
                await GetPrecios().ConfigureAwait(false);
                
                    /*from ap in db.AlmacenProductos
                                               join a in db.Almacens on ap.AlmacenID equals a.ID
                                               join p in db.Productos on ap.ProductoID equals p.ID
                                               join kx in 
                                                   ((from k in db.Kardexes
                                                   join m in db.Movimientos on k.MovimientoID equals m.ID
                                                     where (m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(43) || m.MovimientoTipoID.Equals(69)) & m.Anulado.Equals(false) &
                                                   k.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) &
                                                   k.SubEstacionID.Equals(Parametros.General.SubEstacionID)
                                                     select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).GroupBy(g => g.ProductoID)) on ap.ProductoID equals kx.Key
                                               join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                                               join ar in db.Areas on c.AreaID equals ar.ID
                                               join u in db.UnidadMedidas on p.UnidadMedidaID equals u.ID
                                               where a.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) & a.SubEstacionID.Equals(Parametros.General.SubEstacionID) & p.Activo & !p.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())
                                               group ap by new { ap.ProductoID, Area = ar.Nombre, Producto = p.Codigo + " | " + p.Nombre, UnidadNombre = u.Nombre, kx.FirstOrDefault().CostoEntrada } into gr
                                               select new
                                               {
                                                  ProductoID = gr.Key.ProductoID,
                                                  Producto = gr.Key.Producto,
                                                  Area = gr.Key.Area,
                                                  UnidadNombre = gr.Key.UnidadNombre,
                                                  PrecioTotal = gr.Max(m => m.PrecioTotal),
                                                  PrecioVenta = gr.Max(m => m.PrecioVenta),
                                                  PrecioSugerido = gr.FirstOrDefault().PrecioSugerido,
                                                  UltimoCosto = gr.FirstOrDefault().Costo,
                                                  Cantidad = gr.Sum(s => s.Cantidad)
                                               };
                    */

               

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        async Task GetPrecios()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                
                await Task.Delay(10);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var lista = db.GetListaPrecio(Parametros.General.EstacionServicioID, Parametros.General.SubEstacionID);
                    
                bdsManejadorDatos.DataSource = Parametros.General.LINQToDataTable(lista);

                this.grid.DataSource = bdsManejadorDatos;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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

            FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        private void gvData_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == gridColumn3)
                {
                    decimal vPrecio = 0;
                    decimal vPrecioSinIva = 0;
                    if (gvData.GetRowCellValue(e.RowHandle, "ProductoID") != DBNull.Value)
                    {
                        if (gvData.GetRowCellValue(e.RowHandle, "PrecioTotal") != DBNull.Value)
                        {
                            vPrecio = Decimal.Round(Convert.ToDecimal(gvData.GetRowCellValue(e.RowHandle, "PrecioTotal")), 2, MidpointRounding.AwayFromZero);

                            if (!db.Productos.Single(p => p.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(e.RowHandle, "ProductoID")))).ExentoIVA)
                                vPrecioSinIva = Decimal.Round((vPrecio / 1.15m), 6, MidpointRounding.AwayFromZero);
                            else
                                vPrecioSinIva = Decimal.Round(vPrecio, 6, MidpointRounding.AwayFromZero);

                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            var almacenes = from al in db.AlmacenProductos.Where(o => o.ProductoID.Equals(Convert.ToInt32(gvData.GetRowCellValue(e.RowHandle, "ProductoID"))))
                                            join a in db.Almacens.Where(l => l.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) & l.SubEstacionID.Equals(Parametros.General.SubEstacionID))
                                            on al.AlmacenID equals a.ID
                                            select al;

                            almacenes.ToList().ForEach(obj =>
                                {
                                    db.AlmacenProductos.Single(o => o.ID.Equals(obj.ID)).PrecioTotal = vPrecio;
                                    db.AlmacenProductos.Single(o => o.ID.Equals(obj.ID)).PrecioVenta = vPrecioSinIva;
                                    db.SubmitChanges();
                                });

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                        "Se modificó el precio del producto: " + gvData.GetRowCellValue(e.RowHandle, "Producto").ToString(), this.Name);
                            this.timerMSG.Start();
                            gvData.SetRowCellValue(e.RowHandle, "PrecioVenta", vPrecioSinIva);
                        }
                        else
                        {
                            gvData.SetRowCellValue(e.RowHandle, "PrecioTotal", 0);
                            gvData.SetRowCellValue(e.RowHandle, "PrecioVenta", 0);
                        }
                    }
                    else
                        Parametros.General.DialogMsg("La lista de producto no fue cargada correctamente.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }


        #endregion

        
    }
}
