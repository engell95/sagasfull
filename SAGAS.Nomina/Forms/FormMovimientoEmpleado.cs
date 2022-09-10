using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAGAS.Nomina.Forms
{                                
    public partial class FormMovimientoEmpleado : SAGAS.Parametros.Forms.FormBase
    {
        #region <<< DECLARACIONES >>>

        private Forms.Wizards.wizMovimiento nfz;
        private Forms.Dialogs.DialogMovimientoEmpleado nfe;
        private Entidad.SAGASDataClassesDataContext db;
        private Entidad.SAGASDataViewsDataContext dv;
        private bool ShowMsgDialog = Parametros.Config.DisplayMessageDialog();
        private int Usuario = Parametros.General.UserID;
        
        #endregion

        #region <<< INICIO >>>
        
        public FormMovimientoEmpleado()
        {
            InitializeComponent();
            Parametros.General.SetFormAcciones(Parametros.General.UserID, this.barManager, this);
        }

        private void FormSucursal_Load(object sender, EventArgs e)
        {
            this.FillControl();
            this.btnAnular.Caption = "Anular";
        }

        #endregion

        #region <<< METODOS >>>

        protected override void FillControl()
        {
            try
            { 
                dv = new Entidad.SAGASDataViewsDataContext(Parametros.Config.GetCadenaConexionString());
                db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
               
                bdsManejadorDatos.DataSource = from me in dv.VistaMovimientoEmpleado
                                               select me;

                this.grid.DataSource = bdsManejadorDatos;

                //**Zonas
                
                /*chkAplicaRetencion.f = from p in db.Planillas
                                         where p.Activo
                                        select new { p.ID, p.FechaFin };

                chkAplicaRetencion.DisplayMember = "FechaFin";
                chkAplicaRetencion.ValueMember = "ID";*/

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.Message);
                Parametros.General.AddLogBook(db, Parametros.TipoAccion.Error, ex.Message, this.Name);
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

        internal void CleanDialog(bool ShowMSG, bool RefreshMDI, bool _Editing)
        {
            if(!_Editing)
                nfz = null;
            else if(_Editing)
                nfe = null;

            if (ShowMSG)
            {
                if (ShowMsgDialog)
                    Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                else
                    this.timerMSG.Start();
            }

            if(RefreshMDI)
                FillControl();

            this.Activate();
        }

        protected override void CleanFilter()
        {
            this.gvData.ActiveFilter.Clear();
        }

        protected override void Add()
        {
            try
            {
                if (nfz == null)
                {
                    nfz = new Forms.Wizards.wizMovimiento();
                    nfz.Text = "Crear Movimiento Empleado";
                    nfz.Owner = this.Owner;
                    nfz.MdiParent = this.MdiParent;
                    nfz.MDI = this;
                    nfz.Show();
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
                if (nfe == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        db = new Entidad.SAGASDataClassesDataContext(Parametros.Config.GetCadenaConexionString());
                        nfe = new Forms.Dialogs.DialogMovimientoEmpleado(Usuario);
                        nfe.EtAnterior = db.MovimientoEmpleado.Single(e => e.ID == Convert.ToInt32(gvData.GetFocusedRowCellValue(colID)));
                        nfe.Text = "Editar Movimiento Empleado";
                        nfe.Owner = this;
                        nfe.Editable = true;
                        nfe.MDI = this;
                        nfe.Show();
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
                if (nfe == null)
                {
                    if (gvData.FocusedRowHandle >= 0)
                    {
                        int id = Convert.ToInt32(gvData.GetFocusedRowCellValue(colID));

                        if (Parametros.General.DialogMsg(Parametros.Properties.Resources.SEGUROINACTIVAR + Environment.NewLine, Parametros.MsgType.question) == DialogResult.OK)
                        {

                            Entidad.MovimientoEmpleado ME = db.MovimientoEmpleado.Single(me => me.ID == id);
                            Entidad.DetalleMovimientoPlanilla DMP;

                            var lista_eliminar = (from l in db.DetalleMovimientoPlanilla
                                                  where l.MovimientoEmpleadoID.Equals(id)
                                                  select new {id = l.ID}).ToList();

                            foreach (var row in lista_eliminar.Select(d => d.id))
                            {
                                DMP = db.DetalleMovimientoPlanilla.Single(d => d.ID == row);
                                db.DetalleMovimientoPlanilla.DeleteOnSubmit(DMP);
                            }
                            db.MovimientoEmpleado.DeleteOnSubmit(ME);
                            db.SubmitChanges();

                            Parametros.General.AddLogBook(db, Parametros.TipoAccion.Nomina, "Se anuló el Movimiento: " + gvData.GetRowCellValue(id,"TipoMovimientoEmpleadoNombre") + " al Empleado: " + gvData.GetRowCellValue(id, "TipoMovimientoEmpleadoNombre"), this.Name);

                            if (ShowMsgDialog)
                                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGCAMBIOSGUARDADOS, Parametros.MsgType.message);
                            else
                                this.timerMSG.Start();

                            FillControl();
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
        

    }
}
