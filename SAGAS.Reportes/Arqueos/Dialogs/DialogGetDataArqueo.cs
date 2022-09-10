using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Linq;
using DevExpress.Skins;
using System.IO;
using System.Reflection;

namespace SAGAS.Reportes.Arqueos.Dialogs
{
    public partial class DialogGetDataArqueo : Form
    {
        #region *** DECLARACIONES ***
        private Entidad.SAGASDataClassesDataContext db;
        private int UsuarioID = Parametros.General.UserID;
        public Form _MDIParent = new Form();
        public bool Comprobante = false;

        public int RD
        {
            get { return Convert.ToInt32(lkRD.EditValue); }
            set { lkRD.EditValue = value; }
        }

        struct ListContado
        {
            public decimal Litros;
            public decimal Galones;
            public decimal Valor;
        };

        public int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        public int IDRD
        {
            get { return Convert.ToInt32(lkRD.EditValue); }
            set { lkRD.EditValue = value; }
        }
         
        #endregion

        #region *** INICIO ***

        public DialogGetDataArqueo()
        {
            InitializeComponent();

        }
                
        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                lkES.Properties.DataSource = from es in db.EstacionServicios
                                                            where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                                                            select new { es.ID, es.Nombre };
                lkES.Properties.DisplayMember = "Nombre";
                lkES.Properties.ValueMember = "ID";
                lkES.EditValue = Parametros.General.EstacionServicioID;

                

                //if (Convert.ToInt32(db.ResumenDias.Count(r => r.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && r.SubEstacionID.Equals(IDSubEstacion))) > 0)
                //{
                //    Fecha = Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(Parametros.General.EstacionServicioID)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1);
                //    this.dateFecha.Properties.ReadOnly = true;
                //}

                //if (Parametros.General.ListSES.Count > 0)
                //{
                //    this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                //    if (Parametros.General.ListSES.Count.Equals(1))
                //    {
                //        IDSubEstacion = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                //        lkSES.Properties.ReadOnly = true;
                //    }
                //}


                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** METODOS ***

        
        //private bool Guardar()
        //{
        //    try
        //    {
        //        if (!IsSusOnly)
        //        {
        //            if (Convert.ToDateTime(dateFecha.EditValue) < FechaMinima || Convert.ToDateTime(dateFecha.EditValue) > FechaMaxima)
        //            {
        //                Parametros.General.DialogMsg("La fecha debe de estar dentro del rango", Parametros.MsgType.warning);
        //                return false;
        //            }
        //        }

        //        if (Parametros.General.ListSES.Count > 0 && lkSES.EditValue == null)
        //        {
        //            Parametros.General.DialogMsg("Debe de seleccionar una Sub Estación", Parametros.MsgType.warning);
        //            return false;

        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //        return false;
        //    }
        //}

        private decimal OtroEquivalente(string texto)
        {
            try
            {
                string input = Regex.Replace(texto, "[^0-9]+", string.Empty);
                return Convert.ToDecimal(input);
            }
            catch
            {return 1m;}
        }

