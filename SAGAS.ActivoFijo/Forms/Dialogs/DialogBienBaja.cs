using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.ActivoFijo.Forms.Dialogs
{
    public partial class DialogBienBaja : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        internal Forms.FormBien MDI;
        internal Entidad.VistaBienes EntidadAnterior;
        internal bool Editable = false;
        private bool ShowMsg = false;
        internal bool RefreshMDI = false;
        internal DateTime vFecha;
        internal int _ID = 0;
        internal decimal _TipoCambio = 0;
        private int IDEstacionServicio = 0;
        private int IDSubEstacion = 0;
        DXErrorProvider errRequiredField = new DXErrorProvider();
        

        //public int Codigo
        //{
        //    get { return Convert.ToInt32(txtCodigo.Text); }
        //    set { txtCodigo.EditValue = value; }
        //}

        //public string Nombre
        //{
        //    get { return txtNombre.Text; }
        //    set { txtNombre.Text = value; }
        //}
        
        //public string Descripcion
        //{
        //    get { return mmoDescripcion.Text; }
        //    set { mmoDescripcion.Text = value; }
        //}

        //public bool EsTangible 
        //{
        //    get { return chkTangible.Checked; }
        //    set { chkTangible.Checked = value; }        
        //}

        //public bool EsDepreciable
        //{
        //    get { return chkDepreciable.Checked; }
        //    set { chkDepreciable.Checked = value; }
        //}

        //public int CuentaActivo 
        //{
        //    get { return Convert.ToInt32(this.glkCuentaActivo.EditValue); }
        //    set { this.glkCuentaActivo.EditValue = value; }        
        //}

        //public int CuentaGasto
        //{
        //    get { return Convert.ToInt32(this.glkCuentaGasto.EditValue); }
        //    set { this.glkCuentaGasto.EditValue = value; }
        //}
        
        //public int CuentaDepreciacionAcumulada
        //{
        //    get { return Convert.ToInt32(this.glkCuentaDepreciacionAcumulada.EditValue); }
        //    set { this.glkCuentaDepreciacionAcumulada.EditValue = value; }
        //}

        //public int VidaUtil
        //{
        //    get { return Convert.ToInt32(this.seVidaUtil.Value); }
        //    set { this.seVidaUtil.Value = value; }
        //}


        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private DataTable DetalleComprobante;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        
        #endregion

        public DialogBienBaja(int id)
        {
            InitializeComponent();
            _ID = id;
        }
        
        private void DialogBienEdit_Shown(object sender, EventArgs e)
        {
            FillControl();
        }

        #region *** METODOS ***

        private void FillControl()
        {
            try
            {
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), (Editable ? Parametros.Properties.Resources.TXTCARGANDO : Parametros.Properties.Resources.TXTFORMULARIO));
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                Entidad.SAGASDataViewsDataContext dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                vFecha = Convert.ToDateTime(db.GetDateServer());
                dateFecha.DateTime = vFecha.Date;
                dateFecha_Validated(dateFecha, null);
                lkArea.Properties.DataSource = db.CentroCostos.Where(c => c.Activo).Select(s => new { s.ID, s.Nombre });

                EntidadAnterior = dv.VistaBienes.Single(s => s.ID.Equals(_ID));

                txtComentario.Text = EntidadAnterior.Descripcion;
                txtCodigo.Text = EntidadAnterior.Codigo.ToString();
                txtNombre.Text = EntidadAnterior.Nombre;
                txtActivo.Text = EntidadAnterior.ActivoNombre;
                txtTipoActivo.Text = EntidadAnterior.TipoActivoNombre;
                txtNroSerie.Text = EntidadAnterior.NoSerie;
                txtMarca.Text = EntidadAnterior.Marca;
                txtModelo.Text = EntidadAnterior.Modelo;
                txtMatricula.Text = EntidadAnterior.Matricula;
                txtChasis.Text = EntidadAnterior.Chasis;
                txtMotor.Text = EntidadAnterior.Motor;
                txtFecha.Text = EntidadAnterior.FechaAdquisicion.ToShortDateString();
                txtFactura.Text = EntidadAnterior.NoFactura;
                txtValorAdquisicion.Text = EntidadAnterior.ValorAdquisicion.ToString("#,#.00");
                txtDepreciacionAcumulada.Text = EntidadAnterior.ValorDepreActual.ToString("#,#.00");

                txtAsignado.Text = EntidadAnterior.UsuarioAsignado;
                lkArea.EditValue = EntidadAnterior.AreaID;
                txtEstacionAnterior.Text = (EntidadAnterior.SubEstacionID > 0 ? db.SubEstacions.Single(s => s.ID.Equals(EntidadAnterior.SubEstacionID)).Nombre : db.EstacionServicios.Single(s => s.ID.Equals(EntidadAnterior.EstacionID)).Nombre);
                IDEstacionServicio = EntidadAnterior.EstacionID;
                IDSubEstacion = EntidadAnterior.SubEstacionID;

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
                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

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

                this.bdsDetalleOtros.DataSource = this.DetalleComprobante;

                Parametros.General.splashScreenManagerMain.CloseWaitForm();
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        public bool ValidarCampos()
        {

            DateTime vFechaV = new DateTime(dateFecha.DateTime.Year, dateFecha.DateTime.Month, 1).AddMonths(1).AddDays(-1);

            DateTime FechaAnterior = new DateTime(vFechaV.Year, vFechaV.Month, 01);

                if (_TipoCambio <= 0)
                {
                    Parametros.General.DialogMsg("No existe tipo de cambio para la fecha:  " + dateFecha.DateTime.ToShortDateString() + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }

                if (String.IsNullOrEmpty(memoComentario.Text))
                {
                    Parametros.General.DialogMsg("Debe ingresar un comentario para la baja del bien." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }


                if (!Parametros.General.ValidatePeriodoContable(dateFecha.DateTime, db, EntidadAnterior.EstacionID))
                {
                    Parametros.General.DialogMsg("El Periodo contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    return false;
                }

                if (db.Movimientos.Count(m => m.MovimientoTipoID == 75 && !m.Anulado && m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && (m.FechaRegistro.Date > FechaAnterior.Date && m.FechaRegistro.Date <= vFechaV.Date)) >= 1)
                {
                    Parametros.General.DialogMsg("Ya existe una depreciacion para este periodo", Parametros.MsgType.warning);
                    return false;
                }
            
            return true;
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


        private bool Guardar()
        {
            if (!ValidarCampos()) return false;

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);
                    Entidad.Bien EB = db.Bien.Single(s => s.ID.Equals(EntidadAnterior.ID));

                    EB.Activo = false;

                    db.SubmitChanges();

                        Entidad.Movimiento M = new Entidad.Movimiento();
                        M.MovimientoTipoID = 78;

                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.MonedaID = Parametros.Config.MonedaPrincipal();
                        M.TipoCambio = _TipoCambio;
                        M.UsuarioID = Parametros.General.UserID;

                        M.FechaRegistro = dateFecha.DateTime.Date;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && m.SubEstacionID.Equals(EntidadAnterior.SubEstacionID) && m.MovimientoTipoID.Equals(78)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(EntidadAnterior.EstacionID) && m.SubEstacionID.Equals(EntidadAnterior.SubEstacionID) && m.MovimientoTipoID.Equals(78)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                        M.Numero = number;

                        M.EstacionServicioID = EntidadAnterior.EstacionID;
                        M.SubEstacionID = EntidadAnterior.SubEstacionID;
                        M.Monto = Decimal.Round(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion, 2, MidpointRounding.AwayFromZero);
                        M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / M.TipoCambio, 2, MidpointRounding.AwayFromZero);
                        db.Movimientos.InsertOnSubmit(M);
                        db.SubmitChanges();

                        #region <<< REGISTRANDO COMPROBANTE >>>

                        List<Entidad.ComprobanteContable> Compronbante = PartidasContable;

                        //var objCC = from cds in Compronbante
                        //            join cc in db.CuentaContables on cds.CuentaContableID equals cc.ID
                        //            join tc in db.TipoCuentas on cc.IDTipoCuenta equals tc.ID
                        //            select new
                        //            {
                        //                Debito = (cds.Monto > 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                        //                Credito = (cds.Monto < 0 ? Math.Abs(Decimal.Round(cds.Monto, 2, MidpointRounding.AwayFromZero)) : 0),
                        //            };

                        //if (!(Decimal.Round(objCC.Sum(s => s.Debito), 2, MidpointRounding.AwayFromZero) - Decimal.Round((objCC.Sum(s => s.Credito)), 2, MidpointRounding.AwayFromZero)).Equals(0))
                        if (!Compronbante.Sum(s => s.Monto).Equals(0))
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
                        EB.MovimientoBajaID = M.ID;
                        EB.FechaLiquidacion = M.FechaRegistro;
                    
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se dio de Baja al Activo: " + EntidadAnterior.Nombre, this.Name);//, dtPosterior, dtAnterior);

                    db.SubmitChanges();
                    trans.Commit();

                    RefreshMDI = true;
                    ShowMsg = true;
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    return true;
                }

                catch (Exception ex)
                {
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
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

                     int cuenta = db.TipoActivo.Single(s => s.ID.Equals(EntidadAnterior.TipoActivoID)).CuentaActivo;

                    //Depreciación

                    CD.Add(new Entidad.ComprobanteContable
                    {
                        CuentaContableID = EntidadAnterior.CuentaActivo,
                        Monto = Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion),
                        TipoCambio = _TipoCambio,
                        MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorDepreActual > 0 ? EntidadAnterior.ValorDepreActual : EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero),
                        Fecha = dateFecha.DateTime.Date,
                        Descripcion = memoComentario.Text,
                        Linea = i,
                        EstacionServicioID = EntidadAnterior.EstacionID,
                        SubEstacionID = EntidadAnterior.SubEstacionID
                    });
                    i++;

                    //Adquisición
                    CD.Add(new Entidad.ComprobanteContable
                    {
                        CuentaContableID = cuenta,
                        Monto = -Math.Abs(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion)),
                        TipoCambio = _TipoCambio,
                        MontoMonedaSecundaria = -Math.Abs(Decimal.Round(Convert.ToDecimal(Convert.ToDecimal(EntidadAnterior.ValorAdquisicion) / _TipoCambio), 2, MidpointRounding.AwayFromZero)),
                        Fecha = dateFecha.DateTime.Date,
                        Descripcion = memoComentario.Text,
                        Linea = i,
                        EstacionServicioID = EntidadAnterior.EstacionID,
                        SubEstacionID = EntidadAnterior.SubEstacionID
                    });
                    i++;

                    foreach (DataRow linea in DetalleComprobante.Rows)
                    {

                        CD.Add(new Entidad.ComprobanteContable
                        {
                            CuentaContableID = Convert.ToInt32(linea["CuentaContableID"]),
                            Monto = Convert.ToDecimal(Convert.ToDecimal(linea["Monto"])),
                            TipoCambio = _TipoCambio,
                            MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal((Convert.ToDecimal(linea["Monto"]) / _TipoCambio)), 2, MidpointRounding.AwayFromZero),
                            Fecha = dateFecha.DateTime.Date,
                            Descripcion = Convert.ToString(linea["Descripcion"]),
                            Linea = i,
                            CentroCostoID = Convert.ToInt32(linea["CentroCostoID"]),
                            EstacionServicioID = IDEstacionServicio,
                            SubEstacionID = IDSubEstacion
                        });
                        i++;
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

        #endregion

        #region *** EVENTOS ***

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Parametros.General.DialogMsg("¿Desea guardar el registro de la baja del bien?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
            {
                if (!Guardar()) return;

                this.Close();
            }
        }

        private void DialogZona_FormClosed(object sender, FormClosedEventArgs e)
        {
            MDI.CleanDialog(ShowMsg, false, RefreshMDI, false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_Validated(object sender, EventArgs e)
        {
            Parametros.General.ValidateEmptyStringRule((DevExpress.XtraEditors.TextEdit)sender, errRequiredField);
        }

        private void btnShowComprobante_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) return;

                using (Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES nf = new Contabilidad.Forms.Dialogs.DialogShowComprobanteMultiES())
                {
                    nf.DetalleCD = PartidasContable;
                    nf.Text = "Comprobante Contable Baja de Activo";
                    nf.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void dateFecha_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(dateFecha.DateTime.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(dateFecha.DateTime.Date)).First().Valor : 0m);

        }


        #endregion

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {

        }

        //Mensaje Validación del detalle
        private void bgvCuentas_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void bgvCuentas_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
         
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID") != DBNull.Value)
                {
                    if (Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(bgvCuentas.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Nombre", linea.Display);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "UsaCentroCosto", linea.valor);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    bgvCuentas.SetRowCellValue(e.RowHandle, "Linea", 0);

                    if (bgvCuentas.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(bgvCuentas.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        bgvCuentas.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(bgvCuentas.GetRowCellValue(bgvCuentas.RowCount - 2, "Descripcion")));
                    }

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            //GetDiferencias(true);

            #endregion
        
        }

        //metodos para borrar y agregar nueva fila
        private void bgvCuentas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                base.OnKeyUp(e);
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);

                        bgvCuentas.RefreshData();
                        //GetDiferencias(true);
                    }

                }
            }

            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridCuentas.DefaultView;
                view.AddNewRow();
                bgvCuentas.RefreshData();
            }
        }

        private void bgvCuentas_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (bgvCuentas.FocusedColumn == colCentroCosto)
            {
                if (bgvCuentas.GetFocusedRowCellValue(colUsaCto) == DBNull.Value)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(bgvCuentas.GetFocusedRowCellValue(colUsaCto)))
                        e.Cancel = true;
                }
            }
        }

        //Validar Filas de las Cuentas
        private void bgvCuentas_ValidateRow(object sender, ValidateRowEventArgs e)
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

            //GetDiferencias(true);
        }


    }
}
