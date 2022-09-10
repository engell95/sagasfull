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
using SAGAS.Contabilidad.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.DXErrorProvider;

namespace SAGAS.Contabilidad.Forms.Dialogs
{
    public partial class DialogComprobanteContable : Form
    {
        #region *** DECLARACIONES ***

        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        internal Forms.FormComprobanteContable MDI;
        internal Entidad.Movimiento EntidadAnterior;
        private bool ShowMsg = false;
        private int UsuarioID;
        internal bool _Editable;
        private bool NextProvision = false;
        private bool RefreshMDI = false;
        private int IDEstacionServicio = Parametros.General.EstacionServicioID;
        private int IDSubEstacion = Parametros.General.SubEstacionID; 
        private bool _Guardado = false;
        private bool _ToPrint = false;
        private int IDPrint = 0;
        private IQueryable<Parametros.ListIdDisplay> CentrosCostos;
        private int _CuentaIVACredito = Parametros.Config.IVAPorAcreditar();
        internal decimal vTotalPagar = 0m;
        internal int IDMonedaPrincipal;
        internal decimal _TipoCambio;
        
        private DateTime Fecha
        {
            get { return Convert.ToDateTime(dateFecha.EditValue); }
            set { dateFecha.EditValue = value; }
        }


        private int Referencia
        {
            get { return Convert.ToInt32(txtReferencia.Text); }
            set { txtReferencia.Text = value.ToString(); }
        }

        private string Comentario
        {
            get { return mmoComentario.Text; }
            set { mmoComentario.Text = value; }
        }
        
        private IQueryable<Parametros.ListIdDisplayCodeBool> Cuentas;
        private List<Entidad.VistaComprobante> CD;

        #endregion

        #region *** INICIO ***

        public DialogComprobanteContable(int UserID, bool IsEdit)
        {            
            InitializeComponent();
            UsuarioID = UserID;
            _Editable = IsEdit;
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
                Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GENERANDO.ToString(), Parametros.Properties.Resources.TXTFORMULARIO);
                
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                this.CentrosCostos = from cto in db.CentroCostos
                                      join ctoEs in db.CentroCostoPorEstacions on cto equals ctoEs.CentroCosto
                                      where ctoEs.EstacionID.Equals(IDEstacionServicio)
                                      select new Parametros.ListIdDisplay {ID = cto.ID, Display = cto.Nombre};

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
                
