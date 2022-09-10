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
using SAGAS.Nomina.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogMovimientoCorteEmpleado : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormMovimientoCorteEmpleado MDI;
        internal Entidad.MovimientoHorasExtras EtAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;        
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID;
        private bool _Guardado = false;
        private bool _Editable = false;
        public int _IDHE = 0;
        internal int _MovimientoHE;
        private List<Parametros.ListIdCodeDisplay> Plan;
        //private IQueryable<Parametros.ListIdDisplay> listaTanque;
        internal int _MonedaPrimaria;
        internal DateTime _FechaActual;
        internal string _Nombre, _Direccion, _Telefono;
        internal System.Drawing.Image _picture_LogoEmpresa;
        
        private string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }

        private string Comentario
        {
            get { return mmComentario.Text; }
            set { mmComentario.Text = value; }
        }
        
        private DateTime FechaDesde
        {
            get { return Convert.ToDateTime(dateDesde.EditValue); }
            set { dateDesde.EditValue = value; }
        }

        private DateTime FechaHasta
        {
            get { return Convert.ToDateTime(dateHasta.EditValue); }
            set { dateHasta.EditValue = value; }
        }

        private int IDPlanilla
        {
            get { return Convert.ToInt32(lkPlanilla.EditValue); }
            set { lkPlanilla.EditValue = value; }
        }

        private static Entidad.Cliente client;

        private List<Entidad.VistaMovimientoEmpleadoHorasExtras> ME;
      

        #endregion

        #region *** INICIO ***

        public DialogMovimientoCorteEmpleado(int UserID, bool _editando)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            _Editable = _editando;
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

                //Carga de Planillas
                Plan = (from pl in db.Planillas 
                               join es in db.EstacionServicios on pl.EstacionServicioID equals es.ID
                           where pl.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == UsuarioID && ges.EstacionServicioID == es.ID))
                           select new Parametros.ListIdCodeDisplay { ID = pl.ID, Display = pl.Nombre, Code = pl.NumeroPlanilla }).ToList();

                lkPlanilla.Properties.DataSource = Plan.Select(s => new { s.ID, s.Display });
                _MovimientoHE = Parametros.Config.MovimientoHoraExtraID();

                //Asignación por defecto de la planilla
                if (Plan.Count > 0)
                {
                    IDPlanilla = Plan.First().ID;
                    Codigo = Plan.First().Code.ToString();
                    lblEmpresa.Text = _Nombre + " REGISTRO DE HORAS EXTRAS";
                }

                _FechaActual = Convert.ToDateTime(db.GetDateServer());
                FechaDesde = FechaHasta = _FechaActual;

                if (Parametros.General.Empresa != null)
                {
                    Parametros.General.GetCompanyData(out _Nombre, out _Direccion, out _Telefono, out _picture_LogoEmpresa);
                    picLogo.Image = _picture_LogoEmpresa;
                }

                if (_IDHE > 0)
                {
                    EtAnterior = db.MovimientoHorasExtras.Single(o => o.ID.Equals(_IDHE));
                    IDPlanilla = EtAnterior.PlanillaID;
                    FechaDesde = EtAnterior.FechaInicial;
                    FechaHasta = EtAnterior.FechaFinal;
                    Comentario = EtAnterior.Comment;
                }

                Parametros.General.splashScreenManagerMain.CloseWaitForm();

                if (_Editable)
                    btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(mmComentario, errRequiredField);
        }

        public bool ValidarCampos(bool detalle)
        {
            try
            {
                if (mmComentario.Text == "" || lkPlanilla.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del movimiento.", Parametros.MsgType.warning);
                    return false;
                }

                if (dateDesde.EditValue == null || dateHasta.EditValue == null)
                {
                    Parametros.General.DialogMsg("Debe seleccionar las fechas de las horas extras.", Parametros.MsgType.warning);
                    return false;
                }

                if (Convert.ToDateTime(dateDesde.EditValue) > Convert.ToDateTime(dateHasta.EditValue))
                {
                    Parametros.General.DialogMsg("La fecha inicial debe ser menor a la fecha final.", Parametros.MsgType.warning);
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

                if (detalle)
                {
                    if (ME.Count <= 0)
                    {
                        Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                        return false;
                    }
                    
                }

                return true;
            }
            catch { return false; }
        }

        private bool Guardar()
        {
            if (!ValidarCampos(true))
                return false;

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.MovimientoHorasExtras MHE;

                    if (_Editable)
                    {
                        MHE = db.MovimientoHorasExtras.Single(s => s.ID.Equals(EtAnterior.ID));
                    }
                    else
                    {
                        MHE = new Entidad.MovimientoHorasExtras();
                        MHE.PlanillaID = IDPlanilla;
                    }
                    
                    MHE.Comment = Comentario;
                    MHE.FechaInicial = FechaDesde;
                    MHE.FechaFinal = FechaHasta;

                    if (!_Editable)
                        db.MovimientoHorasExtras.InsertOnSubmit(MHE);

                    db.SubmitChanges();

                    #region ::: REGISTRANDO EN MOVIMIENTOS EMPLEADOS DE BD :::
                    db.MovimientoEmpleado.DeleteAllOnSubmit(db.MovimientoEmpleado.Where(o => o.MovimientoHorasExtrasID.Equals(MHE.ID)));
                    db.SubmitChanges();
                    //------------------------------ INSERTAR DATOS MOVIMIENTOS EMPLEADO ------------------------------//
                    foreach (var dm in ME)
                    {
                        Entidad.MovimientoEmpleado M = new Entidad.MovimientoEmpleado();
                        M.TipoMovimientoEmpleadoID = _MovimientoHE;
                        M.EmpleadoID = dm.EmpleadoID;
                        M.Descripcion = dm.Descripcion;
                        M.FechaInicial = MHE.FechaInicial;
                        M.FechaFinal = MHE.FechaFinal;
                        M.SalarioDiario = dm.SalarioDiario;
                        M.CantidadHE = dm.CantidadHE;
                        M.MontoTotal = dm.MontoTotal;
                        M.CantidadHMitrab = dm.CantidadHMitrab;
                        M.CantidadHorasLab = dm.CantidadHorasLab;
                        M.MovimientoHorasExtrasID = MHE.ID;

                        db.MovimientoEmpleado.InsertOnSubmit(M);
                        db.SubmitChanges();
                    }

                    #endregion

                    if (!_Editable)
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se registró las Horas Extras de : " + lkPlanilla.Text + " para la fecha del " + FechaDesde.ToShortDateString() + " Hasta " + FechaHasta.ToShortDateString(), this.Name);
                    }

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
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
            MDI.CleanDialog(ShowMsg, RefreshMDI);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado)
            {
                if (ME != null || Comentario != "")
                {
                    if (Parametros.General.DialogMsg("El movimiento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
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

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            
            ////-- Validar Columna de Productos             
            //if (view.GetRowCellValue(RowHandle, "ProductoID") != null)
            //{
            //    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")) == 0)
            //    {
            //        view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
            //        e.ErrorText = "Debe Seleccionar un Producto";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["ProductoID"], "Debe Seleccionar un Producto");
            //    e.ErrorText = "Debe Seleccionar un Producto";
            //    e.Valid = false;
            //    Validate = false;
            //}


            ////-- Validar Columna de Cantidad
            ////--
            //if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
            //{
            //    if (Convert.ToDouble(view.GetRowCellValue(RowHandle, "CantidadSalida")) <= 0.00)
            //    {

            //        view.SetColumnError(view.Columns["CantidadSalida"], "La Cantidad debe ser mayor a cero");
            //        e.ErrorText = "La Cantidad debe ser mayor a cero";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["CantidadSalida"], "La Cantidad debe ser mayor a cero");
            //    e.ErrorText = "La Cantidad debe ser mayor a cero";
            //    e.Valid = false;
            //    Validate = false;
            //}

            ////-- Validar Columna de Tanque             
            //if (view.GetRowCellValue(RowHandle, "AlmacenSalidaID") != null)
            //{
            //    if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID")) == 0)
            //    {
            //        view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Tanque");
            //        e.ErrorText = "Debe Seleccionar un Tanque";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //    else
            //    {
            //        if (db.Tanques.Count(t => t.ID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "AlmacenSalidaID"))) && t.ProductoID.Equals(Convert.ToInt32(view.GetRowCellValue(RowHandle, "ProductoID")))).Equals(0))
            //        {
            //            view.SetColumnError(view.Columns["AlmacenSalidaID"], "El Tanque seleccionado no contiene este producto");
            //            e.ErrorText = "El Tanque seleccionado no contiene este producto";
            //            e.Valid = false;
            //            Validate = false;
            //        }
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["AlmacenSalidaID"], "Debe Seleccionar un Tanque");
            //    e.ErrorText = "Debe Seleccionar un Tanque";
            //    e.Valid = false;
            //    Validate = false;
            //}

            ////-- Validar Existencia             
            //if (view.GetRowCellValue(RowHandle, "CantidadSalida") != null)
            //{
            //    if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")).Equals(0))
            //    {
            //        if (view.GetRowCellValue(RowHandle, "CantidadEntrada") != null)
            //        {
            //            if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadEntrada")).Equals(0))
            //            {
            //                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) > Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadEntrada")))
            //                {
            //                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
            //                    e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
            //                    e.Valid = false;
            //                    Validate = false;
            //                }
            //            }
            //            else
            //            {
            //                view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
            //                e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
            //                e.Valid = false;
            //                Validate = false;
            //            }
            //        }
            //        else
            //        {
            //            view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
            //            e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
            //            e.Valid = false;
            //            Validate = false;
            //        }

            //        if (view.GetRowCellValue(RowHandle, "CantidadInicial") != null)
            //        {
            //        if (!Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")).Equals(0))
            //            {
            //                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadSalida")) > Convert.ToDecimal(view.GetRowCellValue(RowHandle, "CantidadInicial")))
            //                {
            //                    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
            //                    e.ErrorText = "La cantidad a despachar sobrepasa la existencia";
            //                    e.Valid = false;
            //                    Validate = false;
            //                }
            //            }
            //            else
            //            {
            //                view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
            //                e.ErrorText = "La cantidad a despachar sobrepasa la existencia";
            //                e.Valid = false;
            //                Validate = false;
            //            }
            //        }
            //        else
            //        {
            //            view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa el arqueo");
            //            e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
            //            e.Valid = false;
            //            Validate = false;
            //        }
            //    }
            //    else
            //    {
            //        view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar debe ser mayor a 0 (cero)");
            //        e.ErrorText = "La cantidad a despachar debe ser mayor a 0 (cero)";
            //        e.Valid = false;
            //        Validate = false;
            //    }
            //}
            //else
            //{
            //    view.SetColumnError(view.Columns["CantidadSalida"], "La cantidad a despachar sobrepasa la existencia");
            //    e.ErrorText = "La cantidad a despachar sobrepasa el arqueo";
            //    e.Valid = false;
            //    Validate = false;
            //}

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column == colCantidadHoras || e.Column == colHorasMitrab)
            {
                try
                {
                    if (e.Value == null)
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, e.Column, 0m);
                    }
                    else
                    {
                        decimal vSalarioHora = 0, vHorasLab = 0, vHorasMitrab = 0, vHorasExtras = 0, vMontoHE;

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "SalarioHora") != null)
                            vSalarioHora = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "SalarioHora")), 2, MidpointRounding.AwayFromZero);

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadHorasLab") != null)
                            vHorasLab = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadHorasLab")), 2, MidpointRounding.AwayFromZero);

                        if (gvDetalle.GetRowCellValue(e.RowHandle, "CantidadHMitrab") != null)
                            vHorasMitrab = Decimal.Round(Convert.ToDecimal(gvDetalle.GetRowCellValue(e.RowHandle, "CantidadHMitrab")), 2, MidpointRounding.AwayFromZero);
                        
                        decimal valor = vHorasLab - vHorasMitrab;

                        vHorasExtras = (valor > 0 ? valor : 0m);
                        gvDetalle.SetRowCellValue(e.RowHandle, "CantidadHE", vHorasExtras);

                        vMontoHE = Decimal.Round((vHorasExtras * vSalarioHora) * 2m, 2, MidpointRounding.AwayFromZero);
                        gvDetalle.SetRowCellValue(e.RowHandle, "MontoTotal", vMontoHE);

                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F7))
            {
                btnOK_Click_1(null, null);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }        
            
        private void lkPlanilla_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (IDPlanilla > 0)
                    Codigo = Plan.First(o => o.ID.Equals(IDPlanilla)).Code.ToString();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(false))
                return;

            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ACTUALIZANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                
                ME = dv.VistaMovimientoEmpleadoHorasExtras.Where(o => o.MovimientoHorasExtrasID.Equals(_IDHE)).ToList();

                var obj = (from em in db.Empleados
                              join c in db.Cargo on em.CargoID equals c.ID
                              where em.Activo && em.PlanillaID.Equals(IDPlanilla)
                              select new 
                              {
                              em.ID, 
                              em.Codigo, 
                              NombreCompleto = em.Nombres + " " + em.Apellidos, 
                              em.NumeroSeguroSocial, 
                              em.Cedula, 
                              em.SalarioActual,
                              Cargo = c.Nombre,
                              c.Orden
                              }).ToList();

                List<Entidad.VistaMovimientoEmpleadoHorasExtras> nuevos = new List<Entidad.VistaMovimientoEmpleadoHorasExtras>();

                obj.ForEach(det =>
                    {
                        if (!ME.Select(o => o.EmpleadoID).Contains(det.ID))
                        {
                            nuevos.Add(new Entidad.VistaMovimientoEmpleadoHorasExtras { EmpleadoID = det.ID, NumeroINSS = det.NumeroSeguroSocial, NombreCompleto = det.NombreCompleto, NumeroCedula = det.Cedula, Cargo = det.Cargo, Orden = det.Orden, SalarioActual = det.SalarioActual, SalarioDiario = Decimal.Round(det.SalarioActual / 30m, 2, MidpointRounding.AwayFromZero), SalarioHora = Decimal.Round((det.SalarioActual / 30m) / 8m, 2, MidpointRounding.AwayFromZero) });
                        }
                    });

                ME.AddRange(nuevos);

                this.bdsDetalle.DataSource = ME.OrderBy(o => o.Orden).ToList();
                gvDetalle.RefreshData();
                Parametros.General.splashScreenManagerMain.CloseWaitForm(); 

                this.btnLoad.Enabled = false;
                this.lkPlanilla.Enabled = false;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

    }
}