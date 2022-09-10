using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Administracion.Forms
{                                
    public partial class FormTipoCambio : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogTipoCambio nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();

        #endregion

        #region <<< INICIO >>>

        public FormTipoCambio()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnAnular.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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

                bdsManejadorDatos.DataSource = from P in db.TipoCambios
                                               join md in db.Monedas on P.MonedaPrimariaID equals md.ID
                                               join ms in db.Monedas on P.MonedaSecundariaID equals ms.ID
                                               select new
                                               {
                                                   P.ID,
                                                   MonedaPrimaria = md.Nombre,
                                                   MonedaSecundaria = ms.Nombre,
                                                   P.Fecha,
                                                   P.Valor
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
                    nf = new Forms.Dialogs.DialogTipoCambio();
                    DateTime fecha = Convert.ToDateTime(db.GetDateServer());
                    DateTime vFecha = Convert.ToDateTime(db.TipoCambios.OrderByDescending(o => o.Fecha).First().Fecha).AddDays(1);
                    nf.IDMes = vFecha.Month;
                    nf.Ano = vFecha.Year;
                                           
                    nf.Text = "Crear Tipos de Cambio";
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
                        nf = new Forms.Dialogs.DialogTipoCambio();
                        nf.Text = "Editar Tipos de Cambio";
                        nf.EntidadAnterior = db.TipoCambios.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
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
                      
        #endregion
                
    }
}
