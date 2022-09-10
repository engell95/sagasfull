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
using System.Text.RegularExpressions;

namespace SAGAS.Parametros
{
    public class General
    {
        public static int UserID { get; set; }
        public static string UserName { get; set; }
        public static int EstacionServicioID { get; set; }
        public static int SubEstacionID { get; set; }
        public static string EstacionServicioName { get; set; }
        public static int EmpresaID { get; set; }
        public static string Conexion { get; set; }
        public static List<int> ListSES { get; set; }
        public static List<ListColoresTanque> ListColTanques { get; set; }
        public static Entidad.Empresa Empresa { get; set; }
        public static string ReportServerUrl { get; set; }
        public static string ReportPath { get; set; }
        public static string userNameCredential { get; set; }
        public static string passwordCredential { get; set; }
        public static string domainCredential { get; set; }
        public static DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManagerMain;
        public static decimal LitrosGalones = 3.7854m;
        public static decimal LitrosGalonesTresDecimales = 3.785m;
        public const int diasAnyo = 365;
        public const int mesesAnyo = 12;
        public const int diasMes = 30;
        public const decimal valorDiarioVacaciones = 0.0833m;
        public const decimal valorMensualVacaciones = 2.5m;
        public const decimal valorAnualVacaciones = 30m;

        #region <<< PARAMETROS_VENTAS >>>>

        public static int ClienteID { get; set; }
        public static int AreaVentaID { get; set; }
        public static DateTime FechaVenta { get; set; }

        #endregion

        /// <summary>
        /// Obtener la lista de los Colores del los tanques
        /// </summary>
        /// <param name="IDES"></param>
        public static void GetListaColoresTanques()
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                ListColTanques = new List<ListColoresTanque>();
                Parametros.General.ListColTanques.Clear();

