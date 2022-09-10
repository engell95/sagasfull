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
using SAGAS.Tesoreria.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

//PUNTOS..................
//revisar que al quitar los pagos quiten los valores del IR


namespace SAGAS.Tesoreria.Forms.Dialogs
{
    public partial class DialogPagoManual : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormPagoManual MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal Entidad.ProveedorUsadoPorEstacion EtUsado;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;        
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        private bool _AplicaAlcaldia = false;        
        private int ProveedorNoRegistrado = Parametros.Config.ProveedorNoRegistradoID();
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int _CuentaIVAXAcreditar = Parametros.Config.IVAPorAcreditar();
        private int _CuentaIVAAcreditado = Parametros.Config.IVAAcreditado();
        private Parametros.ListTipoPagoManual listadoPagoManual = new Parametros.ListTipoPagoManual();
        internal bool IsCalculating = false;
        internal int _OficinaCentralID = 0;
        internal int _ZonaManaguaID = 0;
        internal int _CuentaAnticipoProveedorID = 0;

        private string Concepto
        {
            get { return memoComentario.Text; }
            set { memoComentario.Text = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
        }

        private decimal _Iva
        {
            get { return Convert.ToDecimal(spIva.Value); }
            set { spIva.Value = value; }
        }

        private decimal _Nota
        {
            get { return Convert.ToDecimal(spNota.Value); }
            set { spNota.Value = value; }
        }
   
        private DateTime FechaPago
        {
            get { return Convert.ToDateTime(dateFechaPago.EditValue); }
            set { dateFechaPago.EditValue = value; }
        }
        
        private int IDBeneficiario
        {
            get { return Convert.ToInt32(glkBeneficiario.EditValue); }
            set { glkBeneficiario.EditValue = value; }
        }

        private string IDDeudor
        {
            get { return Convert.ToString(glkDeudor.EditValue); }
            set { glkDeudor.EditValue = value; }
        }

        private int IDProveedorReferencia
        {
            get { return Convert.ToInt32(glkProveedorReferencia.EditValue); }
            set { glkProveedorReferencia.EditValue = value; }
        }

        private int IDTipoPago
        {
            get { return Convert.ToInt32(lkTipoPago.EditValue); }
            set { lkTipoPago.EditValue = value; }
        }

        public bool _AplicaIR
        {
            get { return Convert.ToBoolean(chkAplicaIR.Checked); }
            set { chkAplicaIR.Checked = value; }
        }

        public bool _AplicaIVA
        {
            get { return Convert.ToBoolean(chkAplicaIva.Checked); }
            set { chkAplicaIva.Checked = value; }
        }

        public bool _AplicaProveedorReferencia
        {
            get { return Convert.ToBoolean(chkProveedorReferencia.Checked); }
            set { chkProveedorReferencia.Checked = value; }
        }
        
        public bool _AplicaNota
        {
            get { return Convert.ToBoolean(chkAplicarNota.Checked); }
            set { chkAplicarNota.Checked = value; }
        }
        
        private string Referencia
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
        }

        private decimal _Total
        {
            get { return Convert.ToDecimal(spMonto.Value); }
            set { spMonto.Value = value; }
        }
        
        private static Entidad.Proveedor prov;
        private static Entidad.Proveedor provReferencia;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private List<Parametros.ListIdDisplayValue> listaRetencionesPorc;
        private List<Parametros.ListIdIDCuenteDisplayCodeBool> listaDeudores;
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

