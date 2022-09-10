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

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogOpcionesGenerales : Form
    {
        #region *** DECLARACIONES ***
        private List<Entidad.ArqueoTipoCambio> ArqueosTC;

        #endregion

        #region *** INICIO ***

        public DialogOpcionesGenerales()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
           

            GetDataSystem();
        }

        #endregion

        #region *** METODOS ***

         public bool TestConnection(string _stringconexion, bool MostrarMsjBueno)
        {
            try
            {
                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
                {
                    conn.ConnectionString = _stringconexion;
                    conn.Open();
                    conn.Close();

                    if (MostrarMsjBueno)
                        Parametros.General.DialogMsg("CONECCION REALIZADA EXITOSAMENTE.", Parametros.MsgType.message);

                    return true;
                }
            }
            catch (System.Data.SqlClient.SqlException except)
            {
                switch (except.Number)
                {
                    case 50:
                        MessageBox.Show("EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 53:
                        MessageBox.Show("EL SERVIDOR ES INACCESIBLE. ERROR AL CONECTARSE." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 233:
                        MessageBox.Show("CONEXIÓN CON EL SERVIDOR ESTABLECIDA, PERO EL LOGIN ES INCORRECTO." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case 4060:
                        MessageBox.Show("NO SE PUEDE CONECTARSE A LA BASE DE DATOS INDICADA." + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    default:
                        MessageBox.Show("ERROR GENERAL DE LA CONEXION DE SQL!!!" + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                return false;

            }
            catch (System.Exception except)
            {
                MessageBox.Show(Parametros.Properties.Resources.MSGERROR + Environment.NewLine + except.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

         private void GetDataSystem()
         {
             try
             {
                 Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                 db.CommandTimeout = 180;

                 lkeClaseProducto.Properties.DataSource = from e in db.ProductoClases where e.Activo select new { e.ID, e.Nombre };
                 lkeClaseProducto.Properties.DisplayMember = "Nombre";
                 lkeClaseProducto.Properties.ValueMember = "ID";

                 lkClienteManejadorPrestamo.Properties.DataSource = from c in db.Clientes where c.Activo select new { c.ID, Display =c.Nombre };
                 lkClienteManejadorPrestamo.Properties.DisplayMember = "Display";
                 lkClienteManejadorPrestamo.Properties.ValueMember = "ID";

                 lkProveedorManejadorPrestamo.Properties.DataSource = from p in db.Proveedors where p.Activo select new { p.ID, Display=p.Nombre };
                 lkProveedorManejadorPrestamo.Properties.DisplayMember = "Display";
                 lkProveedorManejadorPrestamo.Properties.ValueMember = "ID";

                 ArqueosTC = db.ArqueoTipoCambios.ToList();
                 bdsTipoCambio.DataSource = ArqueosTC;

                 var dtobj = (from c in db.Configuracions select c);

                 foreach (var i in dtobj)
                 {

                     switch (i.Campo.ToString())
                     {
                         /*Opciones de la Empresa*/
                         case "DisplayMessageDialog": this.chkMostrarCuadroInformativo.Checked = Convert.ToBoolean(i.Valor.ToLower()); break;
                         case "ProductoClaseCombustible": this.lkeClaseProducto.EditValue = Convert.ToInt32(i.Valor); break;
                         case "RangoDiasPrecioCombustible": this.speRangoDiasPrecioCombustible.EditValue = Convert.ToInt32(i.Valor); break;
                         case "VariacionPorcentualPrecio": this.speVariacionPorcentualPrecio.EditValue = Convert.ToInt32(i.Valor); break;
                         case "DiferenciaVerificacionLectura": this.speDiferenciaVerificacionLectura.EditValue = Convert.ToInt32(i.Valor); break;
                         case "DescuentoPetroCard": this.speDescuentoPetroCard.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "RangoEfectivo": this.spRangoEfectivo.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "RangoMecElectronico": this.spRangoMecElectronico.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "MargenValorCambioIVA": this.spMargenIVA.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "MargenValorCambioTC": this.spMargenTC.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "FechaCierreActa": this.dateFechaCierreActa.EditValue = Convert.ToDateTime(i.Valor); break;
                         //INVENTARIO
                         case "ClientePrestamoManejoID": this.lkClienteManejadorPrestamo.EditValue = Convert.ToInt32(i.Valor); break;
                         case "ProveedorPrestamoManejoID": this.lkProveedorManejadorPrestamo.EditValue = Convert.ToInt32(i.Valor); break;
                         //NOMINA
                         case "ValorINSSLaboral": this.spValorINSSLaboral.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "ValorINSSPatronal": this.spValorINSSPatronal.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "ValorINATEC": this.spValorINATEC.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "MaximoSalarioINSS": this.spMaximoSalarioINSS.EditValue = Convert.ToDecimal(i.Valor); break;
                         case "FechaInicioAguinaldo": this.dateFechaInicioAguinaldo.EditValue = Convert.ToDateTime(i.Valor); break;
                         case "FechaFinAguinaldo": this.dateFechaFinAguinaldo.EditValue = Convert.ToDateTime(i.Valor); break;
                         //NOMINA

                         //case "company_govid": this.tboxCompanyGovID.Text = i.Value.ToString(); break;
                         //case "authorization_code": this.tboxAuth.Text = i.Value.ToString(); break;
                         ///*Opciones de Facturacion*/
                         //case "MensajeInferior": this.tboxMensaje.Text = i.Value.ToString(); break;
                         //case "MostrarLogoFactura": this.checkmostrar.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "LogoX": this.numlogox.Text = i.Value.ToString(); break;
                         //case "LogoY": this.numlogoy.Text = i.Value.ToString(); break;
                         //case "FacturarEnMonedaPrincipal": this.checkFacturaCartaMonedaPrincipal.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "FacturarPapelCarta": this.checkFacturaCarta.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "MostrarFacturaDosMoneda": this.checkFacturarDosMonedas.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "QuitarFirmaCliente": this.checkFirmaTarjeta.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Opciones de Sistemas*/
                         //case "MensajeProforma": this.memProforma.Text = i.Value.ToString(); break;
                         //case "AcceptCreditCards": this.checkAceptarTarjetas.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "MaxComision": this.NumComision.Text = i.Value.ToString(); break;
                         //case "SelectedVendor": this.cboSelectedVendor.SelectedIndex = Convert.ToInt32(i.Value); break;
                         //case "AcceptChecksPayments": this.checkAceptarcheques.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "AcceptCredit": this.chkAceptarCredito.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "AcceptDiscountCards": this.chkAcceptDiscountCards.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "AcceptGiftCertificate": this.chkAcceptGiftCertificate.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "InventoryAlwaysAvailable": this.chkInventoryAlwaysAvailable.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "PrintDoubleBill": this.checkImprimirdoble.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "CodigoInventarioAuto": this.chkCodigoAutomaticoInv.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ProformaPrintReceipt": this.checkFactura.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "CodigoServicioAuto": this.chkCodigoServicioAutomatico.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UseGiftTable": this.chkGiftTable.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "GenerateBillForCertificate": this.chkGenerateBillForCertificate.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "WorkWithBoxColoration": this.chkWorkWithBoxColoration.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UseStationsWork": this.chkUseStationsWork.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ApplyTaxOnWorkOrder": this.chkApplyTaxOnWorkOrder.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "AllowSelectOtherRequisitions": this.chkAllowSelectOtherRequisitions.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UseSalesArea": this.chkUseSalesArea.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UseServiceSubcategory": this.chkUseServiceSubcategory.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "AllowRentTables": this.chkAllowRentTables.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Descuento*/
                         //case "ApplyDiscount":
                         //    this.chkDiscount.Checked = Convert.ToBoolean(i.Value.ToLower());
                         //    this.chkUsedAutomaticDiscount.Enabled = Convert.ToBoolean(i.Value.ToLower());
                         //    this.chkUsedDiscount.Enabled = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UsedDiscount": this.chkUsedDiscount.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "UsedAutomaticDiscount": this.chkUsedAutomaticDiscount.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Opciones Regionales*/
                         //case "Tax1Name": this.txttaxname1.Text = i.Value.ToString(); break;
                         //case "Tax1Value": this.numtax1.Text = i.Value.ToString(); break;
                         //case "Tax2Name": this.txttaxname2.Text = i.Value.ToString(); break;
                         //case "Tax2Value": this.numtax2.Text = i.Value.ToString(); break;
                         //case "Tax3Name": this.txttaxname3.Text = i.Value.ToString(); break;
                         //case "Tax3Value": this.numtax3.Text = i.Value.ToString(); break;
                         //case "MonedaPrincipal": this.cbo_monedaprincipal.SelectedValue = Convert.ToInt32(i.Value.ToLower()); break;
                         //case "MonedaSecundaria": this.cbo_monedasecundaria.SelectedValue = Convert.ToInt32(i.Value.ToLower()); break;
                         //case "PorcentajeServicio": this.txtTasaPropina.Text = i.Value.ToLower(); break;
                         //case "MetodoInventario": this.rbMetodoInventario.EditValue = i.Value; this.rbMetodoInventario.SelectedIndex = Convert.ToInt32(i.Value); break;

                         //case "RatePerNight":
                         //    this.rdTarifaPorNoche.EditValue = i.Value;
                         //    if (Convert.ToBoolean(i.Value) == true) this.rdTarifaPorNoche.SelectedIndex = 0;
                         //    else this.rdTarifaPorNoche.SelectedIndex = 1;
                         //    break;
                         //case "CheckInRestrictSystemDate": this.cheCheckInByFechaSistema.EditValue = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "CheckOutRestrictSystemDate": this.cheCheckOutByFechaSistema.EditValue = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "BillOnlyIfOutDateRoomsAreEqual": this.cheCheckOutSoloConFechasIguales.EditValue = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Opciones de Diseño Producto*/
                         //case "ShowToolTipProductToBilling": this.chkShowToolTipProductToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ShowWideProductsToBilling": this.chkShowWideProductsToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "WidthBtnProductToBilling": this.spWidthBtnProductToBilling.Text = i.Value.ToString(); break;
                         //case "HeightBtnPruductToBilling": this.spHeightBtnProductToBilling.Text = i.Value.ToString(); break;
                         //case "WidthBtnPictureProductToBilling": this.spWidthBtnPictureProductToBilling.Text = i.Value.ToString(); break;
                         //case "HeightBtnPictureProductToBilling": this.spHeightBtnPictureProductToBilling.Text = i.Value.ToString(); break;
                         //case "MaxCharBtnProductToBilling": this.spMaxCharBtnProductToBilling.Text = i.Value.ToString(); break;
                         //case "ShowPictureProductToBilling": this.chkShowPictureProductToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ViewProductImageOnList": this.ckViewProductImageOnList.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Opciones de Diseño Servicio*/
                         //case "ShowToolTipServiceToBilling": this.chkShowToolTipServiceToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ShowWideServicesToBilling": this.chkShowWideServicesToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "WidthBtnServiceToBilling": this.spWidthBtnServiceToBilling.Text = i.Value.ToString(); break;
                         //case "HeightBtnServiceToBilling": this.spHeightBtnServiceToBilling.Text = i.Value.ToString(); break;
                         //case "WidthBtnPictureServiceToBilling": this.spWidthBtnPictureServiceToBilling.Text = i.Value.ToString(); break;
                         //case "HeightBtnPictureServiceToBilling": this.spHeightBtnPictureServiceToBilling.Text = i.Value.ToString(); break;
                         //case "MaxCharBtnServiceToBilling": this.spMaxCharBtnServiceToBilling.Text = i.Value.ToString(); break;
                         //case "ShowPictureServiceToBilling": this.chkShowPictureServiceToBilling.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         //case "ViewServiceImageOnList": this.ckViewServiceImageOnList.Checked = Convert.ToBoolean(i.Value.ToLower()); break;
                         ///*Otras opciones de diseño*/
                         //case "QuantityTableResourcesPorPage": this.spQuantityTableResourcesPorPage.Text = i.Value.ToString(); break;
                         //case "QuantityVendorResourcesPorPage": this.spQuantityVendorResourcesPorPage.Text = i.Value.ToString(); break;
                         //case "WorkTimeStarBeautyRB": this.timeEditWorkTimeStarBeautyRB.EditValue = Convert.ToDateTime(i.Value.ToString()); break;
                         //case "WorkTimeEndBeautyRB": this.timeEditWorkTimeEndBeautyRB.EditValue = Convert.ToDateTime(i.Value.ToString()); break;
                         //case "TimeScaleBeautyRB": this.timeEditTimeScaleBeautyRB.EditValue = i.Value.ToString(); break;
                         //case "WorkTimeStarEwLightRB": this.timeEditWorkTimeStarEwLightRB.EditValue = Convert.ToDateTime(i.Value.ToString()); break;
                         //case "WorkTimeEndEwLightRB": this.timeEditWorkTimeEndEwLightRB.EditValue = Convert.ToDateTime(i.Value.ToString()); break;
                         //case "TimeScaleEwLightRB": this.timeEditTimeScaleEwLightRB.EditValue = i.Value.ToString(); break;
                         ///*Opciones Tipo / Tamaño del texto en tickets*/
                         //case "MaxWidthTicketPrinter": this.spMaxWidthTicketPrinter.Text = i.Value.ToString(); break;
                         //case "FontTitleS": this.spCFontTitleS.Text = i.Value.ToString(); break;
                         //case "MargenTittle": this.spMargenTittle.Text = i.Value.ToString(); break;
                         //case "FontTitleF": this.feFontTitleF.Text = i.Value.ToString(); break;
                         //case "FontSubTitleS": this.spCFontSubTitleS.Text = i.Value.ToString(); break;
                         //case "MargenSubTittle": this.spMargenSubTittle.Text = i.Value.ToString(); break;
                         //case "FontSubTitleF": this.feFontSubTitleF.Text = i.Value.ToString(); break;
                         //case "FontDataS": this.spFontDataS.Text = i.Value.ToString(); break;
                         //case "MargenDetail": this.spMargenDetail.Text = i.Value.ToString(); break;
                         //case "FontDataF": this.feFontDataF.Text = i.Value.ToString(); break;
                         //case "FontTotalS": this.spFontTotalS.Text = i.Value.ToString(); break;
                         //case "MargenTotal": this.spMargenTotal.Text = i.Value.ToString(); break;
                         //case "FontTotalF": this.feFontTotalF.Text = i.Value.ToString(); break;
                         //case "FontFooterS": this.spFontFooterS.Text = i.Value.ToString(); break;
                         //case "MargenFooter": this.spMargenFooter.Text = i.Value.ToString(); break;
                         //case "FontFooterF": this.feFontFooterF.Text = i.Value.ToString(); break;

                     }

                 }
             }
             catch (Exception ex)
             {
                 Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
             }
         }

        private void SetDataSystem()
        {
            try
            {
                Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 180;

                db.Configuracions.Single(c => c.Campo == "DisplayMessageDialog").Valor = this.chkMostrarCuadroInformativo.Checked.ToString();
                db.Configuracions.Single(c => c.Campo == "ProductoClaseCombustible").Valor = this.lkeClaseProducto.EditValue.ToString();
                db.Configuracions.Single(c => c.Campo == "RangoDiasPrecioCombustible").Valor = this.speRangoDiasPrecioCombustible.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "VariacionPorcentualPrecio").Valor = this.speVariacionPorcentualPrecio.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "DiferenciaVerificacionLectura").Valor = this.speDiferenciaVerificacionLectura.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "DescuentoPetroCard").Valor = this.speDescuentoPetroCard.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "RangoEfectivo").Valor = this.spRangoEfectivo.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "RangoMecElectronico").Valor = this.spRangoMecElectronico.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "MargenValorCambioIVA").Valor = this.spMargenIVA.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "MargenValorCambioTC").Valor = this.spMargenTC.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "FechaCierreActa").Valor = Convert.ToDateTime(this.dateFechaCierreActa.EditValue).ToShortDateString();
                //INVENTARIO
                db.Configuracions.Single(c => c.Campo == "ClientePrestamoManejoID").Valor =this.lkClienteManejadorPrestamo.EditValue.ToString();
                db.Configuracions.Single(c => c.Campo == "ProveedorPrestamoManejoID").Valor = this.lkProveedorManejadorPrestamo.EditValue.ToString();
                //NOMINA*************
                db.Configuracions.Single(c => c.Campo == "ValorINSSLaboral").Valor = this.spValorINSSLaboral.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "ValorINSSPatronal").Valor = this.spValorINSSPatronal.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "ValorINATEC").Valor = this.spValorINATEC.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "MaximoSalarioINSS").Valor = this.spMaximoSalarioINSS.Value.ToString();
                db.Configuracions.Single(c => c.Campo == "FechaInicioAguinaldo").Valor = Convert.ToDateTime(this.dateFechaInicioAguinaldo.EditValue).ToShortDateString();
                db.Configuracions.Single(c => c.Campo == "FechaFinAguinaldo").Valor = Convert.ToDateTime(this.dateFechaFinAguinaldo.EditValue).ToShortDateString();
                //NOMINA*************
                

                //db.customizes.Single(c => c.Field == "company_govid").Value = this.tboxCompanyGovID.Text;
                //db.customizes.Single(c => c.Field == "authorization_code").Value = this.tboxAuth.Text;

                //db.customizes.Single(c => c.Field == "MensajeInferior").Value = this.tboxMensaje.Text;
                //db.customizes.Single(c => c.Field == "MostrarLogoFactura").Value = this.checkmostrar.Checked.ToString();
                //db.customizes.Single(c => c.Field == "LogoX").Value = this.numlogox.Text;
                //db.customizes.Single(c => c.Field == "LogoY").Value = this.numlogoy.Text;
                //db.customizes.Single(c => c.Field == "FacturarEnMonedaPrincipal").Value = this.checkFacturaCartaMonedaPrincipal.Checked.ToString();
                //db.customizes.Single(c => c.Field == "FacturarPapelCarta").Value = this.checkFacturaCarta.Checked.ToString();
                //db.customizes.Single(c => c.Field == "MostrarFacturaDosMoneda").Value = this.checkFacturarDosMonedas.Checked.ToString();
                //db.customizes.Single(c => c.Field == "QuitarFirmaCliente").Value = this.checkFirmaTarjeta.Checked.ToString();
                //db.customizes.Single(c => c.Field == "MensajeProforma").Value = this.memProforma.Text;
                //db.customizes.Single(c => c.Field == "AcceptCreditCards").Value = this.checkAceptarTarjetas.Checked.ToString();
                //db.customizes.Single(c => c.Field == "MaxComision").Value = this.NumComision.Text;
                //db.customizes.Single(c => c.Field == "AcceptChecksPayments").Value = this.checkAceptarcheques.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ApplyDiscount").Value = this.chkDiscount.Checked.ToString();
                //db.customizes.Single(c => c.Field == "UsedDiscount").Value = this.chkUsedDiscount.Checked.ToString();
                //db.customizes.Single(c => c.Field == "UsedAutomaticDiscount").Value = this.chkUsedAutomaticDiscount.Checked.ToString();
                //db.customizes.Single(c => c.Field == "UseGiftTable").Value = this.chkGiftTable.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AcceptCredit").Value = this.chkAceptarCredito.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AcceptDiscountCards").Value = this.chkAcceptDiscountCards.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AcceptGiftCertificate").Value = this.chkAcceptGiftCertificate.Checked.ToString();
                //db.customizes.Single(c => c.Field == "InventoryAlwaysAvailable").Value = this.chkInventoryAlwaysAvailable.Checked.ToString();
                //db.customizes.Single(c => c.Field == "PrintDoubleBill").Value = this.checkImprimirdoble.Checked.ToString();
                //db.customizes.Single(c => c.Field == "GenerateBillForCertificate").Value = this.chkGenerateBillForCertificate.Checked.ToString();
                //db.customizes.Single(c => c.Field == "WorkWithBoxColoration").Value = this.chkWorkWithBoxColoration.Checked.ToString();
                //db.customizes.Single(c => c.Field == "UseStationsWork").Value = this.chkUseStationsWork.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ApplyTaxOnWorkOrder").Value = this.chkApplyTaxOnWorkOrder.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AllowSelectOtherRequisitions").Value = this.chkAllowSelectOtherRequisitions.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AllowSelectOtherRequisitions").Value = this.chkAllowSelectOtherRequisitions.Checked.ToString();
                //db.customizes.Single(c => c.Field == "AllowRentTables").Value = this.chkAllowRentTables.Checked.ToString();
                //db.customizes.Single(c => c.Field == "UseServiceSubcategory").Value = this.chkUseServiceSubcategory.Checked.ToString();
                //db.customizes.Single(c => c.Field == "CodigoInventarioAuto").Value = this.chkCodigoAutomaticoInv.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ProformaPrintReceipt").Value = this.checkFactura.Checked.ToString();
                //db.customizes.Single(c => c.Field == "CodigoServicioAuto").Value = this.chkCodigoServicioAutomatico.Checked.ToString();
                //db.customizes.Single(c => c.Field == "Tax1Name").Value = this.txttaxname1.Text;
                //db.customizes.Single(c => c.Field == "Tax1Value").Value = this.numtax1.Text;
                //db.customizes.Single(c => c.Field == "Tax2Name").Value = this.txttaxname2.Text;
                //db.customizes.Single(c => c.Field == "Tax2Value").Value = this.numtax2.Text;
                //db.customizes.Single(c => c.Field == "Tax3Name").Value = this.txttaxname3.Text;
                //db.customizes.Single(c => c.Field == "Tax3Value").Value = this.numtax3.Text;
                //db.customizes.Single(c => c.Field == "SelectedVendor").Value = this.cboSelectedVendor.SelectedIndex.ToString();
                //db.customizes.Single(c => c.Field == "MonedaPrincipal").Value = this.cbo_monedaprincipal.SelectedValue.ToString();
                //db.customizes.Single(c => c.Field == "MonedaSecundaria").Value = this.cbo_monedasecundaria.SelectedValue.ToString();

                //db.customizes.Single(c => c.Field == "PorcentajeServicio").Value = this.txtTasaPropina.Text;
                //db.customizes.Single(c => c.Field == "MetodoInventario").Value = this.rbMetodoInventario.EditValue.ToString();

                //db.customizes.Single(c => c.Field == "RatePerNight").Value = this.rdTarifaPorNoche.EditValue.ToString();
                //db.customizes.Single(c => c.Field == "CheckInRestrictSystemDate").Value = Convert.ToBoolean(this.cheCheckInByFechaSistema.EditValue).ToString();
                //db.customizes.Single(c => c.Field == "CheckOutRestrictSystemDate").Value = Convert.ToBoolean(this.cheCheckOutByFechaSistema.EditValue).ToString();
                //db.customizes.Single(c => c.Field == "BillOnlyIfOutDateRoomsAreEqual").Value = Convert.ToBoolean(this.cheCheckOutSoloConFechasIguales.EditValue).ToString();
                ///*Opciones de Diseño Producto*/
                //db.customizes.Single(c => c.Field == "ShowToolTipProductToBilling").Value = this.chkShowToolTipProductToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ShowWideProductsToBilling").Value = this.chkShowWideProductsToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "WidthBtnProductToBilling").Value = this.spWidthBtnProductToBilling.Text;
                //db.customizes.Single(c => c.Field == "HeightBtnPruductToBilling").Value = this.spHeightBtnProductToBilling.Text;
                //db.customizes.Single(c => c.Field == "WidthBtnPictureProductToBilling").Value = this.spWidthBtnPictureProductToBilling.Text;
                //db.customizes.Single(c => c.Field == "HeightBtnPictureProductToBilling").Value = this.spHeightBtnPictureProductToBilling.Text;
                //db.customizes.Single(c => c.Field == "MaxCharBtnProductToBilling").Value = this.spMaxCharBtnProductToBilling.Text;
                //db.customizes.Single(c => c.Field == "ShowPictureProductToBilling").Value = this.chkShowPictureProductToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ViewProductImageOnList").Value = this.ckViewProductImageOnList.Checked.ToString();
                ///*Opciones de Diseño Servicio*/
                //db.customizes.Single(c => c.Field == "ShowToolTipServiceToBilling").Value = this.chkShowToolTipServiceToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ShowWideServicesToBilling").Value = this.chkShowWideServicesToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "WidthBtnServiceToBilling").Value = this.spWidthBtnServiceToBilling.Text;
                //db.customizes.Single(c => c.Field == "HeightBtnServiceToBilling").Value = this.spHeightBtnServiceToBilling.Text;
                //db.customizes.Single(c => c.Field == "WidthBtnPictureServiceToBilling").Value = this.spWidthBtnPictureServiceToBilling.Text;
                //db.customizes.Single(c => c.Field == "HeightBtnPictureServiceToBilling").Value = this.spHeightBtnPictureServiceToBilling.Text;
                //db.customizes.Single(c => c.Field == "MaxCharBtnServiceToBilling").Value = this.spMaxCharBtnServiceToBilling.Text;
                //db.customizes.Single(c => c.Field == "ShowPictureServiceToBilling").Value = this.chkShowPictureServiceToBilling.Checked.ToString();
                //db.customizes.Single(c => c.Field == "ViewServiceImageOnList").Value = this.ckViewServiceImageOnList.Checked.ToString();
                ///*Otras opciones de diseño*/
                //db.customizes.Single(c => c.Field == "QuantityTableResourcesPorPage").Value = this.spQuantityTableResourcesPorPage.Text;
                //db.customizes.Single(c => c.Field == "QuantityVendorResourcesPorPage").Value = this.spQuantityVendorResourcesPorPage.Text;
                //db.customizes.Single(c => c.Field == "WorkTimeStarBeautyRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditWorkTimeStarBeautyRB.EditValue));
                //db.customizes.Single(c => c.Field == "WorkTimeEndBeautyRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditWorkTimeEndBeautyRB.EditValue));
                //db.customizes.Single(c => c.Field == "TimeScaleBeautyRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditTimeScaleBeautyRB.EditValue));
                //db.customizes.Single(c => c.Field == "WorkTimeStarEwLightRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditWorkTimeStarEwLightRB.EditValue));
                //db.customizes.Single(c => c.Field == "WorkTimeEndEwLightRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditWorkTimeEndEwLightRB.EditValue));
                //db.customizes.Single(c => c.Field == "TimeScaleEwLightRB").Value = String.Format("{0:HH:mm:ss}", Convert.ToDateTime(this.timeEditTimeScaleEwLightRB.EditValue));
                ///*Opciones Tipo / Tamaño del texto en tickets*/
                //db.customizes.Single(c => c.Field == "MaxWidthTicketPrinter").Value = this.spMaxWidthTicketPrinter.Text;
                //db.customizes.Single(c => c.Field == "FontTitleS").Value = this.spCFontTitleS.Text;
                //db.customizes.Single(c => c.Field == "MargenTittle").Value = this.spMargenTittle.Text;
                //db.customizes.Single(c => c.Field == "FontTitleF").Value = this.feFontTitleF.Text;
                //db.customizes.Single(c => c.Field == "FontSubTitleS").Value = this.spCFontSubTitleS.Text;
                //db.customizes.Single(c => c.Field == "MargenSubTittle").Value = this.spMargenSubTittle.Text;
                //db.customizes.Single(c => c.Field == "FontSubTitleF").Value = this.feFontSubTitleF.Text;
                //db.customizes.Single(c => c.Field == "FontDataS").Value = this.spFontDataS.Text;
                //db.customizes.Single(c => c.Field == "MargenDetail").Value = this.spMargenDetail.Text;
                //db.customizes.Single(c => c.Field == "FontDataF").Value = this.feFontDataF.Text;
                //db.customizes.Single(c => c.Field == "FontTotalS").Value = this.spFontTotalS.Text;
                //db.customizes.Single(c => c.Field == "MargenTotal").Value = this.spMargenTotal.Text;
                //db.customizes.Single(c => c.Field == "FontTotalF").Value = this.feFontTotalF.Text;
                //db.customizes.Single(c => c.Field == "FontFooterS").Value = this.spFontTotalS.Text;
                //db.customizes.Single(c => c.Field == "MargenFooter").Value = this.spMargenFooter.Text;
                //db.customizes.Single(c => c.Field == "FontFooterF").Value = this.feFontTotalF.Text;

                db.SubmitChanges();

                foreach (Entidad.ArqueoTipoCambio i in ArqueosTC)
                {
                    if (db.ArqueoTipoCambios.Count(c => c.Equals(i)).Equals(0))
                    {
                        db.ArqueoTipoCambios.InsertOnSubmit(i);
                        db.SubmitChanges();
                    }
                }
                

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

            }



        }

        private bool Guardar()
        {
            try
            {
                SetDataSystem();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return false;
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            {
                if (!Guardar()) return;

                this.Close();
            }
        }
          
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvDataTC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }
                if (e.KeyCode == Keys.Delete)
                {

                    if (gvDataTC.FocusedRowHandle >= 0)
                    {
                        int _id = Convert.ToInt32(gvDataTC.GetFocusedRowCellValue("ID"));
                        Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        foreach (Entidad.ArqueoTipoCambio i in ArqueosTC)
                        {
                            if (i.ID == _id)
                            {
                                Entidad.ArqueoTipoCambio tc = db.ArqueoTipoCambios.Single(o => o.ID.Equals(_id));
                                ArqueosTC.Remove(i);
                                db.ArqueoTipoCambios.DeleteOnSubmit(tc);
                                db.SubmitChanges();
                                break;
                            }
                        }
                        gvDataTC.RefreshData();

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