                //(from t in db.Tanques.Where(t => t.EstacionServicioID.Equals(IDES))
                // join p in db.Productos on t.ProductoID equals p.ID
                // group t by new {p.ID, p.Codigo, p.Nombre, t.Color})
                (from t in db.Tanques
                 join p in db.Productos on t.ProductoID equals p.ID
                 group t by new { p.ID, p.Codigo, p.Nombre, t.Color }).Distinct().ToList().ForEach(lista =>
                    {
                        ListColTanques.Add(new ListColoresTanque(lista.Key.ID, lista.Key.Codigo, lista.Key.Nombre, lista.Key.Color));
                    }
                );
            }
            catch
            { ListColTanques = new List<ListColoresTanque>(); }
        }

        /// <summary>
        /// Obtener la lista de las SubEstaciones
        /// </summary>
        /// <param name="User"></param>
        /// <param name="IDES"></param>
        public static void GetSubEstaciones(int User, int IDES)
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                ListSES = new List<int>();
                Parametros.General.ListSES.Clear();
                Parametros.General.ListSES.AddRange(db.spListSubEstaciones(User, IDES).Select(s => s.ID));
            }
            catch
            { ListSES = new List<int>(); }
        }

        /// <summary>
        /// Obtener los datos de la empresa
        /// </summary>
        /// <param name="strNombreU">Nombre de la Empresa</param>
        /// <param name="strDireccionU">Dirección de la Empresa</param>
        /// <param name="strTelU">Telefono de la Empresa</param>
        /// <param name="imageLogoEmpresa">Logo / Imagen de la Empresa</param>
        public static void GetCompanyData(out string strNombreU, out string strDireccionU, out string strTelU, out Image imageLogoEmpresa)
        {
            try
            {
                strNombreU = Convert.ToString(Empresa.Nombre);
                strDireccionU = Convert.ToString(Empresa.Direccion);
                strTelU = Convert.ToString(Empresa.Telefono);
                imageLogoEmpresa = BytesToImage(Empresa.imagen);
            }
            catch (Exception ex)
            {
                strNombreU = " ";
                strDireccionU = " ";
                strTelU = " ";
                imageLogoEmpresa = global::SAGAS.Parametros.Properties.Resources.Zanzibar;

                Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }
        }

        /// <summary>
        /// Obtener los datos de la empresa
        /// </summary>
        /// <param name="strNombreU">Nombre de la Empresa</param>
        /// <param name="strDireccionU">Dirección de la Empresa</param>
        /// <param name="strTelU">Telefono de la Empresa</param>
        /// <param name="imageLogoEmpresa">Logo / Imagen de la Empresa</param>
        public static void GetCompanyDataRuc(out string strNombreU, out string strDireccionU, out string strTelU, out string strRUC, out Image imageLogoEmpresa)
        {
            try
            {
                strNombreU = Convert.ToString(Empresa.Nombre);
                strRUC = Convert.ToString(Empresa.NumeroRuc);
                strDireccionU = Convert.ToString(Empresa.Direccion);
                strTelU = Convert.ToString(Empresa.Telefono);
                imageLogoEmpresa = BytesToImage(Empresa.imagen);
            }
            catch (Exception ex)
            {
                strNombreU = " ";
                strDireccionU = " ";
                strTelU = " ";
                strRUC = " ";
                imageLogoEmpresa = global::SAGAS.Parametros.Properties.Resources.Zanzibar;

                Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }
        }


        public static void PrintExportComponet(bool pToPrint
           , string TitleForm
           , System.Windows.Forms.Form mdi
           , DevExpress.XtraPivotGrid.PivotGridControl pivotGridMargin
           , DevExpress.XtraCharts.ChartControl chartControl
           , DevExpress.XtraGrid.GridControl gridcontrol
           , bool landscape, int leftmargin, int rightmargin, int topmargin, int bottommargin, int exporTo)
        {
            try
            {
                #region ::: Declaraciones :::
                DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
                DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink(ps);
                DevExpress.XtraPrinting.PrintableComponentLink pclgrid = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridpiv = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridchar = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PageHeaderFooter phf = (DevExpress.XtraPrinting.PageHeaderFooter)compositeLink.PageHeaderFooter;
                string SaltoLinea = System.Environment.NewLine;
                string param_Empresa = "", param_direccion = "", param_tel = "";
                System.Drawing.Image picture_LogoEmpresa = null;
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                #endregion

                #region ::: Datos de la Empresa :::
                GetCompanyData(out param_Empresa, out param_direccion, out  param_tel, out picture_LogoEmpresa);
                
                //COLOCAR DE FORMA TEMPORAL EL LOGO DE LA EMPRESA
                //picture_LogoEmpresa = global::SAGAS.Parametros.Properties.Resources.Zanzibar;

                string direccionResult = "", direccionTmp = "";
                if (!String.IsNullOrEmpty(param_direccion))
                {
                    string[] direccionSplit = param_direccion.Split(' ');

                    foreach (var i in direccionSplit)
                    {
                        direccionTmp += " " + i;
                        if (direccionTmp.Length >= 75)
                        {
                            direccionResult += SaltoLinea + i;
                            direccionTmp = "";
                        }
                        else direccionResult += " " + i;

                    }
                }

                #endregion

                #region ::: Creando Imagenes de Header y Fooder :::
                System.Windows.Forms.ImageList pict = new System.Windows.Forms.ImageList();
                if (picture_LogoEmpresa != null)
                {
                    pict.Images.Add(picture_LogoEmpresa);
                    pict.ImageSize = new Size(142, 67);


                    compositeLink.Images.Add(pict.Images[0]);
                }

                #endregion

                #region ::: Configuracion del reporte :::
                phf.Header.Content.Clear();
                phf.Header.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                phf.Header.Content.AddRange(new string[] { "[Image 0]", param_Empresa + SaltoLinea + direccionResult + SaltoLinea + param_tel + SaltoLinea + " " + SaltoLinea + TitleForm });
                phf.Footer.Content.AddRange(new string[] { "[Image 1]", "Software de Administración de Gasolinera", "[Page # of Pages #]" });

                pclgrid.Component = gridcontrol;
                pclgridpiv.Component = pivotGridMargin;
                pclgridchar.Component = chartControl;

                compositeLink.Links.Add(pclgrid);
                compositeLink.Links.Add(pclgridpiv);
                compositeLink.Links.Add(pclgridchar);
                compositeLink.CreateDocument();

                ps.PageSettings.Landscape = landscape;
                ps.PageSettings.LeftMargin = leftmargin;
                ps.PageSettings.RightMargin = rightmargin;
                ps.PageSettings.TopMargin = topmargin;
                ps.PageSettings.BottomMargin = bottommargin;
                if (landscape)
                    ps.PageSettings.PaperKind = PaperKind.Legal;

                #endregion

                #region ::: Ejecucion del Reporte :::
                Parametros.Forms.FormReportViewer rForm = new Parametros.Forms.FormReportViewer();
                rForm.Text = TitleForm;
                rForm.MdiParent = mdi;

                rForm.printcontrolAreaReport.PrintingSystem = ps;
                if (pToPrint)
                {
                    rForm.Show();
                }
                else
                {

                    switch (exporTo)
                    {
                        case 1: dglExportToFile.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";
                            break;
                        case 2: dglExportToFile.Filter = "Archivo Formato pdf (*.pdf)|*.pdf";
                            break;
                        //case 3: this.dlgExportToFile.Filter = "Documento HTML (*.htm)|*.htm";
                        //    break;
                    }
                    if (dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dglExportToFile.FileName != "")
                        {
                            switch (exporTo)
                            {
                                case 1:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToXlsx(dglExportToFile.FileName);
                                    break;
                                case 2:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToPdf(dglExportToFile.FileName);
                                    break;
                                //case 3:
                                //    this.dgData.MainView.ExportToHtml(this.dlgExportToFile.FileName);
                                //    break;
                            }
                        }
                    }
                    rForm.Close();
                }


                #endregion
            }
            catch (Exception ex)
            {
                SAGAS.Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }

        }


        public static void PreviewHoliday(bool pToPrint
           , string TitleForm
           , string es
           , System.Windows.Forms.Form mdi
           , DevExpress.XtraPivotGrid.PivotGridControl pivotGridMargin
           , DevExpress.XtraCharts.ChartControl chartControl
           , DevExpress.XtraGrid.GridControl gridcontrol
           , bool landscape, int leftmargin, int rightmargin, int topmargin, int bottommargin, int exporTo)
        {
            try
            {
                #region ::: Declaraciones :::
                DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
                DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink(ps);
                DevExpress.XtraPrinting.PrintableComponentLink pclgrid = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridpiv = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridchar = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PageHeaderFooter phf = (DevExpress.XtraPrinting.PageHeaderFooter)compositeLink.PageHeaderFooter;
                string SaltoLinea = System.Environment.NewLine;
                string param_Empresa = "", param_direccion = "", param_tel = "", param_ruc = "", param_fecha = "Hasta la Fecha del " + FechaVenta.ToShortDateString();
                System.Drawing.Image picture_LogoEmpresa = null;
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                #endregion

                #region ::: Datos de la Empresa :::
                GetCompanyDataRuc(out param_Empresa, out param_direccion, out param_tel, out param_ruc, out picture_LogoEmpresa);
                
                //COLOCAR DE FORMA TEMPORAL EL LOGO DE LA EMPRESA
                //picture_LogoEmpresa = global::SAGAS.Parametros.Properties.Resources.Zanzibar;

                string direccionResult = "", direccionTmp = "";
                if (!String.IsNullOrEmpty(param_direccion))
                {
                    string[] direccionSplit = param_direccion.Split(' ');

                    foreach (var i in direccionSplit)
                    {
                        direccionTmp += " " + i;
                        if (direccionTmp.Length >= 75)
                        {
                            direccionResult += SaltoLinea + i;
                            direccionTmp = "";
                        }
                        else direccionResult += " " + i;

                    }
                }

                #endregion

                #region ::: Creando Imagenes de Header y Fooder :::
                System.Windows.Forms.ImageList pict = new System.Windows.Forms.ImageList();
                if (picture_LogoEmpresa != null)
                {
                    pict.Images.Add(picture_LogoEmpresa);
                    pict.ImageSize = new Size(142, 67);


                    compositeLink.Images.Add(pict.Images[0]);
                }

                #endregion

                #region ::: Configuracion del reporte :::
                phf.Header.Content.Clear();
                phf.Header.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                phf.Header.Content.AddRange(new string[] { "[Image 0]", param_Empresa + SaltoLinea + param_ruc + SaltoLinea + es + SaltoLinea + param_tel + SaltoLinea + " " + SaltoLinea + TitleForm + SaltoLinea + SaltoLinea + param_fecha});
                phf.Footer.Content.AddRange(new string[] { "[Image 1]", "Software de Administración de Gasolinera", "[Page # of Pages #]" });

                pclgrid.Component = gridcontrol;
                pclgridpiv.Component = pivotGridMargin;
                pclgridchar.Component = chartControl;

                compositeLink.Links.Add(pclgrid);
                compositeLink.Links.Add(pclgridpiv);
                compositeLink.Links.Add(pclgridchar);
                compositeLink.CreateDocument();

                ps.PageSettings.Landscape = landscape;
                ps.PageSettings.LeftMargin = leftmargin;
                ps.PageSettings.RightMargin = rightmargin;
                ps.PageSettings.TopMargin = topmargin;
                ps.PageSettings.BottomMargin = bottommargin;
                if (landscape)
                    ps.PageSettings.PaperKind = PaperKind.Legal;

                #endregion

                #region ::: Ejecucion del Reporte :::
                Parametros.Forms.FormReportViewer rForm = new Parametros.Forms.FormReportViewer();
                rForm.Text = TitleForm;
                rForm.MdiParent = mdi;

                rForm.printcontrolAreaReport.PrintingSystem = ps;
                if (pToPrint)
                {
                    rForm.Show();
                }
                else
                {

                    switch (exporTo)
                    {
                        case 1: dglExportToFile.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";
                            break;
                        case 2: dglExportToFile.Filter = "Archivo Formato pdf (*.pdf)|*.pdf";
                            break;
                        //case 3: this.dlgExportToFile.Filter = "Documento HTML (*.htm)|*.htm";
                        //    break;
                    }
                    if (dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dglExportToFile.FileName != "")
                        {
                            switch (exporTo)
                            {
                                case 1:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToXlsx(dglExportToFile.FileName);
                                    break;
                                case 2:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToPdf(dglExportToFile.FileName);
                                    break;
                                //case 3:
                                //    this.dgData.MainView.ExportToHtml(this.dlgExportToFile.FileName);
                                //    break;
                            }
                        }
                    }
                    rForm.Close();
                }


                #endregion
            }
            catch (Exception ex)
            {
                SAGAS.Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }

        }

        public static void PrintExportComponet(bool pToPrint
           , string TitleForm
           , System.Windows.Forms.Form mdi
           , DevExpress.XtraPivotGrid.PivotGridControl pivotGridMargin
           , DevExpress.XtraCharts.ChartControl chartControl
           , DevExpress.XtraTreeList.TreeList tree
           , bool landscape, int leftmargin, int rightmargin, int topmargin, int bottommargin, int exporTo)
        {
            try
            {
                #region ::: Declaraciones :::
                DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
                DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink(ps);
                DevExpress.XtraPrinting.PrintableComponentLink pclgrid = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridpiv = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridchar = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PageHeaderFooter phf = (DevExpress.XtraPrinting.PageHeaderFooter)compositeLink.PageHeaderFooter;
                string SaltoLinea = System.Environment.NewLine;
                string param_Empresa = "", param_direccion = "", param_tel = "";
                System.Drawing.Image picture_LogoEmpresa = null;
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                #endregion

                #region ::: Datos de la Empresa :::
                GetCompanyData(out param_Empresa,
                out param_direccion, out  param_tel, out picture_LogoEmpresa);

                //COLOCAR DE FORMA TEMPORAL EL LOGO DE LA EMPRESA
                //picture_LogoEmpresa = global::SAGAS.Parametros.Properties.Resources.Zanzibar;

                string direccionResult = "", direccionTmp = "";
                if (!String.IsNullOrEmpty(param_direccion))
                {
                    string[] direccionSplit = param_direccion.Split(' ');

                    foreach (var i in direccionSplit)
                    {
                        direccionTmp += " " + i;
                        if (direccionTmp.Length >= 75)
                        {
                            direccionResult += SaltoLinea + i;
                            direccionTmp = "";
                        }
                        else direccionResult += " " + i;

                    }
                }

                #endregion

                #region ::: Creando Imagenes de Header y Fooder :::
                System.Windows.Forms.ImageList pict = new System.Windows.Forms.ImageList();
                if (picture_LogoEmpresa != null)
                {
                    pict.Images.Add(picture_LogoEmpresa);
                    pict.ImageSize = new Size(142, 67);


                    compositeLink.Images.Add(pict.Images[0]);
                }

                #endregion

                #region ::: Configuracion del reporte :::
                phf.Header.Content.Clear();
                phf.Header.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                phf.Header.Content.AddRange(new string[] { "[Image 0]", param_Empresa + SaltoLinea + direccionResult + SaltoLinea + param_tel + SaltoLinea + " " + SaltoLinea + TitleForm });
                phf.Footer.Content.AddRange(new string[] { "[Image 1]", "Software de Administración de Gasolinera", "[Page # of Pages #]" });

                pclgrid.Component = tree;
                pclgridpiv.Component = pivotGridMargin;
                pclgridchar.Component = chartControl;

                compositeLink.Links.Add(pclgrid);
                compositeLink.Links.Add(pclgridpiv);
                compositeLink.Links.Add(pclgridchar);
                compositeLink.CreateDocument();

                ps.PageSettings.Landscape = landscape;
                ps.PageSettings.LeftMargin = leftmargin;
                ps.PageSettings.RightMargin = rightmargin;
                ps.PageSettings.TopMargin = topmargin;
                ps.PageSettings.BottomMargin = bottommargin;


                #endregion

                #region ::: Ejecucion del Reporte :::
                Parametros.Forms.FormReportViewer rForm = new Parametros.Forms.FormReportViewer();
                rForm.Text = TitleForm;
                rForm.MdiParent = mdi;

                rForm.printcontrolAreaReport.PrintingSystem = ps;
                if (pToPrint)
                {
                    rForm.Show();
                }
                else
                {

                    switch (exporTo)
                    {
                        case 1: dglExportToFile.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";
                            break;
                        case 2: dglExportToFile.Filter = "Archivo Formato pdf (*.pdf)|*.pdf";
                            break;
                        //case 3: this.dlgExportToFile.Filter = "Documento HTML (*.htm)|*.htm";
                        //    break;
                    }
                    if (dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dglExportToFile.FileName != "")
                        {
                            switch (exporTo)
                            {
                                case 1:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToXlsx(dglExportToFile.FileName);
                                    break;
                                case 2:
                                    rForm.printcontrolAreaReport.PrintingSystem.ExportToPdf(dglExportToFile.FileName);
                                    break;
                                //case 3:
                                //    this.dgData.MainView.ExportToHtml(this.dlgExportToFile.FileName);
                                //    break;
                            }
                        }
                    }
                    rForm.Close();
                }


                #endregion
            }
            catch (Exception ex)
            {
                SAGAS.Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }

        }
        
        public static void SetFormAcciones(int UserID, DevExpress.XtraBars.BarManager bar, System.Windows.Forms.Form form)
        {
            Entidad.SAGASDataClassesDataContext db = db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            var result = (from acs in db.AccesoSistemas
                          join acc in db.Accesos
                          on acs.AccesoID equals acc.ID
                          where acs.UsuarioID == UserID
                          && acc.Pantalla == form.Name
                          select acs);

            bool agregar = false, modificar = false
                , anular = false, imprimir = false
                , exportar = false;

            foreach (var k in result)
            {
                agregar = k.Agregar;
                modificar = k.Modificar;
                anular = k.Anular;
                imprimir = k.Imprimir;
                exportar = k.Exportar;

            }
            foreach (object ctrl in bar.Items)
            {

                if (ctrl.GetType() == typeof(DevExpress.XtraBars.BarButtonItem))
                {
                    DevExpress.XtraBars.BarButtonItem boton = ctrl as DevExpress.XtraBars.BarButtonItem;
                    switch (boton.Name)
                    {
                        case "btnAgregar":
                            boton.Enabled = agregar;
                            break;
                        case "btnModificar":
                            boton.Enabled = modificar;
                            break;
                        case "btnAnular":
                            boton.Enabled = anular;
                            break;
                        case "barImprimir":
                            boton.Enabled = imprimir;
                            break;
                        case "barExcel":
                            boton.Enabled = exportar;
                            break;
                        case "barPDF":
                            boton.Enabled = exportar;
                            break;
                    }
                }
                
                if (ctrl.GetType() == typeof(DevExpress.XtraBars.BarSubItem))
                {
                    DevExpress.XtraBars.BarSubItem boton = ctrl as DevExpress.XtraBars.BarSubItem;
                    if (boton.Name.Equals("barExportar"))
                        boton.Enabled = exportar;

                }  


            }

        }

        public static bool SystemOptionAcces(int UserID, string Acceso)
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                var result = (from acs in db.AccesoSistemas
                              join acc in db.Accesos
                              on acs.AccesoID equals acc.ID
                              where acs.UsuarioID.Equals(UserID)
                              && acc.Componente.Equals(Acceso)
                              select acs);

                if (result.Count() > 0)
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Promedio de (salarios + Comisiones + Incentivos) de los ultimos 6 meses
        /// </summary>
        /// <param name="ingresoPromedioMensual">Promedio de (salarios + Comisiones + Incentivos) de los ultimos 6 meses</param>
        /// <param name="fechaInicio">Fecha Inicial de Calculo</param>
        /// <param name="fechaFinal">Fecha Final de Calculo</param>
        /// <returns></returns>
        public static decimal CalcularIndemnizacion(decimal ingresoPromedioMensual, DateTime fechaInicio, DateTime fechaFinal)
        {
            decimal cantidadDias = Convert.ToDecimal((fechaFinal - fechaInicio).TotalDays);
            int years = 0, months = 0, days = 0;

            RangoFechasYMD(fechaFinal, fechaInicio, out years, out months, out days);

            if (years == 0)
            {
                if (months == 0)
                    return 0;
                return ((cantidadDias / diasAnyo) * ingresoPromedioMensual);
            }
            if (years > 0 && years < 3)
            {
                return ((cantidadDias / diasAnyo) * ingresoPromedioMensual);
            }
            if (years >= 3)
            {
                decimal acumulado = ingresoPromedioMensual * 3; // ya tiene ganado los 3 salarios
                // calcular los siguientes 3 anyos por los 20 dias
                DateTime fechaCorte = fechaInicio.AddYears(3);
                decimal dias = Convert.ToDecimal((fechaFinal - fechaCorte).TotalDays);
                if (dias > 0)
                {
                    //Realizar un nuevo calculo de los anyos
                    RangoFechasYMD(fechaFinal, fechaCorte, out years, out months, out days);
                    if (years >= 3) //Se ha ganado los otros 2 salarios
                    {
                        acumulado += (ingresoPromedioMensual * 2);
                    }
                    else
                    {
                        acumulado += ((dias / diasAnyo) * ((ingresoPromedioMensual * 20) / 30));
                    }

                }
                return acumulado;
            }
            return 0;
        }

        /// <summary>
        /// Cambia el estado del boton Activar/Desactivar de la ventana de registros(Formularios)
        /// </summary>
        /// <param name="Form">Owner form of the button Activo/Inactivo</param>
        /// <param name="gvData">GridView donde se encuentra el registro activado/desactivado</param>
        /// <param name="fieldName">The fieldname of the column Activo for the focused row</param>
        /// <returns></returns>
        public static void CambiarActivogvData(Parametros.Forms.FormBase Form, DevExpress.XtraGrid.Views.Grid.GridView gvData, string fieldName)
        {
            try
            {
                if (gvData.GetFocusedRowCellValue(fieldName).Equals(false))
                {
                    Form.btnAnular.Caption = "Activar";
                    Form.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Ok20;
                }
                else
                {
                    Form.btnAnular.Caption = "Inactivar";
                    Form.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Anular24;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        public static void CambiarActivoTreeData(Parametros.Forms.FormBase Form, DevExpress.XtraTreeList.TreeList treeData, DevExpress.XtraTreeList.Columns.TreeListColumn colActivo)
        {
            try
            {
                if (treeData.FocusedNode.GetValue(colActivo).Equals(false))
                {
                    Form.btnAnular.Caption = "Activar";
                    Form.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Ok20;
                }
                else
                {
                    Form.btnAnular.Caption = "Inactivar";
                    Form.btnAnular.Glyph = SAGAS.Parametros.Properties.Resources.Anular24;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        /// <summary>
        /// Obtener los dias laborales entre rango de fecha
        /// </summary>
        /// <param name="endDate">Fecha Inicial</param>
        /// <param name="beginDate">Fecha Final</param>
        /// <param name="years">Años</param>
        /// <param name="months">Mes</param>
        /// <param name="days">Días</param>
        /// <param name="add">días a sumarle</param>
        /*public static void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days)
        {
            if (beginDate.Year.Equals(endDate.Year) && beginDate.Month.Equals(endDate.Month))
            {
                int d = endDate.Day - beginDate.Day + 1;
                days = (d >= 31 ? 30 : d);
                years = months = 0;
            }
            else
            {
                if (!beginDate.Month.Equals(2))
                {
                    List<int> mes31 = new List<int>();
                    mes31.Add(1);
                    mes31.Add(3);
                    mes31.Add(5);
                    mes31.Add(7);
                    mes31.Add(8);
                    mes31.Add(10);
                    mes31.Add(12);

                    if (mes31.Contains(beginDate.Date.Month))
                        days = 30 - beginDate.Day;
                    else
                        days = 30 - beginDate.Day;
                }
                else
                    days = Convert.ToInt32(Convert.ToDateTime(new DateTime(beginDate.Year, 03, 01)).AddDays(-1).Day) - beginDate.Day + 1;

                //months = ((endDate.Year - beginDate.Year) * 12) + (((beginDate.Year + beginDate.Month) != (endDate.AddMonths(-1).Year + endDate.AddMonths(-1).Month)) ? (endDate.AddMonths(-1).Month - beginDate.Month) : -12);
                months = ((endDate.AddMonths(-1).Year - beginDate.AddMonths(1).Year) * 12) + ((endDate.AddMonths(-1).Month - beginDate.AddMonths(1).Month) + 1);

                years = months / 12;
                months -= years * 12;

                if (endDate.Month.Equals(2))
                    days += (endDate.Day >= 28 ? 30 : endDate.Day);
                else
                    days += (endDate.Day >= 31 ? 30 : endDate.Day);
            }
            
            //endDate = endDate.Date.AddDays(add);
            //if (endDate < beginDate)
            //{
            //    DateTime d3 = beginDate;
            //    beginDate = endDate;
            //    endDate = d3;
            //}
            //months = 12 * (endDate.Year - beginDate.Year) + (endDate.Month - beginDate.Month);

            //if (endDate.Day < beginDate.Day)
            //{
            //    months--;
            //    days = DateTime.DaysInMonth(beginDate.Year, beginDate.Month) - beginDate.Day + endDate.Day;
            //}
            //else
            //{
            //    if (endDate.Day.Equals(31))
            //        endDate = endDate.Date.AddDays(-1);

            //    days = endDate.Day - beginDate.Day;

            //    if (endDate.Month.Equals(2))
            //        days += 2;

            //}
            //years = months / 12;
            //months -= years * 12;
        }*/

        //public static void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days, int add)
        //{
        //    if (beginDate.Year.Equals(endDate.Year) && beginDate.Month.Equals(endDate.Month))
        //    {
        //        int d = endDate.Day - beginDate.Day + 1;
        //        days = (d >= 31 ? 30 : d);
        //        years = months = 0;
        //    }
        //    else
        //    {
        //        if (!beginDate.Month.Equals(2))
        //        {
        //            List<int> mes31 = new List<int>();
        //            mes31.Add(1);
        //            mes31.Add(3);
        //            mes31.Add(5);
        //            mes31.Add(7);
        //            mes31.Add(8);
        //            mes31.Add(10);
        //            mes31.Add(12);

        //            if (mes31.Contains(beginDate.Date.Month))
        //                days = 31 - beginDate.Day;
        //            else
        //                days = 30 - beginDate.Day;
        //        }
        //        else
        //            days = Convert.ToInt32(Convert.ToDateTime(new DateTime(beginDate.Year, 03, 01)).AddDays(-1).Day) - beginDate.Day + 1;

        //        //months = ((endDate.Year - beginDate.Year) * 12) + (((beginDate.Year + beginDate.Month) != (endDate.AddMonths(-1).Year + endDate.AddMonths(-1).Month)) ? (endDate.AddMonths(-1).Month - beginDate.Month) : -12);
        //        months = ((endDate.AddMonths(-1).Year - beginDate.AddMonths(1).Year) * 12) + ((endDate.AddMonths(-1).Month - beginDate.AddMonths(1).Month) + 1);

        //        years = months / 12;
        //        months -= years * 12;

        //        if (endDate.Month.Equals(2))
        //            days += (endDate.Day >= 28 ? 30 : endDate.Day);
        //        else
        //            days += (endDate.Day >= 31 ? 30 : endDate.Day);

        //        if (days > 30)
        //        {
        //            int tmonths = days / 30;
        //            days -= tmonths * 30;
        //            months += tmonths;
        //        }
        //    }

        //    /*endDate = endDate.Date.AddDays(add);
        //    if (endDate < beginDate)
        //    {
        //        DateTime d3 = beginDate;
        //        beginDate = endDate;
        //        endDate = d3;
        //    }
        //    months = 12 * (endDate.Year - beginDate.Year) + (endDate.Month - beginDate.Month);

        //    if (endDate.Day < beginDate.Day)
        //    {
        //        months--;
        //        days = DateTime.DaysInMonth(beginDate.Year, beginDate.Month) - beginDate.Day + endDate.Day;
        //    }
        //    else
        //    {
        //        if (endDate.Day.Equals(31))
        //            endDate = endDate.Date.AddDays(-1);

        //        days = endDate.Day - beginDate.Day;

        //        if (endDate.Month.Equals(2))
        //            days += 2;

        //    }
        //    years = months / 12;
        //    months -= years * 12;*/
        //}

        
        /// <summary>
        /// Obtener los dias laborales entre rango de fecha
        /// </summary>
        /// <param name="endDate">Fecha Inicial</param>
        /// <param name="beginDate">Fecha Final</param>
        /// <param name="years">Años</param>
        /// <param name="months">Mes</param>
        /// <param name="days">Días</param>
        /// <param name="add">días a sumarle</param>
        //public static void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days)
        //{
        //    if (beginDate.Year.Equals(endDate.Year) && beginDate.Month.Equals(endDate.Month))
        //    {
        //        int d = 0;
        //        if (beginDate.Month.Equals(2))
        //        {
        //            DateTime Febrero = new DateTime(beginDate.Year, 3, 1).AddDays(-1);
        //            if (endDate.Day.Equals(Febrero.Day))
        //                d = 31 - beginDate.Day;
        //            else
        //                d = endDate.Day - beginDate.Day + 1;
        //        }
        //        else
        //            d = endDate.Day - beginDate.Day + 1;

        //        days = (d >= 31 ? 30 : d);
        //        years = months = 0;
        //    }
        //    else
        //    {
        //        //if (!beginDate.Month.Equals(2))
        //            days = 31 - beginDate.Day;
        //        //else
        //        //{
        //        //    if (!beginDate.Day.Equals(1))
        //        //        days = Convert.ToInt32(Convert.ToDateTime(new DateTime(beginDate.Year, 03, 01)).AddDays(-1).Day) - beginDate.Day + 1;
        //        //    else
        //        //        days = 30;
        //        //}
        //        //months = ((endDate.Year - beginDate.Year) * 12) + (((beginDate.Year + beginDate.Month) != (endDate.AddMonths(-1).Year + endDate.AddMonths(-1).Month)) ? (endDate.AddMonths(-1).Month - beginDate.Month) : -12);
        //        months = ((endDate.AddMonths(-1).Year - beginDate.AddMonths(1).Year) * 12) + ((endDate.AddMonths(-1).Month - beginDate.AddMonths(1).Month) + 1);

        //       //Convertir los meses a años y restarlo de los meses
        //        years = months / 12;
        //        months -= years * 12;

        //        //Verificar los ultimos dias de salida
        //        if (endDate.Month.Equals(2))
        //            days += (endDate.Day >= 28 ? 30 : endDate.Day);
        //        else
        //            days += (endDate.Day >= 31 ? 30 : endDate.Day);

        //        //convertir los dias a meses 
        //        int M = days / 30;
        //        months += M;
        //        //borrar los dias que fueron convertidos a meses
        //        days -= M * 30;

        //        //Convertir los meses a años y restarlo de los meses
        //        int Y = months / 12;
        //        years += Y;
        //        months -= Y * 12;
        //    }

        //}
       
         /// <summary>
        /// Obtener los dias laborales entre rango de fecha
        /// </summary>
        /// <param name="endDate">Fecha Inicial</param>
        /// <param name="beginDate">Fecha Final</param>
        /// <param name="years">Años</param>
        /// <param name="months">Mes</param>
        /// <param name="days">Días</param>
        /// <param name="add">días a sumarle</param>
        public static void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days)
        {
            days = 0;
            months = 0;
            years = 0;

            if (beginDate.Year.Equals(endDate.Year) && beginDate.Month.Equals(endDate.Month))
            {
                DateTime final = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);

                if (endDate.Date.Equals(final.Date))
                    days = 31 - beginDate.Day;
                else
                    days = endDate.Day - beginDate.Day + 1;

              if (beginDate.Day.Equals(1))
              {
                  if (endDate.Month.Equals(2))
                      days = (endDate.Day >= 28 ? 30 : endDate.Day);
                  else
                      days = (endDate.Day >= 31 ? 30 : endDate.Day);
              }
            }
            else
            {

            DateTime fecha = beginDate.Date;

            days = 31 - fecha.Day;

            fecha = new DateTime(fecha.AddMonths(1).Year, (fecha.Month.Equals(12) ? 1 : fecha.Month + 1), 01);

                months = ((endDate.AddMonths(-1).Year - fecha.Year) * 12) + ((endDate.AddMonths(-1).Month - fecha.Month) + 1);

                  years = months / 12;
                months -= years * 12;

                if (endDate.Month.Equals(2))
                    days += (endDate.Day >= 28 ? 30 : endDate.Day);
                else
                    days += (endDate.Day >= 31 ? 30 : endDate.Day);

                int oMes = 0;

                oMes = days / 30;
                months += oMes;
                days -= oMes * 30;
            }

        }
        /// <summary>
        /// Corrección de la la fecha para calculo de Indemnización proporcional.
        /// Agregar la variable de configuración IndemnizacionProporcional para calulos proporcinales a empleados con un periodo de tiempo de menos de un añó.
        /// Victor Bonilla, 29/03/2012
        /// </summary>
        /// <param name="ingresoPromedioMensual"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="diasIndem"></param>
        /// <param name="mesesIndem"></param>
        /// <param name="anyosIndem"></param>
        /// <returns></returns>
        public static decimal CalcularIndemnizacion(decimal ingresoPromedioMensual, DateTime fechaInicio, DateTime fechaFinal, out decimal diasIndem, out decimal mesesIndem, out decimal anyosIndem)
        {
            decimal cantidadDias = Convert.ToDecimal((fechaFinal - fechaInicio).TotalDays);
            int years = 0, months = 0, days = 0;
            diasIndem = 0;
            mesesIndem = 0;
            anyosIndem = 0;

            RangoFechasYMD(fechaFinal, fechaInicio, out years, out months, out days);

            if (years >= 0 && years < 3) // Dias y Meses y Anyos
            {
                //if (years < 1 && !Classes.Global.IndemnizacionProporcional) { return 0; }

                diasIndem = Decimal.Round((days * valorDiarioVacaciones), 2);
                mesesIndem = Decimal.Round((months * valorMensualVacaciones), 2);
                anyosIndem = Decimal.Round((years * valorAnualVacaciones), 2);

                return Decimal.Round((diasIndem + mesesIndem + anyosIndem) * (ingresoPromedioMensual / 30m), 2);
            }
            if (years >= 3)
            {
                decimal acumulado = Decimal.Round(ingresoPromedioMensual * 3, 2); // ya tiene ganado los 3 salarios
                decimal anyosMonto = Decimal.Round(acumulado, 2);
                anyosIndem = Decimal.Round((years * valorAnualVacaciones), 2);
                // calcular los siguientes 3 anyos por los 20 dias
                DateTime fechaCorte = fechaInicio.AddYears(3);
                decimal dias = Decimal.Round(Convert.ToDecimal((fechaFinal - fechaCorte).TotalDays), 2);
                if (dias > 0)
                {
                    //Realizar un nuevo calculo de los anyos
                    RangoFechasYMD(fechaFinal, fechaCorte, out years, out months, out days);
                    if (years >= 3) //Se ha ganado los otros 2 salarios
                    {
                        acumulado += Decimal.Round((ingresoPromedioMensual * 2), 2);
                        //anyosIndem += Decimal.Round((years * valorAnualVacaciones), 2);
                    }
                    else
                    {
                        decimal ingresoMensualProporcional = Decimal.Round((ingresoPromedioMensual / 30m) * 20m, 2);

                        //--AÑOS
                        decimal OtrosAnyos = Decimal.Round((years * valorAnualVacaciones) * (ingresoMensualProporcional / 30m), 2);
                        anyosIndem = Decimal.Round(((3 + years) * valorAnualVacaciones), 2);                        

                        //--MESES
                        mesesIndem = Decimal.Round((months * valorMensualVacaciones), 2);
                        decimal mesMonto = Decimal.Round((months * valorMensualVacaciones) * (ingresoMensualProporcional / 30m), 2);

                        decimal diasMonto = 0m;
                        if (!(years.Equals(2) && months.Equals(12)))
                        {
                            //--DIAS
                            diasIndem = Decimal.Round((days * valorDiarioVacaciones), 2);
                            diasMonto = Decimal.Round((days * valorDiarioVacaciones) * (ingresoMensualProporcional / 30m), 2);
                        }
                        
                        //--TOTAL
                        acumulado += Decimal.Round(diasMonto + mesMonto + OtrosAnyos, 2);
                    }

                }
                return Decimal.Round(acumulado, 2);
            }
            return 0;
        }


        #region <<< WAITSPLASHFORM >>>

        public static void ShowWaitSplash(Form Formulario, string Titulo, string Descripcion)
        {
            try
            {
                splashScreenManagerMain = new DevExpress.XtraSplashScreen.SplashScreenManager(Formulario, typeof(global::SAGAS.Parametros.Forms.Dialogs.WaitSplashForm), true, true);
                Parametros.General.splashScreenManagerMain.ShowWaitForm();
                Parametros.General.splashScreenManagerMain.SetWaitFormCaption(Titulo + "...");
                Parametros.General.splashScreenManagerMain.SetWaitFormDescription(Descripcion);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< CONVERSIONES / FORMULAS / KARDEX >>>

        /// <summary>
        /// Metodo que convierte de Byte a Image
        /// </summary>
        /// <param name="img">Valor Imagen</param>
        /// <returns>Type Byte</returns>
        public static System.Drawing.Image BytesToImage(byte[] bytes)
        {
            if (bytes == null) return null;

            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            System.Drawing.Bitmap bm = null;
            try
            {
                bm = new System.Drawing.Bitmap(ms);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bm;
        }

        /// <summary>
        /// Metodo que convierte de Imagen a Byte
        /// </summary>
        /// <param name="img">Valor Imagen</param>
        /// <returns>Type Byte</returns>
        public static byte[] ImageToBytes(System.Drawing.Image img)
        {
            string sTemp = System.IO.Path.GetTempFileName();
            System.IO.FileStream fs = new FileStream(sTemp, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
            img.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
            fs.Position = 0;
            //
            int imgLength = Convert.ToInt32(fs.Length);
            byte[] bytes = new byte[imgLength];
            fs.Read(bytes, 0, imgLength);
            fs.Close();
            return bytes;
        }

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static System.Data.DataTable ToDataTable(SAGAS.Entidad.SAGASDataClassesDataContext db, object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        public static bool EsFormulaCorrecta(string OriginalString)
        {
            try
            {
                char[] delimiters = new char[] { '(', ')', '+', '*', '/', '=', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

                foreach (char letra in OriginalString)
                {
                    if (!delimiters.Contains(letra))
                    {
                        Parametros.General.DialogMsg("La Formula contiene una letra o caracter no valido '" + letra.ToString() + "', por favor revise la fórmula.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                return true;
            }
            catch
            { return false; }
        }

        public static decimal ValorFormula(string OriginalString)
        {
            try
            {

                string formula = "";
                if (OriginalString.StartsWith(@"=") || OriginalString.StartsWith(@"+"))
                    formula = OriginalString.Substring(1);
                else
                    formula = OriginalString;

                decimal Total = 0m;
                if (OriginalString.Contains(@"(") || OriginalString.Contains(@")"))
                {

                    if (OriginalString.Contains(@"(") && OriginalString.Contains(@")") && (OriginalString.Contains(@"/") || OriginalString.Contains(@"*")))
                    {
                        formula = formula.Substring(1);
                        string[] operacion = formula.Split(')');
                        if (operacion.Count() > 2)
                        { Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }
                        else
                        {
                            List<string> result = operacion[0].Split('+').ToList();
                            //Decimal valor = 0m;

                            foreach (var suma in result)
                            {
                                Total += FractionToDouble(suma);
                            }

                            if (operacion[1].StartsWith(@"/"))
                            {
                                return Decimal.Round((Total / Convert.ToDecimal(operacion[1].Substring(1))), 3, MidpointRounding.AwayFromZero);
                            }
                            else if (operacion[1].StartsWith(@"*"))
                            {
                                return Decimal.Round((Total * Convert.ToDecimal(operacion[1].Substring(1))), 3, MidpointRounding.AwayFromZero);
                            }
                            else { Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }

                        }

                    }
                    else
                    { Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }
                }
                else
                {
                    List<string> result = formula.Split('+').ToList();

                    foreach (var suma in result)
                    {
                        Total += FractionToDouble(suma);
                    }

                    return Total;
                }

                return 0.000m;
            }
            catch
            { return 0.000m; }
        }

        public static decimal FractionToDouble(string fraction)
        {
            decimal result;

            if (decimal.TryParse(fraction, out result))
            {
                return result;
            }

            string[] split = fraction.Split(new char[] { '*', '/' });

            if (split.Length == 2)
            {
                decimal a, b;


                if (decimal.TryParse(split[0], out a) && decimal.TryParse(split[1], out b))
                {
                    if (fraction.Contains(@"*"))
                    {
                        return (decimal)a * b;
                    }

                    if (fraction.Contains(@"/"))
                    {
                        return (decimal)a / b;
                    }
                }
            }

            return 0m;
        }

        public static int ValorConsecutivo( string texto)
        {
            try
            {
            string valor = Regex.Match(texto, @"(\d+)").ToString();
            return  Convert.ToInt32(valor) + 1;
            }
            catch {return 0;}
        }

        public static string GetMonthInLetters(int mes)
        {
            string letras = "";
            switch (mes)
            {
                case 1:
                    letras += "Enero ";
                    break;
                case 2:
                    letras += "Febrero ";
                    break;
                case 3:
                    letras += "Marzo ";
                    break;
                case 4:
                    letras += "Abril ";
                    break;
                case 5:
                    letras += "Mayo ";
                    break;
                case 6:
                    letras += "Junio ";
                    break;
                case 7:
                    letras += "Julio ";
                    break;
                case 8:
                    letras += "Agosto ";
                    break;
                case 9:
                    letras += "Septiembre ";
                    break;
                case 10:
                    letras += "Octubre ";
                    break;
                case 11:
                    letras += "Noviembre ";
                    break;
                case 12:
                    letras += "Diciembre ";
                    break;
            }
            return letras;

        }

        public static string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString("00") + "/100";
            }
            else
                dec = " NETOS";

            res = toText(Convert.ToDouble(entero)) + dec;
            return res;
        }

        public static string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }

        #region <<< KARDEX >>>

        public static void LastCost(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDMov, DateTime Fecha, out decimal vCostoFinal, out decimal vCostoEntrada)
        {
            try
            {
                var costo = (from k in db.Kardexes
                             join m in db.Movimientos on k.MovimientoID equals m.ID
                             where k.ProductoID.Equals(IDproducto) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) 
                             && k.Fecha.Date <= Fecha.Date  && !k.MovimientoID.Equals(IDMov) && !k.CostoEntrada.Equals(0)
                             select new { k.ID, k.Fecha, k.CostoFinal, k.CostoEntrada}).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).Take(1);

                vCostoFinal = Decimal.Round((costo.Count() > 0 ? costo.First().CostoFinal : 0m), 4, MidpointRounding.AwayFromZero);
                vCostoEntrada = Decimal.Round((costo.Count() > 0 ? costo.First().CostoEntrada : 0m), 4, MidpointRounding.AwayFromZero);
            }
            catch
            {
                vCostoEntrada = 0m;
                vCostoFinal = 0m;
            }
        }

        public static decimal SaldoKardexCompra(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, DateTime Fecha, bool EsEntrada)
        {
            try
            {
                var obj = from k in db.Kardexes
                          join m in db.Movimientos on k.Movimiento equals m
                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                          where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && !k.EsManejo
                          && !m.Anulado && !mt.EsAnulado
                          //&& (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                          && k.Fecha.Date <= Fecha.Date
                          && (!EsEntrada || !(k.Fecha.Date == Fecha.Date && !mt.Entrada))
                          //&& (k.Fecha.Date > (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) && k.Fecha.Date <= Fecha.Date)
                          select new { k.ID, k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((obj.Count() > 0 ? obj.Sum(s => s.CantidadEntrada) - obj.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        public static decimal SaldoKardex(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha, bool EsEntrada)
        {
            try
            {
                var obj = from k in db.Kardexes
                                  join m in db.Movimientos on k.Movimiento equals m
                                  join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                  where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && !k.EsManejo 
                                  && !m.Anulado && !mt.EsAnulado
                                  && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                                  && k.Fecha.Date <= Fecha.Date
                                  && (!EsEntrada || !(k.Fecha.Date == Fecha.Date && !mt.Entrada))
                                  //&& (k.Fecha.Date > (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) && k.Fecha.Date <= Fecha.Date)
                                  select new { k.ID, k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((obj.Count() > 0 ? obj.Sum(s => s.CantidadEntrada) - obj.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);
                                      
            }
            catch { return 0m; }
        }

        public static decimal SaldoKardexAnulacion(SAGAS.Entidad.SAGASDataClassesDataContext db, int _ID, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha, bool EsEntrada)
        {
            try
            {
                var obj = from k in db.Kardexes
                          join m in db.Movimientos on k.Movimiento equals m
                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                          where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && !k.EsManejo
                          && !m.Anulado && !mt.EsAnulado
                          && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                          && k.Fecha.Date <= Fecha.Date
                          && (!EsEntrada || !(k.Fecha.Date == Fecha.Date && !mt.Entrada)) && !k.ID.Equals(_ID)
                          //&& (k.Fecha.Date > (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) && k.Fecha.Date <= Fecha.Date)
                          select new { k.ID, k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((obj.Count() > 0 ? obj.Sum(s => s.CantidadEntrada) - obj.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        public static void SaldoCostoTotalKardex(SAGAS.Entidad.SAGASDataClassesDataContext db, int _ID, int _IDKD, int IDEstacionServicio, int IDSubEstacion, int IDproducto, DateTime Fecha, out decimal vSaldo)
        {
            try
            {
        var saldo = (from k in db.Kardexes
                     join m in db.Movimientos on k.MovimientoID equals m.ID
                     join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                     where k.ProductoID.Equals(IDproducto) && mt.AplicaKardex && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                    && m.EstacionServicioID.Equals(IDEstacionServicio)
                    && m.SubEstacionID.Equals(IDSubEstacion)
                    && (k.Fecha.Date < Fecha.Date || (k.Fecha.Date == Fecha.Date && mt.Entrada)) && !k.ID.Equals(_ID)
                    //&& (k.Fecha.Date < Fecha.Date || (k.Fecha.Date == Fecha.Date && mt.Entrada && !k.ID.Equals(_ID))) && !k.ID.Equals(_ID)
                     select new { k.CantidadEntrada, k.CantidadSalida, k.CostoFinal, k.CostoTotal, k.Fecha }).OrderByDescending(o => o.Fecha);

                decimal suma = 0;
                saldo.ToList().ForEach(s => suma += ((s.CantidadEntrada >= 0 && s.CantidadSalida <= 0) ? Math.Abs(Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(Decimal.Round(s.CostoTotal, 2, MidpointRounding.AwayFromZero))));
                vSaldo = suma;
            }
            catch { vSaldo = 0m; }
        }
        
        public static decimal SaldoKardexManejo(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha, bool EsEntrada)
        {
            try
            {
                var Manejo = from k in db.Kardexes
                             join m in db.Movimientos on k.MovimientoID equals m.ID
                             join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                             where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion)
                             && k.ProductoID.Equals(IDproducto) && k.EsManejo
                             && !m.Anulado && !mt.EsAnulado
                             && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                             && k.Fecha.Date <= Fecha.Date
                             && (!EsEntrada || !(k.Fecha.Date == Fecha.Date && !mt.Entrada))
                             select new { k.ID, k.CantidadEntrada, k.CantidadSalida };

            return Decimal.Round((Manejo.Count() > 0 ? Manejo.Sum(s => s.CantidadEntrada) - Manejo.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        public static decimal SaldoKardexPost(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha)
        {
            try
            {
                var LastKardex = (from k in db.Kardexes
                             join m in db.Movimientos on k.MovimientoID equals m.ID
                             where k.ProductoID.Equals(IDproducto) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion)
                             && k.Fecha.Date > Fecha.Date
                             select new { k.ID, k.Fecha, k.CostoFinal, k.CostoEntrada }).OrderBy(o => o.Fecha).ThenByDescending(d => d.ID).Take(1);

                DateTime ProximaFehca = Fecha;
                bool Next = true;

                if (LastKardex.Count() > 0)
                {
                    ProximaFehca = LastKardex.First().Fecha;
                    Next = false;
                }

                var obj = from k in db.Kardexes
                          join m in db.Movimientos on k.Movimiento equals m
                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                          where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                          && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                          && (k.Fecha.Date > Fecha.Date && (Next || k.Fecha.Date < ProximaFehca.Date))
                          //&& (k.Fecha.Date > (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) && k.Fecha.Date <= Fecha.Date)
                          select new { k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((obj.Count() > 0 ? obj.Sum(s => s.CantidadEntrada) - obj.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        public static decimal SaldoKardexManejoPost(SAGAS.Entidad.SAGASDataClassesDataContext db, int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha)
        {
            try
            {
                var LastKardex = (from k in db.Kardexes
                                  join m in db.Movimientos on k.MovimientoID equals m.ID
                                  where k.ProductoID.Equals(IDproducto) && m.Anulado.Equals(false) && m.MovimientoTipoID.Equals(18)
                                  && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion)
                                  && k.Fecha.Date > Fecha.Date
                                  select new { k.ID, k.Fecha, k.CostoFinal, k.CostoEntrada }).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).Take(1);

                DateTime ProximaFehca = Fecha;
                bool Next = true;

                if (LastKardex.Count() > 0)
                {
                    ProximaFehca = LastKardex.First().Fecha;
                    Next = false;
                }

                var Manejo = from k in db.Kardexes
                             join m in db.Movimientos on k.Movimiento equals m
                             join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                             where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && k.EsManejo && !m.Anulado && !mt.EsAnulado
                             && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                             && (k.Fecha.Date > Fecha.Date && (Next || k.Fecha.Date < ProximaFehca.Date))
                             select new { k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((Manejo.Count() > 0 ? Manejo.Sum(s => s.CantidadEntrada) - Manejo.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        #endregion

        #endregion

        #region >>> LogBook <<<

        private static string GetMachineName()
        {
            string machineName = String.Empty;
            try
            {
                machineName = Environment.MachineName;
            }
            catch (System.Security.SecurityException)
            {
                machineName = "Acceso denegado";
            }

            return machineName;
        }

        public static void AddLogBook(SAGAS.Entidad.SAGASDataClassesDataContext db,
           TipoAccion tipoActividad, string descripcion, string Acceso)
        {
            try
            {
                Entidad.Audit Log = new Entidad.Audit();
                Log.UsuarioID = UserID;
                Log.EstacionServicioID = EstacionServicioID;
                Log.TipoAccion = tipoActividad.ToString();
                Log.Accion = descripcion;
                Log.Acceso = Acceso;
                Log.Computadora = GetMachineName();
                Log.Date = (DateTime)db.GetDateServer();
                db.Audits.InsertOnSubmit(Log);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Properties.Resources.MSGERROR, SAGAS.Parametros.MsgType.error, ex.Message);
            }
        }

        public static void AddLogBook(SAGAS.Entidad.SAGASDataClassesDataContext db,
           TipoAccion tipoActividad, string descripcion, string Acceso, DataTable EntityPosterior, DataTable EntityAnterior)
        {
              int r = 0;
            foreach (DataRow dRow in EntityAnterior.Rows)
                        {
                            
                            for (int x = 0; x < EntityAnterior.Columns.Count; x++)
                            {
                                if (!String.Equals(dRow[x].ToString(), EntityPosterior.Rows[r][x].ToString()))
                                {
                                 Entidad.Audit Log = new Entidad.Audit();
                                Log.UsuarioID = UserID;
                                Log.EstacionServicioID = EstacionServicioID;
                                Log.TipoAccion = tipoActividad.ToString();
                                Log.Accion = descripcion;
                                Log.Computadora = GetMachineName();
                                Log.Date = (DateTime)db.GetDateServer();
                                Log.Acceso = Acceso;
                                Log.Campo = EntityPosterior.Columns[x].ColumnName;
                                Log.ValorAntesCampo = dRow[x].ToString();
                                Log.ValorDespuesCampo = EntityPosterior.Rows[r][x].ToString();
                                db.Audits.InsertOnSubmit(Log);
                                db.SubmitChanges();
                                }
                            }
                            r++;
                        }
            

        }

        

        #endregion

        #region >>> DIALOGOS DE MENSAJES <<<
        
        /// <summary>
        /// Metodo que muestra los cuadros de dialogos del sistema
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar en el cuadro de dialogo</param>
        /// <param name="tipoMensaje">Tipo de mensaje del cuadro de dialogo</param>
        /// <returns>El cuadro de dialogo</returns>
        public static System.Windows.Forms.DialogResult DialogMsg(string mensaje, MsgType tipoMensaje)
        {
            Forms.Dialogs.DialogMessage dialog = new Forms.Dialogs.DialogMessage(tipoMensaje, mensaje);
            dialog.BringToFront();
            dialog.Activate();
            return dialog.ShowDialog();
        }


        /// <summary>
        /// Metodo que muestra el cuadro de dialogo de error del sistema
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar en el cuadro de dialogo</param>
        /// <param name="tipoMensaje">Tipo de mensaje del cuadro de dialogo</param>
        /// <param name="masInformacion">Mensaje de error del cuadro de dialogo</param>
        /// <returns>El cuadro de dialogo</returns>
        public static System.Windows.Forms.DialogResult DialogMsg(string mensaje, MsgType tipoMensaje, string masInformacion)
        {
            Forms.Dialogs.DialogMessage dialog = new Forms.Dialogs.DialogMessage(tipoMensaje, mensaje, masInformacion);
            dialog.BringToFront();
            dialog.Activate();
            return dialog.ShowDialog();
        }
            
        #endregion
        
        #region <<< DXERRORPROVIDERS >>>

        public static void ValidateEmptyStringRule(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider)
        {
            if (control.EditValue == null || control.EditValue.ToString().Trim().Length == 0)
            {
                dxErrorProvider.SetError(control, "Campo requerido", ErrorType.Warning);
            }
            else
            {
                dxErrorProvider.SetError(control, "", ErrorType.None);
            }
        }

        public static void ValidateDifES(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider)
        {
            if (Convert.ToDecimal(control.EditValue) != EstacionServicioID)
            {
                dxErrorProvider.SetError(control, "La Estación de Servicio es diferente a la predeterminada", ErrorType.Information);
            }
            else
            {
                dxErrorProvider.SetError(control, "", ErrorType.None);
            }
        }

        //public static void ValidateTipoCambio(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider, Entidad.SAGASDataClassesDataContext db)
        //{
        //    if (!db.TipoCambios.Any(tc => tc.Fecha.Equals(Convert.ToDateTime(control.EditValue).Date)))
        //    {
        //        dxErrorProvider.SetError(control, Properties.Resources.MSGNOTIPOCAMBIO, ErrorType.Information);
        //    }
        //    else
        //    {
        //        dxErrorProvider.SetError(control, "", ErrorType.None);
        //    }
        //}

        public static void ValidateError(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider, string texto, bool valor)
        {
            if (valor)
            {
                dxErrorProvider.SetError(control, texto, ErrorType.Critical);
            }
            else
            {
                dxErrorProvider.SetError(control, "", ErrorType.None);
            }
        }

        #endregion 

        public static DateTime GetNextQuincena(DateTime fecha)
        {
            DateTime date = new DateTime();
            try
            {
                int dia = fecha.Day;
                if (dia <= 15)
                    date = new DateTime(fecha.Year, fecha.Month, 15);
                else
                {                                   
                    date = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
                    date = date.AddDays(-1); //Ultimo dia del mes
                }
                return date;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return date;
            }
        }

        public static DateTime GetCurrentQuincena(DateTime fecha)
        {
            int dia = fecha.Day;
            DateTime date = new DateTime();
            if (dia < 15)
                date = new DateTime(fecha.Year, fecha.Month, 1);
            else
            {
                date = new DateTime(fecha.Year, fecha.Month, 16);
            }
            return date;
        }

        #region <<< VALIDACIONES_CONTABLES >>>

        public static int GetNroSerie(int EstacionID, int SubEstacionID, int TipoMovimientoID, Entidad.SAGASDataClassesDataContext db)
        {

            try
            {
                var SerieUso = db.Series.Where(s => s.EstacionServicioID.Equals(EstacionID) && s.SubEstacionID.Equals(SubEstacionID) && s.MovimientoTipoID.Equals(TipoMovimientoID)).ToList();

                if (SerieUso.Count > 0)
                {
                    int number = 0;

                    if (SerieUso.First().NumeroActual.Equals(0))
                        number = SerieUso.First().NumeroInicial;
                    else
                        number = SerieUso.First().NumeroActual + 1;

                    if (!SerieUso.First().NumeroFinal.Equals(0))
                    {
                        if (number >= SerieUso.First().NumeroFinal)
                            return 0;
                        else
                            return number;
                    }
                    else
                        return number;
                }
                else
                    return 0;
            }
            catch { return 0; }

        }

        public static bool ValidatePeriodoContable(DateTime Fecha, Entidad.SAGASDataClassesDataContext db, int IDEstacion)
        {
            try
            {
                if (db.PeriodoContables.Count(p => p.EstacionID.Equals(IDEstacion) && p.FechaInicio.Date <= Fecha.Date) > 0)
                {
                    var query = db.PeriodoContables.FirstOrDefault(p => p.EstacionID.Equals(IDEstacion) && (p.FechaInicio.Date <= Fecha.Date && p.FechaFin.Date >= Fecha.Date));

                    if (query != null)
                    {
                        if (query.Cerrado || query.Consolidado)
                            return false;
                        else
                            return true;
                    }
                    else
                        return true;
                }
                else
                    return false;
            }
            catch { return false; }
        }

        public static bool ValidateKardexMovemente(DateTime Fecha, Entidad.SAGASDataClassesDataContext db, int IDEstacion, int IDSubEstacion, int TipoID, int IDArea)
        {
            try
            {
                var obj = db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacion) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(TipoID) && !m.Anulado && (m.AreaID.Equals(IDArea) || IDArea.Equals(0)) && m.InventarioBloqueado && m.FechaRegistro.Date >= Fecha.Date);

                if (obj.Count() > 0)
                    return false;
                else
                    return true;
            }
            catch { return false; }
        }

        public static bool ValidateTipoCambio(DevExpress.XtraEditors.BaseEdit control, DXErrorProvider dxErrorProvider, Entidad.SAGASDataClassesDataContext db)
        {
            try
            {
                if (!db.TipoCambios.Any(tc => tc.Fecha.Equals(Convert.ToDateTime(control.EditValue).Date)))
                    return false;
                else
                    return true;
            }
            catch { return false; }
        }
        
        #endregion

        #region <<< IMPRESION_ARQUEOS >>>

        #endregion

        #region <<< StringFromEnum >>>

        public static string GetEnumDatosVentas(DatosVentas datos)
        {
            switch (datos)
            {
                case DatosVentas.ExtraccionVentas:
                    return "Extracción / Ventas";
                    break;
                case DatosVentas.PreciosVentas:
                    return "Precio de Venta";
                    break;
                case DatosVentas.Descuento:
                    return "Descuento";
                    break;
                case DatosVentas.ValorVenta:
                    return "Valor de la Venta";
                    break;
                default: return " ";
            }
        }

        public static string GetEnumTiposVerificacion(TiposVerificacion tipo)
        {
            switch (tipo)
            {
                case TiposVerificacion.Efectivo:
                    return "Efectivo";
                    break;
                case TiposVerificacion.MecVsElectronica:
                    return "Mec. Vrs Electronica";
                    break;
                default: return " ";
            }
        }

        public static string GetEnumTiposDescuento(TiposDescuento tipo)
        {
            switch (tipo)
            {
                case TiposDescuento.Isla:
                    return "Isla";
                    break;
                case TiposDescuento.Especial:
                    return "Especial";
                    break;
                case TiposDescuento.GalonesPetroCard:
                    return "PetroCard Gls.";
                    break;
                case TiposDescuento.CordobasPetroCard:
                    return "PetroCard C$";
                    break;
                default: return " ";
            }
        }

        #endregion
    }

    #region >>> ENUMS <<<

    public enum TipoAccion
    {
        InicioSesion,
        FinSesion,
        Error,
        Administrativa,
        Arqueo,
        Contabilidad,
        Nomina,
        Inventario,
        Ventas,
        Tesoreria,
        ActivoFijo

    }

    public enum MsgType
    {
        message = 0,
        question = 1,
        warning = 2,
        error = 3,
        questionNO = 4
    }

    public enum Meses
    {
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }

    public enum Lectura
    {
        Inicial = 0,
        Final = 1,
        Extraccion = 2
    }

    public enum TipoConexion
    {
        Wan = 0,
        Local = 1,
        Admin = 2
    }

    public enum TiposVerificacion
    {
        Efectivo = 0,
        MecVsElectronica = 1
    }

    public enum TiposDescuento
    {
        Isla = 0,
        Especial = 1,
        GalonesPetroCard = 2,
        CordobasPetroCard = 3
    }

    public enum DatosVentas
    {
        ExtraccionVentas = 0,
        PreciosVentas = 1,
        Descuento = 2,
        ValorVenta = 3
    }

    public enum TiposImpresion
    {
        Original = 0,
        Vista_Previa = 1,
        Re_Impresion = 2,
        Modificado = 3
    }

    public enum TiposArqueo
    {
        Isla = 0,
        Turno = 1,
        ResumenDia = 2
    }

    public enum TiposMovimientoInventario
    {
        Entrada = 0,
        Salida = 1,
        Traslado = 2
    }

    public enum TiposMovimientoEmpleadoID
    {
        Salario = 1,
        Vacaciones = 2,
        INSSLaboral = 3,
        IREmpleado = 4,
        ProvIndemnizacion = 5,
        ProvVacaciones = 6,
        ProvAguinaldo = 7,
        HoraExtras = 8,
        INSSPatronal = 9,
        INATEC = 10
    }

    public enum TiposDevConvProveedor
    {
        Devolucion = 0,
        Conversion = 1
    }

    public enum Estados
    {
        Abierto = 0,
        Modificado = 1,
        Cerrado = 2
    }

    public enum TipoComprobanteArqueo
    {
        Ninguno = 0,
        Clientes = 1,
        Manejo = 2,
        AutoConsumo = 3
    }

    public enum EstadosOrdenes
    {
        Abierta = 0,
        Rechazada = 1,
        Enviada = 2,
        Anulada = 3,
        Aprobada = 4,
        Procesada = 5
    }

    public enum TipoDepositos
    {
        VentaContado = 0,
        NotaDebito = 1,
        RecibosOficialesCaja = 2
    }

    public enum TipoPagoManual
    {
        PagoManual = 55,
        LiquidacionAnticipo = 61
    }

    public enum TiposFS
    {
        SinDiferencia = 0,
        Sobrante = 1,
        Faltante = 2
    }

    public enum WaitFormCaption
    {
        GENERANDO = 0,
        GUARDANDO = 1,
        ACTUALIZANDO = 2,
        ANULANDO = 3
    }

    public class ListColoresTanque
    {

        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Color { get; set; }

        public ListColoresTanque()
        {

        }


        public ListColoresTanque(int _id, string _codigo, string _nombre, int _color)
        {
            this.ID = _id;
            this.Codigo = _codigo;
            this.Nombre = _nombre;
            this.Color = _color;
        }
    }

    public class ListTipoComprobanteArqueo
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListTipoComprobanteArqueo()
        {

        }


        public ListTipoComprobanteArqueo(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListTipoComprobanteArqueo> GetListTipoComprobanteArqueo()
        {
            List<ListTipoComprobanteArqueo> lista = new List<ListTipoComprobanteArqueo>();

            lista.Add(new ListTipoComprobanteArqueo((int)TipoComprobanteArqueo.Ninguno, "Ninguno"));
            lista.Add(new ListTipoComprobanteArqueo((int)TipoComprobanteArqueo.Clientes, "Clientes"));
            lista.Add(new ListTipoComprobanteArqueo((int)TipoComprobanteArqueo.Manejo, "Manejo"));
            lista.Add(new ListTipoComprobanteArqueo((int)TipoComprobanteArqueo.AutoConsumo, "AutoConsumo"));

            return lista;
        }
    }

    public class ListTipoDepositos
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListTipoDepositos()
        {

        }


        public ListTipoDepositos(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListTipoDepositos> GetListTipoDepositos()
        {
            List<ListTipoDepositos> lista = new List<ListTipoDepositos>();

            lista.Add(new ListTipoDepositos((int)TipoDepositos.VentaContado, "Venta de Contado"));
            lista.Add(new ListTipoDepositos((int)TipoDepositos.NotaDebito, "Nota Débito"));
            lista.Add(new ListTipoDepositos((int)TipoDepositos.RecibosOficialesCaja, "Recibos Oficiales de Caja"));

            return lista;
        }
    }

    public class ListTipoPagoManual
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListTipoPagoManual()
        {

        }


        public ListTipoPagoManual(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListTipoPagoManual> GetListTipoPagoManual()
        {
            List<ListTipoPagoManual> lista = new List<ListTipoPagoManual>();

            lista.Add(new ListTipoPagoManual((int)TipoPagoManual.PagoManual, "Pago Manual"));
            lista.Add(new ListTipoPagoManual((int)TipoPagoManual.LiquidacionAnticipo, "Liquidación Anticipo"));

            return lista;
        }
    }

    public class ListEstados
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListEstados()
        {

        }


        public ListEstados(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListEstados> GetListEstadosArqueo()
        {
            List<ListEstados> lista = new List<ListEstados>();

            lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
            lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
            lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

            return lista;
        }
    }

    public class ListEstadosOrdenes
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListEstadosOrdenes()
        {

        }


        public ListEstadosOrdenes(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListEstadosOrdenes> GetListEstadosOrdenes()
        {
            List<ListEstadosOrdenes> lista = new List<ListEstadosOrdenes>();

            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Abierta, "Abierta"));
            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Rechazada, "Rechazada"));
            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Enviada, "Enviada"));
            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Anulada, "Anulada"));
            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Aprobada, "Aprobada"));
            lista.Add(new ListEstadosOrdenes((int)EstadosOrdenes.Procesada, "Procesada"));

            return lista;
        }
    }
    
    public class ListMeses
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public ListMeses()
        {

        }


        public ListMeses(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<ListMeses> GetListMeses()
        {
            List<ListMeses> lista = new List<ListMeses>();

            lista.Add(new ListMeses((int)Meses.Enero, Meses.Enero.ToString()));
            lista.Add(new ListMeses((int)Meses.Febrero, Meses.Febrero.ToString()));
            lista.Add(new ListMeses((int)Meses.Marzo, Meses.Marzo.ToString()));
            lista.Add(new ListMeses((int)Meses.Abril, Meses.Abril.ToString()));
            lista.Add(new ListMeses((int)Meses.Mayo, Meses.Mayo.ToString()));
            lista.Add(new ListMeses((int)Meses.Junio, Meses.Junio.ToString()));
            lista.Add(new ListMeses((int)Meses.Julio, Meses.Julio.ToString()));
            lista.Add(new ListMeses((int)Meses.Agosto, Meses.Agosto.ToString()));
            lista.Add(new ListMeses((int)Meses.Septiembre, Meses.Septiembre.ToString()));
            lista.Add(new ListMeses((int)Meses.Octubre, Meses.Octubre.ToString()));
            lista.Add(new ListMeses((int)Meses.Noviembre, Meses.Noviembre.ToString()));
            lista.Add(new ListMeses((int)Meses.Diciembre, Meses.Diciembre.ToString()));

            return lista;
        }
    }

    public class TiposFaltantesSobrantes
    {

        public int ID { get; set; }
        public string Name { get; set; }

        public TiposFaltantesSobrantes()
        {

        }


        public TiposFaltantesSobrantes(int _code, string _name)
        {
            this.ID = _code;
            this.Name = _name;

        }
        public List<TiposFaltantesSobrantes> GetListEstadosArqueo()
        {
            List<TiposFaltantesSobrantes> lista = new List<TiposFaltantesSobrantes>();

            lista.Add(new TiposFaltantesSobrantes((int)TiposFS.SinDiferencia, "Sin Diferencia"));
            lista.Add(new TiposFaltantesSobrantes((int)TiposFS.Sobrante, "Sobrante"));
            lista.Add(new TiposFaltantesSobrantes((int)TiposFS.Faltante, "Faltante"));

            return lista;
        }
    }

    public class ListIdDisplay
    {     
        public int ID { get; set; }
        public string Display { get; set; }

        public ListIdDisplay()
        {

        }   

        public ListIdDisplay(int _code, string _name)
        {
            this.ID = _code;
            this.Display = _name;   
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdCodeDisplay
    {
        public int ID { get; set; }
        public int Code { get; set; }
        public string Display { get; set; }

        public ListIdCodeDisplay()
        {

        }

        public ListIdCodeDisplay(int _ID, int _code, string _display)
        {
            this.ID = _ID;
            this.Code = _code;
            this.Display = _display;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdDisplayNombre
    {
        public int ID { get; set; }
        public string Display { get; set; }
        public string Nombre { get; set; }

        public ListIdDisplayNombre()
        {

        }

        public ListIdDisplayNombre(int _code, string _display, string _name)
        {
            this.ID = _code;
            this.Display = _display;
            this.Nombre = _name;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdDisplayCodeBool
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Display { get; set; }
        public bool valor { get; set; }

        public ListIdDisplayCodeBool()
        {

        }

        public ListIdDisplayCodeBool(int _ID, string _code, string _nombre, bool _valor)
        {
            this.ID = _ID;
            this.Codigo = _code;
            this.Display = _nombre;
            this.valor = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdCuentaDisplayCodeBool
    {
        public int ID { get; set; }
        public int Cuenta { get; set; }
        public string Codigo { get; set; }
        public string Display { get; set; }
        public bool valor { get; set; }

        public ListIdCuentaDisplayCodeBool()
        {

        }

        public ListIdCuentaDisplayCodeBool(int _ID, int _cuenta, string _code, string _nombre, bool _valor)
        {
            this.ID = _ID;
            this.Cuenta = _cuenta;
            this.Codigo = _code;
            this.Display = _nombre;
            this.valor = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListToGeneratePayroll
    {
        public int EmpleadoID { get; set; }
        public string Codigo { get; set; }
        public string NombreCompleto { get; set; }
        public string Planilla { get; set; }
        public bool Selected { get; set; }
        public int AreaID { get; set; }
        public int CentroCostoID { get; set; }
        public int CargoID { get; set; }
        public string INSS { get; set; }
        public int EstructuraOrganizativaID { get; set; }
        public string OrdenCD { get; set; }        
        public bool DifTaejeta { get; set; }
        public decimal SalarioBasico { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool HasPension { get; set; }
        public bool EsPorcentaje { get; set; }
        public decimal MontoPension { get; set; }
        public bool EsSalarioPromedio { get; set; }

        public ListToGeneratePayroll()
        {

        }

        public ListToGeneratePayroll(int _ID, string _Codigo, string _Nombre, string _Planilla, string _INSS, bool _Selected, int _Area, int _CecoID, int _Cargo, int _IDEO, string _Orden, bool _tarjeta, decimal _Salario, DateTime _FechaIngreso, bool _HasPension, bool _Porcentaje, decimal _Pension, bool _esPromedio)
        {
            this.EmpleadoID = _ID;
            this.Codigo = _Codigo;
            this.NombreCompleto = _Nombre;
            this.Planilla = _Planilla;
            this.Selected = _Selected;
            this.AreaID = _Area;
            this.CentroCostoID = _CecoID;
            this.CargoID = _Cargo;
            this.INSS = _INSS;
            this.EstructuraOrganizativaID = _IDEO;
            this.OrdenCD = _Orden;
            this.DifTaejeta = _tarjeta;
            this.SalarioBasico = _Salario;
            this.FechaIngreso = _FechaIngreso;
            this.HasPension = _HasPension;
            this.EsPorcentaje = _Porcentaje;
            this.MontoPension = _Pension;
            this.EsSalarioPromedio = _esPromedio;
        }
    }

    public class ListComprobanteNomina
    {
        public int CuentaContableDebitoID { get; set; }
        public int CuentaContableCreditoID { get; set; }
        public int AreaCentoCostoID { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Orden { get; set; }
        public bool DifTarjeta { get; set; }

        public ListComprobanteNomina()
        {

        }

        public ListComprobanteNomina(int _CuentaContableDebitoID, int _CuentaContableCreditoID, int _AreaCentoCostoID, string _Descripcion, decimal _Monto, string _Orden, bool _Tarjeta)
        {
            this.CuentaContableDebitoID = _CuentaContableDebitoID;
            this.CuentaContableCreditoID = _CuentaContableCreditoID;
            this.AreaCentoCostoID = _AreaCentoCostoID;
            this.Descripcion = _Descripcion;
            this.Monto = _Monto;
            this.Orden = _Orden;
            this.DifTarjeta = _Tarjeta;
        }
    }

    public class ListKardexProducto
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string AreaNombre { get; set; }
        public int AreaID { get; set; }
        public string ClaseNombre { get; set; }
        public int ClaseID { get; set; }
        public bool Selected { get; set; }

        public ListKardexProducto()
        {

        }

        public ListKardexProducto(int _ID, string _code, string _nombre, string _area, int _areaID, string _clase, int _claseID, bool _selected)
        {
            this.ID = _ID;
            this.Codigo = _code;
            this.Nombre = _nombre;
            this.AreaNombre = _area;
            this.AreaID = _areaID;
            this.ClaseNombre = _clase;
            this.ClaseID = _claseID;
            this.Selected = _selected;
        }

    }

    public class ListIdIDCuenteDisplayCodeBool
    {
        public int ID { get; set; }
        public string Cuenta { get; set; }
        public string Codigo { get; set; }
        public string Display { get; set; }
        public bool valor { get; set; }

        public ListIdIDCuenteDisplayCodeBool()
        {

        }

        public ListIdIDCuenteDisplayCodeBool(int _ID, string _cuenta, string _code, string _nombre, bool _valor)
        {
            this.ID = _ID;
            this.Cuenta = _cuenta;
            this.Codigo = _code;
            this.Display = _nombre;
            this.valor = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdDisplayValue
    {
        public int ID { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }

        public ListIdDisplayValue()
        {

        }

        public ListIdDisplayValue(int _ID, string _nombre, decimal _valor)
        {
            this.ID = _ID;
            this.Display = _nombre;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListNombreDisplayValue
    {
        public string Nombre { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }

        public ListNombreDisplayValue()
        {

        }

        public ListNombreDisplayValue(string _Nombre, string _display, decimal _valor)
        {
            this.Nombre = _Nombre;
            this.Display = _display;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListNombreDisplayValueLimite
    {
        public string Nombre { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }
        public decimal Limite { get; set; }

        public ListNombreDisplayValueLimite()
        {

        }

        public ListNombreDisplayValueLimite(string _Nombre, string _display, decimal _valor, decimal _Limite)
        {
            this.Nombre = _Nombre;
            this.Display = _display;
            this.Value = _valor;
            this.Limite = _Limite;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdNombreDisplayValue
    {
        public int ID { get; set; } 
        public string Nombre { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }

        public ListIdNombreDisplayValue()
        {

        }

        public ListIdNombreDisplayValue(int _ID, string _Nombre, string _display, decimal _valor)
        {
            this.ID = _ID;
            this.Nombre = _Nombre;
            this.Display = _display;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdDisplayValueBooleano
    {
        public int ID { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }
        public bool Booleano { get; set; }

        public ListIdDisplayValueBooleano()
        {

        }

        public ListIdDisplayValueBooleano(int _ID, string _nombre, decimal _valor, bool _booleano)
        {
            this.ID = _ID;
            this.Display = _nombre;
            this.Value = _valor;
            this.Booleano = _booleano;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdTidDisplayValue
    {
        public int ID { get; set; }
        public int TID { get; set; }
        public string Display { get; set; }
        public decimal Value { get; set; }

        public ListIdTidDisplayValue()
        {

        }

        public ListIdTidDisplayValue(int _ID, int _TID, string _nombre, decimal _valor)
        {
            this.ID = _ID;
            this.TID = _TID;
            this.Display = _nombre;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListDisplayValue
    {
        public string Display { get; set; }
        public decimal Value { get; set; }

        public ListDisplayValue()
        {

        }

        public ListDisplayValue(string _nombre, decimal _valor)
        {
            this.Display = _nombre;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListAjuste
    {
        public int IDP { get; set; }
        public string Producto { get; set; }
        public string Medida { get; set; }
        public decimal Costo { get; set; }
        public int AlmacenID { get; set; }
        public decimal CantidadExistencia { get; set; }
        public decimal CantidadInventario { get; set; }
        
        public ListAjuste()
        {

        }

        public ListAjuste(int _IDP, string _Producto, string _Medida, decimal _Costo, int _AlmacenID, decimal _CantidadExistencia, decimal _CantidadInventario)
        {
            this.IDP = _IDP;
            this.Producto = _Producto;
            this.Medida = _Medida;
            this.Costo = _Costo;
            this.AlmacenID = _AlmacenID;
            this.CantidadExistencia = _CantidadExistencia;
            this.CantidadInventario = _CantidadInventario;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdDisplayPriceValue
    {
        public int ID { get; set; }
        public string Display { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }

        public ListIdDisplayPriceValue()
        {

        }

        public ListIdDisplayPriceValue(int _ID, string _nombre, decimal _Price, decimal _valor)
        {
            this.ID = _ID;
            this.Display = _nombre;
            this.Price = _Price;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdTidDisplayPriceValue
    {
        public int ID { get; set; }
        public int TID { get; set; }
        public string Display { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }

        public ListIdTidDisplayPriceValue()
        {

        }

        public ListIdTidDisplayPriceValue(int _ID, int _TID, string _nombre, decimal _Price, decimal _valor)
        {
            this.ID = _ID;
            this.TID = _ID;
            this.Display = _nombre;
            this.Price = _Price;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListDateSalarioValue
    {
        public DateTime Fecha { get; set; }
        public decimal Salario { get; set; }
        public decimal Total { get; set; }

        public ListDateSalarioValue()
        {

        }

        public ListDateSalarioValue(DateTime _Fecha, decimal _Salario, decimal _Total)
        {
            this.Fecha = _Fecha;
            this.Salario = _Salario;
            this.Total = _Total;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdString1String3String3
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Display { get; set; }

        public ListIdString1String3String3()
        {

        }

        public ListIdString1String3String3(int _ID, string _codigo, string _nombre, string _display)
        {
            this.ID = _ID;
            this.Codigo = _codigo;
            this.Nombre = _nombre;
            this.Display = _display;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdRefTanqueDisplayPriceValue
    {
        public int ID { get; set; }
        public string Ref { get; set; }
        public string Tanque { get; set; }
        public string Display { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }

        public ListIdRefTanqueDisplayPriceValue()
        {

        }

        public ListIdRefTanqueDisplayPriceValue(int _ID, string _Ref, string _Tanque, string _nombre, decimal _Price, decimal _valor)
        {
            this.ID = _ID;
            this.Ref = _Ref;
            this.Tanque = _Tanque;
            this.Display = _nombre;
            this.Price = _Price;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }

    public class ListIdTIDTanqueDisplayPriceValue
    {
        public int ID { get; set; }
        public int TID { get; set; }
        public string Tanque { get; set; }
        public string Display { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }

        public ListIdTIDTanqueDisplayPriceValue()
        {

        }

        public ListIdTIDTanqueDisplayPriceValue(int _ID, int _TID, string _Tanque, string _nombre, decimal _Price, decimal _valor)
        {
            this.ID = _ID;
            this.TID = _TID;
            this.Tanque = _Tanque;
            this.Display = _nombre;
            this.Price = _Price;
            this.Value = _valor;
        }

        //public List<ListEstados> GetListEstadosArqueo()
        //{
        //    List<ListEstados> lista = new List<ListEstados>();

        //    lista.Add(new ListEstados((int)Estados.Abierto, "Abierto"));
        //    lista.Add(new ListEstados((int)Estados.Modificado, "Modificado"));
        //    lista.Add(new ListEstados((int)Estados.Cerrado, "Cerrado"));

        //    return lista;
        //}
    }
    
    #endregion

    #region Clases para Gestion de Licencias

    public class GetVol
    {
        [DllImport("kernel32.dll")]
        private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);
        /// <summary>
        /// Get Volume Serial Number as string
        /// </summary>
        /// <param name="strDriveLetter">Single letter (e.g., "C")</param>
        /// <returns>string representation of Volume Serial Number</returns>
        public string GetVolumeSerial(string strDriveLetter)
        {
            uint serNum = 0;
            uint maxCompLen = 0;
            StringBuilder VolLabel = new StringBuilder(256);	// Label
            UInt32 VolFlags = new UInt32();
            StringBuilder FSName = new StringBuilder(256);	// File System Name
            strDriveLetter += ":\\";
            long Ret = GetVolumeInformation(strDriveLetter, VolLabel, (UInt32)VolLabel.Capacity, ref serNum, ref maxCompLen, ref VolFlags, FSName, (UInt32)FSName.Capacity);

            return Convert.ToString(serNum);
        }
    }

    public class LicensesManager
    {
        public LicensesManager()
        { }

        public static string GetMacAddress()
        {
            string macAd = "";
            ManagementObjectSearcher objQuery = null;
            ManagementObjectCollection queryCollection = null;

            try
            {
                objQuery = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");

                queryCollection = objQuery.Get();

                foreach (ManagementObject mgmtObject in queryCollection)
                {
                    if (mgmtObject["MacAddress"] != null)
                    {
                        macAd = mgmtObject["MacAddress"].ToString();
                        break;
                    }
                }
            }
            catch 
            {
                return "";
            }
            return macAd;
        }

        public static string GetSerialDisk()
        {
            string diskserial = "";
            ManagementObjectSearcher objQuery = null;
            ManagementObjectCollection queryCollection = null;

            try
            {
                objQuery = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                queryCollection = objQuery.Get();

                foreach (ManagementObject mgmtObject in queryCollection)
                {
                    if (mgmtObject["SerialNumber"] != null)
                    {
                        diskserial = mgmtObject["SerialNumber"].ToString();
                        break;
                    }
                }
            }
            catch
            {
                return "";
            }
            return diskserial.Trim();
        }

        public static string GetBaseBoard()
        {
            string BaseBoard = "";
            ManagementObjectSearcher objQuery = null;
            ManagementObjectCollection queryCollection = null;

            try
            {
                objQuery = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

                queryCollection = objQuery.Get();

                foreach (ManagementObject mgmtObject in queryCollection)
                {
                    if (mgmtObject["SerialNumber"] != null)
                    {
                        BaseBoard = mgmtObject["SerialNumber"].ToString();
                        break;
                    }
                }
            }
            catch
            {
                return "";
            }
            return BaseBoard.Trim();
        }

        public static string GetCPUId()
        {
            string cpuInfo = "";
            ManagementObjectSearcher objQuery = null;
            ManagementObjectCollection queryCollection = null;

            try
            {
                objQuery = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

                queryCollection = objQuery.Get();

                foreach (ManagementObject mgmtObject in queryCollection)
                {
                    if (mgmtObject["ProcessorId"] != null)
                    {
                        cpuInfo = mgmtObject["ProcessorId"].ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return cpuInfo.Trim();

        }

        public static string GetLogicalDiskSerial()
        {
            try
            {
                string dirbase = AppDomain.CurrentDomain.BaseDirectory.Substring(0, 1);
                ManagementObject disk = new ManagementObject("Win32_LogicalDisk.DeviceID=\"" + dirbase + ":\"");
                PropertyData diskPropertyA = disk.Properties["VolumeSerialNumber"];
                return diskPropertyA.Value.ToString();
            }
            catch { return ""; }
        }

        public static string GetNamePC()
        {
            string _namepc = "";

            try
            {
                _namepc = System.Environment.MachineName;
            }
            catch (Exception ex)
            {
                return "";
            }
            return _namepc.Trim();
        }
                
        //public static void SetBussinessKey(Entidades.AdmonPDataClassesDataContext db)
        //{
        //    if (db.Empresa.Count() > 0)
        //    {
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyBussinessName, db.Empresa.First().Nombre);
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyShortBussinessName, db.Empresa.First().NombreCorto);

        //    }
        //}

        public static bool VerificarLicencia(string pNameAplication, SAGAS.Entidad.SAGASDataClassesDataContext db)
        {
            try
            {

            GetVol gv = new GetVol();
            string NamePC = GetNamePC();
            string BaseBoard = GetBaseBoard();
            string SerialDisk = gv.GetVolumeSerial("C");
            string CPUActual = GetCPUId();
            string LogicalDiskSerial = GetLogicalDiskSerial();

            string RKeyRegistro = "", RKeyBD = "";
            string RNamePC = "", RBaseBoard = "", RSerialDisk = "", RCPUActual = "", RLogicalDiskSerial = "", RESID = "", RTipoConexionID = "";

            RNamePC = Config.GetValueByKeyAppSettings(Config.strVarNamePC);
            RBaseBoard = Config.GetValueByKeyAppSettings(Config.strVarBaseBoard);
            RSerialDisk = Config.GetValueByKeyAppSettings(Config.strVarSerialDisk);
            RCPUActual = Config.GetValueByKeyAppSettings(Config.strVarCPUActual);
            RLogicalDiskSerial = Config.GetValueByKeyAppSettings(Config.strLogicalDiskSerial);
            RESID = Config.GetValueByKeyAppSettings(Config.strVarESID);
            RTipoConexionID = Config.GetValueByKeyAppSettings(Config.strTipoConexionID);
            RKeyRegistro = Config.GetValueByKeyAppSettings(Config.strVarKeyRegistro);
            RKeyBD = Config.GetValueByKeyAppSettings(Config.strKeyBD);

            if (!NamePC.Equals(RNamePC) || !BaseBoard.Equals(RBaseBoard) || !SerialDisk.Equals(RSerialDisk) || !CPUActual.Equals(RCPUActual) || !LogicalDiskSerial.Equals(RLogicalDiskSerial))
            {
                if (Parametros.General.DialogMsg(Properties.Resources.MSGLICENCIAINVALIDA + Environment.NewLine + Properties.Resources.MSGVALIDARLICENCIA, SAGAS.Parametros.MsgType.question).Equals(DialogResult.OK))
                {
                    if (!ActualizarLicencia(NamePC, BaseBoard, SerialDisk, CPUActual, LogicalDiskSerial, "", 0, 0))
                        return false;
                }
                else
                return false;
            }


            var Licencia = db.Licencias.Where(l => l.NamePC.Equals(Parametros.Security.Encrypt(RNamePC)) && l.BaseBoard.Equals(Parametros.Security.Encrypt(RBaseBoard)) && l.SerialDisk.Equals(Parametros.Security.Encrypt(RSerialDisk))
                && l.CPUId.Equals(Parametros.Security.Encrypt(RCPUActual)) && l.LogicalDiskSerial.Equals(Parametros.Security.Encrypt(RLogicalDiskSerial)) && l.EstacionServicio.Equals(Parametros.Security.Encrypt(RESID)) 
                //&& l.TipoConexion.Equals(Parametros.Security.Encrypt(RTipoConexionID)) 
                && l.SerialRegistro.Equals(Parametros.Security.Encrypt(RKeyRegistro)) && l.SerialBD.Equals(Parametros.Security.Encrypt(RKeyBD)));

            if (Licencia.Count() <= 0)
            {
                if (Parametros.General.DialogMsg(Properties.Resources.MSGLICENCIAINVALIDA + Environment.NewLine + Properties.Resources.MSGVALIDARLICENCIA, SAGAS.Parametros.MsgType.question).Equals(DialogResult.OK))
                {
                    if (!ActualizarLicencia(NamePC, BaseBoard, SerialDisk, CPUActual, LogicalDiskSerial, RKeyRegistro, Convert.ToInt32(RESID), (int)((TipoConexion)(Enum.Parse(typeof(TipoConexion), RTipoConexionID))))) 
                        return false;                                                                                                                                  //Enum.GetName(typeof(TipoConexion), rgConexion.EditValue))
                }
                else
                    return false;
            }
            
            return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, "VerificarLicencia");
                return false;
            }
        }


        //    if (!IsFull && String.IsNullOrEmpty(key))
        //    {

        //        #region ::: Guardando la Configuracion de la Licencia Demo :::
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyNamePC, NamePC);
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyDisk, diskActual);
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyCPU, CPUActual);
        //        Global.SetValueByKeyAppSettings(Global.strVarReg, "True");
        //        if (String.IsNullOrEmpty(Global.GetValueByKeyAppSettings(Global.strVarKeyQtyIn)))
        //            Global.SetValueByKeyAppSettings(Global.strVarKeyQtyIn, "1");
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyQtyIn, Convert.ToString(Convert.ToInt32(Global.GetValueByKeyAppSettings(Global.strVarKeyQtyIn)) + 1));
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyTypeVersion, Classes.TiposVersion.Demo.ToString());
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyQtyEmployee, "10");
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyQtyPlanilla, "6");
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyBussinessNumber, "1");
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyServer, "True");

        //        #endregion

        //        return true;


        //    }
        //    else
        //    {
        //        #region ::: Guardando la Configuracion de la Licencia Demo :::
        //        string keyLic = "", SerialDiskLic = "", SerialCPULic = "", serialNamePCLic;

        //        keyLic = Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyLicense);

        //        if (String.IsNullOrEmpty(key))
        //            Global.SetValueByKeyAppSettings(Global.strVarKeyLicense, keyLic);

        //        SerialDiskLic = Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyDisk);
        //        SerialCPULic = Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyCPU);
        //        serialNamePCLic = Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyNamePC);

        //        if (SerialDisk != SerialDiskLic || SerialCPU != SerialCPULic || serialNamePC != serialNamePCLic)
        //            return false;

        //        Global.SetValueByKeyAppSettings(Global.strVarKeyTypeVersion, Classes.TiposVersion.Full.ToString());
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyBussinessNumber, Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyBussinessNumber));
        //        Global.SetValueByKeyAppSettings(Global.strVarKeyServer, Global.GetValueByKeyAppSettingsKeyApp(Global.strVarKeyServer));

        //        #endregion

        //        return true;

        //    }



        //    //        if (resultado != 1) //No Registrado
        //    //        {
        //    //            using (Forms.Dialogs.dialogLicense dialog = new Forms.Dialogs.dialogLicense())
        //    //            {
        //    //                dialog._NameAplicacion = pNameAplication;
        //    //                if (dialog.ShowDialog() == DialogResult.OK)
        //    //                {

        //    //                    resultado = Convert.ToInt32(obj_service.SendKeyToValidate(out numerCash, out VersionType, out QtyProduct, out QtyMenus, out QtyInvoice, out QtyCorrection, out QtyTransfer
        //    //                   , out QtyOtherInputs, out QtyOtherOutputs, out QtyTables, out QtyGames, out QtyClients, out QtyTourOperator, out QtyRoom, out QtyRates
        //    //                   , out QtyService, out QtyReservation, out QtyBill, out ExpirationDate, dialog._LicenciaIntroducida, pCodeAplicacion, diskActual, CPUActual, NamePC));

        //    //                    if (resultado == 1) //No Registrado
        //    //                    {
        //    //                        #region ::: Guardando la Configuracion de la Licencia :::
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyLicense, dialog._LicenciaIntroducida);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyNamePC, NamePC);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyDisk, diskActual);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyCPU, CPUActual);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarReg, "True");
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyNumberCash, numerCash);
        //    //                        if (VersionType == "D")
        //    //                            Config.SetValueByKeyAppSettings(Config.strVarKeyTypeVersion, TiposVersion.Demo.ToString());
        //    //                        else if (VersionType == "F") Config.SetValueByKeyAppSettings(Config.strVarKeyTypeVersion, TiposVersion.Full.ToString());
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyProductDemo, QtyProduct);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyMenusDemo, QtyMenus);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyInvoiceDemo, QtyInvoice);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyCorrectionDemo, QtyCorrection);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTransferDemo, QtyTransfer);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyOtherInputsDemo, QtyOtherInputs);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyOtherOutputsDemo, QtyOtherOutputs);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTablesDemo, QtyTables);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyGamesDemo, QtyGames);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyClientsDemo, QtyClients);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTourOperatorDemo, QtyTourOperator);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyRoomDemo, QtyRoom);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyRatesDemo, QtyRates);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyServiceDemo, QtyService);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyReservationDemo, QtyReservation);
        //    //                        Config.SetValueByKeyAppSettings(Config.strVarKeyQtyBillDemo, QtyBill);
        //    //                        if (String.IsNullOrEmpty(ExpirationDate) || ExpirationDate == "0000-00-00" || ExpirationDate == "00-00-0000")
        //    //                            Config.SetValueByKeyAppSettings(Config.strVarKeyExpirationDateDemo, "null");
        //    //                        else
        //    //                        {
        //    //                            Config.SetValueByKeyAppSettings(Config.strVarKeyExpirationDateDemo, ExpirationDate);
        //    //                        }
        //    //                        #endregion

        //    //                        MessageBox.Show("Gracias por Usar Productos PROCOM", "Sistema de Recursos Humanos");
        //    //                        return true;
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        MessageBox.Show("Su licencia no es Valida", "Sistema de Recursos Humanos");
        //    //                        return false;
        //    //                    }
        //    //                }
        //    //            }
        //    //        }
        //    //        else return true;

        //    //    }
        //    //    catch (Exception ex)//Probablemente no hay internet, Utilizar metodo alternativo
        //    //    {
        //    //        if (Registro == "True" && key.Length == 25 && SerialCPU == CPUActual && SerialDisk == diskActual) // Dejar pasar por esta vez
        //    //            return true;


        //    //        dsSystem.Utils.Forms.Dialogs.dialogInfo frm = new dsSystem.Utils.Forms.Dialogs.dialogInfo();
        //    //        frm.lblInfo.Text = "El Sistema no ha podido Validar  la Licencia de este Producto. Por favor verifique la Conexión a Internet. Si el problema persiste comuníquese con PROCOM.   (www.procom.co.cr)";
        //    //        frm.txtInfo.Text = ex.Message;
        //    //        frm.ShowDialog();

        //    //        return false;
        //    //    }
        //    //}
        //    return false;

        //}


        public static bool ActualizarLicencia(string KeyNamePC, string KeyBaseBoard, string KeySerialDisk, string KeyCPUActual, 
            string KeyLogicalDiskSerial, string KeyRegistro, int KeyESID, int keyConexion)
        {
            try
            {
                using (Forms.Dialogs.DialogLicencia dl = new Forms.Dialogs.DialogLicencia())
                {
                    dl.KeyNamePC = KeyNamePC;
                    dl.KeyBaseBoard = KeyBaseBoard;
                    dl.KeySerialDisk = KeySerialDisk;
                    dl.KeyCPUActual = KeyCPUActual;
                    dl.KeyLogicalDiskSerial = KeyLogicalDiskSerial;
                    dl.KeyRegistro = KeyRegistro;
                    dl.ESID = KeyESID;
                    dl.TipoConexionValue = keyConexion;

                    if (dl.ShowDialog().Equals(DialogResult.OK))
                    {

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)//Probablemente no hay internet, Utilizar metodo alternativo
            {
                Parametros.General.DialogMsg("El Sistema no ha podido validar la Licencia de SAGAS.", MsgType.warning);
                return false;
            }
        }
        //        ServiceDSLM.DSLMWebServicePortTypeClient obj_service = new ServiceDSLM.DSLMWebServicePortTypeClient();
        //        string numerCash = "";
        //        string diskActual = GetSerialDisk();
        //        string CPUActual = GetCPUId();
        //        string NamePC = GetNamePC();
        //        string VersionType = "", QtyProduct = "", QtyMenus = "", QtyInvoice = "", QtyCorrection = "", QtyTransfer = ""
        //                , QtyOtherInputs = "", QtyOtherOutputs = "", QtyTables = "", QtyGames = "", QtyClients = "", QtyTourOperator = "", QtyRoom = "", QtyRates = ""
        //                , QtyService = "", QtyReservation = "", QtyBill = "", ExpirationDate = "";

        //        using (Forms.Dialogs.dialogLicense dialog = new Forms.Dialogs.dialogLicense())
        //        {
        //            dialog._NameAplicacion = pNameAplication;
        //            dialog._LicenciaRegistrada = Config.GetValueByKeyAppSettings(Config.strVarKeyLicense);
        //            dialog.EsActualizacion = true;
        //            if (dialog.ShowDialog() == DialogResult.OK)
        //            {
        //                Int32 resultado = Convert.ToInt32(obj_service.SendKeyToValidate(out numerCash, out VersionType, out QtyProduct, out QtyMenus, out QtyInvoice, out QtyCorrection, out QtyTransfer
        //                                , out QtyOtherInputs, out QtyOtherOutputs, out QtyTables, out QtyGames, out QtyClients, out QtyTourOperator, out QtyRoom, out QtyRates
        //                                , out QtyService, out QtyReservation, out QtyBill, out ExpirationDate, dialog._LicenciaIntroducida, pCodeAplicacion, diskActual, CPUActual, NamePC));

        //                if (resultado == 1) //Valido para Registrar
        //                {
        //                    #region ::: Guardando la Configuracion de la Licencia :::
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyLicense, dialog._LicenciaIntroducida);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyNamePC, NamePC);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyDisk, diskActual);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyCPU, CPUActual);
        //                    Config.SetValueByKeyAppSettings(Config.strVarReg, "True");
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyNumberCash, numerCash);
        //                    if (VersionType == "D")
        //                        Config.SetValueByKeyAppSettings(Config.strVarKeyTypeVersion, TiposVersion.Demo.ToString());
        //                    else if (VersionType == "F") Config.SetValueByKeyAppSettings(Config.strVarKeyTypeVersion, TiposVersion.Full.ToString());
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyProductDemo, QtyProduct);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyMenusDemo, QtyMenus);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyInvoiceDemo, QtyInvoice);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyCorrectionDemo, QtyCorrection);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTransferDemo, QtyTransfer);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyOtherInputsDemo, QtyOtherInputs);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyOtherOutputsDemo, QtyOtherOutputs);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTablesDemo, QtyTables);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyGamesDemo, QtyGames);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyClientsDemo, QtyClients);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyTourOperatorDemo, QtyTourOperator);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyRoomDemo, QtyRoom);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyRatesDemo, QtyRates);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyServiceDemo, QtyService);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyReservationDemo, QtyReservation);
        //                    Config.SetValueByKeyAppSettings(Config.strVarKeyQtyBillDemo, QtyBill);
        //                    if (String.IsNullOrEmpty(ExpirationDate) || ExpirationDate == "0000-00-00" || ExpirationDate == "00-00-0000")
        //                        Config.SetValueByKeyAppSettings(Config.strVarKeyExpirationDateDemo, "null");
        //                    else
        //                    {
        //                        Config.SetValueByKeyAppSettings(Config.strVarKeyExpirationDateDemo, ExpirationDate);
        //                    }
        //                    #endregion


        //                    MessageBox.Show("Gracias por Usar Productos PROCOM", "Sistema de Recursos Humanos");
        //                    return true;
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Su licencia no es Valida", "Sistema de Recursos Humanos");
        //                    return false;
        //                }
        //            }
        //        }
      
        

    }

    #endregion

}
/*
 * public static void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days, int add)
        {
            endDate = endDate.Date.AddDays(add);
            if (endDate < beginDate)
            {
                DateTime d3 = beginDate;
                beginDate = endDate;
                endDate = d3;
            }
            months = 12 * (endDate.Year - beginDate.Year) + (endDate.Month - beginDate.Month);

            if (endDate.Day < beginDate.Day)
            {
                months--;
                days = DateTime.DaysInMonth(beginDate.Year, beginDate.Month) - beginDate.Day + endDate.Day;
            }
            else
            {
                if (endDate.Day.Equals(31))
                    endDate = endDate.Date.AddDays(-1);

                days = endDate.Day - beginDate.Day;

                if (endDate.Month.Equals(2))
                    days += 2;

            }
            years = months / 12;
            months -= years * 12;
        }
 */