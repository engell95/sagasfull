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
using SAGAS.Ventas.Forms;
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;
using DevExpress.XtraGrid.Views.Grid;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogArqueoRetiro : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private DataTable dtAMonedas;
        private DataTable dtBBilletes;
        private DataTable dtCBilletes;
        private DataTable dtCheques;
        private int IDAE = 0;
        public decimal TipoCambioRD;
        private int MonedaPrincipal = Parametros.Config.MonedaPrincipal();
        private int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int IDES = Parametros.General.EstacionServicioID;
        public Entidad.ArqueoEfectivoRetiro EtArqueo;
        internal bool EsEspecial = false;
        private int _monedaID = 0;

        public DataTable AMonedas
        {
            get
            {
                return dtAMonedas;
            }
            set
            {
                dtAMonedas = value;
            }
        }

        public DataTable BBilletes
        {
            get
            {
                return dtBBilletes;
            }
            set
            {
                dtBBilletes = value;
            }
        }

        public DataTable CBilletes
        {
            get
            {
                return dtCBilletes;
            }
            set
            {
                dtCBilletes = value;
            }
        }

        public DataTable Cheques
        {
            get
            {
                return dtCheques;
            }
            set          {
                dtCheques = value;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogArqueoRetiro(int MonedaID)
        {
            InitializeComponent();
            _monedaID = MonedaID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            if (FillControl())
                this.panelControlCenter.Location = new Point(2, 5);
            else
                this.BeginInvoke(new MethodInvoker(this.Close)); 
        }

        #endregion

        #region *** METODOS ***

        private bool FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                if (_monedaID.Equals(1))
                    layoutControlGroupDolar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                else
                    layoutControlGroupCor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;


                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
               
                spTipoCambio.Value = TipoCambioRD;

                        //spTotalArqueo.Value = 

                        #region <<< A-MONEDAS >>>

                        AMonedas = new DataTable();

                        AMonedas.Columns.Add("IDA", typeof(Int32));
                        AMonedas.Columns.Add("Valor", typeof(String));
                        AMonedas.Columns.Add("Formula", typeof(String));
                        AMonedas.Columns.Add("Denominacion", typeof(String));
                        AMonedas.Columns.Add("Equivalente", typeof(Decimal));
                        AMonedas.Columns.Add("TotalEfectivo", typeof(Decimal));

                //foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                //{
                //    AMonedas.Rows.Add(objA.ID, "", "", objA.Denominacion, objA.Equivalente, 0m);
                //}

                foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                {
                    //var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objA.ID));
                    var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(objA.ID));
                    if (AEDAnterior.Count() > 0)
                        AMonedas.Rows.Add(objA.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objA.Denominacion, objA.Equivalente, AEDAnterior.First().TotalEfectivo);
                    else
                        AMonedas.Rows.Add(objA.ID, "", "", objA.Denominacion, objA.Equivalente, 0m);
                }

                        gridAMonedas.DataSource = AMonedas;
                        gvDataAMonedas.RefreshData();

                        #endregion

                        #region <<< B-BILLETES >>>

                        BBilletes = new DataTable();

                        BBilletes.Columns.Add("IDB", typeof(Int32));
                        BBilletes.Columns.Add("Valor", typeof(String));
                        BBilletes.Columns.Add("Formula", typeof(String));
                        BBilletes.Columns.Add("Denominacion", typeof(String));
                        BBilletes.Columns.Add("Equivalente", typeof(Decimal));
                        BBilletes.Columns.Add("TotalEfectivo", typeof(Decimal));

                //foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                //{
                //    BBilletes.Rows.Add(objB.ID, "", "", objB.Denominacion, objB.Equivalente, 0m);
                //}

                foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                {
                    //var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objB.ID));
                    var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(objB.ID));
                    if (AEDAnterior.Count() > 0)
                        BBilletes.Rows.Add(objB.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objB.Denominacion, objB.Equivalente, AEDAnterior.First().TotalEfectivo);
                    else
                        BBilletes.Rows.Add(objB.ID, "", "", objB.Denominacion, objB.Equivalente, 0m);
                }

                        gridBBilletes.DataSource = BBilletes;
                        gvDataBBilletes.RefreshData();

                        #endregion

                        #region <<< C-BILLETES >>>

                        CBilletes = new DataTable();

                        CBilletes.Columns.Add("IDC", typeof(Int32));
                        CBilletes.Columns.Add("Valor", typeof(String));
                        CBilletes.Columns.Add("Formula", typeof(String));
                        CBilletes.Columns.Add("Denominacion", typeof(String));
                        CBilletes.Columns.Add("Equivalente", typeof(Decimal));
                        CBilletes.Columns.Add("TotalEfectivo", typeof(Decimal));

                //foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                //{
                //    CBilletes.Rows.Add(objC.ID, "", "", objC.Denominacion, objC.Equivalente, 0m);
                //}

                foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                {
                    //var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objC.ID));
                    var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(objC.ID));

                    if (AEDAnterior.Count() > 0)
                        CBilletes.Rows.Add(objC.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objC.Denominacion, objC.Equivalente, AEDAnterior.First().TotalEfectivo);
                    else
                        CBilletes.Rows.Add(objC.ID, "", "", objC.Denominacion, objC.Equivalente, 0m);
                }

                        //this.layoutControlGroupDolar.Text = "C - Billetes Dólares Tipo de Cambio " + TipoCambioRD.ToString("#,0.00");
                        gridCBilletes.DataSource = CBilletes;
                        gvDataCBilletes.RefreshData();

                        #endregion
                
                        CalculosTotales();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        this.btnOK.Enabled = true;
                        return true;
                
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }

        private bool Guardar()
        {
            try
            {
                Entidad.ArqueoEfectivoRetiro AE;

                AE = new Entidad.ArqueoEfectivoRetiro();
                AE.UsuarioCreado = Parametros.General.UserID;
                AE.FechaCreado = DateTime.Now.Date;

                AE.ArqueoEfectivoRetiroDetalles.Clear();

                #region <<< AMONEDAS >>>
                foreach (DataRow ra in AMonedas.Rows)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(ra["TotalEfectivo"])))
                    {
                        if (Convert.ToDecimal(ra["TotalEfectivo"]) > 0)
                        {
                            var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(Convert.ToInt32(ra["IDA"])));

                            if (AEDAnterior.Count() > 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = AEDAnterior.First();

                                AED.EfectivoID = Convert.ToInt32(ra["IDA"]);
                                AED.TotalEfectivo = Convert.ToDecimal(ra["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(ra["Valor"]);
                                AED.Formula = Convert.ToString(ra["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                                //db.SubmitChanges();
                            }
                            else if (AEDAnterior.Count() <= 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = new Entidad.ArqueoEfectivoRetiroDetalle();
                                AED.EfectivoID = Convert.ToInt32(ra["IDA"]);
                                AED.TotalEfectivo = Convert.ToDecimal(ra["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(ra["Valor"]);
                                AED.Formula = Convert.ToString(ra["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                            }
                        }
                        //else if (String.IsNullOrEmpty(Convert.ToString(ra["TotalEfectivo"])) || Convert.ToDecimal(ra["TotalEfectivo"]) <= 0)
                        //{
                        //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(1) && x.EfectivoID.Equals(Convert.ToInt32(ra["IDA"])));

                        //    if (AEDAnterior.Count() > 0)
                        //    {
                        //        Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                        //        db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                        //        db.SubmitChanges();
                        //    }

                        //}

                    }

                }
                #endregion

                #region <<< BCORDOBAS >>>
                foreach (DataRow rb in BBilletes.Rows)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(rb["TotalEfectivo"])))
                    {
                        if (Convert.ToDecimal(rb["TotalEfectivo"]) > 0)
                        {
                            var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(Convert.ToInt32(rb["IDB"])));

                            if (AEDAnterior.Count() > 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = AEDAnterior.First();

                                AED.EfectivoID = Convert.ToInt32(rb["IDB"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rb["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(rb["Valor"]);
                                AED.Formula = Convert.ToString(rb["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                            }
                            else if (AEDAnterior.Count() <= 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = new Entidad.ArqueoEfectivoRetiroDetalle();
                                AED.EfectivoID = Convert.ToInt32(rb["IDB"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rb["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(rb["Valor"]);
                                AED.Formula = Convert.ToString(rb["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                            }
                        }
                        //else if (String.IsNullOrEmpty(Convert.ToString(rb["TotalEfectivo"])) || Convert.ToDecimal(rb["TotalEfectivo"]) <= 0)
                        //{
                        //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(1) && x.EfectivoID.Equals(Convert.ToInt32(rb["IDB"])));

                        //    if (AEDAnterior.Count() > 0)
                        //    {
                        //        Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                        //        db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                        //        db.SubmitChanges();
                        //    }

                        //}

                    }

                }
                #endregion

                #region <<< CDOLAR >>>
                foreach (DataRow rc in CBilletes.Rows)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(rc["TotalEfectivo"])))
                    {
                        if (Convert.ToDecimal(rc["TotalEfectivo"]) > 0)
                        {
                            var AEDAnterior = EtArqueo.ArqueoEfectivoRetiroDetalles.Where(x => x.EfectivoID.Equals(Convert.ToInt32(rc["IDC"])));

                            if (AEDAnterior.Count() > 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = AEDAnterior.First();

                                AED.EfectivoID = Convert.ToInt32(rc["IDC"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rc["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(rc["Valor"]);
                                AED.Formula = Convert.ToString(rc["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                            }
                            else if (AEDAnterior.Count() <= 0)
                            {
                                Entidad.ArqueoEfectivoRetiroDetalle AED = new Entidad.ArqueoEfectivoRetiroDetalle();
                                AED.EfectivoID = Convert.ToInt32(rc["IDC"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rc["TotalEfectivo"]);
                                AED.Valor = Convert.ToDecimal(rc["Valor"]);
                                AED.Formula = Convert.ToString(rc["Formula"]);
                                AE.ArqueoEfectivoRetiroDetalles.Add(AED);
                            }
                        }
                        //else if (String.IsNullOrEmpty(Convert.ToString(rc["TotalEfectivo"])) || Convert.ToDecimal(rc["TotalEfectivo"]) <= 0)
                        //{
                        //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(1) && x.EfectivoID.Equals(Convert.ToInt32(rc["IDC"])));

                        //    if (AEDAnterior.Count() > 0)
                        //    {
                        //        Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                        //        db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                        //        db.SubmitChanges();
                        //    }

                        //}

                    }

                }

                #endregion

                EtArqueo = AE;
                btnOK.Enabled = false;
                return true;
            }

            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }
        }

        private bool EsFormulaCorrecta(string OriginalString)
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

        private decimal ValorFormula(string OriginalString)
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

        decimal FractionToDouble(string fraction)
        {
            decimal result;

            if (decimal.TryParse(fraction, out result))
            {
                return result;
            }

            string[] split = fraction.Split(new char[] { '*', '/' });

            if (split.Length == 2)
            {
                int a, b;


                if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
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

        private void CalculosTotales()
        {
            try
            {
                spTotalCordoba.Value = Decimal.Round(Convert.ToDecimal(gvDataAMonedas.Columns["TotalEfectivo"].SummaryText) + Convert.ToDecimal(gvDataBBilletes.Columns["TotalEfectivo"].SummaryText), 2, MidpointRounding.AwayFromZero);
                spEquivalenteCordobas.Value = Decimal.Round(Convert.ToDecimal(gvDataCBilletes.Columns["TotalEfectivo"].SummaryText) * TipoCambioRD, 2, MidpointRounding.AwayFromZero);
             
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.DialogResult = DialogResult.OK;
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MDI.CleanDialog(ShowMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region <<< EventosDCHEQUES >>>

        private void gvDataAMonedas_MouseWheel(object sender, MouseEventArgs e)
        {
            // If the mouse wheel delta is positive, move the box up. 
            if (e.Delta > 0)
            {
                this.xtraScrollableControlMain.VerticalScroll.Value -= 20;
            }

            // If the mouse wheel delta is negative, move the box down. 
            if (e.Delta < 0)
            {
                this.xtraScrollableControlMain.VerticalScroll.Value += 20;
            }
        }


        #endregion

        #region <<< EventosAMONEDAS >>>

        private void rpMemoA_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataAMonedas.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataAMonedas.SetFocusedRowCellValue("Formula", Operacion);
                        
                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataAMonedas.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataAMonedas.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataAMonedas.RefreshData();
                        CalculosTotales();
                    } 
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataAMonedas.SetFocusedRowCellValue("Formula", "");
                    gvDataAMonedas.SetFocusedRowCellValue("Valor", "");
                    gvDataAMonedas.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataAMonedas.RefreshData();
                    CalculosTotales();
                } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoA_Popup(object sender, EventArgs e)
        {
            try
            {  
                if (gvDataAMonedas.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                                                                                    
                    me.Text = Convert.ToString(gvDataAMonedas.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region <<< EventosBBILLETES >>>

        private void rpMemoB_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataBBilletes.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataBBilletes.SetFocusedRowCellValue("Formula", Operacion);

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataBBilletes.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataBBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataBBilletes.RefreshData();
                        CalculosTotales();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataBBilletes.SetFocusedRowCellValue("Formula", "");
                    gvDataBBilletes.SetFocusedRowCellValue("Valor", "");
                    gvDataBBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataBBilletes.RefreshData();
                    CalculosTotales();
                } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoB_Popup(object sender, EventArgs e)
        {
            try
            {  
                if (gvDataBBilletes.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                    me.Text = Convert.ToString(gvDataBBilletes.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< EventosCBILLETES >>>

        private void rpMemoC_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataCBilletes.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataCBilletes.SetFocusedRowCellValue("Formula", Operacion);

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataCBilletes.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataCBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataCBilletes.RefreshData();
                        CalculosTotales();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataCBilletes.SetFocusedRowCellValue("Formula", "");
                    gvDataCBilletes.SetFocusedRowCellValue("Valor", "");
                    gvDataCBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataCBilletes.RefreshData();
                    CalculosTotales();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoC_Popup(object sender, EventArgs e)
        {
            try
            {
                if (gvDataCBilletes.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                    me.Text = Convert.ToString(gvDataCBilletes.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion   

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.previewBarTop.Visible = false;
                rv.Text = "Vista Previa Arqueo Efectivo";
                //Reportes.Util.PrintArqueoEfectivo(this, dbView, db, ArqueoEfectivoSelected, EtTurno, EtResumenDia, Parametros.TiposImpresion.Vista_Previa, false, Parametros.Properties.Resources.TXTVISTAPREVIA, rv.printControlAreaReport);
                rv.ShowDialog();

            }
            catch (Exception ex)
            {
                //Parametros.General.splashScreenManagerMain.CloseWaitForm();
                //this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        private void spTotalCordoba_EditValueChanged(object sender, EventArgs e)
        {
            if (btnOK.Enabled.Equals(false))
                btnOK.Enabled = true;
        }

        private void DialogArqueoEfectivo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnOK.Enabled.Equals(true))
            {
                DialogResult resultado;

                resultado = Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGMODIFICADOCLOSING + Environment.NewLine, Parametros.MsgType.questionNO);

                if (resultado == DialogResult.Cancel)
                    e.Cancel = true;
                else if (resultado == DialogResult.OK)
                {
                    if (!Guardar())
                    {
                        this.Cursor = Cursors.Default;
                        e.Cancel = true;
                    }
                }
            }
        }

     
    }


}