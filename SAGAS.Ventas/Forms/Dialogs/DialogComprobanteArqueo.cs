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
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Ventas.Forms.Dialogs
{
	public partial class DialogComprobanteArqueo : Form
	{
		#region *** DECLARACIONES ***

		private Entidad.SAGASDataClassesDataContext db;
		private Entidad.SAGASDataViewsDataContext dbView;
		internal Forms.FormComprobanteArqueo MDI;
		internal int CuentaMonedaNacional;
		internal int CuentaMonedaExtranjera;
		internal int CuentaPorCobrarEmpleado;
		internal int CuentaSobrante;
		internal int CuentaCobrarCompaniaGrupo;
		internal int ManejoID;
		public Entidad.ResumenDia RD;
		internal decimal _TipoCambio;
		private DataTable dtProductos = new DataTable();
		internal int EstacionServicioID;
		internal int SubEstacionID;
		internal int ArqueadorID;
		internal DateTime Fecha;
		internal string Estacion;
		internal string SubEstacion;
		internal int UsuarioID = Parametros.General.UserID;
		internal DateTime FechaServidor;
		internal int MonedaID;
		internal int AreaID;
		internal bool RefreshMDI = false;
		internal bool ShowMsg = false;
		internal int IDRD = 0;
		internal int ConceptoGanancia;
		internal int ConceptoPerdida;
		internal decimal _MargenAjusteComprobanteArqueo;
		internal bool _HasDiferenciado = false;

		//internal Forms.FormZona MDI;
		//internal Entidad.Zona EntidadAnterior;
		//internal bool Editable = false;
		//private bool ShowMsg = false;
		private List<Entidad.ComprobanteContable> CDVC = new List<Entidad.ComprobanteContable>();
		private List<Entidad.ComprobanteContable> CDCup = new List<Entidad.ComprobanteContable>();
		private List<Entidad.ComprobanteContable> CDPetro = new List<Entidad.ComprobanteContable>();
		private List<Entidad.Movimiento> MaestroM = new List<Entidad.Movimiento>();
		private List<Entidad.Kardex> MaestroK = new List<Entidad.Kardex>();
		private List<Entidad.Movimiento> DetalleM = new List<Entidad.Movimiento>();
		private List<Parametros.ListIdDisplayValueBooleano> Diferencias = new List<Parametros.ListIdDisplayValueBooleano>();
				 
		#endregion

		#region *** INICIO ***

		public DialogComprobanteArqueo()
		{
			InitializeComponent();
			
		}

		private void DialogUser_Load(object sender, EventArgs e)
		{
			
		}

		private void DialogComprobanteArqueo_Shown(object sender, EventArgs e)
		{
			FillControl();
			ValidarerrRequiredField();
		}

		#endregion

		#region *** METODOS ***

		//Llenado de datos
		private void FillControl()
		{
			try
			{
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
				
				db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
				dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
				db.CommandTimeout = 500;
				dbView.CommandTimeout = 500;

				FechaServidor = Convert.ToDateTime(db.GetDateServer());
				MonedaID = Parametros.Config.MonedaPrincipal();
				AreaID = db.AreaVentas.First(a => a.EsCombustible).ID;
				CuentaMonedaNacional = Parametros.Config.CajaMonedaNacional();
				CuentaMonedaExtranjera = Parametros.Config.CajaMonedaExtranjera();
				CuentaPorCobrarEmpleado = Parametros.Config.CuentaPorCobrarEmpleado();
				CuentaSobrante = Parametros.Config.CuentaSobranteArqueo();
				CuentaCobrarCompaniaGrupo = Parametros.Config.CuentaCobrarCompaniaGrupo();
				ManejoID = Parametros.Config.TipoExtraccionManejoID();
				ConceptoGanancia = 85;
				ConceptoPerdida = 86;
				_MargenAjusteComprobanteArqueo = Parametros.Config.MargenAjusteComprobanteArqueo();
				lkCeco.DataSource = db.CentroCostos.Where(c => c.Activo).Select(s => new { s.ID, s.Nombre });

				EstacionServicioID = RD.EstacionServicioID;
				Estacion = (!EstacionServicioID.Equals(0) ? db.EstacionServicios.Single(es => es.ID.Equals(EstacionServicioID)).Nombre : "");
				
				SubEstacionID = RD.SubEstacionID;
				SubEstacion = (!SubEstacionID.Equals(0) ? db.SubEstacions.Single(es => es.ID.Equals(SubEstacionID)).Nombre : "");

				if (!SubEstacionID.Equals(0))
				{
					ArqueadorID = db.SubEstacions.Single(s => s.ID.Equals(SubEstacionID)).ArqueadorID;
				}
				else
				{
					ArqueadorID = db.EstacionServicios.Single(s => s.ID.Equals(EstacionServicioID)).ArqueadorID;
				}


				Fecha = RD.FechaInicial.Date;

				_TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(RD.FechaInicial.Date)) > 0 ?
						db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(RD.FechaInicial.Date)).First().Valor : 0m);

				layoutControlGroupTop.Text = "Datos del Arqueo  " + Estacion + "  |  " + SubEstacion;

				lbResumen.Items.Add(Estacion + "  |  " + SubEstacion);
				lbResumen.Items.Add("Nro Resumen Dia | " + RD.Numero.ToString());
				lbResumen.Items.Add("Fecha                   | " + RD.FechaInicial.ToShortDateString());
				lbResumen.Items.Add("T / C Oficial          | " + _TipoCambio.ToString("#,0.0000"));
				lbResumen.Items.Add("T / C Pista            | " + RD.TipoCambioMoneda.ToString("#,0.0000"));

				lbFaltante.Items.Add(" FALTANTES ");

				//Tabla de Precios y costos
				//List<int> P = new List<int>();

				//dtProductos.Columns.Add("IDP", typeof(Int32));
				//dtProductos.Columns.Add("Producto", typeof(String));
				//dtProductos.Columns.Add("Costo", typeof(String));

				var Prod = (from ap in db.ArqueoProductos
						   join p in db.Productos on ap.ProductoID equals p.ID
						   join ai in db.ArqueoIslas on ap.ArqueoIslaID equals ai.ID
						   join t in db.Turnos on ai.TurnoID equals t.ID
						   join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
						   where rd.ID.Equals(RD.ID)
						   group ap by new { ap.ProductoID, p.Nombre, ap.Precio } into gr
						   select new
						   {
							   IDP = gr.Key.ProductoID,
							   Producto = gr.Key.Nombre,
							   Costo = Costo(gr.Key.ProductoID),
							   Precio = gr.Key.Precio,
							   Full = true
						   }).ToList();
				
				var HasDiferenciado = from m in db.Mangueras
									  join t in db.Tanques on m.TanqueID equals t.ID
									  where t.EstacionServicioID.Equals(RD.EstacionServicioID) && t.SubEstacionID.Equals(RD.SubEstacionID) && m.PrecioDiferenciado
									  select new {m.ID};

				if (HasDiferenciado.Count() > 0)
				{
					_HasDiferenciado = true;
					int n = Prod.Count;
					Prod.ToList().ForEach(p =>
						{
							Prod.Add(new { IDP = p.IDP, Producto = p.Producto, Costo = p.Costo, Precio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(p.IDP, RD.EstacionServicioID, RD.SubEstacionID, false, 0, false, Convert.ToDateTime(RD.FechaInicial).Date)), 2, MidpointRounding.AwayFromZero), Full = true });
							Prod.Add(new { IDP = p.IDP, Producto = p.Producto, Costo = p.Costo, Precio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(p.IDP, RD.EstacionServicioID, RD.SubEstacionID, true, 0, false, Convert.ToDateTime(RD.FechaInicial).Date)), 2, MidpointRounding.AwayFromZero), Full = false });
						});

					Prod.RemoveRange(0, n);
					colEsFull.Visible = true;
				}



				//Prod.ToList().ForEach(prod =>
				//    {

				//    });

				gridMain.DataSource = Prod;
				gvDataMain.RefreshData();

				FillVentasContado(db, dbView, RD.ID);
				FillCupones(db, dbView, RD.ID);
				FillPetrocard(db, dbView, RD.ID);
				FillManejo(db, dbView, RD.ID);
				FillClientes(db, dbView, RD.ID);
				FillAutoconsumo(db, dbView, RD.ID);

				if (Diferencias.Count > 0)
				{
					layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					var D = Diferencias.OrderBy(o => o.ID).First();
					layoutControlItemDif.Text = D.Display;
					spDif.Value = D.Value;

					if (Diferencias.Count(d => d.Booleano) > 0)
					{
						layoutControlItembtnAjuste.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
					}
				}

				//Pestaña Movimientos Notas                
				var Notas = from dn in DetalleM
							join mn in MaestroM on dn.MovimientoReferenciaID equals mn.ID
							join tm in db.MovimientoTipos on mn.MovimientoTipoID equals tm.ID
							join tmd in db.MovimientoTipos on dn.MovimientoTipoID equals tmd.ID
							join c in db.Clientes on dn.ClienteID equals c.ID into joinedC from cm in joinedC.DefaultIfEmpty()
							join e in db.Empleados on dn.EmpleadoID equals e.ID into joinedE from em in joinedE.DefaultIfEmpty()
							select new
							{
								TipoMovimiento = tm.Nombre,
								TipoMovimientodetalle = tmd.Nombre,
								Deudor = (dn.ClienteID.Equals(0) ? em.Nombres + " " + em.Apellidos : cm.Nombre),
								Monto = dn.Monto
							};

				gridNotas.DataSource = Notas.ToList();
				gvNotas.RefreshData();

				xtraTabControlMain.SelectedTabPage = xtraTabControlMain.TabPages[0];
				//if (Editable)
				//{
				//    Nombre = EntidadAnterior.Nombre;
				//}
				Parametros.General.splashScreenManagerMain.CloseWaitForm();   
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();   
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
			}

		}

		//COMPROBANTE VENTAS AL CONTADO
		private void FillVentasContado(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				decimal vMonto = 0;
				int linea = 1;
				var vista = dbView.VistaArqueoIslas.Where(ai => ai.ResumenDiaID.Equals(IDRD) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial)));
				List<Parametros.ListIdTidDisplayValue> Combustibles = new List<Parametros.ListIdTidDisplayValue>();
				
				var inico = from afd in db.ArqueoEfectivoDetalles
							join af in db.ArqueoEfectivos on afd.ArqueoEfectivoID equals af.ID
							join e in db.Efectivos on afd.EfectivoID equals e.ID
							into joinedT from e in joinedT.DefaultIfEmpty()
							where (db.Turnos.Where(t => t.ResumenDia.Equals(db.ResumenDias.Single(r => r.ID.Equals(IDRD)))).Any(tn => tn.ID.Equals(af.TurnoID)))
							group afd by new {e.MonedaID } into gr
							select new
							{
								Moneda = (gr.Key.MonedaID != null ? gr.Key.MonedaID : 0),
								Venta = gr.Sum(s => s.TotalEfectivo)
								//VentaUS = gr.Sum(s => s.VistaArqueoFormaPagos.Where(f => f.MonedaID.Equals(2)).Sum(v => v.Valor))

							};

				//sumar cheques en cordobas
				decimal ValorCS = Decimal.Round((inico.Count(v => v.Moneda.Equals(1)) > 0 ? inico.Where(v => v.Moneda.Equals(1)).First().Venta : 0m), 2, MidpointRounding.AwayFromZero);// Sum(s => s.VentaCS), 2, MidpointRounding.AwayFromZero);
				ValorCS += Decimal.Round((inico.Count(v => v.Moneda.Equals(0)) > 0 ? inico.Where(v => v.Moneda.Equals(0)).First().Venta : 0m), 2, MidpointRounding.AwayFromZero);
				decimal ValorUS = Decimal.Round((inico.Count(v => v.Moneda.Equals(2)) > 0 ? Decimal.Round((inico.Where(v => v.Moneda.Equals(2)).First().Venta * RD.TipoCambioMoneda), 2, MidpointRounding.AwayFromZero) : 0m), 2, MidpointRounding.AwayFromZero);
				//decimal prueba = dbView.VistaArqueoFormaPagos.Where(v => )

				//Cargar en monto en cordobo moneda nacional
				if (ValorCS > 0)
				{
					CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaMonedaNacional, Monto = ValorCS, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round(ValorCS / _TipoCambio, 2, MidpointRounding.AwayFromZero) , Descripcion = "Venta al Contado" });
					linea++;
					vMonto += ValorCS;
					DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = Parametros.Config.ClienteVentaConttadoID(), Monto = ValorCS, MovimientoReferenciaID = 1 });
				}

				//Cargar en monto en dolares moneda nacional
				if (ValorUS > 0)
				{
                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaMonedaExtranjera, Monto = ValorUS, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((ValorUS / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = "Depositos cta. Dólar" });
					linea++;
					vMonto += ValorUS;
					DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = Parametros.Config.ClienteVentaContadoMonedaExtrangeraID(), Monto = ValorUS, MovimientoReferenciaID = 1 });
				
				}

				//Lineas de Empleados
				decimal Sobrante = 0;
				vista.ToList().ForEach(emp =>
				{
					decimal Faltante = emp.DiferenciaRecibida + emp.SobranteFaltante;

					if (Faltante < 0)
					{
						lbFaltante.Items.Add(emp.TecnicoNombre + ", Turno: " + emp.TurnoNumero + ", Isla: " + emp.IslaNombre + " | Faltante : " + Faltante.ToString());
                        CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaPorCobrarEmpleado, Monto = Math.Abs(Faltante), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((Math.Abs(Faltante) / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = emp.TecnicoNombre + ", Turno: " + emp.TurnoNumero + ", Isla: " + emp.IslaNombre });
						linea++;
						vMonto += Math.Abs(Faltante);
						DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 27, EmpleadoID = db.ArqueoIslas.Single(ai => ai.ID.Equals(emp.ArqueoIslaID)).TecnicoID, Monto = Math.Abs(Faltante), MovimientoReferenciaID = 1 });
				
					}
					else
						Sobrante += Faltante;
				});

				if (Sobrante > 0)
				{
                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaSobrante, Monto = -Math.Abs(Sobrante), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round((Sobrante / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = "Sobrante de Arqueo" });
					linea++;
				}
				//else
				//{
				//    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaSobrante, Monto = -Math.Abs(Sobrante), Descripcion = "Sobrante de Arqueo" });
				//    linea++;
				//}

				//Suma de las diferencias recibidas
				vista.Sum(s => s.DiferenciaRecibida);
				decimal vDiferencia = vista.Count() > 0 ? vista.Sum(s => s.DiferenciaRecibida) : 0;

				///¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿¿ MANEJO SOBRANTE ????????????????????????
				if (vDiferencia > 0)
				{
					CDVC.Where(c => c.CuentaContableID.Equals(CuentaMonedaNacional)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(vDiferencia));
					DetalleM.Where(d => d.MovimientoTipoID.Equals(7) && d.ClienteID.Equals(Parametros.Config.ClienteVentaConttadoID()) && d.MovimientoReferenciaID.Equals(1)).ToList().ForEach(mov => mov.Monto += Math.Abs(vDiferencia));
					//DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = Parametros.Config.ClienteVentaConttadoID(), Monto = ValorCS, MovimientoReferenciaID = 1 });
				}
				//{
				//    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaSobrante, Monto = -Math.Abs(Sobrante), Descripcion = "Sobrante de Arqueo" });
				//    linea++;
				//}
				//else
				//    CDVC.Where(c => c.CuentaContableID.Equals(CuentaMonedaNacional)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Sobrante));

				var DiferenciaEfectivo = from af in db.ArqueoEfectivos
							where (db.Turnos.Where(t => t.ResumenDia.Equals(db.ResumenDias.Single(r => r.ID.Equals(IDRD)))).Any(tn => tn.ID.Equals(af.TurnoID)))
							select new
							{
								Saldo = af.Diferencia
							};

				if (DiferenciaEfectivo.Sum(s => s.Saldo) < 0)
				{
                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaPorCobrarEmpleado, Monto = Math.Abs(DiferenciaEfectivo.Sum(s => s.Saldo)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round((DiferenciaEfectivo.Sum(s => s.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = "Faltante Arqueo de Efectivo " + vista.First().ArquedaorNombre });
					linea++;
					DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 27, EmpleadoID = ArqueadorID, Monto = Math.Abs(DiferenciaEfectivo.Sum(s => s.Saldo)), MovimientoReferenciaID = 1 });
				}
				else
				{
					//if (CDVC.Count(c => c.CuentaContableID.Equals(CuentaSobrante)) > 0)
					//{
					//    CDVC.Where(c => c.CuentaContableID.Equals(CuentaSobrante)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(DiferenciaEfectivo.Sum(s => s.Saldo)));
					//    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaSobrante, Monto = -Math.Abs(DiferenciaEfectivo.Sum(s => s.Saldo)), Descripcion = "Sobrante Arqueo de Efectivo" });
					//    linea++;
					//}
					//else
					//{
                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaSobrante, Monto = -Math.Abs(DiferenciaEfectivo.Sum(s => s.Saldo)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round((DiferenciaEfectivo.Sum(s => s.Saldo) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = "Sobrante Arqueo de Efectivo" });
						linea++;
					//}
				}
				
				var Pagos = from fp in dbView.VistaArqueoFormaPagos
							join ai in dbView.VistaArqueoIslas on fp.ArqueoIslaID equals ai.ArqueoIslaID
							where ai.ResumenDiaID.Equals(IDRD) && fp.MonedaID.Equals(0) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
							group fp by new { fp.DeudorID, fp.CuentaContableID, ai.TurnoNumero, fp.PagoID } into gr
							select new
							{
								DeudorID = gr.Key.DeudorID,
								CuentaContableID = gr.Key.CuentaContableID,
								PagoID = gr.Key.PagoID,
								TurnoNumero = gr.Key.TurnoNumero,
								Valor = gr.Sum(s => s.Valor)
							};

				//Lineas de Formas de Pago
				//var Pagos = dbView.VistaArqueoFormaPagos.Where(fp => vista.Any(v => v.ArqueoIslaID.Equals(fp.ArqueoIslaID)) && fp.MonedaID.Equals(0)).GroupBy(g => new { g.DeudorID, g.CuentaContableID });
				Pagos.GroupBy(g => new { g.DeudorID, g.CuentaContableID }).ToList().ForEach(pago =>
				{
					bool EsMulti = db.ExtracionPagos.Single(ep => ep.ID.Equals(Convert.ToInt32(Pagos.Where(ps => ps.DeudorID.Equals(pago.Key.DeudorID) && ps.CuentaContableID.Equals(pago.Key.CuentaContableID)).First().PagoID))).ContabilizarPorTurnos;

					if (EsMulti)
					{
						Pagos.Where(ps => ps.DeudorID.Equals(pago.Key.DeudorID) && ps.CuentaContableID.Equals(pago.Key.CuentaContableID)).GroupBy(g => g.TurnoNumero).ToList().ForEach(det => 
							{
								string texto = "";
								int Turno = det.Key;
								int cuenta = 0;
								if (!pago.Key.DeudorID.Equals(0))
								{
									var C = db.Clientes.Single(s => s.ID.Equals(pago.Key.DeudorID));
									texto = C.Nombre + " Turno Nro. " + Turno.ToString();
									cuenta = C.CuentaContableID;
								}
								else if (!pago.Key.CuentaContableID.Equals(0))
								{
									if (pago.Key.CuentaContableID.Equals(CuentaCobrarCompaniaGrupo))
										texto = "DNP - Subsidio Turno Nro. " + Turno.ToString();

									cuenta = pago.Key.CuentaContableID;
								}

								decimal vMontoDet = Decimal.Round(Pagos.Where(ps => ps.DeudorID.Equals(pago.Key.DeudorID) && ps.CuentaContableID.Equals(pago.Key.CuentaContableID) && ps.TurnoNumero.Equals(det.Key)).Sum(s => s.Valor), 2, MidpointRounding.AwayFromZero);

                                CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = cuenta, Monto = vMontoDet, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((vMontoDet / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = texto });
								linea++;
								vMonto += vMontoDet;
								DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 27, ClienteID = (pago.Key.DeudorID.Equals(0) ? Parametros.Config.ClienteDNPSubsidio() : pago.Key.DeudorID), Monto = vMontoDet, MovimientoReferenciaID = 1 });
							});                            
					}
					else
					{
						string texto = "";
						int cuenta = 0;
						if (!pago.Key.DeudorID.Equals(0))
						{
							var C = db.Clientes.Single(s => s.ID.Equals(pago.Key.DeudorID));
							texto = C.Nombre;
							cuenta = C.CuentaContableID;
						}
						else if (!pago.Key.CuentaContableID.Equals(0))
						{
							if (pago.Key.CuentaContableID.Equals(CuentaCobrarCompaniaGrupo))
								texto = "DNP - Subsidio";

							cuenta = pago.Key.CuentaContableID;
						}

                        CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = cuenta, Monto = pago.Sum(s => s.Valor), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((pago.Sum(s => s.Valor) / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = texto });
						linea++;
						vMonto += pago.Sum(s => s.Valor);
						DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 27, ClienteID = (pago.Key.DeudorID.Equals(0) ? Parametros.Config.ClienteDNPSubsidio() : pago.Key.DeudorID), Monto = pago.Sum(s => s.Valor), MovimientoReferenciaID = 1 });
					}
				});

				//Descuento por Productos Dispensadores
				var Descuentos = from ap in db.ArqueoProductos
								 join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
								 join t in db.Turnos on ai.Turno equals t
								 join tn in db.Tanques on ap.TanqueID equals tn.ID
								 join rd in db.ResumenDias on t.ResumenDia equals rd
								 where rd.ID.Equals(IDRD) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
								 group ap by new { ap.ProductoID, ap.TanqueID, tn.Nombre, ap.Precio } into gr
								 select gr;
				
				decimal descPetro = 0;

				Descuentos.GroupBy(g => g.Key.ProductoID).ToList().ForEach(desc =>
				{
					decimal descuento = desc.Sum(s => s.Sum(sx => sx.DescuentoDispensador)) + desc.Sum(s => s.Sum(sx => sx.DescuentoEspecial));
					descPetro += desc.Sum(s => s.Sum(sx => sx.DescuentoGalonesPetroCardValor));

					if (descuento > 0)
					{
						var Prod = db.Productos.Single(p => p.ID.Equals(desc.Key));
                        CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaDescuentoID, Monto = descuento, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((descuento / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = "Descuento " + Prod.Nombre });
						linea++;
					}
				});

				//Descuento Petrocard                
				if (descPetro > 0)
				{
                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaCobrarCompaniaGrupo, Monto = descPetro, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((descPetro / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = "Descuento Petrocard" });
					linea++;
				}

				string Descripcion = "Venta de Contado";
				Descuentos.ToList().ForEach(desc =>
				{
					if (desc.Sum(s => s.VentaLitro) > 0)
					{
						var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ProductoID));
						if (Prod != null)
						{
							if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
							{
								if (!Prod.CuentaCostoID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}

								if (!Prod.CuentaInventarioID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Litros = desc.Sum(s => s.VentaLitro), Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}

								if (!Prod.CuentaVentaID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = -Math.Abs(desc.Sum(s => s.VentaValor)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round((desc.Sum(s => s.VentaValor) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}
							}
							else
							{
								if (!Prod.CuentaCostoID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
								}

								if (!Prod.CuentaInventarioID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
								}

								if (!Prod.CuentaVentaID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(desc.Sum(s => s.VentaValor)));
								}
							}

							vMonto += Decimal.Round(desc.Sum(s => s.VentaLitro), 2, MidpointRounding.AwayFromZero);
							Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TanqueID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.VentaLitro) });
							MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TanqueID, CantidadSalida = desc.Sum(s => s.VentaLitro), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => s.VentaValor), 2, MidpointRounding.AwayFromZero), MovimientoID = 1 });
						}
					}
					else if (desc.Sum(s => s.VentaLitro) < 0)
					{
						var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ProductoID));
						if (Prod != null)
						{
							if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
							{
								if (!Prod.CuentaCostoID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}

								if (!Prod.CuentaInventarioID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Litros = desc.Sum(s => s.VentaLitro), Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}

								if (!Prod.CuentaVentaID.Equals(0))
								{
                                    CDVC.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = Math.Abs(desc.Sum(s => s.VentaValor)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round((desc.Sum(s => s.VentaValor) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = Descripcion });
									linea++;
								}
							}
							else
							{
								if (!Prod.CuentaCostoID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
								}

								if (!Prod.CuentaInventarioID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.VentaLitro) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
								}

								if (!Prod.CuentaVentaID.Equals(0))
								{
									CDVC.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(desc.Sum(s => s.VentaValor)));
								}
							}

							vMonto += Decimal.Round(desc.Sum(s => s.VentaLitro), 2, MidpointRounding.AwayFromZero);
							Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TanqueID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.VentaLitro) });
							MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TanqueID, CantidadSalida = desc.Sum(s => s.VentaLitro), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => s.VentaValor), 2, MidpointRounding.AwayFromZero), MovimientoID = 1 });
						}
					}
				});

				gridVC.DataSource = Combustibles.Select(s => new { IDP = s.ID, Producto = s.Display, Litros = s.Value }).ToList();
				gvVC.RefreshData();

				var obj = (from cds in this.CDVC
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
							   cds.Linea,
							   CECO = cds.CentroCostoID,
							   cds.Litros
						   }).OrderBy(o => o.Linea);

				if (CDVC.Count > 0)
				{
					this.xtraTabPageVC.PageVisible = true;
					this.bsCCVC.DataSource = obj;
				}
				
				MaestroM.Add(new Entidad.Movimiento { MovimientoTipoID = 46, Monto = vMonto, ID = 1 });

				if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
				{
					Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 0, Display = "Diferencias Ventas Contado", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = true });                    
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}

		//COMPROBANTE VENTAS CUPONES
		private void FillCupones(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				if (!_HasDiferenciado)
				{
					int linea = 2;
					List<Parametros.ListIdTidDisplayValue> Combustibles = new List<Parametros.ListIdTidDisplayValue>();
					string text = "Ventas Cupones";

					List<Parametros.ListIdDisplayValue> cValores = new List<Parametros.ListIdDisplayValue>();


					//Lineas de Formas de Pago
					var Cupones = from ape in db.ArqueoProductoExtracions
								  join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
								  join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
								  join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
								  join tn in db.Tanques on ap.TanqueID equals tn.ID
								  join t in db.Turnos on ai.Turno equals t
								  join rd in db.ResumenDias on t.ResumenDia equals rd
								  where rd.ID.Equals(IDRD) && ex.EsCupon && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
								  group ape by new { ap.ProductoID, ap.Precio, ex.DeudorID, ap.TanqueID, ex.EsCuponEspecial, tn.Nombre } into gr
								  select new
								  {
									  ID = gr.Key.ProductoID,
									  TID = gr.Key.TanqueID,
									  Nombre = gr.Key.Nombre,
									  Valor = gr.Sum(s => s.Valor),
									  Precio = gr.Key.Precio,
                                      Especial = gr.Key.EsCuponEspecial,
									  //ExtraxionID = gr.Key.ExtraxionID,
									  ExtraxionDeudor = gr.Key.DeudorID
								  };

					decimal Total = 0;
					Cupones.GroupBy(g => new { g.ID, g.Precio, g.TID, g.Nombre, g.ExtraxionDeudor }).OrderBy(o => o.Key.ID).ToList().ForEach(desc =>
					{
						if (desc.Sum(s => s.Valor) > 0)
						{
							var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ID));
							if (Prod != null)
							{
								if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}
								}
								else
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)));
									}
								}

								Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.Valor) });
								MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TID, CantidadSalida = desc.Sum(s => s.Valor), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoID = 2 });
							}

							Total += Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
						}
					});

					if (Total > 0)
					{
                        CDCup.Add(new Entidad.ComprobanteContable { Linea = 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaCobrarCompaniaGrupo, Monto = Total, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((Total / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = text });
						linea++;
						MaestroM.Add(new Entidad.Movimiento { MovimientoTipoID = 47, Monto = Total, ID = 2 });

						Cupones.GroupBy(g => new { g.ExtraxionDeudor, g.Especial }).ToList().ForEach(det => DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = det.Key.ExtraxionDeudor, Monto = Decimal.Round(det.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoReferenciaID = 2 }));
					}


					gridCup.DataSource = Combustibles.Select(s => new { IDP = s.ID, Producto = s.Display, Litros = s.Value }).ToList();
					gvCup.RefreshData();

					var obj = (from cds in this.CDCup
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
								   cds.Linea,
								   cds.Litros
							   }).OrderBy(o => o.Linea);

					if (CDCup.Count > 0)
					{
						xtraTabPageCup.PageVisible = true;
						this.bsCCCup.DataSource = obj;
					}

					if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 1, Display = "Diferencias Ventas Cupones", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = false });
					}
				}
				else if (_HasDiferenciado)
				{
					#region TienenDiferenciado
					
					int linea = 2;
					List<Parametros.ListIdTidDisplayValue> Combustibles = new List<Parametros.ListIdTidDisplayValue>();
					string text = "Ventas Cupones";

					List<Parametros.ListIdDisplayValue> cValores = new List<Parametros.ListIdDisplayValue>();


					//Lineas de Formas de Pago
					var Cupones = from ape in db.ArqueoProductoExtracions
								  join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
								  join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
								  join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
								  join tn in db.Tanques on ap.TanqueID equals tn.ID
								  join t in db.Turnos on ai.Turno equals t
								  join rd in db.ResumenDias on t.ResumenDia equals rd
								  where rd.ID.Equals(IDRD) && ex.EsCupon && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
								  group ape by new { ap.ProductoID, ap.Precio, ex.DeudorID, ex.EsCuponEspecial, ap.TanqueID, tn.Nombre } into gr
								  select new
								  {
									  ID = gr.Key.ProductoID,
									  TID = gr.Key.TanqueID,
									  Nombre = gr.Key.Nombre,
									  Valor = gr.Sum(s => s.Valor),
                                      Especial = gr.Key.EsCuponEspecial,
									  Precio = Convert.ToDecimal(db.GetPrecio(gr.Key.ProductoID, RD.EstacionServicioID, RD.SubEstacionID, false, 0, false, Convert.ToDateTime(RD.FechaInicial).Date)),
									  //ExtraxionID = gr.Key.ExtraxionID,
									  ExtraxionDeudor = gr.Key.DeudorID
								  };

					decimal Total = 0;
					Cupones.GroupBy(g => new { g.ID, g.Precio, g.TID, g.Nombre, g.ExtraxionDeudor }).OrderBy(o => o.Key.ID).ToList().ForEach(desc =>
					{
						if (desc.Sum(s => s.Valor) > 0)
						{
							var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ID));
							if (Prod != null)
							{
								if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
                                        CDCup.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = text });
										linea++;
									}
								}
								else
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
										CDCup.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)));
									}
								}

								Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.Valor) });
								MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TID, CantidadSalida = desc.Sum(s => s.Valor), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoID = 2 });
							}

							Total += Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
						}
					});

					if (Total > 0)
					{
                        CDCup.Add(new Entidad.ComprobanteContable { Linea = 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaCobrarCompaniaGrupo, Monto = Total, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((Total / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = text });
						linea++;
						MaestroM.Add(new Entidad.Movimiento { MovimientoTipoID = 47, Monto = Total, ID = 2 });

						Cupones.GroupBy(g => new { g.ExtraxionDeudor, g.Especial }).ToList().ForEach(det => DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = det.Key.ExtraxionDeudor, Monto = Decimal.Round(det.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoReferenciaID = 2 }));
					}


					gridCup.DataSource = Combustibles.Select(s => new { IDP = s.ID, Producto = s.Display, Litros = s.Value }).ToList();
					gvCup.RefreshData();

					var obj = (from cds in this.CDCup
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
								   cds.Linea,
								   cds.Litros
							   }).OrderBy(o => o.Linea);

					if (CDCup.Count > 0)
					{
						xtraTabPageCup.PageVisible = true;
						this.bsCCCup.DataSource = obj;
					}

					if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 1, Display = "Diferencias Ventas Cupones", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = false });
					}
					#endregion
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}
		
		//COMPROBANTE VENTAS PETROCARD
		private void FillPetrocard(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				if (!_HasDiferenciado)
				{
					int linea = 2;
					List<Parametros.ListIdTidDisplayValue> Combustibles = new List<Parametros.ListIdTidDisplayValue>();
					string texto = "Ventas Petrocard";

					//Lineas de Formas de Pago
					var Cupones = from ape in db.ArqueoProductoExtracions
								  join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
								  join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
								  join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
								  join t in db.Turnos on ai.Turno equals t
								  join tn in db.Tanques on ap.TanqueID equals tn.ID
								  join rd in db.ResumenDias on t.ResumenDia equals rd
								  where rd.ID.Equals(IDRD) && ex.EsPetrocard && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
								  group ape by new { ap.ProductoID, ap.Precio, Extraxion = ex.ID, ap.TanqueID, tn.Nombre } into gr
								  select new
								  {
									  ID = gr.Key.ProductoID,
									  TID = gr.Key.TanqueID,
									  Nombre = gr.Key.Nombre,
									  Valor = gr.Sum(s => s.Valor),
									  Precio = gr.Key.Precio,
									  Extraxion = gr.Key.Extraxion
								  };

					decimal Total = 0;
					Cupones.GroupBy(g => new { g.ID, g.Precio, g.TID, g.Nombre }).OrderBy(o => o.Key.ID).ToList().ForEach(desc =>
					{
						if (desc.Sum(s => s.Valor) > 0)
						{
							var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ID));
							if (Prod != null)
							{
								if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
                                        CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
                                        CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
                                        CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}
								}
								else
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
										CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
										CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
                                        CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)));
                                        //CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)));
                                    }
								}
								Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.Valor) });
								MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TID, CantidadSalida = desc.Sum(s => s.Valor), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoID = 3 });
							}
							//Total += Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
							Total += Decimal.Round(desc.Sum(s => s.Valor * s.Precio), 2, MidpointRounding.AwayFromZero);
						}
					});

					if (Total > 0)
					{
						CDPetro.Add(new Entidad.ComprobanteContable { Linea = 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaCobrarCompaniaGrupo, Monto = Total, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round((Total / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
						linea++;
						MaestroM.Add(new Entidad.Movimiento { MovimientoTipoID = 48, Monto = Total, ID = 3 });
						Cupones.GroupBy(g => g.Extraxion).ToList().ForEach(det => DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = db.ExtracionPagos.Single(ep => ep.ID.Equals(det.Key)).DeudorID, Monto = Decimal.Round(det.Where(o => o.Extraxion.Equals(det.Key)).Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoReferenciaID = 3 }));
					}

					gridPetro.DataSource = Combustibles.Select(s => new { IDP = s.ID, Producto = s.Display, Litros = s.Value }).ToList();
					gvPetro.RefreshData();

					var obj = (from cds in this.CDPetro
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
								   cds.Linea,
								   cds.Litros
							   }).OrderBy(o => o.Linea);

					if (CDPetro.Count > 0)
					{
						xtraTabPagePetro.PageVisible = true;
						this.bsCCPetro.DataSource = obj;
					}

					if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 2, Display = "Diferencias Ventas Petrocard", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = false });
					}
				}
				else if (_HasDiferenciado)
				{
					#region TienenDiferenciado

					int linea = 2;
					List<Parametros.ListIdTidDisplayValue> Combustibles = new List<Parametros.ListIdTidDisplayValue>();
					string texto = "Ventas Petrocard";

					//Lineas de Formas de Pago
					var Cupones = from ape in db.ArqueoProductoExtracions
								  join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
								  join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
								  join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
								  join t in db.Turnos on ai.Turno equals t
								  join tn in db.Tanques on ap.TanqueID equals tn.ID
								  join rd in db.ResumenDias on t.ResumenDia equals rd
								  where rd.ID.Equals(IDRD) && ex.EsPetrocard && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
								  group ape by new { ap.ProductoID, ap.Precio, Extraxion = ex.ID, ap.TanqueID, tn.Nombre } into gr
								  select new
								  {
									  ID = gr.Key.ProductoID,
									  TID = gr.Key.TanqueID,
									  Nombre = gr.Key.Nombre,
									  Valor = gr.Sum(s => s.Valor),
									  Precio = Convert.ToDecimal(db.GetPrecio(gr.Key.ProductoID, RD.EstacionServicioID, RD.SubEstacionID, false, 0, false, Convert.ToDateTime(RD.FechaInicial).Date)),
									  Extraxion = gr.Key.Extraxion
								  };

					decimal Total = 0;
					Cupones.GroupBy(g => new { g.ID, g.Precio, g.TID, g.Nombre }).OrderBy(o => o.Key.ID).ToList().ForEach(desc =>
					{
						if (desc.Sum(s => s.Valor) > 0)
						{
							var Prod = db.Productos.SingleOrDefault(p => p.ID.Equals(desc.Key.ID));
							if (Prod != null)
							{
								if (!Combustibles.Select(s => s.ID).Contains(Prod.ID))
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
										CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaCostoID, Monto = Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
										CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaInventarioID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
										CDPetro.Add(new Entidad.ComprobanteContable { Linea = linea, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = Prod.CuentaVentaID, Monto = -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round(((Decimal.Round(desc.Sum(s => s.Valor) * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)) / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = texto });
										linea++;
									}
								}
								else
								{
									if (!Prod.CuentaCostoID.Equals(0))
									{
										CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaCostoID)).ToList().ForEach(apunte => apunte.Monto += Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaInventarioID.Equals(0))
									{
										CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaInventarioID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => s.Valor) * Costo(Prod.ID), 2, MidpointRounding.AwayFromZero)));
									}

									if (!Prod.CuentaVentaID.Equals(0))
									{
										CDPetro.Where(c => c.CuentaContableID.Equals(Prod.CuentaVentaID)).ToList().ForEach(apunte => apunte.Monto += -Math.Abs(Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero)));
									}
								}
								Combustibles.Add(new Parametros.ListIdTidDisplayValue { ID = Prod.ID, TID = desc.Key.TID, Display = Prod.Nombre + " => " + desc.Key.Nombre, Value = desc.Sum(s => s.Valor) });
								MaestroK.Add(new Entidad.Kardex { ProductoID = Prod.ID, UnidadMedidaID = Prod.UnidadMedidaID, AlmacenSalidaID = desc.Key.TID, CantidadSalida = desc.Sum(s => s.Valor), CostoSalida = Decimal.Round(Costo(Prod.ID), 4, MidpointRounding.AwayFromZero), Precio = desc.Key.Precio, PrecioTotal = Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * desc.Key.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoID = 3 });
							}

							Total += Decimal.Round(desc.Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero);
						}
					});

					if (Total > 0)
					{
						CDPetro.Add(new Entidad.ComprobanteContable { Linea = 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = CuentaCobrarCompaniaGrupo, Monto = Total, TipoCambio = _TipoCambio, MontoMonedaSecundaria = Decimal.Round((Total / _TipoCambio), 2, MidpointRounding.AwayFromZero), Descripcion = texto });
						linea++;
						MaestroM.Add(new Entidad.Movimiento { MovimientoTipoID = 48, Monto = Total, ID = 3 });
                        Cupones.GroupBy(g => g.Extraxion).ToList().ForEach(det => DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = db.ExtracionPagos.Single(ep => ep.ID.Equals(det.Key)).DeudorID, Monto = Decimal.Round(det.Where(o => o.Extraxion.Equals(det.Key)).Sum(s => Decimal.Round(s.Valor * s.Precio, 2, MidpointRounding.AwayFromZero)), 2, MidpointRounding.AwayFromZero), MovimientoReferenciaID = 3 }));
                        //Cupones.GroupBy(g => g.Extraxion).ToList().ForEach(det => DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, ClienteID = db.ExtracionPagos.Single(ep => ep.ID.Equals(det.Key)).DeudorID, Monto = Total, MovimientoReferenciaID = 3 }));
					}

					gridPetro.DataSource = Combustibles.Select(s => new { IDP = s.ID, Producto = s.Display, Litros = s.Value }).ToList();
					gvPetro.RefreshData();

					var obj = (from cds in this.CDPetro
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
								   cds.Linea,
								   cds.Litros
							   }).OrderBy(o => o.Linea);

					

					if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
					{
                        if (Math.Abs(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)) < 1)
                        {
                            var change = this.CDPetro.OrderByDescending(o => o.Monto).FirstOrDefault();
                            change.Monto -= Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero);

                            if (!(Decimal.Round(obj.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((obj.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                            {
                                Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 2, Display = "Diferencias Ventas Petrocard", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = false });
                            }
                        
                        }
                        else
                            Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 2, Display = "Diferencias Ventas Petrocard", Value = (obj.Sum(s => s.Debito) - (obj.Sum(s => s.Credito))), Booleano = false });
					}

                    if (CDPetro.Count > 0)
                    {
                        xtraTabPagePetro.PageVisible = true;
                        this.bsCCPetro.DataSource = obj;
                    }
					#endregion
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}

		//LISTA MANEJO
		private void FillManejo(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				//Lineas de Formas de Pago
				IQueryable<Parametros.ListIdTidDisplayPriceValue> ClientesArqueo = (from ape in db.ArqueoProductoExtracions
																			join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
																			join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
																			join p in db.Productos on ap.ProductoID equals p.ID
																			join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
																			join t in db.Turnos on ai.Turno equals t
																			join tn in db.Tanques on ap.TanqueID equals tn.ID
																			join rd in db.ResumenDias on t.ResumenDia equals rd
																			where rd.ID.Equals(IDRD) && ex.ID.Equals(ManejoID) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
																			group ape by new { ap.ProductoID, p.Nombre, ap.TanqueID, Tanque = tn.Nombre } into gr
																			select new Parametros.ListIdTidDisplayPriceValue
																			{
																				ID = gr.Key.ProductoID,
																				TID = gr.Key.TanqueID,
																				Display = gr.Key.Nombre + " => " + gr.Key.Tanque,
																				Value = gr.Sum(s => s.Valor)
																			}).OrderBy(o => o.ID);

				gridManejo.DataSource = ClientesArqueo.ToList();
				gvManejo.RefreshData();

				if (ClientesArqueo.Count() > 0)
				{
					this.xtraTabPageManejo.PageVisible = true;
					IQueryable<Parametros.ListIdRefTanqueDisplayPriceValue> ClientesMov = from vk in dbView.VistaKardexes.OrderBy(o => o.ProductoCodigo)
																						  join v in dbView.VistaMovimientos on vk.MovimientoID equals v.ID
																						  where v.ResumenDiaID.Equals(IDRD) && v.MovimientoTipoID.Equals(19) && !v.Anulado
																						  //group vk by new 
																						  //{ vk.MovimientoID, vk.ProductoNombre, vk.CantidadSalida,  } into gr
																						  select new Parametros.ListIdRefTanqueDisplayPriceValue
																						  {
																							  ID = v.ID,
																							  Tanque = vk.ProductoCodigo + " | " + vk.ProductoNombre + " => " + vk.AlmacenSalidaNombre,
																							  Ref = v.Referencia,
																							  Display = v.ClienteNombre,
																							  Value = vk.CantidadSalida,
																							  Price = 0m
																						  };
					
					gridManejoD.DataSource = ClientesMov.OrderBy(o => o.Tanque).ToList();
					gvManejoD.RefreshData();

					if (!((ClientesArqueo.Count() > 0 ? Decimal.Round(ClientesArqueo.Sum(s => s.Value), 2, MidpointRounding.AwayFromZero) : 0) - (ClientesMov.Count() > 0 ? Decimal.Round((ClientesMov.Sum(s => s.Value)), 2, MidpointRounding.AwayFromZero) : 0)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 3, Display = "Diferencias Comparación Manejo", Value = ((ClientesArqueo.Count() > 0 ? ClientesArqueo.Sum(s => s.Value) : 0) - (ClientesMov.Count() > 0 ? (ClientesMov.Sum(s => s.Value)) : 0)), Booleano = false });
					}
				}

			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}

		//LISTA CLIENTES
		private void FillClientes(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				//Lineas de Formas de Pago
				IQueryable<Parametros.ListIdTidDisplayPriceValue> ClientesArqueo = (from ape in db.ArqueoProductoExtracions
																					join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
																					join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
																					join p in db.Productos on ap.ProductoID equals p.ID
																					join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
																					join t in db.Turnos on ai.Turno equals t
																					join tn in db.Tanques on ap.TanqueID equals tn.ID
																					join rd in db.ResumenDias on t.ResumenDia equals rd
																					where rd.ID.Equals(IDRD) && ex.TipoComprobanteArqueo.Equals(Parametros.TipoComprobanteArqueo.Clientes) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
																					group ape by new { ap.ProductoID, p.Nombre, ap.TanqueID, Tanque = tn.Nombre } into gr
																					select new Parametros.ListIdTidDisplayPriceValue
																					 {
																						 ID = gr.Key.ProductoID,
																						 TID = gr.Key.TanqueID,
																						 Display = gr.Key.Nombre + " => " + gr.Key.Tanque,
																						 Value = gr.Sum(s => s.Valor)
																					 }).OrderBy(o => o.ID);

				gridCliente.DataSource = ClientesArqueo.ToList();
					gvCliente.RefreshData();

				if (ClientesArqueo.Count() > 0)
				{
					this.xtraTabPageClientes.PageVisible = true;
					IQueryable<Parametros.ListIdTIDTanqueDisplayPriceValue> ClientesMov = from vk in dbView.VistaKardexes.OrderBy(o => o.ProductoCodigo)
								  join v in dbView.VistaMovimientos on vk.MovimientoID equals v.ID
								  where v.ResumenDiaID.Equals(IDRD) && v.MovimientoTipoID.Equals(7) && !v.Anulado
								  //group vk by new 
								  //{ vk.MovimientoID, vk.ProductoNombre, vk.CantidadSalida,  } into gr
								  select new Parametros.ListIdTIDTanqueDisplayPriceValue
								  {
									  ID = v.ID,
									  Tanque = vk.ProductoCodigo + " | " + vk.ProductoNombre  + " => " + vk.AlmacenSalidaNombre,
									  TID = v.Numero,
									  Display = v.ClienteNombre,
									  Value = vk.CantidadSalida,
									  Price = 0m
								  };

					gridClienteD.DataSource = ClientesMov.OrderBy(o => o.Tanque).ToList();
					gvClienteD.RefreshData();

					if (!((ClientesArqueo.Count() > 0 ? Decimal.Round(ClientesArqueo.Sum(s => s.Value), 2, MidpointRounding.AwayFromZero) : 0) - (ClientesMov.Count() > 0 ?Decimal.Round((ClientesMov.Sum(s => s.Value)), 2, MidpointRounding.AwayFromZero) : 0)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 4, Display = "Diferencias Comparación Clientes", Value = ((ClientesArqueo.Count() > 0 ? ClientesArqueo.Sum(s => s.Value) : 0) - (ClientesMov.Count() > 0 ? (ClientesMov.Sum(s => s.Value)) : 0)), Booleano = false });
					}
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}

		//LISTA AUTOCONSUMO
		private void FillAutoconsumo(Entidad.SAGASDataClassesDataContext db, Entidad.SAGASDataViewsDataContext dbView, int IDRD)
		{
			try
			{
				//Lineas de Formas de Pago
				IQueryable<Parametros.ListIdTidDisplayPriceValue> ClientesArqueo = (from ape in db.ArqueoProductoExtracions
																			join ex in db.ExtracionPagos on ape.ExtracionID equals ex.ID
																			join ap in db.ArqueoProductos on ape.ArqueoProducto equals ap
																			join p in db.Productos on ap.ProductoID equals p.ID
																			join ai in db.ArqueoIslas on ap.ArqueoIsla equals ai
																			join t in db.Turnos on ai.Turno equals t
																			join tn in db.Tanques on ap.TanqueID equals tn.ID
																			join rd in db.ResumenDias on t.ResumenDia equals rd
																			where rd.ID.Equals(IDRD) && ex.TipoComprobanteArqueo.Equals(Parametros.TipoComprobanteArqueo.AutoConsumo) && (!ai.ArqueoEspecial || (ai.ArqueoEspecial && ai.Oficial))
																					group ape by new { ap.ProductoID, p.Nombre, ap.TanqueID, Tanque = tn.Nombre , ExtraxionID = ex.ID, ExtraxionNombre = ex.Nombre} into gr
																					select new Parametros.ListIdTidDisplayPriceValue
																			{
																				ID = gr.Key.ProductoID,
																				TID = gr.Key.TanqueID,
																				Display = gr.Key.ExtraxionNombre + " | " + gr.Key.Nombre + " => " + gr.Key.Tanque,
																				Value = gr.Sum(s => s.Valor)
																			}).OrderBy(o => o.ID);

				gridAutoconsumo.DataSource = ClientesArqueo.ToList();
				gvAutoconsumo.RefreshData();

				if (ClientesArqueo.Count() > 0)
				{
					this.xtraTabPageAutoConsumo.PageVisible = true;
					IQueryable<Parametros.ListIdTIDTanqueDisplayPriceValue> ClientesMov = from vk in dbView.VistaKardexes.OrderBy(o => o.ProductoCodigo)
																						  join v in dbView.VistaMovimientos on vk.MovimientoID equals v.ID
																						  where v.ResumenDiaID.Equals(IDRD) && v.MovimientoTipoID.Equals(25) && !v.Anulado
																						  //group vk by new 
																						  //{ vk.MovimientoID, vk.ProductoNombre, vk.CantidadSalida,  } into gr
																						  select new Parametros.ListIdTIDTanqueDisplayPriceValue
																						  {
																							  ID = v.ID,
																							  Tanque = vk.ProductoCodigo + " | " + vk.ProductoNombre  + " => " + vk.AlmacenSalidaNombre,
																							  TID = v.Numero,
																							  Display = v.ExtracionPagoNombre + (v.ExtracionConceptoNombre == null ? " " : " | " + v.ExtracionConceptoNombre),
																							  Value = vk.CantidadSalida,
																							  Price = 0m
																						  };

					gridAutoconsumoD.DataSource = ClientesMov.OrderBy(o => o.Tanque).ToList();
					gvAutoconsumoD.RefreshData();

					if (!((ClientesArqueo.Count() > 0 ? Decimal.Round(ClientesArqueo.Sum(s => s.Value), 2, MidpointRounding.AwayFromZero) : 0) -  (ClientesMov.Count() > 0 ? Decimal.Round((ClientesMov.Sum(s => s.Value)), 2, MidpointRounding.AwayFromZero) : 0)).Equals(0))
					{
						Diferencias.Add(new Parametros.ListIdDisplayValueBooleano { ID = 5, Display = "Diferencias Comparación Autoconsumo", Value = ((ClientesArqueo.Count() > 0 ? ClientesArqueo.Sum(s => s.Value) : 0) - (ClientesMov.Count() > 0 ? ClientesMov.Sum(s => s.Value) : 0)), Booleano = false });
					}
				}
			}
			catch (Exception ex)
			{
				Parametros.General.splashScreenManagerMain.CloseWaitForm();
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
				Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
			}
		}


		private decimal ValorFormula(string OriginalString)
		{
			try
			{

				string formula = "";
				if (OriginalString.StartsWith(@"=") || OriginalString.StartsWith(@"+"))
					formula = OriginalString.Substring(1);
				else
					formula = OriginalString;

				decimal Total = 0m;
				if (OriginalString.Contains(@"(") || OriginalString.Contains(@")"))
				{

					if (OriginalString.Contains(@"(") && OriginalString.Contains(@")") && (OriginalString.Contains(@"/") || OriginalString.Contains(@"*")))
					{
						formula = formula.Substring(1);
						string[] operacion = formula.Split(')');
						if (operacion.Count() > 2)
						{ Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }
						else
						{
							List<string> result = operacion[0].Split('+').ToList();

							foreach (var suma in result)
							{
								Total += FractionToDouble(suma);
							}

							if (operacion[1].StartsWith(@"/"))
							{
								return Decimal.Round((Total / Convert.ToDecimal(operacion[1].Substring(1))), 3, MidpointRounding.AwayFromZero);
							}
							else if (operacion[1].StartsWith(@"*"))
							{
								return Decimal.Round((Total * Convert.ToDecimal(operacion[1].Substring(1))), 3, MidpointRounding.AwayFromZero);
							}
							else { Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }

						}

					}
					else
					{ Parametros.General.DialogMsg(Parametros.Properties.Resources.OPERACIONMATEMATICAARQUEO + Environment.NewLine + @"Ejemplo: +(1+2+3)/2", Parametros.MsgType.warning); }
				}
				else
				{
					List<string> result = formula.Split('+').ToList();

					foreach (var suma in result)
					{
						Total += FractionToDouble(suma);
					}

					return Total;
				}

				return 0.000m;
			}
			catch
			{ return 0.000m; }
		}

		private decimal Costo(int IDProd)
		{
			try
			{
				decimal VCtoEntrada, vCtoFinal;
				Parametros.General.LastCost(db, EstacionServicioID, SubEstacionID, IDProd, 0, Fecha, out vCtoFinal, out VCtoEntrada);
				return vCtoFinal;
				//var obj = (from k in db.Kardexes
				//           join m in db.Movimientos on k.MovimientoID equals m.ID
				//           where k.ProductoID.Equals(IDProd) && m.MovimientoTipoID.Equals(3) && m.Anulado.Equals(false)
				//             && k.EstacionServicioID.Equals(EstacionServicioID) && k.SubEstacionID.Equals(SubEstacionID) && !k.CostoEntrada.Equals(0)
				//           select k).OrderByDescending(o => o.Fecha).ThenByDescending(d => d.ID).ToList();

				//if (obj.Count > 0)
				//    return obj.First().CostoEntrada;
				//else
				//    return 0;
			}
			catch{return 0;}
		}

		decimal FractionToDouble(string fraction)
		{
			decimal result;

			if (decimal.TryParse(fraction, out result))
			{
				return result;
			}

			string[] split = fraction.Split(new char[] { '*', '/' });

			if (split.Length == 2)
			{
				int a, b;


				if (int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
				{
					if (fraction.Contains(@"*"))
					{
						return (decimal)a * b;
					}

					if (fraction.Contains(@"/"))
					{
						return (decimal)a / b;
					}
				}
			}

			return 0m;
		}

		public bool ValidarCampos()
		{
			if ((xtraTabPageVC.PageVisible && String.IsNullOrEmpty(txtVentasContado.Text)) || (xtraTabPageCup.PageVisible && String.IsNullOrEmpty(txtCupones.Text)) || (xtraTabPagePetro.PageVisible && String.IsNullOrEmpty(txtPetrocard.Text)) || (xtraTabPageFleet.PageVisible && String.IsNullOrEmpty(txtFleet.Text)))
			{
				Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
				return false;
			}

			if (!Parametros.General.ValidatePeriodoContable(Fecha, db, EstacionServicioID))
			{
				Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
				return false;
			}

			if (Diferencias.Count > 0)
			{
				Parametros.General.DialogMsg(layoutControlItemDif.Text + Environment.NewLine + Decimal.Round(spDif.Value, 2, MidpointRounding.AwayFromZero).ToString("N2"), Parametros.MsgType.warning);
				return false;
			}

			return true;
		}

		private bool Guardar()
		{
			if (!ValidarCampos()) 
				return false;

			if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

			using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
			{
				db.Transaction = trans;
				try
				{
					Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
					//VentasContado
					#region <<< VENTAS CONTADO  >>>
					if (xtraTabPageVC.PageVisible)
					{
						//Identificar el Movimiento Ventas Contado Combustible
						var MovVcc = MaestroM.Single(m => m.ID.Equals(1));
						#region <<< REGISTRANDO MOVIMIENTO >>>
						Entidad.Movimiento MS = new Entidad.Movimiento();
						MS.ClienteID = 0;
						MS.MovimientoTipoID = MovVcc.MovimientoTipoID;
						MS.UsuarioID = UsuarioID;
						MS.FechaCreado = FechaServidor;
						MS.FechaRegistro = Fecha;
						MS.MonedaID = MonedaID;
						MS.TipoCambio = _TipoCambio;
						MS.Monto = MovVcc.Monto;
						MS.MontoMonedaSecundaria = Decimal.Round((MS.Monto / MS.TipoCambio), 2, MidpointRounding.AwayFromZero);
						MS.Numero = Convert.ToInt32(txtVentasContado.Text);
						MS.EstacionServicioID = EstacionServicioID;
						MS.SubEstacionID = SubEstacionID;
						MS.Comentario = memoVentasContado.Text;
						MS.ResumenDiaID = RD.ID;
						MS.AreaID = AreaID;

						db.Movimientos.InsertOnSubmit(MS);
						Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
						"Se registró la Venta de Contado Combustible: " + MS.Numero.ToString("000000000"), this.Name);
						#endregion

						#region <<< REGISTRANDO KARDEX >>>
						foreach (var dk in MaestroK.Where(mk => mk.MovimientoID.Equals(1)))
						{
							
						   // var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
							Entidad.Kardex KX = new Entidad.Kardex();

							KX.MovimientoID = MS.ID;
							KX.ProductoID = dk.ProductoID;
							KX.EsProducto = true;
							KX.UnidadMedidaID = dk.UnidadMedidaID;
							KX.Fecha = MS.FechaRegistro;
							KX.EstacionServicioID = EstacionServicioID;
							KX.SubEstacionID = SubEstacionID;
							KX.AlmacenSalidaID = dk.AlmacenSalidaID;
							KX.CantidadSalida = dk.CantidadSalida;
							KX.CostoSalida = dk.CostoSalida;
							KX.CostoFinal = dk.CostoSalida;
							KX.CostoTotal = Decimal.Round(dk.CostoSalida * dk.CantidadSalida, 2, MidpointRounding.AwayFromZero);

							KX.CantidadInicial = Parametros.General.SaldoKardex(db, EstacionServicioID, SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, Fecha, false);
								
							KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

							if (KX.CantidadSalida > KX.CantidadInicial)
							{
								Parametros.General.splashScreenManagerMain.CloseWaitForm();
								trans.Rollback();
								this.btnOK.Enabled = false;
								Parametros.General.DialogMsg("La cantidad a salir del producto: " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
								return false;
							}

							KX.Precio = dk.Precio;
							KX.PrecioTotal = dk.PrecioTotal;

							db.Kardexes.InsertOnSubmit(KX);

							#region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

							//------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

							var Tanque = (from tp in db.TanqueProductos
										  where tp.ProductoID.Equals(KX.ProductoID)
											&& tp.TanqueID.Equals(KX.AlmacenSalidaID)
										  select tp).ToList();

							if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
							{
								Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
								TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;
							}

							db.SubmitChanges();

							#endregion 

						}
						#endregion

						#region <<< REGISTRANDO COMPROBANTE >>>
						List<Entidad.ComprobanteContable> Compronbante = CDVC;

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
							this.btnOK.Enabled = false;
							return false;
						}
						Compronbante.ForEach(linea =>
						{
							MS.ComprobanteContables.Add(linea);
							db.SubmitChanges();
						});

						db.SubmitChanges();

						#endregion

						#region ::: REGISTRANDO VENTA / NOTA :::

						int n = 1;
						DetalleM.Where(dm => dm.MovimientoReferenciaID.Equals(1)).ToList().ForEach(det =>
							{
								//DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, 
								//ClienteID = Parametros.Config.ClienteVentaConttadoID(),
								//Monto = ValorCS, AreaID = db.AreaVentas.Where(a => a.EsCombustible).First().ID, MovimientoReferenciaID = 1 });
									
								Entidad.Movimiento MD = new Entidad.Movimiento();
								MD.ClienteID = det.ClienteID;
								MD.EmpleadoID = det.EmpleadoID;
								MD.MovimientoTipoID = det.MovimientoTipoID;
								MD.UsuarioID = UsuarioID;
								MD.FechaCreado = FechaServidor;
								MD.FechaRegistro = Fecha;
								MD.MonedaID = MonedaID;
								MD.TipoCambio = (det.ClienteID.Equals(457) ? RD.TipoCambioMoneda : _TipoCambio);
								MD.Monto = det.Monto;
								MD.MontoMonedaSecundaria = Decimal.Round((MD.Monto / MD.TipoCambio), 2, MidpointRounding.AwayFromZero);
								MD.Referencia = txtVentasContado.Text + "_" + n.ToString("00");
								MD.EstacionServicioID = EstacionServicioID;
								MD.SubEstacionID = SubEstacionID;
								MD.Comentario = memoVentasContado.Text;
								MD.ResumenDiaID = RD.ID;
								MD.MovimientoReferenciaID = MS.ID;
								MD.AreaID = AreaID;

								db.Movimientos.InsertOnSubmit(MD);
								Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
								"Se registró la Venta de Combustible: " + MD.Referencia, this.Name);

								db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = det.ClienteID, Valor = det.Monto, MovimientoID = MD.ID});
								db.SubmitChanges();
									
								n++;
							});

						#endregion

					}

					#endregion

					#region <<< VENTAS CUPONES  >>>
					if (xtraTabPageCup.PageVisible)
					{
						//Identificar el Movimiento Ventas Contado Combustible
						var MovVcc = MaestroM.Single(m => m.ID.Equals(2));

						Entidad.Movimiento MS = new Entidad.Movimiento();
						MS.ClienteID = 0;
						MS.MovimientoTipoID = MovVcc.MovimientoTipoID;
						MS.UsuarioID = UsuarioID;
						MS.FechaCreado = FechaServidor;
						MS.FechaRegistro = Fecha;
						MS.MonedaID = MonedaID;
						MS.TipoCambio = _TipoCambio;
						MS.Monto = MovVcc.Monto;
						MS.MontoMonedaSecundaria = Decimal.Round((MS.Monto / MS.TipoCambio), 2, MidpointRounding.AwayFromZero);
						MS.Numero = Convert.ToInt32(txtCupones.Text);
						MS.EstacionServicioID = EstacionServicioID;
						MS.SubEstacionID = SubEstacionID;
						MS.Comentario = memoCupones.Text;
						MS.ResumenDiaID = RD.ID;
						MS.AreaID = AreaID;

						db.Movimientos.InsertOnSubmit(MS);
						Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
						"Se registró la Venta de Cupones: " + MS.Numero.ToString("000000000"), this.Name);

						foreach (var dk in MaestroK.Where(mk => mk.MovimientoID.Equals(2)))
						{

							// var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
							Entidad.Kardex KX = new Entidad.Kardex();

							KX.MovimientoID = MS.ID;
							KX.ProductoID = dk.ProductoID;
							KX.EsProducto = true;
							KX.UnidadMedidaID = dk.UnidadMedidaID;
							KX.Fecha = MS.FechaRegistro;
							KX.EstacionServicioID = EstacionServicioID;
							KX.SubEstacionID = SubEstacionID;
							KX.AlmacenSalidaID = dk.AlmacenSalidaID;
							KX.CantidadSalida = dk.CantidadSalida;
							KX.CostoSalida = dk.CostoSalida;
							KX.CostoFinal = dk.CostoSalida;
							KX.CostoTotal = Decimal.Round(dk.CostoSalida * dk.CantidadSalida, 2, MidpointRounding.AwayFromZero);

							KX.CantidadInicial = Parametros.General.SaldoKardex(db, EstacionServicioID, SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, Fecha, false);

							KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

							if (KX.CantidadSalida > KX.CantidadInicial)
							{
								Parametros.General.splashScreenManagerMain.CloseWaitForm();
								trans.Rollback();
								Parametros.General.DialogMsg("La cantidad a salir del producto: " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
								this.btnOK.Enabled = false;
								return false;
							}

							KX.Precio = dk.Precio;
							KX.PrecioTotal = dk.PrecioTotal;

							db.Kardexes.InsertOnSubmit(KX);

							#region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

							//------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

							var Tanque = (from tp in db.TanqueProductos
										  where tp.ProductoID.Equals(KX.ProductoID)
											&& tp.TanqueID.Equals(KX.AlmacenSalidaID)
										  select tp).ToList();

							if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
							{
								Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
								TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;
							}

							db.SubmitChanges();

							#endregion

						}

						#region <<< REGISTRANDO COMPROBANTE >>>
						List<Entidad.ComprobanteContable> Compronbante = CDCup;

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
							this.btnOK.Enabled = false;
							return false;
						}
						Compronbante.ForEach(linea =>
						{
							MS.ComprobanteContables.Add(linea);
							db.SubmitChanges();
						});

						db.SubmitChanges();

						#endregion

						#region ::: REGISTRANDO VENTA / NOTA :::
						int n = 1;
						DetalleM.Where(dm => dm.MovimientoReferenciaID.Equals(2)).ToList().ForEach(det =>
						{
							//DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, 
							//ClienteID = Parametros.Config.ClienteVentaConttadoID(),
							//Monto = ValorCS, AreaID = db.AreaVentas.Where(a => a.EsCombustible).First().ID, MovimientoReferenciaID = 1 });
								
							Entidad.Movimiento MD = new Entidad.Movimiento();
							MD.ClienteID = det.ClienteID;
							MD.EmpleadoID = det.EmpleadoID;
							MD.MovimientoTipoID = det.MovimientoTipoID;
							MD.UsuarioID = UsuarioID;
							MD.FechaCreado = FechaServidor;
							MD.FechaRegistro = Fecha;
							MD.MonedaID = MonedaID;
							MD.TipoCambio = _TipoCambio;
							MD.Monto = det.Monto;
							MD.MontoMonedaSecundaria = Decimal.Round((MD.Monto / MD.TipoCambio), 2, MidpointRounding.AwayFromZero);
							MD.Referencia = txtCupones.Text + "_" + n.ToString("00");
							MD.EstacionServicioID = EstacionServicioID;
							MD.SubEstacionID = SubEstacionID;
							MD.Comentario = memoVentasContado.Text;
							MD.ResumenDiaID = RD.ID;
							MD.MovimientoReferenciaID = MS.ID;
							MD.AreaID = AreaID;

							db.Movimientos.InsertOnSubmit(MD);
							Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
							"Se registró la Venta de Cupones: " + MD.Referencia, this.Name);

							db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = det.ClienteID, Valor = det.Monto, MovimientoID = MD.ID });
							db.SubmitChanges();

							n++;
						});

						#endregion
						
					}

					#endregion

					#region <<< VENTAS PETROCARD  >>>
					if (xtraTabPagePetro.PageVisible)
					{
						//Identificar el Movimiento Ventas Contado Combustible
						var MovVcc = MaestroM.Single(m => m.ID.Equals(3));

						#region <<< REGISTRANDO MOVIMIENTO >>>
						Entidad.Movimiento MS = new Entidad.Movimiento();
						MS.ClienteID = 0;
						MS.MovimientoTipoID = MovVcc.MovimientoTipoID;
						MS.UsuarioID = UsuarioID;
						MS.FechaCreado = FechaServidor;
						MS.FechaRegistro = Fecha;
						MS.MonedaID = MonedaID;
						MS.TipoCambio = _TipoCambio;
						MS.Monto = MovVcc.Monto;
						MS.MontoMonedaSecundaria = Decimal.Round((MS.Monto / MS.TipoCambio), 2, MidpointRounding.AwayFromZero);
						MS.Numero = Convert.ToInt32(txtPetrocard.Text);
						MS.EstacionServicioID = EstacionServicioID;
						MS.SubEstacionID = SubEstacionID;
						MS.Comentario = memoPetrocard.Text;
						MS.ResumenDiaID = RD.ID;
						MS.AreaID = AreaID;

						db.Movimientos.InsertOnSubmit(MS);
						Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
						"Se registró la Venta de Contado Combustible: " + MS.Numero.ToString("000000000"), this.Name);
						#endregion

						#region <<< REGISTRANDO KARDEX >>>
						foreach (var dk in MaestroK.Where(mk => mk.MovimientoID.Equals(3)))
						{

							// var Producto = db.Productos.Single(p => p.ID.Equals(dk.ProductoID));
							Entidad.Kardex KX = new Entidad.Kardex();

							KX.MovimientoID = MS.ID;
							KX.ProductoID = dk.ProductoID;
							KX.EsProducto = true;
							KX.UnidadMedidaID = dk.UnidadMedidaID;
							KX.Fecha = MS.FechaRegistro;
							KX.EstacionServicioID = EstacionServicioID;
							KX.SubEstacionID = SubEstacionID;
							KX.AlmacenSalidaID = dk.AlmacenSalidaID;
							KX.CantidadSalida = dk.CantidadSalida;
							KX.CostoSalida = dk.CostoSalida;
							KX.CostoFinal = dk.CostoSalida;
							KX.CostoTotal = Decimal.Round(dk.CostoSalida * dk.CantidadSalida, 2, MidpointRounding.AwayFromZero);

							KX.CantidadInicial = Parametros.General.SaldoKardex(db, EstacionServicioID, SubEstacionID, KX.ProductoID, KX.AlmacenSalidaID, Fecha, false);

							KX.CantidadFinal = KX.CantidadInicial - KX.CantidadSalida;

							if (KX.CantidadSalida > KX.CantidadInicial)
							{
								Parametros.General.splashScreenManagerMain.CloseWaitForm();
								trans.Rollback();
								Parametros.General.DialogMsg("La cantidad a salir del producto: " + db.Productos.Single(p => p.ID.Equals(KX.ProductoID)).Nombre + " sobrepasa la existencia.", Parametros.MsgType.warning);
								this.btnOK.Enabled = false;
								return false;
							}

							KX.Precio = dk.Precio;
							KX.PrecioTotal = dk.PrecioTotal;

							db.Kardexes.InsertOnSubmit(KX);

							#region ::: REGISTRANDO EXISTENCIAS EN BODEGAS :::

							//------------------ CONSULTA EXISTENCIA DE PRODUCTO EN BODEGA --------------//                                    

							var Tanque = (from tp in db.TanqueProductos
										  where tp.ProductoID.Equals(KX.ProductoID)
											&& tp.TanqueID.Equals(KX.AlmacenSalidaID)
										  select tp).ToList();

							if (!Tanque.Count().Equals(0)) //-- SI NO HAY REGISTRO DE EXISTENCIA INGRESAR INVOICE
							{
								Entidad.TanqueProducto TPto = db.TanqueProductos.Single(p => p.ProductoID.Equals(KX.ProductoID) && p.TanqueID.Equals(KX.AlmacenSalidaID));
								TPto.Cantidad = Tanque.Single(q => q.ProductoID.Equals(KX.ProductoID)).Cantidad - KX.CantidadSalida;
							}

							db.SubmitChanges();

							#endregion
						}
						#endregion

						#region <<< REGISTRANDO COMPROBANTE >>>
						List<Entidad.ComprobanteContable> Compronbante = CDPetro;

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
							this.btnOK.Enabled = false;
							return false;
						}
						Compronbante.ForEach(linea =>
						{
							MS.ComprobanteContables.Add(linea);
							db.SubmitChanges();
						});

						db.SubmitChanges();

						#endregion

						#region ::: REGISTRANDO VENTA / NOTA :::
						int n = 1;
						DetalleM.Where(dm => dm.MovimientoReferenciaID.Equals(3)).ToList().ForEach(det =>
						{
							//DetalleM.Add(new Entidad.Movimiento { MovimientoTipoID = 7, 
							//ClienteID = Parametros.Config.ClienteVentaConttadoID(),
							//Monto = ValorCS, AreaID = db.AreaVentas.Where(a => a.EsCombustible).First().ID, MovimientoReferenciaID = 1 });
								
							Entidad.Movimiento MD = new Entidad.Movimiento();
							MD.ClienteID = det.ClienteID;
							MD.EmpleadoID = det.EmpleadoID;
							MD.MovimientoTipoID = det.MovimientoTipoID;
							MD.UsuarioID = UsuarioID;
							MD.FechaCreado = FechaServidor;
							MD.FechaRegistro = Fecha;
							MD.MonedaID = MonedaID;
							MD.TipoCambio = _TipoCambio;
							MD.Monto = det.Monto;
							MD.MontoMonedaSecundaria = Decimal.Round((MD.Monto / MD.TipoCambio), 2, MidpointRounding.AwayFromZero);
							MD.Referencia = txtPetrocard.Text + "_" + n.ToString("00");
							MD.EstacionServicioID = EstacionServicioID;
							MD.SubEstacionID = SubEstacionID;
							MD.Comentario = memoVentasContado.Text;
							MD.ResumenDiaID = RD.ID;
							MD.MovimientoReferenciaID = MS.ID;
							MD.AreaID = AreaID;

							db.Movimientos.InsertOnSubmit(MD);
							Parametros.General.AddLogBook(db, Parametros.TipoAccion.Ventas,
							"Se registró la Venta de Combustible: " + MD.Referencia, this.Name);

							db.Deudors.InsertOnSubmit(new Entidad.Deudor { ClienteID = det.ClienteID, Valor = det.Monto, MovimientoID = MD.ID });
							db.SubmitChanges();

							n++;
						});

						#endregion

					}

					#endregion

					Entidad.ResumenDia R = db.ResumenDias.Single(r => r.ID.Equals(RD.ID));
					R.Contabilizado = true;
					db.SubmitChanges();
					trans.Commit();

					Parametros.General.splashScreenManagerMain.CloseWaitForm();
					RefreshMDI = true;
					ShowMsg = true;
					IDRD = RD.ID;
					return true;
				}

				catch (Exception ex)
				{
					Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
					Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
					this.btnOK.Enabled = false;
					return false;
				}
				finally
				{
					if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
				}
			}
		}

		private void ValidarerrRequiredField()
		{
			//Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
		}
					
		#endregion

		#region *** EVENTOS ***

		private void btnOK_Click_1(object sender, EventArgs e)
		{
			if (Parametros.General.DialogMsg("¿Desea Guardar el Comprobante de Arqueo?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
			{
				if (!Guardar()) return;
				this.Close();
			}
			
		}

		private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
		{
			MDI.CleanDialog(ShowMsg, RefreshMDI, IDRD);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void txtNombre_Validated(object sender, EventArgs e)
		{
			Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
		}

		private void btnAjustar_Click(object sender, EventArgs e)
		{
			try
			{
				if (Math.Abs(Diferencias.First(d => d.Booleano).Value) > _MargenAjusteComprobanteArqueo)
					Parametros.General.DialogMsg("El valor del ajuste es mayor al margen permisible " + _MargenAjusteComprobanteArqueo.ToString(), Parametros.MsgType.warning);
				else
				{
					int l = CDVC.OrderByDescending(o => o.Linea).FirstOrDefault().Linea;

					if (Diferencias.First(d => d.Booleano).Value > 0)
                        CDVC.Add(new Entidad.ComprobanteContable { Linea = l + 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = db.ConceptoContables.Single(c => c.ID.Equals(ConceptoGanancia)).CuentaContableID, Monto = -Math.Abs(Diferencias.First(d => d.Booleano).Value), TipoCambio = _TipoCambio, MontoMonedaSecundaria = -Math.Abs(Decimal.Round((Diferencias.First(d => d.Booleano).Value / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = "Diferencias Ganadas Pagos de Tarjetas y Arqueos" });
					else if (Diferencias.First(d => d.Booleano).Value < 0)
                        CDVC.Add(new Entidad.ComprobanteContable { Linea = l + 1, Fecha = Fecha, EstacionServicioID = EstacionServicioID, SubEstacionID = SubEstacionID, CuentaContableID = db.ConceptoContables.Single(c => c.ID.Equals(Convert.ToInt32(ConceptoPerdida))).CuentaContableID, CentroCostoID = db.CuentaContables.Single(ct => ct.ID.Equals(Convert.ToInt32(db.ConceptoContables.Single(c => c.ID.Equals(Convert.ToInt32(ConceptoPerdida))).CuentaContableID))).CecoID, Monto = Math.Abs(Diferencias.First(d => d.Booleano).Value), TipoCambio = _TipoCambio, MontoMonedaSecundaria = Math.Abs(Decimal.Round((Diferencias.First(d => d.Booleano).Value / _TipoCambio), 2, MidpointRounding.AwayFromZero)), Descripcion = "Diferencias en Pagos de Tarjetas y Arqueos" });

					var obj = (from cds in this.CDVC
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
								   cds.Linea,
								   CECO = cds.CentroCostoID,
								   cds.Litros
							   }).OrderBy(o => o.Linea);

					bsCCVC.DataSource = obj;
					bgvCCVC.RefreshData();

					Diferencias.Remove(Diferencias.First(o => o.Booleano));

					if (Diferencias.Count > 0)
					{
						layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
						var D = Diferencias.OrderBy(o => o.ID).First();
						layoutControlItemDif.Text = D.Display;
						spDif.Value = D.Value;

						if (Diferencias.Count(d => d.Booleano) > 0)
						{
							layoutControlItembtnAjuste.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
						}
					}
					else
					{
						layoutControlItemDif.Text = "<>";
						spDif.Value = 0m; ;
						layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
					}

					layoutControlItembtnAjuste.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

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