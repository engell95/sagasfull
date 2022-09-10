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
using SAGAS.Ventas.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Ventas.Forms.Dialogs
{
    public partial class DialogCompensacionAnticipos : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormLiquidaciones MDI;
        //internal Entidad.OrdenCompra EntidadAnterior;
        private Parametros.ListTipoDepositos listadoDeposito = new Parametros.ListTipoDepositos();
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        internal int _ClienteCordoba;
        internal int _ClienteDolar;
        internal int _CuentaPorCobrarEmpleado;
        private Parametros.ListMeses listadoMes = new Parametros.ListMeses();
        
        private int IDDeudor
        {
            get { return Convert.ToInt32(glkDeudor.EditValue); }
            set { glkDeudor.EditValue = value; }
        }
        
        private static Entidad.Cliente client;
        private static Entidad.Empleado empl;
        private static Entidad.TipoPago TipoPago;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private List<Entidad.MinutasDeposito> DetalleMinutas = new List<Entidad.MinutasDeposito>();

        private List<Entidad.Movimiento> M = new List<Entidad.Movimiento>();
        public List<Entidad.Movimiento> DetalleM
        {
            get { return M; }
            set
            {
                M = value;
                this.bdsDetalle.DataSource = this.M;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogCompensacionAnticipos(int UserID)
        {            
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            //rgCreditoContado.SelectedIndex = 0;
            //IDProveedor = 186;
            //Referencia = "123";
            //IDAlmacen = 1;
        }
        
        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                lkMesInicial.Properties.DataSource = listadoMes.GetListMeses();

                lkMesInicial.EditValue = DateTime.Now.Month;

                layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemDeudor.Text = "Cliente";
                glkDeudor.Properties.NullText = "<Seleccione el Cliente>";

                glkDeudor.EditValue = null;
                glkDeudor.Properties.DataSource = null;
                glkDeudor.Properties.DataSource = (from c in db.Clientes
                                                   where c.Activo && c.AplicaLiquidacion
                                                   group c by new { c.ID, c.Codigo, c.Nombre } into gr
                                                   select new
                                                   {
                                                       ID = gr.Key.ID,
                                                       Codigo = gr.Key.Codigo,
                                                       Nombre = gr.Key.Nombre,
                                                       Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                   }).ToList();
               
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }


        public bool ValidarCampos(bool detalle)
        {
            //if (txtNumero.Text == "" || lkMoneda.EditValue == null || dateFecha.EditValue == null || lkTipoMov.EditValue == null)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del Deposito.", Parametros.MsgType.warning);
            //    return false;
            //}
            
            //if (!Parametros.General.ValidatePeriodoContable(FechaDeposito, db, IDEstacionServicio))
            //{
            //    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
            //    return false;
            //}

            //if (detalle)
            //{                               

            //    if (DetalleM.Count <= 0)
            //    {
            //        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
            //        return false;
            //    }

            //    if (DetalleMinutas.Count <= 0)
            //    {
            //        if (Parametros.General.DialogMsg("No existe detalle de minutas, desea continuar." + Environment.NewLine, Parametros.MsgType.question) != DialogResult.OK)
            //            return false;
            //    }
            //    else
            //    {
            //        if (DetalleMinutas.Count(c => c.Monto.Equals(0)) > 0)
            //        {
            //            Parametros.General.DialogMsg("El monto de una minuta es igual a 0 (cero), debe ingresar un monto mayor." + Environment.NewLine, Parametros.MsgType.warning);
            //            return false;
            //        }
            //    }                

            //    if (DetalleM.Sum(s => s.Litros) <= 0)
            //    {
            //        Parametros.General.DialogMsg("El total del monto abonado es 0 (cero), debe abonar un monto mayor." + Environment.NewLine, Parametros.MsgType.warning);
            //        return false;
            //    }

            //    if (String.IsNullOrEmpty(Comentario))
            //    {
            //        Parametros.General.DialogMsg("Debe Ingresar un Comentario.", Parametros.MsgType.warning);
            //        return false;
            //    }

            //    if (Parametros.General.ListSES.Count > 0)
            //    {
            //        if (IDSubEstacion <= 0)
            //        {
            //            Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
            //            return false;
            //        }
            //    }
                                
            //}

            return true;
        }
        
        private bool Guardar()
        {
            if (!ValidarCampos(true))
                return false;

            if (1==1)
            {
                Parametros.General.DialogMsg("La diferencia debe ser igual a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                                        
                    db.SubmitChanges();
                    trans.Commit();

                    IDPrint = 0;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    Next = true;
                    return true;

                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() +
                          Environment.NewLine + ex2.Message);
                    }  
                    
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        private decimal GetMontoSecundario()
        {
            try
            {
                decimal monto = 0m;
                DetalleM.Where(m => m.Litros > 0).ToList().ForEach(det =>
                    {
                       monto += Decimal.Round(Decimal.Round(det.Litros, 2, MidpointRounding.AwayFromZero) / Decimal.Round(det.TipoCambio, 2, MidpointRounding.AwayFromZero), 2, MidpointRounding.AwayFromZero); 
                    });

                return monto;
            }
            catch { return 0m; }
        }

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

        #endregion
               
        #region *** EVENTOS ***

        //--GUARDAR
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, Next, RefreshMDI, IDPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (DetalleM.Count > 0)
                {
                    if (Parametros.General.DialogMsg("El Deposito actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        Next = false;
                        e.Cancel = true;
                    }
                }
            }

        }

        //--CANCELAR
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //--VALIDACION CAMPOS
        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Accesos Directos por Botones
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }


        //Selección del Deudor
        

        //Cambiar las opciones del ROC
        
        #region <<< GRID DOCUMENTOS >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

        }

        //Mensaje Validación del detalle
        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMonto)
                {
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                    {
                        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                        {
                            decimal vSaldo = 0, vAbono = 0;

                            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                            if (vAbono > vSaldo)
                            {
                                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                            }
                            else
                            {
                                if (vAbono.Equals(vSaldo))
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                else
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                            }
                        }
                        else
                        {
                            gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                        }
                    }
                    else
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0);
                    }

                    gvDetalle.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion
           
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(false))
                {
                    //this.bdsDetalle.DataSource = null;

                    //if (AreaVenta.Equals(3))
                    //{
                    //    DetalleM = (from m in db.Movimientos
                    //                join c in db.Clientes on m.ClienteID equals c.ID
                    //                where m.MovimientoTipoID.Equals(7) && m.AreaID.Equals(AreaVenta) && c.AplicaLiquidacion && !m.Credito
                    //                && m.ClienteID.Equals((IDMonedaPrincipal.Equals(MonedaPrimaria) ? _ClienteCordoba : _ClienteDolar)) && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                    //                select m).OrderBy(o => o.FechaRegistro).ToList();
                    //}
                    //else
                    //{
                    //    DetalleM = (from m in db.Movimientos
                    //                where m.MovimientoTipoID.Equals(7) && m.AreaID.Equals(AreaVenta) && !m.Credito
                    //                && !m.Anulado && !m.Pagado && m.EstacionServicioID.Equals(IDEstacionServicio)
                    //                select m).OrderBy(o => o.FechaRegistro).ToList();
                    //}

                    //this.bdsDetalle.DataSource = DetalleM;

                    //if (IDMonedaPrincipal.Equals(MonedaSecundaria))
                    //{
                    //    this.colTC.Visible = true;
                    //    this.colTC.VisibleIndex = 5;
                    //    this.colMontoUS.Visible = true;
                    //    this.colMontoUS.VisibleIndex = 6;
                    //    this.layoutControlItemDifCambiario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //    lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.MonedaID.Equals(2)).Select(s => new { s.ID, s.Nombre }).ToList();
                    //}
                    //else
                    //    lkCuenta.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();


                    //gridConcepto.Enabled = true;
                    //gvDetalle.RefreshData();
                    //lkTipoMov.Enabled = false;
                    //lkArea.Enabled = false;
                    //dateFecha.Enabled = false;
                    //lkMoneda.Enabled = false;
                    //btnLoad.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }


       #endregion








    }
}