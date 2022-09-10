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
using SAGAS.Arqueo.Forms;
using DevExpress.XtraEditors.Popup;
using System.Windows.Input;
using DevExpress.XtraGrid.Views.Grid;

namespace SAGAS.Arqueo.Forms.Dialogs
{
    public partial class DialogArqueoEfectivo : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private DataTable dtAMonedas;
        private DataTable dtBBilletes;
        private DataTable dtCBilletes;
        private DataTable dtCheques;
        private Entidad.Turno EtTurno;
        private Entidad.ResumenDia EtResumenDia;
        private Entidad.ArqueoEfectivo ArqueoEfectivoSelected;
        private int IDAE = 0;
        private decimal TipoCambioRD;
        private int MonedaPrincipal = Parametros.Config.MonedaPrincipal();
        private int MonedaSecundaria = Parametros.Config.MonedaSecundaria();
        private int IDES = Parametros.General.EstacionServicioID;
        internal Entidad.ArqueoEfectivo EntidadAnterior;
        internal bool EsEspecial = false;
        public int IDturno = 0;

        public DataTable AMonedas
        {
            get
            {
                return dtAMonedas;
            }
            set
            {
                dtAMonedas = value;
            }
        }

        public DataTable BBilletes
        {
            get
            {
                return dtBBilletes;
            }
            set
            {
                dtBBilletes = value;
            }
        }

        public DataTable CBilletes
        {
            get
            {
                return dtCBilletes;
            }
            set
            {
                dtCBilletes = value;
            }
        }

        public DataTable Cheques
        {
            get
            {
                return dtCheques;
            }
            set
            {
                dtCheques = value;
            }
        }

        #endregion

        #region *** INICIO ***

        public DialogArqueoEfectivo()
        {
            InitializeComponent();
        }

        private void DialogUser_Load(object sender, EventArgs e)
        {
            if (FillControl())
                this.panelControlCenter.Location = new Point(2, 5);
            else
                this.BeginInvoke(new MethodInvoker(this.Close)); 
        }

        #endregion

        #region *** METODOS ***

        private bool FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                int IDSUS = 0;

                if (EntidadAnterior == null && !EsEspecial)
                {
                    if (Parametros.General.ListSES.Count > 0)
                    {
                        if (Parametros.General.ListSES.Count.Equals(1))
                            IDSUS = (Convert.ToInt32(Parametros.General.ListSES.FirstOrDefault()));
                        else if (Parametros.General.ListSES.Count > 1)
                        {
                            using (Forms.Dialogs.DialogGetFecha df = new Forms.Dialogs.DialogGetFecha(true))
                            {
                                df.layoutControlItemCaption.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                                if (df.ShowDialog().Equals(DialogResult.OK))
                                {
                                    IDSUS = df.IDSubEstacion;
                                }
                                else
                                    return false;
                            }
                        }
                    }
                }
                
                if (EntidadAnterior != null || EsEspecial)
                {
                    if (EntidadAnterior != null)
                        EtTurno = db.Turnos.Single(t => t.ID.Equals(Convert.ToInt32(EntidadAnterior.TurnoID)));
                    else
                        EtTurno = db.Turnos.Single(t => t.ID.Equals(Convert.ToInt32(IDturno)));

                    EtResumenDia = EtTurno.ResumenDia;
                }

