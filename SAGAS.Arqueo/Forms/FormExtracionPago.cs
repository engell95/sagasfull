using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Arqueo.Forms
{                                
    public partial class FormExtracionPago : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogExtracionPago nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private Parametros.ListTipoComprobanteArqueo listadoTipo = new Parametros.ListTipoComprobanteArqueo(); 

        #endregion

        #region <<< INICIO >>>

        public FormExtracionPago()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Inactivar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
        }

        private void FormZona_Load(object sender, EventArgs e)
        {
            this.FillControl();
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                bdsManejadorDatos.DataSource = (from ep in db.ExtracionPagos
                                               select ep).OrderBy(o => o.EsPago);

                this.grid.DataSource = bdsManejadorDatos;
                var ListaCC = (from cc in db.CuentaContables where cc.Activo && cc.Detalle select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lkTipo.DataSource = listadoTipo.GetListTipoComprobanteArqueo();
                rpCC.DataSource = ListaCC;
                rpDeudor.DataSource = (from cc in db.Clientes where cc.Activo select new { cc.ID, cc.Codigo, cc.Nombre, Display = cc.Codigo + " | " + cc.Nombre }).OrderBy(o => o.Display).ToList();
                lkCuentaDet.DataSource = ListaCC;
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(grid);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(grid);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(grid);
        }

        internal void CleanDialog(bool ShowMSG)
        {
            nf = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

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
                    nf = new Forms.Dialogs.DialogExtracionPago();
                    nf.Text = "Crear Tipo de Extración / Pago";
                    nf.Owner = this;
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
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        nf = new Forms.Dialogs.DialogExtracionPago();
                        nf.Text = "Editar Tipo de Extración / Pago";
                        nf.EntidadAnterior = db.ExtracionPagos.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
            }
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));
                        var tp = db.ExtracionPagos.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                if (Convert.ToInt32(db.ArqueoProductoExtracions.Count(c => c.ExtracionID.Equals(tp.ID))) > 0)
                                {
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGNOPUEDEINACTIVAR + Environment.NewLine, Parametros.MsgType.warning);
                                    return;
                                }

                                //Desactivar Registro 
                                tp.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se Inactivó el Tipo de Extración / Pago: " + tp.Nombre, this.Name);
                                this.btnAnular.Caption = "Activar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                        else
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                //Activar Registro 
                                tp.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Arqueo, "Se activó el Tipo de Extración / Pago: " + tp.Nombre, this.Name);
                                this.btnAnular.Caption = "Inactivar";

                                if (ShowMsgDialog)
                                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                                else
                                    this.timerMSG.Start();

                                FillControl();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
              
        #endregion

        private void gvData_FocusedRowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
        }

        
    }
}
