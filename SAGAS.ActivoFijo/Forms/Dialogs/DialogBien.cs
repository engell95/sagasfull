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
using SAGAS.ActivoFijo.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogBien : Form
    { 
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormBien MDI;
        private List<ObjectListTipoMovimiento> ListTipoMovimiento = new List<ObjectListTipoMovimiento>();
        private List<ObjectListProveedor> ListProveedor = new List<ObjectListProveedor>();
        internal static ObjectListProveedor Provee; 
        private List<ObjectListCliente> ListCliente = new List<ObjectListCliente>();
        private List<ObjectListEstacion> ListEstacion = new List<ObjectListEstacion>();
        private List<ObjectListSubEstacion> ListSubEstacion = new List<ObjectListSubEstacion>();
        internal Entidad.Movimiento EntidadAnterior;
        internal int _MovimientoAltaActivoSinContabilidadID = 0;
        internal bool Editable = false;
        internal bool ShowMsg;
        internal decimal _TipoCambio;
        internal Entidad.TipoMovimientoActivo EtTipoMovimientoActivo;
        private int UsuarioID;        
        private bool NextBien = false;
        private bool RefreshMDI = false;
        private bool _Guardado = false;
        private bool _ToPrint = false;
        private int IDMonedaPrincipal;
        private int IDPrint = 0;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private int _CuentaIVACredito = Parametros.Config.IVAPorAcreditar();
        internal decimal vTotalPagar = 0m;
        internal int _MonedaPrimaria;
        internal int _MonedaSecundaria;
        private List<ListadosAcitvos> EtActivos;
        private List<ListadosTiposAcitvos> EtTipoActivos;        

        struct ListadosAcitvos
        {
            public int ID;
            public string Nombre;
            public int IDTipo;
        }

        struct ListadosTiposAcitvos
        {
            public int ID;
            public string Nombre;
            public bool EsDepreciable;
            public int CuentaActivo;
            public int VidaUtil;
        }

        private int Tipo
        {
            get { return radioTipo.SelectedIndex; }
            set { radioTipo.SelectedIndex = value; }    
        
        }

        private int IDTipoMovimiento
        {
            get { return Convert.ToInt32(glkTipoMovimiento.EditValue); }
            set { glkTipoMovimiento.EditValue = value; }
        }

        private int IDEstacionServicio
        {
            get { return Convert.ToInt32(lkEs.EditValue); }
            set { lkEs.EditValue = value; }
        }

        private int IDSubEstacion
        {
            get { return Convert.ToInt32(lkSus.EditValue); }
            set { lkSus.EditValue = value; }
        }

        private int IDSujeto
        {
            get { return Convert.ToInt32(glkProvClientEstacion.EditValue); }
            set { glkProvClientEstacion.EditValue = value; }
        }

        private DateTime FechaAdquisicion
        {
            get { return Convert.ToDateTime(dateFechaAdquisicion.EditValue); }
            set { dateFechaAdquisicion.EditValue = value; }
        }

        private string NoFactura 
        {
            get { return txtNoFactura.Text; }
            set { txtNoFactura.Text = value; }
        
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }
        
        
        //{
        //    get { return Convert.ToInt32(lkMoneda.EditValue); }
        //    set { lkMoneda.EditValue = value; }
        //}
                       
        private List<Entidad.Bien> B = new List<Entidad.Bien>();
        public List<Entidad.Bien> DetalleB
        {
            get { return B; }
            set
            {
                B = value;
                this.bdsDetalle.DataSource = this.B;
            }
        }

        #endregion

 
        public DialogBien(int UserID)
        {
            InitializeComponent();
            UsuarioID = UserID;

        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            
        }

        void FillObjectListTipoMovimiento()
        {
            //TIPOMOVIMIENTO
            ListTipoMovimiento = (from tipomov in db.TipoMovimientoActivo.Where(d => d.Activo)
                                  select new ObjectListTipoMovimiento
                                  {
                                      ID = tipomov.ID,
                                      Nombre = tipomov.Nombre,
                                      AplicaDepreciacion = tipomov.AplicaDepreciacion,
                                      AplicaProveedor = tipomov.AplicaProveedor,
                                      AplicaCliente = tipomov.AplicaCliente,
                                      EsAlta = tipomov.EsAlta,
                                      EsBaja = tipomov.EsBaja,
                                      EsTraslado = tipomov.EsTraslado,
                                  }).ToList();
        }

        void FillObjectListProveedor()
        {
            //PROVEEDOR
            ListProveedor = (from proveedor in db.Proveedors.Where(d => d.Activo)
                             select new ObjectListProveedor
                             {
                                 ID = proveedor.ID,
                                 Codigo = proveedor.Codigo,
                                 Nombre = proveedor.Nombre,
                                 Display = proveedor.Codigo + " | " + proveedor.Nombre,
                                 Ruc = proveedor.RUC,
                                 Telefono = proveedor.Telefono1,
                                 Direccion = proveedor.Direccion,
                                 Cuentacontable = proveedor.CuentaContableID,
                                 IVA = proveedor.AplicaIVA
                             }).ToList();
        }

        void FillObjectListCliente()
        {
            //CLIENTE
            ListCliente = (from cliente in db.Clientes.Where(d => d.Activo)
                           select new ObjectListCliente
                           {
                               ID = cliente.ID,
                               Codigo = cliente.Codigo,
                               Nombre = cliente.Nombre,
                               Display = cliente.Codigo + " | " + cliente.Nombre,
                               Ruc = cliente.RUC,
                               Telefono = cliente.Telefono1,
                               Direccion = cliente.Direccion,
                               Cuentacontable = cliente.CuentaContableID,
                           }).ToList();
        }

        void FillObjectListEstacion()
        {
            //ESTACION
            ListEstacion = (from estacion in db.EstacionServicios.Where(d => d.Activo)
                            select new ObjectListEstacion
                            {
                                ID = estacion.ID,
                                Codigo = estacion.Codigo,
                                Nombre = estacion.Nombre,
                                Display = estacion.Codigo + " | " + estacion.Nombre,
                            }).OrderBy(o => o.Codigo).ToList();
        }

        void FillObjectListSubEstacion()
        {
            //SUBESTACION
            ListSubEstacion = (from subestacion in db.SubEstacions.Where(d => d.Activo)
                               select new ObjectListSubEstacion
                               {
                                   StacionID = subestacion.EstacionServicioID,
                                   SubestacionID = subestacion.ID,
                                   Codigo = subestacion.Codigo,
                                   Nombre = subestacion.Nombre,
                                   Display = subestacion.Codigo + " | " + subestacion.Nombre,
                               }).ToList();
        }

        private void DialogCompras_Shown(object sender, EventArgs e)
        {
            FillControl();
            ValidarerrRequiredField();

            //glkTipoMovimiento.Properties.DisplayMember = "Nombre";
            //glkTipoMovimiento.Properties.ValueMember = "ID";

            //glkProvClientEstacion.Properties.DisplayMember = "Display";
            //glkProvClientEstacion.Properties.ValueMember = "ID";

            //lkMoneda.Properties.DisplayMember = "Display";
            //lkMoneda.Properties.ValueMember = "ID";

            //gridCuenta.DisplayMember = "Codigo";
            //gridCuenta.ValueMember = "ID";            
        }




        #region *** METODOS ***

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






        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                Tipo = 0;
                
                //TIPO DE MOVIMIENTO
                FillObjectListTipoMovimiento();
                //PROVEEDOR
                FillObjectListProveedor();
                //CLIENTE
                FillObjectListCliente();
                //ESTACION
                FillObjectListEstacion();
                
                _MonedaPrimaria = Parametros.Config.MonedaPrincipal();
                _MonedaSecundaria = Parametros.Config.MonedaSecundaria();
                _MovimientoAltaActivoSinContabilidadID = Parametros.Config.MovimientoAltaActivoSinContabilidadID();
                IDMonedaPrincipal = _MonedaPrimaria;

                //REPOSITORIOS DEL GRID
                //ACTIVOS...
                EtActivos = db.Activo.Where(o => o.Activado).Select(s => new ListadosAcitvos {ID = s.ID, Nombre = s.Nombre, IDTipo = s.TipoActivoID }).ToList();
                rpLkActivo.DataSource = EtActivos.Select(s => new { s.ID, s.Nombre, s.IDTipo }).ToList();
                
                //TIPOS ACTIVOS
                EtTipoActivos = db.TipoActivo.Where(o => o.Activo).Select(s => new ListadosTiposAcitvos { ID = s.ID, Nombre = s.Nombre, EsDepreciable = s.EsDepreciable, CuentaActivo = s.CuentaActivo, VidaUtil = s.VidaUtil }).ToList();
                rpLkTipoActivo.DataSource = EtTipoActivos.Select(s => new { s.ID, s.Nombre, s.EsDepreciable }).ToList();
                
                //ESTACIONES
                lkEs.Properties.DataSource = ListEstacion.Select(s => new { s.ID, s.Nombre }).ToList();

                //Areas <> CECOS
                rpLkAreaID.DataSource = db.CentroCostos.Where(c => c.Activo).Select(s => new { s.ID, s.Nombre });

                glkTipoMovimiento.EditValue = null;
                glkTipoMovimiento.Properties.DataSource = ListTipoMovimiento.Where(d => d.EsAlta);

                this.bdsDetalle.DataSource = DetalleB;

                if (Editable)
                {
                    var objTipoMov = ListTipoMovimiento.FirstOrDefault(lm => lm.ID.Equals(EntidadAnterior.TipoMovimientoActivoID));

                    if (objTipoMov != null)
                    {
                        if (objTipoMov.EsAlta)
                        {
                            radioTipo.SelectedIndex = 0;
                            radioTipo.Properties.ReadOnly = true;
                        }

                        if (objTipoMov.EsBaja)
                        {
                            radioTipo.SelectedIndex = 1;
                            radioTipo.Properties.ReadOnly = true;
                        }

                        if (objTipoMov.EsTraslado)
                        {
                            radioTipo.SelectedIndex = 2;
                            radioTipo.Properties.ReadOnly = true;
                        }
                        
                        IDTipoMovimiento = EntidadAnterior.TipoMovimientoActivoID;
                        
                    }

                    Comentario = EntidadAnterior.Comentario;
                    FechaAdquisicion = EntidadAnterior.FechaRegistro;
                    dateFechaCompra_Validated(dateFechaAdquisicion, null);
                    NoFactura = EntidadAnterior.Referencia;
                    spSub.Value = EntidadAnterior.SubTotal;
                    IDSujeto = EntidadAnterior.ProveedorID;

                    if (EntidadAnterior.IVA > 0)
                    {
                        lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        chkAplicaIva.Checked = true;
                        spIva.Value = EntidadAnterior.IVA;
                    }

                    IDEstacionServicio = EntidadAnterior.EstacionServicioID;
                    IDSubEstacion = EntidadAnterior.SubEstacionID;

                    DetalleB.AddRange(db.Bien.Where(b => b.MovimientoAltaID.Equals(EntidadAnterior.ID)).ToList());
                    gvBien.RefreshData();
                    gvBien.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
                    gvBien.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.None;
                    glkTipoMovimiento.Properties.ReadOnly = true;
                    glkProvClientEstacion.Properties.ReadOnly = true;

                }

                Parametros.General.splashScreenManagerMain.CloseWaitForm(); 
                                          
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtNoFactura, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(glkProvClientEstacion, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(spTotal, errRequiredField);
        }

        private bool ValidarReferencia(string code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where  (ID.HasValue ? i.Referencia.Equals(code) && !i.ID.Equals(ID) && i.ProveedorID.Equals(IDSujeto) && !i.Anulado : i.Referencia.Equals(code) && i.ProveedorID.Equals(IDSujeto) && !i.Anulado)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidarCodigo(string code, int? ID)
        {
            var result = (from i in db.Proveedors
                          where (ID.HasValue ? i.Codigo == code && i.ID != Convert.ToInt32(ID) : i.Codigo == code)
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }

        public bool ValidarCampos(bool detalle)
        {
            if (dateFechaAdquisicion.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtNoFactura.Text == "" || spTotal.Value.Equals(0))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado del documento.", Parametros.MsgType.warning);
                return false;
            }

            if (lblProvClientEstacion.Visibility.Equals(DevExpress.XtraLayout.Utils.LayoutVisibility.Always))
            {
                if (IDSujeto <= 0)
                {
                    Parametros.General.DialogMsg("Debe seleccionar un " + lblProvClientEstacion.Text, Parametros.MsgType.warning);
                    return false;
                }
            }


            if (!Parametros.General.ValidateTipoCambio(dateFechaAdquisicion, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            DateTime vFechaV = new DateTime(FechaAdquisicion.Year, FechaAdquisicion.Month, 1).AddMonths(1).AddDays(-1);

            DateTime FechaAnterior = new DateTime(vFechaV.Year, vFechaV.Month, 01);

            if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(IDEstacionServicio) && (m.FechaRegistro.Date > FechaAnterior.Date && m.FechaRegistro.Date <= vFechaV.Date)) >= 1)
            {
                Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo", Parametros.MsgType.warning);
                return false;
            }
                       

            if (!Parametros.General.ValidatePeriodoContable(FechaAdquisicion, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                return false;
            }

           
            if (FechaAdquisicion.Date > Convert.ToDateTime(db.GetDateServer()).Date)
            {
                Parametros.General.DialogMsg("La fecha de recibido o de factura no puede ser mayor a la fecha actual del calendario.", Parametros.MsgType.warning);
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

            if (!detalle)
            {
                if (DetalleB.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle del movimiento." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (DetalleB.Count(o => String.IsNullOrEmpty(o.Descripcion )) > 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar la descripción del Bien." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (!ValidarReferencia(Convert.ToString(txtNoFactura.Text), EntidadAnterior == null ? 0 : EntidadAnterior.ID))
                {
                    Parametros.General.DialogMsg("La referencia para este movimiento ya existe : " + Convert.ToString(txtNoFactura.Text), Parametros.MsgType.warning);
                    return false;
                }

                if (layoutControlItemSubEstacion.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
                {
                    if (IDSubEstacion <= 0)
                    {
                        Parametros.General.DialogMsg("Debe seleccionar una Sub Estación", Parametros.MsgType.warning);
                        return false;
                    }
                }

                if (EtTipoMovimientoActivo.AplicaProveedor)
                {
                    if (IDSujeto <= 0)
                    {
                        Parametros.General.DialogMsg("Favor seleccionar un Proveedor", Parametros.MsgType.warning);
                        return false;
                    }
                }

            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (Convert.ToDecimal(DetalleB.Sum(s => s.ValorAdquisicion)).Equals(0))
            {
                if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    return false;
            }

            if (!Convert.ToDecimal(DetalleB.Sum(s => s.ValorAdquisicion)).Equals(Convert.ToDecimal(spSub.Value)))
            {
                Parametros.General.DialogMsg("El subtotal de la factura no es igual al Total contabilizado", Parametros.MsgType.warning);
                return false;
            }

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
                    }
                    else
                    {
                        M = new Entidad.Movimiento();

                        if (radioTipo.SelectedIndex.Equals(0))
                        {
                            M.MovimientoTipoID = 66; //66 Alta o 000 Baja
                            M.ProveedorID = IDSujeto;
                            M.TipoMovimientoActivoID = IDTipoMovimiento;
                        }
                        
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.MonedaID = IDMonedaPrincipal;
                        M.TipoCambio = _TipoCambio;
                    }
                    
                    M.UsuarioID = UsuarioID;
                    
                    M.FechaRegistro = FechaAdquisicion;
                    
                    M.Referencia = NoFactura;
                    M.EstacionServicioID = IDEstacionServicio;
                    M.SubEstacionID = IDSubEstacion;
                    M.Comentario = Comentario;
                    M.SubTotal = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spSub.Value) :
                                Decimal.Round((Convert.ToDecimal(spSub.Value) * M.TipoCambio), 2, MidpointRounding.AwayFromZero));
                    M.IVA = (IDMonedaPrincipal.Equals(_MonedaPrimaria) ? Convert.ToDecimal(spIva.Value) :
                                Decimal.Round((Convert.ToDecimal(spIva.Value) * M.TipoCambio), 2, MidpointRounding.AwayFromZero));

                    M.Monto = Decimal.Round(M.SubTotal + M.IVA, 2, MidpointRounding.AwayFromZero);
                    M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(M.SubTotal + M.IVA) / M.TipoCambio, 2, MidpointRounding.AwayFromZero);

                    if (!Editable)
                        db.Movimientos.InsertOnSubmit(M);

                    db.SubmitChanges();

                    if (radioTipo.SelectedIndex.Equals(0))

                    #region <<< Registrando BIEN >>>
                        foreach (var obj in DetalleB)
                        {
                            //decimal CostoMov = LineaDetalle.Cost;
                            Entidad.Bien EB;

                            if (Editable)
                            {
                                EB = db.Bien.Single(b => b.MovimientoAltaID.Equals(M.ID) && b.ID.Equals(obj.ID));
                            }
                            else
                            {
                                EB = new Entidad.Bien();
                                if (radioTipo.SelectedIndex.Equals(0))
                                    EB.MovimientoAltaID = M.ID;
                            }

                            EB.Codigo = obj.Codigo;
                            EB.NoFactura = NoFactura;
                            EB.Nombre = obj.Nombre;
                            EB.Descripcion = obj.Descripcion;
                            EB.FechaAdquisicion = FechaAdquisicion;
                            EB.ValorAdquisicion = obj.ValorAdquisicion;
                            EB.VidaUtilMeses = EtTipoActivos.Single(s => s.ID.Equals(obj.TipoActivoID)).VidaUtil;
                            EB.UsuarioAsignado = obj.UsuarioAsignado;
                            EB.NoSerie = obj.NoSerie;
                            EB.Marca = obj.Marca;
                            EB.Modelo = obj.Modelo;
                            EB.Matricula = obj.Matricula;
                            EB.Chasis = obj.Chasis;
                            EB.Motor = obj.Motor;
                            EB.Activo = true;
                            EB.EsDepreciado = obj.EsDepreciado;
                            EB.TipoActivoID = obj.TipoActivoID;
                            EB.ActivoID = obj.ActivoID;
                            EB.ProveedorID = IDSujeto;
                            EB.AreaID = obj.AreaID;
                            EB.TipoMovimientoID = IDTipoMovimiento;
                            EB.SubEstacionID = IDSubEstacion;
                            EB.MonedaID = IDMonedaPrincipal;
                            EB.EstacionID = IDEstacionServicio;

                            if (!Editable)
                                db.Bien.InsertOnSubmit(EB);

                            //------------------------------------------------------------------------//
                            //para que actualice los datos del registro
                            db.SubmitChanges();

                    #endregion

                        }


                    #region <<< REGISTRANDO COMPROBANTE >>>
                    if (!IDTipoMovimiento.Equals(_MovimientoAltaActivoSinContabilidadID))
                    {
                        List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                        var objCC = from cds in Compronbante
                                    join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                                    join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                                    select new
                                    {
                                        Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                        Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                                    };

                        if (!(Decimal.Round(objCC.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((objCC.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                        {
                            Parametros.General.splashScreenManagerMain.CloseWaitForm();
                            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCOMPROBANTEDESCUADRADO + Environment.NewLine, Parametros.MsgType.warning);
                            trans.Rollback();
                            return false;
                        }

                        M.ComprobanteContables.Clear();

                        Compronbante.ForEach(linea =>
                            {
                                M.ComprobanteContables.Add(linea);
                                db.SubmitChanges();
                            });

                        db.SubmitChanges();
                    }
                    #endregion

                    //if (!M.Monto.Equals(vTotalPagar))
                    //    M.Monto = Decimal.Round(vTotalPagar, 2, MidpointRounding.AwayFromZero);

                    #region ::: REGISTRANDO DEUDOR :::
                    if (radioTipo.SelectedIndex.Equals(0))
                    {
                        if (IDSujeto > 0)
                        {
                            Entidad.Deudor DL = db.Deudors.Where(d => d.MovimientoID.Equals(M.ID)).FirstOrDefault();

                            if (DL != null)
                            {
                                db.Deudors.DeleteOnSubmit(DL);
                                db.SubmitChanges();
                            }

                            db.Deudors.InsertOnSubmit(new Entidad.Deudor { ProveedorID = IDSujeto, Valor = Decimal.Round(M.Monto, 2, MidpointRounding.AwayFromZero), MovimientoID = M.ID });
                            db.SubmitChanges();
                        }
                    }
                    #endregion

                    if (Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                         "Se modificó el Movimiento de Activo Fijo: " + EntidadAnterior.Referencia, this.Name, dtPosterior, dtAnterior);
                    }
                    else
                    {
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                        "Se registró la Alta de Activo Fijo: " + M.Referencia, this.Name);
                    }


                    db.SubmitChanges();
                    trans.Commit();


                    if (!Editable)
                        NextBien = true;

                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    _ToPrint = true;
                    IDPrint = M.ID;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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
                    int i = 1;
                    decimal _vMP = 0, _vMS = 0;

                    foreach (var linea in DetalleB)
                    {

                        _vMP += linea.ValorAdquisicion;

                        _vMS += Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero);

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = Convert.ToInt32(EtTipoActivos.Single(s => s.ID.Equals(linea.TipoActivoID)).CuentaActivo),
                            Monto = Convert.ToDecimal(linea.ValorAdquisicion),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(linea.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaAdquisicion,
                            Descripcion = Convert.ToString(linea.Descripcion),
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
                    }

                    #endregion

                    decimal _ivaMP = 0, _ivaMS = 0;

                    if (chkAplicaIva.Checked)
                    {
                        _ivaMP = Convert.ToDecimal(spIva.Value);

                        _ivaMS = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spIva.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero);

                        i++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = _CuentaIVACredito,
                            Monto = Convert.ToDecimal(spIva.Value),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(spIva.Value) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                            Fecha = FechaAdquisicion,
                            Descripcion = Provee.Nombre + " – Factura Nro. " + txtNoFactura.Text,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                    }

                    if (EtTipoMovimientoActivo.AplicaProveedor && IDSujeto > 0)
                    {
                        if (Provee != null)
                        {
                            i++;
                            CD.Add(new Entidad.ComprobanteContable
                            {
                                CuentaContableID = Provee.Cuentacontable,
                                Monto = -Math.Abs(Decimal.Round(_vMP + _ivaMP, 2, MidpointRounding.AwayFromZero)),
                                TipoCambio = _TipoCambio,
                                MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_vMS + _ivaMS, 2, MidpointRounding.AwayFromZero)),
                                Fecha = FechaAdquisicion,
                                Descripcion = Provee.Nombre + " – Factura Nro. " + txtNoFactura.Text,
                                Linea = i,
                                EstacionServicioID = IDEstacionServicio,
                                SubEstacionID = IDSubEstacion
                            });
                        }
                    }

                    if (IDSujeto.Equals(0))
                    {
                        i++;
                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = 526,
                            Monto = -Math.Abs(Decimal.Round(_vMP + _ivaMP, 2, MidpointRounding.AwayFromZero)),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = -Math.Abs(Decimal.Round(_vMS + _ivaMS, 2, MidpointRounding.AwayFromZero)),
                            Fecha = FechaAdquisicion,
                            Descripcion = Comentario + " – Factura Nro. " + txtNoFactura.Text,
                            Linea = i,
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                    }

                    vTotalPagar = _vMP + _ivaMP;
                    return CD;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    return null;
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
            MDI.CleanDialog(ShowMsg, NextBien, RefreshMDI, true);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !NextBien)
            {
                if (DetalleB.Count > 0 || txtNoFactura.Text != "" || string.IsNullOrEmpty(mmoComentario.Text))
                {
                    if (Parametros.General.DialogMsg("El Documento actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
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

       

        #region <<< GRID_DETALLES >>>

        //Validar las Filas del detalle
        private void gvDetalle_ValidateRow(object sender, ValidateRowEventArgs e)
        {
            bool Validate = true;
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            int RowHandle = view.FocusedRowHandle;
            
            ////-- Validar Columna de Codigo             
            if (view.GetRowCellValue(RowHandle, "Codigo") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "Codigo")) == 0)
                {
                    view.SetColumnError(view.Columns["Codigo"], "Debe digitar un Codigo");
                    e.ErrorText = "Debe digitar un Codigo";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["Codigo"], "Debe digitar un Codigo");
                e.ErrorText = "Debe digitar un Codigo";
                e.Valid = false;
                Validate = false;
            }

            
            ////-- Validar Columna de Nombre
            ////--
            if (view.GetRowCellValue(RowHandle, "Nombre") == null)
            {
                view.SetColumnError(view.Columns["Nombre"], "Debe de escribir un nombre.");
                e.ErrorText = "Debe de escribir un nombre.";
                e.Valid = false;
                Validate = false;
            }

            
                ////-- Validar Columna de ActivoID             
            if (view.GetRowCellValue(RowHandle, "ActivoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ActivoID")) == 0)
                {
                    view.SetColumnError(view.Columns["ActivoID"], "Debe seleccionar un Activo");
                    e.ErrorText = "Debe seleccionar un Activo";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ActivoID"], "Debe seleccionar un Activo");
                e.ErrorText = "Debe seleccionar un Activo";
                e.Valid = false;
                Validate = false;
            }



            ////-- Validar Columna de TipoActivoID             
            if (view.GetRowCellValue(RowHandle, "TipoActivoID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "TipoActivoID")) == 0)
                {
                    view.SetColumnError(view.Columns["TipoActivoID"], "Debe seleccionar un Tipo de Activo");
                    e.ErrorText = "Debe seleccionar un Tipo de Activo";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["TipoActivoID"], "Debe seleccionar un Tipo de Activo");
                e.ErrorText = "Debe seleccionar un Tipo de Activo";
                e.Valid = false;
                Validate = false;
            }


            ////-- Validar Columna de AreaID             
            if (view.GetRowCellValue(RowHandle, "AreaID") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "AreaID")) == 0)
                {
                    view.SetColumnError(view.Columns["AreaID"], "Debe seleccionar un Área");
                    e.ErrorText = "Debe seleccionar un Área";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["AreaID"], "Debe seleccionar un Área");
                e.ErrorText = "Debe seleccionar un Área";
                e.Valid = false;
                Validate = false;
            }


            ////-- Validar Columna de ValorAdquisicion             
            if (view.GetRowCellValue(RowHandle, "ValorAdquisicion") != null)
            {
                if (Convert.ToInt32(view.GetRowCellValue(RowHandle, "ValorAdquisicion")) < 0)
                {
                    view.SetColumnError(view.Columns["ValorAdquisicion"], "Debe digitar el Valor de Adquisición");
                    e.ErrorText = "Debe digitar el Valor de Adquisición";
                    e.Valid = false;
                    Validate = false;
                }
            }
            else
            {
                view.SetColumnError(view.Columns["ValorAdquisicion"], "Debe digitar el Valor de Adquisición");
                e.ErrorText = "Debe digitar el Valor de Adquisición";
                e.Valid = false;
                Validate = false;
            }

        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column == colActivo)
            {
                try
                {
                    if (gvBien.GetFocusedRowCellValue(colActivo) != null)
                    {
                        if (Convert.ToInt32(gvBien.GetFocusedRowCellValue(colActivo)) > 0)
                        {
                            var obj = EtTipoActivos.Where(s => s.ID.Equals(Convert.ToInt32(EtActivos.Single(sa => sa.ID.Equals(Convert.ToInt32(gvBien.GetFocusedRowCellValue(colActivo)))).IDTipo)));
                            
                            if (obj.Count() > 0)
                            {
                            gvBien.SetFocusedRowCellValue(colTipoActivo, Convert.ToInt32(obj.First().ID));
                            gvBien.SetFocusedRowCellValue(colDepreciacion, Convert.ToBoolean(obj.First().EsDepreciable));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
            
        }
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Editable)
            {

                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }

                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridBien.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                            + view.GetRowCellDisplayText(RowHandle, "ActivoID").ToString() + " | " + view.GetRowCellDisplayText(RowHandle, "Codigo").ToString() + " | " + view.GetRowCellDisplayText(RowHandle, "Nombre").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);
                            
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridBien.DefaultView;
                    view.AddNewRow();
                }
            }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (!Editable)
            {
                if (e.Button.ButtonType == NavigatorButtonType.Remove)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridBien.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                                + view.GetRowCellDisplayText(RowHandle, "ActivoID").ToString() + " | " + view.GetRowCellDisplayText(RowHandle, "Codigo").ToString() + " | " + view.GetRowCellDisplayText(RowHandle, "Nombre").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);


                        }
                    }
                }
            }
        }

        //Validando la asignacion de Centro de Costo
        private void gvDetalle_ShowingEditor(object sender, CancelEventArgs e)
        {
            //if (gvBien.FocusedColumn == colCentroCosto)
            //{
            //    if (gvBien.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
            //        e.Cancel = true;
            //    else
            //    {
            //        if (!Convert.ToBoolean(gvBien.GetFocusedRowCellValue(colUsaCto)))
            //            e.Cancel = true;
            //    }

            //}
        }
        
        #endregion






        //Mostrar el comprobante contable
        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IDTipoMovimiento.Equals(_MovimientoAltaActivoSinContabilidadID))
                {
                    if (!ValidarCampos(true)) return;

                    if (!Convert.ToDecimal(DetalleB.Sum(s => s.ValorAdquisicion)).Equals(Convert.ToDecimal(spSub.Value)))
                    {
                        Parametros.General.DialogMsg("El subtotal de la factura no es igual al Total contabilizado", Parametros.MsgType.warning);
                        return;
                    }

                    if (Convert.ToDecimal(DetalleB.Sum(s => s.ValorAdquisicion)).Equals(0))
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGESCERO, Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                            return;
                    }

                    using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                    {
                        nf.DetalleCD = PartidasContable;
                        nf.Text = "Comprobante Contable Provisión de Gastos";
                        nf.ShowDialog();
                    }
                }
                else
                    Parametros.General.DialogMsg("El Tipo de Movimiento " + glkTipoMovimiento.Text + " no genera contabilidad.", Parametros.MsgType.warning);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFechaVencimiento_EditValueChanged(object sender, EventArgs e)
        {
            txtNombre_Validated_1(sender, null);
        }         

        //Botones para accesos directos
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

            if (this.bntNew.Enabled.Equals(true))
            {
                if (keyData == (Keys.Control | Keys.N))
                {
                    bntNew_Click(null, null);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }        

        //EVENTO REGISTRAR BIENES
        private void bntNew_Click(object sender, EventArgs e)
        {            
                NextBien = true;
                RefreshMDI = false;
                ShowMsg = false;
                this.Close();
        } 
          
        private void gvDetalle_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
           // txtGrandTotal.Text = Convert.ToDecimal(gvBien.Columns["Monto"].SummaryText).ToString("#,0.00");
        }

        private void dateFechaCompra_EditValueChanged(object sender, EventArgs e)
        {
            //if (Provee != null)
            //    FechaVencimiento = FechaAdquisicion.AddDays(Provee.Plazo);
        }  

        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFechaAdquisicion.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(FechaAdquisicion.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(FechaAdquisicion.Date)).First().Valor : 0m);
        }
        
        private void spSub_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spSub.Value) >= 0)
            {
                decimal iva = 0m;
                if (chkAplicaIva.Checked)
                {
                    iva = Decimal.Round(Convert.ToDecimal(spSub.Value * 0.15m), 2, MidpointRounding.AwayFromZero);
                    spIva.Value = iva;
                }

                spTotal.Value = Convert.ToDecimal(spSub.Value + iva);
            }
        }

        private void spIva_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(spIva.Value) > 0)
            {
                spTotal.Value = Convert.ToDecimal(spSub.Value + spIva.Value);
            }
        }

        private void chkAplicaIva_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAplicaIva.Checked)
            {
                lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                spSub_EditValueChanged(null, null);
            }
            else if (!chkAplicaIva.Checked)
            {
                lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                spIva.Value = 0;
                spSub_EditValueChanged(null, null);
            }
        }
        
        private void chkAplicaIva_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (Provee != null)
            {
                if (!Provee.IVA && Convert.ToBoolean(e.NewValue).Equals(true))
                {
                    Parametros.General.DialogMsg("El proveedor no aplica IVA.", Parametros.MsgType.warning);
                    e.Cancel = true;
                }

            }
        }

        #endregion





        private void radioTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   
            try
            {
                switch (radioTipo.SelectedIndex)
                {
                    //ES ALTA
                    case 0:
                        glkTipoMovimiento.EditValue = null;
                        glkTipoMovimiento.Properties.DataSource = ListTipoMovimiento.Where(d => d.EsAlta);
                        break;

                    //ES BAJA
                    case 1:
                        glkTipoMovimiento.EditValue = null;
                        glkTipoMovimiento.Properties.DataSource = ListTipoMovimiento.Where(d => d.EsBaja);
                        break;

                    //ES TRASLADO
                    case 2:
                        glkTipoMovimiento.EditValue = null;
                        glkTipoMovimiento.Properties.DataSource = ListTipoMovimiento.Where(d => d.EsTraslado);
                        lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;  
                        break;
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }            
        }
                

        //EVENTO CONTROL TIPO DE MOVIMIENTO
        private void glkTipoMovimiento_EditValueChanged(object sender, EventArgs e)
        {
            int TipoMovimientoID = Convert.ToInt32(glkTipoMovimiento.EditValue);
            List<ObjectListTipoMovimiento> LocalListTipoMovimiento = ListTipoMovimiento;

            lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;  

            if (LocalListTipoMovimiento.Where(d => d.ID == TipoMovimientoID && d.AplicaProveedor == true && d.EsAlta == true).Count() > 0)
            {
                glkProvClientEstacion.EditValue = null;
                glkProvClientEstacion.Properties.DataSource = ListProveedor;
                lblProvClientEstacion.Text = "Proveedor";
                glkProvClientEstacion.Properties.NullText = "Seleccione Proveedor";

                lblFechaAdquisicion.Text = "Fecha de Aquisición";

                //MOSTRAR CAMPOS
                lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            if (LocalListTipoMovimiento.Where(d => d.ID == TipoMovimientoID && d.AplicaProveedor == false && d.EsAlta == true).Count() > 0)
            {
                glkProvClientEstacion.EditValue = null;
                lblFechaAdquisicion.Text = "Fecha de Aquisición";

                //MOSTRAR CAMPOS
                lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            } 

            if (LocalListTipoMovimiento.Where(d => d.ID == TipoMovimientoID && d.AplicaCliente == true && d.EsBaja == true).Count() > 0)
            {
                glkProvClientEstacion.EditValue = null;
                lblProvClientEstacion.Text = "Cliente";
                glkProvClientEstacion.Properties.NullText = "Seleccione Cliente";
                glkProvClientEstacion.Properties.DataSource = ListCliente;                
                
                //lblFechaAdquisicion.Text = "Fecha de Liquidación";
                lblNoFactura.Text = "Referencia";

                //MOSTRAR CAMPOS
                lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always; 
            }

            if (LocalListTipoMovimiento.Where(d => d.ID == TipoMovimientoID && d.AplicaCliente == false && d.EsBaja == true).Count() > 0)
            {
                glkProvClientEstacion.EditValue = null;
                lblNoFactura.Text = "Referencia";

                //MOSTRAR CAMPOS
                lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
           

            //if (LocalListTipoMovimiento.Where(d => d.ID == TipoMovimientoID && d.EsTraslado == true).Count() > 0)
            //{
            //    lblProvClientEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblbtnActualizar.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblRuc.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblTelefono.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblDireccion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblFechaAdquisicion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblNoFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblTipoMoneda.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblTipoCambio.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblSubTotal.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblTotalFactura.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //    lblComentario.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            //}

            if (TipoMovimientoID > 0)
            {
                radioTipo.Properties.ReadOnly = true;
                EtTipoMovimientoActivo = null;
                EtTipoMovimientoActivo = db.TipoMovimientoActivo.Single(s => s.ID.Equals(TipoMovimientoID));
                layoutControlItemEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                IDEstacionServicio = Parametros.General.EstacionServicioID;
            }
        }

        //EVENTO CONTROL PROVEEDOR - CLIENTE - ESTACION
        private void glkProvClientEstacion_EditValueChanged(object sender, EventArgs e)
        {
             int r = radioTipo.SelectedIndex;

             if (IDSujeto > 0)
             {
                 switch (r)
                 {
                     //ES ALTA
                     case 0:

                         if (IDSujeto > 0)
                         {
                             //OBTENER PROVEEDOR
                             Provee = ListProveedor.Single(d => d.ID.Equals(IDSujeto));
                             if (Provee != null)
                             {
                                 txtRuc.Text = Provee.Ruc + " | " + Provee.Nombre;
                                 txtTelefonos.Text = Provee.Telefono;

                                 if (Provee.IVA)
                                     lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                                 else
                                 {
                                     chkAplicaIva.Checked = false;
                                     lblAplicaIVA.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                                 }
                             }
                         }

                         break;

                     //ES BAJA
                     case 1:

                         if (IDSujeto > 0)
                         {
                             //OBTENER CLIENTE
                             var GetCliente = ListCliente.Single(d => d.ID.Equals(IDSujeto));
                             if (GetCliente != null)
                             {
                                 txtRuc.Text = GetCliente.Ruc + " | " + GetCliente.Nombre;
                                 txtTelefonos.Text = GetCliente.Telefono;
                             }
                         }

                         break;

                     //ES TRASLADO 
                     case 2:

                         if (IDSujeto > 0)
                         {
                             //OBTENER ESTACIONES
                             var GetEstacion = ListEstacion.Single(d => d.ID.Equals(IDSujeto));

                             if (GetEstacion != null)
                             {
                                 //OBTENER BIEN DE DICHA ESTACION
                                 try
                                 {

                                 }
                                 catch (Exception ex)
                                 {
                                     MessageBox.Show(ex.ToString());
                                 }
                             }
                         }

                         break;
                 }
             }
             else
             {
                 txtRuc.Text = "";
                 txtTelefonos.Text = "";
             }
        }

        private void radioTipo_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
           

        }
        
        private void lkEs_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lkEs.EditValue != null)
                {
                    if (Convert.ToInt32(lkEs.EditValue).Equals(0))
                    {
                        this.lkSus.EditValue = null;
                        this.lkSus.Properties.DataSource = null;
                        this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    }
                    else if (!Convert.ToInt32(lkEs.EditValue).Equals(0))
                    {

                        var Sus = (db.SubEstacions.Where(sus => sus.Activo && sus.EstacionServicioID.Equals(Convert.ToInt32(lkEs.EditValue))).Select(s => new { s.ID, s.Nombre })).ToList();

                        if (Sus.Count > 0)
                        {
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            lkSus.Properties.DataSource = Sus;
                        }
                        else
                        {
                            this.lkSus.EditValue = null;
                            this.lkSus.Properties.DataSource = null;
                            this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                        }
                    }
                }
                else
                {
                    this.lkSus.EditValue = null;
                    this.lkSus.Properties.DataSource = null;
                    this.layoutControlItemSubEstacion.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

       
       

    }


    //OBJECT TIPO DE MOVIMIENTO
    public class ObjectListTipoMovimiento 
    {
        int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        bool aplicadepreciacion;
        public bool AplicaDepreciacion
        {
            get { return aplicadepreciacion; }
            set { aplicadepreciacion = value; }
        }

        bool aplicaproveedor;
        public bool AplicaProveedor
        {
            get { return aplicaproveedor; }
            set { aplicaproveedor = value; }
        }

        bool esalta;
        public bool EsAlta
        {
            get { return esalta; }
            set { esalta = value; }
        }

        bool aplicacliente;
        public bool AplicaCliente
        {
            get { return aplicacliente; }
            set { aplicacliente = value; }
        }

        bool estraslado;
        public bool EsTraslado
        {
            get { return estraslado; }
            set { estraslado = value; }
        }

        bool esbaja;
        public bool EsBaja
        {
            get { return esbaja; }
            set { esbaja = value; }
        }
    
    }

    //OBJECT TIPO DE CLIENTE
    public class ObjectListCliente
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        int cuentacontable;

        public int Cuentacontable
        {
            get { return cuentacontable; }
            set { cuentacontable = value; }
        }

        string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        string nombre;
        
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string display;

        public string Display
        {
            get { return display; }
            set { display = value; }
        }

        string ruc;

        public string Ruc
        {
            get { return ruc; }
            set { ruc = value; }
        }

        string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

    }

    //OBJECT TIPO DE PROVEEDOR
    public class ObjectListProveedor
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        int cuentacontable;

        public int Cuentacontable
        {
            get { return cuentacontable; }
            set { cuentacontable = value; }
        }

        string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string display;

        public string Display
        {
            get { return display; }
            set { display = value; }
        }

        string ruc;

        public string Ruc
        {
            get { return ruc; }
            set { ruc = value; }
        }

        string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        public bool iva;
        public bool IVA
        {
            get { return iva; }
            set { iva = value; }
        }
    }

    //OBJECT ESTACIONES
    public class ObjectListEstacion 
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string display;

        public string Display
        {
            get { return display; }
            set { display = value; }
        }
    }

    //OBJECT SUBESTACION
    public class ObjectListSubEstacion 
    {
        int stacionid;

        public int StacionID
        {
            get { return stacionid; }
            set { stacionid = value; }
        }
        int subestacionid;

        public int SubestacionID
        {
            get { return subestacionid; }
            set { subestacionid = value; }
        }
        string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        string display;

        public string Display
        {
            get { return display; }
            set { display = value; }
        }
    
    }
}