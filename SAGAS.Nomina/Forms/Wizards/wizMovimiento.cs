using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SAGAS.Nomina.Forms.Wizards
{
    public partial class wizMovimiento : Form
    {
        #region Declaracion

        private Entidad.SAGASDataViewsDataContext dv;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.VistaMovimientoEmpleado vme;
        internal FormMovimientoEmpleado MDI;
        internal int _MovimientoHoraExtra;
        internal int _MovimientoAusencia;
        internal int _MovimientoSubsidio;
        internal DateTime _Fecha;
        internal List<Parametros.ListIdDisplay> ListIdDisplayPlanilla;
        internal List<Parametros.ListIdDisplay> ListaSalarios;
        private List<Entidad.VistaMovimientoEmpleado> EtVista = new List<Entidad.VistaMovimientoEmpleado>();
        
        public List<Entidad.VistaMovimientoEmpleado> VET
        {
            get { return EtVista; }
            set
            {
                EtVista = value;
                this.bdsVista.DataSource = this.EtVista;
            }
        }

        private int IDMovimiento
        {
            get { return Convert.ToInt32(lkeTipoMovimiento.EditValue); }
            set { lkeTipoMovimiento.EditValue = value; }
        }

        private bool HoraExtra
        {get;set;}

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

        private decimal _TipoCambio = 0;
        private bool _esIngreso = true;
        private bool ShowMsg = false;
        private bool RefreshMDI = false;
        private bool _Editing = false;
        private bool _esAusencia = false;
        private bool _esSubsidio = false;
        private bool _deuda = false;

        #endregion

        #region Inicializacion

        public wizMovimiento()
        {
            InitializeComponent();
        }

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                _MovimientoHoraExtra = Parametros.Config.MovimientoHoraExtraID();
                _MovimientoAusencia = Parametros.Config.MovimientoAusenciaID();
                _MovimientoSubsidio = Parametros.Config.MovimientoSubsidioID();

                _Fecha = Convert.ToDateTime(db.GetDateServer());

                lkrTipoMovimientoEmpl.DataSource = db.TipoMovimientoEmpleado.Where(o => o.Activo);
                lkrTipoMovimientoEmpl.ValueMember = "ID";
                lkrTipoMovimientoEmpl.DisplayMember = "Nombre";

                rlkeTipoAusencia.DataSource = db.TipoAusencia.Where(a => a.Activo);
                rlkeTipoAusencia.ValueMember = "ID";
                rlkeTipoAusencia.DisplayMember = "Nombre";

                ListIdDisplayPlanilla = db.Planillas.Where(v => v.Activo).OrderBy(o => o.Nombre).Select(s => new Parametros.ListIdDisplay { ID = s.ID, Display = s.Nombre }).ToList();
                ListIdDisplayPlanilla.Insert(0, new Parametros.ListIdDisplay { ID = 0, Display = "TODOS" });

                lkePlanilla.Properties.DataSource = ListIdDisplayPlanilla;
            
            
            
                var obj = from em in db.Empleados
                          join pl in db.Planillas on em.PlanillaID equals pl.ID
                              where (em.Activo)
                              select new 
                              {
                               em.ID,
                               em.Codigo,
                               NombreCompleto = em.Nombres + ' ' + em.Apellidos,
                               em.Activo,
                               pl.Nombre,
                               PlanillaID = pl.ID,
                               em.SalarioActual
                              };

                List<Entidad.VistaMovimientoEmpleado> items = (from all in obj
                                                           
                                                              select new Entidad.VistaMovimientoEmpleado
                                                                {
                                                                    EmpleadoID = all.ID,
                                                                    EmpleadoCodigo = all.Codigo,
                                                                    EmpleadoNombreCompleto = all.NombreCompleto,
                                                                    EmpeladoSeleccionado = all.Activo,
                                                                    PlanillaNombre = all.Nombre,
                                                                    PlanillaID = all.PlanillaID,
                                                                    SalarioDiario = Decimal.Round(all.SalarioActual / 30, 2, MidpointRounding.AwayFromZero)
                                                                }).ToList();

                ListaSalarios = items.Select(s => new Parametros.ListIdDisplay { ID = s.EmpleadoID, Display = s.SalarioDiario.ToString() }).ToList();
                
                EtVista.AddRange(items);
                lkePlanilla.EditValue = 0;
                //this.bdsVista.DataSource = this.EtVista;
                //tglSelect.IsOn = false;
                //tglSelect_Toggled(null, null);
                //gvData.RefreshData();
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion
        
        #region Eventos
        
        /// Filtra los tipos de Movimientos Empleados en el lkeMovimiento
        ///
        /// 
        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lkeTipoMovimiento.EditValue = null;
            lkeTipoMovimiento.Properties.DataSource = null;
            lkeTipoMovimiento.Properties.NullText = "<Seleccione el Tipo de Movimiento>";
            try
            {
                
                if (rbtngpTipoMovimiento.SelectedIndex.Equals(0))//INGRESO
                {
                    layoutControlHoraExtra.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    lkeTipoMovimiento.Properties.DataSource = (from tme in db.TipoMovimientoEmpleado
                                                              where tme.Activo && tme.EsIngreso && tme.MostrarEnLista
                                                              select new { tme.ID, tme.Nombre }).ToList();
                    layoutControlTipoMovimiento.Text = "Tipo de Ingreso:";
                    _esIngreso = true;
                    _esAusencia = false;
                    _esSubsidio = false;
                    layoutControlTipoMovimiento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                }
                else if (rbtngpTipoMovimiento.SelectedIndex.Equals(1))//DEDUCCION
                {
                    layoutControlHoraExtra.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lkeTipoMovimiento.Properties.DataSource = (from tme in db.TipoMovimientoEmpleado
                                                              where tme.Activo && !tme.EsIngreso && tme.MostrarEnLista
                                                               select new { tme.ID, tme.Nombre }).ToList();
                    layoutControlTipoMovimiento.Text = "Tipo de Deducción:";
                    _esIngreso = false;
                    _esAusencia = false;
                    _esSubsidio = false;
                    _deuda = true;
                    layoutControlTipoMovimiento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                else if (rbtngpTipoMovimiento.SelectedIndex.Equals(2))//AUSENCIA
                {
                    layoutControlHoraExtra.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    lkeTipoMovimiento.Properties.DataSource = (from ta in db.TipoAusencia
                                                              where ta.Activo
                                                               select new { ta.ID, ta.Nombre }).ToList(); ;
                    layoutControlTipoMovimiento.Text = "Tipo de Ausencia:";
                    _esIngreso = false;
                    _esAusencia = true;
                    _esSubsidio = false;
                    layoutControlTipoMovimiento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                }
                else if (rbtngpTipoMovimiento.SelectedIndex.Equals(3))//SUBSIDIO
                {
                    layoutControlHoraExtra.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    layoutControlTipoMovimiento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    _esAusencia = false;
                    _esIngreso = true;
                    _esSubsidio = true;
                    lkeTipoMovimiento.EditValue = _MovimientoSubsidio;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void wizard_SelectedPageChanged(object sender, DevExpress.XtraWizard.WizardPageChangedEventArgs e)
        {
            try
            {
                //new binding
                if (e.Page == wpFinish)
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                    EtVista.Where(v => v.EmpeladoSeleccionado).ToList().ForEach(det => 
                        {
                            colSalarioDiario.Caption = "Salario Diario";
                            if (_esAusencia)
                            {
                                det.TipoAusenciaID = Convert.ToInt32(lkeTipoMovimiento.EditValue);
                                det.TipoMovimientoID = _MovimientoAusencia;
                            }
                            else if(_esSubsidio)
                            {
                                det.TipoMovimientoID = _MovimientoSubsidio;
                            }
                            else
                                det.TipoMovimientoID = Convert.ToInt32(lkeTipoMovimiento.EditValue);

                            if (HoraExtra)
                            {
                                colSalarioDiario.Caption = "Salario por Hora";
                                decimal valorOriginal = (ListaSalarios.Count(o => o.ID.Equals(det.EmpleadoID)) > 0 ? Convert.ToDecimal(ListaSalarios.First(o => o.ID.Equals(det.EmpleadoID)).Display) : 0m);
                                det.SalarioDiario = valorOriginal;

                                decimal valor = decimal.Round(det.SalarioDiario / 8m, 2, MidpointRounding.AwayFromZero);
                                det.SalarioDiario = valor;
                            }
                            else
                            {
                                decimal valor = (ListaSalarios.Count(o => o.ID.Equals(det.EmpleadoID)) > 0 ? Convert.ToDecimal(ListaSalarios.First(o => o.ID.Equals(det.EmpleadoID)).Display) : 0m);
                                det.SalarioDiario = valor;
                            }
                        });

                    dateFechaInicial.DateTime = _Fecha;

                    bdsSelected.DataSource = VET.Where(v => v.EmpeladoSeleccionado).ToList();

                    colMontoCuota.Caption = "Monto por Cuota";
                                        
                    if (_esIngreso)
                    {
                        layoutControlCantCuotas.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControldateFechaFinal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        layoutControlCantidadDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        
                        colTipoAusencia.Visible = colMontoCuota.Visible = colCantCuota.Visible = colCantidadDias.Visible = colAplicaPlanilla.Visible = false;
                        colSalarioDiario.Visible = true;
                        colSalarioDiario.VisibleIndex = 6;
                        colMontoTotal.VisibleIndex = 7;

                        layoutControldateFechaInicial.Text = "Fecha a Aplicar:";
                        speCantCuotas.Value = 1;
                        wpFinish.Text = "Revision de Ingresos.";

                        if (HoraExtra)
                        {
                            colCantidadHE.Visible = true;
                            colCantidadHE.VisibleIndex = 5;
                            colMontoTotal.OptionsColumn.AllowEdit = false;
                            colMontoTotal.OptionsColumn.AllowFocus = false;
                            wpFinish.Text = "Revision de Horas Extras.";
                        }
                        else if (!HoraExtra)
                        {
                            colCantidadHE.Visible = false;
                            colMontoTotal.OptionsColumn.AllowEdit = true;
                            colMontoTotal.OptionsColumn.AllowFocus = true;
                        }

                        if (_esSubsidio)
                        {
                            layoutControldateFechaFinal.Visibility = layoutControlCantidadDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            colCantidadHE.Visible = colTipoAusencia.Visible = colCantCuota.Visible = colMontoTotal.Visible = false;
                            colCantidadDias.Visible = colAplicaPlanilla.Visible = colMontoCuota.Visible = true;
                            colMontoCuota.Caption = "Monto Subsidio";
                            layoutControldateFechaInicial.Text = "Fecha Inicial:";
                            wpFinish.Text = "Revision de Subsidios.";
                            spCantidadDias_EditValueChanged(null, null);
                            for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                            {
                                gvFinalizar.SetRowCellValue(i, colAplicaPlanilla, true);
                            }

                        }
                    }
                    else if (!_esIngreso)
                    {
                        speCantCuotas_EditValueChanged(sender, e);
                        for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                        {
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", 0);
                        }
                        layoutControlCantCuotas.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControldateFechaFinal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        layoutControlCantidadDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        colCantidadHE.Visible = colSalarioDiario.Visible = colSalarioDiario.Visible = colCantidadDias.Visible = colTipoAusencia.Visible = false;
                        colMontoCuota.Visible = colCantCuota.Visible = true;
                        colAplicaPlanilla.Visible = false;
                        colMontoTotal.OptionsColumn.AllowEdit = true;
                        colMontoTotal.OptionsColumn.AllowFocus = true;
                        colCantCuota.VisibleIndex = 5;
                        colMontoCuota.VisibleIndex = 6;
                        colMontoTotal.VisibleIndex = 7;
                        layoutControldateFechaInicial.Text = "Fecha Inicial:";
                        wpFinish.Text = "Revision de Deducciones.";
                        if (_esAusencia)
                        {
                            spCantidadDias_EditValueChanged(sender, e);
                            layoutControlCantidadDias.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            layoutControlCantCuotas.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                            colTipoAusencia.Visible = colCantidadDias.Visible = colSalarioDiario.Visible = true;
                            colCantCuota.Visible = colMontoCuota.Visible = false;
                            colMontoTotal.OptionsColumn.AllowEdit = false;
                            colMontoTotal.OptionsColumn.AllowFocus = false;
                            colMontoTotal.VisibleIndex = 9;
                            speCantCuotas.Text = "1";
                            colAplicaPlanilla.Visible = true;
                            for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                            {
                                gvFinalizar.SetRowCellValue(i,colAplicaPlanilla,true);
                            }
                        }
                    }
                    ///
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        /// Valida si se ha seleccionado al menos un tipo de movimiento y un empleado
        ///
        ///
        private void wizard_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            try
            {
                if (e.Page == wpEmpleado)
                {
                    if (IDMovimiento <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar al menos un movimiento.", Parametros.MsgType.warning);
                        e.Handled = true;
                    }
                    if (VET.Count(v => v.EmpeladoSeleccionado) <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar al menos un empleado.", Parametros.MsgType.warning); 
                        e.Handled = true;
                    }
                    if (chkHE.Checked)
                    {
                        HoraExtra = true;
                    }
                    else HoraExtra = false;

                    gvData.ActiveFilter.Clear();
                    gvData.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        //Verifica si existe un tipo de cambio para la fecha establecida
        // 
        // 
        private void dateFechaInicial_Validated(object sender, EventArgs e)
        {
            try
            {
                if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
                {
                    _TipoCambio = 0;
                    dateFechaInicial.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
                }
                else
                {
                    _TipoCambio = 0;

                    if (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaInicial.Date)) > 0)
                        _TipoCambio = db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Convert.ToDateTime(FechaInicial).Date)).First().Valor;
                    else
                        _TipoCambio = 0m;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void wizMovimiento_Shown(object sender, EventArgs e)
        {
            FillControl();
            this.rbtngpTipoMovimiento.SelectedIndex = -1;
            this.layoutControlTipoMovimiento.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        //Selecciona o deselecciona todos los empleados en la grid
        //
        //
        private void tglSelect_Toggled(object sender, EventArgs e)
        {
            try
            {
                if (tglSelect.IsOn)
                {
                    if (EtVista.Count > 0)
                    {
                        EtVista.Where(v => v.PlanillaID.Equals(Convert.ToInt32(lkePlanilla.EditValue)) || Convert.ToInt32(lkePlanilla.EditValue).Equals(0)).ToList().ForEach(a => a.EmpeladoSeleccionado = true);
                        gvData.RefreshData();
                    }
                }
                else if (!tglSelect.IsOn)
                {
                    if (EtVista.Count > 0)
                    {
                        EtVista.ForEach(a => a.EmpeladoSeleccionado = false);
                        gvData.RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            
        }
        
        #region <<<SALIENDO DEL WIZ>>>
        
        private void wizard_CancelClick(object sender, CancelEventArgs e)
        {
            ShowMsg = false; RefreshMDI = false; _Editing = false;
            this.Close();
        }

        private void wizMovimiento_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI, _Editing);
        }

        private void wizard_FinishClick(object sender, CancelEventArgs e)
        {
            try
            {
                if (EtVista.Count(v => v.EmpeladoSeleccionado && v.MontoTotal <= 0) <= 0)
                {
                    if (_TipoCambio == 0)
                    {
                        Parametros.General.DialogMsg("No existe un tipo de cambio par la fecha seleccionada" + Environment.NewLine + "Debe ponerse en contacto con Oficna Central", Parametros.MsgType.warning);
                        e.Cancel = true;
                        return;
                    }

                    if (!Guardar())
                    {
                        e.Cancel = true;
                        return;
                    }

                    this.Close();
                }
                else
                {
                    Parametros.General.DialogMsg("Hay un empleado seleccionado con Monto Total igual a 0 (cero)." + Environment.NewLine + "Se deben de registrar las cifras de los Montos Totales mayor a 0 (cero) de todos los empleados seleccionados.", Parametros.MsgType.warning);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        //Evalua y selecciona si es H.E. seleccionado del lke
        //
        //
        private void lkeTipoMovimiento_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkeTipoMovimiento.EditValue != null)
                {
                    if (Convert.ToInt32(lkeTipoMovimiento.EditValue).Equals(_MovimientoHoraExtra))
                        chkHE.Checked = true;
                    else
                        chkHE.Checked = false;
                }
                else
                {
                    chkHE.Checked = false;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        //filtra y muestra en grid solo los empleados registrados con esa planilla
        //
        //
        private void lkePlanilla_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkePlanilla.EditValue != null)
                {                    
                    this.bdsVista.DataSource = this.EtVista.Where(v => (v.PlanillaID.Equals(Convert.ToInt32(lkePlanilla.EditValue)) || Convert.ToInt32(lkePlanilla.EditValue).Equals(0)));
                    tglSelect.IsOn = false;
                    tglSelect_Toggled(null, null);
                    gvData.RefreshData();
                }
                else
                {
                    this.bdsVista.DataSource = null;
                    gvData.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void speCantCuotas_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //int per = 15;
                FechaFinal = Parametros.General.GetNextQuincena(dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(15 * (speCantCuotas.Value - 1))));

                if (speCantCuotas.Value <= 0 || speCantCuotas.Value == null)
                    speCantCuotas.Value = 1;

                for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                {
                    decimal valor = 0;
                    //if (Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "colCantCuota")) != Convert.ToDecimal("0.00"))
                    valor = speCantCuotas.Value;//Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "colCantCuota"));
                    gvFinalizar.SetRowCellValue(i, "CantidadCuotas", valor);
                    if (gvFinalizar.GetRowCellValue(i, "MontoTotal") != null)
                    {
                        gvFinalizar.SetRowCellValue(i, "MontoCuota", decimal.Round((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "MontoTotal")) / speCantCuotas.Value), 2, MidpointRounding.AwayFromZero));
                    }
                    else if (gvFinalizar.GetRowCellValue(i, "MontoTotal") == null || (int)gvFinalizar.GetRowCellValue(i, "MontoTotal") <= 0)
                    {
                        gvFinalizar.SetRowCellValue(i, "MontoTotal", 0);
                        gvFinalizar.SetRowCellValue(i, "MontoCuota", decimal.Round((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "MontoTotal")) / speCantCuotas.Value), 2, MidpointRounding.AwayFromZero));
                    }
                }
                this.gvFinalizar.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            

        }

        private void dateFechaInicial_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateFechaFinal.EditValue = Parametros.General.GetNextQuincena(dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(15 * (speCantCuotas.Value - 1))));
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            
        }

        private void dateFechaInicial_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                dateFechaFinal.EditValue = Parametros.General.GetNextQuincena(dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(15 * (speCantCuotas.Value - 1))));
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            
        }

        private void gvFinalizar_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if ((e.Column == colMontoTotal) && !_esSubsidio)
                {
                    try
                    {
                        decimal vMontoCuota = 0, vCantidad = 0, vMontoTotal = 0;

                        if (gvFinalizar.GetRowCellValue(e.RowHandle, "MontoTotal") != null)
                        {
                            vMontoTotal = Convert.ToDecimal(gvFinalizar.GetRowCellValue(e.RowHandle, "MontoTotal"));
                        }
                        else
                        {
                            gvFinalizar.SetRowCellValue(e.RowHandle, "MontoTotal", vCantidad);
                        }
                        vMontoCuota = vMontoTotal / speCantCuotas.Value;
                        gvFinalizar.SetRowCellValue(e.RowHandle, colMontoCuota, vMontoCuota);
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }

                if(e.Column == colCantidadHE)
                {
                    try
                    {
                        decimal vSalario = 0, vMontoTotal = 0;
                        int HE = 0;
                        if (gvFinalizar.GetRowCellValue(e.RowHandle, "CantidadHorasExtras") != null)
                        {
                            HE = Convert.ToInt32(gvFinalizar.GetRowCellValue(e.RowHandle, "CantidadHorasExtras"));
                        }
                        else
                        {
                            gvFinalizar.SetRowCellValue(e.RowHandle, "CantidadHorasExtras", HE);
                        }
                        vSalario = Convert.ToDecimal(gvFinalizar.GetRowCellValue(e.RowHandle, "SalarioDiario"));
                        vMontoTotal = ((vSalario) * 2) * HE;
                        gvFinalizar.SetRowCellValue(e.RowHandle, "MontoTotal", vMontoTotal);
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            
        }

        private void spCantidadDias_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (spCantidadDias.Value <= 0 || String.IsNullOrEmpty(spCantidadDias.Value.ToString()))
                    spCantidadDias.Value = 1;
                
                FechaFinal = dateFechaInicial.DateTime.Date.AddDays(Convert.ToDouble(spCantidadDias.Value) - 1);

                if (_esAusencia)
                {
                    for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                    {
                        decimal valor = 0;

                        valor = spCantidadDias.Value;
                        gvFinalizar.SetRowCellValue(i, colCantidadDias, valor);
                        if (gvFinalizar.GetRowCellValue(i, "MontoTotal") != null)
                        {
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", decimal.Round((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value), 2, MidpointRounding.AwayFromZero));
                        }
                        else if (gvFinalizar.GetRowCellValue(i, "MontoTotal") == null || (int)gvFinalizar.GetRowCellValue(i, "MontoTotal") <= 0)
                        {
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", 0);
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", decimal.Round((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value), 2, MidpointRounding.AwayFromZero));
                        }
                    }
                }
                else if (_esSubsidio)
                {
                    for (int i = 0; i < EtVista.Count(c => c.EmpeladoSeleccionado); i++)
                    {
                        decimal valor = 0;

                        valor = spCantidadDias.Value;
                        gvFinalizar.SetRowCellValue(i, colCantidadDias, valor);
                        if (gvFinalizar.GetRowCellValue(i, "MontoCuota") != null)
                        {
                            gvFinalizar.SetRowCellValue(i, "MontoCuota", decimal.Round(((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value) * 0.4m), 2, MidpointRounding.AwayFromZero));
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", decimal.Round(((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value)), 2, MidpointRounding.AwayFromZero));
                        }
                        else if (gvFinalizar.GetRowCellValue(i, "MontoCuota") == null || (int)gvFinalizar.GetRowCellValue(i, "MontoCuota") <= 0)
                        {
                            gvFinalizar.SetRowCellValue(i, "MontoCuota", 0);
                            gvFinalizar.SetRowCellValue(i, "MontoCuota", decimal.Round(((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value) * 0.4m), 2, MidpointRounding.AwayFromZero));
                            gvFinalizar.SetRowCellValue(i, "MontoTotal", decimal.Round(((Convert.ToDecimal(gvFinalizar.GetRowCellValue(i, "SalarioDiario")) * spCantidadDias.Value)), 2, MidpointRounding.AwayFromZero));
                        }
                    }
                }
                
                this.gvFinalizar.RefreshData();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        #endregion

        //public static DateTime GetNextQuincena(DateTime fecha)
        //{
        //    DateTime date = new DateTime();
        //    try
        //    {
        //        int dia = fecha.Day;
        //        if (dia <= 15)
        //            date = new DateTime(fecha.Year, fecha.Month, 15);
        //        else
        //        {
        //            //date = new DateTime(fecha.Year, fecha.Month + 1, 1);                
        //            date = new DateTime(fecha.AddMonths(1).Year, fecha.AddMonths(1).Month, 1);
        //            date = date.AddDays(-1); //Ultimo dia del mes
        //        }
        //        return date;
        //    }
        //    catch (Exception ex)
        //    {
        //        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
        //        return date;
        //    }
        //}

        private bool Guardar()
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                    var ListToSave = EtVista.Where(ts => ts.EmpeladoSeleccionado && ts.MontoTotal > 0);
                    
                    foreach (Entidad.VistaMovimientoEmpleado vme in ListToSave)
                    {
                        Entidad.MovimientoEmpleado ME = new Entidad.MovimientoEmpleado();

                        ME.EmpleadoID = vme.EmpleadoID;
                        ME.Descripcion = vme.Descripcion;
                        int almacen = 0;
                        if (_esIngreso)
                        {
                            ME.FechaInicial = ME.FechaFinal = FechaInicial.Date;
                            ME.CantidadCuotas = 1;
                            if (_esSubsidio)
                            {
                                ME.FechaFinal = FechaFinal.Date;
                                ME.QuitarDePlanilla = vme.QuitarDePlanilla;
                            }

                        }
                        else if (!_esIngreso)
                        {
                            ME.FechaInicial = FechaInicial.Date;
                            ME.FechaFinal = FechaFinal.Date;
                            ME.CantidadCuotas = vme.CantidadCuotas;
                            almacen = ME.CantidadCuotas;
                        }

                        if (HoraExtra)
                        {
                            decimal valor = decimal.Round(ME.SalarioDiario / 8m, 2, MidpointRounding.AwayFromZero);
                            ME.SalarioDiario = valor;
                        }
                        else
                        {
                            decimal valor = (ListaSalarios.Count(o => o.ID.Equals(ME.EmpleadoID)) > 0 ? Convert.ToDecimal(ListaSalarios.First(o => o.ID.Equals(ME.EmpleadoID)).Display) : 0m);
                            ME.SalarioDiario = valor;
                        }

                        ME.CantidadDias = vme.CantidadDias;

                        if (_esAusencia)
                        {
                            ME.TipoAusenciaID = Convert.ToInt32(lkeTipoMovimiento.EditValue);
                            ME.TipoMovimientoEmpleadoID = _MovimientoAusencia;
                        }
                        else
                            ME.TipoMovimientoEmpleadoID = Convert.ToInt32(lkeTipoMovimiento.EditValue);

                        ME.QuitarDePlanilla = vme.QuitarDePlanilla;    
                        ME.SalarioDiario = vme.SalarioDiario;
                        ME.CantidadHE = vme.CantidadHorasExtras;
                        ME.MontoTotal = vme.MontoTotal;
                        ME.MonedaID = Parametros.Config.MonedaPrincipal();
                        ME.TipoCambio = _TipoCambio;
                        ME.MontoMonedaSecundaria = Math.Abs(Decimal.Round(vme.MontoTotal / ME.TipoCambio, 2, MidpointRounding.AwayFromZero));
                        ME.MontoCuota = vme.MontoCuota;
                        
                        decimal Monto = vme.MontoCuota;

                        db.MovimientoEmpleado.InsertOnSubmit(ME);
                        db.SubmitChanges();

                        if (_deuda == true)
                        {
                            var index = (from d in db.MovimientoEmpleado
                                         select new { id = d.ID }).OrderByDescending(d => d.id).First();
                            
                            for (int g = 0; g < almacen; g++)
                            {
                                Entidad.DetalleMovimientoPlanilla DMP = new Entidad.DetalleMovimientoPlanilla();
                                DMP.MovimientoEmpleadoID = index.id;
                                DMP.Monto = Monto;
                                DMP.Cuotas = (g + 1);
                                DMP.TipoMovimientoEmpleadoID = _MovimientoAusencia;
                                db.DetalleMovimientoPlanilla.InsertOnSubmit(DMP);
                            }
                        }
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se registró : " + Convert.ToString(lkeTipoMovimiento.Text) + " al Empleado: " + vme.EmpleadoNombreCompleto,this.Name);
                        db.SubmitChanges();
                    }

                    trans.Commit();                   
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    RefreshMDI = true;
                    _Editing = false;

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

        
        
    

    }
}