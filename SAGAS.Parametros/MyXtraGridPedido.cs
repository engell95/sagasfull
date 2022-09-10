using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Linq;
using System.IO;
using System.Reflection;

namespace SAGAS.Parametros
{
    public partial class MyXtraGridPedido : DevExpress.XtraEditors.XtraUserControl
    {
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandProducto;
        private DataTable dtPrecioCombustible;

        public MyXtraGridPedido(Entidad.SAGASDataClassesDataContext db, Entidad.Pedido EntidadAnterior)
        {
            try
            {
                InitializeComponent();

                //Terminales
                rpLkTerminal.DataSource = (from p in db.Terminals select new { p.ID, p.Nombre }); ;

                //ENTREGAR----
                rpCboEntrega.Items.AddRange(Parametros.Config.ListaEntregar());

                //Turnos----
                rpCboTurno.Items.AddRange(Parametros.Config.ListaTurno());

                dtPrecioCombustible = new DataTable();
                bandProducto = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();

                this.bgView.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandProducto});

                dtPrecioCombustible.Columns.Add("ID", typeof(Int32));
                dtPrecioCombustible.Columns.Add("TerminalID", typeof(Int32));
                dtPrecioCombustible.Columns.Add("Entrega", typeof(String));
                dtPrecioCombustible.Columns.Add("Observacion", typeof(String));
                dtPrecioCombustible.Columns.Add("FechaSolicitud", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("TurnoSolicitud", typeof(String));
                dtPrecioCombustible.Columns.Add("FechaSalida", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("FechaEntrega", typeof(DateTime));
                dtPrecioCombustible.Columns.Add("HoraEntrega", typeof(DateTime));

                this.bandProducto.Caption = "PRODUCTOS GALONES";
                this.bandProducto.MinWidth = 20;
                this.bandProducto.OptionsBand.AllowMove = false;
                this.bandProducto.OptionsBand.AllowPress = false;
                this.bandProducto.OptionsBand.ShowCaption = true;
                this.bandProducto.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                this.bandProducto.Width = 300;

                foreach (var item in db.OrdenCombustiblePedidos.OrderBy(o => o.Orden))
                {

                    var orden = db.Productos.Single(o => o.ID.Equals(item.ProductoID));

                    dtPrecioCombustible.Columns.Add(orden.ID.ToString(), typeof(Decimal));
                    DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProd = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                    //**Columna
                    colProd.Caption = orden.Nombre;
                    colProd.FieldName = orden.ID.ToString();
                    colProd.Name = "col" + orden.ID.ToString();
                    colProd.Visible = true;
                    colProd.Width = 100;
                    colProd.AppearanceHeader.Options.UseForeColor = true;
                    colProd.AppearanceHeader.Options.UseBackColor = true;
                    colProd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                    colProd.AppearanceHeader.ForeColor = Color.White;
                    colProd.DisplayFormat.FormatString = "N0";
                    colProd.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;

                    if (db.Tanques.Where(t => t.ProductoID == orden.ID).Count() > 0)
                    {
                        colProd.AppearanceHeader.BackColor = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == orden.ID).First().Color));
                        colProd.AppearanceHeader.BackColor2 = Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == orden.ID).First().Color));

                    }

                    //**Repositorio
                    DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpPrecio = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                    rpPrecio.AutoHeight = false;
                    rpPrecio.AllowMouseWheel = false;
                    rpPrecio.EditFormat.FormatString = "N0";
                    rpPrecio.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    rpPrecio.Mask.EditMask = "N0";

                    rpPrecio.Buttons.Clear();
                    //    rpPrecio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                    //new DevExpress.XtraEditors.Controls.EditorButton()});
                    rpPrecio.MaxValue = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
                    rpPrecio.Name = "rpItem" + orden.ID.ToString();

                    //rpPrecio.EditValueChanged += new System.EventHandler(spinEdits_EditValueChanged);

                    this.bgView.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            colProd});

                    this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            rpPrecio});


                    bandProducto.Columns.Add(colProd);
                    colProd.ColumnEdit = rpPrecio;

                }


                dtPrecioCombustible.Columns.Add("TOTAL", typeof(Decimal));

                //**Columna
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();

                colTotal.Caption = "TOTAL";
                colTotal.FieldName = "TOTAL";
                colTotal.Name = "colTOTAL";
                colTotal.Visible = true;
                colTotal.Width = 100;
                colTotal.AppearanceHeader.Options.UseForeColor = true;
                colTotal.AppearanceHeader.Options.UseBackColor = true;
                colTotal.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                colTotal.AppearanceHeader.ForeColor = Color.Black;
                colTotal.DisplayFormat.FormatString = "N0";
                colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                colTotal.OptionsColumn.AllowEdit = colTotal.OptionsColumn.AllowFocus = false;
                bandProducto.Columns.Add(colTotal);


                EntidadAnterior.PedidoCombustibles.ToList().ForEach(x =>
                {
                    DataRow dr = dtPrecioCombustible.NewRow();

                    dr["ID"] = x.ID;
                    dr["TerminalID"] = x.TerminalID;
                    dr["Observacion"] = x.Observacion;

                    if (x.FechaSolicitud.HasValue)
                        dr["FechaSolicitud"] = x.FechaSolicitud;

                    dr["Entrega"] = x.Entrega;
                    dr["TurnoSolicitud"] = x.TurnoSolicitud;

                    if (x.FechaSalida.HasValue)
                        dr["FechaSalida"] = x.FechaSalida;

                    if (x.FechaEntrega.HasValue)
                        dr["FechaEntrega"] = x.FechaEntrega;

                    if (x.HoraEntrega.HasValue)
                        dr["HoraEntrega"] = x.HoraEntrega;

                    decimal v = 0m;

                    x.PedidoCombustibleDetalles.ToList().ForEach(p =>
                    {
                        dr[p.ProductoID.ToString()] = p.Galones;
                        v += p.Galones;
                    });

                    dr["TOTAL"] = v;

                    dtPrecioCombustible.Rows.Add(dr);
                });

                this.gridDetalle.DataSource = dtPrecioCombustible;
                bgView.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {

        }

        
    }
}
