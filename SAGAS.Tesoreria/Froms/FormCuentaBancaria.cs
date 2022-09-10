using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views;
using DevExpress.XtraReports.UI;
using DevExpress.Data.Linq;
using DevExpress.XtraGrid;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess;

namespace SAGAS.Tesoreria.Forms
{
    public partial class FormCuentaBancaria : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogCuentaBancaria nf;
        private Forms.Dialogs.DialogRemesa nfr;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        private int IDES = Parametros.General.EstacionServicioID;
        private List<int> lista;
        private int _Cuenta = 0;
        
        #endregion

        #region <<< INICIO >>>

        public FormCuentaBancaria()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Anular";
            this.barImprimir.Caption = "Vista previa";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.linqInstantFeedbackSource1.DesignTimeElementType = typeof(SAGAS.Entidad.VistaCuentaBancaria);
            this.linqInstantFeedbackSource1.KeyExpression = "[ID]";
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            FillControl(false);            
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl(bool Refresh)
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                    
                if (Refresh)
                    linqInstantFeedbackSource1.Refresh();
                else
                {
                    lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                                        
                    //DateTime fecha = new DateTime(Convert.ToDateTime(db.GetDateServer()).Year, Convert.ToDateTime(db.GetDateServer()).Month, 01);
                    //this.gvData.ActiveFilterString = (new OperandProperty("Fecha") > fecha.AddDays(-1)).ToString();

                    this.linqInstantFeedbackSourceRemesa.DesignTimeElementType = typeof(SAGAS.Entidad.VistaRemesa);
                    this.linqInstantFeedbackSourceRemesa.KeyExpression = "[ID]";

                    grid.ForceInitialize();
                    gvData.FocusedColumn = this.gvData.VisibleColumns[0];
                    gvData.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;

                    #region <<<   PERMISOS   >>>

                    btnNewRemesa.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnNewRemesa");
                    btnEditRemesa.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnEditRemesa");
                    btnDeleteRemesa.Visible = Parametros.General.SystemOptionAcces(Usuario, "btnDeleteRemesa");

                    #endregion
                }
                
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void linqInstantFeedbackSource1_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var query = from iv in dv.VistaCuentaBancarias.Where(v => v.CuentaBancariaActivo && v.BancoActivo)
                            where lista.Contains(iv.EstacionServicioID) 
                            select iv;

