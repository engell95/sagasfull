using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Forms;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptPagoArqueos : XtraReport
    {
        public SubRptPagoArqueos(float AnchoTabla, decimal ValorTotalVenta, decimal DifValorTotalVenta, object dataFormasPago, object dataDescuento, decimal ValorDescuentoTotal,
            decimal VentasContado, decimal EfectivoRecibido, decimal SobranteFaltante, decimal DiferenciaRecibida)
        {
            try
            {
                InitializeComponent();
                //Valor Total Venta
                this.xrCellValorTotalVenta.Text = ValorTotalVenta.ToString("#,0.00");

                if (!DifValorTotalVenta.Equals(0))
                {
                    this.xrCellValorTotalVenta.ForeColor = Color.Red;
                    //this.xrCellValorTotalVenta.Font. = true;
                }
                //Formas de Pago
                this.xrTableTitulo.WidthF = AnchoTabla;
                this.xrCellNada.WidthF = AnchoTabla / 2;
                this.xrCellValorTotalVenta.WidthF = AnchoTabla / 2;
                this.xrTableFormasTiltulos.WidthF = AnchoTabla;
                this.xrRowNombre.Width = Convert.ToInt32(xrCellNada.WidthF - 8);
                this.xrColValor.Width = Convert.ToInt32(xrCellValorTotalVenta.WidthF - 8);
                this.xrPivotGridFormas.DataSource = dataFormasPago;
                
                //Decuento
                this.xrTableDescuentoTitulo.WidthF = AnchoTabla;
                this.xrPivotGridDescuento.DataSource = dataDescuento;
                this.xrTableTotalDescuento.WidthF = AnchoTabla;
                this.xrCellTotalDescuento.WidthF = AnchoTabla / 2;
                this.xrCellTotalValorDescuento.WidthF = AnchoTabla / 2;
                this.xrCellTotalValorDescuento.Text = ValorDescuentoTotal.ToString("#,0.00");

                //Tabla Detalle
                this.xrTableDetail.WidthF = AnchoTabla;
                this.xrCellVentasContado.WidthF = AnchoTabla / 2;
                this.xrCellValorVentasContado.WidthF = AnchoTabla / 2;
                this.xrCellTotalEfectivoRecibido.WidthF = AnchoTabla / 2;
                this.xrCellValorTotalEfectivoRecibido.WidthF = AnchoTabla / 2;
                this.xrCellValorVentasContado.Text = VentasContado.ToString("#,0.00");
                this.xrCellValorTotalEfectivoRecibido.Text = EfectivoRecibido.ToString("#,0.00");


                #region <<< Sobrante/Faltante >>>

                bool mostrar = false;
                this.xrTableFooter.WidthF = AnchoTabla;
                if (!SobranteFaltante.Equals(0))
                {
                    mostrar = false;
                    this.xrCelltxt1.Text = "";
                    this.xrCelltxt1.WidthF = AnchoTabla;
                    this.xrCelltxt2.WidthF = AnchoTabla / 2;
                    this.xrCelltxt22.WidthF = AnchoTabla / 2;
                    this.xrCelltxt2.Text = (SobranteFaltante.Equals(0) ? "Sin Diferencia" : (SobranteFaltante > 0 ? "Sobrante en Arqueo:" : "Faltante en Arqueo:"));
                    this.xrCelltxt22.Text = SobranteFaltante.ToString("#,0.00");
                    this.xrCelltxt2.BackColor = (SobranteFaltante.Equals(0) ? Color.LimeGreen : (SobranteFaltante > 0 ? Color.DodgerBlue : Color.Red));
                    this.xrCelltxt22.BackColor = (SobranteFaltante.Equals(0) ? Color.LimeGreen : (SobranteFaltante > 0 ? Color.DodgerBlue : Color.Red));
                    this.xrCelltxt2.ForeColor = Color.White;
                    this.xrCelltxt22.ForeColor = Color.White;
                    this.xrTableFooter.HeightF = 36f;
                    this.GroupFooter1.HeightF = 36f;

                }
                else
                {
                    if (DiferenciaRecibida <= 0)
                    {
                        this.xrRow01.Dispose();
                        this.xrRow02.Dispose();
                    }
                }


                if (DiferenciaRecibida > 0)
                {
                    mostrar = true;
                    this.xrCellTotalEfectivoRecibido.Text = "Sub Total Efectivo Recibido";
                    this.xrCelltxt1.WidthF = (float)(AnchoTabla / 2);
                    this.xrCelltxt11.WidthF = (float)(AnchoTabla / 2);
                    this.xrCelltxt1.BackColor = (SobranteFaltante.Equals(0) ? Color.LimeGreen : (SobranteFaltante > 0 ? Color.DodgerBlue : Color.Red));
                    this.xrCelltxt11.BackColor = (SobranteFaltante.Equals(0) ? Color.LimeGreen : (SobranteFaltante > 0 ? Color.DodgerBlue : Color.Red));
                    this.xrCelltxt1.ForeColor = Color.White;
                    this.xrCelltxt11.ForeColor = Color.White;
                    this.xrCelltxt1.Text = (SobranteFaltante.Equals(0) ? "Sin Diferencia" : (SobranteFaltante > 0 ? "Sobrante en Arqueo:" : "Faltante en Arqueo:"));
                    this.xrCelltxt11.Text = SobranteFaltante.ToString("#,0.00");
                    this.xrCelltxt2.WidthF = AnchoTabla / 2;
                    this.xrCelltxt22.WidthF = AnchoTabla / 2;
                    this.xrCelltxt22.BackColor = Color.LightGray;
                    this.xrCelltxt22.ForeColor = Color.Black;
                    this.xrCelltxt2.BackColor = Color.LightGray;
                    this.xrCelltxt2.ForeColor = Color.Black;
                    this.xrCelltxt2.Text = "Diferencia Recibida:";
                    this.xrCelltxt22.Text = DiferenciaRecibida.ToString("#,0.00");
                    this.xrCelltxt3.WidthF = AnchoTabla / 2;
                    this.xrCelltxt33.WidthF = AnchoTabla / 2;
                    this.xrCelltxt4.WidthF = AnchoTabla / 2;
                    this.xrCelltxt44.WidthF = AnchoTabla / 2;
                    decimal valor = 0m;

                    valor = (SobranteFaltante < 0 ? (DiferenciaRecibida - Math.Abs(SobranteFaltante)) : DiferenciaRecibida + SobranteFaltante);

                    this.xrCelltxt3.BackColor = 
                        (
                        valor >= 0 ? (valor.Equals(0) ? Color.LawnGreen : Color.DodgerBlue) : Color.Red
                        );
                    this.xrCelltxt33.BackColor =
                        (
                        valor >= 0 ? (valor.Equals(0) ? Color.LawnGreen : Color.DodgerBlue) : Color.Red
                        );

                    this.xrCelltxt3.Text =
                        (
                        valor >= 0 ? (valor.Equals(0) ? "Sin Diferencia:" : "Sobrante en Diferencia:") : "Faltante en Diferencia:"
                        );
                    this.xrCelltxt3.ForeColor = Color.White;
                    this.xrCelltxt33.ForeColor = Color.White;
                    this.xrCelltxt33.Text = valor.ToString("#,0.00");
                    this.xrCelltxt4.Text = "TOTAL EFECTIVO RECIBIDO";
                    this.xrCelltxt44.Text = (EfectivoRecibido + DiferenciaRecibida).ToString("#,0.00");
                    this.xrCelltxt44.BackColor = Color.LightGray;
                }

                if (!mostrar)
                {
                    this.xrRow03.Dispose();
                    this.xrRow04.Dispose();
                    this.xrCelltxt11.Dispose();
                }

                #endregion

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void xrCellValores_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            if (cell.Text.Equals("0.00") || cell.Text.Equals("0.000"))
                cell.Text = "-";
        }

        private void xrPivotGridProducto_PrintFieldValue(object sender, DevExpress.XtraPivotGrid.CustomExportFieldValueEventArgs e)
        {
            try
            {
                if (e.Field.FieldName.Equals("ProductoNombre"))
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Parametros.General.ListColTanques.Find(f => f.Nombre.Equals(e.Value.ToString())).Color));
                    e.Appearance.ForeColor = Color.White;

                    
                }
            }
            catch {e.Appearance.BackColor = Color.White; }
        }

        private void xrPivotGridFormas_PrintCell(object sender, DevExpress.XtraPivotGrid.CustomExportCellEventArgs e)
        {
            if (e.RowField == null)
                e.Appearance.BackColor = Color.LightGray;
        }

        private void xrPivotGridDescuento_PrintFieldValue(object sender, DevExpress.XtraPivotGrid.CustomExportFieldValueEventArgs e)
        {
            
        }

        private void xrPivotGridDescuento_CustomCellValue(object sender, DevExpress.XtraPivotGrid.PivotCellValueEventArgs e)
        {
            
        }

        private void xrPivotGridFormas_PrintCell(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportCellEventArgs e)
        {
            if (e.RowField == null)
                e.Appearance.BackColor = Color.LightGray;
        }

        private void xrPivotGridDescuento_CustomCellValue(object sender, DevExpress.XtraReports.UI.PivotGrid.PivotCellValueEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.Equals(0m))
                    e.Value = "-";
            }
        }

        private void xrPivotGridDescuento_PrintFieldValue(object sender, DevExpress.XtraReports.UI.PivotGrid.CustomExportFieldValueEventArgs e)
        {
            try
            {
                if (e.Field.FieldName.Equals("ProductoNombre"))
                {
                    e.Appearance.BackColor = Color.FromArgb(Convert.ToInt32(Parametros.General.ListColTanques.Find(f => f.Nombre.Equals(e.Value.ToString())).Color));
                    e.Appearance.ForeColor = Color.White;
                }
            }
            catch { e.Appearance.BackColor = Color.White; }
        }

       
               
    }
}
