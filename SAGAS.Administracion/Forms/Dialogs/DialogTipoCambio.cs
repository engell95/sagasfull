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
using SAGAS.Administracion.Forms;
using System.IO;
using System.Reflection;
using DevExpress.XtraBars;
using System.Text.RegularExpressions;

namespace SAGAS.Administracion.Forms.Dialogs
{
    public partial class DialogTipoCambio : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormTipoCambio MDI;
        internal Entidad.TipoCambio EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public int IDMes
        {
            get { return Convert.ToInt32(lkeMes.EditValue); }
            set { lkeMes.EditValue = value; }
        }

        public int Ano
        {
            get { return Convert.ToInt32(spAno.Value); }
            set { spAno.Value = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogTipoCambio()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                Parametros.ListMeses listado = new Parametros.ListMeses();
                lkeMes.Properties.DataSource = listado.GetListMeses();
                lkeMes.Properties.DisplayMember = "Name";
                lkeMes.Properties.ValueMember = "ID";

                if (Editable)
                {
                    IDMes = EntidadAnterior.Fecha.Month;
                    Ano = EntidadAnterior.Fecha.Year;
                    spAno.Properties.ReadOnly = true;
                    lkeMes.Properties.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private bool Guardar()
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.CommandTimeout = 120;
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    for (int i = 0; i < gvData.RowCount; i++)
                    {
                        Entidad.TipoCambio TC;

                        if (Editable)
                        { TC = db.TipoCambios.Single(e => e.ID.Equals(Convert.ToInt32(gvData.GetRowCellValue(i, colID)))); }
                        else
                        {
                            TC = new Entidad.TipoCambio();
                            TC.MonedaPrimariaID = Parametros.Config.MonedaPrincipal();
                            TC.MonedaSecundariaID = Parametros.Config.MonedaSecundaria();
                        }

                        TC.Fecha = Convert.ToDateTime(gvData.GetRowCellValue(i, colFecha));
                        TC.Valor = Decimal.Round(Convert.ToDecimal(gvData.GetRowCellValue(i, colValor)), 4, MidpointRounding.AwayFromZero);


                        if (!Editable)
                        {
                            db.TipoCambios.InsertOnSubmit(TC);
                        }
                    }

                    if (!Editable)
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Administrativa,
                                "Se creó el Tipo de Cambio para : " + lkeMes.Text + spAno.Value.ToString(), this.Name);

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

        private void spAno_EditValueChanged(object sender, EventArgs e)
        {
            lkeMes_EditValueChanged(null, null);
        }

        private void lkeMes_EditValueChanged(object sender, EventArgs e)
        {
            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            if (db.TipoCambios.Count(tc => tc.Fecha.Year.Equals(Ano) && tc.Fecha.Month.Equals(IDMes)) > 0)
                bdsDetalle.DataSource = db.TipoCambios.Where(tc => tc.Fecha.Year.Equals(Ano) && tc.Fecha.Month.Equals(IDMes));
            else
            {
                List<Entidad.TipoCambio> TP = new List<Entidad.TipoCambio>();

                DateTime fechaInicial = Convert.ToDateTime(new DateTime(Ano, IDMes, 1));
                DateTime fechaFinal = fechaInicial.AddMonths(1).AddDays(-1);

                for (int i = 0; i < fechaFinal.Day; i++)
                {
                    TP.Add(new Entidad.TipoCambio { Fecha = fechaInicial.AddDays(i), Valor = 0.0000m });
                }

                bdsDetalle.DataSource = TP;
            }           

        }

        private void gvData_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            ShowPupUpMenu();
        }

        private void menuButtonCopiar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gvData.CopyToClipboard();
        }

        private void menuButtonPegar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var filas = gvData.GetSelectedRows();
                string lista = Clipboard.GetText();

                string[] lineas = Regex.Split(lista, "\r\n");


                int i = 0;
                foreach (int obj in filas)
                {
                    int j = 0;
                    foreach (string item in lineas.ElementAt(i).Split('\t'))
                    {

                        gvData.SetRowCellValue(obj, gvData.GetSelectedCells(obj).ElementAt(j), item.ToString());
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