        public DialogPagoManual(int UserID)
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

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);

                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                _OficinaCentralID = Parametros.Config.OficinaCentralID();
                _ZonaManaguaID = Parametros.Config.ZonaManaguaID();
                _CuentaAnticipoProveedorID = Parametros.Config.CuentaAnticipoProveedorID();

                lkTipoPago.Properties.DataSource = listadoPagoManual.GetListTipoPagoManual();
                glkBeneficiario.EditValue = null;
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
                                
                glkProveedorReferencia.EditValue = null;
                glkProveedorReferencia.Properties.DataSource = null;
                glkProveedorReferencia.Properties.DataSource = (from p in db.Proveedors
                                                         where p.Activo
                                                         group p by new { p.ID, p.Codigo, p.Nombre } into gr
                                                         select new
                                                         {
                                                             ID = gr.Key.ID,
                                                             Codigo = gr.Key.Codigo,
                                                             Nombre = gr.Key.Nombre,
                                                             Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                         }).ToList();
    
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Abreviatura });
                rpLkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre });
                lkCuentaImpuesto.DataSource = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Nombre }).OrderBy(o => o.Display).ToList();
                _AplicaIR = false;
                _AplicaProveedorReferencia = false;
                _AplicaIVA = false;
                _AplicaNota = false;
                _Iva = 0;

                rlkMoneda.DataSource = (from m in db.Monedas
                                        select new
                                        {
                                            Display = m.Simbolo,
                                            ID = m.ID
                                        }).ToList();

                this.CentrosCostos = from cto in db.CentroCostos
                                     join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                     where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                     select new Parametros.ListIdDisplay { ID = cto.ID, Display = cto.Nombre };

                Cuentas = (from cc in db.CuentaContables
                           join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                           join cces in db.CuentaContableEstacions on cc equals cces.CuentaContable
                           where cc.Detalle && !cc.Modular && cc.Activo && cces.EstacionID.Equals(IDEstacionServicio)
                           select new Parametros.ListIdDisplayCodeBool
                           {
                               ID = cc.ID,
                               Codigo = cc.Codigo,
                               Display = cc.Nombre,
                               valor = tc.UsaCentroCosto
                           }).OrderBy(o => o.Codigo);

                //--- Fill Combos Detalles --//
                gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                gridCuenta.DataSource = null;
                gridCuenta.DataSource = Cuentas;
                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

                //Centro Costo GRID
                lkCentroCosto.DataSource = CentrosCostos;
                lkCentroCosto.DisplayMember = "Display";
                lkCentroCosto.ValueMember = "ID";

                var listComprobante = (from cd in db.ComprobanteContables
                                       join cc in db.CuentaContables on cd.CuentaContableID equals cc.ID
                                       join tc in db.TipoCuentas on cc.TipoCuenta equals tc
                                       where cd.MovimientoID.Equals(0)
                                       select new { cd.CuentaContableID, cc.Nombre, cd.Monto, cd.Descripcion, cd.CentroCostoID, cd.Linea, tc.UsaCentroCosto }).OrderBy(o => o.Linea);

                this.DetalleComprobante = ToDataTable(listComprobante);

                this.bdsDetalleOtros.DataSource = this.DetalleComprobante;

                this.bdsImpuesto.DataSource = this.DetalleImp;

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
            Parametros.General.ValidateEmptyStringRule(dateFechaPago, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtReferencia, errRequiredField);
        }

        private bool ValidarReferencia(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? i.MovimientoTipoID.Equals(39) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.ID != Convert.ToInt32(ID) :
                          i.MovimientoTipoID.Equals(39) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio))
                          select i);
            
            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }
               
        private void SetMontoCheque()
        {
            try
            {
                decimal vMonto = 0;
                decimal vImp = 0;

                    vMonto = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);

                    if (IDTipoPago.Equals(55))
                        vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                                
                    _Total = vMonto - vImp;

                    GetDiferencias(true);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        public bool ValidarCampos(bool detalle)
        {
            if (Convert.ToInt32(glkBeneficiario.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Beneficiario para continuar con el proceso.", Parametros.MsgType.warning);
                return false;
            }

            if (lkTipoPago.EditValue == null || dateFechaPago.EditValue == null || String.IsNullOrEmpty(txtReferencia.Text))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS, Parametros.MsgType.warning);
                return false;
            }              
               
            if (_TipoCambio <= 0)
            {
                Parametros.General.DialogMsg("El tipo de Cambio debe ser mayor a 0 (cero)", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(FechaPago, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

            if (_AplicaProveedorReferencia)
            {
                if (IDProveedorReferencia <= 0)
                {
                    Parametros.General.DialogMsg("Debe de seleccionar un proveedor de referencia", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (_AplicaNota)
            {
                if (String.IsNullOrEmpty(IDDeudor))
                {
                    Parametros.General.DialogMsg("Debe de seleccionar un deudor para la nota", Parametros.MsgType.warning);
                    return false;
                }

                if (_Nota <= 0)
                {
                    Parametros.General.DialogMsg("Debe de digitar un valor a la nota", Parametros.MsgType.warning);
                    return false;
                }

            }

            if (detalle)
            {
                
                if (String.IsNullOrEmpty(memoComentario.Text))
                {
                    Parametros.General.DialogMsg("Debe de ingresar un Concepto al movimiento", Parametros.MsgType.warning);
                    return false;

                } 


                if (DetalleImp.Count > 0)
                {
                    if (IDTipoPago.Equals(55))
                    {
                        if (DetalleImp.Count(d => d.Numero <= 0 && d.Monto > 0) > 0)
                        {
                            Parametros.General.DialogMsg("Debe de digitar el Número de Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }

                        if (DetalleImp.Count(d => String.IsNullOrEmpty(d.Concepto) && d.Monto > 0) > 0)
                        {
                            Parametros.General.DialogMsg("Debe de digitar el Concepto de la Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }
                    else if (IDTipoPago.Equals(61))
                    {
                        if (DetalleImp.Count(d => d.Numero <= 0 && d.Monto > 0) > 0)
                        {
                            Parametros.General.DialogMsg("Debe de digitar el Número de Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }

                        if (DetalleImp.Count(d => String.IsNullOrEmpty(d.Concepto) && d.Monto > 0) > 0)
                        {
                            Parametros.General.DialogMsg("Debe de digitar el Concepto de la Retención correspondiente." + Environment.NewLine, Parametros.MsgType.warning);
                            return false;
                        }
                    }

                }

                if (Parametros.General.ListSES.Count > 0)
                {
                    if (IDSubEstacion <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                //if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                //{
                //    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                //    return false;
                //}
                                
            }

            return true;
        }

        /// <summary>
        /// VERIFICAR ALCALDIA PARA EL PROVEEDOR SELECCIONADO
        /// </summary>
        /// <returns></returns>
        private bool RetieneAlcadia()
        {
            try
            {
                if (prov != null)
                {
                    if (prov.AplicaAlcaldia)
                    {
                        var ES = db.EstacionServicios.SingleOrDefault(es => es.ID.Equals(IDEstacionServicio));

                        if (ES != null)
                        {
                            if (ES.ZonaID.Equals(_ZonaManaguaID))
                            {
                                //if (ES.ID.Equals(_OficinaCentralID))
                                //{
                                    if (ES.AplicaRetencionAlcaldia)
                                    {
                                        if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(prov.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                            return true;
                                        else
                                            return false;
                                    }
                                    else
                                        return false;
                                //}
                                //else
                                //    return false;
                            }
                            else
                            {
                                if (ES.AplicaRetencionAlcaldia)
                                {
                                    if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(prov.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                            }
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

   /// <summary>
        /// VERIFICAR ALCALDIA PARA EL PROVEEDOR REFERENCIA
   /// </summary>
   /// <param name="Fprov">PROVEEDOR REFERENCIA</param>
   /// <returns></returns>
        private bool RetieneAlcadia(Entidad.Proveedor Fprov)
        {
            try
            {
                if (Fprov != null)
                {
                    if (Fprov.AplicaAlcaldia)
                    {
                        var ES = db.EstacionServicios.SingleOrDefault(es => es.ID.Equals(IDEstacionServicio));

                        if (ES != null)
                        {
                            if (ES.ZonaID.Equals(_ZonaManaguaID))
                            {
                                //if (ES.ID.Equals(_OficinaCentralID))
                                //{
                                if (ES.AplicaRetencionAlcaldia)
                                {
                                    if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(Fprov.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                                //}
                                //else
                                //    return false;
                            }
                            else
                            {
                                if (ES.AplicaRetencionAlcaldia)
                                {
                                    if (db.ProveedorAlcaldiaEstacions.Count(pa => pa.ProveedorID.Equals(Fprov.ID) && pa.EstacionServicioID.Equals(ES.ID)) > 0)
                                        return true;
                                    else
                                        return false;
                                }
                                else
                                    return false;
                            }
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

        private void ListaImpuesto(int ID, bool valor, decimal Abonado)
        {
            try
            {
                if (IDTipoPago.Equals(55))
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    var Mov = db.Movimientos.FirstOrDefault(s => s.ID.Equals(ID));

                    #region <<< PAGOS DOCUMENTOS >>>

                    if (Mov != null)
                    {

                        decimal ValorRetener = 0, vSub = 0, vMonto = 0, vResta = 0;

                        vSub = Decimal.Round(Abonado, 2, MidpointRounding.AwayFromZero) - (Mov.IVAPagado ? 0 : Mov.IVA);

                        if (vSub < 0) vSub = 0;

                        ValorRetener = (Mov.SubTotal > 0 ? Mov.SubTotal : Mov.Monto);

                        if (ValorRetener > 0)
                        {
                            var prov = db.Proveedors.FirstOrDefault(p => p.ID.Equals(Mov.ProveedorID));

                            if (prov != null)
                            {
                                if (prov.AplicaRetencion && _AplicaIR)
                                {
                                    vMonto = 0;
                                    if (ValorRetener >= db.CuentaContables.Single(s => s.ID.Equals(prov.ImpuestoRetencionID)).Limite)
                                    {
                                        decimal IR = (db.CuentaContables.Single(cc => cc.ID.Equals(prov.ImpuestoRetencionID)).Porcentaje / 100m);
                                        vMonto = Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);
                                        vResta += Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);

                                        if (DetalleImp.Count(d => !d.EsAlcaldia).Equals(0))
                                        {
                                            if (valor)
                                            {
                                                int xr = 0;

                                                xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                                if (xr > 0)
                                                    colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                                DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = xr, SubTotal = vSub, EsAlcaldia = false, CuentaContableID = prov.ImpuestoRetencionID });
                                            }
                                        }
                                        else
                                        {
                                            if (valor)
                                            {
                                                DetalleImp.First(d => !d.EsAlcaldia).SubTotal += vSub;
                                                DetalleImp.First(d => !d.EsAlcaldia).Monto += vMonto;
                                            }
                                            else
                                            {
                                                DetalleImp.First(d => !d.EsAlcaldia).SubTotal -= vSub;
                                                DetalleImp.First(d => !d.EsAlcaldia).Monto -= vMonto;

                                                if (DetalleImp.Count(d => !d.EsAlcaldia && d.SubTotal <= 0 && d.Monto <= 0) > 0)
                                                    DetalleImp.Remove(DetalleImp.First(d => !d.EsAlcaldia && d.SubTotal.Equals(0) && d.Monto.Equals(0)));

                                            }
                                        }
                                    }
                                }

                                if (_AplicaAlcaldia && _AplicaIR)
                                {
                                    vMonto = 0;
                                    if (ValorRetener >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || prov.TipoProveedorID.Equals(ProveedorNoRegistrado))
                                    {
                                        decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(231)).Porcentaje / 100m);
                                        vMonto = Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);
                                        vResta += Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);

                                        if (DetalleImp.Count(d => d.EsAlcaldia).Equals(0))
                                        {
                                            if (valor)
                                            {
                                                int xa = 0;

                                                xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                                if (xa > 0)
                                                    colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                                DetalleImp.Add(new Entidad.Retencione { MovimientoID = ID, Monto = vMonto, Numero = xa, SubTotal = vSub, EsAlcaldia = true, CuentaContableID = 231 });
                                            }
                                        }
                                        else
                                        {
                                            if (valor)
                                            {
                                                DetalleImp.First(d => d.EsAlcaldia).SubTotal += vSub;
                                                DetalleImp.First(d => d.EsAlcaldia).Monto += vMonto;
                                            }
                                            else
                                            {
                                                DetalleImp.First(d => d.EsAlcaldia).SubTotal -= vSub;
                                                DetalleImp.First(d => d.EsAlcaldia).Monto -= vMonto;

                                                if (DetalleImp.Count(d => d.EsAlcaldia && d.SubTotal <= 0 && d.Monto <= 0) > 0)
                                                    DetalleImp.Remove(DetalleImp.First(d => d.EsAlcaldia && d.SubTotal.Equals(0) && d.Monto.Equals(0)));

                                            }
                                        }
                                    }

                                }
                            }
                        }

                    }

                    #endregion

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

        private void GetDiferencias(bool AutoIva)
        {
            try
            {
                decimal vCuentas = DetalleComprobante.AsEnumerable().Where(o => o["Monto"] != DBNull.Value).Sum(x => ((decimal)x["Monto"]));

                if (_AplicaIVA && AutoIva)
                    _Iva = Decimal.Round(vCuentas * 0.15m, 2, MidpointRounding.AwayFromZero);

                decimal vImp = 0;
                if (IDTipoPago.Equals(61))
                    vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);


                decimal vDiferencia = _Total - (vCuentas + _Iva + _Nota) + vImp;


                if (vDiferencia > 0)
                {
                    layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDif.Text = "DIFERENCIA";
                    spDiferencia.Value = vDiferencia;
                    spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Green;
                }
                else if (vDiferencia < 0)
                {
                    layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDif.Text = "DIFERENCIA";
                    spDiferencia.Value = vDiferencia;
                    spDiferencia.Properties.AppearanceReadOnly.BackColor = Color.Red;
                }
                else if (vDiferencia.Equals(0))
                {
                    layoutControlItemDif.Text = "<>";
                    spDiferencia.Value = vDiferencia;
                    layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private bool Guardar()
        {
            if (!ValidarCampos(true))
                return false;


            if (_Total <= 0)
            {
                Parametros.General.DialogMsg("El valor total del pago es 0 (cero), el valor debe ser mayor a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (DetalleM.Count(d => d.Litros > 0 && d.FechaRegistro.Date > FechaPago.Date) > 0)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
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

                    if (IDTipoPago.Equals(55))
                    {
                        #region <<< PAGO MANUAL >>>
                        
                        Entidad.Movimiento M;

                        M = new Entidad.Movimiento();

                        M.ProveedorID = IDBeneficiario;
                        M.MovimientoTipoID = 55;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.Referencia = Referencia;
                        M.UsuarioID = UsuarioID;
                        M.FechaRegistro = FechaPago;
                        M.Monto = _Total;
                        M.MonedaID = MonedaPrimaria;
                        M.TipoCambio = _TipoCambio;
                        M.MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Total);
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = Concepto;

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

                        #region ::: REGISTRANDO DEUDOR :::

                        Entidad.Deudor D = new Entidad.Deudor();
                        D.ProveedorID = IDBeneficiario;
                        D.MovimientoID = M.ID;
                        D.Valor = -Math.Abs(Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero));
                        db.Deudors.InsertOnSubmit(D);

                        db.SubmitChanges();

                        #endregion

                        #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>
                        string texto = "";

                        if (DetalleM.Count > 0)
                        {
                            foreach (Entidad.Movimiento linea in DetalleM.Where(d => d.Litros > 0))
                            {
                                Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(linea.ID));

                                if (MLine.Anulado)
                                {
                                    texto = (MLine.Numero.Equals(0) ? MLine.Referencia : MLine.Numero.ToString());
                                    break;
                                }

                                Entidad.Pago P = new Entidad.Pago();
                                P.MovimientoPagoID = MLine.ID;
                                P.Monto = linea.Litros;
                                MLine.Abono += linea.Litros;
                                MLine.Pagado = linea.Pagado;
                                if (!MLine.IVAPagado)
                                {
                                    MLine.IVAPagado = true;
                                    P.PagoIVA = true;
                                }

                                D.Pagos.Add(P);
                                db.SubmitChanges();
                            }
                        }

                        if (!String.IsNullOrEmpty(texto))
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + texto + ", esta Anulado.", Parametros.MsgType.warning);
                            return false;
                        }
                        #endregion

                        #region <<< REGISTRAR RETENCIONES  >>>

                        if (DetalleImp.Count > 0)
                        {
                            M.Retenciones.AddRange(DetalleImp);
                            db.SubmitChanges();
                        }
                        #endregion

                        #endregion

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se registró el Pago Manual: " + M.Referencia, this.Name);
                    }

                    if (IDTipoPago.Equals(61))
                    {
                        #region <<< PAGO ANTICIPO >>>

                        Entidad.Movimiento M;

                        M = new Entidad.Movimiento();

                        M.ProveedorID = IDBeneficiario;
                        M.ProveedorReferenciaID = IDProveedorReferencia;
                        M.MovimientoTipoID = 61;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.Referencia = Referencia;
                        M.UsuarioID = UsuarioID;
                        M.FechaRegistro = FechaPago;
                        M.Monto = _Total;
                        M.MonedaID = MonedaPrimaria;
                        M.TipoCambio = _TipoCambio;
                        M.MontoMonedaSecundaria = Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero);
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;
                        M.Comentario = Concepto;

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

                        #region ::: REGISTRANDO DEUDOR :::

                        Entidad.Deudor D = new Entidad.Deudor();
                        D.ProveedorID = IDBeneficiario;
                        D.MovimientoID = M.ID;
                        D.Valor = Math.Abs(Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero));
                        db.Deudors.InsertOnSubmit(D);

                        db.SubmitChanges();

                        #endregion

                        #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>
                        string texto = "";

                        if (DetalleM.Count > 0)
                        {
                            foreach (Entidad.Movimiento linea in DetalleM.Where(d => d.Litros > 0))
                            {
                                Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(linea.ID));

                                if (MLine.Anulado)
                                {
                                    texto = (MLine.Numero.Equals(0) ? MLine.Referencia : MLine.Numero.ToString());
                                    break;
                                }

                                Entidad.Pago P = new Entidad.Pago();
                                P.MovimientoPagoID = MLine.ID;
                                P.Monto = linea.Litros;
                                MLine.Abono += linea.Litros;                                
                                MLine.Pagado = linea.Pagado;
                                
                                D.Pagos.Add(P);
                                db.SubmitChanges();
                            }
                        }

                        if (!String.IsNullOrEmpty(texto))
                        {
                            trans.Rollback();
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El Documento: " + texto + ", esta Anulado.", Parametros.MsgType.warning);
                            return false;
                        }
                        #endregion

                        #region <<< REGISTRAR RETENCIONES  >>>

                        DetalleImp.Where(d => d.Monto > 0).ToList().ForEach(det =>
                            {
                                M.Retenciones.Add(new Entidad.Retencione { Concepto = det.Concepto, CuentaContableID = det.CuentaContableID, EsAlcaldia = det.EsAlcaldia, Monto = det.Monto, Numero = det.Numero, SubTotal = det.SubTotal });
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
                            });

                        #endregion

                        #region <<< NOTA DEBITO >>>
                        if (_AplicaNota)
                        {
                            Entidad.Movimiento MD = new Entidad.Movimiento();

                            if (listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).valor)
                            {
                                MD.ClienteID = listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).ID;
                            }
                            else if (!listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).valor)
                            {
                                MD.EmpleadoID = listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).ID;
                            }

                            MD.MovimientoTipoID = 27;
                            MD.UsuarioID = M.UsuarioID;
                            MD.FechaCreado = M.FechaCreado;
                            MD.FechaRegistro = M.FechaRegistro;
                            MD.MonedaID = M.MonedaID;
                            MD.TipoCambio = M.TipoCambio;
                            MD.Monto = Decimal.Round(_Nota, 2, MidpointRounding.AwayFromZero);
                            MD.MontoMonedaSecundaria = Decimal.Round((MD.Monto / MD.TipoCambio), 2, MidpointRounding.AwayFromZero);
                            MD.Referencia = "ND " + glkDeudor.Text + " Liq. Anticipo " + M.Referencia;
                            MD.EstacionServicioID = M.EstacionServicioID;
                            MD.SubEstacionID = M.SubEstacionID;
                            MD.MovimientoReferenciaID = M.ID;

                            db.Movimientos.InsertOnSubmit(MD);
                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
                            "Se registró la Nota de Debito: " + MD.Referencia, this.Name);

                            db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = MD.ClienteID, EmpleadoID = MD.EmpleadoID, Valor = Decimal.Round(_Nota, 2, MidpointRounding.AwayFromZero), MovimientoID = MD.ID });
                            db.SubmitChanges();
                        }
                        #endregion

                        #endregion

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se registró el Pago Anticipo: " + M.Referencia, this.Name);
                    }

                    ////para que actualice los datos del registro
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

        private List<Entidad.ComprobanteContable> PartidasContable
        {
            get
            {
                try
                {

                    List<Entidad.ComprobanteContable> CD = new List<Entidad.ComprobanteContable>();
                    int i = 0;

                    if (lkTipoPago.EditValue != null)
                    {
                        if (IDTipoPago.Equals(55))
                        {
                            #region <<< PAGOS MANUALES >>>
                            DetalleM.Where(d => d.Litros > 0).ToList().ForEach(item =>
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = (item.CuentaID > 0 ? item.CuentaID : prov.CuentaContableID),
                                    Monto = Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.Litros),
                                    Fecha = FechaPago,
                                    Descripcion = prov.Nombre + " Pago Doc. " + item.Referencia + " con Pago Manual " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            });

                            DetalleM.Where(d => d.Litros > 0).ToList().ForEach(item =>
                            {
                                //Decimal.Round(DetalleM.Where(m => m.Pagado).Sum(s => s.IVA), 2, MidpointRounding.AwayFromZero);

                                //***** MANEJO DEL IVA  *******//

                                if (!item.MovimientoTipoID.Equals(41) && ((item.MovimientoTipoID.Equals(3) && item.Credito) || item.MovimientoTipoID.Equals(5)) && item.IVA > 0 && !item.IVAPagado)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = _CuentaIVAAcreditado,
                                        Monto = Decimal.Round(item.IVA, 2, MidpointRounding.AwayFromZero),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.IVA / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.IVA),
                                        Fecha = FechaPago,
                                        Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Pago Manual " + Referencia,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });

                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = _CuentaIVAXAcreditar,
                                        Monto = -Math.Abs(Decimal.Round(item.IVA, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(item.IVA / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(item.IVA)),
                                        Fecha = FechaPago,
                                        Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Pago Manual " + Referencia,
                                        Linea = i,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }
                            });

                            decimal vIR = 0;
                            string tIR = "";

                            vIR = Decimal.Round(DetalleImp.Where(d => !d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tIR = (DetalleImp.Count(d => !d.EsAlcaldia) > 0 ? DetalleImp.Where(d => !d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => !d.EsAlcaldia).First().Numero.ToString() : "");

                            if (vIR > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = prov.ImpuestoRetencionID,
                                    Monto = -Math.Abs(Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(vIR)),
                                    Fecha = FechaPago,
                                    Descripcion = tIR,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            decimal vAlcaldia = 0;
                            string tAlcaldia = "";
                            vAlcaldia = Decimal.Round(DetalleImp.Where(d => d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tAlcaldia = (DetalleImp.Count(d => d.EsAlcaldia) > 0 ? DetalleImp.Where(d => d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => d.EsAlcaldia).First().Numero.ToString() : "");

                            if (vAlcaldia > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = 231,
                                    Monto = -Math.Abs(Decimal.Round(vAlcaldia, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(vAlcaldia / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(vAlcaldia)),
                                    Fecha = FechaPago,
                                    Descripcion = tAlcaldia,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            foreach (DataRow linea in DetalleComprobante.Rows)
                            {

                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                    Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"]) * -1),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal((Convert.ToDecimal(linea["Monto"]) / _TipoCambio) * -1), 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaPago,
                                    Descripcion = Convert.ToString(linea["Descripcion"]),
                                    Linea = i,
                                    CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                i++;
                            }

                            #endregion
                        }
                        else if (IDTipoPago.Equals(61))
                        {
                            #region <<< PAGOS ANTICIPOS >>>
                            //GASTOS.......................................................
                            foreach (DataRow linea in DetalleComprobante.Rows)
                            {
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                                    Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal((Convert.ToDecimal(linea["Monto"]) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaPago,
                                    Descripcion = Convert.ToString(linea["Descripcion"]),
                                    Linea = i,
                                    CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                i++;
                            }       

                            //***** MANEJO DEL IVA  *******//
                            if (_AplicaIVA)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaIVAAcreditado,
                                    Monto = Decimal.Round(_Iva, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(_Iva / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                    Fecha = FechaPago,
                                    Descripcion = prov.Nombre + " Acreditación IVA con Pago Anticipo " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }
                                                        
                            decimal vIR = 0;
                            string tIR = "";

                            vIR = Decimal.Round(DetalleImp.Where(d => !d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tIR = (DetalleImp.Count(d => !d.EsAlcaldia) > 0 ? DetalleImp.Where(d => !d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => !d.EsAlcaldia).First().Numero.ToString() : "");

                            if (vIR > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = (_AplicaProveedorReferencia ? provReferencia.ImpuestoRetencionID : prov.ImpuestoRetencionID),
                                    Monto = -Math.Abs(Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaPago,
                                    Descripcion = tIR,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            decimal vAlcaldia = 0;
                            string tAlcaldia = "";
                            vAlcaldia = Decimal.Round(DetalleImp.Where(d => d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                            tAlcaldia = (DetalleImp.Count(d => d.EsAlcaldia) > 0 ? DetalleImp.Where(d => d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => d.EsAlcaldia).First().Numero.ToString() : "");

                            if (vAlcaldia > 0)
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = 231,
                                    Monto = -Math.Abs(Decimal.Round(vAlcaldia, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(vAlcaldia / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaPago,
                                    Descripcion = tAlcaldia,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            DetalleM.Where(d => d.Litros > 0).ToList().ForEach(item =>
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaAnticipoProveedorID,
                                    Monto = -Math.Abs(Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaPago,
                                    Descripcion = prov.Nombre + " Liq. Doc. " + item.Referencia + " con Pago Anticipo " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            });

                            if (_Nota > 0)
                            {
                                int vCuenta = 0;

                                if (listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).valor)
                                {
                                    vCuenta = db.Clientes.Single(c => c.ID.Equals(listaDeudores.Single(l => l.Cuenta.Equals(IDDeudor) && l.Display.Equals(glkDeudor.Text)).ID)).CuentaContableID;
                                }
                                else
                                {
                                    vCuenta = 100;
                                }

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = vCuenta,
                                    Monto = Math.Abs(Decimal.Round(_Nota, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Math.Abs(Decimal.Round(_Nota / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaPago,
                                    Descripcion = " Nota Debito " + glkDeudor.Text + " por Pago Anticipo " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            #endregion
                        }
                        else
                            return null;
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
            MDI.CleanDialog(ShowMsg, Next, IDPrint, RefreshMDI, false);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_Guardado)
                {
                    if (DetalleM.Count > 0)
                    {
                        if (Parametros.General.DialogMsg("El Pago Manual actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        {
                            Next = false;
                            e.Cancel = true;
                            return;
                        }
                    }
                }

                if (prov != null)
                {
                    if (prov.AplicaBloqueo && IDTipoPago.Equals(55))
                    {
                        if (EtUsado != null)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                            db.ProveedorUsadoPorEstacions.DeleteOnSubmit(db.ProveedorUsadoPorEstacions.Single(s => s.ID.Equals(EtUsado.ID)));
                            db.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {  }
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

            if (keyData == (Keys.F8))
            {
                btnShowComprobante_Click(null, null);
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                bntNew_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Nuevo ROC
        private void bntNew_Click(object sender, EventArgs e)
        {
            Next = true;
            RefreshMDI = false;
            ShowMsg = false;
            this.Close();
        }

        //Selección del Deudor
        private void glkClient_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(glkBeneficiario.EditValue) > 0)
            {
                try
                {
                    prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));

                    _AplicaAlcaldia = RetieneAlcadia();

                    var obj = (from d in db.Deudors
                               join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio)) on d.MovimientoID equals m.ID
                               join p in db.Proveedors.Where(c => c.ID.Equals(prov.ID)) on d.ProveedorID equals p.ID
                               group d by new { d.ProveedorID, p.Nombre } into gr
                               select new
                               {
                                   ID = gr.Key.ProveedorID,
                                   Cliente = gr.Key.Nombre,
                                   suma = gr.Sum(s => s.Valor)
                               }).ToList();

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        //Cambiar las opciones del ROC
        private void rgOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void dateFechaRecibo_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaPago.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaPago.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaPago.Date)).First().Valor : 0m);

            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Cargar la lista de los pagos del Deudor
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(false))
                {                    
                    this.bdsDetalle.DataSource = null;
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));


                        if (IDTipoPago.Equals(55))//PAGO MANUAL
                        {
                            if (prov.Bloqueado)
                            {
                                Parametros.General.DialogMsg("El Proveedor " + prov.Codigo + " | " + prov.Nombre + ", esta bloquedo. Favor comunicarse con Oficina Central. ", Parametros.MsgType.warning);
                                return;
                            }

                            if (db.ProveedorUsadoPorEstacions.Count(c => c.ProveedorID.Equals(prov.ID) && c.EstacionID.Equals(IDEstacionServicio) && c.SubEstacionID.Equals(IDSubEstacion)) > 0)
                            {
                                Parametros.General.DialogMsg("El Proveedor " + prov.Codigo + " | " + prov.Nombre + ", esta siendo utilizado por otro usuario para la Estación de Servicio Seleccionada. Favor comunicarse con Oficina Central. ", Parametros.MsgType.warning);
                                return;
                            }

                            if (db.Proveedors.Where(p => p.ID.Equals(prov.ID)).FirstOrDefault().MostrarCajaChicaEnPagoManual)
                            {
                                DetalleM = (from m in db.Movimientos
                                            where (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(5) || m.MovimientoTipoID.Equals(66) || m.MovimientoTipoID.Equals(84) || m.MovimientoTipoID.Equals(41)) && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            && !m.Anulado && !m.Pagado && m.ProveedorID.Equals(prov.ID)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }
                            else
                            {
                                DetalleM = (from m in db.Movimientos
                                            where ((m.MovimientoTipoID.Equals(3) && m.Credito) || m.MovimientoTipoID.Equals(5) ||  m.MovimientoTipoID.Equals(84) || m.MovimientoTipoID.Equals(66)) && m.EstacionServicioID.Equals(IDEstacionServicio)
                                            && !m.Anulado && !m.Pagado && m.ProveedorID.Equals(prov.ID)
                                            select m).OrderBy(o => o.FechaRegistro).ToList();
                            }
                            if (prov.AplicaAbono)
                            {
                                colMonto.OptionsColumn.AllowEdit = true;
                                colMonto.OptionsColumn.AllowFocus = true;
                            }

                            colMontoUS.Visible = false;
                                                
                        }
                        else if (IDTipoPago.Equals(61))//LIQUIDACION ANTICIPO
                        {
                            DetalleM = (from m in db.Movimientos
                                        where ((m.MovimientoTipoID.Equals(55) && m.MovimientoReferenciaID > 0) || (m.MovimientoTipoID.Equals(39) && m.EsAnticipo) || (m.MovimientoTipoID.Equals(82) && m.EsAnticipo))
                                        && m.EstacionServicioID.Equals(IDEstacionServicio)
                                        && !m.Anulado && !m.Pagado && m.ProveedorID.Equals(prov.ID)
                                        select m).OrderBy(o => o.FechaRegistro).ToList();

                            if (DetalleM.Count(c => c.MonedaID.Equals(MonedaSecundaria)) > 0)
                            {
                                //colMontoUS.VisibleIndex = 7;
                                colMontoUS.Visible = true;
                            }
                            else
                                colMontoUS.Visible = false;


                            splitContainerControlMain.PanelVisibility = SplitPanelVisibility.Both;
                            layoutControlItemchkIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layoutControlItemIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layoutControlItemAplicaNota.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            listaRetencionesPorc = new List<Parametros.ListIdDisplayValue>();

                            if (_AplicaProveedorReferencia)
                            {
                                provReferencia = db.Proveedors.Single(p => p.ID.Equals(IDProveedorReferencia));

                                if (!provReferencia.ImpuestoRetencionID.Equals(0))
                                {
                                    int xr = 0;

                                    xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                    if (xr > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                    DetalleImp.Add(new Entidad.Retencione { Monto = 0, Numero = xr, SubTotal = 0, EsAlcaldia = false, CuentaContableID = provReferencia.ImpuestoRetencionID });
                                    listaRetencionesPorc.Add(new Parametros.ListIdDisplayValue { ID = provReferencia.ImpuestoRetencionID, Display = "IR", Value = (db.CuentaContables.Count(c => c.ID.Equals(provReferencia.ImpuestoRetencionID)) > 0 ? db.CuentaContables.First(c => c.ID.Equals(provReferencia.ImpuestoRetencionID)).Porcentaje : 0) });
                                }

                                if (RetieneAlcadia(provReferencia))
                                {
                                    int xa = 0;

                                    xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                    if (xa > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;


                                    DetalleImp.Add(new Entidad.Retencione { Monto = 0, Numero = xa, SubTotal = 0, EsAlcaldia = true, CuentaContableID = 231 });
                                    listaRetencionesPorc.Add(new Parametros.ListIdDisplayValue { ID = 231, Display = "ALMA", Value = (db.CuentaContables.Count(c => c.ID.Equals(231)) > 0 ? db.CuentaContables.First(c => c.ID.Equals(231)).Porcentaje : 0) });
                                }

                                if (provReferencia.AplicaIVA)
                                {
                                    _AplicaIVA = true;
                                    _Iva = 0m;
                                }
                            }
                            else if (!_AplicaProveedorReferencia)
                            {

                                if (!prov.ImpuestoRetencionID.Equals(0))
                                {
                                    int xr = 0;

                                    xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                                    if (xr > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                    DetalleImp.Add(new Entidad.Retencione { Monto = 0, Numero = xr, SubTotal = 0, EsAlcaldia = false, CuentaContableID = prov.ImpuestoRetencionID });
                                    listaRetencionesPorc.Add(new Parametros.ListIdDisplayValue { ID = prov.ImpuestoRetencionID, Display = "IR", Value = (db.CuentaContables.Count(c => c.ID.Equals(prov.ImpuestoRetencionID)) > 0 ? db.CuentaContables.First(c => c.ID.Equals(prov.ImpuestoRetencionID)).Porcentaje : 0) });
                                }

                                if (prov.AplicaIVA)
                                {
                                    _AplicaIVA = true;
                                    _Iva = 0m;
                                }


                                if (_AplicaAlcaldia)
                                {
                                    int xa = 0;

                                    xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));

                                    if (xa > 0)
                                        colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;


                                    DetalleImp.Add(new Entidad.Retencione { Monto = 0, Numero = xa, SubTotal = 0, EsAlcaldia = true, CuentaContableID = 231 });
                                    listaRetencionesPorc.Add(new Parametros.ListIdDisplayValue { ID = 231, Display = "ALMA", Value = (db.CuentaContables.Count(c => c.ID.Equals(231)) > 0 ? db.CuentaContables.First(c => c.ID.Equals(231)).Porcentaje : 0) });
                                }
                            }
                            gvDataImp.RefreshData();

                            listaDeudores = new List<Parametros.ListIdIDCuenteDisplayCodeBool>();

                            listaDeudores.AddRange(from cl in db.Clientes
                                                   where cl.Activo
                                                   select new Parametros.ListIdIDCuenteDisplayCodeBool { ID = cl.ID, Cuenta = cl.ID.ToString() + "-1", Codigo = cl.Codigo, Display = cl.Nombre, valor = true });
                                                
                            listaDeudores.AddRange(from cl in db.Empleados
                                                    where cl.Activo
                                                   select new Parametros.ListIdIDCuenteDisplayCodeBool { ID = cl.ID, Cuenta = cl.ID.ToString() + "-0", Codigo = cl.Codigo, Display = cl.Nombres + " " + cl.Apellidos, valor = false });

                            glkDeudor.Properties.DataSource = listaDeudores;
                            glkDeudor.EditValue = null;

                            colMonto.OptionsColumn.AllowEdit = true;
                            colMonto.OptionsColumn.AllowFocus = true;
                        }

                        if (DetalleM.Count <= 0)
                        {
                            Parametros.General.DialogMsg("PROVEEDOR / BENEFICIARIO NO TIENE DOCUMENTOS PENDIENTES DE PAGO", Parametros.MsgType.warning);
                            return;
                        }


                        this.bdsDetalle.DataSource = DetalleM;
                        gvDetalle.RefreshData();
                        SetMontoCheque();

                        gridDetalle.Visible = true;
                        this.glkBeneficiario.Enabled = false;
                        this.btnLoad.Enabled = false;
                        this.lkTipoPago.Properties.ReadOnly = true;
                        this.chkAplicaIR.Properties.ReadOnly = true;
                        this.chkProveedorReferencia.Properties.ReadOnly = true;

                        if (IDTipoPago.Equals(55) && prov.AplicaBloqueo)
                        {
                            EtUsado = new Entidad.ProveedorUsadoPorEstacion();
                            EtUsado.EstacionID = IDEstacionServicio;
                            EtUsado.SubEstacionID = IDSubEstacion;
                            EtUsado.UserID = UsuarioID;
                            EtUsado.Fecha = Convert.ToDateTime(db.GetDateServer());
                            EtUsado.ProveedorID = prov.ID;
                            db.ProveedorUsadoPorEstacions.InsertOnSubmit(EtUsado);
                            db.SubmitChanges();
                        }
                        
                }
                glkBeneficiario.Properties.ReadOnly = true;
                txtReferencia.Properties.ReadOnly = true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

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


        private void gvDataImp_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMontoImp)
                {
                    if (e.Value != null)
                    {
                        //if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "Pagado")).Equals(true))
                        if (Convert.ToDecimal(e.Value) > 0)
                        {
                            decimal vMonto = 0;

                            if (gvDataImp.GetRowCellValue(e.RowHandle, "Monto") != null)
                                vMonto = Convert.ToDecimal(gvDataImp.GetRowCellValue(e.RowHandle, "Monto"));

                            if (IDTipoPago.Equals(61))
                            {
                                int vID = 0;

                                if (gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID") != null)
                                    vID = Convert.ToInt32(gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID"));

                                var Porc = listaRetencionesPorc.FirstOrDefault(l => l.ID.Equals(vID));

                                    if (Porc != null)
                                    {
                                    gvDataImp.SetFocusedRowCellValue("SubTotal", Decimal.Round(Convert.ToDecimal( vMonto / (Porc.Value / 100m) ), 2, MidpointRounding.AwayFromZero));
                                    }

                            }
                        }
                    }

                    gvDataImp.RefreshData();
                    SetMontoCheque();
                    //SetMontoCheque();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cambio valor de Retenciones
        private void gvDataImp_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {

            if (e.Column == colMonto)
            {
                //if (gvDetalle.GetRowCellValue(e.RowHandle, "Pagado") != null)
                //{
                if (e.Value != null)
                {
                    //if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "Pagado")).Equals(true))
                    if (Convert.ToDecimal(e.Value) > 0)
                    {
                        decimal vMonto = 0;

                        if (gvDataImp.GetRowCellValue(e.RowHandle, "Monto") != null)
                            vMonto = Convert.ToDecimal(gvDataImp.GetRowCellValue(e.RowHandle, "Monto"));
                    }
                }

                gvDataImp.RefreshData();
                SetMontoCheque();
            }
        }

        private void gvDetalle_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colPagado && !IsCalculating)
                {
                    if (e.Value != null)
                    {
                        IsCalculating = true;
                        if (e.Value.Equals(true))
                        {
                            decimal vSaldo = 0;

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                            gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);

                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), Convert.ToBoolean(e.Value), vSaldo);

                        }
                        else
                        {
                            decimal vSaldoAnreriot = 0;

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                                vSaldoAnreriot = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));


                            gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0m);
                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), Convert.ToBoolean(e.Value), vSaldoAnreriot);
                        }
                        IsCalculating = false;
                        gvDetalle.RefreshData();
                        SetMontoCheque();
                    }
                }
                
            }
            catch (Exception ex)
            {
                IsCalculating = false;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colMonto && !IsCalculating)
                {
                    IsCalculating = true;
                    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                    {
                        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                        {
                            decimal vSaldo = 0, vAbono = 0, vNuevoSaldo = 0; ;

                            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "NuevoSaldo") != null)
                                vNuevoSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "NuevoSaldo"));

                            //if (!Decimal.Round(vSaldo - vNuevoSaldo, 2, MidpointRounding.AwayFromZero).Equals(vSaldo))
                            //{
                            //    ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), false, vNuevoSaldo);
                            //}

                            if (vAbono > vSaldo)
                            {
                                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                                gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                            }
                            else
                            {
                                if (vAbono.Equals(vSaldo))
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                                else
                                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);

                                ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), (vAbono > 0 ? true : false), (vAbono > 0 ? vAbono : vSaldo));
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
                    IsCalculating = false;
                    gvDetalle.RefreshData();
                    SetMontoCheque();
                }
            }
            catch (Exception ex)
            {
                IsCalculating = false;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
           
        }

        #endregion

        #region <<< GRID CUENTAS CONTABLES >>>

        private void bgvCuentas_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            GetDiferencias(true);
        }

        //Validar Filas de las Cuentas
        private void bgvCuentas_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "CuentaContableID") != DBNull.Value)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "CuentaContableID")) == 0)
                {
                    view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                    e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
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
                view.SetColumnError(view.Columns["CuentaContableID"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                e.Valid = false;
                Validate = false;
            }

            GetDiferencias(true);
        }

        //Mensaje Validación del detalle
        private void bgvCuentas_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void bgvCuentas_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Linea", 0);

                    if (bgvCuentas.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(bgvCuentas.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        bgvCuentas.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(bgvCuentas.GetRowCellValue(bgvCuentas.RowCount - 2, "Descripcion")));
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            GetDiferencias(true);

            #endregion
        }

        //metodos para borrar y agregar nueva fila
        private void bgvCuentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        bgvCuentas.RefreshData();
                        GetDiferencias(true);
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                view.AddNewRow();
                bgvCuentas.RefreshData();
            }
        }

        private void bgvCuentas_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (bgvCuentas.FocusedColumn == colCentroCosto)
            {
                if (bgvCuentas.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(bgvCuentas.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }
            }
        }

        #endregion
        //Botón para mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(true))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante de Pago Manual";
                        nf.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Mostrar Impuesto
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

        //private void spSubMonto_EditValueChanged(object sender, EventArgs e)
        //{
        //    ListaImpuesto(0, false, 0);
        //}

        private void chkAnticipo_CheckedChanged(object sender, EventArgs e)
        {
            
        }
               

        private void glkBeneficiario_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
           
        }

        private void lkTipoPago_EditValueChanged(object sender, EventArgs e)
        {
            if (lkTipoPago.EditValue != null)
            {
                try
                {
                    if (IDTipoPago.Equals(55))
                    {
                        layoutControlItemAplicaIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItemProveRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        _AplicaProveedorReferencia = false;
                    }
                    else if (IDTipoPago.Equals(61))
                    {
                        layoutControlItemProveRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItemAplicaIR.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        _AplicaIR = false;
                    }
                    else
                    {
                        lkTipoPago.EditValue = null;
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }

            }
        }
        
        private void chkProveedorReferencia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProveedorReferencia.Checked)
            {
                layoutControlItemLkProvRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (!chkProveedorReferencia.Checked)
            {
                layoutControlItemLkProvRef.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                glkProveedorReferencia.EditValue = null;
            }
        }

        private void chkAplicaIva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaIva.Checked)
            {
                spIva.Properties.ReadOnly = false;
            }
            else if (!chkAplicaIva.Checked)
            {
                spIva.Value = 0m;
                spIva.Properties.ReadOnly = true;
            }
        }

        private void spIva_EditValueChanged(object sender, EventArgs e)
        {
            GetDiferencias(false);
        }

        private void chkAplicarNota_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicarNota.Checked)
            {
                layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemMontoNota.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (!chkAplicarNota.Checked)
            {
                glkDeudor.EditValue = null;
                _Nota = 0m;
                layoutControlItemMontoNota.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemDeudor.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void spNota_EditValueChanged(object sender, EventArgs e)
        {
            GetDiferencias(false);
        }

        #endregion

        private void gvDetalle_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)sender;
            int RowHandle = view.FocusedRowHandle;

            if (e.Column == colMoneda)
            {
                int value = Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "MonedaID"));
                if (value == 1)
                    e.Appearance.BackColor = Color.DeepSkyBlue;
                else
                    e.Appearance.BackColor = Color.LightGreen;
            }
        }


    }
}