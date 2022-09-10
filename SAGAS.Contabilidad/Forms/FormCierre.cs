using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Contabilidad.Forms
{                                
    public partial class FormCierre : Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Dialogs.DialogCierre nf;
        private Entidad.SAGASDataClassesDataContext db;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;

        #endregion

        #region <<< INICIO >>>

        public FormCierre()
        {
            InitializeComponent();
            this.btnAnular.Caption = "Reaperturar";
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
            this.btnModificar.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
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

                bdsManejadorDatos.DataSource = (from es in db.EstacionServicios
                                                where es.Activo && (db.GetViewEstacionesServicioByUsers.Any(ges => ges.UsuarioID == Parametros.General.UserID && ges.EstacionServicioID == es.ID))
                                                select new { ID = es.ID, es.Codigo, Nombre = es.Nombre, es.Direccion, Activo = es.Activo }).ToList();

                
                this.gridEstaciones.DataSource = bdsManejadorDatos;


            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
            }
        } 

        protected override void Imprimir()
        {
            this.PrintList(gridEstaciones);
        }

        protected override void ExportarExcel()
        {
            this.ExportListToExcel(gridEstaciones);
        }

        protected override void ExportarPDF()
        {
            this.ExportListToPDF(gridEstaciones);
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
            gvDataEstacionServicio_FocusedRowChanged(null, null);
        }

        protected override void CleanFilter()
        {
            this.gvDataEstacionServicio.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nf == null)
                {
                    if (gvDataEstacionServicio.FocusedRowHandle >= 0)
                    {
                    nf = new Forms.Dialogs.DialogCierre(Usuario, Convert.ToInt32(gvDataEstacionServicio.GetFocusedRowCellValue(colID)));
                    nf.Text = "Crear Cierre";
                    nf.Owner = this;
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

        protected override void Edit()
        {
            //try
            //{
            //    if (nf == null)
            //    {
            //        if (gvDataEstacionServicio.FocusedRowHandle >= 0)
            //        {
            //            nf = new Forms.Dialogs.DialogCierre(Usuario);
            //            nf.Text = "Editar Estación";
            //            nf.EntidadAnterior = db.PeriodoContables.Single(es => es.ID == Convert.ToInt32(gvDataEstacionServicio.GetFocusedRowCellValue(colID)));
            //            nf.Owner = this;
            //            nf.Editable = true;
            //            nf.MDI = this;
            //            nf.Show();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            //}
        }

        protected override void Del()
        {
            try
            {
                if (nf == null)
                {
                    if (gvDataEstacionServicio.FocusedRowHandle >= 0)
                    {

                        int id = Convert.ToInt32(gvDataEstacionServicio.GetFocusedRowCellValue(colID));

                        var obj = db.PeriodoContables.Where(c => c.EstacionID.Equals(id) && c.Cerrado).OrderByDescending(o => o.FechaFin).FirstOrDefault();

                        if (obj != null)
                        {
                            Entidad.PeriodoContable PC = db.PeriodoContables.Single(o => o.ID.Equals(obj.ID));

                            if (!PC.Consolidado)
                            {
                                if (db.Movimientos.Count(o => o.MovimientoTipoID.Equals(64) && o.FechaRegistro.Year.Equals(PC.FechaFin.Year) && o.EstacionServicioID.Equals(PC.EstacionID) && !o.Anulado) <= 0)
                                {
                                    if (Parametros.General.DialogMsg("Esta seguro de Reaperturar el Mes " + Parametros.General.GetMonthInLetters(PC.FechaFin.Month) + " de la Estación de Servicio: " + PC.NombreEstacion + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                                    {
                                        PC.Cerrado = false;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                    Parametros.General.DialogMsg("El Periodo Fiscal del año " + PC.FechaFin.Year + " ya esta CERRADO.", Parametros.MsgType.warning);
                            }
                            else
                                Parametros.General.DialogMsg("El Periodos contables esta Consolidado para este Mes", Parametros.MsgType.warning);
                        }
                        else
                            Parametros.General.DialogMsg("No Existen Periodos contables Abiertos para esta Estación de Servicio", Parametros.MsgType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }
        
        private void gvDataEstacionServicio_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvDataEstacionServicio.FocusedRowHandle >= 0)
            {
                try
                {
                    int ID = Convert.ToInt32(gvDataEstacionServicio.GetFocusedRowCellValue(colID));
                    
                    var obj = (from M in db.PeriodoContables
                               where M.EstacionID.Equals(ID)
                               select M).OrderBy(o => o.FechaInicio);

                    gridPeriodo.DataSource = obj;

                }
                catch (Exception ex)
                {
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
                }
            }
        }

        #endregion
    }
}