        public bool ValidarCampos()
        {
            try
            {
                if (lkES.EditValue == null || Convert.ToInt32(lkRD.EditValue) <= 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (this.layoutControlItemSES.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
                {
                    if (lkSES.EditValue == null)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (Comprobante)
                {
                    var RD = db.ResumenDias.FirstOrDefault(r => r.ID.Equals(IDRD));
                    if (RD != null)
                    {
                        if (!RD.Aprobado)
                        {
                            Parametros.General.DialogMsg("El Resumen del Día no esta aprobado" + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                    else
                        return false;
                }

                return true;
            }
            catch
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    if (!Comprobante)
                    {
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                        rv.Text = "Reporte de Resumenes con Valores";

                        Reportes.Arqueos.Hojas.RptResumenCupones rep = new Reportes.Arqueos.Hojas.RptResumenCupones();
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                        var vista = dbView.spListValorExtraxion(IDEstacionServicio, IDRD, 0, 0).ToList();

                        if (vista.Count(v => v.ProductoID.HasValue) <= 0)
                        {
                            Parametros.General.DialogMsg("El Resumen de Día Seleccionado no contiene valores en Distribución de Extracción.", Parametros.MsgType.message);
                            return;
                        }

                        int ArqueoIslaID = 0;
                        int TurnoID = 0;

                        var AIView = from v in dbView.VistaArqueoIslas
                                     where (v.ArqueoIslaID.Equals(ArqueoIslaID) || ArqueoIslaID.Equals(0)) && (!v.ArqueoEspecial || (v.ArqueoEspecial && v.Oficial)) && (v.TurnoID.Equals(TurnoID) || TurnoID.Equals(0))
                                     && v.ResumenDiaID.Equals(IDRD)
                                     select v;

                        var AMView = dbView.VistaArqueoMangueras.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));
                        var APEView = dbView.VistaArqueoProductoExtraxions.Where(v => AMView.Any(am => am.ArqueoProductoID.Equals(v.ArqueoProductoID)));
                        var AFPView = dbView.VistaArqueoFormaPagos.Where(v => AIView.Any(ai => ai.ArqueoIslaID.Equals(v.ArqueoIslaID)));

                        string Nombre, Direccion, Telefono;
                        System.Drawing.Image picture_LogoEmpresa;
                        Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                        rep.PicLogo.Image = picture_LogoEmpresa;
                        rep.CeEmpresa.Text = Nombre;
                        rep.lblTipoImpresion.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                        rep.CeEstacion.Text = (AIView.First().SubEstacionID > 0 ? AIView.First().SubEstacionNombre : AIView.First().EstacionNombre);
                        rep.ceFecha.Text = AIView.First().ResumenDiaFechaInicial.ToShortDateString();

                        rep.xrSubreportExtraxiones.ReportSource.DataSource = db.ExtracionPagos.Where(v => v.Nombre.ToLower().Contains("cup")).OrderBy(o => o.Orden);
                        rep.xrSubreportExtraxionesCard.ReportSource.DataSource = db.ExtracionPagos.Where(v => !v.EsPago && !v.Nombre.ToLower().Contains("cup")).OrderBy(o => o.Orden);

                        float x = 100f;
                        vista.Where(w => w.ProductoID.HasValue).GroupBy(g => g.ProductoID).Select(s => s.FirstOrDefault()).Distinct().OrderBy(o => o.ProductoID).ToList().ForEach(prod =>
                            {
                                //Cupones
                                DevExpress.XtraReports.UI.XRSubreport xrSubProd = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProd.LocationFloat = new DevExpress.Utils.PointFloat(x, 0F);
                                xrSubProd.ReportSource = new Reportes.Arqueos.Hojas.SubRptProductoCupon(prod.Color);
                                xrSubProd.SizeF = new System.Drawing.SizeF(100F, 200F);

                                var datos = from v in vista.Where(w => w.ExtraxionNombre.ToLower().Contains("cup"))
                                            group v by new { v.ID, v.EquivalenteLitros } into gr
                                            select new
                                            {
                                                ProductoNombre = prod.ProductoNombre,
                                                Cantidad = (gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) > 0 ?
                                                (gr.Key.EquivalenteLitros > 0 ?
                                                gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) /
                                                gr.Key.EquivalenteLitros
                                                : Decimal.Round(gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) * prod.Precio /
                                                OtroEquivalente(gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).First().ExtraxionNombre), 0, MidpointRounding.AwayFromZero)
                                                )
                                                : 0),
                                                Litros = gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor),
                                                Galones = (gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) / 3.7854m),
                                                Valor = (gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) * prod.Precio)
                                            };
                                xrSubProd.ReportSource.DataSource = datos;
                                rep.GroupHeaderCupones.Controls.Add(xrSubProd);

                                //PetroCard
                                DevExpress.XtraReports.UI.XRSubreport xrSubProdCard = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProdCard.LocationFloat = new DevExpress.Utils.PointFloat(x, 100F);
                                xrSubProdCard.ReportSource = new Reportes.Arqueos.Hojas.SubRptProductoCupon(prod.Color, true);
                                xrSubProdCard.SizeF = new System.Drawing.SizeF(100F, 200F);

                                var Cards = from v in vista.Where(w => !w.ExtraxionNombre.ToLower().Contains("cup"))
                                            group v by new { v.ID } into gr
                                            select new
                                            {
                                                ProductoNombre = prod.ProductoNombre,
                                                Litros = gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor),
                                                Galones = (gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) / 3.7854m),
                                                Valor = (gr.Where(w => w.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor) * prod.Precio)
                                            };
                                xrSubProdCard.ReportSource.DataSource = Cards;
                                rep.GroupHeaderCupones.Controls.Add(xrSubProdCard);

                                //Ventas Contado
                                DevExpress.XtraReports.UI.XRSubreport xrSubProdVentContado = new DevExpress.XtraReports.UI.XRSubreport();
                                xrSubProdVentContado.LocationFloat = new DevExpress.Utils.PointFloat(x, 200F);
                                xrSubProdVentContado.ReportSource = new Reportes.Arqueos.Hojas.SubRptProductoCupon(prod.Color, false);
                                xrSubProdVentContado.SizeF = new System.Drawing.SizeF(100F, 100F);

                                decimal TotExtraxion = 0m;
                                decimal RestaExtraxion = 0m;
                                var vistaMang = AMView.Where(v => v.ProductoID.Equals(prod.ProductoID));
                                TotExtraxion = vistaMang.Count(v => v.EsLecturaElectronica) > 0 ? Convert.ToDecimal(vistaMang.Sum(s => s.ExtraxionElectronica)) : Convert.ToDecimal(vistaMang.Sum(s => s.ExtraxionMecanica));
                                RestaExtraxion = APEView.Count(v => v.ProductoID.Equals(prod.ProductoID)) > 0 ? Convert.ToDecimal(APEView.Where(v => v.ProductoID.Equals(prod.ProductoID)).Sum(s => s.Valor)) : 0m;
                                decimal total = TotExtraxion - RestaExtraxion;

