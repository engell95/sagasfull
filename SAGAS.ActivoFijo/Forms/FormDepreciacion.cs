using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAGAS.Parametros;

namespace SAGAS.ActivoFijo.Forms
{
    public partial class FormDepreciacion : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogDepreciacion nf;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private int Usuario = Parametros.General.UserID;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private List<int> lista;

        #endregion


        public FormDepreciacion()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Anular";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;            
            FillControl();
        }


        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());

                lista = new List<int>(db.GetViewEstacionesServicioByUsers.Where(ges => ges.UsuarioID == Usuario).Select(s => s.EstacionServicioID));
                 
                bdsManejadorDatos.DataSource = from M in dv.VistaMovimientos
                                               where lista.Contains(M.EstacionServicioID) && (M.MovimientoTipoID.Equals(75) || M.MovimientoTipoID.Equals(76))
                                               select new
                                               {
                                                   M.ID,
                                                   M.EstacionNombre,
                                                   M.SubEstacionNombre,
                                                   M.FechaContabilizacion,
                                                   M.Numero,
                                                   M.Monto,
                                                   M.MovimientoTipoNombre,
                                                   M.Referencia,
                                                   M.Anulado,
                                                   M.MovimientoTipoID,
                                                   M.EstacionServicioID
                                               };


                this.grid.DataSource = bdsManejadorDatos;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        }

        protected override void Imprimir()
        {
            if (gvData.FocusedRowHandle >= 0)
                ImprimirComprobante(Convert.ToInt32(gvData.GetFocusedRowCellValue("ID")));
        }

        /// <summary>
        /// IMPRIMIR COMPROBANTE
        /// </summary>
        /// <param name="ID">ID del Momvimiento</param>
        private void ImprimirComprobante(int ID)
        {

            try
            {
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                var VM = dv.VistaMovimientos.Where(m => m.ID.Equals(ID)).ToList();
                Reportes.FormReportViewer rv = new Reportes.FormReportViewer();
                rv.Text = VM.First().Abreviatura + " " + VM.First().Numero;

                Reportes.ActivoFijo.Hojas.RptDepreciacion rep = new Reportes.ActivoFijo.Hojas.RptDepreciacion();

                string Nombre, Direccion, Telefono;
                System.Drawing.Image picture_LogoEmpresa;
                Parametros.General.GetCompanyData(out Nombre, out Direccion, out Telefono, out picture_LogoEmpresa);
                rep.PicLogo.Image = picture_LogoEmpresa;
                rep.CeEmpresa.Text = Nombre;
                rep.CeEstacion.Text = (VM.First().SubEstacionID > 0 ? VM.First().SubEstacionNombre : VM.First().EstacionNombre);
                rep.DataSource = VM;
                rep.xrSetDate.Text = Convert.ToDateTime(db.GetDateServer()).ToString();
                rv.printControlAreaReport.PrintingSystem = rep.PrintingSystem;
                rv.Owner = this.Owner;
                rv.MdiParent = this.MdiParent;
                rep.RequestParameters = false;
                rep.CreateDocument();

                rv.Show();

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

        internal void CleanDialog(bool ShowMSG, bool NextRegistro)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if (NextRegistro)
                Add();
            else
                FillControl();
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
                    nf = new Forms.Dialogs.DialogDepreciacion(Parametros.General.UserID);
                    nf.Text = "Crear Depreciación";
                    nf.Owner = this.Owner;
                    nf.MdiParent = this.MdiParent;
                    nf.MDI = this;
                    nf.Show();
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Edit()
        {
            try
            {
                if (nf == null)
                {
                    //if (gvData.FocusedRowHandle >= 0)
                    //{
                    //    nf = new Forms.Dialogs.DialogDepreciacion();
                    //    nf.Text = "Editar Tipo de Activo";
                    //    //nf.EntidadAnterior = db.TipoMovimientoActivo.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                    //    nf.Owner = this;
                    //    nf.Editable = true;
                    //    nf.MDI = this;
                    //    nf.Show();
                    //}
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        protected override void Del()
        {
            if (gvData.FocusedRowHandle >= 0)
            {
                if (Convert.ToBoolean(gvData.GetFocusedRowCellValue("Anulado")) || Convert.ToInt32(gvData.GetFocusedRowCellValue("MovimientoTipoID")).Equals(76))
                    Parametros.General.DialogMsg(Convert.ToString(gvData.GetFocusedRowCellValue("Numero")) + Environment.NewLine + Parametros.Properties.Resources.MSGYAANULADO, Parametros.MsgType.warning);
                else
                {
                    if (!Parametros.General.ValidatePeriodoContable(Convert.ToDateTime(gvData.GetFocusedRowCellValue("FechaContabilizacion")).Date, db, Convert.ToInt32(gvData.GetFocusedRowCellValue("EstacionServicioID"))))
                        Parametros.General.DialogMsg("El Periodo Contable para esta fecha ya esta cerrado.", Parametros.MsgType.warning);
                    else
                    {
                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROANULAR, Parametros.MsgType.question) == System.Windows.Forms.DialogResult.OK)
                        {
                            AnularDepreciacion(Convert.ToInt32(gvData.GetFocusedRowCellValue("ID")));
                            FillControl();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ANULAR DEPRECIACION
        /// </summary>
        /// <param name="ID">ID del Movimiento para anular</param>
        private void AnularDepreciacion(int ID)
        {
            if (db.Connection.State == ConnectionState.Closed) db.Connection.Open();

            using (System.Data.Common.DbTransaction trans = db.Connection.BeginTransaction())
            {
                db.Transaction = trans;
                try
                {
                    Parametros.General.ShowWaitSplash(this, Parametros.WaitFormCaption.ANULANDO.ToString(), Parametros.Properties.Resources.TXTANULANDO);

                    Entidad.Movimiento Manterior = db.Movimientos.Single(m => m.ID.Equals(ID));

                    Manterior.Anulado = true;
                    Manterior.FechaAnulado = Convert.ToDateTime(db.GetDateServer());
                    Manterior.UsuarioAnuladoID = Parametros.General.UserID;
                    db.SubmitChanges();

                    Entidad.Movimiento M = new Entidad.Movimiento();

                    M.EstacionServicioID = Manterior.EstacionServicioID;
                    M.SubEstacionID = Manterior.SubEstacionID;
                    M.MovimientoTipoID = 76;
                    M.UsuarioID = Parametros.General.UserID;
                    M.FechaCreado = Convert.ToDateTime(db.GetDateServer());
                    M.FechaRegistro = Manterior.FechaRegistro;
                    M.FechaFisico = Manterior.FechaFisico;
                    M.Monto = Manterior.Monto;
                    M.MonedaID = Manterior.MonedaID;
                    M.MontoMonedaSecundaria = Manterior.MontoMonedaSecundaria;
                    M.TipoCambio = Manterior.TipoCambio;
                    M.Referencia = "ANULADO   " + Manterior.Numero.ToString();
                    db.Movimientos.InsertOnSubmit(M);

                    Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo,
                                "Se anuló la Depreciación: " + Manterior.Numero, this.Name);

                    #region <<< Registrando Detalle >>>
                    List<Entidad.Depreciacion> D = db.Depreciacions.Where(o => o.MovimientoID.Equals(ID)).ToList();
                    foreach (var obj in D)
                    {
                            Entidad.Bien B = db.Bien.SingleOrDefault(s => s.ID.Equals(obj.BienID));

                            if (B != null)
                            {
                                B.ValorDepreActual -= obj.ValorDepreciacion;
                                B.MesesDepreciados = (obj.NumeroDepreciacion.Equals(0) ? 0 : obj.NumeroDepreciacion - 1);

                                B.EsDepreciado = true;
                            }
                            db.SubmitChanges();
                       
                    }

                    db.Depreciacions.DeleteAllOnSubmit(db.Depreciacions.Where(o => o.MovimientoID.Equals(ID)));
                    db.SubmitChanges();
                    #endregion
                    
                    int l = Manterior.ComprobanteContables.Count;
                    Manterior.ComprobanteContables.OrderBy(o => o.Linea).ToList().ForEach(linea =>
                    {
                        Entidad.ComprobanteContable CD = new Entidad.ComprobanteContable();

                        CD.CuentaContableID = linea.CuentaContableID;
                        CD.Monto = linea.Monto * (-1);
                        CD.TipoCambio = linea.TipoCambio;
                        CD.MontoMonedaSecundaria = linea.MontoMonedaSecundaria * (-1);
                        CD.Fecha = linea.Fecha;
                        CD.Descripcion = linea.Descripcion;
                        CD.EstacionServicioID = linea.EstacionServicioID;
                        CD.SubEstacionID = linea.SubEstacionID;
                        CD.CentroCostoID = linea.CentroCostoID;
                        CD.Linea = l;
                        l--;

                        M.ComprobanteContables.Add(CD);
                        db.SubmitChanges();
                    });


                    db.SubmitChanges();
                    trans.Commit();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                }

                catch (Exception ex)
                {
                    trans.Rollback();
                    Parametros.General.splashScreenManagerMain.CloseWaitForm();
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
                finally
                {
                    if (db.Connection.State == ConnectionState.Open) db.Connection.Close();
                }
            }
        }

        #endregion
    }
}
