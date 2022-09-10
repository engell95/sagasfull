using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;

namespace SAGAS.Inventario.Forms.Dialogs
{
    public partial class DialogAjusteInventario : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormAjusteInventario MDI;
        internal Entidad.Movimiento EntidadAnterior;
        internal Entidad.InventarioFisico Inventario;
        internal bool Editable = false;
        internal bool ToPrint = false;
        internal bool PermitirCostoCeroAjuste = false;
        internal bool _SendPrint = false;
        internal int _ID = 0;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private DataTable dtInventario = new DataTable();
        private List<Parametros.ListIdDisplay> listadoNumero;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private List<ListaInventarios> ObjInventario = new List<ListaInventarios>();
        public int InventarioFisicoMDI = 0;
        public bool ShowFinish = false;
        private int REditID = 0;

        private struct ListaK
        {
            public int IDP;
            public int AlmacenID;
            public decimal Value;
            public bool Has;
        };

        private struct Comparacion
        {
            public int Numero;
            public decimal Cantidad;
        };

        private struct ListaInventarios
        {
            public int ID;
            public string Display;
            public string Mostrar;
            public DateTime Fecha; 
        };
       
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

        public int IDInventarioFisico
        {
            get { return Convert.ToInt32(lkInventario.EditValue); }
            set { lkInventario.EditValue = value; }
        }

