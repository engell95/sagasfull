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
    public partial class FormTipoActivo : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogTipoActivo nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = false; // Parametros.Config.DisplayMessageDialog();

        #endregion


        public FormTipoActivo()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this); //Parametros.General.UserID
            this.btnAnular.Caption = "Inactivar";
            FillControl();
        }


        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            {
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());

                var cuentas = db.CuentaContables.Where(c => c.Activo && c.Detalle).Select(s => new { s.ID, Display = s.Codigo + " | " + s.Nombre });

                bdsManejadorDatos.DataSource = from A in db.TipoActivo
                                               select new
                                               {
                                                   A.ID,
                                                   A.Codigo,
                                                   A.Nombre,
                                                   A.Descripcion,
                                                   A.EsTangible,
                                                   A.EsDepreciable,
                                                   A.Activo,
                                                   CuentaActivo = (A.CuentaActivo > 0 ? cuentas.Where(c => c.ID.Equals(A.CuentaActivo)).First().Display : "< N/A >"),
                                                   CuentaGasto = (A.CuentaGasto > 0 ? cuentas.Where(c => c.ID.Equals(A.CuentaGasto)).First().Display : "< N/A >"),
                                                   CuentaDepreciacionAcumulada = (A.CuentaDepreciacionAcumulada > 0 ? cuentas.Where(c => c.ID.Equals(A.CuentaDepreciacionAcumulada)).First().Display : "< N/A >"),                                                   
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
                    nf = new Forms.Dialogs.DialogTipoActivo();
                    nf.Text = "Crear Tipo de Activo";
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
                        nf = new Forms.Dialogs.DialogTipoActivo();
                        nf.Text = "Editar Tipo de Activo";
                        nf.EntidadAnterior = db.TipoActivo.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                        var A = db.TipoActivo.Single(c => c.ID == id);

                        if (gvData.GetFocusedRowCellValue(colActivo).Equals(true))
                        {
                            if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                            {
                                //Desactivar Registro 
                                A.Activo = false;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se Inactivó el Tipo de Activo: " + A.Nombre, this.Name);

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
                                A.Activo = true;
                                db.SubmitChanges();

                                Parametros.General.AddLogBook(db, Parametros.TipoAccion.ActivoFijo, "Se activó el Tipo de Activo: " + A.Nombre, this.Name);

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

        private void gvData_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvData.FocusedRowHandle >= 0)
                Parametros.General.CambiarActivogvData(this, gvData, colActivo.FieldName);
        }
    }
}