                int number = 1;
                if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(51)) > 0)
                {
                    number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(51)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                }

                Referencia = number;
                
                //--- Fill Combos Detalles --//
                gridCuenta.View.OptionsBehavior.AutoPopulateColumns = false;
                gridCuenta.DataSource = null;
                gridCuenta.DataSource = Cuentas;

                //Centro Costo GRID
                lkCentroCosto.DataSource = CentrosCostos;
                lkCentroCosto.DisplayMember = "Display";
                lkCentroCosto.ValueMember = "ID";

                dateFecha.EditValue = Convert.ToDateTime(db.GetDateServer());
                IDMonedaPrincipal = Parametros.Config.MonedaPrincipal();
                            
                gridCuenta.DisplayMember = "Codigo";
                gridCuenta.ValueMember = "ID";

                if (_Editable)
                {
                    Referencia = EntidadAnterior.Numero;
                    Fecha = EntidadAnterior.FechaRegistro;
                    Comentario = EntidadAnterior.Comentario;
                    CD = dv.VistaComprobantes.Where(v => v.MovimientoID.Equals(EntidadAnterior.ID)).OrderBy(o => o.Linea).ToList();
                    CD.ForEach(det => det.Litros = (det.CentroCostoID > 0 ? 1 : 0));
                }
                else
                    CD = dv.VistaComprobantes.Where(v => v.MovimientoID.Equals(0)).ToList();

                this.bdsDetalle.DataSource = CD;

                //if (_Editable)
                //{
                //    for (int i = 0; i < gvDetalle.RowCount -1; i++)
                //    {
                //        int x = CD.ElementAt(i).CentroCostoID;
                //        gvDetalle.SetRowCellValue(, colCentroCosto, x);
                //    }
                //}

                gvDetalle.RefreshData();
                Parametros.General.splashScreenManagerMain.CloseWaitForm(); ;
            }
            catch (Exception ex)
            {
                Parametros.General.splashScreenManagerMain.CloseWaitForm();
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }

        }

        private void ValidarerrRequiredField()
        {
            Parametros.General.ValidateEmptyStringRule(txtReferencia, errRequiredField);
            Parametros.General.ValidateEmptyStringRule(mmoComentario, errRequiredField);
        }

        private bool ValidarReferencia(int code, int? ID)
        {
            var result = (from i in db.Movimientos
                          where (ID.HasValue ? i.MovimientoTipoID.Equals(51) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio) && i.ID != Convert.ToInt32(ID) :
                          i.MovimientoTipoID.Equals(51) && i.Numero.Equals(code) && i.EstacionServicioID.Equals(IDEstacionServicio))
                          select i);

            if (result.Count() > 0)
            {
                return false;
            }
            return true;

        }
        
        public bool ValidarCampos(bool detalle)
        {
            if (dateFecha.EditValue == null || String.IsNullOrEmpty(mmoComentario.Text) || txtReferencia.Text == "")
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGREVISARDATOS + Environment.NewLine + "Complete el encabezado de la provisión.", Parametros.MsgType.warning);
                return false;
            }


            if (!Parametros.General.ValidateTipoCambio(dateFecha, errRequiredField, db))
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOTIPOCAMBIO, Parametros.MsgType.warning);
                return false;
            }

            if (!Parametros.General.ValidatePeriodoContable(Fecha, db, IDEstacionServicio))
            {
                Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
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
                if (CD.Count <= 0)
                {
                    Parametros.General.DialogMsg("Debe de ingresar detalle a la provisión." + Environment.NewLine, Parametros.MsgType.warning);
                    return false;
                }                                
            }

            return true;
        }

        private bool Guardar()
        {
            if (!ValidarCampos(false)) return false;

            if (!spDif.Value.Equals(0))
            {
                Parametros.General.DialogMsg("El comprobante contable no esta cuadrado." + Environment.NewLine, Parametros.MsgType.warning);
                return false;
            }

            db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                db.CommandTimeout = 600;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.GUARDANDO.ToString(), Parametros.Properties.Resources.TXTGUARDANDO);

                    Entidad.Movimiento M;

                    if (_Editable)
                    {
                        M = db.Movimientos.Single(e => e.ID == EntidadAnterior.ID);
                        M.Modificado = true;
                    }
                    else
                    {
                        M = new Entidad.Movimiento();

                        M.MovimientoTipoID = 51;
                        M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                        M.EstacionServicioID = IDEstacionServicio;
                        M.SubEstacionID = IDSubEstacion;

                        int number = 1;
                        if (db.Movimientos.Count(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(51)) > 0)
                        {
                            number = Parametros.General.ValorConsecutivo(db.Movimientos.Where(m => m.EstacionServicioID.Equals(Parametros.General.EstacionServicioID) && m.SubEstacionID.Equals(IDSubEstacion) && m.MovimientoTipoID.Equals(51)).OrderByDescending(o => o.Numero).First().Numero.ToString());
                        }
                        M.Numero = number;
                    }
                    
                    M.UsuarioID = UsuarioID;
                    M.Comentario = Comentario;                    
                    M.FechaRegistro = Fecha;
                    M.MonedaID = IDMonedaPrincipal;
                    M.TipoCambio = _TipoCambio;
                    M.Monto = Decimal.Round(Convert.ToDecimal(CD.Sum(s => s.Debito)), 2, MidpointRounding.AwayFromZero);
                    M.MontoMonedaSecundaria = Decimal.Round(Convert.ToDecimal(CD.Sum(s => s.Debito)), 2, MidpointRounding.AwayFromZero);
                    
                    

                    db.SubmitChanges();

                    if (_Editable)
                    {
                        DataTable dtPosterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(M, 1));
                        DataTable dtAnterior = Parametros.General.LINQToDataTable(Enumerable.Repeat(EntidadAnterior, 1));

                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                         "Se modificó el Comprobante Contable: " + EntidadAnterior.Numero, this.Name, dtPosterior, dtAnterior);

                    }
                    else
                    {
                        db.Movimientos.InsertOnSubmit(M);
                        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad,
                        "Se registró el Comprobante Contable: " + M.Numero, this.Name);
                    }


                    #region <<< REGISTRANDO COMPROBANTE >>>
                    List<Entidad.ComprobanteContable> Compronbante = (from x in CD
                                                                     select new Entidad.ComprobanteContable
                                                                     {
                                                                         CuentaContableID = x.CuentaContableID,
                                                                         Monto = Decimal.Round((x.Debito.HasValue ? Convert.ToDecimal(x.Debito) : 0m) - (x.Credito.HasValue ? Convert.ToDecimal(x.Credito) : 0m), 2, MidpointRounding.AwayFromZero),
                                                                         TipoCambio = _TipoCambio,
                                                                         MontoMonedaSecundaria = Decimal.Round(((x.Debito.HasValue ? Convert.ToDecimal(x.Debito) : 0m) - (x.Credito.HasValue ? Convert.ToDecimal(x.Credito) : 0m)) / _TipoCambio , 2, MidpointRounding.AwayFromZero),
                                                                         Fecha = Fecha,
                                                                         Descripcion = x.Descripcion,
                                                                         CentroCostoID = x.CentroCostoID,
                                                                         EstacionServicioID = IDEstacionServicio,
                                                                         SubEstacionID = IDSubEstacion
                                                                     }).ToList();

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
                        return false;
                    }

                    M.ComprobanteContables.Clear();

                    int l = 1;
                    Compronbante.ForEach(linea =>
                        {
                            linea.Linea = l;
                            M.ComprobanteContables.Add(linea);
                            l++;
                        });

                    #endregion

                    db.SubmitChanges();
                    trans.Commit();

                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    NextProvision = true;
                    RefreshMDI = true;
                    ShowMsg = true;
                    _Guardado = true;
                    _ToPrint = true;
                    IDPrint = M.ID;
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
            MDI.CleanDialog(ShowMsg, NextProvision, RefreshMDI, IDPrint);
        }
        
        //Validando cierre del formulario
        private void DialogCompras_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_Guardado && !NextProvision)
            {
                if (CD.Count > 0)
                {
                    if (Parametros.General.DialogMsg("El Comprobante actual tiene datos registrados. ¿Desea cerrar la ventana?", Parametros.MsgType.question) != System.Windows.Forms.DialogResult.OK)
                    {
                        e.Cancel = true;
                    }
                }
            }

            EntidadAnterior = null;
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
         
            //-- Validar Columna de Cuenta             
            if (view.GetRowCellValue(RowHandle, "CuentaContableID") != null)
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
            if (view.GetRowCellValue(RowHandle, "Descripcion") == null)
            {
                view.SetColumnError(view.Columns["Descripcion"], "Debe de escribir una descripción.");
                e.ErrorText = "Debe de escribir una descripción.";
                e.Valid = false;
                Validate = false;
            }

            //-- Validar Columna de Valor             
            if (view.GetRowCellValue(RowHandle, "Debito") == null && view.GetRowCellValue(RowHandle, "Credito") == null)
            {
                view.SetColumnError(view.Columns["Debito"], "Debe Ingresar el Debito en la linea.");
                e.ErrorText = "Debe Ingresar el Debito en la linea.";
                e.Valid = false;
                Validate = false;
            }
            else
            {
                if (Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Debito")).Equals(0) && Convert.ToDecimal(view.GetRowCellValue(RowHandle, "Credito")).Equals(0))
                {
                    view.SetColumnError(view.Columns["Debito"], "Debe Ingresar el Debito en la linea.");
                    e.ErrorText = "Debe Ingresar el Debito en la linea.";
                    e.Valid = false;
                    Validate = false;
                }
            }


            //-- Validar Columna de Centro de Costo             
            if (view.GetRowCellValue(RowHandle, "Litros") != null)
            {
                if (Convert.ToBoolean(view.GetRowCellValue(RowHandle, "Litros")).Equals(true))
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
                view.SetColumnError(view.Columns["Litros"], "Debe Seleccionar una Cuenta Contable");
                e.ErrorText = "Debe Seleccionar una Cuenta Contable";
                e.Valid = false;
                Validate = false;
            }
        }

        private void gvDetalle_InvalidRowException(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            Parametros.General.DialogMsg(e.ErrorText + Environment.NewLine + "Presione la tecla ESC para cancelar la operación de ingresar registro.", Parametros.MsgType.warning);
        }

        //Cambio de valores en las en las celdas del detalle
        private void gvDetalle_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            #region <<< COLUMNA_CUENTA_CONTABLE >>>
            //-- Especificar la Unidad de Medida Principal y Cantidad Inicial de Cero
            if (e.Column == colCuentaContableID)
            {
                if (gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID") != null)
                {
                    if (Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID")) == 0)
                    {
                        return;
                    }
                }

                try
                {
                    var linea = Cuentas.Single(c => c.ID.Equals(Convert.ToInt32(gvDetalle.GetRowCellValue(e.RowHandle, "CuentaContableID"))));
                    gvDetalle.SetRowCellValue(e.RowHandle, "CuentaNombre", linea.Display);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Litros", linea.valor);
                    gvDetalle.SetRowCellValue(e.RowHandle, "CentroCostoID", 0);
                    gvDetalle.SetRowCellValue(e.RowHandle, "Linea", 0);

                    
                    if (gvDetalle.RowCount > 1 & String.IsNullOrEmpty(Convert.ToString(gvDetalle.GetRowCellValue(e.RowHandle, "Descripcion"))))
                    {
                        gvDetalle.SetRowCellValue(e.RowHandle, "Descripcion", Convert.ToString(gvDetalle.GetRowCellValue(gvDetalle.RowCount - 2, "Descripcion")));
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }

            if (e.Column == colDebe)
            {
                if (e.Value != null)
                {
                    gvDetalle.SetRowCellValue(e.RowHandle, "Credito", null);
                }
                //if (gvDetalle.GetRowCellValue(e.RowHandle, "Monto") == DBNull.Value)
                //    gvDetalle.SetRowCellValue(e.RowHandle, "Monto", 0.00);
                //else
                //{
                //    gvDetalle.RefreshData();
                    
                //}
            }

            if (e.Column == colHaber)
            {
                if (e.Value != null)
                {
                    gvDetalle.SetRowCellValue(e.RowHandle, "Debito", null);
                }
            }

            //gvDetalle.RefreshData();

            decimal Dif = (CD.Sum(s => s.Debito).HasValue ? Convert.ToDecimal(CD.Sum(s => s.Debito)) : 0m) - (CD.Sum(s => s.Credito).HasValue ?  Convert.ToDecimal(CD.Sum(s => s.Credito)) : 0m);
            
            if (Dif.Equals(0))
            {
                spDif.Value = 0m;
                layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else if (!Dif.Equals(0))
            {
                spDif.Value = Dif;
                layoutControlItemDif.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }

            #endregion
        }
                
        //metodos para borrar y agregar nueva fila
        private void gvDetalle_KeyDown(object sender, KeyEventArgs e)
        {
                if (e.KeyCode == Keys.Down)
                {
                    e.Handled = true;
                    base.OnKeyUp(e);
                }

                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.OemMinus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    int RowHandle = view.FocusedRowHandle;
                    if (RowHandle >= 0)
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                            + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                        {
                            view.DeleteRow(view.FocusedRowHandle);
                            
                        }

                    }
                }

                if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
                {
                    DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                    view.AddNewRow();
                }
        }

        private void gridDetalle_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                int RowHandle = view.FocusedRowHandle;
                if (RowHandle >= 0)
                {
                    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                    {
                        view.DeleteRow(view.FocusedRowHandle);
                                               
                    }
                }
            }

            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                //DevExpress.XtraGrid.Views.Grid.GridView view = (DevExpress.XtraGrid.Views.Grid.GridView)this.gridDetalle.DefaultView;
                //int RowHandle = view.FocusedRowHandle;
                //CD.Insert(RowHandle, new Entidad.VistaComprobante());
                //gvDetalle.RefreshData();
                //gvDetalle.FocusedRowHandle = RowHandle;
                ////if (RowHandle >= 0)
                ////{
                ////    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + " : " + Environment.NewLine
                ////        + view.GetRowCellDisplayText(RowHandle, "CuentaContableID").ToString() + " " + view.GetRowCellDisplayText(RowHandle, "Descripcion").ToString(), Parametros.MsgType.question) == DialogResult.OK)
                ////    {
                ////        view.DeleteRow(view.FocusedRowHandle);

                ////    }
                ////}
            }
        }

        //Validando la asignacion de Centro de Costo
        private void gvDetalle_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (gvDetalle.FocusedColumn == colCentroCosto)
            {
                if (gvDetalle.GetFocusedRowCellValue(colLitros) == null)
                    e.Cancel = true;
                else
                {
                    if (!Convert.ToBoolean(gvDetalle.GetFocusedRowCellValue(colLitros)))
                        e.Cancel = true;
                }

            }
        }
        
        #endregion

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

        //Boton nueva provision
        private void bntNew_Click(object sender, EventArgs e)
        {
            if (CD.Count > 0)
            {
                if (Parametros.General.DialogMsg("El Comprobante actual tiene datos registrados. ¿Desea cancelar este comprobante y realizar uno nuevo?", Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                {
                    NextProvision = true;
                    RefreshMDI = false;
                    ShowMsg = false;
                    this.Close();
                }
            }        
        }   

        private void dateFechaCompra_Validated(object sender, EventArgs e)
        {
            if (!Parametros.General.ValidateTipoCambio((BaseEdit)sender, errRequiredField, db))
            {
                _TipoCambio = 0;
                dateFecha.ErrorText = Parametros.Properties.Resources.MSGNOTIPOCAMBIO;
            }
            else
                _TipoCambio = (db.TipoCambios.Count(tc => tc.Fecha.Date.Equals(Fecha.Date)) > 0 ?
                        db.TipoCambios.Where(tc => tc.Fecha.Date.Equals(Fecha.Date)).First().Valor : 0m);
        }

        #endregion

    }
}