using System;
using System.Collections.Generic;
using DevExpress.XtraTreeList.Nodes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms.Wizards
{

    public partial class wizCambioLaboral : Form
    {
        #region <<< DECLARACINOES  >>

        private SAGAS.Entidad.SAGASDataClassesDataContext db;
        private SAGAS.Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormTrasladoEmpleado MDI;
        private List<Entidad.VistaEmleados> EtVistaEmpleados;
        private List<Entidad.EstructuraOrganizativa> EtEstructuras;
        private EstructuraEmpleado EtEmpleado;
        private bool ShowMsg = false;
        private bool RefreshMDI = false;

        //Estructura del Empleado
        public struct EstructuraEmpleado
        {
            public int ID;
            public int PlanillaID;
            public string CodigoID;
            public string Nombres;
            public string Apellidos;
            public int CargoID;
            public int AreaNominaID;
            public int EstructuraOrganizativaID;
            public decimal SalarioActual;
        }

        private int EO
        {
            get
            {
                if (idEOTreeList.FocusedNode[colIDEO] == null)
                    return 0;
                return Convert.ToInt32(idEOTreeList.FocusedNode[colIDEO]);
            }
            set
            {
                if (idEOTreeList.Nodes.Count > 0)
                    SetFocusedNode(idEOTreeList.Nodes[0], value);
            }
        }

        #endregion

        #region <<< INICIO >>>
        public wizCambioLaboral()
        {
            InitializeComponent();
           
        }

        private void wizCambioLaboral_Load(object sender, EventArgs e)
        {
            try
            {//Establecimiento de la coneccion con el data class y
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                //Estructuras Organizativas
                EtEstructuras = db.EstructuraOrganizativa.Where(o => o.Activo).ToList();

                //Vista Empleado
                EtVistaEmpleados = dv.VistaEmleados.Where(a => a.Activo).ToList();
                BdsEmleados.DataSource = EtVistaEmpleados;
                this.gvEmpleados.RefreshData();

                FillCombos();
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion


        #region <<< METODOS >>>
        private void FillCombos()
        {
            // Aqui directamente se cargan directamente los combos
            lkNuevoCargoID.Properties.DataSource = lkCargoActualID.Properties.DataSource = db.Cargo.Where(o => o.Activo).Select(s => new { s.ID, s.Nombre});

            lkAreaNuevaID.Properties.DataSource = lkAreaActualID.Properties.DataSource = db.AreaNomina.Where(a => a.Activo).Select(s => new { s.ID, s.Nombre });

            lkNuevaPlanillaID.Properties.DataSource = lkActualPlanillaID.Properties.DataSource = db.Planillas.Where(a => a.Activo).Select(s => new { s.ID, s.Nombre });

            eoActualLookUpEdit.Properties.DataSource = idEOTreeList.DataSource = EtEstructuras.Select(s => new { s.ID, s.Nombre, s.PadreID }).ToList();
            idEOTreeList.ExpandAll();

            // Aqui se le llama desde el data source y se establece la conexion con la tabla, luego se le pone la condicion 
            //deseada y  se hace una consulta LinQ para llamar los datos
            motivoLookUpEdit.Properties.DataSource = db.TipoTraslado.Where(o => o.Activo).Select(s => new { s.ID, s.Nombre });
           
            //motivoLookUpEdit.Properties.DataSource = db.TipoTraslado.Where(mov => mov.Activo);
            //motivoLookUpEdit.Properties.DisplayMember = "Nombre";
            //motivoLookUpEdit.Properties.ValueMember = "ID";
        }

        private void SetFocusedNode(TreeListNode nodo, int valor)
        {
            foreach (TreeListNode listNo in nodo.Nodes)
            {
                if (listNo.GetValue(colIDEO).Equals(valor))
                {
                    idEOTreeList.FocusedNode = listNo;
                    break;
                }

                if (listNo.HasChildren)
                    SetFocusedNode(listNo, valor);
            }
        }

        private bool ValidarCampos()
        {
            // Sting contiene un metodo para leer  si esta nulo o no ISNULLOrEmpty
            if (motivoLookUpEdit.EditValue  == null || String.IsNullOrEmpty(observacionMemoEdit.Text))
            {
                Parametros.General.DialogMsg("Debe digitar el motivo y la observacion", Parametros.MsgType.warning);
                return false;
            }

            return true;

        }

        #endregion

        #region <<< EVENTOS >>>
        private void wizard_NextClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            try
            {
                if (e.Page == wpEmpleado)
                {
                    //Validar si el empleado es seleccionado
                    if (gvEmpleados.FocusedRowHandle >= 0)
                    {
                        //Obtener Empleado
                        //EtEmpleado = new List<EstructuraEmpleado>();
                        EtEmpleado = new EstructuraEmpleado();
                        // Para los valores que pueden retornar nulo hacemos una condicional para detectar si vienen nulo ? if de una fila , : Entonces
                        // if de una linea ? , : Entonces 
                        // EtEmpleado = EtVistaEmpleados.Where(o => o.ID.Equals(Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue(colIdEmpleado)))).Select(s => new EstructuraEmpleado { ID = s.ID, PlanillaID = (s.PlanillaID.HasValue ? (int)s.PlanillaID : 0) , CodigoID = s.Codigo, Nombres = s.Nombres, Apellidos = s.Apellidos, CargoID = (s.CargoID.HasValue ? (int)s.CargoID : 0), AreaNominaID = (s.AreaNominaID.HasValue ? (int)s.AreaNominaID : 0), EstructuraOrganizativaID = (s.EstructuraOrganizativaID.HasValue ? (int)s.EstructuraOrganizativaID : 0), SalarioActual = s.SalarioActual }).ToList();
                        EtEmpleado = EtVistaEmpleados.Where(o => o.ID.Equals(Convert.ToInt32(gvEmpleados.GetFocusedRowCellValue(colIdEmpleado)))).Select(s => new EstructuraEmpleado { ID = s.ID, PlanillaID = (s.PlanillaID.HasValue ? (int)s.PlanillaID : 0), CodigoID = s.Codigo, Nombres = s.Nombres, Apellidos = s.Apellidos, CargoID = (s.CargoID.HasValue ? (int)s.CargoID : 0), AreaNominaID = (s.AreaNominaID.HasValue ? (int)s.AreaNominaID : 0), EstructuraOrganizativaID = (s.EstructuraOrganizativaID.HasValue ? (int)s.EstructuraOrganizativaID : 0), SalarioActual = s.SalarioActual }).FirstOrDefault();

                        if (EtEmpleado.AreaNominaID > 0)
                            lkAreaNuevaID.EditValue = lkAreaActualID.EditValue = EtEmpleado.AreaNominaID;

                        if (EtEmpleado.CargoID > 0)
                            lkNuevoCargoID.EditValue = lkCargoActualID.EditValue = EtEmpleado.CargoID;

                        if (EtEmpleado.PlanillaID > 0)
                            lkNuevaPlanillaID.EditValue = lkActualPlanillaID.EditValue = EtEmpleado.PlanillaID;

                        if (EtEmpleado.EstructuraOrganizativaID > 0)
                            eoActualLookUpEdit.EditValue = EO = EtEmpleado.EstructuraOrganizativaID;

                        salarioNuevoSpinEdit.Value = salarioActualSpinEdit.Value = EtEmpleado.SalarioActual;

                       // Aqui al label empleado se le asigna Empleado  y se manda a llamar desde la estructura

                        lblEmpleado.Text = "Empleado: " + EtEmpleado.Nombres + " " + EtEmpleado.Apellidos;
                        lblEmpleadoFinal.Text = lblEmpleado.Text;


                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe seleccionar un Trabajador.", Parametros.MsgType.warning);
                        // Sirve para apoderar el evento y no avance
                        e.Handled = true;
                    }
                }
                if (e.Page == wpDatosLaborales)
                {
            //  Condicional de Area, Nuevo Cargo, Nueva Area, Salario, aqui se compara que cualquiera de ellos no sea nulo
                    if (lkAreaNuevaID.EditValue == null || lkNuevoCargoID.EditValue == null || lkNuevaPlanillaID.EditValue == null || salarioNuevoSpinEdit.Value <= 0)
                    {
                        Parametros.General.DialogMsg("Debe digitar toda la nueva información laboral.", Parametros.MsgType.warning);
                        e.Handled = true;
                        return;
                    }

                    // Creamos una varaiable Hay cambio, para luego comparar y pasar de parametros y asi comprobar
                    // si hay cambios
                    bool HayCambios = false;

                    if (!(lkCargoActualID.EditValue != null ? lkCargoActualID.EditValue : 0).Equals((lkNuevoCargoID.EditValue != null ? lkNuevoCargoID.EditValue : 0)))
                    {
                        HayCambios = true;    
                    }

                    if(!(lkAreaActualID.EditValue!= null ? lkAreaActualID.EditValue : 0).Equals(lkAreaNuevaID.EditValue != null ? lkAreaNuevaID.EditValue : 0))
                    {
                        HayCambios = true;  
                    }

                    if (!(lkActualPlanillaID.EditValue != null ? lkActualPlanillaID.EditValue : 0).Equals(lkNuevaPlanillaID.EditValue != null ? lkNuevaPlanillaID.EditValue : 0))
                    {
                        HayCambios = true;                    
                    }

                    if (!salarioActualSpinEdit.Value.Equals(salarioNuevoSpinEdit.Value))
                    {
                        HayCambios = true;
                    }

                    if (!(eoActualLookUpEdit.EditValue != null ? eoActualLookUpEdit.EditValue : 0).Equals(EO != null ? EO : 0))
                    {
                        HayCambios = true;
                    }

                    if (!HayCambios)
                    {
                        Parametros.General.DialogMsg("Debe hacer cambios laborales al empleado para poder continuar", Parametros.MsgType.warning);
                        e.Handled = true;
                        return;
                    }


                    lblArea.Text = "Area : " + lkAreaNuevaID.Text;
                    lblCargo.Text = "Cargo : " + lkNuevoCargoID.Text;
                    lblUbicacion.Text = "Ubicación : " + EtEstructuras.Single(s => s.ID.Equals(EO)).Nombre;
                    lblSalario.Text = "Salario: " + salarioNuevoSpinEdit.Value.ToString("N2");
                    lblPlanilla.Text = "Planilla: " + lkNuevaPlanillaID.Text;

                    
                }
            }
            catch (Exception ex)
            {               
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void wizard_PrevClick(object sender, DevExpress.XtraWizard.WizardCommandButtonClickEventArgs e)
        {
            try
            {
                if (e.Page == wpDatosLaborales)
                {
                    // AL retornar devuelvo a nulo los valores, el EditValue es quien trabaja
                    lkAreaNuevaID.EditValue = lkAreaActualID.EditValue = null;
                    lkNuevaPlanillaID.EditValue = lkActualPlanillaID.EditValue = null;
                    lkNuevoCargoID.EditValue = lkCargoActualID.EditValue = null;
                    salarioNuevoSpinEdit.Value = salarioActualSpinEdit.Value = 0m;
                }
            }
            catch (Exception ex)
            {               
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void wizard_FinishClick(object sender, CancelEventArgs e)
        {

            if (!ValidarCampos())
            {
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

        private bool Guardar()
        {

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 300;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    ////Datos Actuales, Mandamos a obtener los registros, anteriores y nuevos

                    Entidad.TrasladoEmpleado TE = new Entidad.TrasladoEmpleado();
                    TE.TipoTrasladoID = (int)motivoLookUpEdit.EditValue;
                    TE.EmpleadoID = Convert.ToInt32(EtEmpleado.ID);
                    TE.AreaAnteriorID = Convert.ToInt32(EtEmpleado.AreaNominaID);
                    TE.AreaNuevaID = Convert.ToInt32(lkAreaNuevaID.EditValue);
                    TE.CargoAnteriorID = (int)EtEmpleado.CargoID;
                    TE.CargoNuevoID = (int)lkNuevoCargoID.EditValue;
                    TE.SalarioAnterior = Convert.ToDecimal(EtEmpleado.SalarioActual);
                    TE.SalarioNuevo = Convert.ToDecimal(salarioNuevoSpinEdit.Value);
                    TE.PlanillaAnteriorID = (int)EtEmpleado.PlanillaID;
                    TE.PlanillaNuevaID = (int)lkNuevaPlanillaID.EditValue;
                    TE.UbicacionAnteriorID = (int)EtEmpleado.EstructuraOrganizativaID;
                    TE.UbicacionNuevaID = (int)EO;
                    TE.Fecha = Convert.ToDateTime(db.GetDateServer()).Date;
                    TE.EstructuraOrganizativaAnteriorID = (int)EtEmpleado.EstructuraOrganizativaID;
                    TE.EstructuraOrganizativaNuevaID = (int)EO;
                    TE.Observacion = observacionMemoEdit.Text;

                    // Aqui se asigna lo que se mandara a insertar a la tabla
                    db.TrasladoEmpleado.InsertOnSubmit(TE);
                    // Para mandar a guardar los cambios  
                    db.SubmitChanges();
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se Realizó el traslado de: " + EtEmpleado.Nombres + " " + EtEmpleado.Apellidos, this.Name);

                    //Actualizar el empleado
                    Entidad.Empleado E = db.Empleados.Single(s => s.ID.Equals(EtEmpleado.ID));
                    E.AreaNominaID = TE.AreaNuevaID;
                    E.CargoID = TE.CargoNuevoID;
                    E.EstructuraOrganizativaID = TE.EstructuraOrganizativaNuevaID;
                    E.SalarioActual = TE.SalarioNuevo;
                    E.PlanillaID = TE.PlanillaNuevaID;
                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    ShowMsg = true;
                    RefreshMDI = true;
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
        
        private void wizCambioLaboral_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, RefreshMDI);
        }

        #endregion

        private void wizard_CancelClick(object sender, CancelEventArgs e)
        {
            ShowMsg = false; RefreshMDI = false;
            this.Close();
        }

    }
}
