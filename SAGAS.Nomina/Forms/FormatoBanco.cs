using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormatoBanco : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataViewsDataContext dv;
        private List<Entidad.VistaFormatoCuentaTarjeta> EtFormato;
        public int _ID;
        private string _Nombre;
        private bool EsAguinaldo;

        #endregion

        #region <<< INICIO >>>

        public FormatoBanco(bool Aguinaldo)
        {
            InitializeComponent();
            EsAguinaldo = Aguinaldo;
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        private void FillControl()
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                if (EsAguinaldo)
                {
                    EtFormato = (from v in dv.VistaFormatoCuentaTarjetaAguinaldos
                                 where v.ID == Convert.ToInt32(_ID)
                                 select new Entidad.VistaFormatoCuentaTarjeta
                                 {
                                     ID = v.ID,
                                     EmpleadoID = v.EmpleadoID,
                                     Concepto = v.Concepto,
                                     NoCuenta = v.NoCuenta,
                                     NombreCompleto = v.NombreCompleto,
                                     PlanillaID = v.PlanillaID,
                                     Total = v.Total
                                 }).ToList();

                    this.grid.DataSource = EtFormato;
                    this.gvData.RefreshData();

                    var pl = dv.VistaPagoAguinaldos.FirstOrDefault(s => s.ID.Equals(_ID));
                    if (pl != null)
                    {
                        _Nombre = pl.NombrePlanilla;
                        emptyLabel.Text = Parametros.General.Empresa.Nombre + " Planilla " + Convert.ToString(pl.NombrePlanilla) +
                                         ", formato banco periodo del " + pl.FechaInicio.Day.ToString() + " al "
                                         + pl.FechaFin.Day.ToString() + " de "
                                           + Parametros.General.GetMonthInLetters(pl.FechaFin.Month) + " del"
                                           + pl.FechaFin.Year.ToString();
                    }
                }
                else
                {
                    EtFormato = dv.VistaFormatoCuentaTarjeta.Where(p => p.ID == Convert.ToInt32(_ID)).ToList();
                    this.grid.DataSource = EtFormato;
                    this.gvData.RefreshData();

                    var pl = dv.VistaPlanillaGenerada.FirstOrDefault(s => s.ID.Equals(_ID));
                    _Nombre = pl.PlanillaNombre;
                    emptyLabel.Text = Parametros.General.Empresa.Nombre + " Planilla " + Convert.ToString(pl.PlanillaNombre) +
                                     ", formato banco periodo del " + pl.FechaDesde.Day.ToString() + " al "
                                     + pl.FechaHasta.Day.ToString() + " de "
                                       + Parametros.General.GetMonthInLetters(pl.FechaHasta.Month) + " del"
                                       + pl.FechaHasta.Year.ToString();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        
              
        #endregion

        private void btnToExecl_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                dglExportToFile.Filter = "Microsoft Excel (*.xlsx)|*.xlsx";

                if (dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (dglExportToFile.FileName != "")
                    {
                        gvData.ExportToXlsx(dglExportToFile.FileName);

                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnToTxt_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                dglExportToFile.Filter = "Bloc de Nota (*.txt)|*.txt";

                if (dglExportToFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    
                    if (dglExportToFile.FileName != "")
                    {
                        progressBarCont.Properties.Maximum = (int)(gvData.RowCount * gvData.VisibleColumns.Count);
                        progressBarCont.EditValue = 0;
                        string route = dglExportToFile.FileName.Replace("\\", "/");

                        using (StreamWriter sw = File.CreateText(route))
                        {
                            for (int i = 0; i < gvData.RowCount; i++)
                            {
                                string texto = "";
                                foreach (DevExpress.XtraGrid.Columns.GridColumn col in gvData.VisibleColumns)
                                {
                                    if (String.IsNullOrEmpty(texto))
                                        texto += gvData.GetRowCellDisplayText(i, col).Trim();
                                    else
                                        texto += (chkCaracter.Checked ? txtSimbolo.Text : "\t") + gvData.GetRowCellDisplayText(i, col).Trim();

                                    progressBarCont.PerformStep();
                                    progressBarCont.Update();
                                }
                                sw.WriteLine(texto);
                                progressBarCont.PerformStep();
                                progressBarCont.Update();
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void chkCaracter_CheckedChanged(object sender, EventArgs e)
        {
            txtSimbolo.Enabled = chkCaracter.Checked;
        }

        
    }
}
