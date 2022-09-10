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
    public partial class DialogChequeMasivo : Form
    {
        #region *** DECLARACIONES ***
        //CLAVE: la columna Litros es la que se asiganara al abono
        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormCheque MDI;
        internal Forms.FormPagoManual MDIManual;
        internal Entidad.Movimiento EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;        
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        internal List<Entidad.GrupoRetencionAlcaldia> EtGrupo;
        private List<RetAlcaldia> EtAlcadia = new List<RetAlcaldia>();
        private bool _Guardado = false;
        private int IDPrint = 0;
        internal bool Next = false;
        private bool _AplicaAlcaldia = false;
        private bool _EsManual = false;
        internal bool IsCalculating = false;
        internal int IDCuentaPasivoMasiva = 0;
        private int ProveedorNoRegistrado = Parametros.Config.ProveedorNoRegistradoID();
        internal int MonedaPrimaria = Parametros.Config.MonedaPrincipal();
        internal int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int _CuentaIVAXAcreditar = Parametros.Config.IVAPorAcreditar();
        private int _CuentaIVAAcreditado = Parametros.Config.IVAAcreditado();
        internal int Ret;

        private struct RetAlcaldia
        {
            public int Estacion;
            public int Grupo;
        }

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

        private string Referencia
        {
            get { return txtReferencia.Text; }
            set { txtReferencia.Text = value; }
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

        private static Entidad.Proveedor prov;
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

        public DialogChequeMasivo(int UserID, bool IsEdit, bool IsMnual)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = IsEdit;
            _EsManual = IsMnual;
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
                glkBeneficiario.EditValue = null;

                //if (!_EsManual)
                 
                EtGrupo = db.GrupoRetencionAlcaldia.ToList();
               
                lkCuentaBancaria.Properties.DataSource = db.CuentaBancarias.Where(cb => cb.Activo && cb.EstacionServicioID.Equals(IDEstacionServicio)).Select(s => new { s.ID, s.Nombre }).ToList();
                rplkTipo.DataSource = db.MovimientoTipos.Select(s => new { s.ID, s.Abreviatura });
                rpLkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre });
                lkConcepto.Properties.DataSource = db.ConceptoContables.Where(cc => cc.Activo && cc.EsCheque).Select(s => new { s.ID, Display = s.Nombre }).ToList();
                lkCuentaImpuesto.DataSource = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, Display = cc.Nombre }).OrderBy(o => o.Display).ToList();

                this.bdsImpuesto.DataSource = this.DetalleImp;

                //Carga de Proveedores
                this.gridDetalle.Visible = true;
                this.gridDetalle.Dock = DockStyle.Fill;
                btnLoad.Text = "Cargar Lista";
                btnLoad.Enabled = true;
                layoutControlItemTipo.Text = "Proveedor";
                glkBeneficiario.EditValue = null;
                glkBeneficiario.Properties.NullText = "<Seleccione al Proveedor>";
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


                glkBeneficiario.Properties.DataSource = null;
                glkBeneficiario.Properties.DataSource = (from p in db.Proveedors
                                                         where p.Activo && p.PagoMasivo && p.PagoMasivoManual.Equals(_EsManual) && p.EstacionPagoID.Equals(IDEstacionServicio)
                                                         group p by new { p.ID, p.Codigo, p.Nombre } into gr
                                                         select new
                                                         {
                                                             ID = gr.Key.ID,
                                                             Codigo = gr.Key.Codigo,
                                                             Nombre = gr.Key.Nombre,
                                                             Display = gr.Key.Codigo + " | " + gr.Key.Nombre
                                                         }).ToList();
                

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (Editable)
                {
                    Concepto = EntidadAnterior.Comentario;
                    FechaCheque = EntidadAnterior.FechaRegistro;
                    dateHasta.EditValue = FechaCheque;
                    _TipoCambio = EntidadAnterior.TipoCambio;                    
                    IDCuentaBancaria = EntidadAnterior.CuentaBancariaID;
                    lkCuentaBancaria.EditValue = IDCuentaBancaria;
                    Numero = EntidadAnterior.Numero;
                    _Total = EntidadAnterior.Monto;
                    IDBeneficiario = (!EntidadAnterior.ProveedorID.Equals(0) ? EntidadAnterior.ProveedorID : EntidadAnterior.EmpleadoID);
                    _EsAnticipo = EntidadAnterior.EsAnticipo;

                    btnLoad_Click(null, null);

                    this.chkAnticipo.Enabled = false;
                    this.glkBeneficiario.Enabled = false;
                    this.btnLoad.Enabled = false;
                    this.dateHasta.Enabled = false;
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
            Parametros.General.ValidateEmptyStringRule(dateFechaCheque, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoComentario, errRequiredField);
        }

        private bool ValidarReferencia(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? i.MovimientoTipoID.Equals(39) && i.CuentaBancariaID.Equals(IDCuentaBancaria) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.ID != Convert.ToInt32(ID) :
                          i.MovimientoTipoID.Equals(39) && i.Numero.Equals(code) && i.CuentaBancariaID.Equals(IDCuentaBancaria) && i.EstacionServicioID.Equals(IDEstacionServicio))
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

        private void SetMontoCheque()
        {
            try
            {
                decimal vMonto = 0;
                decimal vImp = 0;

                    vMonto = Decimal.Round(DetalleM.Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                    vImp = Decimal.Round(DetalleImp.Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);

                    _Total = vMonto - vImp;                
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
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la fecha del Pago.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToInt32(glkBeneficiario.EditValue) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar un Beneficiario para continuar con el proceso.", Parametros.MsgType.warning);
                return false;
            }

            if (!Load)
            {
                if (Convert.ToDateTime(dateFechaCheque.EditValue).Date > Convert.ToDateTime(db.GetDateServer()).Date)
                {
                    Parametros.General.DialogMsg("La Fecha del Pago no puede ser mayor a la fecha del día actual.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (dateHasta.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la fecha de Vencimiento para cargar la lista.", Parametros.MsgType.warning);
                return false;
            }
            
            if (detalle)
            {

                if (prov.PagoMasivoManual)
                {
                    if (String.IsNullOrEmpty(Referencia))
                    {
                        Parametros.General.DialogMsg("No existe Referencia para este Pago.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                else
                {
                    if (lkCuentaBancaria.EditValue == null)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Seleccione la Cuenta Bancaria.", Parametros.MsgType.warning);
                        return false;
                    }

                    if (Numero <= 0)
                    {
                        Parametros.General.DialogMsg("No existe Número para este Pago.", Parametros.MsgType.warning);
                        return false;
                    }
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

                //if (String.IsNullOrEmpty(Concepto))
                //{
                //    Parametros.General.DialogMsg("Debe Ingresar un Concepto del Cheque.", Parametros.MsgType.warning);
                //    return false;
                //}

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

        private void RetieneAlcaldia(int ProveedorID)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                EtAlcadia.Clear();
                if (prov != null)
                {
                    if (prov.AplicaAlcaldia)
                    {
                        var listaAR = (from p in db.ProveedorAlcaldiaEstacions
                                       join es in db.EstacionServicios on p.EstacionServicioID equals es.ID
                                       where p.ProveedorID.Equals(prov.ID)
                                       select new RetAlcaldia { Estacion = p.EstacionServicioID, Grupo = es.GrupoAlcaldiaID }).ToList();
                        
                        EtAlcadia.AddRange(listaAR);
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

        private void ListaImpuesto(int ID, bool valor)
        {
            /*
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                var Mov = db.Movimientos.FirstOrDefault(s => s.ID.Equals(ID));
                var vPagado = db.Movimientos.Where(m => m.MovimientoReferenciaID.Equals(ID) && m.MovimientoTipoID.Equals(12) && !m.Anulado);

                #region <<< PAGOS DOCUMENTOS >>>
                
                if (Mov != null)
                {

                    decimal vSub = 0, vMonto = 0, vResta = 0;

                    vSub = Mov.SubTotal - (vPagado.Count() > 0 ? vPagado.Sum(s => s.SubTotal) : 0);

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
                                            DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = 0, SubTotal = vSub, EsAlcaldia = false, CuentaContableID = prov.ImpuestoRetencionID });
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
                                            DetalleImp.Add(new Entidad.Retencione { MovimientoID = ID, Monto = vMonto, Numero = 0, SubTotal = vSub, EsAlcaldia = true, CuentaContableID = 231 });
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
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            */
        }

        private void ListaImpuesto(int ID, string ES, bool valor, decimal Abonado)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                var Mov = db.Movimientos.FirstOrDefault(s => s.ID.Equals(ID));

                #region <<< PAGOS DOCUMENTOS >>>

                if (Mov != null)
                {

                    decimal ValorRetener = 0, vSub = 0, vMonto = 0, vResta = 0;

                    vSub = Decimal.Round(Abonado, 2, MidpointRounding.AwayFromZero) - (Mov.IVAPagado ? 0 : Mov.IVA);
                    ValorRetener = (Mov.SubTotal > 0 ? Mov.SubTotal : Mov.Monto);

                    if (ValorRetener > 0)
                    {
                        //var prov = db.Proveedors.FirstOrDefault(p => p.ID.Equals(Mov.ProveedorID));

                        if (prov != null)
                        {
                            if (prov.AplicaRetencion)
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

                                            DetalleImp.Add(new Entidad.Retencione { Monto = vMonto, Numero = Convert.ToInt32(db.GetSeriesRetenciones(IDEstacionServicio, IDSubEstacion, false)), SubTotal = vSub, EsAlcaldia = false, CuentaContableID = prov.ImpuestoRetencionID });
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

                            if (EtAlcadia.Count > 0)
                            {
                                if (EtAlcadia.Count(s => s.Estacion.Equals(Mov.EstacionServicioID)) > 0 )
                                {
                                    var grupo = EtAlcadia.Single(s => s.Estacion.Equals(Mov.EstacionServicioID));
                                    string texto = "";

                                    if (EtGrupo.Count(c => c.ID.Equals(grupo.Grupo)) > 0)
                                    {
                                        texto = EtGrupo.Single(s => s.ID.Equals(grupo.Grupo)).Nombre;
                                    }
                                    else
                                    {
                                        texto = ES;
                                    }

                                    vMonto = 0;
                                    if (ValorRetener >= db.CuentaContables.Single(s => s.ID.Equals(231)).Limite || prov.TipoProveedorID.Equals(ProveedorNoRegistrado))
                                    {
                                        decimal Alcaldia = (db.CuentaContables.Single(cc => cc.ID.Equals(231)).Porcentaje / 100m);
                                        vMonto = Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);
                                        vResta += Decimal.Round((vSub * Alcaldia), 2, MidpointRounding.AwayFromZero);

                                        if (DetalleImp.Count(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo)).Equals(0))
                                        {
                                            if (valor)
                                            {
                                                int xa = 0;

                                                xa = Convert.ToInt32(db.GetSeriesRetenciones((grupo.Grupo.Equals(1) ? 12 : grupo.Estacion), IDSubEstacion, true));

                                                if (xa > 0)
                                                    colNumero.OptionsColumn.AllowEdit = colNumero.OptionsColumn.AllowFocus = false;

                                                DetalleImp.Add(new Entidad.Retencione { MovimientoID = ID, Monto = vMonto, Numero = xa, SubTotal = vSub, EsAlcaldia = true, CuentaContableID = 231, GrupoID = grupo.Grupo, Alcaldia = texto });
                                       

                                               // DetalleImp.Add(new Entidad.Retencione { MovimientoID = ID, Monto = vMonto, Numero = , SubTotal = vSub, EsAlcaldia = true, CuentaContableID = 231, GrupoID = grupo.Grupo, Alcaldia = texto });
                                            }
                                        }
                                        else
                                        {
                                            if (valor)
                                            {
                                                DetalleImp.First(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo)).SubTotal += vSub;
                                                DetalleImp.First(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo)).Monto += vMonto;
                                            }
                                            else
                                            {
                                                DetalleImp.First(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo)).SubTotal -= vSub;
                                                DetalleImp.First(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo)).Monto -= vMonto;

                                                if (DetalleImp.Count(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo) && d.SubTotal <= 0 && d.Monto <= 0) > 0)
                                                    DetalleImp.Remove(DetalleImp.First(d => d.EsAlcaldia && (d.Alcaldia != null ? d.Alcaldia.Equals(texto) : false) && d.GrupoID.Equals(grupo.Grupo) && d.SubTotal.Equals(0) && d.Monto.Equals(0)));

                                            }
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

            if (DetalleM.Count(d => d.Litros > 0) <= 0)
            {
                Parametros.General.DialogMsg("No existe detalle a pagar." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (DetalleM.Count > 0)
            {
                if (DetalleM.Count(c => c.Litros > 0 && c.FechaRegistro.Date > FechaCheque) > 0)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGFECHADOCUMENTOMAYORFECHAPAGO + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
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

                        M.ProveedorID = IDBeneficiario;
                                                
                        M.MovimientoTipoID = (_EsManual ? 73 : 39);
                        M.EsMasivo = true;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        
                        if (_EsManual)
                            M.Referencia = Referencia;
                        else
                        {
                            M.Numero = GetNroCheque(db);

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
                                Parametros.General.DialogMsg("El número del Pago '" + M.Numero + "' ya esta registrado en el sistema, por favor seleccione otro número.", Parametros.MsgType.warning);
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
                    }

                    M.UsuarioID = UsuarioID;
                    M.FechaRegistro = FechaCheque;
                    M.Monto = _Total;// Convert.ToDecimal(DetalleM.Where(o => o.Litros > 0).Sum(s => s.Litros));
                    M.MonedaID = MonedaPrimaria;
                    M.TipoCambio = _TipoCambio;
                    M.MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(_Total) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(_Total));
                    M.SubTotal = _SubMonto;
                    M.ConceptoContableID = IDConepto;
                    M.EsAnticipo = _EsAnticipo;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = @Concepto;
                    
                    if (!Editable)
                        db.Movimientos.InsertOnSubmit(M);
                    else
                        db.SubmitChanges();

                    gvDataImp.RefreshData();
                    
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
                        D.ProveedorID = IDBeneficiario;
                        D.MovimientoID = M.ID;
                    }

                    decimal Saldo = 0;
                    Saldo = Decimal.Round(DetalleM.Where(o => o.Litros > 0).Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                    
                    D.Valor = Math.Abs(Saldo);

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

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                         "Se modificó el pago masivo : " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Tesoreria,
                        "Se registró el pago masivo : " + M.Numero, this.Name);
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
                    int gr = 1;
                    int i = 100;


                    if (_EsManual)
                    {
                        #region <<< MASIVO MANUAL >>>

                        //Alcaldias de Managua
                        var alma = DetalleImp.Where(o => o.GrupoID.Equals(1)).Sum(s => s.Monto);

                        //Montos Otras Alcaldias
                        var otrasAlma = from d in DetalleImp
                                        join es in db.EstacionServicios on d.Alcaldia equals es.Nombre
                                        where d.GrupoID.Equals(0)
                                        group d by new { es.ID, d.Alcaldia } into g
                                        select new { MontoTotal = g.Sum(s => s.Monto), ID = g.Key.ID, Nombre = g.Key.Alcaldia };

                        var grupo = DetalleM.Where(d => d.Litros > 0).GroupBy(dm => dm.EstacionServicioID);

                        foreach (var obj in grupo)
                        {
                            var ES = db.EstacionServicios.Select(s => new { s.ID, s.Nombre, s.GrupoAlcaldiaID, s.SubEstacionPrincipalID, s.CuentaInternaActivo, s.CuentaInternaPasivo }).Single(o => o.ID.Equals(obj.Key));

                            decimal vMontoCuentaInterna = 0;
                            var almas = otrasAlma.Where(o => o.ID.Equals(obj.Key));

                            if (ES.GrupoAlcaldiaID.Equals(1))
                                vMontoCuentaInterna = Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                            else
                            {
                                vMontoCuentaInterna = Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros) - (almas.Count() > 0 ? Convert.ToDecimal(almas.Sum(s => s.MontoTotal)) : 0m), 2, MidpointRounding.AwayFromZero);
                            }
                            
                            /////////////////////////////////////// si es diferente la estacion de pago
                            if (!ES.ID.Equals(IDEstacionServicio))
                            {
                                gr++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = ES.CuentaInternaActivo,
                                    Monto = vMontoCuentaInterna,// Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vMontoCuentaInterna / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vMontoCuentaInterna), //(MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros)),
                                    Fecha = FechaCheque,
                                    Descripcion = "Cuenta por Cobrar. " + ES.Nombre + " con Pago Masivo Manual " + Referencia,
                                    Linea = gr,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }
                            
                            DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).ToList().ForEach(item =>
                            {
                                //**********PAGOS DE DOCUMENTOS**********
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = prov.CuentaContableID,
                                    Monto = Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.Litros),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Pago Doc. " + item.Referencia + " con Pago Masivo Manual " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = item.EstacionServicioID,
                                    SubEstacionID = item.SubEstacionID
                                });

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
                                        Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Pago Masivo Cheque " + Numero.ToString(),
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = item.EstacionServicioID,
                                        SubEstacionID = item.SubEstacionID
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
                                        EstacionServicioID = item.EstacionServicioID,
                                        SubEstacionID = item.SubEstacionID
                                    });
                                }

                            });

                            if (almas.Count() > 0)
                            {
                                //***************RETENCION ALCALDIA ESTACIONES FUERA MANAGUA****************
                                string tALMAOut = "";

                                tALMAOut = (DetalleImp.Count(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)) > 0 ? DetalleImp.Where(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)).First().Concepto + "  " + ES.Nombre + " Nro. " + DetalleImp.Where(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)).First().Numero.ToString() : "");

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = 231,
                                    Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(almas.Sum(s => s.MontoTotal)), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(almas.Sum(s => s.MontoTotal)) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(almas.Sum(s => s.MontoTotal)))),
                                    Fecha = FechaCheque,
                                    Descripcion = tALMAOut,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = ES.ID,
                                    SubEstacionID = ES.SubEstacionPrincipalID
                                });
                            }
                            /////////////////////////////////////////////////////////////////// si es diferente estacion de pago
                            if (!ES.ID.Equals(IDEstacionServicio))
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = IDCuentaPasivoMasiva,
                                    Monto = -Math.Abs(Decimal.Round(vMontoCuentaInterna, 2, MidpointRounding.AwayFromZero)), //-Math.Abs(Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vMontoCuentaInterna / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vMontoCuentaInterna)), //-Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros))),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " cuenta por Pagar. " + ES.Nombre + " con Pago Masivo Manual " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = ES.ID,
                                    SubEstacionID = ES.SubEstacionPrincipalID
                                });
                            }
                        }

                        //***************RETENCION ALCALDIA MANAGUA****************
                        decimal vALMA = 0;
                        string tALMA = "";

                        vALMA = Decimal.Round(alma, 2, MidpointRounding.AwayFromZero);
                        tALMA = (DetalleImp.Count(d => d.EsAlcaldia && d.GrupoID.Equals(1)) > 0 ? DetalleImp.Where(d => d.EsAlcaldia && d.GrupoID.Equals(1)).First().Concepto + "  Managua Nro. " + DetalleImp.Where(d => d.EsAlcaldia && d.GrupoID.Equals(1)).First().Numero.ToString() : "");

                        gr++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = 231,
                            Monto = -Math.Abs(Decimal.Round(vALMA, 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vALMA / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vALMA)),
                            Fecha = FechaCheque,
                            Descripcion = tALMA,
                            Linea = gr,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });

                        //***************RETENCION IR****************
                        decimal vIR = 0;
                        string tIR = "";

                        vIR = Decimal.Round(DetalleImp.Where(d => !d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                        tIR = (DetalleImp.Count(d => !d.EsAlcaldia) > 0 ? DetalleImp.Where(d => !d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => !d.EsAlcaldia).First().Numero.ToString() : "");

                        gr++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = prov.ImpuestoRetencionID,
                            Monto = -Math.Abs(Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vIR)),
                            Fecha = FechaCheque,
                            Descripcion = tIR,
                            Linea = gr,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });


                        //************ULTIMA LINEA************
                        gr++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = 526,
                            Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)), //DetalleM.Where(d => d.Litros > 0).Sum(s => s.Litros)
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(DetalleM.Where(d => d.Litros > 0).Sum(s => s.Litros) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : DetalleM.Where(d => d.Litros > 0).Sum(s => s.Litros))),
                            Fecha = FechaCheque,
                            Descripcion = "Pago Masivo Manual " + Referencia,
                            Linea = gr,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });

                        /*
                        DetalleM.Where(d => d.Pagado).ToList().ForEach(item =>
                        {
                            //Decimal.Round(DetalleM.Where(m => m.Pagado).Sum(s => s.IVA), 2, MidpointRounding.AwayFromZero);

                            if (item.IVA > 0 && !item.IVAPagado)
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
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 0,
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
                        */
                        #endregion
                    }
                    else if (!_EsManual)
                    {
                        #region <<< MASIVO CHEQUE >>>

                        //Alcaldias de Managua
                        var alma = DetalleImp.Where(o => o.GrupoID.Equals(1)).Sum(s => s.Monto);

                        //Montos Otras Alcaldias
                        var otrasAlma = from d in DetalleImp
                                        join es in db.EstacionServicios on d.Alcaldia equals es.Nombre
                                        where d.GrupoID.Equals(0)
                                        group d by new { es.ID, d.Alcaldia } into g
                                        select new { MontoTotal = g.Sum(s => s.Monto), ID = g.Key.ID, Nombre = g.Key.Alcaldia };

                        var grupo = DetalleM.Where(d => d.Litros > 0).GroupBy(dm => dm.EstacionServicioID);

                        foreach (var obj in grupo)
                        {
                            var ES = db.EstacionServicios.Select(s => new { s.ID, s.Nombre, s.GrupoAlcaldiaID, s.SubEstacionPrincipalID, s.CuentaInternaActivo, s.CuentaInternaPasivo }).Single(o => o.ID.Equals(obj.Key));
                            
                            decimal vMontoCuentaInterna = 0;
                            var almas = otrasAlma.Where(o => o.ID.Equals(obj.Key));

                            if (ES.GrupoAlcaldiaID.Equals(1))
                                vMontoCuentaInterna = Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros), 2, MidpointRounding.AwayFromZero);
                            else
                            {                                
                                vMontoCuentaInterna = Decimal.Round(DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).Sum(s => s.Litros) - (almas.Count() > 0 ? Convert.ToDecimal(almas.Sum(s => s.MontoTotal)) : 0m), 2, MidpointRounding.AwayFromZero);
                            }

                            //Si la estacion de Pago es diferente
                            if (!ES.ID.Equals(IDEstacionServicio))
                            {
                                gr++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = ES.CuentaInternaActivo,
                                    Monto = vMontoCuentaInterna,
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vMontoCuentaInterna / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vMontoCuentaInterna),
                                    Fecha = FechaCheque,
                                    Descripcion = "Cuenta por Cobrar. " + ES.Nombre + " con Pago Masivo Cheque " + Numero.ToString(),
                                    Linea = gr,
                                    CentroCostoID = 0,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                            }

                            DetalleM.Where(d => d.Litros > 0 && d.EstacionServicioID.Equals(obj.Key)).ToList().ForEach(item =>
                            {
                                //**********PAGOS DE DOCUMENTOS**********
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = prov.CuentaContableID,
                                    Monto = Decimal.Round(item.Litros, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = (MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(item.Litros / _TipoCambio, 2, MidpointRounding.AwayFromZero) : item.Litros),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " Pago Doc. " + item.Referencia + " con Pago Masivo Cheque " + Numero.ToString(),
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = item.EstacionServicioID,
                                    SubEstacionID = item.SubEstacionID
                                });

                                //***** MANEJO DEL IVA  *******//
                                if (item.IVA > 0 && !item.IVAPagado)
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
                                        Descripcion = prov.Nombre + " Acreditación IVA Doc. " + item.Referencia + " con Pago Masivo Cheque " + Numero.ToString(),
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = item.EstacionServicioID,
                                        SubEstacionID = item.SubEstacionID
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
                                        EstacionServicioID = item.EstacionServicioID,
                                        SubEstacionID = item.SubEstacionID
                                    });
                                }

                            });

                            if (almas.Count() > 0)
                            {
                                //***************RETENCION ALCALDIA ESTACIONES FUERA MANAGUA****************
                                string tALMAOut = "";

                                tALMAOut = (DetalleImp.Count(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)) > 0 ? DetalleImp.Where(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)).First().Concepto + "  " + ES.Nombre + " Nro. " + DetalleImp.Where(d => d.EsAlcaldia && d.Alcaldia.Equals(ES.Nombre)).First().Numero.ToString() : "");

                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = 231,
                                    Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(almas.Sum(s => s.MontoTotal)), 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(Convert.ToDecimal(almas.Sum(s => s.MontoTotal)) / _TipoCambio, 2, MidpointRounding.AwayFromZero) : Convert.ToDecimal(almas.Sum(s => s.MontoTotal)))),
                                    Fecha = FechaCheque,
                                    Descripcion = tALMAOut,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = ES.ID,
                                    SubEstacionID = ES.SubEstacionPrincipalID
                                });
                            }

                            //**************CUENTAS POR PAGAR CASA MATRIZ***************
                            //Si la estacion de Pago es diferente
                            if (!ES.ID.Equals(IDEstacionServicio))
                            {
                                i++;
                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = IDCuentaPasivoMasiva,
                                    Monto = -Math.Abs(Decimal.Round(vMontoCuentaInterna, 2, MidpointRounding.AwayFromZero)),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vMontoCuentaInterna / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vMontoCuentaInterna)),
                                    Fecha = FechaCheque,
                                    Descripcion = prov.Nombre + " cuenta por Pagar. " + ES.Nombre + " con Pago Masivo Cheque " + Referencia,
                                    Linea = i,
                                    CentroCostoID = 0,
                                    EstacionServicioID = ES.ID,
                                    SubEstacionID = ES.SubEstacionPrincipalID
                                });
                            }
                        }

                        //***************RETENCION ALCALDIA MANAGUA****************
                        decimal vALMA = 0;
                        string tALMA = "";
                        if (!alma.Equals(0))
                        {
                            vALMA = Decimal.Round(alma, 2, MidpointRounding.AwayFromZero);
                            tALMA = (DetalleImp.Count(d => d.EsAlcaldia && d.GrupoID.Equals(1)) > 0 ? DetalleImp.Where(d => d.EsAlcaldia && d.GrupoID.Equals(1)).First().Concepto + "  Managua Nro. " + DetalleImp.Where(d => d.EsAlcaldia && d.GrupoID.Equals(1)).First().Numero.ToString() : "");

                            gr++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 231,
                                Monto = -Math.Abs(Decimal.Round(vALMA, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vALMA / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vALMA)),
                                Fecha = FechaCheque,
                                Descripcion = tALMA,
                                Linea = gr,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }
                        //***************RETENCION IR****************
                        decimal vIR = 0;
                        string tIR = "";

                        vIR = Decimal.Round(DetalleImp.Where(d => !d.EsAlcaldia).Sum(s => s.Monto), 2, MidpointRounding.AwayFromZero);
                        tIR = (DetalleImp.Count(d => !d.EsAlcaldia) > 0 ? DetalleImp.Where(d => !d.EsAlcaldia).First().Concepto + "  Nro. " + DetalleImp.Where(d => !d.EsAlcaldia).First().Numero.ToString() : "");

                        gr++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = prov.ImpuestoRetencionID,
                            Monto = -Math.Abs(Decimal.Round(vIR, 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(vIR / _TipoCambio, 2, MidpointRounding.AwayFromZero) : vIR)),
                            Fecha = FechaCheque,
                            Descripcion = tIR,
                            Linea = gr,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });

                        //************ULTIMA LINEA************
                        gr++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = db.CuentaBancarias.Single(b => b.ID.Equals(IDCuentaBancaria)).CuentaContableID,
                            Monto = -Math.Abs(Decimal.Round(_Total, 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs((MonedaPrimaria.Equals(MonedaPrimaria) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : _Total)),
                            Fecha = FechaCheque,
                            Descripcion = "Pago Masivo Cheque " + Numero.ToString(),                            
                            Linea = gr,
                            CentroCostoID = 0,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });

                        /*
                        DetalleM.Where(d => d.Pagado).ToList().ForEach(item =>
                        {
                            //Decimal.Round(DetalleM.Where(m => m.Pagado).Sum(s => s.IVA), 2, MidpointRounding.AwayFromZero);

                            if (item.IVA > 0 && !item.IVAPagado)
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
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = 0,
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
                        */
                        #endregion
                    }

                    return CD.OrderBy(o => o.Linea).ToList();
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
            if (MDI != null)
                MDI.CleanDialog(ShowMsg, Next, IDPrint, RefreshMDI, true);
            if (MDIManual != null)
                MDIManual.CleanDialog(ShowMsg, Next, IDPrint, RefreshMDI, true);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!_Guardado)
                {
                    if (DetalleM != null)
                    {
                        if (DetalleM.Count > 0)
                        {
                            if (Parametros.General.DialogMsg("El Pago Masivo actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            {
                                Next = false;
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }

                if (prov != null)
                {
                    if (prov.AplicaBloqueo)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.Proveedors.Single(s => s.ID.Equals(prov.ID)).Bloqueado = false;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                e.Cancel = true;
                return;
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
                    prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));

                    //if (_EsManual)
                    //    _AplicaAlcaldia = RetieneAlcadia();
                    //else

                        RetieneAlcaldia(prov.ID);

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
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    if (db.ProveedorUsadoPorEstacions.Count(c => c.ProveedorID.Equals(prov.ID)) > 0)
                    {
                        Parametros.General.DialogMsg("El Proveedor " + prov.Codigo + " | " + prov.Nombre + ", esta siendo utilizado por otro usuario. Favor comunicarse con Oficina Central. ", Parametros.MsgType.warning);
                        return;
                    }
                    this.bdsDetalle.DataSource = null;
                    IDCuentaPasivoMasiva = db.EstacionServicios.Single(s => s.ID.Equals(IDEstacionServicio)).CuentaInternaPasivo;
                    //prov = db.Proveedors.Single(p => p.ID.Equals(IDBeneficiario));
                    chkAnticipo.Enabled = false;
                    if (prov.AplicaAbono)
                    {
                        colMonto.OptionsColumn.AllowEdit = true;
                        colMonto.OptionsColumn.AllowFocus = true;
                    }

                    if (prov.PagoMasivoManual)
                        layoutControlItemReferencia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    else
                    {
                        layoutControlItemTransferencia.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlItemNro.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    }
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

                        var lista = db.ProveedorMasivoEstacions.Where(o => o.ProveedorID.Equals(prov.ID)).Select(s => s.EstacionServicioID);

                        if (!Editable)
                        {
                            DetalleM = (from m in db.Movimientos
                                        where (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(5) || m.MovimientoTipoID.Equals(66))
                                        && !m.Anulado && !m.Pagado && m.ProveedorID.Equals(prov.ID) && lista.Any(a => a.Equals(m.EstacionServicioID)) && (m.FechaVencimiento.HasValue ? Convert.ToDateTime(m.FechaVencimiento) : m.FechaRegistro) <= Convert.ToDateTime(dateHasta.EditValue)
                                        select m).OrderBy(o => o.EstacionServicioID).ThenBy(t => t.FechaRegistro).ToList();


                            if (DetalleM.Count <= 0)
                            {
                                Parametros.General.DialogMsg("PROVEEDOR / BENEFICIARIO NO TIENE FACTURAS PENDIENTES DE PAGO", Parametros.MsgType.warning);
                                return;
                            }
                        }
                        else
                        {
                            var ListaMasiva = from p in db.ProveedorMasivoEstacions
                                                     where p.ProveedorID.Equals(prov.ID)
                                                     select new {  p.EstacionServicioID };

                            DetalleM = (from m in db.Movimientos
                                        join l in ListaMasiva on m.EstacionServicioID equals l.EstacionServicioID
                                        where (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(5) || m.MovimientoTipoID.Equals(41) || m.MovimientoTipoID.Equals(66))
                                        && !m.Anulado && (!m.Pagado || m.MovimientoReferenciaID.Equals(EntidadAnterior.ID)) && m.ProveedorID.Equals(prov.ID) 
                                        //&& (m.FechaVencimiento.HasValue ? Convert.ToDateTime(m.FechaVencimiento) : m.FechaRegistro) <= Convert.ToDateTime(dateHasta.EditValue)
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
                                    DetalleImp.Add(new Entidad.Retencione { Monto = det.Monto, Numero = det.Numero, Concepto = det.Concepto, SubTotal = det.SubTotal, EsAlcaldia = det.EsAlcaldia, CuentaContableID = det.CuentaContableID, GrupoID = det.GrupoID, Alcaldia = det.Alcaldia });
                                });
                            
                            gvDataImp.RefreshData();
                            this.lkCuentaBancaria.Properties.ReadOnly = true;
                        }
                        
                        this.bdsDetalle.DataSource = DetalleM;                        
                        gvDetalle.RefreshData();
                        SetMontoCheque();

                    
                        this.glkBeneficiario.Enabled = false;
                        this.btnLoad.Enabled = false;
                        this.dateHasta.Enabled = false;

                        if (_EsManual && prov.AplicaBloqueo)
                        {
                            db.Proveedors.Single(s => s.ID.Equals(prov.ID)).Bloqueado = true;
                            db.SubmitChanges();
                        }
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
                Numero = GetNroCheque(db);
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
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Cambio valor de Retenciones
        private void gvDataImp_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            
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

                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), gvDetalle.GetRowCellDisplayText(e.RowHandle, "EstacionServicioID").ToString(), Convert.ToBoolean(e.Value), vSaldo);

                        }
                        else
                        {
                            decimal vSaldoAnreriot = 0;

                            if (gvDetalle.GetRowCellValue(e.RowHandle, "Litros") != null)
                                vSaldoAnreriot = Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "Litros"));


                            gvDetalle.SetRowCellValue(e.RowHandle, "Litros", 0m);
                            ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), gvDetalle.GetRowCellDisplayText(e.RowHandle, "EstacionServicioID").ToString(), Convert.ToBoolean(e.Value), vSaldoAnreriot);
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

                                ListaImpuesto(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "ID")), gvDetalle.GetRowCellDisplayText(e.RowHandle, "EstacionServicioID").ToString(), (vAbono > 0 ? true : false), (vAbono > 0 ? vAbono : vSaldo));
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

        //Botón para mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCampos(true, false))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable.OrderBy(o => o.Linea).ToList();
                        nf.Text = "Comprobante de Pago";
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
            ListaImpuesto(0, false);
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



    }
}