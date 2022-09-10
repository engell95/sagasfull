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
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogInventarioResumen : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal bool Editable = false;
        private bool ShowMsg = false;
        internal int _ID;
        internal Entidad.ResumenDia Resumen;

        private List<Entidad.InventarioResumenDia> Inv = new List<Entidad.InventarioResumenDia>();
        public List<Entidad.InventarioResumenDia> DetalleInv
        {
            get { return Inv; }
            set
            {
                Inv = value;
                this.bdsLista.DataSource = this.Inv;
            }
        }
        //= new List<Entidad.Movimiento>();

        #endregion

        #region *** INICIO ***

        public DialogInventarioResumen(int ID)
        {
            InitializeComponent();
            _ID = ID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                Resumen = db.ResumenDias.Single(s => s.ID.Equals(_ID));
                lkTanque.DataSource = db.Tanques.Where(t => t.EstacionServicioID.Equals(Resumen.EstacionServicioID) && t.SubEstacionID.Equals(Resumen.SubEstacionID) && t.Activo).Select(s => new { s.ID, s.Nombre });
                lkProducto.DataSource = db.Productos.Where(p => p.ProductoClaseID.Equals(Parametros.Config.ProductoClaseCombustible())).Select(s => new { s.ID, s.Nombre });

                if (db.InventarioResumenDia.Count(i => i.ResumenDiaID.Equals(_ID)) > 0)
                {
                    DetalleInv.AddRange(db.InventarioResumenDia.Where(i => i.ResumenDiaID.Equals(_ID)).ToList());
                }
                else
                {
                    var Item = (from p in db.Productos
                                join t in db.Tanques on p.ID equals t.ProductoID
                                where t.EstacionServicioID.Equals(Resumen.EstacionServicioID) && t.SubEstacionID.Equals(Resumen.SubEstacionID) && t.Activo
                                select new
                                {
                                    TID = t.ID,
                                    IDP = p.ID
                                }).ToList();

                    Item.ForEach(obj => 
                        {
                            DetalleInv.Add(new Entidad.InventarioResumenDia { ResumenDiaID = 0, ProductoID = obj.IDP, TanqueID = obj.TID, Litros = 0m });
                        });
                }

                bdsLista.DataSource = DetalleInv;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());               
            }
        }

        #endregion

        #region *** METODOS ***

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (DetalleInv.All(d => d.Litros > 0))
            {
                Entidad.ResumenDia RD = db.ResumenDias.Single(s => s.ID.Equals(_ID));

                RD.InventarioResumenDia.Clear();
                RD.InventarioResumenDia.AddRange(DetalleInv);
                db.SubmitChanges();

                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();
            }
            else
            {
                Parametros.General.DialogMsg("Debes de ingresar las mediciones de los litros al sistema" + Environment.NewLine, Parametros.MsgType.warning);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion        


    }


}