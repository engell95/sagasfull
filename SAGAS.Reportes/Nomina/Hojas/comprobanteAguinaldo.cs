using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace AdmonP.Reportes.Aguinaldo
{
    public partial class comprobanteAguinaldo : DevExpress.XtraReports.UI.XtraReport
    {
        public Image Logo
        {
            set { pbLogo.Image = value; }
        }

        public DataTable obj;
        private int ID;

        public comprobanteAguinaldo(DataTable dt, int id)
        {
            InitializeComponent();
            obj = dt;
            ID = id;
        }

        private void RangoFechasYMD(DateTime endDate, DateTime beginDate, out int years, out int months, out int days)
        {
            endDate = endDate.Date.AddDays(1);
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
                days = endDate.Day - beginDate.Day;
            }
            years = months / 12;
            months -= years * 12;
        }

        private void xrLabelPeriodo1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
                        
            string empl = "";
        
            int days = 0, month = 0, year = 0;
            string dias = " ", mes = " ", ano = " ";

            foreach (DataRow row in obj.Select("IdEmpleado = " + xrLabelPeriodo1.Text + " and ID = " + ID))
            {

                RangoFechasYMD(Convert.ToDateTime(row["FechaFin"]).Date, Convert.ToDateTime(row["FechaInicio"]).Date, out year, out month, out days);

                if (year > 0) ano += year + " Año ";
                if (month > 0) mes += month + " Mes ";
                if (days > 0) dias += days + " Días ";


                empl = ano + mes + dias;
                break;
            }

            xrLabelPeriodo1.Text = empl;
        }

        private void xrLabelPeriodo2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string empl = "";

            int days = 0, month = 0, year = 0;
            string dias = " ", mes = " ", ano = " ";

            foreach (DataRow row in obj.Select("IdEmpleado = " + xrLabelPeriodo2.Text + " and ID = " + ID))
            {

                RangoFechasYMD(Convert.ToDateTime(row["FechaFin"]).Date, Convert.ToDateTime(row["FechaInicio"]).Date, out year, out month, out days);

                if (year > 0) ano += year + " Año ";
                if (month > 0) mes += month + " Mes ";
                if (days > 0) dias += days + " Días ";


                empl = ano + mes + dias;
                break;
            }

            xrLabelPeriodo2.Text = empl;
        }

    }
}