                if (db.ResumenDias.Count(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSUS)) > 0 || EntidadAnterior != null)
                {
                    if (EntidadAnterior == null && !EsEspecial)
                    EtResumenDia = db.ResumenDias.Where(r => !r.Cerrado && r.EstacionServicioID.Equals(IDES) && r.SubEstacionID.Equals(IDSUS)).First();
                    
                    if (db.Turnos.Count(t => !t.Cerrada && t.ResumenDiaID.Equals(EtResumenDia.ID) && !t.Especial) > 0 || EntidadAnterior != null)
                    {
                        if (EntidadAnterior == null && !EsEspecial)
                            EtTurno = db.Turnos.Where(t => !t.Cerrada && t.ResumenDiaID.Equals(EtResumenDia.ID) && !t.Especial).First();

                        TipoCambioRD = EtResumenDia.TipoCambioMoneda;
                        txtEstacionServicio.Text = db.EstacionServicios.Single(es => es.ID.Equals(EtResumenDia.EstacionServicioID)).Nombre;
                        txtTurno.Text = EtTurno.Numero.ToString();
                        txtFecha.Text = EtTurno.FechaInicial.ToShortDateString();

                        int user = 0;

                        if (EntidadAnterior == null)
                            user = Convert.ToInt32(Parametros.General.UserID);
                        else if (EntidadAnterior != null)
                            user = EntidadAnterior.UsuarioCreado;

                        var Empleado = db.Empleados.Single(e => e.ID.Equals(Convert.ToInt32(db.Usuarios.Single(u => u.ID.Equals(user)).EmpleadoID)));
                        
                        
                        txtArqueador.Text = Empleado.Nombres + " " + Empleado.Apellidos;

                        spTipoCambio.Value = TipoCambioRD;

                        if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(EtTurno.ID)) > 0 || EntidadAnterior != null)
                        {
                            ArqueoEfectivoSelected = db.ArqueoEfectivos.Single(ae => ae.TurnoID.Equals(EtTurno.ID));
                            IDAE = ArqueoEfectivoSelected.ID;

                            if (ArqueoEfectivoSelected.Cerrado)
                            {
                                Parametros.General.DialogMsg("El Arqueo de Efectivo ya esta Finalizado", Parametros.MsgType.message);
                                return false;
                            }
                        }
                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTCARGANDO);
                        decimal _TotalEfectivoRecibido = Convert.ToDecimal(dbView.GetTotalEfectivoRecibido(EtResumenDia.ID, EtTurno.ID, 0));

                        spTotalArqueo.Value = _TotalEfectivoRecibido;

                        //spTotalArqueo.Value = 

                        #region <<< A-MONEDAS >>>

                        AMonedas = new DataTable();

                        AMonedas.Columns.Add("IDA", typeof(Int32));
                        AMonedas.Columns.Add("Valor", typeof(String));
                        AMonedas.Columns.Add("Formula", typeof(String));
                        AMonedas.Columns.Add("Denominacion", typeof(String));
                        AMonedas.Columns.Add("Equivalente", typeof(Decimal));
                        AMonedas.Columns.Add("TotalEfectivo", typeof(Decimal));

                        //foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                        //{
                        //    AMonedas.Rows.Add(objA.ID, "", "", objA.Denominacion, objA.Equivalente, 0m);
                        //}

                        foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                        {
                            var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objA.ID));
                            if (AEDAnterior.Count() > 0)
                                AMonedas.Rows.Add(objA.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objA.Denominacion, objA.Equivalente, AEDAnterior.First().TotalEfectivo);
                            else
                                AMonedas.Rows.Add(objA.ID, "", "", objA.Denominacion, objA.Equivalente, 0m);
                        }

                        gridAMonedas.DataSource = AMonedas;
                        gvDataAMonedas.RefreshData();

                        #endregion

                        #region <<< B-BILLETES >>>

                        BBilletes = new DataTable();

                        BBilletes.Columns.Add("IDB", typeof(Int32));
                        BBilletes.Columns.Add("Valor", typeof(String));
                        BBilletes.Columns.Add("Formula", typeof(String));
                        BBilletes.Columns.Add("Denominacion", typeof(String));
                        BBilletes.Columns.Add("Equivalente", typeof(Decimal));
                        BBilletes.Columns.Add("TotalEfectivo", typeof(Decimal));

                        //foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                        //{
                        //    BBilletes.Rows.Add(objB.ID, "", "", objB.Denominacion, objB.Equivalente, 0m);
                        //}

                        foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                        {
                            var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objB.ID));
                            if (AEDAnterior.Count() > 0)
                                BBilletes.Rows.Add(objB.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objB.Denominacion, objB.Equivalente, AEDAnterior.First().TotalEfectivo);
                            else
                                BBilletes.Rows.Add(objB.ID, "", "", objB.Denominacion, objB.Equivalente, 0m);
                        }

                        gridBBilletes.DataSource = BBilletes;
                        gvDataBBilletes.RefreshData();

                        #endregion

                        #region <<< C-BILLETES >>>

                        CBilletes = new DataTable();

                        CBilletes.Columns.Add("IDC", typeof(Int32));
                        CBilletes.Columns.Add("Valor", typeof(String));
                        CBilletes.Columns.Add("Formula", typeof(String));
                        CBilletes.Columns.Add("Denominacion", typeof(String));
                        CBilletes.Columns.Add("Equivalente", typeof(Decimal));
                        CBilletes.Columns.Add("TotalEfectivo", typeof(Decimal));

                        //foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                        //{
                        //    CBilletes.Rows.Add(objC.ID, "", "", objC.Denominacion, objC.Equivalente, 0m);
                        //}

                        foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                        {
                            var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objC.ID));
                            if (AEDAnterior.Count() > 0)
                                CBilletes.Rows.Add(objC.ID, AEDAnterior.First().Valor, AEDAnterior.First().Formula, objC.Denominacion, objC.Equivalente, AEDAnterior.First().TotalEfectivo);
                            else
                                CBilletes.Rows.Add(objC.ID, "", "", objC.Denominacion, objC.Equivalente, 0m);
                        }

                        //this.layoutControlGroupDolar.Text = "C - Billetes Dólares Tipo de Cambio " + TipoCambioRD.ToString("#,0.00");
                        gridCBilletes.DataSource = CBilletes;
                        gvDataCBilletes.RefreshData();

                        #endregion

                        #region <<< CHEQUES >>>

                        Cheques = new DataTable();

                        Cheques.Columns.Add("IDCh", typeof(Int32));
                        Cheques.Columns.Add("NumeroCheque", typeof(String));
                        Cheques.Columns.Add("Banco", typeof(String));
                        Cheques.Columns.Add("Monto", typeof(Decimal));

                        if (ArqueoEfectivoSelected != null)
                        {
                            db.ArqueoEfectivoDetalles.Where(ae => ae.ArqueoEfectivoID.Equals(ArqueoEfectivoSelected.ID) && ae.EsCheque).ToList().ForEach(ch =>
                                {
                                    Cheques.Rows.Add(ch.ID, ch.NumeroCheque, ch.Banco, ch.TotalEfectivo);
                                });
                        }

                        bindingSourceCheques.DataSource = Cheques;

                        gvDataCheques.RefreshData();

                        if (gvDataCheques.RowCount > 0)
                        {
                            gvDataCheques.RefreshData();
                            gvDataCheques.Columns["Monto"].SummaryItem.DisplayFormat = "{0:#,0.00;(#,0.00)}";
                            gvDataCheques.Columns["Monto"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            gvDataCheques.Columns["Monto"].UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                            gvDataCheques.Columns["Monto"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            gvDataCheques.Columns["Monto"].DisplayFormat.FormatString = "N2";
                            gvDataCheques.RefreshData();

                        }
                        
                        CalculosTotales();
                        Parametros.General.splashScreenManagerMain.CloseWaitForm();
                        this.btnOK.Enabled = true;
                        return true;
                        #endregion
                        
                    }
                    else
                    {
                        Parametros.General.DialogMsg("No Existe un Turno Abierto", Parametros.MsgType.warning);
                        return false;
                    }
                }
                else
                {
                    Parametros.General.DialogMsg("No Existe un Resumen de Día Abierto", Parametros.MsgType.warning);
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

        private bool Guardar()
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Entidad.ArqueoEfectivo AE;

                    if (db.ArqueoEfectivos.Count(ae => ae.TurnoID.Equals(EtTurno.ID)) > 0)
                        AE = db.ArqueoEfectivos.Single(ae => ae.TurnoID.Equals(EtTurno.ID));
                    else
                    {
                        AE = new Entidad.ArqueoEfectivo();

                        AE.TurnoID = EtTurno.ID;
                        AE.UsuarioCreado = Parametros.General.UserID;
                        AE.FechaCreado = EtTurno.FechaInicial;
                        AE.Estado = 0;
                        db.ArqueoEfectivos.InsertOnSubmit(AE);
                    }

                    AE.Diferencia = Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero);

                    db.SubmitChanges();

                    #region <<< AMONEDAS >>>
                    foreach (DataRow ra in AMonedas.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(ra["TotalEfectivo"])))
                        {
                            if (Convert.ToDecimal(ra["TotalEfectivo"]) > 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(ra["IDA"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = AEDAnterior.First();

                                    AED.EfectivoID = Convert.ToInt32(ra["IDA"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(ra["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(ra["Valor"]);
                                    AED.Formula = Convert.ToString(ra["Formula"]);
                                    db.SubmitChanges();
                                }
                                else if (AEDAnterior.Count() <= 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = new Entidad.ArqueoEfectivoDetalle();
                                    AED.ArqueoEfectivoID = AE.ID;
                                    AED.EfectivoID = Convert.ToInt32(ra["IDA"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(ra["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(ra["Valor"]);
                                    AED.Formula = Convert.ToString(ra["Formula"]);
                                    db.ArqueoEfectivoDetalles.InsertOnSubmit(AED);
                                    db.SubmitChanges();
                                }
                            }
                            else if (String.IsNullOrEmpty(Convert.ToString(ra["TotalEfectivo"])) || Convert.ToDecimal(ra["TotalEfectivo"]) <= 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(ra["IDA"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                                    db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                                    db.SubmitChanges();
                                }

                            }

                        }

                    }
                    #endregion

                    #region <<< BCORDOBAS >>>
                    foreach (DataRow rb in BBilletes.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(rb["TotalEfectivo"])))
                        {
                            if (Convert.ToDecimal(rb["TotalEfectivo"]) > 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(rb["IDB"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = AEDAnterior.First();

                                    AED.EfectivoID = Convert.ToInt32(rb["IDB"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(rb["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(rb["Valor"]);
                                    AED.Formula = Convert.ToString(rb["Formula"]);
                                    db.SubmitChanges();
                                }
                                else if (AEDAnterior.Count() <= 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = new Entidad.ArqueoEfectivoDetalle();
                                    AED.ArqueoEfectivoID = AE.ID;
                                    AED.EfectivoID = Convert.ToInt32(rb["IDB"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(rb["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(rb["Valor"]);
                                    AED.Formula = Convert.ToString(rb["Formula"]);
                                    db.ArqueoEfectivoDetalles.InsertOnSubmit(AED);
                                    db.SubmitChanges();
                                }
                            }
                            else if (String.IsNullOrEmpty(Convert.ToString(rb["TotalEfectivo"])) || Convert.ToDecimal(rb["TotalEfectivo"]) <= 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(rb["IDB"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                                    db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                                    db.SubmitChanges();
                                }

                            }

                        }

                    }
                    #endregion

                    #region <<< CDOLAR >>>
                    foreach (DataRow rc in CBilletes.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(rc["TotalEfectivo"])))
                        {
                            if (Convert.ToDecimal(rc["TotalEfectivo"]) > 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(rc["IDC"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = AEDAnterior.First();

                                    AED.EfectivoID = Convert.ToInt32(rc["IDC"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(rc["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(rc["Valor"]);
                                    AED.Formula = Convert.ToString(rc["Formula"]);
                                    db.SubmitChanges();
                                }
                                else if (AEDAnterior.Count() <= 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AED = new Entidad.ArqueoEfectivoDetalle();
                                    AED.ArqueoEfectivoID = AE.ID;
                                    AED.EfectivoID = Convert.ToInt32(rc["IDC"]);
                                    AED.TotalEfectivo = Convert.ToDecimal(rc["TotalEfectivo"]);
                                    AED.Valor = Convert.ToDecimal(rc["Valor"]);
                                    AED.Formula = Convert.ToString(rc["Formula"]);
                                    db.ArqueoEfectivoDetalles.InsertOnSubmit(AED);
                                    db.SubmitChanges();
                                }
                            }
                            else if (String.IsNullOrEmpty(Convert.ToString(rc["TotalEfectivo"])) || Convert.ToDecimal(rc["TotalEfectivo"]) <= 0)
                            {
                                var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.EfectivoID.Equals(Convert.ToInt32(rc["IDC"])));

                                if (AEDAnterior.Count() > 0)
                                {
                                    Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                                    db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                                    db.SubmitChanges();
                                }

                            }

                        }

                    }

                    #endregion

                    foreach (DataRow rch in Cheques.Rows)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(rch["IDCh"])))
                        {
                            var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(AE.ID) && x.ID.Equals(Convert.ToInt32(rch["IDCh"])));

                            if (AEDAnterior.Count() > 0)
                            {
                                Entidad.ArqueoEfectivoDetalle AED = AEDAnterior.First();

                                AED.EsCheque = true;
                                AED.NumeroCheque = Convert.ToString(rch["NumeroCheque"]);
                                AED.Banco = Convert.ToString(rch["Banco"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rch["Monto"]);
                                db.SubmitChanges();
                            }
                            else if (AEDAnterior.Count() <= 0)
                            {
                                Entidad.ArqueoEfectivoDetalle AED = new Entidad.ArqueoEfectivoDetalle();
                                AED.ArqueoEfectivoID = AE.ID;
                                AED.EsCheque = true;
                                AED.NumeroCheque = Convert.ToString(rch["NumeroCheque"]);
                                AED.Banco = Convert.ToString(rch["Banco"]);
                                AED.TotalEfectivo = Convert.ToDecimal(rch["Monto"]);
                                AED.Formula = "";
                                db.ArqueoEfectivoDetalles.InsertOnSubmit(AED);
                                db.SubmitChanges();
                                rch["IDCh"] = AED.ID;
                            }
                        }
                        else if (String.IsNullOrEmpty(Convert.ToString(rch["IDCh"])))
                        {
                            Entidad.ArqueoEfectivoDetalle AED = new Entidad.ArqueoEfectivoDetalle();
                            AED.ArqueoEfectivoID = AE.ID;
                            AED.EsCheque = true;
                            AED.NumeroCheque = Convert.ToString(rch["NumeroCheque"]);
                            AED.Banco = Convert.ToString(rch["Banco"]);
                            AED.TotalEfectivo = Convert.ToDecimal(rch["Monto"]);
                            AED.Formula = "";
                            db.ArqueoEfectivoDetalles.InsertOnSubmit(AED);
                            db.SubmitChanges();  
                            rch["IDCh"] = AED.ID;
                        }
                    }

                    db.SubmitChanges();
                    trans.Commit();

                    //ShowMsg = true;
                    ArqueoEfectivoSelected = AE;
                    btnOK.Enabled = false;
                    return true;
                }

                catch (Exception ex)
                {
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

        private void CalculosTotales()
        {
            try
            {
                spTotalCordoba.Value = Decimal.Round(Convert.ToDecimal(gvDataAMonedas.Columns["TotalEfectivo"].SummaryText) + Convert.ToDecimal(gvDataBBilletes.Columns["TotalEfectivo"].SummaryText), 2, MidpointRounding.AwayFromZero);
                spEquivalenteCordobas.Value = Decimal.Round(Convert.ToDecimal(gvDataCBilletes.Columns["TotalEfectivo"].SummaryText) * TipoCambioRD, 2, MidpointRounding.AwayFromZero);
                gvDataCheques.RefreshData();
                decimal cheques = 0m;
                cheques = Decimal.TryParse(Cheques.Compute("Sum(Monto)", "").ToString(), out cheques) ? cheques : 0m;
                //gvDataCheques.Columns["Monto"].SummaryText
                
                spTotalEfectivo.Value = Decimal.Round(Convert.ToDecimal(spTotalCordoba.Value + cheques + spEquivalenteCordobas.Value), 2, MidpointRounding.AwayFromZero);
                spDiferencia.Value = Decimal.Round(spTotalEfectivo.Value - spTotalArqueo.Value, 2, MidpointRounding.AwayFromZero);

                if (spDiferencia.Value < 0)
                {
                    spDiferencia.BackColor = Color.Red;
                    spDiferencia.ForeColor = Color.White;
                }
                else
                {
                    spDiferencia.BackColor = Color.AliceBlue;
                    spDiferencia.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region *** EVENTOS ***

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (!Guardar()) return;

            this.btnOK.Enabled = false;
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MDI.CleanDialog(ShowMsg);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region <<< EventosDCHEQUES >>>

        private void gvDataAMonedas_MouseWheel(object sender, MouseEventArgs e)
        {
            // If the mouse wheel delta is positive, move the box up. 
            if (e.Delta > 0)
            {
                this.xtraScrollableControlMain.VerticalScroll.Value -= 20;
            }

            // If the mouse wheel delta is negative, move the box down. 
            if (e.Delta < 0)
            {
                this.xtraScrollableControlMain.VerticalScroll.Value += 20;
            }
        }

        private void gvDataCheques_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }
                if (e.KeyCode == Keys.Delete)
                {
                    if (gvDataCheques.FocusedRowHandle >= 0)
                    {
                        if (!String.IsNullOrEmpty(gvDataCheques.GetFocusedRowCellValue("IDCh").ToString()))
                        {
                            var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(ArqueoEfectivoSelected.ID) && x.ID.Equals(Convert.ToInt32(gvDataCheques.GetFocusedRowCellValue("IDCh"))));

                            if (AEDAnterior.Count() > 0)
                            {
                                Entidad.ArqueoEfectivoDetalle AEDDel = AEDAnterior.First();
                                db.ArqueoEfectivoDetalles.DeleteOnSubmit(AEDDel);
                                db.SubmitChanges();
                            }
                        }

                        Cheques.Rows.RemoveAt(gvDataCheques.FocusedRowHandle);

                        gvDataCheques.RefreshData();
                        CalculosTotales();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void gvDataCheques_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column == gvDataCheques.Columns["Monto"])
            {
                gvDataCheques.RefreshData();
                e.Column.SummaryItem.DisplayFormat = "{0:#,0.00;(#,0.00)}";
                e.Column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                e.Column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                e.Column.DisplayFormat.FormatString = "N2";

                gvDataCheques.RefreshData();
                CalculosTotales();
            }
        }

        #endregion

        #region <<< EventosAMONEDAS >>>

        private void rpMemoA_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataAMonedas.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataAMonedas.SetFocusedRowCellValue("Formula", Operacion);
                        
                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataAMonedas.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataAMonedas.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataAMonedas.RefreshData();
                        CalculosTotales();
                    } 
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataAMonedas.SetFocusedRowCellValue("Formula", "");
                    gvDataAMonedas.SetFocusedRowCellValue("Valor", "");
                    gvDataAMonedas.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataAMonedas.RefreshData();
                    CalculosTotales();
                } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoA_Popup(object sender, EventArgs e)
        {
            try
            {  
                if (gvDataAMonedas.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                                                                                    
                    me.Text = Convert.ToString(gvDataAMonedas.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        #region <<< EventosBBILLETES >>>

        private void rpMemoB_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataBBilletes.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataBBilletes.SetFocusedRowCellValue("Formula", Operacion);

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataBBilletes.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataBBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataBBilletes.RefreshData();
                        CalculosTotales();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataBBilletes.SetFocusedRowCellValue("Formula", "");
                    gvDataBBilletes.SetFocusedRowCellValue("Valor", "");
                    gvDataBBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataBBilletes.RefreshData();
                    CalculosTotales();
                } 
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoB_Popup(object sender, EventArgs e)
        {
            try
            {  
                if (gvDataBBilletes.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                    me.Text = Convert.ToString(gvDataBBilletes.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion

        #region <<< EventosCBILLETES >>>

        private void rpMemoC_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
        {
            try
            {
                MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                MemoEdit me = popupForm.Controls[2] as MemoEdit;
                MemoExEdit memo = (MemoExEdit)sender;

                string Operacion = me.EditValue.ToString();
                decimal denominacion = Convert.ToDecimal(gvDataCBilletes.GetFocusedRowCellValue("Equivalente"));

                if (!String.IsNullOrEmpty(Operacion))
                {
                    Operacion = Operacion.Replace(Environment.NewLine, "");
                    Operacion = Operacion.Replace(@"\r\n", "");

                    gvDataCBilletes.SetFocusedRowCellValue("Formula", Operacion);

                    if (!EsFormulaCorrecta(Operacion))
                    {
                        memo.ShowPopup();
                    }
                    else
                    {
                        decimal val = Decimal.Round(ValorFormula(Operacion), 2, MidpointRounding.AwayFromZero);

                        gvDataCBilletes.SetFocusedRowCellValue("Valor", val.ToString("#,0.00"));
                        gvDataCBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((val * denominacion), 2, MidpointRounding.AwayFromZero));
                        //memo.EditValue = val.ToString("#,0.00");
                        gvDataCBilletes.RefreshData();
                        CalculosTotales();
                    }
                }
                if (String.IsNullOrEmpty(Operacion))
                {
                    gvDataCBilletes.SetFocusedRowCellValue("Formula", "");
                    gvDataCBilletes.SetFocusedRowCellValue("Valor", "");
                    gvDataCBilletes.SetFocusedRowCellValue("TotalEfectivo", Decimal.Round((0m * denominacion), 2, MidpointRounding.AwayFromZero));
                    gvDataCBilletes.RefreshData();
                    CalculosTotales();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void rpMemoC_Popup(object sender, EventArgs e)
        {
            try
            {
                if (gvDataCBilletes.FocusedRowHandle >= 0)
                {
                    MemoExPopupForm popupForm = (sender as DevExpress.Utils.Win.IPopupControl).PopupWindow as MemoExPopupForm;
                    popupForm.CloseButton.Visible = false;

                    MemoEdit me = popupForm.Controls[2] as MemoEdit;
                    me.Text = Convert.ToString(gvDataCBilletes.GetFocusedRowCellValue("Formula"));
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        #endregion   

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.previewBarTop.Visible = false;
                rv.Text = "Vista Previa Arqueo Efectivo";
                Reportes.Util.PrintArqueoEfectivo(this, dbView, db, ArqueoEfectivoSelected, EtTurno, EtResumenDia, Parametros.TiposImpresion.Vista_Previa, false, Parametros.Properties.Resources.TXTVISTAPREVIA, rv.printControlAreaReport);
                rv.ShowDialog();


                //Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), "Cargando Datos Arqueo de Efectivo");

                //this.Cursor = Cursors.WaitCursor;
                //decimal ThisTipoCambio = TipoCambioRD;

                //var AEView = from v in db.ArqueoEfectivoDetalles
                //             join ae in db.ArqueoEfectivos on v.ArqueoEfectivoID equals ae.ID
                //             where ae.ID.Equals(ArqueoEfectivoSelected.ID)
                //             select v;

                ////Datos Iniciales del Reporte
                //Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                //Reportes.Arqueos.Hojas.RptEfectivo rep = new Reportes.Arqueos.Hojas.RptEfectivo();
                //rv.previewBarTop.Visible = false;
                //string Nombre, Direccion, Telefono;
                //System.Drawing.Image picture_LogoEmpresa;
                //decimal _TotalAMonedas = 0m;
                //decimal _TotalBcordobas = 0m;
                //decimal _TotalEfectivoCordobas = 0m;
                //decimal _TotalEfectivoDolares = 0m;
                //decimal _TotalDolaresEquivalenteCordobas = 0m;
                //decimal _TotalCheques = 0m;
                //decimal _GRANDTOTAL = 0m;
                ////decimal _DescuentoNegativo = 0m;

                //Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                //rep.PicLogo.Image = picture_LogoEmpresa;
                //rep.CeEmpresa.Text = Nombre;
                ////rep.CeEstacion.Text = Parametros.General.EstacionServicioName;

                //rep.CeEstacion.Text = (EtResumenDia.SubEstacionID.Equals(0) ?
                //    db.EstacionServicios.Single(es => es.ID.Equals(EtResumenDia.EstacionServicioID)).Nombre :
                //    db.SubEstacions.Single(sus => sus.ID.Equals(EtResumenDia.SubEstacionID)).Nombre);

                //rep.Watermark.Text = Parametros.TiposImpresion.Vista_Previa.ToString();

                //rep.lblTipoImpresion.Text = Parametros.TiposImpresion.Vista_Previa.ToString();
                //rep.CeFecha.Text = ArqueoEfectivoSelected.FechaCreado.ToShortDateString();
                //rep.CellPrintDate.Text = db.GetDateServer().ToString();
                //rv.Text = "Efectivo de Arqueo por Turno " + EtTurno.Numero;
                //rep.CellFirmaDepositante.Text = txtArqueador.Text;
                //rep.CeTitulo.Text = "Efectivo de Arqueo por Turno " + EtTurno.Numero;

                //#region <<< TABLA >>>
                //DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                //xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                //xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(110F, 55f);
                //xrTableAP.SizeF = new System.Drawing.SizeF(500f, 50F);

                //#endregion

                //#region A-MONEDAS

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowATitulo = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Titulo
                //xrTableRowATitulo.Weight = 1D;
                //xrTableRowATitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowATitulo.Cells.Add(new Parametros.MyXRTableCell("A - MONEDAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowATitulo);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowAColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Columnas
                //xrTableRowAColumnas.Weight = 1D;
                //xrTableRowAColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowAColumnas);

                //foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                //{
                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowAValor = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Filas Valores
                //    xrTableRowAValor.Weight = 1D;
                //    xrTableRowAValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objA.ID));
                //    if (AEDAnterior.Count() > 0)
                //    {
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        _TotalAMonedas += AEDAnterior.First().TotalEfectivo;
                //    }
                //    else
                //    {
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //    }
                //    xrTableAP.Rows.Add(xrTableRowAValor);
                
                //}

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowATotal = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total A-Monedas
                //xrTableRowATotal.Weight = 1D;
                //xrTableRowATotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Monedas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalAMonedas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowATotal);

                //#endregion

                //#region B-CORDOBAS

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowBTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Titulo
                //xrTableRowBTitulo.Weight = 1D;
                //xrTableRowBTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowBTitulo.Cells.Add(new Parametros.MyXRTableCell("B - BILLETES CÓRDOBAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowBTitulo);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowBColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Columnas
                //xrTableRowBColumnas.Weight = 1D;
                //xrTableRowBColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowBColumnas);

                //foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                //{
                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowBValor = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Filas Valores
                //    xrTableRowBValor.Weight = 1D;
                //    xrTableRowBValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objB.ID));
                //    if (AEDAnterior.Count() > 0)
                //    {
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        _TotalBcordobas += AEDAnterior.First().TotalEfectivo;
                //    }
                //    else
                //    {
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //    }
                //    xrTableAP.Rows.Add(xrTableRowBValor);

                //}

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowBTotal = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total B-Cordobas
                //xrTableRowBTotal.Weight = 1D;
                //xrTableRowBTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("Total B - Billetes Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalBcordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowBTotal);

                //#endregion

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowABTotal = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total A-Monedas + B-Cordobas
                //xrTableRowABTotal.Weight = 1D;
                //xrTableRowABTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo Córdobas A + B ", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //_TotalEfectivoCordobas = _TotalAMonedas + _TotalBcordobas;
                //xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowABTotal);

                //#region C-DOLARES
                //DevExpress.XtraReports.UI.XRTableRow xrTableRowTipoCambio = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Titulo
                //xrTableRowTipoCambio.Weight = 1D;
                //xrTableRowTipoCambio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("Tipo de Cambio", 200f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell(ThisTipoCambio.ToString("#,0.0000"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Goldenrod, Color.White));
                //xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                
                //xrTableAP.Rows.Add(xrTableRowTipoCambio);


                //DevExpress.XtraReports.UI.XRTableRow xrTableRowCTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Titulo
                //xrTableRowCTitulo.Weight = 1D;
                //xrTableRowCTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowCTitulo.Cells.Add(new Parametros.MyXRTableCell("C - BILLETES DOLARES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowCTitulo);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowCColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                ////Fila Columnas
                //xrTableRowCColumnas.Weight = 1D;
                //xrTableRowCColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowCColumnas);

                //foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                //{
                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowCValor = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Filas Valores
                //    xrTableRowCValor.Weight = 1D;
                //    xrTableRowCValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                //    var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objC.ID));
                //    if (AEDAnterior.Count() > 0)
                //    {
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        _TotalEfectivoDolares += AEDAnterior.First().TotalEfectivo;
                //    }
                //    else
                //    {
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //        xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //    }
                //    xrTableAP.Rows.Add(xrTableRowCValor);

                //}

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowCTotal = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total C-Dolares
                //xrTableRowCTotal.Weight = 1D;
                //xrTableRowCTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("Total C - Billetes Dólares", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("$ " + _TotalEfectivoDolares.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowCTotal);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowDolarACordobas = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total Dolares a Cordobas
                //xrTableRowDolarACordobas.Weight = 1D;
                //xrTableRowDolarACordobas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("Dólares Equivalente a Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //_TotalDolaresEquivalenteCordobas = _TotalEfectivoDolares * ThisTipoCambio;
                //xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalDolaresEquivalenteCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowDolarACordobas);

                //#endregion

                //#region CHEQUES
                //if (AEView.Count(ch => ch.EsCheque) > 0)
                //{
                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowChequeTit = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Fila Titulo
                //    xrTableRowChequeTit.Weight = 1D;
                //    xrTableRowChequeTit.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //    xrTableRowChequeTit.Cells.Add(new Parametros.MyXRTableCell("D - CHEQUES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //    xrTableAP.Rows.Add(xrTableRowChequeTit);

                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowDColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Fila Columnas
                //    xrTableRowDColumnas.Weight = 1D;
                //    xrTableRowDColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Número de Cheque", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Banco", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //    xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Monto", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //    xrTableAP.Rows.Add(xrTableRowDColumnas);

                //    AEView.Where(c => c.EsCheque).ToList().ForEach(ch =>
                //        {  
                //            DevExpress.XtraReports.UI.XRTableRow xrTableRowChValor = new DevExpress.XtraReports.UI.XRTableRow();
                //            //Filas Valores
                //            xrTableRowChValor.Weight = 1D;
                //            xrTableRowChValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.NumeroCheque, 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.Banco, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                //            _TotalCheques += ch.TotalEfectivo;

                //            xrTableAP.Rows.Add(xrTableRowChValor);

                //        });

                //    DevExpress.XtraReports.UI.XRTableRow xrTableRowChTotal = new DevExpress.XtraReports.UI.XRTableRow();
                //    //Filas Total C-Dolares
                //    xrTableRowChTotal.Weight = 1D;
                //    xrTableRowChTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //    xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("Total D - CHEQUES", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                //    xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalCheques.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //    xrTableAP.Rows.Add(xrTableRowChTotal);

                //}
                //#endregion

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalEfectivo = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total Dolares a Cordobas
                //xrTableRowTotalEfectivo.Weight = 1D;
                //xrTableRowTotalEfectivo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("TOTAL EFECTIVO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //_GRANDTOTAL = _TotalEfectivoCordobas + _TotalDolaresEquivalenteCordobas + _TotalCheques;
                //xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _GRANDTOTAL.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowTotalEfectivo);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalArqueo = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Total Arqueo
                //xrTableRowTotalArqueo.Weight = 1D;
                //xrTableRowTotalArqueo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("TOTAL ARQUEO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("C$ " + spTotalArqueo.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                //xrTableAP.Rows.Add(xrTableRowTotalArqueo);

                //DevExpress.XtraReports.UI.XRTableRow xrTableRowDiferencia = new DevExpress.XtraReports.UI.XRTableRow();
                ////Filas Diferencia
                //xrTableRowDiferencia.Weight = 1D;
                //xrTableRowDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                //| DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                //xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("Diferencia en Efectivo", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                //if (spDiferencia.Value < 0)
                //    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + spDiferencia.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
                //else
                //    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + spDiferencia.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                //xrTableAP.Rows.Add(xrTableRowDiferencia); 

                //rep.GroupHeaderArqueo.Controls.Add(xrTableAP);

                //rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                //rv.MdiParent = this.MdiParent;

                //rep.RequestParameters = true;

                //rep.CreateDocument();
                //Parametros.General.splashScreenManagerMain.CloseWaitForm();

                //rv.ShowDialog();

                //this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                //Parametros.General.splashScreenManagerMain.CloseWaitForm();
                //this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        //Finalizar Arqueo Efectivo
        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (db.GetArqueosIslasPendientes(IDES, EtTurno.ID, EtResumenDia.SubEstacionID) <= 0 || EtTurno.Especial)
                {
                    if (Parametros.General.DialogMsg("Este arqueo de efectivo sera finalizado y no se podra modifcar, ¿Desea Continuar?" + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
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

                        Parametros.Estados State = ((Parametros.Estados)(ArqueoEfectivoSelected.Estado));
                        Entidad.ArqueoEfectivo AE = ArqueoEfectivoSelected;
                        AE.Cerrado = true;
                        AE.FechaCerrado = db.GetDateServer();
                        AE.Estado = 2;
                        AE.UsuarioCerrado = Parametros.General.UserID;
                        AE.Diferencia = Decimal.Round(spDiferencia.Value, 2, MidpointRounding.AwayFromZero);
                        db.SubmitChanges();

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Finalizó el Arqueo de Efectivo: " + EtTurno.Numero.ToString() + " del " + EtResumenDia.FechaInicial.Date.ToString(), this.Name);

                        Entidad.SAGASDataViewsDataContext dbView = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                        Reportes.Util.PrintArqueoEfectivo(this, dbView, db, AE, EtTurno, EtResumenDia, (State.Equals(Parametros.Estados.Abierto) ? Parametros.TiposImpresion.Original : Parametros.TiposImpresion.Modificado), true, Parametros.Properties.Resources.TXTGUARDANDO, null);
                        this.Close();


                    //    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), "Imprimiendo Arqueo de Efectivo");

                    //    this.Cursor = Cursors.WaitCursor;
                    //    decimal ThisTipoCambio = TipoCambioRD;

                    //    var AEView = from v in db.ArqueoEfectivoDetalles
                    //                 join ae in db.ArqueoEfectivos on v.ArqueoEfectivoID equals ae.ID
                    //                 where ae.ID.Equals(ArqueoEfectivoSelected.ID)
                    //                 select v;



                    //    //Datos Iniciales del Reporte
                    //    Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                    //    Reportes.Arqueos.Hojas.RptEfectivo rep = new Reportes.Arqueos.Hojas.RptEfectivo();
                    //    rv.previewBarTop.Visible = false;
                    //    string Nombre, Direccion, Telefono;
                    //    System.Drawing.Image picture_LogoEmpresa;
                    //    decimal _TotalAMonedas = 0m;
                    //    decimal _TotalBcordobas = 0m;
                    //    decimal _TotalEfectivoCordobas = 0m;
                    //    decimal _TotalEfectivoDolares = 0m;
                    //    decimal _TotalDolaresEquivalenteCordobas = 0m;
                    //    decimal _TotalCheques = 0m;
                    //    decimal _GRANDTOTAL = 0m;
                    //    //decimal _DescuentoNegativo = 0m;

                    //    Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                    //    rep.PicLogo.Image = picture_LogoEmpresa;
                    //    rep.CeEmpresa.Text = Nombre;
                    //    //rep.CeEstacion.Text = Parametros.General.EstacionServicioName;

                    //    rep.CeEstacion.Text = (EtResumenDia.SubEstacionID.Equals(0) ?
                    //db.EstacionServicios.Single(es => es.ID.Equals(EtResumenDia.EstacionServicioID)).Nombre :
                    //db.SubEstacions.Single(sus => sus.ID.Equals(EtResumenDia.SubEstacionID)).Nombre);

                    //    rep.lblTipoImpresion.Text = (((Parametros.Estados)(ArqueoEfectivoSelected.Estado)).Equals(Parametros.Estados.Abierto) ? Parametros.TiposImpresion.Original.ToString() : Parametros.TiposImpresion.Modificado.ToString());
                    //    rep.CeFecha.Text = ArqueoEfectivoSelected.FechaCreado.ToShortDateString();
                    //    rep.CellPrintDate.Text = db.GetDateServer().ToString();
                    //    rv.Text = "Efectivo de Arqueo por Turno " + EtTurno.Numero;
                    //    rep.CellFirmaDepositante.Text = txtArqueador.Text;
                    //    rep.CeTitulo.Text = "Efectivo de Arqueo por Turno " + EtTurno.Numero;

                    //    #region <<< TABLA >>>
                    //    DevExpress.XtraReports.UI.XRTable xrTableAP = new DevExpress.XtraReports.UI.XRTable();
                    //    xrTableAP.Font = new System.Drawing.Font("Tahoma", 8F);
                    //    xrTableAP.LocationFloat = new DevExpress.Utils.PointFloat(110F, 55f);
                    //    xrTableAP.SizeF = new System.Drawing.SizeF(500f, 50F);

                    //    #endregion

                    //    #region A-MONEDAS

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowATitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Titulo
                    //    xrTableRowATitulo.Weight = 1D;
                    //    xrTableRowATitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowATitulo.Cells.Add(new Parametros.MyXRTableCell("A - MONEDAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowATitulo);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowAColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Columnas
                    //    xrTableRowAColumnas.Weight = 1D;
                    //    xrTableRowAColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowAColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowAColumnas);

                    //    foreach (var objA in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(false)).OrderByDescending(o => o.Equivalente))
                    //    {
                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowAValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Filas Valores
                    //        xrTableRowAValor.Weight = 1D;
                    //        xrTableRowAValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //        var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objA.ID));
                    //        if (AEDAnterior.Count() > 0)
                    //        {
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            _TotalAMonedas += AEDAnterior.First().TotalEfectivo;
                    //        }
                    //        else
                    //        {
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell(objA.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowAValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //        }
                    //        xrTableAP.Rows.Add(xrTableRowAValor);

                    //    }

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowATotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total A-Monedas
                    //    xrTableRowATotal.Weight = 1D;
                    //    xrTableRowATotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Monedas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableRowATotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalAMonedas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowATotal);

                    //    #endregion

                    //    #region B-CORDOBAS

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowBTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Titulo
                    //    xrTableRowBTitulo.Weight = 1D;
                    //    xrTableRowBTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowBTitulo.Cells.Add(new Parametros.MyXRTableCell("B - BILLETES CÓRDOBAS", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowBTitulo);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowBColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Columnas
                    //    xrTableRowBColumnas.Weight = 1D;
                    //    xrTableRowBColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowBColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowBColumnas);

                    //    foreach (var objB in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaPrincipal) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                    //    {
                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowBValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Filas Valores
                    //        xrTableRowBValor.Weight = 1D;
                    //        xrTableRowBValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //        var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objB.ID));
                    //        if (AEDAnterior.Count() > 0)
                    //        {
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            _TotalBcordobas += AEDAnterior.First().TotalEfectivo;
                    //        }
                    //        else
                    //        {
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell(objB.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowBValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //        }
                    //        xrTableAP.Rows.Add(xrTableRowBValor);

                    //    }

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowBTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total B-Cordobas
                    //    xrTableRowBTotal.Weight = 1D;
                    //    xrTableRowBTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("Total B - Billetes Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableRowBTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalBcordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowBTotal);

                    //    #endregion

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowABTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total A-Monedas + B-Cordobas
                    //    xrTableRowABTotal.Weight = 1D;
                    //    xrTableRowABTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo Córdobas A + B ", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    _TotalEfectivoCordobas = _TotalAMonedas + _TotalBcordobas;
                    //    xrTableRowABTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalEfectivoCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowABTotal);

                    //    #region C-DOLARES
                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowTipoCambio = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Titulo
                    //    xrTableRowTipoCambio.Weight = 1D;
                    //    xrTableRowTipoCambio.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("Tipo de Cambio", 200f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell(ThisTipoCambio.ToString("#,0.0000"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Goldenrod, Color.White));
                    //    xrTableRowTipoCambio.Cells.Add(new Parametros.MyXRTableCell("", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));

                    //    xrTableAP.Rows.Add(xrTableRowTipoCambio);


                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowCTitulo = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Titulo
                    //    xrTableRowCTitulo.Weight = 1D;
                    //    xrTableRowCTitulo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowCTitulo.Cells.Add(new Parametros.MyXRTableCell("C - BILLETES DOLARES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowCTitulo);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowCColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Fila Columnas
                    //    xrTableRowCColumnas.Weight = 1D;
                    //    xrTableRowCColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Cantidad", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Denominación", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableRowCColumnas.Cells.Add(new Parametros.MyXRTableCell("Total Efectivo", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowCColumnas);

                    //    foreach (var objC in db.Efectivos.Where(ef => ef.MonedaID.Equals(MonedaSecundaria) && ef.EsBillete.Equals(true)).OrderByDescending(o => o.Equivalente))
                    //    {
                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowCValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Filas Valores
                    //        xrTableRowCValor.Weight = 1D;
                    //        xrTableRowCValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //        var AEDAnterior = db.ArqueoEfectivoDetalles.Where(x => x.ArqueoEfectivoID.Equals(IDAE) && x.EfectivoID.Equals(objC.ID));
                    //        if (AEDAnterior.Count() > 0)
                    //        {
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().Valor.ToString("#,0.00"), 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(AEDAnterior.First().TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            _TotalEfectivoDolares += AEDAnterior.First().TotalEfectivo;
                    //        }
                    //        else
                    //        {
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("", 200, DevExpress.XtraPrinting.TextAlignment.MiddleLeft, Color.White, Color.Black));
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell(objC.Denominacion, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowCValor.Cells.Add(new Parametros.MyXRTableCell("-", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //        }
                    //        xrTableAP.Rows.Add(xrTableRowCValor);

                    //    }

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowCTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total C-Dolares
                    //    xrTableRowCTotal.Weight = 1D;
                    //    xrTableRowCTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("Total A - Billetes Dólares", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    xrTableRowCTotal.Cells.Add(new Parametros.MyXRTableCell("$ " + _TotalEfectivoDolares.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowCTotal);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowDolarACordobas = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total Dolares a Cordobas
                    //    xrTableRowDolarACordobas.Weight = 1D;
                    //    xrTableRowDolarACordobas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("Dólares Equivalente a Córdobas", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //    _TotalDolaresEquivalenteCordobas = _TotalEfectivoDolares * ThisTipoCambio;
                    //    xrTableRowDolarACordobas.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalDolaresEquivalenteCordobas.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowDolarACordobas);

                    //    #endregion

                    //    #region CHEQUES
                    //    if (AEView.Count(ch => ch.EsCheque) > 0)
                    //    {
                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowChequeTit = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Fila Titulo
                    //        xrTableRowChequeTit.Weight = 1D;
                    //        xrTableRowChequeTit.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //        xrTableRowChequeTit.Cells.Add(new Parametros.MyXRTableCell("D - CHEQUES", 500f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //        xrTableAP.Rows.Add(xrTableRowChequeTit);

                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowDColumnas = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Fila Columnas
                    //        xrTableRowDColumnas.Weight = 1D;
                    //        xrTableRowDColumnas.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //        xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Número de Cheque", 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //        xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Banco", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //        xrTableRowDColumnas.Cells.Add(new Parametros.MyXRTableCell("Monto", 150f, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //        xrTableAP.Rows.Add(xrTableRowDColumnas);

                    //        AEView.Where(c => c.EsCheque).ToList().ForEach(ch =>
                    //        {
                    //            DevExpress.XtraReports.UI.XRTableRow xrTableRowChValor = new DevExpress.XtraReports.UI.XRTableRow();
                    //            //Filas Valores
                    //            xrTableRowChValor.Weight = 1D;
                    //            xrTableRowChValor.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //            | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));

                    //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.NumeroCheque, 200, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.White, Color.Black));
                    //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.Banco, 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            xrTableRowChValor.Cells.Add(new Parametros.MyXRTableCell(ch.TotalEfectivo.ToString("#,0.00"), 150f, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.White, Color.Black));
                    //            _TotalCheques += ch.TotalEfectivo;

                    //            xrTableAP.Rows.Add(xrTableRowChValor);

                    //        });

                    //        DevExpress.XtraReports.UI.XRTableRow xrTableRowChTotal = new DevExpress.XtraReports.UI.XRTableRow();
                    //        //Filas Total C-Dolares
                    //        xrTableRowChTotal.Weight = 1D;
                    //        xrTableRowChTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //        | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //        xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("Total D - CHEQUES", 350, DevExpress.XtraPrinting.TextAlignment.MiddleCenter, Color.LightGray, Color.Black));
                    //        xrTableRowChTotal.Cells.Add(new Parametros.MyXRTableCell("C$ " + _TotalCheques.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //        xrTableAP.Rows.Add(xrTableRowChTotal);

                    //    }
                    //    #endregion

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalEfectivo = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total Dolares a Cordobas
                    //    xrTableRowTotalEfectivo.Weight = 1D;
                    //    xrTableRowTotalEfectivo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("TOTAL EFECTIVO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    _GRANDTOTAL = _TotalEfectivoCordobas + _TotalDolaresEquivalenteCordobas + _TotalCheques;
                    //    xrTableRowTotalEfectivo.Cells.Add(new Parametros.MyXRTableCell("C$ " + _GRANDTOTAL.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowTotalEfectivo);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowTotalArqueo = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Total Arqueo
                    //    xrTableRowTotalArqueo.Weight = 1D;
                    //    xrTableRowTotalArqueo.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("TOTAL ARQUEO", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableRowTotalArqueo.Cells.Add(new Parametros.MyXRTableCell("C$ " + spTotalArqueo.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));
                    //    xrTableAP.Rows.Add(xrTableRowTotalArqueo);

                    //    DevExpress.XtraReports.UI.XRTableRow xrTableRowDiferencia = new DevExpress.XtraReports.UI.XRTableRow();
                    //    //Filas Diferencia
                    //    xrTableRowDiferencia.Weight = 1D;
                    //    xrTableRowDiferencia.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                    //    | DevExpress.XtraPrinting.BorderSide.Right) | DevExpress.XtraPrinting.BorderSide.Bottom)));
                    //    xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("Diferencia en Efectivo", 350, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                    //    if (spDiferencia.Value < 0)
                    //        xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + spDiferencia.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.Red, Color.White));
                    //    else
                    //        xrTableRowDiferencia.Cells.Add(new Parametros.MyXRTableCell("C$ " + spDiferencia.Value.ToString("#,0.00"), 150, DevExpress.XtraPrinting.TextAlignment.MiddleRight, Color.LightGray, Color.Black));

                    //    xrTableAP.Rows.Add(xrTableRowDiferencia);

                    //    rep.GroupHeaderArqueo.Controls.Add(xrTableAP);

                    //    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                    //    rv.MdiParent = this.MdiParent;

                    //    rep.RequestParameters = true;

                    //    rep.CreateDocument();

                    }
                }
                else
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGARQUEOSISLASABIERTOS, Parametros.MsgType.warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                //Parametros.General.splashScreenManagerMain.CloseWaitForm();
                //this.Cursor = Cursors.Default;
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void spTotalCordoba_EditValueChanged(object sender, EventArgs e)
        {
            if (btnOK.Enabled.Equals(false))
                btnOK.Enabled = true;
        }

        private void DialogArqueoEfectivo_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnOK_EnabledChanged(object sender, EventArgs e)
        {
            if (this.btnOK.Enabled.Equals(false))
            {
                this.btnFinalizar.Enabled = true;
                this.btnPreview.Enabled = true;
            }
            else if (this.btnOK.Enabled.Equals(true))
            {
                this.btnFinalizar.Enabled = false;
                this.btnPreview.Enabled = false;
            }

        }
     
    }


}