        private decimal _TipoCambio
        {
            get { return Convert.ToDecimal(spTC.Value); }
            set { spTC.Value = value; }
        }
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }

        private bool BloquearInventario
        {
            get { return Convert.ToBoolean(chkBloquearInventario.Checked); }
            set { chkBloquearInventario.Checked = value; }
        }

        private List<Entidad.AjusteConcepto> AC = new List<Entidad.AjusteConcepto>();
        public List<Entidad.AjusteConcepto> DetalleAC
        {
            get { return AC; }
            set
            {
                AC = value;
                this.bdsConcepto.DataSource = this.AC;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogAjusteInventario(int UserID, bool _editando, bool _ToPrint)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            Editable = _editando;
            ToPrint = _ToPrint;

            if (Editable)
            { //-- Bloquear Controles --//    
                dateFecha.Properties.ReadOnly = true;
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
                if (ToPrint)
                    this.Visible = false;
                
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                this.PermitirCostoCeroAjuste = Parametros.General.SystemOptionAcces(Parametros.General.UserID, "PermitirCostoCeroAjuste");

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

                DateTime DateServer = Convert.ToDateTime(db.GetDateServer());
                Fecha = Convert.ToDateTime(new DateTime(DateServer.Year, DateServer.Month, 1)).AddDays(-1);

                if (!ToPrint)
                {
                    ObjInventario = (from iv in db.InventarioFisicos
                                     join a in db.Areas on iv.AreaID equals a.ID
                                     where iv.EstacionServicioID.Equals(IDEstacionServicio) && iv.SubEstacionID.Equals(IDSubEstacion) && (iv.MovimientoID.Equals(0) || Editable)
                                     select new ListaInventarios
                                     {
                                         ID = iv.ID,
                                         Display = String.Format("{0:000000000} | {1}", iv.Numero, a.Nombre),
                                         Mostrar = String.Format("{0:000000000} | {1:dd/MM/yyyy} | {2}", iv.Numero, iv.FechaInventario, a.Nombre),
                                         Fecha = iv.FechaInventario.Date
                                     }).OrderBy(o => o.Fecha).ToList();

                    lkInventario.Properties.DataSource = ObjInventario.Select(s => new { s.ID, s.Display, s.Mostrar, s.Fecha }).ToList();
                }
                    //db.InventarioFisicos.Where(i => i.EstacionServicioID.Equals(IDEstacionServicio) && i.SubEstacionID.Equals(IDSubEstacion) && !i.Finalizado).Select(s => new { s.ID, Display = String.Format("{0:000000000} | {1:dd/MM/yyyy}", s.Numero, s.FechaInventario) });//.Select(s => new { s.ID, Display = s.Numero.ToString() });
                if (InventarioFisicoMDI > 0)
                    IDInventarioFisico = InventarioFisicoMDI;

                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(9)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(9)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }   
                
                txtNumero.Text = number.ToString("000000000");

                //nf.layoutControlGroup1.Text += " " + gvData.GetFocusedRowCellValue(colEstacionServicio).ToString() + " | " +
                //                (gvData.GetFocusedRowCellValue(colSubEstacion) == null ? "" : gvData.GetFocusedRowCellValue(colSubEstacion).ToString());
                            
                //-------------------------------//

                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

                this.bdsDetalle.DataSource = this.DetalleComprobante;

                this.lkConcepto.DataSource = db.ConceptoContables.Where(cc => cc.Activo && cc.EsAjuste).Select(s => new { s.ID, Display = s.Nombre }).ToList();

                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (Editable)
                {                    
                    btnLoad_Click(null, null);
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
            Parametros.General.ValidateEmptyStringRule(txtNumero, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkInventario, errRequiredField);
        }

        public bool ValidarCampos(bool conta)
        {
            if (txtNumero.EditValue == null || lkInventario.EditValue == null || Numero.Equals("000000000"))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la compra.", Parametros.MsgType.warning);
                return false;
            }

            if (!ToPrint && conta)
            {
                if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
                {
                    Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
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

            return true;
        }

        private void MostrarConceptos()
        {
            if (!String.IsNullOrEmpty(bgvProductos.Columns["Faltante"].SummaryText))
            {
                if (!Convert.ToDecimal(bgvProductos.Columns["Faltante"].SummaryText).Equals(0))
                {
                    layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                else
                {
                    DetalleAC.Clear();
                    layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
            }
            else
            {
                DetalleAC.Clear();
                layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }

            gvDataConcepto.RefreshData();
        }

        private bool Guardar(Boolean Finish)
        {
            if (!ValidarCampos(true)) return false;

            //if (dtEstacionesServicios.AsEnumerable().Count(c => ((bool) c["SelectedES"]).Equals(true)) <= 0)
                 

           if (dtInventario.AsEnumerable().Count(c => c["Numero"] != DBNull.Value && c["CantidadCompensada"] != DBNull.Value) <= 0)
            {
                if (Parametros.General.DialogMsg("No ha realizado ninguna compensación, Desa continuar.", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

           if (Finish)
           {
               if (dtInventario.AsEnumerable().Count(c => c["Costo"] == DBNull.Value) <= 0)
               {
                   if (dtInventario.AsEnumerable().Count(c => Convert.ToDecimal(c["Costo"]) <= 0) > 0)
                   {
                       Parametros.General.DialogMsg("Existe un costo 0 (cero) o Nulo, favor revisar los costos de los productos.", Parametros.MsgType.warning);

                       if (PermitirCostoCeroAjuste)
                       {
                           if (Parametros.General.DialogMsg("¿Desa finalizar con un costo 0 (cero) o Nulo.", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                               return false;
                       }
                       else
                           return false;
                   }
               }
               else
               {
                   Parametros.General.DialogMsg("Existe un costo 0 (cero) o Nulo, favor revisar los costos de los productos.", Parametros.MsgType.warning);

                   if (PermitirCostoCeroAjuste)
                   {
                       if (Parametros.General.DialogMsg("¿Desa finalizar con un costo 0 (cero) o Nulo.", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                           return false;
                   }
                   else
                       return false;
               }

               if (!String.IsNullOrEmpty(bgvProductos.Columns["Faltante"].SummaryText))
               {
                   decimal vFaltante = Math.Abs(Decimal.Round(Convert.ToDecimal(bgvProductos.Columns["Faltante"].SummaryText), 2, MidpointRounding.AwayFromZero));
                   decimal vConceptos = Math.Abs(DetalleAC.Sum(s => s.Monto));

                   if (!vFaltante.Equals(vConceptos))
                   {
                       Parametros.General.DialogMsg("El total del Faltante es diferente al monto de los Conceptos Contables.", Parametros.MsgType.warning);
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
                        M.AjusteDetalles.Clear();
                    }
                    else
                    {
                        M = new Entidad.Movimiento();
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.MovimientoTipoID = 9;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(9)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(9)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }

                        M.Numero = number;

                    }

                    M.UsuarioID = UsuarioID;
                    M.FechaRegistro = Fecha;
                    M.MonedaID = Parametros.Config.MonedaPrincipal();
                    M.TipoCambio = _TipoCambio;
                    M.AreaID = Inventario.AreaID;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Observacion;
                    M.InventarioBloqueado = BloquearInventario;
                    M.Finalizado = Finish;


                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                         "Se modificó el Ajuste de Inventario: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Inventario,
                        "Se registró el Ajuste de Inventario: " + M.Numero, this.Name);
                    }
                                        
                    db.SubmitChanges();

                    dtInventario.AsEnumerable().Where(c => c["Numero"] != DBNull.Value && c["CantidadCompensada"] != DBNull.Value).ToList().ForEach(item =>
                    {
                        if (!Convert.ToInt32(item["Numero"]).Equals(0) && !Convert.ToDecimal(item["CantidadCompensada"]).Equals(0))
                        {
                            M.AjusteDetalles.Add(new Entidad.AjusteDetalle { ProdcutoID = Convert.ToInt32(item["IDP"]), Cantidad = Convert.ToDecimal(item["CantidadCompensada"]), Numero = Convert.ToInt32(item["Numero"]) });
                            db.SubmitChanges();
                        }
                    });

                    db.AjusteConceptos.DeleteAllOnSubmit(db.AjusteConceptos.Where(a => a.MovimientoID.Equals(M.ID)));
                        
                        DetalleAC.ForEach( det =>
                            {
                                db.AjusteConceptos.InsertOnSubmit( new Entidad.AjusteConcepto { MovimientoID = M.ID, ConceptoID = det.ConceptoID, Monto = det.Monto});
                            });

                    if (Finish)
                    {
                        decimal vRealCompensado = 0m;

                        if (!String.IsNullOrEmpty(bgvProductos.Columns["Compensacion"].SummaryText))
                            vRealCompensado = Decimal.Round(Convert.ToDecimal(bgvProductos.Columns["Compensacion"].SummaryText), 2, MidpointRounding.AwayFromZero);

                        if (vRealCompensado < 0)
                        {
                            if (db.EstacionAreaPermisible.Count(c => c.EstacionID.Equals(M.EstacionServicioID) && c.AreaID.Equals(Inventario.AreaID)) > 0)
                            {
                                decimal vPermisible = Math.Abs(db.EstacionAreaPermisible.FirstOrDefault(c => c.EstacionID.Equals(M.EstacionServicioID) && c.AreaID.Equals(Inventario.AreaID)).Permisible);

                                if (vPermisible != null)
                                {
                                    decimal vCompensado = Math.Abs(vRealCompensado);

                                    if (vCompensado > vPermisible)
                                    {
                                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                        Parametros.General.DialogMsg("El valor a compensar: " + vCompensado.ToString("#,0.00") + Environment.NewLine + "Es mayor al permisible de esta Área: " + vPermisible.ToString("#,0.00"), Parametros.MsgType.warning);
                                        trans.Rollback();
                                        return false;

                                    }
                                }
                                else
                                {
                                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                    Parametros.General.DialogMsg("No existe un valor permisible para esta Área en la Estación de Servicio seleccionada.", Parametros.MsgType.warning);
                                    trans.Rollback();
                                    return false;
                                }

                            }
                            else
                            {
                                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                                Parametros.General.DialogMsg("No existe un valor permisible para esta Área en la Estación de Servicio seleccionada.", Parametros.MsgType.warning);
                                trans.Rollback();
                                return false;
                            }
                        }

                        List<Comparacion> Comp = new List<Comparacion>();

                        //Revisar cifas compensadas
                        foreach (DataRow row in dtInventario.Rows)
                        {
                            if (row["Total"] != DBNull.Value)
                            {
                                if (Convert.ToDecimal(row["Total"]) > 0)
                                {
                                    if (row["Numero"] != DBNull.Value)
                                    {

                                        if (row["CantidadCompensada"] != DBNull.Value)
                                        {
                                            Comp.Add(new Comparacion { Numero = Convert.ToInt32(row["Numero"]), Cantidad = Math.Abs(Convert.ToDecimal(row["CantidadCompensada"])) });
                                        }
                                    }
                                }
                                else if (Convert.ToDecimal(row["Total"]) < 0)
                                {
                                    if (row["Numero"] != DBNull.Value)
                                    {

                                        if (row["CantidadCompensada"] != DBNull.Value)
                                        {
                                            Comp.Add(new Comparacion { Numero = Convert.ToInt32(row["Numero"]), Cantidad = -Math.Abs(Convert.ToDecimal(row["CantidadCompensada"])) });
                                        }
                                    }
                                }
                            }
                        }

                        var grupo = Comp.GroupBy(g => g.Numero).Select(s => new { Numero = s.Key, Cantidad = s.Sum(m => m.Cantidad) });

                        if (grupo.Count(o => !o.Cantidad.Equals(0)) > 0)
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg("El total de la compensación Número: " + grupo.First(o => !o.Cantidad.Equals(0)).Numero + " no es igual a (cero) 0", Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
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
                        Compronbante.ForEach(linea =>
                        {
                            M.ComprobanteContables.Add(linea);
                            db.SubmitChanges();
                        });

                        db.SubmitChanges();

                        #endregion

                        decimal vMonto = 0;

                        foreach (DataRow row in dtInventario.Rows)
                        {
                            foreach (DataColumn col in dtInventario.Columns)
                            {
                                if (col.ColumnName.Contains("Diferencia"))
                                {
                                    if (!Convert.ToDecimal(row[col]).Equals(0))
                                    {
                                        decimal diferencia = Convert.ToDecimal(row[col]);
                                        var prod = db.Productos.Single(s => s.ID.Equals(row["IDP"]));
                                        decimal costo = Decimal.Round(Convert.ToDecimal(row["Costo"]), 4, MidpointRounding.AwayFromZero);
                                        string resultString = Regex.Match(col.ColumnName, @"\d+").Value;
                                        int ID = Convert.ToInt32(resultString);
                                        //decimal CostoMov = LineaDetalle.Cost;
                                        Entidad.Kardex KX = new Entidad.Kardex();
                                        KX.MovimientoID = M.ID;
                                        KX.ProductoID = prod.ID;
                                        KX.EsProducto = true;
                                        KX.EstacionServicioID = M.EstacionServicioID;
                                        KX.SubEstacionID = M.SubEstacionID;
                                        KX.UnidadMedidaID = prod.UnidadMedidaID;
                                        KX.Fecha = M.FechaRegistro;
                                        KX.CantidadInicial = Convert.ToDecimal(row["Kardex" + ID.ToString()]);
                                        KX.CostoInicial = costo;
                                        KX.CostoFinal = costo;

                                        if (diferencia > 0)
                                        {
                                            KX.AlmacenEntradaID = ID;
                                            KX.CantidadEntrada = diferencia;
                                            KX.CostoEntrada = costo;
                                            KX.CostoTotal = Decimal.Round(costo * KX.CantidadEntrada, 2, MidpointRounding.AwayFromZero);
                                        }

                                        if (diferencia < 0)
                                        {
                                            KX.AlmacenSalidaID = ID;
                                            KX.CantidadSalida = Math.Abs(diferencia);
                                            KX.CostoSalida = costo;
                                            KX.CostoTotal = Math.Abs(Decimal.Round(costo * KX.CantidadSalida, 2, MidpointRounding.AwayFromZero));
                                        }

                                        vMonto += KX.CostoTotal;
                                        KX.CantidadFinal = KX.CantidadInicial + diferencia;
                                        db.Kardexes.InsertOnSubmit(KX);

                                        #region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

                                        //------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    
                                             var AL = (from al in db.AlmacenProductos
                                                      where al.ProductoID.Equals(KX.ProductoID)
                                                        && al.AlmacenID.Equals(ID)
                                                      select al).ToList();

                                            if (AL.Count() > 0)
                                            {
                                                Entidad.AlmacenProducto AP = db.AlmacenProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.AlmacenID.Equals(ID));
                                                AP.Cantidad = KX.CantidadFinal;
                                                
                                            }
                                            else
                                            {
                                                //-- CREAR NUEVO REGISTRO 
                                                Entidad.AlmacenProducto AP = new Entidad.AlmacenProducto();
                                                AP.ProductoID = KX.ProductoID;
                                                AP.AlmacenID = ID;
                                                AP.Cantidad = KX.CantidadFinal;
                                                decimal precio = Decimal.Round((KX.CostoFinal * 1.35m), 6, MidpointRounding.AwayFromZero);
                                                //-- INSERTAR REGISTRO 
                                                AP.PrecioSugerido = precio;
                                                AP.Costo = KX.CostoFinal;
                                                db.AlmacenProductos.InsertOnSubmit(AP);
                                            }

                                        
                                        #endregion

                                        //para que actualice los datos del registro
                                        db.SubmitChanges();
                                    }
                                }
                            }

                        }

                        //Ingresar en Tabla InventarioCierre
                        var Bodegas = db.Almacens.Where(o => o.EstacionServicioID.Equals(M.EstacionServicioID) && o.SubEstacionID.Equals(M.SubEstacionID) && o.Activo);
                        
                        foreach (var wh in Bodegas)//Ciclo por Bodegas Activas
                        {
                            var GetDBAjuste = db.SetAjusteCierre(M.EstacionServicioID, M.SubEstacionID, wh.ID, M.AreaID, M.FechaRegistro);

                            List<Entidad.InventarioCierre> ICs = (from v in GetDBAjuste
                                                                  where !v.CostoTotal.Equals(0m) && !v.CantidadFinal.Equals(0)
                                                                  select new Entidad.InventarioCierre
                                                                  {
                                                                      AlmacenID = (int)v.AlmacenID,
                                                                      CantidadFinal = (decimal)v.CantidadFinal,
                                                                      CostoFinal = Decimal.Round((decimal)v.CostoFinal, 4, MidpointRounding.AwayFromZero),
                                                                      CostoTotal = Decimal.Round((decimal)v.CostoTotal, 2, MidpointRounding.AwayFromZero),
                                                                      CostoTotalAlmacen = Decimal.Round((decimal)v.CostoTotalAlmacen, 2, MidpointRounding.AwayFromZero),
                                                                      EstacionServicioID = (int)v.EstacionID,
                                                                      SubEstacionID = (int)v.SubEstacionID,
                                                                      Fecha = Convert.ToDateTime(v.Fecha).Date,
                                                                      ProductoID = (int)v.ProductoID
                                                                  }).ToList();

                            db.InventarioCierres.InsertAllOnSubmit(ICs);
                            db.SubmitChanges();

                        }

                        M.Monto = Decimal.Round(vMonto, 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(vMonto / _TipoCambio, 2, MidpointRounding.AwayFromZero);

                    }


                    
                    Entidad.InventarioFisico INV = db.InventarioFisicos.Single(iv => iv.ID.Equals(Inventario.ID));
                    INV.MovimientoID = M.ID;
                    if (Finish)
                        INV.Finalizado = true;

                    db.SubmitChanges();
                    
                    trans.Commit();
                    _ID = M.ID;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _SendPrint = Finish;                    
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
                    int i = 1;

                    decimal vSobrante = Decimal.Round(Convert.ToDecimal(bgvProductos.Columns["Sobrante"].SummaryText), 2, MidpointRounding.AwayFromZero);
                    decimal vFaltante = Decimal.Round(Convert.ToDecimal(bgvProductos.Columns["Faltante"].SummaryText), 2, MidpointRounding.AwayFromZero);
                    decimal vCompensacion = Decimal.Round(Convert.ToDecimal(bgvProductos.Columns["Compensacion"].SummaryText), 2, MidpointRounding.AwayFromZero);

                    #region <<< SOBRANTE >>>

                    if (vSobrante > 0)
                    {
                        List<Int32> IDSobrante = new List<Int32>();
                        List<Int32> IDInventario = new List<Int32>();

                        foreach (DataRow linea in dtInventario.Rows)
                        {
                            if (linea["Sobrante"] != DBNull.Value)
                            {
                                int IDProducto = Convert.ToInt32(linea["IDP"]);
                                var area = from a in db.Areas
                                           join pc in db.ProductoClases on a.ID equals pc.AreaID
                                           join p in db.Productos on pc.ID equals p.ProductoClaseID
                                           where p.ID.Equals(IDProducto)
                                           select a;

                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                {
                                    if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaInventarioID,
                                            Monto = Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDInventario.Add(area.First().CuentaInventarioID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID)).First();
                                        comprobante.Monto += Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }

                                    if (!IDSobrante.Contains(area.First().CuentaSobranteID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaSobranteID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(area.First().CuentaSobranteID)).CecoID,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDSobrante.Add(area.First().CuentaSobranteID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaSobranteID)).First();
                                        comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }

                                }
                                else
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(IDProducto));
                                    if (!IDInventario.Contains(producto.CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaInventarioID,
                                            Monto = Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDInventario.Add(producto.CuentaInventarioID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID)).First();
                                        comprobante.Monto += Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero);
                                        comprobante.MontoMonedaSecundaria += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero);
                                    }

                                    if (!IDSobrante.Contains(producto.CuentaSobranteID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaSobranteID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(area.First().CuentaSobranteID)).CecoID,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDSobrante.Add(producto.CuentaSobranteID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaSobranteID)).First();
                                        comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }

                                }
                            }
                        }

                        //foreach (DataRow linea in dtInventario.Rows)
                        //{
                        //    if (linea["Sobrante"] != DBNull.Value)
                        //    {
                        //        int IDProducto = Convert.ToInt32(linea["IDP"]);
                        //        var area = from a in db.Areas
                        //                   join pc in db.ProductoClases on a.ID equals pc.AreaID
                        //                   join p in db.Productos on pc.ID equals p.ProductoClaseID
                        //                   where p.ID.Equals(IDProducto)
                        //                   select a;

                        //        if (area.Count(a => !a.CuentaSobranteID.Equals(0)) > 0)
                        //        {
                        //            if (!IDSobrante.Contains(area.First().CuentaSobranteID))
                        //            {
                        //                CD.Add(new Entidad.ComprobanteContable
                        //                {
                        //                    CuentaContableID = area.First().CuentaSobranteID,
                        //                    Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero)),
                        //                    TipoCambio = _TipoCambio,
                        //                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                        //                    Fecha = Fecha,
                        //                    Descripcion = "Ajuste de Inventario / Sobrante",
                        //                    Linea = i,
                        //                    EstacionServicioID = IDEstacionServicio,
                        //                    SubEstacionID = IDSubEstacion
                        //                });
                        //                IDSobrante.Add(area.First().CuentaSobranteID);
                        //                i++;
                        //            }
                        //            else
                        //            {
                        //                var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaSobranteID)).First();
                        //                comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero));
                        //                comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                        //            }
                        //        }
                        //        else
                        //        {
                        //            var producto = db.Productos.Single(p => p.ID.Equals(IDProducto));
                        //            if (!IDSobrante.Contains(producto.CuentaSobranteID))
                        //            {
                        //                CD.Add(new Entidad.ComprobanteContable
                        //                {
                        //                    CuentaContableID = producto.CuentaSobranteID,
                        //                    Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero)),
                        //                    TipoCambio = _TipoCambio,
                        //                    MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                        //                    Fecha = Fecha,
                        //                    Descripcion = "Ajuste de Inventario / Sobrante",
                        //                    Linea = i,
                        //                    EstacionServicioID = IDEstacionServicio,
                        //                    SubEstacionID = IDSubEstacion
                        //                });
                        //                IDSobrante.Add(producto.CuentaSobranteID);
                        //                i++;
                        //            }
                        //            else
                        //            {
                        //                var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaSobranteID)).First();
                        //                comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Sobrante"]), 2, MidpointRounding.AwayFromZero));
                        //                comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Sobrante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                        //            }
                        //        }
                        //    }                       
                        //}
                    }                    

                    #endregion

                    int Sob = i;

                    #region <<< FALTANTE >>>

                    if (vFaltante < 0)
                    {
                        List<Int32> IDInventario = new List<Int32>();

                        DetalleAC.ForEach(det =>
                            {
                                var Concepto = db.ConceptoContables.Single(c => c.ID.Equals(det.ConceptoID));

                                CD.Add(new Entidad.ComprobanteContable
                                {
                                    CuentaContableID = Concepto.CuentaContableID,
                                    Monto = Decimal.Round(det.Monto, 2, MidpointRounding.AwayFromZero),
                                    TipoCambio = _TipoCambio,
                                    MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(det.Monto / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                    Fecha = Fecha,
                                    Descripcion = "Ajuste de Inventario / Faltante" + " Inv. Físico " + lkInventario.Text,
                                    Linea = i,
                                    CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(Concepto.CuentaContableID)).CecoID,
                                    EstacionServicioID = IDEstacionServicio,
                                    SubEstacionID = IDSubEstacion
                                });
                                i++;
                            });

                        foreach (DataRow linea in dtInventario.Rows)
                        {
                            if (linea["Faltante"] != DBNull.Value)
                            {
                                int IDProducto = Convert.ToInt32(linea["IDP"]);
                                var area = from a in db.Areas
                                           join pc in db.ProductoClases on a.ID equals pc.AreaID
                                           join p in db.Productos on pc.ID equals p.ProductoClaseID
                                           where p.ID.Equals(IDProducto)
                                           select a;

                                if (area.Count(a => !a.CuentaInventarioID.Equals(0)) > 0)
                                {
                                    if (!IDInventario.Contains(area.First().CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = area.First().CuentaInventarioID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Faltante"]), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Faltante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Faltante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDInventario.Add(area.First().CuentaInventarioID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(area.First().CuentaInventarioID) && c.Linea >= Sob).First();
                                        comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Faltante"]), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Faltante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }
                                }
                                else
                                {
                                    var producto = db.Productos.Single(p => p.ID.Equals(IDProducto));
                                    if (!IDInventario.Contains(producto.CuentaInventarioID))
                                    {
                                        CD.Add(new Entidad.ComprobanteContable
                                        {
                                            CuentaContableID = producto.CuentaInventarioID,
                                            Monto = -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Faltante"]), 2, MidpointRounding.AwayFromZero)),
                                            TipoCambio = _TipoCambio,
                                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Faltante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                            Fecha = Fecha,
                                            Descripcion = "Ajuste de Inventario / Faltante" + " Inv. Físico " + lkInventario.Text,
                                            Linea = i,
                                            CentroCostoID = 0,
                                            EstacionServicioID = IDEstacionServicio,
                                            SubEstacionID = IDSubEstacion
                                        });
                                        IDInventario.Add(producto.CuentaInventarioID);
                                        i++;
                                    }
                                    else
                                    {
                                        var comprobante = CD.Where(c => c.CuentaContableID.Equals(producto.CuentaInventarioID) && c.Linea >= Sob).First();
                                        comprobante.Monto += -Math.Abs(Decimal.Round(Convert.ToDecimal(linea["Faltante"]), 2, MidpointRounding.AwayFromZero));
                                        comprobante.MontoMonedaSecundaria += -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea["Faltante"]) / _TipoCambio), 2, MidpointRounding.AwayFromZero));
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    if (!vCompensacion.Equals(0))
                    {
                        var area = db.Areas.Single(a => a.ID.Equals(Inventario.AreaID));

                        if (vCompensacion > 0)
                        {
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = area.CuentaInventarioID,
                                Monto = Decimal.Round(vCompensacion, 2, MidpointRounding.AwayFromZero),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(vCompensacion / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                                Fecha = Fecha,
                                Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = area.CuentaSobranteID,
                                Monto = -Math.Abs(Decimal.Round(vCompensacion, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vCompensacion / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                Linea = i,
                                CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(area.CuentaSobranteID)).CecoID,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;
                        }
                        else
                        {
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = area.CuentaFaltanteID,
                                Monto = Math.Abs(Decimal.Round(vCompensacion, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = Math.Abs(Decimal.Round(Convert.ToDecimal(vCompensacion / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                Linea = i,
                                CentroCostoID = db.CuentaContables.Single(s => s.ID.Equals(area.CuentaFaltanteID)).CecoID,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;

                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = area.CuentaInventarioID,
                                Monto = -Math.Abs(Decimal.Round(vCompensacion, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(vCompensacion / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                                Fecha = Fecha,
                                Descripcion = "Ajuste de Inventario / Sobrante" + " Inv. Físico " + lkInventario.Text,
                                Linea = i,
                                CentroCostoID = 0,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                            i++;                            

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

        private DevExpress.XtraGrid.StyleFormatCondition styleFormatConditioner(Parametros.MyBandColumn BCol)
        {
            try
            {
                DevExpress.XtraGrid.StyleFormatCondition style = new DevExpress.XtraGrid.StyleFormatCondition();

                style.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
                style.Appearance.Options.UseBackColor = true;
                style.ApplyToRow = false;
                style.Column = BCol;
                style.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                style.Expression = "[" + BCol.FieldName + "] < " + Convert.ToString("0");
                // + " or [" + BCol.FieldName + "] > " + Convert.ToString(DiferenciaVerificacionLectura);

                return style;
            }
            catch
            { return null; }
        }

        private DevExpress.XtraGrid.StyleFormatCondition styleFormatConditionerPositive(Parametros.MyBandColumn BCol)
        {
            try
            {
                DevExpress.XtraGrid.StyleFormatCondition style = new DevExpress.XtraGrid.StyleFormatCondition();
                //154-205-50
                style.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(154)))), ((int)(((byte)(205)))), ((int)(((byte)(50)))));
                style.Appearance.Options.UseBackColor = true;
                style.ApplyToRow = false;
                style.Column = BCol;
                style.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
                style.Expression = "[" + BCol.FieldName + "] > " + Convert.ToString("0");
                // + " or [" + BCol.FieldName + "] > " + Convert.ToString(DiferenciaVerificacionLectura);

                return style;
            }
            catch
            { return null; }
        }

        private decimal GetCosto(int IDEstacionServicio, int IDSubEstacion, int IDproducto, DateTime Fecha)
        {
            try
            {                
                var KCIList = (from k in db.Kardexes
                               join m in db.Movimientos on k.MovimientoID equals m.ID
                               where k.ProductoID.Equals(IDproducto) && m.Anulado.Equals(false) && (m.MovimientoTipoID.Equals(3) || m.MovimientoTipoID.Equals(1) || m.MovimientoTipoID.Equals(43) || m.MovimientoTipoID.Equals(69))
                             && k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.CostoEntrada.Equals(0)
                             && k.Fecha.Date <= Convert.ToDateTime(Fecha).Date
                               select new { k.ID, k.Fecha, k.CostoFinal }).OrderByDescending(o => o.Fecha).FirstOrDefault();

                if (KCIList != null)
                    return Decimal.Round(KCIList.CostoFinal, 4, MidpointRounding.AwayFromZero);
                else
                    return 0m;
            }
            catch { return 0m; }
        }

        private decimal SaldoAjusteKardex(int IDEstacionServicio, int IDSubEstacion, int IDproducto, int IDAlmacen, DateTime Fecha)
        {
            try
            {
                int IDMov = 0;
                if (EntidadAnterior != null)
                    IDMov = EntidadAnterior.ID;

                var obj = from k in db.Kardexes
                          join m in db.Movimientos on k.Movimiento equals m
                          join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                          where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && k.ProductoID.Equals(IDproducto) && !k.EsManejo
                          && !m.Anulado && !mt.EsAnulado
                          && (k.AlmacenEntradaID.Equals(IDAlmacen) || k.AlmacenSalidaID.Equals(IDAlmacen))
                          && !k.MovimientoID.Equals(IDMov) && k.Fecha.Date <= Fecha.Date
                          //&& (k.Fecha.Date > (Anterior != null ? Anterior.FechaActa.Date : Parametros.Config.FechaCierreActa().Date) && k.Fecha.Date <= Fecha.Date)
                          select new { k.ID, k.CantidadEntrada, k.CantidadSalida };

                return Decimal.Round((obj.Count() > 0 ? obj.Sum(s => s.CantidadEntrada) - obj.Sum(s => s.CantidadSalida) : 0m), 3, MidpointRounding.AwayFromZero);

            }
            catch { return 0m; }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {

                if (!ValidarCampos(false))
                    return;
                
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO + Environment.NewLine + "Por favor espere unos minutos...");

                db.CommandTimeout = 10000;
                Inventario = db.InventarioFisicos.Single(iv => iv.ID.Equals(IDInventarioFisico));
                db.SubmitChanges();

                Fecha = Inventario.FechaInventario;
                IDEstacionServicio = Inventario.EstacionServicioID;
                IDSubEstacion = Inventario.SubEstacionID;
                lkInventario.EditValue = IDInventarioFisico;
                
                    //dtInventario.Columns.Add("IDP", typeof(Int32));
                    //dtInventario.Columns.Add("Producto", typeof(String));

                    //db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion)).ToList().ForEach(alma =>
                    //{
                    //    dtInventario.Columns.Add(new DataColumn { ColumnName = alma.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });

                    //    var colAlma = new Parametros.MyBandColumn(alma.Nombre, alma.ID.ToString(), alma);
                    //    colAlma.OptionsFilter.AllowAutoFilter = false;
                    //    colAlma.OptionsFilter.AllowFilter = false;
                    //    colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                    //    DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();

                    //    //**Repositorio para digitar la lectura
                    //    rpLectura.AutoHeight = false;
                    //    rpLectura.AllowMouseWheel = false;
                    //    rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                    //    rpLectura.Name = "rpItem" + alma.Nombre;
                    //    rpLectura.EditFormat.FormatString = "N3";
                    //    rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    //    rpLectura.DisplayFormat.FormatString = "N3";
                    //    rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    //    rpLectura.EditMask = "N3";
                    //    rpLectura.Buttons.Clear();
                    //    this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                    //    colAlma.ColumnEdit = rpLectura;

                    //    //bandAlmacenes.Columns.Add(colAlma);

                    //});

                    //dtInventario.Rows.Clear();

                    //var query = from p in db.Productos
                    //            join c in db.ProductoClases on p.ProductoClaseID equals c.ID
                    //            join ar in db.Areas on c.AreaID equals ar.ID
                    //            where ar.ID.Equals(IDInventarioFisico)
                    //            select new
                    //            {
                    //                IDP = p.ID,
                    //                Producto = p.Codigo + " | " + p.Nombre
                    //            };

                    //query.ToList().ForEach(obj => { dtInventario.Rows.Add(obj.IDP, obj.Producto); });

                    //bdsDetalle.DataSource = dtInventario;

                    //Despues hacerlo por el DataTable para que lo devuelva ordenado
                    //EntidadAnterior.InventarioFisicoDetalles.ToList().ForEach(obj =>
                    //    {
                    //        bgvProductos.SetRowCellValue(bgvProductos.LocateByValue("IDP", obj.ProductoID, null), obj.AlmacenID.ToString(), obj.Cantidad);
                    //    });

                
                    dtInventario.Columns.Add("IDP", typeof(Int32));
                    dtInventario.Columns.Add("Producto", typeof(String));
                    dtInventario.Columns.Add("Medida", typeof(String));
                    dtInventario.Columns.Add("Costo", typeof(Decimal));
                    dtInventario.Columns.Add("Total", typeof(Decimal));
                    dtInventario.Columns.Add("Sobrante", typeof(Decimal));
                    dtInventario.Columns.Add("Faltante", typeof(Decimal));
                    dtInventario.Columns.Add("Compensacion", typeof(Decimal));
                    dtInventario.Columns.Add("Numero", typeof(Int32));
                    dtInventario.Columns.Add("CantidadCompensada", typeof(Decimal));

                    int index = bgvProductos.Bands.Count - 1;
                    string suma = "";

                    db.Almacens.Where(a => a.EstacionServicioID.Equals(IDEstacionServicio) && a.SubEstacionID.Equals(IDSubEstacion) && a.Activo).ToList().ForEach(obj =>
                    {
                        index++;
                        //var bandLado = new Parametros.MyBand("gBandAlma" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataMecanica.Bands.Count + 1);

                        //Diseño de las bandas del girdAlmacenes
                        this.bgvProductos.Bands.Add(new Parametros.MyBand("gBandAlma" + obj.ID.ToString(), "Almacen " + obj.Nombre.ToString(), 200, index));

                        for (int i = 0; i < 3; i++)
                        {
                            string titulo = "";

                            switch (i)
                            {
                                case 0: titulo = "Kardex";
                                    break;
                                case 1: titulo = "Fisico";
                                    break;
                                case 2: titulo = "Diferencia";
                                    break; ;
                            }

                            dtInventario.Columns.Add(new DataColumn { ColumnName = titulo + obj.ID.ToString(), DataType = typeof(Decimal), DefaultValue = 0.000m });

                            var colAlma = new Parametros.MyBandColumn(titulo, titulo + obj.ID.ToString(), null);
                            colAlma.Tag = i;
                            colAlma.OptionsFilter.AllowAutoFilter = false;
                            colAlma.OptionsFilter.AllowFilter = false;
                            colAlma.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                            colAlma.OptionsColumn.AllowEdit = false;
                            colAlma.OptionsColumn.AllowFocus = false;
                            if (i.Equals(2))
                            {
                                if (!String.IsNullOrEmpty(suma))
                                    suma += " + ";

                                suma += "[" + titulo + obj.ID.ToString() + "]";
                                bgvProductos.FormatConditions.Add(styleFormatConditioner(colAlma));
                                bgvProductos.FormatConditions.Add(styleFormatConditionerPositive(colAlma));
                            }

                            bgvProductos.Bands[index].Columns.Add(colAlma);
                           
                        }
                    });


                    index++;
                    bgvProductos.Bands.MoveTo(index, this.gBandDiferencias);

                    bdsDetalle.DataSource = dtInventario;

                    dtInventario.Rows.Clear();

                    //List<Parametros.ListAjuste> query = (from sp in db.spAjusteInventarioFisico(IDInventarioFisico, Inventario.AreaID)
                    //                                     select new Parametros.ListAjuste
                    //            {
                    //                IDP = sp.ProdcutoID,
                    //                Producto = sp.Producto,
                    //                Medida = sp.UnidadMedida,
                    //                Costo = sp.Costo,
                    //                AlmacenID = sp.AlmacenID,
                    //                CantidadExistencia = sp.CantidadExistencia,
                    //                CantidadInventario = sp.CantidadInventario
                    //            }).ToList();

                //////////////////////////////////////////
                    //if (ToPrint)
                    //{
                    //    var Ajte = from k in db.Kardexes.Where(k => k.MovimientoID.Equals(EntidadAnterior.ID))
                    //               select new { k.ProductoID, Almacen = (k.AlmacenEntradaID.Equals(0) ? k.AlmacenSalidaID : k.AlmacenEntradaID), Cantidad = (k.CantidadEntrada.Equals(0) ? k.CantidadSalida : -Math.Abs(k.CantidadEntrada)) };

                    //    query.ForEach(obj =>
                    //    {
                    //        var _Ajte = Ajte.FirstOrDefault(a => a.ProductoID.Equals(obj.IDP) && a.Almacen.Equals(obj.AlmacenID));

                    //        if (_Ajte != null)
                    //        {
                    //            obj.CantidadExistencia += _Ajte.Cantidad;
                    //        }
                    //    });
                    //}

/********CONSULTA NUEVA*******/
                    List<Parametros.ListAjuste> query = (from ivd in db.InventarioFisicoDetalles
                                                         join iv in db.InventarioFisicos on ivd.InventarioFisicoID equals iv.ID
                                                         join p in db.Productos on ivd.ProductoID equals p.ID
                                                         join u in db.UnidadMedidas on p.UnidadMedidaID equals u.ID
                                                         join a in db.Almacens on ivd.AlmacenID equals a.ID
                                                         where ivd.InventarioFisicoID == IDInventarioFisico
                                                         select new Parametros.ListAjuste
                                                         {
                                                             IDP = ivd.ProductoID,
                                                             Producto = p.Codigo + " | " + p.Nombre,
                                                             Medida = u.Nombre,
                                                             AlmacenID = ivd.AlmacenID,
                                                             CantidadInventario = ivd.Cantidad,
                                                             CantidadExistencia = 0
                                                         }).ToList();


                    List<ListaK> Kardex;

                    if (!ToPrint)
                    {
                        Kardex = (
                                    from k in db.Kardexes
                                    join m in db.Movimientos on k.MovimientoID equals m.ID
                                    join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                    join p in db.Productos on k.ProductoID equals p.ID
                                    join um in db.UnidadMedidas on p.UnidadMedidaID equals um.ID
                                    join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                    join ar in db.Areas on pc.AreaID equals ar.ID
                                    where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.EsManejo && k.EsProducto
                                    && !m.Anulado && !mt.EsAnulado && ar.ID.Equals(Inventario.AreaID) &&
                                    k.Fecha.Date <= Fecha.Date
                                    group k by new { k.ProductoID, k.AlmacenEntradaID, k.AlmacenSalidaID } into gr
                                    select new
                                    {
                                        IDP = gr.Key.ProductoID,
                                        Almacen = (gr.Key.AlmacenEntradaID.Equals(0) ? gr.Key.AlmacenSalidaID : gr.Key.AlmacenEntradaID),
                                        CantidadExistencia = gr.Sum(s => s.CantidadEntrada) - gr.Sum(s => s.CantidadSalida)
                                    }
                                ).GroupBy(g => new { g.IDP, g.Almacen }).Select(s => new ListaK { IDP = s.Key.IDP, AlmacenID = s.Key.Almacen, Value = s.Sum(sm => sm.CantidadExistencia), Has = false }).ToList();
                    }
                    else
                    {
                        Kardex = (
                                                            from k in db.Kardexes
                                                            join m in db.Movimientos on k.MovimientoID equals m.ID
                                                            join mt in db.MovimientoTipos on m.MovimientoTipoID equals mt.ID
                                                            join p in db.Productos on k.ProductoID equals p.ID
                                                            join um in db.UnidadMedidas on p.UnidadMedidaID equals um.ID
                                                            join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                                            join ar in db.Areas on pc.AreaID equals ar.ID
                                                            where k.EstacionServicioID.Equals(IDEstacionServicio) && k.SubEstacionID.Equals(IDSubEstacion) && !k.EsManejo && k.EsProducto
                                                            && !m.Anulado && !mt.EsAnulado && ar.ID.Equals(Inventario.AreaID) && !m.ID.Equals(EntidadAnterior.ID) &&
                                                            k.Fecha.Date <= Fecha.Date
                                                            group k by new { k.ProductoID, k.AlmacenEntradaID, k.AlmacenSalidaID } into gr
                                                            select new
                                                            {
                                                                IDP = gr.Key.ProductoID,
                                                                Almacen = (gr.Key.AlmacenEntradaID.Equals(0) ? gr.Key.AlmacenSalidaID : gr.Key.AlmacenEntradaID),
                                                                CantidadExistencia = gr.Sum(s => s.CantidadEntrada) - gr.Sum(s => s.CantidadSalida)
                                                            }
                                                        ).GroupBy(g => new { g.IDP, g.Almacen }).Select(s => new ListaK { IDP = s.Key.IDP, AlmacenID = s.Key.Almacen, Value = s.Sum(sm => sm.CantidadExistencia), Has = false }).ToList();
                   
                    }


                    query.ForEach(det =>
                        {
                            if (Kardex.Count(l => l.IDP.Equals(det.IDP) && l.AlmacenID.Equals(det.AlmacenID)) > 0)
                            {
                                det.CantidadExistencia = Kardex.FirstOrDefault(l => l.IDP.Equals(det.IDP) && l.AlmacenID.Equals(det.AlmacenID)).Value;
                                Kardex.Remove(Kardex.FirstOrDefault(l => l.IDP.Equals(det.IDP) && l.AlmacenID.Equals(det.AlmacenID)));
                            }
                        }
                        );

                List<Parametros.ListAjuste> query2 = (from k in Kardex
                                                         join p in db.Productos on k.IDP equals p.ID
                                                         join u in db.UnidadMedidas on p.UnidadMedidaID equals u.ID
                                                         where !k.Has
                                                         select new Parametros.ListAjuste
                                                         {
                                                             IDP = k.IDP,
                                                             Producto = p.Codigo + " | " + p.Nombre,
                                                             Medida = u.Nombre,
                                                             AlmacenID = k.AlmacenID,
                                                             CantidadInventario = 0,
                                                             CantidadExistencia = k.Value
                                                         }).ToList();

                query.AddRange(query2);

                    //Kardex.Where(k => !k.Has).ToList().ForEach(det =>
                    //{
                    //    query.Add(new Parametros.ListAjuste { IDP = det.IDP, })
                    //}
                    //    );

                //IDP = ivd.ProductoID,
                //Producto = p.Codigo + " | " + p.Nombre,
                //Medida = u.Nombre,
                //AlmacenID = ivd.AlmacenID,
                //CantidadInventario = ivd.Cantidad,
                //CantidadExistencia = 0

                /********CONSULTA ANTERIOR*******/
                /**
                    List<Parametros.ListAjuste> query = (from ivd in db.InventarioFisicoDetalles.ToList()
                                                         join iv in db.InventarioFisicos.ToList() on ivd.InventarioFisicoID equals iv.ID
                                                         join a in db.Almacens.ToList() on new { iv.EstacionServicioID, iv.SubEstacionID } equals new { a.EstacionServicioID, a.SubEstacionID }
                                                         join k in db.Kardexes.ToList() on new { iv.EstacionServicioID, iv.SubEstacionID } equals new { k.EstacionServicioID, k.SubEstacionID }
                                                         from p in db.Productos.ToList()
                                                         join um in db.UnidadMedidas.ToList() on p.UnidadMedidaID equals um.ID
                                                         join pc in db.ProductoClases.ToList() on p.ProductoClaseID equals pc.ID
                                                         join ar in db.Areas.ToList() on pc.AreaID equals ar.ID
                                                        where ivd.InventarioFisicoID == IDInventarioFisico
                                                        && ar.ID == Inventario.AreaID && a.Activo
                                                        && (ivd.ProductoID == p.ID || k.ProductoID == p.ID)
                                                        group new { p, a, um } by new { p.ID, p.Codigo, p.Nombre, Medida = um.Nombre, AlmacenID = a.ID, AlmacenNombre = a.Nombre } into gr
                                                        select new Parametros.ListAjuste
                                                        {
                                                            IDP = gr.Key.ID,
                                                            Producto = gr.Key.Codigo + " | " + gr.Key.Nombre,
                                                            Medida = gr.Key.Medida,
                                                            Costo = GetCosto(IDEstacionServicio, IDSubEstacion, gr.Key.ID, Fecha),
                                                            AlmacenID = gr.Key.AlmacenID,
                                                            CantidadInventario = (db.InventarioFisicoDetalles.Count(i => i.ProductoID.Equals(gr.Key.ID) && i.AlmacenID.Equals(gr.Key.AlmacenID) && i.InventarioFisicoID.Equals(IDInventarioFisico)) > 0
                                                                                  ? db.InventarioFisicoDetalles.First(i => i.ProductoID.Equals(gr.Key.ID) && i.AlmacenID.Equals(gr.Key.AlmacenID) && i.InventarioFisicoID.Equals(IDInventarioFisico)).Cantidad : 0m),
                                                            CantidadExistencia = SaldoAjusteKardex(IDEstacionServicio, IDSubEstacion, gr.Key.ID, gr.Key.AlmacenID, Fecha)
                                                        }).ToList();
                **/
                //////////////////////////////////////////

                    List<Int32> Prod = new List<Int32>();
                    
                    query.ToList().ForEach(obj => {

                        if (!Prod.Contains(obj.IDP))
                        {
                            DataRow dr = dtInventario.NewRow();
                            dr["IDP"] = obj.IDP;
                            dr["Producto"] = obj.Producto;
                            dr["Medida"] = obj.Medida;
                            dr["Costo"] = obj.Costo;
                            dr["Kardex" + obj.AlmacenID] = obj.CantidadExistencia;
                            dr["Fisico" + obj.AlmacenID] = obj.CantidadInventario;
                            dr["Diferencia" + obj.AlmacenID] = obj.CantidadInventario - obj.CantidadExistencia;
                           
                            dtInventario.Rows.Add(dr);
                            //dtInventario.Rows.Add(obj.IDP, obj.Producto, obj.Medida, obj.Costo);
                        
                            Prod.Add(obj.IDP);
                        }
                        else
                        {
                            DataRow[] ESRow = dtInventario.Select("IDP = " + obj.IDP);
                            DataRow row = ESRow.First();
                            if (Convert.ToDecimal(row["Costo"]) < obj.Costo)
                                row["Costo"] = obj.Costo;

                            row["Kardex" + obj.AlmacenID] = obj.CantidadExistencia;
                            row["Fisico" + obj.AlmacenID] = obj.CantidadInventario;
                            row["Diferencia" + obj.AlmacenID] = obj.CantidadInventario - obj.CantidadExistencia;

                        }
                    });

                    List<Int32> item = new List<Int32>();

                    foreach (DataRow row in dtInventario.Rows)
                    {
                        decimal dif = 0;
                        decimal costo = 0;
                        bool borrar = true;
                        foreach (DataColumn col in dtInventario.Columns)
                        {
                            //if (col.ColumnName.Equals("Costo"))
                            //    costo = Convert.ToDecimal(row[col]);

                            if (col.ColumnName.Contains("Diferencia"))
                            {
                                if (!Convert.ToDecimal(row[col]).Equals(0))
                                    borrar = false;
                                    
                                dif = dif + Convert.ToDecimal(row[col]);
                            }
                        }

                        if (borrar)
                            item.Add(Convert.ToInt32(row["IDP"]));
                        else
                        {
                            row["Total"] = dif;
                            costo = GetCosto(IDEstacionServicio, IDSubEstacion, (int)row["IDP"], Fecha);
                            row["Costo"] = costo;
                            dif = Decimal.Round(dif * costo, 2, MidpointRounding.AwayFromZero);

                            if (dif < 0)
                                row["Faltante"] = dif;
                            else if (dif > 0)
                                row["Sobrante"] = dif;
                        }
                    }

                    item.ForEach(obj => { dtInventario.Rows.Remove(dtInventario.Select("IDP = " + obj.ToString()).First()); });
                
                    if (Editable)
                    {

                        #region <<< EDITABLE >>>

                        //lkInventario.Properties.DataSource = null;

                        //var obj = from iv in db.InventarioFisicos
                        //          join a in db.Areas on iv.AreaID equals a.ID
                        //          where iv.EstacionServicioID.Equals(IDEstacionServicio) && iv.SubEstacionID.Equals(IDSubEstacion) && !iv.Finalizado
                        //          select new
                        //          {
                        //              iv.ID,
                        //              Display = String.Format("{0:000000000} | {1}", iv.Numero, a.Nombre),
                        //              Mostrar = String.Format("{0:000000000} | {1:dd/MM/yyyy} | {2}", iv.Numero, iv.FechaInventario, a.Nombre)
                        //          };

                        //lkInventario.Properties.DataSource = obj;

                        Fecha = EntidadAnterior.FechaRegistro;
                        Numero = EntidadAnterior.Numero.ToString("000000000");
                        Observacion = EntidadAnterior.Comentario;
                        _TipoCambio = EntidadAnterior.TipoCambio;

                        listadoNumero = new List<Parametros.ListIdDisplay>();
                        listadoNumero.Add(new Parametros.ListIdDisplay { ID = 0, Display = "N/A" });

                        EntidadAnterior.AjusteDetalles.GroupBy(g => g.Numero).ToList().ForEach(n =>
                            {
                                listadoNumero.Add(new Parametros.ListIdDisplay { ID = n.Key, Display = n.Key.ToString() });
                            });

                        int z = (listadoNumero.Count > 0 ? (listadoNumero.OrderByDescending(o => o.ID).First().ID + 1) : 1);

                        listadoNumero.Add(new Parametros.ListIdDisplay { ID = z, Display = z.ToString() });

                        lkNro.DataSource = listadoNumero;

                        bgvProductos.RefreshData();

                        EntidadAnterior.AjusteDetalles.ToList().ForEach(det =>
                            {
                                DataRow[] ESRow = dtInventario.Select("IDP = " + det.ProdcutoID);
                                if (ESRow.Count() > 0)
                                {
                                    DataRow row = ESRow.First();
                                    decimal Costo = 0, Diferencias = 0, CostoInicial = 0, CostoFinal = 0;

                                    row["Numero"] = det.Numero;
                                    row["CantidadCompensada"] = det.Cantidad;

                                    Costo = Convert.ToDecimal(row["Costo"]);
                                    Diferencias = Convert.ToDecimal(row["Total"]);
                                    CostoInicial = Math.Abs(Decimal.Round(Costo * Diferencias, 2, MidpointRounding.AwayFromZero));
                                    CostoFinal = Decimal.Round(det.Cantidad * Costo, 2, MidpointRounding.AwayFromZero);

                                    if (Diferencias < 0)
                                    {
                                        CostoInicial *= -1;
                                        row["Faltante"] = CostoInicial + CostoFinal;
                                        CostoFinal *= -1;
                                    }
                                    else
                                    {
                                        row["Sobrante"] = CostoInicial - CostoFinal;
                                    }

                                    row["Compensacion"] = CostoFinal;
                                }
                            });

                        bgvProductos.RefreshData();
                        
                        #endregion
                    }
                    else
                    {

                        #region <<< NO_EDITABLE >>>

                        this.layoutControlGroup1.Text += " " + Parametros.General.EstacionServicioName + " | " +
                                        (IDSubEstacion.Equals(0) ? "" : db.SubEstacions.Single(s => s.ID.Equals(IDSubEstacion)).Nombre);

                        listadoNumero = new List<Parametros.ListIdDisplay>();

                        listadoNumero.Add(new Parametros.ListIdDisplay { ID = 0, Display = "N/A" });
                        listadoNumero.Add(new Parametros.ListIdDisplay { ID = 1, Display = "1" });

                        lkNro.DataSource = listadoNumero;
                        #endregion
                    }



                    
                this.txtNumero.Properties.ReadOnly = true;
                this.dateFecha.Properties.ReadOnly = true;
                this.lkInventario.Properties.ReadOnly = true;
                this.splitContainerControlMain.Visible = true;
                this.chkBloquearInventario.Checked = true;
                this.btnLoad.Enabled = false;

                this.bdsConcepto.DataSource = DetalleAC;
                MostrarConceptos();

                if (Editable)
                {
                    var ajustes = from aj in db.AjusteConceptos where aj.MovimientoID.Equals(EntidadAnterior.ID) select aj;

                    if (ajustes.Count() > 0)
                    {
                        layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        DetalleAC = ajustes.ToList();
                    }
                }

                if (Parametros.General.SystemOptionAcces(Parametros.General.UserID, "bntFinalizarAjusteInventario"))
                    {
                        this.btnRecalc.Visible = true;

                        if (ShowFinish)
                            this.bntFinalizarAjusteInventario.Enabled = true;
                    }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (ToPrint)
                {
                    //IMPRESION DEL COMPROBANTE

                    List<Entidad.ComprobanteContable> CD = db.ComprobanteContables.Where(o => o.MovimientoID.Equals(EntidadAnterior.ID)).ToList();

                    rplkEstacion.DataSource = db.EstacionServicios.Select(s => new { s.ID, s.Nombre });
                    rpCeco.DataSource = db.CentroCostos.Select(s => new { s.ID, Display = s.Nombre });

                    var obj = (from cds in CD
                               join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                               join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                               join cto in db.CentroCostos on cds.CentroCostoID equals cto.ID into ceco
                               from Centros in ceco.DefaultIfEmpty()
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

                    this.bdsCD.DataSource = obj;

                    gridComprobante.Visible = true;
                    bgvComprobante.RefreshData();

                    //if ((obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))).Equals(0))
                    //{
                    //    this.lblDiferencia.Text = "SIN DIFERENCIAS";
                    //}
                    //else
                    //{
                    //    lblDiferencia.ForeColor = Color.Red;
                    //    this.lblDiferencia.Text = "DIFERENCIAS  " + (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))).ToString("#,0.00");
                    //}

                    //using (Contabilidad.Forms.Dialogs.DialogShowComprobante nf = new Contabilidad.Forms.Dialogs.DialogShowComprobante())
                    //{
                    //    nf.DetalleCD = db.ComprobanteContables.Where(o => o.MovimientoID.Equals(EntidadAnterior.ID)).ToList();
                    //    nf.Text = "Comprobante Contable de Ajuste";
                    //    gridComprobante = nf.gridComprobante;
                    //}
                    //this.grid.Dock = DockStyle.None;
                    //this.splitContainerControlMain.Panel1.Controls.Add(gridComprobante);
                    
                    layoutControlItemAlmacen.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemInfo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    gridComprobante.Visible = true;


                    var mostrar = (from iv in db.InventarioFisicos
                                     join a in db.Areas on iv.AreaID equals a.ID
                                     where iv.ID.Equals(IDInventarioFisico)
                                     select new 
                                     {
                                         iv.Numero,
                                         a.Nombre
                                     }).ToList();

                    if (mostrar != null)
                        txtInfo.Text = String.Format("{0:000000000} | {1}", mostrar.First().Numero, mostrar.First().Nombre);

                    var Elaborado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(EntidadAnterior.EstacionServicioID)).ResponsableContableID)));
                    var Revisado = db.Empleados.FirstOrDefault(em => em.ID.Equals(Convert.ToInt32(db.EstacionServicios.Single(es => es.ID.Equals(EntidadAnterior.EstacionServicioID)).AdministradorID)));


                    if (Elaborado != null)
                        lblElaborado.Text = Elaborado.Nombres + " " + Elaborado.Apellidos;
                    else
                        lblElaborado.Text = "<>..........................";

                    if (Revisado != null)
                        lblRevisado.Text = Revisado.Nombres + " " + Revisado.Apellidos;                       
                    else
                        lblRevisado.Text = "<>..........................";

                    layoutControlBottom.Visible = true;

                    this.layoutControlGroup1.Text += " " + db.EstacionServicios.Single(es => es.ID.Equals(EntidadAnterior.EstacionServicioID)).Nombre +
                                        (EntidadAnterior.SubEstacionID.Equals(0) ? "" : " | " + db.SubEstacions.Single(s => s.ID.Equals(EntidadAnterior.SubEstacionID)).Nombre);


                    var Es = db.EstacionServicios.FirstOrDefault(f => f.ID.Equals(EntidadAnterior.EstacionServicioID));
                    var Ses = db.SubEstacions.FirstOrDefault(f => f.ID.Equals(EntidadAnterior.SubEstacionID));
                    string _Fecha = Convert.ToDateTime(db.GetDateServer()).ToString();

                    emptyFecha.Text = (String.IsNullOrEmpty(_Fecha) ? "" : _Fecha);
                                        
                    bgvProductos.Columns["Numero"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;

                    this.Activate();
                    ImprimirActa("Ajuste de Inventario " + Fecha.ToShortDateString(), this.MdiParent, null, layoutControlTop, layoutControlBottom, btnRecost, gridComprobante, (Es != null ? Es.Nombre : ""), (Ses != null ? Ses.Nombre : ""), _Fecha);
                    layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    layoutControlItemAjustes.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    ///////////////////////////

                    this.Close();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                dtInventario.Clear();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                this.Close();
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar(false)) return;

            this.Close();
        }

        private void bntFinalizar_Click(object sender, EventArgs e)
        {
            //ImprimirActa();
            BloquearInventario = true;

            if (!Guardar(true)) return;

            this.Close();
        }

        private void ImprimirActa( string TitleForm
           , System.Windows.Forms.Form mdi
           , DevExpress.XtraPivotGrid.PivotGridControl pivotGridMargin
           , DevExpress.XtraLayout.LayoutControl layOutTop
            , DevExpress.XtraLayout.LayoutControl layOutBottom
           , DevExpress.XtraGrid.GridControl gridcontrol
           , DevExpress.XtraGrid.GridControl gridCD
            ,string Estacion, string SubEstacion, string _Fecha)        
        {
            try
            {
                #region ::: Declaraciones :::
                DevExpress.XtraPrinting.PrintingSystem ps = new DevExpress.XtraPrinting.PrintingSystem();
                DevExpress.XtraPrintingLinks.CompositeLink compositeLink = new DevExpress.XtraPrintingLinks.CompositeLink(ps);
                DevExpress.XtraPrinting.PrintableComponentLink pclgrid = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridCD = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclgridpiv = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclLaoyoutTop = new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PrintableComponentLink pclLaoyoutBottom= new DevExpress.XtraPrinting.PrintableComponentLink();
                DevExpress.XtraPrinting.PageHeaderFooter phf = (DevExpress.XtraPrinting.PageHeaderFooter)compositeLink.PageHeaderFooter;
                string SaltoLinea = System.Environment.NewLine;
                string param_Empresa = "", param_direccion = "", param_tel = "";
                System.Drawing.Image picture_LogoEmpresa = null;
                System.Windows.Forms.SaveFileDialog dglExportToFile = new System.Windows.Forms.SaveFileDialog();
                #endregion

                #region ::: Datos de la Empresa :::
                Parametros.General.GetCompanyData(out param_Empresa,
                out param_direccion, out  param_tel, out picture_LogoEmpresa);

                string direccionResult = "", direccionTmp = "";
                if (!String.IsNullOrEmpty(param_direccion))
                {
                    string[] direccionSplit = param_direccion.Split(' ');

                    foreach (var i in direccionSplit)
                    {
                        direccionTmp += " " + i;
                        if (direccionTmp.Length >= 75)
                        {
                            direccionResult += SaltoLinea + i;
                            direccionTmp = "";
                        }
                        else direccionResult += " " + i;

                    }
                }

                #endregion

                #region ::: Creando Imagenes de Header y Fooder :::
                System.Windows.Forms.ImageList pict = new System.Windows.Forms.ImageList();
                if (picture_LogoEmpresa != null)
                {
                    pict.Images.Add(picture_LogoEmpresa);
                    pict.ImageSize = new Size(142, 67);

                    compositeLink.Images.Add(pict.Images[0]);
                }

                #endregion

                #region ::: Configuracion del reporte :::
                phf.Header.Content.Clear();
                phf.Header.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                phf.Header.Content.AddRange(new string[] { "[Image 0]", param_Empresa + SaltoLinea + Estacion + (String.IsNullOrEmpty(SubEstacion) ? "" : " | " + SubEstacion) + SaltoLinea + " " + SaltoLinea + TitleForm });
                phf.Footer.Content.AddRange(new string[] { "[Image 1]", "Software de Administración de Gasolinera"});

                pclLaoyoutTop.Component = layOutTop;
                pclLaoyoutBottom.Component = layOutBottom;
                pclgrid.Component = gridcontrol;
                pclgridCD.Component = gridCD;
                pclgridpiv.Component = pivotGridMargin;

                Link l2 = new Link();
                l2.CreateDetailArea += new CreateAreaEventHandler(l2_CreateDetailArea);

                Link l = new Link();
                l.CreateDetailArea += new CreateAreaEventHandler(l_CreateDetailArea);
                
                compositeLink.Links.Add(pclLaoyoutTop);                
                compositeLink.Links.Add(pclgrid);
                compositeLink.Links.Add(l);
                compositeLink.Links.Add(pclLaoyoutBottom); 
                compositeLink.Links.Add(l2);
                compositeLink.Links.Add(pclgridCD);
                compositeLink.Links.Add(l);
                compositeLink.Links.Add(pclLaoyoutBottom); 
                compositeLink.Links.Add(pclgridpiv);
                compositeLink.CreateDocument();

                

                ps.PageSettings.Landscape = true;
                ps.PageSettings.LeftMargin = 20;
                ps.PageSettings.RightMargin = 10;
                ps.PageSettings.TopMargin = 100;
                ps.PageSettings.BottomMargin = 10;


                #endregion

                #region ::: Ejecucion del Reporte :::
                Parametros.Forms.FormReportViewer rForm = new Parametros.Forms.FormReportViewer();
                rForm.Text = TitleForm;
                rForm.MdiParent = mdi;

                ps.Document.ScaleFactor = 0.63f;
                rForm.printcontrolAreaReport.PrintingSystem = ps;
                
                rForm.Show();
                rForm.Activate();
                rForm.BringToFront();
                rForm.TopMost = true;
                
                #endregion
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        void l2_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {
            e.Graph.PrintingSystem.InsertPageBreak(0);
        }

        void l_CreateDetailArea(object sender, CreateAreaEventArgs e)
        {

            TextBrick tb = new TextBrick();
            tb.BackColor = Color.Transparent;
            tb.BorderColor = Color.Transparent;
            tb.Rect = new RectangleF(0, 0, 100, 200);
            e.Graph.DrawBrick(tb);
        }

        //Envento despues del cierre del formulario
        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, _SendPrint, _ID, REditID);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {            
            if (!_Guardado && !ToPrint)
            {
                if (REditID.Equals(0))
                {
                    if (btnLoad.Enabled.Equals(false) || lkInventario.EditValue != null)
                    {
                        if (Parametros.General.DialogMsg("El Ajuste de Inventario actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }

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
        
        private void bgvProductos_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colNumero)
                {
                    int lista = 0;

                    if (bgvProductos.GetRowCellValue(e.RowHandle, "Numero") != DBNull.Value)
                        lista = Convert.ToInt32(bgvProductos.GetRowCellValue(e.RowHandle, "Numero"));

                    if (lista > 0)
                    {
                        if (listadoNumero.OrderByDescending(o => o.ID).First().ID.Equals(lista))
                        {
                            lista++;

                            listadoNumero.Add(new Parametros.ListIdDisplay { ID = lista, Display = lista.ToString() });
                        }
                    }

                    bgvProductos.SetRowCellValue(e.RowHandle, "CantidadCompensada", 0);
                                        
                }

                if (e.Column == colCantidadCompensada)
                {
                    if (bgvProductos.GetRowCellValue(e.RowHandle, "CantidadCompensada") != DBNull.Value)
                    {
                        if (bgvProductos.GetRowCellValue(e.RowHandle, "Numero") != DBNull.Value)
                        {
                            if (Convert.ToInt32(bgvProductos.GetRowCellValue(e.RowHandle, "Numero")).Equals(0))
                                bgvProductos.SetRowCellValue(e.RowHandle, "CantidadCompensada", null);

                                decimal Costo = 0, Diferencias = 0, CostoInicial = 0, Cantidad = 0, CostoFinal = 0;

                                if (bgvProductos.GetRowCellValue(e.RowHandle, "Costo") != DBNull.Value)
                                    Costo = Convert.ToDecimal(bgvProductos.GetRowCellValue(e.RowHandle, "Costo"));

                                if (bgvProductos.GetRowCellValue(e.RowHandle, "Total") != DBNull.Value)
                                    Diferencias = Convert.ToDecimal(bgvProductos.GetRowCellValue(e.RowHandle, "Total"));

                                CostoInicial = Math.Abs(Decimal.Round(Costo * Diferencias, 2, MidpointRounding.AwayFromZero));

                                if (bgvProductos.GetRowCellValue(e.RowHandle, "CantidadCompensada") != DBNull.Value)
                                    Cantidad = Convert.ToDecimal(bgvProductos.GetRowCellValue(e.RowHandle, "CantidadCompensada"));

                                CostoFinal = Decimal.Round(Cantidad * Costo, 2, MidpointRounding.AwayFromZero);

                                if ((CostoFinal > CostoInicial) || (Cantidad > Math.Abs(Diferencias)))
                                {
                                    Parametros.General.DialogMsg("La cantidad de la compensación es mayor a la Diferencia", Parametros.MsgType.warning);
                                    bgvProductos.SetRowCellValue(e.RowHandle, "CantidadCompensada", 0);
                                }
                                else
                                {
                                    if (Diferencias < 0)
                                    {
                                        CostoInicial *= -1;
                                        bgvProductos.SetRowCellValue(e.RowHandle, "Faltante", CostoInicial + CostoFinal);
                                        CostoFinal *= -1;
                                    }
                                    else
                                    {
                                        bgvProductos.SetRowCellValue(e.RowHandle, "Sobrante", CostoInicial - CostoFinal);
                                    }

                                    bgvProductos.SetRowCellValue(e.RowHandle, "Compensacion", CostoFinal);
                                }

                            //}
                            //else
                            //{
                            //    Parametros.General.DialogMsg("La fila seleccionada esta asignada con la compensación N/A", Parametros.MsgType.warning);
                            //    bgvProductos.SetRowCellValue(e.RowHandle, "CantidadCompensada", null);
                            //}
                        }
                        else
                        {
                            Parametros.General.DialogMsg("Debe escoger el Numero de Compensación", Parametros.MsgType.warning);
                            bgvProductos.SetRowCellValue(e.RowHandle, "CantidadCompensada", null);
                        }
                    }

                    bgvProductos.RefreshData();
                    MostrarConceptos();
                }

                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void bgvProductos_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Compensaciones
            if (view.GetRowCellValue(RowHandle, "Numero") != DBNull.Value)
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Numero")) > 0)
                {
                    if (view.GetRowCellValue(RowHandle, "CantidadCompensada") != DBNull.Value)
                    {
                        if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadCompensada")) == 0)
                        {
                            view.SetColumnError(view.Columns["CantidadCompensada"], "Debe Ingresar la Cantidad a Compesar");
                            e.ErrorText = "Debe Ingresar la Cantidad a Compesar";
                            e.Valid = false;
                        }
                    }
                    else
                    {
                        view.SetColumnError(view.Columns["CantidadCompensada"], "Debe Ingresar la Cantidad a Compesar");
                        e.ErrorText = "Debe Ingresar la Cantidad a Compesar";
                        e.Valid = false;
                    }
                }
                else
                    view.SetRowCellValue(RowHandle, "CantidadCompensada", null);
            }
        }

        private void bgvProductos_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        #region <<< COMPROBANTE_INFERIOR >>>

        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
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
        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
             #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    gvDetalle.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    gvDetalle.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Linea", 0);

                    
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

            #endregion
        }

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
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
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
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                    }
                }
            }
        }

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

        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {

        }

        #endregion

        private void dateFecha_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);

        }
        
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                {
                    nf.DetalleCD = PartidasContable;
                    nf.Text = "Comprobante Contable de Ajuste";
                    nf.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //Validar filas del Grid Concepto
        private void gvDataConcepto_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;

            //-- Validar Columna de Concepto             
            if (view.GetRowCellValue(RowHandle, "ConceptoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ConceptoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ConceptoID"], "Debe Seleccionar un Concepto");
                    e.ErrorText = "Debe Seleccionar un Concepto";
                    e.Valid = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ConceptoID"], "Debe Seleccionar un Concepto");
                e.ErrorText = "Debe Seleccionar un Concepto";
                e.Valid = false;
            }

            //-- Validar Columna de Monto
            //--
            if (view.GetRowCellValue(RowHandle, "Monto") != null)
            {
                if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "Monto")) <= 0.00)
                {

                    view.SetColumnError(view.Columns["Monto"], "El Monto debe ser mayor a cero");
                    e.ErrorText = "El Monto debe ser mayor a cero";
                    e.Valid = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Monto"], "El Monto debe ser mayor a cero");
                e.ErrorText = "El Monto debe ser mayor a cero";
                e.Valid = false;
            }
        }

        //Mostra mensaje de validación del grid concepto

        private void gvDataConcepto_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }
        
        private void gvDataConcepto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }
            if (e.KeyCode == Keys.Delete)
            {

                if (gvDataConcepto.FocusedRowHandle >= 0)
                {
                    int id = Convert.ToInt32(gvDataConcepto.GetFocusedRowCellValue("ConceptoID"));
                    decimal vMonto = Convert.ToDecimal(gvDataConcepto.GetFocusedRowCellValue("Monto"));

                    DetalleAC.Remove(DetalleAC.Where(o => o.ConceptoID.Equals(id) && o.Monto.Equals(vMonto)).FirstOrDefault());

                    gvDataConcepto.RefreshData();
                }

            }
        }

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
            
            return base.ProcessCmdKey(ref msg, keyData);
        }        

        private void lkInventario_EditValueChanged(object sender, EventArgs e)
        {
            if (lkInventario.EditValue != null)
            {
                if (ObjInventario.Count > 0 && !Editable)
                {
                    if (ObjInventario.Single(s => s.ID.Equals(Convert.ToInt32(lkInventario.EditValue))).Fecha.Date > ObjInventario.GroupBy(g => g.Fecha).OrderBy(o => o.Key).First().Key.Date)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar el Inventario Físico más antiguo.", Parametros.MsgType.warning);
                        lkInventario.EditValue = null;
                    }
                    else
                        Fecha = ObjInventario.Single(s => s.ID.Equals(Convert.ToInt32(lkInventario.EditValue))).Fecha.Date;
                }
            }
        }

        #endregion

        #region <<< RECOSTEO
        
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Inventario != null)
            {
                //Ultima Ajuste
                var vLastAjuste = (from m in db.Movimientos
                                   where m.MovimientoTipoID.Equals(9) && m.EstacionServicioID.Equals(IDEstacionServicio) && m.SubEstacionID.Equals(IDSubEstacion) && m.Finalizado && m.AreaID.Equals(Inventario.AreaID)
                                   select new { m.FechaRegistro, m.ID }).OrderByDescending(o => o.FechaRegistro).FirstOrDefault();

                if (vLastAjuste != null)
                {
                    if (ValidarRecosteo(Inventario.FechaInventario))
                    {
                        List<Parametros.ListIdDisplay> P = (from p in db.Productos
                                                           join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                                           join ar in db.Areas on pc.AreaID equals ar.ID
                                                           where ar.ID.Equals(Inventario.AreaID)
                                                           select new Parametros.ListIdDisplay
                                                           { 
                                                               ID = p.ID, 
                                                               Display = p.Codigo + " | " + p.Nombre 
                                                           }).ToList();

                        if (Parametros.General.DialogMsg("Se realizará el proceso de Recostear para " + P.Count + " Productos esto tomara varios minutos." + Environment.NewLine + "¿Desea Proceder?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                            db.CommandTimeout = 900;
                            bool ReOpen = false;

                            //Contador
                            int x = 1;
                            foreach (var vP in P)
                            {
                                try
                                {
                                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), x.ToString() + "/" + P.Count.ToString() + "...Recosteando " + vP.Display.Substring(0, 10).ToString());
                                    db.SetRecosteoByProducto(vP.ID, IDEstacionServicio, IDSubEstacion, vLastAjuste.FechaRegistro.AddDays(1).Date, Fecha, UsuarioID);
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
                                RefreshMDI = false;
                                _SendPrint = false;
                                _Guardado = false;
                                REditID = (EntidadAnterior != null ? EntidadAnterior.ID : 0);
                                this.Close();
                            }
                        }
                    }
                }
                else
                {
                    if (ValidarRecosteo(Inventario.FechaInventario))
                    {
                        List<Parametros.ListIdDisplay> P = (from p in db.Productos
                                                            join pc in db.ProductoClases on p.ProductoClaseID equals pc.ID
                                                            join ar in db.Areas on pc.AreaID equals ar.ID
                                                            where ar.ID.Equals(Inventario.AreaID)
                                                            select new Parametros.ListIdDisplay
                                                            {
                                                                ID = p.ID,
                                                                Display = p.Codigo + " | " + p.Nombre
                                                            }).ToList();

                        if (Parametros.General.DialogMsg("Se realizará el proceso de Recostear para " + P.Count + " Productos esto tomara varios minutos." + Environment.NewLine + "¿Desea Proceder?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                            db.CommandTimeout = 900;
                            bool ReOpen = false;

                            //Contador
                            int x = 1;
                            foreach (var vP in P)
                            {
                                try
                                {
                                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), x.ToString() + "/" + P.Count.ToString() + "...Recosteando " + vP.Display.Substring(0, 10).ToString());
                                    db.SetRecosteoByProducto(vP.ID, IDEstacionServicio, IDSubEstacion, new DateTime(Inventario.FechaInventario.Year, Inventario.FechaInventario.Month, 1), Fecha, UsuarioID);
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
                                RefreshMDI = false;
                                _SendPrint = false;
                                _Guardado = false;
                                REditID = (EntidadAnterior != null ? EntidadAnterior.ID : 0);
                                this.Close();
                            }
                        }
                    }
                }
            }
        }

        #endregion

    }
}