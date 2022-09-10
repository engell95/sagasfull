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
    public partial class DialogShowComprobanteMultiES : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.PlantillaCarga PCAnterior;
        public int EstacionID, SubEstacionID;
        public decimal _TipoCambio;
        public DateTime Fecha;
        public bool EsCliente;
        private bool _Anticipo = false;
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

        public DialogShowComprobanteMultiES()
        {
            InitializeComponent();            
        }

        public DialogShowComprobanteMultiES(Entidad.PlantillaCarga PC, bool EsAnticipo, DateTime fecha, int vEstacionID, int vSubEstacionID, decimal tipoCambio, bool vEsCliente)
        {
            InitializeComponent();
            _Anticipo = EsAnticipo;
            EsCliente = vEsCliente;
            Fecha = fecha;
            EstacionID = vEstacionID;
            SubEstacionID = vSubEstacionID;
            _TipoCambio = tipoCambio;
            PCAnterior = PC;
            this.progressBarCont.Visible = true;
            progressBarCont.EditValue = 0;

           

        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            
                
        }

        private void SetComprobantes()
        {
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    if (EsCliente)
                    {
                        #region <<< CLIENTE >>>

                        List<Int32> IDCuenta = new List<Int32>();
                        int i = 1;

                        progressBarCont.Properties.Maximum = PCAnterior.PlantillaCargaCliente.Where(o => !o.ClienteID.Equals(0)).Count() + 1;

                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.CommandTimeout = 300;
                        var Cuentas = db.Clientes.Where(c => (PCAnterior.PlantillaCargaCliente.Where(p => !p.ClienteID.Equals(0)).Select(s => s.ClienteID).Contains(c.ID))).Select(s => new { s.ID, s.CuentaContableID });

                        if (!_Anticipo)
                        {
                            PCAnterior.PlantillaCargaCliente.Where(o => !o.ClienteID.Equals(0)).ToList().ForEach(det =>
                            {
                                progressBarCont.PerformStep();
                                progressBarCont.Update();
                                if (!IDCuenta.Contains(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID))
                                {
                                    DetalleCD.Add(new Entidad.ComprobanteContable
                                                {
                                                    CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID,
                                                    Monto = Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero),
                                                    TipoCambio = _TipoCambio,
                                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                                    Fecha = Fecha,
                                                    Descripcion = "Carga Inicial del saldo Clientes",
                                                    Linea = i,
                                                    EstacionServicioID = EstacionID,
                                                    SubEstacionID = SubEstacionID
                                                });
                                    IDCuenta.Add(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = DetalleCD.Where(c => c.CuentaContableID.Equals(Cuentas.First(ct => ct.ID.Equals(det.ClienteID)).CuentaContableID)).First();
                                    comprobante.Monto += Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }
                            });

                            progressBarCont.PerformStep();
                            progressBarCont.Update();
                            DetalleCD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 526,
                                Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaCliente.Where(p => !p.ClienteID.Equals(0)).Sum(s => s.Saldo)), 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaCliente.Where(p => !p.ClienteID.Equals(0)).Sum(s => s.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = "Carga Inicial del saldo Clientes",
                                Linea = i,
                                EstacionServicioID = EstacionID,
                                SubEstacionID = SubEstacionID
                            });
                        }
                        else if (_Anticipo)
                        {
                            progressBarCont.PerformStep();
                            progressBarCont.Update();
                            DetalleCD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 526,
                                Monto = Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaCliente.Where(p => !p.ClienteID.Equals(0)).Sum(s => s.Saldo)), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaCliente.Where(p => !p.ClienteID.Equals(0)).Sum(s => s.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = "Carga Inicial del saldo Clientes",
                                Linea = i,
                                EstacionServicioID = EstacionID,
                                SubEstacionID = SubEstacionID
                            });
                            i++;

                            PCAnterior.PlantillaCargaCliente.Where(o => !o.ClienteID.Equals(0)).ToList().ForEach(det =>
                            {
                                progressBarCont.PerformStep();
                                progressBarCont.Update();
                                if (!IDCuenta.Contains(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID))
                                {
                                    DetalleCD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Carga Inicial del saldo Clientes",
                                        Linea = i,
                                        EstacionServicioID = EstacionID,
                                        SubEstacionID = SubEstacionID
                                    });
                                    IDCuenta.Add(Cuentas.First(c => c.ID.Equals(det.ClienteID)).CuentaContableID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = DetalleCD.Where(c => c.CuentaContableID.Equals(Cuentas.First(ct => ct.ID.Equals(det.ClienteID)).CuentaContableID)).First();
                                    comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }

                            });

                        }

                        this.progressBarCont.Visible = false;
                        FillControl();
                        #endregion
                    }
                    else
                    {
                        #region <<< PROVEEDOR >>>

                        List<Int32> IDCuenta = new List<Int32>();
                        int i = 1;

                        progressBarCont.Properties.Maximum = PCAnterior.PlantillaCargaProveedor.Where(o => !o.ProveedorID.Equals(0)).Count() + 1;

                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.CommandTimeout = 300;
                        var Cuentas = db.Proveedors.Where(c => (PCAnterior.PlantillaCargaProveedor.Where(p => !p.ProveedorID.Equals(0)).Select(s => s.ProveedorID).Contains(c.ID))).Select(s => new { s.ID, s.CuentaContableID }).ToList();

                        if (!_Anticipo)
                        {
                            progressBarCont.PerformStep();
                            progressBarCont.Update();
                            DetalleCD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 526,
                                Monto = Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaProveedor.Where(p => !p.ProveedorID.Equals(0)).Sum(s => s.Saldo)), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(PCAnterior.PlantillaCargaProveedor.Where(p => !p.ProveedorID.Equals(0)).Sum(s => s.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = "Carga Inicial del saldo Proveedores",
                                Linea = i,
                                EstacionServicioID = EstacionID,
                                SubEstacionID = SubEstacionID
                            });
                            i++;

                            PCAnterior.PlantillaCargaProveedor.Where(o => !o.ProveedorID.Equals(0)).ToList().ForEach(det =>
                            {
                                progressBarCont.PerformStep();
                                progressBarCont.Update();
                                if (!IDCuenta.Contains(Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID))
                                {
                                    DetalleCD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID,
                                        Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Carga Inicial del saldo Proveedores",
                                        Linea = i,
                                        EstacionServicioID = EstacionID,
                                        SubEstacionID = SubEstacionID
                                    });
                                    IDCuenta.Add(Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = DetalleCD.Where(c => c.CuentaContableID.Equals(Cuentas.First(ct => ct.ID.Equals(det.ProveedorID)).CuentaContableID)).First();
                                    comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero));
                                    comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                }

                            });

                        }
                        else if (_Anticipo)
                        {                           
                            PCAnterior.PlantillaCargaProveedor.Where(o => !o.ProveedorID.Equals(0)).ToList().ForEach(det =>
                            {
                                progressBarCont.PerformStep();
                                progressBarCont.Update();
                                if (!IDCuenta.Contains(Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID))
                                {
                                    DetalleCD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID,
                                        Monto = Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                        Fecha = Fecha,
                                        Descripcion = "Carga Inicial del saldo Proveedores",
                                        Linea = i,
                                        EstacionServicioID = EstacionID,
                                        SubEstacionID = SubEstacionID
                                    });
                                    IDCuenta.Add(Cuentas.First(c => c.ID.Equals(det.ProveedorID)).CuentaContableID);
                                    i++;
                                }
                                else
                                {
                                    var comprobante = DetalleCD.Where(c => c.CuentaContableID.Equals(Cuentas.First(ct => ct.ID.Equals(det.ProveedorID)).CuentaContableID)).First();
                                    comprobante.Monto += Decimal.Round(Convert.ToDecimal(det.Saldo), 2, MidpointRounding.AwayFromZero);
                                    comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(det.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                }

                            });


                            progressBarCont.PerformStep();
                            progressBarCont.Update();
                            DetalleCD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 526,
                                Monto = Decimal.Round(-Math.Abs(Convert.ToDecimal(PCAnterior.PlantillaCargaProveedor.Where(p => !p.ProveedorID.Equals(0)).Sum(s => s.Saldo))), 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(-Math.Abs(Convert.ToDecimal(PCAnterior.PlantillaCargaProveedor.Where(p => !p.ProveedorID.Equals(0)).Sum(s => s.Saldo) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = "Carga Inicial del saldo Proveedores",
                                Linea = i,
                                EstacionServicioID = EstacionID,
                                SubEstacionID = SubEstacionID
                            });
                            i++;

                        }
                        this.progressBarCont.Visible = false;
                        FillControl();
                        #endregion
                    }

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    progressBarCont.EditValue = 0;
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
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
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 300;
                rplkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre });
                rpCeco.DataSource = db.CentroCostos.Select(s => new { s.ID, Display = s.Nombre });
                
                var obj = (from cds in this.CD
                           join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                           join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                           join cto in db.CentroCostos on cds.CentroCostoID equals cto.ID into ceco from Centros in ceco.DefaultIfEmpty()
                           select new
                                    {
                                        ID = cds.ID,
                                        CodigoCuenta = cc.Codigo,
                                        NombreCuenta = cc.Nombre,
                                        Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                        Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                        cds.Descripcion,
                                        cds.EstacionServicioID,
                                        cds.CentroCostoID,
                                        cds.Linea
                                    }).OrderBy(o => o.Linea);

                this.bdsDetalle.DataSource = obj.OrderBy(o => o.Linea).ToList();


                if (obj.Count() > 0)
                {
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

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
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
                  
        private void bgvComprobante_GroupRowCollapsing(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            e.Allow = false;
        }

        #endregion

        private void DialogShowComprobanteMultiES_Shown(object sender, EventArgs e)
        {
            FillControl();

            if (PCAnterior != null)
                SetComprobantes();
        }
    }
}