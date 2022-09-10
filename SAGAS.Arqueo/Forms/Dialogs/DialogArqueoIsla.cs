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
using SAGAS.Arqueo.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Popup;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogArqueoIsla : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormArqueoIsla MDI;
        internal Forms.FormResumenDia RDI;
        internal Entidad.ArqueoIsla EntidadAnterior;
        internal Entidad.ArqueoIsla ArqueoSelected;
        public int IDSUS = 0;
        internal Entidad.ArqueoProducto EtArqueoProducto;
        internal Entidad.ArqueoManguera EtArqueoManguera;
        public Entidad.ResumenDia ResumenSelected;
        public Entidad.Turno TurnoSelected;
        internal bool Editable = false;
        internal bool Modificando = false;
        private bool ShowMsg = false;
        private bool NextArqueo = false;
        private int UsuarioID;
        private int IDES = Parametros.General.EstacionServicioID;
        private DataTable dtDispensador;
        private DataTable dtMecanica;
        private DataTable dtEfectivo;
        private DataTable dtElectronica;
        //private DataTable dtArqueoProducto;
        //private DataTable dtArqueoManguera;
        private DataTable dtVerificacion;
        private DataTable dtVentas;
        private DataTable dtExtracciones;
        private DataTable dtTotalExtraxion;
        private DataTable dtPago;
        private DataTable dtConcepto;
        private DataTable dtDescuento;
        private DataTable dtPagoEfectivo;
        private List<Entidad.Tanque> Tanques;
        private decimal DiferenciaVerificacionLectura = Parametros.Config.DiferenciaVerificacionLectura();
        private decimal TipoCambioArqueo = 0;
        private int MonedaPrincipal = Parametros.Config.MonedaPrincipal();
        private int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int IDFormaPagoEfectivo = Parametros.Config.IDFormaPagoEfectivo();
        private decimal DescuentoPetroCard = Parametros.Config.DescuentoPetroCard();
        private decimal RangoEfectivo = Parametros.Config.RangoEfectivo();
        private decimal RangoMecElectronico = Parametros.Config.RangoMecElectronico();
        private int IDAI = 0;
        private bool MecanicaAmbasCara = false;
        private bool EsDispensadorProblemaLecturas = false;
        private decimal _GrandSobranteDiferencia = 0;


        public int IDIsla
        {
            get { return Convert.ToInt32(lkIsla.EditValue); }
            set { lkIsla.EditValue = value; }
        }

        private int IDTecnico
        {
            get { return Convert.ToInt32(lkTecnico.EditValue); }
            set { lkTecnico.EditValue = value; }
        }

        public string Observacion
        {
            get { return txtObservacion.Text; }
            set { txtObservacion.Text = value; }
        }

        public int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSES.EditValue); }
            set { lkSES.EditValue = value; }
        }

        //Tabla que mantiene los campos de los dispensadores a validar
        public DataTable Dispensador
        {
            get
            {
                return dtDispensador;
            }
            set
            {
                dtDispensador = value;
            }
        }

        public DataTable Mecanica
        {
            get
            {
                return dtMecanica;
            }
            set
            {
                dtMecanica = value;
            }
        }

        public DataTable Efectivo
        {
            get
            {
                return dtEfectivo;
            }
            set
            {
                dtEfectivo = value;
            }
        }

        public DataTable Electronica
        {
            get
            {
                return dtElectronica;
            }
            set
            {
                dtElectronica = value;
            }
        }

        #endregion

        #region *** INICIO ***

        /// <summary>
        /// Inicializa la ventana de arqueo de isla
        /// </summary>
        /// <param name="UserID">ID del usuario que esta ingresado en el sistema</param>
        public DialogArqueoIsla(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            if (FillControl())
            {

                ValidarerrRequiredField();

                if (Editable)
                    btnLoad_Click(sender, e);

                this.panelControlPrincipal.Location = new Point(2, 10);
            }
            else
                this.BeginInvoke(new MethodInvoker(this.Close));
        }

        #endregion

        #region *** METODOS ***


        /// <summary>
        /// Metodo para cargar los datos iniciales de esta ventana (Arqueo de Isla)
        /// </summary>
        private bool FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                
               
                if (Parametros.General.ListSES.Count > 0)
                {
                    this.layoutControlItemSES.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    //**SubEstación
                    lkSES.Properties.DataSource = db.SubEstacions.Where(sus => sus.Activo && (Parametros.General.ListSES.ToList().Contains(sus.ID))).ToList();
                    lkSES.Properties.DisplayMember = "Nombre";
                    lkSES.Properties.ValueMember = "ID";

                    if (Parametros.General.ListSES.Count.Equals(1))
                    {
                        IDSubEstacion = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                        lkSES.Properties.ReadOnly = true;
                    }
                }


                var ES = db.EstacionServicios.Single(e => e.ID.Equals(IDES));

                List<int> NoID = new List<int>();
                NoID.Add(ES.AdministradorID);
                NoID.Add(ES.ArqueadorID);
                NoID.Add(ES.ResponsableContableID);
                NoID.Add(Convert.ToInt32(db.Usuarios.Single(u => u.ID.Equals(UsuarioID)).EmpleadoID));

                IQueryable<Parametros.ListIdDisplay> listEm = (from em in db.Empleados
                                                               join c in db.Cargo on em.CargoID equals c.ID
                                                               join pl in db.Planillas on em.PlanillaID equals pl.ID
                                                               where em.Activo && (pl.EstacionServicioID == IDES || em.EsMultiEstacion) && c.EsBombero
                                                               && !(NoID.Contains(em.ID))
                                                               select new Parametros.ListIdDisplay { ID = em.ID, Display = em.Nombres + " " + em.Apellidos }).OrderBy(o => o.Display);
             
                List<Parametros.ListIdDisplay> listadoDisplay = new List<Parametros.ListIdDisplay>(listEm);             

                listadoDisplay.Add(new Parametros.ListIdDisplay(0, "Isla Cerrada"));
          
                lkTecnico.Properties.DataSource = listadoDisplay;
                lkTecnico.Properties.DisplayMember = "Display";
                lkTecnico.Properties.ValueMember = "ID";

                if (Modificando)
                {
                    TurnoSelected = EntidadAnterior.Turno;
                    ResumenSelected = TurnoSelected.ResumenDia;

                    if(TurnoSelected == null)
                        return false;

                    if (ResumenSelected == null)
                        return false;
                }
                else
                {
                    if (!ResumenDia())
                        return false;

                    if (!Turno())
                        return false;
                }
                //Cargando los datos predeterminados de la ventana
                txtEstacionServicio.Text = Parametros.General.EstacionServicioName;
                TipoCambioArqueo = ResumenSelected.TipoCambioMoneda;
                DescuentoPetroCard = ResumenSelected.DescuentoPetroCard;
                IDSubEstacion = ResumenSelected.SubEstacionID;

                txtTurno.Text = TurnoSelected.Numero.ToString();
                dateFecha.EditValue = ResumenSelected.FechaInicial;
                txtArqueador.Text = Parametros.General.UserName;
                lblTipoCambio.Text = "El Tipo de Cambio para este Arqueo es: " + TipoCambioArqueo.ToString("#,0.0000");  
                
                 //**Cargando las Islas asigndas a las estaciones de servicio del usuario registrado
                lkIsla.Properties.DataSource = from i in db.Islas
                                               where i.Activo && i.EstacionServicioID.Equals(IDES) && i.SubEstacionID.Equals(ResumenSelected.SubEstacionID)
                                                            select new { i.ID, i.Nombre };
                lkIsla.Properties.DisplayMember = "Nombre";
                lkIsla.Properties.ValueMember = "ID";

                if (Editable)
                {
                    ArqueoSelected = EntidadAnterior;
                    IDIsla = ArqueoSelected.IslaID;
                    IDTecnico = ArqueoSelected.TecnicoID;
                    Observacion = ArqueoSelected.Observacion;

                    if (Parametros.General.ListSES.Count > 0)
                    {
                        IDSubEstacion = ResumenSelected.SubEstacionID;
                    }

                    if (ArqueoSelected.Estado.Equals((int)Parametros.Estados.Modificado))
                        this.dateFecha.EditValue = ArqueoSelected.FechaCreado;
                }

                return true;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }

        /// <summary>
        /// Selecciona el Resumen del día
        /// </summary>
        private bool ResumenDia()
        {
            try
            {
                if (Parametros.General.ListSES.Count <= 0)
                {
                    if (db.ResumenDias.Count(r => !r.Cerrado && r.EstacionServicioID == IDES) > 0)
                    {
                        ResumenSelected = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES)).OrderByDescending(o => o.ID).First();
                        return true;
                    }
                    else
                    {
                        if (Parametros.General.DialogMsg("No existe Resumen del día Abierto, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {
                            DateTime fecha;
                            if (Convert.ToInt32(db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES))) > 0)
                            {
                                if (db.EstacionServicios.Single(es => es.ID.Equals(IDES)).AplicarFechaArqueo)
                                {
                                    fecha = (db.EstacionServicios.Single(es => es.ID.Equals(IDES)).FechaArqueo.HasValue ?
                                        Convert.ToDateTime(db.EstacionServicios.Single(es => es.ID.Equals(IDES)).FechaArqueo) :
                                        Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                }
                                else
                                    fecha = Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1);
                            }
                            else
                            {
                                if (db.EstacionServicios.Single(es => es.ID.Equals(IDES)).AplicarFechaArqueo)
                                {
                                    fecha = (db.EstacionServicios.Single(es => es.ID.Equals(IDES)).FechaArqueo.HasValue ?
                                        Convert.ToDateTime(db.EstacionServicios.Single(es => es.ID.Equals(IDES)).FechaArqueo) :
                                        Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                }
                                else
                                {
                                    using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(Convert.ToDateTime("01/02/2014"), Convert.ToDateTime(db.GetDateServer())))
                                    {
                                        df.layoutControlItemCaption.Text = "Fecha del Resumén del Día";
                                        df.Fecha = Convert.ToDateTime(db.GetDateServer());

                                        if (df.ShowDialog().Equals(DialogResult.OK))
                                        {
                                            fecha = df.Fecha;

                                            if (fecha.Equals(null))
                                            {
                                                Parametros.General.DialogMsg("Debe seleccionar una fecha valida!", Parametros.MsgType.warning);
                                                return false;
                                            }

                                            if (db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES) && r.FechaInicial.Date.Equals(fecha.Date)) > 0)
                                            {
                                                Parametros.General.DialogMsg("Ya existe un Resumen del Día con la fecha: " + fecha.ToShortDateString(), Parametros.MsgType.warning);
                                                return false;
                                            }

                                        }
                                        else
                                            return false;
                                    }
                                }
                            }

                            decimal TC = Parametros.Config.TipoCambioArqueo(fecha);

                            if (TC.Equals(0m))
                            {
                                Parametros.General.DialogMsg("El sistema no cargo bien el Tipo de Cambio para este Resumen del Día.", Parametros.MsgType.warning);
                                return false;
                            }

                            Entidad.ResumenDia RD = new Entidad.ResumenDia();

                            RD.FechaInicial = fecha;
                            RD.EstacionServicioID = IDES;
                            RD.UsuarioAbiertoID = UsuarioID;
                            RD.Numero = Convert.ToInt32(db.ResumenDias.Count(r => r.Cerrado && r.EstacionServicioID == IDES)) > 0 ? Convert.ToInt32(db.ResumenDias.Where(r => r.Cerrado && r.EstacionServicioID == IDES).OrderByDescending(o => o.ID).First().Numero + 1) : 1;
                            RD.TipoCambioMoneda = TC;
                            RD.Cerrado = false;
                            RD.DescuentoPetroCard = DescuentoPetroCard;
                            db.ResumenDias.InsertOnSubmit(RD);
                            db.SubmitChanges();

                            if (db.EstacionServicios.Single(es => es.ID.Equals(IDES)).AplicarFechaArqueo)
                            {
                                Entidad.EstacionServicio ES = db.EstacionServicios.Single(es => es.ID.Equals(IDES));
                                ES.AplicarFechaArqueo = false;
                                db.SubmitChanges();
                            }
                            
                            ResumenSelected = RD;
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else if (Parametros.General.ListSES.Count > 0)
                {
                    if (Parametros.General.ListSES.Count.Equals(1))
                    {
                        if (db.ResumenDias.Count(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))) > 0)
                        {
                            ResumenSelected = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First();
                            return true;
                        }
                        else
                        {
                            if (Parametros.General.DialogMsg("No existe Resumen del día Abierto, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                DateTime fecha;
                                int IDSES = 0;
                                if (Convert.ToInt32(db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault()))))) > 0)
                                {
                                    if (db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).AplicarFechaArqueo)
                                    {
                                        fecha = (db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).FechaArqueo.HasValue ?
                                            Convert.ToDateTime(db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).FechaArqueo) :
                                            Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                        IDSES = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                                    }
                                    else
                                    {
                                        fecha = Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1);
                                        IDSES = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                                    }
                                }
                                else
                                {
                                    if (db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).AplicarFechaArqueo)
                                    {
                                        fecha = (db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).FechaArqueo.HasValue ?
                                            Convert.ToDateTime(db.SubEstacions.Single(es => es.ID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).FechaArqueo) :
                                            Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                        IDSES = Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault());
                                    }
                                    else
                                    {
                                        using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(Convert.ToDateTime("01/02/2014"), Convert.ToDateTime(db.GetDateServer())))
                                        {
                                            df.layoutControlItemCaption.Text = "Fecha del Resumén del Día";
                                            df.Fecha = Convert.ToDateTime(db.GetDateServer());


                                            if (df.ShowDialog().Equals(DialogResult.OK))
                                            {
                                                fecha = df.Fecha;
                                                IDSES = df.IDSubEstacion;

                                                if (fecha.Equals(null))
                                                {
                                                    Parametros.General.DialogMsg("Debe seleccionar una fecha valida!", Parametros.MsgType.warning);
                                                    return false;
                                                }

                                                if (db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES) && r.FechaInicial.Date.Equals(fecha.Date) && r.SubEstacionID.Equals(Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault()))) > 0)
                                                {
                                                    Parametros.General.DialogMsg("Ya existe un Resumen del Día con la fecha: " + fecha.ToShortDateString(), Parametros.MsgType.warning);
                                                    return false;
                                                }

                                            }
                                            else
                                                return false;
                                        }
                                    }
                                }

                                decimal TC = Parametros.Config.TipoCambioArqueo(fecha);
                                
                                if (TC.Equals(0m))
                                {
                                    Parametros.General.DialogMsg("El sistema no cargo bien el Tipo de Cambio para este Resumen del Día.", Parametros.MsgType.warning);
                                    return false;
                                }

                                Entidad.ResumenDia RD = new Entidad.ResumenDia();

                                RD.FechaInicial = fecha;
                                RD.EstacionServicioID = IDES;
                                RD.SubEstacionID = IDSES;
                                RD.UsuarioAbiertoID = UsuarioID;
                                RD.Numero = Convert.ToInt32(db.ResumenDias.Count(r => r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault()))))) > 0 ? Convert.ToInt32(db.ResumenDias.Where(r => r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().Numero + 1) : 1;
                                RD.TipoCambioMoneda = TC;
                                RD.Cerrado = false;
                                RD.DescuentoPetroCard = DescuentoPetroCard;
                                db.ResumenDias.InsertOnSubmit(RD);
                                db.SubmitChanges();

                                if (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).AplicarFechaArqueo)
                                {
                                    Entidad.SubEstacion SES = db.SubEstacions.Single(es => es.ID.Equals(IDSES));
                                    SES.AplicarFechaArqueo = false;
                                    db.SubmitChanges();
                                }

                                ResumenSelected = RD;
                                return true;

                            }
                            else
                                return false;

                        }

                    }
                    else if (Parametros.General.ListSES.Count > 1)
                    {
                        DateTime fecha;
                        int IDSES = 0;

                        if (!Editable)
                        {
                            using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(true))
                            {
                                df.layoutControlItemCaption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                                if (df.ShowDialog().Equals(DialogResult.OK))
                                {
                                    IDSES = df.IDSubEstacion;
                                }
                                else
                                    return false;
                            }
                        }
                        else
                            IDSES = IDSUS;


                            if (db.ResumenDias.Count(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES)) > 0)
                            {
                                ResumenSelected = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES)).OrderByDescending(o => o.ID).First();
                                return true;
                            }
                            else
                            {
                                if (Parametros.General.DialogMsg("No existe Resumen del día Abierto, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                                {
                                    if (Convert.ToInt32(db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES))) > 0)
                                    {
                                        if (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).AplicarFechaArqueo)
                                        {
                                            fecha = (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).FechaArqueo.HasValue ?
                                                Convert.ToDateTime(db.SubEstacions.Single(es => es.ID.Equals(IDSES)).FechaArqueo) :
                                                Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                        }
                                        else
                                            fecha = Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES)).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1);
                                    }
                                    else
                                    {
                                        if (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).AplicarFechaArqueo)
                                        {
                                            fecha = (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).FechaArqueo.HasValue ?
                                                Convert.ToDateTime(db.SubEstacions.Single(es => es.ID.Equals(IDSES)).FechaArqueo) :
                                                Convert.ToDateTime(db.ResumenDias.Where(r => r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals((Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault())))).OrderByDescending(o => o.ID).First().FechaInicial).AddDays(1));
                                        }
                                        else
                                        {
                                            using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(Convert.ToDateTime("01/02/2014"), Convert.ToDateTime(db.GetDateServer())))
                                            {
                                                df.IsReadOnly = true;
                                                df.IDSubEstacion = IDSES;
                                                df.layoutControlItemCaption.Text = "Fecha del Resumén del Día";
                                                df.Fecha = Convert.ToDateTime(db.GetDateServer());

                                                if (df.ShowDialog().Equals(DialogResult.OK))
                                                {
                                                    fecha = df.Fecha;

                                                    if (fecha.Equals(null))
                                                    {
                                                        Parametros.General.DialogMsg("Debe seleccionar una fecha valida!", Parametros.MsgType.warning);
                                                        return false;
                                                    }

                                                    if (db.ResumenDias.Count(r => r.EstacionServicioID.Equals(IDES) && r.FechaInicial.Date.Equals(fecha.Date) && r.SubEstacionID.Equals(IDSES)) > 0)
                                                    {
                                                        Parametros.General.DialogMsg("Ya existe un Resumen del Día con la fecha: " + fecha.ToShortDateString(), Parametros.MsgType.warning);
                                                        return false;
                                                    }

                                                }
                                                else
                                                    return false;
                                            }
                                        }
                                    }

                                    decimal TC = Parametros.Config.TipoCambioArqueo(fecha);

                                    if (TC.Equals(0m))
                                    {
                                        Parametros.General.DialogMsg("El sistema no cargo bien el Tipo de Cambio para este Resumen del Día.", Parametros.MsgType.warning);
                                        return false;
                                    }

                                    Entidad.ResumenDia RD = new Entidad.ResumenDia();

                                    RD.FechaInicial = fecha;
                                    RD.EstacionServicioID = IDES;
                                    RD.SubEstacionID = IDSES;
                                    RD.UsuarioAbiertoID = UsuarioID;
                                    RD.Numero = Convert.ToInt32(db.ResumenDias.Count(r => r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES))) > 0 ? Convert.ToInt32(db.ResumenDias.Where(r => r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSES)).OrderByDescending(o => o.ID).First().Numero + 1) : 1;
                                    RD.TipoCambioMoneda = TC;
                                    RD.Cerrado = false;
                                    RD.DescuentoPetroCard = DescuentoPetroCard;
                                    db.ResumenDias.InsertOnSubmit(RD);
                                    db.SubmitChanges();

                                    if (db.SubEstacions.Single(es => es.ID.Equals(IDSES)).AplicarFechaArqueo)
                                    {
                                        Entidad.SubEstacion SES = db.SubEstacions.Single(es => es.ID.Equals(IDSES));
                                        SES.AplicarFechaArqueo = false;
                                        db.SubmitChanges();
                                    }

                                    ResumenSelected = RD;
                                    return true;

                                }
                                else
                                    return false;

                            }


                    }
                    else
                        return false;
                }
                else 
                    return false;

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }

        /// <summary>
        /// Selecciona el Turno
        /// </summary>
        private bool Turno()
        {
            try
            {
                if (db.Turnos.Count(t => !t.Cerrada && t.ResumenDiaID == ResumenSelected.ID && !t.Especial) > 0)
                {
                    TurnoSelected = db.Turnos.Where(r => !r.Cerrada && r.ResumenDiaID.Equals(ResumenSelected.ID) && !r.Especial).OrderByDescending(o => o.ID).First();
                    return true;
                }
                else
                {
                    if (Parametros.General.DialogMsg("No existe un Turno Abierto, ¿Desea Abrir uno nuevo?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                    {
                        Entidad.Turno T = new Entidad.Turno();

                        T.FechaInicial = ResumenSelected.FechaInicial;
                        T.UsuarioAbiertoID = UsuarioID;
                        T.ResumenDiaID = ResumenSelected.ID;
                        T.Numero = Convert.ToInt32(db.Turnos.Count(r => r.Cerrada && r.ResumenDiaID.Equals(ResumenSelected.ID) && !r.Especial)) > 0 ? Convert.ToInt32(db.Turnos.Where(r => r.Cerrada && r.ResumenDiaID.Equals(ResumenSelected.ID) && !r.Especial).OrderByDescending(o => o.ID).First().Numero) + 1 : 1;

                        db.Turnos.InsertOnSubmit(T);
                        db.SubmitChanges();
                        TurnoSelected = T;
                        return true;
                    }
                    else
                        return false;
                }                             
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }
        }

        #region <<< Validaciones >>>
        /// <summary>
        /// Metodo para validar los campos requeridos del sistema
        /// </summary>
        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(lkIsla, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(lkTecnico, errRequiredField);
        }

        //Validar los campos del encabezado del arqueo
        public bool ValidarCampos()
        {
            if (!Editable)
            {
                if (lkIsla.EditValue == null || lkTecnico.EditValue == null)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + " Para poder cargar el detalle del arqueo ", Parametros.MsgType.warning);
                    return false;
                }

                if (this.chkArqueoEspecial.Checked)
                {
                    if (Parametros.General.DialogMsg("¿Esta seguro de generar este arqueo como un arqueo especial?", Parametros.MsgType.question) != DialogResult.OK)
                        return false;
                }

                if (Convert.ToInt32(db.ArqueoIslas.Count(a => a.TurnoID.Equals(TurnoSelected.ID) && a.IslaID.Equals(IDIsla) && (!a.ArqueoEspecial || (a.ArqueoEspecial && a.Oficial)))) > 0 && !chkArqueoEspecial.Checked)
                {
                    Parametros.General.DialogMsg("No se puede crear el arqueo." + Environment.NewLine + "La isla: " + Convert.ToString(db.Islas.Single(i => i.ID == IDIsla).Nombre) + " ya se encuentra arqueada para este turno.", Parametros.MsgType.warning);
                    return false;
                }

                //if (Parametros.General.ListSES.Count > 0)
                //{
                //    if (Convert.ToInt32(db.ArqueoIslas.Count(a => a.TurnoID.Equals(TurnoSelected.ID) && a.IslaID.Equals(IDIsla))) > 0 && !chkArqueoEspecial.Checked)
                //    {
                //        Parametros.General.DialogMsg("No se puede crear el arqueo." + Environment.NewLine + "La isla: " + Convert.ToString(db.Islas.Single(i => i.ID == IDIsla).Nombre) + " ya se encuentra arqueada para este turno.", Parametros.MsgType.warning);
                //        return false;
                //    }
                //}

                var Precios = from pc in db.PrecioCombustibles
                              join pcd in db.PrecioCombustibleDetalles on pc.ID equals pcd.PrecioCombustibleID
                              where pcd.EstacionServicioID.Equals(IDES) && pcd.SubEstacionID.Equals(ResumenSelected.SubEstacionID) && (pc.FechaInicial <= ResumenSelected.FechaInicial && pc.FechaFinal >= ResumenSelected.FechaInicial)
                              select pcd;

                if (Precios.Count() <= 0)
                {
                    Parametros.General.DialogMsg("No existe precio de venta para los combustibles, Favor comunicarse con Administración.", Parametros.MsgType.warning);
                    return false;
                }
            }
            return true;
        }

        public bool ValidarArqueo()
        {
            try
            {
                //for (int i = 0; i < dgvVerif.RowCount; i++)
                //{
                //    for (int j = 2; j < dgvVerif.Columns.Count; i++)
                //    {
                //        if (Convert.ToDecimal(dgvVerif.GetRowCellValue(i, dgvVerif.Columns[j])) > 5 || Convert.ToDecimal(dgvVerif.GetRowCellValue(i, dgvVerif.Columns[j])) < -5)
                //        {
                //            Parametros.General.DialogMsg("La verificación " + dgvVerif.GetRowCellValue(i, dgvVerif.Columns[1]) + " excede lo maximo permitido", Parametros.MsgType.warning);
                //            return false;
                //        }
                //    }

                //} 

                for (int i = 2; i < dgvVerif.Columns.Count; i++)
                {
                    if (Efectivo.Rows.Count > 0 && Mecanica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                    {
                        decimal valor = Convert.ToDecimal(dgvVerif.GetRowCellValue(0, dgvVerif.Columns[i]));
                        if (valor > RangoEfectivo || valor < -(RangoEfectivo))
                        {
                            Parametros.General.DialogMsg("La verificación en Efectivo excede el rango permitido: " + RangoEfectivo.ToString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }

                    if (Mecanica.Rows.Count > 0 && Electronica.Rows.Count > 0 && !EsDispensadorProblemaLecturas)
                    {
                        decimal valor = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i]));
                        if (valor > RangoMecElectronico || valor < -(RangoMecElectronico))
                        {
                            Parametros.General.DialogMsg("La verificación Mecánica vs Electrónica excede el rango permitido: " + RangoMecElectronico.ToString(), Parametros.MsgType.warning);
                            return false;
                        }
                    }
                }

                    return true;
            }
            catch { return false; }
        }
        #endregion


        /// <summary>
        /// Estilo de color de las celdas para verificación
        /// </summary>
        /// <param name="BCol">BandColumn aplicable para el style</param>
        /// <returns></returns>
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
                 style.Expression = "[" + BCol.FieldName + "] < " + Convert.ToString(Decimal.Negate(DiferenciaVerificacionLectura)) + " or [" + BCol.FieldName + "] > " + Convert.ToString(DiferenciaVerificacionLectura);

                 return style;
             }
            catch
             {return null;}
        }
         
        /// <summary>
        /// Cargar las lecturas de los Grid
        /// </summary>
        private bool FillLecturas()
        {
            try
            {
            
            //Carga los dispensadores de la isla
            var objDis = db.Dispensadors.Where(d => d.IslaID.Equals(IDIsla) && d.Activo);

            if (objDis.Count() > 0)
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);

                #region <<<DatosInicialesArqueo >>>

                //Parametor del dispensador (suma las lecturas mecanicas en ambas caras, es un dispensador con problemas)
                MecanicaAmbasCara = objDis.Count(d => d.MecanicaAmbasCara) > 0 ? true : false;
                EsDispensadorProblemaLecturas = objDis.Count(d => d.EsDispensadorProblemaLecturas) > 0 ? true : false;

                //Cargo los productos para la tabla ArqueoProducto
                var product = (from p in db.Productos
                               join t in db.Tanques on p.ID equals t.ProductoID
                               join m in db.Mangueras on t.ID equals m.TanqueID
                               join d in db.Dispensadors on m.DispensadorID equals d.ID
                               where d.Activo && m.Activo && (db.Dispensadors.Any(di => di.IslaID == IDIsla && di.ID == d.ID))
                               select new { IDProducto = p.ID, Producto = p.Nombre, m.PrecioDiferenciado, IDTanque = t.ID, Tanque = t.Nombre }).Distinct();

                if (product.Count(p => p.PrecioDiferenciado) > 0)
                {
                    layoutControlGroupExtracciones.Width = 580;
                    layoutControlItemVentas.Width = 580;
                }
                else
                {
                    layoutControlGroupExtracciones.Width = 500;
                    layoutControlItemVentas.Width = 500;
                }

                //Creación de las tablas del Arqueo
                dtDispensador = Parametros.General.LINQToDataTable(objDis);
                dtMecanica = new DataTable();
                dtEfectivo = new DataTable();
                dtElectronica = new DataTable();
                dtConcepto = new DataTable();

                //dtMecanica.Columns.Add("", typeof(Int32));
                dtMecanica.Columns.Add("Lectura", typeof(String));
                //dtEfectivo.Columns.Add("IDP", typeof(Int32));
                dtEfectivo.Columns.Add("Lectura", typeof(String));
                //dtElectronica.Columns.Add("IDP", typeof(Int32));
                dtElectronica.Columns.Add("Lectura", typeof(String));

                //Crear las columnas para la tabla dtConcepto
                dtConcepto.Columns.Add("IDAPEC", typeof(Int32));
                dtConcepto.Columns.Add("IDProducto", typeof(Int32));
                dtConcepto.Columns.Add("IDConcepto", typeof(Int32));
                dtConcepto.Columns.Add("Litros", typeof(Decimal));

                //Buscar filas en ArqueoProductoExtracionConcepto de encontrar llenamos la tabla dtConcepto con esas filas
                var ListaConceptos = from apec in db.ArqueoProductoExtracionConceptos
                                     join ape in db.ArqueoProductoExtracions on apec.ArqueoProductoExtracionID equals ape.ID
                                     join ap in db.ArqueoProductos on ape.ArqueoProductoID equals ap.ID
                                     where ap.ArqueoIslaID == ArqueoSelected.ID
                                     select new
                                     {
                                         apec.ID,
                                         ap.ProductoID,
                                         apec.ExtracionConceptoID,
                                         apec.Litros
                                     };

                if (ListaConceptos.Count() > 0)
                {
                    foreach (var list in ListaConceptos)
                    {
                        dtConcepto.Rows.Add(list.ID, list.ProductoID, list.ExtracionConceptoID, list.Litros);
                    }

                }

                //Lista de mangueras en la isla seleccionado
                var manguras = from m in db.Mangueras
                               join d in db.Dispensadors on m.DispensadorID equals d.ID
                               where m.Activo && (objDis.Any(o => o.ID == d.ID))
                               select m;

                Tanques = db.Tanques.Where(t => t.Activo && (manguras.Any(m => m.TanqueID == t.ID))).ToList();

                //Agregar bandas al grid verificacion de acuerdo a los lados del dispensador
                foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                {
                    var bandVerif = new Parametros.MyBand("bandVerif" + lado.Key.ToString(), "Verificación Lado " + lado.Key.ToString(), Convert.ToInt32(lado.Count() * 120), dgvVerif.Bands.Count + 1);

                    dgvVerif.Bands.Add(bandVerif);
                }

                //Numero de bandas en el grid de verificacion
                int bandas = dgvVerif.Bands.Count;

                dtVerificacion = new DataTable();
                dtVerificacion.Columns.Add("TipoVerificacion", typeof(String));

                #endregion

                ////////////////***************************************************************************************/////////////////////

                #region <<< DistribuciónExtracciónPago >>>

                //Agrego las colunmnas a la tabla dtVentas, dtExtracciones, dtTotalExtraxion            
                dtVentas = new DataTable();
                dtExtracciones = new DataTable();
                dtTotalExtraxion = new DataTable();
                dtPago = new DataTable();

                dtVentas.Columns.Add("DatosVentas", typeof(String));

                dtExtracciones.Columns.Add("IDExtraxion", typeof(Int32));
                dtExtracciones.Columns.Add("NombreExtracciones", typeof(String));
                dtExtracciones.Columns.Add("TieneConcepto", typeof(Boolean));
                dtExtracciones.Columns.Add("EquivalenteLitros", typeof(Decimal));

                //Llenar las columnas del DataTable dtPago
                dtPago.Columns.Add("IDPago", typeof(Int32));
                dtPago.Columns.Add("NombrePago", typeof(String));
                dtPago.Columns.Add("ValorPago", typeof(String));
                dtPago.Columns.Add("FormulaPago", typeof(String));

                dtTotalExtraxion.Columns.Add("Texto", typeof(String));

                //Se Agregan las bandas a los grid
                var bandProductos = new Parametros.MyBand("bandProductos", "PRECIO FULL", Convert.ToInt32(product.Count(p => !p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                this.bgvDataExtraciones.Bands.Add(bandProductos);

                var bandVentas = new Parametros.MyBand("bandVentas", "PRECIO FULL", Convert.ToInt32(product.Count(p => !p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                this.dgvVentas.Bands.Add(bandVentas);

                var bandTotalExtraxion = new Parametros.MyBand("bandTotalExtraxion", "PRECIO FULL", Convert.ToInt32(product.Count(p => !p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                this.dgvDataTotalExtraxion.Bands.Add(bandTotalExtraxion);

                if (product.Count(p => !p.PrecioDiferenciado) <= 0)
                {
                    this.bgvDataExtraciones.Bands[1].Visible = false;
                    this.dgvDataTotalExtraxion.Bands[1].Visible = false;
                    this.dgvVentas.Bands[1].Visible = false;
                }
                if (product.Count(p => p.PrecioDiferenciado) > 0)
                {
                    var bandProductosDif = new Parametros.MyBand("bandProductos", "PRECIO DIFERENCIADO", Convert.ToInt32(product.Count(p => p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                    this.bgvDataExtraciones.Bands.Add(bandProductosDif);

                    var bandVentasDif = new Parametros.MyBand("bandVentas", "PRECIO DIFERENCIADO", Convert.ToInt32(product.Count(p => p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                    this.dgvVentas.Bands.Add(bandVentasDif);

                    var bandTotalExtraxionDif = new Parametros.MyBand("bandTotalExtraxionDif", "PRECIO DIFERENCIADO", Convert.ToInt32(product.Count(p => p.PrecioDiferenciado) * 100), bgvDataExtraciones.Bands.Count + 1);
                    this.dgvDataTotalExtraxion.Bands.Add(bandTotalExtraxionDif);
                }

                //Por cada valor en productos llenar la tabla dtArqueoProducto y las columna a dtVentas
                bdsArqueoIsla.DataMember = "ArqueoProductos";
                foreach (var list in product)
                {
                    decimal precio = 0;

                    if (!Editable)
                    {
                        precio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(list.IDProducto, IDES, ResumenSelected.SubEstacionID, list.PrecioDiferenciado, TurnoSelected.Numero, ArqueoSelected.ArqueoEspecial, Convert.ToDateTime(ArqueoSelected.FechaCreado).Date)), 2, MidpointRounding.AwayFromZero);

                        EtArqueoProducto = new Entidad.ArqueoProducto();
                        EtArqueoProducto.ArqueoIslaID = ArqueoSelected.ID;
                        EtArqueoProducto.ProductoID = list.IDProducto;
                        EtArqueoProducto.EsDiferenciado = list.PrecioDiferenciado;
                        EtArqueoProducto.Precio = precio;
                        EtArqueoProducto.TanqueID = list.IDTanque;
                        bdsArqueoIsla.Add(EtArqueoProducto);
                    }

                    if (!list.PrecioDiferenciado)
                    {
                        dtVentas.Columns.Add(list.IDProducto.ToString() + "-"  + list.IDTanque.ToString() , typeof(Decimal));
                        dtTotalExtraxion.Columns.Add(list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), typeof(Decimal));
                        dtExtracciones.Columns.Add("Valor" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), typeof(String));
                        dtExtracciones.Columns.Add("Formula" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), typeof(String));
                        var colVentas = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), dgvVentas.Bands[1].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        var colExtra = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, "Valor" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), bgvDataExtraciones.Bands[1].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        var colTotalExtraxion = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, list.IDProducto.ToString() + "-" + list.IDTanque.ToString(), dgvDataTotalExtraxion.Bands[1].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        colExtra.Tag = list.IDProducto.ToString() + "-" + list.IDTanque.ToString();
                        colVentas.Tag = list.PrecioDiferenciado.ToString() + "-" + list.IDTanque.ToString();
                        colTotalExtraxion.Tag = list.IDProducto.ToString() + "-" + list.IDTanque.ToString();
                        //DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpFormula = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();

                        //rpFormula.AcceptsTab = false;
                        //rpFormula.AutoHeight = false;
                        //rpFormula.ShowIcon = false;
                        //rpFormula.ValidateOnEnterKey = false;
                        //rpFormula.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpFormula_Closed);
                        //rpFormula.Popup += new System.EventHandler(this.rpFormula_Popup);

                        colExtra.SummaryItem.DisplayFormat = "{0:#,0.000;(#,0.000)}";
                        colExtra.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        colExtra.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                        colExtra.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colExtra.DisplayFormat.FormatString = "N3";

                        colTotalExtraxion.DisplayFormat.FormatString = "N3";
                        colTotalExtraxion.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colTotalExtraxion.AppearanceCell.BackColor = Color.AliceBlue;

                        this.dgvVentas.Bands[1].Columns.Add(colVentas);
                        this.dgvDataTotalExtraxion.Bands[1].Columns.Add(colTotalExtraxion);
                        this.bgvDataExtraciones.Bands[1].Columns.Add(colExtra);
                    }
                    else if (list.PrecioDiferenciado)
                    {
                        dtVentas.Columns.Add(list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", typeof(Decimal));
                        dtTotalExtraxion.Columns.Add(list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", typeof(String));
                        dtExtracciones.Columns.Add("Valor" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", typeof(String));
                        dtExtracciones.Columns.Add("Formula" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", typeof(String));
                        var colVentas = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", dgvVentas.Bands[2].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        var colTotalExtraxion = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", dgvDataTotalExtraxion.Bands[2].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        var colExtra = new Parametros.MyBandColumn(list.Producto + " => " + list.Tanque, "Valor" + list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif", bgvDataExtraciones.Bands[2].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == list.IDProducto && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        colExtra.Tag = list.IDProducto.ToString() + "-" + list.IDTanque.ToString();
                        colVentas.Tag = list.PrecioDiferenciado.ToString() + "-" + list.IDTanque.ToString();
                        colTotalExtraxion.Tag = list.IDProducto.ToString() + "-" + list.IDTanque.ToString();
                        //DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpFormula = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();

                        //rpFormula.AcceptsTab = false;
                        //rpFormula.AutoHeight = false;
                        //rpFormula.ShowIcon = false;
                        //rpFormula.ValidateOnEnterKey = true;
                        //rpFormula.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpFormula_Closed);
                        //rpFormula.Popup += new System.EventHandler(this.rpFormula_Popup);

                        colExtra.SummaryItem.DisplayFormat = "{0:#,0.000;(#,0.000)}";
                        colExtra.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        colExtra.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;

                        colTotalExtraxion.DisplayFormat.FormatString = "N3";
                        colTotalExtraxion.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colTotalExtraxion.AppearanceCell.BackColor = Color.AliceBlue;

                        this.dgvVentas.Bands[2].Columns.Add(colVentas);
                        this.dgvDataTotalExtraxion.Bands[2].Columns.Add(colTotalExtraxion);
                        this.bgvDataExtraciones.Bands[2].Columns.Add(colExtra);
                    }
                }

                //Recorremos cada manguera de la isla
                foreach (var rmang in manguras)
                {
                    if (!Editable)
                    {
                        //Agregamos cada manguera como fila a la entidad EtArqueoManguera
                        EtArqueoManguera = new Entidad.ArqueoManguera();

                        EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(db.Tanques.Single(t => t.ID == rmang.TanqueID).ProductoID) && ap.TanqueID.Equals(rmang.TanqueID) && ap.EsDiferenciado == rmang.PrecioDiferenciado);

                        EtArqueoManguera.ArqueoProductoID = EtArqueoProducto.ID;

                        EtArqueoManguera.MangueraID = rmang.ID;
                        EtArqueoManguera.Lado = rmang.Lado;
                        EtArqueoManguera.DispensadorID = rmang.DispensadorID;
                        EtArqueoManguera.TanqueID = rmang.TanqueID;
                        EtArqueoManguera.EsLecturaGalones = rmang.EsLecturaGalones;
                        EtArqueoManguera.FlujoRapido = rmang.FlujoRapido;

                        if (objDis.Count(o => o.LecturaMecanica) > 0)
                        {
                            EtArqueoManguera.LecturaMecanicaInicial = rmang.LecturaMecanica;
                            EtArqueoManguera.LecturaMecanicaFinal = 0.000m;
                        }

                        if (objDis.Count(o => o.LecturaEfectivo) > 0)
                        {
                            EtArqueoManguera.LecturaEfectivoInicial = rmang.LecturaEfectivo;
                            EtArqueoManguera.LecturaEfectivoFinal = 0.000m;
                        }

                        if (objDis.Count(o => o.LecturaElectronica) > 0)
                        {
                            EtArqueoManguera.LecturaElectronicaInicial = rmang.LecturaElectronica;
                            EtArqueoManguera.LecturaElectronicaFinal = 0.000m;
                        }

                        EtArqueoManguera.Descuento = rmang.Descuento;

                        bdsArqueoProducto.DataSource = EtArqueoProducto;
                        bdsArqueoProducto.DataMember = "ArqueoMangueras";
                        bdsArqueoProducto.Add(EtArqueoManguera);
                    }

                    //dtArqueoManguera.Rows.Add(rmang.ID, rmang.LecturaMecanica, 0, rmang.LecturaEfectivo, 0, rmang.LecturaElectronica, 0);

                    //Se agrega una columna por manguera a la tabla dtVerificacion
                    dtVerificacion.Columns.Add(rmang.ID.ToString(), typeof(Decimal));

                    //Producto de la manguera
                    var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(rmang.TanqueID))).ProductoID));


                    //Validar donde agregar la manguera si el lado A o lado B
                    if (rmang.Lado.ToString() == "A")
                    {
                        //var col = new Parametros.MyBandColumn(rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, false);
                        var col = new Parametros.MyBandColumn(prod.Nombre + " (" + rmang.Numero + ")", rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == rmang.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                        dgvVerif.FormatConditions.Add(styleFormatConditioner(col));
                        this.dgvVerif.Bands[1].Columns.Add(col);

                    }
                    else if (rmang.Lado.ToString() == "B")
                    {
                        if (bandas > 1)
                        {
                            //var col = new Parametros.MyBandColumn(rmang.ID.ToString(), dgvVerif.Bands[2].Columns.Count + 1, 120, false);
                            var col = new Parametros.MyBandColumn(prod.Nombre + " (" + rmang.Numero + ")", rmang.ID.ToString(), dgvVerif.Bands[1].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == rmang.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                            dgvVerif.FormatConditions.Add(styleFormatConditioner(col));

                            if (this.dgvVerif.Bands.Count > 2)
                                this.dgvVerif.Bands[2].Columns.Add(col);
                            else
                                this.dgvVerif.Bands[1].Columns.Add(col);
                        }
                    }

                }

                //Se carga el enum de los datos de la Verificacion
                foreach (Parametros.TiposVerificacion tipo in Enum.GetValues(typeof(Parametros.TiposVerificacion)))
                {
                    //Creamos una nueva fila en el dataTable dtVerificacion
                    DataRow dr = dtVerificacion.NewRow();
                    dr["TipoVerificacion"] = Parametros.General.GetEnumTiposVerificacion(tipo);

                    //foreach (var list in manguras)
                    //{
                    //    dr[list.ID.ToString()] = 0;
                    //}

                    foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                    {
                        foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                        { dr[am.MangueraID.ToString()] = 0; }
                    }

                    dtVerificacion.Rows.Add(dr);

                }

                gridVerif.DataSource = dtVerificacion;
                dgvVerif.RefreshData();

                //Se carga el enum de los datos de la venta                 
                foreach (Parametros.DatosVentas val in Enum.GetValues(typeof(Parametros.DatosVentas)))
                {
                    //Creamos una nueva fila en el dataTable dtVentas
                    DataRow dr = dtVentas.NewRow();
                    dr["DatosVentas"] = Parametros.General.GetEnumDatosVentas(val);


                    foreach (var list in product)
                    {
                        //Si el producto tiene precio diferenciado al ID del Pord se le agrega "Dif"
                        string ColNombre = list.PrecioDiferenciado ? list.IDProducto.ToString() + "-" + list.IDTanque.ToString() + "Dif" : list.IDProducto.ToString() + "-" + list.IDTanque.ToString();

                        if (Parametros.DatosVentas.PreciosVentas == val)
                        {
                            foreach (Entidad.ArqueoProducto rowAP in ArqueoSelected.ArqueoProductos)
                            {
                                if (Convert.ToInt32(rowAP.ProductoID) == list.IDProducto && Convert.ToBoolean(rowAP.EsDiferenciado) == list.PrecioDiferenciado)
                                    dr[ColNombre] = Decimal.Round(Convert.ToDecimal(rowAP.Precio), 2, MidpointRounding.AwayFromZero);
                            }

                        }
                        else if (Parametros.DatosVentas.Descuento == val)
                        {
                            foreach (Entidad.ArqueoProducto rowAP in ArqueoSelected.ArqueoProductos)
                            {
                                if (Convert.ToInt32(rowAP.ProductoID) == list.IDProducto && Convert.ToBoolean(rowAP.EsDiferenciado).Equals(list.PrecioDiferenciado))
                                {
                                    //EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.EsDiferenciado == mang.PrecioDiferenciado);
                                    //EtArqueoManguera = rowAP.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                                    //EtArqueoManguera.LecturaMecanicaFinal = extracion;

                                    dr[ColNombre] = rowAP.ArqueoMangueras.Where(am => am.ArqueoProductoID == rowAP.ID && am.Descuento != 0).Count() > 0 ? rowAP.ArqueoMangueras.Where(am => am.ArqueoProductoID == rowAP.ID && am.Descuento != 0).First().Descuento : 0.00m;

                                    if (rowAP.ArqueoMangueras.Where(am => am.ArqueoProductoID == rowAP.ID && am.Descuento != 0).Count() > 1)
                                        layoutLblDescuento.Text += "Descuento en el Producto: " + db.Productos.Single(p => p.ID == rowAP.ProductoID).Nombre + " en ambos lados.     ";
                                    else if (rowAP.ArqueoMangueras.Where(am => am.ArqueoProductoID == rowAP.ID && am.Descuento != 0).Count() == 1)
                                        layoutLblDescuento.Text += "Descuento en el Producto: " + db.Productos.Single(p => p.ID == rowAP.ProductoID).Nombre + " solo en el lado: " + db.Mangueras.Single(m => m.ID == Convert.ToInt32(rowAP.ArqueoMangueras.Where(am => am.ArqueoProductoID == rowAP.ID && am.Descuento != 0).First().MangueraID)).Lado + ".     ";

                                }

                            }
                        }
                        else
                            dr[ColNombre] = 0.00m;
                    }

                    dtVentas.Rows.Add(dr);
                }

                this.gridVentas.DataSource = dtVentas;

                var objExtra = (from ext in db.ExtracionPagos
                                where ext.Activo && !ext.EsPago
                                select new { IDExtraxion = ext.ID, NombreExtracciones = ext.Nombre, ext.TieneConcepto, ext.Orden, ext.EquivalenteLitros }).OrderBy(o => o.Orden);

                foreach (var extra in objExtra)
                {
                    //Creamos una nueva fila en el dataTable dtExtracciones
                    DataRow dr = dtExtracciones.NewRow();
                    dr["IDExtraxion"] = extra.IDExtraxion;
                    dr["NombreExtracciones"] = extra.NombreExtracciones;
                    dr["TieneConcepto"] = extra.TieneConcepto;
                    dr["EquivalenteLitros"] = extra.EquivalenteLitros;

                    if (Editable)
                    {
                        foreach (Entidad.ArqueoProducto AP in ArqueoSelected.ArqueoProductos)
                        {
                            string valor = "Valor" + AP.ProductoID + "-" + AP.TanqueID + (AP.EsDiferenciado ? "Dif" : "");
                            string formula = "Formula" + AP.ProductoID + "-" + AP.TanqueID + (AP.EsDiferenciado ? "Dif" : "");

                            foreach (Entidad.ArqueoProductoExtracion APE in AP.ArqueoProductoExtracions)
                            {

                                if (APE.ExtracionID == Convert.ToInt32(dr["IDExtraxion"]))
                                {
                                    dr[valor] = APE.Valor;
                                    dr[formula] = APE.Formula;
                                }

                            }
                        }

                    }
                    dtExtracciones.Rows.Add(dr);
                }

                this.gridExtraciones.DataSource = dtExtracciones;

                foreach (var pago in db.ExtracionPagos.Where(p => p.Activo && p.EsPago && !p.EsEfectivo).OrderBy(o => o.Orden))
                {
                    var AFP = db.ArqueoFormaPagos.Where(a => a.ArqueoIslaID == ArqueoSelected.ID && a.PagoID == pago.ID);
                    if (AFP.Count() > 0)
                        dtPago.Rows.Add(pago.ID, pago.Nombre, AFP.First().Valor, AFP.First().Formula);
                    else
                        dtPago.Rows.Add(pago.ID, pago.Nombre);
                }

                gridPago.DataSource = dtPago;
                bgvDataPago.RefreshData();

                //Creamos una nueva fila en el dataTable dtTotalExtraxion
                DataRow row = dtTotalExtraxion.Rows.Add("Extracción Total");

                gridTotalExtraxion.DataSource = dtTotalExtraxion;
                bgvDataExtraciones.RefreshData();

                //VerificacionLecturaEfectivo();
                //VerificacionMecVsElectronica();

                #endregion

                ///**************************************************************************************************************************///  

                #region <<< LecturaMecanica >>>

                //verificar si en la lista objDis esta activada la lectura mecanica
                if (objDis.Count(o => o.LecturaMecanica) > 0)
                {
                    //Mostrar el grid Mecanica
                    this.layoutControlGroupMecanica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    
                    //Variable para contar los puntos de llenado
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {

                        //crear banda para los lados del grid de lectura mecanica
                        var bandLado = new Parametros.MyBand("gBandMec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataMecanica.Bands.Count + 1);

                        //Diseño de las bandas del gridMecanica
                        this.bgvDataMecanica.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);

                            //Se rocorren las mangueras de cada Dispensador
                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura mecanica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtMecanica.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));

                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataMecanica.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                //rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();

                                //agrego la columna al BandeGridView bgvDataMecanica
                                //this.bgvDataMecanica.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridMecanica.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);
                                //bandLado.Columns.Add(colProd);                           

                            }
                        }
                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtMecanica.NewRow();
                        dr["Lectura"] = val.ToString();



                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura

                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID);
                                //Convert.ToString(db.Tanques.Single(t => t.ID == obj.TanqueID).ProductoID) + obj.Lado;

                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaMecanicaInicial;
                                    //obj.LecturaMecanica;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaMecanicaFinal;
                                    //0.00m;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtMecanica.Rows[1][nombre]) - Convert.ToDecimal(dtMecanica.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtMecanica.Rows.Add(dr);

                    }

                    gridMecanica.DataSource = Mecanica;
                }

                #endregion

                ///**************************************************************************************************************************///

                #region <<< LecturaEfectivo >>>

                //verificar si en la lista objDis esta activada la lectura Efectivo
                if (objDis.Count(o => o.LecturaEfectivo) > 0)
                {
                    //Mostrar el grid Efectivo
                    this.layoutControlGroupEfectivo.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    

                    //Variable para contar los puntos de llenado
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {

                        //crear banda para los lados del grid de lectura efectivo
                        var bandLado = new Parametros.MyBand("gBandEfec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataEfectivo.Bands.Count + 1);

                        //Diseño de las bandas del gridEfectivo
                        this.bgvDataEfectivo.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);


                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura mecanica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtEfectivo.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));


                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataEfectivo.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                //rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();
                                //agrego la columna al BandeGridView bgvDataMecanica
                                //this.bgvDataEfectivo.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridEfectivo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);

                            }
                        }

                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtEfectivo.NewRow();
                        dr["Lectura"] = val.ToString();


                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura
                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID);

                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaEfectivoInicial;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaEfectivoFinal;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtEfectivo.Rows[1][nombre]) - Convert.ToDecimal(dtEfectivo.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtEfectivo.Rows.Add(dr);
                    }

                    gridEfectivo.DataSource = Efectivo;
                }

                #endregion

                ///*************************************************************************************************************************///

                #region <<< LecturaElectronica >>>

                //Cargar Gird de Lecuras en ELECTRONICA
                if (objDis.Count(o => o.LecturaElectronica) > 0)
                {
                    //Mostrar el grid Efectivo
                    this.layoutControlGroupElectronica.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                    
                    //lado.key devuelve el valor del lado de la manguera
                    int l = 1;
                    //Agrupar por los Lados asignados a la manguera
                    //Se diseña por orden de lados primero LADO A y despues LADO B
                    foreach (var lado in manguras.GroupBy(m => m.Lado).Distinct())
                    {
                        //crear banda para los lados del grid de lectura Electronica
                        var bandLado = new Parametros.MyBand("gBandElec" + lado.Key.ToString(), "LADO " + lado.Key.ToString() + " Punto de Llenado " + l.ToString(), Convert.ToInt32(manguras.Count() * 120), bgvDataElectronico.Bands.Count + 1);

                        //Diseño de las bandas del gridElectronica
                        this.bgvDataElectronico.Bands.Add(bandLado);
                        ////fin del diseño de la banda

                        #region <<< CreacionColumnasRepositorio >>>

                        //Se recorren los dispensadores de la Isla
                        foreach (var dis in objDis)
                        {
                            //Numero de manueras del dispensador a recorrer
                            int ancho = db.Dispensadors.Count(d => d.ID == dis.ID && (db.Mangueras.Any(m => m.Activo && m.Lado == lado.Key && m.DispensadorID == d.ID)));

                            //crear banda para los lados del grid de lectura mecanica
                            var bandDis = new Parametros.MyBand("gBandDos" + dis.Nombre, "Dispensador " + dis.Nombre, Convert.ToInt32(ancho * 120), bandLado.Children.Count + 1);

                            bandLado.Children.Add(bandDis);

                            foreach (var item in manguras.Where(m => m.Lado == lado.Key && m.DispensadorID == dis.ID))
                            {
                                //se crea el objeto producto de acuerdo al tanque que corresponde
                                var prod = db.Productos.Single(p => p.ID == Convert.ToInt32((db.Tanques.Single(t => t.ID == Convert.ToInt32(item.TanqueID))).ProductoID));

                                //se añade la columna del producto al data table de lectura Electronica
                                //el nombre de la columna es el ID del producto mas el lado del dispensador (1A)
                                dtElectronica.Columns.Add(prod.ID.ToString() + lado.Key + item.ID.ToString(), typeof(Decimal));

                                var colProd = new Parametros.MyBandColumn(prod.Nombre + " (" + item.Numero + ")", prod.ID.ToString() + lado.Key + item.ID.ToString(), bgvDataElectronico.Bands[l].Columns.Count + 1, 120, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ID == item.TanqueID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                                colProd.Tag = item;

                                //**Repositorio para digitar la lectura
                                DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLectura = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                                rpLectura.AutoHeight = false;
                                rpLectura.AllowMouseWheel = false;
                                //rpLectura.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                                rpLectura.MaxValue = new decimal(1000000000000);
                                rpLectura.Name = "rpItem" + item.ID.ToString();

                                rpLectura.EditFormat.FormatString = "N3";
                                rpLectura.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.DisplayFormat.FormatString = "N3";
                                rpLectura.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                                rpLectura.EditMask = "N3";
                                rpLectura.Buttons.Clear();
                                //agrego la columna al BandeGridView bgvDataMecanica
                                this.bgvDataElectronico.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] { colProd });
                                //agrego el repositorio a GridControl gridMecanica
                                this.gridElectronica.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpLectura });

                                colProd.ColumnEdit = rpLectura;
                                bandDis.Columns.Add(colProd);

                            }
                        }

                        l++;
                        #endregion

                    }

                    foreach (Parametros.Lectura val in Enum.GetValues(typeof(Parametros.Lectura)))
                    {
                        //Creamos una nueva fila en el dataTable Mecanica
                        DataRow dr = dtElectronica.NewRow();
                        dr["Lectura"] = val.ToString();


                        //recorremos las mangueras de la lista (mang) para agregarle los valores de lectura
                        foreach (Entidad.ArqueoProducto ap in ArqueoSelected.ArqueoProductos)
                        {
                            foreach (Entidad.ArqueoManguera am in ap.ArqueoMangueras)
                            {
                                //buscamos el nombre por ID del producto y lado (1A)
                                string nombre = Convert.ToString(ap.ProductoID + db.Mangueras.Single(m => m.ID == am.MangueraID).Lado + am.MangueraID); ;

                                //Asignamos los saldos iniciales
                                if (val == Parametros.Lectura.Inicial)
                                {
                                    dr[nombre] = am.LecturaElectronicaInicial;
                                }
                                else if (val == Parametros.Lectura.Final)
                                {
                                    dr[nombre] = am.LecturaElectronicaFinal;
                                }
                                else if (val == Parametros.Lectura.Extraccion)
                                {
                                    //resto la lectura final (2da fila) - la inicial (1ra fila)
                                    dr[nombre] = Decimal.Round(Convert.ToDecimal(dtElectronica.Rows[1][nombre]) - Convert.ToDecimal(dtElectronica.Rows[0][nombre]), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                        dtElectronica.Rows.Add(dr);

                    }

                    gridElectronica.DataSource = Electronica;

                }

                #endregion

                ///************************************************************************************************************************///


                #region <<< Descuento >>>

                dtDescuento = new DataTable();
                dtDescuento.Columns.Add("IDDescuento", typeof(Int32));
                dtDescuento.Columns.Add("TipoDescuento", typeof(String));

                var bandDesc = new Parametros.MyBand("bandDesc", "PRODUCTOS", 120, bgvDataDescuento.Bands.Count + 1);
                bgvDataDescuento.Bands.Add(bandDesc);

                foreach (var ap in ArqueoSelected.ArqueoProductos.GroupBy(a => new {a.ProductoID, a.TanqueID}).Distinct())
                {
                    Entidad.ArqueoProducto Eap = ArqueoSelected.ArqueoProductos.Where(o => o.ProductoID.Equals(ap.Key.ProductoID) && o.TanqueID.Equals(ap.Key.TanqueID)).First();
                    dtDescuento.Columns.Add(Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString(), typeof(Decimal));
                    var colDesc = new Parametros.MyBandColumn(db.Productos.Single(p => p.ID == Eap.ProductoID).Nombre + " => " + db.Tanques.Single(t => t.ID.Equals(Eap.TanqueID)).Nombre, Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString(), bgvDataDescuento.Bands[1].Columns.Count + 1, 100, true, Color.FromArgb(Convert.ToInt32(db.Tanques.Where(t => t.ProductoID == Eap.ProductoID && t.EstacionServicioID == Convert.ToInt32(db.Islas.Single(i => i.ID == ArqueoSelected.IslaID).EstacionServicioID)).First().Color)));
                    colDesc.Tag = Eap;

                    colDesc.SummaryItem.DisplayFormat = "{0:#,0.00;(#,0.00)}";
                    colDesc.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                    colDesc.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                    colDesc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    colDesc.DisplayFormat.FormatString = "N2";

                    DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpDesc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                    rpDesc.AutoHeight = false;
                    rpDesc.MaxValue = new decimal(1000000000000);
                    //rpDesc.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });

                    rpDesc.EditFormat.FormatString = "N2";
                    rpDesc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    rpDesc.DisplayFormat.FormatString = "N2";
                    rpDesc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    rpDesc.EditMask = "N2";
                    rpDesc.Buttons.Clear();
                    this.gridDescuento.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpDesc });

                    colDesc.ColumnEdit = rpDesc;

                    this.bgvDataDescuento.Bands[1].Columns.Add(colDesc);

                }

                foreach (Parametros.TiposDescuento val in Enum.GetValues(typeof(Parametros.TiposDescuento)))
                {
                    DataRow dr = dtDescuento.NewRow();
                    dr["IDDescuento"] = val;
                    dr["TipoDescuento"] = Parametros.General.GetEnumTiposDescuento(val);
                    foreach (var ap in ArqueoSelected.ArqueoProductos.GroupBy(a => new { a.ProductoID, a.TanqueID }).Distinct())
                    {
                        Entidad.ArqueoProducto Eap = ArqueoSelected.ArqueoProductos.FirstOrDefault(o => o.ProductoID.Equals(ap.Key.ProductoID) && o.TanqueID.Equals(ap.Key.TanqueID));

                        if (Eap != null)
                        {
                            if (val.Equals(Parametros.TiposDescuento.Isla))
                                dr[Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString()] = 0.00m;
                            else if (val.Equals(Parametros.TiposDescuento.Especial))
                                dr[Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString()] = Eap.DescuentoEspecial;
                            else if (val.Equals(Parametros.TiposDescuento.GalonesPetroCard))
                                dr[Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString()] = Eap.DescuentoGalonesPetroCardFormula != null ? ValorFormula(Eap.DescuentoGalonesPetroCardFormula) : 0;
                            else if (val.Equals(Parametros.TiposDescuento.CordobasPetroCard))
                                dr[Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString()] = Decimal.Round(ValorFormula(Eap.DescuentoGalonesPetroCardFormula) * DescuentoPetroCard, 2, MidpointRounding.AwayFromZero);
                            else
                                dr[Eap.ProductoID.ToString() + "-" + Eap.TanqueID.ToString()] = 0.00m;
                        }
                    }


                    dtDescuento.Rows.Add(dr);
                }

                gridDescuento.DataSource = dtDescuento;
                bgvDataDescuento.RefreshData();

                #endregion

                #region <<< PagoEfectivo >>>

                //Llenar las columnas del DataTable dtPagoEfectivo
                dtPagoEfectivo = new DataTable();
                dtPagoEfectivo.Columns.Add("IDPago", typeof(Int32));
                dtPagoEfectivo.Columns.Add("Titulo", typeof(String));

                foreach (var objMoneda in db.Monedas)
                {
                    //dtPagoEfectivo.Columns.Add(objMoneda.ID.ToString(), typeof(Int32));
                    dtPagoEfectivo.Columns.Add("Valor" + objMoneda.ID.ToString(), typeof(String));
                    dtPagoEfectivo.Columns.Add("Formula" + objMoneda.ID.ToString(), typeof(String));
                    //dtPagoEfectivo.Columns.Add(objMoneda.ID.ToString(), typeof(Int32));

                    if (objMoneda.ID == MonedaPrincipal)
                    {
                        colValorC.FieldName = "Valor" + objMoneda.ID.ToString();
                        colValorC.Tag = "Formula" + objMoneda.ID.ToString();
                    }
                    else
                    {
                        colValorUS.FieldName = "Valor" + objMoneda.ID.ToString();
                        colValorUS.Tag = "Formula" + objMoneda.ID.ToString();
                    }
                }

                var listEfectivo = ArqueoSelected.ArqueoFormaPagos.Where(af => af.MonedaID > 0);

                if (listEfectivo.Count() > 0)
                {
                    DataRow PER = dtPagoEfectivo.NewRow();
                    PER["IDPago"] = IDFormaPagoEfectivo;
                    PER["Titulo"] = "Efectivo Recibido";

                    foreach (var item in listEfectivo)
                    {

                        PER["Valor" + item.MonedaID.ToString()] = item.Valor;
                        PER["Formula" + item.MonedaID.ToString()] = item.Formula;
                    }

                    dtPagoEfectivo.Rows.Add(PER);
                }
                else
                    dtPagoEfectivo.Rows.Add(IDFormaPagoEfectivo, "Efectivo Recibido");

                gridPagoEfectivo.DataSource = dtPagoEfectivo;
                bgvDataPagoEfectivo.RefreshData();

                decimal TotalEfectivo = 0m;
                foreach (DataColumn r in dtPagoEfectivo.Columns)
                {
                    if (r.ColumnName.Contains("Valor"))
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0][r])))
                        {
                            TotalEfectivo += Convert.ToDecimal(dtPagoEfectivo.Rows[0][r]);
                        }
                    }
                }
                txtSubTotalEfectivoRecibido.EditValue = Decimal.Round(Convert.ToDecimal(TotalEfectivo), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");

                #endregion

                VerificacionLecturaEfectivo();
                VerificacionMecVsElectronica();
                txtEfectivoRecibido_EditValueChanged(null, null);

                //if (!ArqueoSelected.DiferenciaRecibida.Equals(0))
                  
                spDiferenciaRecibida.Value = ArqueoSelected.DiferenciaRecibida;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                return true;
            }
            else
            {  
                Parametros.General.DialogMsg("La isla no tiene asignado ningún dispensador.", Parametros.MsgType.warning);
                return false;
            }
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                return false;
            }

        }

        #region <<< VerificacionLecturas/TotalExtraxion/Formula >>>

        private void VerificacionLecturaEfectivo()
        {
            try
            {
                if (Efectivo.Rows.Count > 0 && Mecanica.Rows.Count > 0)
                {
                    for (int i = 2; i < dgvVerif.Columns.Count; i++)
                    {
                        decimal PrimerValor = Decimal.Round(Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, bgvDataEfectivo.Columns[i])), 3, MidpointRounding.AwayFromZero);
                        decimal SegundoValor = 0;
                        
                        if (Electronica.Rows.Count > 0)
                            SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])), 3, MidpointRounding.AwayFromZero);
                        else
                            SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        decimal total = 0m;

                        var Manguera = Electronica.Rows.Count > 0 ? ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag)) : ((Entidad.Manguera)((Entidad.Manguera)bgvDataMecanica.Columns[i].Tag));

                        string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString();
                        //string Producto = bgvDataElectronico.Columns[i].FieldName.Substring(0, 1);
                        Producto += "-" + Manguera.TanqueID.ToString();


                        //EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(Producto) && );
                        if (Manguera.PrecioDiferenciado)
                            Producto += "Dif";
                                               

                        //var Prod = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag));
                        decimal Precio = Convert.ToDecimal(dgvVentas.GetRowCellValue(1, dgvVentas.Columns[Producto]));

                        decimal Descuento = 0m;
                            
                            var Desc = ArqueoSelected.ArqueoProductos.Where(ap => ap.TanqueID.Equals(Manguera.TanqueID) 
                                && ap.ProductoID.Equals(Convert.ToInt32(Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID)) 
                                && ap.EsDiferenciado.Equals(Manguera.PrecioDiferenciado)
                                ).First().ArqueoMangueras.Where(am => am.MangueraID.Equals(Manguera.ID)).First();

                            if (Desc != null)
                                Descuento = Desc.Descuento;

                        // = Convert.ToDecimal(dgvVentas.GetRowCellValue(2, dgvVentas.Columns[Producto]));

                        if (!Descuento.Equals(0m))
                            total = Decimal.Round(PrimerValor - (SegundoValor * (Precio - Descuento)), 3, MidpointRounding.AwayFromZero);
                        else
                            total = Decimal.Round(PrimerValor - (SegundoValor * (Precio)), 3, MidpointRounding.AwayFromZero);
                        
                        dgvVerif.SetRowCellValue(0, dgvVerif.Columns[Manguera.ID.ToString()], total);
                    }

                    dgvVerif.RefreshData();

                    if (btnOK.Enabled.Equals(false))
                        btnOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void VerificacionMecVsElectronica()
        {
            try
            {
                if (Mecanica.Rows.Count > 0 && Electronica.Rows.Count > 0)
                {
                    for (int i = 2; i < dgvVerif.Columns.Count; i++)
                    {
                        decimal PrimerValor = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        decimal SegundoValor = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])), 3, MidpointRounding.AwayFromZero);

                        var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[i].Tag));

                        decimal total = Decimal.Round(PrimerValor - SegundoValor, 3, MidpointRounding.AwayFromZero);

                        dgvVerif.SetRowCellValue(1, dgvVerif.Columns[Manguera.ID.ToString()], total);
                    }
                    dgvVerif.RefreshData();

                    if (MecanicaAmbasCara)
                    {
                        int col = Convert.ToInt32((dgvVerif.Columns.Count - 2) / 2m);

                        for (int i = 2; i < dgvVerif.Columns.Count; i++)
                        {
                            if (i < (col + 2))
                            {
                                decimal x = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i]));
                                decimal y = Convert.ToDecimal(dgvVerif.GetRowCellValue(1, dgvVerif.Columns[i + col]));
                                
                                dgvVerif.SetRowCellValue(1, dgvVerif.Columns[i], x + y);
                            }
                            else
                                dgvVerif.SetRowCellValue(1, dgvVerif.Columns[i], 0);
                        }

                        dgvVerif.RefreshData();
                    }

                    List<string> ID = new List<string>();
                    
                    for (int i = 2; i < dgvDataTotalExtraxion.Columns.Count; i++)
                    {
                        string prod = dgvDataTotalExtraxion.Columns[i].FieldName;
                        decimal total = 0;

                        for (int j = 2; j < bgvDataElectronico.Columns.Count; j++)
                        {
                            var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[j].Tag));
                            string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString() + "-" + Manguera.TanqueID.ToString();

                            if (Manguera.PrecioDiferenciado)
                                Producto += "Dif";

                            if (prod.Equals(Producto))
                            {
                                decimal venta = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[j])), 3, MidpointRounding.AwayFromZero);
                                if (Manguera.EsLecturaGalones)
                                    total += (venta * Parametros.General.LitrosGalonesTresDecimales);
                                else
                                    total += venta;
                            }
                        }

                        //Obtengo el ID del producto y del tanque de la tabla Total Extraxion
                        string item = ((string)(dgvDataTotalExtraxion.Columns[i].Tag));
                        string result = item.Replace("Dif", "");
                        //string input = Regex.Replace(item, "[^0-9]+", string.Empty);
                        string[] llaves = result.Split('-');

                        int IDP = Convert.ToInt32(llaves.ElementAt(0));
                        int IDT = Convert.ToInt32(llaves.ElementAt(1));

                        
                        //Verifico si el ID esta en la lista de ID's, si esta se resetea a 0 el valor del descuento automatico
                        if (!ID.Contains(result))
                        {
                            bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[result], 0m);

                            var AP = ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(IDP) && ap.TanqueID.Equals(IDT));
                                                    
                            foreach (var objAP in AP)
                            {
                                foreach (var objAM in objAP.ArqueoMangueras.Where(a => a.ArqueoProductoID.Equals(objAP.ID)))
                                {
                                    decimal Desc = 0;
                                    Desc += Convert.ToDecimal(bgvDataDescuento.GetRowCellValue(0, bgvDataDescuento.Columns[result]));
                                    Desc += Decimal.Round((objAM.LecturaElectronicaFinal - objAM.LecturaElectronicaInicial) * Convert.ToDecimal(objAM.Descuento), 2, MidpointRounding.AwayFromZero);
                                    bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[result], Desc);
                                    objAP.DescuentoDispensador = Desc;
                                    bgvDataDescuento.RefreshData();

                                }
                            }
                            ID.Add(result);
                        }

                        dgvDataTotalExtraxion.SetRowCellValue(0, dgvDataTotalExtraxion.Columns[i], total.ToString("#,0.000"));

                        
                    }
                    decimal suma = 0.00m;

                    for (int i = 2; i < bgvDataDescuento.Columns.Count; i++)
                    {
                        decimal calc = 0m;
                        suma += Decimal.TryParse(bgvDataDescuento.Columns[i].SummaryText, out calc) ? calc : 0;
                        txtDescuentoTotal.Text = Decimal.Round(suma, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    }

                    dgvDataTotalExtraxion.RefreshData();
                    TotalExtraxionVenta();
                    if (btnOK.Enabled.Equals(false))
                        btnOK.Enabled = true;
                }
                else
                {
                    List<string> ID = new List<string>();
                    for (int i = 2; i < dgvDataTotalExtraxion.Columns.Count; i++)
                    {
                        string prod = dgvDataTotalExtraxion.Columns[i].FieldName;
                        decimal total = 0;

                        if (Mecanica.Rows.Count > 0)
                        {
                            for (int j = 1; j <= bgvDataMecanica.Columns.Count; j++)
                            {
                                if (bgvDataMecanica.Columns.Count >= (j + 1))
                                {
                                    var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataMecanica.Columns[j].Tag));
                                    if (Manguera != null)
                                    {
                                        string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString() + "-" + Manguera.TanqueID.ToString();

                                        if (Manguera.PrecioDiferenciado)
                                            Producto += "Dif";

                                        if (prod.Equals(Producto))
                                        {
                                            decimal venta = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[j])), 3, MidpointRounding.AwayFromZero);
                                            if (Manguera.EsLecturaGalones)
                                                total += (venta * Parametros.General.LitrosGalonesTresDecimales);
                                            else
                                                total += venta;

                                        }
                                    }
                                }
                            }
                        }
                        else if (Electronica.Rows.Count > 0)
                        {
                            for (int j = 1; j < bgvDataElectronico.Columns.Count; j++)
                            {
                                if (bgvDataElectronico.Columns.Count >= (j + 1))
                                {
                                    var Manguera = ((Entidad.Manguera)((Entidad.Manguera)bgvDataElectronico.Columns[j].Tag));
                                    if (Manguera != null)
                                    {
                                        string Producto = Tanques.Where(t => t.ID == Convert.ToInt32(Manguera.TanqueID)).First().ProductoID.ToString() + "-" + Manguera.TanqueID.ToString();

                                        if (Manguera.PrecioDiferenciado)
                                            Producto += "Dif";

                                        if (prod.Equals(Producto))
                                        {
                                            decimal venta = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[j])), 3, MidpointRounding.AwayFromZero);
                                            if (Manguera.EsLecturaGalones)
                                                total += (venta * Parametros.General.LitrosGalonesTresDecimales);
                                            else
                                                total += venta;
                                        }
                                    }
                                }
                            }
                        }

                        //Obtengo el ID del producto de la tabla Total Extraxion
                        //int IDProd = ((Int32)(dgvDataTotalExtraxion.Columns[i].Tag));

                        //Obtengo el ID del producto y del tanque de la tabla Total Extraxion
                        string item = ((string)(dgvDataTotalExtraxion.Columns[i].Tag));
                        string result = item.Replace("Dif", "");
                        //string input = Regex.Replace(item, "[^0-9]+", string.Empty);
                        string[] llaves = result.Split('-');

                        int IDP = Convert.ToInt32(llaves.ElementAt(0));
                        int IDT = Convert.ToInt32(llaves.ElementAt(1));

                        //Verifico si el ID esta en la lista de ID's, si esta se resetea a 0 el valor del descuento automatico
                        if (!ID.Contains(result))
                        {
                            bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[result], 0m);

                            var AP = ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(IDP) && ap.TanqueID.Equals(IDT));

                            foreach (var objAP in AP)
                            {
                                foreach (var objAM in objAP.ArqueoMangueras.Where(a => a.ArqueoProductoID.Equals(objAP.ID)))
                                {
                                    decimal Desc = 0;
                                    Desc += Convert.ToDecimal(bgvDataDescuento.GetRowCellValue(0, bgvDataDescuento.Columns[result]));
                                    Desc += Decimal.Round((objAM.LecturaMecanicaFinal - objAM.LecturaMecanicaInicial) * Convert.ToDecimal(objAM.Descuento), 2, MidpointRounding.AwayFromZero);
                                    bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[result], Desc);
                                    objAP.DescuentoDispensador = Desc;
                                    bgvDataDescuento.RefreshData();

                                }
                            }
                            ID.Add(result);
                        }
                        dgvDataTotalExtraxion.SetRowCellValue(0, dgvDataTotalExtraxion.Columns[i], total.ToString("#,0.000"));
                    }

                    decimal suma = 0.00m;

                    for (int i = 2; i < bgvDataDescuento.Columns.Count; i++)
                    {
                        decimal calc = 0m;
                        suma += Decimal.TryParse(bgvDataDescuento.Columns[i].SummaryText, out calc) ? calc : 0;
                        txtDescuentoTotal.Text = Decimal.Round(suma, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
                    }

                    dgvDataTotalExtraxion.RefreshData();
                    TotalExtraxionVenta();
                    if (btnOK.Enabled.Equals(false))
                        btnOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void TotalExtraxionVenta()
        {
            try
            {
                decimal totalVentas = 0.00m;
                for (int i = 2; i < dgvVentas.Columns.Count; i++)
                {
                    string item = dgvVentas.Columns[i].FieldName;

                    decimal resta = 0.000m;

                    for (int j = 2; j < bgvDataExtraciones.Columns.Count; j++)
                    {
                        if (Convert.ToString(bgvDataExtraciones.Columns[j].FieldName).Equals("Valor" + item))
                        {
                            if (!String.IsNullOrEmpty(bgvDataExtraciones.Columns[j].SummaryText))
                                resta += Decimal.Round(Convert.ToDecimal(bgvDataExtraciones.Columns[j].SummaryText), 3, MidpointRounding.AwayFromZero);
                        }
                    }

                    decimal suma = 0.000m;
                    for (int j = 2; j < dgvDataTotalExtraxion.Columns.Count; j++)
                    {
                        if (Convert.ToString(dgvDataTotalExtraxion.Columns[j].FieldName).Equals(item))
                        {                                
                            suma += Decimal.Round(Convert.ToDecimal(dgvDataTotalExtraxion.GetRowCellValue(0, dgvDataTotalExtraxion.Columns[j])), 3, MidpointRounding.AwayFromZero);                        
                        }
                    }         
                       
                    //Valor Extraccion / Ventas (Venta Litros)
                    dgvVentas.SetRowCellValue(0, dgvVentas.Columns[i], Decimal.Round((suma - resta), 3, MidpointRounding.AwayFromZero));

                    //se obtiene valor de la venta de acuerdo al precio del día (Venta Valor)
                    decimal ValorVenta = Decimal.Round((Convert.ToDecimal(dgvVentas.GetRowCellValue(0, dgvVentas.Columns[i])) * Convert.ToDecimal(dgvVentas.GetRowCellValue(1, dgvVentas.Columns[i]))), 2, MidpointRounding.AwayFromZero);
                    dgvVentas.SetRowCellValue(3, dgvVentas.Columns[i], ValorVenta);
                    totalVentas += ValorVenta;

                    string result = item.Replace("Dif", "");
                    //string input = Regex.Replace(item, "[^0-9]+", string.Empty);
                    string[] llaves = result.Split('-');

                    int IDP = Convert.ToInt32(llaves.ElementAt(0));
                    int IDT = Convert.ToInt32(llaves.ElementAt(1));

                    var AP = ArqueoSelected.ArqueoProductos.Where(ap => ap.ProductoID.Equals(Convert.ToInt32(IDP)) && ap.TanqueID.Equals(IDT));
                    AP.First(o => o.EsDiferenciado.Equals(item.Contains("Dif"))).VentaLitro = Decimal.Round((suma - resta), 3, MidpointRounding.AwayFromZero);
                    AP.First(o => o.EsDiferenciado.Equals(item.Contains("Dif"))).VentaValor = ValorVenta;

                    bool EsAmbosLados = true;
                    decimal NonDesc = 0m;
                    foreach (var objAP in AP)
                    {
                        foreach (var objAM in objAP.ArqueoMangueras.Where(a => a.ArqueoProductoID.Equals(objAP.ID)))
                        {
                            if (objAM.Descuento.Equals(0))
                            {
                                EsAmbosLados = false;
                                NonDesc = objAM.Descuento;
                                break;
                            }

                            NonDesc = objAM.Descuento;
                        }

                    }

                    if (EsAmbosLados)
                    {
                        //decimal valorAnterior = Convert.ToDecimal(bgvDataDescuento.GetRowCellValue(0, bgvDataDescuento.Columns[item]));
                        bgvDataDescuento.SetRowCellValue(0, bgvDataDescuento.Columns[item], Decimal.Round((suma - resta) * NonDesc, 2, MidpointRounding.AwayFromZero));
                        AP.First().DescuentoDispensador = Decimal.Round((suma - resta) * NonDesc, 2, MidpointRounding.AwayFromZero);
                        bgvDataDescuento.RefreshData();
                    }

                }

                txtTotalVentas.Text = totalVentas.ToString("N2");

                dgvVentas.RefreshData();
            }   
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private bool EsFormulaCorrecta(string OriginalString)
        {
            try
            {
                char[] delimiters = new char[] { '(', ')', '+', '*', '/', '=', '.', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

                foreach (char letra in OriginalString)
                {
                    if (!delimiters.Contains(letra))
                    {
                        Parametros.General.DialogMsg("La Formula contiene una letra o caracter no valido '" + letra.ToString() + "', por favor revise la fórmula.", Parametros.MsgType.warning);
                        return false;
                    }
                }
                return true;
            }
            catch
            { return false; }
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
                            //Decimal valor = 0m;

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
                decimal a, b;


                if (decimal.TryParse(split[0], out a) && decimal.TryParse(split[1], out b))
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

        #endregion
        
        //****************//
        // GUARDAR ARQUEO //
        //****************//
        // GUARDAR ARQUEO //
        //****************// 

        private bool Guardar()
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 200;
                string info = "";
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                try
                {   
                    
                    if (ArqueoSelected != null)
                    {
                        #region <<< Arqueo Isla >>>

                        Entidad.ArqueoIsla AI = ArqueoSelected;

                        if (!Editable)
                        {
                            var ObjNumber = from ai in db.ArqueoIslas
                                            join t in db.Turnos on ai.TurnoID equals t.ID
                                            join rd in db.ResumenDias on t.ResumenDiaID equals rd.ID
                                            where rd.EstacionServicioID.Equals(ResumenSelected.EstacionServicioID) && rd.SubEstacionID.Equals(ResumenSelected.SubEstacionID)
                                            select ai;


                            AI.Numero = Convert.ToInt32(ObjNumber.Count()) > 0 ? Convert.ToInt32(ObjNumber.Max(o => o.Numero)) + 1 : 1;
                                        
                            db.ArqueoIslas.InsertOnSubmit(AI);
                        }
                        else
                        {
                        AI = db.ArqueoIslas.Single(ai => ai.ID.Equals(ArqueoSelected.ID));
                        }

                        AI.Observacion = Observacion;
                        AI.DiferenciaRecibida = spDiferenciaRecibida.Value;
                        AI.TecnicoID = IDTecnico;
                        db.SubmitChanges();

                        if (Modificando)
                        {
                            DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(ArqueoSelected, 1));
                            DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(AI, 1));

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                "Se editó la Diferencia recibida en el Arqueo de Isla: "
                                + AI.Numero, this.Name, dtPosterior, dtAnterior); 
                        }

                        info = "Favor revisar los datos del encabezado del Arqueo";

                        #endregion

                        foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
                        {   
                            #region <<< ArqueosProductos >>>
                            info = "Favor revisar los datos (Productos del Arqueo)";
                            Entidad.ArqueoProducto NewAP = db.ArqueoProductos.Single(a => a.ID == arp.ID);
                            NewAP.DescuentoEspecial = arp.DescuentoEspecial;
                            NewAP.DescuentoGalonesPetroCardValor = arp.DescuentoGalonesPetroCardValor;
                            NewAP.DescuentoGalonesPetroCardFormula = arp.DescuentoGalonesPetroCardFormula;
                            NewAP.DescuentoDispensador = arp.DescuentoDispensador;
                            NewAP.VentaLitro = arp.VentaLitro;
                            NewAP.VentaValor = arp.VentaValor;
                            db.SubmitChanges();
                         
                            foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                            {
                                
                                info = "Favor revisar los datos (Lecturas de las Mangueras)";
                                if (arm.ID > 0)
                                {
                                    Entidad.ArqueoManguera AM = db.ArqueoMangueras.Single(a => a.ID == arm.ID);

                                    if (Editable)
                                    {
                                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(arm, 1));
                                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(AM, 1));

                                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                         "Se editó las lecturas del Arqueo de la Isla Número: " + ArqueoSelected.Numero + ", Manguera: " + db.Mangueras.Single(m => m.ID == arm.MangueraID).Numero, this.Name, dtPosterior, dtAnterior);
                                    }
                                    AM.LecturaMecanicaInicial = arm.LecturaMecanicaInicial;
                                    AM.LecturaMecanicaFinal = arm.LecturaMecanicaFinal;
                                    AM.LecturaEfectivoInicial = arm.LecturaEfectivoInicial;
                                    AM.LecturaEfectivoFinal = arm.LecturaEfectivoFinal;
                                    AM.LecturaElectronicaInicial = arm.LecturaElectronicaInicial;
                                    AM.LecturaElectronicaFinal = arm.LecturaElectronicaFinal;
                                    AM.Descuento = arm.Descuento;
                                    AM.Lado = arm.Lado;
                                    AM.DispensadorID = arm.DispensadorID;
                                    AM.TanqueID = arm.TanqueID;
                                    AM.EsLecturaGalones = arm.EsLecturaGalones;
                                    AM.FlujoRapido = arm.FlujoRapido;


                                    db.SubmitChanges();                                
                                }
                            }
                            #endregion

                            #region <<< ArqueoProductoExtraxion >>>
                            foreach (DataRow RowAPE in dtExtracciones.Rows)
                            {
                                info = "Favor revisar los datos (Distribución de Extracción)";
                                string valor = "Valor" + arp.ProductoID + "-" + arp.TanqueID;
                                string formula = "Formula" + arp.ProductoID + "-" + arp.TanqueID;

                                if (arp.EsDiferenciado)
                                {
                                    valor += "Dif";
                                    formula += "Dif";
                                }

                                if (!String.IsNullOrEmpty(Convert.ToString(RowAPE[valor])))
                                {
                                    if (Convert.ToDecimal(RowAPE[valor]) >= 0)
                                    {
                                        var APEAnterior = db.ArqueoProductoExtracions.Where(x => x.ArqueoProductoID == arp.ID && x.ExtracionID == Convert.ToInt32(RowAPE["IDExtraxion"]));

                                        if (APEAnterior.Count() > 0)
                                        {
                                            Entidad.ArqueoProductoExtracion APE = APEAnterior.First();

                                            APE.Valor = Convert.ToDecimal(RowAPE[valor]);
                                            APE.Formula = Convert.ToString(RowAPE[formula]);
                                            db.SubmitChanges();

                                            if (Convert.ToBoolean(RowAPE["TieneConcepto"]))
                                            {
                                                var APECAnterior = db.ArqueoProductoExtracionConceptos.Where(x => x.ArqueoProductoExtracionID == APE.ID);

                                                if (APECAnterior.Count() > 0)
                                                {
                                                    foreach (var objAPEC in APECAnterior)
                                                    {
                                                        DataRow[] ESRow = dtConcepto.Select("IDAPEC = " + objAPEC.ID);
                                                        if (ESRow.Count() > 0)
                                                        {
                                                            DataRow row = ESRow.First();
                                                            Entidad.ArqueoProductoExtracionConcepto APEC = objAPEC;
                                                            APEC.ExtracionConceptoID = Convert.ToInt32(row["IDConcepto"]);
                                                            APEC.Litros = Convert.ToDecimal(row["Litros"]);
                                                            db.SubmitChanges();
                                                            dtConcepto.Rows.Remove(row);

                                                        }
                                                        else if (ESRow.Count() <= 0)
                                                        {
                                                            Entidad.ArqueoProductoExtracionConcepto APEC = objAPEC;
                                                            db.ArqueoProductoExtracionConceptos.DeleteOnSubmit(APEC);
                                                            db.SubmitChanges();
                                                        }
                                                    }
                                                }

                                                foreach (DataRow dtCr in dtConcepto.DefaultView.Table.Rows)
                                                {
                                                    if (!String.IsNullOrEmpty(dtCr["IDProducto"].ToString()))
                                                    {
                                                        if (Convert.ToInt32(dtCr["IDProducto"]) == APE.ArqueoProducto.ProductoID)
                                                        {
                                                            Entidad.ArqueoProductoExtracionConcepto APEC = new Entidad.ArqueoProductoExtracionConcepto();
                                                            APEC.ArqueoProductoExtracionID = APE.ID;
                                                            APEC.ExtracionConceptoID = Convert.ToInt32(dtCr["IDConcepto"]);
                                                            APEC.Litros = Convert.ToDecimal(dtCr["Litros"]);
                                                            db.ArqueoProductoExtracionConceptos.InsertOnSubmit(APEC);
                                                            db.SubmitChanges();
                                                        }
                                                    }
                                                }

                                            }

                                        }
                                        if (APEAnterior.Count() <= 0)
                                        {
                                            Entidad.ArqueoProductoExtracion APE = new Entidad.ArqueoProductoExtracion();
                                            APE.ArqueoProductoID = arp.ID;
                                            APE.ExtracionID = Convert.ToInt32(RowAPE["IDExtraxion"]);
                                            APE.Valor = Convert.ToDecimal(RowAPE[valor]);
                                            APE.Formula = Convert.ToString(RowAPE[formula]);
                                            db.ArqueoProductoExtracions.InsertOnSubmit(APE);
                                            db.SubmitChanges();

                                            //if (!Convert.ToBoolean(RowAPE["TieneConcepto"]))
                                            //    APE.Formula = Convert.ToString(RowAPE[formula]);
                                            if (Convert.ToBoolean(RowAPE["TieneConcepto"]))
                                            {
                                                foreach (DataRow dtCr in dtConcepto.DefaultView.Table.Rows)
                                                {
                                                    if (!String.IsNullOrEmpty(dtCr["IDProducto"].ToString()))
                                                    {
                                                        if (Convert.ToInt32(dtCr["IDProducto"]) == APE.ArqueoProducto.ProductoID)
                                                        {
                                                            Entidad.ArqueoProductoExtracionConcepto APEC = new Entidad.ArqueoProductoExtracionConcepto();
                                                            APEC.ArqueoProductoExtracionID = APE.ID;
                                                            APEC.ExtracionConceptoID = Convert.ToInt32(dtCr["IDConcepto"]);
                                                            APEC.Litros = Convert.ToDecimal(dtCr["Litros"]);
                                                            db.ArqueoProductoExtracionConceptos.InsertOnSubmit(APEC);
                                                            db.SubmitChanges();
                                                        }
                                                    }
                                                }

                                            }

                                            //db.ArqueoProductoExtracions.InsertOnSubmit(APE);
                                            //db.SubmitChanges();

                                            if (Editable)
                                            {
                                                //Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                                                //    "Se editó la Distribucíon de Extracción del Arqueo de la Isla Número: "
                                                //    + ArqueoSelected.Numero + ", Agregando la Extracción: " +
                                                //    db.ExtracionPagos.Single(x => x.ID == APE.ExtracionID).Nombre +
                                                //        "al Producto: " + db.Productos.Single(p => p.ID == arp.ID).Nombre,
                                                //this.Name);

                                            }
                                        }

                                    }
                                }
                                else if (String.IsNullOrEmpty(Convert.ToString(RowAPE[valor])) || Convert.ToDecimal(RowAPE[valor]) < 0)
                                {
                                    var APEAnterior = db.ArqueoProductoExtracions.Where(x => x.ArqueoProductoID == arp.ID && x.ExtracionID == Convert.ToInt32(RowAPE["IDExtraxion"]));

                                    if (APEAnterior.Count() > 0)
                                    {
                                        Entidad.ArqueoProductoExtracion APEDel = APEAnterior.First();
                                        var APECAnterior = db.ArqueoProductoExtracionConceptos.Where(ac => ac.ArqueoProductoExtracionID == APEDel.ID);
                                        db.ArqueoProductoExtracionConceptos.DeleteAllOnSubmit(APECAnterior);
                                        db.ArqueoProductoExtracions.DeleteOnSubmit(APEDel);
                                        db.SubmitChanges();
                                    }

                                }


                                //foreach (DataColumn ColAPE in RowAPE.Table.Columns)
                                //{
                                //    if (ColAPE.ColumnName == "Valor" + arp.ProductoID)
                                //    {
                                //        if (!String.IsNullOrEmpty(Convert.ToString(RowAPE[ColAPE])))
                                //        {
                                //            if (Convert.ToDecimal(RowAPE[ColAPE]) > 0)
                                //                MessageBox.Show(RowAPE[ColAPE].ToString());
                                //        }
                                //    }


                                //}
                            }
                            db.SubmitChanges();
                            #endregion

                        }

                        foreach (DataRow rPago in dtPago.Rows)
                        {
                            info = "Favor revisar los datos (Formas de Pago)";
                            if (!String.IsNullOrEmpty(Convert.ToString(rPago["ValorPago"])))
                            {
                                if (Convert.ToDecimal(rPago["ValorPago"]) > 0)
                                {
                                    var AFPAnterior = db.ArqueoFormaPagos.Where(x => x.ArqueoIslaID == AI.ID && x.PagoID == Convert.ToInt32(rPago["IDPago"]));

                                    if (AFPAnterior.Count() > 0)
                                    {
                                        Entidad.ArqueoFormaPago AFP = AFPAnterior.First();

                                        AFP.Valor = Convert.ToDecimal(rPago["ValorPago"]);
                                        AFP.Formula = Convert.ToString(rPago["FormulaPago"]);
                                        db.SubmitChanges();
                                    }
                                    else if (AFPAnterior.Count() <= 0)
                                    {
                                        Entidad.ArqueoFormaPago AFP = new Entidad.ArqueoFormaPago();
                                        AFP.ArqueoIslaID = AI.ID;
                                        AFP.PagoID = Convert.ToInt32(rPago["IDPago"]);
                                        AFP.Valor = Convert.ToDecimal(rPago["ValorPago"]);
                                        AFP.Formula = Convert.ToString(rPago["FormulaPago"]);
                                        AFP.MonedaID = 0;
                                        db.ArqueoFormaPagos.InsertOnSubmit(AFP);
                                        db.SubmitChanges();
                                    }
                                }
                            }
                            else if (String.IsNullOrEmpty(Convert.ToString(rPago["ValorPago"])) || Convert.ToDecimal(rPago["FormulaPago"]) <= 0)
                            {
                                var AFPAnterior = db.ArqueoFormaPagos.Where(x => x.ArqueoIslaID == AI.ID && x.PagoID == Convert.ToInt32(rPago["IDPago"]));

                                if (AFPAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoFormaPago APEDel = AFPAnterior.First();
                                    db.ArqueoFormaPagos.DeleteOnSubmit(APEDel);
                                    db.SubmitChanges();
                                }

                            }
                        }

                        info = "Favor revisar los datos (Efectivo Recibido)";
                        if (!String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0]["Valor" + MonedaPrincipal.ToString()])))
                        {
                            if (Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaPrincipal.ToString()]) > 0)
                            {
                                var AFPEAnterior = db.ArqueoFormaPagos.Where(af => af.ArqueoIslaID == AI.ID && af.PagoID == IDFormaPagoEfectivo && af.MonedaID == MonedaPrincipal);

                                if (AFPEAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoFormaPago AFPE = AFPEAnterior.First();

                                    AFPE.Valor = Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaPrincipal.ToString()]);
                                    AFPE.Formula = Convert.ToString(dtPagoEfectivo.Rows[0]["Formula" + MonedaPrincipal.ToString()]);
                                    db.SubmitChanges();
                                }
                                else if (AFPEAnterior.Count() <= 0)
                                {
                                    Entidad.ArqueoFormaPago AFPE = new Entidad.ArqueoFormaPago();
                                    AFPE.ArqueoIslaID = AI.ID;
                                    AFPE.PagoID = IDFormaPagoEfectivo;
                                    AFPE.Valor = Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaPrincipal.ToString()]);
                                    AFPE.Formula = Convert.ToString(dtPagoEfectivo.Rows[0]["Formula" + MonedaPrincipal.ToString()]);
                                    AFPE.MonedaID = MonedaPrincipal;
                                    db.ArqueoFormaPagos.InsertOnSubmit(AFPE);
                                    db.SubmitChanges();
                                }

                            }
                        }
                        else if (String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0]["Valor" + MonedaPrincipal.ToString()])))
                        {
                            var AFPEAnterior = db.ArqueoFormaPagos.Where(af => af.ArqueoIslaID == AI.ID && af.PagoID == IDFormaPagoEfectivo && af.MonedaID == MonedaPrincipal);

                            if (AFPEAnterior.Count() > 0)
                            {
                                Entidad.ArqueoFormaPago AFPE = AFPEAnterior.First();
                                db.ArqueoFormaPagos.DeleteOnSubmit(AFPE);
                                db.SubmitChanges();
                            }
                        }

                        if (!String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0]["Valor" + MonedaSecundaria.ToString()])))
                        {
                            if (Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaSecundaria.ToString()]) > 0)
                            {
                                var AFPEAnterior = db.ArqueoFormaPagos.Where(af => af.ArqueoIslaID == AI.ID && af.PagoID == IDFormaPagoEfectivo && af.MonedaID == MonedaSecundaria);

                                if (AFPEAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoFormaPago AFPE = AFPEAnterior.First();

                                    AFPE.Valor = Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaSecundaria.ToString()]);
                                    AFPE.Formula = Convert.ToString(dtPagoEfectivo.Rows[0]["Formula" + MonedaSecundaria.ToString()]);
                                    db.SubmitChanges();
                                }
                                else if (AFPEAnterior.Count() <= 0)
                                {
                                    Entidad.ArqueoFormaPago AFPE = new Entidad.ArqueoFormaPago();
                                    AFPE.ArqueoIslaID = AI.ID;
                                    AFPE.PagoID = IDFormaPagoEfectivo;
                                    AFPE.Valor = Convert.ToDecimal(dtPagoEfectivo.Rows[0]["Valor" + MonedaSecundaria.ToString()]);
                                    AFPE.Formula = Convert.ToString(dtPagoEfectivo.Rows[0]["Formula" + MonedaSecundaria.ToString()]);
                                    AFPE.MonedaID = MonedaSecundaria;
                                    db.ArqueoFormaPagos.InsertOnSubmit(AFPE);
                                    db.SubmitChanges();
                                }

                            }
                        }
                        else if (String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0]["Valor" + MonedaSecundaria.ToString()])))
                        {
                            var AFPEAnterior = db.ArqueoFormaPagos.Where(af => af.ArqueoIslaID == AI.ID && af.PagoID == IDFormaPagoEfectivo && af.MonedaID == MonedaSecundaria);

                            if (AFPEAnterior.Count() > 0)
                            {
                                Entidad.ArqueoFormaPago AFPE = AFPEAnterior.First();
                                db.ArqueoFormaPagos.DeleteOnSubmit(AFPE);
                                db.SubmitChanges();
                            }
                        }

                        //if (!Editable)
                        //{
                        //    db.ArqueoIslas.InsertOnSubmit(AI);
                        //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo,
                        //    "Se creó el Arqueo de Isla Número: " + ArqueoSelected.Numero, this.Name);

                        //}

                        db.SubmitChanges();
                        trans.Commit();

                        ShowMsg = true;
                        btnOK.Enabled = false;
                        return true;
                    }
                    else
                    {
                        Parametros.General.DialogMsg("Debe de cargar los datos del encabezado para el Arqueo", Parametros.MsgType.warning);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR + Environment.NewLine + info, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                    return false;
                }

                finally
                {                    
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
            
        }

        #endregion

        #region *** EVENTOS ***

        #region <<< EventosGenerales >>>

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (!Guardar())
            {
                this.Cursor = Cursors.Default;
                return;
            }

            this.NextArqueo = true;
            this.Close();
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MDI != null)
                MDI.CleanDialog(ShowMsg, NextArqueo, IDAI);

            if (RDI != null)
                RDI.CleanDialog(ShowMsg);
        }
         
        private void DialogArqueoIsla_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnOK.Enabled.Equals(true))
            {
                DialogResult resultado;

                resultado = Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGMODIFICADOCLOSING + Environment.NewLine, Parametros.MsgType.questionNO);

                if (resultado == DialogResult.Cancel)
                    e.Cancel = true;
                else if (resultado == DialogResult.OK)
                {
                    if (!Guardar())
                    {
                        this.Cursor = Cursors.Default;
                        e.Cancel = true;
                    }
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void lkTecnico_Validated(object sender, EventArgs e)
        {
                Parametros.General.ValidateEmptyStringRule((BaseEdit)sender, errRequiredField);
        }

        /// <summary>
        /// Cargar los datos a mostrar en el detalle del arqueo de isla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                //Validar si el campo "ISLA" tiene datos para poder cargar los grid, una vez cargada los grid el boton se deshabilita
                if (ValidarCampos())
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (!Editable)
                    {

                        Entidad.ArqueoIsla AI = new Entidad.ArqueoIsla();

                        AI.UsuarioCreado = UsuarioID;
                        AI.TurnoID = TurnoSelected.ID;
                        AI.TecnicoID = IDTecnico;
                        AI.IslaID = IDIsla;
                        AI.FechaCreado = ResumenSelected.FechaInicial;
                        AI.ArqueoEspecial = chkArqueoEspecial.Checked;

                        AI.Estado = Convert.ToInt32(Parametros.Estados.Abierto);
                        ArqueoSelected = AI;
                    }

                    bdsArqueoIsla.DataSource = ArqueoSelected;
                    layoutControlGridLecturas.Visible = true;
                    if (FillLecturas())
                    {
                        this.btnLoad.Enabled = false;
                        this.lkIsla.Properties.ReadOnly = true;
                        this.dateFecha.Properties.ReadOnly = true;
                        this.lkSES.Properties.ReadOnly = true;
                        this.txtNumero.Text = ArqueoSelected.Numero.ToString();
                        this.chkArqueoEspecial.Checked = ArqueoSelected.ArqueoEspecial;
                        if (Efectivo.Rows.Count > 0 || Electronica.Rows.Count > 0)
                        {
                            if (Efectivo.Rows.Count <= 0)
                            {
                                this.layoutControlGridLecturas.Height -= 125;
                                this.layoutControlGroupElectronica.Height = 100;
                                this.panelControlPrincipal.Height -= 170;
                            }

                            if (Electronica.Rows.Count <= 0)
                            {
                                this.layoutControlGridLecturas.Height -= 125;
                                this.layoutControlGroupEfectivo.Height = 100;
                                this.panelControlPrincipal.Height -= 170;
                            }

                            this.layoutControlGroupMecanica.Height = 128;
                                                        
                                this.layoutControlVerificacion.Visible = true;
                                this.layoutControlVerificacion.BringToFront();
                        }
                        else
                        {
                            this.layoutControlGridLecturas.Height -= 250;
                            this.panelControlPrincipal.Height -= 360;
                        }
                        this.chkArqueoEspecial.Properties.ReadOnly = true;
                        this.layoutControlExtracionPago.Visible = true;
                        this.layoutControlExtracionPago.BringToFront();


                        if (Editable)
                            btnOK.Enabled = false;
                        else if (!Editable)
                            btnOK.Enabled = true;
                    }                
                    else
                        this.Close(); 
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        //*********************************************************************VISTA PREVIA******************************************************//
        /// <summary>
        /// Mostrar un Preview del Arqueo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (ArqueoSelected != null)
            {
                try
                {
                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    if (dbView.VistaArqueoIslas.Count(v => v.ArqueoIslaID.Equals(ArqueoSelected.ID)) > 0)
                    {
                        if (btnOK.Enabled.Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGMODIFICADOVISTAPREVIA + Environment.NewLine, Parametros.MsgType.question) == DialogResult.Cancel)
                                return;
                            else
                            {
                                if (!Guardar())
                                {
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                        }

                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                        rv.previewBarTop.Visible = false;
                        rv.TopMost = true;
                        rv.Text = "Vista Previa Arqueo # " + ArqueoSelected.Numero.ToString();
                        Reportes.Util.PrintArqueoFast(this, dbView, db, ArqueoSelected.ID, 0, ResumenSelected, Parametros.TiposImpresion.Vista_Previa, false, Parametros.TiposArqueo.Isla, Parametros.Properties.Resources.TXTVISTAPREVIA, rv.printControlAreaReport);
                        rv.ShowDialog();
                        rv.BringToFront();
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El Arqueo no ha sido guardado, primero debe de guardar el arqueo para luego ser visualizado.", Parametros.MsgType.warning);
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                }

            }
        }

        //*********************************************************************CERRAR ARQUEO******************************************************//
        /// <summary>
        /// Cerrar el Arqueo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarArqueo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Parametros.General.DialogMsg("¿Esta seguro de cerrar este arqueo?", Parametros.MsgType.question) == DialogResult.Cancel)
                    return;

                if (this.layoutControlGroupElectronica.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always && this.layoutControlGroupMecanica.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
                    {

                        foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                        {
                            if (arm.LecturaMecanicaFinal.Equals(0))
                            {
                                if (!arm.LecturaMecanicaInicial.Equals(0))
                                {
                                    Parametros.General.DialogMsg("Debe de ingresar las lecturas finales Mecánicas", Parametros.MsgType.warning);
                                    return;
                                }
                            }


                            if (this.layoutControlGroupElectronica.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                            {
                                if (arm.LecturaEfectivoFinal.Equals(0))
                                {

                                    if (!arm.LecturaEfectivoInicial.Equals(0))
                                    {
                                        Parametros.General.DialogMsg("Debe de ingresar las lecturas finales Electrónicas", Parametros.MsgType.warning);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (Entidad.ArqueoProducto arp in ArqueoSelected.ArqueoProductos)
                {
                    bool MecanicaCero = false,  ElectronicaCero = false;

                    foreach (Entidad.ArqueoManguera arm in arp.ArqueoMangueras)
                    {
                        var Disp = db.Dispensadors.FirstOrDefault(f => f.ID.Equals(db.Mangueras.Single(s => s.ID.Equals(arm.MangueraID)).DispensadorID));

                        if (Disp.LecturaMecanica)
                        {
                            if ((arm.LecturaMecanicaFinal - arm.LecturaMecanicaInicial) < 0)
                            {
                                Parametros.General.DialogMsg("La extracción en las lecturas Mecánicas no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                                return;
                            }
                            else if ((arm.LecturaMecanicaFinal - arm.LecturaMecanicaInicial) == 0)
                                MecanicaCero = true;
                        }

                        if (Disp.LecturaEfectivo)
                        {
                            if ((arm.LecturaEfectivoFinal - arm.LecturaEfectivoInicial) < 0)
                            {
                                Parametros.General.DialogMsg("La extracción en las lecturas Efectivo no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                                return;
                            }
                        }

                        if (Disp.LecturaElectronica)
                        {
                            if ((arm.LecturaElectronicaFinal - arm.LecturaElectronicaInicial) < 0)
                            {
                                Parametros.General.DialogMsg("La extracción en las lecturas Electrónica no pueden ser menor a cero (0)", Parametros.MsgType.warning);
                                return;
                            }
                            else
                            {
                                if (!Disp.MecanicaAmbasCara)
                                {
                                    if (Disp.LecturaMecanica)
                                    {
                                        if ((arm.LecturaElectronicaFinal - arm.LecturaElectronicaInicial) == 0)
                                            ElectronicaCero = true;

                                        if (arm.LecturaElectronicaFinal > 0 || arm.LecturaElectronicaInicial > 0)
                                        {
                                            if (!MecanicaCero.Equals(ElectronicaCero))
                                            {
                                                Parametros.General.DialogMsg("Las lecturas Mecánicas y Electrónicas ambas no son iguales a Cero", Parametros.MsgType.warning);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                  
                    //if (MecanicaCero != EfectivoCero || MecanicaCero != ElectronicaCero || EfectivoCero != ElectronicaCero)
                    //{
                    //    if (arp.ArqueoMangueras.Where(o => o.))
                    //}
                }

                if (!ArqueoSelected.ArqueoProductos.Sum(s => s.VentaValor).Equals((String.IsNullOrEmpty(txtTotalVentas.Text) ? 0m : Convert.ToDecimal(txtTotalVentas.Text))))
                {
                    Parametros.General.DialogMsg("La suma del valor de la venta en datos de la venta no es igual al Valor Total Venta en el cuadro de resumen", Parametros.MsgType.warning);
                    return;
                }

                if (lkTecnico.EditValue != null)
                {
                    if (Convert.ToInt32(lkTecnico.EditValue).Equals(0) && !_GrandSobranteDiferencia.Equals(0))
                    {
                        Parametros.General.DialogMsg("La Isla cerrada no puede tener faltante o sobrante", Parametros.MsgType.warning);
                        return;
                    }
                }

                if (btnOK.Enabled.Equals(true))
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGMODIFICADOFINALIZAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.Cancel)
                        return;
                    else
                    {
                        if (!Guardar())
                        {
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

            if (!ValidarArqueo()) return;

            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 200;

                try
                {
                    Entidad.ArqueoIsla AI = db.ArqueoIslas.Single(a => a.ID == ArqueoSelected.ID);
                    AI.Cerrado = true;
                    AI.FechaCerrado = Convert.ToDateTime(db.GetDateServer());
                    AI.UsuarioCerrado = Parametros.General.UserID;
                    Parametros.Estados State = ((Parametros.Estados)(AI.Estado));
                    AI.Estado = Convert.ToInt32(Parametros.Estados.Cerrado);
                    AI.ValorTotalVenta = Convert.ToDecimal(txtTotalVentas.Text);
                    AI.VentasContado = Convert.ToDecimal(txtVentasContado.Text);
                    AI.SobranteFaltante = _GrandSobranteDiferencia;
                    db.SubmitChanges();
                    
                    if (!AI.ArqueoEspecial && (!Modificando || db.ArqueoIslas.Where(ai => ai.IslaID.Equals(ArqueoSelected.IslaID)).OrderByDescending(o => o.ID).First().ID.Equals(ArqueoSelected.ID)))
                    {
                        foreach (var AM in db.ArqueoMangueras.Where(am => am.ArqueoProducto.ArqueoIslaID.Equals(AI.ID)))
                        {
                            Entidad.Manguera EtManguera = db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID));

                            EtManguera.LecturaMecanica = AM.LecturaMecanicaFinal;

                            if (this.layoutControlGroupEfectivo.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                                EtManguera.LecturaEfectivo = AM.LecturaEfectivoFinal;
                            else
                                EtManguera.LecturaEfectivo = AM.LecturaEfectivoInicial = AM.LecturaEfectivoFinal = 0;

                            if (this.layoutControlGroupElectronica.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                                EtManguera.LecturaElectronica = AM.LecturaElectronicaFinal;
                            else
                                EtManguera.LecturaElectronica = AM.LecturaElectronicaInicial = AM.LecturaElectronicaFinal = 0;

                            db.SubmitChanges();
                        }
                    }

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Finalizó el Arqueo de Isla: " + AI.Numero.ToString() + " del " + ResumenSelected.FechaInicial.Date.ToString(), this.Name);
                    db.SubmitChanges();
                    trans.Commit();
                    this.ShowMsg = true;
                    Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    Reportes.Util.PrintArqueoFast(this, dbView, db, AI.ID, 0, ResumenSelected, (State.Equals(Parametros.Estados.Abierto) ? Parametros.TiposImpresion.Original : Parametros.TiposImpresion.Modificado), true, Parametros.TiposArqueo.Isla, Parametros.Properties.Resources.TXTCIERREREGISTRO, null);
                    this.Close();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR + Environment.NewLine, Parametros.MsgType.error, ex.ToString());
                    return;
                }

                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();                    
                }
            }
        }


        /// <summary>
        /// Actualizar Datos del Arqueo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ArqueoSelected != null)
                {
                    if (db.ArqueoIslas.Count(v => v.ID.Equals(ArqueoSelected.ID)) > 0)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                        Entidad.ResumenDia RD = db.ResumenDias.Single(rd => rd.ID.Equals(ResumenSelected.ID));
                        RD.TipoCambioMoneda = Parametros.Config.TipoCambioArqueo(RD.FechaInicial);
                        RD.DescuentoPetroCard = Parametros.Config.DescuentoPetroCard();
                        db.SubmitChanges();

                        ResumenSelected = RD;
                        TipoCambioArqueo = ResumenSelected.TipoCambioMoneda;
                        DescuentoPetroCard = ResumenSelected.DescuentoPetroCard;


                        foreach (var ObjAP in db.ArqueoProductos.Where(ap => ap.ArqueoIslaID.Equals(ArqueoSelected.ID)))
                        {
                            Entidad.ArqueoProducto AP = db.ArqueoProductos.Single(ap => ap.ID.Equals(ObjAP.ID));
                            decimal precio = 0;
                            try
                            {
                                precio = Decimal.Round(Convert.ToDecimal(db.GetPrecio(AP.ProductoID, IDES, ResumenSelected.SubEstacionID, AP.EsDiferenciado, TurnoSelected.Numero, ArqueoSelected.ArqueoEspecial, Convert.ToDateTime(ArqueoSelected.FechaCreado).Date)), 2, MidpointRounding.AwayFromZero);
                            }
                            catch { precio = 0; }

                            AP.Precio = precio;
                            db.SubmitChanges();

                            ArqueoSelected.ArqueoProductos.ToList().ForEach(arqueos =>
                                {
                                    foreach (Entidad.ArqueoManguera AM in arqueos.ArqueoMangueras)
                                    {
                                        AM.Descuento = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).Descuento);
                                        Entidad.Manguera etMang = db.Mangueras.Single(s => s.ID.Equals(AM.MangueraID));

                                        AM.Lado = etMang.Lado;
                                        AM.DispensadorID = etMang.DispensadorID;
                                        AM.TanqueID = etMang.TanqueID;
                                        AM.EsLecturaGalones = etMang.EsLecturaGalones;
                                        AM.FlujoRapido = etMang.FlujoRapido;

                                        if (!ArqueoSelected.Cerrado && ArqueoSelected.Estado.Equals(Convert.ToInt32(Parametros.Estados.Abierto)))
                                        {
                                            AM.LecturaMecanicaInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaMecanica);
                                            AM.LecturaEfectivoInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaEfectivo);
                                            AM.LecturaElectronicaInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaElectronica);
                                        }

                                        if (ArqueoSelected.Estado.Equals(Parametros.Estados.Modificado))
                                        {
                                            var AIAnterior = db.ArqueoIslas.Where(d => d.IslaID.Equals(ArqueoSelected.IslaID)).OrderByDescending(o => o.ID).First();

                                            if (ArqueoSelected.ID.Equals(AIAnterior.ID))
                                            {
                                            AM.LecturaMecanicaInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaMecanica);
                                            AM.LecturaEfectivoInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaEfectivo);
                                            AM.LecturaElectronicaInicial = Convert.ToDecimal(db.Mangueras.Single(m => m.ID.Equals(AM.MangueraID)).LecturaElectronica);
                                        
                                            }
                                        }

                                    }
                                });

                        }

                        if (!Guardar())
                        {
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        Parametros.General.DialogMsg("Actualización realizada exitosamente." + Environment.NewLine + "Los datos volverán a ser generados", Parametros.MsgType.message);
                        this.btnOK.Enabled = false;
                        IDAI = ArqueoSelected.ID;
                        this.Close();
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El Arqueo no ha sido guardado, primero debe de guardar el arqueo para luego ser visualizado.", Parametros.MsgType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region <<< EventosLecturaMecanica >>>

        private void bgvDataMecanica_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la primera y ultima fila

            if (Modificando)
            {
                if (!db.ArqueoIslas.Where(ai => ai.IslaID.Equals(ArqueoSelected.IslaID)).OrderByDescending(o => o.ID).First().ID.Equals(ArqueoSelected.ID))
                e.Cancel = true;
            }

            if (bgvDataMecanica.FocusedRowHandle == 0)
                e.Cancel = true; 

            if (bgvDataMecanica.FocusedRowHandle == 2)
                e.Cancel = true;
        }           

        private void bgvDataMecanica_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataMecanica.GetFocusedRowCellValue(e.Column))))
                    bgvDataMecanica.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    /////////////////////////////////////////////////////////////////////MessageBox.Show(e.Column.FieldName);

                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(0, e.Column)), 3);
                    extracion = Decimal.Round(Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(1, e.Column)), 3);

                    final = extracion - inicial;

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado);
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaMecanicaFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataMecanica.SetRowCellValue(2, e.Column, final);
                    VerificacionMecVsElectronica();
                                                                      

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }
          
        private void bgvDataMecanica_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataMecanica.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataMecanica.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataMecanica.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataMecanica_LostFocus(object sender, EventArgs e)
        {
            //Recorrer las columnas de la lectura Mecanica
            for (int i = 2; i < bgvDataMecanica.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataMecanica.GetRowCellValue(2, bgvDataMecanica.Columns[i])) < 0)
                {   
                    bgvDataMecanica.SetColumnError(bgvDataMecanica.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);
                    break;
                }
            }
        }

        #endregion 

        #region  <<< EventosLecturaEfectivo >>>

        private void bgvDataEfectivo_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la primera y ultima fila
            if (Modificando)
            {
                if (!db.ArqueoIslas.Where(ai => ai.IslaID.Equals(ArqueoSelected.IslaID)).OrderByDescending(o => o.ID).First().ID.Equals(ArqueoSelected.ID))
                    e.Cancel = true;
            }

            if (bgvDataEfectivo.FocusedRowHandle == 0)
                e.Cancel = true;

            if (bgvDataEfectivo.FocusedRowHandle == 2)
                e.Cancel = true;
                   
        }

        private void bgvDataEfectivo_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataEfectivo.GetFocusedRowCellValue(e.Column))))
                    bgvDataEfectivo.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(0, e.Column));
                    extracion = Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(1, e.Column));

                    final = extracion - inicial;

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado);
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaEfectivoFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataEfectivo.SetRowCellValue(2, e.Column, final);

                    //Recorrer las columnas del Grid de Verificación y validar el total
                    VerificacionLecturaEfectivo(); 

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void bgvDataEfectivo_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataEfectivo.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataEfectivo.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataEfectivo.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataEfectivo_LostFocus(object sender, EventArgs e)
        {

            //Recorrer las columnas de la lectura Efectivo
            for (int i = 2; i < bgvDataEfectivo.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataEfectivo.GetRowCellValue(2, bgvDataEfectivo.Columns[i])) < 0)
                {
                    bgvDataEfectivo.SetColumnError(bgvDataEfectivo.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);

                    break;
                }

            }
        }

        #endregion

        #region <<< EventosLecturaElectronica >>>

        private void bgvDataElectrico_ShowingEditor(object sender, CancelEventArgs e)
        {
            //desabilitar edicion para la primera y ultima fila
            if (Modificando)
            {
                if (!db.ArqueoIslas.Where(ai => ai.IslaID.Equals(ArqueoSelected.IslaID)).OrderByDescending(o => o.ID).First().ID.Equals(ArqueoSelected.ID))
                    e.Cancel = true;
            }

            if (bgvDataElectronico.FocusedRowHandle == 0)
                e.Cancel = true;

            if (bgvDataElectronico.FocusedRowHandle == 2)
                e.Cancel = true;
        }

        private void bgvDataElectrico_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle == 2)
                    return;
                //verifica si la celda no tenga valor nulo, de ser asi se le asigna el valor de 0
                if (String.IsNullOrEmpty(Convert.ToString(bgvDataElectronico.GetFocusedRowCellValue(e.Column))))
                    bgvDataElectronico.SetFocusedRowCellValue(e.Column, 0.000m);
                else
                {
                    decimal inicial, extracion, final;

                    //tomamos los datos de las dos primeras filas
                    inicial = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(0, e.Column)), 3, MidpointRounding.AwayFromZero);
                    extracion = Decimal.Round(Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(1, e.Column)), 3, MidpointRounding.AwayFromZero);

                    final = extracion - inicial;

                    //Asignar el monto de la extraxion final a la entidad ArqueoManguera
                    var mang = ((Entidad.Manguera)((Parametros.MyBandColumn)e.Column).Tag);

                    int IdProd = Tanques.Where(t => t.ID == Convert.ToInt32(mang.TanqueID)).First().ProductoID;

                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.ProductoID == Convert.ToInt32(IdProd) && ap.TanqueID.Equals(mang.TanqueID) && ap.EsDiferenciado == mang.PrecioDiferenciado); 
                    EtArqueoManguera = EtArqueoProducto.ArqueoMangueras.Single(am => am.MangueraID == mang.ID);
                    EtArqueoManguera.LecturaElectronicaFinal = extracion;

                    //asignamos el valor final de la lectura
                    bgvDataElectronico.SetRowCellValue(2, e.Column, final);
                    VerificacionLecturaEfectivo();
                    VerificacionMecVsElectronica();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void bgvDataElectrico_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (bgvDataElectronico.FocusedRowHandle == 1)
            {
                if (e.PrevFocusedColumn != null)
                {
                    if (e.PrevFocusedColumn.ColumnHandle >= 1)
                    {
                        //ErrorProvider para los valores finales menores que 0
                        if (Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, e.PrevFocusedColumn)) < 0)
                        {
                            bgvDataElectronico.SetColumnError(e.PrevFocusedColumn, "La Extracción es NEGATIVA", ErrorType.Critical);
                        }
                        else
                            bgvDataElectronico.SetColumnError(e.PrevFocusedColumn, "", ErrorType.None);
                    }
                }
            }
        }

        private void bgvDataElectrico_LostFocus(object sender, EventArgs e)
        {
            //Recorrer las columnas de la lectura Efectivo
            for (int i = 2; i < bgvDataEfectivo.Columns.Count; i++)
            {
                if (Convert.ToDecimal(bgvDataElectronico.GetRowCellValue(2, bgvDataElectronico.Columns[i])) < 0)
                {
                    bgvDataElectronico.SetColumnError(bgvDataElectronico.Columns[i], "La Extracción es NEGATIVA", ErrorType.Critical);
                    break;
                }
            }
        }

        #endregion

        #region <<< EventosExtraxiones >>>

        /// <summary>
        /// Agregar un RepositoryItem diferente por cada celda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDataExtraciones_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //try
            //{
            //    if (e.Column == bgvDataExtraciones.FocusedColumn)
            //    {
            //        if (Convert.ToBoolean(bgvDataExtraciones.GetFocusedRowCellValue("TieneConcepto")) && bgvDataExtraciones.FocusedColumn.VisibleIndex > 0)
            //        {
            //            DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit rpPopUp = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();

            //            BindingSource bdsConceptos = new System.Windows.Forms.BindingSource(); 

            //            DevExpress.XtraGrid.GridControl rpGrid = new DevExpress.XtraGrid.GridControl();
            //            DevExpress.XtraGrid.Views.Grid.GridView rpGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            //            DevExpress.XtraGrid.Columns.GridColumn colIDAPEC = new DevExpress.XtraGrid.Columns.GridColumn();
            //            DevExpress.XtraGrid.Columns.GridColumn colIDConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            //            DevExpress.XtraGrid.Columns.GridColumn colConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
            //            DevExpress.XtraGrid.Columns.GridColumn colLitro = new DevExpress.XtraGrid.Columns.GridColumn();
            //            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkConcepto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            //            DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLitro = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            //            rpGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { lkConcepto, rpLitro });

            //            lkConcepto.AutoHeight = false;
            //            //lkConcepto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            //            lkConcepto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] { new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Concepto", "Conceptos") });
            //            lkConcepto.NullText = "<No tiene Concepto Asignado>";

            //            lkConcepto.DataSource = from es in db.ExtracionConceptos
            //                                           select new { IDConcepto = es.ID, es.Concepto };

            //            lkConcepto.DisplayMember = "Concepto";
            //            lkConcepto.ValueMember = "IDConcepto";

            //            //**Repositorio para digitar la lectura                         
            //            rpLitro.AutoHeight = false;
            //            rpLitro.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
            //            rpLitro.Buttons.Clear();

            //            rpLitro.EditFormat.FormatString = "N3";
            //            rpLitro.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //            rpLitro.DisplayFormat.FormatString = "N3";
            //            rpLitro.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //            rpLitro.EditMask = "N3";

            //            //dtConcepto.Select("IDProducto = " + Convert.ToString(e.Column.Tag));
            //            dtConcepto.DefaultView.RowFilter = "IDProducto = " + Convert.ToString(e.Column.Tag);
            //            bdsConceptos.DataSource = dtConcepto.DefaultView;

            //            //bdsConceptos.Filter = "IDProducto = " + Convert.ToString(e.Column.Tag);
            //            //Grid
            //            rpGrid.MainView = rpGridView;
            //            rpGrid.DataSource = bdsConceptos;
            //            //GridView
            //            rpGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {colIDConcepto,  colConcepto, colLitro});

            //            rpGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            //            rpGridView.OptionsView.ShowGroupPanel = false;
            //            rpGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            //            rpGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            //            rpGridView.OptionsNavigation.AutoFocusNewRow = true;
            //            rpGridView.OptionsView.EnableAppearanceEvenRow = true;
            //            rpGridView.OptionsView.EnableAppearanceOddRow = true;
            //            rpGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            //            rpGridView.Tag = e.Column.Tag;
            //            //rpGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.rpGridView_FocusedRowChanged);
            //            rpGridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.rpGridView_CellValueChanged);
            //            rpGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rpGridView_KeyDown);
            //            //
            //            // ColIDAPEC
            //            //
            //            colIDConcepto.FieldName = "IDAPEC";
            //            colConcepto.Visible = false;   
            //            //
            //            // ColID
            //            //
            //            colIDConcepto.FieldName = "IDProducto";
            //            colConcepto.Visible = false;                        
            //            // 
            //            // colConcepto
            //            // 
            //            colConcepto.Caption = "Concepto";
            //            colConcepto.FieldName = "IDConcepto";
            //            colConcepto.ColumnEdit = lkConcepto;
            //            colConcepto.Visible = true;
            //            colConcepto.VisibleIndex = 0;
            //            //colConcepto. = 0;
            //            // 
            //            // colLitro
            //            // 
            //            colLitro.Caption = "Litros";
            //            colLitro.FieldName = "Litros";
            //            colLitro.Visible = true;
            //            colLitro.VisibleIndex = 1;
            //            colLitro.ColumnEdit = rpLitro;
            //            colLitro.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            //            colLitro.DisplayFormat.FormatString = "N3";

            //            rpGrid.Dock = DockStyle.Fill;
            //            DevExpress.XtraEditors.PopupContainerControl popupControl = new DevExpress.XtraEditors.PopupContainerControl();
            //            popupControl.Controls.Add(rpGrid);


            //            //PopupContainerEdit editor = new PopupContainerEdit();
            //            rpPopUp.Properties.PopupControl = popupControl;
            //            rpPopUp.Tag = rpGridView;
            //            rpPopUp.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpPopUp_Closed);

            //            //Controls.Add(editor);

            //            e.RepositoryItem = rpPopUp;
            //            //dgvDataExtraciones.SetFocusedRowCellValue(dgvDataExtraciones.FocusedColumn, "test");
                        
            //        }
            //        else if (!Convert.ToBoolean(bgvDataExtraciones.GetFocusedRowCellValue("TieneConcepto")) && bgvDataExtraciones.FocusedColumn.VisibleIndex > 0)
            //        {
            //            DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpFormula = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();

            //            rpFormula.AcceptsReturn = false;
            //            rpFormula.AcceptsTab = false;
            //            rpFormula.AutoHeight = false;
            //            rpFormula.ShowIcon = false;
            //            rpFormula.ValidateOnEnterKey = false;
            //            rpFormula.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpFormula_Closed);
            //            rpFormula.Popup += new System.EventHandler(this.rpFormula_Popup);

            //            //this.gridExtraciones.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpFormula });
            //            e.RepositoryItem = rpFormula;
            //            //colExtra.ColumnEdit = rpFormula;
            //        }
            //    }
            //    //if (e.RowHandle != 10)
            //    //    return;
            //    //else
            //    // 

            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            //}
        }

        private void bgvDataExtraciones_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column == bgvDataExtraciones.FocusedColumn)
                {
                    if (Convert.ToBoolean(bgvDataExtraciones.GetFocusedRowCellValue("TieneConcepto")) && bgvDataExtraciones.FocusedColumn.VisibleIndex > 0)
                    {
                        DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit rpPopUp = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();

                        BindingSource bdsConceptos = new System.Windows.Forms.BindingSource();                       

                        DevExpress.XtraGrid.GridControl rpGrid = new DevExpress.XtraGrid.GridControl();
                        DevExpress.XtraGrid.Views.Grid.GridView rpGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
                        DevExpress.XtraGrid.Columns.GridColumn colIDAPEC = new DevExpress.XtraGrid.Columns.GridColumn();
                        DevExpress.XtraGrid.Columns.GridColumn colIDConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
                        DevExpress.XtraGrid.Columns.GridColumn colConcepto = new DevExpress.XtraGrid.Columns.GridColumn();
                        DevExpress.XtraGrid.Columns.GridColumn colLitro = new DevExpress.XtraGrid.Columns.GridColumn();
                        DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkConcepto = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
                        DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rpLitro = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
                        rpGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { lkConcepto, rpLitro });

                        lkConcepto.AutoHeight = false;
                        //lkConcepto.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
                        lkConcepto.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] { new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Concepto", "Conceptos") });
                        lkConcepto.NullText = "<No tiene Concepto Asignado>";

                        lkConcepto.DataSource = from es in db.ExtracionConceptos
                                                select new { IDConcepto = es.ID, es.Concepto };

                        lkConcepto.DisplayMember = "Concepto";
                        lkConcepto.ValueMember = "IDConcepto";

                        //**Repositorio para digitar la lectura                         
                        rpLitro.AutoHeight = false;
                        rpLitro.MaxValue = new decimal(1000000000000);
                        //rpLitro.MaxValue = new decimal(new int[] { 1000000000, 0, 0, 0 });
                        rpLitro.Buttons.Clear();

                        rpLitro.EditFormat.FormatString = "N3";
                        rpLitro.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpLitro.DisplayFormat.FormatString = "N3";
                        rpLitro.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        rpLitro.EditMask = "N3";

                        //Obtengo el ID del producto y del tanque de la tabla Total Extraxion
                        string item = ((string)(e.Column.Tag));
                        string result = item.Replace("Dif", "");
                        //string input = Regex.Replace(item, "[^0-9]+", string.Empty);
                        string[] llaves = result.Split('-');

                        int IDP = Convert.ToInt32(llaves.ElementAt(0));
                        int IDT = Convert.ToInt32(llaves.ElementAt(1));

                        //dtConcepto.Select("IDProducto = " + Convert.ToString(e.Column.Tag));
                        dtConcepto.DefaultView.RowFilter = "IDProducto = " + Convert.ToString(IDP);
                        bdsConceptos.DataSource = dtConcepto.DefaultView;

                        //bdsConceptos.Filter = "IDProducto = " + Convert.ToString(e.Column.Tag);
                        //Grid
                        rpGrid.MainView = rpGridView;
                        rpGrid.DataSource = bdsConceptos;
                        //GridView
                        rpGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colIDConcepto, colConcepto, colLitro });

                        rpGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                        rpGridView.OptionsView.ShowGroupPanel = false;
                        rpGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
                        rpGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
                        rpGridView.OptionsNavigation.AutoFocusNewRow = true;
                        rpGridView.OptionsView.EnableAppearanceEvenRow = true;
                        rpGridView.OptionsView.EnableAppearanceOddRow = true;
                        rpGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
                        rpGridView.Tag = e.Column.Tag;
                        //rpGridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.rpGridView_FocusedRowChanged);
                        rpGridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.rpGridView_CellValueChanged);
                        rpGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rpGridView_KeyDown);
                        //
                        // ColIDAPEC
                        //
                        colIDConcepto.FieldName = "IDAPEC";
                        colConcepto.Visible = false;
                        //
                        // ColID
                        //
                        colIDConcepto.FieldName = "IDProducto";
                        colConcepto.Visible = false;
                        // 
                        // colConcepto
                        // 
                        colConcepto.Caption = "Concepto";
                        colConcepto.FieldName = "IDConcepto";
                        colConcepto.ColumnEdit = lkConcepto;
                        colConcepto.Visible = true;
                        colConcepto.VisibleIndex = 0;
                        //colConcepto. = 0;
                        // 
                        // colLitro
                        // 
                        colLitro.Caption = "Litros";
                        colLitro.FieldName = "Litros";
                        colLitro.Visible = true;
                        colLitro.VisibleIndex = 1;
                        colLitro.ColumnEdit = rpLitro;
                        colLitro.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colLitro.DisplayFormat.FormatString = "N3";

                        rpGrid.Dock = DockStyle.Fill;
                        DevExpress.XtraEditors.PopupContainerControl popupControl = new DevExpress.XtraEditors.PopupContainerControl();
                        popupControl.Controls.Add(rpGrid);


                        //PopupContainerEdit editor = new PopupContainerEdit();
                        rpPopUp.Properties.PopupControl = popupControl;
                        rpPopUp.Tag = rpGridView;
                        rpPopUp.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpPopUp_Closed);

                        //Controls.Add(editor);

                        e.RepositoryItem = rpPopUp;
                        //dgvDataExtraciones.SetFocusedRowCellValue(dgvDataExtraciones.FocusedColumn, "test");

                    }
                    else if (!Convert.ToBoolean(bgvDataExtraciones.GetFocusedRowCellValue("TieneConcepto")) && bgvDataExtraciones.FocusedColumn.VisibleIndex > 0)
                    {
                        DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpFormula = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();

                        rpFormula.AcceptsReturn = false;
                        rpFormula.AcceptsTab = false;
                        rpFormula.AutoHeight = false;
                        rpFormula.ShowIcon = false;
                        rpFormula.ValidateOnEnterKey = false;
                        rpFormula.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Enter);
                        rpFormula.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpFormula_Closed);
                        rpFormula.Popup += new System.EventHandler(this.rpFormula_Popup);
                        

                        //this.gridExtraciones.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { rpFormula });
                        e.RepositoryItem = rpFormula;
                        //colExtra.ColumnEdit = rpFormula;
                    }
                }
                //if (e.RowHandle != 10)
                //    return;
                //else
                // 

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        /// <summary>
        /// Evento para borrar un registro del grid Concetos en Distribución de Extraxion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView rpGridView = (DevExpress.XtraGrid.Views.Grid.GridView)(sender);

            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }
            if (e.KeyCode == Keys.Delete)
            {

                if (rpGridView.FocusedRowHandle >= 0)
                {
                    int _id = Convert.ToInt32(rpGridView.GetFocusedRowCellValue("IDConcepto"));

                    foreach (DataRow r in dtConcepto.Rows)
                    {
                        if (Convert.ToInt32(r["IDConcepto"]) == _id)
                        {

                            //Entity.PrerequisiteClass p = db.PrerequisiteClasses.Single(o => o.PrerequisiteClassID == _id);
                            //db.PrerequisiteClasses.DeleteOnSubmit(p);
                            dtConcepto.Rows.Remove(r);

                            //db.SubmitChanges();
                            break;
                        }
                    }
                    rpGridView.RefreshData();
                }

            }
        }

        /// <summary>
        /// Asignar valor a la celda seleccionada que contenga concepto de Extraxion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpPopUp_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            PopupContainerEdit rpPopUp = (PopupContainerEdit)(sender);
            PopupContainerControl popupControl = rpPopUp.Properties.PopupControl;
            DevExpress.XtraGrid.GridControl rpGrid = popupControl.Controls[0] as DevExpress.XtraGrid.GridControl;
            DevExpress.XtraGrid.Views.Grid.GridView rpGridView = (DevExpress.XtraGrid.Views.Grid.GridView)(rpGrid.MainView);
            //DevExpress.XtraGrid.Views.Grid.GridView rpGridView = ((DevExpress.XtraGrid.Views.Grid.GridView)((DevExpress.XtraGrid. RepositoryItemPopupContainerEdit)sender).Tag);
             
             decimal valor = Decimal.Round(Convert.ToDecimal(rpGridView.Columns["Litros"].Summary.Add(DevExpress.Data.SummaryItemType.Sum).SummaryValue), 3);

            if (valor > 0)
             bgvDataExtraciones.SetFocusedRowCellValue(bgvDataExtraciones.FocusedColumn, valor);
            else
                bgvDataExtraciones.SetFocusedRowCellValue(bgvDataExtraciones.FocusedColumn, "");

            bgvDataExtraciones.RefreshData();
            TotalExtraxionVenta();                        
        }

        /// <summary>
        /// Asignar el ID del Producto al Concepto Extraxion cuando la el valor de la celda sea cambiada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpGridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView rpGridView = (DevExpress.XtraGrid.Views.Grid.GridView)(sender);

                if (e.Column == rpGridView.Columns["Litros"])
                {
                    //Obtengo el ID del producto y del tanque de la tabla Total Extraxion
                    string item = ((string)(rpGridView.Tag));
                    string result = item.Replace("Dif", "");
                    //string input = Regex.Replace(item, "[^0-9]+", string.Empty);
                    string[] llaves = result.Split('-');

                    int IDP = Convert.ToInt32(llaves.ElementAt(0));
                    int IDT = Convert.ToInt32(llaves.ElementAt(1));

                    rpGridView.SetFocusedRowCellValue("IDProducto", Convert.ToInt32(IDP));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        /// <summary>
        /// Cerrar el PopUp y asignar monto a la columna "Valor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpFormula_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();

                int IDExtra = Convert.ToInt32(bgvDataExtraciones.GetFocusedRowCellValue("IDExtraxion"));
                string Valor = Convert.ToString(bgvDataExtraciones.FocusedColumn.FieldName);
                string Formula = "Formula" + Convert.ToString(bgvDataExtraciones.FocusedColumn.Tag) + (Valor.Contains("Dif") ? "Dif" : "");

                //Buscar fila del IDExtraccion
                DataRow[] Rows = dtExtracciones.Select("IDExtraxion = " + IDExtra);

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");
                                                                  
                    Rows[0][Formula] = Operacion;

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion) * (Convert.ToDecimal(Rows[0]["EquivalenteLitros"]) > 0 ? Convert.ToDecimal(Rows[0]["EquivalenteLitros"]) : 1), 3, MidpointRounding.AwayFromZero);
                        //decimal EquivalenteLitros = Convert.ToDecimal(Rows[0]["EquivalenteLitros"]) > 0 ? Convert.ToDecimal(Rows[0]["EquivalenteLitros"]) : 1;
                        
                        Rows[0][Valor] = val;

                        memo.EditValue = val.ToString("#,0.000");
                        bgvDataExtraciones.RefreshData();
                        TotalExtraxionVenta();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    Rows[0][Valor] = "";
                    Rows[0][Formula] = "";
                    bgvDataExtraciones.RefreshData();
                    TotalExtraxionVenta();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        /// <summary>
        /// Abrir el PopUp y asignar una operación a la columna "Formula"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpFormula_Popup(object sender, EventArgs e)
        {
            try
            {
                if (bgvDataExtraciones.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;

                    //Valor para el ID de la Distribucion de Extraccion
                    int IDExtra = Convert.ToInt32(bgvDataExtraciones.GetFocusedRowCellValue("IDExtraxion"));
                    string Valor = Convert.ToString(bgvDataExtraciones.FocusedColumn.FieldName);
                    string Formula = "Formula" + Convert.ToString(bgvDataExtraciones.FocusedColumn.Tag) + (Valor.Contains("Dif") ? "Dif" : "");

                    //Buscar fila del IDExtraccion
                    DataRow[] Rows = dtExtracciones.Select("IDExtraxion = " + IDExtra);

                    me.Text = Convert.ToString(Rows[0][Formula]);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void dgvDataTotalExtraxion_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.ColumnHandle > 0)
            {
                if (e.RowHandle <= 0)
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }                
            }
        }

        #endregion

        #region <<< Ventas >>>

        private void dgvVentas_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.ColumnHandle > 0)
            {
                if (e.RowHandle <= 0)
                {
                    e.DisplayText = Decimal.Round(Convert.ToDecimal(e.CellValue), 3).ToString("#,0.000");
                    e.Appearance.BackColor = Color.AliceBlue;
                }
                else if (e.RowHandle > 0)
                {
                    e.DisplayText = Decimal.Round(Convert.ToDecimal(e.CellValue), 2).ToString("#,0.00");
                    e.Appearance.BackColor = Color.AliceBlue;

                    if (e.RowHandle == 3)
                        e.Appearance.BackColor = Color.LightGreen;
                }
            }
        }

        private void txtTotalVentas_EditValueChanged(object sender, EventArgs e)
        {
            txtVentasContado.Text = Decimal.Round(String.IsNullOrEmpty(txtTotalVentas.Text) ? 0m : Convert.ToDecimal(txtTotalVentas.Text) - (String.IsNullOrEmpty(bgvDataPago.Columns["ValorPago"].SummaryText) ? 0m : Convert.ToDecimal(bgvDataPago.Columns["ValorPago"].SummaryText)) - Convert.ToDecimal(String.IsNullOrEmpty(txtDescuentoTotal.Text) ? 0m : Convert.ToDecimal(txtDescuentoTotal.Text)), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");

            //txtVentasContado.Text = Decimal.Round(String.IsNullOrEmpty(txtTotalVentas.Text) ? 0m : Convert.ToDecimal(txtTotalVentas.Text) - Convert.ToDecimal(bgvDataPago.Columns["ValorPago"].SummaryText) - Convert.ToDecimal(String.IsNullOrEmpty(txtDescuentoTotal.Text) ? 0m : Convert.ToDecimal(txtDescuentoTotal.Text)), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }

        #endregion

        #region <<< FormasPago >>>

        private void rpMemoPago_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                int IDPago = Convert.ToInt32(bgvDataPago.GetFocusedRowCellValue("IDPago"));
                DataRow[] Rows = dtPago.Select("IDPago = " + IDPago);

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    
                    //string Valor = Convert.ToString(dgvDataExtraciones.FocusedColumn.FieldName);
                    //string Formula = "Formula" + Convert.ToString(dgvDataExtraciones.FocusedColumn.FieldName).Substring(dgvDataExtraciones.FocusedColumn.FieldName.ToString().Length - 1);

                    //Buscar fila del IDExtraccion                            

                    Rows[0]["FormulaPago"] = Operacion;

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = ValorFormula(Operacion);

                        Rows[0]["ValorPago"] = val;

                        memo.EditValue = val.ToString("#,0.00");
                        bgvDataExtraciones.RefreshData();
                        TotalExtraxionVenta();
                    }
                    
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    Rows[0]["ValorPago"] = "";
                    Rows[0]["FormulaPago"] = "";
                }

                bgvDataPago.RefreshData();

                txtTotalVentas_EditValueChanged(null, null);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoPago_Popup(object sender, EventArgs e)
        {
            try
            {
                if (bgvDataPago.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;

                    //Valor para el ID de la Distribucion de Extraccion
                    int IDPago = Convert.ToInt32(bgvDataPago.GetFocusedRowCellValue("IDPago"));
                    //string Valor = Convert.ToString(dgvDataExtraciones.FocusedColumn.FieldName);
                    //string Formula = "Formula" + Convert.ToString(dgvDataExtraciones.FocusedColumn.FieldName).Substring(dgvDataExtraciones.FocusedColumn.FieldName.ToString().Length - 1);

                    //Buscar fila del IDExtraccion
                    DataRow[] Rows = dtPago.Select("IDPago = " + IDPago);

                    me.Text = Convert.ToString(Rows[0]["FormulaPago"]);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region <<< Descuento >>>

        private void bgvDataDescuento_ShowingEditor(object sender, CancelEventArgs e)
        {   
            //desabilitar edicion para las filas del Grid
            if (bgvDataDescuento.FocusedRowHandle.Equals(0) || bgvDataDescuento.FocusedRowHandle.Equals(2) || bgvDataDescuento.FocusedRowHandle.Equals(3))
                e.Cancel = true;
            
        }

        private void bgvDataDescuento_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.ColumnHandle > 1)
                {
                    if (e.CellValue != DBNull.Value)
                    {
                        if (e.RowHandle.Equals(0) || e.RowHandle.Equals(3))
                        {
                            e.DisplayText = Decimal.Round(Convert.ToDecimal(e.CellValue), 2).ToString("#,0.00");
                            e.Appearance.BackColor = Color.AliceBlue;
                        }
                        else if (e.RowHandle.Equals(2))
                            e.DisplayText = Decimal.Round(Convert.ToDecimal(e.CellValue), 2).ToString("#,0.000");
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
            //else
            //{
            //    DevExpress.Utils.SuperToolTip stt_ToolTip = new DevExpress.Utils.SuperToolTip();

            //    DevExpress.XtraGrid.Views.Base.GridCell celda = ((DevExpress.XtraGrid.Views.Base.GridCell)(e.Cell));
            //    DevExpress.XtraGrid.Views.Base bgvDataExtraciones.GetRow(e.RowHandle)
            //        e.
            //}
        }

        private void bgvDataDescuento_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Detectar la fila de DescuentoEspecial para ingresarla a la Entidad ArqueoProducto
            if (e.RowHandle.Equals(1))
            {

                EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.Equals((Entidad.ArqueoProducto)(e.Column.Tag)));
                decimal valor = 0m;
                valor = Decimal.TryParse(e.Value.ToString(), out valor) ? valor : 0m;
                EtArqueoProducto.DescuentoEspecial = valor;
            }

            bgvDataDescuento.RefreshData();

            decimal suma = 0.00m;

            

            for (int i = 2; i < bgvDataDescuento.Columns.Count; i++)
            {
                decimal calc = 0m;
                suma += Decimal.TryParse(bgvDataDescuento.Columns[i].SummaryText, out calc) ? calc : 0;
                txtDescuentoTotal.Text = Decimal.Round(suma, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
            }
        }

        private void bgvDataDescuento_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column.Equals(bgvDataDescuento.FocusedColumn) && e.RowHandle.Equals(2))
                {
                    /*DESHABILITADO TEMPORALMENTE//////
                    DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit rpDescFormula = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();

                    rpDescFormula.AcceptsReturn = false;
                    rpDescFormula.AcceptsTab = false;
                    rpDescFormula.AutoHeight = false;
                    rpDescFormula.ShowIcon = false;
                    rpDescFormula.ValidateOnEnterKey = false;
                    rpDescFormula.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.Enter);
                    rpDescFormula.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.rpDescFormula_Closed);
                    rpDescFormula.Popup += new System.EventHandler(this.rpDescFormula_Popup);

                    e.RepositoryItem = rpDescFormula;
                    **********/

                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }

        }

        /// <summary>
        /// Cerrar el PopUp y asignar monto a la columna "Valor"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpDescFormula_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = "";

                if (me.EditValue != null)
                {
                   Operacion = String.IsNullOrEmpty(me.EditValue.ToString()) ? "" : me.EditValue.ToString();                   
                }

                EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.Equals((Entidad.ArqueoProducto)(bgvDataDescuento.FocusedColumn.Tag)));
                
                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    EtArqueoProducto.DescuentoGalonesPetroCardFormula = Operacion;

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 3, MidpointRounding.AwayFromZero); 
                        
                        EtArqueoProducto.DescuentoGalonesPetroCardValor = val * DescuentoPetroCard;
                        bgvDataDescuento.SetRowCellValue(3, bgvDataDescuento.FocusedColumn, val * DescuentoPetroCard);

                        memo.EditValue = val.ToString("#,0.000");
                        bgvDataDescuento.RefreshData();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    bgvDataDescuento.SetRowCellValue(2, bgvDataDescuento.FocusedColumn, 0.00m);
                    bgvDataDescuento.SetRowCellValue(3, bgvDataDescuento.FocusedColumn, 0.00m);
                    EtArqueoProducto.DescuentoGalonesPetroCardValor = 0m;
                    EtArqueoProducto.DescuentoGalonesPetroCardFormula = "";
                    bgvDataDescuento.RefreshData();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        /// <summary>
        /// Abrir el PopUp y asignar una operación a la columna "Formula"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpDescFormula_Popup(object sender, EventArgs e)
        {
            try
            {
                if (bgvDataDescuento.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;

                    //Valor para el ID del Arqueo Producto
                    EtArqueoProducto = ArqueoSelected.ArqueoProductos.Single(ap => ap.Equals((Entidad.ArqueoProducto)(bgvDataDescuento.FocusedColumn.Tag)));
                    me.Text = Convert.ToString(EtArqueoProducto.DescuentoGalonesPetroCardFormula);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void bgvDataDescuento_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            DevExpress.XtraGrid.GridSummaryItem obj = ((DevExpress.XtraGrid.GridSummaryItem)e.Item);

            if (!obj.FieldName.Equals("TipoDescuento"))
            {
                decimal total = 0m;
                for (int i = 0; i < bgvDataDescuento.RowCount; i++)
                {
                    if (!i.Equals(2))
                    {
                        if (bgvDataDescuento.GetRowCellValue(i, obj.FieldName) != DBNull.Value)
                            total += Convert.ToDecimal(bgvDataDescuento.GetRowCellValue(i, obj.FieldName));
                    }
                }

                e.TotalValue = total;
            }
        }

        private void toolTipControllerCelda_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl.Equals(gridDescuento))
            {
                ToolTipControlInfo info = null;

                //Obtiene la vista de la posicion del mouse
                GridView view = gridDescuento.GetViewAt(e.ControlMousePosition) as GridView;
                if (view == null) return;
                //Obtengo el elemento de la vista
                GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
                //Muestra el indicador de la celda
                if (hi.InRowCell && hi.RowHandle.Equals(2))
                {
                    //Obtiene la celda especifica
                    string cellKey = hi.RowHandle.ToString() + " - " + hi.Column.ToString();
                    info = new ToolTipControlInfo(cellKey, "Esta fila no aplica a la suma de Descuentos Totales");
                }

                if (info != null)
                    e.Info = info;

            }
        }

        #endregion

        #region <<< PagoEfectivo >>>

        private void rpMemoCor_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal TotalEfectivo = 0m;

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    //Buscar fila del IDExtraccion
                    //me.Text = Convert.ToString(dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.FieldName]);

                    dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.Tag.ToString()] = Operacion;
                    

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = bgvDataPagoEfectivo.FocusedColumn == colValorUS ? Decimal.Round(ValorFormula(Operacion) * TipoCambioArqueo, 2, MidpointRounding.AwayFromZero) : ValorFormula(Operacion);

                        if (bgvDataPagoEfectivo.FocusedColumn == colValorUS)
                            dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.FieldName] = val;
                        else
                            dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.FieldName] = val;

                        memo.EditValue = val.ToString("#,0.00");
                        bgvDataPagoEfectivo.RefreshData();                        

                        
                    }
                    
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.Tag.ToString()] = "";
                    dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.FieldName] = "";
                }

                foreach (DataColumn r in dtPagoEfectivo.Columns)
                        {
                            if (r.ColumnName.Contains("Valor"))
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(dtPagoEfectivo.Rows[0][r])))
                                {
                                TotalEfectivo += Convert.ToDecimal(dtPagoEfectivo.Rows[0][r]);
                                }
                            }
                        }
                txtSubTotalEfectivoRecibido.EditValue = Decimal.Round(Convert.ToDecimal(TotalEfectivo), 2, MidpointRounding.AwayFromZero).ToString("#,0.00");    

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoCor_Popup(object sender, EventArgs e)
        {
            try
            {
                if (bgvDataPagoEfectivo.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                    me.Text = Convert.ToString(dtPagoEfectivo.Rows[0][bgvDataPagoEfectivo.FocusedColumn.Tag.ToString()]);
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void txtEfectivoRecibido_EditValueChanged(object sender, EventArgs e)
        {
            decimal suma = txtSubTotalEfectivoRecibido.EditValue != null ? Convert.ToDecimal(txtSubTotalEfectivoRecibido.EditValue) : 0m;
            decimal resta = txtVentasContado.EditValue != null ? Convert.ToDecimal(txtVentasContado.EditValue) : 0m;

            _GrandSobranteDiferencia = Decimal.Round((suma - resta), 2, MidpointRounding.AwayFromZero);

            if (_GrandSobranteDiferencia > 0)
            {
                elblDiferencia.Text = "Sobrante en Arqueo:      " + _GrandSobranteDiferencia.ToString("#,0.00");
                elblDiferencia.AppearanceItemCaption.ForeColor = Color.White;
                elblDiferencia.AppearanceItemCaption.BackColor = Color.DodgerBlue;
                spDiferenciaRecibida.Properties.ReadOnly = true;
                spDiferenciaRecibida.Value = 0m;
                elblPostDiferencia.Text = "Sin Diferencias ";
                elblPostDiferencia.AppearanceItemCaption.BackColor = Color.Transparent;
                elblPostDiferencia.AppearanceItemCaption.ForeColor = Color.Black;

            }
            else if (_GrandSobranteDiferencia < 0)
            {
                elblDiferencia.Text = "Faltante en Arqueo:      " + _GrandSobranteDiferencia.ToString("#,0.00");
                elblDiferencia.AppearanceItemCaption.ForeColor = Color.White;
                elblDiferencia.AppearanceItemCaption.BackColor = Color.Red;
                spDiferenciaRecibida.Properties.ReadOnly = false;
            }
            else if (_GrandSobranteDiferencia == 0)
            {
                elblDiferencia.Text = "Sin Diferencias:      " + _GrandSobranteDiferencia.ToString("#,0.00");
                elblDiferencia.AppearanceItemCaption.ForeColor = Color.Black;
                elblDiferencia.AppearanceItemCaption.BackColor = Color.LimeGreen;
                spDiferenciaRecibida.Properties.ReadOnly = true;
                spDiferenciaRecibida.Value = 0m;
                elblPostDiferencia.Text = "Sin Diferencias";
                elblPostDiferencia.AppearanceItemCaption.BackColor = Color.Transparent;
                elblPostDiferencia.AppearanceItemCaption.ForeColor = Color.Black;
            }

            if (btnOK.Enabled.Equals(false))
                btnOK.Enabled = true;

            txtTotalEfectivoRecibido.Text = Convert.ToDecimal(suma).ToString("#,0.00");
        }

        private void spDiferenciaRecibida_EditValueChanged(object sender, EventArgs e)
        {
            if (spDiferenciaRecibida.Value != null)
            {
                decimal suma = txtSubTotalEfectivoRecibido.EditValue != null ? Convert.ToDecimal(txtSubTotalEfectivoRecibido.EditValue) : 0m;
                decimal diferenciaRecibida = spDiferenciaRecibida.Value;
                decimal resta = txtVentasContado.EditValue != null ? Convert.ToDecimal(txtVentasContado.EditValue) : 0m;

                decimal diferencia = Decimal.Round(diferenciaRecibida + (suma - resta), 2, MidpointRounding.AwayFromZero);

                if (diferencia > 0)
                {
                    elblPostDiferencia.Text = "Sobrante en Diferencia:      " + diferencia.ToString("#,0.00");
                    elblPostDiferencia.AppearanceItemCaption.ForeColor = Color.White;
                    elblPostDiferencia.AppearanceItemCaption.BackColor = Color.DodgerBlue;
                }
                else if (diferencia < 0)
                {
                    elblPostDiferencia.Text = "Faltante en Diferencia:      " + diferencia.ToString("#,0.00");
                    elblPostDiferencia.AppearanceItemCaption.ForeColor = Color.White;
                    elblPostDiferencia.AppearanceItemCaption.BackColor = Color.Red;
                }
                else if (diferencia == 0)
                {
                    elblPostDiferencia.Text = "Sin Diferencias:      " + diferencia.ToString("#,0.00");
                    elblPostDiferencia.AppearanceItemCaption.ForeColor = Color.Black;
                    elblPostDiferencia.AppearanceItemCaption.BackColor = Color.LimeGreen;
                }

                if (btnOK.Enabled.Equals(false))
                    btnOK.Enabled = true;

                txtTotalEfectivoRecibido.Text = Convert.ToDecimal(suma + diferenciaRecibida).ToString("#,0.00");
            }
        }

        #endregion

        #region <<< MouseWheel >>>

        private void bgvDataExtraciones_MouseWheel(object sender, MouseEventArgs e)
        {
            // If the mouse wheel delta is positive, move the box up. 
            if (e.Delta > 0)
            {
                this.xtraScrollableControl.VerticalScroll.Value -= 20;
            }

            // If the mouse wheel delta is negative, move the box down. 
            if (e.Delta < 0)
            {
                this.xtraScrollableControl.VerticalScroll.Value += 20;
            }


        }

        #endregion  

        private void txtObservacion_EditValueChanged(object sender, EventArgs e)
        {
            if (btnOK.Enabled.Equals(false))
                btnOK.Enabled = true;
        }

        private void lkTecnico_EditValueChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = true;
        }    
            
        #endregion        

        private void chkArqueoEspecial_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkArqueoEspecial.Checked)
                {
                    layoutControlGroup1.AppearanceItemCaption.BackColor = Color.Red;
                    chkArqueoEspecial.BackColor = Color.Red;
                }
                else
                {
                    layoutControlGroup1.AppearanceItemCaption.BackColor = Color.Transparent;
                    chkArqueoEspecial.BackColor = Color.Transparent;
                }
            }
            catch
            {
                layoutControlGroup1.AppearanceItemCaption.BackColor = Color.Transparent;
                chkArqueoEspecial.BackColor = Color.Transparent;
            }
        }

        
    }
}