                e.QueryableSource = query;
                e.Tag = dv;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }

        }

        private void linqInstantFeedbackSource1_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }

       

        protected override void Imprimir()
        {
            
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                var obj = db.CuentaBancarias.Where(cb => cb.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)))).First();//((Entidad.Reporte)((Parametros.MyBarButtonItem)(e.Item)).Tag);

                if (!String.IsNullOrEmpty(obj.Extension))
                {

                    if (obj.Extension.Equals(".repx"))
                    {
                        //Reporte en DevExpress
                        Reportes.FormReportViewer rv = new Reportes.FormReportViewer();

                        System.IO.MemoryStream ms = new System.IO.MemoryStream(obj.Archivo);

                        DevExpress.XtraReports.UI.XtraReport rep = DevExpress.XtraReports.UI.XtraReport.FromStream(ms, true);


                                    rv.Text = obj.Nombre;


                                    var ds = rep.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
                                    //Asignar la nueva cadena de conexion
                                    ds.Connection.ConnectionString = Parametros.Config.GetCadenaConexionString() + @"; Network Library=DBMSSOCN; connection timeout=900";
                                    //ds.Connection.Open();

                                    //var Vista = dv.VistaMovimientos.FirstOrDefault(o => o.CuentaBancariaID.Equals(obj.ID) && !o.Anulado);

                                    //if (Vista != null)
                                    //{

                                        rep.DataSource = ds;
                                        rep.Parameters["ParametroID"].Value = 39783;
                                        //rep.Param.Visible = false;
                                    //}
                                    //else
                                    //{
                                    //    rep.DataSource = db.Movimientos.Where(m => m.ID.Equals(0));
                                    //    rep.CreateDocument(true);
                                    //}


                                    rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                                    rv.Owner = this;
                                    rv.MdiParent = this.MdiParent;
                                    rep.CreateDocument(true);
                                    rv.Show();

                    }
                    /*else if (obj.Extension.Equals(".rpt"))
                    {

                        //Reportes en Crystal

                        if (!System.IO.Directory.Exists(routeRpt))
                            System.IO.Directory.CreateDirectory(routeRpt);
                        routeRpt += @"\" + @obj.ArchivoNombre + obj.Extension;
                        File.WriteAllBytes(routeRpt, obj.Archivo);

                        if (System.IO.File.Exists(@routeRpt))
                        {
                            Reportes.FormCrystalViewer CR = new Reportes.FormCrystalViewer();
                            CR.Text = obj.Nombre;
                            CR.Tag = @routeRpt;
                            CR.crystalReportViewer1.ReportSource = @routeRpt;
                            CR.Refresh();
                            CR.Owner = this;
                            CR.MdiParent = this;
                            CR.Show();
                        }
                    }
                    else
                    {
                        Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);
                    }*/
                }
                else
                    Parametros.General.DialogMsg("El reporte no tiene una extensión valida.", Parametros.MsgType.warning);
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG, bool EsRemesa, bool Detail)
        {
            if (EsRemesa)
                nfr = null;
            else
                nf = null;
            
            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if (Detail)
                gvData_FocusedRowChanged(null, null);
            else
                FillControl(true);
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    nf = new Forms.Dialogs.DialogCuentaBancaria(Usuario);
                    nf.Text = "Crear Nueva Cuenta Bancaria";
                    nf.Owner = this;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        nf = new Forms.Dialogs.DialogCuentaBancaria(Usuario);
                        nf.Text = "Editar Cuenta Bancaria";
                        nf.EntidadAnterior = db.CuentaBancarias.Single(e => e.ID.Equals(Convert.ToInt32(gvData.GetFocusedRowCellValue(colID))));
                        nf.Owner = this;
                        nf.Editable = true;
                        nf.MDI = this;
                        nf.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
            
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    //if (treeTipoCuenta.FocusedNode != null)
                    //{
                    //    int id = Convert.ToInt32(treeTipoCuenta.FocusedNode.GetValue(colID));

                    //    if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                    //    {

                    //        var tc = db.TipoCuentas.Single(c => c.ID == id);

                    //        if (Convert.ToInt32(db.CuentaContables.Where(o => o.IDTipoCuenta.Equals(tc.ID) && o.Activo).Count()) > 0)
                    //        {
                    //            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                    //            return;
                    //        }

                    //        //Desactivar Registro 
                    //        tc.Activo = false;
                    //        db.SubmitChanges();

                    //        Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se Inactivó el Tipo de Cuenta: " + tc.Nombre, this.Name);

                    //        if (ShowMsgDialog)
                    //            Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                    //        else
                    //            this.timerMSG.Start(); 

                    //        FillControl();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        private void gvData_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                try
                {
                    if (gvData.GetFocusedRowCellValue(colID).GetType() != typeof(DevExpress.Data.NotLoadedObject))
                    {
                        gridRemesa.DataSource = null;
                        _Cuenta = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        //linqInstantFeedbackSourceRemesa_GetQueryable(cuenta, new GetQueryableEventArgs());
                        gridRemesa.DataSource = linqInstantFeedbackSourceRemesa;
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        #region <<< REMESAS >>>

        //CONSULTA REMESAS
        private void linqInstantFeedbackSourceRemesa_GetQueryable(object sender, GetQueryableEventArgs e)
        {
            try
                {
                    dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                    var query = from iv in dv.VistaRemesas.Where(v => v.CuentaBancariaID.Equals(_Cuenta))
                                select iv;

                    e.QueryableSource = query;
                    e.Tag = dv;
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
        }

        private void linqInstantFeedbackSourceRemesa_DismissQueryable(object sender, GetQueryableEventArgs e)
        {
            if (((Entidad.SAGASDataViewsDataContext)e.Tag) != null)
                ((Entidad.SAGASDataViewsDataContext)e.Tag).Dispose();
        }
        
        private void btnNewRemesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (nfr == null && !_Cuenta.Equals(0))
                {
                    nfr = new Forms.Dialogs.DialogRemesa(Usuario);
                    nfr.Text = "Crear Nueva Remesa";
                    nfr.Cuenta = _Cuenta;
                    nfr.Owner = this;
                    nfr.MDI = this;
                    nfr.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
            }
        }

        #endregion

        private void btnEditRemesa_Click(object sender, EventArgs e)
        {
            if (gvRemesa.FocusedRowHandle >= 0)
            {
                try
                {
                    if (gvRemesa.GetFocusedRowCellValue(colID).GetType() != typeof(DevExpress.Data.NotLoadedObject))
                    {
                        if (nfr == null)
                        {
                            //var orderDetail = gvOrdenes.GetFocusedRow() as dsSystem.Entity.OrderDetail;
                            nfr = new Forms.Dialogs.DialogRemesa(Usuario);
                            nfr.Text = "Editar Remesa";
                            nfr.EntidadAnterior = db.Remesas.Single(r => r.ID.Equals(Convert.ToInt32(gvRemesa.GetFocusedRowCellValue("ID"))));
                            nfr.Editable = true;
                            nfr.Owner = this;
                            nfr.MDI = this;
                            nfr.Show();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
                }
            }
        }
        
        private void btnDeleteRemesa_Click(object sender, EventArgs e)
        {
            if (nf == null)
            {
                if (gvRemesa.FocusedRowHandle >= 0)
                {
                    try
                    {
                        if (gvRemesa.GetFocusedRowCellValue(colID).GetType() != typeof(DevExpress.Data.NotLoadedObject))
                        {
                            Entidad.SAGASDataClassesDataContext db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                            int id = Convert.ToInt32(gvRemesa.GetFocusedRowCellValue(colID));

                            Entidad.Remesa R = db.Remesas.Single(r => r.ID.Equals(id));

                            if (R.EnUso.Equals(true))
                            {
                                Parametros.General.DialogMsg("La Remesa seleccionada esta en uso, no se puede eliminar", Parametros.MsgType.warning);
                                return;
                            }

                            if (R.Usada.Equals(true))
                            {
                                Parametros.General.DialogMsg("La Remesa seleccionada fue utilizada y tiene registros relacionados, no se puede eliminar", Parametros.MsgType.warning);
                                return;
                            }

                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROBORRAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                string consecutivo = R.Consecutivo.ToString();
                                db.Remesas.DeleteOnSubmit(R);
                                db.SubmitChanges();
                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Contabilidad, "Se Eliminó La Remesa: " + consecutivo, this.Name);

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                gvData_FocusedRowChanged(null, null);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                    }
                }
            }
        }

        #endregion
    }
}
