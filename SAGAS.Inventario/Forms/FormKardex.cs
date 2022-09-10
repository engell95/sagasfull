using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid.Views;

namespace SAGAS.Inventario.Forms
{                                
    public partial class FormKardex : Form
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;


        #endregion

        #region <<< INICIO >>>

        public FormKardex()
        {
            InitializeComponent();
            //this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaKardex);
            //this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
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
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());








                //linqInstantFeedbackSource1.GetQueryable += linqInstantFeedbackSource1_GetQueryable;

                //this.grid.DataSource = linqInstantFeedbackSource1;

                //DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                //this.bgvData.ActiveFilterString = (new OperandProperty("Fecha") > fecha.AddDays(-1)).ToString();
           

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                //List<int> lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));

                //var query = from k in dv.VistaKardexes
                //            join m in dv.VistaMovimientos on k.VistaMovimiento equals m
                //            where lista.Contains(k.EstacionServicioID) && m.AplicaKardex && !k.EsManejo
                //            select new
                //            {
                //                k.ID,
                //                k.EsProducto,
                //                m.Abreviatura,
                //                m.Referencia,
                //                Descripcion = (m.MovimientoTipoID.Equals(3) ? m.ProveedorNombre : " "),
                //                Producto = k.ProductoCodigo + " | " + k.ProductoNombre,
                //                k.UnidadMedidaNombre,
                //                k.Fecha,
                //                k.EstacionNombre,
                //                k.SubEstacionNombre,
                //                Almacen = (m.Entrada ? k.AlmacenEntradaNombre : k.AlmacenSalidaNombre),
                //                k.CantidadInicial,
                //                k.CantidadEntrada,
                //                k.CantidadSalida,
                //                k.CantidadFinal,
                //                Costo = (m.Entrada ? k.CostoEntrada : k.CostoFinal),
                //                CostoEntrada = (m.Entrada ? k.CostoTotal : 0m),
                //                CostoSalida = (!m.Entrada ? k.CostoTotal : 0m),
                //                Saldo = Decimal.Round(Convert.ToDecimal(k.CantidadFinal * k.CostoFinal), 2, MidpointRounding.AwayFromZero)
                //            };

                //e.QueryableSource = query;
                //e.Tag = dv;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            //((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }


              
        #endregion   
        
    }
}
