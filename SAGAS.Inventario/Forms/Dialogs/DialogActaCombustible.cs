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
using SAGAS.Inventario.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DevExpress.Data.Linq;
using System.Data.Linq.SqlClient;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogActaCombustible : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormActaCombustible MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal bool ShowFinish = false;        
        internal Entidad.InventarioCombustible Inventario;
        internal Entidad.EstacionServicio ES;
        internal List<Entidad.ProductoVariacion> PV;
        internal bool Editable = false;
        private bool ShowMsg = false;
        private bool Next = false;
        private int UsuarioID;        
        private int _IDPrint = 0;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private int REditID = 0;
        private bool _Changes = false;
        private DataTable dtInventario = new DataTable();
        private List<Parametros.ListIdTIDTanqueDisplayPriceValue> Equivalente = new List<Parametros.ListIdTIDTanqueDisplayPriceValue>();
        private bool ReLoad = false;
        private int contadorReLoad = 1;
        //Entidad Acta
        private List<Entidad.ActaCombustible> Actas = new List<Entidad.ActaCombustible>();
        public List<Entidad.ActaCombustible> A
        {
            get { return Actas; }
            set
            {
                Actas = value;
                this.bdsDetalle.DataSource = this.Actas;
            }
        }

        private string Numero
        {
            get { return txtNumero.Text; }
            set { txtNumero.Text = value; }
        }

        private string Observacion
        {
            get { return memoObservacion.Text; }
            set { memoObservacion.Text = value; }
        }

        private string ComentarioActa
        {
            get { return memoComentarioActa.Text; }
            set { memoComentarioActa.Text = value; }
        }

        public int IDInventarioCombustible
        {
            get { return Convert.ToInt32(lkInventario.EditValue); }
            set { lkInventario.EditValue = value; }
        }

        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        private bool InventarioFinalizado
        {
            get { return Convert.ToBoolean(chkInventarioFinalizado.Checked); }
            set { chkInventarioFinalizado.Checked = value; }
        }

        private bool BloquearCombustible
        {
            get { return Convert.ToBoolean(chkBloquearCombustible.Checked); }
            set { chkBloquearCombustible.Checked = value; }
        }

        #endregion

        #region *** INICIO ***

        public DialogActaCombustible(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;

            if (Editable)
            { //-- Bloquear Controles --//    
                dateFecha.Properties.ReadOnly = false;
                txtNumero.Properties.ReadOnly = true;
                lkInventario.Properties.ReadOnly = true;
            }
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
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                db.CommandTimeout = 600;
                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(24)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(24)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                txtNumero.Text = number.ToString("000000000");

                var obj = from iv in db.InventarioCombustibles
                          where iv.EstacionServicioID.Equals(IDEstacionServicio) && iv.SubEstacionID.Equals(IDSubEstacion) && (iv.MovimientoID.Equals(0) || Editable)
                          select new
                          {
                              iv.ID,
                              Display = String.Format("{0:000000000} | {1:dd/MM/yyyy}", iv.Numero, iv.FechaInventario),
                              Mostrar = String.Format("{0:000000000} | {1:dd/MM/yyyy}", iv.Numero, iv.FechaInventario)
                          };

                this.memoComentarioActa.Enabled = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "memoComentarioActa");
                lkInventario.Properties.DataSource = obj;

                if (Editable)
                {
                    Numero = EntidadAnterior.Numero.ToString("000000000");
                    Fecha = EntidadAnterior.FechaRegistro;
                    ComentarioActa = EntidadAnterior.ComentarioActa;
                    Observacion = EntidadAnterior.Comentario;

                    btnLoad_Click(null, null);
                }
                else
                    Fecha = Convert.ToDateTime(db.GetDateServer()).Date;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(memoObservacion, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtNumero.EditValue == null || dateFecha.EditValue == null || lkInventario.EditValue == null || String.IsNullOrEmpty(Observacion))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del Acta de Combustible.", Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidateTipoCambio(dateFecha, errRequiredField, db))
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

            if (!Editable)
            {
                if (db.Movimientos.Count(c => c.MovimientoTipoID.Equals(24) && c.EstacionServicioID.Equals(IDEstacionServicio) && c.SubEstacionID.Equals(IDSubEstacion) && !c.Finalizado) > 0)
                {
                    Parametros.General.DialogMsg("Existe una Acta sin finalizar, favor revisar.", Parametros.MsgType.warning);
                    return false;
                }
            }

            return true;
        }

        private bool ValidarRecosteo(DateTime vFecha)
        {
            try
            {
                {
                    db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                    if (!Parametros.General.ValidatePeriodoContable(vFecha, db, IDEstacionServicio))
                    {
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            { 
            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            return false;
            }
        }

        private bool Guardar(Boolean Finish)
        {
            if (!ValidarCampos()) return false;

            if (Finish)
            {
                bool NoDa = false;
                bool NoHas = false;
                string Prod = "", vari = "";

               foreach ( var p in Actas)
                    { 
                        var Obj = PV.FirstOrDefault( s => s.ProductoID.Equals(p.ProductoID));

                        if (Obj != null)
                        {
                        if (Math.Abs(p.Diferencia) > Obj.PermisibleActaDiferencia)
                        {
                            Prod = p.Nombre;
                            vari = Obj.PermisibleActaDiferencia.ToString("#,0.000");
                            NoDa = true;
                            break;
                        }
                        }
                        else
                        {
                            Prod = p.Nombre;
                            NoHas = true;
                            break;
                        }
                    
                    }

                if (NoHas)
                {
                    Parametros.General.DialogMsg("la Estación de servicio no tiene variación del producto " + Prod, Parametros.MsgType.warning);
                    return false;
                }

                if (NoDa)
                {
                    Parametros.General.DialogMsg("La DIFERENCIA SOB/FALT LIBROS VRS CDI es mayor al valor permitible de la Estación de Servicio en el producto " + Prod + ", " + vari, Parametros.MsgType.warning);
                    return false;
                }
            }

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 9000;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M;

                    if (Editable)
                    {
                        M = db.Movimientos.Single(m => m.ID.Equals(EntidadAnterior.ID));
                        M.ActaCombustibles.Clear();
                        M.ComprobanteContables.Clear();
                    }
                    else
                    {
                        M = new Entidad.Movimiento();
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.MovimientoTipoID = 24;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(24)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(24)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        M.Numero = number;
                    }

                    M.UsuarioID = UsuarioID;
                    M.FechaRegistro = Fecha;
                    M.MonedaID = Parametros.Config.MonedaPrincipal();
                    

                    if (EntidadAnterior == null)
                    {
                        if (!String.IsNullOrEmpty(ComentarioActa))
                        {
                            M.ComentarioActa = ComentarioActa;
                            M.UsuarioComentarioActaID = UsuarioID;
                        }
                    }
                    else
                    {
                        if (EntidadAnterior.ComentarioActa == null)
                        {
                            if (!String.IsNullOrEmpty(ComentarioActa))
                            {
                                M.ComentarioActa = ComentarioActa;
                                M.UsuarioComentarioActaID = UsuarioID;
                            }
                        }
                        else
                        {
                            if (!Convert.ToString(EntidadAnterior.ComentarioActa).Equals(ComentarioActa))
                            {
                                M.ComentarioActa = ComentarioActa;
                                M.UsuarioComentarioActaID = UsuarioID;
                            }
                        }
                    }

                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Observacion;
                    M.InventarioBloqueado = BloquearCombustible;
                    M.Finalizado = Finish;
                    
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó el Acta de Combustible: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Acta de Combustible: " + M.Numero, this.Name);
                    }

                    db.SubmitChanges();

                    

                    Actas.ForEach(line => line.EstacionServicioID = M.EstacionServicioID);
                    Actas.ForEach(line => line.SubEstacionID = M.SubEstacionID);
                    Actas.ForEach(line => line.FechaActa = M.FechaRegistro);
                    M.ActaCombustibles.AddRange(Actas);
                    db.SubmitChanges();

                    M.Monto = Decimal.Round(M.ActaCombustibles.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);
                     M.TipoCambio = Decimal.Round((db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha .Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m), 4, MidpointRounding.AwayFromZero);
                     M.MontoMonedaSecundaria = Decimal.Round(M.Monto / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                    

                    Entidad.InventarioCombustible INV = db.InventarioCombustibles.Single(iv => iv.ID.Equals(Inventario.ID));
                    INV.MovimientoID = M.ID;
                    INV.Finalizado = InventarioFinalizado;

                    if (Finish)
                    {
                        #region <<< REGISTRANDO  KARDEX >>>
                        
                        foreach (Entidad.ActaCombustible dk in Actas)
                        {
                            
                            var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));

                            Entidad.Kardex KX = new Entidad.Kardex();
                            KX.MovimientoID = M.ID;

                            KX.ProductoID = Producto.ID;
                            KX.EsProducto = !Producto.EsServicio;
                            KX.UnidadMedidaID = Producto.UnidadMedidaID;
                            KX.Fecha = M.FechaRegistro;
                            KX.EstacionServicioID = IDEstacionServicio;
                            KX.SubEstacionID = IDSubEstacion;

                            if (dk.Variaciones < 0)
                            {
                                KX.AlmacenSalidaID = dk.TanqueID;
                                KX.CantidadInicial = Decimal.Round(Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, dk.ProductoID, dk.TanqueID, M.FechaRegistro.Date, false), 3, MidpointRounding.AwayFromZero);
                                KX.CantidadSalida = Math.Abs(Decimal.Round(dk.Variaciones, 3, MidpointRounding.AwayFromZero));
                                KX.CostoSalida = Math.Abs(Decimal.Round(dk.CostoPromedio, 4, MidpointRounding.AwayFromZero));
                                //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                                KX.CantidadFinal = Math.Abs(Decimal.Round(KX.CantidadInicial - KX.CantidadSalida, 3, MidpointRounding.AwayFromZero));
                                KX.CostoFinal = Math.Abs(Decimal.Round(KX.CostoSalida, 4, MidpointRounding.AwayFromZero));
                                KX.CostoTotal = Math.Abs(Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                if (KX.CantidadSalida > KX.CantidadInicial)
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    trans.Rollback();
                                    Parametros.General.DialogMsg("La cantidad a salir del producto: " + Producto.Codigo + " | " + Producto.Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
                                    return false;
                                }

                                db.Kardexes.InsertOnSubmit(KX);
                                db.SubmitChanges();

                                
                            }
                            else if (dk.Variaciones > 0)
                            {
                                KX.AlmacenEntradaID = dk.TanqueID;
                                KX.CantidadInicial = Decimal.Round(Parametros.General.SaldoKardex(db, IDEstacionServicio, IDSubEstacion, dk.ProductoID, dk.TanqueID, M.FechaRegistro.Date, true), 3, MidpointRounding.AwayFromZero);

                                KX.CantidadEntrada = Math.Abs(Decimal.Round(dk.Variaciones, 3, MidpointRounding.AwayFromZero));
                                KX.CostoEntrada = Math.Abs(Decimal.Round(dk.CostoPromedio, 4, MidpointRounding.AwayFromZero));
                                //------- ESTABLECER CANTIDAD FINAL ---------//                                     
                                KX.CantidadFinal = Math.Abs(Decimal.Round(KX.CantidadInicial + KX.CantidadEntrada, 3, MidpointRounding.AwayFromZero));
                                KX.CostoTotal = Math.Abs(Decimal.Round(dk.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                KX.CostoFinal = Math.Abs(Decimal.Round(KX.CostoEntrada, 4, MidpointRounding.AwayFromZero));

                                db.Kardexes.InsertOnSubmit(KX);
                                db.SubmitChanges();
                            }

                        }

                        foreach (Entidad.ActaCombustible dk in Actas)
                        {
                            //Tabla Inventario Cierre
                            Entidad.InventarioCierre IC = new Entidad.InventarioCierre();
                            IC.AlmacenID = dk.TanqueID;
                            IC.Fecha = M.FechaRegistro;
                            IC.ProductoID = dk.ProductoID;
                            IC.EstacionServicioID = IDEstacionServicio;
                            IC.SubEstacionID = IDSubEstacion;
                            IC.CantidadFinal = Decimal.Round(Convert.ToDecimal(db.SaldoKardex(IDEstacionServicio, IDSubEstacion, dk.ProductoID, dk.TanqueID, M.FechaRegistro, false)), 3, MidpointRounding.AwayFromZero);
                            IC.CostoFinal = Math.Abs(Decimal.Round(dk.CostoPromedio, 4, MidpointRounding.AwayFromZero));
                            IC.CostoTotalAlmacen = Decimal.Round(Convert.ToDecimal(db.SaldoCostoTotalKardexPorAlmacen(dk.TanqueID, dk.ProductoID, IDEstacionServicio, IDSubEstacion, M.FechaRegistro)), 2, MidpointRounding.AwayFromZero);
                            IC.CostoTotal = Decimal.Round(Convert.ToDecimal(db.SaldoCostoTotalPorCierre(dk.TanqueID, dk.ProductoID, IDEstacionServicio, IDSubEstacion, M.FechaRegistro)), 2, MidpointRounding.AwayFromZero);
                            db.InventarioCierres.InsertOnSubmit(IC);
                            db.SubmitChanges();
                        }

                        #endregion

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


                    }

                    db.SubmitChanges();
                    trans.Commit();

                    _IDPrint = (Finish ? M.ID : 0);
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    _Guardado = true;
                    REditID = 0;
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

                    #region <<< DETALLE_COMPROBANTE >>>
                    decimal _TC = Decimal.Round((db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m), 4, MidpointRounding.AwayFromZero);
                    int i = 1;
                    Actas.ForEach(line =>
                        {
                            var P = db.Productos.Single(p => p.ID.Equals(line.ProductoID));

                            if (!line.CostoTotal.Equals(0))
                            {
                                if (line.CostoTotal > 0)
                                {
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = P.CuentaFaltanteID,
                                        Monto = Math.Abs(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TC,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero) / _TC, 2, MidpointRounding.AwayFromZero)),  //_TC  0,//(IDMonedaPrincipal.Equals(Parametros.Config.MonedaPrincipal()) ? Decimal.Round(_Total / _TipoCambio, 2, MidpointRounding.AwayFromZero) : spMonto.Value),
                                        Fecha = Fecha,
                                        Descripcion = "Acta de combustible mes " + Numero,
                                        Linea = i,
                                        CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(P.CuentaFaltanteID)).CecoID,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;

                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = P.CuentaInventarioID,
                                        Monto = -Math.Abs(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TC,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero) / _TC, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Acta de combustible Nro " + Numero,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;
                                }
                                else
                                {      
                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = P.CuentaInventarioID,
                                        Monto = Math.Abs(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TC,
                                        MontoMonedaSecundaria = Math.Abs(Decimal.Round(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero) / _TC, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Acta de combustible mes " + Numero,
                                        Linea = i,
                                        CentroCostoID = 0,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;

                                    CD.Add(new Entidad.ComprobanteContable
                                    {
                                        CuentaContableID = P.CuentaSobranteID,
                                        Monto = -Math.Abs(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero)),
                                        TipoCambio = _TC,
                                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Decimal.Round(line.CostoTotal, 2, MidpointRounding.AwayFromZero) / _TC, 2, MidpointRounding.AwayFromZero)),
                                        Fecha = Fecha,
                                        Descripcion = "Acta de combustible mes " + Numero,
                                        Linea = i,
                                        CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(P.CuentaSobranteID)).CecoID,
                                        EstacionServicioID = IDEstacionServicio,
                                        SubEstacionID = IDSubEstacion
                                    });
                                    i++;
                                }
                            }
                        });

                        #endregion

                        return CD;
                    

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
                }

            }

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

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar(false)) return;

            this.Close();
        }

        private void bntFinalizar_Click(object sender, EventArgs e)
        {
            BloquearCombustible = true;
            InventarioFinalizado = true;

            if (!Guardar(true)) return;

            this.Close();
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            EntidadAnterior = null;
            MDI.CleanDialog(ShowMsg, Next, _IDPrint, REditID);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (REditID.Equals(0))
                {
                    if (btnLoad.Enabled.Equals(false))
                    {
                        if (Parametros.General.DialogMsg("El Acta de Combustible actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        {
                            Next = false;
                            e.Cancel = true;
                        }
                    }

                    if (!Editable && Inventario != null)
                    {
                        db.InventarioCombustibles.Single(iv => iv.ID.Equals(Inventario.ID)).Finalizado = false;
                        db.SubmitChanges();
                    }
                }
            }            
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated_1(object sender, EventArgs e)
        {
             Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }
       
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                try
                {

                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    dv.CommandTimeout = 600;
                    Inventario = db.InventarioCombustibles.Single(iv => iv.ID.Equals(IDInventarioCombustible));
                    Inventario.Finalizado = true;
                    db.SubmitChanges();
                    dtInventario = new DataTable();
                    Fecha = Inventario.FechaInventario;
                    IDEstacionServicio = Inventario.EstacionServicioID;
                    IDSubEstacion = Inventario.SubEstacionID;

                    ES = db.EstacionServicios.Single(s => s.ID.Equals(IDEstacionServicio));
                    PV = db.ProductoVariacions.Where(o => o.EstacionServicioID.Equals(ES.ID) && o.SubEstacionServicioID.Equals(IDSubEstacion)).ToList();

                    List<Int32> ListaAC = new List<int>();

                    if (!Editable)
                    {
                        var obj = (from inv in Inventario.InventarioCombustibleValores
                                   join t in db.Tanques on inv.TanqueID equals t.ID
                                   join p in db.Productos on inv.ProductoID equals p.ID
                                   select new
                                   {
                                       TanqueID = t.ID,
                                       Tanque = t.Nombre,
                                       ProductoID = p.ID,
                                       Codigo = p.Codigo,
                                       Nombre = p.Nombre
                                   }).OrderBy(o => o.Codigo).ThenBy(tb => tb.Tanque).ToList();

                        obj.ForEach(item =>
                            {
                                Actas.Add(new Entidad.ActaCombustible { TanqueID = item.TanqueID, TanqueNombre = item.Tanque, ProductoID = item.ProductoID, Codigo = item.Codigo, Nombre = item.Nombre, FechaActa = Fecha });
                            });
                    }
                    else if (Editable)
                    {
                        Actas = EntidadAnterior.ActaCombustibles.ToList();
                        Actas.ForEach(a => ListaAC.Add(a.ID));
                    }
                    #region <<< MEDICIONES >>>
                    //TABLA MEDICIONES

                    
                        dtInventario.Columns.Add("ID", typeof(Int32));
                        dtInventario.Columns.Add("Datos", typeof(String));

                    
                    decimal Cantidad = Inventario.InventarioCombustibleDetalles.GroupBy(g => g.MedicionNumero).Count();

                    for (int i = 1; i <= Cantidad; i++)
                    {
                        dtInventario.Columns.Add(new DataColumn { ColumnName = i.ToString(), DataType = typeof(String), DefaultValue = "" });

                        var colAlma = new Parametros.MyBandColumn("Medición " + i.ToString(), i.ToString(), null);
                        colAlma.Tag = i;
                        colAlma.OptionsFilter.AllowAutoFilter = false;
                        colAlma.OptionsFilter.AllowFilter = false;
                        colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                        colAlma.OptionsColumn.AllowEdit = true;

                        BandDatos.Columns.Add(colAlma);
                    }

                    colDatos.Width = 125;

                    var Lecturas = (from p in db.Productos
                                    join t in db.Tanques on p.ID equals t.ProductoID
                                    where t.EstacionServicioID.Equals(IDEstacionServicio) && t.SubEstacionID.Equals(IDSubEstacion) && t.Activo
                                    select new
                                    {
                                        ID = t.ID,
                                        IDP = p.ID,
                                        Datos = t.Nombre + " => " + p.Nombre,
                                        Combustible = p.Nombre,
                                        t.Color
                                    }).ToList();

                    Lecturas.ForEach(line => { dtInventario.Rows.Add(line.ID, line.Datos); });

                    Inventario.InventarioCombustibleDetalles.ToList().ForEach(line =>
                    {
                        DataRow[] ESRow = dtInventario.Select("ID = " + line.TanqueID);
                        DataRow row = ESRow.FirstOrDefault();

                        if (row != null)
                            row[line.MedicionNumero.ToString()] = line.MedicionValor;
                    });

                    gridLecturas.DataSource = dtInventario;
                    bgvDataLecturas.RefreshData();

                    #endregion

                    DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
                    DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
                    styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.Red;
                    styleFormatCondition1.Appearance.Options.UseBackColor = true;
                    styleFormatCondition1.Column = this.colI;
                    styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                    styleFormatCondition1.Expression = "Abs([PorcentajesVariaciones]) > " + ES.PermisibleActaVariacion.ToString();
                    styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.Red;
                    styleFormatCondition2.Appearance.Options.UseBackColor = true;
                    styleFormatCondition2.Column = this.colL;
                    styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                    styleFormatCondition2.Expression = "Abs([Diferencia]) > " + ES.PermisibleActaDiferencia.ToString();
                    this.bgvActa.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
                    styleFormatCondition1, styleFormatCondition2});

                    bdsDetalle.DataSource = Actas;
                    bgvActa.RefreshData();

                    DateTime FechaAnterior = (db.ActaCombustibles.Count(ac => !ListaAC.Contains(ac.ID) && ac.EstacionServicioID.Equals(IDEstacionServicio) && ac.SubEstacionID.Equals(IDSubEstacion) && ac.Movimiento.Finalizado) > 0 ?
                        db.ActaCombustibles.Where(ac => !ListaAC.Contains(ac.ID) && ac.EstacionServicioID.Equals(IDEstacionServicio) && ac.SubEstacionID.Equals(IDSubEstacion) && ac.Movimiento.Finalizado).OrderByDescending(o => o.FechaActa).First().FechaActa : Parametros.Config.FechaCierreActa());

                    var Extraxion = from vam in dv.VistaArqueoMangueras
                                    join vai in dv.VistaArqueoIslas on vam.VistaArqueoIsla equals vai
                                    where vai.EstacionServicioID.Equals(IDEstacionServicio) && vai.SubEstacionID.Equals(IDSubEstacion) && (!vai.ArqueoEspecial || (vai.ArqueoEspecial && vai.TurnoEspecial))
                                    && vai.ResumenDiaFechaInicial.Date > FechaAnterior.Date && vai.ResumenDiaFechaInicial.Date <= Fecha.Date
                                    group vam by new { vam.TanqueID, vam.ProductoID } into g
                                    select new
                                    {
                                        TanqueID = g.Key.TanqueID,
                                        ProductoID = g.Key.ProductoID,
                                        Total = g.Sum(a => (a.ExtraxionElectronica > 0 ? a.ExtraxionElectronica : (a.ExtraxionMecanica > 0 ? a.ExtraxionMecanica : 0m)))
                                        //Total = g.Sum(a => (a.ExtraxionElectronica > 0 ? a.ExtraxionElectronica : a.ExtraxionMecanica))
                                        // Total = (g.Count(s => s.EsLecturaElectronica) > 0 ? g.Sum(a => a.ExtraxionElectronica) : g.Sum(a => a.ExtraxionMecanica))
                                    };

                    Actas.ForEach(ats =>
                    {
                        //ResumenSelected = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First();

                        var Anterior = db.ActaCombustibles.Where(ac => !ListaAC.Contains(ac.ID) && ac.EstacionServicioID.Equals(0) && ac.SubEstacionID.Equals(0) && ac.ProductoID.Equals(0) && ac.TanqueID.Equals(0)).OrderByDescending(o => o.FechaActa).FirstOrDefault();

                        var Combustible = from k in db.Kardexes
                                          join m in db.Movimientos on k.Movimiento equals m
                                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                          where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(ats.ProductoID) && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                                          && (k.AlmacenEntradaID.Equals(ats.TanqueID) || k.AlmacenSalidaID.Equals(ats.TanqueID))
                                              //&& (k.Fecha.Date >= (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date)
                                          && k.Fecha.Date <= Fecha.Date
                                          select new
                                          {
                                              k.ID,
                                              k.Fecha,
                                              k.CostoFinal,
                                              k.CantidadEntrada,
                                              k.CantidadSalida
                                          };

                        var Manejo = from k in db.Kardexes
                                     join m in db.Movimientos on k.Movimiento equals m
                                     join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                     where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(ats.ProductoID) && k.EsManejo && !m.Anulado && !mt.EsAnulado
                                     && (k.AlmacenEntradaID.Equals(ats.TanqueID) || k.AlmacenSalidaID.Equals(ats.TanqueID))
                                     //&& (k.Fecha.Date >= (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) 
                                     && k.Fecha.Date <= Fecha.Date
                                     select k;

                        if (Combustible.Count() > 0)
                            ats.ExistenciaPropiedad = (Anterior != null ? Anterior.ExistenciaPropiedad : 0m) + Combustible.Sum(s => s.CantidadEntrada - s.CantidadSalida);
                        else
                            ats.ExistenciaPropiedad = 0m;

                        bgvActa_CellValueChanged(null, new CellValueChangedEventArgs(bgvActa.LocateByValue("TanqueID", ats.TanqueID), colC, ats.ExistenciaPropiedad));

                        if (Manejo.Count() > 0)
                            ats.ExistenciaManejo = (Anterior != null ? Anterior.ExistenciaManejo : 0m) + Manejo.Sum(s => s.CantidadEntrada - s.CantidadSalida);
                        else
                            ats.ExistenciaManejo = 0m;

                        bgvActa_CellValueChanged(null, new CellValueChangedEventArgs(bgvActa.LocateByValue("TanqueID", ats.TanqueID), colD, ats.ExistenciaManejo));


                        if (Inventario.InventarioCombustibleValores.Count(i => i.ProductoID.Equals(ats.ProductoID)) > 0)
                            ats.TotalExistenciaFisico = Inventario.InventarioCombustibleValores.First(i => i.ProductoID.Equals(ats.ProductoID) && i.TanqueID.Equals(ats.TanqueID)).Litros;
                        else
                            ats.TotalExistenciaFisico = 0m;

                        bgvActa_CellValueChanged(null, new CellValueChangedEventArgs(bgvActa.LocateByValue("TanqueID", ats.TanqueID), colF, ats.TotalExistenciaFisico));

                        if (Extraxion.Count(ex => ex.ProductoID.Equals(ats.ProductoID) && ex.TanqueID.Equals(ats.TanqueID)) > 0)
                            ats.ExtraccionesTotalesArqueo = Convert.ToDecimal(Extraxion.First(i => i.ProductoID.Equals(ats.ProductoID) && i.TanqueID.Equals(ats.TanqueID)).Total);
                        else
                            ats.ExtraccionesTotalesArqueo = 0m;

                        if (Combustible.Count() > 0)
                        {
                            ats.CostoPromedio = Combustible.OrderByDescending(o => o.Fecha).First().CostoFinal;
                        }
                        else
                            ats.CostoPromedio = 0m;

                    });


                    Actas.ForEach(ats =>
                    {
                        if (Inventario.InventarioCombustibleValores.Count(i => i.ProductoID.Equals(ats.ProductoID)) > 0)
                            ats.TotalExistenciaFisico = Inventario.InventarioCombustibleValores.First(i => i.ProductoID.Equals(ats.ProductoID) && i.TanqueID.Equals(ats.TanqueID)).Litros;
                        else
                            ats.TotalExistenciaFisico = 0m;

                        bgvActa_CellValueChanged(null, new CellValueChangedEventArgs(bgvActa.LocateByValue("TanqueID", ats.TanqueID), colF, ats.TotalExistenciaFisico));

                    });
                    bgvActa.RefreshData();

                    this.btnOK.Enabled = true;

                    if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "bntFinalizarActaCombustible"))
                    {
                        this.btnRecost.Visible = true;

                        if (ShowFinish)
                            this.bntFinalizar.Enabled = true;
                    }

                    //this.dateFecha.Enabled = false;
                    this.lkInventario.Enabled = false;
                    bgvActa.OptionsBehavior.Editable = true;
                    this.txtNumero.Properties.ReadOnly = true;
                    this.lkInventario.Properties.ReadOnly = true;
                    this.chkBloquearCombustible.Checked = true;
                    this.chkInventarioFinalizado.Checked = true;
                    this.btnLoad.Enabled = false;
                   
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());

                    if (Editable)
                        this.BeginInvoke(new MethodInvoker(this.Close));

                }
            }
            else
            {
                if (Editable)
                    this.BeginInvoke(new MethodInvoker(this.Close));
            }
        }

        private void bgvActa_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< CALCULOS_MONTOS >>>
            try
            {
                //--  CALCULO (E)
                if (e.Column == colC || e.Column == colD || e.Column == colF || e.Column == colH || e.Column == colJ)
                {
                    decimal vC = 0, vD = 0, vE = 0, vF = 0, vG = 0, vH = 0, vI = 0, vJ = 0, vCU = 0;

                    if (bgvActa.GetRowCellValue(e.RowHandle, "ExistenciaPropiedad") != null)
                        vC = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "ExistenciaPropiedad"));

                    if (bgvActa.GetRowCellValue(e.RowHandle, "ExistenciaManejo") != null)
                        vD = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "ExistenciaManejo"));

                    bgvActa.SetRowCellValue(e.RowHandle, "TotalExistencia", Decimal.Round(vC + vD, 3, MidpointRounding.AwayFromZero));
                
                //--  CALCULO (G)
                    if (bgvActa.GetRowCellValue(e.RowHandle, "TotalExistencia") != null)
                        vE = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "TotalExistencia"));

                    if (bgvActa.GetRowCellValue(e.RowHandle, "TotalExistenciaFisico") != null)
                        vF = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "TotalExistenciaFisico"));

                    bgvActa.SetRowCellValue(e.RowHandle, "Variaciones", Decimal.Round(vF - vE, 3, MidpointRounding.AwayFromZero));
                    
                //--  CALCULO (I)
                    if (bgvActa.GetRowCellValue(e.RowHandle, "Variaciones") != null)
                        vG = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "Variaciones"));

                    if (bgvActa.GetRowCellValue(e.RowHandle, "ExtraccionesTotalesArqueo") != null)
                        vH = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "ExtraccionesTotalesArqueo"));

                    bgvActa.SetRowCellValue(e.RowHandle, "PorcentajesVariaciones", Decimal.Round(((vH.Equals(0m) ? (vG / (vF.Equals(0m) ? 1 : vF)) : vG / vH) * 100m), 3, MidpointRounding.AwayFromZero));
                    
                    //--  CALCULO (K,L)
                    if (bgvActa.GetRowCellValue(e.RowHandle, "PorcentajesVariaciones") != null)
                        vI = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "PorcentajesVariaciones"));

                    if (bgvActa.GetRowCellValue(e.RowHandle, "VariacionesCDI") != null)
                        vJ = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "VariacionesCDI"));

                    bgvActa.SetRowCellValue(e.RowHandle, "PorcentajesVariacionesSecundario", Decimal.Round((vJ / (vH.Equals(0m) ? 1 : vH)), 3, MidpointRounding.AwayFromZero));

                    bgvActa.SetRowCellValue(e.RowHandle, "Diferencia", Decimal.Round(vG - vJ, 3, MidpointRounding.AwayFromZero));

                    //--  CALCULO (CT)
                    if (bgvActa.GetRowCellValue(e.RowHandle, "CostoPromedio") != null)
                        vCU = Convert.ToDecimal(bgvActa.GetRowCellValue(e.RowHandle, "CostoPromedio"));

                    bgvActa.SetRowCellValue(e.RowHandle, "CostoTotal", Decimal.Round((vG * vCU) * -1, 2, MidpointRounding.AwayFromZero));

                }

                bgvActa.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            #endregion
                       
        }

        private void bntNew_Click(object sender, EventArgs e)
        {
            Next = true;
            ShowMsg = false;
            this.Close();
        }
        
        private void bgvDataLecturas_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column != colDatos && e.Column != colIDDatos && e.Column != colColor)
            {
                List<String> lista = new List<string>();
                int ID = Convert.ToInt32(bgvDataLecturas.GetRowCellValue(e.RowHandle, colIDDatos));

                DataRow[] ESRow = dtInventario.Select("ID = " + ID);
                        DataRow row = ESRow.FirstOrDefault();

                if (row != null)
                {
                 foreach (DataColumn col in dtInventario.Columns)
                 {
                     if (col.DataType == typeof(String))
                     {
                         lista.Add(Convert.ToString(row[col]));
                     }
                 }
                }

                if (lista.Count(l => l.Equals(Convert.ToString(e.CellValue))) > 1)
                {
                    e.Appearance.ForeColor = Color.DarkMagenta;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
            }
        }

        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.ValidateTipoCambio(dateFecha, errRequiredField, db))
                {
                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Acta de Combustible";
                        nf.ShowDialog();
                    }
                }
                else
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                                 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

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

        #endregion

        private void btnRecost_Click(object sender, EventArgs e)
        {
            //PARA AGILIZAR EL PROCESO DE RECOSTEO
            //Next = false;
            //_IDPrint = 0;
            //REditID = (EntidadAnterior != null ? EntidadAnterior.ID : 0);
            //this.Close();

            //Ultima Acta
            var vLastActa = (from ac in db.ActaCombustibles
                             join m in db.Movimientos on ac.MovimientoID equals m.ID
                             where ac.EstacionServicioID.Equals(IDEstacionServicio) && ac.SubEstacionID.Equals(IDSubEstacion) && m.Finalizado
                             group ac by new { m.ID, ac.FechaActa } into gr
                             select gr).OrderByDescending(o => o.Key.FechaActa).FirstOrDefault();

            if (vLastActa != null)
            {
                if (ValidarRecosteo(vLastActa.Key.FechaActa.AddMonths(1)))
                {
                    List<Parametros.ListIdDisplay> P = Actas.GroupBy(g => new { ID = g.ProductoID, Display = g.Codigo + " | " + g.Nombre }).Select(s => new Parametros.ListIdDisplay { ID = s.Key.ID, Display = s.Key.Display }).ToList();

                    if (Parametros.General.DialogMsg("Se realizará el proceso de Recostear para " + P.Count + " Combustibles esto tomara varios minutos." + Environment.NewLine + "¿Desea Proceder?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.CommandTimeout = 45000;
                        bool ReOpen = false;

                        //Contador
                        int x = 1;
                        foreach (var vP in P)
                        {
                            try
                            {
                                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), x.ToString() + "/" + P.Count.ToString() + "...Recosteando " + vP.Display);
                                db.SetRecosteoByProducto(vP.ID, IDEstacionServicio, IDSubEstacion, vLastActa.Key.FechaActa.AddDays(1).Date, Fecha, UsuarioID);
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            }
                            catch (Exception ex)
                            {
                                ReOpen = false;
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                            }
                            x++;
                        }

                        if (!ReOpen)
                        {
                            Next = false;
                            _IDPrint = 0;
                            REditID = (EntidadAnterior != null ? EntidadAnterior.ID : 0);
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                if (ValidarRecosteo(Fecha.Date))
                {

                    List<Parametros.ListIdDisplay> P = Actas.GroupBy(g => new { ID = g.ProductoID, Display = g.Codigo + " | " + g.Nombre }).Select(s => new Parametros.ListIdDisplay { ID = s.Key.ID, Display = s.Key.Display }).ToList();

                    if (Parametros.General.DialogMsg("Se realizará el proceso de Recostear para " + P.Count + " Combustibles esto tomara varios minutos." + Environment.NewLine + "¿Desea Proceder?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        db.CommandTimeout = 3600;
                        bool ReOpen = false;

                        //Contador
                        int x = 1;
                        foreach (var vP in P)
                        {
                            try
                            {
                                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), x.ToString() + "/" + P.Count.ToString() + "...Recosteando " + vP.Display);
                                DateTime vFecha = new DateTime(Fecha.Year, Fecha.Month, 1);
                                db.SetRecosteoByProducto(vP.ID, IDEstacionServicio, IDSubEstacion, vFecha.Date, Fecha, UsuarioID);
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            }
                            catch (Exception ex)
                            {
                                ReOpen = false;
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                            }
                            x++;
                        }

                        if (!ReOpen)
                        {
                            Next = false;
                            _IDPrint = 0;
                            REditID = (EntidadAnterior != null ? EntidadAnterior.ID : 0);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void dateFecha_EditValueChanged(object sender, EventArgs e)
        {
            if (contadorReLoad == 1)
            {
                contadorReLoad += 1;
            }
            else
            {
                this.btnLoad.Enabled = true;
            }
        }
        

    }
}