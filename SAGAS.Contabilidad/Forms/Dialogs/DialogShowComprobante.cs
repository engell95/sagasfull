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
using DevExpress.XtraBars;
using System.Text.RegularExpressions;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogShowComprobante : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
        public List<Entidad.ComprobanteContable> DetalleCD
        {
            get { return CD; }
            set
            {
                CD = value;
                this.bdsDetalle.DataSource = this.CD;
            }
        }
        #endregion

        #region *** INICIO ***

        public DialogShowComprobante()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private System.Data.DataTable ToDataTable(object query)
        {
            if (query == null)
                throw new ArgumentNullException("Consulta no especificada!");

            System.Data.IDbCommand cmd = db.GetCommand(query as System.Linq.IQueryable);
            System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
            adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
            System.Data.DataTable dt = new System.Data.DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, System.Data.SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                
                var obj = (from cds in this.CD
                           join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                           join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                           select new
                                    {
                                        ID = cds.ID,
                                        CodigoCuenta = cc.Codigo,
                                        NombreCuenta = cc.Nombre,
                                        Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                        Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                        cds.Descripcion,
                                        cds.Linea
                                    }).OrderBy(o => o.Linea);

                this.bdsDetalle.DataSource = obj;

                if ((obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))).Equals(0))
                {
                    this.lblDiferencia.Text = "SIN DIFERENCIAS";
                }
                else
                {
                    lblDiferencia.ForeColor = Color.Red;
                    this.lblDiferencia.Text = "DIFERENCIAS  " + (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))).ToString("#,0.00");
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        #endregion

        #region *** EVENTOS ***
     
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                btnCancel_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
               
        #endregion
    }
}