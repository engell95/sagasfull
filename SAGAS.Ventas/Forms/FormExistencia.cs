using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Ventas.Forms
{
    public partial class FormExistencia : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormExistencia()
        {
            InitializeComponent();
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnAgregar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var lista = from ap in db.AlmacenProductos
                                               join a in db.Almacens on ap.AlmacenID equals a.ID
                                               join p in db.Productos on ap.ProductoID equals p.ID
                                               join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                                               join ar in db.Areas on c.AreaID equals ar.ID
                                               join u in db.UnidadMedidas on p.UnidadMedidaID equals u.ID
                            where (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == a.EstacionServicioID))
                                               select new
                                               {
                                                  ProductoID = ap.ProductoID,
                                                  a.EstacionServicioID,
                                                  a.SubEstacionID,
                                                  Almacen = a.Nombre,
                                                  Area = ar.Nombre,
                                                  Clase = c.Nombre,
                                                  Producto = p.Codigo + " | " + p.Nombre,
                                                  UnidadNombre = u.Nombre,
                                                  Cantidad = ap.Cantidad
                                               };

                bdsManejadorDatos.DataSource = Parametros.General.LINQToDataTable(lista);

                this.grid.DataSource = bdsManejadorDatos;

                //**Estaciones de Servicio
                rpEstacionServicio.DataSource = from es in db.EstacionServicios
                                                where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Usuario && ges.EstacionServicioID == es.ID))
                                                select new { es.ID, es.Nombre };

                rpEstacionServicio.DisplayMember = "Nombre";
                rpEstacionServicio.ValueMember = "ID";

                if (Parametros.General.ListSES.Count <= 0)
                    this.colSUS.Visible = false;

                //**SubEstacionServicio
                lkSES.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).Select(s => new { s.ID, s.Nombre });
                lkSES.DisplayMember = "Nombre";
                lkSES.ValueMember = "ID";

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG)
        {

            FillControl();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        #endregion
    }
}
