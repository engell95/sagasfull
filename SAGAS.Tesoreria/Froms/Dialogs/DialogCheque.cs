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
    public partial class DialogCheque : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCheque MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;        
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        internal bool EsEmpleado = false;
        internal bool _PagoMasivo = false;
        private bool _AplicaAlcaldia = false;
        private int ProveedorNoRegistrado = Parametros.Config.ProveedorNoRegistradoID();
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int _CuentaIVAXAcreditar = Parametros.Config.IVAPorAcreditar();
        private int _CuentaIVAAcreditado = Parametros.Config.IVAAcreditado();
        internal bool _AplicaAnticipo = false;
        internal decimal _DifAnticipo = 0;
        internal int _MonedaID = 0;
        internal int _OficinaCentral = 0;
        internal int _CuentaPerdidaCambiaria = 0;
        internal int _CECOOficinaDiferencialID = 0;
        internal int _CECOEstacionesDiferencialID = 0;
        //internal bool _FromST = false;

        private List<Entidad.ConceptoContable> EtConcepto;

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
                
        private DateTime FechaCheque
        {
            get { return Convert.ToDateTime(dateFechaCheque.EditValue); }
            set { dateFechaCheque.EditValue = value; }
        }

        private int IDConepto
        {
            get { return Convert.ToInt32(lkConcepto.EditValue); }
            set { lkConcepto.EditValue = value; }
        }

        private int IDBeneficiario
        {
            get { return Convert.ToInt32(glkBeneficiario.EditValue); }
            set { glkBeneficiario.EditValue = value; }
        }


        private int IDCuentaBancaria
        {
            get { return Convert.ToInt32(lkCuentaBancaria.EditValue); }
            set { lkCuentaBancaria.EditValue = value; }
        }

        private int Numero
        {
            get { return Convert.ToInt32(spNumero.Value); }
            set { spNumero.Value = value; }
        }

        private decimal _SubMonto
        {
            get { return Convert.ToDecimal(spSubMonto.Value); }
            set { spSubMonto.Value = value; }
        }
        private decimal _TotalSubI
        {
            get { return Convert.ToDecimal(spTotal.Value); }
            set { spTotal.Value = value; }
        }
        private decimal _Iva
        {
            get { return Convert.ToDecimal(spIVA.Value); }
            set { spIVA.Value = value; }
        }

        private decimal _Total
        {
            get { return Convert.ToDecimal(spMonto.Value); }
            set { spMonto.Value = value; }
        }
        
        public bool _EsAnticipo
        {
            get { return Convert.ToBoolean(chkAnticipo.Checked); }
            set { chkAnticipo.Checked = value; }
        }

        public decimal _DiferencialCambiario
        {
            get { return Convert.ToDecimal(spDifCam.Value); }
            set { spDifCam.Value = value; }
        }

        private static Entidad.Proveedor prov;
        private static Entidad.Empleado empl;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;

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

        private List<Entidad.Movimiento> DetalleM = new List<Entidad.Movimiento>();
        //public List<Entidad.Movimiento> DetalleM
        //{
        //    get { return M; }
        //    set
        //    {
        //        M = value;
        //        this.bdsDetalle.DataSource = this.M;
        //    }
        //}

        #endregion

        #region *** INICIO ***

        public DialogCheque(int UserID, bool IsEdit)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = IsEdit;
            this.colValue.Visible = false;
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
                rgOption.SelectedIndex = -1;
                glkBeneficiario.EditValue = null;

                _AplicaAnticipo = Parametros.General.SystemOptionAcces(UsuarioID, "chkAnticipoCheque");
                spTC.Properties.ReadOnly = !Parametros.General.SystemOptionAcces(UsuarioID, "chequeCambiarTipoCambio");
                EtConcepto = db.ConceptoContables.Where(o => o.Activo && o.EsCheque).ToList();


                _OficinaCentral = Parametros.Config.OficinaCentralID();
                _CuentaPerdidaCambiaria = Parametros.Config.CuentaPerdidaCambiariaID();
                _CECOOficinaDiferencialID = Parametros.Config.CECOOficinaDiferencialID();
                _CECOEstacionesDiferencialID = Parametros.Config.CECOEstacionesDiferencialID();
                //this.layoutControlItemrgOptions.Width = 60;
                //this.layoutControlItemFecha.Width = 150;

                //int number = 1;
                //if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(22)) > 0)
                //{
                //    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(22)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                //}

                //txtNumero.Text = number.ToString("000000000");

                lkCuentaBancaria.Properties.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Abreviatura });
                rpLkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre });
                lkConcepto.Properties.DataSource = EtConcepto.Select(s => new { s.ID, Display = s.Nombre }).ToList();
                //db.ConceptoContables.Where(cc => cc.Activo && cc.EsCheque)
                lkCuentaImpuesto.DataSource = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Nombre }).OrderBy(o => o.Display).ToList();

                //dateFechaCheque.EditValue = Convert.ToDateTime(db.GetDateServer());

                //dateHasta.EditValue = dateFechaCheque.EditValue;

                //if (dateFechaCheque.EditValue != null)
                //    _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaCheque.Date)) > 0 ?
                //            db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaCheque.Date)).First().Valor : 0m);

                this.bdsImpuesto.DataSource = this.DetalleImp;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (Editable)
                {
                    Concepto = EntidadAnterior.Comentario;
                    FechaCheque = EntidadAnterior.FechaRegistro;
                    dateHasta.EditValue = FechaCheque;
                    _TipoCambio = EntidadAnterior.TipoCambio;

                    if (EntidadAnterior.EmpleadoID.Equals(0))
                        rgOption.SelectedIndex = 0;
                    else
                        rgOption.SelectedIndex = 1;

                    IDCuentaBancaria = EntidadAnterior.CuentaBancariaID;
                    lkCuentaBancaria.EditValue = IDCuentaBancaria;
                    Numero = EntidadAnterior.Numero;
                    _Total = EntidadAnterior.Monto;
                    IDBeneficiario = (!EntidadAnterior.ProveedorID.Equals(0) ? EntidadAnterior.ProveedorID : EntidadAnterior.EmpleadoID);
                    _EsAnticipo = EntidadAnterior.EsAnticipo;

                    if (!EsEmpleado && !_EsAnticipo)
                        btnLoad_Click(null, null);
                    else
                    {

                        chkAnticipo.Enabled = false;
                        IDConepto = EntidadAnterior.ConceptoContableID;
                        if (!EsEmpleado)
                        {
                            prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));
                            empl = null;
                            _AplicaAlcaldia = RetieneAlcadia();

                            db.Retenciones.Where(r => r.MovimientoID.Equals(EntidadAnterior.ID)).ToList().ForEach(det =>
                            {
                                DetalleImp.Add(new Entidad.Retencione { Monto = det.Monto, Numero = det.Numero, Concepto = det.Concepto, SubTotal = det.SubTotal, EsAlcaldia = det.EsAlcaldia, CuentaContableID = det.CuentaContableID });
                            });
                            gvDataImp.RefreshData();

                            //db.Retenciones.Where(r => r.MovimientoID.Equals(EntidadAnterior.ID)).ToList().ForEach(det =>
                            //{
                            //    DetalleImp.Add(new Entidad.Retencione { Monto = det.Monto, Numero = det.Numero, Concepto = det.Concepto, SubTotal = det.SubTotal, EsAlcaldia = det.EsAlcaldia, CuentaContableID = det.CuentaContableID });
                            //});
                            //gvDataImp.RefreshData();
                            this.lkCuentaBancaria.Properties.ReadOnly = true;
                            _SubMonto = EntidadAnterior.SubTotal;
                            _Iva = EntidadAnterior.IVA;
                            _TotalSubI = EntidadAnterior.TotalSubIva;


                        }
                        else
                        {
                            empl = db.Empleados.Single(s => s.ID.Equals(IDBeneficiario));
                            prov = null;
                            this.lkCuentaBancaria.Properties.ReadOnly = true;
                            _SubMonto = EntidadAnterior.SubTotal;
                        }
                    }

                    this.chkAnticipo.Enabled = false;
                    this.rgOption.Enabled = false;
                    this.glkBeneficiario.Enabled = false;
                    this.btnLoad.Enabled = false;
                    this.dateHasta.Enabled = this.dateFechaCheque.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(lkCuentaBancaria, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(dateFechaCheque, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
        }

        private bool ValidarReferencia(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? i.MovimientoTipoID.Equals(39) && i.CuentaBancariaID.Equals(IDCuentaBancaria) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.ID != Convert.ToInt32(ID) :
                          i.MovimientoTipoID.Equals(39) && i.CuentaBancariaID.Equals(IDCuentaBancaria) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio))
                          select i);
            
            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        private int GetNroCheque(Entidad.SAGASDataClassesDataContext dbx)
        {
            if (IDCuentaBancaria > 0)
            {
                try
                {
                    var chqUso = dbx.Remesas.Where(r => r.CuentaBancariaID.Equals(IDCuentaBancaria) && r.EnUso).ToList();

                    if (chqUso.Count > 0)
                    {
                        int number = 0;

                        if (chqUso.First().Numero.Equals(0))
                            number = chqUso.First().NumeroInicial;
                        else
                            number = chqUso.First().Numero + 1;

                        if (number > chqUso.First().NumeroFinal)
                        {
                            spNumero.Properties.MinValue = chqUso.First().NumeroInicial;
                            spNumero.Properties.MaxValue = chqUso.First().NumeroFinal;
                            return 0;
                        }
                        else
                        {
                            spNumero.Properties.MinValue = chqUso.First().NumeroInicial;
                            spNumero.Properties.MaxValue = chqUso.First().NumeroFinal;
                            return number;
                        }
                        
                    }

                    var chqUsar = dbx.Remesas.Where(r => r.CuentaBancariaID.Equals(IDCuentaBancaria) && !r.EnUso && !r.Usada).OrderBy(o => o.Consecutivo).ToList();

                    //no hay remesas de cheque para esta cuenta bancaria

                    if (chqUsar.Count > 0)
                    {
                        int number = 0;

                        if (chqUsar.First().Numero.Equals(0))
                            number = chqUsar.First().NumeroInicial;
                        else
                            number = chqUsar.First().Numero + 1;

                        if (number >= chqUsar.First().NumeroFinal)
                        {
                            spNumero.Properties.MinValue = chqUsar.First().NumeroInicial;
                            spNumero.Properties.MaxValue = chqUsar.First().NumeroFinal;
                            return 0;
                        }
                        else
                        {
                            spNumero.Properties.MinValue = chqUsar.First().NumeroInicial;
                            spNumero.Properties.MaxValue = chqUsar.First().NumeroFinal;
                            return number;
                        }
                    }

                    return 0;
                }
                catch { return 0; }
            }
            else
                return 0;
            
        }

        private void SetMontoCheque()//Establece el valor a pagar del cheque
        {
            try
            {
                decimal vMonto = 0;
                decimal vImp = 0;
                decimal tot = 0;
                bool EsDif = false;

                if (!EsEmpleado && !_EsAnticipo)
                {
                    //vMonto = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);

                    if (_MonedaID.Equals(MonedaPrimaria))//si el cheque es generado en la moneda primaria (Cordobas)
                    {
                        DetalleM.ForEach(det =>
                            {
                                if (det.MonedaID.Equals(MonedaPrimaria))
                                    vMonto += Decimal.Round(det.Litros, 2, MidpointRounding.AwayFromZero);
                                else
                                {
                                    if (!det.Litros.Equals(0m))
                                    {
                                        vMonto += Decimal.Round(det.MontoMonedaSecundaria * _TipoCambio, 2, MidpointRounding.AwayFromZero);
                                        EsDif = true;
                                    }
                                }
                            });
                    }
                    else
                    {
                        DetalleM.ForEach(det =>
                        {
                            if (det.MonedaID.Equals(MonedaPrimaria))
                                vMonto += Decimal.Round(det.Litros, 2, MidpointRounding.AwayFromZero);
                            else
                            {
                                if (!det.Litros.Equals(0m))
                                {
                                    vMonto += Decimal.Round(det.MontoMonedaSecundaria * _TipoCambio, 2, MidpointRounding.AwayFromZero);
                                    EsDif = true;
                                }
                            }
                        });

                        //vMonto = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                    }

                    vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);

                }
                else
                {
                    if (_EsAnticipo)
                    {
                        vMonto = _TotalSubI;
                        vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                    }
                    if (EsEmpleado)
                    {
                        vMonto = _SubMonto;
                    }
                }

                tot = vMonto - vImp;
                if (_TipoCambio > 0)
                {
                    if (_MonedaID.Equals(MonedaPrimaria))
                    {
                            _Total = tot;

                            if (EsDif)
                            {
                                _DiferencialCambiario = vMonto - Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero) - Decimal.Round(DetalleM.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                                _DiferencialCambiario = 0;
                    }
                    else if (_MonedaID.Equals(MonedaSecundaria))
                    {
                        if (_EsAnticipo)
                        {
                            _Total = Decimal.Round(tot, 2, MidpointRounding.AwayFromZero);
                            //_DiferencialCambiario = vMonto - Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero) - Decimal.Round(DetalleM.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                         
                        }
                        else
                        {
                            _Total = Decimal.Round(tot / _TipoCambio, 2, MidpointRounding.AwayFromZero);
                            _DiferencialCambiario = vMonto - Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero) - Decimal.Round(DetalleM.Sum(s => s.Abono), 2, MidpointRounding.AwayFromZero);
                            //_DiferencialCambiario = Math.Abs(Decimal.Round(DetalleM.Sum(s => s.MontoMonedaSecundaria), 2, MidpointRounding.AwayFromZero) - _Total);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        public bool ValidarCampos(bool detalle, bool Load)
        {
            if (dateFechaCheque.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la fecha del Cheque.", Parametros.MsgType.warning);
                return false;
            }

            if (!Load)
            {
                if (Convert.ToDateTime(dateFechaCheque.EditValue).Date > Convert.ToDateTime(db.GetDateServer()).Date)
                {
                    Parametros.General.DialogMsg("La Fecha del Cheque no puede ser mayor a la fecha del día actual.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToInt32(glkBeneficiario.EditValue) <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar un Beneficiario para continuar con el proceso.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (EsEmpleado || _EsAnticipo)
            {
                if (IDConepto < 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar un concepto contable para continuar con el proceso.", Parametros.MsgType.warning);
                    return false;
                }

            }
            else
            {
                if (dateHasta.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la fecha de Vencimiento para cargar la lista.", Parametros.MsgType.warning);
                    return false;
                }
            }
            
            if (detalle)
            {

                if (lkCuentaBancaria.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la Cuenta Bancaria.", Parametros.MsgType.warning);
                    return false;
                }

                if (Numero <= 0)
                {
                    Parametros.General.DialogMsg("No existe Número para este cheque.", Parametros.MsgType.warning);
                    return false;
                }

                if (!Parametros.General.ValidatePeriodoContable(FechaCheque, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
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

                if (String.IsNullOrEmpty(Concepto))
                {
                    Parametros.General.DialogMsg("Debe Ingresar un Concepto del Cheque.", Parametros.MsgType.warning);
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
                //if (!ValidarReferencia(Convert.ToString(txtNumero.Text)))
                //{
                //    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNumero.Text), Parametros.MsgType.warning);
                //    return false;
                //}
                                
            }

            return true;
        }

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

        private void ListaImpuesto(int ID, bool valor)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                var Mov = db.Movimientos.FirstOrDefault(s => s.ID.Equals(ID));
                var vPagado = db.Movimientos.Where(m => m.MovimientoReferenciaID.Equals(ID) && m.MovimientoTipoID.Equals(12) && !m.Anulado);

                #region <<< PAGOS DOCUMENTOS >>>
                
                if (Mov != null)
                {

                    decimal vSub = 0, vMonto = 0, vResta = 0;

                    vSub = (Mov.MonedaID.Equals(MonedaPrimaria) ? Mov.SubTotal : Decimal.Round((Mov.SubTotal / Mov.TipoCambio) * _TipoCambio, 2, MidpointRounding.AwayFromZero)) - (vPagado.Count() > 0 ? vPagado.Sum(s => s.SubTotal) : 0);

                    if (vSub > 0)
                    {
                        var prov = db.Proveedors.FirstOrDefault(p => p.ID.Equals(Mov.ProveedorID));

                        if (prov != null)
                        {
                            if (prov.AplicaRetencion)
                            {
                                vMonto = 0;
                                if (vSub >= db.CuentaContables.Single(s => s.ID.Equals(prov.ImpuestoRetencionID)).Limite)
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

                                            if (DetalleImp.Count(d => !d.EsAlcaldia && d.SubTotal <= 0) > 0)
                                                DetalleImp.Remove(DetalleImp.First(d => !d.EsAlcaldia && d.SubTotal <= 0));

                                        }
                                    }
                                }
                            }

                            if (_AplicaAlcaldia)
                            {
                                vMonto = 0;
                                if (vSub >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || prov.TipoProveedorID.Equals(ProveedorNoRegistrado))
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

                                            if (DetalleImp.Count(d => d.EsAlcaldia && d.SubTotal <= 0) > 0)
                                                DetalleImp.Remove(DetalleImp.First(d => d.EsAlcaldia && d.SubTotal <= 0));

                                        }
                                    }
                                }

                            }

                        }
                    }

                }

                #endregion

                if (!EsEmpleado && _EsAnticipo)
                {
                    #region <<< PAGOS ANTICIPOS >>>                    
                    decimal vSub = 0, vMonto = 0, vResta = 0;

                    vSub = (_MonedaID.Equals(MonedaPrimaria) ? _SubMonto : Decimal.Round(_SubMonto * _TipoCambio, 2, MidpointRounding.AwayFromZero));
                    vSub = _SubMonto;

                    if (vSub >= 0)
                    {
                        int xa = 0, xr = 0;
                        
                        xa = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, true));
                        xr = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false));

                        if (xa > 0 && xr > 0)
                            colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                        int xANumero = xa;
                        string xATexto = "";

                        int xIRNumero = xr;
                        string xIRTexto = "";

                        if (DetalleImp.Count(d => !d.EsAlcaldia) > 0)
                        {
                            xIRNumero = DetalleImp.First(d => !d.EsAlcaldia).Numero;
                            xIRTexto = DetalleImp.First(d => !d.EsAlcaldia).Concepto;
                        }

                        if (DetalleImp.Count(d => d.EsAlcaldia) > 0)
                        {
                            xANumero = DetalleImp.First(d => d.EsAlcaldia).Numero;
                            xATexto = DetalleImp.First(d => d.EsAlcaldia).Concepto;
                        }

                        DetalleImp.Clear();

                        if (prov != null)
                        {
                            if (prov.AplicaRetencion)
                            {
                                
                                vMonto = 0;
                                if ((_MonedaID.Equals(MonedaPrimaria) ? _SubMonto : Decimal.Round(_SubMonto * _TipoCambio, 2, MidpointRounding.AwayFromZero)) >= db.CuentaContables.Single(s => s.ID.Equals(prov.ImpuestoRetencionID)).Limite)
                                {
                                    decimal IR = (db.CuentaContables.Single(cc => cc.ID.Equals(prov.ImpuestoRetencionID)).Porcentaje / 100m);
                                    vMonto = Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);
                                    vResta += Decimal.Round((vSub * IR), 2, MidpointRounding.AwayFromZero);
                                                                        
                                    DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = xIRNumero, Concepto = xIRTexto, SubTotal = vSub, EsAlcaldia = false, CuentaContableID = prov.ImpuestoRetencionID });
                                    
                                }
                            }

                            if (_AplicaAlcaldia)
                            {
                                vMonto = 0;
                                if ((_MonedaID.Equals(MonedaPrimaria) ? _SubMonto : Decimal.Round(_SubMonto * _TipoCambio, 2, MidpointRounding.AwayFromZero)) >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || prov.TipoProveedorID.Equals(ProveedorNoRegistrado))
                                {
                                    decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(231)).Porcentaje / 100m);
                                    vMonto = Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);
                                    vResta += Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);

                                    DetalleImp.Add(new Entidad.Retencione { MovimientoID = ID, Monto = vMonto, Numero = xANumero, Concepto = xATexto, SubTotal = vSub, EsAlcaldia = true, CuentaContableID = 231 });

                                }

                            }
                        }

                        _Total = _TotalSubI - vResta;
                    }

                    #endregion
                }

                if (EsEmpleado)
                    _Total = _SubMonto;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                gvDataImp.RefreshData();

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private bool Guardar()
        {
            if (!ValidarCampos(true, false))
                return false;

            if (!EsEmpleado && !_EsAnticipo)
            {
                if (DetalleM.Count(d => d.Pagado) <= 0)
                {
                    if (Parametros.General.DialogMsg("No existe detalle a pagar, desea continuar." + Environment.NewLine, Parametros.MsgType.question) != DialogResult.OK)
                        return false;
                }
                else
                {
                    if (_Total <= 0)
                    {
                        Parametros.General.DialogMsg("El valor total del Cheque es 0 (cero), el valor debe ser mayor a 0 (cero)." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (DetalleM.Count > 0)
                {
                    if (DetalleM.Count(c => c.Litros > 0 && c.FechaRegistro.Date > FechaCheque) > 0)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                }
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

                    if (Editable)
                    {
                        M = db.Movimientos.Single(m => m.ID.Equals(EntidadAnterior.ID));                   
                    }
                    else
                    {
                        M = new Entidad.Movimiento();

                        if (EsEmpleado)
                            M.EmpleadoID = IDBeneficiario;
                        else
                            M.ProveedorID = IDBeneficiario;

                        M.MovimientoTipoID = 39;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());

                        M.Numero = GetNroCheque(db);
                        M.CuentaBancariaID = IDCuentaBancaria;

                        if (M.Numero.Equals(0))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("El Número del Cheque es invalido.", Parametros.MsgType.warning);
                            Numero = 0;
                            return false;
                        }

                        if (!ValidarReferencia(Convert.ToInt32(M.Numero), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            trans.Rollback();
                            Parametros.General.DialogMsg("El número del Cheque '" + M.Numero + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
                            Numero = M.Numero;
                            return false;
                        }

                        var chqUso = db.Remesas.Where(r => r.CuentaBancariaID.Equals(IDCuentaBancaria) && r.EnUso).ToList();

                        if (chqUso.Count > 0)
                        {
                            Entidad.Remesa R = db.Remesas.Single(r => r.ID.Equals(chqUso.First().ID));
                            R.Numero = M.Numero;
                            M.CuentaBancariaID = R.CuentaBancariaID;

                            if (R.Numero.Equals(R.NumeroFinal))
                            {
                                R.Usada = true;
                                R.EnUso = false;
                            }

                            db.SubmitChanges();

                        }
                        else
                        {
                            var chqUsar = db.Remesas.Where(r => r.CuentaBancariaID.Equals(IDCuentaBancaria) && !r.EnUso && !r.Usada).OrderBy(o => o.Consecutivo).ToList();

                            if (chqUsar.Count > 0)
                            {
                                Entidad.Remesa R = db.Remesas.Single(r => r.ID.Equals(chqUsar.First().ID));
                                R.Numero = M.Numero;
                                M.CuentaBancariaID = R.CuentaBancariaID;
                                R.EnUso = true;

                                db.SubmitChanges();
                            }

                        }
                        //Entidad.Remesa.
                    }

                    //M.Numero = Numero;//GetNroCheque(db);
                    //M.CuentaBancariaID = IDCuentaBancaria;

                    //if (M.Numero.Equals(0))
                    //{
                    //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    //    trans.Rollback();
                    //    Parametros.General.DialogMsg("El Número del Cheque es invalido.", Parametros.MsgType.warning);
                    //    Numero = 0;
                    //    return false;
                    //}

                    //if (!ValidarReferencia(Convert.ToInt32(M.Numero), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
                    //{
                    //    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    //    trans.Rollback();
                    //    Parametros.General.DialogMsg("El número del Cheque '" + M.Numero + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
                    //    Numero = M.Numero;
                    //    return false;
                    //} 

                    M.UsuarioID = UsuarioID;
                    M.FechaRegistro = FechaCheque;
                    M.Monto = (_MonedaID.Equals(MonedaPrimaria) ? _Total : Decimal.Round(_Total * _TipoCambio, 2, MidpointRounding.AwayFromZero));
                    M.MonedaID = _MonedaID;
                    M.TipoCambio = _TipoCambio;
                    M.MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Total);
                    M.SubTotal = _SubMonto;
                    M.ConceptoContableID = IDConepto;
                    M.EsAnticipo = _EsAnticipo;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = @Concepto;
                    M.DiferencialCambiario = _DiferencialCambiario;
                    M.IVA = _Iva;
                    M.TotalSubIva = _TotalSubI;

                    if (!Editable)
                        db.Movimientos.InsertOnSubmit(M);
                    else
                        db.SubmitChanges();

                    if (IDConepto == 81)
                    {
                        Entidad.Empleado E = db.Empleados.Single(m => m.ID.Equals(IDBeneficiario));
                        if (!E.Activo)
                            E.Liquidado = true;
                    }
                    
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
                    
                    M.ComprobanteContables.Clear();

                    Compronbante.ForEach(linea =>
                    {
                        M.ComprobanteContables.Add(linea);
                        db.SubmitChanges();
                    });

                    db.SubmitChanges();

                    #endregion

                    #region ::: REGISTRANDO DEUDOR :::

                    Entidad.Deudor D;

                    if (Editable)
                    {
                        D = db.Deudors.Single(d => d.MovimientoID.Equals(EntidadAnterior.ID));
                        D.Pagos.Clear();
                    }
                    else
                    {
                        D = new Entidad.Deudor();
                        if (EsEmpleado)
                            D.EmpleadoID = IDBeneficiario;
                        else
                            D.ProveedorID = IDBeneficiario;

                        D.MovimientoID = M.ID;
                    }

                    decimal Saldo = 0;

                    if (!EsEmpleado && !_EsAnticipo)
                        Saldo = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                    else
                        Saldo = _Total;
                    
                        D.Valor = (!M.ConceptoContableID.Equals(0) ? (!EtConcepto.Single(o => o.ID.Equals(M.ConceptoContableID)).GeneraDocumento ? 0 : (EsEmpleado ? Math.Abs(Saldo) : -Math.Abs(Saldo))) : (EsEmpleado ? Math.Abs(Saldo) : -Math.Abs(Saldo)));

                    if (!Editable)
                        db.Deudors.InsertOnSubmit(D);

                    db.SubmitChanges();

                    #endregion

                    #region <<< REGISTRAR PAGO A MOVIMIENTO  >>>
                    string texto = "";
                    
                    if (DetalleM.Count > 0)
                    {
                        //DetalleM.Where(d => d.Litros > 0).ToList().ForEach(linea =>
                            foreach (Entidad.Movimiento linea in DetalleM)
                            {
                                Entidad.Movimiento MLine = db.Movimientos.Single(mv => mv.ID.Equals(linea.ID));

                                if (MLine.Anulado)
                                {
                                    texto = (MLine.Numero.Equals(0) ? MLine.Referencia : MLine.Numero.ToString());
                                    break;
                                }

                                /////PROBLEMA de ABONO y GUARDADO
                                         
                                if (linea.Litros > 0)
                                {
                                    bool IvaPaid = false;
                                    if (linea.IVA > 0 && linea.Abono.Equals(0) && !linea.IVAPagado)
                                    {
                                        MLine.IVAPagado = true;
                                        IvaPaid = true;
                                    }

                                    MLine.Pagado = linea.Pagado;
                                    MLine.Abono = linea.Abono + linea.Litros;
                                    MLine.MovimientoReferenciaID = M.ID;
                                    D.Pagos.Add(new Entidad.Pago { MovimientoPagoID = MLine.ID, Monto = linea.Litros, PagoIVA = IvaPaid });
                                    //db.SubmitChanges();
                                }
                                else
                                {
                                    MLine.Abono = linea.Abono + linea.Litros;
                                    MLine.Pagado = linea.Pagado;
                                    MLine.MovimientoReferenciaID = 0;
                                    if (linea.IVA > 0 && linea.Abono.Equals(0) && !linea.IVAPagado)
                                        MLine.IVAPagado = false;                                   
                                }


                                //if (!linea.IVAPagado)
                                //{
                                //    if (linea.Pagado)
                                //    {

                                //        MLine.IVAPagado = true;
                                //    }
                                //    else
                                //    {
                                //        MLine.IVAPagado = false;
                                //    }
                                //}

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

                    M.Retenciones.Clear();

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

                    if (_EsAnticipo)
                    {
                        if (!EsEmpleado)
                        {
                            M.DiferenciaAnticipo = (M.Retenciones.Count > 0 ? (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(M.Retenciones.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero) : Decimal.Round(M.Retenciones.Sum(s => s.Monto) * _TipoCambio, 2, MidpointRounding.AwayFromZero)) : 0m);
                        }
                    }

                    ////para que actualice los datos del registro

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                         "Se modificó el Cheque: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se registró el Cheque: " + M.Numero, this.Name);
                    }

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
                    
                    if (!EsEmpleado && !_EsAnticipo)
                    {
                        #region <<< NO ANTICIPOS >>>
                        DetalleM.Where(d => d.Pagado).ToList().ForEach(item =>
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = prov.CuentaContableID,
                                Monto = Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.Litros),
                                Fecha = FechaCheque,
                                Descripcion = prov.Nombre + " Pago Doc. " + item.Referencia + " con Cheque " + Numero.ToString(),
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        });

                        DetalleM.Where(d => d.Pagado).ToList().ForEach(item =>
                        {
                            //Decimal.Round(DetalleM.Where(m => m.Pagado).Sum(s => s.IVA), 2, MidpointRounding.AwayFromZero);
                            
                            //***** MANEJO DEL IVA  *******//

                            if (!item.MovimientoTipoID.Equals(41) && ((item.MovimientoTipoID.Equals(3) && item.Credito) || item.MovimientoTipoID.Equals(5)) && item.IVA > 0 && !item.IVAPagado)
                            {
                                var vIvaPagado = db.Movimientos.Where(m => m.MovimientoReferenciaID.Equals(item.ID) && m.MovimientoTipoID.Equals(12) && !m.Anulado);

                                decimal vIvaAbonado = (vIvaPagado.Count() > 0 ? vIvaPagado.Sum(s => s.IVA) : 0m);

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaIVAAcreditado,
                                    Monto = Decimal.Round(item.IVA - vIvaAbonado, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.IVA - vIvaAbonado / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.IVA - vIvaAbonado),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Cheque " + Numero.ToString(),
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = _CuentaIVAXAcreditar,
                                    Monto = -Math.Abs(Decimal.Round(item.IVA - vIvaAbonado, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(item.IVA - vIvaAbonado / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(item.IVA - vIvaAbonado)),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Cheque " + Numero.ToString(),
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
                                Fecha = FechaCheque,
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
                                Fecha = FechaCheque,
                                Descripcion = tAlcaldia,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }

                        if (_Total > 0)
                        {
                            if (_MonedaID.Equals(MonedaSecundaria))
                            {
                                if (_DiferencialCambiario > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = _CuentaPerdidaCambiaria,
                                        Monto = Math.Abs(Decimal.Round(_DiferencialCambiario, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TipoCambio,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(_DiferencialCambiario, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = FechaCheque,
                                        Descripcion = prov.Nombre + " Diferencial Cambiario – Cheque Nro. " + Numero.ToString(),
                                        Linea = i,
                                        CentroCostoID = (_OficinaCentral.Equals(IDEstacionServicio) ? _CECOOficinaDiferencialID : _CECOEstacionesDiferencialID),
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                }

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                                    Monto = -Math.Abs(Decimal.Round(_Total * _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Pago Total del Cheque " + Numero.ToString(),
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });

                                //decimal vDif = CD.Sum(s => s.Monto);

                                //if (!vDif.Equals(0))
                                //{
                                //    i++;
                                //    CD.Add(new Entidad.ComprobanteContable
                                //    {
                                //        CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                                //        Monto = Decimal.Round(vDif, 2, MidpointRounding.AwayFromZero),
                                //        TipoCambio = _TipoCambio,
                                //        MontoMonedaSecundaria = Decimal.Round(vDif / _TipoCambio, 2, MidpointRounding.AwayFromZero),
                                //        Fecha = FechaCheque,
                                //        Descripcion = prov.Nombre + " Diferencial Cambiario del Cheque " + Numero.ToString(),
                                //        Linea = i,
                                //        EstacionServicioID = IDEstacionServicio,
                                //        SubEstacionID = IDSubEstacion
                                //    });
                                //}
                            }
                            else
                            {
                                if (_DiferencialCambiario > 0)
                                {
                                    i++;
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                         CuentaContableID = _CuentaPerdidaCambiaria,
                                            Monto = Math.Abs(Decimal.Round(_DiferencialCambiario, 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Math.Abs(Decimal.Round(_DiferencialCambiario / _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                            Fecha = FechaCheque,
                                            Descripcion = prov.Nombre + " Diferencial Cambiario – Cheque Nro. " + Numero.ToString(),
                                            Linea = i,
                                         CentroCostoID = (_OficinaCentral.Equals(IDEstacionServicio) ? _CECOOficinaDiferencialID : _CECOEstacionesDiferencialID),
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                    });
                                }

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                                    Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(_Total)),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Pago Total del Cheque " + Numero.ToString(),
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }
                        }
                    #endregion
                    }
                    else if (!EsEmpleado && _EsAnticipo)
                    {
                        #region <<< ANTICIPOS   >>>
                        
                        if (_Total > 0)
                        {

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = db.ConceptoContables.Single(cc => cc.ID.Equals(IDConepto)).CuentaContableID,
                                Monto = (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_SubMonto, 2, MidpointRounding.AwayFromZero) : Decimal.Round(_SubMonto * _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_SubMonto / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _SubMonto),
                                Fecha = FechaCheque,
                                Descripcion = Concepto,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });

                            if (_Iva > 0m)
                            {
                                //i++;
                                //CD.Add(new Entidad.ComprobanteContable
                                //{
                                //    CuentaContableID = _CuentaIVAAcreditado,
                                //    Monto = (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Iva, 2, MidpointRounding.AwayFromZero) : Decimal.Round(_Iva * _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                //    TipoCambio = _TipoCambio,
                                //    MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Iva / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Iva),
                                //    Fecha = FechaCheque,
                                //    Descripcion = prov.Nombre + " Acreditación IVA al Cheque " + Numero.ToString(),
                                //    Linea = i,
                                //    CentroCostoID = 0,
                                //    EstacionServicioID = IDEstacionServicio,
                                //    SubEstacionID = IDSubEstacion
                                //});

                                var comprobante = CD.Where(c => c.CuentaContableID.Equals(db.ConceptoContables.Single(cc => cc.ID.Equals(IDConepto)).CuentaContableID)).First();
                                comprobante.Monto += (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Iva, 2, MidpointRounding.AwayFromZero) : Decimal.Round(_Iva * _TipoCambio, 2, MidpointRounding.AwayFromZero));
                                comprobante.MontoMonedaSecundaria += (_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Iva / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Iva);
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
                                    CuentaContableID = prov.ImpuestoRetencionID,
                                    Monto = -Math.Abs(_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero) : Decimal.Round(vIR * _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(vIR)),
                                    Fecha = FechaCheque,
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
                                    CuentaContableID = 231,
                                    Monto = -Math.Abs(_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(vAlcaldia, 2, MidpointRounding.AwayFromZero) : Decimal.Round(vAlcaldia * _TipoCambio, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(vAlcaldia / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(vAlcaldia)),
                                    Fecha = FechaCheque,
                                    Descripcion = tAlcaldia,
                                    Linea = i,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                                Monto = -Math.Abs((_MonedaID.Equals(MonedaPrimaria) ? Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero) : Decimal.Round(_Total * _TipoCambio, 2, MidpointRounding.AwayFromZero))),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (_MonedaID.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(_Total)),
                                Fecha = FechaCheque,
                                Descripcion = prov.Nombre + " Pago Total del Cheque " + Numero.ToString(),
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }
                        #endregion
                    }
                    else if (EsEmpleado)
                    {
                        #region <<< Empleados   >>>

                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = db.ConceptoContables.Single(cc => cc.ID.Equals(IDConepto)).CuentaContableID,
                                Monto = Decimal.Round(_SubMonto, 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(_SubMonto / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _SubMonto),
                                Fecha = FechaCheque,
                                Descripcion = Concepto,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });

                        if (_Total > 0)
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                                Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? -Math.Abs(Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero)) : -Math.Abs(_Total)),
                                Fecha = FechaCheque,
                                Descripcion = empl.Nombres + " " + empl.Apellidos + " Pago Total del Cheque " + Numero.ToString(),
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }
                        #endregion
                    }

                    if (!CD.Sum(s => s.Monto).Equals(0))
                    {
                        decimal d = CD.Sum(s => s.Monto);

                        if (Math.Abs(d) <= 0.10m)
                        {
                            var obj = CD.Where(o => o.Monto < 0).OrderBy(x => x.Monto).FirstOrDefault();
                            if (obj != null)
                            {
                                obj.Monto -= d;

                            }
                        }
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
            if (!_Guardado)
            {
                if (DetalleM.Count > 0 || lkCuentaBancaria.EditValue != null)
                {
                    if (Parametros.General.DialogMsg("El Cheque actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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
            try
            {
                if (Convert.ToInt32(glkBeneficiario.EditValue) > 0)
                {
                    rgOption.Enabled = false;
                    if (!EsEmpleado)
                    {

                        empl = null;
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

                        spLimite.Value = prov.LimiteCredito;

                        decimal Saldo = (db.Deudors.Count(d => d.ProveedorID.Equals(IDBeneficiario)) > 0 ? db.Deudors.Where(d => d.ProveedorID.Equals(IDBeneficiario)).Sum(s => s.Valor) : 0m);
                        spSaldo.Value = Saldo;

                        decimal _Disponible = prov.LimiteCredito - Saldo;
                        spDisponible.Value = _Disponible;
                    }
                    else if (EsEmpleado)
                    {
                        prov = null;
                        empl = db.Empleados.Single(p => p.ID.Equals(IDBeneficiario));
                        spSubMonto.Enabled = true;
                        lkConcepto.EditValue = null;

                        var obj = (from d in db.Deudors
                                   join m in db.Movimientos.Where(m => m.EstacionServicioID.Equals(IDEstacionServicio)) on d.MovimientoID equals m.ID
                                   join em in db.Empleados.Where(c => c.ID.Equals(empl.ID)) on d.EmpleadoID equals em.ID
                                   group d by new { d.EmpleadoID, em.Nombres, em.Apellidos } into gr
                                   select new
                                   {
                                       ID = gr.Key.EmpleadoID,
                                       Empleado = gr.Key.Nombres + " " + gr.Key.Apellidos,
                                       suma = gr.Sum(s => s.Valor)
                                   }).ToList();

                        spLimite.Value = 0;// + emp.LimiteCredito.ToString("#,0.00"));

                        decimal Saldo = (db.Deudors.Count(d => d.EmpleadoID.Equals(IDBeneficiario)) > 0 ? db.Deudors.Where(d => d.EmpleadoID.Equals(IDBeneficiario)).Sum(s => s.Valor) : 0m);
                        spSaldo.Value = Saldo;

                        decimal _Disponible = 0 - Saldo;
                        spDisponible.Value = _Disponible;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        //Cambiar las opciones del ROC
        private void rgOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rgOption.SelectedIndex.Equals(0))
                {
                    this.gridDetalle.Visible = true;
                    this.gridDetalle.Dock = DockStyle.Fill;
                    btnLoad.Text = "Cargar Lista";
                    btnLoad.Enabled = true;
                    layoutControlItemTipo.Text = "Proveedor";
                    glkBeneficiario.EditValue = null;
                    glkBeneficiario.Properties.NullText = "<Seleccione al Proveedor>";
                    EsEmpleado = false;
                    spLimite.Value = 0;
                    spSaldo.Value = 0;
                    spDisponible.Value = 0;
                    _SubMonto = 0;
                    chkAnticipo.Checked = false;
                    lkConcepto.EditValue = null;

                    layoutControlItemConc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemSubMonto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemVenc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemLim.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemDisp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemSaldo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemCargar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    

                    if (_AplicaAnticipo)
                        layoutControlItemAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    glkBeneficiario.Properties.DataSource = null;
                    glkBeneficiario.Properties.DataSource = (from p in db.Proveedors
                                                       //join ces in db.es on c.ID equals ces.ClienteID
                                                       //where ces.EstacionServicioID.Equals(IDEstacionServicio) 
                                                             where p.Activo
                                                       group p by new { p.ID, p.Codigo, p.Nombre } into gr
                                                       select new
                                                       {
                                                           ID = gr.Key.ID,
                                                           Codigo = gr.Key.Codigo,
                                                           Nombre = gr.Key.Nombre,
                                                           Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                       }).ToList();
                }
                else if (rgOption.SelectedIndex.Equals(1))
                {
                    this.gridDetalle.Visible = false;
                    this.gridDetalle.Dock = DockStyle.None;                    
                    btnLoad.Enabled = false;
                    layoutControlItemTipo.Text = "Empleado";
                    glkBeneficiario.EditValue = null;
                    glkBeneficiario.Properties.NullText = "<Seleccione al Empleado>";
                    EsEmpleado = true;
                    spLimite.Value = 0;
                    spSaldo.Value = 0;
                    spDisponible.Value = 0;
                    chkAnticipo.Checked = false;
                    lkConcepto.EditValue = null;

                    layoutControlItemAnticipo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemVenc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemLim.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemDisp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemSaldo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemCargar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemConc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemSubMonto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    glkBeneficiario.Properties.DataSource = null;
                    glkBeneficiario.Properties.DataSource = (from em in db.Empleados
                                                       join p in db.Planillas on em.PlanillaID equals p.ID
                                                       where (em.Activo || !em.Liquidado) //&& p.EstacionServicioID.Equals(IDEstacionServicio)
                                                       select new { em.ID, em.Codigo, Nombre = em.Nombres + " " + em.Apellidos, Display = em.Codigo + " | " + em.Nombres + " " + em.Apellidos }).ToList();
                }                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void dateFechaRecibo_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaCheque.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaCheque.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaCheque.Date)).First().Valor : 0m);

            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        //Cargar la lista de los pagos del Deudor
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(false, true))
                {                    
                    this.bdsDetalle.DataSource = null;

                    if (!EsEmpleado)
                    {
                        prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));
                        empl = null;
                        chkAnticipo.Enabled = false;
                        rlkMoneda.DataSource = (from m in db.Monedas
                                                select new
                                                {
                                                    ID = m.ID,
                                                    Display = m.Nombre
                                                }).ToList();
                        //if (client.RealizaPagoGrupoES && client.EstacionPagoID.Equals(IDEstacionServicio))
                        //{
                        //    _PagoGrupo = true;
                        //    DetalleM = (from m in db.Movimientos
                        //                where (m.MovimientoTipoID.Equals(7) || m.MovimientoTipoID.Equals(27))
                        //                && !m.Anulado && !m.Pagado && m.ClienteID.Equals(IDDeudor)
                        //                select m).OrderBy(o => o.FechaRegistro).ToList();
                        //}
                        //else
                        //{

                        if (!Editable)
                        {
                            DetalleM = (from m in db.Movimientos
                                        where (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(5) || m.MovimientoTipoID.Equals(41) || m.MovimientoTipoID.Equals(84) || m.MovimientoTipoID.Equals(66)) && m.EstacionServicioID.Equals(IDEstacionServicio)
                                        && !m.Anulado && !m.Pagado && m.ProveedorID.Equals(prov.ID)
                                        select m).OrderBy(o => o.FechaRegistro).ToList();


                            if (DetalleM.Count <= 0)
                            {
                                Parametros.General.DialogMsg("PROVEEDOR / BENEFICIARIO NO TIENE FACTURAS PENDIENTES DE PAGO", Parametros.MsgType.warning);
                                return;
                            }

                            DetalleM.Where(m => (Convert.ToDateTime(m.FechaVencimiento).Date <= Convert.ToDateTime(dateHasta.EditValue).Date)).OrderBy(o => o.FechaRegistro).ToList().ForEach(item =>
                            {
                                decimal saldo = item.Monto - item.Abono;
                                item.Litros += saldo;
                                item.Pagado = true;

                                ListaImpuesto(item.ID, true);

                            });
                        }
                        else
                        {
                            DetalleM = (from m in db.Movimientos
                                        where (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(5) || m.MovimientoTipoID.Equals(84) || m.MovimientoTipoID.Equals(41) || m.MovimientoTipoID.Equals(66)) && m.EstacionServicioID.Equals(IDEstacionServicio)
                                        && !m.Anulado && (!m.Pagado || m.MovimientoReferenciaID.Equals(EntidadAnterior.ID)) && m.ProveedorID.Equals(prov.ID) 
                                        select m).OrderBy(o => o.FechaRegistro).ToList();

                            var P = (from p in db.Pagos
                                     join d in db.Deudors on p.Deudor equals d
                                     where d.MovimientoID.Equals(EntidadAnterior.ID)
                                     select p).ToList();

                            DetalleM.Where(m => P.Select(p => p.MovimientoPagoID).Contains(m.ID)).ToList().ForEach(item =>
                                {
                                    item.Litros = P.Single(v => v.MovimientoPagoID.Equals(item.ID)).Monto;
                                    item.Abono =  item.Abono - item.Litros;

                                    if (P.Single(v => v.MovimientoPagoID.Equals(item.ID)).PagoIVA)
                                        item.IVAPagado = false;
                                });

                            db.Retenciones.Where(r => r.MovimientoID.Equals(EntidadAnterior.ID)).ToList().ForEach(det =>
                                {
                                    DetalleImp.Add(new Entidad.Retencione { Monto = det.Monto, Numero = det.Numero, Concepto = det.Concepto, SubTotal = det.SubTotal, EsAlcaldia = det.EsAlcaldia, CuentaContableID = det.CuentaContableID });                                    
                                });
                            gvDataImp.RefreshData();
                            this.lkCuentaBancaria.Properties.ReadOnly = true;
                        }

                        this.bdsDetalle.DataSource = DetalleM;                        
                        gvDetalle.RefreshData();
                        SetMontoCheque();

                    }
                    else if (EsEmpleado)
                    {
                        empl = db.Empleados.Single(s => s.ID.Equals(IDBeneficiario));
                        prov = null;

                        DetalleM = (from m in db.Movimientos
                                    where m.EstacionServicioID.Equals(IDEstacionServicio)
                                    && !m.Anulado && !m.Pagado && m.EmpleadoID.Equals(IDBeneficiario)
                                    select m).OrderBy(o => o.FechaRegistro).ToList();

                        this.bdsDetalle.DataSource = DetalleM;
                        gvDetalle.RefreshData();

                        if (DetalleM.Count <= 0)
                        {
                            Parametros.General.DialogMsg("EMPLEADO / BENEFICIARIO NO TIENE FACTURAS PENDIENTES DE PAGO", Parametros.MsgType.warning);
                            return;
                        }
                    }

                        this.rgOption.Enabled = false;
                        this.glkBeneficiario.Enabled = false;
                        this.btnLoad.Enabled = false;
                        this.dateHasta.Enabled = false;
                        
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Seleccionar la cuenta bancaria
        private void lkCuentaBancaria_EditValueChanged(object sender, EventArgs e)
        {
            if (lkCuentaBancaria.EditValue != null)
            {
                //if (!Editable)  
                Numero = GetNroCheque(db);
                _MonedaID = db.CuentaBancarias.Where(m => m.ID.Equals(IDCuentaBancaria)).First().MonedaID;
                if (_MonedaID.Equals(MonedaSecundaria))
                {
                    this.colValue.Visible = this.colTipoCambio.Visible = true;
                    this.colTipoCambio.VisibleIndex = 7;
                    spMonto.BackColor = Color.ForestGreen;
                    spMonto.ForeColor = Color.White;
                }
                else if (_MonedaID.Equals(MonedaPrimaria))
                {
                    this.colValue.Visible = this.colTipoCambio.Visible = false;
                    spMonto.BackColor = Color.White;
                    spMonto.ForeColor = Color.Black;
                }

                SetMontoCheque();

            }
        }
      
        #region <<< GRID DOCUMENTOS >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            if (DetalleM.Sum(s => s.Litros) <= 0)
            {
                DetalleImp.Clear();
                gvDataImp.RefreshData();
            }
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
                            int vID = 0;

                            if (gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID") != null)
                                vID = Convert.ToInt32(gvDataImp.GetRowCellValue(e.RowHandle, "CuentaContableID"));

                            //var Porc = listaRetencionesPorc.FirstOrDefault(l => l.ID.Equals(vID));
                            //decimal Porc =Convert.ToDecimal(db.CuentaContables.Where(c => c.ID.Equals(vID)).FirstOrDefault().Porcentaje);
                            //if (Porc>0)
                            //{
                            //    gvDataImp.SetFocusedRowCellValue("SubTotal", Decimal.Round(Convert.ToDecimal(vMonto / (Porc / 100m)), 2, MidpointRounding.AwayFromZero));
                            //}
                        }
                    }

                    gvDataImp.RefreshData();
                    SetMontoCheque();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void gvDetalle_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colPagado)
                {
                    //if (gvDetalle.GetRowCellValue(e.RowHandle, "Pagado") != null)
                    //{
                    if (e.Value != null)
                    {
                        //if (Convert.ToBoolean(gvDetalle.GetRowCellValue(e.RowHandle, "Pagado")).Equals(true))
                        if (e.Value.Equals(true))
                        {
                            decimal vSaldo = 0;

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                            gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);

                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), Convert.ToBoolean(e.Value));
                            
                        }
                        else
                        {
                            gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0m);
                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), Convert.ToBoolean(e.Value));
                        }

                        gvDetalle.RefreshData();
                        SetMontoCheque();
                    }
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
            try
            {
                //if (e.Column == colMonto)
                //{
                //    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                //    {
                //        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                //        {
                //            decimal vSaldo = 0, vAbono = 0;

                //            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                //            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                //                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                //            if (vAbono > vSaldo)
                //            {
                //                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                //                gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                //            }
                //            else
                //            {
                //                if (vAbono.Equals(vSaldo))
                //                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                //                else
                //                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                //            }
                //        }
                //        else
                //        {
                //            gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                //        }
                //    }
                //    else
                //    {
                //        gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0);
                //    }

                //    gvDetalle.RefreshData();
                //    SetMontoCheque();
                //}
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

                //if (e.Column == colMonto)
                //{
                //    if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                //    {
                //        if (!Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros")).Equals(0))
                //        {
                //            decimal vSaldo = 0, vAbono = 0;

                //            vAbono = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));

                //            if (gvDetalle.GetRowCellValue(e.RowHandle, "Saldo") != null)
                //                vSaldo = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Saldo"));

                //            if (vAbono > vSaldo)
                //            {
                //                Parametros.General.DialogMsg("El monto abonado sobrepasa el saldo pendiente", Parametros.MsgType.warning);
                //                gvDetalle.SetRowCellValue(e.RowHandle, "Litros", vSaldo);
                //            }
                //            else
                //            {
                //                if (vAbono.Equals(vSaldo))
                //                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", true);
                //                else
                //                    gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                //            }
                //        }
                //        else
                //        {
                //            gvDetalle.SetRowCellValue(e.RowHandle, "Pagado", false);
                //        }
                //    }
                //    else
                //    {
                //        gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0);
                //    }

                //    gvDetalle.RefreshData();
                //}
           
        }

        #endregion

        //Botón para mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(true, false))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante de Cheque";
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

        private void spSubMonto_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (prov != null)
                {
                    //_FromST = true;
                    _Iva = ((prov.AplicaIVA && IDConepto.Equals(78)) ? Decimal.Round(_SubMonto * Convert.ToDecimal(0.15m), 2, MidpointRounding.AwayFromZero) : 0m);
                    _TotalSubI = _SubMonto + _Iva;
                    ListaImpuesto(0, false);
                    //_FromST = false;
                }
                else
                    ListaImpuesto(0, false);
            }
            catch (Exception ex)
            {
                //_FromST = false;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void chkAnticipo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAnticipo.Checked)
            {
                layoutControlItemVenc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemLim.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemDisp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemSaldo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemCargar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemConc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemSubMonto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutcontrolitemiIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemTOTAL.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                spSubMonto.Enabled = true;
                lkConcepto.EditValue = null;
                this.gridDetalle.Visible = false;
                this.gridDetalle.Dock = DockStyle.None; 
            }
            else if (!chkAnticipo.Checked)
            {
                layoutControlItemVenc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemLim.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemDisp.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemSaldo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlItemCargar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                _SubMonto = 0;
                spSubMonto.Enabled = false;
                lkConcepto.EditValue = null;
                layoutControlItemConc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemSubMonto.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutcontrolitemiIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemTOTAL.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                this.gridDetalle.Visible = true;
                this.gridDetalle.Dock = DockStyle.Fill;
            }
        }

        #endregion

        private void glkBeneficiario_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (spSubMonto.Value > 0)
            {
                Parametros.General.DialogMsg("El Sub Total debe ser 0 (cero) para poder seleccionar otro beneficiario .", Parametros.MsgType.warning);
                e.Cancel = true;
            }
        }

        private void spTotal_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void spIVA_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                //if (!_FromST)
                //{
                    //if (prov.AplicaIVA && IDConepto.Equals(78))
                    //{
                    //    if (!Convert.ToDecimal(e.NewValue).Equals(Convert.ToDecimal(e.OldValue)))
                    //    {
                    //        if (Math.Abs(Convert.ToDecimal(e.NewValue) - Convert.ToDecimal(e.OldValue)) > 1m)
                    //        {
                    //            Parametros.General.DialogMsg("El cambio en el IVA no puede ser mayor a 1.00 " + Environment.NewLine, Parametros.MsgType.warning);
                    //            e.Cancel = true;
                    //        }
                    //        else
                    //        {
                                _TotalSubI = _SubMonto + Convert.ToDecimal(e.NewValue);
                                    ListaImpuesto(0, false);
                    //        }
                    //    }
                    //}
                    //else
                    //    e.Cancel = true;
                //}
            }
            catch { e.Cancel = true; }
        }



    }
}