using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.Common;
using System.Globalization;
using System.Drawing;
using System.Reflection;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Drawing.Printing;
using System.Security.Cryptography;
using System.Configuration;
using System.Management;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes
{
    public class Util
    {
        struct ListDescuento
        {
            public int TipoDescuentoID;
            public string TipoDescuentoNombre;
            public int ProductoID;
            public string ProductoNombre;
            public decimal Valor;
        };

        struct ListDist
        {
            public int ID;
            public string ExtraxionNombre;
            public int ProductoID;            
            public string ProductoNombre;
            public int TanqueID;
            public string TanqueNombre;
            public decimal? Valor;
        };

        public static void PrintArqueoFast(Form Formulario, Entidad.SAGASDataViewsDataContext dbView,
          Entidad.SAGASDataClassesDataContext db, int ArqueoIslaID, int TurnoID, Entidad.ResumenDia RD,
          Parametros.TiposImpresion TI, bool ToPrint, Parametros.TiposArqueo TA, string Descripcion, DevExpress.XtraPrinting.Control.PrintControl printArea)
        {
            try
            {
                Parametros.General.ShowWaitSplash(Formulario, Parametros.WaitFormCaption.GENERANDO.ToString(), Descripcion);
                //Stopwatch sw = new Stopwatch();
                //sw.Start();

                Formulario.Cursor = Cursors.WaitCursor;
                decimal ThisDescuentoPetroCard = RD.DescuentoPetroCard;
                
                //llamado de los arqueos de isla
                List<Entidad.VistaArqueoIsla> AIView = (from v in dbView.VistaArqueoIslas
                             where (v.ArqueoIslaID.Equals(ArqueoIslaID) || ArqueoIslaID.Equals(0)) && ((!v.ArqueoEspecial || (v.ArqueoEspecial && v.Oficial)) || TA.Equals(Parametros.TiposArqueo.Isla)) && (v.TurnoID.Equals(TurnoID) || TurnoID.Equals(0))
                             && v.ResumenDiaID.Equals(RD.ID)
                             select v).ToList();
              
                //llamados de las extracciones
                List<Entidad.spListValorExtraxionResult> vistaExtracciones = dbView.spListValorExtraxion(RD.EstacionServicioID, RD.ID, ArqueoIslaID, TurnoID).ToList();

                //llamado del Detalle Manejo
                List<Entidad.GetDetalleManejoIsla> EtManejo = dbView.GetDetalleManejoIslas.Where(o => o.ArqueoIslaID.Equals(ArqueoIslaID)).ToList();

                //llamados de ADMINISTRADOR
                var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));

                //llamados de CONTADOR
                var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
                               

                if (AIView.Count() > 0)
                {
                    //Datos Iniciales del Reporte
                    Reportes.Arqueos.Hojas.RptArqueoTest rep = new Reportes.Arqueos.Hojas.RptArqueoTest();
                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    decimal _ValorTotalVenta = 0m;
                    decimal _DifValorTotalVenta = 0m;
                    decimal _GrandTotalDesucento = 0m;
                    decimal _VentasContado = 0m;
                    decimal _TotalEfectivoRecibido = 0m;
                    decimal _TotalSobranteFaltante = 0m;
                    decimal _TotalDiferenciaRecibida = 0m;
                    decimal _GRANDTOTAL = 0m;
                    bool _Contador = true;
                    bool _Admin = true;

                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (String.IsNullOrEmpty(AIView.First().SubEstacionNombre) ? AIView.First().EstacionNombre : AIView.First().SubEstacionNombre);
                    rep.lblTipoImpresion.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
                    rep.Watermark.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
                    rep.CeFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                    rep.CellPrintDate.Text = db.GetDateServer().ToString();

                    //Parametros para Reporte Arqueo Isla
                    #region <<< ParametrosReporte >>>

                        if (TA.Equals(Parametros.TiposArqueo.Isla) || TA.Equals(Parametros.TiposArqueo.Turno))
                        {
                            rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;

                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                            {

                                if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                                {
                                    rep.Watermark.Text += " Nro. " + AIView.First().ReImpresionCount.ToString();
                                    rep.lblTipoImpresion.Text += " Nro. " + AIView.First().ReImpresionCount.ToString();
                                }
                                if (AIView.Count(ai => ai.ArqueoEspecial) > 0)
                                    rep.ceEspecial.Text = (AIView.Count(a => a.Oficial) > 0 ? "Arqueo Especial / Oficial" : "Arqueo Especial");

                                rep.CeTecnico.Text = AIView.First().TecnicoNombre;
                                rep.CeTurno.Text = AIView.First().TurnoNumero.ToString();
                                rep.CeIsla.Text = AIView.First().IslaNombre;
                                rep.TablaFirmas.Rows[0].Visible = false;
                                rep.TablaFirmas.Rows[1].Visible = false;
                                rep.TablaFirmas.Rows[2].Visible = false;
                                rep.CeTitulo.Text = "ARQUEO DE ISLA";
                                rep.CellSegundaFirmaTitulo.Text = "Firma Técnico de Pista";
                                rep.CellSegundaFirmaNombre.Text = AIView.First().TecnicoNombre;
                                rep.CeNumeroArqueo.Text = AIView.First().ArqueoNumero.ToString();
                                rep.xrTableFecha.Visible = false;
                            }
                            else
                            {
                                if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                                {
                                    int contador = 0;
                                    contador = db.Turnos.Single(t => t.ID.Equals(TurnoID)).ReImpresionCount;
                                    rep.Watermark.Text += " Nro. " + contador.ToString();
                                    rep.lblTipoImpresion.Text += " Nro. " + contador.ToString();
                                }
                                rep.xrCellFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                                rep.xrTableHeader.Visible = false;
                                rep.xrTableCell1.Visible = false;
                                rep.xrTableCell8.Visible = false;
                                rep.CeTecnico.Visible = false;
                                rep.CeIsla.Visible = false;
                                rep.xrTableCell2.Visible = false;
                                rep.xrTableCell5.Visible = false;
                                rep.CeTurno.Visible = false;
                                rep.CeNumeroArqueo.Visible = false;
                                rep.CeTitulo.Text = "RESUMÉN DE TURNO # " + AIView.First().TurnoNumero.ToString();
                                rep.CellPrimerFirmaTitulo.Text = "Administrador";
                                //var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
                                if (empleadoadmin.Count() <= 0)
                                    _Admin = false;
                                else
                                    rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;

                                rep.CellSegundaFirmaTitulo.Text = "Contador";
                                //var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
                                if (empleadoCont.Count() <= 0)
                                    _Contador = false;
                                else
                                    rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;
                            }
                        }
                        else
                        {
                            if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                            {
                                rep.Watermark.Text += " Nro. " + RD.ReImpresionCount.ToString();
                                rep.lblTipoImpresion.Text += " Nro. " + RD.ReImpresionCount.ToString();
                            }
                            rep.xrTableHeader.Visible = false;
                            rep.xrCellFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                            //rv.Text = "RESUMEN DE ARQUEO POR DIA";
                            rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;
                            rep.CeTitulo.Text = "RESUMÉN DE ARQUEO POR DÍA # " + AIView.First().ResumenDiaNumero.ToString();
                            rep.CellPrimerFirmaTitulo.Text = "Administrador";
                            //var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
                            if (empleadoadmin.Count() <= 0)
                                _Admin = false;
                            else
                                rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;


                            rep.CellSegundaFirmaTitulo.Text = "Contador";
                            //var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
                            if (empleadoCont.Count() <= 0)
                                _Contador = false;
                            else
                                rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;

                        }



                        #endregion

                        List<Entidad.ExtracionPago> ExtraxionPago = db.ExtracionPagos.Where(ep => ep.Activo).OrderBy(o => o.Orden).ToList();

                                                                   //dbView.VistaArqueoMangueras.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID))).ToList();
                        List<Entidad.VistaArqueoManguera> AMView = dbView.VistaArqueoMangueras.Where(v => AIView.Select(s => s.ArqueoIslaID).Contains(v.ArqueoIslaID)).ToList();//  AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID))).ToList();
                        List<Entidad.VistaArqueoFormaPago> AFPView = dbView.VistaArqueoFormaPagos.Where(v => AIView.Select(s => s.ArqueoIslaID).Contains(v.ArqueoIslaID)).ToList();


                    //    //Tabla de Lecturas
                        if (AMView.Count() > 0)
                        {
                            #region <<< TituloTablasLecturas >>>

                            DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                            xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                            float y = 55f;
                            y = (TA.Equals(Parametros.TiposArqueo.Turno) ? 10f :
                                TA.Equals(Parametros.TiposArqueo.ResumenDia) ? 10f : 55f);


                            xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(0F, y);
                            int WF = 0;

                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                                WF = Convert.ToInt32(AMView.Count(ap => ap.ArqueoMangueraID > 0));
                            else
                                WF = Convert.ToInt32(AMView.GroupBy(g => new { g.ProductoID, g.Lado }).Distinct().Count());

                            xrTableAP.WidthF = (100f + (WF * 90f));

                            DevExpress.XtraReports.UI.XRTableRow xrTableRowAPLado = new DevExpress.XtraReports.UI.XRTableRow();
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowAPDisp = new DevExpress.XtraReports.UI.XRTableRow();
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowAPProd = new DevExpress.XtraReports.UI.XRTableRow();
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDis = new DevExpress.XtraReports.UI.XRTableRow();
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotales = new DevExpress.XtraReports.UI.XRTableRow();

                            //Filas Tipo de Productos
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotProd = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowTotProd.Weight = 1D;
                            xrTableRowTotProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                            //Fila Cantidad Totales
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotCant = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowTotCant.Weight = 1D;
                            xrTableRowTotCant.Cells.Add(new Parametros.MyXRTableCell("Extracción Total", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                            //Fila Lados
                            xrTableRowAPLado.Weight = 1D;
                            xrTableRowAPLado.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            var celda = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
                            celda.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                            celda.StylePriority.UseBorders = false;
                            xrTableRowAPLado.Cells.Add(celda);
                            int llenado = 1;

                            //Fila Dispensadores
                            xrTableRowAPDisp.Weight = 1D;
                            xrTableRowAPDisp.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            var celdaDisp = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
                            celdaDisp.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                            celdaDisp.StylePriority.UseBorders = false;
                            xrTableRowAPDisp.Cells.Add(celdaDisp);

                            //Fila Productos
                            xrTableRowAPProd.Weight = 1D;
                            xrTableRowAPProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            var celdaProd = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.TopCenter, Color.Transparent, Color.Black);
                            celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                            celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                            celdaProd.StylePriority.UseBorders = false;
                            xrTableRowAPProd.Cells.Add(celdaProd);

                            #endregion

                            #region <<< LadosMangueras >>>

                            foreach (var lado in AMView.GroupBy(l => l.Lado).Distinct())
                            {
                                float ancho = lado.Count() * 90;
                                xrTableRowAPLado.Cells.Add(new Parametros.MyXRTableCell("LADO " + lado.Key + " Punto de Llenado " + llenado.ToString(), ancho, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                                llenado++;

                                if (TA.Equals(Parametros.TiposArqueo.Isla))
                                {
                                    foreach (var disp in AMView.Where(d => d.Lado.Equals(lado.Key)).GroupBy(g => new { g.DispensadorNombre, g.Lado }).Distinct())
                                    {
                                        float anchoDisp = disp.Count() * 90;
                                        xrTableRowAPDisp.Cells.Add(new Parametros.MyXRTableCell("Dispensador: " + disp.Key.DispensadorNombre, anchoDisp, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                                        AMView.Where(p => p.DispensadorNombre.Equals(disp.Key.DispensadorNombre) && p.Lado.Equals(disp.Key.Lado)).ToList().ForEach(prod =>
                                        {
                                            xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));

                                        });
                                    }
                                }
                                else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                                {
                                    AMView.Where(l => l.Lado.Equals(lado.Key)).GroupBy(ap => new { ap.ProductoID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
                                    {
                                        xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));
                                    });
                                }
                            }

                            #endregion

                            xrTableAP.Rows.Add(xrTableRowAPLado);
                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                            {
                                xrTableAP.Rows.Add(xrTableRowAPDisp);
                            }

                            xrTableAP.Rows.Add(xrTableRowAPProd);
                            rep.GroupHeaderArqueo.Controls.Add(xrTableAP);
                            y += xrTableAP.SizeF.Height;


                            #region <<< LecturasMecanicas >>>

                            if (AMView.Count(l => l.EsLecturaMecanica) > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA MECÁNICA", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightBlue, Color.Black);
                                LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                                y += 18f;

                                DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                                xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaMecanicaInicial", "LecturaMecanicaFinal", "ExtraxionMecanica", false, true, true);
                                xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);

                                if (TA.Equals(Parametros.TiposArqueo.Isla))
                                    xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                                else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                                {
                                    var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                                   group am by new { am.Lado, am.ProductoID } into gr
                                                   select gr;

                                    //select new
                                    //{
                                    //    LecturaMecanicaInicial = gr.Sum(s => s.LecturaMecanicaInicial),
                                    //    LecturaMecanicaFinal = gr.Sum(s => s.LecturaMecanicaFinal),
                                    //    ExtraxionMecanica = gr.Sum(s => s.ExtraxionMecanica)
                                    //};
                                    List<ExtracionesMecanica> Mec = new List<ExtracionesMecanica>();
                                    agrupado.ToList().ForEach(det =>
                                    {
                                        Mec.Add(new ExtracionesMecanica { Orden = det.First().MangueraNumero, LecturaMecanicaInicial = det.Sum(s => s.LecturaMecanicaInicial), LecturaMecanicaFinal = det.Sum(s => s.LecturaMecanicaFinal), ExtraxionMecanica = det.Sum(s => Convert.ToDecimal(s.ExtraxionMecanica)) });
                                    });

                                    xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);
                                    //.Where(a => a.LecturaMecanicaInicial > 0 && a.LecturaMecanicaFinal > 0 && a.ExtraxionMecanica > 0).OrderBy(o => o.Orden);
                                }

                                rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                                DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                                y += 18f;


                            }

                            #endregion

                            #region <<< LecturaEfectivo >>>

                            if (AMView.Count(l => l.EsLecturaEfectivo) > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA EFECTIVO", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightGray, Color.Black);
                                LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                                y += 18f;

                                //Lecturas Mecanicas
                                DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                                xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaEfectivoInicial", "LecturaEfectivoFinal", "ExtraxionEfectivo", false, true, true);
                                xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);

                                if (TA.Equals(Parametros.TiposArqueo.Isla))
                                    xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                                else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                                {
                                    //var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                    //               group am by new { am.ProductoID, am.Lado } into gr
                                    //               select new
                                    //               {
                                    //                   LecturaEfectivoInicial = gr.Sum(s => s.LecturaEfectivoInicial),
                                    //                   LecturaEfectivoFinal = gr.Sum(s => s.LecturaEfectivoFinal),
                                    //                   ExtraxionEfectivo = gr.Sum(s => s.ExtraxionEfectivo)
                                    //               };

                                    //xrSubProd.ReportSource.DataSource = agrupado;

                                    var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                                   group am by new { am.Lado, am.ProductoID } into gr
                                                   select gr;

                                    List<ExtracionesEfectiva> Mec = new List<ExtracionesEfectiva>();
                                    agrupado.ToList().ForEach(det =>
                                    {
                                        Mec.Add(new ExtracionesEfectiva { Orden = det.First().MangueraNumero, LecturaEfectivoInicial = det.Sum(s => s.LecturaEfectivoInicial), LecturaEfectivoFinal = det.Sum(s => s.LecturaEfectivoFinal), ExtraxionEfectivo = det.Sum(s => Convert.ToDecimal(s.ExtraxionEfectivo)) });
                                    });

                                    xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);

                                }

                                rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                                DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                                y += 18f;

                            }

                            #endregion

                            #region <<< LecturaElectronica >>>

                            if (AMView.Count(l => l.EsLecturaElectronica) > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA ELECTRÓNICA", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightSalmon, Color.Black);
                                LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                                y += 18f;

                                //Lecturas Mecanicas
                                DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                                xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaElectronicaInicial", "LecturaElectronicaFinal", "ExtraxionElectronica", false, true, true);
                                xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);

                                if (TA.Equals(Parametros.TiposArqueo.Isla))
                                    xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                                else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                                {
                                    //var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                    //               group am by new { am.ProductoID, am.Lado } into gr
                                    //               select new
                                    //               {
                                    //                   LecturaElectronicaInicial = gr.Sum(s => s.LecturaElectronicaInicial),
                                    //                   LecturaElectronicaFinal = gr.Sum(s => s.LecturaElectronicaFinal),
                                    //                   ExtraxionElectronica = gr.Sum(s => s.ExtraxionElectronica)
                                    //               };

                                    //xrSubProd.ReportSource.DataSource = agrupado;

                                    var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                                   group am by new { am.Lado, am.ProductoID } into gr
                                                   select gr;

                                    List<ExtracionesElectronica> Mec = new List<ExtracionesElectronica>();
                                    agrupado.ToList().ForEach(det =>
                                    {
                                        Mec.Add(new ExtracionesElectronica { Orden = det.First().MangueraNumero, LecturaElectronicaInicial = det.Sum(s => s.LecturaElectronicaInicial), LecturaElectronicaFinal = det.Sum(s => s.LecturaElectronicaFinal), ExtraxionElectronica = det.Sum(s => Convert.ToDecimal(s.ExtraxionElectronica)) });
                                    });

                                    xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);

                                }

                                rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                                DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                                y += 18f;

                            }

                            #endregion

                            #region <<< Verificacion >>>

                            if (AMView.Count(l => l.EsLecturaEfectivo) > 0 || AMView.Count(l => l.EsLecturaElectronica) > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel LabelVerificacion = new Parametros.MyXRLabel("VERIFICACIÓN", xrTableAP.WidthF, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGreen, Color.Black);
                                LabelVerificacion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelVerificacion);
                                y += 18f;

                                //Verificación
                                DevExpress.XtraReports.UI.XRSubreport xrSubVerificacion = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubVerificacion.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                                xrSubVerificacion.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("VerificacionEfectivo", "VerificacionMecElectronica", "", true,
                                    (AMView.Count(c => c.EsLecturaEfectivo) > 0 ? true : false),
                                    (AMView.Count(c => c.EsLecturaElectronica) > 0 ? true : false)
                                );
                                xrSubVerificacion.SizeF = new System.Drawing.SizeF(725F, 54F);

                                if (TA.Equals(Parametros.TiposArqueo.Isla))
                                    xrSubVerificacion.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                                else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                                {
                                    var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                                   group am by new { am.ProductoID, am.Lado } into gr
                                                   select new
                                                   {
                                                       VerificacionEfectivo = gr.Sum(s => (s.VerificacionEfectivo.HasValue ? (s.ExtraxionElectronica > 0 ? s.VerificacionEfectivo : 0m) : 0m)),  // s.VerificacionEfectivo),
                                                       VerificacionMecElectronica = gr.Sum(s => (s.VerificacionMecElectronica.HasValue ? (s.ExtraxionElectronica > 0 ? s.VerificacionMecElectronica : 0m) : 0m)) //s.VerificacionMecElectronica)
                                                   };

                                    xrSubVerificacion.ReportSource.DataSource = agrupado;
                                }

                                rep.GroupHeaderArqueo.Controls.Add(xrSubVerificacion);

                                DevExpress.XtraReports.UI.XRLabel LabelEfectivo = new Parametros.MyXRLabel("Efectivo", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelEfectivo.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelEfectivo);
                                y += 18f;
                                DevExpress.XtraReports.UI.XRLabel LabelMecElectronica = new Parametros.MyXRLabel("Mec. Vs Electrónica", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                                LabelMecElectronica.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                                rep.GroupHeaderArqueo.Controls.Add(LabelMecElectronica);
                            }

                            #endregion

                            float sizeWidht = (100f + (AMView.GroupBy(ap => new { ap.ProductoID }).Distinct().Count() * 90));
                            float sizeWidhtSec = (100f + (AMView.GroupBy(ap => new { ap.ProductoID }).Distinct().Count() * 80));
                            float yf = 0f;

                            #region <<< TITULOSDESCUENTO >>>

                            //Label Descuento
                            DevExpress.XtraReports.UI.XRLabel LabelTituloDescuento = new Parametros.MyXRLabel("DESCUENTO", sizeWidht, 17f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black);
                            LabelTituloDescuento.LocationFloat = new DevExpress.Utils.PointFloat(0f, yf);
                            //rep.Detail.Controls.Add(LabelTituloDescuento);
                            //yf += 17f;

                            DevExpress.XtraReports.UI.XRLabel LabelTituloProductos = new Parametros.MyXRLabel("PRODUCTOS", sizeWidht - 100f, 17f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black);
                            LabelTituloProductos.LocationFloat = new DevExpress.Utils.PointFloat(0f + 100f, yf);
                            //rep.Detail.Controls.Add(LabelTituloProductos);

                            //Titulo Producto Descuento
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProdTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowDescuentoProdTitulo.Weight = 1D;
                            xrTableRowDescuentoProdTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                            //xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell("PRODUCTOS", xrTableAPPago.WidthF - 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                            //Productos Descuento
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProd = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowDescuentoProd.Weight = 1D;
                            xrTableRowDescuentoProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowDescuentoProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                            //Linea Descuentos Totales
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDesc = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowTotalDesc.Weight = 1D;
                            xrTableRowTotalDesc.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowTotalDesc.Cells.Add(new Parametros.MyXRTableCell("Descuentos Totales", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                            //Liena Total Descuento
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDescuentos = new DevExpress.XtraReports.UI.XRTableRow();
                            xrTableRowTotalDescuentos.Weight = 1D;
                            xrTableRowTotalDescuentos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowTotalDescuentos.Cells.Add(new Parametros.MyXRTableCell("Total Descuento", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                            #endregion

                            #region <<< TOTALESEXTRAXION >>>

                            //Parametros SubReport
                            float WidthFull = 0f;
                            float WidthDiferenciado = 0f;
                            bool ShowFull = false;
                            bool ShowDiferenciado = false;

                            if (AMView.Count(a => !a.EsDiferenciado) > 0)
                            {
                                WidthFull = AMView.GroupBy(w => new { w.EsDiferenciado, w.ProductoID, w.TanqueID }).Count(c => !c.Key.EsDiferenciado) * 90;
                                ShowFull = true;
                            }

                            if (AMView.Count(a => a.EsDiferenciado) > 0)
                            {
                                WidthDiferenciado = AMView.GroupBy(w => new { w.EsDiferenciado, w.ProductoID, w.TanqueID }).Count(c => c.Key.EsDiferenciado) * 90;
                                ShowDiferenciado = true;
                            }

                            //EXTRAXIONES VENTAS
                            var ListaExtraxionVenta = from APTot in AMView.GroupBy(ap => new { ap.EsDiferenciado, ap.ProductoID, ap.ProductoNombre, ap.TanqueID, ap.TanqueNombre }).Distinct().OrderBy(o => o.Key.EsDiferenciado).ThenByDescending(o => o.Key.ProductoNombre).ThenByDescending(o => o.Key.TanqueNombre).ToList()
                                                      select new
                                                      {
                                                          ExtraVenta = Decimal.Round((APTot.GroupBy(g => new { g.ArqueoProductoID, g.VentaLitro }).Sum(s => s.Key.VentaLitro)), 3, MidpointRounding.AwayFromZero),
                                                          //GroupBy(g => g.VentaLitro).FirstOrDefault() != null ? APTot.GroupBy(g => g.VentaLitro).FirstOrDefault().Key : 0m)
                                                          //((APTot.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica))) -
                                                          //(vistaExtracciones.Count(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)) > 0 ? Convert.ToDecimal(vistaExtracciones.Where(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)).Sum(s => s.Valor)) : 0m)), 3, MidpointRounding.AwayFromZero),
                                                          PrecioVenta = APTot.Average(o => o.Precio),
                                                          Descuento = Convert.ToDecimal(APTot.Max(v => v.MangueraDescuento)),
                                                          ValorVenta = Decimal.Round((APTot.GroupBy(g => new { g.ArqueoProductoID, g.VentaValor }).Sum(s => s.Key.VentaValor)), 3, MidpointRounding.AwayFromZero),
                                                          //Decimal.Round((((APTot.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica))) -
                                                          //(vistaExtracciones.Count(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)) > 0 ? Convert.ToDecimal(vistaExtracciones.Where(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)).Sum(s => s.Valor)) : 0m)) * APTot.Average(o => o.Precio)), 2, MidpointRounding.AwayFromZero)
                                                      };

                            List<ListDist> ListExtraDist = new List<ListDist>();

                            vistaExtracciones.Where(v => !v.Valor.Equals(0)).OrderBy(o => o.EsDiferenciado).ThenBy(t => t.ProductoID).ToList().ForEach(vExtra =>
                            {
                                AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID }).OrderBy(o => o.Key.EsDiferenciado).Select(s => s.FirstOrDefault()).Distinct().ToList().ForEach(prod =>
                                {
                                    ListDist ld = new ListDist();
                                    ld.ID = Convert.ToInt32(vExtra.ID);
                                    ld.ExtraxionNombre = vExtra.ExtraxionNombre;
                                    ld.ProductoID = prod.ProductoID;
                                    ld.TanqueID = prod.TanqueID;
                                    ld.TanqueNombre = prod.TanqueNombre;
                                    ld.ProductoNombre = (prod.EsDiferenciado ? "0" : "1") + prod.ProductoNombre;
                                    if (prod.ProductoID.Equals(vExtra.ProductoID) && prod.TanqueID.Equals(vExtra.TanqueID) && prod.EsDiferenciado.Equals(vExtra.EsDiferenciado))
                                        ld.Valor = vExtra.Valor;

                                    ListExtraDist.Add(ld);
                                });

                            });

                            var listDistFinal = from l in ListExtraDist
                                                select new
                                                {
                                                    ID = l.ID,
                                                    ExtraxionNombre = l.ExtraxionNombre,
                                                    ProductoID = l.ProductoID,
                                                    ProductoNombre = l.ProductoNombre + " => " + l.TanqueNombre,
                                                    TanqueID = l.TanqueID,
                                                    TanqueNombre = l.TanqueNombre,
                                                    Valor = l.Valor
                                                };

                            var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (TA.Equals(Parametros.TiposArqueo.Isla) ? (s.Sum(x => x.ExtraxionElectronica) > 0 ? s.Sum(x => x.ExtraxionElectronica) : (s.Sum(x => x.ExtraxionElectronica.HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica /*(!x.MecanicaAmbasCara ? x.ExtraxionMecanica : 0m)*/) : x.ExtraxionMecanica))) : (s.Sum(x => x.ExtraxionElectronica.HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica) : x.ExtraxionMecanica))), s.Key.EsDiferenciado });
                            //var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (s.Sum(x => s.Sum(z => z.ExtraxionElectronica).HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica /*(!x.MecanicaAmbasCara ? x.ExtraxionMecanica : 0m)*/) : x.ExtraxionMecanica)), s.Key.EsDiferenciado });
                            //var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (s.Count(c => c.EsLecturaElectronica) > 0 ? s.Sum(x => x.ExtraxionElectronica) : s.Sum(x => x.ExtraxionMecanica)), s.Key.EsDiferenciado });

                            rep.xrSubreportExtraxiones.ReportSource = new Reportes.Arqueos.Hojas.SubRptPivotProduct((100f + (AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID }).Distinct().Count() * 90)), ShowFull, WidthFull, ShowDiferenciado, WidthDiferenciado, TotalExtraxion, listDistFinal, ListaExtraxionVenta);
                            _DifValorTotalVenta = (ListaExtraxionVenta.Count() > 0 ? ListaExtraxionVenta.Sum(s => s.ValorVenta) : 0m);
                            #endregion

                            #region <<< RESUMEN >>>

                            var ListaFP = (from list in AFPView.Where(v => v.MonedaID.Equals(0))
                                           group list by new { list.PagoID, list.PagoNombre, list.Orden } into gr
                                           select new
                                           {
                                               Nombre = gr.Key.PagoNombre,
                                               Orden = gr.Key.Orden,
                                               Valor = gr.Where(w => w.PagoID.Equals(gr.Key.PagoID)).Sum(s => s.Valor)
                                           }).OrderBy(o => o.Orden);


                            ///DESCUENTOS...........................

                            List<ListDescuento> desc = new List<ListDescuento>();

                            foreach (Parametros.TiposDescuento val in Enum.GetValues(typeof(Parametros.TiposDescuento)).AsParallel())
                            {

                                AMView.GroupBy(ap => new { ap.ProductoID, ap.TanqueID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
                                {
                                    var vista = AMView.Where(v => v.ProductoID.Equals(prod.ProductoID) && v.TanqueID.Equals(prod.TanqueID));

                                    if (vista.Count() > 0)
                                    {

                                        ListDescuento ld = new ListDescuento();
                                        ld.TipoDescuentoID = Convert.ToInt32(val);
                                        ld.TipoDescuentoNombre = Parametros.General.GetEnumTiposDescuento(val);
                                        ld.ProductoID = prod.ProductoID;
                                        ld.ProductoNombre = prod.ProductoNombre;

                                        if (val.Equals(Parametros.TiposDescuento.Isla))
                                        {
                                            decimal Desc = 0m;
                                            decimal NonDesc = 0m;

                                            List<string> dispensadores = new List<string>();

                                            vista.ToList().ForEach(objMang =>
                                            {
                                                if (!objMang.MangueraDescuento.Equals(0))
                                                {
                                                    decimal extraxion = Convert.ToDecimal(objMang.EsLecturaElectronica ? objMang.ExtraxionElectronica : objMang.ExtraxionMecanica);
                                                    Desc += Decimal.Round(Convert.ToDecimal(extraxion * objMang.MangueraDescuento), 2, MidpointRounding.AwayFromZero);
                                                    NonDesc = objMang.MangueraDescuento;

                                                    if (dispensadores.Contains(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString()))
                                                    {
                                                        var vistaAPE = vistaExtracciones.Where(v => v.ProductoID.Equals(prod.ProductoID) && v.TanqueID.Equals(prod.TanqueID) && v.ArqueoProductoID.Equals(objMang.ArqueoProductoID));

                                                        if (vistaAPE.Count() > 0)
                                                            Desc -= Decimal.Round(Convert.ToDecimal(vistaAPE.Sum(s => s.Valor)) * NonDesc, 2, MidpointRounding.AwayFromZero);
                                                    }
                                                    dispensadores.Add(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString());
                                                }
                                            });

                                            ld.Valor = Desc;
                                        }
                                        else if (val.Equals(Parametros.TiposDescuento.Especial))
                                        {
                                            decimal ValorDE = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID, g.TanqueID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoEspecial)), 2, MidpointRounding.AwayFromZero);
                                            ld.Valor = ValorDE;
                                        }
                                        else if (val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
                                        {
                                            decimal ValorDGPCF = 0m;

                                            vista.GroupBy(ap => new { ap.ProductoID, ap.ArqueoIslaID, ap.TanqueID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(objProd =>
                                            {
                                                ValorDGPCF += objProd.DescuentoGalonesPetroCardFormula != null ? Decimal.Round(Convert.ToDecimal(Parametros.General.ValorFormula(objProd.DescuentoGalonesPetroCardFormula.ToString())), 2, MidpointRounding.AwayFromZero) : 0m;
                                            });

                                            ld.Valor = ValorDGPCF;
                                        }
                                        else if (val.Equals(Parametros.TiposDescuento.CordobasPetroCard))
                                        {
                                            decimal ValorDGDCV = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID, g.TanqueID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoGalonesPetroCardValor)), 2, MidpointRounding.AwayFromZero);

                                            ld.Valor = ValorDGDCV;
                                        }

                                        if (!val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
                                            _GrandTotalDesucento += ld.Valor; ;

                                        desc.Add(ld);
                                    }
                                });



                            }
                            ///DESCUENTOS.......

                            var listDescuento = from l in desc
                                                select new
                                                {
                                                    TipoDescuentoID = l.TipoDescuentoID,
                                                    TipoDescuentoNombre = l.TipoDescuentoNombre,
                                                    ProductoNombre = l.ProductoNombre,
                                                    Valor = l.Valor
                                                };

                            rep.xrSubreportResumen.LocationF = new DevExpress.Utils.PointFloat((100f + (AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID }).Distinct().Count() * 90)) + 10f, 0f);
                            _ValorTotalVenta = AIView.Sum(s => s.ValorTotalVenta);
                            _VentasContado = AIView.Sum(s => s.VentasContado);
                            _TotalSobranteFaltante = AIView.Sum(s => s.SobranteFaltante);
                            _TotalDiferenciaRecibida = AIView.Sum(s => s.DiferenciaRecibida);


                            if (AFPView.Count(v => !v.MonedaID.Equals(0)) > 0)
                                _TotalEfectivoRecibido = Decimal.Round(AFPView.Where(v => !v.MonedaID.Equals(0)).Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);

                            rep.xrSubreportResumen.ReportSource = new Reportes.Arqueos.Hojas.SubRptPagoArqueos(sizeWidhtSec, _ValorTotalVenta, _DifValorTotalVenta - _ValorTotalVenta, ListaFP, listDescuento, _GrandTotalDesucento, _VentasContado, _TotalEfectivoRecibido, _TotalSobranteFaltante, _TotalDiferenciaRecibida);

                            #endregion

                            #region <<< TANQUES >>>
                            if (TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                //Tabla Tanques
                                DevExpress.XtraReports.UI.XRTable xrTableTanques = new DevExpress.XtraReports.UI.XRTable();
                                xrTableTanques.Font = new System.Drawing.Font("Tahoma", 8F);
                                xrTableTanques.LocationFloat = new DevExpress.Utils.PointFloat(0f, 5f);

                                //Filas
                                DevExpress.XtraReports.UI.XRTableRow xrTableRowTanTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                                DevExpress.XtraReports.UI.XRTableRow xrTableRowTanExtraxion = new DevExpress.XtraReports.UI.XRTableRow();

                                //Fila Titulo
                                xrTableRowTanTitulo.Weight = 1D;
                                xrTableRowTanTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                                xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell("TANQUES", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));


                                //Fila Extraxion 
                                xrTableRowTanExtraxion.Weight = 1D;
                                xrTableRowTanExtraxion.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                                xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell("EXTRACCIÓN", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                                var tanques = from t in AMView
                                              group t by new { t.TanqueID, t.TanqueNombre, t.Color, t.ProductoNombre } into g
                                              select new
                                              {
                                                  Nombre = g.Key.TanqueNombre + " (" + g.Key.ProductoNombre + ")",
                                                  Color = g.Key.Color,
                                                  Total = (g.Count(s => s.EsLecturaElectronica) > 0 ? g.Sum(a => a.ExtraxionElectronica) : g.Sum(a => a.ExtraxionMecanica))
                                              };

                                xrTableTanques.SizeF = new System.Drawing.SizeF((100f + (tanques.Count() * 90)), 32F);

                                tanques.OrderBy(o => o.Nombre).ToList().ForEach(lista =>
                                {
                                    xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell(lista.Nombre, 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(lista.Color), Color.White));
                                    xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(lista.Total).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                                });

                                xrTableTanques.Rows.Add(xrTableRowTanTitulo);
                                xrTableTanques.Rows.Add(xrTableRowTanExtraxion);

                                rep.GroupFooterArqueo.Controls.Add(xrTableTanques);
                            }
                            #endregion

                            #region <<< OBSERVACIONES >>>

                            if (AIView.Count(ai => ai.Observacion.Length > 0) > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel xrLabelO = new DevExpress.XtraReports.UI.XRLabel();
                                xrLabelO.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
                                xrLabelO.LocationFloat = new DevExpress.Utils.PointFloat(0F, 43f);
                                xrLabelO.Name = "xrLabelO";
                                xrLabelO.Multiline = true;
                                xrLabelO.SizeF = new System.Drawing.SizeF(820F, 15F);
                                xrLabelO.Text = "  Observaciones : ";

                                AIView.Where(ai => ai.Observacion.Length > 0).ToList().ForEach(nota =>
                                { xrLabelO.Text += nota.Observacion.ToString() + "  |  "; });
                                rep.GroupFooterArqueo.Controls.Add(xrLabelO);
                            }
                            #endregion

                            #region Detalle Manejo

                            if (EtManejo.Count > 0)
                            {
                                DevExpress.XtraReports.UI.XRLabel xrLabeldm = new DevExpress.XtraReports.UI.XRLabel();
                                xrLabeldm.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
                                xrLabeldm.LocationFloat = new DevExpress.Utils.PointFloat(0F, 60f);
                                xrLabeldm.Name = "xrLabeldm";
                                xrLabeldm.Padding = new DevExpress.XtraPrinting.PaddingInfo(3);
                                xrLabeldm.Multiline = true;
                                xrLabeldm.SizeF = new System.Drawing.SizeF(820F, 15F);
                                xrLabeldm.Text = "  Detalle autoconsumo : ";

                                EtManejo.ForEach(dm =>
                                { xrLabeldm.Text += dm.Combustible + " " + dm.Concepto; });
                                rep.GroupFooterArqueo.Controls.Add(xrLabeldm);
                            }

                            #endregion
                            

                        }

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    if (!_Admin)
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOADMNISTRADOR + Environment.NewLine, Parametros.MsgType.warning);

                    if (!_Contador)
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOCONTADOR + Environment.NewLine, Parametros.MsgType.warning);

                    if (ToPrint)
                    {
                        rep.CreateDocument();
                        rep.Print();

                    }
                    else
                    {
                        rep.CreateDocument(true);
                        printArea.PrintingSystem = rep.PrintingSystem;
                    }

                    //sw.Stop();
                    //MessageBox.Show(sw.Elapsed.ToString());
                    Formulario.Cursor = Cursors.Default;
                    rep.RequestParameters = true;

                }
                else
                {
                    //vaild = false;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Formulario.Cursor = Cursors.Default;
                    Parametros.General.DialogMsg("No existe ningún Arqueo de Isla creado.", Parametros.MsgType.warning);
                }

            }
            catch (Exception ex)
            {
                //vaild = false;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, Formulario.Name);
            }


        }


        public static void PrintArqueox(Form Formulario, Entidad.SAGASDataViewsDataContext dbView,
            Entidad.SAGASDataClassesDataContext db, int ArqueoIslaID, int TurnoID, Entidad.ResumenDia RD,
            Parametros.TiposImpresion TI, bool ToPrint, Parametros.TiposArqueo TA, string Descripcion, DevExpress.XtraPrinting.Control.PrintControl printArea)
        {
            try
            {
                Parametros.General.ShowWaitSplash(Formulario, Parametros.WaitFormCaption.GENERANDO.ToString(), Descripcion);
                
                Formulario.Cursor = Cursors.WaitCursor;
                decimal ThisDescuentoPetroCard = RD.DescuentoPetroCard;

                //List<Entidad.VistaArqueoIsla> AIView = new List<Entidad.VistaArqueoIsla>();

                //if (TA.Equals(Parametros.TiposArqueo.Isla))
                //{
                //    AIView = (from v in dbView.VistaArqueoIslas
                //              where (v.ArqueoIslaID.Equals(ArqueoIslaID) || ArqueoIslaID.Equals(0)) && (v.TurnoID.Equals(TurnoID) || TurnoID.Equals(0))
                //              && v.ResumenDiaID.Equals(RD.ID)
                //              select v).ToList();
                //}
                //else
                //{
                   var AIView = from v in dbView.VistaArqueoIslas
                              where (v.ArqueoIslaID.Equals(ArqueoIslaID) || ArqueoIslaID.Equals(0)) && ((!v.ArqueoEspecial || (v.ArqueoEspecial && v.Oficial)) || TA.Equals(Parametros.TiposArqueo.Isla)) && (v.TurnoID.Equals(TurnoID) || TurnoID.Equals(0))
                              && v.ResumenDiaID.Equals(RD.ID)
                              select v;
              //}

                var vistaExtracciones = dbView.spListValorExtraxion(RD.EstacionServicioID, RD.ID, ArqueoIslaID, TurnoID).ToList();

                if (AIView.Count() > 0)
                {
                    //Datos Iniciales del Reporte
                    Reportes.Arqueos.Hojas.RptArqueoTest rep = new Reportes.Arqueos.Hojas.RptArqueoTest();
                    string Nombre, Direccion, Telefono;
                    System.Drawing.Image picture_LogoEmpresa;
                    decimal _ValorTotalVenta = 0m;
                     decimal _DifValorTotalVenta = 0m;
                    decimal _GrandTotalDesucento = 0m;
                    decimal _VentasContado = 0m;
                    decimal _TotalEfectivoRecibido = 0m;
                    decimal _TotalSobranteFaltante = 0m;
                    decimal _TotalDiferenciaRecibida = 0m;
                    decimal _GRANDTOTAL = 0m;
                    bool _Contador = true;
                    bool _Admin = true;

                    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    rep.PicLogo.Image = picture_LogoEmpresa;
                    rep.CeEmpresa.Text = Nombre;
                    rep.CeEstacion.Text = (String.IsNullOrEmpty(AIView.First().SubEstacionNombre) ? AIView.First().EstacionNombre : AIView.First().SubEstacionNombre) ;
                    rep.lblTipoImpresion.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
                    rep.Watermark.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
                    rep.CeFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                    rep.CellPrintDate.Text = db.GetDateServer().ToString();

                    //Parametros para Reporte Arqueo Isla
                    #region <<< ParametrosReporte >>>
                    
                    if (TA.Equals(Parametros.TiposArqueo.Isla) || TA.Equals(Parametros.TiposArqueo.Turno))
                    {
                        rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;

                        if (TA.Equals(Parametros.TiposArqueo.Isla))
                        {

                            if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                            {
                                rep.Watermark.Text += " Nro. " + AIView.First().ReImpresionCount.ToString();
                                rep.lblTipoImpresion.Text += " Nro. " + AIView.First().ReImpresionCount.ToString();
                            }
                            if (AIView.Count(ai => ai.ArqueoEspecial) > 0)
                                rep.ceEspecial.Text = (AIView.Count(a => a.Oficial) > 0 ? "Arqueo Especial / Oficial" : "Arqueo Especial");
                            
                            rep.CeTecnico.Text = AIView.First().TecnicoNombre;
                            rep.CeTurno.Text = AIView.First().TurnoNumero.ToString();
                            rep.CeIsla.Text = AIView.First().IslaNombre;
                            rep.TablaFirmas.Rows[0].Visible = false;
                            rep.TablaFirmas.Rows[1].Visible = false;
                            rep.TablaFirmas.Rows[2].Visible = false;
                            rep.CeTitulo.Text = "ARQUEO DE ISLA";
                            rep.CellSegundaFirmaTitulo.Text = "Firma Técnico de Pista";
                            rep.CellSegundaFirmaNombre.Text = AIView.First().TecnicoNombre;
                            rep.CeNumeroArqueo.Text = AIView.First().ArqueoNumero.ToString();
                            rep.xrTableFecha.Visible = false;
                        }
                        else
                        {
                            if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                            {
                                int contador = 0;
                                contador = db.Turnos.Single(t => t.ID.Equals(TurnoID)).ReImpresionCount;
                                rep.Watermark.Text += " Nro. " + contador.ToString();
                                rep.lblTipoImpresion.Text += " Nro. " + contador.ToString();
                            }
                            rep.xrCellFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                            rep.xrTableHeader.Visible = false;
                            rep.xrTableCell1.Visible = false;
                            rep.xrTableCell8.Visible = false;
                            rep.CeTecnico.Visible = false;
                            rep.CeIsla.Visible = false;
                            rep.xrTableCell2.Visible = false;
                            rep.xrTableCell5.Visible = false;
                            rep.CeTurno.Visible = false;
                            rep.CeNumeroArqueo.Visible = false;
                            rep.CeTitulo.Text = "RESUMÉN DE TURNO # " + AIView.First().TurnoNumero.ToString();
                            rep.CellPrimerFirmaTitulo.Text = "Administrador";
                            var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
                            if (empleadoadmin.Count() <= 0)
                                _Admin = false;
                            else
                                rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;

                            rep.CellSegundaFirmaTitulo.Text = "Contador";
                            var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
                            if (empleadoCont.Count() <= 0)
                                _Contador = false;
                            else
                                rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;
                        }
                    }
                    else
                    {
                        if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
                        {
                            rep.Watermark.Text += " Nro. " + RD.ReImpresionCount.ToString();
                            rep.lblTipoImpresion.Text += " Nro. " + RD.ReImpresionCount.ToString();
                        }
                        rep.xrTableHeader.Visible = false;
                        rep.xrCellFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
                        //rv.Text = "RESUMEN DE ARQUEO POR DIA";
                        rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;
                        rep.CeTitulo.Text = "RESUMÉN DE ARQUEO POR DÍA # " + AIView.First().ResumenDiaNumero.ToString();
                        rep.CellPrimerFirmaTitulo.Text = "Administrador";
                        var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
                        if (empleadoadmin.Count() <= 0)
                            _Admin = false;
                        else
                            rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;


                        rep.CellSegundaFirmaTitulo.Text = "Contador";
                        var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
                        if (empleadoCont.Count() <= 0)
                            _Contador = false;
                        else
                            rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;

                    }

                    
                    
                    #endregion

                    var ExtraxionPago = db.ExtracionPagos.Where(ep => ep.Activo).OrderBy(o => o.Orden).ToList();

                    var AMView = dbView.VistaArqueoMangueras.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));
                    var AFPView = dbView.VistaArqueoFormaPagos.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));

                    
                    //Tabla de Lecturas
                    if (AMView.Count() > 0)
                    {
                        #region <<< TituloTablasLecturas >>>

                        DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                        xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                        float y = 55f;
                        y = (TA.Equals(Parametros.TiposArqueo.Turno) ? 10f :
                            TA.Equals(Parametros.TiposArqueo.ResumenDia) ? 10f : 55f);


                        xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(0F, y);
                        int WF = 0;

                        if (TA.Equals(Parametros.TiposArqueo.Isla))
                            WF = Convert.ToInt32(AMView.Count(ap => ap.ArqueoMangueraID > 0));
                        else
                            WF = Convert.ToInt32(AMView.GroupBy(g => new { g.ProductoID, g.Lado }).Distinct().Count());

                        xrTableAP.WidthF = (100f + (WF * 90f));

                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAPLado = new DevExpress.XtraReports.UI.XRTableRow();
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAPDisp = new DevExpress.XtraReports.UI.XRTableRow();
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAPProd = new DevExpress.XtraReports.UI.XRTableRow();
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDis = new DevExpress.XtraReports.UI.XRTableRow();
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotales = new DevExpress.XtraReports.UI.XRTableRow();

                        //Filas Tipo de Productos
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotProd = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowTotProd.Weight = 1D;
                        xrTableRowTotProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                        //Fila Cantidad Totales
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotCant = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowTotCant.Weight = 1D;
                        xrTableRowTotCant.Cells.Add(new Parametros.MyXRTableCell("Extracción Total", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
                        
                        //Fila Lados
                        xrTableRowAPLado.Weight = 1D;
                        xrTableRowAPLado.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        var celda = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
                        celda.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                        celda.StylePriority.UseBorders = false;
                        xrTableRowAPLado.Cells.Add(celda);
                        int llenado = 1;

                        //Fila Dispensadores
                        xrTableRowAPDisp.Weight = 1D;
                        xrTableRowAPDisp.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        var celdaDisp = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
                        celdaDisp.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                        celdaDisp.StylePriority.UseBorders = false;
                        xrTableRowAPDisp.Cells.Add(celdaDisp);

                        //Fila Productos
                        xrTableRowAPProd.Weight = 1D;
                        xrTableRowAPProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        var celdaProd = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.TopCenter, Color.Transparent, Color.Black);
                        celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Right;
                        celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                        celdaProd.StylePriority.UseBorders = false;
                        xrTableRowAPProd.Cells.Add(celdaProd);

                        #endregion
                    
                        #region <<< LadosMangueras >>>

                        foreach (var lado in AMView.GroupBy(l => l.Lado).Distinct())
                        {
                            float ancho = lado.Count() * 90;
                            xrTableRowAPLado.Cells.Add(new Parametros.MyXRTableCell("LADO " + lado.Key + " Punto de Llenado " + llenado.ToString(), ancho, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                            llenado++;

                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                            {
                                foreach (var disp in AMView.Where(d => d.Lado.Equals(lado.Key)).GroupBy(g => new { g.DispensadorNombre, g.Lado }).Distinct())
                                {
                                    float anchoDisp = disp.Count() * 90;
                                    xrTableRowAPDisp.Cells.Add(new Parametros.MyXRTableCell("Dispensador: " + disp.Key.DispensadorNombre, anchoDisp, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                                    AMView.Where(p => p.DispensadorNombre.Equals(disp.Key.DispensadorNombre) && p.Lado.Equals(disp.Key.Lado)).ToList().ForEach(prod =>
                                    {
                                    xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));
                                   
                                    });
                                }
                            }
                            else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                AMView.Where(l => l.Lado.Equals(lado.Key)).GroupBy(ap => new { ap.ProductoID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
                                {
                                    xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));
                                });
                            }
                        }
                        
                        #endregion
                    
                        xrTableAP.Rows.Add(xrTableRowAPLado);
                        if (TA.Equals(Parametros.TiposArqueo.Isla))
                        {
                            xrTableAP.Rows.Add(xrTableRowAPDisp);
                        }

                        xrTableAP.Rows.Add(xrTableRowAPProd);
                        rep.GroupHeaderArqueo.Controls.Add(xrTableAP);
                        y += xrTableAP.SizeF.Height;

                    
                        #region <<< LecturasMecanicas >>>

                        if (AMView.Count(l => l.EsLecturaMecanica) > 0)
                        {                            
                            DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA MECÁNICA", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightBlue, Color.Black);
                            LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                            y += 18f;
                                                        
                            DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                            xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaMecanicaInicial", "LecturaMecanicaFinal", "ExtraxionMecanica", false, true, true);
                            xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);

                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                                xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                            else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                               group am by new {am.Lado, am.ProductoID } into gr
                                               select gr;

                                               //select new
                                               //{
                                               //    LecturaMecanicaInicial = gr.Sum(s => s.LecturaMecanicaInicial),
                                               //    LecturaMecanicaFinal = gr.Sum(s => s.LecturaMecanicaFinal),
                                               //    ExtraxionMecanica = gr.Sum(s => s.ExtraxionMecanica)
                                               //};
                                List<ExtracionesMecanica> Mec = new List<ExtracionesMecanica>();
                                agrupado.ToList().ForEach(det =>
                                    {
                                        Mec.Add(new ExtracionesMecanica { Orden = det.First().MangueraNumero, LecturaMecanicaInicial = det.Sum(s => s.LecturaMecanicaInicial), LecturaMecanicaFinal = det.Sum(s => s.LecturaMecanicaFinal), ExtraxionMecanica = det.Sum(s => Convert.ToDecimal(s.ExtraxionMecanica)) });
                                    });

                                xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);
                                    //.Where(a => a.LecturaMecanicaInicial > 0 && a.LecturaMecanicaFinal > 0 && a.ExtraxionMecanica > 0).OrderBy(o => o.Orden);
                            }

                            rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                            DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                            y += 18f;


                        }

                        #endregion

                        #region <<< LecturaEfectivo >>>

                        if (AMView.Count(l => l.EsLecturaEfectivo) > 0)
                        {
                            DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA EFECTIVO", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightGray, Color.Black);
                            LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                            y += 18f;

                            //Lecturas Mecanicas
                            DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                            xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaEfectivoInicial", "LecturaEfectivoFinal", "ExtraxionEfectivo", false, true, true);
                            xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);                            
                            
                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                                xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                            else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                //var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                //               group am by new { am.ProductoID, am.Lado } into gr
                                //               select new
                                //               {
                                //                   LecturaEfectivoInicial = gr.Sum(s => s.LecturaEfectivoInicial),
                                //                   LecturaEfectivoFinal = gr.Sum(s => s.LecturaEfectivoFinal),
                                //                   ExtraxionEfectivo = gr.Sum(s => s.ExtraxionEfectivo)
                                //               };

                                //xrSubProd.ReportSource.DataSource = agrupado;

                                var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                               group am by new { am.Lado, am.ProductoID } into gr
                                               select gr;

                                List<ExtracionesEfectiva> Mec = new List<ExtracionesEfectiva>();
                                agrupado.ToList().ForEach(det =>
                                {
                                    Mec.Add(new ExtracionesEfectiva { Orden = det.First().MangueraNumero, LecturaEfectivoInicial = det.Sum(s => s.LecturaEfectivoInicial), LecturaEfectivoFinal = det.Sum(s => s.LecturaEfectivoFinal), ExtraxionEfectivo = det.Sum(s => Convert.ToDecimal(s.ExtraxionEfectivo)) });
                                });

                                xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);
                           
                            }

                            rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                            DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                            y += 18f;

                        }

                        #endregion

                        #region <<< LecturaElectronica >>>

                        if (AMView.Count(l => l.EsLecturaElectronica) > 0)
                        {
                            DevExpress.XtraReports.UI.XRLabel LabelLM = new Parametros.MyXRLabel("LECTURA ELECTRÓNICA", 150f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightSalmon, Color.Black);
                            LabelLM.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelLM);
                            y += 18f;

                            //Lecturas Mecanicas
                            DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                            xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("LecturaElectronicaInicial", "LecturaElectronicaFinal", "ExtraxionElectronica", false, true, true);
                            xrSubProd.SizeF = new System.Drawing.SizeF(725F, 54F);                           
                           
                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                                xrSubProd.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                            else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                //var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                //               group am by new { am.ProductoID, am.Lado } into gr
                                //               select new
                                //               {
                                //                   LecturaElectronicaInicial = gr.Sum(s => s.LecturaElectronicaInicial),
                                //                   LecturaElectronicaFinal = gr.Sum(s => s.LecturaElectronicaFinal),
                                //                   ExtraxionElectronica = gr.Sum(s => s.ExtraxionElectronica)
                                //               };

                                //xrSubProd.ReportSource.DataSource = agrupado;

                                var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                               group am by new { am.Lado, am.ProductoID } into gr
                                               select gr;

                                List<ExtracionesElectronica> Mec = new List<ExtracionesElectronica>();
                                agrupado.ToList().ForEach(det =>
                                {
                                    Mec.Add(new ExtracionesElectronica { Orden = det.First().MangueraNumero, LecturaElectronicaInicial = det.Sum(s => s.LecturaElectronicaInicial), LecturaElectronicaFinal = det.Sum(s => s.LecturaElectronicaFinal), ExtraxionElectronica = det.Sum(s => Convert.ToDecimal(s.ExtraxionElectronica)) });
                                });

                                xrSubProd.ReportSource.DataSource = Mec.OrderBy(o => o.Orden);
                           
                            }
                            
                            rep.GroupHeaderArqueo.Controls.Add(xrSubProd);

                            DevExpress.XtraReports.UI.XRLabel LabelInicial = new Parametros.MyXRLabel("Inicial", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelInicial.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelInicial);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelFinal = new Parametros.MyXRLabel("Final", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelFinal.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelFinal);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelExtraxion = new Parametros.MyXRLabel("Extracción", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelExtraxion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelExtraxion);
                            y += 18f;

                        }

                        #endregion

                        #region <<< Verificacion >>>

                        if (AMView.Count(l => l.EsLecturaEfectivo) > 0 || AMView.Count(l => l.EsLecturaElectronica) > 0)
                        {
                            DevExpress.XtraReports.UI.XRLabel LabelVerificacion = new Parametros.MyXRLabel("VERIFICACIÓN", xrTableAP.WidthF, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGreen, Color.Black);
                            LabelVerificacion.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelVerificacion);
                            y += 18f;

                            //Verificación
                            DevExpress.XtraReports.UI.XRSubreport xrSubVerificacion = new DevExpress.XtraReports.UI.XRSubreport();
                            xrSubVerificacion.LocationFloat = new DevExpress.Utils.PointFloat(100f, y);
                            xrSubVerificacion.ReportSource = new Reportes.Arqueos.Hojas.SubRptLecturas("VerificacionEfectivo", "VerificacionMecElectronica", "", true,
                                (AMView.Count(c => c.EsLecturaEfectivo) > 0 ? true : false),
                                (AMView.Count(c => c.EsLecturaElectronica) > 0 ? true : false)
                            );
                            xrSubVerificacion.SizeF = new System.Drawing.SizeF(725F, 54F);

                            if (TA.Equals(Parametros.TiposArqueo.Isla))
                                xrSubVerificacion.ReportSource.DataSource = AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero);
                            else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
                            {
                                var agrupado = from am in AMView.OrderBy(o => o.Lado).ThenBy(o => o.DispensadorNombre).ThenBy(o => o.MangueraNumero)
                                               group am by new { am.ProductoID, am.Lado } into gr
                                               select new
                                               {
                                                   VerificacionEfectivo = gr.Sum(s => (s.VerificacionEfectivo.HasValue ? (s.ExtraxionElectronica > 0 ? s.VerificacionEfectivo : 0m) : 0m)),  // s.VerificacionEfectivo),
                                                   VerificacionMecElectronica = gr.Sum(s => (s.VerificacionMecElectronica.HasValue ? (s.ExtraxionElectronica > 0 ? s.VerificacionMecElectronica : 0m) : 0m)) //s.VerificacionMecElectronica)
                                               };

                                xrSubVerificacion.ReportSource.DataSource = agrupado;
                            }

                            rep.GroupHeaderArqueo.Controls.Add(xrSubVerificacion);

                            DevExpress.XtraReports.UI.XRLabel LabelEfectivo = new Parametros.MyXRLabel("Efectivo", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelEfectivo.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelEfectivo);
                            y += 18f;
                            DevExpress.XtraReports.UI.XRLabel LabelMecElectronica = new Parametros.MyXRLabel("Mec. Vs Electrónica", 100f, 18f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black);
                            LabelMecElectronica.LocationFloat = new DevExpress.Utils.PointFloat(0f, y);
                            rep.GroupHeaderArqueo.Controls.Add(LabelMecElectronica);
                        }

                        #endregion

                        float sizeWidht = (100f + (AMView.GroupBy(ap => new { ap.ProductoID}).Distinct().Count() * 90));
                        float sizeWidhtSec = (100f + (AMView.GroupBy(ap => new { ap.ProductoID }).Distinct().Count() * 80));
                        float yf = 0f;

                        #region <<< TITULOSDESCUENTO >>>

                        //Label Descuento
                        DevExpress.XtraReports.UI.XRLabel LabelTituloDescuento = new Parametros.MyXRLabel("DESCUENTO", sizeWidht, 17f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black);
                        LabelTituloDescuento.LocationFloat = new DevExpress.Utils.PointFloat(0f, yf);
                        //rep.Detail.Controls.Add(LabelTituloDescuento);
                        //yf += 17f;

                        DevExpress.XtraReports.UI.XRLabel LabelTituloProductos = new Parametros.MyXRLabel("PRODUCTOS", sizeWidht - 100f, 17f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black);
                        LabelTituloProductos.LocationFloat = new DevExpress.Utils.PointFloat(0f + 100f, yf);
                        //rep.Detail.Controls.Add(LabelTituloProductos);
                        
                        //Titulo Producto Descuento
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProdTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowDescuentoProdTitulo.Weight = 1D;
                        xrTableRowDescuentoProdTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                        //xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell("PRODUCTOS", xrTableAPPago.WidthF - 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                        //Productos Descuento
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProd = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowDescuentoProd.Weight = 1D;
                        xrTableRowDescuentoProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        xrTableRowDescuentoProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                        //Linea Descuentos Totales
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDesc = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowTotalDesc.Weight = 1D;
                        xrTableRowTotalDesc.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        xrTableRowTotalDesc.Cells.Add(new Parametros.MyXRTableCell("Descuentos Totales", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                        //Liena Total Descuento
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDescuentos = new DevExpress.XtraReports.UI.XRTableRow();
                        xrTableRowTotalDescuentos.Weight = 1D;
                        xrTableRowTotalDescuentos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                        xrTableRowTotalDescuentos.Cells.Add(new Parametros.MyXRTableCell("Total Descuento", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

                        #endregion
                        
                        #region <<< TOTALESEXTRAXION >>>

                        //Parametros SubReport
                        float WidthFull = 0f;
                        float WidthDiferenciado = 0f;
                        bool ShowFull = false;
                        bool ShowDiferenciado = false;

                        if (AMView.Count(a => !a.EsDiferenciado) > 0)
                        {
                            WidthFull = AMView.GroupBy(w => new { w.EsDiferenciado, w.ProductoID, w.TanqueID }).Count(c => !c.Key.EsDiferenciado) * 90;
                            ShowFull = true;
                        }
                        
                        if (AMView.Count(a => a.EsDiferenciado) > 0)
                        {
                            WidthDiferenciado = AMView.GroupBy(w => new { w.EsDiferenciado, w.ProductoID, w.TanqueID }).Count(c => c.Key.EsDiferenciado) * 90;
                            ShowDiferenciado = true;
                        }

                        //EXTRAXIONES VENTAS
                        var ListaExtraxionVenta = from APTot in AMView.GroupBy(ap => new { ap.EsDiferenciado, ap.ProductoID, ap.ProductoNombre, ap.TanqueID, ap.TanqueNombre }).Distinct().OrderBy(o => o.Key.EsDiferenciado).ThenByDescending(o => o.Key.ProductoNombre).ThenByDescending(o => o.Key.TanqueNombre).ToList()
                                                  select new
                                                  {
                                                      ExtraVenta = Decimal.Round((APTot.GroupBy(g => new { g.ArqueoProductoID, g.VentaLitro }).Sum(s => s.Key.VentaLitro)), 3, MidpointRounding.AwayFromZero),
                                                      //GroupBy(g => g.VentaLitro).FirstOrDefault() != null ? APTot.GroupBy(g => g.VentaLitro).FirstOrDefault().Key : 0m)
                                                      //((APTot.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica))) -
                                                      //(vistaExtracciones.Count(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)) > 0 ? Convert.ToDecimal(vistaExtracciones.Where(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)).Sum(s => s.Valor)) : 0m)), 3, MidpointRounding.AwayFromZero),
                                                      PrecioVenta = APTot.Average(o => o.Precio),
                                                      Descuento = Convert.ToDecimal(APTot.Max(v => v.MangueraDescuento)),
                                                      ValorVenta = Decimal.Round((APTot.GroupBy(g => new { g.ArqueoProductoID, g.VentaValor }).Sum(s => s.Key.VentaValor)), 3, MidpointRounding.AwayFromZero),
                                                      //Decimal.Round((((APTot.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica))) -
                                                      //(vistaExtracciones.Count(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)) > 0 ? Convert.ToDecimal(vistaExtracciones.Where(v => v.ProductoID.Equals(APTot.Key.ProductoID) && v.EsDiferenciado.Equals(APTot.Key.EsDiferenciado)).Sum(s => s.Valor)) : 0m)) * APTot.Average(o => o.Precio)), 2, MidpointRounding.AwayFromZero)
                                                  };

                        List<ListDist> ListExtraDist = new List<ListDist>();

                        vistaExtracciones.Where(v => !v.Valor.Equals(0)).OrderBy(o => o.EsDiferenciado).ThenBy(t => t.ProductoID).ToList().ForEach(vExtra =>
                            {
                                AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID }).OrderBy(o => o.Key.EsDiferenciado).Select(s => s.FirstOrDefault()).Distinct().ToList().ForEach(prod =>
                            {
                                ListDist ld = new ListDist();
                                ld.ID = Convert.ToInt32(vExtra.ID);
                                ld.ExtraxionNombre = vExtra.ExtraxionNombre;
                                ld.ProductoID = prod.ProductoID;
                                ld.TanqueID = prod.TanqueID;
                                ld.TanqueNombre = prod.TanqueNombre;
                                ld.ProductoNombre = (prod.EsDiferenciado ? "0" : "1") + prod.ProductoNombre;
                                if (prod.ProductoID.Equals(vExtra.ProductoID) && prod.TanqueID.Equals(vExtra.TanqueID) && prod.EsDiferenciado.Equals(vExtra.EsDiferenciado))
                                    ld.Valor = vExtra.Valor;
     
                                ListExtraDist.Add(ld);
                            });

                            });

                        var listDistFinal = from l in ListExtraDist
                                            select new
                                            {
                                                ID = l.ID,
                                                ExtraxionNombre = l.ExtraxionNombre,
                                                ProductoID = l.ProductoID,
                                                ProductoNombre = l.ProductoNombre + " => " + l.TanqueNombre,
                                                TanqueID = l.TanqueID,
                                                TanqueNombre = l.TanqueNombre,
                                                Valor = l.Valor
                                            };
                       
                        var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (TA.Equals(Parametros.TiposArqueo.Isla) ? (s.Sum(x => x.ExtraxionElectronica) > 0 ? s.Sum(x => x.ExtraxionElectronica) : (s.Sum(x => x.ExtraxionElectronica.HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica /*(!x.MecanicaAmbasCara ? x.ExtraxionMecanica : 0m)*/) : x.ExtraxionMecanica))) : (s.Sum(x => x.ExtraxionElectronica.HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica) : x.ExtraxionMecanica))), s.Key.EsDiferenciado });
                        //var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (s.Sum(x => s.Sum(z => z.ExtraxionElectronica).HasValue ? (x.ExtraxionElectronica > 0 ? x.ExtraxionElectronica : x.ExtraxionMecanica /*(!x.MecanicaAmbasCara ? x.ExtraxionMecanica : 0m)*/) : x.ExtraxionMecanica)), s.Key.EsDiferenciado });
                        //var TotalExtraxion = AMView.GroupBy(g => new { g.ProductoID, ProductoNombre = g.ProductoNombre + " => " + g.TanqueNombre, g.EsDiferenciado, g.Precio, g.TanqueID }).Select(s => new { s.Key.ProductoNombre, ExtraxionElectronica = (s.Count(c => c.EsLecturaElectronica) > 0 ? s.Sum(x => x.ExtraxionElectronica) : s.Sum(x => x.ExtraxionMecanica)), s.Key.EsDiferenciado });

                        rep.xrSubreportExtraxiones.ReportSource = new Reportes.Arqueos.Hojas.SubRptPivotProduct((100f + (AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID }).Distinct().Count() * 90)), ShowFull, WidthFull, ShowDiferenciado, WidthDiferenciado, TotalExtraxion, listDistFinal, ListaExtraxionVenta);
                        _DifValorTotalVenta = (ListaExtraxionVenta.Count() > 0 ? ListaExtraxionVenta.Sum(s => s.ValorVenta) : 0m);
                        #endregion

                        #region <<< RESUMEN >>>

                        var ListaFP = (from list in AFPView.Where(v => v.MonedaID.Equals(0))
                                       group list by new { list.PagoID, list.PagoNombre, list.Orden } into gr
                                       select new
                                       {
                                           Nombre = gr.Key.PagoNombre,
                                           Orden = gr.Key.Orden,
                                           Valor = gr.Where(w => w.PagoID.Equals(gr.Key.PagoID)).Sum(s => s.Valor)
                                       }).OrderBy(o => o.Orden);


                        ///DESCUENTOS...........................
                        
                        List<ListDescuento> desc = new List<ListDescuento>();

                        foreach (Parametros.TiposDescuento val in Enum.GetValues(typeof(Parametros.TiposDescuento)).AsParallel())
                        {

                            AMView.GroupBy(ap => new { ap.ProductoID, ap.TanqueID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
                            {
                                var vista = AMView.Where(v => v.ProductoID.Equals(prod.ProductoID) && v.TanqueID.Equals(prod.TanqueID));

                                if (vista.Count() > 0)
                                {
                                    
                                    ListDescuento ld = new ListDescuento();
                                    ld.TipoDescuentoID = Convert.ToInt32(val);
                                    ld.TipoDescuentoNombre = Parametros.General.GetEnumTiposDescuento(val);
                                    ld.ProductoID = prod.ProductoID;
                                    ld.ProductoNombre = prod.ProductoNombre;

                                    if (val.Equals(Parametros.TiposDescuento.Isla))
                                    {
                                        decimal Desc = 0m;
                                        decimal NonDesc = 0m;

                                        List<string> dispensadores = new List<string>();

                                        vista.ToList().ForEach(objMang =>
                                        {
                                            if (!objMang.MangueraDescuento.Equals(0))
                                            {
                                                decimal extraxion = Convert.ToDecimal(objMang.EsLecturaElectronica ? objMang.ExtraxionElectronica : objMang.ExtraxionMecanica);
                                                Desc += Decimal.Round(Convert.ToDecimal(extraxion * objMang.MangueraDescuento), 2, MidpointRounding.AwayFromZero);
                                                NonDesc = objMang.MangueraDescuento;

                                                if (dispensadores.Contains(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString()))
                                                {
                                                    var vistaAPE = vistaExtracciones.Where(v => v.ProductoID.Equals(prod.ProductoID) && v.TanqueID.Equals(prod.TanqueID) && v.ArqueoProductoID.Equals(objMang.ArqueoProductoID));

                                                    if (vistaAPE.Count() > 0)
                                                        Desc -= Decimal.Round(Convert.ToDecimal(vistaAPE.Sum(s => s.Valor)) * NonDesc, 2, MidpointRounding.AwayFromZero);
                                                }
                                                dispensadores.Add(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString());
                                            }
                                        });

                                        ld.Valor = Desc;
                                    }
                                    else if (val.Equals(Parametros.TiposDescuento.Especial))
                                    {
                                        decimal ValorDE = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID, g.TanqueID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoEspecial)), 2, MidpointRounding.AwayFromZero);
                                        ld.Valor = ValorDE;
                                    }
                                    else if (val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
                                    {
                                        decimal ValorDGPCF = 0m;

                                        vista.GroupBy(ap => new { ap.ProductoID, ap.ArqueoIslaID, ap.TanqueID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(objProd =>
                                        {
                                            ValorDGPCF += objProd.DescuentoGalonesPetroCardFormula != null ? Decimal.Round(Convert.ToDecimal(Parametros.General.ValorFormula(objProd.DescuentoGalonesPetroCardFormula.ToString())), 2, MidpointRounding.AwayFromZero) : 0m;
                                        });

                                        ld.Valor = ValorDGPCF;
                                   }
                                    else if (val.Equals(Parametros.TiposDescuento.CordobasPetroCard))
                                    {
                                        decimal ValorDGDCV = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID, g.TanqueID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoGalonesPetroCardValor)), 2, MidpointRounding.AwayFromZero);
                                    
                                        ld.Valor = ValorDGDCV;
                                    }

                                    if (!val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
                                        _GrandTotalDesucento += ld.Valor; ;
                                    
                                    desc.Add(ld);
                                }
                            });



                        }
                        ///DESCUENTOS.......

                        var listDescuento = from l in desc
                                        select new
                                        {
                                            TipoDescuentoID = l.TipoDescuentoID,
                                            TipoDescuentoNombre = l.TipoDescuentoNombre,
                                            ProductoNombre = l.ProductoNombre,
                                            Valor = l.Valor
                                        };

                        rep.xrSubreportResumen.LocationF = new DevExpress.Utils.PointFloat((100f + (AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado, ap.TanqueID}).Distinct().Count() * 90)) + 10f, 0f);
                        _ValorTotalVenta = AIView.Sum(s => s.ValorTotalVenta);
                        _VentasContado = AIView.Sum(s => s.VentasContado);
                        _TotalSobranteFaltante = AIView.Sum(s => s.SobranteFaltante);
                        _TotalDiferenciaRecibida = AIView.Sum(s => s.DiferenciaRecibida);
                        

                        if (AFPView.Count(v => !v.MonedaID.Equals(0)) > 0)
                            _TotalEfectivoRecibido = Decimal.Round(AFPView.Where(v => !v.MonedaID.Equals(0)).Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);

                        rep.xrSubreportResumen.ReportSource = new Reportes.Arqueos.Hojas.SubRptPagoArqueos(sizeWidhtSec, _ValorTotalVenta, _DifValorTotalVenta -_ValorTotalVenta,  ListaFP, listDescuento, _GrandTotalDesucento, _VentasContado, _TotalEfectivoRecibido, _TotalSobranteFaltante, _TotalDiferenciaRecibida);
                        
                        #endregion

                        #region <<< TANQUES >>>
                        if (TA.Equals(Parametros.TiposArqueo.ResumenDia))
                        {
                            //Tabla Tanques
                            DevExpress.XtraReports.UI.XRTable xrTableTanques = new DevExpress.XtraReports.UI.XRTable();
                            xrTableTanques.Font = new System.Drawing.Font("Tahoma", 8F);
                            xrTableTanques.LocationFloat = new DevExpress.Utils.PointFloat(0f, 5f);

                            //Filas
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTanTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                            DevExpress.XtraReports.UI.XRTableRow xrTableRowTanExtraxion = new DevExpress.XtraReports.UI.XRTableRow();

                            //Fila Titulo
                            xrTableRowTanTitulo.Weight = 1D;
                            xrTableRowTanTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell("TANQUES", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));


                            //Fila Extraxion 
                            xrTableRowTanExtraxion.Weight = 1D;
                            xrTableRowTanExtraxion.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                            xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell("EXTRACCIÓN", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

                            var tanques = from t in AMView
                                          group t by new { t.TanqueID, t.TanqueNombre, t.Color, t.ProductoNombre } into g
                                          select new
                                          {
                                              Nombre = g.Key.TanqueNombre + " (" + g.Key.ProductoNombre + ")",
                                              Color = g.Key.Color,
                                              Total = (g.Count(s => s.EsLecturaElectronica) > 0 ? g.Sum(a => a.ExtraxionElectronica) : g.Sum(a => a.ExtraxionMecanica))
                                          };

                            xrTableTanques.SizeF = new System.Drawing.SizeF((100f + (tanques.Count() * 90)), 32F);

                            tanques.OrderBy(o => o.Nombre).ToList().ForEach(lista =>
                            {
                                xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell(lista.Nombre, 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(lista.Color), Color.White));
                                xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(lista.Total).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                            });

                            xrTableTanques.Rows.Add(xrTableRowTanTitulo);
                            xrTableTanques.Rows.Add(xrTableRowTanExtraxion);

                            rep.GroupFooterArqueo.Controls.Add(xrTableTanques);
                        }
                        #endregion

                        #region <<< OBSERVACIONES >>>

                        if (AIView.Count(ai => ai.Observacion.Length > 0) > 0)
                        {
                            DevExpress.XtraReports.UI.XRLabel xrLabelO = new DevExpress.XtraReports.UI.XRLabel();
                            xrLabelO.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
                            xrLabelO.LocationFloat = new DevExpress.Utils.PointFloat(0F, 43f);
                            xrLabelO.Name = "xrLabelO";
                            xrLabelO.Multiline = true;
                            xrLabelO.SizeF = new System.Drawing.SizeF(820F, 15F);
                            xrLabelO.Text = "Observaciones: ";

                            AIView.Where(ai => ai.Observacion.Length > 0).ToList().ForEach(nota =>
                            { xrLabelO.Text += nota.Observacion.ToString() + "  |  "; });
                            rep.GroupFooterArqueo.Controls.Add(xrLabelO);
                        }
                        #endregion


                    }

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    if (!_Admin)
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOADMNISTRADOR + Environment.NewLine, Parametros.MsgType.warning);

                    if (!_Contador)
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOCONTADOR + Environment.NewLine, Parametros.MsgType.warning);


                    if (ToPrint)
                    {
                        rep.CreateDocument();
                        rep.Print();
                        
                    }
                    else
                    {
                        rep.CreateDocument(true);
                        printArea.PrintingSystem = rep.PrintingSystem;
                    }

                    Formulario.Cursor = Cursors.Default;

                    rep.RequestParameters = true;

                }
                else
                {
                    //vaild = false;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Formulario.Cursor = Cursors.Default;
                    Parametros.General.DialogMsg("No existe ningún Arqueo de Isla creado.", Parametros.MsgType.warning);
                }

            }
            catch (Exception ex)
            {
                //vaild = false;
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, Formulario.Name);
            }


        }

        /////////////******************************************NO SE USA********************************************************///////////////
        //public static void PrintArqueoTest(Form Formulario, Entidad.SAGASDataViewsDataContext dbView,
        //    Entidad.SAGASDataClassesDataContext db, int ArqueoIslaID, int TurnoID, Entidad.ResumenDia RD,
        //    Parametros.TiposImpresion TI, bool ToPrint, Parametros.TiposArqueo TA, string Descripcion, DevExpress.XtraPrinting.Control.PrintControl printArea)
        //{
        //    try
        //    {
        //        Parametros.General.ShowWaitSplash(Formulario, Parametros.WaitFormCaption.GENERANDO.ToString(), Descripcion);

        //        Formulario.Cursor = Cursors.WaitCursor;
        //        decimal ThisDescuentoPetroCard = RD.DescuentoPetroCard;

        //        var AIView = from v in dbView.VistaArqueoIslas
        //                     where (v.ArqueoIslaID.Equals(ArqueoIslaID) || ArqueoIslaID.Equals(0)) && (v.TurnoID.Equals(TurnoID) || TurnoID.Equals(0))
        //                     && v.ResumenDiaID.Equals(RD.ID)
        //                     select v;

        //        if (AIView.Count() > 0)
        //        {
        //            //Datos Iniciales del Reporte
        //            //Reportes.Arqueos.Hojas.RptArqueo 
        //            //rep = new Reportes.Arqueos.Hojas.RptArqueo();
        //            Reportes.Arqueos.Hojas.RptArqueo rep = new Reportes.Arqueos.Hojas.RptArqueo();
        //            string Nombre, Direccion, Telefono;
        //            System.Drawing.Image picture_LogoEmpresa;
        //            decimal _ValorTotalVenta = 0m;
        //            decimal _TotalFormasPago = 0m;
        //            decimal _GrandTotalDesucento = 0m;
        //            decimal _VentasContado = 0m;
        //            decimal _TotalEfectivoRecibido = 0m;
        //            decimal _GRANDTOTAL = 0m;
        //            bool _Contador = true;
        //            bool _Admin = true;

        //            Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
        //            rep.PicLogo.Image = picture_LogoEmpresa;
        //            rep.CeEmpresa.Text = Nombre;
        //            rep.CeEstacion.Text = AIView.First().EstacionNombre;
        //            rep.lblTipoImpresion.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
        //            rep.Watermark.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();
        //            rep.CeFecha.Text = AIView.First().ArqueoIslaFecha.ToShortDateString();
        //            rep.CellPrintDate.Text = db.GetDateServer().ToString();

        //            //Parametros para Reporte Arqueo Isla
        //            if (TA.Equals(Parametros.TiposArqueo.Isla) || TA.Equals(Parametros.TiposArqueo.Turno))
        //            {
        //                rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;

        //                if (TA.Equals(Parametros.TiposArqueo.Isla))
        //                {

        //                    if (TI.Equals(Parametros.TiposImpresion.Re_Impresion))
        //                        rep.lblTipoImpresion.Text += " Nro. " + AIView.First().ReImpresionCount.ToString();
        //                    rep.CeTecnico.Text = AIView.First().TecnicoNombre;
        //                    rep.CeTurno.Text = AIView.First().TurnoNumero.ToString();
        //                    rep.CeIsla.Text = AIView.First().IslaNombre;
        //                    rep.TablaFirmas.Rows[0].Visible = false;
        //                    rep.TablaFirmas.Rows[1].Visible = false;
        //                    rep.TablaFirmas.Rows[2].Visible = false;
        //                    rep.CeTitulo.Text = "ARQUEO DE ISLA";
        //                    rep.CellSegundaFirmaTitulo.Text = "Firma Técnico de Pista";
        //                    rep.CellSegundaFirmaNombre.Text = AIView.First().TecnicoNombre;
        //                    rep.CeNumeroArqueo.Text = AIView.First().ArqueoNumero.ToString();
        //                }
        //                else
        //                {
        //                    rep.xrTableCell1.Visible = false;
        //                    rep.xrTableCell8.Visible = false;
        //                    rep.CeTecnico.Visible = false;
        //                    rep.CeIsla.Visible = false;
        //                    rep.xrTableCell2.Visible = false;
        //                    rep.xrTableCell5.Visible = false;
        //                    rep.CeTurno.Visible = false;
        //                    rep.CeNumeroArqueo.Visible = false;
        //                    rep.CeTitulo.Text = "RESUMÉN DE TURNO # " + AIView.First().TurnoNumero.ToString();
        //                    rep.CellPrimerFirmaTitulo.Text = "Administrador";
        //                    var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
        //                    if (empleadoadmin.Count() <= 0)
        //                        _Admin = false;
        //                    else
        //                        rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;
                                  
        //                    rep.CellSegundaFirmaTitulo.Text = "Contador";
        //                    var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
        //                    if (empleadoCont.Count() <= 0)
        //                        _Contador = false;
        //                    else
        //                        rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;
        //                }
        //            }
        //            else
        //            {
        //                rep.xrTableHeader.Visible = false;
        //                //rv.Text = "RESUMEN DE ARQUEO POR DIA";
        //                rep.CellFirmaArqueador.Text = AIView.First().ArquedaorNombre;
        //                rep.CeTitulo.Text = "RESUMÉN DE ARQUEO POR DÍA # " + AIView.First().ResumenDiaNumero.ToString();
        //                rep.CellPrimerFirmaTitulo.Text = "Administrador";
        //                var empleadoadmin = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).AdministradorID)));
        //                if (empleadoadmin.Count() <= 0)
        //                    _Admin = false;
        //                else
        //                    rep.CellPrimerFirmaNombre.Text = empleadoadmin.First().Nombres + " " + empleadoadmin.First().Apellidos;


        //                rep.CellSegundaFirmaTitulo.Text = "Contador";
        //                var empleadoCont = db.Empleados.Where(ep => ep.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(RD.EstacionServicioID)).ResponsableContableID)));
        //                if (empleadoCont.Count() <= 0)
        //                    _Contador = false;
        //                else
        //                    rep.CellSegundaFirmaNombre.Text = empleadoCont.First().Nombres + " " + empleadoCont.First().Apellidos;

        //            }

                    
        //            var ExtraxionPago = db.ExtracionPagos.Where(ep => ep.Activo).OrderBy(o => o.Orden).ToList();

        //            var AMView = dbView.VistaArqueoMangueras.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));

        //            //Tabla de Lecturas
        //            if (AMView.Count() > 0)
        //            {
        //                #region <<< CREACIONTABLASFILAS >>>

        //                #region <<< TituloTablasLecturas >>>

        //                DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
        //                xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
        //                float y = 55f;
        //                y = (TA.Equals(Parametros.TiposArqueo.Turno) ? 40f :
        //                    TA.Equals(Parametros.TiposArqueo.ResumenDia) ? 10f : 55f);

        //                xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(0F, y);
        //                int WF = 0;

        //                if (TA.Equals(Parametros.TiposArqueo.Isla))
        //                    WF = Convert.ToInt32(AMView.Count(ap => ap.ArqueoMangueraID > 0));
        //                else
        //                    WF = Convert.ToInt32(AMView.GroupBy(g => new { g.ProductoID, g.Lado }).Distinct().Count());


        //                xrTableAP.SizeF = new System.Drawing.SizeF((100f + (WF * 90f)), 50F);

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPLado = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPDisp = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPProd = new DevExpress.XtraReports.UI.XRTableRow();



        //                //Fila Lados
        //                xrTableRowAPLado.Weight = 1D;
        //                xrTableRowAPLado.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                var celda = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
        //                celda.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        //                celda.StylePriority.UseBorders = false;
        //                xrTableRowAPLado.Cells.Add(celda);
        //                int llenado = 1;

        //                //Fila Dispensadores
        //                xrTableRowAPDisp.Weight = 1D;
        //                xrTableRowAPDisp.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                var celdaDisp = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black);
        //                celdaDisp.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        //                celdaDisp.StylePriority.UseBorders = false;
        //                xrTableRowAPDisp.Cells.Add(celdaDisp);

        //                //Fila Productos
        //                xrTableRowAPProd.Weight = 1D;
        //                xrTableRowAPProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                var celdaProd = new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.TopCenter, Color.Transparent, Color.Black);
        //                celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Right;
        //                celdaProd.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
        //                celdaProd.StylePriority.UseBorders = false;
        //                xrTableRowAPProd.Cells.Add(celdaProd);

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPEfectivo = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPMecVsElec = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Titulo
        //                xrTableRowAPTitulo.Weight = 1D;
        //                xrTableRowAPTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPTitulo.Cells.Add(new Parametros.MyXRTableCell("VERIFICACIÓN", xrTableAP.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGreen, Color.Black));

        //                //Fila Efectivo
        //                xrTableRowAPEfectivo.Weight = 1D;
        //                xrTableRowAPEfectivo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPEfectivo.Cells.Add(new Parametros.MyXRTableCell("Efectivo", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Mec Vs Elec
        //                xrTableRowAPMecVsElec.Weight = 1D;
        //                xrTableRowAPMecVsElec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPMecVsElec.Cells.Add(new Parametros.MyXRTableCell("Mec. Vs Electrónica", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< TITULOSMECANICA >>>
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPTituloMec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPInicialMec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPFinalMec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPExtraxionMec = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Titulo
        //                xrTableRowAPTituloMec.Weight = 1D;
        //                xrTableRowAPTituloMec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPTituloMec.Cells.Add(new Parametros.MyXRTableCell("LECTURA MECÁNICA", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightBlue, Color.Black));
        //                xrTableRowAPTituloMec.Cells.Add(new Parametros.MyXRTableCell(" ", xrTableAP.WidthF - 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Inicial
        //                xrTableRowAPInicialMec.Weight = 1D;
        //                xrTableRowAPInicialMec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPInicialMec.Cells.Add(new Parametros.MyXRTableCell("Inicial", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Final
        //                xrTableRowAPFinalMec.Weight = 1D;
        //                xrTableRowAPFinalMec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPFinalMec.Cells.Add(new Parametros.MyXRTableCell("Final", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Extraxion
        //                xrTableRowAPExtraxionMec.Weight = 1D;
        //                xrTableRowAPExtraxionMec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPExtraxionMec.Cells.Add(new Parametros.MyXRTableCell("Extracción", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                #endregion

        //                #region <<< TITULOSEFECTIVO >>>
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPTituloEfe = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPInicialEfe = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPFinalEfe = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPExtraxionEfe = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Titulo
        //                xrTableRowAPTituloEfe.Weight = 1D;
        //                xrTableRowAPTituloEfe.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPTituloEfe.Cells.Add(new Parametros.MyXRTableCell("LECTURA EFECTIVO", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightGray, Color.Black));
        //                xrTableRowAPTituloEfe.Cells.Add(new Parametros.MyXRTableCell(" ", xrTableAP.WidthF - 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Inicial
        //                xrTableRowAPInicialEfe.Weight = 1D;
        //                xrTableRowAPInicialEfe.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPInicialEfe.Cells.Add(new Parametros.MyXRTableCell("Inicial", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Final
        //                xrTableRowAPFinalEfe.Weight = 1D;
        //                xrTableRowAPFinalEfe.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPFinalEfe.Cells.Add(new Parametros.MyXRTableCell("Final", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Extraxion
        //                xrTableRowAPExtraxionEfe.Weight = 1D;
        //                xrTableRowAPExtraxionEfe.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPExtraxionEfe.Cells.Add(new Parametros.MyXRTableCell("Extracción", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< TITULOSELECTRONICO >>>

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPTituloElec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPInicialElec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPFinalElec = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPExtraxionElec = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Titulo
        //                xrTableRowAPTituloElec.Weight = 1D;
        //                xrTableRowAPTituloElec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPTituloElec.Cells.Add(new Parametros.MyXRTableCell("LECTURA ELECTRÓNICA", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LightSalmon, Color.Black));
        //                xrTableRowAPTituloElec.Cells.Add(new Parametros.MyXRTableCell(" ", xrTableAP.WidthF - 150f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Inicial
        //                xrTableRowAPInicialElec.Weight = 1D;
        //                xrTableRowAPInicialElec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPInicialElec.Cells.Add(new Parametros.MyXRTableCell("Inicial", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Final
        //                xrTableRowAPFinalElec.Weight = 1D;
        //                xrTableRowAPFinalElec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPFinalElec.Cells.Add(new Parametros.MyXRTableCell("Final", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Extraxion
        //                xrTableRowAPExtraxionElec.Weight = 1D;
        //                xrTableRowAPExtraxionElec.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPExtraxionElec.Cells.Add(new Parametros.MyXRTableCell("Extracción", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< TOTALESEXTRAXION >>>

        //                DevExpress.XtraReports.UI.XRTable xrTableAPProd = new DevExpress.XtraReports.UI.XRTable();
        //                xrTableAPProd.Font = new System.Drawing.Font("Tahoma", 8F);
        //                xrTableAPProd.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10f);

        //                xrTableAPProd.SizeF = new System.Drawing.SizeF((100f + (AMView.GroupBy(ap => new { ap.ProductoID, ap.EsDiferenciado }).Distinct().Count() * 90)), 50F);

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDis = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotales = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotProd = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotCant = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Total
        //                xrTableRowTotalDis.Weight = 1D;
        //                xrTableRowTotalDis.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotalDis.Cells.Add(new Parametros.MyXRTableCell("TOTAL DISPENSADOR", xrTableAPProd.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                xrTableAPProd.Rows.Add(xrTableRowTotalDis);

        //                //Fila Totales 
        //                xrTableRowTotales.Weight = 1D;
        //                xrTableRowTotales.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotales.Cells.Add(new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                if (AMView.Count(a => !a.EsDiferenciado) > 0)
        //                    xrTableRowTotales.Cells.Add(new Parametros.MyXRTableCell("PRECIO FULL", AMView.Select(o => new { o.EsDiferenciado, o.ProductoID }).Where(w => !w.EsDiferenciado).Distinct().Count() * 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                if (AMView.Count(a => a.EsDiferenciado) > 0)
        //                    xrTableRowTotales.Cells.Add(new Parametros.MyXRTableCell("PRECIO DIFERENCIADO", AMView.Select(o => new { o.EsDiferenciado, o.ProductoID }).Where(w => w.EsDiferenciado).Distinct().Count() * 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                xrTableAPProd.Rows.Add(xrTableRowTotales);

        //                //Filas Tipo de Productos
        //                xrTableRowTotProd.Weight = 1D;
        //                xrTableRowTotProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Fila Cantidad Totales
        //                xrTableRowTotCant.Weight = 1D;
        //                xrTableRowTotCant.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotCant.Cells.Add(new Parametros.MyXRTableCell("Extracción Total", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< DistribucionExtraxion >>>

        //                var APEView = dbView.VistaArqueoProductoExtraxions.Where(v => AMView.Any(am => am.ArqueoProductoID.Equals(v.ArqueoProductoID))); //)(AMView.Select(am => am.ArqueoProductoID).Contains(v.ArqueoProductoID)));

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPEDist = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Distribucion Extraxion
        //                xrTableRowAPEDist.Weight = 1D;
        //                xrTableRowAPEDist.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPEDist.Cells.Add(new Parametros.MyXRTableCell("DISTRIBUCIÓN DE EXTRACCIÓN", xrTableAPProd.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowAPTotExtraxion = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowAPTotExtraxion.Weight = 1D;
        //                xrTableRowAPTotExtraxion.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowAPTotExtraxion.Cells.Add(new Parametros.MyXRTableCell("Total", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< RESUMEN >>>

        //                //Tabla Pagos (Lado Drecho del arqueo)
        //                DevExpress.XtraReports.UI.XRTable xrTableAPPago = new DevExpress.XtraReports.UI.XRTable();
        //                xrTableAPPago.Font = new System.Drawing.Font("Tahoma", 8F);
        //                xrTableAPPago.LocationFloat = new DevExpress.Utils.PointFloat(xrTableAPProd.WidthF + 10f, 10f);
        //                xrTableAPPago.SizeF = new System.Drawing.SizeF((100f + (AMView.GroupBy(ap => ap.Codigo).Distinct().Count() * 90)), 50F);

        //                //Fila Titulo del Reseumen
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowResumen = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowResumen.Weight = 1D;
        //                xrTableRowResumen.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowResumen.Cells.Add(new Parametros.MyXRTableCell("RESUMEN", xrTableAPProd.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                xrTableAPPago.Rows.Add(xrTableRowResumen);

        //                //Fila Valor total de la Venta
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowValorVentas = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowValorVentas.Weight = 1D;
        //                xrTableRowValorVentas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowValorVentas.Cells.Add(new Parametros.MyXRTableCell("Valor Total Venta", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                xrTableRowValorVentas.Cells.Add(new Parametros.MyXRTableCell(_ValorTotalVenta.ToString("#,0.00"), 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

        //                //Fila Datos de la Venta
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDatosVentasTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowDatosVentasTitulo.Weight = 1D;
        //                xrTableRowDatosVentasTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowDatosVentasTitulo.Cells.Add(new Parametros.MyXRTableCell("DATOS DE LA VENTA", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));


        //                #endregion

        //                #region <<< FORMASPAGO >>>

        //                var AFPView = dbView.VistaArqueoFormaPagos.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));//.Equals(AIView.ArqueoIslaID));
        //                //Fila Formas de Pago
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowFormasPago = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowFormasPago.Weight = 1D;
        //                xrTableRowFormasPago.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowFormasPago.Cells.Add(new Parametros.MyXRTableCell("FORMAS DE PAGO", xrTableAPProd.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                //Fila Titulo Formas De Pago
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowFormasPagoTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowFormasPagoTitulo.Weight = 1D;
        //                xrTableRowFormasPagoTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowFormasPagoTitulo.Cells.Add(new Parametros.MyXRTableCell("MENOS", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                xrTableRowFormasPagoTitulo.Cells.Add(new Parametros.MyXRTableCell("VALOR", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                xrTableAPPago.Rows.Add(xrTableRowValorVentas);
        //                xrTableAPPago.Rows.Add(xrTableRowFormasPago);
        //                xrTableAPPago.Rows.Add(xrTableRowFormasPagoTitulo);

        //                //Totales Formas De Pago
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowFormasPagoTotal = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowFormasPagoTotal.Weight = 1D;
        //                xrTableRowFormasPagoTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowFormasPagoTotal.Cells.Add(new Parametros.MyXRTableCell("Total", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                xrTableRowFormasPagoTotal.Cells.Add(new Parametros.MyXRTableCell(_TotalFormasPago.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));


        //                #endregion

        //                #region <<< DESCUENTO >>>

        //                //Titulo Descuento
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowDescuentoTitulo.Weight = 1D;
        //                xrTableRowDescuentoTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowDescuentoTitulo.Cells.Add(new Parametros.MyXRTableCell("DESCUENTO", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                //Titulo Producto Descuento
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProdTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowDescuentoProdTitulo.Weight = 1D;
        //                xrTableRowDescuentoProdTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                xrTableRowDescuentoProdTitulo.Cells.Add(new Parametros.MyXRTableCell("PRODUCTOS", xrTableAPPago.WidthF - 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                //Productos Descuento
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDescuentoProd = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowDescuentoProd.Weight = 1D;
        //                xrTableRowDescuentoProd.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowDescuentoProd.Cells.Add(new Parametros.MyXRTableCell(" ", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                //Linea Descuentos Totales
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDesc = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowTotalDesc.Weight = 1D;
        //                xrTableRowTotalDesc.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotalDesc.Cells.Add(new Parametros.MyXRTableCell("Descuentos Totales", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                //Liena Total Descuento
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalDescuentos = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowTotalDescuentos.Weight = 1D;
        //                xrTableRowTotalDescuentos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTotalDescuentos.Cells.Add(new Parametros.MyXRTableCell("Total Descuento", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                #endregion

        //                #region <<< TANQUES >>>

        //                //Tabla Tanques
        //                DevExpress.XtraReports.UI.XRTable xrTableTanques = new DevExpress.XtraReports.UI.XRTable();
        //                xrTableTanques.Font = new System.Drawing.Font("Tahoma", 8F);
        //                xrTableTanques.LocationFloat = new DevExpress.Utils.PointFloat(0f, 5f);

        //                //Filas
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTanTitulo = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowTanExtraxion = new DevExpress.XtraReports.UI.XRTableRow();

        //                //Fila Titulo
        //                xrTableRowTanTitulo.Weight = 1D;
        //                xrTableRowTanTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell("TANQUES", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
                        

        //                //Fila Extraxion 
        //                xrTableRowTanExtraxion.Weight = 1D;
        //                xrTableRowTanExtraxion.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell("EXTRACCIÓN", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));


        //                #endregion

        //                #endregion

        //                #region <<< LadosMangueras >>>

        //                foreach (var lado in AMView.GroupBy(l => l.Lado).Distinct())
        //                {
        //                    float ancho = lado.Count() * 90;
        //                    xrTableRowAPLado.Cells.Add(new Parametros.MyXRTableCell("LADO " + lado.Key + " Punto de Llenado " + llenado.ToString(), ancho, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                    llenado++;

        //                    if (TA.Equals(Parametros.TiposArqueo.Isla))
        //                    {
        //                        foreach (var disp in AMView.Where(d => d.Lado.Equals(lado.Key)).GroupBy(g => new { g.DispensadorNombre, g.Lado }).Distinct())
        //                        {
        //                            float anchoDisp = disp.Count() * 90;
        //                            xrTableRowAPDisp.Cells.Add(new Parametros.MyXRTableCell("Dispensador: " + disp.Key.DispensadorNombre, anchoDisp, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                            AMView.Where(p => p.DispensadorNombre.Equals(disp.Key.DispensadorNombre) && p.Lado.Equals(disp.Key.Lado)).ToList().ForEach(prod =>
        //                            {
        //                                if (prod.EsLecturaMecanica)
        //                                {
        //                                    xrTableRowAPInicialMec.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaMecanicaInicial.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPFinalMec.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaMecanicaFinal.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPExtraxionMec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(prod.ExtraxionMecanica).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                }

        //                                if (prod.EsLecturaEfectivo)
        //                                {
        //                                    xrTableRowAPInicialEfe.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaEfectivoInicial.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPFinalEfe.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaEfectivoFinal.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPExtraxionEfe.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(prod.ExtraxionEfectivo).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                }

        //                                if (prod.EsLecturaElectronica)
        //                                {
        //                                    xrTableRowAPInicialElec.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaElectronicaInicial.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPFinalElec.Cells.Add(new Parametros.MyXRTableCell(prod.LecturaElectronicaFinal.ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                    xrTableRowAPExtraxionElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(prod.ExtraxionElectronica).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                }

        //                                xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));
        //                                //Datos Verificacion
        //                                xrTableRowAPEfectivo.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(prod.VerificacionEfectivo).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPMecVsElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(prod.VerificacionMecElectronica).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));

        //                            });
        //                        }
        //                    }
        //                    else if (TA.Equals(Parametros.TiposArqueo.Turno) || TA.Equals(Parametros.TiposArqueo.ResumenDia))
        //                    {
        //                        AMView.GroupBy(ap => new { ap.ProductoID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
        //                        {
        //                            xrTableRowAPProd.Cells.Add(new Parametros.MyXRTableCell(prod.ProductoNombre + " (" + prod.MangueraNumero + ")", 90, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(prod.Color), Color.White));

        //                            var APTot = AMView.Where(am => am.ProductoID.Equals(prod.ProductoID) && am.Lado.Equals(lado.Key));

        //                            if (prod.EsLecturaElectronica)
        //                            {
        //                                xrTableRowAPInicialMec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaMecanicaInicial)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPFinalMec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaMecanicaFinal)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPExtraxionMec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }

        //                            if (prod.EsLecturaEfectivo)
        //                            {
        //                                xrTableRowAPInicialEfe.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaEfectivoInicial)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPFinalEfe.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaEfectivoFinal)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPExtraxionEfe.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.ExtraxionEfectivo)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }

        //                            if (prod.EsLecturaElectronica)
        //                            {
        //                                xrTableRowAPInicialElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaElectronicaInicial)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPFinalElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.LecturaElectronicaFinal)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                xrTableRowAPExtraxionElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }

        //                            xrTableRowAPEfectivo.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.VerificacionEfectivo)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            xrTableRowAPMecVsElec.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.VerificacionMecElectronica)).ToString("#,0.000"), 90, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                        });
        //                    }
        //                }

        //                #endregion

        //                #region <<< LECTURASCELDAS >>>

        //                rep.GroupHeaderArqueo.Controls.Add(xrTableAP);

        //                xrTableAP.Rows.Add(xrTableRowAPLado);
        //                if (TA.Equals(Parametros.TiposArqueo.Isla))
        //                    xrTableAP.Rows.Add(xrTableRowAPDisp);

        //                xrTableAP.Rows.Add(xrTableRowAPProd);

        //                //FILAS MECANICAS
        //                xrTableAP.Rows.Add(xrTableRowAPTituloMec);
        //                xrTableAP.Rows.Add(xrTableRowAPInicialMec);
        //                xrTableAP.Rows.Add(xrTableRowAPFinalMec);
        //                xrTableAP.Rows.Add(xrTableRowAPExtraxionMec);

        //                if (AMView.Any(am => am.EsLecturaEfectivo))
        //                {
        //                    //FILAS EFECTIVO
        //                    xrTableAP.Rows.Add(xrTableRowAPTituloEfe);
        //                    xrTableAP.Rows.Add(xrTableRowAPInicialEfe);
        //                    xrTableAP.Rows.Add(xrTableRowAPFinalEfe);
        //                    xrTableAP.Rows.Add(xrTableRowAPExtraxionEfe);
        //                }

        //                if (AMView.Any(am => am.EsLecturaElectronica))
        //                {
        //                    //FILAS ELECTRONICO
        //                    xrTableAP.Rows.Add(xrTableRowAPTituloElec);
        //                    xrTableAP.Rows.Add(xrTableRowAPInicialElec);
        //                    xrTableAP.Rows.Add(xrTableRowAPFinalElec);
        //                    xrTableAP.Rows.Add(xrTableRowAPExtraxionElec);
        //                }

        //                //FILAS VERIFICACION
        //                xrTableAP.Rows.Add(xrTableRowAPTitulo);
        //                xrTableAP.Rows.Add(xrTableRowAPEfectivo);
        //                xrTableAP.Rows.Add(xrTableRowAPMecVsElec);

        //                #endregion

        //                #region <<< EXTRAXIONPAGOSCELDAS >>>

        //                var prodAMView = AMView.GroupBy(ap => new { ap.EsDiferenciado, ap.ProductoID }).Distinct().OrderBy(o => o.Key.EsDiferenciado).ThenBy(o => o.Key.ProductoID).ToList();
        //                bool setTitle = true;

        //                foreach (var item in ExtraxionPago.AsParallel())
        //                {
        //                    if (!item.EsPago)
        //                    {
        //                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAPEDistValor = new DevExpress.XtraReports.UI.XRTableRow();
        //                        xrTableRowAPEDistValor.Weight = 1D;
        //                        xrTableRowAPEDistValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                        xrTableRowAPEDistValor.Cells.Add(new Parametros.MyXRTableCell(item.Nombre, 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                        prodAMView.ForEach(prod =>
        //                        {
        //                            if (setTitle)
        //                            {
        //                                var APTot = AMView.Where(am => am.ProductoID.Equals(prod.Key.ProductoID) && am.EsDiferenciado.Equals(prod.Key.EsDiferenciado));

        //                                xrTableRowTotProd.Cells.Add(new Parametros.MyXRTableCell(APTot.First().ProductoNombre, 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(APTot.First().Color), Color.White));

        //                                if (APTot.Count(c => c.EsLecturaElectronica) > 0)
        //                                    xrTableRowTotCant.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.ExtraxionElectronica)).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                                else
        //                                    xrTableRowTotCant.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(APTot.Sum(s => s.ExtraxionMecanica)).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

        //                                var total = APEView.Where(v => v.ProductoID.Equals(prod.Key.ProductoID) && v.EsDiferenciado.Equals(prod.Key.EsDiferenciado));
        //                                xrTableRowAPTotExtraxion.Cells.Add(new Parametros.MyXRTableCell(total.Count() > 0 ? Convert.ToDecimal(total.Sum(s => s.Valor)).ToString("#,0.000") : "0", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

        //                            }

        //                            var valor = APEView.Where(v => v.ProductoID.Equals(prod.Key.ProductoID) && v.ExtracionID.Equals(item.ID) && v.EsDiferenciado.Equals(prod.Key.EsDiferenciado));
        //                            if (valor.Count() > 0)
        //                                xrTableRowAPEDistValor.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(valor.Sum(s => s.Valor)).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            else
        //                                xrTableRowAPEDistValor.Cells.Add(new Parametros.MyXRTableCell("-", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                        });

        //                        if (setTitle)
        //                        {
        //                            xrTableAPProd.Rows.Add(xrTableRowTotProd);
        //                            xrTableAPProd.Rows.Add(xrTableRowTotCant);
        //                            xrTableAPProd.Rows.Add(xrTableRowAPEDist);
        //                            setTitle = false;
        //                        }

        //                        xrTableAPProd.Rows.Add(xrTableRowAPEDistValor);
        //                    }
        //                    else if (item.EsPago && !item.EsEfectivo)
        //                    {
        //                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAPEPagoValor = new DevExpress.XtraReports.UI.XRTableRow();
        //                        xrTableRowAPEPagoValor.Weight = 1D;
        //                        xrTableRowAPEPagoValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                        xrTableRowAPEPagoValor.Cells.Add(new Parametros.MyXRTableCell(item.Nombre, 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                        var valor = AFPView.Where(v => v.PagoID.Equals(item.ID));
        //                        if (valor.Count() > 0)
        //                        {
        //                            _TotalFormasPago += Convert.ToDecimal(valor.Sum(s => s.Valor));
        //                            xrTableRowAPEPagoValor.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(valor.Sum(s => s.Valor)).ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                        }
        //                        else
        //                            xrTableRowAPEPagoValor.Cells.Add(new Parametros.MyXRTableCell("-", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                        xrTableAPPago.Rows.Add(xrTableRowAPEPagoValor);

        //                    }
        //                }
        //                xrTableRowFormasPagoTotal.Cells[1].Text = _TotalFormasPago.ToString("#,0.00");

        //                xrTableAPPago.Rows.Add(xrTableRowFormasPagoTotal);

        //                xrTableAPProd.Rows.Add(xrTableRowAPTotExtraxion);

        //                rep.Detail.Controls.Add(xrTableAPProd);

        //                #endregion

        //                #region <<< DatosVentaCeldas >>>
        //                //FILADATOSDELAVENTA
        //                xrTableAPProd.Rows.Add(xrTableRowDatosVentasTitulo);
        //                foreach (Parametros.DatosVentas val in Enum.GetValues(typeof(Parametros.DatosVentas)).AsParallel())
        //                {
        //                    DevExpress.XtraReports.UI.XRTableRow xrTableRowAPEDatosVentas = new DevExpress.XtraReports.UI.XRTableRow();
        //                    xrTableRowAPEDatosVentas.Weight = 1D;
        //                    xrTableRowAPEDatosVentas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                    xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell(Parametros.General.GetEnumDatosVentas(val), 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                    decimal RestaExtraxion = 0m;
        //                    decimal TotExtraxion = 0m;

        //                    //foreach (var prod in 
        //                    AMView.GroupBy(ap => new { ap.EsDiferenciado, ap.Precio, ap.ProductoID }).Distinct().OrderBy(o => o.Key.EsDiferenciado).ThenBy(o => o.Key.ProductoID).ToList().ForEach(prod =>
        //                    {
        //                        var vista = AMView.Where(v => v.ProductoID.Equals(prod.Key.ProductoID) && v.EsDiferenciado.Equals(prod.Key.EsDiferenciado));

        //                        if (vista.Count() > 0)
        //                        {
        //                            if (val.Equals(Parametros.DatosVentas.ExtraccionVentas))
        //                            {
        //                                TotExtraxion = vista.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(vista.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(vista.Sum(s => s.ExtraxionMecanica));
        //                                RestaExtraxion = APEView.Count(v => v.ProductoID.Equals(prod.Key.ProductoID)) > 0 ? Convert.ToDecimal(APEView.Where(v => v.ProductoID.Equals(prod.Key.ProductoID)).Sum(s => s.Valor)) : 0m;
        //                                xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell(Decimal.Round(TotExtraxion - RestaExtraxion, 3, MidpointRounding.AwayFromZero).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                            else if (val.Equals(Parametros.DatosVentas.PreciosVentas))
        //                                xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell(Decimal.Round(prod.Key.Precio, 2, MidpointRounding.AwayFromZero).ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            else if (val.Equals(Parametros.DatosVentas.Descuento))
        //                            {
        //                                decimal Descuento = Convert.ToDecimal(vista.Max(v => v.MangueraDescuento));
        //                                xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell(Descuento.ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                            else if (val.Equals(Parametros.DatosVentas.ValorVenta))
        //                            {
        //                                TotExtraxion = vista.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(vista.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(vista.Sum(s => s.ExtraxionMecanica));
        //                                RestaExtraxion = APEView.Count(v => v.ProductoID.Equals(prod.Key.ProductoID)) > 0 ? Convert.ToDecimal(APEView.Where(v => v.ProductoID.Equals(prod.Key.ProductoID)).Sum(s => s.Valor)) : 0m;
        //                                xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell(Decimal.Round((TotExtraxion - RestaExtraxion) * prod.Key.Precio, 2, MidpointRounding.AwayFromZero).ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                                _ValorTotalVenta += Decimal.Round((TotExtraxion - RestaExtraxion) * prod.Key.Precio, 2, MidpointRounding.AwayFromZero);
        //                            }
        //                            else
        //                                xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell("-", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));

        //                        }
        //                        else
        //                        {
        //                            xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell("-", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                            xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell("-", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                            xrTableRowAPEDatosVentas.Cells.Add(new Parametros.MyXRTableCell("-", 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                        }
        //                    });
        //                    xrTableAPProd.Rows.Add(xrTableRowAPEDatosVentas);
        //                }
        //                xrTableRowValorVentas.Cells[1].Text = _ValorTotalVenta.ToString("#,0.00");


        //                #endregion

        //                #region <<< DescuentoCeldas >>>


        //                xrTableAPPago.Rows.Add(xrTableRowDescuentoTitulo);
        //                xrTableAPPago.Rows.Add(xrTableRowDescuentoProdTitulo);

        //                bool SetProductDiscount = true;
        //                foreach (Parametros.TiposDescuento val in Enum.GetValues(typeof(Parametros.TiposDescuento)).AsParallel())
        //                {
        //                    //Linea Detalle Descuento
        //                    DevExpress.XtraReports.UI.XRTableRow xrTableRowDetalleDescuento = new DevExpress.XtraReports.UI.XRTableRow();
        //                    xrTableRowDetalleDescuento.Weight = 1D;
        //                    xrTableRowDetalleDescuento.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                    xrTableRowDetalleDescuento.Cells.Add(new Parametros.MyXRTableCell(Parametros.General.GetEnumTiposDescuento(val), 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));

        //                    AMView.GroupBy(ap => new { ap.ProductoID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
        //                    {
        //                        var vista = AMView.Where(v => v.ProductoID.Equals(prod.ProductoID));

        //                        if (vista.Count() > 0)
        //                        {
        //                            if (SetProductDiscount)
        //                            {
        //                                xrTableRowDescuentoProd.Cells.Add(new Parametros.MyXRTableCell(vista.First().ProductoNombre, 90F, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(vista.First().Color), Color.White));
        //                            }


        //                            if (val.Equals(Parametros.TiposDescuento.Isla))
        //                            {
        //                                decimal Desc = 0m;
        //                                //bool EsAmbosLados = true;
        //                                decimal NonDesc = 0m;

        //                                List<string> dispensadores = new List<string>();

        //                                vista.ToList().ForEach(objMang =>
        //                                {
        //                                    if (!objMang.MangueraDescuento.Equals(0))
        //                                    {
        //                                        decimal extraxion = Convert.ToDecimal(objMang.EsLecturaElectronica ? objMang.ExtraxionElectronica : objMang.ExtraxionMecanica);
        //                                        Desc += Decimal.Round(Convert.ToDecimal(extraxion * objMang.MangueraDescuento), 2, MidpointRounding.AwayFromZero);
        //                                        NonDesc = objMang.MangueraDescuento;

        //                                        if (dispensadores.Contains(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString()))
        //                                        {
        //                                            var vistaAPE = APEView.Where(v => v.ProductoID.Equals(prod.ProductoID) && v.ArqueoProductoID.Equals(objMang.ArqueoProductoID));

        //                                            if (vistaAPE.Count() > 0)
        //                                                Desc -= Decimal.Round(Convert.ToDecimal(vistaAPE.Sum(s => s.Valor)) * NonDesc, 2, MidpointRounding.AwayFromZero);
        //                                        }
        //                                        dispensadores.Add(objMang.DispensadorNombre + objMang.ArqueoProductoID.ToString());
        //                                    }
        //                                });

        //                                _GrandTotalDesucento += Desc;
        //                                xrTableRowDetalleDescuento.Cells.Add(new Parametros.MyXRTableCell(Desc.ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                            else if (val.Equals(Parametros.TiposDescuento.Especial))
        //                            {
        //                                decimal ValorDE = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoEspecial)), 2, MidpointRounding.AwayFromZero);
        //                                _GrandTotalDesucento += ValorDE;
        //                                xrTableRowDetalleDescuento.Cells.Add(new Parametros.MyXRTableCell(ValorDE.ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                            else if (val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
        //                            {
        //                                decimal ValorDGPCF = 0m;

        //                                vista.GroupBy(ap => new { ap.ProductoID, ap.ArqueoIslaID }).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(objProd =>
        //                                {
        //                                    ValorDGPCF += objProd.DescuentoGalonesPetroCardFormula != null ? Decimal.Round(Convert.ToDecimal(Parametros.General.ValorFormula(objProd.DescuentoGalonesPetroCardFormula.ToString())), 2, MidpointRounding.AwayFromZero) : 0m;
        //                                });

        //                                xrTableRowDetalleDescuento.Cells.Add(new Parametros.MyXRTableCell(ValorDGPCF.ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                            else if (val.Equals(Parametros.TiposDescuento.CordobasPetroCard))
        //                            {
        //                                decimal ValorDGDCV = Decimal.Round((vista.GroupBy(g => new { g.ProductoID, g.ArqueoIslaID }).Select(s => s.FirstOrDefault()).Sum(s => s.DescuentoGalonesPetroCardValor)), 2, MidpointRounding.AwayFromZero);
        //                                _GrandTotalDesucento += ValorDGDCV;
        //                                xrTableRowDetalleDescuento.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(ValorDGDCV).ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                                //    xrTableRowTotalDesc.Cells.Add(new Parametros.MyXRTableCell(TotalDescuento.ToString("#,0.00"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Transparent, Color.Black));
        //                            }
        //                        }
        //                    });

        //                    if (SetProductDiscount)
        //                    {
        //                        xrTableAPPago.Rows.Add(xrTableRowDescuentoProd);
        //                        SetProductDiscount = false;
        //                    }

        //                    xrTableAPPago.Rows.Add(xrTableRowDetalleDescuento);
        //                }
        //                xrTableRowTotalDescuentos.Cells.Add(new Parametros.MyXRTableCell(_GrandTotalDesucento.ToString("#,0.00"), 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                xrTableAPPago.Rows.Add(xrTableRowTotalDescuentos);



        //                #endregion

        //                #region <<< VentasContado >>>

        //                //Relleno 1
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowRelleno = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowRelleno.Weight = 1D;
        //                xrTableRowRelleno.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowRelleno.Cells.Add(new Parametros.MyXRTableCell(" ", xrTableAPPago.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                xrTableAPPago.Rows.Add(xrTableRowRelleno);

        //                //Liena Ventas Contado
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowVentasContado = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowVentasContado.Weight = 1D;
        //                xrTableRowVentasContado.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowVentasContado.Cells.Add(new Parametros.MyXRTableCell("Ventas de Contado", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                _VentasContado = Decimal.Round(_ValorTotalVenta - _TotalFormasPago - _GrandTotalDesucento, 2, MidpointRounding.AwayFromZero);
        //                xrTableRowVentasContado.Cells.Add(new Parametros.MyXRTableCell(_VentasContado.ToString("#,0.00"), 100f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                xrTableAPPago.Rows.Add(xrTableRowVentasContado);

        //                #endregion

        //                #region <<< EfectivoRecibido >>>
        //                decimal DiferenciaRecibida = AIView.Sum(v => v.DiferenciaRecibida);
        //                string TextoEfectivoRecibido = DiferenciaRecibida <= 0 ? "Total Efectivo Recibido" : "Sub Total Efectivo Recibido"; 

        //                //Fila EfectivoRecibido
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowEfectivoRecibido = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowEfectivoRecibido.Weight = 1D;
        //                xrTableRowEfectivoRecibido.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                xrTableRowEfectivoRecibido.Cells.Add(new Parametros.MyXRTableCell(TextoEfectivoRecibido, 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                _TotalEfectivoRecibido = 0m;
        //                if (AFPView.Count(v => !v.MonedaID.Equals(0)) > 0)
        //                    _TotalEfectivoRecibido = Decimal.Round(AFPView.Where(v => !v.MonedaID.Equals(0)).Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);

        //                xrTableRowEfectivoRecibido.Cells.Add(new Parametros.MyXRTableCell(_TotalEfectivoRecibido.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                xrTableAPPago.Rows.Add(xrTableRowEfectivoRecibido);

        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDifRecibida = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowConDiferencia = new DevExpress.XtraReports.UI.XRTableRow();
        //                DevExpress.XtraReports.UI.XRTableRow xrTableTotalConDiferencia = new DevExpress.XtraReports.UI.XRTableRow();

        //                if (DiferenciaRecibida <= 0)
        //                {
        //                    //Relleno 2
        //                    DevExpress.XtraReports.UI.XRTableRow xrTableRowRelleno2 = new DevExpress.XtraReports.UI.XRTableRow();
        //                    xrTableRowRelleno2.Weight = 1D;
        //                    xrTableRowRelleno2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                    xrTableRowRelleno2.Cells.Add(new Parametros.MyXRTableCell(" ", xrTableAPPago.WidthF, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                    xrTableAPPago.Rows.Add(xrTableRowRelleno2);
        //                }
        //                else
        //                {
        //                    //Relleno 2
                           
        //                    xrTableRowDifRecibida.Weight = 1D;
        //                    xrTableRowDifRecibida.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                    xrTableRowDifRecibida.Cells.Add(new Parametros.MyXRTableCell("Diferencia Recibida:", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
        //                    xrTableRowDifRecibida.Cells.Add(new Parametros.MyXRTableCell(DiferenciaRecibida.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

        //                    xrTableRowConDiferencia.Weight = 1D;
        //                    xrTableRowConDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

        //                    xrTableTotalConDiferencia.Weight = 1D;
        //                    xrTableTotalConDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                    xrTableTotalConDiferencia.Cells.Add(new Parametros.MyXRTableCell("TOTAL EFECTIVO RECIBIDO:", 100f, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Transparent, Color.Black));
                        
                        
        //                }
        //                #endregion

        //                #region <<< Diferencias >>>
        //                //Diferencias
        //                DevExpress.XtraReports.UI.XRTableRow xrTableRowDiferencias = new DevExpress.XtraReports.UI.XRTableRow();
        //                xrTableRowDiferencias.Weight = 1D;
        //                xrTableRowDiferencias.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
        //                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
        //                _GRANDTOTAL = Decimal.Round(_TotalEfectivoRecibido - _VentasContado, 2, MidpointRounding.AwayFromZero);

        //                if (_GRANDTOTAL > 0)
        //                {
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell("Sobrante en Arqueo:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.DodgerBlue, Color.White));
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell(_GRANDTOTAL.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.DodgerBlue, Color.White));
        //                }
        //                else if (_GRANDTOTAL < 0)
        //                {
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell("Faltante en Arqueo:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Red, Color.White));
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell(_GRANDTOTAL.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
        //                }
        //                else if (_GRANDTOTAL == 0)
        //                {
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell("Sin Diferencias:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LimeGreen, Color.Black));
        //                    xrTableRowDiferencias.Cells.Add(new Parametros.MyXRTableCell(_GRANDTOTAL.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LimeGreen, Color.Black));
        //                }
        //                xrTableAPPago.Rows.Add(xrTableRowDiferencias);

        //                if (DiferenciaRecibida > 0)
        //                {
        //                    xrTableAPPago.Rows.Add(xrTableRowDifRecibida);

        //                    decimal ConDiferencia = DiferenciaRecibida - Math.Abs(_GRANDTOTAL);

        //                    if (ConDiferencia > 0)
        //                    {
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell("Sobrante en Diferencia:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.DodgerBlue, Color.White));
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell(ConDiferencia.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.DodgerBlue, Color.White));
        //                    }
        //                    else if (ConDiferencia < 0)
        //                    {
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell("Faltante en Diferencia:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.Red, Color.White));
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell(ConDiferencia.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
        //                    }
        //                    else if (ConDiferencia == 0)
        //                    {
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell("Sin Diferencias:", 100F, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.LimeGreen, Color.Black));
        //                        xrTableRowConDiferencia.Cells.Add(new Parametros.MyXRTableCell(ConDiferencia.ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LimeGreen, Color.Black));
        //                    }


        //                    xrTableAPPago.Rows.Add(xrTableRowConDiferencia);
        //                    xrTableTotalConDiferencia.Cells.Add(new Parametros.MyXRTableCell((_TotalEfectivoRecibido + DiferenciaRecibida).ToString("#,0.00"), 100F, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
        //                    xrTableAPPago.Rows.Add(xrTableTotalConDiferencia);                            

        //                }
        //                #endregion

        //                rep.Detail.Controls.Add(xrTableAPPago);

        //                #region <<< Tanques >>>

        //                var tanques = from t in AMView
        //                                group t by new {t.TanqueID, t.TanqueNombre, t.Color, t.EsLecturaElectronica} into g
        //                                select new {
        //                                    Nombre = g.Key.TanqueNombre,
        //                                    Color = g.Key.Color,
        //                                    Total = (g.Key.EsLecturaElectronica ? g.Sum(a => a.ExtraxionElectronica) : g.Sum(a => a.ExtraxionMecanica))
        //                                };

        //                xrTableTanques.SizeF = new System.Drawing.SizeF((100f + (tanques.Count() * 90)), 32F);

        //                tanques.ToList().ForEach(lista =>
        //                    {
        //                        xrTableRowTanTitulo.Cells.Add(new Parametros.MyXRTableCell(lista.Nombre, 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.FromArgb(lista.Color), Color.White));
        //                        xrTableRowTanExtraxion.Cells.Add(new Parametros.MyXRTableCell(Convert.ToDecimal(lista.Total).ToString("#,0.000"), 90f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.Transparent, Color.Black));
        //                    });

        //                xrTableTanques.Rows.Add(xrTableRowTanTitulo);
        //                xrTableTanques.Rows.Add(xrTableRowTanExtraxion);

        //                rep.GroupFooterArqueo.Controls.Add(xrTableTanques);

        //                #endregion

        //                #region <<< OBSERVACIONES >>>

        //                if (AIView.Count(ai => ai.Observacion.Length > 0) > 0)
        //                {
        //                    DevExpress.XtraReports.UI.XRLabel xrLabelO = new DevExpress.XtraReports.UI.XRLabel();
        //                    xrLabelO.LocationFloat = new DevExpress.Utils.PointFloat(0F, 40F);
        //                    xrLabelO.Name = "xrLabelO";
        //                    xrLabelO.Multiline = true;
        //                    xrLabelO.SizeF = new System.Drawing.SizeF(820F, 15F);
        //                    xrLabelO.Text = "Observaciones: ";

        //                    AIView.Where(ai => ai.Observacion.Length > 0).ToList().ForEach(nota =>
        //                        { xrLabelO.Text += nota.Observacion.ToString() + "  |  "; });
        //                    rep.GroupFooterArqueo.Controls.Add(xrLabelO);
        //                }
        //                #endregion

        //            }

        //            Parametros.General.splashScreenManagerMain.CloseWaitForm();
        //            if (!_Admin)
        //                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOADMNISTRADOR + Environment.NewLine, Parametros.MsgType.warning);

        //            if (!_Contador)
        //                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOCONTADOR + Environment.NewLine, Parametros.MsgType.warning);

        //            if (ToPrint)
        //            {
        //                rep.CreateDocument();
        //                rep.Print(Parametros.Config.SelectedPrinterLocal);
        //            }
        //            else
        //            {
        //                rep.CreateDocument(true);
        //                printArea.PrintingSystem = rep.PrintingSystem;
        //            }

        //            Formulario.Cursor = Cursors.Default;

        //            rep.RequestParameters = true;

        //        }
        //        else
        //        {
        //            //vaild = false;
        //            Parametros.General.splashScreenManagerMain.CloseWaitForm();
        //            Formulario.Cursor = Cursors.Default;
        //            Parametros.General.DialogMsg("No existe ningún Arqueo de Isla creado.", Parametros.MsgType.warning);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //vaild = false;
        //        Parametros.General.splashScreenManagerMain.CloseWaitForm();
        //        Formulario.Cursor = Cursors.Default;
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, Formulario.Name);
        //    }


        //}


        public static void PrintArqueoEfectivo(Form Formulario, Entidad.SAGASDataViewsDataContext dbView, Entidad.SAGASDataClassesDataContext db,
            Entidad.ArqueoEfectivo AE, Entidad.Turno T, Entidad.ResumenDia RD,
            Parametros.TiposImpresion TI, bool ToPrint, string Descripcion, DevExpress.XtraPrinting.Control.PrintControl printArea)
        {
            try
            {
                Parametros.General.ShowWaitSplash(Formulario, Parametros.WaitFormCaption.GENERANDO.ToString(), Descripcion);

                Formulario.Cursor = Cursors.WaitCursor;
                decimal ThisTipoCambio = RD.TipoCambioMoneda;

                List<Entidad.ArqueoEfectivoDetalle> AEView = new List<Entidad.ArqueoEfectivoDetalle>();

                if (AE != null)
                AEView = (from v in db.ArqueoEfectivoDetalles
                             join ae in db.ArqueoEfectivos on v.ArqueoEfectivoID equals ae.ID
                             where ae.ID.Equals(AE.ID)
                             select v).ToList();
                else
                    AEView = (from v in db.ArqueoEfectivoDetalles
                              join ae in db.ArqueoEfectivos on v.ArqueoEfectivoID equals ae.ID
                              join t in db.Turnos on ae.TurnoID equals t.ID
                              join r in db.ResumenDias on t.ResumenDia equals r
                              where r.ID.Equals(RD.ID)
                              select v).ToList();


                //Datos Iniciales del Reporte
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                Reportes.Arqueos.Hojas.RptEfectivo rep = new Reportes.Arqueos.Hojas.RptEfectivo();
                rv.previewBarTop.Visible = false;
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                int MonedaPrincipal = Parametros.Config.MonedaPrincipal();
                int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
                decimal _TotalAMonedas = 0m;
                decimal _TotalBcordobas = 0m;
                decimal _TotalEfectivoCordobas = 0m;
                decimal _TotalEfectivoDolares = 0m;
                decimal _TotalDolaresEquivalenteCordobas = 0m;
                decimal _GRANDTOTAL = 0m;
                decimal _TotalEfectivoRecibido = 0m;
                decimal _TotalCheques = 0m;
                decimal _Diferencia = 0m;
                //decimal _DescuentoNegativo = 0m;

                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (String.IsNullOrEmpty(dbView.VistaArqueoIslas.Where(vai => vai.ResumenDiaID.Equals(RD.ID)).First().SubEstacionNombre) ?
                    dbView.VistaArqueoIslas.Where(vai => vai.ResumenDiaID.Equals(RD.ID)).First().EstacionNombre :
                    dbView.VistaArqueoIslas.Where(vai => vai.ResumenDiaID.Equals(RD.ID)).First().SubEstacionNombre);
                rep.Watermark.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();

                rep.lblTipoImpresion.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();

                if (TI.Equals(Parametros.TiposImpresion.Re_Impresion) && AE != null)
                {
                    rep.Watermark.Text += " Nro. " + AE.ReImpresionCount.ToString();
                    rep.lblTipoImpresion.Text += " Nro. " + AE.ReImpresionCount.ToString();
                }

                rep.CeFecha.Text = (AE != null ? AE.FechaCreado.ToShortDateString() : "");
                rep.CellPrintDate.Text = db.GetDateServer().ToString();
                rv.Text = "Arqueo de Efectivo " + (T != null ? T.Numero.ToString() : RD.FechaInicial.ToShortDateString());
                int IDFirma = (AE != null ? AE.UsuarioCreado : RD.UsuarioAbiertoID);
                var empleadoAE = db.Empleados.Single(ep => ep.ID.Equals(Convert.ToInt32(db.Usuarios.Single(es => es.ID.Equals(IDFirma)).EmpleadoID)));
                rep.CellFirmaDepositante.Text = empleadoAE.Nombres + " " + empleadoAE.Apellidos;
                rep.CeTitulo.Text = "Arqueo de Efectivo " + (T != null ? T.Numero.ToString() : RD.FechaInicial.ToShortDateString());

                #region <<< TABLA >>>
                DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(110F, 55f);
                xrTableAP.SizeF = new System.Drawing.SizeF(500f, 50F);

                #endregion

                #region A-MONEDAS

                DevExpress.XtraReports.UI.XRTableRow xrTableRowATitulo = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Titulo
                xrTableRowATitulo.Weight = 1D;
                xrTableRowATitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowATitulo.Cells.Add(new Parametros.MyXRTableCell("A - MONEDAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowATitulo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowAColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Columnas
                xrTableRowAColumnas.Weight = 1D;
                xrTableRowAColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableAP.Rows.Add(xrTableRowAColumnas);

                foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                {
                    DevExpress.XtraReports.UI.XRTableRow xrTableRowAValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Valores
                    xrTableRowAValor.Weight = 1D;
                    xrTableRowAValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                    //if (AE != null)
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objA.ID)).ToList();
                    //else
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objA.ID)).ToList();
                    var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objA.ID));
                    if (AEDAnterior.Count() > 0)
                    {
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        _TotalAMonedas += AEDAnterior.Sum(s => s.TotalEfectivo);
                    }
                    else
                    {
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    }
                    xrTableAP.Rows.Add(xrTableRowAValor);

                }

                DevExpress.XtraReports.UI.XRTableRow xrTableRowATotal = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total A-Monedas
                xrTableRowATotal.Weight = 1D;
                xrTableRowATotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Monedas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalAMonedas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowATotal);

                #endregion

                #region B-CORDOBAS

                DevExpress.XtraReports.UI.XRTableRow xrTableRowBTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Titulo
                xrTableRowBTitulo.Weight = 1D;
                xrTableRowBTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowBTitulo.Cells.Add(new Parametros.MyXRTableCell("B - BILLETES CÓRDOBAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowBTitulo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowBColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Columnas
                xrTableRowBColumnas.Weight = 1D;
                xrTableRowBColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableAP.Rows.Add(xrTableRowBColumnas);

                foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                {
                    DevExpress.XtraReports.UI.XRTableRow xrTableRowBValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Valores
                    xrTableRowBValor.Weight = 1D;
                    xrTableRowBValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                   // List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                    //if (AE != null)
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objB.ID)).ToList();
                    //else
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objB.ID)).ToList();
                    var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objB.ID));           
                    if (AEDAnterior.Count() > 0)
                    {
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        _TotalBcordobas += AEDAnterior.Sum( s=> s.TotalEfectivo);
                    }
                    else
                    {
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    }
                    xrTableAP.Rows.Add(xrTableRowBValor);

                }

                DevExpress.XtraReports.UI.XRTableRow xrTableRowBTotal = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total B-Cordobas
                xrTableRowBTotal.Weight = 1D;
                xrTableRowBTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("Total B - Billetes Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalBcordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowBTotal);

                #endregion

                DevExpress.XtraReports.UI.XRTableRow xrTableRowABTotal = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total A-Monedas + B-Cordobas
                xrTableRowABTotal.Weight = 1D;
                xrTableRowABTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo Córdobas A + B ", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                _TotalEfectivoCordobas = _TotalAMonedas + _TotalBcordobas;
                xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowABTotal);

                #region C-DOLARES
                DevExpress.XtraReports.UI.XRTableRow xrTableRowTipoCambio = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Titulo
                xrTableRowTipoCambio.Weight = 1D;
                xrTableRowTipoCambio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("Tipo de Cambio", 200f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell(ThisTipoCambio.ToString("#,0.0000"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Goldenrod, Color.White));
                xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));

                xrTableAP.Rows.Add(xrTableRowTipoCambio);


                DevExpress.XtraReports.UI.XRTableRow xrTableRowCTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Titulo
                xrTableRowCTitulo.Weight = 1D;
                xrTableRowCTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowCTitulo.Cells.Add(new Parametros.MyXRTableCell("C - BILLETES DOLARES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowCTitulo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowCColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                //Fila Columnas
                xrTableRowCColumnas.Weight = 1D;
                xrTableRowCColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                xrTableAP.Rows.Add(xrTableRowCColumnas);

                foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                {
                    DevExpress.XtraReports.UI.XRTableRow xrTableRowCValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Valores
                    xrTableRowCValor.Weight = 1D;
                    xrTableRowCValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                    //if (AE != null)
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objC.ID)).ToList();
                    //else
                    //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objC.ID)).ToList();


                    var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objC.ID));  
                    if (AEDAnterior.Count() > 0)
                    {
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        _TotalEfectivoDolares += AEDAnterior.Sum(s => s.TotalEfectivo);
                    }
                    else
                    {
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    }
                    xrTableAP.Rows.Add(xrTableRowCValor);

                }

                DevExpress.XtraReports.UI.XRTableRow xrTableRowCTotal = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total C-Dolares
                xrTableRowCTotal.Weight = 1D;
                xrTableRowCTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Billetes Dólares", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("$ " + _TotalEfectivoDolares.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowCTotal);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowDolarACordobas = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total Dolares a Cordobas
                xrTableRowDolarACordobas.Weight = 1D;
                xrTableRowDolarACordobas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("Dólares Equivalente a Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                _TotalDolaresEquivalenteCordobas = _TotalEfectivoDolares * ThisTipoCambio;
                xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalDolaresEquivalenteCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowDolarACordobas);

                #endregion

                #region CHEQUES
                if (AEView.Count(ch => ch.EsCheque) > 0)
                {
                    DevExpress.XtraReports.UI.XRTableRow xrTableRowChequeTit = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Titulo
                    xrTableRowChequeTit.Weight = 1D;
                    xrTableRowChequeTit.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowChequeTit.Cells.Add(new Parametros.MyXRTableCell("D - CHEQUES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowChequeTit);

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowDColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Columnas
                    xrTableRowDColumnas.Weight = 1D;
                    xrTableRowDColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Número de Cheque", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Banco", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Monto", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowDColumnas);

                    AEView.Where(c => c.EsCheque).ToList().ForEach(ch =>
                    {
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowChValor = new DevExpress.XtraReports.UI.XRTableRow();
                        //Filas Valores
                        xrTableRowChValor.Weight = 1D;
                        xrTableRowChValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                        xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.NumeroCheque, 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                        xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.Banco, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        _TotalCheques += ch.TotalEfectivo;

                        xrTableAP.Rows.Add(xrTableRowChValor);

                    });

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowChTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total C-Dolares
                    xrTableRowChTotal.Weight = 1D;
                    xrTableRowChTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("Total D - CHEQUES", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalCheques.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowChTotal);

                }
                #endregion

                #region <<< TOTALES >>>

                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalEfectivo = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total Dolares a Cordobas
                xrTableRowTotalEfectivo.Weight = 1D;
                xrTableRowTotalEfectivo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("TOTAL EFECTIVO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                _GRANDTOTAL = _TotalEfectivoCordobas + _TotalDolaresEquivalenteCordobas + _TotalCheques;
                xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _GRANDTOTAL.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowTotalEfectivo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalArqueo = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total Arqueo
                xrTableRowTotalArqueo.Weight = 1D;
                xrTableRowTotalArqueo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("TOTAL ARQUEO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                _TotalEfectivoRecibido = Convert.ToDecimal(dbView.GetTotalEfectivoRecibido(RD.ID, (T != null ? T.ID : 0), 0));
                xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoRecibido.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowTotalArqueo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowDiferencia = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Diferencia
                xrTableRowDiferencia.Weight = 1D;
                xrTableRowDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("Diferencia en Efectivo", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                #endregion

                _Diferencia = _GRANDTOTAL - _TotalEfectivoRecibido;

                if (_Diferencia < 0)
                    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + _Diferencia.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
                else
                    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + _Diferencia.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                xrTableAP.Rows.Add(xrTableRowDiferencia);

                rep.GroupHeaderArqueo.Controls.Add(xrTableAP);

                rep.RequestParameters = true;
                

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;

                if (ToPrint)
                {
                    rep.CreateDocument();
                    rep.Print();
                    rv.Close();
                }
                else
                {
                    rep.CreateDocument(true);
                    printArea.PrintingSystem = rep.PrintingSystem;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, Formulario.Name);
            }
        }


        public static void PrintArqueoEfectivoRetiro(Form Formulario, Entidad.SAGASDataViewsDataContext dbView, Entidad.SAGASDataClassesDataContext db,
         Entidad.Movimiento MV, Parametros.TiposImpresion TI, bool ToPrint, string Descripcion, DevExpress.XtraPrinting.Control.PrintControl printArea)
        {
            try
            {
                Parametros.General.ShowWaitSplash(Formulario, Parametros.WaitFormCaption.GENERANDO.ToString(), Descripcion);

                Formulario.Cursor = Cursors.WaitCursor;
                decimal ThisTipoCambio = MV.TipoCambio;

                Entidad.ArqueoEfectivoRetiro AE = db.ArqueoEfectivoRetiros.Single(s => s.MovimientoID.Equals(MV.ID));

                List<Entidad.ArqueoEfectivoRetiroDetalle> AEView = new List<Entidad.ArqueoEfectivoRetiroDetalle>();

                    AEView = (from v in db.ArqueoEfectivoRetiroDetalles
                              join ae in db.ArqueoEfectivoRetiros on v.ArqueoEfectivoRetiroID equals ae.ID
                              where ae.ID.Equals(AE.ID)
                              select v).ToList();
          


                //Datos Iniciales del Reporte
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                Reportes.Arqueos.Hojas.RptEfectivo rep = new Reportes.Arqueos.Hojas.RptEfectivo();
                rv.previewBarTop.Visible = false;
                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                int MonedaPrincipal = Parametros.Config.MonedaPrincipal();
                int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
                decimal _TotalAMonedas = 0m;
                decimal _TotalBcordobas = 0m;
                decimal _TotalEfectivoCordobas = 0m;
                decimal _TotalEfectivoDolares = 0m;
                decimal _TotalDolaresEquivalenteCordobas = 0m;
                decimal _GRANDTOTAL = 0m;
                decimal _TotalEfectivoRecibido = 0m;
                decimal _TotalCheques = 0m;
                decimal _Diferencia = 0m;
                //decimal _DescuentoNegativo = 0m;

                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = dbView.VistaMovimientos.FirstOrDefault(f => f.ID.Equals(MV.ID)).EstacionNombre;
                //    (vai => vai.ResumenDiaID.Equals(RD.ID)).First().SubEstacionNombre) ?
                //    dbView.VistaArqueoIslas.Where(vai => vai.ResumenDiaID.Equals(RD.ID)).First().EstacionNombre :
                //    dbView.VistaArqueoIslas.Where(vai => vai.ResumenDiaID.Equals(RD.ID)).First().SubEstacionNombre);
                //rep.Watermark.Text = TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();

                rep.lblTipoImpresion.Text = "Original";//TI.Equals(Parametros.TiposImpresion.Original) ? " " : TI.ToString();

                //if (TI.Equals(Parametros.TiposImpresion.Re_Impresion) && AE != null)
                //{
                //    rep.Watermark.Text += " Nro. " + AE.ReImpresionCount.ToString();
                //    rep.lblTipoImpresion.Text += " Nro. " + AE.ReImpresionCount.ToString();
                //}

                rep.CeFecha.Text = MV.Comentario; //(AE != null ? AE.FechaCreado.ToShortDateString() : "");
                rep.CellPrintDate.Text = db.GetDateServer().ToString();
                rv.Text = "Arqueo Entrega de Efectivo " + MV.Numero;
                //int IDFirma = (AE != null ? AE.UsuarioCreado : RD.UsuarioAbiertoID);
                //var empleadoAE = db.Empleados.Single(ep => ep.ID.Equals(Convert.ToInt32(db.Usuarios.Single(es => es.ID.Equals(IDFirma)).EmpleadoID)));
                //rep.CellFirmaDepositante.Text = empleadoAE.Nombres + " " + empleadoAE.Apellidos;
                rep.CeTitulo.Text = "Arqueo Entrega de Efectivo " + MV.Numero + ".    " + (AE != null ? AE.FechaCreado.ToShortDateString() : "");
                rep.xrTableCell12.Text = "Firma del Retiro";

                #region <<< TABLA >>>
                DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(110F, 55f);
                xrTableAP.SizeF = new System.Drawing.SizeF(500f, 50F);

                #endregion

                if (db.Efectivos.Count(ef => ef.MonedaID.Equals(MonedaPrincipal) && AEView.Select(s => s.EfectivoID).Contains(ef.ID)) > 0)
                {
                    #region A-MONEDAS

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowATitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Titulo
                    xrTableRowATitulo.Weight = 1D;
                    xrTableRowATitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowATitulo.Cells.Add(new Parametros.MyXRTableCell("A - MONEDAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowATitulo);

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowAColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Columnas
                    xrTableRowAColumnas.Weight = 1D;
                    xrTableRowAColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowAColumnas);

                    foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                    {
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowAValor = new DevExpress.XtraReports.UI.XRTableRow();
                        //Filas Valores
                        xrTableRowAValor.Weight = 1D;
                        xrTableRowAValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                        //List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                        //if (AE != null)
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objA.ID)).ToList();
                        //else
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objA.ID)).ToList();
                        var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objA.ID));
                        if (AEDAnterior.Count() > 0)
                        {
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            _TotalAMonedas += AEDAnterior.Sum(s => s.TotalEfectivo);
                        }
                        else
                        {
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        }
                        xrTableAP.Rows.Add(xrTableRowAValor);

                    }

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowATotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total A-Monedas
                    xrTableRowATotal.Weight = 1D;
                    xrTableRowATotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Monedas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalAMonedas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowATotal);

                    #endregion

                    #region B-CORDOBAS

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowBTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Titulo
                    xrTableRowBTitulo.Weight = 1D;
                    xrTableRowBTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowBTitulo.Cells.Add(new Parametros.MyXRTableCell("B - BILLETES CÓRDOBAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowBTitulo);

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowBColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Columnas
                    xrTableRowBColumnas.Weight = 1D;
                    xrTableRowBColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowBColumnas);

                    foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                    {
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowBValor = new DevExpress.XtraReports.UI.XRTableRow();
                        //Filas Valores
                        xrTableRowBValor.Weight = 1D;
                        xrTableRowBValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                        // List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                        //if (AE != null)
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objB.ID)).ToList();
                        //else
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objB.ID)).ToList();
                        var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objB.ID));
                        if (AEDAnterior.Count() > 0)
                        {
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            _TotalBcordobas += AEDAnterior.Sum(s => s.TotalEfectivo);
                        }
                        else
                        {
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        }
                        xrTableAP.Rows.Add(xrTableRowBValor);

                    }

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowBTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total B-Cordobas
                    xrTableRowBTotal.Weight = 1D;
                    xrTableRowBTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("Total B - Billetes Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalBcordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowBTotal);

                    #endregion

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowABTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total A-Monedas + B-Cordobas
                    xrTableRowABTotal.Weight = 1D;
                    xrTableRowABTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo Córdobas A + B ", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    _TotalEfectivoCordobas = _TotalAMonedas + _TotalBcordobas;
                    xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowABTotal);
                }

                #region C-DOLARES

                if (db.Efectivos.Count(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true) && AEView.Select(s => s.EfectivoID).Contains(ef.ID)) > 0)
                {
                    DevExpress.XtraReports.UI.XRTableRow xrTableRowTipoCambio = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Titulo
                    xrTableRowTipoCambio.Weight = 1D;
                    xrTableRowTipoCambio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("Tipo de Cambio", 200f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell(ThisTipoCambio.ToString("#,0.0000"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Goldenrod, Color.White));
                    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));

                    xrTableAP.Rows.Add(xrTableRowTipoCambio);


                    DevExpress.XtraReports.UI.XRTableRow xrTableRowCTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Titulo
                    xrTableRowCTitulo.Weight = 1D;
                    xrTableRowCTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowCTitulo.Cells.Add(new Parametros.MyXRTableCell("C - BILLETES DOLARES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowCTitulo);

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowCColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //Fila Columnas
                    xrTableRowCColumnas.Weight = 1D;
                    xrTableRowCColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowCColumnas);

                    foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                    {
                        DevExpress.XtraReports.UI.XRTableRow xrTableRowCValor = new DevExpress.XtraReports.UI.XRTableRow();
                        //Filas Valores
                        xrTableRowCValor.Weight = 1D;
                        xrTableRowCValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                        //List<Entidad.ArqueoEfectivoDetalle> AEDAnterior = new List<Entidad.ArqueoEfectivoDetalle>();

                        //if (AE != null)
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => (AE != null ? x.ArqueoEfectivoID.Equals(AE.ID) : x.ArqueoEfectivoID.Equals(0)) && x.EfectivoID.Equals(objC.ID)).ToList();
                        //else
                        //    AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(0) && x.EfectivoID.Equals(objC.ID)).ToList();


                        var AEDAnterior = AEView.Where(x => x.EfectivoID.Equals(objC.ID));
                        if (AEDAnterior.Count() > 0)
                        {
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.Valor).ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.Sum(s => s.TotalEfectivo).ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            _TotalEfectivoDolares += AEDAnterior.Sum(s => s.TotalEfectivo);
                        }
                        else
                        {
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                        }
                        xrTableAP.Rows.Add(xrTableRowCValor);

                    }

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowCTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total C-Dolares
                    xrTableRowCTotal.Weight = 1D;
                    xrTableRowCTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Billetes Dólares", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("$ " + _TotalEfectivoDolares.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowCTotal);

                    DevExpress.XtraReports.UI.XRTableRow xrTableRowDolarACordobas = new DevExpress.XtraReports.UI.XRTableRow();
                    //Filas Total Dolares a Cordobas
                    xrTableRowDolarACordobas.Weight = 1D;
                    xrTableRowDolarACordobas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("Dólares Equivalente a Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    _TotalDolaresEquivalenteCordobas = _TotalEfectivoDolares * ThisTipoCambio;
                    xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalDolaresEquivalenteCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    xrTableAP.Rows.Add(xrTableRowDolarACordobas);

                }
                #endregion

                #region <<< TOTALES >>>

                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalEfectivo = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total Dolares a Cordobas
                xrTableRowTotalEfectivo.Weight = 1D;
                xrTableRowTotalEfectivo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("TOTAL EFECTIVO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                _GRANDTOTAL = _TotalEfectivoCordobas + _TotalDolaresEquivalenteCordobas + _TotalCheques;
                xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _GRANDTOTAL.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowTotalEfectivo);

                /*
                DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalArqueo = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Total Arqueo
                xrTableRowTotalArqueo.Weight = 1D;
                xrTableRowTotalArqueo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("TOTAL ARQUEO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //_TotalEfectivoRecibido = Convert.ToDecimal(dbView.GetTotalEfectivoRecibido(RD.ID, (T != null ? T.ID : 0), 0));
                xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoRecibido.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                xrTableAP.Rows.Add(xrTableRowTotalArqueo);

                DevExpress.XtraReports.UI.XRTableRow xrTableRowDiferencia = new DevExpress.XtraReports.UI.XRTableRow();
                //Filas Diferencia
                xrTableRowDiferencia.Weight = 1D;
                xrTableRowDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("Diferencia en Efectivo", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                */

                #endregion

                /*
                _Diferencia = _GRANDTOTAL - _TotalEfectivoRecibido;

                if (_Diferencia < 0)
                    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + _Diferencia.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
                else
                    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + _Diferencia.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                xrTableAP.Rows.Add(xrTableRowDiferencia);
                */

                rep.xrTableA.Visible = rep.xrTableB.Visible = true;

                rep.GroupHeaderArqueo.Controls.Add(xrTableAP);
                

                rep.RequestParameters = true;


                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;

                if (ToPrint)
                {
                    rep.CreateDocument();
                    rep.Print();
                    rv.Close();
                }
                else
                {
                    rep.CreateDocument(true);
                    printArea.PrintingSystem = rep.PrintingSystem;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Formulario.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, Formulario.Name);
            }
        }



        #region <<< ParametrosReportes >>>

        //public int GetProductColor()
        //{
        //    try
        //    {
        //        Entidad.SAGASDataClassesDataContext db;
        //        return 
        //    }
        //    catch { return }
        //}

        #endregion

    }

    public class ExtracionesMecanica
    {
        public string Orden { get; set; }
        public decimal LecturaMecanicaInicial { get; set; }
        public decimal LecturaMecanicaFinal { get; set; }
        public decimal ExtraxionMecanica { get; set; }

        public ExtracionesMecanica()
        {

        }

        public ExtracionesMecanica(string _orden, decimal _Inicial, decimal _Final, decimal _Extraxion)
        {
            this.Orden = _orden;
            this.LecturaMecanicaInicial = _Inicial;
            this.LecturaMecanicaFinal = _Final;
            this.ExtraxionMecanica = _Extraxion;
        }

    }

    public class ExtracionesEfectiva

    {
        public string Orden { get; set; }
        public decimal LecturaEfectivoInicial { get; set; }
        public decimal LecturaEfectivoFinal { get; set; }
        public decimal ExtraxionEfectivo { get; set; }

        public ExtracionesEfectiva()
        {

        }

        public ExtracionesEfectiva(string _orden, decimal _Inicial, decimal _Final, decimal _Extraxion)
        {
            this.Orden = _orden;
            this.LecturaEfectivoInicial = _Inicial;
            this.LecturaEfectivoFinal = _Final;
            this.ExtraxionEfectivo = _Extraxion;
        }

    }

    public class ExtracionesElectronica
    {
        public string Orden { get; set; }
        public decimal LecturaElectronicaInicial { get; set; }
        public decimal LecturaElectronicaFinal { get; set; }
        public decimal ExtraxionElectronica { get; set; }

        public ExtracionesElectronica()
        {

        }

        public ExtracionesElectronica(string _orden, decimal _Inicial, decimal _Final, decimal _Extraxion)
        {
            this.Orden = _orden;
            this.LecturaElectronicaInicial = _Inicial;
            this.LecturaElectronicaFinal = _Final;
            this.ExtraxionElectronica = _Extraxion;
        }

    }
}
