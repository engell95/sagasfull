using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SAGAS.Inventario.Forms
{
    public partial class FormRecosteo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private DataTable dtProductos;
        private int Usuario = Parametros.General.UserID;
        internal int _ProductoClaseCombustible;
        internal List<Entidad.EstacionServicio> EtEstaciones;
        internal List<Entidad.SubEstacion> EtSubEstaciones;
        internal List<Entidad.Tanque> EtTanques;
        internal List<Entidad.Almacen> EtAlmacenes;
        internal List<Entidad.Area> EtAreas;
        internal List<Entidad.ProductoClase> EtClases;
        internal List<Entidad.SetRecalculo> PreCalculo, Postcalculo;
        internal List<Entidad.Movimiento> EtMovmimiento;
        private BackgroundWorker bgw;

        private int IDEstacion
        {
            get { return Convert.ToInt32(lkES.EditValue); }
            set { lkES.EditValue = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSUS.EditValue); }
            set { lkSUS.EditValue = value; }
        }

        private int IDArea
        {
            get { return Convert.ToInt32(lkArea.EditValue); }
            set { lkArea.EditValue = value; }
        }

        private int IDClase
        {
            get { return Convert.ToInt32(lkClase.EditValue); }
            set { lkClase.EditValue = value; }
        }

        #endregion

        #region *** INICIO ***

        public FormRecosteo()
        {
            InitializeComponent();            
            FillControl();
        }

        #endregion

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                //Valores
                _ProductoClaseCombustible = Parametros.Config.ProductoClaseCombustible();
                chkAplicaPeriodoCierre.Checked = false;

                //if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "chkAplicaPeriodoCierre"))
                //    layoutControlItemChkMesesCerrados.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                //Listas
                EtEstaciones = db.EstacionServicios.Where(es => es.Activo).ToList();
                EtSubEstaciones = db.SubEstacions.Where(ses => ses.Activo).ToList();
                EtTanques = db.Tanques.Where(t => t.Activo).ToList();
                EtAlmacenes = db.Almacens.Where(a => a.Activo).ToList();
                EtAreas = db.Areas.Where(ar => ar.Activo).ToList();
                EtClases = db.ProductoClases.Where(c => c.Activo).ToList();

                //LOOKUPS
                lkES.Properties.DataSource = EtEstaciones;
                IDEstacion = Parametros.General.EstacionServicioID;

                var A = EtAreas.Select(s => new {s.ID, s.Nombre}).ToList();
                A.Insert(0, new { ID = 0, Nombre = "TODAS" });
                lkArea.Properties.DataSource = A;
                IDArea = 0;

                //var C = EtClases.Select(s => new { s.ID, s.Nombre }).ToList();
                //C.Insert(0, new { ID = 0, Nombre = "TODAS" });
                //lkClase.Properties.DataSource = C;
                //IDClase = 0;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        public bool ValidarCampos()
        {
            if (lkES.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Estación de Servicio", Parametros.MsgType.warning);
                return false;
            }

            if (layoutControlItemSes.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
            {
                if (lkSUS.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar la Sub Estación de Servicio", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (PreCalculo != null)
            {

                if (PreCalculo.Count <= 0)
                {
                    Parametros.General.DialogMsg("No existen Productos para ser recalculados", Parametros.MsgType.warning);
                    return false;

                }
            }
            else
            {
                Parametros.General.DialogMsg("Primeramente debe de cargar los productos", Parametros.MsgType.warning);
                return false;
            }

            if (PreCalculo.Count(c => c.Selected) <= 0)
            {
                Parametros.General.DialogMsg("Debe seleccionar al menos un Producto para ser recalculado", Parametros.MsgType.warning);
                return false;
            }

            

            if (dateInicial.EditValue == null || dateFinal.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la fecha del periodo a recalcular.", Parametros.MsgType.warning);
                return false;
            }

            if (Convert.ToDateTime(dateInicial.EditValue) > Convert.ToDateTime(dateFinal.EditValue))
            {
                Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
                return false;
            }

            
                if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(dateInicial.EditValue).Date, db, IDEstacion))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }
            

            if (lkArea.EditValue == null || lkClase.EditValue == null)
            {
                Parametros.General.DialogMsg("Favor revisar el Área o Clase de Productos.", Parametros.MsgType.warning);
                return false;
            }
            
            return true;
        }

        private void Recalcular()
        {
           

            Postcalculo = new List<Entidad.SetRecalculo>();
            var Lista = PreCalculo.Where(c => c.Selected).ToList();
            int Under = Lista.Count;
            int Top = 0;
            GRestante.Text = Top.ToString() + "/" + Under.ToString();

            foreach (Entidad.SetRecalculo obj in Lista)
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

                using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
                {
                    db.Transaction = trans;
                    db.CommandTimeout = 6000;
                    //decimal vCotso = 0;
                    //bool vFin = false;
                    try
                    {
                        Top ++;
                        progressBarControlK.EditValue = 0;
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                        var lista = (from m in db.Movimientos
                                     join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                     join k in db.Kardexes on m.ID equals k.MovimientoID
                                     where m.EstacionServicioID.Equals(IDEstacion)
                                     && m.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(obj.ID)
                                     && !k.EsManejo && !m.Anulado && !mt.EsAnulado && (k.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date && k.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date)
                                     select new { m.ID, m.FechaRegistro, TipoID = mt.ID, mt.Entrada, mt.Nombre }).OrderBy(o => o.FechaRegistro.Date).ThenBy(t => !t.Entrada).ToList();
                                                    
                            
                            //(from m in db.Movimientos
                            //         join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                            //         join k in db.Kardexes on m.ID equals k.MovimientoID
                            //         where m.EstacionServicioID.Equals(8)
                            //         && m.SubEstacionID.Equals(3) && k.ProductoID.Equals(927)
                            //         && !k.EsManejo && !m.Anulado && !mt.EsAnulado
                            //         select new { m.ID, m.FechaRegistro, TipoID = mt.ID, mt.Entrada, mt.Nombre }).OrderBy(o => o.FechaRegistro.Date).ThenBy(t => t.Entrada).ToList();

                        EtMovmimiento = new List<Entidad.Movimiento>();

                        lista.GroupBy(g => g.ID).ToList().ForEach(l => EtMovmimiento.Add(db.Movimientos.Single(m => m.ID.Equals(l.Key))));
                        //EtMovmimiento = (from m in db.Movimientos.ToList()
                        //                 join l in lista on m.ID equals l.ID
                        //                 select m).ToList();

                        var KCIList = (from k in db.Kardexes
                                       join m in db.Movimientos on k.MovimientoID equals m.ID
                                       where k.ProductoID.Equals(obj.ID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                     && k.EstacionServicioID.Equals(IDEstacion) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                                     && k.Fecha.Date <= Convert.ToDateTime(dateInicial.EditValue).Date
                                       select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).FirstOrDefault();

                        List<int> dobleCosteo = db.MovimientoTipos.Where(o => o.DobleCosteo).Select(s => s.ID).ToList();

                        decimal vCosto = 0m;
                        if (KCIList != null)
                            vCosto = Decimal.Round(KCIList.CostoFinal, 4, MidpointRounding.AwayFromZero);

                        
                        progressBarControlK.Properties.Maximum = EtMovmimiento.Count;
                        EtMovmimiento.ForEach(det =>
                        {
                            progressBarControlK.PerformStep();
                            progressBarControlK.Update();

                            var periodo = db.PeriodoContables.FirstOrDefault(p => p.EstacionID.Equals(det.EstacionServicioID) && (p.FechaInicio.Date <= det.FechaRegistro && p.FechaFin.Date >= det.FechaRegistro));

                            bool MesCerrado = false;

                            if (periodo != null)
                                MesCerrado = periodo.Cerrado;
                            
                            if (lista.First(l => l.ID.Equals(det.ID)).Entrada)
                            {                               

                                #region <<< ENTRADA DE INVENTARIO >>>                                
                                //det.Kardexes.Where(k => k.ProductoID.Equals(927)).ToList().ForEach(detK => detK.CantidadSalida = 666);
                                if (det.MovimientoTipoID.Equals(31) || det.MovimientoTipoID.Equals(37))
                                {
                                    #region Primer proceso
                                    if (!MesCerrado)
                                    {
                                        det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID) && !k.EsManejo).ToList().ForEach(detK =>
                                        {
                                            detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                            detK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                            detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);

                                            var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaInventarioID;

                                            Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                            var Anterior = (from k in db.Kardexes
                                                            join p in db.Productos on k.ProductoID equals p.ID
                                                            where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && !k.EsManejo
                                                            select new { k.ID, k.CostoTotal }).ToList();

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                            if (NCC != null)
                                            {

                                                if (NCC.Monto < 0)
                                                    NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else if (NCC.Monto > 0)
                                                    NCC.Monto = Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else
                                                    NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                db.SubmitChanges();

                                                List<int> cuentas = db.Productos.Where(p => p.Activo).Select(s => s.CuentaInventarioID).ToList();

                                                Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && !cuentas.Contains(o.CuentaContableID));

                                                if (NCCDif != null)
                                                {
                                                    if (NCC.Monto < 0)
                                                    {
                                                        NCCDif.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCCDif.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                    }
                                                    else if (NCC.Monto >= 0)
                                                    {
                                                        NCCDif.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCCDif.MontoMonedaSecundaria = -Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                    }
                                                    db.SubmitChanges();
                                                }
                                            }

                                        });
                                    }
#endregion
                                }
                                else
                                {
                                    //Costo Inicial es vCosto
                                    decimal vCantidades = 0m;
                                    //Cantidad Inicial para el calculo
                                    vCantidades = Parametros.General.SaldoKardex(db, IDEstacion, IDSubEstacion, obj.ID, 0, det.FechaRegistro.Date, true);
                                    //Costo Total Inicial para el calculo
                                    //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);
                                    int idKD = 0;

                                    var KD = det.Kardexes.FirstOrDefault(k => k.ProductoID.Equals(obj.ID));

                                    if (KD != null)
                                        idKD = KD.ID;

                                    decimal vSaldoAct = 0m;
                                    Parametros.General.SaldoCostoTotalKardex(db, 0, idKD, IDEstacion, IDSubEstacion, obj.ID, det.FechaRegistro.Date, out vSaldoAct);
                                    
                                    vCosto = Math.Abs(Decimal.Round(Convert.ToDecimal(vSaldoAct / (!vCantidades.Equals(0) ? vCantidades : 1m)), 4, MidpointRounding.AwayFromZero));

                                    det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID)).ToList().ForEach(detK => detK.CostoFinal = vCosto);
                                }

                                #endregion
                            }
                            else if (!lista.First(l => l.ID.Equals(det.ID)).Entrada)
                            {

                                if (dobleCosteo.Contains(det.MovimientoTipoID))
                                {
                                    #region  <<< Movimientos Doble Recosteo >>>

                                    if (det.MovimientoTipoID.Equals(33) || det.MovimientoTipoID.Equals(35))
                                    {
                                        if (!MesCerrado)
                                        {
                                            det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID) && !k.EsManejo).ToList().ForEach(detK =>
                                            {
                                                detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                                detK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadSalida, 2, MidpointRounding.AwayFromZero);

                                                var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaInventarioID;

                                                Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                                var Anterior = (from k in db.Kardexes
                                                                join p in db.Productos on k.ProductoID equals p.ID
                                                                where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && !k.EsManejo
                                                                select new { k.ID, k.CostoTotal }).ToList();

                                                decimal vSuma = 0m;

                                                if (Anterior.Count() > 0)
                                                    vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                if (NCC != null)
                                                {

                                                    if (NCC.Monto < 0)
                                                        NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else if (NCC.Monto > 0)
                                                        NCC.Monto = Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    else
                                                        NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                    NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                    db.SubmitChanges();

                                                    List<int> cuentas = db.Productos.Where(p => p.Activo).Select(s => s.CuentaInventarioID).ToList();

                                                    Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && !cuentas.Contains(o.CuentaContableID));

                                                    if (NCCDif != null)
                                                    {
                                                        if (NCC.Monto < 0)
                                                        {
                                                            NCCDif.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            NCCDif.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        }
                                                        else if (NCC.Monto >= 0)
                                                        {
                                                            NCCDif.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            NCCDif.MontoMonedaSecundaria = -Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        }
                                                        db.SubmitChanges();
                                                    }
                                                }

                                            });
                                        }
                                    }
                                    else if (det.MovimientoTipoID.Equals(11))
                                    {
                                        if (!MesCerrado)
                                        {
                                            #region <<< Recibo de Mercaderia >>>
                                            det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID) && !k.EsManejo).ToList().ForEach(detK =>
                                                {
                                                    detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                                    detK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                    detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                                });
                                        }
                                        #endregion
                                    }
                                    else if (det.MovimientoTipoID.Equals(12))
                                    {
                                        #region <<< DEVOLUCION  >>>

                                        //Costo Inicial es vCosto
                                        decimal vCantidades = 0m;
                                        //Cantidad Inicial para el calculo
                                        vCantidades = Parametros.General.SaldoKardex(db, IDEstacion, IDSubEstacion, obj.ID, 0, det.FechaRegistro.Date, true);
                                        //Costo Total Inicial para el calculo
                                        //decimal vCostosInicialesRecalculo = Decimal.Round(Convert.ToDecimal(vCantidadInicial * vCosto), 4, MidpointRounding.AwayFromZero);

                                        int idKD = 0;

                                        var KD = det.Kardexes.FirstOrDefault(k => k.ProductoID.Equals(obj.ID));

                                        if (KD != null)
                                            idKD = KD.ID;

                                        decimal vSaldoAct = 0m;
                                        Parametros.General.SaldoCostoTotalKardex(db, idKD, 0, IDEstacion, IDSubEstacion, obj.ID, det.FechaRegistro.Date, out vSaldoAct);

                                        vCosto = Math.Abs(Decimal.Round(Convert.ToDecimal(vSaldoAct / (!vCantidades.Equals(0) ? vCantidades : 1m)), 4, MidpointRounding.AwayFromZero));

                                        det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID)).ToList().ForEach(detK => detK.CostoFinal = vCosto);

                                        #endregion
                                    }
                                    else
                                    {
                                        List<int> CRepetidos = new List<int>();
                                        List<int> CDifRepetidos = new List<int>();
                                        #region <<< RECOSTEO DOBLE >>>
                                        if (!MesCerrado)
                                        {
                                            det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID) && !k.EsManejo).ToList().ForEach(detK =>
                                            {
                                                detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                                if (detK.CantidadEntrada > 0)
                                                {
                                                    detK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                    detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                                    db.SubmitChanges();
                                                }
                                                else if (detK.CantidadSalida > 0)
                                                {
                                                    detK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                    detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                                    db.SubmitChanges();
                                                }

                                                var PClase = (from pc in db.ProductoClases
                                                              join p in db.Productos on pc.ID equals p.ProductoClaseID
                                                              where p.ID.Equals(detK.ProductoID)
                                                              select pc).ToList();


                                                var area = (from a in db.Areas
                                                            where PClase.Select(p => p.AreaID).Contains(a.ID)
                                                            select a).ToList();

                                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                                {
                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                                    int clase = 0;

                                                    if (PClase.Count > 0)
                                                        clase = PClase.First().ID;

                                                    int AreaID, AreaInventario = 0;

                                                    if (area.Count > 0)
                                                    {
                                                        AreaID = area.First().ID;
                                                        AreaInventario = area.First().CuentaInventarioID;
                                                    }

                                                    area.AddRange(db.Areas.Where(a => !a.ID.Equals(a.ID) && a.CuentaCostoID.Equals(AreaInventario)));
                                                    PClase.AddRange(db.ProductoClases.Where(p => !p.ID.Equals(clase) && area.Select(a => a.ID).Contains(p.AreaID)));

                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && PClase.Select(s => s.ID).Contains(p.ProductoClaseID)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    //if (NCC != null)
                                                    //{
                                                    //        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                    //        NCC.MontoMonedaSecundaria = -Math.Abs(Decimal.Round(NCC.Monto * NCC.TipoCambio, 4, MidpointRounding.AwayFromZero));
                                                    //    db.SubmitChanges();

                                                    //    Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && !o.CuentaContableID.Equals(NCC.CuentaContableID));

                                                    //    if (NCCDif != null)
                                                    //    {
                                                    //        NCCDif.Monto = Math.Abs(Decimal.Round(NCC.Monto, 2, MidpointRounding.AwayFromZero));
                                                    //        NCCDif.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCCDif.Monto * NCCDif.TipoCambio, 4, MidpointRounding.AwayFromZero));
                                                    //        db.SubmitChanges();
                                                    //    }
                                                    //}

                                                    if (NCC != null)
                                                    {
                                                        if (NCC.Monto < 0)
                                                            NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        else if (NCC.Monto > 0)
                                                            NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        else
                                                            NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                        NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);

                                                        db.SubmitChanges();

                                                        if (area.Count(a => !a.CuentaSobranteID.Equals(0)) > 0 && db.ComprobanteContables.Count(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.Count(a => !a.CuentaSobranteID.Equals(0)) > 0 ? area.First().CuentaSobranteID : 0)) > 0)
                                                        {
                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaSobranteID));

                                                            if (NCCDif != null)
                                                            {
                                                                NCCDif.Monto = Decimal.Round(NCC.Monto * -1, 2, MidpointRounding.AwayFromZero);
                                                                NCCDif.MontoMonedaSecundaria = Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                                db.SubmitChanges();
                                                            }
                                                        }
                                                        else if (area.Count(a => !a.CuentaFaltanteID.Equals(0)) > 0 && db.ComprobanteContables.Count(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.Count(a => !a.CuentaSobranteID.Equals(0)) > 0 ? area.First().CuentaFaltanteID : 0)) > 0)
                                                        {
                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaFaltanteID));

                                                            if (NCCDif != null)
                                                            {
                                                                NCCDif.Monto = Decimal.Round(NCC.Monto * -1, 2, MidpointRounding.AwayFromZero);
                                                                NCCDif.MontoMonedaSecundaria = Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                                db.SubmitChanges();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && !o.CuentaContableID.Equals(NCC.CuentaContableID));

                                                            if (NCCDif != null)
                                                            {
                                                                NCCDif.Monto = Math.Abs(Decimal.Round(NCC.Monto, 2, MidpointRounding.AwayFromZero));
                                                                NCCDif.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                                db.SubmitChanges();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaInventarioID;
                                                    var ProductoSobrante = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaSobranteID;
                                                    var ProductoFaltante = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaFaltanteID;

                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta) && !CRepetidos.Contains(o.ID));

                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && p.CuentaInventarioID.Equals(ProductoVenta)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    if (NCC != null)
                                                    {
                                                        CRepetidos.Add(NCC.ID);

                                                        if (NCC.Monto < 0)
                                                            NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        else if (NCC.Monto > 0)
                                                            NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        else
                                                            NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                        NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                        db.SubmitChanges();

                                                        if (db.ComprobanteContables.Count(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoSobrante)) > 0)
                                                        {
                                                            #region <<< ACTA SOBRANTE Ingreso Combustible  >>>

                                                            if (NCC.Monto < 0)
                                                                NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            else if (NCC.Monto > 0)
                                                                NCC.Monto = Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            else
                                                                NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                            NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                            db.SubmitChanges();

                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoSobrante) && !CDifRepetidos.Contains(o.ID));

                                                            if (NCCDif != null)
                                                            {
                                                                CDifRepetidos.Add(NCCDif.ID);
                                                                NCCDif.Monto = Decimal.Round(NCC.Monto * -1, 2, MidpointRounding.AwayFromZero);
                                                                NCCDif.MontoMonedaSecundaria = Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                                db.SubmitChanges();
                                                            }
                                                            #endregion
                                                        }
                                                        else if (db.ComprobanteContables.Count(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoFaltante)) > 0)
                                                        {
                                                            #region <<< ACTA FALTANTE Gasto / Salida Combustible  >>>
                                                            if (NCC.Monto < 0)
                                                                NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            else if (NCC.Monto > 0)
                                                                NCC.Monto = Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                            else
                                                                NCC.Monto = -Math.Abs(Decimal.Round(detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                            NCC.MontoMonedaSecundaria = Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                            db.SubmitChanges();

                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoFaltante) && !CDifRepetidos.Contains(o.ID));

                                                            if (NCCDif != null)
                                                            {
                                                                CDifRepetidos.Add(NCCDif.ID);
                                                                NCCDif.Monto = Decimal.Round(NCC.Monto * -1, 2, MidpointRounding.AwayFromZero);
                                                                NCCDif.MontoMonedaSecundaria = Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero);
                                                                db.SubmitChanges();
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region <<< MOVIMIENTO DE INVENTARIO  >>>
                                                            Entidad.ComprobanteContable NCCDif = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && !o.CuentaContableID.Equals(NCC.CuentaContableID));

                                                            if (NCCDif != null)
                                                            {
                                                                NCCDif.Monto = Math.Abs(Decimal.Round(NCC.Monto, 2, MidpointRounding.AwayFromZero));
                                                                NCCDif.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCCDif.Monto / (NCCDif.TipoCambio > 0 ? NCCDif.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                                db.SubmitChanges();
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                            });
                                        }

                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region <<< RECOSTEO SIMPLE >>>
                                    if (!MesCerrado)
                                    {
                                        det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID) && !k.EsManejo).ToList().ForEach(detK =>
                                            {

                                                detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);

                                                detK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                                detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadSalida, 2, MidpointRounding.AwayFromZero);

                                                var PClase = (from pc in db.ProductoClases
                                                              join p in db.Productos on pc.ID equals p.ProductoClaseID
                                                              where p.ID.Equals(detK.ProductoID)
                                                              select pc).ToList();


                                                var area = (from a in db.Areas
                                                            where PClase.Select(p => p.AreaID).Contains(a.ID)
                                                            select a).ToList();

                                                if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                                {
                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                                    int clase = 0;

                                                    if (PClase.Count > 0)
                                                        clase = PClase.First().ID;

                                                    int AreaID, AreaCosto = 0;

                                                    if (area.Count > 0)
                                                    {
                                                        AreaID = area.First().ID;
                                                        AreaCosto = area.First().CuentaCostoID;
                                                    }

                                                    area.AddRange(db.Areas.Where(a => !a.ID.Equals(a.ID) && a.CuentaCostoID.Equals(AreaCosto)));
                                                    PClase.AddRange(db.ProductoClases.Where(p => !p.ID.Equals(clase) && area.Select(a => a.ID).Contains(p.AreaID)));


                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    //join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && PClase.Select(s => s.ID).Contains(p.ProductoClaseID)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    if (NCC != null)
                                                    {
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCC.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        db.SubmitChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaCostoID;

                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && p.CuentaCostoID.Equals(ProductoVenta)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    if (NCC != null)
                                                    {
                                                        NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCC.MontoMonedaSecundaria = Math.Abs(Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        db.SubmitChanges();
                                                    }
                                                }

                                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                                {
                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));

                                                    int clase = 0;

                                                    if (PClase.Count > 0)
                                                        clase = PClase.First().ID;

                                                    int AreaID, AreaInventario = 0;

                                                    if (area.Count > 0)
                                                    {
                                                        AreaID = area.First().ID;
                                                        AreaInventario = area.First().CuentaInventarioID;
                                                    }

                                                    area.AddRange(db.Areas.Where(a => !a.ID.Equals(a.ID) && a.CuentaCostoID.Equals(AreaInventario)));
                                                    PClase.AddRange(db.ProductoClases.Where(p => !p.ID.Equals(clase) && area.Select(a => a.ID).Contains(p.AreaID)));

                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && PClase.Select(s => s.ID).Contains(p.ProductoClaseID)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    if (NCC != null)
                                                    {
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCC.MontoMonedaSecundaria = -Math.Abs(Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        db.SubmitChanges();
                                                    }
                                                }
                                                else
                                                {
                                                    var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaInventarioID;

                                                    Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                                    var Anterior = (from k in db.Kardexes
                                                                    join p in db.Productos on k.ProductoID equals p.ID
                                                                    where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && p.CuentaInventarioID.Equals(ProductoVenta)
                                                                    select new { k.ID, k.CostoTotal }).ToList();

                                                    decimal vSuma = 0m;

                                                    if (Anterior.Count() > 0)
                                                        vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                                    if (NCC != null)
                                                    {
                                                        NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                        NCC.MontoMonedaSecundaria = -Math.Abs(Decimal.Round(NCC.Monto / (NCC.TipoCambio > 0 ? NCC.TipoCambio : 1m), 4, MidpointRounding.AwayFromZero));
                                                        db.SubmitChanges();
                                                    }
                                                }
                                            });
                                    }
                                    #endregion
                                }
                            }

                            db.SubmitChanges();
                        });

                        progressBarControlK.EditValue = 0;
                        //Postcalculo = new List<Entidad.SetRecalculo>();
                        db.SubmitChanges();
                        obj.Finish = true;
                        obj.Costo = vCosto;
                        trans.Commit();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
                    }
                    finally
                    {
                        Postcalculo.Add(obj);
                        GRestante.Text = Top.ToString() + "/" + Under.ToString();
                        if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                    }
                }
            }

            bdsPostCalculo.DataSource = null;
            bdsPostCalculo.DataSource = Postcalculo;
            gvDataPost.RefreshData();
        }

        #endregion

        #region <<< EVENTOS >>>
        private void btnCargarLista_Click(object sender, EventArgs e)
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                gvDataPre.ActiveFilter.Clear();
                PreCalculo = db.SetRecalculo.Where(v => (v.AreaID.Equals(IDArea) || IDArea.Equals(0)) && (v.ClaseID.Equals(IDClase) || IDClase.Equals(0))).ToList();
                bdsPreView.DataSource = null;
                bdsPreView.DataSource = PreCalculo;
                gvDataPre.RefreshData();

                var T = EtAlmacenes.Where(a => a.EstacionServicioID.Equals(IDEstacion) && a.SubEstacionID.Equals(IDSubEstacion)).Select(s => new {s.ID, s.Nombre}).ToList();

                if (PreCalculo.Count(c => !c.ClaseID.Equals(_ProductoClaseCombustible)).Equals(0))
                    T.Clear();

                if (PreCalculo.Select(s => s.ClaseID).Contains(_ProductoClaseCombustible)) 
                    T.AddRange(EtTanques.Where(a => a.EstacionServicioID.Equals(IDEstacion) && a.SubEstacionID.Equals(IDSubEstacion)).Select(s => new { s.ID, s.Nombre }).ToList());
                
                gridAlmacenes.DataSource = null;
                gridAlmacenes.DataSource = T;
                gvDataAlmacenes.RefreshData();
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }
        
        private void lkES_EditValueChanged(object sender, EventArgs e)
        {
            if (lkES.EditValue != null)
            {
                if (IDEstacion > 0)
                {
                    var Sus = EtSubEstaciones.Where(sus => sus.EstacionServicioID.Equals(IDEstacion)).ToList();

                    if (Sus.Count > 0)
                    {
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lkSUS.Properties.DataSource = Sus;
                        lkSUS.EditValue = null;
                    }
                    else
                    {
                        this.lkSUS.EditValue = null;
                        this.lkSUS.Properties.DataSource = null;
                        this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    }
                }
            }
            else
            {
                this.lkSUS.EditValue = null;
                this.lkSUS.Properties.DataSource = null;
                this.layoutControlItemSes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (PreCalculo.Count > 0)
                    PreCalculo.ForEach(p => p.Selected = true);

                gvDataPre.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnUnselect_Click(object sender, EventArgs e)
        {
            try
            {
                if (PreCalculo.Count > 0)
                    PreCalculo.ForEach(p => p.Selected = false);

                gvDataPre.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
                return;

            var Lista = PreCalculo.Where(c => c.Selected).ToList();

             List<Parametros.ListIdDisplay> P = (from p in db.Productos
                                                            where Lista.Select(s => s.ID).Contains(p.ID)
                                                            select new Parametros.ListIdDisplay
                                                            {
                                                                ID = p.ID,
                                                                Display = p.Codigo + " | " + p.Nombre
                                                            }).ToList();

             if (Parametros.General.DialogMsg("Se realizará el proceso de Recostear para " + P.Count + " Productos esto tomara varios minutos." + Environment.NewLine + "¿Desea Proceder?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
             {
                 db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                 db.CommandTimeout = 900;

                 progressBarControlK.EditValue = 0;
                 //Contador
                    progressBarControlK.Properties.Maximum = P.Count;
                 Postcalculo = new List<Entidad.SetRecalculo>();
                 int x = 1;
                 foreach (var vP in P)
                 {
                     progressBarControlK.PerformStep();
                     progressBarControlK.Update();
                     try
                     {
                         Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), x.ToString() + "/" + P.Count.ToString() + " Rec... " + vP.Display.Substring(0, 10).ToString());
                         db.SetRecosteoByProducto(vP.ID, IDEstacion, IDSubEstacion, Convert.ToDateTime(dateInicial.EditValue).Date, Convert.ToDateTime(dateFinal.EditValue).Date, Usuario);
                         Parametros.General.splashScreenManagerMain.CloseWaitForm();
                         
                         Entidad.SetRecalculo Obj = Lista.SingleOrDefault(s => s.ID.Equals(vP.ID));

                         if (Obj != null)
                         {
                             Obj.Finish = true;
                             Postcalculo.Add(Obj);
                         }
                         
                     }
                     catch (Exception ex)
                     {
                         Parametros.General.splashScreenManagerMain.CloseWaitForm();
                         Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                     }
                     x++;

                 }

                 bdsPostCalculo.DataSource = null;
                 bdsPostCalculo.DataSource = Postcalculo;
                 gvDataPost.RefreshData();
             }

            //gvDataPost.ActiveFilter.Clear();
            //Recalcular();
            //bgw = new BackgroundWorker();
            //bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            //bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            //bgw.RunWorkerAsync();
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            Postcalculo = new List<Entidad.SetRecalculo>();
            var Lista = PreCalculo.Where(c => c.Selected).ToList();
            int Under = Lista.Count;
            int Top = 0;
            GRestante.Text = Top.ToString() + "/" + Under.ToString();

            foreach (Entidad.SetRecalculo obj in Lista)
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

                using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
                {
                    db.Transaction = trans;
                    db.CommandTimeout = 6000;
                    try
                    {
                        Top++;
                        //progressBarControlK.EditValue = 0;
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                        var lista = (from m in db.Movimientos
                                     join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                     join k in db.Kardexes on m.ID equals k.MovimientoID
                                     where m.EstacionServicioID.Equals(IDEstacion)
                                     && m.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(obj.ID)
                                     && !k.EsManejo && !m.Anulado && !mt.EsAnulado && (k.Fecha.Date >= Convert.ToDateTime(dateInicial.EditValue).Date && k.Fecha.Date <= Convert.ToDateTime(dateFinal.EditValue).Date)
                                     select new { m.ID, m.FechaRegistro, TipoID = mt.ID, mt.Entrada, mt.Nombre }).OrderBy(o => o.FechaRegistro.Date).ThenBy(t => !t.Entrada).ToList();


                        EtMovmimiento = new List<Entidad.Movimiento>();

                        lista.GroupBy(g => g.ID).ToList().ForEach(l => EtMovmimiento.Add(db.Movimientos.Single(m => m.ID.Equals(l.Key))));
                     
                        var KCIList = (from k in db.Kardexes
                                       join m in db.Movimientos on k.MovimientoID equals m.ID
                                       where k.ProductoID.Equals(obj.ID) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43))
                                     && k.EstacionServicioID.Equals(IDEstacion) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                                     && k.Fecha.Date <= Convert.ToDateTime(dateInicial.EditValue).Date
                                       select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).FirstOrDefault();

                        List<int> dobleCosteo = db.MovimientoTipos.Where(o => o.DobleCosteo).Select(s => s.ID).ToList();

                        decimal vCosto = 0m;
                        if (KCIList != null)
                            vCosto = Decimal.Round(KCIList.CostoFinal, 4, MidpointRounding.AwayFromZero);

                        //progressBarControlK.Properties.Maximum = EtMovmimiento.Count;
                        EtMovmimiento.ForEach(det =>
                        {
                            //progressBarControlK.PerformStep();
                            //progressBarControlK.Update();
                            if (lista.First(l => l.ID.Equals(det.ID)).Entrada)
                            {
                                decimal vCantidades = 0m;
                                vCantidades = Parametros.General.SaldoKardex(db, IDEstacion, IDSubEstacion, obj.ID, 0, det.FechaRegistro.Date, true);

                                int idKD = 0;

                                var KD = det.Kardexes.FirstOrDefault(k => k.ProductoID.Equals(obj.ID));

                                if (KD != null)
                                    idKD = KD.ID;

                                decimal vSaldoAct = 0m;
                                Parametros.General.SaldoCostoTotalKardex(db, idKD, 0, IDEstacion, IDSubEstacion, obj.ID, det.FechaRegistro.Date, out vSaldoAct);

                                vCosto = Decimal.Round(Convert.ToDecimal(vSaldoAct / (vCantidades > 0 ? vCantidades : 1m)), 4, MidpointRounding.AwayFromZero);

                                det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID)).ToList().ForEach(detK => detK.CostoFinal = vCosto);

                            }
                            else if (!lista.First(l => l.ID.Equals(det.ID)).Entrada)
                            {

                                if (dobleCosteo.Contains(det.MovimientoTipoID))
                                {

                                }
                                else
                                {
                                    #region <<< RECOSTEO SIMPLE >>>
                                    det.Kardexes.Where(k => k.ProductoID.Equals(obj.ID)).ToList().ForEach(detK =>
                                    {


                                        detK.CostoFinal = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                        if (!detK.CostoEntrada.Equals(0))
                                        {
                                            detK.CostoEntrada = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                            detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                        }

                                        if (!detK.CostoSalida.Equals(0))
                                        {
                                            detK.CostoSalida = Decimal.Round(vCosto, 4, MidpointRounding.AwayFromZero);
                                            detK.CostoTotal = Decimal.Round(detK.CostoFinal * detK.CantidadSalida, 2, MidpointRounding.AwayFromZero);
                                        }

                                        var PClase = (from pc in db.ProductoClases.ToList()
                                                      join p in db.Productos on pc.ID equals p.ProductoClaseID
                                                      where p.ID.Equals(detK.ProductoID)
                                                      select pc).ToList();

                                        var area = (from a in db.Areas.ToList()
                                                    where PClase.Select(p => p.AreaID).Contains(a.ID)
                                                    select a).ToList();

                                        if (area.Count(a => !a.CuentaCostoID.Equals(0)) > 0)
                                        {
                                            Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaCostoID));

                                            var Anterior = (from k in db.Kardexes.ToList()
                                                            join p in db.Productos.ToList() on k.ProductoID equals p.ID
                                                            where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && PClase.Select(s => s.ID).Contains(p.ProductoClaseID)
                                                            select new { k.ID, k.CostoTotal }).ToList();

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                            if (NCC != null)
                                            {
                                                if (NCC.Monto < 0)
                                                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else
                                                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                db.SubmitChanges();
                                            }
                                        }
                                        else
                                        {
                                            var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaCostoID;

                                            Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                            var Anterior = (from k in db.Kardexes.ToList()
                                                            join p in db.Productos.ToList() on k.ProductoID equals p.ID
                                                            where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && p.CuentaCostoID.Equals(ProductoVenta)
                                                            select new { k.ID, k.CostoTotal }).ToList();

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                            if (NCC != null)
                                            {
                                                if (NCC.Monto < 0)
                                                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else
                                                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                db.SubmitChanges();
                                            }
                                        }

                                        if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                        {
                                            Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(area.First().CuentaInventarioID));


                                            var Anterior = (from k in db.Kardexes.ToList()
                                                            join p in db.Productos.ToList() on k.ProductoID equals p.ID
                                                            where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && PClase.Select(s => s.ID).Contains(p.ProductoClaseID)
                                                            select new { k.ID, k.CostoTotal }).ToList();

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                            if (NCC != null)
                                            {
                                                if (NCC.Monto < 0)
                                                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else
                                                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                db.SubmitChanges();
                                            }
                                        }
                                        else
                                        {
                                            var ProductoVenta = db.Productos.Single(o => o.ID.Equals(detK.ProductoID)).CuentaInventarioID;

                                            Entidad.ComprobanteContable NCC = db.ComprobanteContables.FirstOrDefault(o => o.MovimientoID.Equals(detK.MovimientoID) && o.CuentaContableID.Equals(ProductoVenta));

                                            var Anterior = (from k in db.Kardexes.ToList()
                                                            join p in db.Productos.ToList() on k.ProductoID equals p.ID
                                                            where k.MovimientoID.Equals(detK.MovimientoID) && !k.ID.Equals(detK.ID) && p.CuentaInventarioID.Equals(ProductoVenta)
                                                            select new { k.ID, k.CostoTotal }).ToList();

                                            decimal vSuma = 0m;

                                            if (Anterior.Count() > 0)
                                                vSuma = Decimal.Round(Anterior.Sum(s => s.CostoTotal), 2, MidpointRounding.AwayFromZero);

                                            if (NCC != null)
                                            {
                                                if (NCC.Monto < 0)
                                                    NCC.Monto = -Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));
                                                else
                                                    NCC.Monto = Math.Abs(Decimal.Round(vSuma + detK.CostoTotal, 2, MidpointRounding.AwayFromZero));

                                                db.SubmitChanges();
                                            }
                                        }
                                    });

                                    #endregion
                                }
                            }

                            db.SubmitChanges();
                        });

                        //progressBarControlK.EditValue = 0;
                        db.SubmitChanges();
                        obj.Finish = true;
                        obj.Costo = vCosto;
                        trans.Commit();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
                    }
                    finally
                    {
                        Postcalculo.Add(obj);
                        GRestante.Text = Top.ToString() + "/" + Under.ToString();
                        if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                    }
                }
            }
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bdsPostCalculo.DataSource = null;
            bdsPostCalculo.DataSource = Postcalculo;
            gvDataPost.RefreshData();
        }

        #endregion

        private void lkArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkArea.EditValue != null)
                {
                    if (IDArea.Equals(0))
                    {
                        lkClase.Properties.DataSource = null;
                        var C = EtClases.Select(s => new { s.ID, s.Nombre }).ToList();
                        C.Insert(0, new { ID = 0, Nombre = "TODAS" });
                        lkClase.Properties.DataSource = C;
                        IDClase = 0;
                    }
                    else
                    {
                        lkClase.Properties.DataSource = null;
                        var C = EtClases.Where(o => o.AreaID.Equals(IDArea)).Select(s => new { s.ID, s.Nombre }).ToList();
                        C.Insert(0, new { ID = 0, Nombre = "TODAS" });
                        lkClase.Properties.DataSource = C;
                        IDClase = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

       
       
    }
}