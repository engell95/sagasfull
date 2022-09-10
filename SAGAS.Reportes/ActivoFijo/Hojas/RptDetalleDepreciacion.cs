using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;

namespace SAGAS.Reportes.ActivoFijo.Hojas
{
    public partial class RptDetalleDepreciacion : DevExpress.XtraReports.UI.XtraReport
    {
        Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
        public RptDetalleDepreciacion()
        {
            InitializeComponent();
        }

        private void RptKardex_DataSourceDemanded(object sender, EventArgs e)
        {
            
        }

        private void RptKardex_ParametersRequestSubmit(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {

        }

        private void cfValorDepreciable_GetValue(object sender, GetValueEventArgs e)
        {
            try
            {
                Entidad.GetListaActivosDepreciadosResult obj = e.Row as Entidad.GetListaActivosDepreciadosResult;
                //e.Row[""]
                //if (e.Value != null)
                //System.Windows.Forms.MessageBox.Show(e.Value.ToString());

                if (obj.MesesDepreciados > 0)
                {

                    if (obj.VidaUtilMeses - obj.MaxMesDepreciado < 0)
                        e.Value = 0;
                    else
                    {
                        if (obj.FechaAdquisicion.Date.Month >= Convert.ToDateTime(Parameters["Fecha"].Value).Date.Month && obj.FechaAdquisicion.Date.Year >= Convert.ToDateTime(Parameters["Fecha"].Value).Date.Year)
                            e.Value = 0;
                        else
                        {

                            var dep = (from d in db.Depreciacions
                                       join m in db.Movimientos on d.MovimientoID equals m.ID
                                       where d.BienID.Equals(obj.ID) && m.FechaRegistro.Date.Equals(Convert.ToDateTime(Parameters["Fecha"].Value).Date)
                                       select new { d.ValorDepreciacion }).FirstOrDefault();

                            if (dep != null)
                                e.Value = Math.Round(dep.ValorDepreciacion, 2, MidpointRounding.AwayFromZero);
                            else
                                e.Value = Math.Round(obj.ValorAdquisicion / obj.VidaUtilMeses, 2, MidpointRounding.AwayFromZero);
                        }
                    }
                }
                else
                    e.Value = 0;

                //e.Value = obj.ID;
            }
            catch { e.Value = 0; }
        }

        private void cfEsMaxMeses_GetValue(object sender, GetValueEventArgs e)
        {
            try
            {
                Entidad.GetListaActivosDepreciadosResult obj = e.Row as Entidad.GetListaActivosDepreciadosResult;
                //e.Row[""]
                //if (e.Value != null)
                //System.Windows.Forms.MessageBox.Show(e.Value.ToString());

                if (obj.MesesDepreciados > 0)
                {

                    if (obj.VidaUtilMeses < obj.MaxMesDepreciado)
                        e.Value = obj.VidaUtilMeses;
                    else
                    {
                        //if (!obj.EsDepreciado)
                        //    e.Value = 0;
                        //else
                        //{

                        var dep = (from d in db.Depreciacions
                                   join m in db.Movimientos on d.MovimientoID equals m.ID
                                   where d.BienID.Equals(obj.ID) && m.FechaRegistro.Date.Equals(Convert.ToDateTime(Parameters["Fecha"].Value).Date)
                                   select new { d.NumeroDepreciacion }).FirstOrDefault();

                        if (dep != null)
                            e.Value = dep.NumeroDepreciacion;
                        else
                            e.Value = obj.MaxMesDepreciado;
                        //}
                    }
                }
                else
                {
                    //if (!obj.EsDepreciado)
                    //    e.Value = 0;
                    //else
                    //{
                        e.Value = obj.MaxMesDepreciado;
                    //}
                }
            }
            catch { e.Value = 0; }
            /*
            Iif([MaxMesDepreciado] > [VidaUtilMeses],
             *       [VidaUtilMeses], 
                     else Iif([EsDepreciado] == True,
             *                 Iif([MaxMesDepreciado] ==  [VidaUtilMeses],
             *                      [MesesDepreciados],
             *                      [MaxMesDepreciado])
                    ,[VidaUtilMeses])
                )
             */
        }
              
    }
}
