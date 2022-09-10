using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogMovimientoEmpleado : Form
    {
        #region <<<DECLARACIONES>>>

        public Entidad.MovimientoEmpleado EtAnterior;
        internal FormMovimientoEmpleado MDI;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsg = false;
        internal bool Editable = false;
        private int UsuarioID;
        internal int _MovimientoHoraExtra;
        private bool RefreshMDI = false;
        private bool _Editing = true;
        private Entidad.TipoMovimientoEmpleado tme;
        internal List<ListCuotas> _MEDetalle = new List<ListCuotas>();
        internal int _MovimientoAusencia;
        internal int _MovimientoSubsidio;
        private bool _esAusencia;
        private bool _esSubsidio;
        String parametro;
        #endregion
        
        public DialogMovimientoEmpleado(int UserID)
        {
            InitializeComponent();
            this.UsuarioID = UserID;
        }

        //#region <<<Properties>>>
        public decimal MontoTotal
        {
            get { return speMontoTotal.Value; }
            set { speMontoTotal.Value = value; }
        }

        public decimal SalarioDiario
        {
            get { return Convert.ToDecimal(txtSalDia.Text); }
            set { txtSalDia.Text = value.ToString(); }
        }

        public decimal MontoCuota
        {
            get { return Convert.ToDecimal(txtMontoCuota.Text); }
            set { txtMontoCuota.Text = value.ToString(); }
        }

        public decimal CantidadDias
        {
            get { return Convert.ToDecimal(speCantDias.Value); }
            set { speCantDias.Value = value; }
        }

        public string Descripcion
        {
            get { return mmoDescripcion.Text; }
            set { mmoDescripcion.Text = value; }
        }

        public string Titulo
        {
            set
            {
                this.Text = value;
            }
        }

        public decimal CantidadHE
        {
            get { return speCantidadHE.Value; }
            set { speCantidadHE.Value = value; }
        }

        public int CantidadCuotas
        {
            get { return Convert.ToInt32(speCantCuotas.Value); }
            set { speCantCuotas.Value = value; }
        }

        private DateTime FechaInicial
        {
            get { return Convert.ToDateTime(dateFechaInicial.EditValue); }
            set { dateFechaInicial.EditValue = value; }
        }

        private DateTime FechaFinal
        {
            get { return Convert.ToDateTime(dateFechaFinal.EditValue); }
            set { dateFechaFinal.EditValue = value; }
        }
       // #endregion

        #region <<<eventos>>>
        private void DialogTipoMovimientoEmpleado_Load(object sender, EventArgs e)
        {
            FillControll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowMsg = RefreshMDI = false;
            _Editing = true;
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        private void DialogTipoMovimientoEmpleado_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, _Editing);
        }
        #endregion

        private void comprobar()
        {
            var estado = (from d in db.MovimientoEmpleado
                              where d.ID.Equals(1)
                          select new { abono = d.Abono, numero_cuotoa = d.NumeroCuota, Monto_cuota = d.MontoCuota, Monto_Total = d.MontoTotal,                           saldo = d.MontoTotal - d.Abono }).First();

            
        }
        
        private void speCantCuotas_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //int per = 15;
                FechaFinal = GetNextQuincena(dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(15 * (speCantCuotas.Value - 1))));

                if (speCantCuotas.Value <= 0 || speCantCuotas.Value == null)
                    speCantCuotas.Value = 1;

                decimal valor = 0;
                valor = speCantCuotas.Value;
                if (speMontoTotal.Value != null)
                {
                    txtMontoCuota.Text = decimal.Round(speMontoTotal.Value / speCantCuotas.Value, 2, MidpointRounding.AwayFromZero).ToString();
                }
                else if (speMontoTotal.Value == null || (speMontoTotal.Value <= 0))
                {
                    speMontoTotal.Value=0;
                    txtMontoCuota.Text = decimal.Round(speMontoTotal.Value / speCantCuotas.Value, 2, MidpointRounding.AwayFromZero).ToString();
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #region <<< Metodos>>>
        private void FillControll()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                _MovimientoHoraExtra = Parametros.Config.MovimientoHoraExtraID();
                _MovimientoAusencia = Parametros.Config.MovimientoAusenciaID();
                _MovimientoSubsidio = Parametros.Config.MovimientoSubsidioID();

                tme = db.TipoMovimientoEmpleado.Single(s => s.ID.Equals(EtAnterior.TipoMovimientoEmpleadoID));

                if (tme.EsIngreso)
                {
                    layoutControlFechaFinal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlCantCuotas.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlMontoCuota.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlCantDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    if (!EtAnterior.TipoMovimientoEmpleadoID.Equals(_MovimientoHoraExtra))
                    {
                        layoutControlCantidadHE.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        if (EtAnterior.TipoMovimientoEmpleadoID.Equals(_MovimientoSubsidio))
                            _esSubsidio = true;
                    }
                    else if (EtAnterior.TipoMovimientoEmpleadoID.Equals(_MovimientoHoraExtra))
                    {
                        //speMontoTotal.Properties.ReadOnly = true;
                        //speMontoTotal.Properties.AllowFocused = false;
                    }

                    if (_esSubsidio)
                    {
                        layoutControlMontoCuota.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlCantDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlMontoTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                }
                else if (!tme.EsIngreso)
                {
                    if (EtAnterior.TipoMovimientoEmpleadoID.Equals(_MovimientoAusencia))
                    {
                        //speMontoTotal.Properties.ReadOnly = true;
                        //speMontoTotal.Properties.AllowFocused = false;
                        _esAusencia = true;
                        layoutControlCantCuotas.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    layoutControlCantidadHE.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                    layoutControlItemAdd.Visibility = layoutControlItemDel.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }

                if (Editable)
                {

                    Descripcion = EtAnterior.Descripcion;
                    SalarioDiario = EtAnterior.SalarioDiario;
                    CantidadHE = EtAnterior.CantidadHE;
                    CantidadCuotas = EtAnterior.CantidadCuotas - EtAnterior.NumeroCuota;
                    dateFechaInicial.DateTime = EtAnterior.FechaInicial;
                    dateFechaFinal.DateTime = EtAnterior.FechaFinal;
                    MontoCuota = EtAnterior.MontoCuota;
                    MontoTotal = EtAnterior.MontoTotal - EtAnterior.Abono;
                    CantidadDias = EtAnterior.CantidadDias;
                    spinEdit1.Value = EtAnterior.Abono;
                    spinEdit2.Value = EtAnterior.MontoTotal;

                    if (EtAnterior.Abono > 0)
                    {
                        speMontoTotal.Enabled = false;
                        txtSalDia.Enabled = false;
                        layoutControlItemAbonos.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlMontoTotal.Text = "Cuota Faltante";
                        layoutControlIFaltante.Visibility =  DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlIFaltante.Text = "Total Deuda";
                        layoutControlCantCuotas.Text = "Cuotas Faltantes";
                        layoutControlItem2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        label1.Text = "Cantidad Abonada " + EtAnterior.NumeroCuota;
                        layoutControlGroup3.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                        _MEDetalle = (from o in db.GetCuotaMovEmpleado(EtAnterior.ID)
                            //(from d in db.DetalleMovimientoPlanilla
                            //          join dg in db.DetallePlanillaGenerada on d.DetallePlanillaGeneradaID equals dg.ID
                            //          join g in db.PlanillaGenerada on dg.PlanillaGeneradaID equals g.ID
                            //          where d.MovimientoEmpleadoID.Equals(EtAnterior.ID)
                            //          && d.TipoMovimientoEmpleadoID.Equals(EtAnterior.TipoMovimientoEmpleadoID)
                                      select new ListCuotas
                                      { ID = o.ID, Fecha= o.Fecha, Numero = o.Numero, Value = o.Value, Tipo = Convert.ToChar(o.Tipo) }).ToList();
                    
                        grid.DataSource = _MEDetalle;
                        gvData.AddNewRow();
                    }
                    else
                    {
                        this.Size = new Size(350, 446);
                        layoutControlGroup5.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                }
                //speCantidadHE_EditValueChanged(null, null);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        public bool ValidarCampos()
        {
            if (speMontoTotal.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 120;
                try
                {
                    Entidad.MovimientoEmpleado ME;

                    if (Editable)
                    { ME = db.MovimientoEmpleado.Single(e => e.ID == EtAnterior.ID); }
                    else
                    {
                        ME = new Entidad.MovimientoEmpleado();
                    }

                    ME.Descripcion = Descripcion;
                    ME.CantidadHE = CantidadHE;
                    ME.CantidadCuotas = CantidadCuotas;
                    ME.FechaFinal = FechaFinal;
                    ME.FechaInicial = FechaInicial;
                    ME.MontoTotal = MontoTotal;
                    ME.MontoCuota = MontoCuota;
                    ME.CantidadDias = CantidadDias;

                    //if (Editable)
                    //{
                    //    DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(ME, 1));
                    //    DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EtAnterior, 1));

                    //    Entidad.DetalleMovimientoPlanilla DMP;

                    //    var lista_eliminar = (from l in db.DetalleMovimientoPlanilla
                    //                          where l.MovimientoEmpleadoID.Equals(EtAnterior.ID)
                    //                          select new { id = l.ID }).ToList();

                    //    foreach (var row in lista_eliminar.Select(d => d.id))
                    //    {
                    //        DMP = db.DetalleMovimientoPlanilla.Single(d => d.ID == row);
                    //        db.DetalleMovimientoPlanilla.DeleteOnSubmit(DMP);
                    //    }
                    //    db.SubmitChanges();

                    //    for (int g = 0; g < EtAnterior.MontoCuota; g++)
                    //    {
                    //        Entidad.DetalleMovimientoPlanilla DMtP = new Entidad.DetalleMovimientoPlanilla();
                    //        DMtP.MovimientoEmpleadoID = EtAnterior.ID;
                    //        DMtP.Monto = EtAnterior.MontoTotal;
                    //        DMtP.Cuotas = (g + 1);
                    //        DMtP.TipoMovimientoEmpleadoID = _MovimientoAusencia;
                    //        db.DetalleMovimientoPlanilla.InsertOnSubmit(DMtP);
                    //    }

                    //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                    //     "Se modificó el Movimiento Empleado: " + EtAnterior.Descripcion, this.Name, dtPosterior, dtAnterior);

                    //}
                    //else
                    //{
                    //    db.MovimientoEmpleado.InsertOnSubmit(ME);
                    //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                    //    "Se modifico el Movimiento Empleado: " + tme.Descripcion, this.Name);

                    //}

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = RefreshMDI = _Editing = true;
                    return true;
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                    return false;
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        public static DateTime GetNextQuincena(DateTime fecha)
        {
            DateTime date = new DateTime();
            try
            {
                int dia = fecha.Day;
                if (dia <= 15)
                    date = new DateTime(fecha.Year, fecha.Month, 15);
                else
                {
                    //date = new DateTime(fecha.Year, fecha.Month + 1, 1);                
                    date = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
                    date = date.AddDays(-1); //Ultimo dia del mes
                }
                return date;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                return date;
            }
        }
        #endregion

        private void dateFechaInicial_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FechaFinal = GetNextQuincena(FechaInicial.Date.AddDays(Convert.ToDouble(15 * (speCantCuotas.Value - 1))));
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void speCantidadHE_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                MontoTotal = ((SalarioDiario / 8) * 2) * speCantidadHE.Value;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void speCantDias_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FechaFinal = dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(speCantDias.Value));

                if (speCantDias.Value <= 0 || String.IsNullOrEmpty(speCantDias.Value.ToString()))
                    speCantDias.Value = 1;

                if (_esAusencia)
                {
                    
                    decimal valor = 0;

                    valor = speCantDias.Value;
                    //gvFinalizar.SetRowCellValue(i, colCantidadDias, valor);
                    if (MontoTotal != null)
                    {
                        MontoTotal = decimal.Round((SalarioDiario * speCantDias.Value), 2, MidpointRounding.AwayFromZero);
                    }
                    else if (MontoTotal == null || (int) MontoTotal <= 0)
                    {
                        MontoTotal = 0;
                        MontoTotal = decimal.Round((SalarioDiario * speCantDias.Value), 2, MidpointRounding.AwayFromZero);
                    }
                }
                else if (_esSubsidio)
                {
                    
                    decimal valor = 0;

                    valor = speCantDias.Value;
                    //gvFinalizar.SetRowCellValue(i, colCantidadDias, valor);
                    if (MontoCuota != null)
                    {
                        MontoCuota = decimal.Round((SalarioDiario * speCantDias.Value * 0.4m), 2, MidpointRounding.AwayFromZero);
                        MontoTotal = decimal.Round((SalarioDiario * speCantDias.Value), 2, MidpointRounding.AwayFromZero);
                    }
                    else if (MontoCuota == null || (int)MontoCuota <= 0)
                    {
                        MontoCuota = 0;
                        MontoCuota = decimal.Round((SalarioDiario * speCantDias.Value * 0.4m), 2, MidpointRounding.AwayFromZero);
                        MontoTotal = decimal.Round((SalarioDiario * speCantDias.Value), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvData.FocusedRowHandle >= 0)
                {
                    if (Convert.ToString(gvData.GetFocusedRowCellValue(colTipo)).Equals("A"))
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                            Entidad.AbonoCuota A = db.AbonoCuotas.SingleOrDefault(s => s.ID.Equals(Convert.ToString(gvData.GetFocusedRowCellValue(colID))));

                            if (A != null)
                            {
                               
                                Entidad.MovimientoEmpleado M = db.MovimientoEmpleado.Single(s => s.ID.Equals(EtAnterior.ID));
                                M.NumeroCuota = M.NumeroCuota - 1;
                                M.Abono -= A.Monto;
                                db.SubmitChanges();

                                db.AbonoCuotas.DeleteOnSubmit(A);
                                db.SubmitChanges();

                                _MEDetalle = (from o in db.GetCuotaMovEmpleado(EtAnterior.ID)
                                              select new ListCuotas { ID = o.ID, Fecha = o.Fecha, Numero = o.Numero, Value = o.Value, Tipo = Convert.ToChar(o.Tipo) }).ToList();

                                grid.DataSource = _MEDetalle;
                                gvData.RefreshData();
                            }
                            else
                                Parametros.General.DialogMsg("¡El abono no tiene ID!", Parametros.MsgType.warning);
                        }
                    }
                    else
                        Parametros.General.DialogMsg("El abono es generado por pago de planilla, ¡No puede ser eliminado!", Parametros.MsgType.warning);
           
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (Dialogs.DialogAbonoCuota df = new DialogAbonoCuota())
            {
                df.Text = "Datos del Abono";


                if (df.ShowDialog().Equals(DialogResult.OK))
                {

                    Entidad.AbonoCuota A = new Entidad.AbonoCuota();

                    A.Cantidad = 1;
                    A.Cuotas = EtAnterior.NumeroCuota + 1;
                    A.EmpleadoID = EtAnterior.EmpleadoID;
                    A.Fecha = df.Fecha.Date;
                    A.Monto = df.Abono;
                    A.MovimientoEmpleadoID = EtAnterior.ID;
                    A.TipoMovimientoEmpleadoID = EtAnterior.TipoMovimientoEmpleadoID;

                    db.AbonoCuotas.InsertOnSubmit(A);
                    db.SubmitChanges();

                    Entidad.MovimientoEmpleado M = db.MovimientoEmpleado.Single(s => s.ID.Equals(EtAnterior.ID));
                    M.Abono += A.Monto;
                    M.NumeroCuota = A.Cuotas;
                    db.SubmitChanges();

                    _MEDetalle = (from o in db.GetCuotaMovEmpleado(EtAnterior.ID)
                                  select new ListCuotas { ID = o.ID, Fecha = o.Fecha, Numero = o.Numero, Value = o.Value, Tipo = Convert.ToChar(o.Tipo) }).ToList();

                    grid.DataSource = _MEDetalle;
                    gvData.RefreshData();
                }
            }
        }
    }

    public class ListCuotas
    {
        public int ID { get; set; }
        public int Numero { get; set; }
        public decimal Value { get; set; }
        public DateTime Fecha { get; set; }
        public char Tipo { get; set; }

        public ListCuotas()
        {

        }

        public ListCuotas(int _id, int _numero, decimal _valor, DateTime _fecha, char _tipo)
        {
            this.ID = _id;
            this.Numero = _numero;
            this.Value = _valor;
            this.Fecha = _fecha;
            this.Tipo = _tipo;
        }

    }
}
