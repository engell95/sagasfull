using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics; 
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using DevExpress.XtraCharts;

namespace SAGAS.ActivoFijo.Forms
{
    public partial class FormDetalleDepreciacion : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dbView;
        internal int UsuarioID = Parametros.General.UserID;
        private List<int> lista;

        #endregion

        #region <<< INICIO >>>

        public FormDetalleDepreciacion()
        {
            InitializeComponent();
        }

        private void FormDetalleDepreciacion_Shown(object sender, EventArgs e)
        {
            Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

            lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == UsuarioID).Select(s => s.EstacionServicioID));

            lkEstacion.Properties.DataSource = from es in db.EstacionServicios
                                               where lista.Contains(es.ID)
                                               select new { es.ID, es.Nombre };
            lkEstacion.EditValue = Parametros.General.EstacionServicioID;

            dateFecha.DateTime = DateTime.Now.Date;
            Parametros.General.splashScreenManagerMain.CloseWaitForm();
        }

        #endregion

        #region <<< PIVOT >>>
        
        async Task ListaTotales(DateTime Fecha)
        {
            try
            {                
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                await Task.Delay(10);
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                dbView.CommandTimeout = 900;
                Reportes.ActivoFijo.Hojas.RptDetalleDepreciacion rep = new Reportes.ActivoFijo.Hojas.RptDetalleDepreciacion();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);

                
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = lkEstacion.Text;
                rep.xrCeFecha.Text = Fecha.ToShortDateString();
                var V = dbView.GetListaActivosDepreciados(Fecha).Where(o => o.EstacionID.Equals(Convert.ToInt32(lkEstacion.EditValue)));
                rep.Fecha.Value = Fecha.Date;
                rep.DataSource = V.ToList();
               var ES = db.EstacionServicios.SingleOrDefault(es => es.ID.Equals(Convert.ToInt32(lkEstacion.EditValue)));
                
                if (ES != null)
                {
                    var Admin = db.Empleados.Select(s => new { s.ID, Display = s.Nombres + " " + s.Apellidos}).SingleOrDefault(o => o.ID.Equals(ES.AdministradorID));
                    rep.xrCellRevisado.Text = (Admin != null ? Admin.Display : "");

                    var Cont = db.Empleados.Select(s => new { s.ID, Display = s.Nombres + " " + s.Apellidos }).SingleOrDefault(o => o.ID.Equals(ES.ResponsableContableID));                    
                    rep.xrCellContador.Text = (Cont != null ? Cont.Display : "");
                }

                this.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rep.RequestParameters = false;
                rep.CreateDocument();
                Parametros.General.splashScreenManagerMain.CloseWaitForm();

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

       
        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (lkEstacion.EditValue != null && dateFecha.EditValue != null)
                {
                    var Dep = from d in db.Depreciacions
                              join b in db.Bien on d.BienID equals b.ID
                              join m in db.Movimientos on d.MovimientoID equals m.ID
                              where b.EstacionID.Equals(Convert.ToInt32(lkEstacion.EditValue)) && (m.FechaRegistro.Month.Equals(Convert.ToDateTime(dateFecha.EditValue).Month) && m.FechaRegistro.Year.Equals(Convert.ToDateTime(dateFecha.EditValue).Year))
                              select d;

                    if (Dep.Count() > 0)
                        ListaTotales(Convert.ToDateTime(dateFecha.EditValue)).ConfigureAwait(false);
                    else
                        Parametros.General.DialogMsg("En el modulo no hay datos regisrtrado de depreciacion para el mes seleccionado." + Environment.NewLine + dateFecha.Text, Parametros.MsgType.warning);
                }
                else
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS, Parametros.MsgType.warning);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime vFecha = new DateTime(dateFecha.DateTime.Year, dateFecha.DateTime.Month, 1).AddMonths(1).AddDays(-1);

                if (!dateFecha.DateTime.Date.Equals(vFecha.Date))
                    dateFecha.DateTime = vFecha;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        
 
    }
}
