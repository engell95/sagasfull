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
using SAGAS.Contabilidad.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Tesoreria.Forms.Dialogs
{
    public partial class DialogCajaChica : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCajaChica MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool NextProvision = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID; 
        private bool _ToPrint = false;
        private int IDPrint = 0;
        private int IDSubSerie = 0;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private int _CuentaIVACredito = Parametros.Config.IVAAcreditado();
        private decimal _TipoCambio;
        private int _CuentaCajaChica;
        private int _CajaChicaID;
        private bool EsEmpleado = false;
        private bool _AplicaAlcaldia = false;
        private int ProveedorNoRegistrado = Parametros.Config.ProveedorNoRegistradoID();
        private bool _Guardado = false;
        internal int _MonedaID;
        internal int _CuentaAlcaldiaPorPagarID;
        internal int _CuentaPorCobrarEmpleado;
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFechaCaja.EditValue); }
            set { dateFechaCaja.EditValue = value; }
        }

        private int IDProveedor
        {
            get { return Convert.ToInt32(glkProvee.EditValue); }
            set { glkProvee.EditValue = value; }
        }

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }        
        
        private decimal _Total
        {
            get { return Convert.ToDecimal(spGranTotal.Value); }
            set { spGranTotal.Value = value; }
        }

        private decimal _IVA
        {
            get { return Convert.ToDecimal(spIva.Value); }
            set { spIva.Value = value; }
        }

        private decimal _SubTotal
        {
            get { return Convert.ToDecimal(spSub.Value); }
            set { spSub.Value = value; }
        }

        public bool EsCargo
        {
            get { return Convert.ToBoolean(chkCargo.Checked); }
            set { chkCargo.Checked = value; }
        }    
        
        private static Entidad.Proveedor Provee;
        private static Entidad.Empleado emp;
        private IQueryable<Parametros.ListIdCuentaDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;

        private List<Entidad.Retencione> Imp = new List<Entidad.Retencione>();
        public List<Entidad.Retencione> DetalleImp
        {
            get { return Imp; }
            set
            {
                Imp = value;
                this.bdsImpuesto.DataSource = this.Imp;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogCajaChica(int UserID)
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
            ValidarerrRequiredField();
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
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

               _MonedaID = Parametros.Config.MonedaPrincipal();
               _CuentaAlcaldiaPorPagarID = Parametros.Config.CuentaAlcaldiaPorPagarID();
               _CuentaPorCobrarEmpleado = Parametros.Config.CuentaPorCobrarEmpleado();

                this.CentrosCostos = from cto in db.CentroCostos
                                      join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                      where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                      select new Parametros.ListIdDisplay {ID = cto.ID, Display = cto.Nombre};

                Cuentas = (from con in db.ConceptoContables
                           join cc in db.CuentaContables on con.CuentaContableID equals cc.ID
                           join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                           join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                           where con.EsCajaChica && cces.EstacionID.Equals(IDEstacionServicio)
                           select new Parametros.ListIdCuentaDisplayCodeBool
                           {
                               ID = con.ID,
                               Codigo = con.Nombre,
                               Cuenta = cc.ID,
                               Display = cc.Codigo + " | " + cc.Nombre,
                               valor = tc.UsaCentroCosto
                           }).OrderBy(o => o.Codigo);

                var listComprobante = (from cd in db.ComprobanteContables
                                       join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                       join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                       where cd.MovimientoID.Equals(0)
                                       select new { ConceptoID = cd.CuentaContableID, Nombre = cc.Codigo + "  " + cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                this.DetalleComprobante = ToDataTable(listComprobante);
                lkCuentaImpuesto.DataSource = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Nombre }).OrderBy(o => o.Display).ToList();
                rgOption.SelectedIndex = -1;
                this.bdsImpuesto.DataSource = this.DetalleImp;

                int Pchica = 0;

                if (IDSubEstacion > 0 && db.SubEstacions.Count(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0) > 0)
                {
                    Pchica = db.SubEstacions.First(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0).ProveedorCajaChicaID;
                    IDSubSerie = db.SubEstacions.First(s => s.ID.Equals(IDSubEstacion) && s.ProveedorCajaChicaID > 0).ID;
                }
                else
                    Pchica = db.EstacionServicios.First(s => s.ID.Equals(IDEstacionServicio) && s.ProveedorCajaChicaID > 0).ProveedorCajaChicaID;


                //Proveedor
                var P = from p in db.Proveedors
                        where p.ID.Equals(Pchica)
                         select new { p.ID, p.Codigo, p.Nombre, p.NombreComercial, p.CuentaContableID, p.LimiteCredito, Display = p.Codigo + " | " + p.Nombre};

                glkProvee.Properties.DataSource = P;
                glkProvee.Properties.DisplayMember = "Display";
                glkProvee.Properties.ValueMember = "ID";

                if (P.Count() > 0)
                {
                    IDProveedor = P.First().ID;
                    _CuentaCajaChica = P.First().CuentaContableID;
                    _CajaChicaID = P.First().ID;

                    var obj = (from d in db.Deudors
                               join m in db.Movimientos.Where(m => m.ProveedorID.Equals(_CajaChicaID) && (m.MovimientoTipoID.Equals(41) || (m.MovimientoTipoID.Equals(3) && !m.Credito)) && !m.Pagado) on d.MovimientoID equals m.ID
                               group d by new { d.ProveedorID } into gr
                               select new
                               {
                                   suma = gr.Sum(s => s.Valor)
                               }).ToList();

                    spLimite.Value = P.First().LimiteCredito;

                    decimal Saldo = (obj.Count > 0 ? obj.FirstOrDefault().suma : 0m); //(db.Deudors.Count(d => d.ProveedorID.Equals(_CajaChicaID)) > 0 ? db.Deudors.Where(d => d.ProveedorID.Equals(_CajaChicaID)).Sum(s => s.Valor) : 0m);
                    spSaldo.Value = Saldo;

                    decimal _Disponible = P.First().LimiteCredito - Saldo;
                    spDisponible.Value = _Disponible;
                }

                //--- Fill Combos Detalles --//
                gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                gridCuenta.DataSource = null;
                gridCuenta.DataSource = Cuentas;

                //Centro Costo GRID
                lkCentroCosto.DataSource = CentrosCostos;
                lkCentroCosto.DisplayMember = "Display";
                lkCentroCosto.ValueMember = "ID";

                glkBeneficiario.EditValue = null;

                dateFechaCaja.EditValue = Convert.ToDateTime(db.GetDateServer());

                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);

                
                    
                //gridCuenta.DisplayMember = "Codigo";
                //gridCuenta.ValueMember = "ID";

                this.bdsDetalle.DataSource = this.DetalleComprobante;

                txtNumero.Text = Parametros.General.GetNroSerie(IDEstacionServicio, IDSubSerie, 41, db).ToString("000000000");
                Parametros.General.splashScreenManagerMain.CloseWaitForm();                                
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(dateFechaCaja, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtEntregado, errRequiredField);
        }

        private bool ValidarReferencia(string code)
        {
            var result = (from i in db.Movimientos
                          where  i.Referencia.Equals(code) && i.ProveedorID.Equals(IDProveedor) && !i.Anulado
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool ValidarCampos(bool detalle)
        {
            if (dateFechaCaja.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || String.IsNullOrEmpty(txtEntregado.Text) || txtNumero.Text == "" || spGranTotal.Value.Equals(0))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del documento.", Parametros.MsgType.warning);
                return false;
            }

            if (rgOption.SelectedIndex.Equals(0))
            {
                if (String.IsNullOrEmpty(txtReferencia.Text))
                {
                    Parametros.General.DialogMsg("Debe digitar el número de factura.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (Convert.ToInt32(glkBeneficiario.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Beneficiario.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFechaCaja, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (Parametros.General.ListSES.Count > 0)
            {
                if (IDSubEstacion <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (!detalle && !EsCargo)
            {
                if (DetalleComprobante.Rows.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle al documento." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (DetalleImp.Count > 0)
                {
                    if (!DetalleImp.All(d => d.Numero > 0))
                    {
                        Parametros.General.DialogMsg("Debe de digitar el Número de Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }

                    if (!DetalleImp.All(d => !String.IsNullOrEmpty(d.Concepto)))
                    {
                        Parametros.General.DialogMsg("Debe de digitar el Concepto de la Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }

                }

                //if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                //{
                //    Parametros.General.DialogMsg("La referencia para este documento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                //    return false;
                //}

            }

            return true;
        }

        private bool ValidarNumeroCajaChica(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? (i.MovimientoTipoID.Equals(41) || i.MovimientoTipoID.Equals(3)) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubSerie) && i.ID != Convert.ToInt32(ID) :
                          (i.MovimientoTipoID.Equals(41) || i.MovimientoTipoID.Equals(3)) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio)) && i.SubEstacionID.Equals(IDSubSerie)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        private void ViewDiferencia()
        {
            decimal Dif = 0m;

            if (DetalleComprobante.Rows.Count > 0)
            Dif =  Convert.ToDecimal(DetalleComprobante.Compute("Sum(Monto)", ""));
                        
            if (!_SubTotal.Equals(Dif) && !EsCargo)
            {
                layoutControlDif.Visible = true;
                spDiferencia.Value = 0;

                    layoutControlDif.Visible = true;
                    spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Red;
                    layoutControlItemDif.Text = "Diferencia";
                    spDiferencia.Value = Math.Abs(_SubTotal - Dif);
            }
            else
            {
                layoutControlDif.Visible = false;
                spDiferencia.Value = 0;
            }

        }

        private bool RetieneAlcadia()
        {
            try
            {
                if (Provee != null)
                {
                    if (Provee.AplicaAlcaldia)
                    {
                        var ES = db.EstacionServicios.SingleOrDefault(es => es.ID.Equals(IDEstacionServicio));

                        if (ES != null)
                        {
                            if (ES.AplicaRetencionAlcaldia)
                            {
                                if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(Provee.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                    return true;
                                else
                                    return false;
                            }
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;

            }
            catch { return false; }
        }

        private void ListaImpuesto()
        {
            try
            {
                if (Provee != null)
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    
                    decimal vSub = 0, vMonto = 0, vResta = 0;

                    vSub = _SubTotal;

                    if (Provee.AplicaRetencion || _AplicaAlcaldia)
                    {
                        if (Provee.AplicaRetencion)
                        {
                            vMonto = 0;
                            if (vSub >= db.CuentaContables.Single(s => s.ID.Equals(Provee.ImpuestoRetencionID)).Limite)
                            {
                                decimal IR = (db.CuentaContables.Single(cc => cc.ID.Equals(Provee.ImpuestoRetencionID)).Porcentaje / 100m);
                                vMonto = Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);
                                vResta += Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);

                                if (DetalleImp.Count(d => !d.EsAlcaldia).Equals(0))
                                {
                                    int xr = 0;

                                    xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                    if (xr > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                    DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = xr, SubTotal = vSub, EsAlcaldia = false, CuentaContableID = Provee.ImpuestoRetencionID });
                                }
                                else
                                {
                                    DetalleImp.First(d => !d.EsAlcaldia).SubTotal = vSub;
                                    DetalleImp.First(d => !d.EsAlcaldia).Monto = vMonto;
                                }
                            }
                            else
                            {
                                if (DetalleImp.Count(d => !d.EsAlcaldia) > 0)
                                    DetalleImp.Remove(DetalleImp.First(d => !d.EsAlcaldia));
                            }
                        }

                        if (_AplicaAlcaldia)
                        {
                            vMonto = 0;
                            if (vSub >= db.CuentaContables.Single(s => s.ID.Equals(_CuentaAlcaldiaPorPagarID)).Limite || Provee.TipoProveedorID.Equals(ProveedorNoRegistrado))
                            {
                                decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(_CuentaAlcaldiaPorPagarID)).Porcentaje / 100m);
                                vMonto = Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);
                                vResta += Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);

                                if (DetalleImp.Count(d => d.EsAlcaldia).Equals(0))
                                {
                                    int xa = 0;

                                    xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                    if (xa > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                    DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = xa, SubTotal = vSub, EsAlcaldia = true, CuentaContableID = _CuentaAlcaldiaPorPagarID });
                                }
                                else
                                {
                                    DetalleImp.First(d => d.EsAlcaldia).SubTotal = vSub;
                                    DetalleImp.First(d => d.EsAlcaldia).Monto = vMonto;

                                }

                            }
                            else
                            {
                                if (DetalleImp.Count(d => d.EsAlcaldia) > 0)
                                    DetalleImp.Remove(DetalleImp.First(d => d.EsAlcaldia));
                            }

                        }
                    }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                gvDataImp.RefreshData();
            }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (Convert.ToDecimal(_Total).Equals(0))
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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

                    Entidad.Movimiento M;

                    M = new Entidad.Movimiento();
                    M.MovimientoTipoID = 41;
                    M.ProveedorID = _CajaChicaID;
                    M.EmpleadoID = (EsEmpleado ? (chkCargo.Checked ? emp.ID : 0) : 0);
                    M.ProveedorReferenciaID = (EsEmpleado ? 0 : Provee.ID);
                    M.CuentaID = _CuentaCajaChica;
                    M.UsuarioID = UsuarioID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Fecha; 
                    M.FechaVencimiento = Fecha;
                    M.MonedaID = _MonedaID;
                    M.TipoCambio = _TipoCambio;                   
                    M.Entregado = txtEntregado.Text;
                    M.Referencia = txtReferencia.Text;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.SubTotal = _SubTotal;
                    M.IVA = _IVA;
                    M.Monto = Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero);
                    M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(_Total) / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                    M.Numero = Parametros.General.GetNroSerie(IDEstacionServicio, IDSubSerie, 41, db);

                    if (M.Numero > 0)
                    {
                        var SerieUso = db.Series.Where(s => s.EstacionServicioID.Equals(M.EstacionServicioID) && s.SubEstacionID.Equals(IDSubSerie) && s.MovimientoTipoID.Equals(41)).ToList();

                        if (!ValidarNumeroCajaChica(Convert.ToInt32(M.Numero), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El número consecutivo de la caja chica '" + M.Numero + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        if (String.IsNullOrEmpty(txtEntregado.Text))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("Favor digite a quien será entregado el desembolso.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        if (SerieUso.Count > 0)
                        {
                            Entidad.Series SR = db.Series.First(s => s.EstacionServicioID.Equals(M.EstacionServicioID) && s.SubEstacionID.Equals(IDSubSerie) && s.MovimientoTipoID.Equals(41));
                            SR.NumeroActual = M.Numero;
                            db.SubmitChanges();
                        }
                        else
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("No se puede guardar el número consecutivo de la caja chica.", Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg("El número consecutivo de caja chica no puede ser 0 (cero).", Parametros.MsgType.warning);
                        trans.Rollback();
                        return false;
                    }


                    db.Movimientos.InsertOnSubmit(M);
                    db.SubmitChanges();

                    #region <<< REGISTRANDO COMPROBANTE >>>
                    List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                    var obj = from cds in Compronbante
                              join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                              join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                              select new
                              {
                                  Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                  Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                              };

                    if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    {
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                        trans.Rollback();
                        return false;
                    }
                    Compronbante.ForEach(linea =>
                        {
                            M.ComprobanteContables.Add(linea);
                            db.SubmitChanges();
                        });

                    db.SubmitChanges();

                    #endregion

                    #region <<< REGISTRAR RETENCIONES  >>>

                    if (DetalleImp.Count > 0)
                    {
                        M.Retenciones.AddRange(DetalleImp);
                        db.SubmitChanges();

                        if (DetalleImp.Count(c => !c.EsAlcaldia) > 0)
                        {
                            int n = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                            if (n > 0)
                                db.SeriesRetenciones.Single(s => s.EstacionServicioID.Equals(IDEstacionServicio) && s.SubEstacionServicioID.Equals(IDSubEstacion) && s.EsAlcaldia.Equals(false)).Numero = n + 1;
                        }

                        if (DetalleImp.Count(c => c.EsAlcaldia) > 0)
                        {
                            int n = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                            if (n > 0)
                                db.SeriesRetenciones.Single(s => s.EstacionServicioID.Equals(IDEstacionServicio) && s.SubEstacionServicioID.Equals(IDSubEstacion) && s.EsAlcaldia.Equals(true)).Numero = n + 1;
                        }

                        db.SubmitChanges();
                    }
                    #endregion

                    #region ::: REGISTRANDO DEUDOR :::

                    db.Deudors.InsertOnSubmit(new Entidad.Deudor { ProveedorID = _CajaChicaID, Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                    db.SubmitChanges();

                    if (EsEmpleado && EsCargo)
                    {
                        Entidad.Movimiento MC = new Entidad.Movimiento();
                        MC.MovimientoTipoID = 53;
                        MC.MovimientoReferenciaID = M.ID;
                        MC.EmpleadoID = M.EmpleadoID;
                        MC.UsuarioID = M.UsuarioID;
                        MC.FechaCreado = M.FechaCreado;
                        MC.FechaRegistro = M.FechaRegistro;
                        MC.MonedaID = M.MonedaID;
                        MC.CuentaID = (EsEmpleado ? _CuentaPorCobrarEmpleado : 0);
                        MC.TipoCambio = M.TipoCambio;
                        MC.Referencia = "C. C. Nro " + M.Numero.ToString();
                        MC.EstacionServicioID = M.EstacionServicioID;
                        MC.SubEstacionID = M.SubEstacionID;
                        MC.Comentario = M.Comentario;
                        MC.Monto = M.Monto;
                        MC.MontoMonedaSecundaria = M.MontoMonedaSecundaria;

                        db.Movimientos.InsertOnSubmit(MC);

                        db.Deudors.InsertOnSubmit(new Entidad.Deudor { EmpleadoID = emp.ID, Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                        db.SubmitChanges();
                    }
                    #endregion
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                    "Se registró la Caja Chica: " + M.Numero, this.Name);
                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    NextProvision = true;
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    _ToPrint = true;
                    IDPrint = M.ID;
                    return true;

                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();

                    try
                    { trans.Rollback(); }
                    catch (Exception ex2)
                    { Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, "Tipo : " + ex2.GetType().ToString() + 
                        Environment.NewLine + ex2.Message); }                    
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }
        
        private List<Entidad.ComprobanteContable> PartidasContable
        {
            get
            {
                try
                {
                        if (Provee == null && emp == null)
                        {
                            Parametros.General.DialogMsg("Debe seleccionar un Proveedor o Empleado.", Parametros.MsgType.warning);
                            return null;
                        }

                        List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                        int i = 1;

                        if (!EsEmpleado || !EsCargo)
                        {
                            #region <<< DETALLE_COMPROBANTE >>>

                            foreach (DataRow linea in DetalleComprobante.Rows)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Convert.ToInt32(linea["Linea"]),
                                    Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                    Fecha = Fecha,
                                    Descripcion = Convert.ToString(linea["Descripcion"]),
                                    Linea = i,
                                    CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                
                            }

                            #endregion

                            if (chkAplicaIva.Checked)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaIVACredito,
                                    Monto = Convert.ToDecimal(spIva.Value),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spIva.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                    Fecha = Fecha,
                                    Descripcion = "Acreditación IVA Doc. " + Numero.ToString(),
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            decimal vIR = 0;
                            string tIR = "";

                            vIR = Decimal.Round(DetalleImp.Where(d => !d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tIR = (DetalleImp.Count(d => !d.EsAlcaldia) > 0 ? DetalleImp.Where(d => !d.EsAlcaldia).First().Concepto : "");

                            if (vIR > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Provee.ImpuestoRetencionID,
                                    Monto = -Math.Abs(Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = Fecha,
                                    Descripcion = tIR,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            decimal vAlcaldia = 0;
                            string tAlcaldia = "";
                            vAlcaldia = Decimal.Round(DetalleImp.Where(d => d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tAlcaldia = (DetalleImp.Count(d => d.EsAlcaldia) > 0 ? DetalleImp.Where(d => d.EsAlcaldia).First().Concepto : "");

                            if (vAlcaldia > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaAlcaldiaPorPagarID,
                                    Monto = -Math.Abs(Decimal.Round(vAlcaldia, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(vAlcaldia / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = Fecha,
                                    Descripcion = tAlcaldia,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaCajaChica,
                                    Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = Fecha,
                                    Descripcion = (!EsEmpleado ? Provee.Nombre : emp.Nombres + " " + emp.Apellidos) + " – Doc.Caja Chica Nro. " + txtNumero.Text,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                        }
                        else
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaPorCobrarEmpleado,
                                Monto = Convert.ToDecimal(_Total),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(_Total) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = Comentario + " Doc. Caja Chica Nro. ",
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = _CuentaCajaChica,
                                Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = Comentario + " Doc. Caja Chica Nro. ",
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }


                        return CD;
                    
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }       
        }

        private void GetTotal(bool chkAplica, bool IsSub, bool IsIva, bool IsImp)
        {
            decimal iva = 0m;
            decimal vImp = 0m;

            if (chkAplica || IsSub)
            {
                if (chkAplicaIva.Checked)
                    iva = Decimal.Round(Convert.ToDecimal(spSub.Value * 0.15m), 2, MidpointRounding.AwayFromZero);
            }

            if (IsIva)
                iva = spIva.Value;

            spIva.Value = iva;

            spTotal.Value = Convert.ToDecimal(spSub.Value + iva);
            ViewDiferencia();

            if (!IsImp)
                ListaImpuesto();

            if (DetalleImp.Count > 0)
                vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);

            spIR.Value = vImp;

            _Total = Convert.ToDecimal(spTotal.Value - vImp);
        }
        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, NextProvision, RefreshMDI, IDPrint, _ToPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!NextProvision && !_Guardado)
            {
                if (DetalleComprobante.Rows.Count > 0 || string.IsNullOrEmpty(mmoComentario.Text))
                {
                    if (Parametros.General.DialogMsg("el documento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            Provee = null;
            EntidadAnterior = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Selecciona el Proveedor
        private void glkBeneficiario_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.chkBtnImpuesto.Checked = false;
                this.chkBtnImpuesto.Visible = false;
                this.DetalleImp.Clear();
                this.gvDataImp.RefreshData();

                if (Convert.ToInt32(glkBeneficiario.EditValue) > 0)
                {
                    if (!EsEmpleado)
                    {
                        emp = null;
                        Provee = db.Proveedors.Single(p => p.ID.Equals(Convert.ToInt32(glkBeneficiario.EditValue)));
                        txtRuc.Text = Provee.RUC;
                        _AplicaAlcaldia = RetieneAlcadia();

                        if (Provee.AplicaRetencion || _AplicaAlcaldia)
                        {
                            this.chkBtnImpuesto.Visible = true;
                            ListaImpuesto();                            
                        }

                        GetTotal(false, false, false, false);
                    }
                    else
                    {
                        Provee = null;
                        emp = db.Empleados.Single(p => p.ID.Equals(Convert.ToInt32(glkBeneficiario.EditValue)));
                        txtEntregado.Text = emp.Nombres + " " + emp.Apellidos;
                    }
                    rgOption.Properties.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #region <<< GRID_DETALLES >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            
            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "ConceptoID") != DBNull.Value)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ConceptoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ConceptoID"], "Debe Seleccionar un Concepto Contable");
                    e.ErrorText = "Debe Seleccionar un Concepto Contable";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ConceptoID"], "Debe Seleccionar un Concepto Contable");
                e.ErrorText = "Debe Seleccionar un Concepto Contable";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Descripcion
            //--
            if (view.GetRowCellValue(RowHandle, "Descripcion") == DBNull.Value)
            {
                view.SetColumnError(view.Columns["Descripcion"], "Debe de escribir una descripción.");
                e.ErrorText = "Debe de escribir una descripción.";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Valor             
            if (view.GetRowCellValue(RowHandle, "Monto") != DBNull.Value)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Monto")).Equals(0))
                {
                    view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                    e.ErrorText = "Debe Ingresar un valor en la linea.";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Monto"], "Debe Ingresar un valor en la linea.");
                e.ErrorText = "Debe Ingresar un valor en la linea.";
                e.Valid = false;
                Validate = false;
            }


            //-- Validar Columna de Centro de Costo             
            if (view.GetRowCellValue(RowHandle, "UsaCentroCosto") != DBNull.Value)
            {
                if (Convert.ToBoolean(view.GetRowCellValue(RowHandle, "UsaCentroCosto")).Equals(true))
                {
                    if (view.GetRowCellValue(RowHandle, "CentroCostoID") != DBNull.Value)
                    {
                        if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CentroCostoID")).Equals(0))
                        {

                            view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo.");
                            e.ErrorText = "Debe seleccionar un centro de costo.";
                            e.Valid = false;
                            Validate = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CentroCostoID"], "Debe seleccionar un centro de costo");
                        e.ErrorText = "Debe seleccionar un centro de costo";
                        e.Valid = false;
                        Validate = false;
                    }
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ConceptoID"], "Debe Seleccionar un Concepto Contable");
                e.ErrorText = "Debe Seleccionar un Concepto Contable";
                e.Valid = false;
                Validate = false;
            }
            ViewDiferencia();
        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        private void gvDataImp_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMontoImp)
                {
                    //if (gvDetalle.GetRowCellValue(e.RowHandle, "Pagado") != null)
                    //{
                    if (e.Value != null)
                    {
                        //if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "Pagado")).Equals(true))
                        if (Convert.ToDecimal(e.Value) > 0)
                        {
                            
                            //cambios Mykell Flores
                            decimal vMonto = 0;

                            if (gvDataImp.GetRowCellValue(e.RowHandle, "Monto") != null)
                                vMonto = Convert.ToDecimal(gvDataImp.GetRowCellValue(e.RowHandle, "Monto"));

                            //if (IDTipoPago.Equals(61))
                            //{
                            int vID = 0;

                            if (gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID") != null)
                                vID = Convert.ToInt32(gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID"));

                            decimal Porc = Convert.ToDecimal(db.CuentaContables.Where(cc=> cc.ID.Equals(vID)).FirstOrDefault().Porcentaje);

                            if (Porc != null)
                            {
                                gvDataImp.SetFocusedRowCellValue("SubTotal", Decimal.Round(Convert.ToDecimal(vMonto / (Porc / 100m)), 2, MidpointRounding.AwayFromZero));
                            }
                        }
                    }

                    gvDataImp.RefreshData();
                    GetTotal(false, false, true, true);
                    ViewDiferencia();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "ConceptoID") != DBNull.Value)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ConceptoID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ConceptoID"))));
                    gvDetalle.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    gvDetalle.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Linea", linea.Cuenta);

                    
                    if (gvDetalle.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(gvDetalle.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(gvDetalle.GetRowCellValue(gvDetalle.RowCount - 2, "Descripcion")));
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            if (e.Column == colValor)
            {
                //if (gvDetalle.GetRowCellValue(e.RowHandle, "Monto") == DBNull.Value)
                //    gvDetalle.SetRowCellValue(e.RowHandle, "Monto", 0.00);
                //else
                //{
                //    gvDetalle.RefreshData();
                    
                //}
            }

            ViewDiferencia();
            #endregion
        }
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {

                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }

                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                            + view.GetRowCellDisplayText(RowHandle, "ConceptoID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);
                            ViewDiferencia();
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    view.AddNewRow();
                }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "ConceptoID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                        ViewDiferencia();
                    }
                }
            }
        }

        //Validando la asignacion de Centro de Costo
        private void gvDetalle_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvDetalle.FocusedColumn == colCentroCosto)
            {
                if (gvDetalle.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(gvDetalle.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }

            }
        }
        
        #endregion

        //Mostrar el comprobante contable        
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDecimal(_Total).Equals(0))
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        return;
                }

                if (Provee != null || emp != null)
                {
                    var obj = from cds in PartidasContable.ToList()
                              join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                              join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                              select new
                              {
                                  Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                  Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                              };

                    if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                        return;
                    }

                    using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable Caja Chica";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
       
        //Botones para accesos directos
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.btnOK.Visible.Equals(true))
            {
                if (keyData == (Keys.F7))
                {
                    btnOK_Click_1(null, null);
                    return true;
                }
            }

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
                return true;
            }

            if (this.bntNew.Enabled.Equals(true))
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    bntNew_Click(null, null);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        //Boton nueva provision
        private void bntNew_Click(object sender, EventArgs e)
        {

            if (DetalleComprobante.Rows.Count > 0)
            {
                if (Parametros.General.DialogMsg("El documento actual tiene datos registrados. ¿Desea cancelar este documento y realizar uno nueva?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    NextProvision = true;
                    RefreshMDI = false;
                    ShowMsg = false;
                    this.Close();
                }
            }
            else
            {
                NextProvision = true;
                RefreshMDI = false;
                ShowMsg = false;
                this.Close();
            }

        } 
          
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            ViewDiferencia();
        }
        
        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaCaja.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
        }
        
        private void spSub_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spSub.Value) > 0)
            {
                GetTotal(false, true, false, false);
            }
        }

        private void spIva_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spIva.Value) > 0)
            {
                GetTotal(false, false, true, true);
                //spTotal.Value = Convert.ToDecimal(spSub.Value + spIva.Value);
            }
        }

        private void chkAplicaIva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaIva.Checked)
            {
                layoutControlItemIva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                GetTotal(true, false, false, false);
            }
            else if (!chkAplicaIva.Checked)
            {
                layoutControlItemIva.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                GetTotal(true, false, false, false);
            }
        }
        
        private void chkAplicaIva_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Provee != null)
            {
                if (!Provee.AplicaIVA && Convert.ToBoolean(e.NewValue).Equals(true))
                {
                    Parametros.General.DialogMsg("El proveedor no aplica IVA.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }

            }
        }
        
        //Selección de Beneficiario
        private void rgOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgOption.SelectedIndex.Equals(0))
            {
                spSub.Enabled = true;
                layoutControlItem17.Text = "Proveedor";
                layoutControlItem4.Text = "RUC";
                txtRuc.Text = "";
                glkBeneficiario.EditValue = null;
                glkBeneficiario.Properties.NullText = "<Seleccione al Proveedor>";
                EsEmpleado = false;

                glkBeneficiario.Properties.DataSource = null;
                glkBeneficiario.Properties.DataSource = (from p in db.Proveedors
                                                         where p.Activo
                                                         group p by new { p.ID, p.Codigo, p.Nombre } into gr
                                                         select new
                                                         {
                                                             ID = gr.Key.ID,
                                                             Codigo = gr.Key.Codigo,
                                                             Nombre = gr.Key.Nombre,
                                                             Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                         }).ToList();

                chkAplicaIva.Enabled = true;
                gridDetalle.Enabled = true;

                chkCargo.Checked = false;
                txtEntregado.Enabled = true;
                layoutControlItemChkCargo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemNumero.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (rgOption.SelectedIndex.Equals(1))
            {
                spSub.Enabled = true;
                layoutControlItem17.Text = "Empleado";
                layoutControlItem4.Text = "Nro INSS";
                glkBeneficiario.EditValue = null;
                glkBeneficiario.Properties.NullText = "<Seleccione al Empleado>";
                EsEmpleado = true;
                txtRuc.Text = "";
                glkBeneficiario.Properties.DataSource = null;
                glkBeneficiario.Properties.DataSource = (from em in db.Empleados
                                                         join p in db.Planillas on em.PlanillaID equals p.ID
                                                         where em.Activo && (p.EstacionServicioID == IDEstacionServicio || em.EsMultiEstacion)
                                                         select new { em.ID, em.Codigo, Nombre = em.Nombres + " " + em.Apellidos, Display = em.Codigo + " | " + em.Nombres + " " + em.Apellidos }).ToList();

                DetalleComprobante.Rows.Clear();
                gvDetalle.RefreshData();
                gridDetalle.Enabled = true;
                chkCargo.Checked = false;
                chkAplicaIva.Checked = false;
                chkAplicaIva.Enabled = false;
                chkCargo.Enabled = true;                
                txtEntregado.Enabled = false;
                txtReferencia.Text = "";
                layoutControlItemChkCargo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemNumero.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                
            }
        }

        private void chkBtnImpuesto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBtnImpuesto.Checked)
            {
                this.splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Both;
                this.chkBtnImpuesto.Image = Tesoreria.Properties.Resources.Ocultar;
                this.chkBtnImpuesto.Text = "Ocultar" + Environment.NewLine + "Retenciones";
            }
            else if (!chkBtnImpuesto.Checked)
            {
                this.splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Panel1;
                this.chkBtnImpuesto.Image = Tesoreria.Properties.Resources.Mostrar;
                this.chkBtnImpuesto.Text = "Mostrar" + Environment.NewLine + "Retenciones";
            }
        }

        private void chkCargo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCargo.Checked)
            {
                DetalleComprobante.Rows.Clear();
                gvDetalle.RefreshData();
                gridDetalle.Enabled = false;
            }
            else if (!chkCargo.Checked)
            {
                DetalleComprobante.Rows.Clear();
                gvDetalle.RefreshData();
                gridDetalle.Enabled = true;
            }

        }

        #endregion

    }
}