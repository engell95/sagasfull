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
using DevExpress.XtraTreeList.Nodes;

namespace SAGAS.Nomina.Forms.Dialogs
{
    public partial class DialogEmpleado : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormEmpleado MDI;
        internal Entidad.Empleado EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;

        public string Nombres
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value; }
        }

        public string Apellidos
        {
            get { return txtApellido.Text; }
            set { txtApellido.Text = value; }
        }

        public int IDPlanilla
        {
            get { return Convert.ToInt32(lkePlanilla.EditValue); }
            set { lkePlanilla.EditValue = value; }
        }

        public string Codigo
        {
            get { return txtCodigo.Text; }
            set { txtCodigo.Text = value; }
        }
        
        private string Cedula
        {
            get { return txtCedula.Text; }
            set { txtCedula.Text = value; }
        }

        private string Celular
        {
            get { return txtCelular.Text; }
            set { txtCelular.Text = value; }
        }

        private string Telefono
        {
            get { return txtTelefono.Text; }
            set { txtTelefono.Text = value; }
        }

        private string Sangre
        {
            get { return txtTipoSangre.Text; }
            set { txtTipoSangre.Text = value; }
        }

        private string Pariente
        {
            get { return txtPariente.Text; }
            set { txtPariente.Text = value; }
        }

        private string Email
        {
            get { return txtEmail.Text; }
            set { txtEmail.Text = value; }
        }
        
        private string Direccion
        {
            get { return mmoDireccion.Text; }
            set { mmoDireccion.Text = value; }
        }

        
        private int GeneroID
        {
            get { return Convert.ToInt32(lkeGenero.SelectedIndex); }
            set { lkeGenero.SelectedIndex = value; }
        }
                
        private int ProfesionOficio
        {
            get { return Convert.ToInt32(lkeProfesionOficio.EditValue); }
            set { lkeProfesionOficio.EditValue = value; }
        }

        private int EstadoCivilID
        {
            get { return Convert.ToInt32(lkeEstadoCivil.EditValue); }
            set { lkeEstadoCivil.EditValue = value; }
        }

        private Byte[] Foto
        {
            get
            {
                if (picFoto.Image == null) return null;
                else return Parametros.General.ImageToBytes(picFoto.Image);
            }
            set
            {
                if (value == null) picFoto.Image = null;
                else picFoto.Image = Parametros.General.BytesToImage(value);
            }
        }
        
        private DateTime FechaNacimiento
        {
            get { return Convert.ToDateTime(dateFechaNacimiento.EditValue); }
            set { dateFechaNacimiento.EditValue = value; }
        }

        private decimal Salario
        {
            get { return Convert.ToDecimal(speSalario.Value); }
            set { speSalario.Value = value; }
        }

        private int AreaID
        {
            get { return Convert.ToInt32(lkeArea.EditValue); }
            set { lkeArea.EditValue = value; }
        }

        private int CargoID
        {
            get { return Convert.ToInt32(lkeCargo.EditValue); }
            set { lkeCargo.EditValue = value; }
        }

        private DateTime FechaIngreso
        {
            get { return Convert.ToDateTime(dateFechaIngreso.EditValue); }
            set { dateFechaIngreso.EditValue = value; }
        }
        
        private DateTime FechaContrato
        {
            get { return Convert.ToDateTime(dateFechaContrato.EditValue); }
            set { dateFechaContrato.EditValue = value; }
        }

        private string NumeroNominaINSS
        {
            get { return txtNoNominaINSS.Text; }
            set { txtNoNominaINSS.Text = value; }
        }

        private string NumeroINSS
        {
            get { return txtNumeroINSS.Text; }
            set { txtNumeroINSS.Text = value; }
        }

        private DateTime FechaVacaciones
        {
            get { return Convert.ToDateTime(dateVacaciones.EditValue); }
            set { dateVacaciones.EditValue = value; }
        }

        private DateTime FechaAguinaldo
        {
            get { return Convert.ToDateTime(dateAguinaldo.EditValue); }
            set { dateAguinaldo.EditValue = value; }
        }

        public bool EsPension
        {
            get { return Convert.ToBoolean(chkPension.Checked); }
            set { chkPension.Checked = value; }
        }
                
        public bool EsMultiES
        {
            get { return Convert.ToBoolean(chkMultiES.Checked); }
            set { chkMultiES.Checked = value; }
        }

        public bool EsActivo
        {
            get { return Convert.ToBoolean(chkActivo.Checked); }
            set { chkActivo.Checked = value; }
        }

        public bool PorcentajePension
        {
            get { return Convert.ToBoolean(chkPorcentaje.Checked); }
            set { chkPorcentaje.Checked = value; }
        }

        public decimal Pension
        {
            get { return Convert.ToDecimal(spPension.Value); }
            set { spPension.Value = value; }
        }

        public decimal Bono
        {
            get { return Convert.ToDecimal(spBono.Value); }
            set { spBono.Value = value; }
        }
        
        public bool CalculoPromedio
        {
            get { return Convert.ToBoolean(chkCalculoPromedio.Checked); }
            set { chkCalculoPromedio.Checked = value; }
        }

        public bool ConTarjeta
        {
            get { return Convert.ToBoolean(chkPagoTarjeta.Checked); }
            set { chkPagoTarjeta.Checked = value; }
        }

        private string NumeroCuenta
        {
            get { return txtCuenta.Text; }
            set { txtCuenta.Text = value; }
        }

        private int EO
        {
            get
            {
                if (tlstEO.FocusedNode[colID] == null)
                    return 0;
                return Convert.ToInt32(tlstEO.FocusedNode[colID]);
            }
            set
            {
                if (tlstEO.Nodes.Count > 0)
                    SetFocusedNode(tlstEO.Nodes[0], value);
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogEmpleado()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
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

                lkePlanilla.Properties.DataSource = from p in db.Planillas where p.Activo select new {p.ID, p.Nombre };
                lkeProfesionOficio.Properties.DataSource = from o in db.ProfesionOficio where o.Activo select new { o.ID, o.Nombre };
                lkeEstadoCivil.Properties.DataSource = from e in db.EstadoCivil where e.Activo select new { e.ID, e.Nombre };
                lkeArea.Properties.DataSource = from e in db.AreaNomina where e.Activo select new { e.ID, e.Nombre };
                lkeCargo.Properties.DataSource = from e in db.Cargo where e.Activo select new { e.ID, e.Nombre };
                tlstEO.DataSource = db.EstructuraOrganizativa.Where(o => o.Activo);
                tlstEO.ExpandAll();
                ConTarjeta = false;
                layoutControlItemCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                if (Editable)
                {
                    Nombres = EntidadAnterior.Nombres;
                    Apellidos = EntidadAnterior.Apellidos;
                    IDPlanilla = EntidadAnterior.PlanillaID;
                    Codigo = EntidadAnterior.Codigo;
                    Cedula = EntidadAnterior.Cedula;
                    Celular = EntidadAnterior.Celular;
                    Telefono = EntidadAnterior.Telefono;
                    Sangre = EntidadAnterior.TipoSangre;
                    Pariente = EntidadAnterior.Pariente;
                    Email = EntidadAnterior.Email;
                    EsMultiES = EntidadAnterior.EsMultiEstacion;
                    Direccion = EntidadAnterior.Direccion;
                    GeneroID = EntidadAnterior.Genero;
                    ProfesionOficio = EntidadAnterior.ProfesionOficioID;
                    EstadoCivilID = EntidadAnterior.EstadoCivil;
                    Foto = EntidadAnterior.Foto;
                    Salario = EntidadAnterior.SalarioActual;
                    AreaID = EntidadAnterior.AreaNominaID;
                    CargoID = EntidadAnterior.CargoID;
                    NumeroNominaINSS = EntidadAnterior.NumeroNominaInss;
                    NumeroINSS = EntidadAnterior.NumeroSeguroSocial;
                    EsPension = EntidadAnterior.PagaPension;
                    PorcentajePension = EntidadAnterior.EsPorcentaje;
                    Pension = EntidadAnterior.MontoPension;
                    CalculoPromedio = EntidadAnterior.CalculoSalarioPromedioAguinaldo;
                    ConTarjeta = !EntidadAnterior.DiferenteDeTarjeta;
                    NumeroCuenta = EntidadAnterior.NumeroCuentaTarjeta;
                    EO = EntidadAnterior.EstructuraOrganizativaID;
                    EsActivo = EntidadAnterior.Activo;
                    Bono = EntidadAnterior.Bono;

                    if (EntidadAnterior.FechaIngreso != null)
                        FechaIngreso = Convert.ToDateTime(EntidadAnterior.FechaIngreso);

                    if (EntidadAnterior.FechaContrato != null)
                        FechaContrato = Convert.ToDateTime(EntidadAnterior.FechaContrato);

                    if (EntidadAnterior.FechaUltimaVacacion != null)
                        FechaVacaciones = Convert.ToDateTime(EntidadAnterior.FechaUltimaVacacion);

                    if (EntidadAnterior.FechaUltimoAguinaldo != null)
                        FechaAguinaldo = Convert.ToDateTime(EntidadAnterior.FechaUltimoAguinaldo);


                    if (EntidadAnterior.FechaNacimiento != null)
                        FechaNacimiento = Convert.ToDateTime(EntidadAnterior.FechaNacimiento);
                }
                else
                {
                    var arr = db.Empleados.Where(o => o.Activo).Select(s => s.Codigo);
                    int[] myInts = arr.Select(int.Parse).ToArray();

                    if (myInts.Count() > 0)
                        Codigo = Convert.ToString((myInts.Max() >= 50000 ? myInts.Max() + 1 : 50000));
                    else
                        Codigo = "50000";

                    EsActivo = true;

                    //var lCodigo = db.Empleados.Where(em => em.Activo).OrderByDescending(o => o.Codigo).FirstOrDefault().Codigo;

                    //if (lCodigo != null)

                    //    Codigo = lCodigo;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtCodigo, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtNombre, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(txtApellido, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkePlanilla, errRequiredField);
        }

        public bool ValidarCampos()
        {
            if (txtCodigo.Text == "" || txtNombre.Text == "" || txtApellido.Text == "" || lkePlanilla.EditValue == null)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            if (tlstEO.FocusedNode[colID] == null)
            {
                Parametros.General.DialogMsg("Debe seleccionar la Estructura Organizativa", Parametros.MsgType.warning);
                return false;
            }

            if (PorcentajePension)
            {
                if (Pension >= 100)
                {
                    Parametros.General.DialogMsg("El porcentaje de la pensión no debe ser mayor o igual al 100 %.", Parametros.MsgType.warning);
                    return false;
                }
            }
            else
            {
                if (Pension >= Salario)
                {
                    Parametros.General.DialogMsg("El porcentaje de la pensión no debe ser mayor o igual al Salario del trabajador.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (Salario <= 0)
            {
                if (Parametros.General.DialogMsg("El salario es 0 (cero), ¿Desea continuar?.", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

            if (ConTarjeta)
            {
                if (String.IsNullOrEmpty(NumeroCuenta))
                {
                    Parametros.General.DialogMsg("Debe ingresar el Número de Cuenta.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (dateFechaIngreso.EditValue == null)
            {
                Parametros.General.DialogMsg("Debe digitar la Fecha de Ingreso.", Parametros.MsgType.warning);
                return false;
            }
            else
            {
                if (FechaIngreso.Date < Convert.ToDateTime("01/01/2000"))
                {
                    Parametros.General.DialogMsg("Debe digitar una Fecha de Ingreso valida.", Parametros.MsgType.warning);
                    return false;
                }
            }

            if (!ValidarCodigo(Convert.ToString(txtCodigo.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
            {
                Parametros.General.DialogMsg("El código del Empleado '" + Convert.ToString(txtCodigo.Text) + "' ya esta registrado en el sistema, por favor seleccione otro código.", Parametros.MsgType.warning);
                return false;
            }

            return true;
        }

        private void SetFocusedNode(TreeListNode nodo, int valor)
        {
            foreach (TreeListNode listNo in nodo.Nodes)
            {
                if (listNo.GetValue(colID).Equals(valor))
                {
                    tlstEO.FocusedNode = listNo;
                    break;
                }

                if (listNo.HasChildren)
                    SetFocusedNode(listNo, valor);
            }
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Empleados
                          where (ID.HasValue ? i.Codigo == code && i.ID != Convert.ToInt32(ID) : i.Codigo == code)
                          select i);

            if (result.Count() > 0)
            {
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
                try
                {
                    Entidad.Empleado E;

                    if (Editable)
                    { E = db.Empleados.Single(e => e.ID == EntidadAnterior.ID); }
                    else 
                    {
                        E = new Entidad.Empleado();
                        E.Activo = true;
                    }

                    E.Nombres = Nombres;
                    E.Apellidos = Apellidos;
                    E.PlanillaID = IDPlanilla;
                    E.Codigo = Codigo;
                    E.Cedula = Cedula;
                    E.Celular = Celular;
                    E.Telefono = Telefono;
                    E.TipoSangre = Sangre;
                    E.Pariente = Pariente;
                    E.Email = Email;
                    E.Direccion = Direccion;
                    E.Genero = GeneroID;
                    E.ProfesionOficioID = ProfesionOficio;
                    E.EstadoCivil = EstadoCivilID;
                    E.Foto = Foto;
                    E.SalarioActual  = Salario;
                    E.EsMultiEstacion = EsMultiES;
                    E.AreaNominaID = AreaID;
                    E.CargoID = CargoID;
                    E.NumeroNominaInss = NumeroNominaINSS;
                    E.NumeroSeguroSocial = NumeroINSS;
                    E.PagaPension = EsPension;
                    E.EsPorcentaje = PorcentajePension;
                    E.MontoPension = Pension;
                    E.CalculoSalarioPromedioAguinaldo = CalculoPromedio;
                    E.DiferenteDeTarjeta = !ConTarjeta;
                    E.NumeroCuentaTarjeta = NumeroCuenta;
                    E.EstructuraOrganizativaID = EO;
                    E.Bono = Bono;
                    E.Activo = EsActivo;

                    if (FechaIngreso != null)
                    {
                        if (FechaIngreso.Date > Convert.ToDateTime("01/01/2000"))
                            E.FechaIngreso = FechaIngreso;
                    }

                    if (FechaContrato != null)
                    {
                        if (FechaContrato.Date > Convert.ToDateTime("01/01/2000"))
                            E.FechaContrato = FechaContrato;
                    }

                    if (FechaVacaciones != null)
                    {
                        if (FechaVacaciones.Date > Convert.ToDateTime("01/01/2000"))
                            E.FechaUltimaVacacion = FechaVacaciones;
                    }

                    if (FechaAguinaldo != null)
                    {
                        if (FechaAguinaldo.Date > Convert.ToDateTime("01/01/2000"))
                            E.FechaUltimoAguinaldo = FechaAguinaldo;
                    }

                    if (FechaNacimiento != null)
                    {
                        if (FechaNacimiento.Date > Convert.ToDateTime("01/01/1000"))
                            E.FechaNacimiento = FechaNacimiento;
                    }
                                 
                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(E, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                         "Se modificó el empleado: " + EntidadAnterior.Nombres + " " + EntidadAnterior.Apellidos, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Empleados.InsertOnSubmit(E);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina,
                        "Se creó el empleado: " + E.Nombres + " " + E.Apellidos, this.Name);

                    }

                    db.SubmitChanges();
                    trans.Commit();

                    ShowMsg = true;
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

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkPension_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPension.Checked)
            {
                layoutControlGroupPension.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else if (!chkPension.Checked)
            {
                chkPorcentaje.Checked = false;
                spPension.EditValue = 0;
                layoutControlGroupPension.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }

        #endregion        

        private void txtCodigo_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.BaseEdit)sender, errRequiredField);
        }

        private void chkPagoTarjeta_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPagoTarjeta.Checked)
            {
                layoutControlItemCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            else
            {
                NumeroCuenta = "";
                layoutControlItemCuenta.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
        }        
        
    }

    class ListTipoGenero
    {

        public int ID { get; set; }
        public string Nombre { get; set; }

        public ListTipoGenero()
        {

        }


        public ListTipoGenero(int _code, string _nombre)
        {
            this.ID = _code;
            this.Nombre = _nombre;

        }
        public List<ListTipoGenero> GetListTipoGenero()
        {
            List<ListTipoGenero> lista = new List<ListTipoGenero>();

            lista.Add(new ListTipoGenero((int)TipoGeneros.Masculino, "Masculino"));
            lista.Add(new ListTipoGenero((int)TipoGeneros.Femenino, "Femenino"));
            lista.Add(new ListTipoGenero((int)TipoGeneros.Otros, "Otros"));

            return lista;
        }
    }

    public enum TipoGeneros
    {
        Masculino = 0,
        Femenino = 1,
        Otros = 2
    }
}