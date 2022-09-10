using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Nomina.Hojas
{
    public partial class Planillaz : DevExpress.XtraReports.UI.XtraReport
    {
        private decimal _SalarioPlanilla = 0;
        private decimal _TotalSalarioPlanilla = 0;
        private decimal _DiaLaborado = 0;
        private decimal _TotalDiaLaborado = 0;
        private decimal _TotalIngreso = 0;
        private decimal _TotalTotalIngreso = 0;
        private decimal _TotalEgreso = 0;
        private decimal _TotalTotalEgreso = 0;
        private decimal _TotalNeto = 0;
        private decimal _TotalTotalNeto = 0;
        private decimal _Entrada = 0;
        private decimal _Salida = 0;
        private float _Location = 0;

        public Planillaz()
        {
            InitializeComponent();
            FillControl();
        }

        public Planillaz(object Procedure)
        {
            try
            { 
            InitializeComponent();
            this.DataSource = Procedure;
           calculatedField1.DataSource = cfTotalOtrasDeducciones.DataSource = cfMonto3.DataSource = cfMonto26.DataSource = Procedure;
            //var suma = ((System.Data.DataTable)Procedure);
            //    suma.AsEnumerable().Where
            //    //dtEstacionesServicios.AsEnumerable().Count(c => ((bool) c["SelectedES"]).Equals(true)) <= 0)
                 
            //table.AsEnumerable().Sum(x => x.Field<int>(3));

                System.Collections.Generic.IEnumerable<XRTableCell> celdas = this.AllControls<XRTableCell>();

                foreach (XRTableCell celda in celdas)
                {
                    if (!String.IsNullOrEmpty(celda.Tag.ToString()))
                    {
                        celda.DataBindings.Add("Text", Procedure, celda.Tag.ToString(), "{0:N2}");

                        //celda.Summary = new DevExpress.XtraReports.UI.XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:N2}");
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void FillControl()
        {
            try
            {
                
                //this.xrCellList.DataBindings.Add("Text", null, "Nombre");
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


        #region <<< VALORES MOVIMIENTOS POR DEFECTOS >>>
        private void xrTableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell celda = (XRTableCell)sender;

            if (celda.Name.Contains("Salario"))
            {
                #region <<< SALARIO PLANILLA >>>
                _SalarioPlanilla += (String.IsNullOrEmpty(celda.Text) ? 0 : Convert.ToDecimal(celda.Text));
                #endregion
            }
            else if (celda.Name.Contains("DiaLaborado"))
            {
                #region <<< DIA LABORADO >>>
                _DiaLaborado += (String.IsNullOrEmpty(celda.Text) ? 0 : Convert.ToDecimal(celda.Text));
                #endregion
            }
            else if (celda.Name.Contains("TotalIngreso"))
            {
                #region <<< TOTAL INGRESO  >>>
                _TotalIngreso += (String.IsNullOrEmpty(celda.Text) ? 0 : Convert.ToDecimal(celda.Text));
                #endregion
            }
            else if (celda.Name.Contains("TotalEgreso"))
            {
                #region <<< TOTAL EGRESO  >>>
                _TotalEgreso += (String.IsNullOrEmpty(celda.Text) ? 0 : Convert.ToDecimal(celda.Text));
                #endregion
            }
            else if (celda.Name.Contains("TotalNeto"))
            {
                #region <<< TOTAL NETO  >>>
                _TotalNeto += (String.IsNullOrEmpty(celda.Text) ? 0 : Convert.ToDecimal(celda.Text));
                #endregion
            }

        }

        private void xrTableCell43_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell celda = (XRTableCell)sender;

            if (celda.Name.Contains("Salario"))
            {
                #region <<< SALARIO PLANILLA >>>
                _TotalSalarioPlanilla += _SalarioPlanilla;
                celda.Text = Convert.ToDecimal(_SalarioPlanilla).ToString("#,0.00");
                _SalarioPlanilla = 0;
                #endregion
            }
            else if (celda.Name.Contains("DiaLaborado"))
            {
                #region <<< DIA LABORADO >>>
                _TotalDiaLaborado += _DiaLaborado;
                celda.Text = Convert.ToDecimal(_DiaLaborado).ToString("#,0.00");
                _DiaLaborado = 0;
                #endregion
            }
            else if (celda.Name.Contains("TotalIngreso"))
            {
                #region <<< TOTAL INGRESO >>>
                _TotalTotalIngreso += _TotalIngreso;
                celda.Text = Convert.ToDecimal(_TotalIngreso).ToString("#,0.00");
                _TotalIngreso = 0;
                #endregion
            }
            else if (celda.Name.Contains("TotalEgreso"))
            {
                #region <<< TOTAL EGRESO >>>
                _TotalTotalEgreso += _TotalEgreso;
                celda.Text = Convert.ToDecimal(_TotalEgreso).ToString("#,0.00");
                _TotalEgreso = 0;
                #endregion
            }
            else if (celda.Name.Contains("TotalNeto"))
            {
                #region <<< TOTAL NETO >>>
                _TotalTotalNeto += _TotalNeto;
                celda.Text = Convert.ToDecimal(_TotalNeto).ToString("#,0.00");
                _TotalNeto = 0;
                #endregion
            }
        }

        private void xrTableCell61_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell celda = (XRTableCell)sender;

            if (celda.Name.Contains("Salario"))
            {
                #region <<< SALARIO PLANILLA >>>
                celda.Text = Convert.ToDecimal(_TotalSalarioPlanilla).ToString("#,0.00");
                #endregion
            }
            else if (celda.Name.Contains("DiaLaborado"))
            {
                #region <<< DIA LABORADO >>>
                celda.Text = Convert.ToDecimal(_TotalDiaLaborado).ToString("#,0.00");
                #endregion
            }
            else if (celda.Name.Contains("TotalIngreso"))
            {
                #region <<< TOTAL INGRESO >>>
                celda.Text = Convert.ToDecimal(_TotalTotalIngreso).ToString("#,0.00");
                #endregion
            }
            else if (celda.Name.Contains("TotalEgreso"))
            {
                #region <<< TOTAL EGRESO >>>
                celda.Text = Convert.ToDecimal(_TotalTotalEgreso).ToString("#,0.00");
                #endregion
            }
            else if (celda.Name.Contains("TotalNeto"))
            {
                #region <<< TOTAL NETO >>>
                celda.Text = Convert.ToDecimal(_TotalTotalNeto).ToString("#,0.00");
                #endregion
            }
        }
        #endregion

        private void xrTableCell25_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            XRTableCell celda = (XRTableCell)sender;
                       
            if (!String.IsNullOrEmpty(celda.Text))
            {
                if (Convert.ToDecimal(celda.Text).Equals(0m))
                    celda.Text = "";
                
            }
            else
                celda.Text = "";
        }


        
        
    }
}
