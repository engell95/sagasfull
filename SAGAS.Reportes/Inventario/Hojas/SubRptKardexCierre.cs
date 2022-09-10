using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Linq;

namespace SAGAS.Reportes.Inventario.Hojas
{
    public partial class SubRptKardexCierre : DevExpress.XtraReports.UI.XtraReport
    {
        public SubRptKardexCierre(List<Entidad.GetInventarioCorteResult> datos, string Estacion)
       {
            try
            {
            InitializeComponent();
            this.xrEstacion.Text = Estacion;
            var obj = datos.GroupBy(g => new { g.AreaID, g.AreaNombre }).OrderBy(b => b.Key.AreaID);
            foreach (var item in obj)
            {
                DevExpress.XtraReports.UI.GroupHeaderBand GH = new DevExpress.XtraReports.UI.GroupHeaderBand();
                DevExpress.XtraReports.UI.XRSubreport xrSubCierre = new DevExpress.XtraReports.UI.XRSubreport();

                xrSubCierre.ReportSource = new Reportes.Inventario.Hojas.SubRptCierreKardex(datos.Where(o => o.AreaID.Equals(item.Key.AreaID)).ToList(), item.Key.AreaNombre);
                GH.Controls.Add(xrSubCierre);
                this.Bands.Add(GH);
            }
            //this.xrPivot.DataSource = datos;
            
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }
     
    }
}