                                //ListContado lc;
                                //lc.Litros = total;
                                //lc.Galones = Decimal.Round(total / 3.785m, 2, MidpointRounding.AwayFromZero);
                                //lc.Valor = Decimal.Round(total * prod.Precio, 2, MidpointRounding.AwayFromZero);

                                //List<ListContado> list = new List<ListContado>();
                                //list.Add(lc);

                                //var listTotal = from l in list
                                //                select new
                                //                {
                                //                    ProductoNombre = prod.ProductoNombre,
                                //                    Litros = l.Litros,
                                //                    Galones = l.Galones,
                                //                    Valor = l.Valor
                                //                };

                                //xrSubProdVentContado.ReportSource.DataSource = listTotal;
                                //rep.GroupHeaderCupones.Controls.Add(xrSubProdVentContado);

                                x += 180;
                            });

                        rep.DataSource = AFPView.Where(v => v.MonedaID.Equals(0)).GroupBy(g => g.PagoID).Select(o => new { PagoNombre = o.First().PagoNombre, Valor = o.Where(w => w.PagoID.Equals(o.Key)).Sum(s => s.Valor) });
                        rep.xrTableCellFormasPago.DataBindings.Add("Text", null, "PagoNombre", "{0:n2}");
                        rep.xrTableCellFPValor.DataBindings.Add("Text", null, "Valor", "{0:n2}");
                        rep.xrTableCellFPValorTotal.DataBindings.Add("Text", null, "Valor", "{0:n2}");



                        rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                        //rv.Owner = this.Owner;
                        rv.MdiParent = _MDIParent;

                        rep.RequestParameters = false;
                        rep.CreateDocument();
                        rv.Show();
                        this.Dispose();
                    }
                    else
                        this.Close();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {
            IDRD = 0;
            this.Close();
        }
     
         #endregion 

        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkES.EditValue != null)
                {
                    
                    if (db.SubEstacions.Count(sus => sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))) > 0)
                    {
                        this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        //**SubEstación
                        lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))).ToList();
                        lkSES.Properties.DisplayMember = "Nombre";
                        lkSES.Properties.ValueMember = "ID";
                        lkRD.Properties.DataSource = null;

                    }
                    else
                    {
                        this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        if (!Comprobante)
                        {
                            var listaRD = (from r in db.ResumenDias.ToList()
                                           where r.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue))
                                           select new { r.ID, r.Numero, r.FechaInicial, Display = r.Numero + " - " + String.Format("{0:d}", r.FechaInicial) }).ToList();

                            lkRD.Properties.DataSource = listaRD;
                            lkRD.Properties.DisplayMember = "Display";
                            lkRD.Properties.ValueMember = "ID";
                        }
                        else if (Comprobante)
                        {
                            DateTime vFecha = db.PeriodoContables.Where(p => p.EstacionID.Equals(Convert.ToInt32(lkES.EditValue)) && !p.Cerrado && !p.Consolidado).OrderBy(o => o.FechaInicio).First().FechaInicio;

                            var listaRD = (from r in db.ResumenDias.ToList()
                                           where r.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) && !r.Contabilizado && r.FechaInicial.Date >= vFecha.Date 
                                           select new { r.ID, r.Numero, r.FechaInicial, Display = r.Numero + " - " + String.Format("{0:d}", r.FechaInicial) }).ToList();

                            lkRD.Properties.DataSource = listaRD;
                            lkRD.Properties.DisplayMember = "Display";
                            lkRD.Properties.ValueMember = "ID";
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

        private void lkSES_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Comprobante)
                {
                    var listaRD = from r in db.ResumenDias
                                  where r.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) && r.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue))
                                  select new { r.ID, r.Numero, r.FechaInicial, Display = r.Numero + " - " + r.FechaInicial };

                    lkRD.Properties.DataSource = listaRD;
                    lkRD.Properties.DisplayMember = "Display";
                    lkRD.Properties.ValueMember = "ID";
                }
                else if (Comprobante)
                {
                    DateTime vFecha = db.PeriodoContables.Where(p => p.EstacionID.Equals(Convert.ToInt32(lkES.EditValue)) && !p.Cerrado && !p.Consolidado).OrderBy(o => o.FechaInicio).First().FechaInicio;

                    var listaRD = from r in db.ResumenDias
                                  where r.EstacionServicioID.Equals(Convert.ToInt32(lkES.EditValue)) && r.SubEstacionID.Equals(Convert.ToInt32(lkSES.EditValue)) && !r.Contabilizado && r.FechaInicial.Date >= vFecha.Date
                                  select new { r.ID, r.Numero, r.FechaInicial, Display = r.Numero + " - " + r.FechaInicial };
                    
                    lkRD.Properties.DataSource = listaRD;
                    lkRD.Properties.DisplayMember = "Display";
                    lkRD.Properties.ValueMember = "ID";
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

    }
}